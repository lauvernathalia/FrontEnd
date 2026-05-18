using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
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

using System.Collections.Specialized;
using System.Globalization;
//using System.Drawing;
//using System.Drawing.Printing;


public partial class index : System.Web.UI.Page
{
    private int Licenciado;
    private int Pessoa;
    private string Tipo;

    private string TipoVendas
    {
        get
        {
            if (ViewState["VENDAS"] != null)
                return ViewState["VENDAS"].ToString();

            return "H"; // padrão HOJE
        }
        set
        {
            ViewState["VENDAS"] = value;
        }
    }


    private string TipoGraficoSelecionado
    {
        get
        {
            if (ViewState["GRAFICO"] != null)
                return ViewState["GRAFICO"].ToString();

            return "B"; // padrão HOJE
        }
        set
        {
            ViewState["GRAFICO"] = value;
        }
    }



    protected void Page_Load(object sender, EventArgs e)
    {
        // ===== SESSÃO SEGURA (PADRÃO LEGADO) =====
        Licenciado = (Session["LICENCIADO"] != null) ? Funcoes.strToInt(Session["LICENCIADO"].ToString()) : 0;
        Pessoa = (Session["PESSOA"] != null) ? Funcoes.strToInt(Session["PESSOA"].ToString()) : 0;
        Tipo = (Session["TIPO"] != null) ? Session["TIPO"].ToString().Trim() : "";

        // Verifica autenticação
        if (HttpContext.Current.Session == null || HttpContext.Current.Session.Count <= 0)
        {
            FormsAuthentication.SignOut();
            Response.Redirect("login.aspx");
            return;
        }

        // ===============================
        // CONFIGURAÇÕES INICIAIS
        // ===============================
        Funcoes.GravaAcesso("Acesso ao sistema");

        ctnAdministrador.Visible = false;
        cntEstabelecimento.Visible = false;
        ctnMarketplace.Visible = false;
        ctnRepresentante.Visible = false;

        cntPadrao.Visible = true;

        dvTitulo.Visible = true;
        dvData.Visible = true;
        dvAlerta.Visible = false;

        dvVendas.Visible = true;
        dvProgressao.Visible = true;
        dvTPV.Visible = true;
        dvFaturamento.Visible = true;
        dvBandeiras.Visible = true;

        // ===============================
        // PRIMEIRO LOAD
        // ===============================
        if (!IsPostBack)
        {
            ddlTop10Vendas.Items.Insert(0, new ListItem("Estabelecimentos", "E"));

            if (Tipo == "M")
                ddlTop10Vendas.Items.Insert(0, new ListItem("Representantes", "R"));

            if (Tipo == "A")
            {
                ddlTop10Vendas.Items.Insert(0, new ListItem("Representantes", "R"));
                ddlTop10Vendas.Items.Insert(0, new ListItem("Marketplaces", "M"));
            }

            CarregaAdquirentes();

            Funcoes.GravaAuditoria(
                0,
                Request.ServerVariables["REMOTE_ADDR"] != null ? Request.ServerVariables["REMOTE_ADDR"].ToString() : "",
                "Dashboard",
                "Acesso a tela de dashboard"
            );

            ContaDigital();
            ConsultaIntegracoes();

            PainelTitulo();
            PainelData();

            btnExtratoEDI.Visible = false;

            // ===============================
            // SALDO
            // ===============================

            ConsultaSaldoBaas("A");

            if (Tipo == "E")
                PainelDocumentos();
        }

        // ===============================
        // CHAT SUPORTE
        // ===============================
        if (Session["CHAT"] != null && Session["CHAT"].ToString() == "S")
        {
            if (Tipo == "M" || Tipo == "R" || Tipo == "E")
            {
                ClientScript.RegisterStartupScript(
                    this.GetType(),
                    "Chat",
                    "(function (s, u, i, t, e) { var share = s.createElement('script'); share.async = true; share.id = 'suiteshare'; share.src = 'https://static.suiteshare.com/widgets.js'; share.setAttribute('init', i); s.head.appendChild(share); })(document, 'script', '4160150588b0642f4af15797348d48354f3e01c8');",
                    true
                );
            }
        }

        // ===============================
        // VISÕES DO DASHBOARD
        // ===============================

        if (Tipo == "L")   // Administrador
        {
            dvVendas.Visible = true;
            dvTransacoesLicenciados.Visible = true;
            ConsultaDashboardAdministrador();

            dvProgressao.Visible = false;
            dvTPV.Visible = false;
            dvFaturamento.Visible = false;
            dvBandeiras.Visible = false;

            dvBotoes.Visible = false;
            dvSaldo.Visible = false;
            dvPlanos.Visible = false;
            dvComissoes.Visible = false;
            dvRentabilidade.Visible = false;
            dvMapa.Visible = false;
            dvTop10.Visible = false;
            dvMarketplace.Visible = false;
            dvExtrato.Visible = false;
            dvUltimasVendas.Visible = false;
        }

        if (Tipo == "E")   // Estabelecimento
        {
            int lic = Licenciado;

            hrfWidget01.Visible = (lic == 13654 || lic == 1);

            dvBotoes01.Visible = true;
            dvSaldo.Visible = true;

            CarregaSaldo();

            if (!IsPostBack)
                CarregaContas();

            dvPlanos.Visible = false;
            dvComissoes.Visible = false;
            dvRentabilidade.Visible = false;
            dvMapa.Visible = false;
            dvTop10.Visible = false;
            dvMarketplace.Visible = true;
            dvExtrato.Visible = true;
            dvUltimasVendas.Visible = true;
            dvFaturamento.Visible = false;
        }

        if (Tipo == "A" || Tipo == "M" || Tipo == "R")
        {
            dvBotoes01.Visible = false;
            dvSaldo.Visible = false;
            dvPlanos.Visible = true;
            dvComissoes.Visible = true;

            dvMapa.Visible = true;
            dvTop10.Visible = true;
            dvMarketplace.Visible = false;
            dvExtrato.Visible = false;
            dvUltimasVendas.Visible = false;
            dvFaturamento.Visible = true;
        }

        // ===============================
        // PAINÉIS
        // ===============================
        PainelCadastros();
        PainelTPV();
        PainelFaturamento();
        PainelBandeiras();
        PainelPlanos();
        PainelRepasses();

        if (Tipo == "A" || Tipo == "R" || Tipo == "M")
            PainelTop10Vendas();

        if (Tipo == "E")
        {
            PainelExtrato();
            PainelResumo();
        }

        if (!IsPostBack)
        {
            AcessoDashboardRepresentante();
            AcessoDashboardMarketplace();

            TipoVendas = "H";
            TipoGraficoSelecionado = "B";

            PainelMensagens();
            PainelAtualizacoes();
        }

        if (Tipo == "E")
            cntEstabelecimento.Visible = true;

        // ===============================
        // CONTROLE DE ACESSOS DASHBOARD
        // ===============================
        if (Tipo == "E")
        {
            using (SqlConnection conn = new SqlConnection(Funcoes.conexao()))
            using (SqlCommand cmd = new SqlCommand("dbo.stp_dashboard_acessos_ins", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@flg_operacao", SqlDbType.Char).Value = "L";
                cmd.Parameters.Add("@COD_ID_PESSOA_LICENCIADO", SqlDbType.Int).Value = Licenciado;
                cmd.Parameters.Add("@FLG_TIPO", SqlDbType.Char).Value = "E";

                conn.Open();

                using (SqlDataReader r = cmd.ExecuteReader())
                {
                    while (r.Read())
                    {
                        string dash = r["NOM_DASHBOARD"].ToString();
                        bool acesso = r["FLG_ACESSO"].ToString() == "S";

                        if (dash == "dvBotoes") dvBotoes.Visible = acesso;
                        if (dash == "dvVendas") dvVendas.Visible = acesso;
                        if (dash == "dvMovimentacao") dvMovimentacao.Visible = acesso;
                        if (dash == "dvSaldoConta") dvSaldoConta.Visible = acesso;
                        if (dash == "dvCanaisVendas") dvCanaisVendas.Visible = acesso;
                        if (dash == "dvProgressaoVendas") dvProgressaoVendas.Visible = acesso;
                        if (dash == "dvFormasPagamento") dvFormasPagamento.Visible = acesso;
                        if (dash == "dvTPV") dvTPV.Visible = acesso;
                        if (dash == "dvBandeiras") dvBandeiras.Visible = acesso;
                        if (dash == "dvExtrato") dvExtrato.Visible = acesso;
                        if (dash == "dvUltimasVendas") dvUltimasVendas.Visible = acesso;
                        if (dash == "dvMarketplace") dvMarketplace.Visible = acesso;
                    }
                }
            }
        }
    }



    private void ConsultaDashboardAdministrador()
    {
        DateTime dataInicio;
        DateTime dataFim;

        // ===== VALIDAÇÃO DE DATAS (PADRÃO LEGADO SEGURO) =====
        if (!DateTime.TryParse(txtDataIni.Text, out dataInicio))
            dataInicio = DateTime.Now.Date;

        if (!DateTime.TryParse(txtDataFim.Text, out dataFim))
            dataFim = DateTime.Now.Date;

        // ===================================================
        // TRANSAÇÕES POR LICENCIADO
        // ===================================================
        using (SqlConnection conn = new SqlConnection(Funcoes.conexao()))
        {
            using (SqlDataAdapter da = new SqlDataAdapter("dbo.stp_dashboard_administrador_ins", conn))
            {
                da.SelectCommand.CommandType = CommandType.StoredProcedure;
                da.SelectCommand.CommandTimeout = 0;

                da.SelectCommand.Parameters.Add("@flg_operacao", SqlDbType.Char).Value = "L";
                da.SelectCommand.Parameters.Add("@DTA_INICIO", SqlDbType.DateTime).Value = dataInicio;
                da.SelectCommand.Parameters.Add("@DTA_FIM", SqlDbType.DateTime).Value = dataFim;

                DataSet ds = new DataSet();
                da.Fill(ds, "TRANSACOES");

                rptConsultaTransacoesLicenciado.DataSource = ds.Tables["TRANSACOES"];
                rptConsultaTransacoesLicenciado.DataBind();
            }
        }

        // ===================================================
        // TRANSAÇÕES POR ADQUIRENTE
        // ===================================================
        using (SqlConnection conn = new SqlConnection(Funcoes.conexao()))
        {
            using (SqlDataAdapter da = new SqlDataAdapter("dbo.stp_dashboard_administrador_ins", conn))
            {
                da.SelectCommand.CommandType = CommandType.StoredProcedure;
                da.SelectCommand.CommandTimeout = 0;

                da.SelectCommand.Parameters.Add("@flg_operacao", SqlDbType.Char).Value = "F";
                da.SelectCommand.Parameters.Add("@DTA_INICIO", SqlDbType.DateTime).Value = dataInicio;
                da.SelectCommand.Parameters.Add("@DTA_FIM", SqlDbType.DateTime).Value = dataFim;

                DataSet ds = new DataSet();
                da.Fill(ds, "TRANSACOES");

                rptConsultaTransacoesAdquirente.DataSource = ds.Tables["TRANSACOES"];
                rptConsultaTransacoesAdquirente.DataBind();
            }
        }

        // ===================================================
        // ESTABELECIMENTOS POR LICENCIADO
        // ===================================================
        using (SqlConnection conn = new SqlConnection(Funcoes.conexao()))
        {
            using (SqlDataAdapter da = new SqlDataAdapter("dbo.stp_dashboard_administrador_ins", conn))
            {
                da.SelectCommand.CommandType = CommandType.StoredProcedure;
                da.SelectCommand.CommandTimeout = 0;

                da.SelectCommand.Parameters.Add("@flg_operacao", SqlDbType.Char).Value = "E";
                da.SelectCommand.Parameters.Add("@DTA_INICIO", SqlDbType.DateTime).Value = dataInicio;
                da.SelectCommand.Parameters.Add("@DTA_FIM", SqlDbType.DateTime).Value = dataFim;

                DataSet ds = new DataSet();
                da.Fill(ds, "PESSOAS_FJ");

                rptConsultaEstabelecimentosLicenciado.DataSource = ds.Tables["PESSOAS_FJ"];
                rptConsultaEstabelecimentosLicenciado.DataBind();
            }
        }
    }


    private void CarregaAdquirentes()
    {
        // ===================================================
        // DEFINIÇÃO DA OPERAÇÃO CONFORME O TIPO DO USUÁRIO
        // ===================================================
        string flgOperacao;

        if (Tipo == "L")
            flgOperacao = "G";
        else
            flgOperacao = "M";

        // ===================================================
        // CONSULTA
        // ===================================================
        using (SqlConnection conn = new SqlConnection(Funcoes.conexao()))
        {
            using (SqlCommand cmd = new SqlCommand("dbo.stp_pessoas_fj_integracoes_ins", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandTimeout = 0;

                cmd.Parameters.Add("@flg_operacao", SqlDbType.VarChar).Value = flgOperacao;
                cmd.Parameters.Add("@COD_ID_PESSOA_LICENCIADO", SqlDbType.Int).Value = Licenciado;
                cmd.Parameters.Add("@FLG_ATIVO", SqlDbType.Char).Value = "S";
                cmd.Parameters.Add("@FLG_ACESSO", SqlDbType.Char).Value = "S";

                using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                {
                    DataSet ds = new DataSet();
                    da.Fill(ds, "PESSOAS_FJ_INTEGRACOES");

                    ddlAdquirentes.DataTextField = "NOM_FANTASIA_INTEGRACAO";
                    ddlAdquirentes.DataValueField = "FLG_INTEGRACAO";
                    ddlAdquirentes.DataSource = ds.Tables["PESSOAS_FJ_INTEGRACOES"];
                    ddlAdquirentes.DataBind();
                }
            }
        }

        // ===================================================
        // ITEM PADRÃO
        // ===================================================
        ddlAdquirentes.Items.Insert(0, new ListItem("Todos", ""));
    }


    private void AcessoDashboardMarketplace()
    {
        if (Tipo == "M")
        {
            AcessoDashboard("M");
        }
    }


    private void AcessoDashboardRepresentante()
    {
        if (Tipo == "R")
        {
            AcessoDashboard("R");
        }
    }

    private void AcessoDashboard(string flgTipo)
    {
        using (SqlConnection conn = new SqlConnection(Funcoes.conexao()))
        {
            using (SqlCommand cmd = new SqlCommand("dbo.stp_dashboard_acessos_ins", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandTimeout = 0;

                cmd.Parameters.Add("@flg_operacao", SqlDbType.Char).Value = "L";
                cmd.Parameters.Add("@COD_ID_PESSOA_LICENCIADO", SqlDbType.Int).Value = Licenciado;
                cmd.Parameters.Add("@FLG_TIPO", SqlDbType.Char).Value = flgTipo;

                conn.Open();

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        string dashboard = reader["NOM_DASHBOARD"].ToString();
                        string acesso = reader["FLG_ACESSO"].ToString();

                        bool visivel = (acesso == "S");

                        if (dashboard == "dvVendas") dvVendas.Visible = visivel;
                        if (dashboard == "dvProgressaoVendas") dvProgressaoVendas.Visible = visivel;
                        if (dashboard == "dvFormasPagamento") dvFormasPagamento.Visible = visivel;
                        if (dashboard == "dvTPV") dvTPV.Visible = visivel;
                        if (dashboard == "dvFaturamento") dvFaturamento.Visible = visivel;
                        if (dashboard == "dvBandeiras") dvBandeiras.Visible = visivel;
                        if (dashboard == "dvPlanos") dvPlanos.Visible = visivel;
                        if (dashboard == "dvComissoes") dvComissoes.Visible = visivel;
                        // dvRentabilidade permanece desativado
                        if (dashboard == "dvMapa") dvMapa.Visible = visivel;
                        if (dashboard == "dvTop10") dvTop10.Visible = visivel;
                    }
                }
            }
        }
    }



    private void PainelDocumentos()
    {
        using (SqlConnection conn = new SqlConnection(Funcoes.conexao()))
        {
            using (SqlDataAdapter da = new SqlDataAdapter("dbo.stp_pessoas_fj_assinaturas_ins", conn))
            {
                da.SelectCommand.CommandType = CommandType.StoredProcedure;
                da.SelectCommand.CommandTimeout = 0;

                da.SelectCommand.Parameters.Add("@flg_operacao", SqlDbType.Char).Value = "T";
                da.SelectCommand.Parameters.Add("@COD_ID_PESSOA_LICENCIADO", SqlDbType.Int).Value = Licenciado;
                da.SelectCommand.Parameters.Add("@COD_ID_PESSOAS_FJ", SqlDbType.Int).Value = Pessoa;

                DataSet ds = new DataSet();
                da.Fill(ds, "PESSOAS_FJ_ASSINATURAS");

                rptDocumentos.DataSource = ds.Tables["PESSOAS_FJ_ASSINATURAS"].DefaultView;
                rptDocumentos.DataBind();
            }
        }
    }
    private void PainelMensagens()
    {
        using (SqlConnection conn = new SqlConnection(Funcoes.conexao()))
        {
            using (SqlDataAdapter da = new SqlDataAdapter("dbo.stp_mensagens_ins", conn))
            {
                da.SelectCommand.CommandType = CommandType.StoredProcedure;
                da.SelectCommand.CommandTimeout = 0;

                da.SelectCommand.Parameters.Add("@flg_operacao", SqlDbType.Char).Value = "M";
                da.SelectCommand.Parameters.Add("@COD_ID_PESSOA_LICENCIADO", SqlDbType.Int).Value = Licenciado;
                da.SelectCommand.Parameters.Add("@FLG_PERFIL", SqlDbType.Char).Value = Tipo;

                DataSet ds = new DataSet();
                da.Fill(ds, "MENSAGENS");

                rptMensagens.DataSource = ds.Tables["MENSAGENS"].DefaultView;
                rptMensagens.DataBind();
            }
        }
    }
    private void PainelAtualizacoes()
    {
        int iAtualizacoes = 0;

        using (SqlConnection conn = new SqlConnection(Funcoes.conexao()))
        {
            using (SqlCommand cmd = new SqlCommand("dbo.stp_atualizacoes_ins", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandTimeout = 0;

                cmd.Parameters.Add("@flg_operacao", SqlDbType.Char).Value = "M";
                cmd.Parameters.Add("@COD_ID_PESSOA_LICENCIADO", SqlDbType.Int).Value = Licenciado;
                cmd.Parameters.Add("@FLG_PERFIL", SqlDbType.Char).Value = Tipo;

                conn.Open();

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        iAtualizacoes++;
                    }
                }
            }
        }

        if (iAtualizacoes > 0)
        {
            divAtualizacoes.Visible = true;

            using (SqlConnection conn = new SqlConnection(Funcoes.conexao()))
            {
                using (SqlDataAdapter da = new SqlDataAdapter("dbo.stp_atualizacoes_ins", conn))
                {
                    da.SelectCommand.CommandType = CommandType.StoredProcedure;
                    da.SelectCommand.CommandTimeout = 0;

                    da.SelectCommand.Parameters.Add("@flg_operacao", SqlDbType.Char).Value = "M";
                    da.SelectCommand.Parameters.Add("@COD_ID_PESSOA_LICENCIADO", SqlDbType.Int).Value = Licenciado;
                    da.SelectCommand.Parameters.Add("@FLG_PERFIL", SqlDbType.Char).Value = Tipo;

                    DataSet ds = new DataSet();
                    da.Fill(ds, "ATUALIZACOES");

                    rptAtualizacoes.DataSource = ds.Tables["ATUALIZACOES"].DefaultView;
                    rptAtualizacoes.DataBind();
                }
            }
        }
        else
        {
            divAtualizacoes.Visible = false;
        }
    }



