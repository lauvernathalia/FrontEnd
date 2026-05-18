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

public partial class con_transacoes_processamento : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            txtDataIni.Text = DateTime.Now.AddDays(-7).ToShortDateString();
            txtDataFim.Text = DateTime.Now.ToShortDateString();

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
        SqlDataAdapter SDAConsulta = new SqlDataAdapter("dbo.stp_notificacoes_zoop_ins", myConsulta);
        SDAConsulta.SelectCommand.CommandType = CommandType.StoredProcedure;
        SDAConsulta.SelectCommand.Parameters.Add("@flg_operacao", SqlDbType.Char).Value = "P";
        SDAConsulta.SelectCommand.Parameters.Add("@NUM_TOKEN", SqlDbType.VarChar).Value = txtToken.Text.ToString();
        SDAConsulta.SelectCommand.Parameters.Add("@DTA_INICIO", SqlDbType.DateTime).Value = Convert.ToDateTime(txtDataIni.Text.ToString());
        SDAConsulta.SelectCommand.Parameters.Add("@DTA_FIM", SqlDbType.DateTime).Value = Convert.ToDateTime(txtDataFim.Text.ToString());
        DataSet dsConsulta = new DataSet();
        SDAConsulta.Fill(dsConsulta, "NOTIFICACOES_ZOOP");
        rptConsulta.DataSource = dsConsulta.Tables["NOTIFICACOES_ZOOP"].DefaultView;
        rptConsulta.DataBind();
        myConsulta.Close(); myConsulta.Dispose();
    }

    
    protected void btnProcessar_Click(object sender, EventArgs e)
    {
        foreach (RepeaterItem item in rptConsulta.Items)
        {
            var bodyText = ((TextBox)item.FindControl("txtJSON")).Text.ToString();

            string sConexao = ConfigurationManager.AppSettings["user"].ToString() + ";" +
                ConfigurationManager.AppSettings["password"].ToString() + ";" +
                ConfigurationManager.AppSettings["datasource"].ToString() + ";" +
                ConfigurationManager.AppSettings["basecatalog"].ToString();

            SqlConnection connInsConsZOOP = new SqlConnection(sConexao);
            connInsConsZOOP.Open();
            SqlCommand cmdInsConsZOOP = new SqlCommand("dbo.stp_notificacoes_zoop_ins", connInsConsZOOP);
            cmdInsConsZOOP.CommandType = CommandType.StoredProcedure;
            cmdInsConsZOOP.Parameters.Add("@flg_operacao", SqlDbType.Char).Value = 'I';
            cmdInsConsZOOP.Parameters.Add("@DES_JSON", SqlDbType.Text).Value = bodyText.ToString();
            string sToken = "";


            string sCode = "";

            if (bodyText.ToString().Trim() != "")
            {
                string json2 = bodyText.ToString();
                JObject o2 = JObject.Parse(json2);
                string sType2 = o2["type"].ToString();

                if ((sType2.Contains("transaction") == true) || (sType2.Contains("buyer.transaction") == true))
                {
                    cmdInsConsZOOP.Parameters.Add("@DTA_DATA", SqlDbType.DateTime).Value = Convert.ToDateTime(o2["payload"]["object"]["created_at"].ToString());
                    cmdInsConsZOOP.Parameters.Add("@NOM_CODE", SqlDbType.VarChar).Value = o2["payload"]["object"]["id"].ToString();
                    sCode = o2["payload"]["object"]["id"].ToString();
                    sToken = o2["payload"]["object"]["on_behalf_of"].ToString();
                    cmdInsConsZOOP.Parameters.Add("@NUM_TOKEN", SqlDbType.VarChar).Value = o2["payload"]["object"]["on_behalf_of"].ToString();
                    cmdInsConsZOOP.Parameters.Add("@NOM_TIPO", SqlDbType.VarChar).Value = o2["type"].ToString();
                    cmdInsConsZOOP.Parameters.Add("@FLG_STATUS", SqlDbType.VarChar).Value = o2["payload"]["object"]["status"].ToString();
                }

                if ((sType2.Contains("ping") == true))
                {
                    cmdInsConsZOOP.Parameters.Add("@DTA_DATA", SqlDbType.DateTime).Value = Convert.ToDateTime(o2["created_at"].ToString());
                    cmdInsConsZOOP.Parameters.Add("@NOM_CODE", SqlDbType.VarChar).Value = o2["id"].ToString();
                    sCode = o2["id"].ToString();
                    sToken = "";
                    cmdInsConsZOOP.Parameters.Add("@NUM_TOKEN", SqlDbType.VarChar).Value = "";
                    cmdInsConsZOOP.Parameters.Add("@NOM_TIPO", SqlDbType.VarChar).Value = o2["type"].ToString();
                    cmdInsConsZOOP.Parameters.Add("@FLG_STATUS", SqlDbType.VarChar).Value = "";
                }

                if ((sType2.Contains("transfer") == true))
                {
                    cmdInsConsZOOP.Parameters.Add("@DTA_DATA", SqlDbType.DateTime).Value = Convert.ToDateTime(o2["payload"]["object"]["created_at"].ToString());
                    cmdInsConsZOOP.Parameters.Add("@NOM_CODE", SqlDbType.VarChar).Value = o2["payload"]["object"]["id"].ToString();
                    sCode = o2["payload"]["object"]["id"].ToString();
                    sToken = o2["payload"]["object"]["recipient"].ToString();
                    cmdInsConsZOOP.Parameters.Add("@NUM_TOKEN", SqlDbType.VarChar).Value = o2["payload"]["object"]["recipient"].ToString();
                    cmdInsConsZOOP.Parameters.Add("@NOM_TIPO", SqlDbType.VarChar).Value = o2["type"].ToString();
                    cmdInsConsZOOP.Parameters.Add("@FLG_STATUS", SqlDbType.VarChar).Value = o2["payload"]["object"]["status"].ToString();
                }

                if ((sType2.Contains("receivable") == true))
                {
                    cmdInsConsZOOP.Parameters.Add("@DTA_DATA", SqlDbType.DateTime).Value = Convert.ToDateTime(o2["created_at"].ToString());
                    cmdInsConsZOOP.Parameters.Add("@NOM_CODE", SqlDbType.VarChar).Value = o2["payload"]["object"]["id"].ToString();
                    sCode = o2["payload"]["object"]["id"].ToString();
                    sToken = o2["payload"]["object"]["recipient"].ToString();
                    cmdInsConsZOOP.Parameters.Add("@NUM_TOKEN", SqlDbType.VarChar).Value = o2["payload"]["object"]["recipient"].ToString();
                    cmdInsConsZOOP.Parameters.Add("@NOM_TIPO", SqlDbType.VarChar).Value = o2["type"].ToString();
                    cmdInsConsZOOP.Parameters.Add("@FLG_STATUS", SqlDbType.VarChar).Value = o2["payload"]["object"]["status"].ToString();
                }


            }

            string iCodigoNotificacao = cmdInsConsZOOP.ExecuteScalar().ToString();
            //cmdInsConsZOOP.ExecuteNonQuery();
            connInsConsZOOP.Close();
            connInsConsZOOP.Dispose();

            if (bodyText.ToString().Trim() != "")
            {

                string json2 = bodyText.ToString();
                JObject o2 = JObject.Parse(json2);
                string sType2 = o2["type"].ToString();



                if ((sType2.Contains("transfer") == true))
                {
                    // Transferências
                    SqlConnection connInsCons = new SqlConnection(Funcoes.conexao());
                    connInsCons.Open();
                    SqlCommand cmdInsCons = new SqlCommand("dbo.stp_extrato_ins", connInsCons);
                    cmdInsCons.CommandType = CommandType.StoredProcedure;
                    cmdInsCons.Parameters.Add("@flg_operacao", SqlDbType.Char).Value = 'I';

                    // Busca os dados do Licenciado e do Estabelecimento
                    int iEstabelecimento = 0;
                    int iLicenciado = 0;

                    SqlConnection mySelCadastroEstabelecimento = new SqlConnection(sConexao);
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

                    if (bodyText.ToString().Trim() != "")
                    {
                        string json = bodyText.ToString();
                        JObject o = JObject.Parse(json);
                        string sType = o["type"].ToString();

                        cmdInsCons.Parameters.Add("@DTA_DATA", SqlDbType.DateTime).Value = DateTime.Now;
                        cmdInsCons.Parameters.Add("@DTA_CRIADO", SqlDbType.DateTime).Value = Convert.ToDateTime(o["payload"]["object"]["created_at"].ToString());
                        cmdInsCons.Parameters.Add("@DTA_ATUALIZADO", SqlDbType.DateTime).Value = Convert.ToDateTime(o["payload"]["object"]["updated_at"].ToString());

                        if (o["payload"]["object"]["transfer_date"].ToString().Trim() != "" && o["payload"]["object"]["transfer_date"].ToString().Trim() != "null")
                        {
                            cmdInsCons.Parameters.Add("@DTA_TRANSFERENCIA", SqlDbType.DateTime).Value = Convert.ToDateTime(o["payload"]["object"]["transfer_date"].ToString());
                        }


                        cmdInsCons.Parameters.Add("@FLG_STATUS", SqlDbType.VarChar).Value = o["payload"]["object"]["status"].ToString();

                        cmdInsCons.Parameters.Add("@NOM_OBJECT_ID", SqlDbType.VarChar).Value = o["payload"]["object"]["id"].ToString();
                        cmdInsCons.Parameters.Add("@NOM_TOKEN_RECEBEDOR", SqlDbType.VarChar).Value = o["payload"]["object"]["recipient"].ToString();
                        cmdInsCons.Parameters.Add("@NOM_TOKEN_REMETENTE", SqlDbType.VarChar).Value = o["payload"]["object"]["sender"].ToString();

                        cmdInsCons.Parameters.Add("@NUM_VALOR", SqlDbType.Float).Value = Funcoes.strToDouble(o["payload"]["object"]["amount"].ToString()) / 100;
                        cmdInsCons.Parameters.Add("@NUM_VALOR_BRUTO", SqlDbType.Float).Value = Funcoes.strToDouble(o["payload"]["object"]["original_amount"].ToString()) / 100;
                        cmdInsCons.Parameters.Add("@DES_DESCRICAO", SqlDbType.Text).Value = o["payload"]["object"]["description"].ToString();

                        if (o["payload"]["object"]["bank_account"].ToString().Trim() != "")
                        {
                            cmdInsCons.Parameters.Add("@NOM_TOKEN_CLIENTE", SqlDbType.VarChar).Value = o["payload"]["object"]["bank_account"]["customer"].ToString();
                            cmdInsCons.Parameters.Add("@NOM_BANCO_ID", SqlDbType.VarChar).Value = o["payload"]["object"]["bank_account"]["id"].ToString();
                            cmdInsCons.Parameters.Add("@NOM_URI", SqlDbType.VarChar).Value = o["payload"]["object"]["bank_account"]["uri"].ToString();
                            cmdInsCons.Parameters.Add("@NOM_TITULAR_BANCO", SqlDbType.VarChar).Value = o["payload"]["object"]["bank_account"]["holder_name"].ToString();

                            cmdInsCons.Parameters.Add("@NOM_CNPJCPF", SqlDbType.VarChar).Value = o["payload"]["object"]["bank_account"]["taxpayer_id"].ToString();
                            cmdInsCons.Parameters.Add("@NOM_BANCO", SqlDbType.VarChar).Value = o["payload"]["object"]["bank_account"]["bank_name"].ToString();
                            cmdInsCons.Parameters.Add("@NOM_CODIGO_BANCO", SqlDbType.VarChar).Value = o["payload"]["object"]["bank_account"]["bank_code"].ToString();
                            cmdInsCons.Parameters.Add("@NOM_AGENCIA_BANCO", SqlDbType.VarChar).Value = o["payload"]["object"]["bank_account"]["routing_number"].ToString();
                            cmdInsCons.Parameters.Add("@NOM_CONTA_BANCO", SqlDbType.VarChar).Value = o["payload"]["object"]["bank_account"]["account_number"].ToString();
                        }
                        cmdInsCons.Parameters.Add("@FLG_TIPO", SqlDbType.VarChar).Value = o["payload"]["object"]["type"].ToString();
                        cmdInsCons.Parameters.Add("@FLG_RECURSO", SqlDbType.VarChar).Value = o["payload"]["object"]["resource"].ToString();

                    }
                    cmdInsCons.ExecuteNonQuery();
                    connInsCons.Close();
                    connInsCons.Dispose();
                }


                if ((sType2.Contains("receivable") == true))
                {
                    // Recebimentos
                    SqlConnection connInsCons = new SqlConnection(Funcoes.conexao());
                    connInsCons.Open();
                    SqlCommand cmdInsCons = new SqlCommand("dbo.stp_extrato_ins", connInsCons);
                    cmdInsCons.CommandType = CommandType.StoredProcedure;
                    cmdInsCons.Parameters.Add("@flg_operacao", SqlDbType.Char).Value = 'I';

                    // Busca os dados do Licenciado e do Estabelecimento
                    int iEstabelecimento = 0;
                    int iLicenciado = 0;

                    SqlConnection mySelCadastroEstabelecimento = new SqlConnection(sConexao);
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

                    if (bodyText.ToString().Trim() != "")
                    {
                        string json = bodyText.ToString();
                        JObject o = JObject.Parse(json);
                        string sType = o["type"].ToString();

                        cmdInsCons.Parameters.Add("@DTA_DATA", SqlDbType.DateTime).Value = DateTime.Now;
                        cmdInsCons.Parameters.Add("@DTA_CRIADO", SqlDbType.DateTime).Value = Convert.ToDateTime(o["created_at"].ToString());
                        cmdInsCons.Parameters.Add("@DTA_ATUALIZADO", SqlDbType.DateTime).Value = Convert.ToDateTime(o["updated_at"].ToString());

                        if (o["payload"]["object"]["expected_on"].ToString().Trim() != "" && o["payload"]["object"]["expected_on"].ToString().Trim() != "null")
                        {
                            cmdInsCons.Parameters.Add("@DTA_PREVISAO", SqlDbType.DateTime).Value = Convert.ToDateTime(o["payload"]["object"]["expected_on"].ToString());
                        }

                        if (o["payload"]["object"]["paid_at"].ToString().Trim() != "" && o["payload"]["object"]["paid_at"].ToString().Trim() != "null")
                        {
                            cmdInsCons.Parameters.Add("@DTA_PAGAMENTO", SqlDbType.DateTime).Value = Convert.ToDateTime(o["payload"]["object"]["paid_at"].ToString());
                        }

                        cmdInsCons.Parameters.Add("@FLG_STATUS", SqlDbType.VarChar).Value = o["payload"]["object"]["status"].ToString();

                        cmdInsCons.Parameters.Add("@NOM_OBJECT_ID", SqlDbType.VarChar).Value = o["payload"]["object"]["id"].ToString();
                        cmdInsCons.Parameters.Add("@NOM_TOKEN_RECEBEDOR", SqlDbType.VarChar).Value = o["payload"]["object"]["recipient"].ToString();
                        cmdInsCons.Parameters.Add("@NOM_TOKEN_REMETENTE", SqlDbType.VarChar).Value = o["payload"]["object"]["recipient"].ToString();

                        cmdInsCons.Parameters.Add("@NUM_VALOR", SqlDbType.Float).Value = Funcoes.strToDouble(o["payload"]["object"]["amount"].ToString()) / 100;
                        cmdInsCons.Parameters.Add("@NUM_VALOR_BRUTO", SqlDbType.Float).Value = Funcoes.strToDouble(o["payload"]["object"]["gross_amount"].ToString()) / 100;

                        cmdInsCons.Parameters.Add("@NOM_OBJECT_ID_TRANSACAO", SqlDbType.VarChar).Value = o["payload"]["object"]["transaction"].ToString();


                        cmdInsCons.Parameters.Add("@DES_DESCRICAO", SqlDbType.Text).Value = "Recebimento Venda";//o["payload"]["object"]["description"].ToString();

                        cmdInsCons.Parameters.Add("@FLG_TIPO", SqlDbType.VarChar).Value = o["payload"]["object"]["status"].ToString();
                        cmdInsCons.Parameters.Add("@FLG_RECURSO", SqlDbType.VarChar).Value = o["payload"]["object"]["resource"].ToString();

                    }
                    cmdInsCons.ExecuteNonQuery();
                    connInsCons.Close();
                    connInsCons.Dispose();
                }

                // transaction.succeeded

                if ((sType2.Contains("transaction.succeeded") == true) || (sType2.Contains("buyer.transaction.succeeded") == true))
                {
                    // Transacoes
                    SqlConnection connInsCons = new SqlConnection(sConexao);
                    connInsCons.Open();
                    SqlCommand cmdInsCons = new SqlCommand("dbo.stp_transacoes_ins", connInsCons);
                    cmdInsCons.CommandType = CommandType.StoredProcedure;
                    cmdInsCons.Parameters.Add("@flg_operacao", SqlDbType.Char).Value = 'I';


                    int iEstabelecimento = 0;
                    int iLicenciado = 0;

                    SqlConnection mySelCadastroEstabelecimento = new SqlConnection(sConexao);
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

                    if (bodyText.ToString().Trim() != "")
                    {
                        string json = bodyText.ToString();
                        JObject o = JObject.Parse(json);
                        string sType = o["type"].ToString();

                        cmdInsCons.Parameters.Add("@DTA_DATA", SqlDbType.DateTime).Value = Convert.ToDateTime(o["payload"]["object"]["created_at"].ToString());
                        cmdInsCons.Parameters.Add("@NOM_CODE", SqlDbType.VarChar).Value = o["payload"]["object"]["id"].ToString();
                        cmdInsCons.Parameters.Add("@NUM_TOKEN", SqlDbType.VarChar).Value = o["payload"]["object"]["on_behalf_of"].ToString();

                        cmdInsCons.Parameters.Add("@COD_ID_TIPO", SqlDbType.Int).Value = 1;
                        cmdInsCons.Parameters.Add("@NOM_STATUS", SqlDbType.VarChar).Value = o["payload"]["object"]["status"].ToString();

                        if ((o["payload"]["object"]["point_of_sale"]["identification_number"].ToString().Trim() == "") || (o["payload"]["object"]["point_of_sale"]["identification_number"].ToString().Trim() == "null"))
                        {
                            cmdInsCons.Parameters.Add("@FLG_PRESENCIAL_ONLINE", SqlDbType.Char).Value = "O";
                        }
                        else
                        {
                            cmdInsCons.Parameters.Add("@FLG_PRESENCIAL_ONLINE", SqlDbType.Char).Value = "P";
                        }

                        cmdInsCons.Parameters.Add("@NOM_TIPO_PAGAMENTO", SqlDbType.VarChar).Value = o["payload"]["object"]["payment_type"].ToString();

                        if ((o["payload"]["object"]["payment_type"].ToString() == "pix"))
                        {
                            cmdInsCons.Parameters.Add("@NOM_CODIGO_PAGAMENTO", SqlDbType.VarChar).Value = o["payload"]["object"]["payment_method"]["provider"].ToString();
                            cmdInsCons.Parameters.Add("@NUM_PARCELAS", SqlDbType.Int).Value = 1;
                            cmdInsCons.Parameters.Add("@NOM_QRCODE", SqlDbType.VarChar).Value = o["payload"]["object"]["payment_method"]["qr_code"]["emv"].ToString();

                        }
                        if ((o["payload"]["object"]["payment_type"].ToString() == "boleto"))
                        {
                            cmdInsCons.Parameters.Add("@NOM_CODIGO_PAGAMENTO", SqlDbType.VarChar).Value = o["payload"]["object"]["payment_method"]["resource"].ToString();
                            cmdInsCons.Parameters.Add("@NUM_PARCELAS", SqlDbType.Int).Value = 1;

                            cmdInsCons.Parameters.Add("@NOM_CODIGO_BARRAS", SqlDbType.VarChar).Value = o["payload"]["object"]["payment_method"]["barcode"].ToString();
                            cmdInsCons.Parameters.Add("@NOM_URL_BOLETO", SqlDbType.VarChar).Value = o["payload"]["object"]["payment_method"]["url"].ToString();
                            cmdInsCons.Parameters.Add("@NOM_NUMERO_DOCUMENTO", SqlDbType.VarChar).Value = o["payload"]["object"]["payment_method"]["document_number"].ToString();
                            cmdInsCons.Parameters.Add("@NOM_DATA_VENCIMENTO", SqlDbType.VarChar).Value = o["payload"]["object"]["payment_method"]["expiration_date"].ToString();

                        }


                        if ((o["payload"]["object"]["payment_type"].ToString() == "credit"))
                        {
                            cmdInsCons.Parameters.Add("@NOM_CODIGO_PAGAMENTO", SqlDbType.VarChar).Value = o["payload"]["object"]["payment_method"]["resource"].ToString() + " " + o["payload"]["object"]["payment_method"]["card_brand"].ToString();

                            try { cmdInsCons.Parameters.Add("@NUM_PARCELAS", SqlDbType.Int).Value = Funcoes.strToInt(o["payload"]["object"]["installment_plan"]["number_installments"].ToString()); }
                            catch { }

                            //cmdInsCons.Parameters.Add("@NUM_PARCELAS", SqlDbType.Int).Value = Funcoes.strToInt(o["payload"]["object"]["installment_plan"]["number_installments"].ToString());

                            cmdInsCons.Parameters.Add("@NOM_CARTAO_4_PRIMEIROS_DIGIROS", SqlDbType.VarChar).Value = o["payload"]["object"]["payment_method"]["first4_digits"].ToString();
                            cmdInsCons.Parameters.Add("@NOM_CARTAO_4_ULTIMOS_DIGITOS", SqlDbType.VarChar).Value = o["payload"]["object"]["payment_method"]["last4_digits"].ToString();
                            cmdInsCons.Parameters.Add("@NOM_CARTAO_MES", SqlDbType.VarChar).Value = o["payload"]["object"]["payment_method"]["expiration_month"].ToString();
                            cmdInsCons.Parameters.Add("@NOM_CARTAO_ANO", SqlDbType.VarChar).Value = o["payload"]["object"]["payment_method"]["expiration_year"].ToString();
                        }

                        if ((o["payload"]["object"]["payment_type"].ToString() == "debit"))
                        {
                            cmdInsCons.Parameters.Add("@NOM_CODIGO_PAGAMENTO", SqlDbType.VarChar).Value = o["payload"]["object"]["payment_method"]["resource"].ToString() + " " + o["payload"]["object"]["payment_method"]["card_brand"].ToString();
                            cmdInsCons.Parameters.Add("@NUM_PARCELAS", SqlDbType.Int).Value = 1;

                            cmdInsCons.Parameters.Add("@NOM_CARTAO_4_PRIMEIROS_DIGIROS", SqlDbType.VarChar).Value = o["payload"]["object"]["payment_method"]["first4_digits"].ToString();
                            cmdInsCons.Parameters.Add("@NOM_CARTAO_4_ULTIMOS_DIGITOS", SqlDbType.VarChar).Value = o["payload"]["object"]["payment_method"]["last4_digits"].ToString();
                            cmdInsCons.Parameters.Add("@NOM_CARTAO_MES", SqlDbType.VarChar).Value = o["payload"]["object"]["payment_method"]["expiration_month"].ToString();
                            cmdInsCons.Parameters.Add("@NOM_CARTAO_ANO", SqlDbType.VarChar).Value = o["payload"]["object"]["payment_method"]["expiration_year"].ToString();
                        }



                        cmdInsCons.Parameters.Add("@NUM_VALOR_BRUTO", SqlDbType.Float).Value = Funcoes.strToDouble(o["payload"]["object"]["amount"].ToString()) / 100;
                        cmdInsCons.Parameters.Add("@NUM_VALOR_DESCONTO", SqlDbType.Float).Value = 0;
                        cmdInsCons.Parameters.Add("@NUM_VALOR_INTERMEDIACAO_RATE", SqlDbType.Float).Value = 0;

                        cmdInsCons.Parameters.Add("@NUM_VALOR_INTERMEDIACAO_FEE", SqlDbType.Float).Value = Funcoes.strToDouble(o["payload"]["object"]["fees"].ToString()) / 100;
                        cmdInsCons.Parameters.Add("@NUM_VALOR_LIQUIDO", SqlDbType.Float).Value = (Funcoes.strToDouble(o["payload"]["object"]["amount"].ToString()) / 100) - (Funcoes.strToDouble(o["payload"]["object"]["fees"].ToString()) / 100);
                        cmdInsCons.Parameters.Add("@NUM_VALOR_EXTRAS", SqlDbType.Float).Value = 0;

                        cmdInsCons.Parameters.Add("@DTA_DATA_DEPOSITO", SqlDbType.DateTime).Value = Convert.ToDateTime(o["payload"]["object"]["created_at"].ToString());

                        cmdInsCons.Parameters.Add("@NUM_ITENS", SqlDbType.Int).Value = 1;

                        cmdInsCons.Parameters.Add("@NOM_FONTE_CANCELAMENTO", SqlDbType.VarChar).Value = "";
                        cmdInsCons.Parameters.Add("@NOM_ORIGEM", SqlDbType.VarChar).Value = o["payload"]["object"]["gateway_authorizer"].ToString();

                        try { cmdInsCons.Parameters.Add("@NOM_CLIENTE", SqlDbType.VarChar).Value = o["payload"]["object"]["payment_method"]["holder_name"].ToString(); }
                        catch { }

                        // Inserido em 21/06/2024 -----------------------------------------------------------------------
                        // Dados do cartao no caso de existência

                        //if (o["payload"]["object"]["payment_method"].Contains("first4_digits") == true) { cmdInsCons.Parameters.Add("@NOM_CARTAO_4_PRIMEIROS_DIGIROS", SqlDbType.VarChar).Value = o["payload"]["object"]["payment_method"]["first4_digits"].ToString(); }
                        //if (o["payload"]["object"]["payment_method"].Contains("last4_digits") == true) { cmdInsCons.Parameters.Add("@NOM_CARTAO_4_ULTIMOS_DIGITOS", SqlDbType.VarChar).Value = o["payload"]["object"]["payment_method"]["last4_digits"].ToString(); }
                        //if (o["payload"]["object"]["payment_method"].Contains("expiration_month") == true) { cmdInsCons.Parameters.Add("@NOM_CARTAO_MES", SqlDbType.VarChar).Value = o["payload"]["object"]["payment_method"]["expiration_month"].ToString(); }
                        //if (o["payload"]["object"]["payment_method"].Contains("expiration_year") == true) { cmdInsCons.Parameters.Add("@NOM_CARTAO_ANO", SqlDbType.VarChar).Value = o["payload"]["object"]["payment_method"]["expiration_year"].ToString(); }

                        cmdInsCons.Parameters.Add("@NOM_NUMERO_TRANSACAO", SqlDbType.VarChar).Value = o["payload"]["object"]["transaction_number"].ToString();
                        cmdInsCons.Parameters.Add("@NOM_RECIBO", SqlDbType.VarChar).Value = o["payload"]["object"]["sales_receipt"].ToString();
                        // Carregar os dados do Recibo //




                        // Dados do Boleto

                        //if (o["payload"]["object"]["payment_method"].Contains("barcode") == true) { cmdInsCons.Parameters.Add("@NOM_CODIGO_BARRAS", SqlDbType.VarChar).Value = o["payload"]["object"]["payment_method"]["barcode"].ToString(); }
                        //if (o["payload"]["object"]["payment_method"].Contains("url") == true) { cmdInsCons.Parameters.Add("@NOM_URL_BOLETO", SqlDbType.VarChar).Value = o["payload"]["object"]["payment_method"]["url"].ToString(); }
                        //if (o["payload"]["object"]["payment_method"].Contains("document_number") == true) { cmdInsCons.Parameters.Add("@NOM_NUMERO_DOCUMENTO", SqlDbType.VarChar).Value = o["payload"]["object"]["payment_method"]["document_number"].ToString(); }
                        //if (o["payload"]["object"]["payment_method"].Contains("expiration_date") == true) { cmdInsCons.Parameters.Add("@NOM_DATA_VENCIMENTO", SqlDbType.VarChar).Value = o["payload"]["object"]["payment_method"]["expiration_date"].ToString(); }

                        // Dados do PIX

                        //if (o["payload"]["object"]["payment_method"].Contains("qr_code") == true) { cmdInsCons.Parameters.Add("@NOM_QRCODE", SqlDbType.VarChar).Value = o["payload"]["object"]["payment_method"]["qr_code"]["emv"].ToString(); }

                        // -----------------------------------------------------------------------------------------------


                        cmdInsCons.Parameters.Add("@NOM_AUTORIZACAO", SqlDbType.VarChar).Value = o["payload"]["object"]["transaction_number"].ToString();
                        cmdInsCons.Parameters.Add("@DES_DESCRICAO", SqlDbType.Text).Value = o["payload"]["object"]["payment_type"].ToString();

                        if (o["payload"]["object"]["history"].Count() > 0)
                        {
                            for (int i = 0; i < o["payload"]["object"]["history"].Count(); i++)
                            {
                                // Inserção de dados do Histórico

                                SqlConnection connInsConsHistorico = new SqlConnection(sConexao);
                                connInsConsHistorico.Open();
                                SqlCommand cmdInsConsHistorico = new SqlCommand("dbo.stp_transacoes_historico_ins", connInsConsHistorico);
                                cmdInsConsHistorico.CommandType = CommandType.StoredProcedure;
                                cmdInsConsHistorico.Parameters.Add("@flg_operacao", SqlDbType.Char).Value = 'I';
                                cmdInsConsHistorico.Parameters.Add("@COD_ID_PESSOA_LICENCIADO", SqlDbType.Int).Value = iLicenciado;
                                cmdInsConsHistorico.Parameters.Add("@NOM_CODE", SqlDbType.VarChar).Value = o["payload"]["object"]["id"].ToString();
                                cmdInsConsHistorico.Parameters.Add("@DTA_DATA", SqlDbType.DateTime).Value = DateTime.Now;
                                cmdInsConsHistorico.Parameters.Add("@NOM_TIPO", SqlDbType.VarChar).Value = o["payload"]["object"]["history"][i]["operation_type"].ToString();
                                cmdInsConsHistorico.Parameters.Add("@NOM_STATUS", SqlDbType.VarChar).Value = o["payload"]["object"]["history"][i]["status"].ToString();
                                cmdInsConsHistorico.Parameters.Add("@NUM_VALOR", SqlDbType.Float).Value = Funcoes.strToDouble(o["payload"]["object"]["history"][i]["amount"].ToString());
                                cmdInsConsHistorico.Parameters.Add("@NOM_CODIGO_RESPOSTA", SqlDbType.VarChar).Value = o["payload"]["object"]["history"][i]["response_code"].ToString();
                                cmdInsConsHistorico.Parameters.Add("@NOM_DESCRICAO_RESPOSTA", SqlDbType.VarChar).Value = o["payload"]["object"]["history"][i]["response_message"].ToString();
                                cmdInsConsHistorico.Parameters.Add("@NOM_ID_AUTORIZACAO", SqlDbType.VarChar).Value = o["payload"]["object"]["history"][i]["authorizer_id"].ToString();
                                cmdInsConsHistorico.Parameters.Add("@NOM_NSU_AUTORIZACAO", SqlDbType.VarChar).Value = o["payload"]["object"]["history"][i]["authorization_nsu"].ToString();
                                cmdInsConsHistorico.Parameters.Add("@DTA_DATA_CRIACAO", SqlDbType.DateTime).Value = Convert.ToDateTime(o["payload"]["object"]["history"][i]["created_at"].ToString());
                                cmdInsConsHistorico.ExecuteNonQuery();
                                connInsConsHistorico.Close();
                                connInsConsHistorico.Dispose();
                            }
                        }

                        // Carregar os dados do recibo ----------------------------------------------------------

                        if (o["payload"]["object"]["sales_receipt"].ToString().Trim() != "")
                        {
                            string jsonRecibo = zoop.DetalhesRecibo(o["payload"]["object"]["sales_receipt"].ToString());

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

                        // Carregar os recebíveis ----------------------------------------------------------

                        string jsonRecebiveis = zoop.DetalhesRecebiveis(o["payload"]["object"]["id"].ToString());
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
                                cmdInsConsRecebiveis.Parameters.Add("@COD_ID_PESSOA_LICENCIADO", SqlDbType.Int).Value = iLicenciado;
                                cmdInsConsRecebiveis.Parameters.Add("@NOM_CODE_TRANSACAO", SqlDbType.VarChar).Value = o["payload"]["object"]["id"].ToString();
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
                        // --------------------------------------------------------------------------------------


                    }
                    cmdInsCons.ExecuteNonQuery();
                    connInsCons.Close();
                    connInsCons.Dispose();
                }

                // Transaction.cancelled

                if ((sType2.Contains("transaction.canceled") == true) || (sType2.Contains("buyer.transaction.canceled") == true))
                {
                    // Transacoes
                    SqlConnection connInsCons = new SqlConnection(sConexao);
                    connInsCons.Open();
                    SqlCommand cmdInsCons = new SqlCommand("dbo.stp_transacoes_ins", connInsCons);
                    cmdInsCons.CommandType = CommandType.StoredProcedure;
                    cmdInsCons.Parameters.Add("@flg_operacao", SqlDbType.Char).Value = 'I';


                    int iEstabelecimento = 0;
                    int iLicenciado = 0;

                    SqlConnection mySelCadastroEstabelecimento = new SqlConnection(sConexao);
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

                    if (bodyText.ToString().Trim() != "")
                    {
                        string json = bodyText.ToString();
                        JObject o = JObject.Parse(json);
                        string sType = o["type"].ToString();

                        cmdInsCons.Parameters.Add("@DTA_DATA", SqlDbType.DateTime).Value = Convert.ToDateTime(o["payload"]["object"]["created_at"].ToString());
                        cmdInsCons.Parameters.Add("@NOM_CODE", SqlDbType.VarChar).Value = o["payload"]["object"]["id"].ToString();
                        cmdInsCons.Parameters.Add("@NUM_TOKEN", SqlDbType.VarChar).Value = o["payload"]["object"]["on_behalf_of"].ToString();

                        cmdInsCons.Parameters.Add("@COD_ID_TIPO", SqlDbType.Int).Value = 1;
                        cmdInsCons.Parameters.Add("@NOM_STATUS", SqlDbType.VarChar).Value = o["payload"]["object"]["status"].ToString();

                        if ((o["payload"]["object"]["point_of_sale"]["identification_number"].ToString().Trim() == "") || (o["payload"]["object"]["point_of_sale"]["identification_number"].ToString().Trim() == "null"))
                        {
                            cmdInsCons.Parameters.Add("@FLG_PRESENCIAL_ONLINE", SqlDbType.Char).Value = "O";
                        }
                        else
                        {
                            cmdInsCons.Parameters.Add("@FLG_PRESENCIAL_ONLINE", SqlDbType.Char).Value = "P";
                        }

                        cmdInsCons.Parameters.Add("@NOM_TIPO_PAGAMENTO", SqlDbType.VarChar).Value = o["payload"]["object"]["payment_type"].ToString();

                        if ((o["payload"]["object"]["payment_type"].ToString() == "pix"))
                        {
                            cmdInsCons.Parameters.Add("@NOM_CODIGO_PAGAMENTO", SqlDbType.VarChar).Value = o["payload"]["object"]["payment_method"]["provider"].ToString();
                            cmdInsCons.Parameters.Add("@NUM_PARCELAS", SqlDbType.Int).Value = 1;
                            cmdInsCons.Parameters.Add("@NOM_QRCODE", SqlDbType.VarChar).Value = o["payload"]["object"]["payment_method"]["qr_code"]["emv"].ToString();

                        }
                        if ((o["payload"]["object"]["payment_type"].ToString() == "boleto"))
                        {
                            cmdInsCons.Parameters.Add("@NOM_CODIGO_PAGAMENTO", SqlDbType.VarChar).Value = o["payload"]["object"]["payment_method"]["resource"].ToString();
                            cmdInsCons.Parameters.Add("@NUM_PARCELAS", SqlDbType.Int).Value = 1;

                            cmdInsCons.Parameters.Add("@NOM_CODIGO_BARRAS", SqlDbType.VarChar).Value = o["payload"]["object"]["payment_method"]["barcode"].ToString();
                            cmdInsCons.Parameters.Add("@NOM_URL_BOLETO", SqlDbType.VarChar).Value = o["payload"]["object"]["payment_method"]["url"].ToString();
                            cmdInsCons.Parameters.Add("@NOM_NUMERO_DOCUMENTO", SqlDbType.VarChar).Value = o["payload"]["object"]["payment_method"]["document_number"].ToString();
                            cmdInsCons.Parameters.Add("@NOM_DATA_VENCIMENTO", SqlDbType.VarChar).Value = o["payload"]["object"]["payment_method"]["expiration_date"].ToString();

                        }


                        if ((o["payload"]["object"]["payment_type"].ToString() == "credit"))
                        {
                            cmdInsCons.Parameters.Add("@NOM_CODIGO_PAGAMENTO", SqlDbType.VarChar).Value = o["payload"]["object"]["payment_method"]["resource"].ToString() + " " + o["payload"]["object"]["payment_method"]["card_brand"].ToString();

                            try { cmdInsCons.Parameters.Add("@NUM_PARCELAS", SqlDbType.Int).Value = Funcoes.strToInt(o["payload"]["object"]["installment_plan"]["number_installments"].ToString()); }
                            catch { }

                            //cmdInsCons.Parameters.Add("@NUM_PARCELAS", SqlDbType.Int).Value = Funcoes.strToInt(o["payload"]["object"]["installment_plan"]["number_installments"].ToString());

                            cmdInsCons.Parameters.Add("@NOM_CARTAO_4_PRIMEIROS_DIGIROS", SqlDbType.VarChar).Value = o["payload"]["object"]["payment_method"]["first4_digits"].ToString();
                            cmdInsCons.Parameters.Add("@NOM_CARTAO_4_ULTIMOS_DIGITOS", SqlDbType.VarChar).Value = o["payload"]["object"]["payment_method"]["last4_digits"].ToString();
                            cmdInsCons.Parameters.Add("@NOM_CARTAO_MES", SqlDbType.VarChar).Value = o["payload"]["object"]["payment_method"]["expiration_month"].ToString();
                            cmdInsCons.Parameters.Add("@NOM_CARTAO_ANO", SqlDbType.VarChar).Value = o["payload"]["object"]["payment_method"]["expiration_year"].ToString();
                        }

                        if ((o["payload"]["object"]["payment_type"].ToString() == "debit"))
                        {
                            cmdInsCons.Parameters.Add("@NOM_CODIGO_PAGAMENTO", SqlDbType.VarChar).Value = o["payload"]["object"]["payment_method"]["resource"].ToString() + " " + o["payload"]["object"]["payment_method"]["card_brand"].ToString();
                            cmdInsCons.Parameters.Add("@NUM_PARCELAS", SqlDbType.Int).Value = 1;

                            cmdInsCons.Parameters.Add("@NOM_CARTAO_4_PRIMEIROS_DIGIROS", SqlDbType.VarChar).Value = o["payload"]["object"]["payment_method"]["first4_digits"].ToString();
                            cmdInsCons.Parameters.Add("@NOM_CARTAO_4_ULTIMOS_DIGITOS", SqlDbType.VarChar).Value = o["payload"]["object"]["payment_method"]["last4_digits"].ToString();
                            cmdInsCons.Parameters.Add("@NOM_CARTAO_MES", SqlDbType.VarChar).Value = o["payload"]["object"]["payment_method"]["expiration_month"].ToString();
                            cmdInsCons.Parameters.Add("@NOM_CARTAO_ANO", SqlDbType.VarChar).Value = o["payload"]["object"]["payment_method"]["expiration_year"].ToString();
                        }



                        cmdInsCons.Parameters.Add("@NUM_VALOR_BRUTO", SqlDbType.Float).Value = Funcoes.strToDouble(o["payload"]["object"]["amount"].ToString()) / 100;
                        cmdInsCons.Parameters.Add("@NUM_VALOR_DESCONTO", SqlDbType.Float).Value = 0;
                        cmdInsCons.Parameters.Add("@NUM_VALOR_INTERMEDIACAO_RATE", SqlDbType.Float).Value = 0;

                        cmdInsCons.Parameters.Add("@NUM_VALOR_INTERMEDIACAO_FEE", SqlDbType.Float).Value = Funcoes.strToDouble(o["payload"]["object"]["fees"].ToString()) / 100;
                        cmdInsCons.Parameters.Add("@NUM_VALOR_LIQUIDO", SqlDbType.Float).Value = (Funcoes.strToDouble(o["payload"]["object"]["amount"].ToString()) / 100) - (Funcoes.strToDouble(o["payload"]["object"]["fees"].ToString()) / 100);
                        cmdInsCons.Parameters.Add("@NUM_VALOR_EXTRAS", SqlDbType.Float).Value = 0;

                        cmdInsCons.Parameters.Add("@DTA_DATA_DEPOSITO", SqlDbType.DateTime).Value = Convert.ToDateTime(o["payload"]["object"]["created_at"].ToString());

                        cmdInsCons.Parameters.Add("@NUM_ITENS", SqlDbType.Int).Value = 1;

                        cmdInsCons.Parameters.Add("@NOM_FONTE_CANCELAMENTO", SqlDbType.VarChar).Value = "";
                        cmdInsCons.Parameters.Add("@NOM_ORIGEM", SqlDbType.VarChar).Value = o["payload"]["object"]["gateway_authorizer"].ToString();

                        try { cmdInsCons.Parameters.Add("@NOM_CLIENTE", SqlDbType.VarChar).Value = o["payload"]["object"]["payment_method"]["holder_name"].ToString(); }
                        catch { }

                        // Inserido em 21/06/2024 -----------------------------------------------------------------------
                        // Dados do cartao no caso de existência

                        //if (o["payload"]["object"]["payment_method"].Contains("first4_digits") == true) { cmdInsCons.Parameters.Add("@NOM_CARTAO_4_PRIMEIROS_DIGIROS", SqlDbType.VarChar).Value = o["payload"]["object"]["payment_method"]["first4_digits"].ToString(); }
                        //if (o["payload"]["object"]["payment_method"].Contains("last4_digits") == true) { cmdInsCons.Parameters.Add("@NOM_CARTAO_4_ULTIMOS_DIGITOS", SqlDbType.VarChar).Value = o["payload"]["object"]["payment_method"]["last4_digits"].ToString(); }
                        //if (o["payload"]["object"]["payment_method"].Contains("expiration_month") == true) { cmdInsCons.Parameters.Add("@NOM_CARTAO_MES", SqlDbType.VarChar).Value = o["payload"]["object"]["payment_method"]["expiration_month"].ToString(); }
                        //if (o["payload"]["object"]["payment_method"].Contains("expiration_year") == true) { cmdInsCons.Parameters.Add("@NOM_CARTAO_ANO", SqlDbType.VarChar).Value = o["payload"]["object"]["payment_method"]["expiration_year"].ToString(); }

                        cmdInsCons.Parameters.Add("@NOM_NUMERO_TRANSACAO", SqlDbType.VarChar).Value = o["payload"]["object"]["transaction_number"].ToString();
                        cmdInsCons.Parameters.Add("@NOM_RECIBO", SqlDbType.VarChar).Value = o["payload"]["object"]["sales_receipt"].ToString();

                        // Dados do Boleto

                        //if (o["payload"]["object"]["payment_method"].Contains("barcode") == true) { cmdInsCons.Parameters.Add("@NOM_CODIGO_BARRAS", SqlDbType.VarChar).Value = o["payload"]["object"]["payment_method"]["barcode"].ToString(); }
                        //if (o["payload"]["object"]["payment_method"].Contains("url") == true) { cmdInsCons.Parameters.Add("@NOM_URL_BOLETO", SqlDbType.VarChar).Value = o["payload"]["object"]["payment_method"]["url"].ToString(); }
                        //if (o["payload"]["object"]["payment_method"].Contains("document_number") == true) { cmdInsCons.Parameters.Add("@NOM_NUMERO_DOCUMENTO", SqlDbType.VarChar).Value = o["payload"]["object"]["payment_method"]["document_number"].ToString(); }
                        //if (o["payload"]["object"]["payment_method"].Contains("expiration_date") == true) { cmdInsCons.Parameters.Add("@NOM_DATA_VENCIMENTO", SqlDbType.VarChar).Value = o["payload"]["object"]["payment_method"]["expiration_date"].ToString(); }

                        // Dados do PIX

                        //if (o["payload"]["object"]["payment_method"].Contains("qr_code") == true) { cmdInsCons.Parameters.Add("@NOM_QRCODE", SqlDbType.VarChar).Value = o["payload"]["object"]["payment_method"]["qr_code"]["emv"].ToString(); }

                        // -----------------------------------------------------------------------------------------------


                        cmdInsCons.Parameters.Add("@NOM_AUTORIZACAO", SqlDbType.VarChar).Value = o["payload"]["object"]["transaction_number"].ToString();
                        cmdInsCons.Parameters.Add("@DES_DESCRICAO", SqlDbType.Text).Value = o["payload"]["object"]["payment_type"].ToString();

                        if (o["payload"]["object"]["history"].Count() > 0)
                        {
                            for (int i = 0; i < o["payload"]["object"]["history"].Count(); i++)
                            {
                                // Inserção de dados do Histórico

                                SqlConnection connInsConsHistorico = new SqlConnection(sConexao);
                                connInsConsHistorico.Open();
                                SqlCommand cmdInsConsHistorico = new SqlCommand("dbo.stp_transacoes_historico_ins", connInsConsHistorico);
                                cmdInsConsHistorico.CommandType = CommandType.StoredProcedure;
                                cmdInsConsHistorico.Parameters.Add("@flg_operacao", SqlDbType.Char).Value = 'I';
                                cmdInsConsHistorico.Parameters.Add("@COD_ID_PESSOA_LICENCIADO", SqlDbType.Int).Value = iLicenciado;
                                cmdInsConsHistorico.Parameters.Add("@NOM_CODE", SqlDbType.VarChar).Value = o["payload"]["object"]["id"].ToString();
                                cmdInsConsHistorico.Parameters.Add("@DTA_DATA", SqlDbType.DateTime).Value = DateTime.Now;
                                cmdInsConsHistorico.Parameters.Add("@NOM_TIPO", SqlDbType.VarChar).Value = o["payload"]["object"]["history"][i]["operation_type"].ToString();
                                cmdInsConsHistorico.Parameters.Add("@NOM_STATUS", SqlDbType.VarChar).Value = o["payload"]["object"]["history"][i]["status"].ToString();
                                cmdInsConsHistorico.Parameters.Add("@NUM_VALOR", SqlDbType.Float).Value = Funcoes.strToDouble(o["payload"]["object"]["history"][i]["amount"].ToString());
                                cmdInsConsHistorico.Parameters.Add("@NOM_CODIGO_RESPOSTA", SqlDbType.VarChar).Value = o["payload"]["object"]["history"][i]["response_code"].ToString();
                                cmdInsConsHistorico.Parameters.Add("@NOM_DESCRICAO_RESPOSTA", SqlDbType.VarChar).Value = o["payload"]["object"]["history"][i]["response_message"].ToString();
                                cmdInsConsHistorico.Parameters.Add("@NOM_ID_AUTORIZACAO", SqlDbType.VarChar).Value = o["payload"]["object"]["history"][i]["authorizer_id"].ToString();
                                cmdInsConsHistorico.Parameters.Add("@NOM_NSU_AUTORIZACAO", SqlDbType.VarChar).Value = o["payload"]["object"]["history"][i]["authorization_nsu"].ToString();
                                cmdInsConsHistorico.Parameters.Add("@DTA_DATA_CRIACAO", SqlDbType.DateTime).Value = Convert.ToDateTime(o["payload"]["object"]["history"][i]["created_at"].ToString());
                                cmdInsConsHistorico.ExecuteNonQuery();
                                connInsConsHistorico.Close();
                                connInsConsHistorico.Dispose();
                            }
                        }

                        // Carregar os dados do recibo ----------------------------------------------------------

                        if (o["payload"]["object"]["sales_receipt"].ToString().Trim() != "")
                        {
                            string jsonRecibo = zoop.DetalhesRecibo(o["payload"]["object"]["sales_receipt"].ToString());
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

                        // Carregar os recebíveis ----------------------------------------------------------

                        string jsonRecebiveis = zoop.DetalhesRecebiveis(o["payload"]["object"]["id"].ToString());
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
                                cmdInsConsRecebiveis.Parameters.Add("@COD_ID_PESSOA_LICENCIADO", SqlDbType.Int).Value = iLicenciado;
                                cmdInsConsRecebiveis.Parameters.Add("@NOM_CODE_TRANSACAO", SqlDbType.VarChar).Value = o["payload"]["object"]["id"].ToString();
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
                        // --------------------------------------------------------------------------------------


                    }
                    cmdInsCons.ExecuteNonQuery();
                    connInsCons.Close();
                    connInsCons.Dispose();
                }

                // transaction.reversed

                if ((sType2.Contains("transaction.reversed") == true) || (sType2.Contains("buyer.transaction.reversed") == true))
                {
                    // Transacoes
                    SqlConnection connInsCons = new SqlConnection(sConexao);
                    connInsCons.Open();
                    SqlCommand cmdInsCons = new SqlCommand("dbo.stp_transacoes_ins", connInsCons);
                    cmdInsCons.CommandType = CommandType.StoredProcedure;
                    cmdInsCons.Parameters.Add("@flg_operacao", SqlDbType.Char).Value = 'I';


                    int iEstabelecimento = 0;
                    int iLicenciado = 0;

                    SqlConnection mySelCadastroEstabelecimento = new SqlConnection(sConexao);
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

                    if (bodyText.ToString().Trim() != "")
                    {
                        string json = bodyText.ToString();
                        JObject o = JObject.Parse(json);
                        string sType = o["type"].ToString();

                        cmdInsCons.Parameters.Add("@DTA_DATA", SqlDbType.DateTime).Value = Convert.ToDateTime(o["payload"]["object"]["created_at"].ToString());
                        cmdInsCons.Parameters.Add("@NOM_CODE", SqlDbType.VarChar).Value = o["payload"]["object"]["id"].ToString();
                        cmdInsCons.Parameters.Add("@NUM_TOKEN", SqlDbType.VarChar).Value = o["payload"]["object"]["on_behalf_of"].ToString();

                        cmdInsCons.Parameters.Add("@COD_ID_TIPO", SqlDbType.Int).Value = 1;
                        cmdInsCons.Parameters.Add("@NOM_STATUS", SqlDbType.VarChar).Value = o["payload"]["object"]["status"].ToString();

                        if ((o["payload"]["object"]["point_of_sale"]["identification_number"].ToString().Trim() == "") || (o["payload"]["object"]["point_of_sale"]["identification_number"].ToString().Trim() == "null"))
                        {
                            cmdInsCons.Parameters.Add("@FLG_PRESENCIAL_ONLINE", SqlDbType.Char).Value = "O";
                        }
                        else
                        {
                            cmdInsCons.Parameters.Add("@FLG_PRESENCIAL_ONLINE", SqlDbType.Char).Value = "P";
                        }

                        cmdInsCons.Parameters.Add("@NOM_TIPO_PAGAMENTO", SqlDbType.VarChar).Value = o["payload"]["object"]["payment_type"].ToString();

                        if ((o["payload"]["object"]["payment_type"].ToString() == "pix"))
                        {
                            cmdInsCons.Parameters.Add("@NOM_CODIGO_PAGAMENTO", SqlDbType.VarChar).Value = o["payload"]["object"]["payment_method"]["provider"].ToString();
                            cmdInsCons.Parameters.Add("@NUM_PARCELAS", SqlDbType.Int).Value = 1;
                            cmdInsCons.Parameters.Add("@NOM_QRCODE", SqlDbType.VarChar).Value = o["payload"]["object"]["payment_method"]["qr_code"]["emv"].ToString();

                        }
                        if ((o["payload"]["object"]["payment_type"].ToString() == "boleto"))
                        {
                            cmdInsCons.Parameters.Add("@NOM_CODIGO_PAGAMENTO", SqlDbType.VarChar).Value = o["payload"]["object"]["payment_method"]["resource"].ToString();
                            cmdInsCons.Parameters.Add("@NUM_PARCELAS", SqlDbType.Int).Value = 1;

                            cmdInsCons.Parameters.Add("@NOM_CODIGO_BARRAS", SqlDbType.VarChar).Value = o["payload"]["object"]["payment_method"]["barcode"].ToString();
                            cmdInsCons.Parameters.Add("@NOM_URL_BOLETO", SqlDbType.VarChar).Value = o["payload"]["object"]["payment_method"]["url"].ToString();
                            cmdInsCons.Parameters.Add("@NOM_NUMERO_DOCUMENTO", SqlDbType.VarChar).Value = o["payload"]["object"]["payment_method"]["document_number"].ToString();
                            cmdInsCons.Parameters.Add("@NOM_DATA_VENCIMENTO", SqlDbType.VarChar).Value = o["payload"]["object"]["payment_method"]["expiration_date"].ToString();

                        }


                        if ((o["payload"]["object"]["payment_type"].ToString() == "credit"))
                        {
                            cmdInsCons.Parameters.Add("@NOM_CODIGO_PAGAMENTO", SqlDbType.VarChar).Value = o["payload"]["object"]["payment_method"]["resource"].ToString() + " " + o["payload"]["object"]["payment_method"]["card_brand"].ToString();

                            try { cmdInsCons.Parameters.Add("@NUM_PARCELAS", SqlDbType.Int).Value = Funcoes.strToInt(o["payload"]["object"]["installment_plan"]["number_installments"].ToString()); }
                            catch { }

                            //cmdInsCons.Parameters.Add("@NUM_PARCELAS", SqlDbType.Int).Value = Funcoes.strToInt(o["payload"]["object"]["installment_plan"]["number_installments"].ToString());

                            cmdInsCons.Parameters.Add("@NOM_CARTAO_4_PRIMEIROS_DIGIROS", SqlDbType.VarChar).Value = o["payload"]["object"]["payment_method"]["first4_digits"].ToString();
                            cmdInsCons.Parameters.Add("@NOM_CARTAO_4_ULTIMOS_DIGITOS", SqlDbType.VarChar).Value = o["payload"]["object"]["payment_method"]["last4_digits"].ToString();
                            cmdInsCons.Parameters.Add("@NOM_CARTAO_MES", SqlDbType.VarChar).Value = o["payload"]["object"]["payment_method"]["expiration_month"].ToString();
                            cmdInsCons.Parameters.Add("@NOM_CARTAO_ANO", SqlDbType.VarChar).Value = o["payload"]["object"]["payment_method"]["expiration_year"].ToString();
                        }

                        if ((o["payload"]["object"]["payment_type"].ToString() == "debit"))
                        {
                            cmdInsCons.Parameters.Add("@NOM_CODIGO_PAGAMENTO", SqlDbType.VarChar).Value = o["payload"]["object"]["payment_method"]["resource"].ToString() + " " + o["payload"]["object"]["payment_method"]["card_brand"].ToString();
                            cmdInsCons.Parameters.Add("@NUM_PARCELAS", SqlDbType.Int).Value = 1;

                            cmdInsCons.Parameters.Add("@NOM_CARTAO_4_PRIMEIROS_DIGIROS", SqlDbType.VarChar).Value = o["payload"]["object"]["payment_method"]["first4_digits"].ToString();
                            cmdInsCons.Parameters.Add("@NOM_CARTAO_4_ULTIMOS_DIGITOS", SqlDbType.VarChar).Value = o["payload"]["object"]["payment_method"]["last4_digits"].ToString();
                            cmdInsCons.Parameters.Add("@NOM_CARTAO_MES", SqlDbType.VarChar).Value = o["payload"]["object"]["payment_method"]["expiration_month"].ToString();
                            cmdInsCons.Parameters.Add("@NOM_CARTAO_ANO", SqlDbType.VarChar).Value = o["payload"]["object"]["payment_method"]["expiration_year"].ToString();
                        }



                        cmdInsCons.Parameters.Add("@NUM_VALOR_BRUTO", SqlDbType.Float).Value = Funcoes.strToDouble(o["payload"]["object"]["amount"].ToString()) / 100;
                        cmdInsCons.Parameters.Add("@NUM_VALOR_DESCONTO", SqlDbType.Float).Value = 0;
                        cmdInsCons.Parameters.Add("@NUM_VALOR_INTERMEDIACAO_RATE", SqlDbType.Float).Value = 0;

                        cmdInsCons.Parameters.Add("@NUM_VALOR_INTERMEDIACAO_FEE", SqlDbType.Float).Value = Funcoes.strToDouble(o["payload"]["object"]["fees"].ToString()) / 100;
                        cmdInsCons.Parameters.Add("@NUM_VALOR_LIQUIDO", SqlDbType.Float).Value = (Funcoes.strToDouble(o["payload"]["object"]["amount"].ToString()) / 100) - (Funcoes.strToDouble(o["payload"]["object"]["fees"].ToString()) / 100);
                        cmdInsCons.Parameters.Add("@NUM_VALOR_EXTRAS", SqlDbType.Float).Value = 0;

                        cmdInsCons.Parameters.Add("@DTA_DATA_DEPOSITO", SqlDbType.DateTime).Value = Convert.ToDateTime(o["payload"]["object"]["created_at"].ToString());

                        cmdInsCons.Parameters.Add("@NUM_ITENS", SqlDbType.Int).Value = 1;

                        cmdInsCons.Parameters.Add("@NOM_FONTE_CANCELAMENTO", SqlDbType.VarChar).Value = "";
                        cmdInsCons.Parameters.Add("@NOM_ORIGEM", SqlDbType.VarChar).Value = o["payload"]["object"]["gateway_authorizer"].ToString();

                        try { cmdInsCons.Parameters.Add("@NOM_CLIENTE", SqlDbType.VarChar).Value = o["payload"]["object"]["payment_method"]["holder_name"].ToString(); }
                        catch { }

                        // Inserido em 21/06/2024 -----------------------------------------------------------------------
                        // Dados do cartao no caso de existência

                        //if (o["payload"]["object"]["payment_method"].Contains("first4_digits") == true) { cmdInsCons.Parameters.Add("@NOM_CARTAO_4_PRIMEIROS_DIGIROS", SqlDbType.VarChar).Value = o["payload"]["object"]["payment_method"]["first4_digits"].ToString(); }
                        //if (o["payload"]["object"]["payment_method"].Contains("last4_digits") == true) { cmdInsCons.Parameters.Add("@NOM_CARTAO_4_ULTIMOS_DIGITOS", SqlDbType.VarChar).Value = o["payload"]["object"]["payment_method"]["last4_digits"].ToString(); }
                        //if (o["payload"]["object"]["payment_method"].Contains("expiration_month") == true) { cmdInsCons.Parameters.Add("@NOM_CARTAO_MES", SqlDbType.VarChar).Value = o["payload"]["object"]["payment_method"]["expiration_month"].ToString(); }
                        //if (o["payload"]["object"]["payment_method"].Contains("expiration_year") == true) { cmdInsCons.Parameters.Add("@NOM_CARTAO_ANO", SqlDbType.VarChar).Value = o["payload"]["object"]["payment_method"]["expiration_year"].ToString(); }

                        cmdInsCons.Parameters.Add("@NOM_NUMERO_TRANSACAO", SqlDbType.VarChar).Value = o["payload"]["object"]["transaction_number"].ToString();
                        cmdInsCons.Parameters.Add("@NOM_RECIBO", SqlDbType.VarChar).Value = o["payload"]["object"]["sales_receipt"].ToString();

                        // Dados do Boleto

                        //if (o["payload"]["object"]["payment_method"].Contains("barcode") == true) { cmdInsCons.Parameters.Add("@NOM_CODIGO_BARRAS", SqlDbType.VarChar).Value = o["payload"]["object"]["payment_method"]["barcode"].ToString(); }
                        //if (o["payload"]["object"]["payment_method"].Contains("url") == true) { cmdInsCons.Parameters.Add("@NOM_URL_BOLETO", SqlDbType.VarChar).Value = o["payload"]["object"]["payment_method"]["url"].ToString(); }
                        //if (o["payload"]["object"]["payment_method"].Contains("document_number") == true) { cmdInsCons.Parameters.Add("@NOM_NUMERO_DOCUMENTO", SqlDbType.VarChar).Value = o["payload"]["object"]["payment_method"]["document_number"].ToString(); }
                        //if (o["payload"]["object"]["payment_method"].Contains("expiration_date") == true) { cmdInsCons.Parameters.Add("@NOM_DATA_VENCIMENTO", SqlDbType.VarChar).Value = o["payload"]["object"]["payment_method"]["expiration_date"].ToString(); }

                        // Dados do PIX

                        //if (o["payload"]["object"]["payment_method"].Contains("qr_code") == true) { cmdInsCons.Parameters.Add("@NOM_QRCODE", SqlDbType.VarChar).Value = o["payload"]["object"]["payment_method"]["qr_code"]["emv"].ToString(); }

                        // -----------------------------------------------------------------------------------------------


                        cmdInsCons.Parameters.Add("@NOM_AUTORIZACAO", SqlDbType.VarChar).Value = o["payload"]["object"]["transaction_number"].ToString();
                        cmdInsCons.Parameters.Add("@DES_DESCRICAO", SqlDbType.Text).Value = o["payload"]["object"]["payment_type"].ToString();

                        if (o["payload"]["object"]["history"].Count() > 0)
                        {
                            for (int i = 0; i < o["payload"]["object"]["history"].Count(); i++)
                            {
                                // Inserção de dados do Histórico

                                SqlConnection connInsConsHistorico = new SqlConnection(sConexao);
                                connInsConsHistorico.Open();
                                SqlCommand cmdInsConsHistorico = new SqlCommand("dbo.stp_transacoes_historico_ins", connInsConsHistorico);
                                cmdInsConsHistorico.CommandType = CommandType.StoredProcedure;
                                cmdInsConsHistorico.Parameters.Add("@flg_operacao", SqlDbType.Char).Value = 'I';
                                cmdInsConsHistorico.Parameters.Add("@COD_ID_PESSOA_LICENCIADO", SqlDbType.Int).Value = iLicenciado;
                                cmdInsConsHistorico.Parameters.Add("@NOM_CODE", SqlDbType.VarChar).Value = o["payload"]["object"]["id"].ToString();
                                cmdInsConsHistorico.Parameters.Add("@DTA_DATA", SqlDbType.DateTime).Value = DateTime.Now;
                                cmdInsConsHistorico.Parameters.Add("@NOM_TIPO", SqlDbType.VarChar).Value = o["payload"]["object"]["history"][i]["operation_type"].ToString();
                                cmdInsConsHistorico.Parameters.Add("@NOM_STATUS", SqlDbType.VarChar).Value = o["payload"]["object"]["history"][i]["status"].ToString();
                                cmdInsConsHistorico.Parameters.Add("@NUM_VALOR", SqlDbType.Float).Value = Funcoes.strToDouble(o["payload"]["object"]["history"][i]["amount"].ToString());
                                cmdInsConsHistorico.Parameters.Add("@NOM_CODIGO_RESPOSTA", SqlDbType.VarChar).Value = o["payload"]["object"]["history"][i]["response_code"].ToString();
                                cmdInsConsHistorico.Parameters.Add("@NOM_DESCRICAO_RESPOSTA", SqlDbType.VarChar).Value = o["payload"]["object"]["history"][i]["response_message"].ToString();
                                cmdInsConsHistorico.Parameters.Add("@NOM_ID_AUTORIZACAO", SqlDbType.VarChar).Value = o["payload"]["object"]["history"][i]["authorizer_id"].ToString();
                                cmdInsConsHistorico.Parameters.Add("@NOM_NSU_AUTORIZACAO", SqlDbType.VarChar).Value = o["payload"]["object"]["history"][i]["authorization_nsu"].ToString();
                                cmdInsConsHistorico.Parameters.Add("@DTA_DATA_CRIACAO", SqlDbType.DateTime).Value = Convert.ToDateTime(o["payload"]["object"]["history"][i]["created_at"].ToString());
                                cmdInsConsHistorico.ExecuteNonQuery();
                                connInsConsHistorico.Close();
                                connInsConsHistorico.Dispose();
                            }
                        }

                        // Carregar os dados do recibo ----------------------------------------------------------

                        if (o["payload"]["object"]["sales_receipt"].ToString().Trim() != "")
                        {
                            string jsonRecibo = zoop.DetalhesRecibo(o["payload"]["object"]["sales_receipt"].ToString());
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

                        // Carregar os recebíveis ----------------------------------------------------------

                        string jsonRecebiveis = zoop.DetalhesRecebiveis(o["payload"]["object"]["id"].ToString());
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
                                cmdInsConsRecebiveis.Parameters.Add("@COD_ID_PESSOA_LICENCIADO", SqlDbType.Int).Value = iLicenciado;
                                cmdInsConsRecebiveis.Parameters.Add("@NOM_CODE_TRANSACAO", SqlDbType.VarChar).Value = o["payload"]["object"]["id"].ToString();
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
                        // --------------------------------------------------------------------------------------


                    }
                    cmdInsCons.ExecuteNonQuery();
                    connInsCons.Close();
                    connInsCons.Dispose();
                }

                // Transaction Failed

                if ((sType2.Contains("transaction.failed") == true) || (sType2.Contains("buyer.transaction.failed") == true))
                {
                    // Transacoes
                    SqlConnection connInsCons = new SqlConnection(sConexao);
                    connInsCons.Open();
                    SqlCommand cmdInsCons = new SqlCommand("dbo.stp_transacoes_ins", connInsCons);
                    cmdInsCons.CommandType = CommandType.StoredProcedure;
                    cmdInsCons.Parameters.Add("@flg_operacao", SqlDbType.Char).Value = 'I';


                    int iEstabelecimento = 0;
                    int iLicenciado = 0;

                    SqlConnection mySelCadastroEstabelecimento = new SqlConnection(sConexao);
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

                    if (bodyText.ToString().Trim() != "")
                    {
                        string json = bodyText.ToString();
                        JObject o = JObject.Parse(json);
                        string sType = o["type"].ToString();

                        cmdInsCons.Parameters.Add("@DTA_DATA", SqlDbType.DateTime).Value = Convert.ToDateTime(o["payload"]["object"]["created_at"].ToString());
                        cmdInsCons.Parameters.Add("@NOM_CODE", SqlDbType.VarChar).Value = o["payload"]["object"]["id"].ToString();
                        cmdInsCons.Parameters.Add("@NUM_TOKEN", SqlDbType.VarChar).Value = o["payload"]["object"]["on_behalf_of"].ToString();

                        cmdInsCons.Parameters.Add("@COD_ID_TIPO", SqlDbType.Int).Value = 1;
                        cmdInsCons.Parameters.Add("@NOM_STATUS", SqlDbType.VarChar).Value = o["payload"]["object"]["status"].ToString();

                        if ((o["payload"]["object"]["point_of_sale"]["identification_number"].ToString().Trim() == "") || (o["payload"]["object"]["point_of_sale"]["identification_number"].ToString().Trim() == "null"))
                        {
                            cmdInsCons.Parameters.Add("@FLG_PRESENCIAL_ONLINE", SqlDbType.Char).Value = "O";
                        }
                        else
                        {
                            cmdInsCons.Parameters.Add("@FLG_PRESENCIAL_ONLINE", SqlDbType.Char).Value = "P";
                        }

                        cmdInsCons.Parameters.Add("@NOM_TIPO_PAGAMENTO", SqlDbType.VarChar).Value = o["payload"]["object"]["payment_type"].ToString();

                        if ((o["payload"]["object"]["payment_type"].ToString() == "pix"))
                        {
                            cmdInsCons.Parameters.Add("@NOM_CODIGO_PAGAMENTO", SqlDbType.VarChar).Value = o["payload"]["object"]["payment_method"]["provider"].ToString();
                            cmdInsCons.Parameters.Add("@NUM_PARCELAS", SqlDbType.Int).Value = 1;
                            cmdInsCons.Parameters.Add("@NOM_QRCODE", SqlDbType.VarChar).Value = o["payload"]["object"]["payment_method"]["qr_code"]["emv"].ToString();

                        }
                        if ((o["payload"]["object"]["payment_type"].ToString() == "boleto"))
                        {
                            cmdInsCons.Parameters.Add("@NOM_CODIGO_PAGAMENTO", SqlDbType.VarChar).Value = o["payload"]["object"]["payment_method"]["resource"].ToString();
                            cmdInsCons.Parameters.Add("@NUM_PARCELAS", SqlDbType.Int).Value = 1;

                            cmdInsCons.Parameters.Add("@NOM_CODIGO_BARRAS", SqlDbType.VarChar).Value = o["payload"]["object"]["payment_method"]["barcode"].ToString();
                            cmdInsCons.Parameters.Add("@NOM_URL_BOLETO", SqlDbType.VarChar).Value = o["payload"]["object"]["payment_method"]["url"].ToString();
                            cmdInsCons.Parameters.Add("@NOM_NUMERO_DOCUMENTO", SqlDbType.VarChar).Value = o["payload"]["object"]["payment_method"]["document_number"].ToString();
                            cmdInsCons.Parameters.Add("@NOM_DATA_VENCIMENTO", SqlDbType.VarChar).Value = o["payload"]["object"]["payment_method"]["expiration_date"].ToString();

                        }


                        if ((o["payload"]["object"]["payment_type"].ToString() == "credit"))
                        {
                            cmdInsCons.Parameters.Add("@NOM_CODIGO_PAGAMENTO", SqlDbType.VarChar).Value = o["payload"]["object"]["payment_method"]["resource"].ToString() + " " + o["payload"]["object"]["payment_method"]["card_brand"].ToString();

                            try { cmdInsCons.Parameters.Add("@NUM_PARCELAS", SqlDbType.Int).Value = Funcoes.strToInt(o["payload"]["object"]["installment_plan"]["number_installments"].ToString()); }
                            catch { }

                            //cmdInsCons.Parameters.Add("@NUM_PARCELAS", SqlDbType.Int).Value = Funcoes.strToInt(o["payload"]["object"]["installment_plan"]["number_installments"].ToString());

                            cmdInsCons.Parameters.Add("@NOM_CARTAO_4_PRIMEIROS_DIGIROS", SqlDbType.VarChar).Value = o["payload"]["object"]["payment_method"]["first4_digits"].ToString();
                            cmdInsCons.Parameters.Add("@NOM_CARTAO_4_ULTIMOS_DIGITOS", SqlDbType.VarChar).Value = o["payload"]["object"]["payment_method"]["last4_digits"].ToString();
                            cmdInsCons.Parameters.Add("@NOM_CARTAO_MES", SqlDbType.VarChar).Value = o["payload"]["object"]["payment_method"]["expiration_month"].ToString();
                            cmdInsCons.Parameters.Add("@NOM_CARTAO_ANO", SqlDbType.VarChar).Value = o["payload"]["object"]["payment_method"]["expiration_year"].ToString();
                        }

                        if ((o["payload"]["object"]["payment_type"].ToString() == "debit"))
                        {
                            cmdInsCons.Parameters.Add("@NOM_CODIGO_PAGAMENTO", SqlDbType.VarChar).Value = o["payload"]["object"]["payment_method"]["resource"].ToString() + " " + o["payload"]["object"]["payment_method"]["card_brand"].ToString();
                            cmdInsCons.Parameters.Add("@NUM_PARCELAS", SqlDbType.Int).Value = 1;

                            cmdInsCons.Parameters.Add("@NOM_CARTAO_4_PRIMEIROS_DIGIROS", SqlDbType.VarChar).Value = o["payload"]["object"]["payment_method"]["first4_digits"].ToString();
                            cmdInsCons.Parameters.Add("@NOM_CARTAO_4_ULTIMOS_DIGITOS", SqlDbType.VarChar).Value = o["payload"]["object"]["payment_method"]["last4_digits"].ToString();
                            cmdInsCons.Parameters.Add("@NOM_CARTAO_MES", SqlDbType.VarChar).Value = o["payload"]["object"]["payment_method"]["expiration_month"].ToString();
                            cmdInsCons.Parameters.Add("@NOM_CARTAO_ANO", SqlDbType.VarChar).Value = o["payload"]["object"]["payment_method"]["expiration_year"].ToString();
                        }



                        cmdInsCons.Parameters.Add("@NUM_VALOR_BRUTO", SqlDbType.Float).Value = Funcoes.strToDouble(o["payload"]["object"]["amount"].ToString()) / 100;
                        cmdInsCons.Parameters.Add("@NUM_VALOR_DESCONTO", SqlDbType.Float).Value = 0;
                        cmdInsCons.Parameters.Add("@NUM_VALOR_INTERMEDIACAO_RATE", SqlDbType.Float).Value = 0;

                        cmdInsCons.Parameters.Add("@NUM_VALOR_INTERMEDIACAO_FEE", SqlDbType.Float).Value = Funcoes.strToDouble(o["payload"]["object"]["fees"].ToString()) / 100;
                        cmdInsCons.Parameters.Add("@NUM_VALOR_LIQUIDO", SqlDbType.Float).Value = (Funcoes.strToDouble(o["payload"]["object"]["amount"].ToString()) / 100) - (Funcoes.strToDouble(o["payload"]["object"]["fees"].ToString()) / 100);
                        cmdInsCons.Parameters.Add("@NUM_VALOR_EXTRAS", SqlDbType.Float).Value = 0;

                        cmdInsCons.Parameters.Add("@DTA_DATA_DEPOSITO", SqlDbType.DateTime).Value = Convert.ToDateTime(o["payload"]["object"]["created_at"].ToString());

                        cmdInsCons.Parameters.Add("@NUM_ITENS", SqlDbType.Int).Value = 1;

                        cmdInsCons.Parameters.Add("@NOM_FONTE_CANCELAMENTO", SqlDbType.VarChar).Value = "";
                        cmdInsCons.Parameters.Add("@NOM_ORIGEM", SqlDbType.VarChar).Value = o["payload"]["object"]["gateway_authorizer"].ToString();

                        try { cmdInsCons.Parameters.Add("@NOM_CLIENTE", SqlDbType.VarChar).Value = o["payload"]["object"]["payment_method"]["holder_name"].ToString(); }
                        catch { }

                        // Inserido em 21/06/2024 -----------------------------------------------------------------------
                        // Dados do cartao no caso de existência

                        //if (o["payload"]["object"]["payment_method"].Contains("first4_digits") == true) { cmdInsCons.Parameters.Add("@NOM_CARTAO_4_PRIMEIROS_DIGIROS", SqlDbType.VarChar).Value = o["payload"]["object"]["payment_method"]["first4_digits"].ToString(); }
                        //if (o["payload"]["object"]["payment_method"].Contains("last4_digits") == true) { cmdInsCons.Parameters.Add("@NOM_CARTAO_4_ULTIMOS_DIGITOS", SqlDbType.VarChar).Value = o["payload"]["object"]["payment_method"]["last4_digits"].ToString(); }
                        //if (o["payload"]["object"]["payment_method"].Contains("expiration_month") == true) { cmdInsCons.Parameters.Add("@NOM_CARTAO_MES", SqlDbType.VarChar).Value = o["payload"]["object"]["payment_method"]["expiration_month"].ToString(); }
                        //if (o["payload"]["object"]["payment_method"].Contains("expiration_year") == true) { cmdInsCons.Parameters.Add("@NOM_CARTAO_ANO", SqlDbType.VarChar).Value = o["payload"]["object"]["payment_method"]["expiration_year"].ToString(); }

                        cmdInsCons.Parameters.Add("@NOM_NUMERO_TRANSACAO", SqlDbType.VarChar).Value = o["payload"]["object"]["transaction_number"].ToString();
                        cmdInsCons.Parameters.Add("@NOM_RECIBO", SqlDbType.VarChar).Value = o["payload"]["object"]["sales_receipt"].ToString();

                        // Dados do Boleto

                        //if (o["payload"]["object"]["payment_method"].Contains("barcode") == true) { cmdInsCons.Parameters.Add("@NOM_CODIGO_BARRAS", SqlDbType.VarChar).Value = o["payload"]["object"]["payment_method"]["barcode"].ToString(); }
                        //if (o["payload"]["object"]["payment_method"].Contains("url") == true) { cmdInsCons.Parameters.Add("@NOM_URL_BOLETO", SqlDbType.VarChar).Value = o["payload"]["object"]["payment_method"]["url"].ToString(); }
                        //if (o["payload"]["object"]["payment_method"].Contains("document_number") == true) { cmdInsCons.Parameters.Add("@NOM_NUMERO_DOCUMENTO", SqlDbType.VarChar).Value = o["payload"]["object"]["payment_method"]["document_number"].ToString(); }
                        //if (o["payload"]["object"]["payment_method"].Contains("expiration_date") == true) { cmdInsCons.Parameters.Add("@NOM_DATA_VENCIMENTO", SqlDbType.VarChar).Value = o["payload"]["object"]["payment_method"]["expiration_date"].ToString(); }

                        // Dados do PIX

                        //if (o["payload"]["object"]["payment_method"].Contains("qr_code") == true) { cmdInsCons.Parameters.Add("@NOM_QRCODE", SqlDbType.VarChar).Value = o["payload"]["object"]["payment_method"]["qr_code"]["emv"].ToString(); }

                        // -----------------------------------------------------------------------------------------------


                        cmdInsCons.Parameters.Add("@NOM_AUTORIZACAO", SqlDbType.VarChar).Value = o["payload"]["object"]["transaction_number"].ToString();
                        cmdInsCons.Parameters.Add("@DES_DESCRICAO", SqlDbType.Text).Value = o["payload"]["object"]["payment_type"].ToString();

                        if (o["payload"]["object"]["history"].Count() > 0)
                        {
                            for (int i = 0; i < o["payload"]["object"]["history"].Count(); i++)
                            {
                                // Inserção de dados do Histórico

                                SqlConnection connInsConsHistorico = new SqlConnection(sConexao);
                                connInsConsHistorico.Open();
                                SqlCommand cmdInsConsHistorico = new SqlCommand("dbo.stp_transacoes_historico_ins", connInsConsHistorico);
                                cmdInsConsHistorico.CommandType = CommandType.StoredProcedure;
                                cmdInsConsHistorico.Parameters.Add("@flg_operacao", SqlDbType.Char).Value = 'I';
                                cmdInsConsHistorico.Parameters.Add("@COD_ID_PESSOA_LICENCIADO", SqlDbType.Int).Value = iLicenciado;
                                cmdInsConsHistorico.Parameters.Add("@NOM_CODE", SqlDbType.VarChar).Value = o["payload"]["object"]["id"].ToString();
                                cmdInsConsHistorico.Parameters.Add("@DTA_DATA", SqlDbType.DateTime).Value = DateTime.Now;
                                cmdInsConsHistorico.Parameters.Add("@NOM_TIPO", SqlDbType.VarChar).Value = o["payload"]["object"]["history"][i]["operation_type"].ToString();
                                cmdInsConsHistorico.Parameters.Add("@NOM_STATUS", SqlDbType.VarChar).Value = o["payload"]["object"]["history"][i]["status"].ToString();
                                cmdInsConsHistorico.Parameters.Add("@NUM_VALOR", SqlDbType.Float).Value = Funcoes.strToDouble(o["payload"]["object"]["history"][i]["amount"].ToString());
                                cmdInsConsHistorico.Parameters.Add("@NOM_CODIGO_RESPOSTA", SqlDbType.VarChar).Value = o["payload"]["object"]["history"][i]["response_code"].ToString();
                                cmdInsConsHistorico.Parameters.Add("@NOM_DESCRICAO_RESPOSTA", SqlDbType.VarChar).Value = o["payload"]["object"]["history"][i]["response_message"].ToString();
                                cmdInsConsHistorico.Parameters.Add("@NOM_ID_AUTORIZACAO", SqlDbType.VarChar).Value = o["payload"]["object"]["history"][i]["authorizer_id"].ToString();
                                cmdInsConsHistorico.Parameters.Add("@NOM_NSU_AUTORIZACAO", SqlDbType.VarChar).Value = o["payload"]["object"]["history"][i]["authorization_nsu"].ToString();
                                cmdInsConsHistorico.Parameters.Add("@DTA_DATA_CRIACAO", SqlDbType.DateTime).Value = Convert.ToDateTime(o["payload"]["object"]["history"][i]["created_at"].ToString());
                                cmdInsConsHistorico.ExecuteNonQuery();
                                connInsConsHistorico.Close();
                                connInsConsHistorico.Dispose();
                            }
                        }

                        // Carregar os dados do recibo ----------------------------------------------------------

                        if (o["payload"]["object"]["sales_receipt"].ToString().Trim() != "")
                        {
                            string jsonRecibo = zoop.DetalhesRecibo(o["payload"]["object"]["sales_receipt"].ToString());
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

                        // Carregar os recebíveis ----------------------------------------------------------

                        string jsonRecebiveis = zoop.DetalhesRecebiveis(o["payload"]["object"]["id"].ToString());
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
                                cmdInsConsRecebiveis.Parameters.Add("@COD_ID_PESSOA_LICENCIADO", SqlDbType.Int).Value = iLicenciado;
                                cmdInsConsRecebiveis.Parameters.Add("@NOM_CODE_TRANSACAO", SqlDbType.VarChar).Value = o["payload"]["object"]["id"].ToString();
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


                        // --------------------------------------------------------------------------------------


                    }
                    cmdInsCons.ExecuteNonQuery();
                    connInsCons.Close();
                    connInsCons.Dispose();
                }

                // Transaction.created

                if ((sType2.Contains("transaction.created") == true) || (sType2.Contains("buyer.transaction.created") == true))
                {
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

                    if (bodyText.ToString().Trim() != "")
                    {
                        string json = bodyText.ToString();
                        JObject o = JObject.Parse(json);
                        string sType = o["type"].ToString();

                        cmdInsCons.Parameters.Add("@DTA_DATA", SqlDbType.DateTime).Value = Convert.ToDateTime(o["payload"]["object"]["created_at"].ToString());
                        cmdInsCons.Parameters.Add("@NOM_CODE", SqlDbType.VarChar).Value = o["payload"]["object"]["id"].ToString();
                        cmdInsCons.Parameters.Add("@NUM_TOKEN", SqlDbType.VarChar).Value = o["payload"]["object"]["on_behalf_of"].ToString();

                        cmdInsCons.Parameters.Add("@COD_ID_TIPO", SqlDbType.Int).Value = 1;
                        cmdInsCons.Parameters.Add("@NOM_STATUS", SqlDbType.VarChar).Value = o["payload"]["object"]["status"].ToString();

                        if ((o["payload"]["object"]["point_of_sale"]["identification_number"].ToString().Trim() == "") || (o["payload"]["object"]["point_of_sale"]["identification_number"].ToString().Trim() == "null"))
                        {
                            cmdInsCons.Parameters.Add("@FLG_PRESENCIAL_ONLINE", SqlDbType.Char).Value = "O";
                        }
                        else
                        {
                            cmdInsCons.Parameters.Add("@FLG_PRESENCIAL_ONLINE", SqlDbType.Char).Value = "P";
                        }

                        cmdInsCons.Parameters.Add("@NOM_TIPO_PAGAMENTO", SqlDbType.VarChar).Value = o["payload"]["object"]["payment_type"].ToString();

                        if ((o["payload"]["object"]["payment_type"].ToString() == "pix"))
                        {
                            cmdInsCons.Parameters.Add("@NOM_CODIGO_PAGAMENTO", SqlDbType.VarChar).Value = o["payload"]["object"]["payment_method"]["provider"].ToString();
                            cmdInsCons.Parameters.Add("@NUM_PARCELAS", SqlDbType.Int).Value = 1;
                            cmdInsCons.Parameters.Add("@NOM_QRCODE", SqlDbType.VarChar).Value = o["payload"]["object"]["payment_method"]["qr_code"]["emv"].ToString();

                        }
                        if ((o["payload"]["object"]["payment_type"].ToString() == "boleto"))
                        {
                            cmdInsCons.Parameters.Add("@NOM_CODIGO_PAGAMENTO", SqlDbType.VarChar).Value = o["payload"]["object"]["payment_method"]["resource"].ToString();
                            cmdInsCons.Parameters.Add("@NUM_PARCELAS", SqlDbType.Int).Value = 1;

                            cmdInsCons.Parameters.Add("@NOM_CODIGO_BARRAS", SqlDbType.VarChar).Value = o["payload"]["object"]["payment_method"]["barcode"].ToString();
                            cmdInsCons.Parameters.Add("@NOM_URL_BOLETO", SqlDbType.VarChar).Value = o["payload"]["object"]["payment_method"]["url"].ToString();
                            cmdInsCons.Parameters.Add("@NOM_NUMERO_DOCUMENTO", SqlDbType.VarChar).Value = o["payload"]["object"]["payment_method"]["document_number"].ToString();
                            cmdInsCons.Parameters.Add("@NOM_DATA_VENCIMENTO", SqlDbType.VarChar).Value = o["payload"]["object"]["payment_method"]["expiration_date"].ToString();

                        }


                        if ((o["payload"]["object"]["payment_type"].ToString() == "credit"))
                        {
                            cmdInsCons.Parameters.Add("@NOM_CODIGO_PAGAMENTO", SqlDbType.VarChar).Value = o["payload"]["object"]["payment_method"]["resource"].ToString() + " " + o["payload"]["object"]["payment_method"]["card_brand"].ToString();

                            try { cmdInsCons.Parameters.Add("@NUM_PARCELAS", SqlDbType.Int).Value = Funcoes.strToInt(o["payload"]["object"]["installment_plan"]["number_installments"].ToString()); }
                            catch { }

                            //cmdInsCons.Parameters.Add("@NUM_PARCELAS", SqlDbType.Int).Value = Funcoes.strToInt(o["payload"]["object"]["installment_plan"]["number_installments"].ToString());

                            cmdInsCons.Parameters.Add("@NOM_CARTAO_4_PRIMEIROS_DIGIROS", SqlDbType.VarChar).Value = o["payload"]["object"]["payment_method"]["first4_digits"].ToString();
                            cmdInsCons.Parameters.Add("@NOM_CARTAO_4_ULTIMOS_DIGITOS", SqlDbType.VarChar).Value = o["payload"]["object"]["payment_method"]["last4_digits"].ToString();
                            cmdInsCons.Parameters.Add("@NOM_CARTAO_MES", SqlDbType.VarChar).Value = o["payload"]["object"]["payment_method"]["expiration_month"].ToString();
                            cmdInsCons.Parameters.Add("@NOM_CARTAO_ANO", SqlDbType.VarChar).Value = o["payload"]["object"]["payment_method"]["expiration_year"].ToString();
                        }

                        if ((o["payload"]["object"]["payment_type"].ToString() == "debit"))
                        {
                            cmdInsCons.Parameters.Add("@NOM_CODIGO_PAGAMENTO", SqlDbType.VarChar).Value = o["payload"]["object"]["payment_method"]["resource"].ToString() + " " + o["payload"]["object"]["payment_method"]["card_brand"].ToString();
                            cmdInsCons.Parameters.Add("@NUM_PARCELAS", SqlDbType.Int).Value = 1;

                            cmdInsCons.Parameters.Add("@NOM_CARTAO_4_PRIMEIROS_DIGIROS", SqlDbType.VarChar).Value = o["payload"]["object"]["payment_method"]["first4_digits"].ToString();
                            cmdInsCons.Parameters.Add("@NOM_CARTAO_4_ULTIMOS_DIGITOS", SqlDbType.VarChar).Value = o["payload"]["object"]["payment_method"]["last4_digits"].ToString();
                            cmdInsCons.Parameters.Add("@NOM_CARTAO_MES", SqlDbType.VarChar).Value = o["payload"]["object"]["payment_method"]["expiration_month"].ToString();
                            cmdInsCons.Parameters.Add("@NOM_CARTAO_ANO", SqlDbType.VarChar).Value = o["payload"]["object"]["payment_method"]["expiration_year"].ToString();
                        }



                        cmdInsCons.Parameters.Add("@NUM_VALOR_BRUTO", SqlDbType.Float).Value = Funcoes.strToDouble(o["payload"]["object"]["amount"].ToString()) / 100;
                        cmdInsCons.Parameters.Add("@NUM_VALOR_DESCONTO", SqlDbType.Float).Value = 0;
                        cmdInsCons.Parameters.Add("@NUM_VALOR_INTERMEDIACAO_RATE", SqlDbType.Float).Value = 0;

                        cmdInsCons.Parameters.Add("@NUM_VALOR_INTERMEDIACAO_FEE", SqlDbType.Float).Value = Funcoes.strToDouble(o["payload"]["object"]["fees"].ToString()) / 100;
                        cmdInsCons.Parameters.Add("@NUM_VALOR_LIQUIDO", SqlDbType.Float).Value = (Funcoes.strToDouble(o["payload"]["object"]["amount"].ToString()) / 100) - (Funcoes.strToDouble(o["payload"]["object"]["fees"].ToString()) / 100);
                        cmdInsCons.Parameters.Add("@NUM_VALOR_EXTRAS", SqlDbType.Float).Value = 0;

                        cmdInsCons.Parameters.Add("@DTA_DATA_DEPOSITO", SqlDbType.DateTime).Value = Convert.ToDateTime(o["payload"]["object"]["created_at"].ToString());

                        cmdInsCons.Parameters.Add("@NUM_ITENS", SqlDbType.Int).Value = 1;

                        cmdInsCons.Parameters.Add("@NOM_FONTE_CANCELAMENTO", SqlDbType.VarChar).Value = "";
                        cmdInsCons.Parameters.Add("@NOM_ORIGEM", SqlDbType.VarChar).Value = o["payload"]["object"]["gateway_authorizer"].ToString();

                        try { cmdInsCons.Parameters.Add("@NOM_CLIENTE", SqlDbType.VarChar).Value = o["payload"]["object"]["payment_method"]["holder_name"].ToString(); }
                        catch { }

                        // Inserido em 21/06/2024 -----------------------------------------------------------------------
                        // Dados do cartao no caso de existência

                        //if (o["payload"]["object"]["payment_method"].Contains("first4_digits") == true) { cmdInsCons.Parameters.Add("@NOM_CARTAO_4_PRIMEIROS_DIGIROS", SqlDbType.VarChar).Value = o["payload"]["object"]["payment_method"]["first4_digits"].ToString(); }
                        //if (o["payload"]["object"]["payment_method"].Contains("last4_digits") == true) { cmdInsCons.Parameters.Add("@NOM_CARTAO_4_ULTIMOS_DIGITOS", SqlDbType.VarChar).Value = o["payload"]["object"]["payment_method"]["last4_digits"].ToString(); }
                        //if (o["payload"]["object"]["payment_method"].Contains("expiration_month") == true) { cmdInsCons.Parameters.Add("@NOM_CARTAO_MES", SqlDbType.VarChar).Value = o["payload"]["object"]["payment_method"]["expiration_month"].ToString(); }
                        //if (o["payload"]["object"]["payment_method"].Contains("expiration_year") == true) { cmdInsCons.Parameters.Add("@NOM_CARTAO_ANO", SqlDbType.VarChar).Value = o["payload"]["object"]["payment_method"]["expiration_year"].ToString(); }

                        cmdInsCons.Parameters.Add("@NOM_NUMERO_TRANSACAO", SqlDbType.VarChar).Value = o["payload"]["object"]["transaction_number"].ToString();
                        cmdInsCons.Parameters.Add("@NOM_RECIBO", SqlDbType.VarChar).Value = o["payload"]["object"]["sales_receipt"].ToString();

                        // Dados do Boleto

                        //if (o["payload"]["object"]["payment_method"].Contains("barcode") == true) { cmdInsCons.Parameters.Add("@NOM_CODIGO_BARRAS", SqlDbType.VarChar).Value = o["payload"]["object"]["payment_method"]["barcode"].ToString(); }
                        //if (o["payload"]["object"]["payment_method"].Contains("url") == true) { cmdInsCons.Parameters.Add("@NOM_URL_BOLETO", SqlDbType.VarChar).Value = o["payload"]["object"]["payment_method"]["url"].ToString(); }
                        //if (o["payload"]["object"]["payment_method"].Contains("document_number") == true) { cmdInsCons.Parameters.Add("@NOM_NUMERO_DOCUMENTO", SqlDbType.VarChar).Value = o["payload"]["object"]["payment_method"]["document_number"].ToString(); }
                        //if (o["payload"]["object"]["payment_method"].Contains("expiration_date") == true) { cmdInsCons.Parameters.Add("@NOM_DATA_VENCIMENTO", SqlDbType.VarChar).Value = o["payload"]["object"]["payment_method"]["expiration_date"].ToString(); }

                        // Dados do PIX

                        //if (o["payload"]["object"]["payment_method"].Contains("qr_code") == true) { cmdInsCons.Parameters.Add("@NOM_QRCODE", SqlDbType.VarChar).Value = o["payload"]["object"]["payment_method"]["qr_code"]["emv"].ToString(); }

                        // -----------------------------------------------------------------------------------------------


                        cmdInsCons.Parameters.Add("@NOM_AUTORIZACAO", SqlDbType.VarChar).Value = o["payload"]["object"]["transaction_number"].ToString();
                        cmdInsCons.Parameters.Add("@DES_DESCRICAO", SqlDbType.Text).Value = o["payload"]["object"]["payment_type"].ToString();

                        if (o["payload"]["object"]["history"].Count() > 0)
                        {
                            for (int i = 0; i < o["payload"]["object"]["history"].Count(); i++)
                            {
                                // Inserção de dados do Histórico

                                SqlConnection connInsConsHistorico = new SqlConnection(sConexao);
                                connInsConsHistorico.Open();
                                SqlCommand cmdInsConsHistorico = new SqlCommand("dbo.stp_transacoes_historico_ins", connInsConsHistorico);
                                cmdInsConsHistorico.CommandType = CommandType.StoredProcedure;
                                cmdInsConsHistorico.Parameters.Add("@flg_operacao", SqlDbType.Char).Value = 'I';
                                cmdInsConsHistorico.Parameters.Add("@COD_ID_PESSOA_LICENCIADO", SqlDbType.Int).Value = iLicenciado;
                                cmdInsConsHistorico.Parameters.Add("@NOM_CODE", SqlDbType.VarChar).Value = o["payload"]["object"]["id"].ToString();
                                cmdInsConsHistorico.Parameters.Add("@DTA_DATA", SqlDbType.DateTime).Value = DateTime.Now;
                                cmdInsConsHistorico.Parameters.Add("@NOM_TIPO", SqlDbType.VarChar).Value = o["payload"]["object"]["history"][i]["operation_type"].ToString();
                                cmdInsConsHistorico.Parameters.Add("@NOM_STATUS", SqlDbType.VarChar).Value = o["payload"]["object"]["history"][i]["status"].ToString();
                                cmdInsConsHistorico.Parameters.Add("@NUM_VALOR", SqlDbType.Float).Value = Funcoes.strToDouble(o["payload"]["object"]["history"][i]["amount"].ToString());
                                cmdInsConsHistorico.Parameters.Add("@NOM_CODIGO_RESPOSTA", SqlDbType.VarChar).Value = o["payload"]["object"]["history"][i]["response_code"].ToString();
                                cmdInsConsHistorico.Parameters.Add("@NOM_DESCRICAO_RESPOSTA", SqlDbType.VarChar).Value = o["payload"]["object"]["history"][i]["response_message"].ToString();
                                cmdInsConsHistorico.Parameters.Add("@NOM_ID_AUTORIZACAO", SqlDbType.VarChar).Value = o["payload"]["object"]["history"][i]["authorizer_id"].ToString();
                                cmdInsConsHistorico.Parameters.Add("@NOM_NSU_AUTORIZACAO", SqlDbType.VarChar).Value = o["payload"]["object"]["history"][i]["authorization_nsu"].ToString();
                                cmdInsConsHistorico.Parameters.Add("@DTA_DATA_CRIACAO", SqlDbType.DateTime).Value = Convert.ToDateTime(o["payload"]["object"]["history"][i]["created_at"].ToString());
                                cmdInsConsHistorico.ExecuteNonQuery();
                                connInsConsHistorico.Close();
                                connInsConsHistorico.Dispose();
                            }
                        }

                        // Carregar os dados do recibo ----------------------------------------------------------

                        if (o["payload"]["object"]["sales_receipt"].ToString().Trim() != "")
                        {
                            string jsonRecibo = zoop.DetalhesRecibo(o["payload"]["object"]["sales_receipt"].ToString());
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
                        // Carregar os recebíveis ----------------------------------------------------------

                        string jsonRecebiveis = zoop.DetalhesRecebiveis(o["payload"]["object"]["id"].ToString());
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
                                cmdInsConsRecebiveis.Parameters.Add("@COD_ID_PESSOA_LICENCIADO", SqlDbType.Int).Value = iLicenciado;
                                cmdInsConsRecebiveis.Parameters.Add("@NOM_CODE_TRANSACAO", SqlDbType.VarChar).Value = o["payload"]["object"]["id"].ToString();
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

                        // --------------------------------------------------------------------------------------


                    }
                    cmdInsCons.ExecuteNonQuery();
                    connInsCons.Close();
                    connInsCons.Dispose();
                }

            }

            SqlConnection connInsConsU = new SqlConnection(Funcoes.conexao());
            connInsConsU.Open();
            SqlCommand cmdInsConsU = new SqlCommand("dbo.stp_notificacoes_zoop_ins", connInsConsU);
            cmdInsConsU.CommandType = CommandType.StoredProcedure;
            cmdInsConsU.Parameters.Add("@flg_operacao", SqlDbType.Char).Value = "R";
            cmdInsConsU.Parameters.Add("@NOM_CODE", SqlDbType.VarChar).Value = sCode.ToString();

            cmdInsConsU.ExecuteNonQuery();
            connInsConsU.Close();
            connInsConsU.Dispose();
        }
        ConsultaGeral();
    }
}