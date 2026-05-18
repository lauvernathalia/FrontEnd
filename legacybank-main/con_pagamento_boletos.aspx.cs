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

public partial class con_pagamento_boletos : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (HttpContext.Current.Session.Count <= 0)
        {
            Response.Redirect("login.aspx");
            FormsAuthentication.SignOut();
        }


        if (!IsPostBack)
        {

            Funcoes.GravaAuditoria(0, Request.ServerVariables["REMOTE_ADDR"].ToString(), "Pagamento de Boletos", "Realizar novo Pagamento");
            
            btnContinuar.Visible = true;
            btnConfirmar.Visible = false;

            ConsultaSaldoBaas("A");

        }

    }
    protected void btnContinuar_Click(object sender, EventArgs e)
    {
        if (txtLinhaDigitavel.Text.ToString().Trim() != "")
        {

            Funcoes.GravaAuditoria(0, Request.ServerVariables["REMOTE_ADDR"].ToString(), "Pagamento Pix", "Continuar com pagamento: " + txtLinhaDigitavel.Text.ToString().Trim());


            string sToken = asaas.PegarTokenSubconta(Funcoes.strToInt(HttpContext.Current.Session["PESSOA"].ToString()), Funcoes.strToInt(HttpContext.Current.Session["LICENCIADO"].ToString()));
            dadosPagamentoAsaaS.SimularPagamento dpagamento = new dadosPagamentoAsaaS.SimularPagamento()
            {
                identificationField = txtLinhaDigitavel.Text.ToString()
            };
            string json = JsonConvert.SerializeObject(dpagamento);
            string jsonBoleto = asaas.SimularPagamento(sToken, json);
            if (jsonBoleto.ToString().Trim() != "")
            {
                JObject oBoleto = JObject.Parse(jsonBoleto.ToString());
                txtResposta.Text = jsonBoleto.ToString();

                try
                {

                    if (Convert.ToBoolean(oBoleto["bankSlipInfo"]["isOverdue"].ToString()) == true)
                    {
                        // Mensagem de Vencimento
                        ClientScript.RegisterStartupScript(this.GetType(), "BoletoVencido", "alert('O boleto encontra-se vencido e não pode ser pago! Entre em contato com a instituição emissora.');", true);
                    }
                    else
                    {
                        btnContinuar.Visible = false;
                        btnConfirmar.Visible = true;

                        divDocumento.Visible = true;
                        lblCodigoBarras.Text = oBoleto["bankSlipInfo"]["identificationField"].ToString();
                        lblInstituicaoEmissora.Text = oBoleto["bankSlipInfo"]["companyName"].ToString();
                        // Verificar o banco emissor
                        if (oBoleto["bankSlipInfo"]["bank"].ToString().Trim() != "")
                        {
                            SqlConnection mySelCadastro = new SqlConnection(Funcoes.conexao());
                            mySelCadastro.Open();
                            SqlCommand cmdSelCadastro = new SqlCommand("dbo.stp_bancos_ins", mySelCadastro);
                            cmdSelCadastro.CommandType = CommandType.StoredProcedure;
                            cmdSelCadastro.Parameters.Add("@flg_operacao", SqlDbType.Char).Value = "S";
                            cmdSelCadastro.Parameters.Add("@COD_ID", SqlDbType.Int).Value = Funcoes.strToInt(oBoleto["bankSlipInfo"]["bank"].ToString().Trim());
                            cmdSelCadastro.Parameters.Add("@COD_ID_PESSOA_LICENCIADO", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["LICENCIADO"].ToString());

                            SqlDataReader ReaderCadastro = cmdSelCadastro.ExecuteReader();
                            while (ReaderCadastro.Read())
                            {
                                lblBancoEmissor.Text = ReaderCadastro["NOM_BANCO"].ToString();
                            }
                        }
                        else
                        {
                            lblBancoEmissor.Text = "Banco emissor não especificado";
                        }


                        

                        lblValorDocumento.Text = String.Format("{0:n2}", Funcoes.strToDouble(oBoleto["bankSlipInfo"]["value"].ToString()));
                        txtValorPagar.Text = String.Format("{0:n2}", Funcoes.strToDouble(oBoleto["bankSlipInfo"]["value"].ToString()));
                        if (Convert.ToBoolean(oBoleto["bankSlipInfo"]["allowChangeValue"].ToString()) == true)
                        {
                            txtValorPagar.Enabled = true;
                        }
                        else
                        {
                            txtValorPagar.Enabled = false;
                        }

                        txtMenorValor.Text = string.Format("{0:n2}", Funcoes.strToDouble(oBoleto["bankSlipInfo"]["minValue"].ToString().Trim()));
                        txtMaiorValor.Text = string.Format("{0:n2}", Funcoes.strToDouble(oBoleto["bankSlipInfo"]["maxValue"].ToString().Trim()));
                        lblTaxaCobrada.Text = string.Format("{0:n2}", Funcoes.strToDouble(oBoleto["fee"].ToString().Trim()));

                        if ((oBoleto["bankSlipInfo"]["discountValue"].ToString().Trim() == "") && (oBoleto["bankSlipInfo"]["interestValue"].ToString().Trim() == "") && (oBoleto["bankSlipInfo"]["fineValue"].ToString().Trim() == ""))
                        {
                            divValores.Visible = false;
                        }
                        else
                        {
                            divValores.Visible = true;
                            lblDesconto.Text = string.Format("{0:n2}", Funcoes.strToDouble(oBoleto["bankSlipInfo"]["discountValue"].ToString().Trim()));
                            lblMulta.Text = string.Format("{0:n2}", Funcoes.strToDouble(oBoleto["bankSlipInfo"]["fineValue"].ToString().Trim()));
                            lblJuros.Text = string.Format("{0:n2}", Funcoes.strToDouble(oBoleto["bankSlipInfo"]["interestValue"].ToString().Trim()));
                        }
                        if (oBoleto["bankSlipInfo"]["dueDate"].ToString().Trim() != "")
                        {
                            lblVencimento.Text = Convert.ToDateTime(oBoleto["dueDate"].ToString()).ToShortDateString();
                        }
                        else
                        {
                            lblVencimento.Text = Convert.ToDateTime(oBoleto["minimumScheduleDate"].ToString()).ToShortDateString();
                        }

                        txtDataPagamento.Text = Convert.ToDateTime(oBoleto["minimumScheduleDate"].ToString()).ToShortDateString();

                        txtValorTotal.Text = string.Format("{0:n2}",
                            Funcoes.strToDouble(txtValorPagar.Text.ToString()) +
                            Funcoes.strToDouble(lblMulta.Text.ToString()) +
                            Funcoes.strToDouble(lblJuros.Text.ToString()) -
                            Funcoes.strToDouble(lblDesconto.Text.ToString()) +
                            Funcoes.strToDouble(lblTaxaCobrada.Text.ToString())
                            );

                    }
                }
                catch
                {
                    try
                    {
                        //ClientScript.RegisterStartupScript(this.GetType(), "Leitura", "alert('Ocorreu um erro ao identificar o boleto! Verifique e reentre.');", true);
                        ClientScript.RegisterStartupScript(this.GetType(), "Leitura", "alert('" + oBoleto["errors"][0]["description"].ToString() + "');", true);
                        //divResposta.Visible = true;
                        //txtResposta.Visible = true;
                    }
                    catch
                    {
                        ClientScript.RegisterStartupScript(this.GetType(), "Leitura", "alert('Ocorreu um erro ao tentar identificar o boleto! Verifique e reentre.');", true);
                        btnContinuar.Visible = true;
                        btnConfirmar.Visible = false;
                        divDocumento.Visible = false;

                    }
                }
            }
        }
        else
        {
            ClientScript.RegisterStartupScript(this.GetType(), "LinhaDigitavel", "alert('Nenhuma linha digitável ou código de barras foi informado! Verifique e reentre.');", true);

        }
    }
    protected void btnConfirmar_Click(object sender, System.EventArgs e)
    {
        if (Funcoes.strToDouble(txtValorTotal.Text.ToString()) <= Funcoes.strToDouble(lblSaldo.Text.ToString()))
        {
            if (Funcoes.strToDouble(txtValorTotal.Text.ToString()) > 0)
            {
                Funcoes.GravaAuditoria(0, Request.ServerVariables["REMOTE_ADDR"].ToString(), "Pagamento Pix", "Confirmar o pagamento: " + txtLinhaDigitavel.Text.ToString());

                string sToken = asaas.PegarTokenSubconta(Funcoes.strToInt(HttpContext.Current.Session["PESSOA"].ToString()), Funcoes.strToInt(HttpContext.Current.Session["LICENCIADO"].ToString()));

                // Criar o json de criação de pagamento
                dadosPagamentoAsaaS.CriarPagamento dpagamento = new dadosPagamentoAsaaS.CriarPagamento()
                {
                    identificationField = txtLinhaDigitavel.Text.ToString(),
                    description = txtIdentificacao.Text.ToString(),
                    externalReference = txtReferencia.Text.ToString(),
                    value = (float)Convert.ToDouble(txtValorPagar.Text.ToString()) + (float)Convert.ToDouble(lblMulta.Text.ToString()) + (float)Convert.ToDouble(lblJuros.Text.ToString()) - (float)Convert.ToDouble(lblDesconto.Text.ToString()),
                    discount = (float)Convert.ToDouble(lblDesconto.Text.ToString()),
                    interest = (float)Convert.ToDouble(lblJuros.Text.ToString()),
                    fine = (float)Convert.ToDouble(lblMulta.Text.ToString()),
                    scheduleDate = Convert.ToDateTime(txtDataPagamento.Text.ToString()),
                    dueDate = Convert.ToDateTime(txtDataPagamento.Text.ToString())
                };

                string json = JsonConvert.SerializeObject(dpagamento);
                string jsonBoleto = asaas.CriarPagamento(sToken, json);

                if (jsonBoleto.ToString().Trim() != "")
                {
                    JObject oBoleto = JObject.Parse(jsonBoleto.ToString());
                    txtResposta.Text = jsonBoleto.ToString();
                }

                // Executar pagamento



                // Capturar o status de retorno


                // Gravar os dados na base de dados
            }
            else
            {
                ClientScript.RegisterStartupScript(this.GetType(), "ValorTotalZerado", "alert('O valor total não foi especificado! Verifique e reentre');", true);
            }
        }
        else
        {
            ClientScript.RegisterStartupScript(this.GetType(), "SaldoInsufuciente", "alert('Você não possui saldo suficiente para realizar este pagamento! Verifique e reentre.');", true);
        }
    }
}



    private void ConsultaSaldoBaas(string integracao)
    {
        try
        {
            if (integracao != "A")
                return;

            // Valor padrão seguro
            lblSaldo.Text = "0,00";

            // Validação de Session
            if (HttpContext.Current.Session["PESSOA"] == null ||
                HttpContext.Current.Session["LICENCIADO"] == null)
                return;

            int pessoa = Funcoes.strToInt(HttpContext.Current.Session["PESSOA"].ToString());
            int licenciado = Funcoes.strToInt(HttpContext.Current.Session["LICENCIADO"].ToString());

            string sToken = asaas.PegarTokenSubconta(pessoa, licenciado);

            if (string.IsNullOrWhiteSpace(sToken))
                return;

            string jsonSaldo = asaas.SaldoSubconta(sToken);

            if (string.IsNullOrWhiteSpace(jsonSaldo))
                return;

            JObject oSaldo = JObject.Parse(jsonSaldo);

            // 🚨 Se a API retornou erro
            if (oSaldo["errors"] != null)
            {

                // Você pode logar o erro se quiser
                // string erroApi = oSaldo["errors"][0]?["description"]?.ToString();
                string erroApi = oSaldo.SelectToken("errors[0].description") != null ? oSaldo.SelectToken("errors[0].description").ToString() : string.Empty;

                ClientScript.RegisterStartupScript(this.GetType(), "AtualizarDadosCadastrais", "alert('Conta Digital informa que: "+erroApi+"');", true);


                lblSaldo.Text = "0,00";
                return;
            }

            // 🔐 Proteção total contra null
            JToken balanceToken = oSaldo["balance"];

            if (balanceToken == null || balanceToken.Type == JTokenType.Null)
            {
                lblSaldo.Text = "0,00";
                return;
            }

            double saldo = Funcoes.strToDouble(balanceToken.ToString());
            lblSaldo.Text = String.Format("{0:n2}", saldo);
        }
        catch (Exception ex)
        {
            // 🔥 Nunca deixa quebrar a página
            lblSaldo.Text = "0,00";

            // Opcional: log
            // Logger.Gravar(ex);
        }
    }