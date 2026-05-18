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


public partial class con_pagamento_extrato : System.Web.UI.Page
{

    public static DataTable dtExtrato;
    
    protected void Page_Load(object sender, EventArgs e)
    {

        if (HttpContext.Current.Session.Count <= 0)
        {
            FormsAuthentication.SignOut();
            Response.Redirect("login.aspx");
        }

        if (!IsPostBack)
        {

            dtExtrato = new DataTable();
            dtExtrato.Columns.Add("id", typeof(string));
            dtExtrato.Columns.Add("data", typeof(string));
            dtExtrato.Columns.Add("valor", typeof(string));
            dtExtrato.Columns.Add("saldo", typeof(string));
            dtExtrato.Columns.Add("descricao", typeof(string));

            Funcoes.GravaAuditoria(0, Request.ServerVariables["REMOTE_ADDR"].ToString(), "Extrato", "Consulta Extrato Conta Digital");
            DateTime data = DateTime.Today;
            DateTime primeiroDiaDoMes = new DateTime(data.Year, data.Month, 1);
            DateTime ultimoDiaDoMes = new DateTime(data.Year, data.Month, DateTime.DaysInMonth(data.Year, data.Month));
            txtDataIni.Text = primeiroDiaDoMes.ToShortDateString();
            txtDataFim.Text = ultimoDiaDoMes.ToShortDateString();

            ConsultaSaldoBaas("A");
            btnPesquisarGestao_Click(null, null);
        }

    }

    protected void btnPesquisarGestao_Click(object sender, EventArgs e)
    {
        DateTime DataInicial = Convert.ToDateTime(txtDataIni.Text.ToString());
        DateTime DataFinal = Convert.ToDateTime(txtDataFim.Text.ToString());

        TimeSpan span = DataFinal.Subtract(DataInicial);
        int Dias = span.Days;

        if (Dias <= 60)
        {
            string sstartDate = DataInicial.Year.ToString().PadLeft(4,'0')+"-"+DataInicial.Month.ToString().PadLeft(2,'0')+"-"+DataInicial.Day.ToString().PadLeft(2,'0');
            string sfinishDate = DataFinal.Year.ToString().PadLeft(4, '0') + "-" + DataFinal.Month.ToString().PadLeft(2, '0') + "-" + DataFinal.Day.ToString().PadLeft(2, '0');

            
            string sFiltro = "?offset=0&limit=100&startDate=" + sstartDate + "&finishDate="+sfinishDate+"&order=desc";
            

            // Bloco de Listagem do Extrato

            int iOffSet = 0;
            int iLimit = 100;
            bool bhas_more = true;
            
            while (bhas_more == true)
            {
                // Consulta Extrato
                sstartDate = DataInicial.Year.ToString().PadLeft(4, '0') + "-" + DataInicial.Month.ToString().PadLeft(2, '0') + "-" + DataInicial.Day.ToString().PadLeft(2, '0');
                sfinishDate = DataFinal.Year.ToString().PadLeft(4, '0') + "-" + DataFinal.Month.ToString().PadLeft(2, '0') + "-" + DataFinal.Day.ToString().PadLeft(2, '0');
                
                sFiltro = "?offset=" + iOffSet.ToString() + "&limit=100&startDate=" + sstartDate + "&finishDate=" + sfinishDate + "&order=desc";
                
                string jsonExtrato = asaas.ExtratoSubconta(asaas.PegarTokenSubconta(Funcoes.strToInt(HttpContext.Current.Session["PESSOA"].ToString()), Funcoes.strToInt(HttpContext.Current.Session["LICENCIADO"].ToString())), sFiltro);
                try
                {

                    if (jsonExtrato.ToString().Trim() != "")
                    {
                        JObject o = JObject.Parse(jsonExtrato);

                        bhas_more = Convert.ToBoolean(o["hasMore"].ToString());

                        for (int i = 0; i < o["data"].Count(); i++)
                        {
                            if (o["data"][i]["paymentId"].ToString().Trim() != "") { dtExtrato.Rows.Add(o["data"][i]["paymentId"].ToString(), Convert.ToDateTime(o["data"][i]["date"].ToString()).ToShortDateString(), String.Format("{0:c2}", Funcoes.strToDouble(o["data"][i]["value"].ToString())), String.Format("{0:c2}", Funcoes.strToDouble(o["data"][i]["balance"].ToString())), o["data"][i]["description"].ToString()); }
                            if (o["data"][i]["splitId"].ToString().Trim() != "") { dtExtrato.Rows.Add(o["data"][i]["splitId"].ToString(), Convert.ToDateTime(o["data"][i]["date"].ToString()).ToShortDateString(), String.Format("{0:c2}", Funcoes.strToDouble(o["data"][i]["value"].ToString())), String.Format("{0:c2}", Funcoes.strToDouble(o["data"][i]["balance"].ToString())), o["data"][i]["description"].ToString()); }
                            if (o["data"][i]["transferId"].ToString().Trim() != "") { dtExtrato.Rows.Add(o["data"][i]["transferId"].ToString(), Convert.ToDateTime(o["data"][i]["date"].ToString()).ToShortDateString(), String.Format("{0:c2}", Funcoes.strToDouble(o["data"][i]["value"].ToString())), String.Format("{0:c2}", Funcoes.strToDouble(o["data"][i]["balance"].ToString())), o["data"][i]["description"].ToString()); }
                            if (o["data"][i]["anticipationId"].ToString().Trim() != "") { dtExtrato.Rows.Add(o["data"][i]["anticipationId"].ToString(), Convert.ToDateTime(o["data"][i]["date"].ToString()).ToShortDateString(), String.Format("{0:c2}", Funcoes.strToDouble(o["data"][i]["value"].ToString())), String.Format("{0:c2}", Funcoes.strToDouble(o["data"][i]["balance"].ToString())), o["data"][i]["description"].ToString()); }
                            if (o["data"][i]["billId"].ToString().Trim() != "") { dtExtrato.Rows.Add(o["data"][i]["billId"].ToString(), Convert.ToDateTime(o["data"][i]["date"].ToString()).ToShortDateString(), String.Format("{0:c2}", Funcoes.strToDouble(o["data"][i]["value"].ToString())), String.Format("{0:c2}", Funcoes.strToDouble(o["data"][i]["balance"].ToString())), o["data"][i]["description"].ToString()); }
                            if (o["data"][i]["invoiceId"].ToString().Trim() != "") { dtExtrato.Rows.Add(o["data"][i]["invoiceId"].ToString(), Convert.ToDateTime(o["data"][i]["date"].ToString()).ToShortDateString(), String.Format("{0:c2}", Funcoes.strToDouble(o["data"][i]["value"].ToString())), String.Format("{0:c2}", Funcoes.strToDouble(o["data"][i]["balance"].ToString())), o["data"][i]["description"].ToString()); }
                            if (o["data"][i]["paymentDunningId"].ToString().Trim() != "") { dtExtrato.Rows.Add(o["data"][i]["paymentDunningId"].ToString(), Convert.ToDateTime(o["data"][i]["date"].ToString()).ToShortDateString(), String.Format("{0:c2}", Funcoes.strToDouble(o["data"][i]["value"].ToString())), String.Format("{0:c2}", Funcoes.strToDouble(o["data"][i]["balance"].ToString())), o["data"][i]["description"].ToString()); }
                            if (o["data"][i]["creditBureauReportId"].ToString().Trim() != "") { dtExtrato.Rows.Add(o["data"][i]["creditBureauReportId"].ToString(), Convert.ToDateTime(o["data"][i]["date"].ToString()).ToShortDateString(), String.Format("{0:c2}", Funcoes.strToDouble(o["data"][i]["value"].ToString())), String.Format("{0:c2}", Funcoes.strToDouble(o["data"][i]["balance"].ToString())), o["data"][i]["description"].ToString()); }

                            //dtExtrato.Rows.Add(o["data"][i]["id"].ToString(), Convert.ToDateTime(o["data"][i]["date"].ToString()).ToShortDateString(), String.Format("{0:c2}", Funcoes.strToDouble(o["data"][i]["value"].ToString())), String.Format("{0:c2}", Funcoes.strToDouble(o["data"][i]["balance"].ToString())), o["data"][i]["description"].ToString());
                        }
                        if (bhas_more == true)
                        {
                            iOffSet = iOffSet + 100;
                        }
                    }
                    else
                    {
                        bhas_more = false;
                        return;
                    }
                }
                catch
                {
                    bhas_more = false;
                    return;
                }
                    
            }

            this.rptConsulta.DataSource = dtExtrato;
            this.rptConsulta.DataBind();
        }
        else
        {
            ClientScript.RegisterStartupScript(this.GetType(), "PeriodoSuperior30", "alert('Período informado é superior a 60 dias!');", true);
        }
    }

