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

public partial class con_carrega_transacoes_estabelecimento : System.Web.UI.Page
{
    public static DataTable dtTransacoes;


    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {

            SqlConnection myEstabelecimentos = new SqlConnection(Funcoes.conexao());
            myEstabelecimentos.Open();
            SqlCommand cmdEstabelecimentos = new SqlCommand("dbo.stp_pessoas_fj_ins", myEstabelecimentos);
            cmdEstabelecimentos.CommandType = CommandType.StoredProcedure;
            cmdEstabelecimentos.Parameters.Add("@flg_operacao", SqlDbType.VarChar).Value = "Y";

            cmdEstabelecimentos.Parameters.Add("@COD_ID_PESSOA_LICENCIADO", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["LICENCIADO"].ToString());
            cmdEstabelecimentos.Parameters.Add("@NOM_RAZAOSOCIAL", SqlDbType.VarChar).Value = " ";
            cmdEstabelecimentos.Parameters.Add("@NUM_DOCUMENTO", SqlDbType.VarChar).Value = " ";
            cmdEstabelecimentos.Parameters.Add("@NUM_TOKEN", SqlDbType.VarChar).Value = " ";
            cmdEstabelecimentos.Parameters.Add("@FLG_ATIVO", SqlDbType.Char).Value = " ";
            cmdEstabelecimentos.Parameters.Add("@FLG_TIPO", SqlDbType.Char).Value = "E";

            if (HttpContext.Current.Session["TIPO"].ToString() == "M")
            {
                cmdEstabelecimentos.Parameters.Add("@COD_ID_MARKETPLACE", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["PESSOA"].ToString());
            }
            if (HttpContext.Current.Session["TIPO"].ToString() == "R")
            {
                cmdEstabelecimentos.Parameters.Add("@COD_ID_REPRESENTANTE", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["PESSOA"].ToString());
            }


            SqlDataAdapter drEstabelecimentos = new SqlDataAdapter();
            drEstabelecimentos.SelectCommand = cmdEstabelecimentos;
            DataSet dsEstabelecimentos = new DataSet();
            drEstabelecimentos.Fill(dsEstabelecimentos, "PESSOAS_FJ");
            ddlEstabelecimento.DataTextField = "NOM_RAZAOSOCIAL";
            ddlEstabelecimento.DataValueField = "COD_ID";
            ddlEstabelecimento.DataSource = dsEstabelecimentos.Tables["PESSOAS_FJ"].DefaultView;
            ddlEstabelecimento.DataBind();
            ddlEstabelecimento.Items.Insert(0, new ListItem("Todos", ""));
            myEstabelecimentos.Close();
            myEstabelecimentos.Dispose();

        }

    }

    public string GetPostBackScript()
    {
        PostBackOptions options = new PostBackOptions(btnPostback);
        Page.ClientScript.RegisterForEventValidation(options);

        return Page.ClientScript.GetPostBackEventReference(options);
    }

    protected void btnPostBack_Click(object sender, EventArgs e)
    {
        
    }

    protected void btnCarregar_Click(object sender, EventArgs e)
    {
        try
        {
            dtTransacoes = new DataTable();
            dtTransacoes.Columns.Add("id", typeof(string));
            dtTransacoes.Columns.Add("data", typeof(string));
            dtTransacoes.Columns.Add("valor", typeof(string));
            
            txtJson.Text = "";

            SqlConnection mySelCadastro = new SqlConnection(Funcoes.conexao());
            mySelCadastro.Open();
            SqlCommand cmdSelCadastro = new SqlCommand("dbo.stp_pessoas_fj_ins", mySelCadastro);
            cmdSelCadastro.CommandType = CommandType.StoredProcedure;
            cmdSelCadastro.Parameters.Add("@flg_operacao", SqlDbType.Char).Value = "S";
            cmdSelCadastro.Parameters.Add("@COD_ID", SqlDbType.Int).Value = Funcoes.strToInt(ddlEstabelecimento.SelectedValue.ToString());
            cmdSelCadastro.Parameters.Add("@COD_ID_PESSOA_LICENCIADO", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["LICENCIADO"].ToString());
            SqlDataReader ReaderCadastro = cmdSelCadastro.ExecuteReader();
            while (ReaderCadastro.Read())
            {
                ServicePointManager.Expect100Continue = true;
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12
                | SecurityProtocolType.Tls11
                | SecurityProtocolType.Tls
                | SecurityProtocolType.Ssl3;

                // Convert to Base64

                int iOffSet = 0;
                int iLimit = 100;

                bool bhas_more = true;

                while (bhas_more == true)
                {
                    var byteArray = Encoding.ASCII.GetBytes(HttpContext.Current.Session["USERNAMEMARKETPLACE"].ToString() + ":" + "");
                    string encodeString = Convert.ToBase64String(byteArray);
                    string sBusca = "";

                    var dData = Convert.ToDateTime(txtDataInicio.Text.ToString()).Year.ToString() + "-" + Convert.ToDateTime(txtDataInicio.Text.ToString()).Month.ToString().PadLeft(2, '0') + "-" + Convert.ToDateTime(txtDataInicio.Text.ToString()).Day.ToString().PadLeft(2, '0');

                    var myUri = new Uri("https://api.zoop.ws/v1/marketplaces/" + HttpContext.Current.Session["IDMARKETPLACE"].ToString() + "/sellers/" + ReaderCadastro["NUM_TOKEN"].ToString() + "/transactions?limit=" + iLimit.ToString() + "&page=1&sort=time-descending&offset=" + iOffSet.ToString() + "&date_range[gte]=" + dData.ToString());
                    var myWebRequest = WebRequest.Create(myUri);
                    var myHttpWebRequest = (HttpWebRequest)myWebRequest;
                    myHttpWebRequest.PreAuthenticate = true;
                    myHttpWebRequest.Headers.Add("Authorization", "Basic " + encodeString.ToString() + "");
                    myHttpWebRequest.Accept = "application/json";

                    var myWebResponse = myWebRequest.GetResponse();
                    var responseStream = myWebResponse.GetResponseStream();


                    StreamReader myStreamReader = new StreamReader(responseStream, Encoding.UTF8);
                    var json = myStreamReader.ReadToEnd();

                    txtJson.Text = txtJson.Text.ToString() + json.ToString();

                    JObject o = JObject.Parse(json);

                    bhas_more = Convert.ToBoolean(o["has_more"].ToString());
                    int iTransacoes = o["items"].Count();
                    for (int i = 0; i < o["items"].Count(); i++)
                    {

                        // Transacoes
                        SqlConnection connInsCons = new SqlConnection(Funcoes.conexao());
                        connInsCons.Open();
                        SqlCommand cmdInsCons = new SqlCommand("dbo.stp_transacoes_ins", connInsCons);
                        cmdInsCons.CommandType = CommandType.StoredProcedure;
                        cmdInsCons.Parameters.Add("@flg_operacao", SqlDbType.Char).Value = 'I';

                        cmdInsCons.Parameters.Add("@COD_ID_PESSOA_LICENCIADO", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["LICENCIADO"].ToString());
                        cmdInsCons.Parameters.Add("@COD_ID_PESSOAS_FJ", SqlDbType.Int).Value = Funcoes.strToInt(ddlEstabelecimento.SelectedValue.ToString());
                        cmdInsCons.Parameters.Add("@FLG_ORIGEM", SqlDbType.Char).Value = "Z";

                        cmdInsCons.Parameters.Add("@DTA_DATA", SqlDbType.DateTime).Value = Convert.ToDateTime(o["items"][i]["created_at"].ToString());
                        cmdInsCons.Parameters.Add("@NOM_CODE", SqlDbType.VarChar).Value = o["items"][i]["id"].ToString();
                        cmdInsCons.Parameters.Add("@NUM_TOKEN", SqlDbType.VarChar).Value = o["items"][i]["on_behalf_of"].ToString();

                        cmdInsCons.Parameters.Add("@COD_ID_TIPO", SqlDbType.Int).Value = 1;
                        cmdInsCons.Parameters.Add("@NOM_STATUS", SqlDbType.VarChar).Value = o["items"][i]["status"].ToString();

                        if ((o["items"][i]["point_of_sale"]["identification_number"].ToString().Trim() == "") || (o["items"][i]["point_of_sale"]["identification_number"].ToString().Trim() == "null"))
                        {
                            cmdInsCons.Parameters.Add("@FLG_PRESENCIAL_ONLINE", SqlDbType.Char).Value = "O";
                        }
                        else
                        {
                            cmdInsCons.Parameters.Add("@FLG_PRESENCIAL_ONLINE", SqlDbType.Char).Value = "P";
                        }

                        cmdInsCons.Parameters.Add("@NOM_TIPO_PAGAMENTO", SqlDbType.VarChar).Value = o["items"][i]["payment_type"].ToString();

                        if ((o["items"][i]["payment_type"].ToString() == "pix"))
                        {
                            cmdInsCons.Parameters.Add("@NOM_CODIGO_PAGAMENTO", SqlDbType.VarChar).Value = o["items"][i]["payment_method"]["provider"].ToString();
                            cmdInsCons.Parameters.Add("@NUM_PARCELAS", SqlDbType.Int).Value = 1;
                        }
                        if ((o["items"][i]["payment_type"].ToString() == "boleto"))
                        {
                            cmdInsCons.Parameters.Add("@NOM_CODIGO_PAGAMENTO", SqlDbType.VarChar).Value = o["items"][i]["payment_method"]["resource"].ToString();
                            cmdInsCons.Parameters.Add("@NUM_PARCELAS", SqlDbType.Int).Value = 1;
                        }


                        if ((o["items"][i]["payment_type"].ToString() == "credit"))
                        {
                            cmdInsCons.Parameters.Add("@NOM_CODIGO_PAGAMENTO", SqlDbType.VarChar).Value = o["items"][i]["payment_method"]["resource"].ToString() + " " + o["items"][i]["payment_method"]["card_brand"].ToString();
                            if ((o["items"][i]["installment_plan"].ToString().Trim() != "null") && (o["items"][i]["installment_plan"].ToString().Trim() != ""))
                            {
                                cmdInsCons.Parameters.Add("@NUM_PARCELAS", SqlDbType.Int).Value = Funcoes.strToInt(o["items"][i]["installment_plan"]["number_installments"].ToString());
                            }
                            else
                            {
                                cmdInsCons.Parameters.Add("@NUM_PARCELAS", SqlDbType.Int).Value = 1;//Funcoes.strToInt(o["items"][i]["installment_plan"]["number_installments"].ToString());

                            }

                        }

                        if ((o["items"][i]["payment_type"].ToString() == "debit"))
                        {
                            cmdInsCons.Parameters.Add("@NOM_CODIGO_PAGAMENTO", SqlDbType.VarChar).Value = o["items"][i]["payment_method"]["resource"].ToString() + " " + o["items"][i]["payment_method"]["card_brand"].ToString();
                            cmdInsCons.Parameters.Add("@NUM_PARCELAS", SqlDbType.Int).Value = 1;

                        }


                        cmdInsCons.Parameters.Add("@NUM_VALOR_BRUTO", SqlDbType.Float).Value = Funcoes.strToDouble(o["items"][i]["amount"].ToString()) / 100;
                        cmdInsCons.Parameters.Add("@NUM_VALOR_DESCONTO", SqlDbType.Float).Value = 0;
                        cmdInsCons.Parameters.Add("@NUM_VALOR_INTERMEDIACAO_RATE", SqlDbType.Float).Value = 0;
                        cmdInsCons.Parameters.Add("@NUM_VALOR_INTERMEDIACAO_FEE", SqlDbType.Float).Value = Funcoes.strToDouble(o["items"][i]["fees"].ToString()) / 100;
                        cmdInsCons.Parameters.Add("@NUM_VALOR_LIQUIDO", SqlDbType.Float).Value = (Funcoes.strToDouble(o["items"][i]["amount"].ToString()) / 100) - (Funcoes.strToDouble(o["items"][i]["fees"].ToString()) / 100);
                        cmdInsCons.Parameters.Add("@NUM_VALOR_EXTRAS", SqlDbType.Float).Value = 0;

                        cmdInsCons.Parameters.Add("@DTA_DATA_DEPOSITO", SqlDbType.DateTime).Value = Convert.ToDateTime(o["items"][i]["created_at"].ToString());

                        cmdInsCons.Parameters.Add("@NUM_ITENS", SqlDbType.Int).Value = 1;

                        cmdInsCons.Parameters.Add("@NOM_FONTE_CANCELAMENTO", SqlDbType.VarChar).Value = "";
                        cmdInsCons.Parameters.Add("@NOM_ORIGEM", SqlDbType.VarChar).Value = o["items"][i]["gateway_authorizer"].ToString();

                        if (o["items"][i]["payment_method"].Contains("holder_name") == true)
                        {
                            cmdInsCons.Parameters.Add("@NOM_CLIENTE", SqlDbType.VarChar).Value = o["items"][i]["payment_method"]["holder_name"].ToString();
                        }

                        cmdInsCons.Parameters.Add("@NOM_AUTORIZACAO", SqlDbType.VarChar).Value = o["items"][i]["transaction_number"].ToString();
                        cmdInsCons.Parameters.Add("@DES_DESCRICAO", SqlDbType.Text).Value = o["items"][i]["payment_type"].ToString();

                        cmdInsCons.ExecuteNonQuery();
                        connInsCons.Close();
                        connInsCons.Dispose();



                        dtTransacoes.Rows.Add(o["items"][i]["id"].ToString(), Convert.ToDateTime(o["items"][i]["created_at"].ToString()).ToShortDateString(), (Funcoes.strToDouble(o["items"][i]["amount"].ToString()) / 100).ToString());

                        this.lsvTransacoes.DataSource = dtTransacoes;
                        this.lsvTransacoes.DataBind();

                    }

                    ClientScript.RegisterStartupScript(this.GetType(), "Importacao", "alert('Foram importadas " + iTransacoes.ToString() + " transações');  ", true);


                    if (bhas_more == true)
                    {
                        iOffSet = iOffSet + 100;
                    }
                    responseStream.Close();
                    myWebResponse.Close();
                }
            }
        }
        catch
        {
            ClientScript.RegisterStartupScript(this.GetType(), "Erro", "alert('Não foi possível realizar a importação das transações! Verifique e tente novamente');  ", true);

        }

    }
}