<%@ Page Language="C#" AutoEventWireup="true" CodeFile="con_menu.aspx.cs" Inherits="con_menu" %>

<%@ Register TagPrefix="Portal" TagName="PageBottom" Src="rodapepadrao.ascx" %>
<%@ Register TagPrefix="Portal" TagName="PageLeft" Src="menupadrao2.ascx" %>
<%@ Register TagPrefix="Portal" TagName="PageHeader" Src="topopadrao.ascx" %>
<%@ Register TagPrefix="Portal" TagName="PageRotina" Src="rotinaspadroes.ascx" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <meta charset="utf-8">
    <meta name="viewport" content="width=device-width, initial-scale=1">
    <meta http-equiv="x-ua-compatible" content="ie=edge">

    <title>LEGACYBANK</title>

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

            <PORTAL:PAGELEFT id="Pageheader2" title="Site Directory" runat="server" ModuleSource="menupadrao2.ascx"></PORTAL:PAGELEFT>


            <div class="content-wrapper">

                <section class="content">


                </section>
            </div>
            <PORTAL:PAGEBOTTOM id="Pageheader3" title="Site Directory" runat="server" ModuleSource="rodapepadrao.ascx"></PORTAL:PAGEBOTTOM><!-- Fim Rodapé da Pagina -->
    
        </div>
    </form>


<script type="text/javascript" src="../dist/js/demo.js"></script>

<script type="text/javascript" src="../plugins/jquery-mousewheel/jquery.mousewheel.js"></script>
<script type="text/javascript" src="../plugins/raphael/raphael.min.js"></script>
<script type="text/javascript" src="../plugins/jquery-mapael/jquery.mapael.min.js"></script>
<script type="text/javascript" src="../plugins/jquery-mapael/maps/world_countries.min.js"></script>
<script type="text/javascript" src="../plugins/jquery-mapael/maps/brazil.min.js"></script>

<script type="text/javascript" src="../plugins/chart.js/Chart.js"></script>


<script type="text/javascript" src="../plugins/datatables/jquery.dataTables.js"></script>
<script type="text/javascript" src="../plugins/datatables-bs4/js/dataTables.bootstrap4.js"></script>

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




</body>
</html>
