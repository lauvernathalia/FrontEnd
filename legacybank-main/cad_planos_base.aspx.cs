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


public partial class cad_planos_base : System.Web.UI.Page
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

        // Verifica se o usuário esta autenticado
        if (HttpContext.Current.Session.Count <= 0)
        {
            Response.Redirect("login.aspx");
            FormsAuthentication.SignOut();
        }
        if (!IsPostBack)
        {

            Funcoes.GravaAuditoria(Funcoes.strToInt(sid_id), Request.ServerVariables["REMOTE_ADDR"].ToString(), "Cadastro - Planos Base", "Edição");


            CarregaAdquirentes();
            
            // Carrega Planos De Referência da Adquirente
            SqlConnection myPlanosRef = new SqlConnection(Funcoes.conexao());
            myPlanosRef.Open();
            SqlCommand cmdPlanosRef = new SqlCommand("dbo.stp_pessoas_fj_planos_referencia_ins", myPlanosRef);
            cmdPlanosRef.CommandType = CommandType.StoredProcedure;
            cmdPlanosRef.Parameters.Add("@flg_operacao", SqlDbType.VarChar).Value = "X";
            cmdPlanosRef.Parameters.Add("@COD_ID_PESSOA_LICENCIADO", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["LICENCIADO"].ToString());
            cmdPlanosRef.Parameters.Add("@COD_ID_PESSOAS_FJ", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["PESSOA"].ToString());
            cmdPlanosRef.Parameters.Add("@FLG_MODELO_PLANO_REFERENCIA", SqlDbType.Char).Value = ddlAdquirentes.SelectedValue.ToString();
            cmdPlanosRef.Parameters.Add("@FLG_ACESSO", SqlDbType.Char).Value = "S";
            SqlDataAdapter drPlanosRef = new SqlDataAdapter();
            drPlanosRef.SelectCommand = cmdPlanosRef;
            DataSet dsPlanosRef = new DataSet();
            drPlanosRef.Fill(dsPlanosRef, "PESSOAS_FJ_PLANOS_REFERENCIA");
            ddlPlanosReferencia.DataTextField = "NOM_PLANO_REFERENCIA";
            ddlPlanosReferencia.DataValueField = "COD_ID_PLANO_REFERENCIA";
            ddlPlanosReferencia.DataSource = dsPlanosRef.Tables["PESSOAS_FJ_PLANOS_REFERENCIA"].DefaultView;
            ddlPlanosReferencia.DataBind();
            ddlPlanosReferencia.Items.Insert(0, new ListItem("", "0"));


            if (Funcoes.strToInt(sid_id) != 0)
            {
                ConsultaFicha();
                ConsultaGeral();
            }
        }

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
        cmdAdquirentes.Parameters.Add("@COD_ID_PESSOAS_FJ", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["PESSOA"].ToString());
        cmdAdquirentes.Parameters.Add("@FLG_ATIVO", SqlDbType.Char).Value = "S";
        cmdAdquirentes.Parameters.Add("@FLG_ACESSO", SqlDbType.Char).Value = "S";
        SqlDataAdapter drAdquirentes = new SqlDataAdapter();
        drAdquirentes.SelectCommand = cmdAdquirentes;
        DataSet dsAdquirentes = new DataSet();
        drAdquirentes.Fill(dsAdquirentes, "PESSOAS_FJ_INTEGRACOES");

        ddlAdquirentes.DataTextField = "NOM_FANTASIA_INTEGRACAO";
        ddlAdquirentes.DataValueField = "FLG_INTEGRACAO";
        ddlAdquirentes.DataSource = dsAdquirentes.Tables["PESSOAS_FJ_INTEGRACOES"].DefaultView;
        ddlAdquirentes.DataBind();

    }


    private void ConsultaFicha()
    {
        SqlConnection mySelCadastro = new SqlConnection(Funcoes.conexao());
        mySelCadastro.Open();
        SqlCommand cmdSelCadastro = new SqlCommand("dbo.stp_planos_referencia_ins", mySelCadastro);
        cmdSelCadastro.CommandType = CommandType.StoredProcedure;
        cmdSelCadastro.Parameters.Add("@flg_operacao", SqlDbType.Char).Value = "S";
        cmdSelCadastro.Parameters.Add("@COD_ID", SqlDbType.Int).Value = Funcoes.strToInt(sid_id.ToString());
        cmdSelCadastro.Parameters.Add("@COD_ID_PESSOA_LICENCIADO", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["LICENCIADO"].ToString());
        SqlDataReader ReaderCadastro = cmdSelCadastro.ExecuteReader();
        while (ReaderCadastro.Read())
        {
            lblID.Text = ReaderCadastro["COD_ID"].ToString();
            ddlAdquirentes.SelectedValue = ReaderCadastro["FLG_MODELO_PLANO_REFERENCIA"].ToString();
            txtCodigo.Text = ReaderCadastro["COD_ID_PLANO_REFERENCIA"].ToString();
            txtNome.Text = ReaderCadastro["NOM_PLANO_REFERENCIA"].ToString();
            txtDescricao.Text = ReaderCadastro["DES_PLANO_REFERENCIA"].ToString();
            txtTransacao.Text = ReaderCadastro["NOM_INVOICE_PLANO_REFERENCIA"].ToString();
            txtPeriodicidade.Text = ReaderCadastro["NOM_PERIODICIDADE"].ToString();
            ddlAtivo.SelectedValue = ReaderCadastro["FLG_ATIVO"].ToString();

            ddlAdquirentes_SelectedIndexChanged(null, null);
            ddlPlanosReferencia.SelectedValue = ReaderCadastro["COD_ID_PLANO_REFERENCIA"].ToString();
        }

    }

    private void ConsultaGeral()
    {
        //dtgConsulta.Visible = true;
        SqlConnection myConsulta = new SqlConnection(Funcoes.conexao());
        myConsulta.Open();
        SqlDataAdapter SDAConsulta = new SqlDataAdapter("dbo.stp_planos_referencia_taxas_ins", myConsulta);
        SDAConsulta.SelectCommand.CommandType = CommandType.StoredProcedure;
        SDAConsulta.SelectCommand.Parameters.Add("@flg_operacao", SqlDbType.Char).Value = "R";
        SDAConsulta.SelectCommand.Parameters.Add("@COD_ID_PESSOA_LICENCIADO", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["LICENCIADO"].ToString());
        SDAConsulta.SelectCommand.Parameters.Add("@COD_ID_PLANOS_REFERENCIA", SqlDbType.Int).Value = Funcoes.strToInt(lblID.Text.ToString());

        DataSet dsConsulta = new DataSet();
        SDAConsulta.Fill(dsConsulta, "PLANOS_REFERENCIA_TAXAS");
        rptConsulta.DataSource = dsConsulta.Tables["PLANOS_REFERENCIA_TAXAS"].DefaultView;
        rptConsulta.DataBind();
        myConsulta.Close(); myConsulta.Dispose();

    }

    protected void btnSalvar_Click(object sender, EventArgs e)
    {

        Funcoes.GravaAuditoria(Funcoes.strToInt(sid_id), Request.ServerVariables["REMOTE_ADDR"].ToString(), "Cadastro - Planos Base", "Gravar");
        
        SqlConnection connInsCons = new SqlConnection(Funcoes.conexao());
        connInsCons.Open();
        SqlCommand cmdInsCons = new SqlCommand("dbo.stp_planos_referencia_ins", connInsCons);
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
        cmdInsCons.Parameters.Add("@FLG_MODELO_PLANO_REFERENCIA", SqlDbType.Char).Value = ddlAdquirentes.SelectedValue.ToString();
        cmdInsCons.Parameters.Add("@FLG_ATIVO", SqlDbType.Char).Value = ddlAtivo.SelectedValue.ToString();

        cmdInsCons.Parameters.Add("@COD_ID_PLANO_REFERENCIA", SqlDbType.VarChar).Value = ddlPlanosReferencia.SelectedValue.ToString();//txtCodigo.Text.ToString();

        cmdInsCons.Parameters.Add("@NOM_PLANO_REFERENCIA", SqlDbType.VarChar).Value = txtNome.Text.ToString();
        cmdInsCons.Parameters.Add("@NOM_INVOICE_PLANO_REFERENCIA", SqlDbType.VarChar).Value = txtTransacao.Text.ToString();
        cmdInsCons.Parameters.Add("@NOM_PERIODICIDADE", SqlDbType.VarChar).Value = txtPeriodicidade.Text.ToString();
        cmdInsCons.Parameters.Add("@DES_PLANO_REFERENCIA", SqlDbType.Text).Value = txtDescricao.Text.ToString();
        cmdInsCons.ExecuteNonQuery();
        connInsCons.Close();
        connInsCons.Dispose();

        SalvarTaxasPlanoReferencia();

        ClientScript.RegisterStartupScript(this.GetType(),
            "Alerta", "alert('Dados gravados com sucesso'); opener.PostBackOnMainPage(); window.close(); ", true);
    }

    private void SalvarTaxasPlanoReferencia()
    {
        foreach (RepeaterItem itemP in rptConsulta.Items)
        {
            SqlConnection connInsCons = new SqlConnection(Funcoes.conexao());
            connInsCons.Open();
            SqlCommand cmdInsCons = new SqlCommand("dbo.stp_planos_referencia_taxas_ins", connInsCons);
            cmdInsCons.CommandType = CommandType.StoredProcedure;
            cmdInsCons.Parameters.Add("@flg_operacao", SqlDbType.Char).Value = 'U';
            cmdInsCons.Parameters.Add("@COD_ID_PESSOA_LICENCIADO", SqlDbType.Int).Value = Funcoes.strToInt(((TextBox)itemP.FindControl("idLicenciado")).Text.ToString());
            cmdInsCons.Parameters.Add("@COD_ID_PLANOS_REFERENCIA", SqlDbType.Int).Value = Funcoes.strToInt(((TextBox)itemP.FindControl("idPlanoReferencia")).Text.ToString());
            cmdInsCons.Parameters.Add("@COD_ID_BANDEIRAS", SqlDbType.VarChar).Value = Funcoes.strToInt(((TextBox)itemP.FindControl("idBandeira")).Text.ToString());

            cmdInsCons.Parameters.Add("@NOM_TIPO_PAGAMENTO", SqlDbType.VarChar).Value = ((TextBox)itemP.FindControl("TipoPagamento")).Text.ToString();
            cmdInsCons.Parameters.Add("@NUM_PARCELAS", SqlDbType.Int).Value = Funcoes.strToInt(((TextBox)itemP.FindControl("idParcelas")).Text.ToString());
            cmdInsCons.Parameters.Add("@NOM_MODO_CAPTURA", SqlDbType.VarChar).Value = ((TextBox)itemP.FindControl("ModoCaptura")).Text.ToString();
            cmdInsCons.Parameters.Add("@NUM_VALOR_MARKUP", SqlDbType.Float).Value = Funcoes.strToDouble(((TextBox)itemP.FindControl("txtMarkup")).Text.ToString());

            cmdInsCons.Parameters.Add("@NUM_VALOR_REBATE", SqlDbType.Float).Value = Funcoes.strToDouble(((TextBox)itemP.FindControl("txtRebate")).Text.ToString());

            cmdInsCons.ExecuteNonQuery();
            connInsCons.Close();
            connInsCons.Dispose();

        }
    }


    protected void btnCancelar_Click(object sender, EventArgs e)
    {
        ClientScript.RegisterStartupScript(this.GetType(),
            "Alerta", "opener.PostBackOnMainPage(); window.close(); ", true);
    }

    protected void btnImportar_Click(object sender, EventArgs e)
    {
        if (ddlAdquirentes.SelectedValue.ToString() == "Z")
        {
            try
            {
                // Apagar primeiro as taxas




                JObject o = JObject.Parse(zoop.planos_referencia(txtCodigo.Text.ToString()).ToString());

                for (int i = 0; i < o["fee_details"].Count(); i++)
                {
                    SqlConnection connInsCons = new SqlConnection(Funcoes.conexao());
                    connInsCons.Open();
                    SqlCommand cmdInsCons = new SqlCommand("dbo.stp_planos_referencia_taxas_ins", connInsCons);
                    cmdInsCons.CommandType = CommandType.StoredProcedure;
                    cmdInsCons.Parameters.Add("@flg_operacao", SqlDbType.Char).Value = 'I';
                    cmdInsCons.Parameters.Add("@COD_ID_PESSOA_LICENCIADO", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["LICENCIADO"].ToString());
                    cmdInsCons.Parameters.Add("@COD_ID_PLANOS_REFERENCIA", SqlDbType.Int).Value = Funcoes.strToInt(lblID.Text.ToString());
                    if (o["fee_details"][i]["card_brand"].ToString().Trim() != "")
                    {
                        cmdInsCons.Parameters.Add("@NOM_BANDEIRA", SqlDbType.VarChar).Value = o["fee_details"][i]["card_brand"].ToString();
                    }
                    else
                    {
                        if (o["fee_details"][i]["payment_type"].ToString().Trim() == "credit")
                        {
                            cmdInsCons.Parameters.Add("@NOM_BANDEIRA", SqlDbType.VarChar).Value = "OUTROS CRÉDITO";
                        }
                        else
                        {
                            cmdInsCons.Parameters.Add("@NOM_BANDEIRA", SqlDbType.VarChar).Value = o["fee_details"][i]["payment_type"].ToString();
                        }
                    }
                    cmdInsCons.Parameters.Add("@NUM_VALOR_PERCENTUAL", SqlDbType.Float).Value = Funcoes.strToDouble(o["fee_details"][i]["percent_amount"].ToString()) / 100;
                    cmdInsCons.Parameters.Add("@NUM_VALOR", SqlDbType.Float).Value = Funcoes.strToDouble(o["fee_details"][i]["dollar_amount"].ToString()) / 100;
                    cmdInsCons.Parameters.Add("@NOM_MOEDA", SqlDbType.VarChar).Value = o["fee_details"][i]["currency"].ToString();
                    cmdInsCons.Parameters.Add("@NOM_TIPO_PAGAMENTO", SqlDbType.VarChar).Value = o["fee_details"][i]["payment_type"].ToString();
                    cmdInsCons.Parameters.Add("@NUM_PARCELAS", SqlDbType.Int).Value = Funcoes.strToInt(o["fee_details"][i]["number_installments"].ToString());
                    cmdInsCons.Parameters.Add("@NOM_MODO_CAPTURA", SqlDbType.VarChar).Value = o["fee_details"][i]["capture_mode"].ToString();
                    cmdInsCons.Parameters.Add("@NOM_ORIGEM", SqlDbType.VarChar).Value = o["fee_details"][i]["source"].ToString();
                    cmdInsCons.Parameters.Add("@FLG_PRE_PAGO", SqlDbType.Char).Value = (o["fee_details"][i]["prepaid"].ToString() == "True") ? "S" : "N";
                    cmdInsCons.Parameters.Add("@NOM_TIPO", SqlDbType.VarChar).Value = o["fee_details"][i]["type"].ToString();
                    cmdInsCons.ExecuteNonQuery();
                    connInsCons.Close();
                    connInsCons.Dispose();
                }

                ClientScript.RegisterStartupScript(this.GetType(),
            "Alerta", "alert('Taxas Importadas com Sucesso'); ", true);

            }
            catch
            {
                ClientScript.RegisterStartupScript(this.GetType(),
            "Erro", "alert('Alguma coisa deu errado!');", true);
            }
            finally
            {
                ConsultaGeral();
            }
        }
        if (ddlAdquirentes.SelectedValue.ToString() == "C")
        {

        }
    }
    protected void ddlAdquirentes_SelectedIndexChanged(object sender, EventArgs e)
    {
        // Carrega Planos De Referência da Adquirente
        SqlConnection myPlanosRef = new SqlConnection(Funcoes.conexao());
        myPlanosRef.Open();
        SqlCommand cmdPlanosRef = new SqlCommand("dbo.stp_pessoas_fj_planos_referencia_ins", myPlanosRef);
        cmdPlanosRef.CommandType = CommandType.StoredProcedure;
        cmdPlanosRef.Parameters.Add("@flg_operacao", SqlDbType.VarChar).Value = "X";
        cmdPlanosRef.Parameters.Add("@COD_ID_PESSOA_LICENCIADO", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["LICENCIADO"].ToString());
        cmdPlanosRef.Parameters.Add("@COD_ID_PESSOAS_FJ", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["PESSOA"].ToString());
        cmdPlanosRef.Parameters.Add("@FLG_MODELO_PLANO_REFERENCIA", SqlDbType.Char).Value = ddlAdquirentes.SelectedValue.ToString();
        cmdPlanosRef.Parameters.Add("@FLG_ACESSO", SqlDbType.Char).Value = "S";
        SqlDataAdapter drPlanosRef = new SqlDataAdapter();
        drPlanosRef.SelectCommand = cmdPlanosRef;
        DataSet dsPlanosRef = new DataSet();
        drPlanosRef.Fill(dsPlanosRef, "PESSOAS_FJ_PLANOS_REFERENCIA");
        ddlPlanosReferencia.DataTextField = "NOM_PLANO_REFERENCIA";
        ddlPlanosReferencia.DataValueField = "COD_ID_PLANO_REFERENCIA";
        ddlPlanosReferencia.DataSource = dsPlanosRef.Tables["PESSOAS_FJ_PLANOS_REFERENCIA"].DefaultView;
        ddlPlanosReferencia.DataBind();
        ddlPlanosReferencia.Items.Insert(0, new ListItem("", "0"));
    }

    protected void btnZerar_Click(object sender, EventArgs e)
    {
        SqlConnection connInsCons = new SqlConnection(Funcoes.conexao());
        connInsCons.Open();
        SqlCommand cmdInsCons = new SqlCommand("dbo.stp_planos_referencia_taxas_ins", connInsCons);
        cmdInsCons.CommandType = CommandType.StoredProcedure;
        cmdInsCons.Parameters.Add("@flg_operacao", SqlDbType.Char).Value = 'D';
        cmdInsCons.Parameters.Add("@COD_ID_PESSOA_LICENCIADO", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["LICENCIADO"].ToString());
        cmdInsCons.Parameters.Add("@COD_ID_PLANOS_REFERENCIA", SqlDbType.Int).Value = Funcoes.strToInt(lblID.Text.ToString());
        cmdInsCons.ExecuteNonQuery();
        connInsCons.Close();
        connInsCons.Dispose();

        ConsultaGeral();


    }
}