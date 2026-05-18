<%@ Page Language="C#" AutoEventWireup="true" CodeFile="con_submenus.aspx.cs" Inherits="events_con_submenus" %>

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
                              <li class="breadcrumb-item active">Submenus</li>
                            </ol>
                          </div>
                        </div>




                        <div class="row">
                          <div class="col-12">
                            <div class="card">

                              <div class="card-header bg-whitelabel1">
                                <h3 class="card-title">SUBMENUS</h3>
                              </div>
                              <div class="card-body">


                                <div class="form-group row col-12">
                                    <label class="col-sm-1 col-form-label">Tipo</label>

                                    <div class="col-sm-3">
                                        <asp:DropDownList runat="server" id="ddlTipo" CssClass="form-control">
                                            <asp:ListItem Value="E">Estabelecimento</asp:ListItem>
                                            <asp:ListItem Value="R">Representante</asp:ListItem>
                                            <asp:ListItem Value="M">Marketplace</asp:ListItem>
                                            <asp:ListItem Value="A">Licenciado</asp:ListItem>
                                            <asp:ListItem Value="L">Administrador</asp:ListItem>
                                        </asp:DropDownList>
                                    </div>
                                    <label class="col-sm-1 col-form-label">Ativo</label>

                                    <div class="col-sm-2">
                                        <asp:DropDownList runat="server" id="ddlAtivo" CssClass="form-control">
                                            <asp:ListItem Value="S">Sim</asp:ListItem>
                                            <asp:ListItem Value="N">Não</asp:ListItem>
                                            <asp:ListItem Value=" ">Todos</asp:ListItem>
                                        </asp:DropDownList>
                                    </div>

                                    <div class="col-sm-5">
                                        <asp:Button runat="server" ID="btnPesquisar" CssClass="btn btn-whitelabel1" Text="Pesquisar" onclick="btnPesquisar_Click"/>
                                        <asp:Button runat="server" ID="btnNovo" CssClass="btn btn-whitelabel1 float-right" Text="(+) Novo Registro" onclick="btnNovo_Click"/>
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
                              <div class="card-footer">

                              </div>
                            </div>
                          </div>

                        <div class="row">
                          <div class="col-12">
                            <div class="card">

                              <div class="card-header bg-whitelabel1">
                                <h3 class="card-title">CADASTROS</h3>
                              </div>
                              <div class="card-body" style="overflow:auto; width: 100%; ">

                                <div class="row">
                                    <div class="col-12" runat="server" id="dvCompleta" visible="true">
                                        <table id="tblCompleta" class="table table-bordered table-hover">
                                            <thead>
                                            <tr>
                                              <th><small><strong>ID</strong></small></th>
                                              <th><small><strong>Editar</strong></small></th>
                                              <th><small><strong>Menu</strong></small></th>
                                              <th><small><strong>Submenu</strong></small></th>
                                              <th><small><strong>Ordem</strong></small></th>
                                              <th><small><strong>Tipo</strong></small></th>
                                              <th><small><strong>Ativo</strong></small></th>
                                              <th><small><strong>Ativar/Inativar</strong></small></th>
                                            </tr>
                                            </thead>
                                            <tbody>
                                            <asp:Repeater runat="server" ID="rptConsultaCompleta" OnItemCommand="rptConsultaCompleta_OnItemCommand">
                                                <ItemTemplate>

                                                    <tr>

                                                      <td>
                                                          <small><strong><%# DataBinder.Eval(Container.DataItem, "COD_ID")%></strong></small>

                                                          <asp:TextBox runat="server" id="txtid" Text='<%# DataBinder.Eval(Container.DataItem, "COD_ID")%>' visible="false"></asp:TextBox>
                                                          <asp:TextBox runat="server" id="txtativo" Text='<%# DataBinder.Eval(Container.DataItem, "FLG_ATIVO")%>' visible="false"></asp:TextBox>

                                                      </td>
                                                      <td>

                                                        <a href="" onclick="javascript:openPopupWindow('cad_submenus.aspx?id=<%# Funcoes.Encrypt(DataBinder.Eval(Container.DataItem,"COD_ID").ToString())%>', 'EditarSubmenus', 1024, 800); return false;" class="btn btn-sm btn-app">
                                                            <i class="fas fa-edit"></i> Editar
                                                        </a>
                                                      </td>


                                                      <td>
                                                          <small><strong><%# DataBinder.Eval(Container.DataItem, "NOM_MENU")%></strong></small>
                                                      </td>

                                                      <td>
                                                          <small><strong><%# DataBinder.Eval(Container.DataItem, "NOM_SUBMENU")%></strong></small>
                                                      </td>

                                                      <td>
                                                          <small><strong><%# DataBinder.Eval(Container.DataItem, "NUM_ORDEM")%></strong></small>
                                                      </td>
                                                      <td>
                                                          <small><strong><%# DataBinder.Eval(Container.DataItem, "FLG_TIPO")%></strong></small>
                                                      </td>
                                                      <td>
                                                          <small><strong><%# DataBinder.Eval(Container.DataItem, "FLG_ATIVO")%></strong></small>
                                                      </td>
                                                        <td>
                                                            <asp:linkbutton ID="lkbAtivar" CssClass="btn btn-whitelabel1 btn-sm" commandname="AtivarInativar" runat="server" text="Ativar/Inativar"  ToolTip="Ativar/Inativar" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "COD_ID")%>'></asp:linkbutton>
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



                    </div>
                </section>
            </div>
            <PORTAL:PAGEBOTTOM id="Pageheader3" title="Site Directory" runat="server" ModuleSource="rodapepadrao.ascx"></PORTAL:PAGEBOTTOM><!-- Fim Rodapé da Pagina -->
    
        </div>
    </form>

<script type="text/javascript" src="../plugins/datatables/jquery.dataTables.js"></script>
<script type="text/javascript" src="../plugins/datatables-bs4/js/dataTables.bootstrap4.js"></script>
<script type="text/javascript">

$(document).ready(function () {
    var table = $('#tblCompleta').DataTable({
      order: [[0, 'desc']],
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
