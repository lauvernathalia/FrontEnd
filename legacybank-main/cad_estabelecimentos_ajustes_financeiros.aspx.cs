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
public partial class cad_estabelecimentos_ajustes_financeiros : System.Web.UI.Page
{
    public string sid_id
    {
        get
        {
            try { return Funcoes.Decrypt(Request["id"].ToString()); }
            catch { return ""; }
        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            txtDataIni.Text = DateTime.Now.AddDays(1).ToShortDateString();

            if (Funcoes.strToInt(sid_id) != 0)
            {
                CarregaAdquirentes();
                CarregaEstabelecimentos();
                CarregaCredor();
                ConsultaFicha();
                ConsultaGeral();
            }

        }
    }


    private void CarregaCredor()
    {
        SqlConnection myEstabelecimentos = new SqlConnection(Funcoes.conexao());
        myEstabelecimentos.Open();
        SqlCommand cmdEstabelecimentos = new SqlCommand("dbo.stp_pessoas_fj_split_ins", myEstabelecimentos);
        cmdEstabelecimentos.CommandType = CommandType.StoredProcedure;
        cmdEstabelecimentos.Parameters.Add("@flg_operacao", SqlDbType.VarChar).Value = "F";

        cmdEstabelecimentos.Parameters.Add("@COD_ID_PESSOA_LICENCIADO", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["LICENCIADO"].ToString());
        cmdEstabelecimentos.Parameters.Add("@FLG_INTEGRACAO", SqlDbType.Char).Value = ddlAdquirentes.SelectedValue.ToString();
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
        ddlCredor.DataTextField = "NOM_RAZAOSOCIAL";
        ddlCredor.DataValueField = "COD_ID";
        ddlCredor.DataSource = dsEstabelecimentos.Tables["PESSOAS_FJ"].DefaultView;
        ddlCredor.DataBind();
    }


    private void CarregaEstabelecimentos()
    {
        SqlConnection myEstabelecimentos = new SqlConnection(Funcoes.conexao());
        myEstabelecimentos.Open();
        SqlCommand cmdEstabelecimentos = new SqlCommand("dbo.stp_pessoas_fj_ins", myEstabelecimentos);
        cmdEstabelecimentos.CommandType = CommandType.StoredProcedure;
        cmdEstabelecimentos.Parameters.Add("@flg_operacao", SqlDbType.VarChar).Value = "B";
        cmdEstabelecimentos.Parameters.Add("@COD_ID_PESSOA_LICENCIADO", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["LICENCIADO"].ToString());
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
        ddlDevedor.DataTextField = "NOM_RAZAOSOCIAL";
        ddlDevedor.DataValueField = "COD_ID";
        ddlDevedor.DataSource = dsEstabelecimentos.Tables["PESSOAS_FJ"].DefaultView;
        ddlDevedor.DataBind();
    }

