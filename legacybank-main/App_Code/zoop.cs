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
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Converters;

using System.Collections.Specialized;

using System.Security.Cryptography.X509Certificates;

/// <summary>
/// Summary description for zoop
/// </summary>
// COMPRADOR CONSULTA E CADASTRO

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

public class dadosTransferencia
{
    public class Transferencia
    {
        public int amount { get; set; }
        public string statement_descriptor { get; set; }
        public string description { get; set; }
    }
}

public class dadosPoliticaRecebimento
{
    public class PoliticaRecebimento
    {
        public string transfer_interval { get; set; }
        public int transfer_day { get; set; }
        public bool transfer_enabled { get; set; }
        public int minimum_transfer_value { get; set; }
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

public class dadosAssociarContaBancaria
{
    public class AssociarContaBancaria
    {
        public string customer { get; set; }
        public string token { get; set; }

    }
}

public class dadosParearTerminalZoop
{
    public class root
    {
        public string seller { get; set; }
        public bool marketplace_id { get; set; }
        public string token { get; set; }
        public bool isStaging { get; set; }
    }
}


// ----------------------------------------------------------------------------------------------------------------------------------//
// ESTORNAR TRANSAÇÃO 

public class dadosEstornarTransacao
{
    public class Estornar
    {
        public string on_behalf_of { get; set; }
        public int amount { get; set; }
        public void_rules[] void_rules { get; set; } 
    }

    public class void_rules
    {
        public string recipient { get; set; }
        public string split_rule { get; set; }
        public int amount { get; set; }
    }
}

//
// ----------------------------------------------------------------------------------------------------------------------------------//
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

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public three_d_secure three_d_secure { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public split_rules[] split_rules { get; set; }

    }

    public class three_d_secure
    {
        public string challenge_type { get; set; }
        public string ip_address { get; set; }
        public string user_agent { get; set; }
        public string on_failure { get; set; }
        public device device { get; set; }
    }

    public class device
    {
        public int color_depth { get; set; }
        public string type { get; set; }
        public bool java_enabled { get; set; }
        public string language { get; set; }
        public int screen_height { get; set; }
        public int screen_width { get; set; }
        public int time_zone_offset { get; set; }
    }



    public class TransacaoPix
    {
        public string on_behalf_of { get; set; }
        public string description { get; set; }
        public string currency { get; set; }
        public int amount { get; set; }
        public string payment_type { get; set; }
        public string pix_expiration_date_time { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public split_rules[] split_rules { get; set; }
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
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public split_rules[] split_rules { get; set; }
    }

