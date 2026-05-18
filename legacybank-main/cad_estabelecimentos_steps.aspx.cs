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

using System.Net.Mime;
using System.Net.Configuration;



public partial class cad_estabelecimentos_steps : System.Web.UI.Page
{
    public static DataTable dtPessoas;
    public static DataTable dtEmpresas;


    private int Licenciado { get; set; }
    private string Tipo { get; set; }
    private int Pessoa { get; set; }
    private int Usuario { get; set; }

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
    // === CARGA SEGURA DE SESSION ===
    Licenciado = Session["LICENCIADO"] != null ? Funcoes.strToInt(Session["LICENCIADO"].ToString()) : 0;
    Tipo = Session["TIPO"] != null ? Session["TIPO"].ToString().Trim() : "";
    Pessoa = Session["PESSOA"] != null ? Funcoes.strToInt(Session["PESSOA"].ToString()) : 0;
    Usuario = Session["CODIGO"] != null ? Funcoes.strToInt(Session["CODIGO"].ToString()) : 0;

    // Verifica se o usuário esta autenticado
    if (Session.Count <= 0)
    {
        FormsAuthentication.SignOut();
        Response.Redirect("login.aspx", true);
        return;
    }

    // Regras de botão
    btnSalvar.Visible = false;

    if (!IsPostBack)
    {
        Funcoes.GravaAuditoria(
            Funcoes.strToInt(sid_id),
            Request.ServerVariables["REMOTE_ADDR"].ToString(),
            "Cadastro - Estabelecimentos - Steps - Editar",
            "Acesso - ID: " + Funcoes.strToInt(sid_id).ToString()
        );

        // ============================
        // TABELAS EM MEMÓRIA
        // ============================
        dtPessoas = new DataTable();
        dtPessoas.Columns.Add("nome", typeof(string));
        dtPessoas.Columns.Add("email", typeof(string));
        dtPessoas.Columns.Add("cpf", typeof(string));

        dtEmpresas = new DataTable();
        dtEmpresas.Columns.Add("razaosocial", typeof(string));
        dtEmpresas.Columns.Add("nomefantasia", typeof(string));
        dtEmpresas.Columns.Add("cnpj", typeof(string));

        // ============================
        // MCC
        // ============================
        using (SqlConnection conn = new SqlConnection(Funcoes.conexao()))
        using (SqlCommand cmd = new SqlCommand("dbo.stp_mcc_ins", conn))
        {
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandTimeout = 120;
            cmd.Parameters.Add("@flg_operacao", SqlDbType.VarChar).Value = "L";

            using (SqlDataAdapter da = new SqlDataAdapter(cmd))
            {
                DataSet ds = new DataSet();
                da.Fill(ds, "MCC");

                ddlAtividadeEconomica.DataTextField = "NOM_MCC";
                ddlAtividadeEconomica.DataValueField = "COD_ID";
                ddlAtividadeEconomica.DataSource = ds.Tables["MCC"].DefaultView;
                ddlAtividadeEconomica.DataBind();
                ddlAtividadeEconomica.Items.Insert(0, new ListItem("", "0"));
            }
        }

        // ============================
        // MARKETPLACE
        // ============================
        using (SqlConnection conn = new SqlConnection(Funcoes.conexao()))
        using (SqlCommand cmd = new SqlCommand("dbo.stp_pessoas_fj_ins", conn))
        {
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandTimeout = 120;
            cmd.Parameters.Add("@flg_operacao", SqlDbType.VarChar).Value = "L";
            cmd.Parameters.Add("@COD_ID_PESSOA_LICENCIADO", SqlDbType.Int).Value = Licenciado;
            cmd.Parameters.Add("@FLG_ATIVO", SqlDbType.Char).Value = "S";
            cmd.Parameters.Add("@FLG_TIPO", SqlDbType.Char).Value = "M";

            using (SqlDataAdapter da = new SqlDataAdapter(cmd))
            {
                DataSet ds = new DataSet();
                da.Fill(ds, "PESSOAS_FJ");

                ddlMarketplace.DataTextField = "NOM_RAZAOSOCIAL";
                ddlMarketplace.DataValueField = "COD_ID";
                ddlMarketplace.DataSource = ds.Tables["PESSOAS_FJ"].DefaultView;
                ddlMarketplace.DataBind();
                ddlMarketplace.Items.Insert(0, new ListItem("", "0"));
            }
        }

        if (Tipo == "M")
        {
            ddlMarketplace.SelectedValue = Pessoa.ToString();
            ddlMarketplace.Enabled = false;
        }

        // ============================
        // REPRESENTANTES
        // ============================
        using (SqlConnection conn = new SqlConnection(Funcoes.conexao()))
        using (SqlCommand cmd = new SqlCommand("dbo.stp_pessoas_fj_ins", conn))
        {
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandTimeout = 120;
            cmd.Parameters.Add("@flg_operacao", SqlDbType.VarChar).Value = "L";
            cmd.Parameters.Add("@COD_ID_PESSOA_LICENCIADO", SqlDbType.Int).Value = Licenciado;
            cmd.Parameters.Add("@FLG_ATIVO", SqlDbType.Char).Value = "S";
            cmd.Parameters.Add("@FLG_TIPO", SqlDbType.Char).Value = "R";

            if (Tipo == "M")
                cmd.Parameters.Add("@COD_ID_MARKETPLACE", SqlDbType.Int).Value = Pessoa;

            using (SqlDataAdapter da = new SqlDataAdapter(cmd))
            {
                DataSet ds = new DataSet();
                da.Fill(ds, "PESSOAS_FJ");

                ddlRepresentante.DataTextField = "NOM_RAZAOSOCIAL";
                ddlRepresentante.DataValueField = "COD_ID";
                ddlRepresentante.DataSource = ds.Tables["PESSOAS_FJ"].DefaultView;
                ddlRepresentante.DataBind();
                ddlRepresentante.Items.Insert(0, new ListItem("", "0"));
            }
        }

        if (Tipo == "R")
        {
            ddlRepresentante.SelectedValue = Pessoa.ToString();
            ddlRepresentante.Enabled = false;

            if (Session["PESSOAMARKETPLACE"] != null)
            {
                string valorMarketplace = Session["PESSOAMARKETPLACE"].ToString();

                if (ddlMarketplace.Items.FindByValue(valorMarketplace) != null)
                {
                    ddlMarketplace.SelectedValue = valorMarketplace;
                }
            }            
            ddlMarketplace.Enabled = false;
        }

        // ============================
        // OUTRAS ROTINAS
        // ============================
        ConsultaIntegracao(3);

        if (Funcoes.strToInt(sid_id) != 0)
        {
            ConsultaFicha();
            dvUsuario.Visible = false;
        }
        else
        {
            dvUsuario.Visible = false;
        }
    }
}


