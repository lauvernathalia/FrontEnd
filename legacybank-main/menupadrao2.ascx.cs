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



public partial class menupadrao2 : System.Web.UI.UserControl
{
    protected void Page_Load(object sender, EventArgs e)
    {
        // Verifica se o usuário esta autenticado
        if (HttpContext.Current.Session.Count <= 0)
        {
            Response.Redirect("login.aspx");
            FormsAuthentication.SignOut();
        }

        imgLicenciado.ImageUrl = "../images/logo-dark-2.png";
        string urlorigem = Request.ServerVariables["SERVER_NAME"].ToString();

        SqlConnection mySelCadastro = new SqlConnection(Funcoes.conexao());
        mySelCadastro.Open();
        SqlCommand cmdSelCadastro = new SqlCommand("dbo.stp_pessoas_fj_ins", mySelCadastro);
        cmdSelCadastro.CommandType = CommandType.StoredProcedure;
        cmdSelCadastro.Parameters.Add("@flg_operacao", SqlDbType.Char).Value = "T";
        cmdSelCadastro.Parameters.Add("@NOM_URL", SqlDbType.VarChar).Value = urlorigem.ToString();

        //cmdSelCadastro.Parameters.Add("@COD_ID_PESSOA_LICENCIADO", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["LICENCIADO"].ToString());
        SqlDataReader ReaderCadastro = cmdSelCadastro.ExecuteReader();
        while (ReaderCadastro.Read())
        {
            imgLicenciado.ImageUrl = "public_html/" + ReaderCadastro["NOM_LOGO_MENU"].ToString();
            //asbMenu.Attributes.Add("style", "background-image: linear-gradient(15deg, " + ReaderCadastro["NOM_COR_PRIMARIA_FUNDO"].ToString() + "  0%, " + ReaderCadastro["NOM_COR_SECUNDARIA_FUNDO"].ToString() + "  100%);");
            //dvMenu.Attributes.Add("style", "background-image: linear-gradient(15deg, " + ReaderCadastro["NOM_COR_PRIMARIA_FUNDO"].ToString() + "  0%, " + ReaderCadastro["NOM_COR_SECUNDARIA_FUNDO"].ToString() + "  100%);");
        }

        ConsultaMenu();
    }






    public string Cores()
    {
        string sChavePix = "";
        SqlConnection mySelCadastroBaas = new SqlConnection(Funcoes.conexao());
        mySelCadastroBaas.Open();
        SqlCommand cmdSelCadastroBaas = new SqlCommand("dbo.stp_pessoas_fj_baas_ins", mySelCadastroBaas);
        cmdSelCadastroBaas.CommandType = CommandType.StoredProcedure;
        cmdSelCadastroBaas.Parameters.Add("@flg_operacao", SqlDbType.Char).Value = "S";
        cmdSelCadastroBaas.Parameters.Add("@COD_ID_PESSOA_LICENCIADO", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["LICENCIADO"].ToString());
        cmdSelCadastroBaas.Parameters.Add("@COD_ID_PESSOAS_FJ", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["PESSOA"].ToString());
        cmdSelCadastroBaas.Parameters.Add("@FLG_ORIGEM", SqlDbType.Char).Value = "A";
        SqlDataReader ReaderCadastroBaas = cmdSelCadastroBaas.ExecuteReader();
        while (ReaderCadastroBaas.Read())
        {
            sChavePix = ReaderCadastroBaas["NUM_CHAVE_PIX"].ToString();
        }
        return sChavePix;
    }

