using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
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

using System.Collections.Specialized;

/// <summary>
/// Summary description for vendas
/// </summary>
public class vendas
{
    // Classes ----------------------------------------------------------------------------------------------------------------------------------------------------------------

    public class Venda
    {
        public int CodId { get; set; }
        public int? CodIdPessoaLicenciado { get; set; }
        public int? CodIdPessoasFjVendedor { get; set; }
        public int? CodIdPessoasFjComprador { get; set; }
        public DateTime? DtaData { get; set; }
        public int? CodIdProduto { get; set; }
        public double NumValor { get; set; }
        public DateTime? DtaVencimento { get; set; }
        public string FlgTipo { get; set; }
        public string FlgMulta { get; set; }
        public string FlgTipoMulta { get; set; }
        public double NumMulta { get; set; }
        public string FlgJuros { get; set; }
        public string FlgTipoJuros { get; set; }
        public double NumJuros { get; set; }
        public string FlgDesconto { get; set; }
        public string FlgTipoDesconto { get; set; }
        public double NumDesconto { get; set; }
        public int? NumDias { get; set; }
        public string FlgAviso { get; set; }
        public string DesAviso { get; set; }
        public DateTime? DtaPixVencimento { get; set; }
        public string NumCartao { get; set; }
        public string NomCartao { get; set; }
        public int? NumMesCartao { get; set; }
        public int? NumAnoCartao { get; set; }
        public int? NumCodigoCartao { get; set; }
        public string NomPix { get; set; }
        public string NomBoleto { get; set; }
        public string NomCode { get; set; }
        public string NomCheckout { get; set; }
        public int? CodIdTransacao { get; set; }
        public string NomImagemPix { get; set; }
        public string NomOrigem { get; set; }
        public string DesJson { get; set; }
        public int? NumParcelas { get; set; }
        public string FlgTipoCobranca { get; set; }
        public string FlgBoleto { get; set; }
        public string FlgPix { get; set; }
        public string FlgCartaoCredito { get; set; }
        public string FlgCliente { get; set; }
        public string NomReferenciaProduto { get; set; }
        public string NomDescricaoProduto { get; set; }
        public string FlgProduto { get; set; }
        public string FlgTipoOperacao { get; set; }
        public string FlgOperadora { get; set; }
        public string FlgPrecoParcelamento { get; set; }
        public string FlgSplit { get; set; }
        public string NomEmailLink { get; set; }
        public string NumCnpjCpf { get; set; }
        public string NomNome { get; set; }
        public string NomSobrenome { get; set; }
        public string NomEndereco { get; set; }
        public string NomBairro { get; set; }
        public string NomCidade { get; set; }
        public string NomUf { get; set; }
        public string NomCep { get; set; }
        public string NomEmail { get; set; }
        public string NomNumero { get; set; }
        public string NomComplemento { get; set; }
        public string NomPais { get; set; }
        public string NomCelular { get; set; }
        public string CodIdComprador { get; set; }
        public string NomImagem { get; set; }
        public string FlgFormaSplit { get; set; }
        public string NomBarcodeBoleto { get; set; }
        public string FlgExibirProdutos { get; set; }
        public double NumValor01 { get; set; }
        public double NumValor02 { get; set; }
        public double NumValor03 { get; set; }
        public double NumValor04 { get; set; }
        public double NumValor05 { get; set; }
        public double NumValor06 { get; set; }
        public double NumValor07 { get; set; }
        public double NumValor08 { get; set; }
        public double NumValor09 { get; set; }
        public double NumValor10 { get; set; }
        public double NumValor11 { get; set; }
        public double NumValor12 { get; set; }
        public string FlgLinkPermanente { get; set; }
        public string NomCampo01 { get; set; }
        public string NomConteudoCampo01 { get; set; }
        public int? NumDiasVencimento { get; set; }
        public string FlgIntegracao { get; set; }
    }
    
