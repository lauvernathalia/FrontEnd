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
using System.Security.Cryptography;
using System.Net.Sockets;
using System.IO;
using System.Net;

using System.Net.Mail;
using System.Net.Mime;
using System.Net.Configuration;
using System.Net;

using System.Collections.Generic;
using System.Management;

public partial class login : System.Web.UI.Page
{
    public string Recaptcha { get; set; }

    protected void Page_Load(object sender, EventArgs e)
    {


        //lblTokenAcesso.Text = Funcoes.TokenValidator.GetCurrentToken();

        string urlorigem = Request.ServerVariables["SERVER_NAME"].ToString();
        string Nomeorigem = Request.ServerVariables["REMOTE_HOST"].ToString();
        string IPOrigem = Request.ServerVariables["HTTP_ACCEPT_LANGUAGE"].ToString();
        string UsuarioOrigem = Request.ServerVariables["HTTP_USER_AGENT"].ToString();
        //string CoresOrigem = Request.ServerVariables["HTTP_UA_COLOR"].ToString();
        //string PixelsOrigem = Request.ServerVariables["HTTP_UA_PIXELS"].ToString();
        //lbldados.Text = Nomeorigem + " - " + IPOrigem + " - " + UsuarioOrigem + " - " + CoresOrigem + " - " + PixelsOrigem;

        dvTipo.Visible = false;

        imgLogoPrincipal.ImageUrl = "images/logo-colorida.png";
        SqlConnection mySelCadastro = new SqlConnection(Funcoes.conexao());
        mySelCadastro.Open();
        SqlCommand cmdSelCadastro = new SqlCommand("dbo.stp_pessoas_fj_ins", mySelCadastro);
        cmdSelCadastro.CommandType = CommandType.StoredProcedure;
        cmdSelCadastro.Parameters.Add("@flg_operacao", SqlDbType.Char).Value = "T";
        cmdSelCadastro.Parameters.Add("@NOM_URL", SqlDbType.VarChar).Value = urlorigem.ToString();
        SqlDataReader ReaderCadastro = cmdSelCadastro.ExecuteReader();
        while (ReaderCadastro.Read())
        {
            HttpContext.Current.Session.Add("LICENCIADO", ReaderCadastro["COD_ID_PESSOA_LICENCIADO"].ToString());

            imgLogoPrincipal.ImageUrl = "public_html/" + ReaderCadastro["NOM_LOGO"].ToString();
            imgDestaque.ImageUrl = "public_html/" + ReaderCadastro["NOM_LOGO_MENU"].ToString();
            if (ReaderCadastro["NOM_LINK_QRCODE"].ToString().Trim() != "")
            {
                imgQRCode.ImageUrl = "public_html/" + ReaderCadastro["NOM_LINK_QRCODE"].ToString();
            }
            else
            {
                imgQRCode.ImageUrl = "images/qrcode_legacybank_site.png";
            }
            //lblSlogan.Text = ReaderCadastro["NOM_SLOGAN"].ToString();

            Page.Title = ReaderCadastro["NOM_TITULO_URL"].ToString();
            //Page.Header.Title = ReaderCadastro["NOM_RAZAOSOCIAL"].ToString();
            FavIconLink.Text = "<link rel='icon' type='image/x-icon' href='" + "public_html/" + ReaderCadastro["NOM_FAVICON"].ToString() + "'>";
            if (ReaderCadastro["NOM_IMAGEM_FUNDO"].ToString().Trim() != "")
            {
                bdyLogin.Attributes.Add("style", "background: url('public_html/" + ReaderCadastro["NOM_IMAGEM_FUNDO"].ToString() + "') no-repeat center center fixed;  -webkit-background-size: cover;  -moz-background-size: cover;  background-size: cover;  -o-background-size: cover;");
            }
            else
            {
                if ((ReaderCadastro["NOM_COR_PRIMARIA_FUNDO"].ToString().Trim() != "") && (ReaderCadastro["NOM_COR_SECUNDARIA_FUNDO"].ToString().Trim() != ""))
                {
                    bdyLogin.Attributes.Add("style", "background: linear-gradient(" + ReaderCadastro["NOM_COR_PRIMARIA_FUNDO"].ToString() + ", " + ReaderCadastro["NOM_COR_SECUNDARIA_FUNDO"].ToString() + ");");
                }
            }
            // 2FA
            HttpContext.Current.Session.Add("AUTENTICACAO2FA", ReaderCadastro["FLG_2FA"].ToString());
            
            // Pegar URL Origem

            HttpContext.Current.Session.Add("URLORIGEM", urlorigem.ToString());

            // Pegar dados do Logotipo

            HttpContext.Current.Session.Add("LOGOPADRAO", ReaderCadastro["NOM_LOGO"].ToString());

            // Pegar os dados ZOOP

            HttpContext.Current.Session.Add("ZOOPINTEGRACAO", ReaderCadastro["FLG_INTEGRACAO_ZOOP"].ToString());
            HttpContext.Current.Session.Add("ZOOPTERCEIROS", ReaderCadastro["FLG_TERCEIROS_ZOOP"].ToString());

            if (ReaderCadastro["NOM_ID_MARKETPLACE"].ToString().Trim() != "")
            { HttpContext.Current.Session.Add("IDMARKETPLACE", ReaderCadastro["NOM_ID_MARKETPLACE"].ToString()); }
            else { HttpContext.Current.Session.Add("IDMARKETPLACE", ConfigurationManager.AppSettings["idzoop"].ToString()); }

            if (ReaderCadastro["NOM_KEY_MARKETPLACE"].ToString().Trim() != "")
            { HttpContext.Current.Session.Add("USERNAMEMARKETPLACE", ReaderCadastro["NOM_KEY_MARKETPLACE"].ToString()); }
            else { HttpContext.Current.Session.Add("USERNAMEMARKETPLACE", ConfigurationManager.AppSettings["keyzoop"].ToString()); }

            // Pegar os dados E-MAIL

            // Dados E-mail Padrao
            HttpContext.Current.Session.Add("EMAILHOST", ReaderCadastro["NOM_HOST_EMAIL_PADRAO"].ToString());
            HttpContext.Current.Session.Add("EMAIL", ReaderCadastro["NOM_EMAIL_PADRAO"].ToString());
            HttpContext.Current.Session.Add("EMAILSENHA", ReaderCadastro["NOM_SENHA_EMAIL_PADRAO"].ToString());
            HttpContext.Current.Session.Add("EMAILPORTA", ReaderCadastro["NUM_PORTA_EMAIL_PADRAO"].ToString());
            HttpContext.Current.Session.Add("EMAILSSL", ReaderCadastro["FLG_SSL_EMAIL_PADRAO"].ToString());
            HttpContext.Current.Session.Add("EMAILRODAPE", ReaderCadastro["NOM_RODAPE_EMAIL"].ToString());
            HttpContext.Current.Session.Add("EMAILLOGOTIPO", ReaderCadastro["NOM_LOGOTIPO_EMAIL"].ToString());
            
            // Dados dos Textos de Aceite
            HttpContext.Current.Session.Add("TERMOSCONDICOS", ReaderCadastro["NOM_TERMOS_CONDICOES_USO"].ToString());
            HttpContext.Current.Session.Add("POLITICAPRIVACIDADE", ReaderCadastro["NOM_POLITICA_PRIVACIDADE"].ToString());

            // CHAT DE SUPORTE
            HttpContext.Current.Session.Add("CHAT", ReaderCadastro["FLG_CHAT"].ToString());

            lblTermo.Text = HttpContext.Current.Session["TERMOSCONDICOS"].ToString();
            lblPolitica.Text = HttpContext.Current.Session["POLITICAPRIVACIDADE"].ToString();

            btnCriarConta.Visible = (ReaderCadastro["FLG_CRIAR_CONTA"].ToString().Trim() == "N") ? false : true;
        
        }

        // Verificar a existência de aceite para o IP 

        SqlConnection mySelAceite = new SqlConnection(Funcoes.conexao());
        mySelAceite.Open();
        SqlCommand cmdSelAceite = new SqlCommand("dbo.stp_aceite_ins", mySelAceite);
        cmdSelAceite.CommandType = CommandType.StoredProcedure;
        cmdSelAceite.Parameters.Add("@flg_operacao", SqlDbType.Char).Value = "S";
        cmdSelAceite.Parameters.Add("@NUM_IP", SqlDbType.VarChar).Value = Request.ServerVariables["REMOTE_ADDR"].ToString();
        SqlDataReader ReaderAceite = cmdSelAceite.ExecuteReader();
        while (ReaderAceite.Read())
        {
            if (ReaderAceite["FLG_ACEITE"].ToString() == "S")
            {
                ckbAceite.Checked = true;
                divAguardandoAceite.Visible = false;
            }
            else
            {
                ckbAceite.Checked = false;
                divAguardandoAceite.Visible = true;
            }

        }

        if (!IsPostBack)
        {
            divBemvindo.Visible = true;
            
            dvLogin.Visible = true;
            dv2FA.Visible = false;
            btnEnviar.Visible = true;
            btnAcessar.Visible = false;
        }


    }
    protected void btnEnviar_Click(object sender, EventArgs e)
    {
        //ClientScript.RegisterStartupScript(this.GetType(),
        //    "Captcha", "alert('" + Request["recaptcha"].ToString() + "');", true);
        //if (Request["recaptcha"].ToString().Length == 0)
        //{
        //    ClientScript.RegisterStartupScript(this.GetType(),
        //        "Captcha", "alert('A validação do reCaptcha falhou. Verifique e reentre!');", true);
        //    return;
        //}
        // Verificar primeiro se existem vários tipos no login

        if (ckbAceite.Checked == true)
        {
            divAguardandoAceite.Visible = false;

            // Grava no arquivo de aceite para verificação posterior
            SqlConnection connInsCons = new SqlConnection(Funcoes.conexao());
            connInsCons.Open();
            SqlCommand cmdInsCons = new SqlCommand("dbo.stp_aceite_ins", connInsCons);
            cmdInsCons.CommandType = CommandType.StoredProcedure;
            cmdInsCons.Parameters.Add("@flg_operacao", SqlDbType.Char).Value = "I";
            cmdInsCons.Parameters.Add("@NUM_IP", SqlDbType.VarChar).Value = Request.ServerVariables["REMOTE_ADDR"].ToString();
            cmdInsCons.Parameters.Add("@FLG_ACEITE", SqlDbType.Char).Value = (ckbAceite.Checked == true) ? "S" : "N";
            cmdInsCons.ExecuteNonQuery();
            connInsCons.Close();
            connInsCons.Dispose();

            //
            if ((txtLogin.Text.ToString().Trim() != "") && (txtSenha.Text.ToString().Trim() != ""))
            {


                SqlConnection connVerificaUsuarioTipo = new SqlConnection(Funcoes.conexao());
                SqlCommand cmdVerificaUsuarioTipo = new SqlCommand("dbo.stp_usuario_sel", connVerificaUsuarioTipo);
                cmdVerificaUsuarioTipo.CommandType = CommandType.StoredProcedure;
                cmdVerificaUsuarioTipo.Parameters.Add("@flg_operacao", SqlDbType.Char).Value = "S";
                cmdVerificaUsuarioTipo.Parameters.Add("@NOM_LOGIN", SqlDbType.VarChar).Value = txtLogin.Text.ToString();
                cmdVerificaUsuarioTipo.Parameters.Add("@NOM_SENHA", SqlDbType.VarChar).Value = txtSenha.Text.ToString();
                connVerificaUsuarioTipo.Open();
                SqlDataReader mReaderTipo = cmdVerificaUsuarioTipo.ExecuteReader();
                int iTipo = 0;
                
                ddlTipo.Items.Clear();
                while (mReaderTipo.Read())
                {
                    ddlTipo.Items.Insert(0, new ListItem(mReaderTipo["NOM_FLG_TIPO"].ToString(), mReaderTipo["COD_ID"].ToString()));
                    iTipo = iTipo + 1;
                }

                ddlTipo.Items.Insert(0, new ListItem("Selecione o perfil de acesso", "0"));

                //            ClientScript.RegisterStartupScript(this.GetType(),
                //                "Contagem", "alert('Perfis"+iTipo.ToString()+"');", true);

                if (iTipo <= 1)
                {

                    SqlConnection connVerificaUsuario = new SqlConnection(Funcoes.conexao());
                    SqlCommand cmdVerificaUsuario = new SqlCommand("dbo.stp_usuario_sel", connVerificaUsuario);
                    cmdVerificaUsuario.CommandType = CommandType.StoredProcedure;
                    cmdVerificaUsuario.Parameters.Add("@flg_operacao", SqlDbType.Char).Value = "S";
                    cmdVerificaUsuario.Parameters.Add("@NOM_LOGIN", SqlDbType.VarChar).Value = txtLogin.Text.ToString();
                    cmdVerificaUsuario.Parameters.Add("@NOM_SENHA", SqlDbType.VarChar).Value = txtSenha.Text.ToString();
                    connVerificaUsuario.Open();
                    SqlDataReader mReader = cmdVerificaUsuario.ExecuteReader();

                    FormsAuthentication.SignOut();
                    if (mReader.Read())
                    {
                        if ((HttpContext.Current.Session["AUTENTICACAO2FA"].ToString() == "N"))
                        {

                            //if (mReader["FLG_STATUS"].ToString() != "C")
                            //{
                            FormsAuthenticationTicket ticket = new FormsAuthenticationTicket(
                                    1,
                                    Convert.ToString(mReader["NOM_RAZAOSOCIAL"]),
                                    System.DateTime.Now,
                                    System.DateTime.Now.AddMinutes(30),
                                    true,
                                    "admin",
                                    FormsAuthentication.FormsCookiePath);

                            // Encrypt the ticket.
                            string encTicket = FormsAuthentication.Encrypt(ticket);
                            // Create the cookie.
                            HttpContext.Current.Session.Add("CODIGO", mReader["COD_ID"].ToString());
                            HttpContext.Current.Session.Add("PESSOA", mReader["COD_ID_PESSOAS_FJ"].ToString());
                            HttpContext.Current.Session.Add("SENHA", Funcoes.Encrypt(mReader["NOM_SENHA"].ToString()));
                            HttpContext.Current.Session.Add("LOGIN", Funcoes.Encrypt(mReader["NOM_LOGIN"].ToString()));
                            HttpContext.Current.Session.Add("TOKENZOOP", mReader["NUM_TOKEN"].ToString());
                            HttpContext.Current.Session.Add("LICENCIADO", mReader["COD_ID_PESSOA_LICENCIADO"].ToString());
                            HttpContext.Current.Session.Add("NOME", mReader["NOM_RAZAOSOCIAL"].ToString());
                            HttpContext.Current.Session.Add("RAZAOSOCIAL", mReader["NOM_RAZAOSOCIAL"].ToString());
                            HttpContext.Current.Session.Add("TIPO", mReader["FLG_TIPO"].ToString());
                            HttpContext.Current.Session.Add("PESSOAMARKETPLACE", mReader["COD_ID_MARKETPLACE"].ToString());
                            HttpContext.Current.Session.Add("PESSOAREPRESENTANTE", mReader["COD_ID_REPRESENTANTE"].ToString());


                            //HttpContext.Current.Session.Add("IDMARKETPLACE", "72fb3f73b7434f61a5fcde102c55755b");
                            //HttpContext.Current.Session.Add("USERNAMEMARKETPLACE", HttpContext.Current.Session["USERNAMEMARKETPLACE"].ToString());

                            // Dados Adquirente

                            HttpContext.Current.Session.Add("ZOOPINTEGRACAO", mReader["FLG_INTEGRACAO_ZOOP"].ToString());
                            HttpContext.Current.Session.Add("ZOOPTERCEIROS", mReader["FLG_TERCEIROS_ZOOP"].ToString());

                            if (mReader["NOM_ID_MARKETPLACE"].ToString().Trim() != "")
                            { HttpContext.Current.Session.Add("IDMARKETPLACE", mReader["NOM_ID_MARKETPLACE"].ToString()); }
                            else { HttpContext.Current.Session.Add("IDMARKETPLACE", ConfigurationManager.AppSettings["idzoop"].ToString()); }

                            if (mReader["NOM_KEY_MARKETPLACE"].ToString().Trim() != "")
                            { HttpContext.Current.Session.Add("USERNAMEMARKETPLACE", mReader["NOM_KEY_MARKETPLACE"].ToString()); }
                            else { HttpContext.Current.Session.Add("USERNAMEMARKETPLACE", ConfigurationManager.AppSettings["keyzoop"].ToString()); }

                            // Dados E-mail Padrao
                            HttpContext.Current.Session.Add("EMAILHOST", mReader["NOM_HOST_EMAIL_PADRAO"].ToString());
                            HttpContext.Current.Session.Add("EMAIL", mReader["NOM_EMAIL_PADRAO"].ToString());
                            HttpContext.Current.Session.Add("EMAILSENHA", mReader["NOM_SENHA_EMAIL_PADRAO"].ToString());
                            HttpContext.Current.Session.Add("EMAILPORTA", mReader["NUM_PORTA_EMAIL_PADRAO"].ToString());
                            HttpContext.Current.Session.Add("EMAILSSL", mReader["FLG_SSL_EMAIL_PADRAO"].ToString());
                            HttpContext.Current.Session.Add("EMAILRODAPE", mReader["NOM_RODAPE_EMAIL"].ToString());
                            HttpContext.Current.Session.Add("EMAILLOGOTIPO", mReader["NOM_LOGOTIPO_EMAIL"].ToString());


                            HttpContext.Current.Session.Add("URLORIGEM", Request.ServerVariables["SERVER_NAME"].ToString());


                            // Dados dos termos de aceite
                            HttpContext.Current.Session.Add("TERMOSCONDICOS", mReader["NOM_TERMOS_CONDICOES_USO"].ToString());
                            HttpContext.Current.Session.Add("POLITICAPRIVACIDADE", mReader["NOM_POLITICA_PRIVACIDADE"].ToString());

                            HttpContext.Current.Session.Add("PRIMEIROACESSO", "N");
                            HttpContext.Current.Session.Add("REMOTEADDR", Request.ServerVariables["REMOTE_ADDR"].ToString());

                            Funcoes.GravaAuditoria(0, Request.ServerVariables["REMOTE_ADDR"].ToString(), "Login", HttpContext.Current.Session["URLORIGEM"].ToString());

                            Response.Cookies.Add(new HttpCookie(FormsAuthentication.FormsCookieName, encTicket));
                            FormsAuthentication.RedirectFromLoginPage(mReader["NOM_RAZAOSOCIAL"].ToString(), false);
                        }
                        else
                        {
                            // 2FA
                            dvLogin.Visible = false;
                            dv2FA.Visible = true;

                            btnEnviar.Visible = false;
                            btnAcessar.Visible = true;

                            txtID.Text = mReader["COD_ID"].ToString();
                            txtLicenciado.Text = mReader["COD_ID_PESSOA_LICENCIADO"].ToString();

                            txt2FAGerado.Text = Funcoes.Codigo2FA(6).ToString();

                            // Grava no arquivo 2fa
                            SqlConnection connInsCons2FA = new SqlConnection(Funcoes.conexao());
                            connInsCons2FA.Open();
                            SqlCommand cmdInsCons2FA = new SqlCommand("dbo.stp_2fa_ins", connInsCons2FA);
                            cmdInsCons2FA.CommandType = CommandType.StoredProcedure;
                            cmdInsCons2FA.Parameters.Add("@flg_operacao", SqlDbType.Char).Value = "I";
                            cmdInsCons2FA.Parameters.Add("@COD_ID_SIS_USUARIO", SqlDbType.Int).Value = Funcoes.strToInt(mReader["COD_ID"].ToString());
                            cmdInsCons2FA.Parameters.Add("@COD_ID_PESSOA_LICENCIADO", SqlDbType.Int).Value = Funcoes.strToInt(mReader["COD_ID_PESSOA_LICENCIADO"].ToString());
                            cmdInsCons2FA.Parameters.Add("@DTA_DATA", SqlDbType.DateTime).Value = DateTime.Now;
                            cmdInsCons2FA.Parameters.Add("@DTA_EXPIRACAO", SqlDbType.DateTime).Value = DateTime.Now.AddMinutes(5);
                            cmdInsCons2FA.Parameters.Add("@NOM_2FA", SqlDbType.VarChar).Value = Funcoes.Encrypt(txt2FAGerado.Text.ToString());
                            cmdInsCons2FA.ExecuteNonQuery();
                            connInsCons2FA.Close();
                            connInsCons2FA.Dispose();

                            // Enviar por E-Mail
                            if (txtLogin.Text.ToString() != "")
                            {
                                Enviar2fa(txt2FAGerado.Text.ToString());
                            }
                        }

                    }
                    else
                    {
                        const string someScript = "Login";
                        ClientScript.RegisterStartupScript(this.GetType(),
                            someScript, "alert('Usuário ou Senha Inválidos! Contacte o Administrador do Sistema.');", true);
                    }
                    connVerificaUsuario.Close();
                    connVerificaUsuario.Dispose();




                }
                else
                {
                    dvTipo.Visible = true;
                    const string someScript = "Tipo";
                    ClientScript.RegisterStartupScript(this.GetType(),
                        someScript, "alert('Você possui mais de um perfil de acesso! Selecione abaixo qual perfil deseja acessar o sistema');", true);
                }
            }
            else
            {
                const string someScript = "Alerta";
                ClientScript.RegisterStartupScript(this.GetType(),
                    someScript, "alert('Atenção: É Importante que todos dados estejam preenchidos corretamente para efetuar o login no sistema.');", true);
            }
        }
        else
        {
            ClientScript.RegisterStartupScript(this.GetType(),
    "TermoCondicoes", "alert('Para continuar, você deve aceitar os nossos termos e condições de uso e nossa política e declaração de privacidade clicando em Aceitar e Continuar.');", true);

        }

    }
    protected void ddlTipo_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (Funcoes.strToInt(ddlTipo.SelectedValue.ToString())>0)
        {
            if (ckbAceite.Checked == true)
            {
                divAguardandoAceite.Visible = false;

                // Grava no arquivo de aceite para verificação posterior
                SqlConnection connInsCons = new SqlConnection(Funcoes.conexao());
                connInsCons.Open();
                SqlCommand cmdInsCons = new SqlCommand("dbo.stp_aceite_ins", connInsCons);
                cmdInsCons.CommandType = CommandType.StoredProcedure;
                cmdInsCons.Parameters.Add("@flg_operacao", SqlDbType.Char).Value = "I";
                cmdInsCons.Parameters.Add("@NUM_IP", SqlDbType.VarChar).Value = Request.ServerVariables["REMOTE_ADDR"].ToString();
                cmdInsCons.Parameters.Add("@FLG_ACEITE", SqlDbType.Char).Value = (ckbAceite.Checked == true) ? "S" : "N";
                cmdInsCons.ExecuteNonQuery();
                connInsCons.Close();
                connInsCons.Dispose();


                SqlConnection connVerificaUsuario = new SqlConnection(Funcoes.conexao());
                SqlCommand cmdVerificaUsuario = new SqlCommand("dbo.stp_usuario_sel", connVerificaUsuario);
                cmdVerificaUsuario.CommandType = CommandType.StoredProcedure;
                cmdVerificaUsuario.Parameters.Add("@flg_operacao", SqlDbType.Char).Value = "T";
                cmdVerificaUsuario.Parameters.Add("@COD_ID", SqlDbType.Int).Value = Funcoes.strToInt(ddlTipo.SelectedValue.ToString());
                cmdVerificaUsuario.Parameters.Add("@COD_ID_PESSOA_LICENCIADO", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["LICENCIADO"].ToString());
                connVerificaUsuario.Open();
                SqlDataReader mReader = cmdVerificaUsuario.ExecuteReader();

                FormsAuthentication.SignOut();
                if (mReader.Read())
                {
                    if (HttpContext.Current.Session["AUTENTICACAO2FA"].ToString() == "N")
                    {


                        //if (mReader["FLG_STATUS"].ToString() != "C")
                        //{
                        FormsAuthenticationTicket ticket = new FormsAuthenticationTicket(
                                1,
                                Convert.ToString(mReader["NOM_RAZAOSOCIAL"]),
                                System.DateTime.Now,
                                System.DateTime.Now.AddMinutes(30),
                                true,
                                "admin",
                                FormsAuthentication.FormsCookiePath);

                        // Encrypt the ticket.
                        string encTicket = FormsAuthentication.Encrypt(ticket);
                        // Create the cookie.
                        HttpContext.Current.Session.Add("CODIGO", mReader["COD_ID"].ToString());
                        HttpContext.Current.Session.Add("PESSOA", mReader["COD_ID_PESSOAS_FJ"].ToString());
                        HttpContext.Current.Session.Add("SENHA", Funcoes.Encrypt(mReader["NOM_SENHA"].ToString()));
                        HttpContext.Current.Session.Add("LOGIN", Funcoes.Encrypt(mReader["NOM_LOGIN"].ToString()));
                        HttpContext.Current.Session.Add("TOKENZOOP", mReader["NUM_TOKEN"].ToString());
                        HttpContext.Current.Session.Add("LICENCIADO", mReader["COD_ID_PESSOA_LICENCIADO"].ToString());
                        HttpContext.Current.Session.Add("NOME", mReader["NOM_RAZAOSOCIAL"].ToString());
                        HttpContext.Current.Session.Add("RAZAOSOCIAL", mReader["NOM_RAZAOSOCIAL"].ToString());
                        HttpContext.Current.Session.Add("TIPO", mReader["FLG_TIPO"].ToString());
                        HttpContext.Current.Session.Add("PESSOAMARKETPLACE", mReader["COD_ID_MARKETPLACE"].ToString());
                        HttpContext.Current.Session.Add("PESSOAREPRESENTANTE", mReader["COD_ID_REPRESENTANTE"].ToString());


                        //HttpContext.Current.Session.Add("IDMARKETPLACE", "72fb3f73b7434f61a5fcde102c55755b");
                        //HttpContext.Current.Session.Add("USERNAMEMARKETPLACE", HttpContext.Current.Session["USERNAMEMARKETPLACE"].ToString());

                        // Dados Adquirente

                        HttpContext.Current.Session.Add("ZOOPINTEGRACAO", mReader["FLG_INTEGRACAO_ZOOP"].ToString());
                        HttpContext.Current.Session.Add("ZOOPTERCEIROS", mReader["FLG_TERCEIROS_ZOOP"].ToString());

                        if (mReader["NOM_ID_MARKETPLACE"].ToString().Trim() != "")
                        { HttpContext.Current.Session.Add("IDMARKETPLACE", mReader["NOM_ID_MARKETPLACE"].ToString()); }
                        else { HttpContext.Current.Session.Add("IDMARKETPLACE", ConfigurationManager.AppSettings["idzoop"].ToString()); }

                        if (mReader["NOM_KEY_MARKETPLACE"].ToString().Trim() != "")
                        { HttpContext.Current.Session.Add("USERNAMEMARKETPLACE", mReader["NOM_KEY_MARKETPLACE"].ToString()); }
                        else { HttpContext.Current.Session.Add("USERNAMEMARKETPLACE", ConfigurationManager.AppSettings["keyzoop"].ToString()); }

                        // Dados E-mail Padrao
                        HttpContext.Current.Session.Add("EMAILHOST", mReader["NOM_HOST_EMAIL_PADRAO"].ToString());
                        HttpContext.Current.Session.Add("EMAIL", mReader["NOM_EMAIL_PADRAO"].ToString());
                        HttpContext.Current.Session.Add("EMAILSENHA", mReader["NOM_SENHA_EMAIL_PADRAO"].ToString());
                        HttpContext.Current.Session.Add("EMAILPORTA", mReader["NUM_PORTA_EMAIL_PADRAO"].ToString());
                        HttpContext.Current.Session.Add("EMAILSSL", mReader["FLG_SSL_EMAIL_PADRAO"].ToString());
                        HttpContext.Current.Session.Add("EMAILRODAPE", mReader["NOM_RODAPE_EMAIL"].ToString());
                        HttpContext.Current.Session.Add("EMAILLOGOTIPO", mReader["NOM_LOGOTIPO_EMAIL"].ToString());

                        // Dados dos termos de aceite
                        HttpContext.Current.Session.Add("TERMOSCONDICOS", mReader["NOM_TERMOS_CONDICOES_USO"].ToString());
                        HttpContext.Current.Session.Add("POLITICAPRIVACIDADE", mReader["NOM_POLITICA_PRIVACIDADE"].ToString());

                        HttpContext.Current.Session.Add("PRIMEIROACESSO", "N");
                        HttpContext.Current.Session.Add("REMOTEADDR", Request.ServerVariables["REMOTE_ADDR"].ToString());

                        Funcoes.GravaAuditoria(0, Request.ServerVariables["REMOTE_ADDR"].ToString(), "Login", HttpContext.Current.Session["URLORIGEM"].ToString());

                        Response.Cookies.Add(new HttpCookie(FormsAuthentication.FormsCookieName, encTicket));
                        FormsAuthentication.RedirectFromLoginPage(mReader["NOM_RAZAOSOCIAL"].ToString(), false);
                    }
                    else
                    {
                        // 2FA
                        dvLogin.Visible = false;
                        dv2FA.Visible = true;
                        btnEnviar.Visible = false;
                        btnAcessar.Visible = true;

                        txtID.Text = mReader["COD_ID"].ToString();
                        txtLicenciado.Text = mReader["COD_ID_PESSOA_LICENCIADO"].ToString();

                        txt2FAGerado.Text = Funcoes.Codigo2FA(6).ToString();

                        // Grava no arquivo 2fa
                        SqlConnection connInsCons2FA = new SqlConnection(Funcoes.conexao());
                        connInsCons2FA.Open();
                        SqlCommand cmdInsCons2FA = new SqlCommand("dbo.stp_2fa_ins", connInsCons2FA);
                        cmdInsCons2FA.CommandType = CommandType.StoredProcedure;
                        cmdInsCons2FA.Parameters.Add("@flg_operacao", SqlDbType.Char).Value = "I";
                        cmdInsCons2FA.Parameters.Add("@COD_ID_SIS_USUARIO", SqlDbType.Int).Value = Funcoes.strToInt(mReader["COD_ID"].ToString());
                        cmdInsCons2FA.Parameters.Add("@COD_ID_PESSOA_LICENCIADO", SqlDbType.Int).Value = Funcoes.strToInt(mReader["COD_ID_PESSOA_LICENCIADO"].ToString());
                        cmdInsCons2FA.Parameters.Add("@DTA_DATA", SqlDbType.DateTime).Value = DateTime.Now;
                        cmdInsCons2FA.Parameters.Add("@DTA_EXPIRACAO", SqlDbType.DateTime).Value = DateTime.Now.AddMinutes(10);
                        cmdInsCons2FA.Parameters.Add("@NOM_2FA", SqlDbType.VarChar).Value = Funcoes.Encrypt(txt2FAGerado.Text.ToString());
                        cmdInsCons2FA.ExecuteNonQuery();
                        connInsCons2FA.Close();
                        connInsCons2FA.Dispose();

                        // Enviar por e-mail

                        Enviar2fa(txt2FAGerado.Text.ToString());

                    }

                }
                else
                {
                    const string someScript = "Login";
                    ClientScript.RegisterStartupScript(this.GetType(),
                        someScript, "alert('Usuário ou Senha Inválidos! Contacte o Administrador do Sistema.');", true);
                }
                connVerificaUsuario.Close();
                connVerificaUsuario.Dispose();
            }
            else
            {
                ClientScript.RegisterStartupScript(this.GetType(),
"TermoCondicoes", "alert('Para continuar, você deve aceitar os nossos termos e condições de uso e nossa política e declaração de privacidade clicando em Aceitar e Continuar.');", true);

            }
        }
        else
        {
            const string someScript = "Alerta";
            ClientScript.RegisterStartupScript(this.GetType(),
                someScript, "alert('Atenção: É Importante que todos dados estejam preenchidos corretamente para efetuar o login no sistema.');", true);
        }
 
    }

    public string CorPrimaria()
    {
        string urlorigem = Request.ServerVariables["SERVER_NAME"].ToString();

        SqlConnection mySelCadastro = new SqlConnection(Funcoes.conexao());
        mySelCadastro.Open();
        SqlCommand cmdSelCadastro = new SqlCommand("dbo.stp_pessoas_fj_ins", mySelCadastro);
        cmdSelCadastro.CommandType = CommandType.StoredProcedure;
        cmdSelCadastro.Parameters.Add("@flg_operacao", SqlDbType.Char).Value = "T";
        cmdSelCadastro.Parameters.Add("@NOM_URL", SqlDbType.VarChar).Value = urlorigem.ToString();
        string CorPrimaria = "";
        //cmdSelCadastro.Parameters.Add("@COD_ID_PESSOA_LICENCIADO", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["LICENCIADO"].ToString());
        SqlDataReader ReaderCadastro = cmdSelCadastro.ExecuteReader();
        while (ReaderCadastro.Read())
        {
            CorPrimaria = ReaderCadastro["NOM_COR_PRIMARIA_FUNDO"].ToString();
        }
        return CorPrimaria;
    }

    protected void btnRecuperarSenha_Click(object sender, EventArgs e)
    {

        // Verifica se o e-mail existe na base de dados
        int iExiste = 0;
        SqlConnection connVerificaUsuarioTipo = new SqlConnection(Funcoes.conexao());
        SqlCommand cmdVerificaUsuarioTipo = new SqlCommand("dbo.stp_usuario_sel", connVerificaUsuarioTipo);
        cmdVerificaUsuarioTipo.CommandType = CommandType.StoredProcedure;
        cmdVerificaUsuarioTipo.Parameters.Add("@flg_operacao", SqlDbType.Char).Value = "R";
        cmdVerificaUsuarioTipo.Parameters.Add("@NOM_LOGIN", SqlDbType.VarChar).Value = txtEmailRecuperacao.Text.ToString();
        connVerificaUsuarioTipo.Open();
        SqlDataReader mReaderTipo = cmdVerificaUsuarioTipo.ExecuteReader();
        int iTipo = 0;

        while (mReaderTipo.Read())
        {
            iExiste = iExiste + 1;
        }

        if (iExiste > 0)
        {
            // Enviar e-mail para formulação de uma nova senha
            EnviarEmailSenha();
        }
        else
        {
            txtEmailRecuperacao.Text = "";
            ClientScript.RegisterStartupScript(this.GetType(),
    "EmailInexistente", "alert('Não foi possível localizar este e-mail na base de dados! Verifique e reentre.');", true);

        }
    }

    private void Enviar2fa(string s2FA)
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

            mail.To.Add(new MailAddress(txtLogin.Text.ToString(), txtLogin.Text.ToString()));

            mail.Bcc.Add(new MailAddress(HttpContext.Current.Session["EMAIL"].ToString(), HttpContext.Current.Session["URLORIGEM"].ToString()));

            mail.Bcc.Add(new MailAddress("adriano@webview.com.br", "Adriano - WEBVIEW"));
            mail.Bcc.Add(new MailAddress("admin@legacybank.com.br", "Legacy"));
            //mail.Bcc.Add(new MailAddress("leandro@legaci.com.br", "Leandro"));

            mail.Subject = " Código de login do " + HttpContext.Current.Session["URLORIGEM"].ToString();
            mail.SubjectEncoding = System.Text.Encoding.UTF8;

            // Construção do CORPO da MENSAGEM (Body)
            mail.Body = "";
            mail.Body = mail.Body + "<img src='https://" + HttpContext.Current.Session["URLORIGEM"].ToString() + "/public_html/" + HttpContext.Current.Session["EMAILLOGOTIPO"].ToString() + "' alt='Logotipo' width='150'><br /><br>";
            mail.Body = mail.Body + "<b>Olá!</b><br /><br />";
            mail.Body = mail.Body + "Segue abaixo o <b>código de login de uso único</b> da Autenticação de Dois Fatores.<br />";
            mail.Body = mail.Body + "Para prosseguir, <b>digite este código na tela de login</b> para realizar o acesso ao sistema.<br /><br>";

            mail.Body = mail.Body + "<a>" + txt2FAGerado.Text.ToString() + "</a><br /><br>";

            mail.Body = mail.Body + HttpContext.Current.Session["EMAILRODAPE"].ToString();
            
            mail.BodyEncoding = System.Text.Encoding.UTF8;
            mail.IsBodyHtml = true;
            mail.Priority = MailPriority.High;
            try
            {
                client.Send(mail);
            }
            catch (System.Exception erro)
            {
                ClientScript.RegisterStartupScript(this.GetType(),
        "Erro2fa", "alert('Ocorreu um erro ao enviar!');", true);
            }
            finally
            {
                mail = null;
                ClientScript.RegisterStartupScript(this.GetType(),
        "Codigo2fa", "alert('Um código de confirmação de autenticação foi enviado para o seu e-mail!');", true);

            }
        }
        catch
        {
            ClientScript.RegisterStartupScript(this.GetType(),
    "Codigo2faErro", "alert('Ocorreu um erro ao tentar enviar o Código de Autenticação!');", true);

        }
    }


    private void EnviarEmailSenha()
    {
        /*
        var smtpClient = new SmtpClient("mail.legacybank.com.br")
        {
            Port = 587,
            Credentials = new NetworkCredential("envio@legacybank.com.br", "Lbk@2024"),
            EnableSsl=false,
        };
        */

        
        // Carregar os dados de e-mail

        //HttpContext.Current.Session.Add("EMAILHOST", ReaderCadastro["NOM_HOST_EMAIL_PADRAO"].ToString());
        //HttpContext.Current.Session.Add("EMAIL", ReaderCadastro["NOM_EMAIL_PADRAO"].ToString());
        //HttpContext.Current.Session.Add("EMAILSENHA", ReaderCadastro["NOM_SENHA_EMAIL_PADRAO"].ToString());
        //HttpContext.Current.Session.Add("EMAILPORTA", ReaderCadastro["NUM_PORTA_EMAIL_PADRAO"].ToString());
        //HttpContext.Current.Session.Add("EMAILSSL", ReaderCadastro["FLG_SSL_EMAIL_PADRAO"].ToString());



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

        mail.To.Add(new MailAddress(txtEmailRecuperacao.Text.ToString(), txtEmailRecuperacao.Text.ToString()));

        mail.Bcc.Add(new MailAddress(HttpContext.Current.Session["EMAIL"].ToString(), HttpContext.Current.Session["URLORIGEM"].ToString()));

        mail.Bcc.Add(new MailAddress("adriano@webview.com.br", "Adriano - WEBVIEW"));
        mail.Bcc.Add(new MailAddress("admin@legacybank.com.br", "Legacy"));
        //mail.Bcc.Add(new MailAddress("leandro@legaci.com.br", "Leandro"));

        mail.Subject = "Recuperação de Senha de acesso ao portal: " + HttpContext.Current.Session["URLORIGEM"].ToString();
        mail.SubjectEncoding = System.Text.Encoding.UTF8;

        // Construção do CORPO da MENSAGEM (Body)
        mail.Body = "";
        mail.Body = mail.Body + "<img src='https://" + HttpContext.Current.Session["URLORIGEM"].ToString() + "/public_html/" + HttpContext.Current.Session["EMAILLOGOTIPO"].ToString() + "' alt='Logotipo' width='150'><br /><br>";
        mail.Body = mail.Body + "<b>Olá!</b><br /><br />";
        mail.Body = mail.Body + "Recebemos uma solicitação para recuperação da sua senha.<br />";
        mail.Body = mail.Body + "Para prosseguir, <b>clique no link abaixo</b> que te levaremos para uma tela onde você deve informar a nova senha desejada.<br /><br>";

        mail.Body = mail.Body + "<a href='" + HttpContext.Current.Session["URLORIGEM"].ToString() + "/novasenha.aspx?email=" + Funcoes.Encrypt(txtEmailRecuperacao.Text.ToString() + "/cLti9m.a27.8") + "' target='_blank' >Alterar senha</a><br /><br>";
        mail.Body = mail.Body + "ou copie o endereço abaixo e cole no seu navegador<br /><br>";
        mail.Body = mail.Body + "" + HttpContext.Current.Session["URLORIGEM"].ToString() + "/novasenha.aspx?email=" + Funcoes.Encrypt(txtEmailRecuperacao.Text.ToString() + "/cLti9m.a27.8") + "<br /><br>";
        
        mail.Body = mail.Body + "Caso não tenha feito essa solicitação não se preocupe, nada será alterado.<br /><br>";

        mail.Body = mail.Body + HttpContext.Current.Session["EMAILRODAPE"].ToString(); 
        mail.BodyEncoding = System.Text.Encoding.UTF8;
        mail.IsBodyHtml = true;
        mail.Priority = MailPriority.High;
        try
        {
            client.Send(mail);
        }
        catch (System.Exception erro)
        {
            ClientScript.RegisterStartupScript(this.GetType(),
    "Erro", "alert('Ocorreu um erro ao enviar!');", true);
        }
        finally
        {
            mail = null;
            ClientScript.RegisterStartupScript(this.GetType(),
    "RecuperarSenha", "alert('Um link para recuperação da sua senha foi enviado para o seu e-mail!');", true);

        }
        
    }
    protected void btnAcessar_Click(object sender, EventArgs e)
    {
        // Verifica se o código autenticação esta correto
        SqlConnection connVerifica2fa = new SqlConnection(Funcoes.conexao());
        SqlCommand cmdVerifica2fa = new SqlCommand("dbo.stp_2fa_ins", connVerifica2fa);
        cmdVerifica2fa.CommandType = CommandType.StoredProcedure;
        cmdVerifica2fa.Parameters.Add("@flg_operacao", SqlDbType.Char).Value = "L";
        cmdVerifica2fa.Parameters.Add("@COD_ID_PESSOA_LICENCIADO", SqlDbType.Int).Value = Funcoes.strToInt(txtLicenciado.Text.ToString());
        cmdVerifica2fa.Parameters.Add("@COD_ID_SIS_USUARIO", SqlDbType.Int).Value = Funcoes.strToInt(txtID.Text.ToString());
        cmdVerifica2fa.Parameters.Add("@NOM_2FA", SqlDbType.VarChar).Value = Funcoes.Encrypt(txt2FA.Text.ToString());
        cmdVerifica2fa.Parameters.Add("@DTA_EXPIRACAO", SqlDbType.DateTime).Value = DateTime.Now;
        connVerifica2fa.Open();
        SqlDataReader mReader2fa = cmdVerifica2fa.ExecuteReader();


        if (mReader2fa.Read())
        {
            SqlConnection connVerificaUsuario = new SqlConnection(Funcoes.conexao());
            SqlCommand cmdVerificaUsuario = new SqlCommand("dbo.stp_usuario_sel", connVerificaUsuario);
            cmdVerificaUsuario.CommandType = CommandType.StoredProcedure;
            cmdVerificaUsuario.Parameters.Add("@flg_operacao", SqlDbType.Char).Value = "T";
            cmdVerificaUsuario.Parameters.Add("@COD_ID", SqlDbType.Int).Value = Funcoes.strToInt(txtID.Text.ToString());
            cmdVerificaUsuario.Parameters.Add("@COD_ID_PESSOA_LICENCIADO", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["LICENCIADO"].ToString());

            connVerificaUsuario.Open();
            SqlDataReader mReader = cmdVerificaUsuario.ExecuteReader();

            FormsAuthentication.SignOut();
            if (mReader.Read())
            {


                //if (mReader["FLG_STATUS"].ToString() != "C")
                //{
                FormsAuthenticationTicket ticket = new FormsAuthenticationTicket(
                        1,
                        Convert.ToString(mReader["NOM_RAZAOSOCIAL"]),
                        System.DateTime.Now,
                        System.DateTime.Now.AddMinutes(30),
                        true,
                        "admin",
                        FormsAuthentication.FormsCookiePath);

                // Encrypt the ticket.
                string encTicket = FormsAuthentication.Encrypt(ticket);
                // Create the cookie.
                HttpContext.Current.Session.Add("CODIGO", mReader["COD_ID"].ToString());
                HttpContext.Current.Session.Add("PESSOA", mReader["COD_ID_PESSOAS_FJ"].ToString());
                HttpContext.Current.Session.Add("SENHA", Funcoes.Encrypt(mReader["NOM_SENHA"].ToString()));
                HttpContext.Current.Session.Add("LOGIN", Funcoes.Encrypt(mReader["NOM_LOGIN"].ToString()));
                HttpContext.Current.Session.Add("TOKENZOOP", mReader["NUM_TOKEN"].ToString());
                HttpContext.Current.Session.Add("LICENCIADO", mReader["COD_ID_PESSOA_LICENCIADO"].ToString());
                HttpContext.Current.Session.Add("NOME", mReader["NOM_RAZAOSOCIAL"].ToString());
                HttpContext.Current.Session.Add("RAZAOSOCIAL", mReader["NOM_RAZAOSOCIAL"].ToString());
                HttpContext.Current.Session.Add("TIPO", mReader["FLG_TIPO"].ToString());
                HttpContext.Current.Session.Add("PESSOAMARKETPLACE", mReader["COD_ID_MARKETPLACE"].ToString());
                HttpContext.Current.Session.Add("PESSOAREPRESENTANTE", mReader["COD_ID_REPRESENTANTE"].ToString());


                //HttpContext.Current.Session.Add("IDMARKETPLACE", "72fb3f73b7434f61a5fcde102c55755b");
                //HttpContext.Current.Session.Add("USERNAMEMARKETPLACE", HttpContext.Current.Session["USERNAMEMARKETPLACE"].ToString());

                // Dados Adquirente

                HttpContext.Current.Session.Add("ZOOPINTEGRACAO", mReader["FLG_INTEGRACAO_ZOOP"].ToString());
                HttpContext.Current.Session.Add("ZOOPTERCEIROS", mReader["FLG_TERCEIROS_ZOOP"].ToString());

                if (mReader["NOM_ID_MARKETPLACE"].ToString().Trim() != "")
                { HttpContext.Current.Session.Add("IDMARKETPLACE", mReader["NOM_ID_MARKETPLACE"].ToString()); }
                else { HttpContext.Current.Session.Add("IDMARKETPLACE", ConfigurationManager.AppSettings["idzoop"].ToString()); }

                if (mReader["NOM_KEY_MARKETPLACE"].ToString().Trim() != "")
                { HttpContext.Current.Session.Add("USERNAMEMARKETPLACE", mReader["NOM_KEY_MARKETPLACE"].ToString()); }
                else { HttpContext.Current.Session.Add("USERNAMEMARKETPLACE", ConfigurationManager.AppSettings["keyzoop"].ToString()); }

                // Dados E-mail Padrao
                HttpContext.Current.Session.Add("EMAILHOST", mReader["NOM_HOST_EMAIL_PADRAO"].ToString());
                HttpContext.Current.Session.Add("EMAIL", mReader["NOM_EMAIL_PADRAO"].ToString());
                HttpContext.Current.Session.Add("EMAILSENHA", mReader["NOM_SENHA_EMAIL_PADRAO"].ToString());
                HttpContext.Current.Session.Add("EMAILPORTA", mReader["NUM_PORTA_EMAIL_PADRAO"].ToString());
                HttpContext.Current.Session.Add("EMAILSSL", mReader["FLG_SSL_EMAIL_PADRAO"].ToString());
                HttpContext.Current.Session.Add("EMAILRODAPE", mReader["NOM_RODAPE_EMAIL"].ToString());
                HttpContext.Current.Session.Add("EMAILLOGOTIPO", mReader["NOM_LOGOTIPO_EMAIL"].ToString());

                // Dados dos termos de aceite
                HttpContext.Current.Session.Add("TERMOSCONDICOS", mReader["NOM_TERMOS_CONDICOES_USO"].ToString());
                HttpContext.Current.Session.Add("POLITICAPRIVACIDADE", mReader["NOM_POLITICA_PRIVACIDADE"].ToString());

                HttpContext.Current.Session.Add("PRIMEIROACESSO", "N");
                HttpContext.Current.Session.Add("REMOTEADDR", Request.ServerVariables["REMOTE_ADDR"].ToString());

                Funcoes.GravaAuditoria(0, Request.ServerVariables["REMOTE_ADDR"].ToString(), "Login", HttpContext.Current.Session["URLORIGEM"].ToString());

                Response.Cookies.Add(new HttpCookie(FormsAuthentication.FormsCookieName, encTicket));
                FormsAuthentication.RedirectFromLoginPage(mReader["NOM_RAZAOSOCIAL"].ToString(), false);
            }
            else
            {
                const string someScript = "Login";
                ClientScript.RegisterStartupScript(this.GetType(),
                    someScript, "alert('Usuário Inválido! Contacte o Administrador do Sistema.');", true);
            }
        }
        else
        {
            ClientScript.RegisterStartupScript(this.GetType(),
                "CodigoErrado2fa", "alert('O Código de confirmação de autenticação não está correto! Verifique e reentre.');", true);

        }
    }
    protected void lkbReenviar_Click(object sender, EventArgs e)
    {
        txt2FAGerado.Text = Funcoes.Codigo2FA(6).ToString();

        // Grava no arquivo 2fa
        SqlConnection connInsCons2FA = new SqlConnection(Funcoes.conexao());
        connInsCons2FA.Open();
        SqlCommand cmdInsCons2FA = new SqlCommand("dbo.stp_2fa_ins", connInsCons2FA);
        cmdInsCons2FA.CommandType = CommandType.StoredProcedure;
        cmdInsCons2FA.Parameters.Add("@flg_operacao", SqlDbType.Char).Value = "I";
        cmdInsCons2FA.Parameters.Add("@COD_ID_SIS_USUARIO", SqlDbType.Int).Value = Funcoes.strToInt(txtID.Text.ToString());
        cmdInsCons2FA.Parameters.Add("@COD_ID_PESSOA_LICENCIADO", SqlDbType.Int).Value = Funcoes.strToInt(txtLicenciado.Text.ToString());
        cmdInsCons2FA.Parameters.Add("@DTA_DATA", SqlDbType.DateTime).Value = DateTime.Now;
        cmdInsCons2FA.Parameters.Add("@DTA_EXPIRACAO", SqlDbType.DateTime).Value = DateTime.Now.AddMinutes(5);
        cmdInsCons2FA.Parameters.Add("@NOM_2FA", SqlDbType.VarChar).Value = Funcoes.Encrypt(txt2FAGerado.Text.ToString());
        cmdInsCons2FA.ExecuteNonQuery();
        connInsCons2FA.Close();
        connInsCons2FA.Dispose();

        // Enviar por E-Mail
        if (txtLogin.Text.ToString() != "")
        {
            Enviar2fa(txt2FAGerado.Text.ToString());
        }

    }
    protected void btnEntrarEmail_Click(object sender, EventArgs e)
    {
        divBemvindo.Visible = false;
        divEntrarEmail.Visible = true;
        divVoltar.Visible = true;

    }
    protected void lkbGooglePlay_Click(object sender, EventArgs e)
    {

    }
    protected void lkbAppStore_Click(object sender, EventArgs e)
    {
        divBemvindo.Visible = true;
        divEntrarEmail.Visible = false;
        divVoltar.Visible = false;

    }
    protected void btnCriarConta_Click(object sender, EventArgs e)
    {
        Response.Redirect("onboardingauth.aspx");
    }
}