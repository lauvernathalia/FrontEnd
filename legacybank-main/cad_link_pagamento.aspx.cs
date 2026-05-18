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

public partial class cad_link_pagamento : System.Web.UI.Page
{
    public string sid_id
    {
        get
        {
            try { return Request["id"]; }
            catch { return ""; }
        }
    }

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

            if (Funcoes.strToInt(sid_id) != 0)
            {
                ConsultaFicha();
            }
            else
            {
                ddlParcelas.SelectedValue = "1";
                ckbCredito.Checked = true;
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

            txtValorTotal.Text = ReaderCadastro["NUM_VALOR"].ToString();
            txtReferencia.Text = ReaderCadastro["NOM_REFERENCIA_PRODUTO"].ToString();
            ddlLinkPermanente.SelectedValue = ReaderCadastro["FLG_LINK_PERMANENTE"].ToString();
            txtDescricao.Text = ReaderCadastro["NOM_DESCRICAO_PRODUTO"].ToString();
            txtCampo01.Text = ReaderCadastro["NOM_CAMPO_01"].ToString();
            txtImagem.Text = ReaderCadastro["NOM_IMAGEM"].ToString();

            ckbBoleto.Checked = (ReaderCadastro["FLG_BOLETO"].ToString() == "S") ? true : false;
            ckbPix.Checked = (ReaderCadastro["FLG_PIX"].ToString() == "S") ? true : false;
            ckbCredito.Checked = (ReaderCadastro["FLG_CARTAO_CREDITO"].ToString() == "S") ? true : false;
            ddlParcelas.SelectedValue = ReaderCadastro["NUM_PARCELAS"].ToString();
            ddlDias.SelectedValue = ReaderCadastro["NUM_DIAS_VENCIMENTO"].ToString();
            txtDataLimite.Text = ReaderCadastro["DTA_PIX_VENCIMENTO"].ToString();
            txtImagem.Text = ReaderCadastro["NOM_IMAGEM"].ToString();
        }
    }


    protected void btnCancelar_Click(object sender, EventArgs e)
    {
        ClientScript.RegisterStartupScript(this.GetType(),
    "Alerta", "opener.PostBackOnMainPage(); window.close(); ", true);
    }

    public string GetPostBackScript()
    {
        PostBackOptions options = new PostBackOptions(btnPostback);
        Page.ClientScript.RegisterForEventValidation(options);

        return Page.ClientScript.GetPostBackEventReference(options);
    }

    protected void btnPostBack_Click(object sender, EventArgs e)
    {

    }

    protected void btnAvancar01_Click(object sender, EventArgs e)
    {
        dvInformacoes.Visible = false;

        string NomeArquivoflImagem = "";
        string StrFileNameflImagem = flFoto.PostedFile.FileName.Substring(flFoto.PostedFile.FileName.LastIndexOf("\\") + 1);
        string StrFileTypeflImagem = flFoto.PostedFile.ContentType;
        int IntFileSizeflImagem = flFoto.PostedFile.ContentLength;
        if (StrFileNameflImagem.Trim() != "")
        {
            string CodificacaoflImagem = DateTime.Now.Year.ToString() + DateTime.Now.Month.ToString() + DateTime.Now.Day.ToString() + DateTime.Now.Hour.ToString() + DateTime.Now.Minute.ToString() + DateTime.Now.Second.ToString();
            flFoto.PostedFile.SaveAs(Server.MapPath("public_html") + "\\" + HttpContext.Current.Session["CODIGO"].ToString().Trim() + "_" + CodificacaoflImagem.ToString() + "_" + StrFileNameflImagem);
            NomeArquivoflImagem = HttpContext.Current.Session["CODIGO"].ToString().Trim() + "_" + CodificacaoflImagem.ToString() + "_" + StrFileNameflImagem;
            txtImagem.Text = NomeArquivoflImagem.ToString();
        }

        dvFormaPagamento.Visible = true;
    }
    protected void btnVoltar02_Click(object sender, EventArgs e)
    {
        dvInformacoes.Visible = true;
        dvFormaPagamento.Visible = false;

    }
    protected void btnAvancar02_Click(object sender, EventArgs e)
    {
        dvFormaPagamento.Visible = false;
        dvResumo.Visible = true;

        lblValor.Text = txtValorTotal.Text.ToString();
        lblNome.Text = txtReferencia.Text.ToString();
        lblDescricao.Text = txtDescricao.Text.ToString();

        lblDias.Text = ddlDias.SelectedValue.ToString();
        lblDataLimite.Text = txtDataLimite.Text.ToString();
        lblFormaPagamento.Text = "";
        if (ckbBoleto.Checked == true) { lblFormaPagamento.Text = lblFormaPagamento.Text + "(Boleto) "; }
        if (ckbPix.Checked == true) { lblFormaPagamento.Text = lblFormaPagamento.Text + "(Pix) "; }
        if (ckbCredito.Checked == true) { lblFormaPagamento.Text = lblFormaPagamento.Text + "(Crédito) "; }

        lblFoto.Text = txtImagem.Text.ToString();

    }
    protected void btnVoltar03_Click(object sender, EventArgs e)
    {
        dvFormaPagamento.Visible = true;
        dvResumo.Visible = false;

    }
    protected void btnConfirmar_Click(object sender, EventArgs e)
    {

        // Verifica se foi selecionada alguma opção de pagamento
        if ((ckbBoleto.Checked == true) || (ckbPix.Checked == true) || (ckbCredito.Checked == true))
        {
            if (Funcoes.strToDouble(txtValorTotal.Text.ToString().Trim()) > 0)
            {
                // 1o. Verifica se o cliente foi especificado (Comprador)
                Funcoes.GravaAuditoria(Funcoes.strToInt(sid_id), Request.ServerVariables["REMOTE_ADDR"].ToString(), "Venda por link", "Salvar dados");
                // Imagem Fundo
                try
                {
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
                    cmdInsCons.Parameters.Add("@FLG_INTEGRACAO", SqlDbType.Char).Value = 'Z';


                    cmdInsCons.Parameters.Add("@NOM_REFERENCIA_PRODUTO", SqlDbType.VarChar).Value = txtReferencia.Text.ToString();
                    cmdInsCons.Parameters.Add("@NOM_DESCRICAO_PRODUTO", SqlDbType.Text).Value = txtDescricao.Text.ToString();

                    cmdInsCons.Parameters.Add("@FLG_LINK_PERMANENTE", SqlDbType.Char).Value = ddlLinkPermanente.SelectedValue.ToString();

                    cmdInsCons.Parameters.Add("@FLG_BOLETO", SqlDbType.Char).Value = (ckbBoleto.Checked == true) ? "S" : "N";
                    cmdInsCons.Parameters.Add("@FLG_PIX", SqlDbType.Char).Value = (ckbPix.Checked == true) ? "S" : "N";
                    cmdInsCons.Parameters.Add("@FLG_CARTAO_CREDITO", SqlDbType.Char).Value = (ckbCredito.Checked == true) ? "S" : "N";

                    cmdInsCons.Parameters.Add("@FLG_TIPO_COBRANCA", SqlDbType.Char).Value = "U";

                    cmdInsCons.Parameters.Add("@FLG_TIPO_OPERACAO", SqlDbType.Char).Value = "C";
                    cmdInsCons.Parameters.Add("@FLG_OPERADORA", SqlDbType.Char).Value = "Z";

                    cmdInsCons.Parameters.Add("@FLG_PRECO_PARCELAMENTO", SqlDbType.Char).Value = "N";

                    cmdInsCons.Parameters.Add("@NOM_EMAIL_LINK", SqlDbType.VarChar).Value = "";

                    cmdInsCons.Parameters.Add("@NOM_CAMPO_01", SqlDbType.VarChar).Value = txtCampo01.Text.ToString();
                    cmdInsCons.Parameters.Add("@NOM_IMAGEM", SqlDbType.VarChar).Value = txtImagem.Text.ToString();
                    cmdInsCons.Parameters.Add("@NUM_VALOR", SqlDbType.Float).Value = Funcoes.strToDouble(txtValorTotal.Text.ToString());

                    cmdInsCons.Parameters.Add("@NUM_PARCELAS", SqlDbType.Int).Value = Funcoes.strToInt(ddlParcelas.SelectedValue.ToString());

                    if (txtDataLimite.Text.ToString().Trim() != "") { cmdInsCons.Parameters.Add("@DTA_PIX_VENCIMENTO", SqlDbType.DateTime).Value = Convert.ToDateTime(txtDataLimite.Text.ToString()); }

                    string urlorigem = Request.ServerVariables["SERVER_NAME"].ToString();
                    cmdInsCons.Parameters.Add("@NOM_ORIGEM", SqlDbType.VarChar).Value = urlorigem.ToString() + "/checkout.aspx?id=";
                    cmdInsCons.ExecuteNonQuery();
                    connInsCons.Close();
                    connInsCons.Dispose();
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
}