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


public partial class con_notificacoes_adm : System.Web.UI.Page
{
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
        SqlDataAdapter SDAConsulta = new SqlDataAdapter("dbo.stp_notificacoes_pagseguro_ins", myConsulta);
        SDAConsulta.SelectCommand.CommandType = CommandType.StoredProcedure;
        SDAConsulta.SelectCommand.Parameters.Add("@flg_operacao", SqlDbType.Char).Value = "C";
        SDAConsulta.SelectCommand.Parameters.Add("@COD_ID_PESSOA_LICENCIADO", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["LICENCIADO"].ToString());
        SDAConsulta.SelectCommand.Parameters.Add("@NOM_FILTRO", SqlDbType.VarChar).Value = txtFiltro.Text.ToString();
        DataSet dsConsulta = new DataSet();
        SDAConsulta.Fill(dsConsulta, "NOTIFCACOES_PAGSEGURO");
        rptConsulta.DataSource = dsConsulta.Tables["NOTIFCACOES_PAGSEGURO"].DefaultView;
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
    protected void btnProcessar_Click(object sender, EventArgs e)
    {
                ServicePointManager.Expect100Continue = true;
        ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 
        | SecurityProtocolType.Tls11 
        | SecurityProtocolType.Tls
        | SecurityProtocolType.Ssl3;

        
        SqlConnection mySelCadastro = new SqlConnection(Funcoes.conexao());
        mySelCadastro.Open();
        SqlCommand cmdSelCadastro = new SqlCommand("dbo.stp_notificacoes_pagseguro_ins", mySelCadastro);
        cmdSelCadastro.CommandType = CommandType.StoredProcedure;
        cmdSelCadastro.Parameters.Add("@flg_operacao", SqlDbType.Char).Value = "C";
        cmdSelCadastro.Parameters.Add("@COD_ID_PESSOA_LICENCIADO", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["LICENCIADO"].ToString());
        cmdSelCadastro.Parameters.Add("@NOM_FILTRO", SqlDbType.VarChar).Value = "";
        SqlDataReader ReaderCadastro = cmdSelCadastro.ExecuteReader();
        while (ReaderCadastro.Read())
        {
            try
            {
                string s = "";
                using (WebClient client = new WebClient())
                {
                    s = client.DownloadString("https://ws.pagseguro.uol.com.br/v3/transactions/notifications/" + ReaderCadastro["NOM_CODE"].ToString() + "?token=" + ReaderCadastro["NOM_TOKEN"].ToString() + "&email=" + ReaderCadastro["NOM_EMAIL"].ToString());

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
                    }

                    // Insere dados no banco de transações

                    Nullable<DateTime> dt = null;


                    SqlConnection connInsCons = new SqlConnection(Funcoes.conexao());
                    connInsCons.Open();
                    SqlCommand cmdInsCons = new SqlCommand("dbo.stp_transacoes_ins", connInsCons);
                    cmdInsCons.CommandType = CommandType.StoredProcedure;
                    cmdInsCons.Parameters.Add("@flg_operacao", SqlDbType.Char).Value = 'I';
                    cmdInsCons.Parameters.Add("@COD_ID_PESSOA_LICENCIADO", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["LICENCIADO"].ToString());
                    cmdInsCons.Parameters.Add("@COD_ID_PESSOAS_FJ", SqlDbType.Int).Value = Funcoes.strToInt(ReaderCadastro["COD_ID_PESSOAS_FJ"].ToString());
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
                    cmdInsCons.Parameters.Add("@NUM_VALOR_TAXA_PARCELAMENTO", SqlDbType.Float).Value = (dataDictionary.ContainsKey("transaction.creditorFees.installmentFeeAmount")) ? Funcoes.strToDouble(dataDictionary["transaction.creditorFees.installmentFeeAmount"]) / 100 : 0;
                    cmdInsCons.Parameters.Add("@NUM_VALOR_LIQUIDO", SqlDbType.Float).Value = (dataDictionary.ContainsKey("transaction.netAmount")) ? Funcoes.strToDouble(dataDictionary["transaction.netAmount"]) / 100 : 0;
                    cmdInsCons.Parameters.Add("@NUM_VALOR_EXTRAS", SqlDbType.Float).Value = (dataDictionary.ContainsKey("transaction.extraAmount")) ? Funcoes.strToDouble(dataDictionary["transaction.extraAmount"]) / 100 : 0;

                    cmdInsCons.Parameters.Add("@NUM_PARCELAS", SqlDbType.Int).Value = Funcoes.strToInt(dataDictionary["transaction.installmentCount"]);

                    
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
                    cmdInsConsU.Parameters.Add("@NOM_TOKEN", SqlDbType.VarChar).Value = ReaderCadastro["NOM_TOKEN"].ToString();
                    cmdInsConsU.Parameters.Add("@NOM_CODE", SqlDbType.VarChar).Value = ReaderCadastro["NOM_CODE"].ToString();

                    cmdInsConsU.ExecuteNonQuery();
                    connInsConsU.Close();
                    connInsConsU.Dispose();


                }
            }
            catch
            {

            }
        }

    }
}