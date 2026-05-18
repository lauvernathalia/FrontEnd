<%@ Application Language="C#" %>

<script runat="server">
    

    void Application_Start(object sender, EventArgs e) 
    {
        // Code that runs on application startup

    }
    
    void Application_End(object sender, EventArgs e) 
    {
        //  Code that runs on application shutdown

    }

    void Application_Error(object sender, EventArgs e)
    {
        
        Exception exc = Server.GetLastError();
        //string userIp = HttpContext.Current.Request.UserHostAddress;

        //string allowedIp = "177.221.214.126";

        //if (userIp == allowedIp)
        //{
            // Exibir detalhes do erro
        //    Response.Clear();
        //    Response.Write("<h1>Erro</h1>");
        //    Response.Write("<pre>" + exc.ToString() + "</pre>");
        //    Response.End();
        //}
        //else
        //{
            /*
            if (exc is SystemException)
            {
                Server.Transfer("ErrorPage.aspx", true);
            }
            if (exc is ApplicationException)
            {
                Server.Transfer("ErrorPage.aspx", true);
            }
            if (exc is DivideByZeroException)
            {
                Server.Transfer("ErrorPage.aspx", true);
            }
            if (exc is HttpUnhandledException)
            {
                Server.Transfer("ErrorPage.aspx", true);
            }
            */
        //}
        
        
    }    
            
    void Session_Start(object sender, EventArgs e) 
    {
        // Code that runs when a new session is started

    }

    void Session_End(object sender, EventArgs e) 
    {
        // Code that runs when a session ends. 
        // Note: The Session_End event is raised only when the sessionstate mode
        // is set to InProc in the Web.config file. If session mode is set to StateServer 
        // or SQLServer, the event is not raised.

    }

    void Application_BeginRequest(object sender, EventArgs e)
    {
        HttpContext.Current.Items["__inicio"] = DateTime.Now;
    }

    void Application_EndRequest(object sender, EventArgs e)
    {
        try
        {
            var inicio = HttpContext.Current.Items["__inicio"];
            if (inicio == null) return;

            var tempo = DateTime.Now - (DateTime)inicio;

            if (tempo.TotalSeconds >= 8)
            {
                var url = HttpContext.Current.Request.RawUrl;
                var metodo = HttpContext.Current.Request.HttpMethod;

                System.IO.File.AppendAllText(
                    @"C:\Logs\web_lentidao.log",
                    string.Format(
                        "{0:yyyy-MM-dd HH:mm:ss} | {1:N1}s | {2} | {3}\r\n",
                        DateTime.Now,
                        tempo.TotalSeconds,
                        metodo,
                        url
                    )
                );
            }
        }
        catch { }
    }

    protected void Application_AcquireRequestState(object sender, EventArgs e)
    {
        HttpContext.Current.Items["__req_start"] = DateTime.Now;
    }

    protected void Application_ReleaseRequestState(object sender, EventArgs e)
    {
        object startObj = HttpContext.Current.Items["__req_start"];
        if (startObj == null)
            return;

        DateTime start = (DateTime)startObj;
        TimeSpan elapsed = DateTime.Now - start;

        if (elapsed.TotalSeconds > 5)
        {
            string path = HttpContext.Current.Server.MapPath("~/App_Data/session_lock.log");

            System.IO.File.AppendAllText(
                path,
                string.Format(
                    "{0:yyyy-MM-dd HH:mm:ss} | {1:N1}s | {2}\r\n",
                    DateTime.Now,
                    elapsed.TotalSeconds,
                    HttpContext.Current.Request.RawUrl
                )
            );
        }
    }

    
</script>
