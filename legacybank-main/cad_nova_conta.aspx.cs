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

public partial class cad_nova_conta : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        string urlorigem = Request.ServerVariables["SERVER_NAME"].ToString();

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

            Page.Title = ReaderCadastro["NOM_RAZAOSOCIAL"].ToString();
            //Page.Header.Title = ReaderCadastro["NOM_RAZAOSOCIAL"].ToString();
            FavIconLink.Text = "<link rel='icon' type='image/x-icon' href='" + "public_html/" + ReaderCadastro["NOM_ICONE"].ToString() + "'>";
            if (ReaderCadastro["NOM_IMAGEM_FUNDO"].ToString().Trim() != "")
            {
                bdyLogin.Attributes.Add("style", "background: url(public_html/" + ReaderCadastro["NOM_IMAGEM_FUNDO"].ToString() + ") no-repeat center center fixed;  -webkit-background-size: cover;  -moz-background-size: cover;  background-size: cover;  -o-background-size: cover;");
            }
            else
            {
                if ((ReaderCadastro["NOM_COR_PRIMARIA_FUNDO"].ToString().Trim() != "") && (ReaderCadastro["NOM_COR_SECUNDARIA_FUNDO"].ToString().Trim() != ""))
                {
                    bdyLogin.Attributes.Add("style", "background: linear-gradient(" + ReaderCadastro["NOM_COR_PRIMARIA_FUNDO"].ToString() + ", " + ReaderCadastro["NOM_COR_SECUNDARIA_FUNDO"].ToString() + ");");
                }
            }
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

            // Dados dos Textos de Aceite
            HttpContext.Current.Session.Add("TERMOSCONDICOS", ReaderCadastro["NOM_TERMOS_CONDICOES_USO"].ToString());
            HttpContext.Current.Session.Add("POLITICAPRIVACIDADE", ReaderCadastro["NOM_POLITICA_PRIVACIDADE"].ToString());


        }

        if (!IsPostBack)
        {
            // CarregaTabela PAIS


            SqlConnection myMCC = new SqlConnection(Funcoes.conexao());
            myMCC.Open();
            SqlCommand cmdMCC = new SqlCommand("dbo.stp_mcc_ins", myMCC);
            cmdMCC.CommandType = CommandType.StoredProcedure;
            cmdMCC.Parameters.Add("@flg_operacao", SqlDbType.VarChar).Value = "L";
            SqlDataAdapter drMCC = new SqlDataAdapter();
            drMCC.SelectCommand = cmdMCC;
            DataSet dsMCC = new DataSet();
            drMCC.Fill(dsMCC, "MCC");
            ddlProfissao.DataTextField = "NOM_MCC";
            ddlProfissao.DataValueField = "COD_ID";
            ddlProfissao.DataSource = dsMCC.Tables["MCC"].DefaultView;
            ddlProfissao.DataBind();
            ddlProfissao.Items.Insert(0, new ListItem("", "0"));
            myMCC.Close();


            SqlConnection myPais = new SqlConnection(Funcoes.conexao());
            myPais.Open();
            SqlCommand cmdPais = new SqlCommand("dbo.stp_paises_ins", myPais);
            cmdPais.CommandType = CommandType.StoredProcedure;
            cmdPais.Parameters.Add("@flg_operacao", SqlDbType.VarChar).Value = "C";
            SqlDataAdapter drPais = new SqlDataAdapter();
            drPais.SelectCommand = cmdPais;
            DataSet dsPais = new DataSet();
            drPais.Fill(dsPais, "PAIS");
            ddlPais.DataTextField = "NOM_PAIS";
            ddlPais.DataValueField = "NOM_PAIS";
            ddlPais.DataSource = dsPais.Tables["PAIS"].DefaultView;
            ddlPais.DataBind();
            ddlPais.Items.Insert(0, new ListItem("", ""));
            myPais.Close();

            ddlPais.SelectedValue = "BRASIL";

            HttpContext.Current.Session.Add("TIPOCONTA", "PF");
            HttpContext.Current.Session.Add("ETAPA", "1");
        }

    }
    protected void btnVamos_Click(object sender, EventArgs e)
    {
        crdIniciar.Visible = false;
        crdTipoConta.Visible = true;
    }
    protected void btnPessoal_Click(object sender, EventArgs e)
    {
        HttpContext.Current.Session["TIPOCONTA"] = "PF";
        crdTipoConta.Visible = false;
        crdCPF.Visible = true;
        lblCPF.Text = "CPF";
    }
    protected void btnNegocio_Click(object sender, EventArgs e)
    {
        HttpContext.Current.Session["TIPOCONTA"] = "PJ";
        crdTipoConta.Visible = false;
        crdCPF.Visible = true;
        lblCPF.Text = "CPF do representante legal";
    }
    protected void btnContinuar01_Click(object sender, EventArgs e)
    {
        crdCPF.Visible = false;
        crdPessoais.Visible = true;

        if (HttpContext.Current.Session["TIPOCONTA"].ToString() == "PF") { lblNome.Text = "Nome completo"; checkboxSuccess4.Visible = false; lblcheckboxSuccess4.Visible = false; }
        if (HttpContext.Current.Session["TIPOCONTA"].ToString() == "PJ") { lblNome.Text = "Nome completo do representante legal"; checkboxSuccess4.Visible = true; lblcheckboxSuccess4.Visible = true; }
    }
    protected void btnVoltar01_Click(object sender, EventArgs e)
    {
        crdIniciar.Visible = true;
        crdTipoConta.Visible = false;

    }
    protected void btnLogin_Click(object sender, EventArgs e)
    {
        Response.Redirect("login.aspx");
    }
    protected void btnVoltar02_Click(object sender, EventArgs e)
    {
        crdTipoConta.Visible = true;
        crdCPF.Visible = false;

    }
    protected void btnContinuar02_Click(object sender, EventArgs e)
    {
        crdPessoais.Visible = false;
        crdConfirmar.Visible = true;
    }
    protected void btnVoltar03_Click(object sender, EventArgs e)
    {
        crdCPF.Visible = true;
        crdPessoais.Visible = false;

    }
    protected void btnContinuar03_Click(object sender, EventArgs e)
    {
        crdConfirmar.Visible = false;
        crdAceite.Visible = true;
    }
    protected void btnVoltar04_Click(object sender, EventArgs e)
    {
        crdPessoais.Visible = true;
        crdConfirmar.Visible = false;

    }
    protected void btnAceito_Click(object sender, EventArgs e)
    {
        LimparStatus();

        crdStatus.Visible = true;
        crdAceite.Visible = false;
        HttpContext.Current.Session["ETAPA"] = "2";
        checkboxSuccess1.Checked = true;
        lblStatus.Text = "Vamos continuar " + txtNome.Text.ToString() + "?";
    }
    protected void btnNaoAceito_Click(object sender, EventArgs e)
    {
        Response.Redirect("login.aspx");
    }
    protected void btnSim_Click(object sender, EventArgs e)
    {
        if (HttpContext.Current.Session["ETAPA"].ToString() == "2")
        {
            crdEnderecoCEP.Visible = true;
            crdStatus.Visible = false;
        }

        if (HttpContext.Current.Session["ETAPA"].ToString() == "3")
        {
            crdPais.Visible = true;
            crdStatus.Visible = false;
        }
    }
    protected void btnVoltarNao_Click(object sender, EventArgs e)
    {
        if (HttpContext.Current.Session["ETAPA"].ToString() == "2")
        {
            crdAceite.Visible = true;
            crdStatus.Visible = false;
        }
        
        if (HttpContext.Current.Session["ETAPA"].ToString() == "3")
        {
            crdEnderecoComplemento.Visible = true;
            crdStatus.Visible = false;
        }

    }
    protected void btnContinuarCEP_Click(object sender, EventArgs e)
    {
        // Verifica os dados do CEP Informado
        VerificaCEP();
        crdEnderecoDados.Visible = true;
        crdEnderecoCEP.Visible = false;
    }
    protected void btnVoltarCEP_Click(object sender, EventArgs e)
    {
        LimparStatus();
        crdEnderecoCEP.Visible = false;
        crdStatus.Visible = true;
        HttpContext.Current.Session["ETAPA"] = "2";
        checkboxSuccess1.Checked = true;
        lblStatus.Text = "Vamos continuar " + txtNome.Text.ToString() + "?";
    }

    private void VerificaCEP()
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
                txtEndereco.Text = m.logradouro;
                txtBairro.Text = m.bairro;
                txtCidade.Text = m.localidade;
                ddlEstado.SelectedValue = m.uf;

                responseStream.Close();
                myWebResponse.Close();
            }
            catch
            {
                ClientScript.RegisterStartupScript(this.GetType(),
    "Alerta", "alert('Não foi possível localizar o seu endereço pelo CEP informado! Verifique e reentre'); ", true);
            }
        }

    }
    protected void btnVoltarEnderecoDados_Click(object sender, EventArgs e)
    {
        crdEnderecoDados.Visible = false;
        crdEnderecoCEP.Visible = true;
    }
    protected void btnContinuarEnderecoDados_Click(object sender, EventArgs e)
    {
        crdEnderecoDados.Visible = false;
        crdEnderecoComplemento.Visible = true;
    }
    protected void btnVoltarEnderecoComplamento_Click(object sender, EventArgs e)
    {
        crdEnderecoDados.Visible = true;
        crdEnderecoComplemento.Visible = false;

    }
    protected void btnContinuarEnderecoComplemento_Click(object sender, EventArgs e)
    {
        LimparStatus();

        crdEnderecoComplemento.Visible = false; 
        crdStatus.Visible = true;
        HttpContext.Current.Session["ETAPA"] = "3";
        checkboxSuccess1.Checked = true;
        checkboxSuccess2.Checked = true;
        lblStatus.Text = "Vamos continuar " + txtNome.Text.ToString() + "?";
    }

    private void LimparStatus()
    {
        checkboxSuccess1.Checked = false;
        checkboxSuccess2.Checked = false;
        checkboxSuccess3.Checked = false;
        checkboxSuccess4.Checked = false;
    }
    protected void btnVoltarPais_Click(object sender, EventArgs e)
    {
        LimparStatus();
        crdPais.Visible = false;
        crdStatus.Visible = true;
        HttpContext.Current.Session["ETAPA"] = "3";
        checkboxSuccess1.Checked = true;
        checkboxSuccess2.Checked = true;
        lblStatus.Text = "Vamos continuar " + txtNome.Text.ToString() + "?";
    }
    protected void btnContinuarPais_Click(object sender, EventArgs e)
    {
        crdPais.Visible = false;
        crdProfissao.Visible = true;
    }
    protected void btnVoltarProfissao_Click(object sender, EventArgs e)
    {
        crdPais.Visible = true;
        crdProfissao.Visible = false;
    }
    protected void btnContinuarProfissao_Click(object sender, EventArgs e)
    {

    }
}