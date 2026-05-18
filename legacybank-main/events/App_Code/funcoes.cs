using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Data.SqlClient;
using System.Security.Cryptography;
using System.IO;
using System.Net;
using System.ComponentModel;
using System.Drawing;
using System.Web.SessionState;
using System.Text;
using System.Collections.Generic;
using System.Data.OleDb;
using System.Text.RegularExpressions;
using System.Collections;

using System.Linq;
using System.Threading.Tasks;

using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;



/// <summary>
/// Summary description for Class1
/// </summary>


public class WebCEP
{
    #region "Váriavies"
    string _uf;
    string _cidade;
    string _bairro;
    string _tipo_lagradouro;
    string _lagradouro;
    string _resultado;
    string _resultato_txt;
    #endregion

    #region "Propiedades"
    public string UF
    {
        get { return _uf; }
    }
    public string Cidade
    {
        get { return _cidade; }
    }
    public string Bairro
    {
        get { return _bairro; }
    }
    public string TipoLagradouro
    {
        get { return _tipo_lagradouro; }
    }
    public string Lagradouro
    {
        get { return _lagradouro; }
    }
    public string Resultado
    {
        get { return _resultado; }
    }
    public string ResultadoTXT
    {
        get { return _resultato_txt; }
    }
    #endregion
    #region "Construtor"
    /// <summary>  
    /// WebService para Busca de CEP  
    ///  </summary>  
    /// <param  name="CEP"></param>  
    /// 
    public WebCEP(string CEP)
    {
        _uf = "";
        _cidade = "";
        _bairro = "";
        _tipo_lagradouro = "";
        _lagradouro = "";
        _resultado = "0";
        _resultato_txt = "CEP não encontrado";

        //Cria um DataSet  baseado no retorno do XML  
        DataSet ds = new DataSet();
        ds.ReadXml("http://cep.republicavirtual.com.br/web_cep.php?cep=" + CEP.Replace("-", "").Trim() + "&formato=xml");

        if (ds != null)
        {
            if (ds.Tables[0].Rows.Count > 0)
            {
                _resultado = ds.Tables[0].Rows[0]["resultado"].ToString();
                switch (_resultado)
                {
                    case "1":
                        _uf = ds.Tables[0].Rows[0]["uf"].ToString().Trim();
                        _cidade = ds.Tables[0].Rows[0]["cidade"].ToString().Trim();
                        _bairro = ds.Tables[0].Rows[0]["bairro"].ToString().Trim();
                        _tipo_lagradouro = ds.Tables[0].Rows[0]["tipo_logradouro"].ToString().Trim();
                        _lagradouro = ds.Tables[0].Rows[0]["logradouro"].ToString().Trim();
                        _resultato_txt = "CEP completo";
                        break;
                    case "2":
                        _uf = ds.Tables[0].Rows[0]["uf"].ToString().Trim();
                        _cidade = ds.Tables[0].Rows[0]["cidade"].ToString().Trim();
                        _bairro = "";
                        _tipo_lagradouro = "";
                        _lagradouro = "";
                        _resultato_txt = "CEP  único";
                        break;
                    default:
                        _uf = "";
                        _cidade = "";
                        _bairro = "";
                        _tipo_lagradouro = "";
                        _lagradouro = "";
                        _resultato_txt = "CEP não  encontrado";
                        break;
                }
            }
        }
        //Exemplo do retorno da  WEB  
        //<?xml version="1.0"  encoding="iso-8859-1"?>  
        //<webservicecep>  
        //<uf>RS</uf>  
        //<cidade>Porto  Alegre</cidade>  
        //<bairro>Passo  D'Areia</bairro>  
        //<tipo_logradouro>Avenida</tipo_logradouro>  
        //<logradouro>Assis Brasil</logradouro>  
        //<resultado>1</resultado>  
        //<resultado_txt>sucesso - cep  completo</resultado_txt>  
        //</webservicecep>  
    }
    #endregion
}




public class Licenciado
{
    private int codigo;
    private string nome;

    public int Codigo
    {
        get
        {
            return codigo;
        }
        set
        {
            codigo = value;
        }
    }

    public string Nome
    {
        get
        {
            return nome;
        }
        set
        {
            nome = value;
        }
    }
}


public class clsFuncao
{
    private string _Funcao;
    public string Funcao
    {
        get
        {
            return _Funcao;
        }
        set
        {
            _Funcao = value;
        }
    }

}


public class Funcoes
{

