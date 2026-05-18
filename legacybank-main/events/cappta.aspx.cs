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


public partial class events_cappta : System.Web.UI.Page
{
    public string sid_id
    {
        get
        {
            try { return Request["id"].ToString(); }
            catch { return ""; }
        }
    }

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

        string sConexao = ConfigurationManager.AppSettings["user"].ToString() + ";" +
            ConfigurationManager.AppSettings["password"].ToString() + ";" +
            ConfigurationManager.AppSettings["datasource"].ToString() + ";" +
            ConfigurationManager.AppSettings["basecatalog"].ToString();

        SqlConnection connInsConsCAPPTA_TRANS = new SqlConnection(sConexao);
        connInsConsCAPPTA_TRANS.Open();
        SqlCommand cmdInsConsCAPPTA_TRANS = new SqlCommand("dbo.stp_notificacoes_cappta_ins", connInsConsCAPPTA_TRANS);
        cmdInsConsCAPPTA_TRANS.CommandType = CommandType.StoredProcedure;
        cmdInsConsCAPPTA_TRANS.Parameters.Add("@flg_operacao", SqlDbType.Char).Value = 'I';
        cmdInsConsCAPPTA_TRANS.Parameters.Add("@DES_JSON", SqlDbType.Text).Value = bodyText.ToString();
        cmdInsConsCAPPTA_TRANS.Parameters.Add("@DTA_DATA", SqlDbType.DateTime).Value = DateTime.Now;
        cmdInsConsCAPPTA_TRANS.Parameters.Add("@NOM_CODE", SqlDbType.VarChar).Value = sid_id.ToString();
        cmdInsConsCAPPTA_TRANS.Parameters.Add("@NUM_TOKEN", SqlDbType.VarChar).Value = "0000000000000000000";
        cmdInsConsCAPPTA_TRANS.Parameters.Add("@NOM_TIPO", SqlDbType.VarChar).Value = "INDEFINIDO";
        cmdInsConsCAPPTA_TRANS.Parameters.Add("@FLG_STATUS", SqlDbType.VarChar).Value = "INDEFINIDO";

        string iCodigoNotificacao = cmdInsConsCAPPTA_TRANS.ExecuteScalar().ToString();
        //cmdInsConsCAPPTA_TRANS.ExecuteNonQuery();
        connInsConsCAPPTA_TRANS.Close();
        connInsConsCAPPTA_TRANS.Dispose();

        int iLicenciado = 0;

        SqlConnection mySelCadastroEstabelecimento = new SqlConnection(sConexao);
        mySelCadastroEstabelecimento.Open();
        SqlCommand cmdSelCadastroEstabelecimento = new SqlCommand("dbo.stp_pessoas_fj_integracoes_chaves_ins", mySelCadastroEstabelecimento);
        cmdSelCadastroEstabelecimento.CommandType = CommandType.StoredProcedure;
        cmdSelCadastroEstabelecimento.Parameters.Add("@flg_operacao", SqlDbType.Char).Value = "W";
        cmdSelCadastroEstabelecimento.Parameters.Add("@NOM_WEBHOOK_ID", SqlDbType.VarChar).Value = sid_id.ToString();
        SqlDataReader ReaderCadastroEstabelecimento = cmdSelCadastroEstabelecimento.ExecuteReader();
        while (ReaderCadastroEstabelecimento.Read())
        {
            HttpContext.Current.Session.Add("LICENCIADO", ReaderCadastroEstabelecimento["COD_ID_PESSOA_LICENCIADO"].ToString());
            iLicenciado = Funcoes.strToInt(ReaderCadastroEstabelecimento["COD_ID_PESSOA_LICENCIADO"].ToString());
        }

