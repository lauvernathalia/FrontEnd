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
using System.Net.Mail;
using System.IO.IsolatedStorage;


public partial class cad_estabelecimentos_usuarios : System.Web.UI.Page
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

        // Verifica se o usuário esta autenticado
        if (HttpContext.Current.Session.Count <= 0)
        {
            Response.Redirect("login.aspx");
            FormsAuthentication.SignOut();
        }


        if (HttpContext.Current.Session["TIPO"].ToString() == "E")
        {
            Funcoes.GravaAuditoria(0, Request.ServerVariables["REMOTE_ADDR"].ToString(), "Cadastro - Estabelecimentos - Usuários", "Consulta - Tentativa acesso de usuário não autorizado");
            Response.Redirect("index.aspx");
        }

        if (!IsPostBack)
        {
            Funcoes.GravaAuditoria(0, Request.ServerVariables["REMOTE_ADDR"].ToString(), "Cadastro - Estabelecimentos - Usuários", "Acesso - ID: " + Funcoes.strToInt(sid_id.ToString()).ToString() + " - " + Request.ServerVariables["SERVER_NAME"].ToString());

            sOperacao = "";
            sUrlOrigem = "";
            sCodigo = "0";


            ConsultaFicha();
            ConsultaGeral();
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
            txtID.Text = ReaderCadastro["COD_ID"].ToString();
            txtRazaosocial.Text = ReaderCadastro["NOM_RAZAOSOCIAL"].ToString();
        }

        SqlConnection mySelCadastroLicenciado = new SqlConnection(Funcoes.conexao());
        mySelCadastroLicenciado.Open();
        SqlCommand cmdSelCadastroLicenciado = new SqlCommand("dbo.stp_pessoas_fj_licenciados_ins", mySelCadastroLicenciado);
        cmdSelCadastroLicenciado.CommandType = CommandType.StoredProcedure;
        cmdSelCadastroLicenciado.Parameters.Add("@flg_operacao", SqlDbType.Char).Value = "S";
        cmdSelCadastroLicenciado.Parameters.Add("@COD_ID", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["LICENCIADO"].ToString());
        SqlDataReader ReaderCadastroLicenciado = cmdSelCadastroLicenciado.ExecuteReader();
        while (ReaderCadastroLicenciado.Read())
        {
            txtURL.Text = ReaderCadastroLicenciado["NOM_URL"].ToString();
        }

    }


    private void ConsultaGeral()
    {
        SqlConnection myConsulta = new SqlConnection(Funcoes.conexao());
        myConsulta.Open();
        SqlDataAdapter SDAConsulta = new SqlDataAdapter("dbo.stp_sis_usuario_ins", myConsulta);
        SDAConsulta.SelectCommand.CommandType = CommandType.StoredProcedure;
        SDAConsulta.SelectCommand.Parameters.Add("@flg_operacao", SqlDbType.Char).Value = "C";
        SDAConsulta.SelectCommand.Parameters.Add("@COD_ID_PESSOA_LICENCIADO", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["LICENCIADO"].ToString());
        SDAConsulta.SelectCommand.Parameters.Add("@COD_ID_PESSOAS_FJ", SqlDbType.Int).Value = Funcoes.strToInt(sid_id.ToString());
        SDAConsulta.SelectCommand.Parameters.Add("@FLG_TIPO", SqlDbType.Char).Value = HttpContext.Current.Session["TIPO"].ToString();
        DataSet dsConsulta = new DataSet();
        SDAConsulta.Fill(dsConsulta, "PESSOAS_FJ");

        rptConsultaUsuario.DataSource = dsConsulta.Tables["PESSOAS_FJ"].DefaultView;
        rptConsultaUsuario.DataBind();
        
        myConsulta.Close(); myConsulta.Dispose();
    }

    protected void btnIncluir_Click(object sender, EventArgs e)
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

            if (Funcoes.Enviar2faCadastro() == true)
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

    private void GravarDados()
    {

        Funcoes.GravaAuditoria(0, Request.ServerVariables["REMOTE_ADDR"].ToString(), "Cadastro - Estabelecimentos - Usuários", "Gravar - ID: " + Funcoes.strToInt(sid_id.ToString()).ToString() + " - " + Request.ServerVariables["SERVER_NAME"].ToString());
        
        SqlConnection mySelCadastro = new SqlConnection(Funcoes.conexao());
        mySelCadastro.Open();
        SqlCommand cmdSelCadastro = new SqlCommand("dbo.stp_sis_usuario_ins", mySelCadastro);
        cmdSelCadastro.CommandType = CommandType.StoredProcedure;
        cmdSelCadastro.Parameters.Add("@flg_operacao", SqlDbType.Char).Value = "L";
        cmdSelCadastro.Parameters.Add("@COD_ID_PESSOA_LICENCIADO", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["LICENCIADO"].ToString());
        cmdSelCadastro.Parameters.Add("@NOM_LOGIN", SqlDbType.VarChar).Value = txtEmail.Text.ToString();
        cmdSelCadastro.Parameters.Add("@NOM_SENHA", SqlDbType.VarChar).Value = txtSenha.Text.ToString();

        SqlDataReader ReaderCadastro = cmdSelCadastro.ExecuteReader();
        while (ReaderCadastro.Read())
        {
            ClientScript.RegisterStartupScript(this.GetType(),
                "Alerta", "alert('Já existe outro usuário com este login e senha! Verifique e reentre.'); ", true);

            return;
        }
        // Salva Primeiro cadastro da Pessoa F/J
        if ((txtNome.Text.ToString().Trim() != "") && (txtEmail.Text.ToString().Trim() != "") && (txtSenha.Text.ToString().Trim() != ""))
        {
            SqlConnection connInsCons = new SqlConnection(Funcoes.conexao());
            connInsCons.Open();
            SqlCommand cmdInsCons = new SqlCommand("dbo.stp_sis_usuario_ins", connInsCons);
            cmdInsCons.CommandType = CommandType.StoredProcedure;
            cmdInsCons.Parameters.Add("@flg_operacao", SqlDbType.Char).Value = 'I';
            cmdInsCons.Parameters.Add("@COD_ID_PESSOA_LICENCIADO", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["LICENCIADO"].ToString());
            cmdInsCons.Parameters.Add("@COD_ID_PESSOAS_FJ", SqlDbType.Int).Value = Funcoes.strToInt(sid_id.ToString());

            cmdInsCons.Parameters.Add("@NOM_NOME", SqlDbType.VarChar).Value = txtNome.Text.ToString();
            cmdInsCons.Parameters.Add("@NOM_LOGIN", SqlDbType.VarChar).Value = txtEmail.Text.ToString();
            cmdInsCons.Parameters.Add("@NOM_SENHA", SqlDbType.VarChar).Value = txtSenha.Text.ToString();
            cmdInsCons.Parameters.Add("@FLG_ATIVO", SqlDbType.Char).Value = "S";

            cmdInsCons.ExecuteNonQuery();
            connInsCons.Close();
            connInsCons.Dispose();

            ClientScript.RegisterStartupScript(this.GetType(),
                "Alerta", "alert('Dados gravados com sucesso'); ", true);

            txtNome.Text = "";
            txtEmail.Text = "";
            txtSenha.Text = "";

            ConsultaGeral();
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
    protected void rptConsultaUsuario_ItemCommand(object source, RepeaterCommandEventArgs e)
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

        if (Funcoes.Enviar2faCadastro() == true)
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

        ConsultaGeral();

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

        ConsultaGeral();

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
                GravarDados();
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
        ClientScript.RegisterStartupScript(this.GetType(),
            "Relogar", "window.open('https://" + txtURL.Text.ToString() + "/relogar.aspx?id=" + Funcoes.Encrypt(sCodigo.ToString()) + "', '_blank' );", true);
    }

    protected void lkbReenviar_Click(object sender, System.EventArgs e)
    {
        if (Funcoes.Enviar2faCadastro() == true)
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