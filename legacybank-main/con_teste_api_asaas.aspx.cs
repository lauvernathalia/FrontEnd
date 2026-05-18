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
using System.Web.UI.WebControls.Expressions;
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

using System.Reflection;
using System.Runtime.Versioning;

public partial class con_teste_api_asaas : System.Web.UI.Page
{
    //private async void btnAsaas_Click(object sender, EventArgs e)
    //{
    //
    //    txtJsonAsaas.Text = "Validando...";
    //    btnAsaas.Enabled = false; // Desativa o botão enquanto a validação ocorre
    //    var validator = new processos();

        // Chama o método assíncrono
        //bool isValid = await validator.ProcessarTransacoesCappta("2025-01-01");

        // Atualiza o resultado na UI
        //txtJsonAsaas.Text = isValid ? "String válida!" : "String inválida!";
    //  btnAsaas.Enabled = true; // Reativa o botão
    //}

    

    protected void Page_Load(object sender, EventArgs e)
    {

    }

    protected void btnWebhooks_Click(object sender, System.EventArgs e)
    {
        txtJsonWebhooks.Text = asaas.ListarTaxas("");
        string jsonTaxas = asaas.ListarTaxas("");
        if (jsonTaxas.ToString().Trim() != "")
        {
            JObject oTaxas = JObject.Parse(jsonTaxas);
            // Pagamentos
            GravarTaxas(1, 1, Funcoes.strToDouble(oTaxas["payment"]["bankSlip"]["defaultValue"].ToString()), 0, Funcoes.strToInt(oTaxas["payment"]["bankSlip"]["daysToReceive"].ToString()), 0, 0);
            GravarTaxas(1, 2, Funcoes.strToDouble(oTaxas["payment"]["creditCard"]["operationValue"].ToString()), Funcoes.strToDouble(oTaxas["payment"]["creditCard"]["oneInstallmentPercentage"].ToString()), Funcoes.strToInt(oTaxas["payment"]["creditCard"]["daysToReceive"].ToString()), 1, 1);
            GravarTaxas(1, 2, Funcoes.strToDouble(oTaxas["payment"]["creditCard"]["operationValue"].ToString()), Funcoes.strToDouble(oTaxas["payment"]["creditCard"]["upToSixInstallmentsPercentage"].ToString()), Funcoes.strToInt(oTaxas["payment"]["creditCard"]["daysToReceive"].ToString()), 2, 6);
            GravarTaxas(1, 2, Funcoes.strToDouble(oTaxas["payment"]["creditCard"]["operationValue"].ToString()), Funcoes.strToDouble(oTaxas["payment"]["creditCard"]["upToTwelveInstallmentsPercentage"].ToString()), Funcoes.strToInt(oTaxas["payment"]["creditCard"]["daysToReceive"].ToString()), 7, 12);
            GravarTaxas(1, 3, Funcoes.strToDouble(oTaxas["payment"]["pix"]["fixedFeeValue"].ToString()), 0, 0, 0, 0);
            GravarTaxas(1, 4, Funcoes.strToDouble(oTaxas["payment"]["debitCard"]["operationValue"].ToString()), Funcoes.strToDouble(oTaxas["payment"]["debitCard"]["defaultPercentage"].ToString()), Funcoes.strToInt(oTaxas["payment"]["debitCard"]["daysToReceive"].ToString()), 0, 0);

            // Transferências
            GravarTaxas(2, 5, Funcoes.strToDouble(oTaxas["transfer"]["ted"]["feeValue"].ToString()), 0, 0, 0, 0);
            GravarTaxas(2, 3, Funcoes.strToDouble(oTaxas["transfer"]["pix"]["feeValue"].ToString()), 0, 0, 0, 0);

            // Cartão Asaas
            GravarTaxas(3, 4, Funcoes.strToDouble(oTaxas["asaasCard"]["debit"]["nationalCashWithdrawalFeeValue"].ToString()), 0, 0, 0, 0);
            GravarTaxas(3, 6, Funcoes.strToDouble(oTaxas["asaasCard"]["prepaid"]["nationalCashWithdrawalFeeValue"].ToString()), 0, 0, 0, 0);
            GravarTaxas(3, 2, Funcoes.strToDouble(oTaxas["asaasCard"]["credit"]["nationalCashWithdrawalFeeValue"].ToString()), 0, 0, 0, 0);

            // Conta Digital
            GravarTaxas(7, 13, Funcoes.strToDouble(oTaxas["childAccount"]["creationFeeValue"].ToString()), 0, 0, 0, 0);

            // Notificações
            GravarTaxas(4, 7, Funcoes.strToDouble(oTaxas["notification"]["phoneCallFeeValue"].ToString()), 0, 0, 0, 0);
            GravarTaxas(4, 9, Funcoes.strToDouble(oTaxas["notification"]["whatsAppFeeValue"].ToString()), 0, 0, 0, 0);
            GravarTaxas(4, 10, Funcoes.strToDouble(oTaxas["notification"]["messagingFeeValue"].ToString()), 0, 0, 0, 0);
            GravarTaxas(4, 11, Funcoes.strToDouble(oTaxas["notification"]["postalServiceFeeValue"].ToString()), 0, 0, 0, 0);
            GravarTaxas(4, 8, Funcoes.strToDouble(oTaxas["notification"]["smsFeeValue"].ToString()), 0, 0, 0, 0);

            // Antecipação
            GravarTaxas(6, 1, 0, Funcoes.strToDouble(oTaxas["anticipation"]["bankSlip"]["monthlyFeePercentage"].ToString()), 0, 0, 0);
            GravarTaxas(6, 2, Funcoes.strToDouble(oTaxas["anticipation"]["creditCard"]["installmentMonthlyFeeValue"].ToString()), 0, 0, 0, 0);
            GravarTaxas(6, 3, 0, Funcoes.strToDouble(oTaxas["anticipation"]["pix"]["monthlyFeePercentage"].ToString()), 0, 0, 0);
        }

        /*
        1	Pagamentos	PG
        2	Transferências	TR
        3	Cartões	CC
        4	Notificações	NT
        5	Fatura/Nota Fiscal	FT
        6	Antecipação	AN
        7	Conta Digital	CD
        8	Cobrança	CO        
         */

        /*
        1	Boleto	BOL
        2	Crédito	CRD
        3	PIX	PIX
        4	Débito	DEB
        5	TED	TED
        6	Pré-pago	PRP
        7	Chamada Telefônica	CTE
        8	SMS	SMS
        9	Whatsapp	WAT
        10	Messaging	MSG
        11	Serviço Postal	SRP
        12	Fatura	FAT
        13	Conta Digital	CTD
         */
    }

