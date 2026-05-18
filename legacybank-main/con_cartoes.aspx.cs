using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Collections.Specialized;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Security.Cryptography;
using System.Data.SqlClient;
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


public partial class con_cartoes : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (HttpContext.Current.Session.Count <= 0)
        {
            Response.Redirect("login.aspx");
            FormsAuthentication.SignOut();
        }

        if (!IsPostBack)
        {
            Funcoes.GravaAuditoria(0, Request.ServerVariables["REMOTE_ADDR"].ToString(), "Cartões", "Meus Cartões");
            ConsultaSolicitacoes();
        }
    }


    private void ConsultaSolicitacoes()
    {
        SqlConnection myConsulta = new SqlConnection(Funcoes.conexao());
        myConsulta.Open();
        SqlDataAdapter SDAConsulta = new SqlDataAdapter("dbo.stp_cartoes_ins", myConsulta);
        SDAConsulta.SelectCommand.CommandType = CommandType.StoredProcedure;
        SDAConsulta.SelectCommand.Parameters.Add("@flg_operacao", SqlDbType.Char).Value = "C";
        SDAConsulta.SelectCommand.Parameters.Add("@COD_ID_PESSOA_LICENCIADO", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["LICENCIADO"].ToString());
        SDAConsulta.SelectCommand.Parameters.Add("@COD_ID_PESSOAS_FJ", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["PESSOA"].ToString());
        SDAConsulta.SelectCommand.Parameters.Add("@FLG_ORIGEM", SqlDbType.Char).Value = "A";

        DataSet dsConsulta = new DataSet();
        SDAConsulta.Fill(dsConsulta, "CARTOES");
        rptConsulta.DataSource = dsConsulta.Tables["CARTOES"].DefaultView;
        rptConsulta.DataBind();
        myConsulta.Close(); myConsulta.Dispose();
    }

    protected void rptConsulta_OnItemCommand(object source, System.Web.UI.WebControls.RepeaterCommandEventArgs e)
    {
        if (e.CommandName == "Atualizar")
        {
            foreach (RepeaterItem itemE in rptConsulta.Items)
            {
                if (Funcoes.strToInt(((TextBox)itemE.FindControl("txtid")).Text.ToString()) == Funcoes.strToInt(Convert.ToString(e.CommandArgument)))
                {

                    string jsonCartoes = asaas.ListarCartoesAsaas(asaas.PegarTokenSubconta(Funcoes.strToInt(HttpContext.Current.Session["PESSOA"].ToString()), Funcoes.strToInt(HttpContext.Current.Session["LICENCIADO"].ToString())));
                    JObject oCartoes = JObject.Parse(jsonCartoes);

                    //ClientScript.RegisterStartupScript(this.GetType(), "Consulta", "alert('" + jsonCartoes.ToString() + "');", true);

                    if (jsonCartoes.ToString().Trim() != "")
                    {
                        try
                        {
                            if (oCartoes["data"].Count() > 0)
                            {
                                for (int i = 0; i < oCartoes["data"].Count(); i++)
                                {
                                    //ClientScript.RegisterStartupScript(this.GetType(), "ConsultaNome" + i.ToString(), "alert('" + oCartoes["data"][i]["name"].ToString() + "');", true);

                                    if (oCartoes["data"][i]["name"].ToString() == ((TextBox)itemE.FindControl("txtNomeCartao")).Text.ToString())
                                    {
                                        SqlConnection connInsCons = new SqlConnection(Funcoes.conexao());
                                        connInsCons.Open();
                                        SqlCommand cmdInsCons = new SqlCommand("dbo.stp_cartoes_ins", connInsCons);
                                        cmdInsCons.CommandType = CommandType.StoredProcedure;
                                        cmdInsCons.Parameters.Add("@flg_operacao", SqlDbType.Char).Value = 'U';
                                        cmdInsCons.Parameters.Add("@COD_ID_PESSOA_LICENCIADO", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["LICENCIADO"].ToString());
                                        cmdInsCons.Parameters.Add("@COD_ID_PESSOAS_FJ", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["PESSOA"].ToString());
                                        cmdInsCons.Parameters.Add("@COD_ID", SqlDbType.Int).Value = Funcoes.strToInt(Convert.ToString(e.CommandArgument));

                                        cmdInsCons.Parameters.Add("@NUM_ID_CARTAO", SqlDbType.VarChar).Value = oCartoes["data"][i]["id"].ToString();
                                        cmdInsCons.Parameters.Add("@DES_JSON", SqlDbType.VarChar).Value = jsonCartoes.ToString();
                                        cmdInsCons.Parameters.Add("@FLG_STATUS", SqlDbType.VarChar).Value = oCartoes["data"][i]["status"].ToString();

                                        cmdInsCons.ExecuteNonQuery();
                                        connInsCons.Close();
                                        connInsCons.Dispose();
                                        ClientScript.RegisterStartupScript(this.GetType(), "ConsultaSucesso", "alert('Dados do cartão atualizados com sucesso!');", true);


                                    }
                                }
                            }
                        }
                        catch
                        {
                            try
                            {
                                ClientScript.RegisterStartupScript(this.GetType(), "ConsultaErro", "alert('ATENÇÃO! " + oCartoes["errors"][0]["description"].ToString() + "');", true);
                            }
                            catch
                            {
                                ClientScript.RegisterStartupScript(this.GetType(), "ConsultaErroGeral", "alert('ATENÇÃO! Ocorreu um erro ao tentar atualizar os dados do cartão! Verifique e tente novamente.');", true);
                            }

                        }
                    }
                }
            }


            ConsultaSolicitacoes();
        }
    }
}