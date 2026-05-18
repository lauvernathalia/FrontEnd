<%@ Page Language="C#" AutoEventWireup="true" CodeFile="con_marketplaces_comissionamentos_adm.aspx.cs" Inherits="con_marketplaces_comissionamentos_adm" %>

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
                        <h3 class="card-title">MARKETPLACE COMISSIONAMENTO</h3>
                    </div>
                    <div class="card-body">
                        <h3><span class="float-center badge bg-success">Adquirência</span></h3>

                        <div class="row">
                            <div class="col-sm-2">
                                <div class="form-group">
                                    <label>ID</label>
                                    <asp:TextBox runat="server" id="txtID" cssClass="form-control" Enabled="false"></asp:TextBox>
                                </div>
                            </div>
                            <div class="col-sm-10">
                                <div class="form-group">
                                    <label>Representante</label>
                                    <asp:TextBox runat="server" id="txtRazaosocial" cssClass="form-control" Enabled="false"></asp:TextBox>
                                </div>
                            </div>
                        </div>


                        <div class="row">
                            <div class="col-12">
                                <table id="tabConsulta" class="table table-bordered table-hover table-striped">
                                    <thead>
                                    <tr>
                                        <th>Ordem</th>
                                        <th>Bandeira</th>
                                        <th>Operação</th>
                                        <th>Comissão (%)</th>
                                    </tr>
                                    </thead>
                                    <tbody>
                                    <asp:Repeater runat="server" ID="rptConsulta" >
                                        <ItemTemplate>

                                            <tr>
                                                <td>
                                                <asp:TextBox runat="server" id="txtidpessoa" Text='<%# DataBinder.Eval(Container.DataItem, "COD_ID_PESSOAS_FJ")%>' visible="false"></asp:TextBox>
                                                <asp:TextBox runat="server" id="txtidlicenciado" Text='<%# DataBinder.Eval(Container.DataItem, "COD_ID_PESSOA_LICENCIADO")%>' visible="false"></asp:TextBox>
                                                <asp:TextBox runat="server" id="txtidbandeira" Text='<%# DataBinder.Eval(Container.DataItem, "COD_ID_BANDEIRA")%>' visible="false"></asp:TextBox>
                                                <asp:TextBox runat="server" id="txtidoperacao" Text='<%# DataBinder.Eval(Container.DataItem, "COD_ID_OPERACAO")%>' visible="false"></asp:TextBox>
                                                <asp:TextBox runat="server" id="txtidcomissionamento" Text='<%# DataBinder.Eval(Container.DataItem, "COD_ID_COMISSIONAMENTO")%>' visible="false"></asp:TextBox>
                                                    <small><strong><%# DataBinder.Eval(Container.DataItem, "NUM_ORDEM_BANDEIRAS")%></strong></small>
                                                </td>

                                                <td>
                                                    <small><strong><%# DataBinder.Eval(Container.DataItem, "NOM_BANDEIRA")%></strong></small>
                                                </td>
                                                <td>
                                                    <small><strong><%# DataBinder.Eval(Container.DataItem, "NOM_OPERACAO")%></strong></small>
                                                </td>

                                                <td>
                                                    <small><asp:TextBox runat="server" id="txtComissao" Text='<%# String.Format("{0:n2}",DataBinder.Eval(Container.DataItem, "NUM_PERCENTUAL_COMISSIONAMENTO"))%>' class="form-control"></asp:TextBox></small>
                                                </td>

                                            </tr>
                                        </ItemTemplate>
                                    </asp:Repeater>

                                    </tbody>
                                </table>
                                </div>
                            </div>





                    </div>

                    <div class="card-footer">
                        <asp:Button runat="server" ID="btnSalvar" CssClass="btn btn-whitelabel1" Text="Salvar" onclick="btnSalvar_Click"/>
                        <asp:Button runat="server" ID="btnCancelar" CssClass="btn btn-danger float-right" Text="Cancelar" onclick="btnCancelar_Click"/>
                    </div>
                </div>            
            
            </div>
        </div>
    </form>

<script type="text/javascript" src="../plugins/datatables/jquery.dataTables.js"></script>
<script type="text/javascript" src="../plugins/datatables-bs4/js/dataTables.bootstrap4.js"></script>

<script type="text/javascript" src="../plugins/inputmask/jquery.inputmask.bundle.js"></script>
<script type="text/javascript" src="../plugins/moment/moment.min.js"></script>

<script type="text/javascript">

$(document).ready(function () {
    var table = $('#tabConsulta').DataTable({
      "paging": false,
      "lengthChange": false,
      "searching": false,
      "ordering": false,
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
