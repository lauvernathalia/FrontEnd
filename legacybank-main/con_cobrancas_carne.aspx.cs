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

public partial class con_cobrancas_carne : System.Web.UI.Page
{

    public static DataTable dtParceiros;


    protected void Page_Load(object sender, EventArgs e)
    {
        if (HttpContext.Current.Session.Count <= 0)
        {
            Response.Redirect("login.aspx");
            FormsAuthentication.SignOut();
        }


        if (!IsPostBack)
        {

            dtParceiros = new DataTable();
            dtParceiros.Columns.Add("id", typeof(string));
            dtParceiros.Columns.Add("parceiro", typeof(string));
            dtParceiros.Columns.Add("valor", typeof(string));
            dtParceiros.Columns.Add("percentual", typeof(string));

            AdquirentesService();
            CompradoresService();
            ParceiroService();
            ConfiguracoesService();





            TaxaService();
            txtValor.Text = "10,00";
            txtVencimento.Text = DateTime.Now.ToShortDateString();
        }

    }

    private void ConfiguracoesService()
    {
            SqlConnection mySelCadastro = new SqlConnection(Funcoes.conexao());
            mySelCadastro.Open();
            SqlCommand cmdSelCadastro = new SqlCommand("dbo.stp_pessoas_fj_configuracoes_ins", mySelCadastro);
            cmdSelCadastro.CommandType = CommandType.StoredProcedure;
            cmdSelCadastro.Parameters.Add("@flg_operacao", SqlDbType.Char).Value = "C";
            cmdSelCadastro.Parameters.Add("@COD_ID_PESSOA_LICENCIADO", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["LICENCIADO"].ToString());
            cmdSelCadastro.Parameters.Add("@COD_ID_PESSOAS_FJ", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["PESSOA"].ToString());
            SqlDataReader ReaderCadastro = cmdSelCadastro.ExecuteReader();
            while (ReaderCadastro.Read())
            {
                ckbMulta.Checked = (ReaderCadastro["FLG_MULTA"].ToString().Trim() == "S") ? true : false;
                ddlMulta.SelectedValue = ReaderCadastro["FLG_TIPO_MULTA"].ToString().Trim();
                txtMulta.Text = ReaderCadastro["NUM_MULTA"].ToString().Trim();

                ckbJuros.Checked = (ReaderCadastro["FLG_JUROS"].ToString().Trim() == "S") ? true : false;
                ddlJuros.SelectedValue = ReaderCadastro["FLG_TIPO_JUROS"].ToString().Trim();
                txtJuros.Text = ReaderCadastro["NUM_JUROS"].ToString().Trim();


                ckbDesconto.Checked = (ReaderCadastro["FLG_DESCONTO"].ToString().Trim() == "S") ? true : false;
                ddlDesconto.SelectedValue = ReaderCadastro["FLG_TIPO_DESCONTO"].ToString().Trim();
                txtDesconto.Text = ReaderCadastro["NUM_DESCONTO"].ToString().Trim();
                txtDias.Text = ReaderCadastro["NUM_DIAS"].ToString().Trim();
            }
    }

    private void ParceiroService()
    {
        divParceirosInclusao.Visible = true;
        
        SqlConnection myParceiros = new SqlConnection(Funcoes.conexao());
        myParceiros.Open();
        SqlCommand cmdParceiros = new SqlCommand("dbo.stp_pessoas_fj_ins", myParceiros);
        cmdParceiros.CommandType = CommandType.StoredProcedure;
        cmdParceiros.Parameters.Add("@flg_operacao", SqlDbType.VarChar).Value = "&";
        cmdParceiros.Parameters.Add("@COD_ID_PESSOA_LICENCIADO", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["LICENCIADO"].ToString());
        cmdParceiros.Parameters.Add("@COD_ID_PESSOAS_FJ", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["PESSOA"].ToString());
        cmdParceiros.Parameters.Add("@FLG_TIPO", SqlDbType.Char).Value = "P";
        cmdParceiros.Parameters.Add("@FLG_ATIVO", SqlDbType.Char).Value = "";


        SqlDataAdapter drParceiros = new SqlDataAdapter();
        drParceiros.SelectCommand = cmdParceiros;
        DataSet dsParceiros = new DataSet();
        drParceiros.Fill(dsParceiros, "PESSOAS_FJ");
        ddlParceiros.DataTextField = "NOM_NOME";
        ddlParceiros.DataValueField = "COD_ID";
        ddlParceiros.DataSource = dsParceiros.Tables["PESSOAS_FJ"].DefaultView;
        ddlParceiros.DataBind();
        ddlParceiros.Items.Insert(0, new ListItem("", ""));
    }


    private void TaxaService()
    {
        SqlConnection mySelCadastroTaxa = new SqlConnection(Funcoes.conexao());
        mySelCadastroTaxa.Open();
        SqlCommand cmdSelCadastroTaxa = new SqlCommand("dbo.stp_vendas_ins", mySelCadastroTaxa);
        cmdSelCadastroTaxa.CommandType = CommandType.StoredProcedure;
        cmdSelCadastroTaxa.Parameters.Add("@flg_operacao", SqlDbType.Char).Value = "T";
        cmdSelCadastroTaxa.Parameters.Add("@COD_ID_PESSOAS_FJ_VENDEDOR", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["PESSOA"].ToString());
        cmdSelCadastroTaxa.Parameters.Add("@COD_ID_PESSOA_LICENCIADO", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["LICENCIADO"].ToString());
        cmdSelCadastroTaxa.Parameters.Add("@NOM_TIPO_OPERACAO", SqlDbType.VarChar).Value = "BOLETO";
        cmdSelCadastroTaxa.Parameters.Add("@NOM_TIPO_OPERACAO_BANDEIRA", SqlDbType.VarChar).Value = "BOLETO";
        cmdSelCadastroTaxa.Parameters.Add("@FLG_INTEGRACAO", SqlDbType.VarChar).Value = ddlAdquirentes.SelectedValue.ToString();

        SqlDataReader ReaderCadastroTaxa = cmdSelCadastroTaxa.ExecuteReader();

        while (ReaderCadastroTaxa.Read()) // Percorre os registros do banco
        {
            txtTaxa.Text = String.Format("{0:c2}", Funcoes.strToDouble(ReaderCadastroTaxa["NUM_TAXA_PREVISTA"].ToString()));
        }


    }