    public static bool ValidaCNPJ(string vrCNPJ)
    {

        string CNPJ = vrCNPJ.Replace(".", "");

        CNPJ = CNPJ.Replace("/", "");

        CNPJ = CNPJ.Replace("-", "");



        int[] digitos, soma, resultado;

        int nrDig;

        string ftmt;

        bool[] CNPJOk;



        ftmt = "6543298765432";

        digitos = new int[14];

        soma = new int[2];

        soma[0] = 0;

        soma[1] = 0;

        resultado = new int[2];

        resultado[0] = 0;

        resultado[1] = 0;

        CNPJOk = new bool[2];

        CNPJOk[0] = false;

        CNPJOk[1] = false;



        try
        {

            for (nrDig = 0; nrDig < 14; nrDig++)
            {

                digitos[nrDig] = int.Parse(

                    CNPJ.Substring(nrDig, 1));

                if (nrDig <= 11)

                    soma[0] += (digitos[nrDig] *

                      int.Parse(ftmt.Substring(

                      nrDig + 1, 1)));

                if (nrDig <= 12)

                    soma[1] += (digitos[nrDig] *

                      int.Parse(ftmt.Substring(

                      nrDig, 1)));

            }



            for (nrDig = 0; nrDig < 2; nrDig++)
            {

                resultado[nrDig] = (soma[nrDig] % 11);

                if ((resultado[nrDig] == 0) || (

                     resultado[nrDig] == 1))

                    CNPJOk[nrDig] = (

                    digitos[12 + nrDig] == 0);

                else

                    CNPJOk[nrDig] = (

                    digitos[12 + nrDig] == (

                    11 - resultado[nrDig]));

            }

            return (CNPJOk[0] && CNPJOk[1]);

        }

        catch
        {

            return false;

        }

    }



    public static bool ValidaCPF(string vrCPF)
    {

        string valor = vrCPF.Replace(".", "");

        valor = valor.Replace("-", "");



        if (valor.Length != 11)

            return false;



        bool igual = true;

        for (int i = 1; i < 11 && igual; i++)

            if (valor[i] != valor[0])

                igual = false;



        if (igual || valor == "12345678909")

            return false;



        int[] numeros = new int[11];



        for (int i = 0; i < 11; i++)

            numeros[i] = int.Parse(
              valor[i].ToString());


        int soma = 0;

        for (int i = 0; i < 9; i++)

            soma += (10 - i) * numeros[i];



        int resultado = soma % 11;



        if (resultado == 1 || resultado == 0)
        {

            if (numeros[9] != 0)

                return false;

        }

        else if (numeros[9] != 11 - resultado)

            return false;



        soma = 0;

        for (int i = 0; i < 10; i++)

            soma += (11 - i) * numeros[i];



        resultado = soma % 11;



        if (resultado == 1 || resultado == 0)
        {

            if (numeros[10] != 0)

                return false;

        }

        else

            if (numeros[10] != 11 - resultado)

                return false;



        return true;

    }

    public static string CarregaVerificaCliente()
    {
        SqlConnection connSelConf = new SqlConnection(conexao());
        SqlCommand cmdSelConf = new SqlCommand("dbo.stp_configuracoes_ins", connSelConf);
        cmdSelConf.CommandType = CommandType.StoredProcedure;
        cmdSelConf.Parameters.Add("@flg_operacao", SqlDbType.Char).Value = "S";
        cmdSelConf.Parameters.Add("@COD_ID_PESSOA_LICENCIADO", SqlDbType.Int).Value = strToInt(HttpContext.Current.Session["PESSOA"].ToString());

        connSelConf.Open();
        SqlDataReader Reader = cmdSelConf.ExecuteReader();
        string VerificaCliente = "N";
        while (Reader.Read())
        {
            VerificaCliente = Reader["FLG_VERIFICA_CLIENTE"].ToString();
        }
        return VerificaCliente.ToString();
    }

    public static string CarregaVerificaFornecedor()
    {
        SqlConnection connSelConf = new SqlConnection(conexao());
        SqlCommand cmdSelConf = new SqlCommand("dbo.stp_configuracoes_ins", connSelConf);
        cmdSelConf.CommandType = CommandType.StoredProcedure;
        cmdSelConf.Parameters.Add("@flg_operacao", SqlDbType.Char).Value = "S";
        cmdSelConf.Parameters.Add("@COD_ID_PESSOA_LICENCIADO", SqlDbType.Int).Value = strToInt(HttpContext.Current.Session["PESSOA"].ToString());

        connSelConf.Open();
        SqlDataReader Reader = cmdSelConf.ExecuteReader();
        string VerificaFornecedor = "N";
        while (Reader.Read())
        {
            VerificaFornecedor = Reader["FLG_VERIFICA_FORNECEDOR"].ToString();
        }
        return VerificaFornecedor.ToString();
    }


    public static string CarregaEstilo()
    {
        SqlConnection connSelConf = new SqlConnection(conexao());
        SqlCommand cmdSelConf = new SqlCommand("dbo.stp_configuracoes_ins", connSelConf);
        cmdSelConf.CommandType = CommandType.StoredProcedure;
        cmdSelConf.Parameters.Add("@flg_operacao", SqlDbType.Char).Value = "S";
        cmdSelConf.Parameters.Add("@COD_ID_PESSOA_LICENCIADO", SqlDbType.Int).Value = strToInt(HttpContext.Current.Session["PESSOA"].ToString());

        connSelConf.Open();
        SqlDataReader Reader = cmdSelConf.ExecuteReader();
        string estilopadrao = "Padrao.css";
        while (Reader.Read())
        {
            estilopadrao = Reader["NOM_ESTILO"].ToString();
        }
        return estilopadrao.ToString();
    }


