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

public partial class cad_vendas_link_split : System.Web.UI.Page
{
    public string sid_id
    {
        get
        {
            try { return Funcoes.Decrypt(Request["id"]); }
            catch { return ""; }
        }
    }
    
    public string svenda_id
    {
        get
        {
            try { return Funcoes.Decrypt(Request["venda"]); }
            catch { return ""; }
        }
    }
    
    
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
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

            
            txtResposta.Visible = false;
            ConsultaGeral();
        }
    }

    private void ConsultaGeral()
    {
        SqlConnection myConsultaParceiros = new SqlConnection(Funcoes.conexao());
        myConsultaParceiros.Open();
        SqlDataAdapter SDAConsultaParceiros = new SqlDataAdapter("dbo.stp_vendas_split_ins", myConsultaParceiros);
        SDAConsultaParceiros.SelectCommand.CommandType = CommandType.StoredProcedure;
        SDAConsultaParceiros.SelectCommand.Parameters.Add("@flg_operacao", SqlDbType.Char).Value = "L";
        SDAConsultaParceiros.SelectCommand.Parameters.Add("@COD_ID_PESSOA_LICENCIADO", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["LICENCIADO"].ToString());
        SDAConsultaParceiros.SelectCommand.Parameters.Add("@COD_ID_VENDAS_VENDAS", SqlDbType.Int).Value = Funcoes.strToInt(sid_id.ToString());

        DataSet dsConsultaParceiros = new DataSet();
        SDAConsultaParceiros.Fill(dsConsultaParceiros, "VENDAS_SPLIT");
        rptParceiros.DataSource = dsConsultaParceiros.Tables["VENDAS_SPLIT"].DefaultView;
        rptParceiros.DataBind();
        myConsultaParceiros.Close();
        myConsultaParceiros.Dispose();

    }
    protected void btnFechar_Click(object sender, EventArgs e)
    {
        ClientScript.RegisterStartupScript(this.GetType(),
   "Alerta", "opener.PostBackOnMainPage(); window.close();", true);
    }
    protected void btnConfirmar_Click(object sender, EventArgs e)
    {
        foreach (RepeaterItem itemP in rptParceiros.Items)
        {

            if (((TextBox)itemP.FindControl("txtidsplit")).Text.ToString().Trim() == "")
            {
                string json = "";
                if (Funcoes.strToDouble(((TextBox)itemP.FindControl("txtPercentual")).Text.ToString().Trim()) > 0)
                {
                    zoop.split_transacao dsplit = new zoop.split_transacao()
                    {
                        recipient = ((TextBox)itemP.FindControl("txttoken")).Text.ToString().Trim(),
                        charge_processing_fee = true,
                        liable = true,
                        percentage = Funcoes.strToDouble(((TextBox)itemP.FindControl("txtPercentual")).Text.ToString().Trim())
                    };
                    json = JsonConvert.SerializeObject(dsplit);
                }

                if (Funcoes.strToDouble(((TextBox)itemP.FindControl("txtValor")).Text.ToString().Trim()) > 0)
                {
                    zoop.split_transacao dsplit = new zoop.split_transacao()
                    {
                        recipient = ((TextBox)itemP.FindControl("txttoken")).Text.ToString().Trim(),
                        charge_processing_fee = true,
                        liable = true,
                        amount = Funcoes.strToInt((Funcoes.strToDouble(((TextBox)itemP.FindControl("txtValor")).Text.ToString().Trim())*100).ToString())
                    };
                    json = JsonConvert.SerializeObject(dsplit);
                }


                //ClientScript.RegisterStartupScript(this.GetType(), "Operacao" + ((TextBox)itemP.FindControl("txtidparceiro")).Text.ToString().Trim(), "alert('" + json.ToString() + "');", true);

                string jsonSplit = zoop.SplitTransacao(((TextBox)itemP.FindControl("txtidtransacao")).Text.ToString().Trim(), json);
                //txtResposta.Text = jsonSplit.ToString();
                //txtResposta.Visible = true;
                //return;                


                if (jsonSplit.ToString().Trim() != "")
                {
                    JObject oSplit = JObject.Parse(jsonSplit);
                    try
                    {
                        // Atualizar split

                        SqlConnection connInsConsSplitParceiros = new SqlConnection(Funcoes.conexao());
                        SqlCommand cmdInsConsSplitParceiros = new SqlCommand("dbo.stp_vendas_split_ins", connInsConsSplitParceiros);
                        cmdInsConsSplitParceiros.CommandType = CommandType.StoredProcedure;
                        cmdInsConsSplitParceiros.Parameters.Add("@flg_operacao", SqlDbType.Char).Value = 'I';
                        cmdInsConsSplitParceiros.Parameters.Add("@COD_ID_VENDAS_VENDAS", SqlDbType.Int).Value = Funcoes.strToInt(((TextBox)itemP.FindControl("txtidvenda")).Text.ToString().Trim());
                        cmdInsConsSplitParceiros.Parameters.Add("@COD_ID_VENDAS_PARCEIROS", SqlDbType.Int).Value = Funcoes.strToInt(((TextBox)itemP.FindControl("txtidparceiro")).Text.ToString().Trim());
                        cmdInsConsSplitParceiros.Parameters.Add("@COD_ID_PESSOA_LICENCIADO", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["LICENCIADO"].ToString());

                        cmdInsConsSplitParceiros.Parameters.Add("@NOM_ID", SqlDbType.VarChar).Value = oSplit["id"].ToString();
                        cmdInsConsSplitParceiros.Parameters.Add("@DTA_DATA", SqlDbType.DateTime).Value = DateTime.Now;

                        cmdInsConsSplitParceiros.Parameters.Add("@NUM_PERCENTUAL", SqlDbType.Float).Value = Funcoes.strToDouble(((TextBox)itemP.FindControl("txtPercentual")).Text.ToString().Trim());
                        cmdInsConsSplitParceiros.Parameters.Add("@NUM_VALOR", SqlDbType.Float).Value = Funcoes.strToDouble(((TextBox)itemP.FindControl("txtValor")).Text.ToString().Trim());
                        connInsConsSplitParceiros.Open();
                        cmdInsConsSplitParceiros.ExecuteNonQuery();
                        
                    }
                    catch
                    {
                        try
                        {
                            ClientScript.RegisterStartupScript(this.GetType(), "ErroSplit", "alert('Ocorreu um erro ao tentar gerar o SPLIT do Parceiro: " + ((TextBox)itemP.FindControl("txtnome")).Text.ToString().Trim() + " - Erro: " + oSplit["error"]["message"].ToString() + "');", true);
                        }
                        catch
                        {
                            ClientScript.RegisterStartupScript(this.GetType(), "ErroSplitGeral", "alert('Ocorreu um erro ao tentar gerar o SPLIT do Parceiro: " + ((TextBox)itemP.FindControl("txtnome")).Text.ToString().Trim() + "');", true);

                        }
                    }

                }

            }


        }

        ConsultaGeral();

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
        SqlConnection connInsConsVendasParceiros = new SqlConnection(Funcoes.conexao());
        SqlCommand cmdInsConsVendasParceiros = new SqlCommand("dbo.stp_vendas_parceiros_ins", connInsConsVendasParceiros);
        cmdInsConsVendasParceiros.CommandType = CommandType.StoredProcedure;
        cmdInsConsVendasParceiros.Parameters.Add("@flg_operacao", SqlDbType.Char).Value = "I";
        cmdInsConsVendasParceiros.Parameters.Add("@COD_ID_VENDAS", SqlDbType.Int).Value = Funcoes.strToInt(svenda_id.ToString());
        cmdInsConsVendasParceiros.Parameters.Add("@COD_ID_PESSOA_LICENCIADO", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["LICENCIADO"].ToString());

        cmdInsConsVendasParceiros.Parameters.Add("@COD_ID_PARCEIRO", SqlDbType.Int).Value = Funcoes.strToInt(txtIDParceiro.Text.ToString());
        cmdInsConsVendasParceiros.Parameters.Add("@NUM_PERCENTUAL", SqlDbType.Float).Value = Funcoes.strToDouble(txtPercentualParceiro.Text.ToString());
        cmdInsConsVendasParceiros.Parameters.Add("@NUM_VALOR", SqlDbType.Float).Value = Funcoes.strToDouble(txtValorParceiro.Text.ToString());
        connInsConsVendasParceiros.Open();
        cmdInsConsVendasParceiros.ExecuteNonQuery();
        
        ConsultaParceiros();
        ConsultaGeral();

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

    protected void rptParceiros_ItemCommand(object source, RepeaterCommandEventArgs e)
    {
        if (e.CommandName == "Excluir")
        {
            // Excluir diretamente o Parceiro
            SqlConnection connInsConsVendasParceiros = new SqlConnection(Funcoes.conexao());
            SqlCommand cmdInsConsVendasParceiros = new SqlCommand("dbo.stp_vendas_parceiros_ins", connInsConsVendasParceiros);
            cmdInsConsVendasParceiros.CommandType = CommandType.StoredProcedure;
            cmdInsConsVendasParceiros.Parameters.Add("@flg_operacao", SqlDbType.Char).Value = 'E';
            cmdInsConsVendasParceiros.Parameters.Add("@COD_ID_PESSOA_LICENCIADO", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["LICENCIADO"].ToString());
            cmdInsConsVendasParceiros.Parameters.Add("@COD_ID", SqlDbType.Int).Value = Funcoes.strToInt(e.CommandArgument.ToString());
            connInsConsVendasParceiros.Open();
            cmdInsConsVendasParceiros.ExecuteNonQuery();
        }

    }
}