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


public partial class cad_marketplaces_onboarding : System.Web.UI.Page
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
            // Verificar Integrações
            CarregaAdquirentes();

            if (Funcoes.strToInt(sid_id) != 0)
            {
                ConsultaFicha();
                ConsultaGeral();
                ddlAdquirentes_SelectedIndexChanged(null, null);
            }

        }
    }

    private void CarregaAdquirentes()
    {
        SqlConnection myAdquirentes = new SqlConnection(Funcoes.conexao());
        myAdquirentes.Open();
        SqlCommand cmdAdquirentes = new SqlCommand("dbo.stp_pessoas_fj_integracoes_ins", myAdquirentes);
        cmdAdquirentes.CommandType = CommandType.StoredProcedure;
        if ((HttpContext.Current.Session["TIPO"].ToString() == "L"))
        {
            cmdAdquirentes.Parameters.Add("@flg_operacao", SqlDbType.VarChar).Value = "G";
        }
        else
        {
            cmdAdquirentes.Parameters.Add("@flg_operacao", SqlDbType.VarChar).Value = "M";
        }

        cmdAdquirentes.Parameters.Add("@COD_ID_PESSOA_LICENCIADO", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["LICENCIADO"].ToString());
        cmdAdquirentes.Parameters.Add("@FLG_ATIVO", SqlDbType.Char).Value = "S";
        cmdAdquirentes.Parameters.Add("@FLG_ACESSO", SqlDbType.Char).Value = "S";
        SqlDataAdapter drAdquirentes = new SqlDataAdapter();
        drAdquirentes.SelectCommand = cmdAdquirentes;
        DataSet dsAdquirentes = new DataSet();
        drAdquirentes.Fill(dsAdquirentes, "PESSOAS_FJ_INTEGRACOES");

        ddlAdquirentes.DataTextField = "NOM_FANTASIA_INTEGRACAO";
        ddlAdquirentes.DataValueField = "FLG_INTEGRACAO";
        ddlAdquirentes.DataSource = dsAdquirentes.Tables["PESSOAS_FJ_INTEGRACOES"].DefaultView;
        ddlAdquirentes.DataBind();
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
        }

    }

    protected void btnFechar_Click(object sender, EventArgs e)
    {
        ClientScript.RegisterStartupScript(this.GetType(), "FecharOnboarding", "opener.PostBackOnMainPage(); window.close(); ", true);

    }


    private void ConsultaGeral()
    {

    }

    // -----------------------------------------------------------------------------------------------------------------------------------------------------------------
    // SETOR CAPPTA 
    // -----------------------------------------------------------------------------------------------------------------------------------------------------------------

    private void ConsultaCappta()
    {
        try
        {
            ConsultaTabelasCapta();

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
        catch
        {
            ClientScript.RegisterStartupScript(this.GetType(), "ErroCarregaDadosCappta", "alert('Ocorreu um erro ao tentar carregar os dados da adquirente! Verifique e tente novamente!');", true);
        }
    }


    private void ConsultaTabelasCapta()
    {
        try
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
        catch
        {
            ClientScript.RegisterStartupScript(this.GetType(), "ErroTabelaCappta", "alert('Ocorreu um erro ao tentar carregar as tabelas da adquirente! Verifique e tente novamente!');", true);
        }

    }


    protected void btnSalvarCappta_Click(object sender, EventArgs e)
    {


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

    protected void ddlAdquirentes_SelectedIndexChanged(object sender, EventArgs e)
    {

        divAdquirenteCappta.Visible = false;

        if (ddlAdquirentes.SelectedValue.ToString() == "C")
        {
            divAdquirenteCappta.Visible = true;
            ConsultaCappta();
        }
    }
    protected void btnSalvar_Click(object sender, EventArgs e)
    {

        int iContas = 0;
        SqlConnection mySelCadastroContas = new SqlConnection(Funcoes.conexao());
        mySelCadastroContas.Open();
        SqlCommand cmdSelCadastroContas = new SqlCommand("dbo.stp_pessoas_fj_contas_ins", mySelCadastroContas);
        cmdSelCadastroContas.CommandType = CommandType.StoredProcedure;
        cmdSelCadastroContas.Parameters.Add("@flg_operacao", SqlDbType.Char).Value = "C";
        cmdSelCadastroContas.Parameters.Add("@COD_ID_PESSOAS_FJ", SqlDbType.Int).Value = Funcoes.strToInt(sid_id.ToString());
        cmdSelCadastroContas.Parameters.Add("@COD_ID_PESSOA_LICENCIADO", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["LICENCIADO"].ToString());
        SqlDataReader ReaderCadastroContas = cmdSelCadastroContas.ExecuteReader();
        while (ReaderCadastroContas.Read())
        {
            iContas = iContas + 1;
        }

        if (iContas > 0)
        {
            if (ddlAdquirentes.SelectedValue.ToString() == "C")
            {
                GravarDadosCappta();
            }
        }
        else
        {
            ClientScript.RegisterStartupScript(this.GetType(), "ErroContasInexistentes", "alert('Não existem contas bancárias cadastradas! Verifique e tente novamente!');", true);
        }


    }

    private void GravarDadosCappta()
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
            DadosRevendedor.Root drevendedor = new DadosRevendedor.Root()
            {
                reseller = new DadosRevendedor.Reseller()
                {
                    document = (ReaderCadastro["FLG_TIPO_PESSOA"].ToString().Trim() == "PF") ? TIRAACENTOS(ReaderCadastro["NOM_CPF"].ToString().Trim()) : TIRAACENTOS(ReaderCadastro["NOM_CNPJ"].ToString().Trim()),
                    legalNatureId = Funcoes.strToInt(ddlNatureza.SelectedValue.ToString()),
                    mccId = Funcoes.strToInt(ReaderCadastro["COD_ID_MCC"].ToString()),
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
                    phone = "0000000000"//TIRAACENTOS(ReaderCadastro["NUM_TELEFONE"].ToString().Trim()).Substring(1,10),
                },
                bankAccount = new DadosRevendedor.BankAccount()
                {
                    accountType = (ReaderCadastro["NOM_TIPO_BANCO_PADRAO"].ToString().Trim() == "C") ? 1 : 2,
                    account = TIRAACENTOS(ReaderCadastro["NOM_NUMERO_CONTA_BANCO_PADRAO"].ToString().Trim()) + TIRAACENTOS(ReaderCadastro["NOM_NUMERO_DIGITO_CONTA_BANCO_PADRAO"].ToString().Trim()),
                    bankCode = TIRAACENTOS(ReaderCadastro["NOM_CODIGO_BANCO_PADRAO"].ToString().Trim()),
                    branch = TIRAACENTOS(ReaderCadastro["NOM_NUMERO_AGENCIA_BANCO_PADRAO"].ToString().Trim())
                }
            };
            jsonRevendedor = JsonConvert.SerializeObject(drevendedor);
        }

        string jsonResposta = hubcappta.CadastrarRevendedor(jsonRevendedor);
        txtResposta.Text = jsonResposta;
        txtResposta.Visible = true;

        if (jsonResposta.ToString().Trim() != "")
        {
            JObject oRevendedor = JObject.Parse(jsonResposta.ToString());
            try
            {
                txtTokenCappta.Text = oRevendedor["resellerDocument"].ToString();

                txtStatusCappta.Text = oRevendedor["statusDescription"].ToString();
                txtNomeStatusCappta.Text = oRevendedor["statusDescription"].ToString();

                ClientScript.RegisterStartupScript(this.GetType(), "Alerta", "alert('Dados gravados e enviados com sucesso');", true);
            }
            catch
            {
                try
                {
                    ClientScript.RegisterStartupScript(this.GetType(), "ErroCadastrarRevendedor", "alert('Ocorreu um erro ao tentar salvar os dados do Marketplace! " + oRevendedor["errorMessage"].ToString() + "');", true);
                }
                catch
                {
                    ClientScript.RegisterStartupScript(this.GetType(), "ErroCadastrarRevendedorGeral", "alert('Ocorreu um erro ao tentar salvar os dados do Marketplace!');", true);
                }

            }

        }

        SqlConnection connInsCons = new SqlConnection(Funcoes.conexao());
        connInsCons.Open();
        SqlCommand cmdInsCons = new SqlCommand("dbo.stp_pessoas_fj_cappta_ins", connInsCons);
        cmdInsCons.CommandType = CommandType.StoredProcedure;
        cmdInsCons.Parameters.Add("@flg_operacao", SqlDbType.Char).Value = 'I';
        cmdInsCons.Parameters.Add("@COD_ID_PESSOA_LICENCIADO", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["LICENCIADO"].ToString());
        cmdInsCons.Parameters.Add("@COD_ID_PESSOAS_FJ", SqlDbType.Int).Value = Funcoes.strToInt(sid_id.ToString());

        cmdInsCons.Parameters.Add("@NUM_TOKEN_CAPPTA", SqlDbType.VarChar).Value = txtTokenCappta.Text.ToString();
        if (txtStatusCappta.Text.ToString().Trim() != "")
        {
            cmdInsCons.Parameters.Add("@FLG_STATUS_CAPPTA", SqlDbType.VarChar).Value = txtStatusCappta.Text.ToString();
        }
        else
        {
            cmdInsCons.Parameters.Add("@FLG_STATUS_CAPPTA", SqlDbType.VarChar).Value = txtNomeStatusCappta.Text.ToString();
        }
        cmdInsCons.Parameters.Add("@FLG_CAPPTA", SqlDbType.Char).Value = ddlCappta.SelectedValue.ToString();
        cmdInsCons.Parameters.Add("@DES_JSON_CAPPTA", SqlDbType.VarChar).Value = txtResposta.Text.ToString();
        cmdInsCons.Parameters.Add("@COD_ID_NATUREZA_CAPPTA", SqlDbType.Int).Value = ddlNatureza.SelectedValue.ToString();

        cmdInsCons.ExecuteNonQuery();
        connInsCons.Close();
        connInsCons.Dispose();


    }
}