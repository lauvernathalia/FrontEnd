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

public partial class cad_tabela_markup : System.Web.UI.Page
{
    public string sAdquirente
    {
        get
        {
            try { return Funcoes.Decrypt(Request["adquirente"].ToString()); }
            catch { return ""; }
        }
    }
    public string sTipo
    {
        get
        {
            try { return Funcoes.Decrypt(Request["tipo"].ToString()); }
            catch { return ""; }
        }
    }
    public string sNome
    {
        get
        {
            try { return Funcoes.Decrypt(Request["nome"].ToString()); }
            catch { return ""; }
        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            txtNome.Text = sNome;
            CarregaAdquirentes();

            if (sAdquirente.ToString().Trim() != "") { ddlAdquirentes.SelectedValue =  sAdquirente; }
            if (sTipo.ToString().Trim() != "") { ddlPresencialOnLine.SelectedValue = sTipo; }

            ConsultaGeral();
        }

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
        ddlAdquirentes.DataValueField = "COD_ID";
        ddlAdquirentes.DataSource = dsAdquirentes.Tables["PESSOAS_FJ_INTEGRACOES"].DefaultView;
        ddlAdquirentes.DataBind();
        //ddlAdquirentes.Items.Insert(0, new ListItem("Selecione a adquirente", "0"));
    }


    private void ConsultaGeral()
    {
        SqlConnection myConsulta = new SqlConnection(Funcoes.conexao());
        myConsulta.Open();
        SqlDataAdapter SDAConsulta = new SqlDataAdapter("dbo.stp_markup_parcelas_ins", myConsulta);
        SDAConsulta.SelectCommand.CommandType = CommandType.StoredProcedure;
        SDAConsulta.SelectCommand.Parameters.Add("@flg_operacao", SqlDbType.Char).Value = "P";

        SDAConsulta.SelectCommand.Parameters.Add("@COD_ID_PESSOA_LICENCIADO", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["LICENCIADO"].ToString());
        SDAConsulta.SelectCommand.Parameters.Add("@COD_ID_PESSOAS_FJ_MARKUP", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["PESSOA"].ToString());
        SDAConsulta.SelectCommand.Parameters.Add("@FLG_PRESENCIAL_ONLINE", SqlDbType.Char).Value = ddlPresencialOnLine.SelectedValue.ToString();
        SDAConsulta.SelectCommand.Parameters.Add("@COD_ID_INTEGRACAO", SqlDbType.Int).Value = Funcoes.strToInt(ddlAdquirentes.SelectedValue.ToString());

        DataSet dsConsulta = new DataSet();
        SDAConsulta.Fill(dsConsulta, "MARKUP_PARCELAS");
        rptConsultaMarkup.DataSource = dsConsulta.Tables["MARKUP_PARCELAS"].DefaultView;
        rptConsultaMarkup.DataBind();
        myConsulta.Close(); myConsulta.Dispose();
    }

