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

using System.Threading;
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


public partial class events_asaaspayout : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

        ServicePointManager.Expect100Continue = true;
        ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12
        | SecurityProtocolType.Tls11
        | SecurityProtocolType.Tls
        | SecurityProtocolType.Ssl3;


        string sConexao = ConfigurationManager.AppSettings["user"].ToString() + ";" +
            ConfigurationManager.AppSettings["password"].ToString() + ";" +
            ConfigurationManager.AppSettings["datasource"].ToString() + ";" +
            ConfigurationManager.AppSettings["basecatalog"].ToString();

        var bodyStream = new StreamReader(HttpContext.Current.Request.InputStream);
        bodyStream.BaseStream.Seek(0, SeekOrigin.Begin);
        var bodyText = bodyStream.ReadToEnd();
        
        
        if (HttpContext.Current.Request.HttpMethod == "POST")
        {
            HttpContext.Current.Response.ContentType = "application/json";
            HttpContext.Current.Response.Write("{\"status\":\"APPROVED\"}");
            HttpContext.Current.Response.End();

            SqlConnection connInsConsASAASResp = new SqlConnection(sConexao);
            connInsConsASAASResp.Open();
            SqlCommand cmdInsConsASAASResp = new SqlCommand("dbo.stp_notificacoes_asaas_ins", connInsConsASAASResp);
            cmdInsConsASAASResp.CommandType = CommandType.StoredProcedure;
            cmdInsConsASAASResp.Parameters.Add("@flg_operacao", SqlDbType.Char).Value = 'I';

            cmdInsConsASAASResp.Parameters.Add("@DES_JSON", SqlDbType.Text).Value = bodyText.ToString();
            cmdInsConsASAASResp.Parameters.Add("@DTA_DATA", SqlDbType.DateTime).Value = DateTime.Now;
            cmdInsConsASAASResp.Parameters.Add("@NOM_CODE", SqlDbType.VarChar).Value = "TRANSFERENCIA RESPONSE";
            cmdInsConsASAASResp.Parameters.Add("@NOM_TIPO", SqlDbType.VarChar).Value = "CONFIRMAÇÃO RESPONSE";
            cmdInsConsASAASResp.Parameters.Add("@FLG_STATUS", SqlDbType.VarChar).Value = "R";
            cmdInsConsASAASResp.Parameters.Add("@NUM_TOKEN", SqlDbType.VarChar).Value = "R";
            cmdInsConsASAASResp.ExecuteNonQuery();
            connInsConsASAASResp.Close();
            connInsConsASAASResp.Dispose();


        }

        // Grava JSON na Base de dados
        SqlConnection connInsConsASAAS = new SqlConnection(sConexao);
        connInsConsASAAS.Open();
        SqlCommand cmdInsConsASAAS = new SqlCommand("dbo.stp_notificacoes_asaas_ins", connInsConsASAAS);
        cmdInsConsASAAS.CommandType = CommandType.StoredProcedure;
        cmdInsConsASAAS.Parameters.Add("@flg_operacao", SqlDbType.Char).Value = 'I';

        cmdInsConsASAAS.Parameters.Add("@DES_JSON", SqlDbType.Text).Value = bodyText.ToString();
        cmdInsConsASAAS.Parameters.Add("@DTA_DATA", SqlDbType.DateTime).Value = DateTime.Now;
        cmdInsConsASAAS.Parameters.Add("@NOM_CODE", SqlDbType.VarChar).Value = "TRANSFERENCIA";
        cmdInsConsASAAS.Parameters.Add("@NOM_TIPO", SqlDbType.VarChar).Value = "CONFIRMAÇÃO";
        cmdInsConsASAAS.Parameters.Add("@FLG_STATUS", SqlDbType.VarChar).Value = "X";
        cmdInsConsASAAS.Parameters.Add("@NUM_TOKEN", SqlDbType.VarChar).Value = "X";
        cmdInsConsASAAS.ExecuteNonQuery();
        connInsConsASAAS.Close();
        connInsConsASAAS.Dispose();


    }
}