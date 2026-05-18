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

public partial class cad_bandeiras_adm : System.Web.UI.Page
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
        SqlCommand cmdSelCadastro = new SqlCommand("dbo.stp_bandeiras_ins", mySelCadastro);
        cmdSelCadastro.CommandType = CommandType.StoredProcedure;
        cmdSelCadastro.Parameters.Add("@flg_operacao", SqlDbType.Char).Value = "S";
        cmdSelCadastro.Parameters.Add("@COD_ID", SqlDbType.Int).Value = Funcoes.strToInt(sid_id.ToString());
        SqlDataReader ReaderCadastro = cmdSelCadastro.ExecuteReader();
        while (ReaderCadastro.Read())
        {
            lblID.Text = ReaderCadastro["COD_ID"].ToString();

            txtNome.Text = ReaderCadastro["NOM_BANDEIRA"].ToString();
            ddlTipo.SelectedValue = ReaderCadastro["FLG_TIPO"].ToString();
            ddlAtivo.SelectedValue = ReaderCadastro["FLG_ATIVO"].ToString();
            txtImagem.Text = ReaderCadastro["NOM_IMAGEM"].ToString();
            txtOrdem.Text = ReaderCadastro["NUM_ORDEM"].ToString();
            
        }

    }

    protected void btnSalvar_Click(object sender, EventArgs e)
    {
        string serverPath = System.Web.HttpContext.Current.Server.MapPath("~/");
        string StrFileName = flAnexo.PostedFile.FileName.Substring(flAnexo.PostedFile.FileName.LastIndexOf("\\") + 1);
        if (StrFileName.ToString().Trim() != "")
        {
            flAnexo.PostedFile.SaveAs(Server.MapPath("public_html") + "\\" + StrFileName.ToString());
        }

        SqlConnection connInsCons = new SqlConnection(Funcoes.conexao());
        connInsCons.Open();
        SqlCommand cmdInsCons = new SqlCommand("dbo.stp_bandeiras_ins", connInsCons);
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
        cmdInsCons.Parameters.Add("@NOM_BANDEIRA", SqlDbType.VarChar).Value = txtNome.Text.ToString();
        cmdInsCons.Parameters.Add("@FLG_TIPO", SqlDbType.Char).Value = ddlTipo.SelectedValue.ToString();
        cmdInsCons.Parameters.Add("@FLG_ATIVO", SqlDbType.Char).Value = ddlAtivo.SelectedValue.ToString();
        cmdInsCons.Parameters.Add("@NUM_ORDEM", SqlDbType.Int).Value = Funcoes.strToInt(txtOrdem.Text.ToString());

        if (StrFileName.ToString().Trim() == "")
        {
            cmdInsCons.Parameters.Add("@NOM_IMAGEM", SqlDbType.VarChar).Value = txtImagem.Text.ToString();
        }
        else
        {
            cmdInsCons.Parameters.Add("@NOM_IMAGEM", SqlDbType.VarChar).Value = StrFileName.ToString().Trim();
        }


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