    public string TipoGrafico()
    {
        string sData = "bar";
        if (TipoGraficoSelecionado == "B") { sData = "'bar'"; }
        if (TipoGraficoSelecionado == "L") { sData = "'line'"; }
        return sData;
    }


    public string ConsultaSetor1(string sTipo)
    {
        string sData = "0,00";

        using (SqlConnection conn = new SqlConnection(Funcoes.conexao()))
        {
            using (SqlCommand cmd = new SqlCommand("dbo.stp_transacoes_ins", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandTimeout = 0;

                cmd.Parameters.Add("@flg_operacao", SqlDbType.Char).Value = "D";
                cmd.Parameters.Add("@COD_ID_PESSOA_LICENCIADO", SqlDbType.Int).Value = Licenciado;

                DateTime hoje = DateTime.Today;
                cmd.Parameters.Add("@DTA_INICIO", SqlDbType.DateTime).Value = hoje;
                cmd.Parameters.Add("@DTA_FIM", SqlDbType.DateTime).Value = hoje;

                if (Tipo == "M")
                {
                    cmd.Parameters.Add("@COD_ID_MARKETPLACE", SqlDbType.Int).Value = Pessoa;
                }

                if (Tipo == "R")
                {
                    cmd.Parameters.Add("@COD_ID_REPRESENTANTE", SqlDbType.Int).Value = Pessoa;
                }

                cmd.Parameters.Add("@FLG_SETOR", SqlDbType.Char).Value = "1";

                conn.Open();

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        if (reader["NOM_LEGENDA"].ToString().Trim() == "S")
                        {
                            if (sTipo == "A")
                            {
                                sData = String.Format(
                                    "{0:n2}",
                                    Funcoes.strToDouble(reader["NUM_VALOR_TOTAL"].ToString())
                                );
                            }

                            if (sTipo == "M")
                            {
                                sData = String.Format(
                                    "{0:n2}",
                                    Funcoes.strToDouble(reader["NUM_VALOR_MEDIO"].ToString())
                                );
                            }
                        }
                        else
                        {
                            if (sTipo == "F")
                            {
                                sData = String.Format(
                                    "{0:n2}",
                                    Funcoes.strToDouble(reader["NUM_VALOR_TOTAL"].ToString())
                                );
                            }
                        }
                    }
                }
            }
        }

        return sData;
    }



public string ConsultaGrafico1()
{
    string sData = "[";

    using (SqlConnection conn = new SqlConnection(Funcoes.conexao()))
    using (SqlCommand cmd = new SqlCommand("dbo.stp_transacoes_ins", conn))
    {
        cmd.CommandType = CommandType.StoredProcedure;

        cmd.Parameters.Add("@flg_operacao", SqlDbType.Char).Value = "D";
        cmd.Parameters.Add("@COD_ID_PESSOA_LICENCIADO", SqlDbType.Int).Value = Licenciado;

        // =========================
        // DEFINIÇÃO DE DATAS
        // =========================
        DateTime dtInicio;
        DateTime dtFim;

        if (TipoVendas == "O") // Ontem
        {
            dtInicio = DateTime.Today.AddDays(-1);
            dtFim    = DateTime.Today.AddDays(-1);
        }
        else if (TipoVendas == "H") // Hoje
        {
            dtInicio = DateTime.Today;
            dtFim    = DateTime.Today;
        }
        else if (TipoVendas == "S") // Semana
        {
            DateTime inicioSemana = Funcoes.GoToWeek(
                DateTime.Now.Year,
                Funcoes.strToInt(Funcoes.WeekOfYear(DateTime.Now).ToString())
            );

            dtInicio = inicioSemana;
            dtFim    = inicioSemana.AddDays(6);
        }
        else if (TipoVendas == "M") // Mês
        {
            DateTime hoje = DateTime.Today;
            dtInicio = new DateTime(hoje.Year, hoje.Month, 1);
            dtFim    = new DateTime(hoje.Year, hoje.Month,
                                    DateTime.DaysInMonth(hoje.Year, hoje.Month));
        }
        else if (TipoVendas == "A") // Ano
        {
            dtInicio = new DateTime(DateTime.Now.Year, 1, 1);
            dtFim    = new DateTime(DateTime.Now.Year, 12, 31);
        }
        else
        {
            // fallback seguro
            dtInicio = DateTime.Today;
            dtFim    = DateTime.Today;
        }

        cmd.Parameters.Add("@DTA_INICIO", SqlDbType.DateTime).Value = dtInicio;
        cmd.Parameters.Add("@DTA_FIM",    SqlDbType.DateTime).Value = dtFim;

        cmd.Parameters.Add("@FLG_SETOR", SqlDbType.Char).Value = "2";

        // =========================
        // REGRA POR TIPO DE USUÁRIO
        // =========================
        if (Tipo == "R")
        {
            cmd.Parameters.Add("@COD_ID_REPRESENTANTE", SqlDbType.Int).Value = Pessoa;
        }

        conn.Open();

        using (SqlDataReader reader = cmd.ExecuteReader())
        {
            while (reader.Read())
            {
                string valor = "0";

                if (reader["NUM_VALOR_TOTAL"] != DBNull.Value)
                {
                    valor = reader["NUM_VALOR_TOTAL"].ToString();
                }

                sData += valor.Replace(',', '.') + ",";
            }
        }
    }

    sData += "]";
    return sData;
}





public string ConsultaGrafico2()
{
    string sData = "[";

    SqlConnection mySelProgressaoVendas = new SqlConnection(Funcoes.conexao());
    mySelProgressaoVendas.Open();

    SqlCommand cmdSelProgressaoVendas = new SqlCommand("dbo.stp_transacoes_ins", mySelProgressaoVendas);
    cmdSelProgressaoVendas.CommandType = CommandType.StoredProcedure;

    cmdSelProgressaoVendas.Parameters.Add("@flg_operacao", SqlDbType.Char).Value = "D";
    cmdSelProgressaoVendas.Parameters.Add("@COD_ID_PESSOA_LICENCIADO", SqlDbType.Int).Value = Licenciado;

    cmdSelProgressaoVendas.Parameters.Add("@DTA_INICIO", SqlDbType.DateTime).Value =
        Convert.ToDateTime(DateTime.Now.ToShortDateString());

    cmdSelProgressaoVendas.Parameters.Add("@DTA_FIM", SqlDbType.DateTime).Value =
        Convert.ToDateTime(DateTime.Now.ToShortDateString());

    cmdSelProgressaoVendas.Parameters.Add("@FLG_SETOR", SqlDbType.Char).Value = "3";

    // PERFIL REPRESENTANTE
    if (Tipo == "R")
    {
        cmdSelProgressaoVendas.Parameters.Add("@COD_ID_REPRESENTANTE", SqlDbType.Int).Value = Pessoa;
    }

    SqlDataReader ReaderProgressaoVendas = cmdSelProgressaoVendas.ExecuteReader();

    while (ReaderProgressaoVendas.Read())
    {
        string legenda = "";

        if (ReaderProgressaoVendas["NOM_LEGENDA"] != DBNull.Value)
            legenda = ReaderProgressaoVendas["NOM_LEGENDA"].ToString();

        sData = sData + "'" + legenda + "',";
    }

    ReaderProgressaoVendas.Close();
    mySelProgressaoVendas.Close();
    mySelProgressaoVendas.Dispose();

    sData = sData + "]";
    return sData;
}