        if (bodyText.ToString().Trim() != "")
        {
            //try
            //{
            string jsonTransacao = bodyText.ToString();

            JObject oTransacao = JObject.Parse(jsonTransacao);

            string sMarketplace = hubcappta.VerificaMarketplace(oTransacao["resellerDocument"].ToString());
            string sEstabelecimento = hubcappta.VerificaEstabelecimento(oTransacao["resellerDocument"].ToString(), oTransacao["merchantDocument"].ToString());

            // Transacoes
            SqlConnection connInsCons = new SqlConnection(Funcoes.conexao());
            connInsCons.Open();
            SqlCommand cmdInsCons = new SqlCommand("dbo.stp_transacoes_ins", connInsCons);
            cmdInsCons.CommandType = CommandType.StoredProcedure;
            cmdInsCons.Parameters.Add("@flg_operacao", SqlDbType.Char).Value = 'I';

            cmdInsCons.Parameters.Add("@COD_ID_PESSOA_LICENCIADO", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["LICENCIADO"].ToString());
            cmdInsCons.Parameters.Add("@COD_ID_PESSOAS_FJ", SqlDbType.Int).Value = Funcoes.strToInt(sEstabelecimento.ToString());
            cmdInsCons.Parameters.Add("@COD_ID_MARKETPLACE", SqlDbType.Int).Value = Funcoes.strToInt(sMarketplace.ToString());
            //cmdInsCons.Parameters.Add("@COD_ID_PLANO", SqlDbType.Int).Value = Funcoes.strToInt(oTransacao["fees"]["planId"].ToString());
            cmdInsCons.Parameters.Add("@FLG_ORIGEM", SqlDbType.Char).Value = "C";

            cmdInsCons.Parameters.Add("@DTA_DATA", SqlDbType.DateTime).Value = Convert.ToDateTime(oTransacao["createdAt"].ToString());
            cmdInsCons.Parameters.Add("@NOM_CODE", SqlDbType.VarChar).Value = oTransacao["id"].ToString();
            cmdInsCons.Parameters.Add("@NUM_TOKEN", SqlDbType.VarChar).Value = oTransacao["merchantDocument"].ToString();

            cmdInsCons.Parameters.Add("@COD_ID_TIPO", SqlDbType.Int).Value = 1;
            cmdInsCons.Parameters.Add("@NOM_STATUS", SqlDbType.VarChar).Value = oTransacao["status"].ToString();

            // Parte II
            cmdInsCons.Parameters.Add("@FLG_PRESENCIAL_ONLINE", SqlDbType.Char).Value = "P";
            cmdInsCons.Parameters.Add("@NOM_TIPO_PAGAMENTO", SqlDbType.VarChar).Value = oTransacao["paymentMethod"].ToString();

            cmdInsCons.Parameters.Add("@NOM_CODIGO_PAGAMENTO", SqlDbType.VarChar).Value = oTransacao["card"]["cardBrand"].ToString();
            cmdInsCons.Parameters.Add("@NUM_PARCELAS", SqlDbType.Int).Value = Funcoes.strToInt(oTransacao["installments"].ToString());
            cmdInsCons.Parameters.Add("@NUM_DEVICE_BIN", SqlDbType.Char).Value = oTransacao["card"]["cardFirstSixDigits"].ToString();

            if (oTransacao["card"]["cardFirstSixDigits"].ToString().Trim() != "")
            {
                cmdInsCons.Parameters.Add("@NOM_TIPO_OPERACAO_BANDEIRA", SqlDbType.VarChar).Value = ObterBandeiraCartao(oTransacao["card"]["cardFirstSixDigits"].ToString());
            }
            else
            {
                cmdInsCons.Parameters.Add("@NOM_TIPO_OPERACAO_BANDEIRA", SqlDbType.VarChar).Value = oTransacao["card"]["cardBrand"].ToString(); 
            }

            cmdInsCons.Parameters.Add("@NOM_TIPO_OPERACAO", SqlDbType.VarChar).Value = RetornaTipoPagamento(oTransacao["paymentMethod"].ToString());


            cmdInsCons.Parameters.Add("@NOM_CARTAO_4_PRIMEIROS_DIGIROS", SqlDbType.VarChar).Value = oTransacao["card"]["cardFirstSixDigits"].ToString();
            cmdInsCons.Parameters.Add("@NOM_CARTAO_4_ULTIMOS_DIGITOS", SqlDbType.VarChar).Value = oTransacao["card"]["cardLastFourDigits"].ToString();

            cmdInsCons.Parameters.Add("@NUM_VALOR_BRUTO", SqlDbType.Float).Value = Funcoes.strToDouble(oTransacao["amountInCents"].ToString()) / 100;
            cmdInsCons.Parameters.Add("@NUM_VALOR_LIQUIDO", SqlDbType.Float).Value = Funcoes.strToDouble(oTransacao["netAmountInCents"].ToString()) / 100;
            cmdInsCons.Parameters.Add("@NUM_VALOR_INTERMEDIACAO_FEE", SqlDbType.Float).Value = Funcoes.strToDouble(oTransacao["feeAmountInCents"].ToString()) / 100;
            cmdInsCons.Parameters.Add("@NUM_VALOR_INTERMEDIACAO_RATE", SqlDbType.Float).Value = Funcoes.strToDouble(oTransacao["fees"]["merchantRate"].ToString()) / 100;
            cmdInsCons.Parameters.Add("@NUM_VALOR_DESCONTO", SqlDbType.Float).Value = 0;
            cmdInsCons.Parameters.Add("@NUM_VALOR_EXTRAS", SqlDbType.Float).Value = 0;

            cmdInsCons.Parameters.Add("@NUM_ITENS", SqlDbType.Int).Value = 1;

            cmdInsCons.Parameters.Add("@NOM_FONTE_CANCELAMENTO", SqlDbType.VarChar).Value = "";
            cmdInsCons.Parameters.Add("@NOM_ORIGEM", SqlDbType.VarChar).Value = "cappta";


            cmdInsCons.Parameters.Add("@NOM_CLIENTE", SqlDbType.VarChar).Value = oTransacao["card"]["cardHolderName"].ToString();
            cmdInsCons.Parameters.Add("@NOM_NUMERO_TRANSACAO", SqlDbType.VarChar).Value = oTransacao["id"].ToString();
            cmdInsCons.Parameters.Add("@NOM_RECIBO", SqlDbType.VarChar).Value = "";

            cmdInsCons.Parameters.Add("@NOM_AUTORIZACAO", SqlDbType.VarChar).Value = oTransacao["authorizationNumber"].ToString();
            cmdInsCons.Parameters.Add("@DES_DESCRICAO", SqlDbType.Text).Value = oTransacao["responseMessage"].ToString();

            // Insere dados do Historico

            SqlConnection connInsConsHistorico = new SqlConnection(Funcoes.conexao());
            connInsConsHistorico.Open();
            SqlCommand cmdInsConsHistorico = new SqlCommand("dbo.stp_transacoes_historico_ins", connInsConsHistorico);
            cmdInsConsHistorico.CommandType = CommandType.StoredProcedure;
            cmdInsConsHistorico.Parameters.Add("@flg_operacao", SqlDbType.Char).Value = 'I';
            cmdInsConsHistorico.Parameters.Add("@COD_ID_PESSOA_LICENCIADO", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["LICENCIADO"].ToString());

            cmdInsConsHistorico.Parameters.Add("@DTA_DATA", SqlDbType.DateTime).Value = DateTime.Now;
            cmdInsConsHistorico.Parameters.Add("@DTA_DATA_CRIACAO", SqlDbType.DateTime).Value = Convert.ToDateTime(oTransacao["createdAt"].ToString());
            cmdInsConsHistorico.Parameters.Add("@NOM_CODE", SqlDbType.VarChar).Value = oTransacao["id"].ToString();

            cmdInsConsHistorico.Parameters.Add("@NOM_STATUS", SqlDbType.VarChar).Value = oTransacao["status"].ToString();
            cmdInsConsHistorico.Parameters.Add("@NOM_TIPO", SqlDbType.VarChar).Value = oTransacao["paymentMethod"].ToString();
            cmdInsConsHistorico.Parameters.Add("@NUM_VALOR", SqlDbType.Float).Value = Funcoes.strToDouble(oTransacao["amountInCents"].ToString()) / 100;

            cmdInsConsHistorico.Parameters.Add("@NOM_ID_AUTORIZACAO", SqlDbType.VarChar).Value = oTransacao["authorizationNumber"].ToString();
            cmdInsConsHistorico.Parameters.Add("@NOM_DESCRICAO_RESPOSTA", SqlDbType.Text).Value = oTransacao["responseMessage"].ToString();
            cmdInsConsHistorico.Parameters.Add("@NOM_CODIGO_RESPOSTA", SqlDbType.Text).Value = oTransacao["nsuAcquirer"].ToString();
            cmdInsConsHistorico.Parameters.Add("@NOM_NSU_AUTORIZACAO", SqlDbType.VarChar).Value = oTransacao["nsuProvider"].ToString();
            cmdInsConsHistorico.ExecuteNonQuery();
            connInsConsHistorico.Close();
            connInsConsHistorico.Dispose();


            cmdInsCons.Parameters.Add("@NOM_CODIGO_TERMINAL", SqlDbType.VarChar).Value = oTransacao["terminalIdentifiers"]["serialNumber"].ToString();
            cmdInsCons.Parameters.Add("@NOM_STATUS_TERMINAL", SqlDbType.VarChar).Value = "";
            cmdInsCons.Parameters.Add("@NOM_NUMERO_SERIAL_TERMINAL", SqlDbType.VarChar).Value = oTransacao["terminalIdentifiers"]["serialNumber"].ToString();
            cmdInsCons.Parameters.Add("@NUM_DEVICE_SERIAL", SqlDbType.VarChar).Value = oTransacao["terminalIdentifiers"]["serialNumber"].ToString();
            cmdInsCons.Parameters.Add("@NOM_MODELO_TERMINAL", SqlDbType.VarChar).Value = oTransacao["terminalIdentifiers"]["model"].ToString();
            cmdInsCons.Parameters.Add("@NOM_FABRICANTE_TERMINAL", SqlDbType.VarChar).Value = oTransacao["terminalIdentifiers"]["model"].ToString();

            // Plano
            try
            {
                SqlConnection mySelCadastroPlano = new SqlConnection(Funcoes.conexao());
                mySelCadastroPlano.Open();
                SqlCommand cmdSelCadastroPlano = new SqlCommand("dbo.stp_planos_ins", mySelCadastroPlano);
                cmdSelCadastroPlano.CommandType = CommandType.StoredProcedure;
                cmdSelCadastroPlano.Parameters.Add("@flg_operacao", SqlDbType.Char).Value = "Z";
                cmdSelCadastroPlano.Parameters.Add("@COD_ID_PESSOA_LICENCIADO", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["LICENCIADO"].ToString());
                cmdSelCadastroPlano.Parameters.Add("@NOM_TITULO_PLANO", SqlDbType.VarChar).Value = oTransacao["fees"]["planName"].ToString();
                cmdSelCadastroPlano.Parameters.Add("@FLG_MODELO_PLANO", SqlDbType.VarChar).Value = "C";

                SqlDataReader ReaderCadastroPlano = cmdSelCadastroPlano.ExecuteReader();
                while (ReaderCadastroPlano.Read())
                {
                    cmdInsCons.Parameters.Add("@COD_ID_PLANO", SqlDbType.Int).Value = Funcoes.strToInt(ReaderCadastroPlano["COD_ID"].ToString());
                }
            }
            catch
            {
                cmdInsCons.Parameters.Add("@COD_ID_PLANO", SqlDbType.Int).Value = 0;
            }

            // EXECUTA A GRAVAÇÃO
            cmdInsCons.ExecuteNonQuery();
            connInsCons.Close();
            connInsCons.Dispose();


            SqlConnection connInsConsU = new SqlConnection(Funcoes.conexao());
            connInsConsU.Open();
            SqlCommand cmdInsConsU = new SqlCommand("dbo.stp_notificacoes_cappta_ins", connInsConsU);
            cmdInsConsU.CommandType = CommandType.StoredProcedure;
            cmdInsConsU.Parameters.Add("@flg_operacao", SqlDbType.Char).Value = "U";
            cmdInsConsU.Parameters.Add("@COD_ID", SqlDbType.Int).Value = iCodigoNotificacao.ToString();

            cmdInsConsU.ExecuteNonQuery();
            connInsConsU.Close();
            connInsConsU.Dispose();
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


    public static string RetornaTipoPagamento(string sTipoPagamento)
    {
        string sRetorno = "";
        switch (sTipoPagamento)
        {
            case "Debit":
                sRetorno = "Débito";
                break;
            case "Credit":
                sRetorno = "Crédito";
                break;
            case "Pix":
                sRetorno = "Pix";
                break;
            case "Boleto":
                sRetorno = "Boleto";
                break;
            default:
                sRetorno = "Não Identificado";
                break;
        }
        return sRetorno.ToString();
    }
}