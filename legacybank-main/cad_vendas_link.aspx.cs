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

public partial class cad_vendas_link : System.Web.UI.Page
{
    public string sid_id
    {
        get
        {
            try { return Request["id"]; }
            catch { return ""; }
        }
    }

    public static DataTable dtProdutos;
    public static DataTable dtParceiros;
    public static DataTable dtPreco;

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
            Funcoes.GravaAuditoria(Funcoes.strToInt(sid_id), Request.ServerVariables["REMOTE_ADDR"].ToString(), "Venda por link", "Edição");


            dtProdutos = new DataTable();
            dtProdutos.Columns.Add("id", typeof(string));
            dtProdutos.Columns.Add("produto", typeof(string));
            dtProdutos.Columns.Add("qtde", typeof(string));
            dtProdutos.Columns.Add("valor", typeof(string));

            dtParceiros = new DataTable();
            dtParceiros.Columns.Add("id", typeof(string));
            dtParceiros.Columns.Add("parceiro", typeof(string));
            dtParceiros.Columns.Add("valor", typeof(string));
            dtParceiros.Columns.Add("percentual", typeof(string));

            dtPreco = new DataTable();
            dtPreco.Columns.Add("id", typeof(string));
            dtPreco.Columns.Add("formapagamento", typeof(string));
            dtPreco.Columns.Add("valortotal", typeof(string));

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
            ddlComprador.DataTextField = "NOM_NOME";
            ddlComprador.DataValueField = "COD_ID";
            ddlComprador.DataSource = dsCompradores.Tables["PESSOAS_FJ"].DefaultView;
            ddlComprador.DataBind();
            ddlComprador.Items.Insert(0, new ListItem("", ""));

            // Tabela de Parceiros

            SqlConnection myParceiros = new SqlConnection(Funcoes.conexao());
            myParceiros.Open();
            SqlCommand cmdParceiros = new SqlCommand("dbo.stp_pessoas_fj_ins", myParceiros);
            cmdParceiros.CommandType = CommandType.StoredProcedure;
            cmdParceiros.Parameters.Add("@flg_operacao", SqlDbType.VarChar).Value = "&";
            cmdParceiros.Parameters.Add("@COD_ID_PESSOA_LICENCIADO", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["LICENCIADO"].ToString());
            cmdParceiros.Parameters.Add("@COD_ID_PESSOAS_FJ", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["PESSOA"].ToString());
            cmdParceiros.Parameters.Add("@FLG_TIPO", SqlDbType.Char).Value = "P";
            cmdParceiros.Parameters.Add("@FLG_ATIVO", SqlDbType.Char).Value = "%";


            SqlDataAdapter drParceiros = new SqlDataAdapter();
            drParceiros.SelectCommand = cmdParceiros;
            DataSet dsParceiros = new DataSet();
            drParceiros.Fill(dsParceiros, "PESSOAS_FJ");
            ddlParceiros.DataTextField = "NOM_NOME";
            ddlParceiros.DataValueField = "COD_ID";
            ddlParceiros.DataSource = dsParceiros.Tables["PESSOAS_FJ"].DefaultView;
            ddlParceiros.DataBind();
            ddlParceiros.Items.Insert(0, new ListItem("", ""));

            // Simulação de venda
            SqlConnection mySimulado = new SqlConnection(Funcoes.conexao());
            mySimulado.Open();
            SqlCommand cmdSimulado = new SqlCommand("dbo.stp_simulador_venda_ins", mySimulado);
            cmdSimulado.CommandType = CommandType.StoredProcedure;
            cmdSimulado.Parameters.Add("@flg_operacao", SqlDbType.VarChar).Value = "C";
            cmdSimulado.Parameters.Add("@COD_ID_PESSOA_LICENCIADO", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["LICENCIADO"].ToString());
            cmdSimulado.Parameters.Add("@COD_ID_PESSOAS_FJ", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["PESSOA"].ToString());
            SqlDataAdapter drSimulado = new SqlDataAdapter();
            drSimulado.SelectCommand = cmdSimulado;
            DataSet dsSimulado = new DataSet();
            drSimulado.Fill(dsSimulado, "SIMULAR_VENDAS");
            ddlSimulado.DataTextField = "NOM_SIMULAR_VENDAS";
            ddlSimulado.DataValueField = "COD_ID";
            ddlSimulado.DataSource = dsSimulado.Tables["SIMULAR_VENDAS"].DefaultView;
            ddlSimulado.DataBind();
            ddlSimulado.Items.Insert(0, new ListItem("", ""));


            // Tabela de Produtos

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


