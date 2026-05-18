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
/// Summary description for adiq
/// </summary>
/// 

public class DadosTokenCartaoAdiq
{
    public class TokenCartao
    {
        public string cardNumber { get; set; }
    }
}

public class DadosCofreCartaoAdiq
{
    public class Root
    {
        public string numberToken { get; set; }
        public string brand { get; set; }
        public string cardholderName { get; set; }
        public string expirationMonth { get; set; }
        public string expirationYear { get; set; }
        public bool verifyCard { get; set; }
        public string securityCode { get; set; }
    }
}

public class DadosPagamentoCofreAdiq
{

    public class Root
    {
        public Payment payment { get; set; }
        public CardInfo cardInfo { get; set; }
        public LineItem[] LineItems { get; set; }
        public Customer Customer { get; set; }
        public ShipTo ShipTo { get; set; }
        public DeviceInfo DeviceInfo { get; set; }
        public SellerInfo sellerInfo { get; set; }
        public List<Seller> sellers { get; set; }
    }
    
    public class CardInfo
    {
        public string vaultId { get; set; }
    }

    public class Customer
    {
        public string DocumentType { get; set; }
        public string DocumentNumber { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string MobilePhoneNumber { get; set; }
        public string Address { get; set; }
        public string Complement { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string ZipCode { get; set; }
        public string IpAddress { get; set; }
        public string Country { get; set; }
    }

    public class DeviceInfo
    {
        public string HttpAcceptBrowserValue { get; set; }
        public string HttpAcceptContent { get; set; }
        public string HttpBrowserLanguage { get; set; }
        public string HttpBrowserJavaEnabled { get; set; }
        public string HttpBrowserJavaScriptEnabled { get; set; }
        public string HttpBrowserColorDepth { get; set; }
        public string HttpBrowserScreenHeight { get; set; }
        public string HttpBrowserScreenWidth { get; set; }
        public string HttpBrowserTimeDifference { get; set; }
        public string UserAgentBrowserValue { get; set; }
    }

    public class Item
    {
        public string id { get; set; }
        public string description { get; set; }
        public int amount { get; set; }
        public int ratePercent { get; set; }
        public int rateAmount { get; set; }
    }

    public class LineItem
    {
        public string UnitPrice { get; set; }
        public string Quantity { get; set; }
        public string ProductSKU { get; set; }
        public string ProductName { get; set; }
        public string ProductCode { get; set; }
    }

    public class Payment
    {
        public string transactionType { get; set; }
        public int amount { get; set; }
        public string currencyCode { get; set; }
        public string productType { get; set; }
        public int installments { get; set; }
        public string captureType { get; set; }
        public bool recurrent { get; set; }
    }


    public class Seller
    {
        public string id { get; set; }
        public int amount { get; set; }
        public List<Item> items { get; set; }
    }

    public class SellerInfo
    {
        public string orderNumber { get; set; }
        public string softDescriptor { get; set; }
        //public int dynamicMcc { get; set; }
        public string code3DS { get; set; }
        public string urlSite3DS { get; set; }
        public string codeAntiFraud { get; set; }
        public Antifraud Antifraud {get; set;}
    }
    public class Antifraud
    {
        public bool CheckDocBearer { get; set; }
    }

    public class ShipTo
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
        public string Address { get; set; }
        public string Complement { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string ZipCode { get; set; }
        public string Country { get; set; }
    }

}

public class DadosPagamentoTokenAdiq
{
    public class CardInfo
    {
        public string numberToken { get; set; }
        public string cardholderName { get; set; }
        public string securityCode { get; set; }
        public string brand { get; set; }
        public string expirationMonth { get; set; }
        public string expirationYear { get; set; }
    }

