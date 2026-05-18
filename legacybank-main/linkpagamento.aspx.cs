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


public partial class linkpagamento : System.Web.UI.Page
{
    public string sid_id
    {
        get
        {
            try { return Funcoes.Decrypt(Request["id"].ToString()); }
            catch { return ""; }
        }
    }

    public string stipo_tipo
    {
        get
        {
            try { return Funcoes.Decrypt(Request["tipo"].ToString()); }
            catch { return ""; }
        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {

        string urlorigem = Request.ServerVariables["SERVER_NAME"].ToString();

        SqlConnection mySelCadastro = new SqlConnection(Funcoes.conexao());
        mySelCadastro.Open();
        SqlCommand cmdSelCadastro = new SqlCommand("dbo.stp_pessoas_fj_ins", mySelCadastro);
        cmdSelCadastro.CommandType = CommandType.StoredProcedure;
        cmdSelCadastro.Parameters.Add("@flg_operacao", SqlDbType.Char).Value = "T";
        cmdSelCadastro.Parameters.Add("@NOM_URL", SqlDbType.VarChar).Value = urlorigem.ToString();
        SqlDataReader ReaderCadastro = cmdSelCadastro.ExecuteReader();
        while (ReaderCadastro.Read())
        {
            HttpContext.Current.Session.Add("LICENCIADO", ReaderCadastro["COD_ID_PESSOA_LICENCIADO"].ToString());

            imgLogoPrincipal.Src = "public_html/" + ReaderCadastro["NOM_LOGO"].ToString();

            if (stipo_tipo.ToString().Trim()=="B")
            {
                Page.Title = "Cobrança - Pagamento em Boleto Bancário";
                imgLogoPrincipal.Src = "public_html/" + ReaderCadastro["NOM_LOGO"].ToString();
            }
            if (stipo_tipo.ToString().Trim()=="Q")
            {
                Page.Title = "Cobrança - Pagamento em QRCode";
                imgLogoPrincipal01.Src = "public_html/" + ReaderCadastro["NOM_LOGO"].ToString();
            }

            //Page.Header.Title = ReaderCadastro["NOM_RAZAOSOCIAL"].ToString();
            //FavIconLink.Text = "<link rel='icon' type='image/x-icon' href='" + "public_html/" + ReaderCadastro["NOM_FAVICON"].ToString() + "'>";

            // 2FA
            HttpContext.Current.Session.Add("AUTENTICACAO2FA", ReaderCadastro["FLG_2FA"].ToString());

            // Pegar URL Origem

            HttpContext.Current.Session.Add("URLORIGEM", urlorigem.ToString());

            // Pegar dados do Logotipo

            HttpContext.Current.Session.Add("LOGOPADRAO", ReaderCadastro["NOM_LOGO"].ToString());

            // Pegar os dados ZOOP

            HttpContext.Current.Session.Add("ZOOPINTEGRACAO", ReaderCadastro["FLG_INTEGRACAO_ZOOP"].ToString());
            HttpContext.Current.Session.Add("ZOOPTERCEIROS", ReaderCadastro["FLG_TERCEIROS_ZOOP"].ToString());

            if (ReaderCadastro["NOM_ID_MARKETPLACE"].ToString().Trim() != "")
            { HttpContext.Current.Session.Add("IDMARKETPLACE", ReaderCadastro["NOM_ID_MARKETPLACE"].ToString()); }
            else { HttpContext.Current.Session.Add("IDMARKETPLACE", ConfigurationManager.AppSettings["idzoop"].ToString()); }

            if (ReaderCadastro["NOM_KEY_MARKETPLACE"].ToString().Trim() != "")
            { HttpContext.Current.Session.Add("USERNAMEMARKETPLACE", ReaderCadastro["NOM_KEY_MARKETPLACE"].ToString()); }
            else { HttpContext.Current.Session.Add("USERNAMEMARKETPLACE", ConfigurationManager.AppSettings["keyzoop"].ToString()); }

            // Pegar os dados E-MAIL

            // Dados E-mail Padrao
            HttpContext.Current.Session.Add("EMAILHOST", ReaderCadastro["NOM_HOST_EMAIL_PADRAO"].ToString());
            HttpContext.Current.Session.Add("EMAIL", ReaderCadastro["NOM_EMAIL_PADRAO"].ToString());
            HttpContext.Current.Session.Add("EMAILSENHA", ReaderCadastro["NOM_SENHA_EMAIL_PADRAO"].ToString());
            HttpContext.Current.Session.Add("EMAILPORTA", ReaderCadastro["NUM_PORTA_EMAIL_PADRAO"].ToString());
            HttpContext.Current.Session.Add("EMAILSSL", ReaderCadastro["FLG_SSL_EMAIL_PADRAO"].ToString());
            HttpContext.Current.Session.Add("EMAILRODAPE", ReaderCadastro["NOM_RODAPE_EMAIL"].ToString());
            HttpContext.Current.Session.Add("EMAILLOGOTIPO", ReaderCadastro["NOM_LOGOTIPO_EMAIL"].ToString());

            // Dados dos Textos de Aceite
            HttpContext.Current.Session.Add("TERMOSCONDICOS", ReaderCadastro["NOM_TERMOS_CONDICOES_USO"].ToString());
            HttpContext.Current.Session.Add("POLITICAPRIVACIDADE", ReaderCadastro["NOM_POLITICA_PRIVACIDADE"].ToString());

            // CHAT DE SUPORTE
            HttpContext.Current.Session.Add("CHAT", ReaderCadastro["FLG_CHAT"].ToString());
        }


        if (!IsPostBack)
        {
            if ((stipo_tipo.ToString().Trim() == "LB") || (stipo_tipo.ToString().Trim() == "LP"))
            {
                SqlConnection mySelCadastroCobranca = new SqlConnection(Funcoes.conexao());
                mySelCadastroCobranca.Open();
                SqlCommand cmdSelCadastroCobranca = new SqlCommand("dbo.stp_vendas_vendas_ins", mySelCadastroCobranca);
                cmdSelCadastroCobranca.CommandType = CommandType.StoredProcedure;
                cmdSelCadastroCobranca.Parameters.Add("@flg_operacao", SqlDbType.Char).Value = "D";
                cmdSelCadastroCobranca.Parameters.Add("@NOM_CODE", SqlDbType.VarChar).Value = sid_id.ToString();
                SqlDataReader ReaderCadastroCobranca = cmdSelCadastroCobranca.ExecuteReader();
                while (ReaderCadastroCobranca.Read())
                {
                    

                    imgLogoPrincipal.Src = "public_html/" + ReaderCadastroCobranca["NOM_LOGOTIPO_CHECKOUT"].ToString();

                    if (stipo_tipo.ToString().Trim() == "LB")
                    {
                        Page.Title = "Cobrança - Pagamento em Boleto Bancário";
                        ConsultaBoletoCobrancaLinkPagamento(sid_id.ToString());
                        frmBoleto.Visible = true;

                    }
                    if (stipo_tipo.ToString().Trim() == "LP")
                    {
                        Page.Title = "Cobrança - Pagamento em QRCode";
                        ConsultaQRCodeCobrancaLinkPagamento(sid_id.ToString());
                        frmQRCode.Visible = true;
                    }
                }

            }
            else
            {
                SqlConnection mySelCadastroCobranca = new SqlConnection(Funcoes.conexao());
                mySelCadastroCobranca.Open();
                SqlCommand cmdSelCadastroCobranca = new SqlCommand("dbo.stp_cobrancas_ins", mySelCadastroCobranca);
                cmdSelCadastroCobranca.CommandType = CommandType.StoredProcedure;
                cmdSelCadastroCobranca.Parameters.Add("@flg_operacao", SqlDbType.Char).Value = "O";
                cmdSelCadastroCobranca.Parameters.Add("@COD_ID_COBRANCA", SqlDbType.VarChar).Value = sid_id.ToString();
                SqlDataReader ReaderCadastroCobranca = cmdSelCadastroCobranca.ExecuteReader();
                while (ReaderCadastroCobranca.Read())
                {
                    imgLogoPrincipal.Src = "public_html/" + ReaderCadastroCobranca["NOM_LOGOTIPO_CHECKOUT"].ToString();

                    if (stipo_tipo.ToString().Trim() == "B")
                    {
                        Page.Title = "Cobrança - Pagamento em Boleto Bancário";
                        ConsultaBoletoCobranca(sid_id.ToString());
                        frmBoleto.Visible = true;

                    }
                    if (stipo_tipo.ToString().Trim() == "Q")
                    {
                        Page.Title = "Cobrança - Pagamento em QRCode";
                        ConsultaQRCodeCobranca(sid_id.ToString());
                        frmQRCode.Visible = true;
                    }
                }

            }
            
        }
    }

    private void ConsultaQRCodeCobranca(string sID)
    {
        SqlConnection mySelCadastro = new SqlConnection(Funcoes.conexao());
        mySelCadastro.Open();
        SqlCommand cmdSelCadastro = new SqlCommand("dbo.stp_cobrancas_ins", mySelCadastro);
        cmdSelCadastro.CommandType = CommandType.StoredProcedure;
        cmdSelCadastro.Parameters.Add("@flg_operacao", SqlDbType.Char).Value = "L";
        cmdSelCadastro.Parameters.Add("@COD_ID_COBRANCA", SqlDbType.VarChar).Value = sID.ToString();
        SqlDataReader ReaderCadastro = cmdSelCadastro.ExecuteReader();
        while (ReaderCadastro.Read())
        {
            string jsonQRCodeCobranca = asaas.ObterQRCodeCobranca(asaas.PegarTokenSubconta(Funcoes.strToInt(ReaderCadastro["COD_ID_PESSOAS_FJ_VENDEDOR"].ToString()), Funcoes.strToInt(ReaderCadastro["COD_ID_PESSOA_LICENCIADO"].ToString())), sid_id.ToString());

            if (jsonQRCodeCobranca.ToString().Trim() != "")
            {
                try
                {
                    lblComprador01.Text = "Olá, " + ReaderCadastro["NOM_NOME"].ToString();
                    lblMensagemEntrada01.Text = "Aqui está o seu QRCode";

                    
                    JObject oQRCodeCobranca = JObject.Parse(jsonQRCodeCobranca.ToString());
                    lblPixCopiaCola01.Text = oQRCodeCobranca["payload"].ToString();
                    string base64String = oQRCodeCobranca["encodedImage"].ToString(); //encodedImage
                    QRCodeEncoder encoder = new QRCodeEncoder();
                    Bitmap image = encoder.Encode(oQRCodeCobranca["payload"].ToString());
                    image.Save(Server.MapPath("public_html") + "\\" + sid_id.ToString() + ".bmp");
                    imgQRcode01.Src = "../public_html/" + sid_id.ToString() + ".bmp";
                }
                catch
                {

                }

            }

            //string jsonDadosConta = asaas.
        }
    }

    private void ConsultaBoletoCobranca(string sID)
    {
        SqlConnection mySelCadastro = new SqlConnection(Funcoes.conexao());
        mySelCadastro.Open();
        SqlCommand cmdSelCadastro = new SqlCommand("dbo.stp_cobrancas_ins", mySelCadastro);
        cmdSelCadastro.CommandType = CommandType.StoredProcedure;
        cmdSelCadastro.Parameters.Add("@flg_operacao", SqlDbType.Char).Value = "L";
        cmdSelCadastro.Parameters.Add("@COD_ID_COBRANCA", SqlDbType.VarChar).Value = sID.ToString();
        SqlDataReader ReaderCadastro = cmdSelCadastro.ExecuteReader();
        while (ReaderCadastro.Read())
        {
            if (ReaderCadastro["FLG_CANCELADO"].ToString() == "S")
            {
                dvCancelado.Visible = true;
            }
            else
            {
                dvCancelado.Visible = false;
            }

            string jsonLinhaDigitavel = asaas.ObterLinhaDigitavelCobranca(asaas.PegarTokenSubconta(Funcoes.strToInt(ReaderCadastro["COD_ID_PESSOAS_FJ_VENDEDOR"].ToString()), Funcoes.strToInt(ReaderCadastro["COD_ID_PESSOA_LICENCIADO"].ToString())), sid_id.ToString());

            if (jsonLinhaDigitavel.ToString().Trim() != "")
            {
                try
                {
                    JObject oLinhaDigitavel = JObject.Parse(jsonLinhaDigitavel.ToString());

                    lblNossoNumeroCompensacao.Text = oLinhaDigitavel["nossoNumero"].ToString();
                    lblNossoNumeroPagador.Text = oLinhaDigitavel["nossoNumero"].ToString();

                    lblLinhaDigitavelCompensacao.Text = oLinhaDigitavel["identificationField"].ToString();
                    lblLinhaDigitavelPagador.Text = oLinhaDigitavel["identificationField"].ToString();
                    lblLinhaDigitavel.Text = oLinhaDigitavel["identificationField"].ToString();

                    lblCodigoBarrasCompensacao.Text = oLinhaDigitavel["barCode"].ToString();

                    lblValorDocumentoCompensacao.Text = String.Format("{0:c2}", Funcoes.strToDouble(ReaderCadastro["NUM_VALOR"].ToString()));
                    lblValorDocumentoPagador.Text = String.Format("{0:c2}", Funcoes.strToDouble(ReaderCadastro["NUM_VALOR"].ToString()));

                    lblDataDocumentoCompensacao.Text = String.Format("{0:dd/MM/yyyy}", Convert.ToDateTime(ReaderCadastro["DTA_DATA"].ToString()));
                    lblDataDocumentoPagador.Text = String.Format("{0:dd/MM/yyyy}", Convert.ToDateTime(ReaderCadastro["DTA_DATA"].ToString()));
                    lblDataProcessamentoCompensacao.Text = String.Format("{0:dd/MM/yyyy}", Convert.ToDateTime(ReaderCadastro["DTA_DATA"].ToString()));
                    lblDataProcessamentoPagador.Text = String.Format("{0:dd/MM/yyyy}", Convert.ToDateTime(ReaderCadastro["DTA_DATA"].ToString()));

                    lblBeneficiarioPagador.Text = ReaderCadastro["NOM_VENDEDOR"].ToString();
                    lblDocumentoBeneficiarioPagador.Text = ReaderCadastro["NUM_DOCUMENTO_VENDEDOR"].ToString();
                    lblBeneficiarioCompensacao.Text = ReaderCadastro["NOM_VENDEDOR"].ToString();
                    lblDocumentoBeneficiarioCompensacao.Text = ReaderCadastro["NUM_DOCUMENTO_VENDEDOR"].ToString();

                    lblVencimentoCompensacao.Text = String.Format("{0:dd/MM/yyyy}", Convert.ToDateTime(ReaderCadastro["DTA_VENCIMENTO"].ToString()));
                    lblVencimentoPagador.Text = String.Format("{0:dd/MM/yyyy}", Convert.ToDateTime(ReaderCadastro["DTA_VENCIMENTO"].ToString()));

                    lblNomeCompradorCompensacao.Text = ReaderCadastro["NOM_NOME"].ToString();
                    lblDocumentoCompradorCompensacao.Text = ReaderCadastro["NUM_CNPJCPF"].ToString();
                    lblNomeCompradorPagador.Text = ReaderCadastro["NOM_NOME"].ToString();
                    lblDocumentoCompradorPagador.Text = ReaderCadastro["NUM_CNPJCPF"].ToString();

                    lblComprador.Text = "Olá, " + ReaderCadastro["NOM_NOME"].ToString();
                    lblMensagemEntrada.Text = "Aqui está o seu boleto bancário";

                    lblNumeroDocumentoCompensacao.Text = ReaderCadastro["COD_ID"].ToString().PadLeft(10, '0');   //ReaderCadastro["NOM_REFERENCIA"].ToString();
                    lblNumeroDocumentoPagador.Text = ReaderCadastro["COD_ID"].ToString().PadLeft(10, '0'); //ReaderCadastro["NOM_REFERENCIA"].ToString();

                    lblContaPagador.Text = ReaderCadastro["NUM_AGENCIA_BAAS"].ToString().PadLeft(3, '0') + "/" + ReaderCadastro["NUM_CONTA_BAAS"].ToString() + "-" + ReaderCadastro["NUM_DIGITO_CONTA_BAAS"].ToString();
                    lblContaCompensacao.Text = ReaderCadastro["NUM_AGENCIA_BAAS"].ToString().PadLeft(3, '0') + "/" + ReaderCadastro["NUM_CONTA_BAAS"].ToString() + "-" + ReaderCadastro["NUM_DIGITO_CONTA_BAAS"].ToString();


                    lblInstrucoesCompensacao.Text = "Nao receber com cheque. <br />" + ReaderCadastro["DES_DESCRICAO"].ToString();
                    lblInstrucoesPagador.Text = "Nao receber com cheque. <br />" + ReaderCadastro["DES_DESCRICAO"].ToString() + "<br />" + oLinhaDigitavel["barCode"].ToString();
                }
                catch
                {
                }

            }
            string jsonQRCodeCobranca = asaas.ObterQRCodeCobranca(asaas.PegarTokenSubconta(Funcoes.strToInt(ReaderCadastro["COD_ID_PESSOAS_FJ_VENDEDOR"].ToString()), Funcoes.strToInt(ReaderCadastro["COD_ID_PESSOA_LICENCIADO"].ToString())), sid_id.ToString());

            if (jsonQRCodeCobranca.ToString().Trim() != "")
            {
                try
                {
                    JObject oQRCodeCobranca = JObject.Parse(jsonQRCodeCobranca.ToString());

                    lblPixCopiaCola.Text = oQRCodeCobranca["payload"].ToString();

                    string base64String = oQRCodeCobranca["encodedImage"].ToString(); //encodedImage

                    QRCodeEncoder encoder = new QRCodeEncoder();

                    Bitmap image = encoder.Encode(oQRCodeCobranca["payload"].ToString());
                    image.Save(Server.MapPath("public_html") + "\\" + sid_id.ToString() + ".bmp");

                    imgQRcode.Src = "../public_html/" + sid_id.ToString() + ".bmp";
                    imgQRcodeBoleto.Src = "../public_html/" + sid_id.ToString() + ".bmp";
                }
                catch
                {

                }

            }

            //string jsonDadosConta = asaas.
        }
    }


    private void ConsultaBoletoCobrancaLinkPagamento(string sID)
    {
        SqlConnection mySelCadastro = new SqlConnection(Funcoes.conexao());
        mySelCadastro.Open();
        SqlCommand cmdSelCadastro = new SqlCommand("dbo.stp_vendas_vendas_ins", mySelCadastro);
        cmdSelCadastro.CommandType = CommandType.StoredProcedure;
        cmdSelCadastro.Parameters.Add("@flg_operacao", SqlDbType.Char).Value = "D";
        cmdSelCadastro.Parameters.Add("@NOM_CODE", SqlDbType.VarChar).Value = sID.ToString();
        SqlDataReader ReaderCadastro = cmdSelCadastro.ExecuteReader();
        while (ReaderCadastro.Read())
        {
            //if (ReaderCadastro["FLG_CANCELADO"].ToString() == "S")
            //{
            //    dvCancelado.Visible = true;
            //}
            //else
            //{
            //    dvCancelado.Visible = false;
            //}

            string jsonLinhaDigitavel = asaas.ObterLinhaDigitavelCobranca(asaas.PegarTokenSubconta(Funcoes.strToInt(ReaderCadastro["COD_ID_PESSOAS_FJ_VENDEDOR"].ToString()), Funcoes.strToInt(ReaderCadastro["COD_ID_PESSOA_LICENCIADO"].ToString())), sid_id.ToString());

            if (jsonLinhaDigitavel.ToString().Trim() != "")
            {
                try
                {
                    JObject oLinhaDigitavel = JObject.Parse(jsonLinhaDigitavel.ToString());

                    lblNossoNumeroCompensacao.Text = oLinhaDigitavel["nossoNumero"].ToString();
                    lblNossoNumeroPagador.Text = oLinhaDigitavel["nossoNumero"].ToString();

                    lblLinhaDigitavelCompensacao.Text = oLinhaDigitavel["identificationField"].ToString();
                    lblLinhaDigitavelPagador.Text = oLinhaDigitavel["identificationField"].ToString();
                    lblLinhaDigitavel.Text = oLinhaDigitavel["identificationField"].ToString();

                    lblCodigoBarrasCompensacao.Text = oLinhaDigitavel["barCode"].ToString();

                    lblValorDocumentoCompensacao.Text = String.Format("{0:c2}", Funcoes.strToDouble(ReaderCadastro["NUM_VALOR"].ToString()));
                    lblValorDocumentoPagador.Text = String.Format("{0:c2}", Funcoes.strToDouble(ReaderCadastro["NUM_VALOR"].ToString()));

                    lblDataDocumentoCompensacao.Text = String.Format("{0:dd/MM/yyyy}", Convert.ToDateTime(ReaderCadastro["DTA_DATA"].ToString()));
                    lblDataDocumentoPagador.Text = String.Format("{0:dd/MM/yyyy}", Convert.ToDateTime(ReaderCadastro["DTA_DATA"].ToString()));
                    lblDataProcessamentoCompensacao.Text = String.Format("{0:dd/MM/yyyy}", Convert.ToDateTime(ReaderCadastro["DTA_DATA"].ToString()));
                    lblDataProcessamentoPagador.Text = String.Format("{0:dd/MM/yyyy}", Convert.ToDateTime(ReaderCadastro["DTA_DATA"].ToString()));

                    lblBeneficiarioPagador.Text = ReaderCadastro["NOM_VENDEDOR"].ToString();
                    lblDocumentoBeneficiarioPagador.Text = ReaderCadastro["NUM_DOCUMENTO_VENDEDOR"].ToString();
                    lblBeneficiarioCompensacao.Text = ReaderCadastro["NOM_VENDEDOR"].ToString();
                    lblDocumentoBeneficiarioCompensacao.Text = ReaderCadastro["NUM_DOCUMENTO_VENDEDOR"].ToString();

                    lblVencimentoCompensacao.Text = String.Format("{0:dd/MM/yyyy}", Convert.ToDateTime(ReaderCadastro["DTA_VENCIMENTO"].ToString()));
                    lblVencimentoPagador.Text = String.Format("{0:dd/MM/yyyy}", Convert.ToDateTime(ReaderCadastro["DTA_VENCIMENTO"].ToString()));

                    lblNomeCompradorCompensacao.Text = ReaderCadastro["NOM_NOME"].ToString();
                    lblDocumentoCompradorCompensacao.Text = ReaderCadastro["NUM_CNPJCPF"].ToString();
                    lblNomeCompradorPagador.Text = ReaderCadastro["NOM_NOME"].ToString();
                    lblDocumentoCompradorPagador.Text = ReaderCadastro["NUM_CNPJCPF"].ToString();

                    lblComprador.Text = "Olá, " + ReaderCadastro["NOM_NOME"].ToString();
                    lblMensagemEntrada.Text = "Aqui está o seu boleto bancário";

                    lblNumeroDocumentoCompensacao.Text = ReaderCadastro["COD_ID"].ToString().PadLeft(10, '0');   //ReaderCadastro["NOM_REFERENCIA"].ToString();
                    lblNumeroDocumentoPagador.Text = ReaderCadastro["COD_ID"].ToString().PadLeft(10, '0'); //ReaderCadastro["NOM_REFERENCIA"].ToString();

                    lblContaPagador.Text = ReaderCadastro["NUM_AGENCIA_BAAS"].ToString().PadLeft(3, '0') + "/" + ReaderCadastro["NUM_CONTA_BAAS"].ToString() + "-" + ReaderCadastro["NUM_DIGITO_CONTA_BAAS"].ToString();
                    lblContaCompensacao.Text = ReaderCadastro["NUM_AGENCIA_BAAS"].ToString().PadLeft(3, '0') + "/" + ReaderCadastro["NUM_CONTA_BAAS"].ToString() + "-" + ReaderCadastro["NUM_DIGITO_CONTA_BAAS"].ToString();


                    lblInstrucoesCompensacao.Text = "Nao receber com cheque. <br />" + ReaderCadastro["DES_DESCRICAO"].ToString();
                    lblInstrucoesPagador.Text = "Nao receber com cheque. <br />" + ReaderCadastro["DES_DESCRICAO"].ToString() + "<br />" + oLinhaDigitavel["barCode"].ToString();
                }
                catch
                {
                }

            }
            
            string jsonQRCodeCobranca = asaas.ObterQRCodeCobranca(asaas.PegarTokenSubconta(Funcoes.strToInt(ReaderCadastro["COD_ID_PESSOAS_FJ_VENDEDOR"].ToString()), Funcoes.strToInt(ReaderCadastro["COD_ID_PESSOA_LICENCIADO"].ToString())), sid_id.ToString());

            if (jsonQRCodeCobranca.ToString().Trim() != "")
            {
                try
                {
                    JObject oQRCodeCobranca = JObject.Parse(jsonQRCodeCobranca.ToString());

                    lblPixCopiaCola.Text = oQRCodeCobranca["payload"].ToString();

                    string base64String = oQRCodeCobranca["encodedImage"].ToString(); //encodedImage

                    QRCodeEncoder encoder = new QRCodeEncoder();

                    Bitmap image = encoder.Encode(oQRCodeCobranca["payload"].ToString());
                    image.Save(Server.MapPath("public_html") + "\\" + sid_id.ToString() + ".bmp");

                    imgQRcode.Src = "../public_html/" + sid_id.ToString() + ".bmp";
                    imgQRcodeBoleto.Src = "../public_html/" + sid_id.ToString() + ".bmp";
                }
                catch
                {

                }

            }

            //string jsonDadosConta = asaas.
        }
    }

    private void ConsultaQRCodeCobrancaLinkPagamento(string sID)
    {
        SqlConnection mySelCadastro = new SqlConnection(Funcoes.conexao());
        mySelCadastro.Open();
        SqlCommand cmdSelCadastro = new SqlCommand("dbo.stp_vendas_vendas_ins", mySelCadastro);
        cmdSelCadastro.CommandType = CommandType.StoredProcedure;
        cmdSelCadastro.Parameters.Add("@flg_operacao", SqlDbType.Char).Value = "D";
        cmdSelCadastro.Parameters.Add("@NOM_CODE", SqlDbType.VarChar).Value = sID.ToString();
        SqlDataReader ReaderCadastro = cmdSelCadastro.ExecuteReader();
        while (ReaderCadastro.Read())
        {
            string jsonQRCodeCobranca = asaas.ObterQRCodeCobranca(asaas.PegarTokenSubconta(Funcoes.strToInt(ReaderCadastro["COD_ID_PESSOAS_FJ_VENDEDOR"].ToString()), Funcoes.strToInt(ReaderCadastro["COD_ID_PESSOA_LICENCIADO"].ToString())), sid_id.ToString());

            if (jsonQRCodeCobranca.ToString().Trim() != "")
            {
                try
                {
                    lblComprador01.Text = "Olá, " + ReaderCadastro["NOM_NOME"].ToString();
                    lblMensagemEntrada01.Text = "Aqui está o seu QRCode";


                    JObject oQRCodeCobranca = JObject.Parse(jsonQRCodeCobranca.ToString());
                    lblPixCopiaCola01.Text = oQRCodeCobranca["payload"].ToString();
                    string base64String = oQRCodeCobranca["encodedImage"].ToString(); //encodedImage
                    QRCodeEncoder encoder = new QRCodeEncoder();
                    Bitmap image = encoder.Encode(oQRCodeCobranca["payload"].ToString());
                    image.Save(Server.MapPath("public_html") + "\\" + sid_id.ToString() + ".bmp");
                    imgQRcode01.Src = "../public_html/" + sid_id.ToString() + ".bmp";
                }
                catch
                {

                }

            }

            //string jsonDadosConta = asaas.
        }
    }

}