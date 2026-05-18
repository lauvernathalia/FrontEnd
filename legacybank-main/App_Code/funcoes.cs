using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.Adapters;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Data.SqlClient;
using System.Security.Cryptography;
using System.IO;
using System.Net;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using System.Drawing.Drawing2D;
using System.Drawing.Printing;
using System.Drawing.Imaging;
using System.Drawing.Text;

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

using System.Net.Sockets;

using System.Net.Mail;
using System.Net.Mime;
using System.Net.Configuration;

/// <summary>
/// Summary description for Class1
/// </summary>


public static class ProtocoloGenerator
{
    private static Random _random = new Random();

    public static string GerarProtocolo(int numeroCliente, string prefixo)
    {
        string mesAno = DateTime.Now.ToString("MMyy");
        string clienteFormatado = numeroCliente.ToString("D6"); // 6 dígitos
        string controleAleatorio = _random.Next(1000, 9999).ToString(); // 4 dígitos

        return string.Format("{0}-{1}-{2}-{3}", prefixo, mesAno, clienteFormatado, controleAleatorio);
    }
}


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
        SqlConnection connSelConf = new SqlConnection(Funcoes.conexao());
        SqlCommand cmdSelConf = new SqlCommand("dbo.stp_configuracoes_ins", connSelConf);
        cmdSelConf.CommandType = CommandType.StoredProcedure;
        cmdSelConf.Parameters.Add("@flg_operacao", SqlDbType.Char).Value = "S";
        cmdSelConf.Parameters.Add("@COD_ID_PESSOA_LICENCIADO", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["PESSOA"].ToString());

        connSelConf.Open();
        SqlDataReader Reader = cmdSelConf.ExecuteReader();
        string VerificaCliente = "N";
        while (Reader.Read())
        {
            VerificaCliente = Reader["FLG_VERIFICA_CLIENTE"].ToString();
        }
        return VerificaCliente.ToString();
    }

    public static bool CarregaPerfilComercial(string sTipo)
    {
        bool bAcesso = true;
        SqlConnection connSelConf = new SqlConnection(Funcoes.conexao());
        SqlCommand cmdSelConf = new SqlCommand("dbo.stp_pessoas_fj_perfil_comercial_ins", connSelConf);
        cmdSelConf.CommandType = CommandType.StoredProcedure;
        cmdSelConf.Parameters.Add("@flg_operacao", SqlDbType.Char).Value = "L";
        cmdSelConf.Parameters.Add("@COD_ID_PESSOAS_FJ", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["PESSOA"].ToString());
        cmdSelConf.Parameters.Add("@COD_ID_PESSOA_LICENCIADO", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["LICENCIADO"].ToString());

        connSelConf.Open();
        SqlDataReader Reader = cmdSelConf.ExecuteReader();
        while (Reader.Read())
        {
            if (sTipo.ToString().Trim() == "B" )
            {
                bAcesso = (Reader["FLG_BOLETO"].ToString().Trim() == "S") ? true : false;
            }
            if (sTipo.ToString().Trim() == "C")
            {
                bAcesso = (Reader["FLG_CREDITO"].ToString().Trim() == "S") ? true : false;
            }
            if (sTipo.ToString().Trim() == "X")
            {
                bAcesso= (Reader["FLG_PIX"].ToString().Trim() == "S")? true : false; 
            }
            if (sTipo.ToString().Trim() == "P")
            {
                bAcesso = (Reader["FLG_PARCELADO"].ToString().Trim() == "S") ? true : false;
            }
        }
        return bAcesso;
    }




    public static string RetornaStatusBaas(string sStatus)
    {
        string sRetorno = "";
        switch (sStatus)
        {
            case "PENDING":
                sRetorno = "Pendente";
                break;
            case "PAID":
                sRetorno = "Pago";
                break;
            case "CANCELLED":
                sRetorno = "Cancelado";
                break;
            case "REFUNDED":
                sRetorno = "Estornado";
                break;
            case "BANK_PROCESSING":
                sRetorno = "Enviado ao banco";
                break;
            case "FAILED":
                sRetorno = "Falhou";
                break;
            case "AWAITING_CHECKOUT_RISK_ANALYSIS_REQUEST":
                sRetorno = "Em análise";
                break;
            default:
                sRetorno = "Não Identificado";
                break;
        }
        return sRetorno.ToString();
    }


    public static string colorTween(string c1, string c2, int p)
    {
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


    public static string CoresWhitelabel(string urlorigem, int p)
    {
        string sCor = "";
        SqlConnection mySelCadastro = new SqlConnection(Funcoes.conexao());
        mySelCadastro.Open();
        SqlCommand cmdSelCadastro = new SqlCommand("dbo.stp_pessoas_fj_ins", mySelCadastro);
        cmdSelCadastro.CommandType = CommandType.StoredProcedure;
        cmdSelCadastro.Parameters.Add("@flg_operacao", SqlDbType.Char).Value = "T";
        cmdSelCadastro.Parameters.Add("@NOM_URL", SqlDbType.VarChar).Value = urlorigem.ToString();
        SqlDataReader ReaderCadastro = cmdSelCadastro.ExecuteReader();
        while (ReaderCadastro.Read())
        {
            if (p == 1) { sCor = ReaderCadastro["NOM_COR_PRIMARIA_FUNDO"].ToString(); }
            if (p == 2) { sCor = ReaderCadastro["NOM_COR_SECUNDARIA_FUNDO"].ToString(); }
        }
        return sCor;
    }

    public static string CarregaVerificaFornecedor()
    {
        SqlConnection connSelConf = new SqlConnection(Funcoes.conexao());
        SqlCommand cmdSelConf = new SqlCommand("dbo.stp_configuracoes_ins", connSelConf);
        cmdSelConf.CommandType = CommandType.StoredProcedure;
        cmdSelConf.Parameters.Add("@flg_operacao", SqlDbType.Char).Value = "S";
        cmdSelConf.Parameters.Add("@COD_ID_PESSOA_LICENCIADO", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["PESSOA"].ToString());

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
        SqlConnection connSelConf = new SqlConnection(Funcoes.conexao());
        SqlCommand cmdSelConf = new SqlCommand("dbo.stp_configuracoes_ins", connSelConf);
        cmdSelConf.CommandType = CommandType.StoredProcedure;
        cmdSelConf.Parameters.Add("@flg_operacao", SqlDbType.Char).Value = "S";
        cmdSelConf.Parameters.Add("@COD_ID_PESSOA_LICENCIADO", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["PESSOA"].ToString());

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
        SqlConnection mySelAcesso = new SqlConnection(Funcoes.conexao());
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
        SqlConnection connSelConf = new SqlConnection(Funcoes.conexao());
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


    public static bool IsEmail(string strEmail)
    {
        /*
        string strModelo = @"^[a-zA-Z0-9_.+-]+@[a-zA-Z0-9-]+\.[a-zA-Z]{2,9}$";
        //string strModelo = "^([0-9a-zA-Z]([-.\\w]*[0-9a-zA-Z])*@([0-9a-zA-Z][-\\w]*[0-9a-zA-Z]\\.)+[a-zA-Z]{2,9})$";
        if (System.Text.RegularExpressions.Regex.IsMatch(strEmail, strModelo))
        {
            return true;
        }
        else
        {
            return false;
        }
         * */
        string strModelo = @"^[a-zA-Z0-9_.+-]+@[a-zA-Z0-9-]+(\.[a-zA-Z0-9-]+)+$";
        return System.Text.RegularExpressions.Regex.IsMatch(strEmail, strModelo);
    }

    public static bool IsSenha(string strEmail)
    {
        string strModelo = "^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])(?=.*?[#?!@$%^&*-]).{6,32}$";
        //string strModelo = "^(?=.*[0-9])(?=.*[a-z])(?=.*[A-Z])[a-zA-Z0-9]+/g{6,32}$";
        if (System.Text.RegularExpressions.Regex.IsMatch(strEmail, strModelo))
        {
            return true;
        }
        else
        {
            return false;
        }
    }


    public static string VerificaPUBLICOPRIVADO(int submenu, int usuario)
    {
        string sqlSelAcesso = "SELECT ISNULL(FLG_PUBLICO_PRIVADO,'P') AS FLG_PUBLICO_PRIVADO FROM SIS_USUARIO_ACESSOS WHERE COD_ID_SUBMENUS = " + submenu.ToString() + " AND COD_ID_USUARIO = " + usuario.ToString();
        SqlConnection mySelAcesso = new SqlConnection(Funcoes.conexao());
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
        SqlConnection mySelAcesso = new SqlConnection(Funcoes.conexao());
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
        SqlConnection mySelAcesso = new SqlConnection(Funcoes.conexao());
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
        SqlConnection mySelAcesso = new SqlConnection(Funcoes.conexao());
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

    public static bool IsDoubleRealNumber(string valueToTest)
    {
        var stringNumber = valueToTest.ToString();
        double numericValue;
        bool isNumber = double.TryParse(stringNumber, out numericValue);
        if (isNumber)
        {
            return true;
        }
        return false;
    }

    public static bool exportarExcelRPT(Repeater rep, string saveAsFile)
    {
        HttpContext.Current.Response.Clear();
        HttpContext.Current.Response.Buffer = true;
        HttpContext.Current.Response.AddHeader("content-disposition", "attachment;filename=" + saveAsFile.ToString() + "");
        HttpContext.Current.Response.Charset = "UTF-8";
        HttpContext.Current.Response.ContentEncoding = System.Text.Encoding.GetEncoding("windows-1252");

        HttpContext.Current.Response.ContentType = "application/vnd.ms-excel";

        System.IO.StringWriter stringWrite = new System.IO.StringWriter();
        System.Web.UI.HtmlTextWriter htmlWrite = new HtmlTextWriter(stringWrite);
        //     Your Repeater Name Mine is "Rep"

        rep.RenderControl(htmlWrite);
        HttpContext.Current.Response.Write("<table>");
        HttpContext.Current.Response.Write(stringWrite.ToString());
        HttpContext.Current.Response.Write("</table>");
        HttpContext.Current.Response.End();


        //if (grid.Items.Count + 1 <= 65536)
        //{
        //HttpContext.Current.Response.Clear();
        //HttpContext.Current.Response.ContentType = "application/vnd.ms-excel";
        //HttpContext.Current.Response.AddHeader("content-disposition", "attachment;filename=" + saveAsFile + ".xls");

        //              Remover caracteres do header - Content-Type
        //HttpContext.Current.Response.Charset = "UTF-8";
        //HttpContext.Current.Response.ContentEncoding = System.Text.Encoding.GetEncoding("windows-1252");
        //HttpContext.Current.Response.Charset = "";

        //              desabilita o  view state.
        //grid.EnableViewState = false;
        //StringWriter tw = new System.IO.StringWriter();
        //HtmlTextWriter hw = new System.Web.UI.HtmlTextWriter(tw);
        //grid.RenderControl(hw);
        //              Escrever o html no navegador
        //HttpContext.Current.Response.Write(tw.ToString());
        //              Termina o response
        //HttpContext.Current.Response.End();
        //}
        //else
        //{
        //	HttpContext.Current.Response.Write("Muitas linhas para exportar para o Exel !!!");
        //}    
        return true;

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

    public static string alfanumericoAleatorio(int tamanho)
    {
        var chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789abcdefghijklmnopqrstuvxwyz";
        var random = new Random();
        var result = new string(
            Enumerable.Repeat(chars, tamanho)
                      .Select(s => s[random.Next(s.Length)])
                      .ToArray());
        return result;
    }

    public static string Codigo2FA(int tamanho)
    {
        var chars = "0123456789";
        var random = new Random();
        var result = new string(
            Enumerable.Repeat(chars, tamanho)
                      .Select(s => s[random.Next(s.Length)])
                      .ToArray());
        return result;
    }

    public static bool GravarOPeracoesBaas(string sOperacao, string sCodigo, string sStatus, string sOrigem, string sJson)
    {
        bool bRetorno = false;

        SqlConnection connInsCons = new SqlConnection(Funcoes.conexao());
        connInsCons.Open();
        SqlCommand cmdInsCons = new SqlCommand("dbo.stp_operacoes_baas_ins", connInsCons);
        cmdInsCons.CommandType = CommandType.StoredProcedure;
        cmdInsCons.Parameters.Add("@flg_operacao", SqlDbType.Char).Value = "I";
        cmdInsCons.Parameters.Add("@COD_ID_PESSOAS_FJ", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["PESSOA"].ToString());
        cmdInsCons.Parameters.Add("@COD_ID_PESSOA_LICENCIADO", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["LICENCIADO"].ToString());
        cmdInsCons.Parameters.Add("@DTA_DATA", SqlDbType.DateTime).Value = DateTime.Now;

        cmdInsCons.Parameters.Add("@NOM_OPERACAO", SqlDbType.VarChar).Value = sOperacao.ToString();
        cmdInsCons.Parameters.Add("@COD_OPERACAO", SqlDbType.VarChar).Value = sCodigo.ToString();
        cmdInsCons.Parameters.Add("@FLG_STATUS", SqlDbType.VarChar).Value = sStatus.ToString();
        cmdInsCons.Parameters.Add("@FLG_ORIGEM", SqlDbType.VarChar).Value = sOrigem.ToString();
        cmdInsCons.Parameters.Add("@DES_JSON", SqlDbType.Text).Value = sJson.ToString();
        
        cmdInsCons.ExecuteNonQuery();
        connInsCons.Close();
        connInsCons.Dispose();

        bRetorno = true;
        return bRetorno;
    }

    public static string DadosEstabelecimento(string sTipo)
    {
        string sRetorno = "";
        SqlConnection mySelCadastroEstabelecimento = new SqlConnection(Funcoes.conexao());
        mySelCadastroEstabelecimento.Open();
        SqlCommand cmdSelCadastroEstabelecimento = new SqlCommand("dbo.stp_pessoas_fj_ins", mySelCadastroEstabelecimento);
        cmdSelCadastroEstabelecimento.CommandType = CommandType.StoredProcedure;
        cmdSelCadastroEstabelecimento.Parameters.Add("@flg_operacao", SqlDbType.Char).Value = "S";
        cmdSelCadastroEstabelecimento.Parameters.Add("@COD_ID_PESSOA_LICENCIADO", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["LICENCIADO"].ToString());
        cmdSelCadastroEstabelecimento.Parameters.Add("@COD_ID", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["PESSOA"].ToString());
        SqlDataReader ReaderCadastroEstabelecimento = cmdSelCadastroEstabelecimento.ExecuteReader();
        while (ReaderCadastroEstabelecimento.Read())
        {
            if (sTipo.ToString().Trim() == "E")
            {
                if (ReaderCadastroEstabelecimento["NOM_EMAIL_EMPRESA"].ToString().Trim() != "")
                {
                    sRetorno = ReaderCadastroEstabelecimento["NOM_EMAIL_EMPRESA"].ToString();
                }
                else
                {
                    sRetorno = ReaderCadastroEstabelecimento["NOM_EMAIL"].ToString();
                }
            }

            if (sTipo.ToString().Trim() == "C")
            {
                sRetorno = ReaderCadastroEstabelecimento["NOM_CELULAR"].ToString();
            }
        }
        return sRetorno.ToString();
    }

    public static string DadosIntegracaoNome(string sIntegracao, string sTipo)
    {
        string sRetorno = "";

        SqlConnection mySelCadastroIntegracao = new SqlConnection(Funcoes.conexao());
        mySelCadastroIntegracao.Open();
        SqlCommand cmdSelCadastroIntegracao = new SqlCommand("dbo.stp_pessoas_fj_integracoes_ins", mySelCadastroIntegracao);
        cmdSelCadastroIntegracao.CommandType = CommandType.StoredProcedure;
        cmdSelCadastroIntegracao.Parameters.Add("@flg_operacao", SqlDbType.Char).Value = "N";
        cmdSelCadastroIntegracao.Parameters.Add("@COD_ID_PESSOA_LICENCIADO", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["LICENCIADO"].ToString());
        cmdSelCadastroIntegracao.Parameters.Add("@FLG_ACESSO", SqlDbType.Char).Value = "S";
        cmdSelCadastroIntegracao.Parameters.Add("@FLG_INTEGRACAO", SqlDbType.Char).Value = sIntegracao.ToString();

        SqlDataReader ReaderCadastroIntegracao = cmdSelCadastroIntegracao.ExecuteReader();
        while (ReaderCadastroIntegracao.Read())
        {
            sRetorno = ReaderCadastroIntegracao["NOM_FANTASIA_INTEGRACAO"].ToString();
        }
        return sRetorno.ToString();
    }

    public static bool DadosIntegracaoVisivel(string sIntegracao, string sTipo)
    {
        bool sRetorno = false;

        SqlConnection mySelCadastroIntegracao = new SqlConnection(Funcoes.conexao());
        mySelCadastroIntegracao.Open();
        SqlCommand cmdSelCadastroIntegracao = new SqlCommand("dbo.stp_pessoas_fj_integracoes_ins", mySelCadastroIntegracao);
        cmdSelCadastroIntegracao.CommandType = CommandType.StoredProcedure;
        cmdSelCadastroIntegracao.Parameters.Add("@flg_operacao", SqlDbType.Char).Value = "N";
        cmdSelCadastroIntegracao.Parameters.Add("@COD_ID_PESSOA_LICENCIADO", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["LICENCIADO"].ToString());
        cmdSelCadastroIntegracao.Parameters.Add("@FLG_ACESSO", SqlDbType.Char).Value = "S";
        cmdSelCadastroIntegracao.Parameters.Add("@FLG_INTEGRACAO", SqlDbType.Char).Value = sIntegracao.ToString();

        SqlDataReader ReaderCadastroIntegracao = cmdSelCadastroIntegracao.ExecuteReader();
        while (ReaderCadastroIntegracao.Read())
        {
                sRetorno = true;
        }
        return sRetorno;
    }


    public static bool Enviar2fa()
    {
        string s2FA = "";
        bool bRetorno = false;
        try
        {
            // Grava os dados

            s2FA = Funcoes.Codigo2FA(6).ToString();

            // Grava no arquivo 2fa
            SqlConnection connInsCons2FA = new SqlConnection(Funcoes.conexao());
            connInsCons2FA.Open();
            SqlCommand cmdInsCons2FA = new SqlCommand("dbo.stp_2fa_ins", connInsCons2FA);
            cmdInsCons2FA.CommandType = CommandType.StoredProcedure;
            cmdInsCons2FA.Parameters.Add("@flg_operacao", SqlDbType.Char).Value = "I";
            cmdInsCons2FA.Parameters.Add("@COD_ID_SIS_USUARIO", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["CODIGO"].ToString());
            cmdInsCons2FA.Parameters.Add("@COD_ID_PESSOA_LICENCIADO", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["LICENCIADO"].ToString());
            cmdInsCons2FA.Parameters.Add("@DTA_DATA", SqlDbType.DateTime).Value = DateTime.Now;
            cmdInsCons2FA.Parameters.Add("@DTA_EXPIRACAO", SqlDbType.DateTime).Value = DateTime.Now.AddMinutes(5);
            cmdInsCons2FA.Parameters.Add("@NOM_2FA", SqlDbType.VarChar).Value = Funcoes.Encrypt(s2FA.ToString());
            cmdInsCons2FA.ExecuteNonQuery();
            connInsCons2FA.Close();
            connInsCons2FA.Dispose();

            // Envia Email

            System.Net.Mail.SmtpClient client = new System.Net.Mail.SmtpClient();
            client.Host = HttpContext.Current.Session["EMAILHOST"].ToString();
            if (HttpContext.Current.Session["EMAILSSL"].ToString() == "S")
            { client.EnableSsl = true; }
            else { client.EnableSsl = false; }
            client.Port = Funcoes.strToInt(HttpContext.Current.Session["EMAILPORTA"].ToString());
            //client.Timeout = 0;
            client.Credentials = new System.Net.NetworkCredential(HttpContext.Current.Session["EMAIL"].ToString(), HttpContext.Current.Session["EMAILSENHA"].ToString());
            MailMessage mail = new MailMessage();
            mail.Sender = new System.Net.Mail.MailAddress(HttpContext.Current.Session["EMAIL"].ToString(), "");
            mail.From = new MailAddress(HttpContext.Current.Session["EMAIL"].ToString(), "");

            if (HttpContext.Current.Session["TIPO"].ToString().Trim() == "L")
            {
                mail.To.Add(new MailAddress(HttpContext.Current.Session["EMAIL"].ToString(), HttpContext.Current.Session["EMAIL"].ToString()));
            }
            else
            {
                mail.To.Add(new MailAddress(Funcoes.Decrypt(HttpContext.Current.Session["LOGIN"].ToString().Trim()), Funcoes.Decrypt(HttpContext.Current.Session["LOGIN"].ToString().Trim())));
            }

            mail.Bcc.Add(new MailAddress("adriano@webview.com.br", "Adriano - WEBVIEW"));
            mail.Bcc.Add(new MailAddress("admin@legacybank.com.br", "Admin - LEGACY"));

            mail.Subject = "Código Confirmação da Autenticação de Dois Fatores - " + HttpContext.Current.Session["URLORIGEM"].ToString();
            mail.SubjectEncoding = System.Text.Encoding.UTF8;

            // Construção do CORPO da MENSAGEM (Body)
            mail.Body = "";
            mail.Body = mail.Body + "<img src='https://" + HttpContext.Current.Session["URLORIGEM"].ToString() + "/public_html/" + HttpContext.Current.Session["EMAILLOGOTIPO"].ToString() + "' alt='Logotipo' width='150'><br /><br>";
            mail.Body = mail.Body + "<b>Olá!</b><br /><br />";
            mail.Body = mail.Body + "Segue abaixo o código de confirmação da Autenticação de Dois Fatores.<br />";
            mail.Body = mail.Body + "Para prosseguir, digite este código na tela de confirmação para confirmar a operação.<br /><br>";

            mail.Body = mail.Body + "<a>" + s2FA.ToString() + "</a><br /><br>";

            mail.Body = mail.Body + "Este e-mail é apenas informativo e não requer resposta ou ação.<br />";
            mail.Body = mail.Body + "Caso tenha dúvidas ou precise de suporte, entre em contato pelos canais habituais.<br /><br />";

            mail.Body = mail.Body + HttpContext.Current.Session["EMAILRODAPE"].ToString();

            mail.BodyEncoding = System.Text.Encoding.UTF8;
            mail.IsBodyHtml = true;
            mail.Priority = MailPriority.High;
            try
            {
                client.Send(mail);
                bRetorno = true;
            }
            catch (System.Exception erro)
            {
                bRetorno = false;
            }
            finally
            {
                mail = null;
            }
        }
        catch
        {
            bRetorno = false;
        }
        return bRetorno;

    }

    public static bool Enviar2faCadastro()
    {
        string s2FA = "";
        bool bRetorno = false;
        try
        {
            // Grava os dados

            s2FA = Funcoes.Codigo2FA(6).ToString();

            // Grava no arquivo 2fa
            SqlConnection connInsCons2FA = new SqlConnection(Funcoes.conexao());
            connInsCons2FA.Open();
            SqlCommand cmdInsCons2FA = new SqlCommand("dbo.stp_2fa_ins", connInsCons2FA);
            cmdInsCons2FA.CommandType = CommandType.StoredProcedure;
            cmdInsCons2FA.Parameters.Add("@flg_operacao", SqlDbType.Char).Value = "I";
            cmdInsCons2FA.Parameters.Add("@COD_ID_SIS_USUARIO", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["CODIGO"].ToString());
            cmdInsCons2FA.Parameters.Add("@COD_ID_PESSOA_LICENCIADO", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["LICENCIADO"].ToString());
            cmdInsCons2FA.Parameters.Add("@DTA_DATA", SqlDbType.DateTime).Value = DateTime.Now;
            cmdInsCons2FA.Parameters.Add("@DTA_EXPIRACAO", SqlDbType.DateTime).Value = DateTime.Now.AddMinutes(5);
            cmdInsCons2FA.Parameters.Add("@NOM_2FA", SqlDbType.VarChar).Value = Funcoes.Encrypt(s2FA.ToString());
            cmdInsCons2FA.ExecuteNonQuery();
            connInsCons2FA.Close();
            connInsCons2FA.Dispose();

            // Envia Email

            string urlorigem = HttpContext.Current.Request.ServerVariables["SERVER_NAME"].ToString();
            SqlConnection mySelCadastro = new SqlConnection(Funcoes.conexao());
            mySelCadastro.Open();
            SqlCommand cmdSelCadastro = new SqlCommand("dbo.stp_pessoas_fj_ins", mySelCadastro);
            cmdSelCadastro.CommandType = CommandType.StoredProcedure;
            cmdSelCadastro.Parameters.Add("@flg_operacao", SqlDbType.Char).Value = "T";
            cmdSelCadastro.Parameters.Add("@NOM_URL", SqlDbType.VarChar).Value = urlorigem.ToString();
            SqlDataReader ReaderCadastro = cmdSelCadastro.ExecuteReader();

            while (ReaderCadastro.Read())
            {

                System.Net.Mail.SmtpClient client = new System.Net.Mail.SmtpClient();
                client.Host = ReaderCadastro["NOM_HOST_EMAIL_PADRAO"].ToString();
                if (ReaderCadastro["FLG_SSL_EMAIL_PADRAO"].ToString() == "S")
                { client.EnableSsl = true; }
                else { client.EnableSsl = false; }
                client.Port = Funcoes.strToInt(ReaderCadastro["NUM_PORTA_EMAIL_PADRAO"].ToString());
                //client.Timeout = 0;
                client.Credentials = new System.Net.NetworkCredential(ReaderCadastro["NOM_EMAIL_PADRAO"].ToString(), ReaderCadastro["NOM_SENHA_EMAIL_PADRAO"].ToString());
                MailMessage mail = new MailMessage();
                mail.Sender = new System.Net.Mail.MailAddress(ReaderCadastro["NOM_EMAIL_PADRAO"].ToString(), "");
                mail.From = new MailAddress(ReaderCadastro["NOM_EMAIL_PADRAO"].ToString(), "");

                if (HttpContext.Current.Session["TIPO"].ToString().Trim() == "L")
                {
                    mail.To.Add(new MailAddress(ReaderCadastro["NOM_EMAIL_PADRAO"].ToString(), ReaderCadastro["NOM_EMAIL_PADRAO"].ToString()));
                }
                else
                {
                    mail.To.Add(new MailAddress(Funcoes.Decrypt(HttpContext.Current.Session["LOGIN"].ToString().Trim()), Funcoes.Decrypt(HttpContext.Current.Session["LOGIN"].ToString().Trim())));
                }

                mail.Bcc.Add(new MailAddress("adriano@webview.com.br", "Adriano - WEBVIEW"));
                mail.Bcc.Add(new MailAddress("admin@legacybank.com.br", "Admin - LEGACY"));

                mail.Subject = "Código Confirmação da Autenticação de Dois Fatores - " + urlorigem;
                mail.SubjectEncoding = System.Text.Encoding.UTF8;

                // Construção do CORPO da MENSAGEM (Body)
                mail.Body = "";
                mail.Body = mail.Body + "<img src='https://" + urlorigem + "/public_html/" + ReaderCadastro["NOM_LOGOTIPO_EMAIL"].ToString() + "' alt='Logotipo' width='150'><br /><br>";
                mail.Body = mail.Body + "<b>Olá!</b><br /><br />";
                mail.Body = mail.Body + "Segue abaixo o código de confirmação da Autenticação de Dois Fatores.<br />";
                mail.Body = mail.Body + "Para prosseguir, digite este código na tela de confirmação para confirmar a operação.<br /><br>";

                mail.Body = mail.Body + "<a>" + s2FA.ToString() + "</a><br /><br>";

                mail.Body = mail.Body + "Este e-mail é apenas informativo e não requer resposta ou ação.<br />";
                mail.Body = mail.Body + "Caso tenha dúvidas ou precise de suporte, entre em contato pelos canais habituais.<br /><br />";

                mail.Body = mail.Body + ReaderCadastro["NOM_RODAPE_EMAIL"].ToString();

                mail.BodyEncoding = System.Text.Encoding.UTF8;
                mail.IsBodyHtml = true;
                mail.Priority = MailPriority.High;
                try
                {
                    client.Send(mail);
                    bRetorno = true;
                }
                catch (System.Exception erro)
                {
                    bRetorno = false;
                }
                finally
                {
                    mail = null;
                }
            }
        }
        catch
        {
            bRetorno = false;
        }
    
        return bRetorno;
    
    }

    public static string OcultarCaracteresEmail(string sEmail)
    {
        string sRetorno = "";
        int iTamanho = sEmail.ToString().Trim().Length;

        if (iTamanho >= 7)
        {
            string s1 = sEmail.ToString().Trim().Substring(0,3);
            string s2 = ("*").PadLeft(iTamanho - 6, '*');

            string s3 = sEmail.ToString().Trim().Substring(iTamanho-3, 3);
            sRetorno = s1 + s2 + s3;
        }
        else
        {
            sRetorno = sEmail.ToString();
        }

        return sRetorno.ToString();
    }


    public static bool Enviar2faEstabelecimento()
    {
        string s2FA = "";
        bool bRetorno = false;
        try
        {
            // Grava os dados

            s2FA = Funcoes.Codigo2FA(6).ToString();

            // Grava no arquivo 2fa
            SqlConnection connInsCons2FA = new SqlConnection(Funcoes.conexao());
            connInsCons2FA.Open();
            SqlCommand cmdInsCons2FA = new SqlCommand("dbo.stp_2fa_ins", connInsCons2FA);
            cmdInsCons2FA.CommandType = CommandType.StoredProcedure;
            cmdInsCons2FA.Parameters.Add("@flg_operacao", SqlDbType.Char).Value = "I";
            cmdInsCons2FA.Parameters.Add("@COD_ID_SIS_USUARIO", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["CODIGO"].ToString());
            cmdInsCons2FA.Parameters.Add("@COD_ID_PESSOA_LICENCIADO", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["LICENCIADO"].ToString());
            cmdInsCons2FA.Parameters.Add("@DTA_DATA", SqlDbType.DateTime).Value = DateTime.Now;
            cmdInsCons2FA.Parameters.Add("@DTA_EXPIRACAO", SqlDbType.DateTime).Value = DateTime.Now.AddMinutes(5);
            cmdInsCons2FA.Parameters.Add("@NOM_2FA", SqlDbType.VarChar).Value = Funcoes.Encrypt(s2FA.ToString());
            cmdInsCons2FA.ExecuteNonQuery();
            connInsCons2FA.Close();
            connInsCons2FA.Dispose();

            string urlorigem = HttpContext.Current.Request.ServerVariables["SERVER_NAME"].ToString();
            SqlConnection mySelCadastro = new SqlConnection(Funcoes.conexao());
            mySelCadastro.Open();
            SqlCommand cmdSelCadastro = new SqlCommand("dbo.stp_pessoas_fj_ins", mySelCadastro);
            cmdSelCadastro.CommandType = CommandType.StoredProcedure;
            cmdSelCadastro.Parameters.Add("@flg_operacao", SqlDbType.Char).Value = "T";
            cmdSelCadastro.Parameters.Add("@NOM_URL", SqlDbType.VarChar).Value = urlorigem.ToString();
            SqlDataReader ReaderCadastro = cmdSelCadastro.ExecuteReader();

            while (ReaderCadastro.Read())
            {

                System.Net.Mail.SmtpClient client = new System.Net.Mail.SmtpClient();
                client.Host = ReaderCadastro["NOM_HOST_EMAIL_PADRAO"].ToString();
                if (ReaderCadastro["FLG_SSL_EMAIL_PADRAO"].ToString() == "S")
                { client.EnableSsl = true; }
                else { client.EnableSsl = false; }
                client.Port = Funcoes.strToInt(ReaderCadastro["NUM_PORTA_EMAIL_PADRAO"].ToString());
                //client.Timeout = 0;
                client.Credentials = new System.Net.NetworkCredential(ReaderCadastro["NOM_EMAIL_PADRAO"].ToString(), ReaderCadastro["NOM_SENHA_EMAIL_PADRAO"].ToString());
                MailMessage mail = new MailMessage();
                mail.Sender = new System.Net.Mail.MailAddress(ReaderCadastro["NOM_EMAIL_PADRAO"].ToString(), "");
                mail.From = new MailAddress(ReaderCadastro["NOM_EMAIL_PADRAO"].ToString(), "");

                if (HttpContext.Current.Session["TIPO"].ToString().Trim() == "L")
                {
                    mail.To.Add(new MailAddress(ReaderCadastro["NOM_EMAIL_PADRAO"].ToString(), ReaderCadastro["NOM_EMAIL_PADRAO"].ToString()));
                }
                else
                {
                    mail.To.Add(new MailAddress(Funcoes.Decrypt(HttpContext.Current.Session["LOGIN"].ToString().Trim()), Funcoes.Decrypt(HttpContext.Current.Session["LOGIN"].ToString().Trim())));
                }

                string sEmailEstabelecimento = Funcoes.DadosEstabelecimento("E");
                mail.To.Add(new MailAddress(sEmailEstabelecimento, sEmailEstabelecimento));

                mail.Bcc.Add(new MailAddress("webview.solucoes@gmail.com", "Adriano - WEBVIEW"));
                mail.Bcc.Add(new MailAddress("admin@legacybank.com.br", "Admin - LEGACY"));

                mail.Subject = "Código Confirmação da Autenticação de Dois Fatores - " + HttpContext.Current.Session["URLORIGEM"].ToString();
                mail.SubjectEncoding = System.Text.Encoding.UTF8;

                // Construção do CORPO da MENSAGEM (Body)
                mail.Body = "";
                mail.Body = mail.Body + "<img src='https://" + HttpContext.Current.Session["URLORIGEM"].ToString() + "/public_html/" + HttpContext.Current.Session["EMAILLOGOTIPO"].ToString() + "' alt='Logotipo' width='150'><br /><br>";
                mail.Body = mail.Body + "<b>Olá!</b><br /><br />";
                mail.Body = mail.Body + "Segue abaixo o código de confirmação da Autenticação de Dois Fatores.<br />";
                mail.Body = mail.Body + "Para prosseguir, digite este código na tela de confirmação para confirmar a operação.<br /><br>";

                mail.Body = mail.Body + "<a>" + s2FA.ToString() + "</a><br /><br>";

                mail.Body = mail.Body + "Este e-mail é apenas informativo e não requer resposta ou ação.<br />";
                mail.Body = mail.Body + "Caso tenha dúvidas ou precise de suporte, entre em contato pelos canais habituais.<br /><br />";

                mail.Body = mail.Body + HttpContext.Current.Session["EMAILRODAPE"].ToString();

                mail.BodyEncoding = System.Text.Encoding.UTF8;
                mail.IsBodyHtml = true;
                mail.Priority = MailPriority.High;
                try
                {
                    client.Send(mail);
                    bRetorno = true;
                }
                catch (System.Exception erro)
                {
                    bRetorno = false;
                }
                finally
                {
                    mail = null;
                }
            }
        }
        catch
        {
            bRetorno = false;
        }
        return bRetorno;

    }
    
    
    public static string conexaoAzure()
    {
        string sconexao = ConfigurationManager.AppSettings["userAzure"].ToString() + ";" +
            ConfigurationManager.AppSettings["passwordAzure"].ToString() + ";" +
            ConfigurationManager.AppSettings["datasourceAzure"].ToString() + ";" +
            ConfigurationManager.AppSettings["basecatalogAzure"].ToString();
        return sconexao;
    }

    public static string conexaoApi()
    {
        string sconexao = ConfigurationManager.AppSettings["userApi"].ToString() + ";" +
            ConfigurationManager.AppSettings["passwordApi"].ToString() + ";" +
            ConfigurationManager.AppSettings["datasourceApi"].ToString() + ";" +
            ConfigurationManager.AppSettings["basecatalogApi"].ToString();
        return sconexao;
    }

    public static string conexaoMySql()
    {

        string sqlSelAcessoMySql = "SELECT NOM_CONEXAO_PORTAL FROM CONFIGURACOES WHERE COD_ID_PESSOA_LICENCIADO = " + HttpContext.Current.Session["PESSOA"].ToString();
        SqlConnection mySelAcessoMySql = new SqlConnection(Funcoes.conexao());
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
        SqlConnection myTipo = new SqlConnection(Funcoes.conexao());
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
        SqlConnection myTipoR = new SqlConnection(Funcoes.conexao());
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
        SqlConnection myContas = new SqlConnection(Funcoes.conexao());
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
        SqlConnection myCFIN = new SqlConnection(Funcoes.conexao());
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
        SqlConnection myCCUSTO = new SqlConnection(Funcoes.conexao());
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
        SqlConnection mySelAcesso = new SqlConnection(Funcoes.conexao());
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
        SqlConnection mySelAcesso = new SqlConnection(Funcoes.conexao());
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

    public static string TIRAACENTOSDOCUMENTOS(string str)
    {
        str = str.Replace("-", "");
        str = str.Replace(".", "");
        str = str.Replace("/", "");
        str = str.Replace("(", "");
        str = str.Replace(")", "");
        str = str.Replace(" ", "");
        return str;
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
                         FileName + ";Extended Properties=\"Excel 12.0;HDR=" +
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
                OleDbCommand cmd = new OleDbCommand("SELECT TOP 7000 * FROM [" + sheet + "]", conn);

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
        SqlConnection myGravar = new SqlConnection(Funcoes.conexao());
        myGravar.Open();
        SqlCommand cmdGravar = new SqlCommand("dbo.stp_acessos_ins", myGravar);
        cmdGravar.CommandType = CommandType.StoredProcedure;
        cmdGravar.Parameters.Add("@COD_ID_PESSOA_LICENCIADO", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["LICENCIADO"].ToString());
        cmdGravar.Parameters.Add("@COD_ID_PESSOAS_FJ", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["PESSOA"].ToString());
        cmdGravar.Parameters.Add("@COD_ID_USUARIO", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["CODIGO"].ToString());
        cmdGravar.Parameters.Add("@NOM_OPERACAO", SqlDbType.VarChar).Value = rotina.ToString();
        cmdGravar.Parameters.Add("@DTA_DATA", SqlDbType.DateTime).Value = DateTime.Now;
        cmdGravar.Parameters.Add("@flg_operacao", SqlDbType.Char).Value = "I";
        cmdGravar.ExecuteScalar().ToString();
        return true;
    }

    public static bool GravaAuditoriaNovo(int iID, string sIP, string sAuditoria, string sDescricao, int Licenciado, int Pessoa, int Usuario)
    {
        SqlConnection myGravar = new SqlConnection(Funcoes.conexao());
        myGravar.Open();
        SqlCommand cmdGravar = new SqlCommand("dbo.stp_auditoria_ins", myGravar);
        cmdGravar.CommandType = CommandType.StoredProcedure;
        cmdGravar.Parameters.Add("@COD_ID_PESSOA_LICENCIADO", SqlDbType.Int).Value = Licenciado;
        cmdGravar.Parameters.Add("@COD_ID_PESSOAS_FJ", SqlDbType.Int).Value = Pessoa;
        cmdGravar.Parameters.Add("@COD_ID_USUARIO", SqlDbType.Int).Value = Usuario;

        cmdGravar.Parameters.Add("@COD_IDENTIFICACAO", SqlDbType.Int).Value = iID;

        cmdGravar.Parameters.Add("@DTA_DATA", SqlDbType.DateTime).Value = DateTime.Now;


        cmdGravar.Parameters.Add("@NOM_AUDITORIA", SqlDbType.VarChar).Value = sAuditoria.ToString();
        cmdGravar.Parameters.Add("@DES_AUDITORIA", SqlDbType.VarChar).Value = sDescricao.ToString();
        cmdGravar.Parameters.Add("@NUM_IP", SqlDbType.VarChar).Value = sIP.ToString();

        cmdGravar.Parameters.Add("@flg_operacao", SqlDbType.Char).Value = "I";
        cmdGravar.ExecuteScalar().ToString();
        return true;
    }


    public static bool GravaAuditoria(int iID, string sIP, string sAuditoria, string sDescricao)
    {
        SqlConnection myGravar = new SqlConnection(Funcoes.conexao());
        myGravar.Open();
        SqlCommand cmdGravar = new SqlCommand("dbo.stp_auditoria_ins", myGravar);
        cmdGravar.CommandType = CommandType.StoredProcedure;
        cmdGravar.Parameters.Add("@COD_ID_PESSOA_LICENCIADO", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["LICENCIADO"].ToString());
        cmdGravar.Parameters.Add("@COD_ID_PESSOAS_FJ", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["PESSOA"].ToString());
        cmdGravar.Parameters.Add("@COD_ID_USUARIO", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["CODIGO"].ToString());

        cmdGravar.Parameters.Add("@COD_IDENTIFICACAO", SqlDbType.Int).Value = iID;

        cmdGravar.Parameters.Add("@DTA_DATA", SqlDbType.DateTime).Value = DateTime.Now;


        cmdGravar.Parameters.Add("@NOM_AUDITORIA", SqlDbType.VarChar).Value = sAuditoria.ToString();
        cmdGravar.Parameters.Add("@DES_AUDITORIA", SqlDbType.VarChar).Value = sDescricao.ToString();
        cmdGravar.Parameters.Add("@NUM_IP", SqlDbType.VarChar).Value = sIP.ToString();

        cmdGravar.Parameters.Add("@flg_operacao", SqlDbType.Char).Value = "I";
        cmdGravar.ExecuteScalar().ToString();
        return true;
    }


    public static string GetRandomPassword(int length)
    {
        const string chars = "0123456789abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ";

        StringBuilder sb = new StringBuilder();
        Random rnd = new Random();

        for (int i = 0; i < length; i++)
        {
            int index = rnd.Next(chars.Length);
            sb.Append(chars[index]);
        }

        return sb.ToString();
    }

    public static string Encrypt(string clearText)
    {
        var inputBytes = Encoding.Unicode.GetBytes(clearText);
        var outputBytes = AesConvert(inputBytes, aes => aes.CreateEncryptor());
        return Convert.ToBase64String(outputBytes);
    }

    public static string Decrypt(string cipherText)
    {
        var inputBytes = Convert.FromBase64String(cipherText.Replace(" ", "+"));
        var outputBytes = AesConvert(inputBytes, aes => aes.CreateDecryptor());
        return Encoding.Unicode.GetString(outputBytes);
    }

    private static byte[] AesConvert(byte[] inputBytes, Func<Aes, ICryptoTransform> convert)
    {
        var aes = Aes.Create();
        var key = "populateThisFromAppSettings";
        var derivedBytes = new Rfc2898DeriveBytes(key, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
        aes.Key = derivedBytes.GetBytes(32);
        aes.IV = derivedBytes.GetBytes(16);
        var ms = new MemoryStream();
        var cs = new CryptoStream(ms, convert(aes), CryptoStreamMode.Write);
        cs.Write(inputBytes, 0, inputBytes.Length);
        cs.Close();
        return ms.ToArray();
    }

    public static string NomeSobrenome(string NomeCompleto, string Tipo)
    {
        string sData = "";

        string nome = NomeCompleto;
        string primeiro = "";
        string meio = " "; // Sim, tem um espaço aqui!
        string ultimo = "";

        string[] nomes = nome.Split(' '); // Separa cada nome pelo espaço.

        primeiro = nomes[0]; // Reserva o primeiro nome.

        for (int i = 1; i < nomes.Length - 1; i++)
        {
            if (!nomes[i].ToLower().Equals("de") && !nomes[i].ToLower().Equals("da") && !nomes[i].ToLower().Equals("do") && !nomes[i].ToLower().Equals("das") && !nomes[i].ToLower().Equals("dos"))
            {
                meio += nomes[i]; // Reserva a inicial do próximo nome.
                meio += " "; // Põe um ponto e um espaço após a inicial.
            }
        }

        ultimo = nomes[nomes.Length - 1]; // Reserva o ultimo nome.

        if (Tipo == "P")
        {
            sData = primeiro + meio;
        }
        if (Tipo == "U")
        {
            sData = ultimo;
        }
        
        return sData;
    }


    public string Cores(int cor)
    {
        string urlorigem = HttpContext.Current.Session["REMOTEADDR"].ToString();

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
            if (cor == 1) { CorPrimaria = ReaderCadastro["NOM_COR_PRIMARIA_FUNDO"].ToString(); }
            if (cor == 2) { CorPrimaria = ReaderCadastro["NOM_COR_SECUNDARIA_FUNDO"].ToString(); }

        }
        return CorPrimaria;
    }

    public class TokenValidator
    {
        private const int TOKEN_DURATION = 30; // segundos
        private const int TOKEN_LENGTH = 6;
        private const string SECRET = "minha-chave-super-secreta";  // aqui teria que pegar os dados unicos de acesso deste usuario

        public static bool IsValidToken(string token)
        {
            long currentTimeBlock = GetTimeBlock(DateTime.UtcNow);
            return token == GenerateToken(currentTimeBlock) || token == GenerateToken(currentTimeBlock - 1);
        }

        public static string GetCurrentToken()
        {
            long currentTimeBlock = GetTimeBlock(DateTime.UtcNow);
            return GenerateToken(currentTimeBlock);
        }

        private static long GetTimeBlock(DateTime dateTime)
        {
            return (long)(dateTime - new DateTime(1970, 1, 1)).TotalSeconds / TOKEN_DURATION;
        }

        private static string GenerateToken(long timeBlock)
        {
            string data = SECRET + "-" + timeBlock.ToString(); // C# 4.7.2 não suporta interpolação de string segura

            using (SHA256 sha = SHA256.Create())
            {
                byte[] hashBytes = sha.ComputeHash(Encoding.UTF8.GetBytes(data));
                string hash = BitConverter.ToString(hashBytes).Replace("-", "").ToLower();

                string numeric = Regex.Replace(hash, @"\D", "");
                if (numeric.Length < TOKEN_LENGTH)
                    numeric = numeric.PadRight(TOKEN_LENGTH, '0');

                return numeric.Substring(numeric.Length - TOKEN_LENGTH, TOKEN_LENGTH);
            }
        }
    }
}
