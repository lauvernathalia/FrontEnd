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


public partial class con_configuracoes_gerais : System.Web.UI.Page
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
            ConsultaCadastro();
        }
    }
    protected void btnSalvar_Click(object sender, EventArgs e)
    {

        SqlConnection connInsConsConfiguracoes = new SqlConnection(Funcoes.conexao());
        connInsConsConfiguracoes.Open();
        SqlCommand cmdInsConsConfiguracoes = new SqlCommand("dbo.stp_pessoas_fj_configuracoes_ins", connInsConsConfiguracoes);
        cmdInsConsConfiguracoes.CommandType = CommandType.StoredProcedure;
        cmdInsConsConfiguracoes.Parameters.Add("@flg_operacao", SqlDbType.Char).Value = 'I';
        cmdInsConsConfiguracoes.Parameters.Add("@COD_ID_PESSOA_LICENCIADO", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["LICENCIADO"].ToString());
        cmdInsConsConfiguracoes.Parameters.Add("@COD_ID_PESSOAS_FJ", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["PESSOA"].ToString());

        cmdInsConsConfiguracoes.Parameters.Add("@DTA_DATA_ALTERACAO", SqlDbType.DateTime).Value = DateTime.Now;

        // Multa
        cmdInsConsConfiguracoes.Parameters.Add("@FLG_MULTA", SqlDbType.Char).Value = (ckbMulta.Checked==true)? "S": "N";
        cmdInsConsConfiguracoes.Parameters.Add("@FLG_TIPO_MULTA", SqlDbType.Char).Value = ddlMulta.SelectedValue.ToString();
        cmdInsConsConfiguracoes.Parameters.Add("@NUM_MULTA", SqlDbType.Float).Value = Funcoes.strToDouble(txtMulta.Text.ToString());

        // Juros
        cmdInsConsConfiguracoes.Parameters.Add("@FLG_JUROS", SqlDbType.Char).Value = (ckbJuros.Checked == true) ? "S" : "N";
        cmdInsConsConfiguracoes.Parameters.Add("@FLG_TIPO_JUROS", SqlDbType.Char).Value = ddlJuros.SelectedValue.ToString();
        cmdInsConsConfiguracoes.Parameters.Add("@NUM_JUROS", SqlDbType.Float).Value = Funcoes.strToDouble(txtJuros.Text.ToString());

        // Desconto
        cmdInsConsConfiguracoes.Parameters.Add("@FLG_DESCONTO", SqlDbType.Char).Value = (ckbDesconto.Checked == true) ? "S" : "N";
        cmdInsConsConfiguracoes.Parameters.Add("@FLG_TIPO_DESCONTO", SqlDbType.Char).Value = ddlDesconto.SelectedValue.ToString();
        cmdInsConsConfiguracoes.Parameters.Add("@NUM_DESCONTO", SqlDbType.Float).Value = Funcoes.strToDouble(txtDesconto.Text.ToString());
        cmdInsConsConfiguracoes.Parameters.Add("@NUM_DIAS", SqlDbType.Int).Value = Funcoes.strToInt(txtDias.Text.ToString());

        cmdInsConsConfiguracoes.ExecuteNonQuery();
        connInsConsConfiguracoes.Close();
        connInsConsConfiguracoes.Dispose();

    }

    private void ConsultaCadastro()
    {
        SqlConnection mySelCadastro = new SqlConnection(Funcoes.conexao());
        mySelCadastro.Open();
        SqlCommand cmdSelCadastro = new SqlCommand("dbo.stp_pessoas_fj_configuracoes_ins", mySelCadastro);
        cmdSelCadastro.CommandType = CommandType.StoredProcedure;
        cmdSelCadastro.Parameters.Add("@flg_operacao", SqlDbType.Char).Value = "C";
        cmdSelCadastro.Parameters.Add("@COD_ID_PESSOA_LICENCIADO", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["LICENCIADO"].ToString());
        cmdSelCadastro.Parameters.Add("@COD_ID_PESSOAS_FJ", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["PESSOA"].ToString());
        SqlDataReader ReaderCadastro = cmdSelCadastro.ExecuteReader();
        while (ReaderCadastro.Read())
        {
            ckbMulta.Checked = (ReaderCadastro["FLG_MULTA"].ToString().Trim()=="S")? true : false;
            ddlMulta.SelectedValue = ReaderCadastro["FLG_TIPO_MULTA"].ToString().Trim();
            txtMulta.Text = ReaderCadastro["NUM_MULTA"].ToString().Trim();

            ckbJuros.Checked = (ReaderCadastro["FLG_JUROS"].ToString().Trim() == "S") ? true : false;
            ddlJuros.SelectedValue = ReaderCadastro["FLG_TIPO_JUROS"].ToString().Trim();
            txtJuros.Text = ReaderCadastro["NUM_JUROS"].ToString().Trim();


            ckbDesconto.Checked = (ReaderCadastro["FLG_DESCONTO"].ToString().Trim() == "S") ? true : false;
            ddlDesconto.SelectedValue = ReaderCadastro["FLG_TIPO_DESCONTO"].ToString().Trim();
            txtDesconto.Text = ReaderCadastro["NUM_DESCONTO"].ToString().Trim();
            txtDias.Text = ReaderCadastro["NUM_DIAS"].ToString().Trim();
        }

    }
}