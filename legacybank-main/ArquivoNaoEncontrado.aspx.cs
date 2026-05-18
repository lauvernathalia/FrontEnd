using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Handlers;
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

using System.Web.UI.WebControls;


public partial class ArquivoNaoEncontrado : System.Web.UI.Page
{

    protected void Page_Load(object sender, EventArgs e)
    {
        string urlorigem = Request.ServerVariables["SERVER_NAME"].ToString();
        SqlConnection mySelCadastro = new SqlConnection(Funcoes.conexao());
        mySelCadastro.Open();
        SqlCommand cmdSelCadastro = new SqlCommand("dbo.stp_pessoas_fj_ins", mySelCadastro);
        cmdSelCadastro.CommandType = CommandType.StoredProcedure;
        cmdSelCadastro.Parameters.Add("@flg_operacao", SqlDbType.Char).Value = "T";
        cmdSelCadastro.Parameters.Add("@NOM_URL", SqlDbType.VarChar).Value = urlorigem.ToString();
        SqlDataReader ReaderCadastro = cmdSelCadastro.ExecuteReader();
        while (ReaderCadastro.Read())
        {
            imgDestaque.ImageUrl = "../public_html/" + ReaderCadastro["NOM_LOGO_MENU"].ToString();
            bdyLogin.Attributes.Add("style", "background: url('../public_html/" + ReaderCadastro["NOM_IMAGEM_FUNDO"].ToString() + "') no-repeat center center fixed;  -webkit-background-size: cover;  -moz-background-size: cover;  background-size: cover;  -o-background-size: cover;");
        }

        Exception exc = Server.GetLastError();

        // Gravar na base de dados de ocorrências
        SqlConnection connInsConsOcorrencias = new SqlConnection(Funcoes.conexao());
        connInsConsOcorrencias.Open();
        SqlCommand cmdInsConsOcorrencias = new SqlCommand("dbo.stp_ocorrencias_ins", connInsConsOcorrencias);
        cmdInsConsOcorrencias.CommandType = CommandType.StoredProcedure;
        cmdInsConsOcorrencias.Parameters.Add("@flg_operacao", SqlDbType.Char).Value = 'I';

        try
        {
            cmdInsConsOcorrencias.Parameters.Add("@COD_ID_PESSOA_LICENCIADO", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["LICENCIADO"].ToString());
        }
        catch
        {
            cmdInsConsOcorrencias.Parameters.Add("@COD_ID_PESSOA_LICENCIADO", SqlDbType.Int).Value = 0;
        }
        try
        {
            cmdInsConsOcorrencias.Parameters.Add("@COD_ID_PESSOAS_FJ", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["PESSOA"].ToString());
        }
        catch
        {
            cmdInsConsOcorrencias.Parameters.Add("@COD_ID_PESSOAS_FJ", SqlDbType.Int).Value = 0;
        }        
        cmdInsConsOcorrencias.Parameters.Add("@DTA_DATA", SqlDbType.DateTime).Value = DateTime.Now;

        cmdInsConsOcorrencias.Parameters.Add("@DES_DESCRICAO", SqlDbType.Text).Value = "Página ou arquivo não encontrado - 404";//exc.ToString();

        cmdInsConsOcorrencias.ExecuteNonQuery();
        connInsConsOcorrencias.Close();
        connInsConsOcorrencias.Dispose();
    }

    protected void btnFechar_Click(object sender, EventArgs e)
    {
        ClientScript.RegisterStartupScript(this.GetType(),
    "Alerta", "window.close(); ", true);

    }
}