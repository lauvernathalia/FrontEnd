<%@ Page Language="C#" AutoEventWireup="true" CodeFile="con_relatorios_tpv.aspx.cs" Inherits="con_relatorios_tpv"  Async="true" EnableSessionState="ReadOnly" MaintainScrollPositionOnPostback="true" %>

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
                              <li class="breadcrumb-item"><a href="#">Relatórios</a></li>
                              <li class="breadcrumb-item active">TPV</li>
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
                                    <label class="col-1 col-form-label">Mês</label>
						            <div class="col-1">
                                        <asp:DropDownList runat="server" ID="ddlMes" CssClass="form-control"></asp:DropDownList>
                                        <asp:TextBox runat="server" id="txtMes" class="form-control" Visible="false" ></asp:TextBox>
                                    </div>

                                    <label class="col-1 col-form-label">Ano</label>
						            <div class="col-1">
                                        <asp:DropDownList runat="server" ID="ddlAno" CssClass="form-control" />
                                        <asp:TextBox runat="server" id="txtAno" class="form-control" Visible="false"></asp:TextBox>
                                    </div>


                                    <label class="col-2 col-form-label">Marketplace</label>
						            <div class="col-2">
                                        <asp:DropDownList runat="server" id="ddlMarketplace" CssClass="form-control" ></asp:DropDownList>
                                    </div>

                                    <label class="col-2 col-form-label">Representante</label>
						            <div class="col-2">
                                        <asp:DropDownList runat="server" id="ddlRepresentante" CssClass="form-control" ></asp:DropDownList>
                                    </div>

                                </div>
                                <div class="form-group row col-12">
                                    <label class="col-2 col-form-label">Adquirente</label>
						            <div class="col-4">
                                        <asp:DropDownList runat="server" id="ddlAdquirente" CssClass="form-control" >
                                            <asp:ListItem Value=" " Text="Todos"></asp:ListItem>
                                            <asp:ListItem Value="Z" Text="Zoop"></asp:ListItem>
                                            <asp:ListItem Value="P" Text="Pagseguro"></asp:ListItem>
                                        </asp:DropDownList>
                                    </div>

                                </div>
                                
                              </div>

                              <div class="card-footer" style="overflow:auto; width: 100%; ">
                                    <asp:Button runat="server" ID="btnPesquisar" CssClass="btn btn-whitelabel1 btn-sm" Text="Pesquisar" onclick="btnPesquisar_Click"/>

                                <asp:Button runat="server" ID="btnImportarTexto" CssClass="btn btn-whitelabel1 float-right" Text="Importar Excel" onclick="btnImportarTexto_Click"/>

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
                                  <div class="card-body">
                                      <div class="row">
                                  
                                          <div class="col-sm-4 col-6">
                                            <div class="description-block border-right">
                                              <h5 class="description-header text-success"><asp:Label runat="server" ID="lblValorTotalGeral"></asp:Label></h5>
                                              <h5 class="description-header text-success"><asp:Label runat="server" ID="lblQtdeTotalGeral"></asp:Label></h5>
                                              <span class="description-text">TOTAL GERAL</span>
                                            </div>
                                          </div>
                                          <div class="col-sm-4 col-6">
                                            <div class="description-block border-right">
                                              <h5 class="description-header text-warning"><asp:Label runat="server" ID="lblValorTotalPendente"></asp:Label></h5>
                                              <h5 class="description-header text-warning"><asp:Label runat="server" ID="lblQtdeTotalPendente"></asp:Label></h5>
                                              <span class="description-text">PENDENTES</span>
                                            </div>
                                          </div>
                                          <div class="col-sm-4 col-6">
                                            <div class="description-block border-right">
                                              <h5 class="description-header text-danger"><asp:Label runat="server" ID="lblValorTotalCancelada"></asp:Label></h5>
                                              <h5 class="description-header text-danger"><asp:Label runat="server" ID="lblQtdeTotalCancelada"></asp:Label></h5>
                                              <span class="description-text">CANCELADAS</span>
                                            </div>
                                          </div>

                                      </div>
                                  </div>
                                </div>
                            </div>
                          </div>


                          <div class="row">
                            <div class="col-12">
                                <div class="card">

                                  <div class="card-header bg-whitelabel1">
                                    <h3 class="card-title">Relatório TPV</h3>
                                  </div>
                                  <div class="card-body" style="overflow:auto; width: 100%; ">
                                    <div class="row">
                                        <div class="col-12">

                                        <asp:GridView ID="gvConsulta" runat="server" 
                                            AutoGenerateColumns="false" BorderStyle="None" 
                                            CssClass="table table-striped nowrap table-bordered table-hover">
                                            <AlternatingRowStyle BorderStyle="None" />
                                            <RowStyle BorderStyle="None" />
                                            <HeaderStyle BorderWidth="1" BorderStyle="Solid" />
                                            <Columns>

                                              <asp:TemplateField HeaderText="Estabelecimento">
                                                <ItemTemplate>
                                                <small><strong><%# DataBinder.Eval(Container.DataItem, "NOM_RAZAOSOCIAL")%></strong></small>
                                                </ItemTemplate>
                                              </asp:TemplateField>

                                              <asp:TemplateField HeaderText="ID Adquirente">
                                                <ItemTemplate>
                                                <small><strong><%# DataBinder.Eval(Container.DataItem, "COD_ID_PAGSEGURO")%></strong></small>
                                                <asp:TextBox runat="server" ID="txtIDAdquirente" Text='<%# DataBinder.Eval(Container.DataItem, "COD_ID_PAGSEGURO")%>' Visible="false"></asp:TextBox>
                                                </ItemTemplate>
                                              </asp:TemplateField>

                                              <asp:TemplateField HeaderText="Total Mês" HeaderStyle-HorizontalAlign="Right">
                                                <ItemTemplate>
                                                <small><%# String.Format("{0:c2}",DataBinder.Eval(Container.DataItem, "NUM_VALOR_MES"))%></small>
                                                </ItemTemplate>
                                              </asp:TemplateField>

                                              <asp:TemplateField HeaderText="Total Adquirente" HeaderStyle-HorizontalAlign="Right">
                                                <ItemTemplate>
                                                    <small><asp:Label runat="server" ID="lblTotalAdquirente" Text="0,00"></asp:Label></small>
                                                </ItemTemplate>
                                              </asp:TemplateField>


                                              <asp:TemplateField HeaderText="Desempenho">
                                                <ItemTemplate>
                                                    <small><span class='text-<%# DataBinder.Eval(Container.DataItem, "FLG_COR_PERCENTUAL")%>'><i class='fas fa-<%# DataBinder.Eval(Container.DataItem, "FLG_ICONE_PERCENTUAL")%>'></i> <%# String.Format("{0:n2}",DataBinder.Eval(Container.DataItem, "NUM_PERCENTUAL"))%>%</span></small>
                                                </ItemTemplate>
                                              </asp:TemplateField>


                                              <asp:TemplateField HeaderText="Total Mês (-1)"  HeaderStyle-HorizontalAlign="Right">
                                                <ItemTemplate>
                                                    <small><%# String.Format("{0:c2}",DataBinder.Eval(Container.DataItem, "NUM_VALOR_MES_01"))%></small>
                                                </ItemTemplate>
                                              </asp:TemplateField>

                                              <asp:TemplateField HeaderText="Total Mês (-2)"  HeaderStyle-HorizontalAlign="Right">
                                                <ItemTemplate>
                                                    <small><%# String.Format("{0:c2}",DataBinder.Eval(Container.DataItem, "NUM_VALOR_MES_02"))%></small>
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
