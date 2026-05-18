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


public partial class con_suporte_geral : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (HttpContext.Current.Session.Count <= 0)
        {
            FormsAuthentication.SignOut();
            Response.Redirect("login.aspx");
        }


        if (!IsPostBack)
        {
            DateTime data = DateTime.Today;
            DateTime primeiroDiaDoMes = new DateTime(data.Year, data.Month, 1);
            DateTime ultimoDiaDoMes = new DateTime(data.Year, data.Month, DateTime.DaysInMonth(data.Year, data.Month));

            txtDataIni.Text = DateTime.Now.ToShortDateString();
            txtDataFim.Text = DateTime.Now.ToShortDateString();

            ConsultaGeral();
        }
    }

    private void ConsultaGeral()
    {
        //dtgConsulta.Visible = true;
        SqlConnection myConsulta = new SqlConnection(Funcoes.conexao());
        myConsulta.Open();
        SqlDataAdapter SDAConsulta = new SqlDataAdapter("dbo.stp_suporte_ins", myConsulta);
        SDAConsulta.SelectCommand.CommandType = CommandType.StoredProcedure;
        SDAConsulta.SelectCommand.Parameters.Add("@flg_operacao", SqlDbType.Char).Value = "C";
        SDAConsulta.SelectCommand.Parameters.Add("@COD_ID_PESSOA_LICENCIADO", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["LICENCIADO"].ToString());

        if (HttpContext.Current.Session["TIPO"].ToString() == "E")
        {
            SDAConsulta.SelectCommand.Parameters.Add("@COD_ID_PESSOAS_FJ", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["PESSOA"].ToString());
        }

        if (HttpContext.Current.Session["TIPO"].ToString() == "M")
        {
            SDAConsulta.SelectCommand.Parameters.Add("@COD_ID_MARKETPLACE", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["PESSOA"].ToString());
        }
        if (HttpContext.Current.Session["TIPO"].ToString() == "R")
        {
            SDAConsulta.SelectCommand.Parameters.Add("@COD_ID_REPRESENTANTE", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["PESSOA"].ToString());
        }

        SDAConsulta.SelectCommand.Parameters.Add("@NOM_SUPORTE", SqlDbType.VarChar).Value = txtDescricao.Text.ToString();
        SDAConsulta.SelectCommand.Parameters.Add("@FLG_STATUS", SqlDbType.Char).Value = ddlStatus.SelectedValue.ToString();

        SDAConsulta.SelectCommand.Parameters.Add("@DTA_INICIO", SqlDbType.DateTime).Value = Convert.ToDateTime(txtDataIni.Text.ToString());
        SDAConsulta.SelectCommand.Parameters.Add("@DTA_FIM", SqlDbType.DateTime).Value = Convert.ToDateTime(txtDataFim.Text.ToString());

        DataSet dsConsulta = new DataSet();
        SDAConsulta.Fill(dsConsulta, "SUPORTE");
        rptConsulta.DataSource = dsConsulta.Tables["SUPORTE"].DefaultView;
        rptConsulta.DataBind();
        myConsulta.Close(); myConsulta.Dispose();


        foreach (RepeaterItem itemE in rptConsulta.Items)
        {
            if (((TextBox)itemE.FindControl("txtStatus")).Text.ToString().Trim() == "C")
            {
                ((LinkButton)itemE.FindControl("lbkCancelar")).Visible = false;
                ((LinkButton)itemE.FindControl("lbkReabrir")).Visible = true;
            }
            else
            {
                ((LinkButton)itemE.FindControl("lbkCancelar")).Visible = true;
                ((LinkButton)itemE.FindControl("lbkReabrir")).Visible = false;
            }
        }
    }
    protected void btnPesquisar_Click(object sender, EventArgs e)
    {
        ConsultaGeral();
    }
    protected void btnNovo_Click(object sender, EventArgs e)
    {
        ClientScript.RegisterStartupScript(this.GetType(),
    "Perfis", "openPopupWindow('cad_suporte.aspx?id=0','SuporteEdicao',1024,800);", true);

    }
    protected void rptConsulta_OnItemCommand(object source, RepeaterCommandEventArgs e)
    {
        if (e.CommandName == "Cancelar")
        {
            SqlConnection connInsCons = new SqlConnection(Funcoes.conexao());
            connInsCons.Open();
            SqlCommand cmdInsCons = new SqlCommand("dbo.stp_suporte_ins", connInsCons);
            cmdInsCons.CommandType = CommandType.StoredProcedure;
            cmdInsCons.Parameters.Add("@flg_operacao", SqlDbType.Char).Value = "X";
            cmdInsCons.Parameters.Add("@COD_ID", SqlDbType.Int).Value = Funcoes.strToInt(Convert.ToString(e.CommandArgument));
            cmdInsCons.Parameters.Add("@COD_ID_PESSOA_LICENCIADO", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["LICENCIADO"].ToString());
            cmdInsCons.ExecuteNonQuery();
            connInsCons.Close();
            connInsCons.Dispose();

            Funcoes.GravaAuditoria(Funcoes.strToInt(Convert.ToString(e.CommandArgument)), Request.ServerVariables["REMOTE_ADDR"].ToString(), "Consulta - Suporte - Cancelar", "ID: " + Funcoes.strToInt(Convert.ToString(e.CommandArgument)));


            ClientScript.RegisterStartupScript(this.GetType(),
                "Alerta", "alert('Registro Cancelado com sucesso!');", true);
            ConsultaGeral();

        }

        if (e.CommandName == "Reabrir")
        {
            SqlConnection connInsCons = new SqlConnection(Funcoes.conexao());
            connInsCons.Open();
            SqlCommand cmdInsCons = new SqlCommand("dbo.stp_suporte_ins", connInsCons);
            cmdInsCons.CommandType = CommandType.StoredProcedure;
            cmdInsCons.Parameters.Add("@flg_operacao", SqlDbType.Char).Value = "R";
            cmdInsCons.Parameters.Add("@COD_ID", SqlDbType.Int).Value = Funcoes.strToInt(Convert.ToString(e.CommandArgument));
            cmdInsCons.Parameters.Add("@COD_ID_PESSOA_LICENCIADO", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["LICENCIADO"].ToString());
            cmdInsCons.ExecuteNonQuery();
            connInsCons.Close();
            connInsCons.Dispose();

            Funcoes.GravaAuditoria(Funcoes.strToInt(Convert.ToString(e.CommandArgument)), Request.ServerVariables["REMOTE_ADDR"].ToString(), "Consulta - Suporte - Reabrir", "ID: " + Funcoes.strToInt(Convert.ToString(e.CommandArgument)));

            ClientScript.RegisterStartupScript(this.GetType(),
                "Alerta", "alert('Registro Reaberto com sucesso!');", true);
            ConsultaGeral();

        }


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
}