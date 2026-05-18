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


public partial class checkout : System.Web.UI.Page
{
    public string sConfirma { get; set; }

    public string sid_id
    {
        get
        {
            try { return Funcoes.Decrypt(Request["id"]); }
            catch { return ""; }
        }
    }

    public int iOperacao { get; set; }


    protected void Page_Load(object sender, EventArgs e)
    {

        btnConfirmar.Attributes.Add("onclick", "document.body.style.cursor = 'wait'; this.value='Aguarde, Processando o pagamento...'; this.disabled = true; " + ClientScript.GetPostBackEventReference(btnConfirmar, string.Empty) + ";");

        SqlConnection mySelCadastro = new SqlConnection(Funcoes.conexao());
        mySelCadastro.Open();
        SqlCommand cmdSelCadastro = new SqlCommand("dbo.stp_vendas_ins", mySelCadastro);
        cmdSelCadastro.CommandType = CommandType.StoredProcedure;
        cmdSelCadastro.Parameters.Add("@flg_operacao", SqlDbType.Char).Value = "O";
        cmdSelCadastro.Parameters.Add("@COD_ID", SqlDbType.Int).Value = Funcoes.strToInt(sid_id.ToString());
        SqlDataReader ReaderCadastro = cmdSelCadastro.ExecuteReader();
        while (ReaderCadastro.Read())
        {
            HttpContext.Current.Session.Add("CODIGO", "0");
            HttpContext.Current.Session.Add("PESSOA", ReaderCadastro["COD_ID_PESSOAS_FJ_VENDEDOR"].ToString());
            HttpContext.Current.Session.Add("LICENCIADO", ReaderCadastro["COD_ID_PESSOA_LICENCIADO"].ToString());

            if (ReaderCadastro["NOM_ID_MARKETPLACE"].ToString().Trim() != "")
            { HttpContext.Current.Session.Add("IDMARKETPLACE", ReaderCadastro["NOM_ID_MARKETPLACE"].ToString()); }
            else { HttpContext.Current.Session.Add("IDMARKETPLACE", ConfigurationManager.AppSettings["idzoop"].ToString()); }

            if (ReaderCadastro["NOM_KEY_MARKETPLACE"].ToString().Trim() != "")
            { HttpContext.Current.Session.Add("USERNAMEMARKETPLACE", ReaderCadastro["NOM_KEY_MARKETPLACE"].ToString()); }
            else { HttpContext.Current.Session.Add("USERNAMEMARKETPLACE", ConfigurationManager.AppSettings["keyzoop"].ToString()); }



            imgLogoPrincipal.Src = "public_html/" + ReaderCadastro["NOM_LOGOTIPO_CHECKOUT"].ToString();
            imgLogoMensagemGeral.Src = "public_html/" + ReaderCadastro["NOM_LOGOTIPO_CHECKOUT"].ToString();

            Page.Title = "Check-out Pagamentos"; //ReaderCadastro["NOM_RAZAOSOCIAL"].ToString();
            if ((ReaderCadastro["NOM_COR_PRIMARIA_CHECKOUT"].ToString().Trim() != "") && (ReaderCadastro["NOM_COR_SECUNDARIA_CHECKOUT"].ToString().Trim() != ""))
            {
                dvCheckout.Attributes.Add("style", "background: linear-gradient(" + ReaderCadastro["NOM_COR_PRIMARIA_CHECKOUT"].ToString() + ", " + ReaderCadastro["NOM_COR_SECUNDARIA_CHECKOUT"].ToString() + ");");
                dvMensagemGeral.Attributes.Add("style", "background: linear-gradient(" + ReaderCadastro["NOM_COR_PRIMARIA_CHECKOUT"].ToString() + ", " + ReaderCadastro["NOM_COR_SECUNDARIA_CHECKOUT"].ToString() + ");");
            }

            Page.Header.Controls.Add(
            new LiteralControl(
            @"<style type='text/css'>.bg-whitelabel {background: " + ReaderCadastro["NOM_COR_PRIMARIA_CHECKOUT"].ToString() + "; color:#fff;}</style>")); 
        }

        if (!IsPostBack)
        {
            Funcoes.GravaAuditoria(Funcoes.strToInt(sid_id), Request.ServerVariables["REMOTE_ADDR"].ToString(), "Checkout", "Acesso");

            ConsultaGeral();

            cbPagamento.Visible = true;
            cbConfirmacao.Visible = false;
            btnConfirmar.Visible = true;
            btnVoltar.Visible = false;
        }


    }