    private void CompradoresService()
    {
        using (SqlConnection connection = new SqlConnection(Funcoes.conexao()))
        {
            connection.Open();
            using (SqlCommand cmd = new SqlCommand("dbo.stp_compradores_ins", connection))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@flg_operacao", SqlDbType.VarChar).Value = "C";
                cmd.Parameters.Add("@COD_ID_PESSOA_LICENCIADO", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["LICENCIADO"].ToString());
                cmd.Parameters.Add("@COD_ID_PESSOAS_FJ", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["PESSOA"].ToString());
                cmd.Parameters.Add("@NOM_NOME", SqlDbType.VarChar).Value = "";
                cmd.Parameters.Add("@NUM_CNPJCPF", SqlDbType.VarChar).Value = "";
                cmd.Parameters.Add("@FLG_TIPO", SqlDbType.Char).Value = "C";
                cmd.Parameters.Add("@FLG_INTEGRACAO", SqlDbType.Char).Value = ddlAdquirentes.SelectedValue.ToString();

                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                DataTable dtCompradores = new DataTable();
                adapter.Fill(dtCompradores);

                ddlComprador.DataTextField = "NOM_NOME";
                ddlComprador.DataValueField = "COD_ID";
                ddlComprador.DataSource = dtCompradores;
                ddlComprador.DataBind();
                ddlComprador.Items.Insert(0, new ListItem("", ""));

            }
        }


    }


    private void AdquirentesService()
    {
        using (SqlConnection connection = new SqlConnection(Funcoes.conexao()))
        {
            connection.Open();
            using (SqlCommand cmd = new SqlCommand("dbo.stp_pessoas_fj_integracoes_ins", connection))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@flg_operacao", SqlDbType.VarChar).Value = "F";
                cmd.Parameters.Add("@COD_ID_PESSOA_LICENCIADO", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["LICENCIADO"].ToString());
                cmd.Parameters.Add("@COD_ID_PESSOAS_FJ", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["PESSOA"].ToString());
                cmd.Parameters.Add("@FLG_ACESSO", SqlDbType.Char).Value = "S";

                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                DataTable dtAdquirentes = new DataTable();
                adapter.Fill(dtAdquirentes);

                ddlAdquirentes.DataTextField = "NOM_FANTASIA_INTEGRACAO";
                ddlAdquirentes.DataValueField = "FLG_INTEGRACAO";
                ddlAdquirentes.DataSource = dtAdquirentes;
                ddlAdquirentes.DataBind();
            }
        }
    }
    protected void btnGerarCarne_Click(object sender, System.EventArgs e)
    {
        if (ValidaFormulario())
        {
            string id = "";
            if (ddlAdquirentes.SelectedValue.ToString() == "Z")
            {
                
                id = ConsultarCompradorZoop();

                if (id.ToString().Trim() != "")
                {
                    GravarComprador(id.ToString());
                    string sID = GravarVendaZoop(id).ToString().Trim();
                    if (sID.ToString().Trim() != "")
                    {

                        for (int i = 1; i <= Funcoes.strToInt(ddlParcelas.SelectedValue.ToString()); i++)
                        {
                            GravarVendaZoopParcelas(i, sID, id);
                            //ClientScript.RegisterStartupScript(this.GetType(), "Parcela"+i.ToString(), "alert('Parcela: "+i.ToString().PadLeft(2,'0')+"'); ", true);
                        }
                        ClientScript.RegisterStartupScript(this.GetType(), "SucessoGerarCarne", "alert('Dados gravados com sucesso!'); ", true);

                    }
                    else
                    {
                        ClientScript.RegisterStartupScript(this.GetType(), "ErroGravarVenda", "alert('Ocorreu um erro ao tentar gerar o boleto bancário! Verifique e tente novamente.');", true);

                    }


                    //GeraBoletoZoop(id);
                }
                else
                {
                    ClientScript.RegisterStartupScript(this.GetType(), "ErroComprador", "alert('Não foi possível cadastrar o comprador na base de dados!');", true);
                }
                

            }

            if (ddlAdquirentes.SelectedValue.ToString() == "A")
            {
                /*
                id = asaas.VerificaCliente("", "", txtNome.Text.ToString(), txtCNPJCPF.Text.ToString(), txtEmail.Text.ToString(), txtCEP.Text.ToString(), txtNumero.Text.ToString(), txtCelular.Text.ToString());

                if (id.ToString().Trim() != "")
                {
                    GravarComprador(id.ToString());
                    GeraBoletoAsaas(id);

                }
                */
            }
        }
        else
        {
            ClientScript.RegisterStartupScript(this.GetType(), "ErroCamposFormulario", "alert('Todos os campos são obrigatórios!');", true);
        }

    }

    public bool ValidaFormulario()
    {
        if (txtVencimento.Text.ToString() == "" || txtValor.Text.ToString() == "" || txtDescricao.Text.ToString() == "" || txtNome.Text.ToString() == "" || txtCNPJCPF.Text.ToString() == "" || txtEmail.Text.ToString() == "")
        {
            return false;
        }
        return true;
    }

    private void GeraBoletoAsaas(string id)
    {
        DadosCobrancaAsaaS.Root dcobranca = new DadosCobrancaAsaaS.Root()
        {
            customer = id.ToString(),
            billingType = "BOLETO",
            value = Funcoes.strToDouble(txtValor.Text.ToString()),
            dueDate = Convert.ToDateTime(txtVencimento.Text.ToString()).Year.ToString() + "-" + Convert.ToDateTime(txtVencimento.Text.ToString()).Month.ToString() + "-" + Convert.ToDateTime(txtVencimento.Text.ToString()).Day.ToString(),
            daysAfterDueDateToRegistrationCancellation = 30,
            description = txtDescricao.Text.ToString(),
            externalReference = txtReferencia.Text.ToString()
        };

        var splits = split.GerarSplitAsaas("BOLETO", ddlAdquirentes.SelectedValue, "B", "V", "O", 1);
        dcobranca.split = splits;

        string jsonTransacao = JsonConvert.SerializeObject(dcobranca);

        // Chama operação asaas 
        string sApiKey = asaas.PegarTokenSubconta(Funcoes.strToInt(HttpContext.Current.Session["PESSOA"].ToString()), Funcoes.strToInt(HttpContext.Current.Session["LICENCIADO"].ToString()));
        asaas.HttpResponseResult resultado = asaas.CriarCobrancaAsaas(sApiKey, jsonTransacao);

        if ((resultado.StatusCode == 200) || (resultado.StatusCode == 201))
        {
            // Sucesso - Processar JSON normalmente

            string jsonResponse = resultado.Content;
            JObject oTransacao = JObject.Parse(jsonResponse);

            string url = oTransacao["bankSlipUrl"].ToString();
            string barcode = oTransacao["identificationField"].ToString(); ;
            dvurl.Visible = true;
            txtbarcode.Text = barcode;
            hrfurl.HRef = url;

            string resultadotransacao = transacoes.ProcessarTransacoesAsaas(Funcoes.strToInt(HttpContext.Current.Session["LICENCIADO"].ToString()), Funcoes.strToInt(HttpContext.Current.Session["PESSOA"].ToString()), jsonResponse);
            //ClientScript.RegisterStartupScript(this.GetType(), "MostrarJSON", "alert('"+jsonResponse.ToString()+"');", true);

            transacoes.Venda dvenda = new transacoes.Venda()
            {
                CodIdPessoaLicenciado = Funcoes.strToInt(HttpContext.Current.Session["LICENCIADO"].ToString()),
                CodIdPessoasFjVendedor = Funcoes.strToInt(HttpContext.Current.Session["PESSOA"].ToString()),
                CodIdComprador = id.ToString(),
                FlgTipo = "B",
                FlgBoleto = "S",
                FlgPix = "N",
                FlgCartaoCredito = "N",
                FlgIntegracao = ddlAdquirentes.SelectedValue.ToString(),
                NomDescricaoProduto = txtDescricao.Text.ToString(),
                NumValor = Funcoes.strToDouble(txtValor.Text.ToString()),
                FlgSplit = (splits.Count() > 0) ? "S" : "N",
                FlgLinkPermanente = "N",
                DtaPixVencimento = Convert.ToDateTime(txtVencimento.Text.ToString()),
                DtaVencimento = Convert.ToDateTime(txtVencimento.Text.ToString()),
                DtaData = DateTime.Now,
                NomOrigem = Request.ServerVariables["SERVER_NAME"].ToString() + "/checkout.aspx?id=",
                DesJson = jsonResponse.ToString(),
                NomBoleto = url,
                NomBarcodeBoleto = barcode,
                NomCode = oTransacao["id"].ToString(),
                NumParcelas = 1,
                FlgPrecoParcelamento = "N",
                FlgProduto = "N",
                FlgExibirProdutos = "N",
                NomEmailLink = txtEmails.Text.ToString(),
                NomImagem = "",
                CodIdTransacao = Funcoes.strToInt(resultadotransacao),
                NomPix = "",
                FlgEnderecoEntrega = "N"
            };

            bool resultadoVendas = transacoes.GravarVendas(dvenda);

            ClientScript.RegisterStartupScript(this.GetType(), "SucessoBoletoBancario", "alert('Boleto Bancário gerado com sucesso!');", true);

            if (txtEmails.ToString().Trim() != "")
            {
                if (transacoes.EnviarLinkBoletoEmail(url.ToString(), barcode, txtNome.Text.ToString(), txtDescricao.Text.ToString(), txtVencimento.Text.ToString(), txtValor.Text.ToString(), txtEmails.Text.ToString()))
                {
                    ClientScript.RegisterStartupScript(this.GetType(), "SucessoEmailCobranca", "alert('O e-mail da cobrança foi enviado com sucesso!');", true);
                }
                else
                {
                    ClientScript.RegisterStartupScript(this.GetType(), "ErroEmailCobranca", "alert('Ocorreu um erro ao enviar!');", true);
                }
            }


        }
        else
        {
            // Falha - Exibir erro conforme necessário
            string mensagemErro = resultado.Content;
            ClientScript.RegisterStartupScript(this.GetType(), "ErroGeral", "alert('Ocorreu um erro:" + mensagemErro + "');", true);

        }
    }

    private string GravarVendaZoop(string id)
    {
        string bRetorno = "";

        transacoes.Venda dvenda = new transacoes.Venda()
        {
            CodIdPessoaLicenciado = Funcoes.strToInt(HttpContext.Current.Session["LICENCIADO"].ToString()),
            CodIdPessoasFjVendedor = Funcoes.strToInt(HttpContext.Current.Session["PESSOA"].ToString()),
            CodIdComprador = id.ToString(),
            FlgTipo = "N",
            FlgBoleto = "S",
            FlgPix = "N",
            FlgCartaoCredito = "N",
            FlgIntegracao = ddlAdquirentes.SelectedValue.ToString(),
            NomDescricaoProduto = txtDescricao.Text.ToString(),
            NumValor = Funcoes.strToDouble(txtValor.Text.ToString()),
            FlgSplit = (ckbSplit.Checked) ? "S" : "N",
            FlgLinkPermanente = "N",
            DtaPixVencimento = Convert.ToDateTime(txtVencimento.Text.ToString()),
            DtaVencimento = Convert.ToDateTime(txtVencimento.Text.ToString()),
            DtaData = DateTime.Now,
            NomOrigem = Request.ServerVariables["SERVER_NAME"].ToString() + "/checkout.aspx?id=",
            DesJson = "",
            NomBoleto = "",
            NomBarcodeBoleto = "",
            NomCode = "",
            NumParcelas = Funcoes.strToInt(ddlParcelas.SelectedValue.ToString()),
            FlgPrecoParcelamento = "N",
            FlgProduto = "N",
            FlgExibirProdutos = "N",
            NomEmailLink = txtEmails.Text.ToString(),
            NomImagem = "",
            CodIdTransacao = 0,
            NomPix = "",
            FlgEnderecoEntrega = "N"
        };

        string resultadoVendas = transacoes.GravarVendasDiversas(dvenda);

        // Insere Vendas_parceiros - split
        foreach (ListViewItem itemC in lsvParceiros.Items)
        {
            SqlConnection connInsConsVendasParceiros = new SqlConnection(Funcoes.conexao());
            SqlCommand cmdInsConsVendasParceiros = new SqlCommand("dbo.stp_vendas_parceiros_ins", connInsConsVendasParceiros);
            cmdInsConsVendasParceiros.CommandType = CommandType.StoredProcedure;
            cmdInsConsVendasParceiros.Parameters.Add("@flg_operacao", SqlDbType.Char).Value = 'I';
            cmdInsConsVendasParceiros.Parameters.Add("@COD_ID_VENDAS", SqlDbType.Int).Value = Funcoes.strToInt(resultadoVendas.ToString());
            cmdInsConsVendasParceiros.Parameters.Add("@COD_ID_PESSOA_LICENCIADO", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["LICENCIADO"].ToString());
            cmdInsConsVendasParceiros.Parameters.Add("@COD_ID_PARCEIRO", SqlDbType.Int).Value = Funcoes.strToInt(((TextBox)itemC.FindControl("txtIDParceiro")).Text.ToString());
            cmdInsConsVendasParceiros.Parameters.Add("@NUM_PERCENTUAL", SqlDbType.Float).Value = Funcoes.strToDouble(((TextBox)itemC.FindControl("txtPercentual")).Text.ToString());
            cmdInsConsVendasParceiros.Parameters.Add("@NUM_VALOR", SqlDbType.Float).Value = Funcoes.strToDouble(((TextBox)itemC.FindControl("txtValor")).Text.ToString());
            connInsConsVendasParceiros.Open();
            cmdInsConsVendasParceiros.ExecuteNonQuery();
        }


        return resultadoVendas;
    }

    private void GeraBoletoZoop(string id)
    {
        string seller = "";
        var zoopSplits = split.GerarSplitZoop("BOLETO", ddlAdquirentes.SelectedValue, "B", "V", "O", 1, out seller);

        DadosTransacao.TransacaoBoleto transaction = new DadosTransacao.TransacaoBoleto()
        {
            on_behalf_of = HttpContext.Current.Session["TOKENZOOP"].ToString(),
            customer = id.ToString(),
            amount = Funcoes.strToInt(Convert.ToString(Funcoes.strToDouble(txtValor.Text.ToString()) * 100).ToString()),
            currency = "BRL",
            description = txtDescricao.Text.ToString(),
            reference_id = txtReferencia.Text.ToString(),
            payment_type = "boleto",
            payment_method = new DadosTransacao.payment_method()
            {
                expiration_date = (txtVencimento.Text.ToString().Trim() == "") ? "" : String.Format("{0:u}", Convert.ToDateTime(txtVencimento.Text.ToString().Trim())),
                payment_limit_date = null,
                billing_instructions = new DadosTransacao.billing_instructions() { },
                body_instructions = new string[]
                    {  
                        txtDescricao.Text.ToString(),
                    }

            },
        };

        transaction.split_rules = zoopSplits.ToArray();
        string jsonTransacao = JsonConvert.SerializeObject(transaction);

        // Chama operação zoop 
        zoop.HttpResponseResult resultado = zoop.TransacaoBoletoBancario(jsonTransacao);

        if ((resultado.StatusCode == 200) || (resultado.StatusCode == 201))
        {
            // Sucesso - Processar JSON normalmente

            string jsonResponse = resultado.Content;
            JObject oTransacao = JObject.Parse(jsonResponse);

            string url = oTransacao["payment_method"]["url"].ToString();
            string barcode = oTransacao["payment_method"]["barcode"].ToString();
            dvurl.Visible = true;
            txtbarcode.Text = barcode;
            hrfurl.HRef = url;

            string resultadotransacao = transacoes.ProcessarTransacoesZoop(Funcoes.strToInt(HttpContext.Current.Session["LICENCIADO"].ToString()), Funcoes.strToInt(HttpContext.Current.Session["PESSOA"].ToString()), jsonResponse);

            transacoes.Venda dvenda = new transacoes.Venda()
            {
                CodIdPessoaLicenciado = Funcoes.strToInt(HttpContext.Current.Session["LICENCIADO"].ToString()),
                CodIdPessoasFjVendedor = Funcoes.strToInt(HttpContext.Current.Session["PESSOA"].ToString()),
                CodIdComprador = id.ToString(),
                FlgTipo = "N",
                FlgBoleto = "S",
                FlgPix = "N",
                FlgCartaoCredito = "N",
                FlgIntegracao = ddlAdquirentes.SelectedValue.ToString(),
                NomDescricaoProduto = txtDescricao.Text.ToString(),
                NumValor = Funcoes.strToDouble(txtValor.Text.ToString()),
                FlgSplit = (zoopSplits.Length > 0) ? "S" : "N",
                FlgLinkPermanente = "N",
                DtaPixVencimento = Convert.ToDateTime(txtVencimento.Text.ToString()),
                DtaVencimento = Convert.ToDateTime(txtVencimento.Text.ToString()),
                DtaData = DateTime.Now,
                NomOrigem = Request.ServerVariables["SERVER_NAME"].ToString() + "/checkout.aspx?id=",
                DesJson = jsonResponse,
                NomBoleto = url,
                NomBarcodeBoleto = barcode,
                NomCode = oTransacao["id"].ToString(),
                NumParcelas = Funcoes.strToInt(ddlParcelas.SelectedValue.ToString()),
                FlgPrecoParcelamento = "N",
                FlgProduto = "N",
                FlgExibirProdutos = "N",
                NomEmailLink = txtEmails.Text.ToString(),
                NomImagem = "",
                CodIdTransacao = Funcoes.strToInt(resultadotransacao),
                NomPix = "",
                FlgEnderecoEntrega = "N"
            };

            bool resultadoVendas = transacoes.GravarVendas(dvenda);
            ClientScript.RegisterStartupScript(this.GetType(), "SucessoBoletoBancario", "alert('Boleto Bancário gerado com sucesso!');", true);

            if (txtEmails.ToString().Trim() != "")
            {
                if (transacoes.EnviarLinkBoletoEmail(url.ToString(), barcode, txtNome.Text.ToString(), txtDescricao.Text.ToString(), txtVencimento.Text.ToString(), txtValor.Text.ToString(), txtEmails.Text.ToString()))
                {
                    ClientScript.RegisterStartupScript(this.GetType(), "SucessoEmailCobranca", "alert('O e-mail da cobrança foi enviado com sucesso!');", true);
                }
                else
                {
                    ClientScript.RegisterStartupScript(this.GetType(), "ErroEmailCobranca", "alert('Ocorreu um erro ao enviar!');", true);
                }
            }


        }
        else
        {
            // Falha - Exibir erro conforme necessário
            string mensagemErro = resultado.Content;
            string translatedJson = zoop.TranslateApiError(mensagemErro);
        }

    }

    public string ConsultarCompradorZoop()
    {

        var comprador = new
        {
            first_name = txtNome.Text.ToString(),
            email = txtEmail.Text.ToString(),
            phone_number = txtCelular.Text.ToString(),
            taxpayer_id = Funcoes.TIRAACENTOSDOCUMENTOS(txtCNPJCPF.Text.ToString()),
            address = new
            {
                line1 = txtEndereco.Text.ToString(),
                neighborhood = txtBairro.Text.ToString(),
                city = txtCidade.Text.ToString(),
                state = ddlEstado.SelectedValue.ToString(),
                postal_code = Funcoes.TIRAACENTOSDOCUMENTOS(txtCEP.Text.ToString()),
                country_code = "BR"
            }
        };


        string jsonComprador = JsonConvert.SerializeObject(comprador);
        string resultado = zoop.ConsultaCompradorZoop(txtCNPJCPF.Text.ToString(), jsonComprador);

        if (resultado.ToString().Trim() != "")
        {
            return resultado;
        }
        return "";

    }
    protected void ddlComprador_SelectedIndexChanged(object sender, System.EventArgs e)
    {
        CarregaCliente();
    }

    private void CarregaCliente()
    {
        SqlConnection mySelCadastro = new SqlConnection(Funcoes.conexao());
        mySelCadastro.Open();
        SqlCommand cmdSelCadastro = new SqlCommand("dbo.stp_compradores_ins", mySelCadastro);
        cmdSelCadastro.CommandType = CommandType.StoredProcedure;
        cmdSelCadastro.Parameters.Add("@flg_operacao", SqlDbType.Char).Value = "S";
        cmdSelCadastro.Parameters.Add("@COD_ID", SqlDbType.Int).Value = Funcoes.strToInt(ddlComprador.SelectedValue.ToString());
        cmdSelCadastro.Parameters.Add("@COD_ID_PESSOA_LICENCIADO", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["LICENCIADO"].ToString());
        cmdSelCadastro.Parameters.Add("@COD_ID_PESSOAS_FJ", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["PESSOA"].ToString());
        SqlDataReader ReaderCadastro = cmdSelCadastro.ExecuteReader();
        while (ReaderCadastro.Read())
        {
            txtNome.Text = ReaderCadastro["NOM_NOME"].ToString();
            txtEmail.Text = ReaderCadastro["NOM_EMAIL"].ToString();

            txtEmails.Text = ReaderCadastro["NOM_EMAIL"].ToString();

            txtCNPJCPF.Text = ReaderCadastro["NUM_CNPJCPF"].ToString();
            txtCelular.Text = ReaderCadastro["NOM_CELULAR"].ToString();

            txtEndereco.Text = ReaderCadastro["NOM_ENDERECO"].ToString();
            txtNumero.Text = ReaderCadastro["NOM_NUMERO"].ToString();
            txtComplemento.Text = ReaderCadastro["NOM_COMPLEMENTO"].ToString();
            txtBairro.Text = ReaderCadastro["NOM_BAIRRO"].ToString();
            txtCidade.Text = ReaderCadastro["NOM_CIDADE"].ToString();
            ddlEstado.SelectedValue = ReaderCadastro["NOM_UF"].ToString();
            txtCEP.Text = ReaderCadastro["NOM_CEP"].ToString();
        }

    }

    protected void ddlAdquirentes_SelectedIndexChanged(object sender, System.EventArgs e)
    {
        CompradoresService();
    }

    private void GravarComprador(string id)
    {
        SqlConnection connInsConsCliente = new SqlConnection(Funcoes.conexao());
        connInsConsCliente.Open();
        SqlCommand cmdInsConsCliente = new SqlCommand("dbo.stp_compradores_ins", connInsConsCliente);
        cmdInsConsCliente.CommandType = CommandType.StoredProcedure;

        cmdInsConsCliente.Parameters.Add("@flg_operacao", SqlDbType.Char).Value = 'I';
        cmdInsConsCliente.Parameters.Add("@FLG_ATIVO", SqlDbType.Char).Value = "S";
        cmdInsConsCliente.Parameters.Add("@COD_ID_PESSOA_LICENCIADO", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["LICENCIADO"].ToString());
        cmdInsConsCliente.Parameters.Add("@COD_ID_PESSOAS_FJ", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["PESSOA"].ToString());
        cmdInsConsCliente.Parameters.Add("@COD_ID_COMPRADOR", SqlDbType.VarChar).Value = id.ToString();

        cmdInsConsCliente.Parameters.Add("@DTA_DATA", SqlDbType.DateTime).Value = DateTime.Now;
        cmdInsConsCliente.Parameters.Add("@FLG_TIPO", SqlDbType.Char).Value = "C";
        cmdInsConsCliente.Parameters.Add("@FLG_INTEGRACAO", SqlDbType.Char).Value = ddlAdquirentes.SelectedValue.ToString();

        // Responsável
        cmdInsConsCliente.Parameters.Add("@NOM_NOME", SqlDbType.VarChar).Value = txtNome.Text.ToString();
        cmdInsConsCliente.Parameters.Add("@NUM_CNPJCPF", SqlDbType.VarChar).Value = txtCNPJCPF.Text.ToString();
        cmdInsConsCliente.Parameters.Add("@NOM_EMAIL", SqlDbType.VarChar).Value = txtEmail.Text.ToString();
        cmdInsConsCliente.Parameters.Add("@NOM_CELULAR", SqlDbType.VarChar).Value = txtCelular.Text.ToString();

        // Endereço
        cmdInsConsCliente.Parameters.Add("@NOM_ENDERECO", SqlDbType.VarChar).Value = txtEndereco.Text.ToString();
        cmdInsConsCliente.Parameters.Add("@NOM_NUMERO", SqlDbType.VarChar).Value = txtNumero.Text.ToString();
        cmdInsConsCliente.Parameters.Add("@NOM_COMPLEMENTO", SqlDbType.VarChar).Value = txtComplemento.Text.ToString();
        cmdInsConsCliente.Parameters.Add("@NOM_BAIRRO", SqlDbType.VarChar).Value = txtBairro.Text.ToString();
        cmdInsConsCliente.Parameters.Add("@NOM_CIDADE", SqlDbType.VarChar).Value = txtCidade.Text.ToString();
        cmdInsConsCliente.Parameters.Add("@NOM_UF", SqlDbType.VarChar).Value = ddlEstado.SelectedValue.ToString();
        cmdInsConsCliente.Parameters.Add("@NOM_CEP", SqlDbType.VarChar).Value = txtCEP.Text.ToString();
        cmdInsConsCliente.Parameters.Add("@NOM_PAIS", SqlDbType.VarChar).Value = "BR";

        cmdInsConsCliente.ExecuteNonQuery();
        connInsConsCliente.Close();
        connInsConsCliente.Dispose();

    }
    protected void txtCEP_TextChanged(object sender, EventArgs e)
    {
        if (txtCEP.Text.ToString().Trim() != "")
        {
            ServicePointManager.Expect100Continue = true;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12
            | SecurityProtocolType.Tls11
            | SecurityProtocolType.Tls
            | SecurityProtocolType.Ssl3;

            // Convert to Base64

            var myUri = new Uri("https://viacep.com.br/ws/" + txtCEP.Text.ToString() + "/json");
            var myWebRequest = WebRequest.Create(myUri);
            var myHttpWebRequest = (HttpWebRequest)myWebRequest;
            myHttpWebRequest.Accept = "application/json";

            var myWebResponse = myWebRequest.GetResponse();
            var responseStream = myWebResponse.GetResponseStream();

            //if (responseStream == null) return null;

            StreamReader myStreamReader = new StreamReader(responseStream, Encoding.UTF8);
            var json = myStreamReader.ReadToEnd();

            dadosCEP.CEPInfo m = JsonSerializer.Deserialize<dadosCEP.CEPInfo>(json);
            txtEndereco.Text = m.logradouro;
            txtBairro.Text = m.bairro;
            txtCidade.Text = m.localidade;
            ddlEstado.SelectedValue = m.uf;

            responseStream.Close();
            myWebResponse.Close();
        }

    }

    protected void txtEmail_TextChanged(object sender, System.EventArgs e)
    {
        txtEmails.Text = txtEmail.Text.ToString();
    }

    protected void ddlParceiros_SelectedIndexChanged(object sender, EventArgs e)
    {
        SqlConnection mySelCadastro = new SqlConnection(Funcoes.conexao());
        mySelCadastro.Open();
        SqlCommand cmdSelCadastro = new SqlCommand("dbo.stp_pessoas_fj_ins", mySelCadastro);
        cmdSelCadastro.CommandType = CommandType.StoredProcedure;
        cmdSelCadastro.Parameters.Add("@flg_operacao", SqlDbType.Char).Value = "S";
        cmdSelCadastro.Parameters.Add("@COD_ID", SqlDbType.Int).Value = Funcoes.strToInt(ddlParceiros.SelectedValue.ToString());
        cmdSelCadastro.Parameters.Add("@COD_ID_PESSOA_LICENCIADO", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["LICENCIADO"].ToString());
        cmdSelCadastro.Parameters.Add("@COD_ID_PESSOAS_FJ", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["PESSOA"].ToString());
        SqlDataReader ReaderCadastro = cmdSelCadastro.ExecuteReader();
        while (ReaderCadastro.Read())
        {
            txtIDParceiro.Text = ReaderCadastro["COD_ID"].ToString();
            txtParceiro.Text = ReaderCadastro["NOM_RAZAOSOCIAL"].ToString() + "(" + ReaderCadastro["NOM_NOME"].ToString() + " " + ReaderCadastro["NOM_SOBRENOME"].ToString() + ")";
            txtValorParceiro.Text = String.Format("{0:n0}", Funcoes.strToDouble("0"));
            txtPercentualParceiro.Text = String.Format("{0:n0}", Funcoes.strToDouble("0"));
        }

    }
    protected void btNovoParceiro_Click(object sender, EventArgs e)
    {
        ClientScript.RegisterStartupScript(this.GetType(),
"CadastroParceiro", "openPopupWindow('cad_parceiros.aspx?id=0','Parceiros',1024,800);", true);

    }
    protected void btnIncluirParceiro_Click(object sender, EventArgs e)
    {
        dtParceiros.Rows.Add(txtIDParceiro.Text.ToString(), txtParceiro.Text.ToString(), txtValorParceiro.Text.ToString(), txtPercentualParceiro.Text.ToString());

        this.lsvParceiros.DataSource = dtParceiros;
        this.lsvParceiros.DataBind();
    }
    protected void lsvParceiros_ItemCommand(object sender, ListViewCommandEventArgs e)
    {

        if (e.CommandName == "Excluir")
        {
            for (int i = dtParceiros.Rows.Count - 1; i >= 0; i--)
            {
                DataRow dr = dtParceiros.Rows[i];
                if (dr["id"].ToString() == e.CommandArgument.ToString())
                    dr.Delete();
            }
            dtParceiros.AcceptChanges();

            this.lsvParceiros.DataSource = dtParceiros;
            this.lsvParceiros.DataBind();
        }
    }

    protected void ckbSplit_CheckedChanged(object sender, EventArgs e)
    {
        divSplit.Visible = (ckbSplit.Checked) ? true : false;
        divSplit.Focus();

    }

    private void GravarVendaZoopParcelas(int iParcela, string sVenda, string iComprador)
    {

        //string seller = "";
        //var zoopSplits = split.GerarSplitZoop("BOLETO", ddlAdquirentes.SelectedValue, "B", "V", "O", 1, out seller);

        double valorCarne = Funcoes.strToDouble(txtValor.Text);
        int parcelasCarne = Funcoes.strToInt(ddlParcelas.SelectedValue);

        //int amount = (int)Math.Round((valor / parcelas) * 100);

        DadosTransacao.TransacaoBoleto dtransacao = new DadosTransacao.TransacaoBoleto()
        {
            on_behalf_of = HttpContext.Current.Session["TOKENZOOP"].ToString(),
            customer = iComprador.ToString(),
            amount = (int)Math.Round((valorCarne / parcelasCarne) * 100),//Funcoes.strToInt(Convert.ToString((Funcoes.strToDouble(txtValor.Text.ToString()) / Funcoes.strToInt(ddlParcelas.SelectedValue.ToString())) * 100).ToString()),
            currency = "BRL",
            description = txtDescricao.Text.ToString(),
            reference_id = txtReferencia.Text.ToString(),
            payment_type = "boleto",

            payment_method = new DadosTransacao.payment_method()
            {
                expiration_date = Convert.ToDateTime(txtVencimento.Text.ToString()).AddMonths(iParcela - 1).Year.ToString().PadLeft(4, '0') + "-" + Convert.ToDateTime(txtVencimento.Text.ToString()).AddMonths(iParcela - 1).Month.ToString().PadLeft(2, '0') + "-" + Convert.ToDateTime(txtVencimento.Text.ToString()).AddMonths(iParcela - 1).Day.ToString().PadLeft(2, '0'),
                payment_limit_date = Convert.ToDateTime(txtVencimento.Text.ToString()).AddMonths(iParcela - 1).Year.ToString().PadLeft(4, '0') + "-" + Convert.ToDateTime(txtVencimento.Text.ToString()).AddMonths(iParcela - 1).Month.ToString().PadLeft(2, '0') + "-" + Convert.ToDateTime(txtVencimento.Text.ToString()).AddMonths(iParcela - 1).Day.ToString().PadLeft(2, '0'),
                billing_instructions = null,
                body_instructions = new string[]
                    {  
                        txtDescricao.Text.ToString() + " - Parcela:" + iParcela.ToString().PadLeft(2, '0') + "/" + ddlParcelas.SelectedValue.ToString().PadLeft(2, '0'),
                    }
            }
        };


        // Verifica a existência de split Manual --------------------------------------------------------------------------------------------------------------------
        SqlConnection mySelCadastroSplit = new SqlConnection(Funcoes.conexao());
        mySelCadastroSplit.Open();
        SqlCommand cmdSelCadastroSplit = new SqlCommand("dbo.stp_vendas_parceiros_ins", mySelCadastroSplit);
        cmdSelCadastroSplit.CommandType = CommandType.StoredProcedure;
        cmdSelCadastroSplit.Parameters.Add("@flg_operacao", SqlDbType.Char).Value = "C";
        cmdSelCadastroSplit.Parameters.Add("@COD_ID_VENDAS", SqlDbType.Int).Value = Funcoes.strToInt(sVenda.ToString());
        cmdSelCadastroSplit.Parameters.Add("@COD_ID_PESSOA_LICENCIADO", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["LICENCIADO"].ToString());
        SqlDataReader ReaderCadastroSplit = cmdSelCadastroSplit.ExecuteReader();

        List<DadosTransacao.split_rules> listaSplits = new List<DadosTransacao.split_rules>();

        int iContadorSplit = 0;

        while (ReaderCadastroSplit.Read()) // Percorre os registros do banco
        {
            iContadorSplit = iContadorSplit + 1;
            if (Funcoes.strToDouble(ReaderCadastroSplit["NUM_VALOR"].ToString()) > 0)
            {
                listaSplits.Add(
                new DadosTransacao.split_rules()
                {
                    recipient = ReaderCadastroSplit["NOM_TOKEN_ZOOP_PARCEIRO"].ToString(),
                    liable = true,
                    charge_processing_fee = true,
                    amount = Funcoes.strToInt((Funcoes.strToDouble(ReaderCadastroSplit["NUM_PERCENTUAL"].ToString()) * 100).ToString()),
                });
            }
            if (Funcoes.strToDouble(ReaderCadastroSplit["NUM_PERCENTUAL"].ToString()) > 0)
            {
                listaSplits.Add(
                new DadosTransacao.split_rules()
                {
                    recipient = ReaderCadastroSplit["NOM_TOKEN_ZOOP_PARCEIRO"].ToString(),
                    liable = true,
                    charge_processing_fee = true,
                    percentage = Funcoes.strToDouble(ReaderCadastroSplit["NUM_PERCENTUAL"].ToString()),
                });
            }
        }

        // Converte a lista para array antes de atribuir ao objeto
        if (iContadorSplit > 0) { dtransacao.split_rules = listaSplits.ToArray(); }
        // Fim Split Manual ------------------------------------------------------------------------------------------------------------------

        //transaction.split_rules = zoopSplits.ToArray();

        string jsonTransacao = JsonConvert.SerializeObject(dtransacao);

        // Chama operação zoop 
        zoop.HttpResponseResult resultado = zoop.TransacaoBoletoBancario(jsonTransacao);

        if ((resultado.StatusCode == 200) || (resultado.StatusCode == 201))
        {
            // Sucesso - Processar JSON normalmente

            string jsonResponse = resultado.Content;
            JObject oTransacao = JObject.Parse(jsonResponse);

            string url = oTransacao["payment_method"]["url"].ToString();
            string barcode = oTransacao["payment_method"]["barcode"].ToString();

            string resultadotransacao = transacoes.ProcessarTransacoesZoop(Funcoes.strToInt(HttpContext.Current.Session["LICENCIADO"].ToString()), Funcoes.strToInt(HttpContext.Current.Session["PESSOA"].ToString()), jsonResponse);


            string sDescricao = txtDescricao.Text.ToString();
            if (txtAviso.Text.ToString().Trim() != "") { sDescricao = sDescricao + " / " + txtAviso.Text.ToString(); }
            sDescricao = sDescricao + " - Parcela:" + iParcela.ToString().PadLeft(2, '0') + "/" + ddlParcelas.SelectedValue.ToString().PadLeft(2, '0');

            transacoes.Venda dvenda = new transacoes.Venda()
            {
                CodIdPessoaLicenciado = Funcoes.strToInt(HttpContext.Current.Session["LICENCIADO"].ToString()),
                CodIdPessoasFjVendedor = Funcoes.strToInt(HttpContext.Current.Session["PESSOA"].ToString()),
                CodIdComprador = iComprador.ToString(),
                FlgTipo = "N",
                FlgBoleto = "S",
                FlgPix = "N",
                FlgCartaoCredito = "N",
                FlgIntegracao = ddlAdquirentes.SelectedValue.ToString(),
                NomDescricaoProduto = sDescricao,
                NumValor = Funcoes.strToDouble(txtValor.Text.ToString()) / Funcoes.strToInt(ddlParcelas.SelectedValue.ToString()),
                FlgSplit = (ckbSplit.Checked == true) ? "S" : "N",
                FlgLinkPermanente = "N",
                DtaPixVencimento = Convert.ToDateTime(txtVencimento.Text.ToString()).AddMonths(iParcela - 1),
                DtaVencimento = Convert.ToDateTime(txtVencimento.Text.ToString()).AddMonths(iParcela - 1),
                DtaData = DateTime.Now,
                NomOrigem = Request.ServerVariables["SERVER_NAME"].ToString() + "/checkout.aspx?id=",
                DesJson = jsonResponse,
                NomBoleto = url,
                NomBarcodeBoleto = barcode,
                NomCode = oTransacao["id"].ToString(),
                NumParcelas = iParcela,
                FlgPrecoParcelamento = "N",
                FlgProduto = "N",
                FlgExibirProdutos = "N",
                NomEmailLink = txtEmails.Text.ToString(),
                NomImagem = "",
                CodIdTransacao = Funcoes.strToInt(resultadotransacao),
                NomPix = "",
                FlgEnderecoEntrega = "N"
            };

            bool resultadoVendas = transacoes.GravarVendasDetalhe(dvenda, sVenda.ToString());

            if (txtEmails.ToString().Trim() != "")
            {
                transacoes.EnviarLinkBoletoEmail(url.ToString(), barcode, txtNome.Text.ToString(), txtDescricao.Text.ToString(), Convert.ToDateTime(txtVencimento.Text.ToString()).AddMonths(iParcela - 1).ToShortDateString(), String.Format("{0:n2}",Funcoes.strToDouble(txtValor.Text.ToString()) / Funcoes.strToInt(ddlParcelas.SelectedValue.ToString())), txtEmails.Text.ToString());
            }


        }
        else
        {
            // Falha - Exibir erro conforme necessário
            string mensagemErro = resultado.Content;
            string translatedJson = zoop.TranslateApiError(mensagemErro);
            ClientScript.RegisterStartupScript(this.GetType(), "MensagemErro", "alert('"+ mensagemErro +"'); ", true);
        }
    }
    protected void btnVerificaComprador_Click(object sender, System.EventArgs e)
    {
        string jsonResponse = "";
        zoop.HttpResponseResult resultado = zoop.ConsultarCompradorCNPJCPF(Funcoes.TIRAACENTOSDOCUMENTOS(txtCNPJCPF.Text.ToString()));

        jsonResponse = jsonResponse + resultado.StatusCode;
        txtDescricao.Text = resultado.Content;

        ClientScript.RegisterStartupScript(this.GetType(), "MensagemErroComprador", "alert('Resposta:" + jsonResponse + "'); ", true);

    }
}