    private void GravarTaxas(int iTipoTarifa, int iTipoTarifaOperacao, double vValor, double vPercentual, int iDias, int iParcelaIni, int iParcelaFim)
    {

        SqlConnection connInsCons = new SqlConnection(Funcoes.conexao());
        connInsCons.Open();
        SqlCommand cmdInsCons = new SqlCommand("dbo.stp_tarifas_ins", connInsCons);
        cmdInsCons.CommandType = CommandType.StoredProcedure;
        cmdInsCons.Parameters.Add("@flg_operacao", SqlDbType.Char).Value = 'I';
        cmdInsCons.Parameters.Add("@COD_ID_PESSOA_LICENCIADO", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["LICENCIADO"].ToString());
        if (HttpContext.Current.Session["TIPO"].ToString().Trim() != "A")
        {
            cmdInsCons.Parameters.Add("@COD_ID_PESSOAS_FJ", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["PESSOA"].ToString());
        }
        cmdInsCons.Parameters.Add("@COD_ID_TIPO_TARIFA", SqlDbType.Int).Value = iTipoTarifa;
        cmdInsCons.Parameters.Add("@COD_ID_TIPO_TARIFA_OPERACAO", SqlDbType.Int).Value = iTipoTarifaOperacao;
        cmdInsCons.Parameters.Add("@NUM_VALOR", SqlDbType.Float).Value = vValor;
        cmdInsCons.Parameters.Add("@NUM_PERCENTUAL", SqlDbType.Float).Value = vPercentual;
        cmdInsCons.Parameters.Add("@NUM_DIAS", SqlDbType.Int).Value = iDias;
        cmdInsCons.Parameters.Add("@NUM_PARCELAS_INICIAL", SqlDbType.Int).Value = iParcelaIni;
        cmdInsCons.Parameters.Add("@NUM_PARCELAS_FINAL", SqlDbType.Int).Value = iParcelaFim;
        
        cmdInsCons.ExecuteNonQuery();
        connInsCons.Close();
        connInsCons.Dispose();

    }
    protected void btnAccounts_Click(object sender, System.EventArgs e)
    {
        string sWebhookID = "";

        //pegar o WEBHOOK ID DO LICENCIADO
        SqlConnection mySelCadastro = new SqlConnection(Funcoes.conexao());
        mySelCadastro.Open();
        SqlCommand cmdSelCadastro = new SqlCommand("dbo.stp_pessoas_fj_integracoes_chaves_ins", mySelCadastro);
        cmdSelCadastro.CommandType = CommandType.StoredProcedure;
        cmdSelCadastro.Parameters.Add("@flg_operacao", SqlDbType.Char).Value = "L";
        cmdSelCadastro.Parameters.Add("@COD_ID_PESSOA_LICENCIADO", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["LICENCIADO"].ToString());
        cmdSelCadastro.Parameters.Add("@COD_ID_INTEGRACOES", SqlDbType.Int).Value = 5;
        SqlDataReader ReaderCadastro = cmdSelCadastro.ExecuteReader();
        while (ReaderCadastro.Read())
        {
            sWebhookID = ReaderCadastro["NOM_WEBHOOK_ID"].ToString();
        }

        DadosWebhookCappra.Root dwebhook = new DadosWebhookCappra.Root()
        {
            URL = "https://" + Request.ServerVariables["SERVER_NAME"].ToString() + "/events/cappta?id=" + sWebhookID.ToString(),
            Type = "transaction"
        };

        string json = JsonConvert.SerializeObject(dwebhook);
        txtJsonAccounts.Text = txtJsonAccounts.Text + hubcappta.CadastrarWebhook(json);


        txtJsonAccounts.Text = txtJsonAccounts.Text + hubcappta.ListarOpcoesCadastro();
        txtJsonAccounts.Text = txtJsonAccounts.Text + hubcappta.ListarOpcoesPlanos();
    }

