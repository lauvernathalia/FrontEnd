using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Runtime.Serialization.Json;
using System.IO;
using System.Text;


using System.Data;
using System.Configuration;
using System.Collections;
using System.Web.Security;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Data.SqlClient;
using System.Security.Cryptography;


public partial class con_extrato_vendas : System.Web.UI.Page
{
    protected void Page_LoadComplete(object sender, EventArgs e)
    {

    }


    protected void Page_Error(object sender, EventArgs e)
    {

    }

    protected void Page_Init(object sender, EventArgs e)
    {

    }

    protected void Page_InitComplete(object sender, EventArgs e)
    {

    }

    protected void Page_SaveStateComplete(object sender, EventArgs e)
    {

    }

    protected void Page_Unload(object sender, EventArgs e)
    {

    }    

    protected void Page_Load(object sender, EventArgs e)
    {

        if (HttpContext.Current.Session.Count <= 0)
        {
            Response.Redirect("login.aspx");
            FormsAuthentication.SignOut();
        }
        else
        {

            if (!IsPostBack)
            {
                DateTime data = DateTime.Today;
                DateTime primeiroDiaDoMes = new DateTime(data.Year, data.Month, 1);
                DateTime ultimoDiaDoMes = new DateTime(data.Year, data.Month, DateTime.DaysInMonth(data.Year, data.Month));

                txtDataIni.Text = primeiroDiaDoMes.ToShortDateString();
                txtDataFim.Text = DateTime.Now.ToShortDateString();

                SqlConnection myEstabelecimentos = new SqlConnection(Funcoes.conexao());
                myEstabelecimentos.Open();
                SqlCommand cmdEstabelecimentos = new SqlCommand("dbo.stp_pessoas_fj_ins", myEstabelecimentos);
                cmdEstabelecimentos.CommandType = CommandType.StoredProcedure;
                cmdEstabelecimentos.Parameters.Add("@flg_operacao", SqlDbType.VarChar).Value = "Y";

                cmdEstabelecimentos.Parameters.Add("@COD_ID_PESSOA_LICENCIADO", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["LICENCIADO"].ToString());
                cmdEstabelecimentos.Parameters.Add("@NOM_RAZAOSOCIAL", SqlDbType.VarChar).Value = " ";
                cmdEstabelecimentos.Parameters.Add("@NUM_DOCUMENTO", SqlDbType.VarChar).Value = " ";
                cmdEstabelecimentos.Parameters.Add("@NUM_TOKEN", SqlDbType.VarChar).Value = " ";
                cmdEstabelecimentos.Parameters.Add("@FLG_ATIVO", SqlDbType.Char).Value = " ";
                cmdEstabelecimentos.Parameters.Add("@FLG_TIPO", SqlDbType.Char).Value = "E";

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
                ddlEstabelecimento.DataTextField = "NOM_RAZAOSOCIAL";
                ddlEstabelecimento.DataValueField = "COD_ID";
                ddlEstabelecimento.DataSource = dsEstabelecimentos.Tables["PESSOAS_FJ"].DefaultView;
                ddlEstabelecimento.DataBind();
                ddlEstabelecimento.Items.Insert(0, new ListItem("Todos", ""));
                myEstabelecimentos.Close();
                myEstabelecimentos.Dispose();

                if (HttpContext.Current.Session["TIPO"].ToString() == "E")
                {
                    ddlEstabelecimento.SelectedValue = HttpContext.Current.Session["PESSOA"].ToString();
                    ddlEstabelecimento.Enabled = false;
                }



                ConsultaGeral();

            }
        }
    }
    protected void btnPesquisar_Click(object sender, EventArgs e)
    {
        ConsultaGeral();
    }

    private void ConsultaGeral()
    {
        SqlConnection myConsulta = new SqlConnection(Funcoes.conexao());
        myConsulta.Open();
        SqlDataAdapter SDAConsulta = new SqlDataAdapter("dbo.stp_extrato_ins", myConsulta);
        SDAConsulta.SelectCommand.CommandType = CommandType.StoredProcedure;
        SDAConsulta.SelectCommand.Parameters.Add("@flg_operacao", SqlDbType.Char).Value = "C";
        SDAConsulta.SelectCommand.Parameters.Add("@COD_ID_PESSOA_LICENCIADO", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["LICENCIADO"].ToString());
        SDAConsulta.SelectCommand.Parameters.Add("@COD_ID_PESSOAS_FJ", SqlDbType.Int).Value = Funcoes.strToInt(ddlEstabelecimento.SelectedValue.ToString());
        SDAConsulta.SelectCommand.Parameters.Add("@DTA_INICIO", SqlDbType.DateTime).Value = Convert.ToDateTime(txtDataIni.Text.ToString());
        SDAConsulta.SelectCommand.Parameters.Add("@DTA_FIM", SqlDbType.DateTime).Value = Convert.ToDateTime(txtDataFim.Text.ToString());
        SDAConsulta.SelectCommand.Parameters.Add("@FLG_STATUS", SqlDbType.VarChar).Value = "";

        DataSet dsConsulta = new DataSet();
        SDAConsulta.Fill(dsConsulta, "EXTRATO");

        rptConsulta.DataSource = dsConsulta.Tables["EXTRATO"].DefaultView;
        rptConsulta.DataBind();

        myConsulta.Close(); myConsulta.Dispose();
    }

    protected void btnExportar_Click(object sender, EventArgs e)
    {

    }
    public string GetPostBackScript()
    {
        PostBackOptions options = new PostBackOptions(btnPostback);
        Page.ClientScript.RegisterForEventValidation(options);

        return Page.ClientScript.GetPostBackEventReference(options);
    }

    protected void btnPostBack_Click(object sender, EventArgs e)
    {

    }
    protected void rptConsulta_OnItemCommand(object source, RepeaterCommandEventArgs e)
    {

    }
}