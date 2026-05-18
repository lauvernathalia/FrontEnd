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


public partial class onboardingauth : System.Web.UI.Page
{
    public string urlorigem { get; set; }

    protected void Page_Load(object sender, EventArgs e)
    {
        urlorigem = Request.ServerVariables["SERVER_NAME"].ToString();

        imgLogoPrincipal.ImageUrl = "images/logo-colorida.png";
        SqlConnection mySelCadastro = new SqlConnection(Funcoes.conexao());
        mySelCadastro.Open();
        SqlCommand cmdSelCadastro = new SqlCommand("dbo.stp_pessoas_fj_ins", mySelCadastro);
        cmdSelCadastro.CommandType = CommandType.StoredProcedure;
        cmdSelCadastro.Parameters.Add("@flg_operacao", SqlDbType.Char).Value = "T";
        cmdSelCadastro.Parameters.Add("@NOM_URL", SqlDbType.VarChar).Value = urlorigem.ToString();
        SqlDataReader ReaderCadastro = cmdSelCadastro.ExecuteReader();
        while (ReaderCadastro.Read())
        {
            HttpContext.Current.Session.Add("LICENCIADO", ReaderCadastro["COD_ID_PESSOA_LICENCIADO"].ToString());

            imgLogoPrincipal.ImageUrl = "public_html/" + ReaderCadastro["NOM_LOGO"].ToString();

            Page.Title = ReaderCadastro["NOM_TITULO_URL"].ToString();
            //Page.Header.Title = ReaderCadastro["NOM_RAZAOSOCIAL"].ToString();
            FavIconLink.Text = "<link rel='icon' type='image/x-icon' href='" + "public_html/" + ReaderCadastro["NOM_FAVICON"].ToString() + "'>";
            if (ReaderCadastro["NOM_IMAGEM_FUNDO"].ToString().Trim() != "")
            {
                bdyLogin.Attributes.Add("style", "background: url('public_html/" + ReaderCadastro["NOM_IMAGEM_FUNDO"].ToString() + "') no-repeat center center fixed;  -webkit-background-size: cover;  -moz-background-size: cover;  background-size: cover;  -o-background-size: cover;");
            }
            else
            {
                if ((ReaderCadastro["NOM_COR_PRIMARIA_FUNDO"].ToString().Trim() != "") && (ReaderCadastro["NOM_COR_SECUNDARIA_FUNDO"].ToString().Trim() != ""))
                {
                    bdyLogin.Attributes.Add("style", "background: linear-gradient(" + ReaderCadastro["NOM_COR_PRIMARIA_FUNDO"].ToString() + ", " + ReaderCadastro["NOM_COR_SECUNDARIA_FUNDO"].ToString() + ");");
                }
            }
            // 2FA
            HttpContext.Current.Session.Add("AUTENTICACAO2FA", ReaderCadastro["FLG_2FA"].ToString());

            // Pegar URL Origem

            HttpContext.Current.Session.Add("URLORIGEM", urlorigem.ToString());

            // Pegar dados do Logotipo

            HttpContext.Current.Session.Add("LOGOPADRAO", ReaderCadastro["NOM_LOGO"].ToString());

            // Pegar os dados ZOOP

            HttpContext.Current.Session.Add("ZOOPINTEGRACAO", ReaderCadastro["FLG_INTEGRACAO_ZOOP"].ToString());
            HttpContext.Current.Session.Add("ZOOPTERCEIROS", ReaderCadastro["FLG_TERCEIROS_ZOOP"].ToString());

            if (ReaderCadastro["NOM_ID_MARKETPLACE"].ToString().Trim() != "")
            { HttpContext.Current.Session.Add("IDMARKETPLACE", ReaderCadastro["NOM_ID_MARKETPLACE"].ToString()); }
            else { HttpContext.Current.Session.Add("IDMARKETPLACE", ConfigurationManager.AppSettings["idzoop"].ToString()); }

            if (ReaderCadastro["NOM_KEY_MARKETPLACE"].ToString().Trim() != "")
            { HttpContext.Current.Session.Add("USERNAMEMARKETPLACE", ReaderCadastro["NOM_KEY_MARKETPLACE"].ToString()); }
            else { HttpContext.Current.Session.Add("USERNAMEMARKETPLACE", ConfigurationManager.AppSettings["keyzoop"].ToString()); }

            // Pegar os dados E-MAIL

            // Dados E-mail Padrao
            HttpContext.Current.Session.Add("EMAILHOST", ReaderCadastro["NOM_HOST_EMAIL_PADRAO"].ToString());
            HttpContext.Current.Session.Add("EMAIL", ReaderCadastro["NOM_EMAIL_PADRAO"].ToString());
            HttpContext.Current.Session.Add("EMAILSENHA", ReaderCadastro["NOM_SENHA_EMAIL_PADRAO"].ToString());
            HttpContext.Current.Session.Add("EMAILPORTA", ReaderCadastro["NUM_PORTA_EMAIL_PADRAO"].ToString());
            HttpContext.Current.Session.Add("EMAILSSL", ReaderCadastro["FLG_SSL_EMAIL_PADRAO"].ToString());
            HttpContext.Current.Session.Add("EMAILRODAPE", ReaderCadastro["NOM_RODAPE_EMAIL"].ToString());
            HttpContext.Current.Session.Add("EMAILLOGOTIPO", ReaderCadastro["NOM_LOGOTIPO_EMAIL"].ToString());

            // Dados dos Textos de Aceite
            HttpContext.Current.Session.Add("TERMOSCONDICOS", ReaderCadastro["NOM_TERMOS_CONDICOES_USO"].ToString());
            HttpContext.Current.Session.Add("POLITICAPRIVACIDADE", ReaderCadastro["NOM_POLITICA_PRIVACIDADE"].ToString());

            // CHAT DE SUPORTE
            HttpContext.Current.Session.Add("CHAT", ReaderCadastro["FLG_CHAT"].ToString());

            //lblTermo.Text = HttpContext.Current.Session["TERMOSCONDICOS"].ToString();
            //lblPolitica.Text = HttpContext.Current.Session["POLITICAPRIVACIDADE"].ToString();

        }
        if (!IsPostBack)
        {
            dvDocumento.Visible = true;
            dvDocumentof.Visible = true;

            dvDados.Visible = false;
            dvDadosf.Visible = false;

            txtGUID.Text = Guid.NewGuid().ToString();
        }

    }

