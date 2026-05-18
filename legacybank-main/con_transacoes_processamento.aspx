<%@ Page Language="C#" AutoEventWireup="true" CodeFile="con_transacoes_processamento.aspx.cs" Inherits="con_transacoes_processamento" %>

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
                                <div class="card">
                                    <div class="card-header bg-whitelabel1">
                                        <h3 class="card-title">TRANSAÇÕES PENDENTES</h3>
                                    </div>

                                    <div class="card-body">
                                        <div class="form-group row col-12">
                                            <label class="col-2 col-form-label">Data Início</label>
						                    <div class="col-2">
                                                <div class="input-group date" id="datepickerIni" data-target-input="nearest">
								                    <asp:TextBox ID="txtDataIni" runat="server" CssClass="form-control datetimepicker-input" data-target="#datepickerIni"></asp:TextBox>
                                                    <div class="input-group-append" data-target="#datepickerIni" data-toggle="datetimepicker">
                                                        <div class="input-group-text"><i class="fa fa-calendar"></i></div>
                                                    </div>
                                                </div>
                                            </div>

                                            <label class="col-2 col-form-label">Data Fim</label>
						                    <div class="col-2">
                                                <div class="input-group date" id="datepickerFim" data-target-input="nearest">
								                    <asp:TextBox ID="txtDataFim" runat="server" CssClass="form-control datetimepicker-input" data-target="#datepickerFim"></asp:TextBox>
                                                    <div class="input-group-append" data-target="#datepickerFim" data-toggle="datetimepicker">
                                                        <div class="input-group-text"><i class="fa fa-calendar"></i></div>
                                                    </div>
                                                </div>
                                            </div>

                                            <label class="col-2 col-form-label">Token</label>
						                    <div class="col-2">
                                                <asp:TextBox ID="txtToken" runat="server" CssClass="form-control"></asp:TextBox>
                                            </div>
                                        </div>

                                        <div class="row">
                                            <div class="col-12">
                                                <table id="tblConsulta" class="table table-bordered table-hover">
                                                    <thead>
                                                    <tr>
                                                      <th>ID</th>
                                                      <th>DATA</th>
                                                      <th>TOKEN</th>
                                                      <th>CODE</th>
                                                      <th>PROCESSADO</th>
                                                    </tr>
                                                    </thead>
                                                    <tbody>
                                                    <asp:Repeater runat="server" ID="rptConsulta">
                                                        <ItemTemplate>
                                                            <tr>
                                                              <td>
                                                                  <small><strong><%# DataBinder.Eval(Container.DataItem, "COD_ID")%></strong></small>
                                                                  <asp:TextBox runat="server" ID="txtJSON" Text='<%# DataBinder.Eval(Container.DataItem, "DES_JSON")%>' Visible="false"></asp:TextBox>
                                                              </td>
                                                              <td><small><%# DataBinder.Eval(Container.DataItem, "DTA_DATA")%></small></td>
                                                              <td><small><%# DataBinder.Eval(Container.DataItem, "NUM_TOKEN")%></small></td>
                                                              <td><small><%# DataBinder.Eval(Container.DataItem, "NOM_CODE")%></small></td>
                                                              <td><small><%# DataBinder.Eval(Container.DataItem, "FLG_PROCESSADO")%></small></td>
                                                            </tr>
                                                        </ItemTemplate>
                                                    </asp:Repeater>

                                                    </tbody>
                                                </table>
                                              </div>
                                          </div>
                                      </div>
                                      <div class="card-footer">
                                        <asp:Button runat="server" ID="btnPesquisar" 
                                        CssClass="btn btn-whitelabel1" Text="Pesquisar" 
                                        onclick="btnPesquisar_Click" />
                                      </div>
                                  </div>


                        <div class="row">
                            <div class="col-12">
                                <div class="card">

                                    <div class="card-header bg-whitelabel1">
                                        <h3 class="card-title">TRANSAÇÕES PROCESSAMENTO</h3>
                                    </div>
                                    <div class="card-body">
                                        <div class="form-group row col-12">
                                            <label class="col-sm-12 col-form-label">JSON</label>
                                            <div class="col-sm-12">
                                                <asp:TextBox runat="server" ID="txtJsonCobranca" TextMode="MultiLine" Rows="10" 
                                                CssClass="form-control"></asp:TextBox>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="card-footer">
                                        <asp:Button runat="server" ID="btnProcessar" 
                                        CssClass="btn btn-whitelabel1" Text="Processar" 
                                        onclick="btnProcessar_Click" />
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

