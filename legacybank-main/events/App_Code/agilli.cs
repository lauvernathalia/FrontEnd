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
/// Summary description for agilli
/// </summary>
    public static class agilli
    {
        public class DadosCredenciamentoLojistaAgilli
        {
            // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
            public class Anexo
            {
                public string nomeArquivo { get; set; }
                public string conteudo { get; set; }
                public string tipo { get; set; }
            }

            public class DocumentosSocio
            {
                public string identificacao { get; set; }
                public Anexo[] anexos { get; set; }
            }

            public class Terminais
            {
                public string nomeArquivo { get; set; }
                public string conteudo { get; set; }
                public string tipo { get; set; }
            }


            public class Root
            {
                public string cnpj { get; set; }
                public string cnpjCanalWL { get; set; }
                public string cnpjOrigem { get; set; }
                public string identificadorCliente { get; set; }
                public string urlCallback { get; set; }
                public string razaoSocial { get; set; }
                public string nomeFantasia { get; set; }
                public string cnae { get; set; }
                public string ramoAtividade { get; set; }
                public int faturamentoPrevisto { get; set; }
                public string email { get; set; }
                public string dddComercial { get; set; }
                public string telefoneComercial { get; set; }
                public string cep { get; set; }
                public string logradouro { get; set; }
                public int numeroEndereco { get; set; }
                public string complemento { get; set; }
                public string bairro { get; set; }
                public string municipio { get; set; }
                public string uf { get; set; }
                public string dddCel { get; set; }
                public string telefoneCelular { get; set; }
                public string responsavelAssinatura { get; set; }
                public int quantidadePos { get; set; }
                public int faturamentoContratado { get; set; }
                public string antecipacaoAutomatica { get; set; }
                public int taxaAntecipacao { get; set; }
                public string tipoAntecipacao { get; set; }
                public string mcc { get; set; }
                public string tipoContrato { get; set; }
                public string codConfiguracao { get; set; }
                public string cnpjParceiro { get; set; }
                public int idCesta { get; set; }
                public Tarifacao[] tarifacao { get; set; }
                public string codBanco { get; set; }
                public string agencia { get; set; }
                public string digAgencia { get; set; }
                public string numConta { get; set; }
                public string digConta { get; set; }
                public string protocoloCore { get; set; }
                public string hashAceite { get; set; }
                public Terminais[] terminais { get; set; }
                public DocumentosSocio[] documentosSocios { get; set; }
                public Anexo[] anexos { get; set; }
            }

            public class Tarifacao
            {
                public int id { get; set; }
                public double valor { get; set; }
            }

        }


        public class DadosAutenticacaoAgilli
        {
            public class Root
            {
                public string client_id { get; set; }
                public string client_secret { get; set; }
                public string scope { get; set; }
                public string grant_type { get; set; }
            }
        }

        public class DadosConsultaTransacoes
        {
            public class Root
            {
                public string cnpjCliente { get; set; }
                public string docParceiro { get; set; }
                public string identificadorTransacao { get; set; }
                public string statusTransacao { get; set; }
                public string dataInicial { get; set; }
                public string dataFinal { get; set; }
            }
        }

        public static string VersaoAgilli(string versao)
        {
            string sVersaoAgilli = "";
            if (versao == "S") { sVersaoAgilli = "https://acquirer-qa.own.financial/agilli/"; }
            if (versao == "P") { sVersaoAgilli = "https://acquirer.own.financial/agilli/"; }
            return sVersaoAgilli;
        }


        public class HttpResponseResult
        {
            public int StatusCode { get; set; } // Código HTTP (200, 401, 500, etc.)
            public string Content { get; set; } // Resposta JSON ou mensagem de erro

            public HttpResponseResult(int statusCode, string content)
            {
                StatusCode = statusCode;
                Content = content;
            }
        }


        public static string KeyIntegracao()
        {
            string sPadrao = "own.api_wl.api";

            SqlConnection mySelCadastro = new SqlConnection(Funcoes.conexao());
            mySelCadastro.Open();
            SqlCommand cmdSelCadastro = new SqlCommand("dbo.stp_pessoas_fj_integracoes_chaves_ins", mySelCadastro);
            cmdSelCadastro.CommandType = CommandType.StoredProcedure;
            cmdSelCadastro.Parameters.Add("@flg_operacao", SqlDbType.Char).Value = "L";
            cmdSelCadastro.Parameters.Add("@COD_ID_INTEGRACOES", SqlDbType.Int).Value = 13;
            cmdSelCadastro.Parameters.Add("@COD_ID_PESSOA_LICENCIADO", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["LICENCIADO"].ToString());
            SqlDataReader ReaderCadastro = cmdSelCadastro.ExecuteReader();
            while (ReaderCadastro.Read())
            {
                sPadrao = ReaderCadastro["NOM_KEY"].ToString().Trim();
            }
            return sPadrao.ToString();
        }

        public static string SecretIntegracao()
        {
            string sPadrao = "GWu2ZxnU8idsxCvm4L20Aw8Xld4sA1M2";

            SqlConnection mySelCadastro = new SqlConnection(Funcoes.conexao());
            mySelCadastro.Open();
            SqlCommand cmdSelCadastro = new SqlCommand("dbo.stp_pessoas_fj_integracoes_chaves_ins", mySelCadastro);
            cmdSelCadastro.CommandType = CommandType.StoredProcedure;
            cmdSelCadastro.Parameters.Add("@flg_operacao", SqlDbType.Char).Value = "L";
            cmdSelCadastro.Parameters.Add("@COD_ID_INTEGRACOES", SqlDbType.Int).Value = 13;
            cmdSelCadastro.Parameters.Add("@COD_ID_PESSOA_LICENCIADO", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["LICENCIADO"].ToString());
            SqlDataReader ReaderCadastro = cmdSelCadastro.ExecuteReader();
            while (ReaderCadastro.Read())
            {
                sPadrao = ReaderCadastro["NOM_SECRET"].ToString().Trim();
            }
            return sPadrao.ToString();
        }

        public static string IDIntegracao()
        {
            string sPadrao = "35675737000199-own-api.white_label";

            SqlConnection mySelCadastro = new SqlConnection(Funcoes.conexao());
            mySelCadastro.Open();
            SqlCommand cmdSelCadastro = new SqlCommand("dbo.stp_pessoas_fj_integracoes_chaves_ins", mySelCadastro);
            cmdSelCadastro.CommandType = CommandType.StoredProcedure;
            cmdSelCadastro.Parameters.Add("@flg_operacao", SqlDbType.Char).Value = "L";
            cmdSelCadastro.Parameters.Add("@COD_ID_INTEGRACOES", SqlDbType.Int).Value = 13;
            cmdSelCadastro.Parameters.Add("@COD_ID_PESSOA_LICENCIADO", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["LICENCIADO"].ToString());
            SqlDataReader ReaderCadastro = cmdSelCadastro.ExecuteReader();
            while (ReaderCadastro.Read())
            {
                sPadrao = ReaderCadastro["NOM_ID"].ToString().Trim();
            }
            return sPadrao.ToString();
        }

        public static string TokenAutenticacao()
        {
            string sToken = "";

            if (HttpContext.Current.Session["tokenagilli"] != null)
            {
                sToken = HttpContext.Current.Session["tokenagilli"].ToString();

                if (HttpContext.Current.Session["tempoexpiracao"] != null)
                {
                    // Pegue o valor, por exemplo, convertendo para DateTime
                    DateTime tempoExpiracao = (DateTime)HttpContext.Current.Session["tempoexpiracao"];
                    // Faça algo com tempoExpiracao

                    if (tempoExpiracao < DateTime.Now)
                    {
                        agilli.HttpResponseResult resultado = agilli.AutenticacaoAgilli();
                        if (resultado.StatusCode == 200)
                        {
                            // Sucesso - Processar JSON normalmente
                            string jsonResponse = resultado.Content;
                            JObject oAutenticacao = JObject.Parse(jsonResponse);

                            sToken = oAutenticacao["access_token"].ToString();
                            HttpContext.Current.Session["tokenagilli"] = sToken;
                            HttpContext.Current.Session["tempoexpiracao"] = DateTime.Now.AddSeconds(Funcoes.strToInt(oAutenticacao["expires_in"].ToString()));
                        }
                        else
                        {
                            string jsonResponse = resultado.Content;
                        }

                    }
                }
                else
                {
                    agilli.HttpResponseResult resultado = agilli.AutenticacaoAgilli();
                    if (resultado.StatusCode == 200)
                    {
                        // Sucesso - Processar JSON normalmente
                        string jsonResponse = resultado.Content;
                        JObject oAutenticacao = JObject.Parse(jsonResponse);

                        sToken = oAutenticacao["access_token"].ToString();
                        HttpContext.Current.Session["tokenagilli"] = sToken;
                        HttpContext.Current.Session["tempoexpiracao"] = DateTime.Now.AddSeconds(Funcoes.strToInt(oAutenticacao["expires_in"].ToString()));
                    }
                    else
                    {
                        string jsonResponse = resultado.Content;
                    }

                }

            }
            else
            {
                agilli.HttpResponseResult resultado = agilli.AutenticacaoAgilli();
                if (resultado.StatusCode == 200)
                {
                    // Sucesso - Processar JSON normalmente
                    string jsonResponse = resultado.Content;
                    JObject oAutenticacao = JObject.Parse(jsonResponse);

                    sToken = oAutenticacao["access_token"].ToString();
                    HttpContext.Current.Session["tokenagilli"] = sToken;
                    HttpContext.Current.Session["tempoexpiracao"] = DateTime.Now.AddSeconds(Funcoes.strToInt(oAutenticacao["expires_in"].ToString()));
                }
                else
                {
                    string jsonResponse = resultado.Content;
                }


            }
            return sToken;
        }

        public static HttpResponseResult AutenticacaoAgilli()
        {
            agilli.DadosAutenticacaoAgilli.Root dautenticacao = new agilli.DadosAutenticacaoAgilli.Root()
            {
                client_id = agilli.IDIntegracao(),
                client_secret = agilli.SecretIntegracao(),
                scope = agilli.KeyIntegracao(),
                grant_type = "client_credentials"
            };

            string json = JsonConvert.SerializeObject(dautenticacao);

            ServicePointManager.Expect100Continue = true;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12
                | SecurityProtocolType.Tls11
                | SecurityProtocolType.Tls
                | SecurityProtocolType.Ssl3;

            var url = agilli.VersaoAgilli("P").ToString() + "v2/auth";
            var myUri = new Uri(url);
            var myHttpWebRequest = (HttpWebRequest)WebRequest.Create(myUri);

            myHttpWebRequest.ContentType = "application/json";
            myHttpWebRequest.Accept = "application/json";
            myHttpWebRequest.Method = "POST";
            myHttpWebRequest.PreAuthenticate = true;

            using (var streamWriter = new StreamWriter(myHttpWebRequest.GetRequestStream()))
            {
                streamWriter.Write(json);
                streamWriter.Flush();
            }

            try
            {
                using (var response = (HttpWebResponse)myHttpWebRequest.GetResponse())
                using (var reader = new StreamReader(response.GetResponseStream(), Encoding.UTF8))
                {
                    string responseJson = reader.ReadToEnd();
                    return new HttpResponseResult((int)response.StatusCode, responseJson);
                }
            }
            catch (WebException ex)
            {
                if (ex.Response != null)
                {
                    using (var response = (HttpWebResponse)ex.Response)
                    using (var reader = new StreamReader(response.GetResponseStream()))
                    {
                        string errorJson = reader.ReadToEnd();
                        return new HttpResponseResult((int)response.StatusCode, errorJson);
                    }
                }

                return new HttpResponseResult(500, String.Format("Erro inesperado: {0}", ex.Message));
            }
        }

        public static HttpResponseResult ConsultaTransacoes(string json)
        {

            ServicePointManager.Expect100Continue = true;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12
                | SecurityProtocolType.Tls11
                | SecurityProtocolType.Tls
                | SecurityProtocolType.Ssl3;

            var url = agilli.VersaoAgilli("P").ToString() + "transacoes/v2/buscaTransacoesGerais";
            var myUri = new Uri(url);
            var myHttpWebRequest = (HttpWebRequest)WebRequest.Create(myUri);

            myHttpWebRequest.ContentType = "application/json";
            myHttpWebRequest.Accept = "application/json";
            myHttpWebRequest.Method = "POST";
            myHttpWebRequest.PreAuthenticate = true;
            myHttpWebRequest.Headers.Add("Authorization", "Bearer " + agilli.TokenAutenticacao());

            using (var streamWriter = new StreamWriter(myHttpWebRequest.GetRequestStream()))
            {
                streamWriter.Write(json);
                streamWriter.Flush();
            }

            try
            {
                using (var response = (HttpWebResponse)myHttpWebRequest.GetResponse())
                using (var reader = new StreamReader(response.GetResponseStream(), Encoding.UTF8))
                {
                    string responseJson = reader.ReadToEnd();
                    return new HttpResponseResult((int)response.StatusCode, responseJson);
                }
            }
            catch (WebException ex)
            {
                if (ex.Response != null)
                {
                    using (var response = (HttpWebResponse)ex.Response)
                    using (var reader = new StreamReader(response.GetResponseStream()))
                    {
                        string errorJson = reader.ReadToEnd();
                        return new HttpResponseResult((int)response.StatusCode, errorJson);
                    }
                }

                return new HttpResponseResult(500, String.Format("Erro inesperado: {0}", ex.Message));
            }
        }

        public static HttpResponseResult ConsultaCestas(string parametros)
        {

            ServicePointManager.Expect100Continue = true;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12
                | SecurityProtocolType.Tls11
                | SecurityProtocolType.Tls
                | SecurityProtocolType.Ssl3;

            var url = agilli.VersaoAgilli("P").ToString() + "parceiro/v2/consultarCesta" + parametros;
            var myUri = new Uri(url);
            var myHttpWebRequest = (HttpWebRequest)WebRequest.Create(myUri);

            myHttpWebRequest.ContentType = "application/json";
            myHttpWebRequest.Accept = "application/json";
            myHttpWebRequest.Method = "GET";
            myHttpWebRequest.PreAuthenticate = true;
            myHttpWebRequest.Headers.Add("Authorization", "Bearer " + agilli.TokenAutenticacao());

            try
            {
                using (var response = (HttpWebResponse)myHttpWebRequest.GetResponse())
                using (var reader = new StreamReader(response.GetResponseStream(), Encoding.UTF8))
                {
                    string responseJson = reader.ReadToEnd();
                    return new HttpResponseResult((int)response.StatusCode, responseJson);
                }
            }
            catch (WebException ex)
            {
                if (ex.Response != null)
                {
                    using (var response = (HttpWebResponse)ex.Response)
                    using (var reader = new StreamReader(response.GetResponseStream()))
                    {
                        string errorJson = reader.ReadToEnd();
                        return new HttpResponseResult((int)response.StatusCode, errorJson);
                    }
                }

                return new HttpResponseResult(500, String.Format("Erro inesperado: {0}", ex.Message));
            }
        }


        public static HttpResponseResult ConsultaResumidaEstabelecimento(string parametros)
        {

            ServicePointManager.Expect100Continue = true;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12
                | SecurityProtocolType.Tls11
                | SecurityProtocolType.Tls
                | SecurityProtocolType.Ssl3;

            var url = agilli.VersaoAgilli("P").ToString() + "indicadores/v2/cadastrais" + parametros;
            var myUri = new Uri(url);
            var myHttpWebRequest = (HttpWebRequest)WebRequest.Create(myUri);

            myHttpWebRequest.ContentType = "application/json";
            myHttpWebRequest.Accept = "application/json";
            myHttpWebRequest.Method = "GET";
            myHttpWebRequest.PreAuthenticate = true;
            myHttpWebRequest.Headers.Add("Authorization", "Bearer " + agilli.TokenAutenticacao());

            try
            {
                using (var response = (HttpWebResponse)myHttpWebRequest.GetResponse())
                using (var reader = new StreamReader(response.GetResponseStream(), Encoding.UTF8))
                {
                    string responseJson = reader.ReadToEnd();
                    return new HttpResponseResult((int)response.StatusCode, responseJson);
                }
            }
            catch (WebException ex)
            {
                if (ex.Response != null)
                {
                    using (var response = (HttpWebResponse)ex.Response)
                    using (var reader = new StreamReader(response.GetResponseStream()))
                    {
                        string errorJson = reader.ReadToEnd();
                        return new HttpResponseResult((int)response.StatusCode, errorJson);
                    }
                }

                return new HttpResponseResult(500, String.Format("Erro inesperado: {0}", ex.Message));
            }
        }




        public static HttpResponseResult CredenciamentoLogista(string json)
        {

            ServicePointManager.Expect100Continue = true;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12
                | SecurityProtocolType.Tls11
                | SecurityProtocolType.Tls
                | SecurityProtocolType.Ssl3;

            var url = agilli.VersaoAgilli("P").ToString() + "parceiro/v2/cadastrarConveniada";
            var myUri = new Uri(url);
            var myHttpWebRequest = (HttpWebRequest)WebRequest.Create(myUri);

            myHttpWebRequest.ContentType = "application/json";
            myHttpWebRequest.Accept = "application/json";
            myHttpWebRequest.Method = "POST";
            myHttpWebRequest.PreAuthenticate = true;
            myHttpWebRequest.Headers.Add("Authorization", "Bearer " + agilli.TokenAutenticacao());

            using (var streamWriter = new StreamWriter(myHttpWebRequest.GetRequestStream()))
            {
                streamWriter.Write(json);
                streamWriter.Flush();
            }

            try
            {
                using (var response = (HttpWebResponse)myHttpWebRequest.GetResponse())
                using (var reader = new StreamReader(response.GetResponseStream(), Encoding.UTF8))
                {
                    string responseJson = reader.ReadToEnd();
                    return new HttpResponseResult((int)response.StatusCode, responseJson);
                }
            }
            catch (WebException ex)
            {
                if (ex.Response != null)
                {
                    using (var response = (HttpWebResponse)ex.Response)
                    using (var reader = new StreamReader(response.GetResponseStream()))
                    {
                        string errorJson = reader.ReadToEnd();
                        return new HttpResponseResult((int)response.StatusCode, errorJson);
                    }
                }

                return new HttpResponseResult(500, String.Format("Erro inesperado: {0}", ex.Message));
            }
        }


    }
