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


public partial class cad_estabelecimentos_adm : System.Web.UI.Page
{
    public static int iPagina;
    public static string Password;
    public static string PasswordC;
    protected void Page_Load(object sender, EventArgs e)
    {
        // Verifica se o usuário esta autenticado

        if (!HttpContext.Current.User.Identity.IsAuthenticated)
        {
            FormsAuthentication.SignOut();
        }
        if (Session == null && Session["CODIGO"] == null)
        {
            FormsAuthentication.SignOut();
            Response.Redirect("login.aspx");
        }


        if (!IsPostBack)
        {
            div1.Visible = true;
            div2.Visible = false;
            div3.Visible = false;
            div4.Visible = false;
            div5.Visible = false;
            div6.Visible = false;
            iPagina = 1;
            li1.Attributes.Add("class", "active");

            txtSenhaConfirmar.Attributes.Add("onfocus", "this.type='text';");
            txtSenhaConfirmar.Attributes.Add("onblur", "this.type='password';");
            txtSenha.Attributes.Add("onfocus", "this.type='text';");
            txtSenha.Attributes.Add("onblur", "this.type='password';");

            dvPF.Visible = true;
            dvPJ.Visible = false;

            dvFormaRecebimento.Visible = false;


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

            SqlConnection myCNAE = new SqlConnection(Funcoes.conexao());
            myCNAE.Open();
            SqlCommand cmdCNAE = new SqlCommand("dbo.stp_cnae_ins", myCNAE);
            cmdCNAE.CommandType = CommandType.StoredProcedure;
            cmdCNAE.Parameters.Add("@flg_operacao", SqlDbType.VarChar).Value = "L";
            SqlDataAdapter drCNAE = new SqlDataAdapter();
            drCNAE.SelectCommand = cmdCNAE;
            DataSet dsCNAE = new DataSet();
            drCNAE.Fill(dsCNAE, "CNAE");
            ddlRamoAtividade.DataTextField = "NOM_CNAE";
            ddlRamoAtividade.DataValueField = "COD_ID";
            ddlRamoAtividade.DataSource = dsCNAE.Tables["CNAE"].DefaultView;
            ddlRamoAtividade.DataBind();
            ddlRamoAtividade.Items.Insert(0, new ListItem("", "0"));

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

        }

        if (txtSenha.Text.ToString().Trim().Length > 0)
        {
            txtSenha.Attributes["value"] = txtSenha.Text;
            txtSenhaConfirmar.Attributes["value"] = txtSenhaConfirmar.Text;
            Password = txtSenha.Text.ToString();
        }

    }

    private void ConsultaGeral()
    {

    }

