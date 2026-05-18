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


public partial class con_marketplaces_estabelecimentos_adm : System.Web.UI.Page
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
            SqlConnection myEstabelecimentos = new SqlConnection(Funcoes.conexao());
            myEstabelecimentos.Open();
            SqlCommand cmdEstabelecimentos = new SqlCommand("dbo.stp_pessoas_fj_ins", myEstabelecimentos);
            cmdEstabelecimentos.CommandType = CommandType.StoredProcedure;
            cmdEstabelecimentos.Parameters.Add("@flg_operacao", SqlDbType.VarChar).Value = "L";
            cmdEstabelecimentos.Parameters.Add("@COD_ID_PESSOA_LICENCIADO", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["LICENCIADO"].ToString());
            cmdEstabelecimentos.Parameters.Add("@FLG_ATIVO", SqlDbType.Char).Value = "S";
            cmdEstabelecimentos.Parameters.Add("@FLG_TIPO", SqlDbType.Char).Value = "E";
            SqlDataAdapter drEstabelecimentos = new SqlDataAdapter();
            drEstabelecimentos.SelectCommand = cmdEstabelecimentos;
            DataSet dsEstabelecimentos = new DataSet();
            drEstabelecimentos.Fill(dsEstabelecimentos, "PESSOAS_FJ");
            ddlEstabelecimentos.DataTextField = "NOM_RAZAOSOCIAL";
            ddlEstabelecimentos.DataValueField = "COD_ID";
            ddlEstabelecimentos.DataSource = dsEstabelecimentos.Tables["PESSOAS_FJ"].DefaultView;
            ddlEstabelecimentos.DataBind();
            ddlEstabelecimentos.Items.Insert(0, new ListItem("", "0"));

            if (Funcoes.strToInt(sid_id) != 0)
            {
                ConsultaFicha();
                ConsultaEstabelecimentos();
            }
        }

    }

    private void ConsultaEstabelecimentos()
    {
        //dtgConsulta.Visible = true;
        SqlConnection myConsulta = new SqlConnection(Funcoes.conexao());
        myConsulta.Open();
        SqlDataAdapter SDAConsulta = new SqlDataAdapter("dbo.stp_pessoas_fj_Estabelecimentos_ins", myConsulta);
        SDAConsulta.SelectCommand.CommandType = CommandType.StoredProcedure;
        SDAConsulta.SelectCommand.Parameters.Add("@flg_operacao", SqlDbType.Char).Value = "L";
        SDAConsulta.SelectCommand.Parameters.Add("@COD_ID_PESSOA_LICENCIADO", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["LICENCIADO"].ToString());
        SDAConsulta.SelectCommand.Parameters.Add("@COD_ID_MARKETPLACE", SqlDbType.Int).Value = Funcoes.strToInt(sid_id.ToString());
        DataSet dsConsulta = new DataSet();
        SDAConsulta.Fill(dsConsulta, "PESSOAS_FJ");
        rptConsultaEstabelecimentos.DataSource = dsConsulta.Tables["PESSOAS_FJ"].DefaultView;
        rptConsultaEstabelecimentos.DataBind();
        myConsulta.Close(); myConsulta.Dispose();
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


    protected void btnCancelar_Click(object sender, EventArgs e)
    {
        ClientScript.RegisterStartupScript(this.GetType(),
            "Alerta", "opener.PostBackOnMainPage(); window.close(); ", true);
    }


    protected void btnIncluirEstabelecimentos_Click(object sender, EventArgs e)
    {
        SqlConnection connInsCons = new SqlConnection(Funcoes.conexao());
        connInsCons.Open();
        SqlCommand cmdInsCons = new SqlCommand("dbo.stp_pessoas_fj_Estabelecimentos_ins", connInsCons);
        cmdInsCons.CommandType = CommandType.StoredProcedure;
        cmdInsCons.Parameters.Add("@flg_operacao", SqlDbType.Char).Value = 'M';
        cmdInsCons.Parameters.Add("@FLG_RELACAO", SqlDbType.Char).Value = "M";
        cmdInsCons.Parameters.Add("@COD_ID_PESSOA_LICENCIADO", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["LICENCIADO"].ToString());
        cmdInsCons.Parameters.Add("@COD_ID_MARKETPLACE", SqlDbType.Int).Value = Funcoes.strToInt(sid_id.ToString());
        cmdInsCons.Parameters.Add("@COD_ID_PESSOAS_FJ", SqlDbType.Int).Value = Funcoes.strToInt(ddlEstabelecimentos.SelectedValue.ToString());
        cmdInsCons.ExecuteNonQuery();
        connInsCons.Close();
        connInsCons.Dispose();

        ConsultaEstabelecimentos();

    }

    protected void rptConsultaEstabelecimentos_ItemCommand(object source, RepeaterCommandEventArgs e)
    {
        if (e.CommandName == "Excluir")
        {
            SqlConnection connInsCons = new SqlConnection(Funcoes.conexao());
            connInsCons.Open();
            SqlCommand cmdInsCons = new SqlCommand("dbo.stp_pessoas_fj_Estabelecimentos_ins", connInsCons);
            cmdInsCons.CommandType = CommandType.StoredProcedure;
            cmdInsCons.Parameters.Add("@flg_operacao", SqlDbType.Char).Value = 'E';
            cmdInsCons.Parameters.Add("@FLG_RELACAO", SqlDbType.Char).Value = "M";
            cmdInsCons.Parameters.Add("@COD_ID_PESSOAS_FJ", SqlDbType.Int).Value = Funcoes.strToInt(Convert.ToString(e.CommandArgument));
            cmdInsCons.Parameters.Add("@COD_ID_PESSOA_LICENCIADO", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["LICENCIADO"].ToString());
            cmdInsCons.ExecuteNonQuery();
            connInsCons.Close();
            connInsCons.Dispose();

            ConsultaEstabelecimentos();

        }

    }

}