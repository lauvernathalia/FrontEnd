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
using MessagingToolkit.QRCode.Codec;
using MessagingToolkit.QRCode.Codec.Data;


public partial class cad_submenus : System.Web.UI.Page
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

        // Verifica se o usuário esta autenticado
        if (HttpContext.Current.Session.Count <= 0)
        {
            Response.Redirect("login.aspx");
            FormsAuthentication.SignOut();
        }

        if (!IsPostBack)
        {
            // Carrega tabela de menus
            ddlTipo_SelectedIndexChanged(null, null);
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
        SqlCommand cmdSelCadastro = new SqlCommand("dbo.stp_submenu_ins", mySelCadastro);
        cmdSelCadastro.CommandType = CommandType.StoredProcedure;
        cmdSelCadastro.Parameters.Add("@flg_operacao", SqlDbType.Char).Value = "S";
        cmdSelCadastro.Parameters.Add("@COD_ID", SqlDbType.Int).Value = Funcoes.strToInt(sid_id.ToString());
        SqlDataReader ReaderCadastro = cmdSelCadastro.ExecuteReader();
        while (ReaderCadastro.Read())
        {
            txtCodigo.Text = ReaderCadastro["COD_ID"].ToString();

            txtSubmenu.Text = ReaderCadastro["NOM_SUBMENU"].ToString();
            txtOrdem.Text = ReaderCadastro["NUM_ORDEM"].ToString();
            ddlAtivo.SelectedValue = ReaderCadastro["FLG_ATIVO"].ToString();
            ddlTipo.SelectedValue = ReaderCadastro["FLG_TIPO"].ToString();
            
            txtAcao.Text = ReaderCadastro["NOM_ACAO"].ToString();
            txtIcone.Text = ReaderCadastro["NOM_ICONE"].ToString();

            ddlMenu.SelectedValue = ReaderCadastro["COD_ID_MENU"].ToString();
        }

    }

    protected void btnSalvar_Click(object sender, EventArgs e)
    {

        SqlConnection connInsCons = new SqlConnection(Funcoes.conexao());
        connInsCons.Open();
        SqlCommand cmdInsCons = new SqlCommand("dbo.stp_submenu_ins", connInsCons);
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
        cmdInsCons.Parameters.Add("@FLG_ATIVO", SqlDbType.Char).Value = ddlAtivo.SelectedValue.ToString();
        cmdInsCons.Parameters.Add("@FLG_TIPO", SqlDbType.Char).Value = ddlTipo.SelectedValue.ToString();
        cmdInsCons.Parameters.Add("@COD_ID_MENU", SqlDbType.Int).Value = Funcoes.strToInt(ddlMenu.SelectedValue.ToString());
        cmdInsCons.Parameters.Add("@NUM_ORDEM", SqlDbType.Int).Value = Funcoes.strToInt(txtOrdem.Text.ToString());
        cmdInsCons.Parameters.Add("@COD_ID_PESSOA_LICENCIADO", SqlDbType.Int).Value = 1;
        cmdInsCons.Parameters.Add("@NOM_SUBMENU", SqlDbType.VarChar).Value = txtSubmenu.Text.ToString();
        cmdInsCons.Parameters.Add("@NOM_ACAO", SqlDbType.VarChar).Value = txtAcao.Text.ToString();
        cmdInsCons.Parameters.Add("@NOM_ICONE", SqlDbType.VarChar).Value = txtIcone.Text.ToString();

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

    protected void ddlTipo_SelectedIndexChanged(object sender, EventArgs e)
    {
        // Carrega tabela de menus
        SqlConnection myMenu = new SqlConnection(Funcoes.conexao());
        myMenu.Open();
        SqlCommand cmdMenu = new SqlCommand("dbo.stp_menu_ins", myMenu);
        cmdMenu.CommandType = CommandType.StoredProcedure;
        cmdMenu.Parameters.Add("@flg_operacao", SqlDbType.VarChar).Value = "C";
        cmdMenu.Parameters.Add("@FLG_ATIVO", SqlDbType.Char).Value = "S";
        cmdMenu.Parameters.Add("@FLG_TIPO", SqlDbType.Char).Value = ddlTipo.SelectedValue.ToString();
        SqlDataAdapter drMenu = new SqlDataAdapter();
        drMenu.SelectCommand = cmdMenu;
        DataSet dsMenu = new DataSet();
        drMenu.Fill(dsMenu, "MENU");
        ddlMenu.DataTextField = "NOM_MENU";
        ddlMenu.DataValueField = "COD_ID";
        ddlMenu.DataSource = dsMenu.Tables["MENU"].DefaultView;
        ddlMenu.DataBind();
        ddlMenu.Items.Insert(0, new ListItem("", "0"));

    }
}