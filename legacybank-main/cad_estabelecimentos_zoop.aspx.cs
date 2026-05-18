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

public partial class cad_estabelecimentos_zoop : System.Web.UI.Page
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
            Funcoes.GravaAuditoria(Funcoes.strToInt(sid_id), Request.ServerVariables["REMOTE_ADDR"].ToString(), "Cadastro - Estabelecimentos - Zoop", "Acesso - ID: " + Funcoes.strToInt(sid_id).ToString());

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
            cmdPlano.Parameters.Add("@flg_operacao", SqlDbType.VarChar).Value = "G";
            cmdPlano.Parameters.Add("@FLG_ATIVO", SqlDbType.VarChar).Value = "S";
            cmdPlano.Parameters.Add("@COD_ID_PESSOA_LICENCIADO", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["LICENCIADO"].ToString());
            if (HttpContext.Current.Session["TIPO"].ToString() != "A")
            {
                cmdPlano.Parameters.Add("@COD_ID_PESSOAS_FJ", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["PESSOA"].ToString());
            }
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

            ddlZoop.SelectedValue = ReaderCadastro["COD_ID_ZOOP_HABILITADO"].ToString();

            txtToken.Text = ReaderCadastro["NUM_TOKEN"].ToString();

            txtStatus.Text = ReaderCadastro["FLG_STATUS_ZOOP"].ToString();
            ddlPlano.SelectedValue = ReaderCadastro["COD_ID_PLANO_ZOOP"].ToString();

            ddlSaque.SelectedValue = ReaderCadastro["FLG_SAQUE_AUTOMATICA"].ToString();
            ddlPeriodicidade.SelectedValue = ReaderCadastro["FLG_PERIODICIDADE_SAQUE"].ToString();
            txtValorMinimo.Text = ReaderCadastro["NUM_VALOR_MINIMO_SAQUE"].ToString();
        }

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


    protected void btnSalvar_Click(object sender, EventArgs e)
    {
        CadastrarEstabelecimento();
        CadastrarPlano();
        CadastrarContas();
        GravarDadosEstabelecimento();
        AlterarPoliticaRecebimento();
    }

    private void GravarDadosEstabelecimento()
    {
        Funcoes.GravaAuditoria(Funcoes.strToInt(sid_id), Request.ServerVariables["REMOTE_ADDR"].ToString(), "Cadastro - Estabelecimentos - Zoop", "Gravar - ID: " + Funcoes.strToInt(sid_id).ToString());
        // Salva Primeiro cadastro da Pessoa F/J

        SqlConnection connInsCons = new SqlConnection(Funcoes.conexao());
        connInsCons.Open();
        SqlCommand cmdInsCons = new SqlCommand("dbo.stp_pessoas_fj_ins", connInsCons);
        cmdInsCons.CommandType = CommandType.StoredProcedure;
        cmdInsCons.Parameters.Add("@flg_operacao", SqlDbType.Char).Value = 'O';
        cmdInsCons.Parameters.Add("@COD_ID_PESSOA_LICENCIADO", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["LICENCIADO"].ToString());
        cmdInsCons.Parameters.Add("@COD_ID", SqlDbType.Int).Value = Funcoes.strToInt(sid_id.ToString());

        cmdInsCons.Parameters.Add("@COD_ID_ZOOP_HABILITADO", SqlDbType.Int).Value = Funcoes.strToInt(ddlZoop.SelectedValue.ToString());
        cmdInsCons.Parameters.Add("@NUM_TOKEN", SqlDbType.VarChar).Value = txtToken.Text.ToString();
        cmdInsCons.Parameters.Add("@FLG_STATUS_ZOOP", SqlDbType.VarChar).Value = txtStatus.Text.ToString();

        cmdInsCons.Parameters.Add("@FLG_SAQUE_AUTOMATICA", SqlDbType.Char).Value = ddlSaque.SelectedValue.ToString();
        cmdInsCons.Parameters.Add("@FLG_PERIODICIDADE_SAQUE", SqlDbType.Char).Value = ddlPeriodicidade.SelectedValue.ToString();
        cmdInsCons.Parameters.Add("@NUM_VALOR_MINIMO_SAQUE", SqlDbType.Int).Value = Funcoes.strToInt(txtValorMinimo.Text.ToString());

        cmdInsCons.Parameters.Add("@COD_ID_PLANO_ZOOP", SqlDbType.Int).Value = Funcoes.strToInt(ddlPlano.SelectedValue.ToString());

        // Gravar dados Saque Automático


        cmdInsCons.ExecuteNonQuery();
        connInsCons.Close();
        connInsCons.Dispose();


        ClientScript.RegisterStartupScript(this.GetType(),
            "Alerta", "alert('Dados gravados com sucesso');  ", true);

    }

    private void AlterarPoliticaRecebimento()
    {
        string sIntervalo = "";
        if (ddlPeriodicidade.SelectedValue.ToString()=="D"){sIntervalo = "daily";}
        if (ddlPeriodicidade.SelectedValue.ToString()=="S"){sIntervalo = "weekly";}
        if (ddlPeriodicidade.SelectedValue.ToString()=="M"){sIntervalo = "monthly";}
        bool bSaque = (ddlSaque.SelectedValue.ToString()=="S")? true : false;

        dadosPoliticaRecebimento.PoliticaRecebimento dpolitica = new dadosPoliticaRecebimento.PoliticaRecebimento()
        {
            transfer_interval = sIntervalo,
            transfer_day = 1,
            transfer_enabled = bSaque,
            minimum_transfer_value = Funcoes.strToInt(txtValorMinimo.Text.ToString()),
        };

        string json = JsonConvert.SerializeObject(dpolitica);
        string retornoInclusao = zoop.PoliticaRecebimento(txtToken.Text.ToString(), json);
    }
    
    protected void btnCancelar_Click(object sender, EventArgs e)
    {
        ClientScript.RegisterStartupScript(this.GetType(),
            "Alerta", "opener.PostBackOnMainPage(); window.close(); ", true);
    }
    protected void btnIncluir_Click(object sender, EventArgs e)
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
                "Alerta", "alert('Já existe outro conta bancária com estes dados! Verifique e reentre.'); ", true);

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

            SqlConnection mySelCadastro = new SqlConnection(Funcoes.conexao());
            mySelCadastro.Open();
            SqlCommand cmdSelCadastro = new SqlCommand("dbo.stp_pessoas_fj_contas_ins", mySelCadastro);
            cmdSelCadastro.CommandType = CommandType.StoredProcedure;
            cmdSelCadastro.Parameters.Add("@flg_operacao", SqlDbType.Char).Value = "S";
            cmdSelCadastro.Parameters.Add("@COD_ID", SqlDbType.Int).Value = Funcoes.strToInt(Convert.ToString(e.CommandArgument));
            cmdSelCadastro.Parameters.Add("@COD_ID_PESSOA_LICENCIADO", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["LICENCIADO"].ToString());
            SqlDataReader ReaderCadastro = cmdSelCadastro.ExecuteReader();
            while (ReaderCadastro.Read())
            {
                // Altera conta associada ao vendedor

                if (ReaderCadastro["NOM_TOKEN"].ToString().Trim() != "")
                {
                    dadosAssociarContaBancaria.AssociarContaBancaria dassociarcontabancaria = new dadosAssociarContaBancaria.AssociarContaBancaria()
                    {
                        customer = txtToken.Text.ToString(),
                        token = ReaderCadastro["NUM_ID_CONTA"].ToString().Trim(),
                    };

                    string json = JsonConvert.SerializeObject(dassociarcontabancaria);
                    string retornoInclusao = zoop.AssociarContaBancariaVendedor(json);
                }

            }            
            
            
            ClientScript.RegisterStartupScript(this.GetType(),
            "PadraoContas", "alert('Registro alterado com sucesso');", true);

            ConsultaGeral();
        }

    }

    private void CadastrarEstabelecimento()
    {
        //ConsultaID();
        
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
            if (txtToken.Text.ToString().Trim() != "")
            {
                ClientScript.RegisterStartupScript(this.GetType(),
                "ESTABELECIMENTOZOOP", "alert('O estabelecimento " + ReaderCadastro["FLG_TIPO_PESSOA"].ToString().Trim() + " já se encontra cadastrado!');", true);
            }

            string json = "";
            if (ReaderCadastro["FLG_TIPO_PESSOA"].ToString().Trim() == "PF")
            {
                json = json + "{";
                json = json + "\"first_name\":\"" + ReaderCadastro["NOM_NOME"].ToString().Trim() + "\",";
                json = json + "\"last_name\":\"" + ReaderCadastro["NOM_SOBRENOME"].ToString().Trim() + "\",";
                json = json + "\"email\":\"" + ReaderCadastro["NOM_EMAIL"].ToString().Trim() + "\",";
                json = json + "\"phone_number\":\"" + TIRAACENTOS(ReaderCadastro["NOM_CELULAR"].ToString().Trim()) + "\",";
                json = json + "\"taxpayer_id\":\"" + TIRAACENTOS(ReaderCadastro["NOM_CPF"].ToString().Trim()) + "\",";
                json = json + "\"birthdate\":\"" + Convert.ToDateTime(ReaderCadastro["DTA_ANIVERSARIO"].ToString().Trim()).Year.ToString().PadLeft(4, '0') + "-" + Convert.ToDateTime(ReaderCadastro["DTA_ANIVERSARIO"].ToString().Trim()).Month.ToString().PadLeft(2, '0') + "-" + Convert.ToDateTime(ReaderCadastro["DTA_ANIVERSARIO"].ToString().Trim()).Day.ToString().PadLeft(2, '0') + "\",";
                json = json + "\"statement_descriptor\":\"" + TIRAACENTOS(ReaderCadastro["NOM_FANTASIA"].ToString().Trim()) + "\",";
                json = json + "\"revenue\":\"" + Funcoes.strToDouble(ReaderCadastro["NUM_FATURAMENTO"].ToString().Trim()).ToString() + "\",";


                json = json + "\"address\":{";
                json = json + "\"line1\":\"" + ReaderCadastro["NOM_ENDERECO"].ToString().Trim() + "\",";
                json = json + "\"line2\":\"" + ReaderCadastro["NOM_NUMERO"].ToString().Trim() + "\",";
                json = json + "\"line3\":\"" + ReaderCadastro["NOM_COMPLEMENTO"].ToString().Trim() + "\",";
                json = json + "\"neighborhood\":\"" + ReaderCadastro["NOM_BAIRRO"].ToString().Trim() + "\",";
                json = json + "\"city\":\"" + ReaderCadastro["NOM_CIDADE"].ToString().Trim() + "\",";
                json = json + "\"state\":\"" + ReaderCadastro["NOM_UF"].ToString().Trim() + "\",";
                json = json + "\"postal_code\":\"" + TIRAACENTOS(ReaderCadastro["NOM_CEP"].ToString().Trim()) + "\",";
                json = json + "\"country_code\":\"" + "BR" + "\"";
                if (txtToken.Text.ToString().Trim() == "")
                {
                    json = json + "},";
                    json = json + "\"mcc\":\"" + ReaderCadastro["COD_ID_MCC"].ToString().Trim() + "\"}";
                }
                else
                {
                    json = json + "}}";
                }
            }

            if (ReaderCadastro["FLG_TIPO_PESSOA"].ToString().Trim() == "PJ")
            {
                json = json + "{ \"owner\":{";
                json = json + "\"first_name\":\"" + ReaderCadastro["NOM_NOME"].ToString().Trim() + "\",";
                json = json + "\"last_name\":\"" + ReaderCadastro["NOM_SOBRENOME"].ToString().Trim() + "\",";
                json = json + "\"email\":\"" + ReaderCadastro["NOM_EMAIL"].ToString().Trim() + "\",";
                json = json + "\"phone_number\":\"" + TIRAACENTOS(ReaderCadastro["NOM_CELULAR"].ToString().Trim()) + "\",";
                json = json + "\"taxpayer_id\":\"" + TIRAACENTOS(ReaderCadastro["NOM_CPF"].ToString().Trim()) + "\",";
                json = json + "\"birthdate\":\"" + Convert.ToDateTime(ReaderCadastro["DTA_ANIVERSARIO"].ToString().Trim()).Year.ToString().PadLeft(4, '0') + "-" + Convert.ToDateTime(ReaderCadastro["DTA_ANIVERSARIO"].ToString().Trim()).Month.ToString().PadLeft(2, '0') + "-" + Convert.ToDateTime(ReaderCadastro["DTA_ANIVERSARIO"].ToString().Trim()).Day.ToString().PadLeft(2, '0') + "\"";
                json = json + "},";
                json = json + "\"description\":\"" + ReaderCadastro["NOM_RAZAOSOCIAL"].ToString().Trim() + "\",";
                json = json + "\"business_name\":\"" + ReaderCadastro["NOM_RAZAOSOCIAL"].ToString().Trim() + "\",";
                json = json + "\"business_phone\":\"" + TIRAACENTOS(ReaderCadastro["NUM_TELEFONE"].ToString().Trim()) + "\",";
                json = json + "\"business_email\":\"" + ReaderCadastro["NOM_EMAIL_EMPRESA"].ToString().Trim() + "\",";
                json = json + "\"business_description\":\"" + ReaderCadastro["NOM_RAZAOSOCIAL"].ToString().Trim() + "\",";
                json = json + "\"ein\":\"" + TIRAACENTOS(ReaderCadastro["NOM_CNPJ"].ToString().Trim()) + "\",";
                json = json + "\"statement_descriptor\":\"" + ReaderCadastro["NOM_FANTASIA"].ToString().Trim() + "\",";
                json = json + "\"revenue\":\"" + Funcoes.strToDouble(ReaderCadastro["NUM_FATURAMENTO"].ToString().Trim()).ToString() + "\",";

                json = json + "\"business_address\":{";
                json = json + "\"line1\":\"" + ReaderCadastro["NOM_ENDERECO"].ToString().Trim() + "\",";
                json = json + "\"line2\":\"" + ReaderCadastro["NOM_NUMERO"].ToString().Trim() + "\",";
                json = json + "\"line3\":\"" + ReaderCadastro["NOM_COMPLEMENTO"].ToString().Trim() + "\",";
                json = json + "\"neighborhood\":\"" + ReaderCadastro["NOM_BAIRRO"].ToString().Trim() + "\",";
                json = json + "\"city\":\"" + ReaderCadastro["NOM_CIDADE"].ToString().Trim() + "\",";
                json = json + "\"state\":\"" + ReaderCadastro["NOM_UF"].ToString().Trim() + "\",";
                json = json + "\"postal_code\":\"" + TIRAACENTOS(ReaderCadastro["NOM_CEP"].ToString().Trim()) + "\",";
                json = json + "\"country_code\":\"" + "BR" + "\"";
                json = json + "},";
                if (ReaderCadastro["DTA_ABERTURA"].ToString().Trim() != "")
                {
                    json = json + "\"business_opening_date\":\"" + Convert.ToDateTime(ReaderCadastro["DTA_ABERTURA"].ToString().Trim()).Year.ToString().PadLeft(4, '0') + "-" + Convert.ToDateTime(ReaderCadastro["DTA_ABERTURA"].ToString().Trim()).Month.ToString().PadLeft(2, '0') + "-" + Convert.ToDateTime(ReaderCadastro["DTA_ABERTURA"].ToString().Trim()).Day.ToString().PadLeft(2, '0') + "\",";
                }

                json = json + "\"owner_address\":{";
                json = json + "\"line1\":\"" + ReaderCadastro["NOM_ENDERECO"].ToString().Trim() + "\",";
                json = json + "\"line2\":\"" + ReaderCadastro["NOM_NUMERO"].ToString().Trim() + "\",";
                json = json + "\"line3\":\"" + ReaderCadastro["NOM_COMPLEMENTO"].ToString().Trim() + "\",";
                json = json + "\"neighborhood\":\"" + ReaderCadastro["NOM_BAIRRO"].ToString().Trim() + "\",";
                json = json + "\"city\":\"" + ReaderCadastro["NOM_CIDADE"].ToString().Trim() + "\",";
                json = json + "\"state\":\"" + ReaderCadastro["NOM_UF"].ToString().Trim() + "\",";
                json = json + "\"postal_code\":\"" + TIRAACENTOS(ReaderCadastro["NOM_CEP"].ToString().Trim()) + "\",";
                json = json + "\"country_code\":\"" + "BR" + "\"";
                if (txtToken.Text.ToString().Trim() == "")
                {
                    json = json + "},";
                    json = json + "\"mcc\":\"" + ReaderCadastro["COD_ID_MCC"].ToString().Trim() + "\"}";
                }
                else
                {
                    json = json + "}}";
                }
            }
            
            try
            {
                if (ReaderCadastro["FLG_TIPO_PESSOA"].ToString().Trim() == "PJ")
                {
                    var jsonRetorno = "";
                    if (txtToken.Text.ToString().Trim() == "")
                    {
                        jsonRetorno = zoop.CadastrarVendedorPJ(json);
                        ClientScript.RegisterStartupScript(this.GetType(), "Sucesso", "alert('Dados do Estabelecimento PJ gravados com sucesso: " + jsonRetorno.ToString() + "!');", true);
                    }
                    else
                    {
                        jsonRetorno = zoop.AlterarVendedorPJ(json, txtToken.Text.ToString());
                        ClientScript.RegisterStartupScript(this.GetType(), "Sucesso", "alert('Dados do Estabelecimento PJ alterados com sucesso!');", true);
                    }
                    //txtAgencia.Text = jsonRetorno;
                }
                if (ReaderCadastro["FLG_TIPO_PESSOA"].ToString().Trim() == "PF")
                {
                    var jsonRetorno = "";
                    if (txtToken.Text.ToString().Trim() == "")
                    {
                        jsonRetorno = zoop.CadastrarVendedorPF(json);
                        ClientScript.RegisterStartupScript(this.GetType(), "Sucesso", "alert('Dados do Estabelecimento PF gravados com sucesso!');", true);
                    }
                    else
                    {
                        jsonRetorno = zoop.AlterarVendedorPF(json, txtToken.Text.ToString());
                        ClientScript.RegisterStartupScript(this.GetType(), "Sucesso", "alert('Dados do Estabelecimento PF alterados com sucesso!');", true);
                    }
                    //txtAgencia.Text = jsonRetorno;
                }

            }
            catch (WebException ex)
            {
               var resp = new StreamReader(ex.Response.GetResponseStream()).ReadToEnd();
               JObject o = JObject.Parse(resp);
               ClientScript.RegisterStartupScript(this.GetType(), "Erro", "alert('Ocorreu um Erro: " + o["error"]["status_code"].ToString() + " - Motivo: " + o["error"]["category"].ToString() + " - Descrição: " + o["error"]["message"].ToString() + "');", true);
            }
            ConsultaID();

        }
    }

    protected void btnEnviar_Click(object sender, EventArgs e)
    {

        CadastrarEstabelecimento();
    }

    private void ConsultaID()
    {
        var json = "";
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

            if (ReaderCadastro["FLG_TIPO_PESSOA"].ToString().Trim() == "PF")
            {
                json = zoop.ConsultaCadastroSeller(TIRAACENTOS(ReaderCadastro["NOM_CPF"].ToString().Trim()));
            }
            if (ReaderCadastro["FLG_TIPO_PESSOA"].ToString().Trim() == "PJ")
            {
                json = zoop.ConsultaCadastroSeller(TIRAACENTOS(ReaderCadastro["NOM_CNPJ"].ToString().Trim()));
            }
            
            try
            {
                JObject o = JObject.Parse(json);
                txtToken.Text = o["id"].ToString();
                txtStatus.Text = o["status"].ToString();
            }
            catch
            {
                try
                {
                    JObject o = JObject.Parse(json);
                    ClientScript.RegisterStartupScript(this.GetType(), "ErroEspecifico", "alert('Ocorreu um Erro: " + o["error"]["status_code"].ToString() + " - Motivo: " + o["error"]["category"].ToString() + " - Descrição: " + o["error"]["message"].ToString() + "');", true);
                }
                catch
                {
                    ClientScript.RegisterStartupScript(this.GetType(), "ErroGeral", "alert('Ocorreu um erro ao tentar localizar o estabelecimento! Verifique e tente novamente');", true);
                }
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
    protected void btnPlano_Click(object sender, EventArgs e)
    {
        CadastrarPlano();

    }
    private void CadastrarPlano()
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

            AssociarPlano.root dplano = new AssociarPlano.root()
            {
                customer = txtToken.Text.ToString().Trim(),
                plan = ReaderCadastro["COD_ID_PLANO_REFERENCIA"].ToString().Trim(),
                quantity = 1
            };

            string json = JsonConvert.SerializeObject(dplano);
            string jsonPlano = zoop.CadastrarPlanoVendedor(json);
            //txtDigitoConta.Text = txtDigitoConta.Text + ReaderCadastro["COD_ID_PLANO_REFERENCIA"].ToString().Trim();
            //txtDigitoConta.Text = txtDigitoConta.Text + jsonPlano;

            if (jsonPlano.ToString().Trim() != "")
            {
                try
                {
                    JObject oPlano = JObject.Parse(jsonPlano.ToString());
                    ClientScript.RegisterStartupScript(this.GetType(), "SucessoPlano", "alert('Dados do plano enviados com sucesso!');", true);
                }
                catch
                {
                    try
                    {
                        JObject oPlano = JObject.Parse(jsonPlano.ToString());
                        if (oPlano["error"]["status_code"].ToString() == "404")
                        {
                            ClientScript.RegisterStartupScript(this.GetType(), "ErroPlano", "alert('Ocorreu um erro ao tentar associar o plano! O plano não existe ou pode ter sido excluído');", true);
                        }
                    }
                    catch
                    {
                        ClientScript.RegisterStartupScript(this.GetType(), "ErroPlano", "alert('Ocorreu um erro ao tentar associar o plano!');", true);
                    }

                }
            }
        }
    }

    protected void btnContas_Click(object sender, EventArgs e)
    {

        CadastrarContas();

    }

    private void CadastrarContas()
    {

        string sIDTokenConta = "";
        string sIDConta = "";
        
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
            
            foreach (RepeaterItem item in rptConsultaContas.Items)
            {

                if (((TextBox)item.FindControl("txtTokenConta")).Text.ToString().Trim()=="")
                {

                    CriarContaBancaria.root dconta = new CriarContaBancaria.root()
                    {
                        holder_name = ReaderCadastro["NOM_RAZAOSOCIAL"].ToString().Trim(),
                        bank_code = Funcoes.ObterStringSemAcentosECaracteresEspeciais(((TextBox)item.FindControl("txtBanco")).Text.ToString().Trim().PadLeft(3, '0')),
                        routing_number = Funcoes.ObterStringSemAcentosECaracteresEspeciais(((TextBox)item.FindControl("txtAgencia")).Text.ToString().Trim()),
                        account_number = Funcoes.ObterStringSemAcentosECaracteresEspeciais(((TextBox)item.FindControl("txtConta")).Text.ToString().Trim()) + Funcoes.ObterStringSemAcentosECaracteresEspeciais(((TextBox)item.FindControl("txtContaDigito")).Text.ToString().Trim()),
                        taxpayer_id = (ReaderCadastro["FLG_TIPO_PESSOA"].ToString() == "PF")? Funcoes.ObterStringSemAcentosECaracteresEspeciais(ReaderCadastro["NOM_CPF"].ToString().Trim()) : null,
                        ein = (ReaderCadastro["FLG_TIPO_PESSOA"].ToString() == "PJ")? Funcoes.ObterStringSemAcentosECaracteresEspeciais(ReaderCadastro["NOM_CNPJ"].ToString().Trim()) : null,
                        type =(((TextBox)item.FindControl("txtTipo")).Text.ToString().Trim() == "C")? "checking" : "savings"
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
                        AssociarContaBancaria.root dassociarconta = new AssociarContaBancaria.root()
                        {
                            customer = txtToken.Text.ToString().Trim(),
                            token = sIDTokenConta.ToString().Trim()
                        };

                        json = JsonConvert.SerializeObject(dassociarconta);
                        string jsonAssociarConta = zoop.AssociarContaVendedor(json);

                        if (jsonAssociarConta.ToString().Trim() != "")
                        {
                            try
                            {
                                JObject o = JObject.Parse(jsonConta);
                                sIDTokenConta = o["id"].ToString();
                            }
                            catch
                            {
                                ClientScript.RegisterStartupScript(this.GetType(), "ErroAssociar", "alert('Ocorreu um erro ao tentar associar a conta bancária ao estabelecimento!');", true);
                            }

                        }
                    
                    }
                    // Atualiza os dados da conta bancária
                    SqlConnection connInsCons = new SqlConnection(Funcoes.conexao());
                    connInsCons.Open();
                    SqlCommand cmdInsCons = new SqlCommand("dbo.stp_pessoas_fj_contas_ins", connInsCons);
                    cmdInsCons.CommandType = CommandType.StoredProcedure;
                    cmdInsCons.Parameters.Add("@flg_operacao", SqlDbType.Char).Value = 'T';
                    cmdInsCons.Parameters.Add("@COD_ID_PESSOA_LICENCIADO", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["LICENCIADO"].ToString());
                    cmdInsCons.Parameters.Add("@COD_ID", SqlDbType.Int).Value = Funcoes.strToInt(((TextBox)item.FindControl("txtid")).Text.ToString().Trim());
                    cmdInsCons.Parameters.Add("@NOM_TOKEN", SqlDbType.VarChar).Value = txtTokenConta.Text.ToString().Trim();
                    cmdInsCons.Parameters.Add("@NUM_ID_CONTA", SqlDbType.VarChar).Value = sIDConta.ToString().Trim();
                    cmdInsCons.ExecuteNonQuery();
                    connInsCons.Close();
                    connInsCons.Dispose();
                }
            }
            //txtResposta.Text = json.ToString();//responseText; //
        }
    }


    protected void btnEnviarTodos_Click(object sender, EventArgs e)
    {
        CadastrarEstabelecimento();
        CadastrarPlano();
        CadastrarContas();
        GravarDadosEstabelecimento();
    }
    protected void btnImportarContas_Click(object sender, EventArgs e)
    {
        if (txtToken.Text.ToString().Trim() != "")
        {
            var json = zoop.DetalhesVendedorContas(txtToken.Text.ToString());

            if (json.ToString().Trim() != "")
            {

                JObject oContas = JObject.Parse(json);

                if (oContas["items"].Count() > 0)
                {
                    for (int i = 0; i < oContas["items"].Count(); i++)
                    {

                        // Inserção de dados do Recebiveis

                        SqlConnection connInsConsContas = new SqlConnection(Funcoes.conexao());
                        connInsConsContas.Open();
                        SqlCommand cmdInsConsContas = new SqlCommand("dbo.stp_pessoas_fj_contas_ins", connInsConsContas);
                        cmdInsConsContas.CommandType = CommandType.StoredProcedure;
                        cmdInsConsContas.Parameters.Add("@flg_operacao", SqlDbType.Char).Value = 'I';

                        cmdInsConsContas.Parameters.Add("@COD_ID_PESSOA_LICENCIADO", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["LICENCIADO"].ToString());
                        cmdInsConsContas.Parameters.Add("@COD_ID_PESSOAS_FJ", SqlDbType.Int).Value = Funcoes.strToInt(sid_id.ToString());
                        cmdInsConsContas.Parameters.Add("@NOM_TOKEN", SqlDbType.VarChar).Value = oContas["items"][i]["id"].ToString();
                        cmdInsConsContas.Parameters.Add("@NUM_ID_CONTA", SqlDbType.VarChar).Value = oContas["items"][i]["id"].ToString();

                        cmdInsConsContas.Parameters.Add("@NOM_CODIGO_BANCO", SqlDbType.VarChar).Value = oContas["items"][i]["bank_code"].ToString();
                        cmdInsConsContas.Parameters.Add("@NOM_TIPO_BANCO", SqlDbType.VarChar).Value = (oContas["items"][i]["type"].ToString().Trim() == "Checking") ? "C" : "P";

                        cmdInsConsContas.Parameters.Add("@NOM_NUMERO_AGENCIA_BANCO", SqlDbType.VarChar).Value = oContas["items"][i]["routing_number"].ToString();
                        cmdInsConsContas.Parameters.Add("@NOM_NUMERO_DIGITO_AGENCIA_BANCO", SqlDbType.VarChar).Value = "";
                        cmdInsConsContas.Parameters.Add("@NOM_NUMERO_CONTA_BANCO", SqlDbType.VarChar).Value = oContas["items"][i]["account_number"].ToString();
                        cmdInsConsContas.Parameters.Add("@NOM_NUMERO_DIGITO_CONTA_BANCO", SqlDbType.VarChar).Value = "";

                        cmdInsConsContas.Parameters.Add("@FLG_PADRAO", SqlDbType.Char).Value = "N";
                        cmdInsConsContas.ExecuteNonQuery();
                        connInsConsContas.Close();
                        connInsConsContas.Dispose();

                        ClientScript.RegisterStartupScript(this.GetType(),"Sucesso", "alert('Importação Realizada com sucesso!');", true);
                        ConsultaGeral();

                    }
                }
            }
        }
    }
}