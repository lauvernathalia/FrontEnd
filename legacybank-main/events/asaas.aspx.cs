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


public partial class events_asaas : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

        //Parallel.Invoke(() => DoSomeWork(), () => DoSomeOtherWork());

        ServicePointManager.Expect100Continue = true;
        ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12
        | SecurityProtocolType.Tls11
        | SecurityProtocolType.Tls
        | SecurityProtocolType.Ssl3;

        var bodyStream = new StreamReader(HttpContext.Current.Request.InputStream);
        bodyStream.BaseStream.Seek(0, SeekOrigin.Begin);
        var bodyText = bodyStream.ReadToEnd();

        string sResultado = bodyText.ToString() + '\n';
        //StreamWriter strm = new StreamWriter(Server.MapPath("public_html") + "\\" + DateTime.Now.Day.ToString() + DateTime.Now.Month.ToString() + DateTime.Now.Year.ToString() + DateTime.Now.Minute.ToString() + DateTime.Now.Second.ToString() + "_" + "asaas.txt");
        //strm.WriteLine(sResultado);
        //strm.Close();

        // Grava JSON na Base de dados

        string sConexao = ConfigurationManager.AppSettings["user"].ToString() + ";" +
            ConfigurationManager.AppSettings["password"].ToString() + ";" +
            ConfigurationManager.AppSettings["datasource"].ToString() + ";" +
            ConfigurationManager.AppSettings["basecatalog"].ToString();

        SqlConnection connInsConsASAAS = new SqlConnection(sConexao);
        connInsConsASAAS.Open();
        SqlCommand cmdInsConsASAAS = new SqlCommand("dbo.stp_notificacoes_asaas_ins", connInsConsASAAS);
        cmdInsConsASAAS.CommandType = CommandType.StoredProcedure;
        cmdInsConsASAAS.Parameters.Add("@flg_operacao", SqlDbType.Char).Value = "I";
        cmdInsConsASAAS.Parameters.Add("@DES_JSON", SqlDbType.Text).Value = bodyText.ToString();
        
        //txtJson.Text = bodyText.ToString();
        string sToken = "";
        try
        {
            if (bodyText.ToString().Trim() != "")
            {
                sToken = "";

                string json = bodyText.ToString();
                JObject o = JObject.Parse(json);
                string sType = o["event"].ToString();

                cmdInsConsASAAS.Parameters.Add("@DTA_DATA", SqlDbType.DateTime).Value = Convert.ToDateTime(o["dateCreated"].ToString());
                cmdInsConsASAAS.Parameters.Add("@NOM_CODE", SqlDbType.VarChar).Value = o["id"].ToString();
                cmdInsConsASAAS.Parameters.Add("@NOM_TIPO", SqlDbType.VarChar).Value = o["event"].ToString();

                // Cobranças
                if (sType.Contains("PAYMENT") == true)
                {
                    cmdInsConsASAAS.Parameters.Add("@FLG_STATUS", SqlDbType.VarChar).Value = RetornaDescricaoCobranca(o["event"].ToString()).ToString();
                    cmdInsConsASAAS.Parameters.Add("@NUM_TOKEN", SqlDbType.VarChar).Value = o["payment"]["id"].ToString();
                }
                // Contas
                if (sType.Contains("ACCOUNT") == true)
                {
                    cmdInsConsASAAS.Parameters.Add("@FLG_STATUS", SqlDbType.VarChar).Value = RetornaDescricaoConta(o["event"].ToString()).ToString();
                    cmdInsConsASAAS.Parameters.Add("@NUM_TOKEN", SqlDbType.VarChar).Value = o["accountStatus"]["id"].ToString();
                }

                // Transferencias
                if (sType.Contains("TRANSFER") == true)
                {
                    cmdInsConsASAAS.Parameters.Add("@FLG_STATUS", SqlDbType.VarChar).Value = o["event"].ToString();
                    cmdInsConsASAAS.Parameters.Add("@NUM_TOKEN", SqlDbType.VarChar).Value = o["transfer"]["id"].ToString();
                }

                // Transferencias
                if (sType.Contains("BILL") == true)
                {
                    cmdInsConsASAAS.Parameters.Add("@FLG_STATUS", SqlDbType.VarChar).Value = o["event"].ToString();
                    cmdInsConsASAAS.Parameters.Add("@NUM_TOKEN", SqlDbType.VarChar).Value = o["bill"]["id"].ToString();
                }


                // Antecipações
                if (sType.Contains("RECEIVABLE") == true)
                {
                    cmdInsConsASAAS.Parameters.Add("@FLG_STATUS", SqlDbType.VarChar).Value = o["event"].ToString();
                    cmdInsConsASAAS.Parameters.Add("@NUM_TOKEN", SqlDbType.VarChar).Value = o["anticipation"]["id"].ToString();
                }

                // Assinaturas
                if (sType.Contains("SUBSCRIPTION") == true)
                {
                    cmdInsConsASAAS.Parameters.Add("@FLG_STATUS", SqlDbType.VarChar).Value = o["event"].ToString();
                    cmdInsConsASAAS.Parameters.Add("@NUM_TOKEN", SqlDbType.VarChar).Value = o["subscription"]["id"].ToString();
                }


            }
            else
            {
                cmdInsConsASAAS.Parameters.Add("@DTA_DATA", SqlDbType.DateTime).Value = DateTime.Now;
                cmdInsConsASAAS.Parameters.Add("@NOM_CODE", SqlDbType.VarChar).Value = "00000000000000000000";
                sToken = "";
                cmdInsConsASAAS.Parameters.Add("@NUM_TOKEN", SqlDbType.VarChar).Value = "0000000000000000000";
                cmdInsConsASAAS.Parameters.Add("@NOM_TIPO", SqlDbType.VarChar).Value = "INDEFINIDO";
                cmdInsConsASAAS.Parameters.Add("@FLG_STATUS", SqlDbType.VarChar).Value = "INDEFINIDO";
            }

        }
        catch
        {
            cmdInsConsASAAS.Parameters.Add("@DTA_DATA", SqlDbType.DateTime).Value = DateTime.Now;
            cmdInsConsASAAS.Parameters.Add("@NOM_CODE", SqlDbType.VarChar).Value = "00000000000000000000";
            sToken = "";
            cmdInsConsASAAS.Parameters.Add("@NUM_TOKEN", SqlDbType.VarChar).Value = "0000000000000000000";
            cmdInsConsASAAS.Parameters.Add("@NOM_TIPO", SqlDbType.VarChar).Value = "INDEFINIDO";
            cmdInsConsASAAS.Parameters.Add("@FLG_STATUS", SqlDbType.VarChar).Value = "INDEFINIDO";

        }

        cmdInsConsASAAS.ExecuteNonQuery();
        connInsConsASAAS.Close();
        connInsConsASAAS.Dispose();


        
        if (bodyText.ToString().Trim() != "")
        {
            //try
            //{
                string json = bodyText.ToString();
                JObject o = JObject.Parse(json);

                string sType = o["event"].ToString();
                
                if (sType.Contains("PAYMENT") == true)
                {
                    // PAYMENTS
                    // ATUALIZA TABELA DE COBRANÇAS
                    SqlConnection connInsConsCOBRANCA = new SqlConnection(sConexao);
                    connInsConsCOBRANCA.Open();
                    SqlCommand cmdInsConsCOBRANCA = new SqlCommand("dbo.stp_cobrancas_ins", connInsConsCOBRANCA);
                    cmdInsConsCOBRANCA.CommandType = CommandType.StoredProcedure;
                    cmdInsConsCOBRANCA.Parameters.Add("@flg_operacao", SqlDbType.Char).Value = 'P';
                    cmdInsConsCOBRANCA.Parameters.Add("@COD_ID_COBRANCA", SqlDbType.VarChar).Value = o["payment"]["id"].ToString();
                    cmdInsConsCOBRANCA.Parameters.Add("@FLG_STATUS", SqlDbType.VarChar).Value = RetornaDescricaoCobranca(o["event"].ToString()).ToString();
                    cmdInsConsCOBRANCA.ExecuteNonQuery();
                    connInsConsCOBRANCA.Close();
                    connInsConsCOBRANCA.Dispose();
                    // FIM PAYMENTS

                    // PAYMENTS
                    // ATUALIZA TABELA TRANSACOES

                    // Localizar Licenciado e EC

                    SqlConnection mySelCadastroVendas = new SqlConnection(sConexao);
                    mySelCadastroVendas.Open();
                    SqlCommand cmdSelCadastroVendas = new SqlCommand("dbo.stp_vendas_vendas_ins", mySelCadastroVendas);
                    cmdSelCadastroVendas.CommandType = CommandType.StoredProcedure;
                    cmdSelCadastroVendas.Parameters.Add("@flg_operacao", SqlDbType.Char).Value = "D";
                    cmdSelCadastroVendas.Parameters.Add("@NOM_CODE", SqlDbType.VarChar).Value = o["payment"]["id"].ToString();
                    SqlDataReader ReaderCadastroVendas = cmdSelCadastroVendas.ExecuteReader();
                    int iLicenciado = 0;
                    int iVendedor = 0;
                    while (ReaderCadastroVendas.Read()) // Percorre os registros do banco
                    {
                        iLicenciado = Funcoes.strToInt(ReaderCadastroVendas["COD_ID_PESSOA_LICENCIADO"].ToString());
                        iVendedor = Funcoes.strToInt(ReaderCadastroVendas["COD_ID_PESSOAS_FJ_VENDEDOR"].ToString());
                    }


                    SqlConnection connInsCons = new SqlConnection(sConexao);
                    connInsCons.Open();
                    SqlCommand cmdInsCons = new SqlCommand("dbo.stp_transacoes_ins", connInsCons);
                    cmdInsCons.CommandType = CommandType.StoredProcedure;
                    cmdInsCons.Parameters.Add("@flg_operacao", SqlDbType.Char).Value = 'I';

                    cmdInsCons.Parameters.Add("@COD_ID_PESSOA_LICENCIADO", SqlDbType.Int).Value = iLicenciado;
                    cmdInsCons.Parameters.Add("@COD_ID_PESSOAS_FJ", SqlDbType.Int).Value = iVendedor;

                    cmdInsCons.Parameters.Add("@FLG_ORIGEM", SqlDbType.Char).Value = "A";

                    cmdInsCons.Parameters.Add("@DTA_DATA", SqlDbType.DateTime).Value = Convert.ToDateTime(o["payment"]["dateCreated"].ToString());
                    cmdInsCons.Parameters.Add("@NOM_CODE", SqlDbType.VarChar).Value = o["payment"]["id"].ToString();

                    //cmdInsCons.Parameters.Add("@NUM_TOKEN", SqlDbType.VarChar).Value = o["on_behalf_of"].ToString();

                    cmdInsCons.Parameters.Add("@COD_ID_TIPO", SqlDbType.Int).Value = 1;


                    if (o["payment"]["deleted"].ToString() == "false")
                    {
                        cmdInsCons.Parameters.Add("@NOM_STATUS", SqlDbType.VarChar).Value = RetornaStatusAsaas(o["payment"]["status"].ToString());
                    }
                    else
                    {
                        cmdInsCons.Parameters.Add("@NOM_STATUS", SqlDbType.VarChar).Value = "CANCELADO";
                    }
                    //cmdInsCons.Parameters.Add("@NOM_STATUS", SqlDbType.VarChar).Value = RetornaDescricaoCobranca(o["event"].ToString()).ToString();


                    cmdInsCons.Parameters.Add("@FLG_PRESENCIAL_ONLINE", SqlDbType.Char).Value = "O";

                    cmdInsCons.Parameters.Add("@NOM_TIPO_PAGAMENTO", SqlDbType.VarChar).Value = o["payment"]["billingType"].ToString();
                    cmdInsCons.Parameters.Add("@NOM_TIPO_OPERACAO", SqlDbType.VarChar).Value = RetornaTipoPagamentoAsaas(o["payment"]["billingType"].ToString());

                    if ((o["payment"]["billingType"].ToString() == "PIX"))
                    {
                        cmdInsCons.Parameters.Add("@NOM_CODIGO_PAGAMENTO", SqlDbType.VarChar).Value = "Pix";
                        cmdInsCons.Parameters.Add("@NUM_PARCELAS", SqlDbType.Int).Value = 1;
                        //cmdInsCons.Parameters.Add("@NOM_QRCODE", SqlDbType.VarChar).Value = o["payment"]["pixQrCodeId"].ToString();
                        cmdInsCons.Parameters.Add("@NOM_TIPO_OPERACAO_BANDEIRA", SqlDbType.VarChar).Value = "Pix";

                    }
                    if ((o["payment"]["billingType"].ToString() == "BOLETO"))
                    {
                        cmdInsCons.Parameters.Add("@NOM_CODIGO_PAGAMENTO", SqlDbType.VarChar).Value = "Boleto";
                        cmdInsCons.Parameters.Add("@NUM_PARCELAS", SqlDbType.Int).Value = 1;
                        cmdInsCons.Parameters.Add("@NOM_CODIGO_BARRAS", SqlDbType.VarChar).Value = "";
                        cmdInsCons.Parameters.Add("@NOM_URL_BOLETO", SqlDbType.VarChar).Value = o["payment"]["bankSlipUrl"].ToString();
                        cmdInsCons.Parameters.Add("@NOM_TIPO_OPERACAO_BANDEIRA", SqlDbType.VarChar).Value = "Boleto";
                    }


                    if ((o["payment"]["billingType"].ToString() == "CREDIT_CARD"))
                    {
                        cmdInsCons.Parameters.Add("@NOM_CODIGO_PAGAMENTO", SqlDbType.VarChar).Value = ObterBandeiraCartao(o["payment"]["creditCard"]["creditCardNumber"].ToString());
                        try { cmdInsCons.Parameters.Add("@NUM_PARCELAS", SqlDbType.Int).Value = Funcoes.strToInt(o["payment"]["installmentNumber"].ToString()); }
                        catch { }

                        //cmdInsCons.Parameters.Add("@NUM_PARCELAS", SqlDbType.Int).Value = Funcoes.strToInt(o["installment_plan"]["number_installments"].ToString());

                        cmdInsCons.Parameters.Add("@NOM_CARTAO_4_PRIMEIROS_DIGIROS", SqlDbType.VarChar).Value = o["payment"]["creditCard"]["creditCardNumber"].ToString();
                        cmdInsCons.Parameters.Add("@NOM_TIPO_OPERACAO_BANDEIRA", SqlDbType.VarChar).Value = ObterBandeiraCartao(o["payment"]["creditCard"]["creditCardNumber"].ToString());
                        cmdInsCons.Parameters.Add("@NOM_CARTAO_4_ULTIMOS_DIGITOS", SqlDbType.VarChar).Value = "";//o["payment_method"]["last4_digits"].ToString();
                        cmdInsCons.Parameters.Add("@NOM_CARTAO_MES", SqlDbType.VarChar).Value = ""; // o["payment_method"]["expiration_month"].ToString();
                        cmdInsCons.Parameters.Add("@NOM_CARTAO_ANO", SqlDbType.VarChar).Value = ""; // o["payment_method"]["expiration_year"].ToString();
                    }

                    cmdInsCons.Parameters.Add("@NUM_VALOR_BRUTO", SqlDbType.Float).Value = Funcoes.strToDouble(o["payment"]["value"].ToString());
                    cmdInsCons.Parameters.Add("@NUM_VALOR_DESCONTO", SqlDbType.Float).Value = 0;
                    cmdInsCons.Parameters.Add("@NUM_VALOR_INTERMEDIACAO_RATE", SqlDbType.Float).Value = 0;

                    cmdInsCons.Parameters.Add("@NUM_VALOR_INTERMEDIACAO_FEE", SqlDbType.Float).Value = Funcoes.strToDouble(o["payment"]["value"].ToString()) - Funcoes.strToDouble(o["payment"]["netValue"].ToString());
                    cmdInsCons.Parameters.Add("@NUM_VALOR_LIQUIDO", SqlDbType.Float).Value = Funcoes.strToDouble(o["payment"]["netValue"].ToString());
                    cmdInsCons.Parameters.Add("@NUM_VALOR_EXTRAS", SqlDbType.Float).Value = 0;
                    cmdInsCons.Parameters.Add("@NUM_ITENS", SqlDbType.Int).Value = 1;

                    cmdInsCons.Parameters.Add("@NOM_FONTE_CANCELAMENTO", SqlDbType.VarChar).Value = "";
                    //cmdInsCons.Parameters.Add("@NOM_ORIGEM", SqlDbType.VarChar).Value = o["gateway_authorizer"].ToString();

                    try { cmdInsCons.Parameters.Add("@NOM_CLIENTE", SqlDbType.VarChar).Value = ""; }
                    catch { }


                    cmdInsCons.Parameters.Add("@NOM_NUMERO_TRANSACAO", SqlDbType.VarChar).Value = o["payment"]["invoiceNumber"].ToString();
                    cmdInsCons.Parameters.Add("@NOM_RECIBO", SqlDbType.VarChar).Value = o["payment"]["nossoNumero"].ToString();

                    cmdInsCons.Parameters.Add("@NOM_AUTORIZACAO", SqlDbType.VarChar).Value = ""; // o["transaction_number"].ToString();
                    cmdInsCons.Parameters.Add("@DES_DESCRICAO", SqlDbType.Text).Value = o["payment"]["description"].ToString();

                    cmdInsCons.ExecuteNonQuery();
                    connInsCons.Close();
                    connInsCons.Dispose();



                }

                //if (sType.Contains("TRANSFER") == true)
                //{

                //}
                
                if (sType.Contains("ACCOUNT") == true)
                {
                    // ACCOUNTS
                    // ATUALIZA TABELA DE CONTAS BAAS
                    SqlConnection connInsConsACCOUNT = new SqlConnection(sConexao);
                    connInsConsACCOUNT.Open();
                    SqlCommand cmdInsConsACCOUNT = new SqlCommand("dbo.stp_pessoas_fj_baas_ins", connInsConsACCOUNT);
                    cmdInsConsACCOUNT.CommandType = CommandType.StoredProcedure;
                    cmdInsConsACCOUNT.Parameters.Add("@flg_operacao", SqlDbType.Char).Value = 'O';
                    cmdInsConsACCOUNT.Parameters.Add("@NUM_ID_CONTA_BAAS", SqlDbType.VarChar).Value = o["accountStatus"]["id"].ToString();

                    cmdInsConsACCOUNT.Parameters.Add("@FLG_STATUS_BAAS", SqlDbType.VarChar).Value = RetornaDescricaoConta(o["event"].ToString()).ToString();
                    cmdInsConsACCOUNT.Parameters.Add("@FLG_COMERCIAL_BAAS", SqlDbType.VarChar).Value = RetornaStatusDocumentos(o["accountStatus"]["commercialInfo"].ToString()).ToString();
                    cmdInsConsACCOUNT.Parameters.Add("@FLG_CONTA_BAAS", SqlDbType.VarChar).Value = RetornaStatusDocumentos(o["accountStatus"]["bankAccountInfo"].ToString()).ToString();
                    cmdInsConsACCOUNT.Parameters.Add("@FLG_DOCUMENTACAO_BAAS", SqlDbType.VarChar).Value = RetornaStatusDocumentos(o["accountStatus"]["documentation"].ToString()).ToString();
                    cmdInsConsACCOUNT.ExecuteNonQuery();
                    connInsConsACCOUNT.Close();
                    connInsConsACCOUNT.Dispose();

                    // ATUALIZA TABELA TIMELINE
                    SqlConnection connInsConsTIMELINE = new SqlConnection(sConexao);
                    connInsConsTIMELINE.Open();
                    SqlCommand cmdInsConsTIMELINE = new SqlCommand("dbo.stp_pessoas_fj_baas_timeline_ins", connInsConsTIMELINE);
                    cmdInsConsTIMELINE.CommandType = CommandType.StoredProcedure;
                    cmdInsConsTIMELINE.Parameters.Add("@flg_operacao", SqlDbType.Char).Value = 'I';
                    cmdInsConsTIMELINE.Parameters.Add("@NUM_ID_CONTA_BAAS", SqlDbType.VarChar).Value = o["accountStatus"]["id"].ToString();
                    cmdInsConsTIMELINE.Parameters.Add("@NOM_STATUS", SqlDbType.VarChar).Value = RetornaDescricaoConta(o["event"].ToString()).ToString();
                    cmdInsConsTIMELINE.Parameters.Add("@DTA_TIMELINE", SqlDbType.DateTime).Value = Convert.ToDateTime(o["dateCreated"].ToString());
                    cmdInsConsTIMELINE.ExecuteNonQuery();
                    connInsConsTIMELINE.Close();
                    connInsConsTIMELINE.Dispose();

                    // FIM ACCOUNTS
                }

            //}
            //catch
            //{

            //}
        }
        

    }


    public static string ObterBandeiraCartao(string bin)
    {
        if (string.IsNullOrEmpty(bin) || bin.Length < 4)
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

    public static string RetornaDescricaoConta(string sStatus)
    {

        string sRetorno = "";
        switch (sStatus)
        {
            case "PAYMENT_CREATED":
                sRetorno = "Geração de nova cobrança";
                break;

            case "ACCOUNT_STATUS_BANK_ACCOUNT_INFO_APPROVED":
                sRetorno = "Conta bancária aprovada";
                break;
            case "ACCOUNT_STATUS_BANK_ACCOUNT_INFO_AWAITING_APPROVAL":
                sRetorno = "Conta bancária está em análise";
                break;
            case "ACCOUNT_STATUS_BANK_ACCOUNT_INFO_PENDING":
                sRetorno = "Conta bancária voltou para pendente";
                break;
            case "ACCOUNT_STATUS_BANK_ACCOUNT_INFO_REJECTED":
                sRetorno = "Conta bancária reprovada";
                break;
            case "ACCOUNT_STATUS_COMMERCIAL_INFO_APPROVED":
                sRetorno = "Informações comerciais aprovada";
                break;
            case "ACCOUNT_STATUS_COMMERCIAL_INFO_AWAITING_APPROVAL":
                sRetorno = "Informações comerciais em análise";
                break;
            case "ACCOUNT_STATUS_COMMERCIAL_INFO_PENDING":
                sRetorno = "Informações comerciais voltou para pendente";
                break;
            case "ACCOUNT_STATUS_COMMERCIAL_INFO_REJECTED":
                sRetorno = "Informações comerciais reprovada";
                break;
            case "ACCOUNT_STATUS_DOCUMENT_APPROVED":
                sRetorno = "Documentos aprovados";
                break;
            case "ACCOUNT_STATUS_DOCUMENT_AWAITING_APPROVAL":
                sRetorno = "Documentos em análise";
                break;
            case "ACCOUNT_STATUS_DOCUMENT_PENDING":
                sRetorno = "Documentos voltaram para pendente";
                break;
            case "ACCOUNT_STATUS_DOCUMENT_REJECTED":
                sRetorno = "Documentos reprovados";
                break;
            case "ACCOUNT_STATUS_GENERAL_APPROVAL_APPROVED":
                sRetorno = "Conta aprovada";
                break;
            case "ACCOUNT_STATUS_GENERAL_APPROVAL_AWAITING_APPROVAL":
                sRetorno = "Conta em análise";
                break;
            case "ACCOUNT_STATUS_GENERAL_APPROVAL_PENDING":
                sRetorno = "Conta voltou para pendente";
                break;
            case "ACCOUNT_STATUS_GENERAL_APPROVAL_REJECTED":
                sRetorno = "Conta reprovada";
                break;
            default:
                sRetorno = "Não Identificado";
                break;
        }
        return sRetorno;
    }


    public static string RetornaDescricaoCobranca(string sStatus)
    {
        string sRetorno = "";
        switch (sStatus)
        {
            case "PAYMENT_CREATED":
                sRetorno = "Geração de nova cobrança";
                break;
            case "PAYMENT_AWAITING_RISK_ANALYSIS":
                sRetorno = "Pagamento em cartão aguardando aprovação pela análise manual de risco";
                break;
            case "PAYMENT_APPROVED_BY_RISK_ANALYSIS":
                sRetorno = "Pagamento em cartão aprovado pela análise manual de risco";
                break;
            case "PAYMENT_REPROVED_BY_RISK_ANALYSIS":
                sRetorno = "Pagamento em cartão reprovado pela análise manual de risco";
                break;
            case "PAYMENT_AUTHORIZED":
                sRetorno = "Pagamento em cartão que foi autorizado e precisa ser capturado";
                break;
            case "PAYMENT_UPDATED":
                sRetorno = "Alteração no vencimento ou valor de cobrança existente";
                break;
            case "PAYMENT_CONFIRMED":
                sRetorno = "Cobrança confirmada (pagamento efetuado, porém o saldo ainda não foi disponibilizado)";
                break;
            case "PAYMENT_RECEIVED":
                sRetorno = "Cobrança recebida";
                break;
            case "PAYMENT_CREDIT_CARD_CAPTURE_REFUSED":
                sRetorno = "Falha no pagamento de cartão de crédito";
                break;
            case "PAYMENT_ANTICIPATED":
                sRetorno = "Cobrança antecipada";
                break;
            case "PAYMENT_OVERDUE":
                sRetorno = "Cobrança vencida";
                break;
            case "PAYMENT_DELETED":
                sRetorno = "Cobrança removida";
                break;
            case "PAYMENT_RESTORED":
                sRetorno = "Cobrança restaurada";
                break;
            case "PAYMENT_REFUNDED":
                sRetorno = "Cobrança estornada";
                break;
            case "PAYMENT_PARTIALLY_REFUNDED":
                sRetorno = "Cobrança estornada parcialmente";
                break;
            case "PAYMENT_REFUND_IN_PROGRESS":
                sRetorno = "Estorno em processamento (liquidação já está agendada, cobrança será estornada após executar a liquidação)";
                break;
            case "PAYMENT_RECEIVED_IN_CASH_UNDONE":
                sRetorno = "Recebimento em dinheiro desfeito";
                break;
            case "PAYMENT_CHARGEBACK_REQUESTED":
                sRetorno = "Recebido chargeback";
                break;
            case "PAYMENT_CHARGEBACK_DISPUTE":
                sRetorno = "Em disputa de chargeback (caso sejam apresentados documentos para contestação)";
                break;
            case "PAYMENT_AWAITING_CHARGEBACK_REVERSAL":
                sRetorno = "Disputa vencida, aguardando repasse da adquirente";
                break;
            case "PAYMENT_DUNNING_RECEIVED":
                sRetorno = "Recebimento de negativação";
                break;
            case "PAYMENT_DUNNING_REQUESTED":
                sRetorno = "Requisição de negativação";
                break;
            case "PAYMENT_BANK_SLIP_VIEWED":
                sRetorno = "Boleto da cobrança visualizado pelo cliente";
                break;
            case "PAYMENT_CHECKOUT_VIEWED":
                sRetorno = "Fatura da cobrança visualizada pelo cliente";
                break;
            case "PAYMENT_SPLIT_CANCELLED":
                sRetorno = "Cobrança teve um split cancelado";
                break;
            default:
                sRetorno = "Não Identificado";
                break;
        }
        return sRetorno.ToString();
    }

    public static string RetornaStatusCobranca(string sStatus)
    {
        string sRetorno = "";
        switch (sStatus)
        {
            case "PENDING":
                sRetorno = "Pendente";
                break;
            case "PAID":
                sRetorno = "Pago";
                break;
            case "CANCELLED":
                sRetorno = "Cancelado";
                break;
            case "REFUNDED":
                sRetorno = "Estornado";
                break;
            case "BANK_PROCESSING":
                sRetorno = "Enviado ao banco";
                break;
            case "FAILED":
                sRetorno = "Falhou";
                break;
            case "AWAITING_CHECKOUT_RISK_ANALYSIS_REQUEST":
                sRetorno = "Em análise";
                break;
            default:
                sRetorno = "Não Identificado";
                break;
        }
        return sRetorno.ToString();
    }

    public static string RetornaStatusDocumentos(string sStatus)
    {
        string sRetorno = "";
        switch (sStatus)
        {
            case "REJECTED":
                sRetorno = "Rejeitado";
                break;
            case "APPROVED":
                sRetorno = "Aprovado";
                break;
            case "AWAITING_APPROVAL":
                sRetorno = "Aguardando";
                break;
            case "PENDING":
                sRetorno = "Pendente";
                break;
            default:
                sRetorno = "Não Identificado";
                break;
        }
        return sRetorno.ToString();
    }

    public static string RetornaTipoPagamentoAsaas(string sTipoPagamento)
    {
        string sRetorno = "";
        switch (sTipoPagamento)
        {
            case "BOLETO":
                sRetorno = "Boleto";
                break;
            case "PIX":
                sRetorno = "Pix";
                break;
            case "CREDIT_CARD":
                sRetorno = "Crédito";
                break;
            default:
                sRetorno = "Não Identificado";
                break;
        }
        return sRetorno.ToString();
    }

    public static string RetornaStatusAsaas(string sStatus)
    {
        string sRetorno = "";
        switch (sStatus)
        {
            case "PENDING":
                sRetorno = "PENDENTE";
                break;
            case "RECEIVED":
                sRetorno = "RECEBIDO";
                break;
            case "CONFIRMED":
                sRetorno = "CONFIRMADO";
                break;
            case "OVERDUE":
                sRetorno = "ATRASADO";
                break;
            case "REFUNDED":
                sRetorno = "REEMBOLSADO";
                break;
            case "RECEIVED_IN_CASH":
                sRetorno = "RECEBIDO EM DINHEIRO";
                break;
            case "REFUND_REQUESTED":
                sRetorno = "REEMBOLSO SOLICITADO";
                break;
            case "REFUND_IN_PROGRESS":
                sRetorno = "REEMBOLSO EM PROGRESSO";
                break;
            case "CHARGEBACK_REQUESTED":
                sRetorno = "ESTORNO SOLICITADO";
                break;
            case "CHARGEBACK_DISPUTE":
                sRetorno = "DISPUTA DE ESTORNO";
                break;
            case "AWAITING_CHARGEBACK_REVERSAL":
                sRetorno = "AGUARDANDO REVERSÃO DE ESTORNO";
                break;
            case "DUNNING_REQUESTED":
                sRetorno = "COBRANÇA SOLICITADA";
                break;
            case "DUNNING_RECEIVED":
                sRetorno = "COBRANÇA RECEBIDA";
                break;
            case "AWAITING_RISK_ANALYSIS":
                sRetorno = "AGUARDANDO ANÁLISE DE RISCO ";
                break;
            default:
                sRetorno = "NÃO ESPECIFICADO";
                break;

        }
        return sRetorno.ToString();
    }
    /*
     PENDING 
     RECEIVED 
     CONFIRMED 
     OVERDUE 
     REFUNDED 
     RECEIVED_IN_CASH 
     REFUND_REQUESTED 
     REFUND_IN_PROGRESS 
     CHARGEBACK_REQUESTED 
     CHARGEBACK_DISPUTE 
     AWAITING_CHARGEBACK_REVERSAL 
     DUNNING_REQUESTED 
     DUNNING_RECEIVED 
     AWAITING_RISK_ANALYSIS
      
    PENDENTE
    RECEBIDO
    CONFIRMADO
    ATRASADO
    REEMBOLSADO
    RECEBIDO_EM_DINHEIRO
    REEMBOLSO_SOLICITADO
    REEMBOLSO_EM_PROGRESSO
    ESTORNO_SOLICITADO
    DISPUTA_DE_ESTORNO
    AGUARDANDO_REVERSÃO_DE_ESTORNO
    COBRANÇA_SOLICITADA
    COBRANÇA_RECEBIDA
    AGUARDANDO_ANÁLISE_DE_RISCO      
    */

}