    public class split_rules
    {
        public string recipient { get; set; }
        public bool liable { get; set; }
        public bool charge_processing_fee { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public int? amount { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public double? percentage { get; set; }
    }


    // Class BOLETO PIX
    public class TransacaoBoletoPix
    {
        public string on_behalf_of { get; set; }
        public string customer { get; set; }
        public int amount { get; set; }
        public string currency { get; set; }
        public string description { get; set; }
        public string reference_id { get; set; }
        public string payment_type { get; set; }
        public payment_method_pix payment_method { get; set; }
    }

    public class payment_method_pix
    {
        public string due_at { get; set; }
        public string payment_limit_at { get; set; }
        public billing_message_list[] billing_message_list { get; set; }
    }

    public class billing_message_list
    {
        public string message { get; set; }
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

public class AssociarPlano
{
    public class root
    {
        public string customer { get; set; }
        public string plan { get; set; }
        public int quantity { get; set; }
    }
}

public class CriarContaBancaria
{
    public class root
    {
        public string holder_name { get; set; }
        public string bank_code { get; set; }
        public string routing_number { get; set; }
        public string account_number { get; set; }
        public string taxpayer_id { get; set; }
        public string ein { get; set; }
        public string type { get; set; }
    }
}

public class AssociarContaBancaria
{
    public class root
    {
        public string customer { get; set; }
        public string token { get; set; }
    }
}

public class ZoopSplit
{
    public string recipient { get; set; }
    public bool charge_processing_fee { get; set; }
    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public int? amount { get; set; }
    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public double? percentage { get; set; }
}


public class zoop
{
    // Novas rotinas mais modernas implementadas ---------------------------------------------------------------------------------

    // configura resposta com code e content
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

    // dicionário de palavras

    public static class ErrorTranslator
    {
        private static readonly Dictionary<string, string> translations = new Dictionary<string, string>
        {
            { "Not Found", "Não encontrado" },
            { "invalid_request_error", "Erro de solicitação inválida" },
            { "resource_not_found", "Recurso não encontrado" },
            { "Sorry, the buyer you are trying to use does not exist or has been deleted.", "Desculpe, o comprador que você está tentando usar não existe ou foi excluído." },
            {"There was an error generating the boleto. The error was: Invalid value fields","Ocorreu um erro ao gerar o boleto. O erro foi: Campos de valor inválidos"},
            {"Request Failed","Falha na solicitação"},
            {"processing error","erro de processamento"},
            {"server api error","erro de api do servidor"},
            {"Sorry, the taxpayer_id you are trying to use does not exist or has been deleted","Desculpe, o Documento (CNPJ/CPF) que você está tentando usar não existe ou foi excluído"}
        };

        public static string Translate(string message)
        {
            return translations.ContainsKey(message) ? translations[message] : message;
        }
    }

    // Tradução

    public class ApiError
    {
        public string Status { get; set; }
        public int StatusCode { get; set; }
        public string Type { get; set; }
        public string Category { get; set; }
        public string Message { get; set; }
    }

    public static string TranslateApiError(string json)
    {
        try
        {
            JObject obj = JObject.Parse(json);

            // Verifica se a estrutura contém um erro
            if (obj["error"] != null)
            {
                ApiError error = JsonConvert.DeserializeObject<ApiError>(obj["error"].ToString());

                // Traduzindo os campos, se houver tradução disponível
                error.Status = ErrorTranslator.Translate(error.Status);
                error.Type = ErrorTranslator.Translate(error.Type);
                error.Category = ErrorTranslator.Translate(error.Category);
                error.Message = ErrorTranslator.Translate(error.Message);

                return JsonConvert.SerializeObject(new { error }, Formatting.Indented);
            }
        }
        catch (Exception ex)
        {
            return string.Format("Erro ao processar resposta: {0}", ex.Message);

        }

        return json;
    }

    // Nova Consulta

    public static HttpResponseResult DetalhesReciboZoop(string id)
    {
        ServicePointManager.Expect100Continue = true;
        ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12
            | SecurityProtocolType.Tls11
            | SecurityProtocolType.Tls
            | SecurityProtocolType.Ssl3;

        var byteArray = Encoding.ASCII.GetBytes(zoop.Keymarketplace() + ":" + "");
        string encodeString = Convert.ToBase64String(byteArray);

        var url = "https://api.zoop.ws/v1/marketplaces/" + marketplace().ToString() + "/receipts/" + id.ToString().Trim();
        var myHttpWebRequest = (HttpWebRequest)WebRequest.Create(new Uri(url));

        myHttpWebRequest.ClientCertificates.Add(CertificateHelper.ClientCertificate);
        myHttpWebRequest.ContentType = "application/json";
        myHttpWebRequest.Accept = "application/json";
        myHttpWebRequest.Method = "GET";
        myHttpWebRequest.PreAuthenticate = true;
        myHttpWebRequest.Headers.Add("Authorization", "Basic " + encodeString);
        myHttpWebRequest.Headers.Add("x-api-key", zoop.xapikey());        

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

            return new HttpResponseResult(500, "Erro inesperado: " + ex.Message);
        }
    }



    public static HttpResponseResult TransacaoBoletoBancario(string json)
    {
        ServicePointManager.Expect100Continue = true;
        ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12
            | SecurityProtocolType.Tls11
            | SecurityProtocolType.Tls
            | SecurityProtocolType.Ssl3;

        var byteArray = Encoding.ASCII.GetBytes(zoop.Keymarketplace() + ":" + "");
        string encodeString = Convert.ToBase64String(byteArray);


        var url = "https://api.zoop.ws/v1/marketplaces/" + zoop.marketplace().ToString() + "/transactions";
        var myUri = new Uri(url);
        var myHttpWebRequest = (HttpWebRequest)WebRequest.Create(myUri);
        
        myHttpWebRequest.ClientCertificates.Add(CertificateHelper.ClientCertificate);
        myHttpWebRequest.ContentType = "application/json";
        myHttpWebRequest.Accept = "application/json";
        myHttpWebRequest.Method = "POST";
        myHttpWebRequest.PreAuthenticate = true;
        myHttpWebRequest.Headers.Add("Authorization", "Basic " + encodeString);
        myHttpWebRequest.Headers.Add("x-api-key", zoop.xapikey());        

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

    public static HttpResponseResult TransacaoSplitPresencial(string json, string IDTransacao)
    {
        ServicePointManager.Expect100Continue = true;
        ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12
            | SecurityProtocolType.Tls11
            | SecurityProtocolType.Tls
            | SecurityProtocolType.Ssl3;

        var byteArray = Encoding.ASCII.GetBytes(zoop.Keymarketplace() + ":" + "");
        string encodeString = Convert.ToBase64String(byteArray);


        var url = "https://api.zoop.ws/v1/marketplaces/" + zoop.marketplace().ToString() + "/transactions/" + IDTransacao.ToString() + "/split_rules";

        var myUri = new Uri(url);
        var myHttpWebRequest = (HttpWebRequest)WebRequest.Create(myUri);
        myHttpWebRequest.ClientCertificates.Add(CertificateHelper.ClientCertificate);
        myHttpWebRequest.ContentType = "application/json";
        myHttpWebRequest.Accept = "application/json";
        myHttpWebRequest.Method = "POST";
        myHttpWebRequest.PreAuthenticate = true;
        myHttpWebRequest.Headers.Add("Authorization", "Basic " + encodeString);
        myHttpWebRequest.Headers.Add("x-api-key", zoop.xapikey()); 

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

    public static HttpResponseResult CancelarTransacaoSplitPresencial(string IDSplit, string IDTransacao)
    {
        ServicePointManager.Expect100Continue = true;
        ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12
            | SecurityProtocolType.Tls11
            | SecurityProtocolType.Tls
            | SecurityProtocolType.Ssl3;

        var byteArray = Encoding.ASCII.GetBytes(zoop.Keymarketplace() + ":" + "");
        string encodeString = Convert.ToBase64String(byteArray);


        var url = "https://api.zoop.ws/v1/marketplaces/" + zoop.marketplace().ToString() + "/transactions/" + IDTransacao.ToString() + "/split_rules/"+IDSplit.ToString();

        var myUri = new Uri(url);
        var myHttpWebRequest = (HttpWebRequest)WebRequest.Create(myUri);
        
        myHttpWebRequest.ClientCertificates.Add(CertificateHelper.ClientCertificate);

        myHttpWebRequest.ContentType = "application/json";
        myHttpWebRequest.Accept = "application/json";
        myHttpWebRequest.Method = "DELETE";
        myHttpWebRequest.PreAuthenticate = true;
        myHttpWebRequest.Headers.Add("Authorization", "Basic " + encodeString);
        myHttpWebRequest.Headers.Add("x-api-key", zoop.xapikey()); 

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


    public static HttpResponseResult TransacaoCartao(string json)
    {
        ServicePointManager.Expect100Continue = true;
        ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12
            | SecurityProtocolType.Tls11
            | SecurityProtocolType.Tls
            | SecurityProtocolType.Ssl3;

        var byteArray = Encoding.ASCII.GetBytes(zoop.Keymarketplace() + ":" + "");
        string encodeString = Convert.ToBase64String(byteArray);


        var url = "https://api.zoop.ws/v1/marketplaces/" + zoop.marketplace().ToString() + "/transactions";
        var myUri = new Uri(url);
        var myHttpWebRequest = (HttpWebRequest)WebRequest.Create(myUri);
        myHttpWebRequest.ClientCertificates.Add(CertificateHelper.ClientCertificate);
        myHttpWebRequest.ContentType = "application/json";
        myHttpWebRequest.Accept = "application/json";
        myHttpWebRequest.Method = "POST";
        myHttpWebRequest.PreAuthenticate = true;
        myHttpWebRequest.Headers.Add("Authorization", "Basic " + encodeString);
        myHttpWebRequest.Headers.Add("x-api-key", zoop.xapikey());   

        using (var streamWriter = new StreamWriter(myHttpWebRequest.GetRequestStream()))
        {
            streamWriter.Write(json);
            streamWriter.Flush();
        }

        //try
        //{
            using (var response = (HttpWebResponse)myHttpWebRequest.GetResponse())
            using (var reader = new StreamReader(response.GetResponseStream(), Encoding.UTF8))
            {
                string responseJson = reader.ReadToEnd();
                return new HttpResponseResult((int)response.StatusCode, responseJson);
            }
        //}
        //catch (WebException ex)
        //{
        //    if (ex.Response != null)
        //    {
        //        using (var response = (HttpWebResponse)ex.Response)
        //        using (var reader = new StreamReader(response.GetResponseStream()))
        //        {
        //            string errorJson = reader.ReadToEnd();
        //            return new HttpResponseResult((int)response.StatusCode, errorJson);
        //        }
        //    }

        //    return new HttpResponseResult(500, String.Format("Erro inesperado: {0}", ex.Message));
        //}
    }


    public static HttpResponseResult TransacaoPix(string json)
    {
        ServicePointManager.Expect100Continue = true;
        ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12
            | SecurityProtocolType.Tls11
            | SecurityProtocolType.Tls
            | SecurityProtocolType.Ssl3;

        var byteArray = Encoding.ASCII.GetBytes(zoop.Keymarketplace() + ":" + "");
        string encodeString = Convert.ToBase64String(byteArray);


        var url = "https://api.zoop.ws/v1/marketplaces/" + zoop.marketplace().ToString() + "/transactions";
        var myUri = new Uri(url);
        var myHttpWebRequest = (HttpWebRequest)WebRequest.Create(myUri);
        myHttpWebRequest.ClientCertificates.Add(CertificateHelper.ClientCertificate);
        myHttpWebRequest.ContentType = "application/json";
        myHttpWebRequest.Accept = "application/json";
        myHttpWebRequest.Method = "POST";
        myHttpWebRequest.PreAuthenticate = true;
        myHttpWebRequest.Headers.Add("Authorization", "Basic " + encodeString);
        myHttpWebRequest.Headers.Add("x-api-key", zoop.xapikey());   

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

    // Plano do EC

    public static HttpResponseResult ConsultarPlanoVendedor2(string id)
    {
        ServicePointManager.Expect100Continue = true;
        ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12
            | SecurityProtocolType.Tls11
            | SecurityProtocolType.Tls
            | SecurityProtocolType.Ssl3;

        var byteArray = Encoding.ASCII.GetBytes(zoop.Keymarketplace() + ":" + "");
        string encodeString = Convert.ToBase64String(byteArray);

        var url = "https://api.zoop.ws/v1/marketplaces/" + zoop.marketplace().ToString() + "/sellers/" + id + "/subscriptions";
        var myHttpWebRequest = (HttpWebRequest)WebRequest.Create(new Uri(url));
        myHttpWebRequest.ClientCertificates.Add(CertificateHelper.ClientCertificate);
        myHttpWebRequest.ContentType = "application/json";
        myHttpWebRequest.Accept = "application/json";
        myHttpWebRequest.Method = "GET";
        myHttpWebRequest.PreAuthenticate = true;
        myHttpWebRequest.Headers.Add("Authorization", "Basic " + encodeString);
        myHttpWebRequest.Headers.Add("x-api-key", zoop.xapikey()); 

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

            return new HttpResponseResult(500, "Erro inesperado: " + ex.Message);
        }
    }

    // **************************************************************************************************************************
    // CADASTRO E CREDENCIAMENTO 
    // **************************************************************************************************************************

    
    // COMPRADORES
    
    // Buscar Comprador na Adquirente
    public static string ConsultaCompradorZoop(string documento, string json)
    {
        zoop.HttpResponseResult resultado = zoop.ConsultarCompradorCNPJCPF(documento);
        if ((resultado.StatusCode == 200) || (resultado.StatusCode == 201))
        {
            string jsonResponse = resultado.Content;
            JObject oComprador = JObject.Parse(jsonResponse.ToString());

            zoop.HttpResponseResult alteracao = zoop.AlterarCompradorCNPJCPF(oComprador["id"].ToString(), json);
            return oComprador["id"].ToString();
        }
        else
        {
            // No caso de falha cadastrar o comprador
            zoop.HttpResponseResult alteracao = zoop.CadastrarCompradorCNPJCPF(json);

            if ((resultado.StatusCode == 200) || (resultado.StatusCode == 201))
            {
                string jsonResponse = resultado.Content;
                JObject oComprador = JObject.Parse(jsonResponse.ToString());
                return oComprador["id"].ToString();
            }
        }
        return "";
    }
    
    // Consultar Comprador CNPJ/CPF
    public static HttpResponseResult ConsultarCompradorCNPJCPF(string parametros)
    {
        ServicePointManager.Expect100Continue = true;
        ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12
            | SecurityProtocolType.Tls11
            | SecurityProtocolType.Tls
            | SecurityProtocolType.Ssl3;

        var byteArray = Encoding.ASCII.GetBytes(zoop.Keymarketplace() + ":" + "");
        string encodeString = Convert.ToBase64String(byteArray);

        string PARAMS = "";

        PARAMS = PARAMS + "taxpayer_id=" + Funcoes.TIRAACENTOSDOCUMENTOS(parametros.ToString()).ToString().Trim();

        var url = "https://api.zoop.ws/v1/marketplaces/" + zoop.marketplace().ToString() + "/buyers/search?" + PARAMS;

        var myHttpWebRequest = (HttpWebRequest)WebRequest.Create(new Uri(url));
        myHttpWebRequest.ClientCertificates.Add(CertificateHelper.ClientCertificate);
        myHttpWebRequest.ContentType = "application/json";
        myHttpWebRequest.Accept = "application/json";
        myHttpWebRequest.Method = "GET";
        myHttpWebRequest.PreAuthenticate = true;
        myHttpWebRequest.Headers.Add("Authorization", "Basic " + encodeString);
        myHttpWebRequest.Headers.Add("x-api-key", zoop.xapikey()); 

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

            return new HttpResponseResult(500, "Erro inesperado: " + ex.Message);
        }
    }

    // Consultar Transacao ID
    public static HttpResponseResult ConsultarTransacaoZoopID(string parametros)
    {
        ServicePointManager.Expect100Continue = true;
        ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12
            | SecurityProtocolType.Tls11
            | SecurityProtocolType.Tls
            | SecurityProtocolType.Ssl3;

        var byteArray = Encoding.ASCII.GetBytes(zoop.Keymarketplace() + ":" + "");
        string encodeString = Convert.ToBase64String(byteArray);

        var url = "https://api.zoop.ws/v1/marketplaces/" + zoop.marketplace().ToString() + "/transactions/" + parametros;

        var myHttpWebRequest = (HttpWebRequest)WebRequest.Create(new Uri(url));
        myHttpWebRequest.ClientCertificates.Add(CertificateHelper.ClientCertificate);
        myHttpWebRequest.ContentType = "application/json";
        myHttpWebRequest.Accept = "application/json";
        myHttpWebRequest.Method = "GET";
        myHttpWebRequest.PreAuthenticate = true;
        myHttpWebRequest.Headers.Add("Authorization", "Basic " + encodeString);
        myHttpWebRequest.Headers.Add("x-api-key", zoop.xapikey());   

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

            return new HttpResponseResult(500, "Erro inesperado: " + ex.Message);
        }
    }

    // Consultar Transacao ID
    public static HttpResponseResult ConsultarTransacaoZoopIDmTLS(string parametros)
    {
        ServicePointManager.Expect100Continue = true;
        ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12
            | SecurityProtocolType.Tls11
            | SecurityProtocolType.Tls
            | SecurityProtocolType.Ssl3;

        var byteArray = Encoding.ASCII.GetBytes(zoop.Keymarketplace() + ":" + "");
        string encodeString = Convert.ToBase64String(byteArray);

        var url = "https://api.zoop.ws/v1/marketplaces/" + zoop.marketplace().ToString() + "/transactions/" + parametros;

        var myHttpWebRequest = (HttpWebRequest)WebRequest.Create(new Uri(url));
        myHttpWebRequest.ClientCertificates.Add(CertificateHelper.ClientCertificate);
        myHttpWebRequest.ContentType = "application/json";
        myHttpWebRequest.Accept = "application/json";
        myHttpWebRequest.Method = "GET";
        myHttpWebRequest.PreAuthenticate = true;
        myHttpWebRequest.Headers.Add("Authorization", "Basic " + encodeString);
        myHttpWebRequest.Headers.Add("x-api-key", zoop.xapikey());   

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

            return new HttpResponseResult(500, "Erro inesperado: " + ex.Message);
        }
    }


    // ESTRUTURA MTLS *****************************************************************************************************************************************************


    public static class CertificateHelper
    {
        private static readonly Lazy<X509Certificate2> _clientCertificate = new Lazy<X509Certificate2>(LoadCertificate);

        public static X509Certificate2 ClientCertificate
        {
            get { return _clientCertificate.Value; }
        }

        private static X509Certificate2 LoadCertificate()
        {
            string certPath = HttpContext.Current.Server.MapPath("~/certs/legacy-cert.pfx");
            string certPassword = "1a?@H91m!mF4";

            return new X509Certificate2(certPath, certPassword, X509KeyStorageFlags.MachineKeySet);
        }
    }

    public static HttpResponseResult ConsultarTransacaoZoopIDmTLSNew(string parametros)
    {
        ServicePointManager.Expect100Continue = true;
        ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

        // Carregar o certificado do cliente (mTLS)
        //string certPath = HttpContext.Current.Server.MapPath("~/certs/legacy-cert.pfx"); // ajuste conforme o local
        //string certPassword = "1a?@H91m!mF4";
        //var clientCertificate = new X509Certificate2(certPath, certPassword, X509KeyStorageFlags.MachineKeySet);

        var url = "https://api.zoop.ws/v1/marketplaces/" + zoop.marketplace() + "/transactions/" + parametros;

        var request = (HttpWebRequest)WebRequest.Create(new Uri(url));
        request.ClientCertificates.Add(CertificateHelper.ClientCertificate);
        request.ContentType = "application/json";
        request.Accept = "application/json";
        request.Method = "GET";
        request.PreAuthenticate = true;

        // Headers de autenticação
        var byteArray = Encoding.ASCII.GetBytes(zoop.Keymarketplace() + ":");
        string encodedAuth = Convert.ToBase64String(byteArray);

        request.Headers.Add("Authorization", "Basic " + encodedAuth);
        request.Headers.Add("x-api-key", zoop.xapikey());

        try
        {
            using (var response = (HttpWebResponse)request.GetResponse())
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

            return new HttpResponseResult(500, "Erro inesperado: " + ex.Message);
        }
    }

    // FIM ESTRUTURA MTLS *************************************************************************************************************************************************


    public static HttpResponseResult CancelamentoBoletoZoop(string sIDCobranca)
    {
        ServicePointManager.Expect100Continue = true;
        ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12
            | SecurityProtocolType.Tls11
            | SecurityProtocolType.Tls
            | SecurityProtocolType.Ssl3;

        var byteArray = Encoding.ASCII.GetBytes(zoop.Keymarketplace() + ":" + "");
        string encodeString = Convert.ToBase64String(byteArray);

        var url = "https://api.api.zoop.ws/boletos/cancellation/marketplaces/" + marketplace().ToString() + "/transactions/" + sIDCobranca;
        var myHttpWebRequest = (HttpWebRequest)WebRequest.Create(new Uri(url));
        myHttpWebRequest.ClientCertificates.Add(CertificateHelper.ClientCertificate);
        myHttpWebRequest.ContentType = "application/json";
        myHttpWebRequest.Accept = "application/json";
        myHttpWebRequest.Method = "POST";
        myHttpWebRequest.PreAuthenticate = true;
        myHttpWebRequest.Headers.Add("Authorization", "Basic " + encodeString);
        myHttpWebRequest.Headers.Add("x-api-key", zoop.xapikey());        


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

            return new HttpResponseResult(500, "Erro inesperado: " + ex.Message);
        }
    }



    // Deletar Comprador CNPJ/CPF

    
    
    public static HttpResponseResult DeletarCompradorCNPJCPF(string id)
    {
        ServicePointManager.Expect100Continue = true;
        ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12
            | SecurityProtocolType.Tls11
            | SecurityProtocolType.Tls
            | SecurityProtocolType.Ssl3;

        var byteArray = Encoding.ASCII.GetBytes(zoop.Keymarketplace() + ":" + "");
        string encodeString = Convert.ToBase64String(byteArray);

        var url = "https://api.zoop.ws/v1/marketplaces/" + zoop.marketplace().ToString() + "/buyers/" + id.ToString();

        var myHttpWebRequest = (HttpWebRequest)WebRequest.Create(new Uri(url));
        myHttpWebRequest.ClientCertificates.Add(CertificateHelper.ClientCertificate);

        myHttpWebRequest.ContentType = "application/json";
        myHttpWebRequest.Accept = "application/json";
        myHttpWebRequest.Method = "DELETE";
        myHttpWebRequest.PreAuthenticate = true;
        myHttpWebRequest.Headers.Add("Authorization", "Basic " + encodeString);
        myHttpWebRequest.Headers.Add("x-api-key", zoop.xapikey());        


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

            return new HttpResponseResult(500, "Erro inesperado: " + ex.Message);
        }
    }


