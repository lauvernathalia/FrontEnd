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
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Converters;

using System.Collections.Specialized;

using System.Security.Cryptography.X509Certificates;

using MessagingToolkit.QRCode.Codec;
using MessagingToolkit.QRCode.Codec.Data;

using System.Drawing;

/// <summary>
/// Summary description for transacoes
/// </summary>
public class transacoes
{
    public class Venda
    {
        public int CodId { get; set; }
        public int? CodIdPessoaLicenciado { get; set; }
        public int? CodIdPessoasFjVendedor { get; set; }
        public int? CodIdPessoasFjComprador { get; set; }
        public DateTime? DtaData { get; set; }
        public int? CodIdProduto { get; set; }
        public double NumValor { get; set; }
        public DateTime? DtaVencimento { get; set; }
        public string FlgTipo { get; set; }
        public string FlgMulta { get; set; }
        public string FlgTipoMulta { get; set; }
        public double NumMulta { get; set; }
        public string FlgJuros { get; set; }
        public string FlgTipoJuros { get; set; }
        public double NumJuros { get; set; }
        public string FlgDesconto { get; set; }
        public string FlgTipoDesconto { get; set; }
        public double NumDesconto { get; set; }
        public int? NumDias { get; set; }
        public string FlgAviso { get; set; }
        public string DesAviso { get; set; }
        public DateTime? DtaPixVencimento { get; set; }
        public string NumCartao { get; set; }
        public string NomCartao { get; set; }
        public int? NumMesCartao { get; set; }
        public int? NumAnoCartao { get; set; }
        public int? NumCodigoCartao { get; set; }
        public string NomPix { get; set; }
        public string NomBoleto { get; set; }
        public string NomCode { get; set; }
        public string NomCheckout { get; set; }
        public int? CodIdTransacao { get; set; }
        public string NomImagemPix { get; set; }
        public string NomOrigem { get; set; }
        public string DesJson { get; set; }
        public int? NumParcelas { get; set; }
        public string FlgTipoCobranca { get; set; }
        public string FlgBoleto { get; set; }
        public string FlgPix { get; set; }
        public string FlgCartaoCredito { get; set; }
        public string FlgCliente { get; set; }
        public string NomReferenciaProduto { get; set; }
        public string NomDescricaoProduto { get; set; }
        public string FlgProduto { get; set; }
        public string FlgTipoOperacao { get; set; }
        public string FlgOperadora { get; set; }
        public string FlgPrecoParcelamento { get; set; }
        public string FlgSplit { get; set; }
        public string NomEmailLink { get; set; }
        public string NumCnpjCpf { get; set; }
        public string NomNome { get; set; }
        public string NomSobrenome { get; set; }
        public string NomEndereco { get; set; }
        public string NomBairro { get; set; }
        public string NomCidade { get; set; }
        public string NomUf { get; set; }
        public string NomCep { get; set; }
        public string NomEmail { get; set; }
        public string NomNumero { get; set; }
        public string NomComplemento { get; set; }
        public string NomPais { get; set; }
        public string NomCelular { get; set; }
        public string CodIdComprador { get; set; }
        public string NomImagem { get; set; }
        public string FlgFormaSplit { get; set; }
        public string NomBarcodeBoleto { get; set; }
        public string FlgExibirProdutos { get; set; }
        public double NumValor01 { get; set; }
        public double NumValor02 { get; set; }
        public double NumValor03 { get; set; }
        public double NumValor04 { get; set; }
        public double NumValor05 { get; set; }
        public double NumValor06 { get; set; }
        public double NumValor07 { get; set; }
        public double NumValor08 { get; set; }
        public double NumValor09 { get; set; }
        public double NumValor10 { get; set; }
        public double NumValor11 { get; set; }
        public double NumValor12 { get; set; }
        public string FlgLinkPermanente { get; set; }
        public string NomCampo01 { get; set; }
        public string NomConteudoCampo01 { get; set; }
        public int? NumDiasVencimento { get; set; }
        public string FlgIntegracao { get; set; }
        public string FlgEnderecoEntrega { get; set; }
        public string NomEnderecoEntrega { get; set; }
        public string NomNumeroEntrega { get; set; }
        public string NomComplementoEntrega { get; set; }
        public string NomBairroEntrega { get; set; }
        public string NomCidadeEntrega { get; set; }
        public string NomUfEntrega { get; set; }
        public string NomCepEntrega { get; set; }

    }


