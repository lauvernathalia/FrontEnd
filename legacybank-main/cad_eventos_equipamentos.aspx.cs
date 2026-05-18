using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
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

using System.Collections.Specialized;

public partial class cad_eventos_equipamentos : System.Web.UI.Page
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
        if (HttpContext.Current.Session.Count <= 0)
        {
            Response.Redirect("login.aspx");
            FormsAuthentication.SignOut();
        }

        if (!IsPostBack)
        {
            ConsultaTabelas();

            if (Funcoes.strToInt(sid_id) != 0)
            {
                ConsultaFicha();
                ConsultaGeral();
            }
        }

    }

    private void ConsultaTabelas()
    {

        SqlConnection myEquipamento = new SqlConnection(Funcoes.conexao());
        myEquipamento.Open();
        SqlCommand cmdEquipamento = new SqlCommand("dbo.stp_equipamentos_ins", myEquipamento);
        cmdEquipamento.CommandType = CommandType.StoredProcedure;
        cmdEquipamento.Parameters.Add("@flg_operacao", SqlDbType.VarChar).Value = "C";
        cmdEquipamento.Parameters.Add("@COD_ID_PESSOA_LICENCIADO", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["LICENCIADO"].ToString());
        cmdEquipamento.Parameters.Add("@FLG_ATIVO", SqlDbType.Char).Value = "S";

        SqlDataAdapter drEquipamento = new SqlDataAdapter();
        drEquipamento.SelectCommand = cmdEquipamento;
        DataSet dsEquipamento = new DataSet();
        drEquipamento.Fill(dsEquipamento, "EQUIPAMENTOS");
        ddlEquipamento.DataTextField = "NOM_EQUIPAMENTO";
        ddlEquipamento.DataValueField = "COD_ID";
        ddlEquipamento.DataSource = dsEquipamento.Tables["EQUIPAMENTOS"].DefaultView;
        ddlEquipamento.DataBind();
        ddlEquipamento.Items.Insert(0, new ListItem("", "0"));



        SqlConnection myEstabelecimento = new SqlConnection(Funcoes.conexao());
        myEstabelecimento.Open();
        SqlCommand cmdEstabelecimento = new SqlCommand("dbo.stp_pessoas_fj_ins", myEstabelecimento);
        cmdEstabelecimento.CommandType = CommandType.StoredProcedure;
        cmdEstabelecimento.Parameters.Add("@flg_operacao", SqlDbType.VarChar).Value = "L";
        cmdEstabelecimento.Parameters.Add("@COD_ID_PESSOA_LICENCIADO", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["LICENCIADO"].ToString());
        cmdEstabelecimento.Parameters.Add("@FLG_ATIVO", SqlDbType.Char).Value = "S";
        cmdEstabelecimento.Parameters.Add("@FLG_TIPO", SqlDbType.Char).Value = "E";

        SqlDataAdapter drEstabelecimento = new SqlDataAdapter();
        drEstabelecimento.SelectCommand = cmdEstabelecimento;
        DataSet dsEstabelecimento = new DataSet();
        drEstabelecimento.Fill(dsEstabelecimento, "PESSOAS_FJ");
        ddlEstabelecimento.DataTextField = "NOM_RAZAOSOCIAL";
        ddlEstabelecimento.DataValueField = "COD_ID";
        ddlEstabelecimento.DataSource = dsEstabelecimento.Tables["PESSOAS_FJ"].DefaultView;
        ddlEstabelecimento.DataBind();
        ddlEstabelecimento.Items.Insert(0, new ListItem("", "0"));
    }

    private void ConsultaFicha()
    {
        SqlConnection mySelCadastro = new SqlConnection(Funcoes.conexao());
        mySelCadastro.Open();
        SqlCommand cmdSelCadastro = new SqlCommand("dbo.stp_eventos_ins", mySelCadastro);
        cmdSelCadastro.CommandType = CommandType.StoredProcedure;
        cmdSelCadastro.Parameters.Add("@flg_operacao", SqlDbType.Char).Value = "S";
        cmdSelCadastro.Parameters.Add("@COD_ID", SqlDbType.Int).Value = Funcoes.strToInt(sid_id.ToString());
        cmdSelCadastro.Parameters.Add("@COD_ID_PESSOA_LICENCIADO", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["LICENCIADO"].ToString());

        SqlDataReader ReaderCadastro = cmdSelCadastro.ExecuteReader();
        while (ReaderCadastro.Read())
        {
            txtIDEvento.Text = ReaderCadastro["COD_ID"].ToString();
            txtEvento.Text = ReaderCadastro["NOM_EVENTO"].ToString();
        }
    }

    private void ConsultaGeral()
    {
        SqlConnection myConsulta = new SqlConnection(Funcoes.conexao());
        myConsulta.Open();
        SqlDataAdapter SDAConsulta = new SqlDataAdapter("dbo.stp_eventos_equipamentos_ins", myConsulta);
        SDAConsulta.SelectCommand.CommandType = CommandType.StoredProcedure;
        SDAConsulta.SelectCommand.Parameters.Add("@flg_operacao", SqlDbType.Char).Value = "C";
        SDAConsulta.SelectCommand.Parameters.Add("@COD_ID_PESSOA_LICENCIADO", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["LICENCIADO"].ToString());
        SDAConsulta.SelectCommand.Parameters.Add("@COD_ID_EVENTOS", SqlDbType.Int).Value = Funcoes.strToInt(sid_id.ToString());
        DataSet dsConsulta = new DataSet();
        SDAConsulta.Fill(dsConsulta, "EVENTOS_EQUIPAMENTOS");

        rptConsultaUsuario.DataSource = dsConsulta.Tables["EVENTOS_EQUIPAMENTOS"].DefaultView;
        rptConsultaUsuario.DataBind();

        myConsulta.Close(); myConsulta.Dispose();
    }


    protected void btnIncluir_Click(object sender, EventArgs e)
    {
        SqlConnection connInsCons = new SqlConnection(Funcoes.conexao());
        connInsCons.Open();
        SqlCommand cmdInsCons = new SqlCommand("dbo.stp_eventos_equipamentos_ins", connInsCons);
        cmdInsCons.CommandType = CommandType.StoredProcedure;
        cmdInsCons.Parameters.Add("@flg_operacao", SqlDbType.Char).Value = 'I';
        cmdInsCons.Parameters.Add("@COD_ID_PESSOA_LICENCIADO", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["LICENCIADO"].ToString());
        cmdInsCons.Parameters.Add("@COD_ID_EVENTOS", SqlDbType.Int).Value = Funcoes.strToInt(sid_id.ToString());
        cmdInsCons.Parameters.Add("@COD_ID_EQUIPAMENTOS", SqlDbType.Int).Value = Funcoes.strToInt(ddlEquipamento.SelectedValue.ToString());
        cmdInsCons.Parameters.Add("@COD_ID_PESSOAS_FJ", SqlDbType.Int).Value = Funcoes.strToInt(ddlEstabelecimento.SelectedValue.ToString());

        cmdInsCons.ExecuteNonQuery();
        connInsCons.Close();
        connInsCons.Dispose();

        ClientScript.RegisterStartupScript(this.GetType(),
            "Alerta", "alert('Dados gravados com sucesso'); ", true);

        ConsultaGeral();

    }
    protected void btnCancelar_Click(object sender, EventArgs e)
    {
        ClientScript.RegisterStartupScript(this.GetType(),
    "Alerta", "opener.PostBackOnMainPage(); window.close(); ", true);

    }
    protected void rptConsultaUsuario_ItemCommand(object source, RepeaterCommandEventArgs e)
    {
        if (e.CommandName == "Excluir")
        {
            SqlConnection connInsCons = new SqlConnection(Funcoes.conexao());
            connInsCons.Open();
            SqlCommand cmdInsCons = new SqlCommand("dbo.stp_eventos_equipamentos_ins", connInsCons);
            cmdInsCons.CommandType = CommandType.StoredProcedure;
            cmdInsCons.Parameters.Add("@flg_operacao", SqlDbType.Char).Value = "E";
            cmdInsCons.Parameters.Add("@COD_ID", SqlDbType.Int).Value = Funcoes.strToInt(Convert.ToString(e.CommandArgument));
            cmdInsCons.Parameters.Add("@COD_ID_PESSOA_LICENCIADO", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["LICENCIADO"].ToString());
            cmdInsCons.ExecuteNonQuery();
            connInsCons.Close();
            connInsCons.Dispose();

            ClientScript.RegisterStartupScript(this.GetType(), "Excluir", "alert('Dados excluídos com sucesso!');", true);
            ConsultaGeral();

        }

    }
}