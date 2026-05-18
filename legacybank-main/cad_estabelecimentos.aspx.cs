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
using System.Data.SqlClient;


using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

using System.IO;
using System.Text;

using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;

using System.ComponentModel;

using System.Security.Cryptography;
using System.Net.Mail;
using System.IO.IsolatedStorage;



public partial class cad_estabelecimentos : System.Web.UI.Page
{
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
        if ((HttpContext.Current.Session["TIPO"].ToString().Trim() == "E"))
        {
            btnSalvar.Visible = false;
        }
        else
        {
            btnSalvar.Visible = true;
        }

        if (!IsPostBack)
        {
            Funcoes.GravaAuditoria(Funcoes.strToInt(sid_id), Request.ServerVariables["REMOTE_ADDR"].ToString(), "Cadastro - Estabelecimentos - Editar", "Acesso - ID: " + Funcoes.strToInt(sid_id).ToString());


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

            // Marketplaces

            SqlConnection myMarketplace = new SqlConnection(Funcoes.conexao());
            myMarketplace.Open();
            SqlCommand cmdMarketplace = new SqlCommand("dbo.stp_pessoas_fj_ins", myMarketplace);
            cmdMarketplace.CommandType = CommandType.StoredProcedure;
            cmdMarketplace.Parameters.Add("@flg_operacao", SqlDbType.VarChar).Value = "L";
            cmdMarketplace.Parameters.Add("@COD_ID_PESSOA_LICENCIADO", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["LICENCIADO"].ToString());
            cmdMarketplace.Parameters.Add("@FLG_ATIVO", SqlDbType.Char).Value = "S";
            cmdMarketplace.Parameters.Add("@FLG_TIPO", SqlDbType.Char).Value = "M";

            SqlDataAdapter drMarketplace = new SqlDataAdapter();
            drMarketplace.SelectCommand = cmdMarketplace;
            DataSet dsMarketplace = new DataSet();
            drMarketplace.Fill(dsMarketplace, "PESSOAS_FJ");
            ddlMarketplace.DataTextField = "NOM_RAZAOSOCIAL";
            ddlMarketplace.DataValueField = "COD_ID";
            ddlMarketplace.DataSource = dsMarketplace.Tables["PESSOAS_FJ"].DefaultView;
            ddlMarketplace.DataBind();
            ddlMarketplace.Items.Insert(0, new ListItem("", "0"));

            if (HttpContext.Current.Session["TIPO"].ToString().Trim() == "M")
            {
                ddlMarketplace.SelectedValue = HttpContext.Current.Session["PESSOA"].ToString();
                ddlMarketplace.Enabled = false;
            }


            // Representantes
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

            if (HttpContext.Current.Session["TIPO"].ToString().Trim() == "R")
            {
                ddlRepresentante.SelectedValue = HttpContext.Current.Session["PESSOA"].ToString();
                ddlRepresentante.Enabled = false;

                ddlMarketplace.SelectedValue = HttpContext.Current.Session["PESSOAMARKETPLACE"].ToString();
                ddlMarketplace.Enabled = false;
            }

            if (Funcoes.strToInt(sid_id) != 0)
            {
                ConsultaFicha();
                dvUsuario.Visible = false;
            }
            else
            {
                dvUsuario.Visible = true;
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

            ddlTipoFJ.SelectedValue = ReaderCadastro["FLG_TIPO_PESSOA"].ToString();
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


            ddlPresencial.SelectedValue = ReaderCadastro["FLG_PRESENCIAL"].ToString();

            ddlMarketplace.SelectedValue = ReaderCadastro["COD_ID_MARKETPLACE"].ToString();
            ddlRepresentante.SelectedValue = ReaderCadastro["COD_ID_REPRESENTANTE"].ToString();

            if ((HttpContext.Current.Session["TIPO"].ToString().Trim() == "R"))
            {
                ddlRepresentante.Enabled = false;
            }

            txtRazaoSocial.Text = ReaderCadastro["NOM_RAZAOSOCIAL"].ToString();
            txtFantasia.Text = ReaderCadastro["NOM_FANTASIA"].ToString();
            txtCNPJ.Text = ReaderCadastro["NOM_CNPJ"].ToString();
            txtTelefoneEmpresa.Text = ReaderCadastro["NUM_TELEFONE"].ToString();
            txtEmailEmpresa.Text = ReaderCadastro["NOM_EMAIL_EMPRESA"].ToString();
            txtDataAbertura.Text = ReaderCadastro["DTA_ABERTURA"].ToString();
            ddlTipoEmpresa.SelectedValue = ReaderCadastro["NOM_TIPO_EMPRESA"].ToString();
            ddlAtividadeEconomica.SelectedValue = ReaderCadastro["COD_ID_MCC"].ToString();
            txtFaturamento.Text = ReaderCadastro["NUM_FATURAMENTO"].ToString();
            txtPatrimonio.Text = ReaderCadastro["NUM_PATRIMONIO"].ToString();

            txtEndereco.Text = ReaderCadastro["NOM_ENDERECO"].ToString();
            txtNumero.Text = ReaderCadastro["NOM_NUMERO"].ToString();
            txtComplemento.Text = ReaderCadastro["NOM_COMPLEMENTO"].ToString();
            txtBairro.Text = ReaderCadastro["NOM_BAIRRO"].ToString();
            txtCidade.Text = ReaderCadastro["NOM_CIDADE"].ToString();
            ddlEstado.SelectedValue = ReaderCadastro["NOM_UF"].ToString();
            txtCEP.Text = ReaderCadastro["NOM_CEP"].ToString();

            txtNome.Text = ReaderCadastro["NOM_NOME"].ToString();
            txtSobrenome.Text = ReaderCadastro["NOM_SOBRENOME"].ToString();
            txtDocumento.Text = ReaderCadastro["NOM_CPF"].ToString();
            txtNascimento.Text = ReaderCadastro["DTA_ANIVERSARIO"].ToString();
            txtNomeMae.Text = ReaderCadastro["NOM_MAE"].ToString();
            txtRendaMensalPF.Text = ReaderCadastro["NUM_RENDA_MENSAL"].ToString();
            txtEmailResponsavel.Text = ReaderCadastro["NOM_EMAIL"].ToString();
            txtCelular.Text = ReaderCadastro["NOM_CELULAR"].ToString();
            ddlPoliticamenteExposta.SelectedValue = ReaderCadastro["FLG_POLITICAMENTE"].ToString();

            txtNomeUsuario.Text = ReaderCadastro["NOM_NOME_USUARIO"].ToString();
            txtEmail.Text = ReaderCadastro["NOM_LOGIN"].ToString();
            txtSenha.Text = ReaderCadastro["NOM_SENHA"].ToString();
        }

    }

