<%@ Page Language="C#" AutoEventWireup="true" CodeFile="con_conciliacao.aspx.cs" Inherits="con_conciliacao" %>

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
                              <li class="breadcrumb-item"><a href="#">Operações Diárias</a></li>
                              <li class="breadcrumb-item active">Conciliação</li>
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

                                    <label class="col-2 col-form-label">Conciliado</label>
						            <div class="col-2">
								            <asp:DropDownList runat="server" ID="ddlConciliado" CssClass="form-control">
                                                <asp:ListItem Text="Todos" Value=" "></asp:ListItem>
                                                <asp:ListItem Text="Não" Value="N"></asp:ListItem>
                                                <asp:ListItem Text="Sim" Value="S"></asp:ListItem>
                                            </asp:DropDownList>
                                        </div>
                                    </div>


                                </div>

                              </div>

                              <div class="card-footer" style="overflow:auto; width: 100%; ">

                                <asp:Button runat="server" ID="btnPesquisar" CssClass="btn btn-whitelabel1" Text="Pesquisar" onclick="btnPesquisar_Click"/>
                                <asp:Button runat="server" ID="btnConciliar" CssClass="btn btn-whitelabel1" Text="Conciliar" onclick="btnConciliar_Click"/>
                                <asp:Button runat="server" ID="btnProcessar" CssClass="btn btn-whitelabel1" Text="Processar" onclick="btnProcessar_Click"/>
                                <asp:Button runat="server" ID="btnImportarTexto" CssClass="btn btn-whitelabel1 float-right" Text="Importar CSV" onclick="btnImportarTexto_Click"/>

                                <div class="btn btn-default btn-sm float-right">
                                    <i class="fas fa-paperclip"></i> Anexo
                                    <input type="file" name="attachment" runat="server" id="File1" />
                                </div>
                                <br />
                                <asp:GridView ID="GridView1" runat="server" Visible="false"></asp:GridView>

                              </div>

                              </div>

                            </div>
                          </div>
                          <div class="row">
                            <div class="col-12">
                                <div class="card">

                                  <div class="card-header bg-whitelabel1">
                                    <h3 class="card-title">Conciliação</h3>
                                  </div>
                                  <div class="card-body"  style="overflow:auto; width: 100%;">

                                    <div class="row">
                                        <div class="col-12">
                                            <asp:TextBox runat="server" ID="txtJson" Rows="5" TextMode="MultiLine" CssClass="form-control"></asp:TextBox>
                                        </div>
                                    </div>


                                    <div class="row">
                                        <div class="col-12">


                                            <asp:GridView ID="gvConsulta" runat="server" AutoGenerateColumns="false" BorderStyle="None" CssClass="table table-striped nowrap table-bordered table-hover">
                                                <AlternatingRowStyle BorderStyle="None" />
                                                <RowStyle BorderStyle="None" />
                                                <HeaderStyle BorderWidth="1" BorderStyle="Solid" />
                                                <Columns>

                                                  <asp:TemplateField HeaderText="Data">
                                                    <ItemTemplate>
                                                        <small><%# String.Format("{0:dd/MM/yyyy}", DataBinder.Eval(Container.DataItem, "DTA_TRANSACAO"))%> <%# String.Format("{0:HH:mm}", DataBinder.Eval(Container.DataItem, "DTA_TRANSACAO"))%></small>
                                                    </ItemTemplate>
                                                  </asp:TemplateField>
                                                  <asp:TemplateField HeaderText="ID">
                                                    <ItemTemplate>
                                                    <small><strong><%# DataBinder.Eval(Container.DataItem, "COD_ID")%></strong></small>
                                                    <asp:TextBox runat="server" ID="txtID" Text='<%# DataBinder.Eval(Container.DataItem, "COD_ID")%>' Visible="false"></asp:TextBox>
                                                    </ItemTemplate>
                                                  </asp:TemplateField>

                                                  <asp:TemplateField HeaderText="ID Transação">
                                                    <ItemTemplate>
                                                    <small><strong><%# DataBinder.Eval(Container.DataItem, "IDdatransacao")%></strong></small>
                                                    <asp:TextBox runat="server" ID="txtIDTransacao" Text='<%# DataBinder.Eval(Container.DataItem, "IDdatransacao")%>' Visible="false"></asp:TextBox>
                                                    </ItemTemplate>
                                                  </asp:TemplateField>

                                                  <asp:TemplateField HeaderText="Vendedor">
                                                    <ItemTemplate>
                                                    <small><strong><%# DataBinder.Eval(Container.DataItem, "statementDescriptor")%></strong></small>
                                                    </ItemTemplate>
                                                  </asp:TemplateField>

                                                  <asp:TemplateField HeaderText="Tipo">
                                                    <ItemTemplate>
                                                    <small><strong><%# DataBinder.Eval(Container.DataItem, "Tipo")%></strong></small>
                                                    </ItemTemplate>
                                                  </asp:TemplateField>
                                                  <asp:TemplateField HeaderText="Bandeira">
                                                    <ItemTemplate>
                                                    <small><strong><%# DataBinder.Eval(Container.DataItem, "Bandeira")%></strong></small>
                                                    </ItemTemplate>
                                                  </asp:TemplateField>

                                                  <asp:TemplateField HeaderText="Valor">
                                                    <ItemTemplate>
                                                    <small><strong><%# DataBinder.Eval(Container.DataItem, "ValorOriginal")%></strong></small>
                                                    </ItemTemplate>
                                                  </asp:TemplateField>

                                                  <asp:TemplateField HeaderText="Conciliado">
                                                    <ItemTemplate>
                                                    <small><strong><%# DataBinder.Eval(Container.DataItem, "FLG_CONCILIADO")%></strong></small>
                                                    <asp:TextBox runat="server" ID="txtConciliado" Text='<%# DataBinder.Eval(Container.DataItem, "FLG_CONCILIADO")%>' Visible="false"></asp:TextBox>

                                                    </ItemTemplate>
                                                  </asp:TemplateField>

                                                  <asp:TemplateField HeaderText="Código Transação">
                                                    <ItemTemplate>
                                                    <small><strong><%# DataBinder.Eval(Container.DataItem, "COD_ID_TRANSACOES")%></strong></small>
                                                    </ItemTemplate>
                                                  </asp:TemplateField>

                                                  <asp:TemplateField HeaderText="Processado">
                                                    <ItemTemplate>
                                                    <small><strong><%# DataBinder.Eval(Container.DataItem, "FLG_PROCESSADO")%></strong></small>
                                                    </ItemTemplate>
                                                  </asp:TemplateField>
                                                </Columns>
                                                <PagerStyle BorderStyle="None" />
                                                <RowStyle BorderStyle="None" />
                                            </asp:GridView>  

                                       
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
        $('[id$=gvConsulta]').prepend($("<thead></thead>").append($('[id$=gvConsulta]').find("tr:first"))).DataTable({
            "responsive": false,
            "sPaginationType": "full_numbers",
            "oLanguage": {
                "sUrl": '//cdn.datatables.net/plug-ins/2.0.6/i18n/pt-BR.json'
            },
            "paging": true,
            "lengthChange": false,
            "searching": true,
            "ordering": true,
            "info": true,
            "autoWidth": false,
            buttons: ['copy', 'excel', 'pdf', 'csv', 'print'],
            initComplete: function () {
                this.api().buttons().container()
                //.appendTo( $ ('#table_id_wrapper .col-md-6:eq(0)', this.api().table (). container ()));
                //.appendTo( $('#table_id_wrapper .col-md-6:eq(0)' ) );
            .appendTo($('.col-md-6:eq(0)', this.api().table().container()));

            }
        });
    });
</script>


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
      "paging": true,
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
