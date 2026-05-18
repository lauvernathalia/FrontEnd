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


public partial class cad_licenciados_integracoes_chaves : System.Web.UI.Page
{
    public string sid_id
    {
        get
        {
            try { return Request["id"]; }
            catch { return ""; }
        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        // Verifica se o usuário esta autenticado
        if (HttpContext.Current.Session.Count <= 0)
        {
            Response.Redirect("login.aspx");
            FormsAuthentication.SignOut();
        }

        if (!IsPostBack)
        {
            ConsultaTabelas();

            if (Funcoes.strToInt(sid_id) != 0)
            {
                ConsultaFicha();
                ConsultaGeral();
            }
        }

    }

    private void ConsultaTabelas()
    {
        ddlIntegracao.DataTextField = "NOM_INTEGRACAO";
        ddlIntegracao.DataValueField = "COD_ID";
        ddlIntegracao.DataSource = tabelas.tIntegracoes().Tables["INTEGRACOES"].DefaultView;
        ddlIntegracao.DataBind();
        ddlIntegracao.Items.Insert(0, new ListItem("", "0"));
    }

    private void ConsultaFicha()
    {
        SqlConnection mySelCadastro = new SqlConnection(Funcoes.conexao());
        mySelCadastro.Open();
        SqlCommand cmdSelCadastro = new SqlCommand("dbo.stp_pessoas_fj_licenciados_ins", mySelCadastro);
        cmdSelCadastro.CommandType = CommandType.StoredProcedure;
        cmdSelCadastro.Parameters.Add("@flg_operacao", SqlDbType.Char).Value = "S";
        cmdSelCadastro.Parameters.Add("@COD_ID", SqlDbType.Int).Value = Funcoes.strToInt(sid_id.ToString());
        SqlDataReader ReaderCadastro = cmdSelCadastro.ExecuteReader();
        while (ReaderCadastro.Read())
        {
            txtIDLicenciado.Text = ReaderCadastro["COD_ID"].ToString();
            txtCodigoLicenciado.Text = ReaderCadastro["COD_ID_PESSOA_LICENCIADO"].ToString();
            txtLicenciado.Text = ReaderCadastro["NOM_RAZAOSOCIAL"].ToString();
        }
    }

    private void ConsultaGeral()
    {
        SqlConnection myConsulta = new SqlConnection(Funcoes.conexao());
        myConsulta.Open();
        SqlDataAdapter SDAConsulta = new SqlDataAdapter("dbo.stp_pessoas_fj_integracoes_chaves_ins", myConsulta);
        SDAConsulta.SelectCommand.CommandType = CommandType.StoredProcedure;
        SDAConsulta.SelectCommand.Parameters.Add("@flg_operacao", SqlDbType.Char).Value = "C";
        SDAConsulta.SelectCommand.Parameters.Add("@COD_ID_PESSOA_LICENCIADO", SqlDbType.Int).Value = Funcoes.strToInt(txtCodigoLicenciado.Text.ToString());
        DataSet dsConsulta = new DataSet();
        SDAConsulta.Fill(dsConsulta, "PESSOAS_FJ");

        rptConsultaUsuario.DataSource = dsConsulta.Tables["PESSOAS_FJ"].DefaultView;
        rptConsultaUsuario.DataBind();

        myConsulta.Close(); myConsulta.Dispose();
    }


    protected void btnIncluir_Click(object sender, EventArgs e)
    {
        SqlConnection connInsCons = new SqlConnection(Funcoes.conexao());
        connInsCons.Open();
        SqlCommand cmdInsCons = new SqlCommand("dbo.stp_pessoas_fj_integracoes_chaves_ins", connInsCons);
        cmdInsCons.CommandType = CommandType.StoredProcedure;
        cmdInsCons.Parameters.Add("@flg_operacao", SqlDbType.Char).Value = 'I';
        cmdInsCons.Parameters.Add("@COD_ID_PESSOA_LICENCIADO", SqlDbType.Int).Value = Funcoes.strToInt(txtCodigoLicenciado.Text.ToString());
        cmdInsCons.Parameters.Add("@COD_ID_INTEGRACOES", SqlDbType.Int).Value = Funcoes.strToInt(ddlIntegracao.SelectedValue.ToString());

        cmdInsCons.Parameters.Add("@NOM_TOKEN", SqlDbType.Text).Value = txtToken.Text.ToString();
        cmdInsCons.Parameters.Add("@NOM_KEY", SqlDbType.Text).Value = txtKey.Text.ToString();
        cmdInsCons.Parameters.Add("@NOM_SECRET", SqlDbType.Text).Value = txtSecret.Text.ToString();
        cmdInsCons.Parameters.Add("@NOM_ID", SqlDbType.Text).Value = txtChaveID.Text.ToString();
        cmdInsCons.Parameters.Add("@NOM_WEBHOOK_ID", SqlDbType.VarChar).Value = txtWebhookID.Text.ToString();
        cmdInsCons.Parameters.Add("@FLG_ATIVO", SqlDbType.Text).Value = ddlAtivo.SelectedValue.ToString();

        cmdInsCons.ExecuteNonQuery();
        connInsCons.Close();
        connInsCons.Dispose();

        ClientScript.RegisterStartupScript(this.GetType(),
            "Alerta", "alert('Dados gravados com sucesso'); ", true);

        txtToken.Text = ""; txtKey.Text = ""; txtSecret.Text = ""; txtChaveID.Text = "";
        ConsultaGeral();

    }
    protected void btnCancelar_Click(object sender, EventArgs e)
    {
        ClientScript.RegisterStartupScript(this.GetType(),
    "Alerta", "opener.PostBackOnMainPage(); window.close(); ", true);

    }
    protected void rptConsultaUsuario_ItemCommand(object source, RepeaterCommandEventArgs e)
    {

    }
    protected void lkbGerar_Click(object sender, System.EventArgs e)
    {
        txtWebhookID.Text = Guid.NewGuid().ToString(); 
    }
}