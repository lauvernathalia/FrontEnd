using System;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;

public partial class con_relatorios_comissao : System.Web.UI.Page
{
    private int Licenciado;
    private int Pessoa;
    private string Tipo;

    protected void Page_Load(object sender, EventArgs e)
    {
        // ===== SESSÃO SEGURA (PADRÃO LEGADO) =====
        if (Session["LICENCIADO"] != null)
            Licenciado = Funcoes.strToInt(Session["LICENCIADO"].ToString());
        else
            Licenciado = 0;

        if (Session["PESSOA"] != null)
            Pessoa = Funcoes.strToInt(Session["PESSOA"].ToString());
        else
            Pessoa = 0;

        if (Session["TIPO"] != null)
            Tipo = Session["TIPO"].ToString().Trim();
        else
            Tipo = "";

        if (!IsPostBack)
        {
            txtAno.Text = DateTime.Now.Year.ToString();

            // ================= REPRESENTANTES =================
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
                }
            }

            if (Tipo == "R")
            {
                ddlRepresentante.SelectedValue = Pessoa.ToString();
                ddlRepresentante.Enabled = false;
            }

            // ================= MARKETPLACES =================
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
                }
            }

            if (Tipo == "R")
                ddlMarketplace.Enabled = false;

            if (Tipo == "M")
            {
                ddlMarketplace.SelectedValue = Pessoa.ToString();
                ddlMarketplace.Enabled = false;
            }

            CarregaAdquirentes();
        }
    }

    protected void btnPesquisar_Click(object sender, EventArgs e)
    {
        ConsultaGeral();
    }

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

                    ddlAdquirentes.DataTextField = "NOM_FANTASIA_INTEGRACAO";
                    ddlAdquirentes.DataValueField = "FLG_INTEGRACAO";
                    ddlAdquirentes.DataSource = ds.Tables[0];
                    ddlAdquirentes.DataBind();
                    ddlAdquirentes.Items.Insert(0, new ListItem("Todos", ""));
                }
            }
        }
    }
    
    /*
    private void ConsultaGeral()
    {
        dvConsulta.Visible = false;
        dvConsultaParcial.Visible = false;

        using (SqlConnection con = new SqlConnection(Funcoes.conexao()))
        {
            con.Open();
            using (SqlDataAdapter da = new SqlDataAdapter("dbo.stp_pessoas_fj_conta_corrente_ins", con))
            {
                da.SelectCommand.CommandType = CommandType.StoredProcedure;
                da.SelectCommand.CommandTimeout = 120;

                da.SelectCommand.Parameters.Add("@flg_operacao", SqlDbType.Char).Value = "R";
                da.SelectCommand.Parameters.Add("@FLG_MODELO", SqlDbType.Char).Value = "C";
                da.SelectCommand.Parameters.Add("@FLG_ORIGEM", SqlDbType.Char).Value = ddlAdquirentes.SelectedValue;
                da.SelectCommand.Parameters.Add("@COD_ID_PESSOA_LICENCIADO", SqlDbType.Int).Value = Licenciado;
                da.SelectCommand.Parameters.Add("@NUM_ANO", SqlDbType.Int).Value = Funcoes.strToInt(txtAno.Text);

                int rep = Funcoes.strToInt(ddlRepresentante.SelectedValue);
                if (rep > 0)
                    da.SelectCommand.Parameters.Add("@COD_ID_REPRESENTANTE", SqlDbType.Int).Value = rep;

                int mkt = Funcoes.strToInt(ddlMarketplace.SelectedValue);
                if (mkt > 0)
                    da.SelectCommand.Parameters.Add("@COD_ID_MARKETPLACE", SqlDbType.Int).Value = mkt;

                DataSet ds = new DataSet();
                da.Fill(ds);

                dvConsulta.Visible = true;
                gvConsulta.DataSource = ds.Tables[0];
                gvConsulta.DataBind();
            }
        }
    }
    */

    private void ConsultaGeral()
    {
        dvConsulta.Visible = false;
        dvConsultaParcial.Visible = false;

        try
        {
            using (SqlConnection con = new SqlConnection(Funcoes.conexao()))
            using (SqlDataAdapter da = new SqlDataAdapter("dbo.stp_pessoas_fj_conta_corrente_ins", con))
            {
                da.SelectCommand.CommandType = CommandType.StoredProcedure;
                da.SelectCommand.CommandTimeout = 300;

                da.SelectCommand.Parameters.Add("@flg_operacao", SqlDbType.Char).Value = "R";
                da.SelectCommand.Parameters.Add("@FLG_MODELO", SqlDbType.Char).Value = "C";
                da.SelectCommand.Parameters.Add("@FLG_ORIGEM", SqlDbType.Char).Value = ddlAdquirentes.SelectedValue;
                da.SelectCommand.Parameters.Add("@COD_ID_PESSOA_LICENCIADO", SqlDbType.Int).Value = Licenciado;
                da.SelectCommand.Parameters.Add("@NUM_ANO", SqlDbType.Int).Value = Funcoes.strToInt(txtAno.Text);

                int rep = Funcoes.strToInt(ddlRepresentante.SelectedValue);
                if (rep > 0)
                    da.SelectCommand.Parameters.Add("@COD_ID_REPRESENTANTE", SqlDbType.Int).Value = rep;

                int mkt = Funcoes.strToInt(ddlMarketplace.SelectedValue);
                if (mkt > 0)
                    da.SelectCommand.Parameters.Add("@COD_ID_MARKETPLACE", SqlDbType.Int).Value = mkt;

                DataSet ds = new DataSet();
                da.Fill(ds);

                // ✅ VALIDAÇÃO CRÍTICA
                if (ds.Tables.Count > 0)
                {
                    gvConsulta.DataSource = ds.Tables[0];
                    gvConsulta.DataBind();
                    dvConsulta.Visible = true;
                }
                else
                {
                    // Nenhum resultado retornado
                    gvConsulta.DataSource = null;
                    gvConsulta.DataBind();
                    dvConsultaParcial.Visible = true; // se quiser exibir msg
                }
            }
        }
        catch (SqlException ex)
        {
            if (ex.Number == -2) // timeout
            {
                throw new Exception(
                    "A consulta demorou mais do que o esperado devido ao volume de dados. " +
                    "Refine os filtros (representante, marketplace ou ano).",
                    ex
                );
            }

            throw;
        }
    }


    protected void gvConsulta_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (Tipo == "R")
        {
            if (e.Row.RowType == DataControlRowType.DataRow || e.Row.RowType == DataControlRowType.Header)
            {
                e.Row.Cells[3].Visible = false;
                e.Row.Cells[4].Visible = false;
                e.Row.Cells[5].Visible = false;
            }

            if (e.Row.RowType == DataControlRowType.Header)
                e.Row.Cells[6].Text = "Comissão";
        }

        if (Tipo == "M")
        {
            if (e.Row.RowType == DataControlRowType.DataRow || e.Row.RowType == DataControlRowType.Header)
            {
                e.Row.Cells[3].Visible = false;
                e.Row.Cells[5].Visible = false;
            }

            if (e.Row.RowType == DataControlRowType.Header)
                e.Row.Cells[4].Text = "Comissão";
        }
    }
}
