<%@ Page Language="C#" AutoEventWireup="true" CodeFile="cad_estabelecimentos_taxas.aspx.cs" Inherits="cad_estabelecimentos_taxas" %>

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


    <style type="text/css">
        .style1
        {
            height: 4px;
        }
    </style>


</head>
<body class="hold-transition sidebar-mini layout-fixed">
    <form id="frmPrincipal" runat="server">

        <div class="row">
            <div class="col-12">

                <div class="card">
                    <div class="card-header bg-whitelabel1">
                        <h3 class="card-title">TAXAS</h3>
                    </div>
                    <div class="card-body">
                        <h3><span class="float-center badge bg-whitelabel1">Dados do Estabelecimento</span></h3>

                        <div class="row">
                            <div class="col-sm-2">
                                <div class="form-group">
                                    <label>ID</label>
                                    <asp:TextBox runat="server" id="txtID" cssClass="form-control" Enabled="false"></asp:TextBox>
                                </div>
                            </div>
                            <div class="col-sm-3">
                                <div class="form-group">
                                    <label>Estabelecimento</label>
                                    <asp:TextBox runat="server" id="txtRazaosocial" cssClass="form-control" Enabled="false"></asp:TextBox>
                                </div>
                            </div>
                            <div class="col-sm-3">
                                <div class="form-group">
                                    <label>E-mail</label>
                                    <asp:TextBox runat="server" id="txtEmail" cssClass="form-control" Enabled="false"></asp:TextBox>
                                </div>
                            </div>
                            <div class="col-sm-2">
                                <div class="form-group">
                                    <label>Tipo</label>
                                    <asp:TextBox runat="server" id="txtTipo" cssClass="form-control" Enabled="false"></asp:TextBox>
                                </div>
                            </div>
                            <div class="col-sm-2">
                                <div class="form-group">
                                    <label>Documento</label>
                                    <asp:TextBox runat="server" id="txtDocumento" cssClass="form-control" Enabled="false"></asp:TextBox>
                                </div>
                            </div>
                        </div>
                        


                        <h3><span class="float-center badge bg-whitelabel1">Taxas</span></h3>

                        <div class="row">
                            <div class="col-sm-6">
                                <div class="form-group">
                                <label>Adquirente<strong class="text-danger">*</strong></label>
                                <asp:DropDownList runat="server" id="ddlAdquirentes" CssClass="form-control" 
                                        AutoPostBack="True" 
                                        onselectedindexchanged="ddlAdquirentes_SelectedIndexChanged">
                                </asp:DropDownList>
                                </div>
                            </div>
                            <div class="col-sm-6">
                                <div class="form-group">
                                <label>Presencial/On-Line<strong class="text-danger">*</strong></label>
                                <asp:DropDownList runat="server" id="ddlPresencialOnLine" CssClass="form-control" AutoPostBack="True" onselectedindexchanged="ddlPresencialOnLine_SelectedIndexChanged">
                                    <asp:ListItem Text="Presencial" Value="P"></asp:ListItem>
                                    <asp:ListItem Text="On-Line" Value="O"></asp:ListItem>
                                </asp:DropDownList>
                                </div>
                            </div>

                        </div>

                        <div class="row mt-3">
                            <div class="col-12">

                                <div class="card">
                                    <div class="card-header bg-whitelabel1">
                                        <h3 class="card-title">LISTA DO PERCENTUAL DE MARKUP</h3>
                                    </div>
                                    <div class="card-body" style="overflow:auto; width: 100%;">
                                        <table id="tbConsultaMarkup" class="table table-bordered table-hover">
                                            <thead>
                                            <tr>
                                                <th><small>Bandeira</small></th>

                                                <th><small>
                                                    Por Transação (R$)<br />
                                                </small></th>

                                                <th><small>
                                                    Débito/À Vista (%)<br />
                                                </small></th>
                                                <th><small>Crédito 1x (%)</small></th>
                                                <th><small>Crédito 2x (%)</small></th>
                                                <th><small>Crédito 3x (%)</small></th>
                                                <th><small>Crédito 4x (%)</small></th>
                                                <th><small>Crédito 5x (%)</small></th>
                                                <th><small>Crédito 6x (%)</small></th>
                                                <th><small>Crédito 7x (%)</small></th>
                                                <th><small>Crédito 8x (%)</small></th>
                                                <th><small>Crédito 9x (%)</small></th>
                                                <th><small>Crédito 10x (%)</small></th>
                                                <th><small>Crédito 11x (%)</small></th>
                                                <th><small>Crédito 12x (%)</small></th>
                                                <th><small>Crédito 13x (%)</small></th>
                                                <th><small>Crédito 14x (%)</small></th>
                                                <th><small>Crédito 15x (%)</small></th>
                                                <th><small>Crédito 16x (%)</small></th>
                                                <th><small>Crédito 17x (%)</small></th>
                                                <th><small>Crédito 18x (%)</small></th>
                                                <th><small>Crédito 19x (%)</small></th>
                                                <th><small>Crédito 20x (%)</small></th>
                                                <th><small>Crédito 21x (%)</small></th>
                                            </tr>
                                            </thead>
                                            <tbody>
                                            <asp:Repeater runat="server" ID="rptConsultaMarkup" 
                                                    onitemcommand="rptConsultaMarkup_ItemCommand">
                                                <ItemTemplate>
                                                    <tr>
                                                        <td class="text-center">
                                                            <small><strong><%# DataBinder.Eval(Container.DataItem, "NOM_BANDEIRA")%></strong></small><br />
                                                            <asp:TextBox runat="server" id="txtidbandeira" Text='<%# DataBinder.Eval(Container.DataItem, "COD_ID_BANDEIRAS")%>' visible="false"></asp:TextBox>
                                                            <asp:Image runat="server" id="imgBandeira" CssClass="img-fluid img-thumbnail mx-auto" style="max-width:50px;" ImageUrl='<%# DataBinder.Eval(Container.DataItem, "NOM_IMAGEM")%>'></asp:Image><br />
                                                        </td>

                                                        <td class="text-center" style="min-width:90px;">
                                                            <small><strong><%# String.Format("{0:n2}",DataBinder.Eval(Container.DataItem, "NUM_VALOR_MARKUP_TRANSACAO")) %></strong></small>
                                                        </td>

                                                        <td class="text-center" style="min-width:90px;">
                                                            <small><strong><%# String.Format("{0:n2}",DataBinder.Eval(Container.DataItem, "NUM_VALOR_MARKUP_DEBITO")) %></strong></small>
                                                        </td>
                                                        <td class="text-center" style="min-width:90px;">
                                                            <small><strong><%# String.Format("{0:n2}",DataBinder.Eval(Container.DataItem, "NUM_VALOR_MARKUP_VISTA")) %></strong></small>
                                                        </td>
                                                        <td class="text-center" style="min-width:90px;">
                                                            <small><strong><%# String.Format("{0:n2}",DataBinder.Eval(Container.DataItem, "NUM_VALOR_MARKUP_2X")) %></strong></small>
                                                        </td>
                                                        <td class="text-center" style="min-width:90px;">
                                                            <small><strong><%# String.Format("{0:n2}",DataBinder.Eval(Container.DataItem, "NUM_VALOR_MARKUP_3X")) %></strong></small>
                                                        </td>
                                                        <td class="text-center" style="min-width:90px;">
                                                            <small><strong><%# String.Format("{0:n2}",DataBinder.Eval(Container.DataItem, "NUM_VALOR_MARKUP_4X")) %></strong></small>
                                                        </td>
                                                        <td class="text-center" style="min-width:90px;">
                                                            <small><strong><%# String.Format("{0:n2}",DataBinder.Eval(Container.DataItem, "NUM_VALOR_MARKUP_5X")) %></strong></small>
                                                        </td>
                                                        <td class="text-center" style="min-width:90px;">
                                                            <small><strong><%# String.Format("{0:n2}",DataBinder.Eval(Container.DataItem, "NUM_VALOR_MARKUP_6X")) %></strong></small>
                                                        </td>
                                                        <td class="text-center" style="min-width:90px;">
                                                            <small><strong><%# String.Format("{0:n2}",DataBinder.Eval(Container.DataItem, "NUM_VALOR_MARKUP_7X")) %></strong></small>
                                                        </td>
                                                        <td class="text-center" style="min-width:90px;">
                                                            <small><strong><%# String.Format("{0:n2}",DataBinder.Eval(Container.DataItem, "NUM_VALOR_MARKUP_8X")) %></strong></small>
                                                        </td>
                                                        <td class="text-center" style="min-width:90px;">
                                                            <small><strong><%# String.Format("{0:n2}",DataBinder.Eval(Container.DataItem, "NUM_VALOR_MARKUP_9X")) %></strong></small>
                                                        </td>
                                                        <td class="text-center" style="min-width:90px;">
                                                            <small><strong><%# String.Format("{0:n2}",DataBinder.Eval(Container.DataItem, "NUM_VALOR_MARKUP_10X")) %></strong></small>
                                                        </td>
                                                        <td class="text-center" style="min-width:90px;">
                                                            <small><strong><%# String.Format("{0:n2}",DataBinder.Eval(Container.DataItem, "NUM_VALOR_MARKUP_11X")) %></strong></small>
                                                        </td>
                                                        <td class="text-center" style="min-width:90px;">
                                                            <small><strong><%# String.Format("{0:n2}",DataBinder.Eval(Container.DataItem, "NUM_VALOR_MARKUP_12X")) %></strong></small>
                                                        </td>
                                                        <td class="text-center" style="min-width:90px;">
                                                            <small><strong><%# String.Format("{0:n2}",DataBinder.Eval(Container.DataItem, "NUM_VALOR_MARKUP_13X")) %></strong></small>
                                                        </td>
                                                        <td class="text-center" style="min-width:90px;">
                                                            <small><strong><%# String.Format("{0:n2}",DataBinder.Eval(Container.DataItem, "NUM_VALOR_MARKUP_14X")) %></strong></small>
                                                        </td>
                                                        <td class="text-center" style="min-width:90px;">
                                                            <small><strong><%# String.Format("{0:n2}",DataBinder.Eval(Container.DataItem, "NUM_VALOR_MARKUP_15X")) %></strong></small>
                                                        </td>
                                                        <td class="text-center" style="min-width:90px;">
                                                            <small><strong><%# String.Format("{0:n2}",DataBinder.Eval(Container.DataItem, "NUM_VALOR_MARKUP_16X")) %></strong></small>
                                                        </td>
                                                        <td class="text-center" style="min-width:90px;">
                                                            <small><strong><%# String.Format("{0:n2}",DataBinder.Eval(Container.DataItem, "NUM_VALOR_MARKUP_17X")) %></strong></small>
                                                        </td>
                                                        <td class="text-center" style="min-width:90px;">
                                                            <small><strong><%# String.Format("{0:n2}",DataBinder.Eval(Container.DataItem, "NUM_VALOR_MARKUP_18X")) %></strong></small>
                                                        </td>
                                                        <td class="text-center" style="min-width:90px;">
                                                            <small><strong><%# String.Format("{0:n2}",DataBinder.Eval(Container.DataItem, "NUM_VALOR_MARKUP_19X")) %></strong></small>
                                                        </td>
                                                        <td class="text-center" style="min-width:90px;">
                                                            <small><strong><%# String.Format("{0:n2}",DataBinder.Eval(Container.DataItem, "NUM_VALOR_MARKUP_20X")) %></strong></small>
                                                        </td>
                                                        <td class="text-center" style="min-width:90px;">
                                                            <small><strong><%# String.Format("{0:n2}",DataBinder.Eval(Container.DataItem, "NUM_VALOR_MARKUP_21X")) %></strong></small>
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

