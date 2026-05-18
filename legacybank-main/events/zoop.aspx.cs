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


public partial class events_zoop : System.Web.UI.Page
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
        //StreamWriter strm = new StreamWriter(Server.MapPath("public_html") + "\\" + DateTime.Now.Day.ToString() + DateTime.Now.Month.ToString() + DateTime.Now.Year.ToString() + DateTime.Now.Minute.ToString() + DateTime.Now.Second.ToString() + "_" + "zoop.txt");
        //strm.WriteLine(sResultado);
        //strm.Close();

        // Grava JSON na Base de dados

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

        if (bodyText.ToString().Trim() != "")
        {
            string json2 = bodyText.ToString();
            JObject o2 = JObject.Parse(json2);
            string sType2 = o2["type"].ToString();

            if ((sType2.Contains("transaction") == true))
            {
                cmdInsConsZOOP.Parameters.Add("@DTA_DATA", SqlDbType.DateTime).Value = Convert.ToDateTime(o2["payload"]["object"]["created_at"].ToString());
                cmdInsConsZOOP.Parameters.Add("@NOM_CODE", SqlDbType.VarChar).Value = o2["payload"]["object"]["id"].ToString();
                sToken = o2["payload"]["object"]["on_behalf_of"].ToString();
                cmdInsConsZOOP.Parameters.Add("@NUM_TOKEN", SqlDbType.VarChar).Value = o2["payload"]["object"]["on_behalf_of"].ToString();
                cmdInsConsZOOP.Parameters.Add("@NOM_TIPO", SqlDbType.VarChar).Value = o2["type"].ToString();
                cmdInsConsZOOP.Parameters.Add("@FLG_STATUS", SqlDbType.VarChar).Value = o2["payload"]["object"]["status"].ToString();
            }

            if ((sType2.Contains("ping") == true))
            {
                cmdInsConsZOOP.Parameters.Add("@DTA_DATA", SqlDbType.DateTime).Value = Convert.ToDateTime(o2["created_at"].ToString());
                cmdInsConsZOOP.Parameters.Add("@NOM_CODE", SqlDbType.VarChar).Value = o2["id"].ToString();
                sToken = "";
                cmdInsConsZOOP.Parameters.Add("@NUM_TOKEN", SqlDbType.VarChar).Value = "";
                cmdInsConsZOOP.Parameters.Add("@NOM_TIPO", SqlDbType.VarChar).Value = o2["type"].ToString();
                cmdInsConsZOOP.Parameters.Add("@FLG_STATUS", SqlDbType.VarChar).Value = "";
            }

            if ((sType2.Contains("transfer") == true))
            {
                cmdInsConsZOOP.Parameters.Add("@DTA_DATA", SqlDbType.DateTime).Value = Convert.ToDateTime(o2["payload"]["object"]["created_at"].ToString());
                cmdInsConsZOOP.Parameters.Add("@NOM_CODE", SqlDbType.VarChar).Value = o2["payload"]["object"]["id"].ToString();
                sToken = o2["payload"]["object"]["recipient"].ToString();
                cmdInsConsZOOP.Parameters.Add("@NUM_TOKEN", SqlDbType.VarChar).Value = o2["payload"]["object"]["recipient"].ToString();
                cmdInsConsZOOP.Parameters.Add("@NOM_TIPO", SqlDbType.VarChar).Value = o2["type"].ToString();
                cmdInsConsZOOP.Parameters.Add("@FLG_STATUS", SqlDbType.VarChar).Value = o2["payload"]["object"]["status"].ToString();
            }

            if ((sType2.Contains("receivable") == true))
            {
                cmdInsConsZOOP.Parameters.Add("@DTA_DATA", SqlDbType.DateTime).Value = Convert.ToDateTime(o2["created_at"].ToString());
                cmdInsConsZOOP.Parameters.Add("@NOM_CODE", SqlDbType.VarChar).Value = o2["payload"]["object"]["id"].ToString();
                sToken = o2["payload"]["object"]["recipient"].ToString();
                cmdInsConsZOOP.Parameters.Add("@NUM_TOKEN", SqlDbType.VarChar).Value = o2["payload"]["object"]["recipient"].ToString();
                cmdInsConsZOOP.Parameters.Add("@NOM_TIPO", SqlDbType.VarChar).Value = o2["type"].ToString();
                cmdInsConsZOOP.Parameters.Add("@FLG_STATUS", SqlDbType.VarChar).Value = o2["payload"]["object"]["status"].ToString();
            }

            if ((sType2.Contains("seller") == true))
            {
                try
                {
                    cmdInsConsZOOP.Parameters.Add("@DTA_DATA", SqlDbType.DateTime).Value = Convert.ToDateTime(o2["created_at"].ToString());
                    cmdInsConsZOOP.Parameters.Add("@NOM_CODE", SqlDbType.VarChar).Value = o2["payload"]["object"]["id"].ToString();
                    sToken = o2["payload"]["object"]["id"].ToString();
                    cmdInsConsZOOP.Parameters.Add("@NUM_TOKEN", SqlDbType.VarChar).Value = o2["payload"]["object"]["id"].ToString();
                    cmdInsConsZOOP.Parameters.Add("@NOM_TIPO", SqlDbType.VarChar).Value = o2["type"].ToString();
                    cmdInsConsZOOP.Parameters.Add("@FLG_STATUS", SqlDbType.VarChar).Value = o2["payload"]["object"]["status"].ToString();
                }
                catch
                {

                }
            }
            //if ((sType2.Contains("buyer") == true))
            //{
            //    try
            //    {
            //        cmdInsConsZOOP.Parameters.Add("@DTA_DATA", SqlDbType.DateTime).Value = Convert.ToDateTime(o2["created_at"].ToString());
            //        cmdInsConsZOOP.Parameters.Add("@NOM_CODE", SqlDbType.VarChar).Value = o2["payload"]["object"]["id"].ToString();
            //        sToken = o2["payload"]["object"]["recipient"].ToString();
            //        cmdInsConsZOOP.Parameters.Add("@NUM_TOKEN", SqlDbType.VarChar).Value = o2["payload"]["object"]["recipient"].ToString();
            //        cmdInsConsZOOP.Parameters.Add("@NOM_TIPO", SqlDbType.VarChar).Value = o2["type"].ToString();
            //        cmdInsConsZOOP.Parameters.Add("@FLG_STATUS", SqlDbType.VarChar).Value = o2["payload"]["object"]["status"].ToString();
            //    }
            //    catch
            //    {
    
            //    }
            //}

            if ((sType2.Contains("bank_account") == true))
            {
                try
                {
                    cmdInsConsZOOP.Parameters.Add("@DTA_DATA", SqlDbType.DateTime).Value = Convert.ToDateTime(o2["created_at"].ToString());
                    cmdInsConsZOOP.Parameters.Add("@NOM_CODE", SqlDbType.VarChar).Value = o2["payload"]["object"]["id"].ToString();
                    sToken = o2["payload"]["object"]["recipient"].ToString();
                    cmdInsConsZOOP.Parameters.Add("@NUM_TOKEN", SqlDbType.VarChar).Value = o2["payload"]["object"]["recipient"].ToString();
                    cmdInsConsZOOP.Parameters.Add("@NOM_TIPO", SqlDbType.VarChar).Value = o2["type"].ToString();
                    cmdInsConsZOOP.Parameters.Add("@FLG_STATUS", SqlDbType.VarChar).Value = o2["payload"]["object"]["status"].ToString();
                }
                catch
                {

                }
            }
            if ((sType2.Contains("plan") == true))
            {
                try
                {
                    cmdInsConsZOOP.Parameters.Add("@DTA_DATA", SqlDbType.DateTime).Value = Convert.ToDateTime(o2["created_at"].ToString());
                    cmdInsConsZOOP.Parameters.Add("@NOM_CODE", SqlDbType.VarChar).Value = o2["payload"]["object"]["id"].ToString();
                    sToken = o2["payload"]["object"]["recipient"].ToString();
                    cmdInsConsZOOP.Parameters.Add("@NUM_TOKEN", SqlDbType.VarChar).Value = o2["payload"]["object"]["recipient"].ToString();
                    cmdInsConsZOOP.Parameters.Add("@NOM_TIPO", SqlDbType.VarChar).Value = o2["type"].ToString();
                    cmdInsConsZOOP.Parameters.Add("@FLG_STATUS", SqlDbType.VarChar).Value = o2["payload"]["object"]["status"].ToString();
                }
                catch
                {

                }
            }


            if ((sType2.Contains("card") == true))
            {
                try
                {
                    cmdInsConsZOOP.Parameters.Add("@DTA_DATA", SqlDbType.DateTime).Value = Convert.ToDateTime(o2["created_at"].ToString());
                    cmdInsConsZOOP.Parameters.Add("@NOM_CODE", SqlDbType.VarChar).Value = o2["payload"]["object"]["id"].ToString();
                    sToken = o2["payload"]["object"]["recipient"].ToString();
                    cmdInsConsZOOP.Parameters.Add("@NUM_TOKEN", SqlDbType.VarChar).Value = o2["payload"]["object"]["recipient"].ToString();
                    cmdInsConsZOOP.Parameters.Add("@NOM_TIPO", SqlDbType.VarChar).Value = o2["type"].ToString();
                    cmdInsConsZOOP.Parameters.Add("@FLG_STATUS", SqlDbType.VarChar).Value = o2["payload"]["object"]["status"].ToString();
                }
                catch
                {

                }
            }


        }

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
            HttpContext.Current.Session.Add("LICENCIADO", ReaderCadastroEstabelecimento["COD_ID_PESSOA_LICENCIADO"].ToString());
            HttpContext.Current.Session.Add("PESSOA", ReaderCadastroEstabelecimento["COD_ID_PESSOAS_FJ"].ToString());

            iEstabelecimento = Funcoes.strToInt(ReaderCadastroEstabelecimento["COD_ID_PESSOAS_FJ"].ToString());
            iLicenciado = Funcoes.strToInt(ReaderCadastroEstabelecimento["COD_ID_PESSOA_LICENCIADO"].ToString());
        }

        string iCodigoNotificacao = cmdInsConsZOOP.ExecuteScalar().ToString();
        txtZoop.Text = txtZoop.Text + "Notificação: " + iCodigoNotificacao.ToString() + " --- ";
        //cmdInsConsZOOP.ExecuteNonQuery();
        connInsConsZOOP.Close();
        connInsConsZOOP.Dispose();

        if (bodyText.ToString().Trim() != "")
        {

            string json2 = bodyText.ToString();
            JObject o2 = JObject.Parse(json2);
            string sType2 = o2["type"].ToString();

            // **************************************************************************************************************************************************************
            // Seller 
            // **************************************************************************************************************************************************************
            /*
            if ((sType2.Contains("seller") == true))
            {
                if (bodyText.ToString().Trim() != "")
                {
                    // SELLER
                    string json = bodyText.ToString();
                    try
                    {
                        JObject o = JObject.Parse(json);
                        string sType = o["type"].ToString();

                        SqlConnection connInsCons = new SqlConnection(Funcoes.conexao());
                        connInsCons.Open();
                        SqlCommand cmdInsCons = new SqlCommand("dbo.stp_pessoas_fj_vendedor_ins", connInsCons);
                        cmdInsCons.CommandType = CommandType.StoredProcedure;
                        cmdInsCons.Parameters.Add("@flg_operacao", SqlDbType.Char).Value = 'U';
                        cmdInsCons.Parameters.Add("@COD_ID_PESSOA_LICENCIADO", SqlDbType.Int).Value = iLicenciado;
                        cmdInsCons.Parameters.Add("@COD_ID_PESSOAS_FJ", SqlDbType.Int).Value = iEstabelecimento;

                        cmdInsCons.ExecuteNonQuery();
                        connInsCons.Close();
                        connInsCons.Dispose();
                    }
                    catch
                    {

                    }
                }
            }
            */
            // **************************************************************************************************************************************************************
            // Transferências 
            // **************************************************************************************************************************************************************
            
            if ((sType2.Contains("transfer") == true))
            {
                // Transferências
                SqlConnection connInsCons = new SqlConnection(Funcoes.conexao());
                connInsCons.Open();
                SqlCommand cmdInsCons = new SqlCommand("dbo.stp_extrato_ins", connInsCons);
                cmdInsCons.CommandType = CommandType.StoredProcedure;
                cmdInsCons.Parameters.Add("@flg_operacao", SqlDbType.Char).Value = 'I';



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

                    cmdInsCons.Parameters.Add("@NUM_VALOR", SqlDbType.Float).Value = Funcoes.strToDouble(o["payload"]["object"]["amount"].ToString())/100;
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

            // **************************************************************************************************************************************************************
            // Recebíveis
            // **************************************************************************************************************************************************************

            if ((sType2.Contains("receivable") == true))
            {
                // Recebimentos
                SqlConnection connInsCons = new SqlConnection(Funcoes.conexao());
                connInsCons.Open();
                SqlCommand cmdInsCons = new SqlCommand("dbo.stp_extrato_ins", connInsCons);
                cmdInsCons.CommandType = CommandType.StoredProcedure;
                cmdInsCons.Parameters.Add("@flg_operacao", SqlDbType.Char).Value = 'I';

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

            // **************************************************************************************************************************************************************
            // Transações com sucesso
            // **************************************************************************************************************************************************************


            if ((sType2.Contains("transaction.succeeded") == true) || (sType2.Contains("buyer.transaction.succeeded") == true))
            {
                // Transacoes
                SqlConnection connInsCons = new SqlConnection(sConexao);
                connInsCons.Open();
                SqlCommand cmdInsCons = new SqlCommand("dbo.stp_transacoes_ins", connInsCons);
                cmdInsCons.CommandType = CommandType.StoredProcedure;
                cmdInsCons.Parameters.Add("@flg_operacao", SqlDbType.Char).Value = 'I';

                cmdInsCons.Parameters.Add("@COD_ID_PESSOA_LICENCIADO", SqlDbType.Int).Value = iLicenciado;
                cmdInsCons.Parameters.Add("@COD_ID_PESSOAS_FJ", SqlDbType.Int).Value = iEstabelecimento;
                cmdInsCons.Parameters.Add("@FLG_ORIGEM", SqlDbType.Char).Value = "Z";
                txtZoop.Text = txtZoop.Text + iLicenciado.ToString() + "-" + iEstabelecimento.ToString() + " ";
                
                if (bodyText.ToString().Trim() != "")
                {
                    
                    string json = bodyText.ToString();
                    JObject o = JObject.Parse(json);
                    string sType = o["type"].ToString();

                    txtZoop.Text = txtZoop.Text + o["payload"]["object"]["id"].ToString() + " ";
                    
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
                    cmdInsCons.Parameters.Add("@NOM_TIPO_OPERACAO", SqlDbType.VarChar).Value = RetornaTipoPagamento(o["payload"]["object"]["payment_type"].ToString());



                    if ((o["payload"]["object"]["payment_type"].ToString() == "pix"))
                    {
                        cmdInsCons.Parameters.Add("@NOM_CODIGO_PAGAMENTO", SqlDbType.VarChar).Value = o["payload"]["object"]["payment_method"]["provider"].ToString();
                        cmdInsCons.Parameters.Add("@NUM_PARCELAS", SqlDbType.Int).Value = 1;
                        cmdInsCons.Parameters.Add("@NOM_QRCODE", SqlDbType.VarChar).Value = o["payload"]["object"]["payment_method"]["qr_code"]["emv"].ToString();
                        cmdInsCons.Parameters.Add("@NOM_TIPO_OPERACAO_BANDEIRA", SqlDbType.VarChar).Value = "PIX";

                    }
                    if ((o["payload"]["object"]["payment_type"].ToString() == "boleto"))
                    {
                        cmdInsCons.Parameters.Add("@NOM_CODIGO_PAGAMENTO", SqlDbType.VarChar).Value = o["payload"]["object"]["payment_method"]["resource"].ToString();
                        cmdInsCons.Parameters.Add("@NUM_PARCELAS", SqlDbType.Int).Value = 1;

                        cmdInsCons.Parameters.Add("@NOM_CODIGO_BARRAS", SqlDbType.VarChar).Value = o["payload"]["object"]["payment_method"]["barcode"].ToString();
                        cmdInsCons.Parameters.Add("@NOM_URL_BOLETO", SqlDbType.VarChar).Value = o["payload"]["object"]["payment_method"]["url"].ToString();
                        cmdInsCons.Parameters.Add("@NOM_NUMERO_DOCUMENTO", SqlDbType.VarChar).Value = o["payload"]["object"]["payment_method"]["document_number"].ToString();
                        cmdInsCons.Parameters.Add("@NOM_DATA_VENCIMENTO", SqlDbType.VarChar).Value = o["payload"]["object"]["payment_method"]["expiration_date"].ToString();
                        cmdInsCons.Parameters.Add("@NOM_TIPO_OPERACAO_BANDEIRA", SqlDbType.VarChar).Value = "Boleto";
                    }


                    if ((o["payload"]["object"]["payment_type"].ToString() == "credit"))
                    {
                        cmdInsCons.Parameters.Add("@NOM_CODIGO_PAGAMENTO", SqlDbType.VarChar).Value = o["payload"]["object"]["payment_method"]["resource"].ToString() + " " + o["payload"]["object"]["payment_method"]["card_brand"].ToString();
                        try { cmdInsCons.Parameters.Add("@NUM_PARCELAS", SqlDbType.Int).Value = Funcoes.strToInt(o["payload"]["object"]["installment_plan"]["number_installments"].ToString()); }
                        catch { }

                        //cmdInsCons.Parameters.Add("@NUM_PARCELAS", SqlDbType.Int).Value = Funcoes.strToInt(o["payload"]["object"]["installment_plan"]["number_installments"].ToString());

                        cmdInsCons.Parameters.Add("@NOM_CARTAO_4_PRIMEIROS_DIGIROS", SqlDbType.VarChar).Value = o["payload"]["object"]["payment_method"]["first4_digits"].ToString();
                        cmdInsCons.Parameters.Add("@NOM_TIPO_OPERACAO_BANDEIRA", SqlDbType.VarChar).Value = ObterBandeiraCartao(o["payload"]["object"]["payment_method"]["first4_digits"].ToString());
                        cmdInsCons.Parameters.Add("@NOM_CARTAO_4_ULTIMOS_DIGITOS", SqlDbType.VarChar).Value = o["payload"]["object"]["payment_method"]["last4_digits"].ToString();
                        cmdInsCons.Parameters.Add("@NOM_CARTAO_MES", SqlDbType.VarChar).Value = o["payload"]["object"]["payment_method"]["expiration_month"].ToString();
                        cmdInsCons.Parameters.Add("@NOM_CARTAO_ANO", SqlDbType.VarChar).Value = o["payload"]["object"]["payment_method"]["expiration_year"].ToString();
                    }

                    if ((o["payload"]["object"]["payment_type"].ToString() == "debit"))
                    {

                        cmdInsCons.Parameters.Add("@NOM_CODIGO_PAGAMENTO", SqlDbType.VarChar).Value = o["payload"]["object"]["payment_method"]["resource"].ToString() + " " + o["payload"]["object"]["payment_method"]["card_brand"].ToString();
                        cmdInsCons.Parameters.Add("@NUM_PARCELAS", SqlDbType.Int).Value = 1;

                        cmdInsCons.Parameters.Add("@NOM_CARTAO_4_PRIMEIROS_DIGIROS", SqlDbType.VarChar).Value = o["payload"]["object"]["payment_method"]["first4_digits"].ToString();
                        cmdInsCons.Parameters.Add("@NOM_TIPO_OPERACAO_BANDEIRA", SqlDbType.VarChar).Value = ObterBandeiraCartao(o["payload"]["object"]["payment_method"]["first4_digits"].ToString());
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
                        txtZoop.Text = txtZoop.Text + jsonRecibo.ToString()+"   ---   ";

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

                    // Carregar os recebíveis ----------------------------------------------------------

                    string jsonRecebiveis = zoop.DetalhesRecebiveis(o["payload"]["object"]["id"].ToString());
                    txtZoop.Text = txtZoop.Text + jsonRecebiveis.ToString();
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
                    }
                    catch
                    {

                    }
                    // --------------------------------------------------------------------------------------

                }
                cmdInsCons.ExecuteNonQuery();
                connInsCons.Close();
                connInsCons.Dispose();




                if (bodyText.ToString().Trim() != "")
                {
                    string json = bodyText.ToString();
                    JObject o = JObject.Parse(json);
                    string sType = o["type"].ToString();

                    // -------------------------------------- SPLIT POSTERIORI


                    string sSplitTransacao = "NÃO FORAM ENCONTRADOS SPLITS";
                    string sRetornoSplitTransacao = "NÃO POSSUI SPLIT A SER PROCESSADO";
                    string sFlgSplit = "N";
                    string sSplitRealizado = "N";

                    string PresencialOnlineDefinicao = "";

                    if ((o["payload"]["object"]["point_of_sale"]["identification_number"].ToString().Trim() == "") || (o["payload"]["object"]["point_of_sale"]["identification_number"].ToString().Trim() == "null"))
                    {
                        PresencialOnlineDefinicao = "O";
                    }
                    else
                    {
                        PresencialOnlineDefinicao = "P";
                    }


                    string soperacao = "";
                    soperacao = (o["payload"]["object"]["payment_type"].ToString() == "credit") ? "C" : "V";

                    int iparcelas = 1;

                    JToken payload = o["payload"];
                    if (payload != null)
                    {
                        JToken obj = payload["object"];
                        if (obj != null)
                        {
                            string paymentType = "";
                            if (obj["payment_type"] != null)
                            {
                                paymentType = obj["payment_type"].ToString();
                            }

                            if (paymentType == "credit")
                            {
                                JToken installmentPlan = obj["installment_plan"];
                                if (installmentPlan != null && installmentPlan.Type == JTokenType.Object)
                                {
                                    JToken numberInstallments = installmentPlan["number_installments"];
                                    if (numberInstallments != null)
                                    {
                                        iparcelas = Funcoes.strToInt(numberInstallments.ToString());
                                    }
                                }
                            }
                        }
                    }
                    string sbandeira = "";
                    if (o["payload"]["object"]["payment_type"].ToString() == "credit" || o["payload"]["object"]["payment_type"].ToString() == "debit")
                    {
                        sbandeira = ObterBandeiraCartao(o["payload"]["object"]["payment_method"]["first4_digits"].ToString());
                    }
                    if (o["payload"]["object"]["payment_type"].ToString() == "pix")
                    {
                        sbandeira = "PIX";
                    }
                    if (o["payload"]["object"]["payment_type"].ToString() == "boleto")
                    {
                        sbandeira = "Boleto";
                    }

                    string stipo = "";

                    if (o["payload"]["object"]["payment_type"].ToString() == "boleto")
                    {
                        stipo = "B";
                    }
                    if (o["payload"]["object"]["payment_type"].ToString() == "pix")
                    {
                        stipo = "P";
                    }

                    if (o["payload"]["object"]["payment_type"].ToString() == "credit")
                    {
                        stipo = "C";
                    }
                    if (o["payload"]["object"]["payment_type"].ToString() == "debit")
                    {
                        stipo = "D";
                    }

                    //if (/*(iEstabelecimento == 9) && */(iLicenciado == 1))
                    //{
                    //    try
                    //    {

                    string seller;
                    ZoopSplit[] zoopSplits = GerarSplitPresencialZoop(iLicenciado, iEstabelecimento, sbandeira, "Z", stipo, soperacao, PresencialOnlineDefinicao, iparcelas, out seller);
                    
                    if (zoopSplits != null && zoopSplits.Length > 0)
                    {
                        // VERIFICA SE JÁ EXISTE SPLIT PARA ESTA TRANSAÇÃO
                        
                        SqlConnection mySelCadastroSplit = new SqlConnection(sConexao);
                        mySelCadastroSplit.Open();
                        SqlCommand cmdSelCadastroSplit = new SqlCommand("dbo.stp_transacoes_ins", mySelCadastroSplit);
                        cmdSelCadastroSplit.CommandType = CommandType.StoredProcedure;
                        cmdSelCadastroSplit.Parameters.Add("@flg_operacao", SqlDbType.Char).Value = "O";
                        cmdSelCadastroSplit.Parameters.Add("@NOM_CODE", SqlDbType.VarChar).Value = o["payload"]["object"]["id"].ToString();
                        cmdSelCadastroSplit.Parameters.Add("@COD_ID_PESSOA_LICENCIADO", SqlDbType.Int).Value = iLicenciado;
                        cmdSelCadastroSplit.Parameters.Add("@COD_ID_PESSOAS_FJ", SqlDbType.Int).Value = iEstabelecimento;

                        SqlDataReader ReaderCadastroSplit = cmdSelCadastroSplit.ExecuteReader();
                        while (ReaderCadastroSplit.Read())
                        {
                            sSplitRealizado = ReaderCadastroSplit["FLG_SPLIT"].ToString();
                        }


                        if (sSplitRealizado == "N")
                        {

                            string jsonTransacao = JsonConvert.SerializeObject(zoopSplits);
                            sSplitTransacao = jsonTransacao;
                            zoop.HttpResponseResult resultado = zoop.TransacaoSplitPresencial(jsonTransacao, o["payload"]["object"]["id"].ToString());
                            if ((resultado.StatusCode == 200) || (resultado.StatusCode == 201) || (resultado.StatusCode == 202))
                            {
                                // Sucesso - Processar JSON normalmente
                                sFlgSplit = "S";
                                string jsonRetornoTransacao = resultado.Content;
                                sRetornoSplitTransacao = jsonRetornoTransacao;

                                // Processar arquivo de transações SPLIT

                                JArray oSplitGerado = JArray.Parse(jsonRetornoTransacao);

                                if (oSplitGerado.Count > 0)
                                {
                                    for (int i = 0; i < oSplitGerado.Count; i++)
                                    {
                                        SqlConnection connInsConsSplitTransacaoRetorno = new SqlConnection(Funcoes.conexao());
                                        connInsConsSplitTransacaoRetorno.Open();
                                        SqlCommand cmdInsConsSplitTransacaoRetorno = new SqlCommand("dbo.stp_transacoes_split_ins", connInsConsSplitTransacaoRetorno);
                                        cmdInsConsSplitTransacaoRetorno.CommandType = CommandType.StoredProcedure;
                                        cmdInsConsSplitTransacaoRetorno.Parameters.Add("@flg_operacao", SqlDbType.Char).Value = "I";
                                        cmdInsConsSplitTransacaoRetorno.Parameters.Add("@COD_ID_PESSOA_LICENCIADO", SqlDbType.Int).Value = iLicenciado;
                                        cmdInsConsSplitTransacaoRetorno.Parameters.Add("@COD_ID_PESSOAS_FJ", SqlDbType.Int).Value = iEstabelecimento;
                                        cmdInsConsSplitTransacaoRetorno.Parameters.Add("@NUM_VALOR", SqlDbType.Float).Value = Funcoes.strToDouble(oSplitGerado[i]["amount"].ToString())/100;
                                        cmdInsConsSplitTransacaoRetorno.Parameters.Add("@NUM_PERCENTUAL", SqlDbType.Float).Value = Funcoes.strToDouble(oSplitGerado[i]["percentage"].ToString())/100;
                                        cmdInsConsSplitTransacaoRetorno.Parameters.Add("@NUM_VALOR_SPLIT", SqlDbType.Float).Value = Funcoes.strToDouble(oSplitGerado[i]["receivable_amount"].ToString())/100;
                                        cmdInsConsSplitTransacaoRetorno.Parameters.Add("@DTA_DATA", SqlDbType.Date).Value = Convert.ToDateTime(oSplitGerado[i]["created_at"].ToString());
                                        cmdInsConsSplitTransacaoRetorno.Parameters.Add("@NOM_CODE_SPLIT", SqlDbType.VarChar).Value = oSplitGerado[i]["id"].ToString();
                                        cmdInsConsSplitTransacaoRetorno.Parameters.Add("@NOM_CODE", SqlDbType.VarChar).Value = o["payload"]["object"]["id"].ToString();

                                        cmdInsConsSplitTransacaoRetorno.ExecuteNonQuery();
                                        connInsConsSplitTransacaoRetorno.Close();
                                        connInsConsSplitTransacaoRetorno.Dispose();
                                    }
                                }
                            }
                            else
                            {
                                // Falha - Exibir erro conforme necessário
                                sFlgSplit = "N";
                                string mensagemErro = resultado.Content;
                                sRetornoSplitTransacao = mensagemErro;

                            }
                        }

                    }


                    //    }
                    //    catch
                    //    {
                    //        sSplitTransacao = "ERRO";
                    //        sRetornoSplitTransacao = "";
                    //    }
                    //}
                    // Gravar dados do split em transacoes

                    if (sSplitRealizado == "N")
                    {
                        SqlConnection connInsConsSplitTransacao = new SqlConnection(Funcoes.conexao());
                        connInsConsSplitTransacao.Open();
                        SqlCommand cmdInsConsSplitTransacao = new SqlCommand("dbo.stp_transacoes_ins", connInsConsSplitTransacao);
                        cmdInsConsSplitTransacao.CommandType = CommandType.StoredProcedure;
                        cmdInsConsSplitTransacao.Parameters.Add("@flg_operacao", SqlDbType.Char).Value = "L";
                        cmdInsConsSplitTransacao.Parameters.Add("@NOM_CODE", SqlDbType.VarChar).Value = o["payload"]["object"]["id"].ToString();
                        cmdInsConsSplitTransacao.Parameters.Add("@DES_SPLIT", SqlDbType.Text).Value = sSplitTransacao;

                        cmdInsConsSplitTransacao.Parameters.Add("@FLG_SPLIT", SqlDbType.Char).Value = sFlgSplit.ToString();
                        
                        cmdInsConsSplitTransacao.Parameters.Add("@DES_RETORNO_SPLIT", SqlDbType.Text).Value = sRetornoSplitTransacao;

                        cmdInsConsSplitTransacao.Parameters.Add("@COD_ID_PESSOA_LICENCIADO", SqlDbType.Int).Value = iLicenciado;
                        cmdInsConsSplitTransacao.Parameters.Add("@COD_ID_PESSOAS_FJ", SqlDbType.Int).Value = iEstabelecimento;

                        cmdInsConsSplitTransacao.ExecuteNonQuery();
                        connInsConsSplitTransacao.Close();
                        connInsConsSplitTransacao.Dispose();
                    }

                    // -------------------------------------- FIM SPLIT POSTERIORI

                }

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
                    cmdInsCons.Parameters.Add("@NOM_TIPO_OPERACAO", SqlDbType.VarChar).Value = RetornaTipoPagamento(o["payload"]["object"]["payment_type"].ToString());


                    if ((o["payload"]["object"]["payment_type"].ToString() == "pix"))
                    {
                        cmdInsCons.Parameters.Add("@NOM_CODIGO_PAGAMENTO", SqlDbType.VarChar).Value = o["payload"]["object"]["payment_method"]["provider"].ToString();
                        cmdInsCons.Parameters.Add("@NUM_PARCELAS", SqlDbType.Int).Value = 1;
                        cmdInsCons.Parameters.Add("@NOM_QRCODE", SqlDbType.VarChar).Value = o["payload"]["object"]["payment_method"]["qr_code"]["emv"].ToString();
                        cmdInsCons.Parameters.Add("@NOM_TIPO_OPERACAO_BANDEIRA", SqlDbType.VarChar).Value = "PIX";

                    }
                    if ((o["payload"]["object"]["payment_type"].ToString() == "boleto"))
                    {
                        cmdInsCons.Parameters.Add("@NOM_CODIGO_PAGAMENTO", SqlDbType.VarChar).Value = o["payload"]["object"]["payment_method"]["resource"].ToString();
                        cmdInsCons.Parameters.Add("@NUM_PARCELAS", SqlDbType.Int).Value = 1;

                        cmdInsCons.Parameters.Add("@NOM_CODIGO_BARRAS", SqlDbType.VarChar).Value = o["payload"]["object"]["payment_method"]["barcode"].ToString();
                        cmdInsCons.Parameters.Add("@NOM_URL_BOLETO", SqlDbType.VarChar).Value = o["payload"]["object"]["payment_method"]["url"].ToString();
                        cmdInsCons.Parameters.Add("@NOM_NUMERO_DOCUMENTO", SqlDbType.VarChar).Value = o["payload"]["object"]["payment_method"]["document_number"].ToString();
                        cmdInsCons.Parameters.Add("@NOM_DATA_VENCIMENTO", SqlDbType.VarChar).Value = o["payload"]["object"]["payment_method"]["expiration_date"].ToString();
                        cmdInsCons.Parameters.Add("@NOM_TIPO_OPERACAO_BANDEIRA", SqlDbType.VarChar).Value = "Boleto";
                    }


                    if ((o["payload"]["object"]["payment_type"].ToString() == "credit"))
                    {
                        cmdInsCons.Parameters.Add("@NOM_CODIGO_PAGAMENTO", SqlDbType.VarChar).Value = o["payload"]["object"]["payment_method"]["resource"].ToString() + " " + o["payload"]["object"]["payment_method"]["card_brand"].ToString();

                        try { cmdInsCons.Parameters.Add("@NUM_PARCELAS", SqlDbType.Int).Value = Funcoes.strToInt(o["payload"]["object"]["installment_plan"]["number_installments"].ToString()); }
                        catch {  }

                        //cmdInsCons.Parameters.Add("@NUM_PARCELAS", SqlDbType.Int).Value = Funcoes.strToInt(o["payload"]["object"]["installment_plan"]["number_installments"].ToString());

                        cmdInsCons.Parameters.Add("@NOM_CARTAO_4_PRIMEIROS_DIGIROS", SqlDbType.VarChar).Value = o["payload"]["object"]["payment_method"]["first4_digits"].ToString();
                        cmdInsCons.Parameters.Add("@NOM_TIPO_OPERACAO_BANDEIRA", SqlDbType.VarChar).Value = ObterBandeiraCartao(o["payload"]["object"]["payment_method"]["first4_digits"].ToString());
                        cmdInsCons.Parameters.Add("@NOM_CARTAO_4_ULTIMOS_DIGITOS", SqlDbType.VarChar).Value = o["payload"]["object"]["payment_method"]["last4_digits"].ToString();
                        cmdInsCons.Parameters.Add("@NOM_CARTAO_MES", SqlDbType.VarChar).Value = o["payload"]["object"]["payment_method"]["expiration_month"].ToString();
                        cmdInsCons.Parameters.Add("@NOM_CARTAO_ANO", SqlDbType.VarChar).Value = o["payload"]["object"]["payment_method"]["expiration_year"].ToString();
                    }

                    if ((o["payload"]["object"]["payment_type"].ToString() == "debit"))
                    {
                        cmdInsCons.Parameters.Add("@NOM_CODIGO_PAGAMENTO", SqlDbType.VarChar).Value = o["payload"]["object"]["payment_method"]["resource"].ToString() + " " + o["payload"]["object"]["payment_method"]["card_brand"].ToString();
                        cmdInsCons.Parameters.Add("@NUM_PARCELAS", SqlDbType.Int).Value = 1;

                        cmdInsCons.Parameters.Add("@NOM_CARTAO_4_PRIMEIROS_DIGIROS", SqlDbType.VarChar).Value = o["payload"]["object"]["payment_method"]["first4_digits"].ToString();
                        cmdInsCons.Parameters.Add("@NOM_TIPO_OPERACAO_BANDEIRA", SqlDbType.VarChar).Value = ObterBandeiraCartao(o["payload"]["object"]["payment_method"]["first4_digits"].ToString());
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
                    catch {  }

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
                    cmdInsCons.Parameters.Add("@NOM_TIPO_OPERACAO", SqlDbType.VarChar).Value = RetornaTipoPagamento(o["payload"]["object"]["payment_type"].ToString());


                    if ((o["payload"]["object"]["payment_type"].ToString() == "pix"))
                    {
                        cmdInsCons.Parameters.Add("@NOM_CODIGO_PAGAMENTO", SqlDbType.VarChar).Value = o["payload"]["object"]["payment_method"]["provider"].ToString();
                        cmdInsCons.Parameters.Add("@NUM_PARCELAS", SqlDbType.Int).Value = 1;
                        cmdInsCons.Parameters.Add("@NOM_QRCODE", SqlDbType.VarChar).Value = o["payload"]["object"]["payment_method"]["qr_code"]["emv"].ToString();
                        cmdInsCons.Parameters.Add("@NOM_TIPO_OPERACAO_BANDEIRA", SqlDbType.VarChar).Value = "PIX";

                    }
                    if ((o["payload"]["object"]["payment_type"].ToString() == "boleto"))
                    {
                        cmdInsCons.Parameters.Add("@NOM_CODIGO_PAGAMENTO", SqlDbType.VarChar).Value = o["payload"]["object"]["payment_method"]["resource"].ToString();
                        cmdInsCons.Parameters.Add("@NUM_PARCELAS", SqlDbType.Int).Value = 1;

                        cmdInsCons.Parameters.Add("@NOM_CODIGO_BARRAS", SqlDbType.VarChar).Value = o["payload"]["object"]["payment_method"]["barcode"].ToString();
                        cmdInsCons.Parameters.Add("@NOM_URL_BOLETO", SqlDbType.VarChar).Value = o["payload"]["object"]["payment_method"]["url"].ToString();
                        cmdInsCons.Parameters.Add("@NOM_NUMERO_DOCUMENTO", SqlDbType.VarChar).Value = o["payload"]["object"]["payment_method"]["document_number"].ToString();
                        cmdInsCons.Parameters.Add("@NOM_DATA_VENCIMENTO", SqlDbType.VarChar).Value = o["payload"]["object"]["payment_method"]["expiration_date"].ToString();
                        cmdInsCons.Parameters.Add("@NOM_TIPO_OPERACAO_BANDEIRA", SqlDbType.VarChar).Value = "Boleto";
                    }


                    if ((o["payload"]["object"]["payment_type"].ToString() == "credit"))
                    {
                        cmdInsCons.Parameters.Add("@NOM_CODIGO_PAGAMENTO", SqlDbType.VarChar).Value = o["payload"]["object"]["payment_method"]["resource"].ToString() + " " + o["payload"]["object"]["payment_method"]["card_brand"].ToString();

                        try { cmdInsCons.Parameters.Add("@NUM_PARCELAS", SqlDbType.Int).Value = Funcoes.strToInt(o["payload"]["object"]["installment_plan"]["number_installments"].ToString()); }
                        catch {  }

                        //cmdInsCons.Parameters.Add("@NUM_PARCELAS", SqlDbType.Int).Value = Funcoes.strToInt(o["payload"]["object"]["installment_plan"]["number_installments"].ToString());

                        cmdInsCons.Parameters.Add("@NOM_CARTAO_4_PRIMEIROS_DIGIROS", SqlDbType.VarChar).Value = o["payload"]["object"]["payment_method"]["first4_digits"].ToString();
                        cmdInsCons.Parameters.Add("@NOM_TIPO_OPERACAO_BANDEIRA", SqlDbType.VarChar).Value = ObterBandeiraCartao(o["payload"]["object"]["payment_method"]["first4_digits"].ToString());
                        cmdInsCons.Parameters.Add("@NOM_CARTAO_4_ULTIMOS_DIGITOS", SqlDbType.VarChar).Value = o["payload"]["object"]["payment_method"]["last4_digits"].ToString();
                        cmdInsCons.Parameters.Add("@NOM_CARTAO_MES", SqlDbType.VarChar).Value = o["payload"]["object"]["payment_method"]["expiration_month"].ToString();
                        cmdInsCons.Parameters.Add("@NOM_CARTAO_ANO", SqlDbType.VarChar).Value = o["payload"]["object"]["payment_method"]["expiration_year"].ToString();
                    }

                    if ((o["payload"]["object"]["payment_type"].ToString() == "debit"))
                    {
                        cmdInsCons.Parameters.Add("@NOM_CODIGO_PAGAMENTO", SqlDbType.VarChar).Value = o["payload"]["object"]["payment_method"]["resource"].ToString() + " " + o["payload"]["object"]["payment_method"]["card_brand"].ToString();
                        cmdInsCons.Parameters.Add("@NUM_PARCELAS", SqlDbType.Int).Value = 1;

                        cmdInsCons.Parameters.Add("@NOM_CARTAO_4_PRIMEIROS_DIGIROS", SqlDbType.VarChar).Value = o["payload"]["object"]["payment_method"]["first4_digits"].ToString();
                        cmdInsCons.Parameters.Add("@NOM_TIPO_OPERACAO_BANDEIRA", SqlDbType.VarChar).Value = ObterBandeiraCartao(o["payload"]["object"]["payment_method"]["first4_digits"].ToString());
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
                    catch {  }

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
                    cmdInsCons.Parameters.Add("@NOM_TIPO_OPERACAO", SqlDbType.VarChar).Value = RetornaTipoPagamento(o["payload"]["object"]["payment_type"].ToString());


                    if ((o["payload"]["object"]["payment_type"].ToString() == "pix"))
                    {
                        cmdInsCons.Parameters.Add("@NOM_CODIGO_PAGAMENTO", SqlDbType.VarChar).Value = o["payload"]["object"]["payment_method"]["provider"].ToString();
                        cmdInsCons.Parameters.Add("@NUM_PARCELAS", SqlDbType.Int).Value = 1;
                        cmdInsCons.Parameters.Add("@NOM_QRCODE", SqlDbType.VarChar).Value = o["payload"]["object"]["payment_method"]["qr_code"]["emv"].ToString();
                        cmdInsCons.Parameters.Add("@NOM_TIPO_OPERACAO_BANDEIRA", SqlDbType.VarChar).Value = "PIX";

                    }
                    if ((o["payload"]["object"]["payment_type"].ToString() == "boleto"))
                    {
                        cmdInsCons.Parameters.Add("@NOM_CODIGO_PAGAMENTO", SqlDbType.VarChar).Value = o["payload"]["object"]["payment_method"]["resource"].ToString();
                        cmdInsCons.Parameters.Add("@NUM_PARCELAS", SqlDbType.Int).Value = 1;

                        cmdInsCons.Parameters.Add("@NOM_CODIGO_BARRAS", SqlDbType.VarChar).Value = o["payload"]["object"]["payment_method"]["barcode"].ToString();
                        cmdInsCons.Parameters.Add("@NOM_URL_BOLETO", SqlDbType.VarChar).Value = o["payload"]["object"]["payment_method"]["url"].ToString();
                        cmdInsCons.Parameters.Add("@NOM_NUMERO_DOCUMENTO", SqlDbType.VarChar).Value = o["payload"]["object"]["payment_method"]["document_number"].ToString();
                        cmdInsCons.Parameters.Add("@NOM_DATA_VENCIMENTO", SqlDbType.VarChar).Value = o["payload"]["object"]["payment_method"]["expiration_date"].ToString();
                        cmdInsCons.Parameters.Add("@NOM_TIPO_OPERACAO_BANDEIRA", SqlDbType.VarChar).Value = "Boleto";
                    }


                    if ((o["payload"]["object"]["payment_type"].ToString() == "credit"))
                    {
                        cmdInsCons.Parameters.Add("@NOM_CODIGO_PAGAMENTO", SqlDbType.VarChar).Value = o["payload"]["object"]["payment_method"]["resource"].ToString() + " " + o["payload"]["object"]["payment_method"]["card_brand"].ToString();

                        try { cmdInsCons.Parameters.Add("@NUM_PARCELAS", SqlDbType.Int).Value = Funcoes.strToInt(o["payload"]["object"]["installment_plan"]["number_installments"].ToString()); }
                        catch {  }

                        //cmdInsCons.Parameters.Add("@NUM_PARCELAS", SqlDbType.Int).Value = Funcoes.strToInt(o["payload"]["object"]["installment_plan"]["number_installments"].ToString());

                        cmdInsCons.Parameters.Add("@NOM_CARTAO_4_PRIMEIROS_DIGIROS", SqlDbType.VarChar).Value = o["payload"]["object"]["payment_method"]["first4_digits"].ToString();
                        cmdInsCons.Parameters.Add("@NOM_TIPO_OPERACAO_BANDEIRA", SqlDbType.VarChar).Value = ObterBandeiraCartao(o["payload"]["object"]["payment_method"]["first4_digits"].ToString());
                        cmdInsCons.Parameters.Add("@NOM_CARTAO_4_ULTIMOS_DIGITOS", SqlDbType.VarChar).Value = o["payload"]["object"]["payment_method"]["last4_digits"].ToString();
                        cmdInsCons.Parameters.Add("@NOM_CARTAO_MES", SqlDbType.VarChar).Value = o["payload"]["object"]["payment_method"]["expiration_month"].ToString();
                        cmdInsCons.Parameters.Add("@NOM_CARTAO_ANO", SqlDbType.VarChar).Value = o["payload"]["object"]["payment_method"]["expiration_year"].ToString();
                    }

                    if ((o["payload"]["object"]["payment_type"].ToString() == "debit"))
                    {
                        cmdInsCons.Parameters.Add("@NOM_CODIGO_PAGAMENTO", SqlDbType.VarChar).Value = o["payload"]["object"]["payment_method"]["resource"].ToString() + " " + o["payload"]["object"]["payment_method"]["card_brand"].ToString();
                        cmdInsCons.Parameters.Add("@NUM_PARCELAS", SqlDbType.Int).Value = 1;

                        cmdInsCons.Parameters.Add("@NOM_CARTAO_4_PRIMEIROS_DIGIROS", SqlDbType.VarChar).Value = o["payload"]["object"]["payment_method"]["first4_digits"].ToString();
                        cmdInsCons.Parameters.Add("@NOM_TIPO_OPERACAO_BANDEIRA", SqlDbType.VarChar).Value = ObterBandeiraCartao(o["payload"]["object"]["payment_method"]["first4_digits"].ToString());
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
                    catch {  }

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
                    cmdInsCons.Parameters.Add("@NOM_TIPO_OPERACAO", SqlDbType.VarChar).Value = RetornaTipoPagamento(o["payload"]["object"]["payment_type"].ToString());


                    if ((o["payload"]["object"]["payment_type"].ToString() == "pix"))
                    {
                        cmdInsCons.Parameters.Add("@NOM_CODIGO_PAGAMENTO", SqlDbType.VarChar).Value = o["payload"]["object"]["payment_method"]["provider"].ToString();
                        cmdInsCons.Parameters.Add("@NUM_PARCELAS", SqlDbType.Int).Value = 1;
                        cmdInsCons.Parameters.Add("@NOM_QRCODE", SqlDbType.VarChar).Value = o["payload"]["object"]["payment_method"]["qr_code"]["emv"].ToString();
                        cmdInsCons.Parameters.Add("@NOM_TIPO_OPERACAO_BANDEIRA", SqlDbType.VarChar).Value = "PIX";

                    }
                    if ((o["payload"]["object"]["payment_type"].ToString() == "boleto"))
                    {
                        cmdInsCons.Parameters.Add("@NOM_CODIGO_PAGAMENTO", SqlDbType.VarChar).Value = o["payload"]["object"]["payment_method"]["resource"].ToString();
                        cmdInsCons.Parameters.Add("@NUM_PARCELAS", SqlDbType.Int).Value = 1;

                        cmdInsCons.Parameters.Add("@NOM_CODIGO_BARRAS", SqlDbType.VarChar).Value = o["payload"]["object"]["payment_method"]["barcode"].ToString();
                        cmdInsCons.Parameters.Add("@NOM_URL_BOLETO", SqlDbType.VarChar).Value = o["payload"]["object"]["payment_method"]["url"].ToString();
                        cmdInsCons.Parameters.Add("@NOM_NUMERO_DOCUMENTO", SqlDbType.VarChar).Value = o["payload"]["object"]["payment_method"]["document_number"].ToString();
                        cmdInsCons.Parameters.Add("@NOM_DATA_VENCIMENTO", SqlDbType.VarChar).Value = o["payload"]["object"]["payment_method"]["expiration_date"].ToString();
                        cmdInsCons.Parameters.Add("@NOM_TIPO_OPERACAO_BANDEIRA", SqlDbType.VarChar).Value = "Boleto";
                    }

                    if ((o["payload"]["object"]["payment_type"].ToString() == "credit"))
                    {
                        cmdInsCons.Parameters.Add("@NOM_CODIGO_PAGAMENTO", SqlDbType.VarChar).Value = o["payload"]["object"]["payment_method"]["resource"].ToString() + " " + o["payload"]["object"]["payment_method"]["card_brand"].ToString();

                        try { cmdInsCons.Parameters.Add("@NUM_PARCELAS", SqlDbType.Int).Value = Funcoes.strToInt(o["payload"]["object"]["installment_plan"]["number_installments"].ToString()); }
                        catch {  }

                        //cmdInsCons.Parameters.Add("@NUM_PARCELAS", SqlDbType.Int).Value = Funcoes.strToInt(o["payload"]["object"]["installment_plan"]["number_installments"].ToString());

                        cmdInsCons.Parameters.Add("@NOM_CARTAO_4_PRIMEIROS_DIGIROS", SqlDbType.VarChar).Value = o["payload"]["object"]["payment_method"]["first4_digits"].ToString();
                        cmdInsCons.Parameters.Add("@NOM_TIPO_OPERACAO_BANDEIRA", SqlDbType.VarChar).Value = ObterBandeiraCartao(o["payload"]["object"]["payment_method"]["first4_digits"].ToString());
                        cmdInsCons.Parameters.Add("@NOM_CARTAO_4_ULTIMOS_DIGITOS", SqlDbType.VarChar).Value = o["payload"]["object"]["payment_method"]["last4_digits"].ToString();
                        cmdInsCons.Parameters.Add("@NOM_CARTAO_MES", SqlDbType.VarChar).Value = o["payload"]["object"]["payment_method"]["expiration_month"].ToString();
                        cmdInsCons.Parameters.Add("@NOM_CARTAO_ANO", SqlDbType.VarChar).Value = o["payload"]["object"]["payment_method"]["expiration_year"].ToString();
                    }

                    if ((o["payload"]["object"]["payment_type"].ToString() == "debit"))
                    {
                        cmdInsCons.Parameters.Add("@NOM_CODIGO_PAGAMENTO", SqlDbType.VarChar).Value = o["payload"]["object"]["payment_method"]["resource"].ToString() + " " + o["payload"]["object"]["payment_method"]["card_brand"].ToString();
                        cmdInsCons.Parameters.Add("@NUM_PARCELAS", SqlDbType.Int).Value = 1;

                        cmdInsCons.Parameters.Add("@NOM_CARTAO_4_PRIMEIROS_DIGIROS", SqlDbType.VarChar).Value = o["payload"]["object"]["payment_method"]["first4_digits"].ToString();
                        cmdInsCons.Parameters.Add("@NOM_TIPO_OPERACAO_BANDEIRA", SqlDbType.VarChar).Value = ObterBandeiraCartao(o["payload"]["object"]["payment_method"]["first4_digits"].ToString());
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
                    catch {  }

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
                    try
                    {
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
                    }
                    catch
                    {
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
        cmdInsConsU.Parameters.Add("@flg_operacao", SqlDbType.Char).Value = "U";
        cmdInsConsU.Parameters.Add("@COD_ID", SqlDbType.Int).Value = iCodigoNotificacao.ToString();

        txtZoop.Text = txtZoop.Text + " " + "Atualização Notificação: " + iCodigoNotificacao.ToString() + " --- ";

        cmdInsConsU.ExecuteNonQuery();
        connInsConsU.Close();
        connInsConsU.Dispose();


        // Processa os dados diretamente no arquivo de transações
        
        /*
        string json = bodyText.ToString();
        JObject o = JObject.Parse(json);
        string sType = o["type"].ToString();

        if ((sType.Contains("transaction") == true) || (sType.Contains("buyer.transaction") == true))
        {

            // Localizar os dados do Estabelecimento
            int iEstabelecimento = 0;

            SqlConnection mySelCadastroEstabelecimento = new SqlConnection(Funcoes.conexao());
            mySelCadastroEstabelecimento.Open();
            SqlCommand cmdSelCadastroEstabelecimento = new SqlCommand("dbo.stp_pessoas_fj_ins", mySelCadastroEstabelecimento);
            cmdSelCadastroEstabelecimento.CommandType = CommandType.StoredProcedure;
            cmdSelCadastroEstabelecimento.Parameters.Add("@flg_operacao", SqlDbType.Char).Value = "Z";
            cmdSelCadastroEstabelecimento.Parameters.Add("@NUM_TOKEN", SqlDbType.VarChar).Value = o["payload"]["object"]["on_behalf_of"].ToString();
            SqlDataReader ReaderCadastroEstabelecimento = cmdSelCadastroEstabelecimento.ExecuteReader();
            while (ReaderCadastroEstabelecimento.Read())
            {
                iEstabelecimento = Funcoes.strToInt(ReaderCadastroEstabelecimento["COD_ID_PESSOAS_FJ"].ToString());
            }


            SqlConnection connInsCons = new SqlConnection(Funcoes.conexao());
            connInsCons.Open();
            SqlCommand cmdInsCons = new SqlCommand("dbo.stp_transacoes_ins", connInsCons);
            cmdInsCons.CommandType = CommandType.StoredProcedure;
            cmdInsCons.Parameters.Add("@flg_operacao", SqlDbType.Char).Value = 'I';
            cmdInsCons.Parameters.Add("@COD_ID_PESSOA_LICENCIADO", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["LICENCIADO"].ToString());
            cmdInsCons.Parameters.Add("@COD_ID_PESSOAS_FJ", SqlDbType.Int).Value = iEstabelecimento;
            cmdInsCons.Parameters.Add("@FLG_ORIGEM", SqlDbType.Char).Value = "Z";

            cmdInsCons.Parameters.Add("@DTA_DATA", SqlDbType.DateTime).Value = Convert.ToDateTime(o["payload"]["object"]["created_at"].ToString());
            cmdInsCons.Parameters.Add("@NOM_CODE", SqlDbType.VarChar).Value = o["payload"]["object"]["id"].ToString();
            cmdInsCons.Parameters.Add("@NUM_TOKEN", SqlDbType.VarChar).Value = o["payload"]["object"]["on_behalf_of"].ToString();


            cmdInsCons.Parameters.Add("@COD_ID_TIPO", SqlDbType.Int).Value = 1;
            cmdInsCons.Parameters.Add("@NOM_STATUS", SqlDbType.VarChar).Value = o["payload"]["object"]["status"].ToString();
            //cmdInsCons.Parameters.Add("@DTA_DATA_ULTIMA_ATUALIZACAO", SqlDbType.DateTime).Value = Convert.ToDateTime(o["payload"]["object"]["updated_at"].ToString());


            cmdInsCons.Parameters.Add("@NOM_TIPO_PAGAMENTO", SqlDbType.VarChar).Value = o["payload"]["object"]["payment_type"].ToString();

            if ((o["payload"]["object"]["payment_type"].ToString() == "pix"))
            {
                cmdInsCons.Parameters.Add("@NOM_CODIGO_PAGAMENTO", SqlDbType.VarChar).Value = o["payload"]["object"]["payment_method"]["provider"].ToString();
            }
            if ((o["payload"]["object"]["payment_type"].ToString() == "credit") || (o["payload"]["object"]["payment_type"].ToString() == "debit"))
            {
                cmdInsCons.Parameters.Add("@NOM_CODIGO_PAGAMENTO", SqlDbType.VarChar).Value = o["payload"]["object"]["payment_method"]["resource"].ToString() + " " + o["payload"]["object"]["payment_method"]["card_brand"].ToString();
            }

            cmdInsCons.Parameters.Add("@NUM_VALOR_BRUTO", SqlDbType.Float).Value = Funcoes.strToDouble(o["payload"]["object"]["amount"].ToString()) / 100;
            cmdInsCons.Parameters.Add("@NUM_VALOR_DESCONTO", SqlDbType.Float).Value = 0;
            cmdInsCons.Parameters.Add("@NUM_VALOR_INTERMEDIACAO_RATE", SqlDbType.Float).Value = 0;
            cmdInsCons.Parameters.Add("@NUM_VALOR_INTERMEDIACAO_FEE", SqlDbType.Float).Value = Funcoes.strToDouble(o["payload"]["object"]["fees"].ToString()) / 100;
            cmdInsCons.Parameters.Add("@NUM_VALOR_LIQUIDO", SqlDbType.Float).Value = Funcoes.strToDouble(o["payload"]["object"]["amount"].ToString()) / 100;
            cmdInsCons.Parameters.Add("@NUM_VALOR_EXTRAS", SqlDbType.Float).Value = 0;

            cmdInsCons.Parameters.Add("@DTA_DATA_DEPOSITO", SqlDbType.DateTime).Value = Convert.ToDateTime(o["payload"]["object"]["created_at"].ToString());

            cmdInsCons.Parameters.Add("@NUM_ITENS", SqlDbType.Int).Value = 1;

            cmdInsCons.Parameters.Add("@NOM_FONTE_CANCELAMENTO", SqlDbType.VarChar).Value = "";
            cmdInsCons.Parameters.Add("@NOM_ORIGEM", SqlDbType.VarChar).Value = o["payload"]["object"]["history"][1]["authorizer"].ToString();

            if (o["payload"]["object"]["payment_method"].Contains("holder_name") == true)
            {
                cmdInsCons.Parameters.Add("@NOM_CLIENTE", SqlDbType.VarChar).Value = o["payload"]["object"]["payment_method"]["holder_name"].ToString();
            }

            cmdInsCons.Parameters.Add("@NOM_AUTORIZACAO", SqlDbType.VarChar).Value = o["payload"]["object"]["transaction_number"].ToString();


            //cmdInsCons.Parameters.Add("@NUM_CHAVE_PUBLICA", SqlDbType.VarChar).Value = dataDictionary["transaction.primaryReceiver.publicKey"];
            //cmdInsCons.Parameters.Add("@NUM_DEVICE_REFERENCIA", SqlDbType.VarChar).Value = (dataDictionary.ContainsKey("transaction.deviceInfo.reference")) ? dataDictionary["transaction.deviceInfo.reference"] : "";
            //cmdInsCons.Parameters.Add("@NUM_DEVICE_BIN", SqlDbType.Char).Value = (dataDictionary.ContainsKey("transaction.deviceInfo.bin")) ? dataDictionary["transaction.deviceInfo.bin"] : "";
            //cmdInsCons.Parameters.Add("@NUM_DEVICE_TITULAR", SqlDbType.VarChar).Value = (dataDictionary.ContainsKey("transaction.deviceInfo.holder")) ? dataDictionary["transaction.deviceInfo.holder"] : "";
            //cmdInsCons.Parameters.Add("@NUM_DEVICE_SERIAL", SqlDbType.VarChar).Value = (dataDictionary.ContainsKey("transaction.deviceInfo.serialNumber")) ? dataDictionary["transaction.deviceInfo.serialNumber"] : "";

            //cmdInsCons.Parameters.Add("@DTA_PIX", SqlDbType.DateTime).Value = (dataDictionary.ContainsKey("transaction.pix.pixDate")) ? Convert.ToDateTime(dataDictionary["transaction.pix.pixDate"]) : dt;
            //cmdInsCons.Parameters.Add("@NOM_PIX_NOME", SqlDbType.VarChar).Value = (dataDictionary.ContainsKey("transaction.pix.holderName")) ? dataDictionary["transaction.pix.holderName"] : "";
            //cmdInsCons.Parameters.Add("@NOM_PIX_TIPO_FJ", SqlDbType.VarChar).Value = (dataDictionary.ContainsKey("transaction.pix.personType")) ? dataDictionary["transaction.pix.personType"] : "";
            //cmdInsCons.Parameters.Add("@NOM_PIX_NOME_BANCO", SqlDbType.VarChar).Value = (dataDictionary.ContainsKey("transaction.pix.bankName")) ? dataDictionary["transaction.pix.bankName"] : "";
            //cmdInsCons.Parameters.Add("@NOM_PIX_AGENCIA_BANCO", SqlDbType.VarChar).Value = (dataDictionary.ContainsKey("transaction.pix.bankAgency")) ? dataDictionary["transaction.pix.bankAgency"] : "";
            //cmdInsCons.Parameters.Add("@NOM_PIX_CONTA_BANCO", SqlDbType.VarChar).Value = (dataDictionary.ContainsKey("transaction.pix.bankAccount")) ? dataDictionary["transaction.pix.bankAccount"] : "";
            //cmdInsCons.Parameters.Add("@NOM_PIX_TIPO_CONTA_BANCO", SqlDbType.VarChar).Value = (dataDictionary.ContainsKey("transaction.pix.bankAccountType")) ? dataDictionary["transaction.pix.bankAccountType"] : "";
            //cmdInsCons.Parameters.Add("@DES_DESCRICAO", SqlDbType.Text).Value = (dataDictionary.ContainsKey("transaction.items.item.description")) ? dataDictionary["transaction.items.item.description"] : "";

            cmdInsCons.ExecuteNonQuery();
            connInsCons.Close();
            connInsCons.Dispose();
            // Atualiza os dados da Notificação
            
            SqlConnection connInsConsU = new SqlConnection(Funcoes.conexao());
            connInsConsU.Open();
            SqlCommand cmdInsConsU = new SqlCommand("dbo.stp_notificacoes_zoop_ins", connInsConsU);
            cmdInsConsU.CommandType = CommandType.StoredProcedure;
            cmdInsConsU.Parameters.Add("@flg_operacao", SqlDbType.Char).Value = "U";
            cmdInsConsU.Parameters.Add("@COD_ID", SqlDbType.Int).Value = sCodigo.ToString();

            cmdInsConsU.ExecuteNonQuery();
            connInsConsU.Close();
            connInsConsU.Dispose();
            

        }
    */
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


    public class ZoopSplit
    {
        public string recipient { get; set; }
        public bool charge_processing_fee { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public int? amount { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public double? percentage { get; set; }
    }

    public static ZoopSplit[] GerarSplitPresencialZoop(int licenciado, int pessoa, string nomeBandeira, string flgIntegracao, string flgTipo, string flgoperacao, string flgpresencialonline, int parcelas, out string sellerToken)
    {

        List<ZoopSplit> listaSplits = new List<ZoopSplit>();
        sellerToken = "";

        // SPLIT DA TABELA DE MARKUP POR EC
        using (var conn00 = new SqlConnection(Funcoes.conexao()))
        {
            conn00.Open();
            using (var cmd00 = new SqlCommand("dbo.stp_planos_parcelas_ins", conn00))
            {
                cmd00.CommandType = CommandType.StoredProcedure;
                cmd00.Parameters.Add("@flg_operacao", SqlDbType.Char).Value = "T";
                cmd00.Parameters.Add("@COD_ID_PESSOAS_FJ", SqlDbType.Int).Value = pessoa;
                cmd00.Parameters.Add("@COD_ID_PESSOA_LICENCIADO", SqlDbType.Int).Value = licenciado;
                cmd00.Parameters.Add("@NOM_BANDEIRA", SqlDbType.VarChar).Value = nomeBandeira;
                cmd00.Parameters.Add("@FLG_INTEGRACAO", SqlDbType.Char).Value = flgIntegracao;
                cmd00.Parameters.Add("@FLG_PRESENCIAL_ONLINE", SqlDbType.Char).Value = flgpresencialonline;
                cmd00.Parameters.Add("@NUM_PARCELAS", SqlDbType.Int).Value = parcelas;
                cmd00.Parameters.Add("@FLG_TIPO", SqlDbType.Char).Value = flgoperacao;

                using (var reader00 = cmd00.ExecuteReader())
                {
                    while (reader00.Read())
                    {
                        if (Funcoes.strToDouble(reader00["NUM_VALOR"].ToString()) > 0 &&
                            !string.IsNullOrWhiteSpace(reader00["NOM_CHAVE_SPLIT"].ToString()))
                        {

                            listaSplits.Add(new ZoopSplit()
                            {
                                recipient = reader00["NOM_CHAVE_SPLIT"].ToString(),
                                charge_processing_fee = false,
                                amount = Funcoes.strToInt((Funcoes.strToDouble(reader00["NUM_VALOR"].ToString()) * 100).ToString())
                            });
                        }
                        if (Funcoes.strToDouble(reader00["NUM_PERCENTUAL"].ToString()) > 0 &&
                            !string.IsNullOrWhiteSpace(reader00["NOM_CHAVE_SPLIT"].ToString()))
                        {

                            listaSplits.Add(new ZoopSplit()
                            {
                                recipient = reader00["NOM_CHAVE_SPLIT"].ToString(),
                                charge_processing_fee = false,
                                percentage = Funcoes.strToDouble(reader00["NUM_PERCENTUAL"].ToString())
                            });
                        }

                        sellerToken = reader00["NUM_TOKEN"].ToString();
                    }
                }
            }
        }


        using (var conn01 = new SqlConnection(Funcoes.conexao()))
        {
            conn01.Open();
            using (var cmd01 = new SqlCommand("dbo.stp_pessoas_fj_markup_parcelas_ins", conn01))
            {
                cmd01.CommandType = CommandType.StoredProcedure;
                cmd01.Parameters.Add("@flg_operacao", SqlDbType.Char).Value = "T";
                cmd01.Parameters.Add("@COD_ID_PESSOAS_FJ", SqlDbType.Int).Value = pessoa;
                cmd01.Parameters.Add("@COD_ID_PESSOA_LICENCIADO", SqlDbType.Int).Value = licenciado;
                cmd01.Parameters.Add("@NOM_BANDEIRA", SqlDbType.VarChar).Value = nomeBandeira;
                cmd01.Parameters.Add("@FLG_INTEGRACAO", SqlDbType.Char).Value = flgIntegracao;
                cmd01.Parameters.Add("@FLG_PRESENCIAL_ONLINE", SqlDbType.Char).Value = flgpresencialonline;
                cmd01.Parameters.Add("@NUM_PARCELAS", SqlDbType.Int).Value = parcelas;
                cmd01.Parameters.Add("@FLG_TIPO", SqlDbType.Char).Value = flgoperacao;

                using (var reader01 = cmd01.ExecuteReader())
                {
                    while (reader01.Read())
                    {
                        if (Funcoes.strToDouble(reader01["NUM_VALOR"].ToString()) > 0 &&
                            !string.IsNullOrWhiteSpace(reader01["NOM_CHAVE_SPLIT"].ToString()))
                        {
                            listaSplits.Add(new ZoopSplit()
                            {
                                recipient = reader01["NOM_CHAVE_SPLIT"].ToString(),
                                charge_processing_fee = false,
                                amount = Funcoes.strToInt((Funcoes.strToDouble(reader01["NUM_VALOR"].ToString()) * 100).ToString())
                            });
                        }
                        if (Funcoes.strToDouble(reader01["NUM_PERCENTUAL"].ToString()) > 0 &&
                            !string.IsNullOrWhiteSpace(reader01["NOM_CHAVE_SPLIT"].ToString()))
                        {
                            listaSplits.Add(new ZoopSplit()
                            {
                                recipient = reader01["NOM_CHAVE_SPLIT"].ToString(),
                                charge_processing_fee = false,
                                percentage = Funcoes.strToDouble(reader01["NUM_PERCENTUAL"].ToString())
                            });
                        }

                        sellerToken = reader01["NUM_TOKEN"].ToString();
                    }
                }
            }
        }


        // SPLIT DA TABELA DE SPLIT POR EC

        using (var conn02 = new SqlConnection(Funcoes.conexao()))
        {
            conn02.Open();
            using (var cmd02 = new SqlCommand("dbo.stp_pessoas_fj_split_ins", conn02))
            {
                cmd02.CommandType = CommandType.StoredProcedure;
                cmd02.Parameters.Add("@flg_operacao", SqlDbType.Char).Value = "T";
                cmd02.Parameters.Add("@COD_ID_PESSOAS_FJ", SqlDbType.Int).Value = pessoa;
                cmd02.Parameters.Add("@COD_ID_PESSOA_LICENCIADO", SqlDbType.Int).Value = licenciado;
                cmd02.Parameters.Add("@FLG_TIPO", SqlDbType.VarChar).Value = flgTipo;
                cmd02.Parameters.Add("@FLG_INTEGRACAO", SqlDbType.Char).Value = flgIntegracao;

                using (var reader02 = cmd02.ExecuteReader())
                {
                    while (reader02.Read())
                    {

                        if (Funcoes.strToDouble(reader02["NUM_VALOR"].ToString()) > 0 &&
                            !string.IsNullOrWhiteSpace(reader02["NOM_CHAVE_SPLIT"].ToString()))
                        {

                            listaSplits.Add(new ZoopSplit()
                            {
                                recipient = reader02["NOM_CHAVE_SPLIT"].ToString(),
                                charge_processing_fee = false,
                                amount = Funcoes.strToInt((Funcoes.strToDouble(reader02["NUM_VALOR"].ToString()) * 100).ToString())
                            });
                        }
                        if (Funcoes.strToDouble(reader02["NUM_PERCENTUAL"].ToString()) > 0 &&
                            !string.IsNullOrWhiteSpace(reader02["NOM_CHAVE_SPLIT"].ToString()))
                        {

                            listaSplits.Add(new ZoopSplit()
                            {
                                recipient = reader02["NOM_CHAVE_SPLIT"].ToString(),
                                charge_processing_fee = false,
                                percentage = Funcoes.strToDouble(reader02["NUM_PERCENTUAL"].ToString())
                            });
                        }

                        sellerToken = reader02["NUM_TOKEN"].ToString();
                    }
                }
            }
        }

        return listaSplits.ToArray();
    }
}