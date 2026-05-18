using System;
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
using MessagingToolkit.QRCode.Codec;
using MessagingToolkit.QRCode.Codec.Data;

/// <summary>
/// Summary description for CepService
/// </summary>
public class CepService
{
    private const string BaseUrl = "https://viacep.com.br/ws/";
    
    public static CepInfo ObterDadosPorCep(string cep)
    {
        ServicePointManager.Expect100Continue = true;
        ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls | SecurityProtocolType.Ssl3;

        var myUri = new Uri(BaseUrl + cep.ToString() + "/json");
        var myWebRequest = WebRequest.Create(myUri);
        var myHttpWebRequest = (HttpWebRequest)myWebRequest;
        myHttpWebRequest.Accept = "application/json";

        var myWebResponse = myWebRequest.GetResponse();
        var responseStream = myWebResponse.GetResponseStream();

        StreamReader myStreamReader = new StreamReader(responseStream, Encoding.UTF8);
        var json = myStreamReader.ReadToEnd();

        return JsonSerializer.Deserialize<CepInfo>(json);
    }

    public class CepInfo
    {
        public string Cep { get; set; }
        public string Logradouro { get; set; }
        public string Bairro { get; set; }
        public string Localidade { get; set; }
        public string Uf { get; set; }
    }


}