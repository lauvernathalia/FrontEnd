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



public partial class cad_representantes_converter : System.Web.UI.Page
{
    public string sid_id
    {
        get
        {
            try { return Funcoes.Decrypt(Request["id"]); }
            catch { return ""; }
        }
    }
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
            // Verificar Integrações


            if (Funcoes.strToInt(sid_id) != 0)
            {
                ConsultaFicha();
                Funcoes.GravaAuditoria(Funcoes.strToInt(sid_id), Request.ServerVariables["REMOTE_ADDR"].ToString(), "Acesso", "Converter representante para marketplace");
            }

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
            txtEmail.Text = ReaderCadastro["NOM_EMAIL"].ToString();
            txtTipo.Text = ReaderCadastro["FLG_TIPO_PESSOA"].ToString();
            if (ReaderCadastro["FLG_TIPO_PESSOA"].ToString().Trim() == "PF")
            {
                txtDocumento.Text = ReaderCadastro["NOM_CPF"].ToString();
            }
            else
            {
                txtDocumento.Text = ReaderCadastro["NOM_CNPJ"].ToString();

            }



        }

    }
    protected void btnFechar_Click(object sender, EventArgs e)
    {
        ClientScript.RegisterStartupScript(this.GetType(),
    "FecharJanela", "opener.PostBackOnMainPage(); window.close(); ", true);

    }
    protected void btnConverter_Click(object sender, EventArgs e)
    {
        if (txtSenha.Text.ToString().Trim() != "")
        {
            if (Funcoes.Encrypt(txtSenha.Text.ToString()) == HttpContext.Current.Session["SENHA"].ToString())
            {
                // Realiza a conversão de dados do representante para marketplace

                Funcoes.GravaAuditoria(Funcoes.strToInt(sid_id), Request.ServerVariables["REMOTE_ADDR"].ToString(), "Execução", "Converter representante para marketplace");


                SqlConnection connInsConsConverte = new SqlConnection(Funcoes.conexao());
                connInsConsConverte.Open();
                SqlCommand cmdInsConsConverte = new SqlCommand("dbo.stp_pessoas_fj_ins", connInsConsConverte);
                cmdInsConsConverte.CommandType = CommandType.StoredProcedure;
                cmdInsConsConverte.Parameters.Add("@flg_operacao", SqlDbType.Char).Value = '@';
                cmdInsConsConverte.Parameters.Add("@COD_ID", SqlDbType.Int).Value = Funcoes.strToInt(sid_id.ToString());
                cmdInsConsConverte.Parameters.Add("@COD_ID_PESSOA_LICENCIADO", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["LICENCIADO"].ToString());
                cmdInsConsConverte.Parameters.Add("@FLG_TIPO", SqlDbType.Char).Value = "M";
                cmdInsConsConverte.ExecuteNonQuery();
                connInsConsConverte.Close();
                connInsConsConverte.Dispose();

                ClientScript.RegisterStartupScript(this.GetType(),
                "ConverterSucesso", "alert('O representante foi convetido com sucesso!'); ", true);

            
            }
            else
            {
                ClientScript.RegisterStartupScript(this.GetType(),
            "SenhaErro", "alert('A senha digitada não confere com a senha de login! Por favor verifique e reentre.'); ", true);

            }
        }
        else
        {
            ClientScript.RegisterStartupScript(this.GetType(),
        "ConverterErro", "alert('Para realizar a operação é necessário que você digite a sua senha de login! Por favor verifique e reentre.'); ", true);

        }
    }
}