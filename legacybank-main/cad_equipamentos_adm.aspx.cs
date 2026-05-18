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
using System.Data.SqlClient;


using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

using System.IO;
using System.Text;

using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;

using System.ComponentModel;

using System.Security.Cryptography;
using System.Net.Mail;
using System.IO.IsolatedStorage;


public partial class cad_equipamentos_adm : System.Web.UI.Page
{
    public string sid_id
    {
        get
        {
            try { return Request["id"]; }
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
            ConsultaLocal();
            
            if (Funcoes.strToInt(sid_id) != 0)
            {
                ConsultaFicha();
            }
        }

    }

    private void ConsultaTabelas()
    {
        ddlModelo.DataTextField = "NOM_MODELO";
        ddlModelo.DataValueField = "COD_ID";
        ddlModelo.DataSource = tabelas.tModelos().Tables["EQUIPAMENTOS_MODELOS"].DefaultView;
        ddlModelo.DataBind();
        ddlModelo.Items.Insert(0, new ListItem("", "0"));

        ddlSerial.DataTextField = "NUM_SERIAL";
        ddlSerial.DataValueField = "COD_ID";
        ddlSerial.DataSource = tabelas.tSeriais().Tables["EQUIPAMENTOS_SERIAIS"].DefaultView;
        ddlSerial.DataBind();
        ddlSerial.Items.Insert(0, new ListItem("", "0"));

        ddlAdquirente.DataTextField = "NOM_ADQUIRENTE";
        ddlAdquirente.DataValueField = "COD_ID";
        ddlAdquirente.DataSource = tabelas.tAdquirente().Tables["ADQUIRENTE"].DefaultView;
        ddlAdquirente.DataBind();
        ddlAdquirente.Items.Insert(0, new ListItem("", "0"));
    }

    private void ConsultaLocal()
    {
        ddlLocal.DataTextField = "NOM_RAZAOSOCIAL";
        ddlLocal.DataValueField = "COD_ID";
        ddlLocal.DataSource = tabelas.tPessoas(ddlPropriedade.SelectedValue.ToString()).Tables["PESSOAS_FJ"].DefaultView;
        ddlLocal.DataBind();
        ddlLocal.Items.Insert(0, new ListItem("", "0"));
    }


    private void ConsultaFicha()
    {
        SqlConnection mySelCadastro = new SqlConnection(Funcoes.conexao());
        mySelCadastro.Open();
        SqlCommand cmdSelCadastro = new SqlCommand("dbo.stp_equipamentos_ins", mySelCadastro);
        cmdSelCadastro.CommandType = CommandType.StoredProcedure;
        cmdSelCadastro.Parameters.Add("@flg_operacao", SqlDbType.Char).Value = "S";
        cmdSelCadastro.Parameters.Add("@COD_ID", SqlDbType.Int).Value = Funcoes.strToInt(sid_id.ToString());
        cmdSelCadastro.Parameters.Add("@COD_ID_PESSOA_LICENCIADO", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["LICENCIADO"].ToString());
        SqlDataReader ReaderCadastro = cmdSelCadastro.ExecuteReader();
        while (ReaderCadastro.Read())
        {
            lblID.Text = ReaderCadastro["COD_ID"].ToString();
            ddlModelo.SelectedValue = ReaderCadastro["COD_ID_EQUIPAMENTOS_MODELO"].ToString();
            ddlSerial.SelectedValue = ReaderCadastro["COD_ID_EQUIPAMENTOS_SERIAIS"].ToString();
            txtChip01.Text = ReaderCadastro["NUM_CHIP_01"].ToString();
            txtChip02.Text = ReaderCadastro["NUM_CHIP_02"].ToString();

            ddlPropriedade.SelectedValue = ReaderCadastro["FLG_PROPRIEDADE"].ToString();
            ConsultaLocal();
            ddlLocal.SelectedValue = (ReaderCadastro["FLG_PROPRIEDADE"].ToString().Trim() == "M") ? ReaderCadastro["COD_ID_MARKETPLACE"].ToString() : ReaderCadastro["COD_ID_REPRESENTANTE"].ToString();
            
            ddlAtivo.SelectedValue = ReaderCadastro["FLG_ATIVO"].ToString();
            ddlStatus.SelectedValue = ReaderCadastro["FLG_STATUS"].ToString();
            

            ddlAdquirente.SelectedValue = ReaderCadastro["COD_ID_ADQUIRENTE"].ToString();
        }

    }


