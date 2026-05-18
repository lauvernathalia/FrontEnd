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
/// Summary description for zoop
/// </summary>
// COMPRADOR CONSULTA E CADASTRO

public class dadosTransferencia
{
    public class Transferencia
    {
        public int amount { get; set; }
        public string statement_descriptor { get; set; }
        public string description { get; set; }
    }
}

public class dadosTransferenciaP2P
{
    public class TransferenciaP2P
    {
        public int amount { get; set; }
        public string description { get; set; }
        public string reference_id { get; set; }
        public bool is_idempotency { get; set; }
    }
}


public class dadosComprador
{
    public class Comprador
    {
        public string first_name { get; set; }
        public string last_name { get; set; }
        public string email { get; set; }
        public string phone_number { get; set; }
        public string taxpayer_id { get; set; }
        public address address { get; set; }

    }
    public class address
    {
        public string line1 { get; set; }
        public string line2 { get; set; }
        public string line3 { get; set; }
        public string neighborhood { get; set; }
        public string city { get; set; }
        public string state { get; set; }
        public string postal_code { get; set; }
        public string country_code { get; set; }
    }
}
// TRANSACOES CONSULTA CADASTRO
public class DadosTransacao
{
    public class TransacaoCard
    {
        public string on_behalf_of { get; set; }
        public string description { get; set; }
        public string payment_type { get; set; }
        public bool capture { get; set; }
        public string reference_id { get; set; }
        public source source { get; set; }
        public installment_plan installment_plan { get; set; }
    }

    public class TransacaoPix
    {
        public string on_behalf_of { get; set; }
        public string description { get; set; }
        public string currency { get; set; }
        public int amount { get; set; }
        public string payment_type { get; set; }
        public string pix_expiration_date_time { get; set; }
    }

    public class TransacaoBoleto
    {

        public string on_behalf_of { get; set; }

        public string customer { get; set; }
        public int amount { get; set; }
        public string currency { get; set; }
        public string description { get; set; }
        public string reference_id { get; set; }
        public string payment_type { get; set; }
        public string logo { get; set; }
        public payment_method payment_method { get; set; }
    }

    public class payment_method
    {
        public string expiration_date { get; set; }
        public string payment_limit_date { get; set; }
        public string[] body_instructions { get; set; }
        public billing_instructions billing_instructions { get; set; }
    }

    public class billing_instructions
    {
        public late_fee late_fee { get; set; }
        public interest interest { get; set; }
        public discount[] discount { get; set; }
    }

    public class late_fee
    {
        public string mode { get; set; }
        public int amount { get; set; }
        public double percentage { get; set; }
        public string start_date { get; set; }
    }

    public class interest
    {
        public string mode { get; set; }
        public int amount { get; set; }
        public double percentage { get; set; }
        public string start_date { get; set; }
    }

    public class discount
    {
        public string mode { get; set; }
        public int amount { get; set; }
        public string limit_date { get; set; }

    }

    public class source
    {
        public string usage { get; set; }
        public int amount { get; set; }
        public string currency { get; set; }
        public string type { get; set; }
        public card card { get; set; }
    }

    public class installment_plan
    {
        public int number_installments { get; set; }
    }

    public class card
    {
        public string card_number { get; set; }
        public string holder_name { get; set; }
        public string expiration_month { get; set; }
        public string expiration_year { get; set; }
        public string security_code { get; set; }
    }
}




public class zoop
{

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

