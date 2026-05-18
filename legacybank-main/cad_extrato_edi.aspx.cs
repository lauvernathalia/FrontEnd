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

using System.Collections.Specialized;

public partial class cad_extrato_edi : System.Web.UI.Page
{

    private int Licenciado { get; set; }
    private string Tipo { get; set; }
    private int Pessoa { get; set; }
    private int Usuario { get; set; }

    private string ip { get; set; }

    public static DataTable dtTransacoes;

    //private static int ProgressoImportacao = 0;

    protected void Page_Load(object sender, EventArgs e)
    {
        // === CARGA SEGURA DE SESSION ===
        Licenciado = Session["LICENCIADO"] != null ? Funcoes.strToInt(Session["LICENCIADO"].ToString()) : 0;
        Tipo = Session["TIPO"] != null ? Session["TIPO"].ToString().Trim() : "";
        Pessoa = Session["PESSOA"] != null ? Funcoes.strToInt(Session["PESSOA"].ToString()) : 0;
        Usuario = Session["CODIGO"] != null ? Funcoes.strToInt(Session["CODIGO"].ToString()) : 0;
        ip = Request.ServerVariables["REMOTE_ADDR"] != null ? Request.ServerVariables["REMOTE_ADDR"].ToString() : "";

        if (!IsPostBack)
        {

            Funcoes.GravaAuditoria(0, ip, "Dashboard - Extrato EDI", "Consulta");

            dtTransacoes = new DataTable();
            dtTransacoes.Columns.Add("id", typeof(string));
            dtTransacoes.Columns.Add("transacao", typeof(string));
            dtTransacoes.Columns.Add("data", typeof(string));
            dtTransacoes.Columns.Add("valor", typeof(string));

            txtDataIni.Text = DateTime.Now.AddDays(-1).ToShortDateString();

            // ==============================
            // CARREGA REDES (L)
            // ==============================
            using (SqlConnection myProduto = new SqlConnection(Funcoes.conexao()))
            using (SqlCommand cmdProduto = new SqlCommand("dbo.stp_pessoas_fj_edi_ins", myProduto))
            {
                cmdProduto.CommandType = CommandType.StoredProcedure;
                cmdProduto.Parameters.Add("@flg_operacao", SqlDbType.VarChar).Value = "L";
                cmdProduto.Parameters.Add("@COD_ID_PESSOA_LICENCIADO", SqlDbType.Int).Value = Licenciado;

                using (SqlDataAdapter drProduto = new SqlDataAdapter(cmdProduto))
                {
                    DataSet dsProduto = new DataSet();
                    drProduto.Fill(dsProduto, "PESSOAS_FJ_EDI");

                    if (dsProduto.Tables["PESSOAS_FJ_EDI"] != null)
                    {
                        ddlRedes.DataTextField = "NOM_REDE_COMPLETA";
                        ddlRedes.DataValueField = "COD_ID";
                        ddlRedes.DataSource = dsProduto.Tables["PESSOAS_FJ_EDI"].DefaultView;
                        ddlRedes.DataBind();
                    }
                }
            }

            // ==============================
            // CARREGA REDE EM DESTAQUE (S)
            // ==============================
            int codRede = 0;
            if (ddlRedes.SelectedValue != null)
                codRede = Funcoes.strToInt(ddlRedes.SelectedValue.ToString());

            using (SqlConnection mySelCadastro = new SqlConnection(Funcoes.conexao()))
            using (SqlCommand cmdSelCadastro = new SqlCommand("dbo.stp_pessoas_fj_edi_ins", mySelCadastro))
            {
                cmdSelCadastro.CommandType = CommandType.StoredProcedure;
                cmdSelCadastro.Parameters.Add("@flg_operacao", SqlDbType.Char).Value = "S";
                cmdSelCadastro.Parameters.Add("@COD_ID", SqlDbType.Int).Value = codRede;
                cmdSelCadastro.Parameters.Add("@COD_ID_PESSOA_LICENCIADO", SqlDbType.Int).Value = Licenciado;

                mySelCadastro.Open();
                using (SqlDataReader ReaderCadastro = cmdSelCadastro.ExecuteReader())
                {
                    if (ReaderCadastro.Read())
                    {
                        txtUSER.Text = ReaderCadastro["NOM_USER"] != DBNull.Value ? ReaderCadastro["NOM_USER"].ToString() : "";
                        txtTOKEN.Text = ReaderCadastro["NOM_TOKEN"] != DBNull.Value ? ReaderCadastro["NOM_TOKEN"].ToString() : "";
                    }
                }
            }
        }
    }


