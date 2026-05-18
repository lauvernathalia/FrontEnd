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


public partial class con_cobrancas_gestao_detalhe : System.Web.UI.Page
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
        if (!IsPostBack)
        {
            ConsultaGeral();
        }
    }

    private void ConsultaGeral()
    {
        //dtgConsulta.Visible = true;
        SqlConnection myConsulta = new SqlConnection(Funcoes.conexao());
        myConsulta.Open();
        SqlDataAdapter SDAConsulta = new SqlDataAdapter("dbo.stp_vendas_vendas_ins", myConsulta);
        SDAConsulta.SelectCommand.CommandType = CommandType.StoredProcedure;
        SDAConsulta.SelectCommand.Parameters.Add("@flg_operacao", SqlDbType.Char).Value = "C";
        SDAConsulta.SelectCommand.Parameters.Add("@COD_ID_PESSOA_LICENCIADO", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["LICENCIADO"].ToString());
        SDAConsulta.SelectCommand.Parameters.Add("@COD_ID_PESSOAS_FJ_VENDEDOR", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["PESSOA"].ToString());
        SDAConsulta.SelectCommand.Parameters.Add("@COD_ID_VENDAS", SqlDbType.Int).Value = Funcoes.strToInt(sid_id.ToString());
        DataSet dsConsulta = new DataSet();
        SDAConsulta.Fill(dsConsulta, "VENDAS_VENDAS");
        rptConsulta.DataSource = dsConsulta.Tables["VENDAS_VENDAS"].DefaultView;
        rptConsulta.DataBind();
        myConsulta.Close(); myConsulta.Dispose();
    }
    
    protected void btnFechar_Click(object sender, EventArgs e)
    {
        ClientScript.RegisterStartupScript(this.GetType(), "Alerta", "opener.PostBackOnMainPage(); window.close();", true);
    }
    
    protected void rptConsulta_ItemCommand(object source, System.Web.UI.WebControls.RepeaterCommandEventArgs e)
    {
        if (e.CommandName == "Cancelar")
        {
            foreach (RepeaterItem itemE in rptConsulta.Items)
            {
                if ((((TextBox)itemE.FindControl("txtintegracao")).Text.ToString().Trim() == "Z") && (((TextBox)itemE.FindControl("txtcode")).Text.ToString().Trim() == e.CommandArgument.ToString()))
                {
                    string jsonRetorno = zoop.CancelamentoBoleto(e.CommandArgument.ToString());
                    if (jsonRetorno.ToString().Trim() != "")
                    {
                        try
                        {
                            JObject oCancelamento = JObject.Parse(jsonRetorno);
                            if (oCancelamento["status"].ToString() == "REQUESTED")
                            {
                                ClientScript.RegisterStartupScript(this.GetType(), "SucessoCancelar", "alert('Transacao cancelada com sucesso!');", true);
                            }
                            else
                            {
                                ClientScript.RegisterStartupScript(this.GetType(), "FalhaIdentificacaoCancelar", "alert('Ocorreu um erro ao tentar obter o status do cancelamento! Aguarde para verificar se a operação será cancelada.');", true);
                            }
                        }
                        catch
                        {
                            ClientScript.RegisterStartupScript(this.GetType(), "ErroCancelar", "alert('Ocorreu um erro ao tentar cancelar a transação! Verifique e tente novamente.');", true);
                        }
                    }
                }

                if ((((TextBox)itemE.FindControl("txtintegracao")).Text.ToString().Trim() == "A") && (((TextBox)itemE.FindControl("txtcode")).Text.ToString().Trim() == e.CommandArgument.ToString()))
                {
                    string sApiKey = asaas.PegarTokenSubconta(Funcoes.strToInt(HttpContext.Current.Session["PESSOA"].ToString()), Funcoes.strToInt(HttpContext.Current.Session["LICENCIADO"].ToString()));
                    asaas.HttpResponseResult resultado = asaas.ExcluirCobrancaAsaas(sApiKey,e.CommandArgument.ToString());

                    if ((resultado.StatusCode == 200) || (resultado.StatusCode == 201))
                    {
                                    string jsonResponse = resultado.Content;
                        JObject oTransacao = JObject.Parse(jsonResponse);
                        ClientScript.RegisterStartupScript(this.GetType(), "SucessoCancelar", "alert('Transacao cancelada com sucesso!');", true);

                    }
                    else
                    {
                        // Falha - Exibir erro conforme necessário
                        string mensagemErro = resultado.Content;
                        ClientScript.RegisterStartupScript(this.GetType(), "ErroGeral", "alert('Ocorreu um erro:" + mensagemErro + "');", true);

                    }

                }

            }
            ConsultaGeral();

        }

        if (e.CommandName == "Email")
        {
            foreach (RepeaterItem itemE in rptConsulta.Items)
            {
                if ((Funcoes.strToInt(((TextBox)itemE.FindControl("txtid")).Text.ToString().Trim()) == Funcoes.strToInt(e.CommandArgument.ToString())))
                {
                    if (transacoes.EnviarLinkBoletoEmail(((TextBox)itemE.FindControl("txtboleto")).Text.ToString(), ((TextBox)itemE.FindControl("txtbarcodeboleto")).Text.ToString(), ((TextBox)itemE.FindControl("txtnome")).Text.ToString(), ((TextBox)itemE.FindControl("txtdescricao")).Text.ToString(), ((TextBox)itemE.FindControl("txtvencimento")).Text.ToString(), ((TextBox)itemE.FindControl("txtvalor")).Text.ToString(), ((TextBox)itemE.FindControl("txtemails")).Text.ToString()))
                    {
                        ClientScript.RegisterStartupScript(this.GetType(), "SucessoEmailCobranca", "alert('O e-mail da cobrança foi enviado com sucesso!');", true);
                    }
                    else
                    {
                        ClientScript.RegisterStartupScript(this.GetType(), "ErroEmailCobranca", "alert('Ocorreu um erro ao enviar!');", true);
                    }
                }
            }
        }

    }
    protected void rptConsulta_ItemDataBound(object sender, System.Web.UI.WebControls.RepeaterItemEventArgs e)
    {

        ((HtmlAnchor)e.Item.FindControl("hrfBoleto")).Visible = false;
        ((System.Web.UI.WebControls.Image)e.Item.FindControl("imgQRcode")).Visible = false;
        

        string tBoleto = ((TextBox)e.Item.FindControl("txttipoboleto")).Text.ToString();
        string tPix = ((TextBox)e.Item.FindControl("txttipopix")).Text.ToString();
        string tCartaoCredito = ((TextBox)e.Item.FindControl("txttipocartaocredito")).Text.ToString();

        if (tBoleto.ToString().Trim() == "S")
        {
            ((HtmlAnchor)e.Item.FindControl("hrfBoleto")).Visible = true;
        }
        if (tPix.ToString().Trim() == "S")
        {
            ((System.Web.UI.WebControls.Image)e.Item.FindControl("imgQRcode")).Visible = true;

        }
        if (tCartaoCredito.ToString().Trim() == "S")
        {
            ((HtmlAnchor)e.Item.FindControl("hrfBoleto")).Visible = false;

        }

    }
}