    public static string VerificaINCALT(int submenu, int usuario)
    {
        string sqlSelAcesso = "SELECT ISNULL(FLG_INC_ALT,'N') AS FLG_INC_ALT FROM SIS_USUARIO_ACESSOS WHERE COD_ID_SUBMENUS = " + submenu.ToString() + " AND COD_ID_USUARIO = " + usuario.ToString();
        SqlConnection mySelAcesso = new SqlConnection(conexao());
        mySelAcesso.Open();
        SqlCommand objSelAcessoCmd = new SqlCommand(sqlSelAcesso, mySelAcesso);
        SqlDataReader LeSelAcesso = objSelAcessoCmd.ExecuteReader();
        string Acesso = "N";
        while (LeSelAcesso.Read())
        {
            Acesso = LeSelAcesso["FLG_INC_ALT"].ToString();
        }
        mySelAcesso.Close();
        mySelAcesso.Dispose();
        return Acesso;
    }

    public static string VerificaRATEIO(int licenciado)
    {
        SqlConnection connSelConf = new SqlConnection(conexao());
        SqlCommand cmdSelConf = new SqlCommand("dbo.stp_configuracoes_ins", connSelConf);
        cmdSelConf.CommandType = CommandType.StoredProcedure;
        cmdSelConf.Parameters.Add("@flg_operacao", SqlDbType.Char).Value = "S";
        cmdSelConf.Parameters.Add("@COD_ID_PESSOA_LICENCIADO", SqlDbType.Int).Value = licenciado;
        connSelConf.Open();
        SqlDataReader ReaderConf = cmdSelConf.ExecuteReader();
        String sRateio = "S";
        while (ReaderConf.Read())
        {
            sRateio = ReaderConf["FLG_RATEIO"].ToString();
        }
        return sRateio;
    }


    public static string VerificaPUBLICOPRIVADO(int submenu, int usuario)
    {
        string sqlSelAcesso = "SELECT ISNULL(FLG_PUBLICO_PRIVADO,'P') AS FLG_PUBLICO_PRIVADO FROM SIS_USUARIO_ACESSOS WHERE COD_ID_SUBMENUS = " + submenu.ToString() + " AND COD_ID_USUARIO = " + usuario.ToString();
        SqlConnection mySelAcesso = new SqlConnection(conexao());
        mySelAcesso.Open();
        SqlCommand objSelAcessoCmd = new SqlCommand(sqlSelAcesso, mySelAcesso);
        SqlDataReader LeSelAcesso = objSelAcessoCmd.ExecuteReader();
        string Acesso = "P";
        while (LeSelAcesso.Read())
        {
            Acesso = LeSelAcesso["FLG_PUBLICO_PRIVADO"].ToString();
        }
        mySelAcesso.Close();
        mySelAcesso.Dispose();
        return Acesso;
    }

    public static string VerificaRESTRITO(int submenu, int usuario)
    {
        string sqlSelAcesso = "SELECT ISNULL(FLG_RESTRITO,'N') AS FLG_RESTRITO FROM SIS_USUARIO_ACESSOS WHERE COD_ID_SUBMENUS = " + submenu.ToString() + " AND COD_ID_USUARIO = " + usuario.ToString();
        SqlConnection mySelAcesso = new SqlConnection(conexao());
        mySelAcesso.Open();
        SqlCommand objSelAcessoCmd = new SqlCommand(sqlSelAcesso, mySelAcesso);
        SqlDataReader LeSelAcesso = objSelAcessoCmd.ExecuteReader();
        string Acesso = "N";
        while (LeSelAcesso.Read())
        {
            Acesso = LeSelAcesso["FLG_RESTRITO"].ToString();
        }
        mySelAcesso.Close();
        mySelAcesso.Dispose();
        return Acesso;
    }



    public static string VerificaEXP(int submenu, int usuario)
    {
        string sqlSelAcesso = "SELECT ISNULL(FLG_EXP,'N') AS FLG_EXP FROM SIS_USUARIO_ACESSOS WHERE COD_ID_SUBMENUS = " + submenu.ToString() + " AND COD_ID_USUARIO = " + usuario.ToString();
        SqlConnection mySelAcesso = new SqlConnection(conexao());
        mySelAcesso.Open();
        SqlCommand objSelAcessoCmd = new SqlCommand(sqlSelAcesso, mySelAcesso);
        SqlDataReader LeSelAcesso = objSelAcessoCmd.ExecuteReader();
        string Acesso = "N";
        while (LeSelAcesso.Read())
        {
            Acesso = LeSelAcesso["FLG_EXP"].ToString();
        }
        mySelAcesso.Close();
        mySelAcesso.Dispose();

        return Acesso;
    }