            if (Funcoes.strToInt(sid_id) != 0)
            {
                ConsultaFicha();
                divProdutosAlteracao.Visible = true;
                divParceirosAlteracao.Visible = true;
            }
            else
            {
                divProdutosInclusao.Visible = true;
                divParceirosInclusao.Visible = true;
                //dvCartaoCredito.Visible = true;
            }
        }
    }

    private void ConsultaFicha()
    {
        SqlConnection mySelCadastro = new SqlConnection(Funcoes.conexao());
        mySelCadastro.Open();
        SqlCommand cmdSelCadastro = new SqlCommand("dbo.stp_vendas_ins", mySelCadastro);
        cmdSelCadastro.CommandType = CommandType.StoredProcedure;
        cmdSelCadastro.Parameters.Add("@flg_operacao", SqlDbType.Char).Value = "S";
        cmdSelCadastro.Parameters.Add("@COD_ID", SqlDbType.Int).Value = Funcoes.strToInt(sid_id.ToString());
        cmdSelCadastro.Parameters.Add("@COD_ID_PESSOA_LICENCIADO", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["LICENCIADO"].ToString());
        cmdSelCadastro.Parameters.Add("@COD_ID_PESSOAS_FJ_VENDEDOR", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["PESSOA"].ToString());
        SqlDataReader ReaderCadastro = cmdSelCadastro.ExecuteReader();
        while (ReaderCadastro.Read())
        {
            txtCodigo.Text = ReaderCadastro["COD_ID"].ToString();
            ckbCliente.Checked = (ReaderCadastro["FLG_CLIENTE"].ToString() == "S") ? true : false;
            ckbCliente.Checked = (ReaderCadastro["FLG_CLIENTE"].ToString() == "S") ? true : false;
            if (ckbCliente.Checked == true)
            {
                if (Funcoes.strToInt(ReaderCadastro["COD_ID_PESSOAS_FJ_COMPRADOR"].ToString()) > 0)
                {
                    ckbCliente_CheckedChanged(null, null);
                    ddlComprador.SelectedValue = ReaderCadastro["COD_ID_PESSOAS_FJ_COMPRADOR"].ToString();
                    txtIDCliente.Text = ddlComprador.SelectedValue.ToString();
                    CarregaCliente();
                }
                else
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

                    txtIDZOOPCliente.Text = ReaderCadastro["COD_ID_COMPRADOR"].ToString();

                }
            }
            else
            {
                if (ReaderCadastro["NUM_CNPJCPF"].ToString().Trim()!="")
                {
                    ckbCliente.Checked = true;
                    ckbCliente_CheckedChanged(null, null);

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
                }
            }
            //ddlProduto.SelectedValue = ReaderCadastro["COD_ID_PRODUTO"].ToString();
            //ddlFormaRecebimento.SelectedValue = ReaderCadastro["FLG_TIPO"].ToString();
            //txtValor.Text = ReaderCadastro["NUM_VALOR"].ToString();
            //txtID.Text = ReaderCadastro["COD_ID_COMPRADOR"].ToString();
            //txtNome.Text = ReaderCadastro["NOM_NOME"].ToString();
            //txtDocumento.Text = ReaderCadastro["NOM_DOCUMENTO"].ToString();

            txtReferencia.Text = ReaderCadastro["NOM_REFERENCIA_PRODUTO"].ToString();
            txtDescricao.Text = ReaderCadastro["NOM_DESCRICAO_PRODUTO"].ToString();

            ckbProdutos.Checked = (ReaderCadastro["FLG_PRODUTO"].ToString() == "S") ? true : false;
            ckbExibirProdutos.Checked = (ReaderCadastro["FLG_EXIBIR_PRODUTOS"].ToString() == "S") ? true : false;
            ckbProdutos_CheckedChanged(null, null);
            ckbBoleto.Checked = (ReaderCadastro["FLG_BOLETO"].ToString() == "S") ? true : false;
            ckbPix.Checked = (ReaderCadastro["FLG_PIX"].ToString() == "S") ? true : false;
            ckbCredito.Checked = (ReaderCadastro["FLG_CARTAO_CREDITO"].ToString() == "S") ? true : false;

            ckbPreco.Checked = (ReaderCadastro["FLG_PRECO_PARCELAMENTO"].ToString() == "S") ? true : false;
            ckbPreco_CheckedChanged(null, null);

            ckbLinkPermanente.Checked = (ReaderCadastro["FLG_LINK_PERMANENTE"].ToString() == "S") ? true : false;

            rbtTipoCobranca.Checked = true;//(ReaderCadastro["FLG_TIPO_COBRANCA"].ToString() == "U") ? true : false;
            ddlTipoOperacao.SelectedValue = ReaderCadastro["FLG_TIPO_OPERACAO"].ToString();
            ddlOperadora.SelectedValue = ReaderCadastro["FLG_OPERADORA"].ToString();

            txtEmails.Text = ReaderCadastro["NOM_EMAIL_LINK"].ToString();

            txtDataLimite.Text = ReaderCadastro["DTA_PIX_VENCIMENTO"].ToString();
            txtVencimento.Text = ReaderCadastro["DTA_VENCIMENTO"].ToString();
            ddlParcelas.SelectedValue = ReaderCadastro["NUM_PARCELAS"].ToString();

            //ddlParcelas.SelectedValue = ReaderCadastro["NUM_PARCELAS"].ToString();

            txtImagem.Text = ReaderCadastro["NOM_IMAGEM"].ToString();
            if (txtImagem.Text.ToString().Trim() != "")
            {
                imgImagem.ImageUrl = "public_html/" + txtImagem.Text.ToString();
            }

            // Dados do Boleto Bancário
            ckbMulta.Checked = (ReaderCadastro["FLG_MULTA"].ToString() == "S") ? true : false;
            ddlMulta.SelectedValue = ReaderCadastro["FLG_TIPO_MULTA"].ToString();
            txtMulta.Text = ReaderCadastro["NUM_MULTA"].ToString();

            ckbJuros.Checked = (ReaderCadastro["FLG_JUROS"].ToString() == "S") ? true : false;
            ddlJuros.SelectedValue = ReaderCadastro["FLG_TIPO_JUROS"].ToString();
            txtJuros.Text = ReaderCadastro["NUM_JUROS"].ToString();

            ckbDesconto.Checked = (ReaderCadastro["FLG_DESCONTO"].ToString() == "S") ? true : false;
            ddlDesconto.SelectedValue = ReaderCadastro["FLG_TIPO_DESCONTO"].ToString();
            txtDesconto.Text = ReaderCadastro["NUM_DESCONTO"].ToString();

            txtDias.Text = ReaderCadastro["NUM_DIAS"].ToString();

            ckbAvisos.Checked = (ReaderCadastro["FLG_AVISO"].ToString() == "S") ? true : false;
            txtAviso.Text = ReaderCadastro["DES_AVISO"].ToString();

            // Dados do Split
            ckbSplit.Checked = (ReaderCadastro["FLG_SPLIT"].ToString() == "S") ? true : false;
            ckbSplit_CheckedChanged(null, null);
            ddlFormaSplit.SelectedValue = ReaderCadastro["FLG_FORMA_SPLIT"].ToString();
            txtValorTotal.Text = ReaderCadastro["NUM_VALOR"].ToString();

            txtValor01.Text = ReaderCadastro["NUM_VALOR_01"].ToString();
            txtValor02.Text = ReaderCadastro["NUM_VALOR_02"].ToString();
            txtValor03.Text = ReaderCadastro["NUM_VALOR_03"].ToString();
            txtValor04.Text = ReaderCadastro["NUM_VALOR_04"].ToString();
            txtValor05.Text = ReaderCadastro["NUM_VALOR_05"].ToString();
            txtValor06.Text = ReaderCadastro["NUM_VALOR_06"].ToString();
            txtValor07.Text = ReaderCadastro["NUM_VALOR_07"].ToString();
            txtValor08.Text = ReaderCadastro["NUM_VALOR_08"].ToString();
            txtValor09.Text = ReaderCadastro["NUM_VALOR_09"].ToString();
            txtValor10.Text = ReaderCadastro["NUM_VALOR_10"].ToString();
            txtValor11.Text = ReaderCadastro["NUM_VALOR_11"].ToString();
            txtValor12.Text = ReaderCadastro["NUM_VALOR_12"].ToString();
            txtValor13.Text = ReaderCadastro["NUM_VALOR_13"].ToString();
            txtValor14.Text = ReaderCadastro["NUM_VALOR_14"].ToString();
            txtValor15.Text = ReaderCadastro["NUM_VALOR_15"].ToString();
            txtValor16.Text = ReaderCadastro["NUM_VALOR_16"].ToString();
            txtValor17.Text = ReaderCadastro["NUM_VALOR_17"].ToString();
            txtValor18.Text = ReaderCadastro["NUM_VALOR_18"].ToString();
            txtValor19.Text = ReaderCadastro["NUM_VALOR_19"].ToString();
            txtValor20.Text = ReaderCadastro["NUM_VALOR_20"].ToString();
            txtValor21.Text = ReaderCadastro["NUM_VALOR_21"].ToString();

            txtCampo01.Text = ReaderCadastro["NOM_CAMPO_01"].ToString();

            rbtTipoCobranca.Focus();
        }

        ConsultaProdutos();
        ConsultaParceiros();

    }

    private void ConsultaProdutos()
    {

        // Lista de Produtos

        SqlConnection myConsultaProdutos = new SqlConnection(Funcoes.conexao());
        myConsultaProdutos.Open();
        SqlDataAdapter SDAConsultaProdutos = new SqlDataAdapter("dbo.stp_vendas_produtos_ins", myConsultaProdutos);
        SDAConsultaProdutos.SelectCommand.CommandType = CommandType.StoredProcedure;
        SDAConsultaProdutos.SelectCommand.Parameters.Add("@flg_operacao", SqlDbType.Char).Value = "C";
        SDAConsultaProdutos.SelectCommand.Parameters.Add("@COD_ID_PESSOA_LICENCIADO", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["LICENCIADO"].ToString());
        SDAConsultaProdutos.SelectCommand.Parameters.Add("@COD_ID_VENDAS", SqlDbType.Int).Value = Funcoes.strToInt(sid_id.ToString());

        DataSet dsConsultaProdutos = new DataSet();
        SDAConsultaProdutos.Fill(dsConsultaProdutos, "VENDAS_PRODUTOS");
        rptProdutos.DataSource = dsConsultaProdutos.Tables["VENDAS_PRODUTOS"].DefaultView;
        rptProdutos.DataBind();
        myConsultaProdutos.Close();
        myConsultaProdutos.Dispose();
    }

    private void ConsultaParceiros()
    {
        // Lista de Parceiros

        SqlConnection myConsultaParceiros = new SqlConnection(Funcoes.conexao());
        myConsultaParceiros.Open();
        SqlDataAdapter SDAConsultaParceiros = new SqlDataAdapter("dbo.stp_vendas_parceiros_ins", myConsultaParceiros);
        SDAConsultaParceiros.SelectCommand.CommandType = CommandType.StoredProcedure;
        SDAConsultaParceiros.SelectCommand.Parameters.Add("@flg_operacao", SqlDbType.Char).Value = "C";
        SDAConsultaParceiros.SelectCommand.Parameters.Add("@COD_ID_PESSOA_LICENCIADO", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["LICENCIADO"].ToString());
        SDAConsultaParceiros.SelectCommand.Parameters.Add("@COD_ID_VENDAS", SqlDbType.Int).Value = Funcoes.strToInt(sid_id.ToString());

        DataSet dsConsultaParceiros = new DataSet();
        SDAConsultaParceiros.Fill(dsConsultaParceiros, "VENDAS_PARCEIROS");
        rptParceiros.DataSource = dsConsultaParceiros.Tables["VENDAS_PARCEIROS"].DefaultView;
        rptParceiros.DataBind();
        myConsultaParceiros.Close();
        myConsultaParceiros.Dispose();
    }

    protected void btnNovoProduto_Click(object sender, EventArgs e)
    {
        const string someScript = "NovoRegistro";
        ClientScript.RegisterStartupScript(this.GetType(),
            someScript, "openPopupWindow('cad_produtos.aspx?id=0','ProdutosEdicao',1024,800);", true);

    }

    protected void btnSalvar_Click(object sender, EventArgs e)
    {
        // Verifica se foi selecionada alguma opção de pagamento
        if ((ckbBoleto.Checked == true) || (ckbPix.Checked == true) || (ckbCredito.Checked == true))
        {
            if (Funcoes.strToDouble(txtValorTotal.Text.ToString().Trim()) > 0)
            {
                // 1o. Verifica se o cliente foi especificado (Comprador)
                Funcoes.GravaAuditoria(Funcoes.strToInt(sid_id), Request.ServerVariables["REMOTE_ADDR"].ToString(), "Venda por link", "Salvar dados");

                GravaCliente();
                GravaVenda();
                GravaProdutos();
            }
            else
            {
                ClientScript.RegisterStartupScript(this.GetType(),
                   "ValorTotal", "alert('Não foi especificado o valor total da venda! Verifique e reentre');", true);

            }
        }
        else
        {
            ClientScript.RegisterStartupScript(this.GetType(),
               "FormaPagamento", "alert('Não foi selecionada nenhuma forma de pagamento! Verifique e reentre');", true);
        }

    }

    private void GravaCliente()
    {
        if ((ckbCliente.Checked == true) && (divCliente.Visible == true))
        {
            string retornoInclusao = "";

            // Salva os dados do cliente na ZOOP no caso do documento não existir
            bool bClienteExiste = false;
            SqlConnection mySelCadastro = new SqlConnection(Funcoes.conexao());
            mySelCadastro.Open();
            SqlCommand cmdSelCadastro = new SqlCommand("dbo.stp_compradores_ins", mySelCadastro);
            cmdSelCadastro.CommandType = CommandType.StoredProcedure;
            cmdSelCadastro.Parameters.Add("@flg_operacao", SqlDbType.Char).Value = "H";
            cmdSelCadastro.Parameters.Add("@NUM_CNPJCPF", SqlDbType.VarChar).Value = txtDocumentoCliente.Text.ToString();
            cmdSelCadastro.Parameters.Add("@COD_ID_PESSOA_LICENCIADO", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["LICENCIADO"].ToString());
            cmdSelCadastro.Parameters.Add("@COD_ID_PESSOAS_FJ", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["PESSOA"].ToString());
            cmdSelCadastro.Parameters.Add("@FLG_TIPO", SqlDbType.Char).Value = "C";

            SqlDataReader ReaderCadastro = cmdSelCadastro.ExecuteReader();
            while (ReaderCadastro.Read())
            {
                bClienteExiste = true;
                ddlComprador.SelectedValue = ReaderCadastro["COD_ID"].ToString(); ;
                retornoInclusao = ReaderCadastro["COD_ID_COMPRADOR"].ToString();
                CarregaCliente();

            }

            if (bClienteExiste == false)
            {
                int iCampos = 0;
                if (
                    (txtNomeCliente.Text.ToString().Trim()=="") ||
                    (txtSobrenomeCliente.Text.ToString().Trim()=="") ||
                    (txtEmailCliente.Text.ToString().Trim()=="") ||
                    (txtCelularCliente.Text.ToString().Trim()=="") ||
                    (txtDocumentoCliente.Text.ToString().Trim()=="") ||
                    (txtEnderecoCliente.Text.ToString().Trim()=="") ||
                    (txtComplementoCliente.Text.ToString().Trim()=="") ||
                    (txtBairroCliente.Text.ToString().Trim()=="") ||
                    (txtCidadeCliente.Text.ToString().Trim()=="") ||
                    (ddlEstadoCliente.SelectedValue.ToString().Trim()=="") ||
                    (Funcoes.TIRAACENTOS(txtCEPCliente.Text.ToString())=="")
                    )
                {
                    iCampos=iCampos + 1;
                }
                if (iCampos<=0)
                {
                    dadosComprador.Comprador dcomprador = new dadosComprador.Comprador()
                    {
                        first_name = txtNomeCliente.Text.ToString(),
                        last_name = txtSobrenomeCliente.Text.ToString(),
                        email = txtEmailCliente.Text.ToString(),
                        phone_number = txtCelularCliente.Text.ToString(),
                        taxpayer_id = Funcoes.TIRAACENTOS(txtDocumentoCliente.Text.ToString()),
                        address = new dadosComprador.address()
                        {
                            line1 = txtEnderecoCliente.Text.ToString(),
                            line2 = txtComplementoCliente.Text.ToString(),
                            line3 = "",
                            neighborhood = txtBairroCliente.Text.ToString(),
                            city = txtCidadeCliente.Text.ToString(),
                            state = ddlEstadoCliente.SelectedValue.ToString(),
                            postal_code = Funcoes.TIRAACENTOS(txtCEPCliente.Text.ToString()),
                            country_code = "BR"
                        }
                    };

                    string json = JsonConvert.SerializeObject(dcomprador);
                    retornoInclusao = zoop.compradores("I", "", json);

                    SqlConnection connInsConsCliente = new SqlConnection(Funcoes.conexao());
                    connInsConsCliente.Open();
                    SqlCommand cmdInsConsCliente = new SqlCommand("dbo.stp_compradores_ins", connInsConsCliente);
                    cmdInsConsCliente.CommandType = CommandType.StoredProcedure;
                    if (Funcoes.strToInt(sid_id) != 0)
                    {
                        cmdInsConsCliente.Parameters.Add("@flg_operacao", SqlDbType.Char).Value = 'A';
                        cmdInsConsCliente.Parameters.Add("@COD_ID", SqlDbType.Int).Value = Funcoes.strToInt(sid_id.ToString());
                    }
                    else
                    {
                        cmdInsConsCliente.Parameters.Add("@flg_operacao", SqlDbType.Char).Value = 'I';
                        cmdInsConsCliente.Parameters.Add("@FLG_ATIVO", SqlDbType.Char).Value = "S";
                    }
                    cmdInsConsCliente.Parameters.Add("@COD_ID_PESSOA_LICENCIADO", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["LICENCIADO"].ToString());
                    cmdInsConsCliente.Parameters.Add("@COD_ID_PESSOAS_FJ", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["PESSOA"].ToString());
                    cmdInsConsCliente.Parameters.Add("@COD_ID_COMPRADOR", SqlDbType.VarChar).Value = retornoInclusao.ToString();

                    cmdInsConsCliente.Parameters.Add("@DTA_DATA", SqlDbType.DateTime).Value = DateTime.Now;
                    cmdInsConsCliente.Parameters.Add("@FLG_TIPO", SqlDbType.Char).Value = "C";

                    // Responsável
                    cmdInsConsCliente.Parameters.Add("@NOM_NOME", SqlDbType.VarChar).Value = txtNomeCliente.Text.ToString();
                    cmdInsConsCliente.Parameters.Add("@NOM_SOBRENOME", SqlDbType.VarChar).Value = txtSobrenomeCliente.Text.ToString();
                    cmdInsConsCliente.Parameters.Add("@NUM_CNPJCPF", SqlDbType.VarChar).Value = txtDocumentoCliente.Text.ToString();
                    cmdInsConsCliente.Parameters.Add("@NOM_EMAIL", SqlDbType.VarChar).Value = txtEmailCliente.Text.ToString();
                    cmdInsConsCliente.Parameters.Add("@NOM_CELULAR", SqlDbType.VarChar).Value = txtCelularCliente.Text.ToString();

                    // Endereço
                    cmdInsConsCliente.Parameters.Add("@NOM_ENDERECO", SqlDbType.VarChar).Value = txtEnderecoCliente.Text.ToString();
                    cmdInsConsCliente.Parameters.Add("@NOM_NUMERO", SqlDbType.VarChar).Value = txtNumeroCliente.Text.ToString();
                    cmdInsConsCliente.Parameters.Add("@NOM_COMPLEMENTO", SqlDbType.VarChar).Value = txtComplementoCliente.Text.ToString();
                    cmdInsConsCliente.Parameters.Add("@NOM_BAIRRO", SqlDbType.VarChar).Value = txtBairroCliente.Text.ToString();
                    cmdInsConsCliente.Parameters.Add("@NOM_CIDADE", SqlDbType.VarChar).Value = txtCidadeCliente.Text.ToString();
                    cmdInsConsCliente.Parameters.Add("@NOM_UF", SqlDbType.VarChar).Value = ddlEstadoCliente.SelectedValue.ToString();
                    cmdInsConsCliente.Parameters.Add("@NOM_CEP", SqlDbType.VarChar).Value = txtCEPCliente.Text.ToString();
                    cmdInsConsCliente.Parameters.Add("@NOM_PAIS", SqlDbType.VarChar).Value = "BR";

                    cmdInsConsCliente.ExecuteNonQuery();
                    connInsConsCliente.Close();
                    connInsConsCliente.Dispose();
                }
                else
                {
                    ClientScript.RegisterStartupScript(this.GetType(),
                       "DadosCliente", "alert('Todos os dados do cliente são de preenchimento obrigatório!');", true);
                }
            }
        }
    }

    private void GravaVenda()
    {

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

        string iTransacao = "";

        //if ((ddlFormaRecebimento.SelectedValue.ToString() == "C") || (ddlFormaRecebimento.SelectedValue.ToString() == "P"))
        //{

        // Insere na base de dados de vendas

        SqlConnection connInsCons = new SqlConnection(Funcoes.conexao());
        connInsCons.Open();
        SqlCommand cmdInsCons = new SqlCommand("dbo.stp_vendas_ins", connInsCons);
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
        cmdInsCons.Parameters.Add("@COD_ID_PESSOAS_FJ_VENDEDOR", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["PESSOA"].ToString());
        cmdInsCons.Parameters.Add("@COD_ID_PESSOAS_FJ_COMPRADOR", SqlDbType.Int).Value = Funcoes.strToInt(ddlComprador.SelectedValue.ToString());
        cmdInsCons.Parameters.Add("@FLG_INTEGRACAO", SqlDbType.Char).Value = 'Z';

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

        cmdInsCons.Parameters.Add("@NOM_REFERENCIA_PRODUTO", SqlDbType.VarChar).Value = txtReferencia.Text.ToString();
        cmdInsCons.Parameters.Add("@NOM_DESCRICAO_PRODUTO", SqlDbType.Text).Value = txtDescricao.Text.ToString();

        cmdInsCons.Parameters.Add("@FLG_PRODUTO", SqlDbType.Char).Value = (ckbProdutos.Checked == true) ? "S" : "N";
        cmdInsCons.Parameters.Add("@FLG_EXIBIR_PRODUTOS", SqlDbType.Char).Value = (ckbExibirProdutos.Checked == true) ? "S" : "N";
        cmdInsCons.Parameters.Add("@FLG_LINK_PERMANENTE", SqlDbType.Char).Value = (ckbLinkPermanente.Checked == true) ? "S" : "N";

        cmdInsCons.Parameters.Add("@FLG_BOLETO", SqlDbType.Char).Value = (ckbBoleto.Checked == true) ? "S" : "N";
        cmdInsCons.Parameters.Add("@FLG_PIX", SqlDbType.Char).Value = (ckbPix.Checked == true) ? "S" : "N";
        cmdInsCons.Parameters.Add("@FLG_CARTAO_CREDITO", SqlDbType.Char).Value = (ckbCredito.Checked == true) ? "S" : "N";

        cmdInsCons.Parameters.Add("@FLG_TIPO_COBRANCA", SqlDbType.Char).Value = (rbtTipoCobranca.Checked == true) ? "U" : "";
        
        cmdInsCons.Parameters.Add("@FLG_TIPO_OPERACAO", SqlDbType.Char).Value = ddlTipoOperacao.SelectedValue.ToString();
        cmdInsCons.Parameters.Add("@FLG_OPERADORA", SqlDbType.Char).Value = ddlOperadora.SelectedValue.ToString();

        cmdInsCons.Parameters.Add("@FLG_PRECO_PARCELAMENTO", SqlDbType.Char).Value = (ckbPreco.Checked == true) ? "S" : "N";
        cmdInsCons.Parameters.Add("@FLG_SPLIT", SqlDbType.Char).Value = (ckbSplit.Checked == true) ? "S" : "N";

        cmdInsCons.Parameters.Add("@NOM_EMAIL_LINK", SqlDbType.VarChar).Value = txtEmails.Text.ToString();

        cmdInsCons.Parameters.Add("@NOM_CAMPO_01", SqlDbType.VarChar).Value = txtCampo01.Text.ToString();


        if (NomeArquivoflImagem.ToString().Trim() != "")
        {
            cmdInsCons.Parameters.Add("@NOM_IMAGEM", SqlDbType.VarChar).Value = NomeArquivoflImagem.ToString();
        }
        else
        {
            cmdInsCons.Parameters.Add("@NOM_IMAGEM", SqlDbType.VarChar).Value = txtImagem.Text.ToString();

        }


        //cmdInsCons.Parameters.Add("@COD_ID_PRODUTO", SqlDbType.Int).Value = Funcoes.strToInt(ddlProduto.SelectedValue.ToString());

        // Produto
        cmdInsCons.Parameters.Add("@NUM_VALOR", SqlDbType.Float).Value = Funcoes.strToDouble(txtValorTotal.Text.ToString());
        cmdInsCons.Parameters.Add("@NUM_PARCELAS", SqlDbType.Int).Value = Funcoes.strToInt(ddlParcelas.SelectedValue.ToString());

        //cmdInsCons.Parameters.Add("@NUM_PARCELAS", SqlDbType.Int).Value = Funcoes.strToDouble(ddlParcelas.SelectedValue.ToString());
        
        
        //cmdInsCons.Parameters.Add("@FLG_TIPO", SqlDbType.Char).Value = ddlFormaRecebimento.SelectedValue.ToString();
        if (txtDataLimite.Text.ToString().Trim() != "") { cmdInsCons.Parameters.Add("@DTA_PIX_VENCIMENTO", SqlDbType.DateTime).Value = Convert.ToDateTime(txtDataLimite.Text.ToString()); }
        if (txtVencimento.Text.ToString().Trim() != "") { cmdInsCons.Parameters.Add("@DTA_VENCIMENTO", SqlDbType.DateTime).Value = Convert.ToDateTime(txtVencimento.Text.ToString()); }

        cmdInsCons.Parameters.Add("@NUM_VALOR_01", SqlDbType.Float).Value = Funcoes.strToDouble(txtValor01.Text.ToString());
        cmdInsCons.Parameters.Add("@NUM_VALOR_02", SqlDbType.Float).Value = Funcoes.strToDouble(txtValor02.Text.ToString());
        cmdInsCons.Parameters.Add("@NUM_VALOR_03", SqlDbType.Float).Value = Funcoes.strToDouble(txtValor03.Text.ToString());
        cmdInsCons.Parameters.Add("@NUM_VALOR_04", SqlDbType.Float).Value = Funcoes.strToDouble(txtValor04.Text.ToString());
        cmdInsCons.Parameters.Add("@NUM_VALOR_05", SqlDbType.Float).Value = Funcoes.strToDouble(txtValor05.Text.ToString());
        cmdInsCons.Parameters.Add("@NUM_VALOR_06", SqlDbType.Float).Value = Funcoes.strToDouble(txtValor06.Text.ToString());
        cmdInsCons.Parameters.Add("@NUM_VALOR_07", SqlDbType.Float).Value = Funcoes.strToDouble(txtValor07.Text.ToString());
        cmdInsCons.Parameters.Add("@NUM_VALOR_08", SqlDbType.Float).Value = Funcoes.strToDouble(txtValor08.Text.ToString());
        cmdInsCons.Parameters.Add("@NUM_VALOR_09", SqlDbType.Float).Value = Funcoes.strToDouble(txtValor09.Text.ToString());
        cmdInsCons.Parameters.Add("@NUM_VALOR_10", SqlDbType.Float).Value = Funcoes.strToDouble(txtValor10.Text.ToString());
        cmdInsCons.Parameters.Add("@NUM_VALOR_11", SqlDbType.Float).Value = Funcoes.strToDouble(txtValor11.Text.ToString());
        cmdInsCons.Parameters.Add("@NUM_VALOR_12", SqlDbType.Float).Value = Funcoes.strToDouble(txtValor12.Text.ToString());
        cmdInsCons.Parameters.Add("@NUM_VALOR_13", SqlDbType.Float).Value = Funcoes.strToDouble(txtValor13.Text.ToString());
        cmdInsCons.Parameters.Add("@NUM_VALOR_14", SqlDbType.Float).Value = Funcoes.strToDouble(txtValor14.Text.ToString());
        cmdInsCons.Parameters.Add("@NUM_VALOR_15", SqlDbType.Float).Value = Funcoes.strToDouble(txtValor15.Text.ToString());
        cmdInsCons.Parameters.Add("@NUM_VALOR_16", SqlDbType.Float).Value = Funcoes.strToDouble(txtValor16.Text.ToString());
        cmdInsCons.Parameters.Add("@NUM_VALOR_17", SqlDbType.Float).Value = Funcoes.strToDouble(txtValor17.Text.ToString());
        cmdInsCons.Parameters.Add("@NUM_VALOR_18", SqlDbType.Float).Value = Funcoes.strToDouble(txtValor18.Text.ToString());
        cmdInsCons.Parameters.Add("@NUM_VALOR_19", SqlDbType.Float).Value = Funcoes.strToDouble(txtValor19.Text.ToString());
        cmdInsCons.Parameters.Add("@NUM_VALOR_20", SqlDbType.Float).Value = Funcoes.strToDouble(txtValor20.Text.ToString());
        cmdInsCons.Parameters.Add("@NUM_VALOR_21", SqlDbType.Float).Value = Funcoes.strToDouble(txtValor21.Text.ToString());

        string urlorigem = Request.ServerVariables["SERVER_NAME"].ToString();
        cmdInsCons.Parameters.Add("@NOM_ORIGEM", SqlDbType.VarChar).Value = urlorigem.ToString() + "/checkout.aspx?id=";

        if (Funcoes.strToInt(sid_id) != 0)
        {
            cmdInsCons.ExecuteNonQuery();
            iTransacao = sid_id.ToString();
        }
        else
        {
            iTransacao = cmdInsCons.ExecuteScalar().ToString();
        }
        //cmdInsCons.ExecuteNonQuery();
        connInsCons.Close();
        connInsCons.Dispose();


        //Salvar os produtos no caso de inclusão
        if (Funcoes.strToInt(sid_id) == 0)
        {
            // Atualiza tabela vendas produtos
            foreach (ListViewItem itemP in lsvProdutos.Items)
            {
                SqlConnection connInsConsVendasProdutos = new SqlConnection(Funcoes.conexao());
                SqlCommand cmdInsConsVendasProdutos = new SqlCommand("dbo.stp_vendas_produtos_ins", connInsConsVendasProdutos);
                cmdInsConsVendasProdutos.CommandType = CommandType.StoredProcedure;
                cmdInsConsVendasProdutos.Parameters.Add("@flg_operacao", SqlDbType.Char).Value = 'I';
                cmdInsConsVendasProdutos.Parameters.Add("@COD_ID_VENDAS", SqlDbType.Int).Value = Funcoes.strToInt(iTransacao.ToString());
                cmdInsConsVendasProdutos.Parameters.Add("@COD_ID_PESSOA_LICENCIADO", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["LICENCIADO"].ToString());
                cmdInsConsVendasProdutos.Parameters.Add("@COD_ID_PRODUTO", SqlDbType.Int).Value = Funcoes.strToInt(((TextBox)itemP.FindControl("txtIDProduto")).Text.ToString());
                cmdInsConsVendasProdutos.Parameters.Add("@NUM_QTDE", SqlDbType.Float).Value = Funcoes.strToDouble(((TextBox)itemP.FindControl("txtQtde")).Text.ToString());
                cmdInsConsVendasProdutos.Parameters.Add("@NUM_VALOR", SqlDbType.Float).Value = Funcoes.strToDouble(((TextBox)itemP.FindControl("txtValor")).Text.ToString());
                connInsConsVendasProdutos.Open();
                cmdInsConsVendasProdutos.ExecuteNonQuery();

            }

            // Atualiza tabela vendas parceiros
            foreach (ListViewItem itemC in lsvParceiros.Items)
            {
                SqlConnection connInsConsVendasParceiros = new SqlConnection(Funcoes.conexao());
                SqlCommand cmdInsConsVendasParceiros = new SqlCommand("dbo.stp_vendas_parceiros_ins", connInsConsVendasParceiros);
                cmdInsConsVendasParceiros.CommandType = CommandType.StoredProcedure;
                cmdInsConsVendasParceiros.Parameters.Add("@flg_operacao", SqlDbType.Char).Value = 'I';
                cmdInsConsVendasParceiros.Parameters.Add("@COD_ID_VENDAS", SqlDbType.Int).Value = Funcoes.strToInt(iTransacao.ToString());
                cmdInsConsVendasParceiros.Parameters.Add("@COD_ID_PESSOA_LICENCIADO", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["LICENCIADO"].ToString());
                cmdInsConsVendasParceiros.Parameters.Add("@COD_ID_PARCEIRO", SqlDbType.Int).Value = Funcoes.strToInt(((TextBox)itemC.FindControl("txtIDParceiro")).Text.ToString());
                cmdInsConsVendasParceiros.Parameters.Add("@NUM_PERCENTUAL", SqlDbType.Float).Value = Funcoes.strToDouble(((TextBox)itemC.FindControl("txtPercentual")).Text.ToString());
                cmdInsConsVendasParceiros.Parameters.Add("@NUM_VALOR", SqlDbType.Float).Value = Funcoes.strToDouble(((TextBox)itemC.FindControl("txtValor")).Text.ToString());
                connInsConsVendasParceiros.Open();
                cmdInsConsVendasParceiros.ExecuteNonQuery();
            }
        }

        ClientScript.RegisterStartupScript(this.GetType(),
           "Alerta", "alert('Dados gravados com sucesso!'); opener.PostBackOnMainPage(); window.close();", true);
    }

    private void GravaProdutos()
    {

    }

    private void GravaParcelas()
    {

    }

    private void GravaSplit()
    {

    }

    private void SalvarLinkVenda()
    {

        // Verificar se o cliente foi especificado e Salvar dados do Cliente
        if ((ckbCliente.Checked == true) && (divCliente.Visible == true))
        {
            string retornoInclusao = "";


            // Salva os dados do cliente na ZOOP no caso do documento não existir
            bool bClienteExiste = false;
            SqlConnection mySelCadastro = new SqlConnection(Funcoes.conexao());
            mySelCadastro.Open();
            SqlCommand cmdSelCadastro = new SqlCommand("dbo.stp_compradores_ins", mySelCadastro);
            cmdSelCadastro.CommandType = CommandType.StoredProcedure;
            cmdSelCadastro.Parameters.Add("@flg_operacao", SqlDbType.Char).Value = "H";
            cmdSelCadastro.Parameters.Add("@NUM_CNPJCPF", SqlDbType.VarChar).Value = txtDocumentoCliente.Text.ToString();
            cmdSelCadastro.Parameters.Add("@COD_ID_PESSOA_LICENCIADO", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["LICENCIADO"].ToString());
            cmdSelCadastro.Parameters.Add("@COD_ID_PESSOAS_FJ", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["PESSOA"].ToString());
            cmdSelCadastro.Parameters.Add("@FLG_TIPO", SqlDbType.Char).Value = "C";

            SqlDataReader ReaderCadastro = cmdSelCadastro.ExecuteReader();
            while (ReaderCadastro.Read())
            {
                bClienteExiste = true;
                retornoInclusao = ReaderCadastro["COD_ID_COMPRADOR"].ToString();
            }
            if (bClienteExiste == false)
            {
                dadosComprador.Comprador dcomprador = new dadosComprador.Comprador()
                {
                    first_name = txtNomeCliente.Text.ToString(),
                    last_name = txtSobrenomeCliente.Text.ToString(),
                    email = txtEmailCliente.Text.ToString(),
                    phone_number = txtCelularCliente.Text.ToString(),
                    taxpayer_id = Funcoes.TIRAACENTOS(txtDocumentoCliente.Text.ToString()),
                    address = new dadosComprador.address()
                    {
                        line1 = txtEnderecoCliente.Text.ToString(),
                        line2 = txtComplementoCliente.Text.ToString(),
                        line3 = "",
                        neighborhood = txtBairroCliente.Text.ToString(),
                        city = txtCidadeCliente.Text.ToString(),
                        state = ddlEstadoCliente.SelectedValue.ToString(),
                        postal_code = Funcoes.TIRAACENTOS(txtCEPCliente.Text.ToString()),
                        country_code = "BR"
                    }
                };

                string json = JsonConvert.SerializeObject(dcomprador);
                retornoInclusao = zoop.compradores("I", "", json);

                SqlConnection connInsConsCliente = new SqlConnection(Funcoes.conexao());
                connInsConsCliente.Open();
                SqlCommand cmdInsConsCliente = new SqlCommand("dbo.stp_compradores_ins", connInsConsCliente);
                cmdInsConsCliente.CommandType = CommandType.StoredProcedure;
                if (Funcoes.strToInt(sid_id) != 0)
                {
                    cmdInsConsCliente.Parameters.Add("@flg_operacao", SqlDbType.Char).Value = 'A';
                    cmdInsConsCliente.Parameters.Add("@COD_ID", SqlDbType.Int).Value = Funcoes.strToInt(sid_id.ToString());
                }
                else
                {
                    cmdInsConsCliente.Parameters.Add("@flg_operacao", SqlDbType.Char).Value = 'I';
                    cmdInsConsCliente.Parameters.Add("@FLG_ATIVO", SqlDbType.Char).Value = "S";
                }
                cmdInsConsCliente.Parameters.Add("@COD_ID_PESSOA_LICENCIADO", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["LICENCIADO"].ToString());
                cmdInsConsCliente.Parameters.Add("@COD_ID_PESSOAS_FJ", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["PESSOA"].ToString());
                cmdInsConsCliente.Parameters.Add("@COD_ID_COMPRADOR", SqlDbType.VarChar).Value = retornoInclusao.ToString();

                cmdInsConsCliente.Parameters.Add("@DTA_DATA", SqlDbType.DateTime).Value = DateTime.Now;
                cmdInsConsCliente.Parameters.Add("@FLG_TIPO", SqlDbType.Char).Value = "C";

                // Responsável
                cmdInsConsCliente.Parameters.Add("@NOM_NOME", SqlDbType.VarChar).Value = txtNomeCliente.Text.ToString();
                cmdInsConsCliente.Parameters.Add("@NOM_SOBRENOME", SqlDbType.VarChar).Value = txtSobrenomeCliente.Text.ToString();
                cmdInsConsCliente.Parameters.Add("@NUM_CNPJCPF", SqlDbType.VarChar).Value = txtDocumentoCliente.Text.ToString();
                cmdInsConsCliente.Parameters.Add("@NOM_EMAIL", SqlDbType.VarChar).Value = txtEmailCliente.Text.ToString();
                cmdInsConsCliente.Parameters.Add("@NOM_CELULAR", SqlDbType.VarChar).Value = txtCelularCliente.Text.ToString();

                // Endereço
                cmdInsConsCliente.Parameters.Add("@NOM_ENDERECO", SqlDbType.VarChar).Value = txtEnderecoCliente.Text.ToString();
                cmdInsConsCliente.Parameters.Add("@NOM_NUMERO", SqlDbType.VarChar).Value = txtNumeroCliente.Text.ToString();
                cmdInsConsCliente.Parameters.Add("@NOM_COMPLEMENTO", SqlDbType.VarChar).Value = txtComplementoCliente.Text.ToString();
                cmdInsConsCliente.Parameters.Add("@NOM_BAIRRO", SqlDbType.VarChar).Value = txtBairroCliente.Text.ToString();
                cmdInsConsCliente.Parameters.Add("@NOM_CIDADE", SqlDbType.VarChar).Value = txtCidadeCliente.Text.ToString();
                cmdInsConsCliente.Parameters.Add("@NOM_UF", SqlDbType.VarChar).Value = ddlEstadoCliente.SelectedValue.ToString();
                cmdInsConsCliente.Parameters.Add("@NOM_CEP", SqlDbType.VarChar).Value = txtCEPCliente.Text.ToString();
                cmdInsConsCliente.Parameters.Add("@NOM_PAIS", SqlDbType.VarChar).Value = "BR";

                cmdInsConsCliente.ExecuteNonQuery();
                connInsConsCliente.Close();
                connInsConsCliente.Dispose();
            }
        }



        string iTransacao = "";

        //if ((ddlFormaRecebimento.SelectedValue.ToString() == "C") || (ddlFormaRecebimento.SelectedValue.ToString() == "P"))
        //{

        // Insere na base de dados de vendas

        SqlConnection connInsCons = new SqlConnection(Funcoes.conexao());
        connInsCons.Open();
        SqlCommand cmdInsCons = new SqlCommand("dbo.stp_vendas_ins", connInsCons);
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
        cmdInsCons.Parameters.Add("@COD_ID_PESSOAS_FJ_VENDEDOR", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["PESSOA"].ToString());
        cmdInsCons.Parameters.Add("@COD_ID_PESSOAS_FJ_COMPRADOR", SqlDbType.Int).Value = Funcoes.strToInt(ddlComprador.SelectedValue.ToString());
        cmdInsCons.Parameters.Add("@FLG_INTEGRACAO", SqlDbType.Char).Value = 'Z';

        //cmdInsCons.Parameters.Add("@COD_ID_PRODUTO", SqlDbType.Int).Value = Funcoes.strToInt(ddlProduto.SelectedValue.ToString());

        // Produto
        //cmdInsCons.Parameters.Add("@NUM_VALOR", SqlDbType.Float).Value = Funcoes.strToDouble(txtValor.Text.ToString());
        //cmdInsCons.Parameters.Add("@NUM_PARCELAS", SqlDbType.Int).Value = Funcoes.strToDouble(ddlParcelas.SelectedValue.ToString());
        //cmdInsCons.Parameters.Add("@FLG_TIPO", SqlDbType.Char).Value = ddlFormaRecebimento.SelectedValue.ToString();
        if (txtDataLimite.Text.ToString().Trim() != "") { cmdInsCons.Parameters.Add("@DTA_PIX_VENCIMENTO", SqlDbType.DateTime).Value = Convert.ToDateTime(txtDataLimite.Text.ToString()); }

        string urlorigem = Request.ServerVariables["SERVER_NAME"].ToString();
        cmdInsCons.Parameters.Add("@NOM_ORIGEM", SqlDbType.VarChar).Value = urlorigem.ToString() + "/checkout.aspx?id=";

        if (Funcoes.strToInt(sid_id) != 0)
        {
            cmdInsCons.ExecuteNonQuery();
            iTransacao = sid_id.ToString();
        }
        else
        {
            iTransacao = cmdInsCons.ExecuteScalar().ToString();
        }
        //cmdInsCons.ExecuteNonQuery();
        connInsCons.Close();
        connInsCons.Dispose();


        //Salvar os produtos no caso de inclusão
        if (Funcoes.strToInt(sid_id) == 0)
        {
            // Atualiza tabela vendas produtos
            foreach (ListViewItem itemP in lsvProdutos.Items)
            {
                SqlConnection connInsConsVendasProdutos = new SqlConnection(Funcoes.conexao());
                SqlCommand cmdInsConsVendasProdutos = new SqlCommand("dbo.stp_vendas_produtos_ins", connInsConsVendasProdutos);
                cmdInsConsVendasProdutos.CommandType = CommandType.StoredProcedure;
                cmdInsConsVendasProdutos.Parameters.Add("@flg_operacao", SqlDbType.Char).Value = 'I';
                cmdInsConsVendasProdutos.Parameters.Add("@COD_ID_VENDAS", SqlDbType.Int).Value = Funcoes.strToInt(iTransacao.ToString());
                cmdInsConsVendasProdutos.Parameters.Add("@COD_ID_PESSOA_LICENCIADO", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["LICENCIADO"].ToString());
                cmdInsConsVendasProdutos.Parameters.Add("@COD_ID_PRODUTO", SqlDbType.Int).Value = Funcoes.strToInt(((TextBox)itemP.FindControl("txtIDProduto")).Text.ToString());
                cmdInsConsVendasProdutos.Parameters.Add("@NUM_QTDE", SqlDbType.Float).Value = Funcoes.strToDouble(((TextBox)itemP.FindControl("txtQtde")).Text.ToString());
                cmdInsConsVendasProdutos.Parameters.Add("@NUM_VALOR", SqlDbType.Float).Value = Funcoes.strToDouble(((TextBox)itemP.FindControl("txtValor")).Text.ToString());
                connInsConsVendasProdutos.Open();
                cmdInsConsVendasProdutos.ExecuteNonQuery();

            }

            // Atualiza tabela vendas parceiros
            foreach (ListViewItem itemC in lsvParceiros.Items)
            {
                SqlConnection connInsConsVendasParceiros = new SqlConnection(Funcoes.conexao());
                SqlCommand cmdInsConsVendasParceiros = new SqlCommand("dbo.stp_vendas_parceiros_ins", connInsConsVendasParceiros);
                cmdInsConsVendasParceiros.CommandType = CommandType.StoredProcedure;
                cmdInsConsVendasParceiros.Parameters.Add("@flg_operacao", SqlDbType.Char).Value = 'I';
                cmdInsConsVendasParceiros.Parameters.Add("@COD_ID_VENDAS", SqlDbType.Int).Value = Funcoes.strToInt(iTransacao.ToString());
                cmdInsConsVendasParceiros.Parameters.Add("@COD_ID_PESSOA_LICENCIADO", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["LICENCIADO"].ToString());
                cmdInsConsVendasParceiros.Parameters.Add("@COD_ID_PARCEIRO", SqlDbType.Int).Value = Funcoes.strToInt(((TextBox)itemC.FindControl("txtIDParceiro")).Text.ToString());
                cmdInsConsVendasParceiros.Parameters.Add("@NUM_PERCENTUAL", SqlDbType.Float).Value = Funcoes.strToDouble(((TextBox)itemC.FindControl("txtPercentual")).Text.ToString());
                cmdInsConsVendasParceiros.Parameters.Add("@NUM_VALOR", SqlDbType.Float).Value = Funcoes.strToDouble(((TextBox)itemC.FindControl("txtValor")).Text.ToString());
                connInsConsVendasParceiros.Open();
                cmdInsConsVendasParceiros.ExecuteNonQuery();
            }
        }

        ClientScript.RegisterStartupScript(this.GetType(),
           "Alerta", "alert('Dados gravados com sucesso!');", true);

        //}

        // Recebimento em Cartão de Crédito
        /*
        if (ddlFormaRecebimento.SelectedValue.ToString() == "C")
        {

        }
        */

        // Recebimento em PIX
        /*
        if (ddlFormaRecebimento.SelectedValue.ToString() == "P")
        {
            SqlConnection mySelCadastro = new SqlConnection(Funcoes.conexao());
            mySelCadastro.Open();
            SqlCommand cmdSelCadastro = new SqlCommand("dbo.stp_vendas_ins", mySelCadastro);
            cmdSelCadastro.CommandType = CommandType.StoredProcedure;
            cmdSelCadastro.Parameters.Add("@flg_operacao", SqlDbType.Char).Value = "O";
            cmdSelCadastro.Parameters.Add("@COD_ID", SqlDbType.Int).Value = Funcoes.strToInt(iTransacao.ToString());
            SqlDataReader ReaderCadastro = cmdSelCadastro.ExecuteReader();

            while (ReaderCadastro.Read())
            {
                DadosTransacao.TransacaoPix dtransacao = new DadosTransacao.TransacaoPix()
                {
                    on_behalf_of = ReaderCadastro["NUM_TOKEN"].ToString(),
                    description = ReaderCadastro["NOM_PRODUTO"].ToString(),
                    currency = "BRL",
                    amount = Funcoes.strToInt(Convert.ToString(Funcoes.strToDouble(ReaderCadastro["NUM_VALOR"].ToString()) * 100).ToString()),
                    payment_type = "pix",
                    pix_expiration_date_time = (txtDataValidade.Text.ToString().Trim()=="") ? "" : String.Format("{0:u}", Convert.ToDateTime(txtDataValidade.Text.ToString()) )
                };

                string json = JsonConvert.SerializeObject(dtransacao);
                string retornoInclusao = zoop.transacao("I", "", json);
                // Gravar dados atualizados na tabela VENDAS




                SqlConnection connInsCons = new SqlConnection(Funcoes.conexao());
                connInsCons.Open();
                SqlCommand cmdInsCons = new SqlCommand("dbo.stp_vendas_ins", connInsCons);
                cmdInsCons.CommandType = CommandType.StoredProcedure;
                cmdInsCons.Parameters.Add("@flg_operacao", SqlDbType.Char).Value = 'U';
                cmdInsCons.Parameters.Add("@COD_ID", SqlDbType.Int).Value = Funcoes.strToInt(iTransacao.ToString());

                cmdInsCons.Parameters.Add("@NOM_CODE", SqlDbType.VarChar).Value = retornoInclusao.ToString();

                string RetornoJson = zoop.transacao("P", retornoInclusao.ToString(), "");
                
                cmdInsCons.Parameters.Add("@DES_JSON", SqlDbType.Text).Value = RetornoJson.ToString();


                
                if (RetornoJson.ToString().Trim() != "")
                {
                    JObject o = JObject.Parse(RetornoJson.ToString());

                    cmdInsCons.Parameters.Add("@NOM_PIX", SqlDbType.VarChar).Value = o["payment_method"]["qr_code"]["emv"].ToString();

                    string URL = o["payment_method"]["qr_code"]["emv"].ToString();
                    QRCodeEncoder encoder = new QRCodeEncoder();

                    Bitmap image = encoder.Encode(URL);
                    string Codificacao = DateTime.Now.Year.ToString() + DateTime.Now.Month.ToString() + DateTime.Now.Day.ToString() + DateTime.Now.Hour.ToString() + DateTime.Now.Minute.ToString() + DateTime.Now.Second.ToString();
                    image.Save(Server.MapPath("public_html") + "\\" + HttpContext.Current.Session["LICENCIADO"].ToString() + "_" + Codificacao.ToString() + "_" + iTransacao.ToString().PadLeft(10,'0') + ".bmp");
                    cmdInsCons.Parameters.Add("@NOM_IMAGEM_PIX", SqlDbType.VarChar).Value = HttpContext.Current.Session["LICENCIADO"].ToString() + "_" + Codificacao.ToString() + "_" + iTransacao.ToString().PadLeft(10, '0') + ".bmp";

                    imgQRcode.Src = "../public_html/" + HttpContext.Current.Session["LICENCIADO"].ToString() + "_" + Codificacao.ToString() + "_" + iTransacao.ToString().PadLeft(10, '0') + ".bmp";
                    
                    //ClientScript.RegisterStartupScript(this.GetType(),
                    //    "SucessoPix", "alert('" + o["status"].ToString() + "');", true);
                }
                else
                {
                    // Verifica se ocorreu um erro
                    try
                    {
                        JObject o = JObject.Parse(retornoInclusao.ToString());
                        ClientScript.RegisterStartupScript(this.GetType(),
                            "ErroPix", "alert('" + o["error"]["status_code"].ToString() + "');", true);
                    }
                    catch
                    {
                        ClientScript.RegisterStartupScript(this.GetType(),
                           "ErroProcessamento", "alert('Ocorreu um erru ao processar o pagamento! Favor verificar se os dados digitados estão corretos.');", true);
                    }
                }
                
                cmdInsCons.ExecuteNonQuery();
                connInsCons.Close();
                connInsCons.Dispose();



            }

        }
        */
        ClientScript.RegisterStartupScript(this.GetType(),
"Finalizar", "opener.PostBackOnMainPage(); window.close(); ", true);


    }
    
    protected void btnCancelar_Click(object sender, EventArgs e)
    {
        ClientScript.RegisterStartupScript(this.GetType(),
    "Alerta", "opener.PostBackOnMainPage(); window.close(); ", true);

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



    protected void ddlComprador_SelectedIndexChanged(object sender, EventArgs e)
    {
        CarregaCliente();
    }
    protected void ddlProduto_SelectedIndexChanged(object sender, EventArgs e)
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
            txtValorProduto.Text = String.Format("{0:n2}",Funcoes.strToDouble(ReaderCadastro["NUM_VALOR_PADRAO"].ToString()));
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
        if (Funcoes.strToInt(ddlComprador.SelectedValue.ToString()) <= 0)
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
            ddlComprador.DataTextField = "NOM_NOME";
            ddlComprador.DataValueField = "COD_ID";
            ddlComprador.DataSource = dsCompradores.Tables["PESSOAS_FJ"].DefaultView;
            ddlComprador.DataBind();
            ddlComprador.Items.Insert(0, new ListItem("", ""));


        }
        if (Funcoes.strToInt(ddlProduto.SelectedValue.ToString()) <= 0)
        {
            // Tabela de Produtos

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

        if (Funcoes.strToInt(ddlParceiros.SelectedValue.ToString()) <= 0)
        {
            SqlConnection myParceiros = new SqlConnection(Funcoes.conexao());
            myParceiros.Open();
            SqlCommand cmdParceiros = new SqlCommand("dbo.stp_pessoas_fj_ins", myParceiros);
            cmdParceiros.CommandType = CommandType.StoredProcedure;
            cmdParceiros.Parameters.Add("@flg_operacao", SqlDbType.VarChar).Value = "&";
            cmdParceiros.Parameters.Add("@COD_ID_PESSOA_LICENCIADO", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["LICENCIADO"].ToString());
            cmdParceiros.Parameters.Add("@COD_ID_PESSOAS_FJ", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["PESSOA"].ToString());
            cmdParceiros.Parameters.Add("@FLG_TIPO", SqlDbType.Char).Value = "P";
            cmdParceiros.Parameters.Add("@FLG_ATIVO", SqlDbType.Char).Value = "";


            SqlDataAdapter drParceiros = new SqlDataAdapter();
            drParceiros.SelectCommand = cmdParceiros;
            DataSet dsParceiros = new DataSet();
            drParceiros.Fill(dsParceiros, "PESSOAS_FJ");
            ddlParceiros.DataTextField = "NOM_NOME";
            ddlParceiros.DataValueField = "COD_ID";
            ddlParceiros.DataSource = dsParceiros.Tables["PESSOAS_FJ"].DefaultView;
            ddlParceiros.DataBind();
            ddlParceiros.Items.Insert(0, new ListItem("", ""));
        }
    }


    protected void ckbBoleto_CheckedChanged(object sender, EventArgs e)
    {
        if (ckbBoleto.Checked == true)
        { dvBoletoBancario.Visible = true; dvBoletoBancario.Focus(); }
        else { dvBoletoBancario.Visible = false; }
    }
    protected void ckbPreco_CheckedChanged(object sender, EventArgs e)
    {
        if (ckbPreco.Checked == true)
        {
            divPreco.Visible = true;
            divPreco.Focus();
        }
        else
        {
            divPreco.Visible = false;
        }
    }
    protected void ckbCliente_CheckedChanged(object sender, EventArgs e)
    {
        if (ckbCliente.Checked == true)
        {
            divCliente.Visible = true;
            divCliente.Focus();
        }
        else
        {
            divCliente.Visible = false;
        }
    }
    protected void ckbProdutos_CheckedChanged(object sender, EventArgs e)
    {
        if (ckbProdutos.Checked == true)
        {
            divProdutos.Visible = true;
            divProdutos.Focus();
        }
        else
        {
            divProdutos.Visible = false;
        }
    }
    protected void btnIncluirProduto_Click(object sender, EventArgs e)
    {
        double vValorTotal = 0;

        if (Funcoes.strToInt(sid_id) != 0)
        {
            // Incluir diretamente o produto
            SqlConnection connInsConsVendasProdutos = new SqlConnection(Funcoes.conexao());
            SqlCommand cmdInsConsVendasProdutos = new SqlCommand("dbo.stp_vendas_produtos_ins", connInsConsVendasProdutos);
            cmdInsConsVendasProdutos.CommandType = CommandType.StoredProcedure;
            cmdInsConsVendasProdutos.Parameters.Add("@flg_operacao", SqlDbType.Char).Value = 'I';
            cmdInsConsVendasProdutos.Parameters.Add("@COD_ID_VENDAS", SqlDbType.Int).Value = Funcoes.strToInt(sid_id.ToString());
            cmdInsConsVendasProdutos.Parameters.Add("@COD_ID_PESSOA_LICENCIADO", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["LICENCIADO"].ToString());
            cmdInsConsVendasProdutos.Parameters.Add("@COD_ID_PRODUTO", SqlDbType.Int).Value = Funcoes.strToInt(txtIDProduto.Text.ToString());
            cmdInsConsVendasProdutos.Parameters.Add("@NUM_QTDE", SqlDbType.Float).Value = Funcoes.strToDouble(txtQtdeProduto.Text.ToString());
            cmdInsConsVendasProdutos.Parameters.Add("@NUM_VALOR", SqlDbType.Float).Value = Funcoes.strToDouble(txtValorProduto.Text.ToString());
            connInsConsVendasProdutos.Open();
            cmdInsConsVendasProdutos.ExecuteNonQuery();
            ConsultaProdutos();

            // Totaliza o Valor total da venda
            foreach (RepeaterItem itemP in rptProdutos.Items)
            {
                vValorTotal = vValorTotal + (Funcoes.strToDouble(((TextBox)itemP.FindControl("txtQtde")).Text.ToString()) * Funcoes.strToDouble(((TextBox)itemP.FindControl("txtValor")).Text.ToString()));
            }
            txtValorTotal.Text = String.Format("{0:n2}", vValorTotal);


        }
        else
        {
            dtProdutos.Rows.Add(txtIDProduto.Text.ToString(), txtProduto.Text.ToString(), txtQtdeProduto.Text.ToString(), txtValorProduto.Text.ToString());

            this.lsvProdutos.DataSource = dtProdutos;
            this.lsvProdutos.DataBind();

            // Totaliza o Valor total da venda
            foreach (ListViewItem itemP in lsvProdutos.Items)
            {
                vValorTotal = vValorTotal + (Funcoes.strToDouble(((TextBox)itemP.FindControl("txtQtde")).Text.ToString()) * Funcoes.strToDouble(((TextBox)itemP.FindControl("txtValor")).Text.ToString()));
            }
            txtValorTotal.Text = String.Format("{0:n2}", vValorTotal);


        }




    }
    protected void ckbSplit_CheckedChanged(object sender, EventArgs e)
    {
        if (ckbSplit.Checked == true)
        {
            divSplit.Visible = true;
            divSplit.Focus();
        }
        else
        {
            divSplit.Visible = false;
        }
    }
    protected void rptParceiros_ItemCommand(object source, RepeaterCommandEventArgs e)
    {
        if (e.CommandName == "Excluir")
        {
            if (Funcoes.strToInt(sid_id) != 0)
            {
                // Excluir diretamente o Parceiro
                SqlConnection connInsConsVendasParceiros = new SqlConnection(Funcoes.conexao());
                SqlCommand cmdInsConsVendasParceiros = new SqlCommand("dbo.stp_vendas_parceiros_ins", connInsConsVendasParceiros);
                cmdInsConsVendasParceiros.CommandType = CommandType.StoredProcedure;
                cmdInsConsVendasParceiros.Parameters.Add("@flg_operacao", SqlDbType.Char).Value = 'E';
                cmdInsConsVendasParceiros.Parameters.Add("@COD_ID_VENDAS", SqlDbType.Int).Value = Funcoes.strToInt(sid_id.ToString());
                cmdInsConsVendasParceiros.Parameters.Add("@COD_ID_PESSOA_LICENCIADO", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["LICENCIADO"].ToString());
                cmdInsConsVendasParceiros.Parameters.Add("@COD_ID", SqlDbType.Int).Value = Funcoes.strToInt(e.CommandArgument.ToString());
                connInsConsVendasParceiros.Open();
                cmdInsConsVendasParceiros.ExecuteNonQuery();

            }
            ConsultaParceiros();

        }
    }
    protected void btnIncluirParceiro_Click(object sender, EventArgs e)
    {
        if (Funcoes.strToInt(sid_id) != 0)
        {
            // Incluir diretamente o parceiro
            SqlConnection connInsConsVendasParceiros = new SqlConnection(Funcoes.conexao());
            SqlCommand cmdInsConsVendasParceiros = new SqlCommand("dbo.stp_vendas_parceiros_ins", connInsConsVendasParceiros);
            cmdInsConsVendasParceiros.CommandType = CommandType.StoredProcedure;
            cmdInsConsVendasParceiros.Parameters.Add("@flg_operacao", SqlDbType.Char).Value = 'I';
            cmdInsConsVendasParceiros.Parameters.Add("@COD_ID_VENDAS", SqlDbType.Int).Value = Funcoes.strToInt(sid_id.ToString());
            cmdInsConsVendasParceiros.Parameters.Add("@COD_ID_PESSOA_LICENCIADO", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["LICENCIADO"].ToString());
            cmdInsConsVendasParceiros.Parameters.Add("@COD_ID_PARCEIRO", SqlDbType.Int).Value = Funcoes.strToInt(txtIDParceiro.Text.ToString());
            cmdInsConsVendasParceiros.Parameters.Add("@NUM_PERCENTUAL", SqlDbType.Float).Value = Funcoes.strToDouble(txtPercentualParceiro.Text.ToString());
            cmdInsConsVendasParceiros.Parameters.Add("@NUM_VALOR", SqlDbType.Float).Value = Funcoes.strToDouble(txtValorParceiro.Text.ToString());
            connInsConsVendasParceiros.Open();
            cmdInsConsVendasParceiros.ExecuteNonQuery();
            ConsultaParceiros();
        }
        else
        {
            dtParceiros.Rows.Add(txtIDParceiro.Text.ToString(), txtParceiro.Text.ToString(), txtValorParceiro.Text.ToString(), txtPercentualParceiro.Text.ToString());

            this.lsvParceiros.DataSource = dtParceiros;
            this.lsvParceiros.DataBind();
        }
    }
    protected void btnEnviar_Click(object sender, EventArgs e)
    {

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

    protected void lsvParcelas_ItemCommand(object sender, CommandEventArgs e)
    {

    }
    protected void lsvProdutos_ItemCommand(object sender, CommandEventArgs e)
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
            txtValorTotal.Text = String.Format("{0:n2}", vValorTotal);
        }
    }

    protected void lsvParceiros_ItemCommand(object sender, CommandEventArgs e)
    {
        if (e.CommandName == "Excluir")
        {
            for (int i = dtParceiros.Rows.Count - 1; i >= 0; i--)
            {
                DataRow dr = dtParceiros.Rows[i];
                if (dr["id"].ToString() == e.CommandArgument.ToString())
                    dr.Delete();
            }
            dtParceiros.AcceptChanges();

            this.lsvParceiros.DataSource = dtParceiros;
            this.lsvParceiros.DataBind();
        }
    }
    
    protected void rptProdutos_ItemCommand(object source, RepeaterCommandEventArgs e)
    {
        double vValorTotal = 0;
        if (e.CommandName == "Excluir")
        {
            if (Funcoes.strToInt(sid_id) != 0)
            {
                // Excluir diretamente o produto
                SqlConnection connInsConsVendasProdutos = new SqlConnection(Funcoes.conexao());
                SqlCommand cmdInsConsVendasProdutos = new SqlCommand("dbo.stp_vendas_produtos_ins", connInsConsVendasProdutos);
                cmdInsConsVendasProdutos.CommandType = CommandType.StoredProcedure;
                cmdInsConsVendasProdutos.Parameters.Add("@flg_operacao", SqlDbType.Char).Value = 'E';
                cmdInsConsVendasProdutos.Parameters.Add("@COD_ID_VENDAS", SqlDbType.Int).Value = Funcoes.strToInt(sid_id.ToString());
                cmdInsConsVendasProdutos.Parameters.Add("@COD_ID_PESSOA_LICENCIADO", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["LICENCIADO"].ToString());
                cmdInsConsVendasProdutos.Parameters.Add("@COD_ID", SqlDbType.Int).Value = Funcoes.strToInt(e.CommandArgument.ToString());
                connInsConsVendasProdutos.Open();
                cmdInsConsVendasProdutos.ExecuteNonQuery();
            }
            ConsultaProdutos();

            // Totaliza o Valor total da venda
            foreach (RepeaterItem itemP in rptProdutos.Items)
            {
                vValorTotal = vValorTotal + (Funcoes.strToDouble(((TextBox)itemP.FindControl("txtQtde")).Text.ToString()) * Funcoes.strToDouble(((TextBox)itemP.FindControl("txtValor")).Text.ToString()));
            }
            txtValorTotal.Text = String.Format("{0:n2}", vValorTotal);

        }
    }
    protected void lsvProdutos_ItemDeleted(object sender, ListViewDeletedEventArgs e)
    {
        
    }
    protected void lsvProdutos_PagePropertiesChanging(object sender, PagePropertiesChangingEventArgs e)
    {
        

    }
    protected void lsvProdutos_ItemDeleting(object sender, ListViewDeleteEventArgs e)
    {
        

    }
    protected void btNovoParceiro_Click(object sender, EventArgs e)
    {
        ClientScript.RegisterStartupScript(this.GetType(),
    "CadastroParceiro", "openPopupWindow('cad_parceiros.aspx?id=0','Parceiros',1024,800);", true);

    }
    protected void ddlParceiros_SelectedIndexChanged(object sender, EventArgs e)
    {
        SqlConnection mySelCadastro = new SqlConnection(Funcoes.conexao());
        mySelCadastro.Open();
        SqlCommand cmdSelCadastro = new SqlCommand("dbo.stp_pessoas_fj_ins", mySelCadastro);
        cmdSelCadastro.CommandType = CommandType.StoredProcedure;
        cmdSelCadastro.Parameters.Add("@flg_operacao", SqlDbType.Char).Value = "S";
        cmdSelCadastro.Parameters.Add("@COD_ID", SqlDbType.Int).Value = Funcoes.strToInt(ddlParceiros.SelectedValue.ToString());
        cmdSelCadastro.Parameters.Add("@COD_ID_PESSOA_LICENCIADO", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["LICENCIADO"].ToString());
        cmdSelCadastro.Parameters.Add("@COD_ID_PESSOAS_FJ", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["PESSOA"].ToString());
        SqlDataReader ReaderCadastro = cmdSelCadastro.ExecuteReader();
        while (ReaderCadastro.Read())
        {
            txtIDParceiro.Text = ReaderCadastro["COD_ID"].ToString();
            txtParceiro.Text = ReaderCadastro["NOM_RAZAOSOCIAL"].ToString()+ "(" + ReaderCadastro["NOM_NOME"].ToString() + " " + ReaderCadastro["NOM_SOBRENOME"].ToString() + ")";
            txtValorParceiro.Text = String.Format("{0:n0}", Funcoes.strToDouble("0"));
            txtPercentualParceiro.Text = String.Format("{0:n0}", Funcoes.strToDouble("0"));
        }
    }
    protected void btnVisualizarSimulado_Click(object sender, EventArgs e)
    {
        ClientScript.RegisterStartupScript(this.GetType(),
            "SimulacaoVenda", "openPopupWindow('cad_simular_venda.aspx?id="+ddlSimulado.SelectedValue.ToString()+"','ConsultaSimulacaoVenda',1024,800);", true);

    }
    protected void btnAplicarSimulado_Click(object sender, EventArgs e)
    {
        if (Funcoes.strToInt(ddlSimulado.SelectedValue.ToString()) > 0)
        {


            SqlConnection mySelCadastro = new SqlConnection(Funcoes.conexao());
            mySelCadastro.Open();
            SqlCommand cmdSelCadastro = new SqlCommand("dbo.stp_simulador_venda_ins", mySelCadastro);
            cmdSelCadastro.CommandType = CommandType.StoredProcedure;
            cmdSelCadastro.Parameters.Add("@flg_operacao", SqlDbType.Char).Value = "S";
            cmdSelCadastro.Parameters.Add("@COD_ID", SqlDbType.Int).Value = Funcoes.strToInt(ddlSimulado.SelectedValue.ToString());
            cmdSelCadastro.Parameters.Add("@COD_ID_PESSOA_LICENCIADO", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["LICENCIADO"].ToString());
            SqlDataReader ReaderCadastro = cmdSelCadastro.ExecuteReader();
            while (ReaderCadastro.Read())
            {

                
                SqlConnection mySelCadastroParcelas = new SqlConnection(Funcoes.conexao());
                mySelCadastroParcelas.Open();
                SqlCommand cmdSelCadastroParcelas = new SqlCommand("dbo.stp_simulador_venda_ins", mySelCadastroParcelas);
                cmdSelCadastroParcelas.CommandType = CommandType.StoredProcedure;
                cmdSelCadastroParcelas.Parameters.Add("@flg_operacao", SqlDbType.Char).Value = "L";
                cmdSelCadastroParcelas.Parameters.Add("@COD_ID_PESSOAS_FJ", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["PESSOA"].ToString());
                cmdSelCadastroParcelas.Parameters.Add("@COD_ID_PESSOA_LICENCIADO", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["LICENCIADO"].ToString());
                cmdSelCadastroParcelas.Parameters.Add("@FLG_JUROS", SqlDbType.Char).Value = ReaderCadastro["FLG_JUROS"].ToString();

                SqlConnection mySelCadastroPlano = new SqlConnection(Funcoes.conexao());
                mySelCadastroPlano.Open();
                SqlCommand cmdSelCadastroPlano = new SqlCommand("dbo.stp_pessoas_fj_ins", mySelCadastroPlano);
                cmdSelCadastroPlano.CommandType = CommandType.StoredProcedure;
                cmdSelCadastroPlano.Parameters.Add("@flg_operacao", SqlDbType.Char).Value = "S";
                cmdSelCadastroPlano.Parameters.Add("@COD_ID", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["PESSOA"].ToString());
                cmdSelCadastroPlano.Parameters.Add("@COD_ID_PESSOA_LICENCIADO", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["LICENCIADO"].ToString());
                SqlDataReader ReaderCadastroPlano = cmdSelCadastroPlano.ExecuteReader();
                while (ReaderCadastroPlano.Read())
                {
                    cmdSelCadastroParcelas.Parameters.Add("@COD_ID_PLANOS", SqlDbType.Int).Value = Funcoes.strToInt(ReaderCadastroPlano["COD_ID_PLANO_ZOOP"].ToString());
                }
                mySelCadastroPlano.Close();
                mySelCadastroPlano.Dispose();

                cmdSelCadastroParcelas.Parameters.Add("@COD_ID_BANDEIRA", SqlDbType.Int).Value = Funcoes.strToInt(ReaderCadastro["COD_ID_BANDEIRA"].ToString());
                cmdSelCadastroParcelas.Parameters.Add("@NUM_VALOR", SqlDbType.Float).Value = Funcoes.strToDouble(ReaderCadastro["NUM_VALOR"].ToString());
                cmdSelCadastroParcelas.Parameters.Add("@FLG_PRESENCIAL_ONLINE", SqlDbType.Char).Value = "O";


                SqlDataReader ReaderCadastroParcelas = cmdSelCadastroParcelas.ExecuteReader();
                while (ReaderCadastroParcelas.Read())
                {


                    if (Funcoes.strToInt(ReaderCadastroParcelas["NUM_ORDEM"].ToString()) == 2) { txtValor01.Text = String.Format("{0:n2}",Funcoes.strToDouble(ReaderCadastroParcelas["VALOR_TOTAL"].ToString())); }
                    if (Funcoes.strToInt(ReaderCadastroParcelas["NUM_ORDEM"].ToString()) == 3) { txtValor02.Text = String.Format("{0:n2}", Funcoes.strToDouble(ReaderCadastroParcelas["VALOR_TOTAL"].ToString())); }
                    if (Funcoes.strToInt(ReaderCadastroParcelas["NUM_ORDEM"].ToString()) == 4) { txtValor03.Text = String.Format("{0:n2}", Funcoes.strToDouble(ReaderCadastroParcelas["VALOR_TOTAL"].ToString())); }
                    if (Funcoes.strToInt(ReaderCadastroParcelas["NUM_ORDEM"].ToString()) == 5) { txtValor04.Text = String.Format("{0:n2}", Funcoes.strToDouble(ReaderCadastroParcelas["VALOR_TOTAL"].ToString())); }
                    if (Funcoes.strToInt(ReaderCadastroParcelas["NUM_ORDEM"].ToString()) == 6) { txtValor05.Text = String.Format("{0:n2}", Funcoes.strToDouble(ReaderCadastroParcelas["VALOR_TOTAL"].ToString())); }
                    if (Funcoes.strToInt(ReaderCadastroParcelas["NUM_ORDEM"].ToString()) == 7) { txtValor06.Text = String.Format("{0:n2}", Funcoes.strToDouble(ReaderCadastroParcelas["VALOR_TOTAL"].ToString())); }
                    if (Funcoes.strToInt(ReaderCadastroParcelas["NUM_ORDEM"].ToString()) == 8) { txtValor07.Text = String.Format("{0:n2}", Funcoes.strToDouble(ReaderCadastroParcelas["VALOR_TOTAL"].ToString())); }
                    if (Funcoes.strToInt(ReaderCadastroParcelas["NUM_ORDEM"].ToString()) == 9) { txtValor08.Text = String.Format("{0:n2}", Funcoes.strToDouble(ReaderCadastroParcelas["VALOR_TOTAL"].ToString())); }
                    if (Funcoes.strToInt(ReaderCadastroParcelas["NUM_ORDEM"].ToString()) == 10) { txtValor09.Text = String.Format("{0:n2}", Funcoes.strToDouble(ReaderCadastroParcelas["VALOR_TOTAL"].ToString())); }
                    if (Funcoes.strToInt(ReaderCadastroParcelas["NUM_ORDEM"].ToString()) == 11) { txtValor10.Text = String.Format("{0:n2}", Funcoes.strToDouble(ReaderCadastroParcelas["VALOR_TOTAL"].ToString())); }
                    if (Funcoes.strToInt(ReaderCadastroParcelas["NUM_ORDEM"].ToString()) == 12) { txtValor11.Text = String.Format("{0:n2}", Funcoes.strToDouble(ReaderCadastroParcelas["VALOR_TOTAL"].ToString())); }
                    if (Funcoes.strToInt(ReaderCadastroParcelas["NUM_ORDEM"].ToString()) == 13) { txtValor12.Text = String.Format("{0:n2}", Funcoes.strToDouble(ReaderCadastroParcelas["VALOR_TOTAL"].ToString())); }
                    if (Funcoes.strToInt(ReaderCadastroParcelas["NUM_ORDEM"].ToString()) == 14) { txtValor13.Text = String.Format("{0:n2}", Funcoes.strToDouble(ReaderCadastroParcelas["VALOR_TOTAL"].ToString())); }
                    if (Funcoes.strToInt(ReaderCadastroParcelas["NUM_ORDEM"].ToString()) == 15) { txtValor14.Text = String.Format("{0:n2}", Funcoes.strToDouble(ReaderCadastroParcelas["VALOR_TOTAL"].ToString())); }
                    if (Funcoes.strToInt(ReaderCadastroParcelas["NUM_ORDEM"].ToString()) == 16) { txtValor15.Text = String.Format("{0:n2}", Funcoes.strToDouble(ReaderCadastroParcelas["VALOR_TOTAL"].ToString())); }
                    if (Funcoes.strToInt(ReaderCadastroParcelas["NUM_ORDEM"].ToString()) == 17) { txtValor16.Text = String.Format("{0:n2}", Funcoes.strToDouble(ReaderCadastroParcelas["VALOR_TOTAL"].ToString())); }
                    if (Funcoes.strToInt(ReaderCadastroParcelas["NUM_ORDEM"].ToString()) == 18) { txtValor17.Text = String.Format("{0:n2}", Funcoes.strToDouble(ReaderCadastroParcelas["VALOR_TOTAL"].ToString())); }
                    if (Funcoes.strToInt(ReaderCadastroParcelas["NUM_ORDEM"].ToString()) == 19) { txtValor18.Text = String.Format("{0:n2}", Funcoes.strToDouble(ReaderCadastroParcelas["VALOR_TOTAL"].ToString())); }
                    if (Funcoes.strToInt(ReaderCadastroParcelas["NUM_ORDEM"].ToString()) == 20) { txtValor19.Text = String.Format("{0:n2}", Funcoes.strToDouble(ReaderCadastroParcelas["VALOR_TOTAL"].ToString())); }
                    if (Funcoes.strToInt(ReaderCadastroParcelas["NUM_ORDEM"].ToString()) == 21) { txtValor20.Text = String.Format("{0:n2}", Funcoes.strToDouble(ReaderCadastroParcelas["VALOR_TOTAL"].ToString())); }
                    if (Funcoes.strToInt(ReaderCadastroParcelas["NUM_ORDEM"].ToString()) == 22) { txtValor21.Text = String.Format("{0:n2}", Funcoes.strToDouble(ReaderCadastroParcelas["VALOR_TOTAL"].ToString())); }

                }

                mySelCadastroParcelas.Close();
                mySelCadastroParcelas.Dispose();




            }
        }
    }
}