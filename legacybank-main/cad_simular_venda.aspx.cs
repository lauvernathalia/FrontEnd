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

public partial class cad_simular_venda : System.Web.UI.Page
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
            // Carrega Plano do Estabelecimento
            SqlConnection myPlano = new SqlConnection(Funcoes.conexao());
            myPlano.Open();
            SqlCommand cmdPlano = new SqlCommand("dbo.stp_planos_ins", myPlano);
            cmdPlano.CommandType = CommandType.StoredProcedure;
            cmdPlano.Parameters.Add("@flg_operacao", SqlDbType.VarChar).Value = "L";
            cmdPlano.Parameters.Add("@FLG_ATIVO", SqlDbType.VarChar).Value = "S";
            cmdPlano.Parameters.Add("@COD_ID_PESSOA_LICENCIADO", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["LICENCIADO"].ToString());
            SqlDataAdapter drPlano = new SqlDataAdapter();
            drPlano.SelectCommand = cmdPlano;
            DataSet dsPlano = new DataSet();
            drPlano.Fill(dsPlano, "PLANOS");
            ddlPlano.DataTextField = "NOM_TITULO_PLANO";
            ddlPlano.DataValueField = "COD_ID";
            ddlPlano.DataSource = dsPlano.Tables["PLANOS"].DefaultView;
            ddlPlano.DataBind();
            ddlPlano.Items.Insert(0, new ListItem("", "0"));

            // Carrega lista Bandeiras
            SqlConnection myBandeira = new SqlConnection(Funcoes.conexao());
            myBandeira.Open();
            SqlCommand cmdBandeira = new SqlCommand("dbo.stp_bandeiras_ins", myBandeira);
            cmdBandeira.CommandType = CommandType.StoredProcedure;
            cmdBandeira.Parameters.Add("@flg_operacao", SqlDbType.VarChar).Value = "C";
            cmdBandeira.Parameters.Add("@FLG_ATIVO", SqlDbType.VarChar).Value = "S";
            cmdBandeira.Parameters.Add("@FLG_TIPO", SqlDbType.VarChar).Value = "C";
            SqlDataAdapter drBandeira = new SqlDataAdapter();
            drBandeira.SelectCommand = cmdBandeira;
            DataSet dsBandeira = new DataSet();
            drBandeira.Fill(dsBandeira, "BANDEIRAS");
            ddlBandeira.DataTextField = "NOM_BANDEIRA";
            ddlBandeira.DataValueField = "COD_ID";
            ddlBandeira.DataSource = dsBandeira.Tables["BANDEIRAS"].DefaultView;
            ddlBandeira.DataBind();

            SqlConnection mySelCadastro = new SqlConnection(Funcoes.conexao());
            mySelCadastro.Open();
            SqlCommand cmdSelCadastro = new SqlCommand("dbo.stp_pessoas_fj_ins", mySelCadastro);
            cmdSelCadastro.CommandType = CommandType.StoredProcedure;
            cmdSelCadastro.Parameters.Add("@flg_operacao", SqlDbType.Char).Value = "S";
            cmdSelCadastro.Parameters.Add("@COD_ID", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["PESSOA"].ToString());
            cmdSelCadastro.Parameters.Add("@COD_ID_PESSOA_LICENCIADO", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["LICENCIADO"].ToString());
            SqlDataReader ReaderCadastro = cmdSelCadastro.ExecuteReader();
            while (ReaderCadastro.Read())
            {
                ddlPlano.SelectedValue = ReaderCadastro["COD_ID_PLANO_ZOOP"].ToString();
                ddlPlano.Enabled = false;
            }


            if (Funcoes.strToInt(sid_id) != 0)
            {
                btnIncluir.Visible = false;
                btnAtualizar.Visible = true;
                ConsultaFicha();
            }
            else
            {
                btnIncluir.Visible = true;
                btnAtualizar.Visible = false;
            }
        }
    }

    private void ConsultaFicha()
    {
        SqlConnection mySelCadastro = new SqlConnection(Funcoes.conexao());
        mySelCadastro.Open();
        SqlCommand cmdSelCadastro = new SqlCommand("dbo.stp_simulador_venda_ins", mySelCadastro);
        cmdSelCadastro.CommandType = CommandType.StoredProcedure;
        cmdSelCadastro.Parameters.Add("@flg_operacao", SqlDbType.Char).Value = "S";
        cmdSelCadastro.Parameters.Add("@COD_ID", SqlDbType.Int).Value = Funcoes.strToInt(sid_id.ToString());
        cmdSelCadastro.Parameters.Add("@COD_ID_PESSOA_LICENCIADO", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["LICENCIADO"].ToString());
        SqlDataReader ReaderCadastro = cmdSelCadastro.ExecuteReader();
        while (ReaderCadastro.Read())
        {
            txtID.Text = ReaderCadastro["COD_ID"].ToString();
            
            txtSimulacao.Text = ReaderCadastro["NOM_SIMULAR_VENDAS"].ToString();
            ddlModalidade.SelectedValue = ReaderCadastro["FLG_MODALIDADE"].ToString();
            ddlBandeira.SelectedValue = ReaderCadastro["COD_ID_BANDEIRA"].ToString();
            if (ReaderCadastro["FLG_JUROS"].ToString() == "S")
            {
                ckbJurosCliente.Checked = true;
            }
            else
            {
                ckbJurosCliente.Checked = false;
            }
            txtValor.Text = ReaderCadastro["NUM_VALOR"].ToString();

            CalcularTaxas();

        }
    }

    private void GravarDados()
    {
        SqlConnection connInsCons = new SqlConnection(Funcoes.conexao());
        connInsCons.Open();
        SqlCommand cmdInsCons = new SqlCommand("dbo.stp_simulador_venda_ins", connInsCons);
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

        cmdInsCons.Parameters.Add("@NOM_SIMULAR_VENDAS", SqlDbType.VarChar).Value = txtSimulacao.Text.ToString();
        cmdInsCons.Parameters.Add("@FLG_MODALIDADE", SqlDbType.VarChar).Value = ddlModalidade.SelectedValue.ToString();
        cmdInsCons.Parameters.Add("@COD_ID_BANDEIRA", SqlDbType.VarChar).Value = Funcoes.strToInt(ddlBandeira.SelectedValue.ToString());

        if (ckbJurosCliente.Checked==true)
        {
            cmdInsCons.Parameters.Add("@FLG_JUROS", SqlDbType.Char).Value = "S";
        }
        else
        {
            cmdInsCons.Parameters.Add("@FLG_JUROS", SqlDbType.Char).Value = "N";
        }

        cmdInsCons.Parameters.Add("@NUM_VALOR", SqlDbType.Float).Value = Funcoes.strToDouble(txtValor.Text.ToString());

        cmdInsCons.ExecuteNonQuery();
        connInsCons.Close();
        connInsCons.Dispose();

        ClientScript.RegisterStartupScript(this.GetType(),
            "Alerta", "alert('Dados gravados com sucesso'); ", true);
    }
    
    protected void btnIncluir_Click(object sender, EventArgs e)
    {
        GravarDados();
        CalcularTaxas();
    }
    protected void btnCancelar_Click(object sender, EventArgs e)
    {
        ClientScript.RegisterStartupScript(this.GetType(),
    "Alerta", "opener.PostBackOnMainPage(); window.close(); ", true);

    }
    protected void btnAtualizar_Click(object sender, EventArgs e)
    {
        GravarDados();
        CalcularTaxas();
    }

    private void CalcularTaxas()
    {


        SqlConnection myConsulta = new SqlConnection(Funcoes.conexao());
        myConsulta.Open();
        SqlDataAdapter SDAConsulta = new SqlDataAdapter("dbo.stp_simulador_venda_ins", myConsulta);
        SDAConsulta.SelectCommand.CommandType = CommandType.StoredProcedure;
        SDAConsulta.SelectCommand.Parameters.Add("@flg_operacao", SqlDbType.Char).Value = "L";
        SDAConsulta.SelectCommand.Parameters.Add("@COD_ID_PESSOAS_FJ", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["PESSOA"].ToString());
        SDAConsulta.SelectCommand.Parameters.Add("@COD_ID_PESSOA_LICENCIADO", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["LICENCIADO"].ToString());
        if (ckbJurosCliente.Checked == true) { SDAConsulta.SelectCommand.Parameters.Add("@FLG_JUROS", SqlDbType.Char).Value = "S"; } else { { SDAConsulta.SelectCommand.Parameters.Add("@FLG_JUROS", SqlDbType.Char).Value = "N"; } }

        SDAConsulta.SelectCommand.Parameters.Add("@COD_ID_PLANOS", SqlDbType.Int).Value = Funcoes.strToInt(ddlPlano.SelectedValue.ToString());
        SDAConsulta.SelectCommand.Parameters.Add("@COD_ID_BANDEIRA", SqlDbType.Int).Value = Funcoes.strToInt(ddlBandeira.SelectedValue.ToString());
        SDAConsulta.SelectCommand.Parameters.Add("@NUM_VALOR", SqlDbType.Float).Value = Funcoes.strToDouble(txtValor.Text.ToString());
        SDAConsulta.SelectCommand.Parameters.Add("@FLG_PRESENCIAL_ONLINE", SqlDbType.Char).Value = ddlModalidade.SelectedValue.ToString();

        DataSet dsConsulta = new DataSet();
        SDAConsulta.Fill(dsConsulta, "PLANOS_PARCELAS");

        rptConsulta.DataSource = dsConsulta.Tables["PLANOS_PARCELAS"].DefaultView;
        rptConsulta.DataBind();

        myConsulta.Close(); myConsulta.Dispose();
    }
}