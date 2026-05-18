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

public partial class con_eventos_relatorio : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            //Vamos considerar que a data seja o dia de hoje, mas pode ser qualquer data.
            DateTime data = DateTime.Today;
            //DateTime com o primeiro dia do mês
            DateTime primeiroDiaDoMes = new DateTime(data.Year, data.Month, 1);
            //DateTime com o último dia do mês
            DateTime ultimoDiaDoMes = new DateTime(data.Year, data.Month, DateTime.DaysInMonth(data.Year, data.Month));
            txtDataIni.Text = primeiroDiaDoMes.ToShortDateString();
            txtDataFim.Text = ultimoDiaDoMes.ToShortDateString();

            txtDataIni.Enabled = false;
            txtDataFim.Enabled = false;


            // Carregar Eventos
            CarregaEventos();
            CarregaSerial();
            CarregaFicha();

            ddlTipo_SelectedIndexChanged(null, null);
        }
    }
    protected void btnPesquisar_Click(object sender, EventArgs e)
    {
        ConsultaGeral();
    }

    private void CarregaFicha()
    {
        SqlConnection mySelCadastro = new SqlConnection(Funcoes.conexao());
        mySelCadastro.Open();
        SqlCommand cmdSelCadastro = new SqlCommand("dbo.stp_eventos_ins", mySelCadastro);
        cmdSelCadastro.CommandType = CommandType.StoredProcedure;
        cmdSelCadastro.Parameters.Add("@flg_operacao", SqlDbType.Char).Value = "S";
        cmdSelCadastro.Parameters.Add("@COD_ID", SqlDbType.Int).Value = Funcoes.strToInt(ddlEvento.SelectedValue.ToString());
        cmdSelCadastro.Parameters.Add("@COD_ID_PESSOA_LICENCIADO", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["LICENCIADO"].ToString());
        SqlDataReader ReaderCadastro = cmdSelCadastro.ExecuteReader();
        while (ReaderCadastro.Read())
        {
            txtDataIni.Text = ReaderCadastro["DTA_INICIO"].ToString();
            txtDataFim.Text = ReaderCadastro["DTA_TERMINO"].ToString();
            txtDataIni.Enabled = false;
            txtDataFim.Enabled = false;
        }

    }
    
    private void CarregaSerial()
    {
        SqlConnection mySerial = new SqlConnection(Funcoes.conexao());
        mySerial.Open();
        SqlCommand cmdSerial = new SqlCommand("dbo.stp_eventos_ins", mySerial);
        cmdSerial.CommandType = CommandType.StoredProcedure;
        cmdSerial.Parameters.Add("@flg_operacao", SqlDbType.VarChar).Value = "R";
        cmdSerial.Parameters.Add("@COD_ID_PESSOA_LICENCIADO", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["LICENCIADO"].ToString());
        cmdSerial.Parameters.Add("@COD_ID", SqlDbType.Int).Value = Funcoes.strToInt(ddlEvento.SelectedValue.ToString());
        SqlDataAdapter drSerial = new SqlDataAdapter();
        drSerial.SelectCommand = cmdSerial;
        DataSet dsSerial = new DataSet();
        drSerial.Fill(dsSerial, "EVENTOS");
        ddlSerial.DataTextField = "NUM_SERIAL";
        ddlSerial.DataValueField = "NUM_SERIAL";
        ddlSerial.DataSource = dsSerial.Tables["EVENTOS"].DefaultView;
        ddlSerial.DataBind();
        ddlSerial.Items.Insert(0, new ListItem("Todos", ""));
    }

    private void CarregaEventos()
    {
        SqlConnection myEventos = new SqlConnection(Funcoes.conexao());
        myEventos.Open();
        SqlCommand cmdEventos = new SqlCommand("dbo.stp_eventos_ins", myEventos);
        cmdEventos.CommandType = CommandType.StoredProcedure;
        cmdEventos.Parameters.Add("@flg_operacao", SqlDbType.VarChar).Value = "L";
        cmdEventos.Parameters.Add("@COD_ID_PESSOA_LICENCIADO", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["LICENCIADO"].ToString());
        cmdEventos.Parameters.Add("@FLG_ATIVO", SqlDbType.Char).Value = "S";
        if (HttpContext.Current.Session["TIPO"].ToString() == "M")
        {
            cmdEventos.Parameters.Add("@COD_ID_PESSOAS_FJ", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["PESSOA"].ToString());
        }
        SqlDataAdapter drEventos = new SqlDataAdapter();
        drEventos.SelectCommand = cmdEventos;
        DataSet dsEventos = new DataSet();
        drEventos.Fill(dsEventos, "EVENTOS");
        ddlEvento.DataTextField = "NOM_EVENTO";
        ddlEvento.DataValueField = "COD_ID";
        ddlEvento.DataSource = dsEventos.Tables["EVENTOS"].DefaultView;
        ddlEvento.DataBind();
        //ddlEvento.Items.Insert(0, new ListItem("Todos", ""));

    }
    private void ConsultaGeral()
    {
        SqlConnection myConsulta = new SqlConnection(Funcoes.conexao());
        myConsulta.Open();
        SqlDataAdapter SDAConsulta = new SqlDataAdapter("dbo.stp_eventos_transacoes_ins", myConsulta);
        SDAConsulta.SelectCommand.CommandType = CommandType.StoredProcedure;
        SDAConsulta.SelectCommand.Parameters.Add("@flg_operacao", SqlDbType.Char).Value = "L";
        SDAConsulta.SelectCommand.Parameters.Add("@COD_ID_PESSOA_LICENCIADO", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["LICENCIADO"].ToString());
        SDAConsulta.SelectCommand.Parameters.Add("@COD_ID", SqlDbType.Int).Value = Funcoes.strToInt(ddlEvento.SelectedValue.ToString());
        SDAConsulta.SelectCommand.Parameters.Add("@NUM_SERIAL", SqlDbType.VarChar).Value = ddlSerial.SelectedValue.ToString();
        SDAConsulta.SelectCommand.Parameters.Add("@DTA_INICIO", SqlDbType.DateTime).Value = Convert.ToDateTime(txtDataIni.Text.ToString());
        SDAConsulta.SelectCommand.Parameters.Add("@DTA_TERMINO", SqlDbType.DateTime).Value = Convert.ToDateTime(txtDataFim.Text.ToString());
        DataSet dsConsulta = new DataSet();
        SDAConsulta.Fill(dsConsulta, "EVENTOS");
        rptConsulta.DataSource = dsConsulta.Tables["EVENTOS"].DefaultView;
        rptConsulta.DataBind();
        myConsulta.Close(); myConsulta.Dispose();
    }

    private void ConsultaGeralSintetico()
    {
        SqlConnection myConsulta = new SqlConnection(Funcoes.conexao());
        myConsulta.Open();
        SqlDataAdapter SDAConsulta = new SqlDataAdapter("dbo.stp_eventos_transacoes_ins", myConsulta);
        SDAConsulta.SelectCommand.CommandType = CommandType.StoredProcedure;
        SDAConsulta.SelectCommand.Parameters.Add("@flg_operacao", SqlDbType.Char).Value = "T";
        SDAConsulta.SelectCommand.Parameters.Add("@COD_ID_PESSOA_LICENCIADO", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["LICENCIADO"].ToString());
        SDAConsulta.SelectCommand.Parameters.Add("@COD_ID", SqlDbType.Int).Value = Funcoes.strToInt(ddlEvento.SelectedValue.ToString());
        SDAConsulta.SelectCommand.Parameters.Add("@NUM_SERIAL", SqlDbType.VarChar).Value = ddlSerial.SelectedValue.ToString();
        SDAConsulta.SelectCommand.Parameters.Add("@DTA_INICIO", SqlDbType.DateTime).Value = Convert.ToDateTime(txtDataIni.Text.ToString());
        SDAConsulta.SelectCommand.Parameters.Add("@DTA_TERMINO", SqlDbType.DateTime).Value = Convert.ToDateTime(txtDataFim.Text.ToString());
        DataSet dsConsulta = new DataSet();
        SDAConsulta.Fill(dsConsulta, "#resultadosintetico");
        rptConsultaSintetico.DataSource = dsConsulta.Tables["#resultadosintetico"].DefaultView;
        rptConsultaSintetico.DataBind();
        myConsulta.Close(); myConsulta.Dispose();
    }


    private void ConsultaGeralFechamento()
    {
        SqlConnection myConsulta = new SqlConnection(Funcoes.conexao());
        myConsulta.Open();
        SqlDataAdapter SDAConsulta = new SqlDataAdapter("dbo.stp_eventos_transacoes_ins", myConsulta);
        SDAConsulta.SelectCommand.CommandType = CommandType.StoredProcedure;
        SDAConsulta.SelectCommand.Parameters.Add("@flg_operacao", SqlDbType.Char).Value = "F";
        SDAConsulta.SelectCommand.Parameters.Add("@COD_ID_PESSOA_LICENCIADO", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["LICENCIADO"].ToString());
        SDAConsulta.SelectCommand.Parameters.Add("@COD_ID", SqlDbType.Int).Value = Funcoes.strToInt(ddlEvento.SelectedValue.ToString());
        SDAConsulta.SelectCommand.Parameters.Add("@NUM_SERIAL", SqlDbType.VarChar).Value = ddlSerial.SelectedValue.ToString();
        SDAConsulta.SelectCommand.Parameters.Add("@DTA_INICIO", SqlDbType.DateTime).Value = Convert.ToDateTime(txtDataIni.Text.ToString());
        SDAConsulta.SelectCommand.Parameters.Add("@DTA_TERMINO", SqlDbType.DateTime).Value = Convert.ToDateTime(txtDataFim.Text.ToString());
        DataSet dsConsulta = new DataSet();
        SDAConsulta.Fill(dsConsulta, "#resultadosintetico");
        rptConsultaFechamento.DataSource = dsConsulta.Tables["#resultadosintetico"].DefaultView;
        rptConsultaFechamento.DataBind();
        myConsulta.Close(); myConsulta.Dispose();
    }


    public string GetPostBackScript()
    {
        PostBackOptions options = new PostBackOptions(btnPostback);
        Page.ClientScript.RegisterForEventValidation(options);

        return Page.ClientScript.GetPostBackEventReference(options);
    }

    protected void btnPostBack_Click(object sender, EventArgs e)
    {
        ddlTipo_SelectedIndexChanged(null, null);
    }


    protected void rptConsulta_OnItemCommand(object source, System.Web.UI.WebControls.RepeaterCommandEventArgs e)
    {

    }
    protected void ddlEvento_SelectedIndexChanged(object sender, System.EventArgs e)
    {
        CarregaSerial();
        CarregaFicha();
        ddlTipo_SelectedIndexChanged(null, null);
        
    }
    protected void ddlTipo_SelectedIndexChanged(object sender, System.EventArgs e)
    {
        divRelDetalhe.Visible = false;
        divRelSintetico.Visible = false;
        divRelFechamento.Visible = false;

        if (ddlTipo.SelectedValue.ToString() == "A")
        {
            divRelDetalhe.Visible = true;
            ConsultaGeral();
        }
        if (ddlTipo.SelectedValue.ToString() == "S")
        {
            divRelSintetico.Visible = true;
            ConsultaGeralSintetico();
        }
        if (ddlTipo.SelectedValue.ToString() == "F")
        {
            divRelFechamento.Visible = true;
            ConsultaGeralFechamento();
        }
    }
}