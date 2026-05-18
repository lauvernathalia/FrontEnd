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

public partial class con_pagamentos_cobranca_boleto : System.Web.UI.Page
{

    public static DataTable dtParceiros;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (HttpContext.Current.Session.Count <= 0)
        {
            Response.Redirect("login.aspx");
            FormsAuthentication.SignOut();
        }

        //Funcoes.CONFIRMA(btnConfirmar, "Confirma o pagamento do PIX informado?");

        if (!IsPostBack)
        {
            dtParceiros = new DataTable();
            dtParceiros.Columns.Add("beneficiario", typeof(string));
            dtParceiros.Columns.Add("walletid", typeof(string));
            dtParceiros.Columns.Add("valor", typeof(string));
            dtParceiros.Columns.Add("percentual", typeof(string));



            //txtDescricao.Text = DateTimeOffset.Now.Offset.ToString().Split(':').FirstOrDefault().ToString();

            Funcoes.GravaAuditoria(0, Request.ServerVariables["REMOTE_ADDR"].ToString(), "Conta Pagamento - Cobranças", "Cobrança via Boleto Avulso");

        }
    }
    protected void ddlClientePadrao_SelectedIndexChanged(object sender, EventArgs e)
    {

    }
    protected void ddlParceiros_SelectedIndexChanged(object sender, EventArgs e)
    {

    }
    protected void btNovoParceiro_Click(object sender, EventArgs e)
    {

    }
    protected void btnIncluirSplit_Click(object sender, EventArgs e)
    {

    }
    protected void btnSalvar_Click(object sender, EventArgs e)
    {

    }
    protected void btnEnviar_Click(object sender, EventArgs e)
    {

    }
    protected void btnCancelar_Click(object sender, EventArgs e)
    {

    }
    protected void lsvParceiros_ItemCommand(object sender, ListViewCommandEventArgs e)
    {

    }

    public string GetPostBackScript()
    {
        PostBackOptions options = new PostBackOptions(btnPostback);
        Page.ClientScript.RegisterForEventValidation(options);

        return Page.ClientScript.GetPostBackEventReference(options);
    }

    protected void btnPostBack_Click(object sender, EventArgs e)
    {

    }

    protected void ckbSplit_CheckedChanged(object sender, System.EventArgs e)
    {
        if (ckbSplit.Checked == true)
        {
            divSplit.Visible = true;
            divSplit.Focus();
        }
        else
        {
            divSplit.Visible = false;
        }

    }
}