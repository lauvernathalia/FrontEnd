using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Web.UI;

public partial class boletobancario : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
    }

    protected void btnGerar_Click(object sender, EventArgs e)
    {
        string linhaDigitavel = txtLinhaDigitavel.Text.Trim();

        if (linhaDigitavel.Length != 47 && linhaDigitavel.Length != 48)
        {
            Response.Write("<script>alert('A linha digitável deve ter 47 ou 48 dígitos.');</script>");
            return;
        }

        // Converter para código de barras (44 dígitos)
        string codigoBarras = ConverterLinhaParaCodigoBarras(linhaDigitavel);

        // Gerar imagem do código de barras
        GerarCodigoBarras(codigoBarras);
    }

    private string ConverterLinhaParaCodigoBarras(string linhaDigitavel)
    {
        string apenasNumeros = linhaDigitavel.Replace(".", "").Replace(" ", "").Replace("-", "");

        // Verifica se a linha digitável tem 47 ou 48 caracteres
        if (apenasNumeros.Length != 47 && apenasNumeros.Length != 48)
            throw new Exception("A linha digitável deve ter 47 ou 48 caracteres.");

        // Estrutura correta para o código de barras (com 44 dígitos):
        string banco = apenasNumeros.Substring(0, 3);        // Identificação do banco (3 dígitos)
        string moeda = apenasNumeros.Substring(3, 1);         // Moeda (1 dígito)
        string fatorVencimento = apenasNumeros.Substring(19, 4); // Fator de vencimento (4 dígitos)
        string valor = apenasNumeros.Substring(23, 10);       // Valor (10 dígitos)
        string nossoNumero = apenasNumeros.Substring(4, 10);  // Nosso número (10 dígitos)
        string digitoVerificador = apenasNumeros.Substring(44, 1); // Dígito verificador (1 dígito)

        // Reorganiza a linha digitável para o formato do código de barras
        string codigoBarras = banco + moeda + fatorVencimento + valor + nossoNumero + digitoVerificador;

        // Adicionando os padrões de Start (0000) no início e Stop (100) no final
        string codigoBarrasCompleto = "0000" + codigoBarras + "100";

        return codigoBarrasCompleto;
    }

    private void GerarCodigoBarras(string codigoBarras)
    {
        int largura = 800; // Aumentado para melhor legibilidade
        int altura = 120;
        int margem = 20;

        int larguraBarraFina = 5;
        int larguraBarraGrossa = 12; // Correção: 2.5x a largura fina
        int espacoEntreBarras = 3; // Correção: espaço maior entre barras

        Bitmap bitmap = new Bitmap(largura, altura);
        using (Graphics g = Graphics.FromImage(bitmap))
        {
            g.Clear(Color.White);

            int x = margem;
            int y = 20;

            // 🚀 **Adicionando padrão de início do código de barras (Start)**
            string start = "0000"; // Start
            for (int i = 0; i < start.Length; i++)
            {
                int larguraBarra = (start[i] == '1') ? larguraBarraGrossa : larguraBarraFina;
                g.FillRectangle(Brushes.Black, x, y, larguraBarra, altura - 40);
                x += larguraBarra + espacoEntreBarras;
            }

            // 🚀 **Adicionar os pares de barras com base no código de barras**
            for (int i = 0; i < codigoBarras.Length; i += 2) // Par de números
            {
                if (i + 1 >= codigoBarras.Length) break;

                int par1 = codigoBarras[i] - '0';
                int par2 = codigoBarras[i + 1] - '0';

                string bin1 = Interleaved2of5[par1];
                string bin2 = Interleaved2of5[par2];

                for (int j = 0; j < 5; j++) // 5 pares de barras
                {
                    int largura1 = (bin1[j] == '1') ? larguraBarraGrossa : larguraBarraFina;
                    int largura2 = (bin2[j] == '1') ? larguraBarraGrossa : larguraBarraFina;

                    g.FillRectangle(Brushes.Black, x, y, largura1, altura - 40);
                    x += largura1 + espacoEntreBarras;

                    g.FillRectangle(Brushes.Black, x, y, largura2, altura - 40);
                    x += largura2 + espacoEntreBarras;
                }
            }

            // 🚀 **Adicionando padrão de fim do código de barras (Stop)**
            string stop = "100"; // Stop
            for (int i = 0; i < stop.Length; i++)
            {
                int larguraBarra = (stop[i] == '1') ? larguraBarraGrossa : larguraBarraFina;
                g.FillRectangle(Brushes.Black, x, y, larguraBarra, altura - 40);
                x += larguraBarra + espacoEntreBarras;
            }
        }

        using (MemoryStream ms = new MemoryStream())
        {
            bitmap.Save(ms, ImageFormat.Png);
            byte[] imagemBytes = ms.ToArray();
            string base64String = Convert.ToBase64String(imagemBytes);
            imgCodigoBarras.ImageUrl = "data:image/png;base64," + base64String;
            imgCodigoBarras.Visible = true;
        }
    }

    // 🚀 **Tabela Interleaved 2 of 5 - FEBRABAN**
    private readonly string[] Interleaved2of5 = new string[]
{
    "00110", // 0
    "10001", // 1
    "01001", // 2
    "11000", // 3
    "00101", // 4
    "10100", // 5
    "01100", // 6
    "00011", // 7
    "10010", // 8
    "01010"  // 9
};
}