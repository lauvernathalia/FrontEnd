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

public partial class cad_equipamentos_seriais_adm : System.Web.UI.Page
{
    public static DataTable dtSeriais;

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
            // Definição Tabela de Seriais
            dtSeriais = new DataTable();
            dtSeriais.Columns.Add("id", typeof(string));
            dtSeriais.Columns.Add("serial", typeof(string));
            dtSeriais.Columns.Add("modelo", typeof(string));
            dtSeriais.Columns.Add("marketplace", typeof(string));
            dtSeriais.Columns.Add("estabelecimento", typeof(string));


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
        SqlDataAdapter SDAConsulta = new SqlDataAdapter("dbo.stp_equipamentos_seriais_ins", myConsulta);
        SDAConsulta.SelectCommand.CommandType = CommandType.StoredProcedure;
        SDAConsulta.SelectCommand.Parameters.Add("@flg_operacao", SqlDbType.Char).Value = "C";
        SDAConsulta.SelectCommand.Parameters.Add("@COD_ID_PESSOA_LICENCIADO", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["LICENCIADO"].ToString());

        DataSet dsConsulta = new DataSet();
        SDAConsulta.Fill(dsConsulta, "EQUIPAMENTOS_SERIAIS");
        rptConsulta.DataSource = dsConsulta.Tables["EQUIPAMENTOS_SERIAIS"].DefaultView;
        rptConsulta.DataBind();
        myConsulta.Close(); myConsulta.Dispose();

    }

    protected void btnSalvar_Click(object sender, EventArgs e)
    {
        SqlConnection connInsCons = new SqlConnection(Funcoes.conexao());
        connInsCons.Open();
        SqlCommand cmdInsCons = new SqlCommand("dbo.stp_equipamentos_seriais_ins", connInsCons);
        cmdInsCons.CommandType = CommandType.StoredProcedure;
        cmdInsCons.Parameters.Add("@flg_operacao", SqlDbType.Char).Value = 'I';
        cmdInsCons.Parameters.Add("@COD_ID_PESSOA_LICENCIADO", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["LICENCIADO"].ToString());
        cmdInsCons.Parameters.Add("@NUM_TOKEN", SqlDbType.VarChar).Value = txtToken.Text.ToString();
        cmdInsCons.Parameters.Add("@NUM_SERIAL", SqlDbType.VarChar).Value = txtSerial.Text.ToString();

        cmdInsCons.ExecuteNonQuery();
        connInsCons.Close();
        connInsCons.Dispose();

        ClientScript.RegisterStartupScript(this.GetType(),
            "Alerta", "alert('Dados gravados com sucesso');", true);

        txtToken.Text = "";
        txtSerial.Text = "";

        ConsultaGeral();
    }
    protected void btnCancelar_Click(object sender, EventArgs e)
    {
        ClientScript.RegisterStartupScript(this.GetType(),
            "Alerta", "opener.PostBackOnMainPage(); window.close(); ", true);
    }

    protected void rptConsulta_OnItemCommand(object source, RepeaterCommandEventArgs e)
    {
        if (e.CommandName == "Excluir")
        {

            SqlConnection connInsCons = new SqlConnection(Funcoes.conexao());
            connInsCons.Open();
            SqlCommand cmdInsCons = new SqlCommand("dbo.stp_equipamentos_seriais_ins", connInsCons);
            cmdInsCons.CommandType = CommandType.StoredProcedure;
            cmdInsCons.Parameters.Add("@flg_operacao", SqlDbType.Char).Value = "E";
            cmdInsCons.Parameters.Add("@COD_ID", SqlDbType.Int).Value = Funcoes.strToInt(Convert.ToString(e.CommandArgument));
            cmdInsCons.Parameters.Add("@COD_ID_PESSOA_LICENCIADO", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["LICENCIADO"].ToString());

            cmdInsCons.ExecuteNonQuery();
            connInsCons.Close();
            connInsCons.Dispose();

            ClientScript.RegisterStartupScript(this.GetType(),
                "Alerta", "alert('Serial Excluído com sucesso!');", true);
        }

        if (e.CommandName == "Atualizar")
        {
            foreach (RepeaterItem item in rptConsulta.Items)
            {
                if (Funcoes.strToInt(Convert.ToString(e.CommandArgument)) == Funcoes.strToInt(((TextBox)item.FindControl("txtID")).Text.ToString()))
                {
                    SqlConnection connInsCons = new SqlConnection(Funcoes.conexao());
                    connInsCons.Open();
                    SqlCommand cmdInsCons = new SqlCommand("dbo.stp_equipamentos_seriais_ins", connInsCons);
                    cmdInsCons.CommandType = CommandType.StoredProcedure;
                    cmdInsCons.Parameters.Add("@flg_operacao", SqlDbType.Char).Value = "A";
                    cmdInsCons.Parameters.Add("@COD_ID", SqlDbType.Int).Value = Funcoes.strToInt(Convert.ToString(e.CommandArgument));
                    cmdInsCons.Parameters.Add("@COD_ID_PESSOA_LICENCIADO", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["LICENCIADO"].ToString());
                    cmdInsCons.Parameters.Add("@NUM_TOKEN", SqlDbType.VarChar).Value = ((TextBox)item.FindControl("txtToken")).Text.ToString();
                    cmdInsCons.Parameters.Add("@NUM_SERIAL", SqlDbType.Text).Value = ((TextBox)item.FindControl("txtSerial")).Text.ToString();

                    cmdInsCons.ExecuteNonQuery();
                    connInsCons.Close();
                    connInsCons.Dispose();

                    ClientScript.RegisterStartupScript(this.GetType(),
                        "Alerta", "alert('Serial Atualizado com sucesso!');", true);
                }
            }
        }


        ConsultaGeral();

    }

    protected void btnCarregarCadastrosCappta_Click(object sender, EventArgs e)
    {
        dtSeriais.Rows.Clear();

        string jsonSeriais = "";
        jsonSeriais = hubcappta.ConsultarListaPOS("");
        txtJsonCappta.Text = jsonSeriais;

        JArray oSeriais = JArray.Parse(jsonSeriais);
        //JObject oRevendedores = JObject.Parse(jsonRevendedores);


        if (oSeriais.Count > 0)
        {
            for (int i = 0; i < oSeriais.Count; i++)
            {
                dtSeriais.Rows.Add(oSeriais[i]["id"].ToString().Trim(), oSeriais[i]["serialKey"].ToString().Trim(), oSeriais[i]["modelId"].ToString().Trim(), oSeriais[i]["resellerDocument"].ToString().Trim(), oSeriais[i]["merchantDocument"].ToString().Trim());
            }
        }

        this.lsvSeriais.DataSource = dtSeriais;
        this.lsvSeriais.DataBind();

        ClientScript.RegisterStartupScript(this.GetType(),
"SucessoCarregarDados", "alert('Dados dos Modelos de POS carregados com sucesso');", true);


    }
    protected void btnImportarCappta_Click(object sender, EventArgs e)
    {
        foreach (ListViewItem itemP in lsvSeriais.Items)
        {
            if (((CheckBox)itemP.FindControl("ckbImportar")).Checked == true)
            {
                GravarDadosSeriais(((TextBox)itemP.FindControl("txtID")).Text.ToString(), ((TextBox)itemP.FindControl("txtSerial")).Text.ToString(), ((TextBox)itemP.FindControl("txtModelo")).Text.ToString(), ((TextBox)itemP.FindControl("txtMarketplace")).Text.ToString(), ((TextBox)itemP.FindControl("txtEstabelecimento")).Text.ToString());
            }
        }

        ClientScript.RegisterStartupScript(this.GetType(),
"SucessoImportarDados", "alert('Dados dos Modelos de POS importados com sucesso');", true);


    }
    protected void ckbTodos_CheckedChanged(object sender, EventArgs e)
    {
        foreach (ListViewItem itemP in lsvSeriais.Items)
        {
            ((CheckBox)itemP.FindControl("ckbImportar")).Checked = (((CheckBox)itemP.FindControl("ckbImportar")).Checked == false) ? true : false;
        }

    }
    protected void lsvSeriais_ItemCommand(object sender, ListViewCommandEventArgs e)
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

    private void GravarDadosSeriais(string sID, string sSerial, string sModelo, string sMarketplace, string sEstabelecimento)
    {
        // VERIFICA SE O REPRESENTANTE E O ESTABELECIMENTO EXISTEM
        /*
        ClientScript.RegisterStartupScript(this.GetType(),
"CodigoRepresentante", "alert('Representante: " + sRepresentante.ToString() + "');", true);

        ClientScript.RegisterStartupScript(this.GetType(),
"Representante", "alert('Representante: " + VerificaRepresentante(sRepresentante).ToString() + "');", true);
        //VerificaEstabelecimento(sRepresentante, sEstabelecimento);

        ClientScript.RegisterStartupScript(this.GetType(),
"CodigoEstabelecimento", "alert('Estabelecimento: " + sEstabelecimento.ToString() + "');", true);


        ClientScript.RegisterStartupScript(this.GetType(),
"Estabelecimento", "alert('Estabelecimento: " + VerificaEstabelecimento(sRepresentante, sEstabelecimento).ToString() + "');", true);
        */
        
        SqlConnection connInsCons = new SqlConnection(Funcoes.conexao());
        connInsCons.Open();
        SqlCommand cmdInsCons = new SqlCommand("dbo.stp_equipamentos_seriais_ins", connInsCons);
        cmdInsCons.CommandType = CommandType.StoredProcedure;
        cmdInsCons.Parameters.Add("@flg_operacao", SqlDbType.Char).Value = 'I';
        cmdInsCons.Parameters.Add("@COD_ID_PESSOA_LICENCIADO", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["LICENCIADO"].ToString());
        cmdInsCons.Parameters.Add("@COD_ID_MODELO", SqlDbType.VarChar).Value = sModelo.ToString();
        cmdInsCons.Parameters.Add("@NUM_TOKEN", SqlDbType.VarChar).Value = sID.ToString();
        cmdInsCons.Parameters.Add("@NUM_SERIAL", SqlDbType.VarChar).Value = sSerial.ToString();
        cmdInsCons.Parameters.Add("@COD_ID_MARKETPLACE", SqlDbType.Int).Value = Funcoes.strToInt(VerificaMarketplace(sMarketplace).ToString());
        cmdInsCons.Parameters.Add("@COD_ID_PESSOAS_FJ", SqlDbType.Int).Value = Funcoes.strToInt(VerificaEstabelecimento(sMarketplace, sEstabelecimento).ToString());
        cmdInsCons.ExecuteNonQuery();
        connInsCons.Close();
        connInsCons.Dispose();
        
    }

    public string VerificaMarketplace(string sMarketplace)
    {
        string sCodigoMarketplace="0";


        string jsonRetornoCadastro = hubcappta.ConsultarRevendedor(sMarketplace.ToString());
        txtJsonCappta.Visible = false; //true;
        txtJsonCappta.Text = jsonRetornoCadastro;

        JObject oRevendedor = JObject.Parse(jsonRetornoCadastro);

        try
        {
            txtJsonCappta.Text = txtJsonCappta.Text + oRevendedor["partnerDocument"].ToString();

            SqlConnection connInsCons = new SqlConnection(Funcoes.conexao());
            connInsCons.Open();
            SqlCommand cmdInsCons = new SqlCommand("dbo.stp_pessoas_fj_ins", connInsCons);
            cmdInsCons.CommandType = CommandType.StoredProcedure;
            cmdInsCons.Parameters.Add("@flg_operacao", SqlDbType.Char).Value = 'I';
            cmdInsCons.Parameters.Add("@FLG_ATIVO", SqlDbType.Char).Value = "S";
            cmdInsCons.Parameters.Add("@FLG_INSERT_USUARIO", SqlDbType.Char).Value = "S";
            cmdInsCons.Parameters.Add("@COD_ID_PESSOA_LICENCIADO", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["LICENCIADO"].ToString());
            cmdInsCons.Parameters.Add("@DTA_DATA", SqlDbType.DateTime).Value = DateTime.Now;
            cmdInsCons.Parameters.Add("@FLG_TIPO", SqlDbType.Char).Value = "M";


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

            sCodigoMarketplace = cmdInsCons.ExecuteScalar().ToString();

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
            cmdInsConsDadosCappta.Parameters.Add("@COD_ID_PESSOAS_FJ", SqlDbType.Int).Value = Funcoes.strToInt(sCodigoMarketplace.ToString());

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
            cmdInsConsBancoCappta.Parameters.Add("@COD_ID_PESSOAS_FJ", SqlDbType.Int).Value = Funcoes.strToInt(sCodigoMarketplace.ToString());

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
        }
        catch
        {
            sMarketplace = "0";
        }
        return sCodigoMarketplace;
    }

    public string VerificaEstabelecimento(string sMarketplace, string sEstabelecimento)
    {
        string sCodigoEstabelecimento = "0";
        
        if (sEstabelecimento.ToString().Trim() != "")
        {
            string jsonRetornoCadastro = hubcappta.ConsultarLojista(sEstabelecimento.ToString(), sMarketplace.ToString());
            txtJsonCappta.Visible = false;// true;
            //txtJsonCappta.Text = jsonRetornoCadastro + " / " +  sRepresentante.ToString() + " / " + sEstabelecimento.ToString();

            JObject oEstabelecimento = JObject.Parse(jsonRetornoCadastro);

            try
            {
                //txtJsonCappta.Text = txtJsonCappta.Text + oEstabelecimento["resellerDocument"].ToString();
                // ********************************************************************
                // Localizar cadastro do Representante
                // ********************************************************************
                string sIDMarketplace = "0";
                SqlConnection mySelCadastro = new SqlConnection(Funcoes.conexao());
                mySelCadastro.Open();
                SqlCommand cmdSelCadastro = new SqlCommand("dbo.stp_pessoas_fj_ins", mySelCadastro);
                cmdSelCadastro.CommandType = CommandType.StoredProcedure;
                cmdSelCadastro.Parameters.Add("@flg_operacao", SqlDbType.Char).Value = "H";
                cmdSelCadastro.Parameters.Add("@NOM_CNPJ", SqlDbType.VarChar).Value = sMarketplace.ToString();
                cmdSelCadastro.Parameters.Add("@COD_ID_PESSOA_LICENCIADO", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["LICENCIADO"].ToString());
                cmdSelCadastro.Parameters.Add("@FLG_TIPO", SqlDbType.Char).Value = "M";

                SqlDataReader ReaderCadastro = cmdSelCadastro.ExecuteReader();
                while (ReaderCadastro.Read())
                {
                    sIDMarketplace = ReaderCadastro["COD_ID"].ToString();
                }

                SqlConnection connInsCons = new SqlConnection(Funcoes.conexao());
                connInsCons.Open();
                SqlCommand cmdInsCons = new SqlCommand("dbo.stp_pessoas_fj_ins", connInsCons);
                cmdInsCons.CommandType = CommandType.StoredProcedure;
                cmdInsCons.Parameters.Add("@flg_operacao", SqlDbType.Char).Value = 'I';
                cmdInsCons.Parameters.Add("@FLG_ATIVO", SqlDbType.Char).Value = "S";
                cmdInsCons.Parameters.Add("@FLG_INSERT_USUARIO", SqlDbType.Char).Value = "S";
                cmdInsCons.Parameters.Add("@COD_ID_PESSOA_LICENCIADO", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["LICENCIADO"].ToString());
                cmdInsCons.Parameters.Add("@DTA_DATA", SqlDbType.DateTime).Value = DateTime.Now;
                cmdInsCons.Parameters.Add("@FLG_TIPO", SqlDbType.Char).Value = "E";


                cmdInsCons.Parameters.Add("@FLG_TIPO_PESSOA", SqlDbType.VarChar).Value = (oEstabelecimento["merchant"]["document"].ToString().Trim().Length >= 14) ? "PJ" : "PF";
                cmdInsCons.Parameters.Add("@FLG_PRESENCIAL", SqlDbType.Char).Value = "N";
                //            cmdInsCons.Parameters.Add("@COD_ID_MARKETPLACE", SqlDbType.Int).Value = Funcoes.strToInt(ddlMarketplace.SelectedValue.ToString());

                cmdInsCons.Parameters.Add("@COD_ID_MARKETPLACE", SqlDbType.Int).Value = Funcoes.strToInt(sIDMarketplace.ToString());


                // Empresa
                cmdInsCons.Parameters.Add("@NOM_RAZAOSOCIAL", SqlDbType.VarChar).Value = (oEstabelecimento["merchant"]["document"].ToString().Trim().Length >= 14) ? oEstabelecimento["merchant"]["companyName"].ToString().Trim() : oEstabelecimento["owner"]["name"].ToString().Trim();
                cmdInsCons.Parameters.Add("@NOM_FANTASIA", SqlDbType.VarChar).Value = (oEstabelecimento["merchant"]["document"].ToString().Trim().Length >= 14) ? oEstabelecimento["merchant"]["tradingName"].ToString().Trim() : oEstabelecimento["owner"]["name"].ToString().Trim();
                cmdInsCons.Parameters.Add("@NOM_CNPJ", SqlDbType.VarChar).Value = oEstabelecimento["merchant"]["document"].ToString().Trim();

                cmdInsCons.Parameters.Add("@NUM_TELEFONE", SqlDbType.VarChar).Value = oEstabelecimento["owner"]["phone"].ToString().Trim();
                cmdInsCons.Parameters.Add("@NOM_EMAIL_EMPRESA", SqlDbType.VarChar).Value = oEstabelecimento["owner"]["email"].ToString().Trim();
                cmdInsCons.Parameters.Add("@COD_ID_MCC", SqlDbType.Int).Value = Funcoes.strToInt(oEstabelecimento["merchant"]["mccId"].ToString().Trim());
                //cmdInsCons.Parameters.Add("@NOM_TIPO_EMPRESA", SqlDbType.VarChar).Value = ddlTipoEmpresa.SelectedValue.ToString();
                cmdInsCons.Parameters.Add("@NUM_FATURAMENTO", SqlDbType.Float).Value = 0;
                cmdInsCons.Parameters.Add("@NUM_PATRIMONIO", SqlDbType.Float).Value = 0;

                //if (txtDataAbertura.Text.ToString().Trim() != "")
                //{
                //    cmdInsCons.Parameters.Add("@DTA_ABERTURA", SqlDbType.DateTime).Value = Convert.ToDateTime(txtDataAbertura.Text.ToString());
                //}

                // Endereço
                cmdInsCons.Parameters.Add("@NOM_ENDERECO", SqlDbType.VarChar).Value = oEstabelecimento["address"]["streetName"].ToString().Trim();
                cmdInsCons.Parameters.Add("@NOM_NUMERO", SqlDbType.VarChar).Value = oEstabelecimento["address"]["houseNumber"].ToString().Trim();
                cmdInsCons.Parameters.Add("@NOM_COMPLEMENTO", SqlDbType.VarChar).Value = oEstabelecimento["address"]["complement"].ToString().Trim();
                cmdInsCons.Parameters.Add("@NOM_BAIRRO", SqlDbType.VarChar).Value = oEstabelecimento["address"]["neighborhood"].ToString().Trim();
                cmdInsCons.Parameters.Add("@NOM_CIDADE", SqlDbType.VarChar).Value = oEstabelecimento["address"]["city"].ToString().Trim();
                cmdInsCons.Parameters.Add("@NOM_UF", SqlDbType.VarChar).Value = oEstabelecimento["address"]["state"].ToString().Trim();
                cmdInsCons.Parameters.Add("@NOM_CEP", SqlDbType.VarChar).Value = oEstabelecimento["address"]["postalCode"].ToString().Trim();
                cmdInsCons.Parameters.Add("@NOM_PAIS", SqlDbType.VarChar).Value = "BR";

                // Responsável
                cmdInsCons.Parameters.Add("@NOM_NOME", SqlDbType.VarChar).Value = oEstabelecimento["owner"]["name"].ToString().Trim();
                cmdInsCons.Parameters.Add("@NOM_SOBRENOME", SqlDbType.VarChar).Value = oEstabelecimento["owner"]["phone"].ToString().Trim();
                cmdInsCons.Parameters.Add("@NOM_CPF", SqlDbType.VarChar).Value = oEstabelecimento["owner"]["cpf"].ToString().Trim();

                //if (txtNascimento.Text.ToString().Trim() != "")
                //{
                //    cmdInsCons.Parameters.Add("@DTA_ANIVERSARIO", SqlDbType.DateTime).Value = Convert.ToDateTime(txtNascimento.Text.ToString());
                //}

                //cmdInsCons.Parameters.Add("@NOM_MAE", SqlDbType.VarChar).Value = txtNomeMae.Text.ToString();
                //cmdInsCons.Parameters.Add("@NUM_RENDA_MENSAL", SqlDbType.Float).Value = Funcoes.strToDouble(txtRendaMensalPF.Text.ToString());
                cmdInsCons.Parameters.Add("@NOM_EMAIL", SqlDbType.VarChar).Value = oEstabelecimento["owner"]["email"].ToString().Trim();
                cmdInsCons.Parameters.Add("@NOM_CELULAR", SqlDbType.VarChar).Value = oEstabelecimento["owner"]["mobilePhone"].ToString().Trim();
                //cmdInsCons.Parameters.Add("@FLG_POLITICAMENTE", SqlDbType.Char).Value = ddlPoliticamenteExposta.SelectedValue.ToString();

                // Usuário
                cmdInsCons.Parameters.Add("@NOM_NOME_USUARIO", SqlDbType.VarChar).Value = oEstabelecimento["owner"]["name"].ToString().Trim();
                cmdInsCons.Parameters.Add("@NOM_LOGIN", SqlDbType.VarChar).Value = oEstabelecimento["owner"]["email"].ToString().Trim();
                cmdInsCons.Parameters.Add("@NOM_SENHA", SqlDbType.VarChar).Value = oEstabelecimento["merchant"]["document"].ToString().Trim();

                sCodigoEstabelecimento = cmdInsCons.ExecuteScalar().ToString();

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
                cmdInsConsDadosCappta.Parameters.Add("@COD_ID_PESSOAS_FJ", SqlDbType.Int).Value = Funcoes.strToInt(sCodigoEstabelecimento.ToString());

                cmdInsConsDadosCappta.Parameters.Add("@NUM_TOKEN_CAPPTA", SqlDbType.VarChar).Value = oEstabelecimento["merchant"]["document"].ToString();
                cmdInsConsDadosCappta.Parameters.Add("@FLG_STATUS_CAPPTA", SqlDbType.VarChar).Value = oEstabelecimento["statusDescription"].ToString();
                cmdInsConsDadosCappta.Parameters.Add("@FLG_CAPPTA", SqlDbType.Char).Value = "S";
                cmdInsConsDadosCappta.Parameters.Add("@DES_JSON_CAPPTA", SqlDbType.VarChar).Value = jsonRetornoCadastro.ToString();
                cmdInsConsDadosCappta.Parameters.Add("@COD_ID_NATUREZA_CAPPTA", SqlDbType.Int).Value = oEstabelecimento["merchant"]["legalNatureId"].ToString();

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
                cmdInsConsBancoCappta.Parameters.Add("@COD_ID_PESSOAS_FJ", SqlDbType.Int).Value = Funcoes.strToInt(sCodigoEstabelecimento.ToString());

                cmdInsConsBancoCappta.Parameters.Add("@NOM_CODIGO_BANCO", SqlDbType.VarChar).Value = oEstabelecimento["bankAccount"]["bankCode"].ToString();
                cmdInsConsBancoCappta.Parameters.Add("@NOM_TIPO_BANCO", SqlDbType.VarChar).Value = (oEstabelecimento["bankAccount"]["accountType"].ToString().Trim() == "1") ? "C" : "P";
                cmdInsConsBancoCappta.Parameters.Add("@NOM_NUMERO_AGENCIA_BANCO", SqlDbType.VarChar).Value = oEstabelecimento["bankAccount"]["branch"].ToString();
                cmdInsConsBancoCappta.Parameters.Add("@NOM_NUMERO_DIGITO_AGENCIA_BANCO", SqlDbType.VarChar).Value = "";
                cmdInsConsBancoCappta.Parameters.Add("@NOM_NUMERO_CONTA_BANCO", SqlDbType.VarChar).Value = oEstabelecimento["bankAccount"]["account"].ToString();
                cmdInsConsBancoCappta.Parameters.Add("@NOM_NUMERO_DIGITO_CONTA_BANCO", SqlDbType.VarChar).Value = "";
                cmdInsConsBancoCappta.Parameters.Add("@FLG_PADRAO", SqlDbType.Char).Value = "S";

                cmdInsConsBancoCappta.ExecuteNonQuery();
                connInsConsBancoCappta.Close();
                connInsConsBancoCappta.Dispose();

            }
            catch
            {
                sCodigoEstabelecimento = "0";
            }

        }
        return sCodigoEstabelecimento;
    }

}