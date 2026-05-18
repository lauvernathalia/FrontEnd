<%@ Page Language="C#" AutoEventWireup="true" CodeFile="con_representantes_estabelecimentos_mkt.aspx.cs" Inherits="con_representantes_estabelecimentos_mkt" %>

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

        <div class="row">
            <div class="col-12">
            <div class="card card-success">

                <div class="card-header">
                    <h3 class="card-title">Estabelecimentos</h3><br />
                    <asp:Label runat="server" id="lblID"></asp:Label> - <asp:Label runat="server" id="lblNome"></asp:Label>
                </div>
                <div class="card-body">
                    <div class="form-group row col-12">
                        <div class="col-sm-12">
                            <label class="col-sm-12 col-form-label">Estabelecimentos</label>
                            <div class="input-group">
                                <div class="custom-file">
                                <asp:DropDownList runat="server" id="ddlEstabelecimentos" CssClass="form-control">
                                </asp:DropDownList>
                                </div>
                                <div class="input-group-append">
                                <asp:Button runat="server" ID="btnIncluirEstabelecimentos" CssClass="btn btn-whitelabel1" Text="Incluir Estabelecimento" onclick="btnIncluirEstabelecimentos_Click"/>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="card-footer">
                    <div class="row">
                        <div class="col-12">
                            <asp:Repeater runat="server" ID="rptConsultaEstabelecimentos" 
                                onitemcommand="rptConsultaEstabelecimentos_ItemCommand" >
                                <ItemTemplate>

                                            <strong>
                                            <span class="badge badge-secondary">
                                            <%# DataBinder.Eval(Container.DataItem, "NOM_RAZAOSOCIAL")%>
                                            <asp:linkbutton ID="lkbExcluirEstabelecimentos" CssClass="btn btn-secondary" commandname="Excluir" runat="server" text="X"  ToolTip="Excluir" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "COD_ID")%>'></asp:linkbutton>
                                            </span>
                                            </strong>
                                </ItemTemplate>
                            </asp:Repeater>
                            </div>
                        </div>

                </div>
            </div>
            </div>
        </div>

    </form>

<script type="text/javascript" src="../plugins/datatables/jquery.dataTables.js"></script>
<script type="text/javascript" src="../plugins/datatables-bs4/js/dataTables.bootstrap4.js"></script>
<script type="text/javascript">

$(document).ready(function () {
    var table = $('#tabConsulta').DataTable({
      "paging": false,
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
