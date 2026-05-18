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



public partial class cad_carne : System.Web.UI.Page
{
    public string sid_id
    {
        get
        {
            try { return Funcoes.Decrypt(Request["id"]); }
            catch { return ""; }
        }
    }

    public static DataTable dtParceiros;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (HttpContext.Current.Session.Count <= 0)
        {
            ClientScript.RegisterStartupScript(this.GetType(), "logoff", "opener.PostBackOnMainPage(); window.close(); ", true);
            FormsAuthentication.SignOut();
        }

        if (!IsPostBack)
        {
            txtCodigo.Text = sid_id.ToString();

            dtParceiros = new DataTable();
            dtParceiros.Columns.Add("id", typeof(string));
            dtParceiros.Columns.Add("parceiro", typeof(string));
            dtParceiros.Columns.Add("valor", typeof(string));
            dtParceiros.Columns.Add("percentual", typeof(string));


            divDadosBoletoBancario.Visible = false;
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
            cmdCompradores.Parameters.Add("@FLG_INTEGRACAO", SqlDbType.Char).Value = "Z";

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

            if (Funcoes.strToInt(sid_id) != 0)
            {
                ConsultaFicha();
                divParceirosAlteracao.Visible = true;
            }
            else
            {
                divParceirosInclusao.Visible = true;

                SqlConnection mySelCadastro = new SqlConnection(Funcoes.conexao());
                mySelCadastro.Open();
                SqlCommand cmdSelCadastro = new SqlCommand("dbo.stp_pessoas_fj_configuracoes_ins", mySelCadastro);
                cmdSelCadastro.CommandType = CommandType.StoredProcedure;
                cmdSelCadastro.Parameters.Add("@flg_operacao", SqlDbType.Char).Value = "C";
                cmdSelCadastro.Parameters.Add("@COD_ID_PESSOA_LICENCIADO", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["LICENCIADO"].ToString());
                cmdSelCadastro.Parameters.Add("@COD_ID_PESSOAS_FJ", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["PESSOA"].ToString());
                SqlDataReader ReaderCadastro = cmdSelCadastro.ExecuteReader();
                while (ReaderCadastro.Read())
                {
                    ckbMulta.Checked = (ReaderCadastro["FLG_MULTA"].ToString().Trim() == "S") ? true : false;
                    ddlMulta.SelectedValue = ReaderCadastro["FLG_TIPO_MULTA"].ToString().Trim();
                    txtMulta.Text = ReaderCadastro["NUM_MULTA"].ToString().Trim();

                    ckbJuros.Checked = (ReaderCadastro["FLG_JUROS"].ToString().Trim() == "S") ? true : false;
                    ddlJuros.SelectedValue = ReaderCadastro["FLG_TIPO_JUROS"].ToString().Trim();
                    txtJuros.Text = ReaderCadastro["NUM_JUROS"].ToString().Trim();


                    ckbDesconto.Checked = (ReaderCadastro["FLG_DESCONTO"].ToString().Trim() == "S") ? true : false;
                    ddlDesconto.SelectedValue = ReaderCadastro["FLG_TIPO_DESCONTO"].ToString().Trim();
                    txtDesconto.Text = ReaderCadastro["NUM_DESCONTO"].ToString().Trim();
                    txtDias.Text = ReaderCadastro["NUM_DIAS"].ToString().Trim();
                }
                hrBoleto.HRef = "#";
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
            if (Funcoes.strToInt(ReaderCadastro["COD_ID_PESSOAS_FJ_COMPRADOR"].ToString()) > 0)
            {
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

            //ddlFormaRecebimento.Text = ReaderCadastro["FLG_TIPO"].ToString();
            txtNumeroDocumento.Text = ReaderCadastro["NOM_REFERENCIA_PRODUTO"].ToString();
            txtValor.Text = ReaderCadastro["NUM_VALOR"].ToString();
            txtVencimento.Text = ReaderCadastro["DTA_VENCIMENTO"].ToString();


            txtEmails.Text = ReaderCadastro["NOM_EMAIL_LINK"].ToString();

            ddlParcelas.SelectedValue = ReaderCadastro["NUM_PARCELAS"].ToString();
            txtDescricao.Text = ReaderCadastro["NOM_DESCRICAO_PRODUTO"].ToString();
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
            //txtAviso.Text = ReaderCadastro["NOM_DESCRICAO_PRODUTO"].ToString();
            ckbSplit.Checked = (ReaderCadastro["FLG_SPLIT"].ToString() == "S") ? true : false;
            ckbSplit_CheckedChanged(null, null);

        }

        ConsultaParceiros();

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
                txtTokenCliente.Text = ReaderCadastro["COD_ID_COMPRADOR"].ToString();

                txtEmails.Text = ReaderCadastro["NOM_EMAIL"].ToString();
            }
        }
    }


    protected void btnSalvar_Click(object sender, EventArgs e)
    {
        if (Funcoes.strToDouble(txtValor.Text.ToString().Trim()) > 0)
        {
            if (GravaCliente() == true)
            {
                if (GravarVendaCarne() == true)
                {
                    for (int i = 1; i <= Funcoes.strToInt(ddlParcelas.SelectedValue.ToString()); i++)
                    {
                        GravaParcelasCarne(i);
                    }
                    ClientScript.RegisterStartupScript(this.GetType(), "SucessoGerarCarne", "alert('Dados gravados com sucesso!'); ", true);

                    
                }
                else
                {
                    ClientScript.RegisterStartupScript(this.GetType(), "ErroGravarVenda", "alert('Ocorreu um erro ao tentar gerar o boleto bancário! Verifique e tente novamente.');", true);

                }
            }
            else
            {
                ClientScript.RegisterStartupScript(this.GetType(),
                   "ErroCliente", "alert('Ocorreu um erro ao tentar gravar os dados do cliente! Verifique e tente novamente.');", true);
            }
        }
        else
        {
            ClientScript.RegisterStartupScript(this.GetType(),
               "ErroCliente", "alert('Não foi especificado o valor da venda! Verifique e reentre');", true);
        }
    }

    private bool GravaCliente()
    {
        bool bRetorno = false;
        string retornoInclusao = "";

        // Verifica se os campos estão preenchidos
        if (
            (txtNomeCliente.Text.ToString().Trim() == "") ||
            (txtEmailCliente.Text.ToString().Trim() == "") ||
            (txtCelularCliente.Text.ToString().Trim() == "") ||
            (txtDocumentoCliente.Text.ToString().Trim() == "") ||
            (ddlEstadoCliente.SelectedValue.ToString().Trim() == "")
            )
        {
            ClientScript.RegisterStartupScript(this.GetType(),
               "ErroDadosCliente", "alert('Os campos dos dados do cliente são de preenchimento obrigatório! Verifique e tente novamente.');", true);
            return false;
        }

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

        // Verifica se o Cliente já existe na zoop
        string jsonRetornoComprador = "";

        string jsonRetornoConsulta = zoop.compradores("P", txtDocumentoCliente.Text.ToString(), "");
        if (jsonRetornoConsulta.ToString().Trim() != "")
        {
            try
            {
                JObject oRetornoConsulta = JObject.Parse(jsonRetornoConsulta);
                retornoInclusao = oRetornoConsulta["id"].ToString();
                txtTokenCliente.Text = oRetornoConsulta["id"].ToString();
                txtIDZOOPCliente.Text = oRetornoConsulta["id"].ToString();

                jsonRetornoComprador = zoop.compradores("A", retornoInclusao.ToString(), json);
            }
            catch
            {
                retornoInclusao = "";
                jsonRetornoComprador = zoop.compradores("I", "", json);
            }
        }

        JObject oRetornoComprador = JObject.Parse(jsonRetornoComprador);
        try
        {

            // Verificar se este comprador já existe ou não
            txtTokenCliente.Text = oRetornoComprador["id"].ToString();
            txtIDZOOPCliente.Text = oRetornoComprador["id"].ToString();
            // Grava os dados do Cliente na Base de dados

            SqlConnection connInsConsCliente = new SqlConnection(Funcoes.conexao());
            connInsConsCliente.Open();
            SqlCommand cmdInsConsCliente = new SqlCommand("dbo.stp_compradores_ins", connInsConsCliente);
            cmdInsConsCliente.CommandType = CommandType.StoredProcedure;

            cmdInsConsCliente.Parameters.Add("@flg_operacao", SqlDbType.Char).Value = 'I';
            cmdInsConsCliente.Parameters.Add("@FLG_ATIVO", SqlDbType.Char).Value = "S";
            cmdInsConsCliente.Parameters.Add("@COD_ID_PESSOA_LICENCIADO", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["LICENCIADO"].ToString());
            cmdInsConsCliente.Parameters.Add("@COD_ID_PESSOAS_FJ", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["PESSOA"].ToString());
            cmdInsConsCliente.Parameters.Add("@COD_ID_COMPRADOR", SqlDbType.VarChar).Value = retornoInclusao.ToString();

            cmdInsConsCliente.Parameters.Add("@DTA_DATA", SqlDbType.DateTime).Value = DateTime.Now;
            cmdInsConsCliente.Parameters.Add("@FLG_TIPO", SqlDbType.Char).Value = "C";
            cmdInsConsCliente.Parameters.Add("@FLG_INTEGRACAO", SqlDbType.Char).Value = "Z";

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

            bRetorno = true;
        }
        catch
        {
            bRetorno = false;
        }

        return bRetorno;
    }


    private bool GravarVendaCarne()
    {
        bool bRetorno = false;
        string iTransacao = "";

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
        cmdInsCons.Parameters.Add("@FLG_CLIENTE", SqlDbType.Char).Value = "S";
        cmdInsCons.Parameters.Add("@FLG_TIPO", SqlDbType.Char).Value = "N";

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


        // Cliente
        cmdInsCons.Parameters.Add("@COD_ID_COMPRADOR", SqlDbType.VarChar).Value = txtIDZOOPCliente.Text.ToString();

        cmdInsCons.Parameters.Add("@NOM_REFERENCIA_PRODUTO", SqlDbType.VarChar).Value = txtNumeroDocumento.Text.ToString();
        string sDescricao = txtDescricao.Text.ToString();
        if (txtAviso.Text.ToString().Trim() != "") { sDescricao = sDescricao +  " / " + txtAviso.Text.ToString();}
        
        cmdInsCons.Parameters.Add("@NOM_DESCRICAO_PRODUTO", SqlDbType.Text).Value = sDescricao.ToString();

        cmdInsCons.Parameters.Add("@FLG_LINK_PERMANENTE", SqlDbType.Char).Value = "N";

        cmdInsCons.Parameters.Add("@FLG_BOLETO", SqlDbType.Char).Value = "S";
        cmdInsCons.Parameters.Add("@FLG_PIX", SqlDbType.Char).Value = "N";
        cmdInsCons.Parameters.Add("@FLG_CARTAO_CREDITO", SqlDbType.Char).Value = "N";

        cmdInsCons.Parameters.Add("@FLG_TIPO_COBRANCA", SqlDbType.Char).Value = "N";

        cmdInsCons.Parameters.Add("@FLG_TIPO_OPERACAO", SqlDbType.Char).Value = "C";
        cmdInsCons.Parameters.Add("@FLG_OPERADORA", SqlDbType.Char).Value = "Z";

        cmdInsCons.Parameters.Add("@FLG_PRECO_PARCELAMENTO", SqlDbType.Char).Value = "N";
        cmdInsCons.Parameters.Add("@FLG_SPLIT", SqlDbType.Char).Value = (ckbSplit.Checked == true) ? "S" : "N";

        // Multa
        cmdInsCons.Parameters.Add("@FLG_MULTA", SqlDbType.Char).Value = (ckbMulta.Checked == true) ? "S" : "N";
        cmdInsCons.Parameters.Add("@FLG_TIPO_MULTA", SqlDbType.Char).Value = ddlMulta.SelectedValue.ToString();
        cmdInsCons.Parameters.Add("@NUM_MULTA", SqlDbType.Float).Value = Funcoes.strToDouble(txtMulta.Text.ToString());

        // Juros
        cmdInsCons.Parameters.Add("@FLG_JUROS", SqlDbType.Char).Value = (ckbJuros.Checked == true) ? "S" : "N";
        cmdInsCons.Parameters.Add("@FLG_TIPO_JUROS", SqlDbType.Char).Value = ddlJuros.SelectedValue.ToString();
        cmdInsCons.Parameters.Add("@NUM_JUROS", SqlDbType.Float).Value = Funcoes.strToDouble(txtJuros.Text.ToString());

        // Desconto
        cmdInsCons.Parameters.Add("@FLG_DESCONTO", SqlDbType.Char).Value = (ckbDesconto.Checked == true) ? "S" : "N";
        cmdInsCons.Parameters.Add("@FLG_TIPO_DESCONTO", SqlDbType.Char).Value = ddlDesconto.SelectedValue.ToString();
        cmdInsCons.Parameters.Add("@NUM_DESCONTO", SqlDbType.Float).Value = Funcoes.strToDouble(txtDesconto.Text.ToString());
        cmdInsCons.Parameters.Add("@NUM_DIAS", SqlDbType.Int).Value = Funcoes.strToInt(txtDias.Text.ToString());

        cmdInsCons.Parameters.Add("@NOM_EMAIL_LINK", SqlDbType.VarChar).Value = txtEmails.Text.ToString();

        cmdInsCons.Parameters.Add("@NUM_PARCELAS", SqlDbType.Int).Value = Funcoes.strToInt(ddlParcelas.SelectedValue.ToString());

        cmdInsCons.Parameters.Add("@NUM_VALOR", SqlDbType.Float).Value = Funcoes.strToDouble(txtValor.Text.ToString());
        if (txtVencimento.Text.ToString().Trim() != "") { cmdInsCons.Parameters.Add("@DTA_VENCIMENTO", SqlDbType.DateTime).Value = Convert.ToDateTime(txtVencimento.Text.ToString()); }

        string urlorigem = Request.ServerVariables["SERVER_NAME"].ToString();
        cmdInsCons.Parameters.Add("@NOM_ORIGEM", SqlDbType.VarChar).Value = urlorigem.ToString() + "/boletocarne.aspx?id=";

        if (Funcoes.strToInt(sid_id) != 0)
        {
            cmdInsCons.ExecuteNonQuery();
            iTransacao = sid_id.ToString();
        }
        else
        {
            iTransacao = cmdInsCons.ExecuteScalar().ToString();
        }
        connInsCons.Close();
        connInsCons.Dispose();

        txtCodigo.Text = iTransacao.ToString();

        // Insere Vendas_parceiros - split
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

        connInsCons.Close();
        connInsCons.Dispose();

        bRetorno = true;

        return bRetorno;
    }





    protected void btnCancelar_Click(object sender, EventArgs e)
    {
        ClientScript.RegisterStartupScript(this.GetType(), "logoff", "opener.PostBackOnMainPage(); window.close(); ", true);
    }
    protected void btnNovoCliente_Click(object sender, EventArgs e)
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

    protected void ddlComprador_SelectedIndexChanged(object sender, EventArgs e)
    {
        CarregaCliente();
    }
    protected void ckbSplit_CheckedChanged(object sender, EventArgs e)
    {
        divSplit.Visible = (ckbSplit.Checked) ? true : false;
        divSplit.Focus();

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
            txtParceiro.Text = ReaderCadastro["NOM_RAZAOSOCIAL"].ToString() + "(" + ReaderCadastro["NOM_NOME"].ToString() + " " + ReaderCadastro["NOM_SOBRENOME"].ToString() + ")";
            txtValorParceiro.Text = String.Format("{0:n0}", Funcoes.strToDouble("0"));
            txtPercentualParceiro.Text = String.Format("{0:n0}", Funcoes.strToDouble("0"));
        }

    }
    protected void btNovoParceiro_Click(object sender, EventArgs e)
    {
        ClientScript.RegisterStartupScript(this.GetType(),
"CadastroParceiro", "openPopupWindow('cad_parceiros.aspx?id=0','Parceiros',1024,800);", true);

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
    protected void lsvParceiros_ItemCommand(object sender, ListViewCommandEventArgs e)
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

    public string GetPostBackScript()
    {
        PostBackOptions options = new PostBackOptions(btnPostback);
        Page.ClientScript.RegisterForEventValidation(options);

        return Page.ClientScript.GetPostBackEventReference(options);
    }

    protected void btnPostBack_Click(object sender, EventArgs e)
    {
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

    public bool GravarDadosVenda()
    {
        try
        {
            vendas.Venda venda = new vendas.Venda()
            {
                CodId = 1,
                CodIdPessoaLicenciado = Funcoes.strToInt(HttpContext.Current.Session["LICENCIADO"].ToString()),
                CodIdPessoasFjVendedor = Funcoes.strToInt(HttpContext.Current.Session["PESSOA"].ToString()),
                CodIdPessoasFjComprador = Funcoes.strToInt(ddlComprador.SelectedValue.ToString()),
                DtaData = DateTime.Now,
                CodIdProduto = 1,
                NumValor = Funcoes.strToDouble(txtValor.Text.ToString()),
                DtaVencimento = Convert.ToDateTime(txtVencimento.Text.ToString()), //Verificar o preenchimento
                //FlgTipo = ddlFormaRecebimento.SelectedValue.ToString(),
                FlgMulta = (ckbMulta.Checked == true) ? "S" : "N",
                FlgTipoMulta = ddlMulta.SelectedValue.ToString(),
                NumMulta = Funcoes.strToDouble(txtMulta.Text.ToString()),
                FlgJuros = (ckbJuros.Checked == true) ? "S" : "N",
                FlgTipoJuros = ddlJuros.SelectedValue.ToString(),
                NumJuros = Funcoes.strToDouble(txtJuros.Text.ToString()),
                FlgDesconto = (ckbDesconto.Checked == true) ? "S" : "N",
                FlgTipoDesconto = ddlDesconto.SelectedValue.ToString(),
                NumDesconto = Funcoes.strToDouble(txtDesconto.Text.ToString()),
                NumDias = Funcoes.strToInt(txtDias.Text.ToString()),

                FlgAviso = "",
                DesAviso = "",
                DtaPixVencimento = DateTime.Now,
                NumCartao = "",
                NomCartao = "",
                NumMesCartao = 1,
                NumAnoCartao = 1,
                NumCodigoCartao = 1,
                NomPix = "",
                NomBoleto = "",
                NomCode = "",
                NomCheckout = "",
                CodIdTransacao = 1,
                NomImagemPix = "",
                NomOrigem = "",
                DesJson = "",
                NumParcelas = Funcoes.strToInt(ddlParcelas.SelectedValue.ToString()),
                FlgTipoCobranca = "U",
                FlgBoleto = "S",
                FlgPix = "N",
                FlgCartaoCredito = "N",
                FlgCliente = "S",
                NomReferenciaProduto = txtNumeroDocumento.Text.ToString(),
                NomDescricaoProduto = txtAviso.Text.ToString(),
                FlgProduto = "",

                FlgTipoOperacao = "C",
                FlgOperadora = "Z",
                FlgPrecoParcelamento = "",
                FlgSplit = (ckbSplit.Checked == true) ? "S" : "N",
                NomEmailLink = txtEmailCliente.Text.ToString(),

                NumCnpjCpf = txtDocumentoCliente.Text.ToString(),
                NomNome = txtNomeCliente.Text.ToString(),
                NomSobrenome = txtSobrenomeCliente.Text.ToString(),
                NomEndereco = txtEnderecoCliente.Text.ToString(),
                NomBairro = txtBairroCliente.Text.ToString(),
                NomCidade = txtCidadeCliente.Text.ToString(),
                NomUf = ddlEstadoCliente.SelectedValue.ToString(),
                NomCep = txtCEPCliente.Text.ToString(),
                NomEmail = txtEmailCliente.Text.ToString(),
                NomNumero = txtNumeroCliente.Text.ToString(),
                NomComplemento = txtComplementoCliente.Text.ToString(),
                NomPais = "BR",
                NomCelular = txtCelularCliente.Text.ToString(),

                CodIdComprador = txtIDZOOPCliente.Text.ToString(),
                NomImagem = "",
                FlgFormaSplit = "",
                NomBarcodeBoleto = "",
                FlgExibirProdutos = "",
                NumValor01 = 0,
                NumValor02 = 0,
                NumValor03 = 0,
                NumValor04 = 0,
                NumValor05 = 0,
                NumValor06 = 0,
                NumValor07 = 0,
                NumValor08 = 0,
                NumValor09 = 0,
                NumValor10 = 0,
                NumValor11 = 0,
                NumValor12 = 0,
                FlgLinkPermanente = "N",
                NomCampo01 = "",
                NomConteudoCampo01 = "",
                NumDiasVencimento = 1,
                FlgIntegracao = "Z",
            };

            return true;
        }
        catch
        {
            return false;
        }
    }


    private bool GravarVenda()
    {
        bool bRetorno = false;
        string iTransacao = "";

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
        cmdInsCons.Parameters.Add("@FLG_CLIENTE", SqlDbType.Char).Value = "S";

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


        // Cliente
        cmdInsCons.Parameters.Add("@COD_ID_COMPRADOR", SqlDbType.VarChar).Value = txtIDZOOPCliente.Text.ToString();
        cmdInsCons.Parameters.Add("@FLG_TIPO", SqlDbType.Char).Value = "N";

        cmdInsCons.Parameters.Add("@NOM_REFERENCIA_PRODUTO", SqlDbType.VarChar).Value = txtNumeroDocumento.Text.ToString();
        cmdInsCons.Parameters.Add("@NOM_DESCRICAO_PRODUTO", SqlDbType.Text).Value = txtAviso.Text.ToString();

        cmdInsCons.Parameters.Add("@FLG_LINK_PERMANENTE", SqlDbType.Char).Value = "N";

        cmdInsCons.Parameters.Add("@FLG_BOLETO", SqlDbType.Char).Value = "S";
        cmdInsCons.Parameters.Add("@FLG_PIX", SqlDbType.Char).Value = "N";
        cmdInsCons.Parameters.Add("@FLG_CARTAO_CREDITO", SqlDbType.Char).Value = "N";

        cmdInsCons.Parameters.Add("@FLG_TIPO_COBRANCA", SqlDbType.Char).Value = "N";

        cmdInsCons.Parameters.Add("@FLG_TIPO_OPERACAO", SqlDbType.Char).Value = "C";
        cmdInsCons.Parameters.Add("@FLG_OPERADORA", SqlDbType.Char).Value = "Z";

        cmdInsCons.Parameters.Add("@FLG_PRECO_PARCELAMENTO", SqlDbType.Char).Value = "N";
        cmdInsCons.Parameters.Add("@FLG_SPLIT", SqlDbType.Char).Value = (ckbSplit.Checked == true) ? "S" : "N";

        cmdInsCons.Parameters.Add("@NOM_EMAIL_LINK", SqlDbType.VarChar).Value = txtEmails.Text.ToString();

        // Multa
        cmdInsCons.Parameters.Add("@FLG_MULTA", SqlDbType.Char).Value = (ckbMulta.Checked == true) ? "S" : "N";
        cmdInsCons.Parameters.Add("@FLG_TIPO_MULTA", SqlDbType.Char).Value = ddlMulta.SelectedValue.ToString();
        cmdInsCons.Parameters.Add("@NUM_MULTA", SqlDbType.Float).Value = Funcoes.strToDouble(txtMulta.Text.ToString());

        // Juros
        cmdInsCons.Parameters.Add("@FLG_JUROS", SqlDbType.Char).Value = (ckbJuros.Checked == true) ? "S" : "N";
        cmdInsCons.Parameters.Add("@FLG_TIPO_JUROS", SqlDbType.Char).Value = ddlJuros.SelectedValue.ToString();
        cmdInsCons.Parameters.Add("@NUM_JUROS", SqlDbType.Float).Value = Funcoes.strToDouble(txtJuros.Text.ToString());

        // Desconto
        cmdInsCons.Parameters.Add("@FLG_DESCONTO", SqlDbType.Char).Value = (ckbDesconto.Checked == true) ? "S" : "N";
        cmdInsCons.Parameters.Add("@FLG_TIPO_DESCONTO", SqlDbType.Char).Value = ddlDesconto.SelectedValue.ToString();
        cmdInsCons.Parameters.Add("@NUM_DESCONTO", SqlDbType.Float).Value = Funcoes.strToDouble(txtDesconto.Text.ToString());
        cmdInsCons.Parameters.Add("@NUM_DIAS", SqlDbType.Int).Value = Funcoes.strToInt(txtDias.Text.ToString());

        cmdInsCons.Parameters.Add("@NOM_EMAIL_LINK", SqlDbType.VarChar).Value = txtEmails.Text.ToString();

        cmdInsCons.Parameters.Add("@NUM_PARCELAS", SqlDbType.Int).Value = Funcoes.strToInt(ddlParcelas.SelectedValue.ToString());

        cmdInsCons.Parameters.Add("@NUM_VALOR", SqlDbType.Float).Value = Funcoes.strToDouble(txtValor.Text.ToString());
        if (txtVencimento.Text.ToString().Trim() != "") { cmdInsCons.Parameters.Add("@DTA_VENCIMENTO", SqlDbType.DateTime).Value = Convert.ToDateTime(txtVencimento.Text.ToString()); }

        string urlorigem = Request.ServerVariables["SERVER_NAME"].ToString();
        cmdInsCons.Parameters.Add("@NOM_ORIGEM", SqlDbType.VarChar).Value = urlorigem.ToString() + "/boletocarne.aspx?id=";


        if (Funcoes.strToInt(sid_id) != 0)
        {
            cmdInsCons.ExecuteNonQuery();
            iTransacao = sid_id.ToString();
        }
        else
        {
            iTransacao = cmdInsCons.ExecuteScalar().ToString();
        }
        connInsCons.Close();
        connInsCons.Dispose();        
 
        // Insere Vendas_parceiros - split
        if (Funcoes.strToInt(sid_id) == 0)
        {
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

        connInsCons.Close();
        connInsCons.Dispose();

        ClientScript.RegisterStartupScript(this.GetType(),
           "Alerta", "alert('Dados gravados com sucesso!');", true);
        bRetorno = true;

        return bRetorno;
    }

    private void GravaParcelasCarne(int iParcela)
    {

        string sTokenComprador = "";

        // CARREGAR DADOS DADOS VENDA OPERACAO = 'O'
        SqlConnection mySelCadastro = new SqlConnection(Funcoes.conexao());
        mySelCadastro.Open();
        SqlCommand cmdSelCadastro = new SqlCommand("dbo.stp_vendas_ins", mySelCadastro);
        cmdSelCadastro.CommandType = CommandType.StoredProcedure;
        cmdSelCadastro.Parameters.Add("@flg_operacao", SqlDbType.Char).Value = "O";
        cmdSelCadastro.Parameters.Add("@COD_ID", SqlDbType.Int).Value = Funcoes.strToInt(txtCodigo.Text.ToString());
        SqlDataReader ReaderCadastro = cmdSelCadastro.ExecuteReader();

        while (ReaderCadastro.Read())
        {

            SqlConnection mySelCadastroComprador = new SqlConnection(Funcoes.conexao());
            mySelCadastroComprador.Open();
            SqlCommand cmdSelCadastroComprador = new SqlCommand("dbo.stp_compradores_ins", mySelCadastroComprador);
            cmdSelCadastroComprador.CommandType = CommandType.StoredProcedure;
            cmdSelCadastroComprador.Parameters.Add("@flg_operacao", SqlDbType.Char).Value = "H";
            cmdSelCadastroComprador.Parameters.Add("@NUM_CNPJCPF", SqlDbType.VarChar).Value = txtDocumentoCliente.Text.ToString();
            cmdSelCadastroComprador.Parameters.Add("@COD_ID_PESSOA_LICENCIADO", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["LICENCIADO"].ToString());
            cmdSelCadastroComprador.Parameters.Add("@COD_ID_PESSOAS_FJ", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["PESSOA"].ToString());
            cmdSelCadastroComprador.Parameters.Add("@FLG_TIPO", SqlDbType.Char).Value = "C";

            SqlDataReader ReaderCadastroComprador = cmdSelCadastroComprador.ExecuteReader();
            while (ReaderCadastroComprador.Read())
            {
                sTokenComprador = ReaderCadastroComprador["NOM_TOKEN_ZOOP_COMPRADOR"].ToString();
                string jsonCompradorBusca = zoop.compradores("P", txtDocumentoCliente.Text.ToString(), "");
                try
                {
                    JObject oRetornoCompradorBusca = JObject.Parse(jsonCompradorBusca);
                    // Verificar se este comprador já existe ou não
                    sTokenComprador = oRetornoCompradorBusca["id"].ToString();
                }
                catch
                {

                }
            }


            double valorCarne = Funcoes.strToDouble(txtValor.Text);
            int parcelasCarne = Funcoes.strToInt(ddlParcelas.SelectedValue);

            //int amount = (int)Math.Round((valor / parcelas) * 100);

            DadosTransacao.TransacaoBoleto dtransacao = new DadosTransacao.TransacaoBoleto()
            {
                on_behalf_of = ReaderCadastro["NUM_TOKEN"].ToString(),
                customer = sTokenComprador,
                amount = (int)Math.Round((valorCarne / parcelasCarne) * 100),//Funcoes.strToInt(Convert.ToString((Funcoes.strToDouble(txtValor.Text.ToString()) / Funcoes.strToInt(ddlParcelas.SelectedValue.ToString())) * 100).ToString()),
                currency = "BRL",
                description = ReaderCadastro["NOM_DESCRICAO_PRODUTO"].ToString(),
                payment_type = "boleto",
                reference_id = ReaderCadastro["NOM_REFERENCIA_PRODUTO"].ToString(),

                payment_method = new DadosTransacao.payment_method()
                {
                    expiration_date = Convert.ToDateTime(txtVencimento.Text.ToString()).AddMonths(iParcela - 1).Year.ToString().PadLeft(4, '0') + "-" + Convert.ToDateTime(txtVencimento.Text.ToString()).AddMonths(iParcela - 1).Month.ToString().PadLeft(2, '0') + "-" + Convert.ToDateTime(txtVencimento.Text.ToString()).AddMonths(iParcela - 1).Day.ToString().PadLeft(2, '0'),
                    payment_limit_date = Convert.ToDateTime(txtVencimento.Text.ToString()).AddMonths(iParcela - 1).Year.ToString().PadLeft(4, '0') + "-" + Convert.ToDateTime(txtVencimento.Text.ToString()).AddMonths(iParcela - 1).Month.ToString().PadLeft(2, '0') + "-" + Convert.ToDateTime(txtVencimento.Text.ToString()).AddMonths(iParcela - 1).Day.ToString().PadLeft(2, '0'),
                    billing_instructions = null,
                    body_instructions = new string[]
                    {  
                        ReaderCadastro["NOM_DESCRICAO_PRODUTO"].ToString() + " - Parcela:" + iParcela.ToString().PadLeft(2, '0') + "/" + ddlParcelas.SelectedValue.ToString().PadLeft(2, '0'),
                    }
                }
            };

            
            
            // Verifica a existência de split Manual
            SqlConnection mySelCadastroSplit = new SqlConnection(Funcoes.conexao());
            mySelCadastroSplit.Open();
            SqlCommand cmdSelCadastroSplit = new SqlCommand("dbo.stp_vendas_parceiros_ins", mySelCadastroSplit);
            cmdSelCadastroSplit.CommandType = CommandType.StoredProcedure;
            cmdSelCadastroSplit.Parameters.Add("@flg_operacao", SqlDbType.Char).Value = "C";
            cmdSelCadastroSplit.Parameters.Add("@COD_ID_VENDAS", SqlDbType.Int).Value = Funcoes.strToInt(txtCodigo.Text.ToString());
            cmdSelCadastroSplit.Parameters.Add("@COD_ID_PESSOA_LICENCIADO", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["LICENCIADO"].ToString());
            SqlDataReader ReaderCadastroSplit = cmdSelCadastroSplit.ExecuteReader();

            List<DadosTransacao.split_rules> listaSplits = new List<DadosTransacao.split_rules>();

            int iContadorSplit = 0;
            
            while (ReaderCadastroSplit.Read()) // Percorre os registros do banco
            {
                iContadorSplit = iContadorSplit + 1;
                if (Funcoes.strToDouble(ReaderCadastroSplit["NUM_VALOR"].ToString()) > 0)
                {
                    listaSplits.Add(
                    new DadosTransacao.split_rules()
                    {
                        recipient = ReaderCadastroSplit["NOM_TOKEN_ZOOP_PARCEIRO"].ToString(),
                        liable = true,
                        charge_processing_fee = true,
                        amount = Funcoes.strToInt((Funcoes.strToDouble(ReaderCadastroSplit["NUM_PERCENTUAL"].ToString()) * 100).ToString()),
                    });
                }
                if (Funcoes.strToDouble(ReaderCadastroSplit["NUM_PERCENTUAL"].ToString()) > 0)
                {
                    listaSplits.Add(
                    new DadosTransacao.split_rules()
                    {
                        recipient = ReaderCadastroSplit["NOM_TOKEN_ZOOP_PARCEIRO"].ToString(),
                        liable = true,
                        charge_processing_fee = true,
                        percentage = Funcoes.strToDouble(ReaderCadastroSplit["NUM_PERCENTUAL"].ToString()),
                    });
                }
            }

            // Converte a lista para array antes de atribuir ao objeto
            if (iContadorSplit > 0) { dtransacao.split_rules = listaSplits.ToArray(); }
            // Fim Split Manual

            string jsonBoleto = JsonConvert.SerializeObject(dtransacao);
//            txtAviso.Text = jsonBoleto;
//            return;

            string jsonRetorno = zoop.transacao("I", "", jsonBoleto);

            JObject oRetorno = JObject.Parse(jsonRetorno);

            SqlConnection connInsCons = new SqlConnection(Funcoes.conexao());
            connInsCons.Open();
            SqlCommand cmdInsCons = new SqlCommand("dbo.stp_vendas_vendas_ins", connInsCons);
            cmdInsCons.CommandType = CommandType.StoredProcedure;
            cmdInsCons.Parameters.Add("@flg_operacao", SqlDbType.Char).Value = 'I';

            //cmdInsCons.Parameters.Add("@COD_ID", SqlDbType.Int).Value = Funcoes.strToInt(sid_id.ToString());

            cmdInsCons.Parameters.Add("@COD_ID_PESSOA_LICENCIADO", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["LICENCIADO"].ToString());
            cmdInsCons.Parameters.Add("@COD_ID_PESSOAS_FJ_VENDEDOR", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["PESSOA"].ToString());
            // Buscar dados comprador

            cmdInsCons.Parameters.Add("@COD_ID_VENDAS", SqlDbType.Int).Value = Funcoes.strToInt(txtCodigo.Text.ToString());

            cmdInsCons.Parameters.Add("@NUM_VALOR", SqlDbType.Float).Value = Funcoes.strToDouble(txtValor.Text.ToString()) / Funcoes.strToInt(ddlParcelas.SelectedValue.ToString());
            cmdInsCons.Parameters.Add("@NUM_PARCELAS", SqlDbType.Int).Value = iParcela;
            if (txtVencimento.Text.ToString().Trim() != "") { cmdInsCons.Parameters.Add("@DTA_VENCIMENTO", SqlDbType.DateTime).Value = Convert.ToDateTime(txtVencimento.Text.ToString()).AddMonths(iParcela - 1); }

            cmdInsCons.Parameters.Add("@FLG_BOLETO", SqlDbType.VarChar).Value = "S";
            cmdInsCons.Parameters.Add("@NOM_CODE", SqlDbType.VarChar).Value = oRetorno["id"].ToString().Trim();

            cmdInsCons.Parameters.Add("@NOM_REFERENCIA_PRODUTO", SqlDbType.VarChar).Value = txtNumeroDocumento.Text.ToString();
            string sDescricao = txtDescricao.Text.ToString();
            if (txtAviso.Text.ToString().Trim() != "") { sDescricao = sDescricao + " / " + txtAviso.Text.ToString(); }

            sDescricao = sDescricao + " - Parcela:" + iParcela.ToString().PadLeft(2, '0') + "/" + ddlParcelas.SelectedValue.ToString().PadLeft(2, '0');

            cmdInsCons.Parameters.Add("@NOM_DESCRICAO_PRODUTO", SqlDbType.Text).Value = sDescricao.ToString();

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

            cmdInsCons.Parameters.Add("@DES_JSON", SqlDbType.Text).Value = jsonRetorno.ToString();
            cmdInsCons.Parameters.Add("@FLG_INTEGRACAO", SqlDbType.Char).Value = "Z";
            cmdInsCons.Parameters.Add("@FLG_TIPO", SqlDbType.Char).Value = "N";


            cmdInsCons.ExecuteNonQuery();
            connInsCons.Close();
            connInsCons.Dispose();
        }

    }
}