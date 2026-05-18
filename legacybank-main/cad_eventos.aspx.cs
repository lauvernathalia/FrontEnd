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


public partial class cad_eventos : System.Web.UI.Page
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
            ConsultaRealizador();

            if (Funcoes.strToInt(sid_id) != 0)
            {
                ConsultaFicha();
            }
            else
            {
                txtDataIni.Text = DateTime.Now.ToShortDateString();
                txtDataFim.Text = DateTime.Now.ToShortDateString();
            }
        }

    }


    private void ConsultaFicha()
    {
        SqlConnection mySelCadastro = new SqlConnection(Funcoes.conexao());
        mySelCadastro.Open();
        SqlCommand cmdSelCadastro = new SqlCommand("dbo.stp_eventos_ins", mySelCadastro);
        cmdSelCadastro.CommandType = CommandType.StoredProcedure;
        cmdSelCadastro.Parameters.Add("@flg_operacao", SqlDbType.Char).Value = "S";
        cmdSelCadastro.Parameters.Add("@COD_ID", SqlDbType.Int).Value = Funcoes.strToInt(sid_id.ToString());
        cmdSelCadastro.Parameters.Add("@COD_ID_PESSOA_LICENCIADO", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["LICENCIADO"].ToString());
        SqlDataReader ReaderCadastro = cmdSelCadastro.ExecuteReader();
        while (ReaderCadastro.Read())
        {
            lblID.Text = ReaderCadastro["COD_ID"].ToString();
            txtNomeEvento.Text = ReaderCadastro["NOM_EVENTO"].ToString();
            txtDescricao.Text = ReaderCadastro["DES_EVENTO"].ToString();
            txtDataIni.Text = ReaderCadastro["DTA_INICIO"].ToString();
            txtDataFim.Text = ReaderCadastro["DTA_TERMINO"].ToString();
            ddlAtivo.SelectedValue = ReaderCadastro["FLG_ATIVO"].ToString();

            ddlRealizacao.SelectedValue = ReaderCadastro["FLG_TIPO"].ToString();
            ConsultaRealizador();
            ddlRealizador.SelectedValue = ReaderCadastro["COD_ID_PESSOAS_FJ"].ToString();
        }

    }

    private void ConsultaRealizador()
    {
        ddlRealizador.DataTextField = "NOM_RAZAOSOCIAL";
        ddlRealizador.DataValueField = "COD_ID";
        ddlRealizador.DataSource = tabelas.tProprietario(ddlRealizacao.SelectedValue.ToString()).Tables["PESSOAS_FJ"].DefaultView;
        ddlRealizador.DataBind();
        ddlRealizador.Items.Insert(0, new ListItem("", "0"));
    }


    protected void btnSalvar_Click(object sender, EventArgs e)
    {
        SqlConnection connInsCons = new SqlConnection(Funcoes.conexao());
        connInsCons.Open();
        SqlCommand cmdInsCons = new SqlCommand("dbo.stp_eventos_ins", connInsCons);
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
        cmdInsCons.Parameters.Add("@DTA_DATA", SqlDbType.DateTime).Value = DateTime.Now;

        cmdInsCons.Parameters.Add("@NOM_EVENTO", SqlDbType.VarChar).Value = txtNomeEvento.Text.ToString();
        cmdInsCons.Parameters.Add("@DES_EVENTO", SqlDbType.VarChar).Value = txtDescricao.Text.ToString();
        cmdInsCons.Parameters.Add("@DTA_INICIO", SqlDbType.DateTime).Value = Convert.ToDateTime(txtDataIni.Text.ToString());
        cmdInsCons.Parameters.Add("@DTA_TERMINO", SqlDbType.DateTime).Value = Convert.ToDateTime(txtDataFim.Text.ToString());
        cmdInsCons.Parameters.Add("@FLG_ATIVO", SqlDbType.Char).Value = ddlAtivo.SelectedValue.ToString();

        cmdInsCons.Parameters.Add("@FLG_TIPO", SqlDbType.Char).Value = ddlRealizacao.SelectedValue.ToString();
        cmdInsCons.Parameters.Add("@COD_ID_PESSOAS_FJ", SqlDbType.Int).Value = Funcoes.strToInt(ddlRealizador.SelectedValue.ToString());

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

    protected void ddlRealizacao_SelectedIndexChanged(object sender, EventArgs e)
    {
        ConsultaRealizador();
    }
}