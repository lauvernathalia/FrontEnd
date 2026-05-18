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
using System.Threading;
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

public partial class con_estabelecimentosOLD : System.Web.UI.Page
{
    private int Licenciado { get; set; }
    private string Tipo { get; set; }
    private int Pessoa { get; set; }
    private int Usuario { get; set; }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session.Count <= 0)
        {
            FormsAuthentication.SignOut();
            Response.Redirect("login.aspx");
        }

        // SEMPRE carregar
        Licenciado = Session["LICENCIADO"] != null ? Funcoes.strToInt(Session["LICENCIADO"].ToString()) : 0;
        Tipo = Session["TIPO"] != null ? Session["TIPO"].ToString() : "";
        Pessoa = Session["PESSOA"] != null ? Funcoes.strToInt(Session["PESSOA"].ToString()) : 0;
        Usuario = Session["PESSOA"] != null ? Funcoes.strToInt(Session["CODIGO"].ToString()) : 0;        
        
        if (!IsPostBack)
        {

            Funcoes.GravaAuditoriaNovo(0, Request.ServerVariables["REMOTE_ADDR"].ToString(), "Cadastro - Estabelecimentos", "Consulta",Licenciado, Pessoa, Usuario);

            DateTime data = DateTime.Today;
            DateTime primeiroDiaDoMes = new DateTime(data.Year, data.Month, 1);
            DateTime ultimoDiaDoMes = new DateTime(data.Year, data.Month, DateTime.DaysInMonth(data.Year, data.Month));
            txtDataIni.Text = DateTime.Now.ToShortDateString();
            txtDataFim.Text = DateTime.Now.ToShortDateString();

            dvParcial.Visible = false;
            if (Tipo == "A")
            {
                dvCompleta.Visible = true;
                btnImportar.Visible = true;
                dvLimite.Visible = false;
            }
            if (Tipo == "M")
            {
                dvCompleta.Visible = true;
                btnImportar.Visible = false;
            }
            if (Tipo == "R")
            {
                dvCompleta.Visible = true;
                btnImportar.Visible = false;
            }

            RegisterAsyncTask(new PageAsyncTask(async () =>
            {
                await Task.Run(() => ConsultaGeral());
                await Task.Run(() => ConsultaDashboard());
            }));
        }

    }
    protected void btnPesquisar_Click(object sender, EventArgs e)
    {
        RegisterAsyncTask(new PageAsyncTask(async () =>
        {
            await Task.Run(() => ConsultaGeral());
            await Task.Run(() => ConsultaDashboard());
        }));
    }

    private void ConsultaDashboard()
    {
        // Consulta DASHBOARD Status



        using (SqlConnection myConsultaStatus = new SqlConnection(Funcoes.conexao()))
        {
            myConsultaStatus.Open();

            SqlDataAdapter SDAConsultaStatus = new SqlDataAdapter("dbo.stp_pessoas_fj_ins", myConsultaStatus);
            SDAConsultaStatus.SelectCommand.CommandType = CommandType.StoredProcedure;
            SDAConsultaStatus.SelectCommand.Parameters.Add("@flg_operacao", SqlDbType.Char).Value = "G";
            SDAConsultaStatus.SelectCommand.Parameters.Add("@COD_ID_PESSOA_LICENCIADO", SqlDbType.Int).Value = Licenciado;
            SDAConsultaStatus.SelectCommand.Parameters.Add("@NOM_RAZAOSOCIAL", SqlDbType.VarChar).Value = " ";
            SDAConsultaStatus.SelectCommand.Parameters.Add("@FLG_ATIVO", SqlDbType.Char).Value = " "; 
            SDAConsultaStatus.SelectCommand.Parameters.Add("@FLG_TIPO", SqlDbType.Char).Value = "E";

            if (Tipo == "M")
            {
                SDAConsultaStatus.SelectCommand.Parameters.Add("@COD_ID_MARKETPLACE", SqlDbType.Int).Value = Pessoa;
            }
            if (Tipo == "R")
            {
                SDAConsultaStatus.SelectCommand.Parameters.Add("@COD_ID_REPRESENTANTE", SqlDbType.Int).Value = Pessoa;
            }

            DataSet dsConsultaStatus = new DataSet();
            SDAConsultaStatus.Fill(dsConsultaStatus, "PESSOAS_FJ");
            rptStatus.DataSource = dsConsultaStatus.Tables["PESSOAS_FJ"].DefaultView;
            rptStatus.DataBind();
            myConsultaStatus.Close(); myConsultaStatus.Dispose();

            SqlConnection myConsultaTipo = new SqlConnection(Funcoes.conexao());
            myConsultaTipo.Open();
            SqlDataAdapter SDAConsultaTipo = new SqlDataAdapter("dbo.stp_pessoas_fj_ins", myConsultaTipo);
            SDAConsultaTipo.SelectCommand.CommandType = CommandType.StoredProcedure;
            SDAConsultaTipo.SelectCommand.Parameters.Add("@flg_operacao", SqlDbType.Char).Value = "J";
            SDAConsultaTipo.SelectCommand.Parameters.Add("@COD_ID_PESSOA_LICENCIADO", SqlDbType.Int).Value = Licenciado;
            SDAConsultaTipo.SelectCommand.Parameters.Add("@NOM_RAZAOSOCIAL", SqlDbType.VarChar).Value = " ";
            SDAConsultaTipo.SelectCommand.Parameters.Add("@FLG_ATIVO", SqlDbType.Char).Value = " ";
            SDAConsultaTipo.SelectCommand.Parameters.Add("@FLG_TIPO", SqlDbType.Char).Value = "E";

            if (Tipo == "M")
            {
                SDAConsultaTipo.SelectCommand.Parameters.Add("@COD_ID_MARKETPLACE", SqlDbType.Int).Value = Pessoa;
            }
            if (Tipo == "R")
            {
                SDAConsultaTipo.SelectCommand.Parameters.Add("@COD_ID_REPRESENTANTE", SqlDbType.Int).Value = Pessoa;
            }

            DataSet dsConsultaTipo = new DataSet();
            SDAConsultaTipo.Fill(dsConsultaTipo, "PESSOAS_FJ");
            rptTipo.DataSource = dsConsultaTipo.Tables["PESSOAS_FJ"].DefaultView;
            rptTipo.DataBind();
            myConsultaTipo.Close(); myConsultaTipo.Dispose();
        }
    }

    private void ConsultaGeral()
    {
        DateTime dtInicio;
        DateTime dtFim;

        if (DateTime.TryParse(txtDataIni.Text, out dtInicio) && DateTime.TryParse(txtDataFim.Text, out dtFim))
        {
            double dias = (dtFim - dtInicio).TotalDays;

            if ((dias > 90) && (Tipo.Trim()!="A"))
            {

                ClientScript.RegisterStartupScript(this.GetType(), "IntervaloInvalido", "alert('Atenção: o período selecionado ultrapassa 90 dias. Ajuste as datas para continuar.');", true);
            
            }
            else
            {

                Funcoes.GravaAuditoriaNovo(0, Request.ServerVariables["REMOTE_ADDR"].ToString(), "Cadastro - Estabelecimentos", "Consulta: " + txtDataIni.Text.ToString() + " - " + txtDataFim.Text.ToString(),Licenciado,Pessoa, Usuario);

                using (SqlConnection myConsulta = new SqlConnection(Funcoes.conexao()))
                {
                    myConsulta.Open();
                    SqlDataAdapter SDAConsulta = new SqlDataAdapter("dbo.stp_pessoas_fj_ins", myConsulta);
                    SDAConsulta.SelectCommand.CommandType = CommandType.StoredProcedure;
                    SDAConsulta.SelectCommand.Parameters.Add("@flg_operacao", SqlDbType.Char).Value = "[";
                    SDAConsulta.SelectCommand.Parameters.Add("@COD_ID_PESSOA_LICENCIADO", SqlDbType.Int).Value = Licenciado;
                    SDAConsulta.SelectCommand.Parameters.Add("@NOM_RAZAOSOCIAL", SqlDbType.VarChar).Value = txtNome.Text.ToString();
                    SDAConsulta.SelectCommand.Parameters.Add("@NUM_DOCUMENTO", SqlDbType.VarChar).Value = txtCNPJCPF.Text.ToString();
                    SDAConsulta.SelectCommand.Parameters.Add("@NOM_EMAIL", SqlDbType.VarChar).Value = txtEmail.Text.ToString();
                    SDAConsulta.SelectCommand.Parameters.Add("@FLG_ATIVO", SqlDbType.Char).Value = ddlAtivo.SelectedValue.ToString();
                    SDAConsulta.SelectCommand.Parameters.Add("@FLG_TIPO", SqlDbType.Char).Value = "E";

                    SDAConsulta.SelectCommand.Parameters.Add("@DTA_INICIO", SqlDbType.DateTime).Value = Convert.ToDateTime(txtDataIni.Text.ToString());
                    SDAConsulta.SelectCommand.Parameters.Add("@DTA_FIM", SqlDbType.DateTime).Value = Convert.ToDateTime(txtDataFim.Text.ToString());

                    if (Tipo == "M")
                    {
                        SDAConsulta.SelectCommand.Parameters.Add("@COD_ID_MARKETPLACE", SqlDbType.Int).Value = Pessoa;
                    }
                    if (Tipo == "R")
                    {
                        SDAConsulta.SelectCommand.Parameters.Add("@COD_ID_REPRESENTANTE", SqlDbType.Int).Value = Pessoa;
                    }

                    DataSet dsConsulta = new DataSet();
                    SDAConsulta.Fill(dsConsulta, "PESSOAS_FJ");
                    if (Tipo == "A")
                    {
                        rptConsultaCompleta.DataSource = dsConsulta.Tables["PESSOAS_FJ"].DefaultView;
                        rptConsultaCompleta.DataBind();
                    }
                    if (Tipo == "M")
                    {
                        rptConsultaCompleta.DataSource = dsConsulta.Tables["PESSOAS_FJ"].DefaultView;
                        rptConsultaCompleta.DataBind();
                    }
                    if (Tipo == "R")
                    {
                        rptConsultaCompleta.DataSource = dsConsulta.Tables["PESSOAS_FJ"].DefaultView;
                        rptConsultaCompleta.DataBind();
                    }
                    myConsulta.Close(); myConsulta.Dispose();

                    //foreach (RepeaterItem itemE in rptConsultaCompleta.Items)
                    //{
                    //    if (Funcoes.strToInt(((TextBox)itemE.FindControl("txtExcluir")).Text.ToString()) > 0)
                    //    {
                    //        ((LinkButton)itemE.FindControl("lbkExcluir")).Visible = false;
                    //    }
                    //    else
                    //    {
                    //        ((LinkButton)itemE.FindControl("lbkExcluir")).Visible = true;
                    //    }
                    //}            
                }
            }
        }
        else
        {
            ClientScript.RegisterStartupScript(this.GetType(), "DataInvalida", "alert('Data(s) inválida(s)! Verifique e reentre.');", true);
        }        
    }


    protected void rptConsultaCompleta_OnItemCommand(object source, RepeaterCommandEventArgs e)
    {
        if (e.CommandName == "Excluir")
        {
            using (SqlConnection connInsCons = new SqlConnection(Funcoes.conexao()))
            {
                connInsCons.Open();
                SqlCommand cmdInsCons = new SqlCommand("dbo.stp_pessoas_fj_ins", connInsCons);
                cmdInsCons.CommandType = CommandType.StoredProcedure;
                cmdInsCons.Parameters.Add("@flg_operacao", SqlDbType.Char).Value = "E";
                cmdInsCons.Parameters.Add("@COD_ID", SqlDbType.Int).Value = Funcoes.strToInt(Convert.ToString(e.CommandArgument));
                cmdInsCons.Parameters.Add("@COD_ID_PESSOA_LICENCIADO", SqlDbType.Int).Value = Licenciado;
                cmdInsCons.ExecuteNonQuery();
                connInsCons.Close();
                connInsCons.Dispose();

                ClientScript.RegisterStartupScript(this.GetType(),
                    "Alerta", "alert('Registro Excluído com sucesso!');", true);
                ConsultaGeral();
            }

        }
        
        if (e.CommandName == "Verificar")
        {

            using (SqlConnection mySelCadastro = new SqlConnection(Funcoes.conexao()))
            {
                mySelCadastro.Open();
                SqlCommand cmdSelCadastro = new SqlCommand("dbo.stp_pessoas_fj_ins", mySelCadastro);
                cmdSelCadastro.CommandType = CommandType.StoredProcedure;
                cmdSelCadastro.Parameters.Add("@flg_operacao", SqlDbType.Char).Value = "S";
                cmdSelCadastro.Parameters.Add("@COD_ID", SqlDbType.Int).Value = Funcoes.strToInt(Convert.ToString(e.CommandArgument));
                cmdSelCadastro.Parameters.Add("@COD_ID_PESSOA_LICENCIADO", SqlDbType.Int).Value = Licenciado;


                SqlDataReader ReaderCadastro = cmdSelCadastro.ExecuteReader();
                while (ReaderCadastro.Read())
                {
                    var json = "";
                    if (ReaderCadastro["FLG_TIPO_PESSOA"].ToString().Trim() == "PF")
                    {
                        json = zoop.ConsultaCadastroSeller(TIRAACENTOS(ReaderCadastro["NOM_CPF"].ToString().Trim()));
                    }
                    if (ReaderCadastro["FLG_TIPO_PESSOA"].ToString().Trim() == "PJ")
                    {
                        json = zoop.ConsultaCadastroSeller(TIRAACENTOS(ReaderCadastro["NOM_CNPJ"].ToString().Trim()));
                    }
                    if (json.ToString().Trim() != "")
                    {
                        try
                        {
                            JObject o = JObject.Parse(json);

                            using (SqlConnection connInsCons = new SqlConnection(Funcoes.conexao()))
                            {
                                connInsCons.Open();
                                SqlCommand cmdInsCons = new SqlCommand("dbo.stp_pessoas_fj_ins", connInsCons);
                                cmdInsCons.CommandType = CommandType.StoredProcedure;
                                cmdInsCons.Parameters.Add("@flg_operacao", SqlDbType.Char).Value = 'O';
                                cmdInsCons.Parameters.Add("@COD_ID_PESSOA_LICENCIADO", SqlDbType.Int).Value = Licenciado;
                                cmdInsCons.Parameters.Add("@COD_ID", SqlDbType.Int).Value = Funcoes.strToInt(Convert.ToString(e.CommandArgument));

                                cmdInsCons.Parameters.Add("@COD_ID_ZOOP_HABILITADO", SqlDbType.Int).Value = Funcoes.strToInt(ReaderCadastro["COD_ID_ZOOP_HABILITADO"].ToString());
                                cmdInsCons.Parameters.Add("@NUM_TOKEN", SqlDbType.VarChar).Value = o["id"].ToString(); 
                                cmdInsCons.Parameters.Add("@FLG_STATUS_ZOOP", SqlDbType.VarChar).Value = o["status"].ToString();

                                cmdInsCons.Parameters.Add("@COD_ID_PLANO_ZOOP", SqlDbType.Int).Value = Funcoes.strToInt(ReaderCadastro["COD_ID_PLANO_ZOOP"].ToString());

                                cmdInsCons.ExecuteNonQuery();
                                connInsCons.Close();
                                connInsCons.Dispose();
                            }
                        }
                        catch
                        {
                            ClientScript.RegisterStartupScript(this.GetType(),
                                "Alerta", "alert('Ocorreu um erro ao tentar atualizar o status do estabelecimento! Verifique e tente novamente.');", true);
                        }
                    }
                }
            }
            ConsultaGeral();
        }
        
        if (e.CommandName == "Ativar")
        {

            using (SqlConnection connInsCons = new SqlConnection(Funcoes.conexao()))
            {
                connInsCons.Open();
                SqlCommand cmdInsCons = new SqlCommand("dbo.stp_pessoas_fj_ins", connInsCons);
                cmdInsCons.CommandType = CommandType.StoredProcedure;
                cmdInsCons.Parameters.Add("@flg_operacao", SqlDbType.Char).Value = "N";
                cmdInsCons.Parameters.Add("@COD_ID", SqlDbType.Int).Value = Funcoes.strToInt(Convert.ToString(e.CommandArgument));
                cmdInsCons.Parameters.Add("@COD_ID_PESSOA_LICENCIADO", SqlDbType.Int).Value = Licenciado;
                cmdInsCons.Parameters.Add("@FLG_ATIVO", SqlDbType.Char).Value = "S";
                cmdInsCons.ExecuteNonQuery();
                connInsCons.Close();
                connInsCons.Dispose();
            }
            ClientScript.RegisterStartupScript(this.GetType(),
                "Alerta", "alert('Status alterado com sucesso!');", true);
            ConsultaGeral();
        }
        if (e.CommandName == "Inativar")
        {
            using (SqlConnection connInsCons = new SqlConnection(Funcoes.conexao()))
            {
                connInsCons.Open();
                SqlCommand cmdInsCons = new SqlCommand("dbo.stp_pessoas_fj_ins", connInsCons);
                cmdInsCons.CommandType = CommandType.StoredProcedure;
                cmdInsCons.Parameters.Add("@flg_operacao", SqlDbType.Char).Value = "N";
                cmdInsCons.Parameters.Add("@COD_ID", SqlDbType.Int).Value = Funcoes.strToInt(Convert.ToString(e.CommandArgument));
                cmdInsCons.Parameters.Add("@COD_ID_PESSOA_LICENCIADO", SqlDbType.Int).Value = Licenciado;
                cmdInsCons.Parameters.Add("@FLG_ATIVO", SqlDbType.Char).Value = "N";
                cmdInsCons.ExecuteNonQuery();
                connInsCons.Close();
                connInsCons.Dispose();
            }
            ClientScript.RegisterStartupScript(this.GetType(),
                "Alerta", "alert('Status alterado com sucesso!');", true);
            ConsultaGeral();
        }

        if (e.CommandName == "Pendente")
        {
            using (SqlConnection connInsCons = new SqlConnection(Funcoes.conexao()))
            {
                connInsCons.Open();

                SqlCommand cmdInsCons = new SqlCommand("dbo.stp_pessoas_fj_ins", connInsCons);
                cmdInsCons.CommandType = CommandType.StoredProcedure;
                cmdInsCons.Parameters.Add("@flg_operacao", SqlDbType.Char).Value = "N";
                cmdInsCons.Parameters.Add("@COD_ID", SqlDbType.Int).Value = Funcoes.strToInt(Convert.ToString(e.CommandArgument));
                cmdInsCons.Parameters.Add("@COD_ID_PESSOA_LICENCIADO", SqlDbType.Int).Value = Licenciado;
                cmdInsCons.Parameters.Add("@FLG_ATIVO", SqlDbType.Char).Value = "P";
                cmdInsCons.ExecuteNonQuery();
                connInsCons.Close();
                connInsCons.Dispose();
            }
            ClientScript.RegisterStartupScript(this.GetType(),
                "Alerta", "alert('Status alterado com sucesso!');", true);
            ConsultaGeral();
        }
        if (e.CommandName == "Novo")
        {
            using (SqlConnection connInsCons = new SqlConnection(Funcoes.conexao()))
            {
                connInsCons.Open();

                SqlCommand cmdInsCons = new SqlCommand("dbo.stp_pessoas_fj_ins", connInsCons);
                cmdInsCons.CommandType = CommandType.StoredProcedure;
                cmdInsCons.Parameters.Add("@flg_operacao", SqlDbType.Char).Value = "N";
                cmdInsCons.Parameters.Add("@COD_ID", SqlDbType.Int).Value = Funcoes.strToInt(Convert.ToString(e.CommandArgument));
                cmdInsCons.Parameters.Add("@COD_ID_PESSOA_LICENCIADO", SqlDbType.Int).Value = Licenciado;
                cmdInsCons.Parameters.Add("@FLG_ATIVO", SqlDbType.Char).Value = "V";
                cmdInsCons.ExecuteNonQuery();
                connInsCons.Close();
                connInsCons.Dispose();
            }
            ClientScript.RegisterStartupScript(this.GetType(),
                "Alerta", "alert('Status alterado com sucesso!');", true);
            ConsultaGeral();
        }
        ConsultaDashboard();

    }
    protected void rptConsultaParcial_OnItemCommand(object source, RepeaterCommandEventArgs e)
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
        //ClientScript.RegisterStartupScript(this.GetType(),
        //  "AtualizacaoDados", "alert('Caso queira atualizar a consulta exibindo os dados alterados, clique no botão Pesquisar! Caso contrário, basta continuar normalmente.');", true);
        //ConsultaGeral();
    }
    protected void btnNovo_Click(object sender, EventArgs e)
    {
        ClientScript.RegisterStartupScript(this.GetType(),
    "Estabelecimentos", "openPopupWindow('cad_estabelecimentos_steps.aspx?id=" + Funcoes.Encrypt("0").ToString() + "','EstabelecimentosEdicao',1024,800);", true);

    }
    protected void btnImportar_Click(object sender, EventArgs e)
    {
        ClientScript.RegisterStartupScript(this.GetType(),
            "Estabelecimentos", "openPopupWindow('cad_estabelecimentos_importar.aspx?id=0','EstabelecimentosImportacao',1024,800);", true);

    }

    public static string TIRAACENTOS(string str)
    {
        str = str.Replace("-", "");
        str = str.Replace(".", "");
        str = str.Replace("/", "");
        str = str.Replace("(", "");
        str = str.Replace(")", "");
        str = str.Replace(" ", "");
        return str;
    }

    protected void btnNovoPadrao_Click(object sender, EventArgs e)
    {
        ClientScript.RegisterStartupScript(this.GetType(),
    "Estabelecimentos", "openPopupWindow('cad_estabelecimentos_padrao.aspx?id="+Funcoes.Encrypt("0").ToString()+"','EstabelecimentosEdicao',1024,800);", true);

    }
}