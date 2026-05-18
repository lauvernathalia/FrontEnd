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


public partial class cad_suporte : System.Web.UI.Page
{
    public string sid_id
    {
        get
        {
            try { return Funcoes.Decrypt(Request["id"].ToString()); }
            catch { return ""; }
        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        // Verifica se o usuário esta autenticado

        // Verifica se o usuário esta autenticado
        if (HttpContext.Current.Session.Count <= 0)
        {
            Response.Redirect("login.aspx");
            FormsAuthentication.SignOut();
        }

        if (!IsPostBack)
        {
            // Carregar Motivos de Suporte
            SqlConnection myMotivo = new SqlConnection(Funcoes.conexao());
            myMotivo.Open();
            SqlCommand cmdMotivo = new SqlCommand("dbo.stp_suporte_motivo_ins", myMotivo);
            cmdMotivo.CommandType = CommandType.StoredProcedure;
            cmdMotivo.Parameters.Add("@flg_operacao", SqlDbType.Char).Value = "C";
            cmdMotivo.Parameters.Add("@FLG_ATIVO", SqlDbType.Char).Value = "S";
            SqlDataAdapter drMotivo = new SqlDataAdapter();
            drMotivo.SelectCommand = cmdMotivo;
            DataSet dsMotivo = new DataSet();
            drMotivo.Fill(dsMotivo, "SUPORTE_MOTIVO");
            ddlMotivo.DataTextField = "NOM_MOTIVO";
            ddlMotivo.DataValueField = "COD_ID";
            ddlMotivo.DataSource = dsMotivo.Tables["SUPORTE_MOTIVO"].DefaultView;
            ddlMotivo.DataBind();
            ddlMotivo.Items.Insert(0, new ListItem("Selecione o motivo do suporte", "0"));

            if (HttpContext.Current.Session["TIPO"].ToString() == "E")
            {

                // Tabela de Estabelecimento
                SqlConnection myEstabelecimentos = new SqlConnection(Funcoes.conexao());
                myEstabelecimentos.Open();
                SqlCommand cmdEstabelecimentos = new SqlCommand("dbo.stp_pessoas_fj_ins", myEstabelecimentos);
                cmdEstabelecimentos.CommandType = CommandType.StoredProcedure;
                cmdEstabelecimentos.Parameters.Add("@flg_operacao", SqlDbType.VarChar).Value = "Y";

                //cmdEstabelecimentos.Parameters.Add("@COD_ID_PESSOA_LICENCIADO", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["LICENCIADO"].ToString());

                cmdEstabelecimentos.Parameters.Add("@NOM_RAZAOSOCIAL", SqlDbType.VarChar).Value = " ";
                cmdEstabelecimentos.Parameters.Add("@NUM_DOCUMENTO", SqlDbType.VarChar).Value = " ";
                cmdEstabelecimentos.Parameters.Add("@NUM_TOKEN", SqlDbType.VarChar).Value = " ";
                cmdEstabelecimentos.Parameters.Add("@FLG_ATIVO", SqlDbType.Char).Value = " ";
                cmdEstabelecimentos.Parameters.Add("@FLG_TIPO", SqlDbType.Char).Value = "E";

                if (HttpContext.Current.Session["TIPO"].ToString() == "E")
                {
                    cmdEstabelecimentos.Parameters.Add("@COD_ID", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["PESSOA"].ToString());
                    txtNome.Enabled = false;
                    btnPesquisar.Enabled = false;
                }
                if (HttpContext.Current.Session["TIPO"].ToString() == "A")
                {
                    cmdEstabelecimentos.Parameters.Add("@COD_ID_PESSOA_LICENCIADO", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["LICENCIADO"].ToString());
                }
                if (HttpContext.Current.Session["TIPO"].ToString() == "M")
                {
                    cmdEstabelecimentos.Parameters.Add("@COD_ID_MARKETPLACE", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["PESSOA"].ToString());
                }
                if (HttpContext.Current.Session["TIPO"].ToString() == "R")
                {
                    cmdEstabelecimentos.Parameters.Add("@COD_ID_REPRESENTANTE", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["PESSOA"].ToString());
                }

                SqlDataAdapter drEstabelecimentos = new SqlDataAdapter();
                drEstabelecimentos.SelectCommand = cmdEstabelecimentos;
                DataSet dsEstabelecimentos = new DataSet();
                drEstabelecimentos.Fill(dsEstabelecimentos, "PESSOAS_FJ");
                ddlSolicitante.DataTextField = "NOM_RAZAOSOCIAL";
                ddlSolicitante.DataValueField = "COD_ID";
                ddlSolicitante.DataSource = dsEstabelecimentos.Tables["PESSOAS_FJ"].DefaultView;
                ddlSolicitante.DataBind();
                ddlSolicitante.Items.Insert(0, new ListItem("Todos", ""));
            }

            if (Funcoes.strToInt(sid_id) != 0)
            {
                ConsultaCadastro();
                txtSuporte.Enabled = false;
                btnSalvar.Visible = false;
            }
            else
            {
                txtData.Text = DateTime.Now.ToShortDateString();

                if (HttpContext.Current.Session["TIPO"].ToString().Trim() == "E")
                {
                    ddlSolicitante.SelectedValue = HttpContext.Current.Session["PESSOA"].ToString();
                    ddlSolicitante.Enabled = false;
                }

                txtSuporte.Enabled = true;
                btnSalvar.Visible = true;

            }
        }

    }

    private void ConsultaCadastro()
    {
        SqlConnection mySelCadastro = new SqlConnection(Funcoes.conexao());
        mySelCadastro.Open();
        SqlCommand cmdSelCadastro = new SqlCommand("dbo.stp_suporte_ins", mySelCadastro);
        cmdSelCadastro.CommandType = CommandType.StoredProcedure;
        cmdSelCadastro.Parameters.Add("@flg_operacao", SqlDbType.Char).Value = "S";
        cmdSelCadastro.Parameters.Add("@COD_ID", SqlDbType.Int).Value = Funcoes.strToInt(sid_id.ToString());
        cmdSelCadastro.Parameters.Add("@COD_ID_PESSOA_LICENCIADO", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["LICENCIADO"].ToString());

        SqlDataReader ReaderCadastro = cmdSelCadastro.ExecuteReader();
        while (ReaderCadastro.Read())
        {
            txtCodigo.Text = ReaderCadastro["COD_ID"].ToString();
            ddlSolicitante.SelectedValue = ReaderCadastro["COD_ID_PESSOAS_FJ"].ToString();
            txtData.Text = ReaderCadastro["DTA_DATA_ABERTURA"].ToString();
            txtProtocolo.Text = ReaderCadastro["NOM_PROTOCOLO"].ToString();
            txtSuporte.Text = ReaderCadastro["NOM_SUPORTE"].ToString();
            txtDescricao.InnerText = ReaderCadastro["DES_SUPORTE"].ToString();
            ddlStatus.SelectedValue = ReaderCadastro["FLG_STATUS"].ToString();
            ddlMotivo.SelectedValue = ReaderCadastro["COD_ID_SUPORTE_MOTIVO"].ToString();
        }

    }

    protected void btnSalvar_Click(object sender, EventArgs e)
    {
        if (Funcoes.strToInt(ddlSolicitante.SelectedValue.ToString().Trim()) > 0)
        {


            SqlConnection connInsCons = new SqlConnection(Funcoes.conexao());
            connInsCons.Open();
            SqlCommand cmdInsCons = new SqlCommand("dbo.stp_suporte_ins", connInsCons);
            cmdInsCons.CommandType = CommandType.StoredProcedure;
            string mensagem = "";
            if (Funcoes.strToInt(sid_id) != 0)
            {
                cmdInsCons.Parameters.Add("@flg_operacao", SqlDbType.Char).Value = 'A';
                cmdInsCons.Parameters.Add("@COD_ID", SqlDbType.Int).Value = Funcoes.strToInt(sid_id.ToString());
                mensagem = "Dados gravados com sucesso!";
            }
            else
            {
                cmdInsCons.Parameters.Add("@flg_operacao", SqlDbType.Char).Value = 'I';

                cmdInsCons.Parameters.Add("@COD_ID_PESSOAS_FJ", SqlDbType.Int).Value = Funcoes.strToInt(ddlSolicitante.SelectedValue.ToString());

                int numeroDoCliente = Funcoes.strToInt(HttpContext.Current.Session["PESSOA"].ToString());
                string protocolo = ProtocoloGenerator.GerarProtocolo(numeroDoCliente, "SUP");

                cmdInsCons.Parameters.Add("@NOM_PROTOCOLO", SqlDbType.VarChar).Value = protocolo;
                mensagem = "Dados gravados com sucesso!  O número do seu protocolo é: " + protocolo;
            }

            cmdInsCons.Parameters.Add("@COD_ID_PESSOA_LICENCIADO", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["LICENCIADO"].ToString());
            cmdInsCons.Parameters.Add("@NOM_SUPORTE", SqlDbType.VarChar).Value = txtSuporte.Text.ToString();
            cmdInsCons.Parameters.Add("@DES_SUPORTE", SqlDbType.Text).Value = txtDescricao.InnerText.ToString();
            cmdInsCons.Parameters.Add("@COD_ID_SUPORTE_MOTIVO", SqlDbType.Int).Value = Funcoes.strToInt(ddlMotivo.SelectedValue.ToString());

            cmdInsCons.ExecuteNonQuery();
            connInsCons.Close();
            connInsCons.Dispose();

            Funcoes.GravaAuditoria(Funcoes.strToInt(sid_id.ToString()), Request.ServerVariables["REMOTE_ADDR"].ToString(), "Cadastro - Suporte - Inclusão", "ID: " + Funcoes.strToInt(sid_id.ToString()));


            ClientScript.RegisterStartupScript(this.GetType(),
                "Alerta", "alert('" + mensagem + "'); opener.PostBackOnMainPage(); window.close(); ", true);

        }
        else
        {
            ClientScript.RegisterStartupScript(this.GetType(),
                "Solicitante", "alert('É obrigatorio especificar o solicitante do suporte! Verifique e reentre.'); ", true);
        }
    }
    protected void btnCancelar_Click(object sender, EventArgs e)
    {
        ClientScript.RegisterStartupScript(this.GetType(),
            "Alerta", "opener.PostBackOnMainPage(); window.close(); ", true);
    }

    protected void btnPesquisar_Click(object sender, EventArgs e)
    {
        if (txtNome.Text.ToString().Trim().Length >= 3)
        {
            // Tabela de Estabelecimento
            SqlConnection myEstabelecimentos = new SqlConnection(Funcoes.conexao());
            myEstabelecimentos.Open();
            SqlCommand cmdEstabelecimentos = new SqlCommand("dbo.stp_pessoas_fj_ins", myEstabelecimentos);
            cmdEstabelecimentos.CommandType = CommandType.StoredProcedure;
            cmdEstabelecimentos.Parameters.Add("@flg_operacao", SqlDbType.VarChar).Value = "Y";

            //cmdEstabelecimentos.Parameters.Add("@COD_ID_PESSOA_LICENCIADO", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["LICENCIADO"].ToString());

            cmdEstabelecimentos.Parameters.Add("@NOM_RAZAOSOCIAL", SqlDbType.VarChar).Value = txtNome.Text.ToString();
            cmdEstabelecimentos.Parameters.Add("@NUM_DOCUMENTO", SqlDbType.VarChar).Value = " ";
            cmdEstabelecimentos.Parameters.Add("@NUM_TOKEN", SqlDbType.VarChar).Value = " ";
            cmdEstabelecimentos.Parameters.Add("@FLG_ATIVO", SqlDbType.Char).Value = " ";
            cmdEstabelecimentos.Parameters.Add("@FLG_TIPO", SqlDbType.Char).Value = "E";

            if (HttpContext.Current.Session["TIPO"].ToString() == "E")
            {
                cmdEstabelecimentos.Parameters.Add("@COD_ID", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["PESSOA"].ToString());
                txtNome.Enabled = false;
                btnPesquisar.Enabled = false;
            }
            if (HttpContext.Current.Session["TIPO"].ToString() == "A")
            {
                cmdEstabelecimentos.Parameters.Add("@COD_ID_PESSOA_LICENCIADO", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["LICENCIADO"].ToString());
            }
            if (HttpContext.Current.Session["TIPO"].ToString() == "M")
            {
                cmdEstabelecimentos.Parameters.Add("@COD_ID_MARKETPLACE", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["PESSOA"].ToString());
            }
            if (HttpContext.Current.Session["TIPO"].ToString() == "R")
            {
                cmdEstabelecimentos.Parameters.Add("@COD_ID_REPRESENTANTE", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["PESSOA"].ToString());
            }

            SqlDataAdapter drEstabelecimentos = new SqlDataAdapter();
            drEstabelecimentos.SelectCommand = cmdEstabelecimentos;
            DataSet dsEstabelecimentos = new DataSet();
            drEstabelecimentos.Fill(dsEstabelecimentos, "PESSOAS_FJ");
            ddlSolicitante.DataTextField = "NOM_RAZAOSOCIAL";
            ddlSolicitante.DataValueField = "COD_ID";
            ddlSolicitante.DataSource = dsEstabelecimentos.Tables["PESSOAS_FJ"].DefaultView;
            ddlSolicitante.DataBind();
            ddlSolicitante.Items.Insert(0, new ListItem("Todos", ""));
        }
        else
        {
            ClientScript.RegisterStartupScript(this.GetType(),
                "Solicitante", "alert('Para realizar a pesquisa por nome é necessário informar pelo menos 3 caracteres (Ex.: PED, MAR, XXX...)! Verifique e reentre.'); ", true);

        }
    }
}