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

public partial class cad_representantes_importar : System.Web.UI.Page
{
    public static DataTable dtRepresentantes;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            // Definição Tabela de Representantes
            dtRepresentantes = new DataTable();
            dtRepresentantes.Columns.Add("documento", typeof(string));
            dtRepresentantes.Columns.Add("nome", typeof(string));
            dtRepresentantes.Columns.Add("status", typeof(string));

            // Verificar Integrações
            CarregaAdquirentes();
            ddlAdquirentes_SelectedIndexChanged(null, null);

        }

    }

    private void CarregaAdquirentes()
    {
        SqlConnection myAdquirentes = new SqlConnection(Funcoes.conexao());
        myAdquirentes.Open();
        SqlCommand cmdAdquirentes = new SqlCommand("dbo.stp_pessoas_fj_integracoes_chaves_ins", myAdquirentes);
        cmdAdquirentes.CommandType = CommandType.StoredProcedure;
        cmdAdquirentes.Parameters.Add("@flg_operacao", SqlDbType.Char).Value = "F";
        cmdAdquirentes.Parameters.Add("@COD_ID_PESSOA_LICENCIADO", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["LICENCIADO"].ToString());
        cmdAdquirentes.Parameters.Add("@FLG_ATIVO", SqlDbType.Char).Value = "S";

        SqlDataAdapter drAdquirentes = new SqlDataAdapter();
        drAdquirentes.SelectCommand = cmdAdquirentes;
        DataSet dsAdquirentes = new DataSet();
        drAdquirentes.Fill(dsAdquirentes, "PESSOAS_FJ_INTEGRACOES");

        ddlAdquirentes.DataTextField = "NOM_FANTASIA_INTEGRACAO";
        ddlAdquirentes.DataValueField = "FLG_INTEGRACAO";
        ddlAdquirentes.DataSource = dsAdquirentes.Tables["PESSOAS_FJ_INTEGRACOES"].DefaultView;
        ddlAdquirentes.DataBind();
    }

    protected void ddlAdquirentes_SelectedIndexChanged(object sender, EventArgs e)
    {
        divAdquirenteCappta.Visible = false;
        divAdquirenteZoop.Visible = false;
        divAdquirenteAsaas.Visible = false;
        divAdquirentePagseguro.Visible = false;
        divAdquirenteErp.Visible = false;

        if (ddlAdquirentes.SelectedValue.ToString() == "C")
        {
            divAdquirenteCappta.Visible = true;
        }
        if (ddlAdquirentes.SelectedValue.ToString() == "Z")
        {
            divAdquirenteZoop.Visible = true;
        }
        if (ddlAdquirentes.SelectedValue.ToString() == "P")
        {
            divAdquirentePagseguro.Visible = true;
        }
        if (ddlAdquirentes.SelectedValue.ToString() == "A")
        {
            divAdquirenteAsaas.Visible = true;
        }

        if (ddlAdquirentes.SelectedValue.ToString() == "E")
        {
            divAdquirenteErp.Visible = true;
        }
    }


    protected void btnCarregarCadastrosCappta_Click(object sender, EventArgs e)
    {
        try
        {
            string jsonRevendedores = "";
            jsonRevendedores = hubcappta.ConsultarRevendedores();

            JArray oRevendedores = JArray.Parse(jsonRevendedores);
            //JObject oRevendedores = JObject.Parse(jsonRevendedores);

            if (oRevendedores.Count > 0)
            {
                for (int i = 0; i < oRevendedores.Count; i++)
                {
                    dtRepresentantes.Rows.Add(oRevendedores[i]["reseller"]["document"].ToString(), oRevendedores[i]["reseller"]["companyName"].ToString(), oRevendedores[i]["statusDescription"].ToString());
                }
            }

            this.lsvMarketplacesCappta.DataSource = dtRepresentantes;
            this.lsvMarketplacesCappta.DataBind();
        }
        catch
        {
            ClientScript.RegisterStartupScript(this.GetType(), "ErroCarregar", "alert('Ocorreu um erro ao tentar carregar os dados! Verifique e tente novamente mais tarde.');", true);

        }

    }
    protected void btnImportarCappta_Click(object sender, EventArgs e)
    {
        foreach (ListViewItem itemP in lsvMarketplacesCappta.Items)
        {
            if (((CheckBox)itemP.FindControl("ckbImportar")).Checked == true)
            {
                GravarDadosRepresentante(((TextBox)itemP.FindControl("txtDocumento")).Text.ToString());
                ((CheckBox)itemP.FindControl("ckbImportar")).Checked = false;
            }
        }
        ckbTodosCappta.Checked = false;
    }
    protected void btnCarregarCadastrosZoop_Click(object sender, EventArgs e)
    {

    }
    protected void btnImportarZoop_Click(object sender, EventArgs e)
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
    protected void btnFechar_Click(object sender, EventArgs e)
    {
        ClientScript.RegisterStartupScript(this.GetType(), "FecharOnboarding", "opener.PostBackOnMainPage(); window.close(); ", true);
    }
    protected void lsvRepresentantes_ItemCommand(object sender, ListViewCommandEventArgs e)
    {

    }
    protected void ckbTodos_CheckedChanged(object sender, EventArgs e)
    {
        foreach (ListViewItem itemP in lsvMarketplacesCappta.Items)
        {
            ((CheckBox)itemP.FindControl("ckbImportar")).Checked = (((CheckBox)itemP.FindControl("ckbImportar")).Checked == false) ? true : false;
        }

    }
    protected void btnImportarDocumento_Click(object sender, EventArgs e)
    {
        if (txtDocumentoCappta.Text.ToString().Trim() != "")
        {
            GravarDadosRepresentante(txtDocumentoCappta.Text.ToString());
        }
        else
        {
            ClientScript.RegisterStartupScript(this.GetType(),
                "Alerta", "alert('Nenhum documento foi especificado! Verifique e reentre.');", true);

        }
    }

    private void GravarDadosRepresentante(string sDocumento)
    {
        string jsonRetornoCadastro = hubcappta.ConsultarRevendedor(sDocumento.ToString());
        txtDocumentoCappta.Text = jsonRetornoCadastro;

        JObject oRevendedor = JObject.Parse(jsonRetornoCadastro);

        try
        {

            SqlConnection connInsCons = new SqlConnection(Funcoes.conexao());
            connInsCons.Open();
            SqlCommand cmdInsCons = new SqlCommand("dbo.stp_pessoas_fj_ins", connInsCons);
            cmdInsCons.CommandType = CommandType.StoredProcedure;
            cmdInsCons.Parameters.Add("@flg_operacao", SqlDbType.Char).Value = 'I';
            cmdInsCons.Parameters.Add("@FLG_ATIVO", SqlDbType.Char).Value = "S";
            cmdInsCons.Parameters.Add("@FLG_INSERT_USUARIO", SqlDbType.Char).Value = "S";
            cmdInsCons.Parameters.Add("@COD_ID_PESSOA_LICENCIADO", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["LICENCIADO"].ToString());
            cmdInsCons.Parameters.Add("@DTA_DATA", SqlDbType.DateTime).Value = DateTime.Now;
            cmdInsCons.Parameters.Add("@FLG_TIPO", SqlDbType.Char).Value = "R";


            cmdInsCons.Parameters.Add("@FLG_TIPO_PESSOA", SqlDbType.VarChar).Value = (oRevendedor["reseller"]["document"].ToString().Trim().Length >= 14) ? "PJ" : "PF";
            cmdInsCons.Parameters.Add("@FLG_PRESENCIAL", SqlDbType.Char).Value = "N";
//            cmdInsCons.Parameters.Add("@COD_ID_MARKETPLACE", SqlDbType.Int).Value = Funcoes.strToInt(ddlMarketplace.SelectedValue.ToString());

            // Empresa
            cmdInsCons.Parameters.Add("@NOM_RAZAOSOCIAL", SqlDbType.VarChar).Value = oRevendedor["reseller"]["companyName"].ToString().Trim();
            cmdInsCons.Parameters.Add("@NOM_FANTASIA", SqlDbType.VarChar).Value = oRevendedor["reseller"]["tradingName"].ToString().Trim();
            cmdInsCons.Parameters.Add("@NOM_CNPJ", SqlDbType.VarChar).Value = oRevendedor["reseller"]["document"].ToString().Trim();

            cmdInsCons.Parameters.Add("@NUM_TELEFONE", SqlDbType.VarChar).Value = oRevendedor["responsible"]["phone"].ToString().Trim();
            cmdInsCons.Parameters.Add("@NOM_EMAIL_EMPRESA", SqlDbType.VarChar).Value = oRevendedor["responsible"]["email"].ToString().Trim();
            cmdInsCons.Parameters.Add("@COD_ID_MCC", SqlDbType.Int).Value = Funcoes.strToInt(oRevendedor["reseller"]["mccId"].ToString().Trim());
            //cmdInsCons.Parameters.Add("@NOM_TIPO_EMPRESA", SqlDbType.VarChar).Value = ddlTipoEmpresa.SelectedValue.ToString();
            cmdInsCons.Parameters.Add("@NUM_FATURAMENTO", SqlDbType.Float).Value = 0;
            cmdInsCons.Parameters.Add("@NUM_PATRIMONIO", SqlDbType.Float).Value = 0;

            //if (txtDataAbertura.Text.ToString().Trim() != "")
            //{
            //    cmdInsCons.Parameters.Add("@DTA_ABERTURA", SqlDbType.DateTime).Value = Convert.ToDateTime(txtDataAbertura.Text.ToString());
            //}

            // Endereço
            cmdInsCons.Parameters.Add("@NOM_ENDERECO", SqlDbType.VarChar).Value = oRevendedor["address"]["streetName"].ToString().Trim();
            cmdInsCons.Parameters.Add("@NOM_NUMERO", SqlDbType.VarChar).Value = oRevendedor["address"]["houseNumber"].ToString().Trim();
            cmdInsCons.Parameters.Add("@NOM_COMPLEMENTO", SqlDbType.VarChar).Value = oRevendedor["address"]["complement"].ToString().Trim();
            cmdInsCons.Parameters.Add("@NOM_BAIRRO", SqlDbType.VarChar).Value = oRevendedor["address"]["neighborhood"].ToString().Trim();
            cmdInsCons.Parameters.Add("@NOM_CIDADE", SqlDbType.VarChar).Value = oRevendedor["address"]["city"].ToString().Trim();
            cmdInsCons.Parameters.Add("@NOM_UF", SqlDbType.VarChar).Value = oRevendedor["address"]["state"].ToString().Trim();
            cmdInsCons.Parameters.Add("@NOM_CEP", SqlDbType.VarChar).Value = oRevendedor["address"]["postalCode"].ToString().Trim();
            cmdInsCons.Parameters.Add("@NOM_PAIS", SqlDbType.VarChar).Value = "BR";

            // Responsável
            cmdInsCons.Parameters.Add("@NOM_NOME", SqlDbType.VarChar).Value = oRevendedor["responsible"]["name"].ToString().Trim();
            cmdInsCons.Parameters.Add("@NOM_SOBRENOME", SqlDbType.VarChar).Value = oRevendedor["responsible"]["phone"].ToString().Trim();
            cmdInsCons.Parameters.Add("@NOM_CPF", SqlDbType.VarChar).Value = oRevendedor["responsible"]["cpf"].ToString().Trim();
            
            //if (txtNascimento.Text.ToString().Trim() != "")
            //{
            //    cmdInsCons.Parameters.Add("@DTA_ANIVERSARIO", SqlDbType.DateTime).Value = Convert.ToDateTime(txtNascimento.Text.ToString());
            //}

            //cmdInsCons.Parameters.Add("@NOM_MAE", SqlDbType.VarChar).Value = txtNomeMae.Text.ToString();
            //cmdInsCons.Parameters.Add("@NUM_RENDA_MENSAL", SqlDbType.Float).Value = Funcoes.strToDouble(txtRendaMensalPF.Text.ToString());
            cmdInsCons.Parameters.Add("@NOM_EMAIL", SqlDbType.VarChar).Value = oRevendedor["responsible"]["email"].ToString().Trim();
            cmdInsCons.Parameters.Add("@NOM_CELULAR", SqlDbType.VarChar).Value = oRevendedor["responsible"]["mobilePhone"].ToString().Trim();
            //cmdInsCons.Parameters.Add("@FLG_POLITICAMENTE", SqlDbType.Char).Value = ddlPoliticamenteExposta.SelectedValue.ToString();

            // Usuário
            cmdInsCons.Parameters.Add("@NOM_NOME_USUARIO", SqlDbType.VarChar).Value = oRevendedor["responsible"]["name"].ToString().Trim();
            cmdInsCons.Parameters.Add("@NOM_LOGIN", SqlDbType.VarChar).Value = oRevendedor["responsible"]["email"].ToString().Trim();
            cmdInsCons.Parameters.Add("@NOM_SENHA", SqlDbType.VarChar).Value = oRevendedor["reseller"]["document"].ToString().Trim();
            
            string sCodigo = cmdInsCons.ExecuteScalar().ToString();
            //cmdInsCons.ExecuteNonQuery();
            connInsCons.Close();
            connInsCons.Dispose();

            // Dados Onboarding Cappta
            SqlConnection connInsConsDadosCappta = new SqlConnection(Funcoes.conexao());
            connInsConsDadosCappta.Open();
            SqlCommand cmdInsConsDadosCappta = new SqlCommand("dbo.stp_pessoas_fj_cappta_ins", connInsConsDadosCappta);
            cmdInsConsDadosCappta.CommandType = CommandType.StoredProcedure;
            cmdInsConsDadosCappta.Parameters.Add("@flg_operacao", SqlDbType.Char).Value = 'I';
            cmdInsConsDadosCappta.Parameters.Add("@COD_ID_PESSOA_LICENCIADO", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["LICENCIADO"].ToString());
            cmdInsConsDadosCappta.Parameters.Add("@COD_ID_PESSOAS_FJ", SqlDbType.Int).Value = Funcoes.strToInt(sCodigo.ToString());

            cmdInsConsDadosCappta.Parameters.Add("@NUM_TOKEN_CAPPTA", SqlDbType.VarChar).Value = oRevendedor["reseller"]["document"].ToString();
            cmdInsConsDadosCappta.Parameters.Add("@FLG_STATUS_CAPPTA", SqlDbType.VarChar).Value = oRevendedor["statusDescription"].ToString();
            cmdInsConsDadosCappta.Parameters.Add("@FLG_CAPPTA", SqlDbType.Char).Value = "S";
            cmdInsConsDadosCappta.Parameters.Add("@DES_JSON_CAPPTA", SqlDbType.VarChar).Value = jsonRetornoCadastro.ToString();
            cmdInsConsDadosCappta.Parameters.Add("@COD_ID_NATUREZA_CAPPTA", SqlDbType.Int).Value = oRevendedor["reseller"]["legalNatureId"].ToString();

            cmdInsConsDadosCappta.ExecuteNonQuery();
            connInsConsDadosCappta.Close();
            connInsConsDadosCappta.Dispose();

            // Dados Bancários Cappta
            SqlConnection connInsConsBancoCappta = new SqlConnection(Funcoes.conexao());
            connInsConsBancoCappta.Open();
            SqlCommand cmdInsConsBancoCappta = new SqlCommand("dbo.stp_pessoas_fj_contas_ins", connInsConsBancoCappta);
            cmdInsConsBancoCappta.CommandType = CommandType.StoredProcedure;
            cmdInsConsBancoCappta.Parameters.Add("@flg_operacao", SqlDbType.Char).Value = 'I';
            cmdInsConsBancoCappta.Parameters.Add("@COD_ID_PESSOA_LICENCIADO", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["LICENCIADO"].ToString());
            cmdInsConsBancoCappta.Parameters.Add("@COD_ID_PESSOAS_FJ", SqlDbType.Int).Value = Funcoes.strToInt(sCodigo.ToString());

            cmdInsConsBancoCappta.Parameters.Add("@NOM_CODIGO_BANCO", SqlDbType.VarChar).Value = oRevendedor["bankAccount"]["bankCode"].ToString();
            cmdInsConsBancoCappta.Parameters.Add("@NOM_TIPO_BANCO", SqlDbType.VarChar).Value = (oRevendedor["bankAccount"]["accountType"].ToString().Trim() == "1") ? "C" : "P";
            cmdInsConsBancoCappta.Parameters.Add("@NOM_NUMERO_AGENCIA_BANCO", SqlDbType.VarChar).Value = oRevendedor["bankAccount"]["branch"].ToString();
            cmdInsConsBancoCappta.Parameters.Add("@NOM_NUMERO_DIGITO_AGENCIA_BANCO", SqlDbType.VarChar).Value = "";
            cmdInsConsBancoCappta.Parameters.Add("@NOM_NUMERO_CONTA_BANCO", SqlDbType.VarChar).Value = oRevendedor["bankAccount"]["account"].ToString();
            cmdInsConsBancoCappta.Parameters.Add("@NOM_NUMERO_DIGITO_CONTA_BANCO", SqlDbType.VarChar).Value = "";
            cmdInsConsBancoCappta.Parameters.Add("@FLG_PADRAO", SqlDbType.Char).Value = "S";

            cmdInsConsBancoCappta.ExecuteNonQuery();
            connInsConsBancoCappta.Close();
            connInsConsBancoCappta.Dispose();


            ClientScript.RegisterStartupScript(this.GetType(),
                "MensagemSucesso", "alert('Documento(s) gravados com sucesso');", true);

            // Gravar os dados
        }
        catch
        {

            // Exibir mensagem de alerta
            ClientScript.RegisterStartupScript(this.GetType(),
                "MensagemErro", "alert('Ocorreu um erro ao tentar importar o documento: " + sDocumento.ToString() + " - " + oRevendedor["errorMessage"].ToString() + "');", true);
        }


    }

    protected void ckbTodosCappta_CheckedChanged(object sender, EventArgs e)
    {
        foreach (ListViewItem itemP in lsvMarketplacesCappta.Items)
        {
            ((CheckBox)itemP.FindControl("ckbImportar")).Checked = (((CheckBox)itemP.FindControl("ckbImportar")).Checked == false) ? true : false;
        }

    }

    protected void btnImportarDocumentoCappta_Click(object sender, EventArgs e)
    {
        if (txtDocumentoCappta.Text.ToString().Trim() != "")
        {
            GravarDadosRepresentante(txtDocumentoCappta.Text.ToString());
        }
        else
        {
            ClientScript.RegisterStartupScript(this.GetType(),
                "Alerta", "alert('Nenhum documento foi especificado! Verifique e reentre.');", true);

        }

    }
}