<%@ Control Language="C#" AutoEventWireup="true" CodeFile="rotinaspadroes.ascx.cs" Inherits="rotinaspadroes" %>

<asp:Literal ID="FavIconLink" runat="server"></asp:Literal>

<link rel="stylesheet" href="../plugins/fontawesome-free/css/all.min.css"/>
<link rel="stylesheet" href="../plugins/overlayScrollbars/css/OverlayScrollbars.min.css"/>
<link rel="stylesheet" href="../dist/css/adminlte.css"/>
<link rel="stylesheet" href="../dist/css/adminlegacy.css"/>





<link rel="stylesheet" href="../dist/css/2of5_all.css"/>
<link href="https://fonts.googleapis.com/css?family=Source+Sans+Pro:300,400,400i,700" rel="stylesheet"/>

<link rel="stylesheet" href="https://code.ionicframework.com/ionicons/2.0.1/css/ionicons.min.css"/>
<link rel="stylesheet" href="../plugins/daterangepicker/daterangepicker.css"/>
<link rel="stylesheet" href="../plugins/icheck-bootstrap/icheck-bootstrap.min.css"/>

<link rel="stylesheet" href="../plugins/bootstrap-colorpicker/css/bootstrap-colorpicker.min.css"/>
<link rel="stylesheet" href="../plugins/tempusdominus-bootstrap-4/css/tempusdominus-bootstrap-4.min.css"/>
<link rel="stylesheet" href="../plugins/select2/css/select2.min.css"/>
<link rel="stylesheet" href="../plugins/select2-bootstrap4-theme/select2-bootstrap4.min.css"/>
<link rel="stylesheet" href="../plugins/bootstrap4-duallistbox/bootstrap-duallistbox.min.css"/>

<link rel="stylesheet" href="../plugins/datatables-bs4/css/dataTables.bootstrap4.css"/>
<link rel="stylesheet" href="../plugins/datatables-responsive/css/responsive.bootstrap4.min.css"/>
<link rel="stylesheet" href="../plugins/datatables-buttons/css/buttons.bootstrap4.css"/>


<link rel="stylesheet" href="../plugins/datatables-colreorder/css/colReorder.bootstrap4.css"/>


<link rel="stylesheet" href="../plugins/summernote/summernote-bs4.css"/>



<style type="text/css">

.btn-whitelabel1 {
  color: #ffffff;
  background-color: <%=CorPrimaria(1) %>;
  border-color: <%=CorPrimaria(1) %>;
  box-shadow: none;
}

.btn-whitelabel1:hover {
  color: #ffffff;
  background-color: <%=CorPrimaria(1) %>;
  border-color: <%=CorPrimaria(1) %>;
  opacity:0.8;
}

.btn-whitelabel1:focus, .btn-whitelabel1.focus {
  box-shadow: none, 0 0 0 0 rgba(38, 143, 255, 0.5);
}

.btn-whitelabel1.disabled, .btn-whitelabel1:disabled {
  color: #ffffff;
  background-color: <%=CorPrimaria(1) %>;
  border-color: <%=CorPrimaria(1) %>;
}

.btn-whitelabel1:not(:disabled):not(.disabled):active, ..btn-whitelabel1:not(:disabled):not(.disabled).active,
.show > .btn-whitelabel1.dropdown-toggle {
  color: #ffffff;
  background-color: <%=CorPrimaria(1) %>;
  border-color: <%=CorPrimaria(1) %>;
}

.btn-whitelabel1:not(:disabled):not(.disabled):active:focus, .btn-whitelabel1:not(:disabled):not(.disabled).active:focus,
.show > .btn-whitelabel1.dropdown-toggle:focus {
  box-shadow: 0 0 0 0 rgba(38, 143, 255, 0.5);
}

.btn-whitelabel2 {
  background-color: <%=CorPrimaria(2) %>;
  border-color: <%=CorPrimaria(2) %>;
  color: #ffffff;
}

.btn-whitelabel2:hover {
  background-color: <%=CorPrimaria(2) %>;
  border-color: <%=CorPrimaria(2) %>;
  opacity:0.9;
}

.btn-whitelabel2.disabled, .btn-whitelabel2:disabled {
  background-color: <%=CorPrimaria(2) %>;
  border-color: <%=CorPrimaria(2) %>;
}

.btn-whitelabel2:not(:disabled):not(.disabled):active, .btn-whitelabel2:not(:disabled):not(.disabled).active,
.show > .btn-whitelabel2.dropdown-toggle {
  background-color: <%=CorPrimaria(2) %>;
  border-color: <%=CorPrimaria(2) %>;
  opacity:0.9;
}

