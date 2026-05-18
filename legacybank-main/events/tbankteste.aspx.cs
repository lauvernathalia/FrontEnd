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
public partial class events_tbankteste : System.Web.UI.Page
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
        //StreamWriter strm = new StreamWriter(Server.MapPath("public_html") + "\\" + DateTime.Now.Day.ToString() + DateTime.Now.Month.ToString() + DateTime.Now.Year.ToString() + DateTime.Now.Minute.ToString() + DateTime.Now.Second.ToString() + "_" + "TBANK.txt");
        //strm.WriteLine(sResultado);
        //strm.Close();

        // Grava JSON na Base de dados

        //        string sConexao = ConfigurationManager.AppSettings["user"].ToString() + ";" +
        //            ConfigurationManager.AppSettings["password"].ToString() + ";" +
        //            ConfigurationManager.AppSettings["datasource"].ToString() + ";" +
        //            ConfigurationManager.AppSettings["basecatalog"].ToString();

        SqlConnection connInsConsTBANK = new SqlConnection(Funcoes.conexao());
        connInsConsTBANK.Open();
        SqlCommand cmdInsConsTBANK = new SqlCommand("dbo.stp_notificacoes_tbank_ins", connInsConsTBANK);
        cmdInsConsTBANK.CommandType = CommandType.StoredProcedure;
        cmdInsConsTBANK.Parameters.Add("@flg_operacao", SqlDbType.Char).Value = 'I';
        cmdInsConsTBANK.Parameters.Add("@DES_JSON", SqlDbType.Text).Value = "TESTE" + bodyText.ToString();
        cmdInsConsTBANK.Parameters.Add("@DTA_DATA", SqlDbType.DateTime).Value = DateTime.Now.ToShortDateString();

        cmdInsConsTBANK.ExecuteNonQuery();
        connInsConsTBANK.Close();
        connInsConsTBANK.Dispose();


    }

}