    private void CarregaAdquirentes()
    {
        SqlConnection myAdquirentes = new SqlConnection(Funcoes.conexao());
        myAdquirentes.Open();
        SqlCommand cmdAdquirentes = new SqlCommand("dbo.stp_pessoas_fj_integracoes_ins", myAdquirentes);
        cmdAdquirentes.CommandType = CommandType.StoredProcedure;
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

        ddlAdquirentes.DataTextField = "NOM_FANTASIA_INTEGRACAO";
        ddlAdquirentes.DataValueField = "FLG_INTEGRACAO";
        ddlAdquirentes.DataSource = dsAdquirentes.Tables["PESSOAS_FJ_INTEGRACOES"].DefaultView;
        ddlAdquirentes.DataBind();
        ddlAdquirentes.Items.Insert(0, new ListItem("Selecione a adquirente", ""));
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
            txtID.Text = ReaderCadastro["COD_ID"].ToString();
            txtRazaosocial.Text = ReaderCadastro["NOM_RAZAOSOCIAL"].ToString();
            txtEmail.Text = ReaderCadastro["NOM_EMAIL"].ToString();
            txtTipo.Text = ReaderCadastro["NOM_FLG_TIPO_PESSOA"].ToString();
            if (ReaderCadastro["FLG_TIPO_PESSOA"].ToString().Trim() == "PF")
            {
                txtDocumento.Text = ReaderCadastro["NOM_CPF"].ToString();
            }
            else
            {
                txtDocumento.Text = ReaderCadastro["NOM_CNPJ"].ToString();

            }
        }

    }


    protected void btnFechar_Click(object sender, EventArgs e)
    {
        ClientScript.RegisterStartupScript(this.GetType(), "FecharOnboarding", "opener.PostBackOnMainPage(); window.close(); ", true);
    }

    private void ConsultaGeral()
    {
        SqlConnection myConsulta = new SqlConnection(Funcoes.conexao());
        myConsulta.Open();
        SqlDataAdapter SDAConsulta = new SqlDataAdapter("dbo.stp_pessoas_fj_ajustes_financeiros_ins", myConsulta);
        SDAConsulta.SelectCommand.CommandType = CommandType.StoredProcedure;
        SDAConsulta.SelectCommand.Parameters.Add("@flg_operacao", SqlDbType.Char).Value = "C";
        SDAConsulta.SelectCommand.Parameters.Add("@COD_ID_PESSOA_LICENCIADO", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["LICENCIADO"].ToString());
        SDAConsulta.SelectCommand.Parameters.Add("@COD_ID_PESSOAS_FJ", SqlDbType.Int).Value = Funcoes.strToInt(sid_id.ToString());

        DataSet dsConsulta = new DataSet();
        SDAConsulta.Fill(dsConsulta, "PESSOAS_FJ_AJUSTES_FINANCEIROS");
        rptConsultaAjustesFinanceiros.DataSource = dsConsulta.Tables["PESSOAS_FJ_AJUSTES_FINANCEIROS"].DefaultView;
        rptConsultaAjustesFinanceiros.DataBind();
        myConsulta.Close(); myConsulta.Dispose();
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

    protected void btnIncluirAjustesFinanceiros_Click(object sender, EventArgs e)
    {
        if ((Funcoes.strToDouble(txtValorTotal.Text.ToString()) > 0) && (Funcoes.strToDouble(txtTaxa.Text.ToString()) > 0) && (ddlAdquirentes.SelectedValue.ToString().Trim() != "") && (txtDataIni.Text.ToString().Trim() != "") && (Funcoes.strToInt(ddlCredor.SelectedValue.ToString()) > 0) && (Funcoes.strToInt(ddlDevedor.SelectedValue.ToString()) > 0))
        {
            SqlConnection connInsCons = new SqlConnection(Funcoes.conexao());
            connInsCons.Open();
            SqlCommand cmdInsCons = new SqlCommand("dbo.stp_pessoas_fj_ajustes_financeiros_ins", connInsCons);
            cmdInsCons.CommandType = CommandType.StoredProcedure;
            cmdInsCons.Parameters.Add("@flg_operacao", SqlDbType.Char).Value = 'I';
            cmdInsCons.Parameters.Add("@COD_ID_PESSOA_LICENCIADO", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["LICENCIADO"].ToString());
            cmdInsCons.Parameters.Add("@COD_ID_PESSOAS_FJ", SqlDbType.Int).Value = Funcoes.strToInt(ddlCredor.SelectedValue.ToString());
            cmdInsCons.Parameters.Add("@FLG_INTEGRACAO", SqlDbType.Char).Value = ddlAdquirentes.SelectedValue.ToString();
            cmdInsCons.Parameters.Add("@COD_ID_PESSOAS_FJ_AJUSTES_FINANCEIROS", SqlDbType.Int).Value = Funcoes.strToInt(ddlDevedor.SelectedValue.ToString());
            cmdInsCons.Parameters.Add("@COD_ID_SIS_USUARIO", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["CODIGO"].ToString());
            cmdInsCons.Parameters.Add("@DTA_DATA", SqlDbType.DateTime).Value = DateTime.Now;
            cmdInsCons.Parameters.Add("@DTA_DATA_INICIO", SqlDbType.DateTime).Value = Convert.ToDateTime(txtDataIni.Text.ToString());
            cmdInsCons.Parameters.Add("@FLG_TIPO_AJUSTE", SqlDbType.Char).Value = ddlTipo.SelectedValue.ToString();
            cmdInsCons.Parameters.Add("@FLG_STATUS", SqlDbType.Char).Value = "A";
            cmdInsCons.Parameters.Add("@FLG_ACEITE", SqlDbType.Char).Value = "S";
            cmdInsCons.Parameters.Add("@NOM_AJUSTES_FINANCEIROS", SqlDbType.VarChar).Value = txtDescricao.Text.ToString();
            cmdInsCons.Parameters.Add("@NUM_VALOR_PERCENTUAL", SqlDbType.Float).Value = Funcoes.strToDouble(txtTaxa.Text.ToString());
            cmdInsCons.Parameters.Add("@NUM_VALOR", SqlDbType.Float).Value = Funcoes.strToDouble(txtValorTotal.Text.ToString());

            cmdInsCons.ExecuteNonQuery();
            connInsCons.Close();
            connInsCons.Dispose();

            ClientScript.RegisterStartupScript(this.GetType(),
                "Alerta", "alert('Ajuste Financeiro inserido com sucesso'); ", true);

            txtTaxa.Text = "";
            txtValorTotal.Text = "";
            ddlTipo.SelectedValue = "P";

            ConsultaGeral();
        }
        else
        {
            ClientScript.RegisterStartupScript(this.GetType(),
                "Alerta", "alert('Todos os campos são de preenchimento obrigatório! Verifique e tente novamente.'); ", true);
        }


    }
    protected void rptConsultaAjustesFinanceiros_ItemCommand(object source, RepeaterCommandEventArgs e)
    {
        if (e.CommandName == "Cancelar")
        {
            SqlConnection connInsCons = new SqlConnection(Funcoes.conexao());
            SqlCommand cmdInsCons = new SqlCommand("dbo.stp_pessoas_fj_ajustes_financeiros_ins", connInsCons);
            cmdInsCons.CommandType = CommandType.StoredProcedure;
            cmdInsCons.Parameters.Add("@flg_operacao", SqlDbType.Char).Value = 'E';
            cmdInsCons.Parameters.Add("@COD_ID", SqlDbType.Int).Value = Funcoes.strToInt(Convert.ToString(e.CommandArgument));
            cmdInsCons.Parameters.Add("@COD_ID_PESSOA_LICENCIADO", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["LICENCIADO"].ToString());
            connInsCons.Open();
            cmdInsCons.ExecuteNonQuery();

            ClientScript.RegisterStartupScript(this.GetType(),
            "CancelamentoAjusteFinanceiro", "alert('Registro Cancelado com sucesso');", true);

            ConsultaGeral();
        }

    }
    protected void ddlAdquirentes_SelectedIndexChanged(object sender, EventArgs e)
    {
        CarregaCredor();
    }
}