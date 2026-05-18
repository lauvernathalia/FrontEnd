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
public partial class cad_melhorias : System.Web.UI.Page
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
            if (Funcoes.strToInt(sid_id) != 0)
            {
                ConsultaCadastro();
                txtMelhoria.Enabled = false;
                btnSalvar.Visible = false;

            }
            else
            {
                txtData.Text = DateTime.Now.ToShortDateString();
                txtSolicitante.Text = HttpContext.Current.Session["NOME"].ToString();
                txtMelhoria.Enabled = true;
                btnSalvar.Visible = true;
            }
        }

    }

    private void ConsultaCadastro()
    {
        SqlConnection mySelCadastro = new SqlConnection(Funcoes.conexao());
        mySelCadastro.Open();
        SqlCommand cmdSelCadastro = new SqlCommand("dbo.stp_melhorias_ins", mySelCadastro);
        cmdSelCadastro.CommandType = CommandType.StoredProcedure;
        cmdSelCadastro.Parameters.Add("@flg_operacao", SqlDbType.Char).Value = "S";
        cmdSelCadastro.Parameters.Add("@COD_ID", SqlDbType.Int).Value = Funcoes.strToInt(sid_id.ToString());
        cmdSelCadastro.Parameters.Add("@COD_ID_PESSOA_LICENCIADO", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["LICENCIADO"].ToString());

        SqlDataReader ReaderCadastro = cmdSelCadastro.ExecuteReader();
        while (ReaderCadastro.Read())
        {
            txtCodigo.Text = ReaderCadastro["COD_ID"].ToString();
            txtSolicitante.Text = ReaderCadastro["NOM_RAZAOSOCIAL"].ToString();
            txtData.Text = ReaderCadastro["DTA_DATA"].ToString();
            txtProtocolo.Text = ReaderCadastro["NOM_PROTOCOLO"].ToString();
            txtMelhoria.Text = ReaderCadastro["NOM_MELHORIA"].ToString();
            txtDescricao.InnerText = ReaderCadastro["DES_MELHORIA"].ToString();
            ddlStatus.SelectedValue = ReaderCadastro["FLG_STATUS"].ToString();
        }

    }

    protected void btnSalvar_Click(object sender, EventArgs e)
    {

        SqlConnection connInsCons = new SqlConnection(Funcoes.conexao());
        connInsCons.Open();
        SqlCommand cmdInsCons = new SqlCommand("dbo.stp_melhorias_ins", connInsCons);
        cmdInsCons.CommandType = CommandType.StoredProcedure;
        string mensagem = "";
        if (Funcoes.strToInt(sid_id) != 0)
        {
            cmdInsCons.Parameters.Add("@flg_operacao", SqlDbType.Char).Value = 'A';
            cmdInsCons.Parameters.Add("@COD_ID", SqlDbType.Int).Value = Funcoes.strToInt(sid_id.ToString());
            mensagem = "Dados gravados com sucesso!";
        }
        else
        {
            cmdInsCons.Parameters.Add("@flg_operacao", SqlDbType.Char).Value = 'I';
            cmdInsCons.Parameters.Add("@COD_ID_PESSOAS_FJ", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["PESSOA"].ToString());

            int numeroDoCliente = Funcoes.strToInt(HttpContext.Current.Session["PESSOA"].ToString());
            string protocolo = ProtocoloGenerator.GerarProtocolo(numeroDoCliente,"MEL");

            cmdInsCons.Parameters.Add("@NOM_PROTOCOLO", SqlDbType.VarChar).Value = protocolo;
            mensagem = "Dados gravados com sucesso!  O número do seu protocolo é: " + protocolo;
        }

        cmdInsCons.Parameters.Add("@COD_ID_PESSOA_LICENCIADO", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["LICENCIADO"].ToString());
        cmdInsCons.Parameters.Add("@NOM_MELHORIA", SqlDbType.VarChar).Value = txtMelhoria.Text.ToString();
        cmdInsCons.Parameters.Add("@DES_MELHORIA", SqlDbType.Text).Value = txtDescricao.InnerText.ToString();

        cmdInsCons.ExecuteNonQuery();
        connInsCons.Close();
        connInsCons.Dispose();

        Funcoes.GravaAuditoria(Funcoes.strToInt(sid_id.ToString()), Request.ServerVariables["REMOTE_ADDR"].ToString(), "Cadastro - Melhorias - Inclusão", "ID: " + Funcoes.strToInt(sid_id.ToString()));


        ClientScript.RegisterStartupScript(this.GetType(),
            "Alerta", "alert('" + mensagem + "'); opener.PostBackOnMainPage(); window.close(); ", true);


    }
    protected void btnCancelar_Click(object sender, EventArgs e)
    {
        ClientScript.RegisterStartupScript(this.GetType(),
            "Alerta", "opener.PostBackOnMainPage(); window.close(); ", true);
    }

}