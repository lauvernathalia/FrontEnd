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
/// Summary description for bigdatacorp
/// </summary>
/// 

public class DadosPessoas
{
    public class Root
    {
        public string Datasets { get; set; }
        public string q { get; set; }
        public int Limit { get; set; }
    }

}

public class DadosEmpresas
{
    public class Root
    {
        public string Datasets { get; set; }
        public string q { get; set; }
        public int Limit { get; set; }
    }

}



public class bigdatacorp
{

    public class DadosConsultas
    {
        public class Root
        {
            public string Datasets { get; set; }
            public string q { get; set; }
            [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
            public int Limit { get; set; }
        }

    }


    public class HttpResponseResult
    {
        public int StatusCode { get; set; } // Código HTTP (200, 401, 500, etc.)
        public string Content { get; set; } // Resposta JSON ou mensagem de erro

        public HttpResponseResult(int statusCode, string content)
        {
            StatusCode = statusCode;
            Content = content;
        }
    }

    public static bool GravaConsultas(string sDocumento, string sNome, string sOperacao)
    {
        SqlConnection myGravar = new SqlConnection(Funcoes.conexao());
        myGravar.Open();
        SqlCommand cmdGravar = new SqlCommand("dbo.stp_consulta_ficha_cadastro_ins", myGravar);
        cmdGravar.CommandType = CommandType.StoredProcedure;
        cmdGravar.Parameters.Add("@COD_ID_PESSOA_LICENCIADO", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["LICENCIADO"].ToString());
        cmdGravar.Parameters.Add("@COD_ID_PESSOAS_FJ", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["PESSOA"].ToString());
        cmdGravar.Parameters.Add("@COD_ID_USUARIO", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["CODIGO"].ToString());
        cmdGravar.Parameters.Add("@NOM_OPERACAO", SqlDbType.VarChar).Value = sOperacao.ToString();
        cmdGravar.Parameters.Add("@NOM_DOCUMENTO", SqlDbType.VarChar).Value = sDocumento.ToString();
        cmdGravar.Parameters.Add("@NOM_NOME", SqlDbType.VarChar).Value = sNome.ToString();
        cmdGravar.Parameters.Add("@FLG_ORIGEM", SqlDbType.VarChar).Value = "BIGDATACORP";
        cmdGravar.Parameters.Add("@DTA_DATA", SqlDbType.DateTime).Value = DateTime.Now;
        cmdGravar.Parameters.Add("@flg_operacao", SqlDbType.Char).Value = "I";
        cmdGravar.ExecuteScalar().ToString();
        return true;
    }

    public static string UrlAPI()
    {
        return "https://plataforma.bigdatacorp.com.br";
    }

    public static string TokenBigdatacorp()
    {
        string sPadrao = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJuYW1lIjoiREVWQExFR0FDWUJBTksuQ09NLkJSIiwianRpIjoiMGNjNWE5YjYtMzBmYy00YTE2LWExNGUtZWQ2ODc4NDZhMmMzIiwibmFtZVVzZXIiOiJkZXZAbGVnYWN5YmFuay5jb20uYnIiLCJ1bmlxdWVfbmFtZSI6IkRFVkBMRUdBQ1lCQU5LLkNPTS5CUiIsImRvbWFpbiI6IkxFR0FDWSBURUNOT0xPR0lBIEUgUEFHQU1FTlRPUyBMVERBIiwicHJvZHVjdHMiOlsiQklHQk9PU1QiLCJCSUdJRCJdLCJuYmYiOjE3MTQ3NzI1MjMsImV4cCI6MTg5NDc3MjUyMywiaWF0IjoxNzE0NzcyNTIzLCJpc3MiOiJCaWcgRGF0YSBDb3JwLiJ9.kpzwYsmptu12jAJhiXkEpcFVQo0H9TdAVWOCa0pQQgc";
        // Buscar pelo LICENCIADO

        SqlConnection mySelCadastro = new SqlConnection(Funcoes.conexao());
        mySelCadastro.Open();
        SqlCommand cmdSelCadastro = new SqlCommand("dbo.stp_pessoas_fj_integracoes_chaves_ins", mySelCadastro);
        cmdSelCadastro.CommandType = CommandType.StoredProcedure;
        cmdSelCadastro.Parameters.Add("@flg_operacao", SqlDbType.Char).Value = "L";
        cmdSelCadastro.Parameters.Add("@COD_ID_INTEGRACOES", SqlDbType.Int).Value = 3;
        cmdSelCadastro.Parameters.Add("@COD_ID_PESSOA_LICENCIADO", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["LICENCIADO"].ToString());
        SqlDataReader ReaderCadastro = cmdSelCadastro.ExecuteReader();
        while (ReaderCadastro.Read())
        {
            if (ReaderCadastro["FLG_ATIVO"].ToString().Trim() == "S")
            {
                sPadrao = ReaderCadastro["NOM_TOKEN"].ToString().Trim();
            }
        }

        return sPadrao.ToString();
    }