    // =========================================================
    // CHAVES DE CONTROLE DE PROGRESSO (POR USUÁRIO)
    // =========================================================
    private string KeyTotalImport()
    {
        return "IMPORT_TOTAL_" + Session.SessionID;
    }

    private string KeyAtualImport()
    {
        return "IMPORT_ATUAL_" + Session.SessionID;
    }

    protected void btnCarregar_Click(object sender, EventArgs e)
    {
        // ==============================
        // AUDITORIA SEGURA
        // ==============================

        Funcoes.GravaAuditoria(0, ip, "Dashboard - Extrato EDI", "Consulta - Carregar");

        // ==============================
        // MONTA DATA (yyyy-MM-dd)
        // ==============================
        DateTime dataIni;

        if (!DateTime.TryParse(txtDataIni.Text, out dataIni))
        {
            ClientScript.RegisterStartupScript(this.GetType(),
                "DataInvalida",
                "alert('Data inicial inválida. Verifique e tente novamente.');",
                true);
            return;
        }

        string sData = dataIni.Year.ToString().PadLeft(4, '0') + "-" +
                       dataIni.Month.ToString().PadLeft(2, '0') + "-" +
                       dataIni.Day.ToString().PadLeft(2, '0');

        // ==============================
        // CHAMADA DA API
        // ==============================
        string usuario = txtUSER.Text != null ? txtUSER.Text.ToString() : "";
        string token = txtTOKEN.Text != null ? txtTOKEN.Text.ToString() : "";
        string operacao = ddlOperacao.SelectedValue != null
                                ? ddlOperacao.SelectedValue.ToString()
                                : "";

        string jsonRetorno = pagseguro.ConsultarExtratoEDI(
                                usuario,
                                token,
                                sData,
                                1,
                                operacao);

        try
        {
            if (!string.IsNullOrWhiteSpace(jsonRetorno))
            {
                JObject oTransacoes = JObject.Parse(jsonRetorno);

                if (oTransacoes["pagination"] != null)
                {
                    txtTransacoes.Text = oTransacoes["pagination"]["totalElements"] != null
                                            ? oTransacoes["pagination"]["totalElements"].ToString()
                                            : "0";

                    txtPaginas.Text = oTransacoes["pagination"]["totalPages"] != null
                                            ? oTransacoes["pagination"]["totalPages"].ToString()
                                            : "0";
                }
            }
        }
        catch
        {
            ClientScript.RegisterStartupScript(this.GetType(),
                "ErroCarregandoDados",
                "alert('Não existem dados a serem importados! Verifique e tente novamente.');",
                true);
        }

        // ==============================
        // EXIBE RETORNO
        // ==============================
        txtExtrato.Text = txtExtrato.Text +
                          (jsonRetorno != null ? jsonRetorno.ToString() : "");
    }

    protected void btnCancelar_Click(object sender, EventArgs e)
    {
        ClientScript.RegisterStartupScript(this.GetType(),
    "Alerta", "opener.PostBackOnMainPage(); window.close(); ", true);

    }


