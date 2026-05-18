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

public partial class con_marketplaces_usuarios_adm : System.Web.UI.Page
{


    public string sid_id
    {
        get
        {
            try { return Funcoes.Decrypt(Request["id"].ToString()); }
            catch { return ""; }
        }
    }


    public static string sOperacao { get; set; }
    public static string sCodigo { get; set; }
    public static string sUrlOrigem { get; set; }

    protected void Page_Load(object sender, EventArgs e)
    {
        // Verifica se o usuário esta autenticado
        if (HttpContext.Current.Session.Count <= 0)
        {
            Response.Redirect("login.aspx");
            FormsAuthentication.SignOut();
        }

        if (!IsPostBack)
        {
            Funcoes.GravaAuditoria(0, Request.ServerVariables["REMOTE_ADDR"].ToString(), "Marketplaces - Usuários - Adm", "Acesso - ID: " + Funcoes.strToInt(sid_id.ToString()).ToString() + " - " + Request.ServerVariables["SERVER_NAME"].ToString());


            sOperacao = "";
            sUrlOrigem = "";
            sCodigo = "0";

            ConsultaFicha();
            ConsultaUsuarios();
        }

    }

    private void ConsultaFicha()
    {
        SqlConnection mySelCadastro = new SqlConnection(Funcoes.conexao());
        mySelCadastro.Open();
        SqlCommand cmdSelCadastro = new SqlCommand("dbo.stp_pessoas_fj_ins", mySelCadastro);
        cmdSelCadastro.CommandType = CommandType.StoredProcedure;
        cmdSelCadastro.Parameters.Add("@flg_operacao", SqlDbType.Char).Value = "S";
        cmdSelCadastro.Parameters.Add("@COD_ID", SqlDbType.Int).Value = Funcoes.strToInt(sid_id.ToString());
        cmdSelCadastro.Parameters.Add("@COD_ID_PESSOA_LICENCIADO", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["LICENCIADO"].ToString());
        SqlDataReader ReaderCadastro = cmdSelCadastro.ExecuteReader();
        while (ReaderCadastro.Read())
        {
            lblID.Text = ReaderCadastro["COD_ID"].ToString();
            lblNome.Text = ReaderCadastro["NOM_RAZAOSOCIAL"].ToString();
        }

    }

    private void ConsultaUsuarios()
    {
        //dtgConsulta.Visible = true;
        SqlConnection myConsulta = new SqlConnection(Funcoes.conexao());
        myConsulta.Open();
        SqlDataAdapter SDAConsulta = new SqlDataAdapter("dbo.stp_sis_usuario_ins", myConsulta);
        SDAConsulta.SelectCommand.CommandType = CommandType.StoredProcedure;
        SDAConsulta.SelectCommand.Parameters.Add("@flg_operacao", SqlDbType.Char).Value = "C";
        SDAConsulta.SelectCommand.Parameters.Add("@COD_ID_PESSOA_LICENCIADO", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["LICENCIADO"].ToString());
        SDAConsulta.SelectCommand.Parameters.Add("@COD_ID_PESSOAS_FJ", SqlDbType.Int).Value = Funcoes.strToInt(sid_id.ToString());

        DataSet dsConsulta = new DataSet();
        SDAConsulta.Fill(dsConsulta, "SIS_USUARIO");
        rptConsulta.DataSource = dsConsulta.Tables["SIS_USUARIO"].DefaultView;
        rptConsulta.DataBind();
        myConsulta.Close(); myConsulta.Dispose();

    }