    public class Customer
    {
        public string DocumentType { get; set; }
        public string DocumentNumber { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string MobilePhoneNumber { get; set; }
        public string Address { get; set; }
        public string Complement { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string ZipCode { get; set; }
        public string IpAddress { get; set; }
        public string Country { get; set; }
    }

    public class DeviceInfo
    {
        public string HttpAcceptBrowserValue { get; set; }
        public string HttpAcceptContent { get; set; }
        public string HttpBrowserLanguage { get; set; }
        public string HttpBrowserJavaEnabled { get; set; }
        public string HttpBrowserJavaScriptEnabled { get; set; }
        public string HttpBrowserColorDepth { get; set; }
        public string HttpBrowserScreenHeight { get; set; }
        public string HttpBrowserScreenWidth { get; set; }
        public string HttpBrowserTimeDifference { get; set; }
        public string UserAgentBrowserValue { get; set; }
    }

    public class Item
    {
        public string id { get; set; }
        public string description { get; set; }
        public int amount { get; set; }
        public int ratePercent { get; set; }
        public int rateAmount { get; set; }
    }

    public class LineItems
    {
        public string UnitPrice { get; set; }
        public string Quantity { get; set; }
        public string ProductSKU { get; set; }
        public string ProductName { get; set; }
        public string ProductCode { get; set; }
    }

    public class Payment
    {
        public string transactionType { get; set; }
        public int amount { get; set; }
        public string currencyCode { get; set; }
        public string productType { get; set; }
        public int installments { get; set; }
        public string captureType { get; set; }
        public bool recurrent { get; set; }
        public string RecurrentNridElo { get; set; }
        public int RecurrentAmountElo { get; set; }
    }

    public class Root
    {
        public Payment payment { get; set; }
        public CardInfo cardInfo { get; set; }
        public LineItems[] LineItems { get; set; }
        public Customer Customer { get; set; }
        public ShipTo ShipTo { get; set; }
        public DeviceInfo DeviceInfo { get; set; }
        public SellerInfo sellerInfo { get; set; }
        public List<Seller> sellers { get; set; }
    }

    public class Seller
    {
        public string id { get; set; }
        public int amount { get; set; }
        public List<Item> items { get; set; }
    }

    public class SellerInfo
    {
        public string orderNumber { get; set; }
        public string softDescriptor { get; set; }
        //public int dynamicMcc { get; set; }
        public string code3DS { get; set; }
        public string urlSite3DS { get; set; }
        public string codeAntiFraud { get; set; }
        public Antifraud Antifraud { get; set; }
    }
    public class Antifraud
    {
        public bool CheckDocBearer { get; set; }
    }

    public class ShipTo
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
        public string Address { get; set; }
        public string Complement { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string ZipCode { get; set; }
        public string Country { get; set; }
    }

}

public class DadosCancelarPagamento
{
    public class Item
    {
        public string id { get; set; }
        public int amount { get; set; }
    }

    public class Root
    {
        public int amount { get; set; }
        public List<Seller> sellers { get; set; }
    }

    public class Seller
    {
        public string id { get; set; }
        public int amount { get; set; }
        public List<Item> items { get; set; }
    }
}

public class DadosCapturarPagamento
{
    public class Item
    {
        public string id { get; set; }
        public int amount { get; set; }
    }

    public class Root
    {
        public int amount { get; set; }
        public List<Seller> sellers { get; set; }
    }

    public class Seller
    {
        public string id { get; set; }
        public int amount { get; set; }
        public List<Item> items { get; set; }
    }
}

public class DadosPixAdiq
{
    public class Root
    {
        public string order_id { get; set; }
        public DateTime order_date { get; set; }
        public int amount { get; set; }
        public string description { get; set; }
        public string url_web_hook { get; set; }
    }
}


public class DadosBoletoAdiq
{
    public class Address
    {
        public string postalCode { get; set; }
        public string state { get; set; }
        public string city { get; set; }
        public string neighborhood { get; set; }
        public string streetAddress { get; set; }
        public string number { get; set; }
        public string complement { get; set; }
    }

    public class Client
    {
        public string phoneNumber { get; set; }
        public string email { get; set; }
        public Person person { get; set; }
    }

    public class Message
    {
        public string line1 { get; set; }
        public string line2 { get; set; }
        public string line3 { get; set; }
        public string line4 { get; set; }
    }

    public class Payee
    {
        public string document { get; set; }
        public string name { get; set; }
        public int typePerson { get; set; }
        public Address address { get; set; }
    }

    public class Person
    {
        public string document { get; set; }
        public string name { get; set; }
        public int typePerson { get; set; }
        public Address address { get; set; }
    }

    public class Root
    {
        public Client client { get; set; }
        public Payee payee { get; set; }
        public DateTime dueDate { get; set; }
        public double amount { get; set; }
        public Message message { get; set; }
    }

}

public class Autorizacao
{
    public string username { get; set; }
    public string password { get; set; }
    public int access_type_id { get; set; }
}

public class adiq
{
    public class Autorizacao
    {
        public string username { get; set; }
        public string password { get; set; }
        public int access_type_id { get; set; }
    }


    // Autenticação --------------------------------------------------------------------------------------//
    // Documentação: integration.cappta.com.br

    public static string UrlAPIAdiq()
    {
        return "https://ecommerce-hml.adiq.io"; // Homologação
        //return "https://ecommerce.adiq.io"; // Produção
    }

