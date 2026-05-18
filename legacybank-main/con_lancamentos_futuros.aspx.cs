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


public partial class con_lancamentos_futuros : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        // Verifica se o usuário esta autenticado
        if (HttpContext.Current.Session.Count <= 0)
        {
            Response.Redirect("login.aspx");
            FormsAuthentication.SignOut();
        }

        if (!IsPostBack)
        {

            SqlConnection myEstabelecimentos = new SqlConnection(Funcoes.conexao());
            myEstabelecimentos.Open();
            SqlCommand cmdEstabelecimentos = new SqlCommand("dbo.stp_pessoas_fj_ins", myEstabelecimentos);
            cmdEstabelecimentos.CommandType = CommandType.StoredProcedure;
            cmdEstabelecimentos.Parameters.Add("@flg_operacao", SqlDbType.VarChar).Value = "Y";

            cmdEstabelecimentos.Parameters.Add("@COD_ID_PESSOA_LICENCIADO", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["LICENCIADO"].ToString());
            cmdEstabelecimentos.Parameters.Add("@NOM_RAZAOSOCIAL", SqlDbType.VarChar).Value = " ";
            cmdEstabelecimentos.Parameters.Add("@NUM_DOCUMENTO", SqlDbType.VarChar).Value = " ";
            cmdEstabelecimentos.Parameters.Add("@NUM_TOKEN", SqlDbType.VarChar).Value = " ";
            cmdEstabelecimentos.Parameters.Add("@FLG_ATIVO", SqlDbType.Char).Value = " ";
            cmdEstabelecimentos.Parameters.Add("@FLG_TIPO", SqlDbType.Char).Value = "E";

            if (HttpContext.Current.Session["TIPO"].ToString() == "M")
            {
                cmdEstabelecimentos.Parameters.Add("@COD_ID_MARKETPLACE", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["PESSOA"].ToString());
            }
            if (HttpContext.Current.Session["TIPO"].ToString() == "R")
            {
                cmdEstabelecimentos.Parameters.Add("@COD_ID_REPRESENTANTE", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["PESSOA"].ToString());
            }


            SqlDataAdapter drEstabelecimentos = new SqlDataAdapter();
            drEstabelecimentos.SelectCommand = cmdEstabelecimentos;
            DataSet dsEstabelecimentos = new DataSet();
            drEstabelecimentos.Fill(dsEstabelecimentos, "PESSOAS_FJ");
            ddlEstabelecimento.DataTextField = "NOM_RAZAOSOCIAL";
            ddlEstabelecimento.DataValueField = "COD_ID";
            ddlEstabelecimento.DataSource = dsEstabelecimentos.Tables["PESSOAS_FJ"].DefaultView;
            ddlEstabelecimento.DataBind();
            ddlEstabelecimento.Items.Insert(0, new ListItem("Todos", ""));
            myEstabelecimentos.Close();
            myEstabelecimentos.Dispose();

            if (HttpContext.Current.Session["TIPO"].ToString() == "E")
            {
                ddlEstabelecimento.SelectedValue = HttpContext.Current.Session["PESSOA"].ToString();
                ddlEstabelecimento.Enabled = false;
            }

            ConsultaGeral();

        }

    }
    protected void btnPesquisar_Click(object sender, EventArgs e)
    {
        ConsultaGeral();
    }

    private void ConsultaGeral()
    {
        SqlConnection myConsulta = new SqlConnection(Funcoes.conexao());
        myConsulta.Open();
        SqlDataAdapter SDAConsulta = new SqlDataAdapter("dbo.stp_extrato_ins", myConsulta);
        SDAConsulta.SelectCommand.CommandType = CommandType.StoredProcedure;
        SDAConsulta.SelectCommand.Parameters.Add("@flg_operacao", SqlDbType.Char).Value = "F";
        SDAConsulta.SelectCommand.Parameters.Add("@COD_ID_PESSOA_LICENCIADO", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["LICENCIADO"].ToString());
        SDAConsulta.SelectCommand.Parameters.Add("@COD_ID_PESSOAS_FJ", SqlDbType.Int).Value = Funcoes.strToInt(ddlEstabelecimento.SelectedValue.ToString());
        SDAConsulta.SelectCommand.Parameters.Add("@DTA_INICIO", SqlDbType.DateTime).Value = Convert.ToDateTime(DateTime.Now.ToShortDateString());
        SDAConsulta.SelectCommand.Parameters.Add("@FLG_STATUS", SqlDbType.VarChar).Value = "pending";
        SDAConsulta.SelectCommand.Parameters.Add("@FLG_RECURSO", SqlDbType.VarChar).Value = "receivable";

        DataSet dsConsulta = new DataSet();
        SDAConsulta.Fill(dsConsulta, "EXTRATO");
        rptConsulta.DataSource = dsConsulta.Tables["EXTRATO"].DefaultView;
        rptConsulta.DataBind();
        myConsulta.Close(); myConsulta.Dispose();

    }

    protected void btnExportar_Click(object sender, EventArgs e)
    {

    }
    public string GetPostBackScript()
    {
        PostBackOptions options = new PostBackOptions(btnPostback);
        Page.ClientScript.RegisterForEventValidation(options);

        return Page.ClientScript.GetPostBackEventReference(options);
    }

    protected void btnPostBack_Click(object sender, EventArgs e)
    {

    }

}