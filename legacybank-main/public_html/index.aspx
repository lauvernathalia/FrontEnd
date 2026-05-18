<%@ Page Language="C#" AutoEventWireup="true" CodeFile="index.aspx.cs" Inherits="public_html_index" %>

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
        
        .text-whitelabel1 {
          color: <%=CorPrimaria() %> !important;
        }

       
    </style>


</head>
<body runat="server" id="bdyLogin" class="hold-transition login-page vh-100 h-100" style="height: 100vh;">
<form runat="server" id="frmLogin">

<div class="row">
    <div class="col-sm-12">
        <div class="row vh-100 h-100 d-flex">
            <div class="col-12 align-items-center align-self-center align-content-center p-5">
                <center>
                <asp:Image alt="" runat="server" id="imgDestaque" CssClass="img-fluid"></asp:Image>
                </center>
            </div>
        </div>
        <div class="row m-5" style="display:none;">
            <div class="fixed-bottom mb-2 text-white hw-100 v-100 row col-6">
                <div class="col-10 fluid-left mb-4">2024 Legacy Bank. Todos os Direitos Reservados.</div>
                <div class="col-2 fluid-right mb-4">Versão 2.0</div>
            </div>
        </div>
    </div>

</div>
</form>

<script type="text/javascript" src="../../plugins/jquery/jquery.min.js"></script>
<script type="text/javascript" src="../../plugins/bootstrap/js/bootstrap.bundle.min.js"></script>
<script type="text/javascript" src="../../dist/js/adminlte.min.js"></script>
<script type="text/javascript" src="../../plugins/select2/js/select2.full.min.js"></script>
<script type="text/javascript" src="../../plugins/bootstrap4-duallistbox/jquery.bootstrap-duallistbox.min.js"></script>
<script type="text/javascript" src="../../plugins/moment/moment.min.js"></script>
<script type="text/javascript" src="../../plugins/inputmask/min/jquery.inputmask.bundle.min.js"></script>
<script type="text/javascript" src="../../plugins/daterangepicker/daterangepicker.js"></script>
<script type="text/javascript" src="../../plugins/bootstrap-colorpicker/js/bootstrap-colorpicker.min.js"></script>
<script type="text/javascript" src="../../plugins/tempusdominus-bootstrap-4/js/tempusdominus-bootstrap-4.min.js"></script>
<script type="text/javascript" src="../../plugins/bootstrap-switch/js/bootstrap-switch.min.js"></script>
<script type="text/javascript" src="../../dist/js/demo.js"></script>

</body>
</html>

