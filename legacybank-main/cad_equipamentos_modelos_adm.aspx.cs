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

public partial class cad_equipamentos_modelos_adm : System.Web.UI.Page
{
    public static DataTable dtModelos;


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
            // Definição Tabela de Modelos
            dtModelos = new DataTable();
            dtModelos.Columns.Add("codigo", typeof(string));
            dtModelos.Columns.Add("nome", typeof(string));


            ConsultaIntegracoes();
            ConsultaGeral();
        }

    }

    private void ConsultaIntegracoes()
    {
        SqlConnection mySelCadastro = new SqlConnection(Funcoes.conexao());
        mySelCadastro.Open();
        SqlCommand cmdSelCadastro = new SqlCommand("dbo.stp_pessoas_fj_integracoes_ins", mySelCadastro);
        cmdSelCadastro.CommandType = CommandType.StoredProcedure;
        cmdSelCadastro.Parameters.Add("@flg_operacao", SqlDbType.Char).Value = "V";
        cmdSelCadastro.Parameters.Add("@COD_ID_PESSOA_LICENCIADO", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["LICENCIADO"].ToString());
        SqlDataReader ReaderCadastro = cmdSelCadastro.ExecuteReader();
        int iContador = 0;
        while (ReaderCadastro.Read())
        {
            if ((ReaderCadastro["NOM_INTEGRACAO"].ToString().ToUpper().Trim() == "CAPPTA") && (ReaderCadastro["FLG_ACESSO"].ToString().ToUpper() == "N")) { tbcapptatab.Visible = false; tbcappta.Visible = false; }
            if ((ReaderCadastro["NOM_INTEGRACAO"].ToString().ToUpper().Trim() == "ZOOP") && (ReaderCadastro["FLG_ACESSO"].ToString().ToUpper() == "N")) { tbzooptab.Visible = false; tbzoop.Visible = false; }
            if ((ReaderCadastro["NOM_INTEGRACAO"].ToString().ToUpper().Trim() == "ASAAS") && (ReaderCadastro["FLG_ACESSO"].ToString().ToUpper() == "N")) { tbasaastab.Visible = false; tbasaas.Visible = false; }
            if ((ReaderCadastro["NOM_INTEGRACAO"].ToString().ToUpper().Trim() == "PAGSEGURO") && (ReaderCadastro["FLG_ACESSO"].ToString().ToUpper() == "N")) { tbpagsegurotab.Visible = false; tbpagseguro.Visible = false; }
        }

        if ((tbcapptatab.Visible == true) && (iContador == 0)) { tbcapptatab.Attributes.Add("class", "nav-link active"); tbcappta.Attributes.Add("class", "tab-pane fade show active"); iContador = iContador + 1; }
        if ((tbzooptab.Visible == true) && (iContador == 0)) { tbzooptab.Attributes.Add("class", "nav-link active"); tbzoop.Attributes.Add("class", "tab-pane fade show active"); iContador = iContador + 1; }
        if ((tbasaastab.Visible == true) && (iContador == 0)) { tbasaastab.Attributes.Add("class", "nav-link active"); tbasaas.Attributes.Add("class", "tab-pane fade show active"); iContador = iContador + 1; }
        if ((tbpagsegurotab.Visible == true) && (iContador == 0)) { tbpagsegurotab.Attributes.Add("nav-link class", "active"); tbpagseguro.Attributes.Add("class", "tab-pane fade show active"); iContador = iContador + 1; }

    }

    private void ConsultaGeral()
    {
        //dtgConsulta.Visible = true;
        SqlConnection myConsulta = new SqlConnection(Funcoes.conexao());
        myConsulta.Open();
        SqlDataAdapter SDAConsulta = new SqlDataAdapter("dbo.stp_equipamentos_modelos_ins", myConsulta);
        SDAConsulta.SelectCommand.CommandType = CommandType.StoredProcedure;
        SDAConsulta.SelectCommand.Parameters.Add("@flg_operacao", SqlDbType.Char).Value = "C";
        SDAConsulta.SelectCommand.Parameters.Add("@COD_ID_PESSOA_LICENCIADO", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["LICENCIADO"].ToString());

        DataSet dsConsulta = new DataSet();
        SDAConsulta.Fill(dsConsulta, "EQUIPAMENTOS_MODELOS");
        rptConsulta.DataSource = dsConsulta.Tables["EQUIPAMENTOS_MODELOS"].DefaultView;
        rptConsulta.DataBind();
        myConsulta.Close(); myConsulta.Dispose();

    }

    protected void btnSalvar_Click(object sender, EventArgs e)
    {
        // Upload imagem/foto do modelo

        string StrFileNameflFoto = flFoto.PostedFile.FileName.Substring(flFoto.PostedFile.FileName.LastIndexOf("\\") + 1);
        string StrFileTypeflFoto = flFoto.PostedFile.ContentType;
        int IntFileSizeflFoto = flFoto.PostedFile.ContentLength;
        string NomeArquivoflFoto = "";
        if (StrFileNameflFoto.Trim() != "")
        {
            string CodificacaoflFoto = DateTime.Now.Year.ToString() + DateTime.Now.Month.ToString() + DateTime.Now.Day.ToString() + DateTime.Now.Hour.ToString() + DateTime.Now.Minute.ToString() + DateTime.Now.Second.ToString();
            flFoto.PostedFile.SaveAs(Server.MapPath("public_html") + "\\" + HttpContext.Current.Session["CODIGO"].ToString().Trim() + "_" + CodificacaoflFoto.ToString() + "_" + StrFileNameflFoto);
            NomeArquivoflFoto = HttpContext.Current.Session["CODIGO"].ToString().Trim() + "_" + CodificacaoflFoto.ToString() + "_" + StrFileNameflFoto;
        }



        SqlConnection connInsCons = new SqlConnection(Funcoes.conexao());
        connInsCons.Open();
        SqlCommand cmdInsCons = new SqlCommand("dbo.stp_equipamentos_modelos_ins", connInsCons);
        cmdInsCons.CommandType = CommandType.StoredProcedure;
        cmdInsCons.Parameters.Add("@flg_operacao", SqlDbType.Char).Value = 'I';
        cmdInsCons.Parameters.Add("@COD_ID_PESSOA_LICENCIADO", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["LICENCIADO"].ToString());
        cmdInsCons.Parameters.Add("@FLG_ATIVO", SqlDbType.Char).Value = ddlAtivo.SelectedValue.ToString();
        cmdInsCons.Parameters.Add("@NOM_MODELO", SqlDbType.VarChar).Value = txtNome.Text.ToString();
        cmdInsCons.Parameters.Add("@COD_ID_MODELO", SqlDbType.VarChar).Value = txtCodigo.Text.ToString();
        cmdInsCons.Parameters.Add("@DES_MODELO", SqlDbType.Text).Value = txtDescricao.Text.ToString();

        if (NomeArquivoflFoto.ToString().Trim() != "")
        {
            cmdInsCons.Parameters.Add("@NOM_FOTO", SqlDbType.VarChar).Value = NomeArquivoflFoto.ToString();
        }
        else
        {
            cmdInsCons.Parameters.Add("@NOM_FOTO", SqlDbType.VarChar).Value = txtFoto.Text.ToString();

        }


        cmdInsCons.ExecuteNonQuery();
        connInsCons.Close();
        connInsCons.Dispose();

        ClientScript.RegisterStartupScript(this.GetType(),
            "Alerta", "alert('Dados gravados com sucesso');", true);

        txtNome.Text = "";
        txtDescricao.Text = "";

        ConsultaGeral();
    }
    protected void btnCancelar_Click(object sender, EventArgs e)
    {
        ClientScript.RegisterStartupScript(this.GetType(),
            "Alerta", "opener.PostBackOnMainPage(); window.close(); ", true);
    }

    protected void rptConsulta_OnItemCommand(object source, RepeaterCommandEventArgs e)
    {
        if (e.CommandName == "Ativo")
        {

            SqlConnection connInsCons = new SqlConnection(Funcoes.conexao());
            connInsCons.Open();
            SqlCommand cmdInsCons = new SqlCommand("dbo.stp_equipamentos_modelos_ins", connInsCons);
            cmdInsCons.CommandType = CommandType.StoredProcedure;
            cmdInsCons.Parameters.Add("@flg_operacao", SqlDbType.Char).Value = "U";
            cmdInsCons.Parameters.Add("@COD_ID", SqlDbType.Int).Value = Funcoes.strToInt(Convert.ToString(e.CommandArgument));
            cmdInsCons.Parameters.Add("@COD_ID_PESSOA_LICENCIADO", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["LICENCIADO"].ToString());
            cmdInsCons.ExecuteNonQuery();
            connInsCons.Close();
            connInsCons.Dispose();

            ClientScript.RegisterStartupScript(this.GetType(),
                "Alerta", "alert('Modelo Ativado/Inativado com sucesso!');", true);
        }
        if (e.CommandName == "Excluir")
        {

            SqlConnection connInsCons = new SqlConnection(Funcoes.conexao());
            connInsCons.Open();
            SqlCommand cmdInsCons = new SqlCommand("dbo.stp_equipamentos_modelos_ins", connInsCons);
            cmdInsCons.CommandType = CommandType.StoredProcedure;
            cmdInsCons.Parameters.Add("@flg_operacao", SqlDbType.Char).Value = "E";
            cmdInsCons.Parameters.Add("@COD_ID", SqlDbType.Int).Value = Funcoes.strToInt(Convert.ToString(e.CommandArgument));
            cmdInsCons.Parameters.Add("@COD_ID_PESSOA_LICENCIADO", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["LICENCIADO"].ToString());

            cmdInsCons.ExecuteNonQuery();
            connInsCons.Close();
            connInsCons.Dispose();

            ClientScript.RegisterStartupScript(this.GetType(),
                "Alerta", "alert('Modelo Excluído com sucesso!');", true);
        }

        if (e.CommandName == "Atualizar")
        {
            foreach (RepeaterItem item in rptConsulta.Items)
            {
                if (Funcoes.strToInt(Convert.ToString(e.CommandArgument)) == Funcoes.strToInt(((TextBox)item.FindControl("txtID")).Text.ToString()))
                {

                    // Upload imagem/foto do modelo

                    string StrFileNameflFoto = ((HtmlInputFile)item.FindControl("flFoto")).PostedFile.FileName.Substring(((HtmlInputFile)item.FindControl("flFoto")).PostedFile.FileName.LastIndexOf("\\") + 1);
                    string StrFileTypeflFoto = ((HtmlInputFile)item.FindControl("flFoto")).PostedFile.ContentType;
                    int IntFileSizeflFoto = ((HtmlInputFile)item.FindControl("flFoto")).PostedFile.ContentLength;
                    string NomeArquivoflFoto = "";
                    if (StrFileNameflFoto.Trim() != "")
                    {
                        string CodificacaoflFoto = DateTime.Now.Year.ToString() + DateTime.Now.Month.ToString() + DateTime.Now.Day.ToString() + DateTime.Now.Hour.ToString() + DateTime.Now.Minute.ToString() + DateTime.Now.Second.ToString();
                        ((HtmlInputFile)item.FindControl("flFoto")).PostedFile.SaveAs(Server.MapPath("public_html") + "\\" + HttpContext.Current.Session["CODIGO"].ToString().Trim() + "_" + CodificacaoflFoto.ToString() + "_" + StrFileNameflFoto);
                        NomeArquivoflFoto = HttpContext.Current.Session["CODIGO"].ToString().Trim() + "_" + CodificacaoflFoto.ToString() + "_" + StrFileNameflFoto;
                    }
                    

                    SqlConnection connInsCons = new SqlConnection(Funcoes.conexao());
                    connInsCons.Open();
                    SqlCommand cmdInsCons = new SqlCommand("dbo.stp_equipamentos_modelos_ins", connInsCons);
                    cmdInsCons.CommandType = CommandType.StoredProcedure;
                    cmdInsCons.Parameters.Add("@flg_operacao", SqlDbType.Char).Value = "A";
                    cmdInsCons.Parameters.Add("@COD_ID", SqlDbType.Int).Value = Funcoes.strToInt(Convert.ToString(e.CommandArgument));
                    cmdInsCons.Parameters.Add("@COD_ID_PESSOA_LICENCIADO", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["LICENCIADO"].ToString());
                    cmdInsCons.Parameters.Add("@COD_ID_MODELO", SqlDbType.VarChar).Value = ((TextBox)item.FindControl("txtCodigo")).Text.ToString();
                    cmdInsCons.Parameters.Add("@NOM_MODELO", SqlDbType.VarChar).Value = ((TextBox)item.FindControl("txtNomeModelo")).Text.ToString();
                    cmdInsCons.Parameters.Add("@DES_MODELO", SqlDbType.Text).Value = ((TextBox)item.FindControl("txtDescricaoModelo")).Text.ToString();

                    if (NomeArquivoflFoto.ToString().Trim() != "")
                    {
                        cmdInsCons.Parameters.Add("@NOM_FOTO", SqlDbType.VarChar).Value = NomeArquivoflFoto.ToString();
                    }
                    else
                    {
                        cmdInsCons.Parameters.Add("@NOM_FOTO", SqlDbType.VarChar).Value = ((TextBox)item.FindControl("txtFoto")).Text.ToString();
                    }

                    cmdInsCons.ExecuteNonQuery();
                    connInsCons.Close();
                    connInsCons.Dispose();

                    ClientScript.RegisterStartupScript(this.GetType(),
                        "Alerta", "alert('Modelo Atualizado com sucesso!');", true);
                }
            }
        }


        ConsultaGeral();

    }
    protected void btnCarregarCadastrosCappta_Click(object sender, EventArgs e)
    {
            dtModelos.Rows.Clear();

            string jsonModelos = "";
            jsonModelos = hubcappta.ListarOpcoesPOS();
            txtJsonCappta.Text = jsonModelos;

            JObject oModelos = JObject.Parse(jsonModelos);
            //JObject oRevendedores = JObject.Parse(jsonRevendedores);


            if (oModelos["modelId"].Count() > 0)
            {
                for (int i = 0; i < oModelos["modelId"].Count(); i++)
                {
                    dtModelos.Rows.Add(oModelos["modelId"][i]["id"].ToString().Trim(), oModelos["modelId"][i]["name"].ToString().Trim() );
                }
            }

            this.lsvModelos.DataSource = dtModelos;
            this.lsvModelos.DataBind();

            ClientScript.RegisterStartupScript(this.GetType(),
"SucessoCarregarDados", "alert('Dados dos Modelos de POS carregados com sucesso');", true);


    }
    protected void btnImportarCappta_Click(object sender, EventArgs e)
    {
        foreach (ListViewItem itemP in lsvModelos.Items)
        {
            if (((CheckBox)itemP.FindControl("ckbImportar")).Checked == true)
            {
                GravarDadosModelos(((TextBox)itemP.FindControl("txtCodigo")).Text.ToString(), ((TextBox)itemP.FindControl("txtNome")).Text.ToString());
            }
        }

        ClientScript.RegisterStartupScript(this.GetType(),
"SucessoImportarDados", "alert('Dados dos Modelos de POS importados com sucesso');", true);


    }
    protected void ckbTodos_CheckedChanged(object sender, EventArgs e)
    {
        foreach (ListViewItem itemP in lsvModelos.Items)
        {
            ((CheckBox)itemP.FindControl("ckbImportar")).Checked = (((CheckBox)itemP.FindControl("ckbImportar")).Checked == false) ? true : false;
        }

    }
    protected void lsvModelos_ItemCommand(object sender, ListViewCommandEventArgs e)
    {

    }
    protected void btnCarregarCadastrosAsaas_Click(object sender, EventArgs e)
    {

    }
    protected void btnImportarAsaas_Click(object sender, EventArgs e)
    {

    }
    protected void btnCarregarCadastrosPagseguro_Click(object sender, EventArgs e)
    {

    }
    protected void btnImportarPagseguro_Click(object sender, EventArgs e)
    {

    }
    protected void btnCarregarCadastrosZoop_Click(object sender, EventArgs e)
    {

    }
    protected void btnImportarZoop_Click(object sender, EventArgs e)
    {

    }

    private void GravarDadosModelos(string sCodigo, string sNome)
    {
        SqlConnection connInsCons = new SqlConnection(Funcoes.conexao());
        connInsCons.Open();
        SqlCommand cmdInsCons = new SqlCommand("dbo.stp_equipamentos_modelos_ins", connInsCons);
        cmdInsCons.CommandType = CommandType.StoredProcedure;
        cmdInsCons.Parameters.Add("@flg_operacao", SqlDbType.Char).Value = 'I';
        cmdInsCons.Parameters.Add("@COD_ID_PESSOA_LICENCIADO", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["LICENCIADO"].ToString());
        cmdInsCons.Parameters.Add("@FLG_ATIVO", SqlDbType.Char).Value = "S";
        cmdInsCons.Parameters.Add("@COD_ID_MODELO", SqlDbType.VarChar).Value = sCodigo.ToString();
        cmdInsCons.Parameters.Add("@NOM_MODELO", SqlDbType.VarChar).Value = sNome.ToString();
        cmdInsCons.Parameters.Add("@DES_MODELO", SqlDbType.Text).Value = sNome.ToString();
        cmdInsCons.ExecuteNonQuery();
        connInsCons.Close();
        connInsCons.Dispose();

    }
}