    private void ConsultaMenu()
    {
        // Carrega Menu de Opções
        //rptMenu.Visible = true;

        SqlConnection mConnectionMenu = new SqlConnection(Funcoes.conexao());
        SqlCommand cmdConsultaMenu = new SqlCommand("dbo.stp_menu_ins", mConnectionMenu);
        cmdConsultaMenu.CommandType = CommandType.StoredProcedure;
        cmdConsultaMenu.Parameters.Add("@flg_operacao", SqlDbType.Char).Value = "L";
        cmdConsultaMenu.Parameters.Add("@COD_ID_PESSOA_LICENCIADO", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["LICENCIADO"].ToString());
        cmdConsultaMenu.Parameters.Add("@COD_ID_PESSOAS_FJ", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["PESSOA"].ToString());
        cmdConsultaMenu.Parameters.Add("@COD_ID_SIS_USUARIO", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["CODIGO"].ToString());
        cmdConsultaMenu.Parameters.Add("@FLG_TIPO", SqlDbType.Char).Value = HttpContext.Current.Session["TIPO"].ToString();
        lblDadosInternos.Text = HttpContext.Current.Session["TIPO"].ToString() + " - " + HttpContext.Current.Session["PESSOA"].ToString() + " - " + HttpContext.Current.Session["LICENCIADO"].ToString() + " - " + HttpContext.Current.Session["CODIGO"].ToString();
        SqlDataAdapter drConsultaMenu = new SqlDataAdapter();
        drConsultaMenu.SelectCommand = cmdConsultaMenu;

        DataSet dsConsultaMenu = new DataSet();
        drConsultaMenu.Fill(dsConsultaMenu, "MENU");
        //rptMenu.DataSource = dsConsultaMenu.Tables["MENU"].DefaultView;
        //rptMenu.DataBind();
        mConnectionMenu.Close();
        mConnectionMenu.Dispose();
    }




    protected void rptMenu_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        Repeater oGrid = (Repeater)e.Item.FindControl("rptSubmenu");
        oGrid.Visible = true;

        TextBox tMenu = (TextBox)e.Item.FindControl("txtidmenu");

