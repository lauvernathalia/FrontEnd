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
using System.Globalization;
//using System.Drawing;
//using System.Drawing.Printing;

using iTextSharp.text;
using iTextSharp.text.pdf;

public partial class cad_locacao_autorizacao_debito : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
    }


    protected void btnEnviar_Click(object sender, EventArgs e)
    {
        ServicePointManager.Expect100Continue = true;
        ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12
        | SecurityProtocolType.Tls11
        | SecurityProtocolType.Tls
        | SecurityProtocolType.Ssl3;

        if (fileUpload.HasFile && fileUpload.PostedFile.ContentType == "application/pdf")
        {
            string StrFileName = fileUpload.PostedFile.FileName.Substring(fileUpload.PostedFile.FileName.LastIndexOf("\\") + 1);
            fileUpload.PostedFile.SaveAs(Server.MapPath("public_html") + "\\" + StrFileName);
            string NomeArquivo = Server.MapPath("public_html") + "\\" + StrFileName;
            EnviarDocumentoAutentique(NomeArquivo);
        }
        else
        {
            lblMensagem.Text = "Por favor, selecione um arquivo PDF válido.";
        }
    }

    private void EnviarDocumentoAutentique(string fileName)
    {
        var url = "https://api.autentique.com.br/v2/graphql";
        var token = "aa5a3d595b1a8ccc9987c85f0fb8407a84162fa5eafddd6f8f0ce7ad648be9f7"; // Substitua com seu token

        var filePath = fileName; // Substitua com o caminho do seu arquivo

        // Cria o corpo da requisição com os dados operations e map
        string operations = "";
        operations = operations + "{\"query\":\"mutation CreateDocumentMutation($document: DocumentInput!, $signers: [SignerInput!]!, $file: Upload!) {createDocument(document: $document, signers: $signers, file: $file) {id name refusable sortable created_at signatures { public_id name email created_at action { name } link { short_link } user { id name email }}}}\",";
        operations = operations + "\"variables\":{";
        operations = operations + "\"document\": {\"name\": \"TERMO DE AUTORIZAÇÃO DEBITO EM CONTA DE PAGAMENTO\"},\"signers\": [{\"name\": \"ADRIANO LEVY BARBOSA\",\"email\": \"webview.solucoes@gmail.com\",\"action\": \"SIGN\"}],\"file\":null}}";
        
        
        string map = "{\"file\": [\"variables.file\"]}";

        // Gera o boundary que será utilizado no multipart
        string boundary = "----WebKitFormBoundary" + DateTime.Now.Ticks.ToString("x");

        // Converte os dados para byte array
        byte[] operationsBytes = Encoding.UTF8.GetBytes(operations);
        byte[] mapBytes = Encoding.UTF8.GetBytes(map);
        byte[] fileBytes = File.ReadAllBytes(filePath);
        //fileName = Path.GetFileName(filePath);

        // Cria a requisição HTTP
        HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
        request.Method = "POST";
        request.Headers.Add("Authorization", "Bearer " + token);
        request.ContentType = "multipart/form-data; boundary=" + boundary;

        // Abre o fluxo de escrita do request
        using (Stream requestStream = request.GetRequestStream())
        {
            // Escreve a parte "operations"
            WriteFormData(requestStream, boundary, "operations", operationsBytes);

            // Escreve a parte "map"
            WriteFormData(requestStream, boundary, "map", mapBytes);

            // Escreve a parte do arquivo
            requestStream.Write(Encoding.ASCII.GetBytes("\r\n--" + boundary + "\r\n"), 0, Encoding.ASCII.GetByteCount("\r\n--" + boundary + "\r\n"));
            requestStream.Write(Encoding.ASCII.GetBytes("Content-Disposition: form-data; name=\"file\"; filename=\"" + fileName + "\"\r\n"), 0, Encoding.ASCII.GetByteCount("Content-Disposition: form-data; name=\"file\"; filename=\"" + fileName + "\"\r\n"));
            requestStream.Write(Encoding.ASCII.GetBytes("Content-Type: application/octet-stream\r\n\r\n"), 0, Encoding.ASCII.GetByteCount("Content-Type: application/octet-stream\r\n\r\n"));
            requestStream.Write(fileBytes, 0, fileBytes.Length);
            requestStream.Write(Encoding.ASCII.GetBytes("\r\n--" + boundary + "--\r\n"), 0, Encoding.ASCII.GetByteCount("\r\n--" + boundary + "--\r\n"));
        }

        // Envia a requisição e obtém a resposta
        using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
        {
            using (StreamReader reader = new StreamReader(response.GetResponseStream()))
            {
                string responseContent = reader.ReadToEnd();
                lblMensagem.Text = responseContent;
            }
        }
    }
    
    
    static void WriteFormData(Stream requestStream, string boundary, string name, byte[] content)
    {
        requestStream.Write(Encoding.ASCII.GetBytes("\r\n--" + boundary + "\r\n"), 0, Encoding.ASCII.GetByteCount("\r\n--" + boundary + "\r\n"));
        requestStream.Write(Encoding.ASCII.GetBytes("Content-Disposition: form-data; name=\"" + name + "\"\r\n\r\n"), 0, Encoding.ASCII.GetByteCount("Content-Disposition: form-data; name=\"" + name + "\"\r\n\r\n"));
        requestStream.Write(content, 0, content.Length);
    }

    /*
    protected void btnGerarPDF_Click(object sender, System.EventArgs e)
    {
        string nome = txtNome.Text;
        string cidade = txtCidade.Text;
        string cnpj = txtCNPJ.Text;

        // Modelo de contrato com placeholders
        string contratoTexto = @"
TERMO DE AUTORIZAÇÃO PARA DÉBITO AUTOMÁTICO EM CONTA DE PAGAMENTO
                
Pelo presente instrumento particular, {{Nome}}, inscrita no CNPJ/CPF sob o nº {{CNPJ}}, com sede na xxxxxxxxx ,xx, Bairro xxxxx na cidade de {{Cidade}}, Estado de xxx, CEP xxx, neste ato representada na forma de seus atos constitutivos (“Estabelecimento”), firmado em caráter irrevogável e irretratável, autorizo a ZOOP TECNOLOGIA E MEIOS DE PAGAMENTO S.A., inscrita no CNPJ/MF nº 19.468.242/0001-32 a debitar da minha Conta de Pagamento mantida junto à ZOOP sob o id: xxxxxxxxxxxxxxx o valor correspondente aos valores negociados entre o Estabelecimento com LEGACY TECNOLOGIA E PAGAMENTOS LTDA , inscrita no CNPJ nº 37.917.825/0001-85, com sede na Rua Dos Alamos, nº 66B, bairro Setor Comercial, cidade de Sinop, estado do MT, CEP 78.550-188, neste ato representada na forma de seus atos constitutivos (“Parceiro”). 
                                
CONSIDERANDO QUE:

1. Para cumprimento das normas dos reguladores, dentre eles o BACEN (Banco Central do Brasil), a ZOOP, necessariamente precisa de uma autorização expressa do Estabelecimento para realização de débitos da conta de pagamento para repasses de valores ao Parceiro; 
2. O Estabelecimento fornece todos os poderes ao Parceiro para realização dos débitos na conta de pagamento de sua titularidade; e 
3. O Estabelecimento concorda e reconhece que a ZOOP apenas realiza os débitos nas contas de pagamento por conta e ordem do Parceiro, em razão da relação contratual que o Estabelecimento possui diretamente com o Parceiro, estando isenta de quaisquer responsabilidades acerca de eventuais irregularidades quanto ao valor dos débitos realizados.
Resolvem as Partes celebrar o presente Termo de Autorização Para Débito Automático em Conta de Pagamento (“TERMO”), que se regerá pelas cláusulas e condições a seguir.

CLÁUSULA PRIMEIRA – AUTORIZAÇÃO PARA DÉBITO EM CONTA DE PAGAMENTO

1.1. O Estabelecimento autoriza expressamente o Parceiro a realizar débitos em sua conta de pagamento.
1.2. O Estabelecimento também se compromete desde já a manter saldo suficiente para os referidos débitos, ficando a ZOOP isenta de qualquer responsabilidade decorrente da não liquidação do compromisso por insuficiência de saldo na data do vencimento.
1.3. O Estabelecimento será o único responsável por comunicar a ZOOP, através do e-mail optin@zoop.com.br, quanto ao cancelamento da autorização de realização dos débitos na conta de pagamento de sua titularidade. Fica ressalvado que o Estabelecimento, no ato da comunicação por email listado nesta cláusula, poderá colocar o Parceiro em cópia para conhecimento e controle.
1.4. O Estabelecimento autoriza a ZOOP expressamente a informar e disponibilizar ao Parceiro sobre todas as Transações realizadas pelo Sistema Zoop, incluindo a quantidade de Transações realizadas e seu respectivo valor e destino, para fins de formulação de análise de risco e apuração dos valores pagos e a receber.
CLÁUSULA SEGUNDA – CONDIÇÕES GERAIS

2.1. As Partes declaram, sob as penas da Lei, que os signatários do presente instrumento são seus representantes legais, devidamente constituídos na forma dos respectivos documentos societários, com poderes para assumir as obrigações ora contratadas.
2.2. Em caso de dúvida ou reclamação sobre datas de vencimento e/ou valores, devo solicitar esclarecimentos diretamente ao Parceiro. 
2.3.A formalização do presente instrumento não conflita com o teor dos Termos e Condições de Uso do Sistema Zoop, bem como do Contrato de Prestação Conjunta de Serviços, que permanecem integralmente vigentes.
2.4. Fica eleito o Foro Central da Comarca da Capital do Estado de São Paulo como único e exclusivamente competente para dirimir quaisquer dúvidas ou questões oriundas deste Instrumento.
Sinop, 08 de Março de 2025.


_______________________________________________________________
LEGACY TECNOLOGIA E PAGAMENTOS LTDA
NOME :Leandro Giacomini
CPF: 843.511.951-34
CARGO:Ceo



_______________________________________________________________
{{Nome}}
CNPJ/CPF: {{CNPJ}}


1. _____________________________ 



2. _____________________________";

        // Substitui os placeholders pelos valores reais
        contratoTexto = contratoTexto.Replace("{{Nome}}", nome)
                                     .Replace("{{Cidade}}", cidade)
                                     .Replace("{{CNPJ}}", cnpj);

        // Gera o PDF
        lblMensagem.Text = contratoTexto;
        GerarPDF(contratoTexto);
    }

    private void GerarPDF(string textoContrato)
    {
        Document documento = new Document(PageSize.A4, 50, 50, 50, 50);
        MemoryStream memoryStream = new MemoryStream();

        try
        {
            PdfWriter writer = PdfWriter.GetInstance(documento, memoryStream);
            documento.Open();
            documento.Add(new Paragraph(textoContrato));
        }
        finally
        {
            documento.Close();
        }

        // Enviar o PDF para download
        byte[] bytes = memoryStream.ToArray();
        memoryStream.Close();

        Response.Clear();
        Response.ContentType = "application/pdf";
        Response.AddHeader("content-disposition", "attachment; filename=Contrato.pdf");
        Response.BinaryWrite(bytes);
        Response.End();
    }
    */
    
     
        protected void btnGerarPDF_Click(object sender, EventArgs e)
        {
            string nome = txtNome.Text;
            string cidade = txtCidade.Text;
            string cnpj = txtCNPJ.Text;

            // Caminho do PDF gerado
            string Codificacao = DateTime.Now.Year.ToString() + DateTime.Now.Month.ToString() + DateTime.Now.Day.ToString() + DateTime.Now.Hour.ToString() + DateTime.Now.Minute.ToString() + DateTime.Now.Second.ToString();
            string caminhoPDF = Server.MapPath("public_html") + "\\" + HttpContext.Current.Session["CODIGO"].ToString().Trim() + "_" + Codificacao.ToString() + "_" + "AUTORIZACAO_DEBITO.pdf";

            // Criar PDF
            CriarContratoPDF(nome, cidade, cnpj, caminhoPDF);

            // Enviar PDF para download
            EnviarPDFParaDownload(caminhoPDF);
        }     
      
        private void CriarContratoPDF(string nome, string cidade, string cnpj, string caminhoPDF)
        {
            using (FileStream stream = new FileStream(caminhoPDF, FileMode.Create))
            {
                Document document = new Document(PageSize.A4, 40, 40, 40, 40);
                PdfWriter writer = PdfWriter.GetInstance(document, stream);

                document.Open();

                // Definição de fontes
                Font fonteTitulo = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 14);
                Font fonteSubtitulo = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 12);
                Font fonteTexto = FontFactory.GetFont(FontFactory.HELVETICA, 10);

                // **Título do contrato**
                Paragraph titulo = new Paragraph("TERMO DE AUTORIZAÇÃO PARA DÉBITO AUTOMÁTICO EM CONTA DE PAGAMENTO", fonteTitulo);
                titulo.Alignment = Element.ALIGN_CENTER;
                titulo.SpacingAfter = 20;
                document.Add(titulo);

                // **Texto do contrato**
                string contratoTexto = string.Format(@"Pelo presente instrumento particular, {0}, inscrita no CNPJ/CPF sob o nº {1}, com sede na {2} ,{3}, Bairro {4} na cidade de {5}, Estado de {6}, CEP {7}, neste ato representada na forma de seus atos constitutivos (“Estabelecimento”), firmado em caráter irrevogável e irretratável, autorizo a ZOOP TECNOLOGIA E MEIOS DE PAGAMENTO S.A., inscrita no CNPJ/MF nº 19.468.242/0001-32 a debitar da minha Conta de Pagamento mantida junto à ZOOP sob o id: {8} o valor correspondente aos valores negociados entre o Estabelecimento com LEGACY TECNOLOGIA E PAGAMENTOS LTDA , inscrita no CNPJ nº 37.917.825/0001-85, com sede na Rua Dos Alamos, nº 66B, bairro Setor Comercial, cidade de Sinop, estado do MT, CEP 78.550-188, neste ato representada na forma de seus atos constitutivos (“Parceiro”).", nome, cnpj, cidade,"","","","","",""); 
                Paragraph textoContrato = new Paragraph(contratoTexto, fonteTexto);
                textoContrato.SpacingAfter = 15;
                document.Add(textoContrato);

                // **Subtítulo (negrito)**
                Paragraph consideracoes = new Paragraph("CONSIDERNDO QUE:", fonteSubtitulo);
                consideracoes.SpacingBefore = 10;
                consideracoes.SpacingAfter = 5;
                document.Add(consideracoes);

                // **Textos**
                string consideracoes1 = "1. Para cumprimento das normas dos reguladores, dentre eles o BACEN (Banco Central do Brasil), a ZOOP, necessariamente precisa de uma autorização expressa do Estabelecimento para realização de débitos da conta de pagamento para repasses de valores ao Parceiro; ";
                document.Add(new Paragraph(consideracoes1, fonteTexto));
                string consideracoes2 = "\n2. O Estabelecimento fornece todos os poderes ao Parceiro para realização dos débitos na conta de pagamento de sua titularidade; e";
                document.Add(new Paragraph(consideracoes2, fonteTexto));
                string consideracoes3 = "\n3. O Estabelecimento concorda e reconhece que a ZOOP apenas realiza os débitos nas contas de pagamento por conta e ordem do Parceiro, em razão da relação contratual que o Estabelecimento possui diretamente com o Parceiro, estando isenta de quaisquer responsabilidades acerca de eventuais irregularidades quanto ao valor dos débitos realizados.";
                document.Add(new Paragraph(consideracoes3, fonteTexto));
                string consideracoes4 = "\nResolvem as Partes celebrar o presente Termo de Autorização Para Débito Automático em Conta de Pagamento (“TERMO”), que se regerá pelas cláusulas e condições a seguir.";
                document.Add(new Paragraph(consideracoes4, fonteTexto));


                // **Subtítulo (negrito)**
                Paragraph clausula1 = new Paragraph("CLÁUSULA PRIMEIRA – AUTORIZAÇÃO PARA DÉBITO EM CONTA DE PAGAMENTO", fonteSubtitulo);
                clausula1.SpacingBefore = 10;
                clausula1.SpacingAfter = 5;
                document.Add(clausula1);

                // **Textos**
                string clausula1_1 = "1.1. O Estabelecimento autoriza expressamente o Parceiro a realizar débitos em sua conta de pagamento.";
                document.Add(new Paragraph(clausula1_1, fonteTexto));
                string clausula1_2 = "\n1.2. O Estabelecimento também se compromete desde já a manter saldo suficiente para os referidos débitos, ficando a ZOOP isenta de qualquer responsabilidade decorrente da não liquidação do compromisso por insuficiência de saldo na data do vencimento.";
                document.Add(new Paragraph(clausula1_2, fonteTexto));
                string clausula1_3 = "\n1.3. O Estabelecimento será o único responsável por comunicar a ZOOP, através do e-mail optin@zoop.com.br, quanto ao cancelamento da autorização de realização dos débitos na conta de pagamento de sua titularidade. Fica ressalvado que o Estabelecimento, no ato da comunicação por email listado nesta cláusula, poderá colocar o Parceiro em cópia para conhecimento e controle. ";
                document.Add(new Paragraph(clausula1_3, fonteTexto));
                string clausula1_4 = "\n1.4. O Estabelecimento autoriza a ZOOP expressamente a informar e disponibilizar ao Parceiro sobre todas as Transações realizadas pelo Sistema Zoop, incluindo a quantidade de Transações realizadas e seu respectivo valor e destino, para fins de formulação de análise de risco e apuração dos valores pagos e a receber.";
                document.Add(new Paragraph(clausula1_4, fonteTexto));

                // **Subtítulo (negrito)**
                Paragraph clausula2 = new Paragraph("CLÁUSULA SEGUNDA – CONDIÇÕES GERAIS", fonteSubtitulo);
                clausula2.SpacingBefore = 10;
                clausula2.SpacingAfter = 5;
                document.Add(clausula2);

                // **Textos**
                string clausula2_1 = "2.1. As Partes declaram, sob as penas da Lei, que os signatários do presente instrumento são seus representantes legais, devidamente constituídos na forma dos respectivos documentos societários, com poderes para assumir as obrigações ora contratadas.";
                document.Add(new Paragraph(clausula2_1, fonteTexto));
                string clausula2_2 = "\n2.2. Em caso de dúvida ou reclamação sobre datas de vencimento e/ou valores, devo solicitar esclarecimentos diretamente ao Parceiro.";
                document.Add(new Paragraph(clausula2_2, fonteTexto));
                string clausula2_3 = "\n2.3. A formalização do presente instrumento não conflita com o teor dos Termos e Condições de Uso do Sistema Zoop, bem como do Contrato de Prestação Conjunta de Serviços, que permanecem integralmente vigentes.";
                document.Add(new Paragraph(clausula2_3, fonteTexto));
                string clausula2_4 = "\n2.4. Fica eleito o Foro Central da Comarca da Capital do Estado de São Paulo como único e exclusivamente competente para dirimir quaisquer dúvidas ou questões oriundas deste Instrumento.";
                document.Add(new Paragraph(clausula2_4, fonteTexto));

                // **Data**
                Paragraph Data = new Paragraph("\n\n\n" + "São Paulo" + ", " + DateTime.Now.Day.ToString().PadLeft(2, '0') + " de " + DateTime.Now.ToString(@"MMMM", new CultureInfo("PT-pt")) + " de " + DateTime.Now.Year.ToString().PadLeft(4, '0'), fonteTexto);
                Data.Alignment = Element.ALIGN_CENTER;
                Data.SpacingBefore = 40;
                document.Add(Data);



                // **Assinaturas**
                Paragraph assinatura1 = new Paragraph("\n\n\n___________________________\n" + txtNome.Text.ToString() + "\n" + txtCNPJ.Text.ToString(), fonteTexto);
                assinatura1.Alignment = Element.ALIGN_CENTER;
                assinatura1.SpacingBefore = 40;
                document.Add(assinatura1);

                Paragraph assinatura2 = new Paragraph("\n\n\n___________________________\n" + txtNome.Text.ToString() + "\n" + txtCNPJ.Text.ToString(), fonteTexto);
                assinatura2.Alignment = Element.ALIGN_CENTER;
                assinatura2.SpacingBefore = 40;
                document.Add(assinatura2);


                document.Close();
                writer.Close();
            }
        }      
     
        private void EnviarPDFParaDownload(string caminhoPDF)
        {
            if (File.Exists(caminhoPDF))
            {
                Response.ContentType = "application/pdf";
                Response.AppendHeader("Content-Disposition", "attachment; filename=Contrato.pdf");
                Response.TransmitFile(caminhoPDF);
                Response.End();
            }
        }     
     

}