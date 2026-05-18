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


public partial class cad_planos_referencia_importar : System.Web.UI.Page
{
    public static DataTable dtPlanosReferencia;

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
            // Definição Tabela de Modelos
            dtPlanosReferencia = new DataTable();
            dtPlanosReferencia.Columns.Add("codigo", typeof(string));
            dtPlanosReferencia.Columns.Add("nome", typeof(string));

            ConsultaIntegracoes();
        }

    }

    private void ConsultaIntegracoes()
    {
        SqlConnection mySelCadastro = new SqlConnection(Funcoes.conexao());
        mySelCadastro.Open();
        SqlCommand cmdSelCadastro = new SqlCommand("dbo.stp_pessoas_fj_integracoes_ins", mySelCadastro);
        cmdSelCadastro.CommandType = CommandType.StoredProcedure;
        cmdSelCadastro.Parameters.Add("@flg_operacao", SqlDbType.Char).Value = "V";
        cmdSelCadastro.Parameters.Add("@COD_ID_PESSOA_LICENCIADO", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["LICENCIADO"].ToString());
        SqlDataReader ReaderCadastro = cmdSelCadastro.ExecuteReader();
        int iContador = 0;
        while (ReaderCadastro.Read())
        {
            if ((ReaderCadastro["NOM_INTEGRACAO"].ToString().ToUpper().Trim() == "CAPPTA") && (ReaderCadastro["FLG_ACESSO"].ToString().ToUpper() == "N")) { tbcapptatab.Visible = false; tbcappta.Visible = false; }
            if ((ReaderCadastro["NOM_INTEGRACAO"].ToString().ToUpper().Trim() == "ZOOP") && (ReaderCadastro["FLG_ACESSO"].ToString().ToUpper() == "N")) { tbzooptab.Visible = false; tbzoop.Visible = false; }
            if ((ReaderCadastro["NOM_INTEGRACAO"].ToString().ToUpper().Trim() == "ASAAS") && (ReaderCadastro["FLG_ACESSO"].ToString().ToUpper() == "N")) { tbasaastab.Visible = false; tbasaas.Visible = false; }
            if ((ReaderCadastro["NOM_INTEGRACAO"].ToString().ToUpper().Trim() == "PAGSEGURO") && (ReaderCadastro["FLG_ACESSO"].ToString().ToUpper() == "N")) { tbpagsegurotab.Visible = false; tbpagseguro.Visible = false; }
        }

        if ((tbcapptatab.Visible == true) && (iContador == 0)) { tbcapptatab.Attributes.Add("class", "nav-link active"); tbcappta.Attributes.Add("class", "tab-pane fade show active"); iContador = iContador + 1; }
        if ((tbzooptab.Visible == true) && (iContador == 0)) { tbzooptab.Attributes.Add("class", "nav-link active"); tbzoop.Attributes.Add("class", "tab-pane fade show active"); iContador = iContador + 1; }
        if ((tbasaastab.Visible == true) && (iContador == 0)) { tbasaastab.Attributes.Add("class", "nav-link active"); tbasaas.Attributes.Add("class", "tab-pane fade show active"); iContador = iContador + 1; }
        if ((tbpagsegurotab.Visible == true) && (iContador == 0)) { tbpagsegurotab.Attributes.Add("nav-link class", "active"); tbpagseguro.Attributes.Add("class", "tab-pane fade show active"); iContador = iContador + 1; }

    }
    protected void btnCarregarCadastrosCappta_Click(object sender, EventArgs e)
    {
        dtPlanosReferencia.Rows.Clear();

        string JsonOpcoesPlanos = hubcappta.ListarOpcoesPlanos();

        string jsonPlanosReferencia = "";
        jsonPlanosReferencia = hubcappta.ConsultarPlanos(1,"true","Reseller");
        txtJsonCappta.Text = jsonPlanosReferencia;
        //txtJsonCappta.Visible = true;

        JObject oPlanosReferencia = JObject.Parse(jsonPlanosReferencia);
        if (Funcoes.strToInt(oPlanosReferencia["lastPage"].ToString()) > 0)
        {
            for (int i = 0; i < Funcoes.strToInt(oPlanosReferencia["lastPage"].ToString()); i++)
            {
                jsonPlanosReferencia = "";
                jsonPlanosReferencia = hubcappta.ConsultarPlanos(i, "true", "Reseller");
                oPlanosReferencia = JObject.Parse(jsonPlanosReferencia);

                if (oPlanosReferencia["plans"].Count() > 0)
                {
                    for (int x = 0; x < oPlanosReferencia["plans"].Count(); x++)
                    {
                        dtPlanosReferencia.Rows.Add(oPlanosReferencia["plans"][x]["id"].ToString().Trim(), oPlanosReferencia["plans"][x]["name"].ToString().Trim());
                    }
                }
            
            }
        }
        this.lsvPlanosReferencia.DataSource = dtPlanosReferencia;
        this.lsvPlanosReferencia.DataBind();

        ClientScript.RegisterStartupScript(this.GetType(),
"SucessoCarregarDados", "alert('Dados dos planos de referência carregados com sucesso');", true);
    }
    protected void btnImportarCappta_Click(object sender, EventArgs e)
    {
        foreach (ListViewItem itemP in lsvPlanosReferencia.Items)
        {
            if (((CheckBox)itemP.FindControl("ckbImportar")).Checked == true)
            {
                GravarDadosPlanoReferencia(((TextBox)itemP.FindControl("txtCodigo")).Text.ToString(), ((TextBox)itemP.FindControl("txtNome")).Text.ToString());
            }
        }

        ClientScript.RegisterStartupScript(this.GetType(),
"SucessoImportarDados", "alert('Dados dos planos de referência importados com sucesso');", true);


    }
    protected void ckbTodos_CheckedChanged(object sender, EventArgs e)
    {
        foreach (ListViewItem itemP in lsvPlanosReferencia.Items)
        {
            ((CheckBox)itemP.FindControl("ckbImportar")).Checked = (((CheckBox)itemP.FindControl("ckbImportar")).Checked == false) ? true : false;
        }

    }

    protected void btnCarregarCadastrosPagseguro_Click(object sender, EventArgs e)
    {

    }
    protected void btnImportarPagseguro_Click(object sender, EventArgs e)
    {

    }
    protected void btnCarregarCadastrosAsaas_Click(object sender, EventArgs e)
    {

    }
    protected void btnImportarAsaas_Click(object sender, EventArgs e)
    {

    }
    protected void btnCarregarCadastrosZoop_Click(object sender, EventArgs e)
    {
        if (zoop.LicenciadoIntegracaoZoop(HttpContext.Current.Session["LICENCIADO"].ToString()) == "S")
        {
            dtPlanosReferencia.Rows.Clear();
            JObject oPlanosReferencia = JObject.Parse(zoop.planos_referencia("").ToString());

            for (int i = 0; i < oPlanosReferencia["items"].Count(); i++)
            {
                dtPlanosReferencia.Rows.Add(oPlanosReferencia["items"][i]["id"].ToString().Trim(), oPlanosReferencia["items"][i]["name"].ToString().Trim());
            }

            this.lsvPlanosReferenciaZoop.DataSource = dtPlanosReferencia;
            this.lsvPlanosReferenciaZoop.DataBind();

            ClientScript.RegisterStartupScript(this.GetType(), "SucessoCarregarDados", "alert('Dados dos planos de referência carregados com sucesso');", true);
        }
        else
        {
            ClientScript.RegisterStartupScript(this.GetType(), "ErroCarregarDados", "alert('O Licenciado não possui planos de referência nesta adquirente');", true);
        }
    }
    protected void btnImportarZoop_Click(object sender, EventArgs e)
    {
        foreach (ListViewItem itemP in lsvPlanosReferenciaZoop.Items)
        {
            if (((CheckBox)itemP.FindControl("ckbImportar")).Checked == true)
            {
                GravarDadosPlanoReferenciaZoop(((TextBox)itemP.FindControl("txtCodigo")).Text.ToString(), ((TextBox)itemP.FindControl("txtNome")).Text.ToString());
            }
        }

    }
    protected void lsvPlanosReferencia_ItemCommand(object sender, ListViewCommandEventArgs e)
    {

    }

    private void GravarDadosPlanoReferencia(string sCodigo, string sNome)
    {
        SqlConnection connInsCons = new SqlConnection(Funcoes.conexao());
        connInsCons.Open();
        SqlCommand cmdInsCons = new SqlCommand("dbo.stp_planos_referencia_ins", connInsCons);
        cmdInsCons.CommandType = CommandType.StoredProcedure;
        cmdInsCons.Parameters.Add("@flg_operacao", SqlDbType.Char).Value = 'I';

        cmdInsCons.Parameters.Add("@COD_ID_PESSOA_LICENCIADO", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["LICENCIADO"].ToString());
        cmdInsCons.Parameters.Add("@FLG_MODELO_PLANO_REFERENCIA", SqlDbType.Char).Value = "C";
        cmdInsCons.Parameters.Add("@FLG_ATIVO", SqlDbType.Char).Value = "S";
        cmdInsCons.Parameters.Add("@COD_ID_PLANO_REFERENCIA", SqlDbType.VarChar).Value = sCodigo;
        cmdInsCons.Parameters.Add("@NOM_PLANO_REFERENCIA", SqlDbType.VarChar).Value = sNome.ToString();
        cmdInsCons.Parameters.Add("@NOM_INVOICE_PLANO_REFERENCIA", SqlDbType.VarChar).Value = sNome.ToString();
        cmdInsCons.Parameters.Add("@NOM_PERIODICIDADE", SqlDbType.VarChar).Value = "Por transação";
        cmdInsCons.Parameters.Add("@DES_PLANO_REFERENCIA", SqlDbType.Text).Value = sNome.ToString();
        string sCodigoInclusao = cmdInsCons.ExecuteScalar().ToString();
        connInsCons.Close();
        connInsCons.Dispose();

        // Importar as Taxas
        string jsonPlanosReferencia = "";

        jsonPlanosReferencia = hubcappta.ConsultarPlanos(1, "true", "Reseller");
        txtJsonCappta.Text = jsonPlanosReferencia;
        //txtJsonCappta.Visible = true;

        JObject oPlanosReferencia = JObject.Parse(jsonPlanosReferencia);

        if (Funcoes.strToInt(oPlanosReferencia["lastPage"].ToString()) > 0)
        {
            for (int i = 0; i < Funcoes.strToInt(oPlanosReferencia["lastPage"].ToString()); i++)
            {
                jsonPlanosReferencia = "";
                jsonPlanosReferencia = hubcappta.ConsultarPlanos(i, "true", "Reseller");
                oPlanosReferencia = JObject.Parse(jsonPlanosReferencia);

                if (oPlanosReferencia["plans"].Count() > 0)
                {
                    for (int x = 0; x < oPlanosReferencia["plans"].Count(); x++)
                    {
                        if (oPlanosReferencia["plans"][x]["id"].ToString().Trim() == sCodigo.ToString().Trim())
                        {
                            // Inserir na base PLANOS_REFERENCIA_TAXAS

                            for (int t = 0; t < oPlanosReferencia["plans"][x]["schemes"].Count(); t++)
                            {

                                for (int p = 0; p < oPlanosReferencia["plans"][x]["schemes"][t]["fees"].Count(); p++)
                                {

                                    SqlConnection connInsConsTaxas = new SqlConnection(Funcoes.conexao());
                                    connInsConsTaxas.Open();
                                    SqlCommand cmdInsConsTaxas = new SqlCommand("dbo.stp_planos_referencia_taxas_ins", connInsConsTaxas);
                                    cmdInsConsTaxas.CommandType = CommandType.StoredProcedure;
                                    cmdInsConsTaxas.Parameters.Add("@flg_operacao", SqlDbType.Char).Value = 'I';
                                    cmdInsConsTaxas.Parameters.Add("@COD_ID_PESSOA_LICENCIADO", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["LICENCIADO"].ToString());
                                    cmdInsConsTaxas.Parameters.Add("@COD_ID_PLANOS_REFERENCIA", SqlDbType.Int).Value = Funcoes.strToInt(sCodigoInclusao);

                                    int iBandeira = 0;
                                    string sBandeira = "";
                                    string sTipo = "";
                                    // Busca Bandeira Tabela OPÇÕES PLANOS

                                    SqlConnection mySelCadastro = new SqlConnection(Funcoes.conexao());
                                    mySelCadastro.Open();
                                    SqlCommand cmdSelCadastro = new SqlCommand("dbo.stp_opcoes_planos_cappta_ins", mySelCadastro);
                                    cmdSelCadastro.CommandType = CommandType.StoredProcedure;
                                    cmdSelCadastro.Parameters.Add("@flg_operacao", SqlDbType.Char).Value = "B";
                                    cmdSelCadastro.Parameters.Add("@NOM_REFERENCIA", SqlDbType.VarChar).Value = oPlanosReferencia["plans"][x]["schemes"][t]["scheme"].ToString();
                                    SqlDataReader ReaderCadastro = cmdSelCadastro.ExecuteReader();
                                    while (ReaderCadastro.Read())
                                    {
                                        iBandeira = Funcoes.strToInt(ReaderCadastro["COD_BANDEIRA"].ToString());
                                        sBandeira = ReaderCadastro["NOM_BANDEIRA"].ToString();
                                        sTipo = ReaderCadastro["NOM_TIPO"].ToString();
                                    }

                                    cmdInsConsTaxas.Parameters.Add("@COD_ID_BANDEIRAS", SqlDbType.Int).Value = iBandeira;
                                    cmdInsConsTaxas.Parameters.Add("@NOM_BANDEIRA", SqlDbType.VarChar).Value = sBandeira;
                                    cmdInsConsTaxas.Parameters.Add("@NUM_VALOR_PERCENTUAL", SqlDbType.Float).Value = Funcoes.strToDouble(oPlanosReferencia["plans"][x]["schemes"][t]["fees"][p]["rate"].ToString())/100;
                                    cmdInsConsTaxas.Parameters.Add("@NOM_MOEDA", SqlDbType.VarChar).Value = "BRL";
                                    
                                    cmdInsConsTaxas.Parameters.Add("@NOM_TIPO_PAGAMENTO", SqlDbType.VarChar).Value = sTipo.ToString();

                                    cmdInsConsTaxas.Parameters.Add("@NUM_PARCELAS", SqlDbType.Int).Value = Funcoes.strToInt(oPlanosReferencia["plans"][x]["schemes"][t]["fees"][p]["installments"].ToString());
                                    cmdInsConsTaxas.Parameters.Add("@NOM_MODO_CAPTURA", SqlDbType.VarChar).Value = "";
                                    cmdInsConsTaxas.Parameters.Add("@NOM_ORIGEM", SqlDbType.VarChar).Value = "reseller";
                                    cmdInsConsTaxas.Parameters.Add("@FLG_PRE_PAGO", SqlDbType.Char).Value = "N";
                                    cmdInsConsTaxas.Parameters.Add("@NOM_TIPO", SqlDbType.VarChar).Value = oPlanosReferencia["plans"][x]["schemes"][t]["scheme"].ToString() + "_" + sBandeira.ToString() + "_" + sTipo.ToString() + "_" +oPlanosReferencia["plans"][x]["schemes"][t]["fees"][p]["installments"].ToString();
                                    cmdInsConsTaxas.ExecuteNonQuery();
                                    connInsConsTaxas.Close();
                                    connInsConsTaxas.Dispose();
                                }
                            }
                        }
                    }
                }

            }
        }



    }

    private void GravarDadosPlanoReferenciaZoop(string sCodigo, string sNome)
    {
        SqlConnection connInsCons = new SqlConnection(Funcoes.conexao());
        connInsCons.Open();
        SqlCommand cmdInsCons = new SqlCommand("dbo.stp_planos_referencia_ins", connInsCons);
        cmdInsCons.CommandType = CommandType.StoredProcedure;
        cmdInsCons.Parameters.Add("@flg_operacao", SqlDbType.Char).Value = 'I';

        cmdInsCons.Parameters.Add("@COD_ID_PESSOA_LICENCIADO", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["LICENCIADO"].ToString());
        cmdInsCons.Parameters.Add("@FLG_MODELO_PLANO_REFERENCIA", SqlDbType.Char).Value = "Z";
        cmdInsCons.Parameters.Add("@FLG_ATIVO", SqlDbType.Char).Value = "S";
        cmdInsCons.Parameters.Add("@COD_ID_PLANO_REFERENCIA", SqlDbType.VarChar).Value = sCodigo;
        cmdInsCons.Parameters.Add("@NOM_PLANO_REFERENCIA", SqlDbType.VarChar).Value = sNome.ToString();
        cmdInsCons.Parameters.Add("@NOM_INVOICE_PLANO_REFERENCIA", SqlDbType.VarChar).Value = sNome.ToString();
        cmdInsCons.Parameters.Add("@NOM_PERIODICIDADE", SqlDbType.VarChar).Value = "Por transação";
        cmdInsCons.Parameters.Add("@DES_PLANO_REFERENCIA", SqlDbType.Text).Value = sNome.ToString();
        string sCodigoInclusao = cmdInsCons.ExecuteScalar().ToString();
        connInsCons.Close();
        connInsCons.Dispose();

        JObject o = JObject.Parse(zoop.planos_referencia(sCodigo.ToString().Trim()));
        for (int i = 0; i < o["fee_details"].Count(); i++)
        {
            SqlConnection connInsConsTaxas = new SqlConnection(Funcoes.conexao());
            connInsConsTaxas.Open();
            SqlCommand cmdInsConsTaxas = new SqlCommand("dbo.stp_planos_referencia_taxas_ins", connInsConsTaxas);
            cmdInsConsTaxas.CommandType = CommandType.StoredProcedure;
            cmdInsConsTaxas.Parameters.Add("@flg_operacao", SqlDbType.Char).Value = 'I';
            cmdInsConsTaxas.Parameters.Add("@COD_ID_PESSOA_LICENCIADO", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["LICENCIADO"].ToString());
            cmdInsConsTaxas.Parameters.Add("@COD_ID_PLANOS_REFERENCIA", SqlDbType.Int).Value = Funcoes.strToInt(sCodigoInclusao);
            cmdInsConsTaxas.Parameters.Add("@NOM_BANDEIRA", SqlDbType.VarChar).Value = o["fee_details"][i]["card_brand"].ToString();
            cmdInsConsTaxas.Parameters.Add("@NUM_VALOR_PERCENTUAL", SqlDbType.Float).Value = Funcoes.strToDouble(o["fee_details"][i]["percent_amount"].ToString()) / 100;
            cmdInsConsTaxas.Parameters.Add("@NOM_MOEDA", SqlDbType.VarChar).Value = o["fee_details"][i]["currency"].ToString();
            cmdInsConsTaxas.Parameters.Add("@NOM_TIPO_PAGAMENTO", SqlDbType.VarChar).Value = o["fee_details"][i]["payment_type"].ToString();
            cmdInsConsTaxas.Parameters.Add("@NUM_PARCELAS", SqlDbType.Int).Value = Funcoes.strToInt(o["fee_details"][i]["number_installments"].ToString());
            cmdInsConsTaxas.Parameters.Add("@NOM_MODO_CAPTURA", SqlDbType.VarChar).Value = o["fee_details"][i]["capture_mode"].ToString();
            cmdInsConsTaxas.Parameters.Add("@NOM_ORIGEM", SqlDbType.VarChar).Value = o["fee_details"][i]["source"].ToString();
            cmdInsConsTaxas.Parameters.Add("@FLG_PRE_PAGO", SqlDbType.Char).Value = (o["fee_details"][i]["prepaid"].ToString() == "True") ? "S" : "N";
            cmdInsConsTaxas.Parameters.Add("@NOM_TIPO", SqlDbType.VarChar).Value = o["fee_details"][i]["type"].ToString();
            cmdInsConsTaxas.ExecuteNonQuery();
            connInsConsTaxas.Close();
            connInsConsTaxas.Dispose();
        }
    }

    protected void btnFechar_Click(object sender, EventArgs e)
    {
        ClientScript.RegisterStartupScript(this.GetType(),
    "Alerta", "opener.PostBackOnMainPage(); window.close(); ", true);

    }
    protected void ckbTodosZoop_CheckedChanged(object sender, EventArgs e)
    {
        foreach (ListViewItem itemP in lsvPlanosReferenciaZoop.Items)
        {
            ((CheckBox)itemP.FindControl("ckbImportar")).Checked = (((CheckBox)itemP.FindControl("ckbImportar")).Checked == false) ? true : false;
        }

    }
    protected void lsvPlanosReferenciaZoop_ItemCommand(object sender, ListViewCommandEventArgs e)
    {

    }
}