        SqlConnection myConsultaSubmenu = new SqlConnection(Funcoes.conexao());
        myConsultaSubmenu.Open();
        SqlDataAdapter SDAConsultaSubmenu = new SqlDataAdapter("dbo.stp_submenu_ins", myConsultaSubmenu);
        SDAConsultaSubmenu.SelectCommand.CommandType = CommandType.StoredProcedure;
        SDAConsultaSubmenu.SelectCommand.Parameters.Add("@flg_operacao", SqlDbType.Char).Value = "L";
        SDAConsultaSubmenu.SelectCommand.Parameters.Add("@COD_ID_MENU", SqlDbType.Int).Value = Funcoes.strToInt(tMenu.Text.ToString());
        SDAConsultaSubmenu.SelectCommand.Parameters.Add("@COD_ID_PESSOA_LICENCIADO", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["LICENCIADO"].ToString());
        SDAConsultaSubmenu.SelectCommand.Parameters.Add("@COD_ID_PESSOAS_FJ", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["PESSOA"].ToString());
        SDAConsultaSubmenu.SelectCommand.Parameters.Add("@COD_ID_SIS_USUARIO", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["CODIGO"].ToString());
        SDAConsultaSubmenu.SelectCommand.Parameters.Add("@FLG_TIPO", SqlDbType.Char).Value = HttpContext.Current.Session["TIPO"].ToString();
        DataSet dsConsultaSubmenu = new DataSet();
        SDAConsultaSubmenu.Fill(dsConsultaSubmenu, "SUBMENUS");
        oGrid.DataSource = dsConsultaSubmenu.Tables["SUBMENUS"].DefaultView;
        oGrid.DataBind();
        myConsultaSubmenu.Close();
        myConsultaSubmenu.Dispose();

    }


    protected void lkbContaDigital_Click(object sender, EventArgs e)
    {
        SqlConnection mySelCadastro = new SqlConnection(Funcoes.conexao());
        mySelCadastro.Open();
        SqlCommand cmdSelCadastro = new SqlCommand("dbo.stp_pessoas_fj_ins", mySelCadastro);
        cmdSelCadastro.CommandType = CommandType.StoredProcedure;
        cmdSelCadastro.Parameters.Add("@flg_operacao", SqlDbType.Char).Value = "S";
        cmdSelCadastro.Parameters.Add("@COD_ID", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["PESSOA"].ToString());
        cmdSelCadastro.Parameters.Add("@COD_ID_PESSOA_LICENCIADO", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["LICENCIADO"].ToString());
        SqlDataReader ReaderCadastro = cmdSelCadastro.ExecuteReader();
        while (ReaderCadastro.Read())
        {

            string sCompanyType = "";
            if (ReaderCadastro["FLG_TIPO_PESSOA"].ToString().Trim() == "PJ")
            {
                if (ReaderCadastro["NOM_TIPO_EMPRESA"].ToString() == "MEI") { sCompanyType = "MEI"; }
                if (ReaderCadastro["NOM_TIPO_EMPRESA"].ToString() == "Empresário Individual") { sCompanyType = "INDIVIDUAL"; }
                if (ReaderCadastro["NOM_TIPO_EMPRESA"].ToString() == "Sociedade Limitada Unipessoal") { sCompanyType = "LIMITED"; }
                if (ReaderCadastro["NOM_TIPO_EMPRESA"].ToString() == "Sociedade Empresária Limitada") { sCompanyType = "LIMITED"; }
                if (ReaderCadastro["NOM_TIPO_EMPRESA"].ToString() == "Sociedade Simples") { sCompanyType = "LIMITED"; }
                if (ReaderCadastro["NOM_TIPO_EMPRESA"].ToString() == "Sociedade Anônima") { sCompanyType = "LIMITED"; }
            }

            dadosSubconta.Subconta dsubconta = new dadosSubconta.Subconta()
            {
                name = (ReaderCadastro["FLG_TIPO_PESSOA"].ToString().Trim() == "PF") ? ReaderCadastro["NOM_NOME"].ToString().Trim() + " " + ReaderCadastro["NOM_SOBRENOME"].ToString().Trim() : ReaderCadastro["NOM_RAZAOSOCIAL"].ToString().Trim(),
                email = ReaderCadastro["NOM_EMAIL"].ToString().Trim(),
                loginEmail = ReaderCadastro["NOM_EMAIL"].ToString().Trim(),
                birthDate = (ReaderCadastro["FLG_TIPO_PESSOA"].ToString().Trim() == "PF") ? String.Format("{0:yyyy-MM-dd}", Convert.ToDateTime(ReaderCadastro["DTA_ANIVERSARIO"].ToString().Trim())) : "",
                cpfCnpj = (ReaderCadastro["FLG_TIPO_PESSOA"].ToString().Trim() == "PF") ? Funcoes.TIRAACENTOSDOCUMENTOS(ReaderCadastro["NOM_CPF"].ToString().Trim()) : Funcoes.TIRAACENTOSDOCUMENTOS(ReaderCadastro["NOM_CNPJ"].ToString().Trim()),
                companyType = sCompanyType.ToString(),
                mobilePhone = Funcoes.TIRAACENTOSDOCUMENTOS(ReaderCadastro["NOM_CELULAR"].ToString().Trim()),
                incomeValue = (ReaderCadastro["FLG_TIPO_PESSOA"].ToString().Trim() == "PF") ? Funcoes.strToInt(ReaderCadastro["NUM_RENDA_MENSAL"].ToString().Trim()) : Funcoes.strToInt(ReaderCadastro["NUM_FATURAMENTO"].ToString().Trim()),
                address = ReaderCadastro["NOM_ENDERECO"].ToString().Trim(),
                addressNumber = ReaderCadastro["NOM_NUMERO"].ToString().Trim(),
                complement = ReaderCadastro["NOM_COMPLEMENTO"].ToString().Trim(),
                province = ReaderCadastro["NOM_BAIRRO"].ToString().Trim(),
                postalCode = Funcoes.TIRAACENTOSDOCUMENTOS(ReaderCadastro["NOM_CEP"].ToString().Trim()),
                webhooks = new dadosSubconta.webhooks[]
                {
                    new dadosSubconta.webhooks()
                    {
                    name = "WEBHOOK-ID-"+ReaderCadastro["COD_ID"].ToString().Trim().PadLeft(6,'0'),
                    url = "https://conta.legacybank.com.br/events/asaas",
                    email = (HttpContext.Current.Session["EMAIL"].ToString().Trim()!="")? HttpContext.Current.Session["EMAIL"].ToString().Trim() : ReaderCadastro["NOM_EMAIL"].ToString().Trim(),
                    sendType = "SEQUENTIALLY",
                    apiVersion = 3,
                    enabled = true,
                    interrupted = false,
                    authToken = Funcoes.alfanumericoAleatorio(20).ToString(),
                    events = asaas.ListaWebhook
                    }
                }
            };


            string jsonSubconta = JsonConvert.SerializeObject(dsubconta);

            //Page.ClientScript.RegisterStartupScript(this.GetType(),
            //"Alerta", "alert('" + jsonSubconta.ToString() + "');", true);
            //return;


            //Page.ClientScript.RegisterStartupScript(this.GetType(),
            //    "json1", "alert('"+jsonSubconta.ToString()+"');", true);

            //Page.ClientScript.RegisterStartupScript(this.GetType(),
            //    "json2", "alert('" + asaas.TokenAsaas().ToString() + "');", true);
            //txtResposta.Text = jsonSubconta.ToString();
            // return;



            string jsonRetorno = asaas.CriarSubconta(jsonSubconta);
            //txtResposta.Text = txtResposta.Text + " - " + jsonRetorno;

            // tratar os dados de retorno
            try
            {
                JObject o = JObject.Parse(jsonRetorno.ToString());

                SqlConnection connInsCons = new SqlConnection(Funcoes.conexao());
                connInsCons.Open();
                SqlCommand cmdInsCons = new SqlCommand("dbo.stp_pessoas_fj_baas_ins", connInsCons);
                cmdInsCons.CommandType = CommandType.StoredProcedure;
                cmdInsCons.Parameters.Add("@flg_operacao", SqlDbType.Char).Value = "I";
                cmdInsCons.Parameters.Add("@COD_ID_PESSOA_LICENCIADO", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["LICENCIADO"].ToString());
                cmdInsCons.Parameters.Add("@COD_ID_PESSOAS_FJ", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["PESSOA"].ToString());
                cmdInsCons.Parameters.Add("@FLG_ORIGEM", SqlDbType.Char).Value = "A";

                cmdInsCons.Parameters.Add("@NUM_TOKEN_BAAS", SqlDbType.VarChar).Value = o["apiKey"].ToString();
                //cmdInsCons.Parameters.Add("@FLG_STATUS_BAAS", SqlDbType.VarChar).Value = txtStatus.Text.ToString();
                cmdInsCons.Parameters.Add("@FLG_BAAS", SqlDbType.Char).Value = "S";
                cmdInsCons.Parameters.Add("@DES_JSON_BAAS", SqlDbType.VarChar).Value = jsonRetorno.ToString();

                cmdInsCons.Parameters.Add("@NUM_WALLETID_BAAS", SqlDbType.VarChar).Value = o["walletId"].ToString();
                cmdInsCons.Parameters.Add("@NUM_ID_CONTA_BAAS", SqlDbType.VarChar).Value = o["id"].ToString();
                cmdInsCons.Parameters.Add("@NUM_AGENCIA_BAAS", SqlDbType.VarChar).Value = o["accountNumber"]["agency"].ToString();
                cmdInsCons.Parameters.Add("@NUM_CONTA_BAAS", SqlDbType.VarChar).Value = o["accountNumber"]["account"].ToString();
                cmdInsCons.Parameters.Add("@NUM_DIGITO_CONTA_BAAS", SqlDbType.VarChar).Value = o["accountNumber"]["accountDigit"].ToString();

                //cmdInsCons.Parameters.Add("@NUM_CHAVE_PIX", SqlDbType.VarChar).Value = txtChavePix.Text.ToString();

                cmdInsCons.ExecuteNonQuery();
                connInsCons.Close();
                connInsCons.Dispose();


                //ClientScript.RegisterStartupScript(this.GetType(),
                //    "Alerta", "alert('Conta digital criada sucesso');", true);



                //txtToken.Text = o["apiKey"].ToString();
                //txtIDConta.Text = o["id"].ToString();
                //txtAgencia.Text = o["accountNumber"]["agency"].ToString();
                //txtConta.Text = o["accountNumber"]["account"].ToString();
                //txtDigitoConta.Text = o["accountNumber"]["accountDigit"].ToString();
                Page.ClientScript.RegisterStartupScript(this.GetType(),
                    "Alerta", "alert('A conta digital foi criada com sucesso');", true);
            }
            catch
            {
                if (Funcoes.strToInt(HttpContext.Current.Session["PESSOA"].ToString()) == 179)
                {
                    Page.ClientScript.RegisterStartupScript(this.GetType(),
                        "Alerta", "alert('Ocorreu um erro ao tentar criar a sua conta digital! Verifique e tente novamente: " + jsonRetorno + "');", true);
                }
                else
                {
                    //txtToken.Text = o["errors"][0]["description"].ToString();
                    Page.ClientScript.RegisterStartupScript(this.GetType(),
                        "Alerta", "alert('Ocorreu um erro ao tentar criar a sua conta digital! Verifique e tente novamente');", true);
                }
            }
        }
    }


}