    public string CorPrimaria()
    {
        string urlorigem = Request.ServerVariables["SERVER_NAME"].ToString();

        SqlConnection mySelCadastro = new SqlConnection(Funcoes.conexao());
        mySelCadastro.Open();
        SqlCommand cmdSelCadastro = new SqlCommand("dbo.stp_pessoas_fj_ins", mySelCadastro);
        cmdSelCadastro.CommandType = CommandType.StoredProcedure;
        cmdSelCadastro.Parameters.Add("@flg_operacao", SqlDbType.Char).Value = "T";
        cmdSelCadastro.Parameters.Add("@NOM_URL", SqlDbType.VarChar).Value = urlorigem.ToString();
        string CorPrimaria = "";
        //cmdSelCadastro.Parameters.Add("@COD_ID_PESSOA_LICENCIADO", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["LICENCIADO"].ToString());
        SqlDataReader ReaderCadastro = cmdSelCadastro.ExecuteReader();
        while (ReaderCadastro.Read())
        {
            CorPrimaria = ReaderCadastro["NOM_COR_PRIMARIA_FUNDO"].ToString();
        }
        return CorPrimaria;
    }

    protected void rbFisica_CheckedChanged(object sender, EventArgs e)
    {
        lblCPF.Text = "Digite o seu CPF";
        lblDados.Text = "2. Informe os seus dados";
        txtCPF.Text = "";
    }
    protected void rbJuridica_CheckedChanged(object sender, EventArgs e)
    {
        lblCPF.Text = "Digite o CPF do representante legal";
        lblDados.Text = "2. Informe os dados do representante legal";
        txtCPF.Text = "";
    }
    protected void txtCPF_TextChanged(object sender, EventArgs e)
    {
        if (Funcoes.TIRAACENTOSDOCUMENTOS(txtCPF.Text.ToString().Trim()).Length == 11) 
        {
            if (Funcoes.ValidaCPF(Funcoes.TIRAACENTOSDOCUMENTOS(txtCPF.Text.ToString())) == true)
            {
                btnDocumento.Enabled = true;
            }
            else
            {
                ClientScript.RegisterStartupScript(this.GetType(), "CPFInvalido", "alert('CPF Inválido! É necessário informar um CPF válido.'); ", true);
                txtCPF.Text = "";
                btnDocumento.Enabled = false;
            }
        } 
        else 
        {
            ClientScript.RegisterStartupScript(this.GetType(), "CPFInvalido", "alert('CPF Inválido! É necessário informar um CPF válido.'); ", true);
            txtCPF.Text = "";
            btnDocumento.Enabled = false; 
        }
    }

