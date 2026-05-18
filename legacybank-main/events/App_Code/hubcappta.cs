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

// Coleção dados Cappta
// Dados do Revendedor --------------------------------------------------------------------------------------//
public class DadosRevendedor
{
    public class Address
    {
        public string postalCode { get; set; }
        public string streetName { get; set; }
        public string houseNumber { get; set; }
        public string complement { get; set; }
        public string neighborhood { get; set; }
        public string city { get; set; }
        public string state { get; set; }
    }

    public class BankAccount
    {
        public string account { get; set; }
        public string bankCode { get; set; }
        public string branch { get; set; }
        public int accountType { get; set; }
    }

    public class Reseller
    {
        public string document { get; set; }
        public string companyName { get; set; }
        public string tradingName { get; set; }
        public int mccId { get; set; }
        public int legalNatureId { get; set; }
    }

    public class Responsible
    {
        public string name { get; set; }
        public string email { get; set; }
        public string phone { get; set; }
        public string mobilePhone { get; set; }
        public string cpf { get; set; }
    }

    public class Root
    {
        public Reseller reseller { get; set; }
        public Responsible responsible { get; set; }
        public Address address { get; set; }
        public BankAccount bankAccount { get; set; }
    }
}

// Dados do Lojista --------------------------------------------------------------------------------------//
public class DadosLojista
{

    public class Address
    {
        public string postalCode { get; set; }
        public string streetName { get; set; }
        public string houseNumber { get; set; }
        public string complement { get; set; }
        public string neighborhood { get; set; }
        public string city { get; set; }
        public string state { get; set; }
    }

    public class BankAccount
    {
        public string account { get; set; }
        public string bankCode { get; set; }
        public string branch { get; set; }
        public int accountType { get; set; }
    }

    public class Merchant
    {
        public string document { get; set; }
        public string companyName { get; set; }
        public string tradingName { get; set; }
        public int mccId { get; set; }
        public int legalNatureId { get; set; }
        public int TpvExpected { get; set; }
    }

    public class Owner
    {
        public string name { get; set; }
        public string email { get; set; }
        public string phone { get; set; }
        public string mobilePhone { get; set; }
        public string cpf { get; set; }
    }

    public class Root
    {
        public string resellerDocument { get; set; }
        public Merchant merchant { get; set; }
        public Owner owner { get; set; }
        public Address address { get; set; }
        public BankAccount bankAccount { get; set; }
        public int planId { get; set; }
    }

}

public class DadosPOSCappta
{
    public class Root
    {
        public string resellerDocument { get; set; }
        public string serialKey { get; set; }
        public int modelId { get; set; }
    }
}

public class DadosPOSVincularCappta
{
    public class Root
    {
        public string resellerDocument { get; set; }
        public string merchantDocument { get; set; }
    }

}

// Dados de Autenticação --------------------------------------------------------------------------------------//
public class DadosAutenticar
{
    public class Root
    {
        public string clientId { get; set; }
        public string clientSecret { get; set; }
    }
}

/// <summary>
/// Summary description for hubcappta
/// </summary>
public class hubcappta
{
    // Autenticação --------------------------------------------------------------------------------------//
    // Documentação: integration.cappta.com.br

    public static string UrlAPI()
    {
        return "https://api.posportal.com.br/api/hub";
    }    
    
