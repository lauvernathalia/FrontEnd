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


public partial class cad_estabelecimentos_documentos : System.Web.UI.Page
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
        if (!IsPostBack)
        {

            SqlConnection myTipoDocumento = new SqlConnection(Funcoes.conexao());
            myTipoDocumento.Open();
            SqlCommand cmdTipoDocumento = new SqlCommand("dbo.stp_tipo_documento_ins", myTipoDocumento);
            cmdTipoDocumento.CommandType = CommandType.StoredProcedure;
            cmdTipoDocumento.Parameters.Add("@flg_operacao", SqlDbType.VarChar).Value = "C";
            SqlDataAdapter drTipoDocumento = new SqlDataAdapter();
            drTipoDocumento.SelectCommand = cmdTipoDocumento;
            DataSet dsTipoDocumento = new DataSet();
            drTipoDocumento.Fill(dsTipoDocumento, "TIPO_DOCUMENTO");
            ddlTipo.DataTextField = "NOM_TIPO_DOCUMENTO";
            ddlTipo.DataValueField = "NOM_TIPO_DOCUMENTO";
            ddlTipo.DataSource = dsTipoDocumento.Tables["TIPO_DOCUMENTO"].DefaultView;
            ddlTipo.DataBind();


            if (Funcoes.strToInt(sid_id) != 0)
            {
                ConsultaFicha();
                ConsultaGeral();
            }

        }
    }

    private void ConsultaFicha()
    {
        SqlConnection mySelCadastro = new SqlConnection(Funcoes.conexao());
        mySelCadastro.Open();
        SqlCommand cmdSelCadastro = new SqlCommand("dbo.stp_pessoas_fj_ins", mySelCadastro);
        cmdSelCadastro.CommandType = CommandType.StoredProcedure;
        cmdSelCadastro.Parameters.Add("@flg_operacao", SqlDbType.Char).Value = "S";
        cmdSelCadastro.Parameters.Add("@COD_ID", SqlDbType.Int).Value = Funcoes.strToInt(sid_id.ToString());
        cmdSelCadastro.Parameters.Add("@COD_ID_PESSOA_LICENCIADO", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["LICENCIADO"].ToString());

        SqlDataReader ReaderCadastro = cmdSelCadastro.ExecuteReader();
        while (ReaderCadastro.Read())
        {
            txtID.Text = ReaderCadastro["COD_ID"].ToString();
            txtRazaosocial.Text = ReaderCadastro["NOM_RAZAOSOCIAL"].ToString();
            txtEmail.Text = ReaderCadastro["NOM_EMAIL"].ToString();
            txtTipo.Text = ReaderCadastro["NOM_FLG_TIPO_PESSOA"].ToString();
            if (ReaderCadastro["FLG_TIPO_PESSOA"].ToString().Trim() == "PF")
            {
                txtDocumento.Text = ReaderCadastro["NOM_CPF"].ToString();
            }
            else
            {
                txtDocumento.Text = ReaderCadastro["NOM_CNPJ"].ToString();

            }

            txtRepresentante.Text = ReaderCadastro["NOM_RAZAOSOCIAL_REPRESENTANTE"].ToString();
            txtTipoRepresentante.Text = ReaderCadastro["FLG_TIPO_PESSOA_REPRESENTANTE"].ToString();
            txtDocumentoRepresentante.Text = ReaderCadastro["NOM_DOCUMENTO_REPRESENTANTE"].ToString();
        }

    }


    protected void btnFechar_Click(object sender, EventArgs e)
    {
        ClientScript.RegisterStartupScript(this.GetType(), "FecharOnboarding", "opener.PostBackOnMainPage(); window.close(); ", true);

    }

    private void ConsultaGeral()
    {
        SqlConnection myConsulta = new SqlConnection(Funcoes.conexao());
        myConsulta.Open();
        SqlDataAdapter SDAConsulta = new SqlDataAdapter("dbo.stp_pessoas_fj_documentos_ins", myConsulta);
        SDAConsulta.SelectCommand.CommandType = CommandType.StoredProcedure;
        SDAConsulta.SelectCommand.Parameters.Add("@flg_operacao", SqlDbType.Char).Value = "C";
        SDAConsulta.SelectCommand.Parameters.Add("@COD_ID_PESSOA_LICENCIADO", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["LICENCIADO"].ToString());
        SDAConsulta.SelectCommand.Parameters.Add("@COD_ID_PESSOAS_FJ", SqlDbType.Int).Value = Funcoes.strToInt(sid_id.ToString());
        DataSet dsConsulta = new DataSet();
        SDAConsulta.Fill(dsConsulta, "PESSOAS_FJ_DOCUMENTOS");
        rptConsulta.DataSource = dsConsulta.Tables["PESSOAS_FJ_DOCUMENTOS"].DefaultView;
        rptConsulta.DataBind();
        myConsulta.Close(); myConsulta.Dispose();
    }



    protected void btnIncluir_Click(object sender, EventArgs e)
    {

        string StrFileNameflArquivo = flArquivo.PostedFile.FileName.Substring(flArquivo.PostedFile.FileName.LastIndexOf("\\") + 1);
        string sExtensao = StrFileNameflArquivo.ToString().Substring(StrFileNameflArquivo.ToString().Length - 4, 4);

        if ((sExtensao.ToString().Trim() == ".jpg") || (sExtensao.ToString().Trim() == ".png") || (sExtensao.ToString().Trim() == ".bmp") || (sExtensao.ToString().Trim() == "jpeg") || (sExtensao.ToString().Trim() == ".pdf"))
        {
            string StrFileTypeflArquivo = flArquivo.PostedFile.ContentType;

            int IntFileSizeflArquivo = flArquivo.PostedFile.ContentLength;
            string NomeArquivoflArquivo = "";
            if (StrFileNameflArquivo.Trim() != "")
            {
                string CodificacaoflArquivo = DateTime.Now.Year.ToString() + DateTime.Now.Month.ToString() + DateTime.Now.Day.ToString() + DateTime.Now.Hour.ToString() + DateTime.Now.Minute.ToString() + DateTime.Now.Second.ToString();
                flArquivo.PostedFile.SaveAs(Server.MapPath("public_html") + "\\" + HttpContext.Current.Session["CODIGO"].ToString().Trim() + "_" + CodificacaoflArquivo.ToString() + "_" + StrFileNameflArquivo);
                NomeArquivoflArquivo = HttpContext.Current.Session["CODIGO"].ToString().Trim() + "_" + CodificacaoflArquivo.ToString() + "_" + StrFileNameflArquivo;
            }

            SqlConnection connInsCons = new SqlConnection(Funcoes.conexao());
            connInsCons.Open();
            SqlCommand cmdInsCons = new SqlCommand("dbo.stp_pessoas_fj_documentos_ins", connInsCons);
            cmdInsCons.CommandType = CommandType.StoredProcedure;
            cmdInsCons.Parameters.Add("@flg_operacao", SqlDbType.Char).Value = 'I';
            cmdInsCons.Parameters.Add("@COD_ID_PESSOA_LICENCIADO", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["LICENCIADO"].ToString());
            cmdInsCons.Parameters.Add("@COD_ID_PESSOAS_FJ", SqlDbType.Int).Value = Funcoes.strToInt(sid_id.ToString());
            cmdInsCons.Parameters.Add("@NOM_TIPO_DOCUMENTO", SqlDbType.VarChar).Value = ddlTipo.SelectedValue.ToString();
            cmdInsCons.Parameters.Add("@NOM_ARQUIVO", SqlDbType.VarChar).Value = NomeArquivoflArquivo.ToString();
            cmdInsCons.ExecuteNonQuery();
            connInsCons.Close();
            connInsCons.Dispose();

            ClientScript.RegisterStartupScript(this.GetType(), "SucessoDocumento", "alert('Dados gravados com sucesso'); ", true);
            ConsultaGeral();
        }
        else
        {
            ClientScript.RegisterStartupScript(this.GetType(), "ErroDocumento", "alert('Este tipo de arquivo não é aceito no cadastro de documentos do estabelecimento! Verifique e tente novamente.'); ", true);
        }
    }

    public static string TIRAACENTOS(string str)
    {
        str = str.Replace("-", "");
        str = str.Replace(".", "");
        str = str.Replace("/", "");
        str = str.Replace("(", "");
        str = str.Replace(")", "");
        str = str.Replace(" ", "");
        return str;
    }
    protected void rptConsulta_ItemCommand(object source, RepeaterCommandEventArgs e)
    {
        if (e.CommandName == "Excluir")
        {
            SqlConnection connInsCons = new SqlConnection(Funcoes.conexao());
            SqlCommand cmdInsCons = new SqlCommand("dbo.stp_pessoas_fj_documentos_ins", connInsCons);
            cmdInsCons.CommandType = CommandType.StoredProcedure;
            cmdInsCons.Parameters.Add("@flg_operacao", SqlDbType.Char).Value = 'E';
            cmdInsCons.Parameters.Add("@COD_ID", SqlDbType.Int).Value = Funcoes.strToInt(Convert.ToString(e.CommandArgument));
            cmdInsCons.Parameters.Add("@COD_ID_PESSOAS_FJ", SqlDbType.Int).Value = Funcoes.strToInt(sid_id.ToString());
            cmdInsCons.Parameters.Add("@COD_ID_PESSOA_LICENCIADO", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["LICENCIADO"].ToString());
            connInsCons.Open();
            cmdInsCons.ExecuteNonQuery();

            ClientScript.RegisterStartupScript(this.GetType(),"ExclusaoDocumentos", "alert('Registro excluído com sucesso');", true);

            ConsultaGeral();
        }

    }
}