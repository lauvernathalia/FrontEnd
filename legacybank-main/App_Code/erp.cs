using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
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

using System.Collections.Specialized;

/// <summary>
/// Summary description for erp
/// </summary>
public class erp
{
    public static string UrlAPI()
    {
        return "https://legaci.vendaerp.com.br/api";
    }

    public static string TokenERP()
    {
        //string sResposta = "7ce09c1b6e933ab01798ae7f3c901a991f98796b8fd61d756a238b5af9123d73dd313d1ca48059ecbd76f416e5337e8e055307aaadfb821c0ca1860e5690d50632c5d1dc989836b9500b012223e531beac8a31bbd76e5a6c10090f128895577079042536324462553278be69a38a9d7a8b406b5eb04366805a0d32d30d0ecc51";
        string sResposta = "";

        SqlConnection mySelCadastro = new SqlConnection(Funcoes.conexao());
        mySelCadastro.Open();
        SqlCommand cmdSelCadastro = new SqlCommand("dbo.stp_pessoas_fj_erp_ins", mySelCadastro);
        cmdSelCadastro.CommandType = CommandType.StoredProcedure;
        cmdSelCadastro.Parameters.Add("@flg_operacao", SqlDbType.Char).Value = "S";
        cmdSelCadastro.Parameters.Add("@COD_ID_PESSOAS_FJ", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["PESSOA"].ToString());
        cmdSelCadastro.Parameters.Add("@COD_ID_PESSOA_LICENCIADO", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["LICENCIADO"].ToString());
        SqlDataReader ReaderCadastro = cmdSelCadastro.ExecuteReader();
        while (ReaderCadastro.Read())
        {
            sResposta = ReaderCadastro["NOM_TOKEN"].ToString();
        }

        return sResposta;

        // Cada estabelecimento possui sua chave e usuário...
    }

    public static string UsuarioERP()
    {
        //string sResposta = "leandro@legaci.com.br";
        string sResposta = "";

        SqlConnection mySelCadastro = new SqlConnection(Funcoes.conexao());
        mySelCadastro.Open();
        SqlCommand cmdSelCadastro = new SqlCommand("dbo.stp_pessoas_fj_erp_ins", mySelCadastro);
        cmdSelCadastro.CommandType = CommandType.StoredProcedure;
        cmdSelCadastro.Parameters.Add("@flg_operacao", SqlDbType.Char).Value = "S";
        cmdSelCadastro.Parameters.Add("@COD_ID_PESSOAS_FJ", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["PESSOA"].ToString());
        cmdSelCadastro.Parameters.Add("@COD_ID_PESSOA_LICENCIADO", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["LICENCIADO"].ToString());
        SqlDataReader ReaderCadastro = cmdSelCadastro.ExecuteReader();
        while (ReaderCadastro.Read())
        {
            sResposta = ReaderCadastro["NOM_USUARIO"].ToString();
        }

        return sResposta;
    }

    public static string AppERP()
    {
        //string sResposta = "erpbank";
        string sResposta = "";

        SqlConnection mySelCadastro = new SqlConnection(Funcoes.conexao());
        mySelCadastro.Open();
        SqlCommand cmdSelCadastro = new SqlCommand("dbo.stp_pessoas_fj_erp_ins", mySelCadastro);
        cmdSelCadastro.CommandType = CommandType.StoredProcedure;
        cmdSelCadastro.Parameters.Add("@flg_operacao", SqlDbType.Char).Value = "S";
        cmdSelCadastro.Parameters.Add("@COD_ID_PESSOAS_FJ", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["PESSOA"].ToString());
        cmdSelCadastro.Parameters.Add("@COD_ID_PESSOA_LICENCIADO", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["LICENCIADO"].ToString());
        SqlDataReader ReaderCadastro = cmdSelCadastro.ExecuteReader();
        while (ReaderCadastro.Read())
        {
            sResposta = ReaderCadastro["NOM_APP"].ToString();
        }

        return sResposta;
    }

    // *************************************************************************************************************************************************************************
    // PEDIDOS ERP
    // *************************************************************************************************************************************************************************

    public static string ConsultarPedido(string codigo)
    {
        ServicePointManager.Expect100Continue = true;
        ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12
        | SecurityProtocolType.Tls11
        | SecurityProtocolType.Tls
        | SecurityProtocolType.Ssl3;

        var URL = erp.UrlAPI().ToString() + "/request/Pedidos/Pesquisar";
        var PARAMS = "";
        PARAMS = "?codigo=" + codigo.ToString();
        var myUri = new Uri(URL.ToString() + PARAMS.ToString());

        var myWebRequest = WebRequest.Create(myUri);
        var myHttpWebRequest = (HttpWebRequest)myWebRequest;
        myHttpWebRequest.PreAuthenticate = true;
        myHttpWebRequest.Method = "GET";
        myHttpWebRequest.Headers.Add("User", erp.UsuarioERP().ToString());
        myHttpWebRequest.Headers.Add("App", erp.AppERP().ToString());
        myHttpWebRequest.Headers.Add("Authorization-Token", erp.TokenERP().ToString());
        myHttpWebRequest.Accept = "application/json";

        try
        {
            var myWebResponse = myWebRequest.GetResponse();
            var responseStream = myWebResponse.GetResponseStream();

            StreamReader myStreamReader = new StreamReader(responseStream, Encoding.UTF8);
            string json = myStreamReader.ReadToEnd();

            return json;
        }
        catch (WebException ex)
        {
            var resp = new StreamReader(ex.Response.GetResponseStream()).ReadToEnd();
            return resp;
        }

        return "";
    }



}