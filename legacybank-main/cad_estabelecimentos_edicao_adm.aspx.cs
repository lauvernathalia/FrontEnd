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

public partial class cad_estabelecimentos_edicao_adm : System.Web.UI.Page
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
            Funcoes.GravaAuditoria(0, Request.ServerVariables["REMOTE_ADDR"].ToString(), "Estabelecimentos - edição - adm ", "Acesso");

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

            SqlConnection myPlano = new SqlConnection(Funcoes.conexao());
            myPlano.Open();
            SqlCommand cmdPlano = new SqlCommand("dbo.stp_planos_ins", myPlano);
            cmdPlano.CommandType = CommandType.StoredProcedure;
            cmdPlano.Parameters.Add("@flg_operacao", SqlDbType.VarChar).Value = "L";
            cmdPlano.Parameters.Add("@FLG_ATIVO", SqlDbType.VarChar).Value = "S";
            cmdPlano.Parameters.Add("@COD_ID_PESSOA_LICENCIADO", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["LICENCIADO"].ToString());
            SqlDataAdapter drPlano = new SqlDataAdapter();
            drPlano.SelectCommand = cmdPlano;
            DataSet dsPlano = new DataSet();
            drPlano.Fill(dsPlano, "PLANOS");
            ddlPlano.DataTextField = "NOM_TITULO_PLANO";
            ddlPlano.DataValueField = "COD_ID";
            ddlPlano.DataSource = dsPlano.Tables["PLANOS"].DefaultView;
            ddlPlano.DataBind();
            ddlPlano.Items.Insert(0, new ListItem("", "0"));


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
        SqlCommand cmdSelCadastro = new SqlCommand("dbo.stp_pessoas_fj_ins", mySelCadastro);
        cmdSelCadastro.CommandType = CommandType.StoredProcedure;
        cmdSelCadastro.Parameters.Add("@flg_operacao", SqlDbType.Char).Value = "S";
        cmdSelCadastro.Parameters.Add("@COD_ID", SqlDbType.Int).Value = Funcoes.strToInt(sid_id.ToString());
        cmdSelCadastro.Parameters.Add("@COD_ID_PESSOA_LICENCIADO", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["LICENCIADO"].ToString());
        SqlDataReader ReaderCadastro = cmdSelCadastro.ExecuteReader();
        while (ReaderCadastro.Read())
        {
            lblEstabelecimento.Text = ReaderCadastro["NOM_NOME"].ToString() + " " + ReaderCadastro["NOM_SOBRENOME"].ToString();
            imgFoto.ImageUrl = ReaderCadastro["NOM_FOTO"].ToString();

            // Endereço
            txtCEP.Text = ReaderCadastro["NOM_CEP"].ToString();
            txtEndereco.Text = ReaderCadastro["NOM_ENDERECO"].ToString();
            txtNumero.Text = ReaderCadastro["NOM_NUMERO"].ToString();
            txtComplemento.Text =  ReaderCadastro["NOM_COMPLEMENTO"].ToString();
            txtBairro.Text = ReaderCadastro["NOM_BAIRRO"].ToString();
            txtCidade.Text = ReaderCadastro["NOM_CIDADE"].ToString();
            ddlEstado.SelectedValue = ReaderCadastro["NOM_UF"].ToString();

            lblCEP.Text = ReaderCadastro["NOM_CEP"].ToString();
            lblEndereco.Text = ReaderCadastro["NOM_ENDERECO"].ToString();
            lblNumero.Text = ReaderCadastro["NOM_NUMERO"].ToString();
            lblComplemento.Text = ReaderCadastro["NOM_COMPLEMENTO"].ToString();
            lblBairro.Text = ReaderCadastro["NOM_BAIRRO"].ToString();
            lblCidade.Text = ReaderCadastro["NOM_CIDADE"].ToString();
            lblEstado.Text = ReaderCadastro["NOM_UF"].ToString();

            // Dados
            lblNome.Text = ReaderCadastro["NOM_NOME"].ToString();
            lblSobrenome.Text = ReaderCadastro["NOM_SOBRENOME"].ToString();
            lblCPF.Text = ReaderCadastro["NOM_CPF"].ToString();
            lblTipo.Text = ReaderCadastro["NOM_FLG_TIPO_PESSOA"].ToString();
            lblEmail.Text = ReaderCadastro["NOM_EMAIL"].ToString();
            lblCelular.Text = ReaderCadastro["NOM_CELULAR"].ToString();

            txtNome.Text = ReaderCadastro["NOM_NOME"].ToString();
            txtSobrenome.Text = ReaderCadastro["NOM_SOBRENOME"].ToString();
            txtCPF.Text = ReaderCadastro["NOM_CPF"].ToString();
            txtEmail.Text = ReaderCadastro["NOM_EMAIL"].ToString();
            txtCelular.Text = ReaderCadastro["NOM_CELULAR"].ToString();
            txtNascimento.Text = Convert.ToDateTime(ReaderCadastro["DTA_ANIVERSARIO"].ToString()).ToShortDateString();
            txtNomeMae.Text = ReaderCadastro["NOM_MAE"].ToString();

            // Pagseguro
            ddlPagseguro.SelectedValue = ReaderCadastro["COD_ID_PAGSEGURO_HABILITADO"].ToString();
            txtIDPagseguro.Text = ReaderCadastro["COD_ID_PAGSEGURO"].ToString();
            txtEmailPagseguro.Text = ReaderCadastro["NOM_PAGSEGURO_EMAIL"].ToString();
            txtTokenPagseguro.Text = ReaderCadastro["NOM_PAGSEGURO_TOKEN"].ToString();

            // Dados Bancários

            ddlFormaRecebimento.SelectedValue = ReaderCadastro["FLG_RECEBIMENTO"].ToString();

            txtCodigoAtivacao.Text = ReaderCadastro["COD_ID_PAGSEGURO_ATIVACAO"].ToString();
            ddlVerificadaAtivada.SelectedValue = ReaderCadastro["FLG_PAGSEGURO_VERIFICADA_ATIVADA"].ToString();
            ddlPlano.SelectedValue = ReaderCadastro["COD_ID_PLANO"].ToString();

            ConsultaUsuarios();

        }

    }

    private void ConsultaUsuarios()
    {
        //dtgConsulta.Visible = true;
        SqlConnection myConsulta = new SqlConnection(Funcoes.conexao());
        myConsulta.Open();
        SqlDataAdapter SDAConsulta = new SqlDataAdapter("dbo.stp_sis_usuario_ins", myConsulta);
        SDAConsulta.SelectCommand.CommandType = CommandType.StoredProcedure;
        SDAConsulta.SelectCommand.Parameters.Add("@flg_operacao", SqlDbType.Char).Value = "C";
        SDAConsulta.SelectCommand.Parameters.Add("@COD_ID_PESSOA_LICENCIADO", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["LICENCIADO"].ToString());
        SDAConsulta.SelectCommand.Parameters.Add("@COD_ID_PESSOAS_FJ", SqlDbType.Int).Value = Funcoes.strToInt(sid_id.ToString());

        DataSet dsConsulta = new DataSet();
        SDAConsulta.Fill(dsConsulta, "SIS_USUARIO");
        rptConsultaUsuario.DataSource = dsConsulta.Tables["SIS_USUARIO"].DefaultView;
        rptConsultaUsuario.DataBind();
        myConsulta.Close(); myConsulta.Dispose();

    }

    protected void btnVoltar_Click(object sender, EventArgs e)
    {
        Response.Redirect("con_estabelecimentos_adm.aspx");
    }
    protected void btnSalvar_Click(object sender, EventArgs e)
    {
        // Salva Primeiro cadastro da Pessoa F/J

        SqlConnection connInsCons = new SqlConnection(Funcoes.conexao());
        connInsCons.Open();
        SqlCommand cmdInsCons = new SqlCommand("dbo.stp_pessoas_fj_ins", connInsCons);
        cmdInsCons.CommandType = CommandType.StoredProcedure;
        cmdInsCons.Parameters.Add("@flg_operacao", SqlDbType.Char).Value = 'A';
        cmdInsCons.Parameters.Add("@COD_ID_PESSOA_LICENCIADO", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["LICENCIADO"].ToString());
        cmdInsCons.Parameters.Add("@COD_ID", SqlDbType.Int).Value = Funcoes.strToInt(sid_id.ToString());

        cmdInsCons.Parameters.Add("@NOM_NOME", SqlDbType.VarChar).Value = txtNome.Text.ToString();
        cmdInsCons.Parameters.Add("@NOM_SOBRENOME", SqlDbType.VarChar).Value = txtSobrenome.Text.ToString();
        cmdInsCons.Parameters.Add("@NOM_EMAIL", SqlDbType.VarChar).Value = txtEmail.Text.ToString();
        cmdInsCons.Parameters.Add("@NOM_FANTASIA", SqlDbType.VarChar).Value = txtNome.Text.ToString() + " " + txtSobrenome.Text.ToString();
        cmdInsCons.Parameters.Add("@DTA_ANIVERSARIO", SqlDbType.DateTime).Value = Convert.ToDateTime(txtNascimento.Text.ToString());
        cmdInsCons.Parameters.Add("@NOM_MAE", SqlDbType.VarChar).Value = txtNomeMae.Text.ToString();
        cmdInsCons.Parameters.Add("@NOM_CPF", SqlDbType.VarChar).Value = txtCPF.Text.ToString();
        cmdInsCons.Parameters.Add("@NOM_CELULAR", SqlDbType.VarChar).Value = txtCelular.Text.ToString();

        cmdInsCons.Parameters.Add("@FLG_RECEBIMENTO", SqlDbType.Char).Value = ddlFormaRecebimento.SelectedValue.ToString();
        cmdInsCons.Parameters.Add("@COD_ID_PAGSEGURO_HABILITADO", SqlDbType.Int).Value = Funcoes.strToInt(ddlPagseguro.SelectedValue.ToString());


        cmdInsCons.Parameters.Add("@NOM_ENDERECO", SqlDbType.VarChar).Value = txtEndereco.Text.ToString();
        cmdInsCons.Parameters.Add("@NOM_NUMERO", SqlDbType.VarChar).Value = txtNumero.Text.ToString();
        cmdInsCons.Parameters.Add("@NOM_COMPLEMENTO", SqlDbType.VarChar).Value = txtComplemento.Text.ToString();
        cmdInsCons.Parameters.Add("@NOM_BAIRRO", SqlDbType.VarChar).Value = txtBairro.Text.ToString();
        cmdInsCons.Parameters.Add("@NOM_CIDADE", SqlDbType.VarChar).Value = txtCidade.Text.ToString();
        cmdInsCons.Parameters.Add("@NOM_UF", SqlDbType.VarChar).Value = ddlEstado.SelectedValue.ToString();
        cmdInsCons.Parameters.Add("@NOM_CEP", SqlDbType.VarChar).Value = txtCEP.Text.ToString();

        cmdInsCons.Parameters.Add("@COD_ID_PAGSEGURO", SqlDbType.Int).Value = Funcoes.strToInt(txtIDPagseguro.Text.ToString());
        cmdInsCons.Parameters.Add("@NOM_PAGSEGURO_TOKEN", SqlDbType.VarChar).Value = txtTokenPagseguro.Text.ToString();
        cmdInsCons.Parameters.Add("@NOM_PAGSEGURO_EMAIL", SqlDbType.VarChar).Value = txtEmailPagseguro.Text.ToString();
        cmdInsCons.Parameters.Add("@NOM_PAGSEGURO_ATIVACAO", SqlDbType.VarChar).Value = txtEmailPagseguro.Text.ToString();

        cmdInsCons.Parameters.Add("@COD_ID_PAGSEGURO_ATIVACAO", SqlDbType.VarChar).Value = txtCodigoAtivacao.Text.ToString();
        cmdInsCons.Parameters.Add("@FLG_PAGSEGURO_VERIFICADA_ATIVADA", SqlDbType.VarChar).Value = ddlVerificadaAtivada.SelectedValue.ToString();


        cmdInsCons.Parameters.Add("@NOM_CODIGO_BANCO", SqlDbType.VarChar).Value = ddlInstituicaoFinanceira.SelectedValue.ToString();
        cmdInsCons.Parameters.Add("@NOM_TIPO_BANCO", SqlDbType.VarChar).Value = ddlTipoConta.SelectedValue.ToString();
        cmdInsCons.Parameters.Add("@NOM_NUMERO_AGENCIA_BANCO", SqlDbType.VarChar).Value = txtAgencia.Text.ToString();
        cmdInsCons.Parameters.Add("@NOM_NUMERO_DIGITO_AGENCIA_BANCO", SqlDbType.VarChar).Value = txtDigitoAgencia.Text.ToString();
        cmdInsCons.Parameters.Add("@NOM_NUMERO_CONTA_BANCO", SqlDbType.VarChar).Value = txtConta.Text.ToString();
        cmdInsCons.Parameters.Add("@NOM_NUMERO_DIGITO_CONTA_BANCO", SqlDbType.VarChar).Value = txtDigitoConta.Text.ToString();
        cmdInsCons.Parameters.Add("@COD_ID_PLANO", SqlDbType.Int).Value = Funcoes.strToInt(ddlPlano.SelectedValue.ToString());

        cmdInsCons.ExecuteNonQuery();
        connInsCons.Close();
        connInsCons.Dispose();

        ClientScript.RegisterStartupScript(this.GetType(),
            "Alerta", "alert('Dados gravados com sucesso'); ", true);

        Response.Redirect("con_estabelecimentos_adm.aspx");

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