    /*
    protected void btnImportar_Click(object sender, System.EventArgs e)
    {

        dtTransacoes.Rows.Clear();

        if (txtPaginas.Text.ToString().Trim() == "")
        {
            ClientScript.RegisterStartupScript(this.GetType(),"ErroCarregarDados", "alert('Antes de importar é necessário carregar os dados! Verifique e tente novamente');", true);
            return;
        }
        try
        {
            string sData = Convert.ToDateTime(txtDataIni.Text.ToString()).Year.ToString().PadLeft(4, '0') + "-" + Convert.ToDateTime(txtDataIni.Text.ToString()).Month.ToString().PadLeft(2, '0') + "-" + Convert.ToDateTime(txtDataIni.Text.ToString()).Day.ToString().PadLeft(2, '0');
            txtExtrato.Text = "";

            int iPaginas = Funcoes.strToInt(txtPaginas.Text.ToString());


            for (int x = 0; x < iPaginas; x++)
            {

                string jsonRetorno = pagseguro.ConsultarExtratoEDI(txtUSER.Text.ToString(), txtTOKEN.Text.ToString(), sData.ToString(), x + 1, ddlOperacao.SelectedValue.ToString());

                if (jsonRetorno.ToString().Trim() != "")
                {
                    JObject oTransacoes = JObject.Parse(jsonRetorno);

                    if (oTransacoes["detalhes"].Count() > 0)
                    {

                        for (int i = 0; i < oTransacoes["detalhes"].Count(); i++)
                        {
                            txtExtrato.Text = txtExtrato.Text + ((i + 1) + (x * 1000)).ToString() + ":" + oTransacoes["detalhes"][i]["codigo_transacao"].ToString() + "<br />";
                            // Processar as transações

                            try
                            {
                                dtTransacoes.Rows.Add(oTransacoes["detalhes"][i]["estabelecimento"].ToString(), oTransacoes["detalhes"][i]["codigo_transacao"].ToString(), oTransacoes["detalhes"][i]["data_inicial_transacao"].ToString(), oTransacoes["detalhes"][i]["valor_original_transacao"].ToString());





                                SqlConnection connInsCons = new SqlConnection(Funcoes.conexao());
                                connInsCons.Open();
                                SqlCommand cmdInsCons = new SqlCommand("dbo.stp_transacoes_ins", connInsCons);
                                cmdInsCons.CommandType = CommandType.StoredProcedure;
                                cmdInsCons.Parameters.Add("@flg_operacao", SqlDbType.Char).Value = '!';
                                cmdInsCons.Parameters.Add("@COD_ID_PESSOA_LICENCIADO", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["LICENCIADO"].ToString());
                                cmdInsCons.Parameters.Add("@COD_ID_PAGSEGURO", SqlDbType.Int).Value = Funcoes.strToInt(oTransacoes["detalhes"][i]["estabelecimento"].ToString());
                                //cmdInsCons.Parameters.Add("@COD_ID_PLANO", SqlDbType.Int).Value = Funcoes.strToInt(ReaderCadastro["COD_ID_PLANO"].ToString());
                                cmdInsCons.Parameters.Add("@FLG_ORIGEM", SqlDbType.Char).Value = "P";
                                //txtResultado.Text = txtResultado.Text + dataDictionary["transaction.date"] + "\n";
                                cmdInsCons.Parameters.Add("@DTA_DATA", SqlDbType.DateTime).Value = Convert.ToDateTime(oTransacoes["detalhes"][i]["data_inicial_transacao"].ToString());
                                cmdInsCons.Parameters.Add("@NOM_CODE", SqlDbType.VarChar).Value = oTransacoes["detalhes"][i]["codigo_transacao"].ToString();
                                cmdInsCons.Parameters.Add("@COD_ID_TIPO", SqlDbType.Int).Value = Funcoes.strToInt(oTransacoes["detalhes"][i]["tipo_transacao"].ToString());
                                cmdInsCons.Parameters.Add("@COD_ID_STATUS", SqlDbType.Int).Value = Funcoes.strToInt(oTransacoes["detalhes"][i]["status_pagamento"].ToString());

                                cmdInsCons.Parameters.Add("@NOM_STATUS", SqlDbType.VarChar).Value = ObterStatus(oTransacoes["detalhes"][i]["status_pagamento"].ToString());
                                // Localizar na tabela de status da PAGSEGURO

                                cmdInsCons.Parameters.Add("@DTA_DATA_ULTIMA_ATUALIZACAO", SqlDbType.DateTime).Value = Convert.ToDateTime(oTransacoes["detalhes"][i]["data_venda_ajuste"].ToString());

                                cmdInsCons.Parameters.Add("@COD_ID_TIPO_PAGAMENTO", SqlDbType.Int).Value = Funcoes.strToInt(oTransacoes["detalhes"][i]["meio_pagamento"].ToString());
                                // Localizar o CODIGO PAGAMENTO e enviar como NOM_TIPO_OPERACAO
                                cmdInsCons.Parameters.Add("@NOM_TIPO_OPERACAO", SqlDbType.VarChar).Value = RetornaTipoPagamento(oTransacoes["detalhes"][i]["meio_pagamento"].ToString());

                                cmdInsCons.Parameters.Add("@COD_ID_CODIGO_PAGAMENTO", SqlDbType.Int).Value = Funcoes.strToInt(oTransacoes["detalhes"][i]["meio_pagamento"].ToString());
                                // Localizar o CODIGO PAGAMENTO e enviar como NOM_TIPO_OPERACAO_PAGAMENTO

                                cmdInsCons.Parameters.Add("@NOM_TIPO_OPERACAO_PAGAMENTO", SqlDbType.VarChar).Value = RetornaCodigoPagamento(oTransacoes["detalhes"][i]["meio_pagamento"].ToString());
                                string sBandeira = "";
                                try
                                { 
                                    sBandeira = ObterBandeiraCartao(oTransacoes["detalhes"][i]["cartao_bin"].ToString()); 
                                }
                                catch
                                {
                                   sBandeira = RetornaCodigoPagamento(oTransacoes["detalhes"][i]["meio_pagamento"].ToString());
                                }
                                cmdInsCons.Parameters.Add("@NOM_TIPO_OPERACAO_BANDEIRA", SqlDbType.VarChar).Value = sBandeira.ToString();

                                cmdInsCons.Parameters.Add("@NUM_VALOR_BRUTO", SqlDbType.Float).Value = Funcoes.strToDouble(oTransacoes["detalhes"][i]["valor_original_transacao"].ToString());
                                cmdInsCons.Parameters.Add("@NUM_VALOR_DESCONTO", SqlDbType.Float).Value = 0;
                                cmdInsCons.Parameters.Add("@NUM_VALOR_INTERMEDIACAO_RATE", SqlDbType.Float).Value = Funcoes.strToDouble(oTransacoes["detalhes"][i]["taxa_intermediacao"].ToString());
                                cmdInsCons.Parameters.Add("@NUM_VALOR_INTERMEDIACAO_FEE", SqlDbType.Float).Value = Funcoes.strToDouble(oTransacoes["detalhes"][i]["tarifa_intermediacao"].ToString());
                                cmdInsCons.Parameters.Add("@NUM_VALOR_TAXA_PARCELAMENTO", SqlDbType.Float).Value = 0;

                                cmdInsCons.Parameters.Add("@NUM_VALOR_LIQUIDO", SqlDbType.Float).Value = Funcoes.strToDouble(oTransacoes["detalhes"][i]["valor_liquido_transacao"].ToString());
                                cmdInsCons.Parameters.Add("@NUM_VALOR_EXTRAS", SqlDbType.Float).Value = 0;

                                cmdInsCons.Parameters.Add("@NUM_PARCELAS", SqlDbType.Int).Value = Funcoes.strToInt(oTransacoes["detalhes"][i]["quantidade_parcelas"].ToString());


                                //cmdInsCons.Parameters.Add("@DTA_DATA_DEPOSITO", SqlDbType.DateTime).Value = (dataDictionary.ContainsKey("transaction.escrowEndDate")) ? Convert.ToDateTime(dataDictionary["transaction.escrowEndDate"]) : dt;

                                //cmdInsCons.Parameters.Add("@NUM_ITENS", SqlDbType.Int).Value = (dataDictionary.ContainsKey("transaction.itemCount")) ? Funcoes.strToInt(dataDictionary["transaction.itemCount"]) : 0;

                                //cmdInsCons.Parameters.Add("@NOM_FONTE_CANCELAMENTO", SqlDbType.VarChar).Value = (dataDictionary.ContainsKey("transaction.cancellationSource")) ? dataDictionary["transaction.cancellationSource"] : "";
                                try
                                {
                                    cmdInsCons.Parameters.Add("@NUM_CHAVE_PUBLICA", SqlDbType.VarChar).Value = oTransacoes["detalhes"][i]["nsu"].ToString();
                                    cmdInsCons.Parameters.Add("@NUM_DEVICE_REFERENCIA", SqlDbType.VarChar).Value = "";
                                    cmdInsCons.Parameters.Add("@NUM_DEVICE_BIN", SqlDbType.Char).Value = oTransacoes["detalhes"][i]["cartao_bin"].ToString();
                                    cmdInsCons.Parameters.Add("@NUM_DEVICE_TITULAR", SqlDbType.VarChar).Value = oTransacoes["detalhes"][i]["cartao_holder"].ToString();
                                }
                                catch
                                {

                                }

                                try
                                {
                                    cmdInsCons.Parameters.Add("@NUM_DEVICE_SERIAL", SqlDbType.VarChar).Value = oTransacoes["detalhes"][i]["numero_serie_leitor"].ToString();
                                }
                                catch
                                {
                                }

                                //cmdInsCons.Parameters.Add("@DTA_PIX", SqlDbType.DateTime).Value = Convert.ToDateTime(oTransacoes["detalhes"][i]["data_inicial_transacao"].ToString());
                                /*
                                cmdInsCons.Parameters.Add("@NOM_PIX_NOME", SqlDbType.VarChar).Value = (dataDictionary.ContainsKey("transaction.pix.holderName")) ? dataDictionary["transaction.pix.holderName"] : "";
                                cmdInsCons.Parameters.Add("@NOM_PIX_TIPO_FJ", SqlDbType.VarChar).Value = (dataDictionary.ContainsKey("transaction.pix.personType")) ? dataDictionary["transaction.pix.personType"] : "";
                                cmdInsCons.Parameters.Add("@NOM_PIX_NOME_BANCO", SqlDbType.VarChar).Value = (dataDictionary.ContainsKey("transaction.pix.bankName")) ? dataDictionary["transaction.pix.bankName"] : "";
                                cmdInsCons.Parameters.Add("@NOM_PIX_AGENCIA_BANCO", SqlDbType.VarChar).Value = (dataDictionary.ContainsKey("transaction.pix.bankAgency")) ? dataDictionary["transaction.pix.bankAgency"] : "";
                                cmdInsCons.Parameters.Add("@NOM_PIX_CONTA_BANCO", SqlDbType.VarChar).Value = (dataDictionary.ContainsKey("transaction.pix.bankAccount")) ? dataDictionary["transaction.pix.bankAccount"] : "";
                                cmdInsCons.Parameters.Add("@NOM_PIX_TIPO_CONTA_BANCO", SqlDbType.VarChar).Value = (dataDictionary.ContainsKey("transaction.pix.bankAccountType")) ? dataDictionary["transaction.pix.bankAccountType"] : "";
                                cmdInsCons.Parameters.Add("@DES_DESCRICAO", SqlDbType.Text).Value = (dataDictionary.ContainsKey("transaction.items.item.description")) ? dataDictionary["transaction.items.item.description"] : "";
                                */
                                /*        
                                cmdInsCons.ExecuteNonQuery();
                                connInsCons.Close();
                                connInsCons.Dispose();
                            }
                            catch
                            {
                                txtExtrato.Text = "Erro na operação" + oTransacoes["detalhes"][i]["codigo_transacao"].ToString();
                            }
                        }
                    }
                }
            }

            this.rptConsulta.DataSource = dtTransacoes;
            this.rptConsulta.DataBind();

        }
        catch
        {
        }
    }
    */

