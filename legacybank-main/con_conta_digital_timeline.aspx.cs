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

public partial class con_conta_digital_timeline : System.Web.UI.Page
{

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
            ConsultaGeral();
        }
    }

    private void ConsultaGeral()
    {
        SqlConnection myConsulta = new SqlConnection(Funcoes.conexao());
        myConsulta.Open();
        SqlDataAdapter SDAConsulta = new SqlDataAdapter("dbo.stp_pessoas_fj_baas_timeline_ins", myConsulta);
        SDAConsulta.SelectCommand.CommandType = CommandType.StoredProcedure;
        SDAConsulta.SelectCommand.Parameters.Add("@flg_operacao", SqlDbType.Char).Value = "D";
        SDAConsulta.SelectCommand.Parameters.Add("@COD_ID_PESSOA_LICENCIADO", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["LICENCIADO"].ToString());
        SDAConsulta.SelectCommand.Parameters.Add("@COD_ID_PESSOAS_FJ", SqlDbType.Int).Value = Funcoes.strToInt(sid_id.ToString());

        DataSet dsConsulta = new DataSet();
        SDAConsulta.Fill(dsConsulta, "PESSOAS_FJ_BAAS_TIMELINE");
        rptTimeline.DataSource = dsConsulta.Tables["PESSOAS_FJ_BAAS_TIMELINE"].DefaultView;
        rptTimeline.DataBind();
        myConsulta.Close(); myConsulta.Dispose();

        
        string sToken = asaas.PegarTokenSubconta(Funcoes.strToInt(sid_id.ToString()), Funcoes.strToInt(HttpContext.Current.Session["LICENCIADO"].ToString()));

        txtStatus.Text = asaas.ListarDocumentosPendentes(sToken);
        txtStatus.Text = txtStatus.Text + asaas.ListarStatus(sToken);

        asaas.HttpResponseResult resultado = asaas.RecuperarDadosComerciais(sToken);

        if (resultado.StatusCode == 200)
        {
            string jsonResponse = resultado.Content;
            txtStatus.Text = txtStatus.Text + jsonResponse;
        }
        else
        {
            string mensagemErro = resultado.Content;
            txtStatus.Text = txtStatus.Text + mensagemErro;
        }

        txtStatus.Visible = true;
    }

    protected void rptTimeline_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        Repeater oGrid = (Repeater)e.Item.FindControl("rptTimelineDetalhe");
        oGrid.Visible = true;

        TextBox tDataTimeline = (TextBox)e.Item.FindControl("txtdatatimeline");

        SqlConnection myConsultaSubmenu = new SqlConnection(Funcoes.conexao());
        myConsultaSubmenu.Open();
        SqlDataAdapter SDAConsultaSubmenu = new SqlDataAdapter("dbo.stp_pessoas_fj_baas_timeline_ins", myConsultaSubmenu);
        SDAConsultaSubmenu.SelectCommand.CommandType = CommandType.StoredProcedure;
        SDAConsultaSubmenu.SelectCommand.Parameters.Add("@flg_operacao", SqlDbType.Char).Value = "C";
        SDAConsultaSubmenu.SelectCommand.Parameters.Add("@COD_ID_PESSOA_LICENCIADO", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["LICENCIADO"].ToString());
        SDAConsultaSubmenu.SelectCommand.Parameters.Add("@COD_ID_PESSOAS_FJ", SqlDbType.Int).Value = Funcoes.strToInt(sid_id.ToString());
        SDAConsultaSubmenu.SelectCommand.Parameters.Add("@DTA_TIMELINE_FILTRO", SqlDbType.VarChar).Value = tDataTimeline.Text.ToString();
        DataSet dsConsultaSubmenu = new DataSet();
        SDAConsultaSubmenu.Fill(dsConsultaSubmenu, "PESSOAS_FJ_BAAS_TIMELINE");
        oGrid.DataSource = dsConsultaSubmenu.Tables["PESSOAS_FJ_BAAS_TIMELINE"].DefaultView;
        oGrid.DataBind();
        myConsultaSubmenu.Close();
        myConsultaSubmenu.Dispose();

    }

    public static string RetornaStatusDocumentos(string sStatus)
    {
        string sRetorno = "";
        switch (sStatus)
        {
            case "REJECTED":
                sRetorno = "Rejeitado";
                break;
            case "APPROVED":
                sRetorno = "Aprovado";
                break;
            case "AWAITING_APPROVAL":
                sRetorno = "Aguardando";
                break;
            case "PENDING":
                sRetorno = "Pendente";
                break;
            default:
                sRetorno = "Não Identificado";
                break;
        }
        return sRetorno.ToString();
    }

}