    public static string VerificaEXC(int submenu, int usuario)
    {
        string sqlSelAcesso = "SELECT ISNULL(FLG_EXC,'N') AS FLG_EXC FROM SIS_USUARIO_ACESSOS WHERE COD_ID_SUBMENUS = " + submenu.ToString() + " AND COD_ID_USUARIO = " + usuario.ToString();
        SqlConnection mySelAcesso = new SqlConnection(conexao());
        mySelAcesso.Open();
        SqlCommand objSelAcessoCmd = new SqlCommand(sqlSelAcesso, mySelAcesso);
        SqlDataReader LeSelAcesso = objSelAcessoCmd.ExecuteReader();
        string Acesso = "N";
        while (LeSelAcesso.Read())
        {
            Acesso = LeSelAcesso["FLG_EXC"].ToString();
        }
        mySelAcesso.Close();
        mySelAcesso.Dispose();

        return Acesso;
    }


    public static bool exportarExcel(DataGrid grid, string saveAsFile)
    {
        //if (grid.Items.Count + 1 <= 65536)
        //{
        HttpContext.Current.Response.Clear();
        HttpContext.Current.Response.ContentType = "application/vnd.ms-excel";
        HttpContext.Current.Response.AddHeader("content-disposition", "attachment;filename=" + saveAsFile + ".xls");

        //              Remover caracteres do header - Content-Type
        HttpContext.Current.Response.Charset = "UTF-8";
        HttpContext.Current.Response.ContentEncoding = System.Text.Encoding.GetEncoding("windows-1252");
        //HttpContext.Current.Response.Charset = "";

        //              desabilita o  view state.
        grid.EnableViewState = false;
        StringWriter tw = new System.IO.StringWriter();
        HtmlTextWriter hw = new System.Web.UI.HtmlTextWriter(tw);
        grid.RenderControl(hw);
        //              Escrever o html no navegador
        HttpContext.Current.Response.Write(tw.ToString());
        //              Termina o response
        HttpContext.Current.Response.End();
        //}
        //else
        //{
        //	HttpContext.Current.Response.Write("Muitas linhas para exportar para o Exel !!!");
        //}    
        return true;

    }

    public static int strToInt(string str)
    {
        return (str == null || str == "") ? 0 : int.Parse(str);
    }

    public static string conexao()
    {
        string sConexao = ConfigurationManager.AppSettings["user"].ToString() + ";" +
            ConfigurationManager.AppSettings["password"].ToString() + ";" +
            ConfigurationManager.AppSettings["datasource"].ToString() + ";" +
            ConfigurationManager.AppSettings["basecatalog"].ToString();
        //string sConexao = ConfigurationManager.ConnectionStrings["legacysql"].ConnectionString;
        return sConexao;
    }

    public static string conexaoMySql()
    {

        string sqlSelAcessoMySql = "SELECT NOM_CONEXAO_PORTAL FROM CONFIGURACOES WHERE COD_ID_PESSOA_LICENCIADO = " + HttpContext.Current.Session["PESSOA"].ToString();
        SqlConnection mySelAcessoMySql = new SqlConnection(conexao());
        mySelAcessoMySql.Open();
        SqlCommand objSelAcessoCmdMySql = new SqlCommand(sqlSelAcessoMySql, mySelAcessoMySql);
        SqlDataReader LeSelAcessoMySql = objSelAcessoCmdMySql.ExecuteReader();
        string AcessoMySql = "";
        while (LeSelAcessoMySql.Read())
        {
            AcessoMySql = LeSelAcessoMySql["NOM_CONEXAO_PORTAL"].ToString();
        }
        return AcessoMySql;
    }

    public static DataSet dtsTipo()
    {
        //Tipo de Documento a Pagar
        SqlConnection myTipo = new SqlConnection(conexao());
        myTipo.Open();
        SqlDataAdapter myCommandTipo = new SqlDataAdapter("SELECT TDCP.COD_ID, TDCP.NOM_TDCP FROM TDCP WHERE TDCP.FLG_ATIVO='S' AND TDCP.COD_ID_PESSOA_LICENCIADO = " + HttpContext.Current.Session["PESSOA"].ToString() + " ORDER BY NOM_TDCP", myTipo);
        DataSet dsTipo = new DataSet();
        myCommandTipo.Fill(dsTipo, "TDCP");
        myTipo.Close();

        return dsTipo;
    }
    public static DataSet dtsTipoR()
    {
        //Tipo de Documento a Receber
        SqlConnection myTipoR = new SqlConnection(conexao());
        myTipoR.Open();
        SqlDataAdapter myCommandTipoR = new SqlDataAdapter("SELECT TDCR.COD_ID, TDCR.NOM_TDCR FROM TDCR WHERE TDCR.FLG_ATIVO='S' AND TDCR.COD_ID_PESSOA_LICENCIADO = " + HttpContext.Current.Session["PESSOA"].ToString() + " ORDER BY NOM_TDCR", myTipoR);
        DataSet dsTipoR = new DataSet();
        myCommandTipoR.Fill(dsTipoR, "TDCR");
        myTipoR.Close();

        return dsTipoR;
    }

