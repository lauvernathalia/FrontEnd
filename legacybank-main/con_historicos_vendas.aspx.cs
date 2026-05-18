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
using System.Threading;
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

using iTextSharp.text;
using iTextSharp.text.pdf;


public partial class con_historicos_vendas : System.Web.UI.Page
{

    private int Licenciado { get; set; }
    private string Tipo { get; set; }
    private int Pessoa { get; set; }
    private int Usuario { get; set; }
    public int PaginaAtual
    {
        get { return ViewState["PaginaAtual"] == null ? 1 : (int)ViewState["PaginaAtual"]; }
        set { ViewState["PaginaAtual"] = value; }
    }

    public int TotalRegistros
    {
        get { return ViewState["TotalRegistros"] == null ? 0 : (int)ViewState["TotalRegistros"]; }
        set { ViewState["TotalRegistros"] = value; }
    }

    public int TamanhoPagina = 10;


    private string connString = Funcoes.conexao();

    protected async void Page_Load(object sender, EventArgs e)
    {

        Licenciado = Session["LICENCIADO"] != null ? Funcoes.strToInt(Session["LICENCIADO"].ToString()) : 0;
        Tipo = Session["TIPO"] != null ? Session["TIPO"].ToString() : "";
        Pessoa = Session["PESSOA"] != null ? Funcoes.strToInt(Session["PESSOA"].ToString()) : 0;
        Usuario = Session["PESSOA"] != null ? Funcoes.strToInt(Session["CODIGO"].ToString()) : 0; 

        if (!IsPostBack)
        {
            DateTime data = DateTime.Today;
            DateTime primeiroDiaDoMes = new DateTime(data.Year, data.Month, 1);
            DateTime ultimoDiaDoMes = new DateTime(data.Year, data.Month, DateTime.DaysInMonth(data.Year, data.Month));

            txtDataIni.Text = DateTime.Now.ToShortDateString();
            txtDataFim.Text = DateTime.Now.ToShortDateString();

            //await CarregaTabelas();

            await CarregaTabelasAsync();

            CarregaAdquirentes();

            //ConsultaGeral();
            //await ConsultaGeralAsync(0);
        }

    }


    private async Task CarregaTabelasAsync()
{
    string tipo = HttpContext.Current.Session["TIPO"] != null
        ? HttpContext.Current.Session["TIPO"].ToString().Trim()
        : "";

    await CarregaStatusAsync();
    //await CarregaEstabelecimentosAsync(tipo, Pessoa, Licenciado);
    await CarregaRepresentantesAsync(tipo, Pessoa, Licenciado);
    await CarregaMarketplacesAsync(tipo, Pessoa, Licenciado);

    AjustesPorTipo(tipo);
}



    protected void btnAnterior_Click(object sender, EventArgs e)
    {
        if (PaginaAtual > 1)
        {
            PaginaAtual--;
            ConsultaGeralAsync(0);
        }
    }

    protected void btnProximo_Click(object sender, EventArgs e)
    {
        int totalPaginas = (int)Math.Ceiling(TotalRegistros / (double)TamanhoPagina);

        if (PaginaAtual < totalPaginas)
        {
            PaginaAtual++;
            ConsultaGeralAsync(0);
        }
    }

    private void AtualizarPaginacao()
    {
        int totalPaginas = (int)Math.Ceiling(TotalRegistros / (double)TamanhoPagina);

        lblPagina.Text = "Página " + PaginaAtual.ToString() + " de " + totalPaginas.ToString() + " (Total Registros: " + TotalRegistros.ToString() + ")";
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

        cmdAdquirentes.Parameters.Add("@COD_ID_PESSOA_LICENCIADO", SqlDbType.Int).Value = Licenciado;
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
        ddlAdquirente.Items.Insert(0, new System.Web.UI.WebControls.ListItem("Todos", " "));
    }


