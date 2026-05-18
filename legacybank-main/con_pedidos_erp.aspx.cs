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

public partial class con_pedidos_erp : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (HttpContext.Current.Session.Count <= 0)
        {
            Response.Redirect("login.aspx");
            FormsAuthentication.SignOut();
        }


        if (!IsPostBack)
        {
            DateTime data = DateTime.Today;
            DateTime primeiroDiaDoMes = new DateTime(data.Year, data.Month, 1);
            DateTime ultimoDiaDoMes = new DateTime(data.Year, data.Month, DateTime.DaysInMonth(data.Year, data.Month));

            txtDataIni.Text = primeiroDiaDoMes.ToShortDateString();
            txtDataFim.Text = DateTime.Now.ToShortDateString();
            // Verifica se possui integração

            //divAlerta.Visible = false;
            //divErp.Visible = false;
            //divErpLista.Visible = false;

            SqlConnection mySelCadastro = new SqlConnection(Funcoes.conexao());
            mySelCadastro.Open();
            SqlCommand cmdSelCadastro = new SqlCommand("dbo.stp_pessoas_fj_erp_ins", mySelCadastro);
            cmdSelCadastro.CommandType = CommandType.StoredProcedure;
            cmdSelCadastro.Parameters.Add("@flg_operacao", SqlDbType.Char).Value = "S";
            cmdSelCadastro.Parameters.Add("@COD_ID_PESSOAS_FJ", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["PESSOA"].ToString());
            cmdSelCadastro.Parameters.Add("@COD_ID_PESSOA_LICENCIADO", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["LICENCIADO"].ToString());
            SqlDataReader ReaderCadastro = cmdSelCadastro.ExecuteReader();
            while (ReaderCadastro.Read())
            {
                if (ReaderCadastro["FLG_ERP"].ToString().Trim() == "S")
                {
                    divErp.Visible = true;
                    divErpLista.Visible = true;
                    ConsultaGeral();
                }
                else
                {
                    divAlerta.Visible = true;
                }
            }
        }

    }
    protected void btnPesquisar_Click(object sender, EventArgs e)
    {
        ConsultaGeral();
    }
    protected void btnImportar_Click(object sender, EventArgs e)
    {
        // Acessar API
        
        // Inserir os dados do pedido
        string jsonPedido = erp.ConsultarPedido(txtImportar.Text.ToString());
        //txtJsonRetorno.Text = jsonPedido.ToString();
        //try
        //{
            JArray oPedido = JArray.Parse(jsonPedido);
            //txtNome.Text = jsonPedido.ToString();
            
            if (oPedido.Count > 0)
            {
                for (int i = 0; i < oPedido.Count; i++)
                {
                    //Gravar na base de dados Pedidos
                    SqlConnection connInsConsDadosERP = new SqlConnection(Funcoes.conexao());
                    connInsConsDadosERP.Open();
                    SqlCommand cmdInsConsDadosERP = new SqlCommand("dbo.stp_pedido_erp_ins", connInsConsDadosERP);
                    cmdInsConsDadosERP.CommandType = CommandType.StoredProcedure;
                    cmdInsConsDadosERP.Parameters.Add("@flg_operacao", SqlDbType.Char).Value = 'I';
                    cmdInsConsDadosERP.Parameters.Add("@COD_ID_PESSOA_LICENCIADO", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["LICENCIADO"].ToString());
                    cmdInsConsDadosERP.Parameters.Add("@COD_ID_PESSOAS_FJ", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["PESSOA"].ToString());
                    cmdInsConsDadosERP.Parameters.Add("@DTA_DATA", SqlDbType.DateTime).Value = Convert.ToDateTime(oPedido[i]["Data"].ToString().Split('-').FirstOrDefault().ToString());
                    cmdInsConsDadosERP.Parameters.Add("@NOM_ID_PEDIDO", SqlDbType.VarChar).Value = oPedido[i]["ID"].ToString();
                    cmdInsConsDadosERP.Parameters.Add("@FLG_STATUS_PEDIDO", SqlDbType.VarChar).Value = oPedido[i]["StatusSistema"].ToString();
                    cmdInsConsDadosERP.Parameters.Add("@NOM_ID_EMPRESA", SqlDbType.VarChar).Value = oPedido[i]["EmpresaID"].ToString();
                    cmdInsConsDadosERP.Parameters.Add("@NOM_ID_CLIENTE", SqlDbType.VarChar).Value = oPedido[i]["ClienteID"].ToString();
                    cmdInsConsDadosERP.Parameters.Add("@NOM_ID_PESSOA", SqlDbType.VarChar).Value = oPedido[i]["PessoaID"].ToString();
                    cmdInsConsDadosERP.Parameters.Add("@NOM_CLIENTE", SqlDbType.VarChar).Value = oPedido[i]["Cliente"].ToString();
                    cmdInsConsDadosERP.Parameters.Add("@NOM_CNPJCPF_CLIENTE", SqlDbType.VarChar).Value = oPedido[i]["ClienteCNPJ"].ToString();
                    cmdInsConsDadosERP.Parameters.Add("@NOM_EMAIL_CLIENTE", SqlDbType.VarChar).Value = oPedido[i]["ClienteEmail"].ToString();
                    cmdInsConsDadosERP.Parameters.Add("@NOM_ID_FORMA_PAGAMENTO", SqlDbType.VarChar).Value = oPedido[i]["FormaPagamentoID"].ToString();
                    cmdInsConsDadosERP.Parameters.Add("@NOM_FORMA_PAGAMENTO", SqlDbType.VarChar).Value = oPedido[i]["FormaPagamento"].ToString();

                    cmdInsConsDadosERP.Parameters.Add("@NUM_PARCELAS", SqlDbType.Int).Value = Funcoes.strToInt(oPedido[i]["NumeroParcelas"].ToString());
                    cmdInsConsDadosERP.Parameters.Add("@DTA_APROVADO", SqlDbType.DateTime).Value = Convert.ToDateTime(oPedido[i]["DataAprovacaoPedido"].ToString().Split('-').FirstOrDefault().ToString());
                    cmdInsConsDadosERP.Parameters.Add("@DTA_FATURAMENTO", SqlDbType.DateTime).Value = Convert.ToDateTime(oPedido[i]["DataFaturamento"].ToString().Split('-').FirstOrDefault().ToString());
                    cmdInsConsDadosERP.Parameters.Add("@NUM_CODIGO_PEDIDO", SqlDbType.Int).Value = Funcoes.strToInt(oPedido[i]["Codigo"].ToString());
                    cmdInsConsDadosERP.Parameters.Add("@NUM_VALOR_TOTAL", SqlDbType.Int).Value = Funcoes.strToDouble(oPedido[i]["ValorFinal"].ToString());
                    cmdInsConsDadosERP.Parameters.Add("@DES_JSON", SqlDbType.Text).Value = jsonPedido.ToString();
                    string sRetornoInclusao = cmdInsConsDadosERP.ExecuteScalar().ToString();

                    connInsConsDadosERP.Close();
                    connInsConsDadosERP.Dispose();
                    
                    // Inserir Itens
                    if (oPedido[i]["Items"].Count() > 0)
                    {
                        for (int x = 0; x < oPedido[i]["Items"].Count(); x++)
                        {
                            SqlConnection connInsConsDadosERPItem = new SqlConnection(Funcoes.conexao());
                            connInsConsDadosERPItem.Open();
                            SqlCommand cmdInsConsDadosERPItem = new SqlCommand("dbo.stp_pedido_erp_item_ins", connInsConsDadosERPItem);
                            cmdInsConsDadosERPItem.CommandType = CommandType.StoredProcedure;
                            cmdInsConsDadosERPItem.Parameters.Add("@flg_operacao", SqlDbType.Char).Value = 'I';
                            cmdInsConsDadosERPItem.Parameters.Add("@COD_ID_PESSOA_LICENCIADO", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["LICENCIADO"].ToString());
                            cmdInsConsDadosERPItem.Parameters.Add("@COD_ID_PESSOAS_FJ", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["PESSOA"].ToString());
                            cmdInsConsDadosERPItem.Parameters.Add("@COD_ID_PEDIDO_ERP", SqlDbType.Int).Value = Funcoes.strToInt(sRetornoInclusao.ToString());

                            cmdInsConsDadosERPItem.Parameters.Add("@COD_ID_ITEM", SqlDbType.VarChar).Value = oPedido[i]["Items"][x]["Codigo"].ToString();
                            //cmdInsConsDadosERPItem.Parameters.Add("@NOM_UNIDADE_ITEM", SqlDbType.VarChar).Value = oPedido[i]["Items"][x]["Unidade"].ToString();
                            cmdInsConsDadosERPItem.Parameters.Add("@NOM_ITEM", SqlDbType.VarChar).Value = oPedido[i]["Items"][x]["Descricao"].ToString();

                            cmdInsConsDadosERPItem.Parameters.Add("@NUM_QTDE", SqlDbType.Int).Value = Funcoes.strToInt(oPedido[i]["Items"][x]["Quantidade"].ToString());
                            cmdInsConsDadosERPItem.Parameters.Add("@NUM_VALOR_UNITARIO", SqlDbType.Int).Value = Funcoes.strToDouble(oPedido[i]["Items"][x]["ValorUnitario"].ToString());
                            cmdInsConsDadosERPItem.Parameters.Add("@NUM_VALOR_TOTAL", SqlDbType.Int).Value = Funcoes.strToDouble(oPedido[i]["Items"][x]["ValorTotal"].ToString());

                            cmdInsConsDadosERPItem.ExecuteNonQuery();
                            connInsConsDadosERPItem.Close();
                            connInsConsDadosERPItem.Dispose();
                        }
                    }
                    // Inserir Pagamentos
                    if (oPedido[i]["Pagamentos"].Count() > 0)
                    {
                        for (int p = 0; p < oPedido[i]["Pagamentos"].Count(); p++)
                        {
                            SqlConnection connInsConsDadosERPPagamentos = new SqlConnection(Funcoes.conexao());
                            connInsConsDadosERPPagamentos.Open();
                            SqlCommand cmdInsConsDadosERPPagamentos = new SqlCommand("dbo.stp_pedido_erp_pagamento_ins", connInsConsDadosERPPagamentos);
                            cmdInsConsDadosERPPagamentos.CommandType = CommandType.StoredProcedure;
                            cmdInsConsDadosERPPagamentos.Parameters.Add("@flg_operacao", SqlDbType.Char).Value = 'I';
                            cmdInsConsDadosERPPagamentos.Parameters.Add("@COD_ID_PESSOA_LICENCIADO", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["LICENCIADO"].ToString());
                            cmdInsConsDadosERPPagamentos.Parameters.Add("@COD_ID_PESSOAS_FJ", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["PESSOA"].ToString());
                            cmdInsConsDadosERPPagamentos.Parameters.Add("@COD_ID_PEDIDO_ERP", SqlDbType.Int).Value = Funcoes.strToInt(sRetornoInclusao.ToString());

                            cmdInsConsDadosERPPagamentos.Parameters.Add("@COD_ID_PAGAMENTO", SqlDbType.Int).Value = p+1;


                            cmdInsConsDadosERPPagamentos.Parameters.Add("@NOM_FORMA_PAGAMENTO", SqlDbType.VarChar).Value = oPedido[i]["Pagamentos"][p]["FormaPagamento"].ToString();
                            cmdInsConsDadosERPPagamentos.Parameters.Add("@DTA_VENCIMENTO", SqlDbType.DateTime).Value = Convert.ToDateTime(oPedido[i]["Pagamentos"][p]["DataTransacao"].ToString().Split('-').FirstOrDefault().ToString());
                            cmdInsConsDadosERPPagamentos.Parameters.Add("@NUM_VALOR", SqlDbType.Int).Value = Funcoes.strToDouble(oPedido[i]["Pagamentos"][p]["ValorPagamento"].ToString());

                            cmdInsConsDadosERPPagamentos.ExecuteNonQuery();
                            connInsConsDadosERPPagamentos.Close();
                            connInsConsDadosERPPagamentos.Dispose();
                        }
                    }
                    
                    txtJsonRetorno.Text = txtJsonRetorno.Text + " - " + oPedido[i]["ID"].ToString();
                    ClientScript.RegisterStartupScript(this.GetType(), "Sucesso", "alert('Pedido importado com sucesso!'); ", true);
                    ConsultaGeral();
                }
            }
            
        //}
        //catch
        //{
        //    ClientScript.RegisterStartupScript(this.GetType(), "Erro", "alert('Ocorreu um erro ao tentar importar o pedido informado ou pedido não existe! Verifique e reentre'); ", true);
        //}

    }

    private void ConsultaGeral()
    {
        //dtgConsulta.Visible = true;
        SqlConnection myConsulta = new SqlConnection(Funcoes.conexao());
        myConsulta.Open();
        SqlDataAdapter SDAConsulta = new SqlDataAdapter("dbo.stp_pedido_erp_ins", myConsulta);
        SDAConsulta.SelectCommand.CommandType = CommandType.StoredProcedure;
        SDAConsulta.SelectCommand.Parameters.Add("@flg_operacao", SqlDbType.Char).Value = "C";
        SDAConsulta.SelectCommand.Parameters.Add("@COD_ID_PESSOA_LICENCIADO", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["LICENCIADO"].ToString());
        SDAConsulta.SelectCommand.Parameters.Add("@COD_ID_PESSOAS_FJ", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["PESSOA"].ToString());
        SDAConsulta.SelectCommand.Parameters.Add("@DTA_INICIO", SqlDbType.DateTime).Value = Convert.ToDateTime(txtDataIni.Text.ToString());
        SDAConsulta.SelectCommand.Parameters.Add("@DTA_FIM", SqlDbType.DateTime).Value = Convert.ToDateTime(txtDataFim.Text.ToString());
        SDAConsulta.SelectCommand.Parameters.Add("@NOM_CLIENTE", SqlDbType.VarChar).Value = txtNome.Text.ToString();
        SDAConsulta.SelectCommand.Parameters.Add("@NOM_CNPJCPF_CLIENTE", SqlDbType.VarChar).Value = txtDocumento.Text.ToString();

        
        DataSet dsConsulta = new DataSet();
        SDAConsulta.Fill(dsConsulta, "PEDIDO_ERP");
        rptConsulta.DataSource = dsConsulta.Tables["PEDIDO_ERP"].DefaultView;
        rptConsulta.DataBind();
        myConsulta.Close(); myConsulta.Dispose();

    }


    public string GetPostBackScript()
    {
        PostBackOptions options = new PostBackOptions(btnPostback);
        Page.ClientScript.RegisterForEventValidation(options);

        return Page.ClientScript.GetPostBackEventReference(options);
    }

    protected void btnPostBack_Click(object sender, EventArgs e)
    {
        ConsultaGeral();
    }

    protected void rptConsulta_OnItemCommand(object source, RepeaterCommandEventArgs e)
    {

    }
}