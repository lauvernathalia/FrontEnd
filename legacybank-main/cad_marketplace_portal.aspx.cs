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
using System.Data.SqlClient;

using System.Drawing;
using System.Drawing.Printing;
using System.Diagnostics;


using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

using System.IO;
using System.Text;

using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;

using System.ComponentModel;

using System.Security.Cryptography;
using System.Security;
using System.Net.Mail;
using System.IO.IsolatedStorage;



public partial class cad_marketplace_portal : System.Web.UI.Page
{
    public string sid_id
    {
        get
        {
            try { return Request["id"]; }
            catch { return ""; }
        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        // Verifica se o usuário esta autenticado

        // Verifica se o usuário esta autenticado
        if (HttpContext.Current.Session.Count <= 0)
        {
            Response.Redirect("login.aspx");
            FormsAuthentication.SignOut();
        }

        if (!IsPostBack)
        {
            ConsultaFicha();
        }

    }

    private void ConsultaFicha()
    {
        SqlConnection mySelCadastro = new SqlConnection(Funcoes.conexao());
        mySelCadastro.Open();
        SqlCommand cmdSelCadastro = new SqlCommand("dbo.stp_pessoas_fj_licenciados_ins", mySelCadastro);
        cmdSelCadastro.CommandType = CommandType.StoredProcedure;
        cmdSelCadastro.Parameters.Add("@flg_operacao", SqlDbType.Char).Value = "S";
        cmdSelCadastro.Parameters.Add("@COD_ID", SqlDbType.Int).Value = Funcoes.strToInt(sid_id.ToString());
        //cmdSelCadastro.Parameters.Add("@COD_ID_PESSOA_LICENCIADO", SqlDbType.Int).Value = Funcoes.strToInt(HttpContext.Current.Session["LICENCIADO"].ToString());
        SqlDataReader ReaderCadastro = cmdSelCadastro.ExecuteReader();
        while (ReaderCadastro.Read())
        {
            txtID.Text = ReaderCadastro["COD_ID"].ToString();
            txtCodigo.Text = ReaderCadastro["COD_ID_PESSOA_LICENCIADO"].ToString();
            txtRazaosocial.Text = ReaderCadastro["NOM_RAZAOSOCIAL"].ToString();

            txtDominio.Text = ReaderCadastro["NOM_URL"].ToString();
            txtTitulo.Text = ReaderCadastro["NOM_TITULO_URL"].ToString();

            txtLogotipoLogin.Text = ReaderCadastro["NOM_LOGO"].ToString();
            txtLogotipoMenu.Text = ReaderCadastro["NOM_LOGO_MENU"].ToString();

            txtCorPrimaria.Text = ReaderCadastro["NOM_COR_PRIMARIA_FUNDO"].ToString();
            txtCorSecundaria.Text = ReaderCadastro["NOM_COR_SECUNDARIA_FUNDO"].ToString();

            txtImagem.Text = ReaderCadastro["NOM_IMAGEM_FUNDO"].ToString();
            txtFavicon.Text = ReaderCadastro["NOM_FAVICON"].ToString();
            txtCheckout.Text = ReaderCadastro["NOM_LOGOTIPO_CHECKOUT"].ToString();
            ddlCriarConta.SelectedValue = ReaderCadastro["FLG_CRIAR_CONTA"].ToString();
            ddl2FA.SelectedValue = ReaderCadastro["FLG_2FA"].ToString();
        }

    }

    protected void btnCancelar_Click(object sender, EventArgs e)
    {
        ClientScript.RegisterStartupScript(this.GetType(),
            "Alerta", "opener.PostBackOnMainPage(); window.close(); ", true);
    }

    protected void btnSalvar_Click(object sender, EventArgs e)
    {
        // Arquivos de imagem
        // Logotipo Login

        string StrFileNameflLogotipoLogin = flLogotipoLogin.PostedFile.FileName.Substring(flLogotipoLogin.PostedFile.FileName.LastIndexOf("\\") + 1);
        string StrFileTypeflLogotipoLogin = flLogotipoLogin.PostedFile.ContentType;
        int IntFileSizeflLogotipoLogin = flLogotipoLogin.PostedFile.ContentLength;
        string NomeArquivoflLogotipoLogin = "";
        if (StrFileNameflLogotipoLogin.Trim() != "")
        {
            string CodificacaoflLogotipoLogin = DateTime.Now.Year.ToString() + DateTime.Now.Month.ToString() + DateTime.Now.Day.ToString() + DateTime.Now.Hour.ToString() + DateTime.Now.Minute.ToString() + DateTime.Now.Second.ToString();
            flLogotipoLogin.PostedFile.SaveAs(Server.MapPath("public_html") + "\\" + HttpContext.Current.Session["CODIGO"].ToString().Trim() + "_" + CodificacaoflLogotipoLogin.ToString() + "_" + StrFileNameflLogotipoLogin);
            NomeArquivoflLogotipoLogin = HttpContext.Current.Session["CODIGO"].ToString().Trim() + "_" + CodificacaoflLogotipoLogin.ToString() + "_" + StrFileNameflLogotipoLogin;
        }

        // Logotipo Menu

        string StrFileNameflLogotipoMenu = flLogotipoMenu.PostedFile.FileName.Substring(flLogotipoMenu.PostedFile.FileName.LastIndexOf("\\") + 1);
        string StrFileTypeflLogotipoMenu = flLogotipoMenu.PostedFile.ContentType;
        int IntFileSizeflLogotipoMenu = flLogotipoMenu.PostedFile.ContentLength;
        string NomeArquivoflLogotipoMenu = "";
        if (StrFileNameflLogotipoMenu.Trim() != "")
        {
            string CodificacaoflLogotipoMenu = DateTime.Now.Year.ToString() + DateTime.Now.Month.ToString() + DateTime.Now.Day.ToString() + DateTime.Now.Hour.ToString() + DateTime.Now.Minute.ToString() + DateTime.Now.Second.ToString();
            flLogotipoMenu.PostedFile.SaveAs(Server.MapPath("public_html") + "\\" + HttpContext.Current.Session["CODIGO"].ToString().Trim() + "_" + CodificacaoflLogotipoMenu.ToString() + "_" + StrFileNameflLogotipoMenu);
            NomeArquivoflLogotipoMenu = HttpContext.Current.Session["CODIGO"].ToString().Trim() + "_" + CodificacaoflLogotipoMenu.ToString() + "_" + StrFileNameflLogotipoMenu;
        }

        // Imagem Fundo
        string StrFileNameflImagem = flImagem.PostedFile.FileName.Substring(flImagem.PostedFile.FileName.LastIndexOf("\\") + 1);
        string StrFileTypeflImagem = flImagem.PostedFile.ContentType;
        int IntFileSizeflImagem = flImagem.PostedFile.ContentLength;
        string NomeArquivoflImagem = "";
        if (StrFileNameflImagem.Trim() != "")
        {
            string CodificacaoflImagem = DateTime.Now.Year.ToString() + DateTime.Now.Month.ToString() + DateTime.Now.Day.ToString() + DateTime.Now.Hour.ToString() + DateTime.Now.Minute.ToString() + DateTime.Now.Second.ToString();
            flImagem.PostedFile.SaveAs(Server.MapPath("public_html") + "\\" + HttpContext.Current.Session["CODIGO"].ToString().Trim() + "_" + CodificacaoflImagem.ToString() + "_" + StrFileNameflImagem);
            NomeArquivoflImagem = HttpContext.Current.Session["CODIGO"].ToString().Trim() + "_" + CodificacaoflImagem.ToString() + "_" + StrFileNameflImagem;
        }


        // Imagem Fundo
        string StrFileNameflFavicon = flFavicon.PostedFile.FileName.Substring(flFavicon.PostedFile.FileName.LastIndexOf("\\") + 1);
        string StrFileTypeflFavicon = flFavicon.PostedFile.ContentType;
        int IntFileSizeflFavicon = flFavicon.PostedFile.ContentLength;
        string NomeArquivoflFavicon = "";
        if (StrFileNameflFavicon.Trim() != "")
        {
            string CodificacaoflFavicon = DateTime.Now.Year.ToString() + DateTime.Now.Month.ToString() + DateTime.Now.Day.ToString() + DateTime.Now.Hour.ToString() + DateTime.Now.Minute.ToString() + DateTime.Now.Second.ToString();
            flFavicon.PostedFile.SaveAs(Server.MapPath("public_html") + "\\" + HttpContext.Current.Session["CODIGO"].ToString().Trim() + "_" + CodificacaoflFavicon.ToString() + "_" + StrFileNameflFavicon);
            NomeArquivoflFavicon = HttpContext.Current.Session["CODIGO"].ToString().Trim() + "_" + CodificacaoflFavicon.ToString() + "_" + StrFileNameflFavicon;
        }


        // Imagem Checkout
        string StrFileNameflCheckout = flCheckout.PostedFile.FileName.Substring(flCheckout.PostedFile.FileName.LastIndexOf("\\") + 1);
        string StrFileTypeflCheckout = flCheckout.PostedFile.ContentType;
        int IntFileSizeflCheckout = flCheckout.PostedFile.ContentLength;
        string NomeArquivoflCheckout = "";
        if (StrFileNameflCheckout.Trim() != "")
        {
            string CodificacaoflCheckout = DateTime.Now.Year.ToString() + DateTime.Now.Month.ToString() + DateTime.Now.Day.ToString() + DateTime.Now.Hour.ToString() + DateTime.Now.Minute.ToString() + DateTime.Now.Second.ToString();
            flCheckout.PostedFile.SaveAs(Server.MapPath("public_html") + "\\" + HttpContext.Current.Session["CODIGO"].ToString().Trim() + "_" + CodificacaoflCheckout.ToString() + "_" + StrFileNameflCheckout);
            NomeArquivoflCheckout = HttpContext.Current.Session["CODIGO"].ToString().Trim() + "_" + CodificacaoflCheckout.ToString() + "_" + StrFileNameflCheckout;
        }
        
        SqlConnection connInsCons = new SqlConnection(Funcoes.conexao());
        connInsCons.Open();
        SqlCommand cmdInsCons = new SqlCommand("dbo.stp_pessoas_fj_licenciados_ins", connInsCons);
        cmdInsCons.CommandType = CommandType.StoredProcedure;
        cmdInsCons.Parameters.Add("@flg_operacao", SqlDbType.Char).Value = "P";
        cmdInsCons.Parameters.Add("@COD_ID", SqlDbType.Int).Value = Funcoes.strToInt(sid_id.ToString());
        cmdInsCons.Parameters.Add("@NOM_URL", SqlDbType.VarChar).Value = txtDominio.Text.ToString();
        cmdInsCons.Parameters.Add("@NOM_TITULO_URL", SqlDbType.VarChar).Value = txtTitulo.Text.ToString();
        cmdInsCons.Parameters.Add("@NOM_COR_PRIMARIA_FUNDO", SqlDbType.VarChar).Value = txtCorPrimaria.Text.ToString();
        cmdInsCons.Parameters.Add("@NOM_COR_SECUNDARIA_FUNDO", SqlDbType.VarChar).Value = txtCorSecundaria.Text.ToString();
        cmdInsCons.Parameters.Add("@FLG_2FA", SqlDbType.Char).Value = ddl2FA.SelectedValue.ToString();
        cmdInsCons.Parameters.Add("@FLG_CRIAR_CONTA", SqlDbType.Char).Value = ddlCriarConta.SelectedValue.ToString();

        if (NomeArquivoflLogotipoLogin.ToString().Trim() != "")
        {
            cmdInsCons.Parameters.Add("@NOM_LOGO", SqlDbType.VarChar).Value = NomeArquivoflLogotipoLogin.ToString();
            txtLogotipoLogin.Text = NomeArquivoflLogotipoLogin.ToString();

        }
        else
        {
            cmdInsCons.Parameters.Add("@NOM_LOGO", SqlDbType.VarChar).Value = txtLogotipoLogin.Text.ToString();

        }

        if (NomeArquivoflLogotipoMenu.ToString().Trim() != "")
        {
            cmdInsCons.Parameters.Add("@NOM_LOGO_MENU", SqlDbType.VarChar).Value = NomeArquivoflLogotipoMenu.ToString();
            txtLogotipoMenu.Text = NomeArquivoflLogotipoMenu.ToString();
        }
        else
        {
            cmdInsCons.Parameters.Add("@NOM_LOGO_MENU", SqlDbType.VarChar).Value = txtLogotipoMenu.Text.ToString();

        }

        if (NomeArquivoflImagem.ToString().Trim() != "")
        {
            cmdInsCons.Parameters.Add("@NOM_IMAGEM_FUNDO", SqlDbType.VarChar).Value = NomeArquivoflImagem.ToString();
            txtImagem.Text = NomeArquivoflImagem.ToString();
        }
        else
        {
            cmdInsCons.Parameters.Add("@NOM_IMAGEM_FUNDO", SqlDbType.VarChar).Value = txtImagem.Text.ToString();

        }

        if (NomeArquivoflFavicon.ToString().Trim() != "")
        {
            cmdInsCons.Parameters.Add("@NOM_FAVICON", SqlDbType.VarChar).Value = NomeArquivoflFavicon.ToString();
            txtFavicon.Text = NomeArquivoflFavicon.ToString();
        }
        else
        {
            cmdInsCons.Parameters.Add("@NOM_FAVICON", SqlDbType.VarChar).Value = txtFavicon.Text.ToString();

        }

        if (NomeArquivoflCheckout.ToString().Trim() != "")
        {
            cmdInsCons.Parameters.Add("@NOM_LOGOTIPO_CHECKOUT", SqlDbType.VarChar).Value = NomeArquivoflCheckout.ToString();
            txtCheckout.Text = NomeArquivoflCheckout.ToString();
        }
        else
        {
            cmdInsCons.Parameters.Add("@NOM_LOGOTIPO_CHECKOUT", SqlDbType.VarChar).Value = txtCheckout.Text.ToString();

        }

        cmdInsCons.ExecuteNonQuery();
        connInsCons.Close();
        connInsCons.Dispose();

        ClientScript.RegisterStartupScript(this.GetType(),
            "Alerta", "alert('Dados gravados com sucesso'); ", true);


    }
    protected void btnGerar_Click(object sender, EventArgs e)
    {
        string CodificacaoGERACAO = DateTime.Now.Year.ToString() + DateTime.Now.Month.ToString() + DateTime.Now.Day.ToString() + DateTime.Now.Hour.ToString() + DateTime.Now.Minute.ToString() + DateTime.Now.Second.ToString();
        string fileNameGERACAO = Server.MapPath("public_html") + "\\" + "LEGACYGERACAODOMINIO_" + CodificacaoGERACAO.ToString() + ".bat";

        if (File.Exists(fileNameGERACAO))
        {
            File.Delete(fileNameGERACAO);
        }

        using (StreamWriter swGERACAO = File.CreateText(fileNameGERACAO))
        {
            swGERACAO.WriteLine("appcmd add site /name:" + txtDominio.Text.ToString() + " /bindings:http://" + txtDominio.Text.ToString() + ":80 /physicalPath:C:\\inetpub\\wwwroot\\conta.legacybank.com.br");
            swGERACAO.WriteLine("appcmd add app /site.name:" + txtDominio.Text.ToString() + " /path:/events /physicalPath:C:\\inetpub\\wwwroot\\conta.legacybank.com.br\\events");
            swGERACAO.WriteLine("wacs --source iis --host " + txtDominio.Text.ToString() + " --installation iis --sslport 443 --sslipaddress *");
            swGERACAO.Close();
        }



        //System.Diagnostics.Process.Start(Server.MapPath("public_html") + "\\" + "LEGACYGERACAODOMINIO_" + CodificacaoGERACAO.ToString() + ".bat");

        // TESTE DE EXECUÇÃO DIRETA APPCMD



        /*
        string caminhoAPPCMD = "appcmd.exe";
        string comandoAPPCMD = "add site /name:"+txtDominio.Text.ToString()+" /bindings:http://"+txtDominio.Text.ToString()+":80 /physicalPath:C:\\inetpub\\wwwroot\\conta.legacybank.com.br";
        
        System.Diagnostics.ProcessStartInfo processStartInfoAPPCMD = new System.Diagnostics.ProcessStartInfo(caminhoAPPCMD, comandoAPPCMD);
        System.Diagnostics.Process processAPPCMD = new System.Diagnostics.Process();
        processAPPCMD.StartInfo = processStartInfoAPPCMD;
        var sspw = new SecureString();

        foreach (var c in "lIz8N60&4x#H")
            sspw.AppendChar(c);

        processAPPCMD.StartInfo.UserName = "administrador";
        processAPPCMD.StartInfo.Password = sspw;
        processAPPCMD.StartInfo.UseShellExecute = false;

        // Executa o comando.. 
        bool processStartedAPPCMD = processAPPCMD.Start();
        processAPPCMD.WaitForExit();

        ClientScript.RegisterStartupScript(this.GetType(),
            "Resultado", "alert('" + processStartedAPPCMD.ToString()+ "');", true);
        */



        // FIM TESTE 
        /*
        System.Diagnostics.Process process2 = new System.Diagnostics.Process();
        System.Diagnostics.ProcessStartInfo startInfo = new System.Diagnostics.ProcessStartInfo();
        startInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
        startInfo.RedirectStandardOutput = true;
        startInfo.RedirectStandardInput = true;
        startInfo.UseShellExecute = false;
        startInfo.FileName = "notepad.exe";
        process2.StartInfo = startInfo;
        process2.Start();
        */

        Process proc = null;
        string batDir = string.Format(Server.MapPath("public_html") + "\\");
        proc = new Process();
        proc.StartInfo.WorkingDirectory = batDir;

        proc.StartInfo.FileName = "LEGACYGERACAODOMINIO_" + CodificacaoGERACAO.ToString() + ".bat";

        proc.StartInfo.CreateNoWindow = false;
        proc.Start();

        ClientScript.RegisterStartupScript(this.GetType(),
            "Finalizado", "alert('Arquivos do Domínio GERADOS com sucesso!');", true);

        /*
        Process proc = null;
        string batDir = string.Format(Server.MapPath("public_html") + "\\");

        proc = new Process();
        proc.StartInfo.WorkingDirectory = batDir;
        proc.StartInfo.FileName = "LEGACYGERACAODOMINIO_" + CodificacaoGERACAO.ToString() + ".bat";

        var sspw = new SecureString();

        foreach (var c in "lIz8N60&4x#H")
            sspw.AppendChar(c);


        proc.StartInfo.UserName = "administrador";
        proc.StartInfo.Password = sspw;
        proc.StartInfo.UseShellExecute = false;
        
        proc.Start();

        proc.WaitForExit();
        
        proc.Close();
        */
        /*
        Process process = new Process();
        process.StartInfo.FileName = "cmd.exe";
        process.StartInfo.UseShellExecute = false;
        process.StartInfo.CreateNoWindow = true;

        process.Start();
        */

        /*
        // Create a new Process object.
        Process process = new Process();
        // Set the StartInfo.FileName property to the path of the CMD executable.
        process.StartInfo.FileName = "cmd.exe";
        // Set the StartInfo.Arguments property to the CMD command that you want to execute.
        process.StartInfo.Arguments = "/c dir";
        // Start the process.
        process.Start();
        // Wait for the process to finish.
        process.WaitForExit();
        // Read the output of the process.
        string output = "";
        while (!process.StandardOutput.EndOfStream)
        {
            output = process.StandardOutput.ReadToEnd();
        }
        // Display the output of the process.
        ClientScript.RegisterStartupScript(this.GetType(),
            "Finalizado", "alert('" + output.ToString()+ "');", true);
        */

        /*
        ProcessStartInfo ProcessInfo;
        Process process;

        ProcessInfo = new ProcessStartInfo(Server.MapPath("public_html") + "\\LEGACYGERACAODOMINIO_" + CodificacaoGERACAO.ToString() + ".bat");
        ProcessInfo.CreateNoWindow = true;
        ProcessInfo.UseShellExecute = false;
        ProcessInfo.WorkingDirectory = Server.MapPath("public_html")+"\\";
        // *** Redirect the output ***
        ProcessInfo.RedirectStandardError = true;
        ProcessInfo.RedirectStandardOutput = true;

        process = Process.Start(ProcessInfo);
        process.WaitForExit();

        ClientScript.RegisterStartupScript(this.GetType(),
            "Finalizado", "alert('Arquivos do Domínio GERADOS com sucesso!');", true);
        
        process.Close();
         */

    }
}