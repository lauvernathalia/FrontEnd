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


public partial class con_pagamento_pix : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (HttpContext.Current.Session.Count <= 0)
        {
            Response.Redirect("login.aspx");
            FormsAuthentication.SignOut();
        }

        Funcoes.CONFIRMA(btnConfirmar, "Confirma o pagamento do PIX informado?");

        if (!IsPostBack)
        {

            Funcoes.GravaAuditoria(0, Request.ServerVariables["REMOTE_ADDR"].ToString(), "Pagamento de PIX", "Realizar novo Pagamento");

            btnContinuar.Visible = true;
            btnConfirmar.Visible = false;

            ConsultaSaldoBaas("A");
        }

    }
    protected void btnContinuar_Click(object sender, EventArgs e)
    {
        return;
        if (txtPIX.Text.ToString().Trim() != "")
        {
            Funcoes.GravaAuditoria(0, Request.ServerVariables["REMOTE_ADDR"].ToString(), "Pagamento de PIX", "Continuar com pagamento: " + txtPIX.Text.ToString().Trim());
            string sToken = asaas.PegarTokenSubconta(Funcoes.strToInt(HttpContext.Current.Session["PESSOA"].ToString()), Funcoes.strToInt(HttpContext.Current.Session["LICENCIADO"].ToString()));
            dadosPagamentoPixAsaaS.DecodificarQRCode dpagamentoPIX = new dadosPagamentoPixAsaaS.DecodificarQRCode()
            {
                payload = txtPIX.Text.ToString(),
                changeValue = 0
            };
            string json = JsonConvert.SerializeObject(dpagamentoPIX);
            string jsonPix = asaas.DecodificarQRCode(sToken, json);
            txtResposta.Text = sToken + " - " + jsonPix.ToString();
            txtResposta.Visible = true;

            if (jsonPix.ToString().Trim() != "")
            {

                try
                {
                    JObject oPix = JObject.Parse(jsonPix.ToString());

                    if (oPix["expirationDate"].ToString().Trim() != "")
                    {
                        if (Convert.ToDateTime(oPix["expirationDate"].ToString()) < DateTime.Now)
                        {
                            ClientScript.RegisterStartupScript(this.GetType(), "PixExpirado", "alert('O PIX encontra-se com a data de expiração vencida e não pode ser pago! Entre em contato com a instituição emissora.');", true);
                            return;
                        }
                    }

                    btnContinuar.Visible = false;
                    btnConfirmar.Visible = true;

                    divDocumento.Visible = true;
                    lblChavePix.Text = oPix["pixKey"].ToString();
                    lblInstituicaoEmissora.Text = oPix["receiver"]["name"].ToString();
                    // Verificar o banco emissor
                    if (oPix["receiver"]["ispbName"].ToString().Trim() != "")
                    {
                        lblBancoEmissor.Text = oPix["receiver"]["ispbName"].ToString();
                    }
                    else
                    {
                        lblBancoEmissor.Text = "Banco emissor não especificado";
                    }




                    lblValorDocumento.Text = String.Format("{0:n2}", Funcoes.strToDouble(oPix["totalValue"].ToString()));
                    txtValorPagar.Text = String.Format("{0:n2}", Funcoes.strToDouble(oPix["totalValue"].ToString()));
                    if (Convert.ToBoolean(oPix["canBePaidWithDifferentValue"].ToString()) == true)
                    {
                        txtValorPagar.Enabled = true;
                    }
                    else
                    {
                        txtValorPagar.Enabled = false;
                    }

                    /*
                    txtMenorValor.Text = string.Format("{0:n2}", Funcoes.strToDouble(oPix["bankSlipInfo"]["minValue"].ToString().Trim()));
                    txtMaiorValor.Text = string.Format("{0:n2}", Funcoes.strToDouble(oPix["bankSlipInfo"]["maxValue"].ToString().Trim()));
                    lblTaxaCobrada.Text = string.Format("{0:n2}", Funcoes.strToDouble(oPix["fee"].ToString().Trim()));

                    if ((oPix["bankSlipInfo"]["discountValue"].ToString().Trim() == "") && (oPix["bankSlipInfo"]["interestValue"].ToString().Trim() == "") && (oPix["bankSlipInfo"]["fineValue"].ToString().Trim() == ""))
                    {
                        divValores.Visible = false;
                    }
                    else
                    {
                        divValores.Visible = true;
                        lblDesconto.Text = string.Format("{0:n2}", Funcoes.strToDouble(oPix["bankSlipInfo"]["discountValue"].ToString().Trim()));
                        lblMulta.Text = string.Format("{0:n2}", Funcoes.strToDouble(oPix["bankSlipInfo"]["fineValue"].ToString().Trim()));
                        lblJuros.Text = string.Format("{0:n2}", Funcoes.strToDouble(oPix["bankSlipInfo"]["interestValue"].ToString().Trim()));
                    }
                    */


                    if (oPix["dueDate"].ToString().Trim() != "")
                    {
                        lblVencimento.Text = Convert.ToDateTime(oPix["dueDate"].ToString()).ToShortDateString();

                        if (Convert.ToDateTime(oPix["dueDate"].ToString()) < DateTime.Now)
                        {
                            txtDataPagamento.Text = DateTime.Now.ToShortDateString();
                        }
                        else
                        {
                            txtDataPagamento.Text = Convert.ToDateTime(oPix["dueDate"].ToString()).ToShortDateString();
                        }

                    }
                    else
                    {
                        lblVencimento.Text = Convert.ToDateTime(oPix["expirationDate"].ToString()).ToShortDateString();
                        txtDataPagamento.Text = Convert.ToDateTime(oPix["expirationDate"].ToString()).ToShortDateString();
                    }

                    txtValorTotal.Text = string.Format("{0:n2}",
                        Funcoes.strToDouble(txtValorPagar.Text.ToString()) +
                        Funcoes.strToDouble(lblMulta.Text.ToString()) +
                        Funcoes.strToDouble(lblJuros.Text.ToString()) -
                        Funcoes.strToDouble(lblDesconto.Text.ToString()) +
                        Funcoes.strToDouble(lblTaxaCobrada.Text.ToString())
                        );
                }
                catch
                {
                    try
                    {
                        JObject oPix = JObject.Parse(jsonPix.ToString());
                        //ClientScript.RegisterStartupScript(this.GetType(), "Leitura", "alert('Ocorreu um erro ao identificar o boleto! Verifique e reentre.');", true);
                        ClientScript.RegisterStartupScript(this.GetType(), "LeituraErro", "alert('Não Autorizado: " + oPix["errors"][0]["description"] + " Verifique e tente novamente.');", true);
                        //divResposta.Visible = true;
                        //txtResposta.Visible = true;
                    }
                    catch
                    {
                        ClientScript.RegisterStartupScript(this.GetType(), "Erro", "alert('Ocorreu um erro ao tentar identificar o PIX! Verifique e reentre.');", true);
                        btnContinuar.Visible = true;
                        btnConfirmar.Visible = false;
                        divDocumento.Visible = false;

                    }
                }
            }
            
        }
        else
        {
            ClientScript.RegisterStartupScript(this.GetType(), "PixQRCode", "alert('Nenhum QRCode foi informado! Verifique e tente novamente.');", true);

        }
    }
    protected void btnConfirmar_Click(object sender, System.EventArgs e)
    {
        return;
        if (Funcoes.strToDouble(txtValorTotal.Text.ToString()) <= Funcoes.strToDouble(lblSaldo.Text.ToString()))
        {
            if (Funcoes.strToDouble(txtValorTotal.Text.ToString()) > 0)
            {
                Funcoes.GravaAuditoria(0, Request.ServerVariables["REMOTE_ADDR"].ToString(), "Pagamento de PIX", "Confirmar o pagamento: " + txtPIX.Text.ToString());

                string sToken = asaas.PegarTokenSubconta(Funcoes.strToInt(HttpContext.Current.Session["PESSOA"].ToString()), Funcoes.strToInt(HttpContext.Current.Session["LICENCIADO"].ToString()));

                // Criar o json de criação de pagamento
                dadosPagamentoPixAsaaS.PagarQRCode dpagamento = new dadosPagamentoPixAsaaS.PagarQRCode()
                {
                    qrCode = new dadosPagamentoPixAsaaS.qrCode()
                    {
                        payload=txtPIX.Text.ToString(),
                        changeValue=0,
                    },
                    description = (txtReferencia.Text.ToString().Trim() != "") ? txtIdentificacao.Text.ToString().Trim() + "(" + txtReferencia.Text.ToString().Trim() + ")" : txtIdentificacao.Text.ToString().Trim(),
                    value = (float)Convert.ToDouble(txtValorTotal.Text.ToString()),
                    scheduleDate = Convert.ToDateTime(txtDataPagamento.Text.ToString().Trim())
                };

                string json = JsonConvert.SerializeObject(dpagamento);
                string jsonPIX = asaas.PagarQRCode(sToken, json);

                if (jsonPIX.ToString().Trim() != "")
                {

                    try
                    {
                        JObject oPIX = JObject.Parse(jsonPIX.ToString());
                        txtResposta.Text = jsonPIX.ToString();

                        SqlConnection connInsConsPagamentos = new SqlConnection(Funcoes.conexao());
                        connInsConsPagamentos.Open();
                        SqlCommand cmdInsConsPagamentos = new SqlCommand("dbo.stp_pagamentos_ins", connInsConsPagamentos);
                        cmdInsConsPagamentos.CommandType = CommandType.StoredProcedure;

                        cmdInsConsPagamentos.Parameters.Add("@flg_operacao", SqlDbType.Char).Value = 'I';

                        cmdInsConsPagamentos.Parameters.Add("@COD_ID_PESSOA_LICENCIADO", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["LICENCIADO"].ToString());
                        cmdInsConsPagamentos.Parameters.Add("@COD_ID_PESSOAS_FJ", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["PESSOA"].ToString());

                        cmdInsConsPagamentos.Parameters.Add("@DTA_DATA", SqlDbType.DateTime).Value = DateTime.Now;
                        cmdInsConsPagamentos.Parameters.Add("@NUM_VALOR", SqlDbType.Float).Value = Funcoes.strToDouble(txtValorTotal.Text.ToString());
                        cmdInsConsPagamentos.Parameters.Add("@DTA_PAGAMENTO", SqlDbType.DateTime).Value = Convert.ToDateTime(txtDataPagamento.Text.ToString());
                        cmdInsConsPagamentos.Parameters.Add("@NOM_DOCUMENTO", SqlDbType.Text).Value = txtPIX.Text.ToString();
                        cmdInsConsPagamentos.Parameters.Add("@DES_JSON", SqlDbType.Text).Value = jsonPIX.ToString();

                        cmdInsConsPagamentos.Parameters.Add("@FLG_TIPO", SqlDbType.VarChar).Value = "PIX";
                        cmdInsConsPagamentos.Parameters.Add("@FLG_ORIGEM", SqlDbType.VarChar).Value = "ASAAS";
                        cmdInsConsPagamentos.Parameters.Add("@FLG_STATUS", SqlDbType.VarChar).Value = oPIX["status"].ToString();

                        cmdInsConsPagamentos.ExecuteNonQuery();
                        connInsConsPagamentos.Close();
                        connInsConsPagamentos.Dispose();


                        divDocumento.Visible = false;
                        divPagamentorealizado.Visible = true;

                        btnConfirmar.Visible = false;
                        btnNovoPagamento.Visible = true;

                        ClientScript.RegisterStartupScript(this.GetType(), "SucessoPIX", "alert('Pagamento PIX realizado com sucesso!');", true);

                    }
                    catch
                    {
                        try
                        {
                            JObject oPix = JObject.Parse(jsonPIX.ToString());
                            //ClientScript.RegisterStartupScript(this.GetType(), "Leitura", "alert('Ocorreu um erro ao identificar o boleto! Verifique e reentre.');", true);
                            ClientScript.RegisterStartupScript(this.GetType(), "LeituraErro", "alert('Não Autorizado: " + oPix["errors"][0]["description"] + " Verifique e tente novamente.');", true);
                        }
                        catch
                        {
                            ClientScript.RegisterStartupScript(this.GetType(), "Erro", "alert('Ocorreu um erro ao tentar Pagar o PIX! Verifique e reentre.');", true);

                            btnContinuar.Visible = true;
                            btnConfirmar.Visible = false;
                            divDocumento.Visible = false;

                        }
                    }
                }
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
    protected void btnNovoPagamento_Click(object sender, System.EventArgs e)
    {
        Response.Redirect("con_pagamento_pix.aspx");
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

                ClientScript.RegisterStartupScript(this.GetType(), "AtualizarDadosCadastrais", "alert('Conta Digital informa que: " + erroApi + "');", true);


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
}