<%@ Page Language="C#" AutoEventWireup="true" CodeFile="novasenha.aspx.cs" Inherits="novasenha" %>

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

    <script type="text/javascript" src="funcoes.js"></script>
    <script src="https://www.google.com/recaptcha/api.js?hl=pt"></script>

    <script type="text/javascript">
        var imNotARobot = function () {
            $("#recaptcha").val(grecaptcha.getResponse());
        };
    </script>

    <style type="text/css">
        .btn-whitelabel1 {
          background-color: <%=CorPrimaria() %>;
          border-color: <%=CorPrimaria() %>;
        }   
        .btn-whitelabel1:hover {
          background-color: <%=CorPrimaria() %>;
          border-color: <%=CorPrimaria() %>;
          opacity:0.9;
        }        
        .btn-whitelabel1.disabled, .btn-whitelabel1:disabled {
          background-color: <%=CorPrimaria() %>;
          border-color: <%=CorPrimaria() %>;
        }

        .btn-whitelabel1:not(:disabled):not(.disabled):active, .btn-whitelabel1:not(:disabled):not(.disabled).active,
        .show > .btn-whitelabel1.dropdown-toggle {
          background-color: <%=CorPrimaria() %>;
          border-color: <%=CorPrimaria() %>;
          opacity:0.9;
        }
        
    </style>


</head>
<body runat="server" id="bdyLogin" class="hold-transition login-page">

<form runat="server" id="frmLogin">
<div class="row">
    <div class="col-12">
        <center>
        <h1 class="text-white text-center"><strong><asp:Label runat="server" id="lblSlogan"></asp:Label></strong></h1>
        </center>
    </div>
</div>

<div class="login-box rounded-lg">
    <div class="card rounded-lg">
        <div class="card-body login-card-body rounded-lg">
            <center>
                <a class="brand-logo mx-0 px-0">
                    <asp:Image height="50" alt="" runat="server" id="imgLogoPrincipal" CssClass="img-fluid"></asp:Image>
                </a>
                <h2>Seja Bem-vindo! 👋</h2>
                <p class="card-text">Por favor entre com a sua nova senha de acesso</p>
                <div class="form-group">
                    <label for="exampleInputSenha">E-mail: </label>
                    <asp:Label runat="server" ID="lblEmail"></asp:Label>
                </div>

            </center>

            <div class="form-group">
                <label for="exampleInputSenha">Nova Senha</label>
                <asp:TextBox runat="server" ID="txtNovaSenha" CssClass="form-control" placeholder="Informe sua nova senha" TextMode="Password"></asp:TextBox>
            </div>
            <div class="form-group">
                <label for="exampleInputSenha">Repetir Senha</label>
                <asp:TextBox runat="server" ID="txtRepetirSenha" CssClass="form-control" placeholder="Repita a sua nova senha" TextMode="Password"></asp:TextBox>
            </div>

            <div class="row">
                <asp:Button runat="server" ID="btnSalvar" CssClass="btn btn-whitelabel1 btn-block" 
                    Text="Salvar a nova senha" onclick="btnSalvar_Click"/>

            </div>
        </div>
    </div>
</div>
</form>

<script type="text/javascript" src="../plugins/jquery/jquery.min.js"></script>
<script type="text/javascript" src="../plugins/bootstrap/js/bootstrap.bundle.min.js"></script>
<script type="text/javascript" src="../dist/js/adminlte.min.js"></script>
<script type="text/javascript" src="../plugins/select2/js/select2.full.min.js"></script>
<script type="text/javascript" src="../plugins/bootstrap4-duallistbox/jquery.bootstrap-duallistbox.min.js"></script>
<script type="text/javascript" src="../plugins/moment/moment.min.js"></script>
<script type="text/javascript" src="../plugins/inputmask/min/jquery.inputmask.bundle.min.js"></script>
<script type="text/javascript" src="../plugins/daterangepicker/daterangepicker.js"></script>
<script type="text/javascript" src="../plugins/bootstrap-colorpicker/js/bootstrap-colorpicker.min.js"></script>
<script type="text/javascript" src="../plugins/tempusdominus-bootstrap-4/js/tempusdominus-bootstrap-4.min.js"></script>
<script type="text/javascript" src="../plugins/bootstrap-switch/js/bootstrap-switch.min.js"></script>
<script type="text/javascript" src="../dist/js/demo.js"></script>

</body>
</html>
