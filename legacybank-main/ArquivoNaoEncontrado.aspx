<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ArquivoNaoEncontrado.aspx.cs" Inherits="ArquivoNaoEncontrado" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <meta charset="utf-8"/>
    <meta http-equiv="X-UA-Compatible" content="IE=edge"/>
    <title>LegacyBank</title>

    <meta name="viewport" content="width=device-width, initial-scale=1"/>
    <link rel="stylesheet" href="../plugins/fontawesome-free/css/all.min.css"/>
    <link rel="stylesheet" href="https://code.ionicframework.com/ionicons/2.0.1/css/ionicons.min.css"/>
    <link rel="stylesheet" href="../plugins/icheck-bootstrap/icheck-bootstrap.min.css"/>
    <link rel="stylesheet" href="../dist/css/adminlte.css"/>
    <link href="https://fonts.googleapis.com/css?family=Source+Sans+Pro:300,400,400i,700" rel="stylesheet"/>

    <asp:Literal ID="FavIconLink" runat="server"></asp:Literal>

    <script type="text/javascript" src="../funcoes.js"></script>
</head>
<body runat="server" id="bdyLogin" class="hold-transition login-page vh-100 h-100" style="height: 100vh;">
    <form runat="server" id="frmLogin">

        <div class="row d-flex">
            <div class="col-12 align-items-center align-self-center align-content-center p-5">
                <center>
                <asp:Image alt="" runat="server" id="imgDestaque" CssClass="img-fluid col-4"></asp:Image>
                </center>
            </div>
        </div>
        <div class="row">
            <div class="col-12">
                <div class="alert alert-warning alert-dismissible p-5 m-5">
                  <button type="button" class="close" data-dismiss="alert" aria-hidden="true">&times;</button>
                  <h3><i class="icon fas fa-exclamation-triangle"></i> ATENÇÃO!</h3>
                  <h4>Página ou Arquivo não encontrado! Esta página ou arquivo pode ter sido movido para outro local ou removido.</h4>
                  <asp:Button runat="server" ID="btnFechar" CssClass="btn btn-secondary text-white" Text="Fechar" onclick="btnFechar_Click" />
                </div>
            </div>
        </div>

    </form>
</body>
</html>
