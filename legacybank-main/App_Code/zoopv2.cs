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
/// Summary description for zoopv2
/// </summary>
public class zoopv2
{
    // ---------------------------------------------------------------------------------------------------------------------------------------------------
    // COLLECTION VENDA DIGITADA
    // ---------------------------------------------------------------------------------------------------------------------------------------------------

    public class dadosVendaDigitada
    {
        public class Root
        {
            public string on_behalf_of { get; set; }
            public string description { get; set; }
            public string payment_type { get; set; }
            public bool capture { get; set; }
            public string reference_id { get; set; }
            public source source { get; set; }
            public installment_plan installment_plan { get; set; }
            public three_d_secure three_d_secure { get; set; }
            public split_rules[] split_rules { get; set; }
        }


        public class source
        {
            public string usage { get; set; }
            public int amount { get; set; }
            public string currency { get; set; }
            public string type { get; set; }
            public card card { get; set; }
        }

        public class card
        {
            public string card_number { get; set; }
            public string holder_name { get; set; }
            public string expiration_month { get; set; }
            public string expiration_year { get; set; }
            public string security_code { get; set; }
        }

        public class installment_plan
        {
            public int number_installments { get; set; }
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
            [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
            public bool? charge_recipient_processing_fee { get; set; }
            [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
            public bool? is_gross_amount { get; set; }
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

    }

    // ---------------------------------------------------------------------------------------------------------------------------------------------------
    // PADROES DE ACESSO TOKEN E MARKETPLACE
    // ---------------------------------------------------------------------------------------------------------------------------------------------------

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

    public static string VersaoZoop(int versao)
    {
        string sVersaoZoop = "";
        if (versao == 1) { sVersaoZoop = "https://api.zoop.ws/v1/marketplaces/" + zoopv2.marketplace().ToString(); }
        if (versao == 2) { sVersaoZoop = "https://api.zoop.ws/v2/marketplaces/" + zoopv2.marketplace().ToString(); }
        return sVersaoZoop;
    }

    public static string ConsultaVendedor()
    {
        string sRetorno = "";

        SqlConnection mySelCadastro = new SqlConnection(Funcoes.conexao());
        mySelCadastro.Open();
        SqlCommand cmdSelCadastro = new SqlCommand("dbo.stp_pessoas_fj_ins", mySelCadastro);
        cmdSelCadastro.CommandType = CommandType.StoredProcedure;
        cmdSelCadastro.Parameters.Add("@flg_operacao", SqlDbType.Char).Value = "S";
        cmdSelCadastro.Parameters.Add("@COD_ID", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["PESSOA"].ToString());
        cmdSelCadastro.Parameters.Add("@COD_ID_PESSOA_LICENCIADO", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["LICENCIADO"].ToString());
        SqlDataReader ReaderCadastro = cmdSelCadastro.ExecuteReader();
        while (ReaderCadastro.Read())
        {
            sRetorno = ReaderCadastro["NUM_TOKEN"].ToString();
        }

        return sRetorno;
    }

    // ---------------------------------------------------------------------------------------------------------------------------------------------------
    // TRANSAÇÕES DE VENDA - COBRANÇAS 
    // ---------------------------------------------------------------------------------------------------------------------------------------------------

    public static string CriarTransacao(string json)
    {

        ServicePointManager.Expect100Continue = true;
        ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12
        | SecurityProtocolType.Tls11
        | SecurityProtocolType.Tls
        | SecurityProtocolType.Ssl3;


        var byteArray = Encoding.ASCII.GetBytes(zoop.Keymarketplace() + ":" + "");
        string encodeString = Convert.ToBase64String(byteArray);

        var URL = zoopv2.VersaoZoop(1).ToString() + "/transactions";

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


}