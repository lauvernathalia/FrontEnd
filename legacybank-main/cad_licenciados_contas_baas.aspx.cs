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


public partial class cad_licenciados_contas_baas : System.Web.UI.Page
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

        // Verifica se o usuário esta autenticado
        if (HttpContext.Current.Session.Count <= 0)
        {
            Response.Redirect("login.aspx");
            FormsAuthentication.SignOut();
        }

        if (!IsPostBack)
        {
            ConsultaFicha();
            ConsultaGeral();
        }

    }

    private void ConsultaFicha()
    {
        SqlConnection mySelCadastro = new SqlConnection(Funcoes.conexao());
        mySelCadastro.Open();
        SqlCommand cmdSelCadastro = new SqlCommand("dbo.stp_pessoas_fj_licenciados_ins", mySelCadastro);
        cmdSelCadastro.CommandType = CommandType.StoredProcedure;
        cmdSelCadastro.Parameters.Add("@flg_operacao", SqlDbType.Char).Value = "S";
        cmdSelCadastro.Parameters.Add("@COD_ID", SqlDbType.Int).Value = Funcoes.strToInt(sid_id.ToString());
        SqlDataReader ReaderCadastro = cmdSelCadastro.ExecuteReader();
        while (ReaderCadastro.Read())
        {
            txtID.Text = ReaderCadastro["COD_ID"].ToString();
            txtRazaosocial.Text = ReaderCadastro["NOM_RAZAOSOCIAL"].ToString();
        }

    }


    private void ConsultaGeral()
    {
        SqlConnection myConsulta = new SqlConnection(Funcoes.conexao());
        myConsulta.Open();
        SqlDataAdapter SDAConsulta = new SqlDataAdapter("dbo.stp_pessoas_fj_baas_ins", myConsulta);
        SDAConsulta.SelectCommand.CommandType = CommandType.StoredProcedure;
        SDAConsulta.SelectCommand.Parameters.Add("@flg_operacao", SqlDbType.Char).Value = "L";
        SDAConsulta.SelectCommand.Parameters.Add("@COD_ID_PESSOA_LICENCIADO", SqlDbType.Int).Value = Funcoes.strToInt(sid_id.ToString());

        DataSet dsConsulta = new DataSet();
        SDAConsulta.Fill(dsConsulta, "PESSOAS_FJ_BAAS");
        rptConsulta.DataSource = dsConsulta.Tables["PESSOAS_FJ_BAAS"].DefaultView;
        rptConsulta.DataBind();
        myConsulta.Close(); myConsulta.Dispose();
    }

    protected void rptConsulta_OnItemCommand(object source, RepeaterCommandEventArgs e)
    {
        if (e.CommandName == "Atualizar")
        {
            foreach (RepeaterItem itemE in rptConsulta.Items)
            {
                if (((TextBox)itemE.FindControl("txtconta")).Text.ToString().Trim() == Convert.ToString(e.CommandArgument))
                {
                    string jsonStatus = asaas.ListarStatus(Funcoes.Decrypt(((TextBox)itemE.FindControl("txttoken")).Text.ToString()));
                    string jsonRetorno = asaas.SaldoSubconta(Funcoes.Decrypt(((TextBox)itemE.FindControl("txttoken")).Text.ToString()));
                    if (jsonRetorno.ToString().Trim() != "")
                    {
                        try
                        {
                            JObject oSaldo = JObject.Parse(jsonRetorno.ToString());
                            ((Label)itemE.FindControl("lblsaldo")).Text = String.Format("{0:n2}", Funcoes.strToDouble(oSaldo["balance"].ToString()));
                            if (jsonStatus.ToString().Trim()!="")
                            {
                                try
                                {
                                    JObject oStatus = JObject.Parse(jsonStatus.ToString());
                                    ((Label)itemE.FindControl("lblstatus")).Text = RetornaStatusConta(oStatus["general"].ToString());
                                }
                                catch
                                {
                                    ((Label)itemE.FindControl("lblstatus")).Text = "";
                                }
                            }
                        }
                        catch
                        {
                            ((Label)itemE.FindControl("lblsaldo")).Text = "0,00";
                            ((Label)itemE.FindControl("lblstatus")).Text = "";
                        }
                    }
                    else
                    {
                        ((Label)itemE.FindControl("lblsaldo")).Text = "0,00";
                        ((Label)itemE.FindControl("lblstatus")).Text = "";
                    }
                }
            }



        }

    }


    protected void btnFechar_Click(object sender, EventArgs e)
    {
        ClientScript.RegisterStartupScript(this.GetType(),"Alerta", "opener.PostBackOnMainPage(); window.close(); ", true);

    }

    public static string RetornaStatusConta(string sStatus)
    {
        string sRetorno = "";
        switch (sStatus)
        {
            case "REJECTED":
                sRetorno = "Reprovada";
                break;
            case "APPROVED":
                sRetorno = "Aprovada";
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