<%@ Page Language="C#" AutoEventWireup="true" CodeFile="processarcapta.aspx.cs" Inherits="events_processarcapta" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        Resposta: <asp:TextBox runat="server" ID="txtResposta" TextMode="MultiLine" Rows="5" Width="100%"></asp:TextBox><br />
        Inicio: <asp:TextBox runat="server" ID="txtIni"></asp:TextBox><br />
        Término: <asp:TextBox runat="server" ID="txtFim"></asp:TextBox><br />
        <asp:Button runat="server" ID="btnProcessar" Text = "Processar" 
            onclick="btnProcessar_Click" />
    </div>
    </form>
</body>
</html>
