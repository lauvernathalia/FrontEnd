using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
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

using System.Collections.Specialized;


public partial class con_equipamentos_adm : System.Web.UI.Page
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
        SqlDataAdapter SDAConsulta = new SqlDataAdapter("dbo.stp_equipamentos_ins", myConsulta);
        SDAConsulta.SelectCommand.CommandType = CommandType.StoredProcedure;
        SDAConsulta.SelectCommand.Parameters.Add("@flg_operacao", SqlDbType.Char).Value = "C";
        SDAConsulta.SelectCommand.Parameters.Add("@COD_ID_PESSOA_LICENCIADO", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["LICENCIADO"].ToString());
        SDAConsulta.SelectCommand.Parameters.Add("@NOM_FILTRO", SqlDbType.VarChar).Value = txtFiltro.Text.ToString();
        SDAConsulta.SelectCommand.Parameters.Add("@FLG_STATUS", SqlDbType.Char).Value = ddlStatus.SelectedValue.ToString();

        if (HttpContext.Current.Session["TIPO"].ToString() == "M")
        {
            SDAConsulta.SelectCommand.Parameters.Add("@COD_ID_MARKETPLACE", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["PESSOA"].ToString());
        }
        if (HttpContext.Current.Session["TIPO"].ToString() == "R")
        {
            SDAConsulta.SelectCommand.Parameters.Add("@COD_ID_REPRESENTANTE", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["PESSOA"].ToString());
        }

        DataSet dsConsulta = new DataSet();
        SDAConsulta.Fill(dsConsulta, "EQUIPAMENTOS");
        rptConsulta.DataSource = dsConsulta.Tables["EQUIPAMENTOS"].DefaultView;
        rptConsulta.DataBind();
        myConsulta.Close(); myConsulta.Dispose();

    }


    protected void rptConsulta_OnItemCommand(object source, RepeaterCommandEventArgs e)
    {
        if (e.CommandName == "Cadastrar")
        {
            foreach (RepeaterItem item in rptConsulta.Items)
            {
                if (Funcoes.strToInt(Convert.ToString(e.CommandArgument)) == Funcoes.strToInt(((TextBox)item.FindControl("txtID")).Text.ToString()))
                {
                    if (((TextBox)item.FindControl("txtAdquirente")).Text.ToString().ToUpper() == "CAPPTA")
                    {
                        DadosPOSCappta.Root dPOS = new DadosPOSCappta.Root()
                        {
                            resellerDocument = ((TextBox)item.FindControl("txtRepresentante")).Text.ToString(),
                            modelId = Funcoes.strToInt(((TextBox)item.FindControl("txtModelo")).Text.ToString()),
                            serialKey = ((TextBox)item.FindControl("txtSerial")).Text.ToString()
                        };

                        string jsonPOS = JsonConvert.SerializeObject(dPOS);
                        string jsonRetorno = hubcappta.CadastrarPOS(jsonPOS);

                        JObject oPOS = JObject.Parse(jsonRetorno);

                        try
                        {
                            SqlConnection connInsCons = new SqlConnection(Funcoes.conexao());
                            connInsCons.Open();
                            SqlCommand cmdInsCons = new SqlCommand("dbo.stp_equipamentos_ins", connInsCons);
                            cmdInsCons.CommandType = CommandType.StoredProcedure;
                            cmdInsCons.Parameters.Add("@flg_operacao", SqlDbType.Char).Value = "R";
                            cmdInsCons.Parameters.Add("@COD_ID", SqlDbType.Int).Value = Funcoes.strToInt(Convert.ToString(e.CommandArgument));
                            cmdInsCons.Parameters.Add("@COD_ID_PESSOA_LICENCIADO", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["LICENCIADO"].ToString());
                            cmdInsCons.Parameters.Add("@NUM_TOKEN", SqlDbType.VarChar).Value = oPOS["id"];
                            cmdInsCons.ExecuteNonQuery();
                            connInsCons.Close();
                            connInsCons.Dispose();

                            ClientScript.RegisterStartupScript(this.GetType(),
                                "SucessoPOS", "alert('Equipamento cadastrado com sucesso na adquirente! " + oPOS["id"].ToString() + "');", true);
                        }
                        catch
                        {
                            ClientScript.RegisterStartupScript(this.GetType(),
                                "ErroPOS", "alert('Ocorreu um erro ao tentar cadastrar (" + ((TextBox)item.FindControl("txtSerial")).Text.ToString() + "): " + oPOS["errorDetails"]["serialKey"][0].ToString() + "');", true);
                        }
                    }
                    else
                    {
                        ClientScript.RegisterStartupScript(this.GetType(),
                            "ErroAdquirente", "alert('Esta ADQUIRENTE não possui registro de POS');", true);

                    }
                }
            }
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
        ConsultaGeral();
    }
    protected void btnNovo_Click(object sender, EventArgs e)
    {
        const string someScript = "NovoRegistro";
        ClientScript.RegisterStartupScript(this.GetType(),
            someScript, "openPopupWindow('cad_equipamentos_adm.aspx?id=0','EquipamentosEdicao',1024,800);", true);

    }
    protected void btnModelo_Click(object sender, EventArgs e)
    {
        const string someScript = "NovoRegistro";
        ClientScript.RegisterStartupScript(this.GetType(),
            someScript, "openPopupWindow('cad_equipamentos_modelos_adm.aspx?id=0','EquipamentosModelos',1024,800);", true);

    }
    protected void btnSerial_Click(object sender, EventArgs e)
    {
        const string someScript = "NovoRegistro";
        ClientScript.RegisterStartupScript(this.GetType(),
            someScript, "openPopupWindow('cad_equipamentos_seriais_adm.aspx?id=0','EquipamentosSeriais',1024,800);", true);

    }
}