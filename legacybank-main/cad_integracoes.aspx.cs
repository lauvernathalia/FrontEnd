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

public partial class cad_integracoes : System.Web.UI.Page
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
                ConsultaFicha();
            }
        }

    }

    private void ConsultaFicha()
    {
        SqlConnection mySelCadastro = new SqlConnection(Funcoes.conexao());
        mySelCadastro.Open();
        SqlCommand cmdSelCadastro = new SqlCommand("dbo.stp_integracoes_ins", mySelCadastro);
        cmdSelCadastro.CommandType = CommandType.StoredProcedure;
        cmdSelCadastro.Parameters.Add("@flg_operacao", SqlDbType.Char).Value = "S";
        cmdSelCadastro.Parameters.Add("@COD_ID", SqlDbType.Int).Value = Funcoes.strToInt(sid_id.ToString());
        cmdSelCadastro.Parameters.Add("@COD_ID_PESSOA_LICENCIADO", SqlDbType.Int).Value = 0;
        SqlDataReader ReaderCadastro = cmdSelCadastro.ExecuteReader();
        while (ReaderCadastro.Read())
        {
            txtCodigo.Text = ReaderCadastro["COD_ID"].ToString();

            txtIntegracao.Text = ReaderCadastro["NOM_INTEGRACAO"].ToString();
            txtDescricao.InnerText = ReaderCadastro["DES_INTEGRACAO"].ToString();
            ddlAtivo.SelectedValue = ReaderCadastro["FLG_ATIVO"].ToString();
            txtURL.Text = ReaderCadastro["NOM_URL"].ToString();
            txtLogo.Text = ReaderCadastro["NOM_FOTO"].ToString();

            txtFantasia.Text = ReaderCadastro["NOM_FANTASIA_INTEGRACAO"].ToString();
            txtSigla.Text = ReaderCadastro["FLG_INTEGRACAO"].ToString();
            ddlTipo.SelectedValue = ReaderCadastro["FLG_TIPO_INTEGRACAO"].ToString();
        }

    }

    protected void btnSalvar_Click(object sender, EventArgs e)
    {

        string StrFileNameflLogo = flLogo.PostedFile.FileName.Substring(flLogo.PostedFile.FileName.LastIndexOf("\\") + 1);
        string StrFileTypeflLogo = flLogo.PostedFile.ContentType;
        int IntFileSizeflLogo = flLogo.PostedFile.ContentLength;
        string NomeArquivoflLogo = "";
        if (StrFileNameflLogo.Trim() != "")
        {
            string CodificacaoflLogo = DateTime.Now.Year.ToString() + DateTime.Now.Month.ToString() + DateTime.Now.Day.ToString() + DateTime.Now.Hour.ToString() + DateTime.Now.Minute.ToString() + DateTime.Now.Second.ToString();
            flLogo.PostedFile.SaveAs(Server.MapPath("public_html") + "\\" + HttpContext.Current.Session["CODIGO"].ToString().Trim() + "_" + CodificacaoflLogo.ToString() + "_" + StrFileNameflLogo);
            NomeArquivoflLogo = HttpContext.Current.Session["CODIGO"].ToString().Trim() + "_" + CodificacaoflLogo.ToString() + "_" + StrFileNameflLogo;
        }



        SqlConnection connInsCons = new SqlConnection(Funcoes.conexao());
        connInsCons.Open();
        SqlCommand cmdInsCons = new SqlCommand("dbo.stp_integracoes_ins", connInsCons);
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
        cmdInsCons.Parameters.Add("@COD_ID_PESSOA_LICENCIADO", SqlDbType.Int).Value = 0;

        // Produto
        cmdInsCons.Parameters.Add("@NOM_INTEGRACAO", SqlDbType.VarChar).Value = txtIntegracao.Text.ToString();
        cmdInsCons.Parameters.Add("@DES_INTEGRACAO", SqlDbType.Text).Value = txtDescricao.InnerText.ToString();
        cmdInsCons.Parameters.Add("@NOM_URL", SqlDbType.VarChar).Value = txtURL.Text.ToString();

        cmdInsCons.Parameters.Add("@NOM_FANTASIA_INTEGRACAO", SqlDbType.VarChar).Value = txtFantasia.Text.ToString();
        cmdInsCons.Parameters.Add("@FLG_INTEGRACAO", SqlDbType.Char).Value = txtSigla.Text.ToString();
        cmdInsCons.Parameters.Add("@FLG_TIPO_INTEGRACAO", SqlDbType.VarChar).Value = ddlTipo.SelectedValue.ToString();


        if (NomeArquivoflLogo.ToString().Trim() != "")
        {
            cmdInsCons.Parameters.Add("@NOM_FOTO", SqlDbType.VarChar).Value = NomeArquivoflLogo.ToString();
        }
        else
        {
            cmdInsCons.Parameters.Add("@NOM_FOTO", SqlDbType.VarChar).Value = txtLogo.Text.ToString();

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