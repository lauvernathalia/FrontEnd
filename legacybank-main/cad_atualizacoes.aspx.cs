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


public partial class cad_atualizacoes : System.Web.UI.Page
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

        // Verifica se o usuário esta autenticado
        if (HttpContext.Current.Session.Count <= 0)
        {
            Response.Redirect("login.aspx");
            FormsAuthentication.SignOut();
        }
        if (!IsPostBack)
        {
            SqlConnection myLicenciados = new SqlConnection(Funcoes.conexao());
            myLicenciados.Open();
            SqlCommand cmdLicenciados = new SqlCommand("dbo.stp_pessoas_fj_licenciados_ins", myLicenciados);
            cmdLicenciados.CommandType = CommandType.StoredProcedure;
            cmdLicenciados.Parameters.Add("@flg_operacao", SqlDbType.VarChar).Value = "C";

            cmdLicenciados.Parameters.Add("@COD_ID_PESSOA_LICENCIADO", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["LICENCIADO"].ToString());
            cmdLicenciados.Parameters.Add("@NOM_RAZAOSOCIAL", SqlDbType.VarChar).Value = " ";
            cmdLicenciados.Parameters.Add("@FLG_ATIVO", SqlDbType.Char).Value = " ";
            cmdLicenciados.Parameters.Add("@FLG_TIPO", SqlDbType.Char).Value = "A";

            SqlDataAdapter drLicenciados = new SqlDataAdapter();
            drLicenciados.SelectCommand = cmdLicenciados;
            DataSet dsLicenciados = new DataSet();
            drLicenciados.Fill(dsLicenciados, "PESSOAS_FJ");
            ddlLicenciado.DataTextField = "NOM_RAZAOSOCIAL";
            ddlLicenciado.DataValueField = "COD_ID_PESSOA_LICENCIADO";
            ddlLicenciado.DataSource = dsLicenciados.Tables["PESSOAS_FJ"].DefaultView;
            ddlLicenciado.DataBind();
            ddlLicenciado.Items.Insert(0, new ListItem("Todos", "0"));


            if (Funcoes.strToInt(sid_id) != 0)
            {
                ConsultaFicha();
            }
        }

    }

    private void ConsultaFicha()
    {
        SqlConnection mySelCadastro = new SqlConnection(Funcoes.conexao());
        mySelCadastro.Open();
        SqlCommand cmdSelCadastro = new SqlCommand("dbo.stp_atualizacoes_ins", mySelCadastro);
        cmdSelCadastro.CommandType = CommandType.StoredProcedure;
        cmdSelCadastro.Parameters.Add("@flg_operacao", SqlDbType.Char).Value = "S";
        cmdSelCadastro.Parameters.Add("@COD_ID", SqlDbType.Int).Value = Funcoes.strToInt(sid_id.ToString());
        SqlDataReader ReaderCadastro = cmdSelCadastro.ExecuteReader();
        while (ReaderCadastro.Read())
        {
            lblID.Text = ReaderCadastro["COD_ID"].ToString();
            ddlLicenciado.SelectedValue = ReaderCadastro["COD_ID_PESSOA_LICENCIADO"].ToString();
            txtNome.Text = ReaderCadastro["NOM_ATUALIZACAO"].ToString();
            txtDescricao.InnerText = ReaderCadastro["DES_ATUALIZACAO"].ToString();
            txtDataInicio.Text = ReaderCadastro["DTA_INICIO"].ToString();
            txtDataTermino.Text = ReaderCadastro["DTA_TERMINO"].ToString();
            ddlPerfil.SelectedValue = ReaderCadastro["FLG_PERFIL"].ToString();
            ddlAtivo.SelectedValue = ReaderCadastro["FLG_ATIVO"].ToString();
        }

    }

    protected void btnSalvar_Click(object sender, EventArgs e)
    {
        SqlConnection connInsCons = new SqlConnection(Funcoes.conexao());
        connInsCons.Open();
        SqlCommand cmdInsCons = new SqlCommand("dbo.stp_atualizacoes_ins", connInsCons);
        cmdInsCons.CommandType = CommandType.StoredProcedure;
        if (Funcoes.strToInt(sid_id) != 0)
        {
            cmdInsCons.Parameters.Add("@flg_operacao", SqlDbType.Char).Value = 'A';
            cmdInsCons.Parameters.Add("@COD_ID", SqlDbType.Int).Value = Funcoes.strToInt(sid_id.ToString());
        }
        else
        {
            cmdInsCons.Parameters.Add("@flg_operacao", SqlDbType.Char).Value = 'I';
        }

        cmdInsCons.Parameters.Add("@COD_ID_PESSOA_LICENCIADO", SqlDbType.Int).Value = Funcoes.strToInt(ddlLicenciado.SelectedValue.ToString());
        cmdInsCons.Parameters.Add("@NOM_ATUALIZACAO", SqlDbType.VarChar).Value = txtNome.Text.ToString();
        cmdInsCons.Parameters.Add("@DES_ATUALIZACAO", SqlDbType.Text).Value = txtDescricao.InnerText.ToString();
        cmdInsCons.Parameters.Add("@DTA_INICIO", SqlDbType.DateTime).Value = Convert.ToDateTime(txtDataInicio.Text.ToString());
        cmdInsCons.Parameters.Add("@DTA_TERMINO", SqlDbType.DateTime).Value = Convert.ToDateTime(txtDataTermino.Text.ToString());
        cmdInsCons.Parameters.Add("@FLG_PERFIL", SqlDbType.Char).Value = ddlPerfil.SelectedValue.ToString();
        cmdInsCons.Parameters.Add("@FLG_ATIVO", SqlDbType.Char).Value = ddlAtivo.SelectedValue.ToString();
        cmdInsCons.ExecuteNonQuery();
        connInsCons.Close();
        connInsCons.Dispose();

        ClientScript.RegisterStartupScript(this.GetType(),
            "Alerta", "alert('Dados gravados com sucesso'); opener.PostBackOnMainPage(); window.close(); ", true);
    }
    protected void btnCancelar_Click(object sender, EventArgs e)
    {
        ClientScript.RegisterStartupScript(this.GetType(),
            "Alerta", "opener.PostBackOnMainPage(); window.close(); ", true);
    }

}