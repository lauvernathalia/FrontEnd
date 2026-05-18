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


public partial class con_meusdados : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
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


            FavorecidoService();

            ConsultaFicha();
            ConsultaContas();

            dvSplit.Visible = (HttpContext.Current.Session["TIPO"].ToString() == "E") ? false : true;
        }
    }

    private void ConsultaContas()
    {
        SqlConnection myConsulta = new SqlConnection(Funcoes.conexao());
        myConsulta.Open();
        SqlDataAdapter SDAConsulta = new SqlDataAdapter("dbo.stp_pessoas_fj_contas_ins", myConsulta);
        SDAConsulta.SelectCommand.CommandType = CommandType.StoredProcedure;
        SDAConsulta.SelectCommand.Parameters.Add("@flg_operacao", SqlDbType.Char).Value = "C";
        SDAConsulta.SelectCommand.Parameters.Add("@COD_ID_PESSOA_LICENCIADO", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["LICENCIADO"].ToString());
        SDAConsulta.SelectCommand.Parameters.Add("@COD_ID_PESSOAS_FJ", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["PESSOA"].ToString());

        DataSet dsConsulta = new DataSet();
        SDAConsulta.Fill(dsConsulta, "PESSOAS_FJ_CONTAS");
        rptConsultaContas.DataSource = dsConsulta.Tables["PESSOAS_FJ_CONTAS"].DefaultView;
        rptConsultaContas.DataBind();
        myConsulta.Close(); myConsulta.Dispose();
    }

    private void FavorecidoService()
    {
        using (SqlConnection connection = new SqlConnection(Funcoes.conexao()))
        {
            connection.Open();
            using (SqlCommand cmd = new SqlCommand("dbo.stp_pessoas_fj_ins", connection))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@flg_operacao", SqlDbType.VarChar).Value = "8";
                cmd.Parameters.Add("@COD_ID_PESSOA_LICENCIADO", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["LICENCIADO"].ToString());
                cmd.Parameters.Add("@FLG_TIPO", SqlDbType.Char).Value = "E";
                cmd.Parameters.Add("@FLG_ATIVO", SqlDbType.Char).Value = "S";
                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                DataTable dtFavorecido = new DataTable();
                adapter.Fill(dtFavorecido);

                ddlFavorecido.DataTextField = "NOM_RAZAOSOCIAL";
                ddlFavorecido.DataValueField = "COD_ID";
                ddlFavorecido.DataSource = dtFavorecido;
                ddlFavorecido.DataBind();
                ddlFavorecido.Items.Insert(0, new ListItem("", ""));

            }
        }


    }


    private void ConsultaFicha()
    {
        SqlConnection mySelCadastroEstabelecimento = new SqlConnection(Funcoes.conexao());
        mySelCadastroEstabelecimento.Open();
        SqlCommand cmdSelCadastroEstabelecimento = new SqlCommand("dbo.stp_pessoas_fj_ins", mySelCadastroEstabelecimento);
        cmdSelCadastroEstabelecimento.CommandType = CommandType.StoredProcedure;
        cmdSelCadastroEstabelecimento.Parameters.Add("@flg_operacao", SqlDbType.Char).Value = "S";
        cmdSelCadastroEstabelecimento.Parameters.Add("@COD_ID_PESSOA_LICENCIADO", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["LICENCIADO"].ToString());
        cmdSelCadastroEstabelecimento.Parameters.Add("@COD_ID", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["PESSOA"].ToString());
        SqlDataReader ReaderCadastroEstabelecimento = cmdSelCadastroEstabelecimento.ExecuteReader();
        while (ReaderCadastroEstabelecimento.Read())
        {
            txtNome.Text = ReaderCadastroEstabelecimento["NOM_RAZAOSOCIAL"].ToString();
            txtCNPJCPF.Text = ReaderCadastroEstabelecimento["NOM_CNPJ"].ToString();
            txtResponsavel.Text = ReaderCadastroEstabelecimento["NOM_NOME"].ToString() + " " + ReaderCadastroEstabelecimento["NOM_SOBRENOME"].ToString();
            txtDocumento.Text = ReaderCadastroEstabelecimento["NOM_CPF"].ToString();
            txtTipo.Text = ReaderCadastroEstabelecimento["NOM_FLG_TIPO_PESSOA"].ToString();

            txtEmailEstabelecimento.Text = ReaderCadastroEstabelecimento["NOM_EMAIL_EMPRESA"].ToString();
            txtEmail.Text = ReaderCadastroEstabelecimento["NOM_EMAIL"].ToString();
            txtCelular.Text = ReaderCadastroEstabelecimento["NOM_CELULAR"].ToString();
            txtCorPrimaria.Text = ReaderCadastroEstabelecimento["NOM_COR_PRIMARIA_CHECKOUT"].ToString();
            txtCorSecundaria.Text = ReaderCadastroEstabelecimento["NOM_COR_SECUNDARIA_CHECKOUT"].ToString();
            txtCorFonte.Text = ReaderCadastroEstabelecimento["NOM_COR_TEXTO_CHECKOUT"].ToString();

            txtLogotipo.Text = ReaderCadastroEstabelecimento["NOM_LOGOTIPO_CHECKOUT"].ToString();
            txtImagem.Text = ReaderCadastroEstabelecimento["NOM_IMAGEM_FUNDO_CHECKOUT"].ToString();
            txtFoto.Text = ReaderCadastroEstabelecimento["NOM_FOTO"].ToString();

            txtFaturamento.Text = ReaderCadastroEstabelecimento["NUM_FATURAMENTO"].ToString();
            txtRendaMensal.Text = ReaderCadastroEstabelecimento["NUM_RENDA_MENSAL"].ToString();
            ddlAtividadeEconomica.SelectedValue = ReaderCadastroEstabelecimento["COD_ID_MCC"].ToString();
            txtMCC.Text = ReaderCadastroEstabelecimento["COD_ID_MCC"].ToString();

            txtEndereco.Text = ReaderCadastroEstabelecimento["NOM_ENDERECO"].ToString();
            txtNumero.Text = ReaderCadastroEstabelecimento["NOM_NUMERO"].ToString();
            txtComplemento.Text = ReaderCadastroEstabelecimento["NOM_COMPLEMENTO"].ToString();
            txtBairro.Text = ReaderCadastroEstabelecimento["NOM_BAIRRO"].ToString();
            txtCidade.Text = ReaderCadastroEstabelecimento["NOM_CIDADE"].ToString();
            ddlEstado.SelectedValue = ReaderCadastroEstabelecimento["NOM_UF"].ToString();
            txtCEP.Text = ReaderCadastroEstabelecimento["NOM_CEP"].ToString();

            ddlFavorecido.SelectedValue = ReaderCadastroEstabelecimento["COD_ID_PESSOAS_FJ_FAVORECIDO"].ToString();
            ddlFavorecido_SelectedIndexChanged(null, null);
        }

    }



    protected void btnSalvar_Click(object sender, EventArgs e)
    {
        if (Funcoes.Enviar2faEstabelecimento() == true)
        {
            ClientScript.RegisterStartupScript(this.GetType(), "Codigo2fa", "alert('Um código de confirmação foi enviado para o seu e-mail!');", true);
        }
        else
        {
            ClientScript.RegisterStartupScript(this.GetType(), "Codigo2faErro", "alert('Ocorreu um erro ao tentar enviar o Código de Confirmação!');", true);
        }
        ClientScript.RegisterStartupScript(this.GetType(), "ExecutaModal", "$('#mdConfirmar').modal('show');", true);
    }


    private void SalvarDados()
    {

        // Logotipo Menu

        string StrFileNameflLogotipo = flLogotipo.PostedFile.FileName.Substring(flLogotipo.PostedFile.FileName.LastIndexOf("\\") + 1);
        string StrFileTypeflLogotipo = flLogotipo.PostedFile.ContentType;
        int IntFileSizeflLogotipo = flLogotipo.PostedFile.ContentLength;
        string NomeArquivoflLogotipo = "";
        if (StrFileNameflLogotipo.Trim() != "")
        {
            string CodificacaoflLogotipo = DateTime.Now.Year.ToString() + DateTime.Now.Month.ToString() + DateTime.Now.Day.ToString() + DateTime.Now.Hour.ToString() + DateTime.Now.Minute.ToString() + DateTime.Now.Second.ToString();
            flLogotipo.PostedFile.SaveAs(Server.MapPath("public_html") + "\\" + HttpContext.Current.Session["CODIGO"].ToString().Trim() + "_" + CodificacaoflLogotipo.ToString() + "_" + StrFileNameflLogotipo);
            NomeArquivoflLogotipo = HttpContext.Current.Session["CODIGO"].ToString().Trim() + "_" + CodificacaoflLogotipo.ToString() + "_" + StrFileNameflLogotipo;
        }

        // Imagem Fundo
        string StrFileNameflImagem = flImagem.PostedFile.FileName.Substring(flImagem.PostedFile.FileName.LastIndexOf("\\") + 1);
        string StrFileTypeflImagem = flImagem.PostedFile.ContentType;
        int IntFileSizeflImagem = flImagem.PostedFile.ContentLength;
        string NomeArquivoflImagem = "";
        if (StrFileNameflImagem.Trim() != "")
        {
            string CodificacaoflImagem = DateTime.Now.Year.ToString() + DateTime.Now.Month.ToString() + DateTime.Now.Day.ToString() + DateTime.Now.Hour.ToString() + DateTime.Now.Minute.ToString() + DateTime.Now.Second.ToString();
            flImagem.PostedFile.SaveAs(Server.MapPath("public_html") + "\\" + HttpContext.Current.Session["CODIGO"].ToString().Trim() + "_" + CodificacaoflImagem.ToString() + "_" + StrFileNameflImagem);
            NomeArquivoflImagem = HttpContext.Current.Session["CODIGO"].ToString().Trim() + "_" + CodificacaoflImagem.ToString() + "_" + StrFileNameflImagem;
        }


        // Foto

        string StrFileNameflFoto = flFoto.PostedFile.FileName.Substring(flFoto.PostedFile.FileName.LastIndexOf("\\") + 1);
        string StrFileTypeflFoto = flFoto.PostedFile.ContentType;
        int IntFileSizeflFoto = flFoto.PostedFile.ContentLength;
        string NomeArquivoflFoto = "";
        if (StrFileNameflFoto.Trim() != "")
        {
            string CodificacaoflFoto = DateTime.Now.Year.ToString() + DateTime.Now.Month.ToString() + DateTime.Now.Day.ToString() + DateTime.Now.Hour.ToString() + DateTime.Now.Minute.ToString() + DateTime.Now.Second.ToString();
            flFoto.PostedFile.SaveAs(Server.MapPath("public_html") + "\\" + HttpContext.Current.Session["CODIGO"].ToString().Trim() + "_" + CodificacaoflFoto.ToString() + "_" + StrFileNameflFoto);
            NomeArquivoflFoto = HttpContext.Current.Session["CODIGO"].ToString().Trim() + "_" + CodificacaoflFoto.ToString() + "_" + StrFileNameflFoto;
        }


        SqlConnection connInsCons = new SqlConnection(Funcoes.conexao());
        connInsCons.Open();
        SqlCommand cmdInsCons = new SqlCommand("dbo.stp_pessoas_fj_ins", connInsCons);
        cmdInsCons.CommandType = CommandType.StoredProcedure;
        cmdInsCons.Parameters.Add("@flg_operacao", SqlDbType.Char).Value = 'K';
        cmdInsCons.Parameters.Add("@COD_ID_PESSOA_LICENCIADO", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["LICENCIADO"].ToString());
        cmdInsCons.Parameters.Add("@COD_ID", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["PESSOA"].ToString());
        cmdInsCons.Parameters.Add("@NOM_COR_PRIMARIA_CHECKOUT", SqlDbType.VarChar).Value = txtCorPrimaria.Text.ToString();
        cmdInsCons.Parameters.Add("@NOM_COR_SECUNDARIA_CHECKOUT", SqlDbType.VarChar).Value = txtCorSecundaria.Text.ToString();
        cmdInsCons.Parameters.Add("@NOM_COR_TEXTO_CHECKOUT", SqlDbType.VarChar).Value = txtCorFonte.Text.ToString();

        if (NomeArquivoflLogotipo.ToString().Trim() != "")
        {
            cmdInsCons.Parameters.Add("@NOM_LOGOTIPO_CHECKOUT", SqlDbType.VarChar).Value = NomeArquivoflLogotipo.ToString();
        }
        else
        {
            cmdInsCons.Parameters.Add("@NOM_LOGOTIPO_CHECKOUT", SqlDbType.VarChar).Value = txtLogotipo.Text.ToString();

        }

        if (NomeArquivoflImagem.ToString().Trim() != "")
        {
            cmdInsCons.Parameters.Add("@NOM_IMAGEM_FUNDO_CHECKOUT", SqlDbType.VarChar).Value = NomeArquivoflImagem.ToString();
        }
        else
        {
            cmdInsCons.Parameters.Add("@NOM_IMAGEM_FUNDO_CHECKOUT", SqlDbType.VarChar).Value = txtImagem.Text.ToString();

        }

        if (NomeArquivoflFoto.ToString().Trim() != "")
        {
            cmdInsCons.Parameters.Add("@NOM_FOTO", SqlDbType.VarChar).Value = NomeArquivoflFoto.ToString();
        }
        else
        {
            cmdInsCons.Parameters.Add("@NOM_FOTO", SqlDbType.VarChar).Value = txtFoto.Text.ToString();

        }
        cmdInsCons.Parameters.Add("@NOM_EMAIL", SqlDbType.VarChar).Value = txtEmail.Text.ToString();
        cmdInsCons.Parameters.Add("@NOM_EMAIL_EMPRESA", SqlDbType.VarChar).Value = txtEmailEstabelecimento.Text.ToString();
        cmdInsCons.Parameters.Add("@NUM_FATURAMENTO", SqlDbType.Float).Value = Funcoes.strToDouble(txtFaturamento.Text.ToString());
        cmdInsCons.Parameters.Add("@NUM_RENDA_MENSAL", SqlDbType.Float).Value = Funcoes.strToDouble(txtRendaMensal.Text.ToString());
        cmdInsCons.Parameters.Add("@COD_ID_MCC", SqlDbType.Int).Value = Funcoes.strToInt(ddlAtividadeEconomica.SelectedValue.ToString());

        cmdInsCons.Parameters.Add("@NOM_ENDERECO", SqlDbType.VarChar).Value = txtEndereco.Text.ToString();
        cmdInsCons.Parameters.Add("@NOM_NUMERO", SqlDbType.VarChar).Value = txtNumero.Text.ToString();
        cmdInsCons.Parameters.Add("@NOM_COMPLEMENTO", SqlDbType.VarChar).Value = txtComplemento.Text.ToString();
        cmdInsCons.Parameters.Add("@NOM_BAIRRO", SqlDbType.VarChar).Value = txtBairro.Text.ToString();
        cmdInsCons.Parameters.Add("@NOM_CIDADE", SqlDbType.VarChar).Value = txtCidade.Text.ToString();
        cmdInsCons.Parameters.Add("@NOM_UF", SqlDbType.VarChar).Value = ddlEstado.SelectedValue.ToString();
        cmdInsCons.Parameters.Add("@NOM_CEP", SqlDbType.VarChar).Value = txtCEP.Text.ToString();
        cmdInsCons.Parameters.Add("@NOM_PAIS", SqlDbType.VarChar).Value = "BR";

        cmdInsCons.Parameters.Add("@COD_ID_PESSOAS_FJ_FAVORECIDO", SqlDbType.Int).Value = Funcoes.strToInt(ddlFavorecido.SelectedValue.ToString());

        cmdInsCons.ExecuteNonQuery();
        connInsCons.Close();
        connInsCons.Dispose();

        ClientScript.RegisterStartupScript(this.GetType(),
            "Alerta", "alert('Dados gravados com sucesso'); ", true);

        ConsultaFicha();


    }
    protected void lkbReenviar_Click(object sender, System.EventArgs e)
    {
        if (Funcoes.Enviar2faEstabelecimento() == true)
        {
            ClientScript.RegisterStartupScript(this.GetType(), "Codigo2fa", "alert('Um código de confirmação foi enviado para o seu e-mail!');", true);

        }
        else
        {
            ClientScript.RegisterStartupScript(this.GetType(), "Codigo2faErro", "alert('Ocorreu um erro ao tentar enviar o Código de Confirmação!');", true);

        }
        ClientScript.RegisterStartupScript(this.GetType(), "ExecutaModal", "$('#mdConfirmar').modal('show');", true);
    }

    protected void btnConfirmar2FA_Click(object sender, System.EventArgs e)
    {
        // Verifica se o código autenticação esta correto
        SqlConnection connVerifica2fa = new SqlConnection(Funcoes.conexao());
        SqlCommand cmdVerifica2fa = new SqlCommand("dbo.stp_2fa_ins", connVerifica2fa);
        cmdVerifica2fa.CommandType = CommandType.StoredProcedure;
        cmdVerifica2fa.Parameters.Add("@flg_operacao", SqlDbType.Char).Value = "L";
        cmdVerifica2fa.Parameters.Add("@COD_ID_SIS_USUARIO", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["CODIGO"].ToString());
        cmdVerifica2fa.Parameters.Add("@COD_ID_PESSOA_LICENCIADO", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["LICENCIADO"].ToString());
        cmdVerifica2fa.Parameters.Add("@NOM_2FA", SqlDbType.VarChar).Value = Funcoes.Encrypt(txt2FA.Text.ToString());
        cmdVerifica2fa.Parameters.Add("@DTA_EXPIRACAO", SqlDbType.DateTime).Value = DateTime.Now;
        connVerifica2fa.Open();
        SqlDataReader mReader2fa = cmdVerifica2fa.ExecuteReader();

        if (mReader2fa.Read())
        {
            SalvarDados();
        }
        else
        {
            ClientScript.RegisterStartupScript(this.GetType(),
                "CodigoErrado2fa", "alert('O Código de confirmação de autenticação não está correto! Verifique e reentre.');", true);
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
    protected void ddlFavorecido_SelectedIndexChanged(object sender, EventArgs e)
    {
        SqlConnection myConsulta = new SqlConnection(Funcoes.conexao());
        myConsulta.Open();
        SqlDataAdapter SDAConsulta = new SqlDataAdapter("dbo.stp_pessoas_fj_ins", myConsulta);
        SDAConsulta.SelectCommand.CommandType = CommandType.StoredProcedure;
        SDAConsulta.SelectCommand.Parameters.Add("@flg_operacao", SqlDbType.Char).Value = "7";
        SDAConsulta.SelectCommand.Parameters.Add("@COD_ID_PESSOA_LICENCIADO", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["LICENCIADO"].ToString());
        SDAConsulta.SelectCommand.Parameters.Add("@COD_ID_PESSOAS_FJ", SqlDbType.Int).Value = Funcoes.strToInt(ddlFavorecido.SelectedValue.ToString());

        DataSet dsConsulta = new DataSet();
        SDAConsulta.Fill(dsConsulta, "PESSOAS_FJ");

        rptIntegracoes.DataSource = dsConsulta.Tables["PESSOAS_FJ"].DefaultView;
        rptIntegracoes.DataBind();

        myConsulta.Close(); myConsulta.Dispose();

    }
    protected void btnIncluirContaBancaria_Click(object sender, EventArgs e)
    {
        SqlConnection mySelCadastro = new SqlConnection(Funcoes.conexao());
        mySelCadastro.Open();
        SqlCommand cmdSelCadastro = new SqlCommand("dbo.stp_pessoas_fj_contas_ins", mySelCadastro);
        cmdSelCadastro.CommandType = CommandType.StoredProcedure;
        cmdSelCadastro.Parameters.Add("@flg_operacao", SqlDbType.Char).Value = "L";
        cmdSelCadastro.Parameters.Add("@COD_ID_PESSOA_LICENCIADO", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["LICENCIADO"].ToString());
        cmdSelCadastro.Parameters.Add("@COD_ID_PESSOAS_FJ", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["PESSOA"].ToString());

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
            cmdInsCons.Parameters.Add("@COD_ID_PESSOAS_FJ", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["PESSOA"].ToString());

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

            ConsultaContas();
        }
        else
        {
            ClientScript.RegisterStartupScript(this.GetType(),
                "Obrigatorio", "alert('Todos os campos são de preenchimento obrigatório! Não foi possível incluir a conta bancária.'); ", true);
        }
    }
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

            ConsultaContas();
        }
        if (e.CommandName == "Padrao")
        {
            SqlConnection connInsCons = new SqlConnection(Funcoes.conexao());
            SqlCommand cmdInsCons = new SqlCommand("dbo.stp_pessoas_fj_contas_ins", connInsCons);
            cmdInsCons.CommandType = CommandType.StoredProcedure;
            cmdInsCons.Parameters.Add("@flg_operacao", SqlDbType.Char).Value = 'U';
            cmdInsCons.Parameters.Add("@COD_ID", SqlDbType.Int).Value = Funcoes.strToInt(Convert.ToString(e.CommandArgument));
            cmdInsCons.Parameters.Add("@COD_ID_PESSOAS_FJ", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["PESSOA"].ToString());
            cmdInsCons.Parameters.Add("@COD_ID_PESSOA_LICENCIADO", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["LICENCIADO"].ToString());
            connInsCons.Open();
            cmdInsCons.ExecuteNonQuery();

            ClientScript.RegisterStartupScript(this.GetType(),
            "PadraoContas", "alert('Registro alterado com sucesso');", true);

            ConsultaContas();
        }
    }
}