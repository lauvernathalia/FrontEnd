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


public partial class events_autentique : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {


        ServicePointManager.Expect100Continue = true;
        ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12
        | SecurityProtocolType.Tls11
        | SecurityProtocolType.Tls
        | SecurityProtocolType.Ssl3;

        var bodyStream = new StreamReader(HttpContext.Current.Request.InputStream);
        bodyStream.BaseStream.Seek(0, SeekOrigin.Begin);
        var bodyText = bodyStream.ReadToEnd();

        string sResultado = bodyText.ToString() + '\n';
        //StreamWriter strm = new StreamWriter(Server.MapPath("public_html") + "\\" + DateTime.Now.Day.ToString() + DateTime.Now.Month.ToString() + DateTime.Now.Year.ToString() + DateTime.Now.Minute.ToString() + DateTime.Now.Second.ToString() + "_" + "autentique.txt");
        //strm.WriteLine(sResultado);
        //strm.Close();

        // Grava JSON na Base de dados

        string sConexao = ConfigurationManager.AppSettings["user"].ToString() + ";" +
            ConfigurationManager.AppSettings["password"].ToString() + ";" +
            ConfigurationManager.AppSettings["datasource"].ToString() + ";" +
            ConfigurationManager.AppSettings["basecatalog"].ToString();

        SqlConnection connInsConsAUTENTIQUE = new SqlConnection(sConexao);
        connInsConsAUTENTIQUE.Open();
        SqlCommand cmdInsConsAUTENTIQUE = new SqlCommand("dbo.stp_notificacoes_autentique_ins", connInsConsAUTENTIQUE);
        cmdInsConsAUTENTIQUE.CommandType = CommandType.StoredProcedure;
        cmdInsConsAUTENTIQUE.Parameters.Add("@flg_operacao", SqlDbType.Char).Value = 'I';
        cmdInsConsAUTENTIQUE.Parameters.Add("@DES_JSON", SqlDbType.Text).Value = bodyText.ToString();
        cmdInsConsAUTENTIQUE.Parameters.Add("@DTA_DATA", SqlDbType.DateTime).Value = DateTime.Now;
        cmdInsConsAUTENTIQUE.Parameters.Add("@NOM_CODE", SqlDbType.VarChar).Value = "00000000000000000000";
        cmdInsConsAUTENTIQUE.Parameters.Add("@NUM_TOKEN", SqlDbType.VarChar).Value = "0000000000000000000";
        cmdInsConsAUTENTIQUE.Parameters.Add("@NOM_TIPO", SqlDbType.VarChar).Value = "INDEFINIDO";
        cmdInsConsAUTENTIQUE.Parameters.Add("@FLG_STATUS", SqlDbType.VarChar).Value = "INDEFINIDO";

        cmdInsConsAUTENTIQUE.ExecuteNonQuery();
        connInsConsAUTENTIQUE.Close();
        connInsConsAUTENTIQUE.Dispose();

        if (bodyText.ToString().Trim() != "")
        {
            try
            {
                JObject oDocumento = JObject.Parse(bodyText.ToString().Trim());
                if (oDocumento["event"]["type"].ToString().Trim() == "document.updated")
                {
                    SqlConnection connInsConsASSINATURAS = new SqlConnection(sConexao);
                    connInsConsASSINATURAS.Open();
                    SqlCommand cmdInsConsASSINATURAS = new SqlCommand("dbo.stp_pessoas_fj_assinaturas_ins", connInsConsASSINATURAS);
                    cmdInsConsASSINATURAS.CommandType = CommandType.StoredProcedure;
                    cmdInsConsASSINATURAS.Parameters.Add("@flg_operacao", SqlDbType.Char).Value = 'U';
                    cmdInsConsASSINATURAS.Parameters.Add("@NOM_DOCUMENTO_ENVIADO", SqlDbType.VarChar).Value = oDocumento["event"]["data"]["files"]["original"].ToString().Trim();
                    cmdInsConsASSINATURAS.Parameters.Add("@NOM_DOCUMENTO_ASSINADO", SqlDbType.VarChar).Value = oDocumento["event"]["data"]["files"]["signed"].ToString().Trim();
                    cmdInsConsASSINATURAS.Parameters.Add("@COD_ID_DOCUMENTO_ENVIADO", SqlDbType.VarChar).Value = oDocumento["event"]["data"]["id"].ToString().Trim();
                    cmdInsConsASSINATURAS.ExecuteNonQuery();
                    connInsConsASSINATURAS.Close();
                    connInsConsASSINATURAS.Dispose();
                }
                if (oDocumento["event"]["type"].ToString().Trim() == "document.finished")
                {
                    SqlConnection connInsConsASSINATURAS = new SqlConnection(sConexao);
                    connInsConsASSINATURAS.Open();
                    SqlCommand cmdInsConsASSINATURAS = new SqlCommand("dbo.stp_pessoas_fj_assinaturas_ins", connInsConsASSINATURAS);
                    cmdInsConsASSINATURAS.CommandType = CommandType.StoredProcedure;
                    cmdInsConsASSINATURAS.Parameters.Add("@flg_operacao", SqlDbType.Char).Value = 'N';
                    cmdInsConsASSINATURAS.Parameters.Add("@COD_ID_DOCUMENTO_ENVIADO", SqlDbType.VarChar).Value = oDocumento["event"]["data"]["id"].ToString().Trim();
                    cmdInsConsASSINATURAS.ExecuteNonQuery();
                    connInsConsASSINATURAS.Close();
                    connInsConsASSINATURAS.Dispose();
                }
            }
            catch
            {

            }
        }



    }
}