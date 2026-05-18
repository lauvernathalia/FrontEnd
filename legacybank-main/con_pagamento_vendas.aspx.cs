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


public partial class con_pagamento_vendas : System.Web.UI.Page
{
    public static DataTable dtTransacoes = new DataTable("DTTransacoes");

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
            DateTime data = DateTime.Today;
            DateTime primeiroDiaDoMes = new DateTime(data.Year, data.Month, 1);
            DateTime ultimoDiaDoMes = new DateTime(data.Year, data.Month, DateTime.DaysInMonth(data.Year, data.Month));

            txtDataIni.Text = primeiroDiaDoMes.ToShortDateString();
            txtDataFim.Text = DateTime.Now.ToShortDateString();

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

            ConsultaGeral();

        }

    }
    protected void btnPesquisar_Click(object sender, EventArgs e)
    {
        ConsultaGeral();
    }

    private void ConsultaGeral()
    {
        SqlConnection myConsulta = new SqlConnection(Funcoes.conexao());
        myConsulta.Open();
        SqlDataAdapter SDAConsulta = new SqlDataAdapter("dbo.stp_extrato_ins", myConsulta);
        SDAConsulta.SelectCommand.CommandType = CommandType.StoredProcedure;
        SDAConsulta.SelectCommand.Parameters.Add("@flg_operacao", SqlDbType.Char).Value = "C";
        SDAConsulta.SelectCommand.Parameters.Add("@COD_ID_PESSOA_LICENCIADO", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["LICENCIADO"].ToString());
        SDAConsulta.SelectCommand.Parameters.Add("@COD_ID_PESSOAS_FJ", SqlDbType.Int).Value = Funcoes.strToInt(ddlEstabelecimento.SelectedValue.ToString());
        SDAConsulta.SelectCommand.Parameters.Add("@DTA_INICIO", SqlDbType.DateTime).Value = Convert.ToDateTime(txtDataIni.Text.ToString());
        SDAConsulta.SelectCommand.Parameters.Add("@DTA_FIM", SqlDbType.DateTime).Value = Convert.ToDateTime(txtDataFim.Text.ToString());
        SDAConsulta.SelectCommand.Parameters.Add("@FLG_STATUS", SqlDbType.VarChar).Value = "";
        SDAConsulta.SelectCommand.Parameters.Add("@FLG_RECURSO", SqlDbType.VarChar).Value = "transfer";

        DataSet dsConsulta = new DataSet();
        SDAConsulta.Fill(dsConsulta, "EXTRATO");
        rptConsulta.DataSource = dsConsulta.Tables["EXTRATO"].DefaultView;
        rptConsulta.DataBind();
        myConsulta.Close(); myConsulta.Dispose();

    }

    protected void btnExportar_Click(object sender, EventArgs e)
    {

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
    protected void rptConsulta_OnItemCommand(object source, RepeaterCommandEventArgs e)
    {

    }
    protected void rptConsulta_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {

        dtTransacoes = new DataTable("DTTransacoes");
        dtTransacoes.Columns.Add("id", typeof(string));
        dtTransacoes.Columns.Add("data", typeof(string));
        dtTransacoes.Columns.Add("tipo", typeof(string));
        dtTransacoes.Columns.Add("valor", typeof(string));

        //((Repeater)e.Item.FindControl("rptConsultaTransacoes")).Visible=true;

        string tTransferencia = ((TextBox)e.Item.FindControl("txtidtransferencia")).Text.ToString();

        if (tTransferencia.ToString().Trim()!="")
        {
            try
            {
                ServicePointManager.Expect100Continue = true;
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12
                | SecurityProtocolType.Tls11
                | SecurityProtocolType.Tls
                | SecurityProtocolType.Ssl3;

                var byteArray = Encoding.ASCII.GetBytes(HttpContext.Current.Session["USERNAMEMARKETPLACE"].ToString() + ":" + "");
                string encodeString = Convert.ToBase64String(byteArray);
                string sBusca = "";

                var myUri = new Uri("https://api.zoop.ws/v1/marketplaces/" + HttpContext.Current.Session["IDMARKETPLACE"].ToString() + "/transfers/" + tTransferencia.ToString() + "/transactions");
                var myWebRequest = WebRequest.Create(myUri);
                var myHttpWebRequest = (HttpWebRequest)myWebRequest;
                myHttpWebRequest.PreAuthenticate = true;
                myHttpWebRequest.Headers.Add("Authorization", "Basic " + encodeString.ToString() + "");
                myHttpWebRequest.Accept = "application/json";

                var myWebResponse = myWebRequest.GetResponse();
                var responseStream = myWebResponse.GetResponseStream();


                StreamReader myStreamReader = new StreamReader(responseStream, Encoding.UTF8);
                var json = myStreamReader.ReadToEnd();

                //txtJson.Text = json.ToString();
                JObject o = JObject.Parse(json);

                //            ClientScript.RegisterStartupScript(this.GetType(),
                //                "Alerta", "alert('" + json.ToString() + "');  ", true);

                ((Label)e.Item.FindControl("lblOperacoes")).Text = o["transactions"]["items"].Count().ToString();

                for (int i = 0; i < o["transactions"]["items"].Count(); i++)
                {
                    dtTransacoes.Rows.Add(o["transactions"]["items"][i]["id"].ToString(), o["transactions"]["items"][i]["created_at"].ToString(), o["transactions"]["items"][i]["payment_type"].ToString(), o["transactions"]["items"][i]["amount"].ToString());
                    //o["transactions"]["items"][i]["id"].ToString();
                    //o["transactions"]["items"][i]["amount"].ToString();
                    //o["transactions"]["items"][i]["payment_type"].ToString();
                }

                responseStream.Close();
                myWebResponse.Close();

                DataSet ds = new DataSet("dsTransacoes");
                ds.Tables.Add(dtTransacoes);

                Repeater oGrid = (Repeater)e.Item.FindControl("rptTransacoes");
                oGrid.Visible = true;

                oGrid.DataSource = ds;
                oGrid.DataMember = "DTTransacoes";
                oGrid.DataBind();

            }
            catch
            {
            }
        }
    }
}