    public static DataSet dtsContas()
    {
        //Contas Bancárias
        SqlConnection myContas = new SqlConnection(conexao());
        myContas.Open();
        SqlDataAdapter myCommandContas = new SqlDataAdapter("SELECT CONTAS.COD_ID, 'Bco.:'+CAST(BANCOS.NUM_BANCO AS CHAR(3))+'/Ag.:'+RTRIM(CONTAS.NUM_AGENCIA)+'-'+RTRIM(CONTAS.NUM_DIG_AGENCIA)+'/Conta:'+RTRIM(CONTAS.NUM_CONTA)+'-'+RTRIM(CONTAS.NUM_DIG_CONTA)+'/'+RTRIM(CONTAS.NOM_CONTA) AS NOM_CONTA FROM CONTAS LEFT OUTER JOIN BANCOS ON (BANCOS.COD_ID = CONTAS.COD_ID_BANCO) WHERE CONTAS.FLG_ATIVO = 'S'  AND CONTAS.COD_ID_PESSOA_LICENCIADO = " + HttpContext.Current.Session["PESSOA"].ToString() + " ORDER BY 'Bco.:'+CAST(BANCOS.NUM_BANCO AS CHAR(3))+'/Ag.:'+RTRIM(CONTAS.NUM_AGENCIA)+'-'+RTRIM(CONTAS.NUM_DIG_AGENCIA)+'/Conta:'+RTRIM(CONTAS.NUM_CONTA)+'-'+RTRIM(CONTAS.NUM_DIG_CONTA)+'/'+RTRIM(CONTAS.NOM_CONTA)", myContas);
        DataSet dsContas = new DataSet();
        myCommandContas.Fill(dsContas, "CONTAS");
        myContas.Close();

        return dsContas;
    }

    public static DataSet dtsCFIN()
    {
        //Classif. Financeira
        SqlConnection myCFIN = new SqlConnection(conexao());
        myCFIN.Open();
        SqlDataAdapter myCommandCFIN = new SqlDataAdapter("SELECT CFIN.COD_ID, CFIN.NOM_CFIN FROM CFIN WHERE CFIN.FLG_ATIVO = 'S'  AND CFIN.COD_ID_PESSOA_LICENCIADO = " + HttpContext.Current.Session["PESSOA"].ToString() + " ORDER BY CFIN.NOM_CFIN", myCFIN);
        DataSet dsCFIN = new DataSet();
        myCommandCFIN.Fill(dsCFIN, "CFIN");
        myCFIN.Close();

        return dsCFIN;
    }

    public static DataSet dtsCCUSTO()
    {
        //Classif. Financeira
        SqlConnection myCCUSTO = new SqlConnection(conexao());
        myCCUSTO.Open();
        SqlDataAdapter myCommandCCUSTO = new SqlDataAdapter("SELECT CCUSTO.COD_ID, CCUSTO.NOM_CCUSTO FROM CCUSTO WHERE CCUSTO.FLG_ATIVO = 'S'  AND CCUSTO.COD_ID_PESSOA_LICENCIADO = " + HttpContext.Current.Session["PESSOA"].ToString() + " ORDER BY CCUSTO.NOM_CCUSTO", myCCUSTO);
        DataSet dsCCUSTO = new DataSet();
        myCommandCCUSTO.Fill(dsCCUSTO, "CCUSTO");
        myCCUSTO.Close();

        return dsCCUSTO;
    }
    public static double strToDouble(string str)
    {
        return (str == null || str == "") ? 0 : double.Parse(str);
    }

    public static string VerificaAcesso(int submenu, int usuario)
    {
        string sqlSelAcesso = "SELECT ISNULL(FLG_ACESSO,'N') AS FLG_ACESSO FROM SIS_USUARIO_ACESSOS WHERE COD_ID_SUBMENUS = " + submenu.ToString() + " AND COD_ID_USUARIO = " + usuario.ToString();
        SqlConnection mySelAcesso = new SqlConnection(conexao());
        mySelAcesso.Open();
        SqlCommand objSelAcessoCmd = new SqlCommand(sqlSelAcesso, mySelAcesso);
        SqlDataReader LeSelAcesso = objSelAcessoCmd.ExecuteReader();
        string Acesso = "N";
        while (LeSelAcesso.Read())
        {
            Acesso = LeSelAcesso["FLG_ACESSO"].ToString();
        }
        mySelAcesso.Close();
        mySelAcesso.Dispose();

        return Acesso;
    }

