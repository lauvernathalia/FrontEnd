using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Collections.Specialized;
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

using MessagingToolkit.QRCode.Codec;
using MessagingToolkit.QRCode.Codec.Data;
using System.Drawing;


/// <summary>
/// Summary description for split
/// </summary>
public class split
{

    public static DadosCobrancaAsaaS.split[] GerarSplitAsaas(string nomeBandeira, string flgIntegracao, string flgTipo, string flgoperacao, string flgpresencialonline, int parcelas)
    {
        var listaSplits = new List<DadosCobrancaAsaaS.split>();

        // SPLIT DA TABELA DE MARKUP POR EC

        using (var conn = new SqlConnection(Funcoes.conexao()))
        {
            conn.Open();
            using (var cmd = new SqlCommand("dbo.stp_pessoas_fj_markup_parcelas_ins", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@flg_operacao", SqlDbType.Char).Value = "T";
                cmd.Parameters.Add("@COD_ID_PESSOAS_FJ", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["PESSOA"].ToString());
                cmd.Parameters.Add("@COD_ID_PESSOA_LICENCIADO", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["LICENCIADO"].ToString());
                cmd.Parameters.Add("@NOM_BANDEIRA", SqlDbType.VarChar).Value = nomeBandeira;
                cmd.Parameters.Add("@FLG_INTEGRACAO", SqlDbType.Char).Value = flgIntegracao;
                cmd.Parameters.Add("@FLG_PRESENCIAL_ONLINE", SqlDbType.Char).Value = flgpresencialonline;
                cmd.Parameters.Add("@NUM_PARCELAS", SqlDbType.Int).Value = parcelas;
                cmd.Parameters.Add("@FLG_TIPO", SqlDbType.Char).Value = flgoperacao;

                using (var reader = cmd.ExecuteReader())
                {
                    string licenciado = HttpContext.Current.Session["LICENCIADO"].ToString();
                    string pessoa = HttpContext.Current.Session["PESSOA"].ToString();
                    string externalRef = licenciado + "_" + pessoa;

                    while (reader.Read())
                    {
                        if (Funcoes.strToDouble(reader["NUM_VALOR"].ToString()) > 0 &&
                            !string.IsNullOrWhiteSpace(reader["NOM_CHAVE_SPLIT"].ToString()))
                        {
                            listaSplits.Add(new DadosCobrancaAsaaS.split
                            {
                                walletId = reader["NOM_CHAVE_SPLIT"].ToString(),
                                percentualValue = 0,
                                fixedValue = Funcoes.strToDouble(reader["NUM_VALOR"].ToString()),
                                description = "Split para " + reader["NOM_RAZAOSOCIAL_FAVORECIDO"].ToString(),
                                externalReference = externalRef
                            });
                        }

                        if (Funcoes.strToDouble(reader["NUM_PERCENTUAL"].ToString()) > 0 &&
                            !string.IsNullOrWhiteSpace(reader["NOM_CHAVE_SPLIT"].ToString()))
                        {
                            listaSplits.Add(new DadosCobrancaAsaaS.split
                            {
                                walletId = reader["NOM_CHAVE_SPLIT"].ToString(),
                                percentualValue = Funcoes.strToDouble(reader["NUM_PERCENTUAL"].ToString()),
                                fixedValue = 0,
                                description = "Split para " + reader["NOM_RAZAOSOCIAL_FAVORECIDO"].ToString(),
                                externalReference = externalRef
                            });
                        }
                    }
                }
            }
        }

        // SPLIT DA TABELA DE SPLIT POR EC

        using (var conn = new SqlConnection(Funcoes.conexao()))
        {
            conn.Open();
            using (var cmd = new SqlCommand("dbo.stp_pessoas_fj_split_ins", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@flg_operacao", SqlDbType.Char).Value = "T";
                cmd.Parameters.Add("@COD_ID_PESSOAS_FJ", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["PESSOA"].ToString());
                cmd.Parameters.Add("@COD_ID_PESSOA_LICENCIADO", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["LICENCIADO"].ToString());
                cmd.Parameters.Add("@FLG_TIPO", SqlDbType.Char).Value = flgTipo;
                cmd.Parameters.Add("@FLG_INTEGRACAO", SqlDbType.Char).Value = flgIntegracao;

                using (var reader = cmd.ExecuteReader())
                {
                    string licenciado = HttpContext.Current.Session["LICENCIADO"].ToString();
                    string pessoa = HttpContext.Current.Session["PESSOA"].ToString();
                    string externalRef = licenciado + "_" + pessoa;

                    while (reader.Read())
                    {
                        if (Funcoes.strToDouble(reader["NUM_VALOR"].ToString()) > 0 &&
                            !string.IsNullOrWhiteSpace(reader["NOM_CHAVE_SPLIT"].ToString()))
                        {
                            listaSplits.Add(new DadosCobrancaAsaaS.split
                            {
                                walletId = reader["NOM_CHAVE_SPLIT"].ToString(),
                                percentualValue = 0,
                                fixedValue = Funcoes.strToDouble(reader["NUM_VALOR"].ToString()),
                                description = "Split para " + reader["NOM_RAZAOSOCIAL_FAVORECIDO"].ToString(),
                                externalReference = externalRef
                            });
                        }

                        if (Funcoes.strToDouble(reader["NUM_PERCENTUAL"].ToString()) > 0 &&
                            !string.IsNullOrWhiteSpace(reader["NOM_CHAVE_SPLIT"].ToString()))
                        {
                            listaSplits.Add(new DadosCobrancaAsaaS.split
                            {
                                walletId = reader["NOM_CHAVE_SPLIT"].ToString(),
                                percentualValue = Funcoes.strToDouble(reader["NUM_PERCENTUAL"].ToString()),
                                fixedValue = 0,
                                description = "Split para " + reader["NOM_RAZAOSOCIAL_FAVORECIDO"].ToString(),
                                externalReference = externalRef
                            });
                        }
                    }
                }
            }
        }


        return listaSplits.ToArray();
    }

    public static DadosTransacao.split_rules[] GerarSplitZoop(string nomeBandeira, string flgIntegracao, string flgTipo, string flgoperacao, string flgpresencialonline, int parcelas, out string sellerToken)
    {

        List<DadosTransacao.split_rules> listaSplits = new List<DadosTransacao.split_rules>();
        sellerToken = "";

        // SPLIT DA TABELA DE MARKUP POR EC
        using (var conn01 = new SqlConnection(Funcoes.conexao()))
        {
            conn01.Open();
            using (var cmd01 = new SqlCommand("dbo.stp_pessoas_fj_markup_parcelas_ins", conn01))
            {
                cmd01.CommandType = CommandType.StoredProcedure;
                cmd01.Parameters.Add("@flg_operacao", SqlDbType.Char).Value = "T";
                cmd01.Parameters.Add("@COD_ID_PESSOAS_FJ", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["PESSOA"].ToString());
                cmd01.Parameters.Add("@COD_ID_PESSOA_LICENCIADO", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["LICENCIADO"].ToString());
                cmd01.Parameters.Add("@NOM_BANDEIRA", SqlDbType.VarChar).Value = nomeBandeira;
                cmd01.Parameters.Add("@FLG_INTEGRACAO", SqlDbType.Char).Value = flgIntegracao;
                cmd01.Parameters.Add("@FLG_PRESENCIAL_ONLINE", SqlDbType.Char).Value = flgpresencialonline;
                cmd01.Parameters.Add("@NUM_PARCELAS", SqlDbType.Int).Value = parcelas;
                cmd01.Parameters.Add("@FLG_TIPO", SqlDbType.Char).Value = flgoperacao;

                using (var reader01 = cmd01.ExecuteReader())
                {
                    while (reader01.Read())
                    {
                        if (Funcoes.strToDouble(reader01["NUM_VALOR_MARKUP_TRANSACAO"].ToString()) > 0 &&
                            !string.IsNullOrWhiteSpace(reader01["NOM_CHAVE_SPLIT"].ToString()))
                        {
                            listaSplits.Add(new DadosTransacao.split_rules()
                            {
                                recipient = reader01["NOM_CHAVE_SPLIT"].ToString(),
                                charge_processing_fee = false,
                                amount = Funcoes.strToInt((Funcoes.strToDouble(reader01["NUM_VALOR_MARKUP_TRANSACAO"].ToString()) * 100).ToString())
                            });
                        }
                        if (Funcoes.strToDouble(reader01["NUM_VALOR_MARKUP_DEBITO"].ToString()) > 0 &&
                            !string.IsNullOrWhiteSpace(reader01["NOM_CHAVE_SPLIT"].ToString()))
                        {
                            listaSplits.Add(new DadosTransacao.split_rules()
                            {
                                recipient = reader01["NOM_CHAVE_SPLIT"].ToString(),
                                charge_processing_fee = false,
                                percentage = Funcoes.strToDouble(reader01["NUM_VALOR_MARKUP_DEBITO"].ToString())
                            });
                        }

                        sellerToken = reader01["NUM_TOKEN"].ToString();
                    }
                }
            }
        }

        // SPLIT DA TABELA DE SPLIT POR EC

        using (var conn02 = new SqlConnection(Funcoes.conexao()))
        {
            conn02.Open();
            using (var cmd02 = new SqlCommand("dbo.stp_pessoas_fj_split_ins", conn02))
            {
                cmd02.CommandType = CommandType.StoredProcedure;
                cmd02.Parameters.Add("@flg_operacao", SqlDbType.Char).Value = "T";
                cmd02.Parameters.Add("@COD_ID_PESSOAS_FJ", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["PESSOA"].ToString());
                cmd02.Parameters.Add("@COD_ID_PESSOA_LICENCIADO", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["LICENCIADO"].ToString());
                cmd02.Parameters.Add("@FLG_TIPO", SqlDbType.VarChar).Value = flgTipo;
                cmd02.Parameters.Add("@FLG_INTEGRACAO", SqlDbType.Char).Value = flgIntegracao;

                using (var reader02 = cmd02.ExecuteReader())
                {
                    while (reader02.Read())
                    {

                        if (Funcoes.strToDouble(reader02["NUM_VALOR"].ToString()) > 0 &&
                            !string.IsNullOrWhiteSpace(reader02["NOM_CHAVE_SPLIT"].ToString()))
                        {
                            listaSplits.Add(new DadosTransacao.split_rules()
                            {
                                recipient = reader02["NOM_CHAVE_SPLIT"].ToString(),
                                charge_processing_fee = false,
                                amount = Funcoes.strToInt((Funcoes.strToDouble(reader02["NUM_VALOR"].ToString()) * 100).ToString())
                            });
                        }
                        if (Funcoes.strToDouble(reader02["NUM_PERCENTUAL"].ToString()) > 0 &&
                            !string.IsNullOrWhiteSpace(reader02["NOM_CHAVE_SPLIT"].ToString()))
                        {
                            listaSplits.Add(new DadosTransacao.split_rules()
                            {
                                recipient = reader02["NOM_CHAVE_SPLIT"].ToString(),
                                charge_processing_fee = false,
                                percentage = Funcoes.strToDouble(reader02["NUM_PERCENTUAL"].ToString())
                            });
                        }

                        sellerToken = reader02["NUM_TOKEN"].ToString();
                    }
                }
            }
        }

        return listaSplits.ToArray();
    }


    public class ZoopSplit
    {
        public string recipient { get; set; }
        public bool charge_processing_fee { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public int? amount { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public double? percentage { get; set; }
    }

    public static ZoopSplit[] GerarSplitPresencialZoop(int licenciado, int pessoa, string nomeBandeira, string flgIntegracao, string flgTipo, string flgoperacao, string flgpresencialonline, int parcelas, out string sellerToken)
    {

        List<ZoopSplit> listaSplits = new List<ZoopSplit>();
        sellerToken = "";

        // SPLIT DA TABELA DE MARKUP POR EC
        using (var conn00 = new SqlConnection(Funcoes.conexao()))
        {
            conn00.Open();
            using (var cmd00 = new SqlCommand("dbo.stp_planos_parcelas_ins", conn00))
            {
                cmd00.CommandType = CommandType.StoredProcedure;
                cmd00.Parameters.Add("@flg_operacao", SqlDbType.Char).Value = "T";
                cmd00.Parameters.Add("@COD_ID_PESSOAS_FJ", SqlDbType.Int).Value = pessoa;
                cmd00.Parameters.Add("@COD_ID_PESSOA_LICENCIADO", SqlDbType.Int).Value = licenciado;
                cmd00.Parameters.Add("@NOM_BANDEIRA", SqlDbType.VarChar).Value = nomeBandeira;
                cmd00.Parameters.Add("@FLG_INTEGRACAO", SqlDbType.Char).Value = flgIntegracao;
                cmd00.Parameters.Add("@FLG_PRESENCIAL_ONLINE", SqlDbType.Char).Value = flgpresencialonline;
                cmd00.Parameters.Add("@NUM_PARCELAS", SqlDbType.Int).Value = parcelas;
                cmd00.Parameters.Add("@FLG_TIPO", SqlDbType.Char).Value = flgoperacao;

                using (var reader00 = cmd00.ExecuteReader())
                {
                    while (reader00.Read())
                    {
                        if (Funcoes.strToDouble(reader00["NUM_VALOR"].ToString()) > 0 &&
                            !string.IsNullOrWhiteSpace(reader00["NOM_CHAVE_SPLIT"].ToString()))
                        {
                            listaSplits.Add(new ZoopSplit()
                            {
                                recipient = reader00["NOM_CHAVE_SPLIT"].ToString(),
                                charge_processing_fee = false,
                                amount = Funcoes.strToInt((Funcoes.strToDouble(reader00["NUM_VALOR"].ToString()) * 100).ToString())
                            });
                        }
                        if (Funcoes.strToDouble(reader00["NUM_PERCENTUAL"].ToString()) > 0 &&
                            !string.IsNullOrWhiteSpace(reader00["NOM_CHAVE_SPLIT"].ToString()))
                        {
                            listaSplits.Add(new ZoopSplit()
                            {
                                recipient = reader00["NOM_CHAVE_SPLIT"].ToString(),
                                charge_processing_fee = false,
                                percentage = Funcoes.strToDouble(reader00["NUM_PERCENTUAL"].ToString())
                            });
                        }

                        sellerToken = reader00["NUM_TOKEN"].ToString();
                    }
                }
            }
        }


        using (var conn01 = new SqlConnection(Funcoes.conexao()))
        {
            conn01.Open();
            using (var cmd01 = new SqlCommand("dbo.stp_pessoas_fj_markup_parcelas_ins", conn01))
            {
                cmd01.CommandType = CommandType.StoredProcedure;
                cmd01.Parameters.Add("@flg_operacao", SqlDbType.Char).Value = "T";
                cmd01.Parameters.Add("@COD_ID_PESSOAS_FJ", SqlDbType.Int).Value = pessoa;
                cmd01.Parameters.Add("@COD_ID_PESSOA_LICENCIADO", SqlDbType.Int).Value = licenciado;
                cmd01.Parameters.Add("@NOM_BANDEIRA", SqlDbType.VarChar).Value = nomeBandeira;
                cmd01.Parameters.Add("@FLG_INTEGRACAO", SqlDbType.Char).Value = flgIntegracao;
                cmd01.Parameters.Add("@FLG_PRESENCIAL_ONLINE", SqlDbType.Char).Value = flgpresencialonline;
                cmd01.Parameters.Add("@NUM_PARCELAS", SqlDbType.Int).Value = parcelas;
                cmd01.Parameters.Add("@FLG_TIPO", SqlDbType.Char).Value = flgoperacao;

                using (var reader01 = cmd01.ExecuteReader())
                {
                    while (reader01.Read())
                    {
                        if (Funcoes.strToDouble(reader01["NUM_VALOR"].ToString()) > 0 &&
                            !string.IsNullOrWhiteSpace(reader01["NOM_CHAVE_SPLIT"].ToString()))
                        {
                            listaSplits.Add(new ZoopSplit()
                            {
                                recipient = reader01["NOM_CHAVE_SPLIT"].ToString(),
                                charge_processing_fee = false,
                                amount = Funcoes.strToInt((Funcoes.strToDouble(reader01["NUM_VALOR"].ToString()) * 100).ToString())
                            });
                        }
                        if (Funcoes.strToDouble(reader01["NUM_PERCENTUAL"].ToString()) > 0 &&
                            !string.IsNullOrWhiteSpace(reader01["NOM_CHAVE_SPLIT"].ToString()))
                        {
                            listaSplits.Add(new ZoopSplit()
                            {
                                recipient = reader01["NOM_CHAVE_SPLIT"].ToString(),
                                charge_processing_fee = false,
                                percentage = Funcoes.strToDouble(reader01["NUM_PERCENTUAL"].ToString())
                            });
                        }

                        sellerToken = reader01["NUM_TOKEN"].ToString();
                    }
                }
            }
        }


        // SPLIT DA TABELA DE SPLIT POR EC

        using (var conn02 = new SqlConnection(Funcoes.conexao()))
        {
            conn02.Open();
            using (var cmd02 = new SqlCommand("dbo.stp_pessoas_fj_split_ins", conn02))
            {
                cmd02.CommandType = CommandType.StoredProcedure;
                cmd02.Parameters.Add("@flg_operacao", SqlDbType.Char).Value = "T";
                cmd02.Parameters.Add("@COD_ID_PESSOAS_FJ", SqlDbType.Int).Value = pessoa;
                cmd02.Parameters.Add("@COD_ID_PESSOA_LICENCIADO", SqlDbType.Int).Value = licenciado;
                cmd02.Parameters.Add("@FLG_TIPO", SqlDbType.VarChar).Value = flgTipo;
                cmd02.Parameters.Add("@FLG_INTEGRACAO", SqlDbType.Char).Value = flgIntegracao;

                using (var reader02 = cmd02.ExecuteReader())
                {
                    while (reader02.Read())
                    {

                        if (Funcoes.strToDouble(reader02["NUM_VALOR"].ToString()) > 0 &&
                            !string.IsNullOrWhiteSpace(reader02["NOM_CHAVE_SPLIT"].ToString()))
                        {
                            listaSplits.Add(new ZoopSplit()
                            {
                                recipient = reader02["NOM_CHAVE_SPLIT"].ToString(),
                                charge_processing_fee = false,
                                amount = Funcoes.strToInt((Funcoes.strToDouble(reader02["NUM_VALOR"].ToString()) * 100).ToString())
                            });
                        }
                        if (Funcoes.strToDouble(reader02["NUM_PERCENTUAL"].ToString()) > 0 &&
                            !string.IsNullOrWhiteSpace(reader02["NOM_CHAVE_SPLIT"].ToString()))
                        {
                            listaSplits.Add(new ZoopSplit()
                            {
                                recipient = reader02["NOM_CHAVE_SPLIT"].ToString(),
                                charge_processing_fee = false,
                                percentage = Funcoes.strToDouble(reader02["NUM_PERCENTUAL"].ToString())
                            });
                        }

                        sellerToken = reader02["NUM_TOKEN"].ToString();
                    }
                }
            }
        }

        return listaSplits.ToArray();
    }

}