    public static string clientIdAdiq()
    {
        return "3fe67ac6-3c61-40aa-8536-de04e3a91168";
    }

    public static string clientSecretAdiq()
    {
        return "058CE27D-52EA-4E05-BCBE-50F2B5513152";
    }

    // /v1/tokens/cards - Tokenização

    public static string UsernameAdiq()
    {
        return "wander@euromercantil.com.br";
    }

    // /v1/tokens/cards - Tokenização
    public static string PasswordAdiq()
    {
        return "Ssg251103#";
    }

    // /v1/tokens/cards - Tokenização

    public static string GerarTokenAdiq()
    {
        ServicePointManager.Expect100Continue = true;
        ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12
        | SecurityProtocolType.Tls11
        | SecurityProtocolType.Tls
        | SecurityProtocolType.Ssl3;


        adiq.Autorizacao dautorizacao = new adiq.Autorizacao()
        {
            username = UsernameAdiq().ToString(),
            password = PasswordAdiq().ToString(),
            access_type_id = 2
        };

        string json = JsonConvert.SerializeObject(dautorizacao);

        var URL = "https://admin-europay.adiq.io/v1/interface/auth/oauth2";
        var myUri = new Uri(URL);

        var myWebRequest = WebRequest.Create(myUri);
        var myHttpWebRequest = (HttpWebRequest)myWebRequest;
        myHttpWebRequest.UserAgent = "adiqeuro";
        myHttpWebRequest.ContentType = "application/json";
        myHttpWebRequest.Accept = "application/json";
        myHttpWebRequest.Method = "POST";
        myHttpWebRequest.PreAuthenticate = true;

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

            var jsonResposta = myStreamReader.ReadToEnd();
            return jsonResposta;
        }
        catch (WebException ex)
        {
            var resp = new StreamReader(ex.Response.GetResponseStream()).ReadToEnd();
            return resp;
        }
    }




    public static string TokenAutenticacaoAdiq()
    {
        ServicePointManager.Expect100Continue = true;
        ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12
        | SecurityProtocolType.Tls11
        | SecurityProtocolType.Tls
        | SecurityProtocolType.Ssl3;

        var byteArray = Encoding.ASCII.GetBytes(adiq.clientIdAdiq().ToString() + ":" + adiq.clientSecretAdiq().ToString());
        string encodeString = Convert.ToBase64String(byteArray);

        string json = "";
        json = json + "{";
        json = json + "\"grantType\":\"client_credentials\"";
        json = json + "}";

        var URL = adiq.UrlAPIAdiq().ToString() + "/auth/oauth2/v1/token";
        var myUri = new Uri(URL);

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

            var jsonResposta = myStreamReader.ReadToEnd();
            JObject o = JObject.Parse(jsonResposta);

            return o["accessToken"].ToString();
        }
        catch (WebException ex)
        {
            var resp = new StreamReader(ex.Response.GetResponseStream()).ReadToEnd();
            return resp;
        }
    }

    public static string GerarTokenCartao(string json)
    {
        ServicePointManager.Expect100Continue = true;
        ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12
        | SecurityProtocolType.Tls11
        | SecurityProtocolType.Tls
        | SecurityProtocolType.Ssl3;

        var byteArray = Encoding.ASCII.GetBytes(adiq.clientIdAdiq().ToString() + ":" + adiq.clientSecretAdiq().ToString());
        string encodeString = Convert.ToBase64String(byteArray);

        var URL = adiq.UrlAPIAdiq().ToString() + "/v1/tokens/cards";
        var myUri = new Uri(URL);

        var myWebRequest = WebRequest.Create(myUri);
        var myHttpWebRequest = (HttpWebRequest)myWebRequest;
        myHttpWebRequest.ContentType = "application/json";
        myHttpWebRequest.Accept = "application/json";
        myHttpWebRequest.Method = "POST";
        myHttpWebRequest.PreAuthenticate = true;
        myHttpWebRequest.Headers.Add("Authorization", "Bearer " + adiq.TokenAutenticacaoAdiq().ToString() + "");

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

            var jsonResposta = myStreamReader.ReadToEnd();
            JObject o = JObject.Parse(jsonResposta);

            return o["numberToken"].ToString();
        }
        catch (WebException ex)
        {
            var resp = new StreamReader(ex.Response.GetResponseStream()).ReadToEnd();
            return resp;
        }
    }

