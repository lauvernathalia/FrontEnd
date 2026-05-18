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


public partial class con_representantes_usuarios_mkt : System.Web.UI.Page
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
        if (HttpContext.Current.Session.Count <= 0)
        {
            Response.Redirect("login.aspx");
            FormsAuthentication.SignOut();
        }

        if ((faVerificadoSenha.Visible) == true && (faVerificadoEmail.Visible == true))
        {
            btnSalvar.Visible = true;
        }
        else
        {
            btnSalvar.Visible = true;

        }


        if (!IsPostBack)
        {
            Funcoes.GravaAuditoria(0, Request.ServerVariables["REMOTE_ADDR"].ToString(), "Representantes - Usuários - Mkt", "Acesso - ID: " + Funcoes.strToInt(sid_id.ToString()).ToString() + " - " + Request.ServerVariables["SERVER_NAME"].ToString());


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
        Funcoes.GravaAuditoria(0, Request.ServerVariables["REMOTE_ADDR"].ToString(), "Representantes - Usuários - Mkt", "Gravar - ID: " + Funcoes.strToInt(sid_id.ToString()).ToString() + " - " + Request.ServerVariables["SERVER_NAME"].ToString());

        SqlConnection connInsCons = new SqlConnection(Funcoes.conexao());
        connInsCons.Open();
        SqlCommand cmdInsCons = new SqlCommand("dbo.stp_sis_usuario_ins", connInsCons);
        cmdInsCons.CommandType = CommandType.StoredProcedure;
        cmdInsCons.Parameters.Add("@flg_operacao", SqlDbType.Char).Value = 'I';
        cmdInsCons.Parameters.Add("@COD_ID_PESSOA_LICENCIADO", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["LICENCIADO"].ToString());
        cmdInsCons.Parameters.Add("@COD_ID_PESSOAS_FJ", SqlDbType.Int).Value = Funcoes.strToInt(sid_id.ToString());
        cmdInsCons.Parameters.Add("@FLG_ATIVO", SqlDbType.Char).Value = ddlAtivo.SelectedValue.ToString();
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
    protected void btnCancelar_Click(object sender, EventArgs e)
    {
        ClientScript.RegisterStartupScript(this.GetType(),
            "Alerta", "opener.PostBackOnMainPage(); window.close(); ", true);
    }

    protected void txtSenhaC_TextChanged(object sender, EventArgs e)
    {
        if (txtSenha.Text.ToString() == txtSenhaC.Text.ToString())
        {
            faVerificadoSenha.Visible = true;
        }
        else
        {
            faVerificadoSenha.Visible = false;
        }
    }
    protected void txtEmailC_TextChanged(object sender, EventArgs e)
    {
        if (txtEmail.Text.ToString() == txtEmailC.Text.ToString())
        {
            faVerificadoEmail.Visible = true;
        }
        else
        {
            faVerificadoEmail.Visible = false;
        }

    }
    protected void rptConsulta_OnItemCommand(object source, RepeaterCommandEventArgs e)
    {
        if (e.CommandName == "Ativo")
        {

            SqlConnection connInsCons = new SqlConnection(Funcoes.conexao());
            connInsCons.Open();
            SqlCommand cmdInsCons = new SqlCommand("dbo.stp_sis_usuario_ins", connInsCons);
            cmdInsCons.CommandType = CommandType.StoredProcedure;
            cmdInsCons.Parameters.Add("@flg_operacao", SqlDbType.Char).Value = "T";
            cmdInsCons.Parameters.Add("@COD_ID", SqlDbType.Int).Value = Funcoes.strToInt(Convert.ToString(e.CommandArgument));
            cmdInsCons.Parameters.Add("@COD_ID_PESSOA_LICENCIADO", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["LICENCIADO"].ToString());
            cmdInsCons.ExecuteNonQuery();
            connInsCons.Close();
            connInsCons.Dispose();

            ClientScript.RegisterStartupScript(this.GetType(),
                "Alerta", "alert('Usuário Ativado/Inativado com sucesso!');", true);
        }
        if (e.CommandName == "Excluir")
        {

            SqlConnection connInsCons = new SqlConnection(Funcoes.conexao());
            connInsCons.Open();
            SqlCommand cmdInsCons = new SqlCommand("dbo.stp_sis_usuario_ins", connInsCons);
            cmdInsCons.CommandType = CommandType.StoredProcedure;
            cmdInsCons.Parameters.Add("@flg_operacao", SqlDbType.Char).Value = "E";
            cmdInsCons.Parameters.Add("@COD_ID", SqlDbType.Int).Value = Funcoes.strToInt(Convert.ToString(e.CommandArgument));
            cmdInsCons.Parameters.Add("@COD_ID_PESSOA_LICENCIADO", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["LICENCIADO"].ToString());

            cmdInsCons.ExecuteNonQuery();
            connInsCons.Close();
            connInsCons.Dispose();

            ClientScript.RegisterStartupScript(this.GetType(),
                "Alerta", "alert('Usuário Excluído com sucesso!');", true);
        }

        ConsultaUsuarios();

    }
}