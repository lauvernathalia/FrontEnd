<%@ Page Language="C#" AutoEventWireup="true" CodeFile="con_cobrancas_gestao.aspx.cs" Inherits="con_cobrancas_gestao" %>

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
                              <li class="breadcrumb-item"><a href="#">Cobranças</a></li>
                              <li class="breadcrumb-item active">Gestão de Cobranças</li>
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
                                    <div class="col-sm-2">
                                        <div class="form-group">
                                            <label>Data Início</label>
                                            <div class="input-group date" id="datepickerIni" data-target-input="nearest">
								                <asp:TextBox ID="txtDataIni" runat="server" CssClass="form-control datetimepicker-input" data-target="#datepickerIni"></asp:TextBox>
                                                <div class="input-group-append" data-target="#datepickerIni" data-toggle="datetimepicker">
                                                    <div class="input-group-text"><i class="fa fa-calendar"></i></div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>

                                    <div class="col-sm-2">
                                        <div class="form-group">
                                            <label>Data Fim</label>
                                            <div class="input-group date" id="datepickerFim" data-target-input="nearest">
								                <asp:TextBox ID="txtDataFim" runat="server" CssClass="form-control datetimepicker-input" data-target="#datepickerFim"></asp:TextBox>
                                                <div class="input-group-append" data-target="#datepickerFim" data-toggle="datetimepicker">
                                                    <div class="input-group-text"><i class="fa fa-calendar"></i></div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>

                                    <div class="col-sm-3">
                                        <div class="form-group">
                                            <label>Tipo</label>
                                            <asp:DropDownList id="ddlTipo" runat="server" class="form-control" >
							                    <asp:ListItem Value=" ">Todos</asp:ListItem>
							                    <asp:ListItem Value="B">Boleto Bancário</asp:ListItem>
							                    <asp:ListItem Value="P">Pix QRCode</asp:ListItem>
							                    <asp:ListItem Value="C">Venda Digitada</asp:ListItem>
							                    <asp:ListItem Value="L">Link Pagamento</asp:ListItem>
							                    <asp:ListItem Value="N">Carnê</asp:ListItem>
							                    <asp:ListItem Value="A">Assinatura</asp:ListItem>
                                            </asp:DropDownList>                    
                                        </div>
                                    </div>

                                    <div class="col-sm-2">
                                        <div class="form-group">
                                            <label>Status</label>
                                            <asp:DropDownList id="ddlStatus" runat="server" class="form-control" >
							                    <asp:ListItem Value="A">Ativo</asp:ListItem>
							                    <asp:ListItem Value="C">Cancelado</asp:ListItem>
							                    <asp:ListItem Value=" ">Todos</asp:ListItem>
                                            </asp:DropDownList>                    
                                        </div>
                                    </div>


                                    <div class="col-sm-3">
                                        <div class="form-group">
                                            <label>Adquirente</label>
                                            <asp:DropDownList runat="server" ID="ddlAdquirentes" CssClass="form-control">
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
                          <div class="row">
                            <div class="col-12">
                                <div class="card">

                                  <div class="card-header bg-whitelabel1">
                                    <h3 class="card-title">Cobranças</h3>
                                  </div>
                                  <div class="card-body" style="overflow:auto; width: 100%;>
                                    <div class="row">
                                        <div class="col-12">



                                            <table id="tblConsulta" class="table table-bordered table-hover">
                                                <thead>
                                                <tr>
                                                  <th><small>ID</small></th>
                                                  <th><small>Data Criação</small></th>
                                                  <th><small>Tipo Cobrança</small></th>
                                                  <th><small>Tipo Transação</small></th>
                                                  <th><small>Cliente</small></th>
                                                  <th><small>Status</small></th>
                                                  <th><small>Checkout</small></th>
                                                  <th><small>No. Transações</small></th>
                                                  <th><small>Pagas</small></th>
                                                  <th><small>Dta.Último Pagto.</small></th>
                                                  <th><small>Pagar</small></th>
                                                  <th><small>Dta.Próximo Vencto.</small></th>
                                                  <th><small>Canceladas</small></th>

                                                  <th><small>Valor</small></th>
                                                  <th><small>Posição</small></th>
                                                  <th><small>Transações</small></th>
                                                  <th><small>Cancelar</small></th>
                                                </tr>
                                                </thead>
                                                <tbody>
                                                <asp:Repeater runat="server" ID="rptConsulta" OnItemCommand="rptConsulta_OnItemCommand" OnItemDataBound="rptConsulta_ItemDataBound">
                                                    <ItemTemplate>

                                                        <tr>
                                                          <td>
                                                            <small><%# DataBinder.Eval(Container.DataItem,"COD_ID") %></small>
                                                            <asp:TextBox runat="server" ID="txttipo" Text='<%# DataBinder.Eval(Container.DataItem, "FLG_TIPO")%>' Visible="false"></asp:TextBox>
                                                            <asp:TextBox runat="server" ID="txtid" Text='<%# DataBinder.Eval(Container.DataItem, "COD_ID")%>' Visible="false"></asp:TextBox>
                                                            <asp:TextBox runat="server" ID="txtorigem" Text='<%# DataBinder.Eval(Container.DataItem, "NOM_ORIGEM")%>' Visible="false"></asp:TextBox>

                                                          </td>
                                                          <td>
                                                              <small><%# String.Format("{0:dd/MM/yyyy}",DataBinder.Eval(Container.DataItem, "DTA_DATA"))%></small>
                                                          </td>
                                                          <td>
                                                              <small><%# DataBinder.Eval(Container.DataItem, "NOM_FLG_TIPO")%></small>
                                                          
                                                          </td>
                                                          <td>
                                                              <small><%# DataBinder.Eval(Container.DataItem, "NOM_FLG_LINK_PERMANENTE")%></small>
                                                          </td>
                                                         
                                                          <td>
                                                              <small><%# DataBinder.Eval(Container.DataItem, "NOM_NOME_COMPRADOR")%></small>
                                                          </td>

                                                          <td>
                                                              <small><%# DataBinder.Eval(Container.DataItem, "NOM_FLG_STATUS")%></small>
                                                          </td>

                                                          <td>
                                                            <small>
                                                            <a href="" runat="server" id="hrefCheckout" target="_blank" class="btn btn-sm btn-whitelabel1 m-1" >
                                                                <i class="fas fa-shopping-cart"></i>
                                                            </a>
                                                            </small>
                                                          </td>
                                                          
                                                          <td>
                                                              <small><%# DataBinder.Eval(Container.DataItem, "NUM_VENDAS")%></small>
                                                          </td>

                                                          <td>
                                                              <small><strong><%# String.Format("{0:n0}", DataBinder.Eval(Container.DataItem, "NUM_VENDAS_RECEBIDAS "))%></strong></small>
                                                          </td>

                                                          <td>
                                                              <small><strong><%# String.Format("{0:dd/MM/yyyy}", DataBinder.Eval(Container.DataItem, "DTA_ULTIMO_PAGAMENTO"))%> </strong></small>
                                                          </td>
                                                          <td>
                                                              <small><strong><%# String.Format("{0:n0}", DataBinder.Eval(Container.DataItem, "NUM_VENDAS_RECEBER "))%></strong></small>
                                                          </td>

                                                          <td>
                                                              <small><strong><%# String.Format("{0:dd/MM/yyyy}", DataBinder.Eval(Container.DataItem, "DTA_PROXIMO_PAGAMENTO"))%> </strong></small>
                                                          </td>

                                                          <td>
                                                              <small><strong><%# String.Format("{0:n0}", DataBinder.Eval(Container.DataItem, "NUM_VENDAS_CANCELADAS "))%></strong></small>
                                                          </td>


                                                          <td>
                                                              <small><%# String.Format("{0:n2}",DataBinder.Eval(Container.DataItem, "NUM_VALOR"))%></small>
                                                          </td>
                                                          <td>
                                                            <small><%# DataBinder.Eval(Container.DataItem, "FLG_ALERTA_PAGAMENTO")%></small>
                                                          </td>
                                                          <td>
                                                            <small><a href="" onclick="javascript:openPopupWindow('con_cobrancas_gestao_detalhe.aspx?id=<%# Funcoes.Encrypt(DataBinder.Eval(Container.DataItem, "COD_ID").ToString()) %>', 'CobrancasGestaoDetalhe', 1024, 800); return false;" class="btn btn-sm btn-whitelabel1 m-1">
                                                                <i class="fas fa-eye"></i>
                                                            </a></small>
                                                          </td>

                                                           <td>
                                                               <asp:linkbutton ID="lkbCancelar" CssClass="btn btn-sm btn-danger" commandname="Cancelar" runat="server" text="Cancelar"  ToolTip="Cancelar" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "COD_ID")%>'><i class="fas fa-ban"></i></asp:linkbutton>
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
