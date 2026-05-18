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

public partial class KeepSessionAlive : System.Web.UI.Page
{
    protected string WindowStatusText = "";
    protected void Page_Load(object sender, EventArgs e)
    {
        if (User.Identity.IsAuthenticated)
        {
            lblSessao.Text = DateTime.Now.ToShortTimeString();
            // Refresh this page 60 seconds before session timeout, effectively resetting the session timeout counter.
            MetaRefresh.Attributes["content"] = "60;url=KeepSessionAlive.aspx?q=" + DateTime.Now.Ticks;
            WindowStatusText = "Last refresh " + DateTime.Now.ToShortDateString() + " " + DateTime.Now.ToShortTimeString();
        }
    }
}
