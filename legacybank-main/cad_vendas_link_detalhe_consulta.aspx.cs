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


public partial class cad_vendas_link_detalhe_consulta : System.Web.UI.Page
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
            ConsultaDetalhes();
        }
    }

    private void ConsultaDetalhes()
    {
        SqlConnection mySelCadastro = new SqlConnection(Funcoes.conexao());
        mySelCadastro.Open();
        SqlCommand cmdSelCadastro = new SqlCommand("dbo.stp_vendas_vendas_ins", mySelCadastro);
        cmdSelCadastro.CommandType = CommandType.StoredProcedure;
        cmdSelCadastro.Parameters.Add("@flg_operacao", SqlDbType.Char).Value = "S";
        cmdSelCadastro.Parameters.Add("@COD_ID", SqlDbType.Int).Value = Funcoes.strToInt(sid_id.ToString());
        cmdSelCadastro.Parameters.Add("@COD_ID_PESSOA_LICENCIADO", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["LICENCIADO"].ToString());
        cmdSelCadastro.Parameters.Add("@COD_ID_PESSOAS_FJ_VENDEDOR", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["PESSOA"].ToString());

        SqlDataReader ReaderCadastro = cmdSelCadastro.ExecuteReader();

        while (ReaderCadastro.Read())
        {
            txtIDTransacao.Text = ReaderCadastro["NOM_CODE"].ToString();

            txtNome.Text = ReaderCadastro["NOM_NOME"].ToString();
            txtSobrenome.Text = ReaderCadastro["NOM_SOBRENOME"].ToString();
            txtDocumento.Text = ReaderCadastro["NUM_CNPJCPF"].ToString();
            txtEmail.Text = ReaderCadastro["NOM_EMAIL"].ToString();
            txtCelular.Text = ReaderCadastro["NOM_CELULAR"].ToString();

            txtEndereco.Text = ReaderCadastro["NOM_ENDERECO"].ToString();
            txtNumero.Text = ReaderCadastro["NOM_NUMERO"].ToString();
            txtComplemento.Text = ReaderCadastro["NOM_COMPLEMENTO"].ToString();
            txtBairro.Text = ReaderCadastro["NOM_BAIRRO"].ToString();
            txtCidade.Text = ReaderCadastro["NOM_CIDADE"].ToString();
            ddlEstado.SelectedValue = ReaderCadastro["NOM_UF"].ToString();
            txtCEP.Text = ReaderCadastro["NOM_CEP"].ToString();

            if (ReaderCadastro["FLG_BOLETO"].ToString().Trim() == "S")
            {
                btnCancelar.Visible = true;
            }
            else
            {
                btnCancelar.Visible = false;
            }


            if (ReaderCadastro["FLG_ENDERECO_ENTREGA"].ToString().Trim() == "N")
            {
                dvEntrega.Visible = false;
            }
            else
            {
                dvEntrega.Visible = true;
                ckbEnderecoEntrega.Checked = true;
                ckbEnderecoEntrega.Enabled = false;

                txtEnderecoEntrega.Text = ReaderCadastro["NOM_ENDERECO_ENTREGA"].ToString();
                txtNumeroEntrega.Text = ReaderCadastro["NOM_NUMERO_ENTREGA"].ToString();
                txtComplementoEntrega.Text = ReaderCadastro["NOM_COMPLEMENTO_ENTREGA"].ToString();
                txtBairroEntrega.Text = ReaderCadastro["NOM_BAIRRO_ENTREGA"].ToString();
                txtCidadeEntrega.Text = ReaderCadastro["NOM_CIDADE_ENTREGA"].ToString();
                ddlEstadoEntrega.SelectedValue = ReaderCadastro["NOM_UF_ENTREGA"].ToString();
                txtCEPEntrega.Text = ReaderCadastro["NOM_CEP_ENTREGA"].ToString();
            }


            if (ReaderCadastro["NOM_CAMPO_01"].ToString().Trim() == "")
            {
                dvAdicionais.Visible = false;
            }
            else
            {
                dvAdicionais.Visible = true;
                lblAdicional.Text = ReaderCadastro["NOM_CAMPO_01"].ToString();
                txtConteudo01.Text = ReaderCadastro["NOM_CONTEUDO_CAMPO_01"].ToString();
            }

        }
    }
    protected void btnFechar_Click(object sender, EventArgs e)
    {
        ClientScript.RegisterStartupScript(this.GetType(),
"Alerta", "window.close();", true);
    }
    protected void btnCancelar_Click(object sender, EventArgs e)
    {

        ServicePointManager.Expect100Continue = true;
        ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12
        | SecurityProtocolType.Tls11
        | SecurityProtocolType.Tls
        | SecurityProtocolType.Ssl3;

        var byteArray = Encoding.ASCII.GetBytes(HttpContext.Current.Session["USERNAMEMARKETPLACE"].ToString() + ":" + "");
        string encodeString = Convert.ToBase64String(byteArray);


            string PARAMS = txtIDTransacao.Text.ToString();

            var myUri = new Uri("https://api.payments.zoop.ws/boletos/cancellation/marketplaces/" + HttpContext.Current.Session["IDMARKETPLACE"].ToString() + "/transactions/" + PARAMS.ToString());
            var myWebRequest = WebRequest.Create(myUri);
            var myHttpWebRequest = (HttpWebRequest)myWebRequest;

            myHttpWebRequest.Accept = "application/json";
            myHttpWebRequest.Method = "POST";
            myHttpWebRequest.PreAuthenticate = true;
            myHttpWebRequest.Headers.Add("Authorization", "Basic " + encodeString.ToString() + "");

            var myWebResponse = myWebRequest.GetResponse();
            var responseStream = myWebResponse.GetResponseStream();

            StreamReader myStreamReader = new StreamReader(responseStream, Encoding.UTF8);
            var json = myStreamReader.ReadToEnd();



        //string json = zoop.EstornarTransacao(txtIDTransacao.Text.ToString());
        //JObject o = JObject.Parse(json);
        ClientScript.RegisterStartupScript(this.GetType(),
           "Alerta", "alert('"+json.ToString()+"'); ", true);

    }
}