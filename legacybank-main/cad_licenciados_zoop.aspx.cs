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

using System.Drawing;
using System.Drawing.Printing;
using System.Diagnostics;


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
using System.Security;
using System.Net.Mail;
using System.IO.IsolatedStorage;


public partial class cad_licenciados_zoop : System.Web.UI.Page
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
            ConsultaFicha();
        }

    }

    private void ConsultaFicha()
    {
        SqlConnection mySelCadastro = new SqlConnection(Funcoes.conexao());
        mySelCadastro.Open();
        SqlCommand cmdSelCadastro = new SqlCommand("dbo.stp_pessoas_fj_licenciados_ins", mySelCadastro);
        cmdSelCadastro.CommandType = CommandType.StoredProcedure;
        cmdSelCadastro.Parameters.Add("@flg_operacao", SqlDbType.Char).Value = "S";
        cmdSelCadastro.Parameters.Add("@COD_ID", SqlDbType.Int).Value = Funcoes.strToInt(sid_id.ToString());
        //cmdSelCadastro.Parameters.Add("@COD_ID_PESSOA_LICENCIADO", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["LICENCIADO"].ToString());
        SqlDataReader ReaderCadastro = cmdSelCadastro.ExecuteReader();
        while (ReaderCadastro.Read())
        {
            txtID.Text = ReaderCadastro["COD_ID"].ToString();
            txtCodigo.Text = ReaderCadastro["COD_ID_PESSOA_LICENCIADO"].ToString();
            txtRazaosocial.Text = ReaderCadastro["NOM_RAZAOSOCIAL"].ToString();

            txtChave.Text = ReaderCadastro["NOM_KEY_MARKETPLACE"].ToString();
            txtToken.Text = ReaderCadastro["NOM_ID_MARKETPLACE"].ToString();
            ckbZoop.Checked = (ReaderCadastro["FLG_INTEGRACAO_ZOOP"].ToString() == "S") ? true : false;
            ckbTerceiros.Checked = (ReaderCadastro["FLG_TERCEIROS_ZOOP"].ToString() == "S") ? true : false;

        }

    }

    protected void btnCancelar_Click(object sender, EventArgs e)
    {
        ClientScript.RegisterStartupScript(this.GetType(),
            "Alerta", "opener.PostBackOnMainPage(); window.close(); ", true);
    }

    protected void btnSalvar_Click(object sender, EventArgs e)
    {

        SqlConnection connInsCons = new SqlConnection(Funcoes.conexao());
        connInsCons.Open();
        SqlCommand cmdInsCons = new SqlCommand("dbo.stp_pessoas_fj_licenciados_ins", connInsCons);
        cmdInsCons.CommandType = CommandType.StoredProcedure;
        cmdInsCons.Parameters.Add("@flg_operacao", SqlDbType.Char).Value = 'Z';
        cmdInsCons.Parameters.Add("@COD_ID", SqlDbType.Int).Value = Funcoes.strToInt(sid_id.ToString());
        cmdInsCons.Parameters.Add("@NOM_KEY_MARKETPLACE", SqlDbType.VarChar).Value = txtChave.Text.ToString();
        cmdInsCons.Parameters.Add("@NOM_ID_MARKETPLACE", SqlDbType.VarChar).Value = txtToken.Text.ToString();
        cmdInsCons.Parameters.Add("@FLG_INTEGRACAO_ZOOP", SqlDbType.Char).Value = (ckbZoop.Checked == true) ? "S" : "N";
        cmdInsCons.Parameters.Add("@FLG_TERCEIROS_ZOOP", SqlDbType.Char).Value = (ckbTerceiros.Checked == true) ? "S" : "N";

        cmdInsCons.ExecuteNonQuery();
        connInsCons.Close();
        connInsCons.Dispose();

        ClientScript.RegisterStartupScript(this.GetType(),
            "Alerta", "alert('Dados gravados com sucesso'); opener.PostBackOnMainPage(); window.close();", true);


    }
}