    protected void btnAdiq_Click(object sender, System.EventArgs e)
    {

        agilli.HttpResponseResult resultado = agilli.ConsultaResumidaEstabelecimento("?cpfCnpj=30752973000148");

        if (resultado.StatusCode == 200)
        {
            // Sucesso - Processar JSON normalmente
            string jsonResponse = resultado.Content;
            //txtJsonAsaas.Text = txtJsonAsaas.Text + jsonResponse;

            JArray array = JArray.Parse(jsonResponse);

            txtJsonAdiq.Text = array[0]["contrato"].ToString();

        }
        else
        {
            string mensagemErro = resultado.Content;
            txtJsonAsaas.Text = txtJsonAsaas.Text + mensagemErro;

        };
        

        //txtJsonAsaas.Text = txtJsonAsaas.Text + agilli.TokenAutenticacao();
        //txtJsonAsaas.Text = txtJsonAsaas.Text + HttpContext.Current.Session["tokenagilli"];
        //txtJsonAsaas.Text = txtJsonAsaas.Text + HttpContext.Current.Session["tempoexpiracao"];

    }

    public string AntifraudeGUID()
    {
        return "";
    }
    protected void btnCobranca_Click(object sender, System.EventArgs e)
    {

    }
    protected void btnAsaas_Click(object sender, System.EventArgs e)
    {

        txtJsonAsaas.Text = txtJsonAsaas.Text + asaas.ListarSubcontas();

        //string resultado = zoop.compradores("P", "01001905709", "");
        //txtJsonAsaas.Text = txtJsonAsaas.Text + resultado;

        //dadosTransferencia.Transferencia dtransferencia = new dadosTransferencia.Transferencia()
        //{
        //    amount = Funcoes.strToInt(Convert.ToString(Funcoes.strToDouble("500") * 100).ToString()),
        //    statement_descriptor = "ADRIANO LEVY BARBOSA",
        //    description = "SAQUE SALDO CONTA DIGITAL",
        //};
        //string json = JsonConvert.SerializeObject(dtransferencia);

        //zoop.HttpResponseResult resultado = zoop.transferencia("I", "56a83c57021d47519cf173976abbdb31", json);

        //txtJsonAsaas.Text = txtJsonAsaas.Text + resultado.Content;

//            zoop.HttpResponseResult resultado = zoop.ConsultarSaldo(HttpContext.Current.Session["TOKENZOOP"].ToString());
//            txtJsonAsaas.Text = txtJsonAsaas.Text + resultado.Content; 
        
        //zoop.HttpResponseResult resultado = zoop.ConsultarTransacaoZoopIDmTLSNew("fe3698b86ec64b1ba22469b57a3867e2");

        //zoop.HttpResponseResult resultado = zoop.ConsultarCompradorCNPJCPF("52832990000128");
        //if (resultado.StatusCode == 200)
        //{
            // Sucesso - Processar JSON normalmente
            //string jsonResponse = resultado.Content;


            //var byteArray = Encoding.ASCII.GetBytes(zoop.Keymarketplace() + ":");
            //string encodedAuth = Convert.ToBase64String(byteArray);
            //txtJsonAsaas.Text = txtJsonAsaas.Text + " --- " + encodedAuth;
        //}
        //else
        //{
        //    string mensagemErro = resultado.Content;
        //    txtJsonAsaas.Text = txtJsonAsaas.Text + mensagemErro;

        //};


        //txtJsonAsaas.Text = txtJsonAsaas.Text + asaas.ListarChavePix("$aact_YTU5YTE0M2M2N2I4MTliNzk0YTI5N2U5MzdjNWZmNDQ6OjAwMDAwMDAwMDAwMDA0ODUxMjQ6OiRhYWNoXzk0Mzk0NDYzLTcyMzItNGQ1Yi05MWZhLTAxMmI5Yzk1NGIzMg==");
        //txtJsonAsaas.Text = txtJsonAsaas.Text + asaas.ListarSubcontaID("$aact_YTU5YTE0M2M2N2I4MTliNzk0YTI5N2U5MzdjNWZmNDQ6OjAwMDAwMDAwMDAwMDA0NTM1OTg6OiRhYWNoX2U4MTIzZDJmLTY3Y2YtNDkyOC1hZjE4LWY2ODBkZGYzZGM0MA==", "679011c6-8831-48ef-b1a1-db6565e66fd0");
        /*
        string json = "";

        ClientScript.RegisterStartupScript(this.GetType(), "Executando" , "alert('Executando...');", true);
        SqlConnection mySelCadastro = new SqlConnection(Funcoes.conexao());
        mySelCadastro.Open();
        SqlCommand cmdSelCadastro = new SqlCommand("dbo.stp_pessoas_fj_baas_ins", mySelCadastro);
        cmdSelCadastro.CommandType = CommandType.StoredProcedure;
        cmdSelCadastro.Parameters.Add("@flg_operacao", SqlDbType.Char).Value = "C";
        cmdSelCadastro.Parameters.Add("@FLG_ORIGEM", SqlDbType.Char).Value = "A";
        SqlDataReader ReaderCadastro = cmdSelCadastro.ExecuteReader();
        while (ReaderCadastro.Read())
        {
            ClientScript.RegisterStartupScript(this.GetType(), "Processando" + ReaderCadastro["COD_ID"].ToString(), "alert('Processando conta: " + ReaderCadastro["NUM_TOKEN_BAAS"].ToString() + "');", true);
            json = "";
            json = asaas.RecuperarWalletID(ReaderCadastro["NUM_TOKEN_BAAS"].ToString());
            txtJsonAsaas.Text = json.ToString();
            if (json.ToString().Trim() != "")
            {
                JObject oSubConta = JObject.Parse(json.ToString());
                try
                {
                    SqlConnection connInsCons = new SqlConnection(Funcoes.conexao());
                    connInsCons.Open();
                    SqlCommand cmdInsCons = new SqlCommand("dbo.stp_pessoas_fj_baas_ins", connInsCons);
                    cmdInsCons.CommandType = CommandType.StoredProcedure;
                    cmdInsCons.Parameters.Add("@flg_operacao", SqlDbType.Char).Value = 'W';
                    cmdInsCons.Parameters.Add("@COD_ID_PESSOA_LICENCIADO", SqlDbType.Int).Value = Funcoes.strToInt(ReaderCadastro["COD_ID_PESSOA_LICENCIADO"].ToString());
                    cmdInsCons.Parameters.Add("@COD_ID_PESSOAS_FJ", SqlDbType.Int).Value = Funcoes.strToInt(ReaderCadastro["COD_ID_PESSOAS_FJ"].ToString());
                    cmdInsCons.Parameters.Add("@FLG_ORIGEM", SqlDbType.Char).Value = "A";
                    cmdInsCons.Parameters.Add("@NUM_WALLETID_BAAS", SqlDbType.VarChar).Value = oSubConta["data"][0]["id"].ToString();

                    cmdInsCons.ExecuteNonQuery();
                    connInsCons.Close();
                    connInsCons.Dispose();
                }
                catch
                {
                    ClientScript.RegisterStartupScript(this.GetType(), "Alerta" + ReaderCadastro["COD_ID"].ToString(), "alert('Não foi possível localizar conta: " + ReaderCadastro["NUM_TOKEN_BAAS"].ToString() + "');", true);
                }

            }

        }
        mySelCadastro.Close();
        mySelCadastro.Dispose();
        */

//        txtJsonAsaas.Text = asaas.TestedeCode("$aact_YTU5YTE0M2M2N2I4MTliNzk0YTI5N2U5MzdjNWZmNDQ6OjAwMDAwMDAwMDAwMDA1MDc0OTM6OiRhYWNoXzU5YTcyYTZjLTc3YmUtNDlmZS05M2U2LTY2MDQxNTNlZmViOQ==");
//        JObject oTeste = JObject.Parse(txtJsonAsaas.Text.ToString());

//        txtJsonAsaas.Text = txtJsonAsaas.Text + oTeste["StatusCode"].ToString();
//        txtJsonAsaas.Text = txtJsonAsaas.Text + oTeste["json"]["id"].ToString();

        //txtJsonAsaas.Text = adiq.GerarTokenAdiq();
        //txtJsonAsaas.Text = txtJsonAsaas.Text + adiq.TokenAutenticacaoAdiq();
        //txtJsonAsaas.Text = granito.TokenAutenticacao();
        //txtJsonAsaas.Text = asaas.ListarStatus("$aact_YTU5YTE0M2M2N2I4MTliNzk0YTI5N2U5MzdjNWZmNDQ6OjAwMDAwMDAwMDAwMDA0OTU5MTA6OiRhYWNoX2ExMzFkNWIzLWM5YjEtNDA4My1iMzA3LTU0YWEyODViN2I4Mw==");
        //txtJsonAsaas.Text = txtJsonAsaas.Text + HttpContext.Current.Session["PESSOA"].ToString() + " - ";
        //txtJsonAsaas.Text = txtJsonAsaas.Text + HttpContext.Current.Session["LICENCIADO"].ToString() + " - ";

        //txtJsonAsaas.Text = txtJsonAsaas.Text + asaas.DeletarConta("$aact_YTU5YTE0M2M2N2I4MTliNzk0YTI5N2U5MzdjNWZmNDQ6OjAwMDAwMDAwMDAwMDA1MDkzNTI6OiRhYWNoXzNmMzkwNWU4LTkxYTktNDY5OS1hMTE5LTI2NThiZWYyMzEzYQ==", "Usado para teste plataforma");
        
        //txtJsonAsaas.Text = txtJsonAsaas.Text = granito.ProcessarTransacao();
        //txtJsonAsaas.Text = asaas.RecuperarConta("$aact_MzkwODA2MWY2OGM3MWRlMDU2NWM3MzJlNzZmNGZhZGY6OjQxN2VjZGIyLTMxZjYtNDdjZC1hODM3LTQ5YWYyN2UyOTQyMTo6JGFhY2hfYTI2YjhjMTgtZmQyMi00ZWQzLTkxYmItZTYzMTcyMGI5Njlk");
        //txtJsonAsaas.Text = txtJsonAsaas.Text + " ---" + asaas.RecuperarWalletID("$aact_MzkwODA2MWY2OGM3MWRlMDU2NWM3MzJlNzZmNGZhZGY6OjQxN2VjZGIyLTMxZjYtNDdjZC1hODM3LTQ5YWYyN2UyOTQyMTo6JGFhY2hfYTI2YjhjMTgtZmQyMi00ZWQzLTkxYmItZTYzMTcyMGI5Njlk");

        //txtJsonAsaas.Text = asaas.ListarStatus("$aact_MzkwODA2MWY2OGM3MWRlMDU2NWM3MzJlNzZmNGZhZGY6OjQxN2VjZGIyLTMxZjYtNDdjZC1hODM3LTQ5YWYyN2UyOTQyMTo6JGFhY2hfYTI2YjhjMTgtZmQyMi00ZWQzLTkxYmItZTYzMTcyMGI5Njlk");
        //txtJsonAsaas.Text = asaas.ListarStatus("$aact_YTU5YTE0M2M2N2I4MTliNzk0YTI5N2U5MzdjNWZmNDQ6OjAwMDAwMDAwMDAwMDA1MDc0OTM6OiRhYWNoXzU5YTcyYTZjLTc3YmUtNDlmZS05M2U2LTY2MDQxNTNlZmViOQ==");
        //txtJsonAsaas.Text = zoop.transacao("P", "f41832cea42c40f69cf96603e5ac3bc5", "");
        //txtJsonAsaas.Text = zoop.ConsultarPlanoVendedor("d725fa1d1c4847d5aa46a4f8e2850045");
        //txtJsonAsaas.Text = zoop.planos_referencia("plano_std_lbk_fison2_d30");

        //zoop.HttpResponseResult resultado = zoop.ConsultarCompradorCNPJCPF("01001905709");

        //bigdatacorp.DadosConsultas.Root dconsulta = new bigdatacorp.DadosConsultas.Root()
        //{
        //    q = "doc{00475092000166}",
        //    Datasets = "merchant_category_data"
        //};

        //var transaction = new
        //{
        //    Description = "Venda realizada de forma indevida e com duplicidade no sistema"
        //};

        //string json = JsonConvert.SerializeObject(transaction);

        //string sToken = asaas.PegarTokenSubconta(Funcoes.strToInt(HttpContext.Current.Session["PESSOA"].ToString()), Funcoes.strToInt(HttpContext.Current.Session["LICENCIADO"].ToString()));

        //txtJsonAsaas.Text = txtJsonAsaas.Text + agilli.TokenAutenticacao();
        //txtJsonAsaas.Text = txtJsonAsaas.Text + HttpContext.Current.Session["tokenagilli"];
        //txtJsonAsaas.Text = txtJsonAsaas.Text + HttpContext.Current.Session["tempoexpiracao"];

        //agilli.HttpResponseResult resultado = agilli.AutenticacaoAgilli();
        //string jsonResponse = resultado.Content;
        //txtJsonAsaas.Text = txtJsonAsaas.Text + jsonResponse;


        /*
        agilli.DadosCredenciamentoLojistaAgilli.Root dcredenciamento = new agilli.DadosCredenciamentoLojistaAgilli.Root()
        {
            cnpj = "00475092000166",
            cnpjCanalWL = "",
            cnpjOrigem = "",
            identificadorCliente = "0000000001",
            urlCallback = "",
            razaoSocial = "GOODINFO SISTEMAS DE INFORMATICA LRDA",
            nomeFantasia = "GOODINFO INFORMATICA",
            cnae = "4679-6/01",
            ramoAtividade = "DESENVOLVIMENTO DE SISTEMAS",
            faturamentoPrevisto = 20000,
            email = "adriano@webview.com.br",
            dddComercial = "11",
            telefoneComercial = "970308508",
            cep = "01535001",
            logradouro = "Rua Paulo Orozimbo",
            numeroEndereco = 1203,
            complemento = "Casa",
            bairro = "Cambuci",
            municipio = "São Paulo",
            uf = "SP",
            dddCel = "11",
            telefoneCelular = "970308508",
            responsavelAssinatura = "Adriano Levy Barbosa",
            quantidadePos = 1,
            faturamentoContratado = 20000,
            antecipacaoAutomatica = "S",
            taxaAntecipacao = 0,
            tipoAntecipacao = "ROTATIVO",
            mcc = "5814",
            tipoContrato = "W",
            codConfiguracao = "",
            cnpjParceiro = "35675737000199",
            idCesta = 123,
            codBanco = "341",
            agencia = "0036",
            digAgencia = "0",
            numConta = "64944",
            digConta = "0",
            protocoloCore = " ",
            hashAceite = "X",
            terminais = new agilli.DadosCredenciamentoLojistaAgilli.Terminais[]
            {
                // Exemplo de terminal vazio (se não houver, pode deixar o array vazio mesmo)
            },

            documentosSocios = new agilli.DadosCredenciamentoLojistaAgilli.DocumentosSocio[]
            {
                new agilli.DadosCredenciamentoLojistaAgilli.DocumentosSocio
                {
                    identificacao = "01001905709",
                    anexos = new agilli.DadosCredenciamentoLojistaAgilli.Anexo[]
                    {
                        new agilli.DadosCredenciamentoLojistaAgilli.Anexo
                        {
                            nomeArquivo = "semAnexo.jpg",
                            conteudo = "EM BRANCO",
                            tipo = "BRANCO"
                        }
                    }
                }
            },

            anexos = new agilli.DadosCredenciamentoLojistaAgilli.Anexo[]
            {
                new agilli.DadosCredenciamentoLojistaAgilli.Anexo
                {
                    nomeArquivo = "semAnexo.jpg",
                    conteudo = "EM BRANCO",
                    tipo = "BRANCO"
                }
            }
        };

        string json = JsonConvert.SerializeObject(dcredenciamento);
        txtJsonAdiq.Text = json;
        
        agilli.HttpResponseResult resultado = agilli.CredenciamentoLogista(json);

        if (resultado.StatusCode == 200)
        {
            // Sucesso - Processar JSON normalmente
            string jsonResponse = resultado.Content;
            txtJsonAsaas.Text = txtJsonAsaas.Text + jsonResponse;
        }
        else
        {
            string mensagemErro = resultado.Content;
            txtJsonAsaas.Text = txtJsonAsaas.Text + mensagemErro;

        };
        /*

        //txtJsonAsaas.Text = txtJsonAsaas.Text + hubcappta.TokenCappta();
        //txtJsonAsaas.Text = txtJsonAsaas.Text = txtJsonAsaas.Text + hubcappta.ConsultarPlanos(1, "true", "Reseller");
        //txtJsonAsaas.Text = txtJsonAsaas.Text = txtJsonAsaas.Text + hubcappta.ConsultarPlanos(1, "true", "Merchant");
        
        //var redefine = new
        //{
        //    id = "b6cf98c3-d23b-4d81-ae94-5b04211a77b4",
        //    email = "wander@euromercantil.com.br",
        //    password = "Wss251103#"        
        //};
        //string json = JsonConvert.SerializeObject(redefine);

        //txtJsonWebhooks.Text = txtJsonWebhooks.Text + asaas.RedefinirSubconta(json);

        //txtJsonAsaas.Text = txtJsonAsaas.Text + asaas.ListarDocumentosPendentes("$aact_prod_000MzkwODA2MWY2OGM3MWRlMDU2NWM3MzJlNzZmNGZhZGY6OmY4NDE1Yzc5LWFmYzYtNGE2Yy04ZDkyLTJjOGFjZWZhYjk0Yzo6JGFhY2hfNDViNTA0ZWYtMjU2Ni00ZGY5LTliMGQtYWEyNmQzZjA3NzFl");
        

        //txtJsonAsaas.Text = txtJsonAsaas.Text + asaas.ListarChavePix(sToken);


        //else
        //{
            // Falha - Exibir erro conforme necessário

        //    string mensagemErro = resultado.Content;
        //    string translatedJson = zoop.TranslateApiError(mensagemErro);

        //    txtJsonAsaas.Text = translatedJson;
        //}


        //txtJsonAsaas.Text = txtJsonAsaas.Text + asaas.SaldoSubconta("$aact_prod_000MzkwODA2MWY2OGM3MWRlMDU2NWM3MzJlNzZmNGZhZGY6OmY4NDE1Yzc5LWFmYzYtNGE2Yy04ZDkyLTJjOGFjZWZhYjk0Yzo6JGFhY2hfNDViNTA0ZWYtMjU2Ni00ZGY5LTliMGQtYWEyNmQzZjA3NzFl");
        //txtJsonAsaas.Text = txtJsonAsaas.Text + asaas.ExtratoSubconta("$aact_prod_000MzkwODA2MWY2OGM3MWRlMDU2NWM3MzJlNzZmNGZhZGY6OmY4NDE1Yzc5LWFmYzYtNGE2Yy04ZDkyLTJjOGFjZWZhYjk0Yzo6JGFhY2hfNDViNTA0ZWYtMjU2Ni00ZGY5LTliMGQtYWEyNmQzZjA3NzFl", "?offset=0&limit=100&startDate=2025-05-10&finishDate=2025-05-23&order=desc");


        /*
        string seller;
        split.ZoopSplit[] zoopSplits = split.GerarSplitPresencialZoop(1, 9, "BOLETO", "Z", "C", "V", "P", 1, out seller);
        if (zoopSplits != null && zoopSplits.Length > 0)
        {
            string jsonTransacao = JsonConvert.SerializeObject(zoopSplits);

            txtJsonAsaas.Text = jsonTransacao;
        }
        else
        {
            txtJsonAsaas.Text = "Nenhum split encontrado.";
        }
         */
    }


}