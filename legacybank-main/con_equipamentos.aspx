<%@ Page Language="C#" AutoEventWireup="true" CodeFile="con_equipamentos.aspx.cs" Inherits="con_equipamentos" %>

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
                              <li class="breadcrumb-item"><a href="#">Equipamentos</a></li>
                              <li class="breadcrumb-item active">Equipamentos</li>
                            </ol>
                          </div>
                        </div>




                        <div class="row">
                          <div class="col-12">
                            <div class="card">

                                <div class="card-header bg-whitelabel1">
                                    <h3 class="card-title">EQUIPAMENTOS</h3>
                                </div>
                                <div class="card-body">
                                    <div class="form-group row col-12">
                                        <label class="col-sm-2 col-form-label">No.Serial</label>
                                        <div class="col-sm-6">
                                            <asp:TextBox id="txtSerial" runat="server" class="form-control" placeholder="Número Serial"></asp:TextBox>
                                        </div>
                                        <label class="col-sm-2 col-form-label">Ativo</label>
                                        <div class="col-sm-2">
                                            <asp:DropDownList runat="server" id="ddlAtivo" CssClass="form-control">
                                                <asp:ListItem Value="S">Sim</asp:ListItem>
                                                <asp:ListItem Value="N">Não</asp:ListItem>
                                                <asp:ListItem Value=" ">Todos</asp:ListItem>
                                            </asp:DropDownList>
                                        </div>


                                    </div>
                                </div>
                                <div class="card-footer">
                                    <asp:Button runat="server" ID="btnPesquisar" CssClass="btn btn-whitelabel1 float-left" Text="Pesquisar" onclick="btnPesquisar_Click"/>
                                    <asp:Button runat="server" ID="btnNovo" CssClass="btn btn-whitelabel1 float-right" Text="(+) Novo Registro" onclick="btnNovo_Click"/>
                                    <asp:Button ID="btnPostback" runat="server" Visible="false" OnClick="btnPostBack_Click" />
                                    <script type="text/javascript">
                                    function PostBackOnMainPage(){
                                    <%=GetPostBackScript()%>
                                    }
                                    </script>
                                </div>

                              </div>
                              <div class="card-footer" style="overflow:auto; width: 100%; ">


                                <div class="row">
                                    <div class="col-12">
                                        <table id="tblCompleta" class="table table-bordered table-hover">
                                            <thead>
                                            <tr>
                                              <th>ID</th>
                                              <th>Editar</th>
                                              <th>Modelo</th>
                                              <th>Serial</th>
                                              <th>Token</th>
                                              <th>Propriedade</th>
                                              <th>Proprietário</th>
                                              <th>Ativo</th>
                                              <th>Status</th>
                                              <th>Chips</th>
                                              <th>Local (Responsável)</th>
                                              <th>Excluir</th>
                                            </tr>
                                            </thead>
                                            <tbody>
                                            <asp:Repeater runat="server" ID="rptConsulta" OnItemCommand="rptConsulta_OnItemCommand">
                                                <ItemTemplate>

                                                    <tr>
                                                      <td>
                                                          <small><strong><%# DataBinder.Eval(Container.DataItem, "COD_ID")%></strong></small>
                                                      </td>

                                                      <td>
                                                        <a href="" onclick="javascript:openPopupWindow('cad_equipamentos.aspx?id=<%# Funcoes.Encrypt(DataBinder.Eval(Container.DataItem, "COD_ID").ToString())%>', 'Equipamentos', 1024, 800); return false;" class="btn btn-sm btn-secondary">
                                                            <i class="fas fa-edit"></i>
                                                        </a>
                                                      </td>
                                                      <td>
                                                          <small><%# DataBinder.Eval(Container.DataItem, "NOM_MODELO")%></small>
                                                      </td>

                                                      <td>
                                                          <small><%# DataBinder.Eval(Container.DataItem, "NUM_SERIAL")%></small>
                                                      </td>
                                                      <td>
                                                          <small><%# DataBinder.Eval(Container.DataItem, "NUM_TOKEN")%></small>
                                                      </td>
                                                      <td>
                                                          <small><%# DataBinder.Eval(Container.DataItem, "NOM_FLG_TIPO")%></small>
                                                      </td>
                                                      <td>
                                                          <small><%# DataBinder.Eval(Container.DataItem, "NOM_PROPRIETARIO")%></small>
                                                      </td>
                                                      <td>
                                                          <small><%# DataBinder.Eval(Container.DataItem, "NOM_FLG_ATIVO")%></small>
                                                      </td>
                                                      <td>
                                                          <small></small>
                                                      </td>
                                                      <td>
                                                        <a href="" onclick="javascript:openPopupWindow('cad_equipamentos_chips.aspx?id=<%# Funcoes.Encrypt(DataBinder.Eval(Container.DataItem,"COD_ID").ToString())%>', 'EquipamentosChips', 1024, 800); return false;" class="btn btn-sm btn-info">
                                                            <i class="fas fa-microchip"></i>
                                                        </a>
                                                      </td>

                                                      <td>
                                                        <a href="" onclick="javascript:openPopupWindow('cad_equipamentos_local.aspx?id=<%# Funcoes.Encrypt(DataBinder.Eval(Container.DataItem,"COD_ID").ToString())%>', 'EquipamentosLocal', 1024, 800); return false;" class="btn btn-sm btn-info">
                                                            <i class="fas fa-map-marked-alt"></i>
                                                        </a>
                                                      </td>

                                                    

                                                        <td>
                                                            <asp:linkbutton ID="lkbExcluir" CssClass="btn btn-danger btn-sm" commandname="Excluir" runat="server" text="Excluir"  CommandArgument='<%# DataBinder.Eval(Container.DataItem, "COD_ID")%>'></asp:linkbutton>
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

    $(document).ready(function () {
        var table = $('#tblCompleta').DataTable({
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
