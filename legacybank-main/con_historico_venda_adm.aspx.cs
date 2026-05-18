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
using System.Data.SqlClient;


using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

using System.IO;
using System.Text;

using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;

using System.ComponentModel;
using System.Xml;
public partial class con_historico_venda_adm : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        string sdate;
        string scode;
        string stype;
        string sstatus;
        string slastEventDate;
        string stypepayment;
        string scodepayment;
        string sgrossAmount;
        string sdiscountAmount;
        string sfeeAmount;
        string snetAmount;
        string sextraAmount;
        string sescrowEndDate;
        string sinstallmentCount;

        string sitemCount;
        string sid;
        string sdescription;
        string squantity;
        string samount;


        string sreference;
        string sbin;
        string sholder;
        string sserialNumber;
        string sdeviceInfo;
        string stransaction;

        ServicePointManager.Expect100Continue = true;
        ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12
        | SecurityProtocolType.Tls11
        | SecurityProtocolType.Tls
        | SecurityProtocolType.Ssl3;


        String URLString = "https://ws.pagseguro.uol.com.br/v2/transactions/notifications/3DBDF5-2BF384F38453-88848FBF9C99-01C4C6?email=reginaldobc.next@hotmail.com&token=d0cdd573-6aad-4249-873a-1ec44f42d7f35a051ccc4654afd48754855c69f208da1390-3d02-4b07-82b1-f27d7094f3f7";
        XmlTextReader reader = new XmlTextReader(URLString);
        string NomeElemento = "";
        Dictionary<string, string> myDict = new Dictionary<string, string>();
        System.Collections.Specialized.NameValueCollection qsCollection = new System.Collections.Specialized.NameValueCollection();
        //XmlTextReader reader = new XmlTextReader(URLString);
        //XmlDocument xmlDoc = new XmlDocument();
        //xmlDoc.Load(reader);

        //XmlNodeList nodeList = xmlDoc.SelectNodes("transaction");
        //string NomeElemento = "";
        while (reader.Read())
        {
            switch (reader.NodeType)
            {
                case XmlNodeType.Element: // The node is an element.

                    while (reader.MoveToNextAttribute()) // Read the attributes.
                        TextBox1.Text = TextBox1.Text + "";

                    NomeElemento = reader.Name;

                    break;
                case XmlNodeType.Text: //Display the text in each element.
                    TextBox1.Text = TextBox1.Text + NomeElemento + " - " + reader.Value + '\n';
                    qsCollection.Add(NomeElemento, reader.Value);
                    string key = NomeElemento;

                    bool keyExists = myDict.ContainsKey(key);
                    if (!keyExists)
                    {
                        myDict.Add(NomeElemento, reader.Value);
                    }
                    else
                    {
                        myDict.Add(NomeElemento + "_1", reader.Value);
                    }
                    break;
                case XmlNodeType.EndElement: //Display the end of the element.
                    TextBox1.Text = TextBox1.Text + "";
                    break;
            }

        }
        //foreach (XmlElement node in xmlDoc.DocumentElement)
        //{
        //    if (node.HasChildNodes)
        //    {
        //        qsCollection.Add(node.Name, node.InnerText);
        //    }
        //}

        TextBox1.Text = TextBox1.Text + myDict["code"].ToString() + "\n";
        TextBox1.Text = TextBox1.Text + myDict["code_1"].ToString() + "\n";
        TextBox1.Text = TextBox1.Text + myDict["type"].ToString() + "\n";
        TextBox1.Text = TextBox1.Text + myDict["type_1"].ToString() + "\n";

        /*while (reader.Read())
        {
            if (reader.IsStartElement())
            {
                //return only when you have START tag  
                switch (reader.Name.ToString())
                {
                    case "code":
                        TextBox1.Text = TextBox1.Text + " " + "Name of the Element is : " + reader.ReadString();
                        break;
                    case "grossAmount":
                        TextBox1.Text = TextBox1.Text + " " + "Your Location is : " + reader.ReadString();
                        break;
                }
            }
            Console.WriteLine("");
        }*/

        /*while (reader.Read())
        {
            switch (reader.NodeType)
            {
                case XmlNodeType.Element: // The node is an element.
                    TextBox1.Text = TextBox1.Text + "<" + reader.Name;

                    while (reader.MoveToNextAttribute()) // Read the attributes.
                    TextBox1.Text = TextBox1.Text + " " + reader.Name + "='" + reader.Value + "'";
                    TextBox1.Text = TextBox1.Text + ">";
                    NomeElemento = reader.Name; 

                    break;
                case XmlNodeType.Text: //Display the text in each element.
                    if (NomeElemento == "lastEventDate")
                    {
                        TextBox1.Text = TextBox1.Text + NomeElemento + reader.Value;
                    }
                    break;
                case XmlNodeType.EndElement: //Display the end of the element.
                    TextBox1.Text = TextBox1.Text + "</" + reader.Name;
                    TextBox1.Text = TextBox1.Text + ">";
                    break;
            }
        }*/



    }
}