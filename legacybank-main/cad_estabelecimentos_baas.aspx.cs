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


public partial class cad_estabelecimentos_baas : System.Web.UI.Page
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
            Funcoes.GravaAuditoria(Funcoes.strToInt(sid_id), Request.ServerVariables["REMOTE_ADDR"].ToString(), "Cadastro - Estabelecimentos - Conta Digital", "Acesso - ID: " + Funcoes.strToInt(sid_id).ToString());


            if (Funcoes.strToInt(sid_id) != 0)
            {
                ConsultaFicha();
            }
        }

    }

    private void ConsultaFicha()
    {
        SqlConnection mySelCadastroPFJ = new SqlConnection(Funcoes.conexao());
        mySelCadastroPFJ.Open();
        SqlCommand cmdSelCadastroPFJ = new SqlCommand("dbo.stp_pessoas_fj_ins", mySelCadastroPFJ);
        cmdSelCadastroPFJ.CommandType = CommandType.StoredProcedure;
        cmdSelCadastroPFJ.Parameters.Add("@flg_operacao", SqlDbType.Char).Value = "S";
        cmdSelCadastroPFJ.Parameters.Add("@COD_ID", SqlDbType.Int).Value = Funcoes.strToInt(sid_id.ToString());
        cmdSelCadastroPFJ.Parameters.Add("@COD_ID_PESSOA_LICENCIADO", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["LICENCIADO"].ToString());
        SqlDataReader ReaderCadastroPFJ = cmdSelCadastroPFJ.ExecuteReader();
        while (ReaderCadastroPFJ.Read())
        {
            txtID.Text = ReaderCadastroPFJ["COD_ID"].ToString();
            txtRazaosocial.Text = ReaderCadastroPFJ["NOM_RAZAOSOCIAL"].ToString();
            txtEmail.Text = ReaderCadastroPFJ["NOM_EMAIL"].ToString();
            txtTipo.Text = ReaderCadastroPFJ["NOM_FLG_TIPO_PESSOA"].ToString();
            if (ReaderCadastroPFJ["FLG_TIPO_PESSOA"].ToString().Trim() == "PF")
            {
                txtDocumento.Text = ReaderCadastroPFJ["NOM_CPF"].ToString();
            }
            else
            {
                txtDocumento.Text = ReaderCadastroPFJ["NOM_CNPJ"].ToString();

            }

        }
        
        SqlConnection mySelCadastro = new SqlConnection(Funcoes.conexao());
        mySelCadastro.Open();
        SqlCommand cmdSelCadastro = new SqlCommand("dbo.stp_pessoas_fj_baas_ins", mySelCadastro);
        cmdSelCadastro.CommandType = CommandType.StoredProcedure;
        cmdSelCadastro.Parameters.Add("@flg_operacao", SqlDbType.Char).Value = "S";
        cmdSelCadastro.Parameters.Add("@COD_ID_PESSOAS_FJ", SqlDbType.Int).Value = Funcoes.strToInt(sid_id.ToString());
        cmdSelCadastro.Parameters.Add("@COD_ID_PESSOA_LICENCIADO", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["LICENCIADO"].ToString());
        cmdSelCadastro.Parameters.Add("@FLG_ORIGEM", SqlDbType.Char).Value = "A";

        SqlDataReader ReaderCadastro = cmdSelCadastro.ExecuteReader();
        while (ReaderCadastro.Read())
        {
            ddlBaaS.SelectedValue = ReaderCadastro["FLG_BAAS"].ToString();
            txtToken.Text = ReaderCadastro["NUM_TOKEN_BAAS"].ToString();
            txtStatus.Text = ReaderCadastro["FLG_STATUS_BAAS"].ToString();
            txtNomeStatus.Text = ReaderCadastro["NOM_FLG_STATUS_BAAS"].ToString();
            txtResposta.Text = ReaderCadastro["DES_JSON_BAAS"].ToString();

            if ((ddlBaaS.SelectedValue.ToString().Trim() == "S") && txtToken.Text.ToString().Trim() != "")
            {
                txtToken.Enabled = false;
            }

            if ((ddlBaaS.SelectedValue.ToString().Trim() == "S") && (txtToken.Text.ToString().Trim() != "") && (txtIDConta.Text.ToString().Trim() != ""))
            {
                txtIDConta.Text = ReaderCadastro["NUM_ID_CONTA_BAAS"].ToString();
                txtAgencia.Text = ReaderCadastro["NUM_AGENCIA_BAAS"].ToString();
                txtConta.Text = ReaderCadastro["NUM_CONTA_BAAS"].ToString();
                txtDigitoConta.Text = ReaderCadastro["NUM_DIGITO_CONTA_BAAS"].ToString();
                txtWalletID.Text = ReaderCadastro["NUM_WALLETID_BAAS"].ToString();
            }
            else
            {
                if (txtToken.Text.ToString().Trim() != "")
                {
                    // Consultar conta pelo email
                    string ConsultaConta = asaas.ListarSubcontasCNPJCPF(TIRAACENTOS(txtDocumento.Text.ToString()));
                    txtResposta.Text = ConsultaConta.ToString();

                    if (ConsultaConta.ToString().Trim() != "")
                    {
                        JObject oConta = JObject.Parse(ConsultaConta.ToString());
                        txtIDConta.Text = oConta["data"][0]["id"].ToString();
                        txtWalletID.Text = oConta["data"][0]["walletId"].ToString();
                        txtAgencia.Text = oConta["data"][0]["accountNumber"]["agency"].ToString();
                        txtConta.Text = oConta["data"][0]["accountNumber"]["account"].ToString();
                        txtDigitoConta.Text = oConta["data"][0]["accountNumber"]["accountDigit"].ToString();
                    }
                }
            }

            txtChavePix.Text = ReaderCadastro["NUM_CHAVE_PIX"].ToString();

            if (txtToken.Text.ToString().Trim() != "")
            {
                string ConsultaStatus = asaas.ListarStatus(txtToken.Text.ToString());
                txtResposta.Text = ConsultaStatus.ToString();
                JObject oStatus = JObject.Parse(ConsultaStatus.ToString());
                txtStatus.Text = oStatus["general"].ToString();
            }

            txtIDConta.Enabled = false;
            txtAgencia.Enabled = false;
            txtConta.Enabled = false;
            txtDigitoConta.Enabled = false;
            txtNomeStatus.Enabled = false;
            txtChavePix.Enabled = false;

        }

    }

    private void GravarDadosBaaS()
    {

        Funcoes.GravaAuditoria(0, Request.ServerVariables["REMOTE_ADDR"].ToString(), "Cadastro - Estabelecimentos - Conta Digital", "Acesso - ID: " + Funcoes.strToInt(sid_id.ToString()).ToString() + " - " + Request.ServerVariables["SERVER_NAME"].ToString());
        
        // Salva Primeiro cadastro da Pessoa F/J

        SqlConnection connInsCons = new SqlConnection(Funcoes.conexao());
        connInsCons.Open();
        SqlCommand cmdInsCons = new SqlCommand("dbo.stp_pessoas_fj_baas_ins", connInsCons);
        cmdInsCons.CommandType = CommandType.StoredProcedure;
        cmdInsCons.Parameters.Add("@flg_operacao", SqlDbType.Char).Value = "I";
        cmdInsCons.Parameters.Add("@COD_ID_PESSOA_LICENCIADO", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["LICENCIADO"].ToString());
        cmdInsCons.Parameters.Add("@COD_ID_PESSOAS_FJ", SqlDbType.Int).Value = Funcoes.strToInt(sid_id.ToString());
        cmdInsCons.Parameters.Add("@FLG_ORIGEM", SqlDbType.Char).Value = "A";

        cmdInsCons.Parameters.Add("@NUM_TOKEN_BAAS", SqlDbType.VarChar).Value = txtToken.Text.ToString();
        cmdInsCons.Parameters.Add("@FLG_STATUS_BAAS", SqlDbType.VarChar).Value = txtStatus.Text.ToString();
        cmdInsCons.Parameters.Add("@FLG_BAAS", SqlDbType.Char).Value = ddlBaaS.SelectedValue.ToString();
        cmdInsCons.Parameters.Add("@DES_JSON_BAAS", SqlDbType.VarChar).Value = txtResposta.Text.ToString();

        cmdInsCons.Parameters.Add("@NUM_WALLETID_BAAS", SqlDbType.VarChar).Value = txtWalletID.Text.ToString();
        cmdInsCons.Parameters.Add("@NUM_ID_CONTA_BAAS", SqlDbType.VarChar).Value = txtIDConta.Text.ToString();
        cmdInsCons.Parameters.Add("@NUM_AGENCIA_BAAS", SqlDbType.VarChar).Value = txtAgencia.Text.ToString();
        cmdInsCons.Parameters.Add("@NUM_CONTA_BAAS", SqlDbType.VarChar).Value = txtConta.Text.ToString();
        cmdInsCons.Parameters.Add("@NUM_DIGITO_CONTA_BAAS", SqlDbType.VarChar).Value = txtDigitoConta.Text.ToString();

        cmdInsCons.Parameters.Add("@NUM_CHAVE_PIX", SqlDbType.VarChar).Value = txtChavePix.Text.ToString();
        

        cmdInsCons.ExecuteNonQuery();
        connInsCons.Close();
        connInsCons.Dispose();


        ClientScript.RegisterStartupScript(this.GetType(),
            "Alerta", "alert('Dados gravados com sucesso');", true);

    }


    private void CadastrarBaaS()
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

            string sCompanyType = "";
            if (ReaderCadastro["FLG_TIPO_PESSOA"].ToString().Trim() == "PJ")
            {
                if (ReaderCadastro["NOM_TIPO_EMPRESA"].ToString() == "MEI") { sCompanyType = "MEI"; }
                if (ReaderCadastro["NOM_TIPO_EMPRESA"].ToString() == "Empresário Individual") { sCompanyType = "INDIVIDUAL"; }
                if (ReaderCadastro["NOM_TIPO_EMPRESA"].ToString() == "Sociedade Limitada Unipessoal") { sCompanyType = "LIMITED"; }
                if (ReaderCadastro["NOM_TIPO_EMPRESA"].ToString() == "Sociedade Empresária Limitada") { sCompanyType = "LIMITED"; }
                if (ReaderCadastro["NOM_TIPO_EMPRESA"].ToString() == "Sociedade Simples") { sCompanyType = "LIMITED"; }
                if (ReaderCadastro["NOM_TIPO_EMPRESA"].ToString() == "Sociedade Anônima") { sCompanyType = "LIMITED"; }
            }

            dadosSubconta.Subconta dsubconta = new dadosSubconta.Subconta()
            {
                name = (ReaderCadastro["FLG_TIPO_PESSOA"].ToString().Trim() == "PF") ? ReaderCadastro["NOM_NOME"].ToString().Trim() + " " + ReaderCadastro["NOM_SOBRENOME"].ToString().Trim() : ReaderCadastro["NOM_RAZAOSOCIAL"].ToString().Trim(),
                email = ReaderCadastro["NOM_EMAIL"].ToString().Trim(),
                loginEmail = ReaderCadastro["NOM_EMAIL"].ToString().Trim(),
                birthDate = (ReaderCadastro["FLG_TIPO_PESSOA"].ToString().Trim() == "PF") ? String.Format("{0:yyyy-MM-dd}", Convert.ToDateTime(ReaderCadastro["DTA_ANIVERSARIO"].ToString().Trim())) : "",
                cpfCnpj = (ReaderCadastro["FLG_TIPO_PESSOA"].ToString().Trim() == "PF") ? TIRAACENTOS(ReaderCadastro["NOM_CPF"].ToString().Trim()) : TIRAACENTOS(ReaderCadastro["NOM_CNPJ"].ToString().Trim()),
                companyType = sCompanyType.ToString(),
                mobilePhone = TIRAACENTOS(ReaderCadastro["NOM_CELULAR"].ToString().Trim()),
                incomeValue = (ReaderCadastro["FLG_TIPO_PESSOA"].ToString().Trim() == "PF") ? Funcoes.strToInt(ReaderCadastro["NUM_RENDA_MENSAL"].ToString().Trim()) : Funcoes.strToInt(ReaderCadastro["NUM_FATURAMENTO"].ToString().Trim()),
                address = ReaderCadastro["NOM_ENDERECO"].ToString().Trim(),
                addressNumber = ReaderCadastro["NOM_NUMERO"].ToString().Trim(),
                complement = ReaderCadastro["NOM_COMPLEMENTO"].ToString().Trim(),
                province = ReaderCadastro["NOM_BAIRRO"].ToString().Trim(),
                postalCode = TIRAACENTOS(ReaderCadastro["NOM_CEP"].ToString().Trim()),
                webhooks = new dadosSubconta.webhooks[]
                {
                    new dadosSubconta.webhooks()
                    {
                    name = "WEBHOOK-ID-"+ReaderCadastro["COD_ID"].ToString().Trim().PadLeft(6,'0'),
                    url = "https://conta.legacybank.com.br/events/asaas",
                    email = (HttpContext.Current.Session["EMAIL"].ToString().Trim()!="")? HttpContext.Current.Session["EMAIL"].ToString().Trim() : ReaderCadastro["NOM_EMAIL"].ToString().Trim(),
                    sendType = "SEQUENTIALLY",
                    apiVersion = 3,
                    enabled = true,
                    interrupted = false,
                    authToken = Funcoes.alfanumericoAleatorio(20).ToString(),
                    events = asaas.ListaWebhook
                    }
                }
            };
            string jsonSubconta = JsonConvert.SerializeObject(dsubconta);

            //ClientScript.RegisterStartupScript(this.GetType(),
            //    "json", "alert('"+jsonSubconta.ToString()+"');", true);
            //txtResposta.Text = jsonSubconta.ToString();
            //return;
            string jsonRetorno = asaas.CriarSubconta(jsonSubconta);
            //txtResposta.Text = txtResposta.Text + " - " + jsonRetorno;

            // tratar os dados de retorno
            JObject o = JObject.Parse(jsonRetorno.ToString()); 
            try
            {
                txtToken.Text = o["apiKey"].ToString();
                txtIDConta.Text = o["id"].ToString();
                txtAgencia.Text = o["accountNumber"]["agency"].ToString();
                txtConta.Text = o["accountNumber"]["account"].ToString();
                txtDigitoConta.Text = o["accountNumber"]["accountDigit"].ToString();
                txtWalletID.Text = o["walletId"].ToString();

                ClientScript.RegisterStartupScript(this.GetType(),
                    "Alerta", "alert('A conta foi criada com sucesso');", true);
            }
            catch
            {
                //txtToken.Text = o["errors"][0]["description"].ToString();
                ClientScript.RegisterStartupScript(this.GetType(),
                    "Alerta", "alert('Ocorreu um erro ao tentar criar a conta do estabelecimento - erro: " + o["errors"][0]["description"].ToString()  + "');", true);

            }
        }
    }

    
    protected void btnSalvar_Click(object sender, EventArgs e)
    {
        Funcoes.GravaAuditoria(Funcoes.strToInt(sid_id), Request.ServerVariables["REMOTE_ADDR"].ToString(), "Cadastro - Estabelecimentos - Conta Digital", "Gravar - ID: " + Funcoes.strToInt(sid_id).ToString());

        if ((ddlBaaS.SelectedValue.ToString().Trim() == "S"))
        {
            if ((txtToken.Text.ToString().Trim() == ""))
            {
                CadastrarBaaS();
            }
            GravarDadosBaaS();
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

    protected void btnCancelar_Click(object sender, EventArgs e)
    {
        ClientScript.RegisterStartupScript(this.GetType(),
    "Alerta", "opener.PostBackOnMainPage(); window.close(); ", true);

    }
    protected void btnPendentes_Click(object sender, EventArgs e)
    {
        txtResposta.Text = asaas.ListarDocumentosPendentes(txtToken.Text.ToString());
    }
    protected void btnTaxas_Click(object sender, EventArgs e)
    {
        txtResposta.Text = asaas.ListarTaxas(txtToken.Text.ToString());

    }

    protected void lbkGerar_Click(object sender, EventArgs e)
    {
        if (txtChavePix.Text.ToString().Trim() == "")
        {
            string jsonChavePix = "";
            jsonChavePix = jsonChavePix + "{";
            jsonChavePix = jsonChavePix + "\"type\":\"" + "EVP" + "\",";
            jsonChavePix = jsonChavePix + "}";

            string sRetornoChavePix =  asaas.CriarChavePix(txtToken.Text.ToString(), jsonChavePix.ToString());
            JObject oChavePix = JObject.Parse(sRetornoChavePix.ToString());

            try
            {
                SqlConnection connInsCons = new SqlConnection(Funcoes.conexao());
                connInsCons.Open();
                SqlCommand cmdInsCons = new SqlCommand("dbo.stp_pessoas_fj_baas_ins", connInsCons);
                cmdInsCons.CommandType = CommandType.StoredProcedure;
                cmdInsCons.Parameters.Add("@flg_operacao", SqlDbType.Char).Value = 'P';
                cmdInsCons.Parameters.Add("@COD_ID_PESSOA_LICENCIADO", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["LICENCIADO"].ToString());
                cmdInsCons.Parameters.Add("@COD_ID_PESSOAS_FJ", SqlDbType.Int).Value = Funcoes.strToInt(sid_id.ToString());
                cmdInsCons.Parameters.Add("@FLG_ORIGEM", SqlDbType.Char).Value = "A";

                cmdInsCons.Parameters.Add("@NUM_CHAVE_PIX", SqlDbType.VarChar).Value = oChavePix["key"].ToString();
                cmdInsCons.Parameters.Add("@NUM_ID_CHAVE_PIX", SqlDbType.VarChar).Value = oChavePix["id"].ToString();
                cmdInsCons.Parameters.Add("@FLG_STATUS_CHAVE_PIX", SqlDbType.VarChar).Value = oChavePix["status"].ToString();
                cmdInsCons.Parameters.Add("@NOM_IMAGEM_CHAVE_PIX", SqlDbType.VarChar).Value = oChavePix["qrCode"]["encodedImage"].ToString();
                cmdInsCons.Parameters.Add("@NOM_QRCODE_CHAVE_PIX", SqlDbType.VarChar).Value = oChavePix["qrCode"]["payload"].ToString();
                cmdInsCons.Parameters.Add("@DES_JSON_CHAVE_PIX", SqlDbType.Text).Value = sRetornoChavePix.ToString();

                cmdInsCons.ExecuteNonQuery();
                connInsCons.Close();
                connInsCons.Dispose();


                txtChavePix.Text = oChavePix["key"].ToString();

                ClientScript.RegisterStartupScript(this.GetType(),
                    "Alerta", "alert('A chave PIX foi criada com sucesso!');", true);
            }
            catch
            {
                try
                {
                    //txtToken.Text = o["errors"][0]["description"].ToString();
                    ClientScript.RegisterStartupScript(this.GetType(),
                        "ErroChavePix", "alert('Ocorreu um erro ao tentar criar a chave PIX - erro: " + oChavePix["errors"][0]["description"].ToString() + "!');", true);
                }
                catch
                {
                    ClientScript.RegisterStartupScript(this.GetType(),
                        "ErroMensagem", "alert('Ocorreu um erro ao tentar criar a chave PIX!');", true);

                }

            }

        }
        else
        {
            ClientScript.RegisterStartupScript(this.GetType(),
                "Alerta", "alert('a chave PIX já existe no BAAS!');", true);
        }
    }
}