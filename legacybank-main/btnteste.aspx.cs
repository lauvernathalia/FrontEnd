using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class btnteste : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        btn01.Attributes.Add("onclick", "document.body.style.cursor = 'wait'; this.value='Aguarde...'; this.disabled = true; " + ClientScript.GetPostBackEventReference(btn01, string.Empty) + ";");
    }
    protected void btn01_Click(object sender, EventArgs e)
    {
        string str = "";
        for (int i = 0; i < 50000; i++)
        {
            str = str + "0";
        }
        Label1.Text += "\r\n---->RESULTADO EXEMPLOS<-----" + str;
    }
}