    public static string ProcessarTransacoesAsaas(int licenciado, int pessoa, string json)
    {
        string sConexao = ConfigurationManager.AppSettings["user"].ToString() + ";" +
        ConfigurationManager.AppSettings["password"].ToString() + ";" +
        ConfigurationManager.AppSettings["datasource"].ToString() + ";" +
        ConfigurationManager.AppSettings["basecatalog"].ToString();

        SqlConnection connInsCons = new SqlConnection(sConexao);
        connInsCons.Open();
        SqlCommand cmdInsCons = new SqlCommand("dbo.stp_transacoes_ins", connInsCons);
        cmdInsCons.CommandType = CommandType.StoredProcedure;
        cmdInsCons.Parameters.Add("@flg_operacao", SqlDbType.Char).Value = 'I';

        cmdInsCons.Parameters.Add("@COD_ID_PESSOA_LICENCIADO", SqlDbType.Int).Value = licenciado;
        cmdInsCons.Parameters.Add("@COD_ID_PESSOAS_FJ", SqlDbType.Int).Value = pessoa;
        cmdInsCons.Parameters.Add("@FLG_ORIGEM", SqlDbType.Char).Value = "A";

        if (json.ToString().Trim() != "")
        {
            JObject o = JObject.Parse(json);

            cmdInsCons.Parameters.Add("@DTA_DATA", SqlDbType.DateTime).Value = Convert.ToDateTime(o["dateCreated"].ToString());
            cmdInsCons.Parameters.Add("@NOM_CODE", SqlDbType.VarChar).Value = o["id"].ToString();

            //cmdInsCons.Parameters.Add("@NUM_TOKEN", SqlDbType.VarChar).Value = o["on_behalf_of"].ToString();

            cmdInsCons.Parameters.Add("@COD_ID_TIPO", SqlDbType.Int).Value = 1;
            cmdInsCons.Parameters.Add("@NOM_STATUS", SqlDbType.VarChar).Value = o["status"].ToString();

            cmdInsCons.Parameters.Add("@FLG_PRESENCIAL_ONLINE", SqlDbType.Char).Value = "O";

            cmdInsCons.Parameters.Add("@NOM_TIPO_PAGAMENTO", SqlDbType.VarChar).Value = o["billingType"].ToString();
            cmdInsCons.Parameters.Add("@NOM_TIPO_OPERACAO", SqlDbType.VarChar).Value = RetornaTipoPagamentoAsaas(o["billingType"].ToString());



            if ((o["billingType"].ToString() == "PIX"))
            {
                // Carregar dados QRCODE
                string sApiKey = asaas.PegarTokenSubconta(Funcoes.strToInt(HttpContext.Current.Session["PESSOA"].ToString()), Funcoes.strToInt(HttpContext.Current.Session["LICENCIADO"].ToString()));
                asaas.HttpResponseResult QRCodeResultado = asaas.ObterQRCodeCobrancaAsaas(sApiKey, o["id"].ToString());
                string LinkPix = "";
                string QRCode = "";

                if ((QRCodeResultado.StatusCode == 200) || (QRCodeResultado.StatusCode == 201))
                {

                    string jsonQRCodeResponse = QRCodeResultado.Content;
                    JObject oQRCodeTransacao = JObject.Parse(jsonQRCodeResponse);

                    LinkPix = oQRCodeTransacao["payload"].ToString();
                    QRCode = oQRCodeTransacao["encodedImage"].ToString();
                }
                cmdInsCons.Parameters.Add("@NOM_CODIGO_PAGAMENTO", SqlDbType.VarChar).Value = "Pix";
                cmdInsCons.Parameters.Add("@NUM_PARCELAS", SqlDbType.Int).Value = 1;
                cmdInsCons.Parameters.Add("@NOM_QRCODE", SqlDbType.VarChar).Value = LinkPix;
                cmdInsCons.Parameters.Add("@NOM_TIPO_OPERACAO_BANDEIRA", SqlDbType.VarChar).Value = "Pix";

            }
            if ((o["billingType"].ToString() == "BOLETO"))
            {
                cmdInsCons.Parameters.Add("@NOM_CODIGO_PAGAMENTO", SqlDbType.VarChar).Value = "Boleto";
                cmdInsCons.Parameters.Add("@NUM_PARCELAS", SqlDbType.Int).Value = 1;
                cmdInsCons.Parameters.Add("@NOM_CODIGO_BARRAS", SqlDbType.VarChar).Value = "";
                cmdInsCons.Parameters.Add("@NOM_URL_BOLETO", SqlDbType.VarChar).Value = o["bankSlipUrl"].ToString();
                cmdInsCons.Parameters.Add("@NOM_TIPO_OPERACAO_BANDEIRA", SqlDbType.VarChar).Value = "Boleto";
            }


            if ((o["billingType"].ToString() == "CREDIT_CARD"))
            {
                cmdInsCons.Parameters.Add("@NOM_CODIGO_PAGAMENTO", SqlDbType.VarChar).Value = ObterBandeiraCartao(o["creditCard"]["creditCardNumber"].ToString());
                try { cmdInsCons.Parameters.Add("@NUM_PARCELAS", SqlDbType.Int).Value = Funcoes.strToInt(o["installmentNumber"].ToString()); }
                catch { }

                //cmdInsCons.Parameters.Add("@NUM_PARCELAS", SqlDbType.Int).Value = Funcoes.strToInt(o["installment_plan"]["number_installments"].ToString());

                cmdInsCons.Parameters.Add("@NOM_CARTAO_4_PRIMEIROS_DIGIROS", SqlDbType.VarChar).Value = o["creditCard"]["creditCardNumber"].ToString();
                cmdInsCons.Parameters.Add("@NOM_TIPO_OPERACAO_BANDEIRA", SqlDbType.VarChar).Value = ObterBandeiraCartao(o["creditCard"]["creditCardNumber"].ToString());
                cmdInsCons.Parameters.Add("@NOM_CARTAO_4_ULTIMOS_DIGITOS", SqlDbType.VarChar).Value = "";//o["payment_method"]["last4_digits"].ToString();
                cmdInsCons.Parameters.Add("@NOM_CARTAO_MES", SqlDbType.VarChar).Value = ""; // o["payment_method"]["expiration_month"].ToString();
                cmdInsCons.Parameters.Add("@NOM_CARTAO_ANO", SqlDbType.VarChar).Value = ""; // o["payment_method"]["expiration_year"].ToString();
            }

            cmdInsCons.Parameters.Add("@NUM_VALOR_BRUTO", SqlDbType.Float).Value = Funcoes.strToDouble(o["value"].ToString());
            cmdInsCons.Parameters.Add("@NUM_VALOR_DESCONTO", SqlDbType.Float).Value = 0;
            cmdInsCons.Parameters.Add("@NUM_VALOR_INTERMEDIACAO_RATE", SqlDbType.Float).Value = 0;

            cmdInsCons.Parameters.Add("@NUM_VALOR_INTERMEDIACAO_FEE", SqlDbType.Float).Value = Funcoes.strToDouble(o["value"].ToString()) - Funcoes.strToDouble(o["netValue"].ToString());
            cmdInsCons.Parameters.Add("@NUM_VALOR_LIQUIDO", SqlDbType.Float).Value = Funcoes.strToDouble(o["netValue"].ToString());
            cmdInsCons.Parameters.Add("@NUM_VALOR_EXTRAS", SqlDbType.Float).Value = 0;
            cmdInsCons.Parameters.Add("@NUM_ITENS", SqlDbType.Int).Value = 1;

            cmdInsCons.Parameters.Add("@NOM_FONTE_CANCELAMENTO", SqlDbType.VarChar).Value = "";
            //cmdInsCons.Parameters.Add("@NOM_ORIGEM", SqlDbType.VarChar).Value = o["gateway_authorizer"].ToString();

            try { cmdInsCons.Parameters.Add("@NOM_CLIENTE", SqlDbType.VarChar).Value = ""; }
            catch { }


            cmdInsCons.Parameters.Add("@NOM_NUMERO_TRANSACAO", SqlDbType.VarChar).Value = o["invoiceNumber"].ToString();
            cmdInsCons.Parameters.Add("@NOM_RECIBO", SqlDbType.VarChar).Value = o["nossoNumero"].ToString();

            cmdInsCons.Parameters.Add("@NOM_AUTORIZACAO", SqlDbType.VarChar).Value = ""; // o["transaction_number"].ToString();
            cmdInsCons.Parameters.Add("@DES_DESCRICAO", SqlDbType.Text).Value = o["description"].ToString();

        }
        string itransacao = cmdInsCons.ExecuteScalar().ToString();
        connInsCons.Close();
        connInsCons.Dispose();

        return itransacao;

    }
    public static string ProcessarTransacoesZoop(int licenciado, int pessoa, string json)
	{
        //try
        //{
            string sConexao = ConfigurationManager.AppSettings["user"].ToString() + ";" +
                ConfigurationManager.AppSettings["password"].ToString() + ";" +
                ConfigurationManager.AppSettings["datasource"].ToString() + ";" +
                ConfigurationManager.AppSettings["basecatalog"].ToString();

            SqlConnection connInsCons = new SqlConnection(sConexao);
            connInsCons.Open();
            SqlCommand cmdInsCons = new SqlCommand("dbo.stp_transacoes_ins", connInsCons);
            cmdInsCons.CommandType = CommandType.StoredProcedure;
            cmdInsCons.Parameters.Add("@flg_operacao", SqlDbType.Char).Value = 'I';

            cmdInsCons.Parameters.Add("@COD_ID_PESSOA_LICENCIADO", SqlDbType.Int).Value = licenciado;
            cmdInsCons.Parameters.Add("@COD_ID_PESSOAS_FJ", SqlDbType.Int).Value = pessoa;
            cmdInsCons.Parameters.Add("@FLG_ORIGEM", SqlDbType.Char).Value = "Z";

            if (json.ToString().Trim() != "")
            {
                JObject o = JObject.Parse(json);

                cmdInsCons.Parameters.Add("@DTA_DATA", SqlDbType.DateTime).Value = Convert.ToDateTime(o["created_at"].ToString());
                cmdInsCons.Parameters.Add("@NOM_CODE", SqlDbType.VarChar).Value = o["id"].ToString();
                cmdInsCons.Parameters.Add("@NUM_TOKEN", SqlDbType.VarChar).Value = o["on_behalf_of"].ToString();

                cmdInsCons.Parameters.Add("@COD_ID_TIPO", SqlDbType.Int).Value = 1;
                cmdInsCons.Parameters.Add("@NOM_STATUS", SqlDbType.VarChar).Value = o["status"].ToString();

                cmdInsCons.Parameters.Add("@FLG_PRESENCIAL_ONLINE", SqlDbType.Char).Value = "O";

                cmdInsCons.Parameters.Add("@NOM_TIPO_PAGAMENTO", SqlDbType.VarChar).Value = o["payment_type"].ToString();
                cmdInsCons.Parameters.Add("@NOM_TIPO_OPERACAO", SqlDbType.VarChar).Value = RetornaTipoPagamento(o["payment_type"].ToString());



                if ((o["payment_type"].ToString() == "pix"))
                {
                    cmdInsCons.Parameters.Add("@NOM_CODIGO_PAGAMENTO", SqlDbType.VarChar).Value = o["payment_method"]["provider"].ToString();
                    cmdInsCons.Parameters.Add("@NUM_PARCELAS", SqlDbType.Int).Value = 1;
                    cmdInsCons.Parameters.Add("@NOM_QRCODE", SqlDbType.VarChar).Value = o["payment_method"]["qr_code"]["emv"].ToString();
                    cmdInsCons.Parameters.Add("@NOM_TIPO_OPERACAO_BANDEIRA", SqlDbType.VarChar).Value = "PIX";

                }
                if ((o["payment_type"].ToString() == "boleto"))
                {
                    cmdInsCons.Parameters.Add("@NOM_CODIGO_PAGAMENTO", SqlDbType.VarChar).Value = o["payment_method"]["resource"].ToString();
                    cmdInsCons.Parameters.Add("@NUM_PARCELAS", SqlDbType.Int).Value = 1;

                    cmdInsCons.Parameters.Add("@NOM_CODIGO_BARRAS", SqlDbType.VarChar).Value = o["payment_method"]["barcode"].ToString();
                    cmdInsCons.Parameters.Add("@NOM_URL_BOLETO", SqlDbType.VarChar).Value = o["payment_method"]["url"].ToString();
                    cmdInsCons.Parameters.Add("@NOM_NUMERO_DOCUMENTO", SqlDbType.VarChar).Value = o["payment_method"]["document_number"].ToString();
                    cmdInsCons.Parameters.Add("@NOM_DATA_VENCIMENTO", SqlDbType.VarChar).Value = o["payment_method"]["expiration_date"].ToString();
                    cmdInsCons.Parameters.Add("@NOM_TIPO_OPERACAO_BANDEIRA", SqlDbType.VarChar).Value = "Boleto";
                }


                if ((o["payment_type"].ToString() == "credit"))
                {
                    cmdInsCons.Parameters.Add("@NOM_CODIGO_PAGAMENTO", SqlDbType.VarChar).Value = o["payment_method"]["resource"].ToString() + " " + o["payment_method"]["card_brand"].ToString();

                    try { cmdInsCons.Parameters.Add("@NUM_PARCELAS", SqlDbType.Int).Value = Funcoes.strToInt(o["installment_plan"]["number_installments"].ToString()); }
                    catch { }

                    //cmdInsCons.Parameters.Add("@NUM_PARCELAS", SqlDbType.Int).Value = Funcoes.strToInt(o["installment_plan"]["number_installments"].ToString());

                    cmdInsCons.Parameters.Add("@NOM_CARTAO_4_PRIMEIROS_DIGIROS", SqlDbType.VarChar).Value = o["payment_method"]["first4_digits"].ToString();
                    cmdInsCons.Parameters.Add("@NOM_TIPO_OPERACAO_BANDEIRA", SqlDbType.VarChar).Value = ObterBandeiraCartao(o["payment_method"]["first4_digits"].ToString());
                    cmdInsCons.Parameters.Add("@NOM_CARTAO_4_ULTIMOS_DIGITOS", SqlDbType.VarChar).Value = o["payment_method"]["last4_digits"].ToString();
                    cmdInsCons.Parameters.Add("@NOM_CARTAO_MES", SqlDbType.VarChar).Value = o["payment_method"]["expiration_month"].ToString();
                    cmdInsCons.Parameters.Add("@NOM_CARTAO_ANO", SqlDbType.VarChar).Value = o["payment_method"]["expiration_year"].ToString();
                }

                if ((o["payment_type"].ToString() == "debit"))
                {

                    cmdInsCons.Parameters.Add("@NOM_CODIGO_PAGAMENTO", SqlDbType.VarChar).Value = o["payment_method"]["resource"].ToString() + " " + o["payment_method"]["card_brand"].ToString();
                    cmdInsCons.Parameters.Add("@NUM_PARCELAS", SqlDbType.Int).Value = 1;

                    cmdInsCons.Parameters.Add("@NOM_CARTAO_4_PRIMEIROS_DIGIROS", SqlDbType.VarChar).Value = o["payment_method"]["first4_digits"].ToString();
                    cmdInsCons.Parameters.Add("@NOM_TIPO_OPERACAO_BANDEIRA", SqlDbType.VarChar).Value = ObterBandeiraCartao(o["payment_method"]["first4_digits"].ToString());
                    cmdInsCons.Parameters.Add("@NOM_CARTAO_4_ULTIMOS_DIGITOS", SqlDbType.VarChar).Value = o["payment_method"]["last4_digits"].ToString();
                    cmdInsCons.Parameters.Add("@NOM_CARTAO_MES", SqlDbType.VarChar).Value = o["payment_method"]["expiration_month"].ToString();
                    cmdInsCons.Parameters.Add("@NOM_CARTAO_ANO", SqlDbType.VarChar).Value = o["payment_method"]["expiration_year"].ToString();
                }



                cmdInsCons.Parameters.Add("@NUM_VALOR_BRUTO", SqlDbType.Float).Value = Funcoes.strToDouble(o["amount"].ToString()) / 100;
                cmdInsCons.Parameters.Add("@NUM_VALOR_DESCONTO", SqlDbType.Float).Value = 0;
                cmdInsCons.Parameters.Add("@NUM_VALOR_INTERMEDIACAO_RATE", SqlDbType.Float).Value = 0;

                cmdInsCons.Parameters.Add("@NUM_VALOR_INTERMEDIACAO_FEE", SqlDbType.Float).Value = Funcoes.strToDouble(o["fees"].ToString()) / 100;
                cmdInsCons.Parameters.Add("@NUM_VALOR_LIQUIDO", SqlDbType.Float).Value = (Funcoes.strToDouble(o["amount"].ToString()) / 100) - (Funcoes.strToDouble(o["fees"].ToString()) / 100);
                cmdInsCons.Parameters.Add("@NUM_VALOR_EXTRAS", SqlDbType.Float).Value = 0;

                cmdInsCons.Parameters.Add("@DTA_DATA_DEPOSITO", SqlDbType.DateTime).Value = Convert.ToDateTime(o["created_at"].ToString());

                cmdInsCons.Parameters.Add("@NUM_ITENS", SqlDbType.Int).Value = 1;

                cmdInsCons.Parameters.Add("@NOM_FONTE_CANCELAMENTO", SqlDbType.VarChar).Value = "";
                cmdInsCons.Parameters.Add("@NOM_ORIGEM", SqlDbType.VarChar).Value = o["gateway_authorizer"].ToString();

                try { cmdInsCons.Parameters.Add("@NOM_CLIENTE", SqlDbType.VarChar).Value = o["payment_method"]["holder_name"].ToString(); }
                catch { }

                // Inserido em 21/06/2024 -----------------------------------------------------------------------
                // Dados do cartao no caso de existência

                //if (o["payment_method"].Contains("first4_digits") == true) { cmdInsCons.Parameters.Add("@NOM_CARTAO_4_PRIMEIROS_DIGIROS", SqlDbType.VarChar).Value = o["payment_method"]["first4_digits"].ToString(); }
                //if (o["payment_method"].Contains("last4_digits") == true) { cmdInsCons.Parameters.Add("@NOM_CARTAO_4_ULTIMOS_DIGITOS", SqlDbType.VarChar).Value = o["payment_method"]["last4_digits"].ToString(); }
                //if (o["payment_method"].Contains("expiration_month") == true) { cmdInsCons.Parameters.Add("@NOM_CARTAO_MES", SqlDbType.VarChar).Value = o["payment_method"]["expiration_month"].ToString(); }
                //if (o["payment_method"].Contains("expiration_year") == true) { cmdInsCons.Parameters.Add("@NOM_CARTAO_ANO", SqlDbType.VarChar).Value = o["payment_method"]["expiration_year"].ToString(); }

                try
                {
                    cmdInsCons.Parameters.Add("@NOM_NUMERO_TRANSACAO", SqlDbType.VarChar).Value = o["transaction_number"].ToString();
                    cmdInsCons.Parameters.Add("@NOM_RECIBO", SqlDbType.VarChar).Value = o["sales_receipt"].ToString();
                    cmdInsCons.Parameters.Add("@NOM_AUTORIZACAO", SqlDbType.VarChar).Value = o["transaction_number"].ToString();
                }
                catch
                {
                }
                // Carregar os dados do Recibo //




                // Dados do Boleto

                //if (o["payment_method"].Contains("barcode") == true) { cmdInsCons.Parameters.Add("@NOM_CODIGO_BARRAS", SqlDbType.VarChar).Value = o["payment_method"]["barcode"].ToString(); }
                //if (o["payment_method"].Contains("url") == true) { cmdInsCons.Parameters.Add("@NOM_URL_BOLETO", SqlDbType.VarChar).Value = o["payment_method"]["url"].ToString(); }
                //if (o["payment_method"].Contains("document_number") == true) { cmdInsCons.Parameters.Add("@NOM_NUMERO_DOCUMENTO", SqlDbType.VarChar).Value = o["payment_method"]["document_number"].ToString(); }
                //if (o["payment_method"].Contains("expiration_date") == true) { cmdInsCons.Parameters.Add("@NOM_DATA_VENCIMENTO", SqlDbType.VarChar).Value = o["payment_method"]["expiration_date"].ToString(); }

                // Dados do PIX

                //if (o["payment_method"].Contains("qr_code") == true) { cmdInsCons.Parameters.Add("@NOM_QRCODE", SqlDbType.VarChar).Value = o["payment_method"]["qr_code"]["emv"].ToString(); }

                // -----------------------------------------------------------------------------------------------


                cmdInsCons.Parameters.Add("@DES_DESCRICAO", SqlDbType.Text).Value = o["payment_type"].ToString();

                if (o["history"].Count() > 0)
                {
                    for (int i = 0; i < o["history"].Count(); i++)
                    {
                        // Inserção de dados do Histórico
                        
                        SqlConnection connInsConsHistorico = new SqlConnection(sConexao);
                        connInsConsHistorico.Open();
                        SqlCommand cmdInsConsHistorico = new SqlCommand("dbo.stp_transacoes_historico_ins", connInsConsHistorico);
                        cmdInsConsHistorico.CommandType = CommandType.StoredProcedure;
                        cmdInsConsHistorico.Parameters.Add("@flg_operacao", SqlDbType.Char).Value = 'I';
                        cmdInsConsHistorico.Parameters.Add("@COD_ID_PESSOA_LICENCIADO", SqlDbType.Int).Value = licenciado;
                        cmdInsConsHistorico.Parameters.Add("@NOM_CODE", SqlDbType.VarChar).Value = o["id"].ToString();
                        cmdInsConsHistorico.Parameters.Add("@DTA_DATA", SqlDbType.DateTime).Value = DateTime.Now;
                        cmdInsConsHistorico.Parameters.Add("@NOM_TIPO", SqlDbType.VarChar).Value = o["history"][i]["operation_type"].ToString();
                        cmdInsConsHistorico.Parameters.Add("@NOM_STATUS", SqlDbType.VarChar).Value = o["history"][i]["status"].ToString();
                        cmdInsConsHistorico.Parameters.Add("@NUM_VALOR", SqlDbType.Float).Value = Funcoes.strToDouble(o["history"][i]["amount"].ToString());
                        try
                        {
                            cmdInsConsHistorico.Parameters.Add("@NOM_CODIGO_RESPOSTA", SqlDbType.VarChar).Value = o["history"][i]["response_code"].ToString();
                            cmdInsConsHistorico.Parameters.Add("@NOM_DESCRICAO_RESPOSTA", SqlDbType.VarChar).Value = o["history"][i]["response_message"].ToString();
                            cmdInsConsHistorico.Parameters.Add("@NOM_ID_AUTORIZACAO", SqlDbType.VarChar).Value = o["history"][i]["authorizer_id"].ToString();
                            cmdInsConsHistorico.Parameters.Add("@NOM_NSU_AUTORIZACAO", SqlDbType.VarChar).Value = o["history"][i]["authorization_nsu"].ToString();
                        }
                        catch
                        {
                        }
                        cmdInsConsHistorico.Parameters.Add("@DTA_DATA_CRIACAO", SqlDbType.DateTime).Value = Convert.ToDateTime(o["history"][i]["created_at"].ToString());
                        cmdInsConsHistorico.ExecuteNonQuery();
                        connInsConsHistorico.Close();
                        connInsConsHistorico.Dispose();
                    }
                }

                // Carregar os dados do recibo ----------------------------------------------------------

                try
                {
                    if (o["sales_receipt"].ToString().Trim() != "")
                    {
                        string jsonRecibo = zoop.DetalhesRecibo(o["sales_receipt"].ToString());

                        try
                        {
                            JObject oRecibo = JObject.Parse(jsonRecibo);

                            cmdInsCons.Parameters.Add("@NOM_VENDEDOR_RECIBO", SqlDbType.VarChar).Value = oRecibo["business_name"].ToString();
                            cmdInsCons.Parameters.Add("@NOM_ENDERECO_VENDEDOR_RECIBO", SqlDbType.VarChar).Value = oRecibo["business_address"]["line1"].ToString() + " " + oRecibo["business_address"]["line2"].ToString() + " " + oRecibo["business_address"]["state"].ToString() + " " + oRecibo["business_address"]["country_code"].ToString();
                            cmdInsCons.Parameters.Add("@NOM_DOCUMENTO_VENDEDOR_RECIBO", SqlDbType.VarChar).Value = oRecibo["taxpayer_id"].ToString();

                            cmdInsCons.Parameters.Add("@NOM_NUMERO_AUTORIZACAO_RECIBO", SqlDbType.VarChar).Value = oRecibo["auth_number"].ToString();
                            cmdInsCons.Parameters.Add("@NOM_NUMERO_RECIBO", SqlDbType.VarChar).Value = oRecibo["receipt_number"].ToString();
                            cmdInsCons.Parameters.Add("@NOM_NSU_AUTORIZACAO_RECIBO", SqlDbType.VarChar).Value = oRecibo["auth_nsu"].ToString();

                            cmdInsCons.Parameters.Add("@NOM_RECIBO_VENDA_ESTABELECIMENTO", SqlDbType.VarChar).Value = oRecibo["original_receipt"]["sales_receipt_merchant"].ToString();
                            cmdInsCons.Parameters.Add("@NOM_RECIBO_VENDA_CLIENTE", SqlDbType.VarChar).Value = oRecibo["original_receipt"]["sales_receipt_cardholder"].ToString();

                            cmdInsCons.Parameters.Add("@NOM_TERMINAL", SqlDbType.VarChar).Value = oRecibo["terminal"].ToString();

                            // Carrega Dados Terminal

                            if (oRecibo["terminal"].ToString().Trim() != "")
                            {
                                string jsonTerminal = zoop.DetalhesTerminal(oRecibo["terminal"].ToString());
                                try
                                {
                                    JObject oTerminal = JObject.Parse(jsonTerminal);

                                    cmdInsCons.Parameters.Add("@NOM_CODIGO_TERMINAL", SqlDbType.VarChar).Value = oTerminal["code"].ToString();
                                    cmdInsCons.Parameters.Add("@NOM_STATUS_TERMINAL", SqlDbType.VarChar).Value = oTerminal["status"].ToString();
                                    cmdInsCons.Parameters.Add("@NOM_NUMERO_SERIAL_TERMINAL", SqlDbType.VarChar).Value = oTerminal["serial_number"].ToString();
                                    cmdInsCons.Parameters.Add("@NUM_DEVICE_SERIAL", SqlDbType.VarChar).Value = oTerminal["serial_number"].ToString();
                                    cmdInsCons.Parameters.Add("@NOM_MODELO_TERMINAL", SqlDbType.VarChar).Value = oTerminal["terminal_model"].ToString();
                                    cmdInsCons.Parameters.Add("@NOM_FABRICANTE_TERMINAL", SqlDbType.VarChar).Value = oTerminal["manufacturer"].ToString();
                                }
                                catch
                                {
                                    cmdInsCons.Parameters.Add("@NOM_CODIGO_TERMINAL", SqlDbType.VarChar).Value = "";
                                    cmdInsCons.Parameters.Add("@NOM_STATUS_TERMINAL", SqlDbType.VarChar).Value = "";
                                    cmdInsCons.Parameters.Add("@NOM_NUMERO_SERIAL_TERMINAL", SqlDbType.VarChar).Value = "";
                                    cmdInsCons.Parameters.Add("@NOM_MODELO_TERMINAL", SqlDbType.VarChar).Value = "";
                                    cmdInsCons.Parameters.Add("@NOM_FABRICANTE_TERMINAL", SqlDbType.VarChar).Value = "";
                                }
                            }
                        }
                        catch
                        {

                        }

                    }
                }
                catch
                {
                }

                // Carregar os recebíveis ----------------------------------------------------------

                string jsonRecebiveis = zoop.DetalhesRecebiveis(o["id"].ToString());

                try
                {
                    JObject oRecebiveis = JObject.Parse(jsonRecebiveis);
                    if (oRecebiveis["items"].Count() > 0)
                    {
                        for (int i = 0; i < oRecebiveis["items"].Count(); i++)
                        {

                            // Inserção de dados do Recebiveis

                            SqlConnection connInsConsRecebiveis = new SqlConnection(sConexao);
                            connInsConsRecebiveis.Open();
                            SqlCommand cmdInsConsRecebiveis = new SqlCommand("dbo.stp_transacoes_recebiveis_ins", connInsConsRecebiveis);
                            cmdInsConsRecebiveis.CommandType = CommandType.StoredProcedure;
                            cmdInsConsRecebiveis.Parameters.Add("@flg_operacao", SqlDbType.Char).Value = 'I';
                            cmdInsConsRecebiveis.Parameters.Add("@COD_ID_PESSOA_LICENCIADO", SqlDbType.Int).Value = licenciado;
                            cmdInsConsRecebiveis.Parameters.Add("@NOM_CODE_TRANSACAO", SqlDbType.VarChar).Value = o["id"].ToString();
                            cmdInsConsRecebiveis.Parameters.Add("@NOM_CODE_RECEBIVEL", SqlDbType.VarChar).Value = oRecebiveis["items"][i]["id"].ToString();
                            cmdInsConsRecebiveis.Parameters.Add("@NOM_CODE_DESTINATARIO", SqlDbType.VarChar).Value = oRecebiveis["items"][i]["recipient"].ToString();

                            cmdInsConsRecebiveis.Parameters.Add("@DTA_DATA", SqlDbType.DateTime).Value = DateTime.Now;

                            cmdInsConsRecebiveis.Parameters.Add("@NOM_TIPO", SqlDbType.VarChar).Value = oRecebiveis["items"][i]["resource"].ToString();
                            cmdInsConsRecebiveis.Parameters.Add("@NOM_STATUS", SqlDbType.VarChar).Value = oRecebiveis["items"][i]["status"].ToString();

                            cmdInsConsRecebiveis.Parameters.Add("@NUM_VALOR", SqlDbType.Float).Value = Funcoes.strToDouble(oRecebiveis["items"][i]["amount"].ToString());
                            cmdInsConsRecebiveis.Parameters.Add("@NUM_VALOR_BRUTO", SqlDbType.Float).Value = Funcoes.strToDouble(oRecebiveis["items"][i]["gross_amount"].ToString());
                            cmdInsConsRecebiveis.Parameters.Add("@NUM_TAXA_ANTECIPACAO", SqlDbType.Float).Value = Funcoes.strToDouble(oRecebiveis["items"][i]["anticipation_fee"].ToString());

                            cmdInsConsRecebiveis.Parameters.Add("@NOM_CODIGO_AUTORIZACAO", SqlDbType.VarChar).Value = oRecebiveis["items"][i]["authorization_code"].ToString();

                            if ((oRecebiveis["items"][i]["created_at"].ToString().Trim() != "null") && (oRecebiveis["items"][i]["created_at"].ToString().Trim() != ""))
                            {
                                cmdInsConsRecebiveis.Parameters.Add("@DTA_DATA_CRIACAO", SqlDbType.DateTime).Value = Convert.ToDateTime(oRecebiveis["items"][i]["created_at"].ToString());
                            }
                            if ((oRecebiveis["items"][i]["paid_at"].ToString().Trim() != "null") && (oRecebiveis["items"][i]["paid_at"].ToString().Trim() != ""))
                            {
                                cmdInsConsRecebiveis.Parameters.Add("@DTA_DATA_PAGAMENTO", SqlDbType.DateTime).Value = Convert.ToDateTime(oRecebiveis["items"][i]["paid_at"].ToString());
                            }
                            if ((oRecebiveis["items"][i]["canceled_at"].ToString().Trim() != "null") && (oRecebiveis["items"][i]["canceled_at"].ToString().Trim() != ""))
                            {
                                cmdInsConsRecebiveis.Parameters.Add("@DTA_DATA_CANCELAMENTO", SqlDbType.DateTime).Value = Convert.ToDateTime(oRecebiveis["items"][i]["canceled_at"].ToString());
                            }
                            if ((oRecebiveis["items"][i]["expected_on"].ToString().Trim() != "null") && (oRecebiveis["items"][i]["expected_on"].ToString().Trim() != ""))
                            {
                                cmdInsConsRecebiveis.Parameters.Add("@DTA_DATA_PREVISTO", SqlDbType.DateTime).Value = Convert.ToDateTime(oRecebiveis["items"][i]["expected_on"].ToString());
                            }
                            cmdInsConsRecebiveis.ExecuteNonQuery();
                            connInsConsRecebiveis.Close();
                            connInsConsRecebiveis.Dispose();
                        }
                    }
                }
                catch
                {

                }
                // --------------------------------------------------------------------------------------


            }
            string itransacao = cmdInsCons.ExecuteScalar().ToString();
            connInsCons.Close();
            connInsCons.Dispose();

            return itransacao;
        //}
        //catch
        //{
        //    return false;
        //}
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

    public static string RetornaTipoPagamento(string sTipoPagamento)
    {
        string sRetorno = "";
        switch (sTipoPagamento)
        {
            case "credit":
                sRetorno = "Crédito";
                break;
            case "boleto":
                sRetorno = "Boleto";
                break;
            case "debit":
                sRetorno = "Débito";
                break;
            case "pix":
                sRetorno = "Pix";
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




    public static bool GravarVendas(transacoes.Venda MinhaVenda)
    {
        string sConexao = ConfigurationManager.AppSettings["user"].ToString() + ";" +
            ConfigurationManager.AppSettings["password"].ToString() + ";" +
            ConfigurationManager.AppSettings["datasource"].ToString() + ";" +
            ConfigurationManager.AppSettings["basecatalog"].ToString();

        // Inserir na tabela de vendas

        SqlConnection connInsCons = new SqlConnection(sConexao);
        connInsCons.Open();
        SqlCommand cmdInsCons = new SqlCommand("dbo.stp_vendas_ins", connInsCons);
        cmdInsCons.CommandType = CommandType.StoredProcedure;
        cmdInsCons.Parameters.Add("@flg_operacao", SqlDbType.Char).Value = 'I';

        cmdInsCons.Parameters.Add("@COD_ID_PESSOA_LICENCIADO", SqlDbType.Int).Value = MinhaVenda.CodIdPessoaLicenciado;
        cmdInsCons.Parameters.Add("@COD_ID_PESSOAS_FJ_VENDEDOR", SqlDbType.Int).Value = MinhaVenda.CodIdPessoasFjVendedor;

        cmdInsCons.Parameters.Add("@FLG_INTEGRACAO", SqlDbType.Char).Value = MinhaVenda.FlgIntegracao;

        cmdInsCons.Parameters.Add("@FLG_CLIENTE", SqlDbType.Char).Value = (MinhaVenda.CodIdComprador.ToString().Trim() !="") ? "S" : "N";
        cmdInsCons.Parameters.Add("@DES_JSON", SqlDbType.Text).Value = MinhaVenda.DesJson;

        if (MinhaVenda.CodIdComprador.ToString().Trim() != "")
        {
            // Pega os dados do cliente
            SqlConnection mySelCadastro = new SqlConnection(sConexao);
            mySelCadastro.Open();
            SqlCommand cmdSelCadastro = new SqlCommand("dbo.stp_compradores_ins", mySelCadastro);
            cmdSelCadastro.CommandType = CommandType.StoredProcedure;
            cmdSelCadastro.Parameters.Add("@flg_operacao", SqlDbType.Char).Value = "X";
            cmdSelCadastro.Parameters.Add("@COD_ID_COMPRADOR", SqlDbType.VarChar).Value = MinhaVenda.CodIdComprador;
            cmdSelCadastro.Parameters.Add("@FLG_INTEGRACAO", SqlDbType.Char).Value = MinhaVenda.FlgIntegracao;
            cmdSelCadastro.Parameters.Add("@COD_ID_PESSOA_LICENCIADO", SqlDbType.Int).Value = MinhaVenda.CodIdPessoaLicenciado;
            cmdSelCadastro.Parameters.Add("@COD_ID_PESSOAS_FJ", SqlDbType.Int).Value = MinhaVenda.CodIdPessoasFjVendedor;

            SqlDataReader ReaderCadastro = cmdSelCadastro.ExecuteReader();
            while (ReaderCadastro.Read())
            {
                cmdInsCons.Parameters.Add("@COD_ID_PESSOAS_FJ_COMPRADOR", SqlDbType.Int).Value = ReaderCadastro["COD_ID"].ToString();
                
                // Cliente
                cmdInsCons.Parameters.Add("@NOM_NOME", SqlDbType.VarChar).Value = ReaderCadastro["NOM_NOME"].ToString();
                cmdInsCons.Parameters.Add("@NOM_SOBRENOME", SqlDbType.VarChar).Value = ReaderCadastro["NOM_SOBRENOME"].ToString();
                cmdInsCons.Parameters.Add("@NUM_CNPJCPF", SqlDbType.VarChar).Value = ReaderCadastro["NUM_CNPJCPF"].ToString();
                cmdInsCons.Parameters.Add("@NOM_EMAIL", SqlDbType.VarChar).Value = ReaderCadastro["NOM_EMAIL"].ToString();
                cmdInsCons.Parameters.Add("@NOM_CELULAR", SqlDbType.VarChar).Value = ReaderCadastro["NOM_CELULAR"].ToString();

                // Endereço
                cmdInsCons.Parameters.Add("@NOM_ENDERECO", SqlDbType.VarChar).Value = ReaderCadastro["NOM_ENDERECO"].ToString();
                cmdInsCons.Parameters.Add("@NOM_NUMERO", SqlDbType.VarChar).Value = ReaderCadastro["NOM_NUMERO"].ToString();
                cmdInsCons.Parameters.Add("@NOM_COMPLEMENTO", SqlDbType.VarChar).Value = ReaderCadastro["NOM_COMPLEMENTO"].ToString();
                cmdInsCons.Parameters.Add("@NOM_BAIRRO", SqlDbType.VarChar).Value = ReaderCadastro["NOM_BAIRRO"].ToString();
                cmdInsCons.Parameters.Add("@NOM_CIDADE", SqlDbType.VarChar).Value = ReaderCadastro["NOM_CIDADE"].ToString();
                cmdInsCons.Parameters.Add("@NOM_UF", SqlDbType.VarChar).Value = ReaderCadastro["NOM_UF"].ToString();
                cmdInsCons.Parameters.Add("@NOM_CEP", SqlDbType.VarChar).Value = ReaderCadastro["NOM_CEP"].ToString();
                cmdInsCons.Parameters.Add("@NOM_PAIS", SqlDbType.VarChar).Value = "BR";
            }
        }
        
        cmdInsCons.Parameters.Add("@NOM_REFERENCIA_PRODUTO", SqlDbType.VarChar).Value = MinhaVenda.NomReferenciaProduto;
        cmdInsCons.Parameters.Add("@NOM_DESCRICAO_PRODUTO", SqlDbType.Text).Value = MinhaVenda.NomDescricaoProduto;

        cmdInsCons.Parameters.Add("@FLG_PRODUTO", SqlDbType.Char).Value = MinhaVenda.FlgProduto;
        cmdInsCons.Parameters.Add("@FLG_EXIBIR_PRODUTOS", SqlDbType.Char).Value = MinhaVenda.FlgExibirProdutos;

        cmdInsCons.Parameters.Add("@FLG_LINK_PERMANENTE", SqlDbType.Char).Value = MinhaVenda.FlgLinkPermanente;

        cmdInsCons.Parameters.Add("@FLG_BOLETO", SqlDbType.Char).Value = MinhaVenda.FlgBoleto;
        cmdInsCons.Parameters.Add("@FLG_PIX", SqlDbType.Char).Value = MinhaVenda.FlgPix;
        cmdInsCons.Parameters.Add("@FLG_CARTAO_CREDITO", SqlDbType.Char).Value = MinhaVenda.FlgCartaoCredito;

        cmdInsCons.Parameters.Add("@FLG_TIPO_COBRANCA", SqlDbType.Char).Value = "U";

        cmdInsCons.Parameters.Add("@FLG_TIPO_OPERACAO", SqlDbType.Char).Value = "C";
        cmdInsCons.Parameters.Add("@FLG_OPERADORA", SqlDbType.Char).Value = MinhaVenda.FlgIntegracao;

        cmdInsCons.Parameters.Add("@FLG_PRECO_PARCELAMENTO", SqlDbType.Char).Value = MinhaVenda.FlgPrecoParcelamento;
        cmdInsCons.Parameters.Add("@FLG_SPLIT", SqlDbType.Char).Value = MinhaVenda.FlgSplit;

        cmdInsCons.Parameters.Add("@NOM_EMAIL_LINK", SqlDbType.VarChar).Value = MinhaVenda.NomEmailLink;
        cmdInsCons.Parameters.Add("@NOM_CAMPO_01", SqlDbType.VarChar).Value = MinhaVenda.NomCampo01;

        cmdInsCons.Parameters.Add("@NOM_IMAGEM", SqlDbType.VarChar).Value = MinhaVenda.NomImagem;

        //cmdInsCons.Parameters.Add("@COD_ID_PRODUTO", SqlDbType.Int).Value = Funcoes.strToInt(ddlProduto.SelectedValue.ToString());

        cmdInsCons.Parameters.Add("@NUM_VALOR", SqlDbType.Float).Value = MinhaVenda.NumValor;
        cmdInsCons.Parameters.Add("@NUM_PARCELAS", SqlDbType.Int).Value = MinhaVenda.NumParcelas;

        cmdInsCons.Parameters.Add("@FLG_TIPO", SqlDbType.Char).Value = MinhaVenda.FlgTipo;

        if (MinhaVenda.DtaPixVencimento.ToString().Trim() != "") { cmdInsCons.Parameters.Add("@DTA_PIX_VENCIMENTO", SqlDbType.DateTime).Value = MinhaVenda.DtaPixVencimento; }
        if (MinhaVenda.DtaVencimento.ToString().Trim() != "") { cmdInsCons.Parameters.Add("@DTA_VENCIMENTO", SqlDbType.DateTime).Value = MinhaVenda.DtaVencimento; }

        cmdInsCons.Parameters.Add("@NOM_ORIGEM", SqlDbType.VarChar).Value = MinhaVenda.NomOrigem; 

        cmdInsCons.Parameters.Add("@NOM_BOLETO", SqlDbType.VarChar).Value = MinhaVenda.NomBoleto; 
        cmdInsCons.Parameters.Add("@NOM_BARCODE_BOLETO", SqlDbType.VarChar).Value = MinhaVenda.NomBarcodeBoleto; 
        cmdInsCons.Parameters.Add("@NOM_CODE", SqlDbType.VarChar).Value = MinhaVenda.NomCode; 
        string iVenda = cmdInsCons.ExecuteScalar().ToString();
        connInsCons.Close();
        connInsCons.Dispose();

        if (MinhaVenda.NomCode.ToString().Trim() != "")
        {
            GravarVendasDetalhe(MinhaVenda, iVenda);
        }

        return true;
    }

    public static bool GravarVendasDetalhe(transacoes.Venda MinhaVenda, string idvenda)
    {
        string sConexao = ConfigurationManager.AppSettings["user"].ToString() + ";" +
            ConfigurationManager.AppSettings["password"].ToString() + ";" +
            ConfigurationManager.AppSettings["datasource"].ToString() + ";" +
            ConfigurationManager.AppSettings["basecatalog"].ToString();

        
        SqlConnection connInsCons = new SqlConnection(sConexao);
        connInsCons.Open();
        SqlCommand cmdInsCons = new SqlCommand("dbo.stp_vendas_vendas_ins", connInsCons);
        cmdInsCons.CommandType = CommandType.StoredProcedure;
        cmdInsCons.Parameters.Add("@flg_operacao", SqlDbType.Char).Value = 'I';
        cmdInsCons.Parameters.Add("@COD_ID_PESSOA_LICENCIADO", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["LICENCIADO"].ToString());
        cmdInsCons.Parameters.Add("@COD_ID_PESSOAS_FJ_VENDEDOR", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["PESSOA"].ToString());
        cmdInsCons.Parameters.Add("@COD_ID_VENDAS", SqlDbType.Int).Value = idvenda;

        cmdInsCons.Parameters.Add("@FLG_INTEGRACAO", SqlDbType.Char).Value = MinhaVenda.FlgIntegracao;

        cmdInsCons.Parameters.Add("@NUM_VALOR", SqlDbType.Float).Value = MinhaVenda.NumValor;
        cmdInsCons.Parameters.Add("@DTA_VENCIMENTO", SqlDbType.DateTime).Value = (MinhaVenda.DtaVencimento.ToString().Trim()!="")? MinhaVenda.DtaVencimento : null;


        cmdInsCons.Parameters.Add("@NUM_CARTAO", SqlDbType.VarChar).Value = MinhaVenda.NumCartao;
        cmdInsCons.Parameters.Add("@NOM_CARTAO", SqlDbType.VarChar).Value = MinhaVenda.NomCartao;
        cmdInsCons.Parameters.Add("@NUM_MES_CARTAO", SqlDbType.Int).Value = MinhaVenda.NumMesCartao;
        cmdInsCons.Parameters.Add("@NUM_ANO_CARTAO", SqlDbType.Int).Value = MinhaVenda.NumAnoCartao;
        cmdInsCons.Parameters.Add("@NUM_CODIGO_CARTAO", SqlDbType.Int).Value = MinhaVenda.NumCodigoCartao;

        cmdInsCons.Parameters.Add("@NUM_PARCELAS", SqlDbType.Int).Value = MinhaVenda.NumParcelas;

        cmdInsCons.Parameters.Add("@FLG_TIPO", SqlDbType.Char).Value = MinhaVenda.FlgTipo;
        cmdInsCons.Parameters.Add("@FLG_BOLETO", SqlDbType.Char).Value = MinhaVenda.FlgBoleto;
        cmdInsCons.Parameters.Add("@FLG_PIX", SqlDbType.Char).Value = MinhaVenda.FlgPix;
        cmdInsCons.Parameters.Add("@FLG_CARTAO_CREDITO", SqlDbType.Char).Value = MinhaVenda.FlgCartaoCredito;


        cmdInsCons.Parameters.Add("@NOM_REFERENCIA_PRODUTO", SqlDbType.VarChar).Value = MinhaVenda.NomReferenciaProduto;
        cmdInsCons.Parameters.Add("@NOM_DESCRICAO_PRODUTO", SqlDbType.Text).Value = MinhaVenda.NomDescricaoProduto;

        cmdInsCons.Parameters.Add("@FLG_PRODUTO", SqlDbType.Char).Value = MinhaVenda.FlgProduto;
        cmdInsCons.Parameters.Add("@FLG_EXIBIR_PRODUTOS", SqlDbType.Char).Value = MinhaVenda.FlgExibirProdutos;

        cmdInsCons.Parameters.Add("@FLG_LINK_PERMANENTE", SqlDbType.Char).Value = MinhaVenda.FlgLinkPermanente;

        cmdInsCons.Parameters.Add("@FLG_TIPO_COBRANCA", SqlDbType.Char).Value = "U";

        cmdInsCons.Parameters.Add("@FLG_TIPO_OPERACAO", SqlDbType.Char).Value = "C";
        cmdInsCons.Parameters.Add("@FLG_OPERADORA", SqlDbType.Char).Value = MinhaVenda.FlgIntegracao;

        cmdInsCons.Parameters.Add("@FLG_PRECO_PARCELAMENTO", SqlDbType.Char).Value = MinhaVenda.FlgPrecoParcelamento;
        cmdInsCons.Parameters.Add("@FLG_SPLIT", SqlDbType.Char).Value = MinhaVenda.FlgSplit;

        cmdInsCons.Parameters.Add("@NOM_CODE", SqlDbType.VarChar).Value = MinhaVenda.NomCode;
        cmdInsCons.Parameters.Add("@NOM_EMAIL_LINK", SqlDbType.VarChar).Value = MinhaVenda.NomEmailLink;
        cmdInsCons.Parameters.Add("@NOM_CAMPO_01", SqlDbType.VarChar).Value = MinhaVenda.NomCampo01;
        cmdInsCons.Parameters.Add("@NOM_CONTEUDO_CAMPO_01", SqlDbType.VarChar).Value = MinhaVenda.NomConteudoCampo01;

        if (MinhaVenda.CodIdComprador.ToString().Trim() != "")
        {
            // Pega os dados do cliente
            SqlConnection mySelCadastro = new SqlConnection(sConexao);
            mySelCadastro.Open();
            SqlCommand cmdSelCadastro = new SqlCommand("dbo.stp_compradores_ins", mySelCadastro);
            cmdSelCadastro.CommandType = CommandType.StoredProcedure;
            cmdSelCadastro.Parameters.Add("@flg_operacao", SqlDbType.Char).Value = "X";
            cmdSelCadastro.Parameters.Add("@COD_ID_COMPRADOR", SqlDbType.VarChar).Value = MinhaVenda.CodIdComprador;
            cmdSelCadastro.Parameters.Add("@FLG_INTEGRACAO", SqlDbType.Char).Value = MinhaVenda.FlgIntegracao;
            cmdSelCadastro.Parameters.Add("@COD_ID_PESSOA_LICENCIADO", SqlDbType.Int).Value = MinhaVenda.CodIdPessoaLicenciado;
            cmdSelCadastro.Parameters.Add("@COD_ID_PESSOAS_FJ", SqlDbType.Int).Value = MinhaVenda.CodIdPessoasFjVendedor;
            SqlDataReader ReaderCadastro = cmdSelCadastro.ExecuteReader();
            while (ReaderCadastro.Read())
            {
                cmdInsCons.Parameters.Add("@COD_ID_PESSOAS_FJ_COMPRADOR", SqlDbType.Int).Value = ReaderCadastro["COD_ID"].ToString();

                // Cliente
                cmdInsCons.Parameters.Add("@NOM_NOME", SqlDbType.VarChar).Value = ReaderCadastro["NOM_NOME"].ToString();
                cmdInsCons.Parameters.Add("@NOM_SOBRENOME", SqlDbType.VarChar).Value = ReaderCadastro["NOM_SOBRENOME"].ToString();
                cmdInsCons.Parameters.Add("@NUM_CNPJCPF", SqlDbType.VarChar).Value = ReaderCadastro["NUM_CNPJCPF"].ToString();
                cmdInsCons.Parameters.Add("@NOM_EMAIL", SqlDbType.VarChar).Value = ReaderCadastro["NOM_EMAIL"].ToString();
                cmdInsCons.Parameters.Add("@NOM_CELULAR", SqlDbType.VarChar).Value = ReaderCadastro["NOM_CELULAR"].ToString();

                // Endereço
                cmdInsCons.Parameters.Add("@NOM_ENDERECO", SqlDbType.VarChar).Value = ReaderCadastro["NOM_ENDERECO"].ToString();
                cmdInsCons.Parameters.Add("@NOM_NUMERO", SqlDbType.VarChar).Value = ReaderCadastro["NOM_NUMERO"].ToString();
                cmdInsCons.Parameters.Add("@NOM_COMPLEMENTO", SqlDbType.VarChar).Value = ReaderCadastro["NOM_COMPLEMENTO"].ToString();
                cmdInsCons.Parameters.Add("@NOM_BAIRRO", SqlDbType.VarChar).Value = ReaderCadastro["NOM_BAIRRO"].ToString();
                cmdInsCons.Parameters.Add("@NOM_CIDADE", SqlDbType.VarChar).Value = ReaderCadastro["NOM_CIDADE"].ToString();
                cmdInsCons.Parameters.Add("@NOM_UF", SqlDbType.VarChar).Value = ReaderCadastro["NOM_UF"].ToString();
                cmdInsCons.Parameters.Add("@NOM_CEP", SqlDbType.VarChar).Value = ReaderCadastro["NOM_CEP"].ToString();
                cmdInsCons.Parameters.Add("@NOM_PAIS", SqlDbType.VarChar).Value = "BR";

                if (MinhaVenda.FlgEnderecoEntrega.ToString().Trim() == "N")
                {
                    cmdInsCons.Parameters.Add("@NOM_ENDERECO_ENTREGA", SqlDbType.VarChar).Value = ReaderCadastro["NOM_ENDERECO"].ToString();
                    cmdInsCons.Parameters.Add("@NOM_NUMERO_ENTREGA", SqlDbType.VarChar).Value = ReaderCadastro["NOM_NUMERO"].ToString();
                    cmdInsCons.Parameters.Add("@NOM_COMPLEMENTO_ENTREGA", SqlDbType.VarChar).Value = ReaderCadastro["NOM_COMPLEMENTO"].ToString();
                    cmdInsCons.Parameters.Add("@NOM_BAIRRO_ENTREGA", SqlDbType.VarChar).Value = ReaderCadastro["NOM_BAIRRO"].ToString();
                    cmdInsCons.Parameters.Add("@NOM_CIDADE_ENTREGA", SqlDbType.VarChar).Value = ReaderCadastro["NOM_CIDADE"].ToString();
                    cmdInsCons.Parameters.Add("@NOM_UF_ENTREGA", SqlDbType.VarChar).Value = ReaderCadastro["NOM_UF"].ToString();
                    cmdInsCons.Parameters.Add("@NOM_CEP_ENTREGA", SqlDbType.VarChar).Value = ReaderCadastro["NOM_CEP"].ToString();
                }
            
            }
        }

        
        // Endereço Entrega
        cmdInsCons.Parameters.Add("@FLG_ENDERECO_ENTREGA", SqlDbType.Char).Value = MinhaVenda.FlgEnderecoEntrega;

        if (MinhaVenda.FlgEnderecoEntrega.ToString().Trim() == "S")
        {
            cmdInsCons.Parameters.Add("@NOM_ENDERECO_ENTREGA", SqlDbType.VarChar).Value = MinhaVenda.NomEnderecoEntrega;
            cmdInsCons.Parameters.Add("@NOM_NUMERO_ENTREGA", SqlDbType.VarChar).Value = MinhaVenda.NomNumeroEntrega;
            cmdInsCons.Parameters.Add("@NOM_COMPLEMENTO_ENTREGA", SqlDbType.VarChar).Value = MinhaVenda.NomComplementoEntrega;
            cmdInsCons.Parameters.Add("@NOM_BAIRRO_ENTREGA", SqlDbType.VarChar).Value = MinhaVenda.NomBairroEntrega;
            cmdInsCons.Parameters.Add("@NOM_CIDADE_ENTREGA", SqlDbType.VarChar).Value = MinhaVenda.NomCidadeEntrega;
            cmdInsCons.Parameters.Add("@NOM_UF_ENTREGA", SqlDbType.VarChar).Value = MinhaVenda.NomUfEntrega;
            cmdInsCons.Parameters.Add("@NOM_CEP_ENTREGA", SqlDbType.VarChar).Value = MinhaVenda.NomCepEntrega;
        }

        
        cmdInsCons.Parameters.Add("@DES_JSON", SqlDbType.Text).Value = MinhaVenda.DesJson;

        cmdInsCons.Parameters.Add("@NOM_BOLETO", SqlDbType.VarChar).Value = MinhaVenda.NomBoleto;
        cmdInsCons.Parameters.Add("@NOM_BARCODE_BOLETO", SqlDbType.VarChar).Value = MinhaVenda.NomBarcodeBoleto;
        cmdInsCons.Parameters.Add("@NOM_PIX", SqlDbType.VarChar).Value = MinhaVenda.NomPix;

        string URL = MinhaVenda.NomPix;
        if (URL.ToString().Trim() != "")
        {
            QRCodeEncoder encoder = new QRCodeEncoder();
            Bitmap image = encoder.Encode(URL);
            string Codificacao = DateTime.Now.Year.ToString() + DateTime.Now.Month.ToString() + DateTime.Now.Day.ToString() + DateTime.Now.Hour.ToString() + DateTime.Now.Minute.ToString() + DateTime.Now.Second.ToString();
            image.Save(HttpContext.Current.Server.MapPath("public_html") + "\\" + HttpContext.Current.Session["LICENCIADO"].ToString() + "_" + Codificacao.ToString() + "_" + idvenda.ToString().PadLeft(10, '0') + ".bmp");
            cmdInsCons.Parameters.Add("@NOM_IMAGEM_PIX", SqlDbType.VarChar).Value = HttpContext.Current.Session["LICENCIADO"].ToString() + "_" + Codificacao.ToString() + "_" + idvenda.ToString().PadLeft(10, '0') + ".bmp";
        }
        cmdInsCons.ExecuteNonQuery();
        connInsCons.Close();
        connInsCons.Dispose();


        return true;
    }

    public static bool EnviarLinkBoletoEmail(string sURLBoleto, string sLinhaDigitavel, string nome, string descricao, string vencimento, string valor, string email)
    {
        System.Net.Mail.SmtpClient client = new System.Net.Mail.SmtpClient();
        client.Host = HttpContext.Current.Session["EMAILHOST"].ToString();
        if (HttpContext.Current.Session["EMAILSSL"].ToString() == "S")
        { client.EnableSsl = true; }
        else { client.EnableSsl = false; }
        client.Port = Funcoes.strToInt(HttpContext.Current.Session["EMAILPORTA"].ToString());
        //client.Timeout = 0;
        client.Credentials = new System.Net.NetworkCredential(HttpContext.Current.Session["EMAIL"].ToString(), HttpContext.Current.Session["EMAILSENHA"].ToString());
        MailMessage mail = new MailMessage();
        mail.Sender = new System.Net.Mail.MailAddress(HttpContext.Current.Session["EMAIL"].ToString(), "");
        mail.From = new MailAddress(HttpContext.Current.Session["EMAIL"].ToString(), "");

        mail.To.Add(new MailAddress(email, email));
        mail.Bcc.Add(new MailAddress("adriano@webview.com.br", "Adriano - WEBVIEW"));
        mail.Bcc.Add(new MailAddress("admin@legacybank.com.br", "Administrador LEGACY"));

        mail.Subject = "Link de pagamento - " + HttpContext.Current.Session["URLORIGEM"].ToString();
        mail.SubjectEncoding = System.Text.Encoding.UTF8;

        // Construção do CORPO da MENSAGEM (Body)
        mail.Body = "";
        mail.Body = mail.Body + "<img src='https://" + HttpContext.Current.Session["URLORIGEM"].ToString() + "/public_html/" + HttpContext.Current.Session["EMAILLOGOTIPO"].ToString() + "' alt='Logotipo' width='150'><br /><br>";
        mail.Body = mail.Body + "<b>Olá,</b> " + nome + "<br /><br />";
        mail.Body = mail.Body + "Segue abaixo o link de pagamento referente:<br /><br />";
        mail.Body = mail.Body + "Descrição: " + descricao + "<br />";
        mail.Body = mail.Body + "Vencimento: " + Convert.ToDateTime(vencimento).ToShortDateString() + "<br />";
        mail.Body = mail.Body + "Linha Digitável: " + sLinhaDigitavel + "<br />";

        mail.Body = mail.Body + "Valor: " + String.Format("{0:c2}", Funcoes.strToDouble(valor)) + "<br /><br />";
        mail.Body = mail.Body + "<b>Para visualizar o link de pagamento basta clicar no link abaixo<br /><br>";
        mail.Body = mail.Body + "<a href='" + sURLBoleto.ToString() + "' target='_blank'>Clique aqui para visualizar o link de pagamento</a><br /><br />";
        mail.Body = mail.Body + HttpContext.Current.Session["EMAILRODAPE"].ToString();

        mail.BodyEncoding = System.Text.Encoding.UTF8;
        mail.IsBodyHtml = true;
        mail.Priority = MailPriority.High;

        try
        {
            client.Send(mail);
            mail = null;
            return true;
        }
        catch (System.Exception erro)
        {
            return false;
        }

    }

    public static bool EnviarLinkQRCodeEmail(string sQRCode, string sQRCodeImagem, string nome, string descricao, string vencimento, string valor, string email)
    {
        System.Net.Mail.SmtpClient client = new System.Net.Mail.SmtpClient();
        client.Host = HttpContext.Current.Session["EMAILHOST"].ToString();
        if (HttpContext.Current.Session["EMAILSSL"].ToString() == "S")
        { client.EnableSsl = true; }
        else { client.EnableSsl = false; }
        client.Port = Funcoes.strToInt(HttpContext.Current.Session["EMAILPORTA"].ToString());
        //client.Timeout = 0;
        client.Credentials = new System.Net.NetworkCredential(HttpContext.Current.Session["EMAIL"].ToString(), HttpContext.Current.Session["EMAILSENHA"].ToString());
        MailMessage mail = new MailMessage();
        mail.Sender = new System.Net.Mail.MailAddress(HttpContext.Current.Session["EMAIL"].ToString(), "");
        mail.From = new MailAddress(HttpContext.Current.Session["EMAIL"].ToString(), "");

        mail.To.Add(new MailAddress(email, email));
        mail.Bcc.Add(new MailAddress("adriano@webview.com.br", "Adriano - WEBVIEW"));
        mail.Bcc.Add(new MailAddress("admin@legacybank.com.br", "Administrador LEGACY"));

        mail.Subject = "Link de pagamento - " + HttpContext.Current.Session["URLORIGEM"].ToString();
        mail.SubjectEncoding = System.Text.Encoding.UTF8;

        // Construção do CORPO da MENSAGEM (Body)
        mail.Body = "";
        mail.Body = mail.Body + "<img src='https://" + HttpContext.Current.Session["URLORIGEM"].ToString() + "/public_html/" + HttpContext.Current.Session["EMAILLOGOTIPO"].ToString() + "' alt='Logotipo' width='150'><br /><br>";
        mail.Body = mail.Body + "<b>Olá,</b> " + nome + "<br /><br />";
        mail.Body = mail.Body + "Segue abaixo o link de pagamento referente:<br /><br />";
        mail.Body = mail.Body + "Descrição: " + descricao + "<br />";
        mail.Body = mail.Body + "Vencimento: " + Convert.ToDateTime(vencimento).ToShortDateString() + "<br />";
        mail.Body = mail.Body + "Valor: " + String.Format("{0:c2}", Funcoes.strToDouble(valor)) + "<br /><br /><br / >";

        mail.Body = mail.Body + "QR Code Copia e Cola: " + sQRCode + "<br /><br />";
        mail.Body = mail.Body + "<img src='https://" + HttpContext.Current.Session["URLORIGEM"].ToString() + "/public_html/" + sQRCodeImagem + "' alt='Logotipo' width='200'><br /><br>";
        
        mail.Body = mail.Body + HttpContext.Current.Session["EMAILRODAPE"].ToString();

        mail.BodyEncoding = System.Text.Encoding.UTF8;
        mail.IsBodyHtml = true;
        mail.Priority = MailPriority.High;

        try
        {
            client.Send(mail);
            mail = null;
            return true;
        }
        catch (System.Exception erro)
        {
            return false;
        }

    }


    public static string GravarVendasDiversas(transacoes.Venda MinhaVenda)
    {
        string sConexao = ConfigurationManager.AppSettings["user"].ToString() + ";" +
            ConfigurationManager.AppSettings["password"].ToString() + ";" +
            ConfigurationManager.AppSettings["datasource"].ToString() + ";" +
            ConfigurationManager.AppSettings["basecatalog"].ToString();

        // Inserir na tabela de vendas

        SqlConnection connInsCons = new SqlConnection(sConexao);
        connInsCons.Open();
        SqlCommand cmdInsCons = new SqlCommand("dbo.stp_vendas_ins", connInsCons);
        cmdInsCons.CommandType = CommandType.StoredProcedure;
        cmdInsCons.Parameters.Add("@flg_operacao", SqlDbType.Char).Value = 'I';

        cmdInsCons.Parameters.Add("@COD_ID_PESSOA_LICENCIADO", SqlDbType.Int).Value = MinhaVenda.CodIdPessoaLicenciado;
        cmdInsCons.Parameters.Add("@COD_ID_PESSOAS_FJ_VENDEDOR", SqlDbType.Int).Value = MinhaVenda.CodIdPessoasFjVendedor;

        cmdInsCons.Parameters.Add("@FLG_INTEGRACAO", SqlDbType.Char).Value = MinhaVenda.FlgIntegracao;

        cmdInsCons.Parameters.Add("@FLG_CLIENTE", SqlDbType.Char).Value = (MinhaVenda.CodIdComprador.ToString().Trim() != "") ? "S" : "N";
        cmdInsCons.Parameters.Add("@DES_JSON", SqlDbType.Text).Value = MinhaVenda.DesJson;

        if (MinhaVenda.CodIdComprador.ToString().Trim() != "")
        {
            // Pega os dados do cliente
            SqlConnection mySelCadastro = new SqlConnection(sConexao);
            mySelCadastro.Open();
            SqlCommand cmdSelCadastro = new SqlCommand("dbo.stp_compradores_ins", mySelCadastro);
            cmdSelCadastro.CommandType = CommandType.StoredProcedure;
            cmdSelCadastro.Parameters.Add("@flg_operacao", SqlDbType.Char).Value = "X";
            cmdSelCadastro.Parameters.Add("@COD_ID_COMPRADOR", SqlDbType.VarChar).Value = MinhaVenda.CodIdComprador;
            cmdSelCadastro.Parameters.Add("@FLG_INTEGRACAO", SqlDbType.Char).Value = MinhaVenda.FlgIntegracao;
            cmdSelCadastro.Parameters.Add("@COD_ID_PESSOA_LICENCIADO", SqlDbType.Int).Value = MinhaVenda.CodIdPessoaLicenciado;
            cmdSelCadastro.Parameters.Add("@COD_ID_PESSOAS_FJ", SqlDbType.Int).Value = MinhaVenda.CodIdPessoasFjVendedor;

            SqlDataReader ReaderCadastro = cmdSelCadastro.ExecuteReader();
            while (ReaderCadastro.Read())
            {
                cmdInsCons.Parameters.Add("@COD_ID_PESSOAS_FJ_COMPRADOR", SqlDbType.Int).Value = ReaderCadastro["COD_ID"].ToString();

                // Cliente
                cmdInsCons.Parameters.Add("@NOM_NOME", SqlDbType.VarChar).Value = ReaderCadastro["NOM_NOME"].ToString();
                cmdInsCons.Parameters.Add("@NOM_SOBRENOME", SqlDbType.VarChar).Value = ReaderCadastro["NOM_SOBRENOME"].ToString();
                cmdInsCons.Parameters.Add("@NUM_CNPJCPF", SqlDbType.VarChar).Value = ReaderCadastro["NUM_CNPJCPF"].ToString();
                cmdInsCons.Parameters.Add("@NOM_EMAIL", SqlDbType.VarChar).Value = ReaderCadastro["NOM_EMAIL"].ToString();
                cmdInsCons.Parameters.Add("@NOM_CELULAR", SqlDbType.VarChar).Value = ReaderCadastro["NOM_CELULAR"].ToString();

                // Endereço
                cmdInsCons.Parameters.Add("@NOM_ENDERECO", SqlDbType.VarChar).Value = ReaderCadastro["NOM_ENDERECO"].ToString();
                cmdInsCons.Parameters.Add("@NOM_NUMERO", SqlDbType.VarChar).Value = ReaderCadastro["NOM_NUMERO"].ToString();
                cmdInsCons.Parameters.Add("@NOM_COMPLEMENTO", SqlDbType.VarChar).Value = ReaderCadastro["NOM_COMPLEMENTO"].ToString();
                cmdInsCons.Parameters.Add("@NOM_BAIRRO", SqlDbType.VarChar).Value = ReaderCadastro["NOM_BAIRRO"].ToString();
                cmdInsCons.Parameters.Add("@NOM_CIDADE", SqlDbType.VarChar).Value = ReaderCadastro["NOM_CIDADE"].ToString();
                cmdInsCons.Parameters.Add("@NOM_UF", SqlDbType.VarChar).Value = ReaderCadastro["NOM_UF"].ToString();
                cmdInsCons.Parameters.Add("@NOM_CEP", SqlDbType.VarChar).Value = ReaderCadastro["NOM_CEP"].ToString();
                cmdInsCons.Parameters.Add("@NOM_PAIS", SqlDbType.VarChar).Value = "BR";
            }
        }

        cmdInsCons.Parameters.Add("@NOM_REFERENCIA_PRODUTO", SqlDbType.VarChar).Value = MinhaVenda.NomReferenciaProduto;
        cmdInsCons.Parameters.Add("@NOM_DESCRICAO_PRODUTO", SqlDbType.Text).Value = MinhaVenda.NomDescricaoProduto;

        cmdInsCons.Parameters.Add("@FLG_PRODUTO", SqlDbType.Char).Value = MinhaVenda.FlgProduto;
        cmdInsCons.Parameters.Add("@FLG_EXIBIR_PRODUTOS", SqlDbType.Char).Value = MinhaVenda.FlgExibirProdutos;

        cmdInsCons.Parameters.Add("@FLG_LINK_PERMANENTE", SqlDbType.Char).Value = MinhaVenda.FlgLinkPermanente;

        cmdInsCons.Parameters.Add("@FLG_BOLETO", SqlDbType.Char).Value = MinhaVenda.FlgBoleto;
        cmdInsCons.Parameters.Add("@FLG_PIX", SqlDbType.Char).Value = MinhaVenda.FlgPix;
        cmdInsCons.Parameters.Add("@FLG_CARTAO_CREDITO", SqlDbType.Char).Value = MinhaVenda.FlgCartaoCredito;

        cmdInsCons.Parameters.Add("@FLG_TIPO_COBRANCA", SqlDbType.Char).Value = "U";

        cmdInsCons.Parameters.Add("@FLG_TIPO_OPERACAO", SqlDbType.Char).Value = "C";
        cmdInsCons.Parameters.Add("@FLG_OPERADORA", SqlDbType.Char).Value = MinhaVenda.FlgIntegracao;

        cmdInsCons.Parameters.Add("@FLG_PRECO_PARCELAMENTO", SqlDbType.Char).Value = MinhaVenda.FlgPrecoParcelamento;
        cmdInsCons.Parameters.Add("@FLG_SPLIT", SqlDbType.Char).Value = MinhaVenda.FlgSplit;

        cmdInsCons.Parameters.Add("@NOM_EMAIL_LINK", SqlDbType.VarChar).Value = MinhaVenda.NomEmailLink;
        cmdInsCons.Parameters.Add("@NOM_CAMPO_01", SqlDbType.VarChar).Value = MinhaVenda.NomCampo01;

        cmdInsCons.Parameters.Add("@NOM_IMAGEM", SqlDbType.VarChar).Value = MinhaVenda.NomImagem;

        //cmdInsCons.Parameters.Add("@COD_ID_PRODUTO", SqlDbType.Int).Value = Funcoes.strToInt(ddlProduto.SelectedValue.ToString());

        cmdInsCons.Parameters.Add("@NUM_VALOR", SqlDbType.Float).Value = MinhaVenda.NumValor;
        cmdInsCons.Parameters.Add("@NUM_PARCELAS", SqlDbType.Int).Value = MinhaVenda.NumParcelas;

        cmdInsCons.Parameters.Add("@FLG_TIPO", SqlDbType.Char).Value = MinhaVenda.FlgTipo;

        if (MinhaVenda.DtaPixVencimento.ToString().Trim() != "") { cmdInsCons.Parameters.Add("@DTA_PIX_VENCIMENTO", SqlDbType.DateTime).Value = MinhaVenda.DtaPixVencimento; }
        if (MinhaVenda.DtaVencimento.ToString().Trim() != "") { cmdInsCons.Parameters.Add("@DTA_VENCIMENTO", SqlDbType.DateTime).Value = MinhaVenda.DtaVencimento; }

        cmdInsCons.Parameters.Add("@NOM_ORIGEM", SqlDbType.VarChar).Value = MinhaVenda.NomOrigem;

        cmdInsCons.Parameters.Add("@NOM_BOLETO", SqlDbType.VarChar).Value = MinhaVenda.NomBoleto;
        cmdInsCons.Parameters.Add("@NOM_BARCODE_BOLETO", SqlDbType.VarChar).Value = MinhaVenda.NomBarcodeBoleto;
        cmdInsCons.Parameters.Add("@NOM_CODE", SqlDbType.VarChar).Value = MinhaVenda.NomCode;
        string iVenda = cmdInsCons.ExecuteScalar().ToString();
        connInsCons.Close();
        connInsCons.Dispose();

        if (MinhaVenda.NomCode.ToString().Trim() != "")
        {
            GravarVendasDetalhe(MinhaVenda, iVenda);
        }

        return iVenda.ToString();
    }


}