    public static string CadastrarCofreCartao(string json)
    {
        ServicePointManager.Expect100Continue = true;
        ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12
        | SecurityProtocolType.Tls11
        | SecurityProtocolType.Tls
        | SecurityProtocolType.Ssl3;

        var byteArray = Encoding.ASCII.GetBytes(adiq.clientIdAdiq().ToString() + ":" + adiq.clientSecretAdiq().ToString());
        string encodeString = Convert.ToBase64String(byteArray);

        var URL = adiq.UrlAPIAdiq().ToString() + "/v1/vaults/cards";
        var myUri = new Uri(URL);

        var myWebRequest = WebRequest.Create(myUri);
        var myHttpWebRequest = (HttpWebRequest)myWebRequest;
        myHttpWebRequest.ContentType = "application/json";
        myHttpWebRequest.Accept = "application/json";
        myHttpWebRequest.Method = "POST";
        myHttpWebRequest.PreAuthenticate = true;
        myHttpWebRequest.Headers.Add("Authorization", "Bearer " + adiq.TokenAutenticacaoAdiq().ToString() + "");

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

            var jsonResposta = myStreamReader.ReadToEnd();
            JObject o = JObject.Parse(jsonResposta);

            return o["vaultId"].ToString();
        }
        catch (WebException ex)
        {
            var resp = new StreamReader(ex.Response.GetResponseStream()).ReadToEnd();
            return resp;
        }
    }

    public static string ConsultarBINCartao(string binNumber)
    {
        ServicePointManager.Expect100Continue = true;
        ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12
        | SecurityProtocolType.Tls11
        | SecurityProtocolType.Tls
        | SecurityProtocolType.Ssl3;

        var byteArray = Encoding.ASCII.GetBytes(adiq.clientIdAdiq().ToString() + ":" + adiq.clientSecretAdiq().ToString());
        string encodeString = Convert.ToBase64String(byteArray);

        var URL = adiq.UrlAPIAdiq().ToString() + "/v1/bins/search/" + binNumber.ToString();
        var myUri = new Uri(URL);

        var myWebRequest = WebRequest.Create(myUri);
        var myHttpWebRequest = (HttpWebRequest)myWebRequest;
        myHttpWebRequest.ContentType = "application/json";
        myHttpWebRequest.Accept = "application/json";
        myHttpWebRequest.Method = "GET";
        myHttpWebRequest.PreAuthenticate = true;
        myHttpWebRequest.Headers.Add("Authorization", "Bearer " + adiq.TokenAutenticacaoAdiq().ToString() + "");

        try
        {
            var myWebResponse = myWebRequest.GetResponse();
            var responseStream = myWebResponse.GetResponseStream();
            StreamReader myStreamReader = new StreamReader(responseStream, Encoding.UTF8);
            
            var jsonResposta = myStreamReader.ReadToEnd();
            //JObject o = JObject.Parse(jsonResposta);

            //return o["vaultId"].ToString();
            return jsonResposta.ToString();
        }
        catch (WebException ex)
        {
            var resp = new StreamReader(ex.Response.GetResponseStream()).ReadToEnd();
            return resp;
        }
    }

