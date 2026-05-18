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
using System.Security.Cryptography;
using System.Net.Sockets;
using System.IO;
using System.Net;

using System.Net.Mail;
using System.Net.Mime;
using System.Net.Configuration;
using System.Net;



public partial class public_html_index : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        string urlorigem = Request.ServerVariables["SERVER_NAME"].ToString();

        string sConexao = ConfigurationManager.AppSettings["user"].ToString() + ";" +
            ConfigurationManager.AppSettings["password"].ToString() + ";" +
            ConfigurationManager.AppSettings["datasource"].ToString() + ";" +
            ConfigurationManager.AppSettings["basecatalog"].ToString();

        SqlConnection mySelCadastro = new SqlConnection(sConexao);
        mySelCadastro.Open();
        SqlCommand cmdSelCadastro = new SqlCommand("dbo.stp_pessoas_fj_ins", mySelCadastro);
        cmdSelCadastro.CommandType = CommandType.StoredProcedure;
        cmdSelCadastro.Parameters.Add("@flg_operacao", SqlDbType.Char).Value = "T";
        cmdSelCadastro.Parameters.Add("@NOM_URL", SqlDbType.VarChar).Value = urlorigem.ToString();
        SqlDataReader ReaderCadastro = cmdSelCadastro.ExecuteReader();
        while (ReaderCadastro.Read())
        {

            imgDestaque.ImageUrl = ReaderCadastro["NOM_LOGO_MENU"].ToString();

            Page.Title = ReaderCadastro["NOM_TITULO_URL"].ToString();
            FavIconLink.Text = "<link rel='icon' type='image/x-icon' href='" + ReaderCadastro["NOM_FAVICON"].ToString() + "'>";
            if (ReaderCadastro["NOM_IMAGEM_FUNDO"].ToString().Trim() != "")
            {
                bdyLogin.Attributes.Add("style", "background: url('" + ReaderCadastro["NOM_IMAGEM_FUNDO"].ToString() + "') no-repeat center center fixed;  -webkit-background-size: cover;  -moz-background-size: cover;  background-size: cover;  -o-background-size: cover;");
            }
            else
            {
                if ((ReaderCadastro["NOM_COR_PRIMARIA_FUNDO"].ToString().Trim() != "") && (ReaderCadastro["NOM_COR_SECUNDARIA_FUNDO"].ToString().Trim() != ""))
                {
                    bdyLogin.Attributes.Add("style", "background: linear-gradient(" + ReaderCadastro["NOM_COR_PRIMARIA_FUNDO"].ToString() + ", " + ReaderCadastro["NOM_COR_SECUNDARIA_FUNDO"].ToString() + ");");
                }
            }

        }
    }

    public string CorPrimaria()
    {
        string urlorigem = Request.ServerVariables["SERVER_NAME"].ToString();

        string sConexao = ConfigurationManager.AppSettings["user"].ToString() + ";" +
            ConfigurationManager.AppSettings["password"].ToString() + ";" +
            ConfigurationManager.AppSettings["datasource"].ToString() + ";" +
            ConfigurationManager.AppSettings["basecatalog"].ToString();


        SqlConnection mySelCadastro = new SqlConnection(sConexao);
        mySelCadastro.Open();
        SqlCommand cmdSelCadastro = new SqlCommand("dbo.stp_pessoas_fj_ins", mySelCadastro);
        cmdSelCadastro.CommandType = CommandType.StoredProcedure;
        cmdSelCadastro.Parameters.Add("@flg_operacao", SqlDbType.Char).Value = "T";
        cmdSelCadastro.Parameters.Add("@NOM_URL", SqlDbType.VarChar).Value = urlorigem.ToString();
        string CorPrimaria = "";
        //cmdSelCadastro.Parameters.Add("@COD_ID_PESSOA_LICENCIADO", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["LICENCIADO"].ToString());
        SqlDataReader ReaderCadastro = cmdSelCadastro.ExecuteReader();
        while (ReaderCadastro.Read())
        {
            CorPrimaria = ReaderCadastro["NOM_COR_PRIMARIA_FUNDO"].ToString();
        }
        return CorPrimaria;
    }

}