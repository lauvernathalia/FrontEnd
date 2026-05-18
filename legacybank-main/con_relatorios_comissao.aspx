<%@ Page Language="C#" AutoEventWireup="true" CodeFile="con_relatorios_comissao.aspx.cs" Inherits="con_relatorios_comissao" Async="true" EnableSessionState="ReadOnly" MaintainScrollPositionOnPostback="true" %>

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
                              <li class="breadcrumb-item active">Comissões</li>
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
                                    <label class="col-2 col-form-label">Ano</label>
						            <div class="col-2">
                                        <asp:TextBox runat="server" id="txtAno" CssClass="form-control" ></asp:TextBox>
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
                                        <asp:DropDownList runat="server" id="ddlAdquirentes" CssClass="form-control">
                                        </asp:DropDownList>
                                    </div>

                                </div>
                                
                              </div>

                              <div class="card-footer" style="overflow:auto; width: 100%; ">
                                    <asp:Button runat="server" ID="btnPesquisar" CssClass="btn btn-whitelabel1 btn-sm" Text="Pesquisar" onclick="btnPesquisar_Click"/>


                              </div>

                              </div>

                            </div>
                          </div>
                          <div class="row">
                            <div class="col-12">
                                <div class="card ">

                                  <div class="card-header bg-whitelabel1">
                                    <h3 class="card-title">Relatório das Comissões</h3>
                                  </div>
                                  <div class="card-body" style="overflow:auto; width: 100%; white-space: nowrap;">
                                    <div class="row">
                                        <div class="col-12" runat="server" id="dvConsulta" visible="false">

                                            <asp:GridView ID="gvConsulta" runat="server" 
                                                AutoGenerateColumns="false" BorderStyle="None" 
                                                CssClass="table table-striped nowrap table-bordered table-hover" 
                                                onrowdatabound="gvConsulta_RowDataBound">
                                                <AlternatingRowStyle BorderStyle="None" />
                                                <RowStyle BorderStyle="None" />
                                                <HeaderStyle BorderWidth="1" BorderStyle="Solid" />
                                                <Columns>

                                                  <asp:TemplateField HeaderText="Mês/Ano Ref.">
                                                    <ItemTemplate>
                                                    <small><strong><%# DataBinder.Eval(Container.DataItem, "NUM_MES_ANO_VENDA")%></strong></small>
                                                    </ItemTemplate>
                                                  </asp:TemplateField>
                                                  <asp:TemplateField HeaderText="Quantidade">
                                                    <ItemTemplate>
                                                    <small><strong><%# String.Format("{0:n0}",DataBinder.Eval(Container.DataItem, "NUM_QTDE"))%></strong></small>
                                                    </ItemTemplate>
                                                  </asp:TemplateField>

                                                  <asp:TemplateField HeaderText="TPV">
                                                    <ItemTemplate>
                                                        <small><%# String.Format("{0:c2}",DataBinder.Eval(Container.DataItem, "NUM_TPV"))%></small>
                                                    </ItemTemplate>
                                                  </asp:TemplateField>

                                                  <asp:TemplateField HeaderText="Comissão Total">
                                                    <ItemTemplate>
                                                        <small><%# String.Format("{0:c2}",DataBinder.Eval(Container.DataItem, "NUM_COMISSAO_TOTAL"))%></small>
                                                    </ItemTemplate>
                                                  </asp:TemplateField>


                                                  <asp:TemplateField HeaderText="Comissão Total Repasse">
                                                    <ItemTemplate>
                                                        <small><%# String.Format("{0:c2}",DataBinder.Eval(Container.DataItem, "NUM_COMISSAO"))%></small>
                                                    </ItemTemplate>
                                                  </asp:TemplateField>

                                                  <asp:TemplateField HeaderText="Comissão MKT">
                                                    <ItemTemplate>
                                                        <small><%# String.Format("{0:c2}",DataBinder.Eval(Container.DataItem, "NUM_COMISSAO_MKT"))%></small>
                                                    </ItemTemplate>
                                                  </asp:TemplateField>

                                                  <asp:TemplateField HeaderText="Comissão REP">
                                                    <ItemTemplate>
                                                        <small><%# String.Format("{0:c2}",DataBinder.Eval(Container.DataItem, "NUM_COMISSAO_REP"))%></small>
                                                    </ItemTemplate>
                                                  </asp:TemplateField>

                                                  <asp:TemplateField HeaderText="% Média Comissão">
                                                    <ItemTemplate>
                                                        <small><%# String.Format("{0:n2}",DataBinder.Eval(Container.DataItem, "NUM_PERCENTUAL_COMISSAO"))%></small>
                                                    </ItemTemplate>
                                                  </asp:TemplateField>

                                                  <asp:TemplateField HeaderText="Markup Médio">
                                                    <ItemTemplate>
                                                        <small><%# String.Format("{0:n2}",DataBinder.Eval(Container.DataItem, "NUM_MARKUP_MEDIO"))%>%</small>
                                                    </ItemTemplate>
                                                  </asp:TemplateField>



                                                </Columns>
                                                <PagerStyle BorderStyle="None" />
                                                <RowStyle BorderStyle="None" />
                                            </asp:GridView>




                                        </div>
                                        <div class="col-12" runat="server" id="dvConsultaParcial" visible="false">

                                            <asp:GridView ID="gvConsultaParcial" runat="server" 
                                                AutoGenerateColumns="false" BorderStyle="None" 
                                                CssClass="table table-striped nowrap table-bordered table-hover">
                                                <AlternatingRowStyle BorderStyle="None" />
                                                <RowStyle BorderStyle="None" />
                                                <HeaderStyle BorderWidth="1" BorderStyle="Solid" />
                                                <Columns>

                                                  <asp:TemplateField HeaderText="Mês/Ano Ref.">
                                                    <ItemTemplate>
                                                    <small><strong><%# DataBinder.Eval(Container.DataItem, "NUM_MES_ANO_VENDA")%></strong></small>
                                                    </ItemTemplate>
                                                  </asp:TemplateField>
                                                  <asp:TemplateField HeaderText="Quantidade">
                                                    <ItemTemplate>
                                                    <small><strong><%# String.Format("{0:n0}",DataBinder.Eval(Container.DataItem, "NUM_QTDE"))%></strong></small>
                                                    </ItemTemplate>
                                                  </asp:TemplateField>

                                                  <asp:TemplateField HeaderText="TPV">
                                                    <ItemTemplate>
                                                        <small><%# String.Format("{0:c2}",DataBinder.Eval(Container.DataItem, "NUM_TPV"))%></small>
                                                    </ItemTemplate>
                                                  </asp:TemplateField>

                                                  <asp:TemplateField HeaderText="Comissão">
                                                    <ItemTemplate>
                                                        <small><%# String.Format("{0:c2}",DataBinder.Eval(Container.DataItem, "NUM_COMISSAO"))%></small>
                                                    </ItemTemplate>
                                                  </asp:TemplateField>

                                                  <asp:TemplateField HeaderText="Markup Médio">
                                                    <ItemTemplate>
                                                        <small><%# String.Format("{0:n2}",DataBinder.Eval(Container.DataItem, "NUM_MARKUP_MEDIO"))%>%</small>
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
        $('[id$=gvConsultaParcial]').prepend($("<thead></thead>").append($('[id$=gvConsultaParcial]').find("tr:first"))).DataTable({
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


</body>
</html>