    protected void btnDocumento_Click(object sender, EventArgs e)
    {
        dvDocumento.Visible = false;
        dvDocumentof.Visible = false;
        dvDados.Visible = true;
        dvDadosf.Visible = true;
    }
    protected void btnCancelar_Click(object sender, EventArgs e)
    {
        Response.Redirect("login.aspx");
    }
    protected void btnDados_Click(object sender, EventArgs e)
    {
        if ((txtNome.Text.ToString().Trim() != "") && (txtEmail.Text.ToString().Trim() != "") && (txtNascimento.Text.ToString().Trim() != "") && (txtNascimento.Text.ToString().Trim() != "__/__/____") && (txtCelular.Text.ToString().Trim() != "") && (txtCelular.Text.ToString().Trim() != "(__) _____-____"))
        {

            dvDados.Visible = false;
            dvDadosf.Visible = false;

            dvCodigo.Visible = true;
            dvCodigof.Visible = true;


            string s2fagerado = Funcoes.Codigo2FA(6).ToString();
            // Grava no arquivo 2fa
            SqlConnection connInsCons2FA = new SqlConnection(Funcoes.conexao());
            connInsCons2FA.Open();
            SqlCommand cmdInsCons2FA = new SqlCommand("dbo.stp_2faconta_ins", connInsCons2FA);
            cmdInsCons2FA.CommandType = CommandType.StoredProcedure;
            cmdInsCons2FA.Parameters.Add("@flg_operacao", SqlDbType.Char).Value = "I";
            cmdInsCons2FA.Parameters.Add("@NOM_GUID", SqlDbType.VarChar).Value = txtGUID.Text.ToString();
            cmdInsCons2FA.Parameters.Add("@NOM_EMAIL", SqlDbType.VarChar).Value = txtEmail.Text.ToString();
            cmdInsCons2FA.Parameters.Add("@DTA_DATA", SqlDbType.DateTime).Value = DateTime.Now;
            cmdInsCons2FA.Parameters.Add("@DTA_EXPIRACAO", SqlDbType.DateTime).Value = DateTime.Now.AddMinutes(30);
            cmdInsCons2FA.Parameters.Add("@NOM_2FA", SqlDbType.VarChar).Value = Funcoes.Encrypt(s2fagerado);
            cmdInsCons2FA.ExecuteNonQuery();
            connInsCons2FA.Close();
            connInsCons2FA.Dispose();

            Enviar2fa(s2fagerado.ToString());
        }
        else
        {
            ClientScript.RegisterStartupScript(this.GetType(), "CamposObrigatorios", "alert('Todos os campos são de preenchimento obrigatório! Verifique e reentre.');", true);
            txtNome.Focus();
        }

    }
    protected void btnDadosV_Click(object sender, EventArgs e)
    {
        dvDocumento.Visible = true;
        dvDocumentof.Visible = true;

        dvDados.Visible = false;
        dvDadosf.Visible = false;

    }
    protected void btnCodigo_Click(object sender, EventArgs e)
    {
        int iCodigo = 0;
        // Verifica se o código autenticação esta correto
        SqlConnection connVerifica2fa = new SqlConnection(Funcoes.conexao());
        SqlCommand cmdVerifica2fa = new SqlCommand("dbo.stp_2faconta_ins", connVerifica2fa);
        cmdVerifica2fa.CommandType = CommandType.StoredProcedure;
        cmdVerifica2fa.Parameters.Add("@flg_operacao", SqlDbType.Char).Value = "L";
        cmdVerifica2fa.Parameters.Add("@NOM_GUID", SqlDbType.VarChar).Value = txtGUID.Text.ToString();
        cmdVerifica2fa.Parameters.Add("@NOM_EMAIL", SqlDbType.VarChar).Value = txtEmail.Text.ToString();
        cmdVerifica2fa.Parameters.Add("@NOM_2FA", SqlDbType.VarChar).Value = Funcoes.Encrypt(txtCodigo.Text.ToString());
        cmdVerifica2fa.Parameters.Add("@DTA_EXPIRACAO", SqlDbType.DateTime).Value = DateTime.Now;
        connVerifica2fa.Open();
        SqlDataReader mReader2fa = cmdVerifica2fa.ExecuteReader();
        if (mReader2fa.Read())
        {
            iCodigo = Funcoes.strToInt(mReader2fa["COD_ID"].ToString());
        }
        if (iCodigo>0)
        {
            dvCodigo.Visible = false;
            dvCodigof.Visible = false;

            dvEndereco.Visible = true;
            dvEnderecof.Visible= true;
            
        }
        else
        {
            ClientScript.RegisterStartupScript(this.GetType(),"CodigoErrado2fa", "alert('O Código de confirmação dos dados não está correto! Verifique e reentre.');", true);
        }
    }

    private void Enviar2fa(string s2FA)
    {
        try
        {
            System.Net.Mail.SmtpClient client = new System.Net.Mail.SmtpClient();
            client.Host = HttpContext.Current.Session["EMAILHOST"].ToString();
            if (HttpContext.Current.Session["EMAILSSL"].ToString() == "S")
            { client.EnableSsl = true; }
            else { client.EnableSsl = false; }
            client.Port = Funcoes.strToInt(HttpContext.Current.Session["EMAILPORTA"].ToString());
            //client.Timeout = 0;
            client.Credentials = new System.Net.NetworkCredential(HttpContext.Current.Session["EMAIL"].ToString(), HttpContext.Current.Session["EMAILSENHA"].ToString());
            MailMessage mail = new MailMessage();
            mail.Sender = new System.Net.Mail.MailAddress(HttpContext.Current.Session["EMAIL"].ToString(), "");
            mail.From = new MailAddress(HttpContext.Current.Session["EMAIL"].ToString(), "");

            mail.To.Add(new MailAddress(txtEmail.Text.ToString(), txtEmail.Text.ToString()));
            mail.Bcc.Add(new MailAddress("adriano@webview.com.br", "Adriano - WEBVIEW"));
            mail.Bcc.Add(new MailAddress("admin@legacybank.com.br", "Administrador LEGACY"));

            mail.Subject = "Código Confirmação da Autenticação de Dois Fatores - " + HttpContext.Current.Session["URLORIGEM"].ToString();
            mail.SubjectEncoding = System.Text.Encoding.UTF8;

            // Construção do CORPO da MENSAGEM (Body)
            mail.Body = "";
            mail.Body = mail.Body + "<img src='https://" + HttpContext.Current.Session["URLORIGEM"].ToString() + "/public_html/" + HttpContext.Current.Session["EMAILLOGOTIPO"].ToString() + "' alt='Logotipo' width='150'><br /><br>";
            mail.Body = mail.Body + "<b>Olá,</b> " + txtNome.Text.ToString() + "<br /><br />";
            mail.Body = mail.Body + "Segue abaixo o código de confirmação dos seus dados.<br />";
            mail.Body = mail.Body + "<b>Digite este código na tela de cadastro da conta</b> para proseguir com a abertura da sua conta.<br /><br>";

            mail.Body = mail.Body + "<a>" + s2FA.ToString() + "</a><br /><br>";

            mail.Body = mail.Body + HttpContext.Current.Session["EMAILRODAPE"].ToString();

            mail.BodyEncoding = System.Text.Encoding.UTF8;
            mail.IsBodyHtml = true;
            mail.Priority = MailPriority.High;

            try
            {
                client.Send(mail);
            }
            catch (System.Exception erro)
            {
                ClientScript.RegisterStartupScript(this.GetType(),
        "Erro2fa", "alert('Ocorreu um erro ao enviar!');", true);
            }
            finally
            {
                mail = null;
                ClientScript.RegisterStartupScript(this.GetType(),
        "Codigo2fa", "alert('Um código de confirmação dos dados foi enviado para o seu e-mail!');", true);

            }
        }
        catch
        {
            ClientScript.RegisterStartupScript(this.GetType(),
    "Codigo2faErro", "alert('Ocorreu um erro ao tentar enviar o Código de confirmação dos dados!');", true);

        }
    }

