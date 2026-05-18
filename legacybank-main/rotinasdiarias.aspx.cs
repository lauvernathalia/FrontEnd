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

public partial class rotinasdiarias : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            ImportarTransacoesCappta();
            // Criar rotina para registrar a data e hora de execução
            // Criar rotina de backup da base de dados
        }
        FecharProcessamento();
    }

    private void ImportarTransacoesCappta()
    {
        txtResposta.Text = "";
        HttpContext.Current.Session.Add("LICENCIADO", "0");

        SqlConnection mySelCadastro = new SqlConnection(Funcoes.conexao());
        mySelCadastro.Open();
        SqlCommand cmdSelCadastro = new SqlCommand("dbo.stp_pessoas_fj_integracoes_chaves_ins", mySelCadastro);
        cmdSelCadastro.CommandType = CommandType.StoredProcedure;
        cmdSelCadastro.Parameters.Add("@flg_operacao", SqlDbType.Char).Value = "X";
        cmdSelCadastro.Parameters.Add("@COD_ID_INTEGRACOES", SqlDbType.Int).Value = 5;
        SqlDataReader ReaderCadastro = cmdSelCadastro.ExecuteReader();
        //int iContador = 0;

        while (ReaderCadastro.Read())
        {
            HttpContext.Current.Session["LICENCIADO"] = ReaderCadastro["COD_ID_PESSOA_LICENCIADO"].ToString();

            string sPagina = "pageId=1";
            string sInicio = "createdSince=" + DateTime.Now.AddDays(0).Year.ToString()+"-"+DateTime.Now.AddDays(0).Month.ToString() + "-" + DateTime.Now.AddDays(0).Day.ToString() + " 00:00:00";
            string sFim = "createdUntil=" + DateTime.Now.AddDays(0).Year.ToString() + "-" + DateTime.Now.AddDays(0).Month.ToString() + "-" + DateTime.Now.AddDays(0).Day.ToString() + " 23:59:59";

            string jsonTransacoesPagina = hubcappta.ConsultarTransacoes(sPagina + "&" + sInicio + "&" + sFim);
            JObject oTransacoesPagina = JObject.Parse(jsonTransacoesPagina);

            //txtResposta.Text =txtResposta.Text +  "Licenciado: " + HttpContext.Current.Session["LICENCIADO"].ToString();
            //txtResposta.Text = txtResposta.Text + "   ---   " + jsonTransacoesPagina.ToString();

            int iPaginas = Funcoes.strToInt(oTransacoesPagina["lastPage"].ToString());

            //ClientScript.RegisterStartupScript(this.GetType(),
            //"PAGINAS" + iPaginas.ToString(), "alert('Páginas: " + iPaginas.ToString() + "');", true);


            for (int x = 0; x < iPaginas; x++)
            {
                //txtResposta.Text = txtResposta.Text + "   ---   Página: " + iPaginas.ToString();

                //ClientScript.RegisterStartupScript(this.GetType(),
                //"PAGINAATUAL" + x.ToString(), "alert('Página Atual: " + x.ToString() + "');", true);
                
                sPagina = "pageId=" + x.ToString();
                string jsonTransacoes = hubcappta.ConsultarTransacoes(sPagina + "&" + sInicio + "&" + sFim);
                JObject oTransacoes = JObject.Parse(jsonTransacoes);

                if (oTransacoes["payments"].Count() > 0)
                {
                    for (int i = 0; i < oTransacoes["payments"].Count(); i++)
                    {
                        //txtResposta.Text = txtResposta.Text + "   ---   Transações: " + iPaginas.ToString();

                        // Inserir as transações na base de dados
                        
                        // 1o. Verifica se existe a revenda e lojista
                        string sMarketplace = hubcappta.VerificaMarketplace(oTransacoes["payments"][i]["resellerDocument"].ToString());

                        string sEstabelecimento = hubcappta.VerificaEstabelecimento(oTransacoes["payments"][i]["resellerDocument"].ToString(), oTransacoes["payments"][i]["merchantDocument"].ToString());


                        //ClientScript.RegisterStartupScript(this.GetType(),
                        //"REVENDA" + i.ToString(), "alert('R: " + oTransacoes["payments"][i]["resellerDocument"].ToString() + " - " + sRepresentante.ToString() + "');", true);
                        //ClientScript.RegisterStartupScript(this.GetType(),
                        //"LOJA" + i.ToString(), " 'L: " + oTransacoes["payments"][i]["merchantDocument"].ToString() + " - " + sEstabelecimento.ToString() + "');", true);


                        // Transacoes
                        SqlConnection connInsCons = new SqlConnection(Funcoes.conexao());
                        connInsCons.Open();
                        SqlCommand cmdInsCons = new SqlCommand("dbo.stp_transacoes_ins", connInsCons);
                        cmdInsCons.CommandType = CommandType.StoredProcedure;
                        cmdInsCons.Parameters.Add("@flg_operacao", SqlDbType.Char).Value = 'I';

                        cmdInsCons.Parameters.Add("@COD_ID_PESSOA_LICENCIADO", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["LICENCIADO"].ToString());
                        cmdInsCons.Parameters.Add("@COD_ID_PESSOAS_FJ", SqlDbType.Int).Value = Funcoes.strToInt(sEstabelecimento.ToString());
                        cmdInsCons.Parameters.Add("@COD_ID_MARKETPLACE", SqlDbType.Int).Value = Funcoes.strToInt(sMarketplace.ToString());
                        cmdInsCons.Parameters.Add("@COD_ID_PLANO", SqlDbType.Int).Value = Funcoes.strToInt(oTransacoes["payments"][i]["fees"]["planId"].ToString());
                        cmdInsCons.Parameters.Add("@FLG_ORIGEM", SqlDbType.Char).Value = "C";

                        cmdInsCons.Parameters.Add("@DTA_DATA", SqlDbType.DateTime).Value = Convert.ToDateTime(oTransacoes["payments"][i]["createdAt"].ToString());
                        cmdInsCons.Parameters.Add("@NOM_CODE", SqlDbType.VarChar).Value = oTransacoes["payments"][i]["id"].ToString();
                        cmdInsCons.Parameters.Add("@NUM_TOKEN", SqlDbType.VarChar).Value = oTransacoes["payments"][i]["merchantDocument"].ToString();

                        cmdInsCons.Parameters.Add("@COD_ID_TIPO", SqlDbType.Int).Value = 1;
                        cmdInsCons.Parameters.Add("@NOM_STATUS", SqlDbType.VarChar).Value = oTransacoes["payments"][i]["status"].ToString();

                        // Parte II
                        cmdInsCons.Parameters.Add("@FLG_PRESENCIAL_ONLINE", SqlDbType.Char).Value = "P";
                        cmdInsCons.Parameters.Add("@NOM_TIPO_PAGAMENTO", SqlDbType.VarChar).Value = oTransacoes["payments"][i]["paymentMethod"].ToString();

                        cmdInsCons.Parameters.Add("@NOM_CODIGO_PAGAMENTO", SqlDbType.VarChar).Value = oTransacoes["payments"][i]["card"]["cardBrand"].ToString();
                        cmdInsCons.Parameters.Add("@NUM_PARCELAS", SqlDbType.Int).Value = Funcoes.strToInt(oTransacoes["payments"][i]["installmentMethodCode"].ToString());
                        cmdInsCons.Parameters.Add("@NOM_CARTAO_4_PRIMEIROS_DIGIROS", SqlDbType.VarChar).Value = oTransacoes["payments"][i]["card"]["cardFirstSixDigits"].ToString();
                        cmdInsCons.Parameters.Add("@NOM_CARTAO_4_ULTIMOS_DIGITOS", SqlDbType.VarChar).Value = oTransacoes["payments"][i]["card"]["cardLastFourDigits"].ToString();



                        cmdInsCons.Parameters.Add("@NUM_VALOR_BRUTO", SqlDbType.Float).Value = Funcoes.strToDouble(oTransacoes["payments"][i]["amountInCents"].ToString()) / 100;
                        cmdInsCons.Parameters.Add("@NUM_VALOR_LIQUIDO", SqlDbType.Float).Value = Funcoes.strToDouble(oTransacoes["payments"][i]["netAmountInCents"].ToString()) / 100;
                        cmdInsCons.Parameters.Add("@NUM_VALOR_INTERMEDIACAO_FEE", SqlDbType.Float).Value = Funcoes.strToDouble(oTransacoes["payments"][i]["feeAmountInCents"].ToString()) / 100;
                        cmdInsCons.Parameters.Add("@NUM_VALOR_INTERMEDIACAO_RATE", SqlDbType.Float).Value = Funcoes.strToDouble(oTransacoes["payments"][i]["fees"]["merchantRate"].ToString()) / 100;
                        cmdInsCons.Parameters.Add("@NUM_VALOR_DESCONTO", SqlDbType.Float).Value = 0;
                        cmdInsCons.Parameters.Add("@NUM_VALOR_EXTRAS", SqlDbType.Float).Value = 0;

                        cmdInsCons.Parameters.Add("@NUM_ITENS", SqlDbType.Int).Value = 1;

                        cmdInsCons.Parameters.Add("@NOM_FONTE_CANCELAMENTO", SqlDbType.VarChar).Value = "";
                        cmdInsCons.Parameters.Add("@NOM_ORIGEM", SqlDbType.VarChar).Value = "cappta";


                        cmdInsCons.Parameters.Add("@NOM_CLIENTE", SqlDbType.VarChar).Value = oTransacoes["payments"][i]["card"]["cardHolderName"].ToString();
                        cmdInsCons.Parameters.Add("@NOM_NUMERO_TRANSACAO", SqlDbType.VarChar).Value = oTransacoes["payments"][i]["id"].ToString();
                        cmdInsCons.Parameters.Add("@NOM_RECIBO", SqlDbType.VarChar).Value = "";

                        cmdInsCons.Parameters.Add("@NOM_AUTORIZACAO", SqlDbType.VarChar).Value = oTransacoes["payments"][i]["authorizationNumber"].ToString();
                        cmdInsCons.Parameters.Add("@DES_DESCRICAO", SqlDbType.Text).Value = oTransacoes["payments"][i]["responseMessage"].ToString();

                        // Insere dados do Historico

                        SqlConnection connInsConsHistorico = new SqlConnection(Funcoes.conexao());
                        connInsConsHistorico.Open();
                        SqlCommand cmdInsConsHistorico = new SqlCommand("dbo.stp_transacoes_historico_ins", connInsConsHistorico);
                        cmdInsConsHistorico.CommandType = CommandType.StoredProcedure;
                        cmdInsConsHistorico.Parameters.Add("@flg_operacao", SqlDbType.Char).Value = 'I';
                        cmdInsConsHistorico.Parameters.Add("@COD_ID_PESSOA_LICENCIADO", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["LICENCIADO"].ToString());

                        cmdInsConsHistorico.Parameters.Add("@DTA_DATA", SqlDbType.DateTime).Value = DateTime.Now;
                        cmdInsConsHistorico.Parameters.Add("@DTA_DATA_CRIACAO", SqlDbType.DateTime).Value = Convert.ToDateTime(oTransacoes["payments"][i]["createdAt"].ToString());
                        cmdInsConsHistorico.Parameters.Add("@NOM_CODE", SqlDbType.VarChar).Value = oTransacoes["payments"][i]["id"].ToString();

                        cmdInsConsHistorico.Parameters.Add("@NOM_STATUS", SqlDbType.VarChar).Value = oTransacoes["payments"][i]["status"].ToString();
                        cmdInsConsHistorico.Parameters.Add("@NOM_TIPO", SqlDbType.VarChar).Value = oTransacoes["payments"][i]["paymentMethod"].ToString();
                        cmdInsConsHistorico.Parameters.Add("@NUM_VALOR", SqlDbType.Float).Value = Funcoes.strToDouble(oTransacoes["payments"][i]["amountInCents"].ToString()) / 100;

                        cmdInsConsHistorico.Parameters.Add("@NOM_ID_AUTORIZACAO", SqlDbType.VarChar).Value = oTransacoes["payments"][i]["authorizationNumber"].ToString();
                        cmdInsConsHistorico.Parameters.Add("@NOM_DESCRICAO_RESPOSTA", SqlDbType.Text).Value = oTransacoes["payments"][i]["responseMessage"].ToString();
                        cmdInsConsHistorico.Parameters.Add("@NOM_CODIGO_RESPOSTA", SqlDbType.Text).Value = oTransacoes["payments"][i]["nsuAcquirer"].ToString();
                        cmdInsConsHistorico.Parameters.Add("@NOM_NSU_AUTORIZACAO", SqlDbType.VarChar).Value = oTransacoes["payments"][i]["nsuProvider"].ToString();
                        cmdInsConsHistorico.ExecuteNonQuery();
                        connInsConsHistorico.Close();
                        connInsConsHistorico.Dispose();


                        cmdInsCons.Parameters.Add("@NOM_CODIGO_TERMINAL", SqlDbType.VarChar).Value = oTransacoes["payments"][i]["terminalIdentifiers"]["serialNumber"].ToString();
                        cmdInsCons.Parameters.Add("@NOM_STATUS_TERMINAL", SqlDbType.VarChar).Value = "";
                        cmdInsCons.Parameters.Add("@NOM_NUMERO_SERIAL_TERMINAL", SqlDbType.VarChar).Value = oTransacoes["payments"][i]["terminalIdentifiers"]["serialNumber"].ToString();
                        cmdInsCons.Parameters.Add("@NOM_MODELO_TERMINAL", SqlDbType.VarChar).Value = oTransacoes["payments"][i]["terminalIdentifiers"]["model"].ToString();
                        cmdInsCons.Parameters.Add("@NOM_FABRICANTE_TERMINAL", SqlDbType.VarChar).Value = oTransacoes["payments"][i]["terminalIdentifiers"]["model"].ToString();

                        // EXECUTA A GRAVAÇÃO
                        cmdInsCons.ExecuteNonQuery();
                        connInsCons.Close();
                        connInsCons.Dispose();
                        

                    }
                }
            }
        }
    }

    private void FecharProcessamento()
    {
        ClientScript.RegisterStartupScript(this.GetType(),
    "ProcessamentoFinalizacao", "window.opener=self; window.close(); self.close();", true);
    }
}