public string ConsultaGrafico3()
{
    string sData = "[";

    SqlConnection mySelProgressaoVendas = new SqlConnection(Funcoes.conexao());
    mySelProgressaoVendas.Open();

    SqlCommand cmdSelProgressaoVendas = new SqlCommand("dbo.stp_transacoes_ins", mySelProgressaoVendas);
    cmdSelProgressaoVendas.CommandType = CommandType.StoredProcedure;

    cmdSelProgressaoVendas.Parameters.Add("@flg_operacao", SqlDbType.Char).Value = "D";
    cmdSelProgressaoVendas.Parameters.Add("@COD_ID_PESSOA_LICENCIADO", SqlDbType.Int).Value = Licenciado;

    cmdSelProgressaoVendas.Parameters.Add("@DTA_INICIO", SqlDbType.DateTime).Value =
        Convert.ToDateTime(DateTime.Now.ToShortDateString());

    cmdSelProgressaoVendas.Parameters.Add("@DTA_FIM", SqlDbType.DateTime).Value =
        Convert.ToDateTime(DateTime.Now.ToShortDateString());

    cmdSelProgressaoVendas.Parameters.Add("@FLG_SETOR", SqlDbType.Char).Value = "3";

    // PERFIL REPRESENTANTE
    if (Tipo == "R")
    {
        cmdSelProgressaoVendas.Parameters.Add("@COD_ID_REPRESENTANTE", SqlDbType.Int).Value = Pessoa;
    }

    SqlDataReader ReaderProgressaoVendas = cmdSelProgressaoVendas.ExecuteReader();

    while (ReaderProgressaoVendas.Read())
    {
        string valor = "0";

        if (ReaderProgressaoVendas["NUM_VALOR_TOTAL"] != DBNull.Value)
            valor = ReaderProgressaoVendas["NUM_VALOR_TOTAL"].ToString();

        sData = sData + valor.Replace(',', '.') + ",";
    }

    ReaderProgressaoVendas.Close();
    mySelProgressaoVendas.Close();
    mySelProgressaoVendas.Dispose();

    sData = sData + "]";
    return sData;
}


    private void RecarregarProgressaoVendas()
    { 
    
    
    }


    // Novos botões de Progressão de Vendas
    protected void btnOntem_Click(object sender, EventArgs e)
    {
        TipoVendas = "O";
    }
    protected void btnHoje_Click(object sender, EventArgs e)
    {
        TipoVendas = "H";
    }
    protected void btnSemana_Click(object sender, EventArgs e)
    {
        TipoVendas = "S";
    }
    protected void btnMes_Click(object sender, EventArgs e)
    {
        TipoVendas = "M";
    }
    protected void btnAno_Click(object sender, EventArgs e)
    {
        TipoVendas = "A";
    }


    public string ConsultaTitulo()
    {
        string sData = "";
        if (TipoVendas == "O") { sData = "PROGRESSÃO DAS VENDAS - ONTEM"; }
        if (TipoVendas == "H") { sData = "PROGRESSÃO DAS VENDAS - HOJE"; }
        if (TipoVendas == "S") { sData = "PROGRESSÃO DAS VENDAS - SEMANA"; }
        if (TipoVendas == "M") { sData = "PROGRESSÃO DAS VENDAS - MÊS"; }
        if (TipoVendas == "A") { sData = "PROGRESSÃO DAS VENDAS - ANO"; }
        return sData;
    }

    // Paineis Padroes Licenciado, Marketplace, representantes

    private void PainelTitulo() 
    {
        if (HttpContext.Current.Session["TIPO"].ToString() == "A") { lblTitulo.Text = "Licenciado"; }
        if (HttpContext.Current.Session["TIPO"].ToString() == "M") { lblTitulo.Text = "Marketplace"; }
        if (HttpContext.Current.Session["TIPO"].ToString() == "R") { lblTitulo.Text = "Representante"; }
        if (HttpContext.Current.Session["TIPO"].ToString() == "E") { lblTitulo.Text = "Estabelecimento"; }
    }

    private void PainelData() 
    {
        DateTime DataAtual = DateTime.Now;
        string DiadaSemana = DataAtual.DayOfWeek.ToString();
        string DiadaSemanaExtenso = "";
        if (DiadaSemana == "Sunday")
        {
            DiadaSemanaExtenso = "Domingo";
        }
        if (DiadaSemana == "Monday")
        {
            DiadaSemanaExtenso = "Segunda-Feira";
        }
        if (DiadaSemana == "Tuesday")
        {
            DiadaSemanaExtenso = "Terça-Feira";
        }
        if (DiadaSemana == "Wednesday")
        {
            DiadaSemanaExtenso = "Quarta-Feira";
        }
        if (DiadaSemana == "Thursday")
        {
            DiadaSemanaExtenso = "Quinta-Feira";
        }
        if (DiadaSemana == "Friday")
        {
            DiadaSemanaExtenso = "Sexta-Feira";
        }
        if (DiadaSemana == "Saturday")
        {
            DiadaSemanaExtenso = "Sábado";
        }

        int Mes = DataAtual.Month;
        string MesExtenso = "";

        if (Mes == 1) { MesExtenso = "Janeiro"; }
        if (Mes == 2) { MesExtenso = "Fevereiro"; }
        if (Mes == 3) { MesExtenso = "Março"; }
        if (Mes == 4) { MesExtenso = "Abril"; }
        if (Mes == 5) { MesExtenso = "Maio"; }
        if (Mes == 6) { MesExtenso = "Junho"; }
        if (Mes == 7) { MesExtenso = "Julho"; }
        if (Mes == 8) { MesExtenso = "Agosto"; }
        if (Mes == 9) { MesExtenso = "Setembro"; }
        if (Mes == 10) { MesExtenso = "Outubro"; }
        if (Mes == 11) { MesExtenso = "Novembro"; }
        if (Mes == 12) { MesExtenso = "Dezembro"; }
        lblData.Text = DiadaSemanaExtenso + ", " + DataAtual.Day.ToString() + " de " + MesExtenso.ToString() + " de " + DataAtual.Year.ToString();

        //Vamos considerar que a data seja o dia de hoje, mas pode ser qualquer data.
        DateTime data = DateTime.Today;

        //DateTime com o primeiro dia do mês
        DateTime primeiroDiaDoMes = new DateTime(data.Year, data.Month, 1);

        //DateTime com o último dia do mês
        DateTime ultimoDiaDoMes = new DateTime(data.Year, data.Month, DateTime.DaysInMonth(data.Year, data.Month));


        //txtDataIni.Text = primeiroDiaDoMes.ToShortDateString();
        //txtDataFim.Text = DateTime.Now.ToShortDateString();

        txtDataIni.Text = DateTime.Now.ToShortDateString(); 
        txtDataFim.Text = DateTime.Now.ToShortDateString();

  
    }
    public string PainelVendas(string sTipo)
    {
        string sData = "0,00";

        using (SqlConnection mySelCadastro = new SqlConnection(Funcoes.conexao()))
        {
            mySelCadastro.Open();

            using (SqlCommand cmdSelCadastro = new SqlCommand("dbo.stp_transacoes_ins", mySelCadastro))
            {
                cmdSelCadastro.CommandTimeout = 0;
                cmdSelCadastro.CommandType = CommandType.StoredProcedure;

                cmdSelCadastro.Parameters.Add("@flg_operacao", SqlDbType.Char).Value = "D";

                // PERFIS QUE USAM LICENCIADO
                if (Tipo == "M" || Tipo == "R" || Tipo == "E" || Tipo == "A")
                {
                    cmdSelCadastro.Parameters.Add("@COD_ID_PESSOA_LICENCIADO", SqlDbType.Int).Value = Licenciado;
                }

                cmdSelCadastro.Parameters.Add("@DTA_INICIO", SqlDbType.DateTime)
                    .Value = Convert.ToDateTime(txtDataIni.Text);

                cmdSelCadastro.Parameters.Add("@DTA_FIM", SqlDbType.DateTime)
                    .Value = Convert.ToDateTime(txtDataFim.Text);

                // MARKETPLACE
                if (Tipo == "M")
                {
                    cmdSelCadastro.Parameters.Add("@COD_ID_MARKETPLACE", SqlDbType.Int).Value = Pessoa;
                }

                // REPRESENTANTE
                if (Tipo == "R")
                {
                    cmdSelCadastro.Parameters.Add("@COD_ID_REPRESENTANTE", SqlDbType.Int).Value = Pessoa;
                }

                // ESTABELECIMENTO
                if (Tipo == "E")
                {
                    cmdSelCadastro.Parameters.Add("@COD_ID_PESSOAS_FJ", SqlDbType.Int).Value = Pessoa;
                }

                cmdSelCadastro.Parameters.Add("@FLG_SETOR", SqlDbType.Char).Value = "1";
                cmdSelCadastro.Parameters.Add("@FLG_INTEGRACAO", SqlDbType.Char)
                    .Value = ddlAdquirentes.SelectedValue.ToString();

                using (SqlDataReader reader = cmdSelCadastro.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        string legenda = "";
                        if (reader["NOM_LEGENDA"] != DBNull.Value)
                            legenda = reader["NOM_LEGENDA"].ToString().Trim();

                        if (legenda == "S")
                        {
                            if (sTipo == "A" && reader["NUM_VALOR_TOTAL"] != DBNull.Value)
                                sData = String.Format("{0:n2}",
                                    Funcoes.strToDouble(reader["NUM_VALOR_TOTAL"].ToString()));

                            if (sTipo == "M" && reader["NUM_VALOR_MEDIO"] != DBNull.Value)
                                sData = String.Format("{0:n2}",
                                    Funcoes.strToDouble(reader["NUM_VALOR_MEDIO"].ToString()));

                            if (sTipo == "Q" && reader["NUM_TRANSACOES"] != DBNull.Value)
                                sData = String.Format("{0:n0}",
                                    Funcoes.strToDouble(reader["NUM_TRANSACOES"].ToString()));
                        }

                        if (legenda == "N")
                        {
                            if (sTipo == "F" && reader["NUM_VALOR_TOTAL"] != DBNull.Value)
                                sData = String.Format("{0:n2}",
                                    Funcoes.strToDouble(reader["NUM_VALOR_TOTAL"].ToString()));
                        }

                        if (legenda == "X")
                        {
                            if (sTipo == "P" && reader["NUM_VALOR_TOTAL"] != DBNull.Value)
                                sData = String.Format("{0:n2}",
                                    Funcoes.strToDouble(reader["NUM_VALOR_TOTAL"].ToString()));
                        }
                    }
                }
            }
        }

        return sData;
    }


    public string PainelProgressao(string sTipo)
    {
        btnHoje.Enabled = true;
        btnOntem.Enabled = true;

        string sData = "[";
        int iVendas = 0;

        using (SqlConnection mySelProgressaoVendas = new SqlConnection(Funcoes.conexao()))
        {
            mySelProgressaoVendas.Open();

            using (SqlCommand cmdSelProgressaoVendas =
                new SqlCommand("dbo.stp_transacoes_ins", mySelProgressaoVendas))
            {
                cmdSelProgressaoVendas.CommandTimeout = 0;
                cmdSelProgressaoVendas.CommandType = CommandType.StoredProcedure;

                cmdSelProgressaoVendas.Parameters.Add("@flg_operacao", SqlDbType.Char).Value = "D";
                cmdSelProgressaoVendas.Parameters.Add("@COD_ID_PESSOA_LICENCIADO", SqlDbType.Int).Value = Licenciado;

                // ===== PERÍODO =====
                if (TipoVendas == "")
                {
                    cmdSelProgressaoVendas.Parameters.Add("@DTA_INICIO", SqlDbType.DateTime)
                        .Value = Convert.ToDateTime(DateTime.Now.ToShortDateString() + " 00:00:00");
                    cmdSelProgressaoVendas.Parameters.Add("@DTA_FIM", SqlDbType.DateTime)
                        .Value = Convert.ToDateTime(DateTime.Now.ToShortDateString() + " 23:59:59");
                    cmdSelProgressaoVendas.Parameters.Add("@FLG_TOTAIS", SqlDbType.Char).Value = "D";
                    iVendas = 1;
                }

                if (TipoVendas == "O")
                {
                    cmdSelProgressaoVendas.Parameters.Add("@DTA_INICIO", SqlDbType.DateTime)
                        .Value = Convert.ToDateTime(DateTime.Now.AddDays(-1).ToShortDateString());
                    cmdSelProgressaoVendas.Parameters.Add("@DTA_FIM", SqlDbType.DateTime)
                        .Value = Convert.ToDateTime(DateTime.Now.AddDays(-1).ToShortDateString());
                    cmdSelProgressaoVendas.Parameters.Add("@FLG_TOTAIS", SqlDbType.Char).Value = "D";
                    iVendas = 1;
                }

                if (TipoVendas == "H")
                {
                    cmdSelProgressaoVendas.Parameters.Add("@DTA_INICIO", SqlDbType.DateTime)
                        .Value = Convert.ToDateTime(DateTime.Now.ToShortDateString() + " 00:00:00");
                    cmdSelProgressaoVendas.Parameters.Add("@DTA_FIM", SqlDbType.DateTime)
                        .Value = Convert.ToDateTime(DateTime.Now.ToShortDateString() + " 23:59:59");
                    cmdSelProgressaoVendas.Parameters.Add("@FLG_TOTAIS", SqlDbType.Char).Value = "D";
                    iVendas = 1;
                }

                if (TipoVendas == "S")
                {
                    DateTime inicio = Funcoes.GoToWeek(DateTime.Now.Year,
                        Funcoes.strToInt(Funcoes.WeekOfYear(DateTime.Now).ToString()));

                    cmdSelProgressaoVendas.Parameters.Add("@DTA_INICIO", SqlDbType.DateTime).Value = inicio;
                    cmdSelProgressaoVendas.Parameters.Add("@DTA_FIM", SqlDbType.DateTime).Value = inicio.AddDays(6);
                    cmdSelProgressaoVendas.Parameters.Add("@FLG_TOTAIS", SqlDbType.Char).Value = "S";
                    iVendas = 1;
                }

                if (TipoVendas == "M")
                {
                    DateTime data = DateTime.Today;
                    DateTime primeiroDia = new DateTime(data.Year, data.Month, 1);
                    DateTime ultimoDia = new DateTime(data.Year, data.Month,
                        DateTime.DaysInMonth(data.Year, data.Month));

                    cmdSelProgressaoVendas.Parameters.Add("@DTA_INICIO", SqlDbType.DateTime).Value = primeiroDia;
                    cmdSelProgressaoVendas.Parameters.Add("@DTA_FIM", SqlDbType.DateTime).Value = ultimoDia;
                    cmdSelProgressaoVendas.Parameters.Add("@FLG_TOTAIS", SqlDbType.Char).Value = "M";
                    iVendas = 1;
                }

                if (TipoVendas == "A")
                {
                    cmdSelProgressaoVendas.Parameters.Add("@DTA_INICIO", SqlDbType.DateTime)
                        .Value = Convert.ToDateTime("01/01/" + DateTime.Now.Year);
                    cmdSelProgressaoVendas.Parameters.Add("@DTA_FIM", SqlDbType.DateTime)
                        .Value = Convert.ToDateTime("31/12/" + DateTime.Now.Year);
                    cmdSelProgressaoVendas.Parameters.Add("@FLG_TOTAIS", SqlDbType.Char).Value = "A";
                    iVendas = 1;
                }

                if (iVendas == 0)
                {
                    cmdSelProgressaoVendas.Parameters.Add("@DTA_INICIO", SqlDbType.DateTime)
                        .Value = Convert.ToDateTime(DateTime.Now.ToShortDateString() + " 00:00:00");
                    cmdSelProgressaoVendas.Parameters.Add("@DTA_FIM", SqlDbType.DateTime)
                        .Value = Convert.ToDateTime(DateTime.Now.ToShortDateString() + " 23:59:59");
                    cmdSelProgressaoVendas.Parameters.Add("@FLG_TOTAIS", SqlDbType.Char).Value = "D";
                }

                // ===== FILTROS =====
                cmdSelProgressaoVendas.Parameters.Add("@FLG_SETOR", SqlDbType.Char).Value = "2";
                cmdSelProgressaoVendas.Parameters.Add("@FLG_INTEGRACAO", SqlDbType.Char)
                    .Value = ddlAdquirentes.SelectedValue.ToString();

                if (Tipo == "M")
                    cmdSelProgressaoVendas.Parameters.Add("@COD_ID_MARKETPLACE", SqlDbType.Int).Value = Pessoa;

                if (Tipo == "R")
                    cmdSelProgressaoVendas.Parameters.Add("@COD_ID_REPRESENTANTE", SqlDbType.Int).Value = Pessoa;

                if (Tipo == "E")
                    cmdSelProgressaoVendas.Parameters.Add("@COD_ID_PESSOAS_FJ", SqlDbType.Int).Value = Pessoa;

                // ===== LEITURA =====
                using (SqlDataReader reader = cmdSelProgressaoVendas.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        if (sTipo == "V")
                        {
                            string valor = "0";
                            if (reader["NUM_VALOR_TOTAL"] != DBNull.Value)
                                valor = reader["NUM_VALOR_TOTAL"].ToString();

                            sData += valor.Replace(',', '.') + ",";
                        }

                        if (sTipo == "L")
                        {
                            string legenda = "";
                            if (reader["NOM_LEGENDA"] != DBNull.Value)
                                legenda = reader["NOM_LEGENDA"].ToString();

                            sData += "'" + legenda + "',";
                        }

                        if (sTipo == "C")
                        {
                            string urlorigem = Request.ServerVariables["SERVER_NAME"].ToString();

                            using (SqlConnection mySelCadastro = new SqlConnection(Funcoes.conexao()))
                            {
                                mySelCadastro.Open();

                                using (SqlCommand cmdSelCadastro =
                                    new SqlCommand("dbo.stp_pessoas_fj_ins", mySelCadastro))
                                {
                                    cmdSelCadastro.CommandType = CommandType.StoredProcedure;
                                    cmdSelCadastro.Parameters.Add("@flg_operacao", SqlDbType.Char).Value = "T";
                                    cmdSelCadastro.Parameters.Add("@NOM_URL", SqlDbType.VarChar).Value = urlorigem;

                                    using (SqlDataReader rd = cmdSelCadastro.ExecuteReader())
                                    {
                                        int iContador = 1;
                                        while (rd.Read())
                                        {
                                            if (iContador <= 1 && rd["NOM_COR_PRIMARIA_FUNDO"] != DBNull.Value)
                                            {
                                                sData += "'" + rd["NOM_COR_PRIMARIA_FUNDO"].ToString() + "',";
                                                iContador++;
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        sData += "]";
        return sData;
    }





    public string PainelFormasPagamento(string sTipo) 
    {
        SqlConnection mySelProgressaoVendas = new SqlConnection(Funcoes.conexao());
        mySelProgressaoVendas.Open();
        SqlCommand cmdSelProgressaoVendas = new SqlCommand("dbo.stp_transacoes_ins", mySelProgressaoVendas);
        cmdSelProgressaoVendas.CommandTimeout = 0;
        cmdSelProgressaoVendas.CommandType = CommandType.StoredProcedure;
        cmdSelProgressaoVendas.Parameters.Add("@flg_operacao", SqlDbType.Char).Value = "D";
        cmdSelProgressaoVendas.Parameters.Add("@COD_ID_PESSOA_LICENCIADO", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["LICENCIADO"].ToString());
        cmdSelProgressaoVendas.Parameters.Add("@DTA_INICIO", SqlDbType.DateTime).Value = Convert.ToDateTime(txtDataIni.Text.ToString());
        cmdSelProgressaoVendas.Parameters.Add("@DTA_FIM", SqlDbType.DateTime).Value = Convert.ToDateTime(txtDataFim.Text.ToString());
        cmdSelProgressaoVendas.Parameters.Add("@FLG_SETOR", SqlDbType.Char).Value = "3";
        cmdSelProgressaoVendas.Parameters.Add("@FLG_INTEGRACAO", SqlDbType.Char).Value = ddlAdquirentes.SelectedValue.ToString();

        if (HttpContext.Current.Session["TIPO"].ToString() == "M")
        {
            cmdSelProgressaoVendas.Parameters.Add("@COD_ID_MARKETPLACE", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["PESSOA"].ToString());
        }
        if (HttpContext.Current.Session["TIPO"].ToString() == "R")
        {
            cmdSelProgressaoVendas.Parameters.Add("@COD_ID_REPRESENTANTE", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["PESSOA"].ToString());
        }
        if (HttpContext.Current.Session["TIPO"].ToString() == "E")
        {
            cmdSelProgressaoVendas.Parameters.Add("@COD_ID_PESSOAS_FJ", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["PESSOA"].ToString());
        }


        SqlDataReader ReaderProgressaoVendas = cmdSelProgressaoVendas.ExecuteReader();
        string sData = "[";
        int iContador = 1;
        while (ReaderProgressaoVendas.Read())
        {
            if (sTipo == "L") { sData = sData + "'" + ReaderProgressaoVendas["NOM_LEGENDA"].ToString() + "',"; }
            if (sTipo == "V") { sData = sData + ReaderProgressaoVendas["NUM_VALOR_TOTAL"].ToString().Replace(',', '.') + ","; }
            if (sTipo == "C") { sData = sData + "'" + Funcoes.colorTween(Funcoes.CoresWhitelabel(Request.ServerVariables["SERVER_NAME"].ToString(), 1).ToString(), Funcoes.CoresWhitelabel(Request.ServerVariables["SERVER_NAME"].ToString(), 2).ToString(), iContador * 20) + "',"; }
            iContador = iContador + 1;
        }
        sData = sData + "]";
        return sData;
    }

    private void PainelCadastros()
    {
        SqlConnection myConsulta = new SqlConnection(Funcoes.conexao());
        myConsulta.Open();
        SqlDataAdapter SDAConsulta = new SqlDataAdapter("dbo.stp_transacoes_ins", myConsulta);
        SDAConsulta.SelectCommand.CommandTimeout = 0;
        SDAConsulta.SelectCommand.CommandType = CommandType.StoredProcedure;
        SDAConsulta.SelectCommand.Parameters.Add("@flg_operacao", SqlDbType.Char).Value = "D";
        SDAConsulta.SelectCommand.Parameters.Add("@COD_ID_PESSOA_LICENCIADO", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["LICENCIADO"].ToString());

        if (HttpContext.Current.Session["TIPO"].ToString() == "M")
        {
            SDAConsulta.SelectCommand.Parameters.Add("@COD_ID_MARKETPLACE", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["PESSOA"].ToString());
        }
        if (HttpContext.Current.Session["TIPO"].ToString() == "R")
        {
            SDAConsulta.SelectCommand.Parameters.Add("@COD_ID_REPRESENTANTE", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["PESSOA"].ToString());
        }
        SDAConsulta.SelectCommand.Parameters.Add("@FLG_SETOR", SqlDbType.Char).Value = "7";

        DataSet dsConsulta = new DataSet();
        SDAConsulta.Fill(dsConsulta, "#resultado");
        rptCadastros.DataSource = dsConsulta.Tables["#resultado"].DefaultView;
        rptCadastros.DataBind();

        myConsulta.Close(); myConsulta.Dispose();

    }
    
    private void PainelTPV() 
    {
        SqlConnection myConsulta = new SqlConnection(Funcoes.conexao());
        myConsulta.Open();
        SqlDataAdapter SDAConsulta = new SqlDataAdapter("dbo.stp_transacoes_ins", myConsulta);
        SDAConsulta.SelectCommand.CommandTimeout = 0;
        SDAConsulta.SelectCommand.CommandType = CommandType.StoredProcedure;
        SDAConsulta.SelectCommand.Parameters.Add("@flg_operacao", SqlDbType.Char).Value = "D";
        SDAConsulta.SelectCommand.Parameters.Add("@COD_ID_PESSOA_LICENCIADO", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["LICENCIADO"].ToString());
        SDAConsulta.SelectCommand.Parameters.Add("@DTA_INICIO", SqlDbType.DateTime).Value = Convert.ToDateTime(txtDataIni.Text.ToString());
        SDAConsulta.SelectCommand.Parameters.Add("@DTA_FIM", SqlDbType.DateTime).Value = Convert.ToDateTime(txtDataFim.Text.ToString());

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
        SDAConsulta.SelectCommand.Parameters.Add("@FLG_SETOR", SqlDbType.Char).Value = "4";
        SDAConsulta.SelectCommand.Parameters.Add("@FLG_INTEGRACAO", SqlDbType.Char).Value = ddlAdquirentes.SelectedValue.ToString();

        DataSet dsConsulta = new DataSet();
        SDAConsulta.Fill(dsConsulta, "#resultado");
        rptTPV.DataSource = dsConsulta.Tables["#resultado"].DefaultView;
        rptTPV.DataBind();

        myConsulta.Close(); myConsulta.Dispose();
       
    }
    private void PainelFaturamento() 
    {
        SqlConnection myConsulta = new SqlConnection(Funcoes.conexao());
        myConsulta.Open();
        SqlDataAdapter SDAConsulta = new SqlDataAdapter("dbo.stp_transacoes_ins", myConsulta);
        SDAConsulta.SelectCommand.CommandTimeout = 0;
        SDAConsulta.SelectCommand.CommandType = CommandType.StoredProcedure;
        SDAConsulta.SelectCommand.Parameters.Add("@flg_operacao", SqlDbType.Char).Value = "D";
        SDAConsulta.SelectCommand.Parameters.Add("@COD_ID_PESSOA_LICENCIADO", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["LICENCIADO"].ToString());
        SDAConsulta.SelectCommand.Parameters.Add("@DTA_INICIO", SqlDbType.DateTime).Value = Convert.ToDateTime(txtDataIni.Text.ToString());
        SDAConsulta.SelectCommand.Parameters.Add("@DTA_FIM", SqlDbType.DateTime).Value = Convert.ToDateTime(txtDataFim.Text.ToString());

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
        SDAConsulta.SelectCommand.Parameters.Add("@FLG_SETOR", SqlDbType.Char).Value = "8";
        SDAConsulta.SelectCommand.Parameters.Add("@FLG_INTEGRACAO", SqlDbType.Char).Value = ddlAdquirentes.SelectedValue.ToString();


        DataSet dsConsulta = new DataSet();
        SDAConsulta.Fill(dsConsulta, "#resultado");
        rpvFaturamento.DataSource = dsConsulta.Tables["#resultado"].DefaultView;
        rpvFaturamento.DataBind();

        myConsulta.Close(); myConsulta.Dispose();
    }
    
    private void PainelBandeiras() 
    {
        SqlConnection myConsulta = new SqlConnection(Funcoes.conexao());
        myConsulta.Open();
        SqlDataAdapter SDAConsulta = new SqlDataAdapter("dbo.stp_transacoes_ins", myConsulta);
        SDAConsulta.SelectCommand.CommandTimeout = 0;
        SDAConsulta.SelectCommand.CommandType = CommandType.StoredProcedure;
        SDAConsulta.SelectCommand.Parameters.Add("@flg_operacao", SqlDbType.Char).Value = "D";
        SDAConsulta.SelectCommand.Parameters.Add("@COD_ID_PESSOA_LICENCIADO", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["LICENCIADO"].ToString());
        SDAConsulta.SelectCommand.Parameters.Add("@DTA_INICIO", SqlDbType.DateTime).Value = Convert.ToDateTime(txtDataIni.Text.ToString());
        SDAConsulta.SelectCommand.Parameters.Add("@DTA_FIM", SqlDbType.DateTime).Value = Convert.ToDateTime(txtDataFim.Text.ToString());

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
        SDAConsulta.SelectCommand.Parameters.Add("@FLG_SETOR", SqlDbType.Char).Value = "6";
        SDAConsulta.SelectCommand.Parameters.Add("@FLG_INTEGRACAO", SqlDbType.Char).Value = ddlAdquirentes.SelectedValue.ToString();

        DataSet dsConsulta = new DataSet();
        SDAConsulta.Fill(dsConsulta, "#resultado");
        rptBandeiras.DataSource = dsConsulta.Tables["#resultado"].DefaultView;
        rptBandeiras.DataBind();
        myConsulta.Close(); myConsulta.Dispose();

    }

    private void PainelPlanos() 
    {
        SqlConnection mySelCadastro = new SqlConnection(Funcoes.conexao());
        mySelCadastro.Open();
        SqlCommand cmdSelCadastro = new SqlCommand("dbo.stp_transacoes_ins", mySelCadastro);
        cmdSelCadastro.CommandTimeout = 0;
        cmdSelCadastro.CommandType = CommandType.StoredProcedure;
        cmdSelCadastro.Parameters.Add("@flg_operacao", SqlDbType.Char).Value = "D";
        cmdSelCadastro.Parameters.Add("@COD_ID_PESSOA_LICENCIADO", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["LICENCIADO"].ToString());
        cmdSelCadastro.Parameters.Add("@DTA_INICIO", SqlDbType.DateTime).Value = Convert.ToDateTime(txtDataIni.Text.ToString());
        cmdSelCadastro.Parameters.Add("@DTA_FIM", SqlDbType.DateTime).Value = Convert.ToDateTime(txtDataFim.Text.ToString());

        if (HttpContext.Current.Session["TIPO"].ToString() == "M")
        {
            cmdSelCadastro.Parameters.Add("@COD_ID_MARKETPLACE", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["PESSOA"].ToString());
        }


        if (HttpContext.Current.Session["TIPO"].ToString() == "R")
        {
            cmdSelCadastro.Parameters.Add("@COD_ID_REPRESENTANTE", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["PESSOA"].ToString());
        }

        if (HttpContext.Current.Session["TIPO"].ToString() == "E")
        {
            cmdSelCadastro.Parameters.Add("@COD_ID_PESSOAS_FJ", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["PESSOA"].ToString());
        }

        cmdSelCadastro.Parameters.Add("@FLG_SETOR", SqlDbType.Char).Value = "9";
        cmdSelCadastro.Parameters.Add("@FLG_INTEGRACAO", SqlDbType.Char).Value = ddlAdquirentes.SelectedValue.ToString();
        SqlDataReader ReaderCadastro = cmdSelCadastro.ExecuteReader();
        while (ReaderCadastro.Read())
        {
            if (ReaderCadastro["NUM_ORDEM"].ToString().Trim() == "1")
            {
                lblPlanoMaisUsado.Text = ReaderCadastro["NOM_LEGENDA"].ToString(); 
            }
            else
            {
                lblPlanoMenosUsado.Text = ReaderCadastro["NOM_LEGENDA"].ToString(); 
            }
        }

    }
    
    private void PainelRepasses() 
    {
        dvComissaoTotal.Visible = false;
        dvComissaoRepasse.Visible=false;
        dvComissaoMKT.Visible= false;
        dvComissaoREP.Visible = false;

        SqlConnection mySelCadastro = new SqlConnection(Funcoes.conexao());
        mySelCadastro.Open();
        SqlCommand cmdSelCadastro = new SqlCommand("dbo.stp_pessoas_fj_conta_corrente_ins", mySelCadastro);
        cmdSelCadastro.CommandTimeout = 0;
        cmdSelCadastro.CommandType = CommandType.StoredProcedure;

        cmdSelCadastro.Parameters.Add("@flg_operacao", SqlDbType.Char).Value = "R";
        cmdSelCadastro.Parameters.Add("@FLG_MODELO", SqlDbType.Char).Value = "D";

        cmdSelCadastro.Parameters.Add("@FLG_ORIGEM", SqlDbType.Char).Value = ddlAdquirentes.SelectedValue.ToString();
        cmdSelCadastro.Parameters.Add("@COD_ID_PESSOA_LICENCIADO", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["LICENCIADO"].ToString());

        cmdSelCadastro.Parameters.Add("@DTA_INICIO", SqlDbType.DateTime).Value = Convert.ToDateTime(txtDataIni.Text.ToString());
        cmdSelCadastro.Parameters.Add("@DTA_FIM", SqlDbType.DateTime).Value = Convert.ToDateTime(txtDataFim.Text.ToString());

        if (HttpContext.Current.Session["TIPO"].ToString() == "M")
        {
            cmdSelCadastro.Parameters.Add("@COD_ID_MARKETPLACE", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["PESSOA"].ToString());
        }

        if (HttpContext.Current.Session["TIPO"].ToString() == "R")
        {
            cmdSelCadastro.Parameters.Add("@COD_ID_REPRESENTANTE", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["PESSOA"].ToString());
        }

        SqlDataReader ReaderCadastro = cmdSelCadastro.ExecuteReader();
        while (ReaderCadastro.Read())
        {
            lblComissaoTotal.Text = String.Format("{0:n2}",Funcoes.strToDouble(ReaderCadastro["NUM_COMISSAO_TOTAL"].ToString()));
            lblComissaoRepasse.Text = String.Format("{0:n2}",Funcoes.strToDouble(ReaderCadastro["NUM_COMISSAO"].ToString()));
            lblComissaoMKT.Text = String.Format("{0:n2}",Funcoes.strToDouble(ReaderCadastro["NUM_COMISSAO_MKT"].ToString()));
            lblComissaoREP.Text = String.Format("{0:n2}",Funcoes.strToDouble(ReaderCadastro["NUM_COMISSAO_REP"].ToString()));
        }


        if (HttpContext.Current.Session["TIPO"].ToString() == "A")
        {
            dvComissaoTotal.Visible = true;
            dvComissaoRepasse.Visible = true;
            dvComissaoMKT.Visible = true;
            dvComissaoREP.Visible = true;

        }
        if (HttpContext.Current.Session["TIPO"].ToString() == "M")
        {
            dvComissaoTotal.Visible = false;
            dvComissaoRepasse.Visible = true;
            dvComissaoMKT.Visible = false;
            dvComissaoREP.Visible = true;
            lblTituloComissaoRepasse.Text = "Comissão Total";

        }
        if (HttpContext.Current.Session["TIPO"].ToString() == "R")
        {
            dvComissaoTotal.Visible = false;
            dvComissaoRepasse.Visible = false;
            dvComissaoMKT.Visible = false;
            dvComissaoREP.Visible = true;
            lblTituloComissaoREP.Text = "Comissão Total";
        }



    }

    private void PainelMapa() { }
    private void PainelComissao() { }
    private void PainelEstabelecimentos() { }


    private void PainelTop10Vendas()
    {
        try
        {
            using (SqlConnection myConsulta = new SqlConnection(Funcoes.conexao()))
            using (SqlDataAdapter sdaConsulta = new SqlDataAdapter("dbo.stp_dashboard_ins", myConsulta))
            {
                SqlCommand cmd = sdaConsulta.SelectCommand;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandTimeout = 120; // Timeout em segundos

                // Parâmetros fixos
                cmd.Parameters.Add("@flg_operacao", SqlDbType.Char)
                    .Value = "R";

                cmd.Parameters.Add("@COD_ID_PESSOA_LICENCIADO", SqlDbType.Int)
                    .Value = Funcoes.strToInt(HttpContext.Current.Session["LICENCIADO"].ToString());

                cmd.Parameters.Add("@DTA_INICIO", SqlDbType.DateTime)
                    .Value = Convert.ToDateTime(txtDataIni.Text);

                cmd.Parameters.Add("@DTA_FIM", SqlDbType.DateTime)
                    .Value = Convert.ToDateTime(txtDataFim.Text);

                // Parâmetro condicional por tipo
                string tipo = HttpContext.Current.Session["TIPO"].ToString().Trim();
                int pessoaId = Funcoes.strToInt(HttpContext.Current.Session["PESSOA"].ToString());

                if (tipo == "M")
                {
                    cmd.Parameters.Add("@COD_ID_MARKETPLACE", SqlDbType.Int)
                        .Value = pessoaId;
                }
                else if (tipo == "R")
                {
                    cmd.Parameters.Add("@COD_ID_REPRESENTANTE", SqlDbType.Int)
                        .Value = pessoaId;
                }

                cmd.Parameters.Add("@FLG_TIPO", SqlDbType.Char)
                    .Value = ddlTop10Vendas.SelectedValue;

                // Execução
                DataSet dsConsulta = new DataSet();
                sdaConsulta.Fill(dsConsulta, "TRANSACOES");

                rptTop10Vendas.DataSource = dsConsulta.Tables["TRANSACOES"].DefaultView;
                rptTop10Vendas.DataBind();
            }
        }
        catch (SqlException ex)
        {
            // Timeout de execução (SQL Server)
            if (ex.Number == -2)
            {
                // Aqui você pode logar e exibir mensagem amigável
                // Ex: Funcoes.LogErro(ex);

                // Exemplo simples:
                // lblMensagem.Text = "A consulta demorou mais do que o esperado. Tente reduzir o período.";

                throw new Exception(
                    "A consulta excedeu o tempo limite de execução. Tente reduzir o período ou tente novamente mais tarde.",
                    ex
                );
            }

            // Outros erros SQL
            throw;
        }
        catch (Exception ex)
        {
            // Erros gerais
            throw;
        }
    }




    private void PainelExtrato()
    {
        try
        {
            using (SqlConnection myConsulta = new SqlConnection(Funcoes.conexao()))
            using (SqlDataAdapter sdaConsulta = new SqlDataAdapter("dbo.stp_extrato_ins", myConsulta))
            {
                SqlCommand cmd = sdaConsulta.SelectCommand;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandTimeout = 120; // Timeout em segundos (2 minutos)

                // Parâmetros
                cmd.Parameters.Add("@flg_operacao", SqlDbType.Char)
                    .Value = "U";

                cmd.Parameters.Add("@COD_ID_PESSOA_LICENCIADO", SqlDbType.Int)
                    .Value = Funcoes.strToInt(HttpContext.Current.Session["LICENCIADO"].ToString());

                cmd.Parameters.Add("@COD_ID_PESSOAS_FJ", SqlDbType.Int)
                    .Value = Funcoes.strToInt(HttpContext.Current.Session["PESSOA"].ToString());

                cmd.Parameters.Add("@FLG_STATUS", SqlDbType.VarChar)
                    .Value = string.Empty;

                // Execução
                DataSet dsConsulta = new DataSet();
                sdaConsulta.Fill(dsConsulta, "EXTRATO");

                rptConsultaExtrato.DataSource = dsConsulta.Tables["EXTRATO"].DefaultView;
                rptConsultaExtrato.DataBind();
            }
        }
        catch (SqlException ex)
        {
            // Timeout SQL Server
            if (ex.Number == -2)
            {
                // Log opcional
                // Funcoes.LogErro(ex, "PainelExtrato");

                throw new Exception(
                    "A consulta de extrato excedeu o tempo limite de execução. Tente reduzir o período ou tente novamente mais tarde.",
                    ex
                );
            }

            // Outros erros SQL
            throw;
        }
        catch (Exception)
        {
            // Erros gerais
            throw;
        }
    }

    private void PainelResumo()
    {
        try
        {
            using (SqlConnection myConsulta = new SqlConnection(Funcoes.conexao()))
            using (SqlDataAdapter sdaConsulta = new SqlDataAdapter("dbo.stp_transacoes_ins", myConsulta))
            {
                SqlCommand cmd = sdaConsulta.SelectCommand;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandTimeout = 120; // Timeout em segundos

                // Parâmetros
                cmd.Parameters.Add("@flg_operacao", SqlDbType.Char)
                    .Value = "R";

                cmd.Parameters.Add("@COD_ID_PESSOA_LICENCIADO", SqlDbType.Int)
                    .Value = Funcoes.strToInt(HttpContext.Current.Session["LICENCIADO"].ToString());

                cmd.Parameters.Add("@NOM_FILTRO", SqlDbType.VarChar)
                    .Value = " ";

                cmd.Parameters.Add("@NOM_STATUS", SqlDbType.VarChar)
                    .Value = " ";

                cmd.Parameters.Add("@NOM_TIPO_PAGAMENTO", SqlDbType.VarChar)
                    .Value = " ";

                cmd.Parameters.Add("@NOM_RAZAOSOCIAL", SqlDbType.VarChar)
                    .Value = " ";

                cmd.Parameters.Add("@FLG_ORIGEM", SqlDbType.VarChar)
                    .Value = " ";

                cmd.Parameters.Add("@COD_ID_PESSOAS_FJ", SqlDbType.Int)
                    .Value = Funcoes.strToInt(HttpContext.Current.Session["PESSOA"].ToString());

                // Execução
                DataSet dsConsulta = new DataSet();
                sdaConsulta.Fill(dsConsulta, "TRANSACOES");

                rptConsultaResumo.DataSource = dsConsulta.Tables["TRANSACOES"].DefaultView;
                rptConsultaResumo.DataBind();
            }
        }
        catch (SqlException ex)
        {
            // Timeout SQL Server
            if (ex.Number == -2)
            {
                // Log opcional
                // Funcoes.LogErro(ex, "PainelResumo");

                throw new Exception(
                    "A consulta de resumo excedeu o tempo limite de execução. Tente reduzir os filtros ou tente novamente mais tarde.",
                    ex
                );
            }

            // Outros erros SQL
            throw;
        }
        catch (Exception)
        {
            // Erros gerais
            throw;
        }
    }


    protected void txtDataIni_TextChanged(object sender, EventArgs e)
    {

    }
    protected void btnAtualizar_Click(object sender, EventArgs e)
    {
        if (HttpContext.Current.Session["TIPO"].ToString() == "L")
        {
            ConsultaDashboardAdministrador();
        }
    }
    protected void btnBarra_Click(object sender, EventArgs e)
    {
        TipoGraficoSelecionado = "B";

    }
    protected void btnLinha_Click(object sender, EventArgs e)
    {
        TipoGraficoSelecionado = "L";

    }

    public string PainelMapa(string sTipo)
    {
        string sData = string.Empty;

        try
        {
            using (SqlConnection mySelMapa = new SqlConnection(Funcoes.conexao()))
            using (SqlCommand cmdSelMapa = new SqlCommand("dbo.stp_pessoas_fj_ins", mySelMapa))
            {
                cmdSelMapa.CommandType = CommandType.StoredProcedure;
                cmdSelMapa.CommandTimeout = 120; // Timeout em segundos

                cmdSelMapa.Parameters.Add("@flg_operacao", SqlDbType.Char)
                    .Value = "Q";

                cmdSelMapa.Parameters.Add("@COD_ID_PESSOA_LICENCIADO", SqlDbType.Int)
                    .Value = Funcoes.strToInt(HttpContext.Current.Session["LICENCIADO"].ToString());

                cmdSelMapa.Parameters.Add("@FLG_TIPO", SqlDbType.Char)
                    .Value = "E";

                mySelMapa.Open();

                using (SqlDataReader readerMapa = cmdSelMapa.ExecuteReader())
                {
                    while (readerMapa.Read())
                    {
                        sData += "'" + readerMapa["NOM_ESTADO"].ToString() + "': {";
                        sData += "latitude: " + readerMapa["NUM_LATITUDE"].ToString().Replace(",", ".") + ",";
                        sData += "longitude: " + readerMapa["NUM_LONGITUDE"].ToString().Replace(",", ".") + ",";
                        sData += "attrs: {";
                        sData += "fill: '#1f5050'";
                        sData += ", opacity: 0.8";
                        sData += "},";
                        sData += "text: {content: '" + readerMapa["NOM_UF"].ToString() + "', attrs: {'font-size': 8}},";
                        sData += "tooltip: {content: '"
                               + readerMapa["NOM_UF"].ToString() + " - "
                               + readerMapa["NOM_ESTADO"].ToString() + "<br />"
                               + readerMapa["NUM_ESTABELECIMENTOS"].ToString()
                               + " Estabelecimentos'}";
                        sData += "},";
                    }
                }
            }
        }
        catch (SqlException ex)
        {
            // Timeout SQL Server
            if (ex.Number == -2)
            {
                // Log opcional
                // Funcoes.LogErro(ex, "PainelMapa");

                throw new Exception(
                    "A consulta do mapa excedeu o tempo limite de execução. Tente novamente mais tarde.",
                    ex
                );
            }

            // Outros erros SQL
            throw;
        }
        catch (Exception)
        {
            // Erros gerais
            throw;
        }

        return sData;
    }

    public string Cores()
    {
        string sData = "[";

        try
        {
            string urlorigem = Request.ServerVariables["SERVER_NAME"].ToString();

            using (SqlConnection mySelCadastro = new SqlConnection(Funcoes.conexao()))
            using (SqlCommand cmdSelCadastro = new SqlCommand("dbo.stp_pessoas_fj_ins", mySelCadastro))
            {
                cmdSelCadastro.CommandType = CommandType.StoredProcedure;
                cmdSelCadastro.CommandTimeout = 120; // Timeout em segundos

                cmdSelCadastro.Parameters.Add("@flg_operacao", SqlDbType.Char)
                    .Value = "T";

                cmdSelCadastro.Parameters.Add("@NOM_URL", SqlDbType.VarChar)
                    .Value = urlorigem;

                mySelCadastro.Open();

                int iContador = 1;

                using (SqlDataReader readerCadastro = cmdSelCadastro.ExecuteReader())
                {
                    while (readerCadastro.Read())
                    {
                        if (iContador <= 1)
                        {
                            sData += "'" + readerCadastro["NOM_COR_PRIMARIA_FUNDO"].ToString() + "',";
                            sData += "'" + readerCadastro["NOM_COR_SECUNDARIA_FUNDO"].ToString() + "'";
                            iContador++;
                        }
                    }
                }
            }
        }
        catch (SqlException ex)
        {
            // Timeout SQL Server
            if (ex.Number == -2)
            {
                // Log opcional
                // Funcoes.LogErro(ex, "Cores");

                throw new Exception(
                    "A consulta de cores excedeu o tempo limite de execução. Tente novamente mais tarde.",
                    ex
                );
            }

            // Outros erros SQL
            throw;
        }
        catch (Exception)
        {
            // Erros gerais
            throw;
        }

        sData += "];";
        return sData;
    }


    public string Cor()
    {
        string sData = string.Empty;

        try
        {
            string urlorigem = Request.ServerVariables["SERVER_NAME"].ToString();

            using (SqlConnection mySelCadastro = new SqlConnection(Funcoes.conexao()))
            using (SqlCommand cmdSelCadastro = new SqlCommand("dbo.stp_pessoas_fj_ins", mySelCadastro))
            {
                cmdSelCadastro.CommandType = CommandType.StoredProcedure;
                cmdSelCadastro.CommandTimeout = 120; // Timeout em segundos

                cmdSelCadastro.Parameters.Add("@flg_operacao", SqlDbType.Char)
                    .Value = "T";

                cmdSelCadastro.Parameters.Add("@NOM_URL", SqlDbType.VarChar)
                    .Value = urlorigem;

                mySelCadastro.Open();

                int iContador = 1;

                using (SqlDataReader readerCadastro = cmdSelCadastro.ExecuteReader())
                {
                    while (readerCadastro.Read())
                    {
                        if (iContador <= 1)
                        {
                            sData += "'" + readerCadastro["NOM_COR_PRIMARIA_FUNDO"].ToString() + "'";
                            iContador++;
                        }
                    }
                }
            }
        }
        catch (SqlException ex)
        {
            // Timeout SQL Server
            if (ex.Number == -2)
            {
                // Log opcional
                // Funcoes.LogErro(ex, "Cor");

                throw new Exception(
                    "A consulta da cor excedeu o tempo limite de execução. Tente novamente mais tarde.",
                    ex
                );
            }

            // Outros erros SQL
            throw;
        }
        catch (Exception)
        {
            // Erros gerais
            throw;
        }

        return sData;
    }


    private void CarregaSaldo()
    {
        // Zoop
        if (HttpContext.Current.Session["TOKENZOOP"].ToString().Trim() != "")
        {
            zoop.HttpResponseResult resultado = zoop.ConsultarSaldo(HttpContext.Current.Session["TOKENZOOP"].ToString());
            if (resultado.StatusCode == 200)
            {
                string jsonResponse = resultado.Content;
                JObject o = JObject.Parse(jsonResponse);
                //lblSaldoDisponivel.Text = String.Format("{0:n2}",Funcoes.strToDouble(o["items"]["current_balance"].ToString())/1000000);
                lblSaldoDisponivelDetalhe.Text = String.Format("{0:n2}", Funcoes.strToDouble(o["items"]["current_balance"].ToString()) / 1000000);
                lblLancamentosFuturosDetalhe.Text = String.Format("{0:n2}", Funcoes.strToDouble(o["items"]["account_balance"].ToString()) / 1000000);
                lblSaldoContaDigital.Text = String.Format("{0:n2}", Funcoes.strToDouble(o["items"]["current_balance"].ToString()) / 1000000);
            }

        }

        // Verifica se o Usuário possui Contas Bancárias no Sistema, caso contrário tenta importar da zoop
    }

    private void CarregaContas()
    {
        try
        {
            if (HttpContext.Current.Session["TOKENZOOP"].ToString().Trim() == "")
                return;

            int iContas = 0;

            // Verifica se já existem contas cadastradas
            using (SqlConnection mySelCadastro = new SqlConnection(Funcoes.conexao()))
            using (SqlCommand cmdSelCadastro = new SqlCommand("dbo.stp_pessoas_fj_contas_ins", mySelCadastro))
            {
                cmdSelCadastro.CommandType = CommandType.StoredProcedure;
                cmdSelCadastro.CommandTimeout = 120;

                cmdSelCadastro.Parameters.Add("@flg_operacao", SqlDbType.Char)
                    .Value = "C";

                cmdSelCadastro.Parameters.Add("@COD_ID_PESSOA_LICENCIADO", SqlDbType.Int)
                    .Value = Funcoes.strToInt(HttpContext.Current.Session["LICENCIADO"].ToString());

                cmdSelCadastro.Parameters.Add("@COD_ID_PESSOAS_FJ", SqlDbType.Int)
                    .Value = Funcoes.strToInt(HttpContext.Current.Session["PESSOA"].ToString());

                mySelCadastro.Open();

                using (SqlDataReader readerCadastro = cmdSelCadastro.ExecuteReader())
                {
                    while (readerCadastro.Read())
                    {
                        iContas++;
                    }
                }
            }

            // Se não houver contas, consulta na Zoop e grava no banco
            if (iContas <= 0)
            {
                var json = zoop.ConsultaContasSeller(
                    HttpContext.Current.Session["TOKENZOOP"].ToString()
                );

                JObject o = JObject.Parse(json);

                for (int i = 0; i < o["items"].Count(); i++)
                {
                    using (SqlConnection connInsCons = new SqlConnection(Funcoes.conexao()))
                    using (SqlCommand cmdInsCons = new SqlCommand("dbo.stp_pessoas_fj_contas_ins", connInsCons))
                    {
                        cmdInsCons.CommandType = CommandType.StoredProcedure;
                        cmdInsCons.CommandTimeout = 120;

                        cmdInsCons.Parameters.Add("@flg_operacao", SqlDbType.Char)
                            .Value = "B";

                        cmdInsCons.Parameters.Add("@COD_ID_PESSOA_LICENCIADO", SqlDbType.Int)
                            .Value = Funcoes.strToInt(HttpContext.Current.Session["LICENCIADO"].ToString());

                        cmdInsCons.Parameters.Add("@COD_ID_PESSOAS_FJ", SqlDbType.Int)
                            .Value = Funcoes.strToInt(HttpContext.Current.Session["PESSOA"].ToString());

                        cmdInsCons.Parameters.Add("@NOM_CODIGO_BANCO", SqlDbType.VarChar)
                            .Value = o["items"][i]["bank_code"].ToString();

                        cmdInsCons.Parameters.Add("@NOM_TIPO_BANCO", SqlDbType.VarChar)
                            .Value = "C";

                        cmdInsCons.Parameters.Add("@NOM_NUMERO_AGENCIA_BANCO", SqlDbType.VarChar)
                            .Value = o["items"][i]["routing_number"].ToString();

                        cmdInsCons.Parameters.Add("@NOM_NUMERO_DIGITO_AGENCIA_BANCO", SqlDbType.VarChar)
                            .Value = o["items"][i]["routing_check_digit"].ToString();

                        cmdInsCons.Parameters.Add("@NOM_NUMERO_CONTA_BANCO", SqlDbType.VarChar)
                            .Value = o["items"][i]["account_number"].ToString();

                        cmdInsCons.Parameters.Add("@NOM_TOKEN", SqlDbType.VarChar)
                            .Value = o["items"][i]["id"].ToString();

                        cmdInsCons.Parameters.Add("@NUM_ID_CONTA", SqlDbType.VarChar)
                            .Value = o["items"][i]["id"].ToString();

                        cmdInsCons.Parameters.Add("@NOM_BANCO", SqlDbType.VarChar)
                            .Value = o["items"][i]["bank_name"].ToString();

                        cmdInsCons.Parameters.Add("@NOM_TITULAR", SqlDbType.VarChar)
                            .Value = o["items"][i]["holder_name"].ToString();

                        cmdInsCons.Parameters.Add("@NOM_CNPJCPF", SqlDbType.VarChar)
                            .Value = o["items"][i]["taxpayer_id"].ToString();

                        cmdInsCons.Parameters.Add("@FLG_PADRAO", SqlDbType.Char)
                            .Value = "S";

                        connInsCons.Open();
                        cmdInsCons.ExecuteNonQuery();
                    }
                }
            }

            // Lista as contas no modal de saque
            using (SqlConnection myConsulta = new SqlConnection(Funcoes.conexao()))
            using (SqlDataAdapter sdaConsulta = new SqlDataAdapter("dbo.stp_pessoas_fj_contas_ins", myConsulta))
            {
                SqlCommand cmd = sdaConsulta.SelectCommand;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandTimeout = 120;

                cmd.Parameters.Add("@flg_operacao", SqlDbType.Char)
                    .Value = "C";

                cmd.Parameters.Add("@COD_ID_PESSOA_LICENCIADO", SqlDbType.Int)
                    .Value = Funcoes.strToInt(HttpContext.Current.Session["LICENCIADO"].ToString());

                cmd.Parameters.Add("@COD_ID_PESSOAS_FJ", SqlDbType.Int)
                    .Value = Funcoes.strToInt(HttpContext.Current.Session["PESSOA"].ToString());

                DataSet dsConsulta = new DataSet();
                sdaConsulta.Fill(dsConsulta, "PESSOAS_FJ_CONTAS");

                ddlContasBancarias.DataTextField = "NOM_DADOS_BANCO";
                ddlContasBancarias.DataValueField = "COD_ID";
                ddlContasBancarias.DataSource = dsConsulta.Tables["PESSOAS_FJ_CONTAS"].DefaultView;
                ddlContasBancarias.DataBind();

                rptContasBancarias.DataSource = dsConsulta.Tables["PESSOAS_FJ_CONTAS"].DefaultView;
                rptContasBancarias.DataBind();
            }
        }
        catch (SqlException ex)
        {
            // Timeout SQL Server
            if (ex.Number == -2)
            {
                // Funcoes.LogErro(ex, "CarregaContas");

                throw new Exception(
                    "A operação de carga de contas bancárias excedeu o tempo limite. Tente novamente mais tarde.",
                    ex
                );
            }

            throw;
        }
        catch (Exception)
        {
            throw;
        }
    }


    protected void rbtContaDigital_CheckedChanged(object sender, EventArgs e)
    {
        ClientScript.RegisterStartupScript(this.GetType(), "Popup", "$('#modal-saque').modal('show');", true);
        if (rbtContaDigital.Checked == true)
        {
            divContaBancaria.Visible = false;
            divContaDigital.Visible = true;
            if (txtCNPJCPFTransferencia.Text.ToString().Trim() != "")
            {
                crdCNPJCPF.Visible = true;
            }
            else
            {
                crdCNPJCPF.Visible = false;
            }
        }
        else
        {
            divContaBancaria.Visible = true;
            divContaDigital.Visible = false;
            crdCNPJCPF.Visible = false;
        }
    }
    protected void rbtContaBancaria_CheckedChanged(object sender, EventArgs e)
    {
        ClientScript.RegisterStartupScript(this.GetType(), "Popup", "$('#modal-saque').modal('show')", true);
        if (rbtContaBancaria.Checked == true)
        {
            divContaBancaria.Visible = true;
            divContaDigital.Visible = false;
            crdCNPJCPF.Visible = false;
        }
        else
        {
            divContaBancaria.Visible = false;
            divContaDigital.Visible = true;
            if (txtCNPJCPFTransferencia.Text.ToString().Trim() != "")
            {
                crdCNPJCPF.Visible = true;
            }
            else
            {
                crdCNPJCPF.Visible = false;
            }

        }
    }
    protected void txtCNPJCPFTransferencia_TextChanged(object sender, EventArgs e)
    {
        ClientScript.RegisterStartupScript(this.GetType(), "Popup", "$('#modal-saque').modal('show')", true);
        divContaDigital.Visible = false;
        // Realizar busca dos dados do favorecido na ZOOP

        string sResultado = zoop.vendedores("P", txtCNPJCPFTransferencia.Text.ToString(), "");
        if (sResultado.ToString().Trim() == "")
        {
            ClientScript.RegisterStartupScript(this.GetType(), "Favorecido", "alert('Favorecido não localizado! Verifique e reentre');", true);
        }
        else
        {
            lblCNPJCPFDocumento.Text = txtCNPJCPFTransferencia.Text.ToString();
            string[] words = sResultado.Split('/');
            for (int i = 0; i < words.Length; i++)
            {
                if (i == 0) { txtTokenFavorecido.Text = words[i]; }
                if (i == 1) { lblCNPJCPFNomeFavorecido.Text = words[i]; }
                if (i == 2) { lblCNPJCPFEmail.Text = words[i]; }
            }

        }
        crdCNPJCPF.Visible = true;
    }
    protected void btnConfirmarSaque_Click(object sender, EventArgs e)
    {
        if (Funcoes.Enviar2faEstabelecimento() == true)
        {
            ClientScript.RegisterStartupScript(this.GetType(),"Codigo2fa", "alert('Um código de confirmação foi enviado para o seu e-mail!');", true);
        }
        else
        {
            ClientScript.RegisterStartupScript(this.GetType(),"Codigo2faErro", "alert('Ocorreu um erro ao tentar enviar o Código de Confirmação!');", true);
        }
        ClientScript.RegisterStartupScript(this.GetType(),"ExecutaModal", "$('#mdConfirmar').modal('show');", true);





    }

    private void RealizarSaque()
    {
        if (Funcoes.strToDouble(txtValorSaque.Text.ToString()) <= 0)
        {
            ClientScript.RegisterStartupScript(this.GetType(), "ValorSaque", "alert('É necessário especificar um valor! Tente novamente.');", true);
        }
        else
        {
            // Realiza a operação de saque para conta bancária
            if (rbtContaBancaria.Checked == true)
            {
                SqlConnection mySelCadastro = new SqlConnection(Funcoes.conexao());
                mySelCadastro.Open();
                SqlCommand cmdSelCadastro = new SqlCommand("dbo.stp_pessoas_fj_contas_ins", mySelCadastro);
                cmdSelCadastro.CommandType = CommandType.StoredProcedure;
                cmdSelCadastro.Parameters.Add("@flg_operacao", SqlDbType.Char).Value = "S";
                cmdSelCadastro.Parameters.Add("@COD_ID", SqlDbType.Int).Value = Funcoes.strToInt(ddlContasBancarias.SelectedValue.ToString());
                cmdSelCadastro.Parameters.Add("@COD_ID_PESSOA_LICENCIADO", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["LICENCIADO"].ToString());
                SqlDataReader ReaderCadastro = cmdSelCadastro.ExecuteReader();

                while (ReaderCadastro.Read())
                {
                    if (ReaderCadastro["NOM_TOKEN"].ToString().Trim() != "")
                    {
                        if (ReaderCadastro["NUM_ID_CONTA"].ToString().Trim() != "")
                        {

                            dadosTransferencia.Transferencia dtransferencia = new dadosTransferencia.Transferencia()
                            {
                                amount = Funcoes.strToInt(Convert.ToString(Funcoes.strToDouble(txtValorSaque.Text.ToString()) * 100).ToString()),
                                statement_descriptor = ReaderCadastro["NOM_PESSOAS_FJ"].ToString(),
                                description = txtDescricaoSaque.Text.ToString(),
                            };
                            string json = JsonConvert.SerializeObject(dtransferencia);

                            
                            zoop.HttpResponseResult resultado = zoop.transferencia("I", ReaderCadastro["NUM_ID_CONTA"].ToString(), json);
                            string retornoInclusao = "";
                            if (resultado.StatusCode == 200)
                            {
                                retornoInclusao = resultado.Content;
                            }

                            txtRetorno.Text = retornoInclusao;
                            dvRetorno.Visible = true;

                            ClientScript.RegisterStartupScript(this.GetType(), "SucessoIDConta", "alert('ID Conta Bancária: " + ReaderCadastro["NUM_ID_CONTA"].ToString() + " - Operação: " + retornoInclusao + "');", true);
                            
                            if (retornoInclusao.ToString().Trim() != "")
                            {
                                txtRetorno.Text = retornoInclusao;
                                dvRetorno.Visible = true;
                                try
                                {
                                    //JObject oTransferencia = JObject.Parse(retornoInclusao.ToString());
                                    ClientScript.RegisterStartupScript(this.GetType(), "Sucesso", "alert('Solicitação de saque realizada para conta bancária com sucesso!');", true);
                                    // Deveria registrar arquivo transferência



                                    CarregaSaldo();
                                }
                                catch
                                {
                                    ClientScript.RegisterStartupScript(this.GetType(), "ErroTransferencia", "alert('Ocorreu um erro ao tentar realizar a transferência! Verifique os dados e tente novamente.');", true);

                                }
                            }
                        }
                        else
                        {
                            ClientScript.RegisterStartupScript(this.GetType(), "IDContaNaoExiste", "alert('Conta Bancária não possui código de ID da adquirente! Verifique e tente novamente.');", true);
                        }
                    }
                    else
                    {
                        ClientScript.RegisterStartupScript(this.GetType(), "IDTokenNaoExiste", "alert('Conta Bancária não possui cadastro na adquirente! Verifique e tente novamente.');", true);

                    }
                }

            }


            // Realiza a operação de saque para conta digital
            if (rbtContaDigital.Checked == true)
            {

                if (txtTokenFavorecido.Text.ToString().Trim() != "")
                {

                    dadosTransferenciaP2P.TransferenciaP2P dtransferenciaP2P = new dadosTransferenciaP2P.TransferenciaP2P()
                    {
                        amount = Funcoes.strToInt(Convert.ToString(Funcoes.strToDouble(txtValorSaque.Text.ToString()) * 100).ToString()),
                        description = txtDescricaoSaque.Text.ToString(),
                        reference_id = Funcoes.GetRandomPassword(16).ToString(),
                        is_idempotency = true,
                    };
                    string json = JsonConvert.SerializeObject(dtransferenciaP2P);
                    string retornoInclusao = zoop.transferenciaP2P("I", HttpContext.Current.Session["TOKENZOOP"].ToString(), txtTokenFavorecido.Text.ToString(), json);

                    ClientScript.RegisterStartupScript(this.GetType(), "RetornoTransferencia", "alert('" + retornoInclusao.ToString() + "');", true);
                    CarregaSaldo();
                }
                else
                {
                    ClientScript.RegisterStartupScript(this.GetType(), "IDConta", "alert('Favorecido não possui cadastro na adquirente ou não existe! Verifique e tente novamente.');", true);

                }

            }

        }
    }

    protected void btnSubmit2_Click(object sender, EventArgs e)
    {
        ClientScript.RegisterStartupScript(this.GetType(), "ValorSaque", "alert('É necessário especificar um valor!');", true);

    }
    private void ContaDigital()
    {
        try
        {
            string sToken = asaas.PegarTokenSubconta(Funcoes.strToInt(HttpContext.Current.Session["PESSOA"].ToString()), Funcoes.strToInt(HttpContext.Current.Session["LICENCIADO"].ToString()));
            //lblToken.Text = sToken.ToString().Length.ToString();

            if (sToken.ToString().Trim() != "")
            {
                string sUrl = "";
                // Verifica o endereço dos documentos
                string sJsonUrl = asaas.ListarDocumentosPendentes(sToken);
                if (sJsonUrl.ToString().Trim() != "")
                {
                    JObject oDocumentos = JObject.Parse(sJsonUrl.ToString());
                    if (oDocumentos["data"].Count() > 0)
                    {
                        for (int i = 0; i < oDocumentos["data"].Count(); i++)
                        {
                            if (oDocumentos["data"][0]["onboardingUrl"].ToString().Trim() != "")
                            {
                                sUrl = oDocumentos["data"][0]["onboardingUrl"].ToString();
                            }
                        }
                    }
                }
                if (sUrl.ToString().Trim() != "")
                {
                    dvDocumentosContaDigital.Visible = true;
                    hrfDocumentos.HRef = sUrl.ToString();
                }
                else
                {
                    dvDocumentosContaDigital.Visible = false;
                }
            }
            else
            {
                dvDocumentosContaDigital.Visible = false;
            }
        }
        catch
        {
        }
    }


    private void ConsultaIntegracoes()
    {
        try
        {
            using (SqlConnection mySelCadastro = new SqlConnection(Funcoes.conexao()))
            using (SqlCommand cmdSelCadastro = new SqlCommand("dbo.stp_pessoas_fj_integracoes_ins", mySelCadastro))
            {
                cmdSelCadastro.CommandType = CommandType.StoredProcedure;
                cmdSelCadastro.CommandTimeout = 120; // Timeout em segundos

                cmdSelCadastro.Parameters.Add("@flg_operacao", SqlDbType.Char)
                    .Value = "V";

                cmdSelCadastro.Parameters.Add("@COD_ID_PESSOA_LICENCIADO", SqlDbType.Int)
                    .Value = Funcoes.strToInt(HttpContext.Current.Session["LICENCIADO"].ToString());

                mySelCadastro.Open();

                using (SqlDataReader readerCadastro = cmdSelCadastro.ExecuteReader())
                {
                    while (readerCadastro.Read())
                    {
                        string integracao = readerCadastro["NOM_INTEGRACAO"].ToString().ToUpper().Trim();
                        string flgAcesso = readerCadastro["FLG_ACESSO"].ToString().ToUpper();

                        if (integracao == "CAPPTA" && flgAcesso == "N")
                        {
                            // Mantido conforme lógica original
                        }

                        if (integracao == "ZOOP" && flgAcesso == "N")
                        {
                            // Mantido conforme lógica original
                        }

                        if (integracao == "ASAAS" && flgAcesso == "N")
                        {
                            hrfDocumentos.Visible = false;
                        }

                        if (integracao == "PAGSEGURO" && flgAcesso == "N")
                        {
                            // Mantido conforme lógica original
                        }
                    }
                }
            }
        }
        catch (SqlException ex)
        {
            // Timeout SQL Server
            if (ex.Number == -2)
            {
                // Log opcional
                // Funcoes.LogErro(ex, "ConsultaIntegracoes");

                throw new Exception(
                    "A consulta de integrações excedeu o tempo limite de execução. Tente novamente mais tarde.",
                    ex
                );
            }

            throw;
        }
        catch (Exception)
        {
            throw;
        }
    }


    protected void ddlTop10Vendas_SelectedIndexChanged(object sender, EventArgs e)
    {
        PainelTop10Vendas();
    }
    protected void ddlAdquirentes_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlAdquirentes.SelectedValue.ToString() == "P" && HttpContext.Current.Session["TIPO"].ToString() == "A")
        {
            btnExtratoEDI.Visible = true;
        }
        else
        {
            btnExtratoEDI.Visible = false;

        }

    }
    protected void btnExtratoEDI_Click(object sender, EventArgs e)
    {
        ClientScript.RegisterStartupScript(this.GetType(),
   "ExtratoEDI", "openPopupWindow('cad_extrato_edi.aspx','ExtratoEDI',1024,800);", true);
       
    }
    protected void lkbReenviar_Click(object sender, System.EventArgs e)
    {
        if (Funcoes.Enviar2faEstabelecimento() == true)
        {
            ClientScript.RegisterStartupScript(this.GetType(),"Codigo2fa", "alert('Um código de confirmação foi enviado para o seu e-mail!');", true);

        }
        else
        {
            ClientScript.RegisterStartupScript(this.GetType(),"Codigo2faErro", "alert('Ocorreu um erro ao tentar enviar o Código de Confirmação!');", true);

        }
        ClientScript.RegisterStartupScript(this.GetType(),"ExecutaModal", "$('#mdConfirmar').modal('show');", true);
    }

    protected void btnConfirmar2FA_Click(object sender, System.EventArgs e)
    {
        // Verifica se o código autenticação esta correto
        SqlConnection connVerifica2fa = new SqlConnection(Funcoes.conexao());
        SqlCommand cmdVerifica2fa = new SqlCommand("dbo.stp_2fa_ins", connVerifica2fa);
        cmdVerifica2fa.CommandType = CommandType.StoredProcedure;
        cmdVerifica2fa.Parameters.Add("@flg_operacao", SqlDbType.Char).Value = "L";
        cmdVerifica2fa.Parameters.Add("@COD_ID_SIS_USUARIO", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["CODIGO"].ToString());
        cmdVerifica2fa.Parameters.Add("@COD_ID_PESSOA_LICENCIADO", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["LICENCIADO"].ToString());
        cmdVerifica2fa.Parameters.Add("@NOM_2FA", SqlDbType.VarChar).Value = Funcoes.Encrypt(txt2FA.Text.ToString());
        cmdVerifica2fa.Parameters.Add("@DTA_EXPIRACAO", SqlDbType.DateTime).Value = DateTime.Now;
        connVerifica2fa.Open();
        SqlDataReader mReader2fa = cmdVerifica2fa.ExecuteReader();

        if (mReader2fa.Read())
        {
            RealizarSaque();           
        }
        else
        {
            ClientScript.RegisterStartupScript(this.GetType(),
                "CodigoErrado2fa", "alert('O Código de confirmação de autenticação não está correto! Verifique e reentre.');", true);
        }
    }
    protected void rptDocumentos_ItemCommand(object source, RepeaterCommandEventArgs e)
    {
        if (e.CommandName == "Assinar")
        {
            
            SqlConnection connInsCons = new SqlConnection(Funcoes.conexao());
            connInsCons.Open();
            SqlCommand cmdInsCons = new SqlCommand("dbo.stp_pessoas_fj_assinaturas_ins", connInsCons);
            cmdInsCons.CommandType = CommandType.StoredProcedure;
            cmdInsCons.Parameters.Add("@flg_operacao", SqlDbType.Char).Value = "I";
            cmdInsCons.Parameters.Add("@COD_ID_DOCUMENTO", SqlDbType.Int).Value = Funcoes.strToInt(Convert.ToString(e.CommandArgument));
            cmdInsCons.Parameters.Add("@COD_ID_PESSOA_LICENCIADO", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["LICENCIADO"].ToString());
            cmdInsCons.Parameters.Add("@COD_ID_PESSOAS_FJ", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["PESSOA"].ToString());
            string Codificacao = DateTime.Now.Year.ToString() + DateTime.Now.Month.ToString() + DateTime.Now.Day.ToString() + DateTime.Now.Hour.ToString() + DateTime.Now.Minute.ToString() + DateTime.Now.Second.ToString();
            string caminhoPDF = Server.MapPath("public_html") + "\\" + HttpContext.Current.Session["CODIGO"].ToString().Trim() + "_" + Codificacao.ToString() + "_" + "AUTORIZACAO_DEBITO.pdf";
            
            cmdInsCons.Parameters.Add("@NOM_ARQUIVO", SqlDbType.VarChar).Value = documentos.CriarAutorizacaoDebitoPDF(Funcoes.strToInt(HttpContext.Current.Session["PESSOA"].ToString()), Funcoes.strToInt(HttpContext.Current.Session["LICENCIADO"].ToString()), caminhoPDF);

            SqlConnection mySelCadastro = new SqlConnection(Funcoes.conexao());
            mySelCadastro.Open();
            SqlCommand cmdSelCadastro = new SqlCommand("dbo.stp_pessoas_fj_ins", mySelCadastro);
            cmdSelCadastro.CommandType = CommandType.StoredProcedure;
            cmdSelCadastro.Parameters.Add("@flg_operacao", SqlDbType.Char).Value = "S";
            cmdSelCadastro.Parameters.Add("@COD_ID_PESSOA_LICENCIADO", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["LICENCIADO"].ToString());
            cmdSelCadastro.Parameters.Add("@COD_ID", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["PESSOA"].ToString());
            SqlDataReader ReaderCadastro = cmdSelCadastro.ExecuteReader();
            while (ReaderCadastro.Read())
            {
                string jsonRetorno = documentos.EnviarDocumentoAutentique("TERMO DE AUTORIZAÇÃO PARA DÉBITO AUTOMÁTICO EM CONTA DE PAGAMENTO", ReaderCadastro["NOM_RAZAOSOCIAL"].ToString(), ReaderCadastro["NOM_EMAIL_EMPRESA"].ToString(), ReaderCadastro["NOM_RAZAOSOCIAL_LICENCIADO"].ToString(), ReaderCadastro["NOM_EMAIL_LICENCIADO"].ToString(), caminhoPDF);
                cmdInsCons.Parameters.Add("@DES_JSON", SqlDbType.Text).Value = jsonRetorno.ToString();
                if (jsonRetorno.ToString().Trim() != "")
                {
                    try
                    {
                        JObject oDocumento = JObject.Parse(jsonRetorno);
                        cmdInsCons.Parameters.Add("@COD_ID_DOCUMENTO_ENVIADO", SqlDbType.VarChar).Value = oDocumento["data"]["createDocument"]["id"].ToString();
                        cmdInsCons.Parameters.Add("@FLG_ACEITE", SqlDbType.Char).Value = "S";

                        // Mensagem de documento enviado com sucesso! 
                        ClientScript.RegisterStartupScript(this.GetType(), "DocumentoAssinaturaSucesso", "alert('O documento foi enviado com sucesso para o seu e-mail para que seja devidamente assinado de forma digital! Por favor, pedimos que verifique o seu e-mail e providencie a assinatura o mais breve possível.');", true);
                        rptDocumentos.Visible = false;
                    }
                    catch
                    {
                        cmdInsCons.Parameters.Add("@FLG_ACEITE", SqlDbType.Char).Value = "X";
                        ClientScript.RegisterStartupScript(this.GetType(), "DocumentoAssinaturaErro", "alert('Ocorreu um erro ao tentar enviar o documento para assinatura! Verifique e tente novamente.');", true);
                    }
                }
            }

            cmdInsCons.ExecuteNonQuery();
            connInsCons.Close();
            connInsCons.Dispose();

            //Response.Redirect("index.aspx");
        }

        if (e.CommandName == "NaoAssinar")
        {
            
            SqlConnection connInsCons = new SqlConnection(Funcoes.conexao());
            connInsCons.Open();
            SqlCommand cmdInsCons = new SqlCommand("dbo.stp_pessoas_fj_assinaturas_ins", connInsCons);
            cmdInsCons.CommandType = CommandType.StoredProcedure;
            cmdInsCons.Parameters.Add("@flg_operacao", SqlDbType.Char).Value = "I";
            cmdInsCons.Parameters.Add("@COD_ID_DOCUMENTO", SqlDbType.Int).Value = Funcoes.strToInt(Convert.ToString(e.CommandArgument));
            cmdInsCons.Parameters.Add("@COD_ID_PESSOA_LICENCIADO", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["LICENCIADO"].ToString());
            cmdInsCons.Parameters.Add("@COD_ID_PESSOAS_FJ", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["PESSOA"].ToString());
            cmdInsCons.Parameters.Add("@FLG_ACEITE", SqlDbType.Char).Value = "N";
            cmdInsCons.Parameters.Add("@NOM_ARQUIVO", SqlDbType.Char).Value = "";
            cmdInsCons.ExecuteNonQuery();
            connInsCons.Close();
            connInsCons.Dispose();
            
        }

    }

    private void ConsultaSaldoBaas(string integracao)
    {
        try
        {
            if (integracao != "A")
                return;

            // Valor padrão seguro
            lblSaldoDisponivel.Text = "0,00";
            lblSaldoDisponivelHiden.Text = "0,00";

            // Validação de Session
            if (HttpContext.Current.Session["PESSOA"] == null ||
                HttpContext.Current.Session["LICENCIADO"] == null)
                return;

            int pessoa = Funcoes.strToInt(HttpContext.Current.Session["PESSOA"].ToString());
            int licenciado = Funcoes.strToInt(HttpContext.Current.Session["LICENCIADO"].ToString());

            string sToken = asaas.PegarTokenSubconta(pessoa, licenciado);

            if (string.IsNullOrWhiteSpace(sToken))
                return;

            string jsonSaldo = asaas.SaldoSubconta(sToken);

            if (string.IsNullOrWhiteSpace(jsonSaldo))
                return;

            JObject oSaldo = JObject.Parse(jsonSaldo);

            // 🚨 Se a API retornou erro
            if (oSaldo["errors"] != null)
            {

                // Você pode logar o erro se quiser
                // string erroApi = oSaldo["errors"][0]?["description"]?.ToString();
                string erroApi = oSaldo.SelectToken("errors[0].description") != null ? oSaldo.SelectToken("errors[0].description").ToString() : string.Empty;

                ClientScript.RegisterStartupScript(this.GetType(), "AtualizarDadosCadastrais", "alert('Conta Digital informa que: " + erroApi + "');", true);


                lblSaldoDisponivel.Text = "0,00";
                lblSaldoDisponivelHiden.Text = "0,00";
                return;
            }

            // 🔐 Proteção total contra null
            JToken balanceToken = oSaldo["balance"];

            if (balanceToken == null || balanceToken.Type == JTokenType.Null)
            {
                lblSaldoDisponivel.Text = "0,00";
                lblSaldoDisponivelHiden.Text = "0,00";
                return;
            }

            double saldo = Funcoes.strToDouble(balanceToken.ToString());
            lblSaldoDisponivel.Text = String.Format("{0:n2}", saldo);
            lblSaldoDisponivelHiden.Text = String.Format("{0:n2}", saldo);
        }
        catch (Exception ex)
        {
            // 🔥 Nunca deixa quebrar a página
            lblSaldoDisponivel.Text = "0,00";
            lblSaldoDisponivelHiden.Text = "0,00";


            // Opcional: log
            // Logger.Gravar(ex);
        }
    }

}