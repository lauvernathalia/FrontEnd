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


public partial class cad_marketplaces_importar : System.Web.UI.Page
{

    public static DataTable dtMarketplaces;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            // Definição Tabela de Representantes
            dtMarketplaces = new DataTable();
            dtMarketplaces.Columns.Add("documento", typeof(string));
            dtMarketplaces.Columns.Add("nome", typeof(string));
            dtMarketplaces.Columns.Add("status", typeof(string));
            dtMarketplaces.Columns.Add("portal", typeof(string));
            dtMarketplaces.Columns.Add("estabelecimentos", typeof(string));
            dtMarketplaces.Columns.Add("estabelecimentosportal", typeof(string));

            // Verificar Integrações
            CarregaAdquirentes();
            ddlAdquirentes_SelectedIndexChanged(null, null);

        }

    }

    private void CarregaAdquirentes()
    {
        /*
        SqlConnection myAdquirentes = new SqlConnection(Funcoes.conexao());
        myAdquirentes.Open();
        SqlCommand cmdAdquirentes = new SqlCommand("dbo.stp_pessoas_fj_integracoes_chaves_ins", myAdquirentes);
        cmdAdquirentes.CommandType = CommandType.StoredProcedure;
        cmdAdquirentes.Parameters.Add("@flg_operacao", SqlDbType.Char).Value = "F";
        cmdAdquirentes.Parameters.Add("@COD_ID_PESSOA_LICENCIADO", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["LICENCIADO"].ToString());
        cmdAdquirentes.Parameters.Add("@FLG_ATIVO", SqlDbType.Char).Value = "S";

        SqlDataAdapter drAdquirentes = new SqlDataAdapter();
        drAdquirentes.SelectCommand = cmdAdquirentes;
        DataSet dsAdquirentes = new DataSet();
        drAdquirentes.Fill(dsAdquirentes, "PESSOAS_FJ_INTEGRACOES");

        ddlAdquirentes.DataTextField = "NOM_FANTASIA_INTEGRACAO";
        ddlAdquirentes.DataValueField = "FLG_INTEGRACAO";
        ddlAdquirentes.DataSource = dsAdquirentes.Tables["PESSOAS_FJ_INTEGRACOES"].DefaultView;
        ddlAdquirentes.DataBind();
         */

        SqlConnection myAdquirentes = new SqlConnection(Funcoes.conexao());
        myAdquirentes.Open();
        SqlCommand cmdAdquirentes = new SqlCommand("dbo.stp_pessoas_fj_integracoes_ins", myAdquirentes);
        cmdAdquirentes.CommandType = CommandType.StoredProcedure;
        if (HttpContext.Current.Session["TIPO"].ToString() == "L")
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
        ddlAdquirentes.Items.Insert(0, new ListItem("Todos", ""));

    }

    protected void ddlAdquirentes_SelectedIndexChanged(object sender, EventArgs e)
    {
        divAdquirenteCappta.Visible = false;
        divAdquirenteZoop.Visible = false;
        divAdquirenteAsaas.Visible = false;
        divAdquirentePagseguro.Visible = false;
        divAdquirenteErp.Visible = false;

        if (ddlAdquirentes.SelectedValue.ToString() == "C")
        {
            divAdquirenteCappta.Visible = true;
        }
        if (ddlAdquirentes.SelectedValue.ToString() == "Z")
        {
            divAdquirenteZoop.Visible = true;
        }
        if (ddlAdquirentes.SelectedValue.ToString() == "P")
        {
            divAdquirentePagseguro.Visible = true;
        }
        if (ddlAdquirentes.SelectedValue.ToString() == "A")
        {
            divAdquirenteAsaas.Visible = true;
        }

        if (ddlAdquirentes.SelectedValue.ToString() == "E")
        {
            divAdquirenteErp.Visible = true;
        }
    }


    protected void btnCarregarCadastrosCappta_Click(object sender, EventArgs e)
    {
        try
        {
            string jsonRevendedores = "";
            
            jsonRevendedores = hubcappta.ConsultarRevendedores();
            //txtDocumentoCappta.Text = hubcappta.ConsultarRevendedor("37917825000185");
            //txtDocumentoCappta.Text = jsonRevendedores;
            
            JArray oRevendedores = JArray.Parse(jsonRevendedores);
            //JObject oRevendedores = JObject.Parse(jsonRevendedores);

            if (oRevendedores.Count > 0)
            {
                for (int i = 0; i < oRevendedores.Count; i++)
                {
                    // Verificar a existência na base de dados
                    // Verificar a quantidade de estabelecimentos
                    string sEstabelecimentos = "0";
                    string sEstabelecimentosPortal = "0";
                    
                    // Carregar todos os estabelecimentos
                    string jsonEstabelecimentos = "";
                    jsonEstabelecimentos = hubcappta.ConsultarLojistas(oRevendedores[i]["reseller"]["document"].ToString());
                    JArray oEstabelecimentos = JArray.Parse(jsonEstabelecimentos);
                    if (oEstabelecimentos.Count > 0)
                    {
                        sEstabelecimentos = (oEstabelecimentos.Count).ToString();
                    }

                    // Carrega os estabelecimentos portal
                    SqlConnection mySelCadastro = new SqlConnection(Funcoes.conexao());
                    mySelCadastro.Open();
                    SqlCommand cmdSelCadastro = new SqlCommand("dbo.stp_pessoas_fj_ins", mySelCadastro);
                    cmdSelCadastro.CommandType = CommandType.StoredProcedure;
                    cmdSelCadastro.Parameters.Add("@flg_operacao", SqlDbType.Char).Value = "9";
                    cmdSelCadastro.Parameters.Add("@NOM_CNPJ", SqlDbType.VarChar).Value = oRevendedores[i]["reseller"]["document"].ToString();
                    cmdSelCadastro.Parameters.Add("@COD_ID_PESSOA_LICENCIADO", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["LICENCIADO"].ToString());

                    SqlDataReader ReaderCadastro = cmdSelCadastro.ExecuteReader();
                    while (ReaderCadastro.Read())
                    {
                        sEstabelecimentosPortal = ReaderCadastro["NUM_ESTABELECIMENTOS"].ToString();
                    }
                    dtMarketplaces.Rows.Add(oRevendedores[i]["reseller"]["document"].ToString(), oRevendedores[i]["reseller"]["companyName"].ToString(), oRevendedores[i]["statusDescription"].ToString(), (hubcappta.MarketplacePortal(oRevendedores[i]["reseller"]["document"].ToString())) ? "Sim" : "Não", sEstabelecimentos, sEstabelecimentosPortal);
                }
            }

            this.lsvMarketplacesCappta.DataSource = dtMarketplaces;
            this.lsvMarketplacesCappta.DataBind();

            foreach (ListViewItem itemP in lsvMarketplacesCappta.Items)
            {
                if (((TextBox)itemP.FindControl("txtPortal")).Text.ToString().Trim() == "Não")
                {
                    ((CheckBox)itemP.FindControl("ckbImportar")).Checked = true;
                }
                if (Funcoes.strToInt(((TextBox)itemP.FindControl("txtestabelecimentos")).Text.ToString().Trim()) > Funcoes.strToInt(((TextBox)itemP.FindControl("txtestabelecimentosportal")).Text.ToString().Trim()))
                {
                    ((CheckBox)itemP.FindControl("ckbImportar")).Checked = true;
                    ((Label)itemP.FindControl("lblestabelecimentosportal")).ForeColor = System.Drawing.Color.Red;
                    ((Label)itemP.FindControl("lblestabelecimentos")).ForeColor = System.Drawing.Color.Red;

                }
            }

        }
        catch
        {
            ClientScript.RegisterStartupScript(this.GetType(), "ErroCarregar", "alert('Ocorreu um erro ao tentar carregar os dados! Verifique e tente novamente mais tarde.');", true);
        }

    }
    protected void btnImportarCappta_Click(object sender, EventArgs e)
    {
        foreach (ListViewItem itemP in lsvMarketplacesCappta.Items)
        {
            if (((CheckBox)itemP.FindControl("ckbImportar")).Checked == true)
            {
                //Console.Write("MKT:" + ((TextBox)itemP.FindControl("txtDocumento")).Text.ToString());
                GravarDadosMarketplace(((TextBox)itemP.FindControl("txtDocumento")).Text.ToString());

                if (ckbEstabelecimentos.Checked == true)
                {
                    // Carregar todos os estabelecimentos
                    string jsonEstabelecimentos = "";
                    jsonEstabelecimentos = hubcappta.ConsultarLojistas(((TextBox)itemP.FindControl("txtDocumento")).Text.ToString());

                    JArray oEstabelecimentos = JArray.Parse(jsonEstabelecimentos);
                    if (oEstabelecimentos.Count > 0)
                    {
                        ClientScript.RegisterStartupScript(this.GetType(), "QtdeEstabelecimentos", "alert('Existem " + oEstabelecimentos.Count.ToString() + " para importar');", true);

                        
                        for (int i = 0; i < oEstabelecimentos.Count; i++)
                        {

                            if (ckbPlanos.Checked == true)
                            {
                                //txtPlanos.Text = txtPlanos.Text + jsonEstabelecimentos + "     -----     ";
                                //dvPlanos.Visible = true;
                                if (oEstabelecimentos[i]["plans"].Count() > 0)
                                {
                                    //Console.Write("Plano:" + oEstabelecimentos[i]["plans"][0]["id"].ToString());
                                    GravarDadosPlano(oEstabelecimentos[i]["plans"][0]["id"].ToString(), ((TextBox)itemP.FindControl("txtDocumento")).Text.ToString());
                                    // Importar o plano base e o plano do estabelecimento
                                    // oEstabelecimentos[i]["plans"][0]["id"].ToString();
                                }
                            }
                            
                            // Gravar dados do estabelecimento
                            //Console.Write("EC:" + oEstabelecimentos[i]["merchant"]["document"].ToString());
                            GravarDadosEstabelecimentoCappta(oEstabelecimentos[i]["merchant"]["document"].ToString(), ((TextBox)itemP.FindControl("txtDocumento")).Text.ToString());
                        }
                    }
                }

                ((CheckBox)itemP.FindControl("ckbImportar")).Checked = false;
            }
        }
        ckbTodosCappta.Checked = false;
    }
    protected void btnCarregarCadastrosZoop_Click(object sender, EventArgs e)
    {

    }
    protected void btnImportarZoop_Click(object sender, EventArgs e)
    {

    }
    protected void btnCarregarCadastrosAsaas_Click(object sender, EventArgs e)
    {

    }
    protected void btnImportarAsaas_Click(object sender, EventArgs e)
    {

    }
    protected void btnCarregarCadastrosPagseguro_Click(object sender, EventArgs e)
    {

    }
    protected void btnImportarPagseguro_Click(object sender, EventArgs e)
    {

    }
    protected void btnFechar_Click(object sender, EventArgs e)
    {
        ClientScript.RegisterStartupScript(this.GetType(), "FecharOnboarding", "opener.PostBackOnMainPage(); window.close(); ", true);
    }
    protected void lsvRepresentantes_ItemCommand(object sender, ListViewCommandEventArgs e)
    {

    }
    protected void ckbTodos_CheckedChanged(object sender, EventArgs e)
    {
        foreach (ListViewItem itemP in lsvMarketplacesCappta.Items)
        {
            ((CheckBox)itemP.FindControl("ckbImportar")).Checked = (((CheckBox)itemP.FindControl("ckbImportar")).Checked == false) ? true : false;
        }

    }
    protected void btnImportarDocumento_Click(object sender, EventArgs e)
    {
        if (txtDocumentoCappta.Text.ToString().Trim() != "")
        {
            GravarDadosMarketplace(txtDocumentoCappta.Text.ToString());
        }
        else
        {
            ClientScript.RegisterStartupScript(this.GetType(),
                "Alerta", "alert('Nenhum documento foi especificado! Verifique e reentre.');", true);

        }
    }

    private void GravarDadosPlano(string sPlano, string sMarketplace)
    {
        string jsonPlano = hubcappta.ConsultarPlano(sPlano.ToString());

        //txtPlanos.Text = txtPlanos.Text + jsonPlano + "     -----     ";
        if (jsonPlano.ToString().Trim() != "")
        {
            try
            {

                // ********************************************************************
                // Localizar cadastro do Marketplace
                // ********************************************************************
                string sIDMarketplace = "0";
                SqlConnection mySelCadastroMkt = new SqlConnection(Funcoes.conexao());
                mySelCadastroMkt.Open();
                SqlCommand cmdSelCadastroMkt = new SqlCommand("dbo.stp_pessoas_fj_ins", mySelCadastroMkt);
                cmdSelCadastroMkt.CommandType = CommandType.StoredProcedure;
                cmdSelCadastroMkt.Parameters.Add("@flg_operacao", SqlDbType.Char).Value = "H";
                cmdSelCadastroMkt.Parameters.Add("@NOM_CNPJ", SqlDbType.VarChar).Value = sMarketplace.ToString();
                cmdSelCadastroMkt.Parameters.Add("@COD_ID_PESSOA_LICENCIADO", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["LICENCIADO"].ToString());
                cmdSelCadastroMkt.Parameters.Add("@FLG_TIPO", SqlDbType.Char).Value = "M";

                SqlDataReader ReaderCadastroMkt = cmdSelCadastroMkt.ExecuteReader();
                while (ReaderCadastroMkt.Read())
                {
                    sIDMarketplace = ReaderCadastroMkt["COD_ID"].ToString();
                }



                JObject oPlanos = JObject.Parse(jsonPlano.ToString());

                SqlConnection connInsCons = new SqlConnection(Funcoes.conexao());
                connInsCons.Open();
                SqlCommand cmdInsCons = new SqlCommand("dbo.stp_planos_ins", connInsCons);
                cmdInsCons.CommandType = CommandType.StoredProcedure;
                cmdInsCons.Parameters.Add("@flg_operacao", SqlDbType.Char).Value = 'I';

                cmdInsCons.Parameters.Add("@COD_ID_PESSOA_LICENCIADO", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["LICENCIADO"].ToString());
                cmdInsCons.Parameters.Add("@COD_ID_PESSOAS_FJ", SqlDbType.Int).Value = Funcoes.strToInt(sIDMarketplace.ToString());

                cmdInsCons.Parameters.Add("@FLG_MODELO_PLANO", SqlDbType.Char).Value = "C";
                cmdInsCons.Parameters.Add("@FLG_ATIVO", SqlDbType.Char).Value = "S";
                cmdInsCons.Parameters.Add("@FLG_VISIVEL", SqlDbType.Char).Value = "S";
                cmdInsCons.Parameters.Add("@FLG_ANTECIPADO", SqlDbType.Char).Value = "S";
                cmdInsCons.Parameters.Add("@FLG_TIPO_PLANO", SqlDbType.Char).Value = "C";
                cmdInsCons.Parameters.Add("@NOM_REFERENCIA_PLANO", SqlDbType.VarChar).Value = oPlanos["id"].ToString().Trim();


                cmdInsCons.Parameters.Add("@COD_ID_PLANO_BASE", SqlDbType.Int).Value = Funcoes.strToInt(oPlanos["basePlanId"].ToString().Trim());
                // LOCALIZAR PLANO REFERENCIA

                SqlConnection mySelCadastroPlano = new SqlConnection(Funcoes.conexao());
                mySelCadastroPlano.Open();
                SqlCommand cmdSelCadastroPlano = new SqlCommand("dbo.stp_planos_referencia_ins", mySelCadastroPlano);
                cmdSelCadastroPlano.CommandType = CommandType.StoredProcedure;
                cmdSelCadastroPlano.Parameters.Add("@flg_operacao", SqlDbType.Char).Value = "P";
                cmdSelCadastroPlano.Parameters.Add("@COD_ID_PLANO_REFERENCIA", SqlDbType.VarChar).Value = oPlanos["basePlanId"].ToString().Trim();
                SqlDataReader ReaderCadastroPlano = cmdSelCadastroPlano.ExecuteReader();
                while (ReaderCadastroPlano.Read())
                {
                    cmdInsCons.Parameters.Add("@COD_ID_PLANOS_REFERENCIA", SqlDbType.Int).Value = Funcoes.strToInt(ReaderCadastroPlano["COD_ID"].ToString().Trim());
                }

                cmdInsCons.Parameters.Add("@NOM_TITULO_PLANO", SqlDbType.VarChar).Value = oPlanos["name"].ToString().Trim(); ;
                cmdInsCons.Parameters.Add("@DES_PLANO", SqlDbType.Text).Value = oPlanos["name"].ToString().Trim();
                cmdInsCons.Parameters.Add("@NUM_DIAS_LIQUIDACAO", SqlDbType.Int).Value = Funcoes.strToInt(oPlanos["settlementDays"].ToString().Trim());
                cmdInsCons.Parameters.Add("@NUM_TAXA_ANTECIPACAO", SqlDbType.Float).Value = 0;
                string sCodigoInclusao = cmdInsCons.ExecuteScalar().ToString();
                connInsCons.Close();
                connInsCons.Dispose();


                for (int t = 0; t < oPlanos["schemes"].Count(); t++)
                {
                    for (int p = 0; p < oPlanos["schemes"][t]["fees"].Count(); p++)
                    {
                        SqlConnection connInsConsTaxas = new SqlConnection(Funcoes.conexao());
                        connInsConsTaxas.Open();
                        SqlCommand cmdInsConsTaxas = new SqlCommand("dbo.stp_planos_parcelas_ins", connInsConsTaxas);
                        cmdInsConsTaxas.CommandType = CommandType.StoredProcedure;
                        cmdInsConsTaxas.Parameters.Add("@flg_operacao", SqlDbType.Char).Value = 'I';
                        cmdInsConsTaxas.Parameters.Add("@COD_ID_PESSOA_LICENCIADO", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["LICENCIADO"].ToString());
                        cmdInsConsTaxas.Parameters.Add("@COD_ID_PLANOS", SqlDbType.Int).Value = Funcoes.strToInt(sCodigoInclusao);

                        int iBandeira = 0;
                        int iPlanoReferencia = 0;
                        string sBandeira = "";
                        string sTipo = "";
                        // Busca Bandeira Tabela OPÇÕES PLANOS

                        SqlConnection mySelCadastroBandeiras = new SqlConnection(Funcoes.conexao());
                        mySelCadastroBandeiras.Open();
                        SqlCommand cmdSelCadastroBandeiras = new SqlCommand("dbo.stp_opcoes_planos_cappta_ins", mySelCadastroBandeiras);
                        cmdSelCadastroBandeiras.CommandType = CommandType.StoredProcedure;
                        cmdSelCadastroBandeiras.Parameters.Add("@flg_operacao", SqlDbType.Char).Value = "B";
                        cmdSelCadastroBandeiras.Parameters.Add("@NOM_REFERENCIA", SqlDbType.VarChar).Value = oPlanos["schemes"][t]["scheme"].ToString();
                        SqlDataReader ReaderCadastroBandeiras = cmdSelCadastroBandeiras.ExecuteReader();
                        while (ReaderCadastroBandeiras.Read())
                        {
                            iBandeira = Funcoes.strToInt(ReaderCadastroBandeiras["COD_BANDEIRA"].ToString());
                            sBandeira = ReaderCadastroBandeiras["NOM_BANDEIRA"].ToString();
                            sTipo = ReaderCadastroBandeiras["NOM_TIPO"].ToString();
                        }


                        cmdInsConsTaxas.Parameters.Add("@COD_ID_BANDEIRA", SqlDbType.Int).Value = iBandeira;
                        cmdInsConsTaxas.Parameters.Add("@FLG_PRESENCIAL_ONLINE", SqlDbType.Char).Value = "P";
                        // Localizar o plano de referencia


                        // Carrega as taxas
                        if (sTipo == "debit")
                        {
                            cmdInsConsTaxas.Parameters.Add("@NUM_VALOR_DEBITO", SqlDbType.Float).Value = Funcoes.strToDouble(oPlanos["schemes"][t]["fees"][p]["rate"].ToString()) / 100;
                        }
                        if (sTipo == "credit")
                        {
                            if (Funcoes.strToInt(oPlanos["schemes"][t]["fees"][p]["installments"].ToString()) == 1) { cmdInsConsTaxas.Parameters.Add("@NUM_VALOR_CREDITO_VISTA", SqlDbType.Float).Value = Funcoes.strToDouble(oPlanos["schemes"][t]["fees"][p]["rate"].ToString()) / 100; }
                            if (Funcoes.strToInt(oPlanos["schemes"][t]["fees"][p]["installments"].ToString()) == 2) { cmdInsConsTaxas.Parameters.Add("@NUM_VALOR_CREDITO_2X", SqlDbType.Float).Value = Funcoes.strToDouble(oPlanos["schemes"][t]["fees"][p]["rate"].ToString()) / 100; }
                            if (Funcoes.strToInt(oPlanos["schemes"][t]["fees"][p]["installments"].ToString()) == 3) { cmdInsConsTaxas.Parameters.Add("@NUM_VALOR_CREDITO_3X", SqlDbType.Float).Value = Funcoes.strToDouble(oPlanos["schemes"][t]["fees"][p]["rate"].ToString()) / 100; }
                            if (Funcoes.strToInt(oPlanos["schemes"][t]["fees"][p]["installments"].ToString()) == 4) { cmdInsConsTaxas.Parameters.Add("@NUM_VALOR_CREDITO_4X", SqlDbType.Float).Value = Funcoes.strToDouble(oPlanos["schemes"][t]["fees"][p]["rate"].ToString()) / 100; }
                            if (Funcoes.strToInt(oPlanos["schemes"][t]["fees"][p]["installments"].ToString()) == 5) { cmdInsConsTaxas.Parameters.Add("@NUM_VALOR_CREDITO_5X", SqlDbType.Float).Value = Funcoes.strToDouble(oPlanos["schemes"][t]["fees"][p]["rate"].ToString()) / 100; }
                            if (Funcoes.strToInt(oPlanos["schemes"][t]["fees"][p]["installments"].ToString()) == 6) { cmdInsConsTaxas.Parameters.Add("@NUM_VALOR_CREDITO_6X", SqlDbType.Float).Value = Funcoes.strToDouble(oPlanos["schemes"][t]["fees"][p]["rate"].ToString()) / 100; }
                            if (Funcoes.strToInt(oPlanos["schemes"][t]["fees"][p]["installments"].ToString()) == 7) { cmdInsConsTaxas.Parameters.Add("@NUM_VALOR_CREDITO_7X", SqlDbType.Float).Value = Funcoes.strToDouble(oPlanos["schemes"][t]["fees"][p]["rate"].ToString()) / 100; }
                            if (Funcoes.strToInt(oPlanos["schemes"][t]["fees"][p]["installments"].ToString()) == 8) { cmdInsConsTaxas.Parameters.Add("@NUM_VALOR_CREDITO_8X", SqlDbType.Float).Value = Funcoes.strToDouble(oPlanos["schemes"][t]["fees"][p]["rate"].ToString()) / 100; }
                            if (Funcoes.strToInt(oPlanos["schemes"][t]["fees"][p]["installments"].ToString()) == 9) { cmdInsConsTaxas.Parameters.Add("@NUM_VALOR_CREDITO_9X", SqlDbType.Float).Value = Funcoes.strToDouble(oPlanos["schemes"][t]["fees"][p]["rate"].ToString()) / 100; }
                            if (Funcoes.strToInt(oPlanos["schemes"][t]["fees"][p]["installments"].ToString()) == 10) { cmdInsConsTaxas.Parameters.Add("@NUM_VALOR_CREDITO_10X", SqlDbType.Float).Value = Funcoes.strToDouble(oPlanos["schemes"][t]["fees"][p]["rate"].ToString()) / 100; }
                            if (Funcoes.strToInt(oPlanos["schemes"][t]["fees"][p]["installments"].ToString()) == 11) { cmdInsConsTaxas.Parameters.Add("@NUM_VALOR_CREDITO_11X", SqlDbType.Float).Value = Funcoes.strToDouble(oPlanos["schemes"][t]["fees"][p]["rate"].ToString()) / 100; }
                            if (Funcoes.strToInt(oPlanos["schemes"][t]["fees"][p]["installments"].ToString()) == 12) { cmdInsConsTaxas.Parameters.Add("@NUM_VALOR_CREDITO_12X", SqlDbType.Float).Value = Funcoes.strToDouble(oPlanos["schemes"][t]["fees"][p]["rate"].ToString()) / 100; }
                            if (Funcoes.strToInt(oPlanos["schemes"][t]["fees"][p]["installments"].ToString()) == 13) { cmdInsConsTaxas.Parameters.Add("@NUM_VALOR_CREDITO_13X", SqlDbType.Float).Value = Funcoes.strToDouble(oPlanos["schemes"][t]["fees"][p]["rate"].ToString()) / 100; }
                            if (Funcoes.strToInt(oPlanos["schemes"][t]["fees"][p]["installments"].ToString()) == 14) { cmdInsConsTaxas.Parameters.Add("@NUM_VALOR_CREDITO_14X", SqlDbType.Float).Value = Funcoes.strToDouble(oPlanos["schemes"][t]["fees"][p]["rate"].ToString()) / 100; }
                            if (Funcoes.strToInt(oPlanos["schemes"][t]["fees"][p]["installments"].ToString()) == 15) { cmdInsConsTaxas.Parameters.Add("@NUM_VALOR_CREDITO_15X", SqlDbType.Float).Value = Funcoes.strToDouble(oPlanos["schemes"][t]["fees"][p]["rate"].ToString()) / 100; }
                            if (Funcoes.strToInt(oPlanos["schemes"][t]["fees"][p]["installments"].ToString()) == 16) { cmdInsConsTaxas.Parameters.Add("@NUM_VALOR_CREDITO_16X", SqlDbType.Float).Value = Funcoes.strToDouble(oPlanos["schemes"][t]["fees"][p]["rate"].ToString()) / 100; }
                            if (Funcoes.strToInt(oPlanos["schemes"][t]["fees"][p]["installments"].ToString()) == 17) { cmdInsConsTaxas.Parameters.Add("@NUM_VALOR_CREDITO_17X", SqlDbType.Float).Value = Funcoes.strToDouble(oPlanos["schemes"][t]["fees"][p]["rate"].ToString()) / 100; }
                            if (Funcoes.strToInt(oPlanos["schemes"][t]["fees"][p]["installments"].ToString()) == 18) { cmdInsConsTaxas.Parameters.Add("@NUM_VALOR_CREDITO_18X", SqlDbType.Float).Value = Funcoes.strToDouble(oPlanos["schemes"][t]["fees"][p]["rate"].ToString()) / 100; }
                        }


                        cmdInsConsTaxas.ExecuteNonQuery();
                        connInsConsTaxas.Close();
                        connInsConsTaxas.Dispose();
                    }
                }
            }
            catch
            {
            }
        }
    }


    private void GravarDadosMarketplace(string sDocumento)
    {
        string jsonRetornoCadastro = hubcappta.ConsultarRevendedor(sDocumento.ToString());

        txtMarketplaces.Text = jsonRetornoCadastro;

        txtDocumentoCappta.Text = jsonRetornoCadastro;

        string jsonRetornoPlanos = hubcappta.ConsultarPlanos(1, "true", "Scheme&resellerDocument=" + sDocumento.ToString());
        //string jsonRetornoPlanos = hubcappta.ConsultarPlano("15");
        //txtPlanos.Text = jsonRetornoPlanos;
        dvPlanos.Visible = false;

        string jsonRetornoEstabelecimentos = hubcappta.ConsultarLojistas(sDocumento.ToString());
        txtEstabelecimentos.Text = jsonRetornoEstabelecimentos;
        dvEstabelecimentos.Visible = false;

        JObject oRevendedor = JObject.Parse(jsonRetornoCadastro);

        try
        {

            SqlConnection connInsCons = new SqlConnection(Funcoes.conexao());
            connInsCons.Open();
            SqlCommand cmdInsCons = new SqlCommand("dbo.stp_pessoas_fj_ins", connInsCons);
            cmdInsCons.CommandType = CommandType.StoredProcedure;
            cmdInsCons.Parameters.Add("@flg_operacao", SqlDbType.Char).Value = 'I';
            cmdInsCons.Parameters.Add("@FLG_ATIVO", SqlDbType.Char).Value = "S";
            cmdInsCons.Parameters.Add("@FLG_INSERT_USUARIO", SqlDbType.Char).Value = "S";
            cmdInsCons.Parameters.Add("@COD_ID_PESSOA_LICENCIADO", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["LICENCIADO"].ToString());
            cmdInsCons.Parameters.Add("@DTA_DATA", SqlDbType.DateTime).Value = DateTime.Now;
            cmdInsCons.Parameters.Add("@FLG_TIPO", SqlDbType.Char).Value = "M";

            cmdInsCons.Parameters.Add("@FLG_TIPO_PESSOA", SqlDbType.VarChar).Value = (oRevendedor["reseller"]["document"].ToString().Trim().Length >= 14) ? "PJ" : "PF";
            cmdInsCons.Parameters.Add("@FLG_PRESENCIAL", SqlDbType.Char).Value = "N";
            //            cmdInsCons.Parameters.Add("@COD_ID_MARKETPLACE", SqlDbType.Int).Value = Funcoes.strToInt(ddlMarketplace.SelectedValue.ToString());

            // Empresa
            cmdInsCons.Parameters.Add("@NOM_RAZAOSOCIAL", SqlDbType.VarChar).Value = oRevendedor["reseller"]["companyName"].ToString().Trim();
            cmdInsCons.Parameters.Add("@NOM_FANTASIA", SqlDbType.VarChar).Value = oRevendedor["reseller"]["tradingName"].ToString().Trim();
            cmdInsCons.Parameters.Add("@NOM_CNPJ", SqlDbType.VarChar).Value = oRevendedor["reseller"]["document"].ToString().Trim();

            cmdInsCons.Parameters.Add("@NUM_TELEFONE", SqlDbType.VarChar).Value = oRevendedor["responsible"]["phone"].ToString().Trim();
            cmdInsCons.Parameters.Add("@NOM_EMAIL_EMPRESA", SqlDbType.VarChar).Value = oRevendedor["responsible"]["email"].ToString().Trim();
            cmdInsCons.Parameters.Add("@COD_ID_MCC", SqlDbType.Int).Value = Funcoes.strToInt(oRevendedor["reseller"]["mccId"].ToString().Trim());
            //cmdInsCons.Parameters.Add("@NOM_TIPO_EMPRESA", SqlDbType.VarChar).Value = ddlTipoEmpresa.SelectedValue.ToString();
            cmdInsCons.Parameters.Add("@NUM_FATURAMENTO", SqlDbType.Float).Value = 0;
            cmdInsCons.Parameters.Add("@NUM_PATRIMONIO", SqlDbType.Float).Value = 0;

            //if (txtDataAbertura.Text.ToString().Trim() != "")
            //{
            //    cmdInsCons.Parameters.Add("@DTA_ABERTURA", SqlDbType.DateTime).Value = Convert.ToDateTime(txtDataAbertura.Text.ToString());
            //}

            // Endereço
            cmdInsCons.Parameters.Add("@NOM_ENDERECO", SqlDbType.VarChar).Value = oRevendedor["address"]["streetName"].ToString().Trim();
            cmdInsCons.Parameters.Add("@NOM_NUMERO", SqlDbType.VarChar).Value = oRevendedor["address"]["houseNumber"].ToString().Trim();
            cmdInsCons.Parameters.Add("@NOM_COMPLEMENTO", SqlDbType.VarChar).Value = oRevendedor["address"]["complement"].ToString().Trim();
            cmdInsCons.Parameters.Add("@NOM_BAIRRO", SqlDbType.VarChar).Value = oRevendedor["address"]["neighborhood"].ToString().Trim();
            cmdInsCons.Parameters.Add("@NOM_CIDADE", SqlDbType.VarChar).Value = oRevendedor["address"]["city"].ToString().Trim();
            cmdInsCons.Parameters.Add("@NOM_UF", SqlDbType.VarChar).Value = oRevendedor["address"]["state"].ToString().Trim();
            cmdInsCons.Parameters.Add("@NOM_CEP", SqlDbType.VarChar).Value = oRevendedor["address"]["postalCode"].ToString().Trim();
            cmdInsCons.Parameters.Add("@NOM_PAIS", SqlDbType.VarChar).Value = "BR";

            // Responsável
            cmdInsCons.Parameters.Add("@NOM_NOME", SqlDbType.VarChar).Value = oRevendedor["responsible"]["name"].ToString().Trim();
            cmdInsCons.Parameters.Add("@NOM_SOBRENOME", SqlDbType.VarChar).Value = oRevendedor["responsible"]["phone"].ToString().Trim();
            cmdInsCons.Parameters.Add("@NOM_CPF", SqlDbType.VarChar).Value = oRevendedor["responsible"]["cpf"].ToString().Trim();

            //if (txtNascimento.Text.ToString().Trim() != "")
            //{
            //    cmdInsCons.Parameters.Add("@DTA_ANIVERSARIO", SqlDbType.DateTime).Value = Convert.ToDateTime(txtNascimento.Text.ToString());
            //}

            //cmdInsCons.Parameters.Add("@NOM_MAE", SqlDbType.VarChar).Value = txtNomeMae.Text.ToString();
            //cmdInsCons.Parameters.Add("@NUM_RENDA_MENSAL", SqlDbType.Float).Value = Funcoes.strToDouble(txtRendaMensalPF.Text.ToString());
            cmdInsCons.Parameters.Add("@NOM_EMAIL", SqlDbType.VarChar).Value = oRevendedor["responsible"]["email"].ToString().Trim();
            cmdInsCons.Parameters.Add("@NOM_CELULAR", SqlDbType.VarChar).Value = oRevendedor["responsible"]["mobilePhone"].ToString().Trim();
            //cmdInsCons.Parameters.Add("@FLG_POLITICAMENTE", SqlDbType.Char).Value = ddlPoliticamenteExposta.SelectedValue.ToString();

            // Usuário
            cmdInsCons.Parameters.Add("@NOM_NOME_USUARIO", SqlDbType.VarChar).Value = oRevendedor["responsible"]["name"].ToString().Trim();
            cmdInsCons.Parameters.Add("@NOM_LOGIN", SqlDbType.VarChar).Value = oRevendedor["responsible"]["email"].ToString().Trim();
            cmdInsCons.Parameters.Add("@NOM_SENHA", SqlDbType.VarChar).Value = oRevendedor["reseller"]["document"].ToString().Trim();

            string sCodigo = cmdInsCons.ExecuteScalar().ToString();
            //cmdInsCons.ExecuteNonQuery();
            connInsCons.Close();
            connInsCons.Dispose();

            // Dados Onboarding Cappta
            SqlConnection connInsConsDadosCappta = new SqlConnection(Funcoes.conexao());
            connInsConsDadosCappta.Open();
            SqlCommand cmdInsConsDadosCappta = new SqlCommand("dbo.stp_pessoas_fj_cappta_ins", connInsConsDadosCappta);
            cmdInsConsDadosCappta.CommandType = CommandType.StoredProcedure;
            cmdInsConsDadosCappta.Parameters.Add("@flg_operacao", SqlDbType.Char).Value = 'I';
            cmdInsConsDadosCappta.Parameters.Add("@COD_ID_PESSOA_LICENCIADO", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["LICENCIADO"].ToString());
            cmdInsConsDadosCappta.Parameters.Add("@COD_ID_PESSOAS_FJ", SqlDbType.Int).Value = Funcoes.strToInt(sCodigo.ToString());

            cmdInsConsDadosCappta.Parameters.Add("@NUM_TOKEN_CAPPTA", SqlDbType.VarChar).Value = oRevendedor["reseller"]["document"].ToString();
            cmdInsConsDadosCappta.Parameters.Add("@FLG_STATUS_CAPPTA", SqlDbType.VarChar).Value = oRevendedor["statusDescription"].ToString();
            cmdInsConsDadosCappta.Parameters.Add("@FLG_CAPPTA", SqlDbType.Char).Value = "S";
            cmdInsConsDadosCappta.Parameters.Add("@DES_JSON_CAPPTA", SqlDbType.VarChar).Value = jsonRetornoCadastro.ToString();
            cmdInsConsDadosCappta.Parameters.Add("@COD_ID_NATUREZA_CAPPTA", SqlDbType.Int).Value = oRevendedor["reseller"]["legalNatureId"].ToString();

            cmdInsConsDadosCappta.ExecuteNonQuery();
            connInsConsDadosCappta.Close();
            connInsConsDadosCappta.Dispose();

            // Dados Bancários Cappta
            SqlConnection connInsConsBancoCappta = new SqlConnection(Funcoes.conexao());
            connInsConsBancoCappta.Open();
            SqlCommand cmdInsConsBancoCappta = new SqlCommand("dbo.stp_pessoas_fj_contas_ins", connInsConsBancoCappta);
            cmdInsConsBancoCappta.CommandType = CommandType.StoredProcedure;
            cmdInsConsBancoCappta.Parameters.Add("@flg_operacao", SqlDbType.Char).Value = 'I';
            cmdInsConsBancoCappta.Parameters.Add("@COD_ID_PESSOA_LICENCIADO", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["LICENCIADO"].ToString());
            cmdInsConsBancoCappta.Parameters.Add("@COD_ID_PESSOAS_FJ", SqlDbType.Int).Value = Funcoes.strToInt(sCodigo.ToString());

            cmdInsConsBancoCappta.Parameters.Add("@NOM_CODIGO_BANCO", SqlDbType.VarChar).Value = oRevendedor["bankAccount"]["bankCode"].ToString();
            cmdInsConsBancoCappta.Parameters.Add("@NOM_TIPO_BANCO", SqlDbType.VarChar).Value = (oRevendedor["bankAccount"]["accountType"].ToString().Trim() == "1") ? "C" : "P";
            cmdInsConsBancoCappta.Parameters.Add("@NOM_NUMERO_AGENCIA_BANCO", SqlDbType.VarChar).Value = oRevendedor["bankAccount"]["branch"].ToString();
            cmdInsConsBancoCappta.Parameters.Add("@NOM_NUMERO_DIGITO_AGENCIA_BANCO", SqlDbType.VarChar).Value = "";
            cmdInsConsBancoCappta.Parameters.Add("@NOM_NUMERO_CONTA_BANCO", SqlDbType.VarChar).Value = oRevendedor["bankAccount"]["account"].ToString();
            cmdInsConsBancoCappta.Parameters.Add("@NOM_NUMERO_DIGITO_CONTA_BANCO", SqlDbType.VarChar).Value = "";
            cmdInsConsBancoCappta.Parameters.Add("@FLG_PADRAO", SqlDbType.Char).Value = "S";

            cmdInsConsBancoCappta.ExecuteNonQuery();
            connInsConsBancoCappta.Close();
            connInsConsBancoCappta.Dispose();


            ClientScript.RegisterStartupScript(this.GetType(),
                "MensagemSucesso", "alert('Documento(s) gravados com sucesso');", true);

            // Gravar os dados
        }
        catch
        {

            // Exibir mensagem de alerta
            ClientScript.RegisterStartupScript(this.GetType(),
                "MensagemErro", "alert('Ocorreu um erro ao tentar importar o documento: " + sDocumento.ToString() + " - " + oRevendedor["errorMessage"].ToString() + "');", true);
        }


    }

    protected void ckbTodosCappta_CheckedChanged(object sender, EventArgs e)
    {
        foreach (ListViewItem itemP in lsvMarketplacesCappta.Items)
        {
            if (ckbTodosCappta.Checked == true)
            {
                ((CheckBox)itemP.FindControl("ckbImportar")).Checked = true;
            }
            else
            {
                ((CheckBox)itemP.FindControl("ckbImportar")).Checked = false;
            }
            //((CheckBox)itemP.FindControl("ckbImportar")).Checked = (((CheckBox)itemP.FindControl("ckbImportar")).Checked == false) ? true : false;
        }

    }

    protected void btnImportarDocumentoCappta_Click(object sender, EventArgs e)
    {
        if (txtDocumentoCappta.Text.ToString().Trim() != "")
        {
            GravarDadosMarketplace(txtDocumentoCappta.Text.ToString());
        }
        else
        {
            ClientScript.RegisterStartupScript(this.GetType(),
                "Alerta", "alert('Nenhum documento foi especificado! Verifique e reentre.');", true);

        }

    }

    private void GravarDadosEstabelecimentoCappta(string sLojista, string sDocumento)
    {
        string jsonRetornoCadastro = hubcappta.ConsultarLojista(sLojista.ToString(), sDocumento.ToString());

        JObject oEstabelecimento = JObject.Parse(jsonRetornoCadastro);

        //try
        //{
            // ********************************************************************
            // Localizar cadastro do Marketplace
            // ********************************************************************
            string sIDMarketplace = "0";
            SqlConnection mySelCadastro = new SqlConnection(Funcoes.conexao());
            mySelCadastro.Open();
            SqlCommand cmdSelCadastro = new SqlCommand("dbo.stp_pessoas_fj_ins", mySelCadastro);
            cmdSelCadastro.CommandType = CommandType.StoredProcedure;
            cmdSelCadastro.Parameters.Add("@flg_operacao", SqlDbType.Char).Value = "H";
            cmdSelCadastro.Parameters.Add("@NOM_CNPJ", SqlDbType.VarChar).Value = sDocumento.ToString();
            cmdSelCadastro.Parameters.Add("@COD_ID_PESSOA_LICENCIADO", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["LICENCIADO"].ToString());
            cmdSelCadastro.Parameters.Add("@FLG_TIPO", SqlDbType.Char).Value = "M";

            SqlDataReader ReaderCadastro = cmdSelCadastro.ExecuteReader();
            while (ReaderCadastro.Read())
            {
                sIDMarketplace = ReaderCadastro["COD_ID"].ToString();
            }

            SqlConnection connInsCons = new SqlConnection(Funcoes.conexao());
            connInsCons.Open();
            SqlCommand cmdInsCons = new SqlCommand("dbo.stp_pessoas_fj_ins", connInsCons);
            cmdInsCons.CommandType = CommandType.StoredProcedure;
            cmdInsCons.Parameters.Add("@flg_operacao", SqlDbType.Char).Value = 'I';
            cmdInsCons.Parameters.Add("@FLG_ATIVO", SqlDbType.Char).Value = "S";
            cmdInsCons.Parameters.Add("@FLG_INSERT_USUARIO", SqlDbType.Char).Value = "S";
            cmdInsCons.Parameters.Add("@COD_ID_PESSOA_LICENCIADO", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["LICENCIADO"].ToString());
            cmdInsCons.Parameters.Add("@DTA_DATA", SqlDbType.DateTime).Value = DateTime.Now;
            cmdInsCons.Parameters.Add("@FLG_TIPO", SqlDbType.Char).Value = "E";


            cmdInsCons.Parameters.Add("@FLG_TIPO_PESSOA", SqlDbType.VarChar).Value = (oEstabelecimento["merchant"]["document"].ToString().Trim().Length >= 14) ? "PJ" : "PF";
            cmdInsCons.Parameters.Add("@FLG_PRESENCIAL", SqlDbType.Char).Value = "N";
            //            cmdInsCons.Parameters.Add("@COD_ID_MARKETPLACE", SqlDbType.Int).Value = Funcoes.strToInt(ddlMarketplace.SelectedValue.ToString());

            cmdInsCons.Parameters.Add("@COD_ID_MARKETPLACE", SqlDbType.Int).Value = Funcoes.strToInt(sIDMarketplace.ToString());


            // Empresa
            cmdInsCons.Parameters.Add("@NOM_RAZAOSOCIAL", SqlDbType.VarChar).Value = (oEstabelecimento["merchant"]["document"].ToString().Trim().Length >= 14) ? oEstabelecimento["merchant"]["companyName"].ToString().Trim() : oEstabelecimento["owner"]["name"].ToString().Trim();
            cmdInsCons.Parameters.Add("@NOM_FANTASIA", SqlDbType.VarChar).Value = (oEstabelecimento["merchant"]["document"].ToString().Trim().Length >= 14) ? oEstabelecimento["merchant"]["tradingName"].ToString().Trim() : oEstabelecimento["owner"]["name"].ToString().Trim();
            cmdInsCons.Parameters.Add("@NOM_CNPJ", SqlDbType.VarChar).Value = oEstabelecimento["merchant"]["document"].ToString().Trim();

            cmdInsCons.Parameters.Add("@NUM_TELEFONE", SqlDbType.VarChar).Value = oEstabelecimento["owner"]["phone"].ToString().Trim();
            cmdInsCons.Parameters.Add("@NOM_EMAIL_EMPRESA", SqlDbType.VarChar).Value = oEstabelecimento["owner"]["email"].ToString().Trim();
            cmdInsCons.Parameters.Add("@COD_ID_MCC", SqlDbType.Int).Value = Funcoes.strToInt(oEstabelecimento["merchant"]["mccId"].ToString().Trim());
            //cmdInsCons.Parameters.Add("@NOM_TIPO_EMPRESA", SqlDbType.VarChar).Value = ddlTipoEmpresa.SelectedValue.ToString();
            cmdInsCons.Parameters.Add("@NUM_FATURAMENTO", SqlDbType.Float).Value = 0;
            cmdInsCons.Parameters.Add("@NUM_PATRIMONIO", SqlDbType.Float).Value = 0;

            //if (txtDataAbertura.Text.ToString().Trim() != "")
            //{
            //    cmdInsCons.Parameters.Add("@DTA_ABERTURA", SqlDbType.DateTime).Value = Convert.ToDateTime(txtDataAbertura.Text.ToString());
            //}

            // Endereço
            cmdInsCons.Parameters.Add("@NOM_ENDERECO", SqlDbType.VarChar).Value = oEstabelecimento["address"]["streetName"].ToString().Trim();
            cmdInsCons.Parameters.Add("@NOM_NUMERO", SqlDbType.VarChar).Value = oEstabelecimento["address"]["houseNumber"].ToString().Trim();
            cmdInsCons.Parameters.Add("@NOM_COMPLEMENTO", SqlDbType.VarChar).Value = oEstabelecimento["address"]["complement"].ToString().Trim();
            cmdInsCons.Parameters.Add("@NOM_BAIRRO", SqlDbType.VarChar).Value = oEstabelecimento["address"]["neighborhood"].ToString().Trim();
            cmdInsCons.Parameters.Add("@NOM_CIDADE", SqlDbType.VarChar).Value = oEstabelecimento["address"]["city"].ToString().Trim();
            cmdInsCons.Parameters.Add("@NOM_UF", SqlDbType.VarChar).Value = oEstabelecimento["address"]["state"].ToString().Trim();
            cmdInsCons.Parameters.Add("@NOM_CEP", SqlDbType.VarChar).Value = oEstabelecimento["address"]["postalCode"].ToString().Trim();
            cmdInsCons.Parameters.Add("@NOM_PAIS", SqlDbType.VarChar).Value = "BR";

            // Responsável
            cmdInsCons.Parameters.Add("@NOM_NOME", SqlDbType.VarChar).Value = oEstabelecimento["owner"]["name"].ToString().Trim();
            cmdInsCons.Parameters.Add("@NOM_SOBRENOME", SqlDbType.VarChar).Value = oEstabelecimento["owner"]["phone"].ToString().Trim();
            cmdInsCons.Parameters.Add("@NOM_CPF", SqlDbType.VarChar).Value = oEstabelecimento["owner"]["cpf"].ToString().Trim();

            //if (txtNascimento.Text.ToString().Trim() != "")
            //{
            //    cmdInsCons.Parameters.Add("@DTA_ANIVERSARIO", SqlDbType.DateTime).Value = Convert.ToDateTime(txtNascimento.Text.ToString());
            //}

            //cmdInsCons.Parameters.Add("@NOM_MAE", SqlDbType.VarChar).Value = txtNomeMae.Text.ToString();
            //cmdInsCons.Parameters.Add("@NUM_RENDA_MENSAL", SqlDbType.Float).Value = Funcoes.strToDouble(txtRendaMensalPF.Text.ToString());
            cmdInsCons.Parameters.Add("@NOM_EMAIL", SqlDbType.VarChar).Value = oEstabelecimento["owner"]["email"].ToString().Trim();
            cmdInsCons.Parameters.Add("@NOM_CELULAR", SqlDbType.VarChar).Value = oEstabelecimento["owner"]["mobilePhone"].ToString().Trim();
            //cmdInsCons.Parameters.Add("@FLG_POLITICAMENTE", SqlDbType.Char).Value = ddlPoliticamenteExposta.SelectedValue.ToString();

            // Usuário
            cmdInsCons.Parameters.Add("@NOM_NOME_USUARIO", SqlDbType.VarChar).Value = oEstabelecimento["owner"]["name"].ToString().Trim();
            cmdInsCons.Parameters.Add("@NOM_LOGIN", SqlDbType.VarChar).Value = oEstabelecimento["owner"]["email"].ToString().Trim();
            cmdInsCons.Parameters.Add("@NOM_SENHA", SqlDbType.VarChar).Value = oEstabelecimento["merchant"]["document"].ToString().Trim();

            string sCodigo = cmdInsCons.ExecuteScalar().ToString();



            //cmdInsCons.ExecuteNonQuery();
            connInsCons.Close();
            connInsCons.Dispose();

            // Dados Onboarding Cappta
            SqlConnection connInsConsDadosCappta = new SqlConnection(Funcoes.conexao());
            connInsConsDadosCappta.Open();
            SqlCommand cmdInsConsDadosCappta = new SqlCommand("dbo.stp_pessoas_fj_cappta_ins", connInsConsDadosCappta);
            cmdInsConsDadosCappta.CommandType = CommandType.StoredProcedure;
            cmdInsConsDadosCappta.Parameters.Add("@flg_operacao", SqlDbType.Char).Value = 'I';
            cmdInsConsDadosCappta.Parameters.Add("@COD_ID_PESSOA_LICENCIADO", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["LICENCIADO"].ToString());
            cmdInsConsDadosCappta.Parameters.Add("@COD_ID_PESSOAS_FJ", SqlDbType.Int).Value = Funcoes.strToInt(sCodigo.ToString());

            cmdInsConsDadosCappta.Parameters.Add("@NUM_TOKEN_CAPPTA", SqlDbType.VarChar).Value = oEstabelecimento["merchant"]["document"].ToString();
            cmdInsConsDadosCappta.Parameters.Add("@FLG_STATUS_CAPPTA", SqlDbType.VarChar).Value = oEstabelecimento["statusDescription"].ToString();
            cmdInsConsDadosCappta.Parameters.Add("@FLG_CAPPTA", SqlDbType.Char).Value = "S";
            cmdInsConsDadosCappta.Parameters.Add("@DES_JSON_CAPPTA", SqlDbType.VarChar).Value = jsonRetornoCadastro.ToString();
            cmdInsConsDadosCappta.Parameters.Add("@COD_ID_NATUREZA_CAPPTA", SqlDbType.Int).Value = Funcoes.strToInt(oEstabelecimento["merchant"]["legalNatureId"].ToString());

            // Localizar plano
            try
            {
                string sIDPlano = LocalizaPlano(oEstabelecimento["plans"][0]["id"].ToString(), sDocumento.ToString());
                
                cmdInsConsDadosCappta.Parameters.Add("@COD_ID_PLANO", SqlDbType.Int).Value = Funcoes.strToInt(sIDPlano);
                cmdInsConsDadosCappta.Parameters.Add("@COD_ID_PLANO_CAPPTA", SqlDbType.Int).Value = Funcoes.strToInt(oEstabelecimento["plans"][0]["id"].ToString());
                // Lista plano para pegar plano base
                try
                {
                    string jsonPlanoBase = hubcappta.ConsultarPlano(oEstabelecimento["plans"][0]["id"].ToString());
                    if (jsonPlanoBase.ToString().Trim() != "")
                    {
                        JObject oPlanoBase = JObject.Parse(jsonPlanoBase);
                        cmdInsConsDadosCappta.Parameters.Add("@COD_ID_PLANO_BASE_CAPPTA", SqlDbType.Int).Value = Funcoes.strToInt(oPlanoBase["basePlanId"].ToString());
                    }
                }
                catch
                {

                }
            }
            catch
            {

            }


            //txtPlanos.Text = txtPlanos.Text + "C1: " + oEstabelecimento["merchant"]["document"].ToString() + "     -----     ";
            //txtPlanos.Text = txtPlanos.Text + "C2: " + oEstabelecimento["statusDescription"].ToString() + "     -----     ";
            //txtPlanos.Text = txtPlanos.Text + "C3: " + oEstabelecimento["merchant"]["legalNatureId"].ToString() + "     -----     ";
            //txtPlanos.Text = txtPlanos.Text + "PLANO: " + sIDPlano + "     -----     ";
            //txtPlanos.Text = txtPlanos.Text + "EC: " + sCodigo + "     -----     ";
            //txtPlanos.Text = txtPlanos.Text + "LICENCIADO: " + HttpContext.Current.Session["LICENCIADO"].ToString() + "     -----     ";


            cmdInsConsDadosCappta.ExecuteNonQuery();
            connInsConsDadosCappta.Close();
            connInsConsDadosCappta.Dispose();

            try
            {
                // Dados Bancários Cappta
                SqlConnection connInsConsBancoCappta = new SqlConnection(Funcoes.conexao());
                connInsConsBancoCappta.Open();
                SqlCommand cmdInsConsBancoCappta = new SqlCommand("dbo.stp_pessoas_fj_contas_ins", connInsConsBancoCappta);
                cmdInsConsBancoCappta.CommandType = CommandType.StoredProcedure;
                cmdInsConsBancoCappta.Parameters.Add("@flg_operacao", SqlDbType.Char).Value = 'I';
                cmdInsConsBancoCappta.Parameters.Add("@COD_ID_PESSOA_LICENCIADO", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["LICENCIADO"].ToString());
                cmdInsConsBancoCappta.Parameters.Add("@COD_ID_PESSOAS_FJ", SqlDbType.Int).Value = Funcoes.strToInt(sCodigo.ToString());

                cmdInsConsBancoCappta.Parameters.Add("@NOM_CODIGO_BANCO", SqlDbType.VarChar).Value = oEstabelecimento["bankAccount"]["bankCode"].ToString();
                cmdInsConsBancoCappta.Parameters.Add("@NOM_TIPO_BANCO", SqlDbType.VarChar).Value = (oEstabelecimento["bankAccount"]["accountType"].ToString().Trim() == "1") ? "C" : "P";
                cmdInsConsBancoCappta.Parameters.Add("@NOM_NUMERO_AGENCIA_BANCO", SqlDbType.VarChar).Value = oEstabelecimento["bankAccount"]["branch"].ToString();
                cmdInsConsBancoCappta.Parameters.Add("@NOM_NUMERO_DIGITO_AGENCIA_BANCO", SqlDbType.VarChar).Value = "";
                cmdInsConsBancoCappta.Parameters.Add("@NOM_NUMERO_CONTA_BANCO", SqlDbType.VarChar).Value = oEstabelecimento["bankAccount"]["account"].ToString();
                cmdInsConsBancoCappta.Parameters.Add("@NOM_NUMERO_DIGITO_CONTA_BANCO", SqlDbType.VarChar).Value = "";
                cmdInsConsBancoCappta.Parameters.Add("@FLG_PADRAO", SqlDbType.Char).Value = "S";

                cmdInsConsBancoCappta.ExecuteNonQuery();
                connInsConsBancoCappta.Close();
                connInsConsBancoCappta.Dispose();
            }
            catch
            {

            }

            ClientScript.RegisterStartupScript(this.GetType(),
                "MensagemSucesso", "alert('Documento(s) gravados com sucesso');", true);

            // Gravar os dados
        //}
        //catch
        //{
            //Exibir mensagem de alerta
        //    ClientScript.RegisterStartupScript(this.GetType(), "MensagemErro", "alert('Ocorreu um erro ao tentar importar o Estabelecimento: " + sLojista.ToString() + "');", true);
        //}


    }


    public static string LocalizaPlano(string sPlano, string sMarketplace)
    {
        string sIDMarketplace = "0";
        
        SqlConnection mySelCadastroMkt = new SqlConnection(Funcoes.conexao());
        mySelCadastroMkt.Open();
        SqlCommand cmdSelCadastroMkt = new SqlCommand("dbo.stp_pessoas_fj_ins", mySelCadastroMkt);
        cmdSelCadastroMkt.CommandType = CommandType.StoredProcedure;
        cmdSelCadastroMkt.Parameters.Add("@flg_operacao", SqlDbType.Char).Value = "H";
        cmdSelCadastroMkt.Parameters.Add("@NOM_CNPJ", SqlDbType.VarChar).Value = sMarketplace.ToString();
        cmdSelCadastroMkt.Parameters.Add("@COD_ID_PESSOA_LICENCIADO", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["LICENCIADO"].ToString());
        cmdSelCadastroMkt.Parameters.Add("@FLG_TIPO", SqlDbType.Char).Value = "M";

        SqlDataReader ReaderCadastroMkt = cmdSelCadastroMkt.ExecuteReader();
        while (ReaderCadastroMkt.Read())
        {
            sIDMarketplace = ReaderCadastroMkt["COD_ID"].ToString();
        }

        string sID = "";

        SqlConnection mySelCadastro = new SqlConnection(Funcoes.conexao());
        mySelCadastro.Open();
        SqlCommand cmdSelCadastro = new SqlCommand("dbo.stp_planos_ins", mySelCadastro);
        cmdSelCadastro.CommandType = CommandType.StoredProcedure;
        cmdSelCadastro.Parameters.Add("@flg_operacao", SqlDbType.Char).Value = "H";
        cmdSelCadastro.Parameters.Add("@NOM_REFERENCIA_PLANO", SqlDbType.VarChar).Value = sPlano.ToString();
        cmdSelCadastro.Parameters.Add("@FLG_MODELO_PLANO", SqlDbType.Char).Value = "C";
        cmdSelCadastro.Parameters.Add("@COD_ID_PESSOA_LICENCIADO", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["LICENCIADO"].ToString());
        cmdSelCadastro.Parameters.Add("@COD_ID_PESSOAS_FJ", SqlDbType.Int).Value = Funcoes.strToInt(sIDMarketplace);

        SqlDataReader ReaderCadastro = cmdSelCadastro.ExecuteReader();
        while (ReaderCadastro.Read())
        {
            sID = ReaderCadastro["COD_ID"].ToString();
        }
        
        return sID;
    }

}