    protected void btnSalvar_Click(object sender, EventArgs e)
    {

        if ((txtNome.Text.ToString().Trim() != "") && (txtEmail.Text.ToString().Trim() != "") && (txtSenha.Text.ToString().Trim() != ""))
        {

            //Validar email else senha
            if (Funcoes.IsEmail(txtEmail.Text.ToString()) == false)
            {
                ClientScript.RegisterStartupScript(this.GetType(),
                    "AlertaEmailInvalido", "alert('O e-mail digitado não é válido! Verifique e tente novamente.'); ", true);
                return;
            }

            if (Funcoes.IsSenha(txtSenha.Text.ToString()) == false)
            {
                ClientScript.RegisterStartupScript(this.GetType(),
                    "AlertaSenhaInvalida", "alert('A Senha digitada não é válida! Verifique e tente novamente.'); ", true);
                ClientScript.RegisterStartupScript(this.GetType(),
                    "RecomendacaoSenha", "alert('ATENÇÃO! A senha deve atender os seguintes requisitos: Possua pelo menos 1 número, Possua pelo menos 1 letra maiúscula, Possua pelo menos 1 letra minúscula, Possua pelo menos 1 caractere especial e Não permitir espaço. Além disso, a senha deve ter de 6 a 32 caracteres.'); ", true);
                return;
            }

            sOperacao = "I";

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
        else
        {
            ClientScript.RegisterStartupScript(this.GetType(),
                "Obrigatorio", "alert('Todos os campos são de preenchimento obrigatório! Não foi possível incluir o usuário.'); ", true);
        }
    }

    private void GravarUsuario()
    {
        // Salva Primeiro cadastro da Pessoa F/J
        if ((txtNome.Text.ToString().Trim() != "") && (txtEmail.Text.ToString().Trim() != "") && (txtSenha.Text.ToString().Trim() != ""))
        {
            Funcoes.GravaAuditoria(0, Request.ServerVariables["REMOTE_ADDR"].ToString(), "Marketplaces - Usuários - Adm", "Gravar - ID: " + Funcoes.strToInt(sid_id.ToString()).ToString() + " - " + Request.ServerVariables["SERVER_NAME"].ToString());

            SqlConnection connInsCons = new SqlConnection(Funcoes.conexao());
            connInsCons.Open();
            SqlCommand cmdInsCons = new SqlCommand("dbo.stp_sis_usuario_ins", connInsCons);
            cmdInsCons.CommandType = CommandType.StoredProcedure;
            cmdInsCons.Parameters.Add("@flg_operacao", SqlDbType.Char).Value = 'I';
            cmdInsCons.Parameters.Add("@COD_ID_PESSOA_LICENCIADO", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["LICENCIADO"].ToString());
            cmdInsCons.Parameters.Add("@COD_ID_PESSOAS_FJ", SqlDbType.Int).Value = Funcoes.strToInt(sid_id.ToString());
            cmdInsCons.Parameters.Add("@FLG_ATIVO", SqlDbType.Char).Value = "N";
            cmdInsCons.Parameters.Add("@NOM_LOGIN", SqlDbType.VarChar).Value = txtEmail.Text.ToString();
            cmdInsCons.Parameters.Add("@NOM_SENHA", SqlDbType.VarChar).Value = txtSenha.Text.ToString();
            cmdInsCons.Parameters.Add("@NOM_NOME", SqlDbType.VarChar).Value = txtNome.Text.ToString();

            cmdInsCons.ExecuteNonQuery();
            connInsCons.Close();
            connInsCons.Dispose();

            ClientScript.RegisterStartupScript(this.GetType(),
                "Alerta", "alert('Dados gravados com sucesso');", true);
            ConsultaUsuarios();
        }
        else
        {
            ClientScript.RegisterStartupScript(this.GetType(),
                "Obrigatorio", "alert('Todos os campos são de preenchimento obrigatório! Não foi possível incluir o usuário.'); ", true);
        }

    }


    protected void btnCancelar_Click(object sender, EventArgs e)
    {
        ClientScript.RegisterStartupScript(this.GetType(),
            "Alerta", "opener.PostBackOnMainPage(); window.close(); ", true);
    }


    protected void rptConsulta_OnItemCommand(object source, RepeaterCommandEventArgs e)
    {
        sCodigo = Convert.ToString(e.CommandArgument);

        if (e.CommandName == "Excluir")
        {
            sOperacao = "E";
        }

        if (e.CommandName == "Ativo")
        {
            sOperacao = "A";
        }

        if (e.CommandName == "Acesso")
        {
            sOperacao = "L";
        }

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

    private void ExcluirUsuario()
    {
        SqlConnection connInsCons = new SqlConnection(Funcoes.conexao());
        SqlCommand cmdInsCons = new SqlCommand("dbo.stp_sis_usuario_ins", connInsCons);
        cmdInsCons.CommandType = CommandType.StoredProcedure;
        cmdInsCons.Parameters.Add("@flg_operacao", SqlDbType.Char).Value = 'E';
        cmdInsCons.Parameters.Add("@COD_ID", SqlDbType.Int).Value = Funcoes.strToInt(sCodigo.ToString());
        cmdInsCons.Parameters.Add("@COD_ID_PESSOA_LICENCIADO", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["LICENCIADO"].ToString());
        connInsCons.Open();
        cmdInsCons.ExecuteNonQuery();

        ClientScript.RegisterStartupScript(this.GetType(),
        "Usuarios", "alert('Registro excluído com sucesso');", true);

        ConsultaUsuarios();

    }

    private void AtivaInativarUsuario()
    {
        SqlConnection connInsCons = new SqlConnection(Funcoes.conexao());
        SqlCommand cmdInsCons = new SqlCommand("dbo.stp_sis_usuario_ins", connInsCons);
        cmdInsCons.CommandType = CommandType.StoredProcedure;
        cmdInsCons.Parameters.Add("@flg_operacao", SqlDbType.Char).Value = 'T';
        cmdInsCons.Parameters.Add("@COD_ID", SqlDbType.Int).Value = Funcoes.strToInt(sCodigo.ToString());
        cmdInsCons.Parameters.Add("@COD_ID_PESSOA_LICENCIADO", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["LICENCIADO"].ToString());
        connInsCons.Open();
        cmdInsCons.ExecuteNonQuery();

        ClientScript.RegisterStartupScript(this.GetType(),
        "Usuarios", "alert('Usuário Ativado/Inativado com sucesso');", true);

        ConsultaUsuarios();

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
            if (sOperacao.ToString().Trim() == "I")
            {
                GravarUsuario();
            }

            if (sOperacao.ToString().Trim() == "E")
            {
                ExcluirUsuario();
            }

            if (sOperacao.ToString().Trim() == "A")
            {
                AtivaInativarUsuario();
            }

            if (sOperacao.ToString().Trim() == "L")
            {
                EfetuarLogin();
            }

        }
        else
        {
            ClientScript.RegisterStartupScript(this.GetType(),
                "CodigoErrado2fa", "alert('O Código de confirmação de autenticação não está correto! Verifique e reentre.');", true);

        }
    }

    private void EfetuarLogin()
    {
        Funcoes.GravaAuditoria(0, Request.ServerVariables["REMOTE_ADDR"].ToString(), "Marketplace - Usuarios - Relogar", "Acesso - ID: " + sCodigo.ToString() + " - " + Request.ServerVariables["SERVER_NAME"].ToString());
        //string sAcessoLogin = (Funcoes.strToInt(sCodigo.ToString()) + 7985143).ToString();

        string sAcessoLogin = (Funcoes.strToInt(sCodigo.ToString()) + 3156789).ToString();
        string sAcessoMarketplace = (Funcoes.strToInt(HttpContext.Current.Session["LICENCIADO"].ToString()) + 3156789).ToString();

        ClientScript.RegisterStartupScript(this.GetType(),
            "Relogar", "window.open('https://" + Request.ServerVariables["SERVER_NAME"].ToString() + "/relogar.aspx?id=" + Funcoes.Encrypt(sAcessoLogin.ToString()) + "&li=" + Funcoes.Encrypt(sAcessoMarketplace.ToString()) + "', '_blank' );", true);


        //string sAcessoLogin = (Funcoes.strToInt(sCodigo.ToString()) + 3156789).ToString();
        //ClientScript.RegisterStartupScript(this.GetType(), "Relogar", "window.open('https://" + Request.ServerVariables["SERVER_NAME"].ToString() + "/relogar.aspx?id=" + Funcoes.Encrypt(sAcessoLogin.ToString()) + "', '_blank' );", true);
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


}