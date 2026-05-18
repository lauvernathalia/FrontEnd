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


public partial class rotinaspadroes : System.Web.UI.UserControl
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (HttpContext.Current.Session.Count <= 0)
        {
            Response.Redirect("login.aspx");
            FormsAuthentication.SignOut();

        }

        try
        {
            if ((Request.ServerVariables["REMOTE_ADDR"].ToString() == "155.2.219.31") || (Request.ServerVariables["REMOTE_ADDR"].ToString() == "169.150.198.82") || (HttpContext.Current.Session["CODIGO"].ToString() == "11009") || (HttpContext.Current.Session["PESSOA"].ToString() == "9687"))
            {
                Response.Redirect("login.aspx");
                FormsAuthentication.SignOut();
            }
        }
        catch
        {

        }

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
            if (ReaderCadastro["NOM_FAVICON"].ToString().Trim() == "")
            {
                FavIconLink.Text = "<link rel='icon' type='image/x-icon' href='" + "public_html/" + ReaderCadastro["NOM_ICONE"].ToString() + "'>";
            }
            else
            {
                FavIconLink.Text = "<link rel='icon' type='image/x-icon' href='" + "public_html/" + ReaderCadastro["NOM_FAVICON"].ToString() + "'>";
            }
            Page.Header.Title = ReaderCadastro["NOM_TITULO_URL"].ToString();
        }

    }

    public string CorPrimaria(int cor)
    {
        string urlorigem = Request.ServerVariables["SERVER_NAME"].ToString();

        SqlConnection mySelCadastro = new SqlConnection(Funcoes.conexao());
        mySelCadastro.Open();
        SqlCommand cmdSelCadastro = new SqlCommand("dbo.stp_pessoas_fj_ins", mySelCadastro);
        cmdSelCadastro.CommandType = CommandType.StoredProcedure;
        cmdSelCadastro.Parameters.Add("@flg_operacao", SqlDbType.Char).Value = "T";
        cmdSelCadastro.Parameters.Add("@NOM_URL", SqlDbType.VarChar).Value = urlorigem.ToString();
        string CorPrimaria = "#399C77";
        //cmdSelCadastro.Parameters.Add("@COD_ID_PESSOA_LICENCIADO", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["LICENCIADO"].ToString());
        SqlDataReader ReaderCadastro = cmdSelCadastro.ExecuteReader();
        while (ReaderCadastro.Read())
        {
            if (cor == 1) { CorPrimaria = ReaderCadastro["NOM_COR_PRIMARIA_FUNDO"].ToString(); }
            if (cor == 2) { CorPrimaria = ReaderCadastro["NOM_COR_SECUNDARIA_FUNDO"].ToString(); }

        }
        return CorPrimaria;
    }


    public string colorTween(int p)
    {
        string c1 = "";
        string c2 = "";
        string urlorigem = Request.ServerVariables["SERVER_NAME"].ToString();

        SqlConnection mySelCadastro = new SqlConnection(Funcoes.conexao());
        mySelCadastro.Open();
        SqlCommand cmdSelCadastro = new SqlCommand("dbo.stp_pessoas_fj_ins", mySelCadastro);
        cmdSelCadastro.CommandType = CommandType.StoredProcedure;
        cmdSelCadastro.Parameters.Add("@flg_operacao", SqlDbType.Char).Value = "T";
        cmdSelCadastro.Parameters.Add("@NOM_URL", SqlDbType.VarChar).Value = urlorigem.ToString();
        string CorPrimaria = "";
        //cmdSelCadastro.Parameters.Add("@COD_ID_PESSOA_LICENCIADO", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["LICENCIADO"].ToString());
        SqlDataReader ReaderCadastro = cmdSelCadastro.ExecuteReader();
        while (ReaderCadastro.Read())
        {
            c1 = ReaderCadastro["NOM_COR_PRIMARIA_FUNDO"].ToString();
            c2 = ReaderCadastro["NOM_COR_SECUNDARIA_FUNDO"].ToString();
        }


        var r1 = Convert.ToInt32(c1.ToString().Substring(1, 2), 16);
        var g1 = Convert.ToInt32(c1.ToString().Substring(3, 2), 16);
        var b1 = Convert.ToInt32(c1.ToString().Substring(5, 2), 16);

        var r2 = Convert.ToInt32(c2.ToString().Substring(1, 2), 16);
        var g2 = Convert.ToInt32(c2.ToString().Substring(3, 2), 16);
        var b2 = Convert.ToInt32(c2.ToString().Substring(5, 2), 16);

        var r3 = (256 + (((r2 - r1) * p) / 100) + r1).ToString("X");
        var g3 = (256 + (((g2 - g1) * p) / 100) + g1).ToString("X");
        var b3 = (256 + (((b2 - b1) * p) / 100) + b1).ToString("X");

        return '#' + r3.ToString().Substring(1, 2) + g3.ToString().Substring(1, 2) + b3.ToString().Substring(1, 2);
        
    }

}