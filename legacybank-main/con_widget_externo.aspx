<%@ Page Language="C#" AutoEventWireup="true" CodeFile="con_widget_externo.aspx.cs" Inherits="con_widget_externo" %>

<%@ Register TagPrefix="Portal" TagName="PageBottom" Src="rodapepadrao.ascx" %>
<%@ Register TagPrefix="Portal" TagName="PageLeft" Src="menupadrao.ascx" %>
<%@ Register TagPrefix="Portal" TagName="PageHeader" Src="topopadrao.ascx" %>
<%@ Register TagPrefix="Portal" TagName="PageRotina" Src="rotinaspadroes.ascx" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <meta charset="utf-8">
    <meta name="viewport" content="width=device-width, initial-scale=1">
    <meta http-equiv="x-ua-compatible" content="ie=edge">

    <title>LEGACYBANK</title>
    <link rel="icon" type="image/x-icon" href="../images/favicon-96x96.png">

    <PORTAL:PAGEROTINA id="PageRotina1" title="Site Directory" runat="server" ModuleSource="rotinaspadroes.ascx"></PORTAL:PAGEROTINA>
    <link rel="stylesheet" href="../plugins/fullcalendar/main.min.css"/>
    <link rel="stylesheet" href="../plugins/fullcalendar-daygrid/main.min.css"/>
    <link rel="stylesheet" href="../plugins/fullcalendar-timegrid/main.min.css"/>
    <link rel="stylesheet" href="../plugins/fullcalendar-bootstrap/main.min.css"/>
    <script src="https://cdnjs.cloudflare.com/ajax/libs/Chart.js/2.5.0/Chart.min.js"></script>


</head>
<body class="hold-transition sidebar-mini layout-fixed">
    <form id="frmPrincipal" runat="server">

        <div class="wrapper">
            <PORTAL:PAGEHEADER id="PageHeader1" title="Site Directory" runat="server" ModuleSource="topopadrao.ascx"></PORTAL:PAGEHEADER>
            <PORTAL:PAGELEFT id="Pageheader2" title="Site Directory" runat="server" ModuleSource="menupadrao.ascx"></PORTAL:PAGELEFT>
            <div class="content-wrapper">
                <section class="content">
                    <div class="container-fluid" >

                        <div class="row mb-2">
                          <div class="col-sm-12">
                            <ol class="breadcrumb">
                              <li class="breadcrumb-item"><a href="#">Widgets</a></li>
                              <li class="breadcrumb-item active"><asp:Label runat="server" ID="lblMenuWidget" Text="Widget"></asp:Label></li>
                            </ol>
                          </div>
                        </div>




                        <div class="row">
                            <div class="col-12">
                                <div class="card">
                                    <div class="card-header bg-whitelabel1">
                                    <h3 class="card-title"><asp:Label runat="server" ID="lblTituloWidget" Text="Widget"></asp:Label></h3>
                                    </div>
                                    <div class="card-body">

                                        <div id="widget-parceiro">
                                        <!-- begin widget whitelabel -->
                                         <div id="widget-pnh" >
                                         <pnh-widget data='[{"data-company-id":"353","datakey":"87639bd6-788d-483f-8bd1-58bb7fe6db17","data-theme":"<%=CorPrimaria(1) %>"}]' />
                                         </div>
                                         <script type="text/javascript" >
                                             (function () {
                                                 var _pnh = document.createElement("script");
                                                 _pnh.id = "scpPNH";
                                                 _pnh.defer = "defer";
                                                 _pnh.type = "text/javascript";
                                                 _pnh.async = true;
                                                 _pnh.src =
                                        "https://js.parcelenahora.com.br/v1/widget/js/widget.js?v1=" + Math.random();
                                                 var sP = document.getElementById("widget-pnh");
                                                 sP.parentNode.insertBefore(_pnh, sP);
                                             })();
                                         </script>
                                         <!-- end widget whitelabel -->
                                        <div>
                                         <!-- Mais conteúdo -->
                                        </div>                        
                        
                                        </div>



                                    </div>
                                    <div class="card-footer">

                                    </div>
                                </div>
                            </div>
                        </div>



                    </div>
                </section>
            </div>
            <PORTAL:PAGEBOTTOM id="Pageheader3" title="Site Directory" runat="server" ModuleSource="rodapepadrao.ascx"></PORTAL:PAGEBOTTOM><!-- Fim Rodapé da Pagina -->
    
        </div>
    </form>

<script type="text/javascript" src="../plugins/datatables/jquery.dataTables.js"></script>
<script type="text/javascript" src="../plugins/datatables-bs4/js/dataTables.bootstrap4.js"></script>
<script type="text/javascript">

$(document).ready(function () {
    var table = $('#tblCompleta').DataTable({
      order: [[0, 'desc']],
      "paging": true,
      "lengthChange": false,
      "searching": false,
      "ordering": true,
      "info": true,
      "autoWidth": false,
    });
 
    $('a.toggle-vis').on('click', function (e) {
        e.preventDefault();
 
        // Get the column API object
        var column = table.column($(this).attr('data-column'));
 
        // Toggle the visibility
        column.visible(!column.visible());
    });
});

$(document).ready(function () {
    var table = $('#tblParcial').DataTable({
      order: [[0, 'desc']],
      "paging": true,
      "lengthChange": false,
      "searching": false,
      "ordering": true,
      "info": true,
      "autoWidth": false,
    });
 
    $('a.toggle-vis').on('click', function (e) {
        e.preventDefault();
 
        // Get the column API object
        var column = table.column($(this).attr('data-column'));
 
        // Toggle the visibility
        column.visible(!column.visible());
    });
});


</script>       


</body>
</html>
