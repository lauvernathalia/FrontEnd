using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Runtime.Serialization.Json;
using System.IO;
using System.Text;


using System.Data;
using System.Configuration;
using System.Collections;
using System.Web.Security;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Data.SqlClient;
using System.Security.Cryptography;


public partial class con_resumo_vendas : System.Web.UI.Page
{
    private string connString = Funcoes.conexao();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            DateTime data = DateTime.Today;
            DateTime primeiroDiaDoMes = new DateTime(data.Year, data.Month, 1);
            DateTime ultimoDiaDoMes = new DateTime(data.Year, data.Month, DateTime.DaysInMonth(data.Year, data.Month));

            txtDataIni.Text = primeiroDiaDoMes.ToShortDateString();
            txtDataFim.Text = DateTime.Now.ToShortDateString();

            AdquirentesService();
            ConsultaGeralVendas();
            ConsultaGeralOperacao();
            ConsultaGeralParcelamento();
        }
    }

    private void AdquirentesService()
    {
        using (SqlConnection connection = new SqlConnection(Funcoes.conexao()))
        {
            connection.Open();
            using (SqlCommand cmd = new SqlCommand("dbo.stp_pessoas_fj_integracoes_ins", connection))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@flg_operacao", SqlDbType.VarChar).Value = "F";
                cmd.Parameters.Add("@COD_ID_PESSOA_LICENCIADO", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["LICENCIADO"].ToString());
                cmd.Parameters.Add("@COD_ID_PESSOAS_FJ", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["PESSOA"].ToString());
                cmd.Parameters.Add("@FLG_ACESSO", SqlDbType.Char).Value = "S";

                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                DataTable dtAdquirentes = new DataTable();
                adapter.Fill(dtAdquirentes);

                ddlAdquirentes.DataTextField = "NOM_FANTASIA_INTEGRACAO";
                ddlAdquirentes.DataValueField = "FLG_INTEGRACAO";
                ddlAdquirentes.DataSource = dtAdquirentes;
                ddlAdquirentes.DataBind();
                ddlAdquirentes.Items.Insert(0, new ListItem("Todos", "%"));
            }
        }
    }

    private void ConsultaGeralVendas()
    {
        using (SqlConnection connTotal = new SqlConnection(connString))
        {
            using (SqlCommand cmdTotal = new SqlCommand("dbo.stp_transacoes_ins", connTotal))
            {
                cmdTotal.CommandType = CommandType.StoredProcedure;
                cmdTotal.CommandTimeout = 0;
                cmdTotal.Parameters.AddWithValue("@flg_operacao", "T");
                cmdTotal.Parameters.AddWithValue("@COD_ID_PESSOA_LICENCIADO", Funcoes.strToInt(HttpContext.Current.Session["LICENCIADO"].ToString()));

                cmdTotal.Parameters.AddWithValue("@NOM_FILTRO", "");
                cmdTotal.Parameters.AddWithValue("@NOM_STATUS", "");
                cmdTotal.Parameters.AddWithValue("@NOM_TIPO_OPERACAO", "");
                cmdTotal.Parameters.AddWithValue("@NOM_RAZAOSOCIAL", "");
                cmdTotal.Parameters.AddWithValue("@FLG_ORIGEM", ddlAdquirentes.SelectedValue);

                cmdTotal.Parameters.AddWithValue("@DTA_INICIO", Convert.ToDateTime(txtDataIni.Text));
                cmdTotal.Parameters.AddWithValue("@DTA_FIM", Convert.ToDateTime(txtDataFim.Text));


                if (HttpContext.Current.Session["TIPO"].ToString() == "M")
                {
                    cmdTotal.Parameters.Add("@COD_ID_MARKETPLACE", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["PESSOA"].ToString());
                }
                if (HttpContext.Current.Session["TIPO"].ToString() == "R")
                {
                    cmdTotal.Parameters.Add("@COD_ID_REPRESENTANTE", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["PESSOA"].ToString());
                }

                if (HttpContext.Current.Session["TIPO"].ToString() == "E")
                {
                    cmdTotal.Parameters.Add("@COD_ID_PESSOAS_FJ", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["PESSOA"].ToString());
                }


                connTotal.Open();
                SqlDataReader readerTotal = cmdTotal.ExecuteReader();
                int iQtde = 0;
                double dValorTotal = 0;
                double dValorTotalPendente = 0;
                double dValorTotalFalhadas = 0;
                double dValorTicketMedio = 0;
                while (readerTotal.Read())
                {
                    iQtde = iQtde + Funcoes.strToInt(readerTotal["NUM_QTDE_VENDAS"].ToString());
                    lblVendas.Text = readerTotal["NUM_QTDE_VENDAS"].ToString();
                    if (readerTotal["FLG_STATUS"].ToString() == "S")
                    {
                        lblAprovadas.Text = readerTotal["NUM_QTDE_VENDAS"].ToString();
                        dValorTotal = dValorTotal + Funcoes.strToDouble(readerTotal["NUM_VALOR_TOTAL"].ToString());
                        dValorTicketMedio = dValorTicketMedio + Funcoes.strToDouble(readerTotal["NUM_VALOR_TICKET_MEDIO"].ToString());

                    }
                    if (readerTotal["FLG_STATUS"].ToString() == "N")
                    {
                        lblFalhadas.Text = readerTotal["NUM_QTDE_VENDAS"].ToString();
                        dValorTotalFalhadas = dValorTotalFalhadas + Funcoes.strToDouble(readerTotal["NUM_VALOR_TOTAL"].ToString());
                    }
                    if (readerTotal["FLG_STATUS"].ToString() == "X")
                    {
                        lblPendentes.Text = readerTotal["NUM_QTDE_VENDAS"].ToString();
                        dValorTotalPendente = dValorTotalPendente + Funcoes.strToDouble(readerTotal["NUM_VALOR_TOTAL"].ToString());

                    }
                }
                lblVendas.Text = String.Format("{0:n0}", iQtde);
                lblSaldo.Text = String.Format("{0:c2}", dValorTotal);
                lblTicket.Text = String.Format("{0:c2}", dValorTicketMedio);

                lblPendentesValor.Text = String.Format("{0:c2}", dValorTotalPendente);

                lblFalhadasValor.Text = String.Format("{0:c2}", dValorTotalFalhadas);


            }
        }
    }

    private void ConsultaGeralOperacao()
    {
        SqlConnection myConsulta = new SqlConnection(Funcoes.conexao());
        myConsulta.Open();
        SqlDataAdapter SDAConsulta = new SqlDataAdapter("dbo.stp_resumo_vendas_ins", myConsulta);
        SDAConsulta.SelectCommand.CommandType = CommandType.StoredProcedure;
        SDAConsulta.SelectCommand.CommandTimeout = 0;

        SDAConsulta.SelectCommand.Parameters.Add("@flg_operacao", SqlDbType.Char).Value = "O";

        SDAConsulta.SelectCommand.Parameters.Add("@COD_ID_PESSOA_LICENCIADO", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["LICENCIADO"].ToString());

        if (HttpContext.Current.Session["TIPO"].ToString() == "M")
        {
            SDAConsulta.SelectCommand.Parameters.Add("@COD_ID_MARKETPLACE", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["PESSOA"].ToString());
        }
        if (HttpContext.Current.Session["TIPO"].ToString() == "R")
        {
            SDAConsulta.SelectCommand.Parameters.Add("@COD_ID_REPRESENTANTE", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["PESSOA"].ToString());
        }

        if (HttpContext.Current.Session["TIPO"].ToString() == "E")
        {
            SDAConsulta.SelectCommand.Parameters.Add("@COD_ID_PESSOAS_FJ", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["PESSOA"].ToString());
        }
        
        SDAConsulta.SelectCommand.Parameters.Add("@DTA_INICIO", SqlDbType.DateTime).Value = Convert.ToDateTime(txtDataIni.Text.ToString());
        SDAConsulta.SelectCommand.Parameters.Add("@DTA_FIM", SqlDbType.DateTime).Value = Convert.ToDateTime(txtDataFim.Text.ToString());

        SDAConsulta.SelectCommand.Parameters.Add("@FLG_ORIGEM", SqlDbType.Char).Value = ddlAdquirentes.SelectedValue.ToString();

        DataSet dsConsulta = new DataSet();
        SDAConsulta.Fill(dsConsulta, "TRANSACOES");
        rptOperacao.DataSource = dsConsulta.Tables["TRANSACOES"].DefaultView;
        rptOperacao.DataBind();
        myConsulta.Close(); myConsulta.Dispose();


    }

    private void ConsultaGeralParcelamento()
    {
        SqlConnection myConsulta = new SqlConnection(Funcoes.conexao());
        myConsulta.Open();
        SqlDataAdapter SDAConsulta = new SqlDataAdapter("dbo.stp_resumo_vendas_ins", myConsulta);
        SDAConsulta.SelectCommand.CommandTimeout = 0;

        SDAConsulta.SelectCommand.CommandType = CommandType.StoredProcedure;
        SDAConsulta.SelectCommand.Parameters.Add("@flg_operacao", SqlDbType.Char).Value = "B";

        SDAConsulta.SelectCommand.Parameters.Add("@COD_ID_PESSOA_LICENCIADO", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["LICENCIADO"].ToString());

        if (HttpContext.Current.Session["TIPO"].ToString() == "M")
        {
            SDAConsulta.SelectCommand.Parameters.Add("@COD_ID_MARKETPLACE", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["PESSOA"].ToString());
        }
        if (HttpContext.Current.Session["TIPO"].ToString() == "R")
        {
            SDAConsulta.SelectCommand.Parameters.Add("@COD_ID_REPRESENTANTE", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["PESSOA"].ToString());
        }

        if (HttpContext.Current.Session["TIPO"].ToString() == "E")
        {
            SDAConsulta.SelectCommand.Parameters.Add("@COD_ID_PESSOAS_FJ", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["PESSOA"].ToString());
        }

        SDAConsulta.SelectCommand.Parameters.Add("@DTA_INICIO", SqlDbType.DateTime).Value = Convert.ToDateTime(txtDataIni.Text.ToString());
        SDAConsulta.SelectCommand.Parameters.Add("@DTA_FIM", SqlDbType.DateTime).Value = Convert.ToDateTime(txtDataFim.Text.ToString());

        SDAConsulta.SelectCommand.Parameters.Add("@FLG_ORIGEM", SqlDbType.Char).Value = ddlAdquirentes.SelectedValue.ToString();

        DataSet dsConsulta = new DataSet();
        SDAConsulta.Fill(dsConsulta, "TRANSACOES");
        rptParcelamento.DataSource = dsConsulta.Tables["TRANSACOES"].DefaultView;
        rptParcelamento.DataBind();
        myConsulta.Close(); myConsulta.Dispose();


    }

    
    protected void btnPesquisar_Click(object sender, EventArgs e)
    {
        ConsultaGeralOperacao();
        ConsultaGeralParcelamento();

    }

    public string GetPostBackScript()
    {
        PostBackOptions options = new PostBackOptions(btnPostback);
        Page.ClientScript.RegisterForEventValidation(options);

        return Page.ClientScript.GetPostBackEventReference(options);
    }

    protected void btnPostBack_Click(object sender, EventArgs e)
    {

    }
    protected void rptConsulta_OnItemCommand(object source, RepeaterCommandEventArgs e)
    {

    }
    protected void rptOperacao_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        Repeater oGrid = (Repeater)e.Item.FindControl("rptDetalhe");
        oGrid.Visible = true;

        TextBox tOperacao = (TextBox)e.Item.FindControl("txtoperacao");

        SqlConnection myConsulta = new SqlConnection(Funcoes.conexao());
        myConsulta.Open();
        SqlDataAdapter SDAConsulta = new SqlDataAdapter("dbo.stp_resumo_vendas_ins", myConsulta);
        SDAConsulta.SelectCommand.CommandType = CommandType.StoredProcedure;
        SDAConsulta.SelectCommand.CommandTimeout = 0;
        SDAConsulta.SelectCommand.Parameters.Add("@flg_operacao", SqlDbType.Char).Value = "R";

        SDAConsulta.SelectCommand.Parameters.Add("@COD_ID_PESSOA_LICENCIADO", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["LICENCIADO"].ToString());

        if (HttpContext.Current.Session["TIPO"].ToString() == "M")
        {
            SDAConsulta.SelectCommand.Parameters.Add("@COD_ID_MARKETPLACE", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["PESSOA"].ToString());
        }
        if (HttpContext.Current.Session["TIPO"].ToString() == "R")
        {
            SDAConsulta.SelectCommand.Parameters.Add("@COD_ID_REPRESENTANTE", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["PESSOA"].ToString());
        }

        if (HttpContext.Current.Session["TIPO"].ToString() == "E")
        {
            SDAConsulta.SelectCommand.Parameters.Add("@COD_ID_PESSOAS_FJ", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["PESSOA"].ToString());
        }

        SDAConsulta.SelectCommand.Parameters.Add("@DTA_INICIO", SqlDbType.DateTime).Value = Convert.ToDateTime(txtDataIni.Text.ToString());
        SDAConsulta.SelectCommand.Parameters.Add("@DTA_FIM", SqlDbType.DateTime).Value = Convert.ToDateTime(txtDataFim.Text.ToString());

        SDAConsulta.SelectCommand.Parameters.Add("@FLG_ORIGEM", SqlDbType.Char).Value = ddlAdquirentes.SelectedValue.ToString();
        SDAConsulta.SelectCommand.Parameters.Add("@NOM_TIPO_OPERACAO", SqlDbType.VarChar).Value = tOperacao.Text.ToString();

        DataSet dsConsulta = new DataSet();
        SDAConsulta.Fill(dsConsulta, "TRANSACOES");
        oGrid.DataSource = dsConsulta.Tables["TRANSACOES"].DefaultView;
        oGrid.DataBind();
        myConsulta.Close(); myConsulta.Dispose();

    }
    protected void rptParcelamento_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        

        Repeater oGrid = (Repeater)e.Item.FindControl("rptDetalheParcelamento");
        oGrid.Visible = true;

        TextBox tBandeira = (TextBox)e.Item.FindControl("txtbandeira");

        SqlConnection myConsulta = new SqlConnection(Funcoes.conexao());
        myConsulta.Open();
        SqlDataAdapter SDAConsulta = new SqlDataAdapter("dbo.stp_resumo_vendas_ins", myConsulta);
        SDAConsulta.SelectCommand.CommandType = CommandType.StoredProcedure;
        SDAConsulta.SelectCommand.CommandTimeout = 0;
        SDAConsulta.SelectCommand.Parameters.Add("@flg_operacao", SqlDbType.Char).Value = "P";

        SDAConsulta.SelectCommand.Parameters.Add("@COD_ID_PESSOA_LICENCIADO", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["LICENCIADO"].ToString());

        if (HttpContext.Current.Session["TIPO"].ToString() == "M")
        {
            SDAConsulta.SelectCommand.Parameters.Add("@COD_ID_MARKETPLACE", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["PESSOA"].ToString());
        }
        if (HttpContext.Current.Session["TIPO"].ToString() == "R")
        {
            SDAConsulta.SelectCommand.Parameters.Add("@COD_ID_REPRESENTANTE", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["PESSOA"].ToString());
        }

        if (HttpContext.Current.Session["TIPO"].ToString() == "E")
        {
            SDAConsulta.SelectCommand.Parameters.Add("@COD_ID_PESSOAS_FJ", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["PESSOA"].ToString());
        }

        SDAConsulta.SelectCommand.Parameters.Add("@DTA_INICIO", SqlDbType.DateTime).Value = Convert.ToDateTime(txtDataIni.Text.ToString());
        SDAConsulta.SelectCommand.Parameters.Add("@DTA_FIM", SqlDbType.DateTime).Value = Convert.ToDateTime(txtDataFim.Text.ToString());

        SDAConsulta.SelectCommand.Parameters.Add("@FLG_ORIGEM", SqlDbType.Char).Value = ddlAdquirentes.SelectedValue.ToString();
        SDAConsulta.SelectCommand.Parameters.Add("@COD_ID_BANDEIRA", SqlDbType.Int).Value = Funcoes.strToInt(tBandeira.Text.ToString());

        DataSet dsConsulta = new DataSet();
        SDAConsulta.Fill(dsConsulta, "TRANSACOES");
        oGrid.DataSource = dsConsulta.Tables["TRANSACOES"].DefaultView;
        oGrid.DataBind();
        myConsulta.Close(); myConsulta.Dispose();
        
        
    }
}