    protected void btnSalvar_Click(object sender, EventArgs e)
    {
        // Salva Primeiro cadastro da Pessoa F/J

        SqlConnection connInsCons = new SqlConnection(Funcoes.conexao());
        connInsCons.Open();
        SqlCommand cmdInsCons = new SqlCommand("dbo.stp_pessoas_fj_ins", connInsCons);
        cmdInsCons.CommandType = CommandType.StoredProcedure;
        cmdInsCons.Parameters.Add("@flg_operacao", SqlDbType.Char).Value = 'I';
        cmdInsCons.Parameters.Add("@DTA_DATA", SqlDbType.DateTime).Value = DateTime.Now;
        cmdInsCons.Parameters.Add("@FLG_TIPO", SqlDbType.Char).Value = "E";
        cmdInsCons.Parameters.Add("@COD_ID_PESSOA_LICENCIADO", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["LICENCIADO"].ToString());
        cmdInsCons.Parameters.Add("@NOM_LOGIN", SqlDbType.VarChar).Value = txtEmail.Text.ToString();
        cmdInsCons.Parameters.Add("@NOM_EMAIL", SqlDbType.VarChar).Value = txtEmail.Text.ToString();
        cmdInsCons.Parameters.Add("@NOM_SENHA", SqlDbType.VarChar).Value = Password.ToString();
        cmdInsCons.Parameters.Add("@NOM_NOME", SqlDbType.VarChar).Value = txtNome.Text.ToString();
        cmdInsCons.Parameters.Add("@NOM_SOBRENOME", SqlDbType.VarChar).Value = txtSobrenome.Text.ToString();

        cmdInsCons.Parameters.Add("@DTA_ANIVERSARIO", SqlDbType.DateTime).Value = Convert.ToDateTime(txtNascimento.Text.ToString());


        cmdInsCons.Parameters.Add("@NOM_ENDERECO", SqlDbType.VarChar).Value = txtEndereco.Text.ToString();
        cmdInsCons.Parameters.Add("@NOM_NUMERO", SqlDbType.VarChar).Value = txtNumero.Text.ToString();
        cmdInsCons.Parameters.Add("@NOM_COMPLEMENTO", SqlDbType.VarChar).Value = txtComplemento.Text.ToString();
        cmdInsCons.Parameters.Add("@NOM_BAIRRO", SqlDbType.VarChar).Value = txtBairro.Text.ToString();
        cmdInsCons.Parameters.Add("@NOM_CIDADE", SqlDbType.VarChar).Value = txtCidade.Text.ToString();
        cmdInsCons.Parameters.Add("@NOM_UF", SqlDbType.VarChar).Value = ddlEstado.SelectedValue.ToString();
        cmdInsCons.Parameters.Add("@NOM_CEP", SqlDbType.VarChar).Value = txtCEP.Text.ToString();
        cmdInsCons.Parameters.Add("@NOM_PAIS", SqlDbType.VarChar).Value = "BR";

        cmdInsCons.Parameters.Add("@FLG_TIPO_PESSOA", SqlDbType.VarChar).Value = ddlTipoFJ.SelectedValue.ToString();
        cmdInsCons.Parameters.Add("@FLG_ATIVO", SqlDbType.Char).Value = "S";

        if (ddlTipoFJ.SelectedValue.ToString() == "PF")
        {
            cmdInsCons.Parameters.Add("@NOM_RAZAOSOCIAL", SqlDbType.VarChar).Value = txtNome.Text.ToString() + " " + txtSobrenome.Text.ToString();
            cmdInsCons.Parameters.Add("@NOM_FANTASIA", SqlDbType.VarChar).Value = txtNome.Text.ToString() + " " + txtSobrenome.Text.ToString();
            cmdInsCons.Parameters.Add("@NOM_CELULAR", SqlDbType.VarChar).Value = txtCelular.Text.ToString();
            cmdInsCons.Parameters.Add("@NUM_TELEFONE", SqlDbType.VarChar).Value = txtCelular.Text.ToString();
            cmdInsCons.Parameters.Add("@NUM_RENDA_MENSAL", SqlDbType.Float).Value = Funcoes.strToDouble(txtRendaMensalPF.Text.ToString());
            cmdInsCons.Parameters.Add("@NUM_FATURAMENTO", SqlDbType.Float).Value = Funcoes.strToDouble(txtRendaMensalPF.Text.ToString());
            cmdInsCons.Parameters.Add("@NOM_EMAIL_EMPRESA", SqlDbType.VarChar).Value = txtEmail.Text.ToString();
            cmdInsCons.Parameters.Add("@DTA_ABERTURA", SqlDbType.DateTime).Value = DateTime.Now;

        }
        else
        {
            cmdInsCons.Parameters.Add("@NOM_RAZAOSOCIAL", SqlDbType.VarChar).Value = txtRazaoSocial.Text.ToString();
            cmdInsCons.Parameters.Add("@NOM_FANTASIA", SqlDbType.VarChar).Value = txtFantasia.Text.ToString();
            cmdInsCons.Parameters.Add("@NOM_CELULAR", SqlDbType.VarChar).Value = txtCelular.Text.ToString();
            cmdInsCons.Parameters.Add("@NUM_TELEFONE", SqlDbType.VarChar).Value = txtTelefoneEmpresa.Text.ToString();
            cmdInsCons.Parameters.Add("@NUM_RENDA_MENSAL", SqlDbType.Float).Value = Funcoes.strToDouble(txtRendaMensalPF.Text.ToString());
            cmdInsCons.Parameters.Add("@NUM_FATURAMENTO", SqlDbType.Float).Value = Funcoes.strToDouble(txtFaturamentoMensal.Text.ToString());
            cmdInsCons.Parameters.Add("@NOM_EMAIL_EMPRESA", SqlDbType.VarChar).Value = txtEmailEmpresa.Text.ToString();
            cmdInsCons.Parameters.Add("@DTA_ABERTURA", SqlDbType.DateTime).Value = Convert.ToDateTime(txtDataAbertura.Text.ToString());

        }
        cmdInsCons.Parameters.Add("@COD_ID_MCC", SqlDbType.Int).Value = Funcoes.strToInt(ddlAtividadeEconomica.SelectedValue.ToString());
        cmdInsCons.Parameters.Add("@COD_ID_CNAE", SqlDbType.Int).Value = Funcoes.strToInt(ddlRamoAtividade.SelectedValue.ToString());
        cmdInsCons.Parameters.Add("@NOM_TIPO_EMPRESA", SqlDbType.VarChar).Value = ddlTipoEmpresa.SelectedValue.ToString();

        cmdInsCons.Parameters.Add("@NOM_MAE", SqlDbType.VarChar).Value = txtNomeMae.Text.ToString();
        cmdInsCons.Parameters.Add("@NOM_CPF", SqlDbType.VarChar).Value = txtCPF.Text.ToString();
        cmdInsCons.Parameters.Add("@NOM_CNPJ", SqlDbType.VarChar).Value = txtCNPJ.Text.ToString();
        cmdInsCons.Parameters.Add("@NUM_PATRIMONIO", SqlDbType.Float).Value = Funcoes.strToDouble(txtPatrimonio.Text.ToString());

        cmdInsCons.Parameters.Add("@FLG_PRESENCIAL", SqlDbType.Char).Value = ddlPresencial.SelectedValue.ToString();
        cmdInsCons.Parameters.Add("@FLG_POLITICAMENTE", SqlDbType.Char).Value = ddlPoliticamenteExposta.SelectedValue.ToString();

        cmdInsCons.Parameters.Add("@FLG_MODALIDADE", SqlDbType.Char).Value = ddlModalidade.SelectedValue.ToString();
        cmdInsCons.Parameters.Add("@FLG_RECEBIMENTO", SqlDbType.Char).Value = ddlFormaRecebimento.SelectedValue.ToString();
        cmdInsCons.Parameters.Add("@FLG_INSERT_USUARIO", SqlDbType.Char).Value = "S";

        cmdInsCons.Parameters.Add("@NOM_CODIGO_BANCO", SqlDbType.VarChar).Value = ddlInstituicaoFinanceira.SelectedValue.ToString();
        cmdInsCons.Parameters.Add("@NOM_TIPO_BANCO", SqlDbType.VarChar).Value = ddlTipoConta.SelectedValue.ToString();
        cmdInsCons.Parameters.Add("@NOM_NUMERO_AGENCIA_BANCO", SqlDbType.VarChar).Value = txtAgencia.Text.ToString();
        cmdInsCons.Parameters.Add("@NOM_NUMERO_DIGITO_AGENCIA_BANCO", SqlDbType.VarChar).Value = txtDigitoAgencia.Text.ToString();
        cmdInsCons.Parameters.Add("@NOM_NUMERO_CONTA_BANCO", SqlDbType.VarChar).Value = txtConta.Text.ToString();
        cmdInsCons.Parameters.Add("@NOM_NUMERO_DIGITO_CONTA_BANCO", SqlDbType.VarChar).Value = txtDigitoConta.Text.ToString();

        cmdInsCons.ExecuteNonQuery();
        connInsCons.Close();
        connInsCons.Dispose();

        ClientScript.RegisterStartupScript(this.GetType(),
            "Alerta", "alert('Dados gravados com sucesso'); ", true);

        Response.Redirect("con_estabelecimentos_adm.aspx");


    }



