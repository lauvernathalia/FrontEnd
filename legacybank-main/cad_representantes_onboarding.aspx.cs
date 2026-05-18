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


public partial class cad_representantes_onboarding : System.Web.UI.Page
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
        if (!IsPostBack)
        {
            // Verificar Integrações
            ConsultaIntegracoes();

            SqlConnection myBanco = new SqlConnection(Funcoes.conexao());
            myBanco.Open();
            SqlCommand cmdBanco = new SqlCommand("dbo.stp_bancos_ins", myBanco);
            cmdBanco.CommandType = CommandType.StoredProcedure;
            cmdBanco.Parameters.Add("@flg_operacao", SqlDbType.VarChar).Value = "L";
            SqlDataAdapter drBanco = new SqlDataAdapter();
            drBanco.SelectCommand = cmdBanco;
            DataSet dsBanco = new DataSet();
            drBanco.Fill(dsBanco, "BANCO");
            ddlInstituicaoFinanceira.DataTextField = "NOM_BANCO";
            ddlInstituicaoFinanceira.DataValueField = "COD_ID";
            ddlInstituicaoFinanceira.DataSource = dsBanco.Tables["BANCO"].DefaultView;
            ddlInstituicaoFinanceira.DataBind();
            ddlInstituicaoFinanceira.Items.Insert(0, new ListItem("", "0"));


            if (Funcoes.strToInt(sid_id) != 0)
            {
                ConsultaFicha();
                ConsultaGeral();


                // CAPPTA
                if ((tbcapptatab.Visible == true) && (tbcappta.Visible == true)) { ConsultaTabelasCapta(); ConsultaCappta(); }
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
            txtTipo.Text = ReaderCadastro["FLG_TIPO_PESSOA"].ToString();
            if (ReaderCadastro["FLG_TIPO_PESSOA"].ToString().Trim() == "PF")
            {
                txtDocumento.Text = ReaderCadastro["NOM_CPF"].ToString();
            }
            else
            {
                txtDocumento.Text = ReaderCadastro["NOM_CNPJ"].ToString();

            }



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

    private void ConsultaAsaas()
    {

    }
    private void ConsultaZoop()
    {

    }
    private void ConsultaPagseguro()
    {

    }

    protected void btnFechar_Click(object sender, EventArgs e)
    {
        ClientScript.RegisterStartupScript(this.GetType(), "FecharOnboarding", "opener.PostBackOnMainPage(); window.close(); ", true);

    }


    private void ConsultaGeral()
    {
        SqlConnection myConsulta = new SqlConnection(Funcoes.conexao());
        myConsulta.Open();
        SqlDataAdapter SDAConsulta = new SqlDataAdapter("dbo.stp_pessoas_fj_contas_ins", myConsulta);
        SDAConsulta.SelectCommand.CommandType = CommandType.StoredProcedure;
        SDAConsulta.SelectCommand.Parameters.Add("@flg_operacao", SqlDbType.Char).Value = "C";
        SDAConsulta.SelectCommand.Parameters.Add("@COD_ID_PESSOA_LICENCIADO", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["LICENCIADO"].ToString());
        SDAConsulta.SelectCommand.Parameters.Add("@COD_ID_PESSOAS_FJ", SqlDbType.Int).Value = Funcoes.strToInt(sid_id.ToString());

        DataSet dsConsulta = new DataSet();
        SDAConsulta.Fill(dsConsulta, "PESSOAS_FJ_CONTAS");
        rptConsultaContas.DataSource = dsConsulta.Tables["PESSOAS_FJ_CONTAS"].DefaultView;
        rptConsultaContas.DataBind();
        myConsulta.Close(); myConsulta.Dispose();
    }


    // -----------------------------------------------------------------------------------------------------------------------------------------------------------------
    // SETOR BAAS 
    // -----------------------------------------------------------------------------------------------------------------------------------------------------------------



    // -----------------------------------------------------------------------------------------------------------------------------------------------------------------
    // SETOR CAPPTA 
    // -----------------------------------------------------------------------------------------------------------------------------------------------------------------

    private void ConsultaCappta()
    {

        SqlConnection mySelCadastro = new SqlConnection(Funcoes.conexao());
        mySelCadastro.Open();
        SqlCommand cmdSelCadastro = new SqlCommand("dbo.stp_pessoas_fj_cappta_ins", mySelCadastro);
        cmdSelCadastro.CommandType = CommandType.StoredProcedure;
        cmdSelCadastro.Parameters.Add("@flg_operacao", SqlDbType.Char).Value = "S";
        cmdSelCadastro.Parameters.Add("@COD_ID_PESSOA_LICENCIADO", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["LICENCIADO"].ToString());
        cmdSelCadastro.Parameters.Add("@COD_ID_PESSOAS_FJ", SqlDbType.Int).Value = Funcoes.strToInt(sid_id.ToString());
        SqlDataReader ReaderCadastro = cmdSelCadastro.ExecuteReader();
        while (ReaderCadastro.Read())
        {
            ddlCappta.SelectedValue = ReaderCadastro["FLG_CAPPTA"].ToString();
            ddlNatureza.SelectedValue = ReaderCadastro["COD_ID_NATUREZA_CAPPTA"].ToString();
            txtTokenCappta.Text = ReaderCadastro["NUM_TOKEN_CAPPTA"].ToString();
            txtStatusCappta.Text = ReaderCadastro["FLG_STATUS_CAPPTA"].ToString();
            txtNomeStatusCappta.Text = ReaderCadastro["FLG_STATUS_CAPPTA"].ToString();
        }

    }


    private void ConsultaTabelasCapta()
    {
        string jsonOpcoesCadastroNatureza = hubcappta.ListarOpcoesCadastro();
        JObject oNatureza = JObject.Parse(jsonOpcoesCadastroNatureza);

        if (oNatureza["legalNature"].Count() > 0)
        {
            for (int i = 0; i < oNatureza["legalNature"].Count(); i++)
            {
                ddlNatureza.Items.Insert(0, new ListItem(oNatureza["legalNature"][i]["Description"].ToString(), oNatureza["legalNature"][i]["Id"].ToString()));
            }
        }
        ddlNatureza.Items.Insert(0, new ListItem("", "0"));
    }


    protected void btnSalvarCappta_Click(object sender, EventArgs e)
    {
        if (ddlNatureza.SelectedValue.ToString().Trim() == "0")
        {
            ClientScript.RegisterStartupScript(this.GetType(),
                "NaturezaJuridica", "alert('A natureza Jurídica do Estabelecimento precisa ser selecionada! Verifique e reentre.');", true);
        }
        else
        {
            string jsonRevendedor = "";
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
                DadosRevendedor.Root dRevendedor = new DadosRevendedor.Root()
                {
                    reseller = new DadosRevendedor.Reseller()
                    {
                        document = (ReaderCadastro["FLG_TIPO_PESSOA"].ToString().Trim() == "PF") ? TIRAACENTOS(ReaderCadastro["NOM_CPF"].ToString().Trim()) : TIRAACENTOS(ReaderCadastro["NOM_CNPJ"].ToString().Trim()),
                        legalNatureId = Funcoes.strToInt(ddlNatureza.SelectedValue.ToString()),
                        mccId = Funcoes.strToInt(ReaderCadastro["COD_ID_MCC"].ToString()),
                        //TpvExpected = (ReaderCadastro["FLG_TIPO_PESSOA"].ToString().Trim() == "PF") ? Funcoes.strToInt(ReaderCadastro["NUM_RENDA_MENSAL"].ToString().Trim()) : Funcoes.strToInt(ReaderCadastro["NUM_FATURAMENTO"].ToString().Trim()),
                        companyName = (ReaderCadastro["FLG_TIPO_PESSOA"].ToString().Trim() == "PF") ? ReaderCadastro["NOM_NOME"].ToString().Trim() + " " + ReaderCadastro["NOM_SOBRENOME"].ToString().Trim() : ReaderCadastro["NOM_RAZAOSOCIAL"].ToString().Trim(),
                        tradingName = (ReaderCadastro["FLG_TIPO_PESSOA"].ToString().Trim() == "PF") ? ReaderCadastro["NOM_NOME"].ToString().Trim() + " " + ReaderCadastro["NOM_SOBRENOME"].ToString().Trim() : ReaderCadastro["NOM_RAZAOSOCIAL"].ToString().Trim()
                    },
                    address = new DadosRevendedor.Address()
                    {
                        streetName = ReaderCadastro["NOM_ENDERECO"].ToString().Trim(),
                        houseNumber = ReaderCadastro["NOM_NUMERO"].ToString().Trim(),
                        complement = ReaderCadastro["NOM_COMPLEMENTO"].ToString().Trim(),
                        neighborhood = ReaderCadastro["NOM_BAIRRO"].ToString().Trim(),
                        city = ReaderCadastro["NOM_CIDADE"].ToString().Trim(),
                        postalCode = TIRAACENTOS(ReaderCadastro["NOM_CEP"].ToString().Trim()),
                        state = ReaderCadastro["NOM_UF"].ToString().Trim()
                    },
                    responsible = new DadosRevendedor.Responsible()
                    {
                        cpf = TIRAACENTOS(ReaderCadastro["NOM_CPF"].ToString().Trim()),
                        email = ReaderCadastro["NOM_EMAIL"].ToString().Trim(),
                        name = ReaderCadastro["NOM_NOME"].ToString().Trim() + " " + ReaderCadastro["NOM_SOBRENOME"].ToString().Trim(),
                        mobilePhone = TIRAACENTOS(ReaderCadastro["NOM_CELULAR"].ToString().Trim()),
                        phone = "1155737555"//TIRAACENTOS(ReaderCadastro["NUM_TELEFONE"].ToString().Trim()).Substring(1,10),
                    },
                    bankAccount = new DadosRevendedor.BankAccount()
                    {
                        accountType = (ReaderCadastro["NOM_TIPO_BANCO_PADRAO"].ToString().Trim() == "C") ? 1 : 2,
                        account = TIRAACENTOS(ReaderCadastro["NOM_NUMERO_CONTA_BANCO_PADRAO"].ToString().Trim()) + TIRAACENTOS(ReaderCadastro["NOM_NUMERO_DIGITO_CONTA_BANCO_PADRAO"].ToString().Trim()),
                        bankCode = TIRAACENTOS(ReaderCadastro["NOM_CODIGO_BANCO_PADRAO"].ToString().Trim()),
                        branch = TIRAACENTOS(ReaderCadastro["NOM_NUMERO_AGENCIA_BANCO_PADRAO"].ToString().Trim())
                    }
                };
                jsonRevendedor = JsonConvert.SerializeObject(dRevendedor);
            }

            string jsonRetorno = hubcappta.CadastrarRevendedor(jsonRevendedor);
            txtResposta.Text = jsonRevendedor.ToString(); //jsonRetorno.ToString();

            JObject oRevendedor = JObject.Parse(jsonRetorno.ToString());
            try
            {
                txtTokenCappta.Text = oRevendedor["merchantDocument"].ToString();
                txtStatusCappta.Text = oRevendedor["statusDescription"].ToString();

                ClientScript.RegisterStartupScript(this.GetType(),
                    "Alerta", "alert('Os dados foram enviados com sucesso - Representante: " + oRevendedor["merchantDocument"].ToString() + "');", true);
            }
            catch
            {
                ClientScript.RegisterStartupScript(this.GetType(),
                    "Alerta", "alert('Ocorreu um erro ao tentar enviar os dados do Representante! Verifique e reentre.');", true);
            }

            SqlConnection connInsCons = new SqlConnection(Funcoes.conexao());
            connInsCons.Open();
            SqlCommand cmdInsCons = new SqlCommand("dbo.stp_pessoas_fj_cappta_ins", connInsCons);
            cmdInsCons.CommandType = CommandType.StoredProcedure;
            cmdInsCons.Parameters.Add("@flg_operacao", SqlDbType.Char).Value = 'I';
            cmdInsCons.Parameters.Add("@COD_ID_PESSOA_LICENCIADO", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["LICENCIADO"].ToString());
            cmdInsCons.Parameters.Add("@COD_ID_PESSOAS_FJ", SqlDbType.Int).Value = Funcoes.strToInt(sid_id.ToString());

            cmdInsCons.Parameters.Add("@NUM_TOKEN_CAPPTA", SqlDbType.VarChar).Value = txtTokenCappta.Text.ToString();
            cmdInsCons.Parameters.Add("@FLG_STATUS_CAPPTA", SqlDbType.VarChar).Value = txtStatusCappta.Text.ToString();
            cmdInsCons.Parameters.Add("@FLG_CAPPTA", SqlDbType.Char).Value = ddlCappta.SelectedValue.ToString();
            cmdInsCons.Parameters.Add("@DES_JSON_CAPPTA", SqlDbType.VarChar).Value = txtResposta.Text.ToString();
            cmdInsCons.Parameters.Add("@COD_ID_NATUREZA_CAPPTA", SqlDbType.Int).Value = ddlNatureza.SelectedValue.ToString();

            cmdInsCons.ExecuteNonQuery();
            connInsCons.Close();
            connInsCons.Dispose();


            ClientScript.RegisterStartupScript(this.GetType(),
                "Alerta", "alert('Dados gravados com sucesso');", true);
        }
    }


    // -----------------------------------------------------------------------------------------------------------------------------------------------------------------
    // SETOR ZOOP 
    // -----------------------------------------------------------------------------------------------------------------------------------------------------------------



    // -----------------------------------------------------------------------------------------------------------------------------------------------------------------
    // SETOR PAGSEGURO 
    // -----------------------------------------------------------------------------------------------------------------------------------------------------------------



    // -----------------------------------------------------------------------------------------------------------------------------------------------------------------
    // SETOR CONTAS BANCÁRIAS
    // -----------------------------------------------------------------------------------------------------------------------------------------------------------------

    protected void rptConsultaContas_ItemCommand(object source, RepeaterCommandEventArgs e)
    {
        if (e.CommandName == "Excluir")
        {
            SqlConnection connInsCons = new SqlConnection(Funcoes.conexao());
            SqlCommand cmdInsCons = new SqlCommand("dbo.stp_pessoas_fj_contas_ins", connInsCons);
            cmdInsCons.CommandType = CommandType.StoredProcedure;
            cmdInsCons.Parameters.Add("@flg_operacao", SqlDbType.Char).Value = 'E';
            cmdInsCons.Parameters.Add("@COD_ID", SqlDbType.Int).Value = Funcoes.strToInt(Convert.ToString(e.CommandArgument));
            cmdInsCons.Parameters.Add("@COD_ID_PESSOA_LICENCIADO", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["LICENCIADO"].ToString());
            connInsCons.Open();
            cmdInsCons.ExecuteNonQuery();

            ClientScript.RegisterStartupScript(this.GetType(),
            "ExclusaoContas", "alert('Registro excluído com sucesso');", true);

            ConsultaGeral();
        }
        if (e.CommandName == "Padrao")
        {
            SqlConnection connInsCons = new SqlConnection(Funcoes.conexao());
            SqlCommand cmdInsCons = new SqlCommand("dbo.stp_pessoas_fj_contas_ins", connInsCons);
            cmdInsCons.CommandType = CommandType.StoredProcedure;
            cmdInsCons.Parameters.Add("@flg_operacao", SqlDbType.Char).Value = 'U';
            cmdInsCons.Parameters.Add("@COD_ID", SqlDbType.Int).Value = Funcoes.strToInt(Convert.ToString(e.CommandArgument));
            cmdInsCons.Parameters.Add("@COD_ID_PESSOAS_FJ", SqlDbType.Int).Value = Funcoes.strToInt(sid_id.ToString());
            cmdInsCons.Parameters.Add("@COD_ID_PESSOA_LICENCIADO", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["LICENCIADO"].ToString());
            connInsCons.Open();
            cmdInsCons.ExecuteNonQuery();

            ClientScript.RegisterStartupScript(this.GetType(),
            "PadraoContas", "alert('Registro alterado com sucesso');", true);

            ConsultaGeral();
        }

    }


    protected void btnIncluirContaBancaria_Click(object sender, EventArgs e)
    {
        SqlConnection mySelCadastro = new SqlConnection(Funcoes.conexao());
        mySelCadastro.Open();
        SqlCommand cmdSelCadastro = new SqlCommand("dbo.stp_pessoas_fj_contas_ins", mySelCadastro);
        cmdSelCadastro.CommandType = CommandType.StoredProcedure;
        cmdSelCadastro.Parameters.Add("@flg_operacao", SqlDbType.Char).Value = "L";
        cmdSelCadastro.Parameters.Add("@COD_ID_PESSOA_LICENCIADO", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["LICENCIADO"].ToString());
        cmdSelCadastro.Parameters.Add("@COD_ID_PESSOAS_FJ", SqlDbType.Int).Value = Funcoes.strToInt(sid_id.ToString());

        cmdSelCadastro.Parameters.Add("@NOM_CODIGO_BANCO", SqlDbType.VarChar).Value = ddlInstituicaoFinanceira.SelectedValue.ToString();
        cmdSelCadastro.Parameters.Add("@NOM_TIPO_BANCO", SqlDbType.VarChar).Value = ddlTipoConta.SelectedValue.ToString();
        cmdSelCadastro.Parameters.Add("@NOM_NUMERO_AGENCIA_BANCO", SqlDbType.VarChar).Value = txtAgencia.Text.ToString();
        cmdSelCadastro.Parameters.Add("@NOM_NUMERO_DIGITO_AGENCIA_BANCO", SqlDbType.VarChar).Value = txtDigitoAgencia.Text.ToString();
        cmdSelCadastro.Parameters.Add("@NOM_NUMERO_CONTA_BANCO", SqlDbType.VarChar).Value = txtConta.Text.ToString();
        cmdSelCadastro.Parameters.Add("@NOM_NUMERO_DIGITO_CONTA_BANCO", SqlDbType.VarChar).Value = txtDigitoConta.Text.ToString();

        SqlDataReader ReaderCadastro = cmdSelCadastro.ExecuteReader();
        while (ReaderCadastro.Read())
        {
            ClientScript.RegisterStartupScript(this.GetType(),
                "Alerta", "alert('Já existe outra conta bancária com estes dados! Verifique e reentre.'); ", true);

            return;
        }
        // Salva Primeiro cadastro da Pessoa F/J
        if ((txtAgencia.Text.ToString().Trim() != "") && (txtConta.Text.ToString().Trim() != "") && (ddlInstituicaoFinanceira.SelectedValue.ToString().Trim() != ""))
        {
            SqlConnection connInsCons = new SqlConnection(Funcoes.conexao());
            connInsCons.Open();
            SqlCommand cmdInsCons = new SqlCommand("dbo.stp_pessoas_fj_contas_ins", connInsCons);
            cmdInsCons.CommandType = CommandType.StoredProcedure;
            cmdInsCons.Parameters.Add("@flg_operacao", SqlDbType.Char).Value = 'I';
            cmdInsCons.Parameters.Add("@COD_ID_PESSOA_LICENCIADO", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["LICENCIADO"].ToString());
            cmdInsCons.Parameters.Add("@COD_ID_PESSOAS_FJ", SqlDbType.Int).Value = Funcoes.strToInt(sid_id.ToString());

            cmdInsCons.Parameters.Add("@NOM_CODIGO_BANCO", SqlDbType.VarChar).Value = ddlInstituicaoFinanceira.SelectedValue.ToString();
            cmdInsCons.Parameters.Add("@NOM_TIPO_BANCO", SqlDbType.VarChar).Value = ddlTipoConta.SelectedValue.ToString();
            cmdInsCons.Parameters.Add("@NOM_NUMERO_AGENCIA_BANCO", SqlDbType.VarChar).Value = txtAgencia.Text.ToString();
            cmdInsCons.Parameters.Add("@NOM_NUMERO_DIGITO_AGENCIA_BANCO", SqlDbType.VarChar).Value = txtDigitoAgencia.Text.ToString();
            cmdInsCons.Parameters.Add("@NOM_NUMERO_CONTA_BANCO", SqlDbType.VarChar).Value = txtConta.Text.ToString();
            cmdInsCons.Parameters.Add("@NOM_NUMERO_DIGITO_CONTA_BANCO", SqlDbType.VarChar).Value = txtDigitoConta.Text.ToString();

            cmdInsCons.Parameters.Add("@FLG_PADRAO", SqlDbType.Char).Value = ddlPadrao.SelectedValue.ToString();

            cmdInsCons.ExecuteNonQuery();
            connInsCons.Close();
            connInsCons.Dispose();

            ClientScript.RegisterStartupScript(this.GetType(),
                "Alerta", "alert('Dados gravados com sucesso'); ", true);

            // Enviar dados para ZOOP

            // Fim do envio dos dados para zoop


            txtAgencia.Text = "";
            txtDigitoAgencia.Text = "";
            txtConta.Text = "";
            txtDigitoConta.Text = "";

            ConsultaGeral();
        }
        else
        {
            ClientScript.RegisterStartupScript(this.GetType(),
                "Obrigatorio", "alert('Todos os campos são de preenchimento obrigatório! Não foi possível incluir a conta bancária.'); ", true);
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
    protected void btnSalvarZoop_Click(object sender, EventArgs e)
    {

    }
}