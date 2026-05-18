<%@ Page Language="C#" AutoEventWireup="true" CodeFile="con_marketplaces.aspx.cs" Inherits="con_marketplaces" Async="true" EnableSessionState="ReadOnly" MaintainScrollPositionOnPostback="true" %>

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
                              <li class="breadcrumb-item"><a href="#">Cadastro</a></li>
                              <li class="breadcrumb-item active">Marketplaces</li>
                            </ol>
                          </div>
                        </div>

                        <div class="row">
                            <div class="col-12">
                                <div class="card">

                                    <div class="card-header bg-whitelabel1">
                                    <h3 class="card-title">MARKETPLACES</h3>
                                    </div>
                                    <div class="card-body">

                                        <div class="form-group row col-12">
                                            <label class="col-2 col-form-label">Período de</label>
						                    <div class="col-2">
                                                <div class="input-group date" id="datepickerIni" data-target-input="nearest">
								                    <asp:TextBox ID="txtDataIni" runat="server" CssClass="form-control datetimepicker-input" data-target="#datepickerIni"></asp:TextBox>
                                                    <div class="input-group-append" data-target="#datepickerIni" data-toggle="datetimepicker">
                                                        <div class="input-group-text"><i class="fa fa-calendar"></i></div>
                                                    </div>
                                                </div>
                                            </div>

                                            <label class="col-1 col-form-label">à</label>
						                    <div class="col-2">
                                                <div class="input-group date" id="datepickerFim" data-target-input="nearest">
								                    <asp:TextBox ID="txtDataFim" runat="server" CssClass="form-control datetimepicker-input" data-target="#datepickerFim"></asp:TextBox>
                                                    <div class="input-group-append" data-target="#datepickerFim" data-toggle="datetimepicker">
                                                        <div class="input-group-text"><i class="fa fa-calendar"></i></div>
                                                    </div>
                                                </div>
                                            </div>

                                            <label class="col-2 col-form-label">Ativo</label>
                                            <div class="col-sm-2">
                                                <asp:DropDownList runat="server" id="ddlAtivo" CssClass="form-control">
                                                    <asp:ListItem Value="S">Sim</asp:ListItem>
                                                    <asp:ListItem Value="N">Não</asp:ListItem>
                                                    <asp:ListItem Value=" ">Todos</asp:ListItem>
                                                </asp:DropDownList>
                                            </div>

                                        </div>

                                        <div class="form-group row col-12">
                                            <label class="col-2 col-form-label">Razão Social/Nome</label>
                                            <div class="col-sm-4">
                                                <asp:TextBox runat="server" ID="txtNome" CssClass="form-control" placeholder="Digite a Razão Social ou Nome que deseja procurar"></asp:TextBox>
                                            </div>
                                            <label class="col-2 col-form-label">CNPJ/CPF</label>
                                            <div class="col-sm-4">
                                                <asp:TextBox runat="server" ID="txtCNPJCPF" CssClass="form-control" placeholder="Digite o CNPJ ou CPF que deseja procurar"></asp:TextBox>
                                            </div>


                                        </div>
                                    </div>

                                    <div class="card-footer">

                                        <asp:Button runat="server" ID="btnPesquisar" CssClass="btn btn-whitelabel1" Text="Pesquisar" onclick="btnPesquisar_Click"/>

                                        <asp:Button runat="server" ID="btnNovo" CssClass="btn btn-whitelabel1 float-right m-1" Text="(+) Novo Marketplace" onclick="btnNovo_Click"/>

                                        <asp:Button runat="server" ID="btnImportar" CssClass="btn btn-whitelabel1 float-right m-1" Text="Importar" onclick="btnImportar_Click"/>

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

                        <div class="row">
                            <div class="col-12">
                                <div class="card">

                                    <div class="card-header bg-whitelabel1">
                                    <h3 class="card-title">RESULTADO DA PESQUISA</h3>
                                    </div>
                                    <div class="card-body"  style="overflow:auto; width: 100%;">

                                        <div class="row">
                                            <div class="col-12">
                                                <table id="tabConsulta" class="table table-bordered table-hover">
                                                    <thead>
                                                    <tr>
                                                      <th><small>ID</small></th>
                                                      <th><small>Data</small></th>
                                                      <th><small>Editar</small></th>
                                                      <th><small>Ativo</small></th>
                                                      <th><small>Alterar Status</small></th>
                                                      <th><small>Token</small></th>
                                                      <th><small>Nome</small></th>
                                                      <th><small>Documento</small></th>

                                                      <th><small>Usuários</small></th>
                                                      <th><small>Perfil</small></th>

                                                      <th><small>Planos</small></th>
                                                      <th><small>Planos Referência</small></th>
                                                      <th><small>Adquirentes</small></th>
                                                      <th><small>...</small></th>


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
                                                                    <small><strong><%# String.Format("{0:dd/MM/yyyy}",DataBinder.Eval(Container.DataItem, "DTA_DATA"))%></strong></small>
                                                                </td>

                                                              <td>
                                                                <a href="" onclick="javascript:openPopupWindow('cad_marketplaces.aspx?id=<%# Funcoes.Encrypt(DataBinder.Eval(Container.DataItem,"COD_ID").ToString())%>', 'MarketplaceEdicao', 1024, 800); return false;" class="btn btn-sm btn-whitelabel1">
                                                                    <i class="fas fa-edit"></i>
                                                                </a>
                                                              </td>

                                                              <td>
                                                                  <small><strong><%# DataBinder.Eval(Container.DataItem, "NOM_FLG_ATIVO")%></strong></small>
                                                              </td>
                                                            <td>
                                                                <asp:linkbutton ID="lkbAtivar" CssClass="badge badge-success" commandname="Ativar" runat="server" text="Ativar" ToolTip="Ativar"  CommandArgument='<%# DataBinder.Eval(Container.DataItem, "COD_ID")%>'><i class="fas fa-check"></i>   Ativar</asp:linkbutton><br />
                                                                <asp:linkbutton ID="lkbInativar" CssClass="badge badge-danger" commandname="Inativar" runat="server" text="Inativar" ToolTip="Inativar"  CommandArgument='<%# DataBinder.Eval(Container.DataItem, "COD_ID")%>'><i class="fas fa-times"></i>   Inativar</asp:linkbutton><br />
                                                                <asp:linkbutton ID="lkbPendente" CssClass="badge badge-warning" commandname="Pendente" runat="server" text="Pendente" ToolTip="Pendente"  CommandArgument='<%# DataBinder.Eval(Container.DataItem, "COD_ID")%>'><i class="fas fa-exclamation"></i>   Pendente</asp:linkbutton>
                                                            </td>



                                                              <td>
                                                                  <small><strong><%# DataBinder.Eval(Container.DataItem, "NUM_TOKEN")%></strong></small>
                                                              </td>

                                                              <td>
                                                                  <small><strong><%# DataBinder.Eval(Container.DataItem, "NOM_RAZAOSOCIAL")%></strong></small><br />
                                                              </td>
                                                              <td>
                                                                  <small><%# DataBinder.Eval(Container.DataItem, "NOM_CNPJ")%></small>
                                                              </td>

                                                              <td>
                                                                <a href="" onclick="javascript:openPopupWindow('con_marketplaces_usuarios_adm.aspx?id=<%# Funcoes.Encrypt(DataBinder.Eval(Container.DataItem,"COD_ID").ToString())%>', 'MarketplaceUsuarios', 1024, 800); return false;" class="btn btn-sm btn-whitelabel1">
                                                                    <i class="fas fa-users"></i>
                                                                </a>
                                                              </td>
                                                                <td>
                                                                <a href="" onclick="javascript:openPopupWindow('cad_marketplaces_perfil.aspx?id=<%#DataBinder.Eval(Container.DataItem,"COD_ID")%>', 'MarketplacesPerfil', 1024, 800); return false;" class="btn btn-sm btn-whitelabel1">
                                                                    <i class="fas fa-clipboard-check"></i>
                                                                </a>
                                                                </td>


                                                              <td>
                                                                <a href="" onclick="javascript:openPopupWindow('con_marketplaces_planos_adm.aspx?id=<%#DataBinder.Eval(Container.DataItem,"COD_ID")%>', 'MarketplacePlanos', 1024, 800); return false;" class="btn btn-sm btn-whitelabel1">
                                                                    <i class="fas fa-calculator"></i>
                                                                </a>
                                                              </td>

                                                              <td>
                                                                <a href="" onclick="javascript:openPopupWindow('con_marketplaces_planos_referencia_adm.aspx?id=<%#DataBinder.Eval(Container.DataItem,"COD_ID")%>', 'MarketplacePlanosReferencia', 1024, 800); return false;" class="btn btn-sm btn-whitelabel1">
                                                                    <i class="fas fa-percent"></i>
                                                                </a>
                                                              </td>


                                                                <td>
                                                                    <small><%# DataBinder.Eval(Container.DataItem, "NOM_STATUS_ZOOP_ONBOARDING")%></small><br />
                                                                    <small><%# DataBinder.Eval(Container.DataItem, "NOM_STATUS_CAPPTA_ONBOARDING")%></small><br />
                                                                    <small><%# DataBinder.Eval(Container.DataItem, "NOM_STATUS_BAAS_ONBOARDING")%></small>
                                                                    <small><%# DataBinder.Eval(Container.DataItem, "NOM_STATUS_PAGSEGURO_ONBOARDING")%></small>
                                                                </td>


                                                              <!-- *************************** -->

                                                              <td>
                                                                <div class="btn-group">
                                                                    <button type="button" class="btn btn-default"><small>Ações</small></button>
                                                                    <button type="button" class="btn btn-default dropdown-toggle" data-toggle="dropdown">
                                                                      <span class="caret"></span>
                                                                      <span class="sr-only">Toggle Dropdown</span>
                                                                    </button>
                                                                    <div class="dropdown-menu" role="menu">
                                                                      <small>
                                                                      <a class="dropdown-item" href="" onclick="javascript:openPopupWindow('cad_marketplaces_contas.aspx?id=<%#Funcoes.Encrypt(DataBinder.Eval(Container.DataItem,"COD_ID").ToString())%>', 'MarketplaceContas', 1024, 800); return false;">Contas Bancárias</a>
                                                                      <a class="dropdown-item" href="" onclick="javascript:openPopupWindow('cad_marketplaces_onboarding.aspx?id=<%#Funcoes.Encrypt(DataBinder.Eval(Container.DataItem,"COD_ID").ToString())%>', 'MarketplaceOnboarding', 1024, 800); return false;">Onboarding</a>
                                                                      <a class="dropdown-item" href="" onclick="javascript:openPopupWindow('con_marketplaces_representantes_adm.aspx?id=<%#DataBinder.Eval(Container.DataItem,"COD_ID")%>', 'MarketplaceRepresentantes', 1024, 800); return false;">Representantes</a>
                                                                      <a class="dropdown-item" href="" onclick="javascript:openPopupWindow('con_marketplaces_estabelecimentos_adm.aspx?id=<%#DataBinder.Eval(Container.DataItem,"COD_ID")%>', 'MarketplaceEstabelecimentos', 1024, 800); return false;">Estabelecimentos</a>
                                                                      <a class="dropdown-item" href="" onclick="javascript:openPopupWindow('con_marketplaces_comissionamentos_adm.aspx?id=<%#DataBinder.Eval(Container.DataItem,"COD_ID")%>', 'MarketplaceComissionamentos', 1024, 800); return false;">Comissão</a>
                                                                      
                                                                      <a class="dropdown-item" href="" onclick="javascript:openPopupWindow('cad_marketplace_portal.aspx?id=<%#DataBinder.Eval(Container.DataItem,"COD_ID")%>', 'LicenciadosPortal', 1024, 800); return false;">Portal</a>
                                                                      <a class="dropdown-item" href="" onclick="javascript:openPopupWindow('cad_marketplace_email.aspx?id=<%#DataBinder.Eval(Container.DataItem,"COD_ID")%>', 'LicenciadosServidorEmail', 1024, 800); return false;">E-Mail</a>
                                                                      <a class="dropdown-item" href="" onclick="javascript:openPopupWindow('cad_marketplace_termos.aspx?id=<%#DataBinder.Eval(Container.DataItem,"COD_ID")%>', 'LicenciadosZoop', 1024, 800); return false;">Termos</a>
                                                                      </small>
                                                                      <!--<div class="dropdown-divider"></div>
                                                                      <a class="dropdown-item" href="#">04</a>-->
                                                                    </div>
                                                                  </div>                                                              
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


<!--
<script type="text/javascript">
    $(function () {
        $('[id$=gvConsultaCompleta]').prepend($("<thead></thead>").append($('[id$=gvConsultaCompleta]').find("tr:first"))).DataTable({
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

        $('a.toggle-vis').on('click', function (e) {
            e.preventDefault();

            // Get the column API object
            var column = table.column($(this).attr('data-column'));

            // Toggle the visibility
            column.visible(!column.visible());
        });


    });
</script>
-->
<!--
<script type="text/javascript">
    $(function () {
        $('[id$=tblCompleta]').prepend($("<thead></thead>").append($('[id$=tblCompleta]').find("tr:first"))).DataTable({
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

        $('a.toggle-vis').on('click', function (e) {
            e.preventDefault();

            // Get the column API object
            var column = table.column($(this).attr('data-column'));

            // Toggle the visibility
            column.visible(!column.visible());
        });


    });
</script>
-->

<script type="text/javascript">

$(document).ready(function () {
    var table = $('#tabConsulta').DataTable({
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
