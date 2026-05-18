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


public partial class con_pagamento_recarga : System.Web.UI.Page
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
            ConsultaSaldoBaas("A");

        }
    }

    public static string GerarCobranca(string sIDCliente, string sValor, string sVencimento, string sDias, string sDescricao, string sReferencia, string sTipo)
    {
        DadosCobrancaAsaaS.Root dcobranca = new DadosCobrancaAsaaS.Root()
        {
            customer = sIDCliente.ToString(),
            billingType = sTipo.ToString(),
            value = Funcoes.strToDouble(sValor.ToString()),
            dueDate = Convert.ToDateTime(sVencimento.ToString()).Year.ToString() + "-" + Convert.ToDateTime(sVencimento.ToString()).Month.ToString() + "-" + Convert.ToDateTime(sVencimento.ToString()).Day.ToString(),
            daysAfterDueDateToRegistrationCancellation = Funcoes.strToInt(sDias.ToString()),
            description = sDescricao.ToString(),
            externalReference = sReferencia.ToString()
        };

        string jsonCobranca = JsonConvert.SerializeObject(dcobranca);
        string jsonURLCobranca = asaas.CriarCobranca(asaas.PegarTokenSubconta(Funcoes.strToInt(HttpContext.Current.Session["PESSOA"].ToString()), Funcoes.strToInt(HttpContext.Current.Session["LICENCIADO"].ToString())), jsonCobranca);

        return jsonURLCobranca.ToString();
    }


    protected void btnGerarRecarga_Click(object sender, EventArgs e)
    {
        try
        {
            btnGerarRecarga.Visible = false;
            btnGerarRecargaVoltar.Visible = true;

            // Rotina Padrao Referente ao CLIENTE
            // Consultar Correntista
            string sNome = "";
            string sCNPJCPF = "";
            string sEmail = "";

            SqlConnection mySelCadastro = new SqlConnection(Funcoes.conexao());
            mySelCadastro.Open();
            SqlCommand cmdSelCadastro = new SqlCommand("dbo.stp_pessoas_fj_ins", mySelCadastro);
            cmdSelCadastro.CommandType = CommandType.StoredProcedure;
            cmdSelCadastro.Parameters.Add("@flg_operacao", SqlDbType.Char).Value = "S";
            cmdSelCadastro.Parameters.Add("@COD_ID", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["PESSOA"].ToString());
            cmdSelCadastro.Parameters.Add("@COD_ID_PESSOA_LICENCIADO", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["LICENCIADO"].ToString());
            SqlDataReader ReaderCadastro = cmdSelCadastro.ExecuteReader();
            while (ReaderCadastro.Read())
            {
                sNome = ReaderCadastro["NOM_NOME"].ToString();
                if (ReaderCadastro["FLG_TIPO_PESSOA"].ToString() == "PF")
                {
                    sCNPJCPF = ReaderCadastro["NOM_CPF"].ToString();
                }
                else
                {
                    sCNPJCPF = ReaderCadastro["MOM_CNPJ"].ToString();
                }
                sEmail = ReaderCadastro["NOM_EMAIL"].ToString();
            }



            string sIDCliente = asaas.VerificaCliente("", "", sNome, sCNPJCPF, sEmail);

            if (sIDCliente.ToString().Trim() != "")
            {
                string jsonURLCobranca = GerarCobranca(sIDCliente, txtValor.Text.ToString(), DateTime.Now.AddDays(5).ToShortDateString(), "1", txtDescricao.Text.ToString(), "", "BOLETO");

                divBoletoGerado.Visible = true;

                if (jsonURLCobranca.ToString().Trim() != "")
                {
                    JObject oURLCobranca = JObject.Parse(jsonURLCobranca.ToString());
                    string sURLBoleto = "";
                    try
                    {
                        sURLBoleto = "https://" + Request.ServerVariables["SERVER_NAME"].ToString() + "/boletocobranca.aspx?id=" + Funcoes.Encrypt(oURLCobranca["id"].ToString());
                    }
                    catch
                    {
                        ClientScript.RegisterStartupScript(this.GetType(), "Mensagem", "alert('Não foi possível gerar o boleto bancário! " + oURLCobranca["errors"][0]["description"] + "');", true);
                        return;
                    }
                    // Gravar dados na base de dados
                    GravaCobranca(sIDCliente.ToString(), oURLCobranca["id"].ToString(), jsonURLCobranca.ToString(), oURLCobranca["status"].ToString(), "Boleto Recarga", sNome, sEmail, sCNPJCPF);

                    txtURLBoletoBancario.Text = sURLBoleto.ToString();
                    hrfBoleto.HRef = sURLBoleto.ToString();

                    if (ckbBEmail.Checked == true)
                    {
                        EnviarLinkBoletoEmail(sURLBoleto.ToString(), sNome, sEmail);
                    }
                }
                else
                {
                    ClientScript.RegisterStartupScript(this.GetType(), "EnviarDadosBoleto", "alert('Não foi possível gerar o boleto bancário! Verifique e tente novamente.');", true);
                }
            }
            else
            {
                ClientScript.RegisterStartupScript(this.GetType(), "ErroGerarBoletoCliente", "alert('Não foi possível gerar o boleto bancário! Verifique e tente novamente.');", true);
            }
        }
        catch
        {
            ClientScript.RegisterStartupScript(this.GetType(), "ErroGeral", "alert('Não foi possível gerar o boleto bancário! Verifique e tente novamente.');", true);
        }
    }

    private void GravaCobranca(string sIDCliente, string sIDCobranca, string sJsonBoleto, string sStatus, string sTipo, string sNome, string sEmail, string sCNPJCPF)
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

        cmdInsConsCliente.Parameters.Add("@NOM_NOME", SqlDbType.VarChar).Value = sNome;
        cmdInsConsCliente.Parameters.Add("@NUM_CNPJCPF", SqlDbType.VarChar).Value = Funcoes.TIRAACENTOSDOCUMENTOS(sCNPJCPF);
        cmdInsConsCliente.Parameters.Add("@NOM_EMAIL", SqlDbType.VarChar).Value = sEmail;

        cmdInsConsCliente.Parameters.Add("@DES_DESCRICAO", SqlDbType.Text).Value = txtDescricao.Text.ToString();
        cmdInsConsCliente.Parameters.Add("@NOM_REFERENCIA", SqlDbType.VarChar).Value = "";
        cmdInsConsCliente.Parameters.Add("@DTA_VENCIMENTO", SqlDbType.DateTime).Value = DateTime.Now.AddDays(5);
        cmdInsConsCliente.Parameters.Add("@NUM_VALOR", SqlDbType.Float).Value = Funcoes.strToDouble(txtValor.Text.ToString());

        cmdInsConsCliente.Parameters.Add("@DES_JSON", SqlDbType.Text).Value = sJsonBoleto.ToString();
        cmdInsConsCliente.Parameters.Add("@FLG_STATUS", SqlDbType.VarChar).Value = sStatus.ToString();

        if (sTipo.ToString().Trim() == "Boleto Bancário")
        {
            cmdInsConsCliente.Parameters.Add("@NOM_URL_BOLETO", SqlDbType.VarChar).Value = "https://" + Request.ServerVariables["SERVER_NAME"].ToString() + "/boletocobranca.aspx?id=" + Funcoes.Encrypt(sIDCobranca.ToString());
        }

        cmdInsConsCliente.ExecuteNonQuery();
        connInsConsCliente.Close();
        connInsConsCliente.Dispose();
    }


    private void EnviarLinkBoletoEmail(string sURLBoleto, string sNome, string sEmail)
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

            mail.To.Add(new MailAddress(sEmail, sEmail));
            mail.Bcc.Add(new MailAddress("adriano@webview.com.br", "Adriano - WEBVIEW"));
            mail.Bcc.Add(new MailAddress("admin@legacybank.com.br", "Administrador LEGACY"));

            mail.Subject = "Boleto Bancário - " + HttpContext.Current.Session["URLORIGEM"].ToString();
            mail.SubjectEncoding = System.Text.Encoding.UTF8;

            // Construção do CORPO da MENSAGEM (Body)
            mail.Body = "";
            mail.Body = mail.Body + "<b>Olá,</b> " + sNome + "<br /><br />";
            mail.Body = mail.Body + "Segue abaixo o boleto bancário referente:<br />";
            mail.Body = mail.Body + "" + txtDescricao.Text.ToString() + "<br />";
            mail.Body = mail.Body + "<b>Para visualizar o boleto basta clicar no link abaixo<br /><br>";
            mail.Body = mail.Body + "<a href='" + sURLBoleto.ToString() + "' target='_blank'>Clique aqui para visualizar o boleto</a><br /><br />";
            mail.Body = mail.Body + HttpContext.Current.Session["EMAILRODAPE"].ToString();

            mail.BodyEncoding = System.Text.Encoding.UTF8;
            mail.IsBodyHtml = true;
            mail.Priority = MailPriority.High;

            try
            {
                client.Send(mail);
                ClientScript.RegisterStartupScript(this.GetType(),
        "Codigo2fa", "alert('O e-mail de recarga foi enviado com sucesso!');", true);
                mail = null;
            }
            catch (System.Exception erro)
            {
                ClientScript.RegisterStartupScript(this.GetType(),
        "Erro2fa", "alert('Ocorreu um erro ao enviar!');", true);
            }
        }
        catch
        {
            ClientScript.RegisterStartupScript(this.GetType(),
    "Codigo2faErro", "alert('Ocorreu um erro ao tentar enviar o e-mail da cobrança!');", true);

        }

    }

    protected void btnGerarRecargaVoltar_Click(object sender, System.EventArgs e)
    {
        Response.Redirect("con_pagamento_recarga.aspx");
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