    public static string CancelarPagamentos(string paymentId, string json)
    {
        ServicePointManager.Expect100Continue = true;
        ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12
        | SecurityProtocolType.Tls11
        | SecurityProtocolType.Tls
        | SecurityProtocolType.Ssl3;

        var byteArray = Encoding.ASCII.GetBytes(adiq.clientIdAdiq().ToString() + ":" + adiq.clientSecretAdiq().ToString());
        string encodeString = Convert.ToBase64String(byteArray);

        var URL = adiq.UrlAPIAdiq().ToString() + "/v1/payments/" + paymentId.ToString() + "/cancel";
        var myUri = new Uri(URL);

        var myWebRequest = WebRequest.Create(myUri);
        var myHttpWebRequest = (HttpWebRequest)myWebRequest;
        myHttpWebRequest.ContentType = "application/json";
        myHttpWebRequest.Accept = "application/json";
        myHttpWebRequest.Method = "PUT";
        myHttpWebRequest.PreAuthenticate = true;
        myHttpWebRequest.Headers.Add("Authorization", "Bearer " + adiq.TokenAutenticacaoAdiq().ToString() + "");

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

            var jsonResposta = myStreamReader.ReadToEnd();
            //JObject o = JObject.Parse(jsonResposta);

            //return o["vaultId"].ToString();
            return jsonResposta.ToString();
        }
        catch (WebException ex)
        {
            var resp = new StreamReader(ex.Response.GetResponseStream()).ReadToEnd();
            return resp;
        }
    }


    public static string ConsultarPagamentos(string paymentId)
    {
        ServicePointManager.Expect100Continue = true;
        ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12
        | SecurityProtocolType.Tls11
        | SecurityProtocolType.Tls
        | SecurityProtocolType.Ssl3;

        var byteArray = Encoding.ASCII.GetBytes(adiq.clientIdAdiq().ToString() + ":" + adiq.clientSecretAdiq().ToString());
        string encodeString = Convert.ToBase64String(byteArray);

        var URL = adiq.UrlAPIAdiq().ToString() + "/v1/payments/" + paymentId.ToString();
        var myUri = new Uri(URL);

        var myWebRequest = WebRequest.Create(myUri);
        var myHttpWebRequest = (HttpWebRequest)myWebRequest;
        myHttpWebRequest.ContentType = "application/json";
        myHttpWebRequest.Accept = "application/json";
        myHttpWebRequest.Method = "GET";
        myHttpWebRequest.PreAuthenticate = true;
        myHttpWebRequest.Headers.Add("Authorization", "Bearer " + adiq.TokenAutenticacaoAdiq().ToString() + "");

        try
        {
            var myWebResponse = myWebRequest.GetResponse();
            var responseStream = myWebResponse.GetResponseStream();
            StreamReader myStreamReader = new StreamReader(responseStream, Encoding.UTF8);

            var jsonResposta = myStreamReader.ReadToEnd();
            //JObject o = JObject.Parse(jsonResposta);

            //return o["vaultId"].ToString();
            return jsonResposta.ToString();
        }
        catch (WebException ex)
        {
            var resp = new StreamReader(ex.Response.GetResponseStream()).ReadToEnd();
            return resp;
        }
    }


    public static string CapturarPagamentos(string paymentId, string json)
    {
        ServicePointManager.Expect100Continue = true;
        ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12
        | SecurityProtocolType.Tls11
        | SecurityProtocolType.Tls
        | SecurityProtocolType.Ssl3;

        var byteArray = Encoding.ASCII.GetBytes(adiq.clientIdAdiq().ToString() + ":" + adiq.clientSecretAdiq().ToString());
        string encodeString = Convert.ToBase64String(byteArray);

        var URL = adiq.UrlAPIAdiq().ToString() + "/v1/payments/" + paymentId.ToString() + "/capture";
        var myUri = new Uri(URL);

        var myWebRequest = WebRequest.Create(myUri);
        var myHttpWebRequest = (HttpWebRequest)myWebRequest;
        myHttpWebRequest.ContentType = "application/json";
        myHttpWebRequest.Accept = "application/json";
        myHttpWebRequest.Method = "PUT";
        myHttpWebRequest.PreAuthenticate = true;
        myHttpWebRequest.Headers.Add("Authorization", "Bearer " + adiq.TokenAutenticacaoAdiq().ToString() + "");

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

            var jsonResposta = myStreamReader.ReadToEnd();
            //JObject o = JObject.Parse(jsonResposta);

            //return o["vaultId"].ToString();
            return jsonResposta.ToString();
        }
        catch (WebException ex)
        {
            var resp = new StreamReader(ex.Response.GetResponseStream()).ReadToEnd();
            return resp;
        }
    }

    public static string CadastrarPagamentos(string json)
    {
        ServicePointManager.Expect100Continue = true;
        ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12
        | SecurityProtocolType.Tls11
        | SecurityProtocolType.Tls
        | SecurityProtocolType.Ssl3;

        var byteArray = Encoding.ASCII.GetBytes(adiq.clientIdAdiq().ToString() + ":" + adiq.clientSecretAdiq().ToString());
        string encodeString = Convert.ToBase64String(byteArray);

        var URL = adiq.UrlAPIAdiq().ToString() + "/v1/payments";
        var myUri = new Uri(URL);

        var myWebRequest = WebRequest.Create(myUri);
        var myHttpWebRequest = (HttpWebRequest)myWebRequest;
        myHttpWebRequest.ContentType = "application/json";
        myHttpWebRequest.Accept = "application/json";
        myHttpWebRequest.Method = "POST";
        myHttpWebRequest.PreAuthenticate = true;
        myHttpWebRequest.Headers.Add("Authorization", "Bearer " + adiq.TokenAutenticacaoAdiq().ToString() + "");

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

            var jsonResposta = myStreamReader.ReadToEnd();
            //JObject o = JObject.Parse(jsonResposta);

            //return o["vaultId"].ToString();
            return jsonResposta.ToString();
        }
        catch (WebException ex)
        {
            var resp = new StreamReader(ex.Response.GetResponseStream()).ReadToEnd();
            return resp;
        }
    }
}