    protected async void btnPesquisar_Click(object sender, EventArgs e)
    {
        //ConsultaGeral();
        await ConsultaGeralAsync(0);
    }


public async Task ConsultaGeralAsync(int pageIndex)
{
    // =========================
    // CONSULTA PAGINADA
    // =========================
    using (SqlConnection myConsulta = new SqlConnection(Funcoes.conexao()))
    using (SqlDataAdapter SDAConsulta = new SqlDataAdapter("dbo.stp_transacoes_paginado", myConsulta))
    {
        SDAConsulta.SelectCommand.CommandType = CommandType.StoredProcedure;
        SDAConsulta.SelectCommand.CommandTimeout = 120; // timeout ajustado

        SDAConsulta.SelectCommand.Parameters.AddWithValue("@flg_operacao", "H");
        SDAConsulta.SelectCommand.Parameters.Add("@PageNumber", SqlDbType.Int).Value = PaginaAtual;
        SDAConsulta.SelectCommand.Parameters.Add("@PageSize", SqlDbType.Int).Value = TamanhoPagina;
        SDAConsulta.SelectCommand.Parameters.Add("@Exportar", SqlDbType.Bit).Value = 0;
        SDAConsulta.SelectCommand.Parameters.AddWithValue("@COD_ID_PESSOA_LICENCIADO", Licenciado);

        if (HttpContext.Current.Session["TIPO"] != null &&
            HttpContext.Current.Session["TIPO"].ToString().Trim() == "E")
        {
            SDAConsulta.SelectCommand.Parameters.AddWithValue("@COD_ID_PESSOAS_FJ", Pessoa);
        }

        SDAConsulta.SelectCommand.Parameters.AddWithValue("@DTA_INICIO", Convert.ToDateTime(txtDataIni.Text));
        SDAConsulta.SelectCommand.Parameters.AddWithValue("@DTA_FIM", Convert.ToDateTime(txtDataFim.Text));

        if (Funcoes.strToInt(ddlRepresentante.SelectedValue.Trim()) > 0)
        {
            SDAConsulta.SelectCommand.Parameters.AddWithValue(
                "@COD_ID_REPRESENTANTE",
                Funcoes.strToInt(ddlRepresentante.SelectedValue.Trim()));
        }

        if (Funcoes.strToInt(ddlMarketplace.SelectedValue.Trim()) > 0)
        {
            SDAConsulta.SelectCommand.Parameters.AddWithValue(
                "@COD_ID_MARKETPLACE",
                Funcoes.strToInt(ddlMarketplace.SelectedValue.Trim()));
        }

        DataSet dsConsulta = new DataSet();

        myConsulta.Open();
        SDAConsulta.Fill(dsConsulta);

        TotalRegistros = Convert.ToInt32(dsConsulta.Tables[0].Rows[0]["TotalRegistros"]);
        gvConsulta.DataSource = dsConsulta.Tables[1].DefaultView;
        gvConsulta.DataBind();
    }

    AtualizarPaginacao();

    // =========================
    // TOTAIS / RESUMO
    // =========================
    using (SqlConnection connTotal = new SqlConnection(connString))
    using (SqlCommand cmdTotal = new SqlCommand("dbo.stp_transacoes_ins", connTotal))
    {
        cmdTotal.CommandType = CommandType.StoredProcedure;
        cmdTotal.CommandTimeout = 120; // timeout ajustado

        cmdTotal.Parameters.AddWithValue("@flg_operacao", "T");
        cmdTotal.Parameters.AddWithValue("@COD_ID_PESSOA_LICENCIADO", Licenciado);

        if (HttpContext.Current.Session["TIPO"] != null &&
            HttpContext.Current.Session["TIPO"].ToString().Trim() == "E")
        {
            cmdTotal.Parameters.AddWithValue("@COD_ID_PESSOAS_FJ", Pessoa);
        }

        cmdTotal.Parameters.AddWithValue("@NOM_FILTRO", txtFiltro.Text);
        cmdTotal.Parameters.AddWithValue("@NOM_STATUS", ddlStatus.SelectedValue);
        cmdTotal.Parameters.AddWithValue("@NOM_TIPO_OPERACAO", ddlTipoPagamento.SelectedValue);
        cmdTotal.Parameters.AddWithValue("@NOM_RAZAOSOCIAL", txtEstabelecimento.Text);
        cmdTotal.Parameters.AddWithValue("@FLG_ORIGEM", ddlAdquirente.SelectedValue);

        cmdTotal.Parameters.AddWithValue("@DTA_INICIO", Convert.ToDateTime(txtDataIni.Text));
        cmdTotal.Parameters.AddWithValue("@DTA_FIM", Convert.ToDateTime(txtDataFim.Text));

        if (Funcoes.strToInt(ddlRepresentante.SelectedValue.Trim()) > 0)
        {
            cmdTotal.Parameters.AddWithValue(
                "@COD_ID_REPRESENTANTE",
                Funcoes.strToInt(ddlRepresentante.SelectedValue.Trim()));
        }

        if (Funcoes.strToInt(ddlMarketplace.SelectedValue.Trim()) > 0)
        {
            cmdTotal.Parameters.AddWithValue(
                "@COD_ID_MARKETPLACE",
                Funcoes.strToInt(ddlMarketplace.SelectedValue.Trim()));
        }

        connTotal.Open();

        using (SqlDataReader readerTotal = cmdTotal.ExecuteReader())
        {
            int iQtde = 0;
            double dValorTotal = 0;
            double dValorTicketMedio = 0;

            while (readerTotal.Read())
            {
                int qtde = Funcoes.strToInt(readerTotal["NUM_QTDE_VENDAS"].ToString());
                iQtde += qtde;

                lblVendas.Text = qtde.ToString();

                if (readerTotal["FLG_STATUS"].ToString() == "S")
                {
                    lblAprovadas.Text = qtde.ToString();
                    dValorTotal += Funcoes.strToDouble(readerTotal["NUM_VALOR_TOTAL"].ToString());
                    dValorTicketMedio += Funcoes.strToDouble(readerTotal["NUM_VALOR_TICKET_MEDIO"].ToString());
                }

                if (readerTotal["FLG_STATUS"].ToString() == "N")
                {
                    lblFalhadas.Text = qtde.ToString();
                }
            }

            lblVendas.Text = String.Format("{0:n0}", iQtde);
            lblSaldo.Text = String.Format("{0:n2}", dValorTotal);
            lblTicket.Text = String.Format("{0:n2}", dValorTicketMedio);
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
        if (HttpContext.Current.Session["TIPO"].ToString().Trim()=="E")
        {
            if (e.Row.RowType == DataControlRowType.DataRow){e.Row.Cells[6].Visible = false; }
            if (e.Row.RowType == DataControlRowType.Header){e.Row.Cells[6].Visible = false; }
            if (e.Row.RowType == DataControlRowType.DataRow){e.Row.Cells[8].Visible = false; }
            if (e.Row.RowType == DataControlRowType.Header){e.Row.Cells[8].Visible = false; }

            if (e.Row.RowType == DataControlRowType.DataRow){e.Row.Cells[17].Visible = false; }
            if (e.Row.RowType == DataControlRowType.Header){e.Row.Cells[17].Visible = false; }

            if (e.Row.RowType == DataControlRowType.DataRow){e.Row.Cells[18].Visible = false; }
            if (e.Row.RowType == DataControlRowType.Header){e.Row.Cells[18].Visible = false; }

            if (e.Row.RowType == DataControlRowType.DataRow){e.Row.Cells[19].Visible = false; }
            if (e.Row.RowType == DataControlRowType.Header){e.Row.Cells[19].Visible = false; }

            if (e.Row.RowType == DataControlRowType.DataRow){e.Row.Cells[20].Visible = false; }
            if (e.Row.RowType == DataControlRowType.Header){e.Row.Cells[20].Visible = false; }
        }
        

        if (HttpContext.Current.Session["TIPO"].ToString().Trim()=="M")
        {
            if (e.Row.RowType == DataControlRowType.DataRow){e.Row.Cells[6].Visible = false; }
            if (e.Row.RowType == DataControlRowType.Header){e.Row.Cells[6].Visible = false; }

            if (e.Row.RowType == DataControlRowType.DataRow){e.Row.Cells[17].Visible = false; }
            if (e.Row.RowType == DataControlRowType.Header){e.Row.Cells[17].Visible = false; }
            if (e.Row.RowType == DataControlRowType.DataRow){e.Row.Cells[18].Visible = false; }
            if (e.Row.RowType == DataControlRowType.Header){e.Row.Cells[18].Visible = false; }

            if (e.Row.RowType == DataControlRowType.Header) { e.Row.Cells[19].Text = "Comissão"; }

            //if (e.Row.RowType == DataControlRowType.DataRow){e.Row.Cells[19].Visible = false; }
            //if (e.Row.RowType == DataControlRowType.Header){e.Row.Cells[19].Visible = false; }
            //if (e.Row.RowType == DataControlRowType.DataRow){e.Row.Cells[20].Visible = false; }
            //if (e.Row.RowType == DataControlRowType.Header){e.Row.Cells[20].Visible = false; }
        
        }

        if (HttpContext.Current.Session["TIPO"].ToString().Trim()=="R")
        {
            if (e.Row.RowType == DataControlRowType.DataRow){e.Row.Cells[8].Visible = false; }
            if (e.Row.RowType == DataControlRowType.Header){e.Row.Cells[8].Visible = false; }

            if (e.Row.RowType == DataControlRowType.DataRow){e.Row.Cells[19].Visible = false; }
            if (e.Row.RowType == DataControlRowType.Header){e.Row.Cells[19].Visible = false; }
   
            if (e.Row.RowType == DataControlRowType.Header) { e.Row.Cells[20].Text = "Comissão"; }

            if (e.Row.RowType == DataControlRowType.DataRow){e.Row.Cells[6].Visible = false; }
            if (e.Row.RowType == DataControlRowType.Header){e.Row.Cells[6].Visible = false; }
            if (e.Row.RowType == DataControlRowType.DataRow){e.Row.Cells[8].Visible = false; }
            if (e.Row.RowType == DataControlRowType.Header){e.Row.Cells[8].Visible = false; }
            if (e.Row.RowType == DataControlRowType.DataRow){e.Row.Cells[17].Visible = false; }
            if (e.Row.RowType == DataControlRowType.Header){e.Row.Cells[17].Visible = false; }
            if (e.Row.RowType == DataControlRowType.DataRow){e.Row.Cells[18].Visible = false; }
            if (e.Row.RowType == DataControlRowType.Header){e.Row.Cells[18].Visible = false; }
            if (e.Row.RowType == DataControlRowType.DataRow){e.Row.Cells[19].Visible = false; }
            if (e.Row.RowType == DataControlRowType.Header){e.Row.Cells[19].Visible = false; }


            //if (e.Row.RowType == DataControlRowType.DataRow){e.Row.Cells[20].Visible = false; }
            //if (e.Row.RowType == DataControlRowType.Header){e.Row.Cells[20].Visible = false; }
        
        }
        
    }
    protected async void gvConsulta_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        //gvConsulta.PageIndex = e.NewPageIndex;
        //await ConsultaGeralAsync(e.NewPageIndex);


    }
    protected async void  gvConsulta_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName == "Cancelar")
        {

            SqlConnection mySelCadastro = new SqlConnection(Funcoes.conexao());
            mySelCadastro.Open();
            SqlCommand cmdSelCadastro = new SqlCommand("dbo.stp_transacoes_ins", mySelCadastro);
            cmdSelCadastro.CommandType = CommandType.StoredProcedure;
            cmdSelCadastro.Parameters.Add("@flg_operacao", SqlDbType.Char).Value = "Y";
            cmdSelCadastro.Parameters.Add("@COD_ID", SqlDbType.Int).Value = Funcoes.strToInt(Convert.ToString(e.CommandArgument));
            cmdSelCadastro.Parameters.Add("@COD_ID_PESSOA_LICENCIADO", SqlDbType.Int).Value = Licenciado;
            SqlDataReader ReaderCadastro = cmdSelCadastro.ExecuteReader();
            while (ReaderCadastro.Read())
            {
                if (ReaderCadastro["FLG_STATUS_OPERACAO"].ToString()=="X")
                {
                    if (ReaderCadastro["FLG_ORIGEM"].ToString()=="Z")
                    {
                        zoop.HttpResponseResult resultado = zoop.CancelamentoBoletoZoop(ReaderCadastro["NOM_CODE"].ToString());

                        if ((resultado.StatusCode == 200) || (resultado.StatusCode == 201) || (resultado.StatusCode == 202))
                        {
                            // Atualiza STATUS para CANCELADA
                            SqlConnection connInsCons = new SqlConnection(Funcoes.conexao());
                            connInsCons.Open();
                            SqlCommand cmdInsCons = new SqlCommand("dbo.stp_transacoes_ins", connInsCons);
                            cmdInsCons.CommandType = CommandType.StoredProcedure;
                            cmdInsCons.Parameters.Add("@flg_operacao", SqlDbType.Char).Value = "K";
                            cmdInsCons.Parameters.Add("@COD_ID", SqlDbType.Int).Value = Funcoes.strToInt(Convert.ToString(e.CommandArgument));
                            cmdInsCons.Parameters.Add("@COD_ID_PESSOA_LICENCIADO", SqlDbType.Int).Value = Licenciado;
                            cmdInsCons.Parameters.Add("@NOM_STATUS", SqlDbType.Char).Value = "canceled";
                            cmdInsCons.ExecuteNonQuery();
                            connInsCons.Close();
                            connInsCons.Dispose();

                            ClientScript.RegisterStartupScript(this.GetType(), "SucessoGeralZoop", "alert('Transação cancelada com sucesso!');", true);
                            await ConsultaGeralAsync(0);

                        }
                        else
                        {
                            string mensagemErro = resultado.Content;
                            ClientScript.RegisterStartupScript(this.GetType(), "ErroGeralZoop", "alert('Ocorreu um erro ao tentar cancelar a transação: " + mensagemErro + "');", true);
                        }

                    }

                    if (ReaderCadastro["FLG_ORIGEM"].ToString()=="A")
                    {
                        string sApiKey = asaas.PegarTokenSubconta(Pessoa, Licenciado);
                        asaas.HttpResponseResult resultado = asaas.ExcluirCobrancaAsaas(sApiKey, ReaderCadastro["NOM_CODE"].ToString());

                        if ((resultado.StatusCode == 200) || (resultado.StatusCode == 201) || (resultado.StatusCode == 202))
                        {
                            // Atualiza STATUS para CANCELADA
                            SqlConnection connInsCons = new SqlConnection(Funcoes.conexao());
                            connInsCons.Open();
                            SqlCommand cmdInsCons = new SqlCommand("dbo.stp_transacoes_ins", connInsCons);
                            cmdInsCons.CommandType = CommandType.StoredProcedure;
                            cmdInsCons.Parameters.Add("@flg_operacao", SqlDbType.Char).Value = "K";
                            cmdInsCons.Parameters.Add("@COD_ID", SqlDbType.Int).Value = Funcoes.strToInt(Convert.ToString(e.CommandArgument));
                            cmdInsCons.Parameters.Add("@COD_ID_PESSOA_LICENCIADO", SqlDbType.Int).Value = Licenciado;
                            cmdInsCons.Parameters.Add("@NOM_STATUS", SqlDbType.Char).Value = "canceled";
                            cmdInsCons.ExecuteNonQuery();
                            connInsCons.Close();
                            connInsCons.Dispose();


                            ClientScript.RegisterStartupScript(this.GetType(), "SucessoGeralAsaas", "alert('Transação cancelada com sucesso!');", true);

                            await ConsultaGeralAsync(0);

                        }
                        else
                        {
                            string mensagemErro = resultado.Content;
                            ClientScript.RegisterStartupScript(this.GetType(), "ErroGeralAsaas", "alert('Ocorreu um erro ao tentar cancelar a transação: " + mensagemErro + "');", true);
                        }
                    }
                }

                if ((ReaderCadastro["FLG_STATUS_OPERACAO"].ToString()=="S") && (ReaderCadastro["FLG_ORIGEM"].ToString()=="A"))
                {

                        string sApiKey = asaas.PegarTokenSubconta(Funcoes.strToInt(HttpContext.Current.Session["PESSOA"].ToString()), Funcoes.strToInt(HttpContext.Current.Session["LICENCIADO"].ToString()));
                        asaas.HttpResponseResult resultado = asaas.ExcluirCobrancaAsaas(sApiKey, ReaderCadastro["NOM_CODE"].ToString());

                        if ((resultado.StatusCode == 200) || (resultado.StatusCode == 201) || (resultado.StatusCode == 202))
                        {
                            // Atualiza STATUS para CANCELADA
                            SqlConnection connInsCons = new SqlConnection(Funcoes.conexao());
                            connInsCons.Open();
                            SqlCommand cmdInsCons = new SqlCommand("dbo.stp_transacoes_ins", connInsCons);
                            cmdInsCons.CommandType = CommandType.StoredProcedure;
                            cmdInsCons.Parameters.Add("@flg_operacao", SqlDbType.Char).Value = "K";
                            cmdInsCons.Parameters.Add("@COD_ID", SqlDbType.Int).Value = Funcoes.strToInt(Convert.ToString(e.CommandArgument));
                            cmdInsCons.Parameters.Add("@COD_ID_PESSOA_LICENCIADO", SqlDbType.Int).Value = Licenciado;
                            cmdInsCons.Parameters.Add("@NOM_STATUS", SqlDbType.Char).Value = "canceled";
                            cmdInsCons.ExecuteNonQuery();
                            connInsCons.Close();
                            connInsCons.Dispose();


                            ClientScript.RegisterStartupScript(this.GetType(), "SucessoGeralAsaas", "alert('Transação cancelada com sucesso!');", true);

                            await ConsultaGeralAsync(0);

                        }
                        else
                        {
                            string mensagemErro = resultado.Content;
                            ClientScript.RegisterStartupScript(this.GetType(), "ErroGeralAsaas", "alert('Ocorreu um erro ao tentar cancelar a transação: " + mensagemErro + "');", true);
                        }
                
                }
                
                if ((ReaderCadastro["FLG_STATUS_OPERACAO"].ToString()=="S") && (ReaderCadastro["FLG_ORIGEM"].ToString()=="Z"))
                {
                    ClientScript.RegisterStartupScript(this.GetType(),"TransacaoConcluida", "alert('Esta transação já se encontra concluída e não pode ser cancelada!');", true);
                }
                if (ReaderCadastro["FLG_STATUS_OPERACAO"].ToString()=="N")
                {
                    ClientScript.RegisterStartupScript(this.GetType(),"TransacaoConcluida", "alert('Esta transação já se encontra cancelada!');", true);
                }
            }

        }
    }

    protected void ExportarExcel(DataTable dt)
    {
        HttpContext.Current.Response.Clear();
        HttpContext.Current.Response.Buffer = true;
        HttpContext.Current.Response.Charset = "";
        HttpContext.Current.Response.ContentType = "application/vnd.ms-excel";
        HttpContext.Current.Response.AddHeader(
            "content-disposition",
            "attachment;filename=Cadastro_Estabelecimentos_" + DateTime.Now.ToString("yyyyMMdd_HHmmss") + ".xls"
        );

        using (System.IO.StringWriter sw = new System.IO.StringWriter())
        {
            using (System.Web.UI.HtmlTextWriter hw = new System.Web.UI.HtmlTextWriter(sw))
            {
                // Cria uma tabela HTML (Excel entende)
                System.Web.UI.WebControls.GridView gv = new System.Web.UI.WebControls.GridView();
                gv.DataSource = dt;
                gv.DataBind();

                gv.RenderControl(hw);

                HttpContext.Current.Response.Output.Write(sw.ToString());
                HttpContext.Current.Response.Flush();
                HttpContext.Current.Response.End();
            }
        }
    }


    protected void ExportarPDF(DataTable dt)
    {
        HttpContext.Current.Response.Clear();
        HttpContext.Current.Response.ContentType = "application/pdf";
        HttpContext.Current.Response.AddHeader(
            "content-disposition",
            "attachment;filename=Cadastro_Estabelecimentos_" + DateTime.Now.ToString("yyyyMMdd_HHmmss") + ".pdf"
        );
        HttpContext.Current.Response.Cache.SetCacheability(HttpCacheability.NoCache);

        using (Document document = new Document(PageSize.A4.Rotate(), 10f, 10f, 10f, 10f))
        {
            PdfWriter.GetInstance(document, HttpContext.Current.Response.OutputStream);
            document.Open();

            // 🔹 Fonte
            Font fontHeader = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 9);
            Font fontCell = FontFactory.GetFont(FontFactory.HELVETICA, 8);

            // 🔹 Tabela PDF
            PdfPTable table = new PdfPTable(dt.Columns.Count);
            table.WidthPercentage = 100;
            table.HeaderRows = 1;

            // 🔹 Cabeçalhos
            foreach (DataColumn column in dt.Columns)
            {
                PdfPCell headerCell = new PdfPCell(new Phrase(column.ColumnName, fontHeader))
                {
                    BackgroundColor = BaseColor.LIGHT_GRAY,
                    HorizontalAlignment = Element.ALIGN_CENTER,
                    Padding = 5
                };
                table.AddCell(headerCell);
            }

            // 🔹 Linhas
            foreach (DataRow row in dt.Rows)
            {
                foreach (var cell in row.ItemArray)
                {
                    string texto = cell == null ? "" : cell.ToString();

                    // Remove HTML (span, badge, etc)
                    texto = System.Text.RegularExpressions.Regex.Replace(texto, "<.*?>", string.Empty);

                    PdfPCell pdfCell = new PdfPCell(new Phrase(texto, fontCell))
                    {
                        Padding = 4
                    };

                    table.AddCell(pdfCell);
                }
            }

            document.Add(table);
            document.Close();
        }

        HttpContext.Current.Response.Flush();
        HttpContext.Current.Response.End();
    }
    protected void btnExportarExcel_Click(object sender, EventArgs e)
    {
        Funcoes.GravaAuditoriaNovo(0, Request.ServerVariables["REMOTE_ADDR"].ToString(), "Cadastro - Estabelecimentos", "Exportar Excel", Licenciado, Pessoa, Usuario);

        using (SqlConnection myConsulta = new SqlConnection(Funcoes.conexao()))
        {
            myConsulta.Open();
            SqlDataAdapter SDAConsulta = new SqlDataAdapter("dbo.stp_transacoes_paginado", myConsulta);
            SDAConsulta.SelectCommand.CommandType = CommandType.StoredProcedure;

            
                SDAConsulta.SelectCommand.Parameters.AddWithValue("@flg_operacao", "H");

                SDAConsulta.SelectCommand.Parameters.Add("@PageNumber", SqlDbType.Int).Value = PaginaAtual;
                SDAConsulta.SelectCommand.Parameters.Add("@PageSize", SqlDbType.Int).Value = TamanhoPagina;
                SDAConsulta.SelectCommand.Parameters.Add("@Exportar", SqlDbType.Bit).Value = 1;
                SDAConsulta.SelectCommand.Parameters.AddWithValue("@COD_ID_PESSOA_LICENCIADO", Licenciado);

                if (HttpContext.Current.Session["TIPO"].ToString().Trim()=="E")
                {
                    SDAConsulta.SelectCommand.Parameters.AddWithValue("@COD_ID_PESSOAS_FJ", Pessoa);
                }

                SDAConsulta.SelectCommand.Parameters.AddWithValue("@NOM_FILTRO", txtFiltro.Text);
                SDAConsulta.SelectCommand.Parameters.AddWithValue("@NOM_STATUS", ddlStatus.SelectedValue);
                SDAConsulta.SelectCommand.Parameters.AddWithValue("@NOM_TIPO_OPERACAO", ddlTipoPagamento.SelectedValue);
                SDAConsulta.SelectCommand.Parameters.AddWithValue("@NOM_RAZAOSOCIAL", txtEstabelecimento.Text.ToString());
                SDAConsulta.SelectCommand.Parameters.AddWithValue("@FLG_ORIGEM", ddlAdquirente.SelectedValue);

                SDAConsulta.SelectCommand.Parameters.AddWithValue("@DTA_INICIO", Convert.ToDateTime(txtDataIni.Text));
                SDAConsulta.SelectCommand.Parameters.AddWithValue("@DTA_FIM", Convert.ToDateTime(txtDataFim.Text));


                if (Funcoes.strToInt(ddlRepresentante.SelectedValue.Trim()) > 0)
                {
                    SDAConsulta.SelectCommand.Parameters.AddWithValue("@COD_ID_REPRESENTANTE", Funcoes.strToInt(ddlRepresentante.SelectedValue.Trim()));
                }
                if (Funcoes.strToInt(ddlMarketplace.SelectedValue.Trim()) > 0)
                {
                    SDAConsulta.SelectCommand.Parameters.AddWithValue("@COD_ID_MARKETPLACE", Funcoes.strToInt(ddlMarketplace.SelectedValue.Trim()));
                }

            DataSet dsConsulta = new DataSet();
            SDAConsulta.Fill(dsConsulta);

            ExportarExcel(dsConsulta.Tables[1]);
            myConsulta.Close(); myConsulta.Dispose();
        }


    }
    protected void btnExportarPDF_Click(object sender, EventArgs e)
    {
        Funcoes.GravaAuditoriaNovo(0, Request.ServerVariables["REMOTE_ADDR"].ToString(), "Cadastro - Estabelecimentos", "Exportar PDF", Licenciado, Pessoa, Usuario);

        using (SqlConnection myConsulta = new SqlConnection(Funcoes.conexao()))
        {
            myConsulta.Open();
            SqlDataAdapter SDAConsulta = new SqlDataAdapter("dbo.stp_transacoes_paginado", myConsulta);
            SDAConsulta.SelectCommand.CommandType = CommandType.StoredProcedure;

            
                SDAConsulta.SelectCommand.Parameters.AddWithValue("@flg_operacao", "H");

                SDAConsulta.SelectCommand.Parameters.Add("@PageNumber", SqlDbType.Int).Value = PaginaAtual;
                SDAConsulta.SelectCommand.Parameters.Add("@PageSize", SqlDbType.Int).Value = TamanhoPagina;
                SDAConsulta.SelectCommand.Parameters.Add("@Exportar", SqlDbType.Bit).Value = 1;
                SDAConsulta.SelectCommand.Parameters.AddWithValue("@COD_ID_PESSOA_LICENCIADO", Licenciado);

                if (HttpContext.Current.Session["TIPO"].ToString().Trim()=="E")
                {
                    SDAConsulta.SelectCommand.Parameters.AddWithValue("@COD_ID_PESSOAS_FJ", Pessoa);
                }

                SDAConsulta.SelectCommand.Parameters.AddWithValue("@NOM_FILTRO", txtFiltro.Text);
                SDAConsulta.SelectCommand.Parameters.AddWithValue("@NOM_STATUS", ddlStatus.SelectedValue);
                SDAConsulta.SelectCommand.Parameters.AddWithValue("@NOM_TIPO_OPERACAO", ddlTipoPagamento.SelectedValue);
                SDAConsulta.SelectCommand.Parameters.AddWithValue("@NOM_RAZAOSOCIAL", txtEstabelecimento.Text.ToString());
                SDAConsulta.SelectCommand.Parameters.AddWithValue("@FLG_ORIGEM", ddlAdquirente.SelectedValue);

                SDAConsulta.SelectCommand.Parameters.AddWithValue("@DTA_INICIO", Convert.ToDateTime(txtDataIni.Text));
                SDAConsulta.SelectCommand.Parameters.AddWithValue("@DTA_FIM", Convert.ToDateTime(txtDataFim.Text));


                if (Funcoes.strToInt(ddlRepresentante.SelectedValue.Trim()) > 0)
                {
                    SDAConsulta.SelectCommand.Parameters.AddWithValue("@COD_ID_REPRESENTANTE", Funcoes.strToInt(ddlRepresentante.SelectedValue.Trim()));
                }
                if (Funcoes.strToInt(ddlMarketplace.SelectedValue.Trim()) > 0)
                {
                    SDAConsulta.SelectCommand.Parameters.AddWithValue("@COD_ID_MARKETPLACE", Funcoes.strToInt(ddlMarketplace.SelectedValue.Trim()));
                }

            DataSet dsConsulta = new DataSet();
            SDAConsulta.Fill(dsConsulta);

            ExportarPDF(dsConsulta.Tables[1]);
            myConsulta.Close(); myConsulta.Dispose();
        }


    }

    private async Task CarregaStatusAsync()
{
    ddlStatus.Items.Clear();

    using (SqlConnection conn = new SqlConnection(Funcoes.conexao()))
    {
        await conn.OpenAsync();

        using (SqlCommand cmd = new SqlCommand("dbo.stp_status_ins", conn))
        {
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandTimeout = 60;
            cmd.Parameters.Add("@flg_operacao", SqlDbType.VarChar).Value = "L";

            using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
            {
                ddlStatus.DataSource = reader;
                ddlStatus.DataTextField = "NOM_STATUS_ADQUIRENTE";
                ddlStatus.DataValueField = "NOM_STATUS";
                ddlStatus.DataBind();
            }
        }
    }

    ddlStatus.Items.Insert(0, new System.Web.UI.WebControls.ListItem("Todos", ""));
}

