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


public partial class cad_parceiros : System.Web.UI.Page
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

        if (!IsPostBack)
        {

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
            }
        }

    }

    private void ConsultaFicha()
    {
        SqlConnection mySelCadastro = new SqlConnection(Funcoes.conexao());
        mySelCadastro.Open();
        SqlCommand cmdSelCadastro = new SqlCommand("dbo.stp_parceiros_ins", mySelCadastro);
        cmdSelCadastro.CommandType = CommandType.StoredProcedure;
        cmdSelCadastro.Parameters.Add("@flg_operacao", SqlDbType.Char).Value = "S";
        cmdSelCadastro.Parameters.Add("@COD_ID", SqlDbType.Int).Value = Funcoes.strToInt(sid_id.ToString());
        cmdSelCadastro.Parameters.Add("@COD_ID_PESSOAS_FJ", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["PESSOA"].ToString());
        cmdSelCadastro.Parameters.Add("@COD_ID_PESSOA_LICENCIADO", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["LICENCIADO"].ToString());
        SqlDataReader ReaderCadastro = cmdSelCadastro.ExecuteReader();
        while (ReaderCadastro.Read())
        {
            txtID.Text = ReaderCadastro["COD_ID"].ToString();
            txtToken.Text = ReaderCadastro["NOM_ID_CADASTRO"].ToString();


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

            txtRazaoSocial.Text = ReaderCadastro["NOM_RAZAOSOCIAL"].ToString();
            txtFantasia.Text = ReaderCadastro["NOM_FANTASIA"].ToString();

            txtCNPJ.Text = ReaderCadastro["NOM_CNPJ"].ToString();

            txtTelefoneEmpresa.Text = ReaderCadastro["NUM_TELEFONE"].ToString();
            txtEmailEmpresa.Text = ReaderCadastro["NOM_EMAIL_EMPRESA"].ToString();
            txtDataAbertura.Text = ReaderCadastro["DTA_ABERTURA"].ToString();
            ddlTipoEmpresa.SelectedValue = ReaderCadastro["NOM_TIPO_EMPRESA"].ToString();
            ddlAtividadeEconomica.SelectedValue = ReaderCadastro["COD_ID_MCC"].ToString();

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
            txtEmailResponsavel.Text = ReaderCadastro["NOM_EMAIL"].ToString();
            txtCelular.Text = ReaderCadastro["NOM_CELULAR"].ToString();


            SqlConnection mySelCadastroContas = new SqlConnection(Funcoes.conexao());
            mySelCadastroContas.Open();
            SqlCommand cmdSelCadastroContas = new SqlCommand("dbo.stp_pessoas_fj_contas_ins", mySelCadastroContas);
            cmdSelCadastroContas.CommandType = CommandType.StoredProcedure;
            cmdSelCadastroContas.Parameters.Add("@flg_operacao", SqlDbType.Char).Value = "X";
            cmdSelCadastroContas.Parameters.Add("@COD_ID_PESSOAS_FJ", SqlDbType.Int).Value = Funcoes.strToInt(sid_id.ToString());
            cmdSelCadastroContas.Parameters.Add("@COD_ID_PESSOA_LICENCIADO", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["LICENCIADO"].ToString());
            SqlDataReader ReaderCadastroContas = cmdSelCadastroContas.ExecuteReader();
            while (ReaderCadastroContas.Read())
            {
                ddlInstituicaoFinanceira.SelectedValue = ReaderCadastroContas["NOM_CODIGO_BANCO"].ToString();
                ddlTipoConta.SelectedValue = ReaderCadastroContas["NOM_TIPO_BANCO"].ToString();
                txtAgencia.Text = ReaderCadastroContas["NOM_NUMERO_AGENCIA_BANCO"].ToString();
                txtDigitoAgencia.Text = ReaderCadastroContas["NOM_NUMERO_DIGITO_AGENCIA_BANCO"].ToString();
                txtConta.Text = ReaderCadastroContas["NOM_NUMERO_CONTA_BANCO"].ToString();
                txtDigitoConta.Text = ReaderCadastroContas["NOM_NUMERO_DIGITO_CONTA_BANCO"].ToString();
                txtTokenConta.Text = ReaderCadastroContas["NOM_TOKEN"].ToString();
            }

        }
        // Consulta Dados Conta


    }

    protected void btnSalvar_Click(object sender, EventArgs e)
    {

        if (Funcoes.strToInt(sid_id) == 0)
        {
            SqlConnection mySelCadastro = new SqlConnection(Funcoes.conexao());
            mySelCadastro.Open();
            SqlCommand cmdSelCadastro = new SqlCommand("dbo.stp_parceiros_ins", mySelCadastro);
            cmdSelCadastro.CommandType = CommandType.StoredProcedure;
            cmdSelCadastro.Parameters.Add("@flg_operacao", SqlDbType.Char).Value = "H";
            cmdSelCadastro.Parameters.Add("@NOM_CNPJ", SqlDbType.VarChar).Value = txtCNPJ.Text.ToString();
            cmdSelCadastro.Parameters.Add("@COD_ID_PESSOAS_FJ", SqlDbType.Int).Value = Funcoes.strToInt(txtID.Text.ToString());
            cmdSelCadastro.Parameters.Add("@COD_ID_PESSOA_LICENCIADO", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["LICENCIADO"].ToString());
            cmdSelCadastro.Parameters.Add("@FLG_TIPO", SqlDbType.Char).Value = "P";

            SqlDataReader ReaderCadastro = cmdSelCadastro.ExecuteReader();
            while (ReaderCadastro.Read())
            {
                ClientScript.RegisterStartupScript(this.GetType(),
                    "Duplicidade", "alert('Já existe um parceiro cadastrado com este documento (" + ReaderCadastro["COD_ID"].ToString() + " - " + ReaderCadastro["NOM_RAZAOSOCIAL"].ToString() + ")! Verifique e reentre.');", true);
                return;
            }
        }
        
        CadastrarEstabelecimento();
        
        SqlConnection connInsCons = new SqlConnection(Funcoes.conexao());
        connInsCons.Open();
        SqlCommand cmdInsCons = new SqlCommand("dbo.stp_parceiros_ins", connInsCons);
        cmdInsCons.CommandType = CommandType.StoredProcedure;
        cmdInsCons.Parameters.Add("@flg_operacao", SqlDbType.Char).Value = 'I';

        cmdInsCons.Parameters.Add("@FLG_ATIVO", SqlDbType.Char).Value = "S";
        cmdInsCons.Parameters.Add("@COD_ID_PESSOA_LICENCIADO", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["LICENCIADO"].ToString());
        cmdInsCons.Parameters.Add("@COD_ID_PESSOAS_FJ", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["PESSOA"].ToString());
        cmdInsCons.Parameters.Add("@DTA_DATA", SqlDbType.DateTime).Value = DateTime.Now;
        cmdInsCons.Parameters.Add("@FLG_TIPO", SqlDbType.Char).Value = "P";

        cmdInsCons.Parameters.Add("@FLG_TIPO_PESSOA", SqlDbType.VarChar).Value = ddlTipoFJ.SelectedValue.ToString();

        // Empresa
        cmdInsCons.Parameters.Add("@NOM_RAZAOSOCIAL", SqlDbType.VarChar).Value = txtRazaoSocial.Text.ToString();
        cmdInsCons.Parameters.Add("@NOM_FANTASIA", SqlDbType.VarChar).Value = txtFantasia.Text.ToString();
        cmdInsCons.Parameters.Add("@NOM_CNPJ", SqlDbType.VarChar).Value = txtCNPJ.Text.ToString();

        cmdInsCons.Parameters.Add("@NUM_TELEFONE", SqlDbType.VarChar).Value = txtTelefoneEmpresa.Text.ToString();
        cmdInsCons.Parameters.Add("@NOM_EMAIL_EMPRESA", SqlDbType.VarChar).Value = txtEmailEmpresa.Text.ToString();
        cmdInsCons.Parameters.Add("@COD_ID_MCC", SqlDbType.Int).Value = Funcoes.strToInt(ddlAtividadeEconomica.SelectedValue.ToString());
        cmdInsCons.Parameters.Add("@NOM_TIPO_EMPRESA", SqlDbType.VarChar).Value = ddlTipoEmpresa.SelectedValue.ToString();
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
        cmdInsCons.Parameters.Add("@NOM_EMAIL", SqlDbType.VarChar).Value = txtEmailResponsavel.Text.ToString();
        cmdInsCons.Parameters.Add("@NOM_CELULAR", SqlDbType.VarChar).Value = txtCelular.Text.ToString();

        cmdInsCons.Parameters.Add("@NOM_ID_CADASTRO", SqlDbType.VarChar).Value = txtToken.Text.ToString();
        cmdInsCons.Parameters.Add("@FLG_INTEGRACAO", SqlDbType.Char).Value = "Z";

        txtID.Text = cmdInsCons.ExecuteScalar().ToString();
        connInsCons.Close();
        connInsCons.Dispose();

        CadastrarContas();

        //ClientScript.RegisterStartupScript(this.GetType(), "Alerta", "alert('Dados gravados com sucesso'); ", true);
        ClientScript.RegisterStartupScript(this.GetType(), "Alerta", "alert('Dados gravados com sucesso'); opener.PostBackOnMainPage(); window.close();", true);
    }


    private void CadastrarEstabelecimento()
    {
//        ConsultaID();

        if (txtToken.Text.ToString().Trim() == "")
        {
            // Enviar os dados para ZOOP

            string json = "";

            if (ddlTipoFJ.SelectedValue.ToString().Trim() == "PF")
            {
                json = json + "{";
                json = json + "\"first_name\":\"" + txtNome.Text.ToString().Trim() + "\",";
                json = json + "\"last_name\":\"" + txtSobrenome.Text.ToString().Trim() + "\",";
                json = json + "\"email\":\"" + txtEmailResponsavel.Text.ToString().Trim() + "\",";
                json = json + "\"phone_number\":\"" + TIRAACENTOS(txtCelular.Text.ToString().Trim()) + "\",";
                json = json + "\"taxpayer_id\":\"" + TIRAACENTOS(txtDocumento.Text.ToString().Trim()) + "\",";
                json = json + "\"birthdate\":\"" + Convert.ToDateTime(txtNascimento.Text.ToString().Trim()).Year.ToString().PadLeft(4, '0') + "-" + Convert.ToDateTime(txtNascimento.Text.ToString().Trim()).Month.ToString().PadLeft(2, '0') + "-" + Convert.ToDateTime(txtNascimento.Text.ToString().Trim()).Day.ToString().PadLeft(2, '0') + "\",";
                json = json + "\"statement_descriptor\":\"" + TIRAACENTOS(txtFantasia.Text.ToString().Trim()) + "\",";
                json = json + "\"revenue\":\"" + "10000" + "\",";


                json = json + "\"address\":{";
                json = json + "\"line1\":\"" + txtEndereco.Text.ToString().Trim() + "\",";
                json = json + "\"line2\":\"" + txtNumero.Text.ToString().Trim() + "\",";
                json = json + "\"line3\":\"" + txtComplemento.Text.ToString().Trim() + "\",";
                json = json + "\"neighborhood\":\"" + txtBairro.Text.ToString().Trim() + "\",";
                json = json + "\"city\":\"" + txtCidade.Text.ToString().Trim() + "\",";
                json = json + "\"state\":\"" + ddlEstado.SelectedValue.ToString().Trim() + "\",";
                json = json + "\"postal_code\":\"" + TIRAACENTOS(txtCEP.Text.ToString().Trim()) + "\",";
                json = json + "\"country_code\":\"" + "BR" + "\"";
                if (txtToken.Text.ToString().Trim() == "")
                {
                    json = json + "},";
                    json = json + "\"mcc\":\"" + ddlAtividadeEconomica.SelectedValue.ToString().Trim() + "\"}";
                }
                else
                {
                    json = json + "}}";
                }
            }

            if (ddlTipoFJ.SelectedValue.ToString().Trim() == "PJ")
            {
                json = json + "{ \"owner\":{";
                json = json + "\"first_name\":\"" + txtNome.Text.ToString().Trim() + "\",";
                json = json + "\"last_name\":\"" + txtSobrenome.Text.ToString().Trim() + "\",";
                json = json + "\"email\":\"" + txtEmailResponsavel.Text.ToString().Trim() + "\",";
                json = json + "\"phone_number\":\"" + TIRAACENTOS(txtCelular.Text.ToString().Trim()) + "\",";
                json = json + "\"taxpayer_id\":\"" + TIRAACENTOS(txtDocumento.Text.ToString().Trim()) + "\",";
                json = json + "\"birthdate\":\"" + Convert.ToDateTime(txtNascimento.Text.ToString().Trim()).Year.ToString().PadLeft(4, '0') + "-" + Convert.ToDateTime(txtNascimento.Text.ToString().Trim()).Month.ToString().PadLeft(2, '0') + "-" + Convert.ToDateTime(txtNascimento.Text.ToString().Trim()).Day.ToString().PadLeft(2, '0') + "\"";
                json = json + "},";
                json = json + "\"description\":\"" + txtRazaoSocial.Text.ToString().Trim() + "\",";
                json = json + "\"business_name\":\"" + txtRazaoSocial.Text.ToString().Trim() + "\",";
                json = json + "\"business_phone\":\"" + TIRAACENTOS(txtTelefoneEmpresa.Text.ToString().Trim()) + "\",";
                json = json + "\"business_email\":\"" + txtEmailEmpresa.Text.ToString().Trim() + "\",";
                json = json + "\"business_description\":\"" + txtRazaoSocial.Text.ToString().Trim() + "\",";
                json = json + "\"ein\":\"" + TIRAACENTOS(txtCNPJ.Text.ToString().Trim()) + "\",";
                json = json + "\"statement_descriptor\":\"" + txtFantasia.Text.ToString().Trim() + "\",";
                json = json + "\"revenue\":\"" + "10000" + "\",";

                json = json + "\"business_address\":{";
                json = json + "\"line1\":\"" + txtEndereco.Text.ToString().Trim() + "\",";
                json = json + "\"line2\":\"" + txtNumero.Text.ToString().Trim() + "\",";
                json = json + "\"line3\":\"" + txtComplemento.Text.ToString().Trim() + "\",";
                json = json + "\"neighborhood\":\"" + txtBairro.Text.ToString().Trim() + "\",";
                json = json + "\"city\":\"" + txtCidade.Text.ToString().Trim() + "\",";
                json = json + "\"state\":\"" + ddlEstado.SelectedValue.ToString().Trim() + "\",";
                json = json + "\"postal_code\":\"" + TIRAACENTOS(txtCEP.Text.ToString().Trim()) + "\",";
                json = json + "\"country_code\":\"" + "BR" + "\"";
                json = json + "},";
                if (txtDataAbertura.Text.ToString().Trim() != "")
                {
                    json = json + "\"business_opening_date\":\"" + Convert.ToDateTime(txtDataAbertura.Text.ToString().Trim()).Year.ToString().PadLeft(4, '0') + "-" + Convert.ToDateTime(txtDataAbertura.Text.ToString().Trim()).Month.ToString().PadLeft(2, '0') + "-" + Convert.ToDateTime(txtDataAbertura.Text.ToString().Trim()).Day.ToString().PadLeft(2, '0') + "\",";
                }

                json = json + "\"owner_address\":{";
                json = json + "\"line1\":\"" + txtEndereco.Text.ToString().Trim() + "\",";
                json = json + "\"line2\":\"" + txtNumero.Text.ToString().Trim() + "\",";
                json = json + "\"line3\":\"" + txtComplemento.Text.ToString().Trim() + "\",";
                json = json + "\"neighborhood\":\"" + txtBairro.Text.ToString().Trim() + "\",";
                json = json + "\"city\":\"" + txtCidade.Text.ToString().Trim() + "\",";
                json = json + "\"state\":\"" + ddlEstado.SelectedValue.ToString().Trim() + "\",";
                json = json + "\"postal_code\":\"" + TIRAACENTOS(txtCEP.Text.ToString().Trim()) + "\",";
                json = json + "\"country_code\":\"" + "BR" + "\"";
                if (txtToken.Text.ToString().Trim() == "")
                {
                    json = json + "},";
                    json = json + "\"mcc\":\"" + ddlAtividadeEconomica.SelectedValue.ToString().Trim() + "\"}";
                }
                else
                {
                    json = json + "}}";
                }
            }

            try
            {
                if (ddlTipoFJ.SelectedValue.ToString().Trim() == "PJ")
                {
                    string jsonRetorno = "";
                    try
                    {
                        if (txtToken.Text.ToString().Trim() == "")
                        {
                            jsonRetorno = zoop.CadastrarVendedorPJ(json);
                            try
                            {
                                JObject oEstabelecimento = JObject.Parse(jsonRetorno);
                                txtToken.Text = oEstabelecimento["id"].ToString();
                                txtStatus.Text = oEstabelecimento["status"].ToString();
                                ClientScript.RegisterStartupScript(this.GetType(), "SucessoPJ", "alert('Dados do Estabelecimento PJ " + oEstabelecimento["ein"].ToString() + " gravados com sucesso!');", true);
                            }
                            catch
                            {
                                ClientScript.RegisterStartupScript(this.GetType(), "ErroPJ", "alert('Ocorreu um erro ao tentar cadastrar o estabelecimento PJ na Adquirente! Verifique o cadastro e tente novamente');", true);
                            }
                        }
                        else
                        {
                            jsonRetorno = zoop.AlterarVendedorPJ(json, txtToken.Text.ToString());
                            ClientScript.RegisterStartupScript(this.GetType(), "Sucesso", "alert('Dados do Estabelecimento PJ alterados com sucesso!');", true);
                        }
                    }
                    catch
                    {
                        ClientScript.RegisterStartupScript(this.GetType(), "ErroGERALPJ", "alert('Ocorreu um erro ao tentar cadastrar o estabelecimento PJ na Adquirente! Verifique o cadastro e tente novamente');", true);
                    }
                    //txtAgencia.Text = jsonRetorno;
                }
                if (ddlTipoFJ.SelectedValue.ToString().Trim() == "PF")
                {
                    var jsonRetorno = "";
                    try
                    {
                        if (txtToken.Text.ToString().Trim() == "")
                        {
                            jsonRetorno = zoop.CadastrarVendedorPF(json);
                            try
                            {
                                JObject oEstabelecimento = JObject.Parse(jsonRetorno);
                                txtToken.Text = oEstabelecimento["id"].ToString();
                                txtStatus.Text = oEstabelecimento["status"].ToString();

                                ClientScript.RegisterStartupScript(this.GetType(), "Sucesso", "alert('Dados do Estabelecimento PF " + oEstabelecimento["taxpayer_id"].ToString() + " gravados com sucesso!');", true);
                            }
                            catch
                            {
                                ClientScript.RegisterStartupScript(this.GetType(), "ErroPF", "alert('Ocorreu um erro ao tentar cadastrar o estabelecimento PF na Adquirente! Verifique o cadastro e tente novamente');", true);
                            }
                        }
                        else
                        {
                            jsonRetorno = zoop.AlterarVendedorPF(json, txtToken.Text.ToString());
                            ClientScript.RegisterStartupScript(this.GetType(), "Sucesso", "alert('Dados do Estabelecimento PF alterados com sucesso!');", true);
                        }
                    }
                    catch
                    {
                        ClientScript.RegisterStartupScript(this.GetType(), "ErroGERALPF", "alert('Ocorreu um erro ao tentar cadastrar o estabelecimento PJ na Adquirente! Verifique o cadastro e tente novamente');", true);
                    }
                    //txtAgencia.Text = jsonRetorno;
                }

            }
            catch (WebException ex)
            {
                var resp = new StreamReader(ex.Response.GetResponseStream()).ReadToEnd();
                JObject o = JObject.Parse(resp);
                ClientScript.RegisterStartupScript(this.GetType(), "Erro", "alert('CADASTRO: Ocorreu um Erro: " + o["error"]["status_code"].ToString() + " - Motivo: " + o["error"]["category"].ToString() + " - Descrição: " + o["error"]["message"].ToString() + "');", true);
            }
        }
        ConsultaID();
    }




    private void ConsultaID()
    {
        var json = "";

        if (ddlTipoFJ.SelectedValue.ToString().Trim() == "PF")
        {
            json = zoop.ConsultaCadastroSeller(TIRAACENTOS(txtCNPJ.Text.ToString()));
        }
        if (ddlTipoFJ.SelectedValue.ToString().Trim() == "PJ")
        {
            json = zoop.ConsultaCadastroSeller(TIRAACENTOS(txtCNPJ.Text.ToString()));
        }

        JObject o = JObject.Parse(json);
        try
        {
            txtToken.Text = o["id"].ToString();
            txtStatus.Text = o["status"].ToString();
            ClientScript.RegisterStartupScript(this.GetType(), "SucessoLocalizacao", "alert('O parceiro foi localizado na adquirente: " + o["id"] + "');", true);
        }
        catch
        {
            try
            {
                ClientScript.RegisterStartupScript(this.GetType(), "ErroEspecifico", "alert('BUSCA - Ocorreu um Erro: " + o["error"]["status_code"].ToString() + " - Motivo: " + o["error"]["category"].ToString() + " - Descrição: " + o["error"]["message"].ToString() + "');", true);
            }
            catch
            {
                ClientScript.RegisterStartupScript(this.GetType(), "ErroGeral", "alert('Ocorreu um erro ao tentar localizar o estabelecimento! Verifique e tente novamente');", true);
            }
        }
    }

    private void CadastrarContas()
    {

        SqlConnection connInsConsContas = new SqlConnection(Funcoes.conexao());
        connInsConsContas.Open();
        SqlCommand cmdInsConsContas = new SqlCommand("dbo.stp_pessoas_fj_contas_ins", connInsConsContas);
        cmdInsConsContas.CommandType = CommandType.StoredProcedure;
        cmdInsConsContas.Parameters.Add("@flg_operacao", SqlDbType.Char).Value = 'I';
        cmdInsConsContas.Parameters.Add("@COD_ID_PESSOA_LICENCIADO", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["LICENCIADO"].ToString());
        cmdInsConsContas.Parameters.Add("@COD_ID_PESSOAS_FJ", SqlDbType.Int).Value = Funcoes.strToInt(txtID.Text.ToString());

        cmdInsConsContas.Parameters.Add("@NOM_CODIGO_BANCO", SqlDbType.VarChar).Value = ddlInstituicaoFinanceira.SelectedValue.ToString();
        cmdInsConsContas.Parameters.Add("@NOM_TIPO_BANCO", SqlDbType.VarChar).Value = ddlTipoConta.SelectedValue.ToString();
        cmdInsConsContas.Parameters.Add("@NOM_NUMERO_AGENCIA_BANCO", SqlDbType.VarChar).Value = txtAgencia.Text.ToString();
        cmdInsConsContas.Parameters.Add("@NOM_NUMERO_DIGITO_AGENCIA_BANCO", SqlDbType.VarChar).Value = txtDigitoAgencia.Text.ToString();
        cmdInsConsContas.Parameters.Add("@NOM_NUMERO_CONTA_BANCO", SqlDbType.VarChar).Value = txtConta.Text.ToString();
        cmdInsConsContas.Parameters.Add("@NOM_NUMERO_DIGITO_CONTA_BANCO", SqlDbType.VarChar).Value = txtDigitoConta.Text.ToString();

        cmdInsConsContas.Parameters.Add("@FLG_PADRAO", SqlDbType.Char).Value = "S";

        cmdInsConsContas.ExecuteNonQuery();
        connInsConsContas.Close();
        connInsConsContas.Dispose();

        string sIDTokenConta = "";
        string sIDConta = "";
        string sIDContaCodigo = "";

        SqlConnection mySelCadastro = new SqlConnection(Funcoes.conexao());
        mySelCadastro.Open();
        SqlCommand cmdSelCadastro = new SqlCommand("dbo.stp_pessoas_fj_ins", mySelCadastro);
        cmdSelCadastro.CommandType = CommandType.StoredProcedure;
        cmdSelCadastro.Parameters.Add("@flg_operacao", SqlDbType.Char).Value = "S";
        cmdSelCadastro.Parameters.Add("@COD_ID", SqlDbType.Int).Value = Funcoes.strToInt(txtID.Text.ToString());
        cmdSelCadastro.Parameters.Add("@COD_ID_PESSOA_LICENCIADO", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["LICENCIADO"].ToString());
        SqlDataReader ReaderCadastro = cmdSelCadastro.ExecuteReader();
        while (ReaderCadastro.Read())
        {
            SqlConnection mySelCadastroContas = new SqlConnection(Funcoes.conexao());
            mySelCadastroContas.Open();
            SqlCommand cmdSelCadastroContas = new SqlCommand("dbo.stp_pessoas_fj_contas_ins", mySelCadastroContas);
            cmdSelCadastroContas.CommandType = CommandType.StoredProcedure;
            cmdSelCadastroContas.Parameters.Add("@flg_operacao", SqlDbType.Char).Value = "X";
            cmdSelCadastroContas.Parameters.Add("@COD_ID_PESSOAS_FJ", SqlDbType.Int).Value = Funcoes.strToInt(txtID.Text.ToString());
            cmdSelCadastroContas.Parameters.Add("@COD_ID_PESSOA_LICENCIADO", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["LICENCIADO"].ToString());
            SqlDataReader ReaderCadastroContas = cmdSelCadastroContas.ExecuteReader();
            while (ReaderCadastroContas.Read())
            {

                if (ReaderCadastroContas["NOM_TOKEN"].ToString().Trim() == "")
                {

                    sIDContaCodigo = ReaderCadastroContas["COD_ID"].ToString().Trim();

                    CriarContaBancaria.root dconta = new CriarContaBancaria.root()
                    {
                        holder_name = ReaderCadastro["NOM_RAZAOSOCIAL"].ToString().Trim(),
                        bank_code = Funcoes.ObterStringSemAcentosECaracteresEspeciais(ReaderCadastroContas["NOM_CODIGO_BANCO"].ToString().Trim().PadLeft(3, '0')),
                        routing_number = Funcoes.ObterStringSemAcentosECaracteresEspeciais(ReaderCadastroContas["NOM_NUMERO_AGENCIA_BANCO"].ToString().Trim()),
                        account_number = Funcoes.ObterStringSemAcentosECaracteresEspeciais(ReaderCadastroContas["NOM_NUMERO_CONTA_BANCO"].ToString().Trim()) + Funcoes.ObterStringSemAcentosECaracteresEspeciais(ReaderCadastroContas["NOM_NUMERO_DIGITO_CONTA_BANCO"].ToString().Trim()),
                        taxpayer_id = (ReaderCadastro["FLG_TIPO_PESSOA"].ToString() == "PF") ? Funcoes.ObterStringSemAcentosECaracteresEspeciais(ReaderCadastro["NOM_CPF"].ToString().Trim()) : null,
                        ein = (ReaderCadastro["FLG_TIPO_PESSOA"].ToString() == "PJ") ? Funcoes.ObterStringSemAcentosECaracteresEspeciais(ReaderCadastro["NOM_CNPJ"].ToString().Trim()) : null,
                        type = (ReaderCadastroContas["NOM_TIPO_BANCO"].ToString().Trim() == "C") ? "checking" : "savings"
                    };

                    string json = JsonConvert.SerializeObject(dconta);
                    string jsonConta = zoop.CadastrarContaVendedor(json);

                    if (jsonConta.ToString().Trim() != "")
                    {
                        try
                        {
                            JObject o = JObject.Parse(jsonConta);
                            sIDTokenConta = o["id"].ToString();
                            txtTokenConta.Text = o["id"].ToString();
                            ClientScript.RegisterStartupScript(this.GetType(), "SucessoContaBancaria", "alert('Conta Bancária enviada com sucesso!');", true);

                        }
                        catch
                        {
                            sIDTokenConta = "";
                            ClientScript.RegisterStartupScript(this.GetType(), "ErroContaBancaria", "alert('Ocorreu um erro ao tentar cadastrar a conta bancária!');", true);
                        }
                    }

                    if (sIDTokenConta.ToString().Trim() != "")
                    {
                        dadosAssociarContaBancaria.AssociarContaBancaria dassociarconta = new dadosAssociarContaBancaria.AssociarContaBancaria()
                        {
                            customer = txtToken.Text.ToString().Trim(),
                            token = sIDTokenConta.ToString().Trim()
                        };

                        json = JsonConvert.SerializeObject(dassociarconta);
                        string jsonAssociarConta = zoop.AssociarContaBancariaVendedor(json);

                        if (jsonAssociarConta.ToString().Trim() != "")
                        {
                            try
                            {
                                JObject o = JObject.Parse(jsonAssociarConta);
                                sIDConta = o["id"].ToString();
                                ClientScript.RegisterStartupScript(this.GetType(), "SucessoaSSOCIARContaBancaria", "alert('Conta Bancária foi associada ao estabelecimento com sucesso!');", true);
                            }
                            catch
                            {
                                ClientScript.RegisterStartupScript(this.GetType(), "ErroAssociar", "alert('Ocorreu um erro ao tentar associar a conta bancária ao estabelecimento!');", true);
                            }

                        }

                    }
                }
            }

            if (sIDContaCodigo.ToString().Trim() != "")
            {
                // Atualiza os dados da conta bancária
                SqlConnection connInsCons = new SqlConnection(Funcoes.conexao());
                connInsCons.Open();
                SqlCommand cmdInsCons = new SqlCommand("dbo.stp_pessoas_fj_contas_ins", connInsCons);
                cmdInsCons.CommandType = CommandType.StoredProcedure;
                cmdInsCons.Parameters.Add("@flg_operacao", SqlDbType.Char).Value = 'T';
                cmdInsCons.Parameters.Add("@COD_ID_PESSOA_LICENCIADO", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["LICENCIADO"].ToString());
                cmdInsCons.Parameters.Add("@COD_ID", SqlDbType.Int).Value = Funcoes.strToInt(sIDContaCodigo.ToString().Trim());
                cmdInsCons.Parameters.Add("@NOM_TOKEN", SqlDbType.VarChar).Value = sIDTokenConta.ToString().Trim();
                cmdInsCons.Parameters.Add("@NUM_ID_CONTA", SqlDbType.VarChar).Value = sIDConta.ToString().Trim();
                cmdInsCons.ExecuteNonQuery();
                connInsCons.Close();
                connInsCons.Dispose();
            }
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
        if (txtCPFCNPJImportar.Text.ToString().Trim() != "")
        {
            ConsultaFichaImportar();
        }
        else
        {
            ClientScript.RegisterStartupScript(this.GetType(),
        "CPFCNPJImportar", "alert('Para realizar a importação é necessário informar o CPF ou CNPJ! Verifique e reentre.');", true);
        }
    }

    private void ConsultaFichaImportar()
    {
        string jsonRetorno = "";
        if (Funcoes.ObterStringSemAcentosECaracteresEspeciais(txtCPFCNPJImportar.Text.ToString().Trim()).Length == 14)
        {
            ddlTipoFJ.SelectedValue = "PJ";
        }
        if (Funcoes.ObterStringSemAcentosECaracteresEspeciais(txtCPFCNPJImportar.Text.ToString().Trim()).Length == 11)
        {
            ddlTipoFJ.SelectedValue = "PF";
        }
        jsonRetorno = zoop.ConsultaCadastroSeller(Funcoes.ObterStringSemAcentosECaracteresEspeciais(txtCPFCNPJImportar.Text.ToString().Trim()));

        if (jsonRetorno.ToString().Trim() != "")
        {
            try
            {

                JObject o = JObject.Parse(jsonRetorno);
                if (ddlTipoFJ.SelectedValue.ToString() == "PF")
                {
                    txtToken.Text = o["id"].ToString();
                    txtStatus.Text = o["status"].ToString();
                    txtNome.Text = o["first_name"].ToString();
                    txtSobrenome.Text = o["last_name"].ToString();
                    txtRazaoSocial.Text = o["first_name"].ToString() + " " + o["last_name"].ToString();
                    txtFantasia.Text = o["first_name"].ToString() + " " + o["last_name"].ToString();

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
                    try
                    {
                        txtNascimento.Text = Convert.ToDateTime(o["birthdate"].ToString()).ToShortDateString();
                        txtDataAbertura.Text = Convert.ToDateTime(o["birthdate"].ToString()).ToShortDateString();
                    }
                    catch
                    {
                    }
                    txtEmailEmpresa.Text = o["email"].ToString();
                    txtEmailResponsavel.Text = o["email"].ToString();
                }
                if (ddlTipoFJ.SelectedValue.ToString() == "PJ")
                {
                    txtToken.Text = o["id"].ToString();
                    txtStatus.Text = o["status"].ToString();
                    txtNome.Text = o["owner"]["first_name"].ToString();
                    txtSobrenome.Text = o["owner"]["last_name"].ToString();
                    txtRazaoSocial.Text = o["business_name"].ToString();
                    txtFantasia.Text = o["business_description"].ToString();
                    txtCNPJ.Text = o["ein"].ToString();

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

                    txtNascimento.Text = Convert.ToDateTime(o["owner"]["birthdate"].ToString()).ToShortDateString();
                    txtDataAbertura.Text = Convert.ToDateTime(o["business_opening_date"].ToString()).ToShortDateString();

                    txtEmailEmpresa.Text = o["business_email"].ToString();
                    txtEmailResponsavel.Text = o["owner"]["email"].ToString();
                }
                ClientScript.RegisterStartupScript(this.GetType(), "SucessoImportarEstabelecimento", "alert('Dados Importados com sucesso!');", true);


                try
                {
                    string jsonConta = zoop.ConsultaContasSeller(txtToken.Text.ToString());
                    JObject oConta = JObject.Parse(jsonConta);
                    if (Funcoes.strToInt(oConta["total"].ToString()) > 0)
                    {
                        txtDigitoAgencia.Text = "";
                        txtDigitoConta.Text = "";
                        txtConta.Text = oConta["items"][0]["account_number"].ToString();
                        txtAgencia.Text = oConta["items"][0]["routing_number"].ToString();
                        try
                        {
                            ddlInstituicaoFinanceira.SelectedValue = Funcoes.strToInt(oConta["items"][0]["bank_code"].ToString()).ToString();
                        }
                        catch
                        {
                        }
                        txtTokenConta.Text = oConta["items"][0]["id"].ToString();
                        if (oConta["items"][0]["type"].ToString() == "Checking")
                        { ddlTipoConta.SelectedValue = "C"; }
                        else { ddlTipoConta.SelectedValue = "P"; }
                    }
                    ClientScript.RegisterStartupScript(this.GetType(), "SucessoConta", "alert('Dados da conta bancária importada com sucesso!');", true);

                }
                catch
                {
                    ClientScript.RegisterStartupScript(this.GetType(),
                "ExceptionConta", "alert('Ocorreu um erro ao tentar importar os dados bancários! Verifique e tente novamente.');", true);
                }

            }
            catch
            {
                ClientScript.RegisterStartupScript(this.GetType(), "ExceptionImportarEstabelecimento", "alert('Desculpe, o CPF/CNPJ que você está tentando importar não existe ou foi excluído.');", true);
            }
        }
        else
        {
            ClientScript.RegisterStartupScript(this.GetType(),
        "ErroGeral", "alert('Ocorreu um erro ao tentar importar os dados do parceiro beneficiário! Verifique e tente novamente.');", true);

        }

    }
}