    // =========================================================
    // BOTÃO IMPORTAR
    // =========================================================
    protected void btnImportar_Click(object sender, EventArgs e)
    {
        Funcoes.GravaAuditoria(0, Request.ServerVariables["REMOTE_ADDR"],
            "Dashboard - Extrato EDI", "Consulta - Importar");

        txtExtrato.Text = "";

        if (dtTransacoes != null)
            dtTransacoes.Rows.Clear();

        // =========================
        // VALIDAÇÕES
        // =========================
        int paginasInformadas = Funcoes.strToInt(
            txtPaginas.Text != null ? txtPaginas.Text.Trim() : "0");

        if (paginasInformadas <= 0)
        {
            Alert("Antes de importar é necessário carregar os dados!");
            return;
        }

        DateTime dataIni;
        if (!DateTime.TryParse(txtDataIni.Text, out dataIni))
        {
            Alert("Data inválida!");
            return;
        }

        string sData = dataIni.ToString("yyyy-MM-dd");

        string usuario = txtUSER.Text != null ? txtUSER.Text : "";
        string token = txtTOKEN.Text != null ? txtTOKEN.Text : "";
        string operacao = ddlOperacao.SelectedValue != null
                            ? ddlOperacao.SelectedValue.ToString()
                            : "";

        int totalRegistros = 0;
        int iPaginas = paginasInformadas;

        // =========================
        // PRIMEIRA CHAMADA
        // =========================
        string jsonRetorno = pagseguro.ConsultarExtratoEDI(
                                usuario, token, sData, 1, operacao);

        if (!string.IsNullOrWhiteSpace(jsonRetorno))
        {
            JObject oTransacoes = JObject.Parse(jsonRetorno);

            if (oTransacoes["pagination"] != null)
            {
                if (oTransacoes["pagination"]["totalElements"] != null)
                    totalRegistros = Funcoes.strToInt(
                        oTransacoes["pagination"]["totalElements"].ToString());

                if (oTransacoes["pagination"]["totalPages"] != null)
                    iPaginas = Funcoes.strToInt(
                        oTransacoes["pagination"]["totalPages"].ToString());
            }
        }

        // =========================
        // INICIALIZA PROGRESSO
        // =========================
        string keyTotal = KeyTotalImport();
        string keyAtual = KeyAtualImport();

        Application[keyTotal] = totalRegistros;
        Application[keyAtual] = 0;

        int processados = 0;
        StringBuilder sbExtrato = new StringBuilder();

        // =========================
        // PROCESSAMENTO
        // =========================
        for (int x = 0; x < iPaginas; x++)
        {
            jsonRetorno = pagseguro.ConsultarExtratoEDI(
                                usuario, token, sData, x + 1, operacao);

            if (string.IsNullOrWhiteSpace(jsonRetorno))
                continue;

            JObject oTransacoes = JObject.Parse(jsonRetorno);

            if (oTransacoes["detalhes"] == null)
                continue;

            using (SqlConnection conn = new SqlConnection(Funcoes.conexao()))
            {
                conn.Open();

                using (SqlCommand cmd = new SqlCommand("dbo.stp_transacoes_ins", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    // cria parâmetros uma vez
                    cmd.Parameters.Add("@flg_operacao", SqlDbType.Char);
                    cmd.Parameters.Add("@COD_ID_PESSOA_LICENCIADO", SqlDbType.Int);
                    cmd.Parameters.Add("@COD_ID_PAGSEGURO", SqlDbType.Int);
                    cmd.Parameters.Add("@FLG_ORIGEM", SqlDbType.Char);
                    cmd.Parameters.Add("@DTA_DATA", SqlDbType.DateTime);
                    cmd.Parameters.Add("@NOM_CODE", SqlDbType.VarChar);
                    cmd.Parameters.Add("@COD_ID_TIPO", SqlDbType.Int);
                    cmd.Parameters.Add("@COD_ID_STATUS", SqlDbType.Int);
                    cmd.Parameters.Add("@NOM_STATUS", SqlDbType.VarChar);

                    foreach (JToken item in oTransacoes["detalhes"])
                    {
                        string codigo = item["codigo_transacao"] != null
                                            ? item["codigo_transacao"].ToString()
                                            : "";

                        string estab = item["estabelecimento"] != null
                                            ? item["estabelecimento"].ToString()
                                            : "";

                        string dataStr = item["data_inicial_transacao"] != null
                                            ? item["data_inicial_transacao"].ToString()
                                            : "";

                        DateTime dataBanco;
                        if (!DateTime.TryParse(dataStr, out dataBanco))
                            dataBanco = DateTime.Now;

                        string tipo = item["tipo_transacao"] != null
                                            ? item["tipo_transacao"].ToString()
                                            : "0";

                        string status = item["status_pagamento"] != null
                                            ? item["status_pagamento"].ToString()
                                            : "0";

                        // --------- UI ----------
                        sbExtrato.Append((processados + 1) + ":" + codigo + "<br />");

                        if (dtTransacoes != null)
                            dtTransacoes.Rows.Add(estab, codigo, dataStr, "");

                        // --------- DB ----------
                        cmd.Parameters["@flg_operacao"].Value = '!';
                        cmd.Parameters["@COD_ID_PESSOA_LICENCIADO"].Value = Licenciado;
                        cmd.Parameters["@COD_ID_PAGSEGURO"].Value = Funcoes.strToInt(estab);
                        cmd.Parameters["@FLG_ORIGEM"].Value = "P";
                        cmd.Parameters["@DTA_DATA"].Value = dataBanco;
                        cmd.Parameters["@NOM_CODE"].Value = codigo;
                        cmd.Parameters["@COD_ID_TIPO"].Value = Funcoes.strToInt(tipo);
                        cmd.Parameters["@COD_ID_STATUS"].Value = Funcoes.strToInt(status);
                        cmd.Parameters["@NOM_STATUS"].Value = ObterStatus(status);

                        cmd.ExecuteNonQuery();

                        // --------- PROGRESSO ----------
                        processados++;
                        Application[keyAtual] = processados;
                    }
                }
            }
        }

        txtExtrato.Text = sbExtrato.ToString();

        rptConsulta.DataSource = dtTransacoes;
        rptConsulta.DataBind();
    }


    public static string RetornaTipoPagamento(string sTipoPagamento)
    {
        string sRetorno = "";
        switch (sTipoPagamento)
        {
            case "1":
                sRetorno = "Crédito";
                break;
            case "2":
                sRetorno = "Boleto";
                break;
            case "3":
                sRetorno = "Débito";
                break;
            case "4":
                sRetorno = "Débito";
                break;
            //case "5":
            //    sTipoPagamento = "Oi Paggo";
            //    break;
            //case "7":
            //    sTipoPagamento = "Depósito em conta";
            //    break;
            case "8":
                sRetorno = "Débito";
                break;
            case "11":
                sRetorno = "Pix";
                break;
            default:
                sRetorno = "Não Identificado";
                break;
        }
        return sRetorno.ToString();
    }

    public static string RetornaCodigoPagamento(string sCodigoPagamento)
    {

        string sRetorno = "";
        switch (sCodigoPagamento)
        {

            case "101":
                sRetorno = "Cartão de crédito Visa";
                break;
            case "102":
                sRetorno = "Cartão de crédito MasterCard";
                break;
            case "103":
                sRetorno = "Cartão de crédito American Express";
                break;
            case "104":
                sRetorno = "Cartão de crédito Diners";
                break;
            case "105":
                sRetorno = "Cartão de crédito Hipercard";
                break;
            case "106":
                sRetorno = "Cartão de crédito Aura";
                break;
            case "107":
                sRetorno = "Cartão de crédito Elo";
                break;
            case "108":
                sRetorno = "Cartão de crédito PLENOCard";
                break;
            case "109":
                sRetorno = "Cartão de crédito PersonalCard";
                break;
            case "110":
                sRetorno = "Cartão de crédito JCB";
                break;
            case "111":
                sRetorno = "Cartão de crédito Discover";
                break;
            case "112":
                sRetorno = "Cartão de crédito BrasilCard";
                break;
            case "113":
                sRetorno = "Cartão de crédito FORTBRASIL";
                break;
            case "114":
                sRetorno = "Cartão de crédito CARDBAN";
                break;
            case "115":
                sRetorno = "Cartão de crédito VALECARD";
                break;
            case "116":
                sRetorno = "Cartão de crédito Cabal";
                break;
            case "117":
                sRetorno = "Cartão de crédito Mais";
                break;
            case "118":
                sRetorno = "Cartão de crédito Avista";
                break;
            case "119":
                sRetorno = "Cartão de crédito GRANDCARD";
                break;
            case "120":
                sRetorno = "Cartão de crédito Sorocred";
                break;
            case "122":
                sRetorno = "Cartão de crédito Up Policard";
                break;
            case "123":
                sRetorno = "Cartão de crédito Banese Card";
                break;
            case "201":
                sRetorno = "Boleto Bradesco";
                break;
            case "202":
                sRetorno = "Boleto Santander";
                break;
            case "301":
                sRetorno = "Débito online Bradesco";
                break;
            case "302":
                sRetorno = "Débito online Itaú";
                break;
            case "303":
                sRetorno = "Débito online Unibanco";
                break;
            case "304":
                sRetorno = "Débito online Banco do Brasil";
                break;
            case "305":
                sRetorno = "Débito online Banco Real";
                break;
            case "306":
                sRetorno = "Débito online Banrisul";
                break;
            case "307":
                sRetorno = "Débito online HSBC";
                break;
            case "401":
                sRetorno = "Saldo PagSeguro";
                break;
            case "402":
                sRetorno = "PIX";
                break;
            case "501":
                sRetorno = "Oi Paggo";
                break;
            case "701":
                sRetorno = "Depósito em conta - Banco do Brasil";
                break;
            case "802":
                sRetorno = "Cartão Auxílio Emergencial Mastercard";
                break;
            case "801":
                sRetorno = "Cartão Auxílio Emergencial Visa";
                break;
            case "803":
                sRetorno = "Cartão Auxílio Emergencial American Express";
                break;
            case "804":
                sRetorno = "Cartão de crédito Diners";
                break;
            case "805":
                sRetorno = "Cartão de crédito Hipercard";
                break;
            case "806":
                sRetorno = "Cartão de crédito Aura";
                break;
            case "807":
                sRetorno = "Cartão de crédito Elo";
                break;
            default:
                sRetorno = "Não Identificado";
                break;
        }
        return sRetorno.ToString();

    }


    public static string ObterBandeiraCartao(string bin)
    {
        if (string.IsNullOrEmpty(bin) || bin.Length < 6)
            return "";

        if (bin.StartsWith("384100") || bin.StartsWith("384140") || bin.StartsWith("384160") || bin.StartsWith("606282") || bin.StartsWith("637095") || bin.StartsWith("637568")
            || bin.StartsWith("637599") || bin.StartsWith("637609") || bin.StartsWith("637612"))
            return "Hipercard";

        else if (bin.StartsWith("4011") || bin.StartsWith("4389") || bin.StartsWith("4514") || bin.StartsWith("4576") || bin.StartsWith("5041") || bin.StartsWith("506")
            || bin.StartsWith("509") || bin.StartsWith("636") || bin.StartsWith("650") || bin.StartsWith("651") || bin.StartsWith("655"))
            return "Elo";

        else if (bin.StartsWith("40240071") || bin.StartsWith("4539") || bin.StartsWith("4556") || bin.StartsWith("4916") || bin.StartsWith("4532") || bin.StartsWith("4929") || bin.StartsWith("4485") || bin.StartsWith("4716"))
            return "Visa";

        //else if (bin >= "222100" && bin <= "272099")
        //    return "MasterCard";

        else if (bin.StartsWith("36") || bin.StartsWith("300") || bin.StartsWith("305") || bin.StartsWith("3095") || bin.StartsWith("38") || bin.StartsWith("39"))
            return "Diners";

        else if (bin.StartsWith("6369"))
            return "Banescard";

        else if (bin.StartsWith("50"))
            return "MasterCard";

        else if (bin.StartsWith("5078"))
            return "Aura";

        else if (bin.StartsWith("6042") || bin.StartsWith("589657"))
            return "Cabal";

        else if (bin.StartsWith("4"))
            return "Visa";

        else if (bin.StartsWith("5") && "12345".Contains(bin[1]))
            return "MasterCard";

        else if (bin.StartsWith("34") || bin.StartsWith("37"))
            return "American Express";

        else if (bin.StartsWith("6"))
            return "MasterCard";

        else if (bin.StartsWith("35"))
            return "JCB";

        else if (bin.StartsWith("30") || bin.StartsWith("36") || bin.StartsWith("38"))
            return "Diners Club";
        else
            return "Outros";
    }
    public static string ObterStatus(string sCodigoStatus)
    {
        string sRetorno = "";
        switch (sCodigoStatus)
        {

            case "1":
                sRetorno = "Aguardando pagamento";
                break;
            case "2":
                sRetorno = "Em análise";
                break;
            case "3":
                sRetorno = "Paga";
                break;
            case "4":
                sRetorno = "Disponível";
                break;
            case "5":
                sRetorno = "Em Disputa";
                break;
            case "6":
                sRetorno = "Devolvida";
                break;
            case "7":
                sRetorno = "Cancelada";
                break;
            case "8":
                sRetorno = "Debitado";
                break;
            case "9":
                sRetorno = "Retenção temporária";
                break;
            default:
                sRetorno = "Não Identificado";
                break;
        }
        return sRetorno.ToString();

    }

    protected void ddlRedes_SelectedIndexChanged(object sender, System.EventArgs e)
    {
        // ==============================
        // VALIDAÇÕES BÁSICAS
        // ==============================
        if (ddlRedes == null || ddlRedes.SelectedValue == null)
            return;

        int codId = Funcoes.strToInt(ddlRedes.SelectedValue.ToString());

        if (codId <= 0)
            return;

        // ==============================
        // ACESSO AO BANCO (USING)
        // ==============================
        using (SqlConnection mySelCadastro = new SqlConnection(Funcoes.conexao()))
        {
            mySelCadastro.Open();

            using (SqlCommand cmdSelCadastro = new SqlCommand("dbo.stp_pessoas_fj_edi_ins", mySelCadastro))
            {
                cmdSelCadastro.CommandType = CommandType.StoredProcedure;

                cmdSelCadastro.Parameters.Add("@flg_operacao", SqlDbType.Char).Value = "S";
                cmdSelCadastro.Parameters.Add("@COD_ID", SqlDbType.Int).Value = codId;

                // 👉 usa a propriedade já carregada da Session
                cmdSelCadastro.Parameters.Add("@COD_ID_PESSOA_LICENCIADO", SqlDbType.Int).Value = Licenciado;

                using (SqlDataReader ReaderCadastro = cmdSelCadastro.ExecuteReader())
                {
                    if (ReaderCadastro != null && ReaderCadastro.HasRows)
                    {
                        while (ReaderCadastro.Read())
                        {
                            txtUSER.Text = ReaderCadastro["NOM_USER"] != null
                                                ? ReaderCadastro["NOM_USER"].ToString()
                                                : "";

                            txtTOKEN.Text = ReaderCadastro["NOM_TOKEN"] != null
                                                ? ReaderCadastro["NOM_TOKEN"].ToString()
                                                : "";
                        }
                    }
                    else
                    {
                        // limpa se não encontrou nada
                        txtUSER.Text = "";
                        txtTOKEN.Text = "";
                    }
                }
            }
        }
    }

    // =========================================================
    // WEBMETHOD PARA PROGRESSO (AJAX)
    // =========================================================
    [System.Web.Services.WebMethod]
    public static object ProgressoImportacao()
    {
        HttpContext ctx = HttpContext.Current;
        if (ctx == null || ctx.Session == null)
            return new { total = 0, atual = 0 };

        string keyTotal = "IMPORT_TOTAL_" + ctx.Session.SessionID;
        string keyAtual = "IMPORT_ATUAL_" + ctx.Session.SessionID;

        int total = ctx.Application[keyTotal] != null
                        ? Convert.ToInt32(ctx.Application[keyTotal])
                        : 0;

        int atual = ctx.Application[keyAtual] != null
                        ? Convert.ToInt32(ctx.Application[keyAtual])
                        : 0;

        return new { total = total, atual = atual };
    }

    // =========================================================
    // ALERTA SIMPLES
    // =========================================================
    private void Alert(string msg)
    {
        ClientScript.RegisterStartupScript(this.GetType(),
            Guid.NewGuid().ToString(),
            "alert('" + msg.Replace("'", "") + "');",
            true);
    }

}