    public static string IdBigdatacorp()
    {
        string sPadrao = "66355a2b740f750539f6cacd";
        // Buscar pelo LICENCIADO

        SqlConnection mySelCadastro = new SqlConnection(Funcoes.conexao());
        mySelCadastro.Open();
        SqlCommand cmdSelCadastro = new SqlCommand("dbo.stp_pessoas_fj_integracoes_chaves_ins", mySelCadastro);
        cmdSelCadastro.CommandType = CommandType.StoredProcedure;
        cmdSelCadastro.Parameters.Add("@flg_operacao", SqlDbType.Char).Value = "L";
        cmdSelCadastro.Parameters.Add("@COD_ID_INTEGRACOES", SqlDbType.Int).Value = 3;
        cmdSelCadastro.Parameters.Add("@COD_ID_PESSOA_LICENCIADO", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["LICENCIADO"].ToString());
        SqlDataReader ReaderCadastro = cmdSelCadastro.ExecuteReader();
        while (ReaderCadastro.Read())
        {
            if (ReaderCadastro["FLG_ATIVO"].ToString().Trim() == "S")
            {
                sPadrao = ReaderCadastro["NOM_ID"].ToString().Trim();
            }
        }

        return sPadrao.ToString();
    }

    // Novas rotinas de consumo de api

    // *********************************************************  REPRESENTANTE LEGAL ***********************************************************

    public static HttpResponseResult ConsultaRepresentanteLegal(string json)
    {
        ServicePointManager.Expect100Continue = true;
        ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12
            | SecurityProtocolType.Tls11
            | SecurityProtocolType.Tls
            | SecurityProtocolType.Ssl3;

        var byteArray = Encoding.ASCII.GetBytes(zoop.Keymarketplace() + ":" + "");
        string encodeString = Convert.ToBase64String(byteArray);

        string url = bigdatacorp.UrlAPI() + "/ondemand";
        var myUri = new Uri(url);

        var myWebRequest = WebRequest.Create(myUri);
        var myHttpWebRequest = (HttpWebRequest)myWebRequest;
        myHttpWebRequest.ContentType = "application/json";
        myHttpWebRequest.Accept = "application/json";
        myHttpWebRequest.Method = "POST";
        myHttpWebRequest.PreAuthenticate = true;
        myHttpWebRequest.Headers.Add("AccessToken", bigdatacorp.TokenBigdatacorp().ToString());
        myHttpWebRequest.Headers.Add("TokenId", bigdatacorp.IdBigdatacorp().ToString());

        using (var streamWriter = new StreamWriter(myHttpWebRequest.GetRequestStream()))
        {
            streamWriter.Write(json);
            streamWriter.Flush();
        }

        try
        {
            using (var response = (HttpWebResponse)myHttpWebRequest.GetResponse())
            using (var reader = new StreamReader(response.GetResponseStream(), Encoding.UTF8))
            {
                string responseJson = reader.ReadToEnd();
                return new HttpResponseResult((int)response.StatusCode, responseJson);
            }
        }
        catch (WebException ex)
        {
            if (ex.Response != null)
            {
                using (var response = (HttpWebResponse)ex.Response)
                using (var reader = new StreamReader(response.GetResponseStream()))
                {
                    string errorJson = reader.ReadToEnd();
                    return new HttpResponseResult((int)response.StatusCode, errorJson);
                }
            }

            return new HttpResponseResult(500, String.Format("Erro inesperado: {0}", ex.Message));
        }
    }

    // ****************************** CATEGORIA COMERCIAL ***********************************************


