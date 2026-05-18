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

public partial class cad_habilitar_terminal : System.Web.UI.Page
{
    public string sid_id
    {
        get
        {
            try { return Funcoes.Decrypt(Request["id"]); }
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
            SqlConnection myModelos = new SqlConnection(Funcoes.conexao());
            myModelos.Open();
            SqlCommand cmdModelos = new SqlCommand("dbo.stp_equipamentos_modelos_ins", myModelos);
            cmdModelos.CommandType = CommandType.StoredProcedure;
            cmdModelos.Parameters.Add("@flg_operacao", SqlDbType.VarChar).Value = "C";
            cmdModelos.Parameters.Add("@COD_ID_PESSOA_LICENCIADO", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["LICENCIADO"].ToString());
            SqlDataAdapter drModelos = new SqlDataAdapter();
            drModelos.SelectCommand = cmdModelos;
            DataSet dsModelos = new DataSet();
            drModelos.Fill(dsModelos, "EQUIPAMENTOS_MODELOS");
            ddlModelo.DataTextField = "NOM_MODELO";
            ddlModelo.DataValueField = "COD_ID";
            ddlModelo.DataSource = dsModelos.Tables["EQUIPAMENTOS_MODELOS"].DefaultView;
            ddlModelo.DataBind();
            myModelos.Close();
            myModelos.Dispose();

            ddlModelo_SelectedIndexChanged(null, null);


            SqlConnection myEstabelecimentos = new SqlConnection(Funcoes.conexao());
            myEstabelecimentos.Open();
            SqlCommand cmdEstabelecimentos = new SqlCommand("dbo.stp_pessoas_fj_ins", myEstabelecimentos);
            cmdEstabelecimentos.CommandType = CommandType.StoredProcedure;
            cmdEstabelecimentos.Parameters.Add("@flg_operacao", SqlDbType.VarChar).Value = "Y";

            cmdEstabelecimentos.Parameters.Add("@COD_ID_PESSOA_LICENCIADO", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["LICENCIADO"].ToString());
            cmdEstabelecimentos.Parameters.Add("@NOM_RAZAOSOCIAL", SqlDbType.VarChar).Value = " ";
            cmdEstabelecimentos.Parameters.Add("@NUM_DOCUMENTO", SqlDbType.VarChar).Value = " ";
            cmdEstabelecimentos.Parameters.Add("@NUM_TOKEN", SqlDbType.VarChar).Value = " ";
            cmdEstabelecimentos.Parameters.Add("@FLG_ATIVO", SqlDbType.Char).Value = " ";
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
            ddlEstabelecimento.DataTextField = "NOM_RAZAOSOCIAL";
            ddlEstabelecimento.DataValueField = "COD_ID";
            ddlEstabelecimento.DataSource = dsEstabelecimentos.Tables["PESSOAS_FJ"].DefaultView;
            ddlEstabelecimento.DataBind();
            ddlEstabelecimento.Items.Insert(0, new ListItem("Todos", ""));
            myEstabelecimentos.Close();
            myEstabelecimentos.Dispose();

            if (HttpContext.Current.Session["TIPO"].ToString() == "E")
            {
                ddlEstabelecimento.SelectedValue = HttpContext.Current.Session["PESSOA"].ToString();
                ddlEstabelecimento.Enabled = false;
            }

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
        SqlCommand cmdSelCadastro = new SqlCommand("dbo.stp_habilitar_terminais_ins", mySelCadastro);
        cmdSelCadastro.CommandType = CommandType.StoredProcedure;
        cmdSelCadastro.Parameters.Add("@flg_operacao", SqlDbType.Char).Value = "S";
        cmdSelCadastro.Parameters.Add("@COD_ID", SqlDbType.Int).Value = Funcoes.strToInt(sid_id.ToString());
        cmdSelCadastro.Parameters.Add("@COD_ID_PESSOA_LICENCIADO", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["LICENCIADO"].ToString());
        SqlDataReader ReaderCadastro = cmdSelCadastro.ExecuteReader();
        while (ReaderCadastro.Read())
        {
            lblID.Text = ReaderCadastro["COD_ID"].ToString();

            txtSerial.Text = ReaderCadastro["NUM_SERIAL"].ToString();
            txtToken.Text = ReaderCadastro["NUM_TOKEN"].ToString();
            ddlEstabelecimento.SelectedValue = ReaderCadastro["COD_ID_PESSOAS_FJ"].ToString();
        }

    }


    protected void btnSalvar_Click(object sender, EventArgs e)
    {
        dadosParearTerminalZoop.root dterminal = new dadosParearTerminalZoop.root();
        
        if (HttpContext.Current.Session["TIPO"].ToString() == "E")
        {
            dterminal.seller = HttpContext.Current.Session["TOKENZOOP"].ToString();
        }
        else
        {
            SqlConnection connVerificaUsuario = new SqlConnection(Funcoes.conexao());
            SqlCommand cmdVerificaUsuario = new SqlCommand("dbo.stp_pessoas_fj_ins", connVerificaUsuario);
            cmdVerificaUsuario.CommandType = CommandType.StoredProcedure;
            cmdVerificaUsuario.Parameters.Add("@flg_operacao", SqlDbType.Char).Value = "S";
            cmdVerificaUsuario.Parameters.Add("@COD_ID", SqlDbType.Int).Value = Funcoes.strToInt(ddlEstabelecimento.SelectedValue.ToString());
            cmdVerificaUsuario.Parameters.Add("@COD_ID_PESSOA_LICENCIADO", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["LICENCIADO"].ToString());
            connVerificaUsuario.Open();
            SqlDataReader mReader = cmdVerificaUsuario.ExecuteReader();
            if (mReader.Read())
            {
                dterminal.seller = mReader["NUM_TOKEN"].ToString();
            }
            connVerificaUsuario.Close();
            connVerificaUsuario.Dispose();
        }
        dterminal.marketplace_id = true;
        dterminal.token = txtToken.Text.ToString();
        dterminal.isStaging = false;

        string jsonTerminal = JsonConvert.SerializeObject(dterminal);
        string resultado = zoop.ParearTerminal(jsonTerminal);


        
        ClientScript.RegisterStartupScript(this.GetType(), "ResultadoParear", "alert('"+resultado+"');", true);

        // Grava os dados
        
        SqlConnection connInsCons = new SqlConnection(Funcoes.conexao());
        connInsCons.Open();
        SqlCommand cmdInsCons = new SqlCommand("dbo.stp_habilitar_terminais_ins", connInsCons);
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
        cmdInsCons.Parameters.Add("@COD_ID_PESSOA_LICENCIADO", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["LICENCIADO"].ToString());
        cmdInsCons.Parameters.Add("@COD_ID_PESSOAS_FJ", SqlDbType.Int).Value = Funcoes.strToInt(ddlEstabelecimento.SelectedValue.ToString());
        cmdInsCons.Parameters.Add("@DTA_DATA", SqlDbType.DateTime).Value = DateTime.Now;
        cmdInsCons.Parameters.Add("@NUM_SERIAL", SqlDbType.VarChar).Value = txtSerial.Text.ToString();
        cmdInsCons.Parameters.Add("@NUM_TOKEN", SqlDbType.VarChar).Value = txtToken.Text.ToString();
        cmdInsCons.Parameters.Add("@DES_JSON", SqlDbType.Text).Value = resultado.ToString();

        cmdInsCons.ExecuteNonQuery();
        connInsCons.Close();
        connInsCons.Dispose();

        ClientScript.RegisterStartupScript(this.GetType(),"Alerta", "alert('Dados gravados com sucesso');", true);
        
    }

    protected void btnCancelar_Click(object sender, EventArgs e)
    {
        ClientScript.RegisterStartupScript(this.GetType(), "FecharHabilitarTerminal", "opener.PostBackOnMainPage(); window.close(); ", true);
    }

    protected void ddlModelo_SelectedIndexChanged(object sender, EventArgs e)
    {
        SqlConnection mySelModelo = new SqlConnection(Funcoes.conexao());
        mySelModelo.Open();
        SqlCommand cmdSelModelo = new SqlCommand("dbo.stp_equipamentos_modelos_ins", mySelModelo);
        cmdSelModelo.CommandType = CommandType.StoredProcedure;
        cmdSelModelo.Parameters.Add("@flg_operacao", SqlDbType.Char).Value = "S";
        cmdSelModelo.Parameters.Add("@COD_ID", SqlDbType.Int).Value = Funcoes.strToInt(ddlModelo.SelectedValue.ToString());
        cmdSelModelo.Parameters.Add("@COD_ID_PESSOA_LICENCIADO", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["LICENCIADO"].ToString());
        SqlDataReader ReaderModelo = cmdSelModelo.ExecuteReader();
        while (ReaderModelo.Read())
        {
            imgModelo.Src = ReaderModelo["URL_NOM_FOTO"].ToString();
            lblModelo.Text = ReaderModelo["NOM_MODELO"].ToString();
        }

    }
}