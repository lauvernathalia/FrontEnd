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

public partial class con_cartoes_solicitacao : System.Web.UI.Page
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
            Funcoes.GravaAuditoria(0, Request.ServerVariables["REMOTE_ADDR"].ToString(), "Cartões", "Solicitar Cartão");
            ConsultaFicha();
            ConsultaSolicitacoes();
        }
    }

    private void ConsultaFicha()
    {
        SqlConnection mySelCadastroEstabelecimento = new SqlConnection(Funcoes.conexao());
        mySelCadastroEstabelecimento.Open();
        SqlCommand cmdSelCadastroEstabelecimento = new SqlCommand("dbo.stp_pessoas_fj_ins", mySelCadastroEstabelecimento);
        cmdSelCadastroEstabelecimento.CommandType = CommandType.StoredProcedure;
        cmdSelCadastroEstabelecimento.Parameters.Add("@flg_operacao", SqlDbType.Char).Value = "S";
        cmdSelCadastroEstabelecimento.Parameters.Add("@COD_ID_PESSOA_LICENCIADO", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["LICENCIADO"].ToString());
        cmdSelCadastroEstabelecimento.Parameters.Add("@COD_ID", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["PESSOA"].ToString());
        SqlDataReader ReaderCadastroEstabelecimento = cmdSelCadastroEstabelecimento.ExecuteReader();
        while (ReaderCadastroEstabelecimento.Read())
        {
            txtNomeCartao.Text = (ReaderCadastroEstabelecimento["FLG_TIPO_PESSOA"].ToString() == "PJ") ? ReaderCadastroEstabelecimento["NOM_RAZAOSOCIAL"].ToString() : ReaderCadastroEstabelecimento["NOM_NOME"].ToString() + " " + ReaderCadastroEstabelecimento["NOM_SOBRENOME"].ToString();
        }
    }

    private void GravaCartao(string sID, string sStatus, string sJson)
    {
        SqlConnection connInsConsCliente = new SqlConnection(Funcoes.conexao());
        connInsConsCliente.Open();
        SqlCommand cmdInsConsCliente = new SqlCommand("dbo.stp_cartoes_ins", connInsConsCliente);
        cmdInsConsCliente.CommandType = CommandType.StoredProcedure;
        cmdInsConsCliente.Parameters.Add("@flg_operacao", SqlDbType.Char).Value = 'I';
        cmdInsConsCliente.Parameters.Add("@COD_ID_PESSOA_LICENCIADO", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["LICENCIADO"].ToString());
        cmdInsConsCliente.Parameters.Add("@COD_ID_PESSOAS_FJ", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["PESSOA"].ToString());
        cmdInsConsCliente.Parameters.Add("@DTA_DATA", SqlDbType.DateTime).Value = DateTime.Now;

        cmdInsConsCliente.Parameters.Add("@NUM_ID_CARTAO", SqlDbType.VarChar).Value = sID.ToString();
        cmdInsConsCliente.Parameters.Add("@NOM_NOME_CARTAO", SqlDbType.VarChar).Value = txtNomeCartao.Text.ToString();
        cmdInsConsCliente.Parameters.Add("@DES_JSON", SqlDbType.Text).Value = sJson.ToString();
        cmdInsConsCliente.Parameters.Add("@FLG_STATUS", SqlDbType.VarChar).Value = sStatus.ToString();
        cmdInsConsCliente.Parameters.Add("@FLG_ORIGEM", SqlDbType.VarChar).Value = "A";

        cmdInsConsCliente.ExecuteNonQuery();
        connInsConsCliente.Close();
        connInsConsCliente.Dispose();
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


    protected void btnSolicitar_Click(object sender, EventArgs e)
    {
        if (txtNomeCartao.Text.ToString().Trim() != "")
        {
            if (Funcoes.Enviar2faEstabelecimento() == true)
            {
                ClientScript.RegisterStartupScript(this.GetType(),
    "Codigo2fa", "alert('Um código de confirmação foi enviado para o seu e-mail!');", true);

            }
            else
            {
                ClientScript.RegisterStartupScript(this.GetType(),
    "Codigo2faErro", "alert('Ocorreu um erro ao tentar enviar o Código de Confirmação!');", true);

            }
            ClientScript.RegisterStartupScript(this.GetType(),
"ExecutaModal", "$('#mdConfirmar').modal('show');", true);

        }
        else
        {
            ClientScript.RegisterStartupScript(this.GetType(),"NomeEmBranco", "alert('É Obrigatório especificar um nome para ser impresso no cartão! Verifique e tente novamente');", true);
        }
    }

    protected void lkbReenviar_Click(object sender, System.EventArgs e)
    {
        if (Funcoes.Enviar2faEstabelecimento() == true)
        {
            ClientScript.RegisterStartupScript(this.GetType(),
"Codigo2fa", "alert('Um código de confirmação foi enviado para o seu e-mail!');", true);

        }
        else
        {
            ClientScript.RegisterStartupScript(this.GetType(),
"Codigo2faErro", "alert('Ocorreu um erro ao tentar enviar o Código de Confirmação!');", true);

        }
        ClientScript.RegisterStartupScript(this.GetType(),
"ExecutaModal", "$('#mdConfirmar').modal('show');", true);

    }

    protected void btnConfirmar2FA_Click(object sender, System.EventArgs e)
    {
        // Verifica se o código autenticação esta correto
        SqlConnection connVerifica2fa = new SqlConnection(Funcoes.conexao());
        SqlCommand cmdVerifica2fa = new SqlCommand("dbo.stp_2fa_ins", connVerifica2fa);
        cmdVerifica2fa.CommandType = CommandType.StoredProcedure;
        cmdVerifica2fa.Parameters.Add("@flg_operacao", SqlDbType.Char).Value = "L";
        cmdVerifica2fa.Parameters.Add("@COD_ID_SIS_USUARIO", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["CODIGO"].ToString());
        cmdVerifica2fa.Parameters.Add("@COD_ID_PESSOA_LICENCIADO", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["LICENCIADO"].ToString());
        cmdVerifica2fa.Parameters.Add("@NOM_2FA", SqlDbType.VarChar).Value = Funcoes.Encrypt(txt2FA.Text.ToString());
        cmdVerifica2fa.Parameters.Add("@DTA_EXPIRACAO", SqlDbType.DateTime).Value = DateTime.Now;
        connVerifica2fa.Open();
        SqlDataReader mReader2fa = cmdVerifica2fa.ExecuteReader();

        if (mReader2fa.Read())
        {

            SolicitarCartao();
        }
        else
        {
            ClientScript.RegisterStartupScript(this.GetType(),
                "CodigoErrado2fa", "alert('O Código de confirmação de autenticação não está correto! Verifique e reentre.');", true);
        }
    }

    private void SolicitarCartao()
    {
        SqlConnection mySelCadastroEstabelecimento = new SqlConnection(Funcoes.conexao());
        mySelCadastroEstabelecimento.Open();
        SqlCommand cmdSelCadastroEstabelecimento = new SqlCommand("dbo.stp_pessoas_fj_ins", mySelCadastroEstabelecimento);
        cmdSelCadastroEstabelecimento.CommandType = CommandType.StoredProcedure;
        cmdSelCadastroEstabelecimento.Parameters.Add("@flg_operacao", SqlDbType.Char).Value = "S";
        cmdSelCadastroEstabelecimento.Parameters.Add("@COD_ID_PESSOA_LICENCIADO", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["LICENCIADO"].ToString());
        cmdSelCadastroEstabelecimento.Parameters.Add("@COD_ID", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["PESSOA"].ToString());
        SqlDataReader ReaderCadastroEstabelecimento = cmdSelCadastroEstabelecimento.ExecuteReader();
        while (ReaderCadastroEstabelecimento.Read())
        {
            SolicitarCartaoAsaas.Root dcartao = new SolicitarCartaoAsaas.Root()
            {
                name = ReaderCadastroEstabelecimento["NOM_NOME"].ToString().Trim() + " " + ReaderCadastroEstabelecimento["NOM_SOBRENOME"].ToString().Trim(),
                email = ReaderCadastroEstabelecimento["NOM_EMAIL"].ToString().Trim(),
                birthDate = String.Format("{0:yyyy-MM-dd}", Convert.ToDateTime(ReaderCadastroEstabelecimento["DTA_ANIVERSARIO"].ToString().Trim())),
                postalCode = Funcoes.TIRAACENTOSDOCUMENTOS(ReaderCadastroEstabelecimento["NOM_CEP"].ToString().Trim()),
                mobilePhone = Funcoes.TIRAACENTOSDOCUMENTOS(ReaderCadastroEstabelecimento["NOM_CELULAR"].ToString().Trim()),
                address = ReaderCadastroEstabelecimento["NOM_ENDERECO"].ToString().Trim(),
                addressNumber = ReaderCadastroEstabelecimento["NOM_NUMERO"].ToString().Trim(),
                complement = ReaderCadastroEstabelecimento["NOM_COMPLEMENTO"].ToString().Trim(),
                province = ReaderCadastroEstabelecimento["NOM_BAIRRO"].ToString().Trim(),
                cpf = Funcoes.TIRAACENTOSDOCUMENTOS(ReaderCadastroEstabelecimento["NOM_CPF"].ToString().Trim()),
                cardName = txtNomeCartao.Text.ToString(),
                type = "ELO_DEBIT"
            };

            string jsonCartao = JsonConvert.SerializeObject(dcartao);
            ClientScript.RegisterStartupScript(this.GetType(), "Mensagem02", "alert('" + jsonCartao.ToString() + "');", true);


            string jsonRetorno = asaas.SolicitaCartoesAsaas(asaas.PegarTokenSubconta(Funcoes.strToInt(HttpContext.Current.Session["PESSOA"].ToString()), Funcoes.strToInt(HttpContext.Current.Session["LICENCIADO"].ToString())), jsonCartao);
            string sID = "";
            string sStatus = "";

            ClientScript.RegisterStartupScript(this.GetType(), "Mensagem01", "alert('"+jsonRetorno.ToString()+"');", true);
            if (jsonRetorno.ToString().Trim() != "")
            {
                JObject o = JObject.Parse(jsonRetorno.ToString());
                try
                {
                    sID = o["id"].ToString();
                    sStatus = o["status"].ToString();
                    ClientScript.RegisterStartupScript(this.GetType(), "SucessoSolicitacao", "alert('Solicitação do cartão ocorreu com sucesso!');", true);
                }
                catch
                {
                    ClientScript.RegisterStartupScript(this.GetType(), "ErroSolicitacao", "alert('Ocorreu um erro ao tentar realizar a solicitação do cartão: " + o["errors"][0]["description"].ToString() + "');", true);
                    sStatus = "ERRO";
                }
                GravaCartao(sID.ToString(), sStatus.ToString(), jsonRetorno.ToString());
            }
        }
        ConsultaSolicitacoes();
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

                    //ClientScript.RegisterStartupScript(this.GetType(), "Consulta", "alert('" + jsonCartoes .ToString()+ "');", true);

                    if (jsonCartoes.ToString().Trim() != "")
                    {
                        try
                        {
                            if (oCartoes["data"].Count() > 0)
                            {
                                for (int i = 0; i < oCartoes["data"].Count(); i++)
                                {
                                    //ClientScript.RegisterStartupScript(this.GetType(), "ConsultaNome"+i.ToString(), "alert('" + oCartoes["data"][i]["name"].ToString() + "');", true);

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