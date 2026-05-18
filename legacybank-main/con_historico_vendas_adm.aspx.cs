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

public partial class con_historico_vendas_adm : System.Web.UI.Page
{
    private string connString = Funcoes.conexao();

    protected async void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            DateTime data = DateTime.Today;
            DateTime primeiroDiaDoMes = new DateTime(data.Year, data.Month, 1);
            DateTime ultimoDiaDoMes = new DateTime(data.Year, data.Month, DateTime.DaysInMonth(data.Year, data.Month));

            txtDataIni.Text = DateTime.Now.ToShortDateString();
            txtDataFim.Text = DateTime.Now.ToShortDateString();

            await CarregaTabelas();
            CarregaAdquirentes();

            //ConsultaGeral();
            await ConsultaGeralAsync(0);
        }

    }

    private async Task CarregaTabelas()
    {
            SqlConnection myStatus = new SqlConnection(Funcoes.conexao());
            myStatus.Open();
            SqlCommand cmdStatus = new SqlCommand("dbo.stp_status_ins", myStatus);
            cmdStatus.CommandType = CommandType.StoredProcedure;
            cmdStatus.CommandTimeout =0;
            cmdStatus.Parameters.Add("@flg_operacao", SqlDbType.VarChar).Value = "L";
            SqlDataAdapter drStatus = new SqlDataAdapter();
            drStatus.SelectCommand = cmdStatus;
            DataSet dsStatus = new DataSet();
            drStatus.Fill(dsStatus, "STATUS");
            //drStatus.Fill(dsStatus, "STATUS");
            ddlStatus.DataTextField = "NOM_STATUS_ADQUIRENTE";
            ddlStatus.DataValueField = "NOM_STATUS";
            ddlStatus.DataSource = dsStatus.Tables["STATUS"].DefaultView;
            ddlStatus.DataBind();
            ddlStatus.Items.Insert(0, new ListItem("Todos", ""));
            myStatus.Close();
            myStatus.Dispose();

            // Tabela de Tipo de Pagamento

            SqlConnection myTipoPagamento = new SqlConnection(Funcoes.conexao());
            myTipoPagamento.Open();
            SqlCommand cmdTipoPagamento = new SqlCommand("dbo.stp_tipo_pagamento_ins", myTipoPagamento);
            cmdTipoPagamento.CommandType = CommandType.StoredProcedure;
            cmdTipoPagamento.CommandTimeout=0;
            cmdTipoPagamento.Parameters.Add("@flg_operacao", SqlDbType.VarChar).Value = "L";
            SqlDataAdapter drTipoPagamento = new SqlDataAdapter();
            drTipoPagamento.SelectCommand = cmdTipoPagamento;
            DataSet dsTipoPagamento = new DataSet();
            drTipoPagamento.Fill(dsTipoPagamento, "TIPO_PAGAMENTO");
            ddlTipoPagamento.DataTextField = "NOM_TIPO_PAGAMENTO_ADQUIRENTE";
            ddlTipoPagamento.DataValueField = "NOM_TIPO_PAGAMENTO";
            ddlTipoPagamento.DataSource = dsTipoPagamento.Tables["TIPO_PAGAMENTO"].DefaultView;
            ddlTipoPagamento.DataBind();
            ddlTipoPagamento.Items.Insert(0, new ListItem("Todos", ""));
            myTipoPagamento.Close();
            myTipoPagamento.Dispose();

            // Tabela de Estabelecimento

            SqlConnection myEstabelecimentos = new SqlConnection(Funcoes.conexao());
            myEstabelecimentos.Open();
            SqlCommand cmdEstabelecimentos = new SqlCommand("dbo.stp_pessoas_fj_ins", myEstabelecimentos);
            cmdEstabelecimentos.CommandType = CommandType.StoredProcedure;
            cmdEstabelecimentos.CommandTimeout=0;
            cmdEstabelecimentos.Parameters.Add("@flg_operacao", SqlDbType.VarChar).Value = "Y";

            cmdEstabelecimentos.Parameters.Add("@COD_ID_PESSOA_LICENCIADO", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["LICENCIADO"].ToString());
            cmdEstabelecimentos.Parameters.Add("@NOM_RAZAOSOCIAL", SqlDbType.VarChar).Value = " ";
            cmdEstabelecimentos.Parameters.Add("@NUM_DOCUMENTO", SqlDbType.VarChar).Value = " ";
            cmdEstabelecimentos.Parameters.Add("@NUM_TOKEN", SqlDbType.VarChar).Value = " ";
            cmdEstabelecimentos.Parameters.Add("@FLG_ATIVO", SqlDbType.Char).Value = " ";
            cmdEstabelecimentos.Parameters.Add("@FLG_TIPO", SqlDbType.Char).Value = "E";

            if (HttpContext.Current.Session["TIPO"].ToString() == "M")
            {
                cmdEstabelecimentos.Parameters.Add("@COD_ID_MARKETPLACE", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["PESSOA"].ToString());
            }
            if (HttpContext.Current.Session["TIPO"].ToString() == "R")
            {
                cmdEstabelecimentos.Parameters.Add("@COD_ID_REPRESENTANTE", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["PESSOA"].ToString());
            }

            SqlDataAdapter drEstabelecimentos = new SqlDataAdapter();
            drEstabelecimentos.SelectCommand = cmdEstabelecimentos;
            DataSet dsEstabelecimentos = new DataSet();
            drEstabelecimentos.Fill(dsEstabelecimentos, "PESSOAS_FJ");
            ddlEstabelecimentos.DataTextField = "NOM_RAZAOSOCIAL";
            ddlEstabelecimentos.DataValueField = "NOM_RAZAOSOCIAL";
            ddlEstabelecimentos.DataSource = dsEstabelecimentos.Tables["PESSOAS_FJ"].DefaultView;
            ddlEstabelecimentos.DataBind();
            ddlEstabelecimentos.Items.Insert(0, new ListItem("Todos", ""));
            myEstabelecimentos.Close();
            myEstabelecimentos.Dispose();


            SqlConnection myRepresentante = new SqlConnection(Funcoes.conexao());
            myRepresentante.Open();
            SqlCommand cmdRepresentante = new SqlCommand("dbo.stp_pessoas_fj_ins", myRepresentante);
            cmdRepresentante.CommandType = CommandType.StoredProcedure;
            cmdRepresentante.CommandTimeout=0;
            cmdRepresentante.Parameters.Add("@flg_operacao", SqlDbType.VarChar).Value = "L";
            cmdRepresentante.Parameters.Add("@COD_ID_PESSOA_LICENCIADO", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["LICENCIADO"].ToString());
            cmdRepresentante.Parameters.Add("@FLG_ATIVO", SqlDbType.Char).Value = "S";
            cmdRepresentante.Parameters.Add("@FLG_TIPO", SqlDbType.Char).Value = "R";

            if (HttpContext.Current.Session["TIPO"].ToString().Trim() == "M")
            {
                cmdRepresentante.Parameters.Add("@COD_ID_MARKETPLACE", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["PESSOA"].ToString());
            }

            SqlDataAdapter drRepresentante = new SqlDataAdapter();
            drRepresentante.SelectCommand = cmdRepresentante;
            DataSet dsRepresentante = new DataSet();
            drRepresentante.Fill(dsRepresentante, "PESSOAS_FJ");
            ddlRepresentante.DataTextField = "NOM_RAZAOSOCIAL";
            ddlRepresentante.DataValueField = "COD_ID";
            ddlRepresentante.DataSource = dsRepresentante.Tables["PESSOAS_FJ"].DefaultView;
            ddlRepresentante.DataBind();
            ddlRepresentante.Items.Insert(0, new ListItem("Todos", "0"));

            if (HttpContext.Current.Session["TIPO"].ToString().Trim() == "R")
            {
                ddlRepresentante.SelectedValue = HttpContext.Current.Session["PESSOA"].ToString();
                ddlRepresentante.Enabled = false;
            }
            myRepresentante.Close();
            myRepresentante.Dispose();

            // Lista dos Marketplaces

            SqlConnection myMkt = new SqlConnection(Funcoes.conexao());
            myMkt.Open();
            SqlCommand cmdMkt = new SqlCommand("dbo.stp_pessoas_fj_ins", myMkt);
            cmdMkt.CommandType = CommandType.StoredProcedure;
            cmdMkt.CommandTimeout=0;
            cmdMkt.Parameters.Add("@flg_operacao", SqlDbType.VarChar).Value = "L";
            cmdMkt.Parameters.Add("@COD_ID_PESSOA_LICENCIADO", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["LICENCIADO"].ToString());
            cmdMkt.Parameters.Add("@FLG_ATIVO", SqlDbType.Char).Value = "S";
            cmdMkt.Parameters.Add("@FLG_TIPO", SqlDbType.Char).Value = "M";

            SqlDataAdapter drMkt = new SqlDataAdapter();
            drMkt.SelectCommand = cmdMkt;
            DataSet dsMkt = new DataSet();
            drMkt.Fill(dsMkt, "PESSOAS_FJ");
            ddlMarketplace.DataTextField = "NOM_RAZAOSOCIAL";
            ddlMarketplace.DataValueField = "COD_ID";
            ddlMarketplace.DataSource = dsMkt.Tables["PESSOAS_FJ"].DefaultView;
            ddlMarketplace.DataBind();
            ddlMarketplace.Items.Insert(0, new ListItem("Todos", "0"));

            if (HttpContext.Current.Session["TIPO"].ToString().Trim() == "R")
            {
                ddlMarketplace.Enabled = false;
            }


            if (HttpContext.Current.Session["TIPO"].ToString().Trim() == "M")
            {
                ddlMarketplace.SelectedValue = HttpContext.Current.Session["PESSOA"].ToString();
                ddlMarketplace.Enabled = false;
            }
            myMkt.Close();
            myMkt.Dispose();
    }

    private void CarregaAdquirentes()
    {
        SqlConnection myAdquirentes = new SqlConnection(Funcoes.conexao());
        myAdquirentes.Open();
        SqlCommand cmdAdquirentes = new SqlCommand("dbo.stp_pessoas_fj_integracoes_ins", myAdquirentes);
        cmdAdquirentes.CommandType = CommandType.StoredProcedure;
        cmdAdquirentes.CommandTimeout = 0;
        if ((HttpContext.Current.Session["TIPO"].ToString() == "L"))
        {
            cmdAdquirentes.Parameters.Add("@flg_operacao", SqlDbType.VarChar).Value = "G";
        }
        else
        {
            cmdAdquirentes.Parameters.Add("@flg_operacao", SqlDbType.VarChar).Value = "M";
        }

        cmdAdquirentes.Parameters.Add("@COD_ID_PESSOA_LICENCIADO", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["LICENCIADO"].ToString());
        cmdAdquirentes.Parameters.Add("@FLG_ATIVO", SqlDbType.Char).Value = "S";
        cmdAdquirentes.Parameters.Add("@FLG_ACESSO", SqlDbType.Char).Value = "S";
        SqlDataAdapter drAdquirentes = new SqlDataAdapter();
        drAdquirentes.SelectCommand = cmdAdquirentes;
        DataSet dsAdquirentes = new DataSet();
        drAdquirentes.Fill(dsAdquirentes, "PESSOAS_FJ_INTEGRACOES");

        ddlAdquirente.DataTextField = "NOM_FANTASIA_INTEGRACAO";
        ddlAdquirente.DataValueField = "FLG_INTEGRACAO";
        ddlAdquirente.DataSource = dsAdquirentes.Tables["PESSOAS_FJ_INTEGRACOES"].DefaultView;
        ddlAdquirente.DataBind();
        ddlAdquirente.Items.Insert(0, new ListItem("Todos", " "));
    }


    protected async void btnPesquisar_Click(object sender, EventArgs e)
    {
        //ConsultaGeral();
        await ConsultaGeralAsync(0);
    }


    public async Task ConsultaGeralAsync(int pageIndex)
    {

        using (SqlConnection conn = new SqlConnection(connString))
        {
            using (SqlCommand cmd = new SqlCommand("dbo.stp_transacoes_ins", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@flg_operacao", "H");
                cmd.Parameters.AddWithValue("@COD_ID_PESSOA_LICENCIADO", Funcoes.strToInt(HttpContext.Current.Session["LICENCIADO"].ToString()));
                cmd.Parameters.AddWithValue("@NOM_FILTRO", txtFiltro.Text);
                cmd.Parameters.AddWithValue("@NOM_STATUS", ddlStatus.SelectedValue);
                cmd.Parameters.AddWithValue("@NOM_TIPO_PAGAMENTO", ddlTipoPagamento.SelectedValue);
                cmd.Parameters.AddWithValue("@NOM_RAZAOSOCIAL", ddlEstabelecimentos.SelectedValue);
                cmd.Parameters.AddWithValue("@FLG_ORIGEM", ddlAdquirente.SelectedValue);
                cmd.Parameters.AddWithValue("@DTA_INICIO", Convert.ToDateTime(txtDataIni.Text));
                cmd.Parameters.AddWithValue("@DTA_FIM", Convert.ToDateTime(txtDataFim.Text));
                if (Funcoes.strToInt(ddlRepresentante.SelectedValue.Trim()) > 0)
                {
                    cmd.Parameters.AddWithValue("@COD_ID_REPRESENTANTE", Funcoes.strToInt(ddlRepresentante.SelectedValue.Trim()));
                }
                if (Funcoes.strToInt(ddlMarketplace.SelectedValue.Trim()) > 0)
                {
                    cmd.Parameters.AddWithValue("@COD_ID_MARKETPLACE", Funcoes.strToInt(ddlMarketplace.SelectedValue.Trim()));
                }

                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                DataTable dt = new DataTable();
                dt.Load(reader);

                gvConsulta.DataSource = dt;
                gvConsulta.PageIndex = pageIndex;
                gvConsulta.DataBind();
            }
        }


    }    
    
    protected void rptConsulta_OnItemCommand(object source, RepeaterCommandEventArgs e)
    {

    }

    public string GetPostBackScript()
    {
        PostBackOptions options = new PostBackOptions(btnPostback);
        Page.ClientScript.RegisterForEventValidation(options);

        return Page.ClientScript.GetPostBackEventReference(options);
    }

    protected async void btnPostBack_Click(object sender, EventArgs e)
    {
        //ConsultaGeral();
        await ConsultaGeralAsync(0);
    }
    protected async void gvConsulta_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        
        if (HttpContext.Current.Session["TIPO"].ToString().Trim()=="M")
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                e.Row.Cells[6].Visible = false; // Oculta a primeira coluna (ajuste o índice conforme necessário)
            }

            if (e.Row.RowType == DataControlRowType.Header)
            {
                e.Row.Cells[6].Visible = false; // Oculta a primeira coluna (ajuste o índice conforme necessário)
            }
        }

        if (HttpContext.Current.Session["TIPO"].ToString().Trim()=="R")
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {

                e.Row.Cells[8].Visible = false; // Oculta a primeira coluna (ajuste o índice conforme necessário)
            }

            if (e.Row.RowType == DataControlRowType.Header)
            {
                e.Row.Cells[8].Visible = false; // Oculta a primeira coluna (ajuste o índice conforme necessário)
            }
        }
        
    }
    protected async void gvConsulta_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gvConsulta.PageIndex = e.NewPageIndex;
        await ConsultaGeralAsync(e.NewPageIndex);


    }
}