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

public partial class con_acesso_dashboard : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
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
        SqlDataAdapter SDAConsulta = new SqlDataAdapter("dbo.stp_dashboard_acessos_ins", myConsulta);
        SDAConsulta.SelectCommand.CommandType = CommandType.StoredProcedure;
        SDAConsulta.SelectCommand.Parameters.Add("@flg_operacao", SqlDbType.Char).Value = "L";
        SDAConsulta.SelectCommand.Parameters.Add("@COD_ID_PESSOA_LICENCIADO", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["LICENCIADO"].ToString());
        SDAConsulta.SelectCommand.Parameters.Add("@FLG_TIPO", SqlDbType.Char).Value = ddlTipo.SelectedValue.ToString();

        DataSet dsConsulta = new DataSet();
        SDAConsulta.Fill(dsConsulta, "DASHBOARD");
        rptConsulta.DataSource = dsConsulta.Tables["DASHBOARD"].DefaultView;
        rptConsulta.DataBind();
        myConsulta.Close(); myConsulta.Dispose();
    }


    protected void rptConsulta_OnItemCommand(object source, RepeaterCommandEventArgs e)
    {

        if (e.CommandName == "Liberar")
        {
            foreach (RepeaterItem itemP in rptConsulta.Items)
            {
                if (Funcoes.strToInt(Convert.ToString(e.CommandArgument)) == Funcoes.strToInt(((TextBox)itemP.FindControl("txtid")).Text.ToString()))
                {
                    SqlConnection connInsCons = new SqlConnection(Funcoes.conexao());
                    SqlCommand cmdInsCons = new SqlCommand("dbo.stp_dashboard_acessos_ins", connInsCons);
                    cmdInsCons.CommandType = CommandType.StoredProcedure;
                    cmdInsCons.Parameters.Add("@flg_operacao", SqlDbType.Char).Value = 'I';
                    cmdInsCons.Parameters.Add("@COD_ID_DASHBOARD", SqlDbType.Int).Value = Funcoes.strToInt(((TextBox)itemP.FindControl("txtid")).Text.ToString());
                    cmdInsCons.Parameters.Add("@COD_ID_PESSOA_LICENCIADO", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["LICENCIADO"].ToString());
                    cmdInsCons.Parameters.Add("@FLG_ACESSO", SqlDbType.Char).Value = "S";
                    connInsCons.Open();
                    cmdInsCons.ExecuteNonQuery();
                }

            }
            ConsultaGeral();

        }
        if (e.CommandName == "Bloquear")
        {
            foreach (RepeaterItem itemP in rptConsulta.Items)
            {
                if (Funcoes.strToInt(Convert.ToString(e.CommandArgument)) == Funcoes.strToInt(((TextBox)itemP.FindControl("txtid")).Text.ToString()))
                {
                    SqlConnection connInsCons = new SqlConnection(Funcoes.conexao());
                    SqlCommand cmdInsCons = new SqlCommand("dbo.stp_dashboard_acessos_ins", connInsCons);
                    cmdInsCons.CommandType = CommandType.StoredProcedure;
                    cmdInsCons.Parameters.Add("@flg_operacao", SqlDbType.Char).Value = 'I';
                    cmdInsCons.Parameters.Add("@COD_ID_DASHBOARD", SqlDbType.Int).Value = Funcoes.strToInt(((TextBox)itemP.FindControl("txtid")).Text.ToString());
                    cmdInsCons.Parameters.Add("@COD_ID_PESSOA_LICENCIADO", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["LICENCIADO"].ToString());
                    cmdInsCons.Parameters.Add("@FLG_ACESSO", SqlDbType.Char).Value = "N";
                    connInsCons.Open();
                    cmdInsCons.ExecuteNonQuery();
                }

            }
            ConsultaGeral();

        }

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
    protected void ddlTipo_SelectedIndexChanged(object sender, EventArgs e)
    {
        ConsultaGeral();
    }
}