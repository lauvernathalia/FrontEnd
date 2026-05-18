<%@ Page Language="C#" AutoEventWireup="true" CodeFile="boletobancario.aspx.cs" Inherits="boletobancario" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <h2>Gerador de Código de Barras</h2>
            <asp:Label ID="lblLinhaDigitavel" runat="server" Text="Linha Digitável:"></asp:Label>
            <asp:TextBox ID="txtLinhaDigitavel" runat="server" Width="400"></asp:TextBox>
            <asp:Button ID="btnGerar" runat="server" Text="Gerar Código de Barras" OnClick="btnGerar_Click" />
            <br /><br />
            <asp:Image ID="imgCodigoBarras" runat="server" Visible="false" />
        </div>    
    </form>
</body>
</html>
