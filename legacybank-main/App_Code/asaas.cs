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
/// Summary description for asaas
/// </summary>
/// 

public class DadosCobrancaAsaaS
{
    public class Root
    {
        public string customer { get; set; }
        public string billingType { get; set; }
        public double value { get; set; }
        public string dueDate { get; set; }

        public string description { get; set; }
        public int daysAfterDueDateToRegistrationCancellation { get; set; }
        public string externalReference { get; set; }
        public int installmentCount { get; set; }
        public float totalValue { get; set; }
        public float installmentValue { get; set; }

        public discount discount { get; set; }
        public interest interest { get; set; }
        public fine fine { get; set; }
        
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public bool postalService { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public CreditCard creditCard { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public CreditCardHolderInfo creditCardHolderInfo { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string remoteIp { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public split[] split { get; set; }
        //public callback callback { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public bool authorizeOnly { get; set; }
    }

    public class discount
    {
        public float value { get; set; }
        public int dueDateLimitDays { get; set; }
        public string type { get; set; }
    }
    public class fine
    {
        public float value { get; set; }
        public string type { get; set; }
    }
    public class interest
    {
        public float value { get; set; }
    }

    public class split
    {
        public string walletId { get; set; }
        public double fixedValue { get; set; }
        public double percentualValue { get; set; }
        public double totalFixedValue { get; set; }
        public string externalReference { get; set; }
        public string description { get; set; }
    }

    public class CreditCard
    {
        public string holderName { get; set; }
        public string number { get; set; }
        public string expiryMonth { get; set; }
        public string expiryYear { get; set; }
        public string ccv { get; set; }
    }

    public class CreditCardHolderInfo
    {
        public string name { get; set; }
        public string email { get; set; }
        public string cpfCnpj { get; set; }
        public string postalCode { get; set; }
        public string addressNumber { get; set; }
        public object addressComplement { get; set; }
        public string phone { get; set; }
        public string mobilePhone { get; set; }
    }
    //public class callback
    //{
    //    public string successUrl { get; set; }
    //    public bool autoRedirect { get; set; }
    //}

}

public class DadosClienteAsaaS
{
    public class Root
    {
        public string name { get; set; }
        public string cpfCnpj { get; set; }
        public string email { get; set; }
        public string phone { get; set; }
        public string mobilePhone { get; set; }
        public string addressNumber { get; set; }
        public string postalCode { get; set; }
        public bool notificationDisabled { get; set; }
    }
}

public class dadosPagamentoAsaaS
{
    public class SimularPagamento
    {
        public string identificationField { get; set; }
        public string barCode { get; set; }
    }

    public class CriarPagamento
    {
        public string identificationField { get; set; }
        public DateTime scheduleDate { get; set; }
        public string description { get; set; }
        public float discount { get; set; }
        public float interest { get; set; }
        public float fine { get; set; }
        public DateTime dueDate { get; set; }
        public float value { get; set; }
        public string externalReference { get; set; }
    }
}
// TRANSAÇÕES PIX *******************************************************

public class dadosPagamentoPixAsaaS
{

    public class DecodificarQRCode
    {
        public string payload { get; set; }
        public float changeValue { get; set; }
    }

    public class PagarQRCode
    {
        public qrCode qrCode { get; set; }
        public float value { get; set; }
        public string description { get; set; }
        public DateTime scheduleDate { get; set; }
    }

    public class qrCode
    {
        public string payload { get; set; }
        public float changeValue { get; set; }
    }
}

// TRANSAÇÕES TRANSFERÊNCIA
public class dadosTransferenciaPixAsaaS
{
    public class Bank
    {
        public string code { get; set; }
    }

    public class BankAccount
    {
        public Bank bank { get; set; }
        public string accountName { get; set; }
        public string ownerName { get; set; }
        public string ownerBirthDate { get; set; }
        public string cpfCnpj { get; set; }
        public string agency { get; set; }
        public string account { get; set; }
        public string accountDigit { get; set; }
        public string bankAccountType { get; set; }
        public string ispb { get; set; }
    }

    public class Root
    {
        public double value { get; set; }
        public BankAccount bankAccount { get; set; }
        public string operationType { get; set; }
        public string pixAddressKey { get; set; }
        public string pixAddressKeyType { get; set; }
        public string description { get; set; }
        public DateTime scheduleDate { get; set; }
        public string externalReference { get; set; }
    }
}

public class dadosTransferenciaAsaaS
{
    public class Root
    {
        public double value { get; set; }
        public string walletId { get; set; }
        public string externalReference { get; set; }
    }
}

public class dadosSubconta
{
    public class Subconta
    {
        public string name { get; set; }
        public string email { get; set; }
        public string loginEmail { get; set; }
        public string cpfCnpj { get; set; }
        public string birthDate { get; set; }
        public string companyType { get; set; }
        public string phone { get; set; }
        public string mobilePhone { get; set; }
        public string site { get; set; }
        public int incomeValue { get; set; }
        public string address { get; set; }
        public string addressNumber { get; set; }
        public string complement { get; set; }
        public string province { get; set; }
        public string postalCode { get; set; }
        public webhooks[] webhooks { get; set; }
    }

    public class webhooks
    {
        public string name { get; set; }
        public string url { get; set; }
        public string email { get; set; }
        public string sendType { get; set; } // Sequencial (SEQUENTIALLY) ou não sequencial (NON_SEQUENTIALLY)
        public int apiVersion { get; set; } // Defaults to 3 - Versão utilizada da API. Utilize "3" para a versão v3
        public bool enabled { get; set; }
        public bool interrupted { get; set; }
        public string authToken { get; set; }
        public string[] events { get; set; }
    }
}

public class DadosCobrancaCartaoAsaaS
{
    public class Root
    {
        public string customer { get; set; }
        public string billingType { get; set; }
        public double value { get; set; }
        public string dueDate { get; set; }
        public int daysAfterDueDateToRegistrationCancellation {get; set;}
        public  string externalReference {get; set;}
        public int installmentCount {get; set;}
        public double totalValue {get; set;}
        public double installmentValue {get; set;}
        public CreditCard creditCard { get; set; }
        public CreditCardHolderInfo creditCardHolderInfo { get; set; }
        public string remoteIp { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public split[] split { get; set; }
        //public callback callback { get; set; }
    }

    
    public class CreditCard
    {
        public string holderName { get; set; }
        public string number { get; set; }
        public string expiryMonth { get; set; }
        public string expiryYear { get; set; }
        public string ccv { get; set; }
    }

    public class CreditCardHolderInfo
    {
        public string name { get; set; }
        public string email { get; set; }
        public string cpfCnpj { get; set; }
        public string postalCode { get; set; }
        public string addressNumber { get; set; }
        public object addressComplement { get; set; }
        public string phone { get; set; }
        public string mobilePhone { get; set; }
    }

    public class split
    {
        public string walletId { get; set; }
        public double fixedValue { get; set; }
        public double percentualValue { get; set; }
        public double totalFixedValue { get; set; }
        public string externalReference { get; set; }
        public string description { get; set; }
    }

}

public class DadosRedefinirApiKeyAsaas
{
    public class root
    {
        public string id { get; set; }
        public string email { get; set; }
        public string password { get; set; }
    }
}

public class DadosAssinaturaCartao
{
    public class CreditCard
    {
        public string holderName { get; set; }
        public string number { get; set; }
        public string expiryMonth { get; set; }
        public string expiryYear { get; set; }
        public string ccv { get; set; }
    }

    public class CreditCardHolderInfo
    {
        public string name { get; set; }
        public string email { get; set; }
        public string cpfCnpj { get; set; }
        public string postalCode { get; set; }
        public string addressNumber { get; set; }
        public object addressComplement { get; set; }
        public string phone { get; set; }
        public string mobilePhone { get; set; }
    }

    public class Root
    {
        public string customer { get; set; }
        public string billingType { get; set; }
        public string nextDueDate { get; set; }
        public double value { get; set; }
        public string cycle { get; set; }
        public string description { get; set; }
        public string remoteIp { get; set; }
        public CreditCard creditCard { get; set; }
        public CreditCardHolderInfo creditCardHolderInfo { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public split[] split { get; set; }
    }

    public class split
    {
        public string walletId { get; set; }
        public double fixedValue { get; set; }
        public double percentualValue { get; set; }
        public string externalReference { get; set; }
        public string description { get; set; }
    }

}

public class SolicitarCartaoAsaas
{
    public class Root
    {
        public string name { get; set; }
        public string cpf { get; set; }
        public string email { get; set; }
        public string mobilePhone { get; set; }
        public string birthDate { get; set; }
        public string postalCode { get; set; }
        public string address { get; set; }
        public string addressNumber { get; set; }
        public string province { get; set; }
        public string complement { get; set; }
        public string cardName { get; set; }
        public string type { get; set; }
    }
}

public class AtivarCartaoAsaas
{
    public class Root
    {
        public string pin { get; set; }
        public string pinConfirmation { get; set; }
        public string lastDigits { get; set; }
    }
}

public class AtualizarDadosCadastrais
{
    public class Root
    {
        public string personType { get; set; }
        public string cpfCnpj { get; set; }
        public DateTime birthDate { get; set; }
        public string companyType { get; set; }
        public string companyName { get; set; }
        public string incomeValue { get; set; }
        public string email { get; set; }
        public string phone { get; set; }
        public string mobilePhone { get; set; }
        public string site { get; set; }
        public string postalCode { get; set; }
        public string address { get; set; }
        public string addressNumber { get; set; }
        public string complement { get; set; }
        public string Bairro { get; set; }
    }
}


public class TrocarNomeCartaoAsaas
{
    public class Root
    {
        public string name { get; set; }
    }
}

public class TrocarSenhaCartaoAsaas
{
    public class Root
    {
        public string pin { get; set; }
    }
}

public class asaas
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


    public class DadosAntecipacaoAutomatica
    {
        public bool creditCardAutomaticEnabled { get; set; }
    }

    public static string TokenAsaas()
    {
        string sPadrao = "$aact_YTU5YTE0M2M2N2I4MTliNzk0YTI5N2U5MzdjNWZmNDQ6OjAwMDAwMDAwMDAwMDA0NTM1OTg6OiRhYWNoX2U4MTIzZDJmLTY3Y2YtNDkyOC1hZjE4LWY2ODBkZGYzZGM0MA==";

        SqlConnection mySelCadastro = new SqlConnection(Funcoes.conexao());
        mySelCadastro.Open();
        SqlCommand cmdSelCadastro = new SqlCommand("dbo.stp_pessoas_fj_integracoes_chaves_ins", mySelCadastro);
        cmdSelCadastro.CommandType = CommandType.StoredProcedure;
        cmdSelCadastro.Parameters.Add("@flg_operacao", SqlDbType.Char).Value = "L";
        cmdSelCadastro.Parameters.Add("@COD_ID_INTEGRACOES", SqlDbType.Int).Value = 6;
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


    public static string[] ListaWebhook = {
                            "ACCOUNT_STATUS_GENERAL_APPROVAL_REJECTED",
                            "ACCOUNT_STATUS_GENERAL_APPROVAL_AWAITING_APPROVAL",
                            "ACCOUNT_STATUS_DOCUMENT_REJECTED",
                            "ACCOUNT_STATUS_DOCUMENT_AWAITING_APPROVAL",
                            "ACCOUNT_STATUS_COMMERCIAL_INFO_REJECTED",
                            "ACCOUNT_STATUS_COMMERCIAL_INFO_AWAITING_APPROVAL",
                            "ACCOUNT_STATUS_BANK_ACCOUNT_INFO_REJECTED",
                            "ACCOUNT_STATUS_BANK_ACCOUNT_INFO_AWAITING_APPROVAL",
                            "ACCOUNT_STATUS_GENERAL_APPROVAL_PENDING",
                            "ACCOUNT_STATUS_GENERAL_APPROVAL_APPROVED",
                            "ACCOUNT_STATUS_DOCUMENT_PENDING",
                            "ACCOUNT_STATUS_DOCUMENT_APPROVED",
                            "ACCOUNT_STATUS_COMMERCIAL_INFO_PENDING",
                            "ACCOUNT_STATUS_COMMERCIAL_INFO_APPROVED",
                            "ACCOUNT_STATUS_BANK_ACCOUNT_INFO_PENDING",
                            "ACCOUNT_STATUS_BANK_ACCOUNT_INFO_APPROVED",
                            "MOBILE_PHONE_RECHARGE_REFUNDED",
                            "MOBILE_PHONE_RECHARGE_CANCELLED",
                            "MOBILE_PHONE_RECHARGE_CONFIRMED",
                            "MOBILE_PHONE_RECHARGE_PENDING",
                            "RECEIVABLE_ANTICIPATION_DENIED",
                            "RECEIVABLE_ANTICIPATION_CREDITED",
                            "RECEIVABLE_ANTICIPATION_SCHEDULED",
                            "RECEIVABLE_ANTICIPATION_OVERDUE",
                            "RECEIVABLE_ANTICIPATION_DEBITED",
                            "RECEIVABLE_ANTICIPATION_PENDING",
                            "RECEIVABLE_ANTICIPATION_CANCELLED",
                            "BILL_FAILED",
                            "BILL_PAID",
                            "BILL_PENDING",
                            "BILL_REFUNDED",
                            "BILL_CANCELLED",
                            "BILL_BANK_PROCESSING",
                            "BILL_CREATED",
                            "TRANSFER_FAILED",
                            "TRANSFER_BLOCKED",
                            "TRANSFER_PENDING",
                            "TRANSFER_CANCELLED",
                            "TRANSFER_DONE",
                            "TRANSFER_IN_BANK_PROCESSING",
                            "TRANSFER_CREATED",
                            "INVOICE_ERROR",
                            "INVOICE_CANCELED",
                            "INVOICE_AUTHORIZED",
                            "INVOICE_UPDATED",
                            "INVOICE_CANCELLATION_DENIED",
                            "INVOICE_PROCESSING_CANCELLATION",
                            "INVOICE_SYNCHRONIZED",
                            "INVOICE_CREATED",
                            "PAYMENT_CREDIT_CARD_CAPTURE_REFUSED",
                            "PAYMENT_BANK_SLIP_VIEWED",
                            "PAYMENT_DUNNING_RECEIVED",
                            "PAYMENT_CHARGEBACK_DISPUTE",
                            "PAYMENT_RECEIVED_IN_CASH_UNDONE",
                            "PAYMENT_REFUND_IN_PROGRESS",
                            "PAYMENT_RESTORED",
                            "PAYMENT_OVERDUE",
                            "PAYMENT_RECEIVED",
                            "PAYMENT_UPDATED",
                            "PAYMENT_REPROVED_BY_RISK_ANALYSIS",
                            "PAYMENT_AWAITING_RISK_ANALYSIS",
                            "PAYMENT_PARTIALLY_REFUNDED",
                            "PAYMENT_CHECKOUT_VIEWED",
                            "PAYMENT_DUNNING_REQUESTED",
                            "PAYMENT_AWAITING_CHARGEBACK_REVERSAL",
                            "PAYMENT_CHARGEBACK_REQUESTED",
                            "PAYMENT_REFUND_DENIED",
                            "PAYMENT_REFUNDED",
                            "PAYMENT_DELETED",
                            "PAYMENT_ANTICIPATED",
                            "PAYMENT_CONFIRMED",
                            "PAYMENT_CREATED",
                            "PAYMENT_APPROVED_BY_RISK_ANALYSIS",
                            "PAYMENT_AUTHORIZED"
                                    };


    public static string UrlAPI()
    {
        return "https://api.asaas.com/v3";
    }
    //----------------------------------------------------------------------------------------------------------------------------------------------------------------------
    // NOVAS ROTINAS COM RETORNO HTTP CODE
    //----------------------------------------------------------------------------------------------------------------------------------------------------------------------

    public static HttpResponseResult ExcluirCobrancaAsaas(string apiKey, string sIDCobranca)
    {
        ServicePointManager.Expect100Continue = true;
        ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12
            | SecurityProtocolType.Tls11
            | SecurityProtocolType.Tls
            | SecurityProtocolType.Ssl3;

        var byteArray = Encoding.ASCII.GetBytes(zoop.Keymarketplace() + ":" + "");
        string encodeString = Convert.ToBase64String(byteArray);

        string url = asaas.UrlAPI().ToString() + "/payments/"+sIDCobranca;
        var myUri = new Uri(url);
        var myHttpWebRequest = (HttpWebRequest)WebRequest.Create(myUri);

        myHttpWebRequest.Method = "DELETE";
        myHttpWebRequest.UserAgent = "bankasaas";
        myHttpWebRequest.Headers.Add("access_token", apiKey.ToString());
        myHttpWebRequest.Accept = "application/json";
        myHttpWebRequest.ContentType = "application/json";
        myHttpWebRequest.PreAuthenticate = true;

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

    public static HttpResponseResult ObterQRCodeCobrancaAsaas(string apiKey, string sIDCobranca)
    {
        ServicePointManager.Expect100Continue = true;
        ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12
            | SecurityProtocolType.Tls11
            | SecurityProtocolType.Tls
            | SecurityProtocolType.Ssl3;

        var byteArray = Encoding.ASCII.GetBytes(zoop.Keymarketplace() + ":" + "");
        string encodeString = Convert.ToBase64String(byteArray);

        string url = asaas.UrlAPI().ToString() + "/payments/" + sIDCobranca + "/pixQrCode";
        var myUri = new Uri(url);
        var myHttpWebRequest = (HttpWebRequest)WebRequest.Create(myUri);

        myHttpWebRequest.Method = "GET";
        myHttpWebRequest.UserAgent = "bankasaas";
        myHttpWebRequest.Headers.Add("access_token", apiKey.ToString());
        myHttpWebRequest.Accept = "application/json";
        myHttpWebRequest.ContentType = "application/json";
        myHttpWebRequest.PreAuthenticate = true;

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

    public static HttpResponseResult RecuperarDadosComerciais(string apiKey)
    {
        ServicePointManager.Expect100Continue = true;
        ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12
            | SecurityProtocolType.Tls11
            | SecurityProtocolType.Tls
            | SecurityProtocolType.Ssl3;

        var byteArray = Encoding.ASCII.GetBytes(zoop.Keymarketplace() + ":" + "");
        string encodeString = Convert.ToBase64String(byteArray);

        string url = asaas.UrlAPI().ToString() + "/myAccount/commercialInfo";
        var myUri = new Uri(url);
        var myHttpWebRequest = (HttpWebRequest)WebRequest.Create(myUri);

        myHttpWebRequest.Method = "GET";
        myHttpWebRequest.UserAgent = "bankasaas";
        myHttpWebRequest.Headers.Add("access_token", apiKey.ToString());
        myHttpWebRequest.Accept = "application/json";
        myHttpWebRequest.ContentType = "application/json";
        myHttpWebRequest.PreAuthenticate = true;

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


    public static HttpResponseResult ListarAssinaturasAsaas(string apiKey)
    {
        ServicePointManager.Expect100Continue = true;
        ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12
            | SecurityProtocolType.Tls11
            | SecurityProtocolType.Tls
            | SecurityProtocolType.Ssl3;

        var byteArray = Encoding.ASCII.GetBytes(zoop.Keymarketplace() + ":" + "");
        string encodeString = Convert.ToBase64String(byteArray);

        string url = asaas.UrlAPI().ToString() + "/subscriptions";
        var myUri = new Uri(url);
        var myHttpWebRequest = (HttpWebRequest)WebRequest.Create(myUri);

        myHttpWebRequest.Method = "GET";
        myHttpWebRequest.UserAgent = "bankasaas";
        myHttpWebRequest.Headers.Add("access_token", apiKey.ToString());
        myHttpWebRequest.Accept = "application/json";
        myHttpWebRequest.ContentType = "application/json";
        myHttpWebRequest.PreAuthenticate = true;

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


    public static HttpResponseResult RemoverAssinaturasAsaas(string apiKey, string id)
    {
        ServicePointManager.Expect100Continue = true;
        ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12
            | SecurityProtocolType.Tls11
            | SecurityProtocolType.Tls
            | SecurityProtocolType.Ssl3;

        var byteArray = Encoding.ASCII.GetBytes(zoop.Keymarketplace() + ":" + "");
        string encodeString = Convert.ToBase64String(byteArray);

        string url = asaas.UrlAPI().ToString() + "/subscriptions/"+id.ToString();
        var myUri = new Uri(url);
        var myHttpWebRequest = (HttpWebRequest)WebRequest.Create(myUri);

        myHttpWebRequest.Method = "DELETE";
        myHttpWebRequest.UserAgent = "bankasaas";
        myHttpWebRequest.Headers.Add("access_token", apiKey.ToString());
        myHttpWebRequest.Accept = "application/json";
        myHttpWebRequest.ContentType = "application/json";
        myHttpWebRequest.PreAuthenticate = true;

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

    public static HttpResponseResult EstornarCobrancaAsaas(string apiKey, string json, string id)
    {
        ServicePointManager.Expect100Continue = true;
        ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12
            | SecurityProtocolType.Tls11
            | SecurityProtocolType.Tls
            | SecurityProtocolType.Ssl3;

        var byteArray = Encoding.ASCII.GetBytes(zoop.Keymarketplace() + ":" + "");
        string encodeString = Convert.ToBase64String(byteArray);

        string url = asaas.UrlAPI().ToString() + "/payments/"+id.ToString()+"/refund";
        var myUri = new Uri(url);
        var myHttpWebRequest = (HttpWebRequest)WebRequest.Create(myUri);

        myHttpWebRequest.Method = "POST";
        myHttpWebRequest.UserAgent = "bankasaas";
        myHttpWebRequest.Headers.Add("access_token", apiKey.ToString());
        myHttpWebRequest.Accept = "application/json";
        myHttpWebRequest.ContentType = "application/json";
        myHttpWebRequest.PreAuthenticate = true;

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



    public static HttpResponseResult CriarCobrancaAsaas(string apiKey, string json)
    {
        ServicePointManager.Expect100Continue = true;
        ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12
            | SecurityProtocolType.Tls11
            | SecurityProtocolType.Tls
            | SecurityProtocolType.Ssl3;

        var byteArray = Encoding.ASCII.GetBytes(zoop.Keymarketplace() + ":" + "");
        string encodeString = Convert.ToBase64String(byteArray);

        string url = asaas.UrlAPI().ToString() + "/payments";
        var myUri = new Uri(url);
        var myHttpWebRequest = (HttpWebRequest)WebRequest.Create(myUri);

        myHttpWebRequest.Method = "POST";
        myHttpWebRequest.UserAgent = "bankasaas";
        myHttpWebRequest.Headers.Add("access_token", apiKey.ToString());
        myHttpWebRequest.Accept = "application/json";
        myHttpWebRequest.ContentType = "application/json";
        myHttpWebRequest.PreAuthenticate = true;

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


    //----------------------------------------------------------------------------------------------------------------------------------------------------------------------
    // FIM NOVAS ROTINAS COM RETORNO HTTP CODE
    //----------------------------------------------------------------------------------------------------------------------------------------------------------------------

    public static string ConfigurarFatura(string apiKey, string json)
    {
        ServicePointManager.Expect100Continue = true;
        ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12
        | SecurityProtocolType.Tls11
        | SecurityProtocolType.Tls
        | SecurityProtocolType.Ssl3;

        try
        {
            string URL = "https://api.asaas.com/v3/myAccount/paymentCheckoutConfig/";

            var myUri = new Uri(URL);

            var myWebRequest = WebRequest.Create(myUri);
            var myHttpWebRequest = (HttpWebRequest)myWebRequest;
            myHttpWebRequest.PreAuthenticate = true;
            myHttpWebRequest.Method = "GET";
            myHttpWebRequest.UserAgent = "bankasaas";
            myHttpWebRequest.Headers.Add("access_token", asaas.TokenAsaas().ToString());
            myHttpWebRequest.Accept = "application/json";
            myHttpWebRequest.ContentType = "application/x-www-form-urlencoded";

            // Criação conteúdo body

            using (var streamWriter = new StreamWriter(myHttpWebRequest.GetRequestStream()))
            {
                streamWriter.Write(json);
                streamWriter.Flush();
            }

            /*

     --form logoFile='@100-online.png' \
     --form 'logoBackgroundColor=#ff0000' \
     --form 'infoBackgroundColor=#ff0000' \
     --form 'fontColor=#000000' \
     --form enabled=true             
     
             ou
             
    request.AddFile("logoFile", "100-online.png");
    request.AddParameter("logoBackgroundColor", "#ff0000");
    request.AddParameter("infoBackgroundColor", "#ff0000");
    request.AddParameter("fontColor", "#000000");
    request.AddParameter("enabled", "true");             
              
             */


            var myWebResponse = myWebRequest.GetResponse();
            var responseStream = myWebResponse.GetResponseStream();

            StreamReader myStreamReader = new StreamReader(responseStream, Encoding.UTF8);
            json = myStreamReader.ReadToEnd();

            return json;
        }
        catch
        {
            return "";
        }
        return "";
    }



    public static string ListarClientes()
    {
        ServicePointManager.Expect100Continue = true;
        ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12
        | SecurityProtocolType.Tls11
        | SecurityProtocolType.Tls
        | SecurityProtocolType.Ssl3;

        try
        {
            string URL = "https://api.asaas.com/v3/customers";

            var myUri = new Uri(URL);

            var myWebRequest = WebRequest.Create(myUri);
            var myHttpWebRequest = (HttpWebRequest)myWebRequest;
            myHttpWebRequest.PreAuthenticate = true;
            myHttpWebRequest.Method = "GET";
            myHttpWebRequest.UserAgent = "bankasaas";
            myHttpWebRequest.Headers.Add("access_token", asaas.TokenAsaas().ToString());
            myHttpWebRequest.Accept = "application/json";

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
        return "";
    }


    public static string ListarCliente(string apiKey, string sCNPJCPF)
    {
        ServicePointManager.Expect100Continue = true;
        ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12
        | SecurityProtocolType.Tls11
        | SecurityProtocolType.Tls
        | SecurityProtocolType.Ssl3;

        string URL = "https://api.asaas.com/v3/customers";

        var PARAM = "?cpfCnpj=" + sCNPJCPF.ToString();
        var myUri = new Uri(URL + PARAM);

        string sResposta = "";
        string json = "";

        var myWebRequest = WebRequest.Create(myUri);
        var myHttpWebRequest = (HttpWebRequest)myWebRequest;
        myHttpWebRequest.PreAuthenticate = true;
        myHttpWebRequest.Method = "GET";
        myHttpWebRequest.UserAgent = "bankasaas";
        myHttpWebRequest.Headers.Add("access_token", (apiKey.ToString().Trim() == "") ? asaas.TokenAsaas().ToString() : apiKey.ToString());
        myHttpWebRequest.Accept = "application/json";

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

    public static string RecuperarConta(string apiKey)
    {
        ServicePointManager.Expect100Continue = true;
        ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12
        | SecurityProtocolType.Tls11
        | SecurityProtocolType.Tls
        | SecurityProtocolType.Ssl3;

        try
        {
            string URL = "https://api.asaas.com/v3/myAccount/accountNumber/";

            var myUri = new Uri(URL);

            var myWebRequest = WebRequest.Create(myUri);
            var myHttpWebRequest = (HttpWebRequest)myWebRequest;
            myHttpWebRequest.PreAuthenticate = true;
            myHttpWebRequest.Method = "GET";
            myHttpWebRequest.UserAgent = "bankasaas";
            myHttpWebRequest.Headers.Add("access_token", (apiKey.ToString().Trim() == "") ? asaas.TokenAsaas().ToString() : apiKey.ToString());
            myHttpWebRequest.Accept = "application/json";

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
        return "";
    }


    public static string ListarWebhooks()
    {
        ServicePointManager.Expect100Continue = true;
        ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12
        | SecurityProtocolType.Tls11
        | SecurityProtocolType.Tls
        | SecurityProtocolType.Ssl3;

        try
        {
            string URL = "https://api.asaas.com/v3/webhooks";

            var myUri = new Uri(URL);

            var myWebRequest = WebRequest.Create(myUri);
            var myHttpWebRequest = (HttpWebRequest)myWebRequest;
            myHttpWebRequest.PreAuthenticate = true;
            myHttpWebRequest.Method = "GET";
            myHttpWebRequest.UserAgent = "bankasaas";
            myHttpWebRequest.Headers.Add("access_token", asaas.TokenAsaas().ToString());
            myHttpWebRequest.Accept = "application/json";

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
        return "";
    }

    public static string ListarSubcontas()
    {
        ServicePointManager.Expect100Continue = true;
        ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12
        | SecurityProtocolType.Tls11
        | SecurityProtocolType.Tls
        | SecurityProtocolType.Ssl3;

        string URL = "https://api.asaas.com/v3/accounts?limit=100";

        var myUri = new Uri(URL);

        var myWebRequest = WebRequest.Create(myUri);
        var myHttpWebRequest = (HttpWebRequest)myWebRequest;
        myHttpWebRequest.PreAuthenticate = true;
        myHttpWebRequest.Method = "GET";
        myHttpWebRequest.UserAgent = "bankasaas";
        myHttpWebRequest.Headers.Add("access_token", asaas.TokenAsaas().ToString());
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

    public static string RecuperarWalletID(string apiKey)
    {
        ServicePointManager.Expect100Continue = true;
        ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12
        | SecurityProtocolType.Tls11
        | SecurityProtocolType.Tls
        | SecurityProtocolType.Ssl3;

        string URL = "https://api.asaas.com/v3/wallets";

        var myUri = new Uri(URL);

        var myWebRequest = WebRequest.Create(myUri);
        var myHttpWebRequest = (HttpWebRequest)myWebRequest;
        myHttpWebRequest.PreAuthenticate = true;
        myHttpWebRequest.Method = "GET";
        myHttpWebRequest.UserAgent = "bankasaas";
        myHttpWebRequest.Headers.Add("access_token", (apiKey.ToString().Trim() == "") ? asaas.TokenAsaas().ToString() : apiKey.ToString());
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



    public static string ListarSubcontaID(string apiKey, string ID)
    {
        ServicePointManager.Expect100Continue = true;
        ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12
        | SecurityProtocolType.Tls11
        | SecurityProtocolType.Tls
        | SecurityProtocolType.Ssl3;

        string URL = "https://api.asaas.com/v3/accounts/" + ID.ToString();

        var myUri = new Uri(URL);

        var myWebRequest = WebRequest.Create(myUri);
        var myHttpWebRequest = (HttpWebRequest)myWebRequest;
        myHttpWebRequest.PreAuthenticate = true;
        myHttpWebRequest.Method = "GET";
        myHttpWebRequest.UserAgent = "bankasaas";
        myHttpWebRequest.Headers.Add("access_token", (apiKey.ToString().Trim() == "") ? asaas.TokenAsaas().ToString() : apiKey.ToString());
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


    public static string ListarSubcontasCNPJCPF(string sCNPJCPF)
    {
        ServicePointManager.Expect100Continue = true;
        ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12
        | SecurityProtocolType.Tls11
        | SecurityProtocolType.Tls
        | SecurityProtocolType.Ssl3;

        string URL = "https://api.asaas.com/v3/accounts";

        var myUri = new Uri(URL) + "?cpfCnpj=" + sCNPJCPF.ToString();

        var myWebRequest = WebRequest.Create(myUri);
        var myHttpWebRequest = (HttpWebRequest)myWebRequest;
        myHttpWebRequest.PreAuthenticate = true;
        myHttpWebRequest.Method = "GET";
        myHttpWebRequest.UserAgent = "bankasaas";
        myHttpWebRequest.Headers.Add("access_token", asaas.TokenAsaas().ToString());
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


    public static string ListarDocumentosPendentes(string apiKey)
    {
        ServicePointManager.Expect100Continue = true;
        ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12
        | SecurityProtocolType.Tls11
        | SecurityProtocolType.Tls
        | SecurityProtocolType.Ssl3;

        string URL = "https://api.asaas.com/v3/myAccount/documents";

        var myUri = new Uri(URL);

        var myWebRequest = WebRequest.Create(myUri);
        var myHttpWebRequest = (HttpWebRequest)myWebRequest;
        myHttpWebRequest.PreAuthenticate = true;
        myHttpWebRequest.Method = "GET";
        myHttpWebRequest.UserAgent = "bankasaas";
        myHttpWebRequest.Headers.Add("access_token", apiKey.ToString());
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


    public static string ListarStatus(string apiKey)
    {
        ServicePointManager.Expect100Continue = true;
        ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12
        | SecurityProtocolType.Tls11
        | SecurityProtocolType.Tls
        | SecurityProtocolType.Ssl3;

        string URL = "https://api.asaas.com/v3/myAccount/status";

        var myUri = new Uri(URL);

        var myWebRequest = WebRequest.Create(myUri);
        var myHttpWebRequest = (HttpWebRequest)myWebRequest;
        myHttpWebRequest.PreAuthenticate = true;
        myHttpWebRequest.Method = "GET";
        myHttpWebRequest.UserAgent = "bankasaas";
        myHttpWebRequest.Headers.Add("access_token", apiKey.ToString());
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

    public static string ListarChavePix(string apiKey)
    {
        ServicePointManager.Expect100Continue = true;
        ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12
        | SecurityProtocolType.Tls11
        | SecurityProtocolType.Tls
        | SecurityProtocolType.Ssl3;

        string URL = "https://api.asaas.com/v3/pix/addressKeys";

        var myUri = new Uri(URL);

        var myWebRequest = WebRequest.Create(myUri);
        var myHttpWebRequest = (HttpWebRequest)myWebRequest;
        myHttpWebRequest.PreAuthenticate = true;
        myHttpWebRequest.Method = "GET";
        myHttpWebRequest.UserAgent = "bankasaas";
        myHttpWebRequest.Headers.Add("access_token", apiKey.ToString());
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

    public static string PegarTokenSubconta(int codigo, int licenciado)
    {

        string sToken = "";
        SqlConnection mySelCadastro = new SqlConnection(Funcoes.conexao());
        mySelCadastro.Open();
        SqlCommand cmdSelCadastro = new SqlCommand("dbo.stp_pessoas_fj_baas_ins", mySelCadastro);
        cmdSelCadastro.CommandType = CommandType.StoredProcedure;
        cmdSelCadastro.Parameters.Add("@flg_operacao", SqlDbType.Char).Value = "S";
        cmdSelCadastro.Parameters.Add("@COD_ID_PESSOAS_FJ", SqlDbType.Int).Value = Funcoes.strToInt(codigo.ToString());//Funcoes.strToInt(HttpContext.Current.Session["PESSOA"].ToString());
        cmdSelCadastro.Parameters.Add("@COD_ID_PESSOA_LICENCIADO", SqlDbType.Int).Value = Funcoes.strToInt(licenciado.ToString());//Funcoes.strToInt(HttpContext.Current.Session["LICENCIADO"].ToString());
        cmdSelCadastro.Parameters.Add("@FLG_ORIGEM", SqlDbType.Char).Value = "A";

        SqlDataReader ReaderCadastro = cmdSelCadastro.ExecuteReader();
        while (ReaderCadastro.Read())
        {
            sToken = ReaderCadastro["NUM_TOKEN_BAAS"].ToString();
        }
        return sToken;
    }


    public static string CriarChavePix(string apiKey, string json)
    {
        ServicePointManager.Expect100Continue = true;
        ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12
        | SecurityProtocolType.Tls11
        | SecurityProtocolType.Tls
        | SecurityProtocolType.Ssl3;

        string URL = "https://api.asaas.com/v3/pix/addressKeys";

        var myUri = new Uri(URL);
        string sResposta = "";

        var myWebRequest = WebRequest.Create(myUri);
        var myHttpWebRequest = (HttpWebRequest)myWebRequest;
        myHttpWebRequest.PreAuthenticate = true;
        myHttpWebRequest.Method = "POST";
        myHttpWebRequest.UserAgent = "bankasaas";
        myHttpWebRequest.Headers.Add("access_token", apiKey.ToString());
        myHttpWebRequest.Accept = "application/json";

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

        // Salvar na Base de dados de Processos e eventos
        // COD_ID_PESSOA_LICENCIADO
        // COD_ID_PESSOAS_FJ
        // COD_ID
        // NOM_OPERACAO "Subcontas"
        // NOM_METODO "POST" / "GET"
        // NOM_ORIGEM "ASAAS" / "ZOOP"

        return sResposta;
    }




    public static string ListarTaxas(string apiKey)
    {
        ServicePointManager.Expect100Continue = true;
        ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12
        | SecurityProtocolType.Tls11
        | SecurityProtocolType.Tls
        | SecurityProtocolType.Ssl3;

        string URL = "https://api.asaas.com/v3/myAccount/fees";

        var myUri = new Uri(URL);

        var myWebRequest = WebRequest.Create(myUri);
        var myHttpWebRequest = (HttpWebRequest)myWebRequest;
        myHttpWebRequest.UserAgent = "bankasaas";
        myHttpWebRequest.PreAuthenticate = true;
        myHttpWebRequest.Method = "GET";
        myHttpWebRequest.Headers.Add("access_token", (apiKey.ToString().Trim()=="")? asaas.TokenAsaas().ToString(): apiKey.ToString());
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


    public static string CriarSubconta(string json)
    {
        ServicePointManager.Expect100Continue = true;
        ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12
        | SecurityProtocolType.Tls11
        | SecurityProtocolType.Tls
        | SecurityProtocolType.Ssl3;

        string URL = "https://api.asaas.com/v3/accounts";

        var myUri = new Uri(URL);

        string sResposta = "";

        var myWebRequest = WebRequest.Create(myUri);
        var myHttpWebRequest = (HttpWebRequest)myWebRequest;
        myHttpWebRequest.ContentType = "application/json";
        myHttpWebRequest.Accept = "application/json";
        myHttpWebRequest.Method = "POST";
        myHttpWebRequest.PreAuthenticate = true;
        myHttpWebRequest.UserAgent = "bankasaas";
        myHttpWebRequest.Headers.Add("access_token", asaas.TokenAsaas().ToString());

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

        // Salvar na Base de dados de Processos e eventos
        // COD_ID_PESSOA_LICENCIADO
        // COD_ID_PESSOAS_FJ
        // COD_ID
        // NOM_OPERACAO "Subcontas"
        // NOM_METODO "POST" / "GET"
        // NOM_ORIGEM "ASAAS" / "ZOOP"

        return sResposta;
    }

    public static string RedefinirSubconta(string json)
    {
        ServicePointManager.Expect100Continue = true;
        ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12
        | SecurityProtocolType.Tls11
        | SecurityProtocolType.Tls
        | SecurityProtocolType.Ssl3;

        string URL = "https://api.asaas.com/v3/accounts/redefineApiKey";

        var myUri = new Uri(URL);

        string sResposta = "";

        var myWebRequest = WebRequest.Create(myUri);
        var myHttpWebRequest = (HttpWebRequest)myWebRequest;
        myHttpWebRequest.ContentType = "application/json";
        myHttpWebRequest.Accept = "application/json";
        myHttpWebRequest.Method = "POST";
        myHttpWebRequest.PreAuthenticate = true;
        myHttpWebRequest.UserAgent = "bankasaas";
        myHttpWebRequest.Headers.Add("access_token", asaas.TokenAsaas().ToString());

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

    // ----------------------------------- PAGAMENTOS ------------------------------------------------------------------//


    public static string SimularPagamento(string apiKey, string json)
    {
        ServicePointManager.Expect100Continue = true;
        ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12
        | SecurityProtocolType.Tls11
        | SecurityProtocolType.Tls
        | SecurityProtocolType.Ssl3;

        string URL = "https://api.asaas.com/v3/bill/simulate";

        var myUri = new Uri(URL);

        string sResposta = "";

        var myWebRequest = WebRequest.Create(myUri);
        var myHttpWebRequest = (HttpWebRequest)myWebRequest;
        myHttpWebRequest.ContentType = "application/json";
        myHttpWebRequest.Accept = "application/json";
        myHttpWebRequest.Method = "POST";
        myHttpWebRequest.PreAuthenticate = true;
        myHttpWebRequest.UserAgent = "bankasaas";
        myHttpWebRequest.Headers.Add("access_token", (apiKey.ToString().Trim() == "") ? asaas.TokenAsaas().ToString() : apiKey.ToString());

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


    public static string CriarPagamento(string apiKey, string json)
    {
        ServicePointManager.Expect100Continue = true;
        ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12
        | SecurityProtocolType.Tls11
        | SecurityProtocolType.Tls
        | SecurityProtocolType.Ssl3;

        string URL = "https://api.asaas.com/v3/bill";

        var myUri = new Uri(URL);

        string sResposta = "";

        var myWebRequest = WebRequest.Create(myUri);
        var myHttpWebRequest = (HttpWebRequest)myWebRequest;
        myHttpWebRequest.ContentType = "application/json";
        myHttpWebRequest.Accept = "application/json";
        myHttpWebRequest.Method = "POST";
        myHttpWebRequest.PreAuthenticate = true;
        myHttpWebRequest.UserAgent = "bankasaas";
        myHttpWebRequest.Headers.Add("access_token", (apiKey.ToString().Trim() == "") ? asaas.TokenAsaas().ToString() : apiKey.ToString());

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

    public static string CriarPagamentoQRCode(string apiKey, string json)
    {
        ServicePointManager.Expect100Continue = true;
        ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12
        | SecurityProtocolType.Tls11
        | SecurityProtocolType.Tls
        | SecurityProtocolType.Ssl3;

        string URL = "https://api.asaas.com/v3/pix/qrCodes/pay";

        var myUri = new Uri(URL);

        string sResposta = "";

        var myWebRequest = WebRequest.Create(myUri);
        var myHttpWebRequest = (HttpWebRequest)myWebRequest;
        myHttpWebRequest.ContentType = "application/json";
        myHttpWebRequest.Accept = "application/json";
        myHttpWebRequest.Method = "POST";
        myHttpWebRequest.PreAuthenticate = true;
        myHttpWebRequest.UserAgent = "bankasaas";
        myHttpWebRequest.Headers.Add("access_token", (apiKey.ToString().Trim() == "") ? asaas.TokenAsaas().ToString() : apiKey.ToString());

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


    // ----------------------------------- CONTAS ---------------------------------------------------------------------- //
    public static string DetalheTransferencia(string apiKey, string sFiltro)
    {
        ServicePointManager.Expect100Continue = true;
        ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12
        | SecurityProtocolType.Tls11
        | SecurityProtocolType.Tls
        | SecurityProtocolType.Ssl3;

        string URL = "https://api.asaas.com/v3//transfers/";

        var myUri = new Uri(URL + sFiltro.ToString());
        string sResposta = "";

        var myWebRequest = WebRequest.Create(myUri);
        var myHttpWebRequest = (HttpWebRequest)myWebRequest;
        myHttpWebRequest.PreAuthenticate = true;
        myHttpWebRequest.Method = "GET";
        myHttpWebRequest.UserAgent = "bankasaas";
        myHttpWebRequest.Headers.Add("access_token", apiKey.ToString());
        myHttpWebRequest.Accept = "application/json";

        try
        {
            var myWebResponse = myWebRequest.GetResponse();
            var responseStream = myWebResponse.GetResponseStream();

            StreamReader myStreamReader = new StreamReader(responseStream, Encoding.UTF8);
            string json = myStreamReader.ReadToEnd();
            sResposta = json;

        }
        catch (WebException ex)
        {
            var resp = new StreamReader(ex.Response.GetResponseStream()).ReadToEnd();
            sResposta = resp;
        }

        return sResposta;
    }

    public static string ConsultaTransferencia(string apiKey, string sid)
    {
        ServicePointManager.Expect100Continue = true;
        ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12
        | SecurityProtocolType.Tls11
        | SecurityProtocolType.Tls
        | SecurityProtocolType.Ssl3;

        string URL = "https://api.asaas.com/v3//transfers/" + sid.ToString();

        var myUri = new Uri(URL);
        string sResposta = "";

        var myWebRequest = WebRequest.Create(myUri);
        var myHttpWebRequest = (HttpWebRequest)myWebRequest;
        myHttpWebRequest.PreAuthenticate = true;
        myHttpWebRequest.Method = "GET";
        myHttpWebRequest.UserAgent = "bankasaas";
        myHttpWebRequest.Headers.Add("access_token", apiKey.ToString());
        myHttpWebRequest.Accept = "application/json";

        try
        {
            var myWebResponse = myWebRequest.GetResponse();
            var responseStream = myWebResponse.GetResponseStream();

            StreamReader myStreamReader = new StreamReader(responseStream, Encoding.UTF8);
            string json = myStreamReader.ReadToEnd();
            sResposta = json;

        }
        catch (WebException ex)
        {
            var resp = new StreamReader(ex.Response.GetResponseStream()).ReadToEnd();
            sResposta = resp;
        }

        return sResposta;
    }
    

    public static string CancelarTransferencia(string apiKey, string sid)
    {
        ServicePointManager.Expect100Continue = true;
        ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12
        | SecurityProtocolType.Tls11
        | SecurityProtocolType.Tls
        | SecurityProtocolType.Ssl3;

        string URL = "https://api.asaas.com/v3//transfers/"+sid.ToString()+"/cancel";

        var myUri = new Uri(URL);
        string sResposta = "";

        var myWebRequest = WebRequest.Create(myUri);
        var myHttpWebRequest = (HttpWebRequest)myWebRequest;
        myHttpWebRequest.PreAuthenticate = true;
        myHttpWebRequest.Method = "DELETE";
        myHttpWebRequest.UserAgent = "bankasaas";
        myHttpWebRequest.Headers.Add("access_token", apiKey.ToString());
        myHttpWebRequest.Accept = "application/json";

        try
        {
            var myWebResponse = myWebRequest.GetResponse();
            var responseStream = myWebResponse.GetResponseStream();

            StreamReader myStreamReader = new StreamReader(responseStream, Encoding.UTF8);
            string json = myStreamReader.ReadToEnd();
            sResposta = json;

        }
        catch (WebException ex)
        {
            var resp = new StreamReader(ex.Response.GetResponseStream()).ReadToEnd();
            sResposta = resp;
        }

        return sResposta;
    }
    


    public static string ExtratoSubconta(string apiKey, string sFiltro)
    {
        ServicePointManager.Expect100Continue = true;
        ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12
        | SecurityProtocolType.Tls11
        | SecurityProtocolType.Tls
        | SecurityProtocolType.Ssl3;

        string URL = "https://api.asaas.com/v3/financialTransactions";

        var myUri = new Uri(URL + sFiltro.ToString());
        string sResposta = "";

        var myWebRequest = WebRequest.Create(myUri);
        var myHttpWebRequest = (HttpWebRequest)myWebRequest;
        myHttpWebRequest.PreAuthenticate = true;
        myHttpWebRequest.Method = "GET";
        myHttpWebRequest.UserAgent = "bankasaas";
        myHttpWebRequest.Headers.Add("access_token", apiKey.ToString());
        myHttpWebRequest.Accept = "application/json";

        try
        {
            var myWebResponse = myWebRequest.GetResponse();
            var responseStream = myWebResponse.GetResponseStream();

            StreamReader myStreamReader = new StreamReader(responseStream, Encoding.UTF8);
            string json = myStreamReader.ReadToEnd();
            sResposta = json;

        }
        catch (WebException ex)
        {
            var resp = new StreamReader(ex.Response.GetResponseStream()).ReadToEnd();
            sResposta = resp;
        }

        return sResposta;
    }
    
    public static string SaldoSubconta(string apiKey)
    {
        ServicePointManager.Expect100Continue = true;
        ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12
        | SecurityProtocolType.Tls11
        | SecurityProtocolType.Tls
        | SecurityProtocolType.Ssl3;

        string URL = "https://api.asaas.com/v3/finance/balance";

        var myUri = new Uri(URL);
        string sResposta = "";

        var myWebRequest = WebRequest.Create(myUri);
        var myHttpWebRequest = (HttpWebRequest)myWebRequest;
        myHttpWebRequest.PreAuthenticate = true;
        myHttpWebRequest.Method = "GET";
        myHttpWebRequest.UserAgent = "bankasaas";
        myHttpWebRequest.Headers.Add("access_token", apiKey.ToString());
        myHttpWebRequest.Accept = "application/json";

        try
        {
            var myWebResponse = myWebRequest.GetResponse();
            var responseStream = myWebResponse.GetResponseStream();

            StreamReader myStreamReader = new StreamReader(responseStream, Encoding.UTF8);
            string json = myStreamReader.ReadToEnd();
            sResposta = json;

        }
        catch (WebException ex)
        {
            var resp = new StreamReader(ex.Response.GetResponseStream()).ReadToEnd();
            sResposta = resp;
        }

        return sResposta;
    }

    // **********************************************************************************************************************************************************
    // SETOR CLIENTE
    // **********************************************************************************************************************************************************

    public static string CriarCliente(string apiKey, string json)
    {
        ServicePointManager.Expect100Continue = true;
        ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12
        | SecurityProtocolType.Tls11
        | SecurityProtocolType.Tls
        | SecurityProtocolType.Ssl3;

        string URL = asaas.UrlAPI().ToString() + "/customers";

        var myUri = new Uri(URL);
        string sResposta = "";

        var myWebRequest = WebRequest.Create(myUri);
        var myHttpWebRequest = (HttpWebRequest)myWebRequest;
        myHttpWebRequest.PreAuthenticate = true;
        myHttpWebRequest.Method = "POST";
        myHttpWebRequest.UserAgent = "bankasaas";
        myHttpWebRequest.Headers.Add("access_token", apiKey.ToString());
        myHttpWebRequest.Accept = "application/json";

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

        // Salvar na Base de dados de Processos e eventos
        // COD_ID_PESSOA_LICENCIADO
        // COD_ID_PESSOAS_FJ
        // COD_ID
        // NOM_OPERACAO "Subcontas"
        // NOM_METODO "POST" / "GET"
        // NOM_ORIGEM "ASAAS" / "ZOOP"

        return sResposta;
    }

    // **********************************************************************************************************************************************************
    // SETOR COBRANCA
    // **********************************************************************************************************************************************************

    public static string CriarCobranca(string apiKey, string json)
    {
        ServicePointManager.Expect100Continue = true;
        ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12
        | SecurityProtocolType.Tls11
        | SecurityProtocolType.Tls
        | SecurityProtocolType.Ssl3;

        string URL = asaas.UrlAPI().ToString() + "/payments";

        var myUri = new Uri(URL);
        string sResposta = "";

        var myWebRequest = WebRequest.Create(myUri);
        var myHttpWebRequest = (HttpWebRequest)myWebRequest;
        myHttpWebRequest.PreAuthenticate = true;
        myHttpWebRequest.Method = "POST";
        myHttpWebRequest.UserAgent = "bankasaas";
        myHttpWebRequest.Headers.Add("access_token", apiKey.ToString());
        myHttpWebRequest.Accept = "application/json";

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

        // Salvar na Base de dados de Processos e eventos
        // COD_ID_PESSOA_LICENCIADO
        // COD_ID_PESSOAS_FJ
        // COD_ID
        // NOM_OPERACAO "Subcontas"
        // NOM_METODO "POST" / "GET"
        // NOM_ORIGEM "ASAAS" / "ZOOP"

        return sResposta;
    }

    public static string CriarAssinatura(string apiKey, string json, string sOperacao)
    {
        ServicePointManager.Expect100Continue = true;
        ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12
        | SecurityProtocolType.Tls11
        | SecurityProtocolType.Tls
        | SecurityProtocolType.Ssl3;

        string URL = "";
        if (sOperacao == "H")
        {
            URL = "https://sandbox.asaas.com/api/v3/subscriptions";
        }
        else
        {
            URL = asaas.UrlAPI().ToString() + "/subscriptions";
        }
        
        var myUri = new Uri(URL);
        string sResposta = "";

        var myWebRequest = WebRequest.Create(myUri);
        var myHttpWebRequest = (HttpWebRequest)myWebRequest;
        myHttpWebRequest.PreAuthenticate = true;
        myHttpWebRequest.Method = "POST";
        myHttpWebRequest.UserAgent = "bankasaas";
        myHttpWebRequest.Headers.Add("access_token", apiKey.ToString());
        myHttpWebRequest.Accept = "application/json";

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

    public static string ObterQRCodeCobranca(string apiKey, string sIDCobranca)
    {
        ServicePointManager.Expect100Continue = true;
        ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12
        | SecurityProtocolType.Tls11
        | SecurityProtocolType.Tls
        | SecurityProtocolType.Ssl3;
        string URL = "https://api.asaas.com/v3/payments/" + sIDCobranca.ToString()  + "/pixQrCode";
        var myUri = new Uri(URL);
        string sResposta = "";
        var myWebRequest = WebRequest.Create(myUri);
        var myHttpWebRequest = (HttpWebRequest)myWebRequest;
        myHttpWebRequest.PreAuthenticate = true;
        myHttpWebRequest.Method = "GET";
        myHttpWebRequest.UserAgent = "bankasaas";
        myHttpWebRequest.Headers.Add("access_token", apiKey.ToString());
        myHttpWebRequest.Accept = "application/json";

        try
        {
            var myWebResponse = myWebRequest.GetResponse();
            var responseStream = myWebResponse.GetResponseStream();
            StreamReader myStreamReader = new StreamReader(responseStream, Encoding.UTF8);
            string json = myStreamReader.ReadToEnd();
            sResposta = json;
        }
        catch (WebException ex)
        {
            var resp = new StreamReader(ex.Response.GetResponseStream()).ReadToEnd();
            sResposta = resp;
        }
        return sResposta;
    }

    public static string ExcluirCobranca(string apiKey, string sIDCobranca)
    {
        ServicePointManager.Expect100Continue = true;
        ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12
        | SecurityProtocolType.Tls11
        | SecurityProtocolType.Tls
        | SecurityProtocolType.Ssl3;

        string URL = "https://api.asaas.com/v3/payments/" + sIDCobranca.ToString();

        var myUri = new Uri(URL);
        string sResposta = "";
        var myWebRequest = WebRequest.Create(myUri);
        var myHttpWebRequest = (HttpWebRequest)myWebRequest;
        myHttpWebRequest.PreAuthenticate = true;
        myHttpWebRequest.Method = "DELETE";
        myHttpWebRequest.UserAgent = "bankasaas";
        myHttpWebRequest.Headers.Add("access_token", (apiKey.ToString().Trim() == "") ? asaas.TokenAsaas().ToString() : apiKey.ToString());
        myHttpWebRequest.Accept = "application/json";

        try
        {
            var myWebResponse = myWebRequest.GetResponse();
            var responseStream = myWebResponse.GetResponseStream();
            StreamReader myStreamReader = new StreamReader(responseStream, Encoding.UTF8);

            string json = myStreamReader.ReadToEnd();
            sResposta = json;
        }
        catch (WebException ex)
        {
            var resp = new StreamReader(ex.Response.GetResponseStream()).ReadToEnd();
            sResposta = resp;
        }
        return sResposta;
    }


    public static string ObterLinhaDigitavelCobranca(string apiKey, string sIDCobranca)
    {
        ServicePointManager.Expect100Continue = true;
        ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12
        | SecurityProtocolType.Tls11
        | SecurityProtocolType.Tls
        | SecurityProtocolType.Ssl3;

        string URL = "https://api.asaas.com/v3/payments/" + sIDCobranca.ToString() + "/identificationField";

        var myUri = new Uri(URL);
        string sResposta = "";
        var myWebRequest = WebRequest.Create(myUri);
        var myHttpWebRequest = (HttpWebRequest)myWebRequest;
        myHttpWebRequest.PreAuthenticate = true;
        myHttpWebRequest.Method = "GET";
        myHttpWebRequest.UserAgent = "bankasaas";
        myHttpWebRequest.Headers.Add("access_token", apiKey.ToString());
        myHttpWebRequest.Accept = "application/json";

        try
        {
            var myWebResponse = myWebRequest.GetResponse();
            var responseStream = myWebResponse.GetResponseStream();
            StreamReader myStreamReader = new StreamReader(responseStream, Encoding.UTF8);
            string json = myStreamReader.ReadToEnd();
            sResposta = json;
        }
        catch (WebException ex)
        {
            var resp = new StreamReader(ex.Response.GetResponseStream()).ReadToEnd();
            sResposta = resp;
        }
        return sResposta;
    }

    // **********************************************************************************************************************************************************
    // SETOR PIX
    // **********************************************************************************************************************************************************

    public static string DecodificarQRCode(string apiKey, string json)
    {
        ServicePointManager.Expect100Continue = true;
        ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12
        | SecurityProtocolType.Tls11
        | SecurityProtocolType.Tls
        | SecurityProtocolType.Ssl3;

        string URL = "https://api.asaas.com/v3/pix/qrCodes/decode";

        var myUri = new Uri(URL);

        string sResposta = "";

        var myWebRequest = WebRequest.Create(myUri);
        var myHttpWebRequest = (HttpWebRequest)myWebRequest;
        myHttpWebRequest.ContentType = "application/json";
        myHttpWebRequest.Accept = "application/json";
        myHttpWebRequest.Method = "POST";
        myHttpWebRequest.PreAuthenticate = true;
        myHttpWebRequest.UserAgent = "bankasaas";
        myHttpWebRequest.Headers.Add("access_token", (apiKey.ToString().Trim() == "") ? asaas.TokenAsaas().ToString() : apiKey.ToString());

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

    public static string PagarQRCode(string apiKey, string json)
    {
        ServicePointManager.Expect100Continue = true;
        ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12
        | SecurityProtocolType.Tls11
        | SecurityProtocolType.Tls
        | SecurityProtocolType.Ssl3;

        string URL = "https://api.asaas.com/v3/pix/qrCodes/pay";

        var myUri = new Uri(URL);

        string sResposta = "";

        var myWebRequest = WebRequest.Create(myUri);
        var myHttpWebRequest = (HttpWebRequest)myWebRequest;
        myHttpWebRequest.ContentType = "application/json";
        myHttpWebRequest.Accept = "application/json";
        myHttpWebRequest.Method = "POST";
        myHttpWebRequest.PreAuthenticate = true;
        myHttpWebRequest.UserAgent = "bankasaas";
        myHttpWebRequest.Headers.Add("access_token", (apiKey.ToString().Trim() == "") ? asaas.TokenAsaas().ToString() : apiKey.ToString());

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

    // **********************************************************************************************************************************************************
    // SETOR TRANSFERENCIAS
    // **********************************************************************************************************************************************************

    public static string TransferenciaPIXAsaaS(string apiKey, string json)
    {
        ServicePointManager.Expect100Continue = true;
        ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12
        | SecurityProtocolType.Tls11
        | SecurityProtocolType.Tls
        | SecurityProtocolType.Ssl3;

        string URL = "https://api.asaas.com/v3/transfers";

        var myUri = new Uri(URL);

        string sResposta = "";

        var myWebRequest = WebRequest.Create(myUri);
        var myHttpWebRequest = (HttpWebRequest)myWebRequest;
        myHttpWebRequest.ContentType = "application/json";
        myHttpWebRequest.Accept = "application/json";
        myHttpWebRequest.Method = "POST";
        myHttpWebRequest.PreAuthenticate = true;
        myHttpWebRequest.UserAgent = "bankasaas";
        myHttpWebRequest.Headers.Add("access_token", (apiKey.ToString().Trim() == "") ? asaas.TokenAsaas().ToString() : apiKey.ToString());

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

    // -----------------------------------------------------------------------------------------------------------------------------------------------------------------
    // ROTINAS E OPERACOES COM CLIENTES
    // -----------------------------------------------------------------------------------------------------------------------------------------------------------------

    public static string VerificaCliente(string apiKey, string json, string nome, string cnpjcpf, string email, string cep, string numero, string celular)
    {
        string sIDCliente = "";
        // Verifica a existência do cliente
        string jsonRetornoCliente = asaas.ListarCliente(asaas.PegarTokenSubconta(Funcoes.strToInt(HttpContext.Current.Session["PESSOA"].ToString()), Funcoes.strToInt(HttpContext.Current.Session["LICENCIADO"].ToString())), Funcoes.TIRAACENTOSDOCUMENTOS(cnpjcpf.ToString()));

        if (jsonRetornoCliente.ToString().Trim() != "")
        {
            JObject oBoleto = JObject.Parse(jsonRetornoCliente.ToString());

            if (Funcoes.strToInt(oBoleto["totalCount"].ToString()) > 0)
            {
                sIDCliente = oBoleto["data"][0]["id"].ToString();
            }
            if (sIDCliente.ToString().Trim() == "")
            {
                // Criar cliente no ASAAS
                DadosClienteAsaaS.Root dcliente = new DadosClienteAsaaS.Root()
                {
                    name = nome.ToString(),
                    cpfCnpj = Funcoes.TIRAACENTOSDOCUMENTOS(cnpjcpf.ToString()),
                    email = email.ToString(),
                    addressNumber = numero,
                    mobilePhone = celular,
                    phone = celular,
                    postalCode = cep,
                    notificationDisabled = true
                };
                string jsonCliente = JsonConvert.SerializeObject(dcliente);
                string jsonIDCliente = asaas.CriarCliente(asaas.PegarTokenSubconta(Funcoes.strToInt(HttpContext.Current.Session["PESSOA"].ToString()), Funcoes.strToInt(HttpContext.Current.Session["LICENCIADO"].ToString())), jsonCliente);

                JObject o = JObject.Parse(jsonIDCliente.ToString());
                sIDCliente = o["id"].ToString();
            }
        }

        return sIDCliente;
    }

    // -----------------------------------------------------------------------------------------------------------------------------------------------------------------
    // CONSULTAS CONTA DIGITAL
    // -----------------------------------------------------------------------------------------------------------------------------------------------------------------
    public class ChavePix
    {
        public string type { get; set; }
    }

    public class ContaDigital
    {
        public string IDConta { get; set; }
        public string Agencia { get; set; }
        public string Conta { get; set; }
        public string DigitoConta { get; set; }
        public string ChavePix { get; set; }
        public string WalletID { get; set; }
    }

    public static ContaDigital ConsultaContaDigital()
    {
        SqlConnection mySelCadastro = new SqlConnection(Funcoes.conexao());
        mySelCadastro.Open();
        SqlCommand cmdSelCadastro = new SqlCommand("dbo.stp_pessoas_fj_baas_ins", mySelCadastro);
        cmdSelCadastro.CommandType = CommandType.StoredProcedure;
        cmdSelCadastro.Parameters.Add("@flg_operacao", SqlDbType.Char).Value = "S";
        cmdSelCadastro.Parameters.Add("@COD_ID_PESSOAS_FJ", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["PESSOA"].ToString());
        cmdSelCadastro.Parameters.Add("@COD_ID_PESSOA_LICENCIADO", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["LICENCIADO"].ToString());
        cmdSelCadastro.Parameters.Add("@FLG_ORIGEM", SqlDbType.Char).Value = "A";

        asaas.ContaDigital dContaDigital = new asaas.ContaDigital();

        SqlDataReader ReaderCadastro = cmdSelCadastro.ExecuteReader();
        while (ReaderCadastro.Read())
        {
            dContaDigital.IDConta = ReaderCadastro["NUM_ID_CONTA_BAAS"].ToString();
            dContaDigital.Agencia = ReaderCadastro["NUM_AGENCIA_BAAS"].ToString();
            dContaDigital.Conta = ReaderCadastro["NUM_CONTA_BAAS"].ToString();
            dContaDigital.DigitoConta = ReaderCadastro["NUM_DIGITO_CONTA_BAAS"].ToString();
            dContaDigital.ChavePix = ReaderCadastro["NUM_CHAVE_PIX"].ToString();
            dContaDigital.WalletID = ReaderCadastro["NUM_WALLETID_BAAS"].ToString();
        }        
        
        return dContaDigital;
    }


    // -----------------------------------------------------------------------------------------------------------------------------------------------------------------
    // CARTÃO ASAAS
    // -----------------------------------------------------------------------------------------------------------------------------------------------------------------
    public static string ConsultarCartaoAsaas(string apiKey, string id)
    {
        ServicePointManager.Expect100Continue = true;
        ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12
        | SecurityProtocolType.Tls11
        | SecurityProtocolType.Tls
        | SecurityProtocolType.Ssl3;

        string URL = "https://api.asaas.com/v3/asaasCards/?id="+ id.ToString();

        var myUri = new Uri(URL);
        string sResposta = "";
        var myWebRequest = WebRequest.Create(myUri);
        var myHttpWebRequest = (HttpWebRequest)myWebRequest;
        myHttpWebRequest.PreAuthenticate = true;
        myHttpWebRequest.Method = "GET";
        myHttpWebRequest.UserAgent = "bankasaas";
        myHttpWebRequest.Headers.Add("access_token", (apiKey.ToString().Trim() == "") ? asaas.TokenAsaas().ToString() : apiKey.ToString());
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
    }

    public static string ListarCartoesAsaas(string apiKey)
    {
        ServicePointManager.Expect100Continue = true;
        ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12
        | SecurityProtocolType.Tls11
        | SecurityProtocolType.Tls
        | SecurityProtocolType.Ssl3;

        string URL = "https://api.asaas.com/v3/asaasCards";

        var myUri = new Uri(URL);
        string sResposta = "";
        var myWebRequest = WebRequest.Create(myUri);
        var myHttpWebRequest = (HttpWebRequest)myWebRequest;
        myHttpWebRequest.PreAuthenticate = true;
        myHttpWebRequest.Method = "GET";
        myHttpWebRequest.UserAgent = "bankasaas";
        myHttpWebRequest.Headers.Add("access_token", (apiKey.ToString().Trim() == "") ? asaas.TokenAsaas().ToString() : apiKey.ToString());
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
    }

    public static string SolicitaCartoesAsaas(string apiKey, string json)
    {
        ServicePointManager.Expect100Continue = true;
        ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12
        | SecurityProtocolType.Tls11
        | SecurityProtocolType.Tls
        | SecurityProtocolType.Ssl3;

        string URL = "https://api.asaas.com/v3/asaasCards";

        var myUri = new Uri(URL);
        string sResposta = "";
        var myWebRequest = WebRequest.Create(myUri);
        var myHttpWebRequest = (HttpWebRequest)myWebRequest;
        myHttpWebRequest.PreAuthenticate = true;
        myHttpWebRequest.Method = "POST";
        myHttpWebRequest.UserAgent = "bankasaas";
        myHttpWebRequest.Headers.Add("access_token", (apiKey.ToString().Trim() == "") ? asaas.TokenAsaas().ToString() : apiKey.ToString());
        myHttpWebRequest.Accept = "application/json";

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

    public static string AtivarCartoesAsaas(string apiKey, string id, string json)
    {
        ServicePointManager.Expect100Continue = true;
        ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12
        | SecurityProtocolType.Tls11
        | SecurityProtocolType.Tls
        | SecurityProtocolType.Ssl3;

        string URL = "https://api.asaas.com/v3/asaasCards/" + id.ToString() + "/activate";

        var myUri = new Uri(URL);
        string sResposta = "";
        var myWebRequest = WebRequest.Create(myUri);
        var myHttpWebRequest = (HttpWebRequest)myWebRequest;
        myHttpWebRequest.PreAuthenticate = true;
        myHttpWebRequest.Method = "POST";
        myHttpWebRequest.UserAgent = "bankasaas";
        myHttpWebRequest.Headers.Add("access_token", (apiKey.ToString().Trim() == "") ? asaas.TokenAsaas().ToString() : apiKey.ToString());
        myHttpWebRequest.Accept = "application/json";

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

    public static string BloquearCartoesAsaas(string apiKey, string id)
    {
        ServicePointManager.Expect100Continue = true;
        ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12
        | SecurityProtocolType.Tls11
        | SecurityProtocolType.Tls
        | SecurityProtocolType.Ssl3;

        string URL = "https://api.asaas.com/v3/asaasCards/" + id.ToString() + "/block";

        var myUri = new Uri(URL);
        string sResposta = "";
        var myWebRequest = WebRequest.Create(myUri);
        var myHttpWebRequest = (HttpWebRequest)myWebRequest;
        myHttpWebRequest.PreAuthenticate = true;
        myHttpWebRequest.Method = "POST";
        myHttpWebRequest.UserAgent = "bankasaas";
        myHttpWebRequest.Headers.Add("access_token", (apiKey.ToString().Trim() == "") ? asaas.TokenAsaas().ToString() : apiKey.ToString());
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
    }

    public static string DesbloquearCartoesAsaas(string apiKey, string id)
    {
        ServicePointManager.Expect100Continue = true;
        ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12
        | SecurityProtocolType.Tls11
        | SecurityProtocolType.Tls
        | SecurityProtocolType.Ssl3;

        string URL = "https://api.asaas.com/v3/asaasCards/" + id.ToString() + "/unblock";

        var myUri = new Uri(URL);
        string sResposta = "";
        var myWebRequest = WebRequest.Create(myUri);
        var myHttpWebRequest = (HttpWebRequest)myWebRequest;
        myHttpWebRequest.PreAuthenticate = true;
        myHttpWebRequest.Method = "POST";
        myHttpWebRequest.UserAgent = "bankasaas";
        myHttpWebRequest.Headers.Add("access_token", (apiKey.ToString().Trim() == "") ? asaas.TokenAsaas().ToString() : apiKey.ToString());
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
    }


    public static string DeletarCartoesAsaas(string apiKey, string id)
    {
        ServicePointManager.Expect100Continue = true;
        ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12
        | SecurityProtocolType.Tls11
        | SecurityProtocolType.Tls
        | SecurityProtocolType.Ssl3;

        string URL = "https://api.asaas.com/v3/asaasCards/" + id.ToString();

        var myUri = new Uri(URL);
        string sResposta = "";
        var myWebRequest = WebRequest.Create(myUri);
        var myHttpWebRequest = (HttpWebRequest)myWebRequest;
        myHttpWebRequest.PreAuthenticate = true;
        myHttpWebRequest.Method = "DELETE";
        myHttpWebRequest.UserAgent = "bankasaas";
        myHttpWebRequest.Headers.Add("access_token", (apiKey.ToString().Trim() == "") ? asaas.TokenAsaas().ToString() : apiKey.ToString());
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
    }

    public static string DeletarConta(string apiKey, string sMotivo)
    {
        ServicePointManager.Expect100Continue = true;
        ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12
        | SecurityProtocolType.Tls11
        | SecurityProtocolType.Tls
        | SecurityProtocolType.Ssl3;

        string URL = "https://api.asaas.com/v3/myAccount?removeReason="+sMotivo.ToString();

        var myUri = new Uri(URL);
        string sResposta = "";
        var myWebRequest = WebRequest.Create(myUri);
        var myHttpWebRequest = (HttpWebRequest)myWebRequest;
        myHttpWebRequest.PreAuthenticate = true;
        myHttpWebRequest.Method = "DELETE";
        myHttpWebRequest.UserAgent = "bankasaas";
        myHttpWebRequest.Headers.Add("access_token", (apiKey.ToString().Trim() == "") ? asaas.TokenAsaas().ToString() : apiKey.ToString());
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
    }

    public static string AntecipacaoAutomatica(string apiKey, string json)
    {
        ServicePointManager.Expect100Continue = true;
        ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12
        | SecurityProtocolType.Tls11
        | SecurityProtocolType.Tls
        | SecurityProtocolType.Ssl3;

        string URL = "https://api.asaas.com/v3/anticipations/configurations";

        var myUri = new Uri(URL);
        string sResposta = "";
        var myWebRequest = WebRequest.Create(myUri);
        var myHttpWebRequest = (HttpWebRequest)myWebRequest;
        myHttpWebRequest.PreAuthenticate = true;
        myHttpWebRequest.Method = "PUT";
        myHttpWebRequest.UserAgent = "bankasaas";
        myHttpWebRequest.Headers.Add("access_token", (apiKey.ToString().Trim() == "") ? asaas.TokenAsaas().ToString() : apiKey.ToString());
        myHttpWebRequest.Accept = "application/json";

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




    public static string TestedeCode(string apiKey)
    {
        ServicePointManager.Expect100Continue = true;
        ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12
        | SecurityProtocolType.Tls11
        | SecurityProtocolType.Tls
        | SecurityProtocolType.Ssl3;

        string URL = "https://api.asaas.com/v3/myAccount/status";

        var myUri = new Uri(URL);

        var myWebRequest = WebRequest.Create(myUri);
        var myHttpWebRequest = (HttpWebRequest)myWebRequest;
        myHttpWebRequest.PreAuthenticate = true;
        myHttpWebRequest.Method = "GET";
        myHttpWebRequest.UserAgent = "bankasaas";
        myHttpWebRequest.Headers.Add("access_token", apiKey.ToString());
        myHttpWebRequest.Accept = "application/json";

        try
        {
            var myWebResponse = (HttpWebResponse)myWebRequest.GetResponse();
            int statusCode = (int)myWebResponse.StatusCode; // Pega o código HTTP
            var responseStream = myWebResponse.GetResponseStream();

            using (StreamReader myStreamReader = new StreamReader(responseStream, Encoding.UTF8))
            {
                string json = myStreamReader.ReadToEnd();
                return "{\"StatusCode\":\"" + statusCode.ToString() + "\", \"json\":" + json + "}";
            }
        }
        catch (WebException ex)
        {
            //    if (ex.Response is HttpWebResponse errorResponse)
            //    {
            //        int statusCode = (int)errorResponse.StatusCode; // Pega o código HTTP de erro
            //        using (StreamReader reader = new StreamReader(errorResponse.GetResponseStream()))
            //        {
            //            string errorJson = reader.ReadToEnd();
            //            return errorJson; // Retorna código e resposta de erro
            //        }
            //    }
            //string resp = new StreamReader(ex.Response.GetResponseStream()).ReadToEnd();
            //return resp;

            var errorResponse = ex.Response as HttpWebResponse;
            if (errorResponse != null) // Verifica se o cast foi bem-sucedido
            {
                int statusCode = (int)errorResponse.StatusCode;
                using (StreamReader reader = new StreamReader(errorResponse.GetResponseStream()))
                {
                    string errorJson = reader.ReadToEnd();
                    return "{''StatusCode'':''"+statusCode.ToString()+"'', ''json'':"+errorJson+"";
                }
            }
            return "";
        }
        return "";

    }


}