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


public partial class cad_eventos_equipamentos_vendas : System.Web.UI.Page
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
            ConsultaFicha();
            ConsultaGeral();
        }

    }


    private void ConsultaFicha()
    {
        SqlConnection mySelCadastro = new SqlConnection(Funcoes.conexao());
        mySelCadastro.Open();
        SqlCommand cmdSelCadastro = new SqlCommand("dbo.stp_eventos_equipamentos_ins", mySelCadastro);
        cmdSelCadastro.CommandType = CommandType.StoredProcedure;
        cmdSelCadastro.Parameters.Add("@flg_operacao", SqlDbType.Char).Value = "S";
        cmdSelCadastro.Parameters.Add("@COD_ID", SqlDbType.Int).Value = Funcoes.strToInt(sid_id.ToString());
        cmdSelCadastro.Parameters.Add("@COD_ID_PESSOA_LICENCIADO", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["LICENCIADO"].ToString());

        SqlDataReader ReaderCadastro = cmdSelCadastro.ExecuteReader();
        while (ReaderCadastro.Read())
        {
            txtIDEvento.Text = ReaderCadastro["COD_ID"].ToString();
            txtEvento.Text = ReaderCadastro["NOM_EVENTO"].ToString();
            txtInicio.Text = Convert.ToDateTime(ReaderCadastro["DTA_INICIO"].ToString()).ToShortDateString();
            txtFim.Text = Convert.ToDateTime(ReaderCadastro["DTA_TERMINO"].ToString()).ToShortDateString();
            txtSerial.Text = ReaderCadastro["NUM_SERIAL"].ToString();
            txtEstabelecimento.Text = ReaderCadastro["NOM_ESTABELECIMENTO"].ToString();
        }
    }

    private void ConsultaGeral()
    {
        SqlConnection myConsulta = new SqlConnection(Funcoes.conexao());
        myConsulta.Open();
        SqlDataAdapter SDAConsulta = new SqlDataAdapter("dbo.stp_eventos_equipamentos_ins", myConsulta);
        SDAConsulta.SelectCommand.CommandType = CommandType.StoredProcedure;
        SDAConsulta.SelectCommand.Parameters.Add("@flg_operacao", SqlDbType.Char).Value = "T";
        SDAConsulta.SelectCommand.Parameters.Add("@COD_ID_PESSOA_LICENCIADO", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["LICENCIADO"].ToString());
        SDAConsulta.SelectCommand.Parameters.Add("@NUM_DEVICE_SERIAL", SqlDbType.VarChar).Value = txtSerial.Text.ToString();
        SDAConsulta.SelectCommand.Parameters.Add("@DTA_INICIO", SqlDbType.DateTime).Value = Convert.ToDateTime(txtInicio.Text.ToString());
        SDAConsulta.SelectCommand.Parameters.Add("@DTA_FIM", SqlDbType.DateTime).Value = Convert.ToDateTime(txtFim.Text.ToString());

        DataSet dsConsulta = new DataSet();
        SDAConsulta.Fill(dsConsulta, "TRANSACOES");

        rptVendas.DataSource = dsConsulta.Tables["TRANSACOES"].DefaultView;
        rptVendas.DataBind();

        myConsulta.Close(); myConsulta.Dispose();
    }

    protected void btnFechar_Click(object sender, EventArgs e)
    {
        ClientScript.RegisterStartupScript(this.GetType(),
"Alerta", "opener.PostBackOnMainPage(); window.close(); ", true);

    }
}