    public static string TokenCappta()
    {
        string sPadrao = "";

        SqlConnection mySelCadastro = new SqlConnection(Funcoes.conexao());
        mySelCadastro.Open();
        SqlCommand cmdSelCadastro = new SqlCommand("dbo.stp_pessoas_fj_integracoes_chaves_ins", mySelCadastro);
        cmdSelCadastro.CommandType = CommandType.StoredProcedure;
        cmdSelCadastro.Parameters.Add("@flg_operacao", SqlDbType.Char).Value = "L";
        cmdSelCadastro.Parameters.Add("@COD_ID_INTEGRACOES", SqlDbType.Int).Value = 5;
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

    // **************************************************************************************************************************************************************************
    // Setor PLANOS
    // **************************************************************************************************************************************************************************

    public static string ListarOpcoesPlanos()
    {
        ServicePointManager.Expect100Continue = true;
        ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12
        | SecurityProtocolType.Tls11
        | SecurityProtocolType.Tls
        | SecurityProtocolType.Ssl3;

        try
        {
            string URL = hubcappta.UrlAPI() + "/plan";

            var myUri = new Uri(URL);
            var myWebRequest = WebRequest.Create(myUri);
            var myHttpWebRequest = (HttpWebRequest)myWebRequest;
            myHttpWebRequest.PreAuthenticate = true;
            myHttpWebRequest.Method = "OPTIONS";
            myHttpWebRequest.Headers.Add("Authorization", "Bearer " + hubcappta.TokenCappta().ToString() + "");
            myHttpWebRequest.Accept = "application/json";

            var myWebResponse = myWebRequest.GetResponse();
            var responseStream = myWebResponse.GetResponseStream();

            StreamReader myStreamReader = new StreamReader(responseStream, Encoding.UTF8);
            string json = myStreamReader.ReadToEnd();

            // Incluir dados na tabela de opções de planos

            JObject oOpcoesPlanos = JObject.Parse(json);
            if (oOpcoesPlanos["schemes"].Count() > 0)
            {
                for (int i = 0; i < oOpcoesPlanos["schemes"].Count(); i++)
                {
                    // Dados Onboarding Cappta
                    SqlConnection connInsConsDadosCappta = new SqlConnection(Funcoes.conexao());
                    connInsConsDadosCappta.Open();
                    SqlCommand cmdInsConsDadosCappta = new SqlCommand("dbo.stp_opcoes_planos_cappta_ins", connInsConsDadosCappta);
                    cmdInsConsDadosCappta.CommandType = CommandType.StoredProcedure;
                    cmdInsConsDadosCappta.Parameters.Add("@flg_operacao", SqlDbType.Char).Value = 'I';
                    cmdInsConsDadosCappta.Parameters.Add("@COD_ID_OPCOES_PLANOS_CAPPTA", SqlDbType.Int).Value = Funcoes.strToInt(oOpcoesPlanos["schemes"][i]["id"].ToString());
                    cmdInsConsDadosCappta.Parameters.Add("@NOM_REFERENCIA", SqlDbType.VarChar).Value = oOpcoesPlanos["schemes"][i]["scheme"].ToString();
                    cmdInsConsDadosCappta.Parameters.Add("@NOM_TIPO", SqlDbType.VarChar).Value = oOpcoesPlanos["schemes"][i]["type"].ToString();
                    cmdInsConsDadosCappta.Parameters.Add("@NOM_DESCRICAO", SqlDbType.VarChar).Value = oOpcoesPlanos["schemes"][i]["description"].ToString();
                    cmdInsConsDadosCappta.ExecuteNonQuery();
                    connInsConsDadosCappta.Close();
                    connInsConsDadosCappta.Dispose();
                }
            }

            return json;
        }
        catch (WebException ex)
        {
            var resp = new StreamReader(ex.Response.GetResponseStream()).ReadToEnd();
            return resp;
        }

    }
    
    public static string ConsultarPlanos(int iPagina, string sTaxas, string stype)
    {
        ServicePointManager.Expect100Continue = true;
        ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12
        | SecurityProtocolType.Tls11
        | SecurityProtocolType.Tls
        | SecurityProtocolType.Ssl3;

        try
        {
            string URL = hubcappta.UrlAPI() + "/plan?pageId="+iPagina.ToString()+"&withFees="+sTaxas.ToString()+"&type="+stype.ToString();

            var myUri = new Uri(URL);
            var myWebRequest = WebRequest.Create(myUri);
            var myHttpWebRequest = (HttpWebRequest)myWebRequest;
            myHttpWebRequest.PreAuthenticate = true;
            myHttpWebRequest.Method = "GET";
            myHttpWebRequest.Headers.Add("Authorization", "Bearer " + hubcappta.TokenCappta().ToString() + "");
            myHttpWebRequest.Accept = "application/json";

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

    }

    public static string ConsultarPlano(string codigo)
    {
        ServicePointManager.Expect100Continue = true;
        ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12
        | SecurityProtocolType.Tls11
        | SecurityProtocolType.Tls
        | SecurityProtocolType.Ssl3;

        try
        {
            var PARAMS = "";
            PARAMS = "/"+ codigo.ToString();

            string URL = hubcappta.UrlAPI() + "/plan";

            var myUri = new Uri(URL + PARAMS);
            var myWebRequest = WebRequest.Create(myUri);
            var myHttpWebRequest = (HttpWebRequest)myWebRequest;
            myHttpWebRequest.PreAuthenticate = true;
            myHttpWebRequest.Method = "GET";
            myHttpWebRequest.Headers.Add("Authorization", "Bearer " + hubcappta.TokenCappta().ToString() + "");
            myHttpWebRequest.Accept = "application/json";

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

    }

    // **************************************************************************************************************************************************************************
    // Setor REVENDEDOR
    // **************************************************************************************************************************************************************************

    public static string ConsultarRevendedores()
    {
        ServicePointManager.Expect100Continue = true;
        ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12
        | SecurityProtocolType.Tls11
        | SecurityProtocolType.Tls
        | SecurityProtocolType.Ssl3;

        try
        {
            string URL = hubcappta.UrlAPI() + "/onboarding/reseller";

            var myUri = new Uri(URL);
            var myWebRequest = WebRequest.Create(myUri);
            var myHttpWebRequest = (HttpWebRequest)myWebRequest;
            myHttpWebRequest.PreAuthenticate = true;
            myHttpWebRequest.Method = "GET";
            myHttpWebRequest.Headers.Add("Authorization", "Bearer " + hubcappta.TokenCappta().ToString() + "");
            myHttpWebRequest.Accept = "application/json";

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

    }

    public static string ConsultarRevendedor(string sDocumento)
    {
        ServicePointManager.Expect100Continue = true;
        ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12
        | SecurityProtocolType.Tls11
        | SecurityProtocolType.Tls
        | SecurityProtocolType.Ssl3;

        try
        {
            string URL = hubcappta.UrlAPI() + "/onboarding/reseller/"+sDocumento.ToString();

            var myUri = new Uri(URL);
            var myWebRequest = WebRequest.Create(myUri);
            var myHttpWebRequest = (HttpWebRequest)myWebRequest;
            myHttpWebRequest.PreAuthenticate = true;
            myHttpWebRequest.Method = "GET";
            myHttpWebRequest.Headers.Add("Authorization", "Bearer " + hubcappta.TokenCappta().ToString() + "");
            myHttpWebRequest.Accept = "application/json";

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

    }


    public static string CadastrarRevendedor(string json)
    {
        ServicePointManager.Expect100Continue = true;
        ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12
        | SecurityProtocolType.Tls11
        | SecurityProtocolType.Tls
        | SecurityProtocolType.Ssl3;

        string URL = hubcappta.UrlAPI() + "/onboarding/reseller";

        var myUri = new Uri(URL);

        string sResposta = "";

        var myWebRequest = WebRequest.Create(myUri);
        var myHttpWebRequest = (HttpWebRequest)myWebRequest;
        myHttpWebRequest.ContentType = "application/json";
        myHttpWebRequest.Accept = "application/json";
        myHttpWebRequest.Method = "POST";
        myHttpWebRequest.PreAuthenticate = true;
        myHttpWebRequest.Headers.Add("Authorization", "Bearer " + hubcappta.TokenCappta().ToString() + "");

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

    // **************************************************************************************************************************************************************************
    // Setor LOJISTA
    // **************************************************************************************************************************************************************************

    public static string ConsultarLojistas(string sDocumento)
    {
        ServicePointManager.Expect100Continue = true;
        ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12
        | SecurityProtocolType.Tls11
        | SecurityProtocolType.Tls
        | SecurityProtocolType.Ssl3;


        try
        {
            string URL = hubcappta.UrlAPI() + "/onboarding/merchant?resellerDocument="+sDocumento.ToString();

            var myUri = new Uri(URL);
            var myWebRequest = WebRequest.Create(myUri);
            var myHttpWebRequest = (HttpWebRequest)myWebRequest;
            myHttpWebRequest.PreAuthenticate = true;
            myHttpWebRequest.Method = "GET";
            myHttpWebRequest.Headers.Add("Authorization", "Bearer " + hubcappta.TokenCappta().ToString() + "");
            myHttpWebRequest.Accept = "application/json";

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

    }

    public static string ConsultarLojista(string sLojista, string sDocumento)
    {
        ServicePointManager.Expect100Continue = true;
        ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12
        | SecurityProtocolType.Tls11
        | SecurityProtocolType.Tls
        | SecurityProtocolType.Ssl3;

        try
        {
            string URL = hubcappta.UrlAPI() + "/onboarding/merchant/"+ sLojista.ToString() +"?resellerDocument=" + sDocumento.ToString();

            var myUri = new Uri(URL);
            var myWebRequest = WebRequest.Create(myUri);
            var myHttpWebRequest = (HttpWebRequest)myWebRequest;
            myHttpWebRequest.PreAuthenticate = true;
            myHttpWebRequest.Method = "GET";
            myHttpWebRequest.Headers.Add("Authorization", "Bearer " + hubcappta.TokenCappta().ToString() + "");
            myHttpWebRequest.Accept = "application/json";

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

    }


    public static string CadastrarLojista(string json)
    {
        ServicePointManager.Expect100Continue = true;
        ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12
        | SecurityProtocolType.Tls11
        | SecurityProtocolType.Tls
        | SecurityProtocolType.Ssl3;

        string URL = hubcappta.UrlAPI() + "/onboarding/merchant";

        var myUri = new Uri(URL);

        string sResposta = "";

        var myWebRequest = WebRequest.Create(myUri);
        var myHttpWebRequest = (HttpWebRequest)myWebRequest;
        myHttpWebRequest.ContentType = "application/json";
        myHttpWebRequest.Accept = "application/json";
        myHttpWebRequest.Method = "POST";
        myHttpWebRequest.PreAuthenticate = true;
        myHttpWebRequest.Headers.Add("Authorization", "Bearer " + hubcappta.TokenCappta().ToString() + "");

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


    // **************************************************************************************************************************************************************************
    // Setor OUTRAS FUNCOES
    // **************************************************************************************************************************************************************************

    public static string ListarOpcoesCadastro()
    {
        ServicePointManager.Expect100Continue = true;
        ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12
        | SecurityProtocolType.Tls11
        | SecurityProtocolType.Tls
        | SecurityProtocolType.Ssl3;

        try
        {
            string URL = hubcappta.UrlAPI() + "/onboarding";

            var myUri = new Uri(URL);
            var myWebRequest = WebRequest.Create(myUri);
            var myHttpWebRequest = (HttpWebRequest)myWebRequest;
            myHttpWebRequest.PreAuthenticate = true;
            myHttpWebRequest.Method = "OPTIONS";
            myHttpWebRequest.Headers.Add("Authorization", "Bearer " + hubcappta.TokenCappta().ToString() + "");
            myHttpWebRequest.Accept = "application/json";

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

    }


    // **************************************************************************************************************************************************************************
    // Setor POS
    // **************************************************************************************************************************************************************************

    public static string ConsultarListaPOS(string sDocumento)
    {
        ServicePointManager.Expect100Continue = true;
        ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12
        | SecurityProtocolType.Tls11
        | SecurityProtocolType.Tls
        | SecurityProtocolType.Ssl3;

        var PARAMS = "";
        if (sDocumento.ToString().Trim() != "")
        {
            PARAMS = "?resellerDocument=" + sDocumento.ToString();
        }

        try
        {
            string URL = hubcappta.UrlAPI() + "/pos/device";

            var myUri = new Uri(URL + PARAMS);
            var myWebRequest = WebRequest.Create(myUri);
            var myHttpWebRequest = (HttpWebRequest)myWebRequest;
            myHttpWebRequest.PreAuthenticate = true;
            myHttpWebRequest.Method = "GET";
            myHttpWebRequest.Headers.Add("Authorization", "Bearer " + hubcappta.TokenCappta().ToString() + "");
            //myHttpWebRequest.Headers.Add("Authorization", "Bearer " + encodeString.ToString() + "");
            myHttpWebRequest.Accept = "application/json";

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

    }

    public static string ListarOpcoesPOS()
    {
        ServicePointManager.Expect100Continue = true;
        ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12
        | SecurityProtocolType.Tls11
        | SecurityProtocolType.Tls
        | SecurityProtocolType.Ssl3;

        try
        {
            string URL = hubcappta.UrlAPI() + "/pos";

            var myUri = new Uri(URL);
            var myWebRequest = WebRequest.Create(myUri);
            var myHttpWebRequest = (HttpWebRequest)myWebRequest;
            myHttpWebRequest.PreAuthenticate = true;
            myHttpWebRequest.Method = "OPTIONS";
            myHttpWebRequest.Headers.Add("Authorization", "Bearer " + hubcappta.TokenCappta().ToString() + "");
            myHttpWebRequest.Accept = "application/json";

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

    }

    public static string CadastrarPOS(string json)
    {
        ServicePointManager.Expect100Continue = true;
        ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12
        | SecurityProtocolType.Tls11
        | SecurityProtocolType.Tls
        | SecurityProtocolType.Ssl3;

        string URL = hubcappta.UrlAPI() + "/pos/device";

        var myUri = new Uri(URL);

        string sResposta = "";

        var myWebRequest = WebRequest.Create(myUri);
        var myHttpWebRequest = (HttpWebRequest)myWebRequest;
        myHttpWebRequest.ContentType = "application/json";
        myHttpWebRequest.Accept = "application/json";
        myHttpWebRequest.Method = "POST";
        myHttpWebRequest.PreAuthenticate = true;
        myHttpWebRequest.Headers.Add("Authorization", "Bearer " + hubcappta.TokenCappta().ToString() + "");

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


    public static string VincularPOS(string sID, string json)
    {
        ServicePointManager.Expect100Continue = true;
        ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12
        | SecurityProtocolType.Tls11
        | SecurityProtocolType.Tls
        | SecurityProtocolType.Ssl3;


        string PARAMS = "/" + sID.ToString() + "/bind";
        string URL = hubcappta.UrlAPI() + "/pos/device";

        var myUri = new Uri(URL);

        string sResposta = "";

        var myWebRequest = WebRequest.Create(myUri);
        var myHttpWebRequest = (HttpWebRequest)myWebRequest;
        myHttpWebRequest.ContentType = "application/json";
        myHttpWebRequest.Accept = "application/json";
        myHttpWebRequest.Method = "PATCH";
        myHttpWebRequest.PreAuthenticate = true;
        myHttpWebRequest.Headers.Add("Authorization", "Bearer " + hubcappta.TokenCappta().ToString() + "");

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


    public static string DesvincularPOS(string sID)
    {
        ServicePointManager.Expect100Continue = true;
        ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12
        | SecurityProtocolType.Tls11
        | SecurityProtocolType.Tls
        | SecurityProtocolType.Ssl3;


        string PARAMS = "/" + sID.ToString() + "/unbind";
        string URL = hubcappta.UrlAPI() + "/pos/device";

        var myUri = new Uri(URL);

        string sResposta = "";

        var myWebRequest = WebRequest.Create(myUri);
        var myHttpWebRequest = (HttpWebRequest)myWebRequest;
        myHttpWebRequest.ContentType = "application/json";
        myHttpWebRequest.Accept = "application/json";
        myHttpWebRequest.Method = "PATCH";
        myHttpWebRequest.PreAuthenticate = true;
        myHttpWebRequest.Headers.Add("Authorization", "Bearer " + hubcappta.TokenCappta().ToString() + "");

        try
        {
            var myWebResponse = myWebRequest.GetResponse();
            var responseStream = myWebResponse.GetResponseStream();

            StreamReader myStreamReader = new StreamReader(responseStream, Encoding.UTF8);
            sResposta = myStreamReader.ReadToEnd(); 
        }
        catch (WebException ex)
        {
            var resp = new StreamReader(ex.Response.GetResponseStream()).ReadToEnd();
            sResposta = resp;
        }

        return sResposta;
    }

    // **************************************************************************************************************************************************************************
    // Setor PAGAMENTO - TRANSAÇÕES
    // **************************************************************************************************************************************************************************

    public static string ConsultarTransacoes(string filtro)
    {
        ServicePointManager.Expect100Continue = true;
        ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12
        | SecurityProtocolType.Tls11
        | SecurityProtocolType.Tls
        | SecurityProtocolType.Ssl3;

        try
        {
            string URL = hubcappta.UrlAPI() +"/payment?"+ filtro.ToString(); //"/payment?pageId=1&createdSince=2024-04-01&createdUntil=2024-09-29";

            var myUri = new Uri(URL);
            var myWebRequest = WebRequest.Create(myUri);
            var myHttpWebRequest = (HttpWebRequest)myWebRequest;
            myHttpWebRequest.PreAuthenticate = true;
            myHttpWebRequest.Method = "GET";
            myHttpWebRequest.Headers.Add("Authorization", "Bearer " + hubcappta.TokenCappta().ToString() + "");
            myHttpWebRequest.Accept = "application/json";

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

    }

    // ************************
    // VERIFICAR EXISTENCIA DOS CADASTROS
    // ************************

    public static string VerificaMarketplace(string sMarketplace)
    {
        string sCodigoMarketplace = "0";

        // Inicio - Verifica primeiro na base de dados se este CNPJCPF existe para  agilizar a busca e velocidade de processamento

        SqlConnection mySelCadastro = new SqlConnection(Funcoes.conexao());
        mySelCadastro.Open();
        SqlCommand cmdSelCadastro = new SqlCommand("dbo.stp_pessoas_fj_ins", mySelCadastro);
        cmdSelCadastro.CommandType = CommandType.StoredProcedure;
        cmdSelCadastro.Parameters.Add("@flg_operacao", SqlDbType.Char).Value = "H";
        cmdSelCadastro.Parameters.Add("@COD_ID_PESSOA_LICENCIADO", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["LICENCIADO"].ToString());
        cmdSelCadastro.Parameters.Add("@NOM_CNPJ", SqlDbType.VarChar).Value = sMarketplace.ToString();
        cmdSelCadastro.Parameters.Add("@FLG_TIPO", SqlDbType.Char).Value = "M";
        SqlDataReader ReaderCadastro = cmdSelCadastro.ExecuteReader();
        while (ReaderCadastro.Read())
        {
            sCodigoMarketplace = ReaderCadastro["COD_ID"].ToString();
        }

        // Fim - Verifica primeiro na base de dados se este CNPJCPF existe para  agilizar a busca e velocidade de processamento

        if (Funcoes.strToInt(sCodigoMarketplace.ToString()) <= 0)
        {


            string jsonRetornoCadastro = hubcappta.ConsultarRevendedor(sMarketplace.ToString());
            JObject oRevendedor = JObject.Parse(jsonRetornoCadastro);

            try
            {
                SqlConnection connInsCons = new SqlConnection(Funcoes.conexao());
                connInsCons.Open();
                SqlCommand cmdInsCons = new SqlCommand("dbo.stp_pessoas_fj_ins", connInsCons);
                cmdInsCons.CommandType = CommandType.StoredProcedure;
                cmdInsCons.Parameters.Add("@flg_operacao", SqlDbType.Char).Value = 'I';
                cmdInsCons.Parameters.Add("@FLG_ATIVO", SqlDbType.Char).Value = "S";
                cmdInsCons.Parameters.Add("@FLG_INSERT_USUARIO", SqlDbType.Char).Value = "S";
                cmdInsCons.Parameters.Add("@COD_ID_PESSOA_LICENCIADO", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["LICENCIADO"].ToString());
                cmdInsCons.Parameters.Add("@DTA_DATA", SqlDbType.DateTime).Value = DateTime.Now;
                cmdInsCons.Parameters.Add("@FLG_TIPO", SqlDbType.Char).Value = "M";


                cmdInsCons.Parameters.Add("@FLG_TIPO_PESSOA", SqlDbType.VarChar).Value = (oRevendedor["reseller"]["document"].ToString().Trim().Length >= 14) ? "PJ" : "PF";
                cmdInsCons.Parameters.Add("@FLG_PRESENCIAL", SqlDbType.Char).Value = "N";
                //            cmdInsCons.Parameters.Add("@COD_ID_MARKETPLACE", SqlDbType.Int).Value = Funcoes.strToInt(ddlMarketplace.SelectedValue.ToString());

                // Empresa
                cmdInsCons.Parameters.Add("@NOM_RAZAOSOCIAL", SqlDbType.VarChar).Value = oRevendedor["reseller"]["companyName"].ToString().Trim();
                cmdInsCons.Parameters.Add("@NOM_FANTASIA", SqlDbType.VarChar).Value = oRevendedor["reseller"]["tradingName"].ToString().Trim();
                cmdInsCons.Parameters.Add("@NOM_CNPJ", SqlDbType.VarChar).Value = oRevendedor["reseller"]["document"].ToString().Trim();

                cmdInsCons.Parameters.Add("@NUM_TELEFONE", SqlDbType.VarChar).Value = oRevendedor["responsible"]["phone"].ToString().Trim();
                cmdInsCons.Parameters.Add("@NOM_EMAIL_EMPRESA", SqlDbType.VarChar).Value = oRevendedor["responsible"]["email"].ToString().Trim();
                cmdInsCons.Parameters.Add("@COD_ID_MCC", SqlDbType.Int).Value = Funcoes.strToInt(oRevendedor["reseller"]["mccId"].ToString().Trim());
                //cmdInsCons.Parameters.Add("@NOM_TIPO_EMPRESA", SqlDbType.VarChar).Value = ddlTipoEmpresa.SelectedValue.ToString();
                cmdInsCons.Parameters.Add("@NUM_FATURAMENTO", SqlDbType.Float).Value = 0;
                cmdInsCons.Parameters.Add("@NUM_PATRIMONIO", SqlDbType.Float).Value = 0;

                //if (txtDataAbertura.Text.ToString().Trim() != "")
                //{
                //    cmdInsCons.Parameters.Add("@DTA_ABERTURA", SqlDbType.DateTime).Value = Convert.ToDateTime(txtDataAbertura.Text.ToString());
                //}

                // Endereço
                cmdInsCons.Parameters.Add("@NOM_ENDERECO", SqlDbType.VarChar).Value = oRevendedor["address"]["streetName"].ToString().Trim();
                cmdInsCons.Parameters.Add("@NOM_NUMERO", SqlDbType.VarChar).Value = oRevendedor["address"]["houseNumber"].ToString().Trim();
                cmdInsCons.Parameters.Add("@NOM_COMPLEMENTO", SqlDbType.VarChar).Value = oRevendedor["address"]["complement"].ToString().Trim();
                cmdInsCons.Parameters.Add("@NOM_BAIRRO", SqlDbType.VarChar).Value = oRevendedor["address"]["neighborhood"].ToString().Trim();
                cmdInsCons.Parameters.Add("@NOM_CIDADE", SqlDbType.VarChar).Value = oRevendedor["address"]["city"].ToString().Trim();
                cmdInsCons.Parameters.Add("@NOM_UF", SqlDbType.VarChar).Value = oRevendedor["address"]["state"].ToString().Trim();
                cmdInsCons.Parameters.Add("@NOM_CEP", SqlDbType.VarChar).Value = oRevendedor["address"]["postalCode"].ToString().Trim();
                cmdInsCons.Parameters.Add("@NOM_PAIS", SqlDbType.VarChar).Value = "BR";

                // Responsável
                cmdInsCons.Parameters.Add("@NOM_NOME", SqlDbType.VarChar).Value = oRevendedor["responsible"]["name"].ToString().Trim();
                cmdInsCons.Parameters.Add("@NOM_SOBRENOME", SqlDbType.VarChar).Value = oRevendedor["responsible"]["phone"].ToString().Trim();
                cmdInsCons.Parameters.Add("@NOM_CPF", SqlDbType.VarChar).Value = oRevendedor["responsible"]["cpf"].ToString().Trim();

                //if (txtNascimento.Text.ToString().Trim() != "")
                //{
                //    cmdInsCons.Parameters.Add("@DTA_ANIVERSARIO", SqlDbType.DateTime).Value = Convert.ToDateTime(txtNascimento.Text.ToString());
                //}

                //cmdInsCons.Parameters.Add("@NOM_MAE", SqlDbType.VarChar).Value = txtNomeMae.Text.ToString();
                //cmdInsCons.Parameters.Add("@NUM_RENDA_MENSAL", SqlDbType.Float).Value = Funcoes.strToDouble(txtRendaMensalPF.Text.ToString());
                cmdInsCons.Parameters.Add("@NOM_EMAIL", SqlDbType.VarChar).Value = oRevendedor["responsible"]["email"].ToString().Trim();
                cmdInsCons.Parameters.Add("@NOM_CELULAR", SqlDbType.VarChar).Value = oRevendedor["responsible"]["mobilePhone"].ToString().Trim();
                //cmdInsCons.Parameters.Add("@FLG_POLITICAMENTE", SqlDbType.Char).Value = ddlPoliticamenteExposta.SelectedValue.ToString();

                // Usuário
                cmdInsCons.Parameters.Add("@NOM_NOME_USUARIO", SqlDbType.VarChar).Value = oRevendedor["responsible"]["name"].ToString().Trim();
                cmdInsCons.Parameters.Add("@NOM_LOGIN", SqlDbType.VarChar).Value = oRevendedor["responsible"]["email"].ToString().Trim();
                cmdInsCons.Parameters.Add("@NOM_SENHA", SqlDbType.VarChar).Value = oRevendedor["reseller"]["document"].ToString().Trim();

                sCodigoMarketplace = cmdInsCons.ExecuteScalar().ToString();

                //cmdInsCons.ExecuteNonQuery();
                connInsCons.Close();
                connInsCons.Dispose();

                // Dados Onboarding Cappta
                SqlConnection connInsConsDadosCappta = new SqlConnection(Funcoes.conexao());
                connInsConsDadosCappta.Open();
                SqlCommand cmdInsConsDadosCappta = new SqlCommand("dbo.stp_pessoas_fj_cappta_ins", connInsConsDadosCappta);
                cmdInsConsDadosCappta.CommandType = CommandType.StoredProcedure;
                cmdInsConsDadosCappta.Parameters.Add("@flg_operacao", SqlDbType.Char).Value = 'I';
                cmdInsConsDadosCappta.Parameters.Add("@COD_ID_PESSOA_LICENCIADO", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["LICENCIADO"].ToString());
                cmdInsConsDadosCappta.Parameters.Add("@COD_ID_PESSOAS_FJ", SqlDbType.Int).Value = Funcoes.strToInt(sCodigoMarketplace.ToString());

                cmdInsConsDadosCappta.Parameters.Add("@NUM_TOKEN_CAPPTA", SqlDbType.VarChar).Value = oRevendedor["reseller"]["document"].ToString();
                cmdInsConsDadosCappta.Parameters.Add("@FLG_STATUS_CAPPTA", SqlDbType.VarChar).Value = oRevendedor["statusDescription"].ToString();
                cmdInsConsDadosCappta.Parameters.Add("@FLG_CAPPTA", SqlDbType.Char).Value = "S";
                cmdInsConsDadosCappta.Parameters.Add("@DES_JSON_CAPPTA", SqlDbType.VarChar).Value = jsonRetornoCadastro.ToString();
                cmdInsConsDadosCappta.Parameters.Add("@COD_ID_NATUREZA_CAPPTA", SqlDbType.Int).Value = (oRevendedor["reseller"]["legalNatureId"].ToString().Trim() != "") ? Funcoes.strToInt(oRevendedor["reseller"]["legalNatureId"].ToString()) : 0;



                cmdInsConsDadosCappta.ExecuteNonQuery();
                connInsConsDadosCappta.Close();
                connInsConsDadosCappta.Dispose();

                // Dados Bancários Cappta
                SqlConnection connInsConsBancoCappta = new SqlConnection(Funcoes.conexao());
                connInsConsBancoCappta.Open();
                SqlCommand cmdInsConsBancoCappta = new SqlCommand("dbo.stp_pessoas_fj_contas_ins", connInsConsBancoCappta);
                cmdInsConsBancoCappta.CommandType = CommandType.StoredProcedure;
                cmdInsConsBancoCappta.Parameters.Add("@flg_operacao", SqlDbType.Char).Value = 'I';
                cmdInsConsBancoCappta.Parameters.Add("@COD_ID_PESSOA_LICENCIADO", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["LICENCIADO"].ToString());
                cmdInsConsBancoCappta.Parameters.Add("@COD_ID_PESSOAS_FJ", SqlDbType.Int).Value = Funcoes.strToInt(sCodigoMarketplace.ToString());

                cmdInsConsBancoCappta.Parameters.Add("@NOM_CODIGO_BANCO", SqlDbType.VarChar).Value = oRevendedor["bankAccount"]["bankCode"].ToString();
                cmdInsConsBancoCappta.Parameters.Add("@NOM_TIPO_BANCO", SqlDbType.VarChar).Value = (oRevendedor["bankAccount"]["accountType"].ToString().Trim() == "1") ? "C" : "P";
                cmdInsConsBancoCappta.Parameters.Add("@NOM_NUMERO_AGENCIA_BANCO", SqlDbType.VarChar).Value = oRevendedor["bankAccount"]["branch"].ToString();
                cmdInsConsBancoCappta.Parameters.Add("@NOM_NUMERO_DIGITO_AGENCIA_BANCO", SqlDbType.VarChar).Value = "";
                cmdInsConsBancoCappta.Parameters.Add("@NOM_NUMERO_CONTA_BANCO", SqlDbType.VarChar).Value = oRevendedor["bankAccount"]["account"].ToString();
                cmdInsConsBancoCappta.Parameters.Add("@NOM_NUMERO_DIGITO_CONTA_BANCO", SqlDbType.VarChar).Value = "";
                cmdInsConsBancoCappta.Parameters.Add("@FLG_PADRAO", SqlDbType.Char).Value = "S";
                cmdInsConsBancoCappta.ExecuteNonQuery();
                connInsConsBancoCappta.Close();
                connInsConsBancoCappta.Dispose();
            }
            catch
            {
                sMarketplace = "0";
            }
        }
        return sCodigoMarketplace;
    }

    public static string VerificaEstabelecimento(string sMarketplace, string sEstabelecimento)
    {
        string sCodigoEstabelecimento = "0";

        // Inicio - Verifica primeiro na base de dados se este CNPJCPF existe para  agilizar a busca e velocidade de processamento

        SqlConnection mySelCadastro = new SqlConnection(Funcoes.conexao());
        mySelCadastro.Open();
        SqlCommand cmdSelCadastro = new SqlCommand("dbo.stp_pessoas_fj_ins", mySelCadastro);
        cmdSelCadastro.CommandType = CommandType.StoredProcedure;
        cmdSelCadastro.Parameters.Add("@flg_operacao", SqlDbType.Char).Value = "H";
        cmdSelCadastro.Parameters.Add("@COD_ID_PESSOA_LICENCIADO", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["LICENCIADO"].ToString());
        cmdSelCadastro.Parameters.Add("@NOM_CNPJ", SqlDbType.VarChar).Value = sEstabelecimento.ToString();
        cmdSelCadastro.Parameters.Add("@FLG_TIPO", SqlDbType.Char).Value = "E";
        SqlDataReader ReaderCadastro = cmdSelCadastro.ExecuteReader();
        while (ReaderCadastro.Read())
        {
            sCodigoEstabelecimento = ReaderCadastro["COD_ID"].ToString();
        }

        // Fim - Verifica primeiro na base de dados se este CNPJCPF existe para  agilizar a busca e velocidade de processamento

        if (Funcoes.strToInt(sCodigoEstabelecimento.ToString()) <= 0)
        {


            if (sEstabelecimento.ToString().Trim() != "")
            {
                string jsonRetornoCadastro = hubcappta.ConsultarLojista(sEstabelecimento.ToString(), sMarketplace.ToString());

                JObject oEstabelecimento = JObject.Parse(jsonRetornoCadastro);

                try
                {
                    //txtJsonCappta.Text = txtJsonCappta.Text + oEstabelecimento["resellerDocument"].ToString();
                    // ********************************************************************
                    // Localizar cadastro do Representante
                    // ********************************************************************
                    string sIDMarketplace = "0";
                    SqlConnection mySelCadastroVerifica = new SqlConnection(Funcoes.conexao());
                    mySelCadastroVerifica.Open();
                    SqlCommand cmdSelCadastroVerifica = new SqlCommand("dbo.stp_pessoas_fj_ins", mySelCadastroVerifica);
                    cmdSelCadastroVerifica.CommandType = CommandType.StoredProcedure;
                    cmdSelCadastroVerifica.Parameters.Add("@flg_operacao", SqlDbType.Char).Value = "H";
                    cmdSelCadastroVerifica.Parameters.Add("@NOM_CNPJ", SqlDbType.VarChar).Value = sMarketplace.ToString();
                    cmdSelCadastroVerifica.Parameters.Add("@COD_ID_PESSOA_LICENCIADO", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["LICENCIADO"].ToString());
                    cmdSelCadastroVerifica.Parameters.Add("@FLG_TIPO", SqlDbType.Char).Value = "M";

                    SqlDataReader ReaderCadastroVerifica = cmdSelCadastroVerifica.ExecuteReader();
                    while (ReaderCadastroVerifica.Read())
                    {
                        sIDMarketplace = ReaderCadastroVerifica["COD_ID"].ToString();
                    }

                    SqlConnection connInsCons = new SqlConnection(Funcoes.conexao());
                    connInsCons.Open();
                    SqlCommand cmdInsCons = new SqlCommand("dbo.stp_pessoas_fj_ins", connInsCons);
                    cmdInsCons.CommandType = CommandType.StoredProcedure;
                    cmdInsCons.Parameters.Add("@flg_operacao", SqlDbType.Char).Value = 'I';
                    cmdInsCons.Parameters.Add("@FLG_ATIVO", SqlDbType.Char).Value = "S";
                    cmdInsCons.Parameters.Add("@FLG_INSERT_USUARIO", SqlDbType.Char).Value = "S";
                    cmdInsCons.Parameters.Add("@COD_ID_PESSOA_LICENCIADO", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["LICENCIADO"].ToString());
                    cmdInsCons.Parameters.Add("@DTA_DATA", SqlDbType.DateTime).Value = DateTime.Now;
                    cmdInsCons.Parameters.Add("@FLG_TIPO", SqlDbType.Char).Value = "E";


                    cmdInsCons.Parameters.Add("@FLG_TIPO_PESSOA", SqlDbType.VarChar).Value = (oEstabelecimento["merchant"]["document"].ToString().Trim().Length >= 14) ? "PJ" : "PF";
                    cmdInsCons.Parameters.Add("@FLG_PRESENCIAL", SqlDbType.Char).Value = "N";
                    //            cmdInsCons.Parameters.Add("@COD_ID_MARKETPLACE", SqlDbType.Int).Value = Funcoes.strToInt(ddlMarketplace.SelectedValue.ToString());

                    cmdInsCons.Parameters.Add("@COD_ID_MARKETPLACE", SqlDbType.Int).Value = Funcoes.strToInt(sIDMarketplace.ToString());


                    // Empresa
                    cmdInsCons.Parameters.Add("@NOM_RAZAOSOCIAL", SqlDbType.VarChar).Value = (oEstabelecimento["merchant"]["document"].ToString().Trim().Length >= 14) ? oEstabelecimento["merchant"]["companyName"].ToString().Trim() : oEstabelecimento["owner"]["name"].ToString().Trim();
                    cmdInsCons.Parameters.Add("@NOM_FANTASIA", SqlDbType.VarChar).Value = (oEstabelecimento["merchant"]["document"].ToString().Trim().Length >= 14) ? oEstabelecimento["merchant"]["tradingName"].ToString().Trim() : oEstabelecimento["owner"]["name"].ToString().Trim();
                    cmdInsCons.Parameters.Add("@NOM_CNPJ", SqlDbType.VarChar).Value = oEstabelecimento["merchant"]["document"].ToString().Trim();

                    cmdInsCons.Parameters.Add("@NUM_TELEFONE", SqlDbType.VarChar).Value = oEstabelecimento["owner"]["phone"].ToString().Trim();
                    cmdInsCons.Parameters.Add("@NOM_EMAIL_EMPRESA", SqlDbType.VarChar).Value = oEstabelecimento["owner"]["email"].ToString().Trim();
                    cmdInsCons.Parameters.Add("@COD_ID_MCC", SqlDbType.Int).Value = Funcoes.strToInt(oEstabelecimento["merchant"]["mccId"].ToString().Trim());
                    //cmdInsCons.Parameters.Add("@NOM_TIPO_EMPRESA", SqlDbType.VarChar).Value = ddlTipoEmpresa.SelectedValue.ToString();
                    cmdInsCons.Parameters.Add("@NUM_FATURAMENTO", SqlDbType.Float).Value = 0;
                    cmdInsCons.Parameters.Add("@NUM_PATRIMONIO", SqlDbType.Float).Value = 0;

                    //if (txtDataAbertura.Text.ToString().Trim() != "")
                    //{
                    //    cmdInsCons.Parameters.Add("@DTA_ABERTURA", SqlDbType.DateTime).Value = Convert.ToDateTime(txtDataAbertura.Text.ToString());
                    //}

                    // Endereço
                    cmdInsCons.Parameters.Add("@NOM_ENDERECO", SqlDbType.VarChar).Value = oEstabelecimento["address"]["streetName"].ToString().Trim();
                    cmdInsCons.Parameters.Add("@NOM_NUMERO", SqlDbType.VarChar).Value = oEstabelecimento["address"]["houseNumber"].ToString().Trim();
                    cmdInsCons.Parameters.Add("@NOM_COMPLEMENTO", SqlDbType.VarChar).Value = oEstabelecimento["address"]["complement"].ToString().Trim();
                    cmdInsCons.Parameters.Add("@NOM_BAIRRO", SqlDbType.VarChar).Value = oEstabelecimento["address"]["neighborhood"].ToString().Trim();
                    cmdInsCons.Parameters.Add("@NOM_CIDADE", SqlDbType.VarChar).Value = oEstabelecimento["address"]["city"].ToString().Trim();
                    cmdInsCons.Parameters.Add("@NOM_UF", SqlDbType.VarChar).Value = oEstabelecimento["address"]["state"].ToString().Trim();
                    cmdInsCons.Parameters.Add("@NOM_CEP", SqlDbType.VarChar).Value = oEstabelecimento["address"]["postalCode"].ToString().Trim();
                    cmdInsCons.Parameters.Add("@NOM_PAIS", SqlDbType.VarChar).Value = "BR";

                    // Responsável
                    cmdInsCons.Parameters.Add("@NOM_NOME", SqlDbType.VarChar).Value = oEstabelecimento["owner"]["name"].ToString().Trim();
                    cmdInsCons.Parameters.Add("@NOM_SOBRENOME", SqlDbType.VarChar).Value = oEstabelecimento["owner"]["phone"].ToString().Trim();
                    cmdInsCons.Parameters.Add("@NOM_CPF", SqlDbType.VarChar).Value = oEstabelecimento["owner"]["cpf"].ToString().Trim();

                    //if (txtNascimento.Text.ToString().Trim() != "")
                    //{
                    //    cmdInsCons.Parameters.Add("@DTA_ANIVERSARIO", SqlDbType.DateTime).Value = Convert.ToDateTime(txtNascimento.Text.ToString());
                    //}

                    //cmdInsCons.Parameters.Add("@NOM_MAE", SqlDbType.VarChar).Value = txtNomeMae.Text.ToString();
                    //cmdInsCons.Parameters.Add("@NUM_RENDA_MENSAL", SqlDbType.Float).Value = Funcoes.strToDouble(txtRendaMensalPF.Text.ToString());
                    cmdInsCons.Parameters.Add("@NOM_EMAIL", SqlDbType.VarChar).Value = oEstabelecimento["owner"]["email"].ToString().Trim();
                    cmdInsCons.Parameters.Add("@NOM_CELULAR", SqlDbType.VarChar).Value = oEstabelecimento["owner"]["mobilePhone"].ToString().Trim();
                    //cmdInsCons.Parameters.Add("@FLG_POLITICAMENTE", SqlDbType.Char).Value = ddlPoliticamenteExposta.SelectedValue.ToString();

                    // Usuário
                    cmdInsCons.Parameters.Add("@NOM_NOME_USUARIO", SqlDbType.VarChar).Value = oEstabelecimento["owner"]["name"].ToString().Trim();
                    cmdInsCons.Parameters.Add("@NOM_LOGIN", SqlDbType.VarChar).Value = oEstabelecimento["owner"]["email"].ToString().Trim();
                    cmdInsCons.Parameters.Add("@NOM_SENHA", SqlDbType.VarChar).Value = oEstabelecimento["merchant"]["document"].ToString().Trim();

                    sCodigoEstabelecimento = cmdInsCons.ExecuteScalar().ToString();

                    //cmdInsCons.ExecuteNonQuery();
                    connInsCons.Close();
                    connInsCons.Dispose();

                    // Dados Onboarding Cappta
                    SqlConnection connInsConsDadosCappta = new SqlConnection(Funcoes.conexao());
                    connInsConsDadosCappta.Open();
                    SqlCommand cmdInsConsDadosCappta = new SqlCommand("dbo.stp_pessoas_fj_cappta_ins", connInsConsDadosCappta);
                    cmdInsConsDadosCappta.CommandType = CommandType.StoredProcedure;
                    cmdInsConsDadosCappta.Parameters.Add("@flg_operacao", SqlDbType.Char).Value = 'I';
                    cmdInsConsDadosCappta.Parameters.Add("@COD_ID_PESSOA_LICENCIADO", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["LICENCIADO"].ToString());
                    cmdInsConsDadosCappta.Parameters.Add("@COD_ID_PESSOAS_FJ", SqlDbType.Int).Value = Funcoes.strToInt(sCodigoEstabelecimento.ToString());

                    cmdInsConsDadosCappta.Parameters.Add("@NUM_TOKEN_CAPPTA", SqlDbType.VarChar).Value = oEstabelecimento["merchant"]["document"].ToString();
                    cmdInsConsDadosCappta.Parameters.Add("@FLG_STATUS_CAPPTA", SqlDbType.VarChar).Value = oEstabelecimento["statusDescription"].ToString();
                    cmdInsConsDadosCappta.Parameters.Add("@FLG_CAPPTA", SqlDbType.Char).Value = "S";
                    cmdInsConsDadosCappta.Parameters.Add("@DES_JSON_CAPPTA", SqlDbType.VarChar).Value = jsonRetornoCadastro.ToString();
                    cmdInsConsDadosCappta.Parameters.Add("@COD_ID_NATUREZA_CAPPTA", SqlDbType.Int).Value = (oEstabelecimento["merchant"]["legalNatureId"].ToString().Trim() != "") ? Funcoes.strToInt(oEstabelecimento["merchant"]["legalNatureId"].ToString()) : 0;

                    cmdInsConsDadosCappta.ExecuteNonQuery();
                    connInsConsDadosCappta.Close();
                    connInsConsDadosCappta.Dispose();

                    // Dados Bancários Cappta
                    SqlConnection connInsConsBancoCappta = new SqlConnection(Funcoes.conexao());
                    connInsConsBancoCappta.Open();
                    SqlCommand cmdInsConsBancoCappta = new SqlCommand("dbo.stp_pessoas_fj_contas_ins", connInsConsBancoCappta);
                    cmdInsConsBancoCappta.CommandType = CommandType.StoredProcedure;
                    cmdInsConsBancoCappta.Parameters.Add("@flg_operacao", SqlDbType.Char).Value = 'I';
                    cmdInsConsBancoCappta.Parameters.Add("@COD_ID_PESSOA_LICENCIADO", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["LICENCIADO"].ToString());
                    cmdInsConsBancoCappta.Parameters.Add("@COD_ID_PESSOAS_FJ", SqlDbType.Int).Value = Funcoes.strToInt(sCodigoEstabelecimento.ToString());

                    cmdInsConsBancoCappta.Parameters.Add("@NOM_CODIGO_BANCO", SqlDbType.VarChar).Value = oEstabelecimento["bankAccount"]["bankCode"].ToString();
                    cmdInsConsBancoCappta.Parameters.Add("@NOM_TIPO_BANCO", SqlDbType.VarChar).Value = (oEstabelecimento["bankAccount"]["accountType"].ToString().Trim() == "1") ? "C" : "P";
                    cmdInsConsBancoCappta.Parameters.Add("@NOM_NUMERO_AGENCIA_BANCO", SqlDbType.VarChar).Value = oEstabelecimento["bankAccount"]["branch"].ToString();
                    cmdInsConsBancoCappta.Parameters.Add("@NOM_NUMERO_DIGITO_AGENCIA_BANCO", SqlDbType.VarChar).Value = "";
                    cmdInsConsBancoCappta.Parameters.Add("@NOM_NUMERO_CONTA_BANCO", SqlDbType.VarChar).Value = oEstabelecimento["bankAccount"]["account"].ToString();
                    cmdInsConsBancoCappta.Parameters.Add("@NOM_NUMERO_DIGITO_CONTA_BANCO", SqlDbType.VarChar).Value = "";
                    cmdInsConsBancoCappta.Parameters.Add("@FLG_PADRAO", SqlDbType.Char).Value = "S";

                    cmdInsConsBancoCappta.ExecuteNonQuery();
                    connInsConsBancoCappta.Close();
                    connInsConsBancoCappta.Dispose();

                }
                catch
                {
                    sCodigoEstabelecimento = "0";
                }

            }
        }
        return sCodigoEstabelecimento;
    }


}