    protected void btnAnterior_Click(object sender, EventArgs e)
    {
        if (iPagina > 1)
        {
            iPagina = iPagina - 1;
        }

        if (txtSenha.Text.ToString().Trim().Length > 0)
        {
            Password = txtSenha.Text.ToString();
        }



        if (iPagina == 1)
        {
            li2.Attributes["class"]="step";
            div1.Visible = true;
            div2.Visible = false;
            div3.Visible = false;
            div4.Visible = false;
            div5.Visible = false;
            div6.Visible = false;

        }
        if (iPagina == 2)
        {
            li3.Attributes["class"] = "step";
            div1.Visible = false;
            div2.Visible = true;
            div3.Visible = false;
            div4.Visible = false;
            div5.Visible = false;
            div6.Visible = false;

        }
        if (iPagina == 3)
        {
            li4.Attributes["class"] = "step";
            div1.Visible = false;
            div2.Visible = false;
            div3.Visible = true;
            div4.Visible = false;
            div5.Visible = false;
            div6.Visible = false;

        }
        if (iPagina == 4)
        {
            li5.Attributes["class"] = "step";
            div1.Visible = false;
            div2.Visible = false;
            div3.Visible = false;
            div4.Visible = true;
            div5.Visible = false;
            div6.Visible = false;

        }
        if (iPagina == 5)
        {
            li6.Attributes["class"] = "step";
            div1.Visible = false;
            div2.Visible = false;
            div3.Visible = false;
            div4.Visible = false;
            div5.Visible = true;
            div6.Visible = false;

        }

        if (iPagina == 5)
        {
            div1.Visible = false;
            div2.Visible = false;
            div3.Visible = false;
            div4.Visible = false;
            div5.Visible = false;
            div6.Visible = true;

        }
        


    }
    protected void btnProximo_Click(object sender, EventArgs e)
    {
        if (iPagina < 6)
        {
            iPagina = iPagina + 1;
        }


        if (txtSenha.Text.ToString().Trim().Length > 0)
        {
            txtSenha.Text = txtSenha.Text.ToString();
            txtSenhaConfirmar.Text = txtSenhaConfirmar.Text.ToString();
            Password = txtSenha.Text.ToString();
        }



        if (iPagina == 1)
        {
            li1.Attributes.Add("class", "active");
            div1.Visible = true;
            div2.Visible = false;
            div3.Visible = false;
            div4.Visible = false;
            div5.Visible = false;
            div6.Visible = false;
        }
        
        
        if (iPagina == 2)
        {
            li2.Attributes.Add("class", "active");
            div1.Visible = false;
            div2.Visible = true;
            div3.Visible = false;
            div4.Visible = false;
            div5.Visible = false;
            div6.Visible = false;
        }
        if (iPagina == 3)
        {
            li3.Attributes.Add("class", "active");
            div1.Visible = false;
            div2.Visible = false;
            div3.Visible = true;
            div4.Visible = false;
            div5.Visible = false;
            div6.Visible = false;
        }
        if (iPagina == 4)
        {
            li4.Attributes.Add("class", "active");
            div1.Visible = false;
            div2.Visible = false;
            div3.Visible = false;
            div4.Visible = true;
            div5.Visible = false;
            div6.Visible = false;

        }
        if (iPagina == 5)
        {
            li5.Attributes.Add("class", "active");
            div1.Visible = false;
            div2.Visible = false;
            div3.Visible = false;
            div4.Visible = false;
            div5.Visible = true;
            div6.Visible = false;

        }
        if (iPagina == 6)
        {
            li6.Attributes.Add("class", "active");
            div1.Visible = false;
            div2.Visible = false;
            div3.Visible = false;
            div4.Visible = false;
            div5.Visible = false;
            div6.Visible = true;

        }

    }

    protected void ddlTipoFJ_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlTipoFJ.SelectedValue.ToString() == "PF")
        {
            dvPF.Visible = true;
            dvPJ.Visible = false;
        }
        else
        {
            dvPF.Visible = false;
            dvPJ.Visible = true;
        }
    }
    protected void ddlModalidade_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlModalidade.SelectedValue.ToString() == "A")
        {
            dvFormaRecebimento.Visible = true;
        }
        else
        {
            dvFormaRecebimento.Visible = false;
        }
    }
    protected void txtSenhaConfirmar_TextChanged(object sender, EventArgs e)
    {
        if (txtSenha.Text.ToString() != txtSenhaConfirmar.Text.ToString())
        {
            ClientScript.RegisterStartupScript(this.GetType(),
    "Senhas", "alert('As senhas não são iguais. Verifique e reentre!'); ", true);
            

        }

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
}