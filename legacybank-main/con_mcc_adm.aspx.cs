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





public partial class con_mcc_adm : System.Web.UI.Page
{
    public static string line;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            ConsultaGeral();
        }

    }
    protected void btnPesquisar_Click(object sender, EventArgs e)
    {
        ConsultaGeral();
    }

    private void ConsultaGeral()
    {
        //dtgConsulta.Visible = true;
        SqlConnection myConsulta = new SqlConnection(Funcoes.conexao());
        myConsulta.Open();
        SqlDataAdapter SDAConsulta = new SqlDataAdapter("dbo.stp_mcc_ins", myConsulta);
        SDAConsulta.SelectCommand.CommandType = CommandType.StoredProcedure;
        SDAConsulta.SelectCommand.Parameters.Add("@flg_operacao", SqlDbType.Char).Value = "L";
        SDAConsulta.SelectCommand.Parameters.Add("@NOM_MCC", SqlDbType.VarChar).Value = txtFiltro.Text.ToString();

        DataSet dsConsulta = new DataSet();
        SDAConsulta.Fill(dsConsulta, "MCC");
        rptConsulta.DataSource = dsConsulta.Tables["MCC"].DefaultView;
        rptConsulta.DataBind();
        myConsulta.Close(); myConsulta.Dispose();
    }


    protected void rptConsulta_OnItemCommand(object source, RepeaterCommandEventArgs e)
    {

    }

    public string GetPostBackScript()
    {
        PostBackOptions options = new PostBackOptions(btnPostback);
        Page.ClientScript.RegisterForEventValidation(options);

        return Page.ClientScript.GetPostBackEventReference(options);
    }

    protected void btnPostBack_Click(object sender, EventArgs e)
    {
        ConsultaGeral();
    }



    private void ImportarMCC()
    {
        ServicePointManager.Expect100Continue = true;
        ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12
        | SecurityProtocolType.Tls11
        | SecurityProtocolType.Tls
        | SecurityProtocolType.Ssl3;

        // Convert to Base64
        int iOffSet = 0;
        int iLimit = 100;

        bool bhas_more = true;

        while (bhas_more == true)
        {


            var byteArray = Encoding.ASCII.GetBytes(HttpContext.Current.Session["USERNAMEMARKETPLACE"].ToString() + ":" + "");
            string encodeString = Convert.ToBase64String(byteArray);

            var byteArray2 = Encoding.ASCII.GetBytes("zpk_test_K1Rq8nVmA9n2lDWkzQsh3HjX" + ":" + "");
            string encodeString2 = Convert.ToBase64String(byteArray2);

            var myUri = new Uri("https://api.zoop.ws/v1/merchant_category_codes?offset="+iOffSet.ToString()+"&limit="+iLimit.ToString());
            var myWebRequest = WebRequest.Create(myUri);
            var myHttpWebRequest = (HttpWebRequest)myWebRequest;
            myHttpWebRequest.PreAuthenticate = true;
            myHttpWebRequest.Headers.Add("Authorization", "Basic " + encodeString.ToString() + "");
            myHttpWebRequest.Accept = "application/json";
            //enBrX3Rlc3RfSzFScThuVm1BOW4ybERXa3pRc2gzSGpYOg==

            var myWebResponse = myWebRequest.GetResponse();
            var responseStream = myWebResponse.GetResponseStream();

            //if (responseStream == null) return null;

            StreamReader myStreamReader = new StreamReader(responseStream, Encoding.UTF8);
            var json = myStreamReader.ReadToEnd();

            //txtJson.Text = json.ToString();
            JObject o = JObject.Parse(json);

            bhas_more = Convert. ToBoolean(o["has_more"].ToString());

            for (int i = 0; i < o["items"].Count(); i++)
            {

                SqlConnection connInsCons = new SqlConnection(Funcoes.conexao());
                connInsCons.Open();
                SqlCommand cmdInsCons = new SqlCommand("dbo.stp_mcc_ins", connInsCons);
                cmdInsCons.CommandType = CommandType.StoredProcedure;
                cmdInsCons.Parameters.Add("@flg_operacao", SqlDbType.Char).Value = 'I';
                cmdInsCons.Parameters.Add("@COD_ID", SqlDbType.Int).Value = Funcoes.strToInt(o["items"][i]["code"].ToString());
                cmdInsCons.Parameters.Add("@COD_REFERENCIA", SqlDbType.Int).Value = Funcoes.strToInt(o["items"][i]["id"].ToString());
                cmdInsCons.Parameters.Add("@FLG_ATUALIZADO", SqlDbType.Char).Value = "S";
                cmdInsCons.Parameters.Add("@NOM_MCC", SqlDbType.VarChar).Value = o["items"][i]["category"].ToString() + " - " + o["items"][i]["description"].ToString();


                cmdInsCons.ExecuteNonQuery();
                connInsCons.Close();
                connInsCons.Dispose();


            }
            ClientScript.RegisterStartupScript(this.GetType(),
                "Alerta", "alert('Lote Importado com sucesso');  ", true);
            if (bhas_more == true)
            {
                iOffSet = iOffSet + 100;
            }

            responseStream.Close();
            myWebResponse.Close();
        }
    
    }
    protected void btnCarregar_Click(object sender, EventArgs e)
    {
        ImportarMCC();
    }
}