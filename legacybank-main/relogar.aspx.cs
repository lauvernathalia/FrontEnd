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

public partial class relogar : System.Web.UI.Page
{

    public string sid_id
    {
        get
        {
            try { return Funcoes.Decrypt(Request["id"].ToString()); }
            catch { return ""; }
        }
    }

    public string sli
    {
        get
        {
            try { return Funcoes.Decrypt(Request["li"].ToString()); }
            catch { return ""; }
        }
    }


    protected void Page_Load(object sender, EventArgs e)
    {
        EfetuarLogin();
    }

    private void EfetuarLogin()
    {

        //Funcoes.GravaAuditoria(0, Request.ServerVariables["REMOTE_ADDR"].ToString(), "Licenciados - Usuarios - Relogar - Acesso", "Acesso - ID: " + (Funcoes.strToInt(sid_id.ToString()) - 3156789).ToString() + " - " + Request.ServerVariables["SERVER_NAME"].ToString());


        SqlConnection connVerificaUsuario = new SqlConnection(Funcoes.conexao());
        SqlCommand cmdVerificaUsuario = new SqlCommand("dbo.stp_usuario_sel", connVerificaUsuario);
        cmdVerificaUsuario.CommandType = CommandType.StoredProcedure;
        cmdVerificaUsuario.Parameters.Add("@flg_operacao", SqlDbType.Char).Value = "T";
        //cmdVerificaUsuario.Parameters.Add("@COD_ID", SqlDbType.Int).Value = Funcoes.strToInt(sid_id.ToString()) - 7985143;
        cmdVerificaUsuario.Parameters.Add("@COD_ID", SqlDbType.Int).Value = Funcoes.strToInt(sid_id.ToString()) - 3156789;
        cmdVerificaUsuario.Parameters.Add("@COD_ID_PESSOA_LICENCIADO", SqlDbType.Int).Value = Funcoes.strToInt(sli.ToString()) - 3156789;


        connVerificaUsuario.Open();
        SqlDataReader mReader = cmdVerificaUsuario.ExecuteReader();


        FormsAuthentication.SignOut();

        if (mReader.Read())
        {
            if (mReader["FLG_TIPO"].ToString() != "E")
            {
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

                Funcoes.GravaAuditoria(0, Request.ServerVariables["REMOTE_ADDR"].ToString(), "Login", "Acesso pelo Relogar ao Licenciado: " + Request.ServerVariables["SERVER_NAME"].ToString() + " - Usuário: " + mReader["NOM_LOGIN"].ToString());

                Response.Cookies.Add(new HttpCookie(FormsAuthentication.FormsCookieName, encTicket));
                FormsAuthentication.RedirectFromLoginPage(mReader["NOM_RAZAOSOCIAL"].ToString(), false);

                Response.Redirect("index.aspx");
            }
            else
            {
                ClientScript.RegisterStartupScript(this.GetType(),
                    "ACESSONAOAUTORIZADO", "alert('Você está tentando realizar um acesso não autorizado! Seus dados de acesso serão enviados ao Administrador do Sistema.');", true);

                Funcoes.GravaAuditoria(0, Request.ServerVariables["REMOTE_ADDR"].ToString(), "Login", "Acesso não Autorizado: " + Request.ServerVariables["SERVER_NAME"].ToString() + " - Usuário: " + mReader["NOM_LOGIN"].ToString());


            }
        }
        else
        {
            ClientScript.RegisterStartupScript(this.GetType(),
                "ACESSONAOLOCALIZADO", "alert('Não foi possível localizar o seu acesso! Seus dados serão enviados ao Administrador do Sistema.');", true);
        }

    }
}