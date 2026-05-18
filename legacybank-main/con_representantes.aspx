<%@ Page Language="C#" AutoEventWireup="true" CodeFile="con_representantes.aspx.cs" Inherits="con_representantes" Async="true" EnableSessionState="ReadOnly" MaintainScrollPositionOnPostback="true" %>

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
                              <li class="breadcrumb-item active">Representantes</li>
                            </ol>
                          </div>
                        </div>

                        <div class="row">
                            <div class="col-12">
                                <div class="card">
                                    <div class="card-header bg-whitelabel1">
                                    <h3 class="card-title">REPRESENTANTES</h3>
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
                                            <label class="col-sm-2 col-form-label">Filtro</label>
                                            <div class="col-sm-2">
                                                <asp:TextBox id="txtFiltro" runat="server" class="form-control" placeholder="Nome, CNPJ/CPF ou Token"></asp:TextBox>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="card-footer">
                                        <asp:Button runat="server" ID="btnPesquisar" CssClass="btn btn-whitelabel1" Text="Pesquisar" onclick="btnPesquisar_Click"/>
                                        <asp:Button runat="server" ID="btnNovo" CssClass="btn btn-whitelabel1 float-right" Text="(+) Novo Representante" onclick="btnNovo_Click"/>
                                        <asp:Button runat="server" ID="btnImportar" CssClass="btn btn-whitelabel1 float-right" Text="Importar CNPJ/CPF" onclick="btnImportar_Click"/>
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
                                    <div class="card-body" style="overflow:auto; width: 100%; ">
                                        <div class="row">
                                            <div class="col-12" runat="server" id="dvCompleta" visible="false">
                                                <table id="tblCompleta" class="table table-bordered table-hover">
                                                    <thead>
                                                    <tr>
                                                        <th><small><strong>ID</strong></small></th>
                                                        <th><small><strong>Data</strong></small></th>
                                                        <th><small><strong>Editar</strong></small></th>
                                                        <th><small><strong>Converter</strong></small></th>

                                                        <th><small><strong>Status</strong></small></th>
                                                        <th><small><strong>Alterar Status</strong></small></th>
                                                        <th><small><strong>Representante</strong></small></th>
                                                        <th><small><strong>Marketplace</strong></small></th>
                                                        <th><small><strong>Responsável</strong></small></th>
                                                        <th><small><strong>E-mail(s)</strong></small></th>
                                                        <th><small><strong>Telefone(s)</strong></small></th>
                                                        <th><small><strong>Usuário(s)</strong></small></th>
                                                        <th><small><strong>Perfil</strong></small></th>
                                                        <th><small><strong>Clientes</strong></small></th>
                                                        <th><small><strong>Planos</strong></small></th>
                                                        <th><small><strong>Pagseguro</strong></small></th>
                                                        <th><small><strong>Onboarding</strong></small></th>
                                                        <th><small><strong>Comissão</strong></small></th>
                                                        <th><small><strong>Plataformas</strong></small></th>
                                                    </tr>
                                                    </thead>
                                                    <tbody>
                                                    <asp:Repeater runat="server" ID="rptConsultaCompleta" OnItemCommand="rptConsultaCompleta_OnItemCommand">
                                                        <ItemTemplate>

                                                            <tr>
                                                                <td>
                                                                    <small><strong><%# DataBinder.Eval(Container.DataItem, "COD_ID")%></strong></small>
                                                                </td>
                                                                <td>
                                                                    <small><strong><%# String.Format("{0:dd/MM/yyyy}",DataBinder.Eval(Container.DataItem, "DTA_DATA"))%></strong></small>
                                                                </td>

                                                                <td>
                                                                <a href="" onclick="javascript:openPopupWindow('cad_representantes.aspx?id=<%# Funcoes.Encrypt(DataBinder.Eval(Container.DataItem,"COD_ID").ToString())%>', 'RepresentantesEdicao', 1024, 800); return false;" class="btn btn-sm btn-whitelabel1">
                                                                    <i class="fas fa-edit"></i>
                                                                </a>
                                                                </td>


                                                                <td>
                                                                <a href="" onclick="javascript:openPopupWindow('cad_representantes_converter.aspx?id=<%# Funcoes.Encrypt(DataBinder.Eval(Container.DataItem,"COD_ID").ToString())%>', 'RepresentantesConverter', 1024, 800); return false;" class="btn btn-sm btn-whitelabel1">
                                                                    <i class="fas fa-arrow-alt-circle-up"></i>
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
                                                                    <small><strong><%# DataBinder.Eval(Container.DataItem, "NOM_RAZAOSOCIAL")%></strong></small><br />
                                                                    <small><%# DataBinder.Eval(Container.DataItem, "NUM_DOCUMENTO")%></small>
                                                                </td>
                                                              <td>
                                                                  <small><%# DataBinder.Eval(Container.DataItem, "NOM_RAZAOSOCIAL_MARKETPLACE")%></small>
                                                              </td>

                                                                <td>
                                                                    <small><%# DataBinder.Eval(Container.DataItem, "NOM_NOME")%></small> <small><%# DataBinder.Eval(Container.DataItem, "NOM_SOBRENOME")%></small>
                                                                </td>

                                                                <td>
                                                                    <small><%# DataBinder.Eval(Container.DataItem, "NOM_EMAIL_EMPRESA")%></small><br />
                                                                    <small><%# DataBinder.Eval(Container.DataItem, "NOM_EMAIL")%></small>
                                                                </td>
                                                                <td>
                                                                    <small><%# DataBinder.Eval(Container.DataItem, "NUM_TELEFONE")%></small><br />
                                                                    <small><%# DataBinder.Eval(Container.DataItem, "NOM_CELULAR")%></small>
                                                                </td>

                                                                <td>
                                                                <a href="" onclick="javascript:openPopupWindow('cad_representantes_usuarios.aspx?id=<%# Funcoes.Encrypt(DataBinder.Eval(Container.DataItem,"COD_ID").ToString())%>', 'RepresentantesUsuarios', 1024, 800); return false;" class="btn btn-sm btn-whitelabel1">
                                                                    <i class="fas fa-users"></i>
                                                                </a>
                                                                </td>

                                                                <td>
                                                                <a href="" onclick="javascript:openPopupWindow('cad_representantes_perfil.aspx?id=<%#DataBinder.Eval(Container.DataItem,"COD_ID")%>', 'RepresentantesPerfil', 1024, 800); return false;" class="btn btn-sm btn-whitelabel1">
                                                                    <i class="fas fa-clipboard-check"></i>
                                                                </a>
                                                                </td>


                                                                <td>
                                                                <a href="" onclick="javascript:openPopupWindow('cad_representantes_estabelecimentos.aspx?id=<%#DataBinder.Eval(Container.DataItem,"COD_ID")%>', 'RepresentantesEstabelecimentos', 1024, 800); return false;" class="btn btn-sm btn-whitelabel1">
                                                                    <i class="fas fa-store"></i>
                                                                </a>
                                                                </td>


                                                                <td>
                                                                <a href="" onclick="javascript:openPopupWindow('con_representantes_planos_adm.aspx?id=<%#DataBinder.Eval(Container.DataItem,"COD_ID")%>', 'RepresentantesPlanos', 1024, 800); return false;" class="btn btn-sm btn-whitelabel1">
                                                                    <i class="fas fa-percent"></i>
                                                                </a>
                                                                </td>

                                                                <td>
                                                                <a href="" onclick="javascript:openPopupWindow('cad_representantes_pagseguro.aspx?id=<%#DataBinder.Eval(Container.DataItem,"COD_ID")%>', 'RepresentantesPagseguro', 1024, 800); return false;" class="btn btn-sm btn-whitelabel1">
                                                                    <i class="fas fa-money-check-alt"></i>
                                                                </a>
                                                                </td>


                                                                <td>
                                                                <a href="" onclick="javascript:openPopupWindow('cad_representantes_onboarding.aspx?id=<%#Funcoes.Encrypt(DataBinder.Eval(Container.DataItem,"COD_ID").ToString())%>', 'RepresentantesBaaS', 1024, 800); return false;" class="btn btn-sm btn-whitelabel1">
                                                                    <i class="fas fa-coins"></i>
                                                                </a>
                                                                </td>
                                                                <td>
                                                                <a href="" onclick="javascript:openPopupWindow('cad_representantes_comissao.aspx?id=<%#DataBinder.Eval(Container.DataItem,"COD_ID")%>', 'RepresentantesComissao', 1024, 800); return false;" class="btn btn-sm btn-whitelabel1">
                                                                    <i class="fas fa-percent"></i>
                                                                </a>
                                                                </td>


                                                                <td>
                                                                    <small><%# DataBinder.Eval(Container.DataItem, "FLG_ZOOP")%></small>
                                                                    <small><%# DataBinder.Eval(Container.DataItem, "FLG_PAG")%></small>
                                                                    <small><%# DataBinder.Eval(Container.DataItem, "FLG_HUB")%></small>
                                                                </td>
                                                            </tr>
                                                        </ItemTemplate>
                                                    </asp:Repeater>

                                                    </tbody>
                                                </table>
                                                </div>
                                                <!-- Tabela Parcial -->
                                            <div class="col-12" runat="server" id="dvParcial" visible="false">
                                                <table id="tblParcial" class="table table-bordered table-hover">
                                                    <thead>
                                                    <tr>
                                                        <th>ID</th>
                                                        <th>Data</th>
                                                        <th>Editar</th>
                                                        <th>Status</th>
                                                        <th>Alterar Status</th>
                                                        <th>Representante</th>
                                                        <th>Responsável</th>
                                                        <th>E-mail</th>
                                                        <th>Telefone</th>
                                                    </tr>
                                                    </thead>
                                                    <tbody>
                                                    <asp:Repeater runat="server" ID="rptConsultaParcial" OnItemCommand="rptConsultaParcial_OnItemCommand">
                                                        <ItemTemplate>

                                                            <tr>
                                                                <td>
                                                                    <small><strong><%# DataBinder.Eval(Container.DataItem, "COD_ID")%></strong></small>
                                                                </td>
                                                                <td>
                                                                    <small><strong><%# String.Format("{0:dd/MM/yyyy}",DataBinder.Eval(Container.DataItem, "DTA_DATA"))%></strong></small>
                                                                </td>

                                                                <td>
                                                                <a href="" onclick="javascript:openPopupWindow('cad_estabelecimentos.aspx?id=<%#DataBinder.Eval(Container.DataItem,"COD_ID")%>', 'EstabelecimentosEdicao', 1024, 800); return false;" class="btn btn-sm btn-whitelabel1">
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
                                                                    <small><strong><%# DataBinder.Eval(Container.DataItem, "NOM_RAZAOSOCIAL")%></strong></small><br />
                                                                    <small><%# DataBinder.Eval(Container.DataItem, "NUM_DOCUMENTO")%></small>
                                                                </td>

                                                                <td>
                                                                    <small><%# DataBinder.Eval(Container.DataItem, "NOM_NOME")%></small> <small><%# DataBinder.Eval(Container.DataItem, "NOM_SOBRENOME")%></small>
                                                                </td>

                                                                <td>
                                                                    <small><%# DataBinder.Eval(Container.DataItem, "NOM_EMAIL")%></small>
                                                                </td>
                                                                <td>
                                                                    <small><%# DataBinder.Eval(Container.DataItem, "NOM_CELULAR")%></small><br />
                                                                    <small><%# DataBinder.Eval(Container.DataItem, "NUM_TELEFONE")%></small>
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
