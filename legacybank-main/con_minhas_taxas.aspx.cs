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

public partial class con_minhas_taxas : System.Web.UI.Page
{
    protected async void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            await ConsultaPlano();
        }
    }

    private async Task ConsultaPlano()
    {
        int iPlano = 0;

        //txtTabela.Text = "Aguarde... carregando tabela de taxas";

        SqlConnection mySelCadastro = new SqlConnection(Funcoes.conexao());
        mySelCadastro.Open();
        SqlCommand cmdSelCadastro = new SqlCommand("dbo.stp_planos_ins", mySelCadastro);
        cmdSelCadastro.CommandType = CommandType.StoredProcedure;
        cmdSelCadastro.Parameters.Add("@flg_operacao", SqlDbType.Char).Value = "L";
        cmdSelCadastro.Parameters.Add("@COD_ID_PESSOAS_FJ", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["PESSOA"].ToString());
        cmdSelCadastro.Parameters.Add("@COD_ID_PESSOA_LICENCIADO", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["LICENCIADO"].ToString());
        cmdSelCadastro.Parameters.Add("@FLG_MODELO_PLANO", SqlDbType.Char).Value = "A";
        cmdSelCadastro.Parameters.Add("@FLG_ATIVO", SqlDbType.Char).Value = "S";

        SqlDataReader ReaderCadastro = cmdSelCadastro.ExecuteReader();
        while (ReaderCadastro.Read())
        {
            //txtTabela.Text = ReaderCadastro["NOM_TITULO_PLANO"].ToString();
            await ConsultarTaxas(ReaderCadastro["COD_ID"].ToString());
            iPlano = 1;
        }

        if (iPlano == 0)
        {
            // Importar
            // txtTabela.Text = asaas.ListarTaxas(asaas.PegarTokenSubconta(Funcoes.strToInt(HttpContext.Current.Session["PESSOA"].ToString()), Funcoes.strToInt(HttpContext.Current.Session["LICENCIADO"].ToString())));
            await ImportarTaxas();
        }
    }

    private async Task ConsultarTaxas(string sPlano)
    {
        dvParcelas.Visible = true;
        dvParcelasOnline.Visible = true;

        SqlConnection myConsultaParcelas = new SqlConnection(Funcoes.conexao());
        myConsultaParcelas.Open();
        SqlDataAdapter SDAConsultaParcelas = new SqlDataAdapter("dbo.stp_planos_ins", myConsultaParcelas);
        SDAConsultaParcelas.SelectCommand.CommandType = CommandType.StoredProcedure;
        SDAConsultaParcelas.SelectCommand.Parameters.Add("@flg_operacao", SqlDbType.Char).Value = "P";
        SDAConsultaParcelas.SelectCommand.Parameters.Add("@COD_ID_PESSOA_LICENCIADO", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["LICENCIADO"].ToString());
        SDAConsultaParcelas.SelectCommand.Parameters.Add("@COD_ID", SqlDbType.Int).Value = Funcoes.strToInt(sPlano.ToString());
        SDAConsultaParcelas.SelectCommand.Parameters.Add("@FLG_PRESENCIAL_ONLINE", SqlDbType.Char).Value = "P";
        DataSet dsConsultaParcelas = new DataSet();
        SDAConsultaParcelas.Fill(dsConsultaParcelas, "PLANOS_PARCELAS");
        rptConsultaParcelas.DataSource = dsConsultaParcelas.Tables["PLANOS_PARCELAS"].DefaultView;
        rptConsultaParcelas.DataBind();
        myConsultaParcelas.Close(); myConsultaParcelas.Dispose();

        SqlConnection myConsultaParcelasOnLine = new SqlConnection(Funcoes.conexao());
        myConsultaParcelasOnLine.Open();
        SqlDataAdapter SDAConsultaParcelasOnLine = new SqlDataAdapter("dbo.stp_planos_ins", myConsultaParcelasOnLine);
        SDAConsultaParcelasOnLine.SelectCommand.CommandType = CommandType.StoredProcedure;
        SDAConsultaParcelasOnLine.SelectCommand.Parameters.Add("@flg_operacao", SqlDbType.Char).Value = "P";
        SDAConsultaParcelasOnLine.SelectCommand.Parameters.Add("@COD_ID_PESSOA_LICENCIADO", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["LICENCIADO"].ToString());
        SDAConsultaParcelasOnLine.SelectCommand.Parameters.Add("@COD_ID", SqlDbType.Int).Value = Funcoes.strToInt(sPlano.ToString());
        SDAConsultaParcelasOnLine.SelectCommand.Parameters.Add("@FLG_PRESENCIAL_ONLINE", SqlDbType.Char).Value = "O";
        DataSet dsConsultaParcelasOnLine = new DataSet();
        SDAConsultaParcelasOnLine.Fill(dsConsultaParcelasOnLine, "PLANOS_PARCELAS");
        rptConsultaParcelasOnline.DataSource = dsConsultaParcelasOnLine.Tables["PLANOS_PARCELAS"].DefaultView;
        rptConsultaParcelasOnline.DataBind();
        myConsultaParcelasOnLine.Close(); myConsultaParcelasOnLine.Dispose();

    
    }

    private async Task ImportarTaxas()
    {
        string jsonResposta = asaas.ListarTaxas(asaas.PegarTokenSubconta(Funcoes.strToInt(HttpContext.Current.Session["pessoa"].ToString()), Funcoes.strToInt(HttpContext.Current.Session["LICENCIADO"].ToString()))).ToString();
        if (jsonResposta.ToString().Trim() != "")
        {
            try
            {
                JObject oPlano = JObject.Parse(jsonResposta);
                //txtJson.Text = jsonResposta.ToString();
                //txtJson.Visible = true;

                SqlConnection connInsCons = new SqlConnection(Funcoes.conexao());
                connInsCons.Open();
                SqlCommand cmdInsCons = new SqlCommand("dbo.stp_planos_ins", connInsCons);
                cmdInsCons.CommandType = CommandType.StoredProcedure;
                cmdInsCons.Parameters.Add("@flg_operacao", SqlDbType.Char).Value = 'I';

                cmdInsCons.Parameters.Add("@COD_ID_PESSOA_LICENCIADO", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["LICENCIADO"].ToString());
                cmdInsCons.Parameters.Add("@COD_ID_PESSOAS_FJ", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["PESSOA"].ToString());

                cmdInsCons.Parameters.Add("@FLG_MODELO_PLANO", SqlDbType.Char).Value = "A";
                cmdInsCons.Parameters.Add("@FLG_ATIVO", SqlDbType.Char).Value = "S";
                cmdInsCons.Parameters.Add("@FLG_VISIVEL", SqlDbType.Char).Value = "S";
                cmdInsCons.Parameters.Add("@FLG_TIPO_PLANO", SqlDbType.Char).Value = "C";

                cmdInsCons.Parameters.Add("@COD_ID_PLANO_REFERENCIA", SqlDbType.VarChar).Value = "CONTA DIGITAL";

                cmdInsCons.Parameters.Add("@NOM_TITULO_PLANO", SqlDbType.VarChar).Value = "CONTA DIGITAL";
                cmdInsCons.Parameters.Add("@DES_PLANO", SqlDbType.Text).Value = "CONTA DIGITAL";

                cmdInsCons.Parameters.Add("@FLG_ANTECIPADO", SqlDbType.Char).Value = "N";
                cmdInsCons.Parameters.Add("@NUM_DIAS_LIQUIDACAO", SqlDbType.Int).Value = 30;
                cmdInsCons.Parameters.Add("@NUM_TAXA_ANTECIPACAO", SqlDbType.Float).Value = 0;
                string sCodigoInclusao = cmdInsCons.ExecuteScalar().ToString();
                connInsCons.Close();
                connInsCons.Dispose();

                // ---------------------------------------------------------------------------------------------------------------------------------------------------------------
                // SEM ANTECIPAÇÃO
                // ---------------------------------------------------------------------------------------------------------------------------------------------------------------

                // DEBITO 

                SqlConnection mySelCadastroBandeirasDO = new SqlConnection(Funcoes.conexao());
                mySelCadastroBandeirasDO.Open();
                SqlCommand cmdSelCadastroBandeirasDO = new SqlCommand("dbo.stp_bandeiras_ins", mySelCadastroBandeirasDO);
                cmdSelCadastroBandeirasDO.CommandType = CommandType.StoredProcedure;
                cmdSelCadastroBandeirasDO.Parameters.Add("@flg_operacao", SqlDbType.Char).Value = "C";
                cmdSelCadastroBandeirasDO.Parameters.Add("@NOM_BANDEIRA", SqlDbType.VarChar).Value = "%";
                cmdSelCadastroBandeirasDO.Parameters.Add("@FLG_ATIVO", SqlDbType.VarChar).Value = "S";
                cmdSelCadastroBandeirasDO.Parameters.Add("@FLG_TIPO", SqlDbType.VarChar).Value = "V";
                SqlDataReader ReaderCadastroBandeirasDO = cmdSelCadastroBandeirasDO.ExecuteReader();
                while (ReaderCadastroBandeirasDO.Read())
                {
                    if (ReaderCadastroBandeirasDO["NOM_BANDEIRA"].ToString() == "BOLETO")
                    {
                        SqlConnection connInsConsTaxas = new SqlConnection(Funcoes.conexao());
                        connInsConsTaxas.Open();
                        SqlCommand cmdInsConsTaxas = new SqlCommand("dbo.stp_planos_parcelas_ins", connInsConsTaxas);
                        cmdInsConsTaxas.CommandType = CommandType.StoredProcedure;
                        cmdInsConsTaxas.Parameters.Add("@flg_operacao", SqlDbType.Char).Value = 'I';
                        cmdInsConsTaxas.Parameters.Add("@COD_ID_PESSOA_LICENCIADO", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["LICENCIADO"].ToString());
                        cmdInsConsTaxas.Parameters.Add("@COD_ID_PLANOS", SqlDbType.Int).Value = Funcoes.strToInt(sCodigoInclusao);
                        cmdInsConsTaxas.Parameters.Add("@COD_ID_BANDEIRA", SqlDbType.Int).Value = Funcoes.strToInt(ReaderCadastroBandeirasDO["COD_ID"].ToString());
                        cmdInsConsTaxas.Parameters.Add("@FLG_PRESENCIAL_ONLINE", SqlDbType.Char).Value = "O";
                        cmdInsConsTaxas.Parameters.Add("@NUM_VALOR_FIXO_DEBITO", SqlDbType.Float).Value = Funcoes.strToDouble(oPlano["payment"]["bankSlip"]["defaultValue"].ToString());
                        cmdInsConsTaxas.ExecuteNonQuery();
                        connInsConsTaxas.Close();
                        connInsConsTaxas.Dispose();
                    }
                    if (ReaderCadastroBandeirasDO["NOM_BANDEIRA"].ToString() == "PIX")
                    {
                        SqlConnection connInsConsTaxas = new SqlConnection(Funcoes.conexao());
                        connInsConsTaxas.Open();
                        SqlCommand cmdInsConsTaxas = new SqlCommand("dbo.stp_planos_parcelas_ins", connInsConsTaxas);
                        cmdInsConsTaxas.CommandType = CommandType.StoredProcedure;
                        cmdInsConsTaxas.Parameters.Add("@flg_operacao", SqlDbType.Char).Value = 'I';
                        cmdInsConsTaxas.Parameters.Add("@COD_ID_PESSOA_LICENCIADO", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["LICENCIADO"].ToString());
                        cmdInsConsTaxas.Parameters.Add("@COD_ID_PLANOS", SqlDbType.Int).Value = Funcoes.strToInt(sCodigoInclusao);
                        cmdInsConsTaxas.Parameters.Add("@COD_ID_BANDEIRA", SqlDbType.Int).Value = Funcoes.strToInt(ReaderCadastroBandeirasDO["COD_ID"].ToString());
                        cmdInsConsTaxas.Parameters.Add("@FLG_PRESENCIAL_ONLINE", SqlDbType.Char).Value = "O";
                        cmdInsConsTaxas.Parameters.Add("@NUM_VALOR_FIXO_DEBITO", SqlDbType.Float).Value = Funcoes.strToDouble(oPlano["payment"]["bankSlip"]["defaultValue"].ToString());
                        cmdInsConsTaxas.ExecuteNonQuery();
                        connInsConsTaxas.Close();
                        connInsConsTaxas.Dispose();
                    }
                    if (ReaderCadastroBandeirasDO["NOM_BANDEIRA"].ToString() == "VISA")
                    {
                        SqlConnection connInsConsTaxas = new SqlConnection(Funcoes.conexao());
                        connInsConsTaxas.Open();
                        SqlCommand cmdInsConsTaxas = new SqlCommand("dbo.stp_planos_parcelas_ins", connInsConsTaxas);
                        cmdInsConsTaxas.CommandType = CommandType.StoredProcedure;
                        cmdInsConsTaxas.Parameters.Add("@flg_operacao", SqlDbType.Char).Value = 'I';
                        cmdInsConsTaxas.Parameters.Add("@COD_ID_PESSOA_LICENCIADO", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["LICENCIADO"].ToString());
                        cmdInsConsTaxas.Parameters.Add("@COD_ID_PLANOS", SqlDbType.Int).Value = Funcoes.strToInt(sCodigoInclusao);
                        cmdInsConsTaxas.Parameters.Add("@COD_ID_BANDEIRA", SqlDbType.Int).Value = Funcoes.strToInt(ReaderCadastroBandeirasDO["COD_ID"].ToString());
                        cmdInsConsTaxas.Parameters.Add("@FLG_PRESENCIAL_ONLINE", SqlDbType.Char).Value = "O";
                        cmdInsConsTaxas.Parameters.Add("@NUM_VALOR_FIXO_DEBITO", SqlDbType.Float).Value = Funcoes.strToDouble(oPlano["payment"]["debitCard"]["operationValue"].ToString());
                        cmdInsConsTaxas.Parameters.Add("@NUM_VALOR_DEBITO", SqlDbType.Float).Value = Funcoes.strToDouble(oPlano["payment"]["debitCard"]["defaultPercentage"].ToString());
                        cmdInsConsTaxas.ExecuteNonQuery();
                        connInsConsTaxas.Close();
                        connInsConsTaxas.Dispose();
                    }
                    if (ReaderCadastroBandeirasDO["NOM_BANDEIRA"].ToString() == "MASTERCARD")
                    {
                        SqlConnection connInsConsTaxas = new SqlConnection(Funcoes.conexao());
                        connInsConsTaxas.Open();
                        SqlCommand cmdInsConsTaxas = new SqlCommand("dbo.stp_planos_parcelas_ins", connInsConsTaxas);
                        cmdInsConsTaxas.CommandType = CommandType.StoredProcedure;
                        cmdInsConsTaxas.Parameters.Add("@flg_operacao", SqlDbType.Char).Value = 'I';
                        cmdInsConsTaxas.Parameters.Add("@COD_ID_PESSOA_LICENCIADO", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["LICENCIADO"].ToString());
                        cmdInsConsTaxas.Parameters.Add("@COD_ID_PLANOS", SqlDbType.Int).Value = Funcoes.strToInt(sCodigoInclusao);
                        cmdInsConsTaxas.Parameters.Add("@COD_ID_BANDEIRA", SqlDbType.Int).Value = Funcoes.strToInt(ReaderCadastroBandeirasDO["COD_ID"].ToString());
                        cmdInsConsTaxas.Parameters.Add("@FLG_PRESENCIAL_ONLINE", SqlDbType.Char).Value = "O";
                        cmdInsConsTaxas.Parameters.Add("@NUM_VALOR_FIXO_DEBITO", SqlDbType.Float).Value = Funcoes.strToDouble(oPlano["payment"]["debitCard"]["operationValue"].ToString());
                        cmdInsConsTaxas.Parameters.Add("@NUM_VALOR_DEBITO", SqlDbType.Float).Value = Funcoes.strToDouble(oPlano["payment"]["debitCard"]["defaultPercentage"].ToString());
                        cmdInsConsTaxas.ExecuteNonQuery();
                        connInsConsTaxas.Close();
                        connInsConsTaxas.Dispose();
                    }
                }



                SqlConnection mySelCadastroBandeirasDP = new SqlConnection(Funcoes.conexao());
                mySelCadastroBandeirasDP.Open();
                SqlCommand cmdSelCadastroBandeirasDP = new SqlCommand("dbo.stp_bandeiras_ins", mySelCadastroBandeirasDP);
                cmdSelCadastroBandeirasDP.CommandType = CommandType.StoredProcedure;
                cmdSelCadastroBandeirasDP.Parameters.Add("@flg_operacao", SqlDbType.Char).Value = "C";
                cmdSelCadastroBandeirasDP.Parameters.Add("@NOM_BANDEIRA", SqlDbType.VarChar).Value = "%";
                cmdSelCadastroBandeirasDP.Parameters.Add("@FLG_ATIVO", SqlDbType.VarChar).Value = "S";
                cmdSelCadastroBandeirasDP.Parameters.Add("@FLG_TIPO", SqlDbType.VarChar).Value = "V";
                SqlDataReader ReaderCadastroBandeirasDP = cmdSelCadastroBandeirasDP.ExecuteReader();
                while (ReaderCadastroBandeirasDP.Read())
                {
                    if (ReaderCadastroBandeirasDP["NOM_BANDEIRA"].ToString() == "BOLETO")
                    {
                        SqlConnection connInsConsTaxas = new SqlConnection(Funcoes.conexao());
                        connInsConsTaxas.Open();
                        SqlCommand cmdInsConsTaxas = new SqlCommand("dbo.stp_planos_parcelas_ins", connInsConsTaxas);
                        cmdInsConsTaxas.CommandType = CommandType.StoredProcedure;
                        cmdInsConsTaxas.Parameters.Add("@flg_operacao", SqlDbType.Char).Value = 'I';
                        cmdInsConsTaxas.Parameters.Add("@COD_ID_PESSOA_LICENCIADO", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["LICENCIADO"].ToString());
                        cmdInsConsTaxas.Parameters.Add("@COD_ID_PLANOS", SqlDbType.Int).Value = Funcoes.strToInt(sCodigoInclusao);
                        cmdInsConsTaxas.Parameters.Add("@COD_ID_BANDEIRA", SqlDbType.Int).Value = Funcoes.strToInt(ReaderCadastroBandeirasDP["COD_ID"].ToString());
                        cmdInsConsTaxas.Parameters.Add("@FLG_PRESENCIAL_ONLINE", SqlDbType.Char).Value = "P";
                        cmdInsConsTaxas.Parameters.Add("@NUM_VALOR_FIXO_DEBITO", SqlDbType.Float).Value = Funcoes.strToDouble(oPlano["payment"]["bankSlip"]["defaultValue"].ToString());
                        cmdInsConsTaxas.ExecuteNonQuery();
                        connInsConsTaxas.Close();
                        connInsConsTaxas.Dispose();
                    }
                    if (ReaderCadastroBandeirasDP["NOM_BANDEIRA"].ToString() == "PIX")
                    {
                        SqlConnection connInsConsTaxas = new SqlConnection(Funcoes.conexao());
                        connInsConsTaxas.Open();
                        SqlCommand cmdInsConsTaxas = new SqlCommand("dbo.stp_planos_parcelas_ins", connInsConsTaxas);
                        cmdInsConsTaxas.CommandType = CommandType.StoredProcedure;
                        cmdInsConsTaxas.Parameters.Add("@flg_operacao", SqlDbType.Char).Value = 'I';
                        cmdInsConsTaxas.Parameters.Add("@COD_ID_PESSOA_LICENCIADO", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["LICENCIADO"].ToString());
                        cmdInsConsTaxas.Parameters.Add("@COD_ID_PLANOS", SqlDbType.Int).Value = Funcoes.strToInt(sCodigoInclusao);
                        cmdInsConsTaxas.Parameters.Add("@COD_ID_BANDEIRA", SqlDbType.Int).Value = Funcoes.strToInt(ReaderCadastroBandeirasDP["COD_ID"].ToString());
                        cmdInsConsTaxas.Parameters.Add("@FLG_PRESENCIAL_ONLINE", SqlDbType.Char).Value = "P";
                        cmdInsConsTaxas.Parameters.Add("@NUM_VALOR_FIXO_DEBITO", SqlDbType.Float).Value = Funcoes.strToDouble(oPlano["payment"]["pix"]["fixedFeeValue"].ToString());
                        cmdInsConsTaxas.ExecuteNonQuery();
                        connInsConsTaxas.Close();
                        connInsConsTaxas.Dispose();
                    }
                    if (ReaderCadastroBandeirasDP["NOM_BANDEIRA"].ToString() == "VISA")
                    {
                        SqlConnection connInsConsTaxas = new SqlConnection(Funcoes.conexao());
                        connInsConsTaxas.Open();
                        SqlCommand cmdInsConsTaxas = new SqlCommand("dbo.stp_planos_parcelas_ins", connInsConsTaxas);
                        cmdInsConsTaxas.CommandType = CommandType.StoredProcedure;
                        cmdInsConsTaxas.Parameters.Add("@flg_operacao", SqlDbType.Char).Value = 'I';
                        cmdInsConsTaxas.Parameters.Add("@COD_ID_PESSOA_LICENCIADO", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["LICENCIADO"].ToString());
                        cmdInsConsTaxas.Parameters.Add("@COD_ID_PLANOS", SqlDbType.Int).Value = Funcoes.strToInt(sCodigoInclusao);
                        cmdInsConsTaxas.Parameters.Add("@COD_ID_BANDEIRA", SqlDbType.Int).Value = Funcoes.strToInt(ReaderCadastroBandeirasDP["COD_ID"].ToString());
                        cmdInsConsTaxas.Parameters.Add("@FLG_PRESENCIAL_ONLINE", SqlDbType.Char).Value = "P";
                        cmdInsConsTaxas.Parameters.Add("@NUM_VALOR_FIXO_DEBITO", SqlDbType.Float).Value = Funcoes.strToDouble(oPlano["payment"]["debitCard"]["operationValue"].ToString());
                        cmdInsConsTaxas.Parameters.Add("@NUM_VALOR_DEBITO", SqlDbType.Float).Value = Funcoes.strToDouble(oPlano["payment"]["debitCard"]["defaultPercentage"].ToString());
                        cmdInsConsTaxas.ExecuteNonQuery();
                        connInsConsTaxas.Close();
                        connInsConsTaxas.Dispose();
                    }
                    if (ReaderCadastroBandeirasDP["NOM_BANDEIRA"].ToString() == "MASTERCARD")
                    {
                        SqlConnection connInsConsTaxas = new SqlConnection(Funcoes.conexao());
                        connInsConsTaxas.Open();
                        SqlCommand cmdInsConsTaxas = new SqlCommand("dbo.stp_planos_parcelas_ins", connInsConsTaxas);
                        cmdInsConsTaxas.CommandType = CommandType.StoredProcedure;
                        cmdInsConsTaxas.Parameters.Add("@flg_operacao", SqlDbType.Char).Value = 'I';
                        cmdInsConsTaxas.Parameters.Add("@COD_ID_PESSOA_LICENCIADO", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["LICENCIADO"].ToString());
                        cmdInsConsTaxas.Parameters.Add("@COD_ID_PLANOS", SqlDbType.Int).Value = Funcoes.strToInt(sCodigoInclusao);
                        cmdInsConsTaxas.Parameters.Add("@COD_ID_BANDEIRA", SqlDbType.Int).Value = Funcoes.strToInt(ReaderCadastroBandeirasDP["COD_ID"].ToString());
                        cmdInsConsTaxas.Parameters.Add("@FLG_PRESENCIAL_ONLINE", SqlDbType.Char).Value = "P";
                        cmdInsConsTaxas.Parameters.Add("@NUM_VALOR_FIXO_DEBITO", SqlDbType.Float).Value = Funcoes.strToDouble(oPlano["payment"]["debitCard"]["operationValue"].ToString());
                        cmdInsConsTaxas.Parameters.Add("@NUM_VALOR_DEBITO", SqlDbType.Float).Value = Funcoes.strToDouble(oPlano["payment"]["debitCard"]["defaultPercentage"].ToString());
                        cmdInsConsTaxas.ExecuteNonQuery();
                        connInsConsTaxas.Close();
                        connInsConsTaxas.Dispose();
                    }
                }
                // CREDITO 
                SqlConnection mySelCadastroBandeirasCO = new SqlConnection(Funcoes.conexao());
                mySelCadastroBandeirasCO.Open();
                SqlCommand cmdSelCadastroBandeirasCO = new SqlCommand("dbo.stp_bandeiras_ins", mySelCadastroBandeirasCO);
                cmdSelCadastroBandeirasCO.CommandType = CommandType.StoredProcedure;
                cmdSelCadastroBandeirasCO.Parameters.Add("@flg_operacao", SqlDbType.Char).Value = "C";
                cmdSelCadastroBandeirasCO.Parameters.Add("@NOM_BANDEIRA", SqlDbType.VarChar).Value = "%";
                cmdSelCadastroBandeirasCO.Parameters.Add("@FLG_ATIVO", SqlDbType.VarChar).Value = "S";
                cmdSelCadastroBandeirasCO.Parameters.Add("@FLG_TIPO", SqlDbType.VarChar).Value = "C";
                SqlDataReader ReaderCadastroBandeirasCO = cmdSelCadastroBandeirasCO.ExecuteReader();
                while (ReaderCadastroBandeirasCO.Read())
                {
                    SqlConnection connInsConsTaxas = new SqlConnection(Funcoes.conexao());
                    connInsConsTaxas.Open();
                    SqlCommand cmdInsConsTaxas = new SqlCommand("dbo.stp_planos_parcelas_ins", connInsConsTaxas);
                    cmdInsConsTaxas.CommandType = CommandType.StoredProcedure;
                    cmdInsConsTaxas.Parameters.Add("@flg_operacao", SqlDbType.Char).Value = 'I';
                    cmdInsConsTaxas.Parameters.Add("@COD_ID_PESSOA_LICENCIADO", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["LICENCIADO"].ToString());
                    cmdInsConsTaxas.Parameters.Add("@COD_ID_PLANOS", SqlDbType.Int).Value = Funcoes.strToInt(sCodigoInclusao);
                    cmdInsConsTaxas.Parameters.Add("@COD_ID_BANDEIRA", SqlDbType.Int).Value = Funcoes.strToInt(ReaderCadastroBandeirasCO["COD_ID"].ToString());
                    cmdInsConsTaxas.Parameters.Add("@FLG_PRESENCIAL_ONLINE", SqlDbType.Char).Value = "O";

                    cmdInsConsTaxas.Parameters.Add("@NUM_VALOR_CREDITO_VISTA", SqlDbType.Float).Value = Funcoes.strToDouble(oPlano["payment"]["creditCard"]["oneInstallmentPercentage"].ToString());
                    cmdInsConsTaxas.Parameters.Add("@NUM_VALOR_CREDITO_2X", SqlDbType.Float).Value = Funcoes.strToDouble(oPlano["payment"]["creditCard"]["upToSixInstallmentsPercentage"].ToString());
                    cmdInsConsTaxas.Parameters.Add("@NUM_VALOR_CREDITO_3X", SqlDbType.Float).Value = Funcoes.strToDouble(oPlano["payment"]["creditCard"]["upToSixInstallmentsPercentage"].ToString());
                    cmdInsConsTaxas.Parameters.Add("@NUM_VALOR_CREDITO_4X", SqlDbType.Float).Value = Funcoes.strToDouble(oPlano["payment"]["creditCard"]["upToSixInstallmentsPercentage"].ToString());
                    cmdInsConsTaxas.Parameters.Add("@NUM_VALOR_CREDITO_5X", SqlDbType.Float).Value = Funcoes.strToDouble(oPlano["payment"]["creditCard"]["upToSixInstallmentsPercentage"].ToString());
                    cmdInsConsTaxas.Parameters.Add("@NUM_VALOR_CREDITO_6X", SqlDbType.Float).Value = Funcoes.strToDouble(oPlano["payment"]["creditCard"]["upToSixInstallmentsPercentage"].ToString());
                    cmdInsConsTaxas.Parameters.Add("@NUM_VALOR_CREDITO_7X", SqlDbType.Float).Value = Funcoes.strToDouble(oPlano["payment"]["creditCard"]["upToTwelveInstallmentsPercentage"].ToString());
                    cmdInsConsTaxas.Parameters.Add("@NUM_VALOR_CREDITO_8X", SqlDbType.Float).Value = Funcoes.strToDouble(oPlano["payment"]["creditCard"]["upToTwelveInstallmentsPercentage"].ToString());
                    cmdInsConsTaxas.Parameters.Add("@NUM_VALOR_CREDITO_9X", SqlDbType.Float).Value = Funcoes.strToDouble(oPlano["payment"]["creditCard"]["upToTwelveInstallmentsPercentage"].ToString());
                    cmdInsConsTaxas.Parameters.Add("@NUM_VALOR_CREDITO_10X", SqlDbType.Float).Value = Funcoes.strToDouble(oPlano["payment"]["creditCard"]["upToTwelveInstallmentsPercentage"].ToString());
                    cmdInsConsTaxas.Parameters.Add("@NUM_VALOR_CREDITO_11X", SqlDbType.Float).Value = Funcoes.strToDouble(oPlano["payment"]["creditCard"]["upToTwelveInstallmentsPercentage"].ToString());
                    cmdInsConsTaxas.Parameters.Add("@NUM_VALOR_CREDITO_12X", SqlDbType.Float).Value = Funcoes.strToDouble(oPlano["payment"]["creditCard"]["upToTwelveInstallmentsPercentage"].ToString());
                    cmdInsConsTaxas.Parameters.Add("@NUM_VALOR_CREDITO_13X", SqlDbType.Float).Value = 0;
                    cmdInsConsTaxas.Parameters.Add("@NUM_VALOR_CREDITO_14X", SqlDbType.Float).Value = 0;
                    cmdInsConsTaxas.Parameters.Add("@NUM_VALOR_CREDITO_15X", SqlDbType.Float).Value = 0;
                    cmdInsConsTaxas.Parameters.Add("@NUM_VALOR_CREDITO_16X", SqlDbType.Float).Value = 0;
                    cmdInsConsTaxas.Parameters.Add("@NUM_VALOR_CREDITO_17X", SqlDbType.Float).Value = 0;
                    cmdInsConsTaxas.Parameters.Add("@NUM_VALOR_CREDITO_18X", SqlDbType.Float).Value = 0;

                    cmdInsConsTaxas.Parameters.Add("@NUM_VALOR_FIXO_CREDITO_VISTA", SqlDbType.Float).Value = Funcoes.strToDouble(oPlano["payment"]["creditCard"]["operationValue"].ToString());
                    cmdInsConsTaxas.Parameters.Add("@NUM_VALOR_FIXO_CREDITO_2X", SqlDbType.Float).Value = Funcoes.strToDouble(oPlano["payment"]["creditCard"]["operationValue"].ToString());
                    cmdInsConsTaxas.Parameters.Add("@NUM_VALOR_FIXO_CREDITO_3X", SqlDbType.Float).Value = Funcoes.strToDouble(oPlano["payment"]["creditCard"]["operationValue"].ToString());
                    cmdInsConsTaxas.Parameters.Add("@NUM_VALOR_FIXO_CREDITO_4X", SqlDbType.Float).Value = Funcoes.strToDouble(oPlano["payment"]["creditCard"]["operationValue"].ToString());
                    cmdInsConsTaxas.Parameters.Add("@NUM_VALOR_FIXO_CREDITO_5X", SqlDbType.Float).Value = Funcoes.strToDouble(oPlano["payment"]["creditCard"]["operationValue"].ToString());
                    cmdInsConsTaxas.Parameters.Add("@NUM_VALOR_FIXO_CREDITO_6X", SqlDbType.Float).Value = Funcoes.strToDouble(oPlano["payment"]["creditCard"]["operationValue"].ToString());
                    cmdInsConsTaxas.Parameters.Add("@NUM_VALOR_FIXO_CREDITO_7X", SqlDbType.Float).Value = Funcoes.strToDouble(oPlano["payment"]["creditCard"]["operationValue"].ToString());
                    cmdInsConsTaxas.Parameters.Add("@NUM_VALOR_FIXO_CREDITO_8X", SqlDbType.Float).Value = Funcoes.strToDouble(oPlano["payment"]["creditCard"]["operationValue"].ToString());
                    cmdInsConsTaxas.Parameters.Add("@NUM_VALOR_FIXO_CREDITO_9X", SqlDbType.Float).Value = Funcoes.strToDouble(oPlano["payment"]["creditCard"]["operationValue"].ToString());
                    cmdInsConsTaxas.Parameters.Add("@NUM_VALOR_FIXO_CREDITO_10X", SqlDbType.Float).Value = Funcoes.strToDouble(oPlano["payment"]["creditCard"]["operationValue"].ToString());
                    cmdInsConsTaxas.Parameters.Add("@NUM_VALOR_FIXO_CREDITO_11X", SqlDbType.Float).Value = Funcoes.strToDouble(oPlano["payment"]["creditCard"]["operationValue"].ToString());
                    cmdInsConsTaxas.Parameters.Add("@NUM_VALOR_FIXO_CREDITO_12X", SqlDbType.Float).Value = Funcoes.strToDouble(oPlano["payment"]["creditCard"]["operationValue"].ToString());
                    cmdInsConsTaxas.Parameters.Add("@NUM_VALOR_FIXO_CREDITO_13X", SqlDbType.Float).Value = 0;
                    cmdInsConsTaxas.Parameters.Add("@NUM_VALOR_FIXO_CREDITO_14X", SqlDbType.Float).Value = 0;
                    cmdInsConsTaxas.Parameters.Add("@NUM_VALOR_FIXO_CREDITO_15X", SqlDbType.Float).Value = 0;
                    cmdInsConsTaxas.Parameters.Add("@NUM_VALOR_FIXO_CREDITO_16X", SqlDbType.Float).Value = 0;
                    cmdInsConsTaxas.Parameters.Add("@NUM_VALOR_FIXO_CREDITO_17X", SqlDbType.Float).Value = 0;
                    cmdInsConsTaxas.Parameters.Add("@NUM_VALOR_FIXO_CREDITO_18X", SqlDbType.Float).Value = 0;

                    cmdInsConsTaxas.ExecuteNonQuery();
                    connInsConsTaxas.Close();
                    connInsConsTaxas.Dispose();

                }

                SqlConnection mySelCadastroBandeirasCP = new SqlConnection(Funcoes.conexao());
                mySelCadastroBandeirasCP.Open();
                SqlCommand cmdSelCadastroBandeirasCP = new SqlCommand("dbo.stp_bandeiras_ins", mySelCadastroBandeirasCP);
                cmdSelCadastroBandeirasCP.CommandType = CommandType.StoredProcedure;
                cmdSelCadastroBandeirasCP.Parameters.Add("@flg_operacao", SqlDbType.Char).Value = "C";
                cmdSelCadastroBandeirasCP.Parameters.Add("@NOM_BANDEIRA", SqlDbType.VarChar).Value = "%";
                cmdSelCadastroBandeirasCP.Parameters.Add("@FLG_ATIVO", SqlDbType.VarChar).Value = "S";
                cmdSelCadastroBandeirasCP.Parameters.Add("@FLG_TIPO", SqlDbType.VarChar).Value = "C";
                SqlDataReader ReaderCadastroBandeirasCP = cmdSelCadastroBandeirasCP.ExecuteReader();
                while (ReaderCadastroBandeirasCP.Read())
                {
                    SqlConnection connInsConsTaxas = new SqlConnection(Funcoes.conexao());
                    connInsConsTaxas.Open();
                    SqlCommand cmdInsConsTaxas = new SqlCommand("dbo.stp_planos_parcelas_ins", connInsConsTaxas);
                    cmdInsConsTaxas.CommandType = CommandType.StoredProcedure;
                    cmdInsConsTaxas.Parameters.Add("@flg_operacao", SqlDbType.Char).Value = 'I';
                    cmdInsConsTaxas.Parameters.Add("@COD_ID_PESSOA_LICENCIADO", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["LICENCIADO"].ToString());
                    cmdInsConsTaxas.Parameters.Add("@COD_ID_PLANOS", SqlDbType.Int).Value = Funcoes.strToInt(sCodigoInclusao);
                    cmdInsConsTaxas.Parameters.Add("@COD_ID_BANDEIRA", SqlDbType.Int).Value = Funcoes.strToInt(ReaderCadastroBandeirasCP["COD_ID"].ToString());
                    cmdInsConsTaxas.Parameters.Add("@FLG_PRESENCIAL_ONLINE", SqlDbType.Char).Value = "P";

                    cmdInsConsTaxas.Parameters.Add("@NUM_VALOR_CREDITO_VISTA", SqlDbType.Float).Value = Funcoes.strToDouble(oPlano["payment"]["creditCard"]["oneInstallmentPercentage"].ToString());
                    cmdInsConsTaxas.Parameters.Add("@NUM_VALOR_CREDITO_2X", SqlDbType.Float).Value = Funcoes.strToDouble(oPlano["payment"]["creditCard"]["upToSixInstallmentsPercentage"].ToString());
                    cmdInsConsTaxas.Parameters.Add("@NUM_VALOR_CREDITO_3X", SqlDbType.Float).Value = Funcoes.strToDouble(oPlano["payment"]["creditCard"]["upToSixInstallmentsPercentage"].ToString());
                    cmdInsConsTaxas.Parameters.Add("@NUM_VALOR_CREDITO_4X", SqlDbType.Float).Value = Funcoes.strToDouble(oPlano["payment"]["creditCard"]["upToSixInstallmentsPercentage"].ToString());
                    cmdInsConsTaxas.Parameters.Add("@NUM_VALOR_CREDITO_5X", SqlDbType.Float).Value = Funcoes.strToDouble(oPlano["payment"]["creditCard"]["upToSixInstallmentsPercentage"].ToString());
                    cmdInsConsTaxas.Parameters.Add("@NUM_VALOR_CREDITO_6X", SqlDbType.Float).Value = Funcoes.strToDouble(oPlano["payment"]["creditCard"]["upToSixInstallmentsPercentage"].ToString());
                    cmdInsConsTaxas.Parameters.Add("@NUM_VALOR_CREDITO_7X", SqlDbType.Float).Value = Funcoes.strToDouble(oPlano["payment"]["creditCard"]["upToTwelveInstallmentsPercentage"].ToString());
                    cmdInsConsTaxas.Parameters.Add("@NUM_VALOR_CREDITO_8X", SqlDbType.Float).Value = Funcoes.strToDouble(oPlano["payment"]["creditCard"]["upToTwelveInstallmentsPercentage"].ToString());
                    cmdInsConsTaxas.Parameters.Add("@NUM_VALOR_CREDITO_9X", SqlDbType.Float).Value = Funcoes.strToDouble(oPlano["payment"]["creditCard"]["upToTwelveInstallmentsPercentage"].ToString());
                    cmdInsConsTaxas.Parameters.Add("@NUM_VALOR_CREDITO_10X", SqlDbType.Float).Value = Funcoes.strToDouble(oPlano["payment"]["creditCard"]["upToTwelveInstallmentsPercentage"].ToString());
                    cmdInsConsTaxas.Parameters.Add("@NUM_VALOR_CREDITO_11X", SqlDbType.Float).Value = Funcoes.strToDouble(oPlano["payment"]["creditCard"]["upToTwelveInstallmentsPercentage"].ToString());
                    cmdInsConsTaxas.Parameters.Add("@NUM_VALOR_CREDITO_12X", SqlDbType.Float).Value = Funcoes.strToDouble(oPlano["payment"]["creditCard"]["upToTwelveInstallmentsPercentage"].ToString());
                    cmdInsConsTaxas.Parameters.Add("@NUM_VALOR_CREDITO_13X", SqlDbType.Float).Value = 0;
                    cmdInsConsTaxas.Parameters.Add("@NUM_VALOR_CREDITO_14X", SqlDbType.Float).Value = 0;
                    cmdInsConsTaxas.Parameters.Add("@NUM_VALOR_CREDITO_15X", SqlDbType.Float).Value = 0;
                    cmdInsConsTaxas.Parameters.Add("@NUM_VALOR_CREDITO_16X", SqlDbType.Float).Value = 0;
                    cmdInsConsTaxas.Parameters.Add("@NUM_VALOR_CREDITO_17X", SqlDbType.Float).Value = 0;
                    cmdInsConsTaxas.Parameters.Add("@NUM_VALOR_CREDITO_18X", SqlDbType.Float).Value = 0;

                    cmdInsConsTaxas.Parameters.Add("@NUM_VALOR_FIXO_CREDITO_VISTA", SqlDbType.Float).Value = Funcoes.strToDouble(oPlano["payment"]["creditCard"]["operationValue"].ToString());
                    cmdInsConsTaxas.Parameters.Add("@NUM_VALOR_FIXO_CREDITO_2X", SqlDbType.Float).Value = Funcoes.strToDouble(oPlano["payment"]["creditCard"]["operationValue"].ToString());
                    cmdInsConsTaxas.Parameters.Add("@NUM_VALOR_FIXO_CREDITO_3X", SqlDbType.Float).Value = Funcoes.strToDouble(oPlano["payment"]["creditCard"]["operationValue"].ToString());
                    cmdInsConsTaxas.Parameters.Add("@NUM_VALOR_FIXO_CREDITO_4X", SqlDbType.Float).Value = Funcoes.strToDouble(oPlano["payment"]["creditCard"]["operationValue"].ToString());
                    cmdInsConsTaxas.Parameters.Add("@NUM_VALOR_FIXO_CREDITO_5X", SqlDbType.Float).Value = Funcoes.strToDouble(oPlano["payment"]["creditCard"]["operationValue"].ToString());
                    cmdInsConsTaxas.Parameters.Add("@NUM_VALOR_FIXO_CREDITO_6X", SqlDbType.Float).Value = Funcoes.strToDouble(oPlano["payment"]["creditCard"]["operationValue"].ToString());
                    cmdInsConsTaxas.Parameters.Add("@NUM_VALOR_FIXO_CREDITO_7X", SqlDbType.Float).Value = Funcoes.strToDouble(oPlano["payment"]["creditCard"]["operationValue"].ToString());
                    cmdInsConsTaxas.Parameters.Add("@NUM_VALOR_FIXO_CREDITO_8X", SqlDbType.Float).Value = Funcoes.strToDouble(oPlano["payment"]["creditCard"]["operationValue"].ToString());
                    cmdInsConsTaxas.Parameters.Add("@NUM_VALOR_FIXO_CREDITO_9X", SqlDbType.Float).Value = Funcoes.strToDouble(oPlano["payment"]["creditCard"]["operationValue"].ToString());
                    cmdInsConsTaxas.Parameters.Add("@NUM_VALOR_FIXO_CREDITO_10X", SqlDbType.Float).Value = Funcoes.strToDouble(oPlano["payment"]["creditCard"]["operationValue"].ToString());
                    cmdInsConsTaxas.Parameters.Add("@NUM_VALOR_FIXO_CREDITO_11X", SqlDbType.Float).Value = Funcoes.strToDouble(oPlano["payment"]["creditCard"]["operationValue"].ToString());
                    cmdInsConsTaxas.Parameters.Add("@NUM_VALOR_FIXO_CREDITO_12X", SqlDbType.Float).Value = Funcoes.strToDouble(oPlano["payment"]["creditCard"]["operationValue"].ToString());
                    cmdInsConsTaxas.Parameters.Add("@NUM_VALOR_FIXO_CREDITO_13X", SqlDbType.Float).Value = 0;
                    cmdInsConsTaxas.Parameters.Add("@NUM_VALOR_FIXO_CREDITO_14X", SqlDbType.Float).Value = 0;
                    cmdInsConsTaxas.Parameters.Add("@NUM_VALOR_FIXO_CREDITO_15X", SqlDbType.Float).Value = 0;
                    cmdInsConsTaxas.Parameters.Add("@NUM_VALOR_FIXO_CREDITO_16X", SqlDbType.Float).Value = 0;
                    cmdInsConsTaxas.Parameters.Add("@NUM_VALOR_FIXO_CREDITO_17X", SqlDbType.Float).Value = 0;
                    cmdInsConsTaxas.Parameters.Add("@NUM_VALOR_FIXO_CREDITO_18X", SqlDbType.Float).Value = 0;

                    cmdInsConsTaxas.ExecuteNonQuery();
                    connInsConsTaxas.Close();
                    connInsConsTaxas.Dispose();

                }
            }
            catch
            {

            }
        }
    }
    protected async void  btnAtualizar_Click(object sender, EventArgs e)
    {
        await ImportarTaxas();
    }
}