    public static HttpResponseResult ConsultaCategoriaComercial(string json)
    {
        ServicePointManager.Expect100Continue = true;
        ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12
            | SecurityProtocolType.Tls11
            | SecurityProtocolType.Tls
            | SecurityProtocolType.Ssl3;

        var byteArray = Encoding.ASCII.GetBytes(zoop.Keymarketplace() + ":" + "");
        string encodeString = Convert.ToBase64String(byteArray);

        string url = bigdatacorp.UrlAPI() + "/empresas";
        var myUri = new Uri(url);

        var myWebRequest = WebRequest.Create(myUri);
        var myHttpWebRequest = (HttpWebRequest)myWebRequest;
        myHttpWebRequest.ContentType = "application/json";
        myHttpWebRequest.Accept = "application/json";
        myHttpWebRequest.Method = "POST";
        myHttpWebRequest.PreAuthenticate = true;
        myHttpWebRequest.Headers.Add("AccessToken", bigdatacorp.TokenBigdatacorp().ToString());
        myHttpWebRequest.Headers.Add("TokenId", bigdatacorp.IdBigdatacorp().ToString());

        using (var streamWriter = new StreamWriter(myHttpWebRequest.GetRequestStream()))
        {
            streamWriter.Write(json);
            streamWriter.Flush();
        }

        try
        {
            using (var response = (HttpWebResponse)myHttpWebRequest.GetResponse())
            using (var reader = new StreamReader(response.GetResponseStream(), Encoding.UTF8))
            {
                string responseJson = reader.ReadToEnd();
                return new HttpResponseResult((int)response.StatusCode, responseJson);
            }
        }
        catch (WebException ex)
        {
            if (ex.Response != null)
            {
                using (var response = (HttpWebResponse)ex.Response)
                using (var reader = new StreamReader(response.GetResponseStream()))
                {
                    string errorJson = reader.ReadToEnd();
                    return new HttpResponseResult((int)response.StatusCode, errorJson);
                }
            }

            return new HttpResponseResult(500, String.Format("Erro inesperado: {0}", ex.Message));
        }
    }


    // Fim das novas rotinas de consumo de api


    public static string ConsultarDadosPessoas(string json)
    {
        ServicePointManager.Expect100Continue = true;
        ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12
        | SecurityProtocolType.Tls11
        | SecurityProtocolType.Tls
        | SecurityProtocolType.Ssl3;

        string URL = bigdatacorp.UrlAPI() + "/pessoas";

        var myUri = new Uri(URL);

        string sResposta = "";

        var myWebRequest = WebRequest.Create(myUri);
        var myHttpWebRequest = (HttpWebRequest)myWebRequest;
        myHttpWebRequest.ContentType = "application/json";
        myHttpWebRequest.Accept = "application/json";
        myHttpWebRequest.Method = "POST";
        myHttpWebRequest.PreAuthenticate = true;
        myHttpWebRequest.Headers.Add("AccessToken", bigdatacorp.TokenBigdatacorp().ToString());
        myHttpWebRequest.Headers.Add("TokenId", bigdatacorp.IdBigdatacorp().ToString());

        using (var streamWriter = new StreamWriter(myHttpWebRequest.GetRequestStream()))
        {
            streamWriter.Write(json);
            streamWriter.Flush();
        }

        try
        {
            var myWebResponse = myWebRequest.GetResponse();
            var responseStream = myWebResponse.GetResponseStream();

            StreamReader myStreamReader = new StreamReader(responseStream, Encoding.UTF8);
            json = myStreamReader.ReadToEnd();
            sResposta = json;
        }
        catch (WebException ex)
        {
            var resp = new StreamReader(ex.Response.GetResponseStream()).ReadToEnd();
            sResposta = resp;
        }

        return sResposta;
    }

    public static string ConsultarDadosEmpresas(string json)
    {
        ServicePointManager.Expect100Continue = true;
        ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12
        | SecurityProtocolType.Tls11
        | SecurityProtocolType.Tls
        | SecurityProtocolType.Ssl3;

        string URL = bigdatacorp.UrlAPI() + "/empresas";

        var myUri = new Uri(URL);

        string sResposta = "";

        var myWebRequest = WebRequest.Create(myUri);
        var myHttpWebRequest = (HttpWebRequest)myWebRequest;
        myHttpWebRequest.ContentType = "application/json";
        myHttpWebRequest.Accept = "application/json";
        myHttpWebRequest.Method = "POST";
        myHttpWebRequest.PreAuthenticate = true;
        myHttpWebRequest.Headers.Add("AccessToken", bigdatacorp.TokenBigdatacorp().ToString());
        myHttpWebRequest.Headers.Add("TokenId", bigdatacorp.IdBigdatacorp().ToString());

        using (var streamWriter = new StreamWriter(myHttpWebRequest.GetRequestStream()))
        {
            streamWriter.Write(json);
            streamWriter.Flush();
        }

        try
        {
            var myWebResponse = myWebRequest.GetResponse();
            var responseStream = myWebResponse.GetResponseStream();

            StreamReader myStreamReader = new StreamReader(responseStream, Encoding.UTF8);
            json = myStreamReader.ReadToEnd();
            sResposta = json;
        }
        catch (WebException ex)
        {
            var resp = new StreamReader(ex.Response.GetResponseStream()).ReadToEnd();
            sResposta = resp;
        }

        return sResposta;
    }

}