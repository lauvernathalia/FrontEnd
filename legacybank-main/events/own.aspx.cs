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


public partial class events_own : System.Web.UI.Page
{
    public string sid_id
    {
        get
        {
            try { return Request["id"].ToString(); }
            catch { return ""; }
        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        ServicePointManager.Expect100Continue = true;
        ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12
        | SecurityProtocolType.Tls11
        | SecurityProtocolType.Tls
        | SecurityProtocolType.Ssl3;

        string sConexao = ConfigurationManager.AppSettings["user"].ToString() + ";" +
            ConfigurationManager.AppSettings["password"].ToString() + ";" +
            ConfigurationManager.AppSettings["datasource"].ToString() + ";" +
            ConfigurationManager.AppSettings["basecatalog"].ToString();

        var bodyStream = new StreamReader(HttpContext.Current.Request.InputStream);
        bodyStream.BaseStream.Seek(0, SeekOrigin.Begin);
        var bodyText = bodyStream.ReadToEnd();


        if (HttpContext.Current.Request.HttpMethod == "POST")
        {

            SqlConnection connInsConsOWN_TRANS = new SqlConnection(sConexao);
            connInsConsOWN_TRANS.Open();
            SqlCommand cmdInsConsOWN_TRANS = new SqlCommand("dbo.stp_notificacoes_own_ins", connInsConsOWN_TRANS);
            cmdInsConsOWN_TRANS.CommandType = CommandType.StoredProcedure;
            cmdInsConsOWN_TRANS.Parameters.Add("@flg_operacao", SqlDbType.Char).Value = 'I';
            cmdInsConsOWN_TRANS.Parameters.Add("@DES_JSON", SqlDbType.Text).Value = bodyText.ToString();
            cmdInsConsOWN_TRANS.Parameters.Add("@DTA_DATA", SqlDbType.DateTime).Value = DateTime.Now;
            cmdInsConsOWN_TRANS.Parameters.Add("@NOM_CODE", SqlDbType.VarChar).Value = sid_id.ToString();
            cmdInsConsOWN_TRANS.Parameters.Add("@NUM_TOKEN", SqlDbType.VarChar).Value = "0000000000000000000";
            cmdInsConsOWN_TRANS.Parameters.Add("@NOM_TIPO", SqlDbType.VarChar).Value = "INDEFINIDO";
            cmdInsConsOWN_TRANS.Parameters.Add("@FLG_STATUS", SqlDbType.VarChar).Value = "INDEFINIDO";

            //string iCodigoNotificacao = cmdInsConsOWN_TRANS.ExecuteScalar().ToString();

            cmdInsConsOWN_TRANS.ExecuteNonQuery();
            connInsConsOWN_TRANS.Close();
            connInsConsOWN_TRANS.Dispose();


            // Verifica o tipo de JSON


            string json = bodyText.ToString();

            // Localizar o ID da PESSOA LICENCIADA pelo WEBHOOK recebido

            int iPessoaLicenciado = 0;
            int iPessoa = 0;



            SqlConnection mySelCadastroLicenciado = new SqlConnection(sConexao);
            mySelCadastroLicenciado.Open();
            SqlCommand cmdSelCadastroLicenciado = new SqlCommand("dbo.stp_pessoas_fj_integracoes_chaves_ins", mySelCadastroLicenciado);
            cmdSelCadastroLicenciado.CommandType = CommandType.StoredProcedure;
            cmdSelCadastroLicenciado.CommandTimeout = 0;
            cmdSelCadastroLicenciado.Parameters.Add("@flg_operacao", SqlDbType.Char).Value = "W";
            cmdSelCadastroLicenciado.Parameters.Add("@NOM_WEBHOOK_ID", SqlDbType.VarChar).Value = sid_id.ToString();
            SqlDataReader ReaderCadastroLicenciado = cmdSelCadastroLicenciado.ExecuteReader();
            while (ReaderCadastroLicenciado.Read())
            {
                HttpContext.Current.Session.Add("LICENCIADO", ReaderCadastroLicenciado["COD_ID_PESSOA_LICENCIADO"].ToString());
                iPessoaLicenciado = Funcoes.strToInt(ReaderCadastroLicenciado["COD_ID_PESSOA_LICENCIADO"].ToString());
            }



            //try
            //{
                JToken token = JToken.Parse(json);

                if (token.Type == JTokenType.Object)
                {
                    JObject obj = (JObject)token;

                    if (obj != null && obj["modalide"] != null && obj["tipoTransacao"] != null)
                    {
                        
                        // Localizar ESTABELECIMENTO

                        SqlConnection mySelCadastroEC = new SqlConnection(sConexao);
                        mySelCadastroEC.Open();
                        SqlCommand cmdSelCadastroEC = new SqlCommand("dbo.stp_pessoas_fj_ins", mySelCadastroEC);
                        cmdSelCadastroEC.CommandType = CommandType.StoredProcedure;
                        cmdSelCadastroEC.CommandTimeout = 0;
                        cmdSelCadastroEC.Parameters.Add("@flg_operacao", SqlDbType.Char).Value = "H";
                        cmdSelCadastroEC.Parameters.Add("@COD_ID_PESSOA_LICENCIADO", SqlDbType.Int).Value = iPessoaLicenciado;
                        cmdSelCadastroEC.Parameters.Add("@NOM_CNPJ", SqlDbType.VarChar).Value = obj["docParceiro"].ToString();
                        cmdSelCadastroEC.Parameters.Add("@FLG_TIPO", SqlDbType.Char).Value = "E";
                        SqlDataReader ReaderCadastroEC = cmdSelCadastroEC.ExecuteReader();
                        while (ReaderCadastroEC.Read())
                        {
                            iPessoa = Funcoes.strToInt(ReaderCadastroEC["COD_ID"].ToString());
                        }

                        if (iPessoa <= 0)
                        {
                            // Importar dados do ESTABELECIMENTO
                            agilli.HttpResponseResult resultado = agilli.ConsultaResumidaEstabelecimento("?cpfCnpj=" + obj["docParceiro"].ToString());

                            if (resultado.StatusCode == 200)
                            {
                                // Sucesso - Processar JSON normalmente
                                string jsonResponse = resultado.Content;
                                JArray array = JArray.Parse(jsonResponse);

                                SqlConnection connInsConsPFJ = new SqlConnection(sConexao);
                                connInsConsPFJ.Open();
                                SqlCommand cmdInsConsPFJ = new SqlCommand("dbo.stp_pessoas_fj_ins", connInsConsPFJ);

                                cmdInsConsPFJ.CommandType = CommandType.StoredProcedure;
                                cmdInsConsPFJ.CommandTimeout = 0;
                                cmdInsConsPFJ.Parameters.Add("@flg_operacao", SqlDbType.Char).Value = 'I';
                                cmdInsConsPFJ.Parameters.Add("@FLG_ATIVO", SqlDbType.Char).Value = "S";
                                cmdInsConsPFJ.Parameters.Add("@FLG_INSERT_USUARIO", SqlDbType.Char).Value = "N";

                                cmdInsConsPFJ.Parameters.Add("@COD_ID_PESSOA_LICENCIADO", SqlDbType.Int).Value = iPessoaLicenciado;
                                cmdInsConsPFJ.Parameters.Add("@DTA_DATA", SqlDbType.DateTime).Value = DateTime.Now;
                                cmdInsConsPFJ.Parameters.Add("@FLG_TIPO", SqlDbType.Char).Value = "E";
                                cmdInsConsPFJ.Parameters.Add("@FLG_TIPO_PESSOA", SqlDbType.VarChar).Value = "PJ"; // ddlTipoFJ.SelectedValue.ToString();

                                // Empresa
                                cmdInsConsPFJ.Parameters.Add("@NOM_RAZAOSOCIAL", SqlDbType.VarChar).Value = array[0]["razaoSocial"].ToString();
                                cmdInsConsPFJ.Parameters.Add("@NOM_FANTASIA", SqlDbType.VarChar).Value = array[0]["nomeFantasia"].ToString();
                                cmdInsConsPFJ.Parameters.Add("@NOM_CNPJ", SqlDbType.VarChar).Value = array[0]["cnpj"].ToString();

                                cmdInsConsPFJ.Parameters.Add("@COD_ID_MCC", SqlDbType.Int).Value = Funcoes.strToInt(array[0]["mcc"].ToString());
                                cmdInsConsPFJ.Parameters.Add("@NOM_CIDADE", SqlDbType.VarChar).Value = array[0]["cidade"].ToString();
                                cmdInsConsPFJ.Parameters.Add("@NOM_UF", SqlDbType.VarChar).Value = array[0]["uf"].ToString();
                                cmdInsConsPFJ.Parameters.Add("@NOM_PAIS", SqlDbType.VarChar).Value = "BR";


                                string iCodigoNovoEstabelecimento = cmdInsConsPFJ.ExecuteScalar().ToString();
                                connInsConsPFJ.Close();
                                connInsConsPFJ.Dispose();

                                iPessoa = Funcoes.strToInt(iCodigoNovoEstabelecimento);

                                SqlConnection connInsConsCRED = new SqlConnection(sConexao);
                                connInsConsCRED.Open();
                                SqlCommand cmdInsConsCRED = new SqlCommand("dbo.stp_pessoas_fj_credenciamento_ins", connInsConsCRED);

                                cmdInsConsCRED.CommandType = CommandType.StoredProcedure;
                                cmdInsConsCRED.CommandTimeout = 0;
                                cmdInsConsCRED.Parameters.Add("@flg_operacao", SqlDbType.Char).Value = 'I';

                                cmdInsConsCRED.Parameters.Add("@COD_ID_PESSOA_LICENCIADO", SqlDbType.Int).Value = iPessoaLicenciado;
                                cmdInsConsCRED.Parameters.Add("@COD_ID_PESSOAS_FJ", SqlDbType.Int).Value = iPessoa;

                                cmdInsConsCRED.Parameters.Add("@NUM_TOKEN", SqlDbType.VarChar).Value = array[0]["contrato"].ToString();
                                cmdInsConsCRED.Parameters.Add("@FLG_INTEGRACAO", SqlDbType.Char).Value = "W";

                                cmdInsConsCRED.ExecuteNonQuery();
                                connInsConsCRED.Close();
                                connInsConsCRED.Dispose();
                            }
                        }


                        SqlConnection connInsCons = new SqlConnection(sConexao);
                        connInsCons.Open();
                        SqlCommand cmdInsCons = new SqlCommand("dbo.stp_transacoes_ins", connInsCons);
                        cmdInsCons.CommandType = CommandType.StoredProcedure;
                        cmdInsCons.Parameters.Add("@flg_operacao", SqlDbType.Char).Value = 'I';

                        cmdInsCons.Parameters.Add("@COD_ID_PESSOA_LICENCIADO", SqlDbType.Int).Value = iPessoaLicenciado;
                        cmdInsCons.Parameters.Add("@COD_ID_PESSOAS_FJ", SqlDbType.Int).Value = iPessoa;
                        
                        cmdInsCons.Parameters.Add("@FLG_ORIGEM", SqlDbType.Char).Value = "W";

                        cmdInsCons.Parameters.Add("@DTA_DATA", SqlDbType.DateTime).Value = Convert.ToDateTime(obj["data"].ToString());
                        cmdInsCons.Parameters.Add("@NOM_CODE", SqlDbType.VarChar).Value = obj["identificadorTransacao"].ToString();

                        cmdInsCons.Parameters.Add("@COD_ID_TIPO", SqlDbType.Int).Value = 1;
                        cmdInsCons.Parameters.Add("@NOM_STATUS", SqlDbType.VarChar).Value = "Confirmed";

                        cmdInsCons.Parameters.Add("@FLG_PRESENCIAL_ONLINE", SqlDbType.Char).Value = "P";
                        cmdInsCons.Parameters.Add("@NOM_TIPO_PAGAMENTO", SqlDbType.VarChar).Value = RetornaTipoPagamento(obj["modalide"].ToString());

                        cmdInsCons.Parameters.Add("@NOM_CODIGO_PAGAMENTO", SqlDbType.VarChar).Value = obj["bandeira"].ToString();
                        cmdInsCons.Parameters.Add("@NUM_PARCELAS", SqlDbType.Int).Value = Funcoes.strToInt(obj["quantidadeParcela"].ToString());
                        cmdInsCons.Parameters.Add("@NUM_DEVICE_BIN", SqlDbType.Char).Value = obj["cartao"].ToString();

                        cmdInsCons.Parameters.Add("@NOM_TIPO_OPERACAO_BANDEIRA", SqlDbType.VarChar).Value = obj["bandeira"].ToString();
                        // Verificar a existência dos principais tipos de operação

                        cmdInsCons.Parameters.Add("@NOM_TIPO_OPERACAO", SqlDbType.VarChar).Value = RetornaTipoPagamento(obj["modalide"].ToString());



                        cmdInsCons.Parameters.Add("@NOM_CARTAO_4_PRIMEIROS_DIGIROS", SqlDbType.VarChar).Value = obj["cartao"].ToString();
                        cmdInsCons.Parameters.Add("@NOM_CARTAO_4_ULTIMOS_DIGITOS", SqlDbType.VarChar).Value = obj["cartao"].ToString();

                        cmdInsCons.Parameters.Add("@NUM_VALOR_BRUTO", SqlDbType.Float).Value = Funcoes.strToDouble(obj["valor"].ToString());
                        cmdInsCons.Parameters.Add("@NUM_VALOR_LIQUIDO", SqlDbType.Float).Value = Funcoes.strToDouble(obj["valor"].ToString());
                        cmdInsCons.Parameters.Add("@NUM_ITENS", SqlDbType.Int).Value = 1;
                        cmdInsCons.Parameters.Add("@NOM_FONTE_CANCELAMENTO", SqlDbType.VarChar).Value = "";
                        cmdInsCons.Parameters.Add("@NOM_ORIGEM", SqlDbType.VarChar).Value = "OWN";

                        cmdInsCons.Parameters.Add("@NOM_CLIENTE", SqlDbType.VarChar).Value = obj["nomePortador"].ToString();
                        cmdInsCons.Parameters.Add("@NOM_NUMERO_TRANSACAO", SqlDbType.VarChar).Value = obj["numeroSerie"].ToString();
                        cmdInsCons.Parameters.Add("@NOM_RECIBO", SqlDbType.VarChar).Value = "";

                        cmdInsCons.Parameters.Add("@NOM_CODIGO_TERMINAL", SqlDbType.VarChar).Value = obj["terminal"].ToString();
                        cmdInsCons.Parameters.Add("@NOM_STATUS_TERMINAL", SqlDbType.VarChar).Value = "";
                        cmdInsCons.Parameters.Add("@NOM_NUMERO_SERIAL_TERMINAL", SqlDbType.VarChar).Value = obj["terminal"].ToString();
                        cmdInsCons.Parameters.Add("@NUM_DEVICE_SERIAL", SqlDbType.VarChar).Value = obj["terminal"].ToString();

                        cmdInsCons.Parameters.Add("@COD_ID_PLANO", SqlDbType.Int).Value = 0;

                        // EXECUTA A GRAVAÇÃO
                        cmdInsCons.ExecuteNonQuery();
                        connInsCons.Close();
                        connInsCons.Dispose();

                    }
                }
                else if (token.Type == JTokenType.Array)
                {
                    JArray array = (JArray)token;

                    if (array != null && array.Count > 0 && array[0]["lancamentoId"] != null)
                    {


                    }
                }
            //}
            //catch (JsonReaderException ex)
            //{
                //Console.WriteLine("Erro ao interpretar o JSON: " + ex.Message);
            //}

                HttpContext.Current.Response.ContentType = "application/json";
                HttpContext.Current.Response.Write(bodyText.ToString());
                HttpContext.Current.Response.End();


        }

    }

    public static string RetornaTipoPagamento(string sTipoPagamento)
    {
        if (string.IsNullOrWhiteSpace(sTipoPagamento))
            return "Não Identificado";

        // Normaliza: Remove acentos, coloca em maiúsculo e tira espaços extras
        string texto = RemoverAcentos(sTipoPagamento.Trim().ToUpper());

        // Se for código numérico, trata primeiro
        switch (texto)
        {
            case "1": return "Crédito";
            case "2": return "Boleto";
            case "3":
            case "4":
            case "8": return "Débito";
            case "11": return "Pix";
        }

        // Agora trata textos
        if (texto.Contains("PIX")) return "Pix";
        if (texto.Contains("CREDITO")) return "Crédito";
        if (texto.Contains("DEBITO")) return "Débito";
        if (texto.Contains("BOLETO")) return "Boleto";

        return "Não Identificado";
    }

    private static string RemoverAcentos(string texto)
    {
        var normalized = texto.Normalize(System.Text.NormalizationForm.FormD);
        var sb = new System.Text.StringBuilder();
        foreach (var c in normalized)
        {
            if (System.Globalization.CharUnicodeInfo.GetUnicodeCategory(c) !=
                System.Globalization.UnicodeCategory.NonSpacingMark)
            {
                sb.Append(c);
            }
        }
        return sb.ToString().Normalize(System.Text.NormalizationForm.FormC);
    }
}