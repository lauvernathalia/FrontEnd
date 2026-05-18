<%@ Page Language="C#" AutoEventWireup="true" CodeFile="con_acesso_dashboard.aspx.cs" Inherits="con_acesso_dashboard" %>


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
                              <li class="breadcrumb-item"><a href="#">Minha Conta</a></li>
                              <li class="breadcrumb-item active">Acessos Dashboard</li>
                            </ol>
                          </div>
                        </div>




                        <div class="row">
                          <div class="col-12">
                            <div class="card card-success">

                              <div class="card-header">
                                <h3 class="card-title">Acessos Dashboard</h3>
                              </div>
                              <div class="card-body">


                                <div class="form-group row col-12">
                                    <div class="col-sm-4">
                                        <label class="col-sm-2 col-form-label">Tipo</label>
                                        <asp:DropDownList runat="server" ID="ddlTipo" CssClass="form-control" 
                                            AutoPostBack="True" onselectedindexchanged="ddlTipo_SelectedIndexChanged">
                                            <asp:ListItem Value="E" Text="Estabelecimentos"></asp:ListItem>
                                            <asp:ListItem Value="R" Text="Representantes"></asp:ListItem>
                                            <asp:ListItem Value="M" Text="Marketplaces"></asp:ListItem>
                                        </asp:DropDownList>
                                    </div>
                                </div>
                                <div class="form-group row col-12">

                                    <div class="col-sm-12">
                                        <asp:Button runat="server" ID="btnPesquisar" CssClass="btn btn-whitelabel1" Text="Pesquisar" onclick="btnPesquisar_Click"/>
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
                              <div class="card-footer" style="overflow:auto; width: 100%; ">


                                <div class="row">
                                    <div class="col-12">
                                        <table id="tabConsulta" class="table table-bordered table-hover">
                                            <thead>
                                            <tr>
                                              <th>ID</th>
                                              <th>Dashboard</th>
                                              <th>Acesso</th>
                                              <th>Liberar/Bloquear</th>
                                            </tr>
                                            </thead>
                                            <tbody>
                                            <asp:Repeater runat="server" ID="rptConsulta" OnItemCommand="rptConsulta_OnItemCommand">
                                                <ItemTemplate>
                                                    <tr>
                                                      <td>
                                                          <small><strong><%# DataBinder.Eval(Container.DataItem, "COD_ID")%></strong></small>
                                                          <asp:TextBox runat="server" ID="txtid" Text='<%# DataBinder.Eval(Container.DataItem, "COD_ID")%>' Visible="false"></asp:TextBox>
                                                          <asp:TextBox runat="server" ID="txtacesso" Text='<%# DataBinder.Eval(Container.DataItem, "FLG_ACESSO")%>' Visible="false"></asp:TextBox>
                                                      </td>
                                                      <td>
                                                          <small><%# DataBinder.Eval(Container.DataItem, "NOM_DASHBOARD_DIVISAO")%></small>
                                                      </td>
                                                      <td>
                                                          <small><%# DataBinder.Eval(Container.DataItem, "FLG_ACESSO")%></small>
                                                      </td>
                                                        <td>
                                                            <asp:linkbutton ID="lkbLiberar" CssClass="btn btn-whitelabel1 btn-sm" commandname="Liberar" runat="server" text="Liberar"  ToolTip="Liberar" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "COD_ID")%>'></asp:linkbutton>
                                                            <asp:linkbutton ID="lkbBloquear" CssClass="btn btn-danger btn-sm" commandname="Bloquear" runat="server" text="Bloquear"  ToolTip="Bloquear" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "COD_ID")%>'></asp:linkbutton>
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

$(document).ready(function () {
    var table = $('#tabConsulta').DataTable({
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
