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


public partial class con_pagamentos_transferencias : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (HttpContext.Current.Session.Count <= 0)
        {
            Response.Redirect("login.aspx");
            FormsAuthentication.SignOut();
        }

        //Funcoes.CONFIRMA(btnConfirmar, "Confirma o pagamento do PIX informado?");

        if (!IsPostBack)
        {
            // Carrega tabela bancos
            
            SqlConnection myBanco = new SqlConnection(Funcoes.conexao());
            myBanco.Open();
            SqlCommand cmdBanco = new SqlCommand("dbo.stp_bancos_ins", myBanco);
            cmdBanco.CommandType = CommandType.StoredProcedure;
            cmdBanco.Parameters.Add("@flg_operacao", SqlDbType.VarChar).Value = "L";
            SqlDataAdapter drBanco = new SqlDataAdapter();
            drBanco.SelectCommand = cmdBanco;
            DataSet dsBanco = new DataSet();
            drBanco.Fill(dsBanco, "BANCO");
            ddlBanco.DataTextField = "NOM_BANCO";
            ddlBanco.DataValueField = "COD_ID";
            ddlBanco.DataSource = dsBanco.Tables["BANCO"].DefaultView;
            ddlBanco.DataBind();
            ddlBanco.Items.Insert(0, new ListItem("Selecione o banco de destino", "0"));
            
            lblEmailConfirmacao.Text = Funcoes.OcultarCaracteresEmail(Funcoes.DadosEstabelecimento("E"));
            Funcoes.GravaAuditoria(0, Request.ServerVariables["REMOTE_ADDR"].ToString(), "Pagamentos e Transferências", "Realizar Pagamento/Transferência");

            //btnContinuar.Visible = true;
            //btnConfirmar.Visible = false;

            ConsultaSaldoBaas("A");
            txtDataPagamento.Text = DateTime.Now.ToShortDateString();
        }
    }

    private void DesabilitaTodos()
    {
        // Boletos
        divBoleto.Visible=false;
        divCodigoBarras.Visible = false;
        divDarf.Visible = false;

        // Transferir
        divTransferir.Visible = false;
        divContaDigital.Visible = false;
        divPix.Visible = false;

        divConta.Visible = false;
        
        // Padrao Transferencias
        divPadrao.Visible = false;
        divPadraoTransferencia.Visible = false;

        // Retorno Documento Boleto
        divDocumento.Visible = false;
    }



    protected void btnBoletos_Click(object sender, EventArgs e)
    {

        DesabilitaTodos();

        divCodigoBarras.Visible = true;
        btnContinuarBoletos.Visible = true;
    }

    protected void btnTransferir_Click(object sender, EventArgs e)
    {
        DesabilitaTodos();
        divTransferir.Visible = true;
        divPix.Visible = true;

        divPadraoTransferencia.Visible = true;
    }
    protected void btnContinuar_Click(object sender, System.EventArgs e)
    {

    }
    protected void btnConfirmar_Click(object sender, System.EventArgs e)
    {

    }
    protected void btnContinuarBoletos_Click(object sender, System.EventArgs e)
    {
        //return;

        if (rbtCodigoBarras.Checked == true)
        {
            if (txtLinhaDigitavel.Text.ToString().Trim() != "")
            {

                Funcoes.GravaAuditoria(0, Request.ServerVariables["REMOTE_ADDR"].ToString(), "Pagamento Boleto", "Continuar com pagamento: " + txtLinhaDigitavel.Text.ToString().Trim());


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
                            btnContinuarBoletos.Visible = false;
                            btnConfirmarBoletosPagamento.Visible = true;

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
                            btnContinuarBoletos.Visible = true;
                            btnConfirmarBoletosPagamento.Visible = false;
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
        
        if (rbtPixCopiaCola.Checked == true)
        {
            if (txtLinhaDigitavel.Text.ToString().Trim() != "")
            {

                Funcoes.GravaAuditoria(0, Request.ServerVariables["REMOTE_ADDR"].ToString(), "Pagamento Pix", "Continuar com pagamento: " + txtLinhaDigitavel.Text.ToString().Trim());


                string sToken = asaas.PegarTokenSubconta(Funcoes.strToInt(HttpContext.Current.Session["PESSOA"].ToString()), Funcoes.strToInt(HttpContext.Current.Session["LICENCIADO"].ToString()));
                dadosPagamentoPixAsaaS.DecodificarQRCode dpagamento = new dadosPagamentoPixAsaaS.DecodificarQRCode()
                {
                    payload = txtLinhaDigitavel.Text.ToString(),
                    changeValue = 0,
                };
                string json = JsonConvert.SerializeObject(dpagamento);
                string jsonPixCopiaCola = asaas.DecodificarQRCode(sToken, json);
                if (jsonPixCopiaCola.ToString().Trim() != "")
                {
                    JObject oPixCopiaCola = JObject.Parse(jsonPixCopiaCola.ToString());
                    txtResposta.Text = jsonPixCopiaCola.ToString();

                    try
                    {

                        if (Convert.ToBoolean(oPixCopiaCola["canBePaid"].ToString()) == false)
                        {
                            // Mensagem de Vencimento
                            ClientScript.RegisterStartupScript(this.GetType(), "QRCodeVencido", "alert('O Pix copia e cola (QRCode) encontra-se " + oPixCopiaCola["cannotBePaidReason"].ToString() + "! Entre em contato com a instituição emissora.');", true);
                        }
                        else
                        {
                            btnContinuarBoletos.Visible = false;
                            btnConfirmarBoletosPagamento.Visible = true;

                            divDocumento.Visible = true;
                            lblCodigoBarras.Text = oPixCopiaCola["payload"].ToString();
                            lblInstituicaoEmissora.Text = oPixCopiaCola["receiver"]["name"].ToString();
                            lblBancoEmissor.Text = oPixCopiaCola["receiver"]["ispbName"].ToString();

                            if (oPixCopiaCola["receiver"]["ispbName"].ToString().Trim() != "")
                            {
                                lblBancoEmissor.Text = oPixCopiaCola["receiver"]["ispbName"].ToString();
                            }
                            else
                            {
                                lblBancoEmissor.Text = "Banco emissor não especificado";
                            }


                            lblValorDocumento.Text = String.Format("{0:n2}", Funcoes.strToDouble(oPixCopiaCola["value"].ToString()));
                            txtValorPagar.Text = String.Format("{0:n2}", Funcoes.strToDouble(oPixCopiaCola["totalValue"].ToString()));
                            if (Convert.ToBoolean(oPixCopiaCola["canBePaidWithDifferentValue"].ToString()) == true)
                            {
                                txtValorPagar.Enabled = true;
                            }
                            else
                            {
                                txtValorPagar.Enabled = false;
                            }

                            //txtMenorValor.Text = string.Format("{0:n2}", Funcoes.strToDouble(oBoleto["bankSlipInfo"]["minValue"].ToString().Trim()));
                            //txtMaiorValor.Text = string.Format("{0:n2}", Funcoes.strToDouble(oBoleto["bankSlipInfo"]["maxValue"].ToString().Trim()));
                            //lblTaxaCobrada.Text = string.Format("{0:n2}", Funcoes.strToDouble(oBoleto["fee"].ToString().Trim()));

                            if ((oPixCopiaCola["discount"].ToString().Trim() == "") && (oPixCopiaCola["interest"].ToString().Trim() == "") && (oPixCopiaCola["fine"].ToString().Trim() == ""))
                            {
                                divValores.Visible = false;
                            }
                            else
                            {
                                divValores.Visible = true;
                                lblDesconto.Text = string.Format("{0:n2}", Funcoes.strToDouble(oPixCopiaCola["discount"].ToString().Trim()));
                                lblMulta.Text = string.Format("{0:n2}", Funcoes.strToDouble(oPixCopiaCola["fine"].ToString().Trim()));
                                lblJuros.Text = string.Format("{0:n2}", Funcoes.strToDouble(oPixCopiaCola["interest"].ToString().Trim()));
                            }
                            if (oPixCopiaCola["dueDate"].ToString().Trim() != "")
                            {
                                lblVencimento.Text = Convert.ToDateTime(oPixCopiaCola["dueDate"].ToString()).ToShortDateString();
                            }
                            else
                            {
                                lblVencimento.Text = Convert.ToDateTime(oPixCopiaCola["expirationDate"].ToString()).ToShortDateString();
                            }

                            txtDataPagamento.Text = DateTime.Now.ToShortDateString();

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
                            ClientScript.RegisterStartupScript(this.GetType(), "Leitura", "alert('Ocorreu um erro ao tentar identificar o Pix copia e cola (QR code)! Verifique e reentre.');", true);
                            btnContinuarBoletos.Visible = true;
                            btnConfirmarBoletosPagamento.Visible = false;
                            divDocumento.Visible = false;

                    }
                }
            }
            else
            {
                ClientScript.RegisterStartupScript(this.GetType(), "QrcodeCopiaCola", "alert('Nenhum Pix copia e cola (QR Code) foi informado! Verifique e reentre.');", true);

            }

        }

    }


    private void CriarPagamentoBoleto()
    {
        if (rbtCodigoBarras.Checked == true)
        {

            if (Funcoes.strToDouble(txtValorTotal.Text.ToString()) <= Funcoes.strToDouble(lblSaldo.Text.ToString()))
            {
                if (Funcoes.strToDouble(txtValorTotal.Text.ToString()) > 0)
                {
                    Funcoes.GravaAuditoria(0, Request.ServerVariables["REMOTE_ADDR"].ToString(), "Pagamento Boleto", "Confirmar o pagamento: " + txtLinhaDigitavel.Text.ToString());

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
                    string jsonBoleto = asaas.CriarPagamento(asaas.PegarTokenSubconta(Funcoes.strToInt(HttpContext.Current.Session["PESSOA"].ToString()), Funcoes.strToInt(HttpContext.Current.Session["LICENCIADO"].ToString())), json);

                    if (jsonBoleto.ToString().Trim() != "")
                    {
                        JObject oBoleto = JObject.Parse(jsonBoleto.ToString());
                        txtResposta.Text = jsonBoleto.ToString();

                        Funcoes.GravarOPeracoesBaas(oBoleto["object"].ToString(), oBoleto["id"].ToString(), oBoleto["status"].ToString(), "BOLETO", jsonBoleto);
                        GravaCobranca("", oBoleto["id"].ToString(), jsonBoleto.ToString(), oBoleto["status"].ToString(), txtValorTotal.Text.ToString(), txtIdentificacao.Text.ToString(), "Boleto Bancário","A");

                        ClientScript.RegisterStartupScript(this.GetType(), "StatusOperacao", "alert('Pagamento em boleto realizado com sucesso! Status da operação: " + Funcoes.RetornaStatusBaas(oBoleto["status"].ToString()).ToString() + "');", true);
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
        if (rbtPixCopiaCola.Checked==true)
        {
            if (Funcoes.strToDouble(txtValorTotal.Text.ToString()) <= Funcoes.strToDouble(lblSaldo.Text.ToString()))
            {
                if (Funcoes.strToDouble(txtValorTotal.Text.ToString()) > 0)
                {
                    Funcoes.GravaAuditoria(0, Request.ServerVariables["REMOTE_ADDR"].ToString(), "Pagamento Boleto", "Confirmar o pagamento: " + txtLinhaDigitavel.Text.ToString());

                    string sToken = asaas.PegarTokenSubconta(Funcoes.strToInt(HttpContext.Current.Session["PESSOA"].ToString()), Funcoes.strToInt(HttpContext.Current.Session["LICENCIADO"].ToString()));

                    // Criar o json de criação de pagamento
                    dadosPagamentoPixAsaaS.PagarQRCode dpagamento = new dadosPagamentoPixAsaaS.PagarQRCode()
                    {
                        description = txtIdentificacao.Text.ToString(),
                        value = (float)Convert.ToDouble(txtValorPagar.Text.ToString()) + (float)Convert.ToDouble(lblMulta.Text.ToString()) + (float)Convert.ToDouble(lblJuros.Text.ToString()) - (float)Convert.ToDouble(lblDesconto.Text.ToString()),
                        scheduleDate = Convert.ToDateTime(txtDataPagamento.Text.ToString()),
                        qrCode = new dadosPagamentoPixAsaaS.qrCode()
                        {
                            payload = txtLinhaDigitavel.Text.ToString(),
                            changeValue = 0
                        }
                    };

                    string json = JsonConvert.SerializeObject(dpagamento);
                    string jsonPixCopiaCola = asaas.PagarQRCode(asaas.PegarTokenSubconta(Funcoes.strToInt(HttpContext.Current.Session["PESSOA"].ToString()), Funcoes.strToInt(HttpContext.Current.Session["LICENCIADO"].ToString())), json);

                    if (jsonPixCopiaCola.ToString().Trim() != "")
                    {
                        JObject oPixCopiaCola = JObject.Parse(jsonPixCopiaCola.ToString());
                        txtResposta.Text = jsonPixCopiaCola.ToString();

                        Funcoes.GravarOPeracoesBaas(oPixCopiaCola["finality"].ToString(), oPixCopiaCola["id"].ToString(), oPixCopiaCola["status"].ToString(), "PIX QRCODE", jsonPixCopiaCola);
                        GravaCobranca("", oPixCopiaCola["id"].ToString(), jsonPixCopiaCola.ToString(), oPixCopiaCola["status"].ToString(), txtValorTotal.Text.ToString(), txtIdentificacao.Text.ToString(), "Pix QRCode","A");

                        ClientScript.RegisterStartupScript(this.GetType(), "StatusOperacao", "alert('Pagamento do Pix Copia e Cola realizado com sucesso! Status da operação: " + Funcoes.RetornaStatusBaas(oPixCopiaCola["status"].ToString()).ToString() + "');", true);
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

    }
    protected void btnConfirmarBoletosPagamento_Click(object sender, System.EventArgs e)
    {
        // Enviar 2FA

        if (Funcoes.Enviar2faEstabelecimento() == true)
        {
            ClientScript.RegisterStartupScript(this.GetType(),
"Codigo2fa", "alert('Um código de confirmação foi enviado para o seu e-mail!');", true);

        }
        else
        {
            ClientScript.RegisterStartupScript(this.GetType(),
"Codigo2faErro", "alert('Ocorreu um erro ao tentar enviar o Código de Confirmação!');", true);

        }
            ClientScript.RegisterStartupScript(this.GetType(),
"ExecutaModal", "$('#mdConfirmar').modal('show');", true);

        
    }


    protected void lkbReenviar_Click(object sender, System.EventArgs e)
    {
        if (Funcoes.Enviar2faEstabelecimento() == true)
        {
            ClientScript.RegisterStartupScript(this.GetType(),
"Codigo2fa", "alert('Um código de confirmação foi enviado para o seu e-mail!');", true);

        }
        else
        {
            ClientScript.RegisterStartupScript(this.GetType(),
"Codigo2faErro", "alert('Ocorreu um erro ao tentar enviar o Código de Confirmação!');", true);

        }
        ClientScript.RegisterStartupScript(this.GetType(),
"ExecutaModal", "$('#mdConfirmar').modal('show');", true);

    }

    protected void btnConfirmarPagamentoTransferencia2FA_Click(object sender, System.EventArgs e)
    {
                // Verifica se o código autenticação esta correto
        SqlConnection connVerifica2fa = new SqlConnection(Funcoes.conexao());
        SqlCommand cmdVerifica2fa = new SqlCommand("dbo.stp_2fa_ins", connVerifica2fa);
        cmdVerifica2fa.CommandType = CommandType.StoredProcedure;
        cmdVerifica2fa.Parameters.Add("@flg_operacao", SqlDbType.Char).Value = "L";
        cmdVerifica2fa.Parameters.Add("@COD_ID_SIS_USUARIO", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["CODIGO"].ToString());
        cmdVerifica2fa.Parameters.Add("@COD_ID_PESSOA_LICENCIADO", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["LICENCIADO"].ToString());
        cmdVerifica2fa.Parameters.Add("@NOM_2FA", SqlDbType.VarChar).Value = Funcoes.Encrypt(txt2FA.Text.ToString());
        cmdVerifica2fa.Parameters.Add("@DTA_EXPIRACAO", SqlDbType.DateTime).Value = DateTime.Now;
        connVerifica2fa.Open();
        SqlDataReader mReader2fa = cmdVerifica2fa.ExecuteReader();

        if (mReader2fa.Read())
        {

            ClientScript.RegisterStartupScript(this.GetType(),
    "ExecutaModal", "$('#mdSenha').modal('show');", true);
        
        }
        else
        {
            ClientScript.RegisterStartupScript(this.GetType(),
                "CodigoErrado2fa", "alert('O Código de confirmação de autenticação não está correto! Verifique e reentre.');", true);
        }
    }


    private void ConfirmarPagamentoGeral()
    {
        if (divCodigoBarras.Visible == true)
        {
            CriarPagamentoBoleto();
        }
        if (divTransferir.Visible == true)
        {
            CriarTransferencias();
        }
        if (divContaDigital.Visible == true)
        {
            CriarTransferenciasContaDigital();
        }
    }

    private void GerenciarOpcoes()
    {
        divBoleto.Visible = false;
        divDocumento.Visible = false;
        divCodigoBarras.Visible = false;
        divContaDigital.Visible = false;
        divTransferir.Visible = false;
        divPix.Visible = false;
        divPadrao.Visible = false;
    }
    protected void btnTransferencias_Click(object sender, System.EventArgs e)
    {
        GerenciarOpcoes();

        divTransferir.Visible = true;
        divPix.Visible = true;
        btnContinuarTransferencias.Visible = true;
        rbtTransferenciaPix_CheckedChanged(null, null);


        //btnConfirmarTransferenciasPix.Visible = false;
    }
    protected void btnContinuarTransferencias_Click(object sender, System.EventArgs e)
    {
        // Enviar 2FA

        if (Funcoes.Enviar2faEstabelecimento() == true)
        {
            ClientScript.RegisterStartupScript(this.GetType(),
"Codigo2fa", "alert('Um código de confirmação foi enviado para o seu e-mail!');", true);

        }
        else
        {
            ClientScript.RegisterStartupScript(this.GetType(),
"Codigo2faErro", "alert('Ocorreu um erro ao tentar enviar o Código de Confirmação!');", true);

        }
        ClientScript.RegisterStartupScript(this.GetType(),
"ExecutaModal", "$('#mdConfirmar').modal('show');", true);

    }
    protected void rbtTransferenciaPix_CheckedChanged(object sender, System.EventArgs e)
    {
        divPix.Visible = true;
        divConta.Visible = false;
        divPadraoTransferencia.Visible = true;
    }
    protected void rbtTransferenciaConta_CheckedChanged(object sender, System.EventArgs e)
    {
        divPix.Visible = false;
        divConta.Visible = true;
        divPadraoTransferencia.Visible = true;
    }

    private void CriarTransferencias()
    {
        
        if (rbtTransferenciaPix.Checked == true)
        {
            if (Funcoes.strToDouble(txtValorTransferencia.Text.ToString()) <= Funcoes.strToDouble(lblSaldo.Text.ToString()))
            {
                if (Funcoes.strToDouble(txtValorTransferencia.Text.ToString()) >= 10)
                {
                    Funcoes.GravaAuditoria(0, Request.ServerVariables["REMOTE_ADDR"].ToString(), "Transferência PIX", "Confirmar Transferência: " + txtChavePIX.Text.ToString());

                    dadosTransferenciaPixAsaaS.Root dTransferencia = new dadosTransferenciaPixAsaaS.Root()
                    {
                        value = Funcoes.strToDouble(txtValorTransferencia.Text.ToString()),
                        operationType = "PIX",
                        pixAddressKey = txtChavePIX.Text.ToString(),
                        pixAddressKeyType = ddlTipoPIX.SelectedValue.ToString(),
                        description = txtDescricaoTransferencia.Text.ToString(),
                        scheduleDate = Convert.ToDateTime(txtDataTransferencia.Text.ToString())
                    };
                    
                    string json = JsonConvert.SerializeObject(dTransferencia);
                    try
                    {
                        string jsonTransferencia = asaas.TransferenciaPIXAsaaS(asaas.PegarTokenSubconta(Funcoes.strToInt(HttpContext.Current.Session["PESSOA"].ToString()), Funcoes.strToInt(HttpContext.Current.Session["LICENCIADO"].ToString())), json);

                        if (jsonTransferencia.ToString().Trim() != "")
                        {
                            //txtChavePIX.Text = jsonTransferencia;
                            JObject oTransferencia = JObject.Parse(jsonTransferencia.ToString());
                            try
                            {

                                Funcoes.GravarOPeracoesBaas(oTransferencia["object"].ToString(), oTransferencia["id"].ToString(), oTransferencia["status"].ToString(), "PIX", jsonTransferencia);
                                GravaCobranca("", oTransferencia["id"].ToString(), jsonTransferencia.ToString(), oTransferencia["status"].ToString(), txtValorTransferencia.Text.ToString(), txtDescricaoTransferencia.Text.ToString(), "Transferência PIX","A");

                                ClientScript.RegisterStartupScript(this.GetType(), "StatusOperacao", "alert('Transferência realizada com sucesso! Status da operação: " + Funcoes.RetornaStatusBaas(oTransferencia["status"].ToString()).ToString() + "');", true);
                            }
                            catch
                            {
                                ClientScript.RegisterStartupScript(this.GetType(), "ErroRetorno", "alert('Ocorreu um erro ao processar a transferência ("+oTransferencia["errors"][0]["description"].ToString()+")! Verifique e reentre');", true);
                            }
                        }
                    }
                    catch
                    {
                        ClientScript.RegisterStartupScript(this.GetType(), "ErroEnvio", "alert('Não foi possível processar a transferência! Verifique e reentre');", true);

                    }
                }
                else
                {
                    ClientScript.RegisterStartupScript(this.GetType(), "ValorTotalZerado", "alert('O valor total não foi especificado ou o valor é menor que o mínimo permitido! Verifique e reentre');", true);
                }
            }
            else
            {
                ClientScript.RegisterStartupScript(this.GetType(), "SaldoInsufuciente", "alert('Você não possui saldo suficiente para realizar este pagamento! Verifique e reentre.');", true);
            }
        }
        
        if (rbtTransferenciaConta.Checked == true)
        {
            if (Funcoes.strToDouble(txtValorTransferencia.Text.ToString()) <= Funcoes.strToDouble(lblSaldo.Text.ToString()))
            {
                if (Funcoes.strToDouble(txtValorTransferencia.Text.ToString()) >= 10)
                {
                    Funcoes.GravaAuditoria(0, Request.ServerVariables["REMOTE_ADDR"].ToString(), "Transferência Dados Bancários", "Confirmar Transferência: " + txtNome.Text.ToString());

                    dadosTransferenciaPixAsaaS.Root dTransferencia = new dadosTransferenciaPixAsaaS.Root()
                    {
                        value = Funcoes.strToDouble(txtValorTransferencia.Text.ToString()),
                        operationType = "TED",
                        //pixAddressKey = txtChavePIX.Text.ToString(),
                        //pixAddressKeyType = ddlTipoPIX.SelectedValue.ToString(),
                        description = txtDescricaoTransferencia.Text.ToString(),
                        scheduleDate = Convert.ToDateTime(txtDataTransferencia.Text.ToString()),
                        bankAccount = new dadosTransferenciaPixAsaaS.BankAccount()
                        {
                            bank = new dadosTransferenciaPixAsaaS.Bank()
                            {
                                code = ddlBanco.SelectedValue.ToString().Trim().PadLeft(3,'0')
                            },
                            ownerName = txtNome.Text.ToString(),
                            cpfCnpj = txtDocumento.Text.ToString(),
                            agency = txtAgencia.Text.ToString().PadLeft(4,'0'),
                            account = txtConta.Text.ToString(),
                            accountDigit = txtContaDigito.Text.ToString(),
                            bankAccountType="CONTA_CORRENTE"
                        }
                    };

                    string json = JsonConvert.SerializeObject(dTransferencia);
                    try
                    {
                        string jsonTransferencia = asaas.TransferenciaPIXAsaaS(asaas.PegarTokenSubconta(Funcoes.strToInt(HttpContext.Current.Session["PESSOA"].ToString()), Funcoes.strToInt(HttpContext.Current.Session["LICENCIADO"].ToString())), json);

                        if (jsonTransferencia.ToString().Trim() != "")
                        {
                            JObject oTransferencia = JObject.Parse(jsonTransferencia.ToString());
                            try
                            {
                                Funcoes.GravarOPeracoesBaas(oTransferencia["object"].ToString(), oTransferencia["id"].ToString(), oTransferencia["status"].ToString(), "CONTA BANCÁRIA", jsonTransferencia);
                                GravaCobranca("", oTransferencia["id"].ToString(), jsonTransferencia.ToString(), oTransferencia["status"].ToString(), txtValorTransferencia.Text.ToString(), txtDescricaoTransferencia.Text.ToString(), "Transferência PIX","A");

                                ClientScript.RegisterStartupScript(this.GetType(), "StatusOperacao", "alert('Transferência realizada com sucesso! Status da operação: " + Funcoes.RetornaStatusBaas(oTransferencia["status"].ToString()).ToString() + "');", true);
                            }
                            catch
                            {
                                ClientScript.RegisterStartupScript(this.GetType(), "ErroRetorno", "alert('Ocorreu um erro ao processar a transferência (" + oTransferencia["errors"][0]["description"].ToString() + ")! Verifique e reentre');", true);
                            }
                        }
                    }
                    catch
                    {
                        ClientScript.RegisterStartupScript(this.GetType(), "ErroEnvio", "alert('Não foi possível processar a transferência! Verifique e reentre');", true);

                    }

                }
                else
                {
                    ClientScript.RegisterStartupScript(this.GetType(), "ValorTotalZerado", "alert('O valor total não foi especificado ou o valor é menor que o mínimo permitido! Verifique e reentre');", true);
                }
            }
            else
            {
                ClientScript.RegisterStartupScript(this.GetType(), "SaldoInsufuciente", "alert('Você não possui saldo suficiente para realizar este pagamento! Verifique e reentre.');", true);
            }

        }
        
    }

    protected void btnDigitais_Click(object sender, System.EventArgs e)
    {
        DesabilitaTodos();

        divContaDigital.Visible = true;
        btnContinuarContaDigital.Visible = true;
        //btnConfirmarTransferenciasPix.Visible = false;

    }
    protected void btnContinuarContaDigital_Click(object sender, System.EventArgs e)
    {
        if (Funcoes.Enviar2faEstabelecimento() == true)
        {
            ClientScript.RegisterStartupScript(this.GetType(), "Codigo2fa", "alert('Um código de confirmação foi enviado para o seu e-mail!');", true);
        }
        else
        {
            ClientScript.RegisterStartupScript(this.GetType(), "Codigo2faErro", "alert('Ocorreu um erro ao tentar enviar o Código de Confirmação!');", true);
        }
        ClientScript.RegisterStartupScript(this.GetType(), "ExecutaModal", "$('#mdConfirmar').modal('show');", true);
    }

    private void CriarTransferenciasContaDigital()
    {
        if (rbtTransferenciaPix.Checked == true)
        {
            if (Funcoes.strToDouble(txtValorTotal.Text.ToString()) <= Funcoes.strToDouble(lblSaldo.Text.ToString()))
            {
                if (Funcoes.strToDouble(txtValorTotal.Text.ToString()) > 0)
                {
                    Funcoes.GravaAuditoria(0, Request.ServerVariables["REMOTE_ADDR"].ToString(), "Transferência PIX", "Confirmar Transferência: " + txtChavePIX.Text.ToString());

                    string sToken = asaas.PegarTokenSubconta(Funcoes.strToInt(HttpContext.Current.Session["PESSOA"].ToString()), Funcoes.strToInt(HttpContext.Current.Session["LICENCIADO"].ToString()));

                    // Criar o json de criação da transferencia
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

                    string jsonBoleto = asaas.CriarPagamento(asaas.PegarTokenSubconta(Funcoes.strToInt(HttpContext.Current.Session["PESSOA"].ToString()), Funcoes.strToInt(HttpContext.Current.Session["LICENCIADO"].ToString())), json);

                    if (jsonBoleto.ToString().Trim() != "")
                    {
                        JObject oBoleto = JObject.Parse(jsonBoleto.ToString());
                        txtResposta.Text = jsonBoleto.ToString();

                        Funcoes.GravarOPeracoesBaas(oBoleto["object"].ToString(), oBoleto["id"].ToString(), oBoleto["status"].ToString(), "BOLETO", jsonBoleto);


                        ClientScript.RegisterStartupScript(this.GetType(), "StatusOperacao", "alert('Status da operação: " + Funcoes.RetornaStatusBaas(oBoleto["status"].ToString()).ToString() + "');", true);
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

        if (rbtTransferenciaConta.Checked == true)
        {

        }

    }

    protected void btnConfirmarSenha_Click(object sender, System.EventArgs e)
    {

        if (HttpContext.Current.Session["SENHA"].ToString().Trim()==Funcoes.Encrypt(txtSenha.Text.ToString()))
        {
            ConfirmarPagamentoGeral();
        }
        else
        {
            ClientScript.RegisterStartupScript(this.GetType(),
                "CodigoErrado2fa", "alert('Senha inválida! Verifique e reentre.');", true);
        }



    }

    private void GravaCobranca(string sIDCliente, string sIDCobranca, string sJsonBoleto, string sStatus, string sValor, string sDescricao, string sTipo, string sOrigem)
    {
        SqlConnection connInsConsCliente = new SqlConnection(Funcoes.conexao());
        connInsConsCliente.Open();
        SqlCommand cmdInsConsCliente = new SqlCommand("dbo.stp_cobrancas_ins", connInsConsCliente);
        cmdInsConsCliente.CommandType = CommandType.StoredProcedure;
        cmdInsConsCliente.Parameters.Add("@flg_operacao", SqlDbType.Char).Value = 'I';
        cmdInsConsCliente.Parameters.Add("@COD_ID_PESSOA_LICENCIADO", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["LICENCIADO"].ToString());
        cmdInsConsCliente.Parameters.Add("@COD_ID_PESSOAS_FJ_VENDEDOR", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["PESSOA"].ToString());
        cmdInsConsCliente.Parameters.Add("@COD_ID_COMPRADOR", SqlDbType.VarChar).Value = sIDCliente.ToString();
        cmdInsConsCliente.Parameters.Add("@COD_ID_COBRANCA", SqlDbType.VarChar).Value = sIDCobranca.ToString();

        cmdInsConsCliente.Parameters.Add("@DTA_DATA", SqlDbType.DateTime).Value = DateTime.Now;
        cmdInsConsCliente.Parameters.Add("@FLG_TIPO", SqlDbType.Char).Value = sTipo.ToString();

        cmdInsConsCliente.Parameters.Add("@NOM_NOME", SqlDbType.VarChar).Value = "";
        cmdInsConsCliente.Parameters.Add("@NUM_CNPJCPF", SqlDbType.VarChar).Value = "";
        cmdInsConsCliente.Parameters.Add("@NOM_EMAIL", SqlDbType.VarChar).Value = "";

        cmdInsConsCliente.Parameters.Add("@DES_DESCRICAO", SqlDbType.Text).Value = sDescricao.ToString();
        cmdInsConsCliente.Parameters.Add("@NOM_REFERENCIA", SqlDbType.VarChar).Value = txtReferencia.Text.ToString();
        cmdInsConsCliente.Parameters.Add("@DTA_VENCIMENTO", SqlDbType.DateTime).Value = Convert.ToDateTime(txtDataPagamento.Text.ToString());
        cmdInsConsCliente.Parameters.Add("@NUM_VALOR", SqlDbType.Float).Value = Funcoes.strToDouble(sValor.ToString());

        cmdInsConsCliente.Parameters.Add("@DES_JSON", SqlDbType.Text).Value = sJsonBoleto.ToString();
        cmdInsConsCliente.Parameters.Add("@FLG_STATUS", SqlDbType.VarChar).Value = sStatus.ToString();

        cmdInsConsCliente.Parameters.Add("@NOM_URL_BOLETO", SqlDbType.VarChar).Value = "";

        cmdInsConsCliente.ExecuteNonQuery();
        connInsConsCliente.Close();
        connInsConsCliente.Dispose();
    }

    protected void rbtPixCopiaCola_CheckedChanged(object sender, System.EventArgs e)
    {

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
}