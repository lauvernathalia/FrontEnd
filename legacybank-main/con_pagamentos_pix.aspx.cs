using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Collections.Specialized;
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

using MessagingToolkit.QRCode.Codec;
using MessagingToolkit.QRCode.Codec.Data;
using System.Drawing;

public partial class con_pagamentos_pix : System.Web.UI.Page
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
            Funcoes.GravaAuditoria(0, Request.ServerVariables["REMOTE_ADDR"].ToString(), "Pix", "Realizar novo Pagamento");
            VerificaPerfilComercial();

            //btnContinuar.Visible = true;
            //btnConfirmar.Visible = false;

            ConsultaSaldoBaas("A");
            CarregaAdquirentes();
            AtualizaListaClientes();

        }
    }

    private void VerificaPerfilComercial()
    {
        btnReceberQRCode.Visible = Funcoes.CarregaPerfilComercial("X");
    }


    private void AtualizaListaClientes()
    {
        // Tabela de Compradores

        SqlConnection myCompradores = new SqlConnection(Funcoes.conexao());
        myCompradores.Open();
        SqlCommand cmdCompradores = new SqlCommand("dbo.stp_compradores_ins", myCompradores);
        cmdCompradores.CommandType = CommandType.StoredProcedure;
        cmdCompradores.Parameters.Add("@flg_operacao", SqlDbType.VarChar).Value = "C";
        cmdCompradores.Parameters.Add("@COD_ID_PESSOA_LICENCIADO", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["LICENCIADO"].ToString());
        cmdCompradores.Parameters.Add("@COD_ID_PESSOAS_FJ", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["PESSOA"].ToString());
        cmdCompradores.Parameters.Add("@NOM_NOME", SqlDbType.VarChar).Value = "";
        cmdCompradores.Parameters.Add("@NUM_CNPJCPF", SqlDbType.VarChar).Value = "";
        cmdCompradores.Parameters.Add("@FLG_TIPO", SqlDbType.Char).Value = "C";


        SqlDataAdapter drCompradores = new SqlDataAdapter();
        drCompradores.SelectCommand = cmdCompradores;
        DataSet dsCompradores = new DataSet();
        drCompradores.Fill(dsCompradores, "PESSOAS_FJ");
        ddlClientePadrao.DataTextField = "NOM_NOME";
        ddlClientePadrao.DataValueField = "COD_ID";
        ddlClientePadrao.DataSource = dsCompradores.Tables["PESSOAS_FJ"].DefaultView;
        ddlClientePadrao.DataBind();
        ddlClientePadrao.Items.Insert(0, new ListItem("", ""));

    }


    private void CarregaAdquirentes()
    {
        SqlConnection myAdquirentes = new SqlConnection(Funcoes.conexao());
        myAdquirentes.Open();
        SqlCommand cmdAdquirentes = new SqlCommand("dbo.stp_pessoas_fj_integracoes_ins", myAdquirentes);
        cmdAdquirentes.CommandType = CommandType.StoredProcedure;
        cmdAdquirentes.Parameters.Add("@flg_operacao", SqlDbType.VarChar).Value = "F";
        cmdAdquirentes.Parameters.Add("@COD_ID_PESSOA_LICENCIADO", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["LICENCIADO"].ToString());
        cmdAdquirentes.Parameters.Add("@COD_ID_PESSOAS_FJ", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["PESSOA"].ToString());
        cmdAdquirentes.Parameters.Add("@FLG_ACESSO", SqlDbType.Char).Value = "S";
        SqlDataAdapter drAdquirentes = new SqlDataAdapter();
        drAdquirentes.SelectCommand = cmdAdquirentes;
        DataSet dsAdquirentes = new DataSet();
        drAdquirentes.Fill(dsAdquirentes, "PESSOAS_FJ_INTEGRACOES");

        ddlAdquirentes.DataTextField = "NOM_FANTASIA_INTEGRACAO";
        ddlAdquirentes.DataValueField = "FLG_INTEGRACAO";
        ddlAdquirentes.DataSource = dsAdquirentes.Tables["PESSOAS_FJ_INTEGRACOES"].DefaultView;
        ddlAdquirentes.DataBind();
    }
    protected void btnTransferirPix_Click(object sender, EventArgs e)
    {
        Funcoes.GravaAuditoria(0, Request.ServerVariables["REMOTE_ADDR"].ToString(), "PIX - Transferência PIX", "Nova Transferência");
        ControleOpcoes();
        lblTituloPadraoTopo.Text = "Transferir PIX";
        divDadosCobranca.Visible = true;
        divTransferirPix.Visible = true;

    }
    protected void btnReceberQRCode_Click(object sender, EventArgs e)
    {
        Funcoes.GravaAuditoria(0, Request.ServerVariables["REMOTE_ADDR"].ToString(), "PIX - Receber por QRCode", "Novo Recebimento PIX");
        ControleOpcoes();

        divDadosCobranca.Visible = true;
        divDadosCliente.Visible = true;

        divDetalhesCobranca.Visible = true;
        lblTituloPadraoTopo.Text = "Receber por QR Code";

        divReceberQRCode.Visible = true;
    }
    protected void btnChaves_Click(object sender, EventArgs e)
    {
        ControleOpcoes();

        divChaves.Visible = true;
        ConsultaContaDigital();
    }

    private void ConsultaContaDigital()
    {
        asaas.ContaDigital dContaDigital = new asaas.ContaDigital();
        dContaDigital = asaas.ConsultaContaDigital();
        txtChavePix.Text = dContaDigital.ChavePix.ToString();
    }

    private void ControleOpcoes()
    {
        divPadrao.Visible = false;
        divChaves.Visible = false;
        divReceberQRCode.Visible = false;
        divTransferirPix.Visible = false;
        divDadosCobranca.Visible = false;
        divDadosCliente.Visible = false;
        divDetalhesCobranca.Visible = false;
    }

    protected void btnVoltarPixQRCode_Click(object sender, System.EventArgs e)
    {

    }

    protected void btnContinuarPixQRCode_Click(object sender, System.EventArgs e)
    {
        if (ddlAdquirentes.SelectedValue.ToString() == "Z")
        {
            CobrancaZoop("QRCODE");
        }
        if (ddlAdquirentes.SelectedValue.ToString() == "P")
        {

        }
        if (ddlAdquirentes.SelectedValue.ToString() == "A")
        {
            CobrancaAsaas("QRCODE");
        }
        if (ddlAdquirentes.SelectedValue.ToString() == "C")
        {

        }
    }


    private void CobrancaAsaas(string sTipo)
    {
        string jsonRetorno = "";
        // -------------------------------------------------------------------------------------------------------------------------------------------------------------------
        // QRCODE ASAAS
        // -------------------------------------------------------------------------------------------------------------------------------------------------------------------
        
        if (sTipo == "QRCODE")
        {
            string sIDCliente = asaas.VerificaCliente("", "", txtNome.Text.ToString(), txtCNPJCPF.Text.ToString(), txtEmail.Text.ToString(), txtCEP.Text.ToString(), txtNumero.Text.ToString(), txtCelular.Text.ToString());

            if (sIDCliente.ToString().Trim() != "")
            {
                GravaCliente(sIDCliente.ToString());

                DadosCobrancaAsaaS.Root dcobranca = new DadosCobrancaAsaaS.Root()
                {
                    customer = sIDCliente.ToString(),
                    billingType = "PIX",
                    value = Funcoes.strToDouble(txtValor.Text.ToString()),
                    dueDate = Convert.ToDateTime(txtVencimento.Text.ToString()).Year.ToString() + "-" + Convert.ToDateTime(txtVencimento.Text.ToString()).Month.ToString() + "-" + Convert.ToDateTime(txtVencimento.Text.ToString()).Day.ToString(),
                    daysAfterDueDateToRegistrationCancellation = Funcoes.strToInt(txtDias.Text.ToString()),
                    description = txtDescricao.Text.ToString(),
                    externalReference = txtReferencia.Text.ToString()
                };

                string jsonCobranca = JsonConvert.SerializeObject(dcobranca);
                jsonRetorno = asaas.CriarCobranca(asaas.PegarTokenSubconta(Funcoes.strToInt(HttpContext.Current.Session["PESSOA"].ToString()), Funcoes.strToInt(HttpContext.Current.Session["LICENCIADO"].ToString())), jsonCobranca);

                if (jsonRetorno.ToString().Trim() != "")
                {
                    JObject oCobranca = JObject.Parse(jsonRetorno.ToString());
                    try
                    {
                        GravaCobranca(sIDCliente.ToString(), oCobranca["id"].ToString(), oCobranca.ToString(), oCobranca["status"].ToString(), "Receber por QRCode","A");
                        string jsonQRCodeCobranca = asaas.ObterQRCodeCobranca(asaas.PegarTokenSubconta(Funcoes.strToInt(HttpContext.Current.Session["PESSOA"].ToString()), Funcoes.strToInt(HttpContext.Current.Session["LICENCIADO"].ToString())), oCobranca["id"].ToString());

                        if (jsonQRCodeCobranca.ToString().Trim() != "")
                        {
                            try
                            {
                                divQRCode.Visible = true;
                                JObject oQRCodeCobranca = JObject.Parse(jsonQRCodeCobranca.ToString());
                                lblPixCopiaCola.Text = oQRCodeCobranca["payload"].ToString();
                                string base64String = oQRCodeCobranca["encodedImage"].ToString(); //encodedImage
                                QRCodeEncoder encoder = new QRCodeEncoder();
                                Bitmap image = encoder.Encode(oQRCodeCobranca["payload"].ToString());
                                image.Save(Server.MapPath("public_html") + "\\" + oCobranca["id"].ToString() + ".bmp");
                                imgQRcode.Src = "../public_html/" + oCobranca["id"].ToString() + ".bmp";
                            }
                            catch
                            {

                            }

                        }

                        string sURLBoleto = "";
                        try
                        {
                            sURLBoleto = "https://" + Request.ServerVariables["SERVER_NAME"].ToString() + "/linkpagamento.aspx?id=" + Funcoes.Encrypt(oCobranca["id"].ToString()) + "&tipo=" + Funcoes.Encrypt("Q");
                        }
                        catch
                        {
                            string sErrosCobranca = "";
                            for (int i = 0; i < oCobranca["errors"].Count(); i++)
                            {
                                sErrosCobranca = sErrosCobranca + " ( " + oCobranca["errors"][i]["description"].ToString() + " ) ";
                            }
                            ClientScript.RegisterStartupScript(this.GetType(), "Mensagensdeerro", "alert('Ocorreu um erro ao tentar gerar QRCode: " + sErrosCobranca + "');", true);
                        }

                        txtURLQRCode.Text = sURLBoleto.ToString();
                        hrfQRCode.HRef = sURLBoleto.ToString();

                        if (ckbEmailQRCode.Checked == true)
                        {
                            EnviarLinkBoletoEmail(sURLBoleto.ToString());
                        }
                        btnContinuarPixQRCode.Visible = false;
                        btnVoltarPixQRCode.Visible = true;
                        ClientScript.RegisterStartupScript(this.GetType(), "SucessoQRCODE", "alert('Cobrança por QRCode gerada com sucesso!');", true);
                    }
                    catch
                    {
                        string sErrosCobranca = "";
                        for (int i = 0; i < oCobranca["errors"].Count(); i++)
                        {
                            sErrosCobranca = sErrosCobranca + " ( " + oCobranca["errors"][i]["description"].ToString() + " ) ";
                        }
                        ClientScript.RegisterStartupScript(this.GetType(), "Mensagensdeerro", "alert('Ocorreu um erro ao tentar gerar o QRCode: " + sErrosCobranca + "');", true);
                    }
                }
                else
                {
                    ClientScript.RegisterStartupScript(this.GetType(), "ErroVendaDireta", "alert('Ocorreu um erro ao tentar gerar o QRCode! Verifique e tente novamente.');", true);
                }

            }
            else
            {
                ClientScript.RegisterStartupScript(this.GetType(), "ErroVendaDiretaGeral", "alert('Não foi possível gerar o QRCode! Verifique e tente novamente.');", true);
            }

        }
        
    }

    private void CobrancaZoop(string sTipo)
    {
        string jsonRetorno = "";
    }

    private void GravaCliente(string sIDCliente)
    {
        SqlConnection connInsConsCliente = new SqlConnection(Funcoes.conexao());
        connInsConsCliente.Open();
        SqlCommand cmdInsConsCliente = new SqlCommand("dbo.stp_compradores_ins", connInsConsCliente);
        cmdInsConsCliente.CommandType = CommandType.StoredProcedure;
        cmdInsConsCliente.Parameters.Add("@flg_operacao", SqlDbType.Char).Value = 'I';
        cmdInsConsCliente.Parameters.Add("@FLG_ATIVO", SqlDbType.Char).Value = "S";
        cmdInsConsCliente.Parameters.Add("@COD_ID_PESSOA_LICENCIADO", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["LICENCIADO"].ToString());
        cmdInsConsCliente.Parameters.Add("@COD_ID_PESSOAS_FJ", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["PESSOA"].ToString());
        cmdInsConsCliente.Parameters.Add("@COD_ID_COMPRADOR", SqlDbType.VarChar).Value = sIDCliente.ToString();
        cmdInsConsCliente.Parameters.Add("@DTA_DATA", SqlDbType.DateTime).Value = DateTime.Now;
        cmdInsConsCliente.Parameters.Add("@FLG_TIPO", SqlDbType.Char).Value = "C";
        cmdInsConsCliente.Parameters.Add("@NOM_NOME", SqlDbType.VarChar).Value = txtNome.Text.ToString();
        cmdInsConsCliente.Parameters.Add("@NOM_CEP", SqlDbType.VarChar).Value = txtCEP.Text.ToString();
        cmdInsConsCliente.Parameters.Add("@NOM_NUMERO", SqlDbType.VarChar).Value = txtNumero.Text.ToString();
        cmdInsConsCliente.Parameters.Add("@NOM_CELULAR", SqlDbType.VarChar).Value = txtCelular.Text.ToString();
        cmdInsConsCliente.Parameters.Add("@NUM_CNPJCPF", SqlDbType.VarChar).Value = Funcoes.TIRAACENTOSDOCUMENTOS(txtCNPJCPF.Text.ToString());
        cmdInsConsCliente.Parameters.Add("@NOM_EMAIL", SqlDbType.VarChar).Value = txtEmail.Text.ToString();

        cmdInsConsCliente.ExecuteNonQuery();
        connInsConsCliente.Close();
        connInsConsCliente.Dispose();
    }

    private void CarregaDadosCliente()
    {
        SqlConnection mySelCadastro = new SqlConnection(Funcoes.conexao());
        mySelCadastro.Open();
        SqlCommand cmdSelCadastro = new SqlCommand("dbo.stp_compradores_ins", mySelCadastro);
        cmdSelCadastro.CommandType = CommandType.StoredProcedure;
        cmdSelCadastro.Parameters.Add("@flg_operacao", SqlDbType.Char).Value = "S";
        cmdSelCadastro.Parameters.Add("@COD_ID", SqlDbType.Int).Value = Funcoes.strToInt(ddlClientePadrao.SelectedValue.ToString());
        cmdSelCadastro.Parameters.Add("@COD_ID_PESSOA_LICENCIADO", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["LICENCIADO"].ToString());
        cmdSelCadastro.Parameters.Add("@COD_ID_PESSOAS_FJ", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["PESSOA"].ToString());
        SqlDataReader ReaderCadastro = cmdSelCadastro.ExecuteReader();
        while (ReaderCadastro.Read())
        {
            txtNome.Text = ReaderCadastro["NOM_NOME"].ToString();
            txtCNPJCPF.Text = ReaderCadastro["NUM_CNPJCPF"].ToString();
            txtEmail.Text = ReaderCadastro["NOM_EMAIL"].ToString();
            txtCEP.Text = ReaderCadastro["NOM_CEP"].ToString();
            txtCelular.Text = ReaderCadastro["NOM_CELULAR"].ToString();
            txtNumero.Text = ReaderCadastro["NOM_NUMERO"].ToString();
        }
    }

    private void GravaCobranca(string sIDCliente, string sIDCobranca, string sJsonBoleto, string sStatus, string sTipo, string sOrigem)
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

        cmdInsConsCliente.Parameters.Add("@NOM_NOME", SqlDbType.VarChar).Value = txtNome.Text.ToString();
        cmdInsConsCliente.Parameters.Add("@NUM_CNPJCPF", SqlDbType.VarChar).Value = Funcoes.TIRAACENTOSDOCUMENTOS(txtCNPJCPF.Text.ToString());
        cmdInsConsCliente.Parameters.Add("@NOM_EMAIL", SqlDbType.VarChar).Value = txtEmail.Text.ToString();

        cmdInsConsCliente.Parameters.Add("@DES_DESCRICAO", SqlDbType.Text).Value = txtDescricao.Text.ToString();
        cmdInsConsCliente.Parameters.Add("@NOM_REFERENCIA", SqlDbType.VarChar).Value = txtReferencia.Text.ToString();
        cmdInsConsCliente.Parameters.Add("@DTA_VENCIMENTO", SqlDbType.DateTime).Value = Convert.ToDateTime(txtVencimento.Text.ToString());
        cmdInsConsCliente.Parameters.Add("@NUM_VALOR", SqlDbType.Float).Value = Funcoes.strToDouble(txtValor.Text.ToString());

        cmdInsConsCliente.Parameters.Add("@DES_JSON", SqlDbType.Text).Value = sJsonBoleto.ToString();
        cmdInsConsCliente.Parameters.Add("@FLG_STATUS", SqlDbType.VarChar).Value = sStatus.ToString();

        if (sTipo.ToString().Trim() == "Receber por QRCode")
        {
            cmdInsConsCliente.Parameters.Add("@NOM_URL_BOLETO", SqlDbType.VarChar).Value = "https://" + Request.ServerVariables["SERVER_NAME"].ToString() + "/linkpagamento.aspx?id=" + Funcoes.Encrypt(sIDCobranca.ToString()) + "&tipo=" + Funcoes.Encrypt("Q");
        }

        cmdInsConsCliente.ExecuteNonQuery();
        connInsConsCliente.Close();
        connInsConsCliente.Dispose();
    }
    private void EnviarLinkBoletoEmail(string sURLBoleto)
    {
        try
        {

            
            
            System.Net.Mail.SmtpClient client = new System.Net.Mail.SmtpClient();
            client.Host = HttpContext.Current.Session["EMAILHOST"].ToString();
            if (HttpContext.Current.Session["EMAILSSL"].ToString() == "S")
            { client.EnableSsl = true; }
            else { client.EnableSsl = false; }
            client.Port = Funcoes.strToInt(HttpContext.Current.Session["EMAILPORTA"].ToString());
            //client.Timeout = 0;
            client.Credentials = new System.Net.NetworkCredential(HttpContext.Current.Session["EMAIL"].ToString(), HttpContext.Current.Session["EMAILSENHA"].ToString());
            MailMessage mail = new MailMessage();
            mail.Sender = new System.Net.Mail.MailAddress(HttpContext.Current.Session["EMAIL"].ToString(), "");
            mail.From = new MailAddress(HttpContext.Current.Session["EMAIL"].ToString(), "");

            mail.To.Add(new MailAddress(txtEmail.Text.ToString(), txtEmail.Text.ToString()));

            mail.Bcc.Add(new MailAddress("adriano@webview.com.br", "Adriano - WEBVIEW"));
            mail.Bcc.Add(new MailAddress("admin@legacybank.com.br", "Administrador LEGACY"));

            mail.Subject = "Link de pagamento - " + HttpContext.Current.Session["URLORIGEM"].ToString();
            mail.SubjectEncoding = System.Text.Encoding.UTF8;

            // Construção do CORPO da MENSAGEM (Body)
            mail.Body = "";
            mail.Body = mail.Body + "<b>Olá,</b> " + txtNome.Text.ToString() + "<br /><br />";
            mail.Body = mail.Body + "Segue abaixo o link de pagamento referente:<br />";
            mail.Body = mail.Body + "" + txtDescricao.Text.ToString() + "<br />";
            mail.Body = mail.Body + "<b>Para visualizar o link de pagamento basta clicar no link abaixo<br /><br>";
            mail.Body = mail.Body + "<a href='" + sURLBoleto.ToString() + "' target='_blank'>Clique aqui para visualizar o link de pagamento</a><br /><br />";
            mail.Body = mail.Body + HttpContext.Current.Session["EMAILRODAPE"].ToString();

            mail.BodyEncoding = System.Text.Encoding.UTF8;
            mail.IsBodyHtml = true;
            mail.Priority = MailPriority.High;

            try
            {
                client.Send(mail);
                ClientScript.RegisterStartupScript(this.GetType(),"Codigo2fa", "alert('O e-mail da cobrança foi enviado com sucesso!');", true);
                mail = null;
            }
            catch (System.Exception erro)
            {
                ClientScript.RegisterStartupScript(this.GetType(),"Erro2fa", "alert('Ocorreu um erro ao enviar!');", true);
            }
        }
        catch
        {
            ClientScript.RegisterStartupScript(this.GetType(), "Codigo2faErro", "alert('Ocorreu um erro ao tentar enviar o e-mail da cobrança!');", true);

        }

        //ClientScript.RegisterStartupScript(this.GetType(), "QUALEMAIL", "alert('e-mail:" + HttpContext.Current.Session["EMAILHOST"].ToString() + "!');", true);

    }

    protected void ddlClientePadrao_SelectedIndexChanged(object sender, System.EventArgs e)
    {
        CarregaDadosCliente();
    }


    private void CriarTransferencias()
    {
        
            if (Funcoes.strToDouble(txtValor.Text.ToString()) <= Funcoes.strToDouble(lblSaldo.Text.ToString()))
            {
                if (Funcoes.strToDouble(txtValor.Text.ToString()) >= 10)
                {
                    Funcoes.GravaAuditoria(0, Request.ServerVariables["REMOTE_ADDR"].ToString(), "Transferência PIX", "Confirmar Transferência: " + txtChavePix.Text.ToString());

                    dadosTransferenciaPixAsaaS.Root dTransferencia = new dadosTransferenciaPixAsaaS.Root()
                    {
                        value = Funcoes.strToDouble(txtValor.Text.ToString()),
                        operationType = "PIX",
                        pixAddressKey = txtChavePixTransferencia.Text.ToString(),
                        pixAddressKeyType = ddlTipoPIX.SelectedValue.ToString(),
                        description = txtDescricao.Text.ToString(),
                        scheduleDate = Convert.ToDateTime(txtVencimento.Text.ToString())
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
                                GravaCobranca("", oTransferencia["id"].ToString(), jsonTransferencia.ToString(), oTransferencia["status"].ToString(),  "Transferência PIX","A");

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
    protected void btnConfirmarSenha_Click(object sender, System.EventArgs e)
    {
        if (HttpContext.Current.Session["SENHA"].ToString().Trim() == Funcoes.Encrypt(txtSenha.Text.ToString()))
        {
            CriarTransferencias();
        }
        else
        {
            ClientScript.RegisterStartupScript(this.GetType(),
                "CodigoErrado2fa", "alert('Senha inválida! Verifique e reentre.');", true);
        }


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