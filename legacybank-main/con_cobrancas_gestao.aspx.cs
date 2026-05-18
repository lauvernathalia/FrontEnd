using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Collections.Specialized;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Security.Cryptography;
using System.Data.SqlClient;
using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Security.Cryptography;
using System.Data.SqlClient;
using System.Text;
using System.IO;
using System.Net;
using System.Net.Mail;
using System.IO.IsolatedStorage;
using System.Runtime.Serialization;

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using System.IO;
using System.Text;

using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;

using System.ComponentModel;
using System.Security;
using System.Dynamic;
using System.Web.Script.Serialization;
using System.Xml;
using System.Xml.Linq;
using System.Xml.XPath;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Converters;

using MessagingToolkit.QRCode.Codec;
using MessagingToolkit.QRCode.Codec.Data;
using System.Drawing;


public partial class con_cobrancas_gestao : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (HttpContext.Current.Session.Count <= 0)
        {
            Response.Redirect("login.aspx");
            FormsAuthentication.SignOut();
        }

        if (!IsPostBack)
        {
            DateTime data = DateTime.Today;
            DateTime primeiroDiaDoMes = new DateTime(data.Year, data.Month, 1);
            DateTime ultimoDiaDoMes = new DateTime(data.Year, data.Month, DateTime.DaysInMonth(data.Year, data.Month));

            txtDataIni.Text = primeiroDiaDoMes.ToShortDateString();
            txtDataFim.Text = DateTime.Now.ToShortDateString();

            AdquirentesService();
            ConsultaGeral();
        }
    }

    private void AdquirentesService()
    {
        using (SqlConnection connection = new SqlConnection(Funcoes.conexao()))
        {
            connection.Open();
            using (SqlCommand cmd = new SqlCommand("dbo.stp_pessoas_fj_integracoes_ins", connection))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@flg_operacao", SqlDbType.VarChar).Value = "F";
                cmd.Parameters.Add("@COD_ID_PESSOA_LICENCIADO", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["LICENCIADO"].ToString());
                cmd.Parameters.Add("@COD_ID_PESSOAS_FJ", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["PESSOA"].ToString());
                cmd.Parameters.Add("@FLG_ACESSO", SqlDbType.Char).Value = "S";

                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                DataTable dtAdquirentes = new DataTable();
                adapter.Fill(dtAdquirentes);

                ddlAdquirentes.DataTextField = "NOM_FANTASIA_INTEGRACAO";
                ddlAdquirentes.DataValueField = "FLG_INTEGRACAO";
                ddlAdquirentes.DataSource = dtAdquirentes;
                ddlAdquirentes.DataBind();
            }
        }
    }    
    
    protected void btnPesquisar_Click(object sender, EventArgs e)
    {
        ConsultaGeral();
    }

    private void ConsultaGeral()
    {
        //dtgConsulta.Visible = true;
        SqlConnection myConsulta = new SqlConnection(Funcoes.conexao());
        myConsulta.Open();
        SqlDataAdapter SDAConsulta = new SqlDataAdapter("dbo.stp_vendas_ins", myConsulta);
        SDAConsulta.SelectCommand.CommandType = CommandType.StoredProcedure;
        SDAConsulta.SelectCommand.Parameters.Add("@flg_operacao", SqlDbType.Char).Value = "C";

        SDAConsulta.SelectCommand.Parameters.Add("@COD_ID_PESSOA_LICENCIADO", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["LICENCIADO"].ToString());
        SDAConsulta.SelectCommand.Parameters.Add("@COD_ID_PESSOAS_FJ_VENDEDOR", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["PESSOA"].ToString());
        
        SDAConsulta.SelectCommand.Parameters.Add("@DTA_INICIO", SqlDbType.DateTime).Value = Convert.ToDateTime(txtDataIni.Text.ToString());
        SDAConsulta.SelectCommand.Parameters.Add("@DTA_FIM", SqlDbType.DateTime).Value = Convert.ToDateTime(txtDataFim.Text.ToString());
        
        SDAConsulta.SelectCommand.Parameters.Add("@FLG_INTEGRACAO", SqlDbType.Char).Value = ddlAdquirentes.SelectedValue.ToString();
        SDAConsulta.SelectCommand.Parameters.Add("@FLG_TIPO", SqlDbType.Char).Value = ddlTipo.SelectedValue.ToString();
        SDAConsulta.SelectCommand.Parameters.Add("@FLG_STATUS", SqlDbType.Char).Value = ddlStatus.SelectedValue.ToString();
        SDAConsulta.SelectCommand.Parameters.Add("@NOM_DOCUMENTO", SqlDbType.VarChar).Value = "";
        SDAConsulta.SelectCommand.Parameters.Add("@NOM_NOME", SqlDbType.VarChar).Value = "";


        DataSet dsConsulta = new DataSet();
        SDAConsulta.Fill(dsConsulta, "VENDAS");
        rptConsulta.DataSource = dsConsulta.Tables["VENDAS"].DefaultView;
        rptConsulta.DataBind();
        myConsulta.Close(); myConsulta.Dispose();


    }

    public string GetPostBackScript()
    {
        PostBackOptions options = new PostBackOptions(btnPostback);
        Page.ClientScript.RegisterForEventValidation(options);

        return Page.ClientScript.GetPostBackEventReference(options);
    }

    protected void btnPostBack_Click(object sender, EventArgs e)
    {
        ConsultaGeral();
    }

    protected void rptConsulta_OnItemCommand(object source, System.Web.UI.WebControls.RepeaterCommandEventArgs e)
    {
        if (e.CommandName == "Cancelar")
        {
            SqlConnection connInsCons = new SqlConnection(Funcoes.conexao());
            SqlCommand cmdInsCons = new SqlCommand("dbo.stp_vendas_ins", connInsCons);
            cmdInsCons.CommandType = CommandType.StoredProcedure;
            cmdInsCons.Parameters.Add("@flg_operacao", SqlDbType.Char).Value = 'N';
            cmdInsCons.Parameters.Add("@COD_ID", SqlDbType.Int).Value = Funcoes.strToInt(Convert.ToString(e.CommandArgument));
            cmdInsCons.Parameters.Add("@COD_ID_PESSOA_LICENCIADO", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["LICENCIADO"].ToString());
            connInsCons.Open();
            cmdInsCons.ExecuteNonQuery();

            ClientScript.RegisterStartupScript(this.GetType(),
            "CancelamentoAjusteFinanceiro", "alert('Registro Cancelado com sucesso');", true);

            ConsultaGeral();
        }
    }
    protected void rptConsulta_ItemDataBound(object sender, System.Web.UI.WebControls.RepeaterItemEventArgs e)
    {
        ((HtmlAnchor)e.Item.FindControl("hrefCheckout")).Visible = false;

        string tTipo = ((TextBox)e.Item.FindControl("txttipo")).Text.ToString();
        //https://<%# DataBinder.Eval(Container.DataItem, "NOM_ORIGEM")%><%# Funcoes.Encrypt(DataBinder.Eval(Container.DataItem, "COD_ID").ToString())%>

        if (tTipo.ToString().Trim() == "L")
        {
            ((HtmlAnchor)e.Item.FindControl("hrefCheckout")).Visible = true;
            ((HtmlAnchor)e.Item.FindControl("hrefCheckout")).HRef = "https://" + ((TextBox)e.Item.FindControl("txtorigem")).Text.ToString() + Funcoes.Encrypt(((TextBox)e.Item.FindControl("txtid")).Text.ToString());
        }
    }
}