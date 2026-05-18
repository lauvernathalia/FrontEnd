using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Collections.Specialized;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Security.Cryptography;
using System.Data.SqlClient;
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

using MessagingToolkit.QRCode.Codec;
using MessagingToolkit.QRCode.Codec.Data;
using System.Drawing;

public partial class con_pagamentos_cobrança : System.Web.UI.Page
{
    public static DataTable dtParceiros;
    public static DataTable dtProdutos;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (HttpContext.Current.Session.Count <= 0)
        {
            Response.Redirect("login.aspx");
            FormsAuthentication.SignOut();
        }

        //Funcoes.CONFIRMA(btnConfirmar, "Confirma o pagamento do PIX informado?");

        if (!IsPostBack)
        {
            dtParceiros = new DataTable();
            dtParceiros.Columns.Add("beneficiario", typeof(string));
            dtParceiros.Columns.Add("walletid", typeof(string));
            dtParceiros.Columns.Add("valor", typeof(string));
            dtParceiros.Columns.Add("percentual", typeof(string));

            dtProdutos = new DataTable();
            dtProdutos.Columns.Add("id", typeof(string));
            dtProdutos.Columns.Add("produto", typeof(string));
            dtProdutos.Columns.Add("qtde", typeof(string));
            dtProdutos.Columns.Add("valor", typeof(string));


            VerificaPerfilComercial();

            //txtDescricao.Text = DateTimeOffset.Now.Offset.ToString().Split(':').FirstOrDefault().ToString();

            Funcoes.GravaAuditoria(0, Request.ServerVariables["REMOTE_ADDR"].ToString(), "Cobranças", "Acesso ao menu de opções");
            DateTime data = DateTime.Today;
            DateTime primeiroDiaDoMes = new DateTime(data.Year, data.Month, 1);
            DateTime ultimoDiaDoMes = new DateTime(data.Year, data.Month, DateTime.DaysInMonth(data.Year, data.Month));
            txtDataIni.Text = primeiroDiaDoMes.ToShortDateString();
            txtDataFim.Text = ultimoDiaDoMes.ToShortDateString();

            ConsultaSaldoBaas("A");

            divParceirosInclusao.Visible = true;
            divProdutosInclusao.Visible = true;
            CarregaAdquirentes();
            AtualizaListaClientes();
            btnGestaoCobranca_Click(null, null);

            ConsultaProdutos();
            ConsultaBeneficiarios();
            ConsultaCompradores();

        }
    }

    private void ConsultaCompradores()
    {
        SqlConnection myCompradores = new SqlConnection(Funcoes.conexao());
        myCompradores.Open();
        SqlCommand cmdCompradores = new SqlCommand("dbo.stp_compradores_ins", myCompradores);
        cmdCompradores.CommandType = CommandType.StoredProcedure;
        cmdCompradores.Parameters.Add("@flg_operacao", SqlDbType.VarChar).Value = "C";
        cmdCompradores.Parameters.Add("@COD_ID_PESSOA_LICENCIADO", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["LICENCIADO"].ToString());
        cmdCompradores.Parameters.Add("@COD_ID_PESSOAS_FJ", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["PESSOA"].ToString());
        cmdCompradores.Parameters.Add("@NOM_NOME", SqlDbType.VarChar).Value = "";
        cmdCompradores.Parameters.Add("@NUM_CNPJCPF", SqlDbType.VarChar).Value = "";
        cmdCompradores.Parameters.Add("@FLG_TIPO", SqlDbType.Char).Value = "C";


        SqlDataAdapter drCompradores = new SqlDataAdapter();
        drCompradores.SelectCommand = cmdCompradores;
        DataSet dsCompradores = new DataSet();
        drCompradores.Fill(dsCompradores, "PESSOAS_FJ");
        ddlComprador.DataTextField = "NOM_NOME";
        ddlComprador.DataValueField = "COD_ID";
        ddlComprador.DataSource = dsCompradores.Tables["PESSOAS_FJ"].DefaultView;
        ddlComprador.DataBind();
        ddlComprador.Items.Insert(0, new ListItem("", ""));

    }

    private void ConsultaBeneficiarios()
    {

        // Tabela de Parceiros

        SqlConnection myParceiros = new SqlConnection(Funcoes.conexao());
        myParceiros.Open();
        SqlCommand cmdParceiros = new SqlCommand("dbo.stp_vendas_parceiros_ins", myParceiros);
        cmdParceiros.CommandType = CommandType.StoredProcedure;
        cmdParceiros.Parameters.Add("@flg_operacao", SqlDbType.VarChar).Value = "G";
        cmdParceiros.Parameters.Add("@COD_ID_PESSOA_LICENCIADO", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["LICENCIADO"].ToString());
        cmdParceiros.Parameters.Add("@COD_ID_PESSOAS_FJ", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["PESSOA"].ToString());


        SqlDataAdapter drParceiros = new SqlDataAdapter();
        drParceiros.SelectCommand = cmdParceiros;
        DataSet dsParceiros = new DataSet();
        drParceiros.Fill(dsParceiros, "VENDAS_PARCEIROS");
        ddlParceiros.DataTextField = "NOM_NOME_BENEFICIARIO";
        ddlParceiros.DataValueField = "NOM_ID";
        ddlParceiros.DataSource = dsParceiros.Tables["VENDAS_PARCEIROS"].DefaultView;
        ddlParceiros.DataBind();
        ddlParceiros.Items.Insert(0, new ListItem("", ""));

    }



    private void ConsultaProdutos()
    {
        SqlConnection myProduto = new SqlConnection(Funcoes.conexao());
        myProduto.Open();
        SqlCommand cmdProduto = new SqlCommand("dbo.stp_produtos_ins", myProduto);
        cmdProduto.CommandType = CommandType.StoredProcedure;
        cmdProduto.Parameters.Add("@flg_operacao", SqlDbType.VarChar).Value = "C";
        cmdProduto.Parameters.Add("@COD_ID_PESSOA_LICENCIADO", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["LICENCIADO"].ToString());
        cmdProduto.Parameters.Add("@COD_ID_PESSOAS_FJ", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["PESSOA"].ToString());
        cmdProduto.Parameters.Add("@NOM_PRODUTO", SqlDbType.VarChar).Value = "";


        SqlDataAdapter drProduto = new SqlDataAdapter();
        drProduto.SelectCommand = cmdProduto;
        DataSet dsProduto = new DataSet();
        drProduto.Fill(dsProduto, "PRODUTOS");
        ddlProduto.DataTextField = "NOM_PRODUTO";
        ddlProduto.DataValueField = "COD_ID";
        ddlProduto.DataSource = dsProduto.Tables["PRODUTOS"].DefaultView;
        ddlProduto.DataBind();
        ddlProduto.Items.Insert(0, new ListItem("", ""));
    }
    
    
    private void VerificaPerfilComercial()
    {
        btnBoletoAvulso.Visible = Funcoes.CarregaPerfilComercial("B");
        btnVendaDigitada.Visible = Funcoes.CarregaPerfilComercial("C");
        btnCarne.Visible = Funcoes.CarregaPerfilComercial("B");
        btnAssinatura.Visible = Funcoes.CarregaPerfilComercial("B");
        btnReceberQRCode.Visible = Funcoes.CarregaPerfilComercial("X");


        dvFPBoleto.Visible = Funcoes.CarregaPerfilComercial("B");
        dvFPCredito.Visible = Funcoes.CarregaPerfilComercial("C");
        dvFPPix.Visible = Funcoes.CarregaPerfilComercial("X");

        dvParcelado.Visible = Funcoes.CarregaPerfilComercial("P");
        dvParceladoLinkPagamento.Visible = Funcoes.CarregaPerfilComercial("P");
    }

    private void CarregaAdquirentes()
    {
        SqlConnection myAdquirentes = new SqlConnection(Funcoes.conexao());
        myAdquirentes.Open();
        SqlCommand cmdAdquirentes = new SqlCommand("dbo.stp_pessoas_fj_integracoes_ins", myAdquirentes);
        cmdAdquirentes.CommandType = CommandType.StoredProcedure;
        cmdAdquirentes.Parameters.Add("@flg_operacao", SqlDbType.VarChar).Value = "F";
        cmdAdquirentes.Parameters.Add("@COD_ID_PESSOA_LICENCIADO", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["LICENCIADO"].ToString());
        cmdAdquirentes.Parameters.Add("@COD_ID_PESSOAS_FJ", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["PESSOA"].ToString());
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


    // -----------------------------------------------------------------------------------------------------------------------------------------------------------------
    // COBRANÇAS GERAL
    // -----------------------------------------------------------------------------------------------------------------------------------------------------------------

    private void GravaCobranca(string sIDCliente, string sIDCobranca, string sJsonBoleto, string sStatus, string sTipo, string sOrigem)
    {
        SqlConnection connInsConsCliente = new SqlConnection(Funcoes.conexao());
        connInsConsCliente.Open();
        SqlCommand cmdInsConsCliente = new SqlCommand("dbo.stp_cobrancas_ins", connInsConsCliente);
        cmdInsConsCliente.CommandType = CommandType.StoredProcedure;
        cmdInsConsCliente.Parameters.Add("@flg_operacao", SqlDbType.Char).Value = 'I';
        cmdInsConsCliente.Parameters.Add("@COD_ID_PESSOA_LICENCIADO", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["LICENCIADO"].ToString());
        cmdInsConsCliente.Parameters.Add("@COD_ID_PESSOAS_FJ_VENDEDOR", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["PESSOA"].ToString());
        cmdInsConsCliente.Parameters.Add("@COD_ID_COMPRADOR", SqlDbType.VarChar).Value = sIDCliente.ToString();
        
        //cmdInsConsCliente.Parameters.Add("@COD_ID_COBRANCA", SqlDbType.VarChar).Value = sIDCobranca.ToString();
        cmdInsConsCliente.Parameters.Add("@COD_ID_COBRANCA", SqlDbType.VarChar).Value = sIDCobranca.ToString();

        cmdInsConsCliente.Parameters.Add("@DTA_DATA", SqlDbType.DateTime).Value = DateTime.Now;
        cmdInsConsCliente.Parameters.Add("@FLG_TIPO", SqlDbType.Char).Value = sTipo.ToString();

        cmdInsConsCliente.Parameters.Add("@NOM_NOME", SqlDbType.VarChar).Value = txtNome.Text.ToString();
        cmdInsConsCliente.Parameters.Add("@NUM_CNPJCPF", SqlDbType.VarChar).Value = Funcoes.TIRAACENTOSDOCUMENTOS(txtCNPJCPF.Text.ToString());
        cmdInsConsCliente.Parameters.Add("@NOM_EMAIL", SqlDbType.VarChar).Value = txtEmail.Text.ToString();

        cmdInsConsCliente.Parameters.Add("@DES_DESCRICAO", SqlDbType.Text).Value = txtDescricao.Text.ToString();
        cmdInsConsCliente.Parameters.Add("@NOM_REFERENCIA", SqlDbType.VarChar).Value = txtReferencia.Text.ToString();
        cmdInsConsCliente.Parameters.Add("@DTA_VENCIMENTO", SqlDbType.DateTime).Value = Convert.ToDateTime(txtVencimento.Text.ToString());
        cmdInsConsCliente.Parameters.Add("@NUM_VALOR", SqlDbType.Float).Value = Funcoes.strToDouble(txtValor.Text.ToString());

        cmdInsConsCliente.Parameters.Add("@DES_JSON", SqlDbType.Text).Value = sJsonBoleto.ToString();
        cmdInsConsCliente.Parameters.Add("@FLG_STATUS", SqlDbType.VarChar).Value = sStatus.ToString();
        cmdInsConsCliente.Parameters.Add("@FLG_ORIGEM", SqlDbType.VarChar).Value = sOrigem.ToString();

        if (sTipo.ToString().Trim() == "Link de Pagamento")
        {
            cmdInsConsCliente.Parameters.Add("@NOM_URL_BOLETO", SqlDbType.VarChar).Value = "https://" + Request.ServerVariables["SERVER_NAME"].ToString() + "/checkout.aspx?id=" + Funcoes.Encrypt(sIDCobranca.ToString()) + "&tipo=" + Funcoes.Encrypt("L");
        }

        if (sTipo.ToString().Trim() == "Boleto Bancário Avulso")
        {
            cmdInsConsCliente.Parameters.Add("@NOM_URL_BOLETO", SqlDbType.VarChar).Value = "https://" + Request.ServerVariables["SERVER_NAME"].ToString() + "/linkpagamento.aspx?id=" + Funcoes.Encrypt(sIDCobranca.ToString()) + "&tipo=" + Funcoes.Encrypt("B");
        }

        if (sTipo.ToString().Trim() == "Receber por QRCode")
        {
            cmdInsConsCliente.Parameters.Add("@NOM_URL_BOLETO", SqlDbType.VarChar).Value = "https://" + Request.ServerVariables["SERVER_NAME"].ToString() + "/linkpagamento.aspx?id=" + Funcoes.Encrypt(sIDCobranca.ToString()) + "&tipo=" + Funcoes.Encrypt("Q");
        }

        cmdInsConsCliente.ExecuteNonQuery();
        connInsConsCliente.Close();
        connInsConsCliente.Dispose();
    }

    public static string GerarCobranca(string sIDCliente, string sValor, string sVencimento, string sDias, string sDescricao, string sReferencia, string sTipo)
    {
        DadosCobrancaAsaaS.Root dcobranca = new DadosCobrancaAsaaS.Root()
        {
            customer = sIDCliente.ToString(),
            billingType = sTipo.ToString(),
            value = Funcoes.strToDouble(sValor.ToString()),
            dueDate = Convert.ToDateTime(sVencimento.ToString()).Year.ToString() + "-" + Convert.ToDateTime(sVencimento.ToString()).Month.ToString() + "-" + Convert.ToDateTime(sVencimento.ToString()).Day.ToString(),
            daysAfterDueDateToRegistrationCancellation = Funcoes.strToInt(sDias.ToString()),
            description = sDescricao.ToString(),
            externalReference = sReferencia.ToString()
        };

        string jsonCobranca = JsonConvert.SerializeObject(dcobranca);
        string jsonURLCobranca = asaas.CriarCobranca(asaas.PegarTokenSubconta(Funcoes.strToInt(HttpContext.Current.Session["PESSOA"].ToString()), Funcoes.strToInt(HttpContext.Current.Session["LICENCIADO"].ToString())), jsonCobranca);

        return jsonURLCobranca.ToString();
    }

    
    // -----------------------------------------------------------------------------------------------------------------------------------------------------------------
    // Controle de Botões e chamadas de opções 
    // -----------------------------------------------------------------------------------------------------------------------------------------------------------------

    protected void btnGestaoCobranca_Click(object sender, EventArgs e)
    {
        ControleOpcoes();
        divGestao.Visible = true;

        ConsultaGestaoCobrancas();
    }
    protected void btnBoletoAvulso_Click(object sender, EventArgs e)
    {
        Funcoes.GravaAuditoria(0, Request.ServerVariables["REMOTE_ADDR"].ToString(), "Cobranças - Boleto Avulso", "Nova Cobrança");
        ControleOpcoes();

        divDadosCobranca.Visible = true;
        divDadosCliente.Visible = true;

        divDetalhesCobranca.Visible = true;
        lblTituloPadraoTopo.Text = "Gerar Boleto Avulso - Cliente";

        divBoleto.Visible = true;
    }

    protected void btnReceberQRCode_Click(object sender, EventArgs e)
    {
        Funcoes.GravaAuditoria(0, Request.ServerVariables["REMOTE_ADDR"].ToString(), "Cobranças - Receber por QRCode", "Nova Cobrança");
        ControleOpcoes();

        divDadosCobranca.Visible = true;
        divDadosCliente.Visible = true;

        divDetalhesCobranca.Visible = true;
        lblTituloPadraoTopo.Text = "Receber por QR Code - Cliente";

        divPixQRCode.Visible = true;
        //divBoleto.Visible = true;
    }

    protected void btnLinkPagamento_Click(object sender, EventArgs e)
    {
        ControleOpcoes();
        Funcoes.GravaAuditoria(0, Request.ServerVariables["REMOTE_ADDR"].ToString(), "Cobranças - Link Pagamento", "Nova Cobrança");
        divDadosCobranca.Visible = true;
        //divDadosCliente.Visible = true;
        divDetalhesCobranca.Visible = true;

        lblTituloPadraoTopo.Text = "Criar Link de Pagamento - Cliente";
        divLinkPagamento.Visible = true;
    }

    protected void btnVendaDigitada_Click(object sender, System.EventArgs e)
    {
        ControleOpcoes();
        Funcoes.GravaAuditoria(0, Request.ServerVariables["REMOTE_ADDR"].ToString(), "Cobranças - Venda Digitada", "Nova Cobrança");
        divDadosCobranca.Visible = true;
        divDadosCliente.Visible = true;
        ddlFormaPagamento_SelectedIndexChanged(null, null);

        lblTituloPadraoTopo.Text = "Criar Venda Digitada";
        divVendaDigitada.Visible = true;
    }


    protected void btnCarne_Click(object sender, System.EventArgs e)
    {
        ControleOpcoes();
        Funcoes.GravaAuditoria(0, Request.ServerVariables["REMOTE_ADDR"].ToString(), "Cobranças - Carnê de Pagamento", "Nova Cobrança");
        divDadosCobranca.Visible = true;
        divDadosCliente.Visible = true;

        lblTituloPadraoTopo.Text = "Criar Carnê de Pagamento";
        divCarne.Visible = true;

    }
    protected void btnAssinatura_Click(object sender, System.EventArgs e)
    {
        ControleOpcoes();
        Funcoes.GravaAuditoria(0, Request.ServerVariables["REMOTE_ADDR"].ToString(), "Cobranças - Assinatura Recorrente", "Nova Cobrança");
        
        divDadosCobranca.Visible = true;
        divDadosCliente.Visible = true;
        divVendaDigitada.Visible = true;
        
//        ddlFormaPagamento.SelectedValue = "À Vista";
        ddlFormaPagamento.Enabled = false;
//        ddlParcelas.SelectedValue = "1";
        ddlParcelas.Enabled = false;

        lblTituloPadraoTopo.Text = "Criar Assinatura Recorrente";
        divAssinatura.Visible = true;
    }

    private void ControleOpcoes()
    {
        AtualizaListaClientes();

        divDadosCobranca.Visible = false;
        divDadosCliente.Visible = false;
        divDetalhesCobranca.Visible = false;

        lblTituloPadraoTopo.Text = "";

        // Opções
        
        divGestao.Visible = false;
        divBoleto.Visible = false;
        divLinkPagamento.Visible = false;
        divVendaDigitada.Visible = false;
        divPixQRCode.Visible = false;
        divCarne.Visible = false;
        divAssinatura.Visible = false;

        // Gerados

        divBoletoGerado.Visible = false;
        divQRCode.Visible = false;
        // Rotinas Padrões

        ddlClientePadrao.SelectedValue = "";

        txtVencimento.Text = DateTime.Now.ToShortDateString();
        txtDataLimite.Text = DateTime.Now.AddYears(1).ToShortDateString();
        txtQtdeCobranca.Text = "12";
        txtValor.Text = "0,00";
        txtDias.Text = "2";
        txtDescricao.Text = "";
        txtReferencia.Text = "";

        hrfBoleto.HRef = "";

    }
    // -----------------------------------------------------------------------------------------------------------------------------------------------------------------
    // CLIENTE
    // -----------------------------------------------------------------------------------------------------------------------------------------------------------------

    protected void ddlClientePadrao_SelectedIndexChanged(object sender, System.EventArgs e)
    {
        CarregaDadosCliente();
    }

    private void GravaCliente(string sIDCliente)
    {
        SqlConnection connInsConsCliente = new SqlConnection(Funcoes.conexao());
        connInsConsCliente.Open();
        SqlCommand cmdInsConsCliente = new SqlCommand("dbo.stp_compradores_ins", connInsConsCliente);
        cmdInsConsCliente.CommandType = CommandType.StoredProcedure;
        cmdInsConsCliente.Parameters.Add("@flg_operacao", SqlDbType.Char).Value = 'I';
        cmdInsConsCliente.Parameters.Add("@FLG_ATIVO", SqlDbType.Char).Value = "S";
        cmdInsConsCliente.Parameters.Add("@COD_ID_PESSOA_LICENCIADO", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["LICENCIADO"].ToString());
        cmdInsConsCliente.Parameters.Add("@COD_ID_PESSOAS_FJ", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["PESSOA"].ToString());
        cmdInsConsCliente.Parameters.Add("@COD_ID_COMPRADOR", SqlDbType.VarChar).Value = sIDCliente.ToString();

        cmdInsConsCliente.Parameters.Add("@DTA_DATA", SqlDbType.DateTime).Value = DateTime.Now;
        cmdInsConsCliente.Parameters.Add("@FLG_TIPO", SqlDbType.Char).Value = "C";
        cmdInsConsCliente.Parameters.Add("@NOM_NOME", SqlDbType.VarChar).Value = txtNome.Text.ToString();
        cmdInsConsCliente.Parameters.Add("@NOM_CEP", SqlDbType.VarChar).Value = txtCEP.Text.ToString();
        cmdInsConsCliente.Parameters.Add("@NOM_NUMERO", SqlDbType.VarChar).Value = txtNumero.Text.ToString();
        cmdInsConsCliente.Parameters.Add("@NOM_CELULAR", SqlDbType.VarChar).Value = txtCelular.Text.ToString();
        cmdInsConsCliente.Parameters.Add("@NUM_CNPJCPF", SqlDbType.VarChar).Value = Funcoes.TIRAACENTOSDOCUMENTOS(txtCNPJCPF.Text.ToString());
        cmdInsConsCliente.Parameters.Add("@NOM_EMAIL", SqlDbType.VarChar).Value = txtEmail.Text.ToString();

        cmdInsConsCliente.ExecuteNonQuery();
        connInsConsCliente.Close();
        connInsConsCliente.Dispose();
    }

    private void CarregaDadosCliente()
    {
        SqlConnection mySelCadastro = new SqlConnection(Funcoes.conexao());
        mySelCadastro.Open();
        SqlCommand cmdSelCadastro = new SqlCommand("dbo.stp_compradores_ins", mySelCadastro);
        cmdSelCadastro.CommandType = CommandType.StoredProcedure;
        cmdSelCadastro.Parameters.Add("@flg_operacao", SqlDbType.Char).Value = "S";
        cmdSelCadastro.Parameters.Add("@COD_ID", SqlDbType.Int).Value = Funcoes.strToInt(ddlClientePadrao.SelectedValue.ToString());
        cmdSelCadastro.Parameters.Add("@COD_ID_PESSOA_LICENCIADO", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["LICENCIADO"].ToString());
        cmdSelCadastro.Parameters.Add("@COD_ID_PESSOAS_FJ", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["PESSOA"].ToString());
        SqlDataReader ReaderCadastro = cmdSelCadastro.ExecuteReader();
        while (ReaderCadastro.Read())
        {
            txtNome.Text = ReaderCadastro["NOM_NOME"].ToString();
            txtCNPJCPF.Text = ReaderCadastro["NUM_CNPJCPF"].ToString();
            txtEmail.Text = ReaderCadastro["NOM_EMAIL"].ToString();
            txtCEP.Text = ReaderCadastro["NOM_CEP"].ToString();
            txtCelular.Text = ReaderCadastro["NOM_CELULAR"].ToString();
            txtNumero.Text = ReaderCadastro["NOM_NUMERO"].ToString();
        }
    }

    private void AtualizaListaClientes()
    {
        // Tabela de Compradores

        SqlConnection myCompradores = new SqlConnection(Funcoes.conexao());
        myCompradores.Open();
        SqlCommand cmdCompradores = new SqlCommand("dbo.stp_compradores_ins", myCompradores);
        cmdCompradores.CommandType = CommandType.StoredProcedure;
        cmdCompradores.Parameters.Add("@flg_operacao", SqlDbType.VarChar).Value = "C";
        cmdCompradores.Parameters.Add("@COD_ID_PESSOA_LICENCIADO", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["LICENCIADO"].ToString());
        cmdCompradores.Parameters.Add("@COD_ID_PESSOAS_FJ", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["PESSOA"].ToString());
        cmdCompradores.Parameters.Add("@NOM_NOME", SqlDbType.VarChar).Value = "";
        cmdCompradores.Parameters.Add("@NUM_CNPJCPF", SqlDbType.VarChar).Value = "";
        cmdCompradores.Parameters.Add("@FLG_TIPO", SqlDbType.Char).Value = "C";


        SqlDataAdapter drCompradores = new SqlDataAdapter();
        drCompradores.SelectCommand = cmdCompradores;
        DataSet dsCompradores = new DataSet();
        drCompradores.Fill(dsCompradores, "PESSOAS_FJ");
        ddlClientePadrao.DataTextField = "NOM_NOME";
        ddlClientePadrao.DataValueField = "COD_ID";
        ddlClientePadrao.DataSource = dsCompradores.Tables["PESSOAS_FJ"].DefaultView;
        ddlClientePadrao.DataBind();
        ddlClientePadrao.Items.Insert(0, new ListItem("", ""));

    }



    // -----------------------------------------------------------------------------------------------------------------------------------------------------------------
    // GESTÃO COBRANÇAS
    // -----------------------------------------------------------------------------------------------------------------------------------------------------------------
    protected void btnPesquisarGestao_Click(object sender, System.EventArgs e)
    {
        ConsultaGestaoCobrancas();
    }

    private void ConsultaGestaoCobrancas()
    {
        SqlConnection myConsulta = new SqlConnection(Funcoes.conexao());
        myConsulta.Open();
        SqlDataAdapter SDAConsulta = new SqlDataAdapter("dbo.stp_cobrancas_ins", myConsulta);
        SDAConsulta.SelectCommand.CommandType = CommandType.StoredProcedure;
        SDAConsulta.SelectCommand.Parameters.Add("@flg_operacao", SqlDbType.Char).Value = "C";
        SDAConsulta.SelectCommand.Parameters.Add("@COD_ID_PESSOA_LICENCIADO", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["LICENCIADO"].ToString());
        SDAConsulta.SelectCommand.Parameters.Add("@COD_ID_PESSOAS_FJ_VENDEDOR", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["PESSOA"].ToString());
        SDAConsulta.SelectCommand.Parameters.Add("@DTA_INICIO", SqlDbType.DateTime).Value = Convert.ToDateTime(txtDataIni.Text.ToString());
        SDAConsulta.SelectCommand.Parameters.Add("@DTA_FIM", SqlDbType.DateTime).Value = Convert.ToDateTime(txtDataFim.Text.ToString());

        DataSet dsConsulta = new DataSet();
        SDAConsulta.Fill(dsConsulta, "COBRANCAS");
        rptConsulta.DataSource = dsConsulta.Tables["COBRANCAS"].DefaultView;
        rptConsulta.DataBind();
        myConsulta.Close(); myConsulta.Dispose();

        foreach (RepeaterItem itemE in rptConsulta.Items)
        {

            if (((TextBox)itemE.FindControl("txttipo")).Text.ToString().Trim() == "Transferência PIX")
            {
                if (((TextBox)itemE.FindControl("txtcancelado")).Text.ToString().Trim() == "N")
                {
                    ((LinkButton)itemE.FindControl("lbkCancelar")).Visible = true;
                }
                else
                {
                    ((LinkButton)itemE.FindControl("lbkCancelar")).Visible = false;
                }
                ((LinkButton)itemE.FindControl("lbkStatus")).Visible = true;
            }
            else
            {
                ((LinkButton)itemE.FindControl("lbkCancelar")).Visible = false;
                ((LinkButton)itemE.FindControl("lbkStatus")).Visible = false;
            }

            
        }


    }

    protected void rptConsulta_OnItemCommand(object source, System.Web.UI.WebControls.RepeaterCommandEventArgs e)
    {

        if (e.CommandName == "Status")
        {
            foreach (RepeaterItem item in rptConsulta.Items)
            {
                if (Funcoes.strToInt(e.CommandArgument.ToString()) == Funcoes.strToInt(((TextBox)item.FindControl("txtid")).Text.ToString()))
                {
                    string sToken = asaas.PegarTokenSubconta(Funcoes.strToInt(HttpContext.Current.Session["PESSOA"].ToString()),Funcoes.strToInt(HttpContext.Current.Session["LICENCIADO"].ToString()));
                    string jsonStatus = asaas.ConsultaTransferencia(sToken, ((TextBox)item.FindControl("txtidcobranca")).Text.ToString());
                    if (jsonStatus.ToString().Trim() != "")
                    {
                        try
                        {
                            JObject oStatus = JObject.Parse(jsonStatus.ToString());
                            // Atualizar Status
                            SqlConnection connInsConsUPD = new SqlConnection(Funcoes.conexao());
                            connInsConsUPD.Open();
                            SqlCommand cmdInsConsUPD = new SqlCommand("dbo.stp_cobrancas_ins", connInsConsUPD);
                            cmdInsConsUPD.CommandType = CommandType.StoredProcedure;
                            cmdInsConsUPD.Parameters.Add("@flg_operacao", SqlDbType.Char).Value = "T";
                            cmdInsConsUPD.Parameters.Add("@COD_ID_PESSOA_LICENCIADO", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["LICENCIADO"].ToString());
                            cmdInsConsUPD.Parameters.Add("@COD_ID", SqlDbType.Int).Value = Funcoes.strToInt(e.CommandArgument.ToString());
                            cmdInsConsUPD.Parameters.Add("@NOM_NOME", SqlDbType.VarChar).Value = oStatus["bankAccount"]["ownerName"].ToString();
                            cmdInsConsUPD.Parameters.Add("@NUM_CNPJCPF", SqlDbType.VarChar).Value = oStatus["bankAccount"]["cpfCnpj"].ToString();
                            cmdInsConsUPD.Parameters.Add("@FLG_STATUS", SqlDbType.VarChar).Value = oStatus["status"].ToString();
                            cmdInsConsUPD.ExecuteNonQuery();
                            connInsConsUPD.Close();
                            connInsConsUPD.Dispose();


                        }
                        catch
                        {

                        }
                    }
                }
            }
            ConsultaGestaoCobrancas();
        }

        if (e.CommandName == "Cancelar")
        {
            foreach (RepeaterItem item in rptConsulta.Items)
            {
                if (Funcoes.strToInt(e.CommandArgument.ToString()) == Funcoes.strToInt(((TextBox)item.FindControl("txtid")).Text.ToString()))
                {
                    string sToken = asaas.PegarTokenSubconta(Funcoes.strToInt(HttpContext.Current.Session["PESSOA"].ToString()), Funcoes.strToInt(HttpContext.Current.Session["LICENCIADO"].ToString()));
                    string jsonCancelar = asaas.CancelarTransferencia(sToken, ((TextBox)item.FindControl("txtidcobranca")).Text.ToString());

                    if (jsonCancelar.ToString().Trim() != "")
                    {
                        try
                        {
                            JObject oCancelar = JObject.Parse(jsonCancelar.ToString());
                            // Atualizar Status
                            SqlConnection connInsConsUPD = new SqlConnection(Funcoes.conexao());
                            connInsConsUPD.Open();
                            SqlCommand cmdInsConsUPD = new SqlCommand("dbo.stp_cobrancas_ins", connInsConsUPD);
                            cmdInsConsUPD.CommandType = CommandType.StoredProcedure;
                            cmdInsConsUPD.Parameters.Add("@flg_operacao", SqlDbType.Char).Value = "X";
                            cmdInsConsUPD.Parameters.Add("@COD_ID_PESSOA_LICENCIADO", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["LICENCIADO"].ToString());
                            cmdInsConsUPD.Parameters.Add("@COD_ID", SqlDbType.Int).Value = Funcoes.strToInt(e.CommandArgument.ToString());
                            cmdInsConsUPD.Parameters.Add("@NOM_NOME", SqlDbType.VarChar).Value = oCancelar["bankAccount"]["ownerName"].ToString();
                            cmdInsConsUPD.Parameters.Add("@NUM_CNPJCPF", SqlDbType.VarChar).Value = oCancelar["bankAccount"]["cpfCnpj"].ToString();
                            cmdInsConsUPD.Parameters.Add("@FLG_STATUS", SqlDbType.VarChar).Value = oCancelar["status"].ToString();
                            cmdInsConsUPD.ExecuteNonQuery();
                            connInsConsUPD.Close();
                            connInsConsUPD.Dispose();


                        }
                        catch
                        {

                        }
                    }
                }
            }
            ConsultaGestaoCobrancas();
        }
        
        if (e.CommandName == "Atualizar")
        {
            foreach (RepeaterItem item in rptConsulta.Items)
            {
                if (Funcoes.strToInt(e.CommandArgument.ToString()) == Funcoes.strToInt(((TextBox)item.FindControl("txtid")).Text.ToString()))
                {

                    SqlConnection connInsConsUPD = new SqlConnection(Funcoes.conexao());
                    connInsConsUPD.Open();
                    SqlCommand cmdInsConsUPD = new SqlCommand("dbo.stp_cobrancas_ins", connInsConsUPD);
                    cmdInsConsUPD.CommandType = CommandType.StoredProcedure;
                    cmdInsConsUPD.Parameters.Add("@flg_operacao", SqlDbType.Char).Value = 'B';
                    cmdInsConsUPD.Parameters.Add("@COD_ID_PESSOA_LICENCIADO", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["LICENCIADO"].ToString());
                    cmdInsConsUPD.Parameters.Add("@COD_ID_PESSOAS_FJ_VENDEDOR", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["PESSOA"].ToString());
                    cmdInsConsUPD.Parameters.Add("@COD_ID", SqlDbType.Int).Value = Funcoes.strToInt(e.CommandArgument.ToString());
                    cmdInsConsUPD.Parameters.Add("@NOM_URL_BOLETO", SqlDbType.VarChar).Value = "https://" + Request.ServerVariables["SERVER_NAME"].ToString() + "/boletocobranca.aspx?id=" + Funcoes.Encrypt(((TextBox)item.FindControl("txtidcobranca")).Text.ToString());

                    cmdInsConsUPD.ExecuteNonQuery();
                    connInsConsUPD.Close();
                    connInsConsUPD.Dispose();
                }
            }
            ClientScript.RegisterStartupScript(this.GetType(),
                "Alerta", "alert('Cobrança atualizada com sucesso!');", true);
            ConsultaGestaoCobrancas();

        }




        if (e.CommandName == "Excluir")
        {
            foreach (RepeaterItem item in rptConsulta.Items)
            {
                if (Funcoes.strToInt(e.CommandArgument.ToString()) == Funcoes.strToInt(((TextBox)item.FindControl("txtid")).Text.ToString()))
                {
                    // Excluir cobrança AsaaS
                    string jsonRetorno = asaas.ExcluirCobranca(asaas.PegarTokenSubconta(Funcoes.strToInt(HttpContext.Current.Session["PESSOA"].ToString()), Funcoes.strToInt(HttpContext.Current.Session["LICENCIADO"].ToString())), ((TextBox)item.FindControl("txtidcobranca")).Text.ToString());

                    if (jsonRetorno.ToString().Trim() != "")
                    {
                        try
                        {
                            JObject oExcluirCobranca = JObject.Parse(jsonRetorno.ToString());
                            if (Convert.ToBoolean(oExcluirCobranca["deleted"].ToString()) == true)
                            {
                                SqlConnection connInsConsUPD = new SqlConnection(Funcoes.conexao());
                                connInsConsUPD.Open();
                                SqlCommand cmdInsConsUPD = new SqlCommand("dbo.stp_cobrancas_ins", connInsConsUPD);
                                cmdInsConsUPD.CommandType = CommandType.StoredProcedure;
                                cmdInsConsUPD.Parameters.Add("@flg_operacao", SqlDbType.Char).Value = 'E';
                                cmdInsConsUPD.Parameters.Add("@COD_ID_COBRANCA", SqlDbType.VarChar).Value = ((TextBox)item.FindControl("txtidcobranca")).Text.ToString();
                                cmdInsConsUPD.ExecuteNonQuery();
                                connInsConsUPD.Close();
                                connInsConsUPD.Dispose();

                                ClientScript.RegisterStartupScript(this.GetType(), "Alerta", "alert('Cobrança excluída com sucesso!');", true);
                            }
                            else
                            {
                                ClientScript.RegisterStartupScript(this.GetType(), "Alerta", "alert('Não foi possível excluir a cobrança! Verifique e tente novamente."+jsonRetorno.ToString()+"');", true);
                            }
                        }
                        catch
                        {
                            ClientScript.RegisterStartupScript(this.GetType(), "Alerta", "alert('Não foi possível excluir a cobrança! Verifique e tente novamente." + jsonRetorno.ToString() + "');", true);
                        }
                    }
                    else
                    {
                        ClientScript.RegisterStartupScript(this.GetType(), "Alerta", "alert('Não foi possível excluir a cobrança! Verifique e tente novamente." + jsonRetorno.ToString() + "');", true);
                    }
                }
            }
            ConsultaGestaoCobrancas();

        }


    }
    // -----------------------------------------------------------------------------------------------------------------------------------------------------------------
    // BOLETO BANCÁRIO
    // -----------------------------------------------------------------------------------------------------------------------------------------------------------------

    protected void btnContinuarBoletos_Click(object sender, System.EventArgs e)
    {
        try
        {
            btnContinuarBoletos.Visible = false;
            btnVoltarBoletos.Visible = true;
            
            // Rotina Padrao Referente ao CLIENTE

            string sIDCliente = asaas.VerificaCliente("", "", txtNome.Text.ToString(), txtCNPJCPF.Text.ToString(), txtEmail.Text.ToString(), txtCEP.Text.ToString(),txtNumero.Text.ToString(), txtCelular.Text.ToString());

            if (sIDCliente.ToString().Trim() != "")
            {
                GravaCliente(sIDCliente.ToString());
                
                
                string jsonURLCobranca = GerarCobranca(sIDCliente, txtValor.Text.ToString(), txtVencimento.Text.ToString(), txtDias.Text.ToString(), txtDescricao.Text.ToString(), txtReferencia.Text.ToString(), "BOLETO");

                divBoletoGerado.Visible = true;

                if (jsonURLCobranca.ToString().Trim() != "")
                {
                    JObject oURLCobranca = JObject.Parse(jsonURLCobranca.ToString());


                    string sURLBoleto = "";
                    try
                    {
                        sURLBoleto = "https://" + Request.ServerVariables["SERVER_NAME"].ToString() + "/linkpagamento.aspx?id=" + Funcoes.Encrypt(oURLCobranca["id"].ToString()) + "&tipo=" + Funcoes.Encrypt("B");
                    }
                    catch
                    {
                        ClientScript.RegisterStartupScript(this.GetType(), "Mensagem", "alert('Não foi possível gerar o boleto bancário! " + oURLCobranca["errors"][0]["description"] + "');", true);
                        return;
                    }

                    // Gravar dados na base de dados
                    GravaCobranca(sIDCliente.ToString(), oURLCobranca["id"].ToString(), jsonURLCobranca.ToString(), oURLCobranca["status"].ToString(),"Boleto Bancário Avulso","A");

                    txtURLBoletoBancario.Text = sURLBoleto.ToString();
                    hrfBoleto.HRef = sURLBoleto.ToString();

                    if (ckbBEmail.Checked == true)
                    {
                        transacoes.EnviarLinkBoletoEmail(sURLBoleto.ToString(), "", txtNome.Text.ToString(),txtDescricao.Text.ToString(),txtVencimento.Text.ToString(),txtValor.Text.ToString(), txtEmail.Text.ToString());
                    }
                }
                else
                {
                    ClientScript.RegisterStartupScript(this.GetType(), "EnviarDadosBoleto", "alert('Não foi possível gerar o boleto bancário! Verifique e tente novamente.');", true);
                }
            }
            else
            {
                ClientScript.RegisterStartupScript(this.GetType(), "ErroGerarBoletoCliente", "alert('Não foi possível gerar o boleto bancário! Verifique e tente novamente.');", true);
            }
        }
        catch
        {
            ClientScript.RegisterStartupScript(this.GetType(), "ErroGeral", "alert('Não foi possível gerar o boleto bancário! Verifique e tente novamente.');", true);
        }
    }

    private void EnviarLinkBoletoEmail(string sURLBoleto)
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

            mail.Subject = "Link de pagamento - " + HttpContext.Current.Session["URLORIGEM"].ToString();
            mail.SubjectEncoding = System.Text.Encoding.UTF8;

            // Construção do CORPO da MENSAGEM (Body)
            mail.Body = "";
            mail.Body = mail.Body + "<b>Olá,</b> " + txtNome.Text.ToString() + "<br /><br />";
            mail.Body = mail.Body + "Segue abaixo o link de pagamento referente:<br />";
            mail.Body = mail.Body + "" + txtDescricao.Text.ToString() + "<br />";
            mail.Body = mail.Body + "<b>Para visualizar o link de pagamento basta clicar no link abaixo<br /><br>";
            mail.Body = mail.Body + "<a href='" + sURLBoleto.ToString() + "' target='_blank'>Clique aqui para visualizar o link de pagamento</a><br /><br />";
            mail.Body = mail.Body + HttpContext.Current.Session["EMAILRODAPE"].ToString();

            mail.BodyEncoding = System.Text.Encoding.UTF8;
            mail.IsBodyHtml = true;
            mail.Priority = MailPriority.High;

            try
            {
                client.Send(mail);
                ClientScript.RegisterStartupScript(this.GetType(),
        "Codigo2fa", "alert('O e-mail da cobrança foi enviado com sucesso!');", true);
                mail = null;
            }
            catch (System.Exception erro)
            {
                ClientScript.RegisterStartupScript(this.GetType(),
        "Erro2fa", "alert('Ocorreu um erro ao enviar!');", true);
            }
        }
        catch
        {
            ClientScript.RegisterStartupScript(this.GetType(),
    "Codigo2faErro", "alert('Ocorreu um erro ao tentar enviar o e-mail da cobrança!');", true);

        }

    }

    protected void btnVoltarBoletos_Click(object sender, System.EventArgs e)
    {
        ControleOpcoes();

        btnContinuarBoletos.Visible = true;
        btnVoltarBoletos.Visible = false;
        btnBoletoAvulso_Click(null, null);
    }

    // -----------------------------------------------------------------------------------------------------------------------------------------------------------------
    // QRCODE
    // -----------------------------------------------------------------------------------------------------------------------------------------------------------------


    // -----------------------------------------------------------------------------------------------------------------------------------------------------------------
    // LINK PAGAMENTO
    // -----------------------------------------------------------------------------------------------------------------------------------------------------------------



    // -----------------------------------------------------------------------------------------------------------------------------------------------------------------
    // VENDA DIGITADA
    // -----------------------------------------------------------------------------------------------------------------------------------------------------------------


    // -----------------------------------------------------------------------------------------------------------------------------------------------------------------
    // CARNÊ DE PAGAMENTO
    // -----------------------------------------------------------------------------------------------------------------------------------------------------------------


    // -----------------------------------------------------------------------------------------------------------------------------------------------------------------
    // ASSINATURA RECORRENTE
    // -----------------------------------------------------------------------------------------------------------------------------------------------------------------

    protected void ddlFormaPagamento_SelectedIndexChanged(object sender, System.EventArgs e)
    {
        if (ddlFormaPagamento.SelectedValue.ToString() == "V")
        {
            ddlParcelas.SelectedValue = "1";
            ddlParcelas.Enabled = false;
        }
        else
        {
            ddlParcelas.SelectedValue = "1";
            ddlParcelas.Enabled = true;
        }

    }
    protected void btnContinuarVendaDigitada_Click(object sender, System.EventArgs e)
    {
        if (ddlAdquirentes.SelectedValue.ToString() == "Z")
        {
            CobrancaZoop("DIRETA");
        }
        if (ddlAdquirentes.SelectedValue.ToString() == "P")
        {

        }
        if (ddlAdquirentes.SelectedValue.ToString() == "A")
        {
            CobrancaAsaas("DIRETA");
        }
        if (ddlAdquirentes.SelectedValue.ToString() == "C")
        {

        }

    }

    // -----------------------------------------------------------------------------------------------------------------------------------------------------------------
    // ROTINAS ADQUIRENTES
    // -----------------------------------------------------------------------------------------------------------------------------------------------------------------
    private void CobrancaAsaas(string sTipo)
    {
        string jsonRetorno = "";
        // -------------------------------------------------------------------------------------------------------------------------------------------------------------------
        // VENDA DIRETA ASAAS
        // -------------------------------------------------------------------------------------------------------------------------------------------------------------------

        if (sTipo == "DIRETA")
        {
            string sIDCliente = asaas.VerificaCliente("", "", txtNome.Text.ToString(), txtCNPJCPF.Text.ToString(), txtEmail.Text.ToString(), txtCEP.Text.ToString(), txtNumero.Text.ToString(), txtCelular.Text.ToString());

            if (sIDCliente.ToString().Trim() != "")
            {
                GravaCliente(sIDCliente.ToString());

                DadosCobrancaCartaoAsaaS.Root dcobranca = new DadosCobrancaCartaoAsaaS.Root()
                {
                    customer = sIDCliente.ToString(),
                    billingType = "CREDIT_CARD",
                    value = Funcoes.strToDouble(txtValor.Text.ToString()),
                    dueDate = Convert.ToDateTime(txtVencimento.Text.ToString()).Year.ToString() + "-" + Convert.ToDateTime(txtVencimento.Text.ToString()).Month.ToString() + "-" + Convert.ToDateTime(txtVencimento.Text.ToString()).Day.ToString(),
                    creditCard = new DadosCobrancaCartaoAsaaS.CreditCard()
                    {
                        holderName = txtPortador.Text.ToString(),
                        number = txtNumeroCartao.Text.ToString(),
                        ccv = txtCVV.Text.ToString(),
                        expiryYear = txtAno.Text.ToString().PadLeft(4, '0'),
                        expiryMonth = txtMes.Text.ToString().PadLeft(1, '0')
                    },
                    creditCardHolderInfo = new DadosCobrancaCartaoAsaaS.CreditCardHolderInfo()
                    {
                        cpfCnpj = txtCNPJCPF.Text.ToString(),
                        name = txtNome.Text.ToString(),
                        email = txtEmail.Text.ToString(),
                        postalCode = txtCEP.Text.ToString(),
                        mobilePhone = txtCelular.Text.ToString(),
                        phone = txtCelular.Text.ToString(),
                        addressNumber = txtNumero.Text.ToString()
                    }
                };
                
                string jsonVendaDigitada = JsonConvert.SerializeObject(dcobranca);
                jsonRetorno = asaas.CriarCobranca(asaas.PegarTokenSubconta(Funcoes.strToInt(HttpContext.Current.Session["PESSOA"].ToString()), Funcoes.strToInt(HttpContext.Current.Session["LICENCIADO"].ToString())), jsonVendaDigitada);
                //txtNome.Text = jsonRetorno;
                

                if (jsonRetorno.ToString().Trim() != "")
                {
                    
                    JObject oURLVendaDigitada = JObject.Parse(jsonRetorno.ToString());
                    try
                    {
                        // Gravar dados na base de dados
                        GravaCobranca(sIDCliente.ToString(), oURLVendaDigitada["id"].ToString(), oURLVendaDigitada.ToString(), oURLVendaDigitada["status"].ToString(), "Venda Digitada","A");
                        ClientScript.RegisterStartupScript(this.GetType(), "SucessoVENDADIGITADA", "alert('Cobrança por Venda Digitada gerada com sucesso!');", true);

                    }
                    catch
                    {
                        string sErrosCobranca = "";
                        for (int i = 0; i < oURLVendaDigitada["errors"].Count(); i++)
                        {
                            sErrosCobranca = sErrosCobranca + " ( " + oURLVendaDigitada["errors"][i]["description"].ToString() + " ) ";
                        }
                        ClientScript.RegisterStartupScript(this.GetType(), "Mensagensdeerro", "alert('Ocorreu um erro ao tentar gerar a venda digitada: "+ sErrosCobranca+"');", true);
                    }
                }
                else
                {
                    ClientScript.RegisterStartupScript(this.GetType(), "ErroVendaDireta", "alert('Ocorreu um erro ao tentar gerar a venda digitada! Verifique e tente novamente.');", true);
                }
                
            }
            else
            {
                ClientScript.RegisterStartupScript(this.GetType(), "ErroVendaDiretaGeral", "alert('Não foi possível gerar a venda digitada! Verifique e tente novamente.');", true);
            }
        }
        // -------------------------------------------------------------------------------------------------------------------------------------------------------------------
        // BOLETO ASAAS
        // -------------------------------------------------------------------------------------------------------------------------------------------------------------------

        if (sTipo == "BOLETO")
        {


        }

        // -------------------------------------------------------------------------------------------------------------------------------------------------------------------
        // QRCODE ASAAS
        // -------------------------------------------------------------------------------------------------------------------------------------------------------------------

        if (sTipo == "QRCODE")
        {
            string sIDCliente = asaas.VerificaCliente("", "", txtNome.Text.ToString(), txtCNPJCPF.Text.ToString(), txtEmail.Text.ToString(), txtCEP.Text.ToString(), txtNumero.Text.ToString(), txtCelular.Text.ToString());

            if (sIDCliente.ToString().Trim() != "")
            {
                GravaCliente(sIDCliente.ToString());

                //txtResposta.Text = txtResposta.Text + sIDCliente.ToString();

                string jsonCobranca = "";
                divResposta.Visible = false;

                try
                {
                    DadosCobrancaAsaaS.Root dcobranca = new DadosCobrancaAsaaS.Root()
                    {
                        customer = sIDCliente.ToString(),
                        billingType = "PIX",
                        value = Funcoes.strToDouble(txtValor.Text.ToString()),
                        dueDate = Convert.ToDateTime(txtVencimento.Text.ToString()).Year.ToString() + "-" + Convert.ToDateTime(txtVencimento.Text.ToString()).Month.ToString() + "-" + Convert.ToDateTime(txtVencimento.Text.ToString()).Day.ToString(),
                        daysAfterDueDateToRegistrationCancellation = Funcoes.strToInt(txtDias.Text.ToString()),
                        description = txtDescricao.Text.ToString(),
                        externalReference = txtReferencia.Text.ToString(),
                        /*split = new DadosCobrancaAsaaS.split[] 
                        {
                            new DadosCobrancaAsaaS.split()
                            {
                                walletId = "ff8380b5-4a9e-4890-bd3c-a187ac685bb1",
                                percentualValue = 30
                            },
                                                        new DadosCobrancaAsaaS.split()
                            {
                                walletId = "62214bc8-eb9b-4427-8d04-2286a20fe67a",
                                percentualValue = 30
                            },
                                                        new DadosCobrancaAsaaS.split()
                            {
                                walletId = "95e34de4-4ffb-4e46-870a-86d8ebf5af81",
                                percentualValue = 30
                            },
                            new DadosCobrancaAsaaS.split()
                            {
                                walletId = "3cc18c1d-8dde-489a-be47-d01de37da2d1",
                                percentualValue = 10
                            },


                        }*/
                    };



                    SqlConnection mySelCadastroSplit = new SqlConnection(Funcoes.conexao());
                    mySelCadastroSplit.Open();
                    SqlCommand cmdSelCadastroSplit = new SqlCommand("dbo.stp_pessoas_fj_split_ins", mySelCadastroSplit);
                    cmdSelCadastroSplit.CommandType = CommandType.StoredProcedure;
                    cmdSelCadastroSplit.Parameters.Add("@flg_operacao", SqlDbType.Char).Value = "C";
                    cmdSelCadastroSplit.Parameters.Add("@COD_ID_PESSOAS_FJ", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["PESSOA"].ToString());
                    cmdSelCadastroSplit.Parameters.Add("@COD_ID_PESSOA_LICENCIADO", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["LICENCIADO"].ToString());
                    SqlDataReader ReaderCadastroSplit = cmdSelCadastroSplit.ExecuteReader();

                    List<DadosCobrancaAsaaS.split> listaSplits = new List<DadosCobrancaAsaaS.split>();

                    while (ReaderCadastroSplit.Read()) // Percorre os registros do banco
                    {

                        listaSplits.Add(
                        new DadosCobrancaAsaaS.split()
                        {
                            walletId = ReaderCadastroSplit["NUM_WALLETID_BAAS"].ToString(),
                            percentualValue = Funcoes.strToDouble(ReaderCadastroSplit["NUM_PERCENTUAL"].ToString()),
                            fixedValue = Funcoes.strToDouble(ReaderCadastroSplit["NUM_VALOR"].ToString()),
                            description = "Split para " + ReaderCadastroSplit["NOM_RAZAOSOCIAL"].ToString(),
                            externalReference = HttpContext.Current.Session["LICENCIADO"].ToString() + "_" + HttpContext.Current.Session["PESSOA"].ToString(),
                        });
                    }
                    
                    // Converte a lista para array antes de atribuir ao objeto

                    dcobranca.split = listaSplits.ToArray();
                    jsonCobranca = JsonConvert.SerializeObject(dcobranca);

                    txtResposta.Text = txtResposta.Text + jsonCobranca.ToString() ;
                }
                catch
                {
                    ClientScript.RegisterStartupScript(this.GetType(), "Mensagensdeerro", "alert('Ocorreu um erro ao tentar gerar a cobrança por QRCode! Verifique e tente novamente');", true);
                }
                //return;
                jsonRetorno = asaas.CriarCobranca(asaas.PegarTokenSubconta(Funcoes.strToInt(HttpContext.Current.Session["PESSOA"].ToString()), Funcoes.strToInt(HttpContext.Current.Session["LICENCIADO"].ToString())), jsonCobranca);
                txtResposta.Text = txtResposta.Text + jsonRetorno;
                
                if (jsonRetorno.ToString().Trim() != "")
                {
                    JObject oCobranca = JObject.Parse(jsonRetorno.ToString());
                    try
                    {
                        GravaCobranca(sIDCliente.ToString(), oCobranca["id"].ToString(), oCobranca.ToString(), oCobranca["status"].ToString(), "Receber por QRCode","A");
                        string jsonQRCodeCobranca = asaas.ObterQRCodeCobranca(asaas.PegarTokenSubconta(Funcoes.strToInt(HttpContext.Current.Session["PESSOA"].ToString()), Funcoes.strToInt(HttpContext.Current.Session["LICENCIADO"].ToString())), oCobranca["id"].ToString());

                        if (jsonQRCodeCobranca.ToString().Trim() != "")
                        {
                            try
                            {
                                divQRCode.Visible = true;
                                JObject oQRCodeCobranca = JObject.Parse(jsonQRCodeCobranca.ToString());
                                lblPixCopiaCola.Text = oQRCodeCobranca["payload"].ToString();
                                string base64String = oQRCodeCobranca["encodedImage"].ToString(); //encodedImage
                                QRCodeEncoder encoder = new QRCodeEncoder();
                                Bitmap image = encoder.Encode(oQRCodeCobranca["payload"].ToString());
                                image.Save(Server.MapPath("public_html") + "\\" + oCobranca["id"].ToString() + ".bmp");
                                imgQRcode.Src = "../public_html/" + oCobranca["id"].ToString() + ".bmp";
                            }
                            catch
                            {

                            }

                        }

                        string sURLBoleto = "";
                        try
                        {
                            sURLBoleto = "https://" + Request.ServerVariables["SERVER_NAME"].ToString() + "/linkpagamento.aspx?id=" + Funcoes.Encrypt(oCobranca["id"].ToString()) + "&tipo=" + Funcoes.Encrypt("Q");
                        }
                        catch
                        {
                            string sErrosCobranca = "";
                            for (int i = 0; i < oCobranca["errors"].Count(); i++)
                            {
                                sErrosCobranca = sErrosCobranca + " ( " + oCobranca["errors"][i]["description"].ToString() + " ) ";
                            }
                            ClientScript.RegisterStartupScript(this.GetType(), "Mensagensdeerro", "alert('Ocorreu um erro ao tentar gerar o QRCode: " + sErrosCobranca + "');", true);
                        }

                        txtURLQRCode.Text = sURLBoleto.ToString();
                        hrfQRCode.HRef = sURLBoleto.ToString();

                        if (ckbEmailQRCode.Checked == true)
                        {
                            EnviarLinkBoletoEmail(sURLBoleto.ToString());
                        }
                        btnContinuarPixQRCode.Visible = false;
                        btnVoltarPixQRCode.Visible = true;
                        ClientScript.RegisterStartupScript(this.GetType(), "SucessoQRCODE", "alert('Cobrança por QRCode gerada com sucesso!');", true);
                    }
                    catch
                    {
                        string sErrosCobranca = "";
                        for (int i = 0; i < oCobranca["errors"].Count(); i++)
                        {
                            sErrosCobranca = sErrosCobranca + " ( " + oCobranca["errors"][i]["description"].ToString() + " ) ";
                        }
                        ClientScript.RegisterStartupScript(this.GetType(), "Mensagensdeerro", "alert('Ocorreu um erro ao tentar gerar o QRCode: " + sErrosCobranca + "');", true);
                    }
                }
                else
                {
                    ClientScript.RegisterStartupScript(this.GetType(), "ErroVendaDireta", "alert('Ocorreu um erro ao tentar gerar o QRCode! Verifique e tente novamente.');", true);
                }

            }
            else
            {
                ClientScript.RegisterStartupScript(this.GetType(), "ErroVendaDiretaGeral", "alert('Não foi possível gerar o QRCode! Verifique e tente novamente.');", true);
            }

        }
        // -------------------------------------------------------------------------------------------------------------------------------------------------------------------
        // LINK PAGAMENTO ASAAS
        // -------------------------------------------------------------------------------------------------------------------------------------------------------------------
        if (sTipo == "ASSINATURA")
        {
            string sIDCliente = asaas.VerificaCliente("", "", txtNome.Text.ToString(), txtCNPJCPF.Text.ToString(), txtEmail.Text.ToString(), txtCEP.Text.ToString(), txtNumero.Text.ToString(), txtCelular.Text.ToString());

            if (sIDCliente.ToString().Trim() != "")
            {
                GravaCliente(sIDCliente.ToString());
                DadosAssinaturaCartao.Root dcobranca = new DadosAssinaturaCartao.Root()
                {
                    customer = sIDCliente.ToString(),
                    billingType = "CREDIT_CARD",
                    value = Funcoes.strToDouble(txtValor.Text.ToString()),
                    nextDueDate = Convert.ToDateTime(txtVencimento.Text.ToString()).Year.ToString() + "-" + Convert.ToDateTime(txtVencimento.Text.ToString()).Month.ToString() + "-" + Convert.ToDateTime(txtVencimento.Text.ToString()).Day.ToString(),
                    description = txtDescricao.Text.ToString(),
                    cycle = ddlPeriodicidade.SelectedValue.ToString(),
                    remoteIp = Request.ServerVariables["REMOTE_ADDR"].ToString(),
                    creditCard = new DadosAssinaturaCartao.CreditCard()
                    {
                        holderName = txtPortador.Text.ToString(),
                        number = txtNumeroCartao.Text.ToString(),
                        ccv = txtCVV.Text.ToString(),
                        expiryYear = txtAno.Text.ToString().PadLeft(4, '0'),
                        expiryMonth = txtMes.Text.ToString().PadLeft(1, '0')
                    },
                    creditCardHolderInfo = new DadosAssinaturaCartao.CreditCardHolderInfo()
                    {
                        cpfCnpj = txtCNPJCPF.Text.ToString(),
                        name = txtNome.Text.ToString(),
                        email = txtEmail.Text.ToString(),
                        postalCode = txtCEP.Text.ToString(),
                        mobilePhone = txtCelular.Text.ToString(),
                        phone = txtCelular.Text.ToString(),
                        addressNumber = txtNumero.Text.ToString()
                    }
                };

                SqlConnection mySelCadastroSplit = new SqlConnection(Funcoes.conexao());
                mySelCadastroSplit.Open();
                SqlCommand cmdSelCadastroSplit = new SqlCommand("dbo.stp_pessoas_fj_split_ins", mySelCadastroSplit);
                cmdSelCadastroSplit.CommandType = CommandType.StoredProcedure;
                cmdSelCadastroSplit.Parameters.Add("@flg_operacao", SqlDbType.Char).Value = "C";
                cmdSelCadastroSplit.Parameters.Add("@COD_ID_PESSOAS_FJ", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["PESSOA"].ToString());
                cmdSelCadastroSplit.Parameters.Add("@COD_ID_PESSOA_LICENCIADO", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["LICENCIADO"].ToString());
                SqlDataReader ReaderCadastroSplit = cmdSelCadastroSplit.ExecuteReader();

                List<DadosAssinaturaCartao.split> listaSplits = new List<DadosAssinaturaCartao.split>();

                while (ReaderCadastroSplit.Read()) // Percorre os registros do banco
                {

                    listaSplits.Add(
                    new DadosAssinaturaCartao.split()
                    {
                        walletId = ReaderCadastroSplit["NUM_WALLETID_BAAS"].ToString(),
                        percentualValue = Funcoes.strToDouble(ReaderCadastroSplit["NUM_PERCENTUAL"].ToString()),
                        fixedValue = Funcoes.strToDouble(ReaderCadastroSplit["NUM_VALOR"].ToString()),
                        description = "Split para " + ReaderCadastroSplit["NOM_RAZAOSOCIAL"].ToString(),                        
                        externalReference = HttpContext.Current.Session["LICENCIADO"].ToString() + "_" + HttpContext.Current.Session["PESSOA"].ToString(),
                    });
                }

                // Converte a lista para array antes de atribuir ao objeto

                dcobranca.split = listaSplits.ToArray();

                string jsonCobranca = JsonConvert.SerializeObject(dcobranca);
                jsonRetorno = asaas.CriarAssinatura(asaas.PegarTokenSubconta(Funcoes.strToInt(HttpContext.Current.Session["PESSOA"].ToString()), Funcoes.strToInt(HttpContext.Current.Session["LICENCIADO"].ToString())), jsonCobranca,"P");
                txtRetornoAssinatura.Text = jsonRetorno;
                txtRetornoAssinatura.Visible = true;
                if (jsonRetorno.ToString().Trim() != "")
                {
                    JObject oCobranca = JObject.Parse(jsonRetorno.ToString());

                    try
                    {
                        GravaCobranca(sIDCliente.ToString(), oCobranca["id"].ToString(), oCobranca.ToString(), oCobranca["status"].ToString(), "Assinatura","A");
                        btnContinuarAssinatura.Visible = false;
                        btnVoltarAssinatura.Visible = true;

                        ClientScript.RegisterStartupScript(this.GetType(), "SucessoQRCODE", "alert('Cobrança por Assinatura gerada com sucesso!');", true);
                    }
                    catch
                    {
                        try
                        {
                            string sErrosCobranca = "";
                            sErrosCobranca = sErrosCobranca + " ( " + oCobranca["errors"][0]["description"].ToString() + " ) ";
                            ClientScript.RegisterStartupScript(this.GetType(), "Mensagensdeerro", "alert('Ocorreu um erro ao tentar gerar a cobrança por assinatura! Erro: " + sErrosCobranca + "');", true);
                        }
                        catch
                        {
                            ClientScript.RegisterStartupScript(this.GetType(), "Mensagensdeerro", "alert('Ocorreu um erro ao tentar gerar a cobrança por assinatura! Erro não especificado');", true);
                        }

                    }
                }
                else
                {
                    ClientScript.RegisterStartupScript(this.GetType(), "ErroVendaDireta", "alert('Ocorreu um erro ao tentar gerar a cobrança por assinatura! Verifique e tente novamente.');", true);
                }

            }
            else
            {
                ClientScript.RegisterStartupScript(this.GetType(), "ErroVendaDiretaGeral", "alert('Não foi possível gerar a venda digitada! Verifique e tente novamente.');", true);
            }
        }
    }

    private void CobrancaZoop(string sTipo)
    {
        string jsonRetorno = "";
        // -------------------------------------------------------------------------------------------------------------------------------------------------------------------
        // VENDA DIRETA ZOOP
        // -------------------------------------------------------------------------------------------------------------------------------------------------------------------
        if (sTipo == "DIRETA")
        {
            try
            {
                // Criar a Cobrança
                zoopv2.dadosVendaDigitada.Root dvendadigitada = new zoopv2.dadosVendaDigitada.Root()
                {
                    description = txtDescricao.Text.ToString(),
                    on_behalf_of = zoopv2.ConsultaVendedor(),
                    payment_type = "credit",
                    capture = true,
                    reference_id = "",
                    source = new zoopv2.dadosVendaDigitada.source()
                    {
                        usage = "single_use",
                        amount = Funcoes.strToInt(Convert.ToString(Funcoes.strToDouble(txtValor.Text.ToString()) * 100).ToString()),
                        currency = "BRL",
                        type = "card",
                        card = new zoopv2.dadosVendaDigitada.card()
                        {
                            card_number = txtNumeroCartao.Text.ToString(),
                            holder_name = txtPortador.Text.ToString(),
                            expiration_month = txtMes.Text.ToString().PadLeft(1, '0'),
                            expiration_year = txtAno.Text.ToString().PadLeft(4, '0'),
                            security_code = txtCVV.Text.ToString()
                        }
                    },
                    installment_plan = new zoopv2.dadosVendaDigitada.installment_plan()
                    {
                        number_installments = Funcoes.strToInt(ddlParcelas.SelectedValue.ToString())
                    },
                    three_d_secure = new zoopv2.dadosVendaDigitada.three_d_secure()
                    {
                        challenge_type = "DATA_ONLY",
                        ip_address = Request.ServerVariables["REMOTE_ADDR"].ToString(),
                        user_agent = Request.ServerVariables["HTTP_USER_AGENT"].ToString(),
                        on_failure = "continue",
                        device = new zoopv2.dadosVendaDigitada.device()
                        {
                            color_depth = 24,
                            type = "BROWSER",
                            java_enabled = false,
                            language = Request.Headers["Accept-Language"].ToString().Split(';').FirstOrDefault().ToString().Split(',').FirstOrDefault().ToString(),
                            screen_height = (Request.Browser.ScreenPixelsHeight) * 2,
                            screen_width = (Request.Browser.ScreenPixelsWidth) * 2,
                            time_zone_offset = Funcoes.strToInt(DateTimeOffset.Now.Offset.ToString().Split(':').FirstOrDefault().ToString())
                        }
                    }
                };
                string jsonVendaDigitada = JsonConvert.SerializeObject(dvendadigitada);
                jsonRetorno = zoopv2.CriarTransacao(jsonVendaDigitada);

                txtDescricao.Text = jsonRetorno;
                if (jsonRetorno.ToString().Trim() != "")
                {
                    JObject oVendaDireta = JObject.Parse(jsonRetorno.ToString());
                    GravaCobranca("", oVendaDireta["id"].ToString(), jsonRetorno.ToString(), oVendaDireta["status"].ToString(), "Venda Direta","Z");

                    if ((oVendaDireta["status"].ToString().ToString() == "succeeded") || (oVendaDireta["status"].ToString().ToString() == "pre_authorized") || (oVendaDireta["status"].ToString().ToString() == "new"))
                    {
                        ClientScript.RegisterStartupScript(this.GetType(), "SucessoVendaDireta", "alert('Venda direta realizada com sucesso! Verifique e tente novamente.');", true);
                    }
                    else
                    {
                        ClientScript.RegisterStartupScript(this.GetType(), "ErroVendaDireta", "alert('Não foi possível gerar a venda direta! Verifique e tente novamente.');", true);
                    }
                }
            }
            catch
            {
                ClientScript.RegisterStartupScript(this.GetType(), "ErroGeral", "alert('Não foi possível gerar a venda direta! Verifique e tente novamente.');", true);
            }
        }

        // -------------------------------------------------------------------------------------------------------------------------------------------------------------------
        // BOLETO ASAAS
        // -------------------------------------------------------------------------------------------------------------------------------------------------------------------

        if (sTipo == "BOLETO")
        {


        }

        // -------------------------------------------------------------------------------------------------------------------------------------------------------------------
        // QRCODE ASAAS
        // -------------------------------------------------------------------------------------------------------------------------------------------------------------------

        if (sTipo == "QRCODE")
        {


        }
        // -------------------------------------------------------------------------------------------------------------------------------------------------------------------
        // LINK PAGAMENTO ASAAS
        // -------------------------------------------------------------------------------------------------------------------------------------------------------------------


        //succeeded failed canceled pre_authorized reversed refunded pending new partial_refunded dispute charged_back
    }
    protected void btnContinuarPixQRCode_Click(object sender, System.EventArgs e)
    {
        
        if (ddlAdquirentes.SelectedValue.ToString() == "Z")
        {
            CobrancaZoop("QRCODE");
        }
        if (ddlAdquirentes.SelectedValue.ToString() == "P")
        {

        }
        if (ddlAdquirentes.SelectedValue.ToString() == "A")
        {
            CobrancaAsaas("QRCODE");
        }
        if (ddlAdquirentes.SelectedValue.ToString() == "C")
        {

        }
    }
    protected void btnContinuarLinkPagamento_Click(object sender, System.EventArgs e)
    {
        try
        {
            // Verifica se foi selecionada alguma opção de pagamento
            if ((ckbBoleto.Checked == true) || (ckbPix.Checked == true) || (ckbCredito.Checked == true))
            {
                if (Funcoes.strToDouble(txtValor.Text.ToString().Trim()) > 0)
                {
                    btnContinuarLinkPagamento.Visible = false;
                    btnVoltarLinkPagamento.Visible = true;
                    divLinkPagamentoResultado.Visible = true;

                    // Gravar o Link de Venda e pegar o ID
            
                    // 1o. Verifica se o cliente foi especificado (Comprador)
                    Funcoes.GravaAuditoria(0, Request.ServerVariables["REMOTE_ADDR"].ToString(), "Cobranças - Link de Pagamento", "Salvar dados");
                    // Imagem Fundo
                    try
                    {
                        SqlConnection connInsCons = new SqlConnection(Funcoes.conexao());
                        connInsCons.Open();
                        SqlCommand cmdInsCons = new SqlCommand("dbo.stp_vendas_ins", connInsCons);
                        cmdInsCons.CommandType = CommandType.StoredProcedure;
                        cmdInsCons.Parameters.Add("@flg_operacao", SqlDbType.Char).Value = 'I';
                        cmdInsCons.Parameters.Add("@COD_ID_PESSOA_LICENCIADO", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["LICENCIADO"].ToString());
                        cmdInsCons.Parameters.Add("@COD_ID_PESSOAS_FJ_VENDEDOR", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["PESSOA"].ToString());
                        cmdInsCons.Parameters.Add("@FLG_INTEGRACAO", SqlDbType.Char).Value = 'Z';


                        cmdInsCons.Parameters.Add("@NOM_REFERENCIA_PRODUTO", SqlDbType.VarChar).Value = txtReferencia.Text.ToString();
                        cmdInsCons.Parameters.Add("@NOM_DESCRICAO_PRODUTO", SqlDbType.Text).Value = txtDescricao.Text.ToString();

                        cmdInsCons.Parameters.Add("@FLG_LINK_PERMANENTE", SqlDbType.Char).Value = ddlLinkPermanente.SelectedValue.ToString();

                        cmdInsCons.Parameters.Add("@FLG_BOLETO", SqlDbType.Char).Value = (ckbBoleto.Checked == true) ? "S" : "N";
                        cmdInsCons.Parameters.Add("@FLG_PIX", SqlDbType.Char).Value = (ckbPix.Checked == true) ? "S" : "N";
                        cmdInsCons.Parameters.Add("@FLG_CARTAO_CREDITO", SqlDbType.Char).Value = (ckbCredito.Checked == true) ? "S" : "N";

                        cmdInsCons.Parameters.Add("@FLG_TIPO_COBRANCA", SqlDbType.Char).Value = "U";
                        cmdInsCons.Parameters.Add("@FLG_TIPO_OPERACAO", SqlDbType.Char).Value = "C";
                        cmdInsCons.Parameters.Add("@FLG_OPERADORA", SqlDbType.Char).Value = ddlAdquirentes.SelectedValue.ToString();
                        cmdInsCons.Parameters.Add("@FLG_PRECO_PARCELAMENTO", SqlDbType.Char).Value = "N";
                        cmdInsCons.Parameters.Add("@NOM_EMAIL_LINK", SqlDbType.VarChar).Value = "";

                        cmdInsCons.Parameters.Add("@NUM_VALOR", SqlDbType.Float).Value = Funcoes.strToDouble(txtValor.Text.ToString());
                        cmdInsCons.Parameters.Add("@NUM_PARCELAS", SqlDbType.Int).Value = Funcoes.strToInt(ddlParcelas.SelectedValue.ToString());

                        if (txtDataLimite.Text.ToString().Trim() != "") { cmdInsCons.Parameters.Add("@DTA_PIX_VENCIMENTO", SqlDbType.DateTime).Value = Convert.ToDateTime(txtDataLimite.Text.ToString()); }
                        if (txtVencimento.Text.ToString().Trim() != "") { cmdInsCons.Parameters.Add("@DTA_VENCIMENTO", SqlDbType.DateTime).Value = Convert.ToDateTime(txtVencimento.Text.ToString()); }

                        string urlorigem = Request.ServerVariables["SERVER_NAME"].ToString();
                        cmdInsCons.Parameters.Add("@NOM_ORIGEM", SqlDbType.VarChar).Value = urlorigem.ToString() + "/checkout.aspx?id=";
                        string sIDLinkVendas = "";

                        if (ckbLinkPagamentoAvançado.Checked)
                        {
                            cmdInsCons.Parameters.Add("@FLG_CLIENTE", SqlDbType.Char).Value = (ckbCliente.Checked == true) ? "S" : "N";
                            // Cliente
                            cmdInsCons.Parameters.Add("@NOM_NOME", SqlDbType.VarChar).Value = txtNomeCliente.Text.ToString();
                            cmdInsCons.Parameters.Add("@NOM_SOBRENOME", SqlDbType.VarChar).Value = txtSobrenomeCliente.Text.ToString();
                            cmdInsCons.Parameters.Add("@NUM_CNPJCPF", SqlDbType.VarChar).Value = txtDocumentoCliente.Text.ToString();
                            cmdInsCons.Parameters.Add("@NOM_EMAIL", SqlDbType.VarChar).Value = txtEmailCliente.Text.ToString();
                            cmdInsCons.Parameters.Add("@NOM_CELULAR", SqlDbType.VarChar).Value = txtCelularCliente.Text.ToString();

                            // Endereço
                            cmdInsCons.Parameters.Add("@NOM_ENDERECO", SqlDbType.VarChar).Value = txtEnderecoCliente.Text.ToString();
                            cmdInsCons.Parameters.Add("@NOM_NUMERO", SqlDbType.VarChar).Value = txtNumeroCliente.Text.ToString();
                            cmdInsCons.Parameters.Add("@NOM_COMPLEMENTO", SqlDbType.VarChar).Value = txtComplementoCliente.Text.ToString();
                            cmdInsCons.Parameters.Add("@NOM_BAIRRO", SqlDbType.VarChar).Value = txtBairroCliente.Text.ToString();
                            cmdInsCons.Parameters.Add("@NOM_CIDADE", SqlDbType.VarChar).Value = txtCidadeCliente.Text.ToString();
                            cmdInsCons.Parameters.Add("@NOM_UF", SqlDbType.VarChar).Value = ddlEstadoCliente.SelectedValue.ToString();
                            cmdInsCons.Parameters.Add("@NOM_CEP", SqlDbType.VarChar).Value = txtCEPCliente.Text.ToString();
                            cmdInsCons.Parameters.Add("@NOM_PAIS", SqlDbType.VarChar).Value = "BR";

                            cmdInsCons.Parameters.Add("@FLG_PRODUTO", SqlDbType.Char).Value = (ckbProdutos.Checked == true) ? "S" : "N";
                            cmdInsCons.Parameters.Add("@FLG_EXIBIR_PRODUTOS", SqlDbType.Char).Value = (ckbExibirProdutos.Checked == true) ? "S" : "N";

                            cmdInsCons.Parameters.Add("@FLG_SPLIT", SqlDbType.Char).Value = (ckbSplit.Checked == true) ? "S" : "N";

                            cmdInsCons.Parameters.Add("@NOM_CAMPO_01", SqlDbType.VarChar).Value = txtCampo01.Text.ToString();

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

                            
                            if (NomeArquivoflImagem.ToString().Trim() != "")
                            {
                                cmdInsCons.Parameters.Add("@NOM_IMAGEM", SqlDbType.VarChar).Value = NomeArquivoflImagem.ToString();
                            }
                            else
                            {
                                cmdInsCons.Parameters.Add("@NOM_IMAGEM", SqlDbType.VarChar).Value = "";
                            }
                        }

                        sIDLinkVendas = cmdInsCons.ExecuteScalar().ToString();
                        connInsCons.Close();
                        connInsCons.Dispose();

                        if (ckbLinkPagamentoAvançado.Checked)
                        {
                            GravaProdutos(sIDLinkVendas);
                            GravaBeneficiarios(sIDLinkVendas);
                        }                        


                        string sURLLinkPagamento = "";
                        try
                        {
                            sURLLinkPagamento = "https://" + Request.ServerVariables["SERVER_NAME"].ToString() + "/checkout.aspx?id=" + Funcoes.Encrypt(sIDLinkVendas.ToString()) + "&tipo=" + Funcoes.Encrypt("B");
                        }
                        catch
                        {
                            ClientScript.RegisterStartupScript(this.GetType(), "Mensagem", "alert('Não foi possível gerar o link de pagamento!');", true);
                            return;
                        }

                        // Gravar dados na base de dados
                        GravaCobranca("", sIDLinkVendas, "", "Ativo", "Link de Pagamento", ddlAdquirentes.SelectedValue.ToString());

                        txtUrlLinkPagamento.Text = sURLLinkPagamento.ToString();
                        hrfUrlLinkPagamento.HRef = sURLLinkPagamento.ToString();


                        ClientScript.RegisterStartupScript(this.GetType(),
                           "SucessoConfirmar", "alert('Dados gravados com sucesso!'); opener.PostBackOnMainPage(); window.close();", true);
                    }
                    catch
                    {
                        ClientScript.RegisterStartupScript(this.GetType(),
                           "ErroConfirmar", "alert('Ocorreu um erro ao tentar confirmar o link de pagamento! Verifique os dados digitados e tente novamente.');", true);
                    }
                }
                else
                {
                    ClientScript.RegisterStartupScript(this.GetType(),
                       "ValorTotal", "alert('Não foi especificado o valor! Verifique e reentre');", true);

                }
            }
            else
            {
                ClientScript.RegisterStartupScript(this.GetType(),
                   "FormaPagamento", "alert('Não foi selecionada nenhuma forma de pagamento! Verifique e reentre');", true);
            }
        }
        catch
        {
            ClientScript.RegisterStartupScript(this.GetType(), "ErroGeral", "alert('Não foi possível gerar o boleto bancário! Verifique e tente novamente.');", true);
        }

    }

    protected void btnContinuarCarne_Click(object sender, System.EventArgs e)
    {


    }
    protected void btnVoltarPixQRCode_Click(object sender, System.EventArgs e)
    {
        ControleOpcoes();
        btnContinuarPixQRCode.Visible = true;
        btnVoltarPixQRCode.Visible = false;

        btnReceberQRCode_Click(null, null);
    }
    protected void btnContinuarAssinatura_Click(object sender, System.EventArgs e)
    {
        if (ddlAdquirentes.SelectedValue.ToString() == "Z")
        {
            CobrancaZoop("ASSINATURA");
        }
        if (ddlAdquirentes.SelectedValue.ToString() == "P")
        {

        }
        if (ddlAdquirentes.SelectedValue.ToString() == "A")
        {
            CobrancaAsaas("ASSINATURA");
        }
        if (ddlAdquirentes.SelectedValue.ToString() == "C")
        {

        }

    }
    protected void btnVoltarAssinatura_Click(object sender, System.EventArgs e)
    {
        ControleOpcoes();
        btnContinuarAssinatura.Visible = true;
        btnVoltarAssinatura.Visible = false;
        btnAssinatura_Click(null, null);
    }
    protected void btnVoltarLinkPagamento_Click(object sender, System.EventArgs e)
    {
        ControleOpcoes();
        btnVoltarLinkPagamento.Visible = false;
        btnContinuarLinkPagamento.Visible = true;
        btnLinkPagamento_Click(null, null);
    }
    protected void ckbLinkPagamentoAvançado_CheckedChanged(object sender, System.EventArgs e)
    {
        divLinkPagamentoAvancado.Visible = (ckbLinkPagamentoAvançado.Checked) ? true : false;
    }
    protected void ckbSplit_CheckedChanged(object sender, System.EventArgs e)
    {
        divSplit.Visible = (ckbSplit.Checked)? true : false;
        divSplit.Focus();

    }
    protected void ddlParceiros_SelectedIndexChanged(object sender, System.EventArgs e)
    {
            txtBeneficiario.Text = ddlParceiros.SelectedItem.Text.ToString();
            txtWalletID.Text = ddlParceiros.SelectedValue.ToString();

            txtValorBeneficiario.Text = String.Format("{0:n0}", Funcoes.strToDouble("0"));
            txtPercentualBeneficiario.Text = String.Format("{0:n0}", Funcoes.strToDouble("0"));

    }
    protected void lsvParceiros_ItemCommand(object sender, CommandEventArgs e)
    {
        if (e.CommandName == "Excluir")
        {
            for (int i = dtParceiros.Rows.Count - 1; i >= 0; i--)
            {
                DataRow dr = dtParceiros.Rows[i];
                if (dr["beneficiario"].ToString() == e.CommandArgument.ToString())
                    dr.Delete();
            }
            dtParceiros.AcceptChanges();

            this.lsvParceiros.DataSource = dtParceiros;
            this.lsvParceiros.DataBind();
        }
    }

    protected void btnIncluirSplit_Click(object sender, System.EventArgs e)
    {
        dtParceiros.Rows.Add(txtBeneficiario.Text.ToString(), txtWalletID.Text.ToString(), txtValorBeneficiario.Text.ToString(), txtPercentualBeneficiario.Text.ToString());

        this.lsvParceiros.DataSource = dtParceiros;
        this.lsvParceiros.DataBind();
    }
    protected void ckbProdutos_CheckedChanged(object sender, System.EventArgs e)
    {
        divProdutos.Visible = (ckbProdutos.Checked)? true : false;
        divProdutos.Focus();
    }
    protected void ddlProduto_SelectedIndexChanged(object sender, System.EventArgs e)
    {
        SqlConnection mySelCadastro = new SqlConnection(Funcoes.conexao());
        mySelCadastro.Open();
        SqlCommand cmdSelCadastro = new SqlCommand("dbo.stp_produtos_ins", mySelCadastro);
        cmdSelCadastro.CommandType = CommandType.StoredProcedure;
        cmdSelCadastro.Parameters.Add("@flg_operacao", SqlDbType.Char).Value = "S";
        cmdSelCadastro.Parameters.Add("@COD_ID", SqlDbType.Int).Value = Funcoes.strToInt(ddlProduto.SelectedValue.ToString());
        cmdSelCadastro.Parameters.Add("@COD_ID_PESSOA_LICENCIADO", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["LICENCIADO"].ToString());
        cmdSelCadastro.Parameters.Add("@COD_ID_PESSOAS_FJ", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["PESSOA"].ToString());
        SqlDataReader ReaderCadastro = cmdSelCadastro.ExecuteReader();
        while (ReaderCadastro.Read())
        {
            txtIDProduto.Text = ReaderCadastro["COD_ID"].ToString();
            txtProduto.Text = ReaderCadastro["NOM_PRODUTO"].ToString();
            txtQtdeProduto.Text = String.Format("{0:n0}", Funcoes.strToDouble("0"));
            txtValorProduto.Text = String.Format("{0:n2}", Funcoes.strToDouble(ReaderCadastro["NUM_VALOR_PADRAO"].ToString()));
        }

    }
    protected void btnNovoProduto_Click(object sender, System.EventArgs e)
    {
        const string someScript = "NovoRegistro";
        ClientScript.RegisterStartupScript(this.GetType(),
            someScript, "openPopupWindow('cad_produtos.aspx?id=0','ProdutosEdicao',1024,800);", true);

    }
    protected void btnIncluirProduto_Click(object sender, System.EventArgs e)
    {
        double vValorTotal = 0;

        dtProdutos.Rows.Add(txtIDProduto.Text.ToString(), txtProduto.Text.ToString(), txtQtdeProduto.Text.ToString(), txtValorProduto.Text.ToString());

        this.lsvProdutos.DataSource = dtProdutos;
        this.lsvProdutos.DataBind();

        // Totaliza o Valor total da venda
        foreach (ListViewItem itemP in lsvProdutos.Items)
        {
            vValorTotal = vValorTotal + (Funcoes.strToDouble(((TextBox)itemP.FindControl("txtQtde")).Text.ToString()) * Funcoes.strToDouble(((TextBox)itemP.FindControl("txtValor")).Text.ToString()));
        }
        //txtValorTotal.Text = String.Format("{0:n2}", vValorTotal);

    }
    protected void lsvProdutos_ItemCommand(object sender, System.Web.UI.WebControls.ListViewCommandEventArgs e)
    {
        double vValorTotal = 0;
        if (e.CommandName == "Excluir")
        {
            for (int i = dtProdutos.Rows.Count - 1; i >= 0; i--)
            {
                DataRow dr = dtProdutos.Rows[i];
                if (dr["id"].ToString() == e.CommandArgument.ToString())
                    dr.Delete();
            }
            dtProdutos.AcceptChanges();

            this.lsvProdutos.DataSource = dtProdutos;
            this.lsvProdutos.DataBind();

            foreach (ListViewItem itemP in lsvProdutos.Items)
            {
                vValorTotal = vValorTotal + (Funcoes.strToDouble(((TextBox)itemP.FindControl("txtQtde")).Text.ToString()) * Funcoes.strToDouble(((TextBox)itemP.FindControl("txtValor")).Text.ToString()));
            }
            //txtValorTotal.Text = String.Format("{0:n2}", vValorTotal);
        }

    }

    public string GetPostBackScript()
    {
        PostBackOptions options = new PostBackOptions(btnPostback);
        Page.ClientScript.RegisterForEventValidation(options);

        return Page.ClientScript.GetPostBackEventReference(options);
    }


    protected void btnPostBack_Click(object sender, EventArgs e)
    {
        if (Funcoes.strToInt(ddlProduto.SelectedValue.ToString()) <= 0)
        {
            // Tabela de Produtos

            ConsultaProdutos();
        }

    }

    protected void ddlComprador_SelectedIndexChanged(object sender, System.EventArgs e)
    {
        CarregaCliente();

    }
    protected void ckbCliente_CheckedChanged(object sender, System.EventArgs e)
    {
        divCliente.Visible = (ckbCliente.Checked)? true : false;
        divCliente.Focus();
    }
    protected void txtCEP_TextChanged(object sender, EventArgs e)
    {
        if (txtCEPCliente.Text.ToString().Trim() != "")
        {
            ServicePointManager.Expect100Continue = true;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12
            | SecurityProtocolType.Tls11
            | SecurityProtocolType.Tls
            | SecurityProtocolType.Ssl3;

            // Convert to Base64

            var myUri = new Uri("https://viacep.com.br/ws/" + txtCEPCliente.Text.ToString() + "/json");
            var myWebRequest = WebRequest.Create(myUri);
            var myHttpWebRequest = (HttpWebRequest)myWebRequest;
            myHttpWebRequest.Accept = "application/json";

            var myWebResponse = myWebRequest.GetResponse();
            var responseStream = myWebResponse.GetResponseStream();

            //if (responseStream == null) return null;

            StreamReader myStreamReader = new StreamReader(responseStream, Encoding.UTF8);
            var json = myStreamReader.ReadToEnd();

            dadosCEP.CEPInfo m = JsonSerializer.Deserialize<dadosCEP.CEPInfo>(json);
            txtEnderecoCliente.Text = m.logradouro;
            txtBairroCliente.Text = m.bairro;
            txtCidadeCliente.Text = m.localidade;
            ddlEstadoCliente.SelectedValue = m.uf;

            responseStream.Close();
            myWebResponse.Close();
        }

    }

    private void CarregaCliente()
    {

        if (Funcoes.strToInt(ddlComprador.SelectedValue.ToString()) <= 0)
        {
            txtNomeCliente.Text = "";
            txtSobrenomeCliente.Text = "";
            txtDocumentoCliente.Text = "";
            txtEmailCliente.Text = "";
            txtCelularCliente.Text = "";

            txtEnderecoCliente.Text = "";
            txtNumeroCliente.Text = "";
            txtComplementoCliente.Text = "";
            txtBairroCliente.Text = "";
            txtCidadeCliente.Text = "";
            ddlEstadoCliente.SelectedValue = "";
            txtCEPCliente.Text = "";


            txtIDCliente.Text = "";
            txtIDZOOPCliente.Text = "";
        }
        else
        {
            SqlConnection mySelCadastro = new SqlConnection(Funcoes.conexao());
            mySelCadastro.Open();
            SqlCommand cmdSelCadastro = new SqlCommand("dbo.stp_compradores_ins", mySelCadastro);
            cmdSelCadastro.CommandType = CommandType.StoredProcedure;
            cmdSelCadastro.Parameters.Add("@flg_operacao", SqlDbType.Char).Value = "S";
            cmdSelCadastro.Parameters.Add("@COD_ID", SqlDbType.Int).Value = Funcoes.strToInt(ddlComprador.SelectedValue.ToString());
            cmdSelCadastro.Parameters.Add("@COD_ID_PESSOA_LICENCIADO", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["LICENCIADO"].ToString());
            cmdSelCadastro.Parameters.Add("@COD_ID_PESSOAS_FJ", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["PESSOA"].ToString());
            SqlDataReader ReaderCadastro = cmdSelCadastro.ExecuteReader();
            while (ReaderCadastro.Read())
            {
                txtNomeCliente.Text = ReaderCadastro["NOM_NOME"].ToString();
                txtSobrenomeCliente.Text = ReaderCadastro["NOM_SOBRENOME"].ToString();
                txtDocumentoCliente.Text = ReaderCadastro["NUM_CNPJCPF"].ToString();
                txtEmailCliente.Text = ReaderCadastro["NOM_EMAIL"].ToString();
                txtCelularCliente.Text = ReaderCadastro["NOM_CELULAR"].ToString();

                txtEnderecoCliente.Text = ReaderCadastro["NOM_ENDERECO"].ToString();
                txtNumeroCliente.Text = ReaderCadastro["NOM_NUMERO"].ToString();
                txtComplementoCliente.Text = ReaderCadastro["NOM_COMPLEMENTO"].ToString();
                txtBairroCliente.Text = ReaderCadastro["NOM_BAIRRO"].ToString();
                txtCidadeCliente.Text = ReaderCadastro["NOM_CIDADE"].ToString();
                ddlEstadoCliente.SelectedValue = ReaderCadastro["NOM_UF"].ToString();
                txtCEPCliente.Text = ReaderCadastro["NOM_CEP"].ToString();

                txtIDCliente.Text = ReaderCadastro["COD_ID"].ToString();
                txtIDZOOPCliente.Text = ReaderCadastro["COD_ID_COMPRADOR"].ToString();

                txtEmails.Text = ReaderCadastro["NOM_EMAIL"].ToString();
            }
        }
    }

    private void GravaProdutos(string sID)
    {
        foreach (ListViewItem itemP in lsvProdutos.Items)
        {
            SqlConnection connInsConsVendasProdutos = new SqlConnection(Funcoes.conexao());
            SqlCommand cmdInsConsVendasProdutos = new SqlCommand("dbo.stp_vendas_produtos_ins", connInsConsVendasProdutos);
            cmdInsConsVendasProdutos.CommandType = CommandType.StoredProcedure;
            cmdInsConsVendasProdutos.Parameters.Add("@flg_operacao", SqlDbType.Char).Value = 'I';
            cmdInsConsVendasProdutos.Parameters.Add("@COD_ID_VENDAS", SqlDbType.Int).Value = Funcoes.strToInt(sID.ToString());
            cmdInsConsVendasProdutos.Parameters.Add("@COD_ID_PESSOA_LICENCIADO", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["LICENCIADO"].ToString());
            cmdInsConsVendasProdutos.Parameters.Add("@COD_ID_PRODUTO", SqlDbType.Int).Value = Funcoes.strToInt(((TextBox)itemP.FindControl("txtIDProduto")).Text.ToString());
            cmdInsConsVendasProdutos.Parameters.Add("@NUM_QTDE", SqlDbType.Float).Value = Funcoes.strToDouble(((TextBox)itemP.FindControl("txtQtde")).Text.ToString());
            cmdInsConsVendasProdutos.Parameters.Add("@NUM_VALOR", SqlDbType.Float).Value = Funcoes.strToDouble(((TextBox)itemP.FindControl("txtValor")).Text.ToString());
            connInsConsVendasProdutos.Open();
            cmdInsConsVendasProdutos.ExecuteNonQuery();
        }
    }

    private void GravaBeneficiarios(string sID)
    {
        foreach (ListViewItem itemC in lsvParceiros.Items)
        {
            SqlConnection connInsConsVendasParceiros = new SqlConnection(Funcoes.conexao());
            SqlCommand cmdInsConsVendasParceiros = new SqlCommand("dbo.stp_vendas_parceiros_ins", connInsConsVendasParceiros);
            cmdInsConsVendasParceiros.CommandType = CommandType.StoredProcedure;
            cmdInsConsVendasParceiros.Parameters.Add("@flg_operacao", SqlDbType.Char).Value = 'I';
            cmdInsConsVendasParceiros.Parameters.Add("@COD_ID_VENDAS", SqlDbType.Int).Value = Funcoes.strToInt(sID.ToString());
            cmdInsConsVendasParceiros.Parameters.Add("@COD_ID_PESSOA_LICENCIADO", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["LICENCIADO"].ToString());
            //cmdInsConsVendasParceiros.Parameters.Add("@COD_ID_PARCEIRO", SqlDbType.Int).Value = Funcoes.strToInt(((TextBox)itemC.FindControl("txtIDParceiro")).Text.ToString());
            cmdInsConsVendasParceiros.Parameters.Add("@NOM_ID", SqlDbType.VarChar).Value = ((TextBox)itemC.FindControl("txtwalletid")).Text.ToString();
            cmdInsConsVendasParceiros.Parameters.Add("@NOM_NOME", SqlDbType.VarChar).Value = ((TextBox)itemC.FindControl("txtbeneficiario")).Text.ToString();
            
            cmdInsConsVendasParceiros.Parameters.Add("@NUM_PERCENTUAL", SqlDbType.Float).Value = Funcoes.strToDouble(((TextBox)itemC.FindControl("txtPercentual")).Text.ToString());
            cmdInsConsVendasParceiros.Parameters.Add("@NUM_VALOR", SqlDbType.Float).Value = Funcoes.strToDouble(((TextBox)itemC.FindControl("txtValor")).Text.ToString());
            connInsConsVendasParceiros.Open();
            cmdInsConsVendasParceiros.ExecuteNonQuery();
        }

    }

    protected void btnSimulador_Click(object sender, System.EventArgs e)
    {
        ControleOpcoes();
        divSimulador.Visible = true;

        ConsultaSimulador();
    }

    private void ConsultaSimulador()
    {
        SqlConnection myConsulta = new SqlConnection(Funcoes.conexao());
        myConsulta.Open();
        SqlDataAdapter SDAConsulta = new SqlDataAdapter("dbo.stp_simulador_venda_ins", myConsulta);
        SDAConsulta.SelectCommand.CommandType = CommandType.StoredProcedure;
        SDAConsulta.SelectCommand.Parameters.Add("@flg_operacao", SqlDbType.Char).Value = "C";
        SDAConsulta.SelectCommand.Parameters.Add("@COD_ID_PESSOA_LICENCIADO", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["LICENCIADO"].ToString());
        SDAConsulta.SelectCommand.Parameters.Add("@COD_ID_PESSOAS_FJ", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["PESSOA"].ToString());
        SDAConsulta.SelectCommand.Parameters.Add("@NOM_SIMULAR_VENDAS", SqlDbType.VarChar).Value = "";
        SDAConsulta.SelectCommand.Parameters.Add("@FLG_INTEGRACAO", SqlDbType.Char).Value = "A";
        DataSet dsConsulta = new DataSet();
        SDAConsulta.Fill(dsConsulta, "SIMULAR_VENDAS");
        rptSimulador.DataSource = dsConsulta.Tables["SIMULAR_VENDAS"].DefaultView;
        rptSimulador.DataBind();
        myConsulta.Close(); myConsulta.Dispose();

    }

    protected void btnPesquisarSimulador_Click(object sender, System.EventArgs e)
    {

    }
    protected void btnNovaSimulacao_Click(object sender, System.EventArgs e)
    {
        ClientScript.RegisterStartupScript(this.GetType(), "CobrancaSimulador", "openPopupWindow('cad_pagamentos_cobrança_simulador.aspx?id=0','CobrancaSimulador',1024,800);", true);
    }
    protected void Button1_Click(object sender, System.EventArgs e)
    {
        ClientScript.RegisterStartupScript(this.GetType(), "BoletoAvulso", "openPopupWindow('con_pagamentos_cobranca_boleto.aspx?id=0','BoletoAvulso',1024,800);", true);

    }
    private void ConsultaSaldoBaas(string integracao)
    {
        try
        {
            if (integracao != "A")
                return;

            // Valor padrão seguro
            lblSaldo.Text = "0,00";

            // Validação de Session
            if (HttpContext.Current.Session["PESSOA"] == null ||
                HttpContext.Current.Session["LICENCIADO"] == null)
                return;

            int pessoa = Funcoes.strToInt(HttpContext.Current.Session["PESSOA"].ToString());
            int licenciado = Funcoes.strToInt(HttpContext.Current.Session["LICENCIADO"].ToString());

            string sToken = asaas.PegarTokenSubconta(pessoa, licenciado);

            if (string.IsNullOrWhiteSpace(sToken))
                return;

            string jsonSaldo = asaas.SaldoSubconta(sToken);

            if (string.IsNullOrWhiteSpace(jsonSaldo))
                return;

            JObject oSaldo = JObject.Parse(jsonSaldo);

            // 🚨 Se a API retornou erro
            if (oSaldo["errors"] != null)
            {

                // Você pode logar o erro se quiser
                // string erroApi = oSaldo["errors"][0]?["description"]?.ToString();
                string erroApi = oSaldo.SelectToken("errors[0].description") != null ? oSaldo.SelectToken("errors[0].description").ToString() : string.Empty;

                ClientScript.RegisterStartupScript(this.GetType(), "AtualizarDadosCadastrais", "alert('Conta Digital informa que: " + erroApi + "');", true);


                lblSaldo.Text = "0,00";
                return;
            }

            // 🔐 Proteção total contra null
            JToken balanceToken = oSaldo["balance"];

            if (balanceToken == null || balanceToken.Type == JTokenType.Null)
            {
                lblSaldo.Text = "0,00";
                return;
            }

            double saldo = Funcoes.strToDouble(balanceToken.ToString());
            lblSaldo.Text = String.Format("{0:n2}", saldo);
        }
        catch (Exception ex)
        {
            // 🔥 Nunca deixa quebrar a página
            lblSaldo.Text = "0,00";

            // Opcional: log
            // Logger.Gravar(ex);
        }
    }
}