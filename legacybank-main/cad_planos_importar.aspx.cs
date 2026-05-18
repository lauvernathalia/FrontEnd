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




public partial class cad_planos_importar : System.Web.UI.Page
{
    public static DataTable dtPlanos;

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
            CarregaAdquirentes();

            // Definição Tabela de Modelos
            dtPlanos = new DataTable();
            dtPlanos.Columns.Add("id", typeof(string));
            dtPlanos.Columns.Add("base", typeof(string));
            dtPlanos.Columns.Add("plano", typeof(string));
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

    
    protected void ckbTodos_CheckedChanged(object sender, EventArgs e)
    {
        foreach (ListViewItem itemP in lsvPlanos.Items)
        {
            ((CheckBox)itemP.FindControl("ckbImportar")).Checked = (((CheckBox)itemP.FindControl("ckbImportar")).Checked == false) ? true : false;
        }

    }

    protected void lsvPlanos_ItemCommand(object sender, ListViewCommandEventArgs e)
    {

    }

    protected void btnFechar_Click(object sender, EventArgs e)
    {
        ClientScript.RegisterStartupScript(this.GetType(),
    "Alerta", "opener.PostBackOnMainPage(); window.close(); ", true);

    }

    public double ValorPercentualPresencialAntecipado(int idPlanoReferencia, int idBandeira, string sTipo, string sCaptura, int idParcela, int idParc)
    {
        double fValor = 0;

        SqlConnection mySelCadastro = new SqlConnection(Funcoes.conexao());
        mySelCadastro.Open();
        SqlCommand cmdSelCadastro = new SqlCommand("dbo.stp_planos_referencia_taxas_ins", mySelCadastro);
        cmdSelCadastro.CommandType = CommandType.StoredProcedure;
        cmdSelCadastro.Parameters.Add("@flg_operacao", SqlDbType.Char).Value = "P";
        cmdSelCadastro.Parameters.Add("@FLG_ANTECIPADO", SqlDbType.Char).Value = "S";
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

    protected void btnCarregarCadastros_Click(object sender, EventArgs e)
    {

        dtPlanos.Rows.Clear();
        if (ddlAdquirentes.SelectedValue.ToString() == "Z")
        {

            JObject oPlano = JObject.Parse(zoop.planos_referencia("").ToString());

            for (int i = 0; i < oPlano["items"].Count(); i++)
            {
                dtPlanos.Rows.Add(oPlano["items"][i]["id"].ToString().Trim(), oPlano["items"][i]["id"].ToString().Trim(), oPlano["items"][i]["name"].ToString().Trim());
            }

            this.lsvPlanos.DataSource = dtPlanos;
            this.lsvPlanos.DataBind();

            ClientScript.RegisterStartupScript(this.GetType(), "SucessoCarregarDadosZoop", "alert('Planos carregados com sucesso');", true);

        }


        if (ddlAdquirentes.SelectedValue.ToString() == "A")
        {
            string jsonResposta = asaas.ListarTaxas(asaas.PegarTokenSubconta(Funcoes.strToInt(HttpContext.Current.Session["pessoa"].ToString()), Funcoes.strToInt(HttpContext.Current.Session["LICENCIADO"].ToString()))).ToString();
            //JObject oPlano = JObject.Parse(jsonResposta);
            //txtJson.Text = jsonResposta.ToString();
            //txtJson.Visible = true;
            
            //for (int i = 0; i < oPlano["items"].Count(); i++)
            //{
                dtPlanos.Rows.Add("01", "ÚNICO", "CONTA DIGITAL");
            //}

            this.lsvPlanos.DataSource = dtPlanos;
            this.lsvPlanos.DataBind();

            ClientScript.RegisterStartupScript(this.GetType(), "SucessoCarregarDadosAsaaS", "alert('Planos carregados com sucesso');", true);
        }

        if (ddlAdquirentes.SelectedValue.ToString() == "C")
        {

            if (ddlAdquirentes.SelectedValue.ToString().Trim() == "C")
            {
                dtPlanos.Rows.Clear();
                JObject oPlanosReferencia = JObject.Parse(hubcappta.ConsultarPlanos(1, "true", "Merchant"));
                
                txtJson.Visible = true;
                txtJson.Text = hubcappta.ConsultarPlanos(1, "true", "Partner");

                if (Funcoes.strToInt(oPlanosReferencia["lastPage"].ToString()) > 0)
                {
                    for (int i = 0; i < Funcoes.strToInt(oPlanosReferencia["lastPage"].ToString()); i++)
                    {
                        oPlanosReferencia = JObject.Parse(hubcappta.ConsultarPlanos(i, "true", "Merchant"));
                        if (oPlanosReferencia["plans"].Count() > 0)
                        {
                            for (int x = 0; x < oPlanosReferencia["plans"].Count(); x++)
                            {
                                dtPlanos.Rows.Add(oPlanosReferencia["plans"][x]["id"].ToString().Trim(), oPlanosReferencia["plans"][x]["basePlanId"].ToString().Trim(), oPlanosReferencia["plans"][x]["name"].ToString().Trim());
                            }
                        }

                    }
                }
                this.lsvPlanos.DataSource = dtPlanos;
                this.lsvPlanos.DataBind();

                ClientScript.RegisterStartupScript(this.GetType(), "SucessoCarregarDadosCappta", "alert('Planos carregados com sucesso');", true);
            }
        }

    }
    protected void btnImportar_Click(object sender, EventArgs e)
    {
        if (ddlAdquirentes.SelectedValue.ToString().Trim() == "Z")
        {
            foreach (ListViewItem itemP in lsvPlanos.Items)
            {
                if (((CheckBox)itemP.FindControl("ckbImportar")).Checked == true)
                {
                    JObject oPlanos = JObject.Parse(zoop.planos_referencia(((TextBox)itemP.FindControl("txtID")).Text.ToString()).ToString());

                    SqlConnection connInsCons = new SqlConnection(Funcoes.conexao());
                    connInsCons.Open();
                    SqlCommand cmdInsCons = new SqlCommand("dbo.stp_planos_ins", connInsCons);
                    cmdInsCons.CommandType = CommandType.StoredProcedure;
                    cmdInsCons.Parameters.Add("@flg_operacao", SqlDbType.Char).Value = 'I';

                    cmdInsCons.Parameters.Add("@COD_ID_PESSOA_LICENCIADO", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["LICENCIADO"].ToString());
                    cmdInsCons.Parameters.Add("@FLG_MODELO_PLANO", SqlDbType.Char).Value = "Z";
                    cmdInsCons.Parameters.Add("@FLG_ATIVO", SqlDbType.Char).Value = "S";
                    cmdInsCons.Parameters.Add("@FLG_VISIVEL", SqlDbType.Char).Value = "S";
                    cmdInsCons.Parameters.Add("@FLG_ANTECIPADO", SqlDbType.Char).Value = "S";
                    cmdInsCons.Parameters.Add("@FLG_TIPO_PLANO", SqlDbType.Char).Value = "C";
                    //cmdInsCons.Parameters.Add("@COD_ID_PLANO_BASE", SqlDbType.Int).Value = oPlanos["id"].ToString().Trim();
                    cmdInsCons.Parameters.Add("@COD_ID_PLANO_REFERENCIA", SqlDbType.VarChar).Value = oPlanos["id"].ToString().Trim();
                    // Localiza o plano de referencia

                    cmdInsCons.Parameters.Add("@NOM_TITULO_PLANO", SqlDbType.VarChar).Value = oPlanos["name"].ToString().Trim(); ;
                    cmdInsCons.Parameters.Add("@DES_PLANO", SqlDbType.Text).Value = oPlanos["description"].ToString().Trim();
                    cmdInsCons.Parameters.Add("@NUM_DIAS_LIQUIDACAO", SqlDbType.Int).Value = 1;
                    cmdInsCons.Parameters.Add("@NUM_TAXA_ANTECIPACAO", SqlDbType.Float).Value = 0;
                    string sCodigoInclusao = cmdInsCons.ExecuteScalar().ToString();
                    connInsCons.Close();
                    connInsCons.Dispose();

                    ((CheckBox)itemP.FindControl("ckbImportar")).Checked = false;

                }
            }

            ClientScript.RegisterStartupScript(this.GetType(), "SucessoImportarZoop", "alert('Planos importados com sucesso');", true);
        }

        if (ddlAdquirentes.SelectedValue.ToString().Trim() == "A")
        {

            foreach (ListViewItem itemP in lsvPlanos.Items)
            {
                if (((CheckBox)itemP.FindControl("ckbImportar")).Checked == true)
                {
                    string jsonResposta = asaas.ListarTaxas(asaas.PegarTokenSubconta(Funcoes.strToInt(HttpContext.Current.Session["pessoa"].ToString()), Funcoes.strToInt(HttpContext.Current.Session["LICENCIADO"].ToString()))).ToString();
                    JObject oPlano = JObject.Parse(jsonResposta);
                    //txtJson.Text = jsonResposta.ToString();
                    //txtJson.Visible = true;


                    SqlConnection connInsCons = new SqlConnection(Funcoes.conexao());
                    connInsCons.Open();
                    SqlCommand cmdInsCons = new SqlCommand("dbo.stp_planos_ins", connInsCons);
                    cmdInsCons.CommandType = CommandType.StoredProcedure;
                    cmdInsCons.Parameters.Add("@flg_operacao", SqlDbType.Char).Value = 'I';

                    cmdInsCons.Parameters.Add("@COD_ID_PESSOA_LICENCIADO", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["LICENCIADO"].ToString());

                    cmdInsCons.Parameters.Add("@FLG_MODELO_PLANO", SqlDbType.Char).Value = "A";
                    cmdInsCons.Parameters.Add("@FLG_ATIVO", SqlDbType.Char).Value = "S";
                    cmdInsCons.Parameters.Add("@FLG_VISIVEL", SqlDbType.Char).Value = "S";
                    cmdInsCons.Parameters.Add("@FLG_TIPO_PLANO", SqlDbType.Char).Value = "C";
                    
                    string sAntecipacao = (ddlAntecipado.SelectedValue.ToString() == "S") ? " - COM ANTECIPAÇÃO" : " - SEM ANTECIPAÇÃO";
                    cmdInsCons.Parameters.Add("@COD_ID_PLANO_REFERENCIA", SqlDbType.VarChar).Value = "CONTA DIGITAL";

                    cmdInsCons.Parameters.Add("@NOM_TITULO_PLANO", SqlDbType.VarChar).Value = "CONTA DIGITAL" + sAntecipacao.ToString();
                    cmdInsCons.Parameters.Add("@DES_PLANO", SqlDbType.Text).Value = "CONTA DIGITAL" + sAntecipacao.ToString();

                    cmdInsCons.Parameters.Add("@FLG_ANTECIPADO", SqlDbType.Char).Value = ddlAntecipado.SelectedValue.ToString();
                    cmdInsCons.Parameters.Add("@NUM_DIAS_LIQUIDACAO", SqlDbType.Int).Value = ddlDias.SelectedValue.ToString();
                    cmdInsCons.Parameters.Add("@NUM_TAXA_ANTECIPACAO", SqlDbType.Float).Value = Funcoes.strToDouble(txtTaxaAntecipacao.Text.ToString());
                    string sCodigoInclusao = cmdInsCons.ExecuteScalar().ToString();
                    connInsCons.Close();
                    connInsCons.Dispose();

                    // ---------------------------------------------------------------------------------------------------------------------------------------------------------------
                    // SEM ANTECIPAÇÃO
                    // ---------------------------------------------------------------------------------------------------------------------------------------------------------------

                    // DEBITO 

                    SqlConnection mySelCadastroBandeirasDO = new SqlConnection(Funcoes.conexao());
                    mySelCadastroBandeirasDO.Open();
                    SqlCommand cmdSelCadastroBandeirasDO = new SqlCommand("dbo.stp_bandeiras_ins", mySelCadastroBandeirasDO);
                    cmdSelCadastroBandeirasDO.CommandType = CommandType.StoredProcedure;
                    cmdSelCadastroBandeirasDO.Parameters.Add("@flg_operacao", SqlDbType.Char).Value = "C";
                    cmdSelCadastroBandeirasDO.Parameters.Add("@NOM_BANDEIRA", SqlDbType.VarChar).Value = "%";
                    cmdSelCadastroBandeirasDO.Parameters.Add("@FLG_ATIVO", SqlDbType.VarChar).Value = "S";
                    cmdSelCadastroBandeirasDO.Parameters.Add("@FLG_TIPO", SqlDbType.VarChar).Value = "V";
                    SqlDataReader ReaderCadastroBandeirasDO = cmdSelCadastroBandeirasDO.ExecuteReader();
                    while (ReaderCadastroBandeirasDO.Read())
                    {
                        if (ReaderCadastroBandeirasDO["NOM_BANDEIRA"].ToString() == "BOLETO")
                        {
                            SqlConnection connInsConsTaxas = new SqlConnection(Funcoes.conexao());
                            connInsConsTaxas.Open();
                            SqlCommand cmdInsConsTaxas = new SqlCommand("dbo.stp_planos_parcelas_ins", connInsConsTaxas);
                            cmdInsConsTaxas.CommandType = CommandType.StoredProcedure;
                            cmdInsConsTaxas.Parameters.Add("@flg_operacao", SqlDbType.Char).Value = 'I';
                            cmdInsConsTaxas.Parameters.Add("@COD_ID_PESSOA_LICENCIADO", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["LICENCIADO"].ToString());
                            cmdInsConsTaxas.Parameters.Add("@COD_ID_PLANOS", SqlDbType.Int).Value = Funcoes.strToInt(sCodigoInclusao);
                            cmdInsConsTaxas.Parameters.Add("@COD_ID_BANDEIRA", SqlDbType.Int).Value = Funcoes.strToInt(ReaderCadastroBandeirasDO["COD_ID"].ToString());
                            cmdInsConsTaxas.Parameters.Add("@FLG_PRESENCIAL_ONLINE", SqlDbType.Char).Value = "O";
                            cmdInsConsTaxas.Parameters.Add("@NUM_VALOR_FIXO_DEBITO", SqlDbType.Float).Value = Funcoes.strToDouble(oPlano["payment"]["bankSlip"]["defaultValue"].ToString());
                            cmdInsConsTaxas.ExecuteNonQuery();
                            connInsConsTaxas.Close();
                            connInsConsTaxas.Dispose();
                        }
                        if (ReaderCadastroBandeirasDO["NOM_BANDEIRA"].ToString() == "PIX")
                        {
                            SqlConnection connInsConsTaxas = new SqlConnection(Funcoes.conexao());
                            connInsConsTaxas.Open();
                            SqlCommand cmdInsConsTaxas = new SqlCommand("dbo.stp_planos_parcelas_ins", connInsConsTaxas);
                            cmdInsConsTaxas.CommandType = CommandType.StoredProcedure;
                            cmdInsConsTaxas.Parameters.Add("@flg_operacao", SqlDbType.Char).Value = 'I';
                            cmdInsConsTaxas.Parameters.Add("@COD_ID_PESSOA_LICENCIADO", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["LICENCIADO"].ToString());
                            cmdInsConsTaxas.Parameters.Add("@COD_ID_PLANOS", SqlDbType.Int).Value = Funcoes.strToInt(sCodigoInclusao);
                            cmdInsConsTaxas.Parameters.Add("@COD_ID_BANDEIRA", SqlDbType.Int).Value = Funcoes.strToInt(ReaderCadastroBandeirasDO["COD_ID"].ToString());
                            cmdInsConsTaxas.Parameters.Add("@FLG_PRESENCIAL_ONLINE", SqlDbType.Char).Value = "O";
                            cmdInsConsTaxas.Parameters.Add("@NUM_VALOR_FIXO_DEBITO", SqlDbType.Float).Value = Funcoes.strToDouble(oPlano["payment"]["bankSlip"]["defaultValue"].ToString());
                            cmdInsConsTaxas.ExecuteNonQuery();
                            connInsConsTaxas.Close();
                            connInsConsTaxas.Dispose();
                        }
                        if (ReaderCadastroBandeirasDO["NOM_BANDEIRA"].ToString() == "VISA")
                        {
                            SqlConnection connInsConsTaxas = new SqlConnection(Funcoes.conexao());
                            connInsConsTaxas.Open();
                            SqlCommand cmdInsConsTaxas = new SqlCommand("dbo.stp_planos_parcelas_ins", connInsConsTaxas);
                            cmdInsConsTaxas.CommandType = CommandType.StoredProcedure;
                            cmdInsConsTaxas.Parameters.Add("@flg_operacao", SqlDbType.Char).Value = 'I';
                            cmdInsConsTaxas.Parameters.Add("@COD_ID_PESSOA_LICENCIADO", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["LICENCIADO"].ToString());
                            cmdInsConsTaxas.Parameters.Add("@COD_ID_PLANOS", SqlDbType.Int).Value = Funcoes.strToInt(sCodigoInclusao);
                            cmdInsConsTaxas.Parameters.Add("@COD_ID_BANDEIRA", SqlDbType.Int).Value = Funcoes.strToInt(ReaderCadastroBandeirasDO["COD_ID"].ToString());
                            cmdInsConsTaxas.Parameters.Add("@FLG_PRESENCIAL_ONLINE", SqlDbType.Char).Value = "O";
                            cmdInsConsTaxas.Parameters.Add("@NUM_VALOR_FIXO_DEBITO", SqlDbType.Float).Value = Funcoes.strToDouble(oPlano["payment"]["debitCard"]["operationValue"].ToString());
                            cmdInsConsTaxas.Parameters.Add("@NUM_VALOR_DEBITO", SqlDbType.Float).Value = Funcoes.strToDouble(oPlano["payment"]["debitCard"]["defaultPercentage"].ToString());
                            cmdInsConsTaxas.ExecuteNonQuery();
                            connInsConsTaxas.Close();
                            connInsConsTaxas.Dispose();
                        }
                        if (ReaderCadastroBandeirasDO["NOM_BANDEIRA"].ToString() == "MASTERCARD")
                        {
                            SqlConnection connInsConsTaxas = new SqlConnection(Funcoes.conexao());
                            connInsConsTaxas.Open();
                            SqlCommand cmdInsConsTaxas = new SqlCommand("dbo.stp_planos_parcelas_ins", connInsConsTaxas);
                            cmdInsConsTaxas.CommandType = CommandType.StoredProcedure;
                            cmdInsConsTaxas.Parameters.Add("@flg_operacao", SqlDbType.Char).Value = 'I';
                            cmdInsConsTaxas.Parameters.Add("@COD_ID_PESSOA_LICENCIADO", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["LICENCIADO"].ToString());
                            cmdInsConsTaxas.Parameters.Add("@COD_ID_PLANOS", SqlDbType.Int).Value = Funcoes.strToInt(sCodigoInclusao);
                            cmdInsConsTaxas.Parameters.Add("@COD_ID_BANDEIRA", SqlDbType.Int).Value = Funcoes.strToInt(ReaderCadastroBandeirasDO["COD_ID"].ToString());
                            cmdInsConsTaxas.Parameters.Add("@FLG_PRESENCIAL_ONLINE", SqlDbType.Char).Value = "O";
                            cmdInsConsTaxas.Parameters.Add("@NUM_VALOR_FIXO_DEBITO", SqlDbType.Float).Value = Funcoes.strToDouble(oPlano["payment"]["debitCard"]["operationValue"].ToString());
                            cmdInsConsTaxas.Parameters.Add("@NUM_VALOR_DEBITO", SqlDbType.Float).Value = Funcoes.strToDouble(oPlano["payment"]["debitCard"]["defaultPercentage"].ToString());
                            cmdInsConsTaxas.ExecuteNonQuery();
                            connInsConsTaxas.Close();
                            connInsConsTaxas.Dispose();
                        }
                    }



                    SqlConnection mySelCadastroBandeirasDP = new SqlConnection(Funcoes.conexao());
                    mySelCadastroBandeirasDP.Open();
                    SqlCommand cmdSelCadastroBandeirasDP = new SqlCommand("dbo.stp_bandeiras_ins", mySelCadastroBandeirasDP);
                    cmdSelCadastroBandeirasDP.CommandType = CommandType.StoredProcedure;
                    cmdSelCadastroBandeirasDP.Parameters.Add("@flg_operacao", SqlDbType.Char).Value = "C";
                    cmdSelCadastroBandeirasDP.Parameters.Add("@NOM_BANDEIRA", SqlDbType.VarChar).Value = "%";
                    cmdSelCadastroBandeirasDP.Parameters.Add("@FLG_ATIVO", SqlDbType.VarChar).Value = "S";
                    cmdSelCadastroBandeirasDP.Parameters.Add("@FLG_TIPO", SqlDbType.VarChar).Value = "V";
                    SqlDataReader ReaderCadastroBandeirasDP = cmdSelCadastroBandeirasDP.ExecuteReader();
                    while (ReaderCadastroBandeirasDP.Read())
                    {
                        if (ReaderCadastroBandeirasDP["NOM_BANDEIRA"].ToString() == "BOLETO")
                        {
                            SqlConnection connInsConsTaxas = new SqlConnection(Funcoes.conexao());
                            connInsConsTaxas.Open();
                            SqlCommand cmdInsConsTaxas = new SqlCommand("dbo.stp_planos_parcelas_ins", connInsConsTaxas);
                            cmdInsConsTaxas.CommandType = CommandType.StoredProcedure;
                            cmdInsConsTaxas.Parameters.Add("@flg_operacao", SqlDbType.Char).Value = 'I';
                            cmdInsConsTaxas.Parameters.Add("@COD_ID_PESSOA_LICENCIADO", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["LICENCIADO"].ToString());
                            cmdInsConsTaxas.Parameters.Add("@COD_ID_PLANOS", SqlDbType.Int).Value = Funcoes.strToInt(sCodigoInclusao);
                            cmdInsConsTaxas.Parameters.Add("@COD_ID_BANDEIRA", SqlDbType.Int).Value = Funcoes.strToInt(ReaderCadastroBandeirasDP["COD_ID"].ToString());
                            cmdInsConsTaxas.Parameters.Add("@FLG_PRESENCIAL_ONLINE", SqlDbType.Char).Value = "P";
                            cmdInsConsTaxas.Parameters.Add("@NUM_VALOR_FIXO_DEBITO", SqlDbType.Float).Value = Funcoes.strToDouble(oPlano["payment"]["bankSlip"]["defaultValue"].ToString());
                            cmdInsConsTaxas.ExecuteNonQuery();
                            connInsConsTaxas.Close();
                            connInsConsTaxas.Dispose();
                        }
                        if (ReaderCadastroBandeirasDP["NOM_BANDEIRA"].ToString() == "PIX")
                        {
                            SqlConnection connInsConsTaxas = new SqlConnection(Funcoes.conexao());
                            connInsConsTaxas.Open();
                            SqlCommand cmdInsConsTaxas = new SqlCommand("dbo.stp_planos_parcelas_ins", connInsConsTaxas);
                            cmdInsConsTaxas.CommandType = CommandType.StoredProcedure;
                            cmdInsConsTaxas.Parameters.Add("@flg_operacao", SqlDbType.Char).Value = 'I';
                            cmdInsConsTaxas.Parameters.Add("@COD_ID_PESSOA_LICENCIADO", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["LICENCIADO"].ToString());
                            cmdInsConsTaxas.Parameters.Add("@COD_ID_PLANOS", SqlDbType.Int).Value = Funcoes.strToInt(sCodigoInclusao);
                            cmdInsConsTaxas.Parameters.Add("@COD_ID_BANDEIRA", SqlDbType.Int).Value = Funcoes.strToInt(ReaderCadastroBandeirasDP["COD_ID"].ToString());
                            cmdInsConsTaxas.Parameters.Add("@FLG_PRESENCIAL_ONLINE", SqlDbType.Char).Value = "P";
                            cmdInsConsTaxas.Parameters.Add("@NUM_VALOR_FIXO_DEBITO", SqlDbType.Float).Value = Funcoes.strToDouble(oPlano["payment"]["pix"]["fixedFeeValue"].ToString());
                            cmdInsConsTaxas.ExecuteNonQuery();
                            connInsConsTaxas.Close();
                            connInsConsTaxas.Dispose();
                        }
                        if (ReaderCadastroBandeirasDP["NOM_BANDEIRA"].ToString() == "VISA")
                        {
                            SqlConnection connInsConsTaxas = new SqlConnection(Funcoes.conexao());
                            connInsConsTaxas.Open();
                            SqlCommand cmdInsConsTaxas = new SqlCommand("dbo.stp_planos_parcelas_ins", connInsConsTaxas);
                            cmdInsConsTaxas.CommandType = CommandType.StoredProcedure;
                            cmdInsConsTaxas.Parameters.Add("@flg_operacao", SqlDbType.Char).Value = 'I';
                            cmdInsConsTaxas.Parameters.Add("@COD_ID_PESSOA_LICENCIADO", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["LICENCIADO"].ToString());
                            cmdInsConsTaxas.Parameters.Add("@COD_ID_PLANOS", SqlDbType.Int).Value = Funcoes.strToInt(sCodigoInclusao);
                            cmdInsConsTaxas.Parameters.Add("@COD_ID_BANDEIRA", SqlDbType.Int).Value = Funcoes.strToInt(ReaderCadastroBandeirasDP["COD_ID"].ToString());
                            cmdInsConsTaxas.Parameters.Add("@FLG_PRESENCIAL_ONLINE", SqlDbType.Char).Value = "P";
                            cmdInsConsTaxas.Parameters.Add("@NUM_VALOR_FIXO_DEBITO", SqlDbType.Float).Value = Funcoes.strToDouble(oPlano["payment"]["debitCard"]["operationValue"].ToString());
                            cmdInsConsTaxas.Parameters.Add("@NUM_VALOR_DEBITO", SqlDbType.Float).Value = Funcoes.strToDouble(oPlano["payment"]["debitCard"]["defaultPercentage"].ToString());
                            cmdInsConsTaxas.ExecuteNonQuery();
                            connInsConsTaxas.Close();
                            connInsConsTaxas.Dispose();
                        }
                        if (ReaderCadastroBandeirasDP["NOM_BANDEIRA"].ToString() == "MASTERCARD")
                        {
                            SqlConnection connInsConsTaxas = new SqlConnection(Funcoes.conexao());
                            connInsConsTaxas.Open();
                            SqlCommand cmdInsConsTaxas = new SqlCommand("dbo.stp_planos_parcelas_ins", connInsConsTaxas);
                            cmdInsConsTaxas.CommandType = CommandType.StoredProcedure;
                            cmdInsConsTaxas.Parameters.Add("@flg_operacao", SqlDbType.Char).Value = 'I';
                            cmdInsConsTaxas.Parameters.Add("@COD_ID_PESSOA_LICENCIADO", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["LICENCIADO"].ToString());
                            cmdInsConsTaxas.Parameters.Add("@COD_ID_PLANOS", SqlDbType.Int).Value = Funcoes.strToInt(sCodigoInclusao);
                            cmdInsConsTaxas.Parameters.Add("@COD_ID_BANDEIRA", SqlDbType.Int).Value = Funcoes.strToInt(ReaderCadastroBandeirasDP["COD_ID"].ToString());
                            cmdInsConsTaxas.Parameters.Add("@FLG_PRESENCIAL_ONLINE", SqlDbType.Char).Value = "P";
                            cmdInsConsTaxas.Parameters.Add("@NUM_VALOR_FIXO_DEBITO", SqlDbType.Float).Value = Funcoes.strToDouble(oPlano["payment"]["debitCard"]["operationValue"].ToString());
                            cmdInsConsTaxas.Parameters.Add("@NUM_VALOR_DEBITO", SqlDbType.Float).Value = Funcoes.strToDouble(oPlano["payment"]["debitCard"]["defaultPercentage"].ToString());
                            cmdInsConsTaxas.ExecuteNonQuery();
                            connInsConsTaxas.Close();
                            connInsConsTaxas.Dispose();
                        }
                    }                   
                    // CREDITO 
                    SqlConnection mySelCadastroBandeirasCO = new SqlConnection(Funcoes.conexao());
                    mySelCadastroBandeirasCO.Open();
                    SqlCommand cmdSelCadastroBandeirasCO = new SqlCommand("dbo.stp_bandeiras_ins", mySelCadastroBandeirasCO);
                    cmdSelCadastroBandeirasCO.CommandType = CommandType.StoredProcedure;
                    cmdSelCadastroBandeirasCO.Parameters.Add("@flg_operacao", SqlDbType.Char).Value = "C";
                    cmdSelCadastroBandeirasCO.Parameters.Add("@NOM_BANDEIRA", SqlDbType.VarChar).Value = "%";
                    cmdSelCadastroBandeirasCO.Parameters.Add("@FLG_ATIVO", SqlDbType.VarChar).Value = "S";
                    cmdSelCadastroBandeirasCO.Parameters.Add("@FLG_TIPO", SqlDbType.VarChar).Value = "C";
                    SqlDataReader ReaderCadastroBandeirasCO = cmdSelCadastroBandeirasCO.ExecuteReader();
                    while (ReaderCadastroBandeirasCO.Read())
                    {
                        SqlConnection connInsConsTaxas = new SqlConnection(Funcoes.conexao());
                        connInsConsTaxas.Open();
                        SqlCommand cmdInsConsTaxas = new SqlCommand("dbo.stp_planos_parcelas_ins", connInsConsTaxas);
                        cmdInsConsTaxas.CommandType = CommandType.StoredProcedure;
                        cmdInsConsTaxas.Parameters.Add("@flg_operacao", SqlDbType.Char).Value = 'I';
                        cmdInsConsTaxas.Parameters.Add("@COD_ID_PESSOA_LICENCIADO", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["LICENCIADO"].ToString());
                        cmdInsConsTaxas.Parameters.Add("@COD_ID_PLANOS", SqlDbType.Int).Value = Funcoes.strToInt(sCodigoInclusao);
                        cmdInsConsTaxas.Parameters.Add("@COD_ID_BANDEIRA", SqlDbType.Int).Value = Funcoes.strToInt(ReaderCadastroBandeirasCO["COD_ID"].ToString());
                        cmdInsConsTaxas.Parameters.Add("@FLG_PRESENCIAL_ONLINE", SqlDbType.Char).Value = "O";

                        cmdInsConsTaxas.Parameters.Add("@NUM_VALOR_CREDITO_VISTA", SqlDbType.Float).Value = Funcoes.strToDouble(oPlano["payment"]["creditCard"]["oneInstallmentPercentage"].ToString());
                        cmdInsConsTaxas.Parameters.Add("@NUM_VALOR_CREDITO_2X", SqlDbType.Float).Value = Funcoes.strToDouble(oPlano["payment"]["creditCard"]["upToSixInstallmentsPercentage"].ToString());
                        cmdInsConsTaxas.Parameters.Add("@NUM_VALOR_CREDITO_3X", SqlDbType.Float).Value = Funcoes.strToDouble(oPlano["payment"]["creditCard"]["upToSixInstallmentsPercentage"].ToString());
                        cmdInsConsTaxas.Parameters.Add("@NUM_VALOR_CREDITO_4X", SqlDbType.Float).Value = Funcoes.strToDouble(oPlano["payment"]["creditCard"]["upToSixInstallmentsPercentage"].ToString());
                        cmdInsConsTaxas.Parameters.Add("@NUM_VALOR_CREDITO_5X", SqlDbType.Float).Value = Funcoes.strToDouble(oPlano["payment"]["creditCard"]["upToSixInstallmentsPercentage"].ToString());
                        cmdInsConsTaxas.Parameters.Add("@NUM_VALOR_CREDITO_6X", SqlDbType.Float).Value = Funcoes.strToDouble(oPlano["payment"]["creditCard"]["upToSixInstallmentsPercentage"].ToString());
                        cmdInsConsTaxas.Parameters.Add("@NUM_VALOR_CREDITO_7X", SqlDbType.Float).Value = Funcoes.strToDouble(oPlano["payment"]["creditCard"]["upToTwelveInstallmentsPercentage"].ToString());
                        cmdInsConsTaxas.Parameters.Add("@NUM_VALOR_CREDITO_8X", SqlDbType.Float).Value = Funcoes.strToDouble(oPlano["payment"]["creditCard"]["upToTwelveInstallmentsPercentage"].ToString());
                        cmdInsConsTaxas.Parameters.Add("@NUM_VALOR_CREDITO_9X", SqlDbType.Float).Value = Funcoes.strToDouble(oPlano["payment"]["creditCard"]["upToTwelveInstallmentsPercentage"].ToString());
                        cmdInsConsTaxas.Parameters.Add("@NUM_VALOR_CREDITO_10X", SqlDbType.Float).Value = Funcoes.strToDouble(oPlano["payment"]["creditCard"]["upToTwelveInstallmentsPercentage"].ToString());
                        cmdInsConsTaxas.Parameters.Add("@NUM_VALOR_CREDITO_11X", SqlDbType.Float).Value = Funcoes.strToDouble(oPlano["payment"]["creditCard"]["upToTwelveInstallmentsPercentage"].ToString());
                        cmdInsConsTaxas.Parameters.Add("@NUM_VALOR_CREDITO_12X", SqlDbType.Float).Value = Funcoes.strToDouble(oPlano["payment"]["creditCard"]["upToTwelveInstallmentsPercentage"].ToString());
                        cmdInsConsTaxas.Parameters.Add("@NUM_VALOR_CREDITO_13X", SqlDbType.Float).Value = 0;
                        cmdInsConsTaxas.Parameters.Add("@NUM_VALOR_CREDITO_14X", SqlDbType.Float).Value = 0;
                        cmdInsConsTaxas.Parameters.Add("@NUM_VALOR_CREDITO_15X", SqlDbType.Float).Value = 0;
                        cmdInsConsTaxas.Parameters.Add("@NUM_VALOR_CREDITO_16X", SqlDbType.Float).Value = 0;
                        cmdInsConsTaxas.Parameters.Add("@NUM_VALOR_CREDITO_17X", SqlDbType.Float).Value = 0;
                        cmdInsConsTaxas.Parameters.Add("@NUM_VALOR_CREDITO_18X", SqlDbType.Float).Value = 0;

                        cmdInsConsTaxas.Parameters.Add("@NUM_VALOR_FIXO_CREDITO_VISTA", SqlDbType.Float).Value = Funcoes.strToDouble(oPlano["payment"]["creditCard"]["operationValue"].ToString());
                        cmdInsConsTaxas.Parameters.Add("@NUM_VALOR_FIXO_CREDITO_2X", SqlDbType.Float).Value = Funcoes.strToDouble(oPlano["payment"]["creditCard"]["operationValue"].ToString());
                        cmdInsConsTaxas.Parameters.Add("@NUM_VALOR_FIXO_CREDITO_3X", SqlDbType.Float).Value = Funcoes.strToDouble(oPlano["payment"]["creditCard"]["operationValue"].ToString());
                        cmdInsConsTaxas.Parameters.Add("@NUM_VALOR_FIXO_CREDITO_4X", SqlDbType.Float).Value = Funcoes.strToDouble(oPlano["payment"]["creditCard"]["operationValue"].ToString());
                        cmdInsConsTaxas.Parameters.Add("@NUM_VALOR_FIXO_CREDITO_5X", SqlDbType.Float).Value = Funcoes.strToDouble(oPlano["payment"]["creditCard"]["operationValue"].ToString());
                        cmdInsConsTaxas.Parameters.Add("@NUM_VALOR_FIXO_CREDITO_6X", SqlDbType.Float).Value = Funcoes.strToDouble(oPlano["payment"]["creditCard"]["operationValue"].ToString());
                        cmdInsConsTaxas.Parameters.Add("@NUM_VALOR_FIXO_CREDITO_7X", SqlDbType.Float).Value = Funcoes.strToDouble(oPlano["payment"]["creditCard"]["operationValue"].ToString());
                        cmdInsConsTaxas.Parameters.Add("@NUM_VALOR_FIXO_CREDITO_8X", SqlDbType.Float).Value = Funcoes.strToDouble(oPlano["payment"]["creditCard"]["operationValue"].ToString());
                        cmdInsConsTaxas.Parameters.Add("@NUM_VALOR_FIXO_CREDITO_9X", SqlDbType.Float).Value = Funcoes.strToDouble(oPlano["payment"]["creditCard"]["operationValue"].ToString());
                        cmdInsConsTaxas.Parameters.Add("@NUM_VALOR_FIXO_CREDITO_10X", SqlDbType.Float).Value = Funcoes.strToDouble(oPlano["payment"]["creditCard"]["operationValue"].ToString());
                        cmdInsConsTaxas.Parameters.Add("@NUM_VALOR_FIXO_CREDITO_11X", SqlDbType.Float).Value = Funcoes.strToDouble(oPlano["payment"]["creditCard"]["operationValue"].ToString());
                        cmdInsConsTaxas.Parameters.Add("@NUM_VALOR_FIXO_CREDITO_12X", SqlDbType.Float).Value = Funcoes.strToDouble(oPlano["payment"]["creditCard"]["operationValue"].ToString());
                        cmdInsConsTaxas.Parameters.Add("@NUM_VALOR_FIXO_CREDITO_13X", SqlDbType.Float).Value = 0;
                        cmdInsConsTaxas.Parameters.Add("@NUM_VALOR_FIXO_CREDITO_14X", SqlDbType.Float).Value = 0;
                        cmdInsConsTaxas.Parameters.Add("@NUM_VALOR_FIXO_CREDITO_15X", SqlDbType.Float).Value = 0;
                        cmdInsConsTaxas.Parameters.Add("@NUM_VALOR_FIXO_CREDITO_16X", SqlDbType.Float).Value = 0;
                        cmdInsConsTaxas.Parameters.Add("@NUM_VALOR_FIXO_CREDITO_17X", SqlDbType.Float).Value = 0;
                        cmdInsConsTaxas.Parameters.Add("@NUM_VALOR_FIXO_CREDITO_18X", SqlDbType.Float).Value = 0;

                        cmdInsConsTaxas.ExecuteNonQuery();
                        connInsConsTaxas.Close();
                        connInsConsTaxas.Dispose();
                    
                    }

                    SqlConnection mySelCadastroBandeirasCP = new SqlConnection(Funcoes.conexao());
                    mySelCadastroBandeirasCP.Open();
                    SqlCommand cmdSelCadastroBandeirasCP = new SqlCommand("dbo.stp_bandeiras_ins", mySelCadastroBandeirasCP);
                    cmdSelCadastroBandeirasCP.CommandType = CommandType.StoredProcedure;
                    cmdSelCadastroBandeirasCP.Parameters.Add("@flg_operacao", SqlDbType.Char).Value = "C";
                    cmdSelCadastroBandeirasCP.Parameters.Add("@NOM_BANDEIRA", SqlDbType.VarChar).Value = "%";
                    cmdSelCadastroBandeirasCP.Parameters.Add("@FLG_ATIVO", SqlDbType.VarChar).Value = "S";
                    cmdSelCadastroBandeirasCP.Parameters.Add("@FLG_TIPO", SqlDbType.VarChar).Value = "C";
                    SqlDataReader ReaderCadastroBandeirasCP = cmdSelCadastroBandeirasCP.ExecuteReader();
                    while (ReaderCadastroBandeirasCP.Read())
                    {
                        SqlConnection connInsConsTaxas = new SqlConnection(Funcoes.conexao());
                        connInsConsTaxas.Open();
                        SqlCommand cmdInsConsTaxas = new SqlCommand("dbo.stp_planos_parcelas_ins", connInsConsTaxas);
                        cmdInsConsTaxas.CommandType = CommandType.StoredProcedure;
                        cmdInsConsTaxas.Parameters.Add("@flg_operacao", SqlDbType.Char).Value = 'I';
                        cmdInsConsTaxas.Parameters.Add("@COD_ID_PESSOA_LICENCIADO", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["LICENCIADO"].ToString());
                        cmdInsConsTaxas.Parameters.Add("@COD_ID_PLANOS", SqlDbType.Int).Value = Funcoes.strToInt(sCodigoInclusao);
                        cmdInsConsTaxas.Parameters.Add("@COD_ID_BANDEIRA", SqlDbType.Int).Value = Funcoes.strToInt(ReaderCadastroBandeirasCP["COD_ID"].ToString());
                        cmdInsConsTaxas.Parameters.Add("@FLG_PRESENCIAL_ONLINE", SqlDbType.Char).Value = "P";

                        cmdInsConsTaxas.Parameters.Add("@NUM_VALOR_CREDITO_VISTA", SqlDbType.Float).Value = Funcoes.strToDouble(oPlano["payment"]["creditCard"]["oneInstallmentPercentage"].ToString());
                        cmdInsConsTaxas.Parameters.Add("@NUM_VALOR_CREDITO_2X", SqlDbType.Float).Value = Funcoes.strToDouble(oPlano["payment"]["creditCard"]["upToSixInstallmentsPercentage"].ToString());
                        cmdInsConsTaxas.Parameters.Add("@NUM_VALOR_CREDITO_3X", SqlDbType.Float).Value = Funcoes.strToDouble(oPlano["payment"]["creditCard"]["upToSixInstallmentsPercentage"].ToString());
                        cmdInsConsTaxas.Parameters.Add("@NUM_VALOR_CREDITO_4X", SqlDbType.Float).Value = Funcoes.strToDouble(oPlano["payment"]["creditCard"]["upToSixInstallmentsPercentage"].ToString());
                        cmdInsConsTaxas.Parameters.Add("@NUM_VALOR_CREDITO_5X", SqlDbType.Float).Value = Funcoes.strToDouble(oPlano["payment"]["creditCard"]["upToSixInstallmentsPercentage"].ToString());
                        cmdInsConsTaxas.Parameters.Add("@NUM_VALOR_CREDITO_6X", SqlDbType.Float).Value = Funcoes.strToDouble(oPlano["payment"]["creditCard"]["upToSixInstallmentsPercentage"].ToString());
                        cmdInsConsTaxas.Parameters.Add("@NUM_VALOR_CREDITO_7X", SqlDbType.Float).Value = Funcoes.strToDouble(oPlano["payment"]["creditCard"]["upToTwelveInstallmentsPercentage"].ToString());
                        cmdInsConsTaxas.Parameters.Add("@NUM_VALOR_CREDITO_8X", SqlDbType.Float).Value = Funcoes.strToDouble(oPlano["payment"]["creditCard"]["upToTwelveInstallmentsPercentage"].ToString());
                        cmdInsConsTaxas.Parameters.Add("@NUM_VALOR_CREDITO_9X", SqlDbType.Float).Value = Funcoes.strToDouble(oPlano["payment"]["creditCard"]["upToTwelveInstallmentsPercentage"].ToString());
                        cmdInsConsTaxas.Parameters.Add("@NUM_VALOR_CREDITO_10X", SqlDbType.Float).Value = Funcoes.strToDouble(oPlano["payment"]["creditCard"]["upToTwelveInstallmentsPercentage"].ToString());
                        cmdInsConsTaxas.Parameters.Add("@NUM_VALOR_CREDITO_11X", SqlDbType.Float).Value = Funcoes.strToDouble(oPlano["payment"]["creditCard"]["upToTwelveInstallmentsPercentage"].ToString());
                        cmdInsConsTaxas.Parameters.Add("@NUM_VALOR_CREDITO_12X", SqlDbType.Float).Value = Funcoes.strToDouble(oPlano["payment"]["creditCard"]["upToTwelveInstallmentsPercentage"].ToString());
                        cmdInsConsTaxas.Parameters.Add("@NUM_VALOR_CREDITO_13X", SqlDbType.Float).Value = 0;
                        cmdInsConsTaxas.Parameters.Add("@NUM_VALOR_CREDITO_14X", SqlDbType.Float).Value = 0;
                        cmdInsConsTaxas.Parameters.Add("@NUM_VALOR_CREDITO_15X", SqlDbType.Float).Value = 0;
                        cmdInsConsTaxas.Parameters.Add("@NUM_VALOR_CREDITO_16X", SqlDbType.Float).Value = 0;
                        cmdInsConsTaxas.Parameters.Add("@NUM_VALOR_CREDITO_17X", SqlDbType.Float).Value = 0;
                        cmdInsConsTaxas.Parameters.Add("@NUM_VALOR_CREDITO_18X", SqlDbType.Float).Value = 0;

                        cmdInsConsTaxas.Parameters.Add("@NUM_VALOR_FIXO_CREDITO_VISTA", SqlDbType.Float).Value = Funcoes.strToDouble(oPlano["payment"]["creditCard"]["operationValue"].ToString());
                        cmdInsConsTaxas.Parameters.Add("@NUM_VALOR_FIXO_CREDITO_2X", SqlDbType.Float).Value = Funcoes.strToDouble(oPlano["payment"]["creditCard"]["operationValue"].ToString());
                        cmdInsConsTaxas.Parameters.Add("@NUM_VALOR_FIXO_CREDITO_3X", SqlDbType.Float).Value = Funcoes.strToDouble(oPlano["payment"]["creditCard"]["operationValue"].ToString());
                        cmdInsConsTaxas.Parameters.Add("@NUM_VALOR_FIXO_CREDITO_4X", SqlDbType.Float).Value = Funcoes.strToDouble(oPlano["payment"]["creditCard"]["operationValue"].ToString());
                        cmdInsConsTaxas.Parameters.Add("@NUM_VALOR_FIXO_CREDITO_5X", SqlDbType.Float).Value = Funcoes.strToDouble(oPlano["payment"]["creditCard"]["operationValue"].ToString());
                        cmdInsConsTaxas.Parameters.Add("@NUM_VALOR_FIXO_CREDITO_6X", SqlDbType.Float).Value = Funcoes.strToDouble(oPlano["payment"]["creditCard"]["operationValue"].ToString());
                        cmdInsConsTaxas.Parameters.Add("@NUM_VALOR_FIXO_CREDITO_7X", SqlDbType.Float).Value = Funcoes.strToDouble(oPlano["payment"]["creditCard"]["operationValue"].ToString());
                        cmdInsConsTaxas.Parameters.Add("@NUM_VALOR_FIXO_CREDITO_8X", SqlDbType.Float).Value = Funcoes.strToDouble(oPlano["payment"]["creditCard"]["operationValue"].ToString());
                        cmdInsConsTaxas.Parameters.Add("@NUM_VALOR_FIXO_CREDITO_9X", SqlDbType.Float).Value = Funcoes.strToDouble(oPlano["payment"]["creditCard"]["operationValue"].ToString());
                        cmdInsConsTaxas.Parameters.Add("@NUM_VALOR_FIXO_CREDITO_10X", SqlDbType.Float).Value = Funcoes.strToDouble(oPlano["payment"]["creditCard"]["operationValue"].ToString());
                        cmdInsConsTaxas.Parameters.Add("@NUM_VALOR_FIXO_CREDITO_11X", SqlDbType.Float).Value = Funcoes.strToDouble(oPlano["payment"]["creditCard"]["operationValue"].ToString());
                        cmdInsConsTaxas.Parameters.Add("@NUM_VALOR_FIXO_CREDITO_12X", SqlDbType.Float).Value = Funcoes.strToDouble(oPlano["payment"]["creditCard"]["operationValue"].ToString());
                        cmdInsConsTaxas.Parameters.Add("@NUM_VALOR_FIXO_CREDITO_13X", SqlDbType.Float).Value = 0;
                        cmdInsConsTaxas.Parameters.Add("@NUM_VALOR_FIXO_CREDITO_14X", SqlDbType.Float).Value = 0;
                        cmdInsConsTaxas.Parameters.Add("@NUM_VALOR_FIXO_CREDITO_15X", SqlDbType.Float).Value = 0;
                        cmdInsConsTaxas.Parameters.Add("@NUM_VALOR_FIXO_CREDITO_16X", SqlDbType.Float).Value = 0;
                        cmdInsConsTaxas.Parameters.Add("@NUM_VALOR_FIXO_CREDITO_17X", SqlDbType.Float).Value = 0;
                        cmdInsConsTaxas.Parameters.Add("@NUM_VALOR_FIXO_CREDITO_18X", SqlDbType.Float).Value = 0;

                        cmdInsConsTaxas.ExecuteNonQuery();
                        connInsConsTaxas.Close();
                        connInsConsTaxas.Dispose();

                    }

                    // ---------------------------------------------------------------------------------------------------------------------------------------------------------------
                    // COM ANTECIPAÇÃO
                    // ---------------------------------------------------------------------------------------------------------------------------------------------------------------

                    // DEBITO

                    SqlConnection mySelCadastroBandeirasDOS = new SqlConnection(Funcoes.conexao());
                    mySelCadastroBandeirasDOS.Open();
                    SqlCommand cmdSelCadastroBandeirasDOS = new SqlCommand("dbo.stp_bandeiras_ins", mySelCadastroBandeirasDOS);
                    cmdSelCadastroBandeirasDOS.CommandType = CommandType.StoredProcedure;
                    cmdSelCadastroBandeirasDOS.Parameters.Add("@flg_operacao", SqlDbType.Char).Value = "C";
                    cmdSelCadastroBandeirasDOS.Parameters.Add("@NOM_BANDEIRA", SqlDbType.VarChar).Value = "%";
                    cmdSelCadastroBandeirasDOS.Parameters.Add("@FLG_ATIVO", SqlDbType.VarChar).Value = "S";
                    cmdSelCadastroBandeirasDOS.Parameters.Add("@FLG_TIPO", SqlDbType.VarChar).Value = "V";
                    SqlDataReader ReaderCadastroBandeirasDOS = cmdSelCadastroBandeirasDOS.ExecuteReader();
                    while (ReaderCadastroBandeirasDOS.Read())
                    {
                        if (ReaderCadastroBandeirasDOS["NOM_BANDEIRA"].ToString() == "BOLETO")
                        {
                            SqlConnection connInsConsTaxas = new SqlConnection(Funcoes.conexao());
                            connInsConsTaxas.Open();
                            SqlCommand cmdInsConsTaxas = new SqlCommand("dbo.stp_planos_taxas_ins", connInsConsTaxas);
                            cmdInsConsTaxas.CommandType = CommandType.StoredProcedure;
                            cmdInsConsTaxas.Parameters.Add("@flg_operacao", SqlDbType.Char).Value = 'I';
                            cmdInsConsTaxas.Parameters.Add("@COD_ID_PESSOA_LICENCIADO", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["LICENCIADO"].ToString());
                            cmdInsConsTaxas.Parameters.Add("@COD_ID_PLANOS", SqlDbType.Int).Value = Funcoes.strToInt(sCodigoInclusao);
                            cmdInsConsTaxas.Parameters.Add("@COD_ID_BANDEIRAS", SqlDbType.Int).Value = Funcoes.strToInt(ReaderCadastroBandeirasDOS["COD_ID"].ToString());
                            cmdInsConsTaxas.Parameters.Add("@COD_ID_PLANOS_TABELA", SqlDbType.Int).Value = 1;
                            cmdInsConsTaxas.Parameters.Add("@NUM_VALOR", SqlDbType.Float).Value = Funcoes.strToDouble(oPlano["payment"]["bankSlip"]["defaultValue"].ToString());
                            cmdInsConsTaxas.Parameters.Add("@NUM_TAXA", SqlDbType.Float).Value = 0;
                            cmdInsConsTaxas.Parameters.Add("@NUM_REBATE", SqlDbType.Float).Value = 0;
                            cmdInsConsTaxas.Parameters.Add("@FLG_PRESENCIAL_ONLINE", SqlDbType.Char).Value = "O";
                            cmdInsConsTaxas.ExecuteNonQuery();
                            connInsConsTaxas.Close();
                            connInsConsTaxas.Dispose();
                        }
                        if (ReaderCadastroBandeirasDOS["NOM_BANDEIRA"].ToString() == "PIX")
                        {
                            SqlConnection connInsConsTaxas = new SqlConnection(Funcoes.conexao());
                            connInsConsTaxas.Open();
                            SqlCommand cmdInsConsTaxas = new SqlCommand("dbo.stp_planos_taxas_ins", connInsConsTaxas);
                            cmdInsConsTaxas.CommandType = CommandType.StoredProcedure;
                            cmdInsConsTaxas.Parameters.Add("@flg_operacao", SqlDbType.Char).Value = 'I';
                            cmdInsConsTaxas.Parameters.Add("@COD_ID_PESSOA_LICENCIADO", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["LICENCIADO"].ToString());
                            cmdInsConsTaxas.Parameters.Add("@COD_ID_PLANOS", SqlDbType.Int).Value = Funcoes.strToInt(sCodigoInclusao);
                            cmdInsConsTaxas.Parameters.Add("@COD_ID_BANDEIRAS", SqlDbType.Int).Value = Funcoes.strToInt(ReaderCadastroBandeirasDOS["COD_ID"].ToString());
                            cmdInsConsTaxas.Parameters.Add("@COD_ID_PLANOS_TABELA", SqlDbType.Int).Value = 1;
                            cmdInsConsTaxas.Parameters.Add("@NUM_VALOR", SqlDbType.Float).Value = Funcoes.strToDouble(oPlano["payment"]["pix"]["fixedFeeValue"].ToString());
                            cmdInsConsTaxas.Parameters.Add("@NUM_TAXA", SqlDbType.Float).Value = 0;
                            cmdInsConsTaxas.Parameters.Add("@NUM_REBATE", SqlDbType.Float).Value = 0;
                            cmdInsConsTaxas.Parameters.Add("@FLG_PRESENCIAL_ONLINE", SqlDbType.Char).Value = "O";
                            cmdInsConsTaxas.ExecuteNonQuery();
                            connInsConsTaxas.Close();
                            connInsConsTaxas.Dispose();
                        }
                        if (ReaderCadastroBandeirasDOS["NOM_BANDEIRA"].ToString() == "VISA")
                        {
                            SqlConnection connInsConsTaxas = new SqlConnection(Funcoes.conexao());
                            connInsConsTaxas.Open();
                            SqlCommand cmdInsConsTaxas = new SqlCommand("dbo.stp_planos_taxas_ins", connInsConsTaxas);
                            cmdInsConsTaxas.CommandType = CommandType.StoredProcedure;
                            cmdInsConsTaxas.Parameters.Add("@flg_operacao", SqlDbType.Char).Value = 'I';
                            cmdInsConsTaxas.Parameters.Add("@COD_ID_PESSOA_LICENCIADO", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["LICENCIADO"].ToString());
                            cmdInsConsTaxas.Parameters.Add("@COD_ID_PLANOS", SqlDbType.Int).Value = Funcoes.strToInt(sCodigoInclusao);
                            cmdInsConsTaxas.Parameters.Add("@COD_ID_BANDEIRAS", SqlDbType.Int).Value = Funcoes.strToInt(ReaderCadastroBandeirasDOS["COD_ID"].ToString());
                            cmdInsConsTaxas.Parameters.Add("@COD_ID_PLANOS_TABELA", SqlDbType.Int).Value = 1;
                            cmdInsConsTaxas.Parameters.Add("@NUM_VALOR", SqlDbType.Float).Value = Funcoes.strToDouble(oPlano["payment"]["debitCard"]["operationValue"].ToString());
                            cmdInsConsTaxas.Parameters.Add("@NUM_TAXA", SqlDbType.Float).Value = Funcoes.strToDouble(oPlano["payment"]["debitCard"]["defaultPercentage"].ToString()); ;
                            cmdInsConsTaxas.Parameters.Add("@NUM_REBATE", SqlDbType.Float).Value = 0;
                            cmdInsConsTaxas.Parameters.Add("@FLG_PRESENCIAL_ONLINE", SqlDbType.Char).Value = "O";
                            cmdInsConsTaxas.ExecuteNonQuery();
                            connInsConsTaxas.Close();
                            connInsConsTaxas.Dispose();
                        }
                        if (ReaderCadastroBandeirasDOS["NOM_BANDEIRA"].ToString() == "MASTERCARD")
                        {
                            SqlConnection connInsConsTaxas = new SqlConnection(Funcoes.conexao());
                            connInsConsTaxas.Open();
                            SqlCommand cmdInsConsTaxas = new SqlCommand("dbo.stp_planos_taxas_ins", connInsConsTaxas);
                            cmdInsConsTaxas.CommandType = CommandType.StoredProcedure;
                            cmdInsConsTaxas.Parameters.Add("@flg_operacao", SqlDbType.Char).Value = 'I';
                            cmdInsConsTaxas.Parameters.Add("@COD_ID_PESSOA_LICENCIADO", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["LICENCIADO"].ToString());
                            cmdInsConsTaxas.Parameters.Add("@COD_ID_PLANOS", SqlDbType.Int).Value = Funcoes.strToInt(sCodigoInclusao);
                            cmdInsConsTaxas.Parameters.Add("@COD_ID_BANDEIRAS", SqlDbType.Int).Value = Funcoes.strToInt(ReaderCadastroBandeirasDOS["COD_ID"].ToString());
                            cmdInsConsTaxas.Parameters.Add("@COD_ID_PLANOS_TABELA", SqlDbType.Int).Value = 1;
                            cmdInsConsTaxas.Parameters.Add("@NUM_VALOR", SqlDbType.Float).Value = Funcoes.strToDouble(oPlano["payment"]["debitCard"]["operationValue"].ToString());
                            cmdInsConsTaxas.Parameters.Add("@NUM_TAXA", SqlDbType.Float).Value = Funcoes.strToDouble(oPlano["payment"]["debitCard"]["defaultPercentage"].ToString()); ;
                            cmdInsConsTaxas.Parameters.Add("@NUM_REBATE", SqlDbType.Float).Value = 0;
                            cmdInsConsTaxas.Parameters.Add("@FLG_PRESENCIAL_ONLINE", SqlDbType.Char).Value = "O";
                            cmdInsConsTaxas.ExecuteNonQuery();
                            connInsConsTaxas.Close();
                            connInsConsTaxas.Dispose();
                        }
                    }


                    SqlConnection mySelCadastroBandeirasDPS = new SqlConnection(Funcoes.conexao());
                    mySelCadastroBandeirasDPS.Open();
                    SqlCommand cmdSelCadastroBandeirasDPS = new SqlCommand("dbo.stp_bandeiras_ins", mySelCadastroBandeirasDPS);
                    cmdSelCadastroBandeirasDPS.CommandType = CommandType.StoredProcedure;
                    cmdSelCadastroBandeirasDPS.Parameters.Add("@flg_operacao", SqlDbType.Char).Value = "C";
                    cmdSelCadastroBandeirasDPS.Parameters.Add("@NOM_BANDEIRA", SqlDbType.VarChar).Value = "%";
                    cmdSelCadastroBandeirasDPS.Parameters.Add("@FLG_ATIVO", SqlDbType.VarChar).Value = "S";
                    cmdSelCadastroBandeirasDPS.Parameters.Add("@FLG_TIPO", SqlDbType.VarChar).Value = "V";
                    SqlDataReader ReaderCadastroBandeirasDPS = cmdSelCadastroBandeirasDPS.ExecuteReader();
                    while (ReaderCadastroBandeirasDPS.Read())
                    {
                        if (ReaderCadastroBandeirasDPS["NOM_BANDEIRA"].ToString() == "BOLETO")
                        {
                            SqlConnection connInsConsTaxas = new SqlConnection(Funcoes.conexao());
                            connInsConsTaxas.Open();
                            SqlCommand cmdInsConsTaxas = new SqlCommand("dbo.stp_planos_taxas_ins", connInsConsTaxas);
                            cmdInsConsTaxas.CommandType = CommandType.StoredProcedure;
                            cmdInsConsTaxas.Parameters.Add("@flg_operacao", SqlDbType.Char).Value = 'I';
                            cmdInsConsTaxas.Parameters.Add("@COD_ID_PESSOA_LICENCIADO", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["LICENCIADO"].ToString());
                            cmdInsConsTaxas.Parameters.Add("@COD_ID_PLANOS", SqlDbType.Int).Value = Funcoes.strToInt(sCodigoInclusao);
                            cmdInsConsTaxas.Parameters.Add("@COD_ID_BANDEIRAS", SqlDbType.Int).Value = Funcoes.strToInt(ReaderCadastroBandeirasDPS["COD_ID"].ToString());
                            cmdInsConsTaxas.Parameters.Add("@COD_ID_PLANOS_TABELA", SqlDbType.Int).Value = 1;
                            cmdInsConsTaxas.Parameters.Add("@NUM_VALOR", SqlDbType.Float).Value = Funcoes.strToDouble(oPlano["payment"]["bankSlip"]["defaultValue"].ToString());
                            cmdInsConsTaxas.Parameters.Add("@NUM_TAXA", SqlDbType.Float).Value = 0;
                            cmdInsConsTaxas.Parameters.Add("@NUM_REBATE", SqlDbType.Float).Value = 0;
                            cmdInsConsTaxas.Parameters.Add("@FLG_PRESENCIAL_ONLINE", SqlDbType.Char).Value = "P";
                            cmdInsConsTaxas.ExecuteNonQuery();
                            connInsConsTaxas.Close();
                            connInsConsTaxas.Dispose();
                        }
                        if (ReaderCadastroBandeirasDPS["NOM_BANDEIRA"].ToString() == "PIX")
                        {
                            SqlConnection connInsConsTaxas = new SqlConnection(Funcoes.conexao());
                            connInsConsTaxas.Open();
                            SqlCommand cmdInsConsTaxas = new SqlCommand("dbo.stp_planos_taxas_ins", connInsConsTaxas);
                            cmdInsConsTaxas.CommandType = CommandType.StoredProcedure;
                            cmdInsConsTaxas.Parameters.Add("@flg_operacao", SqlDbType.Char).Value = 'I';
                            cmdInsConsTaxas.Parameters.Add("@COD_ID_PESSOA_LICENCIADO", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["LICENCIADO"].ToString());
                            cmdInsConsTaxas.Parameters.Add("@COD_ID_PLANOS", SqlDbType.Int).Value = Funcoes.strToInt(sCodigoInclusao);
                            cmdInsConsTaxas.Parameters.Add("@COD_ID_BANDEIRAS", SqlDbType.Int).Value = Funcoes.strToInt(ReaderCadastroBandeirasDPS["COD_ID"].ToString());
                            cmdInsConsTaxas.Parameters.Add("@COD_ID_PLANOS_TABELA", SqlDbType.Int).Value = 1;
                            cmdInsConsTaxas.Parameters.Add("@NUM_VALOR", SqlDbType.Float).Value = Funcoes.strToDouble(oPlano["payment"]["pix"]["fixedFeeValue"].ToString());
                            cmdInsConsTaxas.Parameters.Add("@NUM_TAXA", SqlDbType.Float).Value = 0;
                            cmdInsConsTaxas.Parameters.Add("@NUM_REBATE", SqlDbType.Float).Value = 0;
                            cmdInsConsTaxas.Parameters.Add("@FLG_PRESENCIAL_ONLINE", SqlDbType.Char).Value = "P";
                            cmdInsConsTaxas.ExecuteNonQuery();
                            connInsConsTaxas.Close();
                            connInsConsTaxas.Dispose();
                        }
                        if (ReaderCadastroBandeirasDPS["NOM_BANDEIRA"].ToString() == "VISA")
                        {
                            SqlConnection connInsConsTaxas = new SqlConnection(Funcoes.conexao());
                            connInsConsTaxas.Open();
                            SqlCommand cmdInsConsTaxas = new SqlCommand("dbo.stp_planos_taxas_ins", connInsConsTaxas);
                            cmdInsConsTaxas.CommandType = CommandType.StoredProcedure;
                            cmdInsConsTaxas.Parameters.Add("@flg_operacao", SqlDbType.Char).Value = 'I';
                            cmdInsConsTaxas.Parameters.Add("@COD_ID_PESSOA_LICENCIADO", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["LICENCIADO"].ToString());
                            cmdInsConsTaxas.Parameters.Add("@COD_ID_PLANOS", SqlDbType.Int).Value = Funcoes.strToInt(sCodigoInclusao);
                            cmdInsConsTaxas.Parameters.Add("@COD_ID_BANDEIRAS", SqlDbType.Int).Value = Funcoes.strToInt(ReaderCadastroBandeirasDPS["COD_ID"].ToString());
                            cmdInsConsTaxas.Parameters.Add("@COD_ID_PLANOS_TABELA", SqlDbType.Int).Value = 1;
                            cmdInsConsTaxas.Parameters.Add("@NUM_VALOR", SqlDbType.Float).Value = Funcoes.strToDouble(oPlano["payment"]["debitCard"]["operationValue"].ToString());
                            cmdInsConsTaxas.Parameters.Add("@NUM_TAXA", SqlDbType.Float).Value = Funcoes.strToDouble(oPlano["payment"]["debitCard"]["defaultPercentage"].ToString()); ;
                            cmdInsConsTaxas.Parameters.Add("@NUM_REBATE", SqlDbType.Float).Value = 0;
                            cmdInsConsTaxas.Parameters.Add("@FLG_PRESENCIAL_ONLINE", SqlDbType.Char).Value = "P";
                            cmdInsConsTaxas.ExecuteNonQuery();
                            connInsConsTaxas.Close();
                            connInsConsTaxas.Dispose();
                        }
                        if (ReaderCadastroBandeirasDPS["NOM_BANDEIRA"].ToString() == "MASTERCARD")
                        {
                            SqlConnection connInsConsTaxas = new SqlConnection(Funcoes.conexao());
                            connInsConsTaxas.Open();
                            SqlCommand cmdInsConsTaxas = new SqlCommand("dbo.stp_planos_taxas_ins", connInsConsTaxas);
                            cmdInsConsTaxas.CommandType = CommandType.StoredProcedure;
                            cmdInsConsTaxas.Parameters.Add("@flg_operacao", SqlDbType.Char).Value = 'I';
                            cmdInsConsTaxas.Parameters.Add("@COD_ID_PESSOA_LICENCIADO", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["LICENCIADO"].ToString());
                            cmdInsConsTaxas.Parameters.Add("@COD_ID_PLANOS", SqlDbType.Int).Value = Funcoes.strToInt(sCodigoInclusao);
                            cmdInsConsTaxas.Parameters.Add("@COD_ID_BANDEIRAS", SqlDbType.Int).Value = Funcoes.strToInt(ReaderCadastroBandeirasDPS["COD_ID"].ToString());
                            cmdInsConsTaxas.Parameters.Add("@COD_ID_PLANOS_TABELA", SqlDbType.Int).Value = 1;
                            cmdInsConsTaxas.Parameters.Add("@NUM_VALOR", SqlDbType.Float).Value = Funcoes.strToDouble(oPlano["payment"]["debitCard"]["operationValue"].ToString());
                            cmdInsConsTaxas.Parameters.Add("@NUM_TAXA", SqlDbType.Float).Value = Funcoes.strToDouble(oPlano["payment"]["debitCard"]["defaultPercentage"].ToString()); ;
                            cmdInsConsTaxas.Parameters.Add("@NUM_REBATE", SqlDbType.Float).Value = 0;
                            cmdInsConsTaxas.Parameters.Add("@FLG_PRESENCIAL_ONLINE", SqlDbType.Char).Value = "P";
                            cmdInsConsTaxas.ExecuteNonQuery();
                            connInsConsTaxas.Close();
                            connInsConsTaxas.Dispose();
                        }
                    }

                    // CREDITO

                    SqlConnection mySelCadastroBandeirasCPS = new SqlConnection(Funcoes.conexao());
                    mySelCadastroBandeirasCPS.Open();
                    SqlCommand cmdSelCadastroBandeirasCPS = new SqlCommand("dbo.stp_bandeiras_ins", mySelCadastroBandeirasCPS);
                    cmdSelCadastroBandeirasCPS.CommandType = CommandType.StoredProcedure;
                    cmdSelCadastroBandeirasCPS.Parameters.Add("@flg_operacao", SqlDbType.Char).Value = "C";
                    cmdSelCadastroBandeirasCPS.Parameters.Add("@NOM_BANDEIRA", SqlDbType.VarChar).Value = "%";
                    cmdSelCadastroBandeirasCPS.Parameters.Add("@FLG_ATIVO", SqlDbType.VarChar).Value = "S";
                    cmdSelCadastroBandeirasCPS.Parameters.Add("@FLG_TIPO", SqlDbType.VarChar).Value = "C";
                    SqlDataReader ReaderCadastroBandeirasCPS = cmdSelCadastroBandeirasCPS.ExecuteReader();
                    while (ReaderCadastroBandeirasCPS.Read())
                    {

                        SqlConnection mySelCadastroTabela = new SqlConnection(Funcoes.conexao());
                        mySelCadastroTabela.Open();
                        SqlCommand cmdSelCadastroTabela = new SqlCommand("dbo.stp_planos_taxas_ins", mySelCadastroTabela);
                        cmdSelCadastroTabela.CommandType = CommandType.StoredProcedure;
                        cmdSelCadastroTabela.Parameters.Add("@flg_operacao", SqlDbType.Char).Value = "T";
                        cmdSelCadastroTabela.Parameters.Add("@FLG_TIPO", SqlDbType.VarChar).Value = "C";
                        SqlDataReader ReaderCadastroTabela = cmdSelCadastroTabela.ExecuteReader();
                        while (ReaderCadastroTabela.Read())
                        {
                            SqlConnection connInsConsTaxas = new SqlConnection(Funcoes.conexao());
                            connInsConsTaxas.Open();
                            SqlCommand cmdInsConsTaxas = new SqlCommand("dbo.stp_planos_taxas_ins", connInsConsTaxas);
                            cmdInsConsTaxas.CommandType = CommandType.StoredProcedure;
                            cmdInsConsTaxas.Parameters.Add("@flg_operacao", SqlDbType.Char).Value = 'I';
                            cmdInsConsTaxas.Parameters.Add("@COD_ID_PESSOA_LICENCIADO", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["LICENCIADO"].ToString());
                            cmdInsConsTaxas.Parameters.Add("@COD_ID_PLANOS", SqlDbType.Int).Value = Funcoes.strToInt(sCodigoInclusao);
                            cmdInsConsTaxas.Parameters.Add("@COD_ID_BANDEIRAS", SqlDbType.Int).Value = Funcoes.strToInt(ReaderCadastroBandeirasCPS["COD_ID"].ToString());
                            cmdInsConsTaxas.Parameters.Add("@COD_ID_PLANOS_TABELA", SqlDbType.Int).Value = Funcoes.strToInt(ReaderCadastroTabela["COD_ID"].ToString());
                            cmdInsConsTaxas.Parameters.Add("@NUM_VALOR", SqlDbType.Float).Value = Funcoes.strToDouble(oPlano["payment"]["creditCard"]["operationValue"].ToString());
                            if (Funcoes.strToInt(ReaderCadastroTabela["COD_PARCELA_FINAL"].ToString()) == 1)
                            {
                                cmdInsConsTaxas.Parameters.Add("@NUM_TAXA", SqlDbType.Float).Value = Funcoes.strToDouble(oPlano["payment"]["creditCard"]["oneInstallmentPercentage"].ToString());
                            }
                            if (Funcoes.strToInt(ReaderCadastroTabela["COD_PARCELA_FINAL"].ToString()) == 6)
                            {
                                cmdInsConsTaxas.Parameters.Add("@NUM_TAXA", SqlDbType.Float).Value = Funcoes.strToDouble(oPlano["payment"]["creditCard"]["upToSixInstallmentsPercentage"].ToString());
                            }
                            if (Funcoes.strToInt(ReaderCadastroTabela["COD_PARCELA_FINAL"].ToString()) == 12)
                            {
                                cmdInsConsTaxas.Parameters.Add("@NUM_TAXA", SqlDbType.Float).Value = Funcoes.strToDouble(oPlano["payment"]["creditCard"]["upToTwelveInstallmentsPercentage"].ToString());
                            }

                            cmdInsConsTaxas.Parameters.Add("@NUM_REBATE", SqlDbType.Float).Value = 0;
                            cmdInsConsTaxas.Parameters.Add("@FLG_PRESENCIAL_ONLINE", SqlDbType.Char).Value = "P";
                            cmdInsConsTaxas.ExecuteNonQuery();
                            connInsConsTaxas.Close();
                            connInsConsTaxas.Dispose();
                        }
                    }


                    SqlConnection mySelCadastroBandeirasCOS = new SqlConnection(Funcoes.conexao());
                    mySelCadastroBandeirasCOS.Open();
                    SqlCommand cmdSelCadastroBandeirasCOS = new SqlCommand("dbo.stp_bandeiras_ins", mySelCadastroBandeirasCOS);
                    cmdSelCadastroBandeirasCOS.CommandType = CommandType.StoredProcedure;
                    cmdSelCadastroBandeirasCOS.Parameters.Add("@flg_operacao", SqlDbType.Char).Value = "C";
                    cmdSelCadastroBandeirasCOS.Parameters.Add("@NOM_BANDEIRA", SqlDbType.VarChar).Value = "%";
                    cmdSelCadastroBandeirasCOS.Parameters.Add("@FLG_ATIVO", SqlDbType.VarChar).Value = "S";
                    cmdSelCadastroBandeirasCOS.Parameters.Add("@FLG_TIPO", SqlDbType.VarChar).Value = "C";
                    SqlDataReader ReaderCadastroBandeirasCOS = cmdSelCadastroBandeirasCOS.ExecuteReader();
                    while (ReaderCadastroBandeirasCOS.Read())
                    {

                        SqlConnection mySelCadastroTabela = new SqlConnection(Funcoes.conexao());
                        mySelCadastroTabela.Open();
                        SqlCommand cmdSelCadastroTabela = new SqlCommand("dbo.stp_planos_taxas_ins", mySelCadastroTabela);
                        cmdSelCadastroTabela.CommandType = CommandType.StoredProcedure;
                        cmdSelCadastroTabela.Parameters.Add("@flg_operacao", SqlDbType.Char).Value = "T";
                        cmdSelCadastroTabela.Parameters.Add("@FLG_TIPO", SqlDbType.VarChar).Value = "C";
                        SqlDataReader ReaderCadastroTabela = cmdSelCadastroTabela.ExecuteReader();
                        while (ReaderCadastroTabela.Read())
                        {
                            SqlConnection connInsConsTaxas = new SqlConnection(Funcoes.conexao());
                            connInsConsTaxas.Open();
                            SqlCommand cmdInsConsTaxas = new SqlCommand("dbo.stp_planos_taxas_ins", connInsConsTaxas);
                            cmdInsConsTaxas.CommandType = CommandType.StoredProcedure;
                            cmdInsConsTaxas.Parameters.Add("@flg_operacao", SqlDbType.Char).Value = 'I';
                            cmdInsConsTaxas.Parameters.Add("@COD_ID_PESSOA_LICENCIADO", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["LICENCIADO"].ToString());
                            cmdInsConsTaxas.Parameters.Add("@COD_ID_PLANOS", SqlDbType.Int).Value = Funcoes.strToInt(sCodigoInclusao);
                            cmdInsConsTaxas.Parameters.Add("@COD_ID_BANDEIRAS", SqlDbType.Int).Value = Funcoes.strToInt(ReaderCadastroBandeirasCOS["COD_ID"].ToString());
                            cmdInsConsTaxas.Parameters.Add("@COD_ID_PLANOS_TABELA", SqlDbType.Int).Value = Funcoes.strToInt(ReaderCadastroTabela["COD_ID"].ToString());
                            cmdInsConsTaxas.Parameters.Add("@NUM_VALOR", SqlDbType.Float).Value = Funcoes.strToDouble(oPlano["payment"]["creditCard"]["operationValue"].ToString());
                            if (Funcoes.strToInt(ReaderCadastroTabela["COD_PARCELA_FINAL"].ToString()) == 1)
                            {
                                cmdInsConsTaxas.Parameters.Add("@NUM_TAXA", SqlDbType.Float).Value = Funcoes.strToDouble(oPlano["payment"]["creditCard"]["oneInstallmentPercentage"].ToString());
                            }
                            if (Funcoes.strToInt(ReaderCadastroTabela["COD_PARCELA_FINAL"].ToString()) == 6)
                            {
                                cmdInsConsTaxas.Parameters.Add("@NUM_TAXA", SqlDbType.Float).Value = Funcoes.strToDouble(oPlano["payment"]["creditCard"]["upToSixInstallmentsPercentage"].ToString());
                            }
                            if (Funcoes.strToInt(ReaderCadastroTabela["COD_PARCELA_FINAL"].ToString()) == 12)
                            {
                                cmdInsConsTaxas.Parameters.Add("@NUM_TAXA", SqlDbType.Float).Value = Funcoes.strToDouble(oPlano["payment"]["creditCard"]["upToTwelveInstallmentsPercentage"].ToString());
                            }

                            cmdInsConsTaxas.Parameters.Add("@NUM_REBATE", SqlDbType.Float).Value = 0;
                            cmdInsConsTaxas.Parameters.Add("@FLG_PRESENCIAL_ONLINE", SqlDbType.Char).Value = "O";
                            cmdInsConsTaxas.ExecuteNonQuery();
                            connInsConsTaxas.Close();
                            connInsConsTaxas.Dispose();
                        }
                    }

                }
            }

            ckbTodos.Checked = false;
            ClientScript.RegisterStartupScript(this.GetType(), "SucessoImportarAsaas", "alert('Planos importados com sucesso');", true);

            // MENSAGEM
        }

        if (ddlAdquirentes.SelectedValue.ToString().Trim() == "C")
        {

            foreach (ListViewItem itemP in lsvPlanos.Items)
            {
                if (((CheckBox)itemP.FindControl("ckbImportar")).Checked == true)
                {

                    JObject oPlanos = JObject.Parse(hubcappta.ConsultarPlano(((TextBox)itemP.FindControl("txtID")).Text.ToString()).ToString());

                    SqlConnection connInsCons = new SqlConnection(Funcoes.conexao());
                    connInsCons.Open();
                    SqlCommand cmdInsCons = new SqlCommand("dbo.stp_planos_ins", connInsCons);
                    cmdInsCons.CommandType = CommandType.StoredProcedure;
                    cmdInsCons.Parameters.Add("@flg_operacao", SqlDbType.Char).Value = 'I';

                    cmdInsCons.Parameters.Add("@COD_ID_PESSOA_LICENCIADO", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["LICENCIADO"].ToString());
                    cmdInsCons.Parameters.Add("@FLG_MODELO_PLANO", SqlDbType.Char).Value = "C";
                    cmdInsCons.Parameters.Add("@FLG_ATIVO", SqlDbType.Char).Value = "S";
                    cmdInsCons.Parameters.Add("@FLG_VISIVEL", SqlDbType.Char).Value = "S";
                    cmdInsCons.Parameters.Add("@FLG_ANTECIPADO", SqlDbType.Char).Value = "S";
                    cmdInsCons.Parameters.Add("@FLG_TIPO_PLANO", SqlDbType.Char).Value = "C";
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

                            SqlConnection mySelCadastro = new SqlConnection(Funcoes.conexao());
                            mySelCadastro.Open();
                            SqlCommand cmdSelCadastro = new SqlCommand("dbo.stp_opcoes_planos_cappta_ins", mySelCadastro);
                            cmdSelCadastro.CommandType = CommandType.StoredProcedure;
                            cmdSelCadastro.Parameters.Add("@flg_operacao", SqlDbType.Char).Value = "B";
                            cmdSelCadastro.Parameters.Add("@NOM_REFERENCIA", SqlDbType.VarChar).Value = oPlanos["schemes"][t]["scheme"].ToString();
                            SqlDataReader ReaderCadastro = cmdSelCadastro.ExecuteReader();
                            while (ReaderCadastro.Read())
                            {
                                iBandeira = Funcoes.strToInt(ReaderCadastro["COD_BANDEIRA"].ToString());
                                sBandeira = ReaderCadastro["NOM_BANDEIRA"].ToString();
                                sTipo = ReaderCadastro["NOM_TIPO"].ToString();
                            }


                            cmdInsConsTaxas.Parameters.Add("@COD_ID_BANDEIRA", SqlDbType.Int).Value = iBandeira;
                            cmdInsConsTaxas.Parameters.Add("@FLG_PRESENCIAL_ONLINE", SqlDbType.Char).Value = "P";
                            // Localizar o plano de referencia


                            SqlConnection mySelCadastroPlanoReferencia = new SqlConnection(Funcoes.conexao());
                            mySelCadastroPlanoReferencia.Open();
                            SqlCommand cmdSelCadastroPlanoReferencia = new SqlCommand("dbo.stp_planos_referencia_ins", mySelCadastroPlanoReferencia);
                            cmdSelCadastroPlanoReferencia.CommandType = CommandType.StoredProcedure;
                            cmdSelCadastroPlanoReferencia.Parameters.Add("@flg_operacao", SqlDbType.Char).Value = "P";
                            cmdSelCadastroPlanoReferencia.Parameters.Add("@COD_ID_PLANO_REFERENCIA", SqlDbType.VarChar).Value = oPlanos["basePlanId"].ToString();
                            SqlDataReader ReaderCadastroPlanoReferencia = cmdSelCadastroPlanoReferencia.ExecuteReader();
                            while (ReaderCadastroPlanoReferencia.Read())
                            {
                                iPlanoReferencia = Funcoes.strToInt(ReaderCadastroPlanoReferencia["COD_ID"].ToString());
                            }

                            // Carrega as taxas
                            if (sTipo == "credit")
                            {
                                if (ValorPercentualPresencialAntecipado(iPlanoReferencia, iBandeira, "V", "P", 1, 0) > 0)
                                {
                                    cmdInsConsTaxas.Parameters.Add("@NUM_VALOR_DEBITO", SqlDbType.Float).Value = ValorPercentualPresencialAntecipado(iPlanoReferencia, iBandeira, "V", "P", 1, 0);
                                }
                                else
                                {
                                    cmdInsConsTaxas.Parameters.Add("@NUM_VALOR_DEBITO", SqlDbType.Float).Value = ValorPercentualPresencialAntecipado(iPlanoReferencia, iBandeira, "C", "P", 1, 0);
                                }
                            }
                            if (sTipo == "credit")
                            {
                                if (Funcoes.strToInt(oPlanos["schemes"][t]["fees"][p]["installments"].ToString()) == 1) { cmdInsConsTaxas.Parameters.Add("@NUM_VALOR_CREDITO_VISTA", SqlDbType.Float).Value = ValorPercentualPresencialAntecipado(iPlanoReferencia, iBandeira, "C", "P", 1, 1); }
                                if (Funcoes.strToInt(oPlanos["schemes"][t]["fees"][p]["installments"].ToString()) == 2) { cmdInsConsTaxas.Parameters.Add("@NUM_VALOR_CREDITO_2X", SqlDbType.Float).Value = ValorPercentualPresencialAntecipado(iPlanoReferencia, iBandeira, "C", "P", 2, 1); }
                                if (Funcoes.strToInt(oPlanos["schemes"][t]["fees"][p]["installments"].ToString()) == 3) { cmdInsConsTaxas.Parameters.Add("@NUM_VALOR_CREDITO_3X", SqlDbType.Float).Value = ValorPercentualPresencialAntecipado(iPlanoReferencia, iBandeira, "C", "P", 3, 1); }
                                if (Funcoes.strToInt(oPlanos["schemes"][t]["fees"][p]["installments"].ToString()) == 4) { cmdInsConsTaxas.Parameters.Add("@NUM_VALOR_CREDITO_4X", SqlDbType.Float).Value = ValorPercentualPresencialAntecipado(iPlanoReferencia, iBandeira, "C", "P", 4, 1); }
                                if (Funcoes.strToInt(oPlanos["schemes"][t]["fees"][p]["installments"].ToString()) == 5) { cmdInsConsTaxas.Parameters.Add("@NUM_VALOR_CREDITO_5X", SqlDbType.Float).Value = ValorPercentualPresencialAntecipado(iPlanoReferencia, iBandeira, "C", "P", 5, 1); }
                                if (Funcoes.strToInt(oPlanos["schemes"][t]["fees"][p]["installments"].ToString()) == 6) { cmdInsConsTaxas.Parameters.Add("@NUM_VALOR_CREDITO_6X", SqlDbType.Float).Value = ValorPercentualPresencialAntecipado(iPlanoReferencia, iBandeira, "C", "P", 6, 1); }
                                if (Funcoes.strToInt(oPlanos["schemes"][t]["fees"][p]["installments"].ToString()) == 7) { cmdInsConsTaxas.Parameters.Add("@NUM_VALOR_CREDITO_7X", SqlDbType.Float).Value = ValorPercentualPresencialAntecipado(iPlanoReferencia, iBandeira, "C", "P", 7, 1); }
                                if (Funcoes.strToInt(oPlanos["schemes"][t]["fees"][p]["installments"].ToString()) == 8) { cmdInsConsTaxas.Parameters.Add("@NUM_VALOR_CREDITO_8X", SqlDbType.Float).Value = ValorPercentualPresencialAntecipado(iPlanoReferencia, iBandeira, "C", "P", 8, 1); }
                                if (Funcoes.strToInt(oPlanos["schemes"][t]["fees"][p]["installments"].ToString()) == 9) { cmdInsConsTaxas.Parameters.Add("@NUM_VALOR_CREDITO_9X", SqlDbType.Float).Value = ValorPercentualPresencialAntecipado(iPlanoReferencia, iBandeira, "C", "P", 9, 1); }
                                if (Funcoes.strToInt(oPlanos["schemes"][t]["fees"][p]["installments"].ToString()) == 10) { cmdInsConsTaxas.Parameters.Add("@NUM_VALOR_CREDITO_10X", SqlDbType.Float).Value = ValorPercentualPresencialAntecipado(iPlanoReferencia, iBandeira, "C", "P", 10, 1); }
                                if (Funcoes.strToInt(oPlanos["schemes"][t]["fees"][p]["installments"].ToString()) == 11) { cmdInsConsTaxas.Parameters.Add("@NUM_VALOR_CREDITO_11X", SqlDbType.Float).Value = ValorPercentualPresencialAntecipado(iPlanoReferencia, iBandeira, "C", "P", 11, 1); }
                                if (Funcoes.strToInt(oPlanos["schemes"][t]["fees"][p]["installments"].ToString()) == 12) { cmdInsConsTaxas.Parameters.Add("@NUM_VALOR_CREDITO_12X", SqlDbType.Float).Value = ValorPercentualPresencialAntecipado(iPlanoReferencia, iBandeira, "C", "P", 12, 1); }
                                if (Funcoes.strToInt(oPlanos["schemes"][t]["fees"][p]["installments"].ToString()) == 13) { cmdInsConsTaxas.Parameters.Add("@NUM_VALOR_CREDITO_13X", SqlDbType.Float).Value = ValorPercentualPresencialAntecipado(iPlanoReferencia, iBandeira, "C", "P", 13, 1); }
                                if (Funcoes.strToInt(oPlanos["schemes"][t]["fees"][p]["installments"].ToString()) == 14) { cmdInsConsTaxas.Parameters.Add("@NUM_VALOR_CREDITO_14X", SqlDbType.Float).Value = ValorPercentualPresencialAntecipado(iPlanoReferencia, iBandeira, "C", "P", 14, 1); }
                                if (Funcoes.strToInt(oPlanos["schemes"][t]["fees"][p]["installments"].ToString()) == 15) { cmdInsConsTaxas.Parameters.Add("@NUM_VALOR_CREDITO_15X", SqlDbType.Float).Value = ValorPercentualPresencialAntecipado(iPlanoReferencia, iBandeira, "C", "P", 15, 1); }
                                if (Funcoes.strToInt(oPlanos["schemes"][t]["fees"][p]["installments"].ToString()) == 16) { cmdInsConsTaxas.Parameters.Add("@NUM_VALOR_CREDITO_16X", SqlDbType.Float).Value = ValorPercentualPresencialAntecipado(iPlanoReferencia, iBandeira, "C", "P", 16, 1); }
                                if (Funcoes.strToInt(oPlanos["schemes"][t]["fees"][p]["installments"].ToString()) == 17) { cmdInsConsTaxas.Parameters.Add("@NUM_VALOR_CREDITO_17X", SqlDbType.Float).Value = ValorPercentualPresencialAntecipado(iPlanoReferencia, iBandeira, "C", "P", 17, 1); }
                                if (Funcoes.strToInt(oPlanos["schemes"][t]["fees"][p]["installments"].ToString()) == 18) { cmdInsConsTaxas.Parameters.Add("@NUM_VALOR_CREDITO_18X", SqlDbType.Float).Value = ValorPercentualPresencialAntecipado(iPlanoReferencia, iBandeira, "C", "P", 18, 1); }
                            }



                            // Carrega os markup
                            if (sTipo == "debit")
                            {
                                cmdInsConsTaxas.Parameters.Add("@NUM_VALOR_MARKUP_DEBITO", SqlDbType.Float).Value = Funcoes.strToDouble(oPlanos["schemes"][t]["fees"][p]["rate"].ToString()) / 100;
                            }
                            if (sTipo == "credit")
                            {
                                if (Funcoes.strToInt(oPlanos["schemes"][t]["fees"][p]["installments"].ToString()) == 1) { cmdInsConsTaxas.Parameters.Add("@NUM_VALOR_MARKUP_VISTA", SqlDbType.Float).Value = Funcoes.strToDouble(oPlanos["schemes"][t]["fees"][p]["rate"].ToString()) / 100; }
                                if (Funcoes.strToInt(oPlanos["schemes"][t]["fees"][p]["installments"].ToString()) == 2) { cmdInsConsTaxas.Parameters.Add("@NUM_VALOR_MARKUP_2X", SqlDbType.Float).Value = Funcoes.strToDouble(oPlanos["schemes"][t]["fees"][p]["rate"].ToString()) / 100; }
                                if (Funcoes.strToInt(oPlanos["schemes"][t]["fees"][p]["installments"].ToString()) == 3) { cmdInsConsTaxas.Parameters.Add("@NUM_VALOR_MARKUP_3X", SqlDbType.Float).Value = Funcoes.strToDouble(oPlanos["schemes"][t]["fees"][p]["rate"].ToString()) / 100; }
                                if (Funcoes.strToInt(oPlanos["schemes"][t]["fees"][p]["installments"].ToString()) == 4) { cmdInsConsTaxas.Parameters.Add("@NUM_VALOR_MARKUP_4X", SqlDbType.Float).Value = Funcoes.strToDouble(oPlanos["schemes"][t]["fees"][p]["rate"].ToString()) / 100; }
                                if (Funcoes.strToInt(oPlanos["schemes"][t]["fees"][p]["installments"].ToString()) == 5) { cmdInsConsTaxas.Parameters.Add("@NUM_VALOR_MARKUP_5X", SqlDbType.Float).Value = Funcoes.strToDouble(oPlanos["schemes"][t]["fees"][p]["rate"].ToString()) / 100; }
                                if (Funcoes.strToInt(oPlanos["schemes"][t]["fees"][p]["installments"].ToString()) == 6) { cmdInsConsTaxas.Parameters.Add("@NUM_VALOR_MARKUP_6X", SqlDbType.Float).Value = Funcoes.strToDouble(oPlanos["schemes"][t]["fees"][p]["rate"].ToString()) / 100; }
                                if (Funcoes.strToInt(oPlanos["schemes"][t]["fees"][p]["installments"].ToString()) == 7) { cmdInsConsTaxas.Parameters.Add("@NUM_VALOR_MARKUP_7X", SqlDbType.Float).Value = Funcoes.strToDouble(oPlanos["schemes"][t]["fees"][p]["rate"].ToString()) / 100; }
                                if (Funcoes.strToInt(oPlanos["schemes"][t]["fees"][p]["installments"].ToString()) == 8) { cmdInsConsTaxas.Parameters.Add("@NUM_VALOR_MARKUP_8X", SqlDbType.Float).Value = Funcoes.strToDouble(oPlanos["schemes"][t]["fees"][p]["rate"].ToString()) / 100; }
                                if (Funcoes.strToInt(oPlanos["schemes"][t]["fees"][p]["installments"].ToString()) == 9) { cmdInsConsTaxas.Parameters.Add("@NUM_VALOR_MARKUP_9X", SqlDbType.Float).Value = Funcoes.strToDouble(oPlanos["schemes"][t]["fees"][p]["rate"].ToString()) / 100; }
                                if (Funcoes.strToInt(oPlanos["schemes"][t]["fees"][p]["installments"].ToString()) == 10) { cmdInsConsTaxas.Parameters.Add("@NUM_VALOR_MARKUP_10X", SqlDbType.Float).Value = Funcoes.strToDouble(oPlanos["schemes"][t]["fees"][p]["rate"].ToString()) / 100; }
                                if (Funcoes.strToInt(oPlanos["schemes"][t]["fees"][p]["installments"].ToString()) == 11) { cmdInsConsTaxas.Parameters.Add("@NUM_VALOR_MARKUP_11X", SqlDbType.Float).Value = Funcoes.strToDouble(oPlanos["schemes"][t]["fees"][p]["rate"].ToString()) / 100; }
                                if (Funcoes.strToInt(oPlanos["schemes"][t]["fees"][p]["installments"].ToString()) == 12) { cmdInsConsTaxas.Parameters.Add("@NUM_VALOR_MARKUP_12X", SqlDbType.Float).Value = Funcoes.strToDouble(oPlanos["schemes"][t]["fees"][p]["rate"].ToString()) / 100; }
                                if (Funcoes.strToInt(oPlanos["schemes"][t]["fees"][p]["installments"].ToString()) == 13) { cmdInsConsTaxas.Parameters.Add("@NUM_VALOR_MARKUP_13X", SqlDbType.Float).Value = Funcoes.strToDouble(oPlanos["schemes"][t]["fees"][p]["rate"].ToString()) / 100; }
                                if (Funcoes.strToInt(oPlanos["schemes"][t]["fees"][p]["installments"].ToString()) == 14) { cmdInsConsTaxas.Parameters.Add("@NUM_VALOR_MARKUP_14X", SqlDbType.Float).Value = Funcoes.strToDouble(oPlanos["schemes"][t]["fees"][p]["rate"].ToString()) / 100; }
                                if (Funcoes.strToInt(oPlanos["schemes"][t]["fees"][p]["installments"].ToString()) == 15) { cmdInsConsTaxas.Parameters.Add("@NUM_VALOR_MARKUP_15X", SqlDbType.Float).Value = Funcoes.strToDouble(oPlanos["schemes"][t]["fees"][p]["rate"].ToString()) / 100; }
                                if (Funcoes.strToInt(oPlanos["schemes"][t]["fees"][p]["installments"].ToString()) == 16) { cmdInsConsTaxas.Parameters.Add("@NUM_VALOR_MARKUP_16X", SqlDbType.Float).Value = Funcoes.strToDouble(oPlanos["schemes"][t]["fees"][p]["rate"].ToString()) / 100; }
                                if (Funcoes.strToInt(oPlanos["schemes"][t]["fees"][p]["installments"].ToString()) == 17) { cmdInsConsTaxas.Parameters.Add("@NUM_VALOR_MARKUP_17X", SqlDbType.Float).Value = Funcoes.strToDouble(oPlanos["schemes"][t]["fees"][p]["rate"].ToString()) / 100; }
                                if (Funcoes.strToInt(oPlanos["schemes"][t]["fees"][p]["installments"].ToString()) == 18) { cmdInsConsTaxas.Parameters.Add("@NUM_VALOR_MARKUP_18X", SqlDbType.Float).Value = Funcoes.strToDouble(oPlanos["schemes"][t]["fees"][p]["rate"].ToString()) / 100; }
                            }
                            cmdInsConsTaxas.ExecuteNonQuery();
                            connInsConsTaxas.Close();
                            connInsConsTaxas.Dispose();
                        }
                    }

                    ((CheckBox)itemP.FindControl("ckbImportar")).Checked = false;
                }
            }

            ckbTodos.Checked = false;
            ClientScript.RegisterStartupScript(this.GetType(), "SucessoImportarCappta", "alert('Planos importados com sucesso');", true);


        }

    }
    protected void ddlAdquirentes_SelectedIndexChanged(object sender, EventArgs e)
    {
        dvAntecipado.Visible = false;
        if (ddlAdquirentes.SelectedValue.ToString() == "A")
        {
            dvAntecipado.Visible = true;
        }
    }
}