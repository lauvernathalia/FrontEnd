<%@ Page Language="C#" AutoEventWireup="true" CodeFile="con_eventos_relatorio.aspx.cs" Inherits="con_eventos_relatorio" %>

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
                              <li class="breadcrumb-item"><a href="#">Eventos</a></li>
                              <li class="breadcrumb-item active">Relatórios</li>
                            </ol>
                          </div>
                        </div>

                        <div class="row">
                          <div class="col-12">

                            <div class="card">

                              <div class="card-header bg-whitelabel1">
                                <h3 class="card-title">Campos de Pesquisa</h3>
                              </div>
                              <div class="card-body">
                                
                                <div class="row">


                                    <div class="col-sm-4">
                                        <div class="form-group">
                                            <label>Evento</label>
                                            <asp:DropDownList id="ddlEvento" runat="server" class="form-control" 
                                                AutoPostBack="True" onselectedindexchanged="ddlEvento_SelectedIndexChanged" >
                                            </asp:DropDownList>                    
                                        </div>
                                    </div>

                                    <div class="col-sm-3">
                                        <div class="form-group">
                                            <label>Início Evento</label>
                                            <div class="input-group date" id="datepickerIni" data-target-input="nearest">
								                <asp:TextBox ID="txtDataIni" runat="server" CssClass="form-control datetimepicker-input" data-target="#datepickerIni"></asp:TextBox>
                                                <div class="input-group-append" data-target="#datepickerIni" data-toggle="datetimepicker">
                                                    <div class="input-group-text"><i class="fa fa-calendar"></i></div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>

                                    <div class="col-sm-3">
                                        <div class="form-group">
                                            <label>Término Evento</label>
                                            <div class="input-group date" id="datepickerFim" data-target-input="nearest">
								                <asp:TextBox ID="txtDataFim" runat="server" CssClass="form-control datetimepicker-input" data-target="#datepickerFim"></asp:TextBox>
                                                <div class="input-group-append" data-target="#datepickerFim" data-toggle="datetimepicker">
                                                    <div class="input-group-text"><i class="fa fa-calendar"></i></div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>

                                    <div class="col-sm-2">
                                        <div class="form-group">
                                            <label>Serial</label>
                                            <asp:DropDownList runat="server" ID="ddlSerial" CssClass="form-control">
                                            </asp:DropDownList>
                                        </div>
                                    </div>

                                </div>
                                <div class="row">
                                    <div class="col-sm-2">
                                        <div class="form-group">
                                            <label>Tipo</label>
                                            <asp:DropDownList runat="server" ID="ddlTipo" CssClass="form-control" 
                                                AutoPostBack="True" onselectedindexchanged="ddlTipo_SelectedIndexChanged">
                                                <asp:ListItem Text="Analítico" Value="A"></asp:ListItem>
                                                <asp:ListItem Text="Sintético" Value="S"></asp:ListItem>
                                                <asp:ListItem Text="Fechamento" Value="F"></asp:ListItem>
                                            </asp:DropDownList>
                                        </div>
                                    </div>
                                </div>

                              </div>

                              <div class="card-footer">
                                <asp:Button runat="server" ID="btnPesquisar" CssClass="btn btn-whitelabel1 col-2" Text="Pesquisar" onclick="btnPesquisar_Click"/>

                                <asp:Button ID="btnPostback" runat="server" Visible="false" OnClick="btnPostBack_Click" />
                                <script type="text/javascript">
                                    function PostBackOnMainPage(){
                                    <%=GetPostBackScript()%>
                                    }
                                </script>
                              </div>

                              </div>

                            </div>
                          </div>
                          <!-- Relatório DETALHE -->
                          <div class="row" >
                            <div class="col-sm-12">

                                <div class="card" runat="server" id="divRelDetalhe" visible="false">

                                  <div class="card-header bg-whitelabel1">
                                    <h3 class="card-title">Relatório Analítico das Vendas</h3>
                                  </div>
                                  <div class="card-body" style="overflow:auto; width: 100%;>
                                    <div class="row">
                                        <div class="col-12">

                                            <table id="tblConsulta" class="table table-bordered table-hover">
                                                <thead>
                                                <tr>
                                                  <th>ID</th>
                                                  <th>Data</th>
                                                  <th>Valor Venda</th>
                                                  <th>Plano</th>
                                                  <th>MDR (%)</th>
                                                  <th>Taxa Total (%)</th>
                                                  <th>Markup (%)</th>
                                                  <th>Markup (R$)</th>
                                                  <th>Total Taxas (R$)</th>
                                                  <th>Valor Líquido</th>
                                                  <th>Parcelas</th>
                                                  <th>Operação</th>
                                                  <th>Bandeira</th>
                                                  <th>Serial</th>
                                                  <th>Status</th>
                                                  <th>Locatário</th>
                                                  <th>Locador</th>
                                                </tr>
                                                </thead>
                                                <tbody>
                                                <asp:Repeater runat="server" ID="rptConsulta" OnItemCommand="rptConsulta_OnItemCommand">
                                                    <ItemTemplate>

                                                        <tr>
                                                          <td>
                                                            <small><%# DataBinder.Eval(Container.DataItem,"NOM_CODE") %></small>
                                                          </td>
                                                          <td>
                                                              <small><%# String.Format("{0:dd/MM/yyyy}",DataBinder.Eval(Container.DataItem, "DTA_DATA"))%></small>
                                                          </td>
                                                          <td>
                                                              <small><%# String.Format("{0:n2}",DataBinder.Eval(Container.DataItem, "NUM_VALOR_BRUTO"))%></small>
                                                          </td>

                                                          <td>
                                                              <small><%# DataBinder.Eval(Container.DataItem, "NOM_TITULO_PLANO")%></small>
                                                          </td>

                                                          <td>
                                                              <small><%# String.Format("{0:n2}",DataBinder.Eval(Container.DataItem, "NUM_TAXA"))%> %</small>
                                                          </td>

                                                          <td>
                                                              <small><%# String.Format("{0:n2}", DataBinder.Eval(Container.DataItem, "NUM_PERCENTUAL_TOTAL_TAXAS"))%> %</small>
                                                          </td>
                                                          <td>
                                                              <small><%# String.Format("{0:n2}",DataBinder.Eval(Container.DataItem, "NUM_MARKUP"))%> %</small>
                                                          </td>
                                                          <td>
                                                              <small><%# String.Format("{0:c2}", DataBinder.Eval(Container.DataItem, "NUM_VALOR_MARKUP"))%></small>
                                                          </td>

                                                          <td>
                                                              <small><%# String.Format("{0:c2}", DataBinder.Eval(Container.DataItem, "NUM_VALOR_TOTAL_TAXAS"))%></small>
                                                          </td>

                                                          <td>
                                                              <small><%# String.Format("{0:n2}",DataBinder.Eval(Container.DataItem, "NUM_VALOR_LIQUIDO"))%></small>
                                                          </td>
                                                          <td>
                                                              <small><%# String.Format("{0:n0}",DataBinder.Eval(Container.DataItem, "NUM_PARCELAS"))%></small>
                                                          </td>

                                                          <td>
                                                            <small><%# DataBinder.Eval(Container.DataItem, "NOM_TIPO_OPERACAO")%></small>
                                                          </td>
                                                          <td>
                                                            <small><%# DataBinder.Eval(Container.DataItem, "NOM_TIPO_OPERACAO_BANDEIRA")%></small>
                                                          </td>
                                                          <td>
                                                            <small><%# DataBinder.Eval(Container.DataItem,"NUM_SERIAL") %></small>
                                                          </td>
                                                          <td>
                                                            <small><%# DataBinder.Eval(Container.DataItem,"NOM_STATUS") %></small>
                                                          </td>

                                                          <td>
                                                            <small><%# DataBinder.Eval(Container.DataItem, "NOM_RAZAOSOCIAL_LOCATARIO")%></small>
                                                          </td>
                                                          <td>
                                                            <small><%# DataBinder.Eval(Container.DataItem, "NOM_RAZAOSOCIAL_LOCADOR")%></small>
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
                            <div class="col-sm-12">

                                <div class="card" runat="server" id="divRelSintetico" visible="false">

                                  <div class="card-header bg-whitelabel1">
                                    <h3 class="card-title">Relatório Sintético das Vendas</h3>
                                  </div>
                                  <div class="card-body" style="overflow:auto; width: 100%;>
                                    <div class="row">
                                        <div class="col-12">


                                            <table id="tblConsultaSintetico" class="table table-bordered table-hover">
                                                <thead>
                                                <tr>
                                                  <th>Locatário</th>
                                                  <th>Serial</th>
                                                  <th>Total Vendas</th>
                                                  <th>Total Taxa (R$)</th>
                                                  <th>Total Markup (R$)</th>
                                                  <th>Total Líquido</th>

                                                </tr>
                                                </thead>
                                                <tbody>
                                                <asp:Repeater runat="server" ID="rptConsultaSintetico">
                                                    <ItemTemplate>

                                                        <tr>
                                                          <td>
                                                            <small><%# DataBinder.Eval(Container.DataItem, "NOM_RAZAOSOCIAL_LOCATARIO")%></small>
                                                          </td>
                                                          <td>
                                                            <small><%# DataBinder.Eval(Container.DataItem,"NUM_SERIAL") %></small>
                                                          </td>

                                                          <td>
                                                              <small><%# String.Format("{0:n2}",DataBinder.Eval(Container.DataItem, "NUM_VALOR_BRUTO"))%></small>
                                                          </td>

                                                          <td>
                                                              <small><%# String.Format("{0:n2}",DataBinder.Eval(Container.DataItem, "NUM_VALOR_TAXA"))%></small>
                                                          </td>
                                                          <td>
                                                              <small><%# String.Format("{0:n2}",DataBinder.Eval(Container.DataItem, "NUM_VALOR_MARKUP"))%></small>
                                                          </td>


                                                          <td>
                                                              <small><%# String.Format("{0:n2}",DataBinder.Eval(Container.DataItem, "NUM_VALOR_LIQUIDO"))%></small>
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
                            <div class="col-sm-12">

                                <div class="card" runat="server" id="divRelFechamento" visible="false">

                                  <div class="card-header bg-whitelabel1">
                                    <h3 class="card-title">Relatório de Fechamento</h3>
                                  </div>
                                  <div class="card-body" style="overflow:auto; width: 100%;>
                                    <div class="row">
                                        <div class="col-12">


                                            <table id="tblConsultaFechamento" class="table table-bordered table-hover">
                                                <thead>
                                                <tr>
                                                  <th>Locatário</th>
                                                  <th>Total Venda</th>
                                                  <th>Total Taxa (R$)</th>
                                                  <th>Total Markup (R$)</th>
                                                  <th>Total Líquido</th>
                                                  <th>Total Repasses</th>
                                                  <th>Saldo Repasses</th>
                                                  <th>Comprovantes</th>
                                                </tr>
                                                </thead>
                                                <tbody>
                                                <asp:Repeater runat="server" ID="rptConsultaFechamento">
                                                    <ItemTemplate>

                                                        <tr>
                                                          <td>
                                                            <small><%# DataBinder.Eval(Container.DataItem, "NOM_RAZAOSOCIAL_LOCATARIO")%></small>
                                                          </td>
                                                          <td>
                                                              <small><%# String.Format("{0:n2}",DataBinder.Eval(Container.DataItem, "NUM_VALOR_BRUTO"))%></small>
                                                          </td>


                                                          <td>
                                                              <small><%# String.Format("{0:n2}",DataBinder.Eval(Container.DataItem, "NUM_VALOR_TAXA"))%></small>
                                                          </td>
                                                          <td>
                                                              <small><%# String.Format("{0:n2}",DataBinder.Eval(Container.DataItem, "NUM_VALOR_MARKUP"))%></small>
                                                          </td>

                                                          <td>
                                                              <small><%# String.Format("{0:n2}",DataBinder.Eval(Container.DataItem, "NUM_VALOR_LIQUIDO"))%></small>
                                                          </td>

                                                          <td>
                                                              <small><%# String.Format("{0:n2}",DataBinder.Eval(Container.DataItem, "NUM_VALOR_REPASSE"))%></small>
                                                          </td>
                                                          <td>
                                                              <small><%# String.Format("{0:n2}",DataBinder.Eval(Container.DataItem, "NUM_VALOR_SALDO"))%></small>
                                                          </td>

                                                          <td>
                                                              <a class="btn btn-sm btn-whitelabel1" href="" onclick="javascript:openPopupWindow('con_eventos_comprovantes.aspx?id=<%# Funcoes.Encrypt(DataBinder.Eval(Container.DataItem,"COD_ID_EVENTOS").ToString())%>&ec=<%# Funcoes.Encrypt(DataBinder.Eval(Container.DataItem,"COD_ID_PESSOAS_FJ").ToString())%>', 'EventosComprovantes', 1024, 800); return false;"><i class="fas fa-clipboard-list"></i></a>                                                            
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
                          <!-- Relatório SINTÉTICO -->
                          <div class="row" >
                            <div class="col-12">

                            </div>
                          </div>


                          <!-- Relatório FECHAMENTO -->
                          <div class="row" >
                            <div class="col-12">

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
    var table = $('#tblConsulta').DataTable({
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


<script type="text/javascript">

    $(document).ready(function () {
        var table = $('#tblConsultaSintetico').DataTable({
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
                    customize: function (doc) {
                        doc.pageMargins = [10, 10, 10, 10]
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


<script type="text/javascript">

    $(document).ready(function () {
        var table = $('#tblConsultaFechamento').DataTable({
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
                    customize: function (doc) {
                        doc.pageMargins = [10, 10, 10, 10]
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
