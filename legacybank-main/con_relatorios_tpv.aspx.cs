using System;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;

public partial class con_relatorios_tpv : System.Web.UI.Page
{
    private int Licenciado { get; set; }
    private string Tipo { get; set; }
    private int Pessoa { get; set; }
    private int Usuario { get; set; }

    public DateTime DataInicio
    {
        get
        {
            int ano = Funcoes.strToInt(ddlAno.SelectedValue);
            int mes = Funcoes.strToInt(ddlMes.SelectedValue);
            return new DateTime(ano, mes, 1);
        }
    }

    public DateTime DataFim
    {
        get
        {
            return DataInicio.AddMonths(1).AddDays(-1);
        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        // === CARGA SEGURA DE SESSION ===
        Licenciado = Session["LICENCIADO"] != null ? Funcoes.strToInt(Session["LICENCIADO"].ToString()) : 0;
        if (Session["TIPO"] != null)            Tipo = Session["TIPO"].ToString().Trim();        else            Tipo = "";
        Pessoa     = Session["PESSOA"] != null ? Funcoes.strToInt(Session["PESSOA"].ToString()) : 0;
        Usuario    = Session["CODIGO"] != null ? Funcoes.strToInt(Session["CODIGO"].ToString()) : 0;

        if (!IsPostBack)
        {
            CarregaAnos();
            CarregarMeses();

            ddlAno.SelectedValue = DateTime.Now.Year.ToString();
            ddlMes.SelectedValue = DateTime.Now.Month.ToString();

            CarregaRepresentantes();
            CarregaMarketplaces();
            CarregaAdquirentes();
        }
    }

    private void CarregarMeses()
    {
        ddlMes.Items.Clear();

        for (int mes = 1; mes <= 12; mes++)
        {
            ddlMes.Items.Add(
                new ListItem(
                    mes.ToString("00"), // Texto exibido (01, 02, ...)
                    mes.ToString()      // Value (1, 2, ...)
                )
            );
        }
    }
    private void CarregaAnos()
    {
        ddlAno.Items.Clear();

        int anoAtual = DateTime.Now.Year;
        int anoInicial = anoAtual - 10;

        for (int ano = anoInicial; ano <= anoAtual; ano++)
        {
            ddlAno.Items.Add(new ListItem(ano.ToString(), ano.ToString()));
        }

        ddlAno.SelectedValue = anoAtual.ToString();
    }


    // =========================================================
    // REPRESENTANTES
    // =========================================================
    private void CarregaRepresentantes()
    {
        using (SqlConnection con = new SqlConnection(Funcoes.conexao()))
        {
            con.Open();
            using (SqlCommand cmd = new SqlCommand("dbo.stp_pessoas_fj_ins", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@flg_operacao", SqlDbType.VarChar).Value = "L";
                cmd.Parameters.Add("@COD_ID_PESSOA_LICENCIADO", SqlDbType.Int).Value = Licenciado;
                cmd.Parameters.Add("@FLG_ATIVO", SqlDbType.Char).Value = "S";
                cmd.Parameters.Add("@FLG_TIPO", SqlDbType.Char).Value = "R";

                if (Tipo == "M")
                    cmd.Parameters.Add("@COD_ID_MARKETPLACE", SqlDbType.Int).Value = Pessoa;

                using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                {
                    DataSet ds = new DataSet();
                    da.Fill(ds);

                    ddlRepresentante.DataTextField = "NOM_RAZAOSOCIAL";
                    ddlRepresentante.DataValueField = "COD_ID";
                    ddlRepresentante.DataSource = ds.Tables[0];
                    ddlRepresentante.DataBind();
                    ddlRepresentante.Items.Insert(0, new ListItem("Todos", "0"));
                }

                if (Tipo == "R")
                {
                    ddlRepresentante.SelectedValue = Pessoa.ToString();
                    ddlRepresentante.Enabled = false;
                }
            }
        }
    }

    // =========================================================
    // MARKETPLACES
    // =========================================================
    private void CarregaMarketplaces()
    {
        using (SqlConnection con = new SqlConnection(Funcoes.conexao()))
        {
            con.Open();
            using (SqlCommand cmd = new SqlCommand("dbo.stp_pessoas_fj_ins", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@flg_operacao", SqlDbType.VarChar).Value = "L";
                cmd.Parameters.Add("@COD_ID_PESSOA_LICENCIADO", SqlDbType.Int).Value = Licenciado;
                cmd.Parameters.Add("@FLG_ATIVO", SqlDbType.Char).Value = "S";
                cmd.Parameters.Add("@FLG_TIPO", SqlDbType.Char).Value = "M";

                using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                {
                    DataSet ds = new DataSet();
                    da.Fill(ds);

                    ddlMarketplace.DataTextField = "NOM_RAZAOSOCIAL";
                    ddlMarketplace.DataValueField = "COD_ID";
                    ddlMarketplace.DataSource = ds.Tables[0];
                    ddlMarketplace.DataBind();
                    ddlMarketplace.Items.Insert(0, new ListItem("Todos", "0"));
                }

                if (Tipo == "M")
                {
                    ddlMarketplace.SelectedValue = Pessoa.ToString();
                    ddlMarketplace.Enabled = false;
                }
                else if (Tipo == "R")
                {
                    ddlMarketplace.Enabled = false;
                }
            }
        }
    }

    // =========================================================
    // ADQUIRENTES
    // =========================================================
    private void CarregaAdquirentes()
    {
        using (SqlConnection con = new SqlConnection(Funcoes.conexao()))
        {
            con.Open();
            using (SqlCommand cmd = new SqlCommand("dbo.stp_pessoas_fj_integracoes_ins", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@flg_operacao", SqlDbType.VarChar).Value = (Tipo == "L" ? "G" : "M");
                cmd.Parameters.Add("@COD_ID_PESSOA_LICENCIADO", SqlDbType.Int).Value = Licenciado;
                cmd.Parameters.Add("@COD_ID_PESSOAS_FJ", SqlDbType.Int).Value = Pessoa;
                cmd.Parameters.Add("@FLG_ATIVO", SqlDbType.Char).Value = "S";
                cmd.Parameters.Add("@FLG_ACESSO", SqlDbType.Char).Value = "S";

                using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                {
                    DataSet ds = new DataSet();
                    da.Fill(ds);

                    ddlAdquirente.DataTextField = "NOM_FANTASIA_INTEGRACAO";
                    ddlAdquirente.DataValueField = "FLG_INTEGRACAO";
                    ddlAdquirente.DataSource = ds.Tables[0];
                    ddlAdquirente.DataBind();
                    ddlAdquirente.Items.Insert(0, new ListItem("Todos", ""));
                }
            }
        }
    }

    // =========================================================
    // BOTÃO PESQUISAR
    // =========================================================
    protected void btnPesquisar_Click(object sender, EventArgs e)
    {
        ConsultaTotalGeral(); // Totais primeiro (UX e performance)
        ConsultaGeral();
    }

    // =========================================================
    // TOTAIS
    // =========================================================
    private void ConsultaTotalGeral()
    {
        DateTime dtaInicio = DataInicio;
        DateTime dtaFim = DataFim;
        
        lblValorTotalGeral.Text = "R$ 0,00";
        lblQtdeTotalGeral.Text = "0";
        lblValorTotalPendente.Text = "R$ 0,00";
        lblQtdeTotalPendente.Text = "0";
        lblValorTotalCancelada.Text = "R$ 0,00";
        lblQtdeTotalCancelada.Text = "0";

        using (SqlConnection con = new SqlConnection(Funcoes.conexao()))
        {
            con.Open();
            using (SqlCommand cmd = new SqlCommand("dbo.stp_relatorios_tpv_otimizado", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandTimeout = 120;

                cmd.Parameters.Add("@flg_operacao", SqlDbType.Char).Value = "G";
                cmd.Parameters.Add("@COD_ID_PESSOA_LICENCIADO", SqlDbType.Int).Value = Licenciado;

                cmd.Parameters.Add("@DTA_INICIO", SqlDbType.DateTime).Value = dtaInicio;
                cmd.Parameters.Add("@DTA_FIM", SqlDbType.DateTime).Value = dtaFim;

                //cmd.Parameters.Add("@NUM_ANO", SqlDbType.Int).Value = Funcoes.strToInt(ddlAno.SelectedValue);
                //cmd.Parameters.Add("@NUM_MES", SqlDbType.Int).Value = Funcoes.strToInt(ddlMes.SelectedValue);

                cmd.Parameters.Add("@FLG_ORIGEM", SqlDbType.Char).Value = ddlAdquirente.SelectedValue;
                cmd.Parameters.Add("@FLG_STATUS", SqlDbType.Char).Value = "%";

                if (Funcoes.strToInt(ddlRepresentante.SelectedValue) > 0)
                    cmd.Parameters.Add("@COD_ID_REPRESENTANTE", SqlDbType.Int).Value = ddlRepresentante.SelectedValue;

                if (Funcoes.strToInt(ddlMarketplace.SelectedValue) > 0)
                    cmd.Parameters.Add("@COD_ID_MARKETPLACE", SqlDbType.Int).Value = ddlMarketplace.SelectedValue;

                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        string status = dr["FLG_STATUS"].ToString().Trim();
                        double valor = Funcoes.strToDouble(dr["NUM_VALOR_TOTAL"].ToString());
                        double qtd = Funcoes.strToDouble(dr["NUM_TRANSACOES"].ToString());

                        if (status == "S")
                        {
                            lblValorTotalGeral.Text = String.Format("{0:c2}", valor);
                            lblQtdeTotalGeral.Text = String.Format("{0:n0}", qtd);
                        }
                        else if (status == "X")
                        {
                            lblValorTotalPendente.Text = String.Format("{0:c2}", valor);
                            lblQtdeTotalPendente.Text = String.Format("{0:n0}", qtd);
                        }
                        else if (status == "N")
                        {
                            lblValorTotalCancelada.Text = String.Format("{0:c2}", valor);
                            lblQtdeTotalCancelada.Text = String.Format("{0:n0}", qtd);
                        }
                    }
                }
            }
        }
    }

    // =========================================================
    // CONSULTA PRINCIPAL
    // =========================================================
    private void ConsultaGeral()
    {

        DateTime dtaInicio = DataInicio;
        DateTime dtaFim = DataFim;

        try
        {
            Funcoes.GravaAuditoriaNovo(0, Request.ServerVariables["REMOTE_ADDR"], "Relatórios - TPV", "Consulta: " + ddlMes.SelectedValue + "/" + ddlAno.SelectedValue, Licenciado, Pessoa, Usuario);
        }
        catch { }

        using (SqlConnection con = new SqlConnection(Funcoes.conexao()))
        {
            con.Open();
            using (SqlDataAdapter da = new SqlDataAdapter("dbo.stp_relatorios_tpv_otimizado", con))
            {
                da.SelectCommand.CommandType = CommandType.StoredProcedure;
                da.SelectCommand.CommandTimeout = 120;

                da.SelectCommand.Parameters.Add("@flg_operacao", SqlDbType.Char).Value = "T";
                da.SelectCommand.Parameters.Add("@COD_ID_PESSOA_LICENCIADO", SqlDbType.Int).Value = Licenciado;



                da.SelectCommand.Parameters.Add("@DTA_INICIO", SqlDbType.DateTime).Value = dtaInicio;
                da.SelectCommand.Parameters.Add("@DTA_FIM", SqlDbType.DateTime).Value = dtaFim;

                //da.SelectCommand.Parameters.Add("@NUM_ANO", SqlDbType.Int).Value = Funcoes.strToInt(txtAno.Text);
                //da.SelectCommand.Parameters.Add("@NUM_MES", SqlDbType.Int).Value = Funcoes.strToInt(txtMes.Text);
                da.SelectCommand.Parameters.Add("@FLG_ORIGEM", SqlDbType.Char).Value = ddlAdquirente.SelectedValue;

                if (Funcoes.strToInt(ddlRepresentante.SelectedValue) > 0)
                    da.SelectCommand.Parameters.Add("@COD_ID_REPRESENTANTE", SqlDbType.Int).Value = ddlRepresentante.SelectedValue;

                if (Funcoes.strToInt(ddlMarketplace.SelectedValue) > 0)
                    da.SelectCommand.Parameters.Add("@COD_ID_MARKETPLACE", SqlDbType.Int).Value = ddlMarketplace.SelectedValue;

                DataSet ds = new DataSet();
                da.Fill(ds);

                gvConsulta.DataSource = ds.Tables[0];
                gvConsulta.DataBind();
            }
        }
    }
    protected void btnImportarTexto_Click(object sender, EventArgs e)
    {

    }


}