/*    private async Task CarregaEstabelecimentosAsync(string tipo, int pessoa, int licenciado)
{
    ddlEstabelecimentos.Items.Clear();

    using (SqlConnection conn = new SqlConnection(Funcoes.conexao()))
    {
        await conn.OpenAsync();

        using (SqlCommand cmd = new SqlCommand("dbo.stp_pessoas_fj_ins", conn))
        {
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandTimeout = 60;

            cmd.Parameters.Add("@flg_operacao", SqlDbType.VarChar).Value = "Y";
            cmd.Parameters.Add("@COD_ID_PESSOA_LICENCIADO", SqlDbType.Int).Value = licenciado;
            cmd.Parameters.Add("@NOM_RAZAOSOCIAL", SqlDbType.VarChar).Value = "";
            cmd.Parameters.Add("@NUM_DOCUMENTO", SqlDbType.VarChar).Value = "";
            cmd.Parameters.Add("@NUM_TOKEN", SqlDbType.VarChar).Value = "";
            cmd.Parameters.Add("@FLG_ATIVO", SqlDbType.Char).Value = "";
            cmd.Parameters.Add("@FLG_TIPO", SqlDbType.Char).Value = "E";

            if (tipo == "M")
                cmd.Parameters.Add("@COD_ID_MARKETPLACE", SqlDbType.Int).Value = pessoa;

            if (tipo == "R")
                cmd.Parameters.Add("@COD_ID_REPRESENTANTE", SqlDbType.Int).Value = pessoa;

            using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
            {
                ddlEstabelecimentos.DataSource = reader;
                ddlEstabelecimentos.DataTextField = "NOM_RAZAOSOCIAL";
                ddlEstabelecimentos.DataValueField = "NOM_RAZAOSOCIAL";
                ddlEstabelecimentos.DataBind();
            }
        }
    }

    ddlEstabelecimentos.Items.Insert(0, new System.Web.UI.WebControls.ListItem("Todos", "0"));
}
 * */
