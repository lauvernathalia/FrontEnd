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


public partial class novasenha : System.Web.UI.Page
{
    public string Recaptcha { get; set; }

    public string semail_email
    {
        get
        {
            try {
                string[] aEmail = Funcoes.Decrypt(Request["email"].ToString()).Split('/');

                return aEmail[0].ToString(); //Funcoes.Decrypt(Request["email"].ToString()); 
        }
            catch { return ""; }
        }
    }

    public string scontrole
    {
        get
        {
            try
            {
                string[] aEmail = Funcoes.Decrypt(Request["email"].ToString()).Split('/');

                return aEmail[1].ToString(); //Funcoes.Decrypt(Request["email"].ToString()); 
            }
            catch { return ""; }
        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {

        string urlorigem = Request.ServerVariables["SERVER_NAME"].ToString();

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
            HttpContext.Current.Session.Add("PESSOA", "0");
            HttpContext.Current.Session.Add("CODIGO", "0");

            imgLogoPrincipal.ImageUrl = "public_html/" + ReaderCadastro["NOM_LOGO"].ToString();
            lblSlogan.Text = ReaderCadastro["NOM_SLOGAN"].ToString();

            Page.Title = ReaderCadastro["NOM_RAZAOSOCIAL"].ToString();
            //Page.Header.Title = ReaderCadastro["NOM_RAZAOSOCIAL"].ToString();
            FavIconLink.Text = "<link rel='icon' type='image/x-icon' href='" + "public_html/" + ReaderCadastro["NOM_ICONE"].ToString() + "'>";
            if (ReaderCadastro["NOM_IMAGEM_FUNDO"].ToString().Trim() != "")
            {
                bdyLogin.Attributes.Add("style", "background: url(public_html/" + ReaderCadastro["NOM_IMAGEM_FUNDO"].ToString() + ") no-repeat center center fixed;  -webkit-background-size: cover;  -moz-background-size: cover;  background-size: cover;  -o-background-size: cover;");
            }
            else
            {
                if ((ReaderCadastro["NOM_COR_PRIMARIA_FUNDO"].ToString().Trim() != "") && (ReaderCadastro["NOM_COR_SECUNDARIA_FUNDO"].ToString().Trim() != ""))
                {
                    bdyLogin.Attributes.Add("style", "background: linear-gradient(" + ReaderCadastro["NOM_COR_PRIMARIA_FUNDO"].ToString() + ", " + ReaderCadastro["NOM_COR_SECUNDARIA_FUNDO"].ToString() + ");");
                }
            }
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

            // Dados dos Textos de Aceite
            HttpContext.Current.Session.Add("TERMOSCONDICOS", ReaderCadastro["NOM_TERMOS_CONDICOES_USO"].ToString());
            HttpContext.Current.Session.Add("POLITICAPRIVACIDADE", ReaderCadastro["NOM_POLITICA_PRIVACIDADE"].ToString());
        }
        lblEmail.Text = semail_email.ToString();

        if (!IsPostBack)
        {
            Funcoes.GravaAuditoria(0, Request.ServerVariables["REMOTE_ADDR"].ToString(), "Redefinir Senha", "Usuário:" + semail_email.ToString());
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




    protected void btnSalvar_Click(object sender, EventArgs e)
    {
        if (scontrole.ToString() == "cLti9m.a27.8")
        {
            // Enviar 2fa
            SalvarNovaSenha();
        }
        else
        {
            ClientScript.RegisterStartupScript(this.GetType(),
"ErroRecuperarSenha", "alert('Não foi possível alterar a senha! Verifique e reentre'); window.close();", true);

        }

    }

    private void SalvarNovaSenha()
    {
        if (txtNovaSenha.Text.ToString().Trim() != txtRepetirSenha.Text.ToString().Trim())
        {
            ClientScript.RegisterStartupScript(this.GetType(),
"RecuperarSenha", "alert('O conteúdo do campo Repetir Senha não é igual ao digitado no campo Nova Senha! Verifique e reentre');", true);

        }
        else
        {
            // Salvar a nova senha
            SqlConnection connInsCons = new SqlConnection(Funcoes.conexao());
            connInsCons.Open();
            SqlCommand cmdInsCons = new SqlCommand("dbo.stp_sis_usuario_ins", connInsCons);
            cmdInsCons.CommandType = CommandType.StoredProcedure;
            cmdInsCons.Parameters.Add("@flg_operacao", SqlDbType.Char).Value = 'U';
            cmdInsCons.Parameters.Add("@COD_ID_PESSOA_LICENCIADO", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["LICENCIADO"].ToString());
            cmdInsCons.Parameters.Add("@NOM_LOGIN", SqlDbType.VarChar).Value = semail_email.ToString();
            cmdInsCons.Parameters.Add("@NOM_SENHA", SqlDbType.VarChar).Value = txtNovaSenha.Text.ToString();
            cmdInsCons.ExecuteNonQuery();
            connInsCons.Close();
            connInsCons.Dispose();

            ClientScript.RegisterStartupScript(this.GetType(),
"SenhaAlterada", "alert('Senha alterada com sucesso!');", true);
            Response.Redirect("login.aspx");

        }

    }
}