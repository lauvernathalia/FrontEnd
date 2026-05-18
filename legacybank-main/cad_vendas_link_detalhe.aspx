<%@ Page Language="C#" AutoEventWireup="true" CodeFile="cad_vendas_link_detalhe.aspx.cs" Inherits="cad_vendas_link_detalhe" %>

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

        <div class="row">
            <div class="col-12">

                        <div class="card m-2">
                            <div class="card-body" style="overflow:auto; width: 100%; ">
                                <h3 class="modal-title text-black text-center"><b class="text-whitelabel1">VENDAS</b> DO CHECKOUT</h3>

                                <table id="Table3" class="table table-bordered table-hover">
                                    <thead>
                                    <tr>
                                        <th>Data</th>
                                        <th>Hora</th>
                                        <th>ID Transação</th>
                                        <th>Tipo</th>
                                        <th>Cliente</th>
                                        <th>Valor</th>
                                        <th>Status</th>
                                        <th>Produto</th>
                                        <th>Descrição</th>
                                        <th>Venda</th>
                                        <th>Transação</th>
                                        <th>Split</th>
                                    </tr>
                                    </thead>
                                    <tbody>
                                        <asp:Repeater runat="server" ID="rptConsulta">
                                            <ItemTemplate>
                                                <tr>
                                                    <td><small><%# String.Format("{0:dd/MM/yyyy}", DataBinder.Eval(Container.DataItem, "DTA_DATA"))%></small></td>
                                                    <td><small><%# String.Format("{0:HH:mm}", DataBinder.Eval(Container.DataItem, "DTA_DATA"))%></small></td>
                                                    <td><small><%# DataBinder.Eval(Container.DataItem, "NOM_CODE")%></small></td>

                                                    <td><small><%# DataBinder.Eval(Container.DataItem, "NOM_FLG_TIPO_PAGAMENTO")%></small></td>

                                                    <td><small><%# DataBinder.Eval(Container.DataItem, "NOM_NOME")%> <%# DataBinder.Eval(Container.DataItem, "NOM_SOBRENOME")%></small></td>
                                                    <td><small><%# String.Format("{0:c2}", DataBinder.Eval(Container.DataItem, "NUM_VALOR"))%></small></td>
                                                    <td><small><%# DataBinder.Eval(Container.DataItem, "FLG_STATUS")%></small></td>
                                                    <td><small><%# DataBinder.Eval(Container.DataItem, "NOM_REFERENCIA_PRODUTO")%></small></td>
                                                    <td><small><%# DataBinder.Eval(Container.DataItem, "NOM_DESCRICAO_PRODUTO")%></small></td>


                                                    <td>
                                                        <a href="" onclick="javascript:openPopupWindow('cad_vendas_link_detalhe_consulta.aspx?id=<%# Funcoes.Encrypt(DataBinder.Eval(Container.DataItem, "COD_ID").ToString()) %>', 'VendasLinkDetalheConsulta', 1024, 800); return false;" class="btn btn-whitelabel1 m-1">
                                                            Detalhe
                                                        </a>
                                                    
                                                    </td>
                                                    <td>
                                                        <a href="" onclick="javascript:openPopupWindow('cad_historico_vendas_detalhe.aspx?id=<%# Funcoes.Encrypt(DataBinder.Eval(Container.DataItem, "COD_ID_TRANSACOES").ToString()) %>', 'HistoricoVendaDetalhe', 1024, 800); return false;" class="btn btn-whitelabel1 m-1">
                                                            Detalhe
                                                        </a>                                                    
                                                    </td>
                                                    <td>
                                                    <a href="" onclick="javascript:openPopupWindow('cad_vendas_link_split.aspx?id=<%# Funcoes.Encrypt(DataBinder.Eval(Container.DataItem, "COD_ID").ToString()) %>&venda=<%# Funcoes.Encrypt(DataBinder.Eval(Container.DataItem, "COD_ID_VENDAS").ToString()) %>', 'VendasLinkSplit', 1024, 800); return false;" class="btn btn-whitelabel1 m-1">
                                                        Split
                                                    </a>

                                                    </td>

                                                </tr>
                                            </ItemTemplate>
                                        </asp:Repeater>

                                    </tbody>
                                </table>                                    

                            </div>

                            <div class="card-footer">
                                <asp:Button runat="server" ID="btnFechar" CssClass="btn btn-whitelabel1 float-right" Text="Fechar" onclick="btnFechar_Click"/>
                            </div>
                        </div>            

            
            </div>
        </div>
    </form>

<script type="text/javascript" src="../plugins/datatables/jquery.dataTables.js"></script>
<script type="text/javascript" src="../plugins/datatables-bs4/js/dataTables.bootstrap4.js"></script>

<script src="../plugins/inputmask/jquery.inputmask.bundle.js"></script>
<script src="../plugins/moment/moment.min.js"></script>


<script type="text/javascript">

$(document).ready(function () {
    var table = $('#tabConsulta').DataTable({
      "paging": false,
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
<script>

    function printDiv(divName) {
        var printContents = document.getElementById(divName).innerHTML;
        var originalContents = document.body.innerHTML;

        document.body.innerHTML = printContents;

        window.print();

        document.body.innerHTML = originalContents;
    }

</script>

</body>
</html>