private void ConsultaIntegracao(int iIntegracao)
{
    btnPesquisarPF.Visible = false;
    btnPesquisarPJ.Visible = false;

    // === proteção: se não há licenciado, não consulta ===
    if (Licenciado <= 0)
        return;

    using (SqlConnection mySelCadastro = new SqlConnection(Funcoes.conexao()))
    {
        using (SqlCommand cmdSelCadastro = new SqlCommand("dbo.stp_pessoas_fj_integracoes_chaves_ins", mySelCadastro))
        {
            cmdSelCadastro.CommandType = CommandType.StoredProcedure;
            cmdSelCadastro.CommandTimeout = 30; // evita request preso indefinidamente

            cmdSelCadastro.Parameters.Add("@flg_operacao", SqlDbType.Char).Value = "L";
            cmdSelCadastro.Parameters.Add("@COD_ID_INTEGRACOES", SqlDbType.Int).Value = iIntegracao;
            cmdSelCadastro.Parameters.Add("@COD_ID_PESSOA_LICENCIADO", SqlDbType.Int).Value = Licenciado;

            mySelCadastro.Open();

            using (SqlDataReader reader = cmdSelCadastro.ExecuteReader())
            {
                while (reader.Read())
                {
                    string flgAtivo = reader["FLG_ATIVO"] != DBNull.Value
                        ? reader["FLG_ATIVO"].ToString().Trim()
                        : "";

                    if (flgAtivo == "S")
                    {
                        btnPesquisarPF.Visible = true;
                        btnPesquisarPJ.Visible = true;
                        break; // já achou ativo → não precisa continuar lendo
                    }
                }
            }
        }
    }
}


private void ConsultaFicha()
{
    using (SqlConnection mySelCadastro = new SqlConnection(Funcoes.conexao()))
    {
        mySelCadastro.Open();

        using (SqlCommand cmdSelCadastro = new SqlCommand("dbo.stp_pessoas_fj_ins", mySelCadastro))
        {
            cmdSelCadastro.CommandType = CommandType.StoredProcedure;
            cmdSelCadastro.Parameters.Add("@flg_operacao", SqlDbType.Char).Value = "S";
            cmdSelCadastro.Parameters.Add("@COD_ID", SqlDbType.Int).Value = Funcoes.strToInt(sid_id.ToString());

            // 🔐 usa a propriedade já carregada da Session
            cmdSelCadastro.Parameters.Add("@COD_ID_PESSOA_LICENCIADO", SqlDbType.Int).Value = Licenciado;

            using (SqlDataReader ReaderCadastro = cmdSelCadastro.ExecuteReader())
            {
                while (ReaderCadastro.Read())
                {
                    dvGeral.Visible = false;
                    dvFisica.Visible = false;
                    dvJuridica.Visible = false;
                    dvEndereco.Visible = false;
                    dvResponsavel.Visible = false;
                    dvUsuario.Visible = false;

                    ddlTipoFJ.SelectedValue = ReaderCadastro["FLG_TIPO_PESSOA"].ToString();

                    if (ddlTipoFJ.SelectedValue == "PF")
                    {
                        ClientScript.RegisterStartupScript(this.GetType(),
                            "CNPJCPF", "$('#txtCNPJ').prop('placeholder', '000.000.000-00');", true);

                        lblRazaoSocial.Text = "Nome Completo";
                        lblFantasia.Text = "Apelido";
                        lblCNPJ.Text = "CPF";
                        lblDataAbertura.Text = "Data Início Atividade";

                        dvFisica.Visible = true;
                        dvEndereco.Visible = true;
                        dvGeral.Visible = true;
                        dvUsuario.Visible = true;
                    }
                    else
                    {
                        ClientScript.RegisterStartupScript(this.GetType(),
                            "CNPJCPF", "$('#txtCNPJ').prop('placeholder', '00.000.000/0000-00');", true);

                        lblRazaoSocial.Text = "Razão Social";
                        lblFantasia.Text = "Nome Fantasia";
                        lblCNPJ.Text = "CNPJ";
                        lblDataAbertura.Text = "Data Abertura Empresa";

                        dvJuridica.Visible = true;
                        dvEndereco.Visible = true;
                        dvResponsavel.Visible = true;
                        dvGeral.Visible = true;
                        dvUsuario.Visible = true;
                    }

                    ddlPresencial.SelectedValue = ReaderCadastro["FLG_PRESENCIAL"].ToString();
                    ddlMarketplace.SelectedValue = ReaderCadastro["COD_ID_MARKETPLACE"].ToString();
                    ddlRepresentante.SelectedValue = ReaderCadastro["COD_ID_REPRESENTANTE"].ToString();

                    // 🔐 agora usa a propriedade Tipo (já vinda da Session)
                    if (Tipo == "R")
                        ddlRepresentante.Enabled = false;

                    if (ddlTipoFJ.SelectedValue == "PJ")
                    {
                        txtRazaoSocial.Text = ReaderCadastro["NOM_RAZAOSOCIAL"].ToString();
                        txtFantasia.Text = ReaderCadastro["NOM_FANTASIA"].ToString();
                        txtCNPJ.Text = ReaderCadastro["NOM_CNPJ"].ToString();
                        txtTelefoneEmpresa.Text = ReaderCadastro["NUM_TELEFONE"].ToString();
                        txtEmailEmpresa.Text = ReaderCadastro["NOM_EMAIL_EMPRESA"].ToString();
                        txtDataAbertura.Text = ReaderCadastro["DTA_ABERTURA"].ToString();
                        txtFaturamento.Text = ReaderCadastro["NUM_FATURAMENTO"].ToString();
                        txtPatrimonio.Text = ReaderCadastro["NUM_PATRIMONIO"].ToString();
                    }

                    ddlTipoEmpresa.SelectedValue = ReaderCadastro["NOM_TIPO_EMPRESA"].ToString();
                    ddlAtividadeEconomica.SelectedValue = ReaderCadastro["COD_ID_MCC"].ToString();

                    txtEndereco.Text = ReaderCadastro["NOM_ENDERECO"].ToString();
                    txtNumero.Text = ReaderCadastro["NOM_NUMERO"].ToString();
                    txtComplemento.Text = ReaderCadastro["NOM_COMPLEMENTO"].ToString();
                    txtBairro.Text = ReaderCadastro["NOM_BAIRRO"].ToString();
                    txtCidade.Text = ReaderCadastro["NOM_CIDADE"].ToString();
                    ddlEstado.SelectedValue = ReaderCadastro["NOM_UF"].ToString();
                    txtCEP.Text = ReaderCadastro["NOM_CEP"].ToString();

                    if (ddlTipoFJ.SelectedValue == "PJ")
                    {
                        // Responsável
                        txtNome.Text = ReaderCadastro["NOM_NOME"].ToString();
                        txtSobrenome.Text = ReaderCadastro["NOM_SOBRENOME"].ToString();
                        txtDocumento.Text = ReaderCadastro["NOM_CPF"].ToString();
                        txtNascimento.Text = ReaderCadastro["DTA_ANIVERSARIO"].ToString();
                        txtNomeMae.Text = ReaderCadastro["NOM_MAE"].ToString();
                        txtRendaMensal.Text = ReaderCadastro["NUM_RENDA_MENSAL"].ToString();
                        txtEmailResponsavel.Text = ReaderCadastro["NOM_EMAIL"].ToString();
                        txtCelular.Text = ReaderCadastro["NOM_CELULAR"].ToString();
                        ddlPoliticamenteExposta.SelectedValue = ReaderCadastro["FLG_POLITICAMENTE"].ToString();
                    }
                    else
                    {
                        // Pessoa Física
                        txtNomePF.Text = ReaderCadastro["NOM_NOME"].ToString();
                        txtSobrenomePF.Text = ReaderCadastro["NOM_SOBRENOME"].ToString();
                        txtDocumentoPF.Text = ReaderCadastro["NOM_CPF"].ToString();
                        txtNascimentoPF.Text = ReaderCadastro["DTA_ANIVERSARIO"].ToString();
                        txtNomeMaePF.Text = ReaderCadastro["NOM_MAE"].ToString();
                        txtRendaMensalPF.Text = ReaderCadastro["NUM_RENDA_MENSAL"].ToString();
                        txtEmailPF.Text = ReaderCadastro["NOM_EMAIL"].ToString();
                        txtCelularPF.Text = ReaderCadastro["NOM_CELULAR"].ToString();
                        ddlPoliticamenteExpostaPF.SelectedValue = ReaderCadastro["FLG_POLITICAMENTE"].ToString();
                    }

                    txtNomeUsuario.Text = ReaderCadastro["NOM_NOME_USUARIO"].ToString();
                    txtEmail.Text = ReaderCadastro["NOM_LOGIN"].ToString();
                    txtSenha.Text = ReaderCadastro["NOM_SENHA"].ToString();
                }
            }
        }
    }
}



