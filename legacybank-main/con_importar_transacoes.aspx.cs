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

public partial class con_importar_transacoes : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }
    protected void btnProcessar_Click(object sender, EventArgs e)
    {
        // Carrega Pesquisa Transações

        string sPagina = "pageId="+txtPagina.Text.ToString();
        string sInicio = "createdSince="+txtInicio.Text.ToString();
        string sFim = "createdUntil=" + txtFim.Text.ToString();

        txtResposta.Text = hubcappta.ConsultarTransacoes(sPagina+"&"+sInicio+"&"+sFim);

        string jsonTransacoesPagina = txtResposta.Text.ToString();
        JObject oTransacoesPagina = JObject.Parse(jsonTransacoesPagina);

        int iPaginas = Funcoes.strToInt(oTransacoesPagina["lastPage"].ToString());

        for (int x = 990; x < 2000; x++)//iPaginas; x++)
        {
            //ClientScript.RegisterStartupScript(this.GetType(),
            //"PAGINA" + x.ToString(), "alert('P: " + sPagina.ToString() + "');", true);

            
            sPagina = "pageId=" + x.ToString();
            
            //txtResposta.Text = hubcappta.ConsultarTransacoes(sPagina + "&" + sInicio + "&" + sFim);

            string jsonTransacoes = hubcappta.ConsultarTransacoes(sPagina + "&" + sInicio + "&" + sFim);
            JObject oTransacoes = JObject.Parse(jsonTransacoes);

            if (oTransacoes["payments"].Count() > 0)
            {
                for (int i = 0; i < oTransacoes["payments"].Count(); i++)
                {
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
    protected void btnBusca_Click(object sender, System.EventArgs e)
    {
        txtResposta.Text = hubcappta.ConsultarLojista(txtLoja.Text.ToString(), txtRevenda.Text.ToString());

        string sCodigoEstabelecimento = "0";

        JObject oEstabelecimento = JObject.Parse(txtResposta.Text.ToString());

            //txtJsonCappta.Text = txtJsonCappta.Text + oEstabelecimento["resellerDocument"].ToString();
            // ********************************************************************
            // Localizar cadastro do Representante
            // ********************************************************************
            string sIDMarketplace = "0";
            SqlConnection mySelCadastro = new SqlConnection(Funcoes.conexao());
            mySelCadastro.Open();
            SqlCommand cmdSelCadastro = new SqlCommand("dbo.stp_pessoas_fj_ins", mySelCadastro);
            cmdSelCadastro.CommandType = CommandType.StoredProcedure;
            cmdSelCadastro.Parameters.Add("@flg_operacao", SqlDbType.Char).Value = "H";
            cmdSelCadastro.Parameters.Add("@NOM_CNPJ", SqlDbType.VarChar).Value = txtRevenda.Text.ToString();
            cmdSelCadastro.Parameters.Add("@COD_ID_PESSOA_LICENCIADO", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["LICENCIADO"].ToString());
            cmdSelCadastro.Parameters.Add("@FLG_TIPO", SqlDbType.Char).Value = "M";

            SqlDataReader ReaderCadastro = cmdSelCadastro.ExecuteReader();
            while (ReaderCadastro.Read())
            {
                sIDMarketplace = ReaderCadastro["COD_ID"].ToString();
            }

            SqlConnection connInsCons = new SqlConnection(Funcoes.conexao());
            connInsCons.Open();
            SqlCommand cmdInsCons = new SqlCommand("dbo.stp_pessoas_fj_ins", connInsCons);
            cmdInsCons.CommandType = CommandType.StoredProcedure;
            cmdInsCons.Parameters.Add("@flg_operacao", SqlDbType.Char).Value = 'I';
            cmdInsCons.Parameters.Add("@FLG_ATIVO", SqlDbType.Char).Value = "S";
            cmdInsCons.Parameters.Add("@FLG_INSERT_USUARIO", SqlDbType.Char).Value = "S";
            cmdInsCons.Parameters.Add("@COD_ID_PESSOA_LICENCIADO", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["LICENCIADO"].ToString());
            cmdInsCons.Parameters.Add("@DTA_DATA", SqlDbType.DateTime).Value = DateTime.Now;
            cmdInsCons.Parameters.Add("@FLG_TIPO", SqlDbType.Char).Value = "E";


            cmdInsCons.Parameters.Add("@FLG_TIPO_PESSOA", SqlDbType.VarChar).Value = (oEstabelecimento["merchant"]["document"].ToString().Trim().Length >= 14) ? "PJ" : "PF";
            cmdInsCons.Parameters.Add("@FLG_PRESENCIAL", SqlDbType.Char).Value = "N";
            //            cmdInsCons.Parameters.Add("@COD_ID_MARKETPLACE", SqlDbType.Int).Value = Funcoes.strToInt(ddlMarketplace.SelectedValue.ToString());

            cmdInsCons.Parameters.Add("@COD_ID_MARKETPLACE", SqlDbType.Int).Value = Funcoes.strToInt(sIDMarketplace.ToString());


            // Empresa
            cmdInsCons.Parameters.Add("@NOM_RAZAOSOCIAL", SqlDbType.VarChar).Value = (oEstabelecimento["merchant"]["document"].ToString().Trim().Length >= 14) ? oEstabelecimento["merchant"]["companyName"].ToString().Trim() : oEstabelecimento["owner"]["name"].ToString().Trim();
            cmdInsCons.Parameters.Add("@NOM_FANTASIA", SqlDbType.VarChar).Value = (oEstabelecimento["merchant"]["document"].ToString().Trim().Length >= 14) ? oEstabelecimento["merchant"]["tradingName"].ToString().Trim() : oEstabelecimento["owner"]["name"].ToString().Trim();
            cmdInsCons.Parameters.Add("@NOM_CNPJ", SqlDbType.VarChar).Value = oEstabelecimento["merchant"]["document"].ToString().Trim();

            cmdInsCons.Parameters.Add("@NUM_TELEFONE", SqlDbType.VarChar).Value = oEstabelecimento["owner"]["phone"].ToString().Trim();
            cmdInsCons.Parameters.Add("@NOM_EMAIL_EMPRESA", SqlDbType.VarChar).Value = oEstabelecimento["owner"]["email"].ToString().Trim();
            cmdInsCons.Parameters.Add("@COD_ID_MCC", SqlDbType.Int).Value = Funcoes.strToInt(oEstabelecimento["merchant"]["mccId"].ToString().Trim());
            //cmdInsCons.Parameters.Add("@NOM_TIPO_EMPRESA", SqlDbType.VarChar).Value = ddlTipoEmpresa.SelectedValue.ToString();
            cmdInsCons.Parameters.Add("@NUM_FATURAMENTO", SqlDbType.Float).Value = 0;
            cmdInsCons.Parameters.Add("@NUM_PATRIMONIO", SqlDbType.Float).Value = 0;

            //if (txtDataAbertura.Text.ToString().Trim() != "")
            //{
            //    cmdInsCons.Parameters.Add("@DTA_ABERTURA", SqlDbType.DateTime).Value = Convert.ToDateTime(txtDataAbertura.Text.ToString());
            //}

            // Endereço
            cmdInsCons.Parameters.Add("@NOM_ENDERECO", SqlDbType.VarChar).Value = oEstabelecimento["address"]["streetName"].ToString().Trim();
            cmdInsCons.Parameters.Add("@NOM_NUMERO", SqlDbType.VarChar).Value = oEstabelecimento["address"]["houseNumber"].ToString().Trim();
            cmdInsCons.Parameters.Add("@NOM_COMPLEMENTO", SqlDbType.VarChar).Value = oEstabelecimento["address"]["complement"].ToString().Trim();
            cmdInsCons.Parameters.Add("@NOM_BAIRRO", SqlDbType.VarChar).Value = oEstabelecimento["address"]["neighborhood"].ToString().Trim();
            cmdInsCons.Parameters.Add("@NOM_CIDADE", SqlDbType.VarChar).Value = oEstabelecimento["address"]["city"].ToString().Trim();
            cmdInsCons.Parameters.Add("@NOM_UF", SqlDbType.VarChar).Value = oEstabelecimento["address"]["state"].ToString().Trim();
            cmdInsCons.Parameters.Add("@NOM_CEP", SqlDbType.VarChar).Value = oEstabelecimento["address"]["postalCode"].ToString().Trim();
            cmdInsCons.Parameters.Add("@NOM_PAIS", SqlDbType.VarChar).Value = "BR";

            // Responsável
            cmdInsCons.Parameters.Add("@NOM_NOME", SqlDbType.VarChar).Value = oEstabelecimento["owner"]["name"].ToString().Trim();
            cmdInsCons.Parameters.Add("@NOM_SOBRENOME", SqlDbType.VarChar).Value = oEstabelecimento["owner"]["phone"].ToString().Trim();
            cmdInsCons.Parameters.Add("@NOM_CPF", SqlDbType.VarChar).Value = oEstabelecimento["owner"]["cpf"].ToString().Trim();

            //if (txtNascimento.Text.ToString().Trim() != "")
            //{
            //    cmdInsCons.Parameters.Add("@DTA_ANIVERSARIO", SqlDbType.DateTime).Value = Convert.ToDateTime(txtNascimento.Text.ToString());
            //}

            //cmdInsCons.Parameters.Add("@NOM_MAE", SqlDbType.VarChar).Value = txtNomeMae.Text.ToString();
            //cmdInsCons.Parameters.Add("@NUM_RENDA_MENSAL", SqlDbType.Float).Value = Funcoes.strToDouble(txtRendaMensalPF.Text.ToString());
            cmdInsCons.Parameters.Add("@NOM_EMAIL", SqlDbType.VarChar).Value = oEstabelecimento["owner"]["email"].ToString().Trim();
            cmdInsCons.Parameters.Add("@NOM_CELULAR", SqlDbType.VarChar).Value = oEstabelecimento["owner"]["mobilePhone"].ToString().Trim();
            //cmdInsCons.Parameters.Add("@FLG_POLITICAMENTE", SqlDbType.Char).Value = ddlPoliticamenteExposta.SelectedValue.ToString();

            // Usuário
            cmdInsCons.Parameters.Add("@NOM_NOME_USUARIO", SqlDbType.VarChar).Value = oEstabelecimento["owner"]["name"].ToString().Trim();
            cmdInsCons.Parameters.Add("@NOM_LOGIN", SqlDbType.VarChar).Value = oEstabelecimento["owner"]["email"].ToString().Trim();
            cmdInsCons.Parameters.Add("@NOM_SENHA", SqlDbType.VarChar).Value = oEstabelecimento["merchant"]["document"].ToString().Trim();

            sCodigoEstabelecimento = cmdInsCons.ExecuteScalar().ToString();

            //cmdInsCons.ExecuteNonQuery();
            connInsCons.Close();
            connInsCons.Dispose();
            
            // Dados Onboarding Cappta
            SqlConnection connInsConsDadosCappta = new SqlConnection(Funcoes.conexao());
            connInsConsDadosCappta.Open();
            SqlCommand cmdInsConsDadosCappta = new SqlCommand("dbo.stp_pessoas_fj_cappta_ins", connInsConsDadosCappta);
            cmdInsConsDadosCappta.CommandType = CommandType.StoredProcedure;
            cmdInsConsDadosCappta.Parameters.Add("@flg_operacao", SqlDbType.Char).Value = 'I';
            cmdInsConsDadosCappta.Parameters.Add("@COD_ID_PESSOA_LICENCIADO", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["LICENCIADO"].ToString());
            cmdInsConsDadosCappta.Parameters.Add("@COD_ID_PESSOAS_FJ", SqlDbType.Int).Value = Funcoes.strToInt(sCodigoEstabelecimento.ToString());

            cmdInsConsDadosCappta.Parameters.Add("@NUM_TOKEN_CAPPTA", SqlDbType.VarChar).Value = oEstabelecimento["merchant"]["document"].ToString();
            cmdInsConsDadosCappta.Parameters.Add("@FLG_STATUS_CAPPTA", SqlDbType.VarChar).Value = oEstabelecimento["statusDescription"].ToString();
            cmdInsConsDadosCappta.Parameters.Add("@FLG_CAPPTA", SqlDbType.Char).Value = "S";
            cmdInsConsDadosCappta.Parameters.Add("@DES_JSON_CAPPTA", SqlDbType.VarChar).Value = txtResposta.Text.ToString();
            cmdInsConsDadosCappta.Parameters.Add("@COD_ID_NATUREZA_CAPPTA", SqlDbType.Int).Value = (oEstabelecimento["merchant"]["legalNatureId"].ToString().Trim() != "") ? Funcoes.strToInt(oEstabelecimento["merchant"]["legalNatureId"].ToString()) : 0;
            
        //cmdInsConsDadosCappta.Parameters.Add("@COD_ID_NATUREZA_CAPPTA", SqlDbType.Int).Value = oEstabelecimento["merchant"]["legalNatureId"].ToString();

            cmdInsConsDadosCappta.ExecuteNonQuery();
            connInsConsDadosCappta.Close();
            connInsConsDadosCappta.Dispose();
            
            // Dados Bancários Cappta
            SqlConnection connInsConsBancoCappta = new SqlConnection(Funcoes.conexao());
            connInsConsBancoCappta.Open();
            SqlCommand cmdInsConsBancoCappta = new SqlCommand("dbo.stp_pessoas_fj_contas_ins", connInsConsBancoCappta);
            cmdInsConsBancoCappta.CommandType = CommandType.StoredProcedure;
            cmdInsConsBancoCappta.Parameters.Add("@flg_operacao", SqlDbType.Char).Value = 'I';
            cmdInsConsBancoCappta.Parameters.Add("@COD_ID_PESSOA_LICENCIADO", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["LICENCIADO"].ToString());
            cmdInsConsBancoCappta.Parameters.Add("@COD_ID_PESSOAS_FJ", SqlDbType.Int).Value = Funcoes.strToInt(sCodigoEstabelecimento.ToString());

            cmdInsConsBancoCappta.Parameters.Add("@NOM_CODIGO_BANCO", SqlDbType.VarChar).Value = oEstabelecimento["bankAccount"]["bankCode"].ToString();
            cmdInsConsBancoCappta.Parameters.Add("@NOM_TIPO_BANCO", SqlDbType.VarChar).Value = (oEstabelecimento["bankAccount"]["accountType"].ToString().Trim() == "1") ? "C" : "P";
            cmdInsConsBancoCappta.Parameters.Add("@NOM_NUMERO_AGENCIA_BANCO", SqlDbType.VarChar).Value = oEstabelecimento["bankAccount"]["branch"].ToString();
            cmdInsConsBancoCappta.Parameters.Add("@NOM_NUMERO_DIGITO_AGENCIA_BANCO", SqlDbType.VarChar).Value = "";
            cmdInsConsBancoCappta.Parameters.Add("@NOM_NUMERO_CONTA_BANCO", SqlDbType.VarChar).Value = oEstabelecimento["bankAccount"]["account"].ToString();
            cmdInsConsBancoCappta.Parameters.Add("@NOM_NUMERO_DIGITO_CONTA_BANCO", SqlDbType.VarChar).Value = "";
            cmdInsConsBancoCappta.Parameters.Add("@FLG_PADRAO", SqlDbType.Char).Value = "S";

            cmdInsConsBancoCappta.ExecuteNonQuery();
            connInsConsBancoCappta.Close();
            connInsConsBancoCappta.Dispose();
            

        txtResposta.Text = txtResposta.Text + " - " + hubcappta.VerificaEstabelecimento(txtRevenda.Text.ToString(), txtLoja.Text.ToString());
    }
}