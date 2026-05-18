<%@ Page Language="C#" AutoEventWireup="true" CodeFile="btnteste.aspx.cs" Inherits="btnteste" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
<form id="form1" runat="server">
    <div>
        <table class="auto-style1">
            <tr>
                <td>
                    <asp:Label ID="Label2" runat="server" Text="EXEMPLO 01" style="font-weight: 700; font-size: large"></asp:Label>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Button runat="server" ID="btn01" Text = "Executar01" Width="150px" Height="50px" onclick="btn01_Click" UseSubmitBehavior="false"  /> 
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="Label1" runat="server" Text=""></asp:Label>
                </td>
            </tr>
        </table>
        <br />
    </div>
</form>
</body>
</html>
