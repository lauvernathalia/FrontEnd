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


public partial class cad_planos : System.Web.UI.Page
{
    public string sid_id { get; set; }



    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            sid_id = Funcoes.Decrypt(Request["id"].ToString());
        }
        catch
        {
            sid_id = "0";
        }


        // Verifica se o usuário esta autenticado

        // Verifica se o usuário esta autenticado
        if (HttpContext.Current.Session.Count <= 0)
        {
            Response.Redirect("login.aspx");
            FormsAuthentication.SignOut();
        }

        if (!IsPostBack)
        {
            CarregaAdquirentes();

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
            SqlCommand cmdPlanosRef = new SqlCommand("dbo.stp_pessoas_fj_planos_referencia_ins", myPlanosRef);
            cmdPlanosRef.CommandType = CommandType.StoredProcedure;
            cmdPlanosRef.Parameters.Add("@flg_operacao", SqlDbType.VarChar).Value = "X";
            cmdPlanosRef.Parameters.Add("@COD_ID_PESSOA_LICENCIADO", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["LICENCIADO"].ToString());

            if (HttpContext.Current.Session["TIPO"].ToString() != "A")
            {
                cmdPlanosRef.Parameters.Add("@COD_ID_PESSOAS_FJ", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["PESSOA"].ToString());
            }

            cmdPlanosRef.Parameters.Add("@FLG_MODELO_PLANO_REFERENCIA", SqlDbType.Char).Value = ddlAdquirentes.SelectedValue.ToString();
            cmdPlanosRef.Parameters.Add("@FLG_ACESSO", SqlDbType.Char).Value = "S";
            SqlDataAdapter drPlanosRef = new SqlDataAdapter();
            drPlanosRef.SelectCommand = cmdPlanosRef;
            DataSet dsPlanosRef = new DataSet();
            drPlanosRef.Fill(dsPlanosRef, "PESSOAS_FJ_PLANOS_REFERENCIA");
            ddlPlanosReferencia.DataTextField = "NOM_PLANO_REFERENCIA";
            ddlPlanosReferencia.DataValueField = "COD_ID_PLANOS_REFERENCIA";
            ddlPlanosReferencia.DataSource = dsPlanosRef.Tables["PESSOAS_FJ_PLANOS_REFERENCIA"].DefaultView;
            ddlPlanosReferencia.DataBind();
            ddlPlanosReferencia.Items.Insert(0, new ListItem("", "0"));

            if (Funcoes.strToInt(sid_id) != 0)
            {
                btnImportarTaxas.Visible = true;
                ConsultaFicha();

                dvTaxas.Visible = true;
                dvParcelas.Visible = true;
                dvTaxasOnline.Visible = true;
                dvParcelasOnline.Visible = true;


                ConsultaGeral();
                if (ddlAdquirentes.SelectedValue.ToString().Trim() == "Z") { ConsultaGeralOnline(); }

                //if (ddlAntecipado.SelectedValue.ToString() == "S")
                //{
                    dvTaxas.Visible = false;
                    dvParcelas.Visible = true;
                    dvTaxasOnline.Visible = false;
                    dvParcelasOnline.Visible = true;
                //}
                //else
                //{
                //    dvTaxas.Visible = true;
                //    dvParcelas.Visible = false;
                //    dvTaxasOnline.Visible = true;
                //    dvParcelasOnline.Visible = false;
                //}

            }

            if (ddlAdquirentes.SelectedValue.ToString().Trim() == "A") { tab_online.Visible = false; }
            if (ddlAdquirentes.SelectedValue.ToString().Trim() == "P") { tab_online.Visible = false; }
            if (ddlAdquirentes.SelectedValue.ToString().Trim() == "Z") { tab_online.Visible = true; }

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


    private void ConsultaGeral()
    {
        /*
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

        if (ddlAdquirentes.SelectedValue.ToString() == "P")
        {
            foreach (RepeaterItem itemPr in rptConsulta.Items)
            {
                ((TextBox)itemPr.FindControl("txtTaxa")).Enabled = true;
            }
        }
        */
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

        if (ddlAdquirentes.SelectedValue.ToString() == "P")
        {
            foreach (RepeaterItem itemP in rptConsultaParcelas.Items)
            {
                // Libera campo de taxas

                ((TextBox)itemP.FindControl("txtdebito")).Enabled = true;
                ((TextBox)itemP.FindControl("txtcredito")).Enabled = true;
                ((TextBox)itemP.FindControl("txtcredito2x")).Enabled = true;
                ((TextBox)itemP.FindControl("txtcredito3x")).Enabled = true;
                ((TextBox)itemP.FindControl("txtcredito4x")).Enabled = true;
                ((TextBox)itemP.FindControl("txtcredito5x")).Enabled = true;
                ((TextBox)itemP.FindControl("txtcredito6x")).Enabled = true;
                ((TextBox)itemP.FindControl("txtcredito7x")).Enabled = true;
                ((TextBox)itemP.FindControl("txtcredito8x")).Enabled = true;
                ((TextBox)itemP.FindControl("txtcredito9x")).Enabled = true;
                ((TextBox)itemP.FindControl("txtcredito10x")).Enabled = true;
                ((TextBox)itemP.FindControl("txtcredito11x")).Enabled = true;
                ((TextBox)itemP.FindControl("txtcredito12x")).Enabled = true;
                ((TextBox)itemP.FindControl("txtcredito13x")).Enabled = true;
                ((TextBox)itemP.FindControl("txtcredito14x")).Enabled = true;
                ((TextBox)itemP.FindControl("txtcredito15x")).Enabled = true;
                ((TextBox)itemP.FindControl("txtcredito16x")).Enabled = true;
                ((TextBox)itemP.FindControl("txtcredito17x")).Enabled = true;
                ((TextBox)itemP.FindControl("txtcredito18x")).Enabled = true;

                ((TextBox)itemP.FindControl("txtcredito19x")).Enabled = true;
                ((TextBox)itemP.FindControl("txtcredito20x")).Enabled = true;
                ((TextBox)itemP.FindControl("txtcredito21x")).Enabled = true;


                // Libera campo valor fixo

                ((TextBox)itemP.FindControl("txtdebitofixo")).Enabled = true;
                ((TextBox)itemP.FindControl("txtcreditofixo")).Enabled = true;
                ((TextBox)itemP.FindControl("txtcredito2xfixo")).Enabled = true;
                ((TextBox)itemP.FindControl("txtcredito3xfixo")).Enabled = true;
                ((TextBox)itemP.FindControl("txtcredito4xfixo")).Enabled = true;
                ((TextBox)itemP.FindControl("txtcredito5xfixo")).Enabled = true;
                ((TextBox)itemP.FindControl("txtcredito6xfixo")).Enabled = true;
                ((TextBox)itemP.FindControl("txtcredito7xfixo")).Enabled = true;
                ((TextBox)itemP.FindControl("txtcredito8xfixo")).Enabled = true;
                ((TextBox)itemP.FindControl("txtcredito9xfixo")).Enabled = true;
                ((TextBox)itemP.FindControl("txtcredito10xfixo")).Enabled = true;
                ((TextBox)itemP.FindControl("txtcredito11xfixo")).Enabled = true;
                ((TextBox)itemP.FindControl("txtcredito12xfixo")).Enabled = true;
                ((TextBox)itemP.FindControl("txtcredito13xfixo")).Enabled = true;
                ((TextBox)itemP.FindControl("txtcredito14xfixo")).Enabled = true;
                ((TextBox)itemP.FindControl("txtcredito15xfixo")).Enabled = true;
                ((TextBox)itemP.FindControl("txtcredito16xfixo")).Enabled = true;
                ((TextBox)itemP.FindControl("txtcredito17xfixo")).Enabled = true;
                ((TextBox)itemP.FindControl("txtcredito18xfixo")).Enabled = true;

                ((TextBox)itemP.FindControl("txtcredito19xfixo")).Enabled = true;
                ((TextBox)itemP.FindControl("txtcredito20xfixo")).Enabled = true;
                ((TextBox)itemP.FindControl("txtcredito21xfixo")).Enabled = true;

            }
        }

    }

    private void ConsultaGeralOnline()
    {
        /*
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

        if (ddlAdquirentes.SelectedValue.ToString() == "P")
        {
            foreach (RepeaterItem itemPr in rptConsultaOnline.Items)
            {
                ((TextBox)itemPr.FindControl("txtTaxa")).Enabled = true;
            }
        }
        */

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

        if ((ddlAdquirentes.SelectedValue.ToString() == "P") || (ddlAdquirentes.SelectedValue.ToString() == "Z"))
        {
            foreach (RepeaterItem itemP in rptConsultaParcelasOnline.Items)
            {
                ((TextBox)itemP.FindControl("txtDebito")).Enabled = true;
                ((TextBox)itemP.FindControl("txtcredito")).Enabled = true;
                ((TextBox)itemP.FindControl("txtcredito2x")).Enabled = true;
                ((TextBox)itemP.FindControl("txtcredito3x")).Enabled = true;
                ((TextBox)itemP.FindControl("txtcredito4x")).Enabled = true;
                ((TextBox)itemP.FindControl("txtcredito5x")).Enabled = true;
                ((TextBox)itemP.FindControl("txtcredito6x")).Enabled = true;
                ((TextBox)itemP.FindControl("txtcredito7x")).Enabled = true;
                ((TextBox)itemP.FindControl("txtcredito8x")).Enabled = true;
                ((TextBox)itemP.FindControl("txtcredito9x")).Enabled = true;
                ((TextBox)itemP.FindControl("txtcredito10x")).Enabled = true;
                ((TextBox)itemP.FindControl("txtcredito11x")).Enabled = true;
                ((TextBox)itemP.FindControl("txtcredito12x")).Enabled = true;
                ((TextBox)itemP.FindControl("txtcredito13x")).Enabled = true;
                ((TextBox)itemP.FindControl("txtcredito14x")).Enabled = true;
                ((TextBox)itemP.FindControl("txtcredito15x")).Enabled = true;
                ((TextBox)itemP.FindControl("txtcredito16x")).Enabled = true;
                ((TextBox)itemP.FindControl("txtcredito17x")).Enabled = true;
                ((TextBox)itemP.FindControl("txtcredito18x")).Enabled = true;

                ((TextBox)itemP.FindControl("txtcredito19x")).Enabled = true;
                ((TextBox)itemP.FindControl("txtcredito20x")).Enabled = true;
                ((TextBox)itemP.FindControl("txtcredito21x")).Enabled = true;


                // Libera campo valor fixo

                ((TextBox)itemP.FindControl("txtdebitofixo")).Enabled = true;
                ((TextBox)itemP.FindControl("txtcreditofixo")).Enabled = true;
                ((TextBox)itemP.FindControl("txtcredito2xfixo")).Enabled = true;
                ((TextBox)itemP.FindControl("txtcredito3xfixo")).Enabled = true;
                ((TextBox)itemP.FindControl("txtcredito4xfixo")).Enabled = true;
                ((TextBox)itemP.FindControl("txtcredito5xfixo")).Enabled = true;
                ((TextBox)itemP.FindControl("txtcredito6xfixo")).Enabled = true;
                ((TextBox)itemP.FindControl("txtcredito7xfixo")).Enabled = true;
                ((TextBox)itemP.FindControl("txtcredito8xfixo")).Enabled = true;
                ((TextBox)itemP.FindControl("txtcredito9xfixo")).Enabled = true;
                ((TextBox)itemP.FindControl("txtcredito10xfixo")).Enabled = true;
                ((TextBox)itemP.FindControl("txtcredito11xfixo")).Enabled = true;
                ((TextBox)itemP.FindControl("txtcredito12xfixo")).Enabled = true;
                ((TextBox)itemP.FindControl("txtcredito13xfixo")).Enabled = true;
                ((TextBox)itemP.FindControl("txtcredito14xfixo")).Enabled = true;
                ((TextBox)itemP.FindControl("txtcredito15xfixo")).Enabled = true;
                ((TextBox)itemP.FindControl("txtcredito16xfixo")).Enabled = true;
                ((TextBox)itemP.FindControl("txtcredito17xfixo")).Enabled = true;
                ((TextBox)itemP.FindControl("txtcredito18xfixo")).Enabled = true;

                ((TextBox)itemP.FindControl("txtcredito19xfixo")).Enabled = true;
                ((TextBox)itemP.FindControl("txtcredito20xfixo")).Enabled = true;
                ((TextBox)itemP.FindControl("txtcredito21xfixo")).Enabled = true;


            }

            foreach (RepeaterItem itemP in rptConsultaParcelas.Items)
            {
                ((TextBox)itemP.FindControl("txtDebito")).Enabled = true;
                ((TextBox)itemP.FindControl("txtcredito")).Enabled = true;
                ((TextBox)itemP.FindControl("txtcredito2x")).Enabled = true;
                ((TextBox)itemP.FindControl("txtcredito3x")).Enabled = true;
                ((TextBox)itemP.FindControl("txtcredito4x")).Enabled = true;
                ((TextBox)itemP.FindControl("txtcredito5x")).Enabled = true;
                ((TextBox)itemP.FindControl("txtcredito6x")).Enabled = true;
                ((TextBox)itemP.FindControl("txtcredito7x")).Enabled = true;
                ((TextBox)itemP.FindControl("txtcredito8x")).Enabled = true;
                ((TextBox)itemP.FindControl("txtcredito9x")).Enabled = true;
                ((TextBox)itemP.FindControl("txtcredito10x")).Enabled = true;
                ((TextBox)itemP.FindControl("txtcredito11x")).Enabled = true;
                ((TextBox)itemP.FindControl("txtcredito12x")).Enabled = true;
                ((TextBox)itemP.FindControl("txtcredito13x")).Enabled = true;
                ((TextBox)itemP.FindControl("txtcredito14x")).Enabled = true;
                ((TextBox)itemP.FindControl("txtcredito15x")).Enabled = true;
                ((TextBox)itemP.FindControl("txtcredito16x")).Enabled = true;
                ((TextBox)itemP.FindControl("txtcredito17x")).Enabled = true;
                ((TextBox)itemP.FindControl("txtcredito18x")).Enabled = true;

                ((TextBox)itemP.FindControl("txtcredito19x")).Enabled = true;
                ((TextBox)itemP.FindControl("txtcredito20x")).Enabled = true;
                ((TextBox)itemP.FindControl("txtcredito21x")).Enabled = true;


                // Libera campo valor fixo

                ((TextBox)itemP.FindControl("txtdebitofixo")).Enabled = true;
                ((TextBox)itemP.FindControl("txtcreditofixo")).Enabled = true;
                ((TextBox)itemP.FindControl("txtcredito2xfixo")).Enabled = true;
                ((TextBox)itemP.FindControl("txtcredito3xfixo")).Enabled = true;
                ((TextBox)itemP.FindControl("txtcredito4xfixo")).Enabled = true;
                ((TextBox)itemP.FindControl("txtcredito5xfixo")).Enabled = true;
                ((TextBox)itemP.FindControl("txtcredito6xfixo")).Enabled = true;
                ((TextBox)itemP.FindControl("txtcredito7xfixo")).Enabled = true;
                ((TextBox)itemP.FindControl("txtcredito8xfixo")).Enabled = true;
                ((TextBox)itemP.FindControl("txtcredito9xfixo")).Enabled = true;
                ((TextBox)itemP.FindControl("txtcredito10xfixo")).Enabled = true;
                ((TextBox)itemP.FindControl("txtcredito11xfixo")).Enabled = true;
                ((TextBox)itemP.FindControl("txtcredito12xfixo")).Enabled = true;
                ((TextBox)itemP.FindControl("txtcredito13xfixo")).Enabled = true;
                ((TextBox)itemP.FindControl("txtcredito14xfixo")).Enabled = true;
                ((TextBox)itemP.FindControl("txtcredito15xfixo")).Enabled = true;
                ((TextBox)itemP.FindControl("txtcredito16xfixo")).Enabled = true;
                ((TextBox)itemP.FindControl("txtcredito17xfixo")).Enabled = true;
                ((TextBox)itemP.FindControl("txtcredito18xfixo")).Enabled = true;

                ((TextBox)itemP.FindControl("txtcredito19xfixo")).Enabled = true;
                ((TextBox)itemP.FindControl("txtcredito20xfixo")).Enabled = true;
                ((TextBox)itemP.FindControl("txtcredito21xfixo")).Enabled = true;


            }
        }

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
            ddlAdquirentes.SelectedValue = ReaderCadastro["FLG_MODELO_PLANO"].ToString();
            ddlAdquirentes_SelectedIndexChanged(null, null);

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
            if (Funcoes.strToInt(lblID.Text.ToString()) != 0)
            {
                cmdInsCons.Parameters.Add("@flg_operacao", SqlDbType.Char).Value = 'A';
                cmdInsCons.Parameters.Add("@COD_ID", SqlDbType.Int).Value = Funcoes.strToInt(lblID.Text.ToString());
            }
            else
            {
                cmdInsCons.Parameters.Add("@flg_operacao", SqlDbType.Char).Value = 'I';
            }
            cmdInsCons.Parameters.Add("@COD_ID_PESSOA_LICENCIADO", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["LICENCIADO"].ToString());

            if (HttpContext.Current.Session["TIPO"].ToString() != "A")
            {
                cmdInsCons.Parameters.Add("@COD_ID_PESSOAS_FJ", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["PESSOA"].ToString());
            }

            cmdInsCons.Parameters.Add("@NOM_TITULO_PLANO", SqlDbType.VarChar).Value = txtNome.Text.ToString();
            cmdInsCons.Parameters.Add("@DES_PLANO", SqlDbType.Text).Value = txtDescricao.Text.ToString();

            cmdInsCons.Parameters.Add("@FLG_MODELO_PLANO", SqlDbType.Char).Value = ddlAdquirentes.SelectedValue.ToString();
            cmdInsCons.Parameters.Add("@FLG_TIPO_PLANO", SqlDbType.Char).Value = ddlTipo.SelectedValue.ToString();
            cmdInsCons.Parameters.Add("@FLG_VISIVEL", SqlDbType.Char).Value = ddlVisivel.SelectedValue.ToString();
            cmdInsCons.Parameters.Add("@FLG_ANTECIPADO", SqlDbType.Char).Value = ddlAntecipado.SelectedValue.ToString();
            cmdInsCons.Parameters.Add("@FLG_ATIVO", SqlDbType.Char).Value = ddlAtivo.SelectedValue.ToString();

            cmdInsCons.Parameters.Add("@COD_ID_PLANO_BASE", SqlDbType.Int).Value = Funcoes.strToInt(ddlPlanos.SelectedValue.ToString());
            cmdInsCons.Parameters.Add("@COD_ID_PLANOS_REFERENCIA", SqlDbType.Int).Value = Funcoes.strToInt(ddlPlanosReferencia.SelectedValue.ToString());
            cmdInsCons.Parameters.Add("@NUM_DIAS_LIQUIDACAO", SqlDbType.Int).Value = Funcoes.strToInt(ddlDias.SelectedValue.ToString());
            cmdInsCons.Parameters.Add("@NUM_TAXA_ANTECIPACAO", SqlDbType.Float).Value = Funcoes.strToDouble(txtTaxaAntecipacao.Text.ToString());

            if (Funcoes.strToInt(lblID.Text.ToString()) != 0)
            {
                cmdInsCons.ExecuteNonQuery();
            }
            else
            {
                lblID.Text = cmdInsCons.ExecuteScalar().ToString();
                sid_id = lblID.Text.ToString();
            }
            connInsCons.Close();
            connInsCons.Dispose();

            btnImportarTaxas.Visible = true;

            dvTaxas.Visible = true;
            dvParcelas.Visible = true;
            dvTaxasOnline.Visible = true;
            dvParcelasOnline.Visible = true;


            ConsultaGeral();
            if (ddlAdquirentes.SelectedValue.ToString().Trim() == "Z") { ConsultaGeralOnline(); }

            //if (ddlAntecipado.SelectedValue.ToString() == "S")
            //{
                dvTaxas.Visible = false;
                dvParcelas.Visible = true;
                dvTaxasOnline.Visible = false;
                dvParcelasOnline.Visible = true;
            //}
            //else
            //{
            //    dvTaxas.Visible = true;
            //    dvParcelas.Visible = false;
            //    dvTaxasOnline.Visible = true;
            //    dvParcelasOnline.Visible = false;
            //}

            //ImportarTaxas();

            ClientScript.RegisterStartupScript(this.GetType(),
                "Alerta", "alert('Dados salvos com sucesso'); ", true);
        }
        else
        {
            ClientScript.RegisterStartupScript(this.GetType(),
                "Titulo", "alert('É obrigatório o preenchimento do campo Nome! Verifique e reentre'); ", true);
        }
    }
    protected void btnCancelar_Click(object sender, EventArgs e)
    {
        ClientScript.RegisterStartupScript(this.GetType(),
     "Titulo", "opener.PostBackOnMainPage(); window.close(); ", true);
    }

    protected void btnGravar_Click(object sender, EventArgs e)
    {
        //Gravar();
        //GravarOnline();
        GravarParcelas();
        GravarParcelasOnline();

        ClientScript.RegisterStartupScript(this.GetType(),
    "Alerta", "alert('Dados gravados com sucesso'); ", true);

        ConsultaGeral();
        ConsultaGeralOnline();



    }
    protected void btnGravarParcelas_Click(object sender, EventArgs e)
    {

        // Gravar();
        //GravarOnline();
        GravarParcelas();
        GravarParcelasOnline();

        ClientScript.RegisterStartupScript(this.GetType(),
    "Alerta", "alert('Dados gravados com sucesso'); ", true);

        ConsultaGeral();
        ConsultaGeralOnline();

    }


    protected void btnRecalcular_Click(object sender, EventArgs e)
    {

    }
    protected void ddlAdquirentes_SelectedIndexChanged(object sender, EventArgs e)
    {

        if (ddlAdquirentes.SelectedValue.ToString().Trim() == "A") { tab_online.Visible = false; dvImportarExcel.Visible = false; }
        if (ddlAdquirentes.SelectedValue.ToString().Trim() == "P") { tab_online.Visible = false; dvImportarExcel.Visible = true; }
        if (ddlAdquirentes.SelectedValue.ToString().Trim() == "C") { tab_online.Visible = false; dvImportarExcel.Visible = false; }
        if (ddlAdquirentes.SelectedValue.ToString().Trim() == "Z") { tab_online.Visible = true;  dvImportarExcel.Visible = false; }

        SqlConnection myPlanosRef = new SqlConnection(Funcoes.conexao());
        myPlanosRef.Open();
        SqlCommand cmdPlanosRef = new SqlCommand("dbo.stp_pessoas_fj_planos_referencia_ins", myPlanosRef);
        cmdPlanosRef.CommandType = CommandType.StoredProcedure;
        cmdPlanosRef.Parameters.Add("@flg_operacao", SqlDbType.VarChar).Value = "X";
        cmdPlanosRef.Parameters.Add("@COD_ID_PESSOA_LICENCIADO", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["LICENCIADO"].ToString());

        if (HttpContext.Current.Session["TIPO"].ToString() != "A")
        {
            cmdPlanosRef.Parameters.Add("@COD_ID_PESSOAS_FJ", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["PESSOA"].ToString());
        }

        cmdPlanosRef.Parameters.Add("@FLG_MODELO_PLANO_REFERENCIA", SqlDbType.Char).Value = ddlAdquirentes.SelectedValue.ToString();
        cmdPlanosRef.Parameters.Add("@FLG_ACESSO", SqlDbType.Char).Value = "S";
        SqlDataAdapter drPlanosRef = new SqlDataAdapter();
        drPlanosRef.SelectCommand = cmdPlanosRef;
        DataSet dsPlanosRef = new DataSet();
        drPlanosRef.Fill(dsPlanosRef, "PESSOAS_FJ_PLANOS_REFERENCIA");
        ddlPlanosReferencia.DataTextField = "NOM_PLANO_REFERENCIA";
        ddlPlanosReferencia.DataValueField = "COD_ID_PLANOS_REFERENCIA";
        ddlPlanosReferencia.DataSource = dsPlanosRef.Tables["PESSOAS_FJ_PLANOS_REFERENCIA"].DefaultView;
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

        //Gravar();
        //GravarOnline();
        GravarParcelas();
        GravarParcelasOnline();

        ClientScript.RegisterStartupScript(this.GetType(),
    "Alerta", "alert('Dados gravados com sucesso'); ", true);

        ConsultaGeral();
        ConsultaGeralOnline();





    }
    protected void btnGravarParcelasOnline_Click(object sender, EventArgs e)
    {
        //Gravar();
        //GravarOnline();
        GravarParcelas();
        GravarParcelasOnline();

        ClientScript.RegisterStartupScript(this.GetType(),
    "Alerta", "alert('Dados gravados com sucesso'); ", true);


        ConsultaGeral();
        ConsultaGeralOnline();


    }

    private void GravarParcelas()
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

            cmdInsCons.Parameters.Add("@NUM_VALOR_CREDITO_19X", SqlDbType.Float).Value = (Funcoes.IsDoubleRealNumber(((TextBox)itemP.FindControl("txtcredito19x")).Text.ToString())) ? Funcoes.strToDouble(((TextBox)itemP.FindControl("txtcredito19x")).Text.ToString()) : 0;
            cmdInsCons.Parameters.Add("@NUM_VALOR_CREDITO_20X", SqlDbType.Float).Value = (Funcoes.IsDoubleRealNumber(((TextBox)itemP.FindControl("txtcredito20x")).Text.ToString())) ? Funcoes.strToDouble(((TextBox)itemP.FindControl("txtcredito20x")).Text.ToString()) : 0;
            cmdInsCons.Parameters.Add("@NUM_VALOR_CREDITO_21X", SqlDbType.Float).Value = (Funcoes.IsDoubleRealNumber(((TextBox)itemP.FindControl("txtcredito21x")).Text.ToString())) ? Funcoes.strToDouble(((TextBox)itemP.FindControl("txtcredito21x")).Text.ToString()) : 0;

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

            cmdInsCons.Parameters.Add("@NUM_VALOR_MARKUP_19X", SqlDbType.Float).Value = (Funcoes.IsDoubleRealNumber(((TextBox)itemP.FindControl("txtmarkup19x")).Text.ToString())) ? Funcoes.strToDouble(((TextBox)itemP.FindControl("txtmarkup19x")).Text.ToString()) : 0;
            cmdInsCons.Parameters.Add("@NUM_VALOR_MARKUP_20X", SqlDbType.Float).Value = (Funcoes.IsDoubleRealNumber(((TextBox)itemP.FindControl("txtmarkup20x")).Text.ToString())) ? Funcoes.strToDouble(((TextBox)itemP.FindControl("txtmarkup20x")).Text.ToString()) : 0;
            cmdInsCons.Parameters.Add("@NUM_VALOR_MARKUP_21X", SqlDbType.Float).Value = (Funcoes.IsDoubleRealNumber(((TextBox)itemP.FindControl("txtmarkup21x")).Text.ToString())) ? Funcoes.strToDouble(((TextBox)itemP.FindControl("txtmarkup21x")).Text.ToString()) : 0;

            cmdInsCons.Parameters.Add("@NUM_VALOR_REBATE_DEBITO", SqlDbType.Float).Value = (Funcoes.IsDoubleRealNumber(((TextBox)itemP.FindControl("txtrebatedebito")).Text.ToString())) ? Funcoes.strToDouble(((TextBox)itemP.FindControl("txtrebatedebito")).Text.ToString()) : 0;
            cmdInsCons.Parameters.Add("@NUM_VALOR_REBATE_VISTA", SqlDbType.Float).Value = (Funcoes.IsDoubleRealNumber(((TextBox)itemP.FindControl("txtrebatevista")).Text.ToString())) ? Funcoes.strToDouble(((TextBox)itemP.FindControl("txtrebatevista")).Text.ToString()) : 0;
            cmdInsCons.Parameters.Add("@NUM_VALOR_REBATE_2X", SqlDbType.Float).Value = (Funcoes.IsDoubleRealNumber(((TextBox)itemP.FindControl("txtrebate2x")).Text.ToString())) ? Funcoes.strToDouble(((TextBox)itemP.FindControl("txtrebate2x")).Text.ToString()) : 0;
            cmdInsCons.Parameters.Add("@NUM_VALOR_REBATE_3X", SqlDbType.Float).Value = (Funcoes.IsDoubleRealNumber(((TextBox)itemP.FindControl("txtrebate3x")).Text.ToString())) ? Funcoes.strToDouble(((TextBox)itemP.FindControl("txtrebate3x")).Text.ToString()) : 0;
            cmdInsCons.Parameters.Add("@NUM_VALOR_REBATE_4X", SqlDbType.Float).Value = (Funcoes.IsDoubleRealNumber(((TextBox)itemP.FindControl("txtrebate4x")).Text.ToString())) ? Funcoes.strToDouble(((TextBox)itemP.FindControl("txtrebate4x")).Text.ToString()) : 0;
            cmdInsCons.Parameters.Add("@NUM_VALOR_REBATE_5X", SqlDbType.Float).Value = (Funcoes.IsDoubleRealNumber(((TextBox)itemP.FindControl("txtrebate5x")).Text.ToString())) ? Funcoes.strToDouble(((TextBox)itemP.FindControl("txtrebate5x")).Text.ToString()) : 0;
            cmdInsCons.Parameters.Add("@NUM_VALOR_REBATE_6X", SqlDbType.Float).Value = (Funcoes.IsDoubleRealNumber(((TextBox)itemP.FindControl("txtrebate6x")).Text.ToString())) ? Funcoes.strToDouble(((TextBox)itemP.FindControl("txtrebate6x")).Text.ToString()) : 0;
            cmdInsCons.Parameters.Add("@NUM_VALOR_REBATE_7X", SqlDbType.Float).Value = (Funcoes.IsDoubleRealNumber(((TextBox)itemP.FindControl("txtrebate7x")).Text.ToString())) ? Funcoes.strToDouble(((TextBox)itemP.FindControl("txtrebate7x")).Text.ToString()) : 0;
            cmdInsCons.Parameters.Add("@NUM_VALOR_REBATE_8X", SqlDbType.Float).Value = (Funcoes.IsDoubleRealNumber(((TextBox)itemP.FindControl("txtrebate8x")).Text.ToString())) ? Funcoes.strToDouble(((TextBox)itemP.FindControl("txtrebate8x")).Text.ToString()) : 0;
            cmdInsCons.Parameters.Add("@NUM_VALOR_REBATE_9X", SqlDbType.Float).Value = (Funcoes.IsDoubleRealNumber(((TextBox)itemP.FindControl("txtrebate9x")).Text.ToString())) ? Funcoes.strToDouble(((TextBox)itemP.FindControl("txtrebate9x")).Text.ToString()) : 0;
            cmdInsCons.Parameters.Add("@NUM_VALOR_REBATE_10X", SqlDbType.Float).Value = (Funcoes.IsDoubleRealNumber(((TextBox)itemP.FindControl("txtrebate10x")).Text.ToString())) ? Funcoes.strToDouble(((TextBox)itemP.FindControl("txtrebate10x")).Text.ToString()) : 0;
            cmdInsCons.Parameters.Add("@NUM_VALOR_REBATE_11X", SqlDbType.Float).Value = (Funcoes.IsDoubleRealNumber(((TextBox)itemP.FindControl("txtrebate11x")).Text.ToString())) ? Funcoes.strToDouble(((TextBox)itemP.FindControl("txtrebate11x")).Text.ToString()) : 0;
            cmdInsCons.Parameters.Add("@NUM_VALOR_REBATE_12X", SqlDbType.Float).Value = (Funcoes.IsDoubleRealNumber(((TextBox)itemP.FindControl("txtrebate12x")).Text.ToString())) ? Funcoes.strToDouble(((TextBox)itemP.FindControl("txtrebate12x")).Text.ToString()) : 0;
            cmdInsCons.Parameters.Add("@NUM_VALOR_REBATE_13X", SqlDbType.Float).Value = (Funcoes.IsDoubleRealNumber(((TextBox)itemP.FindControl("txtrebate13x")).Text.ToString())) ? Funcoes.strToDouble(((TextBox)itemP.FindControl("txtrebate13x")).Text.ToString()) : 0;
            cmdInsCons.Parameters.Add("@NUM_VALOR_REBATE_14X", SqlDbType.Float).Value = (Funcoes.IsDoubleRealNumber(((TextBox)itemP.FindControl("txtrebate14x")).Text.ToString())) ? Funcoes.strToDouble(((TextBox)itemP.FindControl("txtrebate14x")).Text.ToString()) : 0;
            cmdInsCons.Parameters.Add("@NUM_VALOR_REBATE_15X", SqlDbType.Float).Value = (Funcoes.IsDoubleRealNumber(((TextBox)itemP.FindControl("txtrebate15x")).Text.ToString())) ? Funcoes.strToDouble(((TextBox)itemP.FindControl("txtrebate15x")).Text.ToString()) : 0;
            cmdInsCons.Parameters.Add("@NUM_VALOR_REBATE_16X", SqlDbType.Float).Value = (Funcoes.IsDoubleRealNumber(((TextBox)itemP.FindControl("txtrebate16x")).Text.ToString())) ? Funcoes.strToDouble(((TextBox)itemP.FindControl("txtrebate16x")).Text.ToString()) : 0;
            cmdInsCons.Parameters.Add("@NUM_VALOR_REBATE_17X", SqlDbType.Float).Value = (Funcoes.IsDoubleRealNumber(((TextBox)itemP.FindControl("txtrebate17x")).Text.ToString())) ? Funcoes.strToDouble(((TextBox)itemP.FindControl("txtrebate17x")).Text.ToString()) : 0;
            cmdInsCons.Parameters.Add("@NUM_VALOR_REBATE_18X", SqlDbType.Float).Value = (Funcoes.IsDoubleRealNumber(((TextBox)itemP.FindControl("txtrebate18x")).Text.ToString())) ? Funcoes.strToDouble(((TextBox)itemP.FindControl("txtrebate18x")).Text.ToString()) : 0;

            cmdInsCons.Parameters.Add("@NUM_VALOR_REBATE_19X", SqlDbType.Float).Value = (Funcoes.IsDoubleRealNumber(((TextBox)itemP.FindControl("txtrebate19x")).Text.ToString())) ? Funcoes.strToDouble(((TextBox)itemP.FindControl("txtrebate19x")).Text.ToString()) : 0;
            cmdInsCons.Parameters.Add("@NUM_VALOR_REBATE_20X", SqlDbType.Float).Value = (Funcoes.IsDoubleRealNumber(((TextBox)itemP.FindControl("txtrebate20x")).Text.ToString())) ? Funcoes.strToDouble(((TextBox)itemP.FindControl("txtrebate20x")).Text.ToString()) : 0;
            cmdInsCons.Parameters.Add("@NUM_VALOR_REBATE_21X", SqlDbType.Float).Value = (Funcoes.IsDoubleRealNumber(((TextBox)itemP.FindControl("txtrebate21x")).Text.ToString())) ? Funcoes.strToDouble(((TextBox)itemP.FindControl("txtrebate21x")).Text.ToString()) : 0;

            cmdInsCons.Parameters.Add("@FLG_PRESENCIAL_ONLINE", SqlDbType.Char).Value = "P";


            cmdInsCons.Parameters.Add("@NUM_VALOR_FIXO_DEBITO", SqlDbType.Float).Value = (Funcoes.IsDoubleRealNumber(((TextBox)itemP.FindControl("txtDebitofixo")).Text.ToString())) ? Funcoes.strToDouble(((TextBox)itemP.FindControl("txtDebitofixo")).Text.ToString()) : 0;
            cmdInsCons.Parameters.Add("@NUM_VALOR_FIXO_CREDITO_VISTA", SqlDbType.Float).Value = (Funcoes.IsDoubleRealNumber(((TextBox)itemP.FindControl("txtcreditofixo")).Text.ToString())) ? Funcoes.strToDouble(((TextBox)itemP.FindControl("txtcreditofixo")).Text.ToString()) : 0;
            cmdInsCons.Parameters.Add("@NUM_VALOR_FIXO_CREDITO_2X", SqlDbType.Float).Value = (Funcoes.IsDoubleRealNumber(((TextBox)itemP.FindControl("txtcredito2xfixo")).Text.ToString())) ? Funcoes.strToDouble(((TextBox)itemP.FindControl("txtcredito2xfixo")).Text.ToString()) : 0;
            cmdInsCons.Parameters.Add("@NUM_VALOR_FIXO_CREDITO_3X", SqlDbType.Float).Value = (Funcoes.IsDoubleRealNumber(((TextBox)itemP.FindControl("txtcredito3xfixo")).Text.ToString())) ? Funcoes.strToDouble(((TextBox)itemP.FindControl("txtcredito3xfixo")).Text.ToString()) : 0;
            cmdInsCons.Parameters.Add("@NUM_VALOR_FIXO_CREDITO_4X", SqlDbType.Float).Value = (Funcoes.IsDoubleRealNumber(((TextBox)itemP.FindControl("txtcredito4xfixo")).Text.ToString())) ? Funcoes.strToDouble(((TextBox)itemP.FindControl("txtcredito4xfixo")).Text.ToString()) : 0;
            cmdInsCons.Parameters.Add("@NUM_VALOR_FIXO_CREDITO_5X", SqlDbType.Float).Value = (Funcoes.IsDoubleRealNumber(((TextBox)itemP.FindControl("txtcredito5xfixo")).Text.ToString())) ? Funcoes.strToDouble(((TextBox)itemP.FindControl("txtcredito5xfixo")).Text.ToString()) : 0;
            cmdInsCons.Parameters.Add("@NUM_VALOR_FIXO_CREDITO_6X", SqlDbType.Float).Value = (Funcoes.IsDoubleRealNumber(((TextBox)itemP.FindControl("txtcredito6xfixo")).Text.ToString())) ? Funcoes.strToDouble(((TextBox)itemP.FindControl("txtcredito6xfixo")).Text.ToString()) : 0;
            cmdInsCons.Parameters.Add("@NUM_VALOR_FIXO_CREDITO_7X", SqlDbType.Float).Value = (Funcoes.IsDoubleRealNumber(((TextBox)itemP.FindControl("txtcredito7xfixo")).Text.ToString())) ? Funcoes.strToDouble(((TextBox)itemP.FindControl("txtcredito7xfixo")).Text.ToString()) : 0;
            cmdInsCons.Parameters.Add("@NUM_VALOR_FIXO_CREDITO_8X", SqlDbType.Float).Value = (Funcoes.IsDoubleRealNumber(((TextBox)itemP.FindControl("txtcredito8xfixo")).Text.ToString())) ? Funcoes.strToDouble(((TextBox)itemP.FindControl("txtcredito8xfixo")).Text.ToString()) : 0;
            cmdInsCons.Parameters.Add("@NUM_VALOR_FIXO_CREDITO_9X", SqlDbType.Float).Value = (Funcoes.IsDoubleRealNumber(((TextBox)itemP.FindControl("txtcredito9xfixo")).Text.ToString())) ? Funcoes.strToDouble(((TextBox)itemP.FindControl("txtcredito9xfixo")).Text.ToString()) : 0;
            cmdInsCons.Parameters.Add("@NUM_VALOR_FIXO_CREDITO_10X", SqlDbType.Float).Value = (Funcoes.IsDoubleRealNumber(((TextBox)itemP.FindControl("txtcredito10xfixo")).Text.ToString())) ? Funcoes.strToDouble(((TextBox)itemP.FindControl("txtcredito10xfixo")).Text.ToString()) : 0;
            cmdInsCons.Parameters.Add("@NUM_VALOR_FIXO_CREDITO_11X", SqlDbType.Float).Value = (Funcoes.IsDoubleRealNumber(((TextBox)itemP.FindControl("txtcredito11xfixo")).Text.ToString())) ? Funcoes.strToDouble(((TextBox)itemP.FindControl("txtcredito11xfixo")).Text.ToString()) : 0;
            cmdInsCons.Parameters.Add("@NUM_VALOR_FIXO_CREDITO_12X", SqlDbType.Float).Value = (Funcoes.IsDoubleRealNumber(((TextBox)itemP.FindControl("txtcredito12xfixo")).Text.ToString())) ? Funcoes.strToDouble(((TextBox)itemP.FindControl("txtcredito12xfixo")).Text.ToString()) : 0;
            cmdInsCons.Parameters.Add("@NUM_VALOR_FIXO_CREDITO_13X", SqlDbType.Float).Value = (Funcoes.IsDoubleRealNumber(((TextBox)itemP.FindControl("txtcredito13xfixo")).Text.ToString())) ? Funcoes.strToDouble(((TextBox)itemP.FindControl("txtcredito13xfixo")).Text.ToString()) : 0;
            cmdInsCons.Parameters.Add("@NUM_VALOR_FIXO_CREDITO_14X", SqlDbType.Float).Value = (Funcoes.IsDoubleRealNumber(((TextBox)itemP.FindControl("txtcredito14xfixo")).Text.ToString())) ? Funcoes.strToDouble(((TextBox)itemP.FindControl("txtcredito14xfixo")).Text.ToString()) : 0;
            cmdInsCons.Parameters.Add("@NUM_VALOR_FIXO_CREDITO_15X", SqlDbType.Float).Value = (Funcoes.IsDoubleRealNumber(((TextBox)itemP.FindControl("txtcredito15xfixo")).Text.ToString())) ? Funcoes.strToDouble(((TextBox)itemP.FindControl("txtcredito15xfixo")).Text.ToString()) : 0;
            cmdInsCons.Parameters.Add("@NUM_VALOR_FIXO_CREDITO_16X", SqlDbType.Float).Value = (Funcoes.IsDoubleRealNumber(((TextBox)itemP.FindControl("txtcredito16xfixo")).Text.ToString())) ? Funcoes.strToDouble(((TextBox)itemP.FindControl("txtcredito16xfixo")).Text.ToString()) : 0;
            cmdInsCons.Parameters.Add("@NUM_VALOR_FIXO_CREDITO_17X", SqlDbType.Float).Value = (Funcoes.IsDoubleRealNumber(((TextBox)itemP.FindControl("txtcredito17xfixo")).Text.ToString())) ? Funcoes.strToDouble(((TextBox)itemP.FindControl("txtcredito17xfixo")).Text.ToString()) : 0;
            cmdInsCons.Parameters.Add("@NUM_VALOR_FIXO_CREDITO_18X", SqlDbType.Float).Value = (Funcoes.IsDoubleRealNumber(((TextBox)itemP.FindControl("txtcredito18xfixo")).Text.ToString())) ? Funcoes.strToDouble(((TextBox)itemP.FindControl("txtcredito18xfixo")).Text.ToString()) : 0;

            cmdInsCons.Parameters.Add("@NUM_VALOR_FIXO_CREDITO_19X", SqlDbType.Float).Value = (Funcoes.IsDoubleRealNumber(((TextBox)itemP.FindControl("txtcredito19xfixo")).Text.ToString())) ? Funcoes.strToDouble(((TextBox)itemP.FindControl("txtcredito19xfixo")).Text.ToString()) : 0;
            cmdInsCons.Parameters.Add("@NUM_VALOR_FIXO_CREDITO_20X", SqlDbType.Float).Value = (Funcoes.IsDoubleRealNumber(((TextBox)itemP.FindControl("txtcredito20xfixo")).Text.ToString())) ? Funcoes.strToDouble(((TextBox)itemP.FindControl("txtcredito20xfixo")).Text.ToString()) : 0;
            cmdInsCons.Parameters.Add("@NUM_VALOR_FIXO_CREDITO_21X", SqlDbType.Float).Value = (Funcoes.IsDoubleRealNumber(((TextBox)itemP.FindControl("txtcredito21xfixo")).Text.ToString())) ? Funcoes.strToDouble(((TextBox)itemP.FindControl("txtcredito21xfixo")).Text.ToString()) : 0;


            cmdInsCons.Parameters.Add("@NUM_VALOR_FIXO_MARKUP_DEBITO", SqlDbType.Float).Value = (Funcoes.IsDoubleRealNumber(((TextBox)itemP.FindControl("txtmarkupdebitofixo")).Text.ToString())) ? Funcoes.strToDouble(((TextBox)itemP.FindControl("txtmarkupdebitofixo")).Text.ToString()) : 0;
            cmdInsCons.Parameters.Add("@NUM_VALOR_FIXO_MARKUP_VISTA", SqlDbType.Float).Value = (Funcoes.IsDoubleRealNumber(((TextBox)itemP.FindControl("txtmarkupcreditofixo")).Text.ToString())) ? Funcoes.strToDouble(((TextBox)itemP.FindControl("txtmarkupcreditofixo")).Text.ToString()) : 0;
            cmdInsCons.Parameters.Add("@NUM_VALOR_FIXO_MARKUP_2X", SqlDbType.Float).Value = (Funcoes.IsDoubleRealNumber(((TextBox)itemP.FindControl("txtmarkup2xfixo")).Text.ToString())) ? Funcoes.strToDouble(((TextBox)itemP.FindControl("txtmarkup2xfixo")).Text.ToString()) : 0;
            cmdInsCons.Parameters.Add("@NUM_VALOR_FIXO_MARKUP_3X", SqlDbType.Float).Value = (Funcoes.IsDoubleRealNumber(((TextBox)itemP.FindControl("txtmarkup3xfixo")).Text.ToString())) ? Funcoes.strToDouble(((TextBox)itemP.FindControl("txtmarkup3xfixo")).Text.ToString()) : 0;
            cmdInsCons.Parameters.Add("@NUM_VALOR_FIXO_MARKUP_4X", SqlDbType.Float).Value = (Funcoes.IsDoubleRealNumber(((TextBox)itemP.FindControl("txtmarkup4xfixo")).Text.ToString())) ? Funcoes.strToDouble(((TextBox)itemP.FindControl("txtmarkup4xfixo")).Text.ToString()) : 0;
            cmdInsCons.Parameters.Add("@NUM_VALOR_FIXO_MARKUP_5X", SqlDbType.Float).Value = (Funcoes.IsDoubleRealNumber(((TextBox)itemP.FindControl("txtmarkup5xfixo")).Text.ToString())) ? Funcoes.strToDouble(((TextBox)itemP.FindControl("txtmarkup5xfixo")).Text.ToString()) : 0;
            cmdInsCons.Parameters.Add("@NUM_VALOR_FIXO_MARKUP_6X", SqlDbType.Float).Value = (Funcoes.IsDoubleRealNumber(((TextBox)itemP.FindControl("txtmarkup6xfixo")).Text.ToString())) ? Funcoes.strToDouble(((TextBox)itemP.FindControl("txtmarkup6xfixo")).Text.ToString()) : 0;
            cmdInsCons.Parameters.Add("@NUM_VALOR_FIXO_MARKUP_7X", SqlDbType.Float).Value = (Funcoes.IsDoubleRealNumber(((TextBox)itemP.FindControl("txtmarkup7xfixo")).Text.ToString())) ? Funcoes.strToDouble(((TextBox)itemP.FindControl("txtmarkup7xfixo")).Text.ToString()) : 0;
            cmdInsCons.Parameters.Add("@NUM_VALOR_FIXO_MARKUP_8X", SqlDbType.Float).Value = (Funcoes.IsDoubleRealNumber(((TextBox)itemP.FindControl("txtmarkup8xfixo")).Text.ToString())) ? Funcoes.strToDouble(((TextBox)itemP.FindControl("txtmarkup8xfixo")).Text.ToString()) : 0;
            cmdInsCons.Parameters.Add("@NUM_VALOR_FIXO_MARKUP_9X", SqlDbType.Float).Value = (Funcoes.IsDoubleRealNumber(((TextBox)itemP.FindControl("txtmarkup9xfixo")).Text.ToString())) ? Funcoes.strToDouble(((TextBox)itemP.FindControl("txtmarkup9xfixo")).Text.ToString()) : 0;
            cmdInsCons.Parameters.Add("@NUM_VALOR_FIXO_MARKUP_10X", SqlDbType.Float).Value = (Funcoes.IsDoubleRealNumber(((TextBox)itemP.FindControl("txtmarkup10xfixo")).Text.ToString())) ? Funcoes.strToDouble(((TextBox)itemP.FindControl("txtmarkup10xfixo")).Text.ToString()) : 0;
            cmdInsCons.Parameters.Add("@NUM_VALOR_FIXO_MARKUP_11X", SqlDbType.Float).Value = (Funcoes.IsDoubleRealNumber(((TextBox)itemP.FindControl("txtmarkup11xfixo")).Text.ToString())) ? Funcoes.strToDouble(((TextBox)itemP.FindControl("txtmarkup11xfixo")).Text.ToString()) : 0;
            cmdInsCons.Parameters.Add("@NUM_VALOR_FIXO_MARKUP_12X", SqlDbType.Float).Value = (Funcoes.IsDoubleRealNumber(((TextBox)itemP.FindControl("txtmarkup12xfixo")).Text.ToString())) ? Funcoes.strToDouble(((TextBox)itemP.FindControl("txtmarkup12xfixo")).Text.ToString()) : 0;
            cmdInsCons.Parameters.Add("@NUM_VALOR_FIXO_MARKUP_13X", SqlDbType.Float).Value = (Funcoes.IsDoubleRealNumber(((TextBox)itemP.FindControl("txtmarkup13xfixo")).Text.ToString())) ? Funcoes.strToDouble(((TextBox)itemP.FindControl("txtmarkup13xfixo")).Text.ToString()) : 0;
            cmdInsCons.Parameters.Add("@NUM_VALOR_FIXO_MARKUP_14X", SqlDbType.Float).Value = (Funcoes.IsDoubleRealNumber(((TextBox)itemP.FindControl("txtmarkup14xfixo")).Text.ToString())) ? Funcoes.strToDouble(((TextBox)itemP.FindControl("txtmarkup14xfixo")).Text.ToString()) : 0;
            cmdInsCons.Parameters.Add("@NUM_VALOR_FIXO_MARKUP_15X", SqlDbType.Float).Value = (Funcoes.IsDoubleRealNumber(((TextBox)itemP.FindControl("txtmarkup15xfixo")).Text.ToString())) ? Funcoes.strToDouble(((TextBox)itemP.FindControl("txtmarkup15xfixo")).Text.ToString()) : 0;
            cmdInsCons.Parameters.Add("@NUM_VALOR_FIXO_MARKUP_16X", SqlDbType.Float).Value = (Funcoes.IsDoubleRealNumber(((TextBox)itemP.FindControl("txtmarkup16xfixo")).Text.ToString())) ? Funcoes.strToDouble(((TextBox)itemP.FindControl("txtmarkup16xfixo")).Text.ToString()) : 0;
            cmdInsCons.Parameters.Add("@NUM_VALOR_FIXO_MARKUP_17X", SqlDbType.Float).Value = (Funcoes.IsDoubleRealNumber(((TextBox)itemP.FindControl("txtmarkup17xfixo")).Text.ToString())) ? Funcoes.strToDouble(((TextBox)itemP.FindControl("txtmarkup17xfixo")).Text.ToString()) : 0;
            cmdInsCons.Parameters.Add("@NUM_VALOR_FIXO_MARKUP_18X", SqlDbType.Float).Value = (Funcoes.IsDoubleRealNumber(((TextBox)itemP.FindControl("txtmarkup18xfixo")).Text.ToString())) ? Funcoes.strToDouble(((TextBox)itemP.FindControl("txtmarkup18xfixo")).Text.ToString()) : 0;

            cmdInsCons.Parameters.Add("@NUM_VALOR_FIXO_MARKUP_19X", SqlDbType.Float).Value = (Funcoes.IsDoubleRealNumber(((TextBox)itemP.FindControl("txtmarkup19xfixo")).Text.ToString())) ? Funcoes.strToDouble(((TextBox)itemP.FindControl("txtmarkup19xfixo")).Text.ToString()) : 0;
            cmdInsCons.Parameters.Add("@NUM_VALOR_FIXO_MARKUP_20X", SqlDbType.Float).Value = (Funcoes.IsDoubleRealNumber(((TextBox)itemP.FindControl("txtmarkup20xfixo")).Text.ToString())) ? Funcoes.strToDouble(((TextBox)itemP.FindControl("txtmarkup20xfixo")).Text.ToString()) : 0;
            cmdInsCons.Parameters.Add("@NUM_VALOR_FIXO_MARKUP_21X", SqlDbType.Float).Value = (Funcoes.IsDoubleRealNumber(((TextBox)itemP.FindControl("txtmarkup21xfixo")).Text.ToString())) ? Funcoes.strToDouble(((TextBox)itemP.FindControl("txtmarkup21xfixo")).Text.ToString()) : 0;



            cmdInsCons.ExecuteNonQuery();
            connInsCons.Close();
            connInsCons.Dispose();
        }

    }

    private void GravarParcelasOnline()
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
            
            cmdInsCons.Parameters.Add("@NUM_VALOR_CREDITO_19X", SqlDbType.Float).Value = (Funcoes.IsDoubleRealNumber(((TextBox)itemP.FindControl("txtcredito19x")).Text.ToString())) ? Funcoes.strToDouble(((TextBox)itemP.FindControl("txtcredito19x")).Text.ToString()) : 0;
            cmdInsCons.Parameters.Add("@NUM_VALOR_CREDITO_20X", SqlDbType.Float).Value = (Funcoes.IsDoubleRealNumber(((TextBox)itemP.FindControl("txtcredito20x")).Text.ToString())) ? Funcoes.strToDouble(((TextBox)itemP.FindControl("txtcredito20x")).Text.ToString()) : 0;
            cmdInsCons.Parameters.Add("@NUM_VALOR_CREDITO_21X", SqlDbType.Float).Value = (Funcoes.IsDoubleRealNumber(((TextBox)itemP.FindControl("txtcredito21x")).Text.ToString())) ? Funcoes.strToDouble(((TextBox)itemP.FindControl("txtcredito21x")).Text.ToString()) : 0;
            
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

            cmdInsCons.Parameters.Add("@NUM_VALOR_MARKUP_19X", SqlDbType.Float).Value = (Funcoes.IsDoubleRealNumber(((TextBox)itemP.FindControl("txtmarkup19x")).Text.ToString())) ? Funcoes.strToDouble(((TextBox)itemP.FindControl("txtmarkup19x")).Text.ToString()) : 0;
            cmdInsCons.Parameters.Add("@NUM_VALOR_MARKUP_20X", SqlDbType.Float).Value = (Funcoes.IsDoubleRealNumber(((TextBox)itemP.FindControl("txtmarkup20x")).Text.ToString())) ? Funcoes.strToDouble(((TextBox)itemP.FindControl("txtmarkup20x")).Text.ToString()) : 0;
            cmdInsCons.Parameters.Add("@NUM_VALOR_MARKUP_21X", SqlDbType.Float).Value = (Funcoes.IsDoubleRealNumber(((TextBox)itemP.FindControl("txtmarkup21x")).Text.ToString())) ? Funcoes.strToDouble(((TextBox)itemP.FindControl("txtmarkup21x")).Text.ToString()) : 0;

            cmdInsCons.Parameters.Add("@NUM_VALOR_REBATE_DEBITO", SqlDbType.Float).Value = (Funcoes.IsDoubleRealNumber(((TextBox)itemP.FindControl("txtrebatedebito")).Text.ToString())) ? Funcoes.strToDouble(((TextBox)itemP.FindControl("txtrebatedebito")).Text.ToString()) : 0;
            cmdInsCons.Parameters.Add("@NUM_VALOR_REBATE_VISTA", SqlDbType.Float).Value = (Funcoes.IsDoubleRealNumber(((TextBox)itemP.FindControl("txtrebatevista")).Text.ToString())) ? Funcoes.strToDouble(((TextBox)itemP.FindControl("txtrebatevista")).Text.ToString()) : 0;
            cmdInsCons.Parameters.Add("@NUM_VALOR_REBATE_2X", SqlDbType.Float).Value = (Funcoes.IsDoubleRealNumber(((TextBox)itemP.FindControl("txtrebate2x")).Text.ToString())) ? Funcoes.strToDouble(((TextBox)itemP.FindControl("txtrebate2x")).Text.ToString()) : 0;
            cmdInsCons.Parameters.Add("@NUM_VALOR_REBATE_3X", SqlDbType.Float).Value = (Funcoes.IsDoubleRealNumber(((TextBox)itemP.FindControl("txtrebate3x")).Text.ToString())) ? Funcoes.strToDouble(((TextBox)itemP.FindControl("txtrebate3x")).Text.ToString()) : 0;
            cmdInsCons.Parameters.Add("@NUM_VALOR_REBATE_4X", SqlDbType.Float).Value = (Funcoes.IsDoubleRealNumber(((TextBox)itemP.FindControl("txtrebate4x")).Text.ToString())) ? Funcoes.strToDouble(((TextBox)itemP.FindControl("txtrebate4x")).Text.ToString()) : 0;
            cmdInsCons.Parameters.Add("@NUM_VALOR_REBATE_5X", SqlDbType.Float).Value = (Funcoes.IsDoubleRealNumber(((TextBox)itemP.FindControl("txtrebate5x")).Text.ToString())) ? Funcoes.strToDouble(((TextBox)itemP.FindControl("txtrebate5x")).Text.ToString()) : 0;
            cmdInsCons.Parameters.Add("@NUM_VALOR_REBATE_6X", SqlDbType.Float).Value = (Funcoes.IsDoubleRealNumber(((TextBox)itemP.FindControl("txtrebate6x")).Text.ToString())) ? Funcoes.strToDouble(((TextBox)itemP.FindControl("txtrebate6x")).Text.ToString()) : 0;
            cmdInsCons.Parameters.Add("@NUM_VALOR_REBATE_7X", SqlDbType.Float).Value = (Funcoes.IsDoubleRealNumber(((TextBox)itemP.FindControl("txtrebate7x")).Text.ToString())) ? Funcoes.strToDouble(((TextBox)itemP.FindControl("txtrebate7x")).Text.ToString()) : 0;
            cmdInsCons.Parameters.Add("@NUM_VALOR_REBATE_8X", SqlDbType.Float).Value = (Funcoes.IsDoubleRealNumber(((TextBox)itemP.FindControl("txtrebate8x")).Text.ToString())) ? Funcoes.strToDouble(((TextBox)itemP.FindControl("txtrebate8x")).Text.ToString()) : 0;
            cmdInsCons.Parameters.Add("@NUM_VALOR_REBATE_9X", SqlDbType.Float).Value = (Funcoes.IsDoubleRealNumber(((TextBox)itemP.FindControl("txtrebate9x")).Text.ToString())) ? Funcoes.strToDouble(((TextBox)itemP.FindControl("txtrebate9x")).Text.ToString()) : 0;
            cmdInsCons.Parameters.Add("@NUM_VALOR_REBATE_10X", SqlDbType.Float).Value = (Funcoes.IsDoubleRealNumber(((TextBox)itemP.FindControl("txtrebate10x")).Text.ToString())) ? Funcoes.strToDouble(((TextBox)itemP.FindControl("txtrebate10x")).Text.ToString()) : 0;
            cmdInsCons.Parameters.Add("@NUM_VALOR_REBATE_11X", SqlDbType.Float).Value = (Funcoes.IsDoubleRealNumber(((TextBox)itemP.FindControl("txtrebate11x")).Text.ToString())) ? Funcoes.strToDouble(((TextBox)itemP.FindControl("txtrebate11x")).Text.ToString()) : 0;
            cmdInsCons.Parameters.Add("@NUM_VALOR_REBATE_12X", SqlDbType.Float).Value = (Funcoes.IsDoubleRealNumber(((TextBox)itemP.FindControl("txtrebate12x")).Text.ToString())) ? Funcoes.strToDouble(((TextBox)itemP.FindControl("txtrebate12x")).Text.ToString()) : 0;
            cmdInsCons.Parameters.Add("@NUM_VALOR_REBATE_13X", SqlDbType.Float).Value = (Funcoes.IsDoubleRealNumber(((TextBox)itemP.FindControl("txtrebate13x")).Text.ToString())) ? Funcoes.strToDouble(((TextBox)itemP.FindControl("txtrebate13x")).Text.ToString()) : 0;
            cmdInsCons.Parameters.Add("@NUM_VALOR_REBATE_14X", SqlDbType.Float).Value = (Funcoes.IsDoubleRealNumber(((TextBox)itemP.FindControl("txtrebate14x")).Text.ToString())) ? Funcoes.strToDouble(((TextBox)itemP.FindControl("txtrebate14x")).Text.ToString()) : 0;
            cmdInsCons.Parameters.Add("@NUM_VALOR_REBATE_15X", SqlDbType.Float).Value = (Funcoes.IsDoubleRealNumber(((TextBox)itemP.FindControl("txtrebate15x")).Text.ToString())) ? Funcoes.strToDouble(((TextBox)itemP.FindControl("txtrebate15x")).Text.ToString()) : 0;
            cmdInsCons.Parameters.Add("@NUM_VALOR_REBATE_16X", SqlDbType.Float).Value = (Funcoes.IsDoubleRealNumber(((TextBox)itemP.FindControl("txtrebate16x")).Text.ToString())) ? Funcoes.strToDouble(((TextBox)itemP.FindControl("txtrebate16x")).Text.ToString()) : 0;
            cmdInsCons.Parameters.Add("@NUM_VALOR_REBATE_17X", SqlDbType.Float).Value = (Funcoes.IsDoubleRealNumber(((TextBox)itemP.FindControl("txtrebate17x")).Text.ToString())) ? Funcoes.strToDouble(((TextBox)itemP.FindControl("txtrebate17x")).Text.ToString()) : 0;
            cmdInsCons.Parameters.Add("@NUM_VALOR_REBATE_18X", SqlDbType.Float).Value = (Funcoes.IsDoubleRealNumber(((TextBox)itemP.FindControl("txtrebate18x")).Text.ToString())) ? Funcoes.strToDouble(((TextBox)itemP.FindControl("txtrebate18x")).Text.ToString()) : 0;

            cmdInsCons.Parameters.Add("@NUM_VALOR_REBATE_19X", SqlDbType.Float).Value = (Funcoes.IsDoubleRealNumber(((TextBox)itemP.FindControl("txtrebate19x")).Text.ToString())) ? Funcoes.strToDouble(((TextBox)itemP.FindControl("txtrebate19x")).Text.ToString()) : 0;
            cmdInsCons.Parameters.Add("@NUM_VALOR_REBATE_20X", SqlDbType.Float).Value = (Funcoes.IsDoubleRealNumber(((TextBox)itemP.FindControl("txtrebate20x")).Text.ToString())) ? Funcoes.strToDouble(((TextBox)itemP.FindControl("txtrebate20x")).Text.ToString()) : 0;
            cmdInsCons.Parameters.Add("@NUM_VALOR_REBATE_21X", SqlDbType.Float).Value = (Funcoes.IsDoubleRealNumber(((TextBox)itemP.FindControl("txtrebate21x")).Text.ToString())) ? Funcoes.strToDouble(((TextBox)itemP.FindControl("txtrebate21x")).Text.ToString()) : 0;


            cmdInsCons.Parameters.Add("@FLG_PRESENCIAL_ONLINE", SqlDbType.Char).Value = "O";

            cmdInsCons.Parameters.Add("@NUM_VALOR_FIXO_DEBITO", SqlDbType.Float).Value = (Funcoes.IsDoubleRealNumber(((TextBox)itemP.FindControl("txtDebitofixo")).Text.ToString())) ? Funcoes.strToDouble(((TextBox)itemP.FindControl("txtDebitofixo")).Text.ToString()) : 0;
            cmdInsCons.Parameters.Add("@NUM_VALOR_FIXO_CREDITO_VISTA", SqlDbType.Float).Value = (Funcoes.IsDoubleRealNumber(((TextBox)itemP.FindControl("txtcreditofixo")).Text.ToString())) ? Funcoes.strToDouble(((TextBox)itemP.FindControl("txtcreditofixo")).Text.ToString()) : 0;
            cmdInsCons.Parameters.Add("@NUM_VALOR_FIXO_CREDITO_2X", SqlDbType.Float).Value = (Funcoes.IsDoubleRealNumber(((TextBox)itemP.FindControl("txtcredito2xfixo")).Text.ToString())) ? Funcoes.strToDouble(((TextBox)itemP.FindControl("txtcredito2xfixo")).Text.ToString()) : 0;
            cmdInsCons.Parameters.Add("@NUM_VALOR_FIXO_CREDITO_3X", SqlDbType.Float).Value = (Funcoes.IsDoubleRealNumber(((TextBox)itemP.FindControl("txtcredito3xfixo")).Text.ToString())) ? Funcoes.strToDouble(((TextBox)itemP.FindControl("txtcredito3xfixo")).Text.ToString()) : 0;
            cmdInsCons.Parameters.Add("@NUM_VALOR_FIXO_CREDITO_4X", SqlDbType.Float).Value = (Funcoes.IsDoubleRealNumber(((TextBox)itemP.FindControl("txtcredito4xfixo")).Text.ToString())) ? Funcoes.strToDouble(((TextBox)itemP.FindControl("txtcredito4xfixo")).Text.ToString()) : 0;
            cmdInsCons.Parameters.Add("@NUM_VALOR_FIXO_CREDITO_5X", SqlDbType.Float).Value = (Funcoes.IsDoubleRealNumber(((TextBox)itemP.FindControl("txtcredito5xfixo")).Text.ToString())) ? Funcoes.strToDouble(((TextBox)itemP.FindControl("txtcredito5xfixo")).Text.ToString()) : 0;
            cmdInsCons.Parameters.Add("@NUM_VALOR_FIXO_CREDITO_6X", SqlDbType.Float).Value = (Funcoes.IsDoubleRealNumber(((TextBox)itemP.FindControl("txtcredito6xfixo")).Text.ToString())) ? Funcoes.strToDouble(((TextBox)itemP.FindControl("txtcredito6xfixo")).Text.ToString()) : 0;
            cmdInsCons.Parameters.Add("@NUM_VALOR_FIXO_CREDITO_7X", SqlDbType.Float).Value = (Funcoes.IsDoubleRealNumber(((TextBox)itemP.FindControl("txtcredito7xfixo")).Text.ToString())) ? Funcoes.strToDouble(((TextBox)itemP.FindControl("txtcredito7xfixo")).Text.ToString()) : 0;
            cmdInsCons.Parameters.Add("@NUM_VALOR_FIXO_CREDITO_8X", SqlDbType.Float).Value = (Funcoes.IsDoubleRealNumber(((TextBox)itemP.FindControl("txtcredito8xfixo")).Text.ToString())) ? Funcoes.strToDouble(((TextBox)itemP.FindControl("txtcredito8xfixo")).Text.ToString()) : 0;
            cmdInsCons.Parameters.Add("@NUM_VALOR_FIXO_CREDITO_9X", SqlDbType.Float).Value = (Funcoes.IsDoubleRealNumber(((TextBox)itemP.FindControl("txtcredito9xfixo")).Text.ToString())) ? Funcoes.strToDouble(((TextBox)itemP.FindControl("txtcredito9xfixo")).Text.ToString()) : 0;
            cmdInsCons.Parameters.Add("@NUM_VALOR_FIXO_CREDITO_10X", SqlDbType.Float).Value = (Funcoes.IsDoubleRealNumber(((TextBox)itemP.FindControl("txtcredito10xfixo")).Text.ToString())) ? Funcoes.strToDouble(((TextBox)itemP.FindControl("txtcredito10xfixo")).Text.ToString()) : 0;
            cmdInsCons.Parameters.Add("@NUM_VALOR_FIXO_CREDITO_11X", SqlDbType.Float).Value = (Funcoes.IsDoubleRealNumber(((TextBox)itemP.FindControl("txtcredito11xfixo")).Text.ToString())) ? Funcoes.strToDouble(((TextBox)itemP.FindControl("txtcredito11xfixo")).Text.ToString()) : 0;
            cmdInsCons.Parameters.Add("@NUM_VALOR_FIXO_CREDITO_12X", SqlDbType.Float).Value = (Funcoes.IsDoubleRealNumber(((TextBox)itemP.FindControl("txtcredito12xfixo")).Text.ToString())) ? Funcoes.strToDouble(((TextBox)itemP.FindControl("txtcredito12xfixo")).Text.ToString()) : 0;
            cmdInsCons.Parameters.Add("@NUM_VALOR_FIXO_CREDITO_13X", SqlDbType.Float).Value = (Funcoes.IsDoubleRealNumber(((TextBox)itemP.FindControl("txtcredito13xfixo")).Text.ToString())) ? Funcoes.strToDouble(((TextBox)itemP.FindControl("txtcredito13xfixo")).Text.ToString()) : 0;
            cmdInsCons.Parameters.Add("@NUM_VALOR_FIXO_CREDITO_14X", SqlDbType.Float).Value = (Funcoes.IsDoubleRealNumber(((TextBox)itemP.FindControl("txtcredito14xfixo")).Text.ToString())) ? Funcoes.strToDouble(((TextBox)itemP.FindControl("txtcredito14xfixo")).Text.ToString()) : 0;
            cmdInsCons.Parameters.Add("@NUM_VALOR_FIXO_CREDITO_15X", SqlDbType.Float).Value = (Funcoes.IsDoubleRealNumber(((TextBox)itemP.FindControl("txtcredito15xfixo")).Text.ToString())) ? Funcoes.strToDouble(((TextBox)itemP.FindControl("txtcredito15xfixo")).Text.ToString()) : 0;
            cmdInsCons.Parameters.Add("@NUM_VALOR_FIXO_CREDITO_16X", SqlDbType.Float).Value = (Funcoes.IsDoubleRealNumber(((TextBox)itemP.FindControl("txtcredito16xfixo")).Text.ToString())) ? Funcoes.strToDouble(((TextBox)itemP.FindControl("txtcredito16xfixo")).Text.ToString()) : 0;
            cmdInsCons.Parameters.Add("@NUM_VALOR_FIXO_CREDITO_17X", SqlDbType.Float).Value = (Funcoes.IsDoubleRealNumber(((TextBox)itemP.FindControl("txtcredito17xfixo")).Text.ToString())) ? Funcoes.strToDouble(((TextBox)itemP.FindControl("txtcredito17xfixo")).Text.ToString()) : 0;
            cmdInsCons.Parameters.Add("@NUM_VALOR_FIXO_CREDITO_18X", SqlDbType.Float).Value = (Funcoes.IsDoubleRealNumber(((TextBox)itemP.FindControl("txtcredito18xfixo")).Text.ToString())) ? Funcoes.strToDouble(((TextBox)itemP.FindControl("txtcredito18xfixo")).Text.ToString()) : 0;

            cmdInsCons.Parameters.Add("@NUM_VALOR_FIXO_CREDITO_19X", SqlDbType.Float).Value = (Funcoes.IsDoubleRealNumber(((TextBox)itemP.FindControl("txtcredito19xfixo")).Text.ToString())) ? Funcoes.strToDouble(((TextBox)itemP.FindControl("txtcredito19xfixo")).Text.ToString()) : 0;
            cmdInsCons.Parameters.Add("@NUM_VALOR_FIXO_CREDITO_20X", SqlDbType.Float).Value = (Funcoes.IsDoubleRealNumber(((TextBox)itemP.FindControl("txtcredito20xfixo")).Text.ToString())) ? Funcoes.strToDouble(((TextBox)itemP.FindControl("txtcredito20xfixo")).Text.ToString()) : 0;
            cmdInsCons.Parameters.Add("@NUM_VALOR_FIXO_CREDITO_21X", SqlDbType.Float).Value = (Funcoes.IsDoubleRealNumber(((TextBox)itemP.FindControl("txtcredito21xfixo")).Text.ToString())) ? Funcoes.strToDouble(((TextBox)itemP.FindControl("txtcredito21xfixo")).Text.ToString()) : 0;

            cmdInsCons.Parameters.Add("@NUM_VALOR_FIXO_MARKUP_DEBITO", SqlDbType.Float).Value = (Funcoes.IsDoubleRealNumber(((TextBox)itemP.FindControl("txtmarkupdebitofixo")).Text.ToString())) ? Funcoes.strToDouble(((TextBox)itemP.FindControl("txtmarkupdebitofixo")).Text.ToString()) : 0;
            cmdInsCons.Parameters.Add("@NUM_VALOR_FIXO_MARKUP_VISTA", SqlDbType.Float).Value = (Funcoes.IsDoubleRealNumber(((TextBox)itemP.FindControl("txtmarkupcreditofixo")).Text.ToString())) ? Funcoes.strToDouble(((TextBox)itemP.FindControl("txtmarkupcreditofixo")).Text.ToString()) : 0;
            cmdInsCons.Parameters.Add("@NUM_VALOR_FIXO_MARKUP_2X", SqlDbType.Float).Value = (Funcoes.IsDoubleRealNumber(((TextBox)itemP.FindControl("txtmarkup2xfixo")).Text.ToString())) ? Funcoes.strToDouble(((TextBox)itemP.FindControl("txtmarkup2xfixo")).Text.ToString()) : 0;
            cmdInsCons.Parameters.Add("@NUM_VALOR_FIXO_MARKUP_3X", SqlDbType.Float).Value = (Funcoes.IsDoubleRealNumber(((TextBox)itemP.FindControl("txtmarkup3xfixo")).Text.ToString())) ? Funcoes.strToDouble(((TextBox)itemP.FindControl("txtmarkup3xfixo")).Text.ToString()) : 0;
            cmdInsCons.Parameters.Add("@NUM_VALOR_FIXO_MARKUP_4X", SqlDbType.Float).Value = (Funcoes.IsDoubleRealNumber(((TextBox)itemP.FindControl("txtmarkup4xfixo")).Text.ToString())) ? Funcoes.strToDouble(((TextBox)itemP.FindControl("txtmarkup4xfixo")).Text.ToString()) : 0;
            cmdInsCons.Parameters.Add("@NUM_VALOR_FIXO_MARKUP_5X", SqlDbType.Float).Value = (Funcoes.IsDoubleRealNumber(((TextBox)itemP.FindControl("txtmarkup5xfixo")).Text.ToString())) ? Funcoes.strToDouble(((TextBox)itemP.FindControl("txtmarkup5xfixo")).Text.ToString()) : 0;
            cmdInsCons.Parameters.Add("@NUM_VALOR_FIXO_MARKUP_6X", SqlDbType.Float).Value = (Funcoes.IsDoubleRealNumber(((TextBox)itemP.FindControl("txtmarkup6xfixo")).Text.ToString())) ? Funcoes.strToDouble(((TextBox)itemP.FindControl("txtmarkup6xfixo")).Text.ToString()) : 0;
            cmdInsCons.Parameters.Add("@NUM_VALOR_FIXO_MARKUP_7X", SqlDbType.Float).Value = (Funcoes.IsDoubleRealNumber(((TextBox)itemP.FindControl("txtmarkup7xfixo")).Text.ToString())) ? Funcoes.strToDouble(((TextBox)itemP.FindControl("txtmarkup7xfixo")).Text.ToString()) : 0;
            cmdInsCons.Parameters.Add("@NUM_VALOR_FIXO_MARKUP_8X", SqlDbType.Float).Value = (Funcoes.IsDoubleRealNumber(((TextBox)itemP.FindControl("txtmarkup8xfixo")).Text.ToString())) ? Funcoes.strToDouble(((TextBox)itemP.FindControl("txtmarkup8xfixo")).Text.ToString()) : 0;
            cmdInsCons.Parameters.Add("@NUM_VALOR_FIXO_MARKUP_9X", SqlDbType.Float).Value = (Funcoes.IsDoubleRealNumber(((TextBox)itemP.FindControl("txtmarkup9xfixo")).Text.ToString())) ? Funcoes.strToDouble(((TextBox)itemP.FindControl("txtmarkup9xfixo")).Text.ToString()) : 0;
            cmdInsCons.Parameters.Add("@NUM_VALOR_FIXO_MARKUP_10X", SqlDbType.Float).Value = (Funcoes.IsDoubleRealNumber(((TextBox)itemP.FindControl("txtmarkup10xfixo")).Text.ToString())) ? Funcoes.strToDouble(((TextBox)itemP.FindControl("txtmarkup10xfixo")).Text.ToString()) : 0;
            cmdInsCons.Parameters.Add("@NUM_VALOR_FIXO_MARKUP_11X", SqlDbType.Float).Value = (Funcoes.IsDoubleRealNumber(((TextBox)itemP.FindControl("txtmarkup11xfixo")).Text.ToString())) ? Funcoes.strToDouble(((TextBox)itemP.FindControl("txtmarkup11xfixo")).Text.ToString()) : 0;
            cmdInsCons.Parameters.Add("@NUM_VALOR_FIXO_MARKUP_12X", SqlDbType.Float).Value = (Funcoes.IsDoubleRealNumber(((TextBox)itemP.FindControl("txtmarkup12xfixo")).Text.ToString())) ? Funcoes.strToDouble(((TextBox)itemP.FindControl("txtmarkup12xfixo")).Text.ToString()) : 0;
            cmdInsCons.Parameters.Add("@NUM_VALOR_FIXO_MARKUP_13X", SqlDbType.Float).Value = (Funcoes.IsDoubleRealNumber(((TextBox)itemP.FindControl("txtmarkup13xfixo")).Text.ToString())) ? Funcoes.strToDouble(((TextBox)itemP.FindControl("txtmarkup13xfixo")).Text.ToString()) : 0;
            cmdInsCons.Parameters.Add("@NUM_VALOR_FIXO_MARKUP_14X", SqlDbType.Float).Value = (Funcoes.IsDoubleRealNumber(((TextBox)itemP.FindControl("txtmarkup14xfixo")).Text.ToString())) ? Funcoes.strToDouble(((TextBox)itemP.FindControl("txtmarkup14xfixo")).Text.ToString()) : 0;
            cmdInsCons.Parameters.Add("@NUM_VALOR_FIXO_MARKUP_15X", SqlDbType.Float).Value = (Funcoes.IsDoubleRealNumber(((TextBox)itemP.FindControl("txtmarkup15xfixo")).Text.ToString())) ? Funcoes.strToDouble(((TextBox)itemP.FindControl("txtmarkup15xfixo")).Text.ToString()) : 0;
            cmdInsCons.Parameters.Add("@NUM_VALOR_FIXO_MARKUP_16X", SqlDbType.Float).Value = (Funcoes.IsDoubleRealNumber(((TextBox)itemP.FindControl("txtmarkup16xfixo")).Text.ToString())) ? Funcoes.strToDouble(((TextBox)itemP.FindControl("txtmarkup16xfixo")).Text.ToString()) : 0;
            cmdInsCons.Parameters.Add("@NUM_VALOR_FIXO_MARKUP_17X", SqlDbType.Float).Value = (Funcoes.IsDoubleRealNumber(((TextBox)itemP.FindControl("txtmarkup17xfixo")).Text.ToString())) ? Funcoes.strToDouble(((TextBox)itemP.FindControl("txtmarkup17xfixo")).Text.ToString()) : 0;
            cmdInsCons.Parameters.Add("@NUM_VALOR_FIXO_MARKUP_18X", SqlDbType.Float).Value = (Funcoes.IsDoubleRealNumber(((TextBox)itemP.FindControl("txtmarkup18xfixo")).Text.ToString())) ? Funcoes.strToDouble(((TextBox)itemP.FindControl("txtmarkup18xfixo")).Text.ToString()) : 0;

            cmdInsCons.Parameters.Add("@NUM_VALOR_FIXO_MARKUP_19X", SqlDbType.Float).Value = (Funcoes.IsDoubleRealNumber(((TextBox)itemP.FindControl("txtmarkup19xfixo")).Text.ToString())) ? Funcoes.strToDouble(((TextBox)itemP.FindControl("txtmarkup19xfixo")).Text.ToString()) : 0;
            cmdInsCons.Parameters.Add("@NUM_VALOR_FIXO_MARKUP_20X", SqlDbType.Float).Value = (Funcoes.IsDoubleRealNumber(((TextBox)itemP.FindControl("txtmarkup20xfixo")).Text.ToString())) ? Funcoes.strToDouble(((TextBox)itemP.FindControl("txtmarkup20xfixo")).Text.ToString()) : 0;
            cmdInsCons.Parameters.Add("@NUM_VALOR_FIXO_MARKUP_21X", SqlDbType.Float).Value = (Funcoes.IsDoubleRealNumber(((TextBox)itemP.FindControl("txtmarkup21xfixo")).Text.ToString())) ? Funcoes.strToDouble(((TextBox)itemP.FindControl("txtmarkup21xfixo")).Text.ToString()) : 0;


            cmdInsCons.ExecuteNonQuery();
            connInsCons.Close();
            connInsCons.Dispose();
        }

    }

    private void Gravar()
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
            cmdInsCons.Parameters.Add("@NUM_REBATE", SqlDbType.Float).Value = Funcoes.strToDouble(((TextBox)itemP.FindControl("txtRebate")).Text.ToString());
            cmdInsCons.Parameters.Add("@FLG_PRESENCIAL_ONLINE", SqlDbType.Char).Value = "P";
            cmdInsCons.ExecuteNonQuery();
            connInsCons.Close();
            connInsCons.Dispose();
        }


    }

    private void GravarOnline()
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
            cmdInsCons.Parameters.Add("@NUM_REBATE", SqlDbType.Float).Value = Funcoes.strToDouble(((TextBox)itemP.FindControl("txtRebate")).Text.ToString());
            cmdInsCons.Parameters.Add("@FLG_PRESENCIAL_ONLINE", SqlDbType.Char).Value = "O";
            cmdInsCons.ExecuteNonQuery();
            connInsCons.Close();
            connInsCons.Dispose();
        }


    }

    private void ImportarTaxas()
    {
        /*
        if (ddlAntecipado.SelectedValue.ToString().Trim() == "S")
        {
         */

            // Taxas em Percentual
            // On-Line
            foreach (RepeaterItem itemPO in rptConsultaParcelasOnline.Items)
            {
                // Percentual
                if (ValorPercentualPresencialAntecipado(Funcoes.strToInt(ddlPlanosReferencia.SelectedValue.ToString()), Funcoes.strToInt(((TextBox)itemPO.FindControl("txtidbandeira")).Text.ToString()), "V", "O", 1, 0) > 0)
                {
                    ((TextBox)itemPO.FindControl("txtDebito")).Text = String.Format("{0:n2}", ValorPercentualPresencialAntecipado(Funcoes.strToInt(ddlPlanosReferencia.SelectedValue.ToString()), Funcoes.strToInt(((TextBox)itemPO.FindControl("txtidbandeira")).Text.ToString()), "V", "O", 1, 0));
                }
                else
                {
                    ((TextBox)itemPO.FindControl("txtDebito")).Text = String.Format("{0:n2}", ValorPercentualPresencialAntecipado(Funcoes.strToInt(ddlPlanosReferencia.SelectedValue.ToString()), Funcoes.strToInt(((TextBox)itemPO.FindControl("txtidbandeira")).Text.ToString()), "C", "O", 1, 0));
                }
                ((TextBox)itemPO.FindControl("txtcredito")).Text = String.Format("{0:n2}", ValorPercentualPresencialAntecipado(Funcoes.strToInt(ddlPlanosReferencia.SelectedValue.ToString()), Funcoes.strToInt(((TextBox)itemPO.FindControl("txtidbandeira")).Text.ToString()), "C", "O", 1, 1));
                ((TextBox)itemPO.FindControl("txtcredito2x")).Text = String.Format("{0:n2}", ValorPercentualPresencialAntecipado(Funcoes.strToInt(ddlPlanosReferencia.SelectedValue.ToString()), Funcoes.strToInt(((TextBox)itemPO.FindControl("txtidbandeira")).Text.ToString()), "C", "O", 2, 1));
                ((TextBox)itemPO.FindControl("txtcredito3x")).Text = String.Format("{0:n2}", ValorPercentualPresencialAntecipado(Funcoes.strToInt(ddlPlanosReferencia.SelectedValue.ToString()), Funcoes.strToInt(((TextBox)itemPO.FindControl("txtidbandeira")).Text.ToString()), "C", "O", 3, 1));
                ((TextBox)itemPO.FindControl("txtcredito4x")).Text = String.Format("{0:n2}", ValorPercentualPresencialAntecipado(Funcoes.strToInt(ddlPlanosReferencia.SelectedValue.ToString()), Funcoes.strToInt(((TextBox)itemPO.FindControl("txtidbandeira")).Text.ToString()), "C", "O", 4, 1));
                ((TextBox)itemPO.FindControl("txtcredito5x")).Text = String.Format("{0:n2}", ValorPercentualPresencialAntecipado(Funcoes.strToInt(ddlPlanosReferencia.SelectedValue.ToString()), Funcoes.strToInt(((TextBox)itemPO.FindControl("txtidbandeira")).Text.ToString()), "C", "O", 5, 1));
                ((TextBox)itemPO.FindControl("txtcredito6x")).Text = String.Format("{0:n2}", ValorPercentualPresencialAntecipado(Funcoes.strToInt(ddlPlanosReferencia.SelectedValue.ToString()), Funcoes.strToInt(((TextBox)itemPO.FindControl("txtidbandeira")).Text.ToString()), "C", "O", 6, 1));
                ((TextBox)itemPO.FindControl("txtcredito7x")).Text = String.Format("{0:n2}", ValorPercentualPresencialAntecipado(Funcoes.strToInt(ddlPlanosReferencia.SelectedValue.ToString()), Funcoes.strToInt(((TextBox)itemPO.FindControl("txtidbandeira")).Text.ToString()), "C", "O", 7, 1));
                ((TextBox)itemPO.FindControl("txtcredito8x")).Text = String.Format("{0:n2}", ValorPercentualPresencialAntecipado(Funcoes.strToInt(ddlPlanosReferencia.SelectedValue.ToString()), Funcoes.strToInt(((TextBox)itemPO.FindControl("txtidbandeira")).Text.ToString()), "C", "O", 8, 1));
                ((TextBox)itemPO.FindControl("txtcredito9x")).Text = String.Format("{0:n2}", ValorPercentualPresencialAntecipado(Funcoes.strToInt(ddlPlanosReferencia.SelectedValue.ToString()), Funcoes.strToInt(((TextBox)itemPO.FindControl("txtidbandeira")).Text.ToString()), "C", "O", 9, 1));
                ((TextBox)itemPO.FindControl("txtcredito10x")).Text = String.Format("{0:n2}", ValorPercentualPresencialAntecipado(Funcoes.strToInt(ddlPlanosReferencia.SelectedValue.ToString()), Funcoes.strToInt(((TextBox)itemPO.FindControl("txtidbandeira")).Text.ToString()), "C", "O", 10, 1));
                ((TextBox)itemPO.FindControl("txtcredito11x")).Text = String.Format("{0:n2}", ValorPercentualPresencialAntecipado(Funcoes.strToInt(ddlPlanosReferencia.SelectedValue.ToString()), Funcoes.strToInt(((TextBox)itemPO.FindControl("txtidbandeira")).Text.ToString()), "C", "O", 11, 1));
                ((TextBox)itemPO.FindControl("txtcredito12x")).Text = String.Format("{0:n2}", ValorPercentualPresencialAntecipado(Funcoes.strToInt(ddlPlanosReferencia.SelectedValue.ToString()), Funcoes.strToInt(((TextBox)itemPO.FindControl("txtidbandeira")).Text.ToString()), "C", "O", 12, 1));
                ((TextBox)itemPO.FindControl("txtcredito13x")).Text = String.Format("{0:n2}", ValorPercentualPresencialAntecipado(Funcoes.strToInt(ddlPlanosReferencia.SelectedValue.ToString()), Funcoes.strToInt(((TextBox)itemPO.FindControl("txtidbandeira")).Text.ToString()), "C", "O", 13, 1));
                ((TextBox)itemPO.FindControl("txtcredito14x")).Text = String.Format("{0:n2}", ValorPercentualPresencialAntecipado(Funcoes.strToInt(ddlPlanosReferencia.SelectedValue.ToString()), Funcoes.strToInt(((TextBox)itemPO.FindControl("txtidbandeira")).Text.ToString()), "C", "O", 14, 1));
                ((TextBox)itemPO.FindControl("txtcredito15x")).Text = String.Format("{0:n2}", ValorPercentualPresencialAntecipado(Funcoes.strToInt(ddlPlanosReferencia.SelectedValue.ToString()), Funcoes.strToInt(((TextBox)itemPO.FindControl("txtidbandeira")).Text.ToString()), "C", "O", 15, 1));
                ((TextBox)itemPO.FindControl("txtcredito16x")).Text = String.Format("{0:n2}", ValorPercentualPresencialAntecipado(Funcoes.strToInt(ddlPlanosReferencia.SelectedValue.ToString()), Funcoes.strToInt(((TextBox)itemPO.FindControl("txtidbandeira")).Text.ToString()), "C", "O", 16, 1));
                ((TextBox)itemPO.FindControl("txtcredito17x")).Text = String.Format("{0:n2}", ValorPercentualPresencialAntecipado(Funcoes.strToInt(ddlPlanosReferencia.SelectedValue.ToString()), Funcoes.strToInt(((TextBox)itemPO.FindControl("txtidbandeira")).Text.ToString()), "C", "O", 17, 1));
                ((TextBox)itemPO.FindControl("txtcredito18x")).Text = String.Format("{0:n2}", ValorPercentualPresencialAntecipado(Funcoes.strToInt(ddlPlanosReferencia.SelectedValue.ToString()), Funcoes.strToInt(((TextBox)itemPO.FindControl("txtidbandeira")).Text.ToString()), "C", "O", 18, 1));
                ((TextBox)itemPO.FindControl("txtcredito19x")).Text = String.Format("{0:n2}", ValorPercentualPresencialAntecipado(Funcoes.strToInt(ddlPlanosReferencia.SelectedValue.ToString()), Funcoes.strToInt(((TextBox)itemPO.FindControl("txtidbandeira")).Text.ToString()), "C", "O", 19, 1));
                ((TextBox)itemPO.FindControl("txtcredito20x")).Text = String.Format("{0:n2}", ValorPercentualPresencialAntecipado(Funcoes.strToInt(ddlPlanosReferencia.SelectedValue.ToString()), Funcoes.strToInt(((TextBox)itemPO.FindControl("txtidbandeira")).Text.ToString()), "C", "O", 20, 1));
                ((TextBox)itemPO.FindControl("txtcredito21x")).Text = String.Format("{0:n2}", ValorPercentualPresencialAntecipado(Funcoes.strToInt(ddlPlanosReferencia.SelectedValue.ToString()), Funcoes.strToInt(((TextBox)itemPO.FindControl("txtidbandeira")).Text.ToString()), "C", "O", 21, 1));
                
                
                // Valor
                if (ValorFixoPresencialAntecipado(Funcoes.strToInt(ddlPlanosReferencia.SelectedValue.ToString()), Funcoes.strToInt(((TextBox)itemPO.FindControl("txtidbandeira")).Text.ToString()), "V", "O", 1, 0) > 0)
                {
                    ((TextBox)itemPO.FindControl("txtDebitofixo")).Text = String.Format("{0:n2}", ValorFixoPresencialAntecipado(Funcoes.strToInt(ddlPlanosReferencia.SelectedValue.ToString()), Funcoes.strToInt(((TextBox)itemPO.FindControl("txtidbandeira")).Text.ToString()), "V", "O", 1, 0));
                }
                else
                {
                    ((TextBox)itemPO.FindControl("txtDebitofixo")).Text = String.Format("{0:n2}", ValorFixoPresencialAntecipado(Funcoes.strToInt(ddlPlanosReferencia.SelectedValue.ToString()), Funcoes.strToInt(((TextBox)itemPO.FindControl("txtidbandeira")).Text.ToString()), "C", "O", 1, 0));
                }
                ((TextBox)itemPO.FindControl("txtcreditofixo")).Text = String.Format("{0:n2}", ValorFixoPresencialAntecipado(Funcoes.strToInt(ddlPlanosReferencia.SelectedValue.ToString()), Funcoes.strToInt(((TextBox)itemPO.FindControl("txtidbandeira")).Text.ToString()), "C", "O", 1, 1));
                ((TextBox)itemPO.FindControl("txtcredito2xfixo")).Text = String.Format("{0:n2}", ValorFixoPresencialAntecipado(Funcoes.strToInt(ddlPlanosReferencia.SelectedValue.ToString()), Funcoes.strToInt(((TextBox)itemPO.FindControl("txtidbandeira")).Text.ToString()), "C", "O", 2, 1));
                ((TextBox)itemPO.FindControl("txtcredito3xfixo")).Text = String.Format("{0:n2}", ValorFixoPresencialAntecipado(Funcoes.strToInt(ddlPlanosReferencia.SelectedValue.ToString()), Funcoes.strToInt(((TextBox)itemPO.FindControl("txtidbandeira")).Text.ToString()), "C", "O", 3, 1));
                ((TextBox)itemPO.FindControl("txtcredito4xfixo")).Text = String.Format("{0:n2}", ValorFixoPresencialAntecipado(Funcoes.strToInt(ddlPlanosReferencia.SelectedValue.ToString()), Funcoes.strToInt(((TextBox)itemPO.FindControl("txtidbandeira")).Text.ToString()), "C", "O", 4, 1));
                ((TextBox)itemPO.FindControl("txtcredito5xfixo")).Text = String.Format("{0:n2}", ValorFixoPresencialAntecipado(Funcoes.strToInt(ddlPlanosReferencia.SelectedValue.ToString()), Funcoes.strToInt(((TextBox)itemPO.FindControl("txtidbandeira")).Text.ToString()), "C", "O", 5, 1));
                ((TextBox)itemPO.FindControl("txtcredito6xfixo")).Text = String.Format("{0:n2}", ValorFixoPresencialAntecipado(Funcoes.strToInt(ddlPlanosReferencia.SelectedValue.ToString()), Funcoes.strToInt(((TextBox)itemPO.FindControl("txtidbandeira")).Text.ToString()), "C", "O", 6, 1));
                ((TextBox)itemPO.FindControl("txtcredito7xfixo")).Text = String.Format("{0:n2}", ValorFixoPresencialAntecipado(Funcoes.strToInt(ddlPlanosReferencia.SelectedValue.ToString()), Funcoes.strToInt(((TextBox)itemPO.FindControl("txtidbandeira")).Text.ToString()), "C", "O", 7, 1));
                ((TextBox)itemPO.FindControl("txtcredito8xfixo")).Text = String.Format("{0:n2}", ValorFixoPresencialAntecipado(Funcoes.strToInt(ddlPlanosReferencia.SelectedValue.ToString()), Funcoes.strToInt(((TextBox)itemPO.FindControl("txtidbandeira")).Text.ToString()), "C", "O", 8, 1));
                ((TextBox)itemPO.FindControl("txtcredito9xfixo")).Text = String.Format("{0:n2}", ValorFixoPresencialAntecipado(Funcoes.strToInt(ddlPlanosReferencia.SelectedValue.ToString()), Funcoes.strToInt(((TextBox)itemPO.FindControl("txtidbandeira")).Text.ToString()), "C", "O", 9, 1));
                ((TextBox)itemPO.FindControl("txtcredito10xfixo")).Text = String.Format("{0:n2}", ValorFixoPresencialAntecipado(Funcoes.strToInt(ddlPlanosReferencia.SelectedValue.ToString()), Funcoes.strToInt(((TextBox)itemPO.FindControl("txtidbandeira")).Text.ToString()), "C", "O", 10, 1));
                ((TextBox)itemPO.FindControl("txtcredito11xfixo")).Text = String.Format("{0:n2}", ValorFixoPresencialAntecipado(Funcoes.strToInt(ddlPlanosReferencia.SelectedValue.ToString()), Funcoes.strToInt(((TextBox)itemPO.FindControl("txtidbandeira")).Text.ToString()), "C", "O", 11, 1));
                ((TextBox)itemPO.FindControl("txtcredito12xfixo")).Text = String.Format("{0:n2}", ValorFixoPresencialAntecipado(Funcoes.strToInt(ddlPlanosReferencia.SelectedValue.ToString()), Funcoes.strToInt(((TextBox)itemPO.FindControl("txtidbandeira")).Text.ToString()), "C", "O", 12, 1));
                ((TextBox)itemPO.FindControl("txtcredito13xfixo")).Text = String.Format("{0:n2}", ValorFixoPresencialAntecipado(Funcoes.strToInt(ddlPlanosReferencia.SelectedValue.ToString()), Funcoes.strToInt(((TextBox)itemPO.FindControl("txtidbandeira")).Text.ToString()), "C", "O", 13, 1));
                ((TextBox)itemPO.FindControl("txtcredito14xfixo")).Text = String.Format("{0:n2}", ValorFixoPresencialAntecipado(Funcoes.strToInt(ddlPlanosReferencia.SelectedValue.ToString()), Funcoes.strToInt(((TextBox)itemPO.FindControl("txtidbandeira")).Text.ToString()), "C", "O", 14, 1));
                ((TextBox)itemPO.FindControl("txtcredito15xfixo")).Text = String.Format("{0:n2}", ValorFixoPresencialAntecipado(Funcoes.strToInt(ddlPlanosReferencia.SelectedValue.ToString()), Funcoes.strToInt(((TextBox)itemPO.FindControl("txtidbandeira")).Text.ToString()), "C", "O", 15, 1));
                ((TextBox)itemPO.FindControl("txtcredito16xfixo")).Text = String.Format("{0:n2}", ValorFixoPresencialAntecipado(Funcoes.strToInt(ddlPlanosReferencia.SelectedValue.ToString()), Funcoes.strToInt(((TextBox)itemPO.FindControl("txtidbandeira")).Text.ToString()), "C", "O", 16, 1));
                ((TextBox)itemPO.FindControl("txtcredito17xfixo")).Text = String.Format("{0:n2}", ValorFixoPresencialAntecipado(Funcoes.strToInt(ddlPlanosReferencia.SelectedValue.ToString()), Funcoes.strToInt(((TextBox)itemPO.FindControl("txtidbandeira")).Text.ToString()), "C", "O", 17, 1));
                ((TextBox)itemPO.FindControl("txtcredito18xfixo")).Text = String.Format("{0:n2}", ValorFixoPresencialAntecipado(Funcoes.strToInt(ddlPlanosReferencia.SelectedValue.ToString()), Funcoes.strToInt(((TextBox)itemPO.FindControl("txtidbandeira")).Text.ToString()), "C", "O", 18, 1));
                ((TextBox)itemPO.FindControl("txtcredito19xfixo")).Text = String.Format("{0:n2}", ValorFixoPresencialAntecipado(Funcoes.strToInt(ddlPlanosReferencia.SelectedValue.ToString()), Funcoes.strToInt(((TextBox)itemPO.FindControl("txtidbandeira")).Text.ToString()), "C", "O", 19, 1));
                ((TextBox)itemPO.FindControl("txtcredito20xfixo")).Text = String.Format("{0:n2}", ValorFixoPresencialAntecipado(Funcoes.strToInt(ddlPlanosReferencia.SelectedValue.ToString()), Funcoes.strToInt(((TextBox)itemPO.FindControl("txtidbandeira")).Text.ToString()), "C", "O", 20, 1));
                ((TextBox)itemPO.FindControl("txtcredito21xfixo")).Text = String.Format("{0:n2}", ValorFixoPresencialAntecipado(Funcoes.strToInt(ddlPlanosReferencia.SelectedValue.ToString()), Funcoes.strToInt(((TextBox)itemPO.FindControl("txtidbandeira")).Text.ToString()), "C", "O", 21, 1));


            }

            // Presencial

            foreach (RepeaterItem itemPP in rptConsultaParcelas.Items)
            {
                // Percentual
                
                if (ValorPercentualPresencialAntecipado(Funcoes.strToInt(ddlPlanosReferencia.SelectedValue.ToString()), Funcoes.strToInt(((TextBox)itemPP.FindControl("txtidbandeira")).Text.ToString()), "V", "P", 1, 0) > 0)
                {
                    ((TextBox)itemPP.FindControl("txtDebito")).Text = String.Format("{0:n2}", ValorPercentualPresencialAntecipado(Funcoes.strToInt(ddlPlanosReferencia.SelectedValue.ToString()), Funcoes.strToInt(((TextBox)itemPP.FindControl("txtidbandeira")).Text.ToString()), "V", "P", 1, 0));
                }
                else
                {
                    ((TextBox)itemPP.FindControl("txtDebito")).Text = String.Format("{0:n2}", ValorPercentualPresencialAntecipado(Funcoes.strToInt(ddlPlanosReferencia.SelectedValue.ToString()), Funcoes.strToInt(((TextBox)itemPP.FindControl("txtidbandeira")).Text.ToString()), "C", "P", 1, 0));
                }
                ((TextBox)itemPP.FindControl("txtcredito")).Text = String.Format("{0:n2}", ValorPercentualPresencialAntecipado(Funcoes.strToInt(ddlPlanosReferencia.SelectedValue.ToString()), Funcoes.strToInt(((TextBox)itemPP.FindControl("txtidbandeira")).Text.ToString()), "C", "P", 1, 1));
                ((TextBox)itemPP.FindControl("txtcredito2x")).Text = String.Format("{0:n2}", ValorPercentualPresencialAntecipado(Funcoes.strToInt(ddlPlanosReferencia.SelectedValue.ToString()), Funcoes.strToInt(((TextBox)itemPP.FindControl("txtidbandeira")).Text.ToString()), "C", "P", 2, 1));
                ((TextBox)itemPP.FindControl("txtcredito3x")).Text = String.Format("{0:n2}", ValorPercentualPresencialAntecipado(Funcoes.strToInt(ddlPlanosReferencia.SelectedValue.ToString()), Funcoes.strToInt(((TextBox)itemPP.FindControl("txtidbandeira")).Text.ToString()), "C", "P", 3, 1));
                ((TextBox)itemPP.FindControl("txtcredito4x")).Text = String.Format("{0:n2}", ValorPercentualPresencialAntecipado(Funcoes.strToInt(ddlPlanosReferencia.SelectedValue.ToString()), Funcoes.strToInt(((TextBox)itemPP.FindControl("txtidbandeira")).Text.ToString()), "C", "P", 4, 1));
                ((TextBox)itemPP.FindControl("txtcredito5x")).Text = String.Format("{0:n2}", ValorPercentualPresencialAntecipado(Funcoes.strToInt(ddlPlanosReferencia.SelectedValue.ToString()), Funcoes.strToInt(((TextBox)itemPP.FindControl("txtidbandeira")).Text.ToString()), "C", "P", 5, 1));
                ((TextBox)itemPP.FindControl("txtcredito6x")).Text = String.Format("{0:n2}", ValorPercentualPresencialAntecipado(Funcoes.strToInt(ddlPlanosReferencia.SelectedValue.ToString()), Funcoes.strToInt(((TextBox)itemPP.FindControl("txtidbandeira")).Text.ToString()), "C", "P", 6, 1));
                ((TextBox)itemPP.FindControl("txtcredito7x")).Text = String.Format("{0:n2}", ValorPercentualPresencialAntecipado(Funcoes.strToInt(ddlPlanosReferencia.SelectedValue.ToString()), Funcoes.strToInt(((TextBox)itemPP.FindControl("txtidbandeira")).Text.ToString()), "C", "P", 7, 1));
                ((TextBox)itemPP.FindControl("txtcredito8x")).Text = String.Format("{0:n2}", ValorPercentualPresencialAntecipado(Funcoes.strToInt(ddlPlanosReferencia.SelectedValue.ToString()), Funcoes.strToInt(((TextBox)itemPP.FindControl("txtidbandeira")).Text.ToString()), "C", "P", 8, 1));
                ((TextBox)itemPP.FindControl("txtcredito9x")).Text = String.Format("{0:n2}", ValorPercentualPresencialAntecipado(Funcoes.strToInt(ddlPlanosReferencia.SelectedValue.ToString()), Funcoes.strToInt(((TextBox)itemPP.FindControl("txtidbandeira")).Text.ToString()), "C", "P", 9, 1));
                ((TextBox)itemPP.FindControl("txtcredito10x")).Text = String.Format("{0:n2}", ValorPercentualPresencialAntecipado(Funcoes.strToInt(ddlPlanosReferencia.SelectedValue.ToString()), Funcoes.strToInt(((TextBox)itemPP.FindControl("txtidbandeira")).Text.ToString()), "C", "P", 10, 1));
                ((TextBox)itemPP.FindControl("txtcredito11x")).Text = String.Format("{0:n2}", ValorPercentualPresencialAntecipado(Funcoes.strToInt(ddlPlanosReferencia.SelectedValue.ToString()), Funcoes.strToInt(((TextBox)itemPP.FindControl("txtidbandeira")).Text.ToString()), "C", "P", 11, 1));
                ((TextBox)itemPP.FindControl("txtcredito12x")).Text = String.Format("{0:n2}", ValorPercentualPresencialAntecipado(Funcoes.strToInt(ddlPlanosReferencia.SelectedValue.ToString()), Funcoes.strToInt(((TextBox)itemPP.FindControl("txtidbandeira")).Text.ToString()), "C", "P", 12, 1));
                ((TextBox)itemPP.FindControl("txtcredito13x")).Text = String.Format("{0:n2}", ValorPercentualPresencialAntecipado(Funcoes.strToInt(ddlPlanosReferencia.SelectedValue.ToString()), Funcoes.strToInt(((TextBox)itemPP.FindControl("txtidbandeira")).Text.ToString()), "C", "P", 13, 1));
                ((TextBox)itemPP.FindControl("txtcredito14x")).Text = String.Format("{0:n2}", ValorPercentualPresencialAntecipado(Funcoes.strToInt(ddlPlanosReferencia.SelectedValue.ToString()), Funcoes.strToInt(((TextBox)itemPP.FindControl("txtidbandeira")).Text.ToString()), "C", "P", 14, 1));
                ((TextBox)itemPP.FindControl("txtcredito15x")).Text = String.Format("{0:n2}", ValorPercentualPresencialAntecipado(Funcoes.strToInt(ddlPlanosReferencia.SelectedValue.ToString()), Funcoes.strToInt(((TextBox)itemPP.FindControl("txtidbandeira")).Text.ToString()), "C", "P", 15, 1));
                ((TextBox)itemPP.FindControl("txtcredito16x")).Text = String.Format("{0:n2}", ValorPercentualPresencialAntecipado(Funcoes.strToInt(ddlPlanosReferencia.SelectedValue.ToString()), Funcoes.strToInt(((TextBox)itemPP.FindControl("txtidbandeira")).Text.ToString()), "C", "P", 16, 1));
                ((TextBox)itemPP.FindControl("txtcredito17x")).Text = String.Format("{0:n2}", ValorPercentualPresencialAntecipado(Funcoes.strToInt(ddlPlanosReferencia.SelectedValue.ToString()), Funcoes.strToInt(((TextBox)itemPP.FindControl("txtidbandeira")).Text.ToString()), "C", "P", 17, 1));
                ((TextBox)itemPP.FindControl("txtcredito18x")).Text = String.Format("{0:n2}", ValorPercentualPresencialAntecipado(Funcoes.strToInt(ddlPlanosReferencia.SelectedValue.ToString()), Funcoes.strToInt(((TextBox)itemPP.FindControl("txtidbandeira")).Text.ToString()), "C", "P", 18, 1));
                ((TextBox)itemPP.FindControl("txtcredito19x")).Text = String.Format("{0:n2}", ValorPercentualPresencialAntecipado(Funcoes.strToInt(ddlPlanosReferencia.SelectedValue.ToString()), Funcoes.strToInt(((TextBox)itemPP.FindControl("txtidbandeira")).Text.ToString()), "C", "P", 19, 1));
                ((TextBox)itemPP.FindControl("txtcredito20x")).Text = String.Format("{0:n2}", ValorPercentualPresencialAntecipado(Funcoes.strToInt(ddlPlanosReferencia.SelectedValue.ToString()), Funcoes.strToInt(((TextBox)itemPP.FindControl("txtidbandeira")).Text.ToString()), "C", "P", 20, 1));
                ((TextBox)itemPP.FindControl("txtcredito21x")).Text = String.Format("{0:n2}", ValorPercentualPresencialAntecipado(Funcoes.strToInt(ddlPlanosReferencia.SelectedValue.ToString()), Funcoes.strToInt(((TextBox)itemPP.FindControl("txtidbandeira")).Text.ToString()), "C", "P", 21, 1));

                // Valor
                if (ValorFixoPresencialAntecipado(Funcoes.strToInt(ddlPlanosReferencia.SelectedValue.ToString()), Funcoes.strToInt(((TextBox)itemPP.FindControl("txtidbandeira")).Text.ToString()), "V", "P", 1, 0) > 0)
                {
                    ((TextBox)itemPP.FindControl("txtDebitofixo")).Text = String.Format("{0:n2}", ValorFixoPresencialAntecipado(Funcoes.strToInt(ddlPlanosReferencia.SelectedValue.ToString()), Funcoes.strToInt(((TextBox)itemPP.FindControl("txtidbandeira")).Text.ToString()), "V", "P", 1, 0));
                }
                else
                {
                    ((TextBox)itemPP.FindControl("txtDebitofixo")).Text = String.Format("{0:n2}", ValorFixoPresencialAntecipado(Funcoes.strToInt(ddlPlanosReferencia.SelectedValue.ToString()), Funcoes.strToInt(((TextBox)itemPP.FindControl("txtidbandeira")).Text.ToString()), "C", "P", 1, 0));
                }
                ((TextBox)itemPP.FindControl("txtcreditofixo")).Text = String.Format("{0:n2}", ValorFixoPresencialAntecipado(Funcoes.strToInt(ddlPlanosReferencia.SelectedValue.ToString()), Funcoes.strToInt(((TextBox)itemPP.FindControl("txtidbandeira")).Text.ToString()), "C", "P", 1, 1));
                ((TextBox)itemPP.FindControl("txtcredito2xfixo")).Text = String.Format("{0:n2}", ValorFixoPresencialAntecipado(Funcoes.strToInt(ddlPlanosReferencia.SelectedValue.ToString()), Funcoes.strToInt(((TextBox)itemPP.FindControl("txtidbandeira")).Text.ToString()), "C", "P", 2, 1));
                ((TextBox)itemPP.FindControl("txtcredito3xfixo")).Text = String.Format("{0:n2}", ValorFixoPresencialAntecipado(Funcoes.strToInt(ddlPlanosReferencia.SelectedValue.ToString()), Funcoes.strToInt(((TextBox)itemPP.FindControl("txtidbandeira")).Text.ToString()), "C", "P", 3, 1));
                ((TextBox)itemPP.FindControl("txtcredito4xfixo")).Text = String.Format("{0:n2}", ValorFixoPresencialAntecipado(Funcoes.strToInt(ddlPlanosReferencia.SelectedValue.ToString()), Funcoes.strToInt(((TextBox)itemPP.FindControl("txtidbandeira")).Text.ToString()), "C", "P", 4, 1));
                ((TextBox)itemPP.FindControl("txtcredito5xfixo")).Text = String.Format("{0:n2}", ValorFixoPresencialAntecipado(Funcoes.strToInt(ddlPlanosReferencia.SelectedValue.ToString()), Funcoes.strToInt(((TextBox)itemPP.FindControl("txtidbandeira")).Text.ToString()), "C", "P", 5, 1));
                ((TextBox)itemPP.FindControl("txtcredito6xfixo")).Text = String.Format("{0:n2}", ValorFixoPresencialAntecipado(Funcoes.strToInt(ddlPlanosReferencia.SelectedValue.ToString()), Funcoes.strToInt(((TextBox)itemPP.FindControl("txtidbandeira")).Text.ToString()), "C", "P", 6, 1));
                ((TextBox)itemPP.FindControl("txtcredito7xfixo")).Text = String.Format("{0:n2}", ValorFixoPresencialAntecipado(Funcoes.strToInt(ddlPlanosReferencia.SelectedValue.ToString()), Funcoes.strToInt(((TextBox)itemPP.FindControl("txtidbandeira")).Text.ToString()), "C", "P", 7, 1));
                ((TextBox)itemPP.FindControl("txtcredito8xfixo")).Text = String.Format("{0:n2}", ValorFixoPresencialAntecipado(Funcoes.strToInt(ddlPlanosReferencia.SelectedValue.ToString()), Funcoes.strToInt(((TextBox)itemPP.FindControl("txtidbandeira")).Text.ToString()), "C", "P", 8, 1));
                ((TextBox)itemPP.FindControl("txtcredito9xfixo")).Text = String.Format("{0:n2}", ValorFixoPresencialAntecipado(Funcoes.strToInt(ddlPlanosReferencia.SelectedValue.ToString()), Funcoes.strToInt(((TextBox)itemPP.FindControl("txtidbandeira")).Text.ToString()), "C", "P", 9, 1));
                ((TextBox)itemPP.FindControl("txtcredito10xfixo")).Text = String.Format("{0:n2}", ValorFixoPresencialAntecipado(Funcoes.strToInt(ddlPlanosReferencia.SelectedValue.ToString()), Funcoes.strToInt(((TextBox)itemPP.FindControl("txtidbandeira")).Text.ToString()), "C", "P", 10, 1));
                ((TextBox)itemPP.FindControl("txtcredito11xfixo")).Text = String.Format("{0:n2}", ValorFixoPresencialAntecipado(Funcoes.strToInt(ddlPlanosReferencia.SelectedValue.ToString()), Funcoes.strToInt(((TextBox)itemPP.FindControl("txtidbandeira")).Text.ToString()), "C", "P", 11, 1));
                ((TextBox)itemPP.FindControl("txtcredito12xfixo")).Text = String.Format("{0:n2}", ValorFixoPresencialAntecipado(Funcoes.strToInt(ddlPlanosReferencia.SelectedValue.ToString()), Funcoes.strToInt(((TextBox)itemPP.FindControl("txtidbandeira")).Text.ToString()), "C", "P", 12, 1));
                ((TextBox)itemPP.FindControl("txtcredito13xfixo")).Text = String.Format("{0:n2}", ValorFixoPresencialAntecipado(Funcoes.strToInt(ddlPlanosReferencia.SelectedValue.ToString()), Funcoes.strToInt(((TextBox)itemPP.FindControl("txtidbandeira")).Text.ToString()), "C", "P", 13, 1));
                ((TextBox)itemPP.FindControl("txtcredito14xfixo")).Text = String.Format("{0:n2}", ValorFixoPresencialAntecipado(Funcoes.strToInt(ddlPlanosReferencia.SelectedValue.ToString()), Funcoes.strToInt(((TextBox)itemPP.FindControl("txtidbandeira")).Text.ToString()), "C", "P", 14, 1));
                ((TextBox)itemPP.FindControl("txtcredito15xfixo")).Text = String.Format("{0:n2}", ValorFixoPresencialAntecipado(Funcoes.strToInt(ddlPlanosReferencia.SelectedValue.ToString()), Funcoes.strToInt(((TextBox)itemPP.FindControl("txtidbandeira")).Text.ToString()), "C", "P", 15, 1));
                ((TextBox)itemPP.FindControl("txtcredito16xfixo")).Text = String.Format("{0:n2}", ValorFixoPresencialAntecipado(Funcoes.strToInt(ddlPlanosReferencia.SelectedValue.ToString()), Funcoes.strToInt(((TextBox)itemPP.FindControl("txtidbandeira")).Text.ToString()), "C", "P", 16, 1));
                ((TextBox)itemPP.FindControl("txtcredito17xfixo")).Text = String.Format("{0:n2}", ValorFixoPresencialAntecipado(Funcoes.strToInt(ddlPlanosReferencia.SelectedValue.ToString()), Funcoes.strToInt(((TextBox)itemPP.FindControl("txtidbandeira")).Text.ToString()), "C", "P", 17, 1));
                ((TextBox)itemPP.FindControl("txtcredito18xfixo")).Text = String.Format("{0:n2}", ValorFixoPresencialAntecipado(Funcoes.strToInt(ddlPlanosReferencia.SelectedValue.ToString()), Funcoes.strToInt(((TextBox)itemPP.FindControl("txtidbandeira")).Text.ToString()), "C", "P", 18, 1));
                ((TextBox)itemPP.FindControl("txtcredito19xfixo")).Text = String.Format("{0:n2}", ValorFixoPresencialAntecipado(Funcoes.strToInt(ddlPlanosReferencia.SelectedValue.ToString()), Funcoes.strToInt(((TextBox)itemPP.FindControl("txtidbandeira")).Text.ToString()), "C", "P", 19, 1));
                ((TextBox)itemPP.FindControl("txtcredito20xfixo")).Text = String.Format("{0:n2}", ValorFixoPresencialAntecipado(Funcoes.strToInt(ddlPlanosReferencia.SelectedValue.ToString()), Funcoes.strToInt(((TextBox)itemPP.FindControl("txtidbandeira")).Text.ToString()), "C", "P", 20, 1));
                ((TextBox)itemPP.FindControl("txtcredito21xfixo")).Text = String.Format("{0:n2}", ValorFixoPresencialAntecipado(Funcoes.strToInt(ddlPlanosReferencia.SelectedValue.ToString()), Funcoes.strToInt(((TextBox)itemPP.FindControl("txtidbandeira")).Text.ToString()), "C", "P", 21, 1));

            }
            /*
        }
        else
        {
            // OnLine
            foreach (RepeaterItem itemPO in rptConsultaOnline.Items)
            {
                if (ValorPercentualPresencialAntecipado(Funcoes.strToInt(ddlPlanosReferencia.SelectedValue.ToString()), Funcoes.strToInt(((TextBox)itemPO.FindControl("txtidbandeira")).Text.ToString()), "V", "X", 1, 0) > 0)
                {
                    ((TextBox)itemPO.FindControl("txtTaxa")).Text = String.Format("{0:n2}", ValorPercentualPresencialAntecipado(Funcoes.strToInt(ddlPlanosReferencia.SelectedValue.ToString()), Funcoes.strToInt(((TextBox)itemPO.FindControl("txtidbandeira")).Text.ToString()), "V", "X", 1, 0));
                }
                else
                {
                    ((TextBox)itemPO.FindControl("txtTaxa")).Text = String.Format("{0:n2}", ValorPercentualPresencialAntecipado(Funcoes.strToInt(ddlPlanosReferencia.SelectedValue.ToString()), Funcoes.strToInt(((TextBox)itemPO.FindControl("txtidbandeira")).Text.ToString()), "C", "O", Funcoes.strToInt(((TextBox)itemPO.FindControl("txtparcini")).Text.ToString()), 1));
                }
            }

            // Presencial
            foreach (RepeaterItem itemPP in rptConsulta.Items)
            {
                if (ValorPercentualPresencialAntecipado(Funcoes.strToInt(ddlPlanosReferencia.SelectedValue.ToString()), Funcoes.strToInt(((TextBox)itemPP.FindControl("txtidbandeira")).Text.ToString()), "V", "X", 1, 0) > 0)
                {
                    ((TextBox)itemPP.FindControl("txtTaxa")).Text = String.Format("{0:n2}", ValorPercentualPresencialAntecipado(Funcoes.strToInt(ddlPlanosReferencia.SelectedValue.ToString()), Funcoes.strToInt(((TextBox)itemPP.FindControl("txtidbandeira")).Text.ToString()), "V", "X", 1, 0));
                }
                else
                {
                    ((TextBox)itemPP.FindControl("txtTaxa")).Text = String.Format("{0:n2}", ValorPercentualPresencialAntecipado(Funcoes.strToInt(ddlPlanosReferencia.SelectedValue.ToString()), Funcoes.strToInt(((TextBox)itemPP.FindControl("txtidbandeira")).Text.ToString()), "C", "P", Funcoes.strToInt(((TextBox)itemPP.FindControl("txtparcini")).Text.ToString()), 1));
                }
            }

        }
             */

        // Mensagem
        ClientScript.RegisterStartupScript(this.GetType(),"ImportacaoTaxas", "alert('Taxas importadas com sucesso!'); ", true);

    }

    protected void btnImportarTaxas_Click(object sender, EventArgs e)
    {
        ImportarTaxas();
    }

    public double ValorPercentualPresencialAntecipado(int idPlanoReferencia, int idBandeira, string sTipo, string sCaptura, int idParcela, int idParc)
    {
        double fValor = 0;

        SqlConnection mySelCadastro = new SqlConnection(Funcoes.conexao());
        mySelCadastro.Open();
        SqlCommand cmdSelCadastro = new SqlCommand("dbo.stp_planos_referencia_taxas_ins", mySelCadastro);
        cmdSelCadastro.CommandType = CommandType.StoredProcedure;
        cmdSelCadastro.Parameters.Add("@flg_operacao", SqlDbType.Char).Value = "P";
        cmdSelCadastro.Parameters.Add("@FLG_ANTECIPADO", SqlDbType.Char).Value = ddlAntecipado.SelectedValue.ToString();
        cmdSelCadastro.Parameters.Add("@COD_ID_BANDEIRAS", SqlDbType.Int).Value = Funcoes.strToInt(idBandeira.ToString());
        cmdSelCadastro.Parameters.Add("@COD_ID_PLANOS_REFERENCIA", SqlDbType.Int).Value = Funcoes.strToInt(idPlanoReferencia.ToString());
        cmdSelCadastro.Parameters.Add("@NUM_PARCELAS", SqlDbType.Int).Value = Funcoes.strToInt(idParcela.ToString());
        cmdSelCadastro.Parameters.Add("@NUM_PARC", SqlDbType.Int).Value = Funcoes.strToInt(idParc.ToString());
        cmdSelCadastro.Parameters.Add("@NOM_MODO_CAPTURA", SqlDbType.Char).Value = sCaptura.ToString();
        cmdSelCadastro.Parameters.Add("@FLG_TIPO", SqlDbType.Char).Value = sTipo.ToString();
        SqlDataReader ReaderCadastro = cmdSelCadastro.ExecuteReader();
        while (ReaderCadastro.Read())
        {
            fValor = Funcoes.strToDouble(ReaderCadastro["NUM_VALOR_PERCENTUAL"].ToString());
        }

        return fValor;
    }


    public double ValorFixoPresencialAntecipado(int idPlanoReferencia, int idBandeira, string sTipo, string sCaptura, int idParcela, int idParc)
    {
        double fValor = 0;

        SqlConnection mySelCadastro = new SqlConnection(Funcoes.conexao());
        mySelCadastro.Open();
        SqlCommand cmdSelCadastro = new SqlCommand("dbo.stp_planos_referencia_taxas_ins", mySelCadastro);
        cmdSelCadastro.CommandType = CommandType.StoredProcedure;
        cmdSelCadastro.Parameters.Add("@flg_operacao", SqlDbType.Char).Value = "P";
        cmdSelCadastro.Parameters.Add("@FLG_ANTECIPADO", SqlDbType.Char).Value = ddlAntecipado.SelectedValue.ToString();
        cmdSelCadastro.Parameters.Add("@COD_ID_BANDEIRAS", SqlDbType.Int).Value = Funcoes.strToInt(idBandeira.ToString());
        cmdSelCadastro.Parameters.Add("@COD_ID_PLANOS_REFERENCIA", SqlDbType.Int).Value = Funcoes.strToInt(idPlanoReferencia.ToString());
        cmdSelCadastro.Parameters.Add("@NUM_PARCELAS", SqlDbType.Int).Value = Funcoes.strToInt(idParcela.ToString());
        cmdSelCadastro.Parameters.Add("@NUM_PARC", SqlDbType.Int).Value = Funcoes.strToInt(idParc.ToString());
        cmdSelCadastro.Parameters.Add("@NOM_MODO_CAPTURA", SqlDbType.Char).Value = sCaptura.ToString();
        cmdSelCadastro.Parameters.Add("@FLG_TIPO", SqlDbType.Char).Value = sTipo.ToString();
        SqlDataReader ReaderCadastro = cmdSelCadastro.ExecuteReader();
        while (ReaderCadastro.Read())
        {
            fValor = Funcoes.strToDouble(ReaderCadastro["NUM_VALOR"].ToString());
        }

        return fValor;
    }

    protected void btnImportarTexto_Click(object sender, EventArgs e)
    {
        string sCodificacao = DateTime.Now.Year.ToString() + DateTime.Now.Month.ToString() + DateTime.Now.Day.ToString() + DateTime.Now.Hour.ToString() + DateTime.Now.Minute.ToString() + DateTime.Now.Second.ToString();
        string serverPath = System.Web.HttpContext.Current.Server.MapPath("~/");
        string StrFileName = File1.PostedFile.FileName.Substring(File1.PostedFile.FileName.LastIndexOf("\\") + 1);
        string StrFileType = File1.PostedFile.ContentType;
        int IntFileSize = File1.PostedFile.ContentLength;
        File1.PostedFile.SaveAs(Server.MapPath("public_html") + "\\" + sCodificacao + "_ARQUIVO_" + StrFileName.ToString());

        string sArquivo = "";

        sArquivo = Server.MapPath("public_html") + "\\" + sCodificacao + "_ARQUIVO_" + StrFileName.ToString();

        GridView1.DataSource = Funcoes.ImportExcelXLS(sArquivo.ToString(), true).Tables[0];
        GridView1.DataBind();

        //GridView1.Visible = true;

        // Credito Linha de 3 a 23 e colunas de 2 a 7

        int iRow = 3;
        int iCol = 2;

        //ClientScript.RegisterStartupScript(this.GetType(), "ColunaLinha", "alert('"+ GridView1.Rows[iRow].Cells[iCol].Text.Trim() + "'); ", true);
        //ClientScript.RegisterStartupScript(this.GetType(), "Cabecalho", "alert('" + GridView1.HeaderRow.Cells[iCol].Text.Trim() + "'); ", true);

        for (int j = 2; j <= 7; j++) // colunas 2 a 7 (índices 1 até 6)
        {

            //ClientScript.RegisterStartupScript(this.GetType(), "Cabecalho"+j.ToString(), "alert('" + GridView1.HeaderRow.Cells[j].Text.ToString().Trim() + "'); ", true);
            
            string cabecalho = GridView1.HeaderRow.Cells[j].Text.Trim();

            foreach (RepeaterItem itemPP in rptConsultaParcelas.Items)
            {
                string txt = ((TextBox)itemPP.FindControl("txtnomebandeira")).Text.ToString().Trim();

                if (txt.Contains(cabecalho))
                {
                    for (int i = 3; i <= 23; i++) // linhas 3 a 23 (índices 2 até 22)
                    {
                        if (i - 2 == 1)
                        {
                            ((TextBox)itemPP.FindControl("txtcredito")).Text = (Funcoes.strToDouble(GridView1.Rows[i].Cells[j].Text.ToString().Trim())*100).ToString();
                            ((TextBox)itemPP.FindControl("txtrebatevista")).Text = (Funcoes.strToDouble(GridView1.Rows[i].Cells[10].Text.ToString().Trim())*100).ToString();
                        }
                        else
                        {
                            ((TextBox)itemPP.FindControl("txtcredito" + (i-2).ToString() + "x")).Text = (Funcoes.strToDouble(GridView1.Rows[i].Cells[j].Text.ToString().Trim())*100).ToString();
                            ((TextBox)itemPP.FindControl("txtrebate" + (i - 2).ToString() + "x")).Text = (Funcoes.strToDouble(GridView1.Rows[i].Cells[10].Text.ToString().Trim()) * 100).ToString();
                        }

                        //GridView1.Rows[i].Cells[j].Text.Trim()
                    }
                }
            }
        }

        ClientScript.RegisterStartupScript(this.GetType(), "ImportacaoExcel", "alert('Importação realizada com sucesso!'); ", true);


        //foreach (GridViewRow row in GridView1.Rows)
        //{
        //
        //}
    }
}