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


using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

using System.IO;
using System.Text;

using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;

using System.ComponentModel;

public partial class topopadrao : System.Web.UI.UserControl
{
    protected void Page_Load(object sender, EventArgs e)
    {

        // Verifica se o usuário esta autenticado
        if (HttpContext.Current.Session.Count <= 0)
        {
            Response.Redirect("login.aspx");
            FormsAuthentication.SignOut();
        }


        //Verifica se o site url de origem é igual ao de destino....

        string sURLAtual = Request.ServerVariables["SERVER_NAME"].ToString();
        string sURLLogin = HttpContext.Current.Session["URLORIGEM"].ToString();

        if (sURLAtual != sURLLogin)
        {
            Funcoes.GravaAuditoria(0, Request.ServerVariables["REMOTE_ADDR"].ToString(), "Login Indevido", "Login Indevido - Tentativa acesso de usuário não autorizado");
            Response.Redirect(sURLLogin.ToString());

        }

        if ((Request.ServerVariables["REMOTE_ADDR"].ToString() == "155.2.219.31") || (Request.ServerVariables["REMOTE_ADDR"].ToString() == "169.150.198.82") || (HttpContext.Current.Session["CODIGO"].ToString() == "11009") || (HttpContext.Current.Session["PESSOA"].ToString() == "9687"))
        {
            Response.Redirect("login.aspx");
            FormsAuthentication.SignOut();
        }

    }
}