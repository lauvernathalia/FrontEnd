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
using System.Data.SqlClient;


using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

using System.IO;
using System.Text;

using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;

using System.ComponentModel;

using System.Security.Cryptography;
using System.Net.Mail;
using System.IO.IsolatedStorage;


public partial class cad_estabelecimentos_api : System.Web.UI.Page
{
    public string sid_id
    {
        get
        {
            try { return Request["id"]; }
            catch { return ""; }
        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        // Verifica se o usuário esta autenticado

        // Verifica se o usuário esta autenticado
        if (HttpContext.Current.Session.Count <= 0)
        {
            Response.Redirect("login.aspx");
            FormsAuthentication.SignOut();
        }

        if (!IsPostBack)
        {
            ConsultaFicha();
            ConsultaGeral();
        }

    }

    private void ConsultaFicha()
    {
        SqlConnection mySelCadastro = new SqlConnection(Funcoes.conexao());
        mySelCadastro.Open();
        SqlCommand cmdSelCadastro = new SqlCommand("dbo.stp_pessoas_fj_ins", mySelCadastro);
        cmdSelCadastro.CommandType = CommandType.StoredProcedure;
        cmdSelCadastro.Parameters.Add("@flg_operacao", SqlDbType.Char).Value = "S";
        cmdSelCadastro.Parameters.Add("@COD_ID", SqlDbType.Int).Value = Funcoes.strToInt(sid_id.ToString());
        cmdSelCadastro.Parameters.Add("@COD_ID_PESSOA_LICENCIADO", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["LICENCIADO"].ToString());
        SqlDataReader ReaderCadastro = cmdSelCadastro.ExecuteReader();
        while (ReaderCadastro.Read())
        {
            txtID.Text = ReaderCadastro["COD_ID"].ToString();
            txtRazaosocial.Text = ReaderCadastro["NOM_RAZAOSOCIAL"].ToString();
        }

    }


    private void ConsultaGeral()
    {
        SqlConnection myConsulta = new SqlConnection(Funcoes.conexao());
        myConsulta.Open();
        SqlDataAdapter SDAConsulta = new SqlDataAdapter("dbo.stp_pessoas_fj_tokens_ins", myConsulta);
        SDAConsulta.SelectCommand.CommandType = CommandType.StoredProcedure;
        SDAConsulta.SelectCommand.Parameters.Add("@flg_operacao", SqlDbType.Char).Value = "C";
        SDAConsulta.SelectCommand.Parameters.Add("@COD_ID_PESSOAS_FJ", SqlDbType.Int).Value = Funcoes.strToInt(sid_id.ToString());
        DataSet dsConsulta = new DataSet();
        SDAConsulta.Fill(dsConsulta, "PESSOAS_FJ_TOKENS");

        rptConsultaTokens.DataSource = dsConsulta.Tables["PESSOAS_FJ_TOKENS"].DefaultView;
        rptConsultaTokens.DataBind();

        myConsulta.Close(); myConsulta.Dispose();
    }

    protected void btnIncluir_Click(object sender, EventArgs e)
    {


            SqlConnection connInsCons = new SqlConnection(Funcoes.conexao());
            connInsCons.Open();
            SqlCommand cmdInsCons = new SqlCommand("dbo.stp_pessoas_fj_tokens_ins", connInsCons);
            cmdInsCons.CommandType = CommandType.StoredProcedure;
            cmdInsCons.Parameters.Add("@flg_operacao", SqlDbType.Char).Value = 'I';
            cmdInsCons.Parameters.Add("@COD_ID_PESSOAS_FJ", SqlDbType.Int).Value = Funcoes.strToInt(sid_id.ToString());
            // Gerar token 64 caracteres 

            cmdInsCons.Parameters.Add("@NOM_TOKEN_API", SqlDbType.VarChar).Value = Funcoes.alfanumericoAleatorio(64);
        
            cmdInsCons.Parameters.Add("@FLAG_ATIVO", SqlDbType.NChar).Value = "S";
            cmdInsCons.Parameters.Add("@FLAGS_API", SqlDbType.NVarChar).Value = "[\"zoop\"]";
            cmdInsCons.Parameters.Add("@DTA_CRIACAO", SqlDbType.DateTime).Value = DateTime.Now;
            cmdInsCons.Parameters.Add("@DTA_ATUALIZACAO", SqlDbType.DateTime).Value = DateTime.Now;

            cmdInsCons.ExecuteNonQuery();
            connInsCons.Close();
            connInsCons.Dispose();

            ClientScript.RegisterStartupScript(this.GetType(),
                "Alerta", "alert('Dados gravados com sucesso'); ", true);

            ConsultaGeral();
    }


    protected void btnCancelar_Click(object sender, EventArgs e)
    {
        ClientScript.RegisterStartupScript(this.GetType(),
            "Alerta", "opener.PostBackOnMainPage(); window.close(); ", true);
    }
    protected void rptConsultaTokens_ItemCommand(object source, RepeaterCommandEventArgs e)
    {
        if (e.CommandName == "Excluir")
        {
            SqlConnection connInsCons = new SqlConnection(Funcoes.conexao());
            SqlCommand cmdInsCons = new SqlCommand("dbo.stp_pessoas_fj_tokens_ins", connInsCons);
            cmdInsCons.CommandType = CommandType.StoredProcedure;
            cmdInsCons.Parameters.Add("@flg_operacao", SqlDbType.Char).Value = 'E';
            cmdInsCons.Parameters.Add("@COD_ID", SqlDbType.Int).Value = Funcoes.strToInt(Convert.ToString(e.CommandArgument));
            cmdInsCons.Parameters.Add("@COD_ID_PESSOAS_FJ", SqlDbType.Int).Value = Funcoes.strToInt(sid_id.ToString());
            connInsCons.Open();
            cmdInsCons.ExecuteNonQuery();

            ClientScript.RegisterStartupScript(this.GetType(),
            "Usuarios", "alert('Registro excluído com sucesso');", true);

            ConsultaGeral();
        }

        if (e.CommandName == "Ativo")
        {
            SqlConnection connInsCons = new SqlConnection(Funcoes.conexao());
            SqlCommand cmdInsCons = new SqlCommand("dbo.stp_pessoas_fj_tokens_ins", connInsCons);
            cmdInsCons.CommandType = CommandType.StoredProcedure;
            cmdInsCons.Parameters.Add("@flg_operacao", SqlDbType.Char).Value = 'T';
            cmdInsCons.Parameters.Add("@COD_ID", SqlDbType.Int).Value = Funcoes.strToInt(Convert.ToString(e.CommandArgument));
            cmdInsCons.Parameters.Add("@COD_ID_PESSOAS_FJ", SqlDbType.Int).Value = Funcoes.strToInt(sid_id.ToString());
            connInsCons.Open();
            cmdInsCons.ExecuteNonQuery();

            ClientScript.RegisterStartupScript(this.GetType(),
            "Usuarios", "alert('Usuário Ativado/Inativado com sucesso');", true);

            ConsultaGeral();
        }


    }
}