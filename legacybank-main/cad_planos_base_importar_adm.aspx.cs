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

public partial class cad_planos_base_importar_adm : System.Web.UI.Page
{
    public static DataTable dtPlanos;

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

            // Definição Tabela de Estabelecimentos
            dtPlanos = new DataTable();
            dtPlanos.Columns.Add("id", typeof(string));
            dtPlanos.Columns.Add("plano", typeof(string));

            CarregaAdquirentes();
        }
    }


    private void CarregaAdquirentes()
    {
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
    }

    protected void ckbTodos_CheckedChanged(object sender, EventArgs e)
    {
        foreach (ListViewItem itemP in lsvPlanos.Items)
        {
            ((CheckBox)itemP.FindControl("ckbImportar")).Checked = (((CheckBox)itemP.FindControl("ckbImportar")).Checked == false) ? true : false;
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

                    JObject oPlano = JObject.Parse(zoop.planos_referencia(((TextBox)itemP.FindControl("txtID")).Text.ToString()).ToString());
                    SqlConnection connInsCons = new SqlConnection(Funcoes.conexao());
                    connInsCons.Open();
                    SqlCommand cmdInsCons = new SqlCommand("dbo.stp_planos_referencia_ins", connInsCons);
                    cmdInsCons.CommandType = CommandType.StoredProcedure;
                    cmdInsCons.Parameters.Add("@flg_operacao", SqlDbType.Char).Value = 'I';
                    cmdInsCons.Parameters.Add("@COD_ID_PESSOA_LICENCIADO", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["LICENCIADO"].ToString());
                    cmdInsCons.Parameters.Add("@FLG_MODELO_PLANO_REFERENCIA", SqlDbType.Char).Value = "Z";
                    cmdInsCons.Parameters.Add("@FLG_ATIVO", SqlDbType.Char).Value = (oPlano["is_active"].ToString() == "True") ? "S" : "N";
                    cmdInsCons.Parameters.Add("@COD_ID_PLANO_REFERENCIA", SqlDbType.VarChar).Value = oPlano["id"].ToString();
                    cmdInsCons.Parameters.Add("@NOM_PLANO_REFERENCIA", SqlDbType.VarChar).Value = oPlano["name"].ToString();
                    cmdInsCons.Parameters.Add("@NOM_INVOICE_PLANO_REFERENCIA", SqlDbType.VarChar).Value = oPlano["invoice_name"].ToString();
                    cmdInsCons.Parameters.Add("@NOM_PERIODICIDADE", SqlDbType.VarChar).Value = oPlano["interval"].ToString();
                    cmdInsCons.Parameters.Add("@DES_PLANO_REFERENCIA", SqlDbType.Text).Value = oPlano["description"].ToString();
                    string sCodigoInclusao = cmdInsCons.ExecuteScalar().ToString();
                    connInsCons.Close();
                    connInsCons.Dispose();

                    for (int i = 0; i < oPlano["fee_details"].Count(); i++)
                    {
                        SqlConnection connInsConsTaxas = new SqlConnection(Funcoes.conexao());
                        connInsConsTaxas.Open();
                        SqlCommand cmdInsConsTaxas = new SqlCommand("dbo.stp_planos_referencia_taxas_ins", connInsConsTaxas);
                        cmdInsConsTaxas.CommandType = CommandType.StoredProcedure;
                        cmdInsConsTaxas.Parameters.Add("@flg_operacao", SqlDbType.Char).Value = 'I';
                        cmdInsConsTaxas.Parameters.Add("@COD_ID_PESSOA_LICENCIADO", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["LICENCIADO"].ToString());
                        cmdInsConsTaxas.Parameters.Add("@COD_ID_PLANOS_REFERENCIA", SqlDbType.Int).Value = Funcoes.strToInt(sCodigoInclusao);
                        cmdInsConsTaxas.Parameters.Add("@NOM_BANDEIRA", SqlDbType.VarChar).Value = oPlano["fee_details"][i]["card_brand"].ToString();
                        cmdInsConsTaxas.Parameters.Add("@NUM_VALOR_PERCENTUAL", SqlDbType.Float).Value = Funcoes.strToDouble(oPlano["fee_details"][i]["percent_amount"].ToString()) / 100;
                        cmdInsConsTaxas.Parameters.Add("@NOM_MOEDA", SqlDbType.VarChar).Value = oPlano["fee_details"][i]["currency"].ToString();
                        cmdInsConsTaxas.Parameters.Add("@NOM_TIPO_PAGAMENTO", SqlDbType.VarChar).Value = oPlano["fee_details"][i]["payment_type"].ToString();
                        cmdInsConsTaxas.Parameters.Add("@NUM_PARCELAS", SqlDbType.Int).Value = Funcoes.strToInt(oPlano["fee_details"][i]["number_installments"].ToString());
                        cmdInsConsTaxas.Parameters.Add("@NOM_MODO_CAPTURA", SqlDbType.VarChar).Value = oPlano["fee_details"][i]["capture_mode"].ToString();
                        cmdInsConsTaxas.Parameters.Add("@NOM_ORIGEM", SqlDbType.VarChar).Value = oPlano["fee_details"][i]["source"].ToString();
                        cmdInsConsTaxas.Parameters.Add("@FLG_PRE_PAGO", SqlDbType.Char).Value = (oPlano["fee_details"][i]["prepaid"].ToString() == "True") ? "S" : "N";
                        cmdInsConsTaxas.Parameters.Add("@NOM_TIPO", SqlDbType.VarChar).Value = oPlano["fee_details"][i]["type"].ToString();
                        cmdInsConsTaxas.ExecuteNonQuery();
                        connInsConsTaxas.Close();
                        connInsConsTaxas.Dispose();
                    }
                    ((CheckBox)itemP.FindControl("ckbImportar")).Checked = false;
                }
            }

            ckbTodos.Checked = false;
            ClientScript.RegisterStartupScript(this.GetType(), "SucessoImportarZoop", "alert('Planos importados com sucesso');", true);

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
                    SqlCommand cmdInsCons = new SqlCommand("dbo.stp_planos_referencia_ins", connInsCons);
                    cmdInsCons.CommandType = CommandType.StoredProcedure;
                    cmdInsCons.Parameters.Add("@flg_operacao", SqlDbType.Char).Value = 'I';

                    cmdInsCons.Parameters.Add("@COD_ID_PESSOA_LICENCIADO", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["LICENCIADO"].ToString());
                    cmdInsCons.Parameters.Add("@FLG_MODELO_PLANO_REFERENCIA", SqlDbType.Char).Value = "C";
                    cmdInsCons.Parameters.Add("@FLG_ATIVO", SqlDbType.Char).Value = "S";
                    cmdInsCons.Parameters.Add("@COD_ID_PLANO_REFERENCIA", SqlDbType.VarChar).Value = oPlanos["id"].ToString();
                    cmdInsCons.Parameters.Add("@NOM_PLANO_REFERENCIA", SqlDbType.VarChar).Value = oPlanos["name"].ToString();
                    cmdInsCons.Parameters.Add("@NOM_INVOICE_PLANO_REFERENCIA", SqlDbType.VarChar).Value = oPlanos["name"].ToString();
                    cmdInsCons.Parameters.Add("@NOM_PERIODICIDADE", SqlDbType.VarChar).Value = "Por transação";
                    cmdInsCons.Parameters.Add("@DES_PLANO_REFERENCIA", SqlDbType.Text).Value = oPlanos["name"].ToString();
                    string sCodigoInclusao = cmdInsCons.ExecuteScalar().ToString();
                    connInsCons.Close();
                    connInsCons.Dispose();


                    for (int t = 0; t < oPlanos["schemes"].Count(); t++)
                    {

                        for (int p = 0; p < oPlanos["schemes"][t]["fees"].Count(); p++)
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
                            cmdSelCadastro.Parameters.Add("@NOM_REFERENCIA", SqlDbType.VarChar).Value = oPlanos["schemes"][t]["scheme"].ToString();
                            SqlDataReader ReaderCadastro = cmdSelCadastro.ExecuteReader();
                            while (ReaderCadastro.Read())
                            {
                                iBandeira = Funcoes.strToInt(ReaderCadastro["COD_BANDEIRA"].ToString());
                                sBandeira = ReaderCadastro["NOM_BANDEIRA"].ToString();
                                sTipo = ReaderCadastro["NOM_TIPO"].ToString();
                            }

                            cmdInsConsTaxas.Parameters.Add("@COD_ID_BANDEIRAS", SqlDbType.Int).Value = iBandeira;
                            cmdInsConsTaxas.Parameters.Add("@NOM_BANDEIRA", SqlDbType.VarChar).Value = sBandeira;
                            cmdInsConsTaxas.Parameters.Add("@NUM_VALOR_PERCENTUAL", SqlDbType.Float).Value = Funcoes.strToDouble(oPlanos["schemes"][t]["fees"][p]["rate"].ToString()) / 100;
                            cmdInsConsTaxas.Parameters.Add("@NOM_MOEDA", SqlDbType.VarChar).Value = "BRL";

                            cmdInsConsTaxas.Parameters.Add("@NOM_TIPO_PAGAMENTO", SqlDbType.VarChar).Value = sTipo.ToString();

                            cmdInsConsTaxas.Parameters.Add("@NUM_PARCELAS", SqlDbType.Int).Value = Funcoes.strToInt(oPlanos["schemes"][t]["fees"][p]["installments"].ToString());
                            cmdInsConsTaxas.Parameters.Add("@NOM_MODO_CAPTURA", SqlDbType.VarChar).Value = "";
                            cmdInsConsTaxas.Parameters.Add("@NOM_ORIGEM", SqlDbType.VarChar).Value = "reseller";
                            cmdInsConsTaxas.Parameters.Add("@FLG_PRE_PAGO", SqlDbType.Char).Value = "N";
                            cmdInsConsTaxas.Parameters.Add("@NOM_TIPO", SqlDbType.VarChar).Value = oPlanos["schemes"][t]["scheme"].ToString() + "_" + sBandeira.ToString() + "_" + sTipo.ToString() + "_" + oPlanos["schemes"][t]["fees"][p]["installments"].ToString();
                            cmdInsConsTaxas.ExecuteNonQuery();
                            connInsConsTaxas.Close();
                            connInsConsTaxas.Dispose();
                        }
                    }

                    ((CheckBox)itemP.FindControl("ckbImportar")).Checked = false;
                }
            }

            ckbTodos.Checked = false;
            // Mensagem de Importação
            ClientScript.RegisterStartupScript(this.GetType(), "SucessoImportarCappta", "alert('Planos importados com sucesso');", true);

        }


    }
    protected void btnCarregarCadastros_Click(object sender, EventArgs e)
    {
        dtPlanos.Rows.Clear();

        if (ddlAdquirentes.SelectedValue.ToString().Trim() == "Z")
        {
            JObject oPlano = JObject.Parse(zoop.planos_referencia("").ToString());

            for (int i = 0; i < oPlano["items"].Count(); i++)
            {
                dtPlanos.Rows.Add(oPlano["items"][i]["id"].ToString(), oPlano["items"][i]["name"].ToString());
            }

            this.lsvPlanos.DataSource = dtPlanos;
            this.lsvPlanos.DataBind();

            ClientScript.RegisterStartupScript(this.GetType(), "SucessoCarregarDadosZoop", "alert('Planos carregados com sucesso');", true);
        }

        if (ddlAdquirentes.SelectedValue.ToString().Trim() == "C")
        {
            dtPlanos.Rows.Clear();
            JObject oPlanosReferencia = JObject.Parse(hubcappta.ConsultarPlanos(1, "true", "Reseller"));

            if (Funcoes.strToInt(oPlanosReferencia["lastPage"].ToString()) > 0)
            {
                for (int i = 0; i < Funcoes.strToInt(oPlanosReferencia["lastPage"].ToString()); i++)
                {
                    oPlanosReferencia = JObject.Parse(hubcappta.ConsultarPlanos(i, "true", "Reseller"));
                    if (oPlanosReferencia["plans"].Count() > 0)
                    {
                        for (int x = 0; x < oPlanosReferencia["plans"].Count(); x++)
                        {
                            dtPlanos.Rows.Add(oPlanosReferencia["plans"][x]["id"].ToString().Trim(), oPlanosReferencia["plans"][x]["name"].ToString().Trim());
                        }
                    }

                }
            }
            this.lsvPlanos.DataSource = dtPlanos;
            this.lsvPlanos.DataBind();

            ClientScript.RegisterStartupScript(this.GetType(), "SucessoCarregarDadosCappta", "alert('Planos carregados com sucesso');", true);
        }

    }
    protected void lsvPlanos_ItemCommand(object sender, ListViewCommandEventArgs e)
    {

    }
    protected void btnFechar_Click(object sender, EventArgs e)
    {
        ClientScript.RegisterStartupScript(this.GetType(), "FecharPlanosBaseImportar", "opener.PostBackOnMainPage(); window.close(); ", true);

    }
}