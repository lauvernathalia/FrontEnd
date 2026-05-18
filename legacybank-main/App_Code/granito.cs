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
/// Summary description for granito
/// </summary>
public class granito
{
    //public static string UrlAPI()
    //{
    //    return "https://gatewayint.granitopagamentos.com.br:1002";
    //}

    public static string UrlAPI()
    {
        return "https://interface.granitopagamentos.com.br/accreditation/";    }

    public static string iss()
    {
        return "00000DFA-0000-0000-EC81-CEB997DCDC08";
    }

    public static string secret()
    {
        return "2eCpbddT4FTTYrtgvxemOVaOaFYiVUV1Pk3WOnPBoyq0xt3nxe7h8zOIkiuaFVZX3bhWxRHKeX1tZVw0nEkN3K4z2GIj3G2Ic6V6slXZgK0ZIlC2yujqdqSSSXd+ZeYsr9xO2D9qKVlNA8gj/VzIRqw0+by2coSwBqU+WMCikmcNm/n4MO6jlrmp0cvUKJOfJY+wf6XWF5uSgbSEkjWzRUwD6GQLrWSHbqNGALonH/5XaAFGceU4HksJuIWho1R4oh8ppUF7DgiMCLSz2Jaa20J5dS+T225ehA9csBRu0PZyqllw9qkezPWJ+cYQwhRIfBQQCOcQTzm9EMGzIWDOwA==";
    }

    public static string TokenAutenticacao()
    {
        string sToken = "";
        // Geração do Header
        var header = new
        {
            alg = "HS256",
            typ = "JWT"
        };
        string headerJson = Newtonsoft.Json.JsonConvert.SerializeObject(header);
        string encodedHeader = Base64Encode(headerJson);        
        
        // Geração Payload

        var payload = new
        {
            jti = Guid.NewGuid().ToString(),
            iat = ToUnixTimeSeconds(DateTime.Now),
            exp = ToUnixTimeSeconds(DateTime.Now.AddHours(2)),
            iss = granito.iss()
        };

        string payloadJson = Newtonsoft.Json.JsonConvert.SerializeObject(payload);
        string encodedPayload = Base64Encode(payloadJson);

        // Geração Assinatura

        string secret = granito.secret(); // Your secret key
        string signature = CreateHMACSHA256Signature(encodedHeader, encodedPayload, secret);

        // Geração JWT

        string jwt =  encodedHeader + "." + encodedPayload + "." + signature;

        return jwt;
    }

    public static string Base64Encode(string input)
    {
        var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(input);
        return System.Convert.ToBase64String(plainTextBytes)
                .TrimEnd('=').Replace('+', '-').Replace('/', '_');
    }

    public static string CreateHMACSHA256Signature(string header, string payload, string secret)
    {
        var keyBytes = System.Text.Encoding.UTF8.GetBytes(secret);
        var message = header + "." + payload;
        using (var hmac = new System.Security.Cryptography.HMACSHA256(keyBytes))
        {
            var hash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(message));
            return Convert.ToBase64String(hash)
                          .TrimEnd('=').Replace('+', '-').Replace('/', '_');
        }
    }

    public static long ToUnixTimeSeconds(DateTime dateTime)
    {
        DateTime unixEpoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
        TimeSpan timeSinceEpoch = dateTime.ToUniversalTime() - unixEpoch;
        return (long)timeSinceEpoch.TotalSeconds;
    }

    public static string ProcessarTransacao()
    {

        ServicePointManager.Expect100Continue = true;
        ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12
        | SecurityProtocolType.Tls11
        | SecurityProtocolType.Tls
        | SecurityProtocolType.Ssl3;

        var URL = "";
        URL = hubcappta.UrlAPI() + "/Payments/Authorize/Payment/Process";

        string json = TokenAutenticacao();
        var myUri = new Uri(URL);

        var myWebRequest = WebRequest.Create(myUri);
        var myHttpWebRequest = (HttpWebRequest)myWebRequest;
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
            string jsonRetorno = myStreamReader.ReadToEnd();

            return jsonRetorno;

        }
        catch (WebException ex)
        {
            var resp = new StreamReader(ex.Response.GetResponseStream()).ReadToEnd();
            return resp;
        }

    }

}