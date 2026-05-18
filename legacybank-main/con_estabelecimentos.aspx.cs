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

public partial class con_estabelecimentos : System.Web.UI.Page
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

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session.Count <= 0)
        {
            FormsAuthentication.SignOut();
            Response.Redirect("login.aspx");
        }

        // SEMPRE carregar
        Licenciado = Session["LICENCIADO"] != null ? Funcoes.strToInt(Session["LICENCIADO"].ToString()) : 0;
        Tipo = Session["TIPO"] != null ? Session["TIPO"].ToString() : "";
        Pessoa = Session["PESSOA"] != null ? Funcoes.strToInt(Session["PESSOA"].ToString()) : 0;
        Usuario = Session["PESSOA"] != null ? Funcoes.strToInt(Session["CODIGO"].ToString()) : 0;        
        
        if (!IsPostBack)
        {

            Funcoes.GravaAuditoriaNovo(0, Request.ServerVariables["REMOTE_ADDR"].ToString(), "Cadastro - Estabelecimentos", "Consulta",Licenciado, Pessoa, Usuario);

            DateTime data = DateTime.Today;
            DateTime primeiroDiaDoMes = new DateTime(data.Year, data.Month, 1);
            DateTime ultimoDiaDoMes = new DateTime(data.Year, data.Month, DateTime.DaysInMonth(data.Year, data.Month));
            txtDataIni.Text = DateTime.Now.ToShortDateString();
            txtDataFim.Text = DateTime.Now.ToShortDateString();

            SqlConnection myRepresentante = new SqlConnection(Funcoes.conexao());
            myRepresentante.Open();
            SqlCommand cmdRepresentante = new SqlCommand("dbo.stp_pessoas_fj_ins", myRepresentante);
            cmdRepresentante.CommandType = CommandType.StoredProcedure;
            cmdRepresentante.Parameters.Add("@flg_operacao", SqlDbType.VarChar).Value = "L";
            cmdRepresentante.Parameters.Add("@COD_ID_PESSOA_LICENCIADO", SqlDbType.Int).Value = Licenciado;
            cmdRepresentante.Parameters.Add("@FLG_ATIVO", SqlDbType.Char).Value = "S";
            cmdRepresentante.Parameters.Add("@FLG_TIPO", SqlDbType.Char).Value = "R";

            if (HttpContext.Current.Session["TIPO"].ToString().Trim() == "M")
            {
                cmdRepresentante.Parameters.Add("@COD_ID_MARKETPLACE", SqlDbType.Int).Value = Pessoa;
            }

            SqlDataAdapter drRepresentante = new SqlDataAdapter();
            drRepresentante.SelectCommand = cmdRepresentante;
            DataSet dsRepresentante = new DataSet();
            drRepresentante.Fill(dsRepresentante, "PESSOAS_FJ");
            ddlRepresentante.DataTextField = "NOM_RAZAOSOCIAL";
            ddlRepresentante.DataValueField = "COD_ID";
            ddlRepresentante.DataSource = dsRepresentante.Tables["PESSOAS_FJ"].DefaultView;
            ddlRepresentante.DataBind();
            ddlRepresentante.Items.Insert(0, new System.Web.UI.WebControls.ListItem("Todos", "0"));

            if (HttpContext.Current.Session["TIPO"].ToString().Trim() == "R")
            {
                ddlRepresentante.SelectedValue = Pessoa.ToString();
                ddlRepresentante.Enabled = false;
            }

            // Lista dos Marketplaces

            SqlConnection myMkt = new SqlConnection(Funcoes.conexao());
            myMkt.Open();
            SqlCommand cmdMkt = new SqlCommand("dbo.stp_pessoas_fj_ins", myMkt);
            cmdMkt.CommandType = CommandType.StoredProcedure;
            cmdMkt.Parameters.Add("@flg_operacao", SqlDbType.VarChar).Value = "L";
            cmdMkt.Parameters.Add("@COD_ID_PESSOA_LICENCIADO", SqlDbType.Int).Value = Licenciado;
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
            ddlMarketplace.Items.Insert(0, new System.Web.UI.WebControls.ListItem("Todos", "0"));

            if (Tipo == "R")
            {
                ddlMarketplace.Enabled = false;
            }


            if (Tipo == "M")
            {
                ddlMarketplace.SelectedValue = Pessoa.ToString();
                ddlMarketplace.Enabled = false;
            }


            //dvParcial.Visible = false;
            if (Tipo == "A")
            {
                dvCompleta.Visible = true;
                btnImportar.Visible = true;
                dvLimite.Visible = false;
            }
            if (Tipo == "M")
            {
                dvCompleta.Visible = true;
                btnImportar.Visible = false;
            }
            if (Tipo == "R")
            {
                dvCompleta.Visible = true;
                btnImportar.Visible = false;
            }

        }

    }

    protected void btnAnterior_Click(object sender, EventArgs e)
    {
        if (PaginaAtual > 1)
        {
            PaginaAtual--;
            ConsultaGeral();
        }
    }

    protected void btnProximo_Click(object sender, EventArgs e)
    {
        int totalPaginas = (int)Math.Ceiling(TotalRegistros / (double)TamanhoPagina);

        if (PaginaAtual < totalPaginas)
        {
            PaginaAtual++;
            ConsultaGeral();
        }
    }

    protected void btnPesquisar_Click(object sender, EventArgs e)
    {
        ConsultaGeral();
        ConsultaDashboard();
    }

    private void ConsultaDashboard()
    {
        using (SqlConnection conn = new SqlConnection(Funcoes.conexao()))
        {
            using (SqlDataAdapter da = new SqlDataAdapter())
            {
                da.SelectCommand = new SqlCommand("dbo.stp_pessoas_fj_ins", conn);
                da.SelectCommand.CommandType = CommandType.StoredProcedure;
                da.SelectCommand.CommandTimeout = 120;

                // Parâmetros comuns
                da.SelectCommand.Parameters.Add("@COD_ID_PESSOA_LICENCIADO", SqlDbType.Int).Value = Licenciado;
                da.SelectCommand.Parameters.Add("@NOM_RAZAOSOCIAL", SqlDbType.VarChar).Value = " ";
                da.SelectCommand.Parameters.Add("@FLG_ATIVO", SqlDbType.Char).Value = " ";
                da.SelectCommand.Parameters.Add("@FLG_TIPO", SqlDbType.Char).Value = "E";

                if (Tipo == "M")
                    da.SelectCommand.Parameters.Add("@COD_ID_MARKETPLACE", SqlDbType.Int).Value = Pessoa;

                if (Tipo == "R")
                    da.SelectCommand.Parameters.Add("@COD_ID_REPRESENTANTE", SqlDbType.Int).Value = Pessoa;

                // ==========================
                // STATUS (flg_operacao = G)
                // ==========================
                da.SelectCommand.Parameters.Add("@flg_operacao", SqlDbType.Char).Value = "G";

                DataTable dtStatus = new DataTable();
                da.Fill(dtStatus);

                rptStatus.DataSource = dtStatus.DefaultView;
                rptStatus.DataBind();

                // ==========================
                // TIPO (flg_operacao = J)
                // ==========================
                da.SelectCommand.Parameters["@flg_operacao"].Value = "J";

                DataTable dtTipo = new DataTable();
                da.Fill(dtTipo);

                rptTipo.DataSource = dtTipo.DefaultView;
                rptTipo.DataBind();
            }
        }
    }


    private void ConsultaGeral()
    {
        DateTime dtInicio;
        DateTime dtFim;

        if (DateTime.TryParse(txtDataIni.Text, out dtInicio) && DateTime.TryParse(txtDataFim.Text, out dtFim))
        {
            double dias = (dtFim - dtInicio).TotalDays;

            if ((dias > 9000) && (Tipo.Trim() != "A"))
            {

                ClientScript.RegisterStartupScript(this.GetType(), "IntervaloInvalido", "alert('Atenção: o período selecionado ultrapassa 90 dias. Ajuste as datas para continuar.');", true);

            }
            else
            {

                Funcoes.GravaAuditoriaNovo(0, Request.ServerVariables["REMOTE_ADDR"].ToString(), "Cadastro - Estabelecimentos", "Consulta: " + txtDataIni.Text.ToString() + " - " + txtDataFim.Text.ToString(), Licenciado, Pessoa, Usuario);

                using (SqlConnection myConsulta = new SqlConnection(Funcoes.conexao()))
                {
                    //myConsulta.Open();
                    using (SqlDataAdapter SDAConsulta = new SqlDataAdapter("dbo.stp_pessoas_fj_paginado", myConsulta))
                    {
                        SDAConsulta.SelectCommand.CommandType = CommandType.StoredProcedure;
                        SDAConsulta.SelectCommand.CommandTimeout = 120; // timeout ajustado


                        SDAConsulta.SelectCommand.Parameters.Add("@PageNumber", SqlDbType.Int).Value = PaginaAtual;
                        SDAConsulta.SelectCommand.Parameters.Add("@PageSize", SqlDbType.Int).Value = TamanhoPagina;
                        SDAConsulta.SelectCommand.Parameters.Add("@Exportar", SqlDbType.Bit).Value = 0;

                        SDAConsulta.SelectCommand.Parameters.Add("@COD_ID_PESSOA_LICENCIADO", SqlDbType.Int).Value = Licenciado;
                        SDAConsulta.SelectCommand.Parameters.Add("@NOM_RAZAOSOCIAL", SqlDbType.VarChar).Value = txtNome.Text.ToString();
                        SDAConsulta.SelectCommand.Parameters.Add("@NUM_DOCUMENTO", SqlDbType.VarChar).Value = txtCNPJCPF.Text.ToString();
                        SDAConsulta.SelectCommand.Parameters.Add("@NOM_EMAIL", SqlDbType.VarChar).Value = txtEmail.Text.ToString();
                        SDAConsulta.SelectCommand.Parameters.Add("@FLG_ATIVO", SqlDbType.Char).Value = ddlAtivo.SelectedValue.ToString();
                        SDAConsulta.SelectCommand.Parameters.Add("@FLG_TIPO", SqlDbType.Char).Value = "E";
                        SDAConsulta.SelectCommand.Parameters.Add("@DTA_INICIO", SqlDbType.DateTime).Value = Convert.ToDateTime(txtDataIni.Text.ToString());
                        SDAConsulta.SelectCommand.Parameters.Add("@DTA_FIM", SqlDbType.DateTime).Value = Convert.ToDateTime(txtDataFim.Text.ToString());

                        if (Tipo == "M")
                        {
                            SDAConsulta.SelectCommand.Parameters.Add("@COD_ID_MARKETPLACE", SqlDbType.Int).Value = Pessoa;
                        }
                        else
                        {
                            if (Funcoes.strToInt(ddlMarketplace.SelectedValue.ToString()) > 0)
                            {
                                SDAConsulta.SelectCommand.Parameters.Add("@COD_ID_MARKETPLACE", SqlDbType.Int).Value = Funcoes.strToInt(ddlMarketplace.SelectedValue.ToString());
                            }

                        }
                        if (Tipo == "R")
                        {
                            SDAConsulta.SelectCommand.Parameters.Add("@COD_ID_REPRESENTANTE", SqlDbType.Int).Value = Pessoa;
                        }
                        else
                        {
                            if (Funcoes.strToInt(ddlMarketplace.SelectedValue.ToString()) > 0)
                            {
                                SDAConsulta.SelectCommand.Parameters.Add("@COD_ID_REPRESENTANTE", SqlDbType.Int).Value = Funcoes.strToInt(ddlRepresentante.SelectedValue.ToString());
                            }
                        }


                        DataSet dsConsulta = new DataSet();
                        SDAConsulta.Fill(dsConsulta);


                        TotalRegistros = Convert.ToInt32(dsConsulta.Tables[0].Rows[0]["TotalRegistros"]);
                        rptConsultaCompleta.DataSource = dsConsulta.Tables[1].DefaultView;
                        rptConsultaCompleta.DataBind();
                    }
                    //myConsulta.Close(); myConsulta.Dispose();

                    //foreach (RepeaterItem itemE in rptConsultaCompleta.Items)
                    //{
                    //    if (Funcoes.strToInt(((TextBox)itemE.FindControl("txtExcluir")).Text.ToString()) > 0)
                    //    {
                    //        ((LinkButton)itemE.FindControl("lbkExcluir")).Visible = false;
                    //    }
                    //    else
                    //    {
                    //        ((LinkButton)itemE.FindControl("lbkExcluir")).Visible = true;
                    //    }
                    //}            
                }
                AtualizarPaginacao();
            }
        }
        else
        {
            ClientScript.RegisterStartupScript(this.GetType(), "DataInvalida", "alert('Data(s) inválida(s)! Verifique e reentre.');", true);
        }
    }


    protected void rptConsultaCompleta_OnItemCommand(object source, RepeaterCommandEventArgs e)
    {
        if (e.CommandName == "Excluir")
        {
            using (SqlConnection connInsCons = new SqlConnection(Funcoes.conexao()))
            {
                connInsCons.Open();
                SqlCommand cmdInsCons = new SqlCommand("dbo.stp_pessoas_fj_ins", connInsCons);
                cmdInsCons.CommandType = CommandType.StoredProcedure;
                cmdInsCons.Parameters.Add("@flg_operacao", SqlDbType.Char).Value = "E";
                cmdInsCons.Parameters.Add("@COD_ID", SqlDbType.Int).Value = Funcoes.strToInt(Convert.ToString(e.CommandArgument));
                cmdInsCons.Parameters.Add("@COD_ID_PESSOA_LICENCIADO", SqlDbType.Int).Value = Licenciado;
                cmdInsCons.ExecuteNonQuery();
                connInsCons.Close();
                connInsCons.Dispose();

                ClientScript.RegisterStartupScript(this.GetType(),
                    "Alerta", "alert('Registro Excluído com sucesso!');", true);
                ConsultaGeral();
            }

        }

        if (e.CommandName == "Verificar")
        {

            using (SqlConnection mySelCadastro = new SqlConnection(Funcoes.conexao()))
            {
                mySelCadastro.Open();
                SqlCommand cmdSelCadastro = new SqlCommand("dbo.stp_pessoas_fj_ins", mySelCadastro);
                cmdSelCadastro.CommandType = CommandType.StoredProcedure;
                cmdSelCadastro.Parameters.Add("@flg_operacao", SqlDbType.Char).Value = "S";
                cmdSelCadastro.Parameters.Add("@COD_ID", SqlDbType.Int).Value = Funcoes.strToInt(Convert.ToString(e.CommandArgument));
                cmdSelCadastro.Parameters.Add("@COD_ID_PESSOA_LICENCIADO", SqlDbType.Int).Value = Licenciado;


                SqlDataReader ReaderCadastro = cmdSelCadastro.ExecuteReader();
                while (ReaderCadastro.Read())
                {
                    var json = "";
                    if (ReaderCadastro["FLG_TIPO_PESSOA"].ToString().Trim() == "PF")
                    {
                        json = zoop.ConsultaCadastroSeller(TIRAACENTOS(ReaderCadastro["NOM_CPF"].ToString().Trim()));
                    }
                    if (ReaderCadastro["FLG_TIPO_PESSOA"].ToString().Trim() == "PJ")
                    {
                        json = zoop.ConsultaCadastroSeller(TIRAACENTOS(ReaderCadastro["NOM_CNPJ"].ToString().Trim()));
                    }
                    if (json.ToString().Trim() != "")
                    {
                        try
                        {
                            JObject o = JObject.Parse(json);

                            using (SqlConnection connInsCons = new SqlConnection(Funcoes.conexao()))
                            {
                                connInsCons.Open();
                                SqlCommand cmdInsCons = new SqlCommand("dbo.stp_pessoas_fj_ins", connInsCons);
                                cmdInsCons.CommandType = CommandType.StoredProcedure;
                                cmdInsCons.Parameters.Add("@flg_operacao", SqlDbType.Char).Value = 'O';
                                cmdInsCons.Parameters.Add("@COD_ID_PESSOA_LICENCIADO", SqlDbType.Int).Value = Licenciado;
                                cmdInsCons.Parameters.Add("@COD_ID", SqlDbType.Int).Value = Funcoes.strToInt(Convert.ToString(e.CommandArgument));

                                cmdInsCons.Parameters.Add("@COD_ID_ZOOP_HABILITADO", SqlDbType.Int).Value = Funcoes.strToInt(ReaderCadastro["COD_ID_ZOOP_HABILITADO"].ToString());
                                cmdInsCons.Parameters.Add("@NUM_TOKEN", SqlDbType.VarChar).Value = o["id"].ToString();
                                cmdInsCons.Parameters.Add("@FLG_STATUS_ZOOP", SqlDbType.VarChar).Value = o["status"].ToString();

                                cmdInsCons.Parameters.Add("@COD_ID_PLANO_ZOOP", SqlDbType.Int).Value = Funcoes.strToInt(ReaderCadastro["COD_ID_PLANO_ZOOP"].ToString());

                                cmdInsCons.ExecuteNonQuery();
                                connInsCons.Close();
                                connInsCons.Dispose();
                            }
                        }
                        catch
                        {
                            ClientScript.RegisterStartupScript(this.GetType(),
                                "Alerta", "alert('Ocorreu um erro ao tentar atualizar o status do estabelecimento! Verifique e tente novamente.');", true);
                        }
                    }
                }
            }
            ConsultaGeral();
        }

        if (e.CommandName == "Ativar")
        {

            using (SqlConnection connInsCons = new SqlConnection(Funcoes.conexao()))
            {
                connInsCons.Open();
                SqlCommand cmdInsCons = new SqlCommand("dbo.stp_pessoas_fj_ins", connInsCons);
                cmdInsCons.CommandType = CommandType.StoredProcedure;
                cmdInsCons.Parameters.Add("@flg_operacao", SqlDbType.Char).Value = "N";
                cmdInsCons.Parameters.Add("@COD_ID", SqlDbType.Int).Value = Funcoes.strToInt(Convert.ToString(e.CommandArgument));
                cmdInsCons.Parameters.Add("@COD_ID_PESSOA_LICENCIADO", SqlDbType.Int).Value = Licenciado;
                cmdInsCons.Parameters.Add("@FLG_ATIVO", SqlDbType.Char).Value = "S";
                cmdInsCons.ExecuteNonQuery();
                connInsCons.Close();
                connInsCons.Dispose();
            }
            ClientScript.RegisterStartupScript(this.GetType(),
                "Alerta", "alert('Status alterado com sucesso!');", true);
            ConsultaGeral();
        }
        if (e.CommandName == "Inativar")
        {
            using (SqlConnection connInsCons = new SqlConnection(Funcoes.conexao()))
            {
                connInsCons.Open();
                SqlCommand cmdInsCons = new SqlCommand("dbo.stp_pessoas_fj_ins", connInsCons);
                cmdInsCons.CommandType = CommandType.StoredProcedure;
                cmdInsCons.Parameters.Add("@flg_operacao", SqlDbType.Char).Value = "N";
                cmdInsCons.Parameters.Add("@COD_ID", SqlDbType.Int).Value = Funcoes.strToInt(Convert.ToString(e.CommandArgument));
                cmdInsCons.Parameters.Add("@COD_ID_PESSOA_LICENCIADO", SqlDbType.Int).Value = Licenciado;
                cmdInsCons.Parameters.Add("@FLG_ATIVO", SqlDbType.Char).Value = "N";
                cmdInsCons.ExecuteNonQuery();
                connInsCons.Close();
                connInsCons.Dispose();
            }
            ClientScript.RegisterStartupScript(this.GetType(),
                "Alerta", "alert('Status alterado com sucesso!');", true);
            ConsultaGeral();
        }

        if (e.CommandName == "Pendente")
        {
            using (SqlConnection connInsCons = new SqlConnection(Funcoes.conexao()))
            {
                connInsCons.Open();

                SqlCommand cmdInsCons = new SqlCommand("dbo.stp_pessoas_fj_ins", connInsCons);
                cmdInsCons.CommandType = CommandType.StoredProcedure;
                cmdInsCons.Parameters.Add("@flg_operacao", SqlDbType.Char).Value = "N";
                cmdInsCons.Parameters.Add("@COD_ID", SqlDbType.Int).Value = Funcoes.strToInt(Convert.ToString(e.CommandArgument));
                cmdInsCons.Parameters.Add("@COD_ID_PESSOA_LICENCIADO", SqlDbType.Int).Value = Licenciado;
                cmdInsCons.Parameters.Add("@FLG_ATIVO", SqlDbType.Char).Value = "P";
                cmdInsCons.ExecuteNonQuery();
                connInsCons.Close();
                connInsCons.Dispose();
            }
            ClientScript.RegisterStartupScript(this.GetType(),
                "Alerta", "alert('Status alterado com sucesso!');", true);
            ConsultaGeral();
        }
        if (e.CommandName == "Novo")
        {
            using (SqlConnection connInsCons = new SqlConnection(Funcoes.conexao()))
            {
                connInsCons.Open();

                SqlCommand cmdInsCons = new SqlCommand("dbo.stp_pessoas_fj_ins", connInsCons);
                cmdInsCons.CommandType = CommandType.StoredProcedure;
                cmdInsCons.Parameters.Add("@flg_operacao", SqlDbType.Char).Value = "N";
                cmdInsCons.Parameters.Add("@COD_ID", SqlDbType.Int).Value = Funcoes.strToInt(Convert.ToString(e.CommandArgument));
                cmdInsCons.Parameters.Add("@COD_ID_PESSOA_LICENCIADO", SqlDbType.Int).Value = Licenciado;
                cmdInsCons.Parameters.Add("@FLG_ATIVO", SqlDbType.Char).Value = "V";
                cmdInsCons.ExecuteNonQuery();
                connInsCons.Close();
                connInsCons.Dispose();
            }
            ClientScript.RegisterStartupScript(this.GetType(),
                "Alerta", "alert('Status alterado com sucesso!');", true);
            ConsultaGeral();
        }
        ConsultaDashboard();

    }

    public string GetPostBackScript()
    {
        PostBackOptions options = new PostBackOptions(btnPostback);
        Page.ClientScript.RegisterForEventValidation(options);

        return Page.ClientScript.GetPostBackEventReference(options);
    }

    protected void btnPostBack_Click(object sender, EventArgs e)
    {
        //ClientScript.RegisterStartupScript(this.GetType(),
        //  "AtualizacaoDados", "alert('Caso queira atualizar a consulta exibindo os dados alterados, clique no botão Pesquisar! Caso contrário, basta continuar normalmente.');", true);
        //ConsultaGeral();
    }
    protected void btnNovo_Click(object sender, EventArgs e)
    {
        ClientScript.RegisterStartupScript(this.GetType(),
    "Estabelecimentos", "openPopupWindow('cad_estabelecimentos_steps.aspx?id=" + Funcoes.Encrypt("0").ToString() + "','EstabelecimentosEdicao',1024,800);", true);

    }
    protected void btnImportar_Click(object sender, EventArgs e)
    {
        ClientScript.RegisterStartupScript(this.GetType(),
            "Estabelecimentos", "openPopupWindow('cad_estabelecimentos_importar.aspx?id=0','EstabelecimentosImportacao',1024,800);", true);

    }

    public static string TIRAACENTOS(string str)
    {
        str = str.Replace("-", "");
        str = str.Replace(".", "");
        str = str.Replace("/", "");
        str = str.Replace("(", "");
        str = str.Replace(")", "");
        str = str.Replace(" ", "");
        return str;
    }

    protected void btnNovoPadrao_Click(object sender, EventArgs e)
    {
        ClientScript.RegisterStartupScript(this.GetType(),
    "Estabelecimentos", "openPopupWindow('cad_estabelecimentos_padrao.aspx?id=" + Funcoes.Encrypt("0").ToString() + "','EstabelecimentosEdicao',1024,800);", true);

    }

    private void AtualizarPaginacao()
    {
        int totalPaginas = (int)Math.Ceiling(TotalRegistros / (double)TamanhoPagina);

        lblPagina.Text = "Página " + PaginaAtual.ToString() + " de " + totalPaginas.ToString() + " (Total Registros: " + TotalRegistros.ToString() + ")";
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
            SqlDataAdapter SDAConsulta = new SqlDataAdapter("dbo.stp_pessoas_fj_paginado", myConsulta);
            SDAConsulta.SelectCommand.CommandType = CommandType.StoredProcedure;


            SDAConsulta.SelectCommand.Parameters.Add("@PageNumber", SqlDbType.Int).Value = 1;
            SDAConsulta.SelectCommand.Parameters.Add("@PageSize", SqlDbType.Int).Value = TamanhoPagina;
            SDAConsulta.SelectCommand.Parameters.Add("@Exportar", SqlDbType.Bit).Value = 1;

            SDAConsulta.SelectCommand.Parameters.Add("@COD_ID_PESSOA_LICENCIADO", SqlDbType.Int).Value = Licenciado;
            SDAConsulta.SelectCommand.Parameters.Add("@NOM_RAZAOSOCIAL", SqlDbType.VarChar).Value = txtNome.Text.ToString();
            SDAConsulta.SelectCommand.Parameters.Add("@NUM_DOCUMENTO", SqlDbType.VarChar).Value = txtCNPJCPF.Text.ToString();
            SDAConsulta.SelectCommand.Parameters.Add("@NOM_EMAIL", SqlDbType.VarChar).Value = txtEmail.Text.ToString();
            SDAConsulta.SelectCommand.Parameters.Add("@FLG_ATIVO", SqlDbType.Char).Value = ddlAtivo.SelectedValue.ToString();
            SDAConsulta.SelectCommand.Parameters.Add("@FLG_TIPO", SqlDbType.Char).Value = "E";
            SDAConsulta.SelectCommand.Parameters.Add("@DTA_INICIO", SqlDbType.DateTime).Value = Convert.ToDateTime(txtDataIni.Text.ToString());
            SDAConsulta.SelectCommand.Parameters.Add("@DTA_FIM", SqlDbType.DateTime).Value = Convert.ToDateTime(txtDataFim.Text.ToString());

            if (Tipo == "M")
            {
                SDAConsulta.SelectCommand.Parameters.Add("@COD_ID_MARKETPLACE", SqlDbType.Int).Value = Pessoa;
            }
            else
            {
                if (Funcoes.strToInt(ddlMarketplace.SelectedValue.ToString()) > 0)
                {
                    SDAConsulta.SelectCommand.Parameters.Add("@COD_ID_MARKETPLACE", SqlDbType.Int).Value = Funcoes.strToInt(ddlMarketplace.SelectedValue.ToString());
                }

            }
            if (Tipo == "R")
            {
                SDAConsulta.SelectCommand.Parameters.Add("@COD_ID_REPRESENTANTE", SqlDbType.Int).Value = Pessoa;
            }
            else
            {
                if (Funcoes.strToInt(ddlMarketplace.SelectedValue.ToString()) > 0)
                {
                    SDAConsulta.SelectCommand.Parameters.Add("@COD_ID_REPRESENTANTE", SqlDbType.Int).Value = Funcoes.strToInt(ddlRepresentante.SelectedValue.ToString());
                }
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
            SqlDataAdapter SDAConsulta = new SqlDataAdapter("dbo.stp_pessoas_fj_paginado", myConsulta);
            SDAConsulta.SelectCommand.CommandType = CommandType.StoredProcedure;


            SDAConsulta.SelectCommand.Parameters.Add("@PageNumber", SqlDbType.Int).Value = 1;
            SDAConsulta.SelectCommand.Parameters.Add("@PageSize", SqlDbType.Int).Value = TamanhoPagina;
            SDAConsulta.SelectCommand.Parameters.Add("@Exportar", SqlDbType.Bit).Value = 1;

            SDAConsulta.SelectCommand.Parameters.Add("@COD_ID_PESSOA_LICENCIADO", SqlDbType.Int).Value = Licenciado;
            SDAConsulta.SelectCommand.Parameters.Add("@NOM_RAZAOSOCIAL", SqlDbType.VarChar).Value = txtNome.Text.ToString();
            SDAConsulta.SelectCommand.Parameters.Add("@NUM_DOCUMENTO", SqlDbType.VarChar).Value = txtCNPJCPF.Text.ToString();
            SDAConsulta.SelectCommand.Parameters.Add("@NOM_EMAIL", SqlDbType.VarChar).Value = txtEmail.Text.ToString();
            SDAConsulta.SelectCommand.Parameters.Add("@FLG_ATIVO", SqlDbType.Char).Value = ddlAtivo.SelectedValue.ToString();
            SDAConsulta.SelectCommand.Parameters.Add("@FLG_TIPO", SqlDbType.Char).Value = "E";
            SDAConsulta.SelectCommand.Parameters.Add("@DTA_INICIO", SqlDbType.DateTime).Value = Convert.ToDateTime(txtDataIni.Text.ToString());
            SDAConsulta.SelectCommand.Parameters.Add("@DTA_FIM", SqlDbType.DateTime).Value = Convert.ToDateTime(txtDataFim.Text.ToString());

            if (Tipo == "M")
            {
                SDAConsulta.SelectCommand.Parameters.Add("@COD_ID_MARKETPLACE", SqlDbType.Int).Value = Pessoa;
            }
            else
            {
                if (Funcoes.strToInt(ddlMarketplace.SelectedValue.ToString()) > 0)
                {
                    SDAConsulta.SelectCommand.Parameters.Add("@COD_ID_MARKETPLACE", SqlDbType.Int).Value = Funcoes.strToInt(ddlMarketplace.SelectedValue.ToString());
                }

            }
            if (Tipo == "R")
            {
                SDAConsulta.SelectCommand.Parameters.Add("@COD_ID_REPRESENTANTE", SqlDbType.Int).Value = Pessoa;
            }
            else
            {
                if (Funcoes.strToInt(ddlMarketplace.SelectedValue.ToString()) > 0)
                {
                    SDAConsulta.SelectCommand.Parameters.Add("@COD_ID_REPRESENTANTE", SqlDbType.Int).Value = Funcoes.strToInt(ddlRepresentante.SelectedValue.ToString());
                }
            }

            DataSet dsConsulta = new DataSet();
            SDAConsulta.Fill(dsConsulta);

            ExportarPDF(dsConsulta.Tables[1]);
            myConsulta.Close(); myConsulta.Dispose();
        }


    }
}