    public static string marketplace()
    {
        string sPadrao = "17583e0c32244a3e917f27c7e2a6d997";
        try
        {
            SqlConnection mySelCadastro = new SqlConnection(Funcoes.conexao());
            mySelCadastro.Open();
            SqlCommand cmdSelCadastro = new SqlCommand("dbo.stp_pessoas_fj_integracoes_chaves_ins", mySelCadastro);
            cmdSelCadastro.CommandType = CommandType.StoredProcedure;
            cmdSelCadastro.Parameters.Add("@flg_operacao", SqlDbType.Char).Value = "L";
            cmdSelCadastro.Parameters.Add("@COD_ID_INTEGRACOES", SqlDbType.Int).Value = 1;
            cmdSelCadastro.Parameters.Add("@COD_ID_PESSOA_LICENCIADO", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["LICENCIADO"].ToString());
            SqlDataReader ReaderCadastro = cmdSelCadastro.ExecuteReader();
            while (ReaderCadastro.Read())
            {
                sPadrao = ReaderCadastro["NOM_ID"].ToString().Trim();
            }
        }
        catch
        {

        }

        return sPadrao.ToString();
    }
    public static string Keymarketplace()
    {
        string sPadrao = "zpk_prod_Nxo9anXlr0i9M8aUZMcWY2Ck";

        try
        {
            SqlConnection mySelCadastro = new SqlConnection(Funcoes.conexao());
            mySelCadastro.Open();
            SqlCommand cmdSelCadastro = new SqlCommand("dbo.stp_pessoas_fj_integracoes_chaves_ins", mySelCadastro);
            cmdSelCadastro.CommandType = CommandType.StoredProcedure;
            cmdSelCadastro.Parameters.Add("@flg_operacao", SqlDbType.Char).Value = "L";
            cmdSelCadastro.Parameters.Add("@COD_ID_INTEGRACOES", SqlDbType.Int).Value = 1;
            cmdSelCadastro.Parameters.Add("@COD_ID_PESSOA_LICENCIADO", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["LICENCIADO"].ToString());
            SqlDataReader ReaderCadastro = cmdSelCadastro.ExecuteReader();
            while (ReaderCadastro.Read())
            {
                sPadrao = ReaderCadastro["NOM_KEY"].ToString().Trim();
            }
        }
        catch
        {

        }
        return sPadrao;
    }

    public static string OperacaoZoop(string operacao)
    {
        string sOperacaoZoop = "";
        if (operacao == "planos") { sOperacaoZoop = "/plans"; }
        return sOperacaoZoop;
    }

    public static string VersaoZoop(int versao)
    {
        string sVersaoZoop = "";
        if (versao == 1) { sVersaoZoop = "https://api.zoop.ws/v1/marketplaces/"; }
        if (versao == 2) { sVersaoZoop = "https://api.zoop.ws/v2/marketplaces/"; }
        return sVersaoZoop;
    }


    public static string planos_referencia(string detalhe)
    {

        ServicePointManager.Expect100Continue = true;
        ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12
        | SecurityProtocolType.Tls11
        | SecurityProtocolType.Tls
        | SecurityProtocolType.Ssl3;

        var byteArray = Encoding.ASCII.GetBytes(Keymarketplace() + ":" + "");
        string encodeString = Convert.ToBase64String(byteArray);
        
        try
            {
                string URL = zoop.VersaoZoop(1).ToString() + marketplace() + OperacaoZoop("planos").ToString();

                if (detalhe.ToString().Trim() != "")
                {
                    detalhe = "/" + detalhe;
                }
                var myUri = new Uri(URL + detalhe.ToString().Trim());
                
                var myWebRequest = WebRequest.Create(myUri);
                var myHttpWebRequest = (HttpWebRequest)myWebRequest;
                myHttpWebRequest.PreAuthenticate = true;
                myHttpWebRequest.Headers.Add("Authorization", "Basic " + encodeString.ToString() + "");
                myHttpWebRequest.Accept = "application/json";

                var myWebResponse = myWebRequest.GetResponse();
                var responseStream = myWebResponse.GetResponseStream();

                StreamReader myStreamReader = new StreamReader(responseStream, Encoding.UTF8);
                string json = myStreamReader.ReadToEnd();
                
                return json;
                
                //return myUri.ToString();
            }
            catch
            {
                return "";
            }


        return "";
    }

