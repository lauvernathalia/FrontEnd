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


public partial class events_adiq_trans : System.Web.UI.Page
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

        string sConexao = ConfigurationManager.AppSettings["user"].ToString() + ";" +
            ConfigurationManager.AppSettings["password"].ToString() + ";" +
            ConfigurationManager.AppSettings["datasource"].ToString() + ";" +
            ConfigurationManager.AppSettings["basecatalog"].ToString();

        SqlConnection connInsConsADIQ_TRANS = new SqlConnection(sConexao);
        connInsConsADIQ_TRANS.Open();
        SqlCommand cmdInsConsADIQ_TRANS = new SqlCommand("dbo.stp_notificacoes_adiq_ins", connInsConsADIQ_TRANS);
        cmdInsConsADIQ_TRANS.CommandType = CommandType.StoredProcedure;
        cmdInsConsADIQ_TRANS.Parameters.Add("@flg_operacao", SqlDbType.Char).Value = 'I';
        cmdInsConsADIQ_TRANS.Parameters.Add("@DES_JSON", SqlDbType.Text).Value = bodyText.ToString();
        cmdInsConsADIQ_TRANS.Parameters.Add("@DTA_DATA", SqlDbType.DateTime).Value = DateTime.Now;
        cmdInsConsADIQ_TRANS.Parameters.Add("@NOM_CODE", SqlDbType.VarChar).Value = "00000000000000000000";
        cmdInsConsADIQ_TRANS.Parameters.Add("@NUM_TOKEN", SqlDbType.VarChar).Value = "0000000000000000000";
        cmdInsConsADIQ_TRANS.Parameters.Add("@NOM_TIPO", SqlDbType.VarChar).Value = "INDEFINIDO";
        cmdInsConsADIQ_TRANS.Parameters.Add("@FLG_STATUS", SqlDbType.VarChar).Value = "INDEFINIDO";
        cmdInsConsADIQ_TRANS.ExecuteNonQuery();
        connInsConsADIQ_TRANS.Close();
        connInsConsADIQ_TRANS.Dispose();
    }
}