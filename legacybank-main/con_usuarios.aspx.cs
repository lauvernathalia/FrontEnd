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


public partial class con_usuarios : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

        if (HttpContext.Current.Session["TIPO"].ToString() == "E")
        {
            Funcoes.GravaAuditoria(0, Request.ServerVariables["REMOTE_ADDR"].ToString(), "Usuários", "Consulta - Tentativa acesso de usuário não autorizado");
            Response.Redirect("index.aspx");
        }

        
        if (!IsPostBack)
        {
            Funcoes.GravaAuditoria(0, Request.ServerVariables["REMOTE_ADDR"].ToString(), "Usuários", "Consulta");
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
        SqlDataAdapter SDAConsulta = new SqlDataAdapter("dbo.stp_sis_usuario_ins", myConsulta);
        SDAConsulta.SelectCommand.CommandType = CommandType.StoredProcedure;
        SDAConsulta.SelectCommand.Parameters.Add("@flg_operacao", SqlDbType.Char).Value = "C";
        SDAConsulta.SelectCommand.Parameters.Add("@COD_ID_PESSOA_LICENCIADO", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["LICENCIADO"].ToString());
        SDAConsulta.SelectCommand.Parameters.Add("@COD_ID_PESSOAS_FJ", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["PESSOA"].ToString());
        SDAConsulta.SelectCommand.Parameters.Add("@NOM_NOME", SqlDbType.VarChar).Value = txtFiltro.Text.ToString();

        DataSet dsConsulta = new DataSet();
        SDAConsulta.Fill(dsConsulta, "SIS_USUARIO");
        rptConsulta.DataSource = dsConsulta.Tables["SIS_USUARIO"].DefaultView;
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
        ClientScript.RegisterStartupScript(this.GetType(),
            "Usuarios", "openPopupWindow('cad_usuarios.aspx?id=0','UsuariosEdicao',1024,800);", true);

        //Response.Redirect("cad_usuarios.aspx");

    }
}