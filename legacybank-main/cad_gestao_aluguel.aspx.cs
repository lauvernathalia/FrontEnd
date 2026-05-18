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


public partial class cad_gestao_aluguel : System.Web.UI.Page
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
        // Verifica se o usuário esta autenticado
        if (HttpContext.Current.Session.Count <= 0)
        {
            Response.Redirect("login.aspx");
            FormsAuthentication.SignOut();
        }

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

            if (HttpContext.Current.Session["TIPO"].ToString() == "E")
            {
                ddlEstabelecimento.SelectedValue = HttpContext.Current.Session["PESSOA"].ToString();
                ddlEstabelecimento.Enabled = false;
            }

            if (Funcoes.strToInt(sid_id) != 0)
            {
                ConsultaFicha();
            }
        }

    }

    private void ConsultaFicha()
    {
        SqlConnection mySelCadastro = new SqlConnection(Funcoes.conexao());
        mySelCadastro.Open();
        SqlCommand cmdSelCadastro = new SqlCommand("dbo.stp_gestao_aluguel_ins", mySelCadastro);
        cmdSelCadastro.CommandType = CommandType.StoredProcedure;
        cmdSelCadastro.Parameters.Add("@flg_operacao", SqlDbType.Char).Value = "S";
        cmdSelCadastro.Parameters.Add("@COD_ID", SqlDbType.Int).Value = Funcoes.strToInt(sid_id.ToString());
        cmdSelCadastro.Parameters.Add("@COD_ID_PESSOA_LICENCIADO", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["LICENCIADO"].ToString());
        SqlDataReader ReaderCadastro = cmdSelCadastro.ExecuteReader();
        while (ReaderCadastro.Read())
        {
            txtID.Text = ReaderCadastro["COD_ID"].ToString();
            ddlEstabelecimento.SelectedValue = ReaderCadastro["COD_ID_PESSOAS_FJ"].ToString();

            txtFaturamentoMinimo.Text = ReaderCadastro["NUM_FATURAMENTO_MINIMO"].ToString();

            if (ReaderCadastro["DTA_PRIMEIRA_COBRANCA"].ToString().Trim() != "")
            {
                txtDataPrimeiraCobranca.Text = Convert.ToDateTime(ReaderCadastro["DTA_PRIMEIRA_COBRANCA"].ToString()).ToShortDateString();
            }

            txtValor.Text = ReaderCadastro["NUM_VALOR"].ToString();
            ddlModelo.SelectedValue = ReaderCadastro["FLG_MODELO"].ToString();
            ddlTipo.SelectedValue = ReaderCadastro["FLG_TIPO"].ToString();
        }

    }


    protected void btnSalvar_Click(object sender, EventArgs e)
    {
        // Grava os dados

        SqlConnection connInsCons = new SqlConnection(Funcoes.conexao());
        connInsCons.Open();
        SqlCommand cmdInsCons = new SqlCommand("dbo.stp_gestao_aluguel_ins", connInsCons);
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

        if (HttpContext.Current.Session["TIPO"].ToString() == "E")
        {
            cmdInsCons.Parameters.Add("@COD_ID_PESSOAS_FJ", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["PESSOA"].ToString());
        }
        else
        {
            cmdInsCons.Parameters.Add("@COD_ID_PESSOAS_FJ", SqlDbType.Int).Value = Funcoes.strToInt(ddlEstabelecimento.SelectedValue.ToString());
        }


        cmdInsCons.Parameters.Add("@NUM_FATURAMENTO_MINIMO", SqlDbType.Float).Value = Funcoes.strToDouble(txtFaturamentoMinimo.Text.ToString());
        cmdInsCons.Parameters.Add("@NUM_VALOR", SqlDbType.Float).Value = Funcoes.strToDouble(txtValor.Text.ToString());

        cmdInsCons.Parameters.Add("@DTA_DATA", SqlDbType.DateTime).Value = DateTime.Now;
        cmdInsCons.Parameters.Add("@FLG_MODELO", SqlDbType.Char).Value = ddlModelo.SelectedValue.ToString();
        cmdInsCons.Parameters.Add("@FLG_TIPO", SqlDbType.VarChar).Value = ddlTipo.SelectedValue.ToString();
        if (txtDataPrimeiraCobranca.Text.ToString().Trim() != "")
        {
            cmdInsCons.Parameters.Add("@DTA_PRIMEIRA_COBRANCA", SqlDbType.DateTime).Value = Convert.ToDateTime(txtDataPrimeiraCobranca.Text.ToString());
        }

        cmdInsCons.ExecuteNonQuery();
        connInsCons.Close();
        connInsCons.Dispose();

        ClientScript.RegisterStartupScript(this.GetType(),
            "Alerta", "alert('Dados gravados com sucesso'); opener.PostBackOnMainPage(); window.close();", true);


        // Envia dados ZOOP

        /*

        ServicePointManager.Expect100Continue = true;
        ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12
        | SecurityProtocolType.Tls11
        | SecurityProtocolType.Tls
        | SecurityProtocolType.Ssl3;

        var byteArray = Encoding.ASCII.GetBytes(HttpContext.Current.Session["USERNAMEMARKETPLACE"].ToString() + ":" + "");
        string encodeString = Convert.ToBase64String(byteArray);

        var myUri = new Uri("https://api.zoop.ws/v1/marketplaces/" + HttpContext.Current.Session["IDMARKETPLACE"].ToString() + "/terminals/pairing");
        var myWebRequest = WebRequest.Create(myUri);
        var myHttpWebRequest = (HttpWebRequest)myWebRequest;
        myHttpWebRequest.ContentType = "application/json";
        myHttpWebRequest.Accept = "application/json";
        myHttpWebRequest.Method = "POST";
        myHttpWebRequest.PreAuthenticate = true;
        myHttpWebRequest.Headers.Add("Authorization", "Basic " + encodeString.ToString() + "");

        string json = "";
        string resultado = "";
        using (var streamWriter = new StreamWriter(myHttpWebRequest.GetRequestStream()))
        {
            json = json + "{";

            if (HttpContext.Current.Session["TIPO"].ToString() == "E")
            {
                json = json + "\"seller\":\"" + HttpContext.Current.Session["TOKENZOOP"].ToString() + "\",";
            }
            else
            {
                //Buscar Token

                SqlConnection connVerificaUsuario = new SqlConnection(Funcoes.conexao());
                SqlCommand cmdVerificaUsuario = new SqlCommand("dbo.stp_pessoas_fj_ins", connVerificaUsuario);
                cmdVerificaUsuario.CommandType = CommandType.StoredProcedure;
                cmdVerificaUsuario.Parameters.Add("@flg_operacao", SqlDbType.Char).Value = "S";
                cmdVerificaUsuario.Parameters.Add("@COD_ID", SqlDbType.Int).Value = Funcoes.strToInt(ddlEstabelecimento.SelectedValue.ToString());
                cmdVerificaUsuario.Parameters.Add("@COD_ID_PESSOA_LICENCIADO", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["LICENCIADO"].ToString());
                connVerificaUsuario.Open();
                SqlDataReader mReader = cmdVerificaUsuario.ExecuteReader();

                FormsAuthentication.SignOut();
                if (mReader.Read())
                {
                    json = json + "\"seller\":\"" + mReader["NUM_TOKEN"].ToString() + "\",";
                }
            }
            json = json + "\"marketplace_id\": true,";
            json = json + "\"token\":\"" + txtToken.Text.ToString() + "\",";
            json = json + "\"isStaging\": false";
            json = json + "}";

            streamWriter.Write(json);
            streamWriter.Flush();
        }
        try
        {
            var myWebResponse = myWebRequest.GetResponse();
            var responseStream = myWebResponse.GetResponseStream();

            StreamReader myStreamReader = new StreamReader(responseStream, Encoding.UTF8);
            var resp = myStreamReader.ReadToEnd();
            resultado = resp.ToString();
        }
        catch (WebException ex)
        {
            var resp = new StreamReader(ex.Response.GetResponseStream()).ReadToEnd();
            resultado = resp.ToString();
            JObject o = JObject.Parse(resp);
        }



        */
    }



    protected void btnCancelar_Click(object sender, EventArgs e)
    {
        ClientScript.RegisterStartupScript(this.GetType(),
    "Alerta", "opener.PostBackOnMainPage(); window.close(); ", true);
    }
}