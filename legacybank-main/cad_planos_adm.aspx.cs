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
using System.Data.SqlClient;


using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

using System.IO;
using System.Text;

using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;

using System.ComponentModel;


public partial class cad_planos_adm : System.Web.UI.Page
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
            SqlConnection myPlanos = new SqlConnection(Funcoes.conexao());
            myPlanos.Open();
            SqlCommand cmdPlanos = new SqlCommand("dbo.stp_planos_ins", myPlanos);
            cmdPlanos.CommandType = CommandType.StoredProcedure;
            cmdPlanos.Parameters.Add("@flg_operacao", SqlDbType.VarChar).Value = "L";
            cmdPlanos.Parameters.Add("@COD_ID_PESSOA_LICENCIADO", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["LICENCIADO"].ToString());
            cmdPlanos.Parameters.Add("@FLG_ATIVO", SqlDbType.Char).Value = "S";
            cmdPlanos.Parameters.Add("@FLG_TIPO_PLANO", SqlDbType.Char).Value = "B";
            SqlDataAdapter drPlanos = new SqlDataAdapter();
            drPlanos.SelectCommand = cmdPlanos;
            DataSet dsPlanos = new DataSet();
            drPlanos.Fill(dsPlanos, "PLANOS");
            ddlPlanos.DataTextField = "NOM_TITULO_PLANO";
            ddlPlanos.DataValueField = "COD_ID";
            ddlPlanos.DataSource = dsPlanos.Tables["PLANOS"].DefaultView;
            ddlPlanos.DataBind();
            ddlPlanos.Items.Insert(0, new ListItem("", "0"));

            SqlConnection myPlanosRef = new SqlConnection(Funcoes.conexao());
            myPlanosRef.Open();
            SqlCommand cmdPlanosRef = new SqlCommand("dbo.stp_planos_referencia_ins", myPlanosRef);
            cmdPlanosRef.CommandType = CommandType.StoredProcedure;
            cmdPlanosRef.Parameters.Add("@flg_operacao", SqlDbType.VarChar).Value = "L";
            cmdPlanosRef.Parameters.Add("@COD_ID_PESSOA_LICENCIADO", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["LICENCIADO"].ToString());
            cmdPlanosRef.Parameters.Add("@FLG_MODELO_PLANO_REFERENCIA", SqlDbType.Char).Value = " ";
            cmdPlanosRef.Parameters.Add("@FLG_ATIVO", SqlDbType.Char).Value = "S";
            SqlDataAdapter drPlanosRef = new SqlDataAdapter();
            drPlanosRef.SelectCommand = cmdPlanosRef;
            DataSet dsPlanosRef = new DataSet();
            drPlanosRef.Fill(dsPlanosRef, "PLANOS_REFERENCIA");
            ddlPlanosReferencia.DataTextField = "NOM_PLANO_REFERENCIA";
            ddlPlanosReferencia.DataValueField = "COD_ID";
            ddlPlanosReferencia.DataSource = dsPlanosRef.Tables["PLANOS_REFERENCIA"].DefaultView;
            ddlPlanosReferencia.DataBind();
            ddlPlanosReferencia.Items.Insert(0, new ListItem("", "0"));


            if (Funcoes.strToInt(sid_id) != 0)
            {
                ConsultaFicha();
                
                dvTaxas.Visible = true;
                dvParcelas.Visible = true;
                dvTaxasOnline.Visible = true;
                dvParcelasOnline.Visible = true;


                ConsultaGeral();
                if (ddlModelo.SelectedValue.ToString().Trim() == "Z") { ConsultaGeralOnline(); } 
                
                if (ddlAntecipado.SelectedValue.ToString() == "S")
                {
                    dvTaxas.Visible = false;
                    dvParcelas.Visible = true;
                    dvTaxasOnline.Visible = false;
                    dvParcelasOnline.Visible = true;
                }
                else
                {
                    dvTaxas.Visible = true;
                    dvParcelas.Visible = false;
                    dvTaxasOnline.Visible = true;
                    dvParcelasOnline.Visible = false;
                }

            }

            if (ddlModelo.SelectedValue.ToString().Trim() == "P") { tab_online.Visible = false; }
            if (ddlModelo.SelectedValue.ToString().Trim() == "Z") { tab_online.Visible = true; } 

        }

    }

    private void ConsultaGeral()
    {
        //dtgConsulta.Visible = true;
        SqlConnection myConsulta = new SqlConnection(Funcoes.conexao());
        myConsulta.Open();
        SqlDataAdapter SDAConsulta = new SqlDataAdapter("dbo.stp_planos_ins", myConsulta);
        SDAConsulta.SelectCommand.CommandType = CommandType.StoredProcedure;
        SDAConsulta.SelectCommand.Parameters.Add("@flg_operacao", SqlDbType.Char).Value = "B";
        SDAConsulta.SelectCommand.Parameters.Add("@COD_ID_PESSOA_LICENCIADO", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["LICENCIADO"].ToString());
        SDAConsulta.SelectCommand.Parameters.Add("@COD_ID", SqlDbType.Int).Value = Funcoes.strToInt(sid_id.ToString());
        SDAConsulta.SelectCommand.Parameters.Add("@FLG_PRESENCIAL_ONLINE", SqlDbType.Char).Value = "P";
        DataSet dsConsulta = new DataSet();
        SDAConsulta.Fill(dsConsulta, "BANDEIRAS");
        rptConsulta.DataSource = dsConsulta.Tables["BANDEIRAS"].DefaultView;
        rptConsulta.DataBind();
        myConsulta.Close(); myConsulta.Dispose();

        SqlConnection myConsultaParcelas = new SqlConnection(Funcoes.conexao());
        myConsultaParcelas.Open();
        SqlDataAdapter SDAConsultaParcelas = new SqlDataAdapter("dbo.stp_planos_ins", myConsultaParcelas);
        SDAConsultaParcelas.SelectCommand.CommandType = CommandType.StoredProcedure;
        SDAConsultaParcelas.SelectCommand.Parameters.Add("@flg_operacao", SqlDbType.Char).Value = "P";
        SDAConsultaParcelas.SelectCommand.Parameters.Add("@COD_ID_PESSOA_LICENCIADO", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["LICENCIADO"].ToString());
        SDAConsultaParcelas.SelectCommand.Parameters.Add("@COD_ID", SqlDbType.Int).Value = Funcoes.strToInt(sid_id.ToString());
        SDAConsultaParcelas.SelectCommand.Parameters.Add("@FLG_PRESENCIAL_ONLINE", SqlDbType.Char).Value = "P";
        DataSet dsConsultaParcelas = new DataSet();
        SDAConsultaParcelas.Fill(dsConsultaParcelas, "PLANOS_PARCELAS");
        rptConsultaParcelas.DataSource = dsConsultaParcelas.Tables["PLANOS_PARCELAS"].DefaultView;
        rptConsultaParcelas.DataBind();
        myConsultaParcelas.Close(); myConsultaParcelas.Dispose();
    }

    private void ConsultaGeralOnline()
    {
        //dtgConsulta.Visible = true;
        SqlConnection myConsulta = new SqlConnection(Funcoes.conexao());
        myConsulta.Open();
        SqlDataAdapter SDAConsulta = new SqlDataAdapter("dbo.stp_planos_ins", myConsulta);
        SDAConsulta.SelectCommand.CommandType = CommandType.StoredProcedure;
        SDAConsulta.SelectCommand.Parameters.Add("@flg_operacao", SqlDbType.Char).Value = "B";
        SDAConsulta.SelectCommand.Parameters.Add("@COD_ID_PESSOA_LICENCIADO", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["LICENCIADO"].ToString());
        SDAConsulta.SelectCommand.Parameters.Add("@COD_ID", SqlDbType.Int).Value = Funcoes.strToInt(sid_id.ToString());
        SDAConsulta.SelectCommand.Parameters.Add("@FLG_PRESENCIAL_ONLINE", SqlDbType.Char).Value = "O";
        DataSet dsConsulta = new DataSet();
        SDAConsulta.Fill(dsConsulta, "BANDEIRAS");
        rptConsultaOnline.DataSource = dsConsulta.Tables["BANDEIRAS"].DefaultView;
        rptConsultaOnline.DataBind();
        myConsulta.Close(); myConsulta.Dispose();

        SqlConnection myConsultaParcelas = new SqlConnection(Funcoes.conexao());
        myConsultaParcelas.Open();
        SqlDataAdapter SDAConsultaParcelas = new SqlDataAdapter("dbo.stp_planos_ins", myConsultaParcelas);
        SDAConsultaParcelas.SelectCommand.CommandType = CommandType.StoredProcedure;
        SDAConsultaParcelas.SelectCommand.Parameters.Add("@flg_operacao", SqlDbType.Char).Value = "P";
        SDAConsultaParcelas.SelectCommand.Parameters.Add("@COD_ID_PESSOA_LICENCIADO", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["LICENCIADO"].ToString());
        SDAConsultaParcelas.SelectCommand.Parameters.Add("@COD_ID", SqlDbType.Int).Value = Funcoes.strToInt(sid_id.ToString());
        SDAConsultaParcelas.SelectCommand.Parameters.Add("@FLG_PRESENCIAL_ONLINE", SqlDbType.Char).Value = "O";
        DataSet dsConsultaParcelas = new DataSet();
        SDAConsultaParcelas.Fill(dsConsultaParcelas, "PLANOS_PARCELAS");
        rptConsultaParcelasOnline.DataSource = dsConsultaParcelas.Tables["PLANOS_PARCELAS"].DefaultView;
        rptConsultaParcelasOnline.DataBind();
        myConsultaParcelas.Close(); myConsultaParcelas.Dispose();
    }


    private void ConsultaFicha()
    {
        SqlConnection mySelCadastro = new SqlConnection(Funcoes.conexao());
        mySelCadastro.Open();
        SqlCommand cmdSelCadastro = new SqlCommand("dbo.stp_planos_ins", mySelCadastro);
        cmdSelCadastro.CommandType = CommandType.StoredProcedure;
        cmdSelCadastro.Parameters.Add("@flg_operacao", SqlDbType.Char).Value = "S";
        cmdSelCadastro.Parameters.Add("@COD_ID", SqlDbType.Int).Value = Funcoes.strToInt(sid_id.ToString());
        cmdSelCadastro.Parameters.Add("@COD_ID_PESSOA_LICENCIADO", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["LICENCIADO"].ToString());
        SqlDataReader ReaderCadastro = cmdSelCadastro.ExecuteReader();
        while (ReaderCadastro.Read())
        {
            lblID.Text = ReaderCadastro["COD_ID"].ToString();
            ddlModelo.SelectedValue = ReaderCadastro["FLG_MODELO_PLANO"].ToString();
            txtNome.Text = ReaderCadastro["NOM_TITULO_PLANO"].ToString();
            txtDescricao.Text = ReaderCadastro["DES_PLANO"].ToString();

            ddlAtivo.SelectedValue = ReaderCadastro["FLG_ATIVO"].ToString();
            ddlVisivel.SelectedValue = ReaderCadastro["FLG_VISIVEL"].ToString();
            ddlAntecipado.SelectedValue = ReaderCadastro["FLG_ANTECIPADO"].ToString();
            ddlTipo.SelectedValue = ReaderCadastro["FLG_TIPO_PLANO"].ToString();

            ddlPlanos.SelectedValue = ReaderCadastro["COD_ID_PLANO_BASE"].ToString();
            ddlDias.SelectedValue = ReaderCadastro["NUM_DIAS_LIQUIDACAO"].ToString();
            //txtDias.Text = ReaderCadastro["NUM_DIAS_LIQUIDACAO"].ToString();
            txtTaxaAntecipacao.Text = ReaderCadastro["NUM_TAXA_ANTECIPACAO"].ToString();
            ddlPlanosReferencia.SelectedValue = ReaderCadastro["COD_ID_PLANOS_REFERENCIA"].ToString();
        }


        if (ddlAntecipado.SelectedValue.ToString() == "S")
        {
            dvTaxas.Visible = false;
            dvParcelas.Visible = true;
        }
        else
        {
            dvTaxas.Visible = true;
            dvParcelas.Visible = false;
        }

    }

    protected void btnSalvar_Click(object sender, EventArgs e)
    {
        if (txtNome.Text.ToString().Trim() != "")
        {
            SqlConnection connInsCons = new SqlConnection(Funcoes.conexao());
            connInsCons.Open();
            SqlCommand cmdInsCons = new SqlCommand("dbo.stp_planos_ins", connInsCons);
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

            cmdInsCons.Parameters.Add("@NOM_TITULO_PLANO", SqlDbType.VarChar).Value = txtNome.Text.ToString();
            cmdInsCons.Parameters.Add("@DES_PLANO", SqlDbType.Text).Value = txtDescricao.Text.ToString();

            cmdInsCons.Parameters.Add("@FLG_MODELO_PLANO", SqlDbType.Char).Value = ddlModelo.SelectedValue.ToString();
            cmdInsCons.Parameters.Add("@FLG_TIPO_PLANO", SqlDbType.Char).Value = ddlTipo.SelectedValue.ToString();
            cmdInsCons.Parameters.Add("@FLG_VISIVEL", SqlDbType.Char).Value = ddlVisivel.SelectedValue.ToString();
            cmdInsCons.Parameters.Add("@FLG_ANTECIPADO", SqlDbType.Char).Value = ddlAntecipado.SelectedValue.ToString();
            cmdInsCons.Parameters.Add("@FLG_ATIVO", SqlDbType.Char).Value = ddlAtivo.SelectedValue.ToString();

            cmdInsCons.Parameters.Add("@COD_ID_PLANO_BASE", SqlDbType.Int).Value = Funcoes.strToInt(ddlPlanos.SelectedValue.ToString());
            cmdInsCons.Parameters.Add("@COD_ID_PLANOS_REFERENCIA", SqlDbType.Int).Value = Funcoes.strToInt(ddlPlanosReferencia.SelectedValue.ToString());
            cmdInsCons.Parameters.Add("@NUM_DIAS_LIQUIDACAO", SqlDbType.Int).Value = Funcoes.strToInt(ddlDias.SelectedValue.ToString());
            cmdInsCons.Parameters.Add("@NUM_TAXA_ANTECIPACAO", SqlDbType.Float).Value = Funcoes.strToDouble(txtTaxaAntecipacao.Text.ToString());

            cmdInsCons.ExecuteNonQuery();
            connInsCons.Close();
            connInsCons.Dispose();

            ClientScript.RegisterStartupScript(this.GetType(),
                "Alerta", "alert('Dados gravados com sucesso'); ", true);

            Response.Redirect("con_planos_adm.aspx");
        }
        else
        {
            ClientScript.RegisterStartupScript(this.GetType(),
                "Titulo", "alert('É obrigatório o preenchimento do campo Nome! Verifique e reentre'); ", true);
        }
    }
    protected void btnCancelar_Click(object sender, EventArgs e)
    {
        Response.Redirect("con_planos_adm.aspx");

    }

    protected void btnGravar_Click(object sender, EventArgs e)
    {

        foreach (RepeaterItem itemP in rptConsulta.Items)
        {
            SqlConnection connInsCons = new SqlConnection(Funcoes.conexao());
            connInsCons.Open();
            SqlCommand cmdInsCons = new SqlCommand("dbo.stp_planos_taxas_ins", connInsCons);
            cmdInsCons.CommandType = CommandType.StoredProcedure;
            cmdInsCons.Parameters.Add("@flg_operacao", SqlDbType.Char).Value = 'I';
            cmdInsCons.Parameters.Add("@COD_ID_PESSOA_LICENCIADO", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["LICENCIADO"].ToString());
            cmdInsCons.Parameters.Add("@COD_ID_PLANOS", SqlDbType.Int).Value = Funcoes.strToInt(((TextBox)itemP.FindControl("txtidplano")).Text.ToString());
            cmdInsCons.Parameters.Add("@COD_ID_BANDEIRAS", SqlDbType.Int).Value = Funcoes.strToInt(((TextBox)itemP.FindControl("txtidbandeira")).Text.ToString());
            cmdInsCons.Parameters.Add("@COD_ID_PLANOS_TABELA", SqlDbType.Int).Value = Funcoes.strToInt(((TextBox)itemP.FindControl("txtidtabela")).Text.ToString());
            cmdInsCons.Parameters.Add("@NUM_TAXA", SqlDbType.Float).Value = Funcoes.strToDouble(((TextBox)itemP.FindControl("txtTaxa")).Text.ToString());
            cmdInsCons.Parameters.Add("@NUM_MARKUP", SqlDbType.Float).Value = Funcoes.strToDouble(((TextBox)itemP.FindControl("txtMarkup")).Text.ToString());
            cmdInsCons.Parameters.Add("@FLG_PRESENCIAL_ONLINE", SqlDbType.Char).Value = "P";
            cmdInsCons.ExecuteNonQuery();
            connInsCons.Close();
            connInsCons.Dispose();
        }

        ClientScript.RegisterStartupScript(this.GetType(),
            "Alerta", "alert('Dados gravados com sucesso'); ", true);
        ConsultaGeral();

    }
    protected void btnGravarParcelas_Click(object sender, EventArgs e)
    {
        foreach (RepeaterItem itemP in rptConsultaParcelas.Items)
        {
            SqlConnection connInsCons = new SqlConnection(Funcoes.conexao());
            connInsCons.Open();
            SqlCommand cmdInsCons = new SqlCommand("dbo.stp_planos_parcelas_ins", connInsCons);
            cmdInsCons.CommandType = CommandType.StoredProcedure;
            cmdInsCons.Parameters.Add("@flg_operacao", SqlDbType.Char).Value = 'I';
            cmdInsCons.Parameters.Add("@COD_ID_PESSOA_LICENCIADO", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["LICENCIADO"].ToString());
            cmdInsCons.Parameters.Add("@COD_ID_PLANOS", SqlDbType.Int).Value = Funcoes.strToInt(lblID.Text.ToString());
            cmdInsCons.Parameters.Add("@COD_ID_BANDEIRA", SqlDbType.Int).Value = Funcoes.strToInt(((TextBox)itemP.FindControl("txtidbandeira")).Text.ToString());
            cmdInsCons.Parameters.Add("@NUM_VALOR_DEBITO", SqlDbType.Float).Value = (Funcoes.IsDoubleRealNumber(((TextBox)itemP.FindControl("txtDebito")).Text.ToString())) ? Funcoes.strToDouble(((TextBox)itemP.FindControl("txtDebito")).Text.ToString()) : 0;
            cmdInsCons.Parameters.Add("@NUM_VALOR_CREDITO_VISTA", SqlDbType.Float).Value = (Funcoes.IsDoubleRealNumber(((TextBox)itemP.FindControl("txtcredito")).Text.ToString())) ? Funcoes.strToDouble(((TextBox)itemP.FindControl("txtcredito")).Text.ToString()) : 0;
            cmdInsCons.Parameters.Add("@NUM_VALOR_CREDITO_2X", SqlDbType.Float).Value = (Funcoes.IsDoubleRealNumber(((TextBox)itemP.FindControl("txtcredito2x")).Text.ToString())) ? Funcoes.strToDouble(((TextBox)itemP.FindControl("txtcredito2x")).Text.ToString()) : 0;
            cmdInsCons.Parameters.Add("@NUM_VALOR_CREDITO_3X", SqlDbType.Float).Value = (Funcoes.IsDoubleRealNumber(((TextBox)itemP.FindControl("txtcredito3x")).Text.ToString())) ? Funcoes.strToDouble(((TextBox)itemP.FindControl("txtcredito3x")).Text.ToString()) : 0;
            cmdInsCons.Parameters.Add("@NUM_VALOR_CREDITO_4X", SqlDbType.Float).Value = (Funcoes.IsDoubleRealNumber(((TextBox)itemP.FindControl("txtcredito4x")).Text.ToString())) ? Funcoes.strToDouble(((TextBox)itemP.FindControl("txtcredito4x")).Text.ToString()) : 0;
            cmdInsCons.Parameters.Add("@NUM_VALOR_CREDITO_5X", SqlDbType.Float).Value = (Funcoes.IsDoubleRealNumber(((TextBox)itemP.FindControl("txtcredito5x")).Text.ToString())) ? Funcoes.strToDouble(((TextBox)itemP.FindControl("txtcredito5x")).Text.ToString()) : 0;
            cmdInsCons.Parameters.Add("@NUM_VALOR_CREDITO_6X", SqlDbType.Float).Value = (Funcoes.IsDoubleRealNumber(((TextBox)itemP.FindControl("txtcredito6x")).Text.ToString())) ? Funcoes.strToDouble(((TextBox)itemP.FindControl("txtcredito6x")).Text.ToString()) : 0;
            cmdInsCons.Parameters.Add("@NUM_VALOR_CREDITO_7X", SqlDbType.Float).Value = (Funcoes.IsDoubleRealNumber(((TextBox)itemP.FindControl("txtcredito7x")).Text.ToString())) ? Funcoes.strToDouble(((TextBox)itemP.FindControl("txtcredito7x")).Text.ToString()) : 0;
            cmdInsCons.Parameters.Add("@NUM_VALOR_CREDITO_8X", SqlDbType.Float).Value = (Funcoes.IsDoubleRealNumber(((TextBox)itemP.FindControl("txtcredito8x")).Text.ToString())) ? Funcoes.strToDouble(((TextBox)itemP.FindControl("txtcredito8x")).Text.ToString()) : 0;
            cmdInsCons.Parameters.Add("@NUM_VALOR_CREDITO_9X", SqlDbType.Float).Value = (Funcoes.IsDoubleRealNumber(((TextBox)itemP.FindControl("txtcredito9x")).Text.ToString())) ? Funcoes.strToDouble(((TextBox)itemP.FindControl("txtcredito9x")).Text.ToString()) : 0;
            cmdInsCons.Parameters.Add("@NUM_VALOR_CREDITO_10X", SqlDbType.Float).Value = (Funcoes.IsDoubleRealNumber(((TextBox)itemP.FindControl("txtcredito10x")).Text.ToString())) ? Funcoes.strToDouble(((TextBox)itemP.FindControl("txtcredito10x")).Text.ToString()) : 0;
            cmdInsCons.Parameters.Add("@NUM_VALOR_CREDITO_11X", SqlDbType.Float).Value = (Funcoes.IsDoubleRealNumber(((TextBox)itemP.FindControl("txtcredito11x")).Text.ToString())) ? Funcoes.strToDouble(((TextBox)itemP.FindControl("txtcredito11x")).Text.ToString()) : 0;
            cmdInsCons.Parameters.Add("@NUM_VALOR_CREDITO_12X", SqlDbType.Float).Value = (Funcoes.IsDoubleRealNumber(((TextBox)itemP.FindControl("txtcredito12x")).Text.ToString())) ? Funcoes.strToDouble(((TextBox)itemP.FindControl("txtcredito12x")).Text.ToString()) : 0;
            cmdInsCons.Parameters.Add("@NUM_VALOR_CREDITO_13X", SqlDbType.Float).Value = (Funcoes.IsDoubleRealNumber(((TextBox)itemP.FindControl("txtcredito13x")).Text.ToString())) ? Funcoes.strToDouble(((TextBox)itemP.FindControl("txtcredito13x")).Text.ToString()) : 0;
            cmdInsCons.Parameters.Add("@NUM_VALOR_CREDITO_14X", SqlDbType.Float).Value = (Funcoes.IsDoubleRealNumber(((TextBox)itemP.FindControl("txtcredito14x")).Text.ToString())) ? Funcoes.strToDouble(((TextBox)itemP.FindControl("txtcredito14x")).Text.ToString()) : 0;
            cmdInsCons.Parameters.Add("@NUM_VALOR_CREDITO_15X", SqlDbType.Float).Value = (Funcoes.IsDoubleRealNumber(((TextBox)itemP.FindControl("txtcredito15x")).Text.ToString())) ? Funcoes.strToDouble(((TextBox)itemP.FindControl("txtcredito15x")).Text.ToString()) : 0;
            cmdInsCons.Parameters.Add("@NUM_VALOR_CREDITO_16X", SqlDbType.Float).Value = (Funcoes.IsDoubleRealNumber(((TextBox)itemP.FindControl("txtcredito16x")).Text.ToString())) ? Funcoes.strToDouble(((TextBox)itemP.FindControl("txtcredito16x")).Text.ToString()) : 0;
            cmdInsCons.Parameters.Add("@NUM_VALOR_CREDITO_17X", SqlDbType.Float).Value = (Funcoes.IsDoubleRealNumber(((TextBox)itemP.FindControl("txtcredito17x")).Text.ToString())) ? Funcoes.strToDouble(((TextBox)itemP.FindControl("txtcredito17x")).Text.ToString()) : 0;
            cmdInsCons.Parameters.Add("@NUM_VALOR_CREDITO_18X", SqlDbType.Float).Value = (Funcoes.IsDoubleRealNumber(((TextBox)itemP.FindControl("txtcredito18x")).Text.ToString())) ? Funcoes.strToDouble(((TextBox)itemP.FindControl("txtcredito18x")).Text.ToString()) : 0;
            cmdInsCons.Parameters.Add("@NUM_VALOR_MARKUP_DEBITO", SqlDbType.Float).Value = (Funcoes.IsDoubleRealNumber(((TextBox)itemP.FindControl("txtmarkupdebito")).Text.ToString())) ? Funcoes.strToDouble(((TextBox)itemP.FindControl("txtmarkupdebito")).Text.ToString()) : 0;
            cmdInsCons.Parameters.Add("@NUM_VALOR_MARKUP_VISTA", SqlDbType.Float).Value = (Funcoes.IsDoubleRealNumber(((TextBox)itemP.FindControl("txtmarkupcredito")).Text.ToString())) ? Funcoes.strToDouble(((TextBox)itemP.FindControl("txtmarkupcredito")).Text.ToString()) : 0;
            cmdInsCons.Parameters.Add("@NUM_VALOR_MARKUP_2X", SqlDbType.Float).Value = (Funcoes.IsDoubleRealNumber(((TextBox)itemP.FindControl("txtmarkup2x")).Text.ToString())) ? Funcoes.strToDouble(((TextBox)itemP.FindControl("txtmarkup2x")).Text.ToString()) : 0;
            cmdInsCons.Parameters.Add("@NUM_VALOR_MARKUP_3X", SqlDbType.Float).Value = (Funcoes.IsDoubleRealNumber(((TextBox)itemP.FindControl("txtmarkup3x")).Text.ToString())) ? Funcoes.strToDouble(((TextBox)itemP.FindControl("txtmarkup3x")).Text.ToString()) : 0;
            cmdInsCons.Parameters.Add("@NUM_VALOR_MARKUP_4X", SqlDbType.Float).Value = (Funcoes.IsDoubleRealNumber(((TextBox)itemP.FindControl("txtmarkup4x")).Text.ToString())) ? Funcoes.strToDouble(((TextBox)itemP.FindControl("txtmarkup4x")).Text.ToString()) : 0;
            cmdInsCons.Parameters.Add("@NUM_VALOR_MARKUP_5X", SqlDbType.Float).Value = (Funcoes.IsDoubleRealNumber(((TextBox)itemP.FindControl("txtmarkup5x")).Text.ToString())) ? Funcoes.strToDouble(((TextBox)itemP.FindControl("txtmarkup5x")).Text.ToString()) : 0;
            cmdInsCons.Parameters.Add("@NUM_VALOR_MARKUP_6X", SqlDbType.Float).Value = (Funcoes.IsDoubleRealNumber(((TextBox)itemP.FindControl("txtmarkup6x")).Text.ToString())) ? Funcoes.strToDouble(((TextBox)itemP.FindControl("txtmarkup6x")).Text.ToString()) : 0;
            cmdInsCons.Parameters.Add("@NUM_VALOR_MARKUP_7X", SqlDbType.Float).Value = (Funcoes.IsDoubleRealNumber(((TextBox)itemP.FindControl("txtmarkup7x")).Text.ToString())) ? Funcoes.strToDouble(((TextBox)itemP.FindControl("txtmarkup7x")).Text.ToString()) : 0;
            cmdInsCons.Parameters.Add("@NUM_VALOR_MARKUP_8X", SqlDbType.Float).Value = (Funcoes.IsDoubleRealNumber(((TextBox)itemP.FindControl("txtmarkup8x")).Text.ToString())) ? Funcoes.strToDouble(((TextBox)itemP.FindControl("txtmarkup8x")).Text.ToString()) : 0;
            cmdInsCons.Parameters.Add("@NUM_VALOR_MARKUP_9X", SqlDbType.Float).Value = (Funcoes.IsDoubleRealNumber(((TextBox)itemP.FindControl("txtmarkup9x")).Text.ToString())) ? Funcoes.strToDouble(((TextBox)itemP.FindControl("txtmarkup9x")).Text.ToString()) : 0;
            cmdInsCons.Parameters.Add("@NUM_VALOR_MARKUP_10X", SqlDbType.Float).Value = (Funcoes.IsDoubleRealNumber(((TextBox)itemP.FindControl("txtmarkup10x")).Text.ToString())) ? Funcoes.strToDouble(((TextBox)itemP.FindControl("txtmarkup10x")).Text.ToString()) : 0;
            cmdInsCons.Parameters.Add("@NUM_VALOR_MARKUP_11X", SqlDbType.Float).Value = (Funcoes.IsDoubleRealNumber(((TextBox)itemP.FindControl("txtmarkup11x")).Text.ToString())) ? Funcoes.strToDouble(((TextBox)itemP.FindControl("txtmarkup11x")).Text.ToString()) : 0;
            cmdInsCons.Parameters.Add("@NUM_VALOR_MARKUP_12X", SqlDbType.Float).Value = (Funcoes.IsDoubleRealNumber(((TextBox)itemP.FindControl("txtmarkup12x")).Text.ToString())) ? Funcoes.strToDouble(((TextBox)itemP.FindControl("txtmarkup12x")).Text.ToString()) : 0;
            cmdInsCons.Parameters.Add("@NUM_VALOR_MARKUP_13X", SqlDbType.Float).Value = (Funcoes.IsDoubleRealNumber(((TextBox)itemP.FindControl("txtmarkup13x")).Text.ToString())) ? Funcoes.strToDouble(((TextBox)itemP.FindControl("txtmarkup13x")).Text.ToString()) : 0;
            cmdInsCons.Parameters.Add("@NUM_VALOR_MARKUP_14X", SqlDbType.Float).Value = (Funcoes.IsDoubleRealNumber(((TextBox)itemP.FindControl("txtmarkup14x")).Text.ToString())) ? Funcoes.strToDouble(((TextBox)itemP.FindControl("txtmarkup14x")).Text.ToString()) : 0;
            cmdInsCons.Parameters.Add("@NUM_VALOR_MARKUP_15X", SqlDbType.Float).Value = (Funcoes.IsDoubleRealNumber(((TextBox)itemP.FindControl("txtmarkup15x")).Text.ToString())) ? Funcoes.strToDouble(((TextBox)itemP.FindControl("txtmarkup15x")).Text.ToString()) : 0;
            cmdInsCons.Parameters.Add("@NUM_VALOR_MARKUP_16X", SqlDbType.Float).Value = (Funcoes.IsDoubleRealNumber(((TextBox)itemP.FindControl("txtmarkup16x")).Text.ToString())) ? Funcoes.strToDouble(((TextBox)itemP.FindControl("txtmarkup16x")).Text.ToString()) : 0;
            cmdInsCons.Parameters.Add("@NUM_VALOR_MARKUP_17X", SqlDbType.Float).Value = (Funcoes.IsDoubleRealNumber(((TextBox)itemP.FindControl("txtmarkup17x")).Text.ToString())) ? Funcoes.strToDouble(((TextBox)itemP.FindControl("txtmarkup17x")).Text.ToString()) : 0;
            cmdInsCons.Parameters.Add("@NUM_VALOR_MARKUP_18X", SqlDbType.Float).Value = (Funcoes.IsDoubleRealNumber(((TextBox)itemP.FindControl("txtmarkup18x")).Text.ToString())) ? Funcoes.strToDouble(((TextBox)itemP.FindControl("txtmarkup18x")).Text.ToString()) : 0;
            cmdInsCons.Parameters.Add("@FLG_PRESENCIAL_ONLINE", SqlDbType.Char).Value = "P";

            cmdInsCons.ExecuteNonQuery();
            connInsCons.Close();
            connInsCons.Dispose();
        }

        ClientScript.RegisterStartupScript(this.GetType(),
            "Alerta", "alert('Dados gravados com sucesso'); ", true);
        ConsultaGeral();

    }

    
    protected void btnRecalcular_Click(object sender, EventArgs e)
    {

    }
    protected void ddlModelo_SelectedIndexChanged(object sender, EventArgs e)
    {

        if (ddlModelo.SelectedValue.ToString().Trim() == "P") { tab_online.Visible = false; }
        if (ddlModelo.SelectedValue.ToString().Trim() == "Z") { tab_online.Visible = true; } 


        SqlConnection myPlanosRef = new SqlConnection(Funcoes.conexao());
        myPlanosRef.Open();
        SqlCommand cmdPlanosRef = new SqlCommand("dbo.stp_planos_referencia_ins", myPlanosRef);
        cmdPlanosRef.CommandType = CommandType.StoredProcedure;
        cmdPlanosRef.Parameters.Add("@flg_operacao", SqlDbType.VarChar).Value = "L";
        cmdPlanosRef.Parameters.Add("@COD_ID_PESSOA_LICENCIADO", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["LICENCIADO"].ToString());
        cmdPlanosRef.Parameters.Add("@FLG_MODELO_PLANO_REFERENCIA", SqlDbType.Char).Value = " ";
        cmdPlanosRef.Parameters.Add("@FLG_ATIVO", SqlDbType.Char).Value = "S";
        SqlDataAdapter drPlanosRef = new SqlDataAdapter();
        drPlanosRef.SelectCommand = cmdPlanosRef;
        DataSet dsPlanosRef = new DataSet();
        drPlanosRef.Fill(dsPlanosRef, "PLANOS_REFERENCIA");
        ddlPlanosReferencia.DataTextField = "NOM_PLANO_REFERENCIA";
        ddlPlanosReferencia.DataValueField = "COD_ID";
        ddlPlanosReferencia.DataSource = dsPlanosRef.Tables["PLANOS_REFERENCIA"].DefaultView;
        ddlPlanosReferencia.DataBind();
        ddlPlanosReferencia.Items.Insert(0, new ListItem("", "0"));

    }
    protected void ddlAntecipado_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlAntecipado.SelectedValue.ToString() == "S")
        {
            dvTaxas.Visible = false;
            dvParcelas.Visible = true;
        }
        else
        {
            dvTaxas.Visible = true;
            dvParcelas.Visible = false;
        }
    }


    protected void btnGravarOnline_Click(object sender, EventArgs e)
    {

        foreach (RepeaterItem itemP in rptConsultaOnline.Items)
        {
            SqlConnection connInsCons = new SqlConnection(Funcoes.conexao());
            connInsCons.Open();
            SqlCommand cmdInsCons = new SqlCommand("dbo.stp_planos_taxas_ins", connInsCons);
            cmdInsCons.CommandType = CommandType.StoredProcedure;
            cmdInsCons.Parameters.Add("@flg_operacao", SqlDbType.Char).Value = 'I';
            cmdInsCons.Parameters.Add("@COD_ID_PESSOA_LICENCIADO", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["LICENCIADO"].ToString());
            cmdInsCons.Parameters.Add("@COD_ID_PLANOS", SqlDbType.Int).Value = Funcoes.strToInt(((TextBox)itemP.FindControl("txtidplano")).Text.ToString());
            cmdInsCons.Parameters.Add("@COD_ID_BANDEIRAS", SqlDbType.Int).Value = Funcoes.strToInt(((TextBox)itemP.FindControl("txtidbandeira")).Text.ToString());
            cmdInsCons.Parameters.Add("@COD_ID_PLANOS_TABELA", SqlDbType.Int).Value = Funcoes.strToInt(((TextBox)itemP.FindControl("txtidtabela")).Text.ToString());
            cmdInsCons.Parameters.Add("@NUM_TAXA", SqlDbType.Float).Value = Funcoes.strToDouble(((TextBox)itemP.FindControl("txtTaxa")).Text.ToString());
            cmdInsCons.Parameters.Add("@NUM_MARKUP", SqlDbType.Float).Value = Funcoes.strToDouble(((TextBox)itemP.FindControl("txtMarkup")).Text.ToString());
            cmdInsCons.Parameters.Add("@FLG_PRESENCIAL_ONLINE", SqlDbType.Char).Value = "O";
            cmdInsCons.ExecuteNonQuery();
            connInsCons.Close();
            connInsCons.Dispose();
        }

        ClientScript.RegisterStartupScript(this.GetType(),
            "Alerta", "alert('Dados gravados com sucesso'); ", true);
        ConsultaGeralOnline();

    }
    protected void btnGravarParcelasOnline_Click(object sender, EventArgs e)
    {
        foreach (RepeaterItem itemP in rptConsultaParcelasOnline.Items)
        {
            SqlConnection connInsCons = new SqlConnection(Funcoes.conexao());
            connInsCons.Open();
            SqlCommand cmdInsCons = new SqlCommand("dbo.stp_planos_parcelas_ins", connInsCons);
            cmdInsCons.CommandType = CommandType.StoredProcedure;
            cmdInsCons.Parameters.Add("@flg_operacao", SqlDbType.Char).Value = 'I';
            cmdInsCons.Parameters.Add("@COD_ID_PESSOA_LICENCIADO", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["LICENCIADO"].ToString());
            cmdInsCons.Parameters.Add("@COD_ID_PLANOS", SqlDbType.Int).Value = Funcoes.strToInt(lblID.Text.ToString());
            cmdInsCons.Parameters.Add("@COD_ID_BANDEIRA", SqlDbType.Int).Value = Funcoes.strToInt(((TextBox)itemP.FindControl("txtidbandeira")).Text.ToString());
            cmdInsCons.Parameters.Add("@NUM_VALOR_DEBITO", SqlDbType.Float).Value = (Funcoes.IsDoubleRealNumber(((TextBox)itemP.FindControl("txtDebito")).Text.ToString())) ? Funcoes.strToDouble(((TextBox)itemP.FindControl("txtDebito")).Text.ToString()) : 0;
            cmdInsCons.Parameters.Add("@NUM_VALOR_CREDITO_VISTA", SqlDbType.Float).Value = (Funcoes.IsDoubleRealNumber(((TextBox)itemP.FindControl("txtcredito")).Text.ToString())) ? Funcoes.strToDouble(((TextBox)itemP.FindControl("txtcredito")).Text.ToString()) : 0;
            cmdInsCons.Parameters.Add("@NUM_VALOR_CREDITO_2X", SqlDbType.Float).Value = (Funcoes.IsDoubleRealNumber(((TextBox)itemP.FindControl("txtcredito2x")).Text.ToString())) ? Funcoes.strToDouble(((TextBox)itemP.FindControl("txtcredito2x")).Text.ToString()) : 0;
            cmdInsCons.Parameters.Add("@NUM_VALOR_CREDITO_3X", SqlDbType.Float).Value = (Funcoes.IsDoubleRealNumber(((TextBox)itemP.FindControl("txtcredito3x")).Text.ToString())) ? Funcoes.strToDouble(((TextBox)itemP.FindControl("txtcredito3x")).Text.ToString()) : 0;
            cmdInsCons.Parameters.Add("@NUM_VALOR_CREDITO_4X", SqlDbType.Float).Value = (Funcoes.IsDoubleRealNumber(((TextBox)itemP.FindControl("txtcredito4x")).Text.ToString())) ? Funcoes.strToDouble(((TextBox)itemP.FindControl("txtcredito4x")).Text.ToString()) : 0;
            cmdInsCons.Parameters.Add("@NUM_VALOR_CREDITO_5X", SqlDbType.Float).Value = (Funcoes.IsDoubleRealNumber(((TextBox)itemP.FindControl("txtcredito5x")).Text.ToString())) ? Funcoes.strToDouble(((TextBox)itemP.FindControl("txtcredito5x")).Text.ToString()) : 0;
            cmdInsCons.Parameters.Add("@NUM_VALOR_CREDITO_6X", SqlDbType.Float).Value = (Funcoes.IsDoubleRealNumber(((TextBox)itemP.FindControl("txtcredito6x")).Text.ToString())) ? Funcoes.strToDouble(((TextBox)itemP.FindControl("txtcredito6x")).Text.ToString()) : 0;
            cmdInsCons.Parameters.Add("@NUM_VALOR_CREDITO_7X", SqlDbType.Float).Value = (Funcoes.IsDoubleRealNumber(((TextBox)itemP.FindControl("txtcredito7x")).Text.ToString())) ? Funcoes.strToDouble(((TextBox)itemP.FindControl("txtcredito7x")).Text.ToString()) : 0;
            cmdInsCons.Parameters.Add("@NUM_VALOR_CREDITO_8X", SqlDbType.Float).Value = (Funcoes.IsDoubleRealNumber(((TextBox)itemP.FindControl("txtcredito8x")).Text.ToString())) ? Funcoes.strToDouble(((TextBox)itemP.FindControl("txtcredito8x")).Text.ToString()) : 0;
            cmdInsCons.Parameters.Add("@NUM_VALOR_CREDITO_9X", SqlDbType.Float).Value = (Funcoes.IsDoubleRealNumber(((TextBox)itemP.FindControl("txtcredito9x")).Text.ToString())) ? Funcoes.strToDouble(((TextBox)itemP.FindControl("txtcredito9x")).Text.ToString()) : 0;
            cmdInsCons.Parameters.Add("@NUM_VALOR_CREDITO_10X", SqlDbType.Float).Value = (Funcoes.IsDoubleRealNumber(((TextBox)itemP.FindControl("txtcredito10x")).Text.ToString())) ? Funcoes.strToDouble(((TextBox)itemP.FindControl("txtcredito10x")).Text.ToString()) : 0;
            cmdInsCons.Parameters.Add("@NUM_VALOR_CREDITO_11X", SqlDbType.Float).Value = (Funcoes.IsDoubleRealNumber(((TextBox)itemP.FindControl("txtcredito11x")).Text.ToString())) ? Funcoes.strToDouble(((TextBox)itemP.FindControl("txtcredito11x")).Text.ToString()) : 0;
            cmdInsCons.Parameters.Add("@NUM_VALOR_CREDITO_12X", SqlDbType.Float).Value = (Funcoes.IsDoubleRealNumber(((TextBox)itemP.FindControl("txtcredito12x")).Text.ToString())) ? Funcoes.strToDouble(((TextBox)itemP.FindControl("txtcredito12x")).Text.ToString()) : 0;
            cmdInsCons.Parameters.Add("@NUM_VALOR_CREDITO_13X", SqlDbType.Float).Value = (Funcoes.IsDoubleRealNumber(((TextBox)itemP.FindControl("txtcredito13x")).Text.ToString())) ? Funcoes.strToDouble(((TextBox)itemP.FindControl("txtcredito13x")).Text.ToString()) : 0;
            cmdInsCons.Parameters.Add("@NUM_VALOR_CREDITO_14X", SqlDbType.Float).Value = (Funcoes.IsDoubleRealNumber(((TextBox)itemP.FindControl("txtcredito14x")).Text.ToString())) ? Funcoes.strToDouble(((TextBox)itemP.FindControl("txtcredito14x")).Text.ToString()) : 0;
            cmdInsCons.Parameters.Add("@NUM_VALOR_CREDITO_15X", SqlDbType.Float).Value = (Funcoes.IsDoubleRealNumber(((TextBox)itemP.FindControl("txtcredito15x")).Text.ToString())) ? Funcoes.strToDouble(((TextBox)itemP.FindControl("txtcredito15x")).Text.ToString()) : 0;
            cmdInsCons.Parameters.Add("@NUM_VALOR_CREDITO_16X", SqlDbType.Float).Value = (Funcoes.IsDoubleRealNumber(((TextBox)itemP.FindControl("txtcredito16x")).Text.ToString())) ? Funcoes.strToDouble(((TextBox)itemP.FindControl("txtcredito16x")).Text.ToString()) : 0;
            cmdInsCons.Parameters.Add("@NUM_VALOR_CREDITO_17X", SqlDbType.Float).Value = (Funcoes.IsDoubleRealNumber(((TextBox)itemP.FindControl("txtcredito17x")).Text.ToString())) ? Funcoes.strToDouble(((TextBox)itemP.FindControl("txtcredito17x")).Text.ToString()) : 0;
            cmdInsCons.Parameters.Add("@NUM_VALOR_CREDITO_18X", SqlDbType.Float).Value = (Funcoes.IsDoubleRealNumber(((TextBox)itemP.FindControl("txtcredito18x")).Text.ToString())) ? Funcoes.strToDouble(((TextBox)itemP.FindControl("txtcredito18x")).Text.ToString()) : 0;
            cmdInsCons.Parameters.Add("@NUM_VALOR_MARKUP_DEBITO", SqlDbType.Float).Value = (Funcoes.IsDoubleRealNumber(((TextBox)itemP.FindControl("txtmarkupdebito")).Text.ToString())) ? Funcoes.strToDouble(((TextBox)itemP.FindControl("txtmarkupdebito")).Text.ToString()) : 0;
            cmdInsCons.Parameters.Add("@NUM_VALOR_MARKUP_VISTA", SqlDbType.Float).Value = (Funcoes.IsDoubleRealNumber(((TextBox)itemP.FindControl("txtmarkupcredito")).Text.ToString())) ? Funcoes.strToDouble(((TextBox)itemP.FindControl("txtmarkupcredito")).Text.ToString()) : 0;
            cmdInsCons.Parameters.Add("@NUM_VALOR_MARKUP_2X", SqlDbType.Float).Value = (Funcoes.IsDoubleRealNumber(((TextBox)itemP.FindControl("txtmarkup2x")).Text.ToString())) ? Funcoes.strToDouble(((TextBox)itemP.FindControl("txtmarkup2x")).Text.ToString()) : 0;
            cmdInsCons.Parameters.Add("@NUM_VALOR_MARKUP_3X", SqlDbType.Float).Value = (Funcoes.IsDoubleRealNumber(((TextBox)itemP.FindControl("txtmarkup3x")).Text.ToString())) ? Funcoes.strToDouble(((TextBox)itemP.FindControl("txtmarkup3x")).Text.ToString()) : 0;
            cmdInsCons.Parameters.Add("@NUM_VALOR_MARKUP_4X", SqlDbType.Float).Value = (Funcoes.IsDoubleRealNumber(((TextBox)itemP.FindControl("txtmarkup4x")).Text.ToString())) ? Funcoes.strToDouble(((TextBox)itemP.FindControl("txtmarkup4x")).Text.ToString()) : 0;
            cmdInsCons.Parameters.Add("@NUM_VALOR_MARKUP_5X", SqlDbType.Float).Value = (Funcoes.IsDoubleRealNumber(((TextBox)itemP.FindControl("txtmarkup5x")).Text.ToString())) ? Funcoes.strToDouble(((TextBox)itemP.FindControl("txtmarkup5x")).Text.ToString()) : 0;
            cmdInsCons.Parameters.Add("@NUM_VALOR_MARKUP_6X", SqlDbType.Float).Value = (Funcoes.IsDoubleRealNumber(((TextBox)itemP.FindControl("txtmarkup6x")).Text.ToString())) ? Funcoes.strToDouble(((TextBox)itemP.FindControl("txtmarkup6x")).Text.ToString()) : 0;
            cmdInsCons.Parameters.Add("@NUM_VALOR_MARKUP_7X", SqlDbType.Float).Value = (Funcoes.IsDoubleRealNumber(((TextBox)itemP.FindControl("txtmarkup7x")).Text.ToString())) ? Funcoes.strToDouble(((TextBox)itemP.FindControl("txtmarkup7x")).Text.ToString()) : 0;
            cmdInsCons.Parameters.Add("@NUM_VALOR_MARKUP_8X", SqlDbType.Float).Value = (Funcoes.IsDoubleRealNumber(((TextBox)itemP.FindControl("txtmarkup8x")).Text.ToString())) ? Funcoes.strToDouble(((TextBox)itemP.FindControl("txtmarkup8x")).Text.ToString()) : 0;
            cmdInsCons.Parameters.Add("@NUM_VALOR_MARKUP_9X", SqlDbType.Float).Value = (Funcoes.IsDoubleRealNumber(((TextBox)itemP.FindControl("txtmarkup9x")).Text.ToString())) ? Funcoes.strToDouble(((TextBox)itemP.FindControl("txtmarkup9x")).Text.ToString()) : 0;
            cmdInsCons.Parameters.Add("@NUM_VALOR_MARKUP_10X", SqlDbType.Float).Value = (Funcoes.IsDoubleRealNumber(((TextBox)itemP.FindControl("txtmarkup10x")).Text.ToString())) ? Funcoes.strToDouble(((TextBox)itemP.FindControl("txtmarkup10x")).Text.ToString()) : 0;
            cmdInsCons.Parameters.Add("@NUM_VALOR_MARKUP_11X", SqlDbType.Float).Value = (Funcoes.IsDoubleRealNumber(((TextBox)itemP.FindControl("txtmarkup11x")).Text.ToString())) ? Funcoes.strToDouble(((TextBox)itemP.FindControl("txtmarkup11x")).Text.ToString()) : 0;
            cmdInsCons.Parameters.Add("@NUM_VALOR_MARKUP_12X", SqlDbType.Float).Value = (Funcoes.IsDoubleRealNumber(((TextBox)itemP.FindControl("txtmarkup12x")).Text.ToString())) ? Funcoes.strToDouble(((TextBox)itemP.FindControl("txtmarkup12x")).Text.ToString()) : 0;
            cmdInsCons.Parameters.Add("@NUM_VALOR_MARKUP_13X", SqlDbType.Float).Value = (Funcoes.IsDoubleRealNumber(((TextBox)itemP.FindControl("txtmarkup13x")).Text.ToString())) ? Funcoes.strToDouble(((TextBox)itemP.FindControl("txtmarkup13x")).Text.ToString()) : 0;
            cmdInsCons.Parameters.Add("@NUM_VALOR_MARKUP_14X", SqlDbType.Float).Value = (Funcoes.IsDoubleRealNumber(((TextBox)itemP.FindControl("txtmarkup14x")).Text.ToString())) ? Funcoes.strToDouble(((TextBox)itemP.FindControl("txtmarkup14x")).Text.ToString()) : 0;
            cmdInsCons.Parameters.Add("@NUM_VALOR_MARKUP_15X", SqlDbType.Float).Value = (Funcoes.IsDoubleRealNumber(((TextBox)itemP.FindControl("txtmarkup15x")).Text.ToString())) ? Funcoes.strToDouble(((TextBox)itemP.FindControl("txtmarkup15x")).Text.ToString()) : 0;
            cmdInsCons.Parameters.Add("@NUM_VALOR_MARKUP_16X", SqlDbType.Float).Value = (Funcoes.IsDoubleRealNumber(((TextBox)itemP.FindControl("txtmarkup16x")).Text.ToString())) ? Funcoes.strToDouble(((TextBox)itemP.FindControl("txtmarkup16x")).Text.ToString()) : 0;
            cmdInsCons.Parameters.Add("@NUM_VALOR_MARKUP_17X", SqlDbType.Float).Value = (Funcoes.IsDoubleRealNumber(((TextBox)itemP.FindControl("txtmarkup17x")).Text.ToString())) ? Funcoes.strToDouble(((TextBox)itemP.FindControl("txtmarkup17x")).Text.ToString()) : 0;
            cmdInsCons.Parameters.Add("@NUM_VALOR_MARKUP_18X", SqlDbType.Float).Value = (Funcoes.IsDoubleRealNumber(((TextBox)itemP.FindControl("txtmarkup18x")).Text.ToString())) ? Funcoes.strToDouble(((TextBox)itemP.FindControl("txtmarkup18x")).Text.ToString()) : 0;
            cmdInsCons.Parameters.Add("@FLG_PRESENCIAL_ONLINE", SqlDbType.Char).Value = "O";

            cmdInsCons.ExecuteNonQuery();
            connInsCons.Close();
            connInsCons.Dispose();
        }

        ClientScript.RegisterStartupScript(this.GetType(),
            "Alerta", "alert('Dados gravados com sucesso'); ", true);
        ConsultaGeralOnline();

    }

}