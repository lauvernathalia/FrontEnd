using System;
using System.Data;
using System.Data.OleDb;
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
using MessagingToolkit.QRCode.Codec;
using MessagingToolkit.QRCode.Codec.Data;

using System.Drawing;

public partial class con_conciliacao : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            //Vamos considerar que a data seja o dia de hoje, mas pode ser qualquer data.
            DateTime data = DateTime.Today;
            //DateTime com o primeiro dia do mês
            DateTime primeiroDiaDoMes = new DateTime(data.Year, data.Month, 1);
            //DateTime com o último dia do mês
            DateTime ultimoDiaDoMes = new DateTime(data.Year, data.Month, DateTime.DaysInMonth(data.Year, data.Month));
            //txtDataIni.Text = primeiroDiaDoMes.ToShortDateString();
            //txtDataFim.Text = ultimoDiaDoMes.ToShortDateString();

            txtDataIni.Text = DateTime.Now.ToShortDateString();
            txtDataFim.Text = DateTime.Now.ToShortDateString();

            ConsultaGeral();

        }

    }

    private void ConsultaGeral()
    {

        SqlConnection myConsulta = new SqlConnection(Funcoes.conexao());
        myConsulta.Open();
        SqlDataAdapter SDAConsulta = new SqlDataAdapter("dbo.stp_conciliacao_ins", myConsulta);
        SDAConsulta.SelectCommand.CommandType = CommandType.StoredProcedure;
        SDAConsulta.SelectCommand.CommandTimeout = 0;
        SDAConsulta.SelectCommand.Parameters.Add("@flg_operacao", SqlDbType.Char).Value = "L";
        SDAConsulta.SelectCommand.Parameters.Add("@COD_ID_PESSOA_LICENCIADO", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["LICENCIADO"].ToString());
        SDAConsulta.SelectCommand.Parameters.Add("@DTA_INICIO", SqlDbType.DateTime).Value = Convert.ToDateTime(txtDataIni.Text.ToString());
        SDAConsulta.SelectCommand.Parameters.Add("@DTA_FIM", SqlDbType.DateTime).Value = Convert.ToDateTime(txtDataFim.Text.ToString());
        SDAConsulta.SelectCommand.Parameters.Add("@FLG_CONCILIADO", SqlDbType.Char).Value = ddlConciliado.SelectedValue.ToString();

        DataSet dsConsulta = new DataSet();
        SDAConsulta.Fill(dsConsulta, "CONCILIACAO");
        gvConsulta.DataSource = dsConsulta.Tables["CONCILIACAO"].DefaultView;
        gvConsulta.DataBind();
        myConsulta.Close(); myConsulta.Dispose();



    }


    protected void btnPesquisar_Click(object sender, EventArgs e)
    {
        ConsultaGeral();

    }


    protected void btnImportarTexto_Click(object sender, EventArgs e)
    {
        string sCodificacao = DateTime.Now.Year.ToString() + DateTime.Now.Month.ToString() + DateTime.Now.Day.ToString() + DateTime.Now.Hour.ToString() + DateTime.Now.Minute.ToString() + DateTime.Now.Second.ToString();
        string serverPath = System.Web.HttpContext.Current.Server.MapPath("~/");
        string StrFileName = File1.PostedFile.FileName.Substring(File1.PostedFile.FileName.LastIndexOf("\\") + 1);
        string StrFileType = File1.PostedFile.ContentType;
        int IntFileSize = File1.PostedFile.ContentLength;
        File1.PostedFile.SaveAs(Server.MapPath("public_html") + "\\" + sCodificacao + "_ARQUIVO_" + StrFileName.ToString());

        string sArquivo = "";

        sArquivo = Server.MapPath("public_html") + "\\" + sCodificacao + "_ARQUIVO_" + StrFileName.ToString();
        int iContador = 0;

        StreamReader x;

        string Caminho = sArquivo;

        x = File.OpenText(Caminho);

        while (x.EndOfStream != true)
        {
            //iContador = iContador + 1;
            //if (iContador >= 10) { return; }
            string linha = x.ReadLine();
            string[] DadosConciliacao = linha.Split(';');


            //ClientScript.RegisterStartupScript(this.GetType(),
            //    "LinhasCSV"+iContador.ToString(), "alert('" + DadosConciliacao[0].ToString() + "');", true);
            //ClientScript.RegisterStartupScript(this.GetType(),
            //    "LinhasCSV2", "alert('" + DadosConciliacao[4].ToString() + "');", true);
        
        
            if ((DadosConciliacao[0].ToString().Trim() != "&nbsp;") && (DadosConciliacao[0].ToString() != null) && (DadosConciliacao[0].ToString().Trim() != "") && (DadosConciliacao[0].ToString().Trim().ToUpper() != "MARKETPLACE"))
            {

                //ClientScript.RegisterStartupScript(this.GetType(),
                //    "LinhasCSV3", "alert('Entrei');", true);
                //return;

                SqlConnection connInsCons = new SqlConnection(Funcoes.conexao());
                connInsCons.Open();
                SqlCommand cmdInsCons = new SqlCommand("dbo.stp_conciliacao_ins", connInsCons);
                cmdInsCons.CommandType = CommandType.StoredProcedure;
                cmdInsCons.Parameters.Add("@flg_operacao", SqlDbType.Char).Value = 'I';

                cmdInsCons.Parameters.Add("@FLG_PROCESSADO", SqlDbType.Char).Value = "N";
                cmdInsCons.Parameters.Add("@COD_ID_PESSOA_LICENCIADO", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["LICENCIADO"].ToString());

                cmdInsCons.Parameters.Add("@Marketplace", SqlDbType.VarChar).Value = DadosConciliacao[0].ToString();
                cmdInsCons.Parameters.Add("@IDSeller", SqlDbType.VarChar).Value = DadosConciliacao[1].ToString();
                cmdInsCons.Parameters.Add("@Conta", SqlDbType.VarChar).Value = DadosConciliacao[2].ToString();
                cmdInsCons.Parameters.Add("@CPFCNPJ", SqlDbType.VarChar).Value = DadosConciliacao[3].ToString();
                cmdInsCons.Parameters.Add("@Datadatransacao", SqlDbType.VarChar).Value = DadosConciliacao[4].ToString();
                cmdInsCons.Parameters.Add("@IDdatransacao", SqlDbType.VarChar).Value = DadosConciliacao[5].ToString();
                cmdInsCons.Parameters.Add("@Captura", SqlDbType.VarChar).Value = DadosConciliacao[6].ToString();
                cmdInsCons.Parameters.Add("@Serial", SqlDbType.VarChar).Value = DadosConciliacao[7].ToString();
                cmdInsCons.Parameters.Add("@statementDescriptor", SqlDbType.VarChar).Value = DadosConciliacao[8].ToString();
                cmdInsCons.Parameters.Add("@Antecipado", SqlDbType.VarChar).Value = DadosConciliacao[9].ToString();
                cmdInsCons.Parameters.Add("@Tipo", SqlDbType.VarChar).Value = DadosConciliacao[10].ToString();
                cmdInsCons.Parameters.Add("@Bandeira", SqlDbType.VarChar).Value = DadosConciliacao[11].ToString();
                cmdInsCons.Parameters.Add("@Parcelas", SqlDbType.VarChar).Value = DadosConciliacao[12].ToString();
                cmdInsCons.Parameters.Add("@ValorOriginal", SqlDbType.VarChar).Value = DadosConciliacao[13].ToString();
                cmdInsCons.Parameters.Add("@Valor", SqlDbType.VarChar).Value = DadosConciliacao[14].ToString();
                cmdInsCons.Parameters.Add("@Markup", SqlDbType.VarChar).Value = DadosConciliacao[15].ToString();
                cmdInsCons.Parameters.Add("@Markupfixo", SqlDbType.VarChar).Value = DadosConciliacao[16].ToString();
                cmdInsCons.Parameters.Add("@Markuptotal", SqlDbType.VarChar).Value = DadosConciliacao[17].ToString();
                cmdInsCons.Parameters.Add("@Recebedor", SqlDbType.VarChar).Value = DadosConciliacao[18].ToString();
                cmdInsCons.Parameters.Add("@FeeType", SqlDbType.VarChar).Value = DadosConciliacao[19].ToString();
                cmdInsCons.Parameters.Add("@Plano", SqlDbType.VarChar).Value = DadosConciliacao[20].ToString();

                if (DadosConciliacao[4].ToString().Trim() != "")
                {
                    cmdInsCons.Parameters.Add("@DTA_TRANSACAO", SqlDbType.DateTime).Value = Convert.ToDateTime(DadosConciliacao[4].ToString());
                }
                cmdInsCons.ExecuteNonQuery();
                connInsCons.Close();
                connInsCons.Dispose();
            }
        }
        x.Close();

    }
    protected void btnConciliar_Click(object sender, EventArgs e)
    {
        SqlConnection connInsCons = new SqlConnection(Funcoes.conexao());
        connInsCons.Open();
        SqlCommand cmdInsCons = new SqlCommand("dbo.stp_conciliacao_ins", connInsCons);
        cmdInsCons.CommandType = CommandType.StoredProcedure;
        cmdInsCons.Parameters.Add("@flg_operacao", SqlDbType.Char).Value = 'N';

        cmdInsCons.Parameters.Add("@COD_ID_PESSOA_LICENCIADO", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["LICENCIADO"].ToString());
        cmdInsCons.Parameters.Add("@DTA_INICIO", SqlDbType.DateTime).Value = Convert.ToDateTime(txtDataIni.Text.ToString());
        cmdInsCons.Parameters.Add("@DTA_FIM", SqlDbType.DateTime).Value = Convert.ToDateTime(txtDataFim.Text.ToString());
        cmdInsCons.Parameters.Add("@FLG_CONCILIADO", SqlDbType.Char).Value = ddlConciliado.SelectedValue.ToString();
        
        cmdInsCons.ExecuteNonQuery();
        connInsCons.Close();
        connInsCons.Dispose();


        ConsultaGeral();

    }
    protected void btnProcessar_Click(object sender, EventArgs e)
    {
        int iContador = 0;
        // Inserir dados na transacao
        foreach (GridViewRow row in gvConsulta.Rows)
        {
            if (((TextBox)row.FindControl("txtConciliado")).Text.ToString() == "N")
            {
                string IDTransacao = ((TextBox)row.FindControl("txtIDTransacao")).Text.ToString();
                string json = zoop.transacao("P", IDTransacao, "");
                txtJson.Text = json.ToString();

                string sToken = "";

                if (json.ToString().Trim() != "")
                {
                    JObject o = JObject.Parse(json);
                    sToken = o["on_behalf_of"].ToString();

                    // Transacoes
                    SqlConnection connInsCons = new SqlConnection(Funcoes.conexao());
                    connInsCons.Open();
                    SqlCommand cmdInsCons = new SqlCommand("dbo.stp_transacoes_ins", connInsCons);
                    cmdInsCons.CommandType = CommandType.StoredProcedure;
                    cmdInsCons.Parameters.Add("@flg_operacao", SqlDbType.Char).Value = 'I';


                    int iEstabelecimento = 0;
                    int iLicenciado = 0;

                    SqlConnection mySelCadastroEstabelecimento = new SqlConnection(Funcoes.conexao());
                    mySelCadastroEstabelecimento.Open();
                    SqlCommand cmdSelCadastroEstabelecimento = new SqlCommand("dbo.stp_pessoas_fj_ins", mySelCadastroEstabelecimento);
                    cmdSelCadastroEstabelecimento.CommandType = CommandType.StoredProcedure;
                    cmdSelCadastroEstabelecimento.Parameters.Add("@flg_operacao", SqlDbType.Char).Value = "Z";
                    cmdSelCadastroEstabelecimento.Parameters.Add("@NUM_TOKEN", SqlDbType.VarChar).Value = sToken.ToString();
                    SqlDataReader ReaderCadastroEstabelecimento = cmdSelCadastroEstabelecimento.ExecuteReader();
                    while (ReaderCadastroEstabelecimento.Read())
                    {
                        iEstabelecimento = Funcoes.strToInt(ReaderCadastroEstabelecimento["COD_ID_PESSOAS_FJ"].ToString());
                        iLicenciado = Funcoes.strToInt(ReaderCadastroEstabelecimento["COD_ID_PESSOA_LICENCIADO"].ToString());
                    }

                    cmdInsCons.Parameters.Add("@COD_ID_PESSOA_LICENCIADO", SqlDbType.Int).Value = iLicenciado;
                    cmdInsCons.Parameters.Add("@COD_ID_PESSOAS_FJ", SqlDbType.Int).Value = iEstabelecimento;
                    cmdInsCons.Parameters.Add("@FLG_ORIGEM", SqlDbType.Char).Value = "Z";


                    cmdInsCons.Parameters.Add("@DTA_DATA", SqlDbType.DateTime).Value = Convert.ToDateTime(o["created_at"].ToString());
                    cmdInsCons.Parameters.Add("@NOM_CODE", SqlDbType.VarChar).Value = o["id"].ToString();
                    cmdInsCons.Parameters.Add("@NUM_TOKEN", SqlDbType.VarChar).Value = o["on_behalf_of"].ToString();

                    cmdInsCons.Parameters.Add("@COD_ID_TIPO", SqlDbType.Int).Value = 1;
                    cmdInsCons.Parameters.Add("@NOM_STATUS", SqlDbType.VarChar).Value = o["status"].ToString();

                    if ((o["point_of_sale"]["identification_number"].ToString().Trim() == "") || (o["point_of_sale"]["identification_number"].ToString().Trim() == "null"))
                    {
                        cmdInsCons.Parameters.Add("@FLG_PRESENCIAL_ONLINE", SqlDbType.Char).Value = "O";
                    }
                    else
                    {
                        cmdInsCons.Parameters.Add("@FLG_PRESENCIAL_ONLINE", SqlDbType.Char).Value = "P";
                    }

                    cmdInsCons.Parameters.Add("@NOM_TIPO_PAGAMENTO", SqlDbType.VarChar).Value = o["payment_type"].ToString();

                    if ((o["payment_type"].ToString() == "pix"))
                    {
                        cmdInsCons.Parameters.Add("@NOM_CODIGO_PAGAMENTO", SqlDbType.VarChar).Value = o["payment_method"]["provider"].ToString();
                        cmdInsCons.Parameters.Add("@NUM_PARCELAS", SqlDbType.Int).Value = 1;
                        cmdInsCons.Parameters.Add("@NOM_QRCODE", SqlDbType.VarChar).Value = o["payment_method"]["qr_code"]["emv"].ToString();

                    }
                    if ((o["payment_type"].ToString() == "boleto"))
                    {
                        cmdInsCons.Parameters.Add("@NOM_CODIGO_PAGAMENTO", SqlDbType.VarChar).Value = o["payment_method"]["resource"].ToString();
                        cmdInsCons.Parameters.Add("@NUM_PARCELAS", SqlDbType.Int).Value = 1;

                        cmdInsCons.Parameters.Add("@NOM_CODIGO_BARRAS", SqlDbType.VarChar).Value = o["payment_method"]["barcode"].ToString();
                        cmdInsCons.Parameters.Add("@NOM_URL_BOLETO", SqlDbType.VarChar).Value = o["payment_method"]["url"].ToString();
                        cmdInsCons.Parameters.Add("@NOM_NUMERO_DOCUMENTO", SqlDbType.VarChar).Value = o["payment_method"]["document_number"].ToString();
                        cmdInsCons.Parameters.Add("@NOM_DATA_VENCIMENTO", SqlDbType.VarChar).Value = o["payment_method"]["expiration_date"].ToString();

                    }


                    if ((o["payment_type"].ToString() == "credit"))
                    {
                        cmdInsCons.Parameters.Add("@NOM_CODIGO_PAGAMENTO", SqlDbType.VarChar).Value = o["payment_method"]["resource"].ToString() + " " + o["payment_method"]["card_brand"].ToString();

                        try { cmdInsCons.Parameters.Add("@NUM_PARCELAS", SqlDbType.Int).Value = Funcoes.strToInt(o["installment_plan"]["number_installments"].ToString()); }
                        catch { }

                        //cmdInsCons.Parameters.Add("@NUM_PARCELAS", SqlDbType.Int).Value = Funcoes.strToInt(o["installment_plan"]["number_installments"].ToString());

                        cmdInsCons.Parameters.Add("@NOM_CARTAO_4_PRIMEIROS_DIGIROS", SqlDbType.VarChar).Value = o["payment_method"]["first4_digits"].ToString();
                        cmdInsCons.Parameters.Add("@NOM_CARTAO_4_ULTIMOS_DIGITOS", SqlDbType.VarChar).Value = o["payment_method"]["last4_digits"].ToString();
                        cmdInsCons.Parameters.Add("@NOM_CARTAO_MES", SqlDbType.VarChar).Value = o["payment_method"]["expiration_month"].ToString();
                        cmdInsCons.Parameters.Add("@NOM_CARTAO_ANO", SqlDbType.VarChar).Value = o["payment_method"]["expiration_year"].ToString();
                    }

                    if ((o["payment_type"].ToString() == "debit"))
                    {
                        cmdInsCons.Parameters.Add("@NOM_CODIGO_PAGAMENTO", SqlDbType.VarChar).Value = o["payment_method"]["resource"].ToString() + " " + o["payment_method"]["card_brand"].ToString();
                        cmdInsCons.Parameters.Add("@NUM_PARCELAS", SqlDbType.Int).Value = 1;

                        cmdInsCons.Parameters.Add("@NOM_CARTAO_4_PRIMEIROS_DIGIROS", SqlDbType.VarChar).Value = o["payment_method"]["first4_digits"].ToString();
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

                    cmdInsCons.Parameters.Add("@NOM_NUMERO_TRANSACAO", SqlDbType.VarChar).Value = o["transaction_number"].ToString();
                    cmdInsCons.Parameters.Add("@NOM_RECIBO", SqlDbType.VarChar).Value = o["sales_receipt"].ToString();
                    // Carregar os dados do Recibo //




                    // Dados do Boleto

                    //if (o["payment_method"].Contains("barcode") == true) { cmdInsCons.Parameters.Add("@NOM_CODIGO_BARRAS", SqlDbType.VarChar).Value = o["payment_method"]["barcode"].ToString(); }
                    //if (o["payment_method"].Contains("url") == true) { cmdInsCons.Parameters.Add("@NOM_URL_BOLETO", SqlDbType.VarChar).Value = o["payment_method"]["url"].ToString(); }
                    //if (o["payment_method"].Contains("document_number") == true) { cmdInsCons.Parameters.Add("@NOM_NUMERO_DOCUMENTO", SqlDbType.VarChar).Value = o["payment_method"]["document_number"].ToString(); }
                    //if (o["payment_method"].Contains("expiration_date") == true) { cmdInsCons.Parameters.Add("@NOM_DATA_VENCIMENTO", SqlDbType.VarChar).Value = o["payment_method"]["expiration_date"].ToString(); }

                    // Dados do PIX

                    //if (o["payment_method"].Contains("qr_code") == true) { cmdInsCons.Parameters.Add("@NOM_QRCODE", SqlDbType.VarChar).Value = o["payment_method"]["qr_code"]["emv"].ToString(); }

                    // -----------------------------------------------------------------------------------------------


                    cmdInsCons.Parameters.Add("@NOM_AUTORIZACAO", SqlDbType.VarChar).Value = o["transaction_number"].ToString();
                    cmdInsCons.Parameters.Add("@DES_DESCRICAO", SqlDbType.Text).Value = o["payment_type"].ToString();

                    if (o["history"].Count() > 0)
                    {
                        for (int i = 0; i < o["history"].Count(); i++)
                        {
                            // Inserção de dados do Histórico

                            SqlConnection connInsConsHistorico = new SqlConnection(Funcoes.conexao());
                            connInsConsHistorico.Open();
                            SqlCommand cmdInsConsHistorico = new SqlCommand("dbo.stp_transacoes_historico_ins", connInsConsHistorico);
                            cmdInsConsHistorico.CommandType = CommandType.StoredProcedure;
                            cmdInsConsHistorico.Parameters.Add("@flg_operacao", SqlDbType.Char).Value = 'I';
                            cmdInsConsHistorico.Parameters.Add("@COD_ID_PESSOA_LICENCIADO", SqlDbType.Int).Value = iLicenciado;
                            cmdInsConsHistorico.Parameters.Add("@NOM_CODE", SqlDbType.VarChar).Value = o["id"].ToString();
                            cmdInsConsHistorico.Parameters.Add("@DTA_DATA", SqlDbType.DateTime).Value = DateTime.Now;
                            cmdInsConsHistorico.Parameters.Add("@NOM_TIPO", SqlDbType.VarChar).Value = o["history"][i]["operation_type"].ToString();
                            cmdInsConsHistorico.Parameters.Add("@NOM_STATUS", SqlDbType.VarChar).Value = o["history"][i]["status"].ToString();
                            cmdInsConsHistorico.Parameters.Add("@NUM_VALOR", SqlDbType.Float).Value = Funcoes.strToDouble(o["history"][i]["amount"].ToString());
                            cmdInsConsHistorico.Parameters.Add("@NOM_CODIGO_RESPOSTA", SqlDbType.VarChar).Value = o["history"][i]["response_code"].ToString();
                            cmdInsConsHistorico.Parameters.Add("@NOM_DESCRICAO_RESPOSTA", SqlDbType.VarChar).Value = o["history"][i]["response_message"].ToString();
                            cmdInsConsHistorico.Parameters.Add("@NOM_ID_AUTORIZACAO", SqlDbType.VarChar).Value = o["history"][i]["authorizer_id"].ToString();
                            cmdInsConsHistorico.Parameters.Add("@NOM_NSU_AUTORIZACAO", SqlDbType.VarChar).Value = o["history"][i]["authorization_nsu"].ToString();
                            cmdInsConsHistorico.Parameters.Add("@DTA_DATA_CRIACAO", SqlDbType.DateTime).Value = Convert.ToDateTime(o["history"][i]["created_at"].ToString());
                            cmdInsConsHistorico.ExecuteNonQuery();
                            connInsConsHistorico.Close();
                            connInsConsHistorico.Dispose();
                        }
                    }

                    // Carregar os dados do recibo ----------------------------------------------------------

                    if (o["sales_receipt"].ToString().Trim() != "")
                    {
                        string jsonRecibo = zoop.DetalhesRecibo(o["sales_receipt"].ToString());
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
                            if (jsonTerminal.ToString().Trim() != "")
                            {
                                JObject oTerminal = JObject.Parse(jsonTerminal);

                                cmdInsCons.Parameters.Add("@NOM_CODIGO_TERMINAL", SqlDbType.VarChar).Value = oTerminal["code"].ToString();
                                cmdInsCons.Parameters.Add("@NOM_STATUS_TERMINAL", SqlDbType.VarChar).Value = oTerminal["status"].ToString();
                                cmdInsCons.Parameters.Add("@NOM_NUMERO_SERIAL_TERMINAL", SqlDbType.VarChar).Value = oTerminal["serial_number"].ToString();
                                cmdInsCons.Parameters.Add("@NOM_MODELO_TERMINAL", SqlDbType.VarChar).Value = oTerminal["terminal_model"].ToString();
                                cmdInsCons.Parameters.Add("@NOM_FABRICANTE_TERMINAL", SqlDbType.VarChar).Value = oTerminal["manufacturer"].ToString();
                            }
                        }

                    }

                    // Carregar os recebíveis ----------------------------------------------------------

                    string jsonRecebiveis = zoop.DetalhesRecebiveis(o["id"].ToString());
                    JObject oRecebiveis = JObject.Parse(jsonRecebiveis);

                    if (oRecebiveis["items"].Count() > 0)
                    {
                        for (int i = 0; i < oRecebiveis["items"].Count(); i++)
                        {

                            // Inserção de dados do Recebiveis

                            SqlConnection connInsConsRecebiveis = new SqlConnection(Funcoes.conexao());
                            connInsConsRecebiveis.Open();
                            SqlCommand cmdInsConsRecebiveis = new SqlCommand("dbo.stp_transacoes_recebiveis_ins", connInsConsRecebiveis);
                            cmdInsConsRecebiveis.CommandType = CommandType.StoredProcedure;
                            cmdInsConsRecebiveis.Parameters.Add("@flg_operacao", SqlDbType.Char).Value = 'I';
                            cmdInsConsRecebiveis.Parameters.Add("@COD_ID_PESSOA_LICENCIADO", SqlDbType.Int).Value = iLicenciado;
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

                    cmdInsCons.ExecuteNonQuery();
                    connInsCons.Close();
                    connInsCons.Dispose();
                    iContador = iContador + 1;
                }
            }
        }

        ClientScript.RegisterStartupScript(this.GetType(),
            "Sucesso", "alert('"+iContador.ToString()+" Transações não conciliadas processadas com sucesso!');", true);

    }
}