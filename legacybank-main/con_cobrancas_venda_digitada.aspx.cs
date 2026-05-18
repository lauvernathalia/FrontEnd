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

public partial class con_cobrancas_venda_digitada : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (HttpContext.Current.Session.Count <= 0)
        {
            Response.Redirect("login.aspx");
            FormsAuthentication.SignOut();
        }


        if (!IsPostBack)
        {
            AdquirentesService();
            CompradoresService();
            TaxaService();

            for (int i = 30; i >= 0; i--)
            {
                ddlAnoCartao.Items.Insert(0, new ListItem((DateTime.Now.Year + i).ToString(), (DateTime.Now.Year + i).ToString()));
            }
            
            txtValor.Text = "10,00";
            txtVencimento.Text = DateTime.Now.ToShortDateString();
        }

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
        cmdSelCadastroTaxa.Parameters.Add("@NOM_TIPO_OPERACAO", SqlDbType.VarChar).Value = "Crédito";
        cmdSelCadastroTaxa.Parameters.Add("@NOM_TIPO_OPERACAO_BANDEIRA", SqlDbType.VarChar).Value = lblBandeira.Text.ToString();
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
    protected void btnGerarVendaDireta_Click(object sender, System.EventArgs e)
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
                    GeraVendaDigitadaZoop(id);
                }
                else
                {
                    ClientScript.RegisterStartupScript(this.GetType(), "ErroComprador", "alert('Não foi possível cadastrar o comprador na base de dados!');", true);
                }
            }

            if (ddlAdquirentes.SelectedValue.ToString() == "A")
            {
                id = asaas.VerificaCliente("", "", txtNome.Text.ToString(), txtCNPJCPF.Text.ToString(), txtEmail.Text.ToString(), txtCEP.Text.ToString(), txtNumero.Text.ToString(), txtCelular.Text.ToString());

                if (id.ToString().Trim() != "")
                {
                    GravarComprador(id.ToString());
                    GeraVendaDigitadaAsaas(id);

                }
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

    private void GeraVendaDigitadaAsaas(string id)
    {

        DadosCobrancaAsaaS.Root dcobranca = new DadosCobrancaAsaaS.Root()
        {
            customer = id.ToString(),
            billingType = "CREDIT_CARD",
            value = Funcoes.strToDouble(txtValor.Text.ToString()),
            dueDate = Convert.ToDateTime(txtVencimento.Text.ToString()).Year.ToString() + "-" + Convert.ToDateTime(txtVencimento.Text.ToString()).Month.ToString() + "-" + Convert.ToDateTime(txtVencimento.Text.ToString()).Day.ToString(),
            daysAfterDueDateToRegistrationCancellation = 30,
            creditCard = new DadosCobrancaAsaaS.CreditCard()
            {
                holderName = txtNomeCartao.Text.ToString(),
                number = txtNumeroCartao.Text.ToString(),
                ccv = txtCVVCartao.Text.ToString(),
                expiryYear = ddlAnoCartao.SelectedValue.ToString().PadLeft(4, '0'),
                expiryMonth = ddlMesCartao.SelectedValue.ToString().PadLeft(1, '0')
            },
            creditCardHolderInfo = new DadosCobrancaAsaaS.CreditCardHolderInfo()
            {
                cpfCnpj = txtCNPJCPF.Text.ToString(),
                name = txtNome.Text.ToString(),
                email = txtEmail.Text.ToString(),
                postalCode = txtCEP.Text.ToString(),
                mobilePhone = txtCelular.Text.ToString(),
                phone = txtCelular.Text.ToString(),
                addressNumber = txtNumero.Text.ToString()
            }
        };

        var splits = split.GerarSplitAsaas(lblBandeira.Text.ToString(), ddlAdquirentes.SelectedValue, "C","C","O",Funcoes.strToInt(ddlParcelas.SelectedValue.ToString()));
        dcobranca.split = splits;

        string jsonTransacao = JsonConvert.SerializeObject(dcobranca);
        //txtDescricao.Text = jsonTransacao;
        // Chama operação asaas 
        string sApiKey = asaas.PegarTokenSubconta(Funcoes.strToInt(HttpContext.Current.Session["PESSOA"].ToString()), Funcoes.strToInt(HttpContext.Current.Session["LICENCIADO"].ToString()));
        asaas.HttpResponseResult resultado = asaas.CriarCobrancaAsaas(sApiKey, jsonTransacao);
        //txtReferencia.Text = resultado.Content;

        if ((resultado.StatusCode == 200) || (resultado.StatusCode == 201))
        {
            // Sucesso - Processar JSON normalmente

            string jsonResponse = resultado.Content;
            JObject oTransacao = JObject.Parse(jsonResponse);

            string url = "";
            string barcode = "";
            dvCartao.Visible = true;
            txtcartao.Text = "";

            string resultadotransacao = transacoes.ProcessarTransacoesAsaas(Funcoes.strToInt(HttpContext.Current.Session["LICENCIADO"].ToString()), Funcoes.strToInt(HttpContext.Current.Session["PESSOA"].ToString()), jsonResponse);
            //ClientScript.RegisterStartupScript(this.GetType(), "MostrarJSON", "alert('"+jsonResponse.ToString()+"');", true);

            transacoes.Venda dvenda = new transacoes.Venda()
            {
                CodIdPessoaLicenciado = Funcoes.strToInt(HttpContext.Current.Session["LICENCIADO"].ToString()),
                CodIdPessoasFjVendedor = Funcoes.strToInt(HttpContext.Current.Session["PESSOA"].ToString()),
                CodIdComprador = id.ToString(),
                FlgTipo = "C",
                FlgBoleto = "N",
                FlgPix = "N",
                FlgCartaoCredito = "S",
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

            ClientScript.RegisterStartupScript(this.GetType(), "SucessoBoletoBancario", "alert('Venda Digitada gerado com sucesso!');", true);

            //if (txtEmails.ToString().Trim() != "")
            //{
            //    if (transacoes.EnviarLinkBoletoEmail(url.ToString(), barcode, txtNome.Text.ToString(), txtDescricao.Text.ToString(), txtVencimento.Text.ToString(), txtValor.Text.ToString(), txtEmails.Text.ToString()))
            //    {
            //        ClientScript.RegisterStartupScript(this.GetType(), "SucessoEmailCobranca", "alert('O e-mail da cobrança foi enviado com sucesso!');", true);
            //    }
            //    else
            //    {
            //        ClientScript.RegisterStartupScript(this.GetType(), "ErroEmailCobranca", "alert('Ocorreu um erro ao enviar!');", true);
            //    }
            //}


        }
        else
        {
            // Falha - Exibir erro conforme necessário
            string mensagemErro = resultado.Content;
            if (mensagemErro.ToString().Trim() != "")
            {
                ClientScript.RegisterStartupScript(this.GetType(), "ErroAutorizacao", "alert('Ocorreu um erro ao criar a venda digitada! Erro:" + mensagemErro + "');", true);
            }
            else
            {
                ClientScript.RegisterStartupScript(this.GetType(), "ErroAutorizacaoGeral", "alert('Ocorreu um erro ao criar a venda digitada!');", true);
            }

        }
    }


    private void GeraVendaDigitadaZoop(string id)
    {
        string seller = "";
        var zoopSplits = split.GerarSplitZoop(lblBandeira.Text.ToString(), ddlAdquirentes.SelectedValue, "C", "C", "O", Funcoes.strToInt(ddlParcelas.SelectedValue.ToString()), out seller);
        /*
        DadosTransacao.TransacaoCard transaction = new DadosTransacao.TransacaoCard()
        {
            on_behalf_of = HttpContext.Current.Session["TOKENZOOP"].ToString(),
            description = txtDescricao.Text.ToString(),
            reference_id = txtReferencia.Text.ToString(),
            payment_type = "credit",
            capture = false,
            source = new DadosTransacao.source()
            {
                usage = "single_use",
                amount = Funcoes.strToInt(Convert.ToString(Funcoes.strToDouble(txtValor.Text.ToString()) * 100).ToString()),
                currency = "BRL",
                type = "card",
                card = new DadosTransacao.card()
                {
                    card_number = txtNumeroCartao.Text.ToString(),
                    holder_name = txtNomeCartao.Text.ToString(),
                    expiration_month = ddlMesCartao.SelectedValue.ToString(),
                    expiration_year = ddlAnoCartao.SelectedValue.ToString(),
                    security_code = txtCVVCartao.Text.ToString()
                }
            },

            installment_plan = new DadosTransacao.installment_plan
            {
                number_installments = Funcoes.strToInt(ddlParcelas.SelectedValue.ToString())
            }
        };
        */
        DadosTransacao.TransacaoCard dcobranca = new DadosTransacao.TransacaoCard()
        {
            on_behalf_of = HttpContext.Current.Session["TOKENZOOP"].ToString(),
            description = txtDescricao.Text.ToString(),
            reference_id = txtReferencia.Text.ToString(),
            payment_type = "credit",
            capture = true,
            source = new DadosTransacao.source()
            {
                usage = "single_use",
                amount = Funcoes.strToInt(Convert.ToString(Funcoes.strToDouble(txtValor.Text.ToString()) * 100).ToString()),
                currency = "BRL",
                type = "card",
                card = new DadosTransacao.card()
                {
                    card_number = txtNumeroCartao.Text.ToString(),
                    holder_name = txtNomeCartao.Text.ToString(),
                    expiration_month = ddlMesCartao.SelectedValue.ToString(),
                    expiration_year = ddlAnoCartao.SelectedValue.ToString(),
                    security_code = txtCVVCartao.Text.ToString()
                }
            },
            installment_plan = new DadosTransacao.installment_plan()
            {
                number_installments = Funcoes.strToInt(ddlParcelas.SelectedValue.ToString())
            },
            three_d_secure = new DadosTransacao.three_d_secure()
            {
                challenge_type = "DATA_ONLY",
                ip_address = Request.ServerVariables["REMOTE_ADDR"].ToString(),
                user_agent = Request.ServerVariables["HTTP_USER_AGENT"].ToString(),
                on_failure = "continue",
                device = new DadosTransacao.device()
                {
                    color_depth = 24,
                    type = "BROWSER",
                    java_enabled = false,
                    language = Request.Headers["Accept-Language"].ToString().Split(';').FirstOrDefault().ToString().Split(',').FirstOrDefault().ToString(),
                    screen_height = (Request.Browser.ScreenPixelsHeight) * 2,
                    screen_width = (Request.Browser.ScreenPixelsWidth) * 2,
                    time_zone_offset = Funcoes.strToInt(DateTimeOffset.Now.Offset.ToString().Split(':').FirstOrDefault().ToString())
                }
            }
        };

        dcobranca.split_rules = zoopSplits.ToArray();

        //string jsonVendaDigitada = JsonConvert.SerializeObject(dcobranca);
        //txtDescricao.Text = jsonVendaDigitada;

        //string jsonRetorno = zoopv2.CriarTransacao(jsonVendaDigitada);
        //txtReferencia.Text = jsonRetorno;
        //return;
        
        string jsonTransacao = JsonConvert.SerializeObject(dcobranca);
        // Chama operação zoop 
        //txtDescricao.Text = jsonTransacao;

        zoop.HttpResponseResult resultado = zoop.TransacaoCartao(jsonTransacao);
        //txtReferencia.Text = resultado.Content;
        //return;

        if ((resultado.StatusCode == 200) || (resultado.StatusCode == 201))
        {
            // Sucesso - Processar JSON normalmente

            string jsonResponse = resultado.Content;
            JObject oTransacao = JObject.Parse(jsonResponse);

            dvCartao.Visible = true;
            txtcartao.Text = oTransacao["payment_authorization"]["authorizer_id"].ToString() + " - NSU:" + oTransacao["payment_authorization"]["authorization_nsu"].ToString() + " - CODE:" + oTransacao["payment_authorization"]["authorization_code"].ToString();

            string url = "";
            string barcode = "";
            dvCartao.Visible = true;
            txtcartao.Text = "";

            string resultadotransacao = transacoes.ProcessarTransacoesZoop(Funcoes.strToInt(HttpContext.Current.Session["LICENCIADO"].ToString()), Funcoes.strToInt(HttpContext.Current.Session["PESSOA"].ToString()), jsonResponse);

            transacoes.Venda dvenda = new transacoes.Venda()
            {
                CodIdPessoaLicenciado = Funcoes.strToInt(HttpContext.Current.Session["LICENCIADO"].ToString()),
                CodIdPessoasFjVendedor = Funcoes.strToInt(HttpContext.Current.Session["PESSOA"].ToString()),
                CodIdComprador = id.ToString(),
                FlgTipo = "C",
                FlgBoleto = "N",
                FlgPix = "N",
                FlgCartaoCredito = "S",
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
            ClientScript.RegisterStartupScript(this.GetType(), "SucessoBoletoBancario", "alert('Venda Digitada gerado com sucesso!');", true);

            //if (txtEmails.ToString().Trim() != "")
            //{
            //    if (transacoes.EnviarLinkBoletoEmail(url.ToString(), barcode, txtNome.Text.ToString(), txtDescricao.Text.ToString(), txtVencimento.Text.ToString(), txtValor.Text.ToString(), txtEmails.Text.ToString()))
            //    {
            //        ClientScript.RegisterStartupScript(this.GetType(), "SucessoEmailCobranca", "alert('O e-mail da cobrança foi enviado com sucesso!');", true);
            //    }
            //    else
            //    {
            //        ClientScript.RegisterStartupScript(this.GetType(), "ErroEmailCobranca", "alert('Ocorreu um erro ao enviar!');", true);
            //    }
            //}


        }
        else
        {
            // Falha - Exibir erro conforme necessário
            string mensagemErro = resultado.Content;
            //string translatedJson = zoop.TranslateApiError(mensagemErro);
            if (mensagemErro.ToString().Trim() != "")
            {
                ClientScript.RegisterStartupScript(this.GetType(), "ErroAutorizacao", "alert('Ocorreu um erro ao criar a venda digitada! Erro:" + mensagemErro + "');", true);
            }
            else
            {
                ClientScript.RegisterStartupScript(this.GetType(), "ErroAutorizacaoGeral", "alert('Ocorreu um erro ao criar a venda digitada!');", true);
            }
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
                postal_code = txtCEP.Text.ToString(),
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

    public static string ObterBandeiraCartao(string bin)
    {
        if (string.IsNullOrEmpty(bin) || bin.Length < 4)
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


    protected void txtNumeroCartao_TextChanged(object sender, System.EventArgs e)
    {
        lblBandeira.Text = ObterBandeiraCartao(txtNumeroCartao.Text.ToString());

        SqlConnection mySelCadastroBandeira = new SqlConnection(Funcoes.conexao());
        mySelCadastroBandeira.Open();
        SqlCommand cmdSelCadastroBandeira = new SqlCommand("dbo.stp_bandeiras_ins", mySelCadastroBandeira);
        cmdSelCadastroBandeira.CommandType = CommandType.StoredProcedure;
        cmdSelCadastroBandeira.Parameters.Add("@flg_operacao", SqlDbType.Char).Value = "C";
        cmdSelCadastroBandeira.Parameters.Add("@NOM_BANDEIRA", SqlDbType.VarChar).Value = lblBandeira.Text.ToString();
        cmdSelCadastroBandeira.Parameters.Add("@FLG_ATIVO", SqlDbType.VarChar).Value = "S";
        cmdSelCadastroBandeira.Parameters.Add("@FLG_TIPO", SqlDbType.Char).Value = "C";
        SqlDataReader ReaderCadastroBandeira = cmdSelCadastroBandeira.ExecuteReader();
        while (ReaderCadastroBandeira.Read()) 
        {
            imgBandeira.ImageUrl = ReaderCadastroBandeira["NOM_IMAGEM"].ToString();
        }


    }
}