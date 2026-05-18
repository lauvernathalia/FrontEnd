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

public partial class teste_funcoes : System.Web.UI.Page
{

    private string connString = Funcoes.conexao();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            CarregarDados(0); // Carrega todos os dados na sessão
        }
    }

    private void CarregarDados(int pageIndex)
    {
        using (SqlConnection conn = new SqlConnection(connString))
        {
            using (SqlCommand cmd = new SqlCommand("dbo.stp_transacoes_ins", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@flg_operacao", "H");
                cmd.Parameters.AddWithValue("@COD_ID_PESSOA_LICENCIADO", Funcoes.strToInt(HttpContext.Current.Session["LICENCIADO"].ToString()));
                cmd.Parameters.AddWithValue("@DTA_INICIO", Convert.ToDateTime("13/03/2025"));
                cmd.Parameters.AddWithValue("@DTA_FIM", Convert.ToDateTime("13/03/2025"));

                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                DataTable dt = new DataTable();
                dt.Load(reader);

                GridView1.DataSource = dt;
                GridView1.PageIndex = pageIndex;
                GridView1.DataBind();
            }
        }
    }


    protected void GridView1_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        GridView1.PageIndex = e.NewPageIndex;
        CarregarDados(e.NewPageIndex);

    }
}