<%@ Page Language="C#" AutoEventWireup="true" CodeFile="con_cartoes_desbloquear.aspx.cs" Inherits="con_cartoes_desbloquear" %>




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
                              <li class="breadcrumb-item"><a href="#">Cartões</a></li>
                              <li class="breadcrumb-item active">Desbloquear Cartões</li>
                            </ol>
                          </div>
                        </div>




                        <div class="row">
                          <div class="col-12">
                            <div class="card">

                              <div class="card-header bg-whitelabel1">
                                <h3 class="card-title">Desbloquear Cartões</h3>
                              </div>
                              <div class="card-body">
                                <h6></b>ATENÇÃO!</b> Para desbloquear o cartrão é necessário primeiro realizar a sua ativação.</h6>
                                <h6 class="text-whitelabel1">Para ativar o cartão digite as informações abaixo:</h6>

                                <div class="row">
                                    <div class="col-sm-4">
                                        <div class="form-group">
                                            <asp:Label runat="server" ID="Label20" Text="PIN (Senha do Cartão)"></asp:Label>
                                            <asp:TextBox runat="server" ID="txtPIN" CssClass="form-control"></asp:TextBox>
                                        </div>
                                    </div>

                                    <div class="col-sm-4">
                                        <div class="form-group">
                                            <asp:Label runat="server" ID="Label2" Text="Confirme o PIN (Senha do Cartão)"></asp:Label>
                                            <asp:TextBox runat="server" ID="txtPINConfirmacao" CssClass="form-control"></asp:TextBox>
                                        </div>
                                    </div>

                                    <div class="col-sm-4">
                                        <div class="form-group">
                                            <asp:Label runat="server" ID="Label1" Text="Últimos 4 Dígitos do Cartão"></asp:Label>
                                            <asp:TextBox runat="server" ID="txtDigitos" CssClass="form-control"></asp:TextBox>
                                        </div>
                                    </div>
                                </div>


                              </div>
                              <div class="card-footer">


                              </div>
                            </div>
                          </div>
                        </div>
                        <div class="row">
                          <div class="col-12">
                            <div class="card">

                              <div class="card-header bg-whitelabel1">
                                <h3 class="card-title">Cartões</h3>
                              </div>
                              <div class="card-body">

                                <div class="row"  style="overflow:auto; width: 100%;">
                                    <div class="col-12">
                                        <table id="tabConsulta" class="table table-bordered table-hover">
                                            <thead>
                                                <tr>
                                                    <th>ID</th>
                                                    <th>Data</th>
                                                    <th>Nome Cartão</th>
                                                    <th>Status</th>
                                                    <th>Ativo</th>
                                                    <th>Desbloqueado</th>
                                                    <th>Ativar</th>
                                                    <th>Desbloquear</th>
                                                </tr>
                                            </thead>
                                            <tbody>
                                                <asp:Repeater runat="server" ID="rptConsulta" OnItemCommand="rptConsulta_OnItemCommand">
                                                    <ItemTemplate>
                                                        <tr>
                                                            <td>
                                                                <small><%# DataBinder.Eval(Container.DataItem, "COD_ID")%></small>
                                                                <asp:TextBox runat="server" ID="txtid" Text='<%# DataBinder.Eval(Container.DataItem, "COD_ID")%>' Visible="false"></asp:TextBox>
                                                                <asp:TextBox runat="server" ID="txtidcartao" Text='<%# DataBinder.Eval(Container.DataItem, "NUM_ID_CARTAO")%>' Visible="false"></asp:TextBox>
                                                            </td>
                                                            <td>
                                                                <small><%# String.Format("{0:dd/MM/yyyy}",DataBinder.Eval(Container.DataItem, "DTA_DATA"))%></small>
                                                            </td>
                                                            <td>
                                                                <small><%# DataBinder.Eval(Container.DataItem, "NOM_NOME_CARTAO")%></small>
                                                                <asp:TextBox runat="server" ID="txtNomeCartao" Text='<%# DataBinder.Eval(Container.DataItem, "NOM_NOME_CARTAO")%>' Visible="false"></asp:TextBox>
                                                            </td>
                                                            <td>
                                                                <small><%# DataBinder.Eval(Container.DataItem, "FLG_STATUS")%></small>
                                                            </td>
                                                            <td>
                                                                <small><%# DataBinder.Eval(Container.DataItem, "FLG_ATIVO")%></small>
                                                            </td>
                                                            <td>
                                                                <small><%# DataBinder.Eval(Container.DataItem, "FLG_DESBLOQUEADO")%></small>
                                                            </td>

                                                            <td>
                                                                <asp:linkbutton ID="lbkAtivar" CssClass="btn btn-info" commandname="Ativar" runat="server" text="Ativar" ToolTip="Ativar"  CommandArgument='<%# DataBinder.Eval(Container.DataItem, "COD_ID")%>'><i class="fas fa-check"></i></asp:linkbutton>
                                                            </td>

                                                            <td>
                                                                <asp:linkbutton ID="lkbDesbloquear" CssClass="btn btn-info" commandname="Desbloquear" runat="server" text="Desbloquear" ToolTip="Desbloquear"  CommandArgument='<%# DataBinder.Eval(Container.DataItem, "COD_ID")%>'><i class="fas fa-unlock"></i></asp:linkbutton>
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
                        </div>



                    </div>
                </section>
            </div>
            <PORTAL:PAGEBOTTOM id="Pageheader3" title="Site Directory" runat="server" ModuleSource="rodapepadrao.ascx"></PORTAL:PAGEBOTTOM><!-- Fim Rodapé da Pagina -->
    
        </div>
    </form>

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

<script type="text/javascript">

    $(function () {

        //Date range picker
        $('#datepickerIni').datetimepicker({
            format: 'DD/MM/YYYY'
        });
        $('#datepickerFim').datetimepicker({
            format: 'DD/MM/YYYY'
        });
    })
</script>        


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
