<%@ Page Language="C#" AutoEventWireup="true" CodeFile="con_vendas_link.aspx.cs" Inherits="con_vendas_link" %>

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
                              <li class="breadcrumb-item"><a href="#">Minhas Vendas</a></li>
                              <li class="breadcrumb-item active">Vendas por Link</li>
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
                                </div>

                              </div>

                              <div class="card-footer">
                                <asp:Button runat="server" ID="btnPesquisar" CssClass="btn btn-whitelabel1 col-2" Text="Pesquisar" onclick="btnPesquisar_Click"/>
                                <asp:Button runat="server" ID="btnNovo" CssClass="btn btn-whitelabel1 col-3" Text="Nova venda por link" onclick="btnNovo_Click"/>
                                <asp:Button runat="server" ID="btnNovoAvancado" CssClass="btn btn-whitelabel1 col-3" Text="Nova venda por link Avançado" onclick="btnNovoAvancado_Click"/>
                                <asp:Button CssClass="btn btn-whitelabel1 col-2 float-right" runat="server" id="btnExportar" Text="Exportar Excel" onclick="btnExportar_Click" />
                                
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
                                    <h3 class="card-title">Vendas por Link</h3>
                                  </div>
                                  <div class="card-body" style="overflow:auto; width: 100%;>
                                    <div class="row">
                                        <div class="col-12">

                                            <table id="tblConsulta" class="table table-bordered table-hover">
                                                <thead>
                                                <tr>
                                                  <th>Editar</th>
                                                  <th>Editar Avançado</th>
                                                  <th>Data</th>
                                                  <th>Tipo</th>
                                                  <th>Código</th>
                                                  <th>Comprador</th>
                                                  <th>Adicional</th>
                                                  <th>Valor</th>
                                                  <th>Checkout</th>
                                                  <th>Qtde. Vendas</th>
                                                  <th>Detalhe</th>

                                                </tr>
                                                </thead>
                                                <tbody>
                                                <asp:Repeater runat="server" ID="rptConsulta" OnItemCommand="rptConsulta_OnItemCommand">
                                                    <ItemTemplate>

                                                        <tr>
                                                          <td>
                                                            <a href="" onclick="javascript:openPopupWindow('cad_link_pagamento.aspx?id=<%#DataBinder.Eval(Container.DataItem,"COD_ID")%>', 'VendasEdicao', 1024, 800); return false;" class="btn btn-sm btn-info">
                                                                <i class="fas fa-edit"></i>
                                                            </a>
                                                          </td>
                                                          <td>
                                                            <a href="" onclick="javascript:openPopupWindow('cad_vendas_link.aspx?id=<%#DataBinder.Eval(Container.DataItem,"COD_ID")%>', 'VendasEdicao', 1024, 800); return false;" class="btn btn-sm btn-info">
                                                                <i class="fas fa-edit"></i>
                                                            </a>
                                                          </td>

                                                          <td>
                                                              <small><strong><%# String.Format("{0:dd/MM/yyyy}",DataBinder.Eval(Container.DataItem, "DTA_DATA"))%> </strong></small>
                                                          </td>
                                                          <td>
                                                              <small><strong><%# DataBinder.Eval(Container.DataItem, "NOM_FLG_TIPO")%> </strong></small>
                                                          </td>


                                                          <td>
                                                              <small><strong><%# DataBinder.Eval(Container.DataItem, "COD_ID")%> </strong></small>
                                                          </td>

                                                          <td>
                                                              <small><strong><%# DataBinder.Eval(Container.DataItem, "NOM_NOME")%> </strong></small>
                                                          </td>

                                                          <td>
                                                              <small><strong><%# DataBinder.Eval(Container.DataItem, "NOM_CAMPO_01")%>: </strong></small><br />
                                                              <small><strong><%# DataBinder.Eval(Container.DataItem, "NOM_CONTEUDO_CAMPO_01")%> </strong></small>
                                                          </td>
                                                          <td>
                                                              <small><strong><%# String.Format("{0:n2}",DataBinder.Eval(Container.DataItem, "NUM_VALOR"))%> </strong></small>

                                                          </td>
                                                          <td>
                                                            <a href='https://<%# DataBinder.Eval(Container.DataItem, "NOM_ORIGEM")%><%# Funcoes.Encrypt(DataBinder.Eval(Container.DataItem, "COD_ID").ToString())%>'  target="_blank">
                                                                https://<%# DataBinder.Eval(Container.DataItem, "NOM_ORIGEM")%><%# Funcoes.Encrypt(DataBinder.Eval(Container.DataItem, "COD_ID").ToString())%>
                                                            </a>
                                                          </td>
                                                          <td>
                                                              <small><strong><%# String.Format("{0:n0}",DataBinder.Eval(Container.DataItem, "NUM_VENDAS"))%> </strong></small>
                                                          </td>
                                                          <td>
                                                            <a href="" onclick="javascript:openPopupWindow('cad_vendas_link_detalhe.aspx?id=<%# Funcoes.Encrypt(DataBinder.Eval(Container.DataItem, "COD_ID").ToString()) %>', 'VendasLinkDetalhe', 1024, 800); return false;" class="btn btn-whitelabel1 m-1">
                                                                Detalhe
                                                            </a>

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