    public static string VerificaRestrito(int submenu, int usuario)
    {
        string sqlSelAcesso = "SELECT ISNULL(FLG_RESTRITO,'N') AS FLG_RESTRITO  FROM SIS_USUARIO_ACESSOS WHERE COD_ID_SUBMENUS = " + submenu.ToString() + " AND COD_ID_USUARIO = " + usuario.ToString();
        SqlConnection mySelAcesso = new SqlConnection(conexao());
        mySelAcesso.Open();
        SqlCommand objSelAcessoCmd = new SqlCommand(sqlSelAcesso, mySelAcesso);
        SqlDataReader LeSelAcesso = objSelAcessoCmd.ExecuteReader();
        string Restrito = "N";
        while (LeSelAcesso.Read())
        {
            Restrito = LeSelAcesso["FLG_RESTRITO"].ToString();

        }
        mySelAcesso.Close();
        mySelAcesso.Dispose();

        return Restrito;
    }
    public static string TIRAACENTOS(string str)
    {
        // Acento Agudo
        str = str.Replace('á', 'a');
        str = str.Replace('é', 'e');
        str = str.Replace('í', 'i');
        str = str.Replace('ó', 'o');
        str = str.Replace('ú', 'u');
        str = str.Replace('Á', 'A');
        str = str.Replace('É', 'E');
        str = str.Replace('Í', 'I');
        str = str.Replace('Ó', 'O');
        str = str.Replace('Ú', 'U');

        // Acento Til
        str = str.Replace('ã', 'a');
        str = str.Replace('õ', 'o');
        str = str.Replace('Ã', 'A');
        str = str.Replace('Õ', 'O');

        // Acento Craseado
        str = str.Replace('à', 'a');
        str = str.Replace('è', 'e');
        str = str.Replace('ì', 'i');
        str = str.Replace('ò', 'o');
        str = str.Replace('ù', 'u');
        str = str.Replace('À', 'A');
        str = str.Replace('È', 'E');
        str = str.Replace('Ì', 'I');
        str = str.Replace('Ò', 'O');
        str = str.Replace('Ù', 'U');

        // Acento Trema
        str = str.Replace('ä', 'a');
        str = str.Replace('ë', 'e');
        str = str.Replace('ï', 'i');
        str = str.Replace('ö', 'o');
        str = str.Replace('ü', 'u');
        str = str.Replace('Ä', 'A');
        str = str.Replace('Ë', 'E');
        str = str.Replace('Ï', 'I');
        str = str.Replace('Ö', 'O');
        str = str.Replace('Ü', 'U');

        // Diversos
        str = str.Replace('´', ' ');
        str = str.Replace('`', ' ');
        str = str.Replace('~', ' ');
        str = str.Replace('^', ' ');
        str = str.Replace('¨', ' ');
        str = str.Replace('´', ' ');
        str = str.Replace('°', 'o');
        str = str.Replace('º', 'o');
        str = str.Replace('ª', 'a');


        // Acentos Circunflex
        str = str.Replace('â', 'a');
        str = str.Replace('ê', 'e');
        str = str.Replace('î', 'i');
        str = str.Replace('ô', 'o');
        str = str.Replace('û', 'u');
        str = str.Replace('Â', 'A');
        str = str.Replace('Ê', 'E');
        str = str.Replace('Î', 'I');
        str = str.Replace('Ô', 'O');
        str = str.Replace('Û', 'U');

        // Cidilha
        str = str.Replace('ç', 'c');
        str = str.Replace('Ç', 'C');
        return str;
    }

    public static string ObterStringSemAcentosECaracteresEspeciais(string str)
    {
        /** Troca os caracteres acentuados por não acentuados **/
        string[] acentos = new string[] { "ç", "Ç", "á", "é", "í", "ó", "ú", "ý", "Á", "É", "Í", "Ó", "Ú", "Ý", "à", "è", "ì", "ò", "ù", "À", "È", "Ì", "Ò", "Ù", "ã", "õ", "ñ", "ä", "ë", "ï", "ö", "ü", "ÿ", "Ä", "Ë", "Ï", "Ö", "Ü", "Ã", "Õ", "Ñ", "â", "ê", "î", "ô", "û", "Â", "Ê", "Î", "Ô", "Û" };
        string[] semAcento = new string[] { "c", "C", "a", "e", "i", "o", "u", "y", "A", "E", "I", "O", "U", "Y", "a", "e", "i", "o", "u", "A", "E", "I", "O", "U", "a", "o", "n", "a", "e", "i", "o", "u", "y", "A", "E", "I", "O", "U", "A", "O", "N", "a", "e", "i", "o", "u", "A", "E", "I", "O", "U" };

        for (int i = 0; i < acentos.Length; i++)
        {
            str = str.Replace(acentos[i], semAcento[i]);
        }
        /** Troca os caracteres especiais da string por "" **/
        string[] caracteresEspeciais = { "¹", "²", "³", "£", "¢", "¬", "º", "¨", "\"", "'", ".", ",", "-", ":", "(", ")", "ª", "|", "\\\\", "°", "_", "@", "#", "!", "$", "%", "&", "*", ";", "/", "<", ">", "?", "[", "]", "{", "}", "=", "+", "§", "´", "`", "^", "~" };

        for (int i = 0; i < caracteresEspeciais.Length; i++)
        {
            str = str.Replace(caracteresEspeciais[i], "");
        }

        /** Troca os caracteres especiais da string por " " **/
        str = Regex.Replace(str, @"[^\w\.@-]", " ", RegexOptions.None);

        return str.Trim();
    }


