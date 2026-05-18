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

public partial class con_parceiros : System.Web.UI.Page
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
        SqlDataAdapter SDAConsulta = new SqlDataAdapter("dbo.stp_parceiros_ins", myConsulta);
        SDAConsulta.SelectCommand.CommandType = CommandType.StoredProcedure;
        SDAConsulta.SelectCommand.Parameters.Add("@flg_operacao", SqlDbType.Char).Value = "C";
        SDAConsulta.SelectCommand.Parameters.Add("@COD_ID_PESSOA_LICENCIADO", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["LICENCIADO"].ToString());
        SDAConsulta.SelectCommand.Parameters.Add("@COD_ID_PESSOAS_FJ", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["PESSOA"].ToString());
        SDAConsulta.SelectCommand.Parameters.Add("@NOM_NOME", SqlDbType.VarChar).Value = txtNome.Text.ToString();
        SDAConsulta.SelectCommand.Parameters.Add("@NOM_CNPJ", SqlDbType.VarChar).Value = txtDocumento.Text.ToString();
        SDAConsulta.SelectCommand.Parameters.Add("@FLG_TIPO", SqlDbType.Char).Value = "P";
        SDAConsulta.SelectCommand.Parameters.Add("@FLG_INTEGRACAO", SqlDbType.Char).Value = "Z";

        DataSet dsConsulta = new DataSet();
        SDAConsulta.Fill(dsConsulta, "PESSOAS_FJ");
        rptConsulta.DataSource = dsConsulta.Tables["PESSOAS_FJ"].DefaultView;
        rptConsulta.DataBind();
        myConsulta.Close(); myConsulta.Dispose();
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

    protected void btnPostBack_Click(object sender, EventArgs e)
    {
        ConsultaGeral();
    }


    protected void btnNovo_Click(object sender, EventArgs e)
    {
        const string someScript = "NovoRegistro";
        ClientScript.RegisterStartupScript(this.GetType(),
            someScript, "openPopupWindow('cad_parceiros.aspx?id=0','CompradoresEdicao',1024,800);", true);

    }
    protected void btnExportar_Click(object sender, EventArgs e)
    {

    }
}