private async Task CarregaRepresentantesAsync(string tipo, int pessoa, int licenciado)
{
    ddlRepresentante.Items.Clear();

    using (SqlConnection conn = new SqlConnection(Funcoes.conexao()))
    {
        await conn.OpenAsync();

        using (SqlCommand cmd = new SqlCommand("dbo.stp_pessoas_fj_ins", conn))
        {
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandTimeout = 60;

            cmd.Parameters.Add("@flg_operacao", SqlDbType.VarChar).Value = "L";
            cmd.Parameters.Add("@COD_ID_PESSOA_LICENCIADO", SqlDbType.Int).Value = licenciado;
            cmd.Parameters.Add("@FLG_ATIVO", SqlDbType.Char).Value = "S";
            cmd.Parameters.Add("@FLG_TIPO", SqlDbType.Char).Value = "R";

            if (tipo == "M")
                cmd.Parameters.Add("@COD_ID_MARKETPLACE", SqlDbType.Int).Value = pessoa;

            using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
            {
                ddlRepresentante.DataSource = reader;
                ddlRepresentante.DataTextField = "NOM_RAZAOSOCIAL";
                ddlRepresentante.DataValueField = "COD_ID";
                ddlRepresentante.DataBind();
            }
        }
    }

    ddlRepresentante.Items.Insert(0, new System.Web.UI.WebControls.ListItem("Todos", "0"));

    if (tipo == "R")
    {
        ddlRepresentante.SelectedValue = pessoa.ToString();
        ddlRepresentante.Enabled = false;
    }
}
private async Task CarregaMarketplacesAsync(string tipo, int pessoa, int licenciado)
{
    ddlMarketplace.Items.Clear();

    using (SqlConnection conn = new SqlConnection(Funcoes.conexao()))
    {
        await conn.OpenAsync();

        using (SqlCommand cmd = new SqlCommand("dbo.stp_pessoas_fj_ins", conn))
        {
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandTimeout = 60;

            cmd.Parameters.Add("@flg_operacao", SqlDbType.VarChar).Value = "L";
            cmd.Parameters.Add("@COD_ID_PESSOA_LICENCIADO", SqlDbType.Int).Value = licenciado;
            cmd.Parameters.Add("@FLG_ATIVO", SqlDbType.Char).Value = "S";
            cmd.Parameters.Add("@FLG_TIPO", SqlDbType.Char).Value = "M";

            using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
            {
                ddlMarketplace.DataSource = reader;
                ddlMarketplace.DataTextField = "NOM_RAZAOSOCIAL";
                ddlMarketplace.DataValueField = "COD_ID";
                ddlMarketplace.DataBind();
            }
        }
    }

    ddlMarketplace.Items.Insert(0, new System.Web.UI.WebControls.ListItem("Todos", "0"));

    if (tipo == "R" || tipo == "M")
    {
        ddlMarketplace.Enabled = false;

        if (tipo == "M")
            ddlMarketplace.SelectedValue = pessoa.ToString();
    }
}
private void AjustesPorTipo(string tipo)
{
    if (tipo == "E")
    {
        //ddlEstabelecimentos.Enabled = false;
        ddlMarketplace.Enabled = false;
        ddlRepresentante.Enabled = false;
    }
}


}