    protected void btnSalvar_Click(object sender, EventArgs e)
    {


        SqlConnection connInsCons = new SqlConnection(Funcoes.conexao());
        connInsCons.Open();
        SqlCommand cmdInsCons = new SqlCommand("dbo.stp_equipamentos_ins", connInsCons);
        cmdInsCons.CommandType = CommandType.StoredProcedure;
        if (Funcoes.strToInt(sid_id) != 0)
        {
            cmdInsCons.Parameters.Add("@flg_operacao", SqlDbType.Char).Value = 'A';
            cmdInsCons.Parameters.Add("@COD_ID", SqlDbType.Int).Value = Funcoes.strToInt(sid_id.ToString());
        }
        else
        {
            cmdInsCons.Parameters.Add("@flg_operacao", SqlDbType.Char).Value = 'I';
        }
        cmdInsCons.Parameters.Add("@COD_ID_PESSOA_LICENCIADO", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["LICENCIADO"].ToString());
        
        if (HttpContext.Current.Session["TIPO"].ToString() == "M")
        {
            cmdInsCons.Parameters.Add("@COD_ID_MARKETPLACE", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["PESSOA"].ToString());
        }
        
        if (HttpContext.Current.Session["TIPO"].ToString() == "R")
        {
            cmdInsCons.Parameters.Add("@COD_ID_REPRESENTANTE", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["PESSOA"].ToString());
        }


        //cmdInsCons.Parameters.Add("@COD_ID_PESSOAS_FJ", SqlDbType.Int).Value = Funcoes.strToInt(ddlLocal.SelectedValue.ToString());
        cmdInsCons.Parameters.Add("@COD_ID_EQUIPAMENTOS_MODELO", SqlDbType.Int).Value = Funcoes.strToInt(ddlModelo.SelectedValue.ToString());
        cmdInsCons.Parameters.Add("@COD_ID_EQUIPAMENTOS_SERIAIS", SqlDbType.Int).Value = Funcoes.strToInt(ddlSerial.SelectedValue.ToString());
        cmdInsCons.Parameters.Add("@FLG_ATIVO", SqlDbType.Char).Value = ddlAtivo.SelectedValue.ToString();
        cmdInsCons.Parameters.Add("@FLG_STATUS", SqlDbType.Char).Value = ddlStatus.SelectedValue.ToString();
        cmdInsCons.Parameters.Add("@FLG_PROPRIEDADE", SqlDbType.Char).Value = ddlPropriedade.SelectedValue.ToString();
        cmdInsCons.Parameters.Add("@NUM_CHIP_01", SqlDbType.VarChar).Value = txtChip01.Text.ToString();
        cmdInsCons.Parameters.Add("@NUM_CHIP_02", SqlDbType.VarChar).Value = txtChip02.Text.ToString();
        cmdInsCons.Parameters.Add("@COD_ID_ADQUIRENTE", SqlDbType.Int).Value = Funcoes.strToInt(ddlAdquirente.SelectedValue.ToString());

        cmdInsCons.ExecuteNonQuery();
        connInsCons.Close();
        connInsCons.Dispose();

        ClientScript.RegisterStartupScript(this.GetType(),
            "Alerta", "alert('Dados gravados com sucesso'); opener.PostBackOnMainPage(); window.close();", true);
    }
    protected void btnCancelar_Click(object sender, EventArgs e)
    {
        ClientScript.RegisterStartupScript(this.GetType(),
            "Alerta", "opener.PostBackOnMainPage(); window.close(); ", true);
    }

    protected void ddlPropriedade_SelectedIndexChanged(object sender, EventArgs e)
    {
        ConsultaLocal();
    }
}