.bg-whitelabel1, .alert-success {
  background-color: <%=CorPrimaria(1) %> !important;
  color: #ffffff;
}

.bg-whitelabel2, .alert-success {
  background-color: <%=CorPrimaria(2) %> !important;
  color: #ffffff;
}

.text-whitelabel1 {
  color: <%=CorPrimaria(1) %> !important;
}

.text-whitelabel2 {
  color: <%=CorPrimaria(2) %> !important;
}

.bg-cor1
{
    background-color:   <%=colorTween(10) %> !important;
}
.bg-cor2
{
    background-color:   <%=colorTween(20) %> !important;
}
.bg-cor3
{
    background-color:   <%=colorTween(30) %> !important;
}
.bg-cor4
{
    background-color:   <%=colorTween(40) %> !important;
}
.bg-cor5
{
    background-color:   <%=colorTween(50) %> !important;
}
.bg-cor6
{
    background-color:   <%=colorTween(60) %> !important;
}
.bg-cor7
{
    background-color:   <%=colorTween(70) %> !important;
}
.bg-cor8
{
    background-color:   <%=colorTween(80) %> !important;
}
.bg-cor9
{
    background-color:   <%=colorTween(90) %> !important;
}
.bg-cor10
{
    background-color:   <%=colorTween(100) %> !important;
}

</style>

<style type="text/css">
    /* Make circles that indicate the steps of the form: */
    .step {
      height: 30px;
      width: 30px;
      margin: 0 2px;
      background-color: #bbbbbb;
      border: none;  
      border-radius: 50%;
      display: inline-block;
      opacity: 1;
    }

    .step.active {
      opacity: 1;
      background-color: #626262;
      color: #ffffff;
    }

    /* Mark the steps that are finished and valid: */
    .step.finish {
      background-color: <%=CorPrimaria(1) %>;
      color: #ffffff;
    }
</style>

<script type="text/javascript" src="../plugins/jquery/jquery.min.js"></script>
<script type="text/javascript" src="../plugins/bootstrap/js/bootstrap.bundle.min.js"></script>
<script type="text/javascript" src="../plugins/overlayScrollbars/js/jquery.overlayScrollbars.min.js"></script>
<script type="text/javascript" src="../dist/js/adminlte.js"></script>
<script type="text/javascript" src="../dist/js/demo.js"></script>


<script type="text/javascript" src="../dist/js/pages/dashboard2.js"></script>   
<script type="text/javascript" src="../plugins/jquery-knob/jquery.knob.min.js"></script>
        
        
<script type="text/javascript" src="../plugins/select2/js/select2.full.min.js"></script>
<script type="text/javascript" src="../plugins/bootstrap4-duallistbox/jquery.bootstrap-duallistbox.min.js"></script>

<script type="text/javascript" src="../plugins/bootstrap-switch/js/bootstrap-switch.min.js"></script>
<script type="text/javascript" src="../dist/js/adminlte.js"></script>

<script type="text/javascript" src="../funcoes.js"></script>
<script type="text/javascript" src="../plugins/summernote/summernote-bs4.min.js"></script>

<script type="text/javascript" src="../plugins/datatables/jquery.dataTables.min.js"></script>
<script type="text/javascript" src="../plugins/datatables-bs4/js/dataTables.bootstrap4.min.js"></script>
<script type="text/javascript" src="../plugins/datatables-responsive/js/dataTables.responsive.min.js"></script>
<script type="text/javascript" src="../plugins/datatables-responsive/js/responsive.bootstrap4.min.js"></script>
<script type="text/javascript" src="../plugins/datatables-buttons/js/dataTables.buttons.min.js"></script>
<script type="text/javascript" src="../plugins/datatables-buttons/js/buttons.bootstrap4.min.js"></script>
<script type="text/javascript" src="../plugins/jszip/jszip.min.js"></script>
<script type="text/javascript" src="../plugins/pdfmake/pdfmake.min.js"></script>
<script type="text/javascript" src="../plugins/pdfmake/vfs_fonts.js"></script>
<script type="text/javascript" src="../plugins/datatables-buttons/js/buttons.html5.min.js"></script>
<script type="text/javascript" src="../plugins/datatables-buttons/js/buttons.print.min.js"></script>
<script type="text/javascript" src="../plugins/datatables-buttons/js/buttons.colVis.min.js"></script>
