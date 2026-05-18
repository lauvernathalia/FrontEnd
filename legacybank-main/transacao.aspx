<%@ Page Language="C#" AutoEventWireup="true" CodeFile="transacao.aspx.cs" Inherits="transacao" Async="true" EnableSessionState="ReadOnly" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Transação Assíncrona</title>
</head>
<body>
    <form id="form1" runat="server">
        <asp:Button ID="btnInserir" runat="server" Text="Inserir Transação" OnClick="btnInserir_Click" />
        <asp:Label ID="lblResultado" runat="server" Text=""></asp:Label>
        <asp:Repeater runat="server" ID="rptConsulta" Visible="true">
            <ItemTemplate>
                <%# DataBinder.Eval(Container.DataItem, "NOM_CODE")%>
            </ItemTemplate>
        </asp:Repeater>
    </form>
</body>
</html>