    protected void btnFechar_Click(object sender, EventArgs e)
    {
        ClientScript.RegisterStartupScript(this.GetType(), "FecharOnboarding", "opener.PostBackOnMainPage(); window.close(); ", true);

    }
    protected void rptConsultaMarkup_ItemCommand(object source, RepeaterCommandEventArgs e)
    {

    }
    protected void ddlAdquirentes_SelectedIndexChanged(object sender, EventArgs e)
    {
        ConsultaGeral();
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

    private void GravarParcelas()
    {
        foreach (RepeaterItem itemP in rptConsultaMarkup.Items)
        {
            SqlConnection connInsCons = new SqlConnection(Funcoes.conexao());
            connInsCons.Open();
            SqlCommand cmdInsCons = new SqlCommand("dbo.stp_markup_parcelas_ins", connInsCons);
            cmdInsCons.CommandType = CommandType.StoredProcedure;
            cmdInsCons.Parameters.Add("@flg_operacao", SqlDbType.Char).Value = 'I';
            cmdInsCons.Parameters.Add("@COD_ID_PESSOA_LICENCIADO", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["LICENCIADO"].ToString());
            cmdInsCons.Parameters.Add("@COD_ID_PESSOAS_FJ_MARKUP", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["PESSOA"].ToString());
            cmdInsCons.Parameters.Add("@COD_ID_BANDEIRA", SqlDbType.Int).Value = Funcoes.strToInt(((TextBox)itemP.FindControl("txtidbandeira")).Text.ToString());
            cmdInsCons.Parameters.Add("@COD_ID_INTEGRACAO", SqlDbType.Int).Value = Funcoes.strToInt(ddlAdquirentes.SelectedValue.ToString());
            cmdInsCons.Parameters.Add("@FLG_PRESENCIAL_ONLINE", SqlDbType.Char).Value = ddlPresencialOnLine.SelectedValue.ToString();
            cmdInsCons.Parameters.Add("@NOM_MARKUP", SqlDbType.VarChar).Value = txtNome.Text.ToString();

            cmdInsCons.Parameters.Add("@NUM_VALOR_MARKUP_DEBITO", SqlDbType.Float).Value = (Funcoes.IsDoubleRealNumber(((TextBox)itemP.FindControl("txtmarkupdebito")).Text.ToString())) ? Funcoes.strToDouble(((TextBox)itemP.FindControl("txtmarkupdebito")).Text.ToString()) : 0;
            cmdInsCons.Parameters.Add("@NUM_VALOR_MARKUP_VISTA", SqlDbType.Float).Value = (Funcoes.IsDoubleRealNumber(((TextBox)itemP.FindControl("txtmarkupcredito")).Text.ToString())) ? Funcoes.strToDouble(((TextBox)itemP.FindControl("txtmarkupcredito")).Text.ToString()) : 0;
            cmdInsCons.Parameters.Add("@NUM_VALOR_MARKUP_2X", SqlDbType.Float).Value = (Funcoes.IsDoubleRealNumber(((TextBox)itemP.FindControl("txtmarkup2x")).Text.ToString())) ? Funcoes.strToDouble(((TextBox)itemP.FindControl("txtmarkup2x")).Text.ToString()) : 0;
            cmdInsCons.Parameters.Add("@NUM_VALOR_MARKUP_3X", SqlDbType.Float).Value = (Funcoes.IsDoubleRealNumber(((TextBox)itemP.FindControl("txtmarkup3x")).Text.ToString())) ? Funcoes.strToDouble(((TextBox)itemP.FindControl("txtmarkup3x")).Text.ToString()) : 0;
            cmdInsCons.Parameters.Add("@NUM_VALOR_MARKUP_4X", SqlDbType.Float).Value = (Funcoes.IsDoubleRealNumber(((TextBox)itemP.FindControl("txtmarkup4x")).Text.ToString())) ? Funcoes.strToDouble(((TextBox)itemP.FindControl("txtmarkup4x")).Text.ToString()) : 0;
            cmdInsCons.Parameters.Add("@NUM_VALOR_MARKUP_5X", SqlDbType.Float).Value = (Funcoes.IsDoubleRealNumber(((TextBox)itemP.FindControl("txtmarkup5x")).Text.ToString())) ? Funcoes.strToDouble(((TextBox)itemP.FindControl("txtmarkup5x")).Text.ToString()) : 0;
            cmdInsCons.Parameters.Add("@NUM_VALOR_MARKUP_6X", SqlDbType.Float).Value = (Funcoes.IsDoubleRealNumber(((TextBox)itemP.FindControl("txtmarkup6x")).Text.ToString())) ? Funcoes.strToDouble(((TextBox)itemP.FindControl("txtmarkup6x")).Text.ToString()) : 0;
            cmdInsCons.Parameters.Add("@NUM_VALOR_MARKUP_7X", SqlDbType.Float).Value = (Funcoes.IsDoubleRealNumber(((TextBox)itemP.FindControl("txtmarkup7x")).Text.ToString())) ? Funcoes.strToDouble(((TextBox)itemP.FindControl("txtmarkup7x")).Text.ToString()) : 0;
            cmdInsCons.Parameters.Add("@NUM_VALOR_MARKUP_8X", SqlDbType.Float).Value = (Funcoes.IsDoubleRealNumber(((TextBox)itemP.FindControl("txtmarkup8x")).Text.ToString())) ? Funcoes.strToDouble(((TextBox)itemP.FindControl("txtmarkup8x")).Text.ToString()) : 0;
            cmdInsCons.Parameters.Add("@NUM_VALOR_MARKUP_9X", SqlDbType.Float).Value = (Funcoes.IsDoubleRealNumber(((TextBox)itemP.FindControl("txtmarkup9x")).Text.ToString())) ? Funcoes.strToDouble(((TextBox)itemP.FindControl("txtmarkup9x")).Text.ToString()) : 0;
            cmdInsCons.Parameters.Add("@NUM_VALOR_MARKUP_10X", SqlDbType.Float).Value = (Funcoes.IsDoubleRealNumber(((TextBox)itemP.FindControl("txtmarkup10x")).Text.ToString())) ? Funcoes.strToDouble(((TextBox)itemP.FindControl("txtmarkup10x")).Text.ToString()) : 0;
            cmdInsCons.Parameters.Add("@NUM_VALOR_MARKUP_11X", SqlDbType.Float).Value = (Funcoes.IsDoubleRealNumber(((TextBox)itemP.FindControl("txtmarkup11x")).Text.ToString())) ? Funcoes.strToDouble(((TextBox)itemP.FindControl("txtmarkup11x")).Text.ToString()) : 0;
            cmdInsCons.Parameters.Add("@NUM_VALOR_MARKUP_12X", SqlDbType.Float).Value = (Funcoes.IsDoubleRealNumber(((TextBox)itemP.FindControl("txtmarkup12x")).Text.ToString())) ? Funcoes.strToDouble(((TextBox)itemP.FindControl("txtmarkup12x")).Text.ToString()) : 0;
            cmdInsCons.Parameters.Add("@NUM_VALOR_MARKUP_13X", SqlDbType.Float).Value = (Funcoes.IsDoubleRealNumber(((TextBox)itemP.FindControl("txtmarkup13x")).Text.ToString())) ? Funcoes.strToDouble(((TextBox)itemP.FindControl("txtmarkup13x")).Text.ToString()) : 0;
            cmdInsCons.Parameters.Add("@NUM_VALOR_MARKUP_14X", SqlDbType.Float).Value = (Funcoes.IsDoubleRealNumber(((TextBox)itemP.FindControl("txtmarkup14x")).Text.ToString())) ? Funcoes.strToDouble(((TextBox)itemP.FindControl("txtmarkup14x")).Text.ToString()) : 0;
            cmdInsCons.Parameters.Add("@NUM_VALOR_MARKUP_15X", SqlDbType.Float).Value = (Funcoes.IsDoubleRealNumber(((TextBox)itemP.FindControl("txtmarkup15x")).Text.ToString())) ? Funcoes.strToDouble(((TextBox)itemP.FindControl("txtmarkup15x")).Text.ToString()) : 0;
            cmdInsCons.Parameters.Add("@NUM_VALOR_MARKUP_16X", SqlDbType.Float).Value = (Funcoes.IsDoubleRealNumber(((TextBox)itemP.FindControl("txtmarkup16x")).Text.ToString())) ? Funcoes.strToDouble(((TextBox)itemP.FindControl("txtmarkup16x")).Text.ToString()) : 0;
            cmdInsCons.Parameters.Add("@NUM_VALOR_MARKUP_17X", SqlDbType.Float).Value = (Funcoes.IsDoubleRealNumber(((TextBox)itemP.FindControl("txtmarkup17x")).Text.ToString())) ? Funcoes.strToDouble(((TextBox)itemP.FindControl("txtmarkup17x")).Text.ToString()) : 0;
            cmdInsCons.Parameters.Add("@NUM_VALOR_MARKUP_18X", SqlDbType.Float).Value = (Funcoes.IsDoubleRealNumber(((TextBox)itemP.FindControl("txtmarkup18x")).Text.ToString())) ? Funcoes.strToDouble(((TextBox)itemP.FindControl("txtmarkup18x")).Text.ToString()) : 0;
            cmdInsCons.Parameters.Add("@NUM_VALOR_MARKUP_19X", SqlDbType.Float).Value = (Funcoes.IsDoubleRealNumber(((TextBox)itemP.FindControl("txtmarkup19x")).Text.ToString())) ? Funcoes.strToDouble(((TextBox)itemP.FindControl("txtmarkup19x")).Text.ToString()) : 0;
            cmdInsCons.Parameters.Add("@NUM_VALOR_MARKUP_20X", SqlDbType.Float).Value = (Funcoes.IsDoubleRealNumber(((TextBox)itemP.FindControl("txtmarkup20x")).Text.ToString())) ? Funcoes.strToDouble(((TextBox)itemP.FindControl("txtmarkup20x")).Text.ToString()) : 0;
            cmdInsCons.Parameters.Add("@NUM_VALOR_MARKUP_21X", SqlDbType.Float).Value = (Funcoes.IsDoubleRealNumber(((TextBox)itemP.FindControl("txtmarkup21x")).Text.ToString())) ? Funcoes.strToDouble(((TextBox)itemP.FindControl("txtmarkup21x")).Text.ToString()) : 0;
            cmdInsCons.Parameters.Add("@NUM_VALOR_MARKUP_TRANSACAO", SqlDbType.Float).Value = (Funcoes.IsDoubleRealNumber(((TextBox)itemP.FindControl("txtmarkuptransacao")).Text.ToString())) ? Funcoes.strToDouble(((TextBox)itemP.FindControl("txtmarkuptransacao")).Text.ToString()) : 0;

            cmdInsCons.ExecuteNonQuery();
            connInsCons.Close();
            connInsCons.Dispose();
            ClientScript.RegisterStartupScript(this.GetType(), "GravarParecelasSucesso", "alert('Dados foram gravados com sucesso!'); opener.PostBackOnMainPage(); window.close();", true);

        }

    }


    protected void ddlPresencialOnLine_SelectedIndexChanged(object sender, EventArgs e)
    {
        ConsultaGeral();
    }
    protected void btnSalvar_Click(object sender, EventArgs e)
    {
        if (txtNome.Text.ToString().Trim() != "")
        {
            GravarParcelas();
        }
        else
        {
            ClientScript.RegisterStartupScript(this.GetType(), "GravarParecelas", "alert('É obrigatório especificar um nome para a tabela de Markup! Exemplo: Markup 20 ou Markup 10'); ", true);
        }
    }
}