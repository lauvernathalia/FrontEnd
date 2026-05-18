using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;

using System.Data;
using System.Configuration;
using System.Collections;
using System.Web.Security;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Security.Cryptography;
using System.Text;
using System.IO;
using System.Net;
using System.Net.Mail;
using System.IO.IsolatedStorage;
using System.Runtime.Serialization;

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


public partial class transacao : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }
    protected async void btnInserir_Click(object sender, EventArgs e)
    {


        try
            {
                int resultado = await InserirTransacaoAsync();

                lblResultado.Text = "Linhas afetadas:" + resultado.ToString();
            }
            catch (Exception ex)
            {
                lblResultado.Text = "Erro:" + ex.Message.ToString();
            }
    }

     private async Task<int> InserirTransacaoAsync()
        {
            string connectionString = Funcoes.conexao(); // Sua função de conexão
            /*
            using (SqlConnection connInsCons = new SqlConnection(connectionString))
            {
                await connInsCons.OpenAsync(); // Abre a conexão de forma assíncrona

                using (SqlCommand cmdInsCons = new SqlCommand("dbo.stp_ocorrencias_ins", connInsCons))
                {
                    cmdInsCons.CommandType = CommandType.StoredProcedure;
                    cmdInsCons.CommandTimeout = 0; 
                    cmdInsCons.Parameters.Add("@flg_operacao", SqlDbType.Char).Value = "T";
                    cmdInsCons.Parameters.Add("@COD_ID_PESSOA_LICENCIADO", SqlDbType.Int).Value = 1;

                    // Executa a procedure de forma assíncrona
                    return await cmdInsCons.ExecuteNonQueryAsync().ConfigureAwait(false); 
                }
            }
    */


        using (SqlConnection myConsulta = new SqlConnection(connectionString))
        {
            await myConsulta.OpenAsync(); // Método assíncrono para abrir a conexão

            SqlDataAdapter SDAConsulta = new SqlDataAdapter("dbo.stp_ocorrencias_ins", myConsulta);
            SDAConsulta.SelectCommand.CommandType = CommandType.StoredProcedure;
            SDAConsulta.SelectCommand.Parameters.Add("@flg_operacao", SqlDbType.Char).Value = "T";
            SDAConsulta.SelectCommand.Parameters.Add("@COD_ID_PESSOA_LICENCIADO", SqlDbType.Int).Value = 1;
            // Criar o DataSet
            DataSet dsConsulta = new DataSet();
            // Preencher o DataSet de forma assíncrona
            await Task.Run(() => SDAConsulta.Fill(dsConsulta, "TRANSACOES")); // Rodando em uma task separada
            // Preencher o Repeater
            rptConsulta.DataSource = dsConsulta.Tables["TRANSACOES"].DefaultView;
            rptConsulta.DataBind();

        }
    }
}