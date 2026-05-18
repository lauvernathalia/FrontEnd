<%@ Page Language="C#" AutoEventWireup="true" CodeFile="con_suporte.aspx.cs" Inherits="con_suporte" %>

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
                              <li class="breadcrumb-item"><a href="#">Suporte</a></li>
                              <li class="breadcrumb-item active">Solicitação Suporte</li>
                            </ol>
                          </div>
                        </div>




                        <div class="row">
                          <div class="col-12">


                            <div class="card">

                              <div class="card-header bg-whitelabel1">
                                <h3 class="card-title">SOLICITAÇÃO DE SUPORTE</h3>
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

                                    <label class="col-sm-2 col-form-label">Status</label>
                                    <div class="col-sm-3">
                                        <asp:DropDownList runat="server" id="ddlStatus" CssClass="form-control">
                                            <asp:ListItem Value="P">Pendentes</asp:ListItem>
                                            <asp:ListItem Value="R">Resolvidos</asp:ListItem>
                                            <asp:ListItem Value="C">Cancelados</asp:ListItem>
                                            <asp:ListItem Value=" ">Todos</asp:ListItem>
                                        </asp:DropDownList>
                                    </div>
                                </div>


                                <div class="form-group row col-12">
                                    <label class="col-sm-2 col-form-label">Descrição</label>
                                    <div class="col-sm-10">
                                        <asp:TextBox id="txtDescricao" runat="server" class="form-control"></asp:TextBox>
                                    </div>
                                </div>

                              </div>
                              <div class="card-footer" style="overflow:auto; width: 100%; ">


                                        <asp:Button runat="server" ID="btnPesquisar" CssClass="btn btn-whitelabel1" Text="Pesquisar" onclick="btnPesquisar_Click"/>
                                        <asp:Button runat="server" ID="btnNovo" CssClass="btn btn-whitelabel1 float-right" Text="(+) Nova Solicitação Suporte" onclick="btnNovo_Click"/>
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
                                <h3 class="card-title">SOLICITAÇÃO DE SUPORTE</h3>
                              </div>
                              <div class="card-body" style="overflow:auto; width: 100%;">



                                        <table id="tabConsulta" class="table table-bordered table-hover">
                                            <thead>
                                            <tr>
                                              <th><small>Data Abertura</small></th>
                                              <th><small>ID</small></th>
                                              <th><small>Editar</small></th>
                                              <th><small>Solicitante</small></th>
                                              <th><small>Protocolo</small></th>
                                              <th><small>Data Última Interação</small></th>
                                              <th><small>Motivo</small></th>
                                              <th><small>Suporte</small></th>
                                              <th><small>Status</small></th>
                                              <th><small>Tempo</small></th>
                                              <th><small>Interações</small></th>
                                              <th><small>Cancelar</small></th>
                                              <th><small>Reabrir</small></th>
                                            </tr>
                                            </thead>
                                            <tbody>
                                            <asp:Repeater runat="server" ID="rptConsulta" OnItemCommand="rptConsulta_OnItemCommand">
                                                <ItemTemplate>

                                                    <tr>

                                                      <td>
                                                          <small><%# String.Format("{0:dd/MM/yyyy}",DataBinder.Eval(Container.DataItem, "DTA_DATA_ABERTURA"))%></small>
                                                      </td>

                                                      <td>
                                                          <small><strong><%# DataBinder.Eval(Container.DataItem, "COD_ID")%></strong></small>
                                                          <asp:TextBox runat="server" ID="txtStatus" Text='<%# DataBinder.Eval(Container.DataItem, "FLG_STATUS")%>' Visible="false"></asp:TextBox>
                                                      </td>
                                                      <td class="text-center">
                                                        <a href="" onclick="javascript:openPopupWindow('cad_suporte.aspx?id=<%# Funcoes.Encrypt(DataBinder.Eval(Container.DataItem, "COD_ID").ToString())%>', 'SuporteEdicao', 1024, 800); return false;">
                                                            <i class="fas fa-edit text-center text-secondary"></i>
                                                        </a>
                                                      </td>

                                                      <td>
                                                          <small><strong><%# DataBinder.Eval(Container.DataItem, "NOM_RAZAOSOCIAL")%></strong></small>
                                                      </td>

                                                      <td>
                                                          <small><strong><%# DataBinder.Eval(Container.DataItem, "NOM_PROTOCOLO")%></strong></small>
                                                      </td>

                                                      <td>
                                                          <small><%# String.Format("{0:dd/MM/yyyy}", DataBinder.Eval(Container.DataItem, "DTA_DATA_ULTIMA_INTERACAO"))%></small>
                                                      </td>
                                                      <td>
                                                          <small><strong><%# DataBinder.Eval(Container.DataItem, "NOM_MOTIVO")%></strong></small>
                                                      </td>

                                                      <td>
                                                          <small><strong><%# DataBinder.Eval(Container.DataItem, "NOM_SUPORTE")%></strong></small>
                                                      </td>
                                                      <td>
                                                          <small><strong><%# DataBinder.Eval(Container.DataItem, "NOM_FLG_STATUS")%></strong></small>
                                                      </td>
                                                      <td>
                                                          <small><strong><%# DataBinder.Eval(Container.DataItem, "FLG_PRAZO")%></strong></small>
                                                      </td>
                                                      <td class="text-center">
                                                        <a href="" onclick="javascript:openPopupWindow('cad_suporte_interacoes.aspx?id=<%# Funcoes.Encrypt(DataBinder.Eval(Container.DataItem, "COD_ID").ToString())%>', 'SuporteInteracaoEdicao', 1024, 800); return false;" class="btn btn-app btn-sm" >
                                                            <span class="badge bg-warning"><%# DataBinder.Eval(Container.DataItem, "NUM_INTERACOES")%></span>
                                                            <i class="fas fa-eye text-center text-info"></i>
                                                        </a>
                                                      </td>
                                                      <td class="text-center">
                                                            <asp:linkbutton ID="lbkCancelar" commandname="Cancelar" runat="server" text="Cancelar" ToolTip="Cancelar"  CommandArgument='<%# DataBinder.Eval(Container.DataItem, "COD_ID")%>'><i class="fas fa-ban text-center text-danger"></i></asp:linkbutton>
                                                      </td>
                                                      <td class="text-center">
                                                            <asp:linkbutton ID="lbkReabrir" commandname="Reabrir" runat="server" text="Reabrir" ToolTip="Reabrir"  CommandArgument='<%# DataBinder.Eval(Container.DataItem, "COD_ID")%>'><i class="fas fa-undo-alt text-center text-success"></i></asp:linkbutton>
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
        var table = $('#tabConsulta').DataTable({
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
