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



public partial class cad_notificacoes_adm : System.Web.UI.Page
{
    public string sid_id
    {
        get
        {
            try { return Request["id"]; }
            catch { return ""; }
        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        // Verifica se o usuário esta autenticado
        if (HttpContext.Current.Session.Count <= 0)
        {
            Response.Redirect("login.aspx");
            FormsAuthentication.SignOut();
        }

        if (!IsPostBack)
        {
            if (Funcoes.strToInt(sid_id) != 0)
            {
                ConsultaFicha();
            }
        }

    }

    private void ConsultaFicha()
    {
        SqlConnection mySelCadastro = new SqlConnection(Funcoes.conexao());
        mySelCadastro.Open();
        SqlCommand cmdSelCadastro = new SqlCommand("dbo.stp_notificacoes_pagseguro_ins", mySelCadastro);
        cmdSelCadastro.CommandType = CommandType.StoredProcedure;
        cmdSelCadastro.Parameters.Add("@flg_operacao", SqlDbType.Char).Value = "S";
        cmdSelCadastro.Parameters.Add("@COD_ID", SqlDbType.Int).Value = Funcoes.strToInt(sid_id.ToString());
        cmdSelCadastro.Parameters.Add("@COD_ID_PESSOA_LICENCIADO", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["LICENCIADO"].ToString());
        SqlDataReader ReaderCadastro = cmdSelCadastro.ExecuteReader();
        while (ReaderCadastro.Read())
        {
            lblID.Text = ReaderCadastro["COD_ID"].ToString();
            lblToken.Text = ReaderCadastro["NOM_TOKEN"].ToString();
            lblCode.Text = ReaderCadastro["NOM_CODE"].ToString();
            lblEmail.Text = ReaderCadastro["NOM_EMAIL"].ToString();
            lblEstabelecimento.Text = ReaderCadastro["COD_ID_PESSOAS_FJ"].ToString();
            lblNomeEstabelecimento.Text = ReaderCadastro["NOM_RAZAOSOCIAL"].ToString();
        }

    }


    protected void btnCancelar_Click(object sender, EventArgs e)
    {
        ClientScript.RegisterStartupScript(this.GetType(),
            "Alerta", "opener.PostBackOnMainPage(); window.close(); ", true);
    }
    protected void btnProcessar_Click(object sender, EventArgs e)
    {
                ServicePointManager.Expect100Continue = true;
        ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 
        | SecurityProtocolType.Tls11 
        | SecurityProtocolType.Tls
        | SecurityProtocolType.Ssl3;
        string s = "";
        using (WebClient client = new WebClient())
        {
            s = client.DownloadString("https://ws.pagseguro.uol.com.br/v3/transactions/notifications/" + lblCode.Text.ToString() + "?token=" + lblToken.Text.ToString() + "&email=" + lblEmail.Text.ToString());
            txtURL.Text = "https://ws.pagseguro.uol.com.br/v3/transactions/notifications/" + lblCode.Text.ToString() + "?token=" + lblToken.Text.ToString() + "&email=" + lblEmail.Text.ToString();

            // Nova Rotina

            XDocument doc = XDocument.Parse(s);
            Dictionary<string, string> dataDictionary = new Dictionary<string, string>();

            foreach (XElement element in doc.Descendants().Where(p => p.HasElements == false))
            {
                int keyInt = 0;
                string keyName = element.Name.LocalName;

                var parent = element.Parent;
                while (parent != null)
                {
                    keyName = parent.Name.LocalName + "." + keyName;

                    parent = parent.Parent;
                }

                while (dataDictionary.ContainsKey(keyName))
                {
                    keyName = keyName + "_" + keyInt++;
                }

                dataDictionary.Add(keyName, element.Value);
                txtRetorno.Text = txtRetorno.Text + keyName + " - " + element.Value + "\n";
            }



            // Insere dados no banco de transações
            Nullable<DateTime> dt = null;

            SqlConnection connInsCons = new SqlConnection(Funcoes.conexao());
            connInsCons.Open();
            SqlCommand cmdInsCons = new SqlCommand("dbo.stp_transacoes_ins", connInsCons);
            cmdInsCons.CommandType = CommandType.StoredProcedure;
            cmdInsCons.Parameters.Add("@flg_operacao", SqlDbType.Char).Value = 'I';
            cmdInsCons.Parameters.Add("@COD_ID_PESSOA_LICENCIADO", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["LICENCIADO"].ToString());
            cmdInsCons.Parameters.Add("@COD_ID_PESSOAS_FJ", SqlDbType.Int).Value = Funcoes.strToInt(lblEstabelecimento.Text.ToString());
            cmdInsCons.Parameters.Add("@FLG_ORIGEM", SqlDbType.Char).Value = "P";

            cmdInsCons.Parameters.Add("@DTA_DATA", SqlDbType.DateTime).Value = Convert.ToDateTime(dataDictionary["transaction.date"]);
            cmdInsCons.Parameters.Add("@NOM_CODE", SqlDbType.VarChar).Value = dataDictionary["transaction.code"];
            cmdInsCons.Parameters.Add("@COD_ID_TIPO", SqlDbType.Int).Value = Funcoes.strToInt(dataDictionary["transaction.type"]);
            cmdInsCons.Parameters.Add("@COD_ID_STATUS", SqlDbType.Int).Value = Funcoes.strToInt(dataDictionary["transaction.status"]);
            cmdInsCons.Parameters.Add("@DTA_DATA_ULTIMA_ATUALIZACAO", SqlDbType.DateTime).Value = Convert.ToDateTime(dataDictionary["transaction.lastEventDate"]);
            cmdInsCons.Parameters.Add("@COD_ID_TIPO_PAGAMENTO", SqlDbType.Int).Value = Funcoes.strToInt(dataDictionary["transaction.paymentMethod.type"]);
            cmdInsCons.Parameters.Add("@COD_ID_CODIGO_PAGAMENTO", SqlDbType.Int).Value = Funcoes.strToInt(dataDictionary["transaction.paymentMethod.code"]);

            cmdInsCons.Parameters.Add("@NUM_VALOR_BRUTO", SqlDbType.Float).Value = (dataDictionary.ContainsKey("transaction.grossAmount")) ? Funcoes.strToDouble(dataDictionary["transaction.grossAmount"]) / 100 : 0;
            cmdInsCons.Parameters.Add("@NUM_VALOR_DESCONTO", SqlDbType.Float).Value = (dataDictionary.ContainsKey("transaction.discountAmount")) ? Funcoes.strToDouble(dataDictionary["transaction.discountAmount"]) / 100 : 0;
            cmdInsCons.Parameters.Add("@NUM_VALOR_INTERMEDIACAO_RATE", SqlDbType.Float).Value = (dataDictionary.ContainsKey("transaction.creditorFees.intermediationRateAmount")) ? Funcoes.strToDouble(dataDictionary["transaction.creditorFees.intermediationRateAmount"]) / 100 : 0;
            cmdInsCons.Parameters.Add("@NUM_VALOR_INTERMEDIACAO_FEE", SqlDbType.Float).Value = (dataDictionary.ContainsKey("transaction.creditorFees.intermediationFeeAmount")) ? Funcoes.strToDouble(dataDictionary["transaction.creditorFees.intermediationFeeAmount"]) / 100 : 0;
            cmdInsCons.Parameters.Add("@NUM_VALOR_LIQUIDO", SqlDbType.Float).Value = (dataDictionary.ContainsKey("transaction.netAmount")) ? Funcoes.strToDouble(dataDictionary["transaction.netAmount"]) / 100 : 0;
            cmdInsCons.Parameters.Add("@NUM_VALOR_EXTRAS", SqlDbType.Float).Value = (dataDictionary.ContainsKey("transaction.extraAmount")) ? Funcoes.strToDouble(dataDictionary["transaction.extraAmount"]) / 100 : 0;

            cmdInsCons.Parameters.Add("@DTA_DATA_DEPOSITO", SqlDbType.DateTime).Value = (dataDictionary.ContainsKey("transaction.escrowEndDate")) ? Convert.ToDateTime(dataDictionary["transaction.escrowEndDate"]) : dt;

            cmdInsCons.Parameters.Add("@NUM_ITENS", SqlDbType.Int).Value = (dataDictionary.ContainsKey("transaction.itemCount")) ? Funcoes.strToInt(dataDictionary["transaction.itemCount"]) : 0;

            cmdInsCons.Parameters.Add("@NOM_FONTE_CANCELAMENTO", SqlDbType.VarChar).Value = (dataDictionary.ContainsKey("transaction.cancellationSource")) ? dataDictionary["transaction.cancellationSource"] : "";


            cmdInsCons.Parameters.Add("@NUM_CHAVE_PUBLICA", SqlDbType.VarChar).Value = dataDictionary["transaction.primaryReceiver.publicKey"];
            cmdInsCons.Parameters.Add("@NUM_DEVICE_REFERENCIA", SqlDbType.VarChar).Value = (dataDictionary.ContainsKey("transaction.deviceInfo.reference")) ? dataDictionary["transaction.deviceInfo.reference"] : "";
            cmdInsCons.Parameters.Add("@NUM_DEVICE_BIN", SqlDbType.Char).Value = (dataDictionary.ContainsKey("transaction.deviceInfo.bin")) ? dataDictionary["transaction.deviceInfo.bin"] : "";
            cmdInsCons.Parameters.Add("@NUM_DEVICE_TITULAR", SqlDbType.VarChar).Value = (dataDictionary.ContainsKey("transaction.deviceInfo.holder")) ? dataDictionary["transaction.deviceInfo.holder"] : "";
            cmdInsCons.Parameters.Add("@NUM_DEVICE_SERIAL", SqlDbType.VarChar).Value = (dataDictionary.ContainsKey("transaction.deviceInfo.serialNumber")) ? dataDictionary["transaction.deviceInfo.serialNumber"] : "";

            cmdInsCons.Parameters.Add("@DTA_PIX", SqlDbType.DateTime).Value = (dataDictionary.ContainsKey("transaction.pix.pixDate")) ? Convert.ToDateTime(dataDictionary["transaction.pix.pixDate"]) : dt;
            cmdInsCons.Parameters.Add("@NOM_PIX_NOME", SqlDbType.VarChar).Value = (dataDictionary.ContainsKey("transaction.pix.holderName")) ? dataDictionary["transaction.pix.holderName"] : "";
            cmdInsCons.Parameters.Add("@NOM_PIX_TIPO_FJ", SqlDbType.VarChar).Value = (dataDictionary.ContainsKey("transaction.pix.personType")) ? dataDictionary["transaction.pix.personType"] : "";
            cmdInsCons.Parameters.Add("@NOM_PIX_NOME_BANCO", SqlDbType.VarChar).Value = (dataDictionary.ContainsKey("transaction.pix.bankName")) ? dataDictionary["transaction.pix.bankName"] : "";
            cmdInsCons.Parameters.Add("@NOM_PIX_AGENCIA_BANCO", SqlDbType.VarChar).Value = (dataDictionary.ContainsKey("transaction.pix.bankAgency")) ? dataDictionary["transaction.pix.bankAgency"] : "";
            cmdInsCons.Parameters.Add("@NOM_PIX_CONTA_BANCO", SqlDbType.VarChar).Value = (dataDictionary.ContainsKey("transaction.pix.bankAccount")) ? dataDictionary["transaction.pix.bankAccount"] : "";
            cmdInsCons.Parameters.Add("@NOM_PIX_TIPO_CONTA_BANCO", SqlDbType.VarChar).Value = (dataDictionary.ContainsKey("transaction.pix.bankAccountType")) ? dataDictionary["transaction.pix.bankAccountType"] : "";
            cmdInsCons.Parameters.Add("@DES_DESCRICAO", SqlDbType.Text).Value = (dataDictionary.ContainsKey("transaction.items.item.description")) ? dataDictionary["transaction.items.item.description"] : "";

            cmdInsCons.ExecuteNonQuery();
            connInsCons.Close();
            connInsCons.Dispose();

            // Atualiza os dados da Notificação

            SqlConnection connInsConsU = new SqlConnection(Funcoes.conexao());
            connInsConsU.Open();
            SqlCommand cmdInsConsU = new SqlCommand("dbo.stp_notificacoes_pagseguro_ins", connInsConsU);
            cmdInsConsU.CommandType = CommandType.StoredProcedure;
            cmdInsConsU.Parameters.Add("@flg_operacao", SqlDbType.Char).Value = "U";
            cmdInsConsU.Parameters.Add("@NOM_TOKEN", SqlDbType.VarChar).Value = lblToken.Text.ToString();
            cmdInsConsU.Parameters.Add("@NOM_CODE", SqlDbType.VarChar).Value = lblCode.Text.ToString();

            cmdInsConsU.ExecuteNonQuery();
            connInsConsU.Close();
            connInsConsU.Dispose();



        }
    }

    Dictionary<string, string> XmlToDictionary(string data)
    {
        XElement rootElement = XElement.Parse(data);
        var names = rootElement.Elements("Key").Elements("Name").Select(n => n.Value);
        var values = rootElement.Elements("Key").Elements("Value").Select(v => v.Value);
        var list = names.Zip(values, (k, v) => new { k, v }).ToDictionary(item => item.k, item => item.v);
        return list;
    }
}