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


using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

using System.IO;
using System.Text;

using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;

using System.ComponentModel;

using System.Security.Cryptography;
using System.Net.Mail;
using System.IO.IsolatedStorage;

public partial class cad_equipamentos : System.Web.UI.Page
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
        // Verifica se o usuário esta autenticado
        if (HttpContext.Current.Session.Count <= 0)
        {
            Response.Redirect("login.aspx");
            FormsAuthentication.SignOut();
        }

        if (!IsPostBack)
        {
            ConsultaTabelas();
            ConsultaProprietario();

            if (Funcoes.strToInt(sid_id) != 0)
            {
                ConsultaFicha();
            }
        }

    }

    private void ConsultaTabelas()
    {
        ddlModelo.DataTextField = "NOM_MODELO";
        ddlModelo.DataValueField = "COD_ID";
        ddlModelo.DataSource = tabelas.tModelos().Tables["EQUIPAMENTOS_MODELOS"].DefaultView;
        ddlModelo.DataBind();
        ddlModelo.Items.Insert(0, new ListItem("", "0"));
    }

    private void ConsultaProprietario()
    {
        ddlProprietario.DataTextField = "NOM_RAZAOSOCIAL";
        ddlProprietario.DataValueField = "COD_ID";
        ddlProprietario.DataSource = tabelas.tProprietario(ddlPropriedade.SelectedValue.ToString()).Tables["PESSOAS_FJ"].DefaultView;
        ddlProprietario.DataBind();
        ddlProprietario.Items.Insert(0, new ListItem("", "0"));
    }


    private void ConsultaFicha()
    {
        SqlConnection mySelCadastro = new SqlConnection(Funcoes.conexao());
        mySelCadastro.Open();
        SqlCommand cmdSelCadastro = new SqlCommand("dbo.stp_equipamentos_ins", mySelCadastro);
        cmdSelCadastro.CommandType = CommandType.StoredProcedure;
        cmdSelCadastro.Parameters.Add("@flg_operacao", SqlDbType.Char).Value = "S";
        cmdSelCadastro.Parameters.Add("@COD_ID", SqlDbType.Int).Value = Funcoes.strToInt(sid_id.ToString());
        cmdSelCadastro.Parameters.Add("@COD_ID_PESSOA_LICENCIADO", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["LICENCIADO"].ToString());
        SqlDataReader ReaderCadastro = cmdSelCadastro.ExecuteReader();
        while (ReaderCadastro.Read())
        {
            lblID.Text = ReaderCadastro["COD_ID"].ToString();
            ddlModelo.SelectedValue = ReaderCadastro["COD_ID_EQUIPAMENTOS_MODELO"].ToString();
            txtSerial.Text = ReaderCadastro["NUM_SERIAL"].ToString();
            txtToken.Text = ReaderCadastro["NUM_TOKEN"].ToString();

            ddlPropriedade.SelectedValue = ReaderCadastro["FLG_TIPO"].ToString();
            ConsultaProprietario();
            ddlProprietario.SelectedValue = ReaderCadastro["COD_ID_PESSOAS_FJ"].ToString();
            ddlAtivo.SelectedValue = ReaderCadastro["FLG_ATIVO"].ToString();
        }

    }


    protected void btnSalvar_Click(object sender, EventArgs e)
    {


        SqlConnection connInsCons = new SqlConnection(Funcoes.conexao());
        connInsCons.Open();
        SqlCommand cmdInsCons = new SqlCommand("dbo.stp_equipamentos_ins", connInsCons);
        cmdInsCons.CommandType = CommandType.StoredProcedure;

        if (Funcoes.strToInt(sid_id) != 0)
        {
            cmdInsCons.Parameters.Add("@flg_operacao", SqlDbType.Char).Value = "A";
            cmdInsCons.Parameters.Add("@COD_ID", SqlDbType.Int).Value = Funcoes.strToInt(sid_id.ToString());
        }
        else
        {
            cmdInsCons.Parameters.Add("@flg_operacao", SqlDbType.Char).Value = "I";
        }

        cmdInsCons.Parameters.Add("@COD_ID_PESSOA_LICENCIADO", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["LICENCIADO"].ToString());

        cmdInsCons.Parameters.Add("@COD_ID_EQUIPAMENTOS_MODELO", SqlDbType.Int).Value = Funcoes.strToInt(ddlModelo.SelectedValue.ToString());
        cmdInsCons.Parameters.Add("@NUM_SERIAL", SqlDbType.VarChar).Value = txtSerial.Text.ToString();
        cmdInsCons.Parameters.Add("@NUM_TOKEN", SqlDbType.VarChar).Value = txtToken.Text.ToString();

        cmdInsCons.Parameters.Add("@FLG_TIPO", SqlDbType.Char).Value = ddlPropriedade.SelectedValue.ToString();
        cmdInsCons.Parameters.Add("@COD_ID_PESSOAS_FJ", SqlDbType.Int).Value = Funcoes.strToInt(ddlProprietario.SelectedValue.ToString());
        cmdInsCons.Parameters.Add("@FLG_ATIVO", SqlDbType.Char).Value = ddlAtivo.SelectedValue.ToString();

        cmdInsCons.ExecuteNonQuery();
        connInsCons.Close();
        connInsCons.Dispose();

        ClientScript.RegisterStartupScript(this.GetType(),
            "Alerta", "alert('Dados gravados com sucesso'); opener.PostBackOnMainPage(); window.close();", true);
    }
    protected void btnCancelar_Click(object sender, EventArgs e)
    {
        ClientScript.RegisterStartupScript(this.GetType(),
            "Alerta", "opener.PostBackOnMainPage(); window.close(); ", true);
    }

    protected void ddlPropriedade_SelectedIndexChanged(object sender, EventArgs e)
    {
        ConsultaProprietario();
    }
}