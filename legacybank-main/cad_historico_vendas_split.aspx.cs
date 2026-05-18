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

public partial class cad_historico_vendas_split : System.Web.UI.Page
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
        if (!IsPostBack)
        {
            if (Funcoes.strToInt(sid_id) != 0)
            {
                CarregaAdquirentes();
                CarregaEstabelecimentos();
                ConsultaFicha();
                ConsultaGeral();
            }

        }
    }

    private void CarregaEstabelecimentos()
    {
        SqlConnection myEstabelecimentos = new SqlConnection(Funcoes.conexao());
        myEstabelecimentos.Open();
        SqlCommand cmdEstabelecimentos = new SqlCommand("dbo.stp_parceiros_ins", myEstabelecimentos);
        cmdEstabelecimentos.CommandType = CommandType.StoredProcedure;
        cmdEstabelecimentos.Parameters.Add("@flg_operacao", SqlDbType.VarChar).Value = "C";

        cmdEstabelecimentos.Parameters.Add("@COD_ID_PESSOA_LICENCIADO", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["LICENCIADO"].ToString());
        cmdEstabelecimentos.Parameters.Add("@COD_ID_PESSOAS_FJ", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["PESSOA"].ToString());
        cmdEstabelecimentos.Parameters.Add("@NOM_NOME", SqlDbType.VarChar).Value = "";
        cmdEstabelecimentos.Parameters.Add("@NOM_CNPJ", SqlDbType.VarChar).Value = "";
        cmdEstabelecimentos.Parameters.Add("@FLG_TIPO", SqlDbType.Char).Value = "P";
        cmdEstabelecimentos.Parameters.Add("@FLG_INTEGRACAO", SqlDbType.Char).Value = "Z";

        SqlDataAdapter drEstabelecimentos = new SqlDataAdapter();
        drEstabelecimentos.SelectCommand = cmdEstabelecimentos;
        DataSet dsEstabelecimentos = new DataSet();
        drEstabelecimentos.Fill(dsEstabelecimentos, "PESSOAS_FJ");
        ddlFavorecido.DataTextField = "NOM_RAZAOSOCIAL";
        ddlFavorecido.DataValueField = "COD_ID";
        ddlFavorecido.DataSource = dsEstabelecimentos.Tables["PESSOAS_FJ"].DefaultView;
        ddlFavorecido.DataBind();
    }

    private void CarregaAdquirentes()
    {
        SqlConnection myAdquirentes = new SqlConnection(Funcoes.conexao());
        myAdquirentes.Open();
        SqlCommand cmdAdquirentes = new SqlCommand("dbo.stp_pessoas_fj_integracoes_ins", myAdquirentes);
        cmdAdquirentes.CommandType = CommandType.StoredProcedure;
        if ((HttpContext.Current.Session["TIPO"].ToString() == "L"))
        {
            cmdAdquirentes.Parameters.Add("@flg_operacao", SqlDbType.VarChar).Value = "G";
        }
        else
        {
            cmdAdquirentes.Parameters.Add("@flg_operacao", SqlDbType.VarChar).Value = "M";
        }

        cmdAdquirentes.Parameters.Add("@COD_ID_PESSOA_LICENCIADO", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["LICENCIADO"].ToString());
        cmdAdquirentes.Parameters.Add("@FLG_ATIVO", SqlDbType.Char).Value = "S";
        cmdAdquirentes.Parameters.Add("@FLG_ACESSO", SqlDbType.Char).Value = "S";
        SqlDataAdapter drAdquirentes = new SqlDataAdapter();
        drAdquirentes.SelectCommand = cmdAdquirentes;
        DataSet dsAdquirentes = new DataSet();
        drAdquirentes.Fill(dsAdquirentes, "PESSOAS_FJ_INTEGRACOES");

        ddlAdquirentes.DataTextField = "NOM_FANTASIA_INTEGRACAO";
        ddlAdquirentes.DataValueField = "COD_ID";
        ddlAdquirentes.DataSource = dsAdquirentes.Tables["PESSOAS_FJ_INTEGRACOES"].DefaultView;
        ddlAdquirentes.DataBind();
        ddlAdquirentes.Items.Insert(0, new ListItem("Selecione a adquirente", "0"));

        ddlAdquirentes.SelectedItem.Text = "ZOOP";
        ddlAdquirentes.Enabled = false;
    }


    private void ConsultaFicha()
    {
        SqlConnection mySelCadastro = new SqlConnection(Funcoes.conexao());
        mySelCadastro.Open();
        SqlCommand cmdSelCadastro = new SqlCommand("dbo.stp_transacoes_ins", mySelCadastro);
        cmdSelCadastro.CommandType = CommandType.StoredProcedure;
        cmdSelCadastro.CommandTimeout = 0;
        cmdSelCadastro.Parameters.Add("@flg_operacao", SqlDbType.Char).Value = "S";
        cmdSelCadastro.Parameters.Add("@COD_ID_PESSOA_LICENCIADO", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["LICENCIADO"].ToString());
        cmdSelCadastro.Parameters.Add("@COD_ID", SqlDbType.Int).Value = Funcoes.strToInt(sid_id.ToString());
        SqlDataReader ReaderCadastro = cmdSelCadastro.ExecuteReader();
        while (ReaderCadastro.Read())
        {
            txtID.Text = ReaderCadastro["COD_ID_PESSOAS_FJ"].ToString();
            txtRazaosocial.Text = ReaderCadastro["NOM_VENDEDOR"].ToString();
            txtTipo.Text = ReaderCadastro["NOM_TIPO_PAGAMENTO"].ToString();
            txtData.Text = Convert.ToDateTime(ReaderCadastro["DTA_DATA"].ToString()).ToShortDateString();
            txtValorVenda.Text = String.Format("{0:n2}",Funcoes.strToDouble(ReaderCadastro["NUM_VALOR_BRUTO"].ToString()));
            txtIDTransacao.Text = ReaderCadastro["NOM_CODE"].ToString();
        }

    }


    protected void btnFechar_Click(object sender, EventArgs e)
    {
        ClientScript.RegisterStartupScript(this.GetType(), "FecharOnboarding", "opener.PostBackOnMainPage(); window.close(); ", true);

    }

    private void ConsultaGeral()
    {
        SqlConnection myConsulta = new SqlConnection(Funcoes.conexao());
        myConsulta.Open();
        SqlDataAdapter SDAConsulta = new SqlDataAdapter("dbo.stp_transacoes_split_ins", myConsulta);
        SDAConsulta.SelectCommand.CommandType = CommandType.StoredProcedure;
        SDAConsulta.SelectCommand.Parameters.Add("@flg_operacao", SqlDbType.Char).Value = "C";
        SDAConsulta.SelectCommand.Parameters.Add("@COD_ID_PESSOA_LICENCIADO", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["LICENCIADO"].ToString());
        SDAConsulta.SelectCommand.Parameters.Add("@NOM_CODE", SqlDbType.VarChar).Value = txtIDTransacao.Text.ToString();

        DataSet dsConsulta = new DataSet();
        SDAConsulta.Fill(dsConsulta, "TRANSACOES_SPLIT");
        rptConsultaSplit.DataSource = dsConsulta.Tables["TRANSACOES_SPLIT"].DefaultView;
        rptConsultaSplit.DataBind();
        myConsulta.Close(); myConsulta.Dispose();

        foreach (RepeaterItem itemE in rptConsultaSplit.Items)
        {
            if (((TextBox)itemE.FindControl("txtcodesplit")).Text.ToString().Trim() !="")
            {
                ((LinkButton)itemE.FindControl("lkbProcessar")).Visible = false;
                ((LinkButton)itemE.FindControl("lkbExcluir")).Visible = false;
                ((LinkButton)itemE.FindControl("lkbCancelar")).Visible = true;

            }
            else
            {
                ((LinkButton)itemE.FindControl("lkbProcessar")).Visible = true;
                ((LinkButton)itemE.FindControl("lkbExcluir")).Visible = true;
                ((LinkButton)itemE.FindControl("lkbCancelar")).Visible = false;
            }
        }

    }



    protected void btnIncluirSplit_Click(object sender, EventArgs e)
    {
        SqlConnection connInsCons = new SqlConnection(Funcoes.conexao());
        connInsCons.Open();
        SqlCommand cmdInsCons = new SqlCommand("dbo.stp_transacoes_split_ins", connInsCons);
        cmdInsCons.CommandType = CommandType.StoredProcedure;
        cmdInsCons.Parameters.Add("@flg_operacao", SqlDbType.Char).Value = 'I';
        cmdInsCons.Parameters.Add("@COD_ID_PESSOA_LICENCIADO", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["LICENCIADO"].ToString());
        cmdInsCons.Parameters.Add("@COD_ID_PESSOAS_FJ", SqlDbType.Int).Value = Funcoes.strToInt(ddlFavorecido.SelectedValue.ToString());
        cmdInsCons.Parameters.Add("@NOM_CODE", SqlDbType.VarChar).Value = txtIDTransacao.Text.ToString();
        cmdInsCons.Parameters.Add("@DTA_DATA", SqlDbType.DateTime).Value = DateTime.Now;

        cmdInsCons.Parameters.Add("@NUM_PERCENTUAL", SqlDbType.Float).Value = Funcoes.strToDouble(txtPercentual.Text.ToString());
        cmdInsCons.Parameters.Add("@NUM_VALOR", SqlDbType.Float).Value = Funcoes.strToDouble(txtValor.Text.ToString());

        cmdInsCons.ExecuteNonQuery();
        connInsCons.Close();
        connInsCons.Dispose();

        ClientScript.RegisterStartupScript(this.GetType(),
            "Alerta", "alert('Split inserido com sucesso'); ", true);

        txtPercentual.Text = "";
        txtValor.Text = "";

        ConsultaGeral();
    }

    public static string TIRAACENTOS(string str)
    {
        str = str.Replace("-", "");
        str = str.Replace(".", "");
        str = str.Replace("/", "");
        str = str.Replace("(", "");
        str = str.Replace(")", "");
        str = str.Replace(" ", "");
        return str;
    }
    protected void rptConsultaSplit_ItemCommand(object source, RepeaterCommandEventArgs e)
    {
        if (e.CommandName == "Excluir")
        {
            SqlConnection connInsCons = new SqlConnection(Funcoes.conexao());
            SqlCommand cmdInsCons = new SqlCommand("dbo.stp_transacoes_split_ins", connInsCons);
            cmdInsCons.CommandType = CommandType.StoredProcedure;
            cmdInsCons.Parameters.Add("@flg_operacao", SqlDbType.Char).Value = 'E';
            cmdInsCons.Parameters.Add("@COD_ID", SqlDbType.Int).Value = Funcoes.strToInt(Convert.ToString(e.CommandArgument));
            cmdInsCons.Parameters.Add("@COD_ID_PESSOA_LICENCIADO", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["LICENCIADO"].ToString());
            cmdInsCons.Parameters.Add("@NOM_CODE", SqlDbType.VarChar).Value = txtIDTransacao.Text.ToString();
            connInsCons.Open();
            cmdInsCons.ExecuteNonQuery();

            ClientScript.RegisterStartupScript(this.GetType(),
            "ExclusaoContas", "alert('Registro excluído com sucesso');", true);

            ConsultaGeral();
        }


        if (e.CommandName == "Processar")
        {
            // Selecionar a linha do SPLIT

            SqlConnection mySelCadastro = new SqlConnection(Funcoes.conexao());
            mySelCadastro.Open();
            SqlCommand cmdSelCadastro = new SqlCommand("dbo.stp_transacoes_split_ins", mySelCadastro);
            cmdSelCadastro.CommandType = CommandType.StoredProcedure;
            cmdSelCadastro.Parameters.Add("@flg_operacao", SqlDbType.Char).Value = "S";
            cmdSelCadastro.Parameters.Add("@COD_ID_PESSOA_LICENCIADO", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["LICENCIADO"].ToString());
            cmdSelCadastro.Parameters.Add("@COD_ID", SqlDbType.Int).Value = Funcoes.strToInt(e.CommandArgument.ToString());
            cmdSelCadastro.Parameters.Add("@NOM_CODE", SqlDbType.VarChar).Value = txtIDTransacao.Text.ToString();
            SqlDataReader ReaderCadastro = cmdSelCadastro.ExecuteReader();
            while (ReaderCadastro.Read())
            {
                string jsonSplit = "";
                if (Funcoes.strToDouble(ReaderCadastro["NUM_VALOR"].ToString())>0)
                {
                    ZoopSplit dsplit = new ZoopSplit()
                    {
                        amount = Funcoes.strToInt((Funcoes.strToDouble(ReaderCadastro["NUM_VALOR"].ToString()) * 100).ToString()),
                        charge_processing_fee = false,
                        recipient = ReaderCadastro["NOM_ID_CADASTRO"].ToString()
                    };
                    jsonSplit = JsonConvert.SerializeObject(dsplit);
                }
                if (Funcoes.strToDouble(ReaderCadastro["NUM_PERCENTUAL"].ToString()) > 0)
                {
                    ZoopSplit dsplit = new ZoopSplit()
                    {
                        percentage = Funcoes.strToDouble(ReaderCadastro["NUM_PERCENTUAL"].ToString()),
                        charge_processing_fee = false,
                        recipient = ReaderCadastro["NOM_ID_CADASTRO"].ToString()
                    };
                    jsonSplit = JsonConvert.SerializeObject(dsplit);
                }

                zoop.HttpResponseResult resultado = zoop.TransacaoSplitPresencial(jsonSplit, txtIDTransacao.Text.ToString());

                if ((resultado.StatusCode == 200) || (resultado.StatusCode == 201) || (resultado.StatusCode == 202))
                {
                    string jsonRetornoSplit = resultado.Content;
                    
                    txtRetorno.Text = jsonRetornoSplit;

                    JObject oSplit = JObject.Parse(jsonRetornoSplit);


                    SqlConnection connInsCons = new SqlConnection(Funcoes.conexao());
                    connInsCons.Open();
                    SqlCommand cmdInsCons = new SqlCommand("dbo.stp_transacoes_split_ins", connInsCons);
                    cmdInsCons.CommandType = CommandType.StoredProcedure;
                    cmdInsCons.Parameters.Add("@flg_operacao", SqlDbType.Char).Value = 'U';
                    cmdInsCons.Parameters.Add("@COD_ID_PESSOA_LICENCIADO", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["LICENCIADO"].ToString());
                    cmdInsCons.Parameters.Add("@COD_ID", SqlDbType.Int).Value = Funcoes.strToInt(e.CommandArgument.ToString());
                    cmdInsCons.Parameters.Add("@NOM_CODE", SqlDbType.VarChar).Value = txtIDTransacao.Text.ToString();
                    cmdInsCons.Parameters.Add("@NOM_CODE_SPLIT", SqlDbType.VarChar).Value = oSplit["id"].ToString();
                    cmdInsCons.Parameters.Add("@NUM_VALOR_SPLIT", SqlDbType.Float).Value = Funcoes.strToDouble(oSplit["receivable_amount"].ToString());
                    cmdInsCons.ExecuteNonQuery();
                    connInsCons.Close();
                    connInsCons.Dispose();
                }
            }
        }

        if (e.CommandName == "Cancelar")
        {
            txtRetorno.Text = "FORA";
            SqlConnection mySelCadastro = new SqlConnection(Funcoes.conexao());
            mySelCadastro.Open();
            SqlCommand cmdSelCadastro = new SqlCommand("dbo.stp_transacoes_split_ins", mySelCadastro);
            cmdSelCadastro.CommandType = CommandType.StoredProcedure;
            cmdSelCadastro.Parameters.Add("@flg_operacao", SqlDbType.Char).Value = "S";

            cmdSelCadastro.Parameters.Add("@COD_ID_PESSOA_LICENCIADO", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["LICENCIADO"].ToString());
            cmdSelCadastro.Parameters.Add("@COD_ID", SqlDbType.Int).Value = Funcoes.strToInt(e.CommandArgument.ToString());
            cmdSelCadastro.Parameters.Add("@NOM_CODE", SqlDbType.VarChar).Value = txtIDTransacao.Text.ToString();
            
            SqlDataReader ReaderCadastro = cmdSelCadastro.ExecuteReader();
            while (ReaderCadastro.Read())
            {
                txtRetorno.Text = "DENTRO ANTES";
                zoop.HttpResponseResult resultado = zoop.CancelarTransacaoSplitPresencial(ReaderCadastro["NOM_CODE_SPLIT"].ToString(), txtIDTransacao.Text.ToString());
                txtRetorno.Text = resultado.Content.ToString();

                if ((resultado.StatusCode == 200) || (resultado.StatusCode == 201) || (resultado.StatusCode == 202))
                {
                    txtRetorno.Text = "DENTRO DEPOIS";
                    string jsonRetornoSplit = resultado.Content;

                    txtRetorno.Text = jsonRetornoSplit;

                    SqlConnection connInsCons = new SqlConnection(Funcoes.conexao());
                    connInsCons.Open();
                    SqlCommand cmdInsCons = new SqlCommand("dbo.stp_transacoes_split_ins", connInsCons);
                    cmdInsCons.CommandType = CommandType.StoredProcedure;
                    cmdInsCons.Parameters.Add("@flg_operacao", SqlDbType.Char).Value = 'U';
                    cmdInsCons.Parameters.Add("@COD_ID_PESSOA_LICENCIADO", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["LICENCIADO"].ToString());
                    cmdInsCons.Parameters.Add("@COD_ID", SqlDbType.Int).Value = Funcoes.strToInt(e.CommandArgument.ToString());
                    cmdInsCons.Parameters.Add("@NOM_CODE", SqlDbType.VarChar).Value = txtIDTransacao.Text.ToString();
                    cmdInsCons.Parameters.Add("@NOM_CODE_SPLIT", SqlDbType.VarChar).Value = "";
                    cmdInsCons.Parameters.Add("@NUM_VALOR_SPLIT", SqlDbType.Float).Value = 0;
                    cmdInsCons.ExecuteNonQuery();
                    connInsCons.Close();
                    connInsCons.Dispose();
                }

            }
        }
    }
    protected void ddlAdquirentes_SelectedIndexChanged(object sender, EventArgs e)
    {
        CarregaEstabelecimentos();
    }
}