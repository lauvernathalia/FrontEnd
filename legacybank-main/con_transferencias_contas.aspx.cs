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


public partial class con_transferencias_contas : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (HttpContext.Current.Session.Count <= 0)
        {
            Response.Redirect("login.aspx");
            FormsAuthentication.SignOut();
        }

        //Funcoes.CONFIRMA(btnConfirmar, "Confirma o pagamento do PIX informado?");

        if (!IsPostBack)
        {
            // CARREGA CONTAS ORIGEM

            SqlConnection myOrigem = new SqlConnection(Funcoes.conexao());
            myOrigem.Open();
            SqlCommand cmdOrigem = new SqlCommand("dbo.stp_pessoas_fj_baas_ins", myOrigem);
            cmdOrigem.CommandType = CommandType.StoredProcedure;
            cmdOrigem.Parameters.Add("@flg_operacao", SqlDbType.VarChar).Value = "F";
            cmdOrigem.Parameters.Add("@COD_ID_PESSOA_LICENCIADO", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["LICENCIADO"].ToString());

            SqlDataAdapter drOrigem = new SqlDataAdapter();
            drOrigem.SelectCommand = cmdOrigem;
            DataSet dsOrigem = new DataSet();
            drOrigem.Fill(dsOrigem, "PESSOAS_FJ_BAAS");

            ddlOrigem.DataTextField = "NOM_NOME";
            ddlOrigem.DataValueField = "COD_ID_PESSOAS_FJ";
            ddlOrigem.DataSource = dsOrigem.Tables["PESSOAS_FJ_BAAS"].DefaultView;
            ddlOrigem.DataBind();
            ddlOrigem.Items.Insert(0, new ListItem("", ""));

            // CARREGA CONTAS DESTINO

            ddlDestino.DataTextField = "NOM_NOME";
            ddlDestino.DataValueField = "COD_ID_PESSOAS_FJ";
            ddlDestino.DataSource = dsOrigem.Tables["PESSOAS_FJ_BAAS"].DefaultView;
            ddlDestino.DataBind();
            ddlDestino.Items.Insert(0, new ListItem("", ""));


            Funcoes.GravaAuditoria(0, Request.ServerVariables["REMOTE_ADDR"].ToString(), "Pagamentos e Transferências", "Realizar Pagamento/Transferência");

            DateTime data = DateTime.Today;
            DateTime primeiroDiaDoMes = new DateTime(data.Year, data.Month, 1);
            DateTime ultimoDiaDoMes = new DateTime(data.Year, data.Month, DateTime.DaysInMonth(data.Year, data.Month));
            txtDataIni.Text = primeiroDiaDoMes.ToShortDateString();
            txtDataFim.Text = ultimoDiaDoMes.ToShortDateString();

            ConsultaSaldoBaas("A");

            ConsultaGeral();
        }
    }


    private void ConsultaGeral()
    {
        SqlConnection myConsulta = new SqlConnection(Funcoes.conexao());
        myConsulta.Open();
        SqlDataAdapter SDAConsulta = new SqlDataAdapter("dbo.stp_transferencias_ins", myConsulta);
        SDAConsulta.SelectCommand.CommandType = CommandType.StoredProcedure;
        SDAConsulta.SelectCommand.Parameters.Add("@flg_operacao", SqlDbType.Char).Value = "C";
        SDAConsulta.SelectCommand.Parameters.Add("@COD_ID_PESSOA_LICENCIADO", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["LICENCIADO"].ToString());
        SDAConsulta.SelectCommand.Parameters.Add("@COD_ID_PESSOAS_FJ", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["PESSOA"].ToString());
        SDAConsulta.SelectCommand.Parameters.Add("@DTA_INICIO", SqlDbType.DateTime).Value = Convert.ToDateTime(txtDataIni.Text.ToString());
        SDAConsulta.SelectCommand.Parameters.Add("@DTA_FIM", SqlDbType.DateTime).Value = Convert.ToDateTime(txtDataFim.Text.ToString());

        DataSet dsConsulta = new DataSet();
        SDAConsulta.Fill(dsConsulta, "TRANSFERENCIAS");
        rptConsulta.DataSource = dsConsulta.Tables["TRANSFERENCIAS"].DefaultView;
        rptConsulta.DataBind();
        myConsulta.Close(); myConsulta.Dispose();

    }

    protected void btnConfirmarPagamentoTransferencia2FA_Click(object sender, System.EventArgs e)
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

            ClientScript.RegisterStartupScript(this.GetType(),
    "ExecutaModal", "$('#mdSenha').modal('show');", true);

        }
        else
        {
            ClientScript.RegisterStartupScript(this.GetType(),
                "CodigoErrado2fa", "alert('O Código de confirmação de autenticação não está correto! Verifique e reentre.');", true);
        }
    }
    protected void btnConfirmarSenha_Click(object sender, System.EventArgs e)
    {
        if (HttpContext.Current.Session["SENHA"].ToString().Trim() == Funcoes.Encrypt(txtSenha.Text.ToString()))
        {
            ConfirmarTransferencia();
        }
        else
        {
            ClientScript.RegisterStartupScript(this.GetType(),
                "CodigoErrado2fa", "alert('Senha inválida! Verifique e reentre.');", true);
        }

    }
    protected void lkbReenviar_Click(object sender, System.EventArgs e)
    {
        if (Funcoes.Enviar2fa() == true)
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

    protected void btnConfirmar_Click(object sender, System.EventArgs e)
    {
        if (ddlOrigem.SelectedValue.ToString() == ddlDestino.SelectedValue.ToString())
        {
            ClientScript.RegisterStartupScript(this.GetType(), "ErroOrigemDestino", "alert('ATENÇÃO! A conta de origem e de destino não podem ser iguais. Verifique e tente novamente');", true);
            return;
        }
        if (ddlOrigem.SelectedValue.ToString().Trim() == "")
        {
            ClientScript.RegisterStartupScript(this.GetType(), "ErroOrigem", "alert('ATENÇÃO! É obrigatório selecionar uma conta de origem. Verifique e tente novamente');", true);
            return;
        }
        if (ddlDestino.SelectedValue.ToString().Trim() == "")
        {
            ClientScript.RegisterStartupScript(this.GetType(), "ErroDestino", "alert('ATENÇÃO! É obrigatório selecionar uma conta de destino. Verifique e tente novamente');", true);
            return;
        }
        if (txtValor.Text.ToString().Trim() == "")
        {
            ClientScript.RegisterStartupScript(this.GetType(), "ErroValor", "alert('ATENÇÃO! É obrigatório especificar um valor. Verifique e tente novamente');", true);
            return;
        }

        //ConfirmarTransferencia();
        
        if (Funcoes.Enviar2fa() == true)
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

    private void ConfirmarTransferencia()
    {
        string sGuid = Guid.NewGuid().ToString();
        // Localizar ID Conta (WalletID) Destino
        string sContaDestino = "";

        SqlConnection mySelCadastro = new SqlConnection(Funcoes.conexao());
        mySelCadastro.Open();
        SqlCommand cmdSelCadastro = new SqlCommand("dbo.stp_pessoas_fj_baas_ins", mySelCadastro);
        cmdSelCadastro.CommandType = CommandType.StoredProcedure;
        cmdSelCadastro.Parameters.Add("@flg_operacao", SqlDbType.Char).Value = "S";
        cmdSelCadastro.Parameters.Add("@COD_ID_PESSOA_LICENCIADO", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["LICENCIADO"].ToString());
        cmdSelCadastro.Parameters.Add("@COD_ID_PESSOAS_FJ", SqlDbType.Int).Value = Funcoes.strToInt(ddlDestino.SelectedValue.ToString());
        cmdSelCadastro.Parameters.Add("@FLG_ORIGEM", SqlDbType.Char).Value = "A";
        SqlDataReader ReaderCadastro = cmdSelCadastro.ExecuteReader();
        while (ReaderCadastro.Read())
        {
            string jsonSubcontaWalletID = asaas.ListarSubcontaID("", ReaderCadastro["NUM_ID_CONTA_BAAS"].ToString());
            JObject oWalletDestino = JObject.Parse(jsonSubcontaWalletID);
            sContaDestino = oWalletDestino["walletId"].ToString();
        }

        dadosTransferenciaAsaaS.Root dtransferencia = new dadosTransferenciaAsaaS.Root()
        {
            walletId = sContaDestino.ToString(),
            value = Funcoes.strToDouble(txtValor.Text.ToString()),
            externalReference = sGuid.ToString()
        };
        string jsonTransferencia = JsonConvert.SerializeObject(dtransferencia);
        string jsonTransferenciaRetorno = asaas.TransferenciaPIXAsaaS(asaas.PegarTokenSubconta(Funcoes.strToInt(ddlOrigem.SelectedValue.ToString()), Funcoes.strToInt(HttpContext.Current.Session["LICENCIADO"].ToString())), jsonTransferencia);
        JObject oTransferencia = JObject.Parse(jsonTransferenciaRetorno);

        if (jsonTransferenciaRetorno.ToString().Trim() != "")
        {
            try
            {
                // Gravar dados da transferencia
                SqlConnection connInsConsTransf = new SqlConnection(Funcoes.conexao());
                connInsConsTransf.Open();
                SqlCommand cmdInsConsTransf = new SqlCommand("dbo.stp_transferencias_ins", connInsConsTransf);
                cmdInsConsTransf.CommandType = CommandType.StoredProcedure;
                cmdInsConsTransf.Parameters.Add("@flg_operacao", SqlDbType.Char).Value = 'I';
                cmdInsConsTransf.Parameters.Add("@COD_ID_PESSOA_LICENCIADO", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["LICENCIADO"].ToString());
                cmdInsConsTransf.Parameters.Add("@COD_ID_PESSOAS_FJ", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["PESSOA"].ToString());

                cmdInsConsTransf.Parameters.Add("@COD_ID_PESSOAS_FJ_ORIGEM", SqlDbType.Int).Value = Funcoes.strToInt(ddlOrigem.SelectedValue.ToString());
                cmdInsConsTransf.Parameters.Add("@COD_ID_PESSOAS_FJ_DESTINO", SqlDbType.Int).Value = Funcoes.strToInt(ddlDestino.SelectedValue.ToString());
                cmdInsConsTransf.Parameters.Add("@COD_ID_TRANSFERENCIA", SqlDbType.VarChar).Value = oTransferencia["id"].ToString();
                cmdInsConsTransf.Parameters.Add("@NOM_REFERENCIA", SqlDbType.VarChar).Value = sGuid.ToString();
                cmdInsConsTransf.Parameters.Add("@DTA_DATA", SqlDbType.DateTime).Value = DateTime.Now;
                cmdInsConsTransf.Parameters.Add("@DTA_TRANSFERENCIA", SqlDbType.DateTime).Value = DateTime.Now;
                cmdInsConsTransf.Parameters.Add("@DES_JSON", SqlDbType.Text).Value = jsonTransferenciaRetorno.ToString();
                cmdInsConsTransf.Parameters.Add("@FLG_TIPO", SqlDbType.Char).Value = "T";
                cmdInsConsTransf.Parameters.Add("@NUM_VALOR", SqlDbType.Float).Value = Funcoes.strToDouble(txtValor.Text.ToString());
                cmdInsConsTransf.Parameters.Add("@DES_DESCRICAO", SqlDbType.Text).Value = txtDescricao.Text.ToString();
                cmdInsConsTransf.Parameters.Add("@FLG_STATUS", SqlDbType.VarChar).Value = oTransferencia["status"].ToString();
                cmdInsConsTransf.ExecuteNonQuery();
                connInsConsTransf.Close();
                connInsConsTransf.Dispose();

                ClientScript.RegisterStartupScript(this.GetType(), "ConsultaSucesso", "alert('Transferência realizada com sucesso!');", true);
            }
            catch
            {
                try
                {
                    ClientScript.RegisterStartupScript(this.GetType(), "ConsultaErro", "alert('ATENÇÃO! " + oTransferencia["errors"][0]["description"].ToString() + "');", true);
                }
                catch
                {
                    ClientScript.RegisterStartupScript(this.GetType(), "ConsultaErroGeral", "alert('ATENÇÃO! Ocorreu um erro ao tentar realizar a transferência! Verifique e tente novamente.');", true);
                }
            }
        }

    }
    protected void btnPesquisarGestao_Click(object sender, System.EventArgs e)
    {
        ConsultaGeral();
    }
    protected void rptConsulta_OnItemCommand(object source, System.Web.UI.WebControls.RepeaterCommandEventArgs e)
    {

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