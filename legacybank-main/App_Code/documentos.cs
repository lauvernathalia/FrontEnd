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

/// <summary>
/// Summary description for documentos
/// </summary>
public class documentos
{
    public static string CriarAutorizacaoDebitoPDF(int pessoa, int licenciado, string caminhoPDF)
    {
        
            SqlConnection mySelCadastro = new SqlConnection(Funcoes.conexao());
            mySelCadastro.Open();
            SqlCommand cmdSelCadastro = new SqlCommand("dbo.stp_pessoas_fj_ins", mySelCadastro);
            cmdSelCadastro.CommandType = CommandType.StoredProcedure;
            cmdSelCadastro.Parameters.Add("@flg_operacao", SqlDbType.Char).Value = "S";
            cmdSelCadastro.Parameters.Add("@COD_ID_PESSOA_LICENCIADO", SqlDbType.Int).Value = licenciado;
            cmdSelCadastro.Parameters.Add("@COD_ID", SqlDbType.Int).Value = pessoa;
            SqlDataReader ReaderCadastro = cmdSelCadastro.ExecuteReader();
            while (ReaderCadastro.Read())
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
                    string contratoTexto = string.Format(@"Pelo presente instrumento particular, {0}, inscrita no CNPJ/CPF sob o nº {1}, com sede na {2} ,{3}, Bairro {4} na cidade de {5}, Estado de {6}, CEP {7}, neste ato representada na forma de seus atos constitutivos (“Estabelecimento”), firmado em caráter irrevogável e irretratável, autorizo a ZOOP TECNOLOGIA E MEIOS DE PAGAMENTO S.A., inscrita no CNPJ/MF nº 19.468.242/0001-32 a debitar da minha Conta de Pagamento mantida junto à ZOOP sob o id: {8} o valor correspondente aos valores negociados entre o Estabelecimento com LEGACY TECNOLOGIA E PAGAMENTOS LTDA , inscrita no CNPJ nº 37.917.825/0001-85, com sede na Rua Dos Alamos, nº 66B, bairro Setor Comercial, cidade de Sinop, estado do MT, CEP 78.550-188, neste ato representada na forma de seus atos constitutivos (“Parceiro”).", ReaderCadastro["NOM_RAZAOSOCIAL"].ToString(), ReaderCadastro["NOM_CNPJ"].ToString(), ReaderCadastro["NOM_ENDERECO"].ToString(), ReaderCadastro["NOM_NUMERO"].ToString() + " " + ReaderCadastro["NOM_COMPLEMENTO"].ToString(), ReaderCadastro["NOM_BAIRRO"].ToString(), ReaderCadastro["NOM_CIDADE"].ToString(), ReaderCadastro["NOM_UF"].ToString(), ReaderCadastro["NOM_CEP"].ToString(), ReaderCadastro["NUM_TOKEN"].ToString());
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
                    Paragraph Data = new Paragraph("\n\n\n" + ReaderCadastro["NOM_CIDADE"].ToString() + ", " + DateTime.Now.Day.ToString().PadLeft(2, '0') + " de " + DateTime.Now.ToString(@"MMMM", new CultureInfo("PT-pt")) + " de " + DateTime.Now.Year.ToString().PadLeft(4, '0'), fonteTexto);
                    Data.Alignment = Element.ALIGN_CENTER;
                    Data.SpacingBefore = 40;
                    document.Add(Data);

                    // LEGACY TECNOLOGIA E PAGAMENTOS LTDA 37.917.825/0001-85

                    // **Assinaturas**
                    Paragraph assinatura1 = new Paragraph("\n\n\n___________________________\nLEGACY TECNOLOGIA E PAGAMENTOS LTDA\nCNPJ/CPF 37.917.825/0001-85", fonteTexto);
                    assinatura1.Alignment = Element.ALIGN_CENTER;
                    assinatura1.SpacingBefore = 40;
                    document.Add(assinatura1);

                    Paragraph assinatura2 = new Paragraph("\n\n\n___________________________\n" + ReaderCadastro["NOM_RAZAOSOCIAL"].ToString() + "\nCNPJ/CPF" + ReaderCadastro["NOM_CNPJ"].ToString(), fonteTexto);
                    assinatura2.Alignment = Element.ALIGN_CENTER;
                    assinatura2.SpacingBefore = 40;
                    document.Add(assinatura2);


                    document.Close();
                    writer.Close();
                }
            }
            return caminhoPDF;
    }

    public static string EnviarDocumentoAutentique(string documento, string assinante, string email, string assinantelicenciado, string emaillicenciado, string fileName)
    {
        string responseContent = "";
        
        var url = "https://api.autentique.com.br/v2/graphql";
        var token = "aa5a3d595b1a8ccc9987c85f0fb8407a84162fa5eafddd6f8f0ce7ad648be9f7"; // Substitua com seu token

        var filePath = fileName; // Substitua com o caminho do seu arquivo

        // Cria o corpo da requisição com os dados operations e map
        string operations = "";
        operations = operations + "{\"query\":\"mutation CreateDocumentMutation($document: DocumentInput!, $signers: [SignerInput!]!, $file: Upload!) {createDocument(document: $document, signers: $signers, file: $file) {id name refusable sortable created_at signatures { public_id name email created_at action { name } link { short_link } user { id name email }}}}\",";
        operations = operations + "\"variables\":{";
        operations = operations + "\"document\": {\"name\": \"" + documento + "\"},\"signers\": [{\"name\": \"" + assinantelicenciado + "\",\"email\": \"" + emaillicenciado + "\",\"action\": \"SIGN\"},{\"name\": \"" + assinante + "\",\"email\": \"" + email + "\",\"action\": \"SIGN\"} ],\"file\":null}}";
        
        
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
                responseContent = reader.ReadToEnd();
            }
        }
        return responseContent;
    }
    
    
    static void WriteFormData(Stream requestStream, string boundary, string name, byte[] content)
    {
        requestStream.Write(Encoding.ASCII.GetBytes("\r\n--" + boundary + "\r\n"), 0, Encoding.ASCII.GetByteCount("\r\n--" + boundary + "\r\n"));
        requestStream.Write(Encoding.ASCII.GetBytes("Content-Disposition: form-data; name=\"" + name + "\"\r\n\r\n"), 0, Encoding.ASCII.GetByteCount("Content-Disposition: form-data; name=\"" + name + "\"\r\n\r\n"));
        requestStream.Write(content, 0, content.Length);
    }



	public documentos()
	{
		//
		// TODO: Add constructor logic here
		//
	}
}