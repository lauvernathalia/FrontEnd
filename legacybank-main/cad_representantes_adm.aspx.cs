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


public partial class cad_representantes_adm : System.Web.UI.Page
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
        if ((faVerificadoSenha.Visible) == true && (faVerificadoEmail.Visible == true))
        {
            btnSalvar.Visible = true;
        }
        else
        {
            btnSalvar.Visible = true;

        }
        if (!IsPostBack)
        {
            SqlConnection myMkt = new SqlConnection(Funcoes.conexao());
            myMkt.Open();
            SqlCommand cmdMkt = new SqlCommand("dbo.stp_pessoas_fj_ins", myMkt);
            cmdMkt.CommandType = CommandType.StoredProcedure;
            cmdMkt.Parameters.Add("@flg_operacao", SqlDbType.VarChar).Value = "L";
            cmdMkt.Parameters.Add("@COD_ID_PESSOA_LICENCIADO", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["LICENCIADO"].ToString());
            cmdMkt.Parameters.Add("@FLG_ATIVO", SqlDbType.Char).Value = "S";
            cmdMkt.Parameters.Add("@FLG_TIPO", SqlDbType.Char).Value = "M";
            SqlDataAdapter drMkt = new SqlDataAdapter();
            drMkt.SelectCommand = cmdMkt;
            DataSet dsMkt = new DataSet();
            drMkt.Fill(dsMkt, "PESSOAS_FJ");
            ddlMarketplace.DataTextField = "NOM_RAZAOSOCIAL";
            ddlMarketplace.DataValueField = "COD_ID";
            ddlMarketplace.DataSource = dsMkt.Tables["PESSOAS_FJ"].DefaultView;
            ddlMarketplace.DataBind();
            ddlMarketplace.Items.Insert(0, new ListItem("", "0"));


            if (Funcoes.strToInt(sid_id) != 0)
            {
                ConsultaFicha();
                dvSenha.Visible = false;
            }
            else
            {
                dvSenha.Visible = true;
            }
        }

    }

    private void ConsultaFicha()
    {
        SqlConnection mySelCadastro = new SqlConnection(Funcoes.conexao());
        mySelCadastro.Open();
        SqlCommand cmdSelCadastro = new SqlCommand("dbo.stp_pessoas_fj_mkt_ins", mySelCadastro);
        cmdSelCadastro.CommandType = CommandType.StoredProcedure;
        cmdSelCadastro.Parameters.Add("@flg_operacao", SqlDbType.Char).Value = "S";
        cmdSelCadastro.Parameters.Add("@COD_ID", SqlDbType.Int).Value = Funcoes.strToInt(sid_id.ToString());
        cmdSelCadastro.Parameters.Add("@COD_ID_PESSOAS_FJ", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["PESSOA"].ToString());
        cmdSelCadastro.Parameters.Add("@COD_ID_PESSOA_LICENCIADO", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["LICENCIADO"].ToString());
        SqlDataReader ReaderCadastro = cmdSelCadastro.ExecuteReader();
        while (ReaderCadastro.Read())
        {
            lblID.Text = ReaderCadastro["COD_ID"].ToString();

            txtNome.Text = ReaderCadastro["NOM_NOME"].ToString();
            txtSobrenome.Text = ReaderCadastro["NOM_SOBRENOME"].ToString();
            txtDocumento.Text = ReaderCadastro["NOM_CPF"].ToString();
            txtEmail.Text = ReaderCadastro["NOM_EMAIL"].ToString();
            txtEmailC.Text = ReaderCadastro["NOM_EMAIL"].ToString();

            txtRazaoSocial.Text = ReaderCadastro["NOM_RAZAOSOCIAL"].ToString();
            txtFantasia.Text = ReaderCadastro["NOM_FANTASIA"].ToString();
            txtCNPJ.Text = ReaderCadastro["NOM_CNPJ"].ToString();
            txtTelefoneEmpresa.Text = ReaderCadastro["NOM_CELULAR"].ToString();
            txtEmailEmpresa.Text = ReaderCadastro["NOM_EMAIL_EMPRESA"].ToString();

            txtEndereco.Text = ReaderCadastro["NOM_ENDERECO"].ToString();
            txtNumero.Text = ReaderCadastro["NOM_NUMERO"].ToString();
            txtComplemento.Text = ReaderCadastro["NOM_COMPLEMENTO"].ToString();
            txtBairro.Text = ReaderCadastro["NOM_BAIRRO"].ToString();
            txtCidade.Text = ReaderCadastro["NOM_CIDADE"].ToString();
            ddlEstado.SelectedValue = ReaderCadastro["NOM_UF"].ToString();
            txtCEP.Text = ReaderCadastro["NOM_CEP"].ToString();
            ddlTipoFJ.SelectedValue = ReaderCadastro["FLG_TIPO_PESSOA"].ToString();

            ddlMarketplace.SelectedValue = ReaderCadastro["COD_ID_MARKETPLACE"].ToString();
            
            ddlTipoFJ_SelectedIndexChanged(null, null);
        }

    }

    protected void btnSalvar_Click(object sender, EventArgs e)
    {
        SqlConnection connInsCons = new SqlConnection(Funcoes.conexao());
        connInsCons.Open();
        SqlCommand cmdInsCons = new SqlCommand("dbo.stp_pessoas_fj_mkt_ins", connInsCons);
        cmdInsCons.CommandType = CommandType.StoredProcedure;
        if (Funcoes.strToInt(sid_id) != 0)
        {
            cmdInsCons.Parameters.Add("@flg_operacao", SqlDbType.Char).Value = 'A';
            cmdInsCons.Parameters.Add("@COD_ID", SqlDbType.Int).Value = Funcoes.strToInt(sid_id.ToString());
        }
        else
        {
            cmdInsCons.Parameters.Add("@flg_operacao", SqlDbType.Char).Value = 'I';
        }
        cmdInsCons.Parameters.Add("@COD_ID_PESSOA_LICENCIADO", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["LICENCIADO"].ToString());
        cmdInsCons.Parameters.Add("@COD_ID_PESSOAS_FJ", SqlDbType.Int).Value = ddlMarketplace.SelectedValue.ToString();

        cmdInsCons.Parameters.Add("@DTA_DATA", SqlDbType.DateTime).Value = DateTime.Now;
        cmdInsCons.Parameters.Add("@FLG_TIPO", SqlDbType.Char).Value = "R";
        cmdInsCons.Parameters.Add("@FLG_ATIVO", SqlDbType.Char).Value = "S";
        cmdInsCons.Parameters.Add("@FLG_TIPO_PESSOA", SqlDbType.VarChar).Value = ddlTipoFJ.SelectedValue.ToString();


        cmdInsCons.Parameters.Add("@NOM_NOME", SqlDbType.VarChar).Value = txtNome.Text.ToString();
        cmdInsCons.Parameters.Add("@NOM_SOBRENOME", SqlDbType.VarChar).Value = txtSobrenome.Text.ToString();
        cmdInsCons.Parameters.Add("@NUM_DOCUMENTO", SqlDbType.VarChar).Value = txtDocumento.Text.ToString();
        cmdInsCons.Parameters.Add("@NOM_CPF", SqlDbType.VarChar).Value = txtDocumento.Text.ToString();
        cmdInsCons.Parameters.Add("@NOM_EMAIL", SqlDbType.VarChar).Value = txtEmail.Text.ToString();

        if (Funcoes.strToInt(sid_id.ToString().Trim()) == 0)
        {
            cmdInsCons.Parameters.Add("@NOM_LOGIN", SqlDbType.VarChar).Value = txtEmail.Text.ToString();
            cmdInsCons.Parameters.Add("@NOM_SENHA", SqlDbType.VarChar).Value = txtSenha.Text.ToString();
        }


        cmdInsCons.Parameters.Add("@NOM_RAZAOSOCIAL", SqlDbType.VarChar).Value = txtRazaoSocial.Text.ToString();
        cmdInsCons.Parameters.Add("@NOM_FANTASIA", SqlDbType.VarChar).Value = txtFantasia.Text.ToString();
        cmdInsCons.Parameters.Add("@NOM_CNPJ", SqlDbType.VarChar).Value = txtCNPJ.Text.ToString();
        cmdInsCons.Parameters.Add("@NOM_CELULAR", SqlDbType.VarChar).Value = txtTelefoneEmpresa.Text.ToString();
        cmdInsCons.Parameters.Add("@NUM_TELEFONE", SqlDbType.VarChar).Value = txtTelefoneEmpresa.Text.ToString();
        cmdInsCons.Parameters.Add("@NOM_EMAIL_EMPRESA", SqlDbType.VarChar).Value = txtEmailEmpresa.Text.ToString();


        cmdInsCons.Parameters.Add("@NOM_ENDERECO", SqlDbType.VarChar).Value = txtEndereco.Text.ToString();
        cmdInsCons.Parameters.Add("@NOM_NUMERO", SqlDbType.VarChar).Value = txtNumero.Text.ToString();
        cmdInsCons.Parameters.Add("@NOM_COMPLEMENTO", SqlDbType.VarChar).Value = txtComplemento.Text.ToString();
        cmdInsCons.Parameters.Add("@NOM_BAIRRO", SqlDbType.VarChar).Value = txtBairro.Text.ToString();
        cmdInsCons.Parameters.Add("@NOM_CIDADE", SqlDbType.VarChar).Value = txtCidade.Text.ToString();
        cmdInsCons.Parameters.Add("@NOM_UF", SqlDbType.VarChar).Value = ddlEstado.SelectedValue.ToString();
        cmdInsCons.Parameters.Add("@NOM_CEP", SqlDbType.VarChar).Value = txtCEP.Text.ToString();
        cmdInsCons.Parameters.Add("@NOM_PAIS", SqlDbType.VarChar).Value = "BR";

        cmdInsCons.ExecuteNonQuery();
        connInsCons.Close();
        connInsCons.Dispose();

        ClientScript.RegisterStartupScript(this.GetType(),
            "Alerta", "alert('Dados gravados com sucesso'); opener.PostBackOnMainPage(); window.close(); ", true);
    }
    protected void btnCancelar_Click(object sender, EventArgs e)
    {
        ClientScript.RegisterStartupScript(this.GetType(),
            "Alerta", "opener.PostBackOnMainPage(); window.close(); ", true);
    }

    protected void txtCEP_TextChanged(object sender, EventArgs e)
    {
        if ((txtCEP.Text.ToString().Trim() != "") && (txtCEP.Text.ToString().Trim() != "_____-___"))
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
            dvEmpresa.Visible = false;
        }
        else
        {
            dvEmpresa.Visible = true;
        }
    }
    protected void txtSenhaC_TextChanged(object sender, EventArgs e)
    {
        if (txtSenha.Text.ToString() == txtSenhaC.Text.ToString())
        {
            faVerificadoSenha.Visible = true;
        }
        else
        {
            faVerificadoSenha.Visible = false;
        }
    }
    protected void txtEmailC_TextChanged(object sender, EventArgs e)
    {
        if (txtEmail.Text.ToString() == txtEmailC.Text.ToString())
        {
            faVerificadoEmail.Visible = true;
        }
        else
        {
            faVerificadoEmail.Visible = false;
        }

    }
}