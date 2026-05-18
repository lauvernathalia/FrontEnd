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

/// <summary>
/// Summary description for tabelas
/// </summary>
public class tabelas
{
	public static DataSet tModelos()
	{
            SqlConnection myModelos = new SqlConnection(Funcoes.conexao());
            myModelos.Open();
            SqlCommand cmdModelos = new SqlCommand("dbo.stp_equipamentos_modelos_ins", myModelos);
            cmdModelos.CommandType = CommandType.StoredProcedure;
            cmdModelos.Parameters.Add("@flg_operacao", SqlDbType.VarChar).Value = "C";
            cmdModelos.Parameters.Add("@COD_ID_PESSOA_LICENCIADO", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["LICENCIADO"].ToString());
            SqlDataAdapter drModelos = new SqlDataAdapter();
            drModelos.SelectCommand = cmdModelos;
            DataSet dsModelos = new DataSet();
            drModelos.Fill(dsModelos, "EQUIPAMENTOS_MODELOS");

            return dsModelos;
	}

    public static DataSet tSeriais()
    {
        SqlConnection mySeriais = new SqlConnection(Funcoes.conexao());
        mySeriais.Open();
        SqlCommand cmdSeriais = new SqlCommand("dbo.stp_equipamentos_seriais_ins", mySeriais);
        cmdSeriais.CommandType = CommandType.StoredProcedure;
        cmdSeriais.Parameters.Add("@flg_operacao", SqlDbType.VarChar).Value = "C";
        cmdSeriais.Parameters.Add("@COD_ID_PESSOA_LICENCIADO", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["LICENCIADO"].ToString());
        SqlDataAdapter drSeriais = new SqlDataAdapter();
        drSeriais.SelectCommand = cmdSeriais;
        DataSet dsSeriais = new DataSet();
        drSeriais.Fill(dsSeriais, "EQUIPAMENTOS_SERIAIS");

        return dsSeriais;
    }

    public static DataSet tOperadoras()
    {
        SqlConnection myOperadoras = new SqlConnection(Funcoes.conexao());
        myOperadoras.Open();
        SqlCommand cmdOperadoras = new SqlCommand("dbo.stp_operadoras_ins", myOperadoras);
        cmdOperadoras.CommandType = CommandType.StoredProcedure;
        cmdOperadoras.Parameters.Add("@flg_operacao", SqlDbType.VarChar).Value = "C";
        SqlDataAdapter drOperadoras = new SqlDataAdapter();
        drOperadoras.SelectCommand = cmdOperadoras;
        DataSet dsOperadoras = new DataSet();
        drOperadoras.Fill(dsOperadoras, "OPERADORAS");

        return dsOperadoras;
    }

    public static DataSet tProprietario(string sTipo)
    {
        SqlConnection myEstabelecimentos = new SqlConnection(Funcoes.conexao());
        myEstabelecimentos.Open();
        SqlCommand cmdEstabelecimentos = new SqlCommand("dbo.stp_pessoas_fj_ins", myEstabelecimentos);
        cmdEstabelecimentos.CommandType = CommandType.StoredProcedure;
        cmdEstabelecimentos.Parameters.Add("@flg_operacao", SqlDbType.VarChar).Value = "(";
        cmdEstabelecimentos.Parameters.Add("@COD_ID_PESSOA_LICENCIADO", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["LICENCIADO"].ToString());

        cmdEstabelecimentos.Parameters.Add("@FLG_TIPO", SqlDbType.Char).Value = sTipo.ToString();
        SqlDataAdapter drEstabelecimentos = new SqlDataAdapter();
        drEstabelecimentos.SelectCommand = cmdEstabelecimentos;
        DataSet dsEstabelecimentos = new DataSet();
        drEstabelecimentos.Fill(dsEstabelecimentos, "PESSOAS_FJ");

        return dsEstabelecimentos;
    }

    public static DataSet tPessoas(string sTipo)
    {
        SqlConnection myEstabelecimentos = new SqlConnection(Funcoes.conexao());
        myEstabelecimentos.Open();
        SqlCommand cmdEstabelecimentos = new SqlCommand("dbo.stp_pessoas_fj_ins", myEstabelecimentos);
        cmdEstabelecimentos.CommandType = CommandType.StoredProcedure;
        cmdEstabelecimentos.Parameters.Add("@flg_operacao", SqlDbType.VarChar).Value = "C";
        cmdEstabelecimentos.Parameters.Add("@COD_ID_PESSOA_LICENCIADO", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["LICENCIADO"].ToString());

        if (HttpContext.Current.Session["TIPO"].ToString() == "M")
        {
            cmdEstabelecimentos.Parameters.Add("@COD_ID_MARKETPLACE", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["PESSOA"].ToString());
        }

        if (HttpContext.Current.Session["TIPO"].ToString() == "R")
        {
            cmdEstabelecimentos.Parameters.Add("@COD_ID_REPRESENTANTE", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["PESSOA"].ToString());
        }

        cmdEstabelecimentos.Parameters.Add("@FLG_TIPO", SqlDbType.Char).Value = sTipo.ToString();
        SqlDataAdapter drEstabelecimentos = new SqlDataAdapter();
        drEstabelecimentos.SelectCommand = cmdEstabelecimentos;
        DataSet dsEstabelecimentos = new DataSet();
        drEstabelecimentos.Fill(dsEstabelecimentos, "PESSOAS_FJ");

        return dsEstabelecimentos;
    }

    public static DataSet tAdquirente()
    {
        SqlConnection myAdquirente = new SqlConnection(Funcoes.conexao());
        myAdquirente.Open();
        SqlCommand cmdAdquirente = new SqlCommand("dbo.stp_adquirente_ins", myAdquirente);
        cmdAdquirente.CommandType = CommandType.StoredProcedure;
        cmdAdquirente.Parameters.Add("@flg_operacao", SqlDbType.VarChar).Value = "C";
        SqlDataAdapter drAdquirente = new SqlDataAdapter();
        drAdquirente.SelectCommand = cmdAdquirente;
        DataSet dsAdquirente = new DataSet();
        drAdquirente.Fill(dsAdquirente, "ADQUIRENTE");

        return dsAdquirente;
    }

    public static DataSet tIntegracoes()
    {
        SqlConnection myIntegracoes = new SqlConnection(Funcoes.conexao());
        myIntegracoes.Open();
        SqlCommand cmdIntegracoes = new SqlCommand("dbo.stp_integracoes_ins", myIntegracoes);
        cmdIntegracoes.CommandType = CommandType.StoredProcedure;
        cmdIntegracoes.Parameters.Add("@flg_operacao", SqlDbType.VarChar).Value = "C";
        cmdIntegracoes.Parameters.Add("@FLG_ATIVO", SqlDbType.Char).Value = "S";
        SqlDataAdapter drIntegracoes = new SqlDataAdapter();
        drIntegracoes.SelectCommand = cmdIntegracoes;
        DataSet dsIntegracoes = new DataSet();
        drIntegracoes.Fill(dsIntegracoes, "INTEGRACOES");

        return dsIntegracoes;
    }


}