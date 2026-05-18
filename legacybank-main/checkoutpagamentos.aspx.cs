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


public partial class checkoutpagamentos : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        Console.Write("Pagina lida " + DateTime.Now.ToString());
    }

    protected void btnAdiq_Click(object sender, System.EventArgs e)
    {
        Console.Write("click botão " + DateTime.Now.ToString());

        DadosTokenCartaoAdiq.TokenCartao dtoken = new DadosTokenCartaoAdiq.TokenCartao()
        {
            cardNumber = txtCartao.Text.ToString()
        };
        string jsonCartao = JsonConvert.SerializeObject(dtoken);
        txtTokenCartao.Text = adiq.GerarTokenCartao(jsonCartao);


        string jsonBIN = adiq.ConsultarBINCartao(txtCartao.Text.ToString().Substring(0, 6));
        txtJsonAdiq.Text = jsonBIN.ToString();

        //JArray oBIN = JArray.Parse(jsonBIN); 
        //txtBandeira.Text = oBIN[0]["brand"].ToString();
        //try { JObject oBIN = JObject.Parse(jsonBIN); txtBandeira.Text = oBIN["brand"].ToString(); }
        //catch { txtBandeira.Text = "Não Identificado"; }

        // Criar Pagamento

        //Guid g = Guid.NewGuid();
        //Console.WriteLine(g);
        //Console.WriteLine(Guid.NewGuid());
        string jsonPagamento = "";
        // Gerar Número Ordem Aleatório

        Random randNum = new Random();
        string nOrdem = randNum.Next().ToString();
        txtNumeroOrdem.Text = nOrdem;

        if (ddlCofre.SelectedValue.ToString() == "N")
        {
            DadosPagamentoTokenAdiq.Root dpagamento = new DadosPagamentoTokenAdiq.Root()
            {
                payment = new DadosPagamentoTokenAdiq.Payment()
                {
                    transactionType = "credit",
                    currencyCode = "brl",
                    productType = ddlTipo.SelectedValue.ToString(),
                    captureType = ddlCaptura.SelectedValue.ToString(),
                    recurrent = false,
                    installments = Funcoes.strToInt(txtParcela.Text.ToString()),
                    amount = 1000
                },
                cardInfo = new DadosPagamentoTokenAdiq.CardInfo()
                {
                    numberToken = txtTokenCartao.Text.ToString().Trim(),
                    brand = txtBandeira.Text.ToString(),
                    cardholderName = "ADRIANO LEVY BARBOSA",
                    expirationMonth = txtValidade.Text.ToString().Substring(0, 2),
                    expirationYear = txtValidade.Text.ToString().Substring(3, 2),
                    securityCode = txtCVV.Text.ToString()
                    //vaultId = txtCofreCartao.Text.ToString()
                },
                Customer = new DadosPagamentoTokenAdiq.Customer()
                {
                    DocumentType = "cpf",
                    DocumentNumber = "01001905709",
                    FirstName = "Adriano",
                    LastName = "Barbosa",
                    Email = "adriano@webview.com.br",
                    MobilePhoneNumber = "11970308508",
                    Address = "Rua Paulo Orozimbo",
                    IpAddress = Request.ServerVariables["REMOTE_ADDR"].ToString(),
                    Complement = "CASA",
                    State = "SP",
                    ZipCode = "01535001",
                    City = "São Paulo",
                    Country = "BR",
                    PhoneNumber = "1155737555"
                },
                ShipTo = new DadosPagamentoTokenAdiq.ShipTo()
                {
                    FirstName = "Adriano",
                    LastName = "Barbosa",
                    PhoneNumber = "1155737555",
                    Address = "Rua Paulo Orozimbo",
                    Complement = "CASA",
                    State = "SP",
                    ZipCode = "01535001",
                    City = "São Paulo",
                    Country = "BR"
                },
                LineItems = new DadosPagamentoTokenAdiq.LineItems[]
                {
                    new DadosPagamentoTokenAdiq.LineItems()
                    {
                       Quantity="1",
                       UnitPrice="100",
                       ProductSKU="12121212",
                       ProductCode="12121212",
                       ProductName="TESTE DE PRODUTO"
                    }
                },
                sellerInfo = new DadosPagamentoTokenAdiq.SellerInfo()
                {
                    orderNumber = txtNumeroOrdem.Text.ToString(),
                    codeAntiFraud = txtGUID.Text.ToString(),
                    softDescriptor = "PAG*CheckOutPRD",
                    Antifraud = new DadosPagamentoTokenAdiq.Antifraud()
                    {
                        CheckDocBearer = false
                    }
                }
            };

            Console.Write("sem cofre " + DateTime.Now.ToString());
            jsonPagamento = JsonConvert.SerializeObject(dpagamento);
            txtJsonEnvio.Text = jsonPagamento.ToString();

        }
        else
        {

            // Gerar Cofre
            DadosCofreCartaoAdiq.Root dcofre = new DadosCofreCartaoAdiq.Root()
            {
                numberToken = txtTokenCartao.Text.ToString(),
                brand = txtBandeira.Text.ToString(),
                cardholderName = "ADRIANO LEVY BARBOSA",
                expirationMonth = txtValidade.Text.ToString().Substring(0, 2),
                expirationYear = txtValidade.Text.ToString().Substring(3, 2),
                verifyCard = false,
                securityCode = txtCVV.Text.ToString()
            };


            string jsonCofre = JsonConvert.SerializeObject(dcofre);
            txtCofreCartao.Text = adiq.CadastrarCofreCartao(jsonCofre);
            //txtGUID.Text = Guid.NewGuid().ToString();

            DadosPagamentoCofreAdiq.Root dpagamento = new DadosPagamentoCofreAdiq.Root()
            {
                payment = new DadosPagamentoCofreAdiq.Payment()
                {
                    transactionType = "credit",
                    currencyCode = "brl",
                    productType = ddlTipo.SelectedValue.ToString(),
                    captureType = ddlCaptura.SelectedValue.ToString(),
                    recurrent = false,
                    installments = Funcoes.strToInt(txtParcela.Text.ToString()),
                    amount = 1000
                },
                cardInfo = new DadosPagamentoCofreAdiq.CardInfo()
                {
                    vaultId = txtCofreCartao.Text.ToString()
                },
                Customer = new DadosPagamentoCofreAdiq.Customer()
                {
                    DocumentType = "cpf",
                    DocumentNumber = "01001905709",
                    FirstName = "Adriano",
                    LastName = "Barbosa",
                    Email = "adriano@webview.com.br",
                    MobilePhoneNumber = "11970308508",
                    Address = "Rua Paulo Orozimbo",
                    IpAddress = Request.ServerVariables["REMOTE_ADDR"].ToString(),
                    Complement = "CASA",
                    State = "SP",
                    ZipCode = "01535001",
                    City = "São Paulo",
                    Country = "BR",
                    PhoneNumber = "1155737555"
                },
                sellerInfo = new DadosPagamentoCofreAdiq.SellerInfo()
                {
                    orderNumber = txtNumeroOrdem.Text.ToString(),
                    codeAntiFraud = txtGUID.Text.ToString() 
                }
            };


            jsonPagamento = JsonConvert.SerializeObject(dpagamento);

            txtJsonEnvio.Text = jsonPagamento.ToString();

        }
        txtJsonAdiq.Text = adiq.CadastrarPagamentos(jsonPagamento);
        // Preencher ID Pagamento e Autorização
        if (txtJsonAdiq.Text.ToString().Trim() != "")
        {
            try
            {
                JObject o = JObject.Parse(txtJsonAdiq.Text.ToString());
                txtIDPagamento.Text = o["paymentAuthorization"]["paymentId"].ToString();
                txtAutorizacao.Text = o["paymentAuthorization"]["authorizationCode"].ToString();
            }
            catch
            {
                txtIDPagamento.Text = "";
                txtAutorizacao.Text = "";
            }

        }

        if (ddlOperacao.SelectedValue.ToString() == "T")
        {
            if (txtIDPagamento.Text.ToString().Trim() != "")
            {
                txtJsonAdiq.Text = txtJsonAdiq.Text + "   -   " + adiq.ConsultarPagamentos(txtIDPagamento.Text.ToString());
            }
        }

        if (ddlOperacao.SelectedValue.ToString() == "L")
        {
            if (txtIDPagamento.Text.ToString().Trim() != "")
            {
                txtJsonAdiq.Text = txtJsonAdiq.Text + "   -   " + adiq.ConsultarPagamentos(txtIDPagamento.Text.ToString());
            }
            if (txtIDPagamento.Text.ToString().Trim() != "")
            {
                DadosCancelarPagamento.Root dcancelar = new DadosCancelarPagamento.Root()
                {
                    amount = 1000
                };

                string jsonCancelar = JsonConvert.SerializeObject(dcancelar);

                txtJsonAdiq.Text = txtJsonAdiq.Text + "   -   " + adiq.CancelarPagamentos(txtIDPagamento.Text.ToString(), jsonCancelar);
            }

        }

        //Cancelar
        if (ddlOperacao.SelectedValue.ToString() == "C")
        {
            if (txtIDPagamento.Text.ToString().Trim() != "")
            {
                DadosCancelarPagamento.Root dcancelar = new DadosCancelarPagamento.Root()
                {
                    amount = 1000
                };

                string jsonCancelar = JsonConvert.SerializeObject(dcancelar);

                txtJsonAdiq.Text = txtJsonAdiq.Text + "   -   " + adiq.CancelarPagamentos(txtIDPagamento.Text.ToString(), jsonCancelar);
            }
        }
        // Captura
        if (ddlOperacao.SelectedValue.ToString() == "P")
        {
            if (txtIDPagamento.Text.ToString().Trim() != "")
            {
                DadosCapturarPagamento.Root dcapturar = new DadosCapturarPagamento.Root()
                {
                    amount = 1000
                };

                string jsonCapturar = JsonConvert.SerializeObject(dcapturar);

                txtJsonAdiq.Text = txtJsonAdiq.Text + "   -   " + adiq.CapturarPagamentos(txtIDPagamento.Text.ToString(), jsonCapturar);
            }

        }

        if (ddlOperacao.SelectedValue.ToString() == "X")
        {
            if (txtIDPagamento.Text.ToString().Trim() != "")
            {
                DadosCapturarPagamento.Root dcapturar = new DadosCapturarPagamento.Root()
                {
                    amount = 1000
                };

                string jsonCapturar = JsonConvert.SerializeObject(dcapturar);

                txtJsonAdiq.Text = txtJsonAdiq.Text + "   -   " + adiq.CapturarPagamentos(txtIDPagamento.Text.ToString(), jsonCapturar);
            }
            if (txtIDPagamento.Text.ToString().Trim() != "")
            {
                DadosCancelarPagamento.Root dcancelar = new DadosCancelarPagamento.Root()
                {
                    amount = 1000
                };

                string jsonCancelar = JsonConvert.SerializeObject(dcancelar);

                txtJsonAdiq.Text = txtJsonAdiq.Text + "   -   " + adiq.CancelarPagamentos(txtIDPagamento.Text.ToString(), jsonCancelar);
            }

        }

        txtJsonAdiq.Text = txtJsonAdiq.Text + " -- (" + txtGUID.Text.ToString() + ") --   ";

    }

    public string afGUID()
    {
        return txtGUID.Text.ToString();
    }

    public string AntifraudeGUID()
    {
        txtGUID.Text = Guid.NewGuid().ToString();


        return txtGUID.Text.ToString();
    }
}