protected void btnSalvar_Click(object sender, EventArgs e)
{
    if (ddlTipoFJ.SelectedValue.Trim() == "")
    {
        ClientScript.RegisterStartupScript(this.GetType(),
            "TipoRegistro", "alert('Selecione primeiro o tipo de cadastro (PF) Pessoa Física ou (PJ) Pessoa Jurídica');", true);
        return;
    }

    // valida email e senha
    if (!(((txtEmail.Text.Trim() == txtEmail.Text.Trim()) &&
           (txtSenha.Text.Trim() == txtSenhaC.Text.Trim())) ||
          (Funcoes.strToInt(sid_id) != 0)))
    {
        ClientScript.RegisterStartupScript(this.GetType(),
            "Usuario", "alert('E-mail ou Senha do Usuário são diferentes dos dados de confirmação! Verifique e reentre.');", true);
        return;
    }

    // validar nome usuario
    if (txtNomeUsuario.Text.Trim() == "")
    {
        ClientScript.RegisterStartupScript(this.GetType(),
            "AlertaUsuarioInvalido", "alert('O nome do usuário não pode ser deixado em branco! Verifique e tente novamente.');", true);
        return;
    }

    // validar email
    if (!Funcoes.IsEmail(txtEmail.Text))
    {
        ClientScript.RegisterStartupScript(this.GetType(),
            "AlertaEmailInvalido", "alert('O e-mail digitado não é válido! Verifique e tente novamente.');", true);
        return;
    }

    // ==========================================================
    // 🔎 VERIFICA DUPLICIDADE (somente inclusão)
    // ==========================================================
    if (Funcoes.strToInt(sid_id) == 0)
    {
        using (SqlConnection mySelCadastro = new SqlConnection(Funcoes.conexao()))
        using (SqlCommand cmdSelCadastro = new SqlCommand("dbo.stp_pessoas_fj_ins", mySelCadastro))
        {
            mySelCadastro.Open();
            cmdSelCadastro.CommandType = CommandType.StoredProcedure;
            cmdSelCadastro.Parameters.Add("@flg_operacao", SqlDbType.Char).Value = "H";

            if (ddlTipoFJ.SelectedValue == "PJ")
                cmdSelCadastro.Parameters.Add("@NOM_CNPJ", SqlDbType.VarChar).Value = txtCNPJ.Text;
            else
                cmdSelCadastro.Parameters.Add("@NOM_CNPJ", SqlDbType.VarChar).Value = txtDocumentoPF.Text;

            // 🔐 usa a propriedade já carregada
            cmdSelCadastro.Parameters.Add("@COD_ID_PESSOA_LICENCIADO", SqlDbType.Int).Value = Licenciado;
            cmdSelCadastro.Parameters.Add("@FLG_TIPO", SqlDbType.Char).Value = "E";

            using (SqlDataReader ReaderCadastro = cmdSelCadastro.ExecuteReader())
            {
                if (ReaderCadastro.Read())
                {
                    ClientScript.RegisterStartupScript(this.GetType(),
                        "Duplicidade",
                        "alert('Já existe um estabelecimento cadastrado com este documento (" +
                        ReaderCadastro["COD_ID"] + " - " +
                        ReaderCadastro["NOM_RAZAOSOCIAL"] + ")! Verifique e reentre.');", true);
                    return;
                }
            }
        }
    }

    // ==========================================================
    // 💾 INSERT / UPDATE
    // ==========================================================
    using (SqlConnection connInsCons = new SqlConnection(Funcoes.conexao()))
    using (SqlCommand cmdInsCons = new SqlCommand("dbo.stp_pessoas_fj_ins", connInsCons))
    {
        connInsCons.Open();
        cmdInsCons.CommandType = CommandType.StoredProcedure;

        if (Funcoes.strToInt(sid_id) != 0)
        {
            cmdInsCons.Parameters.Add("@flg_operacao", SqlDbType.Char).Value = 'A';
            cmdInsCons.Parameters.Add("@COD_ID", SqlDbType.Int).Value = Funcoes.strToInt(sid_id);
        }
        else
        {
            cmdInsCons.Parameters.Add("@flg_operacao", SqlDbType.Char).Value = 'I';
            cmdInsCons.Parameters.Add("@FLG_ATIVO", SqlDbType.Char).Value = "V";
            cmdInsCons.Parameters.Add("@FLG_INSERT_USUARIO", SqlDbType.Char).Value = "S";

            // 🔐 propriedades em vez de Session
            cmdInsCons.Parameters.Add("@COD_ID_SIS_USUARIO_INCLUSAO", SqlDbType.Int).Value = Pessoa;
            cmdInsCons.Parameters.Add("@NUM_IP_INCLUSAO", SqlDbType.VarChar).Value =
                Request.ServerVariables["REMOTE_ADDR"];
        }

        cmdInsCons.Parameters.Add("@COD_ID_PESSOA_LICENCIADO", SqlDbType.Int).Value = Licenciado;
        cmdInsCons.Parameters.Add("@DTA_DATA", SqlDbType.DateTime).Value = DateTime.Now;
        cmdInsCons.Parameters.Add("@FLG_TIPO", SqlDbType.Char).Value = "E";

        cmdInsCons.Parameters.Add("@FLG_TIPO_PESSOA", SqlDbType.VarChar).Value = ddlTipoFJ.SelectedValue;
        cmdInsCons.Parameters.Add("@FLG_PRESENCIAL", SqlDbType.Char).Value = ddlPresencial.SelectedValue;

        cmdInsCons.Parameters.Add("@COD_ID_MARKETPLACE", SqlDbType.Int)
            .Value = Funcoes.strToInt(ddlMarketplace.SelectedValue);
        cmdInsCons.Parameters.Add("@COD_ID_REPRESENTANTE", SqlDbType.Int)
            .Value = Funcoes.strToInt(ddlRepresentante.SelectedValue);

        // ==========================================================
        // 🧩 DADOS PJ / PF
        // ==========================================================
        if (ddlTipoFJ.SelectedValue == "PJ")
        {
            txtSobrenome.Text = Funcoes.NomeSobrenome(txtNome.Text, "U").Trim();
            txtNome.Text = Funcoes.NomeSobrenome(txtNome.Text, "P").Trim();

            cmdInsCons.Parameters.Add("@NOM_RAZAOSOCIAL", SqlDbType.VarChar).Value = txtRazaoSocial.Text;
            cmdInsCons.Parameters.Add("@NOM_FANTASIA", SqlDbType.VarChar).Value = txtFantasia.Text;
            cmdInsCons.Parameters.Add("@NOM_CNPJ", SqlDbType.VarChar).Value = txtCNPJ.Text;
            cmdInsCons.Parameters.Add("@NUM_TELEFONE", SqlDbType.VarChar).Value = txtTelefoneEmpresa.Text;
            cmdInsCons.Parameters.Add("@NOM_EMAIL_EMPRESA", SqlDbType.VarChar).Value = txtEmailEmpresa.Text;
            cmdInsCons.Parameters.Add("@NUM_FATURAMENTO", SqlDbType.Float).Value = Funcoes.strToDouble(txtFaturamento.Text);
            cmdInsCons.Parameters.Add("@NUM_PATRIMONIO", SqlDbType.Float).Value = Funcoes.strToDouble(txtPatrimonio.Text);

            if (txtDataAbertura.Text.Trim() != "")
                cmdInsCons.Parameters.Add("@DTA_ABERTURA", SqlDbType.DateTime)
                    .Value = Convert.ToDateTime(txtDataAbertura.Text);

            cmdInsCons.Parameters.Add("@NOM_NOME", SqlDbType.VarChar).Value = txtNome.Text;
            cmdInsCons.Parameters.Add("@NOM_SOBRENOME", SqlDbType.VarChar).Value = txtSobrenome.Text;
            cmdInsCons.Parameters.Add("@NOM_CPF", SqlDbType.VarChar).Value = txtDocumento.Text;

            if (txtNascimento.Text.Trim() != "")
                cmdInsCons.Parameters.Add("@DTA_ANIVERSARIO", SqlDbType.DateTime)
                    .Value = Convert.ToDateTime(txtNascimento.Text);

            cmdInsCons.Parameters.Add("@NOM_MAE", SqlDbType.VarChar).Value = txtNomeMae.Text;
            cmdInsCons.Parameters.Add("@NUM_RENDA_MENSAL", SqlDbType.Float).Value = Funcoes.strToDouble(txtRendaMensal.Text);
            cmdInsCons.Parameters.Add("@NOM_EMAIL", SqlDbType.VarChar).Value = txtEmailResponsavel.Text;
            cmdInsCons.Parameters.Add("@NOM_CELULAR", SqlDbType.VarChar).Value = txtCelular.Text;
            cmdInsCons.Parameters.Add("@FLG_POLITICAMENTE", SqlDbType.Char).Value = ddlPoliticamenteExposta.SelectedValue;
        }
        else
        {
            txtSobrenomePF.Text = Funcoes.NomeSobrenome(txtNomePF.Text, "U").Trim();
            txtNomePF.Text = Funcoes.NomeSobrenome(txtNomePF.Text, "P").Trim();

            string nomeCompleto = txtNomePF.Text + " " + txtSobrenomePF.Text;

            cmdInsCons.Parameters.Add("@NOM_RAZAOSOCIAL", SqlDbType.VarChar).Value = nomeCompleto;
            cmdInsCons.Parameters.Add("@NOM_FANTASIA", SqlDbType.VarChar).Value = nomeCompleto;
            cmdInsCons.Parameters.Add("@NOM_CNPJ", SqlDbType.VarChar).Value = txtDocumentoPF.Text;
            cmdInsCons.Parameters.Add("@NUM_TELEFONE", SqlDbType.VarChar).Value = txtCelularPF.Text;
            cmdInsCons.Parameters.Add("@NOM_EMAIL_EMPRESA", SqlDbType.VarChar).Value = txtEmailPF.Text;
            cmdInsCons.Parameters.Add("@NUM_FATURAMENTO", SqlDbType.Float).Value = Funcoes.strToDouble(txtRendaMensalPF.Text);
            cmdInsCons.Parameters.Add("@NUM_PATRIMONIO", SqlDbType.Float).Value = 0;

            if (txtNascimentoPF.Text.Trim() != "")
                cmdInsCons.Parameters.Add("@DTA_ABERTURA", SqlDbType.DateTime)
                    .Value = Convert.ToDateTime(txtNascimentoPF.Text);

            cmdInsCons.Parameters.Add("@NOM_NOME", SqlDbType.VarChar).Value = txtNomePF.Text;
            cmdInsCons.Parameters.Add("@NOM_SOBRENOME", SqlDbType.VarChar).Value = txtSobrenomePF.Text;
            cmdInsCons.Parameters.Add("@NOM_CPF", SqlDbType.VarChar).Value = txtDocumentoPF.Text;

            if (txtNascimentoPF.Text.Trim() != "")
                cmdInsCons.Parameters.Add("@DTA_ANIVERSARIO", SqlDbType.DateTime)
                    .Value = Convert.ToDateTime(txtNascimentoPF.Text);

            cmdInsCons.Parameters.Add("@NOM_MAE", SqlDbType.VarChar).Value = txtNomeMaePF.Text;
            cmdInsCons.Parameters.Add("@NUM_RENDA_MENSAL", SqlDbType.Float).Value = Funcoes.strToDouble(txtRendaMensalPF.Text);
            cmdInsCons.Parameters.Add("@NOM_EMAIL", SqlDbType.VarChar).Value = txtEmailPF.Text;
            cmdInsCons.Parameters.Add("@NOM_CELULAR", SqlDbType.VarChar).Value = txtCelularPF.Text;
            cmdInsCons.Parameters.Add("@FLG_POLITICAMENTE", SqlDbType.Char).Value = ddlPoliticamenteExpostaPF.SelectedValue;
        }

        // ==========================================================
        // 📍 DADOS GERAIS / ENDEREÇO
        // ==========================================================
        cmdInsCons.Parameters.Add("@COD_ID_MCC", SqlDbType.Int)
            .Value = Funcoes.strToInt(ddlAtividadeEconomica.SelectedValue);
        cmdInsCons.Parameters.Add("@NOM_TIPO_EMPRESA", SqlDbType.VarChar).Value = ddlTipoEmpresa.SelectedValue;

        cmdInsCons.Parameters.Add("@NOM_ENDERECO", SqlDbType.VarChar).Value = txtEndereco.Text;
        cmdInsCons.Parameters.Add("@NOM_NUMERO", SqlDbType.VarChar).Value = txtNumero.Text;
        cmdInsCons.Parameters.Add("@NOM_COMPLEMENTO", SqlDbType.VarChar).Value = txtComplemento.Text;
        cmdInsCons.Parameters.Add("@NOM_BAIRRO", SqlDbType.VarChar).Value = txtBairro.Text;
        cmdInsCons.Parameters.Add("@NOM_CIDADE", SqlDbType.VarChar).Value = txtCidade.Text;
        cmdInsCons.Parameters.Add("@NOM_UF", SqlDbType.VarChar).Value = ddlEstado.SelectedValue;
        cmdInsCons.Parameters.Add("@NOM_CEP", SqlDbType.VarChar).Value = txtCEP.Text;
        cmdInsCons.Parameters.Add("@NOM_PAIS", SqlDbType.VarChar).Value = "BR";

        // ==========================================================
        // 👤 USUÁRIO (somente inclusão)
        // ==========================================================
        if (Funcoes.strToInt(sid_id) == 0)
        {
            cmdInsCons.Parameters.Add("@NOM_NOME_USUARIO", SqlDbType.VarChar).Value = txtNomeUsuario.Text;
            cmdInsCons.Parameters.Add("@NOM_LOGIN", SqlDbType.VarChar).Value = txtEmail.Text;

            string caracteres = "abcdefghijkmnopqrstuvwxyzABCDEFGHJKLMNOPQRSTUVWXYZ0123456789!@$?_-";
            char[] chars = new char[8];
            Random rd = new Random();
            for (int i = 0; i < 8; i++)
                chars[i] = caracteres[rd.Next(0, caracteres.Length)];

            string sNovaSenha = new string(chars);
            cmdInsCons.Parameters.Add("@NOM_SENHA", SqlDbType.VarChar).Value = sNovaSenha;
            txtSenha.Text = sNovaSenha;
        }

        cmdInsCons.ExecuteNonQuery();
    }

    // ==========================================================
    // ✉️ ENVIO DE E-MAIL (mantém Session pois são configs globais)
    // ==========================================================
    if (ckbEnviarEmail.Checked && Funcoes.strToInt(sid_id) == 0)
    {
        try
        {
            System.Net.Mail.SmtpClient client = new System.Net.Mail.SmtpClient
            {
                Host = Session["EMAILHOST"].ToString(),
                EnableSsl = Session["EMAILSSL"].ToString() == "S",
                Port = Funcoes.strToInt(Session["EMAILPORTA"].ToString()),
                Credentials = new System.Net.NetworkCredential(
                    Session["EMAIL"].ToString(),
                    Session["EMAILSENHA"].ToString())
            };

            MailMessage mail = new MailMessage
            {
                From = new MailAddress("envio@" + Request.ServerVariables["SERVER_NAME"], "Envio Senha de Acesso"),
                Subject = "Sistema WEB: Seus dados de acesso",
                SubjectEncoding = System.Text.Encoding.UTF8,
                IsBodyHtml = true,
                Priority = MailPriority.High
            };

            mail.To.Add(new MailAddress(txtEmail.Text, txtNomeUsuario.Text));
            mail.Bcc.Add(new MailAddress("adriano@webview.com.br", "Adriano - WEBVIEW"));

            mail.Body =
                "<img src='https://" + Session["URLORIGEM"] + "/public_html/" + Session["EMAILLOGOTIPO"] + "' width='150'><br/><br/>" +
                "Olá!<br/><br/>" +
                "<b>A seguir segue a sua senha de acesso ao SISTEMA WEB:</b><br/>" +
                "E-Mail: " + txtEmail.Text + "<br/>" +
                "Senha de acesso: " + txtSenha.Text + "<br/><br/>" +
                "<b>NOTA:</b> Este é um e-mail automático.<br/>" +
                Session["EMAILRODAPE"];

            client.Send(mail);

            ClientScript.RegisterStartupScript(this.GetType(),
                "EnvioEmailOK", "alert('Atenção: Foi enviada uma SENHA DE ACESSO para o E-MAIL informado.');", true);
        }
        catch
        {
            ClientScript.RegisterStartupScript(this.GetType(),
                "EnvioEmailERRO", "alert('Atenção: Não foi possível enviar a SENHA DE ACESSO para o E-MAIL informado');", true);
        }
    }

    ClientScript.RegisterStartupScript(this.GetType(),
        "AlertaFinal", "alert('Dados gravados com sucesso'); opener.PostBackOnMainPage(); window.close();", true);
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
        dvGeral.Visible = false;
        dvFisica.Visible = false;
        dvJuridica.Visible = false;
        dvEndereco.Visible = false;
        dvResponsavel.Visible = false;
        dvUsuario.Visible = false;

        if (ddlTipoFJ.SelectedValue.ToString() == "PF")
        {
 //           ClientScript.RegisterStartupScript(this.GetType(),
 //               "CNPJCPF", "$('#txtCNPJ').prop('placeholder', '000.000.000-00');", true);
            lblRazaoSocial.Text = "Nome Completo";
            lblFantasia.Text = "Apelido";
            lblCNPJ.Text = "CPF";
            lblDataAbertura.Text = "Data Início Atividade";

            dvFisica.Visible = true;
            dvEndereco.Visible = true;
            dvGeral.Visible = true;
            dvUsuario.Visible = true;

        }
        else
        {
//            ClientScript.RegisterStartupScript(this.GetType(),
//            "CNPJCPF", "$('#txtCNPJ').prop('placeholder', '00.000.000/0000-00');", true);
            lblRazaoSocial.Text = "Razão Social";
            lblFantasia.Text = "Nome Fantasia";
            lblCNPJ.Text = "CNPJ";
            lblDataAbertura.Text = "Data Abertura Empresa";

            dvJuridica.Visible = true;
            dvEndereco.Visible = true;
            dvResponsavel.Visible = true;
            dvGeral.Visible = true;
            dvUsuario.Visible = true;

        }

    }
    protected void btnPessoal_Click(object sender, EventArgs e)
    {
        ddlTipoFJ.SelectedValue = "PF";
        dvTipo.Visible = false;
        dvCPF.Visible = true;

        dvStepPF01.Visible = true;
        dvStepPF02.Visible = true;
        dvStepPF03.Visible = true;
        dvStepPF04.Visible = true;

        dvStepPJ01.Visible = false;
        dvStepPJ02.Visible = false;
        dvStepPJ03.Visible = false;
        dvStepPJ04.Visible = false;
        dvStepPJ05.Visible = false;
    
    }
    protected void btnNegocio_Click(object sender, EventArgs e)
    {
        ddlTipoFJ.SelectedValue = "PJ";
        dvTipo.Visible = false;
        dvCNPJ.Visible = true;

        dvStepPF01.Visible = false;
        dvStepPF02.Visible = false;
        dvStepPF03.Visible = false;
        dvStepPF04.Visible = false;
        
        dvStepPJ01.Visible = true;
        dvStepPJ02.Visible = true;
        dvStepPJ03.Visible = true;
        dvStepPJ04.Visible = true;
        dvStepPJ05.Visible = true;

    }
    protected void btnAvancar01_Click(object sender, EventArgs e)
    {

    }
    protected void btnVoltar01_Click(object sender, EventArgs e)
    {

    }
    protected void btnVoltar02PJ_Click(object sender, EventArgs e)
    {
        dvCNPJ.Visible = false;
        dvTipo.Visible = true;

    }


    protected void btnAvancar02PJ_Click(object sender, EventArgs e)
    {
        if (dvJuridica.Visible == false)
        {
            using (SqlConnection mySelCadastro = new SqlConnection(Funcoes.conexao()))
            using (SqlCommand cmdSelCadastro = new SqlCommand("dbo.stp_pessoas_fj_ins", mySelCadastro))
            {
                cmdSelCadastro.CommandType = CommandType.StoredProcedure;

                cmdSelCadastro.Parameters.Add("@flg_operacao", SqlDbType.Char).Value = "H";

                if (ddlTipoFJ.SelectedValue == "PJ")
                {
                    cmdSelCadastro.Parameters.Add("@NOM_CNPJ", SqlDbType.VarChar).Value =
                        txtCNPJ.Text ?? "";
                }
                else
                {
                    cmdSelCadastro.Parameters.Add("@NOM_CNPJ", SqlDbType.VarChar).Value =
                        txtDocumentoPF.Text ?? "";
                }

                // 👉 usa a propriedade já carregada da Session
                cmdSelCadastro.Parameters.Add("@COD_ID_PESSOA_LICENCIADO", SqlDbType.Int).Value =
                    Licenciado;

                cmdSelCadastro.Parameters.Add("@FLG_TIPO", SqlDbType.Char).Value = "E";

                mySelCadastro.Open();

                using (SqlDataReader ReaderCadastro = cmdSelCadastro.ExecuteReader())
                {
                    if (ReaderCadastro.Read())
                    {
                        string codId = ReaderCadastro["COD_ID"] != DBNull.Value
                            ? ReaderCadastro["COD_ID"].ToString()
                            : "";

                        string razao = ReaderCadastro["NOM_RAZAOSOCIAL"] != DBNull.Value
                            ? ReaderCadastro["NOM_RAZAOSOCIAL"].ToString()
                            : "";

                        ClientScript.RegisterStartupScript(
                            this.GetType(),
                            "Duplicidade",
                            "alert('Já existe um estabelecimento cadastrado com este documento (" +
                            codId + " - " + razao + ")! Verifique e reentre.');",
                            true);

                        return;
                    }
                }
            }

            dvJuridica.Visible = true;
            dvConsultaEmpresa.Visible = false;
        }
        else
        {
            dvCNPJ.Visible = false;
            dvEndereco.Visible = true;
        }
    }
    
    
    protected void btnVoltar02PF_Click(object sender, EventArgs e)
    {
        dvCPF.Visible = false;
        dvTipo.Visible = true;
    }


    protected void btnAvancar02PF_Click(object sender, EventArgs e)
    {
        if (dvFisica.Visible == false)
        {
            using (SqlConnection mySelCadastro = new SqlConnection(Funcoes.conexao()))
            using (SqlCommand cmdSelCadastro = new SqlCommand("dbo.stp_pessoas_fj_ins", mySelCadastro))
            {
                cmdSelCadastro.CommandType = CommandType.StoredProcedure;
                cmdSelCadastro.Parameters.Add("@flg_operacao", SqlDbType.Char).Value = "H";

                if (ddlTipoFJ.SelectedValue == "PJ")
                {
                    cmdSelCadastro.Parameters.Add("@NOM_CNPJ", SqlDbType.VarChar).Value =
                        txtCNPJ.Text ?? "";
                }
                else
                {
                    cmdSelCadastro.Parameters.Add("@NOM_CNPJ", SqlDbType.VarChar).Value =
                        txtDocumentoPF.Text ?? "";
                }

                // 👉 usa a propriedade já carregada da Session
                cmdSelCadastro.Parameters.Add("@COD_ID_PESSOA_LICENCIADO", SqlDbType.Int).Value =
                    Licenciado;

                cmdSelCadastro.Parameters.Add("@FLG_TIPO", SqlDbType.Char).Value = "E";

                mySelCadastro.Open();

                using (SqlDataReader ReaderCadastro = cmdSelCadastro.ExecuteReader())
                {
                    if (ReaderCadastro.Read())
                    {
                        string codId =
                            ReaderCadastro["COD_ID"] != DBNull.Value
                            ? ReaderCadastro["COD_ID"].ToString()
                            : "";

                        string razao =
                            ReaderCadastro["NOM_RAZAOSOCIAL"] != DBNull.Value
                            ? ReaderCadastro["NOM_RAZAOSOCIAL"].ToString()
                            : "";

                        ClientScript.RegisterStartupScript(
                            this.GetType(),
                            "Duplicidade",
                            "alert('Já existe um estabelecimento cadastrado com este documento (" +
                            codId + " - " + razao + ")! Verifique e reentre.');",
                            true);

                        return;
                    }
                }
            }

            dvFisica.Visible = true;
            dvConsultaPessoa.Visible = false;
        }
        else
        {
            dvCPF.Visible = false;
            dvEndereco.Visible = true;
        }
    }


    protected void btnVoltar03_Click(object sender, EventArgs e)
    {
        if (ddlTipoFJ.SelectedValue.ToString() == "PF")
        {
            dvEndereco.Visible = false;
            dvCPF.Visible = true;
            dvConsultaPessoa.Visible = false;
            dvFisica.Visible = true;
        }
        else
        {
            dvEndereco.Visible = false;
            dvCNPJ.Visible = true;
            dvConsultaEmpresa.Visible = false;
            dvJuridica.Visible = true;

        }
    }
    protected void btnAvancar03_Click(object sender, EventArgs e)
    {
        if (ddlTipoFJ.SelectedValue.ToString() == "PF")
        {
            dvEndereco.Visible = false;
            dvGeral.Visible = true;
        }
        else
        {
            dvEndereco.Visible = false;
            dvResponsavel.Visible = true;
        }
    }
    protected void btnVoltar04_Click(object sender, EventArgs e)
    {
        dvResponsavel.Visible = false;
        dvEndereco.Visible = true;
    }
    protected void btnAvancar04_Click(object sender, EventArgs e)
    {
        dvResponsavel.Visible = false;
        dvGeral.Visible = true;
    }
    protected void btnVoltar05_Click(object sender, EventArgs e)
    {
        if (ddlTipoFJ.SelectedValue.ToString() == "PF")
        {
            dvEndereco.Visible = true;
            dvGeral.Visible = false;
        }
        else
        {
            dvGeral.Visible = false;
            dvResponsavel.Visible = true;
        }

    }
    protected void btnAvancar05_Click(object sender, EventArgs e)
    {
        if (ddlTipoFJ.SelectedValue.ToString() == "PF")
        {
            txtNomeUsuario.Text = txtNomePF.Text.ToString() + " " + txtSobrenomePF.Text.ToString();
            txtEmail.Text = txtEmailPF.Text.ToString();
        }
        else
        {
            txtNomeUsuario.Text = txtNome.Text.ToString() + " " + txtSobrenome.Text.ToString();
            txtEmail.Text = txtEmailResponsavel.Text.ToString();
        }
        btnSalvar.Visible = true;

        dvUsuario.Visible = true;
        dvGeral.Visible = false;
    }
    protected void btnVoltar06_Click(object sender, EventArgs e)
    {
        dvUsuario.Visible = false;
        dvGeral.Visible = true;

    }


    public string CorPrimaria(int cor)
    {
        string urlorigem = "";

        if (Request.ServerVariables["SERVER_NAME"] != null)
            urlorigem = Request.ServerVariables["SERVER_NAME"].ToString();

        string corPrimaria = "";

        using (SqlConnection mySelCadastro = new SqlConnection(Funcoes.conexao()))
        using (SqlCommand cmdSelCadastro = new SqlCommand("dbo.stp_pessoas_fj_ins", mySelCadastro))
        {
            cmdSelCadastro.CommandType = CommandType.StoredProcedure;
            cmdSelCadastro.Parameters.Add("@flg_operacao", SqlDbType.Char).Value = "T";
            cmdSelCadastro.Parameters.Add("@NOM_URL", SqlDbType.VarChar).Value = urlorigem;

            mySelCadastro.Open();

            using (SqlDataReader ReaderCadastro = cmdSelCadastro.ExecuteReader())
            {
                if (ReaderCadastro.Read())   // só precisa de um registro
                {
                    if (cor == 1)
                    {
                        corPrimaria =
                            ReaderCadastro["NOM_COR_PRIMARIA_FUNDO"] != DBNull.Value
                            ? ReaderCadastro["NOM_COR_PRIMARIA_FUNDO"].ToString()
                            : "";
                    }
                    else if (cor == 2)
                    {
                        corPrimaria =
                            ReaderCadastro["NOM_COR_SECUNDARIA_FUNDO"] != DBNull.Value
                            ? ReaderCadastro["NOM_COR_SECUNDARIA_FUNDO"].ToString()
                            : "";
                    }
                }
            }
        }

        return corPrimaria;
    }


    protected void btnPesquisarPF_Click(object sender, EventArgs e)
    {
        dvConsultaPessoa.Visible = true;

        DadosPessoas.Root dpessoas = new DadosPessoas.Root()
        {
            Datasets = "registration_data",
            q = "doc{" + Funcoes.TIRAACENTOSDOCUMENTOS(txtDocumentoPF .Text.ToString())+ "}"
        };

        string json = JsonConvert.SerializeObject(dpessoas);
        string jsonPessoas = bigdatacorp.ConsultarDadosPessoas(json);

        txtRespostaPF.Text = jsonPessoas.ToString();
        
        if (jsonPessoas.ToString().Trim() != "")
        {
            JObject oPessoas = JObject.Parse(jsonPessoas.ToString());
            string sNome = oPessoas["Result"][0]["RegistrationData"]["BasicData"]["Name"].ToString();
            string sEmail = oPessoas["Result"][0]["RegistrationData"]["Emails"]["Primary"]["EmailAddress"].ToString();
            string sCPF = oPessoas["Result"][0]["RegistrationData"]["BasicData"]["TaxIdNumber"].ToString();

            dtPessoas.Clear();
            dtPessoas.Rows.Add(sNome, sEmail, sCPF);

            this.lsvConsultaPessoa.DataSource = dtPessoas;
            this.lsvConsultaPessoa.DataBind();

            bigdatacorp.GravaConsultas(sCPF, sNome, "PF registration_data");
        }
        else
        {
            ClientScript.RegisterStartupScript(this.GetType(), "PessoasErro", "alert('Não foi possível realizar a consulta do CPF informado! Verifique e tente novamente.');", true);
        }
        


    }
    protected void lsvConsultaPessoa_ItemCommand(object sender, ListViewCommandEventArgs e)
    {
        if (e.CommandName == "Utilizar")
        {
            if (txtRespostaPF.Text.ToString().Trim() != "")
            {
                try
                {
                    JObject oPessoas = JObject.Parse(txtRespostaPF.Text.ToString());

                    txtNomePF.Text = oPessoas["Result"][0]["RegistrationData"]["BasicData"]["Name"].ToString();
                    txtNomeMaePF.Text = oPessoas["Result"][0]["RegistrationData"]["BasicData"]["MotherName"].ToString();
                    if (oPessoas["Result"][0]["RegistrationData"]["BasicData"]["BirthDate"].ToString().Trim() != "") { txtNascimentoPF.Text = Convert.ToDateTime(oPessoas["Result"][0]["RegistrationData"]["BasicData"]["BirthDate"].ToString()).ToShortDateString(); }
                    txtEmailPF.Text = oPessoas["Result"][0]["RegistrationData"]["Emails"]["Primary"]["EmailAddress"].ToString();
                    txtCelularPF.Text = "(" + oPessoas["Result"][0]["RegistrationData"]["Phones"]["Primary"]["AreaCode"].ToString() + ")" + oPessoas["Result"][0]["RegistrationData"]["Phones"]["Primary"]["Number"].ToString();

                    txtCEP.Text = oPessoas["Result"][0]["RegistrationData"]["Addresses"]["Primary"]["ZipCode"].ToString();
                    txtEndereco.Text = oPessoas["Result"][0]["RegistrationData"]["Addresses"]["Primary"]["Typology"].ToString() + " " + oPessoas["Result"][0]["RegistrationData"]["Addresses"]["Primary"]["AddressMain"].ToString();
                    txtNumero.Text = oPessoas["Result"][0]["RegistrationData"]["Addresses"]["Primary"]["Number"].ToString();
                    txtComplemento.Text = oPessoas["Result"][0]["RegistrationData"]["Addresses"]["Primary"]["Complement"].ToString();
                    txtBairro.Text = oPessoas["Result"][0]["RegistrationData"]["Addresses"]["Primary"]["Neighborhood"].ToString();
                    txtCidade.Text = oPessoas["Result"][0]["RegistrationData"]["Addresses"]["Primary"]["City"].ToString();
                    ddlEstado.SelectedValue = oPessoas["Result"][0]["RegistrationData"]["Addresses"]["Primary"]["State"].ToString();

                    ClientScript.RegisterStartupScript(this.GetType(), "UtilizarSucesso", "alert('Os dados foram utilizados com sucesso!');", true);
                    btnAvancar02PF_Click(null, null);

                }
                catch
                {
                    ClientScript.RegisterStartupScript(this.GetType(), "UtilizarErro", "alert('Não foi possível utilizar os dados do CPF informado! Verifique e tente novamente.');", true);
                }

            }
        }

    }

    protected void btnPesquisarPJ_Click(object sender, EventArgs e)
    {

        dvConsultaEmpresa.Visible = true;
        dtEmpresas.Clear();

        DadosEmpresas.Root dempresas = new DadosEmpresas.Root()
        {
            Datasets = "registration_data",
            q = "doc{" + Funcoes.TIRAACENTOSDOCUMENTOS(txtCNPJ.Text.ToString()) + "}"
        };

        string json = JsonConvert.SerializeObject(dempresas);
        string jsonEmpresas = bigdatacorp.ConsultarDadosEmpresas(json);
        txtRespostaPJ.Text = jsonEmpresas.ToString();

        if (jsonEmpresas.ToString().Trim() != "")
        {
            JObject oEmpresas = JObject.Parse(jsonEmpresas.ToString());
            string sRazaosocial = oEmpresas["Result"][0]["RegistrationData"]["BasicData"]["OfficialName"].ToString();
            string sNomeFantasia = oEmpresas["Result"][0]["RegistrationData"]["BasicData"]["TradeName"].ToString();
            string sCNPJ = oEmpresas["Result"][0]["RegistrationData"]["BasicData"]["TaxIdNumber"].ToString();

            try
            {
                dtEmpresas.Clear();
                dtEmpresas.Rows.Add(sRazaosocial, sNomeFantasia, sCNPJ);

                this.lsvConsultaEmpresa.DataSource = dtEmpresas;
                this.lsvConsultaEmpresa.DataBind();

                bigdatacorp.GravaConsultas(sCNPJ, sRazaosocial, "PJ registration_data");
            }
            catch
            {

            }
        }
        else
        {
            ClientScript.RegisterStartupScript(this.GetType(), "PessoasErro", "alert('Não foi possível realizar a consulta do CNPJ informado! Verifique e tente novamente.');", true);
        }

    }
    protected void lsvConsultaEmpresa_ItemCommand(object sender, ListViewCommandEventArgs e)
    {

        if (e.CommandName == "Utilizar")
        {
            if (txtRespostaPJ.Text.ToString().Trim() != "")
            {
                try
                {
                    JObject oEmpresas = JObject.Parse(txtRespostaPJ.Text.ToString());

                    txtRazaoSocial.Text = oEmpresas["Result"][0]["RegistrationData"]["BasicData"]["OfficialName"].ToString();
                    txtFantasia.Text = (oEmpresas["Result"][0]["RegistrationData"]["BasicData"]["TradeName"].ToString().Trim()!="")? oEmpresas["Result"][0]["RegistrationData"]["BasicData"]["TradeName"].ToString().Trim(): oEmpresas["Result"][0]["RegistrationData"]["BasicData"]["OfficialName"].ToString().Trim();
                    txtTelefoneEmpresa.Text = "(" + oEmpresas["Result"][0]["RegistrationData"]["Phones"]["Primary"]["AreaCode"].ToString() + ")" + oEmpresas["Result"][0]["RegistrationData"]["Phones"]["Primary"]["Number"].ToString();
                    txtEmailEmpresa.Text = oEmpresas["Result"][0]["RegistrationData"]["Emails"]["Primary"]["EmailAddress"].ToString();
                    if (oEmpresas["Result"][0]["RegistrationData"]["BasicData"]["FoundedDate"].ToString().Trim() != "") { txtDataAbertura.Text = Convert.ToDateTime(oEmpresas["Result"][0]["RegistrationData"]["BasicData"]["FoundedDate"].ToString()).ToShortDateString(); }
                    txtPatrimonio.Text = "";
                    txtFaturamento.Text = "";
                    // Consulta MCC

                    // Realizar a consulta do MCC

                    bigdatacorp.DadosConsultas.Root dmcc = new bigdatacorp.DadosConsultas.Root()
                    {
                        q = "doc{" + Funcoes.TIRAACENTOSDOCUMENTOS(txtCNPJ.Text.ToString()) + "}",
                        Datasets = "merchant_category_data"
                    };

                    string jsonMcc = JsonConvert.SerializeObject(dmcc);

                    bigdatacorp.HttpResponseResult resultadoMcc = bigdatacorp.ConsultaCategoriaComercial(jsonMcc);

                    if (resultadoMcc.StatusCode == 200)
                    {
                        string jsonResponseMcc = resultadoMcc.Content;

                        JObject oMcc = JObject.Parse(jsonResponseMcc.ToString());
                        if (oMcc["Result"].Count() > 0)
                        {
                            if (oMcc["Result"][0]["MerchantCategoryData"]["CNAECategories"].Count() > 0)
                            {
                                ddlAtividadeEconomica.SelectedValue = oMcc["Result"][0]["MerchantCategoryData"]["CNAECategories"][0]["MCC"].ToString();
                            }
                        }
                    }


                    // Realizar a consulta dos dados do sócio

                    bigdatacorp.DadosConsultas.Root dconsulta = new bigdatacorp.DadosConsultas.Root()
                    {
                        q = "doc{" + Funcoes.TIRAACENTOSDOCUMENTOS(txtCNPJ.Text.ToString()) + "}",
                        Datasets = "ondemand_legal_representative_company"
                    };

                    string json = JsonConvert.SerializeObject(dconsulta);

                    bigdatacorp.HttpResponseResult resultado = bigdatacorp.ConsultaRepresentanteLegal(json);

                    if (resultado.StatusCode == 200)
                    {
                        // Sucesso - Processar JSON normalmente
                        string jsonResponse = resultado.Content;

                        JObject oRepresentante = JObject.Parse(jsonResponse.ToString());
                        if (oRepresentante["Result"].Count() > 0)
                        {
                            if (oRepresentante["Result"][0]["OnlineQuery"].Count() > 0)
                            {
                                txtNome.Text = oRepresentante["Result"][0]["OnlineQuery"][0]["QueryResultData"]["Name"].ToString();
                                txtSobrenome.Text = "";
                                txtDocumento.Text = oRepresentante["Result"][0]["OnlineQuery"][0]["QueryResultData"]["TaxIdNumber"].ToString();

                                bigdatacorp.GravaConsultas(Funcoes.TIRAACENTOSDOCUMENTOS(txtCNPJ.Text.ToString()), txtRazaoSocial.Text.ToString(), "PJ ondemand_legal_representative_company");


                                // Se tem representante legal... realizar a copnsulta da pessoa física
                                
                                DadosPessoas.Root dpessoas = new DadosPessoas.Root()
                                {
                                    Datasets = "registration_data",
                                    q = "doc{" + Funcoes.TIRAACENTOSDOCUMENTOS(oRepresentante["Result"][0]["OnlineQuery"][0]["QueryResultData"]["TaxIdNumber"].ToString()) + "}"
                                };

                                string jsonRepresentante = JsonConvert.SerializeObject(dpessoas);

                                string jsonPessoas = bigdatacorp.ConsultarDadosPessoas(jsonRepresentante);

                                if (jsonPessoas.ToString().Trim() != "")
                                {
                                    JObject oPessoas = JObject.Parse(jsonPessoas.ToString());
                                    txtEmailResponsavel.Text = oPessoas["Result"][0]["RegistrationData"]["Emails"]["Primary"]["EmailAddress"].ToString();
                                    txtNomeMae.Text = oPessoas["Result"][0]["RegistrationData"]["BasicData"]["MotherName"].ToString();
                                    
                                    if (oPessoas["Result"][0]["RegistrationData"]["BasicData"]["BirthDate"].ToString().Trim() != "") { txtNascimento.Text = Convert.ToDateTime(oPessoas["Result"][0]["RegistrationData"]["BasicData"]["BirthDate"].ToString()).ToShortDateString(); }
                                    
                                    txtCelular.Text = "(" + oPessoas["Result"][0]["RegistrationData"]["Phones"]["Primary"]["AreaCode"].ToString() + ")" + oPessoas["Result"][0]["RegistrationData"]["Phones"]["Primary"]["Number"].ToString();

                                    bigdatacorp.GravaConsultas(oRepresentante["Result"][0]["OnlineQuery"][0]["QueryResultData"]["TaxIdNumber"].ToString(), oRepresentante["Result"][0]["OnlineQuery"][0]["QueryResultData"]["Name"].ToString(), "PF registration_data");
                                }

 
                            }
                        }
                    }


                    // Fim da consulta do sócio




                    //txtNome.Text = oPessoas["Result"][0]["RegistrationData"]["BasicData"]["Name"].ToString();
                    //txtNomeMae.Text = oPessoas["Result"][0]["RegistrationData"]["BasicData"]["MotherName"].ToString();
                    //if (oPessoas["Result"][0]["RegistrationData"]["BasicData"]["BirthDate"].ToString().Trim() != "") { txtNascimento.Text = Convert.ToDateTime(oPessoas["Result"][0]["RegistrationData"]["BasicData"]["BirthDate"].ToString()).ToShortDateString(); }
                    //txtEmail.Text = oPessoas["Result"][0]["RegistrationData"]["Emails"]["Primary"]["EmailAddress"].ToString();
                    //txtCelular.Text = "(" + oPessoas["Result"][0]["RegistrationData"]["Phones"]["Primary"]["AreaCode"].ToString() + ")" + oPessoas["Result"][0]["RegistrationData"]["Phones"]["Primary"]["Number"].ToString();
                    
                    txtCEP.Text = oEmpresas["Result"][0]["RegistrationData"]["Addresses"]["Primary"]["ZipCode"].ToString();
                    txtEndereco.Text = oEmpresas["Result"][0]["RegistrationData"]["Addresses"]["Primary"]["Typology"].ToString() + " " + oEmpresas["Result"][0]["RegistrationData"]["Addresses"]["Primary"]["AddressMain"].ToString();
                    txtNumero.Text = oEmpresas["Result"][0]["RegistrationData"]["Addresses"]["Primary"]["Number"].ToString();
                    txtComplemento.Text = oEmpresas["Result"][0]["RegistrationData"]["Addresses"]["Primary"]["Complement"].ToString();
                    txtBairro.Text = oEmpresas["Result"][0]["RegistrationData"]["Addresses"]["Primary"]["Neighborhood"].ToString();
                    txtCidade.Text = oEmpresas["Result"][0]["RegistrationData"]["Addresses"]["Primary"]["City"].ToString();
                    ddlEstado.SelectedValue = oEmpresas["Result"][0]["RegistrationData"]["Addresses"]["Primary"]["State"].ToString();

                    ClientScript.RegisterStartupScript(this.GetType(), "UtilizarSucesso", "alert('Os dados foram utilizados com sucesso!');", true);
                    btnAvancar02PJ_Click(null, null);

                }
                catch
                {
                    ClientScript.RegisterStartupScript(this.GetType(), "UtilizarErro", "alert('Não foi possível utilizar os dados do CNPJ informado! Verifique e tente novamente.');", true);
                }

            }
        }

    }
}