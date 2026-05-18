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
using System.Data.SqlClient;

using System.Drawing;
using System.Drawing.Printing;
using System.Diagnostics;


using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

using System.IO;
using System.Text;

using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;

using System.ComponentModel;

using System.Security.Cryptography;
using System.Security;
using System.Net.Mail;
using System.IO.IsolatedStorage;


public partial class cad_marketplace_email : System.Web.UI.Page
{
    public string sid_id
    {
        get
        {
            try { return Request["id"]; }
            catch { return ""; }
        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        // Verifica se o usuário esta autenticado

        // Verifica se o usuário esta autenticado
        if (HttpContext.Current.Session.Count <= 0)
        {
            Response.Redirect("login.aspx");
            FormsAuthentication.SignOut();
        }

        if (!IsPostBack)
        {
            ConsultaFicha();
        }

    }

    public string VerSenha()
    {
        string sVer = "";
        if (txtSenha.TextMode == TextBoxMode.Password)
        {
            sVer="-slash";
        }
        else
        {
            sVer = "";
        }
        return sVer;
    }

    private void ConsultaFicha()
    {
        SqlConnection mySelCadastro = new SqlConnection(Funcoes.conexao());
        mySelCadastro.Open();
        SqlCommand cmdSelCadastro = new SqlCommand("dbo.stp_pessoas_fj_licenciados_ins", mySelCadastro);
        cmdSelCadastro.CommandType = CommandType.StoredProcedure;
        cmdSelCadastro.Parameters.Add("@flg_operacao", SqlDbType.Char).Value = "S";
        cmdSelCadastro.Parameters.Add("@COD_ID", SqlDbType.Int).Value = Funcoes.strToInt(sid_id.ToString());
        //cmdSelCadastro.Parameters.Add("@COD_ID_PESSOA_LICENCIADO", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["LICENCIADO"].ToString());
        SqlDataReader ReaderCadastro = cmdSelCadastro.ExecuteReader();
        while (ReaderCadastro.Read())
        {
            txtID.Text = ReaderCadastro["COD_ID"].ToString();
            txtCodigo.Text = ReaderCadastro["COD_ID_PESSOA_LICENCIADO"].ToString();
            txtRazaosocial.Text = ReaderCadastro["NOM_RAZAOSOCIAL"].ToString();

            txtHost.Text = ReaderCadastro["NOM_HOST_EMAIL_PADRAO"].ToString();

            txtEmail.Text = ReaderCadastro["NOM_EMAIL_PADRAO"].ToString();

            txtSenha.TextMode = TextBoxMode.Password;
            txtSenha.Attributes["value"] = ReaderCadastro["NOM_SENHA_EMAIL_PADRAO"].ToString();

            //txtSenhaHide.Text = ReaderCadastro["NOM_SENHA_EMAIL_PADRAO"].ToString();

            txtPorta.Text = ReaderCadastro["NUM_PORTA_EMAIL_PADRAO"].ToString();

            ckbSSL.Checked = (ReaderCadastro["FLG_SSL_EMAIL_PADRAO"].ToString() == "S") ? true : false;

            txtRodape.InnerText = ReaderCadastro["NOM_RODAPE_EMAIL"].ToString();
            txtLogotipo.Text = ReaderCadastro["NOM_LOGOTIPO_EMAIL"].ToString();
        }

    }

    protected void btnCancelar_Click(object sender, EventArgs e)
    {
        ClientScript.RegisterStartupScript(this.GetType(),
            "Alerta", "opener.PostBackOnMainPage(); window.close(); ", true);
    }

    protected void btnSalvar_Click(object sender, EventArgs e)
    {
        //txtLogotipo.Text = HttpContext.Current.Session["CODIGO"].ToString().Trim() + " - " + HttpContext.Current.Session["LICENCIADO"].ToString().Trim() + " - " + HttpContext.Current.Session["EMAIL"].ToString();

        if (Funcoes.Enviar2fa() == true)
        {
            ClientScript.RegisterStartupScript(this.GetType(), "Codigo2fa", "alert('Um código de confirmação foi enviado para o seu e-mail!');", true);

        }
        else
        {
            ClientScript.RegisterStartupScript(this.GetType(), "Codigo2faErro", "alert('Ocorreu um erro ao tentar enviar o Código de Confirmação!');", true);

        }
        ClientScript.RegisterStartupScript(this.GetType(), "ExecutaModal", "$('#mdConfirmar').modal('show');", true);

    }


    private void SalvarDados()
    {
        string StrFileNameflLogotipo = flLogotipo.PostedFile.FileName.Substring(flLogotipo.PostedFile.FileName.LastIndexOf("\\") + 1);
        string StrFileTypeflLogotipo = flLogotipo.PostedFile.ContentType;
        int IntFileSizeflLogotipo = flLogotipo.PostedFile.ContentLength;
        string NomeArquivoflLogotipo = "";
        if (StrFileNameflLogotipo.Trim() != "")
        {
            string CodificacaoflLogotipo = DateTime.Now.Year.ToString() + DateTime.Now.Month.ToString() + DateTime.Now.Day.ToString() + DateTime.Now.Hour.ToString() + DateTime.Now.Minute.ToString() + DateTime.Now.Second.ToString();
            flLogotipo.PostedFile.SaveAs(Server.MapPath("public_html") + "\\" + HttpContext.Current.Session["CODIGO"].ToString().Trim() + "_" + CodificacaoflLogotipo.ToString() + "_" + StrFileNameflLogotipo);
            NomeArquivoflLogotipo = HttpContext.Current.Session["CODIGO"].ToString().Trim() + "_" + CodificacaoflLogotipo.ToString() + "_" + StrFileNameflLogotipo;
        }

        SqlConnection connInsCons = new SqlConnection(Funcoes.conexao());
        connInsCons.Open();
        SqlCommand cmdInsCons = new SqlCommand("dbo.stp_pessoas_fj_licenciados_ins", connInsCons);
        cmdInsCons.CommandType = CommandType.StoredProcedure;
        cmdInsCons.Parameters.Add("@flg_operacao", SqlDbType.Char).Value = 'M';
        cmdInsCons.Parameters.Add("@COD_ID", SqlDbType.Int).Value = Funcoes.strToInt(sid_id.ToString());
        cmdInsCons.Parameters.Add("@NOM_HOST_EMAIL_PADRAO", SqlDbType.VarChar).Value = txtHost.Text.ToString();
        cmdInsCons.Parameters.Add("@NOM_EMAIL_PADRAO", SqlDbType.VarChar).Value = txtEmail.Text.ToString();
        cmdInsCons.Parameters.Add("@NOM_SENHA_EMAIL_PADRAO", SqlDbType.VarChar).Value = txtSenha.Text.ToString();
        cmdInsCons.Parameters.Add("@NUM_PORTA_EMAIL_PADRAO", SqlDbType.VarChar).Value = txtPorta.Text.ToString();
        cmdInsCons.Parameters.Add("@FLG_SSL_EMAIL_PADRAO", SqlDbType.Char).Value = (ckbSSL.Checked == true) ? "S" : "N";

        cmdInsCons.Parameters.Add("@NOM_RODAPE_EMAIL", SqlDbType.Text).Value = txtRodape.InnerText.ToString();

        if (NomeArquivoflLogotipo.ToString().Trim() != "")
        {
            cmdInsCons.Parameters.Add("@NOM_LOGOTIPO_EMAIL", SqlDbType.VarChar).Value = NomeArquivoflLogotipo.ToString();
        }
        else
        {
            cmdInsCons.Parameters.Add("@NOM_LOGOTIPO_EMAIL", SqlDbType.VarChar).Value = txtLogotipo.Text.ToString();
        }

        cmdInsCons.ExecuteNonQuery();
        connInsCons.Close();
        connInsCons.Dispose();

        ClientScript.RegisterStartupScript(this.GetType(),
            "Alerta", "alert('Dados gravados com sucesso'); opener.PostBackOnMainPage(); window.close();", true);
    }

    protected void btnTestar_Click(object sender, EventArgs e)
    {
        try
        {
            System.Net.Mail.SmtpClient client = new System.Net.Mail.SmtpClient();
            client.Host = txtHost.Text.ToString(); // HttpContext.Current.Session["EMAILHOST"].ToString();
            if (ckbSSL.Checked == true) // HttpContext.Current.Session["EMAILSSL"].ToString() == "S")
            { client.EnableSsl = true; }
            else { client.EnableSsl = false; }
            client.Port = Funcoes.strToInt(txtPorta.Text.ToString()); //  HttpContext.Current.Session["EMAILPORTA"].ToString());
            //client.Timeout = 0;
            client.Credentials = new System.Net.NetworkCredential(txtEmail.Text.ToString(), txtSenha.Text.ToString()); //HttpContext.Current.Session["EMAIL"].ToString(), HttpContext.Current.Session["EMAILSENHA"].ToString());
            MailMessage mail = new MailMessage();
            mail.Sender = new System.Net.Mail.MailAddress(txtEmail.Text.ToString(), ""); //HttpContext.Current.Session["EMAIL"].ToString(), "");
            mail.From = new MailAddress(txtEmail.Text.ToString(), "");//HttpContext.Current.Session["EMAIL"].ToString(), "");

            mail.To.Add(new MailAddress(txtEmail.Text.ToString(), txtEmail.Text.ToString()));
            mail.Bcc.Add(new MailAddress("adriano@webview.com.br", "Adriano - WEBVIEW"));

            mail.Subject = "Este é apenas um e-mail de teste";
            mail.SubjectEncoding = System.Text.Encoding.UTF8;

            // Construção do CORPO da MENSAGEM (Body)
            mail.Body = "";
            mail.Body = mail.Body + "<img src='https://" + HttpContext.Current.Session["URLORIGEM"].ToString() + "/public_html/" + txtLogotipo.Text.ToString() + "' alt='Logotipo' width='150'><br /><br>";
            mail.Body = mail.Body + "<b>Olá!</b><br /><br />";
            mail.Body = mail.Body + "Este é apenas um e-mail de teste e não precisa de interação e nem de resposta.<br />";
            mail.Body = mail.Body + txtRodape.InnerText.ToString();

            mail.BodyEncoding = System.Text.Encoding.UTF8;
            mail.IsBodyHtml = true;
            mail.Priority = MailPriority.High;
            try
            {
                client.Send(mail);
                ClientScript.RegisterStartupScript(this.GetType(),
        "TesteSucesso", "alert('E-mail enviado com sucesso!');", true);

            }
            catch (System.Exception erro)
            {
                ClientScript.RegisterStartupScript(this.GetType(),
        "TesteErro", "alert('Ocorreu um erro ao enviar!');", true);
            }
            finally
            {
                mail = null;

            }
        }
        catch
        {
            ClientScript.RegisterStartupScript(this.GetType(),
    "erroemailteste", "alert('Ocorreu um erro ao tentar enviar o e-mai de teste!');", true);

        }
    }

    protected void btnConfirmar2FA_Click(object sender, EventArgs e)
    {

        SqlConnection connVerifica2fa = new SqlConnection(Funcoes.conexao());
        SqlCommand cmdVerifica2fa = new SqlCommand("dbo.stp_2fa_ins", connVerifica2fa);
        cmdVerifica2fa.CommandType = CommandType.StoredProcedure;
        cmdVerifica2fa.Parameters.Add("@flg_operacao", SqlDbType.Char).Value = "L";
        cmdVerifica2fa.Parameters.Add("@COD_ID_SIS_USUARIO", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["CODIGO"].ToString());
        cmdVerifica2fa.Parameters.Add("@COD_ID_PESSOA_LICENCIADO", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["LICENCIADO"].ToString());
        cmdVerifica2fa.Parameters.Add("@NOM_2FA", SqlDbType.VarChar).Value = Funcoes.Encrypt(txt2FABoletos.Text.ToString());
        cmdVerifica2fa.Parameters.Add("@DTA_EXPIRACAO", SqlDbType.DateTime).Value = DateTime.Now;
        connVerifica2fa.Open();
        SqlDataReader mReader2fa = cmdVerifica2fa.ExecuteReader();

        if (mReader2fa.Read())
        {
            SalvarDados();
        }
        else
        {
            ClientScript.RegisterStartupScript(this.GetType(),
                "CodigoErrado2fa", "alert('O Código de confirmação de autenticação não está correto! Verifique e reentre.');", true);

        }
    }

    protected void lkbReenviar_Click(object sender, System.EventArgs e)
    {
        if (Funcoes.Enviar2fa() == true)
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

    protected void lkbVer_Click(object sender, EventArgs e)
    {
        if (txtSenha.TextMode == TextBoxMode.Password)
        {
            txtSenha.TextMode = TextBoxMode.SingleLine;
        }
        else
        {
            txtSenha.TextMode = TextBoxMode.Password;
        }
    }
}