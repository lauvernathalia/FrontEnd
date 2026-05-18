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


public partial class cad_planos_exportar : System.Web.UI.Page
{

    public string sid_id
    {
        get
        {
            try { return Funcoes.Decrypt(Request["id"]); }
            catch { return ""; }
        }
    }
    protected void Page_Load(object sender, EventArgs e)
    {
        if (HttpContext.Current.Session.Count <= 0)
        {
            FormsAuthentication.SignOut();
            ClientScript.RegisterStartupScript(this.GetType(), "SessaoEncerrada", "opener.PostBackOnMainPage(); window.close(); ", true);
        }
        
        if (!IsPostBack)
        {
            CarregaAdquirentes();
            ConsultaFicha();
            PermissaoExportar();
            ddlAdquirentes.Enabled = false;
            ddlPlanosReferencia.Enabled = false;
        }
    }

    private void PermissaoExportar()
    {
        dvBloqueado.Visible=false;
        dvPermitido.Visible = false;
        if (ddlAdquirentes.SelectedValue.ToString().Trim() == "C") { dvPermitido.Visible = true; dvBloqueado.Visible = false; } else { dvPermitido.Visible = false; dvBloqueado.Visible = true; }
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
            lblIDReferencia.Text = ReaderCadastro["NOM_REFERENCIA_PLANO"].ToString();
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


    protected void ddlAdquirentes_SelectedIndexChanged(object sender, EventArgs e)
    {
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

    
    protected void btnFechar_Click(object sender, EventArgs e)
    {
        ClientScript.RegisterStartupScript(this.GetType(),
"Alerta", "opener.PostBackOnMainPage(); window.close(); ", true);

    }
    protected void btnExportar_Click(object sender, EventArgs e)
    {
        // Verificar 
        
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
            
            hubcappta.DadosPlano.Root dplano = new hubcappta.DadosPlano.Root()
            {
                Name = ReaderCadastro["NOM_TITULO_PLANO"].ToString(),
                product = "POS",
                type = "Reseller",
                resellerDocument = ReaderCadastro["NOM_CNPJ_LICENCIADO"].ToString(),
                settlementDays = Funcoes.strToInt(ReaderCadastro["NUM_DIAS_LIQUIDACAO"].ToString())
            };
            
            List<hubcappta.DadosPlano.Scheme> listaScheme = new List<hubcappta.DadosPlano.Scheme>();
            
            // Leitura das Taxas

            SqlConnection mySelCadastroTaxas = new SqlConnection(Funcoes.conexao());
            mySelCadastroTaxas.Open();
            SqlCommand cmdSelCadastroTaxas = new SqlCommand("dbo.stp_opcoes_planos_cappta_ins", mySelCadastroTaxas);
            cmdSelCadastroTaxas.CommandType = CommandType.StoredProcedure;
            cmdSelCadastroTaxas.Parameters.Add("@flg_operacao", SqlDbType.Char).Value = "E";
            cmdSelCadastroTaxas.Parameters.Add("@COD_ID_PESSOA_LICENCIADO", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["LICENCIADO"].ToString());
            cmdSelCadastroTaxas.Parameters.Add("@COD_ID_PLANOS", SqlDbType.Int).Value = Funcoes.strToInt(sid_id.ToString());
            cmdSelCadastroTaxas.Parameters.Add("@FLG_PRESENCIAL_ONLINE", SqlDbType.Char).Value = "P";
            SqlDataReader ReaderCadastroTaxas = cmdSelCadastroTaxas.ExecuteReader();
            while (ReaderCadastroTaxas.Read())
            {
                int schemeId = Funcoes.strToInt(ReaderCadastroTaxas["COD_ID_OPCOES_PLANOS_CAPPTA"].ToString());
                List<hubcappta.DadosPlano.Fee> listaFee = new List<hubcappta.DadosPlano.Fee>();


                if (ReaderCadastroTaxas["FLG_TIPO"].ToString().Trim() == "V")
                {
                    listaFee.Add(new hubcappta.DadosPlano.Fee()
                    {
                        installments = 1,
                        rate = Funcoes.strToDouble(ReaderCadastroTaxas["NUM_VALOR_FINAL_DEBITO"].ToString())
                    });
                }
                //NUM_VALOR_FINAL_DEBITO
                if (ReaderCadastroTaxas["FLG_TIPO"].ToString().Trim() == "C")
                {
                    if (Funcoes.strToDouble(ReaderCadastroTaxas["NUM_VALOR_FINAL_VISTA"].ToString().Trim()) > 0)
                    {
                        listaFee.Add(new hubcappta.DadosPlano.Fee()
                        {
                            installments = 1,
                            rate = Funcoes.strToDouble(ReaderCadastroTaxas["NUM_VALOR_FINAL_VISTA"].ToString())
                        });
                    }
                    for (int i = 2; i < 18; i++)
                    {
                        if (Funcoes.strToDouble(ReaderCadastroTaxas["NUM_VALOR_FINAL_" + i.ToString() + "X"].ToString().Trim()) > 0)
                        {
                            listaFee.Add(new hubcappta.DadosPlano.Fee()
                            {
                                installments = i,
                                rate = Funcoes.strToDouble(ReaderCadastroTaxas["NUM_VALOR_FINAL_"+i.ToString()+"X"].ToString())
                            });
                        }
                    }
                }

                listaScheme.Add(new hubcappta.DadosPlano.Scheme()
                {
                    id = schemeId,
                    fees = listaFee.ToArray()
                });
            }

            dplano.schemes = listaScheme.ToArray();

            string jsonPlano = JsonConvert.SerializeObject(dplano);
            // Enviar para CAPPTA
            string jsonRetorno = hubcappta.CadastrarNovoPlano(jsonPlano, lblIDReferencia.Text.ToString().Trim());

            if (lblIDReferencia.Text.ToString().Trim() == "")
            {
                // Inclusão - atualizar plano com NOM_REFERENCIA_PLANO
                try
                {
                    JObject oPlano = JObject.Parse(jsonRetorno.ToString());

                    SqlConnection connInsCons = new SqlConnection(Funcoes.conexao());
                    connInsCons.Open();
                    SqlCommand cmdInsCons = new SqlCommand("dbo.stp_opcoes_planos_cappta_ins", connInsCons);
                    cmdInsCons.CommandType = CommandType.StoredProcedure;
                    cmdInsCons.Parameters.Add("@COD_ID_PESSOA_LICENCIADO", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["LICENCIADO"].ToString());
                    cmdInsCons.Parameters.Add("@COD_ID_PLANOS", SqlDbType.Int).Value = Funcoes.strToInt(sid_id.ToString());
                    cmdInsCons.Parameters.Add("@NOM_REFERENCIA_PLANO", SqlDbType.VarChar).Value = oPlano["id"].ToString();
                    cmdInsCons.ExecuteNonQuery();
                    connInsCons.Close();
                    connInsCons.Dispose();

                    ClientScript.RegisterStartupScript(this.GetType(), "Mensagensdesucesso", "alert('Plano exportado com sucesso!');", true);
                }
                catch
                {
                    ClientScript.RegisterStartupScript(this.GetType(), "Mensagensdeerro", "alert('Ocorreu um erro ao tentar exportar o plano! Verifique e tente novamente.');", true);

                }
            }
            else
            {
                try
                {
                    JObject oPlano = JObject.Parse(jsonRetorno.ToString());
                    //if (oPlano["hasError"].ToString().Trim() == "false")
                    //{
                        ClientScript.RegisterStartupScript(this.GetType(), "MensagensdesucessoAlteracao", "alert('Plano exportado e atualizado com sucesso!');", true);
                    //}
                }
                catch
                {
                    ClientScript.RegisterStartupScript(this.GetType(), "MensagensdeerroAlteracao", "alert('Ocorreu um erro ao tentar exportar e atualizar o plano! Verifique e tente novamente.');", true);

                }
            }
            //txtExportacao.Text = jsonRetorno;
        }
        
    }
}