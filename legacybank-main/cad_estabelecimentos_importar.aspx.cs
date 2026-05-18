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

public partial class cad_estabelecimentos_importar : System.Web.UI.Page
{
    public static DataTable dtEstabelecimentos;

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
        
        btnSalvar.Visible = true;

        if (!IsPostBack)
        {

            // Definição Tabela de Estabelecimentos
            dtEstabelecimentos = new DataTable();
            dtEstabelecimentos.Columns.Add("documento", typeof(string));
            dtEstabelecimentos.Columns.Add("nome", typeof(string));
            dtEstabelecimentos.Columns.Add("status", typeof(string));


            SqlConnection myMCC = new SqlConnection(Funcoes.conexao());
            myMCC.Open();
            SqlCommand cmdMCC = new SqlCommand("dbo.stp_mcc_ins", myMCC);
            cmdMCC.CommandType = CommandType.StoredProcedure;
            cmdMCC.Parameters.Add("@flg_operacao", SqlDbType.VarChar).Value = "L";
            SqlDataAdapter drMCC = new SqlDataAdapter();
            drMCC.SelectCommand = cmdMCC;
            DataSet dsMCC = new DataSet();
            drMCC.Fill(dsMCC, "MCC");
            ddlAtividadeEconomica.DataTextField = "NOM_MCC";
            ddlAtividadeEconomica.DataValueField = "COD_ID";
            ddlAtividadeEconomica.DataSource = dsMCC.Tables["MCC"].DefaultView;
            ddlAtividadeEconomica.DataBind();
            ddlAtividadeEconomica.Items.Insert(0, new ListItem("", "0"));


            SqlConnection myRepresentante = new SqlConnection(Funcoes.conexao());
            myRepresentante.Open();
            SqlCommand cmdRepresentante = new SqlCommand("dbo.stp_pessoas_fj_ins", myRepresentante);
            cmdRepresentante.CommandType = CommandType.StoredProcedure;
            cmdRepresentante.Parameters.Add("@flg_operacao", SqlDbType.VarChar).Value = "L";
            cmdRepresentante.Parameters.Add("@COD_ID_PESSOA_LICENCIADO", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["LICENCIADO"].ToString());
            cmdRepresentante.Parameters.Add("@FLG_ATIVO", SqlDbType.Char).Value = "S";
            cmdRepresentante.Parameters.Add("@FLG_TIPO", SqlDbType.Char).Value = "R";
            if (HttpContext.Current.Session["TIPO"].ToString().Trim() == "M")
            {
                cmdRepresentante.Parameters.Add("@COD_ID_MARKETPLACE", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["PESSOA"].ToString());
            }


            SqlDataAdapter drRepresentante = new SqlDataAdapter();
            drRepresentante.SelectCommand = cmdRepresentante;
            DataSet dsRepresentante = new DataSet();
            drRepresentante.Fill(dsRepresentante, "PESSOAS_FJ");

            ddlRepresentante.DataTextField = "NOM_RAZAOSOCIAL";
            ddlRepresentante.DataValueField = "COD_ID";
            ddlRepresentante.DataSource = dsRepresentante.Tables["PESSOAS_FJ"].DefaultView;
            ddlRepresentante.DataBind();
            ddlRepresentante.Items.Insert(0, new ListItem("", "0"));

            
            // MARKETPLACES

            SqlConnection myMarketplace = new SqlConnection(Funcoes.conexao());
            myMarketplace.Open();
            SqlCommand cmdMarketplace = new SqlCommand("dbo.stp_pessoas_fj_ins", myMarketplace);
            cmdMarketplace.CommandType = CommandType.StoredProcedure;
            cmdMarketplace.Parameters.Add("@flg_operacao", SqlDbType.VarChar).Value = "L";
            cmdMarketplace.Parameters.Add("@COD_ID_PESSOA_LICENCIADO", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["LICENCIADO"].ToString());
            cmdMarketplace.Parameters.Add("@FLG_ATIVO", SqlDbType.Char).Value = "S";
            cmdMarketplace.Parameters.Add("@FLG_TIPO", SqlDbType.Char).Value = "M";
            if (HttpContext.Current.Session["TIPO"].ToString().Trim() == "M")
            {
                cmdMarketplace.Parameters.Add("@COD_ID_MARKETPLACE", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["PESSOA"].ToString());
            }


            SqlDataAdapter drMarketplace = new SqlDataAdapter();
            drMarketplace.SelectCommand = cmdMarketplace;
            DataSet dsMarketplace = new DataSet();
            drMarketplace.Fill(dsMarketplace, "PESSOAS_FJ");

            ddlMarketplaceCappta.DataTextField = "NOM_RAZAOSOCIAL";
            ddlMarketplaceCappta.DataValueField = "NUM_DOCUMENTO_IDENTIFICACAO";
            ddlMarketplaceCappta.DataSource = dsMarketplace.Tables["PESSOAS_FJ"].DefaultView;
            ddlMarketplaceCappta.DataBind();
            ddlMarketplaceCappta.Items.Insert(0, new ListItem("", ""));
            
            dvUsuario.Visible = true;

            // Verificar Integrações
            CarregaAdquirentes();
            ddlAdquirentes_SelectedIndexChanged(null, null);
            ConsultaIntegracoes();

        }

    }
    private void CarregaAdquirentes()
    {
        /*
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
        */

        SqlConnection myAdquirentes = new SqlConnection(Funcoes.conexao());
        myAdquirentes.Open();
        SqlCommand cmdAdquirentes = new SqlCommand("dbo.stp_pessoas_fj_integracoes_ins", myAdquirentes);
        cmdAdquirentes.CommandType = CommandType.StoredProcedure;
        if (HttpContext.Current.Session["TIPO"].ToString() == "L")
        {
            cmdAdquirentes.Parameters.Add("@flg_operacao", SqlDbType.VarChar).Value = "G";
        }
        else
        {
            cmdAdquirentes.Parameters.Add("@flg_operacao", SqlDbType.VarChar).Value = "M";
        }

        cmdAdquirentes.Parameters.Add("@COD_ID_PESSOA_LICENCIADO", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["LICENCIADO"].ToString());
        cmdAdquirentes.Parameters.Add("@COD_ID_PESSOAS_FJ", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["PESSOA"].ToString());
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
        ddlAdquirentes.Items.Insert(0, new ListItem("Todos", ""));



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
        }

        if ((tbcapptatab.Visible == true) && (iContador == 0)) { tbcapptatab.Attributes.Add("class", "nav-link active"); tbcappta.Attributes.Add("class", "tab-pane fade show active"); iContador = iContador + 1; }
        if ((tbzooptab.Visible == true) && (iContador == 0)) { tbzooptab.Attributes.Add("class", "nav-link active"); tbzoop.Attributes.Add("class", "tab-pane fade show active"); iContador = iContador + 1; }
    }

    private void ConsultaFicha()
    {
        try
        {
            var json = zoop.ConsultaCadastroSeller(Funcoes.TIRAACENTOSDOCUMENTOS(txtCNPJ.Text.ToString()));
            if (json.ToString().Trim() != "")
            {

                JObject o = JObject.Parse(json);

                if (ddlTipoFJ.SelectedValue.ToString() == "PF")
                {
                    //txtJson.Text = txtJson.Text + o["id"].ToString();
                    txtToken.Text = o["id"].ToString();
                    txtNome.Text = o["first_name"].ToString();
                    txtSobrenome.Text = o["last_name"].ToString();
                    txtRazaoSocial.Text = o["description"].ToString();
                    txtFantasia.Text = o["description"].ToString();

                    txtDocumento.Text = o["taxpayer_id"].ToString();
                    txtTelefoneEmpresa.Text = o["phone_number"].ToString();
                    txtCelular.Text = o["phone_number"].ToString();

                    txtEndereco.Text = o["address"]["line1"].ToString();
                    txtNumero.Text = o["address"]["line2"].ToString();
                    txtComplemento.Text = o["address"]["line3"].ToString();
                    txtBairro.Text = o["address"]["neighborhood"].ToString();
                    txtCidade.Text = o["address"]["city"].ToString();
                    ddlEstado.SelectedValue = o["address"]["state"].ToString();
                    txtCEP.Text = o["address"]["postal_code"].ToString();

                    txtNascimento.Text = Convert.ToDateTime(o["birthdate"].ToString()).ToShortDateString();
                    txtDataAbertura.Text = Convert.ToDateTime(o["birthdate"].ToString()).ToShortDateString();
                    txtEmailEmpresa.Text = o["email"].ToString();
                    txtEmailResponsavel.Text = o["email"].ToString();

                    txtNomeUsuario.Text = o["description"].ToString();
                    txtEmail.Text = o["email"].ToString();
                    txtEmailC.Text = o["email"].ToString();
                    txtSenha.Text = Funcoes.GetRandomPassword(8);
                    txtSenhaC.Text = txtSenha.Text.ToString();

                }
                if (ddlTipoFJ.SelectedValue.ToString() == "PJ")
                {
                    //txtJson.Text = txtJson.Text + o["id"].ToString();
                    txtToken.Text = o["id"].ToString();
                    txtNome.Text = o["owner"]["first_name"].ToString();
                    txtSobrenome.Text = o["owner"]["last_name"].ToString();
                    txtRazaoSocial.Text = o["business_name"].ToString();
                    txtFantasia.Text = o["business_description"].ToString();

                    txtDocumento.Text = o["owner"]["taxpayer_id"].ToString();
                    txtTelefoneEmpresa.Text = o["business_phone"].ToString();
                    txtCelular.Text = o["owner"]["phone_number"].ToString();

                    txtEndereco.Text = o["business_address"]["line1"].ToString();
                    txtNumero.Text = o["business_address"]["line2"].ToString();
                    txtComplemento.Text = o["business_address"]["line3"].ToString();
                    txtBairro.Text = o["business_address"]["neighborhood"].ToString();
                    txtCidade.Text = o["business_address"]["city"].ToString();
                    ddlEstado.SelectedValue = o["business_address"]["state"].ToString();
                    txtCEP.Text = o["business_address"]["postal_code"].ToString();
                    if (o["owner"]["birthdate"].ToString().Trim() != "")
                    {
                        txtNascimento.Text = Convert.ToDateTime(o["owner"]["birthdate"].ToString()).ToShortDateString();
                    }
                    if (o["business_opening_date"].ToString().Trim() != "")
                    {
                        txtDataAbertura.Text = Convert.ToDateTime(o["business_opening_date"].ToString()).ToShortDateString();
                    }

                    txtEmailEmpresa.Text = o["business_email"].ToString();
                    txtEmailResponsavel.Text = o["owner"]["email"].ToString();

                    txtNomeUsuario.Text = o["business_description"].ToString();
                    txtEmail.Text = o["business_email"].ToString();
                    txtEmailC.Text = o["business_email"].ToString();
                    txtSenha.Text = Funcoes.GetRandomPassword(8);
                    txtSenhaC.Text = txtSenha.Text.ToString();

                }
            }

            btnSalvar.Visible = true;

            SqlConnection mySelCadastro = new SqlConnection(Funcoes.conexao());
            mySelCadastro.Open();
            SqlCommand cmdSelCadastro = new SqlCommand("dbo.stp_pessoas_fj_ins", mySelCadastro);
            cmdSelCadastro.CommandType = CommandType.StoredProcedure;
            cmdSelCadastro.Parameters.Add("@flg_operacao", SqlDbType.Char).Value = "W";
            cmdSelCadastro.Parameters.Add("@NUM_TOKEN", SqlDbType.VarChar).Value = txtToken.Text.ToString();
            cmdSelCadastro.Parameters.Add("@NUM_DOCUMENTO", SqlDbType.VarChar).Value = txtCNPJ.Text.ToString();
            cmdSelCadastro.Parameters.Add("@FLG_TIPO_PESSOA", SqlDbType.Char).Value = ddlTipoFJ.SelectedValue.ToString();
            cmdSelCadastro.Parameters.Add("@COD_ID_PESSOA_LICENCIADO", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["LICENCIADO"].ToString());


            SqlDataReader ReaderCadastro = cmdSelCadastro.ExecuteReader();
            while (ReaderCadastro.Read())
            {
                txtID.Text = ReaderCadastro["COD_ID_PESSOAS_FJ"].ToString();
                btnSalvar.Visible = false;
                ClientScript.RegisterStartupScript(this.GetType(),
        "Duplicidade", "alert('Este estabelecimento já se encontra cadastrado na base de dados. Por favor verifique.');", true);
            }
            if (txtToken.Text.ToString().Trim() != "")
            {
                ConsultarTransacoes();
                ConsultarContas();
            }

            ClientScript.RegisterStartupScript(this.GetType(),
    "SucessoImportacao", "alert('Dados importados com sucesso!');", true);
        }
        catch
        {
            ClientScript.RegisterStartupScript(this.GetType(),
    "ErroImportacao", "alert('Erro ao importar os dados do CNPJ/CPF Informado! Verifique e tente novamente');", true);

        }

    }

    private void ConsultarContas()
    {
        try
        {

            var json = zoop.ConsultaContasSeller(txtToken.Text.ToString());
            JObject o = JObject.Parse(json);
            
            txtContas.Text = o["total"].ToString();
            if (o["has_more"].ToString()=="true")
            {
                txtPaginasContas.Text = o["total_pages"].ToString();
            }
            else
            {
                txtPaginasContas.Text = "1";
            }

            if (Funcoes.strToInt(txtContas.Text.ToString()) > 0)
            {
                btnContas.Visible = true;
            }

        }
        catch
        {
            ClientScript.RegisterStartupScript(this.GetType(),
    "Erro", "alert('Alguma coisa deu errado ao consultar as contas bancárias! Verifique os dados digitados e tente novamente.');", true);

        }
    }


    private void ConsultarTransacoes()
    {
        try
        {
            var json = zoop.ConsultaTransacoesSeller(txtToken.Text.ToString(),"0","100");

            //txtJson.Text = json.ToString();
            JObject o = JObject.Parse(json);
            txtTransacoes.Text = o["total"].ToString();
            txtPaginas.Text = o["total_pages"].ToString();

            if (Funcoes.strToInt(txtTransacoes.Text.ToString()) > 0)
            {
                btnTransacoes.Visible = true;
            }

        }
        catch
        {
            ClientScript.RegisterStartupScript(this.GetType(),
    "Erro", "alert('Alguma coisa deu errado ao consultar as transações! Verifique os dados digitados e tente novamente.');", true);

        }

    }

    protected void btnSalvar_Click(object sender, EventArgs e)
    {
        if (txtID.Text.ToString().Trim() == "")
        {
            if (((txtEmail.Text.ToString().Trim() == txtEmail.Text.ToString().Trim()) && (txtSenha.Text.ToString().Trim() == txtSenhaC.Text.ToString().Trim())))
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
                cmdInsCons.Parameters.Add("@FLG_TIPO", SqlDbType.Char).Value = "E";

                cmdInsCons.Parameters.Add("@NUM_TOKEN", SqlDbType.VarChar).Value = txtToken.Text.ToString();

                cmdInsCons.Parameters.Add("@FLG_TIPO_PESSOA", SqlDbType.VarChar).Value = ddlTipoFJ.SelectedValue.ToString();
                cmdInsCons.Parameters.Add("@FLG_PRESENCIAL", SqlDbType.Char).Value = ddlPresencial.SelectedValue.ToString();

                if ((HttpContext.Current.Session["TIPO"].ToString().Trim() == "M"))
                {
                    cmdInsCons.Parameters.Add("@COD_ID_MARKETPLACE", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["PESSOA"].ToString());
                }


                cmdInsCons.Parameters.Add("@COD_ID_REPRESENTANTE", SqlDbType.Int).Value = Funcoes.strToInt(ddlRepresentante.SelectedValue.ToString());

                // Empresa
                cmdInsCons.Parameters.Add("@NOM_RAZAOSOCIAL", SqlDbType.VarChar).Value = txtRazaoSocial.Text.ToString();
                cmdInsCons.Parameters.Add("@NOM_FANTASIA", SqlDbType.VarChar).Value = txtFantasia.Text.ToString();
                cmdInsCons.Parameters.Add("@NOM_CNPJ", SqlDbType.VarChar).Value = txtCNPJ.Text.ToString();
                cmdInsCons.Parameters.Add("@NUM_TELEFONE", SqlDbType.VarChar).Value = txtTelefoneEmpresa.Text.ToString();
                cmdInsCons.Parameters.Add("@NOM_EMAIL_EMPRESA", SqlDbType.VarChar).Value = txtEmailEmpresa.Text.ToString();
                cmdInsCons.Parameters.Add("@COD_ID_MCC", SqlDbType.Int).Value = Funcoes.strToInt(ddlAtividadeEconomica.SelectedValue.ToString());
                cmdInsCons.Parameters.Add("@NOM_TIPO_EMPRESA", SqlDbType.VarChar).Value = ddlTipoEmpresa.SelectedValue.ToString();
                cmdInsCons.Parameters.Add("@NUM_FATURAMENTO", SqlDbType.Float).Value = Funcoes.strToDouble(txtFaturamento.Text.ToString());
                cmdInsCons.Parameters.Add("@NUM_PATRIMONIO", SqlDbType.Float).Value = Funcoes.strToDouble(txtPatrimonio.Text.ToString());
                if (txtDataAbertura.Text.ToString().Trim() != "")
                {
                    cmdInsCons.Parameters.Add("@DTA_ABERTURA", SqlDbType.DateTime).Value = Convert.ToDateTime(txtDataAbertura.Text.ToString());
                }

                // Endereço
                cmdInsCons.Parameters.Add("@NOM_ENDERECO", SqlDbType.VarChar).Value = txtEndereco.Text.ToString();
                cmdInsCons.Parameters.Add("@NOM_NUMERO", SqlDbType.VarChar).Value = txtNumero.Text.ToString();
                cmdInsCons.Parameters.Add("@NOM_COMPLEMENTO", SqlDbType.VarChar).Value = txtComplemento.Text.ToString();
                cmdInsCons.Parameters.Add("@NOM_BAIRRO", SqlDbType.VarChar).Value = txtBairro.Text.ToString();
                cmdInsCons.Parameters.Add("@NOM_CIDADE", SqlDbType.VarChar).Value = txtCidade.Text.ToString();
                cmdInsCons.Parameters.Add("@NOM_UF", SqlDbType.VarChar).Value = ddlEstado.SelectedValue.ToString();
                cmdInsCons.Parameters.Add("@NOM_CEP", SqlDbType.VarChar).Value = txtCEP.Text.ToString();
                cmdInsCons.Parameters.Add("@NOM_PAIS", SqlDbType.VarChar).Value = "BR";

                // Responsável
                cmdInsCons.Parameters.Add("@NOM_NOME", SqlDbType.VarChar).Value = txtNome.Text.ToString();
                cmdInsCons.Parameters.Add("@NOM_SOBRENOME", SqlDbType.VarChar).Value = txtSobrenome.Text.ToString();
                cmdInsCons.Parameters.Add("@NOM_CPF", SqlDbType.VarChar).Value = txtDocumento.Text.ToString();
                if (txtNascimento.Text.ToString().Trim() != "")
                {
                    cmdInsCons.Parameters.Add("@DTA_ANIVERSARIO", SqlDbType.DateTime).Value = Convert.ToDateTime(txtNascimento.Text.ToString());
                }
                cmdInsCons.Parameters.Add("@NOM_MAE", SqlDbType.VarChar).Value = txtNomeMae.Text.ToString();
                cmdInsCons.Parameters.Add("@NUM_RENDA_MENSAL", SqlDbType.Float).Value = Funcoes.strToDouble(txtRendaMensalPF.Text.ToString());
                cmdInsCons.Parameters.Add("@NOM_EMAIL", SqlDbType.VarChar).Value = txtEmailResponsavel.Text.ToString();
                cmdInsCons.Parameters.Add("@NOM_CELULAR", SqlDbType.VarChar).Value = txtCelular.Text.ToString();
                cmdInsCons.Parameters.Add("@FLG_POLITICAMENTE", SqlDbType.Char).Value = ddlPoliticamenteExposta.SelectedValue.ToString();

                // Usuário
                if (Funcoes.strToInt(sid_id.ToString().Trim()) == 0)
                {
                    cmdInsCons.Parameters.Add("@NOM_NOME_USUARIO", SqlDbType.VarChar).Value = txtNomeUsuario.Text.ToString();
                    cmdInsCons.Parameters.Add("@NOM_LOGIN", SqlDbType.VarChar).Value = txtEmail.Text.ToString();
                    cmdInsCons.Parameters.Add("@NOM_SENHA", SqlDbType.VarChar).Value = txtSenha.Text.ToString();
                }

                cmdInsCons.ExecuteNonQuery();
                connInsCons.Close();
                connInsCons.Dispose();

                ClientScript.RegisterStartupScript(this.GetType(),
                    "Alerta", "alert('Dados gravados com sucesso');", true);
            }
            else
            {
                ClientScript.RegisterStartupScript(this.GetType(),
                    "Usuario", "alert('E-mail ou Senha do Usuário são diferentes dos dados de confirmação! Verifique e reentre.');", true);
            }
        }
        else
        {
            ClientScript.RegisterStartupScript(this.GetType(),
                "Duplicidade", "alert('Estabelecimento já se encontra cadastrado na base de dados! Verifique e reentre.');", true);

        }
    }
    protected void btnCancelar_Click(object sender, EventArgs e)
    {
        ClientScript.RegisterStartupScript(this.GetType(),
            "Alerta", "opener.PostBackOnMainPage(); window.close(); ", true);
    }

    protected void txtCEP_TextChanged(object sender, EventArgs e)
    {
        if (txtCEP.Text.ToString().Trim() != "")
        {
            ServicePointManager.Expect100Continue = true;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12
            | SecurityProtocolType.Tls11
            | SecurityProtocolType.Tls
            | SecurityProtocolType.Ssl3;

            // Convert to Base64

            var myUri = new Uri("https://viacep.com.br/ws/" + txtCEP.Text.ToString() + "/json");
            var myWebRequest = WebRequest.Create(myUri);
            var myHttpWebRequest = (HttpWebRequest)myWebRequest;
            myHttpWebRequest.Accept = "application/json";

            var myWebResponse = myWebRequest.GetResponse();
            var responseStream = myWebResponse.GetResponseStream();

            //if (responseStream == null) return null;

            StreamReader myStreamReader = new StreamReader(responseStream, Encoding.UTF8);
            var json = myStreamReader.ReadToEnd();

            dadosCEP.CEPInfo m = JsonSerializer.Deserialize<dadosCEP.CEPInfo>(json);
            txtEndereco.Text = m.logradouro;
            txtBairro.Text = m.bairro;
            txtCidade.Text = m.localidade;
            ddlEstado.SelectedValue = m.uf;

            responseStream.Close();
            myWebResponse.Close();
        }

    }
    protected void ddlTipoFJ_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlTipoFJ.SelectedValue.ToString() == "PF")
        {
            ClientScript.RegisterStartupScript(this.GetType(),
                "CNPJCPF", "$('#txtCNPJ').prop('placeholder', '000.000.000-00');", true);
            lblRazaoSocial.Text = "Nome Completo";
            lblFantasia.Text = "Apelido";
            lblCNPJ.Text = "CPF";
            lblDataAbertura.Text = "Data Início Atividade";
        }
        else
        {
            ClientScript.RegisterStartupScript(this.GetType(),
            "CNPJCPF", "$('#txtCNPJ').prop('placeholder', '00.000.000/0000-00');", true);
            lblRazaoSocial.Text = "Razão Social";
            lblFantasia.Text = "Nome Fantasia";
            lblCNPJ.Text = "CNPJ";
            lblDataAbertura.Text = "Data Abertura Empresa";
        }

    }
    protected void btnImportar_Click(object sender, EventArgs e)
    {
        ConsultaFicha();
    }
    protected void btnTransacoes_Click(object sender, EventArgs e)
    {
        // Localizar os dados do Estabelecimento
        int iEstabelecimento = 0;
        SqlConnection mySelCadastro = new SqlConnection(Funcoes.conexao());
        mySelCadastro.Open();
        SqlCommand cmdSelCadastro = new SqlCommand("dbo.stp_pessoas_fj_ins", mySelCadastro);
        cmdSelCadastro.CommandType = CommandType.StoredProcedure;
        cmdSelCadastro.Parameters.Add("@flg_operacao", SqlDbType.Char).Value = "W";
        cmdSelCadastro.Parameters.Add("@NUM_TOKEN", SqlDbType.VarChar).Value = txtToken.Text.ToString();
        cmdSelCadastro.Parameters.Add("@NUM_DOCUMENTO", SqlDbType.VarChar).Value = txtCNPJ.Text.ToString();
        SqlDataReader ReaderCadastro = cmdSelCadastro.ExecuteReader();
        while (ReaderCadastro.Read())
        {
            txtID.Text = ReaderCadastro["COD_ID_PESSOAS_FJ"].ToString();
            iEstabelecimento = Funcoes.strToInt(ReaderCadastro["COD_ID_PESSOAS_FJ"].ToString());
        }
        if (txtID.Text.ToString().Trim() != "")
        {

            //try
            //{
                int iOffSet = 0;
                int iLimit = 100;
                bool bhas_more = true;
                while (bhas_more == true)
                {
                    var json = zoop.ConsultaTransacoesSeller(txtToken.Text.ToString(),iOffSet.ToString(),iLimit.ToString());
                    JObject o = JObject.Parse(json);

                    bhas_more = Convert.ToBoolean(o["has_more"].ToString());

                    for (int i = 0; i < o["items"].Count(); i++)
                    {

                        SqlConnection connInsCons = new SqlConnection(Funcoes.conexao());
                        connInsCons.Open();
                        SqlCommand cmdInsCons = new SqlCommand("dbo.stp_transacoes_ins", connInsCons);
                        cmdInsCons.CommandType = CommandType.StoredProcedure;
                        cmdInsCons.Parameters.Add("@flg_operacao", SqlDbType.Char).Value = 'I';
                        cmdInsCons.Parameters.Add("@COD_ID_PESSOA_LICENCIADO", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["LICENCIADO"].ToString());
                        cmdInsCons.Parameters.Add("@COD_ID_PESSOAS_FJ", SqlDbType.Int).Value = iEstabelecimento;
                        cmdInsCons.Parameters.Add("@FLG_ORIGEM", SqlDbType.Char).Value = "Z";

                        cmdInsCons.Parameters.Add("@DTA_DATA", SqlDbType.DateTime).Value = Convert.ToDateTime(o["items"][i]["created_at"].ToString());
                        cmdInsCons.Parameters.Add("@NOM_CODE", SqlDbType.VarChar).Value = o["items"][i]["id"].ToString();
                        cmdInsCons.Parameters.Add("@NUM_TOKEN", SqlDbType.VarChar).Value = o["items"][i]["on_behalf_of"].ToString();


                        cmdInsCons.Parameters.Add("@COD_ID_TIPO", SqlDbType.Int).Value = 1;
                        cmdInsCons.Parameters.Add("@NOM_STATUS", SqlDbType.VarChar).Value = o["items"][i]["status"].ToString();
                        if (o["items"][i].Contains("updated_at") == true)
                        {
                            cmdInsCons.Parameters.Add("@DTA_DATA_ULTIMA_ATUALIZACAO", SqlDbType.DateTime).Value = Convert.ToDateTime(o["items"][i]["updated_at"].ToString());
                        }

                        cmdInsCons.Parameters.Add("@NOM_TIPO_PAGAMENTO", SqlDbType.VarChar).Value = o["items"][i]["payment_type"].ToString();

                        if ((o["items"][i]["payment_type"].ToString() == "pix"))
                        {
                            cmdInsCons.Parameters.Add("@NOM_CODIGO_PAGAMENTO", SqlDbType.VarChar).Value = o["items"][i]["payment_method"]["provider"].ToString();
                        }
                        if ((o["items"][i]["payment_type"].ToString() == "credit") || (o["items"][i]["payment_type"].ToString() == "debit"))
                        {
                            cmdInsCons.Parameters.Add("@NOM_CODIGO_PAGAMENTO", SqlDbType.VarChar).Value = o["items"][i]["payment_method"]["resource"].ToString() + " " + o["items"][i]["payment_method"]["card_brand"].ToString();
                        }

                        cmdInsCons.Parameters.Add("@NUM_VALOR_BRUTO", SqlDbType.Float).Value = Funcoes.strToDouble(o["items"][i]["amount"].ToString()) / 100;
                        cmdInsCons.Parameters.Add("@NUM_VALOR_DESCONTO", SqlDbType.Float).Value = 0;
                        cmdInsCons.Parameters.Add("@NUM_VALOR_INTERMEDIACAO_RATE", SqlDbType.Float).Value = 0;
                        cmdInsCons.Parameters.Add("@NUM_VALOR_INTERMEDIACAO_FEE", SqlDbType.Float).Value = Funcoes.strToDouble(o["items"][i]["fees"].ToString()) / 100;
                        cmdInsCons.Parameters.Add("@NUM_VALOR_LIQUIDO", SqlDbType.Float).Value = Funcoes.strToDouble(o["items"][i]["amount"].ToString()) / 100;
                        cmdInsCons.Parameters.Add("@NUM_VALOR_EXTRAS", SqlDbType.Float).Value = 0;

                        cmdInsCons.Parameters.Add("@DTA_DATA_DEPOSITO", SqlDbType.DateTime).Value = Convert.ToDateTime(o["items"][i]["created_at"].ToString());

                        cmdInsCons.Parameters.Add("@NUM_ITENS", SqlDbType.Int).Value = 1;

                        cmdInsCons.Parameters.Add("@NOM_FONTE_CANCELAMENTO", SqlDbType.VarChar).Value = "";
                        cmdInsCons.Parameters.Add("@NOM_ORIGEM", SqlDbType.VarChar).Value = o["items"][i]["history"][1]["authorizer"].ToString();

                        if (o["items"][i]["payment_method"].Contains("holder_name") == true)
                        {
                            cmdInsCons.Parameters.Add("@NOM_CLIENTE", SqlDbType.VarChar).Value = o["items"][i]["payment_method"]["holder_name"].ToString();
                        }

                        cmdInsCons.Parameters.Add("@NOM_AUTORIZACAO", SqlDbType.VarChar).Value = o["items"][i]["transaction_number"].ToString();

                        cmdInsCons.ExecuteNonQuery();
                        connInsCons.Close();
                        connInsCons.Dispose();

                    }
                    if (bhas_more == true)
                    {
                        iOffSet = iOffSet + 100;
                    }
                }
                ClientScript.RegisterStartupScript(this.GetType(),
                    "Alerta", "alert('Lote Importado com sucesso:" + bhas_more.ToString() + "-" + iOffSet.ToString() + "');", true);

            //}
            //catch
            //{
            //    ClientScript.RegisterStartupScript(this.GetType(),
        //"Erro", "alert('Alguma coisa deu errado ao consultar as transações! Verifique os dados digitados e tente novamente.');", true);

            //}
        }
        else
        {
            ClientScript.RegisterStartupScript(this.GetType(),
    "Erro", "alert('O cadastro deve ser salvo primeiro para posteriormente realizar a importação das transações! Verifique os dados digitados e tente novamente.');", true);
        }
    }
    protected void btnContas_Click(object sender, EventArgs e)
    {

    }


    // Bloco das Novas IMPLEMENTAÇÕES ---------------------------------------------------------------------------------------------------------------------

    protected void btnFechar_Click(object sender, EventArgs e)
    {
        ClientScript.RegisterStartupScript(this.GetType(), "FecharOnboarding", "opener.PostBackOnMainPage(); window.close(); ", true);

    }
    protected void btnCarregarCadastrosPagseguro_Click(object sender, EventArgs e)
    {

    }
    protected void btnImportarPagseguro_Click(object sender, EventArgs e)
    {

    }
    protected void btnCarregarCadastrosAsaas_Click(object sender, EventArgs e)
    {

    }
    protected void btnImportarAsaas_Click(object sender, EventArgs e)
    {

    }
    protected void btnCarregarCadastrosZoop_Click(object sender, EventArgs e)
    {
        if (ddlAdquirentes.SelectedValue.ToString() == "Z")
        {
            int iOffSet = 0;
            int iLimit = 100;
            bool bhas_more = true;
            while (bhas_more == true)
            {
                var json = zoop.ConsultaVendedores(iLimit, iOffSet);
                JObject oEstabelecimentos = JObject.Parse(json);

                bhas_more = Convert.ToBoolean(oEstabelecimentos["has_more"].ToString());

                for (int i = 0; i < oEstabelecimentos["items"].Count(); i++)
                {
                    if (oEstabelecimentos["items"][i]["type"].ToString().Trim() == "business")
                    {
                        //txtJsonCappta.Text = txtJsonCappta.Text + " - " + oEstabelecimentos["items"][i]["ein"].ToString();
                        dtEstabelecimentos.Rows.Add(oEstabelecimentos["items"][i]["ein"].ToString(), oEstabelecimentos["items"][i]["business_name"].ToString(), oEstabelecimentos["items"][i]["status"].ToString());
                    }
                    else
                    {
                        dtEstabelecimentos.Rows.Add(oEstabelecimentos["items"][i]["taxpayer_id"].ToString(), oEstabelecimentos["items"][i]["first_name"].ToString() + " " + oEstabelecimentos["items"][i]["last_name"].ToString(), oEstabelecimentos["items"][i]["status"].ToString());
                    }
                }
                if (bhas_more == true)
                {
                    iOffSet = iOffSet + 100;
                }

            }
            this.lsvEstabelecimentosZoop.DataSource = dtEstabelecimentos;
            this.lsvEstabelecimentosZoop.DataBind();

            ClientScript.RegisterStartupScript(this.GetType(), "SucessoCarregarDadosZoop", "alert('Dados dos estabelecimentos carregados com sucesso');", true);
        }
    }
    protected void btnImportarZoop_Click(object sender, EventArgs e)
    {
        if (ddlAdquirentes.SelectedValue.ToString()=="Z")
        {
            foreach (ListViewItem itemP in lsvEstabelecimentosZoop.Items)
            {
                if (
                    (((CheckBox)itemP.FindControl("ckbImportar")).Checked == true) &&
                    (
                    ((TextBox)itemP.FindControl("txtStatus")).Text.ToString().Trim() == "enabled"
                    ||
                    ((TextBox)itemP.FindControl("txtStatus")).Text.ToString().Trim() == "pending"
                    )

                    )
                {
                    GravarDadosEstabelecimentoZoop(((TextBox)itemP.FindControl("txtDocumento")).Text.ToString());

                    ((CheckBox)itemP.FindControl("ckbImportar")).Checked = false;
                }
            }

            ckbTodosZoop.Checked = false;

            ClientScript.RegisterStartupScript(this.GetType(), "SucessoImportarZoop", "alert('Estabelecimentos selecionados importados com sucesso');", true);
        }

    }

    private void GravarDadosEstabelecimentoZoop(string sDocumento)
    {
        var json = zoop.ConsultaCadastroSeller(Funcoes.TIRAACENTOSDOCUMENTOS(sDocumento.ToString()));
        if (json.ToString().Trim() != "")
        {
            try
            {
                JObject o = JObject.Parse(json);
                //txtDocumentoZoop.Text = json.ToString();


                SqlConnection connInsCons = new SqlConnection(Funcoes.conexao());
                connInsCons.Open();
                SqlCommand cmdInsCons = new SqlCommand("dbo.stp_pessoas_fj_ins", connInsCons);

                cmdInsCons.CommandType = CommandType.StoredProcedure;
                cmdInsCons.CommandTimeout = 0;
                cmdInsCons.Parameters.Add("@flg_operacao", SqlDbType.Char).Value = 'I';
                cmdInsCons.Parameters.Add("@FLG_ATIVO", SqlDbType.Char).Value = "S";
                cmdInsCons.Parameters.Add("@FLG_INSERT_USUARIO", SqlDbType.Char).Value = "S";

                cmdInsCons.Parameters.Add("@COD_ID_PESSOA_LICENCIADO", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["LICENCIADO"].ToString());
                cmdInsCons.Parameters.Add("@DTA_DATA", SqlDbType.DateTime).Value = DateTime.Now;
                cmdInsCons.Parameters.Add("@FLG_TIPO", SqlDbType.Char).Value = "E";

                cmdInsCons.Parameters.Add("@NUM_TOKEN", SqlDbType.VarChar).Value = o["type"].ToString() == "individual" ? o["id"].ToString() : o["id"].ToString(); // txtToken.Text.ToString();

                cmdInsCons.Parameters.Add("@FLG_TIPO_PESSOA", SqlDbType.VarChar).Value = o["type"].ToString() == "individual" ? "PF" : "PJ"; // ddlTipoFJ.SelectedValue.ToString();
                cmdInsCons.Parameters.Add("@FLG_PRESENCIAL", SqlDbType.Char).Value = o["type"].ToString() == "individual" ? "S" : "S"; // ddlPresencial.SelectedValue.ToString();

                if ((HttpContext.Current.Session["TIPO"].ToString().Trim() == "M"))
                {
                    cmdInsCons.Parameters.Add("@COD_ID_MARKETPLACE", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["PESSOA"].ToString());
                }
                cmdInsCons.Parameters.Add("@COD_ID_REPRESENTANTE", SqlDbType.Int).Value = 0;



                // Empresa
                cmdInsCons.Parameters.Add("@NOM_RAZAOSOCIAL", SqlDbType.VarChar).Value = o["type"].ToString() == "individual" ? o["description"].ToString() : o["business_name"].ToString(); // txtRazaoSocial.Text.ToString();
                cmdInsCons.Parameters.Add("@NOM_FANTASIA", SqlDbType.VarChar).Value = o["type"].ToString() == "individual" ? o["description"].ToString() : o["business_description"].ToString(); // txtFantasia.Text.ToString();
                cmdInsCons.Parameters.Add("@NOM_CNPJ", SqlDbType.VarChar).Value = o["type"].ToString() == "individual" ? o["taxpayer_id"].ToString() : o["ein"].ToString(); // txtCNPJ.Text.ToString();
                cmdInsCons.Parameters.Add("@NUM_TELEFONE", SqlDbType.VarChar).Value = o["type"].ToString() == "individual" ? o["phone_number"].ToString() : o["business_phone"].ToString(); // txtTelefoneEmpresa.Text.ToString();
                cmdInsCons.Parameters.Add("@NOM_EMAIL_EMPRESA", SqlDbType.VarChar).Value = o["type"].ToString() == "individual" ? o["email"].ToString() : o["business_email"].ToString(); // txtEmailEmpresa.Text.ToString();

                cmdInsCons.Parameters.Add("@COD_ID_MCC", SqlDbType.Int).Value = o["type"].ToString() == "individual" ? o["mcc"].ToString() : o["mcc"].ToString(); // Funcoes.strToInt(ddlAtividadeEconomica.SelectedValue.ToString());
                cmdInsCons.Parameters.Add("@NOM_TIPO_EMPRESA", SqlDbType.VarChar).Value = ddlTipoEmpresa.SelectedValue.ToString();
                cmdInsCons.Parameters.Add("@NUM_FATURAMENTO", SqlDbType.Float).Value = o["type"].ToString() == "individual" ? Funcoes.strToDouble(o["revenue"].ToString()) : Funcoes.strToDouble(o["revenue"].ToString()); //Funcoes.strToDouble(txtFaturamento.Text.ToString());
                cmdInsCons.Parameters.Add("@NUM_PATRIMONIO", SqlDbType.Float).Value = 0;

                try
                {
                    if (o["type"].ToString() == "individual" ? o["birthdate"].ToString().Trim() != "" : o["business_opening_date"].ToString().Trim() != "")
                    {
                        cmdInsCons.Parameters.Add("@DTA_ABERTURA", SqlDbType.DateTime).Value = o["type"].ToString() == "individual" ? Convert.ToDateTime(Convert.ToDateTime(o["birthdate"].ToString()).ToShortDateString()) : Convert.ToDateTime(Convert.ToDateTime(o["business_opening_date"].ToString()).ToShortDateString()); //Convert.ToDateTime(txtDataAbertura.Text.ToString());
                    }
                }
                catch
                {

                }

                // Endereço
                cmdInsCons.Parameters.Add("@NOM_ENDERECO", SqlDbType.VarChar).Value = o["type"].ToString() == "individual" ? o["address"]["line1"].ToString() : o["business_address"]["line1"].ToString(); // txtEndereco.Text.ToString();
                cmdInsCons.Parameters.Add("@NOM_NUMERO", SqlDbType.VarChar).Value = o["type"].ToString() == "individual" ? o["address"]["line2"].ToString() : o["business_address"]["line2"].ToString(); // txtNumero.Text.ToString();
                cmdInsCons.Parameters.Add("@NOM_COMPLEMENTO", SqlDbType.VarChar).Value = o["type"].ToString() == "individual" ? o["address"]["line3"].ToString() : o["business_address"]["line3"].ToString(); // txtComplemento.Text.ToString();
                cmdInsCons.Parameters.Add("@NOM_BAIRRO", SqlDbType.VarChar).Value = o["type"].ToString() == "individual" ? o["address"]["neighborhood"].ToString() : o["business_address"]["neighborhood"].ToString(); // txtBairro.Text.ToString();
                cmdInsCons.Parameters.Add("@NOM_CIDADE", SqlDbType.VarChar).Value = o["type"].ToString() == "individual" ? o["address"]["city"].ToString() : o["business_address"]["city"].ToString(); // txtCidade.Text.ToString();
                cmdInsCons.Parameters.Add("@NOM_UF", SqlDbType.VarChar).Value = o["type"].ToString() == "individual" ? o["address"]["state"].ToString() : o["business_address"]["state"].ToString(); // ddlEstado.SelectedValue.ToString();
                cmdInsCons.Parameters.Add("@NOM_CEP", SqlDbType.VarChar).Value = o["type"].ToString() == "individual" ? o["address"]["postal_code"].ToString() : o["business_address"]["postal_code"].ToString(); // txtCEP.Text.ToString();
                cmdInsCons.Parameters.Add("@NOM_PAIS", SqlDbType.VarChar).Value = "BR";


                // Responsável
                cmdInsCons.Parameters.Add("@NOM_NOME", SqlDbType.VarChar).Value = o["type"].ToString() == "individual" ? o["first_name"].ToString() : o["owner"]["first_name"].ToString();
                cmdInsCons.Parameters.Add("@NOM_SOBRENOME", SqlDbType.VarChar).Value = o["type"].ToString() == "individual" ? o["last_name"].ToString() : o["owner"]["last_name"].ToString();
                cmdInsCons.Parameters.Add("@NOM_CPF", SqlDbType.VarChar).Value = o["type"].ToString() == "individual" ? o["taxpayer_id"].ToString() : o["owner"]["taxpayer_id"].ToString();
                try
                {
                    if (o["type"].ToString() == "individual" ? o["birthdate"].ToString().Trim() != "" : o["owner"]["birthdate"].ToString().Trim() != "")
                    {
                        cmdInsCons.Parameters.Add("@DTA_ANIVERSARIO", SqlDbType.DateTime).Value = o["type"].ToString() == "individual" ? Convert.ToDateTime(Convert.ToDateTime(o["birthdate"].ToString()).ToShortDateString()) : Convert.ToDateTime(Convert.ToDateTime(o["owner"]["birthdate"].ToString()).ToShortDateString()); // Convert.ToDateTime(txtNascimento.Text.ToString());
                    }
                }
                catch
                {

                }
                cmdInsCons.Parameters.Add("@NOM_MAE", SqlDbType.VarChar).Value = o["type"].ToString() == "individual" ? "" : ""; // txtNomeMae.Text.ToString();
                cmdInsCons.Parameters.Add("@NUM_RENDA_MENSAL", SqlDbType.Float).Value = o["type"].ToString() == "individual" ? 0 : 0; // Funcoes.strToDouble(txtRendaMensalPF.Text.ToString());
                cmdInsCons.Parameters.Add("@NOM_EMAIL", SqlDbType.VarChar).Value = o["type"].ToString() == "individual" ? o["email"].ToString() : o["owner"]["email"].ToString(); // txtEmailResponsavel.Text.ToString();
                cmdInsCons.Parameters.Add("@NOM_CELULAR", SqlDbType.VarChar).Value = o["type"].ToString() == "individual" ? o["phone_number"].ToString() : o["owner"]["phone_number"].ToString(); // txtCelular.Text.ToString();
                cmdInsCons.Parameters.Add("@FLG_POLITICAMENTE", SqlDbType.Char).Value = o["type"].ToString() == "individual" ? "N" : "N"; // ddlPoliticamenteExposta.SelectedValue.ToString();

                // Usuário
                if (Funcoes.strToInt(sid_id.ToString().Trim()) == 0)
                {
                    cmdInsCons.Parameters.Add("@NOM_NOME_USUARIO", SqlDbType.VarChar).Value = o["type"].ToString() == "individual" ? o["description"].ToString() : o["business_description"].ToString(); // txtNomeUsuario.Text.ToString();
                    cmdInsCons.Parameters.Add("@NOM_LOGIN", SqlDbType.VarChar).Value = o["type"].ToString() == "individual" ? o["email"].ToString() : o["business_email"].ToString(); // txtEmail.Text.ToString();
                    cmdInsCons.Parameters.Add("@NOM_SENHA", SqlDbType.VarChar).Value = o["type"].ToString() == "individual" ? Funcoes.GetRandomPassword(8) : Funcoes.GetRandomPassword(8); // txtSenha.Text.ToString();
                }

                cmdInsCons.ExecuteNonQuery();
                connInsCons.Close();
                connInsCons.Dispose();

                SqlConnection mySelCadastro = new SqlConnection(Funcoes.conexao());
                mySelCadastro.Open();
                SqlCommand cmdSelCadastro = new SqlCommand("dbo.stp_pessoas_fj_ins", mySelCadastro);
                cmdSelCadastro.CommandType = CommandType.StoredProcedure;
                cmdSelCadastro.CommandTimeout = 0;
                cmdSelCadastro.Parameters.Add("@flg_operacao", SqlDbType.Char).Value = "W";
                cmdSelCadastro.Parameters.Add("@NUM_DOCUMENTO", SqlDbType.VarChar).Value = o["type"].ToString() == "individual" ? o["taxpayer_id"].ToString() : o["ein"].ToString(); ;
                cmdSelCadastro.Parameters.Add("@FLG_TIPO_PESSOA", SqlDbType.Char).Value = o["type"].ToString() == "individual" ? "PF" : "PJ";
                cmdSelCadastro.Parameters.Add("@COD_ID_PESSOA_LICENCIADO", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["LICENCIADO"].ToString());


                SqlDataReader ReaderCadastro = cmdSelCadastro.ExecuteReader();
                while (ReaderCadastro.Read())
                {
                    // Atualizar dados TOKEN
                    SqlConnection connInsConsToken = new SqlConnection(Funcoes.conexao());
                    connInsConsToken.Open();
                    SqlCommand cmdInsConsToken = new SqlCommand("dbo.stp_pessoas_fj_ins", connInsConsToken);

                    cmdInsConsToken.CommandType = CommandType.StoredProcedure;
                    cmdInsConsToken.CommandTimeout = 0;
                    cmdInsConsToken.Parameters.Add("@flg_operacao", SqlDbType.Char).Value = "O";
                    cmdInsConsToken.Parameters.Add("@COD_ID", SqlDbType.Int).Value = Funcoes.strToInt(ReaderCadastro["COD_ID"].ToString());
                    cmdInsConsToken.Parameters.Add("@NUM_TOKEN", SqlDbType.VarChar).Value = o["id"].ToString();
                    cmdInsConsToken.Parameters.Add("@FLG_STATUS_ZOOP", SqlDbType.VarChar).Value = o["status"].ToString();
                    cmdInsConsToken.Parameters.Add("@COD_ID_ZOOP_HABILITADO", SqlDbType.Int).Value = 1;

                    cmdInsConsToken.ExecuteNonQuery();
                    connInsConsToken.Close();
                    connInsConsToken.Dispose();

                }
            }
            catch
            {

            }
        }
    }

    protected void btnImportarCappta_Click(object sender, EventArgs e)
    {

        foreach (ListViewItem itemP in lsvEstabelecimentosCappta.Items)
        {
            if (((CheckBox)itemP.FindControl("ckbImportar")).Checked == true)
            {
                GravarDadosEstabelecimentoCappta(((TextBox)itemP.FindControl("txtDocumento")).Text.ToString(), ddlMarketplaceCappta.SelectedValue.ToString());
                ((CheckBox)itemP.FindControl("ckbImportar")).Checked = false;
            }
        }

        ckbTodosCappta.Checked = false;
    }
    protected void btnCarregarCadastrosCappta_Click(object sender, EventArgs e)
    {
        if (ddlMarketplaceCappta.SelectedValue.ToString().Trim() != "")
        {
            dtEstabelecimentos.Rows.Clear();

            string jsonEstabelecimentos = "";
            jsonEstabelecimentos = hubcappta.ConsultarLojistas(ddlMarketplaceCappta.SelectedValue.ToString());
            txtJsonCappta.Text = jsonEstabelecimentos;

            JArray oEstabelecimentos = JArray.Parse(jsonEstabelecimentos);
            //JObject oRevendedores = JObject.Parse(jsonRevendedores);


            txtJsonCappta.Text = txtJsonCappta.Text + " - " + oEstabelecimentos.Count.ToString();

            if (oEstabelecimentos.Count > 0)
            {
                for (int i = 0; i < oEstabelecimentos.Count; i++)
                {
                    txtJsonCappta.Text = txtJsonCappta.Text + " - " + oEstabelecimentos[i]["merchant"]["document"].ToString();
                    dtEstabelecimentos.Rows.Add(oEstabelecimentos[i]["merchant"]["document"].ToString(), (oEstabelecimentos[i]["merchant"]["companyName"].ToString().Trim() != "") ? oEstabelecimentos[i]["merchant"]["companyName"].ToString().Trim() : oEstabelecimentos[i]["owner"]["name"].ToString().Trim(), oEstabelecimentos[i]["statusDescription"].ToString());
                }
            }

            this.lsvEstabelecimentosCappta.DataSource = dtEstabelecimentos;
            this.lsvEstabelecimentosCappta.DataBind();

            ClientScript.RegisterStartupScript(this.GetType(),
"SucessoCarregarDados", "alert('Dados dos estabelecimentos do marketplace " + ddlMarketplaceCappta.SelectedItem.Text.ToString() + " carregados com sucesso');", true);

        }
        else
        {
            ClientScript.RegisterStartupScript(this.GetType(),
    "ErroCarregarDados", "alert('É obrigatório selecionar um marketplace para carregar a lista de estabelecimentos! Verifique e tente novamente." + ddlRepresentante.SelectedValue.ToString() + "');", true);

        }
    }
    protected void btnImportarDocumentoCappta_Click(object sender, EventArgs e)
    {
        if (txtDocumentoCappta.Text.ToString().Trim() != "")
        {
            GravarDadosEstabelecimentoCappta(txtDocumentoCappta.Text.ToString(), ddlMarketplaceCappta.SelectedValue.ToString());
            ClientScript.RegisterStartupScript(this.GetType(),
                "Alerta", "alert('Dados Gravados com sucesso!');", true);
        }
        else
        {
            ClientScript.RegisterStartupScript(this.GetType(),
                "Alerta", "alert('Nenhum documento foi especificado! Verifique e reentre.');", true);

        }

    }
    protected void ckbTodosCappta_CheckedChanged(object sender, EventArgs e)
    {
        foreach (ListViewItem itemP in lsvEstabelecimentosCappta.Items)
        {
            ((CheckBox)itemP.FindControl("ckbImportar")).Checked = (((CheckBox)itemP.FindControl("ckbImportar")).Checked == false) ? true : false;
        }

    }
    protected void lsvEstabelecimentosCappta_ItemCommand(object sender, ListViewCommandEventArgs e)
    {

    }

    protected void lsvEstabelecimentosZoop_ItemCommand(object sender, ListViewCommandEventArgs e)
    {

    }
    private void GravarDadosEstabelecimentoCappta(string sLojista, string sDocumento)
    {
        string jsonRetornoCadastro = hubcappta.ConsultarLojista(sLojista.ToString(), sDocumento.ToString());
        txtJsonCappta.Visible = false;// true;
        txtJsonCappta.Text = jsonRetornoCadastro;

        JObject oEstabelecimento = JObject.Parse(jsonRetornoCadastro);

        try
        {
            txtJsonCappta.Text = txtJsonCappta.Text + oEstabelecimento["resellerDocument"].ToString();
            // ********************************************************************
            // Localizar cadastro do Marketplace
            // ********************************************************************
            string sIDMarketplace = "0";
            SqlConnection mySelCadastro = new SqlConnection(Funcoes.conexao());
            mySelCadastro.Open();
            SqlCommand cmdSelCadastro = new SqlCommand("dbo.stp_pessoas_fj_ins", mySelCadastro);
            cmdSelCadastro.CommandType = CommandType.StoredProcedure;
            cmdSelCadastro.Parameters.Add("@flg_operacao", SqlDbType.Char).Value = "H";
            cmdSelCadastro.Parameters.Add("@NOM_CNPJ", SqlDbType.VarChar).Value = ddlMarketplaceCappta.SelectedValue.ToString().Trim();
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
            cmdInsCons.Parameters.Add("@NOM_RAZAOSOCIAL", SqlDbType.VarChar).Value = (oEstabelecimento["merchant"]["document"].ToString().Trim().Length >= 14) ? oEstabelecimento["merchant"]["companyName"].ToString().Trim(): oEstabelecimento["owner"]["name"].ToString().Trim();
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
            cmdInsConsBancoCappta.Parameters.Add("@COD_ID_PESSOAS_FJ", SqlDbType.Int).Value = Funcoes.strToInt(sCodigo.ToString());

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


            ClientScript.RegisterStartupScript(this.GetType(),
                "MensagemSucesso", "alert('Documento(s) gravados com sucesso');", true);

            // Gravar os dados
        }
        catch
        {

            // Exibir mensagem de alerta
            //ClientScript.RegisterStartupScript(this.GetType(),
            //    "MensagemErro", "alert('Ocorreu um erro ao tentar importar o documento: " + sDocumento.ToString() + " - " + oEstabelecimento["errorMessage"].ToString() + "');", true);
        }


    }
    protected void ddlAdquirentes_SelectedIndexChanged(object sender, EventArgs e)
    {
        dvCapta.Visible = false;
        dvZoop.Visible = false;

        if (ddlAdquirentes.SelectedValue.ToString().Trim() == "C")
        {
            dvCapta.Visible = true;
        }

        if ((ddlAdquirentes.SelectedValue.ToString().Trim() == "Z") || (ddlAdquirentes.SelectedValue.ToString().Trim() == "W"))
        {
            dvZoop.Visible = true;
        }
    }
    protected void btnImportarDocumentoZoop_Click(object sender, EventArgs e)
    {
        if (ddlAdquirentes.SelectedValue.ToString().Trim() == "W")
        {
            //agilli.HttpResponseResult resultado = agilli.ConsultaResumidaEstabelecimento("?cpfCnpj=" + Funcoes.TIRAACENTOSDOCUMENTOS(txtDocumentoZoop.Text.ToString()));
            agilli.HttpResponseResult resultado = agilli.ConsultaResumidaEstabelecimento("?contrato=" + "029-249-13");

            if ((resultado.StatusCode == 200) || (resultado.StatusCode == 201))
            {
                string jsonResponse = resultado.Content;
                JArray oEstabelecimento = JArray.Parse(jsonResponse);
                txtJsonZoop.Text = jsonResponse;
            }
            else
            {
                // Falha - Exibir erro conforme necessário
                string mensagemErro = resultado.Content;
                txtJsonZoop.Text = mensagemErro;
                if (mensagemErro.ToString().Trim() != "")
                {
                    ClientScript.RegisterStartupScript(this.GetType(), "ErroImportar", "alert('Ocorreu um erro ao tentar importar os dados do estabelecimento especificado! Erro:" + mensagemErro + "');", true);
                }
            }
        }
    }
    protected void ckbTodosZoop_CheckedChanged(object sender, EventArgs e)
    {
        foreach (ListViewItem itemP in lsvEstabelecimentosZoop.Items)
        {
            ((CheckBox)itemP.FindControl("ckbImportar")).Checked = (((CheckBox)itemP.FindControl("ckbImportar")).Checked == false) ? true : false;
        }

    }
    protected void btnCarregarCadastrosZoopx_Click(object sender, EventArgs e)
    {

    }
    protected void btnImportarZoopx_Click(object sender, EventArgs e)
    {

    }
}