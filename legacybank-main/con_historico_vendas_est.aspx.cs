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
using System.Xml;


public partial class con_historico_vendas_est : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {

            //Vamos considerar que a data seja o dia de hoje, mas pode ser qualquer data.
            DateTime data = DateTime.Today;
            //DateTime com o primeiro dia do mês
            DateTime primeiroDiaDoMes = new DateTime(data.Year, data.Month, 1);
            //DateTime com o último dia do mês
            DateTime ultimoDiaDoMes = new DateTime(data.Year, data.Month, DateTime.DaysInMonth(data.Year, data.Month));
            txtDataIni.Text = primeiroDiaDoMes.ToShortDateString();
            txtDataFim.Text = ultimoDiaDoMes.ToShortDateString();

            // Tabela de Status de Venda

            SqlConnection myStatus = new SqlConnection(Funcoes.conexao());
            myStatus.Open();
            SqlCommand cmdStatus = new SqlCommand("dbo.stp_status_ins", myStatus);
            cmdStatus.CommandType = CommandType.StoredProcedure;
            cmdStatus.Parameters.Add("@flg_operacao", SqlDbType.VarChar).Value = "L";
            SqlDataAdapter drStatus = new SqlDataAdapter();
            drStatus.SelectCommand = cmdStatus;
            DataSet dsStatus = new DataSet();
            drStatus.Fill(dsStatus, "STATUS");
            ddlStatus.DataTextField = "NOM_STATUS_ADQUIRENTE";
            ddlStatus.DataValueField = "NOM_STATUS";
            ddlStatus.DataSource = dsStatus.Tables["STATUS"].DefaultView;
            ddlStatus.DataBind();
            ddlStatus.Items.Insert(0, new ListItem("Todos", ""));

            // Tabela de Tipo de Pagamento

            SqlConnection myTipoPagamento = new SqlConnection(Funcoes.conexao());
            myTipoPagamento.Open();
            SqlCommand cmdTipoPagamento = new SqlCommand("dbo.stp_tipo_pagamento_ins", myTipoPagamento);
            cmdTipoPagamento.CommandType = CommandType.StoredProcedure;
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