$(document).ready(function () {
    var table = $('#tbConsultaMarkup').DataTable({
        order: [[0, 'desc']],
        "responsive": false,
        "sPaginationType": "full_numbers",
        "oLanguage": {
            "sUrl": '//cdn.datatables.net/plug-ins/2.0.6/i18n/pt-BR.json'
        },
        "paging": true,
        "lengthChange": true,
        "searching": true,
        "ordering": true,
        "info": true,
        "autoWidth": true,
        buttons: [
                {
                    extend: 'copy',
                    exportOptions: {
                        columns: ':visible'
                    }
                },        
                {
                    extend: 'excel',
                    exportOptions: {
                        columns: ':visible'
                    }
                },        
                {
                    extend: 'pdf',
                       text: 'Export PDF',
                       orientation: 'landscape',
                       pageSize: 'LEGAL',
                       customize: function ( doc ) {
                         doc.pageMargins = [10,10,10,10]
                     },
                     exportOptions: {
                        columns: ':visible'
                    }
                },
                {
                    extend: 'csv',
                    exportOptions: {
                        columns: ':visible'
                    }
                },

                {
                    extend: 'print',
                    exportOptions: {
                        columns: ':visible'
                    }
                },
                'colvis'
        ],
        initComplete: function () {
            this.api().buttons().container()
            //.appendTo( $ ('#table_id_wrapper .col-md-6:eq(0)', this.api().table (). container ()));
            //.appendTo( $('#table_id_wrapper .col-md-6:eq(0)' ) );
        .appendTo($('.col-md-6:eq(0)', this.api().table().container()));

        }

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