    public static int MODULO11(string sequencia, int NumBase)
    {
        int intContador, intNumero, intTotalNumero = 0;
        int intMultiplicador, intResto, intresultado;
        string caracter;

        intMultiplicador = 2;

        for (intContador = sequencia.Length - 1; intContador >= 0; intContador--)
        {
            caracter = sequencia.Substring(intContador, 1);

            intNumero = Int32.Parse(caracter) * intMultiplicador;
            intTotalNumero = intTotalNumero + intNumero;

            intMultiplicador++;
            if (intMultiplicador > NumBase)
                intMultiplicador = 2;

        }

        intResto = intTotalNumero % 11;
        intresultado = 11 - intResto;

        if (intresultado == 10 || intresultado == 11)
            return 1;

        return intresultado;
    }

    public static void ALERTA(System.Web.UI.WebControls.Button Botao, string mensagem)
    {
        string Script = "window.alert('" + mensagem + "');";
        Botao.Attributes.Add("onclick", Script);
    }

    public static void CONFIRMA(System.Web.UI.WebControls.Button Botao, string mensagem)
    {
        string Script = " return window.confirm('" + mensagem + "');";
        Botao.Attributes.Add("onclick", Script);
    }

    public static void CONFIRMATXT(System.Web.UI.WebControls.TextBox TextBoxConfirma, string mensagem)
    {
        string Script = " return window.confirm('" + mensagem + "');";
        TextBoxConfirma.Attributes.Add("onchange", Script);
    }


    public static string PADL(string campo, char caracter, int casas)
    {
        return campo.Trim().PadLeft(casas, caracter);
    }

    public static string PADR(string campo, char caracter, int casas)
    {
        return campo.Trim().PadRight(casas, caracter);
    }

    public static string ESPACO(int casas)
    {
        string preenche = "";
        int intContador = 0;
        for (intContador = 1; intContador <= casas; intContador++)
        {
            preenche = preenche + " ";
        }
        return preenche;

    }

    public static void addBlurAtt(Control cntrl)
    {
        if (cntrl.Controls.Count > 0)
        {
            foreach (Control childControl in cntrl.Controls)
            {
                addBlurAtt(childControl);
            }
        }
        if (cntrl.GetType() == typeof(TextBox))
        {
            TextBox TempTextBox = (TextBox)cntrl;
            TempTextBox.Attributes.Add("onFocus", "DoFocus(this);");
            TempTextBox.Attributes.Add("onBlur", "DoBlur(this);");
        }
        if (cntrl.GetType() == typeof(ListBox))
        {
            ListBox TempTextBox = (ListBox)cntrl;
            TempTextBox.Attributes.Add("onFocus", "DoFocus(this);");
            TempTextBox.Attributes.Add("onBlur", "DoBlur(this);");
        }
    }

    public static void HookOnFocus(Control CurrentControl)
    {
        //checks if control is one of TextBox, DropDownList, ListBox or Button
        if ((CurrentControl is TextBox) ||
            (CurrentControl is DropDownList) ||
            (CurrentControl is ListBox) ||
            (CurrentControl is Button))
            //adds a script which saves active control on receiving focus 
            //in the hidden field __LASTFOCUS.
            addBlurAtt(CurrentControl);
        //				(CurrentControl as WebControl).Attributes.Add("onfocus",	"try{document.getElementById('__LASTFOCUS').value=this.id} 	catch(e) {}");
        //checks if the control has children
        if (CurrentControl.HasControls())
            //if yes do them all recursively
            foreach (Control CurrentChildControl in CurrentControl.Controls)
                HookOnFocus(CurrentChildControl);
    }

    public static string Formatar(string valor, string mascara)
    {
        StringBuilder dado = new StringBuilder();
        // remove caracteres nao numericos 
        foreach (char c in valor)
        {
            if (Char.IsNumber(c))
                dado.Append(c);
        }
        int indMascara = mascara.Length;
        int indCampo = dado.Length;
        for (; indCampo > 0 && indMascara > 0; )
        {
            if (mascara[--indMascara] == '#')
                indCampo--;
        }
        StringBuilder saida = new StringBuilder();
        for (; indMascara < mascara.Length; indMascara++)
            saida.Append((mascara[indMascara] == '#') ? dado[indCampo++] : mascara[indMascara]);
        return saida.ToString();
    }

