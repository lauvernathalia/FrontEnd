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


public partial class cad_usuarios_adm : System.Web.UI.Page
{
    public static string Password;
    public static string PasswordC;

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
        if (HttpContext.Current.Session.Count <= 0)
        {
            Response.Redirect("login.aspx");
            FormsAuthentication.SignOut();
        }

        if (!IsPostBack)
        {

            Funcoes.GravaAuditoria(0, Request.ServerVariables["REMOTE_ADDR"].ToString(), "Usuários Adm - Usuários", "Acesso - ID: " + Funcoes.strToInt(sid_id.ToString()).ToString() + " - " + Request.ServerVariables["SERVER_NAME"].ToString());

            txtSenhaConfirmar.Attributes.Add("onfocus", "this.type='text';");
            txtSenhaConfirmar.Attributes.Add("onblur", "this.type='password';");
            txtSenha.Attributes.Add("onfocus", "this.type='text';");
            txtSenha.Attributes.Add("onblur", "this.type='password';");

            if (Funcoes.strToInt(sid_id) != 0)
            {
                ConsultaFicha();

            }
        }

        if (txtSenha.Text.ToString().Trim().Length > 0)
        {
            txtSenha.Attributes["value"] = txtSenha.Text;
            txtSenhaConfirmar.Attributes["value"] = txtSenhaConfirmar.Text;
            Password = txtSenha.Text.ToString();
        }

    }

    private void ConsultaFicha()
    {
        SqlConnection mySelCadastro = new SqlConnection(Funcoes.conexao());
        mySelCadastro.Open();
        SqlCommand cmdSelCadastro = new SqlCommand("dbo.stp_sis_usuario_ins", mySelCadastro);
        cmdSelCadastro.CommandType = CommandType.StoredProcedure;
        cmdSelCadastro.Parameters.Add("@flg_operacao", SqlDbType.Char).Value = "S";
        cmdSelCadastro.Parameters.Add("@COD_ID", SqlDbType.Int).Value = Funcoes.strToInt(sid_id.ToString());
        cmdSelCadastro.Parameters.Add("@COD_ID_PESSOAS_FJ", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["PESSOA"].ToString());
        cmdSelCadastro.Parameters.Add("@COD_ID_PESSOA_LICENCIADO", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["LICENCIADO"].ToString());
        SqlDataReader ReaderCadastro = cmdSelCadastro.ExecuteReader();
        while (ReaderCadastro.Read())
        {
            txtNome.Text = ReaderCadastro["NOM_NOME"].ToString();
            txtEmail.Text = ReaderCadastro["NOM_LOGIN"].ToString();
            txtSenha.Text = ReaderCadastro["NOM_SENHA"].ToString();
            txtSenhaConfirmar.Text = ReaderCadastro["NOM_SENHA"].ToString();
            ddlAtivo.SelectedValue = ReaderCadastro["FLG_ATIVO"].ToString();
        }

    }
    protected void txtSenhaConfirmar_TextChanged(object sender, EventArgs e)
    {
        if (txtSenha.Text.ToString() != txtSenhaConfirmar.Text.ToString())
        {
            ClientScript.RegisterStartupScript(this.GetType(),
    "Senhas", "alert('As senhas não são iguais. Verifique e reentre!'); ", true);

        }
    }
    protected void btnSalvar_Click(object sender, EventArgs e)
    {
        if ((txtSenha.Text.ToString().Trim() != "") && (txtSenha.Text.ToString() != txtSenhaConfirmar.Text.ToString()))
        {
            ClientScript.RegisterStartupScript(this.GetType(),
"Senhas", "alert('As senhas não são iguais. Verifique e reentre!'); ", true);

        }
        else
        {
            // Salva Primeiro cadastro da Pessoa F/J
            Funcoes.GravaAuditoria(0, Request.ServerVariables["REMOTE_ADDR"].ToString(), "Usuários Adm - Usuários", "Gravar - ID: " + Funcoes.strToInt(sid_id.ToString()).ToString() + " - " + Request.ServerVariables["SERVER_NAME"].ToString());

            SqlConnection connInsCons = new SqlConnection(Funcoes.conexao());
            connInsCons.Open();
            SqlCommand cmdInsCons = new SqlCommand("dbo.stp_sis_usuario_ins", connInsCons);
            cmdInsCons.CommandType = CommandType.StoredProcedure;
            if (Funcoes.strToInt(sid_id) != 0)
            {
                cmdInsCons.Parameters.Add("@flg_operacao", SqlDbType.Char).Value = 'A';
                cmdInsCons.Parameters.Add("@COD_ID", SqlDbType.Int).Value = Funcoes.strToInt(sid_id.ToString());
            }
            else
            {
                cmdInsCons.Parameters.Add("@flg_operacao", SqlDbType.Char).Value = 'I';
            }
            cmdInsCons.Parameters.Add("@COD_ID_PESSOA_LICENCIADO", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["LICENCIADO"].ToString());
            cmdInsCons.Parameters.Add("@COD_ID_PESSOAS_FJ", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["PESSOA"].ToString());

            cmdInsCons.Parameters.Add("@FLG_ATIVO", SqlDbType.Char).Value = ddlAtivo.SelectedValue.ToString();

            cmdInsCons.Parameters.Add("@NOM_NOME", SqlDbType.VarChar).Value = txtNome.Text.ToString();
            cmdInsCons.Parameters.Add("@NOM_LOGIN", SqlDbType.VarChar).Value = txtEmail.Text.ToString();
            cmdInsCons.Parameters.Add("@NOM_SENHA", SqlDbType.VarChar).Value = txtSenha.Text.ToString();
            cmdInsCons.ExecuteNonQuery();
            connInsCons.Close();
            connInsCons.Dispose();

            ClientScript.RegisterStartupScript(this.GetType(),
                "Alerta", "alert('Dados gravados com sucesso'); opener.PostBackOnMainPage();", true);
        }
    }
}