    protected void btnCodigoV_Click(object sender, EventArgs e)
    {
        dvCodigo.Visible = false;
        dvCodigof.Visible = false;

        dvDados.Visible = true;
        dvDadosf.Visible = true;
    }
    protected void txtCEP_TextChanged(object sender, EventArgs e)
    {

        if (txtCEP.Text.ToString().Trim() != "")
        {
            try
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

                //mostrar DIV

                txtEndereco.Text = m.logradouro;
                txtBairro.Text = m.bairro;
                txtCidade.Text = m.localidade;
                ddlEstado.SelectedValue = m.uf;

                responseStream.Close();
                myWebResponse.Close();
            }
            catch
            {
                ClientScript.RegisterStartupScript(this.GetType(),"CEPIncorreto", "alert('Não foi possível identificar o endereço pelo CEP informado! Verifique e reentre.');", true);

            }
        }
    }

    protected void btnEndereco_Click(object sender, EventArgs e)
    {
        if ((txtCEP.Text.ToString().Trim() != "") && (txtEndereco.Text.ToString().Trim() != "") && (txtBairro.Text.ToString().Trim() != "") && (txtCidade.Text.ToString().Trim() != "") && (txtNumero.Text.ToString().Trim() != "") && (txtComplemento.Text.ToString().Trim() != "") && (ddlEstado.SelectedValue.ToString().Trim() != ""))
        {
            if (rbFisica.Checked == true)
            {
                dvEndereco.Visible = false;
                dvEnderecof.Visible = false;

                dvConfirmacao.Visible = true;
                dvConfirmacaof.Visible = true;

            }
            if (rbJuridica.Checked == true)
            {
                dvEndereco.Visible = false;
                dvEnderecof.Visible = false;

                dvJuridica.Visible = true;
                dvJuridicaf.Visible = true;
            }

        }
        else
        {
            ClientScript.RegisterStartupScript(this.GetType(), "CamposObrigatorios", "alert('Todos os campos são de preenchimento obrigatório! Verifique e reentre.');", true);
            txtCEP.Focus();
        }

    }
    protected void btnEnderecoV_Click(object sender, EventArgs e)
    {
        dvEndereco.Visible = false;
        dvEnderecof.Visible = false;

        dvDados.Visible = true;
        dvDadosf.Visible = true;
    }
    protected void lkbReenviar_Click(object sender, EventArgs e)
    {
        string s2fagerado = Funcoes.Codigo2FA(6).ToString();
        // Grava no arquivo 2fa
        SqlConnection connInsCons2FA = new SqlConnection(Funcoes.conexao());
        connInsCons2FA.Open();
        SqlCommand cmdInsCons2FA = new SqlCommand("dbo.stp_2faconta_ins", connInsCons2FA);
        cmdInsCons2FA.CommandType = CommandType.StoredProcedure;
        cmdInsCons2FA.Parameters.Add("@flg_operacao", SqlDbType.Char).Value = "I";
        cmdInsCons2FA.Parameters.Add("@NOM_GUID", SqlDbType.VarChar).Value = txtGUID.Text.ToString();
        cmdInsCons2FA.Parameters.Add("@NOM_EMAIL", SqlDbType.VarChar).Value = txtEmail.Text.ToString();
        cmdInsCons2FA.Parameters.Add("@DTA_DATA", SqlDbType.DateTime).Value = DateTime.Now;
        cmdInsCons2FA.Parameters.Add("@DTA_EXPIRACAO", SqlDbType.DateTime).Value = DateTime.Now.AddMinutes(30);
        cmdInsCons2FA.Parameters.Add("@NOM_2FA", SqlDbType.VarChar).Value = Funcoes.Encrypt(s2fagerado);
        cmdInsCons2FA.ExecuteNonQuery();
        connInsCons2FA.Close();
        connInsCons2FA.Dispose();

        Enviar2fa(s2fagerado.ToString());

    }
    protected void btnJuridica_Click(object sender, EventArgs e)
    {
        if ((txtCNPJ.Text.ToString().Trim() != "") && (txtRazaoSocial.Text.ToString().Trim() != "") && (txtNomeFantasia.Text.ToString().Trim() != "") && (txtAbertura.Text.ToString().Trim() != "") && (ddlTipoEmpresa.SelectedValue.ToString().Trim() != "") && (ddlFaturamento.SelectedValue.ToString().Trim() != ""))
        {
            if (Funcoes.ValidaCNPJ(Funcoes.TIRAACENTOSDOCUMENTOS(txtCNPJ.Text.ToString())) == true)
            {
                dvJuridica.Visible = false;
                dvJuridicaf.Visible = false;

                dvConfirmacao.Visible = true;
                dvConfirmacaof.Visible = true;
            }
            else
            {
                ClientScript.RegisterStartupScript(this.GetType(), "CPFInvalido", "alert('CNPJ Inválido! É necessário informar um CNPJ válido.'); ", true);
                txtCNPJ.Text = "";
                txtCNPJ.Focus();
            }
        }
        else
        {
            ClientScript.RegisterStartupScript(this.GetType(), "CamposObrigatoriosJuridica", "alert('Todos os campos são de preenchimento obrigatório! Verifique e reentre.');", true);
            txtCNPJ.Focus();
        }


    }
    protected void btnJuridicaV_Click(object sender, EventArgs e)
    {
        dvJuridica.Visible = false;
        dvJuridicaf.Visible = false;

        dvEndereco.Visible = true;
        dvEnderecof.Visible = true;
    }

    protected void btnConfirmacao_Click(object sender, EventArgs e)
    {
        // Verifica a existência de uma conta para o tipo de pessoa

        SqlConnection mySelCadastro = new SqlConnection(Funcoes.conexao());
        mySelCadastro.Open();
        SqlCommand cmdSelCadastro = new SqlCommand("dbo.stp_pessoas_fj_ins", mySelCadastro);
        cmdSelCadastro.CommandType = CommandType.StoredProcedure;
        cmdSelCadastro.Parameters.Add("@flg_operacao", SqlDbType.Char).Value = "H";



        string sTipoDocumento = "";
        if (rbJuridica.Checked==true)
        {
            cmdSelCadastro.Parameters.Add("@NOM_CNPJ", SqlDbType.VarChar).Value = Funcoes.TIRAACENTOSDOCUMENTOS(txtCNPJ.Text.ToString());
            sTipoDocumento = "CNPJ";
        }
        else
        {
            cmdSelCadastro.Parameters.Add("@NOM_CNPJ", SqlDbType.VarChar).Value = Funcoes.TIRAACENTOSDOCUMENTOS(txtCPF.Text.ToString());
            sTipoDocumento = "CPF";

        }
        cmdSelCadastro.Parameters.Add("@COD_ID_PESSOA_LICENCIADO", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["LICENCIADO"].ToString());
        cmdSelCadastro.Parameters.Add("@FLG_TIPO", SqlDbType.Char).Value = "E";

        SqlDataReader ReaderCadastro = cmdSelCadastro.ExecuteReader();
        while (ReaderCadastro.Read())
        {
            ClientScript.RegisterStartupScript(this.GetType(),
                "Duplicidade", "alert('Já existe um cadastrado com este documento ("+ sTipoDocumento.ToString() + ")! Verifique e reentre.');", true);
            dvConfirmacao.Visible = false;
            dvConfirmacaof.Visible = false;

            dvDocumento.Visible = true;
            dvDocumentof.Visible = true;
            return;
        }

        // Gravação dos dados
        try
        {
            SqlConnection connInsCons = new SqlConnection(Funcoes.conexao());
            connInsCons.Open();
            SqlCommand cmdInsCons = new SqlCommand("dbo.stp_pessoas_fj_ins", connInsCons);
            cmdInsCons.CommandType = CommandType.StoredProcedure;
            cmdInsCons.Parameters.Add("@flg_operacao", SqlDbType.Char).Value = 'I';
            cmdInsCons.Parameters.Add("@FLG_ATIVO", SqlDbType.Char).Value = "V";
            cmdInsCons.Parameters.Add("@FLG_INSERT_USUARIO", SqlDbType.Char).Value = "S";

            cmdInsCons.Parameters.Add("@NUM_IP_INCLUSAO", SqlDbType.VarChar).Value = Request.ServerVariables["REMOTE_ADDR"].ToString();
            cmdInsCons.Parameters.Add("@COD_ID_PESSOA_LICENCIADO", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["LICENCIADO"].ToString());
            cmdInsCons.Parameters.Add("@DTA_DATA", SqlDbType.DateTime).Value = DateTime.Now;
            cmdInsCons.Parameters.Add("@FLG_TIPO", SqlDbType.Char).Value = "E";
            cmdInsCons.Parameters.Add("@FLG_TIPO_PESSOA", SqlDbType.VarChar).Value = (rbFisica.Checked==true) ? "PF" : "PJ";
            cmdInsCons.Parameters.Add("@FLG_PRESENCIAL", SqlDbType.Char).Value = "N";

            string sNome = Funcoes.NomeSobrenome(txtNome.Text.ToString(), "P").ToString().Trim();
            string sSobrenome = Funcoes.NomeSobrenome(txtNome.Text.ToString(), "U").ToString().Trim();
            
            if (rbJuridica.Checked==true)
            {
                // Empresa
                cmdInsCons.Parameters.Add("@NOM_RAZAOSOCIAL", SqlDbType.VarChar).Value = txtRazaoSocial.Text.ToString();
                cmdInsCons.Parameters.Add("@NOM_FANTASIA", SqlDbType.VarChar).Value = txtNomeFantasia.Text.ToString();
                cmdInsCons.Parameters.Add("@NOM_CNPJ", SqlDbType.VarChar).Value = txtCNPJ.Text.ToString();
                cmdInsCons.Parameters.Add("@NUM_TELEFONE", SqlDbType.VarChar).Value = txtCelular.Text.ToString();
                cmdInsCons.Parameters.Add("@NOM_EMAIL_EMPRESA", SqlDbType.VarChar).Value = txtEmail.Text.ToString();
                cmdInsCons.Parameters.Add("@NUM_FATURAMENTO", SqlDbType.Float).Value = Funcoes.strToDouble(ddlFaturamento.SelectedValue.ToString());

                if (txtAbertura.Text.ToString().Trim() != "")
                {
                    cmdInsCons.Parameters.Add("@DTA_ABERTURA", SqlDbType.DateTime).Value = Convert.ToDateTime(txtAbertura.Text.ToString());
                }

                // Responsável
                cmdInsCons.Parameters.Add("@NOM_NOME", SqlDbType.VarChar).Value = sNome.ToString();
                cmdInsCons.Parameters.Add("@NOM_SOBRENOME", SqlDbType.VarChar).Value = sSobrenome.ToString();

                cmdInsCons.Parameters.Add("@NOM_CPF", SqlDbType.VarChar).Value = txtCPF.Text.ToString();
                if (txtNascimento.Text.ToString().Trim() != "")
                {
                    cmdInsCons.Parameters.Add("@DTA_ANIVERSARIO", SqlDbType.DateTime).Value = Convert.ToDateTime(txtNascimento.Text.ToString());
                }

                cmdInsCons.Parameters.Add("@NOM_MAE", SqlDbType.VarChar).Value = txtNomeMae.Text.ToString();
                cmdInsCons.Parameters.Add("@NUM_RENDA_MENSAL", SqlDbType.Float).Value = Funcoes.strToDouble(ddlRendaMensal.SelectedValue.ToString());
                cmdInsCons.Parameters.Add("@NOM_EMAIL", SqlDbType.VarChar).Value = txtEmail.Text.ToString();
                cmdInsCons.Parameters.Add("@NOM_CELULAR", SqlDbType.VarChar).Value = txtCelular.Text.ToString();
                cmdInsCons.Parameters.Add("@FLG_POLITICAMENTE", SqlDbType.Char).Value = ddlPoliticamenteExpostaPF.SelectedValue.ToString();
            }
            else
            {
                // Empresa
                cmdInsCons.Parameters.Add("@NOM_RAZAOSOCIAL", SqlDbType.VarChar).Value = txtNome.Text.ToString();
                cmdInsCons.Parameters.Add("@NOM_FANTASIA", SqlDbType.VarChar).Value = txtNome.Text.ToString();
                cmdInsCons.Parameters.Add("@NOM_CNPJ", SqlDbType.VarChar).Value = txtCPF.Text.ToString();
                cmdInsCons.Parameters.Add("@NUM_TELEFONE", SqlDbType.VarChar).Value = txtCelular.Text.ToString();
                cmdInsCons.Parameters.Add("@NOM_EMAIL_EMPRESA", SqlDbType.VarChar).Value = txtEmail.Text.ToString();
                cmdInsCons.Parameters.Add("@NUM_FATURAMENTO", SqlDbType.Float).Value = Funcoes.strToDouble(ddlRendaMensal.SelectedValue.ToString());
                if (txtNascimento.Text.ToString().Trim() != "")
                {
                    cmdInsCons.Parameters.Add("@DTA_ABERTURA", SqlDbType.DateTime).Value = Convert.ToDateTime(txtNascimento.Text.ToString());
                }

                // Responsável
                cmdInsCons.Parameters.Add("@NOM_NOME", SqlDbType.VarChar).Value = sNome.ToString();
                cmdInsCons.Parameters.Add("@NOM_SOBRENOME", SqlDbType.VarChar).Value = sSobrenome.ToString();
                cmdInsCons.Parameters.Add("@NOM_CPF", SqlDbType.VarChar).Value = txtCPF.Text.ToString();

                if (txtNascimento.Text.ToString().Trim() != "")
                {
                    cmdInsCons.Parameters.Add("@DTA_ANIVERSARIO", SqlDbType.DateTime).Value = Convert.ToDateTime(txtNascimento.Text.ToString());
                }
                cmdInsCons.Parameters.Add("@NUM_RENDA_MENSAL", SqlDbType.Float).Value = Funcoes.strToDouble(ddlRendaMensal.SelectedValue.ToString());
                cmdInsCons.Parameters.Add("@NOM_EMAIL", SqlDbType.VarChar).Value = txtEmail.Text.ToString();
                cmdInsCons.Parameters.Add("@NOM_CELULAR", SqlDbType.VarChar).Value = txtCelular.Text.ToString();
                cmdInsCons.Parameters.Add("@FLG_POLITICAMENTE", SqlDbType.Char).Value = ddlPoliticamenteExpostaPF.SelectedValue.ToString();


            }
            // Dados Gerais
            //cmdInsCons.Parameters.Add("@COD_ID_MCC", SqlDbType.Int).Value = Funcoes.strToInt(ddlAtividadeEconomica.SelectedValue.ToString());
            cmdInsCons.Parameters.Add("@NOM_TIPO_EMPRESA", SqlDbType.VarChar).Value = ddlTipoEmpresa.SelectedValue.ToString();

            // Endereço
            cmdInsCons.Parameters.Add("@NOM_ENDERECO", SqlDbType.VarChar).Value = txtEndereco.Text.ToString();
            cmdInsCons.Parameters.Add("@NOM_NUMERO", SqlDbType.VarChar).Value = txtNumero.Text.ToString();
            cmdInsCons.Parameters.Add("@NOM_COMPLEMENTO", SqlDbType.VarChar).Value = txtComplemento.Text.ToString();
            cmdInsCons.Parameters.Add("@NOM_BAIRRO", SqlDbType.VarChar).Value = txtBairro.Text.ToString();
            cmdInsCons.Parameters.Add("@NOM_CIDADE", SqlDbType.VarChar).Value = txtCidade.Text.ToString();
            cmdInsCons.Parameters.Add("@NOM_UF", SqlDbType.VarChar).Value = ddlEstado.SelectedValue.ToString();
            cmdInsCons.Parameters.Add("@NOM_CEP", SqlDbType.VarChar).Value = txtCEP.Text.ToString();
            cmdInsCons.Parameters.Add("@NOM_PAIS", SqlDbType.VarChar).Value = "BR";


            // Usuário
            cmdInsCons.Parameters.Add("@NOM_NOME_USUARIO", SqlDbType.VarChar).Value = txtNome.Text.ToString();
            cmdInsCons.Parameters.Add("@NOM_LOGIN", SqlDbType.VarChar).Value = txtEmail.Text.ToString();

            // Gera a senha
            string caracteresPermitidos = "abcdefghijkmnopqrstuvwxyzABCDEFGHJKLMNOPQRSTUVWXYZ0123456789!@$?_-";
            char[] chars = new char[8];
            Random rd = new Random();
            for (int i = 0; i < 8; i++)
            {
                chars[i] = caracteresPermitidos[rd.Next(0, caracteresPermitidos.Length)];
            }
            string sNovaSenha = new string(chars);

            cmdInsCons.Parameters.Add("@NOM_SENHA", SqlDbType.VarChar).Value = sNovaSenha.ToString();
            txtSenha.Text = sNovaSenha.ToString();
            
            txtID.Text = cmdInsCons.ExecuteScalar().ToString();
            
            //cmdInsCons.ExecuteNonQuery();
            
            connInsCons.Close();
            connInsCons.Dispose();

            // Enviar e-mail com dados de acesso

            try
            {
                System.Net.Mail.SmtpClient client = new System.Net.Mail.SmtpClient();
                client.Host = HttpContext.Current.Session["EMAILHOST"].ToString();
                if (HttpContext.Current.Session["EMAILSSL"].ToString() == "S")
                { client.EnableSsl = true; }
                else { client.EnableSsl = false; }
                client.Port = Funcoes.strToInt(HttpContext.Current.Session["EMAILPORTA"].ToString());
                //client.Timeout = 0;
                client.Credentials = new System.Net.NetworkCredential(HttpContext.Current.Session["EMAIL"].ToString(), HttpContext.Current.Session["EMAILSENHA"].ToString());
                MailMessage mail = new MailMessage();
                mail.Sender = new System.Net.Mail.MailAddress(HttpContext.Current.Session["EMAIL"].ToString(), "");
                mail.From = new MailAddress(HttpContext.Current.Session["EMAIL"].ToString(), "");

                mail.To.Add(new MailAddress(txtEmail.Text.ToString(), txtEmail.Text.ToString()));
                mail.Bcc.Add(new MailAddress("adriano@webview.com.br", "Adriano - WEBVIEW"));
                mail.Bcc.Add(new MailAddress("admin@legacybank.com.br", "Administrador LEGACY"));

                mail.Subject = "Dados de acesso - " + HttpContext.Current.Session["URLORIGEM"].ToString();
                mail.SubjectEncoding = System.Text.Encoding.UTF8;

                // Construção do CORPO da MENSAGEM (Body)
                mail.Body = "";
                mail.Body = mail.Body + "<img src='https://" + HttpContext.Current.Session["URLORIGEM"].ToString() + "/public_html/" + HttpContext.Current.Session["EMAILLOGOTIPO"].ToString() + "' alt='Logotipo' width='150'><br /><br>";
                mail.Body = mail.Body + "Olá!<br />";
                mail.Body = mail.Body + "Seja bem vindo(a) " + txtNome.Text.ToString() + "<br />";
                mail.Body = mail.Body + "<b>A seguir segue a sua senha de acesso a " + HttpContext.Current.Session["URLORIGEM"].ToString() + ":</b><br /><br />";

                mail.Body = mail.Body + "Sistema Web: " + Request.ServerVariables["SERVER_NAME"].ToString() + "<br />";

                mail.Body = mail.Body + "E-Mail: " + txtEmail.Text.ToString() + "<br />";
                mail.Body = mail.Body + "Senha de acesso: " + txtSenha.Text.ToString() + "<br />";

                mail.Body = mail.Body + "<b>NOTA:</b> Este é um e-mail automático e não requer nenhuma resposta ou interação.";
                mail.Body = mail.Body + HttpContext.Current.Session["EMAILRODAPE"].ToString(); 

                mail.BodyEncoding = System.Text.Encoding.UTF8;
                mail.IsBodyHtml = true;
                mail.Priority = MailPriority.High;
                try
                {
                    client.Send(mail);
                    ClientScript.RegisterStartupScript(this.GetType(),
                        "EnvioEmailSucesso", "alert('Atenção: Foi enviada uma SENHA DE ACESSO para o E-MAIL informado.');", true);
                }
                catch (System.Exception erro)
                {
                    //trata erro
                    ClientScript.RegisterStartupScript(this.GetType(),
                        "EnvioEmailErro", "alert('Atenção: Não foi possível enviar a SENHA DE ACESSO para o E-MAIL informado');", true);
                }
                finally
                {
                    mail = null;
                }
            }
            catch
            {
                ClientScript.RegisterStartupScript(this.GetType(), "ErroEnvioEmailDadosAcesso", "alert('Ocorreu um erro inesperado ao tentar enviar o e-mail com seus dados de acesso! Tente mais tarde novamente');", true);
            }


            // Criar Conta AsaaS
            try
            {
                /*
                dadosSubconta.Subconta dsubconta = new dadosSubconta.Subconta()
                {
                    name = (rbFisica.Checked==true) ? Funcoes.TIRAACENTOS(txtNome.Text.ToString().Trim()) : Funcoes.TIRAACENTOS(txtRazaoSocial.Text.ToString().Trim()),
                    email = txtEmail.Text.ToString().Trim(),
                    loginEmail = txtEmail.Text.ToString().Trim(),
                    birthDate = (rbFisica.Checked==true) ? String.Format("{0:yyyy-MM-dd}", Convert.ToDateTime(txtNascimento.Text.ToString().Trim())) : "",
                    cpfCnpj = (rbFisica.Checked==true) ? Funcoes.TIRAACENTOSDOCUMENTOS(txtCPF.Text.ToString().Trim()) : Funcoes.TIRAACENTOSDOCUMENTOS(txtCNPJ.Text.ToString().Trim()),
                    companyType = (rbFisica.Checked==true) ? "" : ddlTipoEmpresa.SelectedValue.ToString(),
                    mobilePhone = Funcoes.TIRAACENTOSDOCUMENTOS(txtCelular.Text.ToString().Trim()),
                    incomeValue = (rbFisica.Checked==true) ? Funcoes.strToInt(ddlRendaMensal.SelectedValue.ToString().Trim()) : Funcoes.strToInt(ddlFaturamento.SelectedValue.ToString().Trim()),
                    address = Funcoes.TIRAACENTOS(txtEndereco.Text.ToString().Trim()),
                    addressNumber = txtNumero.Text.ToString().Trim(),
                    complement = Funcoes.TIRAACENTOS(txtComplemento.Text.ToString().Trim()),
                    province = Funcoes.TIRAACENTOS(txtBairro.Text.ToString().Trim()),
                    postalCode = Funcoes.TIRAACENTOSDOCUMENTOS(txtCEP.Text.ToString().Trim()),
                    webhooks = new dadosSubconta.webhooks[]
                    {
                        new dadosSubconta.webhooks()
                        {
                        name = "WEBHOOK-ID-"+txtID.Text.ToString().Trim().PadLeft(6,'0'),
                        url = "https://conta.legacybank.com.br/events/asaas",
                        email = (HttpContext.Current.Session["EMAIL"].ToString().Trim()!="")? HttpContext.Current.Session["EMAIL"].ToString().Trim() : txtEmail.Text.ToString().Trim(),
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
                
                string jsonRetorno = asaas.CriarSubconta(jsonSubconta);
                // tratar os dados de retorno
                JObject o = JObject.Parse(jsonRetorno.ToString());
                */
                try
                {
                    /*
                    SqlConnection connInsConsBaas = new SqlConnection(Funcoes.conexao());
                    connInsConsBaas.Open();
                    SqlCommand cmdInsConsBaas = new SqlCommand("dbo.stp_pessoas_fj_baas_ins", connInsConsBaas);
                    cmdInsConsBaas.CommandType = CommandType.StoredProcedure;
                    cmdInsConsBaas.Parameters.Add("@flg_operacao", SqlDbType.Char).Value = "I";
                    cmdInsConsBaas.Parameters.Add("@COD_ID_PESSOA_LICENCIADO", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["LICENCIADO"].ToString());
                    cmdInsConsBaas.Parameters.Add("@COD_ID_PESSOAS_FJ", SqlDbType.Int).Value = Funcoes.strToInt(txtID.Text.ToString());
                    cmdInsConsBaas.Parameters.Add("@FLG_ORIGEM", SqlDbType.Char).Value = "A";


                    string sStatus = "";
                    try
                    {
                        cmdInsConsBaas.Parameters.Add("@NUM_TOKEN_BAAS", SqlDbType.VarChar).Value = o["apiKey"].ToString();
                        string ConsultaStatus = asaas.ListarStatus(o["apiKey"].ToString());
                        JObject oStatus = JObject.Parse(ConsultaStatus.ToString());
                        sStatus = oStatus["general"].ToString();
                    }
                    catch
                    {
                        sStatus = "";
                    }


                    cmdInsConsBaas.Parameters.Add("@FLG_STATUS_BAAS", SqlDbType.VarChar).Value = sStatus.ToString();
                    cmdInsConsBaas.Parameters.Add("@FLG_BAAS", SqlDbType.Char).Value = "S";
                    cmdInsConsBaas.Parameters.Add("@DES_JSON_BAAS", SqlDbType.VarChar).Value = jsonRetorno.ToString();

                     * cmdInsConsBaas.Parameters.Add("@NUM_WALLETID_BAAS", SqlDbType.VarChar).Value = o["walletId"].ToString();
                    cmdInsConsBaas.Parameters.Add("@NUM_ID_CONTA_BAAS", SqlDbType.VarChar).Value = o["id"].ToString();
                    cmdInsConsBaas.Parameters.Add("@NUM_AGENCIA_BAAS", SqlDbType.VarChar).Value = o["accountNumber"]["agency"].ToString();
                    cmdInsConsBaas.Parameters.Add("@NUM_CONTA_BAAS", SqlDbType.VarChar).Value = o["accountNumber"]["account"].ToString();
                    cmdInsConsBaas.Parameters.Add("@NUM_DIGITO_CONTA_BAAS", SqlDbType.VarChar).Value = o["accountNumber"]["accountDigit"].ToString();

                    cmdInsConsBaas.Parameters.Add("@NUM_CHAVE_PIX", SqlDbType.VarChar).Value = "";


                    cmdInsCons.ExecuteNonQuery();
                    connInsCons.Close();
                    connInsCons.Dispose();
                    */

                    // Gravar na base de dados BAAS

                    ClientScript.RegisterStartupScript(this.GetType(),
                        "SucessoCriarConta", "alert('A conta foi criada com sucesso!');", true);

                    Response.Redirect("login.aspx");
                }
                catch
                {
                    //txtToken.Text = o["errors"][0]["description"].ToString();
                    ClientScript.RegisterStartupScript(this.GetType(),
                        "ErroCriarConta", "alert('Ocorreu um erro ao tentar criar a conta! Tente mais tarde novamente.", true);

                }
            }
            catch
            {
                ClientScript.RegisterStartupScript(this.GetType(),
                    "ErroCriarContaGeral", "alert('Ocorreu um erro ao tentar criar a conta! Tente mais tarde novamente.", true);

            }

        }
        catch
        {
            ClientScript.RegisterStartupScript(this.GetType(), "ErroSalvarDados", "alert('Ocorreu um erro e não foi possível criar a conta digital! Tente mais tarde novamente');", true);
        }

    }
    protected void btnConfirmacaoV_Click(object sender, EventArgs e)
    {
        if (rbFisica.Checked == true)
        {
            dvConfirmacao.Visible = false;
            dvConfirmacaof.Visible = false;

            dvEndereco.Visible = true;
            dvEnderecof.Visible = true;
        }
        if (rbJuridica.Checked == true)
        {
            dvConfirmacao.Visible = false;
            dvConfirmacaof.Visible = false;

            dvJuridica.Visible = true;
            dvJuridicaf.Visible = true;

        }

    }
}