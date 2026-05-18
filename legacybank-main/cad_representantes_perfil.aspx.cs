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


public partial class cad_representantes_perfil : System.Web.UI.Page
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
            ConsultaTabelas();
            ConsultaFicha();
            ConsultaGeral();
        }

    }

    private void ConsultaTabelas()
    {
        SqlConnection myPerfil = new SqlConnection(Funcoes.conexao());
        myPerfil.Open();
        SqlCommand cmdPerfil = new SqlCommand("dbo.stp_perfil_ins", myPerfil);
        cmdPerfil.CommandType = CommandType.StoredProcedure;
        cmdPerfil.Parameters.Add("@flg_operacao", SqlDbType.VarChar).Value = "C";
        cmdPerfil.Parameters.Add("@COD_ID_PESSOA_LICENCIADO", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["LICENCIADO"].ToString());
        cmdPerfil.Parameters.Add("@FLG_ATIVO", SqlDbType.Char).Value = "S";
        cmdPerfil.Parameters.Add("@FLG_TIPO", SqlDbType.Char).Value = "R";
        SqlDataAdapter drPerfil = new SqlDataAdapter();
        drPerfil.SelectCommand = cmdPerfil;
        DataSet dsPerfil = new DataSet();
        drPerfil.Fill(dsPerfil, "PERFIL");
        ddlPerfil.DataTextField = "NOM_PERFIL";
        ddlPerfil.DataValueField = "COD_ID";
        ddlPerfil.DataSource = dsPerfil.Tables["PERFIL"].DefaultView;
        ddlPerfil.DataBind();
        ddlPerfil.Items.Insert(0, new ListItem("- Selecione o Perfil desejado ou deixe em branco para configurar manualmente -", "0"));
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
            ddlPerfil.SelectedValue = ReaderCadastro["COD_ID_PERFIL"].ToString();
        }

    }


    private void ConsultaGeral()
    {
        SqlConnection myConsulta = new SqlConnection(Funcoes.conexao());
        myConsulta.Open();
        SqlDataAdapter SDAConsulta = new SqlDataAdapter("dbo.stp_pessoas_fj_acessos_ins", myConsulta);
        SDAConsulta.SelectCommand.CommandType = CommandType.StoredProcedure;
        SDAConsulta.SelectCommand.Parameters.Add("@flg_operacao", SqlDbType.Char).Value = "L";
        SDAConsulta.SelectCommand.Parameters.Add("@COD_ID_PESSOA_LICENCIADO", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["LICENCIADO"].ToString());
        SDAConsulta.SelectCommand.Parameters.Add("@COD_ID_PESSOAS_FJ", SqlDbType.Int).Value = Funcoes.strToInt(sid_id.ToString());
        SDAConsulta.SelectCommand.Parameters.Add("@COD_ID_PERFIL", SqlDbType.Int).Value = Funcoes.strToInt(ddlPerfil.SelectedValue.ToString());
        SDAConsulta.SelectCommand.Parameters.Add("@FLG_TIPO", SqlDbType.Char).Value = "R";

        DataSet dsConsulta = new DataSet();
        SDAConsulta.Fill(dsConsulta, "PESSOAS_FJ_ACESSOS");
        rptConsultaAcessos.DataSource = dsConsulta.Tables["PESSOAS_FJ_ACESSOS"].DefaultView;
        rptConsultaAcessos.DataBind();
        myConsulta.Close(); myConsulta.Dispose();
    }

    protected void btnSalvar_Click(object sender, EventArgs e)
    {
        if (Funcoes.strToInt(ddlPerfil.SelectedValue.ToString()) > 0)
        {

            SqlConnection connInsConsUPD = new SqlConnection(Funcoes.conexao());
            connInsConsUPD.Open();
            SqlCommand cmdInsConsUPD = new SqlCommand("dbo.stp_pessoas_fj_acessos_ins", connInsConsUPD);
            cmdInsConsUPD.CommandType = CommandType.StoredProcedure;
            cmdInsConsUPD.Parameters.Add("@flg_operacao", SqlDbType.Char).Value = 'U';
            cmdInsConsUPD.Parameters.Add("@COD_ID_PESSOA_LICENCIADO", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["LICENCIADO"].ToString());
            cmdInsConsUPD.Parameters.Add("@COD_ID_PESSOAS_FJ", SqlDbType.Int).Value = Funcoes.strToInt(sid_id.ToString());
            cmdInsConsUPD.Parameters.Add("@COD_ID_PERFIL", SqlDbType.Int).Value = Funcoes.strToInt(ddlPerfil.SelectedValue.ToString());

            cmdInsConsUPD.ExecuteNonQuery();
            connInsConsUPD.Close();
            connInsConsUPD.Dispose();


            SqlConnection connInsCons = new SqlConnection(Funcoes.conexao());
            connInsCons.Open();
            SqlCommand cmdInsCons = new SqlCommand("dbo.stp_pessoas_fj_acessos_ins", connInsCons);
            cmdInsCons.CommandType = CommandType.StoredProcedure;
            cmdInsCons.Parameters.Add("@flg_operacao", SqlDbType.Char).Value = 'X';
            cmdInsCons.Parameters.Add("@COD_ID_PESSOA_LICENCIADO", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["LICENCIADO"].ToString());
            cmdInsCons.Parameters.Add("@COD_ID_PESSOAS_FJ", SqlDbType.Int).Value = Funcoes.strToInt(sid_id.ToString());
            cmdInsCons.Parameters.Add("@COD_ID_PERFIL", SqlDbType.Int).Value = Funcoes.strToInt(ddlPerfil.SelectedValue.ToString());

            cmdInsCons.ExecuteNonQuery();
            connInsCons.Close();
            connInsCons.Dispose();
        }

        ClientScript.RegisterStartupScript(this.GetType(),
        "Usuarios", "alert('Registros atualizados com sucesso');", true);
        ConsultaGeral();
    }

    protected void btnAtualizar_Click(object sender, EventArgs e)
    {
        foreach (RepeaterItem itemP in rptConsultaAcessos.Items)
        {
            SqlConnection connInsCons = new SqlConnection(Funcoes.conexao());
            SqlCommand cmdInsCons = new SqlCommand("dbo.stp_pessoas_fj_acessos_ins", connInsCons);
            cmdInsCons.CommandType = CommandType.StoredProcedure;
            cmdInsCons.Parameters.Add("@flg_operacao", SqlDbType.Char).Value = 'I';
            cmdInsCons.Parameters.Add("@COD_ID_PERFIL", SqlDbType.Int).Value = Funcoes.strToInt(((TextBox)itemP.FindControl("txtidperfil")).Text.ToString());
            cmdInsCons.Parameters.Add("@COD_ID_PESSOA_LICENCIADO", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["LICENCIADO"].ToString());
            cmdInsCons.Parameters.Add("@COD_ID_PESSOAS_FJ", SqlDbType.Int).Value = Funcoes.strToInt(sid_id.ToString());
            cmdInsCons.Parameters.Add("@COD_ID_SUBMENUS", SqlDbType.Int).Value = Funcoes.strToInt(((TextBox)itemP.FindControl("txtidsubmenu")).Text.ToString());
            cmdInsCons.Parameters.Add("@FLG_ACESSO", SqlDbType.Char).Value = ((TextBox)itemP.FindControl("txtidacesso")).Text.ToString();
            connInsCons.Open();
            cmdInsCons.ExecuteNonQuery();
        }

        ClientScript.RegisterStartupScript(this.GetType(),
        "Usuarios", "alert('Registros atualizados com sucesso');", true);
        ConsultaGeral();

    }
    
    protected void btnCancelar_Click(object sender, EventArgs e)
    {
        ClientScript.RegisterStartupScript(this.GetType(),
            "Alerta", "opener.PostBackOnMainPage(); window.close(); ", true);
    }
    protected void rptConsultaAcessos_ItemCommand(object source, RepeaterCommandEventArgs e)
    {
        if (e.CommandName == "Liberar")
        {
            foreach (RepeaterItem itemP in rptConsultaAcessos.Items)
            {
                if (Funcoes.strToInt(Convert.ToString(e.CommandArgument)) == Funcoes.strToInt(((TextBox)itemP.FindControl("txtidsubmenu")).Text.ToString()))
                {
                    SqlConnection connInsCons = new SqlConnection(Funcoes.conexao());
                    SqlCommand cmdInsCons = new SqlCommand("dbo.stp_pessoas_fj_acessos_ins", connInsCons);
                    cmdInsCons.CommandType = CommandType.StoredProcedure;
                    cmdInsCons.Parameters.Add("@flg_operacao", SqlDbType.Char).Value = 'I';
                    cmdInsCons.Parameters.Add("@COD_ID_PERFIL", SqlDbType.Int).Value = Funcoes.strToInt(((TextBox)itemP.FindControl("txtidperfil")).Text.ToString());
                    cmdInsCons.Parameters.Add("@COD_ID_PESSOA_LICENCIADO", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["LICENCIADO"].ToString());
                    cmdInsCons.Parameters.Add("@COD_ID_PESSOAS_FJ", SqlDbType.Int).Value = Funcoes.strToInt(sid_id.ToString());
                    cmdInsCons.Parameters.Add("@COD_ID_SUBMENUS", SqlDbType.Int).Value = Funcoes.strToInt(((TextBox)itemP.FindControl("txtidsubmenu")).Text.ToString());
                    cmdInsCons.Parameters.Add("@FLG_ACESSO", SqlDbType.Char).Value = "S";
                    connInsCons.Open();
                    cmdInsCons.ExecuteNonQuery();
                }

            }
            ConsultaGeral();

        }
        if (e.CommandName == "Bloquear")
        {
            foreach (RepeaterItem itemP in rptConsultaAcessos.Items)
            {
                if (Funcoes.strToInt(Convert.ToString(e.CommandArgument)) == Funcoes.strToInt(((TextBox)itemP.FindControl("txtidsubmenu")).Text.ToString()))
                {
                    SqlConnection connInsCons = new SqlConnection(Funcoes.conexao());
                    SqlCommand cmdInsCons = new SqlCommand("dbo.stp_pessoas_fj_acessos_ins", connInsCons);
                    cmdInsCons.CommandType = CommandType.StoredProcedure;
                    cmdInsCons.Parameters.Add("@flg_operacao", SqlDbType.Char).Value = 'I';
                    cmdInsCons.Parameters.Add("@COD_ID_PERFIL", SqlDbType.Int).Value = Funcoes.strToInt(((TextBox)itemP.FindControl("txtidperfil")).Text.ToString());
                    cmdInsCons.Parameters.Add("@COD_ID_PESSOA_LICENCIADO", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["LICENCIADO"].ToString());
                    cmdInsCons.Parameters.Add("@COD_ID_PESSOAS_FJ", SqlDbType.Int).Value = Funcoes.strToInt(sid_id.ToString());
                    cmdInsCons.Parameters.Add("@COD_ID_SUBMENUS", SqlDbType.Int).Value = Funcoes.strToInt(((TextBox)itemP.FindControl("txtidsubmenu")).Text.ToString());
                    cmdInsCons.Parameters.Add("@FLG_ACESSO", SqlDbType.Char).Value = "N";
                    connInsCons.Open();
                    cmdInsCons.ExecuteNonQuery();
                }

            }
            ConsultaGeral();

        }

    }
}