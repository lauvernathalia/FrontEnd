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



/// <summary>
/// Summary description for FuncoesAssincronas
/// </summary>
public class FuncoesAssincronas
{
    public static async Task<string> ProcessarLentoAsync()
    {
        await Task.Delay(15000); // Simula 15 segundos de espera
        return "Processamento Lento Concluído!";
    }

    public static async Task<string> ProcessarRapidoAsync()
    {
        await Task.Delay(1000); // Simula 1 segundo de espera
        return "Processamento Rápido Concluído!";
    }

}