    public static string parsetext(string text, bool allow)
    {
        //Create a StringBuilder object from the string intput
        //parameter
        StringBuilder sb = new StringBuilder(text);
        //Replace all double white spaces with a single white space
        //and &nbsp;
        sb.Replace("  ", " &nbsp;");
        //Check if HTML tags are not allowed
        if (!allow)
        {
            //Convert the brackets into HTML equivalents
            sb.Replace("<", "&lt;");
            sb.Replace(">", "&gt;");
            //Convert the double quote
            sb.Replace("\"", "&quot;");
        }
        //Create a StringReader from the processed string of 
        //the StringBuilder object
        StringReader sr = new StringReader(sb.ToString());
        StringWriter sw = new StringWriter();
        //Loop while next character exists
        while (sr.Peek() > -1)
        {
            //Read a line from the string and store it to a temp
            //variable
            string temp = sr.ReadLine();
            //write the string with the HTML break tag
            //Note here write method writes to a Internal StringBuilder
            //object created automatically
            sw.Write(temp + "<br>");
        }
        //Return the final processed text
        return sw.GetStringBuilder().ToString();
    }
    public static int WeekOfYear(DateTime date)
    {
        // use the current culture
        System.Globalization.CultureInfo ci = System.Threading.Thread.CurrentThread.CurrentCulture;
        // use current culture's calendar
        System.Globalization.Calendar cal = ci.Calendar;
        // get the calendar week rule (i.e. what determines the first week in the year)
        System.Globalization.CalendarWeekRule cwr = ci.DateTimeFormat.CalendarWeekRule;
        // get the first day of week for the current culture
        DayOfWeek fdow = ci.DateTimeFormat.FirstDayOfWeek;
        // return the week
        return cal.GetWeekOfYear(date, cwr, fdow);
    }

    public static DateTime GoToWeek(int year, int week)
    {
        // use the current culture
        System.Globalization.CultureInfo ci = System.Threading.Thread.CurrentThread.CurrentCulture;
        // get first day of week from culture
        DayOfWeek fdow = ci.DateTimeFormat.FirstDayOfWeek;

        // new empty Date (so starts 01/01/0001)
        DateTime d = new DateTime();

        // year starts at 1, so take away 1 from desired year to prevent going to the next one
        d = d.AddYears(year - 1);

        // get day January 1st falls on
        int startDay = (int)d.DayOfWeek;

        // get the difference between the first day of the week, and the day January 1st starts on
        int difference = (int)fdow - startDay;
        // if it is positive (i.e. after first day of week), take away 7
        if (difference > 0)
        {
            difference = difference - 7;
        }

        // already on week 1, so add desired number of weeks - 1 * days in week
        if (week > 1)
        {
            d = d.AddDays((week - 1) * 7 + difference);
        }

        return d;
    }


    public static DataSet ImportExcelXLS(string FileName, bool hasHeaders)
    {
        string HDR = hasHeaders ? "Yes" : "No";
        string strConn = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" +
                         FileName + ";Extended Properties=\"Excel 8.0;HDR=" +
                         HDR + ";IMEX=1\"";

        DataSet output = new DataSet();
        using (OleDbConnection conn = new OleDbConnection(strConn))
        {
            conn.Open();
            DataTable schemaTable = conn.GetOleDbSchemaTable(
              OleDbSchemaGuid.Tables, new object[] { null, null, null, "TABLE" });

            foreach (DataRow schemaRow in schemaTable.Rows)
            {
                string sheet = schemaRow["TABLE_NAME"].ToString();
                OleDbCommand cmd = new OleDbCommand("SELECT * FROM [" + sheet + "]", conn);

                cmd.CommandType = CommandType.Text;
                DataTable outputTable = new DataTable(sheet);
                output.Tables.Add(outputTable);
                new OleDbDataAdapter(cmd).Fill(outputTable);
            }
        }
        return output;
    }

    public static bool GravaAcesso(string rotina)
    {
        SqlConnection myGravar = new SqlConnection(conexao());
        myGravar.Open();
        SqlCommand cmdGravar = new SqlCommand("dbo.stp_acessos_ins", myGravar);
        cmdGravar.CommandType = CommandType.StoredProcedure;
        cmdGravar.Parameters.Add("@COD_ID_PESSOA_LICENCIADO", SqlDbType.Int).Value = strToInt(HttpContext.Current.Session["LICENCIADO"].ToString());
        cmdGravar.Parameters.Add("@COD_ID_PESSOAS_FJ", SqlDbType.Int).Value = strToInt(HttpContext.Current.Session["PESSOA"].ToString());
        cmdGravar.Parameters.Add("@COD_ID_USUARIO", SqlDbType.Int).Value = strToInt(HttpContext.Current.Session["CODIGO"].ToString());
        cmdGravar.Parameters.Add("@NOM_OPERACAO", SqlDbType.VarChar).Value = rotina.ToString();
        cmdGravar.Parameters.Add("@DTA_DATA", SqlDbType.DateTime).Value = DateTime.Now;
        cmdGravar.Parameters.Add("@flg_operacao", SqlDbType.Char).Value = "I";
        cmdGravar.ExecuteScalar().ToString();
        return true;
    }
}
