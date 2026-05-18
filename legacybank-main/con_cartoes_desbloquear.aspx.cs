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


public partial class con_cartoes_desbloquear : System.Web.UI.Page
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
            Funcoes.GravaAuditoria(0, Request.ServerVariables["REMOTE_ADDR"].ToString(), "Cartões", "Desbloquear Cartões");
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
        if (e.CommandName == "Ativar")
        {
            if (txtPIN.Text.ToString().Trim() == "" || txtPINConfirmacao.Text.ToString().Trim() == "")
            {
                ClientScript.RegisterStartupScript(this.GetType(), "CamposPINErro", "alert('É obrigatório informar e confirmar o PIN (Senha do Cartão)');", true);
                return;
            }

            if (txtDigitos.Text.ToString().Trim() == "")
            {
                ClientScript.RegisterStartupScript(this.GetType(), "CamposDigitosErro", "alert('É obrigatório informar os 4 últimos dígitos do cartão que deseja ativar');", true);
                return;
            }

            if (txtPIN.Text.ToString().Trim() != txtPINConfirmacao.Text.ToString().Trim())
            {
                ClientScript.RegisterStartupScript(this.GetType(), "CamposConfirmacaoPINErro", "alert('O PIN (Senha do Cartão) e o PIN Confirmação (Senha do Cartão) devewm ser iguais');", true);
                return;
            }



            foreach (RepeaterItem itemE in rptConsulta.Items)
            {
                if (Funcoes.strToInt(((TextBox)itemE.FindControl("txtid")).Text.ToString()) == Funcoes.strToInt(Convert.ToString(e.CommandArgument)))
                {
                    AtivarCartaoAsaas.Root dcartao = new AtivarCartaoAsaas.Root()
                    {
                        pin = txtPIN.Text.ToString(),
                        pinConfirmation = txtPIN.Text.ToString(),
                        lastDigits = txtDigitos.Text.ToString()
                    };
                    string jsonCartaoAtivar = JsonConvert.SerializeObject(dcartao);
                    string jsonRetornoCartaoAtivar = asaas.AtivarCartoesAsaas(asaas.PegarTokenSubconta(Funcoes.strToInt(HttpContext.Current.Session["PESSOA"].ToString()), Funcoes.strToInt(HttpContext.Current.Session["LICENCIADO"].ToString())), ((TextBox)itemE.FindControl("txtidcartao")).Text.ToString(), jsonCartaoAtivar.ToString());

                    JObject oCartoes = JObject.Parse(jsonRetornoCartaoAtivar);

                    ClientScript.RegisterStartupScript(this.GetType(), "Consulta", "alert('" + jsonRetornoCartaoAtivar.ToString() + "');", true);

                    if (jsonRetornoCartaoAtivar.ToString().Trim() != "")
                    {
                        try
                        {
                            //ClientScript.RegisterStartupScript(this.GetType(), "ConsultaNome", "alert('" + oCartoes["name"].ToString() + "');", true);

                            if (oCartoes["name"].ToString() == ((TextBox)itemE.FindControl("txtNomeCartao")).Text.ToString())
                            {
                                SqlConnection connInsCons = new SqlConnection(Funcoes.conexao());
                                connInsCons.Open();
                                SqlCommand cmdInsCons = new SqlCommand("dbo.stp_cartoes_ins", connInsCons);
                                cmdInsCons.CommandType = CommandType.StoredProcedure;
                                cmdInsCons.Parameters.Add("@flg_operacao", SqlDbType.Char).Value = 'V';
                                cmdInsCons.Parameters.Add("@COD_ID_PESSOA_LICENCIADO", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["LICENCIADO"].ToString());
                                cmdInsCons.Parameters.Add("@COD_ID_PESSOAS_FJ", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["PESSOA"].ToString());
                                cmdInsCons.Parameters.Add("@COD_ID", SqlDbType.Int).Value = Funcoes.strToInt(Convert.ToString(e.CommandArgument));
                                cmdInsCons.Parameters.Add("@FLG_STATUS", SqlDbType.VarChar).Value = (oCartoes["status"].ToString()=="ACTIVE")?"S":"N";

                                cmdInsCons.ExecuteNonQuery();
                                connInsCons.Close();
                                ClientScript.RegisterStartupScript(this.GetType(), "ConsultaSucessoDesbloquear", "alert('Dados do cartão atualizados com sucesso!');", true);
                                connInsCons.Dispose();

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
                                ClientScript.RegisterStartupScript(this.GetType(), "ConsultaErroGeral", "alert('ATENÇÃO! Ocorreu um erro ao tentar ativar o cartão! Verifique e tente novamente.');", true);
                            }
                        }

                    }

                }
            }
            ConsultaSolicitacoes();
        }
        
        if (e.CommandName == "Desbloquear")
        {
            foreach (RepeaterItem itemE in rptConsulta.Items)
            {
                if (Funcoes.strToInt(((TextBox)itemE.FindControl("txtid")).Text.ToString()) == Funcoes.strToInt(Convert.ToString(e.CommandArgument)))
                {
                        string jsonCartoes = asaas.DesbloquearCartoesAsaas(asaas.PegarTokenSubconta(Funcoes.strToInt(HttpContext.Current.Session["PESSOA"].ToString()), Funcoes.strToInt(HttpContext.Current.Session["LICENCIADO"].ToString())), ((TextBox)itemE.FindControl("txtidcartao")).Text.ToString());
                        JObject oCartoes = JObject.Parse(jsonCartoes);

                        //ClientScript.RegisterStartupScript(this.GetType(), "Consulta", "alert('" + jsonCartoes.ToString() + "');", true);

                        if (jsonCartoes.ToString().Trim() != "")
                        {
                            try
                            {
                                //ClientScript.RegisterStartupScript(this.GetType(), "ConsultaNome", "alert('" + oCartoes["name"].ToString() + "');", true);

                                if (oCartoes["name"].ToString() == ((TextBox)itemE.FindControl("txtNomeCartao")).Text.ToString())
                                {
                                    SqlConnection connInsCons = new SqlConnection(Funcoes.conexao());
                                    connInsCons.Open();
                                    SqlCommand cmdInsCons = new SqlCommand("dbo.stp_cartoes_ins", connInsCons);
                                    cmdInsCons.CommandType = CommandType.StoredProcedure;
                                    cmdInsCons.Parameters.Add("@flg_operacao", SqlDbType.Char).Value = 'U';
                                    cmdInsCons.Parameters.Add("@COD_ID_PESSOA_LICENCIADO", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["LICENCIADO"].ToString());
                                    cmdInsCons.Parameters.Add("@COD_ID_PESSOAS_FJ", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["PESSOA"].ToString());
                                    cmdInsCons.Parameters.Add("@COD_ID", SqlDbType.Int).Value = Funcoes.strToInt(Convert.ToString(e.CommandArgument));

                                    cmdInsCons.Parameters.Add("@NUM_ID_CARTAO", SqlDbType.VarChar).Value = oCartoes["id"].ToString();
                                    cmdInsCons.Parameters.Add("@DES_JSON", SqlDbType.VarChar).Value = jsonCartoes.ToString();
                                    cmdInsCons.Parameters.Add("@FLG_STATUS", SqlDbType.VarChar).Value = oCartoes["status"].ToString();

                                    cmdInsCons.ExecuteNonQuery();
                                    connInsCons.Close();
                                    ClientScript.RegisterStartupScript(this.GetType(), "ConsultaSucessoDesbloquear", "alert('Dados do cartão atualizados com sucesso!');", true);
                                    connInsCons.Dispose();

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
                                    ClientScript.RegisterStartupScript(this.GetType(), "ConsultaErroGeral", "alert('ATENÇÃO! Ocorreu um erro ao tentar desbloquear o cartão! Verifique e tente novamente.');", true);
                                }
                            }

                        }
                }
            }
            ConsultaSolicitacoes();
        }
    }
}