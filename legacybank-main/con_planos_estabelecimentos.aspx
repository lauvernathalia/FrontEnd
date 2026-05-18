<%@ Page Language="C#" AutoEventWireup="true" CodeFile="con_planos_estabelecimentos.aspx.cs" Inherits="con_planos_estabelecimentos" %>

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

                <div class="card">
                    <div class="card-header bg-whitelabel1">
                        <h3 class="card-title">PLANO - Estabelecimentos</h3>
                    </div>
                    <div class="card-body">
                        <h3><span class="float-center badge bg-whitelabel1">Dados do Plano</span></h3>

                        <div class="row">
                            <div class="col-sm-2">
                                <div class="form-group">
                                    <label>ID</label>
                                    <asp:TextBox runat="server" id="txtID" cssClass="form-control" Enabled="false"></asp:TextBox>
                                </div>
                            </div>
                            <div class="col-sm-3">
                                <div class="form-group">
                                    <label>Plano</label>
                                    <asp:TextBox runat="server" id="txtPlano" cssClass="form-control" Enabled="false"></asp:TextBox>
                                </div>
                            </div>
                            <div class="col-sm-7">
                                <div class="form-group">
                                    <label>Adquirente</label>
                                    <asp:TextBox runat="server" id="txtAdquirente" cssClass="form-control" Enabled="false"></asp:TextBox>
                                </div>
                            </div>

                        </div>
                        
                        <div class="row mt-3">
                            <div class="col-12">

                                <div class="card">
                                    <div class="card-header bg-whitelabel1">
                                        <h3 class="card-title">LISTA DE ESTABELECIMENTOS</h3>
                                    </div>
                                    <div class="card-body" style="overflow:auto; width: 100%;">
                                        <table id="tbEstabelecimento" class="table table-bordered table-hover">
                                            <thead>
                                            <tr>
                                                <th>ID</th>
                                                <th>Estabelecimento</th>
                                                <th>Tipo</th>
                                                <th>Documento</th>
                                            </tr>
                                            </thead>
                                            <tbody>
                                            <asp:Repeater runat="server" ID="rptConsulta">
                                                <ItemTemplate>
                                                    <tr>
                                                        <td>
                                                            <small><strong><%# DataBinder.Eval(Container.DataItem, "COD_ID")%></strong></small>
                                                        </td>
                                                        <td>
                                                            <small><strong><%# DataBinder.Eval(Container.DataItem, "NOM_RAZAOSOCIAL")%></strong></small>
                                                        </td>

                                                        <td>
                                                            <small><strong><%# DataBinder.Eval(Container.DataItem, "FLG_TIPO_PESSOA")%></strong></small>
                                                        </td>
                                                        <td>
                                                            <small><%# DataBinder.Eval(Container.DataItem, "NUM_CNPJCPF")%></small>
                                                        </td>

                                                    </tr>
                                                </ItemTemplate>
                                            </asp:Repeater>

                                            </tbody>
                                        </table>
                                    </div>
                                </div>            
                            </div>
                        </div>
                    </div>
                    <div class="card-footer">
                        <asp:Button runat="server" ID="btnFechar" CssClass="btn btn-danger float-right" Text="Fechar" onclick="btnFechar_Click"/>
                    </div>
                </div>

            </div>            
            
        </div>

    </form>

<script type="text/javascript" src="../plugins/datatables/jquery.dataTables.js"></script>
<script type="text/javascript" src="../plugins/datatables-bs4/js/dataTables.bootstrap4.js"></script>

<script src="../plugins/inputmask/jquery.inputmask.bundle.js"></script>
<script src="../plugins/moment/moment.min.js"></script>


<script type="text/javascript">

$(document).ready(function () {
    var table = $('#tbEstabelecimento').DataTable({
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
