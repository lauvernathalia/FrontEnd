<%@ Page Language="C#" AutoEventWireup="true" CodeFile="teste_funcoes.aspx.cs" Inherits="teste_funcoes" %>

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

    <PORTAL:PAGEROTINA id="PageRotina1" title="Site Directory" runat="server" ModuleSource="rotinaspadroes.ascx"></PORTAL:PAGEROTINA>
    <link rel="stylesheet" href="../plugins/fullcalendar/main.min.css"/>
    <link rel="stylesheet" href="../plugins/fullcalendar-daygrid/main.min.css"/>
    <link rel="stylesheet" href="../plugins/fullcalendar-timegrid/main.min.css"/>
    <link rel="stylesheet" href="../plugins/fullcalendar-bootstrap/main.min.css"/>
    <script src="https://cdnjs.cloudflare.com/ajax/libs/Chart.js/2.5.0/Chart.min.js"></script>

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

</head>
<body>
<form id="form1" runat="server">

<div class="row">
    <div class="col-md-12">
        <h5>teste de funcoes sistema</h5>    
    </div>
</div>

<div class="container text-center">
<div class="row">
    <div class="col-md-12">
        <asp:GridView ID="GridView1" runat="server" CssClass="table table-striped"
            AutoGenerateColumns="False"
            AllowPaging="True"
            PageSize="10"
            OnPageIndexChanging="GridView1_PageIndexChanging">

            <Columns>
                <asp:BoundField DataField="COD_ID" HeaderText="Código" />
                <asp:BoundField DataField="DTA_DATA" HeaderText="Data" />
                <asp:BoundField DataField="NOM_CODE" HeaderText="Transação" />
            </Columns>
        </asp:GridView>
    </div>
</div>
</div>
</form>



</body>
</html>
