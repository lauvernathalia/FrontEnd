<%@ Page Language="C#" AutoEventWireup="true" CodeFile="cad_locacao_autorizacao_debito.aspx.cs" Inherits="cad_locacao_autorizacao_debito" Async="true" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <h2>Envio de PDF para Assinatura (Autentique)</h2>
            <asp:FileUpload ID="fileUpload" runat="server" />
            <asp:Button ID="btnEnviar" runat="server" Text="Enviar PDF" OnClick="btnEnviar_Click" />
            <br /><br />
            <asp:Label ID="lblMensagem" runat="server" ForeColor="Red"></asp:Label>
        </div>

        <div>
            <h2>Gerador de Contrato</h2>
        
            <label for="txtNome">Nome:</label>
            <asp:TextBox ID="txtNome" runat="server"></asp:TextBox><br /><br />

            <label for="txtCidade">Cidade:</label>
            <asp:TextBox ID="txtCidade" runat="server"></asp:TextBox><br /><br />

            <label for="txtCNPJ">CNPJ:</label>
            <asp:TextBox ID="txtCNPJ" runat="server"></asp:TextBox><br /><br />

            <asp:Button ID="btnGerarPDF" runat="server" Text="Gerar PDF" OnClick="btnGerarPDF_Click" />        
        </div>

    </form>
</body>
</html>