    // Processos ----------------------------------------------------------------------------------------------------------------------------------------------------------------
    /*
    public static bool GravarVenda(vendas.Venda MinhaVenda)
    {
        

        SqlConnection connInsCons = new SqlConnection(Funcoes.conexao());
        connInsCons.Open();
        SqlCommand cmdInsCons = new SqlCommand("dbo.stp_vendas_ins", connInsCons);
        cmdInsCons.CommandType = CommandType.StoredProcedure;
        cmdInsCons.Parameters.Add("@flg_operacao", SqlDbType.Char).Value = 'I';
        cmdInsCons.Parameters.Add("@COD_ID_PESSOA_LICENCIADO", SqlDbType.Int).Value =  MinhaVenda.CodIdPessoaLicenciado;
        cmdInsCons.Parameters.Add("@COD_ID_PESSOAS_FJ_VENDEDOR", SqlDbType.Int).Value = MinhaVenda.CodIdPessoasFjVendedor;
        cmdInsCons.Parameters.Add("@COD_ID_PESSOAS_FJ_COMPRADOR", SqlDbType.Int).Value = MinhaVenda.CodIdPessoasFjComprador;
        cmdInsCons.Parameters.Add("@FLG_INTEGRACAO", SqlDbType.Char).Value = MinhaVenda.FlgIntegracao;

        cmdInsCons.Parameters.Add("@FLG_CLIENTE", SqlDbType.Char).Value = MinhaVenda.FlgCliente;

        // Cliente
        cmdInsCons.Parameters.Add("@NOM_NOME", SqlDbType.VarChar).Value = MinhaVenda.NomNome;
        cmdInsCons.Parameters.Add("@NOM_SOBRENOME", SqlDbType.VarChar).Value = MinhaVenda.NomSobrenome;
        cmdInsCons.Parameters.Add("@NUM_CNPJCPF", SqlDbType.VarChar).Value = MinhaVenda.NumCnpjCpf;
        cmdInsCons.Parameters.Add("@NOM_EMAIL", SqlDbType.VarChar).Value = MinhaVenda.NomEmail;
        cmdInsCons.Parameters.Add("@NOM_CELULAR", SqlDbType.VarChar).Value = MinhaVenda.NomCelular;

        // Endereço
        cmdInsCons.Parameters.Add("@NOM_ENDERECO", SqlDbType.VarChar).Value = MinhaVenda.NomEndereco;
        cmdInsCons.Parameters.Add("@NOM_NUMERO", SqlDbType.VarChar).Value = MinhaVenda.NomNumero;
        cmdInsCons.Parameters.Add("@NOM_COMPLEMENTO", SqlDbType.VarChar).Value = MinhaVenda.NomComplemento;
        cmdInsCons.Parameters.Add("@NOM_BAIRRO", SqlDbType.VarChar).Value = MinhaVenda.NomBairro;
        cmdInsCons.Parameters.Add("@NOM_CIDADE", SqlDbType.VarChar).Value = MinhaVenda.NomCidade;
        cmdInsCons.Parameters.Add("@NOM_UF", SqlDbType.VarChar).Value = MinhaVenda.NomUf;
        cmdInsCons.Parameters.Add("@NOM_CEP", SqlDbType.VarChar).Value = MinhaVenda.NomCep;
        cmdInsCons.Parameters.Add("@NOM_PAIS", SqlDbType.VarChar).Value = MinhaVenda.NomPais;

        cmdInsCons.Parameters.Add("@NOM_REFERENCIA_PRODUTO", SqlDbType.VarChar).Value = MinhaVenda.NomReferenciaProduto;
        cmdInsCons.Parameters.Add("@NOM_DESCRICAO_PRODUTO", SqlDbType.Text).Value = MinhaVenda.NomDescricaoProduto;

        cmdInsCons.Parameters.Add("@FLG_PRODUTO", SqlDbType.Char).Value = MinhaVenda.FlgProduto;
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

        if (prc01.Checked == true) { cmdInsCons.Parameters.Add("@NUM_PARCELAS", SqlDbType.Int).Value = 1; }
        if (prc02.Checked == true) { cmdInsCons.Parameters.Add("@NUM_PARCELAS", SqlDbType.Int).Value = 2; }
        if (prc03.Checked == true) { cmdInsCons.Parameters.Add("@NUM_PARCELAS", SqlDbType.Int).Value = 3; }
        if (prc04.Checked == true) { cmdInsCons.Parameters.Add("@NUM_PARCELAS", SqlDbType.Int).Value = 4; }
        if (prc05.Checked == true) { cmdInsCons.Parameters.Add("@NUM_PARCELAS", SqlDbType.Int).Value = 5; }
        if (prc06.Checked == true) { cmdInsCons.Parameters.Add("@NUM_PARCELAS", SqlDbType.Int).Value = 6; }
        if (prc07.Checked == true) { cmdInsCons.Parameters.Add("@NUM_PARCELAS", SqlDbType.Int).Value = 7; }
        if (prc08.Checked == true) { cmdInsCons.Parameters.Add("@NUM_PARCELAS", SqlDbType.Int).Value = 8; }
        if (prc09.Checked == true) { cmdInsCons.Parameters.Add("@NUM_PARCELAS", SqlDbType.Int).Value = 9; }
        if (prc10.Checked == true) { cmdInsCons.Parameters.Add("@NUM_PARCELAS", SqlDbType.Int).Value = 10; }
        if (prc11.Checked == true) { cmdInsCons.Parameters.Add("@NUM_PARCELAS", SqlDbType.Int).Value = 11; }
        if (prc12.Checked == true) { cmdInsCons.Parameters.Add("@NUM_PARCELAS", SqlDbType.Int).Value = 12; }

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


        return true;
    }
     */
}