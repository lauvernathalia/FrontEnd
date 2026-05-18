using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Services;

public partial class teste : System.Web.UI.Page
{
    private static int ProgressoImportacao = 0;
    private static bool EmAndamento = false;

    [WebMethod]
    public static string IniciarImportacao()
    {
        if (EmAndamento) return "Já existe uma importação em andamento.";

        EmAndamento = true;
        ProgressoImportacao = 0;

        System.Threading.Tasks.Task.Run(() =>
        {
            try
            {
                int total = 1576; // simulação, você usaria o total real
                for (int i = 0; i < total; i++)
                {
                    System.Threading.Thread.Sleep(100); // simula tempo de processamento
                    ProgressoImportacao = (i + 1) * 100 / total;
                }
            }
            finally
            {
                EmAndamento = false;
            }
        });

        return "Processo iniciado!";
    }

    [WebMethod]
    public static int GetProgresso()
    {
        return ProgressoImportacao;
    }
    protected void Page_Load(object sender, EventArgs e)
    {

    }

}