    private void ConsultaSaldoBaas(string integracao)
    {
        try
        {
            if (integracao != "A")
                return;

            // Valor padrão seguro
            lblSaldo.Text = "0,00";

            // Validação de Session
            if (HttpContext.Current.Session["PESSOA"] == null ||
                HttpContext.Current.Session["LICENCIADO"] == null)
                return;

            int pessoa = Funcoes.strToInt(HttpContext.Current.Session["PESSOA"].ToString());
            int licenciado = Funcoes.strToInt(HttpContext.Current.Session["LICENCIADO"].ToString());

            string sToken = asaas.PegarTokenSubconta(pessoa, licenciado);

            if (string.IsNullOrWhiteSpace(sToken))
                return;

            string jsonSaldo = asaas.SaldoSubconta(sToken);

            if (string.IsNullOrWhiteSpace(jsonSaldo))
                return;

            JObject oSaldo = JObject.Parse(jsonSaldo);

            // 🚨 Se a API retornou erro
            if (oSaldo["errors"] != null)
            {

                // Você pode logar o erro se quiser
                // string erroApi = oSaldo["errors"][0]?["description"]?.ToString();
                string erroApi = oSaldo.SelectToken("errors[0].description") != null ? oSaldo.SelectToken("errors[0].description").ToString() : string.Empty;

                ClientScript.RegisterStartupScript(this.GetType(), "AtualizarDadosCadastrais", "alert('Conta Digital informa que: " + erroApi + "');", true);


                lblSaldo.Text = "0,00";
                return;
            }

            // 🔐 Proteção total contra null
            JToken balanceToken = oSaldo["balance"];

            if (balanceToken == null || balanceToken.Type == JTokenType.Null)
            {
                lblSaldo.Text = "0,00";
                return;
            }

            double saldo = Funcoes.strToDouble(balanceToken.ToString());
            lblSaldo.Text = String.Format("{0:n2}", saldo);
        }
        catch (Exception ex)
        {
            // 🔥 Nunca deixa quebrar a página
            lblSaldo.Text = "0,00";

            // Opcional: log
            // Logger.Gravar(ex);
        }
    }
}