    protected void btnSalvar_Click(object sender, EventArgs e)
    {
        if (((txtEmail.Text.ToString().Trim() == txtEmail.Text.ToString().Trim()) && (txtSenha.Text.ToString().Trim() == txtSenhaC.Text.ToString().Trim())) || (Funcoes.strToInt(sid_id) != 0))
        {

            // validar nome usuario
            if (txtNomeUsuario.Text.ToString().Trim() == "")
            {
                ClientScript.RegisterStartupScript(this.GetType(),
                    "AlertaUsuarioInvalido", "alert('O nome do usuário não pode ser deixado em branco! Verifique e tente novamente.'); ", true);
                return;
            }

            //Validar email e senha
            if (Funcoes.IsEmail(txtEmail.Text.ToString()) == false)
            {
                ClientScript.RegisterStartupScript(this.GetType(),
                    "AlertaEmailInvalido", "alert('O e-mail digitado não é válido! Verifique e tente novamente.'); ", true);
                return;
            }

            // Verificar a existência pelo CNPJ ou CPF

            if (Funcoes.strToInt(sid_id) == 0)
            {
                SqlConnection mySelCadastro = new SqlConnection(Funcoes.conexao());
                mySelCadastro.Open();
                SqlCommand cmdSelCadastro = new SqlCommand("dbo.stp_pessoas_fj_ins", mySelCadastro);
                cmdSelCadastro.CommandType = CommandType.StoredProcedure;
                cmdSelCadastro.Parameters.Add("@flg_operacao", SqlDbType.Char).Value = "H";
                cmdSelCadastro.Parameters.Add("@NOM_CNPJ", SqlDbType.VarChar).Value = txtCNPJ.Text.ToString();
                cmdSelCadastro.Parameters.Add("@COD_ID_PESSOA_LICENCIADO", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["LICENCIADO"].ToString());
                cmdSelCadastro.Parameters.Add("@FLG_TIPO", SqlDbType.Char).Value = "E";

                SqlDataReader ReaderCadastro = cmdSelCadastro.ExecuteReader();
                while (ReaderCadastro.Read())
                {
                    ClientScript.RegisterStartupScript(this.GetType(),
                        "Duplicidade", "alert('Já existe um estabelecimento cadastrado com este documento (" + ReaderCadastro["COD_ID"].ToString() + " - " + ReaderCadastro["NOM_RAZAOSOCIAL"].ToString() + ")! Verifique e reentre.');", true);
                    return;
                }
            }

            SqlConnection connInsCons = new SqlConnection(Funcoes.conexao());
            connInsCons.Open();
            SqlCommand cmdInsCons = new SqlCommand("dbo.stp_pessoas_fj_ins", connInsCons);
            cmdInsCons.CommandType = CommandType.StoredProcedure;
            if (Funcoes.strToInt(sid_id) != 0)
            {
                cmdInsCons.Parameters.Add("@flg_operacao", SqlDbType.Char).Value = 'A';
                cmdInsCons.Parameters.Add("@COD_ID", SqlDbType.Int).Value = Funcoes.strToInt(sid_id.ToString());
            }
            else
            {
                cmdInsCons.Parameters.Add("@flg_operacao", SqlDbType.Char).Value = 'I';
                cmdInsCons.Parameters.Add("@FLG_ATIVO", SqlDbType.Char).Value = "V";
                cmdInsCons.Parameters.Add("@FLG_INSERT_USUARIO", SqlDbType.Char).Value = "S";

                cmdInsCons.Parameters.Add("@COD_ID_SIS_USUARIO_INCLUSAO", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["PESSOA"].ToString());
                cmdInsCons.Parameters.Add("@NUM_IP_INCLUSAO", SqlDbType.VarChar).Value = HttpContext.Current.Session["REMOTEADDR"].ToString();


            }
            cmdInsCons.Parameters.Add("@COD_ID_PESSOA_LICENCIADO", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["LICENCIADO"].ToString());
            cmdInsCons.Parameters.Add("@DTA_DATA", SqlDbType.DateTime).Value = DateTime.Now;
            cmdInsCons.Parameters.Add("@FLG_TIPO", SqlDbType.Char).Value = "E";


            cmdInsCons.Parameters.Add("@FLG_TIPO_PESSOA", SqlDbType.VarChar).Value = ddlTipoFJ.SelectedValue.ToString();
            cmdInsCons.Parameters.Add("@FLG_PRESENCIAL", SqlDbType.Char).Value = ddlPresencial.SelectedValue.ToString();

            //if ((HttpContext.Current.Session["TIPO"].ToString().Trim() == "M"))
            //{
            //    cmdInsCons.Parameters.Add("@COD_ID_MARKETPLACE", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["PESSOA"].ToString());
            //}

            cmdInsCons.Parameters.Add("@COD_ID_MARKETPLACE", SqlDbType.Int).Value = Funcoes.strToInt(ddlMarketplace.SelectedValue.ToString());
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
                "Alerta", "alert('Dados gravados com sucesso'); opener.PostBackOnMainPage(); window.close(); ", true);
        }
        else
        {
            ClientScript.RegisterStartupScript(this.GetType(),
                "Usuario", "alert('E-mail ou Senha do Usuário são diferentes dos dados de confirmação! Verifique e reentre.');", true);
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
}