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


public partial class con_planos_referencia : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            ConsultaGeral();
        }

    }
    protected void btnPesquisar_Click(object sender, EventArgs e)
    {
        ConsultaGeral();
    }

    private void ConsultaGeral()
    {
        //dtgConsulta.Visible = true;
        SqlConnection myConsulta = new SqlConnection(Funcoes.conexao());
        myConsulta.Open();
        SqlDataAdapter SDAConsulta = new SqlDataAdapter("dbo.stp_planos_referencia_ins", myConsulta);
        SDAConsulta.SelectCommand.CommandType = CommandType.StoredProcedure;
        SDAConsulta.SelectCommand.Parameters.Add("@flg_operacao", SqlDbType.Char).Value = "C";
        SDAConsulta.SelectCommand.Parameters.Add("@COD_ID_PESSOA_LICENCIADO", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["LICENCIADO"].ToString());
        SDAConsulta.SelectCommand.Parameters.Add("@NOM_PLANO_REFERENCIA", SqlDbType.VarChar).Value = txtFiltro.Text.ToString();
        SDAConsulta.SelectCommand.Parameters.Add("@FLG_ATIVO", SqlDbType.Char).Value = ddlAtivo.SelectedValue.ToString();

        DataSet dsConsulta = new DataSet();
        SDAConsulta.Fill(dsConsulta, "PLANOS_REFERENCIA");
        rptConsulta.DataSource = dsConsulta.Tables["PLANOS_REFERENCIA"].DefaultView;
        rptConsulta.DataBind();
        myConsulta.Close(); myConsulta.Dispose();
    }


    protected void rptConsulta_OnItemCommand(object source, RepeaterCommandEventArgs e)
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
        ConsultaGeral();
    }
    protected void btnNovo_Click(object sender, EventArgs e)
    {
        const string someScript = "NovoRegistro";
        ClientScript.RegisterStartupScript(this.GetType(),
            someScript, "openPopupWindow('cad_planos_referencia.aspx?id=0','PlanosReferenciaEdicao',1024,800);", true);

    }
    protected void btnImportar_Click(object sender, EventArgs e)
    {
        /*
        try
        {
            JObject o = JObject.Parse(zoop.planos_referencia("").ToString());
                        
            for (int i = 0; i < o["items"].Count(); i++)
            {
                SqlConnection connInsCons = new SqlConnection(Funcoes.conexao());
                connInsCons.Open();
                SqlCommand cmdInsCons = new SqlCommand("dbo.stp_planos_referencia_ins", connInsCons);
                cmdInsCons.CommandType = CommandType.StoredProcedure;
                cmdInsCons.Parameters.Add("@flg_operacao", SqlDbType.Char).Value = 'I';
                cmdInsCons.Parameters.Add("@COD_ID_PESSOA_LICENCIADO", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["LICENCIADO"].ToString());
                cmdInsCons.Parameters.Add("@FLG_MODELO_PLANO_REFERENCIA", SqlDbType.Char).Value = "Z";
                cmdInsCons.Parameters.Add("@FLG_ATIVO", SqlDbType.Char).Value = (o["items"][i]["is_active"].ToString()=="True") ? "S" : "N";
                cmdInsCons.Parameters.Add("@COD_ID_PLANO_REFERENCIA", SqlDbType.VarChar).Value = o["items"][i]["id"].ToString();
                cmdInsCons.Parameters.Add("@NOM_PLANO_REFERENCIA", SqlDbType.VarChar).Value = o["items"][i]["name"].ToString();
                cmdInsCons.Parameters.Add("@NOM_INVOICE_PLANO_REFERENCIA", SqlDbType.VarChar).Value = o["items"][i]["invoice_name"].ToString();
                cmdInsCons.Parameters.Add("@NOM_PERIODICIDADE", SqlDbType.VarChar).Value = o["items"][i]["interval"].ToString();
                cmdInsCons.Parameters.Add("@DES_PLANO_REFERENCIA", SqlDbType.Text).Value = o["items"][i]["description"].ToString();
                cmdInsCons.ExecuteNonQuery();
                connInsCons.Close();
                connInsCons.Dispose();
            }

            ClientScript.RegisterStartupScript(this.GetType(),
        "Alerta", "alert('Planos Importados com Sucesso'); opener.PostBackOnMainPage(); window.close(); ", true);
            
        }
        catch
        {
            ConsultaGeral();
        }
        */
    }
    protected void btnImportarPlanos_Click(object sender, EventArgs e)
    {
        const string someScript = "NovoRegistro";
        ClientScript.RegisterStartupScript(this.GetType(),
            someScript, "openPopupWindow('cad_planos_referencia_importar.aspx?id=0','PlanosReferenciaImportar',1024,800);", true);

    }
}