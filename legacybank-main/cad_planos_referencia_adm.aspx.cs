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

public partial class cad_planos_referencia_adm : System.Web.UI.Page
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
            if (Funcoes.strToInt(sid_id) != 0)
            {
                ConsultaFicha();
                ConsultaGeral();
            }
        }

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
            ddlModelo.SelectedValue = ReaderCadastro["FLG_MODELO_PLANO_REFERENCIA"].ToString();
            txtCodigo.Text = ReaderCadastro["COD_ID_PLANO_REFERENCIA"].ToString();
            txtNome.Text = ReaderCadastro["NOM_PLANO_REFERENCIA"].ToString();
            txtDescricao.Text = ReaderCadastro["DES_PLANO_REFERENCIA"].ToString();
            txtTransacao.Text = ReaderCadastro["NOM_INVOICE_PLANO_REFERENCIA"].ToString();
            txtPeriodicidade.Text = ReaderCadastro["NOM_PERIODICIDADE"].ToString();
            ddlAtivo.SelectedValue = ReaderCadastro["FLG_ATIVO"].ToString();
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
        cmdInsCons.Parameters.Add("@FLG_MODELO_PLANO_REFERENCIA", SqlDbType.Char).Value = ddlModelo.SelectedValue.ToString();
        cmdInsCons.Parameters.Add("@FLG_ATIVO", SqlDbType.Char).Value = ddlAtivo.SelectedValue.ToString();
        cmdInsCons.Parameters.Add("@COD_ID_PLANO_REFERENCIA", SqlDbType.VarChar).Value = txtCodigo.Text.ToString();
        cmdInsCons.Parameters.Add("@NOM_PLANO_REFERENCIA", SqlDbType.VarChar).Value = txtNome.Text.ToString();
        cmdInsCons.Parameters.Add("@NOM_INVOICE_PLANO_REFERENCIA", SqlDbType.VarChar).Value = txtTransacao.Text.ToString();
        cmdInsCons.Parameters.Add("@NOM_PERIODICIDADE", SqlDbType.VarChar).Value = txtPeriodicidade.Text.ToString();
        cmdInsCons.Parameters.Add("@DES_PLANO_REFERENCIA", SqlDbType.Text).Value = txtDescricao.Text.ToString();
        cmdInsCons.ExecuteNonQuery();
        connInsCons.Close();
        connInsCons.Dispose();

        ClientScript.RegisterStartupScript(this.GetType(),
            "Alerta", "alert('Dados gravados com sucesso'); opener.PostBackOnMainPage(); window.close(); ", true);
    }
    protected void btnCancelar_Click(object sender, EventArgs e)
    {
        ClientScript.RegisterStartupScript(this.GetType(),
            "Alerta", "opener.PostBackOnMainPage(); window.close(); ", true);
    }

    protected void btnImportar_Click(object sender, EventArgs e)
    {

        try
        {
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
                cmdInsCons.Parameters.Add("@NOM_BANDEIRA", SqlDbType.VarChar).Value = o["fee_details"][i]["card_brand"].ToString();
                cmdInsCons.Parameters.Add("@NUM_VALOR_PERCENTUAL", SqlDbType.Float).Value = Funcoes.strToDouble(o["fee_details"][i]["percent_amount"].ToString()) / 100;
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
    protected void btnGravar_Click(object sender, EventArgs e)
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
            cmdInsCons.Parameters.Add("@NUM_VALOR_MARKUP", SqlDbType.Float).Value = Funcoes.strToDouble(((TextBox)itemP.FindControl("txMarkup")).Text.ToString());
            cmdInsCons.ExecuteNonQuery();
            connInsCons.Close();
            connInsCons.Dispose();

        }

        ConsultaGeral();
    }
}