    public static string vendedores(string tipo, string CPFCNPJ, string json)
    {
        // https://api.zoop.ws/v1/marketplaces/{marketplace_id}/sellers/search

        // CONFIGURAÇÕES ACESSO ZOOP

        ServicePointManager.Expect100Continue = true;
        ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12
        | SecurityProtocolType.Tls11
        | SecurityProtocolType.Tls
        | SecurityProtocolType.Ssl3;

        var byteArray = Encoding.ASCII.GetBytes(Keymarketplace() + ":" + "");
        string encodeString = Convert.ToBase64String(byteArray);

        var URL = "";
        var PARAMS = "";
        var ID = "";

        if (tipo == "P")
        {
            try
            {
                if (CPFCNPJ.ToString().Length <= 11)
                {
                    PARAMS = "?taxpayer_id=" + CPFCNPJ.ToString();
                }
                if (CPFCNPJ.ToString().Length > 11)
                {
                    PARAMS = "?ein=" + CPFCNPJ.ToString();
                }

                URL = "https://api.zoop.ws/v1/marketplaces/" + HttpContext.Current.Session["IDMARKETPLACE"].ToString() + "/sellers/search";

                var myUri = new Uri(URL + PARAMS);
                var myWebRequest = WebRequest.Create(myUri);
                var myHttpWebRequest = (HttpWebRequest)myWebRequest;
                myHttpWebRequest.PreAuthenticate = true;
                myHttpWebRequest.Headers.Add("Authorization", "Basic " + encodeString.ToString() + "");
                myHttpWebRequest.Accept = "application/json";

                var myWebResponse = myWebRequest.GetResponse();
                var responseStream = myWebResponse.GetResponseStream();

                StreamReader myStreamReader = new StreamReader(responseStream, Encoding.UTF8);
                json = myStreamReader.ReadToEnd();

                JObject o = JObject.Parse(json);
                if (CPFCNPJ.ToString().Length <= 11)
                {
                    return o["id"].ToString() + "/" + o["first_name"].ToString() + " " + o["last_name"].ToString() + "/" + o["email"].ToString(); 
                }
                if (CPFCNPJ.ToString().Length > 11)
                {
                    return o["id"].ToString() + "/" + o["business_name"].ToString() + "/" + o["business_email"].ToString();
                }
            }
            catch
            {
                return "";
            }
        }
        return "";
    }

    public static string status_code(string code)
    {
        if (code=="200"){ return "Operação ocorreu com sucesso!"; }
        if (code=="201"){ return "OK"; }
        if (code=="304"){ return ""; }
        if (code=="400"){ return "Erro ao processar os dados!"; }
        if (code=="401"){ return "Não autorizado!"; }
        if (code == "402") { return "Operacao ocorreu com sucesso porém houve erro ao processar as informações ou as mesmas são inválidas! Favor verificar se os dados digitados estão corretos"; }
        if (code=="403"){ return "Operacao ocorreu com sucesso porém foi recusada por acesso não permitido"; }
        if (code=="404"){ return "Operação inválida"; }
        if (code=="500"){ return "Servidor com problemas! Tente mais tarde novamente."; }
        if (code=="502"){ return "Servidor fora do ar ou em manutenção"; }
        return "";
/*
        200	OK	Tudo funcionou conforme o esperado.
        201	Created	A requisição foi bem sucedida e um novo recurso foi criado.
        304	Not Modified	Não havia dados novos para retornar.
        400	Bad Request	A requisição foi invalida ou não atingiu o servidor. Muitas vezes, falta um parâmetro obrigatório.
        401	Unauthorized	As credenciais de autenticação estavam faltando ou foram incorretas.
        402	Request Failed	Os parâmetros foram válidos mas a requisição falhou.
        403	Forbidden	A requisição foi ok, mas foi recusado ou o acesso não foi permitido. Uma mensagem de erro que acompanha a mensagem explica o porquê.
        404	Not Found	A URI solicitada é inválida ou o recurso solicitado, como por exemplo, um vendedor não existe ou foi excluído.
        500	Internal Server Error	Algo está quebrado. Por favor, assegure-se de que a equipe Zoop esteja investigando.
        502	Bad Gateway	A Zoop caiu ou está sendo atualizada.
*/

    }



    public static string transacao(string tipo, string codigo, string json)
    {
        // CONFIGURAÇÕES ACESSO ZOOP

        ServicePointManager.Expect100Continue = true;
        ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12
        | SecurityProtocolType.Tls11
        | SecurityProtocolType.Tls
        | SecurityProtocolType.Ssl3;

        var byteArray = Encoding.ASCII.GetBytes(Keymarketplace() + ":" + "");
        string encodeString = Convert.ToBase64String(byteArray);

        var URL = "";
        var PARAMS = "";
        var ID = "";

        // Inclusão
        if (tipo == "I")
        {
            PARAMS = "";
            URL = "https://api.zoop.ws/v1/marketplaces/" + marketplace() + "/transactions";

            var myUri = new Uri(URL + PARAMS);

            var myWebRequest = WebRequest.Create(myUri);
            var myHttpWebRequest = (HttpWebRequest)myWebRequest;
            myHttpWebRequest.ContentType = "application/json";
            myHttpWebRequest.Accept = "application/json";
            myHttpWebRequest.Method = "POST";
            myHttpWebRequest.PreAuthenticate = true;
            myHttpWebRequest.Headers.Add("Authorization", "Basic " + encodeString.ToString() + "");

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

            JObject o = JObject.Parse(json);

            return o["id"].ToString();

            }
            catch (WebException ex)
            {
                var resp = new StreamReader(ex.Response.GetResponseStream()).ReadToEnd();
                //JObject o = JObject.Parse(resp);
                //return o["error"]["status"].ToString() + "/" + o["error"]["status_code"].ToString();
                return resp;
            }


            //    return "";

        }