    // Cadastrar Comprador
    public static HttpResponseResult CadastrarCompradorCNPJCPF(string json)
    {
        ServicePointManager.Expect100Continue = true;
        ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12
            | SecurityProtocolType.Tls11
            | SecurityProtocolType.Tls
            | SecurityProtocolType.Ssl3;

        var byteArray = Encoding.ASCII.GetBytes(zoop.Keymarketplace() + ":" + "");
        string encodeString = Convert.ToBase64String(byteArray);

        var url = "https://api.zoop.ws/v1/marketplaces/" + zoop.marketplace().ToString() + "/buyers";
        var myUri = new Uri(url);
        var myHttpWebRequest = (HttpWebRequest)WebRequest.Create(myUri);
        myHttpWebRequest.ClientCertificates.Add(CertificateHelper.ClientCertificate);

        myHttpWebRequest.ContentType = "application/json";
        myHttpWebRequest.Accept = "application/json";
        myHttpWebRequest.Method = "POST";
        myHttpWebRequest.PreAuthenticate = true;
        myHttpWebRequest.Headers.Add("Authorization", "Basic " + encodeString);
        myHttpWebRequest.Headers.Add("x-api-key", zoop.xapikey());     

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

    // Alterar comprador ID
    public static HttpResponseResult AlterarCompradorCNPJCPF(string id, string json)
    {
        ServicePointManager.Expect100Continue = true;
        ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12
            | SecurityProtocolType.Tls11
            | SecurityProtocolType.Tls
            | SecurityProtocolType.Ssl3;

        var byteArray = Encoding.ASCII.GetBytes(zoop.Keymarketplace() + ":" + "");
        string encodeString = Convert.ToBase64String(byteArray);

        var url = "https://api.zoop.ws/v1/marketplaces/" + zoop.marketplace().ToString() + "/buyers/" + id.ToString();
        var myUri = new Uri(url);
        var myHttpWebRequest = (HttpWebRequest)WebRequest.Create(myUri);
        myHttpWebRequest.ClientCertificates.Add(CertificateHelper.ClientCertificate);
        myHttpWebRequest.ContentType = "application/json";
        myHttpWebRequest.Accept = "application/json";
        myHttpWebRequest.Method = "PUT";
        myHttpWebRequest.PreAuthenticate = true;
        myHttpWebRequest.Headers.Add("Authorization", "Basic " + encodeString);
        myHttpWebRequest.Headers.Add("x-api-key", zoop.xapikey());     

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


    // Fim novas rotinas mais modernas implementadas -----------------------------------------------------------------------------





    public class split_transacao
    {
        public string recipient { get; set; }
        public bool liable { get; set; }
        public bool charge_processing_fee { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public int? amount { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public double? percentage { get; set; }
    }
    public class split_transacao_valor
    {
        public string recipient { get; set; }
        public bool liable { get; set; }
        public bool charge_processing_fee { get; set; }
        public int amount { get; set; }
    }


    public static string LicenciadoIntegracaoZoop(string sLicenciado)
    {
        string sPadrao = "N";
        SqlConnection mySelCadastro = new SqlConnection(Funcoes.conexao());
        mySelCadastro.Open();
        SqlCommand cmdSelCadastro = new SqlCommand("dbo.stp_pessoas_fj_integracoes_chaves_ins", mySelCadastro);
        cmdSelCadastro.CommandType = CommandType.StoredProcedure;
        cmdSelCadastro.Parameters.Add("@flg_operacao", SqlDbType.Char).Value = "L";
        cmdSelCadastro.Parameters.Add("@COD_ID_INTEGRACOES", SqlDbType.Int).Value = 1;
        cmdSelCadastro.Parameters.Add("@COD_ID_PESSOA_LICENCIADO", SqlDbType.Int).Value = Funcoes.strToInt(sLicenciado.ToString());
        SqlDataReader ReaderCadastro = cmdSelCadastro.ExecuteReader();
        while (ReaderCadastro.Read())
        {
            sPadrao = ReaderCadastro["FLG_ATIVO"].ToString().Trim();
        }

        return sPadrao.ToString();
    }

    public static string xapikey()
    {
        string sPadrao = "w7yPQuauxM25jcnZwGwdH4JYULAH85zC6wmq5Sr6";
        return sPadrao;
    }

    
    public static string Keymarketplace()
    {
        string sPadrao = "zpk_prod_Nxo9anXlr0i9M8aUZMcWY2Ck";
        

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

        return sPadrao;
    }


    public static string TokenZoop()
    {
        ServicePointManager.Expect100Continue = true;
        ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12
        | SecurityProtocolType.Tls11
        | SecurityProtocolType.Tls
        | SecurityProtocolType.Ssl3;

        string sPadrao = "zpk_prod_Nxo9anXlr0i9M8aUZMcWY2Ck";

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

        var byteArray = Encoding.ASCII.GetBytes(sPadrao.ToString() + ":" + "");
        string encodeString = Convert.ToBase64String(byteArray);

        return encodeString;
    }

    public static string marketplace()
    {
        string sPadrao = "17583e0c32244a3e917f27c7e2a6d997";
                          
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
        return sPadrao.ToString();
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

        try
        {
            string URL = zoop.VersaoZoop(1).ToString() + marketplace().ToString() + OperacaoZoop("planos").ToString();

            if (detalhe.ToString().Trim() != "")
            {
                detalhe = "/" + detalhe;
            }
            var myUri = new Uri(URL + detalhe.ToString().Trim());

            var myWebRequest = WebRequest.Create(myUri);
            var myHttpWebRequest = (HttpWebRequest)myWebRequest;
            myHttpWebRequest.PreAuthenticate = true;
            myHttpWebRequest.ClientCertificates.Add(CertificateHelper.ClientCertificate);
            myHttpWebRequest.Accept = "application/json";
            myHttpWebRequest.Headers.Add("Authorization", "Basic " + TokenZoop().ToString() + "");
            myHttpWebRequest.Headers.Add("x-api-key", zoop.xapikey());        


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
    // ****************************************************************************************************************************************************************************
    // CONTA BANCARIA
    // ****************************************************************************************************************************************************************************

    public static string CadastrarContaVendedor(string json)
    {

        ServicePointManager.Expect100Continue = true;
        ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12
        | SecurityProtocolType.Tls11
        | SecurityProtocolType.Tls
        | SecurityProtocolType.Ssl3;


        var byteArray = Encoding.ASCII.GetBytes(zoop.Keymarketplace() + ":" + "");
        string encodeString = Convert.ToBase64String(byteArray);

        var URL = "https://api.zoop.ws/v1/marketplaces/" + zoop.marketplace().ToString() + "/bank_accounts/tokens";

        var myUri = new Uri(URL);
        var myWebRequest = WebRequest.Create(myUri);
        var myHttpWebRequest = (HttpWebRequest)myWebRequest;
        myHttpWebRequest.ClientCertificates.Add(CertificateHelper.ClientCertificate);
        myHttpWebRequest.ContentType = "application/json";
        myHttpWebRequest.Accept = "application/json";
        myHttpWebRequest.Method = "POST";
        myHttpWebRequest.PreAuthenticate = true;
        myHttpWebRequest.Headers.Add("Authorization", "Basic " + encodeString.ToString() + "");
        myHttpWebRequest.Headers.Add("x-api-key", zoop.xapikey());    

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

            return json;
        }
        catch (WebException ex)
        {
            var resp = new StreamReader(ex.Response.GetResponseStream()).ReadToEnd();
            return resp;
        }
    }


    public static string AssociarContaVendedor(string json)
    {
        // CONFIGURAÇÕES ACESSO ZOOP

        ServicePointManager.Expect100Continue = true;
        ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12
        | SecurityProtocolType.Tls11
        | SecurityProtocolType.Tls
        | SecurityProtocolType.Ssl3;

        var byteArray = Encoding.ASCII.GetBytes(zoop.Keymarketplace() + ":" + "");
        string encodeString = Convert.ToBase64String(byteArray);

        var URL = "";
        string sResposta = "";

        URL = "https://api.zoop.ws/v1/marketplaces/" + marketplace().ToString() + "/bank_accounts";

        var myUri = new Uri(URL);

        var myWebRequest = WebRequest.Create(myUri);
        var myHttpWebRequest = (HttpWebRequest)myWebRequest;
        myHttpWebRequest.ClientCertificates.Add(CertificateHelper.ClientCertificate);
        myHttpWebRequest.ContentType = "application/json";
        myHttpWebRequest.Accept = "application/json";
        myHttpWebRequest.Method = "POST";
        myHttpWebRequest.PreAuthenticate = true;
        myHttpWebRequest.Headers.Add("Authorization", "Basic " + encodeString.ToString() + "");
        myHttpWebRequest.Headers.Add("x-api-key", zoop.xapikey());    

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
    // ****************************************************************************************************************************************************************************
    // PLANOS
    // ****************************************************************************************************************************************************************************

    public static string CadastrarPlanoVendedor(string json)
    {
        ServicePointManager.Expect100Continue = true;
        ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12
        | SecurityProtocolType.Tls11
        | SecurityProtocolType.Tls
        | SecurityProtocolType.Ssl3;


        var byteArray = Encoding.ASCII.GetBytes(zoop.Keymarketplace() + ":" + "");
        string encodeString = Convert.ToBase64String(byteArray);

        var URL = "https://api.zoop.ws/v1/marketplaces/" + zoop.marketplace().ToString() + "/subscriptions";

        var myUri = new Uri(URL);
        var myWebRequest = WebRequest.Create(myUri);
        var myHttpWebRequest = (HttpWebRequest)myWebRequest;
        myHttpWebRequest.ClientCertificates.Add(CertificateHelper.ClientCertificate);
        myHttpWebRequest.ContentType = "application/json";
        myHttpWebRequest.Accept = "application/json";
        myHttpWebRequest.Method = "POST";
        myHttpWebRequest.PreAuthenticate = true;
        myHttpWebRequest.Headers.Add("Authorization", "Basic " + encodeString.ToString() + "");
        myHttpWebRequest.Headers.Add("x-api-key", zoop.xapikey());        

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

            return json;
        }
        catch (WebException ex)
        {
            var resp = new StreamReader(ex.Response.GetResponseStream()).ReadToEnd();
            return resp;
        }
    }


    public static string ConsultarPlanoVendedor(string id)
    {
        ServicePointManager.Expect100Continue = true;
        ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12
        | SecurityProtocolType.Tls11
        | SecurityProtocolType.Tls
        | SecurityProtocolType.Ssl3;


        var byteArray = Encoding.ASCII.GetBytes(zoop.Keymarketplace() + ":" + "");
        string encodeString = Convert.ToBase64String(byteArray);

        var URL = "https://api.zoop.ws/v1/marketplaces/" + zoop.marketplace().ToString() + "/sellers/" + id.ToString() +"/subscriptions";

        var myUri = new Uri(URL);
        var myWebRequest = WebRequest.Create(myUri);
        var myHttpWebRequest = (HttpWebRequest)myWebRequest;
        myHttpWebRequest.ClientCertificates.Add(CertificateHelper.ClientCertificate);

        myHttpWebRequest.ContentType = "application/json";
        myHttpWebRequest.Accept = "application/json";
        myHttpWebRequest.Method = "GET";
        myHttpWebRequest.PreAuthenticate = true;
        myHttpWebRequest.Headers.Add("Authorization", "Basic " + encodeString.ToString() + "");
        myHttpWebRequest.Headers.Add("x-api-key", zoop.xapikey());        

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
    }

    // ****************************************************************************************************************************************************************************
    // VENDEDORES
    // ****************************************************************************************************************************************************************************

    public static string vendedores(string tipo, string CPFCNPJ, string json)
    {
        // https://api.zoop.ws/v1/marketplaces/{marketplace_id}/sellers/search

        // CONFIGURAÇÕES ACESSO ZOOP

        ServicePointManager.Expect100Continue = true;
        ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12
        | SecurityProtocolType.Tls11
        | SecurityProtocolType.Tls
        | SecurityProtocolType.Ssl3;

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

                URL = "https://api.zoop.ws/v1/marketplaces/" + marketplace().ToString() + "/sellers/search";

                var myUri = new Uri(URL + PARAMS);
                var myWebRequest = WebRequest.Create(myUri);
                var myHttpWebRequest = (HttpWebRequest)myWebRequest;
                myHttpWebRequest.PreAuthenticate = true;
                myHttpWebRequest.Method = "GET";

                myHttpWebRequest.ClientCertificates.Add(CertificateHelper.ClientCertificate);
                myHttpWebRequest.Accept = "application/json";
                myHttpWebRequest.Headers.Add("Authorization", "Basic " + TokenZoop().ToString() + "");
                myHttpWebRequest.Headers.Add("x-api-key", zoop.xapikey());   

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
    // CADASTRAR VENDEDOR PJ

    public static string CadastrarVendedorPJ(string json)
    {
        // CONFIGURAÇÕES ACESSO ZOOP

        ServicePointManager.Expect100Continue = true;
        ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12
        | SecurityProtocolType.Tls11
        | SecurityProtocolType.Tls
        | SecurityProtocolType.Ssl3;


        var byteArray = Encoding.ASCII.GetBytes(zoop.Keymarketplace() + ":" + "");
        string encodeString = Convert.ToBase64String(byteArray);

        var URL = "https://api.zoop.ws/v1/marketplaces/" + zoop.marketplace().ToString() + "/sellers/businesses";

        var myUri = new Uri(URL);
        var myWebRequest = WebRequest.Create(myUri);
        var myHttpWebRequest = (HttpWebRequest)myWebRequest;
        myHttpWebRequest.ClientCertificates.Add(CertificateHelper.ClientCertificate);
        myHttpWebRequest.ContentType = "application/json";
        myHttpWebRequest.Accept = "application/json";
        myHttpWebRequest.Method = "POST";
        myHttpWebRequest.PreAuthenticate = true;
        myHttpWebRequest.Headers.Add("Authorization", "Basic " + encodeString.ToString() + "");
        myHttpWebRequest.Headers.Add("x-api-key", zoop.xapikey());        

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

            return json;
        }
        catch (WebException ex)
        {
            var resp = new StreamReader(ex.Response.GetResponseStream()).ReadToEnd();
            return resp;
        }
    }

    // CADASTRAR VENDEDOR PF
    public static string CadastrarVendedorPF(string json)
    {
        // CONFIGURAÇÕES ACESSO ZOOP

        ServicePointManager.Expect100Continue = true;
        ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12
        | SecurityProtocolType.Tls11
        | SecurityProtocolType.Tls
        | SecurityProtocolType.Ssl3;


        var byteArray = Encoding.ASCII.GetBytes(zoop.Keymarketplace() + ":" + "");
        string encodeString = Convert.ToBase64String(byteArray);

        var URL = "https://api.zoop.ws/v1/marketplaces/" + zoop.marketplace().ToString() + "/sellers/individuals";

        var myUri = new Uri(URL);
        var myWebRequest = WebRequest.Create(myUri);
        var myHttpWebRequest = (HttpWebRequest)myWebRequest;
        myHttpWebRequest.ClientCertificates.Add(CertificateHelper.ClientCertificate);

        myHttpWebRequest.ContentType = "application/json";
        myHttpWebRequest.Accept = "application/json";
        myHttpWebRequest.Method = "POST";
        myHttpWebRequest.PreAuthenticate = true;
        myHttpWebRequest.Headers.Add("Authorization", "Basic " + encodeString.ToString() + "");
        myHttpWebRequest.Headers.Add("x-api-key", zoop.xapikey());        

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

            return json;
        }
        catch (WebException ex)
        {
            var resp = new StreamReader(ex.Response.GetResponseStream()).ReadToEnd();
            return resp;
        }
    }

    // ALTERAR VENDEDOR PJ
    public static string AlterarVendedorPJ(string json, string sID)
    {
        // CONFIGURAÇÕES ACESSO ZOOP

        ServicePointManager.Expect100Continue = true;
        ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12
        | SecurityProtocolType.Tls11
        | SecurityProtocolType.Tls
        | SecurityProtocolType.Ssl3;


        var byteArray = Encoding.ASCII.GetBytes(zoop.Keymarketplace() + ":" + "");
        string encodeString = Convert.ToBase64String(byteArray);

        var URL = "https://api.zoop.ws/v1/marketplaces/" + zoop.marketplace().ToString() + "/sellers/businesses/"+sID;

        var myUri = new Uri(URL);
        var myWebRequest = WebRequest.Create(myUri);
        var myHttpWebRequest = (HttpWebRequest)myWebRequest;
        myHttpWebRequest.ClientCertificates.Add(CertificateHelper.ClientCertificate);

        myHttpWebRequest.ContentType = "application/json";
        myHttpWebRequest.Accept = "application/json";
        myHttpWebRequest.Method = "PUT";
        myHttpWebRequest.PreAuthenticate = true;
        myHttpWebRequest.Headers.Add("Authorization", "Basic " + encodeString.ToString() + "");
        myHttpWebRequest.Headers.Add("x-api-key", zoop.xapikey());

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

            return json;
        }
        catch (WebException ex)
        {
            var resp = new StreamReader(ex.Response.GetResponseStream()).ReadToEnd();
            return resp;
        }
    }

    // ALTERAR VENDEDOR PF
    public static string AlterarVendedorPF(string json, string sID)
    {
        ServicePointManager.Expect100Continue = true;
        ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12
        | SecurityProtocolType.Tls11
        | SecurityProtocolType.Tls
        | SecurityProtocolType.Ssl3;


        var byteArray = Encoding.ASCII.GetBytes(zoop.Keymarketplace() + ":" + "");
        string encodeString = Convert.ToBase64String(byteArray);

        var URL = "https://api.zoop.ws/v1/marketplaces/" + zoop.marketplace().ToString() + "/sellers/individuals/"+sID;

        var myUri = new Uri(URL);
        var myWebRequest = WebRequest.Create(myUri);
        var myHttpWebRequest = (HttpWebRequest)myWebRequest;
        myHttpWebRequest.ClientCertificates.Add(CertificateHelper.ClientCertificate);
        myHttpWebRequest.ContentType = "application/json";
        myHttpWebRequest.Accept = "application/json";
        myHttpWebRequest.Method = "PUT";
        myHttpWebRequest.PreAuthenticate = true;
        myHttpWebRequest.Headers.Add("Authorization", "Basic " + encodeString.ToString() + "");
        myHttpWebRequest.Headers.Add("x-api-key", zoop.xapikey());

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

            return json;
        }
        catch (WebException ex)
        {
            var resp = new StreamReader(ex.Response.GetResponseStream()).ReadToEnd();
            return resp;
        }
    }


    public static string CadastroVendedores(string sTipo, string json, string sToken)
    {
        // CONFIGURAÇÕES ACESSO ZOOP

        ServicePointManager.Expect100Continue = true;
        ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12
        | SecurityProtocolType.Tls11
        | SecurityProtocolType.Tls
        | SecurityProtocolType.Ssl3;


        var byteArray = Encoding.ASCII.GetBytes(zoop.Keymarketplace() + ":" + "");
        string encodeString = Convert.ToBase64String(byteArray);

        var URL = "https://api.zoop.ws/v1/marketplaces/" + zoop.marketplace().ToString() + "/sellers/businesses";

        var myUri = new Uri(URL);
        var myWebRequest = WebRequest.Create(myUri);
        var myHttpWebRequest = (HttpWebRequest)myWebRequest;
        myHttpWebRequest.ClientCertificates.Add(CertificateHelper.ClientCertificate);
        myHttpWebRequest.ContentType = "application/json";
        myHttpWebRequest.Accept = "application/json";
        myHttpWebRequest.Method = "POST";
        myHttpWebRequest.PreAuthenticate = true;
        myHttpWebRequest.Headers.Add("Authorization", "Basic " + encodeString.ToString() + "");
        myHttpWebRequest.Headers.Add("x-api-key", zoop.xapikey()); 

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

            return json;
        }
        catch (WebException ex)
        {
            var resp = new StreamReader(ex.Response.GetResponseStream()).ReadToEnd();
            return resp;
        }
    }


    // ************************************************************************************************************************************************************************************
    // Consultar dados do Seller, contas, cadastro, transações etc...
    // ************************************************************************************************************************************************************************************

    public static string ConsultaCadastroSeller(string CPFCNPJ)
    {

        ServicePointManager.Expect100Continue = true;
        ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12
        | SecurityProtocolType.Tls11
        | SecurityProtocolType.Tls
        | SecurityProtocolType.Ssl3;

        var byteArray = Encoding.ASCII.GetBytes(zoop.Keymarketplace() + ":" + "");
        string encodeString = Convert.ToBase64String(byteArray);


        var URL = "";
        var PARAMS = "";
        var ID = "";

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

            URL = "https://api.zoop.ws/v1/marketplaces/" + marketplace().ToString() + "/sellers/search";


            var myUri = new Uri(URL + PARAMS);
            var myWebRequest = WebRequest.Create(myUri);
            var myHttpWebRequest = (HttpWebRequest)myWebRequest;
            myHttpWebRequest.PreAuthenticate = true;
            myHttpWebRequest.Method = "GET";
            myHttpWebRequest.ClientCertificates.Add(CertificateHelper.ClientCertificate);
            myHttpWebRequest.Accept = "application/json";
            myHttpWebRequest.Headers.Add("Authorization", "Basic " + encodeString.ToString() + "");
            myHttpWebRequest.Headers.Add("x-api-key", zoop.xapikey()); 

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

    // ********************************************************************************************************
    // CONSULTA VENDEDORES
    // ********************************************************************************************************

    public static string ConsultaVendedores(int limit, int offset)
    {

        ServicePointManager.Expect100Continue = true;
        ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12
        | SecurityProtocolType.Tls11
        | SecurityProtocolType.Tls
        | SecurityProtocolType.Ssl3;

        var byteArray = Encoding.ASCII.GetBytes(zoop.Keymarketplace() + ":" + "");
        string encodeString = Convert.ToBase64String(byteArray);

        var URL = "";
        var PARAMS = "";

        try
        {

            PARAMS = "?limit=" + limit.ToString() + "&offset=" + offset.ToString();
            URL = "https://api.zoop.ws/v1/marketplaces/" + marketplace().ToString() + "/sellers";

            var myUri = new Uri(URL + PARAMS);
            var myWebRequest = WebRequest.Create(myUri);
            var myHttpWebRequest = (HttpWebRequest)myWebRequest;
            myHttpWebRequest.PreAuthenticate = true;
            myHttpWebRequest.Method = "GET";
            myHttpWebRequest.ClientCertificates.Add(CertificateHelper.ClientCertificate);
            myHttpWebRequest.Accept = "application/json";
            myHttpWebRequest.Headers.Add("Authorization", "Basic " + encodeString.ToString() + "");
            myHttpWebRequest.Headers.Add("x-api-key", zoop.xapikey());       


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

    
    public static string ConsultaContasSeller(string sTokenSeller)
    {

        ServicePointManager.Expect100Continue = true;
        ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12
        | SecurityProtocolType.Tls11
        | SecurityProtocolType.Tls
        | SecurityProtocolType.Ssl3;

        var URL = "";
        var PARAMS = "";
        var ID = "";

        try
        {
            URL = "https://api.zoop.ws/v1/marketplaces/" + marketplace().ToString() + "/sellers/" + sTokenSeller.ToString() + "/bank_accounts";

            var myUri = new Uri(URL + PARAMS);
            var myWebRequest = WebRequest.Create(myUri);
            var myHttpWebRequest = (HttpWebRequest)myWebRequest;
            myHttpWebRequest.PreAuthenticate = true;
            myHttpWebRequest.Method = "GET";
            myHttpWebRequest.ClientCertificates.Add(CertificateHelper.ClientCertificate);
            myHttpWebRequest.Accept = "application/json"; 
            myHttpWebRequest.Headers.Add("Authorization", "Basic " + TokenZoop().ToString() + "");
            myHttpWebRequest.Headers.Add("x-api-key", zoop.xapikey());       
            

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

    public static string ConsultaTransacoesSeller(string sTokenSeller, string soffset, string slimit)
    {

        ServicePointManager.Expect100Continue = true;
        ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12
        | SecurityProtocolType.Tls11
        | SecurityProtocolType.Tls
        | SecurityProtocolType.Ssl3;

        var URL = "";
        var PARAMS = "";
        var ID = "";

        try
        {
            URL = "https://api.zoop.ws/v1/marketplaces/" + marketplace().ToString() + "/sellers/" + sTokenSeller.ToString() + "/transactions?limit="+slimit.ToString()+"&sort=time-descending&offset="+soffset.ToString()+"";

            var myUri = new Uri(URL + PARAMS);
            var myWebRequest = WebRequest.Create(myUri);
            var myHttpWebRequest = (HttpWebRequest)myWebRequest;
            myHttpWebRequest.PreAuthenticate = true;
            myHttpWebRequest.Method = "GET";
            myHttpWebRequest.ClientCertificates.Add(CertificateHelper.ClientCertificate);
            myHttpWebRequest.Accept = "application/json";
            myHttpWebRequest.Headers.Add("Authorization", "Basic " + TokenZoop().ToString() + "");
            myHttpWebRequest.Headers.Add("x-api-key", zoop.xapikey());  
            

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


    // ************************************************************************************************************************************************************************************
    // 
    // ************************************************************************************************************************************************************************************

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


    public static string TransacaoZoopGeral(string tipo, string codigo, string json)
    {
        // CONFIGURAÇÕES ACESSO ZOOP

        ServicePointManager.Expect100Continue = true;
        ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12
        | SecurityProtocolType.Tls11
        | SecurityProtocolType.Tls
        | SecurityProtocolType.Ssl3;

        var URL = "";
        var PARAMS = "";
        var ID = "";

        PARAMS = "";
        URL = "https://api.zoop.ws/v1/marketplaces/" + marketplace().ToString() + "/transactions";

        var myUri = new Uri(URL + PARAMS);

        var myWebRequest = WebRequest.Create(myUri);
        var myHttpWebRequest = (HttpWebRequest)myWebRequest;
        myHttpWebRequest.ClientCertificates.Add(CertificateHelper.ClientCertificate);

        myHttpWebRequest.ContentType = "application/json";
        myHttpWebRequest.Accept = "application/json";
        myHttpWebRequest.Method = "POST";
        myHttpWebRequest.PreAuthenticate = true;
        myHttpWebRequest.Headers.Add("Authorization", "Basic " + TokenZoop().ToString() + "");
        myHttpWebRequest.Headers.Add("x-api-key", zoop.xapikey());  

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
        return json;
        //}
        //catch (WebException ex)
        //{
        //    var resp = new StreamReader(ex.Response.GetResponseStream()).ReadToEnd();
        //    return resp;
        //}
    }



    public static string transacao(string tipo, string codigo, string json)
    {
        // CONFIGURAÇÕES ACESSO ZOOP

        ServicePointManager.Expect100Continue = true;
        ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12
        | SecurityProtocolType.Tls11
        | SecurityProtocolType.Tls
        | SecurityProtocolType.Ssl3;

        var URL = "";
        var PARAMS = "";
        var ID = "";

        // Inclusão
        if (tipo == "I")
        {
            PARAMS = "";
            URL = "https://api.zoop.ws/v1/marketplaces/" + marketplace().ToString() + "/transactions";

            var myUri = new Uri(URL + PARAMS);

            var myWebRequest = WebRequest.Create(myUri);
            var myHttpWebRequest = (HttpWebRequest)myWebRequest;
            myHttpWebRequest.ClientCertificates.Add(CertificateHelper.ClientCertificate);
            myHttpWebRequest.ContentType = "application/json";
            myHttpWebRequest.Accept = "application/json";
            myHttpWebRequest.Method = "POST";
            myHttpWebRequest.PreAuthenticate = true;
            myHttpWebRequest.Headers.Add("Authorization", "Basic " + TokenZoop().ToString() + "");
            myHttpWebRequest.Headers.Add("x-api-key", zoop.xapikey());  

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
                return json;
            }
            catch (WebException ex)
            {
                var resp = new StreamReader(ex.Response.GetResponseStream()).ReadToEnd();
                return resp;
            }
        }

        // Busca
        if (tipo == "P")
        {

            try
            {
                PARAMS = "" + codigo.ToString();
                URL = "https://api.zoop.ws/v1/marketplaces/" + marketplace().ToString() + "/transactions/";

                var myUri = new Uri(URL + PARAMS);
                var myWebRequest = WebRequest.Create(myUri);
                var myHttpWebRequest = (HttpWebRequest)myWebRequest;
                myHttpWebRequest.PreAuthenticate = true;
                myHttpWebRequest.ClientCertificates.Add(CertificateHelper.ClientCertificate);
                myHttpWebRequest.Accept = "application/json";
                myHttpWebRequest.Headers.Add("Authorization", "Basic " + TokenZoop().ToString() + "");
                myHttpWebRequest.Headers.Add("x-api-key", zoop.xapikey());  
                

                var myWebResponse = myWebRequest.GetResponse();
                var responseStream = myWebResponse.GetResponseStream();

                StreamReader myStreamReader = new StreamReader(responseStream, Encoding.UTF8);
                json = myStreamReader.ReadToEnd();
                return json;
            }
            catch (WebException ex)
            {
                var resp = new StreamReader(ex.Response.GetResponseStream()).ReadToEnd();
                return resp;
            }

        }

        return "";
    }

    public static string DetalhesVendedorPJ(string sDocumento)
    {

        ServicePointManager.Expect100Continue = true;
        ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12
        | SecurityProtocolType.Tls11
        | SecurityProtocolType.Tls
        | SecurityProtocolType.Ssl3;


        var byteArray = Encoding.ASCII.GetBytes(zoop.Keymarketplace() + ":" + "");
        string encodeString = Convert.ToBase64String(byteArray);

        string PARAMS = "?ein=" + TIRAACENTOS(sDocumento.ToString());

        var URL = "https://api.zoop.ws/v1/marketplaces/" + zoop.marketplace().ToString() + "/sellers/search";

        var myUri = new Uri(URL + PARAMS);
        var myWebRequest = WebRequest.Create(myUri);
        var myHttpWebRequest = (HttpWebRequest)myWebRequest;
        myHttpWebRequest.ClientCertificates.Add(CertificateHelper.ClientCertificate);
        myHttpWebRequest.ContentType = "application/json";
        myHttpWebRequest.Accept = "application/json";
        myHttpWebRequest.Method = "GET";
        myHttpWebRequest.PreAuthenticate = true;
        myHttpWebRequest.Headers.Add("Authorization", "Basic " + encodeString.ToString() + "");
        myHttpWebRequest.Headers.Add("x-api-key", zoop.xapikey());

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

    }

    public static string DetalhesVendedorPF(string sDocumento)
    {

        ServicePointManager.Expect100Continue = true;
        ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12
        | SecurityProtocolType.Tls11
        | SecurityProtocolType.Tls
        | SecurityProtocolType.Ssl3;


        var byteArray = Encoding.ASCII.GetBytes(zoop.Keymarketplace() + ":" + "");
        string encodeString = Convert.ToBase64String(byteArray);

        string PARAMS = "?taxpayer_id=" + TIRAACENTOS(sDocumento.ToString());

        var URL = "https://api.zoop.ws/v1/marketplaces/" + zoop.marketplace().ToString() + "/sellers/search" ;

        var myUri = new Uri(URL + PARAMS);
        var myWebRequest = WebRequest.Create(myUri);
        var myHttpWebRequest = (HttpWebRequest)myWebRequest;
        myHttpWebRequest.ClientCertificates.Add(CertificateHelper.ClientCertificate);
        myHttpWebRequest.ContentType = "application/json";
        myHttpWebRequest.Accept = "application/json";
        myHttpWebRequest.Method = "GET";
        myHttpWebRequest.PreAuthenticate = true;
        myHttpWebRequest.Headers.Add("Authorization", "Basic " + encodeString.ToString() + "");
        myHttpWebRequest.Headers.Add("x-api-key", zoop.xapikey());        


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

    }


    public static string EstornarTransacao(string sIDTransacao, string json)
    {
        ServicePointManager.Expect100Continue = true;
        ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12
        | SecurityProtocolType.Tls11
        | SecurityProtocolType.Tls
        | SecurityProtocolType.Ssl3;


        string PARAMS = sIDTransacao.ToString() + "/void";

        var myUri = new Uri("https://api.zoop.ws/v2/marketplaces/" + marketplace().ToString() + "/transactions/" + PARAMS.ToString());

        var myWebRequest = WebRequest.Create(myUri);
        var myHttpWebRequest = (HttpWebRequest)myWebRequest;
        myHttpWebRequest.ClientCertificates.Add(CertificateHelper.ClientCertificate);
        myHttpWebRequest.ContentType = "application/json";
        myHttpWebRequest.Accept = "application/json";
        myHttpWebRequest.Method = "POST";
        myHttpWebRequest.PreAuthenticate = true;
        myHttpWebRequest.Headers.Add("Authorization", "Basic " + TokenZoop().ToString() + "");
        myHttpWebRequest.Headers.Add("x-api-key", zoop.xapikey());

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
            return o["status"].ToString();
        }
        catch (WebException ex)
        {
            var resp = new StreamReader(ex.Response.GetResponseStream()).ReadToEnd();
            return resp;
        }

    }

    public static string CancelamentoBoleto(string sIDCancelamento)
    {
        ServicePointManager.Expect100Continue = true;
        ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12
        | SecurityProtocolType.Tls11
        | SecurityProtocolType.Tls
        | SecurityProtocolType.Ssl3;

        try
        {
            string PARAMS = sIDCancelamento.ToString();

            var myUri = new Uri("https://api.api.zoop.ws/boletos/cancellation/marketplaces/" + marketplace().ToString() + "/transactions/" + PARAMS.ToString());
            var myWebRequest = WebRequest.Create(myUri);
            var myHttpWebRequest = (HttpWebRequest)myWebRequest;
            myHttpWebRequest.PreAuthenticate = true;
            myHttpWebRequest.ClientCertificates.Add(CertificateHelper.ClientCertificate);
            myHttpWebRequest.Accept = "application/json";
            myHttpWebRequest.Method = "POST";
            myHttpWebRequest.Headers.Add("Authorization", "Basic " + TokenZoop().ToString() + "");
            myHttpWebRequest.Headers.Add("x-api-key", zoop.xapikey()); 
            
            //enBrX3Rlc3RfSzFScThuVm1BOW4ybERXa3pRc2gzSGpYOg==

            var myWebResponse = myWebRequest.GetResponse();
            var responseStream = myWebResponse.GetResponseStream();

            //if (responseStream == null) return null;

            StreamReader myStreamReader = new StreamReader(responseStream, Encoding.UTF8);
            var json = myStreamReader.ReadToEnd();

            return json;
        }
        catch
        {
            return "";
        }

    }


    public static string CartaCancelamento(string sIDCancelamento)
    {
        ServicePointManager.Expect100Continue = true;
        ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12
        | SecurityProtocolType.Tls11
        | SecurityProtocolType.Tls
        | SecurityProtocolType.Ssl3;

        try
        {
            string PARAMS = sIDCancelamento.ToString() + "/cancelled-letter";

            var myUri = new Uri("https://api.zoop.ws/v1/marketplaces/" + marketplace().ToString() + "/transactions/" + PARAMS.ToString());
            var myWebRequest = WebRequest.Create(myUri);
            var myHttpWebRequest = (HttpWebRequest)myWebRequest;
            myHttpWebRequest.PreAuthenticate = true;
            myHttpWebRequest.ClientCertificates.Add(CertificateHelper.ClientCertificate);

            myHttpWebRequest.Accept = "application/json";
            myHttpWebRequest.Headers.Add("Authorization", "Basic " + TokenZoop().ToString() + "");
            myHttpWebRequest.Headers.Add("x-api-key", zoop.xapikey()); 
            //enBrX3Rlc3RfSzFScThuVm1BOW4ybERXa3pRc2gzSGpYOg==

            var myWebResponse = myWebRequest.GetResponse();
            var responseStream = myWebResponse.GetResponseStream();

            //if (responseStream == null) return null;

            StreamReader myStreamReader = new StreamReader(responseStream, Encoding.UTF8);
            var json = myStreamReader.ReadToEnd();

            return json;
        }
        catch
        {
            return "";
        }

    }


    public static string DetalhesVendedorContas(string sVendedor)
    {
        ServicePointManager.Expect100Continue = true;
        ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12
        | SecurityProtocolType.Tls11
        | SecurityProtocolType.Tls
        | SecurityProtocolType.Ssl3;

        try
        {
            string PARAMS = sVendedor.ToString() + "/bank_accounts";

            var myUri = new Uri("https://api.zoop.ws/v1/marketplaces/" + marketplace().ToString() + "/sellers/" + PARAMS.ToString());
            var myWebRequest = WebRequest.Create(myUri);
            var myHttpWebRequest = (HttpWebRequest)myWebRequest;
            myHttpWebRequest.PreAuthenticate = true;
            myHttpWebRequest.ClientCertificates.Add(CertificateHelper.ClientCertificate);
            myHttpWebRequest.Accept = "application/json";
            myHttpWebRequest.Headers.Add("Authorization", "Basic " + TokenZoop().ToString() + "");

            myHttpWebRequest.Headers.Add("x-api-key", zoop.xapikey());
            //enBrX3Rlc3RfSzFScThuVm1BOW4ybERXa3pRc2gzSGpYOg==

            var myWebResponse = myWebRequest.GetResponse();
            var responseStream = myWebResponse.GetResponseStream();

            //if (responseStream == null) return null;

            StreamReader myStreamReader = new StreamReader(responseStream, Encoding.UTF8);
            var json = myStreamReader.ReadToEnd();

            return json;
        }
        catch
        {
            return "";
        }

    }

    public static string DetalhesRecibo(string sRecibo)
    {
        ServicePointManager.Expect100Continue = true;
        ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12
        | SecurityProtocolType.Tls11
        | SecurityProtocolType.Tls
        | SecurityProtocolType.Ssl3;

        try
        {
            string PARAMS = "" + sRecibo.ToString();
            string URL = "https://api.zoop.ws/v1/marketplaces/" + marketplace().ToString() + "/receipts/";

            var myUri = new Uri(URL + PARAMS);
            var myWebRequest = WebRequest.Create(myUri);
            var myHttpWebRequest = (HttpWebRequest)myWebRequest;
            myHttpWebRequest.PreAuthenticate = true;
            myHttpWebRequest.ClientCertificates.Add(CertificateHelper.ClientCertificate);
            myHttpWebRequest.Accept = "application/json";
            myHttpWebRequest.Headers.Add("Authorization", "Basic " + TokenZoop().ToString() + "");
            myHttpWebRequest.Headers.Add("x-api-key", zoop.xapikey());

            var myWebResponse = myWebRequest.GetResponse();
            var responseStream = myWebResponse.GetResponseStream();

            StreamReader myStreamReader = new StreamReader(responseStream, Encoding.UTF8);
            string json = myStreamReader.ReadToEnd();

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


        try
        {
            string PARAMS = "" + sTransacao.ToString() + "/receivables";
            string URL = "https://api.zoop.ws/v1/marketplaces/" + marketplace().ToString() + "/transactions/";

            var myUri = new Uri(URL + PARAMS);
            var myWebRequest = WebRequest.Create(myUri);
            var myHttpWebRequest = (HttpWebRequest)myWebRequest;
            myHttpWebRequest.PreAuthenticate = true;
            myHttpWebRequest.ClientCertificates.Add(CertificateHelper.ClientCertificate);
            myHttpWebRequest.Accept = "application/json";
            myHttpWebRequest.Headers.Add("Authorization", "Basic " + TokenZoop().ToString() + "");
            myHttpWebRequest.Headers.Add("x-api-key", zoop.xapikey());
            

            var myWebResponse = myWebRequest.GetResponse();
            var responseStream = myWebResponse.GetResponseStream();

            StreamReader myStreamReader = new StreamReader(responseStream, Encoding.UTF8);
            string json = myStreamReader.ReadToEnd();

            return json;
        }
        catch
        {
            return "";
        }

    }

    public static string DetalhesRecebiveisVendedor(string sVendedor)
    {
        ServicePointManager.Expect100Continue = true;
        ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12
        | SecurityProtocolType.Tls11
        | SecurityProtocolType.Tls
        | SecurityProtocolType.Ssl3;

        var byteArray = Encoding.ASCII.GetBytes(zoop.Keymarketplace() + ":" + "");
        string encodeString = Convert.ToBase64String(byteArray);

        string PARAMS = "" + sVendedor.ToString() + "/receivables";
        string URL = "https://api.zoop.ws/v1/marketplaces/" + zoop.marketplace().ToString() + "/sellers/";

        var myUri = new Uri(URL + PARAMS);
        var myWebRequest = WebRequest.Create(myUri);
        var myHttpWebRequest = (HttpWebRequest)myWebRequest;
        myHttpWebRequest.PreAuthenticate = true;
        myHttpWebRequest.ClientCertificates.Add(CertificateHelper.ClientCertificate);

        myHttpWebRequest.Accept = "application/json";
        myHttpWebRequest.Method = "GET";
        myHttpWebRequest.Headers.Add("Authorization", "Basic " + encodeString.ToString() + "");
        myHttpWebRequest.Headers.Add("x-api-key", zoop.xapikey());

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


    public static string DetalhesTerminal(string sTerminal)
    {
        ServicePointManager.Expect100Continue = true;
        ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12
        | SecurityProtocolType.Tls11
        | SecurityProtocolType.Tls
        | SecurityProtocolType.Ssl3;

        try
        {
            string PARAMS = "" + sTerminal.ToString();
            string URL = "https://api.zoop.ws/v1/card-present/terminals/";

            var myUri = new Uri(URL + PARAMS);
            var myWebRequest = WebRequest.Create(myUri);
            var myHttpWebRequest = (HttpWebRequest)myWebRequest;
            myHttpWebRequest.PreAuthenticate = true;
            myHttpWebRequest.ClientCertificates.Add(CertificateHelper.ClientCertificate);
            myHttpWebRequest.Accept = "application/json";
            myHttpWebRequest.Headers.Add("Authorization", "Basic " + TokenZoop().ToString() + "");
            myHttpWebRequest.Headers.Add("x-api-key", zoop.xapikey());   

            var myWebResponse = myWebRequest.GetResponse();
            var responseStream = myWebResponse.GetResponseStream();

            StreamReader myStreamReader = new StreamReader(responseStream, Encoding.UTF8);
            string json = myStreamReader.ReadToEnd();

            return json;
        }
        catch
        {
            return "";
        }

    }


    
    public static HttpResponseResult transferencia(string tipo, string codigo, string json)
    {
        ServicePointManager.Expect100Continue = true;
        ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

        string marketplaceId = marketplace(); 
        string secretKey = zoop.Keymarketplace();    // live_sk_xxxxx ou test_sk_xxxxx
        string publicKey = zoop.xapikey();      // live_pk_xxxxx ou test_pk_xxxxx

        // Basic Auth correto (secret_key:)
        string authBase64 = Convert.ToBase64String(Encoding.ASCII.GetBytes(secretKey + ":"));
        string url = "https://api.zoop.ws/v1/marketplaces/" + marketplaceId + "/bank_accounts/" + codigo + "/transfers";

        var request = (HttpWebRequest)WebRequest.Create(new Uri(url));

        // Adiciona certificado cliente Zoop
        request.ClientCertificates.Add(CertificateHelper.ClientCertificate);

        request.ContentType = "application/json";
        request.Accept = "application/json";
        request.Method = "POST";
        request.PreAuthenticate = true;

        // Headers obrigatórios
        request.Headers.Add("Authorization", "Basic " + authBase64);
        request.Headers.Add("x-api-key", publicKey);

        // Escreve o JSON no body
        using (var writer = new StreamWriter(request.GetRequestStream()))
            writer.Write(json);

        try
        {
            using (var response = (HttpWebResponse)request.GetResponse())
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

            return new HttpResponseResult(500, "Erro inesperado: " + ex.Message);
        }
    }




    public static string PoliticaRecebimento(string sVendedor, string json)
    {
        // CONFIGURAÇÕES ACESSO ZOOP

        ServicePointManager.Expect100Continue = true;
        ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12
        | SecurityProtocolType.Tls11
        | SecurityProtocolType.Tls
        | SecurityProtocolType.Ssl3;

        var URL = "";
        var PARAMS = "";
        var ID = "";

        PARAMS = "";
        URL = "https://api.zoop.ws/v1/marketplaces/" + marketplace().ToString() + "/sellers/" + sVendedor.ToString() + "/receiving_policy";

        var myUri = new Uri(URL + PARAMS);

        var myWebRequest = WebRequest.Create(myUri);
        var myHttpWebRequest = (HttpWebRequest)myWebRequest;
        myHttpWebRequest.ClientCertificates.Add(CertificateHelper.ClientCertificate);
        myHttpWebRequest.ContentType = "application/json";
        myHttpWebRequest.Accept = "application/json";
        myHttpWebRequest.Method = "POST";
        myHttpWebRequest.PreAuthenticate = true;
        myHttpWebRequest.Headers.Add("Authorization", "Basic " + TokenZoop().ToString() + "");
        myHttpWebRequest.Headers.Add("x-api-key", zoop.xapikey()); 

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
    }

    public static string AssociarContaBancariaVendedor(string json)
    {
        // CONFIGURAÇÕES ACESSO ZOOP

        ServicePointManager.Expect100Continue = true;
        ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12
        | SecurityProtocolType.Tls11
        | SecurityProtocolType.Tls
        | SecurityProtocolType.Ssl3;

        var URL = "https://api.zoop.ws/v1/marketplaces/" + marketplace().ToString() + "/bank_accounts";
        var myUri = new Uri(URL);

        var myWebRequest = WebRequest.Create(myUri);
        var myHttpWebRequest = (HttpWebRequest)myWebRequest;
        myHttpWebRequest.ClientCertificates.Add(CertificateHelper.ClientCertificate);

        myHttpWebRequest.ContentType = "application/json";
        myHttpWebRequest.Accept = "application/json";
        myHttpWebRequest.Method = "POST";
        myHttpWebRequest.PreAuthenticate = true;
        myHttpWebRequest.Headers.Add("Authorization", "Basic " + TokenZoop().ToString() + "");
        myHttpWebRequest.Headers.Add("x-api-key", zoop.xapikey()); 

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
            return json;
        }
        catch (WebException ex)
        {
            var resp = new StreamReader(ex.Response.GetResponseStream()).ReadToEnd();
            return resp;
        }
    }


    public static string transferenciaP2P(string tipo, string codigoPagador, string codigoRecebedor, string json)
    {
        // CONFIGURAÇÕES ACESSO ZOOP

        ServicePointManager.Expect100Continue = true;
        ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12
        | SecurityProtocolType.Tls11
        | SecurityProtocolType.Tls
        | SecurityProtocolType.Ssl3;

        var URL = "";
        var PARAMS = "";
        var ID = "";

        // Inclusão
        if (tipo == "I")
        {
            PARAMS = "";

            URL = "https://api.zoop.ws/v2/marketplaces/" + marketplace().ToString() + "/transfers/" + codigoPagador.ToString() + "/to/" + codigoRecebedor.ToString();
            var myUri = new Uri(URL + PARAMS);

            var myWebRequest = WebRequest.Create(myUri);
            var myHttpWebRequest = (HttpWebRequest)myWebRequest;
            myHttpWebRequest.ClientCertificates.Add(CertificateHelper.ClientCertificate);

            myHttpWebRequest.ContentType = "application/json";
            myHttpWebRequest.Accept = "application/json";
            myHttpWebRequest.Method = "POST";
            myHttpWebRequest.PreAuthenticate = true;
            myHttpWebRequest.Headers.Add("Authorization", "Basic " + TokenZoop().ToString() + "");
            myHttpWebRequest.Headers.Add("x-api-key", zoop.xapikey());      

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

        var URL = "";
        var PARAMS = "";
        var ID = "";

        // Inclusão
        if (tipo == "I")
        {
            PARAMS = "";
            URL = "https://api.zoop.ws/v1/marketplaces/" + marketplace().ToString() + "/buyers";

            var myUri = new Uri(URL + PARAMS);

            var myWebRequest = WebRequest.Create(myUri);
            var myHttpWebRequest = (HttpWebRequest)myWebRequest;
            myHttpWebRequest.ClientCertificates.Add(CertificateHelper.ClientCertificate);
            myHttpWebRequest.ContentType = "application/json";
            myHttpWebRequest.Accept = "application/json";
            myHttpWebRequest.Method = "POST";
            myHttpWebRequest.PreAuthenticate = true;
            myHttpWebRequest.Headers.Add("Authorization", "Basic " + TokenZoop().ToString() + "");
            myHttpWebRequest.Headers.Add("x-api-key", zoop.xapikey());      

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
                return json;
            }
            catch (WebException ex)
            {
                var resp = new StreamReader(ex.Response.GetResponseStream()).ReadToEnd();
                return resp;
            }
        }

        // Alteracao
        if (tipo == "A")
        {
            PARAMS = "/"+CPFCNPJ.ToString();
            URL = "https://api.zoop.ws/v1/marketplaces/" + marketplace().ToString() + "/buyers";

            var myUri = new Uri(URL + PARAMS);

            var myWebRequest = WebRequest.Create(myUri);
            var myHttpWebRequest = (HttpWebRequest)myWebRequest;
            myHttpWebRequest.ClientCertificates.Add(CertificateHelper.ClientCertificate);
            myHttpWebRequest.ContentType = "application/json";
            myHttpWebRequest.Accept = "application/json";
            myHttpWebRequest.Method = "PUT";
            myHttpWebRequest.PreAuthenticate = true;
            myHttpWebRequest.Headers.Add("Authorization", "Basic " + TokenZoop().ToString() + "");
            myHttpWebRequest.Headers.Add("x-api-key", zoop.xapikey());

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
                return json;
            }
            catch (WebException ex)
            {
                var resp = new StreamReader(ex.Response.GetResponseStream()).ReadToEnd();
                return resp;
            }
        }

        // Busca
        if (tipo == "P")
        {
                PARAMS = "?taxpayer_id=" + CPFCNPJ.ToString();
                URL = "https://api.zoop.ws/v1/marketplaces/" + marketplace().ToString() + "/buyers/search";

                var myUri = new Uri(URL + PARAMS);
                var myWebRequest = WebRequest.Create(myUri);
                var myHttpWebRequest = (HttpWebRequest)myWebRequest;
                myHttpWebRequest.PreAuthenticate = true;
                myHttpWebRequest.ClientCertificates.Add(CertificateHelper.ClientCertificate);
                myHttpWebRequest.Accept = "application/json";
                myHttpWebRequest.Headers.Add("Authorization", "Basic " + TokenZoop().ToString() + "");
                myHttpWebRequest.Headers.Add("x-api-key", zoop.xapikey());

                try
                {
                    var myWebResponse = myWebRequest.GetResponse();
                    var responseStream = myWebResponse.GetResponseStream();

                    StreamReader myStreamReader = new StreamReader(responseStream, Encoding.UTF8);
                    json = myStreamReader.ReadToEnd();
                    return json;

                    //JObject o = JObject.Parse(json);
                    //return o["id"].ToString() + "/" + o["first_name"].ToString() + "/" + o["last_name"].ToString() + "/" + o["taxpayer_id"].ToString();
                }
                catch (WebException ex)
                {
                    var resp = new StreamReader(ex.Response.GetResponseStream()).ReadToEnd();
                    return resp;
                }

        }

        return "";
    }

    // ------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
    // TERMINAL E EQUIPAMENTOS
    // ------------------------------------------------------------------------------------------------------------------------------------------------------------------------------

    public static string ParearTerminal(string json)
    {
        // CONFIGURAÇÕES ACESSO ZOOP

        ServicePointManager.Expect100Continue = true;
        ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12
        | SecurityProtocolType.Tls11
        | SecurityProtocolType.Tls
        | SecurityProtocolType.Ssl3;

        var byteArray = Encoding.ASCII.GetBytes(zoop.Keymarketplace() + ":" + "");
        string encodeString = Convert.ToBase64String(byteArray);


        var URL = "https://api.zoop.ws/v1/marketplaces/" + zoop.marketplace().ToString() + "/terminals/pairing";
        var myUri = new Uri(URL);

        var myWebRequest = WebRequest.Create(myUri);
        var myHttpWebRequest = (HttpWebRequest)myWebRequest;
        myHttpWebRequest.ClientCertificates.Add(CertificateHelper.ClientCertificate);
        myHttpWebRequest.ContentType = "application/json";
        myHttpWebRequest.Accept = "application/json";
        myHttpWebRequest.Method = "POST";
        myHttpWebRequest.PreAuthenticate = true;
        myHttpWebRequest.Headers.Add("Authorization", "Basic " + encodeString.ToString() + "");
        myHttpWebRequest.Headers.Add("x-api-key", zoop.xapikey());      

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
            return json;
        }
        catch (WebException ex)
        {
            var resp = new StreamReader(ex.Response.GetResponseStream()).ReadToEnd();
            return resp;
        }
    }


    public static string TIRAACENTOS(string str)
    {
        str = str.Replace("-", "");
        str = str.Replace(".", "");
        str = str.Replace("/", "");
        str = str.Replace("(", "");
        str = str.Replace(")", "");
        str = str.Replace(" ", "");
        return str;
    }

    public static HttpResponseResult ConsultarSaldo(string id)
    {
        ServicePointManager.Expect100Continue = true;
        ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

        string marketplaceId = marketplace();
        string secretKey = zoop.Keymarketplace();    // live_sk_xxxxx ou test_sk_xxxxx
        string publicKey = zoop.xapikey();      // live_pk_xxxxx ou test_pk_xxxxx

        // Basic Auth correto (secret_key:)
        string authBase64 = Convert.ToBase64String(Encoding.ASCII.GetBytes(secretKey + ":"));

        string url = "https://api.zoop.ws/v1/marketplaces/" + marketplaceId + "/sellers/" + id.ToString() + "/balances";

        var request = (HttpWebRequest)WebRequest.Create(new Uri(url));

        // Adiciona certificado cliente Zoop
        request.ClientCertificates.Add(CertificateHelper.ClientCertificate);

        request.ContentType = "application/json";
        request.Accept = "application/json";
        request.Method = "GET";
        request.PreAuthenticate = true;

        // Headers obrigatórios
        request.Headers.Add("Authorization", "Basic " + authBase64);
        request.Headers.Add("x-api-key", publicKey);

        try
        {
            using (var response = (HttpWebResponse)request.GetResponse())
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

            return new HttpResponseResult(500, "Erro inesperado: " + ex.Message);
        }
    }

    // SPLIT --------------------------------------------

    public static string SplitTransacao(string IDTransacao, string json)
    {
        ServicePointManager.Expect100Continue = true;
        ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12
        | SecurityProtocolType.Tls11
        | SecurityProtocolType.Tls
        | SecurityProtocolType.Ssl3;

        var byteArray = Encoding.ASCII.GetBytes(zoop.Keymarketplace() + ":" + "");
        string encodeString = Convert.ToBase64String(byteArray);

        var myUri = new Uri("https://api.zoop.ws/v1/marketplaces/" + zoop.marketplace().ToString() + "/transactions/" + IDTransacao.ToString() + "/split_rules");


        
        var myWebRequest = WebRequest.Create(myUri);
        var myHttpWebRequest = (HttpWebRequest)myWebRequest;
        myHttpWebRequest.ClientCertificates.Add(CertificateHelper.ClientCertificate);
        myHttpWebRequest.ContentType = "application/json";
        myHttpWebRequest.Accept = "application/json";
        myHttpWebRequest.Method = "POST";
        myHttpWebRequest.PreAuthenticate = true;
        myHttpWebRequest.Headers.Add("Authorization", "Basic " + encodeString.ToString() + "");
        myHttpWebRequest.Headers.Add("x-api-key", zoop.xapikey());        

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
            return json;
        }
        catch (WebException ex)
        {
            var resp = new StreamReader(ex.Response.GetResponseStream()).ReadToEnd();
            return resp;
        }

    }

    // ***************************************************************************************************************************************************** //
    // Cadastro e credenciamento *************************************************************************************************************************** //
    // ***************************************************************************************************************************************************** //

    // VENDEDORES

    // COMPRADORES

    // DADOS BANCÁRIOS

    // PLANO DE VENDAS

    // ***************************************************************************************************************************************************** //
    // Gestão de transações e recebíveis ******************************************************************************************************************* //
    // ***************************************************************************************************************************************************** //
    
    



}