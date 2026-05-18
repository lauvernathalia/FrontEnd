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

public partial class cad_pedidos_erp_pagamentos : System.Web.UI.Page
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
        // Verifica se o usuário esta autenticado
        if (HttpContext.Current.Session.Count <= 0)
        {
            Response.Redirect("login.aspx");
            FormsAuthentication.SignOut();
        }

        if (!IsPostBack)
        {
            Funcoes.GravaAuditoria(0, Request.ServerVariables["REMOTE_ADDR"].ToString(), "Pedidos ERP - Pagamentos", "Acesso - ID: " + Funcoes.strToInt(sid_id.ToString()).ToString() + " - " + Request.ServerVariables["SERVER_NAME"].ToString());

            ConsultaFicha();
            ConsultaGeral();
        }

    }

    private void ConsultaFicha()
    {
        SqlConnection mySelCadastro = new SqlConnection(Funcoes.conexao());
        mySelCadastro.Open();
        SqlCommand cmdSelCadastro = new SqlCommand("dbo.stp_pedido_erp_ins", mySelCadastro);
        cmdSelCadastro.CommandType = CommandType.StoredProcedure;
        cmdSelCadastro.Parameters.Add("@flg_operacao", SqlDbType.Char).Value = "S";
        cmdSelCadastro.Parameters.Add("@COD_ID", SqlDbType.Int).Value = Funcoes.strToInt(sid_id.ToString());
        cmdSelCadastro.Parameters.Add("@COD_ID_PESSOA_LICENCIADO", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["LICENCIADO"].ToString());
        cmdSelCadastro.Parameters.Add("@COD_ID_PESSOAS_FJ", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["PESSOA"].ToString());
        SqlDataReader ReaderCadastro = cmdSelCadastro.ExecuteReader();
        while (ReaderCadastro.Read())
        {
            lblID.Text = ReaderCadastro["COD_ID"].ToString();
            lblCliente.Text = ReaderCadastro["NOM_CLIENTE"].ToString();
            lblParcelas.Text = ReaderCadastro["NUM_PARCELAS"].ToString();
            lblValor.Text = ReaderCadastro["NUM_VALOR_TOTAL"].ToString();
            lblPedido.Text = ReaderCadastro["NUM_CODIGO_PEDIDO"].ToString();
            lblDocumento.Text = ReaderCadastro["NOM_CNPJCPF_CLIENTE"].ToString();
            lblEmail.Text = ReaderCadastro["NOM_EMAIL_CLIENTE"].ToString();
            lblIDCliente.Text = ReaderCadastro["COD_ID_COMPRADOR"].ToString();
            txtToken.Text = ReaderCadastro["NUM_TOKEN"].ToString();
            txtLogotipo.Text = "public_html/" + ReaderCadastro["NOM_LOGOTIPO_CHECKOUT"].ToString();
        }

    }


    private void ConsultaGeral()
    {
        SqlConnection myConsulta = new SqlConnection(Funcoes.conexao());
        myConsulta.Open();
        SqlDataAdapter SDAConsulta = new SqlDataAdapter("dbo.stp_pedido_erp_pagamento_ins", myConsulta);
        SDAConsulta.SelectCommand.CommandType = CommandType.StoredProcedure;
        SDAConsulta.SelectCommand.Parameters.Add("@flg_operacao", SqlDbType.Char).Value = "C";
        SDAConsulta.SelectCommand.Parameters.Add("@COD_ID_PESSOA_LICENCIADO", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["LICENCIADO"].ToString());
        SDAConsulta.SelectCommand.Parameters.Add("@COD_ID_PESSOAS_FJ", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["PESSOA"].ToString());
        SDAConsulta.SelectCommand.Parameters.Add("@COD_ID_PEDIDO_ERP", SqlDbType.Int).Value = Funcoes.strToInt(sid_id.ToString());
        DataSet dsConsulta = new DataSet();
        SDAConsulta.Fill(dsConsulta, "PEDIDO_ERP_PAGAMENTO");

        rptConsultaPagamentos.DataSource = dsConsulta.Tables["PEDIDO_ERP_PAGAMENTO"].DefaultView;
        rptConsultaPagamentos.DataBind();

        myConsulta.Close(); myConsulta.Dispose();
    }

    protected void btnCancelar_Click(object sender, System.EventArgs e)
    {
        ClientScript.RegisterStartupScript(this.GetType(),
    "FecharJanela", "opener.PostBackOnMainPage(); window.close(); ", true);

    }
    protected void btnGerar_Click(object sender, System.EventArgs e)
    {
        // Cadastrar o cliente na zoop
        GravaCliente();

        // Cria pagamento em boleto bancário
        GravaPagamentos();
    }

    private void GravaCliente()
    {
        string retornoInclusao = "";
        string sTokenComprador = "";

        // Cadastrar o cliente na zoop

        dadosComprador.Comprador dcomprador = new dadosComprador.Comprador()
        {
            first_name = lblCliente.Text.ToString(),
            email = lblEmail.Text.ToString(),
            taxpayer_id = Funcoes.TIRAACENTOS(lblDocumento.Text.ToString()),
            address = new dadosComprador.address()
            {
                line1 = "Rua",
                line2 = "",
                line3 = "",
                neighborhood = "Bairro",
                city = "São Paulo",
                state = "SP",
                postal_code = "00000000",
                country_code = "BR"
            }
        };




        string json = JsonConvert.SerializeObject(dcomprador);
        string jsonRetornoComprador = zoop.compradores("I", "", json);

        if (jsonRetornoComprador.ToString().Trim() != "")
        {
            JObject oRetornoComprador = JObject.Parse(jsonRetornoComprador);
            sTokenComprador = oRetornoComprador["id"].ToString();

            SqlConnection connInsConsCliente = new SqlConnection(Funcoes.conexao());
            connInsConsCliente.Open();
            SqlCommand cmdInsConsCliente = new SqlCommand("dbo.stp_compradores_ins", connInsConsCliente);
            cmdInsConsCliente.CommandType = CommandType.StoredProcedure;

            cmdInsConsCliente.Parameters.Add("@flg_operacao", SqlDbType.Char).Value = 'I';
            cmdInsConsCliente.Parameters.Add("@FLG_ATIVO", SqlDbType.Char).Value = "S";

            cmdInsConsCliente.Parameters.Add("@COD_ID_PESSOA_LICENCIADO", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["LICENCIADO"].ToString());
            cmdInsConsCliente.Parameters.Add("@COD_ID_PESSOAS_FJ", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["PESSOA"].ToString());
            cmdInsConsCliente.Parameters.Add("@COD_ID_COMPRADOR", SqlDbType.VarChar).Value = sTokenComprador.ToString();

            cmdInsConsCliente.Parameters.Add("@DTA_DATA", SqlDbType.DateTime).Value = DateTime.Now;
            cmdInsConsCliente.Parameters.Add("@FLG_TIPO", SqlDbType.Char).Value = "C";

            cmdInsConsCliente.Parameters.Add("@NOM_NOME", SqlDbType.VarChar).Value = lblCliente.Text.ToString();
            cmdInsConsCliente.Parameters.Add("@NUM_CNPJCPF", SqlDbType.VarChar).Value = lblDocumento.Text.ToString();
            cmdInsConsCliente.Parameters.Add("@NOM_EMAIL", SqlDbType.VarChar).Value = lblEmail.Text.ToString();

            retornoInclusao = cmdInsConsCliente.ExecuteScalar().ToString();
            connInsConsCliente.Close();
            connInsConsCliente.Dispose();


            SqlConnection connInsConsClientePedido = new SqlConnection(Funcoes.conexao());
            connInsConsClientePedido.Open();
            SqlCommand cmdInsConsClientePedido = new SqlCommand("dbo.stp_pedido_erp_ins", connInsConsClientePedido);
            cmdInsConsClientePedido.CommandType = CommandType.StoredProcedure;

            cmdInsConsClientePedido.Parameters.Add("@flg_operacao", SqlDbType.Char).Value = 'P';

            cmdInsConsClientePedido.Parameters.Add("@COD_ID_PESSOA_LICENCIADO", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["LICENCIADO"].ToString());
            cmdInsConsClientePedido.Parameters.Add("@COD_ID_PESSOAS_FJ", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["PESSOA"].ToString());
            cmdInsConsClientePedido.Parameters.Add("@COD_ID", SqlDbType.Int).Value = Funcoes.strToInt(sid_id.ToString());
            cmdInsConsClientePedido.Parameters.Add("@COD_ID_COMPRADOR", SqlDbType.Int).Value = Funcoes.strToInt(retornoInclusao.ToString());

            cmdInsConsClientePedido.ExecuteNonQuery();
            connInsConsClientePedido.Close();
            connInsConsClientePedido.Dispose();

            lblIDCliente.Text = sTokenComprador.ToString();
        }

    }

    private void GravaPagamentos()
    {
        int iParcelas = 1;


        foreach (RepeaterItem itemE in rptConsultaPagamentos.Items)
        {
            double vValorTotal = Funcoes.strToDouble(((TextBox)itemE.FindControl("txtvalor")).Text.ToString());

            string urlorigem = Request.ServerVariables["SERVER_NAME"].ToString();
            string urlLogotipo = "https://" + urlorigem + "/" + txtLogotipo.Text.ToString();
            string sDescricao = "Ref. Pedido No." + lblPedido.Text.ToString() + " - Parc. " + iParcelas.ToString() + "/" + lblParcelas.Text.ToString();
            DadosTransacao.TransacaoBoletoPix dtransacao = new DadosTransacao.TransacaoBoletoPix()
            {
                on_behalf_of = txtToken.Text.ToString(),
                customer = lblIDCliente.Text.ToString(),
                amount = Funcoes.strToInt(Convert.ToString(vValorTotal * 100).ToString()),
                currency = "BRL",
                description = sDescricao.ToString(),
                payment_type = "bolepix",
                reference_id = lblPedido.Text.ToString() + "/" + iParcelas.ToString(),
                payment_method = new DadosTransacao.payment_method_pix()
                {
                    due_at = Convert.ToDateTime(((TextBox)itemE.FindControl("txtvencimento")).Text.ToString().Trim()).Year.ToString().PadLeft(4, '0') + "-" + Convert.ToDateTime(((TextBox)itemE.FindControl("txtvencimento")).Text.ToString().Trim()).Month.ToString().PadLeft(2, '0') + "-" + Convert.ToDateTime(((TextBox)itemE.FindControl("txtvencimento")).Text.ToString().Trim()).Day.ToString().PadLeft(2, '0'),
                    payment_limit_at = Convert.ToDateTime(((TextBox)itemE.FindControl("txtvencimento")).Text.ToString().Trim()).Year.ToString().PadLeft(4, '0') + "-" + Convert.ToDateTime(((TextBox)itemE.FindControl("txtvencimento")).Text.ToString().Trim()).Month.ToString().PadLeft(2, '0') + "-" + Convert.ToDateTime(((TextBox)itemE.FindControl("txtvencimento")).Text.ToString().Trim()).Day.ToString().PadLeft(2, '0'),
                    billing_message_list = new DadosTransacao.billing_message_list[]
                    {
                        new DadosTransacao.billing_message_list()
                        {
                        message = lblPedido.Text.ToString() + "/" + iParcelas.ToString()
                        }
                    }
                }
            };


            /*
            DadosTransacao.TransacaoBoleto dtransacao = new DadosTransacao.TransacaoBoleto()
            {
                on_behalf_of = txtToken.Text.ToString(),
                customer = lblIDCliente.Text.ToString(),
                amount = Funcoes.strToInt(Convert.ToString(vValorTotal * 100).ToString()),
                currency = "BRL",
                description = sDescricao.ToString(),
                payment_type = "bolepix",
                reference_id = lblPedido.Text.ToString() + "/" + iParcelas.ToString(),
                logo = urlLogotipo.ToString(),
                

                payment_method = new DadosTransacao.payment_method()
                {
                    expiration_date = (((TextBox)itemE.FindControl("txtvencimento")).Text.ToString().Trim() == "") ? "" : String.Format("{0:u}", Convert.ToDateTime(((TextBox)itemE.FindControl("txtvencimento")).Text.ToString().Trim())),
                    payment_limit_date = null,
                    billing_instructions = null,
                    body_instructions = null
                },
            };
            */
            string jsonBoleto = JsonConvert.SerializeObject(dtransacao);
            string jsonRetorno = zoop.transacao("I", "", jsonBoleto);
            
            txtRetorno.Visible = true;
            txtRetorno.Text = jsonRetorno;

            if (jsonRetorno.ToString().Trim() != "")
            {
                try
                {
                    JObject oRetorno = JObject.Parse(jsonRetorno);
                    try
                    {
                        if ((oRetorno["status"].ToString().Trim() != "") && (oRetorno["id"].ToString().Trim() != ""))
                        {

                            // Gravar dados atualizados na tabela VENDAS

                            SqlConnection connInsCons = new SqlConnection(Funcoes.conexao());
                            connInsCons.Open();
                            SqlCommand cmdInsCons = new SqlCommand("dbo.stp_pedido_erp_pagamento_ins", connInsCons);
                            cmdInsCons.CommandType = CommandType.StoredProcedure;
                            cmdInsCons.Parameters.Add("@flg_operacao", SqlDbType.Char).Value = 'P';

                            cmdInsCons.Parameters.Add("@COD_ID_PESSOA_LICENCIADO", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["LICENCIADO"].ToString());
                            cmdInsCons.Parameters.Add("@COD_ID_PESSOAS_FJ", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["PESSOA"].ToString());
                            cmdInsCons.Parameters.Add("@COD_ID", SqlDbType.Int).Value = Funcoes.strToInt(((TextBox)itemE.FindControl("txtid")).Text.ToString().Trim());
                            cmdInsCons.Parameters.Add("@NOM_CODE", SqlDbType.VarChar).Value = oRetorno["id"].ToString().Trim();
                            cmdInsCons.Parameters.Add("@FLG_STATUS", SqlDbType.VarChar).Value = oRetorno["status"].ToString().Trim();
                            cmdInsCons.Parameters.Add("@DES_JSON", SqlDbType.Text).Value = jsonRetorno.ToString();

                            string URL = "https://" + Request.ServerVariables["SERVER_NAME"].ToString() + "/boletoerp.aspx?id=" + Funcoes.Encrypt(((TextBox)itemE.FindControl("txtid")).Text.ToString().Trim()) + "&tipo=" + Funcoes.Encrypt("Z");
                            cmdInsCons.Parameters.Add("@NOM_URL_BOLETO", SqlDbType.VarChar).Value = URL.ToString();
                            cmdInsCons.ExecuteNonQuery();
                            connInsCons.Close();
                            connInsCons.Dispose();

                            ClientScript.RegisterStartupScript(this.GetType(), "Sucesso", "alert('Pagamentos gerados com sucesso!');", true);

                        }
                    }
                    catch
                    {
                        ClientScript.RegisterStartupScript(this.GetType(), "ErroGeracaoBoleto", "alert('Ocorreu um erro ao gerar os pagamentos! Observação: " + oRetorno["error"]["reasons"]["message"].ToString() + ".');", true);

                    }
                }
                catch
                {
                    ClientScript.RegisterStartupScript(this.GetType(), "ErroGeralGeracaoBoleto", "alert('Ocorreu um erro ao gerar os pagamentos!');", true);
                }
            }

            iParcelas = iParcelas + 1;
        }
    }
    protected void btnEnviar_Click(object sender, System.EventArgs e)
    {

    }
    protected void rptConsultaPagamentos_ItemCommand(object source, System.Web.UI.WebControls.RepeaterCommandEventArgs e)
    {
        if (e.CommandName == "Excluir")
        {
            foreach (RepeaterItem item in rptConsultaPagamentos.Items)
            {
                if (Funcoes.strToInt(e.CommandArgument.ToString()) == Funcoes.strToInt(((TextBox)item.FindControl("txtid")).Text.ToString()))
                {

                }
            }
        }
    }
}