    private void ConsultaGeral()
    {
        SqlConnection mySelCadastro = new SqlConnection(Funcoes.conexao());
        mySelCadastro.Open();
        SqlCommand cmdSelCadastro = new SqlCommand("dbo.stp_vendas_ins", mySelCadastro);
        cmdSelCadastro.CommandType = CommandType.StoredProcedure;
        cmdSelCadastro.Parameters.Add("@flg_operacao", SqlDbType.Char).Value = "O";
        cmdSelCadastro.Parameters.Add("@COD_ID", SqlDbType.Int).Value = Funcoes.strToInt(sid_id.ToString());
        SqlDataReader ReaderCadastro = cmdSelCadastro.ExecuteReader();

        while (ReaderCadastro.Read())
        {
            if ((ReaderCadastro["FLG_LINK_PERMANENTE"].ToString().Trim() == "N") && (Funcoes.strToInt(ReaderCadastro["NUM_VENDAS"].ToString().Trim()) > 0))
            {
                btnConfirmar.Visible = false;
                ClientScript.RegisterStartupScript(this.GetType(),
                    "LinkPermanente", "alert('Este link de venda já foi utilizado e não permite que seja utilizado novamente!'); window.opener=self; window.close(); self.close();", true);
                dvCheckout.Visible = false;
                dvPagamento.Visible = false;
                dvMensagemGeral.Visible = true;
                lblMensagemGeral.Text = "Este link de venda já foi utilizado e não permite que seja utilizado novamente!<br />Para esclarecer quaisquer dúvidas, contate o administrador do sistema.";
            }


            imgVenda.Src = ReaderCadastro["NOM_URL_IMAGEM"].ToString();

            if (ReaderCadastro["FLG_CLIENTE"].ToString().Trim() == "S")
            {
                txtNome.Text = ReaderCadastro["NOM_NOME"].ToString();
                txtSobrenome.Text = ReaderCadastro["NOM_SOBRENOME"].ToString();
                txtDocumento.Text = ReaderCadastro["NUM_CNPJCPF"].ToString();
                txtEmail.Text = ReaderCadastro["NOM_EMAIL"].ToString();
                txtCelular.Text = ReaderCadastro["NOM_CELULAR"].ToString();

                txtEndereco.Text = ReaderCadastro["NOM_ENDERECO"].ToString();
                txtNumero.Text = ReaderCadastro["NOM_NUMERO"].ToString();
                txtComplemento.Text = ReaderCadastro["NOM_COMPLEMENTO"].ToString();
                txtBairro.Text = ReaderCadastro["NOM_BAIRRO"].ToString();
                txtCidade.Text = ReaderCadastro["NOM_CIDADE"].ToString();
                ddlEstado.SelectedValue = ReaderCadastro["NOM_UF"].ToString();
                txtCEP.Text = ReaderCadastro["NOM_CEP"].ToString();
            }

            if (ReaderCadastro["NOM_CAMPO_01"].ToString().Trim() == "")
            {
                dvAdicionais.Visible = false;
            }
            else
            {
                dvAdicionais.Visible = true;
                lblAdicional.Text = ReaderCadastro["NOM_CAMPO_01"].ToString();
                //txtConteudo01.Text = ReaderCadastro["NOM_CONTEUDO_CAMPO_01"].ToString();
            }
            
            if (ReaderCadastro["FLG_PRECO_PARCELAMENTO"].ToString() == "S")
            {
                //lblValor.Text = "1";
                lblValor.Text = String.Format("{0:n2}", Funcoes.strToDouble(ReaderCadastro["NUM_VALOR_01"].ToString()));
            }
            else
            {
                //lblValor.Text = 2;
                lblValor.Text = String.Format("{0:n2}", Funcoes.strToDouble(ReaderCadastro["NUM_VALOR"].ToString()));
            }

            if (ReaderCadastro["DTA_VENCIMENTO"].ToString().Trim() != "")
            {
                lblVencimento.Text = Convert.ToDateTime(ReaderCadastro["DTA_VENCIMENTO"].ToString()).ToShortDateString();
            }

            lblReferencia.Text = ReaderCadastro["NOM_REFERENCIA_PRODUTO"].ToString();
            lblDescricao.Text = ReaderCadastro["NOM_DESCRICAO_PRODUTO"].ToString();

            dvCartaoCredito.Visible = false;
            h4CartaoCredito.Visible = false;


            if (ReaderCadastro["FLG_BOLETO"].ToString() == "S") { lblFPBoleto.Visible = true; } else { lblFPBoleto.Visible = false; }
            
            if (ReaderCadastro["FLG_CARTAO_CREDITO"].ToString() == "S"){lblFPCredito.Visible = true; } else {lblFPCredito.Visible = false; }

            if (ReaderCadastro["FLG_PIX"].ToString() == "S") { lblFPPix.Visible = true; } else { lblFPPix.Visible = false; }


            if (ReaderCadastro["FLG_PRODUTO"].ToString() == "S")
            {
                divProdutos.Visible = true;
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



            for (int i = 30; i >= 0; i--)
            {
                ddlAnoCartao.Items.Insert(0, new ListItem((DateTime.Now.Year + i).ToString(), (DateTime.Now.Year + i).ToString()));
            }

            string TextoParcelas = "";
            int NumeroParcelas = Funcoes.strToInt(ReaderCadastro["NUM_PARCELAS"].ToString());
            if (NumeroParcelas <= 0) { NumeroParcelas = 21; }
            for (int p = NumeroParcelas; p >= 1; p--)
            {
                if (ReaderCadastro["FLG_PRECO_PARCELAMENTO"].ToString() == "S")
                {
                    if (p == 1) { TextoParcelas = "" + p.ToString() + "X de " + String.Format("{0:c2}", (Funcoes.strToDouble(ReaderCadastro["NUM_VALOR_01"].ToString()) + 0.00) / (p + 0.00)); }
                    if (p == 2) { TextoParcelas = "" + p.ToString() + "X de " + String.Format("{0:c2}", (Funcoes.strToDouble(ReaderCadastro["NUM_VALOR_02"].ToString()) + 0.00) / (p + 0.00)); }
                    if (p == 3) { TextoParcelas = "" + p.ToString() + "X de " + String.Format("{0:c2}", (Funcoes.strToDouble(ReaderCadastro["NUM_VALOR_03"].ToString()) + 0.00) / (p + 0.00)); }
                    if (p == 4) { TextoParcelas = "" + p.ToString() + "X de " + String.Format("{0:c2}", (Funcoes.strToDouble(ReaderCadastro["NUM_VALOR_04"].ToString()) + 0.00) / (p + 0.00)); }
                    if (p == 5) { TextoParcelas = "" + p.ToString() + "X de " + String.Format("{0:c2}", (Funcoes.strToDouble(ReaderCadastro["NUM_VALOR_05"].ToString()) + 0.00) / (p + 0.00)); }
                    if (p == 6) { TextoParcelas = "" + p.ToString() + "X de " + String.Format("{0:c2}", (Funcoes.strToDouble(ReaderCadastro["NUM_VALOR_06"].ToString()) + 0.00) / (p + 0.00)); }
                    if (p == 7) { TextoParcelas = "" + p.ToString() + "X de " + String.Format("{0:c2}", (Funcoes.strToDouble(ReaderCadastro["NUM_VALOR_07"].ToString()) + 0.00) / (p + 0.00)); }
                    if (p == 8) { TextoParcelas = "" + p.ToString() + "X de " + String.Format("{0:c2}", (Funcoes.strToDouble(ReaderCadastro["NUM_VALOR_08"].ToString()) + 0.00) / (p + 0.00)); }
                    if (p == 9) { TextoParcelas = "" + p.ToString() + "X de " + String.Format("{0:c2}", (Funcoes.strToDouble(ReaderCadastro["NUM_VALOR_09"].ToString()) + 0.00) / (p + 0.00)); }
                    if (p == 10) { TextoParcelas = "" + p.ToString() + "X de " + String.Format("{0:c2}", (Funcoes.strToDouble(ReaderCadastro["NUM_VALOR_10"].ToString()) + 0.00) / (p + 0.00)); }
                    if (p == 11) { TextoParcelas = "" + p.ToString() + "X de " + String.Format("{0:c2}", (Funcoes.strToDouble(ReaderCadastro["NUM_VALOR_11"].ToString()) + 0.00) / (p + 0.00)); }
                    if (p == 12) { TextoParcelas = "" + p.ToString() + "X de " + String.Format("{0:c2}", (Funcoes.strToDouble(ReaderCadastro["NUM_VALOR_12"].ToString()) + 0.00) / (p + 0.00)); }
                    if (p == 13) { TextoParcelas = "" + p.ToString() + "X de " + String.Format("{0:c2}", (Funcoes.strToDouble(ReaderCadastro["NUM_VALOR_13"].ToString()) + 0.00) / (p + 0.00)); }
                    if (p == 14) { TextoParcelas = "" + p.ToString() + "X de " + String.Format("{0:c2}", (Funcoes.strToDouble(ReaderCadastro["NUM_VALOR_14"].ToString()) + 0.00) / (p + 0.00)); }
                    if (p == 15) { TextoParcelas = "" + p.ToString() + "X de " + String.Format("{0:c2}", (Funcoes.strToDouble(ReaderCadastro["NUM_VALOR_15"].ToString()) + 0.00) / (p + 0.00)); }
                    if (p == 16) { TextoParcelas = "" + p.ToString() + "X de " + String.Format("{0:c2}", (Funcoes.strToDouble(ReaderCadastro["NUM_VALOR_16"].ToString()) + 0.00) / (p + 0.00)); }
                    if (p == 17) { TextoParcelas = "" + p.ToString() + "X de " + String.Format("{0:c2}", (Funcoes.strToDouble(ReaderCadastro["NUM_VALOR_17"].ToString()) + 0.00) / (p + 0.00)); }
                    if (p == 18) { TextoParcelas = "" + p.ToString() + "X de " + String.Format("{0:c2}", (Funcoes.strToDouble(ReaderCadastro["NUM_VALOR_18"].ToString()) + 0.00) / (p + 0.00)); }
                    if (p == 19) { TextoParcelas = "" + p.ToString() + "X de " + String.Format("{0:c2}", (Funcoes.strToDouble(ReaderCadastro["NUM_VALOR_19"].ToString()) + 0.00) / (p + 0.00)); }
                    if (p == 20) { TextoParcelas = "" + p.ToString() + "X de " + String.Format("{0:c2}", (Funcoes.strToDouble(ReaderCadastro["NUM_VALOR_20"].ToString()) + 0.00) / (p + 0.00)); }
                    if (p == 21) { TextoParcelas = "" + p.ToString() + "X de " + String.Format("{0:c2}", (Funcoes.strToDouble(ReaderCadastro["NUM_VALOR_21"].ToString()) + 0.00) / (p + 0.00)); }
                }
                else
                {
                    TextoParcelas = "" + p.ToString() + "X de " + String.Format("{0:c2}", (Funcoes.strToDouble(lblValor.Text.ToString()) + 0.00) / (p + 0.00));
                }
                ddlParcelas.Items.Insert(0, new ListItem(TextoParcelas.ToString(), p.ToString()));
            }
            ddlParcelas.SelectedValue = ReaderCadastro["NUM_PARCELAS"].ToString();



        }


    }

    private void ConsultaUtilizacao()
    {
        SqlConnection mySelCadastro = new SqlConnection(Funcoes.conexao());
        mySelCadastro.Open();
        SqlCommand cmdSelCadastro = new SqlCommand("dbo.stp_vendas_ins", mySelCadastro);
        cmdSelCadastro.CommandType = CommandType.StoredProcedure;
        cmdSelCadastro.Parameters.Add("@flg_operacao", SqlDbType.Char).Value = "O";
        cmdSelCadastro.Parameters.Add("@COD_ID", SqlDbType.Int).Value = Funcoes.strToInt(sid_id.ToString());
        SqlDataReader ReaderCadastro = cmdSelCadastro.ExecuteReader();

        while (ReaderCadastro.Read())
        {
            if ((ReaderCadastro["FLG_LINK_PERMANENTE"].ToString().Trim() == "N") && (Funcoes.strToInt(ReaderCadastro["NUM_VENDAS"].ToString().Trim()) > 0))
            {
                btnConfirmar.Visible = false;
                ClientScript.RegisterStartupScript(this.GetType(),
                    "LinkPermanente", "alert('Este link de venda já foi utilizado e não permite que seja utilizado novamente!'); self.close();", true);
                dvCheckout.Visible = false;
                dvPagamento.Visible = false;
                dvMensagemGeral.Visible = true;
                lblMensagemGeral.Text = "Este link de venda já foi utilizado e não permite que seja utilizado novamente!<br />Para esclarecer quaisquer dúvidas, contate o administrador do sistema.";
            }
        }
    }

    protected void txtCEP_TextChanged(object sender, EventArgs e)
    {
        if ((txtCEP.Text.ToString().Trim() != "") || (txtCEP.Text.ToString().Trim().Length >= 8))
        {
            try
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
            catch
            {
                ClientScript.RegisterStartupScript(this.GetType(),
                    "ErroBuscaCEP", "alert('O CEP digitado não é válido! Verifique e reentre.'); ", true);

            }
        }
        else
        {
            ClientScript.RegisterStartupScript(this.GetType(),
                "CEPInvalido", "alert('O CEP digitado não é válido! Verifique e reentre.'); ", true);

        }

    }
    protected void btnConfirmar_Click(object sender, EventArgs e)
    {
        string sOperadora = "";
        SqlConnection mySelCadastro = new SqlConnection(Funcoes.conexao());
        mySelCadastro.Open();
        SqlCommand cmdSelCadastro = new SqlCommand("dbo.stp_vendas_ins", mySelCadastro);
        cmdSelCadastro.CommandType = CommandType.StoredProcedure;
        cmdSelCadastro.Parameters.Add("@flg_operacao", SqlDbType.Char).Value = "O";
        cmdSelCadastro.Parameters.Add("@COD_ID", SqlDbType.Int).Value = Funcoes.strToInt(sid_id.ToString());
        SqlDataReader ReaderCadastro = cmdSelCadastro.ExecuteReader();

        while (ReaderCadastro.Read())
        {
            sOperadora = ReaderCadastro["FLG_OPERADORA"].ToString().Trim();
            if ((ReaderCadastro["FLG_LINK_PERMANENTE"].ToString().Trim() == "N") && (Funcoes.strToInt(ReaderCadastro["NUM_VENDAS"].ToString().Trim()) > 0))
            {
                btnConfirmar.Visible = false;
                ClientScript.RegisterStartupScript(this.GetType(),
                    "LinkPermanente", "alert('Este link de venda já foi utilizado e não permite que seja utilizado novamente!'); self.close();", true);
                dvCheckout.Visible = false;
                dvPagamento.Visible = false;
                dvMensagemGeral.Visible = true;
                lblMensagemGeral.Text = "Este link de venda já foi utilizado e não permite que seja utilizado novamente!<br />Para esclarecer quaisquer dúvidas, contate o administrador do sistema.";
                return;
            }
        }


        Funcoes.GravaAuditoria(Funcoes.strToInt(sid_id), Request.ServerVariables["REMOTE_ADDR"].ToString(), "Checkout", "Confirmar Pagamento");

        if ((FPBoleto.Checked == true) || (FPCredito.Checked == true) || (FPPix.Checked == true))
        {
            double vValorTotal = VerificaValorTotal();
            // ZOOP

            
            if (sOperadora == "Z")
            {
                if (FPCredito.Checked == true)
                {
                    GerarTransacaoCredito(vValorTotal);
                }
                if (FPPix.Checked == true)
                {
                    GerarTransacaoPix(vValorTotal);
                }
                if (FPBoleto.Checked == true)
                {
                    GerarTransacaoBoleto(vValorTotal);
                }
            }

            // ZOOP
            if (sOperadora == "A")
            {
                if (FPCredito.Checked == true)
                {
                    GerarTransacaoCreditoAsaas(vValorTotal);
                }
                if (FPPix.Checked == true)
                {
                    GerarTransacaoPixAsaas(vValorTotal);
                }
                if (FPBoleto.Checked == true)
                {
                    GerarTransacaoBoletoAsaas(vValorTotal);
                }
            }
        
        }
        else
        {
            ClientScript.RegisterStartupScript(this.GetType(),
    "ErroFormaPagto", "alert('Não foi selecionada nenhuma forma de pagamento! Por favor, verifique e reentre.');  ", true);

        }
    }
    protected void btnVoltar_Click(object sender, EventArgs e)
    {
        Funcoes.GravaAuditoria(Funcoes.strToInt(sid_id), Request.ServerVariables["REMOTE_ADDR"].ToString(), "Checkout", "Voltar a tela de Pagamento");

        cbPagamento.Visible = true;
        cbConfirmacao.Visible = false;
        cbPix.Visible = false;
        cbBoleto.Visible = false;

        btnVoltar.Visible = false;
        btnConfirmar.Visible = true;

    }
    protected void FPBoleto_CheckedChanged(object sender, EventArgs e)
    {
        HabilitaFormaPagamento();
    }
    protected void FPCredito_CheckedChanged(object sender, EventArgs e)
    {
        HabilitaFormaPagamento();
    }
    protected void FPPix_CheckedChanged(object sender, EventArgs e)
    {
        HabilitaFormaPagamento();
    }

    private void HabilitaFormaPagamento()
    {
        if (FPCredito.Checked == true)
        {
            dvCartaoCredito.Visible = true;
            h4CartaoCredito.Visible = true;
        }
        else
        {
            dvCartaoCredito.Visible = false;
            h4CartaoCredito.Visible = false;
        }

    }
    protected void ckbEnderecoEntrega_CheckedChanged(object sender, EventArgs e)
    {
        if (ckbEnderecoEntrega.Checked == true)
        {
            dvEntrega.Visible = true;
        }
        else
        {
            dvEntrega.Visible = false;
        }


    }
    protected void txtCEPEntrega_TextChanged(object sender, EventArgs e)
    {
        if ((txtCEPEntrega.Text.ToString().Trim() != "") || (txtCEPEntrega.Text.ToString().Trim().Length >= 8))
        {
            try
            {
                ServicePointManager.Expect100Continue = true;
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12
                | SecurityProtocolType.Tls11
                | SecurityProtocolType.Tls
                | SecurityProtocolType.Ssl3;

                // Convert to Base64

                var myUri = new Uri("https://viacep.com.br/ws/" + txtCEPEntrega.Text.ToString() + "/json");
                var myWebRequest = WebRequest.Create(myUri);
                var myHttpWebRequest = (HttpWebRequest)myWebRequest;
                myHttpWebRequest.Accept = "application/json";

                var myWebResponse = myWebRequest.GetResponse();
                var responseStream = myWebResponse.GetResponseStream();

                //if (responseStream == null) return null;

                StreamReader myStreamReader = new StreamReader(responseStream, Encoding.UTF8);
                var json = myStreamReader.ReadToEnd();

                dadosCEP.CEPInfo m = JsonSerializer.Deserialize<dadosCEP.CEPInfo>(json);
                txtEnderecoEntrega.Text = m.logradouro;
                txtBairroEntrega.Text = m.bairro;
                txtCidadeEntrega.Text = m.localidade;
                ddlEstadoEntrega.SelectedValue = m.uf;

                responseStream.Close();
                myWebResponse.Close();
            }
            catch
            {
                ClientScript.RegisterStartupScript(this.GetType(),
                    "ErroBuscaCEP", "alert('O CEP de Entrega digitado não é válido! Verifique e reentre.');  ", true);

            }
        }
        else
        {
            ClientScript.RegisterStartupScript(this.GetType(),
                "CEPInvalido", "alert('O CEP de Entrega digitado não é válido! Verifique e reentre.');  ", true);

        }

    }


    private void GerarTransacaoCreditoAsaas(double vValorTotal)
    {

        if (dvCartaoCredito.Visible == true)
        {
            // Carrega os dados do Link de Pagamento

            SqlConnection mySelCadastro = new SqlConnection(Funcoes.conexao());
            mySelCadastro.Open();
            SqlCommand cmdSelCadastro = new SqlCommand("dbo.stp_vendas_ins", mySelCadastro);
            cmdSelCadastro.CommandType = CommandType.StoredProcedure;
            cmdSelCadastro.Parameters.Add("@flg_operacao", SqlDbType.Char).Value = "O";
            cmdSelCadastro.Parameters.Add("@COD_ID", SqlDbType.Int).Value = Funcoes.strToInt(sid_id.ToString());
            SqlDataReader ReaderCadastro = cmdSelCadastro.ExecuteReader();

            while (ReaderCadastro.Read())
            {

                string sIDCliente = asaas.VerificaCliente("", "", txtNome.Text.ToString(), txtDocumento.Text.ToString(), txtEmail.Text.ToString(), txtCEP.Text.ToString(), txtNumero.Text.ToString(), txtCelular.Text.ToString());
                if (sIDCliente.ToString().Trim() != "")
                {
                    GravaCliente(sIDCliente.ToString());


                    DadosCobrancaCartaoAsaaS.Root dcobranca = new DadosCobrancaCartaoAsaaS.Root()
                    {
                        customer = sIDCliente.ToString(),
                        billingType = "CREDIT_CARD",
                        value = Funcoes.strToDouble(ReaderCadastro["NUM_VALOR"].ToString()),
                        dueDate = Convert.ToDateTime(ReaderCadastro["DTA_VENCIMENTO"].ToString()).Year.ToString() + "-" + Convert.ToDateTime(ReaderCadastro["DTA_VENCIMENTO"].ToString()).Month.ToString() + "-" + Convert.ToDateTime(ReaderCadastro["DTA_VENCIMENTO"].ToString()).Day.ToString(),
                        creditCard = new DadosCobrancaCartaoAsaaS.CreditCard()
                        {
                            holderName = txtNomeCartao.Text.ToString(),
                            number = txtNumeroCartao.Text.ToString(),
                            ccv = txtCVVCartao.Text.ToString(),
                            expiryYear = ddlAnoCartao.SelectedValue.ToString().PadLeft(4, '0'),
                            expiryMonth = ddlMesCartao.SelectedValue.ToString().PadLeft(1, '0')
                        },
                        creditCardHolderInfo = new DadosCobrancaCartaoAsaaS.CreditCardHolderInfo()
                        {
                            cpfCnpj = txtDocumento.Text.ToString(),
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
                    cmdSelCadastroSplit.Parameters.Add("@COD_ID_PESSOAS_FJ", SqlDbType.Int).Value = Funcoes.strToInt(ReaderCadastro["COD_ID_PESSOAS_FJ_VENDEDOR"].ToString());
                    cmdSelCadastroSplit.Parameters.Add("@COD_ID_PESSOA_LICENCIADO", SqlDbType.Int).Value = Funcoes.strToInt(ReaderCadastro["COD_ID_PESSOA_LICENCIADO"].ToString());
                    SqlDataReader ReaderCadastroSplit = cmdSelCadastroSplit.ExecuteReader();

                    List<DadosCobrancaCartaoAsaaS.split> listaSplits = new List<DadosCobrancaCartaoAsaaS.split>();
                    int iContadorSplit = 0;
                    while (ReaderCadastroSplit.Read()) // Percorre os registros do banco
                    {
                        iContadorSplit = iContadorSplit + 1;
                        listaSplits.Add(
                        new DadosCobrancaCartaoAsaaS.split()
                        {
                            walletId = ReaderCadastroSplit["NUM_WALLETID_BAAS"].ToString(),
                            percentualValue = Funcoes.strToDouble(ReaderCadastroSplit["NUM_PERCENTUAL"].ToString()),
                            fixedValue = Funcoes.strToDouble(ReaderCadastroSplit["NUM_VALOR"].ToString()),
                            description = "Split para " + ReaderCadastroSplit["NOM_RAZAOSOCIAL"].ToString(),
                            externalReference = HttpContext.Current.Session["LICENCIADO"].ToString() + "_" + HttpContext.Current.Session["PESSOA"].ToString(),
                        });
                    }

                    // Converte a lista para array antes de atribuir ao objeto
                    if (iContadorSplit > 0) { dcobranca.split = listaSplits.ToArray(); }
                    
                    

                    string jsonCobranca = JsonConvert.SerializeObject(dcobranca);
                    string jsonURLCobranca = asaas.CriarCobranca(asaas.PegarTokenSubconta(Funcoes.strToInt(HttpContext.Current.Session["PESSOA"].ToString()), Funcoes.strToInt(HttpContext.Current.Session["LICENCIADO"].ToString())), jsonCobranca);

                    JObject oRetorno = JObject.Parse(jsonURLCobranca);

                    try
                    {
                        if ((oRetorno["status"].ToString().Trim() != "") && (oRetorno["id"].ToString().Trim() != ""))
                        {
                            string sStatusTransacao = VerificaStatus(oRetorno["status"].ToString().Trim());
                            string sSalva = RetornaPosicaoStatus(oRetorno["status"].ToString().Trim());

                            if (sSalva.ToString().Trim() == "S")
                            {
                                SqlConnection connInsCons = new SqlConnection(Funcoes.conexao());
                                connInsCons.Open();
                                SqlCommand cmdInsCons = new SqlCommand("dbo.stp_vendas_vendas_ins", connInsCons);
                                cmdInsCons.CommandType = CommandType.StoredProcedure;
                                cmdInsCons.Parameters.Add("@flg_operacao", SqlDbType.Char).Value = 'I';
                                cmdInsCons.Parameters.Add("@COD_ID", SqlDbType.Int).Value = Funcoes.strToInt(sid_id.ToString());
                                cmdInsCons.Parameters.Add("@COD_ID_PESSOA_LICENCIADO", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["LICENCIADO"].ToString());
                                cmdInsCons.Parameters.Add("@COD_ID_PESSOAS_FJ_VENDEDOR", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["PESSOA"].ToString());
                                // Buscar dados comprador

                                cmdInsCons.Parameters.Add("@COD_ID_VENDAS", SqlDbType.Int).Value = Funcoes.strToInt(sid_id.ToString());

                                cmdInsCons.Parameters.Add("@NUM_VALOR", SqlDbType.Float).Value = vValorTotal;
                                if (ReaderCadastro["DTA_VENCIMENTO"].ToString().Trim() != "") { cmdInsCons.Parameters.Add("@DTA_VENCIMENTO", SqlDbType.DateTime).Value = Convert.ToDateTime(ReaderCadastro["DTA_VENCIMENTO"].ToString()); }

                                cmdInsCons.Parameters.Add("@NUM_CARTAO", SqlDbType.VarChar).Value = txtNumeroCartao.Text.ToString();
                                cmdInsCons.Parameters.Add("@NOM_CARTAO", SqlDbType.VarChar).Value = txtNomeCartao.Text.ToString();
                                cmdInsCons.Parameters.Add("@NUM_MES_CARTAO", SqlDbType.Int).Value = Funcoes.strToInt(ddlMesCartao.SelectedValue.ToString());
                                cmdInsCons.Parameters.Add("@NUM_ANO_CARTAO", SqlDbType.Int).Value = Funcoes.strToInt(ddlAnoCartao.SelectedValue.ToString());
                                cmdInsCons.Parameters.Add("@NUM_CODIGO_CARTAO", SqlDbType.Int).Value = Funcoes.strToInt(txtCVVCartao.Text.ToString());
                                cmdInsCons.Parameters.Add("@NUM_PARCELAS", SqlDbType.Int).Value = Funcoes.strToInt(ddlParcelas.SelectedValue.ToString());

                                cmdInsCons.Parameters.Add("@FLG_CARTAO_CREDITO", SqlDbType.VarChar).Value = "S";


                                cmdInsCons.Parameters.Add("@NOM_CODE", SqlDbType.VarChar).Value = oRetorno["id"].ToString().Trim();
                                cmdInsCons.Parameters.Add("@NOM_CONTEUDO_CAMPO_01", SqlDbType.VarChar).Value = txtConteudo01.Text.ToString();

                                // Cliente
                                cmdInsCons.Parameters.Add("@NOM_NOME", SqlDbType.VarChar).Value = txtNome.Text.ToString();
                                cmdInsCons.Parameters.Add("@NOM_SOBRENOME", SqlDbType.VarChar).Value = txtSobrenome.Text.ToString();
                                cmdInsCons.Parameters.Add("@NUM_CNPJCPF", SqlDbType.VarChar).Value = txtDocumento.Text.ToString();
                                cmdInsCons.Parameters.Add("@NOM_EMAIL", SqlDbType.VarChar).Value = txtEmail.Text.ToString();
                                cmdInsCons.Parameters.Add("@NOM_CELULAR", SqlDbType.VarChar).Value = txtCelular.Text.ToString();

                                // Endereço
                                cmdInsCons.Parameters.Add("@NOM_ENDERECO", SqlDbType.VarChar).Value = txtEndereco.Text.ToString();
                                cmdInsCons.Parameters.Add("@NOM_NUMERO", SqlDbType.VarChar).Value = txtNumero.Text.ToString();
                                cmdInsCons.Parameters.Add("@NOM_COMPLEMENTO", SqlDbType.VarChar).Value = txtComplemento.Text.ToString();
                                cmdInsCons.Parameters.Add("@NOM_BAIRRO", SqlDbType.VarChar).Value = txtBairro.Text.ToString();
                                cmdInsCons.Parameters.Add("@NOM_CIDADE", SqlDbType.VarChar).Value = txtCidade.Text.ToString();
                                cmdInsCons.Parameters.Add("@NOM_UF", SqlDbType.VarChar).Value = ddlEstado.SelectedValue.ToString();
                                cmdInsCons.Parameters.Add("@NOM_CEP", SqlDbType.VarChar).Value = txtCEP.Text.ToString();
                                cmdInsCons.Parameters.Add("@NOM_PAIS", SqlDbType.VarChar).Value = "BR";

                                // Endereço Entrega
                                cmdInsCons.Parameters.Add("@FLG_ENDERECO_ENTREGA", SqlDbType.Char).Value = (ckbEnderecoEntrega.Checked == true) ? "S" : "N";

                                cmdInsCons.Parameters.Add("@NOM_ENDERECO_ENTREGA", SqlDbType.VarChar).Value = txtEnderecoEntrega.Text.ToString();
                                cmdInsCons.Parameters.Add("@NOM_NUMERO_ENTREGA", SqlDbType.VarChar).Value = txtNumeroEntrega.Text.ToString();
                                cmdInsCons.Parameters.Add("@NOM_COMPLEMENTO_ENTREGA", SqlDbType.VarChar).Value = txtComplementoEntrega.Text.ToString();
                                cmdInsCons.Parameters.Add("@NOM_BAIRRO_ENTREGA", SqlDbType.VarChar).Value = txtBairroEntrega.Text.ToString();
                                cmdInsCons.Parameters.Add("@NOM_CIDADE_ENTREGA", SqlDbType.VarChar).Value = txtCidadeEntrega.Text.ToString();
                                cmdInsCons.Parameters.Add("@NOM_UF_ENTREGA", SqlDbType.VarChar).Value = ddlEstadoEntrega.SelectedValue.ToString();
                                cmdInsCons.Parameters.Add("@NOM_CEP_ENTREGA", SqlDbType.VarChar).Value = txtCEPEntrega.Text.ToString();

                                cmdInsCons.Parameters.Add("@DES_JSON", SqlDbType.Text).Value = jsonURLCobranca.ToString();

                                cmdInsCons.ExecuteNonQuery();
                                connInsCons.Close();
                                connInsCons.Dispose();
                            }
                            txtStatus.Text = "Transação por Cartão de Crédito retornou status: (" + sStatusTransacao.ToString() + ")!";
                            txtTransacao.Text = oRetorno["id"].ToString();
                            Funcoes.GravaAuditoria(Funcoes.strToInt(sid_id), Request.ServerVariables["REMOTE_ADDR"].ToString(), "Checkout", "Pagamento Cartão - Transação Criada: " + sStatusTransacao.ToString());
                        }
                    }
                    catch
                    {
                        txtStatus.Text = zoop.status_code(oRetorno["error"]["status_code"].ToString());
                        txtTransacao.Text = "Falha ao tentar realizar o pagamento";
                        Funcoes.GravaAuditoria(Funcoes.strToInt(sid_id), Request.ServerVariables["REMOTE_ADDR"].ToString(), "Checkout", "Pagamento Cartão - Erro Transação: " + txtStatus.Text.ToString());
                    }
                }

                cbPagamento.Visible = false;
                cbConfirmacao.Visible = true;

                btnConfirmar.Visible = false;
                btnVoltar.Visible = true;
            }
        }

    }
    
    
    private void GerarTransacaoPixAsaas(double vValorTotal)
    {
        //ClientScript.RegisterStartupScript(this.GetType(),
        //    "BoletoAsaas", "alert('Venda boleto ASAAS');  ", true);

        string retornoInclusao = "";
        string sTokenComprador = "";

        //lblBotaoLink.Text = "CLIQUE PARA ACESSAR O QRCODE PIX";
        //lblLink.Text = "Link para QRCode do Pagamento via PIX";
        
        SqlConnection mySelCadastro = new SqlConnection(Funcoes.conexao());
        mySelCadastro.Open();
        SqlCommand cmdSelCadastro = new SqlCommand("dbo.stp_vendas_ins", mySelCadastro);
        cmdSelCadastro.CommandType = CommandType.StoredProcedure;
        cmdSelCadastro.Parameters.Add("@flg_operacao", SqlDbType.Char).Value = "O";
        cmdSelCadastro.Parameters.Add("@COD_ID", SqlDbType.Int).Value = Funcoes.strToInt(sid_id.ToString());
        SqlDataReader ReaderCadastro = cmdSelCadastro.ExecuteReader();

        while (ReaderCadastro.Read())
        {
            
            string sIDCliente = asaas.VerificaCliente("", "", txtNome.Text.ToString(), txtDocumento.Text.ToString(), txtEmail.Text.ToString(), txtCEP.Text.ToString(),txtNumero.Text.ToString(), txtCelular.Text.ToString());
            if (sIDCliente.ToString().Trim() != "")
            {
                GravaCliente(sIDCliente.ToString());
                DadosCobrancaAsaaS.Root dcobranca = new DadosCobrancaAsaaS.Root()
                {
                    customer = sIDCliente.ToString(),
                    billingType = "PIX",
                    value = Funcoes.strToDouble(ReaderCadastro["NUM_VALOR"].ToString()),
                    dueDate = Convert.ToDateTime(ReaderCadastro["DTA_VENCIMENTO"].ToString()).Year.ToString() + "-" + Convert.ToDateTime(ReaderCadastro["DTA_VENCIMENTO"].ToString()).Month.ToString() + "-" + Convert.ToDateTime(ReaderCadastro["DTA_VENCIMENTO"].ToString()).Day.ToString(),
                    daysAfterDueDateToRegistrationCancellation = Funcoes.strToInt("2"),
                    description = ReaderCadastro["NOM_DESCRICAO_PRODUTO"].ToString(),
                    externalReference = ReaderCadastro["NOM_REFERENCIA_PRODUTO"].ToString()
                };

                SqlConnection mySelCadastroSplit = new SqlConnection(Funcoes.conexao());
                mySelCadastroSplit.Open();
                SqlCommand cmdSelCadastroSplit = new SqlCommand("dbo.stp_pessoas_fj_split_ins", mySelCadastroSplit);
                cmdSelCadastroSplit.CommandType = CommandType.StoredProcedure;
                cmdSelCadastroSplit.Parameters.Add("@flg_operacao", SqlDbType.Char).Value = "C";
                cmdSelCadastroSplit.Parameters.Add("@COD_ID_PESSOAS_FJ", SqlDbType.Int).Value = Funcoes.strToInt(ReaderCadastro["COD_ID_PESSOAS_FJ_VENDEDOR"].ToString());
                cmdSelCadastroSplit.Parameters.Add("@COD_ID_PESSOA_LICENCIADO", SqlDbType.Int).Value = Funcoes.strToInt(ReaderCadastro["COD_ID_PESSOA_LICENCIADO"].ToString());
                SqlDataReader ReaderCadastroSplit = cmdSelCadastroSplit.ExecuteReader();

                List<DadosCobrancaAsaaS.split> listaSplits = new List<DadosCobrancaAsaaS.split>();
                int iContadorSplit = 0;
                while (ReaderCadastroSplit.Read()) // Percorre os registros do banco
                {
                    iContadorSplit = iContadorSplit + 1;
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
                if (iContadorSplit > 0) { dcobranca.split = listaSplits.ToArray(); }


                string jsonCobranca = JsonConvert.SerializeObject(dcobranca);
                string jsonURLCobranca = asaas.CriarCobranca(asaas.PegarTokenSubconta(Funcoes.strToInt(HttpContext.Current.Session["PESSOA"].ToString()), Funcoes.strToInt(HttpContext.Current.Session["LICENCIADO"].ToString())), jsonCobranca);

                if (jsonURLCobranca.ToString().Trim() != "")
                {
                    JObject oURLCobranca = JObject.Parse(jsonURLCobranca.ToString());
                    try
                    {
                        string jsonQRCodeCobranca = asaas.ObterQRCodeCobranca(asaas.PegarTokenSubconta(Funcoes.strToInt(HttpContext.Current.Session["PESSOA"].ToString()), Funcoes.strToInt(HttpContext.Current.Session["LICENCIADO"].ToString())), oURLCobranca["id"].ToString());

                        if (jsonQRCodeCobranca.ToString().Trim() != "")
                        {
                                //divQRCode.Visible = true;
                                JObject oQRCodeCobranca = JObject.Parse(jsonQRCodeCobranca.ToString());
                                //lblPixCopiaCola.Text = oQRCodeCobranca["payload"].ToString();
                                string base64String = oQRCodeCobranca["encodedImage"].ToString(); //encodedImage
                                QRCodeEncoder encoder = new QRCodeEncoder();
                                Bitmap image = encoder.Encode(oQRCodeCobranca["payload"].ToString());
                                image.Save(Server.MapPath("public_html") + "\\" + oURLCobranca["id"].ToString() + ".bmp");
                                imgQRcode.Src = "../public_html/" + oURLCobranca["id"].ToString() + ".bmp";
                        }
                    }
                    catch
                    {
                        ClientScript.RegisterStartupScript(this.GetType(), "Mensagem", "alert('Não foi possível gerar o QRCode para pagamento via pix! " + oURLCobranca["errors"][0]["description"] + "');", true);
                        return;
                    }


                    SqlConnection connInsCons = new SqlConnection(Funcoes.conexao());
                    connInsCons.Open();
                    SqlCommand cmdInsCons = new SqlCommand("dbo.stp_vendas_vendas_ins", connInsCons);
                    cmdInsCons.CommandType = CommandType.StoredProcedure;
                    cmdInsCons.Parameters.Add("@flg_operacao", SqlDbType.Char).Value = 'I';
                    cmdInsCons.Parameters.Add("@COD_ID", SqlDbType.Int).Value = Funcoes.strToInt(sid_id.ToString());

                    cmdInsCons.Parameters.Add("@COD_ID_PESSOA_LICENCIADO", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["LICENCIADO"].ToString());
                    cmdInsCons.Parameters.Add("@COD_ID_PESSOAS_FJ_VENDEDOR", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["PESSOA"].ToString());
                    // Buscar dados comprador

                    cmdInsCons.Parameters.Add("@COD_ID_VENDAS", SqlDbType.Int).Value = Funcoes.strToInt(sid_id.ToString());

                    cmdInsCons.Parameters.Add("@NUM_VALOR", SqlDbType.Float).Value = vValorTotal;
                    if (ReaderCadastro["DTA_VENCIMENTO"].ToString().Trim() != "") { cmdInsCons.Parameters.Add("@DTA_VENCIMENTO", SqlDbType.DateTime).Value = Convert.ToDateTime(ReaderCadastro["DTA_VENCIMENTO"].ToString()); }

                    cmdInsCons.Parameters.Add("@FLG_BOLETO", SqlDbType.VarChar).Value = "S";

                    cmdInsCons.Parameters.Add("@NOM_CODE", SqlDbType.VarChar).Value = oURLCobranca["id"].ToString().Trim();
                    cmdInsCons.Parameters.Add("@NOM_CONTEUDO_CAMPO_01", SqlDbType.VarChar).Value = txtConteudo01.Text.ToString();

                    // Cliente
                    cmdInsCons.Parameters.Add("@NOM_NOME", SqlDbType.VarChar).Value = txtNome.Text.ToString();
                    cmdInsCons.Parameters.Add("@NOM_SOBRENOME", SqlDbType.VarChar).Value = txtSobrenome.Text.ToString();
                    cmdInsCons.Parameters.Add("@NUM_CNPJCPF", SqlDbType.VarChar).Value = txtDocumento.Text.ToString();
                    cmdInsCons.Parameters.Add("@NOM_EMAIL", SqlDbType.VarChar).Value = txtEmail.Text.ToString();
                    cmdInsCons.Parameters.Add("@NOM_CELULAR", SqlDbType.VarChar).Value = txtCelular.Text.ToString();

                    // Endereço
                    cmdInsCons.Parameters.Add("@NOM_ENDERECO", SqlDbType.VarChar).Value = txtEndereco.Text.ToString();
                    cmdInsCons.Parameters.Add("@NOM_NUMERO", SqlDbType.VarChar).Value = txtNumero.Text.ToString();
                    cmdInsCons.Parameters.Add("@NOM_COMPLEMENTO", SqlDbType.VarChar).Value = txtComplemento.Text.ToString();
                    cmdInsCons.Parameters.Add("@NOM_BAIRRO", SqlDbType.VarChar).Value = txtBairro.Text.ToString();
                    cmdInsCons.Parameters.Add("@NOM_CIDADE", SqlDbType.VarChar).Value = txtCidade.Text.ToString();
                    cmdInsCons.Parameters.Add("@NOM_UF", SqlDbType.VarChar).Value = ddlEstado.SelectedValue.ToString();
                    cmdInsCons.Parameters.Add("@NOM_CEP", SqlDbType.VarChar).Value = txtCEP.Text.ToString();
                    cmdInsCons.Parameters.Add("@NOM_PAIS", SqlDbType.VarChar).Value = "BR";

                    // Endereço Entrega
                    cmdInsCons.Parameters.Add("@FLG_ENDERECO_ENTREGA", SqlDbType.Char).Value = (ckbEnderecoEntrega.Checked == true) ? "S" : "N";

                    cmdInsCons.Parameters.Add("@NOM_ENDERECO_ENTREGA", SqlDbType.VarChar).Value = txtEnderecoEntrega.Text.ToString();
                    cmdInsCons.Parameters.Add("@NOM_NUMERO_ENTREGA", SqlDbType.VarChar).Value = txtNumeroEntrega.Text.ToString();
                    cmdInsCons.Parameters.Add("@NOM_COMPLEMENTO_ENTREGA", SqlDbType.VarChar).Value = txtComplementoEntrega.Text.ToString();
                    cmdInsCons.Parameters.Add("@NOM_BAIRRO_ENTREGA", SqlDbType.VarChar).Value = txtBairroEntrega.Text.ToString();
                    cmdInsCons.Parameters.Add("@NOM_CIDADE_ENTREGA", SqlDbType.VarChar).Value = txtCidadeEntrega.Text.ToString();
                    cmdInsCons.Parameters.Add("@NOM_UF_ENTREGA", SqlDbType.VarChar).Value = ddlEstadoEntrega.SelectedValue.ToString();
                    cmdInsCons.Parameters.Add("@NOM_CEP_ENTREGA", SqlDbType.VarChar).Value = txtCEPEntrega.Text.ToString();


                    cmdInsCons.Parameters.Add("@DES_JSON", SqlDbType.Text).Value = jsonURLCobranca.ToString();

                    string sURLBoleto = "";
                    try
                    {
                        sURLBoleto = "https://" + Request.ServerVariables["SERVER_NAME"].ToString() + "/linkpagamento.aspx?id=" + Funcoes.Encrypt(oURLCobranca["id"].ToString()) + "&tipo=" + Funcoes.Encrypt("LP");
                    }
                    catch
                    {
                        ClientScript.RegisterStartupScript(this.GetType(), "Mensagem", "alert('Não foi possível gerar o QRCode para pagamento via pix! " + oURLCobranca["errors"][0]["description"] + "');", true);
                        return;
                    }
                    txtBoleto.Text = sURLBoleto.ToString();
                    cmdInsCons.Parameters.Add("@NOM_BOLETO", SqlDbType.VarChar).Value = sURLBoleto.ToString();
                    cmdInsCons.Parameters.Add("@NOM_BARCODE_BOLETO", SqlDbType.VarChar).Value = "";
                    cmdInsCons.Parameters.Add("@NOM_PIX", SqlDbType.VarChar).Value = "";

                    cmdInsCons.ExecuteNonQuery();
                    connInsCons.Close();
                    connInsCons.Dispose();

                    SqlConnection myConsultaBoleto = new SqlConnection(Funcoes.conexao());
                    myConsultaBoleto.Open();
                    SqlDataAdapter SDAConsultaBoleto = new SqlDataAdapter("dbo.stp_vendas_ins", myConsultaBoleto);
                    SDAConsultaBoleto.SelectCommand.CommandType = CommandType.StoredProcedure;
                    SDAConsultaBoleto.SelectCommand.Parameters.Add("@flg_operacao", SqlDbType.Char).Value = "L";
                    SDAConsultaBoleto.SelectCommand.Parameters.Add("@COD_ID_PESSOA_LICENCIADO", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["LICENCIADO"].ToString());
                    SDAConsultaBoleto.SelectCommand.Parameters.Add("@NOM_CODE", SqlDbType.VarChar).Value = oURLCobranca["id"].ToString();
                    DataSet dsConsultaBoleto = new DataSet();
                    SDAConsultaBoleto.Fill(dsConsultaBoleto, "VENDAS_VENDAS");
                    rptBoleto.DataSource = dsConsultaBoleto.Tables["VENDAS_VENDAS"].DefaultView;
                    rptBoleto.DataBind();
                    myConsultaBoleto.Close();
                    myConsultaBoleto.Dispose();

                    txtStatus.Text = "Transação por Boleto retornou status: (" + oURLCobranca["status"].ToString() + ")!";
                    txtTransacao.Text = oURLCobranca["id"].ToString();
                    Funcoes.GravaAuditoria(Funcoes.strToInt(sid_id), Request.ServerVariables["REMOTE_ADDR"].ToString(), "Checkout", "Pagamento PIX - Transação Criada: " + oURLCobranca["status"].ToString());

                }
                else
                {
                    ClientScript.RegisterStartupScript(this.GetType(), "EnviarDadosBoleto", "alert('Não foi possível gerar o QRCode para pagamento via PIX! Verifique e tente novamente.');", true);
                    txtStatus.Text = "";
                    txtTransacao.Text = "Falha ao tentar realizar o pagamento";
                    Funcoes.GravaAuditoria(Funcoes.strToInt(sid_id), Request.ServerVariables["REMOTE_ADDR"].ToString(), "Checkout", "Pagamento PIX: - Erro Transação: " + txtStatus.Text.ToString());

                }



            }
            else
            {
                ClientScript.RegisterStartupScript(this.GetType(), "ErroGerarBoletoCliente", "alert('Não foi possível gerar o boleto bancário! Verifique e tente novamente.');", true);
                txtStatus.Text = "";
                txtTransacao.Text = "Falha ao tentar realizar o pagamento";
                Funcoes.GravaAuditoria(Funcoes.strToInt(sid_id), Request.ServerVariables["REMOTE_ADDR"].ToString(), "Checkout", "Pagamento Boleto: - Erro Transação: " + txtStatus.Text.ToString());
            }


            cbPagamento.Visible = false;
            cbConfirmacao.Visible = true;
            cbBoleto.Visible = true;

            btnConfirmar.Visible = false;
            btnVoltar.Visible = true;
        }


    }

    
    
    private void GerarTransacaoBoletoAsaas(double vValorTotal)
    {
        //ClientScript.RegisterStartupScript(this.GetType(),
        //    "BoletoAsaas", "alert('Venda boleto ASAAS');  ", true);

        string retornoInclusao = "";
        string sTokenComprador = "";

        SqlConnection mySelCadastro = new SqlConnection(Funcoes.conexao());
        mySelCadastro.Open();
        SqlCommand cmdSelCadastro = new SqlCommand("dbo.stp_vendas_ins", mySelCadastro);
        cmdSelCadastro.CommandType = CommandType.StoredProcedure;
        cmdSelCadastro.Parameters.Add("@flg_operacao", SqlDbType.Char).Value = "O";
        cmdSelCadastro.Parameters.Add("@COD_ID", SqlDbType.Int).Value = Funcoes.strToInt(sid_id.ToString());
        SqlDataReader ReaderCadastro = cmdSelCadastro.ExecuteReader();

        while (ReaderCadastro.Read())
        {
            
            string sIDCliente = asaas.VerificaCliente("", "", txtNome.Text.ToString(), txtDocumento.Text.ToString(), txtEmail.Text.ToString(), txtCEP.Text.ToString(),txtNumero.Text.ToString(), txtCelular.Text.ToString());
            if (sIDCliente.ToString().Trim() != "")
            {
                GravaCliente(sIDCliente.ToString());

                //***************************

                DadosCobrancaAsaaS.Root dcobranca = new DadosCobrancaAsaaS.Root()
                {
                    customer = sIDCliente.ToString(),
                    billingType = "BOLETO",
                    value = Funcoes.strToDouble(ReaderCadastro["NUM_VALOR"].ToString()),
                    dueDate = Convert.ToDateTime(ReaderCadastro["DTA_VENCIMENTO"].ToString()).Year.ToString() + "-" + Convert.ToDateTime(ReaderCadastro["DTA_VENCIMENTO"].ToString()).Month.ToString() + "-" + Convert.ToDateTime(ReaderCadastro["DTA_VENCIMENTO"].ToString()).Day.ToString(),
                    daysAfterDueDateToRegistrationCancellation = Funcoes.strToInt(ReaderCadastro["NUM_DIAS"].ToString()),
                    description = ReaderCadastro["NOM_DESCRICAO_PRODUTO"].ToString(),
                    externalReference = ReaderCadastro["NOM_REFERENCIA_PRODUTO"].ToString()
                };


                SqlConnection mySelCadastroSplit = new SqlConnection(Funcoes.conexao());
                mySelCadastroSplit.Open();
                SqlCommand cmdSelCadastroSplit = new SqlCommand("dbo.stp_pessoas_fj_split_ins", mySelCadastroSplit);
                cmdSelCadastroSplit.CommandType = CommandType.StoredProcedure;
                cmdSelCadastroSplit.Parameters.Add("@flg_operacao", SqlDbType.Char).Value = "C";
                cmdSelCadastroSplit.Parameters.Add("@COD_ID_PESSOAS_FJ", SqlDbType.Int).Value = Funcoes.strToInt(ReaderCadastro["COD_ID_PESSOAS_FJ_VENDEDOR"].ToString());
                cmdSelCadastroSplit.Parameters.Add("@COD_ID_PESSOA_LICENCIADO", SqlDbType.Int).Value = Funcoes.strToInt(ReaderCadastro["COD_ID_PESSOA_LICENCIADO"].ToString());
                SqlDataReader ReaderCadastroSplit = cmdSelCadastroSplit.ExecuteReader();

                List<DadosCobrancaAsaaS.split> listaSplits = new List<DadosCobrancaAsaaS.split>();
                int iContadorSplit = 0;
                while (ReaderCadastroSplit.Read()) // Percorre os registros do banco
                {
                    iContadorSplit = iContadorSplit + 1;
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
                if (iContadorSplit > 0) { dcobranca.split = listaSplits.ToArray(); }


                string jsonCobranca = JsonConvert.SerializeObject(dcobranca);
                string jsonURLCobranca = asaas.CriarCobranca(asaas.PegarTokenSubconta(Funcoes.strToInt(HttpContext.Current.Session["PESSOA"].ToString()), Funcoes.strToInt(HttpContext.Current.Session["LICENCIADO"].ToString())), jsonCobranca);

                //****************************
                //string jsonURLCobranca = GerarCobranca(sIDCliente, ReaderCadastro["NUM_VALOR"].ToString(), ReaderCadastro["DTA_VENCIMENTO"].ToString(), "2", ReaderCadastro["NOM_DESCRICAO_PRODUTO"].ToString(), ReaderCadastro["NOM_REFERENCIA_PRODUTO"].ToString(), "BOLETO");

                if (jsonURLCobranca.ToString().Trim() != "")
                {
                    JObject oURLCobranca = JObject.Parse(jsonURLCobranca.ToString());

                    try
                    {

                        SqlConnection connInsCons = new SqlConnection(Funcoes.conexao());
                        connInsCons.Open();
                        SqlCommand cmdInsCons = new SqlCommand("dbo.stp_vendas_vendas_ins", connInsCons);
                        cmdInsCons.CommandType = CommandType.StoredProcedure;
                        cmdInsCons.Parameters.Add("@flg_operacao", SqlDbType.Char).Value = 'I';
                        cmdInsCons.Parameters.Add("@COD_ID", SqlDbType.Int).Value = Funcoes.strToInt(sid_id.ToString());

                        cmdInsCons.Parameters.Add("@COD_ID_PESSOA_LICENCIADO", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["LICENCIADO"].ToString());
                        cmdInsCons.Parameters.Add("@COD_ID_PESSOAS_FJ_VENDEDOR", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["PESSOA"].ToString());
                        // Buscar dados comprador

                        cmdInsCons.Parameters.Add("@COD_ID_VENDAS", SqlDbType.Int).Value = Funcoes.strToInt(sid_id.ToString());

                        cmdInsCons.Parameters.Add("@NUM_VALOR", SqlDbType.Float).Value = vValorTotal;
                        if (ReaderCadastro["DTA_VENCIMENTO"].ToString().Trim() != "") { cmdInsCons.Parameters.Add("@DTA_VENCIMENTO", SqlDbType.DateTime).Value = Convert.ToDateTime(ReaderCadastro["DTA_VENCIMENTO"].ToString()); }

                        cmdInsCons.Parameters.Add("@FLG_BOLETO", SqlDbType.VarChar).Value = "S";

                        cmdInsCons.Parameters.Add("@NOM_CODE", SqlDbType.VarChar).Value = oURLCobranca["id"].ToString().Trim();
                        cmdInsCons.Parameters.Add("@NOM_CONTEUDO_CAMPO_01", SqlDbType.VarChar).Value = txtConteudo01.Text.ToString();

                        // Cliente
                        cmdInsCons.Parameters.Add("@NOM_NOME", SqlDbType.VarChar).Value = txtNome.Text.ToString();
                        cmdInsCons.Parameters.Add("@NOM_SOBRENOME", SqlDbType.VarChar).Value = txtSobrenome.Text.ToString();
                        cmdInsCons.Parameters.Add("@NUM_CNPJCPF", SqlDbType.VarChar).Value = txtDocumento.Text.ToString();
                        cmdInsCons.Parameters.Add("@NOM_EMAIL", SqlDbType.VarChar).Value = txtEmail.Text.ToString();
                        cmdInsCons.Parameters.Add("@NOM_CELULAR", SqlDbType.VarChar).Value = txtCelular.Text.ToString();

                        // Endereço
                        cmdInsCons.Parameters.Add("@NOM_ENDERECO", SqlDbType.VarChar).Value = txtEndereco.Text.ToString();
                        cmdInsCons.Parameters.Add("@NOM_NUMERO", SqlDbType.VarChar).Value = txtNumero.Text.ToString();
                        cmdInsCons.Parameters.Add("@NOM_COMPLEMENTO", SqlDbType.VarChar).Value = txtComplemento.Text.ToString();
                        cmdInsCons.Parameters.Add("@NOM_BAIRRO", SqlDbType.VarChar).Value = txtBairro.Text.ToString();
                        cmdInsCons.Parameters.Add("@NOM_CIDADE", SqlDbType.VarChar).Value = txtCidade.Text.ToString();
                        cmdInsCons.Parameters.Add("@NOM_UF", SqlDbType.VarChar).Value = ddlEstado.SelectedValue.ToString();
                        cmdInsCons.Parameters.Add("@NOM_CEP", SqlDbType.VarChar).Value = txtCEP.Text.ToString();
                        cmdInsCons.Parameters.Add("@NOM_PAIS", SqlDbType.VarChar).Value = "BR";

                        // Endereço Entrega
                        cmdInsCons.Parameters.Add("@FLG_ENDERECO_ENTREGA", SqlDbType.Char).Value = (ckbEnderecoEntrega.Checked == true) ? "S" : "N";

                        cmdInsCons.Parameters.Add("@NOM_ENDERECO_ENTREGA", SqlDbType.VarChar).Value = txtEnderecoEntrega.Text.ToString();
                        cmdInsCons.Parameters.Add("@NOM_NUMERO_ENTREGA", SqlDbType.VarChar).Value = txtNumeroEntrega.Text.ToString();
                        cmdInsCons.Parameters.Add("@NOM_COMPLEMENTO_ENTREGA", SqlDbType.VarChar).Value = txtComplementoEntrega.Text.ToString();
                        cmdInsCons.Parameters.Add("@NOM_BAIRRO_ENTREGA", SqlDbType.VarChar).Value = txtBairroEntrega.Text.ToString();
                        cmdInsCons.Parameters.Add("@NOM_CIDADE_ENTREGA", SqlDbType.VarChar).Value = txtCidadeEntrega.Text.ToString();
                        cmdInsCons.Parameters.Add("@NOM_UF_ENTREGA", SqlDbType.VarChar).Value = ddlEstadoEntrega.SelectedValue.ToString();
                        cmdInsCons.Parameters.Add("@NOM_CEP_ENTREGA", SqlDbType.VarChar).Value = txtCEPEntrega.Text.ToString();


                        cmdInsCons.Parameters.Add("@DES_JSON", SqlDbType.Text).Value = jsonURLCobranca.ToString();

                        string sURLBoleto = "";
                        try
                        {
                            sURLBoleto = "https://" + Request.ServerVariables["SERVER_NAME"].ToString() + "/linkpagamento.aspx?id=" + Funcoes.Encrypt(oURLCobranca["id"].ToString()) + "&tipo=" + Funcoes.Encrypt("LB");
                        }
                        catch
                        {
                            ClientScript.RegisterStartupScript(this.GetType(), "Mensagem", "alert('Não foi possível gerar o boleto bancário! " + oURLCobranca["errors"][0]["description"] + "');", true);
                            return;
                        }
                        txtBoleto.Text = sURLBoleto.ToString();
                        cmdInsCons.Parameters.Add("@NOM_BOLETO", SqlDbType.VarChar).Value = sURLBoleto.ToString();
                        cmdInsCons.Parameters.Add("@NOM_BARCODE_BOLETO", SqlDbType.VarChar).Value = "";
                        cmdInsCons.Parameters.Add("@NOM_PIX", SqlDbType.VarChar).Value = "";

                        cmdInsCons.ExecuteNonQuery();
                        connInsCons.Close();
                        connInsCons.Dispose();

                        SqlConnection myConsultaBoleto = new SqlConnection(Funcoes.conexao());
                        myConsultaBoleto.Open();
                        SqlDataAdapter SDAConsultaBoleto = new SqlDataAdapter("dbo.stp_vendas_ins", myConsultaBoleto);
                        SDAConsultaBoleto.SelectCommand.CommandType = CommandType.StoredProcedure;
                        SDAConsultaBoleto.SelectCommand.Parameters.Add("@flg_operacao", SqlDbType.Char).Value = "L";
                        SDAConsultaBoleto.SelectCommand.Parameters.Add("@COD_ID_PESSOA_LICENCIADO", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["LICENCIADO"].ToString());
                        SDAConsultaBoleto.SelectCommand.Parameters.Add("@NOM_CODE", SqlDbType.VarChar).Value = oURLCobranca["id"].ToString();
                        DataSet dsConsultaBoleto = new DataSet();
                        SDAConsultaBoleto.Fill(dsConsultaBoleto, "VENDAS_VENDAS");
                        rptBoleto.DataSource = dsConsultaBoleto.Tables["VENDAS_VENDAS"].DefaultView;
                        rptBoleto.DataBind();
                        myConsultaBoleto.Close();
                        myConsultaBoleto.Dispose();

                        txtStatus.Text = "Transação por Boleto retornou status: (" + oURLCobranca["status"].ToString() + ")!";
                        txtTransacao.Text = oURLCobranca["id"].ToString();
                        Funcoes.GravaAuditoria(Funcoes.strToInt(sid_id), Request.ServerVariables["REMOTE_ADDR"].ToString(), "Checkout", "Pagamento Boleto - Transação Criada: " + oURLCobranca["status"].ToString());
                    }
                    catch
                    {
                        ClientScript.RegisterStartupScript(this.GetType(), "Mensagem", "alert('Não foi possível gerar o boleto bancário! " + oURLCobranca["errors"][0]["description"] + "');", true);
                        return;

                    }
                }
                else
                {
                    ClientScript.RegisterStartupScript(this.GetType(), "EnviarDadosBoleto", "alert('Não foi possível gerar o boleto bancário! Verifique e tente novamente.');", true);
                    txtStatus.Text = "";
                    txtTransacao.Text = "Falha ao tentar realizar o pagamento";
                    Funcoes.GravaAuditoria(Funcoes.strToInt(sid_id), Request.ServerVariables["REMOTE_ADDR"].ToString(), "Checkout", "Pagamento Boleto: - Erro Transação: " + txtStatus.Text.ToString());

                }



            }
            else
            {
                ClientScript.RegisterStartupScript(this.GetType(), "ErroGerarBoletoCliente", "alert('Não foi possível gerar o boleto bancário! Verifique e tente novamente.');", true);
                txtStatus.Text = "";
                txtTransacao.Text = "Falha ao tentar realizar o pagamento";
                Funcoes.GravaAuditoria(Funcoes.strToInt(sid_id), Request.ServerVariables["REMOTE_ADDR"].ToString(), "Checkout", "Pagamento Boleto: - Erro Transação: " + txtStatus.Text.ToString());
            }


            cbPagamento.Visible = false;
            cbConfirmacao.Visible = true;
            cbBoleto.Visible = true;

            btnConfirmar.Visible = false;
            btnVoltar.Visible = true;
        }


    }


    
    private void GerarTransacaoBoleto(double vValorTotal)
    {
        // ******************************************************************************************************************************************************************
        // Operação de Boleto ***********************************************************************************************************************************************
        // ******************************************************************************************************************************************************************

        string retornoInclusao = "";
        string sTokenComprador = "";

        SqlConnection mySelCadastro = new SqlConnection(Funcoes.conexao());
        mySelCadastro.Open();
        SqlCommand cmdSelCadastro = new SqlCommand("dbo.stp_vendas_ins", mySelCadastro);
        cmdSelCadastro.CommandType = CommandType.StoredProcedure;
        cmdSelCadastro.Parameters.Add("@flg_operacao", SqlDbType.Char).Value = "O";
        cmdSelCadastro.Parameters.Add("@COD_ID", SqlDbType.Int).Value = Funcoes.strToInt(sid_id.ToString());
        SqlDataReader ReaderCadastro = cmdSelCadastro.ExecuteReader();

        while (ReaderCadastro.Read())
        {
            if ((ReaderCadastro["COD_ID_PESSOAS_FJ_COMPRADOR"].ToString().Trim() == "") || (ReaderCadastro["NOM_TOKEN_COMPRADOR"].ToString().Trim() == ""))
            {
                // Registrar o comprador
                int iCampos = 0;
                if (
                    (txtNome.Text.ToString().Trim() == "") ||
                    (txtSobrenome.Text.ToString().Trim() == "") ||
                    (txtEmail.Text.ToString().Trim() == "") ||
                    (txtCelular.Text.ToString().Trim() == "") ||
                    (txtDocumento.Text.ToString().Trim() == "") ||
                    (txtEndereco.Text.ToString().Trim() == "") ||
                    (txtComplemento.Text.ToString().Trim() == "") ||
                    (txtBairro.Text.ToString().Trim() == "") ||
                    (txtCidade.Text.ToString().Trim() == "") ||
                    (ddlEstado.SelectedValue.ToString().Trim() == "") ||
                    (Funcoes.TIRAACENTOS(txtCEP.Text.ToString()) == "")
                    )
                {
                    iCampos = iCampos + 1;
                }
                if (iCampos <= 0)
                {
                    // Salva os dados do cliente na ZOOP no caso do documento não existir
                    retornoInclusao = "";

                    bool bClienteExiste = false;
                    SqlConnection mySelCadastroComprador = new SqlConnection(Funcoes.conexao());
                    mySelCadastroComprador.Open();
                    SqlCommand cmdSelCadastroComprador = new SqlCommand("dbo.stp_compradores_ins", mySelCadastroComprador);
                    cmdSelCadastroComprador.CommandType = CommandType.StoredProcedure;
                    cmdSelCadastroComprador.Parameters.Add("@flg_operacao", SqlDbType.Char).Value = "H";
                    cmdSelCadastroComprador.Parameters.Add("@NUM_CNPJCPF", SqlDbType.VarChar).Value = txtDocumento.Text.ToString();
                    cmdSelCadastroComprador.Parameters.Add("@COD_ID_PESSOA_LICENCIADO", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["LICENCIADO"].ToString());
                    cmdSelCadastroComprador.Parameters.Add("@COD_ID_PESSOAS_FJ", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["PESSOA"].ToString());
                    cmdSelCadastroComprador.Parameters.Add("@FLG_TIPO", SqlDbType.Char).Value = "C";

                    SqlDataReader ReaderCadastroComprador = cmdSelCadastroComprador.ExecuteReader();
                    while (ReaderCadastroComprador.Read())
                    {
                        bClienteExiste = true;
                        retornoInclusao = ReaderCadastroComprador["NOM_TOKEN_ZOOP_COMPRADOR"].ToString();
                        sTokenComprador = ReaderCadastroComprador["NOM_TOKEN_ZOOP_COMPRADOR"].ToString();

                            string jsonCompradorBusca = zoop.compradores("P", txtDocumento.Text.ToString(), "");

                            try
                            {
                                JObject oRetornoCompradorBusca = JObject.Parse(jsonCompradorBusca);
                                // Verificar se este comprador já existe ou não
                                sTokenComprador = oRetornoCompradorBusca["id"].ToString();

                                retornoInclusao = sTokenComprador;
                                sTokenComprador = sTokenComprador;
                            }
                            catch
                            {

                            }


                    }

                    if (bClienteExiste == false)
                    {
                        dadosComprador.Comprador dcomprador = new dadosComprador.Comprador()
                        {
                            first_name = txtNome.Text.ToString(),
                            last_name = txtSobrenome.Text.ToString(),
                            email = txtEmail.Text.ToString(),
                            phone_number = txtCelular.Text.ToString(),
                            taxpayer_id = Funcoes.TIRAACENTOS(txtDocumento.Text.ToString()),
                            address = new dadosComprador.address()
                            {
                                line1 = txtEndereco.Text.ToString(),
                                line2 = txtComplemento.Text.ToString(),
                                line3 = "",
                                neighborhood = txtBairro.Text.ToString(),
                                city = txtCidade.Text.ToString(),
                                state = ddlEstado.SelectedValue.ToString(),
                                postal_code = Funcoes.TIRAACENTOS(txtCEP.Text.ToString()),
                                country_code = "BR"
                            }
                        };

                        string json = JsonConvert.SerializeObject(dcomprador);
                        string jsonRetornoComprador = zoop.compradores("I", "", json);

                        JObject oRetornoComprador = JObject.Parse(jsonRetornoComprador);
                        try
                        {
                            // Verificar se este comprador já existe ou não
                            sTokenComprador = oRetornoComprador["id"].ToString();
                        }
                        catch
                        {

                        }
                    }
                    // Grava os dados do cliente
                    SqlConnection connInsConsCliente = new SqlConnection(Funcoes.conexao());
                    connInsConsCliente.Open();
                    SqlCommand cmdInsConsCliente = new SqlCommand("dbo.stp_compradores_ins", connInsConsCliente);
                    cmdInsConsCliente.CommandType = CommandType.StoredProcedure;

                    cmdInsConsCliente.Parameters.Add("@flg_operacao", SqlDbType.Char).Value = 'I';

                    cmdInsConsCliente.Parameters.Add("@FLG_ATIVO", SqlDbType.Char).Value = "S";

                    cmdInsConsCliente.Parameters.Add("@COD_ID_PESSOA_LICENCIADO", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["LICENCIADO"].ToString());
                    cmdInsConsCliente.Parameters.Add("@COD_ID_PESSOAS_FJ", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["PESSOA"].ToString());
                    cmdInsConsCliente.Parameters.Add("@NOM_TOKEN_ZOOP_COMPRADOR", SqlDbType.VarChar).Value = sTokenComprador.ToString();

                    cmdInsConsCliente.Parameters.Add("@DTA_DATA", SqlDbType.DateTime).Value = DateTime.Now;
                    cmdInsConsCliente.Parameters.Add("@FLG_TIPO", SqlDbType.Char).Value = "C";

                    // Responsável
                    cmdInsConsCliente.Parameters.Add("@NOM_NOME", SqlDbType.VarChar).Value = txtNome.Text.ToString();
                    cmdInsConsCliente.Parameters.Add("@NOM_SOBRENOME", SqlDbType.VarChar).Value = txtSobrenome.Text.ToString();
                    cmdInsConsCliente.Parameters.Add("@NUM_CNPJCPF", SqlDbType.VarChar).Value = txtDocumento.Text.ToString();
                    cmdInsConsCliente.Parameters.Add("@NOM_EMAIL", SqlDbType.VarChar).Value = txtEmail.Text.ToString();
                    cmdInsConsCliente.Parameters.Add("@NOM_CELULAR", SqlDbType.VarChar).Value = txtCelular.Text.ToString();

                    // Endereço
                    cmdInsConsCliente.Parameters.Add("@NOM_ENDERECO", SqlDbType.VarChar).Value = txtEndereco.Text.ToString();
                    cmdInsConsCliente.Parameters.Add("@NOM_NUMERO", SqlDbType.VarChar).Value = txtNumero.Text.ToString();
                    cmdInsConsCliente.Parameters.Add("@NOM_COMPLEMENTO", SqlDbType.VarChar).Value = txtComplemento.Text.ToString();
                    cmdInsConsCliente.Parameters.Add("@NOM_BAIRRO", SqlDbType.VarChar).Value = txtBairro.Text.ToString();
                    cmdInsConsCliente.Parameters.Add("@NOM_CIDADE", SqlDbType.VarChar).Value = txtCidade.Text.ToString();
                    cmdInsConsCliente.Parameters.Add("@NOM_UF", SqlDbType.VarChar).Value = ddlEstado.SelectedValue.ToString();
                    cmdInsConsCliente.Parameters.Add("@NOM_CEP", SqlDbType.VarChar).Value = txtCEP.Text.ToString();
                    cmdInsConsCliente.Parameters.Add("@NOM_PAIS", SqlDbType.VarChar).Value = "BR";

                    cmdInsConsCliente.ExecuteNonQuery();
                    connInsConsCliente.Close();
                    connInsConsCliente.Dispose();

                }
                else
                {
                    ClientScript.RegisterStartupScript(this.GetType(),
                       "DadosCliente", "alert('Todos os dados do cliente são de preenchimento obrigatório!');", true);
                    return;
                }

            }
            else
            {
                sTokenComprador = ReaderCadastro["NOM_TOKEN_COMPRADOR"].ToString().Trim();
            }

            string urlorigem = Request.ServerVariables["SERVER_NAME"].ToString();
            string urlLogotipo = "https://" + urlorigem + "/" + imgLogoPrincipal.Src.ToString();

            DadosTransacao.TransacaoBoleto dtransacao = new DadosTransacao.TransacaoBoleto()
            {
                on_behalf_of = ReaderCadastro["NUM_TOKEN"].ToString(),
                customer = sTokenComprador,
                amount = Funcoes.strToInt(Convert.ToString(vValorTotal * 100).ToString()),
                currency = "BRL",
                description = ReaderCadastro["NOM_DESCRICAO_PRODUTO"].ToString(),
                payment_type = "boleto",
                reference_id = ReaderCadastro["NOM_REFERENCIA_PRODUTO"].ToString(),
                logo = urlLogotipo.ToString(),

                payment_method = new DadosTransacao.payment_method()
                {
                    expiration_date = (ReaderCadastro["DTA_VENCIMENTO"].ToString().Trim() == "") ? "" : String.Format("{0:u}", Convert.ToDateTime(ReaderCadastro["DTA_VENCIMENTO"].ToString().Trim())),
                    payment_limit_date = null,
                    billing_instructions = null,
                    body_instructions = null
                },
            };

            // Verifica a existência de split Manual
            SqlConnection mySelCadastroSplit = new SqlConnection(Funcoes.conexao());
            mySelCadastroSplit.Open();
            SqlCommand cmdSelCadastroSplit = new SqlCommand("dbo.stp_vendas_parceiros_ins", mySelCadastroSplit);
            cmdSelCadastroSplit.CommandType = CommandType.StoredProcedure;
            cmdSelCadastroSplit.Parameters.Add("@flg_operacao", SqlDbType.Char).Value = "C";
            cmdSelCadastroSplit.Parameters.Add("@COD_ID_VENDAS", SqlDbType.Int).Value = Funcoes.strToInt(ReaderCadastro["COD_ID"].ToString());
            cmdSelCadastroSplit.Parameters.Add("@COD_ID_PESSOA_LICENCIADO", SqlDbType.Int).Value = Funcoes.strToInt(ReaderCadastro["COD_ID_PESSOA_LICENCIADO"].ToString());
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
                        amount = Funcoes.strToInt((Funcoes.strToDouble(ReaderCadastroSplit["NUM_PERCENTUAL"].ToString())*100).ToString()),
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
                    percentage= Funcoes.strToDouble(ReaderCadastroSplit["NUM_PERCENTUAL"].ToString()),
                });
                }
            }

            // Converte a lista para array antes de atribuir ao objeto
            if (iContadorSplit > 0) { dtransacao.split_rules = listaSplits.ToArray(); }
            // Fim Split Manual

            string jsonBoleto = JsonConvert.SerializeObject(dtransacao);

            dvBoleto.Visible = true;

            string jsonRetorno = zoop.transacao("I", "", jsonBoleto);
            JObject oRetorno = JObject.Parse(jsonRetorno);

            try
            {
                if ((oRetorno["status"].ToString().Trim() != "") && (oRetorno["id"].ToString().Trim() != ""))
                {

                    string sStatusTransacao = VerificaStatus(oRetorno["status"].ToString().Trim());
                    // Gravar dados atualizados na tabela VENDAS

                    SqlConnection connInsCons = new SqlConnection(Funcoes.conexao());
                    connInsCons.Open();
                    SqlCommand cmdInsCons = new SqlCommand("dbo.stp_vendas_vendas_ins", connInsCons);
                    cmdInsCons.CommandType = CommandType.StoredProcedure;
                    cmdInsCons.Parameters.Add("@flg_operacao", SqlDbType.Char).Value = 'I';
                    cmdInsCons.Parameters.Add("@COD_ID", SqlDbType.Int).Value = Funcoes.strToInt(sid_id.ToString());

                    cmdInsCons.Parameters.Add("@COD_ID_PESSOA_LICENCIADO", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["LICENCIADO"].ToString());
                    cmdInsCons.Parameters.Add("@COD_ID_PESSOAS_FJ_VENDEDOR", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["PESSOA"].ToString());
                    // Buscar dados comprador

                    cmdInsCons.Parameters.Add("@COD_ID_VENDAS", SqlDbType.Int).Value = Funcoes.strToInt(sid_id.ToString());

                    cmdInsCons.Parameters.Add("@NUM_VALOR", SqlDbType.Float).Value = vValorTotal;
                    if (ReaderCadastro["DTA_VENCIMENTO"].ToString().Trim() != "") { cmdInsCons.Parameters.Add("@DTA_VENCIMENTO", SqlDbType.DateTime).Value = Convert.ToDateTime(ReaderCadastro["DTA_VENCIMENTO"].ToString()); }

                    cmdInsCons.Parameters.Add("@FLG_BOLETO", SqlDbType.VarChar).Value = "S";

                    cmdInsCons.Parameters.Add("@NOM_CODE", SqlDbType.VarChar).Value = oRetorno["id"].ToString().Trim();
                    cmdInsCons.Parameters.Add("@NOM_CONTEUDO_CAMPO_01", SqlDbType.VarChar).Value = txtConteudo01.Text.ToString();

                    // Cliente
                    cmdInsCons.Parameters.Add("@NOM_NOME", SqlDbType.VarChar).Value = txtNome.Text.ToString();
                    cmdInsCons.Parameters.Add("@NOM_SOBRENOME", SqlDbType.VarChar).Value = txtSobrenome.Text.ToString();
                    cmdInsCons.Parameters.Add("@NUM_CNPJCPF", SqlDbType.VarChar).Value = txtDocumento.Text.ToString();
                    cmdInsCons.Parameters.Add("@NOM_EMAIL", SqlDbType.VarChar).Value = txtEmail.Text.ToString();
                    cmdInsCons.Parameters.Add("@NOM_CELULAR", SqlDbType.VarChar).Value = txtCelular.Text.ToString();

                    // Endereço
                    cmdInsCons.Parameters.Add("@NOM_ENDERECO", SqlDbType.VarChar).Value = txtEndereco.Text.ToString();
                    cmdInsCons.Parameters.Add("@NOM_NUMERO", SqlDbType.VarChar).Value = txtNumero.Text.ToString();
                    cmdInsCons.Parameters.Add("@NOM_COMPLEMENTO", SqlDbType.VarChar).Value = txtComplemento.Text.ToString();
                    cmdInsCons.Parameters.Add("@NOM_BAIRRO", SqlDbType.VarChar).Value = txtBairro.Text.ToString();
                    cmdInsCons.Parameters.Add("@NOM_CIDADE", SqlDbType.VarChar).Value = txtCidade.Text.ToString();
                    cmdInsCons.Parameters.Add("@NOM_UF", SqlDbType.VarChar).Value = ddlEstado.SelectedValue.ToString();
                    cmdInsCons.Parameters.Add("@NOM_CEP", SqlDbType.VarChar).Value = txtCEP.Text.ToString();
                    cmdInsCons.Parameters.Add("@NOM_PAIS", SqlDbType.VarChar).Value = "BR";

                    // Endereço Entrega
                    cmdInsCons.Parameters.Add("@FLG_ENDERECO_ENTREGA", SqlDbType.Char).Value = (ckbEnderecoEntrega.Checked == true) ? "S" : "N";

                    cmdInsCons.Parameters.Add("@NOM_ENDERECO_ENTREGA", SqlDbType.VarChar).Value = txtEnderecoEntrega.Text.ToString();
                    cmdInsCons.Parameters.Add("@NOM_NUMERO_ENTREGA", SqlDbType.VarChar).Value = txtNumeroEntrega.Text.ToString();
                    cmdInsCons.Parameters.Add("@NOM_COMPLEMENTO_ENTREGA", SqlDbType.VarChar).Value = txtComplementoEntrega.Text.ToString();
                    cmdInsCons.Parameters.Add("@NOM_BAIRRO_ENTREGA", SqlDbType.VarChar).Value = txtBairroEntrega.Text.ToString();
                    cmdInsCons.Parameters.Add("@NOM_CIDADE_ENTREGA", SqlDbType.VarChar).Value = txtCidadeEntrega.Text.ToString();
                    cmdInsCons.Parameters.Add("@NOM_UF_ENTREGA", SqlDbType.VarChar).Value = ddlEstadoEntrega.SelectedValue.ToString();
                    cmdInsCons.Parameters.Add("@NOM_CEP_ENTREGA", SqlDbType.VarChar).Value = txtCEPEntrega.Text.ToString();


                    cmdInsCons.Parameters.Add("@DES_JSON", SqlDbType.Text).Value = jsonRetorno.ToString();

                    string URL = oRetorno["payment_method"]["url"].ToString();
                    URL = URL.Replace("//", "/").Replace(":/", "://");
                    txtBoleto.Text = oRetorno["payment_method"]["barcode"].ToString();

                    cmdInsCons.Parameters.Add("@NOM_BOLETO", SqlDbType.VarChar).Value = URL.ToString();
                    cmdInsCons.Parameters.Add("@NOM_BARCODE_BOLETO", SqlDbType.VarChar).Value = oRetorno["payment_method"]["barcode"].ToString();
                    cmdInsCons.Parameters.Add("@NOM_PIX", SqlDbType.VarChar).Value = urlLogotipo.ToString();

                    cmdInsCons.ExecuteNonQuery();
                    connInsCons.Close();
                    connInsCons.Dispose();

                    SqlConnection myConsultaBoleto = new SqlConnection(Funcoes.conexao());
                    myConsultaBoleto.Open();
                    SqlDataAdapter SDAConsultaBoleto = new SqlDataAdapter("dbo.stp_vendas_ins", myConsultaBoleto);
                    SDAConsultaBoleto.SelectCommand.CommandType = CommandType.StoredProcedure;
                    SDAConsultaBoleto.SelectCommand.Parameters.Add("@flg_operacao", SqlDbType.Char).Value = "L";
                    SDAConsultaBoleto.SelectCommand.Parameters.Add("@COD_ID_PESSOA_LICENCIADO", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["LICENCIADO"].ToString());
                    SDAConsultaBoleto.SelectCommand.Parameters.Add("@NOM_CODE", SqlDbType.VarChar).Value = oRetorno["id"].ToString().Trim();
                    DataSet dsConsultaBoleto = new DataSet();
                    SDAConsultaBoleto.Fill(dsConsultaBoleto, "VENDAS_VENDAS");
                    rptBoleto.DataSource = dsConsultaBoleto.Tables["VENDAS_VENDAS"].DefaultView;
                    rptBoleto.DataBind();
                    myConsultaBoleto.Close();
                    myConsultaBoleto.Dispose();

                    txtStatus.Text = "Transação por Boleto retornou status: (" + sStatusTransacao.ToString() + ")!";
                    txtTransacao.Text = oRetorno["id"].ToString();
                    Funcoes.GravaAuditoria(Funcoes.strToInt(sid_id), Request.ServerVariables["REMOTE_ADDR"].ToString(), "Checkout", "Pagamento Boleto - Transação Criada: " + sStatusTransacao.ToString());
                }
            }
            catch
            {
                txtStatus.Text = zoop.status_code(oRetorno["error"]["status_code"].ToString());
                txtTransacao.Text = "Falha ao tentar realizar o pagamento";
                Funcoes.GravaAuditoria(Funcoes.strToInt(sid_id), Request.ServerVariables["REMOTE_ADDR"].ToString(), "Checkout", "Pagamento Boleto: - Erro Transação: " + txtStatus.Text.ToString());
            }


            cbPagamento.Visible = false;
            cbConfirmacao.Visible = true;
            cbBoleto.Visible = true;

            btnConfirmar.Visible = false;
            btnVoltar.Visible = true;

        }
    }


    private void GerarTransacaoPix(double vValorTotal)
    {

        // ******************************************************************************************************************************************************************
        // Operação PIX *****************************************************************************************************************************************************
        // ******************************************************************************************************************************************************************

        SqlConnection mySelCadastro = new SqlConnection(Funcoes.conexao());
        mySelCadastro.Open();
        SqlCommand cmdSelCadastro = new SqlCommand("dbo.stp_vendas_ins", mySelCadastro);
        cmdSelCadastro.CommandType = CommandType.StoredProcedure;
        cmdSelCadastro.Parameters.Add("@flg_operacao", SqlDbType.Char).Value = "O";
        cmdSelCadastro.Parameters.Add("@COD_ID", SqlDbType.Int).Value = Funcoes.strToInt(sid_id.ToString());
        SqlDataReader ReaderCadastro = cmdSelCadastro.ExecuteReader();

        while (ReaderCadastro.Read())
        {
            DadosTransacao.TransacaoPix dtransacao = new DadosTransacao.TransacaoPix()
            {
                on_behalf_of = ReaderCadastro["NUM_TOKEN"].ToString(),
                description = ReaderCadastro["NOM_REFERENCIA_PRODUTO"].ToString() + " / " + ReaderCadastro["NOM_DESCRICAO_PRODUTO"].ToString() + " - Comprador:" + ReaderCadastro["NOM_NOME"].ToString() + " " + ReaderCadastro["NOM_SOBRENOME"].ToString(),
                currency = "BRL",
                amount = Funcoes.strToInt(Convert.ToString(vValorTotal * 100).ToString()),
                payment_type = "pix",
                pix_expiration_date_time = (ReaderCadastro["DTA_PIX_VENCIMENTO"].ToString().Trim() == "") ? "" : String.Format("{0:u}", Convert.ToDateTime(ReaderCadastro["DTA_PIX_VENCIMENTO"].ToString().Trim()))
            };

            // Verifica a existência de split Manual
            SqlConnection mySelCadastroSplit = new SqlConnection(Funcoes.conexao());
            mySelCadastroSplit.Open();
            SqlCommand cmdSelCadastroSplit = new SqlCommand("dbo.stp_vendas_parceiros_ins", mySelCadastroSplit);
            cmdSelCadastroSplit.CommandType = CommandType.StoredProcedure;
            cmdSelCadastroSplit.Parameters.Add("@flg_operacao", SqlDbType.Char).Value = "C";
            cmdSelCadastroSplit.Parameters.Add("@COD_ID_VENDAS", SqlDbType.Int).Value = Funcoes.strToInt(ReaderCadastro["COD_ID"].ToString());
            cmdSelCadastroSplit.Parameters.Add("@COD_ID_PESSOA_LICENCIADO", SqlDbType.Int).Value = Funcoes.strToInt(ReaderCadastro["COD_ID_PESSOA_LICENCIADO"].ToString());
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




            string jsonPix = JsonConvert.SerializeObject(dtransacao);
            string jsonRetorno = zoop.transacao("I", "", jsonPix);
            JObject oRetorno = JObject.Parse(jsonRetorno);
            try
            {
                if ((oRetorno["status"].ToString().Trim() != "") && (oRetorno["id"].ToString().Trim() != ""))
                {
                    // Gravar dados atualizados na tabela VENDAS
                    string sStatusTransacao = VerificaStatus(oRetorno["status"].ToString().Trim());

                    SqlConnection connInsCons = new SqlConnection(Funcoes.conexao());
                    connInsCons.Open();
                    SqlCommand cmdInsCons = new SqlCommand("dbo.stp_vendas_vendas_ins", connInsCons);
                    cmdInsCons.CommandType = CommandType.StoredProcedure;
                    cmdInsCons.Parameters.Add("@flg_operacao", SqlDbType.Char).Value = 'I';
                    cmdInsCons.Parameters.Add("@COD_ID", SqlDbType.Int).Value = Funcoes.strToInt(sid_id.ToString());

                    cmdInsCons.Parameters.Add("@COD_ID_PESSOA_LICENCIADO", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["LICENCIADO"].ToString());
                    cmdInsCons.Parameters.Add("@COD_ID_PESSOAS_FJ_VENDEDOR", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["PESSOA"].ToString());
                    // Buscar dados comprador

                    cmdInsCons.Parameters.Add("@COD_ID_VENDAS", SqlDbType.Int).Value = Funcoes.strToInt(sid_id.ToString());

                    cmdInsCons.Parameters.Add("@NUM_VALOR", SqlDbType.Float).Value = vValorTotal;
                    if (ReaderCadastro["DTA_VENCIMENTO"].ToString().Trim() != "") { cmdInsCons.Parameters.Add("@DTA_VENCIMENTO", SqlDbType.DateTime).Value = Convert.ToDateTime(ReaderCadastro["DTA_VENCIMENTO"].ToString()); }

                    cmdInsCons.Parameters.Add("@FLG_PIX", SqlDbType.VarChar).Value = "S";


                    cmdInsCons.Parameters.Add("@NOM_CODE", SqlDbType.VarChar).Value = oRetorno["id"].ToString().Trim();
                    cmdInsCons.Parameters.Add("@NOM_CONTEUDO_CAMPO_01", SqlDbType.VarChar).Value = txtConteudo01.Text.ToString();

                    // Cliente
                    cmdInsCons.Parameters.Add("@NOM_NOME", SqlDbType.VarChar).Value = txtNome.Text.ToString();
                    cmdInsCons.Parameters.Add("@NOM_SOBRENOME", SqlDbType.VarChar).Value = txtSobrenome.Text.ToString();
                    cmdInsCons.Parameters.Add("@NUM_CNPJCPF", SqlDbType.VarChar).Value = txtDocumento.Text.ToString();
                    cmdInsCons.Parameters.Add("@NOM_EMAIL", SqlDbType.VarChar).Value = txtEmail.Text.ToString();
                    cmdInsCons.Parameters.Add("@NOM_CELULAR", SqlDbType.VarChar).Value = txtCelular.Text.ToString();

                    // Endereço
                    cmdInsCons.Parameters.Add("@NOM_ENDERECO", SqlDbType.VarChar).Value = txtEndereco.Text.ToString();
                    cmdInsCons.Parameters.Add("@NOM_NUMERO", SqlDbType.VarChar).Value = txtNumero.Text.ToString();
                    cmdInsCons.Parameters.Add("@NOM_COMPLEMENTO", SqlDbType.VarChar).Value = txtComplemento.Text.ToString();
                    cmdInsCons.Parameters.Add("@NOM_BAIRRO", SqlDbType.VarChar).Value = txtBairro.Text.ToString();
                    cmdInsCons.Parameters.Add("@NOM_CIDADE", SqlDbType.VarChar).Value = txtCidade.Text.ToString();
                    cmdInsCons.Parameters.Add("@NOM_UF", SqlDbType.VarChar).Value = ddlEstado.SelectedValue.ToString();
                    cmdInsCons.Parameters.Add("@NOM_CEP", SqlDbType.VarChar).Value = txtCEP.Text.ToString();
                    cmdInsCons.Parameters.Add("@NOM_PAIS", SqlDbType.VarChar).Value = "BR";

                    // Endereço Entrega
                    cmdInsCons.Parameters.Add("@FLG_ENDERECO_ENTREGA", SqlDbType.Char).Value = (ckbEnderecoEntrega.Checked == true) ? "S" : "N";

                    cmdInsCons.Parameters.Add("@NOM_ENDERECO_ENTREGA", SqlDbType.VarChar).Value = txtEnderecoEntrega.Text.ToString();
                    cmdInsCons.Parameters.Add("@NOM_NUMERO_ENTREGA", SqlDbType.VarChar).Value = txtNumeroEntrega.Text.ToString();
                    cmdInsCons.Parameters.Add("@NOM_COMPLEMENTO_ENTREGA", SqlDbType.VarChar).Value = txtComplementoEntrega.Text.ToString();
                    cmdInsCons.Parameters.Add("@NOM_BAIRRO_ENTREGA", SqlDbType.VarChar).Value = txtBairroEntrega.Text.ToString();
                    cmdInsCons.Parameters.Add("@NOM_CIDADE_ENTREGA", SqlDbType.VarChar).Value = txtCidadeEntrega.Text.ToString();
                    cmdInsCons.Parameters.Add("@NOM_UF_ENTREGA", SqlDbType.VarChar).Value = ddlEstadoEntrega.SelectedValue.ToString();
                    cmdInsCons.Parameters.Add("@NOM_CEP_ENTREGA", SqlDbType.VarChar).Value = txtCEPEntrega.Text.ToString();
                    cmdInsCons.Parameters.Add("@DES_JSON", SqlDbType.Text).Value = jsonRetorno.ToString();
                    cmdInsCons.Parameters.Add("@NOM_PIX", SqlDbType.VarChar).Value = oRetorno["payment_method"]["qr_code"]["emv"].ToString();

                    string URL = oRetorno["payment_method"]["qr_code"]["emv"].ToString();
                    QRCodeEncoder encoder = new QRCodeEncoder();

                    Bitmap image = encoder.Encode(URL);
                    string Codificacao = DateTime.Now.Year.ToString() + DateTime.Now.Month.ToString() + DateTime.Now.Day.ToString() + DateTime.Now.Hour.ToString() + DateTime.Now.Minute.ToString() + DateTime.Now.Second.ToString();
                    image.Save(Server.MapPath("public_html") + "\\" + HttpContext.Current.Session["LICENCIADO"].ToString() + "_" + Codificacao.ToString() + "_" + sid_id.ToString().PadLeft(10, '0') + ".bmp");
                    cmdInsCons.Parameters.Add("@NOM_IMAGEM_PIX", SqlDbType.VarChar).Value = HttpContext.Current.Session["LICENCIADO"].ToString() + "_" + Codificacao.ToString() + "_" + sid_id.ToString().PadLeft(10, '0') + ".bmp";

                    imgQRcode.Src = "../public_html/" + HttpContext.Current.Session["LICENCIADO"].ToString() + "_" + Codificacao.ToString() + "_" + sid_id.ToString().PadLeft(10, '0') + ".bmp";
                    txtPix.Text = URL.ToString();
                    if (ReaderCadastro["DTA_PIX_VENCIMENTO"].ToString().Trim() != "")
                    {
                        lblVencimentoPix.Text = Convert.ToDateTime(ReaderCadastro["DTA_PIX_VENCIMENTO"].ToString()).ToShortDateString();
                    }
                    else
                    {
                        lblVencimentoPix.Text = DateTime.Now.ToShortDateString();
                    }


                    cmdInsCons.ExecuteNonQuery();
                    connInsCons.Close();
                    connInsCons.Dispose();

                    txtStatus.Text = "Transação por PIX retornou status: ( (" + sStatusTransacao.ToString() + ")!";
                    txtTransacao.Text = oRetorno["id"].ToString();
                    Funcoes.GravaAuditoria(Funcoes.strToInt(sid_id), Request.ServerVariables["REMOTE_ADDR"].ToString(), "Checkout", "Pagamento Pix - Transação Criada: " + sStatusTransacao.ToString());
                }
            }
            catch
            {
                txtStatus.Text = zoop.status_code(oRetorno["error"]["status_code"].ToString());
                txtTransacao.Text = "Falha ao tentar realizar o pagamento";
                Funcoes.GravaAuditoria(Funcoes.strToInt(sid_id), Request.ServerVariables["REMOTE_ADDR"].ToString(), "Checkout", "Pagamento Pix: - Erro Transação: " + txtStatus.Text.ToString());
            }

            cbPagamento.Visible = false;
            cbConfirmacao.Visible = true;
            cbPix.Visible = true;

            btnConfirmar.Visible = false;
            btnVoltar.Visible = true;

        }
    }

    private void GerarTransacaoCredito(double vValorTotal)
    {
        // ******************************************************************************************************************************************************************
        // Operação de Crédito **********************************************************************************************************************************************
        // ******************************************************************************************************************************************************************

        if (dvCartaoCredito.Visible == true)
        {
            // Carrega os dados do Link de Pagamento

            SqlConnection mySelCadastro = new SqlConnection(Funcoes.conexao());
            mySelCadastro.Open();
            SqlCommand cmdSelCadastro = new SqlCommand("dbo.stp_vendas_ins", mySelCadastro);
            cmdSelCadastro.CommandType = CommandType.StoredProcedure;
            cmdSelCadastro.Parameters.Add("@flg_operacao", SqlDbType.Char).Value = "O";
            cmdSelCadastro.Parameters.Add("@COD_ID", SqlDbType.Int).Value = Funcoes.strToInt(sid_id.ToString());
            SqlDataReader ReaderCadastro = cmdSelCadastro.ExecuteReader();

            while (ReaderCadastro.Read())
            {
                DadosTransacao.TransacaoCard dtransacao = new DadosTransacao.TransacaoCard()
                {
                    on_behalf_of = ReaderCadastro["NUM_TOKEN"].ToString(),
                    description = ReaderCadastro["NOM_PRODUTO"].ToString(),
                    payment_type = "credit",
                    capture = true,
                    reference_id = ReaderCadastro["COD_ID"].ToString(),

                    source = new DadosTransacao.source()
                    {
                        usage = "single_use",
                        amount = Funcoes.strToInt(Convert.ToString(vValorTotal * 100).ToString()),
                        currency = "BRL",
                        type = "card",
                        card = new DadosTransacao.card()
                        {
                            card_number = txtNumeroCartao.Text.ToString(),
                            holder_name = txtNomeCartao.Text.ToString(),
                            expiration_month = ddlMesCartao.SelectedValue.ToString(),
                            expiration_year = ddlAnoCartao.SelectedValue.ToString(),
                            security_code = txtCVVCartao.Text.ToString()
                        }
                    },

                    installment_plan = new DadosTransacao.installment_plan
                    {
                        number_installments = Funcoes.strToInt(ddlParcelas.SelectedValue.ToString())
                    }
                };

                // Verifica a existência de split Manual
                SqlConnection mySelCadastroSplit = new SqlConnection(Funcoes.conexao());
                mySelCadastroSplit.Open();
                SqlCommand cmdSelCadastroSplit = new SqlCommand("dbo.stp_vendas_parceiros_ins", mySelCadastroSplit);
                cmdSelCadastroSplit.CommandType = CommandType.StoredProcedure;
                cmdSelCadastroSplit.Parameters.Add("@flg_operacao", SqlDbType.Char).Value = "C";
                cmdSelCadastroSplit.Parameters.Add("@COD_ID_VENDAS", SqlDbType.Int).Value = Funcoes.strToInt(ReaderCadastro["COD_ID"].ToString());
                cmdSelCadastroSplit.Parameters.Add("@COD_ID_PESSOA_LICENCIADO", SqlDbType.Int).Value = Funcoes.strToInt(ReaderCadastro["COD_ID_PESSOA_LICENCIADO"].ToString());
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


                string jsonCredito = JsonConvert.SerializeObject(dtransacao);

                string jsonRetorno = zoop.transacao("I", "", jsonCredito);
                
                JObject oRetorno = JObject.Parse(jsonRetorno);

                try
                {
                    if ((oRetorno["status"].ToString().Trim() != "") && (oRetorno["id"].ToString().Trim() != ""))
                    {
                        string sStatusTransacao = VerificaStatus(oRetorno["status"].ToString().Trim());
                        string sSalva = RetornaPosicaoStatus(oRetorno["status"].ToString().Trim());

                        if (sSalva.ToString().Trim() == "S")
                        {
                            SqlConnection connInsCons = new SqlConnection(Funcoes.conexao());
                            connInsCons.Open();
                            SqlCommand cmdInsCons = new SqlCommand("dbo.stp_vendas_vendas_ins", connInsCons);
                            cmdInsCons.CommandType = CommandType.StoredProcedure;
                            cmdInsCons.Parameters.Add("@flg_operacao", SqlDbType.Char).Value = 'I';
                            cmdInsCons.Parameters.Add("@COD_ID", SqlDbType.Int).Value = Funcoes.strToInt(sid_id.ToString());
                            cmdInsCons.Parameters.Add("@COD_ID_PESSOA_LICENCIADO", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["LICENCIADO"].ToString());
                            cmdInsCons.Parameters.Add("@COD_ID_PESSOAS_FJ_VENDEDOR", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["PESSOA"].ToString());
                            // Buscar dados comprador

                            cmdInsCons.Parameters.Add("@COD_ID_VENDAS", SqlDbType.Int).Value = Funcoes.strToInt(sid_id.ToString());

                            cmdInsCons.Parameters.Add("@NUM_VALOR", SqlDbType.Float).Value = vValorTotal;
                            if (ReaderCadastro["DTA_VENCIMENTO"].ToString().Trim() != "") { cmdInsCons.Parameters.Add("@DTA_VENCIMENTO", SqlDbType.DateTime).Value = Convert.ToDateTime(ReaderCadastro["DTA_VENCIMENTO"].ToString()); }

                            cmdInsCons.Parameters.Add("@NUM_CARTAO", SqlDbType.VarChar).Value = txtNumeroCartao.Text.ToString();
                            cmdInsCons.Parameters.Add("@NOM_CARTAO", SqlDbType.VarChar).Value = txtNomeCartao.Text.ToString();
                            cmdInsCons.Parameters.Add("@NUM_MES_CARTAO", SqlDbType.Int).Value = Funcoes.strToInt(ddlMesCartao.SelectedValue.ToString());
                            cmdInsCons.Parameters.Add("@NUM_ANO_CARTAO", SqlDbType.Int).Value = Funcoes.strToInt(ddlAnoCartao.SelectedValue.ToString());
                            cmdInsCons.Parameters.Add("@NUM_CODIGO_CARTAO", SqlDbType.Int).Value = Funcoes.strToInt(txtCVVCartao.Text.ToString());
                            cmdInsCons.Parameters.Add("@NUM_PARCELAS", SqlDbType.Int).Value = Funcoes.strToInt(ddlParcelas.SelectedValue.ToString());

                            cmdInsCons.Parameters.Add("@FLG_CARTAO_CREDITO", SqlDbType.VarChar).Value = "S";


                            cmdInsCons.Parameters.Add("@NOM_CODE", SqlDbType.VarChar).Value = oRetorno["id"].ToString().Trim();
                            cmdInsCons.Parameters.Add("@NOM_CONTEUDO_CAMPO_01", SqlDbType.VarChar).Value = txtConteudo01.Text.ToString();

                            // Cliente
                            cmdInsCons.Parameters.Add("@NOM_NOME", SqlDbType.VarChar).Value = txtNome.Text.ToString();
                            cmdInsCons.Parameters.Add("@NOM_SOBRENOME", SqlDbType.VarChar).Value = txtSobrenome.Text.ToString();
                            cmdInsCons.Parameters.Add("@NUM_CNPJCPF", SqlDbType.VarChar).Value = txtDocumento.Text.ToString();
                            cmdInsCons.Parameters.Add("@NOM_EMAIL", SqlDbType.VarChar).Value = txtEmail.Text.ToString();
                            cmdInsCons.Parameters.Add("@NOM_CELULAR", SqlDbType.VarChar).Value = txtCelular.Text.ToString();

                            // Endereço
                            cmdInsCons.Parameters.Add("@NOM_ENDERECO", SqlDbType.VarChar).Value = txtEndereco.Text.ToString();
                            cmdInsCons.Parameters.Add("@NOM_NUMERO", SqlDbType.VarChar).Value = txtNumero.Text.ToString();
                            cmdInsCons.Parameters.Add("@NOM_COMPLEMENTO", SqlDbType.VarChar).Value = txtComplemento.Text.ToString();
                            cmdInsCons.Parameters.Add("@NOM_BAIRRO", SqlDbType.VarChar).Value = txtBairro.Text.ToString();
                            cmdInsCons.Parameters.Add("@NOM_CIDADE", SqlDbType.VarChar).Value = txtCidade.Text.ToString();
                            cmdInsCons.Parameters.Add("@NOM_UF", SqlDbType.VarChar).Value = ddlEstado.SelectedValue.ToString();
                            cmdInsCons.Parameters.Add("@NOM_CEP", SqlDbType.VarChar).Value = txtCEP.Text.ToString();
                            cmdInsCons.Parameters.Add("@NOM_PAIS", SqlDbType.VarChar).Value = "BR";

                            // Endereço Entrega
                            cmdInsCons.Parameters.Add("@FLG_ENDERECO_ENTREGA", SqlDbType.Char).Value = (ckbEnderecoEntrega.Checked == true) ? "S" : "N";

                            cmdInsCons.Parameters.Add("@NOM_ENDERECO_ENTREGA", SqlDbType.VarChar).Value = txtEnderecoEntrega.Text.ToString();
                            cmdInsCons.Parameters.Add("@NOM_NUMERO_ENTREGA", SqlDbType.VarChar).Value = txtNumeroEntrega.Text.ToString();
                            cmdInsCons.Parameters.Add("@NOM_COMPLEMENTO_ENTREGA", SqlDbType.VarChar).Value = txtComplementoEntrega.Text.ToString();
                            cmdInsCons.Parameters.Add("@NOM_BAIRRO_ENTREGA", SqlDbType.VarChar).Value = txtBairroEntrega.Text.ToString();
                            cmdInsCons.Parameters.Add("@NOM_CIDADE_ENTREGA", SqlDbType.VarChar).Value = txtCidadeEntrega.Text.ToString();
                            cmdInsCons.Parameters.Add("@NOM_UF_ENTREGA", SqlDbType.VarChar).Value = ddlEstadoEntrega.SelectedValue.ToString();
                            cmdInsCons.Parameters.Add("@NOM_CEP_ENTREGA", SqlDbType.VarChar).Value = txtCEPEntrega.Text.ToString();

                            cmdInsCons.Parameters.Add("@DES_JSON", SqlDbType.Text).Value = jsonRetorno.ToString();

                            cmdInsCons.ExecuteNonQuery();
                            connInsCons.Close();
                            connInsCons.Dispose();
                        }
                        txtStatus.Text = "Transação por Cartão de Crédito retornou status: (" + sStatusTransacao.ToString() + ")!";
                        txtTransacao.Text = oRetorno["id"].ToString();
                        Funcoes.GravaAuditoria(Funcoes.strToInt(sid_id), Request.ServerVariables["REMOTE_ADDR"].ToString(), "Checkout", "Pagamento Cartão - Transação Criada: " + sStatusTransacao.ToString());
                    }
                }
                catch
                {
                        txtStatus.Text = zoop.status_code(oRetorno["error"]["status_code"].ToString());
                        txtTransacao.Text = "Falha ao tentar realizar o pagamento";
                        Funcoes.GravaAuditoria(Funcoes.strToInt(sid_id), Request.ServerVariables["REMOTE_ADDR"].ToString(), "Checkout", "Pagamento Cartão - Erro Transação: " + txtStatus.Text.ToString());
                }



                cbPagamento.Visible = false;
                cbConfirmacao.Visible = true;
 
                btnConfirmar.Visible = false;
                btnVoltar.Visible = true;
            }
        }

    }

    public double VerificaValorTotal()
    {
        double vValorTotal = 0;

        SqlConnection mySelVenda = new SqlConnection(Funcoes.conexao());
        mySelVenda.Open();
        SqlCommand cmdSelVenda = new SqlCommand("dbo.stp_vendas_ins", mySelVenda);
        cmdSelVenda.CommandType = CommandType.StoredProcedure;
        cmdSelVenda.Parameters.Add("@flg_operacao", SqlDbType.Char).Value = "O";
        cmdSelVenda.Parameters.Add("@COD_ID", SqlDbType.Int).Value = Funcoes.strToInt(sid_id.ToString());
        SqlDataReader ReaderVenda = cmdSelVenda.ExecuteReader();

        while (ReaderVenda.Read())
        {
            if (ReaderVenda["FLG_PRECO_PARCELAMENTO"].ToString() == "S")
            {
                if ((FPBoleto.Checked == true) || (FPPix.Checked == true)) { vValorTotal = Funcoes.strToDouble(ReaderVenda["NUM_VALOR_01"].ToString()); }

                if ((FPCredito.Checked == true))
                {
                    if (Funcoes.strToInt(ddlParcelas.SelectedValue.ToString()) == 1) { vValorTotal = Funcoes.strToDouble(ReaderVenda["NUM_VALOR_01"].ToString()); }
                    if (Funcoes.strToInt(ddlParcelas.SelectedValue.ToString()) == 2) { vValorTotal = Funcoes.strToDouble(ReaderVenda["NUM_VALOR_02"].ToString()); }
                    if (Funcoes.strToInt(ddlParcelas.SelectedValue.ToString()) == 3) { vValorTotal = Funcoes.strToDouble(ReaderVenda["NUM_VALOR_03"].ToString()); }
                    if (Funcoes.strToInt(ddlParcelas.SelectedValue.ToString()) == 4) { vValorTotal = Funcoes.strToDouble(ReaderVenda["NUM_VALOR_04"].ToString()); }
                    if (Funcoes.strToInt(ddlParcelas.SelectedValue.ToString()) == 5) { vValorTotal = Funcoes.strToDouble(ReaderVenda["NUM_VALOR_05"].ToString()); }
                    if (Funcoes.strToInt(ddlParcelas.SelectedValue.ToString()) == 6) { vValorTotal = Funcoes.strToDouble(ReaderVenda["NUM_VALOR_06"].ToString()); }
                    if (Funcoes.strToInt(ddlParcelas.SelectedValue.ToString()) == 7) { vValorTotal = Funcoes.strToDouble(ReaderVenda["NUM_VALOR_07"].ToString()); }
                    if (Funcoes.strToInt(ddlParcelas.SelectedValue.ToString()) == 8) { vValorTotal = Funcoes.strToDouble(ReaderVenda["NUM_VALOR_08"].ToString()); }
                    if (Funcoes.strToInt(ddlParcelas.SelectedValue.ToString()) == 9) { vValorTotal = Funcoes.strToDouble(ReaderVenda["NUM_VALOR_09"].ToString()); }
                    if (Funcoes.strToInt(ddlParcelas.SelectedValue.ToString()) == 10) { vValorTotal = Funcoes.strToDouble(ReaderVenda["NUM_VALOR_10"].ToString()); }
                    if (Funcoes.strToInt(ddlParcelas.SelectedValue.ToString()) == 11) { vValorTotal = Funcoes.strToDouble(ReaderVenda["NUM_VALOR_11"].ToString()); }
                    if (Funcoes.strToInt(ddlParcelas.SelectedValue.ToString()) == 12) { vValorTotal = Funcoes.strToDouble(ReaderVenda["NUM_VALOR_12"].ToString()); }
                    if (Funcoes.strToInt(ddlParcelas.SelectedValue.ToString()) == 13) { vValorTotal = Funcoes.strToDouble(ReaderVenda["NUM_VALOR_13"].ToString()); }
                    if (Funcoes.strToInt(ddlParcelas.SelectedValue.ToString()) == 14) { vValorTotal = Funcoes.strToDouble(ReaderVenda["NUM_VALOR_14"].ToString()); }
                    if (Funcoes.strToInt(ddlParcelas.SelectedValue.ToString()) == 15) { vValorTotal = Funcoes.strToDouble(ReaderVenda["NUM_VALOR_15"].ToString()); }
                    if (Funcoes.strToInt(ddlParcelas.SelectedValue.ToString()) == 16) { vValorTotal = Funcoes.strToDouble(ReaderVenda["NUM_VALOR_16"].ToString()); }
                    if (Funcoes.strToInt(ddlParcelas.SelectedValue.ToString()) == 17) { vValorTotal = Funcoes.strToDouble(ReaderVenda["NUM_VALOR_17"].ToString()); }
                    if (Funcoes.strToInt(ddlParcelas.SelectedValue.ToString()) == 18) { vValorTotal = Funcoes.strToDouble(ReaderVenda["NUM_VALOR_18"].ToString()); }
                    if (Funcoes.strToInt(ddlParcelas.SelectedValue.ToString()) == 19) { vValorTotal = Funcoes.strToDouble(ReaderVenda["NUM_VALOR_19"].ToString()); }
                    if (Funcoes.strToInt(ddlParcelas.SelectedValue.ToString()) == 20) { vValorTotal = Funcoes.strToDouble(ReaderVenda["NUM_VALOR_20"].ToString()); }
                    if (Funcoes.strToInt(ddlParcelas.SelectedValue.ToString()) == 21) { vValorTotal = Funcoes.strToDouble(ReaderVenda["NUM_VALOR_21"].ToString()); }
                }
            }
            else
            {
                vValorTotal = Funcoes.strToDouble(ReaderVenda["NUM_VALOR"].ToString());
            }
        }
        return vValorTotal;
    }

    public string VerificaStatus(string sStatus)
    {
        string sStatusTransacao = "";
        switch (sStatus.ToString())
        {
            case "succeeded":
                sStatusTransacao = "Sucesso";
                break;
            case "failed":
                sStatusTransacao = "Falhou";
                break;
            case "canceled":
                sStatusTransacao = "Cancelada";
                break;
            case "pre_authorized":
                sStatusTransacao = "Pré-autorizada";
                break;
            case "reversed":
                sStatusTransacao = "Revertida";
                break;
            case "refunded":
                sStatusTransacao = "Reembolsada";
                break;
            case "pending":
                sStatusTransacao = "Pendente";
                break;
            case "new":
                sStatusTransacao = "Nova";
                break;
            case "partial_refunded":
                sStatusTransacao = "Reembolsada Parcialmente";
                break;
            case "dispute":
                sStatusTransacao = "Disputa";
                break;
            case "charged_back":
                sStatusTransacao = "Devolvida";
                break;
            default:
                sStatusTransacao = "Outros";
                break;
        }
        return sStatusTransacao.ToString();
    }

    public string RetornaPosicaoStatus(string sStatus)
    {
        string sStatusTransacao = "";
        switch (sStatus.ToString())
        {
            case "succeeded":
                sStatusTransacao = "S";
                break;
            case "failed":
                sStatusTransacao = "N";
                break;
            case "canceled":
                sStatusTransacao = "N";
                break;
            case "pre_authorized":
                sStatusTransacao = "S";
                break;
            case "reversed":
                sStatusTransacao = "S";
                break;
            case "refunded":
                sStatusTransacao = "S";
                break;
            case "pending":
                sStatusTransacao = "S";
                break;
            case "new":
                sStatusTransacao = "S";
                break;
            case "partial_refunded":
                sStatusTransacao = "S";
                break;
            case "dispute":
                sStatusTransacao = "S";
                break;
            case "charged_back":
                sStatusTransacao = "N";
                break;
            default:
                sStatusTransacao = "S";
                break;
        }
        return sStatusTransacao.ToString();
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
            cmdInsConsCliente.Parameters.Add("@NUM_CNPJCPF", SqlDbType.VarChar).Value = Funcoes.TIRAACENTOSDOCUMENTOS(txtDocumento.Text.ToString());
            cmdInsConsCliente.Parameters.Add("@NOM_EMAIL", SqlDbType.VarChar).Value = txtEmail.Text.ToString();

            cmdInsConsCliente.ExecuteNonQuery();
            connInsConsCliente.Close();
            connInsConsCliente.Dispose();
    }

    public static string GerarCobranca(string sIDCliente, string sValor, string sVencimento, string sDias, string sDescricao, string sReferencia, string sTipo, string sID)
    {
        SqlConnection mySelCadastro = new SqlConnection(Funcoes.conexao());
        mySelCadastro.Open();
        SqlCommand cmdSelCadastro = new SqlCommand("dbo.stp_vendas_ins", mySelCadastro);
        cmdSelCadastro.CommandType = CommandType.StoredProcedure;
        cmdSelCadastro.Parameters.Add("@flg_operacao", SqlDbType.Char).Value = "O";
        cmdSelCadastro.Parameters.Add("@COD_ID", SqlDbType.Int).Value = Funcoes.strToInt(sID.ToString());
        SqlDataReader ReaderCadastro = cmdSelCadastro.ExecuteReader();

        while (ReaderCadastro.Read())
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


            SqlConnection mySelCadastroSplit = new SqlConnection(Funcoes.conexao());
            mySelCadastroSplit.Open();
            SqlCommand cmdSelCadastroSplit = new SqlCommand("dbo.stp_pessoas_fj_split_ins", mySelCadastroSplit);
            cmdSelCadastroSplit.CommandType = CommandType.StoredProcedure;
            cmdSelCadastroSplit.Parameters.Add("@flg_operacao", SqlDbType.Char).Value = "C";
            cmdSelCadastroSplit.Parameters.Add("@COD_ID_PESSOAS_FJ", SqlDbType.Int).Value = Funcoes.strToInt(ReaderCadastro["COD_ID_PESSOAS_FJ_VENDEDOR"].ToString());
            cmdSelCadastroSplit.Parameters.Add("@COD_ID_PESSOA_LICENCIADO", SqlDbType.Int).Value = Funcoes.strToInt(ReaderCadastro["COD_ID_PESSOA_LICENCIADO"].ToString());
            SqlDataReader ReaderCadastroSplit = cmdSelCadastroSplit.ExecuteReader();

            List<DadosCobrancaAsaaS.split> listaSplits = new List<DadosCobrancaAsaaS.split>();
            int iContadorSplit = 0;
            while (ReaderCadastroSplit.Read()) // Percorre os registros do banco
            {
                iContadorSplit = iContadorSplit + 1;
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
            if (iContadorSplit > 0) { dcobranca.split = listaSplits.ToArray(); }

            string jsonCobranca = JsonConvert.SerializeObject(dcobranca);
            string jsonURLCobranca = asaas.CriarCobranca(asaas.PegarTokenSubconta(Funcoes.strToInt(HttpContext.Current.Session["PESSOA"].ToString()), Funcoes.strToInt(HttpContext.Current.Session["LICENCIADO"].ToString())), jsonCobranca);

            return jsonURLCobranca.ToString();
        }

        return "";


    }

}