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



public partial class cad_estabelecimentos_pagseguro : System.Web.UI.Page
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
            Funcoes.GravaAuditoria(Funcoes.strToInt(sid_id), Request.ServerVariables["REMOTE_ADDR"].ToString(), "Cadastro - Estabelecimentos - Pagseguro", "Acesso - ID: " + Funcoes.strToInt(sid_id).ToString());

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
            txtID.Text = ReaderCadastro["COD_ID"].ToString();
            txtRazaosocial.Text = ReaderCadastro["NOM_RAZAOSOCIAL"].ToString();

            ddlPagseguro.SelectedValue = ReaderCadastro["COD_ID_PAGSEGURO_HABILITADO"].ToString();

            txtIDPagseguro.Text = ReaderCadastro["COD_ID_PAGSEGURO"].ToString();
            txtTokenPagseguro.Text = ReaderCadastro["NOM_PAGSEGURO_TOKEN"].ToString();
            txtEmailPagseguro.Text = ReaderCadastro["NOM_PAGSEGURO_EMAIL"].ToString();
            txtEmailPagseguro.Text = ReaderCadastro["NOM_PAGSEGURO_ATIVACAO"].ToString();

            txtCodigoAtivacao.Text = ReaderCadastro["COD_ID_PAGSEGURO_ATIVACAO"].ToString();
            ddlVerificadaAtivada.SelectedValue = ReaderCadastro["FLG_PAGSEGURO_VERIFICADA_ATIVADA"].ToString();

            ddlInstituicaoFinanceira.SelectedValue = ReaderCadastro["NOM_CODIGO_BANCO"].ToString();
            ddlTipoConta.SelectedValue = ReaderCadastro["NOM_TIPO_BANCO"].ToString();
            txtAgencia.Text = ReaderCadastro["NOM_NUMERO_AGENCIA_BANCO"].ToString();
            txtDigitoAgencia.Text = ReaderCadastro["NOM_NUMERO_DIGITO_AGENCIA_BANCO"].ToString();
            txtConta.Text = ReaderCadastro["NOM_NUMERO_CONTA_BANCO"].ToString();
            txtDigitoConta.Text = ReaderCadastro["NOM_NUMERO_DIGITO_CONTA_BANCO"].ToString();
            ddlPlano.SelectedValue = ReaderCadastro["COD_ID_PLANO"].ToString();
            if (HttpContext.Current.Session["TIPO"].ToString() == "R")
            {
                if (
                    (txtIDPagseguro.Text.ToString().Trim() == "") ||
                    (txtTokenPagseguro.Text.ToString().Trim() == "") ||
                    (txtEmailPagseguro.Text.ToString().Trim() == "") ||
                    (txtCodigoAtivacao.Text.ToString().Trim() == "")
                   )
                {
                    btnSalvar.Visible = true;
                }
                else
                {
                    btnSalvar.Visible = false;
                }
            }
        }

    }

    protected void btnSalvar_Click(object sender, EventArgs e)
    {
        Funcoes.GravaAuditoria(Funcoes.strToInt(sid_id), Request.ServerVariables["REMOTE_ADDR"].ToString(), "Cadastro - Estabelecimentos - Pagseguro", "Gravar - ID: " + Funcoes.strToInt(sid_id).ToString());
        // Salva Primeiro cadastro da Pessoa F/J

        SqlConnection connInsCons = new SqlConnection(Funcoes.conexao());
        connInsCons.Open();
        SqlCommand cmdInsCons = new SqlCommand("dbo.stp_pessoas_fj_ins", connInsCons);
        cmdInsCons.CommandType = CommandType.StoredProcedure;
        cmdInsCons.Parameters.Add("@flg_operacao", SqlDbType.Char).Value = 'P';
        cmdInsCons.Parameters.Add("@COD_ID_PESSOA_LICENCIADO", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["LICENCIADO"].ToString());
        cmdInsCons.Parameters.Add("@COD_ID", SqlDbType.Int).Value = Funcoes.strToInt(sid_id.ToString());

        cmdInsCons.Parameters.Add("@FLG_RECEBIMENTO", SqlDbType.Char).Value = ddlFormaRecebimento.SelectedValue.ToString();
        cmdInsCons.Parameters.Add("@COD_ID_PAGSEGURO_HABILITADO", SqlDbType.Int).Value = Funcoes.strToInt(ddlPagseguro.SelectedValue.ToString());


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
            "Alerta", "alert('Dados gravados com sucesso'); opener.PostBackOnMainPage(); window.close(); ", true);
    }
    protected void btnCancelar_Click(object sender, EventArgs e)
    {
        ClientScript.RegisterStartupScript(this.GetType(),
            "Alerta", "opener.PostBackOnMainPage(); window.close(); ", true);
    }

}