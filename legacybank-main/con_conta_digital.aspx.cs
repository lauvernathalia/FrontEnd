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


public partial class con_conta_digital : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            ConsultaContaDigital();
        }
    }

    private void ConsultaContaDigital()
    {
        asaas.ContaDigital dContaDigital = new asaas.ContaDigital();
        dContaDigital = asaas.ConsultaContaDigital();
        txtBanco.Text = "461";
        txtAgencia.Text = dContaDigital.Agencia.ToString();
        txtConta.Text = dContaDigital.Conta.ToString() + "-" + dContaDigital.DigitoConta.ToString();
        txtChavePix.Text = dContaDigital.ChavePix.ToString();
        txtWalletID.Text = dContaDigital.WalletID.ToString();

        SqlConnection mySelCadastro = new SqlConnection(Funcoes.conexao());
        mySelCadastro.Open();
        SqlCommand cmdSelCadastro = new SqlCommand("dbo.stp_pessoas_fj_baas_ins", mySelCadastro);
        cmdSelCadastro.CommandType = CommandType.StoredProcedure;
        cmdSelCadastro.Parameters.Add("@flg_operacao", SqlDbType.Char).Value = "S";
        cmdSelCadastro.Parameters.Add("@COD_ID_PESSOAS_FJ", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["PESSOA"].ToString());
        cmdSelCadastro.Parameters.Add("@COD_ID_PESSOA_LICENCIADO", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["LICENCIADO"].ToString());
        cmdSelCadastro.Parameters.Add("@FLG_ORIGEM", SqlDbType.Char).Value = "A";
        SqlDataReader ReaderCadastro = cmdSelCadastro.ExecuteReader();
        while (ReaderCadastro.Read())
        {
            ddlAntecipacaoAutomatica.SelectedValue = ReaderCadastro["FLG_ANTECIPACAO"].ToString();
            txtTokenAsaas.Text = Funcoes.Encrypt(ReaderCadastro["NUM_TOKEN_BAAS"].ToString());

            if (Funcoes.Decrypt(txtTokenAsaas.Text.ToString().Trim()) != "")
            {
                string ConsultaStatus = asaas.ListarStatus(Funcoes.Decrypt(txtTokenAsaas.Text.ToString()));
                try
                {
                    JObject oStatus = JObject.Parse(ConsultaStatus.ToString());
                    txtStatusAsaas.Text = RetornaStatus(oStatus["general"].ToString());
                }
                catch
                {
                    txtStatusAsaas.Text = "";
                }
            }


        }
    }

    protected void lbkGerar_Click(object sender, EventArgs e)
    {
        if (txtChavePix.Text.ToString().Trim() == "")
        {
            asaas.ChavePix dChavePix = new asaas.ChavePix()
            {
                type = "EVP"
            };
            string jsonChavePix  = JsonConvert.SerializeObject(dChavePix);
            
            string sRetornoChavePix = asaas.CriarChavePix(asaas.PegarTokenSubconta(Funcoes.strToInt(HttpContext.Current.Session["PESSOA"].ToString()), Funcoes.strToInt(HttpContext.Current.Session["LICENCIADO"].ToString())), jsonChavePix.ToString());
            
            JObject oChavePix = JObject.Parse(sRetornoChavePix.ToString());

            try
            {
                SqlConnection connInsCons = new SqlConnection(Funcoes.conexao());
                connInsCons.Open();
                SqlCommand cmdInsCons = new SqlCommand("dbo.stp_pessoas_fj_baas_ins", connInsCons);
                cmdInsCons.CommandType = CommandType.StoredProcedure;
                cmdInsCons.Parameters.Add("@flg_operacao", SqlDbType.Char).Value = 'P';
                cmdInsCons.Parameters.Add("@COD_ID_PESSOA_LICENCIADO", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["LICENCIADO"].ToString());
                cmdInsCons.Parameters.Add("@COD_ID_PESSOAS_FJ", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["PESSOA"].ToString());
                cmdInsCons.Parameters.Add("@FLG_ORIGEM", SqlDbType.Char).Value = "A";

                cmdInsCons.Parameters.Add("@NUM_CHAVE_PIX", SqlDbType.VarChar).Value = oChavePix["key"].ToString();
                cmdInsCons.Parameters.Add("@NUM_ID_CHAVE_PIX", SqlDbType.VarChar).Value = oChavePix["id"].ToString();
                cmdInsCons.Parameters.Add("@FLG_STATUS_CHAVE_PIX", SqlDbType.VarChar).Value = oChavePix["status"].ToString();
                cmdInsCons.Parameters.Add("@NOM_IMAGEM_CHAVE_PIX", SqlDbType.VarChar).Value = oChavePix["qrCode"]["encodedImage"].ToString();
                cmdInsCons.Parameters.Add("@NOM_QRCODE_CHAVE_PIX", SqlDbType.VarChar).Value = oChavePix["qrCode"]["payload"].ToString();
                cmdInsCons.Parameters.Add("@DES_JSON_CHAVE_PIX", SqlDbType.Text).Value = sRetornoChavePix.ToString();

                cmdInsCons.ExecuteNonQuery();
                connInsCons.Close();
                connInsCons.Dispose();


                txtChavePix.Text = oChavePix["key"].ToString();

                ClientScript.RegisterStartupScript(this.GetType(),
                    "Alerta", "alert('A chave PIX foi criada com sucesso!');", true);
            }
            catch
            {
                try
                {
                    ClientScript.RegisterStartupScript(this.GetType(),
                        "ErroChavePix", "alert('Ocorreu um erro ao tentar criar a chave PIX - erro: " + oChavePix["errors"][0]["description"].ToString() + "!');", true);
                }
                catch
                {
                    ClientScript.RegisterStartupScript(this.GetType(),
                        "ErroMensagem", "alert('Ocorreu um erro ao tentar criar a chave PIX!');", true);
                }
            }
        }
        else
        {
            ClientScript.RegisterStartupScript(this.GetType(),
                "Alerta", "alert('A chave PIX já existe!');", true);
        }
        ConsultaContaDigital();
    }

    protected void lbkCopiar_Click(object sender, EventArgs e)
    {
        ClientScript.RegisterStartupScript(this.GetType(),
            "CopiarAreaTransferencia", "var copyText = document.getElementById('txtChavePix');        copyText.select();        copyText.setSelectionRange(0, 99999); // For mobile devices        navigator.clipboard.writeText(copyText.value);        alert('Chave Pix: ' + copyText.value + ' copiada com sucesso!');", true);
    }

    protected void btnAntecipacaoAutomatica_Click(object sender, EventArgs e)
    {
        
        asaas.DadosAntecipacaoAutomatica dantecipacao = new asaas.DadosAntecipacaoAutomatica()
        {
            creditCardAutomaticEnabled = (ddlAntecipacaoAutomatica.SelectedValue.ToString().Trim() == "S") ? true : false
        };

        string json = JsonConvert.SerializeObject(dantecipacao);
        try
        {
            string jsonAntecipacao = asaas.AntecipacaoAutomatica(asaas.PegarTokenSubconta(Funcoes.strToInt(HttpContext.Current.Session["PESSOA"].ToString()), Funcoes.strToInt(HttpContext.Current.Session["LICENCIADO"].ToString())), json);
            if (jsonAntecipacao.ToString().Trim() != "")
            {
                try
                {
                    JObject oAntecipacao = JObject.Parse(jsonAntecipacao.ToString());
                    ClientScript.RegisterStartupScript(this.GetType(), "SucessoAntecipacao", "alert('Atualização do status de antecipação realizada com sucesso!');", true);
                    
                    SqlConnection connInsCons = new SqlConnection(Funcoes.conexao());
                    connInsCons.Open();
                    SqlCommand cmdInsCons = new SqlCommand("dbo.stp_pessoas_fj_baas_ins", connInsCons);
                    cmdInsCons.CommandType = CommandType.StoredProcedure;
                    cmdInsCons.Parameters.Add("@flg_operacao", SqlDbType.Char).Value = 'N';
                    cmdInsCons.Parameters.Add("@COD_ID_PESSOA_LICENCIADO", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["LICENCIADO"].ToString());
                    cmdInsCons.Parameters.Add("@COD_ID_PESSOAS_FJ", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["PESSOA"].ToString());
                    cmdInsCons.Parameters.Add("@FLG_ORIGEM", SqlDbType.Char).Value = "A";

                    cmdInsCons.Parameters.Add("@FLG_ANTECIPACAO", SqlDbType.VarChar).Value = ddlAntecipacaoAutomatica.SelectedValue.ToString();

                    cmdInsCons.ExecuteNonQuery();
                    connInsCons.Close();
                    connInsCons.Dispose();

                }
                catch
                {
                    ClientScript.RegisterStartupScript(this.GetType(), "SucessoAntecipacao", "alert('Ocorreu um erro ao tentar atualizar o status de antecipação! Verifique e tente novamente');", true);
                }
            }
        }
        catch
        {
            ClientScript.RegisterStartupScript(this.GetType(), "SucessoAntecipacao", "alert('Ocorreu um erro ao tentar atualizar o status de antecipação! Verifique e tente novamente');", true);
        }
        
    }
    protected void btnExcluir_Click(object sender, EventArgs e)
    {
        if (txtMotivo.Text.ToString().Trim() != "")
        {
            try
            {
                string jsonDeletarConta = asaas.DeletarConta(asaas.PegarTokenSubconta(Funcoes.strToInt(HttpContext.Current.Session["PESSOA"].ToString()), Funcoes.strToInt(HttpContext.Current.Session["LICENCIADO"].ToString())), txtMotivo.Text.ToString().Trim());
                JObject oDeletarConta = JObject.Parse(jsonDeletarConta.ToString());
                try
                {

                    ClientScript.RegisterStartupScript(this.GetType(), "SucessoExclusao", "alert('" + oDeletarConta["observations"].ToString() + "');", true);

                    // Salvar Motivo

                    SqlConnection connInsCons = new SqlConnection(Funcoes.conexao());
                    connInsCons.Open();
                    SqlCommand cmdInsCons = new SqlCommand("dbo.stp_pessoas_fj_baas_ins", connInsCons);
                    cmdInsCons.CommandType = CommandType.StoredProcedure;
                    cmdInsCons.Parameters.Add("@flg_operacao", SqlDbType.Char).Value = 'E';
                    cmdInsCons.Parameters.Add("@COD_ID_PESSOA_LICENCIADO", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["LICENCIADO"].ToString());
                    cmdInsCons.Parameters.Add("@COD_ID_PESSOAS_FJ", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["PESSOA"].ToString());
                    cmdInsCons.Parameters.Add("@FLG_ORIGEM", SqlDbType.Char).Value = "A";

                    cmdInsCons.Parameters.Add("@DTA_EXCLUSAO", SqlDbType.DateTime).Value = DateTime.Now;
                    cmdInsCons.Parameters.Add("@NOM_MOTIVO_EXCLUSAO", SqlDbType.VarChar).Value = txtMotivo.Text.ToString();
                    cmdInsCons.Parameters.Add("@COD_ID_USUARIO_EXCLUSAO", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["CODIGO"].ToString());

                    cmdInsCons.ExecuteNonQuery();
                    connInsCons.Close();
                    connInsCons.Dispose();

                }
                catch
                {
                    ClientScript.RegisterStartupScript(this.GetType(), "ErroExclusao", "alert('" + oDeletarConta["observations"].ToString() + "');", true);
                }

                txtMotivo.Text = jsonDeletarConta;
            }
            catch
            {
                ClientScript.RegisterStartupScript(this.GetType(), "ErroGeral", "alert('Ocorreu um erro ao tentar excluir a conta! Verifique e tente novamente');", true);
            }

            //ConsultaAsaas();

        }
        else
        {
            ClientScript.RegisterStartupScript(this.GetType(),
"EspecificarMotivo", "alert('É Obrigatório especificar o motivo da exclusão da conta! Verifique e tente novamente.');", true);
        }
    }


    public static string RetornaStatus(string sStatus)
    {
        string sRetorno = "";
        switch (sStatus)
        {
            case "REJECTED":
                sRetorno = "Conta reprovada";
                break;
            case "APPROVED":
                sRetorno = "Conta aprovada";
                break;
            case "AWAITING_APPROVAL":
                sRetorno = "Aguardando análise";
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