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


public partial class cad_marketplaces_contas : System.Web.UI.Page
{
    public string sid_id
    {
        get
        {
            try { return Funcoes.Decrypt(Request["id"].ToString()); }
            catch { return ""; }
        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
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
                ConsultaGeral();
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
            txtEmail.Text = ReaderCadastro["NOM_EMAIL"].ToString();
            txtTipo.Text = ReaderCadastro["NOM_FLG_TIPO_PESSOA"].ToString();
            if (ReaderCadastro["FLG_TIPO_PESSOA"].ToString().Trim() == "PF")
            {
                txtDocumento.Text = ReaderCadastro["NOM_CPF"].ToString();
            }
            else
            {
                txtDocumento.Text = ReaderCadastro["NOM_CNPJ"].ToString();

            }
        }

    }


    protected void btnFechar_Click(object sender, EventArgs e)
    {
        ClientScript.RegisterStartupScript(this.GetType(), "FecharOnboarding", "opener.PostBackOnMainPage(); window.close(); ", true);

    }

    private void ConsultaGeral()
    {
        SqlConnection myConsulta = new SqlConnection(Funcoes.conexao());
        myConsulta.Open();
        SqlDataAdapter SDAConsulta = new SqlDataAdapter("dbo.stp_pessoas_fj_contas_ins", myConsulta);
        SDAConsulta.SelectCommand.CommandType = CommandType.StoredProcedure;
        SDAConsulta.SelectCommand.Parameters.Add("@flg_operacao", SqlDbType.Char).Value = "C";
        SDAConsulta.SelectCommand.Parameters.Add("@COD_ID_PESSOA_LICENCIADO", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["LICENCIADO"].ToString());
        SDAConsulta.SelectCommand.Parameters.Add("@COD_ID_PESSOAS_FJ", SqlDbType.Int).Value = Funcoes.strToInt(sid_id.ToString());

        DataSet dsConsulta = new DataSet();
        SDAConsulta.Fill(dsConsulta, "PESSOAS_FJ_CONTAS");
        rptConsultaContas.DataSource = dsConsulta.Tables["PESSOAS_FJ_CONTAS"].DefaultView;
        rptConsultaContas.DataBind();
        myConsulta.Close(); myConsulta.Dispose();
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

            ConsultaGeral();
        }
        if (e.CommandName == "Padrao")
        {
            SqlConnection connInsCons = new SqlConnection(Funcoes.conexao());
            SqlCommand cmdInsCons = new SqlCommand("dbo.stp_pessoas_fj_contas_ins", connInsCons);
            cmdInsCons.CommandType = CommandType.StoredProcedure;
            cmdInsCons.Parameters.Add("@flg_operacao", SqlDbType.Char).Value = 'U';
            cmdInsCons.Parameters.Add("@COD_ID", SqlDbType.Int).Value = Funcoes.strToInt(Convert.ToString(e.CommandArgument));
            cmdInsCons.Parameters.Add("@COD_ID_PESSOAS_FJ", SqlDbType.Int).Value = Funcoes.strToInt(sid_id.ToString());
            cmdInsCons.Parameters.Add("@COD_ID_PESSOA_LICENCIADO", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["LICENCIADO"].ToString());
            connInsCons.Open();
            cmdInsCons.ExecuteNonQuery();

            ClientScript.RegisterStartupScript(this.GetType(),
            "PadraoContas", "alert('Registro alterado com sucesso');", true);

            ConsultaGeral();
        }

    }


    protected void btnIncluirContaBancaria_Click(object sender, EventArgs e)
    {
        SqlConnection mySelCadastro = new SqlConnection(Funcoes.conexao());
        mySelCadastro.Open();
        SqlCommand cmdSelCadastro = new SqlCommand("dbo.stp_pessoas_fj_contas_ins", mySelCadastro);
        cmdSelCadastro.CommandType = CommandType.StoredProcedure;
        cmdSelCadastro.Parameters.Add("@flg_operacao", SqlDbType.Char).Value = "L";
        cmdSelCadastro.Parameters.Add("@COD_ID_PESSOA_LICENCIADO", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["LICENCIADO"].ToString());
        cmdSelCadastro.Parameters.Add("@COD_ID_PESSOAS_FJ", SqlDbType.Int).Value = Funcoes.strToInt(sid_id.ToString());

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
            cmdInsCons.Parameters.Add("@COD_ID_PESSOAS_FJ", SqlDbType.Int).Value = Funcoes.strToInt(sid_id.ToString());

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

            ConsultaGeral();
        }
        else
        {
            ClientScript.RegisterStartupScript(this.GetType(),
                "Obrigatorio", "alert('Todos os campos são de preenchimento obrigatório! Não foi possível incluir a conta bancária.'); ", true);
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

}