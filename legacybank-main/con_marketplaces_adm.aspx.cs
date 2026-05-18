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



public partial class con_marketplaces_adm : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            // Tabela de Licenciado

            SqlConnection myLicenciados = new SqlConnection(Funcoes.conexao());
            myLicenciados.Open();
            SqlCommand cmdLicenciados = new SqlCommand("dbo.stp_pessoas_fj_licenciados_ins", myLicenciados);
            cmdLicenciados.CommandType = CommandType.StoredProcedure;
            cmdLicenciados.Parameters.Add("@flg_operacao", SqlDbType.VarChar).Value = "C";

            cmdLicenciados.Parameters.Add("@COD_ID_PESSOA_LICENCIADO", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["LICENCIADO"].ToString());
            cmdLicenciados.Parameters.Add("@NOM_RAZAOSOCIAL", SqlDbType.VarChar).Value = " ";
            cmdLicenciados.Parameters.Add("@FLG_ATIVO", SqlDbType.Char).Value = " ";
            cmdLicenciados.Parameters.Add("@FLG_TIPO", SqlDbType.Char).Value = "M";

            SqlDataAdapter drLicenciados = new SqlDataAdapter();
            drLicenciados.SelectCommand = cmdLicenciados;
            DataSet dsLicenciados = new DataSet();
            drLicenciados.Fill(dsLicenciados, "PESSOAS_FJ");
            ddlMarketplaces.DataTextField = "NOM_RAZAOSOCIAL";
            ddlMarketplaces.DataValueField = "NOM_RAZAOSOCIAL";
            ddlMarketplaces.DataSource = dsLicenciados.Tables["PESSOAS_FJ"].DefaultView;
            ddlMarketplaces.DataBind();
            ddlMarketplaces.Items.Insert(0, new ListItem("Todos", ""));

            ConsultaGeral();
        }

    }
    protected void btnPesquisar_Click(object sender, EventArgs e)
    {
        ConsultaGeral();
    }


    private void ConsultaGeral()
    {
        SqlConnection myConsulta = new SqlConnection(Funcoes.conexao());
        myConsulta.Open();
        SqlDataAdapter SDAConsulta = new SqlDataAdapter("dbo.stp_pessoas_fj_licenciados_ins", myConsulta);
        SDAConsulta.SelectCommand.CommandType = CommandType.StoredProcedure;
        SDAConsulta.SelectCommand.Parameters.Add("@flg_operacao", SqlDbType.Char).Value = "C";
        SDAConsulta.SelectCommand.Parameters.Add("@COD_ID_PESSOA_LICENCIADO", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["LICENCIADO"].ToString());
        SDAConsulta.SelectCommand.Parameters.Add("@NOM_RAZAOSOCIAL", SqlDbType.VarChar).Value = ddlMarketplaces.SelectedValue.ToString();
        SDAConsulta.SelectCommand.Parameters.Add("@FLG_ATIVO", SqlDbType.Char).Value = ddlAtivo.SelectedValue.ToString();
        SDAConsulta.SelectCommand.Parameters.Add("@FLG_TIPO", SqlDbType.Char).Value = "M";


        DataSet dsConsulta = new DataSet();
        SDAConsulta.Fill(dsConsulta, "PESSOAS_FJ");
        rptConsultaCompleta.DataSource = dsConsulta.Tables["PESSOAS_FJ"].DefaultView;
        rptConsultaCompleta.DataBind();
        myConsulta.Close(); myConsulta.Dispose();
    }


    protected void rptConsultaCompleta_OnItemCommand(object source, RepeaterCommandEventArgs e)
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
    protected void btnNovo_Click(object sender, EventArgs e)
    {
        ClientScript.RegisterStartupScript(this.GetType(),
            "Licenciados", "openPopupWindow('cad_licenciados.aspx?id=0','LicenciadosEdicao',1024,800);", true);

    }
}