        // Busca
        if (tipo == "P")
        {

            try
            {
                PARAMS = "" + codigo.ToString();
                URL = "https://api.zoop.ws/v1/marketplaces/" + marketplace() + "/transactions/";

                var myUri = new Uri(URL + PARAMS);
                var myWebRequest = WebRequest.Create(myUri);
                var myHttpWebRequest = (HttpWebRequest)myWebRequest;
                myHttpWebRequest.PreAuthenticate = true;
                myHttpWebRequest.Headers.Add("Authorization", "Basic " + encodeString.ToString() + "");
                myHttpWebRequest.Accept = "application/json";

                var myWebResponse = myWebRequest.GetResponse();
                var responseStream = myWebResponse.GetResponseStream();

                StreamReader myStreamReader = new StreamReader(responseStream, Encoding.UTF8);
                json = myStreamReader.ReadToEnd();

                JObject o = JObject.Parse(json);

                return json;
            }
            catch
            {
                return "";
            }

        }

        return "";
    }

    public static string DetalhesRecibo(string sRecibo)
    {
        ServicePointManager.Expect100Continue = true;
        ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12
        | SecurityProtocolType.Tls11
        | SecurityProtocolType.Tls
        | SecurityProtocolType.Ssl3;

        var byteArray = Encoding.ASCII.GetBytes(Keymarketplace() + ":" + "");
        string encodeString = Convert.ToBase64String(byteArray);

        
        try
        {
            string PARAMS = "" + sRecibo.ToString();
            string URL = "https://api.zoop.ws/v1/marketplaces/" + marketplace() + "/receipts/";

            var myUri = new Uri(URL + PARAMS);
            var myWebRequest = WebRequest.Create(myUri);
            var myHttpWebRequest = (HttpWebRequest)myWebRequest;
            myHttpWebRequest.PreAuthenticate = true;
            myHttpWebRequest.Headers.Add("Authorization", "Basic " + encodeString.ToString() + "");
            myHttpWebRequest.Accept = "application/json";

            var myWebResponse = myWebRequest.GetResponse();
            var responseStream = myWebResponse.GetResponseStream();

            StreamReader myStreamReader = new StreamReader(responseStream, Encoding.UTF8);
            string json = myStreamReader.ReadToEnd();

            JObject o = JObject.Parse(json);

            return json;
        }
        catch
        {
            return "";
        }

    }


    public static string DetalhesRecebiveis(string sTransacao)
    {
        ServicePointManager.Expect100Continue = true;
        ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12
        | SecurityProtocolType.Tls11
        | SecurityProtocolType.Tls
        | SecurityProtocolType.Ssl3;

        var byteArray = Encoding.ASCII.GetBytes(Keymarketplace() + ":" + "");
        string encodeString = Convert.ToBase64String(byteArray);
        try
        {
            string PARAMS = "" + sTransacao.ToString() + "/receivables";
            string URL = "https://api.zoop.ws/v1/marketplaces/" + marketplace() + "/transactions/";

            var myUri = new Uri(URL + PARAMS);
            var myWebRequest = WebRequest.Create(myUri);
            var myHttpWebRequest = (HttpWebRequest)myWebRequest;
            myHttpWebRequest.PreAuthenticate = true;
            myHttpWebRequest.Headers.Add("Authorization", "Basic " + encodeString.ToString() + "");
            myHttpWebRequest.Accept = "application/json";

            var myWebResponse = myWebRequest.GetResponse();
            var responseStream = myWebResponse.GetResponseStream();

            StreamReader myStreamReader = new StreamReader(responseStream, Encoding.UTF8);
            string json = myStreamReader.ReadToEnd();

            JObject o = JObject.Parse(json);

            return json;
        }
        catch
        {
            return "";
        }

    }

    public static HttpResponseResult TransacaoSplitPresencial(string json, string IDTransacao)
    {
        ServicePointManager.Expect100Continue = true;
        ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12
            | SecurityProtocolType.Tls11
            | SecurityProtocolType.Tls
            | SecurityProtocolType.Ssl3;

        var byteArray = Encoding.ASCII.GetBytes(Keymarketplace() + ":" + "");
        string encodeString = Convert.ToBase64String(byteArray);


        var url = "https://api.zoop.ws/v1/marketplaces/" + marketplace().ToString() + "/transactions/" + IDTransacao.ToString() + "/split_rules";

        var myUri = new Uri(url);
        var myHttpWebRequest = (HttpWebRequest)WebRequest.Create(myUri);

        myHttpWebRequest.ContentType = "application/json";
        myHttpWebRequest.Accept = "application/json";
        myHttpWebRequest.Method = "POST";
        myHttpWebRequest.PreAuthenticate = true;
        myHttpWebRequest.Headers.Add("Authorization", "Basic " + encodeString);

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

    public static string DetalhesTerminal(string sTerminal)
    {
        ServicePointManager.Expect100Continue = true;
        ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12
        | SecurityProtocolType.Tls11
        | SecurityProtocolType.Tls
        | SecurityProtocolType.Ssl3;

        var byteArray = Encoding.ASCII.GetBytes(Keymarketplace() + ":" + "");
        string encodeString = Convert.ToBase64String(byteArray);


        try
        {
            string PARAMS = "" + sTerminal.ToString();
            string URL = "https://api.zoop.ws/v1/card-present/terminals/";

            var myUri = new Uri(URL + PARAMS);
            var myWebRequest = WebRequest.Create(myUri);
            var myHttpWebRequest = (HttpWebRequest)myWebRequest;
            myHttpWebRequest.PreAuthenticate = true;
            myHttpWebRequest.Headers.Add("Authorization", "Basic " + encodeString.ToString() + "");
            myHttpWebRequest.Accept = "application/json";

            var myWebResponse = myWebRequest.GetResponse();
            var responseStream = myWebResponse.GetResponseStream();

            StreamReader myStreamReader = new StreamReader(responseStream, Encoding.UTF8);
            string json = myStreamReader.ReadToEnd();

            JObject o = JObject.Parse(json);

            return json;
        }
        catch
        {
            return "";
        }

    }



    public static string transferencia(string tipo, string codigo, string json)
    {
        // CONFIGURAÇÕES ACESSO ZOOP

        ServicePointManager.Expect100Continue = true;
        ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12
        | SecurityProtocolType.Tls11
        | SecurityProtocolType.Tls
        | SecurityProtocolType.Ssl3;

        var byteArray = Encoding.ASCII.GetBytes(Keymarketplace() + ":" + "");
        string encodeString = Convert.ToBase64String(byteArray);

        var URL = "";
        var PARAMS = "";
        var ID = "";

        // Inclusão
        if (tipo == "I")
        {
            PARAMS = "";
            URL = "https://api.zoop.ws/v1/marketplaces/" + HttpContext.Current.Session["IDMARKETPLACE"].ToString() + "/bank_accounts/" + codigo.ToString() + "/transfers";

            var myUri = new Uri(URL + PARAMS);

            var myWebRequest = WebRequest.Create(myUri);
            var myHttpWebRequest = (HttpWebRequest)myWebRequest;
            myHttpWebRequest.ContentType = "application/json";
            myHttpWebRequest.Accept = "application/json";
            myHttpWebRequest.Method = "POST";
            myHttpWebRequest.PreAuthenticate = true;
            myHttpWebRequest.Headers.Add("Authorization", "Basic " + encodeString.ToString() + "");

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
                JObject o = JObject.Parse(json);

                return o["id"].ToString();

            }
            catch (WebException ex)
            {
                var resp = new StreamReader(ex.Response.GetResponseStream()).ReadToEnd();
                return resp;
            }


            //    return "";

        }

        return "";
    }


    public static string transferenciaP2P(string tipo, string codigoPagador, string codigoRecebedor, string json)
    {
        // CONFIGURAÇÕES ACESSO ZOOP

        ServicePointManager.Expect100Continue = true;
        ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12
        | SecurityProtocolType.Tls11
        | SecurityProtocolType.Tls
        | SecurityProtocolType.Ssl3;

        var byteArray = Encoding.ASCII.GetBytes(Keymarketplace() + ":" + "");
        string encodeString = Convert.ToBase64String(byteArray);

        var URL = "";
        var PARAMS = "";
        var ID = "";

        // Inclusão
        if (tipo == "I")
        {
            PARAMS = "";

            URL = "https://api.zoop.ws/v2/marketplaces/" + HttpContext.Current.Session["IDMARKETPLACE"].ToString() + "/transfers/" + codigoPagador.ToString() + "/to/" + codigoRecebedor.ToString();
            var myUri = new Uri(URL + PARAMS);

            var myWebRequest = WebRequest.Create(myUri);
            var myHttpWebRequest = (HttpWebRequest)myWebRequest;
            myHttpWebRequest.ContentType = "application/json";
            myHttpWebRequest.Accept = "application/json";
            myHttpWebRequest.Method = "POST";
            myHttpWebRequest.PreAuthenticate = true;
            myHttpWebRequest.Headers.Add("Authorization", "Basic " + encodeString.ToString() + "");

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

                JObject o = JObject.Parse(json);

                return "Solicitação de saque realizada com sucesso!";//o["id"].ToString();

            }
            catch (System.Exception sysEx)
            {
                return "Ocorreu um erro ao tentar realizar a soilicitação de saque! Verifique se você possui autorização para realizar esta operação.(" + sysEx.Message.ToString() + ")";
            }
            return "";
        }

        return "";
    }

    public static string compradores(string tipo, string CPFCNPJ, string json)
    {
        // CONFIGURAÇÕES ACESSO ZOOP

        ServicePointManager.Expect100Continue = true;
        ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12
        | SecurityProtocolType.Tls11
        | SecurityProtocolType.Tls
        | SecurityProtocolType.Ssl3;

        var byteArray = Encoding.ASCII.GetBytes(Keymarketplace() + ":" + "");
        string encodeString = Convert.ToBase64String(byteArray);

        var URL = "";
        var PARAMS = "";
        var ID = "";

        // Inclusão
        if (tipo == "I")
        {
            PARAMS = "";
            URL = "https://api.zoop.ws/v1/marketplaces/" + HttpContext.Current.Session["IDMARKETPLACE"].ToString() + "/buyers";

            var myUri = new Uri(URL + PARAMS);

            var myWebRequest = WebRequest.Create(myUri);
            var myHttpWebRequest = (HttpWebRequest)myWebRequest;
            myHttpWebRequest.ContentType = "application/json";
            myHttpWebRequest.Accept = "application/json";
            myHttpWebRequest.Method = "POST";
            myHttpWebRequest.PreAuthenticate = true;
            myHttpWebRequest.Headers.Add("Authorization", "Basic " + encodeString.ToString() + "");

            using (var streamWriter = new StreamWriter(myHttpWebRequest.GetRequestStream()))
            {
                streamWriter.Write(json);
                streamWriter.Flush();
            }
            //try
            //{
            var myWebResponse = myWebRequest.GetResponse();
            var responseStream = myWebResponse.GetResponseStream();

            StreamReader myStreamReader = new StreamReader(responseStream, Encoding.UTF8);
            json = myStreamReader.ReadToEnd();

            JObject o = JObject.Parse(json);

            return o["id"].ToString();

            //}
            //catch (WebException ex)
            //{
            //    var resp = new StreamReader(ex.Response.GetResponseStream()).ReadToEnd();
                //JObject o = JObject.Parse(resp);
            //}


            //    return "";

        }

        // Busca
        if (tipo == "P")
        {

            try
            {
                PARAMS = "?taxpayer_id=" + CPFCNPJ.ToString();
                URL = "https://api.zoop.ws/v1/marketplaces/" + HttpContext.Current.Session["IDMARKETPLACE"].ToString() + "/buyers/search";

                var myUri = new Uri(URL + PARAMS);
                var myWebRequest = WebRequest.Create(myUri);
                var myHttpWebRequest = (HttpWebRequest)myWebRequest;
                myHttpWebRequest.PreAuthenticate = true;
                myHttpWebRequest.Headers.Add("Authorization", "Basic " + encodeString.ToString() + "");
                myHttpWebRequest.Accept = "application/json";

                var myWebResponse = myWebRequest.GetResponse();
                var responseStream = myWebResponse.GetResponseStream();

                StreamReader myStreamReader = new StreamReader(responseStream, Encoding.UTF8);
                json = myStreamReader.ReadToEnd();

                JObject o = JObject.Parse(json);

                return o["id"].ToString() + "/" + o["first_name"].ToString()+"/"+ o["last_name"].ToString()+"/"+ o["taxpayer_id"].ToString();
            }
            catch
            {
                return "";
            }

        }

        return "";
    }

}