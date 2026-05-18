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

public partial class cad_licenciados_planos_referencia : System.Web.UI.Page
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
        if (HttpContext.Current.Session.Count <= 0)
        {
            Response.Redirect("login.aspx");
            FormsAuthentication.SignOut();
        }

        if (!IsPostBack)
        {
            Funcoes.GravaAuditoria(Funcoes.strToInt(sid_id), Request.ServerVariables["REMOTE_ADDR"].ToString(), "Licenciados - Planos Base - Acesso", "Acesso");

            ConsultaFicha();
            ConsultaGeral();

        }
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
        }

    }

    private void ConsultaGeral()
    {
        SqlConnection myConsulta = new SqlConnection(Funcoes.conexao());
        myConsulta.Open();
        SqlDataAdapter SDAConsulta = new SqlDataAdapter("dbo.stp_pessoas_fj_planos_referencia_ins", myConsulta);
        SDAConsulta.SelectCommand.CommandType = CommandType.StoredProcedure;
        SDAConsulta.SelectCommand.Parameters.Add("@flg_operacao", SqlDbType.Char).Value = "L";
        SDAConsulta.SelectCommand.Parameters.Add("@COD_ID_PESSOA_LICENCIADO", SqlDbType.Int).Value = Funcoes.strToInt(txtCodigo.Text.ToString());
        SDAConsulta.SelectCommand.Parameters.Add("@COD_ID_PESSOAS_FJ", SqlDbType.Int).Value = Funcoes.strToInt(txtID.Text.ToString());

        SDAConsulta.SelectCommand.Parameters.Add("@COD_ID_PESSOA_LICENCIADO_ADM", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["LICENCIADO"].ToString());

        DataSet dsConsulta = new DataSet();
        SDAConsulta.Fill(dsConsulta, "PESSOAS_FJ_PLANOS_REFERENCIA");
        rptConsultaAcessos.DataSource = dsConsulta.Tables["PESSOAS_FJ_PLANOS_REFERENCIA"].DefaultView;
        rptConsultaAcessos.DataBind();
        myConsulta.Close(); myConsulta.Dispose();

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
                if (Funcoes.strToInt(Convert.ToString(e.CommandArgument)) == Funcoes.strToInt(((TextBox)itemP.FindControl("txtidplanosreferencia")).Text.ToString()))
                {
                    SqlConnection connInsCons = new SqlConnection(Funcoes.conexao());
                    SqlCommand cmdInsCons = new SqlCommand("dbo.stp_pessoas_fj_planos_referencia_ins", connInsCons);
                    cmdInsCons.CommandType = CommandType.StoredProcedure;
                    cmdInsCons.Parameters.Add("@flg_operacao", SqlDbType.Char).Value = 'I';
                    cmdInsCons.Parameters.Add("@COD_ID_PESSOAS_FJ", SqlDbType.Int).Value = Funcoes.strToInt(txtID.Text.ToString());
                    cmdInsCons.Parameters.Add("@COD_ID_PESSOA_LICENCIADO", SqlDbType.Int).Value = Funcoes.strToInt(txtCodigo.Text.ToString());
                    cmdInsCons.Parameters.Add("@COD_ID_PLANOS_REFERENCIA", SqlDbType.Int).Value = Funcoes.strToInt(((TextBox)itemP.FindControl("txtidplanosreferencia")).Text.ToString());
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
                if (Funcoes.strToInt(Convert.ToString(e.CommandArgument)) == Funcoes.strToInt(((TextBox)itemP.FindControl("txtidplanosreferencia")).Text.ToString()))
                {
                    SqlConnection connInsCons = new SqlConnection(Funcoes.conexao());
                    SqlCommand cmdInsCons = new SqlCommand("dbo.stp_pessoas_fj_planos_referencia_ins", connInsCons);
                    cmdInsCons.CommandType = CommandType.StoredProcedure;
                    cmdInsCons.Parameters.Add("@flg_operacao", SqlDbType.Char).Value = 'I';
                    cmdInsCons.Parameters.Add("@COD_ID_PESSOAS_FJ", SqlDbType.Int).Value = Funcoes.strToInt(txtID.Text.ToString());
                    cmdInsCons.Parameters.Add("@COD_ID_PESSOA_LICENCIADO", SqlDbType.Int).Value = Funcoes.strToInt(txtCodigo.Text.ToString());
                    cmdInsCons.Parameters.Add("@COD_ID_PLANOS_REFERENCIA", SqlDbType.Int).Value = Funcoes.strToInt(((TextBox)itemP.FindControl("txtidplanosreferencia")).Text.ToString());
                    cmdInsCons.Parameters.Add("@FLG_ACESSO", SqlDbType.Char).Value = "N";
                    connInsCons.Open();
                    cmdInsCons.ExecuteNonQuery();
                }

            }
            ConsultaGeral();

        }

    }
    protected void btnSalvar_Click(object sender, EventArgs e)
    {

        Funcoes.GravaAuditoria(Funcoes.strToInt(sid_id), Request.ServerVariables["REMOTE_ADDR"].ToString(), "Licenciados - Planos Base - Acesso", "Salvar");

        foreach (RepeaterItem itemP in rptConsultaAcessos.Items)
        {
            SqlConnection connInsCons = new SqlConnection(Funcoes.conexao());
            SqlCommand cmdInsCons = new SqlCommand("dbo.stp_pessoas_fj_planos_referencia_ins", connInsCons);
            cmdInsCons.CommandType = CommandType.StoredProcedure;
            cmdInsCons.Parameters.Add("@flg_operacao", SqlDbType.Char).Value = 'I';
            cmdInsCons.Parameters.Add("@COD_ID_PESSOAS_FJ", SqlDbType.Int).Value = Funcoes.strToInt(txtID.Text.ToString());
            cmdInsCons.Parameters.Add("@COD_ID_PESSOA_LICENCIADO", SqlDbType.Int).Value = Funcoes.strToInt(txtCodigo.Text.ToString());
            cmdInsCons.Parameters.Add("@COD_ID_PLANOS_REFERENCIA", SqlDbType.Int).Value = Funcoes.strToInt(((TextBox)itemP.FindControl("txtidplanosreferencia")).Text.ToString());
            cmdInsCons.Parameters.Add("@FLG_ACESSO", SqlDbType.Char).Value = ((TextBox)itemP.FindControl("txtacesso")).Text.ToString();
            connInsCons.Open();
            cmdInsCons.ExecuteNonQuery();
        }
        ClientScript.RegisterStartupScript(this.GetType(),
        "Usuarios", "alert('Registro atualizado com sucesso');", true);
        ConsultaGeral();

    }
}