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


public partial class cad_estabelecimentos_onboarding : System.Web.UI.Page
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
            // Verificar Integrações
            CarregaAdquirentes();
            CarregaPlanos();
            

            if (Funcoes.strToInt(sid_id) != 0)
            {
                ConsultaFicha();
                ConsultaGeral();
                ddlAdquirentes_SelectedIndexChanged(null, null);
                if (ddlAdquirentes.SelectedValue.ToString().Trim() == "Z")
                {
                    btnAtualizarPlano.Visible = true;
                }
            }

        }
    }

    private void CarregaAdquirentes()
    {
        SqlConnection myAdquirentes = new SqlConnection(Funcoes.conexao());
        myAdquirentes.Open();
        SqlCommand cmdAdquirentes = new SqlCommand("dbo.stp_pessoas_fj_integracoes_ins", myAdquirentes);
        cmdAdquirentes.CommandType = CommandType.StoredProcedure;
        if ((HttpContext.Current.Session["TIPO"].ToString() == "L"))
        {
            cmdAdquirentes.Parameters.Add("@flg_operacao", SqlDbType.VarChar).Value = "G";
        }
        else
        {
            cmdAdquirentes.Parameters.Add("@flg_operacao", SqlDbType.VarChar).Value = "M";
        }

        cmdAdquirentes.Parameters.Add("@COD_ID_PESSOA_LICENCIADO", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["LICENCIADO"].ToString());
        cmdAdquirentes.Parameters.Add("@FLG_ATIVO", SqlDbType.Char).Value = "S";
        cmdAdquirentes.Parameters.Add("@FLG_ACESSO", SqlDbType.Char).Value = "S";
        SqlDataAdapter drAdquirentes = new SqlDataAdapter();
        drAdquirentes.SelectCommand = cmdAdquirentes;
        DataSet dsAdquirentes = new DataSet();
        drAdquirentes.Fill(dsAdquirentes, "PESSOAS_FJ_INTEGRACOES");

        ddlAdquirentes.DataTextField = "NOM_FANTASIA_INTEGRACAO";
        ddlAdquirentes.DataValueField = "FLG_INTEGRACAO";
        ddlAdquirentes.DataSource = dsAdquirentes.Tables["PESSOAS_FJ_INTEGRACOES"].DefaultView;
        ddlAdquirentes.DataBind();
    }

    private void CarregaPlanos()
    {
        SqlConnection myPlano = new SqlConnection(Funcoes.conexao());
        myPlano.Open();
        SqlCommand cmdPlano = new SqlCommand("dbo.stp_planos_ins", myPlano);
        cmdPlano.CommandType = CommandType.StoredProcedure;
        cmdPlano.Parameters.Add("@flg_operacao", SqlDbType.VarChar).Value = "M";
        cmdPlano.Parameters.Add("@FLG_ATIVO", SqlDbType.VarChar).Value = "S";
        cmdPlano.Parameters.Add("@COD_ID_PESSOA_LICENCIADO", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["LICENCIADO"].ToString());
        if (HttpContext.Current.Session["TIPO"].ToString() != "A")
        {
            cmdPlano.Parameters.Add("@COD_ID_PESSOAS_FJ", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["PESSOA"].ToString());
        }
        cmdPlano.Parameters.Add("@FLG_MODELO_PLANO", SqlDbType.Char).Value = ddlAdquirentes.SelectedValue.ToString();

        SqlDataAdapter drPlano = new SqlDataAdapter();
        drPlano.SelectCommand = cmdPlano;
        DataSet dsPlano = new DataSet();
        drPlano.Fill(dsPlano, "PLANOS");
        ddlPlano.DataTextField = "NOM_TITULO_PLANO";
        ddlPlano.DataValueField = "COD_ID";
        ddlPlano.DataSource = dsPlano.Tables["PLANOS"].DefaultView;
        ddlPlano.DataBind();
        ddlPlano.Items.Insert(0, new ListItem("", "0"));
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

            txtRepresentante.Text = ReaderCadastro["NOM_RAZAOSOCIAL_REPRESENTANTE"].ToString();
            txtTipoRepresentante.Text = ReaderCadastro["FLG_TIPO_PESSOA_REPRESENTANTE"].ToString();
            txtDocumentoRepresentante.Text = ReaderCadastro["NOM_DOCUMENTO_REPRESENTANTE"].ToString();
            txtStatusRepresentante.Text = ReaderCadastro["FLG_STATUS_CAPPTA_REPRESENTANTE"].ToString();

            txtMarketplace.Text = ReaderCadastro["NOM_RAZAOSOCIAL_MARKETPLACE"].ToString();
            txtTipoMarketplace.Text = ReaderCadastro["FLG_TIPO_PESSOA_MARKETPLACE"].ToString();
            txtDocumentoMarketplace.Text = ReaderCadastro["NOM_DOCUMENTO_MARKETPLACE"].ToString();
            txtStatusMarketplace.Text = ReaderCadastro["FLG_STATUS_CAPPTA_MARKETPLACE"].ToString();

        }

    }



    private void ConsultaAsaas()
    {

        SqlConnection mySelCadastro = new SqlConnection(Funcoes.conexao());
        mySelCadastro.Open();
        SqlCommand cmdSelCadastro = new SqlCommand("dbo.stp_pessoas_fj_baas_ins", mySelCadastro);
        cmdSelCadastro.CommandType = CommandType.StoredProcedure;
        cmdSelCadastro.Parameters.Add("@flg_operacao", SqlDbType.Char).Value = "S";
        cmdSelCadastro.Parameters.Add("@COD_ID_PESSOAS_FJ", SqlDbType.Int).Value = Funcoes.strToInt(sid_id.ToString());
        cmdSelCadastro.Parameters.Add("@COD_ID_PESSOA_LICENCIADO", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["LICENCIADO"].ToString());

        cmdSelCadastro.Parameters.Add("@FLG_ORIGEM", SqlDbType.Char).Value = "A";

        SqlDataReader ReaderCadastro = cmdSelCadastro.ExecuteReader();
        while (ReaderCadastro.Read())
        {
            ddlBaaS.SelectedValue = ReaderCadastro["FLG_BAAS"].ToString();
            txtTokenAsaas.Text = Funcoes.Encrypt(ReaderCadastro["NUM_TOKEN_BAAS"].ToString());
            txtStatusAsaas.Text = ReaderCadastro["FLG_STATUS_BAAS"].ToString();
            txtNomeStatus.Text = ReaderCadastro["NOM_FLG_STATUS_BAAS"].ToString();
            txtResposta.Text = ReaderCadastro["DES_JSON_BAAS"].ToString();

            if ((ddlBaaS.SelectedValue.ToString().Trim() == "S") && Funcoes.Decrypt(txtTokenAsaas.Text.ToString().Trim()) != "")
            {
                txtTokenAsaas.Enabled = false;
            }

            if ((ddlBaaS.SelectedValue.ToString().Trim() == "S") && (Funcoes.Decrypt(txtTokenAsaas.Text.ToString().Trim()) != "") && (txtIDConta.Text.ToString().Trim() != ""))
            {
                txtIDConta.Text = ReaderCadastro["NUM_ID_CONTA_BAAS"].ToString();
                txtAgencia.Text = ReaderCadastro["NUM_AGENCIA_BAAS"].ToString();
                txtConta.Text = ReaderCadastro["NUM_CONTA_BAAS"].ToString();
                txtDigitoConta.Text = ReaderCadastro["NUM_DIGITO_CONTA_BAAS"].ToString();
                txtWalletID.Text = ReaderCadastro["NUM_WALLETID_BAAS"].ToString();
            }
            else
            {
                if (Funcoes.Decrypt(txtTokenAsaas.Text.ToString().Trim()) != "")
                {
                    // Consultar conta pelo email
                    string ConsultaConta = asaas.ListarSubcontasCNPJCPF(TIRAACENTOS(txtDocumento.Text.ToString()));
                    txtResposta.Text = ConsultaConta.ToString();

                    if (ConsultaConta.ToString().Trim() != "")
                    {
                        try
                        {
                            JObject oConta = JObject.Parse(ConsultaConta.ToString());
                            txtIDConta.Text = oConta["data"][0]["id"].ToString();
                            txtAgencia.Text = oConta["data"][0]["accountNumber"]["agency"].ToString();
                            txtConta.Text = oConta["data"][0]["accountNumber"]["account"].ToString();
                            txtDigitoConta.Text = oConta["data"][0]["accountNumber"]["accountDigit"].ToString();
                            txtWalletID.Text = oConta["data"][0]["walletId"].ToString();
                        }
                        catch
                        {
                        }
                    }
                    else
                    {
                        txtIDConta.Text = ReaderCadastro["NUM_ID_CONTA_BAAS"].ToString();
                        txtAgencia.Text = ReaderCadastro["NUM_AGENCIA_BAAS"].ToString();
                        txtConta.Text = ReaderCadastro["NUM_CONTA_BAAS"].ToString();
                        txtDigitoConta.Text = ReaderCadastro["NUM_DIGITO_CONTA_BAAS"].ToString();
                        txtWalletID.Text = ReaderCadastro["NUM_WALLETID_BAAS"].ToString();


                    }
                }
            }

            txtChavePix.Text = ReaderCadastro["NUM_CHAVE_PIX"].ToString();

            if (Funcoes.Decrypt(txtTokenAsaas.Text.ToString().Trim()) != "")
            {
                string ConsultaStatus = asaas.ListarStatus(Funcoes.Decrypt(txtTokenAsaas.Text.ToString()));
                txtResposta.Text = ConsultaStatus.ToString();
                try
                {
                    JObject oStatus = JObject.Parse(ConsultaStatus.ToString());
                    txtStatusAsaas.Text = oStatus["general"].ToString();
                }
                catch
                {
                    txtStatusAsaas.Text = "";
                }
            }

            txtIDConta.Enabled = false;
            txtWalletID.Enabled = false;
            txtAgencia.Enabled = false;
            txtConta.Enabled = false;
            txtDigitoConta.Enabled = false;
            txtNomeStatus.Enabled = false;
            txtChavePix.Enabled = false;

        }

    }
    private void ConsultaZoop()
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
            ddlZoop.SelectedValue = ReaderCadastro["COD_ID_ZOOP_HABILITADO"].ToString();

            txtToken.Text = ReaderCadastro["NUM_TOKEN"].ToString();
            if (ReaderCadastro["NUM_TOKEN"].ToString().Trim() != "") { dcZoopSim.Visible = true; dcZoopNao.Visible = false; } else { dcZoopSim.Visible = false; dcZoopNao.Visible = true; }

            txtStatus.Text = ReaderCadastro["FLG_STATUS_ZOOP"].ToString();
            txtStatusZoop.Text = ReaderCadastro["NOM_FLG_STATUS_ZOOP"].ToString();
            sgZoopSim.Visible = false;
            sgZoopNao.Visible = false;
            sgZoopTalvez.Visible = false;

            if (ReaderCadastro["NOM_FLG_STATUS_ZOOP"].ToString().Trim() == "Ativo") { sgZoopSim.Visible = true; }
            if (ReaderCadastro["NOM_FLG_STATUS_ZOOP"].ToString().Trim() == "Pendente") { sgZoopTalvez.Visible = true; }
            if (ReaderCadastro["NOM_FLG_STATUS_ZOOP"].ToString().Trim() == "Inativo") { sgZoopNao.Visible = true; }

            ddlPlano.SelectedValue = ReaderCadastro["COD_ID_PLANO_ZOOP"].ToString();

            ddlSaque.SelectedValue = ReaderCadastro["FLG_SAQUE_AUTOMATICA"].ToString();
            ddlPeriodicidade.SelectedValue = ReaderCadastro["FLG_PERIODICIDADE_SAQUE"].ToString();
            txtValorMinimo.Text = ReaderCadastro["NUM_VALOR_MINIMO_SAQUE"].ToString();

            // Le dados do Plano associado
            if (ReaderCadastro["NUM_TOKEN"].ToString().Trim() != "")
            {
                planZoopSim.Visible = false;
                planZoopNao.Visible = false;

                string jsonConsultaPlano = zoop.ConsultarPlanoVendedor(ReaderCadastro["NUM_TOKEN"].ToString().Trim());

                if (jsonConsultaPlano.ToString().Trim() != "")
                {
                    try
                    {
                        JObject oPlano = JObject.Parse(jsonConsultaPlano.ToString());
                        if (oPlano["items"][0]["plan"]["id"].ToString().Trim() != "")
                        {
                            planZoopSim.Visible = true;
                        }
                        else
                        {
                            planZoopNao.Visible = true;
                        }
                    }
                    catch
                    {
                        planZoopNao.Visible = true;
                    }
                }
            }
        }

        // Verifica os dados bancários
        SqlConnection mySelCadastroConta = new SqlConnection(Funcoes.conexao());
        mySelCadastroConta.Open();
        SqlCommand cmdSelCadastroConta = new SqlCommand("dbo.stp_pessoas_fj_contas_ins", mySelCadastroConta);
        cmdSelCadastroConta.CommandType = CommandType.StoredProcedure;
        cmdSelCadastroConta.Parameters.Add("@flg_operacao", SqlDbType.Char).Value = "X";
        cmdSelCadastroConta.Parameters.Add("@COD_ID_PESSOAS_FJ", SqlDbType.Int).Value = Funcoes.strToInt(sid_id.ToString());
        cmdSelCadastroConta.Parameters.Add("@COD_ID_PESSOA_LICENCIADO", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["LICENCIADO"].ToString());
        SqlDataReader ReaderCadastroConta = cmdSelCadastroConta.ExecuteReader();
        while (ReaderCadastroConta.Read())
        {
            if (ReaderCadastroConta["NOM_TOKEN"].ToString().Trim() != "") { dbZoopSim.Visible = true; dbZoopNao.Visible = false; } else { dbZoopSim.Visible = false; dbZoopNao.Visible = true; }
        }
    }


    private void ConsultaErp()
    {

        SqlConnection mySelCadastro = new SqlConnection(Funcoes.conexao());
        mySelCadastro.Open();
        SqlCommand cmdSelCadastro = new SqlCommand("dbo.stp_pessoas_fj_erp_ins", mySelCadastro);
        cmdSelCadastro.CommandType = CommandType.StoredProcedure;
        cmdSelCadastro.Parameters.Add("@flg_operacao", SqlDbType.Char).Value = "S";
        cmdSelCadastro.Parameters.Add("@COD_ID_PESSOAS_FJ", SqlDbType.Int).Value = Funcoes.strToInt(sid_id.ToString());
        cmdSelCadastro.Parameters.Add("@COD_ID_PESSOA_LICENCIADO", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["LICENCIADO"].ToString());
        SqlDataReader ReaderCadastro = cmdSelCadastro.ExecuteReader();
        while (ReaderCadastro.Read())
        {
            ddlErp.SelectedValue = ReaderCadastro["FLG_ERP"].ToString();

            txtTokenErp.Text = ReaderCadastro["NOM_TOKEN"].ToString();
            txtUsuarioErp.Text = ReaderCadastro["NOM_USUARIO"].ToString();
            txtAppErp.Text = ReaderCadastro["NOM_APP"].ToString();
        }
    }
    

    private void ConsultaPagseguro()
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

            ddlPagseguro.SelectedValue = ReaderCadastro["COD_ID_PAGSEGURO_HABILITADO"].ToString();

            txtIDPagseguro.Text = ReaderCadastro["COD_ID_PAGSEGURO"].ToString();
            txtTokenPagseguro.Text = ReaderCadastro["NOM_PAGSEGURO_TOKEN"].ToString();
            txtEmailPagseguro.Text = ReaderCadastro["NOM_PAGSEGURO_EMAIL"].ToString();
            txtEmailPagseguro.Text = ReaderCadastro["NOM_PAGSEGURO_ATIVACAO"].ToString();

            txtCodigoAtivacao.Text = ReaderCadastro["COD_ID_PAGSEGURO_ATIVACAO"].ToString();
            ddlVerificadaAtivada.SelectedValue = ReaderCadastro["FLG_PAGSEGURO_VERIFICADA_ATIVADA"].ToString();

            //ddlInstituicaoFinanceira.SelectedValue = ReaderCadastro["NOM_CODIGO_BANCO"].ToString();
            //ddlTipoConta.SelectedValue = ReaderCadastro["NOM_TIPO_BANCO"].ToString();
            //txtAgencia.Text = ReaderCadastro["NOM_NUMERO_AGENCIA_BANCO"].ToString();
            //txtDigitoAgencia.Text = ReaderCadastro["NOM_NUMERO_DIGITO_AGENCIA_BANCO"].ToString();
            //txtConta.Text = ReaderCadastro["NOM_NUMERO_CONTA_BANCO"].ToString();
            //txtDigitoConta.Text = ReaderCadastro["NOM_NUMERO_DIGITO_CONTA_BANCO"].ToString();

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
    
    protected void btnFechar_Click(object sender, EventArgs e)
    {
        ClientScript.RegisterStartupScript(this.GetType(),"FecharOnboarding", "opener.PostBackOnMainPage(); window.close(); ", true);

    }


    private void ConsultaGeral()
    {

    }


    // -----------------------------------------------------------------------------------------------------------------------------------------------------------------
    // SETOR BAAS 
    // -----------------------------------------------------------------------------------------------------------------------------------------------------------------



    // -----------------------------------------------------------------------------------------------------------------------------------------------------------------
    // SETOR CAPPTA 
    // -----------------------------------------------------------------------------------------------------------------------------------------------------------------

    private void ConsultaCappta()
    {
        try
        {
            ConsultaTabelasCapta();

            SqlConnection mySelCadastro = new SqlConnection(Funcoes.conexao());
            mySelCadastro.Open();
            SqlCommand cmdSelCadastro = new SqlCommand("dbo.stp_pessoas_fj_cappta_ins", mySelCadastro);
            cmdSelCadastro.CommandType = CommandType.StoredProcedure;
            cmdSelCadastro.Parameters.Add("@flg_operacao", SqlDbType.Char).Value = "S";
            cmdSelCadastro.Parameters.Add("@COD_ID_PESSOA_LICENCIADO", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["LICENCIADO"].ToString());
            cmdSelCadastro.Parameters.Add("@COD_ID_PESSOAS_FJ", SqlDbType.Int).Value = Funcoes.strToInt(sid_id.ToString());
            SqlDataReader ReaderCadastro = cmdSelCadastro.ExecuteReader();
            while (ReaderCadastro.Read())
            {
                ddlCappta.SelectedValue = ReaderCadastro["FLG_CAPPTA"].ToString();
                ddlNatureza.SelectedValue = ReaderCadastro["COD_ID_NATUREZA_CAPPTA"].ToString();
                txtTokenCappta.Text = ReaderCadastro["NUM_TOKEN_CAPPTA"].ToString();
                txtStatusCappta.Text = ReaderCadastro["FLG_STATUS_CAPPTA"].ToString();
                txtNomeStatusCappta.Text = ReaderCadastro["FLG_STATUS_CAPPTA"].ToString();
                ddlPlano.SelectedValue = ReaderCadastro["COD_ID_PLANO"].ToString();
                txtPlanoReferencia.Text = ReaderCadastro["COD_ID_PLANO_CAPPTA"].ToString().Trim();

                if (ReaderCadastro["DES_JSON_CAPPTA"].ToString().Trim() != "")
                {
                    try
                    {
                        JObject oEstabelecimento = JObject.Parse(ReaderCadastro["DES_JSON_CAPPTA"].ToString());

                        if (Funcoes.strToInt(ReaderCadastro["COD_ID_PLANO_CAPPTA"].ToString().Trim()) <= 0)
                        {
                            txtPlanoReferencia.Text = oEstabelecimento["plans"][0]["id"].ToString();
                        }
                        else
                        {
                            txtPlanoReferencia.Text = ReaderCadastro["COD_ID_PLANO_CAPPTA"].ToString().Trim();
                        }

                        if (Funcoes.strToInt(ddlPlano.SelectedValue.ToString().Trim()) <= 0)
                        {
                            SqlConnection mySelCadastroPlanos = new SqlConnection(Funcoes.conexao());
                            mySelCadastroPlanos.Open();
                            SqlCommand cmdSelCadastroPlanos = new SqlCommand("dbo.stp_planos_ins", mySelCadastroPlanos);
                            cmdSelCadastroPlanos.CommandType = CommandType.StoredProcedure;
                            cmdSelCadastroPlanos.Parameters.Add("@flg_operacao", SqlDbType.Char).Value = "H";
                            cmdSelCadastroPlanos.Parameters.Add("@COD_ID_PESSOA_LICENCIADO", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["LICENCIADO"].ToString());
                            cmdSelCadastroPlanos.Parameters.Add("@COD_ID_PESSOAS_FJ", SqlDbType.Int).Value = 0;
                            cmdSelCadastroPlanos.Parameters.Add("@FLG_MODELO_PLANO", SqlDbType.Char).Value = "C";
                            cmdSelCadastroPlanos.Parameters.Add("@NOM_REFERENCIA_PLANO", SqlDbType.VarChar).Value = txtPlanoReferencia.Text.ToString();
                            SqlDataReader ReaderCadastroPlanos = cmdSelCadastroPlanos.ExecuteReader();
                            while (ReaderCadastroPlanos.Read())
                            {
                                ddlPlano.SelectedValue = ReaderCadastroPlanos["COD_ID"].ToString();
                            }
                        }

                        if (Funcoes.strToInt(ddlPlano.SelectedValue.ToString().Trim()) > 0)
                        {
                            lkbImportarPlano.Visible = false;
                        }
                        else
                        {
                            lkbImportarPlano.Visible = true;
                            lblPlanoReferencia.Text = "(OBS: O plano de taxas não foi localizado na base de dados! AÇÃO: Clique no botão <b>IMPORTAR</b>)";
                        }

                    }
                    catch
                    {
                        lkbImportarPlano.Visible = false;
                    }
                }



            }
        }
        catch
        {
            ClientScript.RegisterStartupScript(this.GetType(), "ErroCarregaDadosCappta", "alert('Ocorreu um erro ao tentar carregar os dados da adquirente! Verifique e tente novamente!');", true);
        }
    }


    private void ConsultaTabelasCapta()
    {
        try
        {
        
        string jsonOpcoesCadastroNatureza = hubcappta.ListarOpcoesCadastro();
        JObject oNatureza = JObject.Parse(jsonOpcoesCadastroNatureza);

        if (oNatureza["legalNature"].Count() > 0)
        {
            for (int i = 0; i < oNatureza["legalNature"].Count(); i++)
            {
                ddlNatureza.Items.Insert(0, new ListItem(oNatureza["legalNature"][i]["Description"].ToString(), oNatureza["legalNature"][i]["Id"].ToString()));
            }
        }
        ddlNatureza.Items.Insert(0, new ListItem("", "0"));
        }
        catch
        {
            ClientScript.RegisterStartupScript(this.GetType(), "ErroTabelaCappta", "alert('Ocorreu um erro ao tentar carregar as tabelas da adquirente! Verifique e tente novamente!');", true);
        }

    }

    
    protected void btnSalvarCappta_Click(object sender, EventArgs e)
    {


    }


    // -----------------------------------------------------------------------------------------------------------------------------------------------------------------
    // SETOR ZOOP 
    // -----------------------------------------------------------------------------------------------------------------------------------------------------------------



    // -----------------------------------------------------------------------------------------------------------------------------------------------------------------
    // SETOR PAGSEGURO 
    // -----------------------------------------------------------------------------------------------------------------------------------------------------------------



    // -----------------------------------------------------------------------------------------------------------------------------------------------------------------
    // SETOR CONTAS BANCÁRIAS
    // -----------------------------------------------------------------------------------------------------------------------------------------------------------------


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

    protected void ddlAdquirentes_SelectedIndexChanged(object sender, EventArgs e)
    {
        btnAtualizarPlano.Visible = false;
        CarregaPlanos();

        divAdquirenteCappta.Visible = false;
        divAdquirenteZoop.Visible = false;
        divAdquirenteAsaas.Visible = false;
        divAdquirentePagseguro.Visible = false;
        divAdquirenteErp.Visible = false;

        if (ddlAdquirentes.SelectedValue.ToString() == "C")
        {
            divAdquirenteCappta.Visible = true;
            ConsultaCappta();
        }
        if (ddlAdquirentes.SelectedValue.ToString() == "Z")
        {
            divAdquirenteZoop.Visible = true;
            btnAtualizarPlano.Visible = true;
            ConsultaZoop();
        }
        if (ddlAdquirentes.SelectedValue.ToString() == "P")
        {
            divAdquirentePagseguro.Visible = true;
            ConsultaPagseguro();
        }
        if (ddlAdquirentes.SelectedValue.ToString() == "A")
        {
            divAdquirenteAsaas.Visible = true;
            ConsultaAsaas();
        }

        if (ddlAdquirentes.SelectedValue.ToString() == "E")
        {
            divAdquirenteErp.Visible = true;
            ConsultaErp();
        }
    
    }
    protected void btnSalvar_Click(object sender, EventArgs e)
    {

        int iContas = 0;
        SqlConnection mySelCadastroContas = new SqlConnection(Funcoes.conexao());
        mySelCadastroContas.Open();
        SqlCommand cmdSelCadastroContas = new SqlCommand("dbo.stp_pessoas_fj_contas_ins", mySelCadastroContas);
        cmdSelCadastroContas.CommandType = CommandType.StoredProcedure;
        cmdSelCadastroContas.Parameters.Add("@flg_operacao", SqlDbType.Char).Value = "C";
        cmdSelCadastroContas.Parameters.Add("@COD_ID_PESSOAS_FJ", SqlDbType.Int).Value = Funcoes.strToInt(sid_id.ToString());
        cmdSelCadastroContas.Parameters.Add("@COD_ID_PESSOA_LICENCIADO", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["LICENCIADO"].ToString());
        SqlDataReader ReaderCadastroContas = cmdSelCadastroContas.ExecuteReader();
        while (ReaderCadastroContas.Read())
        {
            iContas = iContas + 1;
        }

        if ((ddlAdquirentes.SelectedValue.ToString().Trim() != "A") && (ddlAdquirentes.SelectedValue.ToString().Trim() != "E") && (ddlAdquirentes.SelectedValue.ToString().Trim() != "P"))
        {
            if (iContas > 0)
            {
                if (Funcoes.strToInt(ddlPlano.SelectedValue.ToString().Trim()) > 0)
                {
                    if (ddlAdquirentes.SelectedValue.ToString() == "C")
                    {
                        GravarDadosCappta();
                    }
                    if (ddlAdquirentes.SelectedValue.ToString() == "Z")
                    {
                        GravarDadosZoop();
                    }
                }
                else
                {
                    ClientScript.RegisterStartupScript(this.GetType(), "ErroPlanosInexistentes", "alert('Não foi selecionado nenhum plano! Verifique e tente novamente!');", true);
                }
            }
            else
            {
                ClientScript.RegisterStartupScript(this.GetType(), "ErroContasInexistentes", "alert('Não existem contas bancárias cadastradas! Verifique e tente novamente!');", true);

                if (ddlAdquirentes.SelectedValue.ToString() == "C")
                {
                    SomenteGravarDadosCappta();
                }

            }
        }
        else
        {
            if (ddlAdquirentes.SelectedValue.ToString() == "P")
            {
                if (Funcoes.strToInt(ddlPlano.SelectedValue.ToString().Trim()) > 0)
                {
                    GravarDadosPagseguro();
                }
                else
                {
                    ClientScript.RegisterStartupScript(this.GetType(), "ErroPlanosInexistentes", "alert('Não foi selecionado nenhum plano! Verifique e tente novamente!');", true);
                }

            }
            if (ddlAdquirentes.SelectedValue.ToString() == "A")
            {
                GravarDadosAsaas();
            }
            if (ddlAdquirentes.SelectedValue.ToString() == "E")
            {
                GravarDadosErp();
            }
        }

    }

    private void SomenteGravarDadosCappta()
    {
        SqlConnection connInsCons = new SqlConnection(Funcoes.conexao());
        connInsCons.Open();
        SqlCommand cmdInsCons = new SqlCommand("dbo.stp_pessoas_fj_cappta_ins", connInsCons);
        cmdInsCons.CommandType = CommandType.StoredProcedure;
        cmdInsCons.Parameters.Add("@flg_operacao", SqlDbType.Char).Value = 'I';
        cmdInsCons.Parameters.Add("@COD_ID_PESSOA_LICENCIADO", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["LICENCIADO"].ToString());
        cmdInsCons.Parameters.Add("@COD_ID_PESSOAS_FJ", SqlDbType.Int).Value = Funcoes.strToInt(sid_id.ToString());

        cmdInsCons.Parameters.Add("@NUM_TOKEN_CAPPTA", SqlDbType.VarChar).Value = txtTokenCappta.Text.ToString();
        cmdInsCons.Parameters.Add("@FLG_STATUS_CAPPTA", SqlDbType.VarChar).Value = txtStatusCappta.Text.ToString();
        cmdInsCons.Parameters.Add("@FLG_CAPPTA", SqlDbType.Char).Value = ddlCappta.SelectedValue.ToString();
        
        //cmdInsCons.Parameters.Add("@DES_JSON_CAPPTA", SqlDbType.VarChar).Value = txtResposta.Text.ToString();
        
        cmdInsCons.Parameters.Add("@COD_ID_NATUREZA_CAPPTA", SqlDbType.Int).Value = ddlNatureza.SelectedValue.ToString();
        cmdInsCons.Parameters.Add("@COD_ID_PLANO", SqlDbType.Int).Value = Funcoes.strToInt(ddlPlano.SelectedValue.ToString());

        cmdInsCons.ExecuteNonQuery();
        connInsCons.Close();
        connInsCons.Dispose();
    }


    private void GravarDadosErp()
    {
        SqlConnection connInsCons = new SqlConnection(Funcoes.conexao());
        connInsCons.Open();
        SqlCommand cmdInsCons = new SqlCommand("dbo.stp_pessoas_fj_erp_ins", connInsCons);
        cmdInsCons.CommandType = CommandType.StoredProcedure;
        cmdInsCons.Parameters.Add("@flg_operacao", SqlDbType.Char).Value = 'I';
        cmdInsCons.Parameters.Add("@COD_ID_PESSOA_LICENCIADO", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["LICENCIADO"].ToString());
        cmdInsCons.Parameters.Add("@COD_ID_PESSOAS_FJ", SqlDbType.Int).Value = Funcoes.strToInt(sid_id.ToString());

        cmdInsCons.Parameters.Add("@NOM_TOKEN", SqlDbType.VarChar).Value = txtTokenErp.Text.ToString();
        cmdInsCons.Parameters.Add("@NOM_USUARIO", SqlDbType.VarChar).Value = txtUsuarioErp.Text.ToString();
        cmdInsCons.Parameters.Add("@FLG_ERP", SqlDbType.Char).Value = ddlErp.SelectedValue.ToString();
        cmdInsCons.Parameters.Add("@NOM_APP", SqlDbType.VarChar).Value = txtAppErp.Text.ToString();

        cmdInsCons.ExecuteNonQuery();
        connInsCons.Close();
        connInsCons.Dispose();

        ClientScript.RegisterStartupScript(this.GetType(),
            "Alerta", "alert('Dados gravados e enviados com sucesso');", true);
    }


    private void GravarDadosCappta()
    {
        txtStatusMarketplace.Text = "ATIVO";
        if (txtTipoMarketplace.Text.ToString().Trim() == "PF")
        {
            ClientScript.RegisterStartupScript(this.GetType(),
                "ErroTipoPessoa", "alert('O Marketplace precisa ser uma pessoa jurídica!');", true);
        }
        else
        {
            if (txtDocumentoMarketplace.Text.ToString().Trim() == "")
            {
                ClientScript.RegisterStartupScript(this.GetType(),
                    "ErroMarketplace", "alert('É obrigatório especificar um marketplace para o estabelecimento!');", true);
            }
            else
            {
                if (txtStatusMarketplace.Text.ToString().Trim() == "")
                {
                    ClientScript.RegisterStartupScript(this.GetType(),
                        "ErroStatusMarketplace", "alert('O Marketplace especificado não encontra-se habilitado na rede cappta!');", true);
                }
                else
                {
                    string jsonLojista = "";
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
                        DadosLojista.Root dlojista = new DadosLojista.Root()
                        {
                            resellerDocument = TIRAACENTOS(ReaderCadastro["NOM_DOCUMENTO_MARKETPLACE"].ToString().Trim()),
                            merchant = new DadosLojista.Merchant()
                            {
                                document = (ReaderCadastro["FLG_TIPO_PESSOA"].ToString().Trim() == "PF") ? TIRAACENTOS(ReaderCadastro["NOM_CPF"].ToString().Trim()) : TIRAACENTOS(ReaderCadastro["NOM_CNPJ"].ToString().Trim()),
                                legalNatureId = Funcoes.strToInt(ddlNatureza.SelectedValue.ToString()),
                                mccId = Funcoes.strToInt(ReaderCadastro["COD_ID_MCC"].ToString()),
                                TpvExpected = (ReaderCadastro["FLG_TIPO_PESSOA"].ToString().Trim() == "PF") ? Funcoes.strToInt(ReaderCadastro["NUM_RENDA_MENSAL"].ToString().Trim()) : Funcoes.strToInt(ReaderCadastro["NUM_FATURAMENTO"].ToString().Trim()),
                                companyName = (ReaderCadastro["FLG_TIPO_PESSOA"].ToString().Trim() == "PF") ? ReaderCadastro["NOM_NOME"].ToString().Trim() + " " + ReaderCadastro["NOM_SOBRENOME"].ToString().Trim() : ReaderCadastro["NOM_RAZAOSOCIAL"].ToString().Trim(),
                                tradingName = (ReaderCadastro["FLG_TIPO_PESSOA"].ToString().Trim() == "PF") ? ReaderCadastro["NOM_NOME"].ToString().Trim() + " " + ReaderCadastro["NOM_SOBRENOME"].ToString().Trim() : ReaderCadastro["NOM_RAZAOSOCIAL"].ToString().Trim()
                            },
                            address = new DadosLojista.Address()
                            {
                                streetName = ReaderCadastro["NOM_ENDERECO"].ToString().Trim(),
                                houseNumber = ReaderCadastro["NOM_NUMERO"].ToString().Trim(),
                                complement = ReaderCadastro["NOM_COMPLEMENTO"].ToString().Trim(),
                                neighborhood = ReaderCadastro["NOM_BAIRRO"].ToString().Trim(),
                                city = ReaderCadastro["NOM_CIDADE"].ToString().Trim(),
                                postalCode = TIRAACENTOS(ReaderCadastro["NOM_CEP"].ToString().Trim()),
                                state = ReaderCadastro["NOM_UF"].ToString().Trim()
                            },
                            owner = new DadosLojista.Owner()
                            {
                                cpf = TIRAACENTOS(ReaderCadastro["NOM_CPF"].ToString().Trim()),
                                email = ReaderCadastro["NOM_EMAIL"].ToString().Trim(),
                                name = ReaderCadastro["NOM_NOME"].ToString().Trim() + " " + ReaderCadastro["NOM_SOBRENOME"].ToString().Trim(),
                                mobilePhone = TIRAACENTOS(ReaderCadastro["NOM_CELULAR"].ToString().Trim()),
                                phone = "0000000000"//TIRAACENTOS(ReaderCadastro["NUM_TELEFONE"].ToString().Trim()).Substring(1,10),
                            },
                            bankAccount = new DadosLojista.BankAccount()
                            {
                                accountType = (ReaderCadastro["NOM_TIPO_BANCO_PADRAO"].ToString().Trim() == "C") ? 1 : 2,
                                account = TIRAACENTOS(ReaderCadastro["NOM_NUMERO_CONTA_BANCO_PADRAO"].ToString().Trim()) + TIRAACENTOS(ReaderCadastro["NOM_NUMERO_DIGITO_CONTA_BANCO_PADRAO"].ToString().Trim()),
                                bankCode = TIRAACENTOS(ReaderCadastro["NOM_CODIGO_BANCO_PADRAO"].ToString().Trim()),
                                branch = TIRAACENTOS(ReaderCadastro["NOM_NUMERO_AGENCIA_BANCO_PADRAO"].ToString().Trim())
                            },
                            planId = 98505
                        };
                        jsonLojista = JsonConvert.SerializeObject(dlojista);
                    }
                    txtResposta.Visible = true;
                    txtResposta.Text = hubcappta.CadastrarLojista(jsonLojista);

                    SqlConnection connInsCons = new SqlConnection(Funcoes.conexao());
                    connInsCons.Open();
                    SqlCommand cmdInsCons = new SqlCommand("dbo.stp_pessoas_fj_cappta_ins", connInsCons);
                    cmdInsCons.CommandType = CommandType.StoredProcedure;
                    cmdInsCons.Parameters.Add("@flg_operacao", SqlDbType.Char).Value = 'I';
                    cmdInsCons.Parameters.Add("@COD_ID_PESSOA_LICENCIADO", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["LICENCIADO"].ToString());
                    cmdInsCons.Parameters.Add("@COD_ID_PESSOAS_FJ", SqlDbType.Int).Value = Funcoes.strToInt(sid_id.ToString());

                    cmdInsCons.Parameters.Add("@NUM_TOKEN_CAPPTA", SqlDbType.VarChar).Value = txtTokenCappta.Text.ToString();
                    cmdInsCons.Parameters.Add("@FLG_STATUS_CAPPTA", SqlDbType.VarChar).Value = txtStatusCappta.Text.ToString();
                    cmdInsCons.Parameters.Add("@FLG_CAPPTA", SqlDbType.Char).Value = ddlCappta.SelectedValue.ToString();
                    cmdInsCons.Parameters.Add("@DES_JSON_CAPPTA", SqlDbType.VarChar).Value = txtResposta.Text.ToString();
                    cmdInsCons.Parameters.Add("@COD_ID_NATUREZA_CAPPTA", SqlDbType.Int).Value = ddlNatureza.SelectedValue.ToString();
                    cmdInsCons.Parameters.Add("@COD_ID_PLANO", SqlDbType.Int).Value = Funcoes.strToInt(ddlPlano.SelectedValue.ToString());

                    cmdInsCons.ExecuteNonQuery();
                    connInsCons.Close();
                    connInsCons.Dispose();


                    ClientScript.RegisterStartupScript(this.GetType(),
                        "Alerta", "alert('Dados gravados e enviados com sucesso');", true);
                }
            }
        }
    }


    private void GravarDadosZoop()
    {
        CadastrarEstabelecimento();
        if (txtToken.Text.ToString().Trim() != "")
        {

            GravarDadosEstabelecimento();
            
            CadastrarPlano();
            CadastrarContas();
            AlterarPoliticaRecebimento();
        }
    }

    private void GravarDadosAsaas()
    {
        Funcoes.GravaAuditoria(Funcoes.strToInt(sid_id), Request.ServerVariables["REMOTE_ADDR"].ToString(), "Cadastro - Estabelecimentos - Conta Digital", "Gravar - ID: " + Funcoes.strToInt(sid_id).ToString());

        if ((ddlBaaS.SelectedValue.ToString().Trim() == "S"))
        {
            if ((Funcoes.Decrypt(txtTokenAsaas.Text.ToString().Trim()) == ""))
            {
                //abrir modal
                Page.ClientScript.RegisterStartupScript(this.GetType(), "ExecutaModal", "$('#mdContaDigital').modal('show');", true);
            }
        }


    }
    private void GravarDadosPagseguro()
    {
        if (Funcoes.strToInt(ddlPlano.SelectedValue.ToString().Trim()) > 0)
        {
            if (ddlPagseguro.SelectedValue.ToString().Trim() == "1")
            {
                if ((txtIDPagseguro.Text.ToString().Trim() != "") && (txtEmailPagseguro.Text.ToString().Trim() != "") && (txtTokenPagseguro.Text.ToString().Trim() != ""))
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


                    //cmdInsCons.Parameters.Add("@NOM_CODIGO_BANCO", SqlDbType.VarChar).Value = ddlInstituicaoFinanceira.SelectedValue.ToString();
                    //cmdInsCons.Parameters.Add("@NOM_TIPO_BANCO", SqlDbType.VarChar).Value = ddlTipoConta.SelectedValue.ToString();
                    //cmdInsCons.Parameters.Add("@NOM_NUMERO_AGENCIA_BANCO", SqlDbType.VarChar).Value = txtAgencia.Text.ToString();
                    //cmdInsCons.Parameters.Add("@NOM_NUMERO_DIGITO_AGENCIA_BANCO", SqlDbType.VarChar).Value = txtDigitoAgencia.Text.ToString();
                    //cmdInsCons.Parameters.Add("@NOM_NUMERO_CONTA_BANCO", SqlDbType.VarChar).Value = txtConta.Text.ToString();
                    //cmdInsCons.Parameters.Add("@NOM_NUMERO_DIGITO_CONTA_BANCO", SqlDbType.VarChar).Value = txtDigitoConta.Text.ToString();

                    cmdInsCons.Parameters.Add("@COD_ID_PLANO", SqlDbType.Int).Value = Funcoes.strToInt(ddlPlano.SelectedValue.ToString());

                    cmdInsCons.ExecuteNonQuery();
                    connInsCons.Close();
                    connInsCons.Dispose();

                    ClientScript.RegisterStartupScript(this.GetType(),
                        "SucessoPagseguro", "alert('Dados gravados e enviados com sucesso');", true);
                }
                else
                {
                    ClientScript.RegisterStartupScript(this.GetType(),
                        "ErroCamposVaziosPagseguro", "alert('É obrigatório informar ID, o E-Mail e o Token! Verifique e tente novamente.');", true);
                }
            }
        }
        else
        {
            ClientScript.RegisterStartupScript(this.GetType(),
                "ErroPagseguro", "alert('É obrigatório selecionar um plano de taxas e tarifas! Verifique e tente novamente.');", true);

        }
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
        if (ddlPeriodicidade.SelectedValue.ToString() == "D") { sIntervalo = "daily"; }
        if (ddlPeriodicidade.SelectedValue.ToString() == "S") { sIntervalo = "weekly"; }
        if (ddlPeriodicidade.SelectedValue.ToString() == "M") { sIntervalo = "monthly"; }
        bool bSaque = (ddlSaque.SelectedValue.ToString() == "S") ? true : false;

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
                    string jsonRetorno = "";
                    try
                    {
                        if (txtToken.Text.ToString().Trim() == "")
                        {
                            jsonRetorno = zoop.CadastrarVendedorPJ(json);
                            try
                            {
                                JObject oEstabelecimento = JObject.Parse(jsonRetorno);
                                ClientScript.RegisterStartupScript(this.GetType(), "SucessoPJ", "alert('Dados do Estabelecimento PJ " + oEstabelecimento["ein"].ToString() + " gravados com sucesso!');", true);
                            }
                            catch
                            {
                                ClientScript.RegisterStartupScript(this.GetType(), "ErroPJ", "alert('Ocorreu um erro ao tentar cadastrar o estabelecimento PJ na Adquirente! Verifique o cadastro e tente novamente');", true);
                                return;
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
                        return;
                    }
                    //txtAgencia.Text = jsonRetorno;
                }
                if (ReaderCadastro["FLG_TIPO_PESSOA"].ToString().Trim() == "PF")
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
                                ClientScript.RegisterStartupScript(this.GetType(), "Sucesso", "alert('Dados do Estabelecimento PF " + oEstabelecimento["taxpayer_id"].ToString() + " gravados com sucesso!');", true);
                            }
                            catch
                            {
                                ClientScript.RegisterStartupScript(this.GetType(), "ErroPF", "alert('Ocorreu um erro ao tentar cadastrar o estabelecimento PF na Adquirente! Verifique o cadastro e tente novamente');", true);
                                return;
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
                        return;
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
                    ClientScript.RegisterStartupScript(this.GetType(), "SucessoPlano", "alert('Dados do plano enviados com sucesso! Status: " + oPlano["status"].ToString() + "');", true);
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


    private void CadastrarContas()
    {

        string sIDTokenConta = "";
        string sIDConta = "";
        string sIDContaCodigo = "";

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



                //if (ReaderCadastroContas["NOM_TOKEN"].ToString().Trim() == "")
                //{

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

                //}
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


    private void GravarDadosBaaS()
    {

        Funcoes.GravaAuditoria(0, Request.ServerVariables["REMOTE_ADDR"].ToString(), "Cadastro - Estabelecimentos - Conta Digital", "Acesso - ID: " + Funcoes.strToInt(sid_id.ToString()).ToString() + " - " + Request.ServerVariables["SERVER_NAME"].ToString());

        // Salva Primeiro cadastro da Pessoa F/J

        SqlConnection connInsCons = new SqlConnection(Funcoes.conexao());
        connInsCons.Open();
        SqlCommand cmdInsCons = new SqlCommand("dbo.stp_pessoas_fj_baas_ins", connInsCons);
        cmdInsCons.CommandType = CommandType.StoredProcedure;
        cmdInsCons.Parameters.Add("@flg_operacao", SqlDbType.Char).Value = "I";
        cmdInsCons.Parameters.Add("@COD_ID_PESSOA_LICENCIADO", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["LICENCIADO"].ToString());
        cmdInsCons.Parameters.Add("@COD_ID_PESSOAS_FJ", SqlDbType.Int).Value = Funcoes.strToInt(sid_id.ToString());

        cmdInsCons.Parameters.Add("@FLG_ORIGEM", SqlDbType.Char).Value = "A";

        cmdInsCons.Parameters.Add("@NUM_TOKEN_BAAS", SqlDbType.VarChar).Value = Funcoes.Decrypt(txtTokenAsaas.Text.ToString());
        cmdInsCons.Parameters.Add("@NUM_WALLETID_BAAS", SqlDbType.VarChar).Value = txtWalletID.Text.ToString();
        cmdInsCons.Parameters.Add("@FLG_STATUS_BAAS", SqlDbType.VarChar).Value = txtStatusAsaas.Text.ToString();
        cmdInsCons.Parameters.Add("@FLG_BAAS", SqlDbType.Char).Value = ddlBaaS.SelectedValue.ToString();
        cmdInsCons.Parameters.Add("@DES_JSON_BAAS", SqlDbType.VarChar).Value = txtResposta.Text.ToString();

        cmdInsCons.Parameters.Add("@NUM_ID_CONTA_BAAS", SqlDbType.VarChar).Value = txtIDConta.Text.ToString();
        cmdInsCons.Parameters.Add("@NUM_AGENCIA_BAAS", SqlDbType.VarChar).Value = txtAgencia.Text.ToString();
        cmdInsCons.Parameters.Add("@NUM_CONTA_BAAS", SqlDbType.VarChar).Value = txtConta.Text.ToString();
        cmdInsCons.Parameters.Add("@NUM_DIGITO_CONTA_BAAS", SqlDbType.VarChar).Value = txtDigitoConta.Text.ToString();

        cmdInsCons.Parameters.Add("@NUM_CHAVE_PIX", SqlDbType.VarChar).Value = txtChavePix.Text.ToString();


        cmdInsCons.ExecuteNonQuery();
        connInsCons.Close();
        connInsCons.Dispose();

        // Inserir no TIMELINE

        // ATUALIZA TABELA TIMELINE
        SqlConnection connInsConsTIMELINE = new SqlConnection(Funcoes.conexao());
        connInsConsTIMELINE.Open();
        SqlCommand cmdInsConsTIMELINE = new SqlCommand("dbo.stp_pessoas_fj_baas_timeline_ins", connInsConsTIMELINE);
        cmdInsConsTIMELINE.CommandType = CommandType.StoredProcedure;
        cmdInsConsTIMELINE.Parameters.Add("@flg_operacao", SqlDbType.Char).Value = 'I';
        cmdInsConsTIMELINE.Parameters.Add("@NUM_ID_CONTA_BAAS", SqlDbType.VarChar).Value = txtIDConta.Text.ToString();
        cmdInsConsTIMELINE.Parameters.Add("@NOM_STATUS", SqlDbType.VarChar).Value = "Conta Criada";
        cmdInsConsTIMELINE.Parameters.Add("@DTA_TIMELINE", SqlDbType.DateTime).Value = DateTime.Now;
        cmdInsConsTIMELINE.ExecuteNonQuery();
        connInsConsTIMELINE.Close();
        connInsConsTIMELINE.Dispose();


        ClientScript.RegisterStartupScript(this.GetType(),
            "Alerta", "alert('Dados gravados com sucesso');", true);

    }


    private void CadastrarBaaS()
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

            string sCompanyType = "";
            if (ReaderCadastro["FLG_TIPO_PESSOA"].ToString().Trim() == "PJ")
            {
                if (ReaderCadastro["NOM_TIPO_EMPRESA"].ToString() == "MEI") { sCompanyType = "MEI"; }
                if (ReaderCadastro["NOM_TIPO_EMPRESA"].ToString() == "Empresário Individual") { sCompanyType = "INDIVIDUAL"; }
                if (ReaderCadastro["NOM_TIPO_EMPRESA"].ToString() == "Sociedade Limitada Unipessoal") { sCompanyType = "LIMITED"; }
                if (ReaderCadastro["NOM_TIPO_EMPRESA"].ToString() == "Sociedade Empresária Limitada") { sCompanyType = "LIMITED"; }
                if (ReaderCadastro["NOM_TIPO_EMPRESA"].ToString() == "Sociedade Simples") { sCompanyType = "LIMITED"; }
                if (ReaderCadastro["NOM_TIPO_EMPRESA"].ToString() == "Sociedade Anônima") { sCompanyType = "LIMITED"; }
            }

            dadosSubconta.Subconta dsubconta = new dadosSubconta.Subconta()
            {
                name = (ReaderCadastro["FLG_TIPO_PESSOA"].ToString().Trim() == "PF") ? ReaderCadastro["NOM_NOME"].ToString().Trim() + " " + ReaderCadastro["NOM_SOBRENOME"].ToString().Trim() : ReaderCadastro["NOM_RAZAOSOCIAL"].ToString().Trim(),
                email = ReaderCadastro["NOM_EMAIL"].ToString().Trim(),
                loginEmail = ReaderCadastro["NOM_EMAIL"].ToString().Trim(),
                birthDate = (ReaderCadastro["FLG_TIPO_PESSOA"].ToString().Trim() == "PF") ? String.Format("{0:yyyy-MM-dd}", Convert.ToDateTime(ReaderCadastro["DTA_ANIVERSARIO"].ToString().Trim())) : "",
                cpfCnpj = (ReaderCadastro["FLG_TIPO_PESSOA"].ToString().Trim() == "PF") ? TIRAACENTOS(ReaderCadastro["NOM_CPF"].ToString().Trim()) : TIRAACENTOS(ReaderCadastro["NOM_CNPJ"].ToString().Trim()),
                companyType = sCompanyType.ToString(),
                mobilePhone = TIRAACENTOS(ReaderCadastro["NOM_CELULAR"].ToString().Trim()),
                incomeValue = (ReaderCadastro["FLG_TIPO_PESSOA"].ToString().Trim() == "PF") ? Funcoes.strToInt(ReaderCadastro["NUM_RENDA_MENSAL"].ToString().Trim()) : Funcoes.strToInt(ReaderCadastro["NUM_FATURAMENTO"].ToString().Trim()),
                address = ReaderCadastro["NOM_ENDERECO"].ToString().Trim(),
                addressNumber = ReaderCadastro["NOM_NUMERO"].ToString().Trim(),
                complement = ReaderCadastro["NOM_COMPLEMENTO"].ToString().Trim(),
                province = ReaderCadastro["NOM_BAIRRO"].ToString().Trim(),
                postalCode = TIRAACENTOS(ReaderCadastro["NOM_CEP"].ToString().Trim()),
                webhooks = new dadosSubconta.webhooks[]
                {
                    new dadosSubconta.webhooks()
                    {
                    name = "WEBHOOK-ID-"+ReaderCadastro["COD_ID"].ToString().Trim().PadLeft(6,'0'),
                    url = "https://conta.legacybank.com.br/events/asaas",
                    email = (HttpContext.Current.Session["EMAIL"].ToString().Trim()!="")? HttpContext.Current.Session["EMAIL"].ToString().Trim() : ReaderCadastro["NOM_EMAIL"].ToString().Trim(),
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

            //ClientScript.RegisterStartupScript(this.GetType(),
            //    "json", "alert('"+jsonSubconta.ToString()+"');", true);
            //txtResposta.Text = jsonSubconta.ToString();
            //return;
            
            string jsonRetorno = asaas.CriarSubconta(jsonSubconta);

            //txtResposta.Text = jsonRetorno;
            //txtResposta.Visible = true;
            // Gravar notificacao asaas
            SqlConnection connInsConsASAAS = new SqlConnection(Funcoes.conexao());
            connInsConsASAAS.Open();
            SqlCommand cmdInsConsASAAS = new SqlCommand("dbo.stp_notificacoes_asaas_ins", connInsConsASAAS);
            cmdInsConsASAAS.CommandType = CommandType.StoredProcedure;
            cmdInsConsASAAS.Parameters.Add("@flg_operacao", SqlDbType.Char).Value = 'I';
            cmdInsConsASAAS.Parameters.Add("@DES_JSON", SqlDbType.Text).Value = jsonRetorno.ToString();
            cmdInsConsASAAS.Parameters.Add("@DTA_DATA", SqlDbType.DateTime).Value = DateTime.Now;
            cmdInsConsASAAS.Parameters.Add("@NOM_CODE", SqlDbType.VarChar).Value = "00000000000000000000";

            cmdInsConsASAAS.Parameters.Add("@NUM_TOKEN", SqlDbType.VarChar).Value = "0000000000000000000";

            cmdInsConsASAAS.Parameters.Add("@NOM_TIPO", SqlDbType.VarChar).Value = "ACCOUNT_CREATED";
            cmdInsConsASAAS.Parameters.Add("@FLG_STATUS", SqlDbType.VarChar).Value = "ACCOUNT_CREATED";
            cmdInsConsASAAS.ExecuteNonQuery();
            connInsConsASAAS.Close();
            connInsConsASAAS.Dispose();

            // tratar os dados de retorno
            try
            {
                JObject o = JObject.Parse(jsonRetorno.ToString());
                try
                {
                    txtTokenAsaas.Text = Funcoes.Encrypt(o["apiKey"].ToString());
                    txtIDConta.Text = o["id"].ToString();
                    txtAgencia.Text = o["accountNumber"]["agency"].ToString();
                    txtConta.Text = o["accountNumber"]["account"].ToString();
                    txtDigitoConta.Text = o["accountNumber"]["accountDigit"].ToString();
                    txtWalletID.Text = o["walletId"].ToString();

                    ClientScript.RegisterStartupScript(this.GetType(),
                        "Alerta", "alert('A conta foi criada com sucesso');", true);

                    // Enviar email com dados da url para captura dos documentos 
                }
                catch
                {
                    //txtToken.Text = o["errors"][0]["description"].ToString();
                    ClientScript.RegisterStartupScript(this.GetType(),
                        "ErroContaDigital", "alert('Ocorreu um erro ao tentar criar a conta do estabelecimento - erro: " + o["errors"][0]["description"].ToString() + "');", true);

                }
            }
            catch
            {
                ClientScript.RegisterStartupScript(this.GetType(),
                    "ErroGeralContaDigital", "alert('Ocorreu um erro ao tentar criar a conta digital!Verifique e tente novamente.');", true);

            }
        }
    }

    protected void lbkGerar_Click(object sender, EventArgs e)
    {
        if (txtChavePix.Text.ToString().Trim() == "")
        {
            string jsonChavePix = "";
            jsonChavePix = jsonChavePix + "{";
            jsonChavePix = jsonChavePix + "\"type\":\"" + "EVP" + "\",";
            jsonChavePix = jsonChavePix + "}";

            string sRetornoChavePix = asaas.CriarChavePix(Funcoes.Decrypt(txtTokenAsaas.Text.ToString()), jsonChavePix.ToString());
            JObject oChavePix = JObject.Parse(sRetornoChavePix.ToString());

            try
            {
                SqlConnection connInsCons = new SqlConnection(Funcoes.conexao());
                connInsCons.Open();
                SqlCommand cmdInsCons = new SqlCommand("dbo.stp_pessoas_fj_baas_ins", connInsCons);
                cmdInsCons.CommandType = CommandType.StoredProcedure;
                cmdInsCons.Parameters.Add("@flg_operacao", SqlDbType.Char).Value = 'P';
                cmdInsCons.Parameters.Add("@COD_ID_PESSOA_LICENCIADO", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["LICENCIADO"].ToString());
                cmdInsCons.Parameters.Add("@COD_ID_PESSOAS_FJ", SqlDbType.Int).Value = Funcoes.strToInt(sid_id.ToString());
                cmdInsCons.Parameters.Add("@FLG_ORIGEM", SqlDbType.Char).Value = "A";

                cmdInsCons.Parameters.Add("@NUM_CHAVE_PIX", SqlDbType.VarChar).Value = oChavePix["key"].ToString();
                cmdInsCons.Parameters.Add("@NUM_ID_CHAVE_PIX", SqlDbType.VarChar).Value = oChavePix["id"].ToString();
                cmdInsCons.Parameters.Add("@FLG_STATUS_CHAVE_PIX", SqlDbType.VarChar).Value = oChavePix["status"].ToString();
                cmdInsCons.Parameters.Add("@NOM_IMAGEM_CHAVE_PIX", SqlDbType.VarChar).Value = oChavePix["qrCode"]["encodedImage"].ToString();
                cmdInsCons.Parameters.Add("@NOM_QRCODE_CHAVE_PIX", SqlDbType.VarChar).Value = oChavePix["qrCode"]["payload"].ToString();
                cmdInsCons.Parameters.Add("@DES_JSON_CHAVE_PIX", SqlDbType.Text).Value = sRetornoChavePix.ToString();

                cmdInsCons.ExecuteNonQuery();
                connInsCons.Close();
                connInsCons.Dispose();


                txtChavePix.Text = oChavePix["key"].ToString();

                ClientScript.RegisterStartupScript(this.GetType(),
                    "Alerta", "alert('A chave PIX foi criada com sucesso!');", true);
            }
            catch
            {
                try
                {
                    //txtToken.Text = o["errors"][0]["description"].ToString();
                    ClientScript.RegisterStartupScript(this.GetType(),
                        "ErroChavePix", "alert('Ocorreu um erro ao tentar criar a chave PIX - erro: " + oChavePix["errors"][0]["description"].ToString() + "!');", true);
                }
                catch
                {
                    ClientScript.RegisterStartupScript(this.GetType(),
                        "ErroMensagem", "alert('Ocorreu um erro ao tentar criar a chave PIX!');", true);

                }

            }

        }
        else
        {
            ClientScript.RegisterStartupScript(this.GetType(),
                "Alerta", "alert('a chave PIX já existe no BAAS!');", true);
        }
    }

    protected void lkbVer_Click(object sender, EventArgs e)
    {
        if (Funcoes.Enviar2fa() == true)
        {
            ClientScript.RegisterStartupScript(this.GetType(), "Codigo2fa", "alert('Um código de confirmação foi enviado para o seu e-mail!');", true);

        }
        else
        {
            ClientScript.RegisterStartupScript(this.GetType(), "Codigo2faErro", "alert('Ocorreu um erro ao tentar enviar o Código de Confirmação!');", true);

        }
        ClientScript.RegisterStartupScript(this.GetType(), "ExecutaModal", "$('#mdConfirmar').modal('show');", true);
    }

    protected void btnConfirmar2FA_Click(object sender, EventArgs e)
    {
        SqlConnection connVerifica2fa = new SqlConnection(Funcoes.conexao());
        SqlCommand cmdVerifica2fa = new SqlCommand("dbo.stp_2fa_ins", connVerifica2fa);
        cmdVerifica2fa.CommandType = CommandType.StoredProcedure;
        cmdVerifica2fa.Parameters.Add("@flg_operacao", SqlDbType.Char).Value = "L";
        cmdVerifica2fa.Parameters.Add("@COD_ID_SIS_USUARIO", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["CODIGO"].ToString());
        cmdVerifica2fa.Parameters.Add("@COD_ID_PESSOA_LICENCIADO", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["LICENCIADO"].ToString());
        cmdVerifica2fa.Parameters.Add("@NOM_2FA", SqlDbType.VarChar).Value = Funcoes.Encrypt(txt2FABoletos.Text.ToString());
        cmdVerifica2fa.Parameters.Add("@DTA_EXPIRACAO", SqlDbType.DateTime).Value = DateTime.Now;
        connVerifica2fa.Open();
        SqlDataReader mReader2fa = cmdVerifica2fa.ExecuteReader();

        if (mReader2fa.Read())
        {
            MostrarToken();
        }
        else
        {
            ClientScript.RegisterStartupScript(this.GetType(),
                "CodigoErrado2fa", "alert('O Código de confirmação de autenticação não está correto! Verifique e reentre.');", true);

        }
    }
    protected void lkbReenviar_Click(object sender, EventArgs e)
    {
        if (Funcoes.Enviar2fa() == true)
        {
            ClientScript.RegisterStartupScript(this.GetType(),
"Codigo2fa", "alert('Um código de confirmação foi enviado para o seu e-mail!');", true);

        }
        else
        {
            ClientScript.RegisterStartupScript(this.GetType(),
"Codigo2faErro", "alert('Ocorreu um erro ao tentar enviar o Código de Confirmação!');", true);

        }
        ClientScript.RegisterStartupScript(this.GetType(),
"ExecutaModal", "$('#mdConfirmar').modal('show');", true);

    }

    private void MostrarToken()
    {
        txtTokenAsaasExibir.Text = Funcoes.Decrypt(txtTokenAsaas.Text.ToString());
        ClientScript.RegisterStartupScript(this.GetType(), "ExecutaModalExibir", "$('#mdToken').modal('show');", true);
        
    }

    public string VerSenha()
    {
        return "";
    }
    protected void btnExcluir_Click(object sender, EventArgs e)
    {
        if (txtMotivo.Text.ToString().Trim() != "")
        {
            try
            {
                string jsonDeletarConta = asaas.DeletarConta(asaas.PegarTokenSubconta(Funcoes.strToInt(sid_id.ToString()), Funcoes.strToInt(HttpContext.Current.Session["LICENCIADO"].ToString())),txtMotivo.Text.ToString().Trim());

                JObject oDeletarConta = JObject.Parse(jsonDeletarConta.ToString());
                try
                {

                    ClientScript.RegisterStartupScript(this.GetType(), "SucessoExclusao", "alert('" + oDeletarConta["observations"].ToString() + "');", true);

                    // Salvar Motivo

                    SqlConnection connInsCons = new SqlConnection(Funcoes.conexao());
                    connInsCons.Open();
                    SqlCommand cmdInsCons = new SqlCommand("dbo.stp_pessoas_fj_baas_ins", connInsCons);
                    cmdInsCons.CommandType = CommandType.StoredProcedure;
                    cmdInsCons.Parameters.Add("@flg_operacao", SqlDbType.Char).Value = 'E';
                    cmdInsCons.Parameters.Add("@COD_ID_PESSOA_LICENCIADO", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["LICENCIADO"].ToString());
                    cmdInsCons.Parameters.Add("@COD_ID_PESSOAS_FJ", SqlDbType.Int).Value = Funcoes.strToInt(sid_id.ToString());
                    cmdInsCons.Parameters.Add("@FLG_ORIGEM", SqlDbType.Char).Value = "A";

                    cmdInsCons.Parameters.Add("@DTA_EXCLUSAO", SqlDbType.DateTime).Value = DateTime.Now;
                    cmdInsCons.Parameters.Add("@NOM_MOTIVO_EXCLUSAO", SqlDbType.VarChar).Value = txtMotivo.Text.ToString();
                    cmdInsCons.Parameters.Add("@COD_ID_USUARIO_EXCLUSAO", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["CODIGO"].ToString());

                    cmdInsCons.ExecuteNonQuery();
                    connInsCons.Close();
                    connInsCons.Dispose();

                }
                catch
                {
                    ClientScript.RegisterStartupScript(this.GetType(), "ErroExclusao", "alert('" + oDeletarConta["observations"].ToString() + "');", true);
                }

                txtMotivo.Text = jsonDeletarConta;
            }
            catch
            {
                ClientScript.RegisterStartupScript(this.GetType(), "ErroGeral", "alert('Ocorreu um erro ao tentar excluir a conta! Verifique e tente novamente');", true);
            }

            ConsultaAsaas();

        }
        else
        {
            ClientScript.RegisterStartupScript(this.GetType(),
"EspecificarMotivo", "alert('É Obrigatório especificar o motivo da exclusão da conta! Verifique e tente novamente.');", true);
        }
    }
    protected void btnConfirmarContaDigital_Click(object sender, EventArgs e)
    {
        CadastrarBaaS();
        GravarDadosBaaS();

    }
    protected void lkbImportarPlano_Click(object sender, EventArgs e)
    {
        if (txtPlanoReferencia.Text.ToString().Trim() != "")
        {
            try
            {
                GravarDadosPlano(txtPlanoReferencia.Text.ToString());
                CarregaPlanos();
                if (Funcoes.strToInt(ddlPlano.SelectedValue.ToString().Trim()) <= 0)
                {
                    SqlConnection mySelCadastroPlanos = new SqlConnection(Funcoes.conexao());
                    mySelCadastroPlanos.Open();
                    SqlCommand cmdSelCadastroPlanos = new SqlCommand("dbo.stp_planos_ins", mySelCadastroPlanos);
                    cmdSelCadastroPlanos.CommandType = CommandType.StoredProcedure;
                    cmdSelCadastroPlanos.Parameters.Add("@flg_operacao", SqlDbType.Char).Value = "H";
                    cmdSelCadastroPlanos.Parameters.Add("@COD_ID_PESSOA_LICENCIADO", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["LICENCIADO"].ToString());
                    cmdSelCadastroPlanos.Parameters.Add("@COD_ID_PESSOAS_FJ", SqlDbType.Int).Value = 0;
                    cmdSelCadastroPlanos.Parameters.Add("@FLG_MODELO_PLANO", SqlDbType.Char).Value = "C";
                    cmdSelCadastroPlanos.Parameters.Add("@NOM_REFERENCIA_PLANO", SqlDbType.VarChar).Value = txtPlanoReferencia.Text.ToString();
                    SqlDataReader ReaderCadastroPlanos = cmdSelCadastroPlanos.ExecuteReader();
                    while (ReaderCadastroPlanos.Read())
                    {
                        ddlPlano.SelectedValue = ReaderCadastroPlanos["COD_ID"].ToString();
                    }
                }
            }
            catch
            {

            }

        }
    }

    private void GravarDadosPlano(string sPlano)
    {
        string jsonPlano = hubcappta.ConsultarPlano(sPlano.ToString());

        //txtPlanos.Text = txtPlanos.Text + jsonPlano + "     -----     ";
        if (jsonPlano.ToString().Trim() != "")
        {
            try
            {
                JObject oPlanos = JObject.Parse(jsonPlano.ToString());

                SqlConnection connInsCons = new SqlConnection(Funcoes.conexao());
                connInsCons.Open();
                SqlCommand cmdInsCons = new SqlCommand("dbo.stp_planos_ins", connInsCons);
                cmdInsCons.CommandType = CommandType.StoredProcedure;
                cmdInsCons.Parameters.Add("@flg_operacao", SqlDbType.Char).Value = 'I';

                cmdInsCons.Parameters.Add("@COD_ID_PESSOA_LICENCIADO", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["LICENCIADO"].ToString());
                cmdInsCons.Parameters.Add("@COD_ID_PESSOAS_FJ", SqlDbType.Int).Value = 0;

                cmdInsCons.Parameters.Add("@FLG_MODELO_PLANO", SqlDbType.Char).Value = "C";
                cmdInsCons.Parameters.Add("@FLG_ATIVO", SqlDbType.Char).Value = "S";
                cmdInsCons.Parameters.Add("@FLG_VISIVEL", SqlDbType.Char).Value = "S";
                cmdInsCons.Parameters.Add("@FLG_ANTECIPADO", SqlDbType.Char).Value = "S";
                cmdInsCons.Parameters.Add("@FLG_TIPO_PLANO", SqlDbType.Char).Value = "C";
                cmdInsCons.Parameters.Add("@NOM_REFERENCIA_PLANO", SqlDbType.VarChar).Value = oPlanos["id"].ToString().Trim();


                cmdInsCons.Parameters.Add("@COD_ID_PLANO_BASE", SqlDbType.Int).Value = Funcoes.strToInt(oPlanos["basePlanId"].ToString().Trim());
                // LOCALIZAR PLANO REFERENCIA

                SqlConnection mySelCadastroPlano = new SqlConnection(Funcoes.conexao());
                mySelCadastroPlano.Open();
                SqlCommand cmdSelCadastroPlano = new SqlCommand("dbo.stp_planos_referencia_ins", mySelCadastroPlano);
                cmdSelCadastroPlano.CommandType = CommandType.StoredProcedure;
                cmdSelCadastroPlano.Parameters.Add("@flg_operacao", SqlDbType.Char).Value = "P";
                cmdSelCadastroPlano.Parameters.Add("@COD_ID_PLANO_REFERENCIA", SqlDbType.VarChar).Value = oPlanos["basePlanId"].ToString().Trim();
                SqlDataReader ReaderCadastroPlano = cmdSelCadastroPlano.ExecuteReader();
                while (ReaderCadastroPlano.Read())
                {
                    cmdInsCons.Parameters.Add("@COD_ID_PLANOS_REFERENCIA", SqlDbType.Int).Value = Funcoes.strToInt(ReaderCadastroPlano["COD_ID"].ToString().Trim());
                }

                cmdInsCons.Parameters.Add("@NOM_TITULO_PLANO", SqlDbType.VarChar).Value = oPlanos["name"].ToString().Trim(); ;
                cmdInsCons.Parameters.Add("@DES_PLANO", SqlDbType.Text).Value = oPlanos["name"].ToString().Trim();
                cmdInsCons.Parameters.Add("@NUM_DIAS_LIQUIDACAO", SqlDbType.Int).Value = Funcoes.strToInt(oPlanos["settlementDays"].ToString().Trim());
                cmdInsCons.Parameters.Add("@NUM_TAXA_ANTECIPACAO", SqlDbType.Float).Value = 0;
                string sCodigoInclusao = cmdInsCons.ExecuteScalar().ToString();
                connInsCons.Close();
                connInsCons.Dispose();


                for (int t = 0; t < oPlanos["schemes"].Count(); t++)
                {
                    for (int p = 0; p < oPlanos["schemes"][t]["fees"].Count(); p++)
                    {
                        SqlConnection connInsConsTaxas = new SqlConnection(Funcoes.conexao());
                        connInsConsTaxas.Open();
                        SqlCommand cmdInsConsTaxas = new SqlCommand("dbo.stp_planos_parcelas_ins", connInsConsTaxas);
                        cmdInsConsTaxas.CommandType = CommandType.StoredProcedure;
                        cmdInsConsTaxas.Parameters.Add("@flg_operacao", SqlDbType.Char).Value = 'I';
                        cmdInsConsTaxas.Parameters.Add("@COD_ID_PESSOA_LICENCIADO", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["LICENCIADO"].ToString());
                        cmdInsConsTaxas.Parameters.Add("@COD_ID_PLANOS", SqlDbType.Int).Value = Funcoes.strToInt(sCodigoInclusao);

                        int iBandeira = 0;
                        int iPlanoReferencia = 0;
                        string sBandeira = "";
                        string sTipo = "";
                        // Busca Bandeira Tabela OPÇÕES PLANOS

                        SqlConnection mySelCadastroBandeiras = new SqlConnection(Funcoes.conexao());
                        mySelCadastroBandeiras.Open();
                        SqlCommand cmdSelCadastroBandeiras = new SqlCommand("dbo.stp_opcoes_planos_cappta_ins", mySelCadastroBandeiras);
                        cmdSelCadastroBandeiras.CommandType = CommandType.StoredProcedure;
                        cmdSelCadastroBandeiras.Parameters.Add("@flg_operacao", SqlDbType.Char).Value = "B";
                        cmdSelCadastroBandeiras.Parameters.Add("@NOM_REFERENCIA", SqlDbType.VarChar).Value = oPlanos["schemes"][t]["scheme"].ToString();
                        SqlDataReader ReaderCadastroBandeiras = cmdSelCadastroBandeiras.ExecuteReader();
                        while (ReaderCadastroBandeiras.Read())
                        {
                            iBandeira = Funcoes.strToInt(ReaderCadastroBandeiras["COD_BANDEIRA"].ToString());
                            sBandeira = ReaderCadastroBandeiras["NOM_BANDEIRA"].ToString();
                            sTipo = ReaderCadastroBandeiras["NOM_TIPO"].ToString();
                        }


                        cmdInsConsTaxas.Parameters.Add("@COD_ID_BANDEIRA", SqlDbType.Int).Value = iBandeira;
                        cmdInsConsTaxas.Parameters.Add("@FLG_PRESENCIAL_ONLINE", SqlDbType.Char).Value = "P";
                        // Localizar o plano de referencia


                        // Carrega as taxas
                        if (sTipo == "debit")
                        {
                            cmdInsConsTaxas.Parameters.Add("@NUM_VALOR_DEBITO", SqlDbType.Float).Value = Funcoes.strToDouble(oPlanos["schemes"][t]["fees"][p]["rate"].ToString()) / 100;
                        }
                        if (sTipo == "credit")
                        {
                            if (Funcoes.strToInt(oPlanos["schemes"][t]["fees"][p]["installments"].ToString()) == 1) { cmdInsConsTaxas.Parameters.Add("@NUM_VALOR_CREDITO_VISTA", SqlDbType.Float).Value = Funcoes.strToDouble(oPlanos["schemes"][t]["fees"][p]["rate"].ToString()) / 100; }
                            if (Funcoes.strToInt(oPlanos["schemes"][t]["fees"][p]["installments"].ToString()) == 2) { cmdInsConsTaxas.Parameters.Add("@NUM_VALOR_CREDITO_2X", SqlDbType.Float).Value = Funcoes.strToDouble(oPlanos["schemes"][t]["fees"][p]["rate"].ToString()) / 100; }
                            if (Funcoes.strToInt(oPlanos["schemes"][t]["fees"][p]["installments"].ToString()) == 3) { cmdInsConsTaxas.Parameters.Add("@NUM_VALOR_CREDITO_3X", SqlDbType.Float).Value = Funcoes.strToDouble(oPlanos["schemes"][t]["fees"][p]["rate"].ToString()) / 100; }
                            if (Funcoes.strToInt(oPlanos["schemes"][t]["fees"][p]["installments"].ToString()) == 4) { cmdInsConsTaxas.Parameters.Add("@NUM_VALOR_CREDITO_4X", SqlDbType.Float).Value = Funcoes.strToDouble(oPlanos["schemes"][t]["fees"][p]["rate"].ToString()) / 100; }
                            if (Funcoes.strToInt(oPlanos["schemes"][t]["fees"][p]["installments"].ToString()) == 5) { cmdInsConsTaxas.Parameters.Add("@NUM_VALOR_CREDITO_5X", SqlDbType.Float).Value = Funcoes.strToDouble(oPlanos["schemes"][t]["fees"][p]["rate"].ToString()) / 100; }
                            if (Funcoes.strToInt(oPlanos["schemes"][t]["fees"][p]["installments"].ToString()) == 6) { cmdInsConsTaxas.Parameters.Add("@NUM_VALOR_CREDITO_6X", SqlDbType.Float).Value = Funcoes.strToDouble(oPlanos["schemes"][t]["fees"][p]["rate"].ToString()) / 100; }
                            if (Funcoes.strToInt(oPlanos["schemes"][t]["fees"][p]["installments"].ToString()) == 7) { cmdInsConsTaxas.Parameters.Add("@NUM_VALOR_CREDITO_7X", SqlDbType.Float).Value = Funcoes.strToDouble(oPlanos["schemes"][t]["fees"][p]["rate"].ToString()) / 100; }
                            if (Funcoes.strToInt(oPlanos["schemes"][t]["fees"][p]["installments"].ToString()) == 8) { cmdInsConsTaxas.Parameters.Add("@NUM_VALOR_CREDITO_8X", SqlDbType.Float).Value = Funcoes.strToDouble(oPlanos["schemes"][t]["fees"][p]["rate"].ToString()) / 100; }
                            if (Funcoes.strToInt(oPlanos["schemes"][t]["fees"][p]["installments"].ToString()) == 9) { cmdInsConsTaxas.Parameters.Add("@NUM_VALOR_CREDITO_9X", SqlDbType.Float).Value = Funcoes.strToDouble(oPlanos["schemes"][t]["fees"][p]["rate"].ToString()) / 100; }
                            if (Funcoes.strToInt(oPlanos["schemes"][t]["fees"][p]["installments"].ToString()) == 10) { cmdInsConsTaxas.Parameters.Add("@NUM_VALOR_CREDITO_10X", SqlDbType.Float).Value = Funcoes.strToDouble(oPlanos["schemes"][t]["fees"][p]["rate"].ToString()) / 100; }
                            if (Funcoes.strToInt(oPlanos["schemes"][t]["fees"][p]["installments"].ToString()) == 11) { cmdInsConsTaxas.Parameters.Add("@NUM_VALOR_CREDITO_11X", SqlDbType.Float).Value = Funcoes.strToDouble(oPlanos["schemes"][t]["fees"][p]["rate"].ToString()) / 100; }
                            if (Funcoes.strToInt(oPlanos["schemes"][t]["fees"][p]["installments"].ToString()) == 12) { cmdInsConsTaxas.Parameters.Add("@NUM_VALOR_CREDITO_12X", SqlDbType.Float).Value = Funcoes.strToDouble(oPlanos["schemes"][t]["fees"][p]["rate"].ToString()) / 100; }
                            if (Funcoes.strToInt(oPlanos["schemes"][t]["fees"][p]["installments"].ToString()) == 13) { cmdInsConsTaxas.Parameters.Add("@NUM_VALOR_CREDITO_13X", SqlDbType.Float).Value = Funcoes.strToDouble(oPlanos["schemes"][t]["fees"][p]["rate"].ToString()) / 100; }
                            if (Funcoes.strToInt(oPlanos["schemes"][t]["fees"][p]["installments"].ToString()) == 14) { cmdInsConsTaxas.Parameters.Add("@NUM_VALOR_CREDITO_14X", SqlDbType.Float).Value = Funcoes.strToDouble(oPlanos["schemes"][t]["fees"][p]["rate"].ToString()) / 100; }
                            if (Funcoes.strToInt(oPlanos["schemes"][t]["fees"][p]["installments"].ToString()) == 15) { cmdInsConsTaxas.Parameters.Add("@NUM_VALOR_CREDITO_15X", SqlDbType.Float).Value = Funcoes.strToDouble(oPlanos["schemes"][t]["fees"][p]["rate"].ToString()) / 100; }
                            if (Funcoes.strToInt(oPlanos["schemes"][t]["fees"][p]["installments"].ToString()) == 16) { cmdInsConsTaxas.Parameters.Add("@NUM_VALOR_CREDITO_16X", SqlDbType.Float).Value = Funcoes.strToDouble(oPlanos["schemes"][t]["fees"][p]["rate"].ToString()) / 100; }
                            if (Funcoes.strToInt(oPlanos["schemes"][t]["fees"][p]["installments"].ToString()) == 17) { cmdInsConsTaxas.Parameters.Add("@NUM_VALOR_CREDITO_17X", SqlDbType.Float).Value = Funcoes.strToDouble(oPlanos["schemes"][t]["fees"][p]["rate"].ToString()) / 100; }
                            if (Funcoes.strToInt(oPlanos["schemes"][t]["fees"][p]["installments"].ToString()) == 18) { cmdInsConsTaxas.Parameters.Add("@NUM_VALOR_CREDITO_18X", SqlDbType.Float).Value = Funcoes.strToDouble(oPlanos["schemes"][t]["fees"][p]["rate"].ToString()) / 100; }
                        }


                        cmdInsConsTaxas.ExecuteNonQuery();
                        connInsConsTaxas.Close();
                        connInsConsTaxas.Dispose();
                    }
                }
            }
            catch
            {
            }
        }
    }

    protected void btnAtualizarPlano_Click(object sender, EventArgs e)
    {
        GravarDadosEstabelecimento();

        CadastrarPlano();
    }
    protected void btnTimeline_Click(object sender, EventArgs e)
    {
        ClientScript.RegisterStartupScript(this.GetType(),
    "EstabelecimentosTimeline", "openPopupWindow('con_conta_digital_timeline.aspx?id=" + Funcoes.Encrypt(txtID.Text.ToString()).ToString() + "','EstabelecimentosTimeline',1024,800);", true);

    }
}