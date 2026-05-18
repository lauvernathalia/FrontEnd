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



public partial class cad_historico_vendas_detalhe : System.Web.UI.Page
{
    public string sid_id
    {
        get
        {
            try { return Funcoes.Decrypt(Request["id"]); }
            catch { return ""; }
        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (HttpContext.Current.Session.Count <= 0)
        {
            Response.Redirect("login.aspx");
            FormsAuthentication.SignOut();
        }


        if (!IsPostBack)
        {

            ConsultaTransacao();
            ConsultaFicha();
        }

    }

    private void ConsultaTransacao()
    {
        SqlConnection myConsulta = new SqlConnection(Funcoes.conexao());
        myConsulta.Open();
        SqlDataAdapter SDAConsulta = new SqlDataAdapter("dbo.stp_transacoes_ins", myConsulta);
        SDAConsulta.SelectCommand.CommandTimeout = 0;
        SDAConsulta.SelectCommand.CommandType = CommandType.StoredProcedure;
        SDAConsulta.SelectCommand.Parameters.Add("@flg_operacao", SqlDbType.Char).Value = "S";
        SDAConsulta.SelectCommand.Parameters.Add("@COD_ID_PESSOA_LICENCIADO", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["LICENCIADO"].ToString());
        SDAConsulta.SelectCommand.Parameters.Add("@COD_ID", SqlDbType.Int).Value = Funcoes.strToInt(sid_id.ToString());
        DataSet dsConsulta = new DataSet();
        SDAConsulta.Fill(dsConsulta, "TRANSACOES");
        rptTransacao.DataSource = dsConsulta.Tables["TRANSACOES"].DefaultView;
        rptTransacao.DataBind();
        myConsulta.Close(); myConsulta.Dispose();

    }

    private void AtualizaHistorico(string transacaoID)
    {
        string json = zoop.transacao("P", transacaoID.ToString(),"");
        JObject o = JObject.Parse(json);
        
        // Verifica se a operação é boleto e pega o código ID do boleto


        if (o["history"].Count() > 0)
        {
            for (int i = 0; i < o["history"].Count(); i++)
            {
                // Inserção de dados do Histórico

                SqlConnection connInsConsHistorico = new SqlConnection(Funcoes.conexao());
                connInsConsHistorico.Open();
                SqlCommand cmdInsConsHistorico = new SqlCommand("dbo.stp_transacoes_historico_ins", connInsConsHistorico);
                cmdInsConsHistorico.CommandType = CommandType.StoredProcedure;
                cmdInsConsHistorico.Parameters.Add("@flg_operacao", SqlDbType.Char).Value = 'I';
                cmdInsConsHistorico.Parameters.Add("@COD_ID_PESSOA_LICENCIADO", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["LICENCIADO"].ToString());
                cmdInsConsHistorico.Parameters.Add("@NOM_CODE", SqlDbType.VarChar).Value = o["id"].ToString();
                cmdInsConsHistorico.Parameters.Add("@DTA_DATA", SqlDbType.DateTime).Value = DateTime.Now;
                cmdInsConsHistorico.Parameters.Add("@NOM_TIPO", SqlDbType.VarChar).Value = o["history"][i]["operation_type"].ToString();
                cmdInsConsHistorico.Parameters.Add("@NOM_STATUS", SqlDbType.VarChar).Value = o["history"][i]["status"].ToString();
                cmdInsConsHistorico.Parameters.Add("@NUM_VALOR", SqlDbType.Float).Value = Funcoes.strToDouble(o["history"][i]["amount"].ToString());
                cmdInsConsHistorico.Parameters.Add("@NOM_CODIGO_RESPOSTA", SqlDbType.VarChar).Value = o["history"][i]["response_code"].ToString();
                cmdInsConsHistorico.Parameters.Add("@NOM_DESCRICAO_RESPOSTA", SqlDbType.VarChar).Value = o["history"][i]["response_message"].ToString();
                cmdInsConsHistorico.Parameters.Add("@NOM_ID_AUTORIZACAO", SqlDbType.VarChar).Value = o["history"][i]["authorizer_id"].ToString();
                cmdInsConsHistorico.Parameters.Add("@NOM_NSU_AUTORIZACAO", SqlDbType.VarChar).Value = o["history"][i]["authorization_nsu"].ToString();
                cmdInsConsHistorico.Parameters.Add("@DTA_DATA_CRIACAO", SqlDbType.DateTime).Value = Convert.ToDateTime(o["history"][i]["created_at"].ToString());
                cmdInsConsHistorico.ExecuteNonQuery();
                connInsConsHistorico.Close();
                connInsConsHistorico.Dispose();
            }
        }
    }


    private void AtualizaStatus(string transacaoID)
    {
        try
        {
            string jsonRecebiveis = zoop.DetalhesRecebiveis(transacaoID.ToString());
            JObject oRecebiveis = JObject.Parse(jsonRecebiveis);

            if (oRecebiveis["items"].Count() > 0)
            {
                for (int i = 0; i < oRecebiveis["items"].Count(); i++)
                {

                    // Inserção de dados do Recebiveis

                    SqlConnection connInsConsRecebiveis = new SqlConnection(Funcoes.conexao());
                    connInsConsRecebiveis.Open();
                    SqlCommand cmdInsConsRecebiveis = new SqlCommand("dbo.stp_transacoes_recebiveis_ins", connInsConsRecebiveis);
                    cmdInsConsRecebiveis.CommandType = CommandType.StoredProcedure;
                    cmdInsConsRecebiveis.Parameters.Add("@flg_operacao", SqlDbType.Char).Value = 'I';
                    cmdInsConsRecebiveis.Parameters.Add("@COD_ID_PESSOA_LICENCIADO", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["LICENCIADO"].ToString());
                    cmdInsConsRecebiveis.Parameters.Add("@NOM_CODE_TRANSACAO", SqlDbType.VarChar).Value = transacaoID.ToString();
                    cmdInsConsRecebiveis.Parameters.Add("@NOM_CODE_RECEBIVEL", SqlDbType.VarChar).Value = oRecebiveis["items"][i]["id"].ToString();
                    cmdInsConsRecebiveis.Parameters.Add("@NOM_CODE_DESTINATARIO", SqlDbType.VarChar).Value = oRecebiveis["items"][i]["recipient"].ToString();

                    cmdInsConsRecebiveis.Parameters.Add("@DTA_DATA", SqlDbType.DateTime).Value = DateTime.Now;

                    cmdInsConsRecebiveis.Parameters.Add("@NOM_TIPO", SqlDbType.VarChar).Value = oRecebiveis["items"][i]["resource"].ToString();
                    cmdInsConsRecebiveis.Parameters.Add("@NOM_STATUS", SqlDbType.VarChar).Value = oRecebiveis["items"][i]["status"].ToString();
                    if (oRecebiveis["items"][i]["installment"].ToString() != "null")
                    {
                        cmdInsConsRecebiveis.Parameters.Add("@NUM_PARCELA", SqlDbType.Int).Value = Funcoes.strToInt(oRecebiveis["items"][i]["installment"].ToString());
                    }
                    else
                    {
                        cmdInsConsRecebiveis.Parameters.Add("@NUM_PARCELA", SqlDbType.Int).Value = 1;
                    }

                    cmdInsConsRecebiveis.Parameters.Add("@NUM_VALOR", SqlDbType.Float).Value = Funcoes.strToDouble(oRecebiveis["items"][i]["amount"].ToString());
                    cmdInsConsRecebiveis.Parameters.Add("@NUM_VALOR_BRUTO", SqlDbType.Float).Value = Funcoes.strToDouble(oRecebiveis["items"][i]["gross_amount"].ToString());
                    cmdInsConsRecebiveis.Parameters.Add("@NUM_TAXA_ANTECIPACAO", SqlDbType.Float).Value = Funcoes.strToDouble(oRecebiveis["items"][i]["anticipation_fee"].ToString());

                    cmdInsConsRecebiveis.Parameters.Add("@NOM_CODIGO_AUTORIZACAO", SqlDbType.VarChar).Value = oRecebiveis["items"][i]["authorization_code"].ToString();

                    if ((oRecebiveis["items"][i]["created_at"].ToString().Trim() != "null") && (oRecebiveis["items"][i]["created_at"].ToString().Trim() != ""))
                    {
                        cmdInsConsRecebiveis.Parameters.Add("@DTA_DATA_CRIACAO", SqlDbType.DateTime).Value = Convert.ToDateTime(oRecebiveis["items"][i]["created_at"].ToString());
                    }
                    if ((oRecebiveis["items"][i]["paid_at"].ToString().Trim() != "null") && (oRecebiveis["items"][i]["paid_at"].ToString().Trim() != ""))
                    {
                        cmdInsConsRecebiveis.Parameters.Add("@DTA_DATA_PAGAMENTO", SqlDbType.DateTime).Value = Convert.ToDateTime(oRecebiveis["items"][i]["paid_at"].ToString());
                    }
                    if ((oRecebiveis["items"][i]["canceled_at"].ToString().Trim() != "null") && (oRecebiveis["items"][i]["canceled_at"].ToString().Trim() != ""))
                    {
                        cmdInsConsRecebiveis.Parameters.Add("@DTA_DATA_CANCELAMENTO", SqlDbType.DateTime).Value = Convert.ToDateTime(oRecebiveis["items"][i]["canceled_at"].ToString());
                    }
                    if ((oRecebiveis["items"][i]["expected_on"].ToString().Trim() != "null") && (oRecebiveis["items"][i]["expected_on"].ToString().Trim() != ""))
                    {
                        cmdInsConsRecebiveis.Parameters.Add("@DTA_DATA_PREVISTO", SqlDbType.DateTime).Value = Convert.ToDateTime(oRecebiveis["items"][i]["expected_on"].ToString());
                    }
                    cmdInsConsRecebiveis.ExecuteNonQuery();
                    connInsConsRecebiveis.Close();
                    connInsConsRecebiveis.Dispose();
                }
            }
        }
        catch
        {

        }
    }

    private void ConsultaFicha()
    {
        SqlConnection mySelCadastro = new SqlConnection(Funcoes.conexao());
        mySelCadastro.Open();
        SqlCommand cmdSelCadastro = new SqlCommand("dbo.stp_transacoes_ins", mySelCadastro);
        cmdSelCadastro.CommandType = CommandType.StoredProcedure;
        cmdSelCadastro.Parameters.Add("@flg_operacao", SqlDbType.Char).Value = "S";
        cmdSelCadastro.Parameters.Add("@COD_ID_PESSOA_LICENCIADO", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["LICENCIADO"].ToString());
        cmdSelCadastro.Parameters.Add("@COD_ID", SqlDbType.Int).Value = Funcoes.strToInt(sid_id.ToString());
        SqlDataReader ReaderCadastro = cmdSelCadastro.ExecuteReader();
        while (ReaderCadastro.Read())
        {
            lblNomeVendedor.Text = ReaderCadastro["NOM_VENDEDOR"].ToString();
            lblDocumentoVendedor.Text = ReaderCadastro["NOM_DOCUMENTO_VENDEDOR"].ToString();
            lblEnderecoVendedor.Text = ReaderCadastro["NOM_ENDERECO_VENDEDOR"].ToString();
            lblIDVendedor.Text = ReaderCadastro["NUM_ID_VENDEDOR"].ToString();
            txtValorEstornar.Text = ReaderCadastro["NUM_VALOR_BRUTO"].ToString();
            
            if (ReaderCadastro["FLG_ORIGEM"].ToString() == "Z")
            {
                ConsultaDadosZoop();
                AtualizaStatus(ReaderCadastro["NOM_CODE"].ToString());
                AtualizaHistorico(ReaderCadastro["NOM_CODE"].ToString());
            }
            if (ReaderCadastro["FLG_ORIGEM"].ToString() == "P")
            {
                ConsultaDadosPagseguro();
            }

            if (ReaderCadastro["NOM_TIPO_PAGAMENTO"].ToString() == "Crédito")
            {
                dvEstornoVendasCartao.Visible = true;
            }

            if (ReaderCadastro["NOM_TIPO_PAGAMENTO"].ToString() == "Boleto")
            {
                dvEstornoVendasBoleto.Visible = true;
            }

        }
    }
    private void ConsultaDadosPagseguro()
    {
       

    }


    private void ConsultaDadosZoop()
    {

        SqlConnection mySelCadastro = new SqlConnection(Funcoes.conexao());
        mySelCadastro.Open();
        SqlCommand cmdSelCadastro = new SqlCommand("dbo.stp_transacoes_ins", mySelCadastro);
        cmdSelCadastro.CommandType = CommandType.StoredProcedure;
        cmdSelCadastro.Parameters.Add("@flg_operacao", SqlDbType.Char).Value = "S";
        cmdSelCadastro.Parameters.Add("@COD_ID_PESSOA_LICENCIADO", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["LICENCIADO"].ToString());
        cmdSelCadastro.Parameters.Add("@COD_ID", SqlDbType.Int).Value = Funcoes.strToInt(sid_id.ToString());
        SqlDataReader ReaderCadastro = cmdSelCadastro.ExecuteReader();
        while (ReaderCadastro.Read())
        {
            txtIDTransacao.Text = ReaderCadastro["NOM_CODE"].ToString();
            lblSerialTerminal.Text = ReaderCadastro["NOM_NUMERO_SERIAL_TERMINAL"].ToString();
            lblCodigoTerminal.Text = ReaderCadastro["NOM_CODIGO_TERMINAL"].ToString();
            lblModeloTerminal.Text = ReaderCadastro["NOM_MODELO_TERMINAL"].ToString();

            imgCliente.ImageUrl = "public_html/" + HttpContext.Current.Session["LOGOPADRAO"].ToString();
            imgEstabelecimento.ImageUrl = "public_html/" + HttpContext.Current.Session["LOGOPADRAO"].ToString();

            lblReciboCliente.Text = ReaderCadastro["NOM_RECIBO_VENDA_CLIENTE"].ToString().Replace("@", "<br />");
            lblReciboEstabelecimento.Text = ReaderCadastro["NOM_RECIBO_VENDA_ESTABELECIMENTO"].ToString().Replace("@", "<br />");

            if (ReaderCadastro["NOM_TIPO_PAGAMENTO"].ToString() == "Pix")
            {
                dvPix.Visible = true;
                lblPixCodigo.Text = ReaderCadastro["NOM_QRCODE"].ToString();
                lblPixDescricao.Text = "";
            }

            if (ReaderCadastro["NOM_TIPO_PAGAMENTO"].ToString() == "Crédito")
            {
                if (ReaderCadastro["NOM_STATUS_PADRAO"].ToString().Trim() == "Cancelada")
                {
                    dvCancelamento.Visible = true;
                    // Consulta Link Carta Cancelamento

                    try
                    {
                        string jsonCancelamento = zoop.CartaCancelamento(txtIDTransacao.Text.ToString());
                        JObject oCancelamento = JObject.Parse(jsonCancelamento);
                        txtCartaCancelamento.Text = oCancelamento["url"].ToString();
                        hrCartaCancelamento.HRef= oCancelamento["url"].ToString();
                    }
                    catch
                    {
                        txtCartaCancelamento.Text = "";
                    }
                }
             
                dvCredito.Visible = true;
                if (ReaderCadastro["NOM_CLIENTE"].ToString().Trim() != "")
                {

                    string[] partes = ReaderCadastro["NOM_CLIENTE"].ToString().Split('/');
                    if (partes.Count() == 1)
                    {
                        lblCreditoPortador.Text = partes[0].ToString();
                    }
                    if (partes.Count() > 1)
                    {
                        lblCreditoPortador.Text = partes[1].ToString() + " " + partes[0].ToString();
                    }
                }
                lblCreditoModoCaptura.Text = ReaderCadastro["NOM_FLG_PRESENCIAL_ONLINE"].ToString();
                lblCreditoNumero.Text = ReaderCadastro["NOM_CARTAO_4_PRIMEIROS_DIGIROS"].ToString() + " XXXX XXXX " + ReaderCadastro["NOM_CARTAO_4_ULTIMOS_DIGITOS"].ToString();
                lblCreditoValidade.Text = ReaderCadastro["NOM_CARTAO_MES"].ToString().PadLeft(2, '0') + "/" + ReaderCadastro["NOM_CARTAO_ANO"].ToString().PadLeft(4,'0');
                lblCreditoAutorizacao.Text = ReaderCadastro["NOM_NUMERO_TRANSACAO"].ToString();


            }
            if (ReaderCadastro["NOM_TIPO_PAGAMENTO"].ToString() == "Débito")
            {
                dvDebito.Visible = true;
                if (ReaderCadastro["NOM_CLIENTE"].ToString().Trim() != "")
                {
                    string[] partes = ReaderCadastro["NOM_CLIENTE"].ToString().Split('/');
                    if (partes.Count() == 1)
                    {
                        lblDebitoPortador.Text = partes[0].ToString();
                    }
                    if (partes.Count() > 1)
                    {
                        lblDebitoPortador.Text = partes[1].ToString() + " " + partes[0].ToString();
                    }
                }
                lblDebitoModoCaptura.Text = ReaderCadastro["NOM_FLG_PRESENCIAL_ONLINE"].ToString();
                lblDebitoNumero.Text = ReaderCadastro["NOM_CARTAO_4_PRIMEIROS_DIGIROS"].ToString() + " XXXX XXXX " + ReaderCadastro["NOM_CARTAO_4_ULTIMOS_DIGITOS"].ToString();
                lblDebitoValidade.Text = ReaderCadastro["NOM_CARTAO_MES"].ToString().PadLeft(2, '0') + "/" + ReaderCadastro["NOM_CARTAO_ANO"].ToString().PadLeft(4, '0');
                lblDebitoAutorizacao.Text = ReaderCadastro["NOM_NUMERO_TRANSACAO"].ToString();

            }
            if (ReaderCadastro["NOM_TIPO_PAGAMENTO"].ToString() == "Boleto")
            {
                dvBoleto.Visible = true;
                lblBoletoCodigoBarras.Text = ReaderCadastro["NOM_CODIGO_BARRAS"].ToString();
                hrfBoleto.HRef = ReaderCadastro["NOM_URL_BOLETO"].ToString();

                lblBoletoNumero.Text = ReaderCadastro["NOM_NUMERO_DOCUMENTO"].ToString();
                lblBoletoVencimento.Text = Convert.ToDateTime(ReaderCadastro["NOM_DATA_VENCIMENTO"].ToString()).ToShortDateString();
            }


            SqlConnection myConsulta = new SqlConnection(Funcoes.conexao());
            myConsulta.Open();
            SqlDataAdapter SDAConsulta = new SqlDataAdapter("dbo.stp_transacoes_historico_ins", myConsulta);
            SDAConsulta.SelectCommand.CommandTimeout = 0;
            SDAConsulta.SelectCommand.CommandType = CommandType.StoredProcedure;
            SDAConsulta.SelectCommand.Parameters.Add("@flg_operacao", SqlDbType.Char).Value = "L";
            SDAConsulta.SelectCommand.Parameters.Add("@COD_ID_PESSOA_LICENCIADO", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["LICENCIADO"].ToString());
            SDAConsulta.SelectCommand.Parameters.Add("@NOM_CODE", SqlDbType.VarChar).Value = ReaderCadastro["NOM_CODE"].ToString();
            DataSet dsConsulta = new DataSet();
            SDAConsulta.Fill(dsConsulta, "TRANSACOES_HISTORICO");
            rptHistorico.DataSource = dsConsulta.Tables["TRANSACOES_HISTORICO"].DefaultView;
            rptHistorico.DataBind();
            myConsulta.Close(); myConsulta.Dispose();

            SqlConnection myConsultaRecebiveis = new SqlConnection(Funcoes.conexao());
            myConsultaRecebiveis.Open();
            SqlDataAdapter SDAConsultaRecebiveis = new SqlDataAdapter("dbo.stp_transacoes_recebiveis_ins", myConsultaRecebiveis);
            SDAConsultaRecebiveis.SelectCommand.CommandTimeout = 0;
            SDAConsultaRecebiveis.SelectCommand.CommandType = CommandType.StoredProcedure;
            SDAConsultaRecebiveis.SelectCommand.Parameters.Add("@flg_operacao", SqlDbType.Char).Value = "L";
            SDAConsultaRecebiveis.SelectCommand.Parameters.Add("@COD_ID_PESSOA_LICENCIADO", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["LICENCIADO"].ToString());
            SDAConsultaRecebiveis.SelectCommand.Parameters.Add("@NOM_CODE_TRANSACAO", SqlDbType.VarChar).Value = ReaderCadastro["NOM_CODE"].ToString();
            DataSet dsConsultaRecebiveis = new DataSet();
            SDAConsultaRecebiveis.Fill(dsConsultaRecebiveis, "TRANSACOES_RECEBIVEIS");
            rptRecebiveis.DataSource = dsConsultaRecebiveis.Tables["TRANSACOES_RECEBIVEIS"].DefaultView;
            rptRecebiveis.DataBind();
            myConsultaRecebiveis.Close(); myConsultaRecebiveis.Dispose();


        }
    }

    protected void btnFechar_Click(object sender, EventArgs e)
    {
        ClientScript.RegisterStartupScript(this.GetType(),
    "Alerta", "opener.PostBackOnMainPage(); window.close(); ", true);

    }

    protected void btnEstornarVenda_Click(object sender, EventArgs e)
    {
        dadosEstornarTransacao.Estornar destornar = new dadosEstornarTransacao.Estornar()
        {
            on_behalf_of = lblIDVendedor.Text.ToString(),
            amount = Funcoes.strToInt(Convert.ToString(Funcoes.strToDouble(txtValorEstornar.Text.ToString()) * 100).ToString()),
        };

        string json = JsonConvert.SerializeObject(destornar);

        string retornoEstorno = zoop.EstornarTransacao(txtIDTransacao.Text.ToString(), json);

        if (retornoEstorno.ToString().Trim().Length <= 20)
        {
            ClientScript.RegisterStartupScript(this.GetType(),
            "StatusEstorno", "alert('Cancelamento da transação com cartão de crédito realizado com sucesso!'); ", true);
        }
        else
        {
            JObject oRetornoErro = JObject.Parse(retornoEstorno.ToString().Trim());

            ClientScript.RegisterStartupScript(this.GetType(),
            "StatusEstornoVazio", "alert('Alguma coisa deu errado ao tentar cancelar a transação com cartão de crédito (" + retornoEstorno.ToString().Trim() + ")'); ", true);
        }

        //txtValorEstornar.Text = retornoEstorno.ToString().Trim();

    }
    protected void btnCancelarBoleto_Click(object sender, EventArgs e)
    {
        string retornoEstorno = zoop.CancelamentoBoleto(txtIDTransacao.Text.ToString());

        ClientScript.RegisterStartupScript(this.GetType(),
    "StatusCancelamento", "alert('Cancelamento do boleto realizado com sucesso'); ", true);
    }
}