            ConsultaGeral();
        }

    }
    protected void btnPesquisar_Click(object sender, EventArgs e)
    {
        ConsultaGeral();
    }

    private void ConsultaGeral()
    {
        //dtgConsulta.Visible = true;
        SqlConnection myConsulta = new SqlConnection(Funcoes.conexao());
        myConsulta.Open();
        SqlDataAdapter SDAConsulta = new SqlDataAdapter("dbo.stp_transacoes_ins", myConsulta);
        SDAConsulta.SelectCommand.CommandType = CommandType.StoredProcedure;
        SDAConsulta.SelectCommand.Parameters.Add("@flg_operacao", SqlDbType.Char).Value = "C";
        SDAConsulta.SelectCommand.Parameters.Add("@COD_ID_PESSOA_LICENCIADO", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["LICENCIADO"].ToString());
        SDAConsulta.SelectCommand.Parameters.Add("@NOM_FILTRO", SqlDbType.VarChar).Value = txtFiltro.Text.ToString();
        SDAConsulta.SelectCommand.Parameters.Add("@NOM_STATUS", SqlDbType.VarChar).Value = ddlStatus.SelectedValue.ToString();
        SDAConsulta.SelectCommand.Parameters.Add("@NOM_TIPO_PAGAMENTO", SqlDbType.VarChar).Value = ddlTipoPagamento.SelectedValue.ToString();
        SDAConsulta.SelectCommand.Parameters.Add("@NOM_RAZAOSOCIAL", SqlDbType.VarChar).Value = txtEstabelecimento.Text.ToString();
        SDAConsulta.SelectCommand.Parameters.Add("@FLG_ORIGEM", SqlDbType.VarChar).Value = ddlAdquirente.SelectedValue.ToString();
        SDAConsulta.SelectCommand.Parameters.Add("@DTA_INICIO", SqlDbType.DateTime).Value = Convert.ToDateTime(txtDataIni.Text.ToString());
        SDAConsulta.SelectCommand.Parameters.Add("@DTA_FIM", SqlDbType.DateTime).Value = Convert.ToDateTime(txtDataFim.Text.ToString());

        if ((HttpContext.Current.Session["TIPO"].ToString().Trim() == "M"))
        {
            SDAConsulta.SelectCommand.Parameters.Add("@COD_ID_MARKETPLACE", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["PESSOA"].ToString());
        }
        if ((HttpContext.Current.Session["TIPO"].ToString().Trim() == "R"))
        {
            SDAConsulta.SelectCommand.Parameters.Add("@COD_ID_REPRESENTANTE", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["PESSOA"].ToString());
        }
        if ((HttpContext.Current.Session["TIPO"].ToString().Trim() == "E"))
        {
            SDAConsulta.SelectCommand.Parameters.Add("@COD_ID_PESSOAS_FJ", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["PESSOA"].ToString());
        }

        DataSet dsConsulta = new DataSet();
        SDAConsulta.Fill(dsConsulta, "TRANSACOES");
        gvConsulta.DataSource = dsConsulta.Tables["TRANSACOES"].DefaultView;
        gvConsulta.DataBind();
        myConsulta.Close(); myConsulta.Dispose();

        // Consulta Totais
        double iVendas = 0;
        double iTicketMedio = 0;
        double iComissao = 0;


        SqlConnection mySelTotais = new SqlConnection(Funcoes.conexao());
        mySelTotais.Open();
        SqlCommand cmdSelTotais = new SqlCommand("dbo.stp_transacoes_ins", mySelTotais);
        cmdSelTotais.CommandType = CommandType.StoredProcedure;
        cmdSelTotais.Parameters.Add("@flg_operacao", SqlDbType.Char).Value = "T";
        cmdSelTotais.Parameters.Add("@COD_ID_PESSOA_LICENCIADO", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["LICENCIADO"].ToString());
        cmdSelTotais.Parameters.Add("@NOM_FILTRO", SqlDbType.VarChar).Value = txtFiltro.Text.ToString();
        cmdSelTotais.Parameters.Add("@NOM_STATUS", SqlDbType.VarChar).Value = ddlStatus.SelectedValue.ToString();
        cmdSelTotais.Parameters.Add("@NOM_TIPO_PAGAMENTO", SqlDbType.VarChar).Value = ddlTipoPagamento.SelectedValue.ToString();
        cmdSelTotais.Parameters.Add("@NOM_RAZAOSOCIAL", SqlDbType.VarChar).Value = txtEstabelecimento.Text.ToString();
        cmdSelTotais.Parameters.Add("@FLG_ORIGEM", SqlDbType.VarChar).Value = ddlAdquirente.SelectedValue.ToString();
        cmdSelTotais.Parameters.Add("@DTA_INICIO", SqlDbType.DateTime).Value = Convert.ToDateTime(txtDataIni.Text.ToString());
        cmdSelTotais.Parameters.Add("@DTA_FIM", SqlDbType.DateTime).Value = Convert.ToDateTime(txtDataFim.Text.ToString());

        if ((HttpContext.Current.Session["TIPO"].ToString().Trim() == "M"))
        {
            cmdSelTotais.Parameters.Add("@COD_ID_MARKETPLACE", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["PESSOA"].ToString());
        }
        if ((HttpContext.Current.Session["TIPO"].ToString().Trim() == "R"))
        {
            cmdSelTotais.Parameters.Add("@COD_ID_REPRESENTANTE", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["PESSOA"].ToString());
        }

        SqlDataReader ReaderTotais = cmdSelTotais.ExecuteReader();
        while (ReaderTotais.Read())
        {
            iVendas = iVendas + Funcoes.strToDouble(ReaderTotais["NUM_TOTAL_VENDAS"].ToString());
            //iComissao = iComissao + Funcoes.strToDouble(ReaderTotais["NUM_VALOR_COMISSAO"].ToString());
            iTicketMedio = iTicketMedio + Funcoes.strToDouble(ReaderTotais["NUM_VALOR_TICKET_MEDIO"].ToString());
        }
        lblVendas.Text = String.Format("{0:n0}", iVendas);
        lblTicket.Text = String.Format("{0:c2}", iTicketMedio);
        //lblSaldo.Text = String.Format("{0:c2}", iComissao);


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

    protected void btnPostBack_Click(object sender, EventArgs e)
    {
        ConsultaGeral();
    }
}