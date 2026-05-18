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


public partial class index : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

        try
        {
            int loop1;
            NameValueCollection coll;

            //Load Form variables into NameValueCollection variable.
            coll = Request.Form;
            // Get names of all forms into a string array.
            String[] arr1 = coll.AllKeys;
            for (loop1 = 0; loop1 < arr1.Length; loop1++)
            {
                //txtResultado.Text = txtResultado.Text + "Form: " + arr1[loop1].ToString() + "<br>" + "\n";
                //txtResultado.Text = txtResultado.Text + Request.Form["notificationCode"].ToString() + "\n";
                //txtResultado.Text = txtResultado.Text + Request.Form["notificationType"].ToString() + "\n";
            }

            //StreamWriter strm = new StreamWriter(Server.MapPath("public_html") + "\\" + Request.Form["notificationCode"].ToString() + "_" + "notifications.txt");
            //strm.WriteLine(Request.QueryString["id"].ToString());
            //strm.WriteLine(Request.Form["notificationCode"].ToString());
            //strm.WriteLine(Request.Form["notificationType"].ToString());
            //strm.Close();

            string sConexao = ConfigurationManager.AppSettings["user"].ToString() + ";" +
                ConfigurationManager.AppSettings["password"].ToString() + ";" +
                ConfigurationManager.AppSettings["datasource"].ToString() + ";" +
                ConfigurationManager.AppSettings["basecatalog"].ToString();

            SqlConnection connInsConsNP = new SqlConnection(sConexao);
            connInsConsNP.Open();
            SqlCommand cmdInsConsNP = new SqlCommand("dbo.stp_notificacoes_pagseguro_ins", connInsConsNP);
            cmdInsConsNP.CommandType = CommandType.StoredProcedure;
            cmdInsConsNP.Parameters.Add("@flg_operacao", SqlDbType.Char).Value = 'I';
            cmdInsConsNP.Parameters.Add("@NOM_TOKEN", SqlDbType.VarChar).Value = Request.QueryString["id"].ToString();
            cmdInsConsNP.Parameters.Add("@NOM_CODE", SqlDbType.VarChar).Value = Request.Form["notificationCode"].ToString();
            cmdInsConsNP.Parameters.Add("@NOM_TYPE", SqlDbType.VarChar).Value = Request.Form["notificationType"].ToString();
            cmdInsConsNP.ExecuteNonQuery();
            connInsConsNP.Close();
            connInsConsNP.Dispose();


            // Seleciona o e-mail e insere os dados

            ServicePointManager.Expect100Continue = true;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12
            | SecurityProtocolType.Tls11
            | SecurityProtocolType.Tls
            | SecurityProtocolType.Ssl3;


            SqlConnection mySelCadastro = new SqlConnection(Funcoes.conexao());
            mySelCadastro.Open();
            SqlCommand cmdSelCadastro = new SqlCommand("dbo.stp_notificacoes_pagseguro_ins", mySelCadastro);
            cmdSelCadastro.CommandType = CommandType.StoredProcedure;
            cmdSelCadastro.Parameters.Add("@flg_operacao", SqlDbType.Char).Value = "L";
            //cmdSelCadastro.Parameters.Add("@COD_ID_PESSOA_LICENCIADO", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["LICENCIADO"].ToString());
            cmdSelCadastro.Parameters.Add("@NOM_TOKEN", SqlDbType.VarChar).Value = Request.QueryString["id"].ToString();
            cmdSelCadastro.Parameters.Add("@NOM_CODE", SqlDbType.VarChar).Value = Request.Form["notificationCode"].ToString();
            SqlDataReader ReaderCadastro = cmdSelCadastro.ExecuteReader();
            while (ReaderCadastro.Read())
            {
                if (ReaderCadastro["NOM_EMAIL"].ToString().Trim() != "")
                {
                    //txtResultado.Text = txtResultado.Text + ReaderCadastro["NOM_EMAIL"].ToString() + "\n";

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
                            cmdInsCons.Parameters.Add("@COD_ID_PESSOA_LICENCIADO", SqlDbType.Int).Value = Funcoes.strToInt(ReaderCadastro["COD_ID_PESSOA_LICENCIADO"].ToString());
                            cmdInsCons.Parameters.Add("@COD_ID_PESSOAS_FJ", SqlDbType.Int).Value = Funcoes.strToInt(ReaderCadastro["COD_ID_PESSOAS_FJ"].ToString());
                            cmdInsCons.Parameters.Add("@COD_ID_PLANO", SqlDbType.Int).Value = Funcoes.strToInt(ReaderCadastro["COD_ID_PLANO"].ToString());
                            cmdInsCons.Parameters.Add("@FLG_ORIGEM", SqlDbType.Char).Value = "P";
                            //txtResultado.Text = txtResultado.Text + dataDictionary["transaction.date"] + "\n";
                            cmdInsCons.Parameters.Add("@DTA_DATA", SqlDbType.DateTime).Value = Convert.ToDateTime(dataDictionary["transaction.date"]);
                            cmdInsCons.Parameters.Add("@NOM_CODE", SqlDbType.VarChar).Value = dataDictionary["transaction.code"];
                            cmdInsCons.Parameters.Add("@COD_ID_TIPO", SqlDbType.Int).Value = Funcoes.strToInt(dataDictionary["transaction.type"]);
                            
                            cmdInsCons.Parameters.Add("@COD_ID_STATUS", SqlDbType.Int).Value = Funcoes.strToInt(dataDictionary["transaction.status"]);

                            cmdInsCons.Parameters.Add("@NOM_STATUS", SqlDbType.VarChar).Value = ObterStatus(dataDictionary["transaction.status"].ToString());
                            // Localizar na tabela de status da PAGSEGURO

                            cmdInsCons.Parameters.Add("@DTA_DATA_ULTIMA_ATUALIZACAO", SqlDbType.DateTime).Value = Convert.ToDateTime(dataDictionary["transaction.lastEventDate"]);

                            cmdInsCons.Parameters.Add("@COD_ID_TIPO_PAGAMENTO", SqlDbType.Int).Value = Funcoes.strToInt(dataDictionary["transaction.paymentMethod.type"]);
                            // Localizar o CODIGO PAGAMENTO e enviar como NOM_TIPO_OPERACAO
                            cmdInsCons.Parameters.Add("@NOM_TIPO_OPERACAO", SqlDbType.VarChar).Value = RetornaTipoPagamento(dataDictionary["transaction.paymentMethod.type"].ToString());

                            cmdInsCons.Parameters.Add("@COD_ID_CODIGO_PAGAMENTO", SqlDbType.Int).Value = Funcoes.strToInt(dataDictionary["transaction.paymentMethod.code"]);
                            // Localizar o CODIGO PAGAMENTO e enviar como NOM_TIPO_OPERACAO_PAGAMENTO
                            cmdInsCons.Parameters.Add("@NOM_TIPO_OPERACAO_PAGAMENTO", SqlDbType.VarChar).Value = RetornaCodigoPagamento(dataDictionary["transaction.paymentMethod.code"]);
                            if (ObterBandeiraCartao((dataDictionary.ContainsKey("transaction.deviceInfo.bin")) ? dataDictionary["transaction.deviceInfo.bin"] : "").ToString().Trim() == "")
                            {
                                cmdInsCons.Parameters.Add("@NOM_TIPO_OPERACAO_BANDEIRA", SqlDbType.VarChar).Value = RetornaTipoPagamento(dataDictionary["transaction.paymentMethod.type"].ToString());
                            }
                            else
                            {
                                cmdInsCons.Parameters.Add("@NOM_TIPO_OPERACAO_BANDEIRA", SqlDbType.VarChar).Value = ObterBandeiraCartao((dataDictionary.ContainsKey("transaction.deviceInfo.bin")) ? dataDictionary["transaction.deviceInfo.bin"] : "");
                            }

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
                            cmdInsConsU.Parameters.Add("@NOM_TOKEN", SqlDbType.VarChar).Value = Request.QueryString["id"].ToString();
                            cmdInsConsU.Parameters.Add("@NOM_CODE", SqlDbType.VarChar).Value = Request.Form["notificationCode"].ToString();

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
        catch
        {

            string sConexao = ConfigurationManager.AppSettings["user"].ToString() + ";" +
                ConfigurationManager.AppSettings["password"].ToString() + ";" +
                ConfigurationManager.AppSettings["datasource"].ToString() + ";" +
                ConfigurationManager.AppSettings["basecatalog"].ToString();

            SqlConnection connInsConsNP = new SqlConnection(sConexao);
            connInsConsNP.Open();
            SqlCommand cmdInsConsNP = new SqlCommand("dbo.stp_notificacoes_pagseguro_ins", connInsConsNP);
            cmdInsConsNP.CommandType = CommandType.StoredProcedure;
            cmdInsConsNP.Parameters.Add("@flg_operacao", SqlDbType.Char).Value = 'I';
            cmdInsConsNP.Parameters.Add("@NOM_TOKEN", SqlDbType.VarChar).Value = "ERRO";
            cmdInsConsNP.Parameters.Add("@NOM_CODE", SqlDbType.VarChar).Value = "ERRO";
            cmdInsConsNP.Parameters.Add("@NOM_TYPE", SqlDbType.VarChar).Value = "ERRO";
            cmdInsConsNP.ExecuteNonQuery();
            connInsConsNP.Close();
            connInsConsNP.Dispose();

        }


        //string TransacaoID = Request.Params.AllKeys["notificationCode"].ToString();
        //string TransacaoIDPost = Request.Params["notificationCode"].ToString();
        /* 
        */
        /*
        try
        {
            string id = "";
            string notificationCode = "";
            string notificationType = "";
            
            
            if (!String.IsNullOrEmpty(Request.QueryString["id"]))
            {
                id = Request.QueryString["id"].ToString();
            }
            if (!String.IsNullOrEmpty(Request.QueryString["notificationCode"]))
            {
                notificationCode = Request.QueryString["notificationCode"].ToString();
            }
            if (!String.IsNullOrEmpty(Request.QueryString["notificationType"]))
            {
                notificationType = Request.QueryString["notificationType"].ToString();
            }
            if (id.ToString().Trim() != "")
            {
                StreamWriter strm = new StreamWriter(Server.MapPath("public_html") + "\\" + id.ToString()+"_"+"notifications.txt");
                strm.WriteLine(id.ToString() + " - " + notificationCode.ToString() + "-" + notificationType.ToString());
                strm.Close();
            }
        }
        catch
        {

        }*/
    }

    public static string RetornaTipoPagamento(string sTipoPagamento)
    {
        string sRetorno = "";
        switch (sTipoPagamento)
        {
            case "1":
                sRetorno = "Crédito";
                break;
            case "2":
                sRetorno = "Boleto";
                break;
            case "3":
                sRetorno = "Débito";
                break;
            case "4":
                sRetorno = "Débito";
                break;
            //case "5":
            //    sTipoPagamento = "Oi Paggo";
            //    break;
            //case "7":
            //    sTipoPagamento = "Depósito em conta";
            //    break;
            case "8":
                sRetorno = "Débito";
                break;
            case "11":
                sRetorno = "Pix";
                break;
            case "12":
                sRetorno = "Crédito";
                break;
            case "13":
                sRetorno = "Débito";
                break;
            case "14":
                sRetorno = "Crédito";
                break;
            case "15":
                sRetorno = "Débito";
                break;
            case "16":
                sRetorno = "Crédito";
                break;
            case "17":
                sRetorno = "Débito";
                break;
            
            default:
                sRetorno = "Não Identificado";
                break;
        }
        return sRetorno.ToString();
    }

    public static string RetornaCodigoPagamento(string sCodigoPagamento)
    {

        string sRetorno = "";
        switch (sCodigoPagamento)
        {
            case "11":
                sRetorno = "Pix";
                break;
            case "101":
                sRetorno = "Cartão de crédito Visa";
                break;
            case "102":
                sRetorno = "Cartão de crédito MasterCard";
                break;
            case "103":
                sRetorno = "Cartão de crédito American Express";
                break;
            case "104":
                sRetorno = "Cartão de crédito Diners";
                break;
            case "105":
                sRetorno = "Cartão de crédito Hipercard";
                break;
            case "106":
                sRetorno = "Cartão de crédito Aura";
                break;
            case "107":
                sRetorno = "Cartão de crédito Elo";
                break;
            case "108":
                sRetorno = "Cartão de crédito PLENOCard";
                break;
            case "109":
                sRetorno = "Cartão de crédito PersonalCard";
                break;
            case "110":
                sRetorno = "Cartão de crédito JCB";
                break;
            case "111":
                sRetorno = "Cartão de crédito Discover";
                break;
            case "112":
                sRetorno = "Cartão de crédito BrasilCard";
                break;
            case "113":
                sRetorno = "Cartão de crédito FORTBRASIL";
                break;
            case "114":
                sRetorno = "Cartão de crédito CARDBAN";
                break;
            case "115":
                sRetorno = "Cartão de crédito VALECARD";
                break;
            case "116":
                sRetorno = "Cartão de crédito Cabal";
                break;
            case "117":
                sRetorno = "Cartão de crédito Mais";
                break;
            case "118":
                sRetorno = "Cartão de crédito Avista";
                break;
            case "119":
                sRetorno = "Cartão de crédito GRANDCARD";
                break;
            case "120":
                sRetorno = "Cartão de crédito Sorocred";
                break;
            case "122":
                sRetorno = "Cartão de crédito Up Policard";
                break;
            case "123":
                sRetorno = "Cartão de crédito Banese Card";
                break;
            case "201":
                sRetorno = "Boleto Bradesco";
                break;
            case "202":
                sRetorno = "Boleto Santander";
                break;
            case "301":
                sRetorno = "Débito online Bradesco";
                break;
            case "302":
                sRetorno = "Débito online Itaú";
                break;
            case "303":
                sRetorno = "Débito online Unibanco";
                break;
            case "304":
                sRetorno = "Débito online Banco do Brasil";
                break;
            case "305":
                sRetorno = "Débito online Banco Real";
                break;
            case "306":
                sRetorno = "Débito online Banrisul";
                break;
            case "307":
                sRetorno = "Débito online HSBC";
                break;
            case "401":
                sRetorno = "Saldo PagSeguro";
                break;
            case "402":
                sRetorno = "PIX";
                break;
            case "501":
                sRetorno = "Oi Paggo";
                break;
            case "701":
                sRetorno = "Depósito em conta - Banco do Brasil";
                break;
            case "802":
                sRetorno = "Cartão Auxílio Emergencial Mastercard";
                break;
            case "801":
                sRetorno = "Cartão Auxílio Emergencial Visa";
                break;
            case "803":
                sRetorno = "Cartão Auxílio Emergencial American Express";
                break;
            case "804":
                sRetorno = "Cartão de crédito Diners";
                break;
            case "805":
                sRetorno = "Cartão de crédito Hipercard";
                break;
            case "806":
                sRetorno = "Cartão de crédito Aura";
                break;
            case "807":
                sRetorno = "Cartão de crédito Elo";
                break;
            default:
                sRetorno = "Não Identificado";
                break;
        }
        return sRetorno.ToString();

    }

    
    public static string ObterBandeiraCartao(string bin)
    {
        if (string.IsNullOrEmpty(bin) || bin.Length < 6)
            return "";

        if (bin.StartsWith("384100") || bin.StartsWith("384140") || bin.StartsWith("384160") || bin.StartsWith("606282") || bin.StartsWith("637095") || bin.StartsWith("637568")
            || bin.StartsWith("637599") || bin.StartsWith("637609") || bin.StartsWith("637612"))
            return "Hipercard";

        else if (bin.StartsWith("4011") || bin.StartsWith("4389") || bin.StartsWith("4514") || bin.StartsWith("4576") || bin.StartsWith("5041") || bin.StartsWith("506")
            || bin.StartsWith("509") || bin.StartsWith("636") || bin.StartsWith("650") || bin.StartsWith("651") || bin.StartsWith("655"))
            return "Elo";

        else if (bin.StartsWith("40240071") || bin.StartsWith("4539") || bin.StartsWith("4556") || bin.StartsWith("4916") || bin.StartsWith("4532") || bin.StartsWith("4929") || bin.StartsWith("4485") || bin.StartsWith("4716"))
            return "Visa";

        //else if (bin >= "222100" && bin <= "272099")
        //    return "MasterCard";

        else if (bin.StartsWith("36") || bin.StartsWith("300") || bin.StartsWith("305") || bin.StartsWith("3095") || bin.StartsWith("38") || bin.StartsWith("39")) 
            return "Diners";

        else if (bin.StartsWith("6369"))
            return "Banescard";

        else if (bin.StartsWith("50"))
            return "MasterCard";

        else if (bin.StartsWith("5078"))
            return "Aura";

        else if (bin.StartsWith("6042") || bin.StartsWith("589657"))
            return "Cabal";

        else if (bin.StartsWith("4"))
            return "Visa";

        else if (bin.StartsWith("5") && "12345".Contains(bin[1]))
            return "MasterCard";
        
        else if (bin.StartsWith("34") || bin.StartsWith("37"))
            return "American Express";
        
        else if (bin.StartsWith("6"))
            return "MasterCard";
        
        else if (bin.StartsWith("35"))
            return "JCB";
        
        else if (bin.StartsWith("30") || bin.StartsWith("36") || bin.StartsWith("38"))
            return "Diners Club";
        else
            return "Outros";
    }
    public static string ObterStatus(string sCodigoStatus)
    {
        string sRetorno = "";
        switch (sCodigoStatus)
        {

            case "1":
                sRetorno = "Aguardando pagamento";
                break;
            case "2":
                sRetorno = "Em análise";
                break;
            case "3":
                sRetorno = "Paga";
                break;
            case "4":
                sRetorno = "Disponível";
                break;
            case "5":
                sRetorno = "Em Disputa";
                break;
            case "6":
                sRetorno = "Devolvida";
                break;
            case "7":
                sRetorno = "Cancelada";
                break;
            case "8":
                sRetorno = "Debitado";
                break;
            case "9":
                sRetorno = "Retenção temporária";
                break;
            default:
                sRetorno = "Não Identificado";
                break;
        }
        return sRetorno.ToString();

    }    

}