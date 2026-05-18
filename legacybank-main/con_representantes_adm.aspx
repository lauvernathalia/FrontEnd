<%@ Page Language="C#" AutoEventWireup="true" CodeFile="con_representantes_adm.aspx.cs" Inherits="con_representantes_adm" %>

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
                            <div class="card card-success">

                              <div class="card-header">
                                <h3 class="card-title">REPRESENTANTES</h3>
                              </div>
                              <div class="card-body">


                                <div class="form-group row col-12">
                                    <div class="col-sm-4">
                                        <label class="col-sm-2 col-form-label">Nome</label>
                                        <asp:TextBox id="txtFiltro" runat="server" class="form-control" placeholder="Nome"></asp:TextBox>
                                    </div>
                                    <div class="col-sm-4">
                                        <label class="col-sm-2 col-form-label">Ativo</label>
                                        <asp:DropDownList runat="server" id="ddlAtivo" CssClass="form-control">
                                            <asp:ListItem Value="S">Sim</asp:ListItem>
                                            <asp:ListItem Value="N">Não</asp:ListItem>
                                            <asp:ListItem Value=" ">Todos</asp:ListItem>
                                        </asp:DropDownList>
                                    </div>
                                </div>
                                <div class="form-group row col-12">

                                    <div class="col-sm-12">
                                        <asp:Button runat="server" ID="btnPesquisar" CssClass="btn btn-whitelabel1" Text="Pesquisar" onclick="btnPesquisar_Click"/>
                                        <asp:Button runat="server" ID="btnNovo" CssClass="btn btn-whitelabel1 float-right" Text="(+) Novo Representante" onclick="btnNovo_Click"/>
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
                                              <th>Editar</th>
                                              <th>Token</th>
                                              <th>Nome</th>
                                              <th>Documento</th>
                                              <th>Ativo</th>
                                              <th>Usuários</th>
                                              <th>Planos</th>
                                              <th>Estabelecimentos</th>
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
                                                        <a href="" onclick="javascript:openPopupWindow('cad_representantes_adm.aspx?id=<%#DataBinder.Eval(Container.DataItem,"COD_ID")%>', 'RepresentantesEdicao', 1024, 800); return false;" class="btn btn-sm btn-app">
                                                            <i class="fas fa-edit"></i> Editar
                                                        </a>
                                                      </td>

                                                      <td>
                                                          <small><strong><%# DataBinder.Eval(Container.DataItem, "NUM_TOKEN")%></strong></small>
                                                      </td>

                                                      <td>
                                                          <small><strong><%# DataBinder.Eval(Container.DataItem, "NOM_RAZAOSOCIAL")%></strong></small><br />
                                                      </td>
                                                      <td>
                                                          <small><%# DataBinder.Eval(Container.DataItem, "NUM_DOCUMENTO")%></small>
                                                      </td>

                                                      <td>
                                                          <small><%# DataBinder.Eval(Container.DataItem, "NOM_FLG_ATIVO")%></small>
                                                      </td>
                                                      <td>
                                                        <a href="" onclick="javascript:openPopupWindow('con_representantes_usuarios_adm.aspx?id=<%#DataBinder.Eval(Container.DataItem,"COD_ID")%>', 'RepresentantesUsuarios', 1024, 800); return false;" class="btn btn-sm btn-app">
                                                            <i class="fas fa-users"></i> Usuários
                                                        </a>
                                                      </td>
                                                      <td>
                                                        <a href="" onclick="javascript:openPopupWindow('con_representantes_planos_adm.aspx?id=<%#DataBinder.Eval(Container.DataItem,"COD_ID")%>', 'RepresentantesPlanos', 1024, 800); return false;" class="btn btn-sm btn-app">
                                                            <i class="fas fa-calculator"></i> Planos
                                                        </a>
                                                      </td>

                                                      <td>
                                                        <a href="" onclick="javascript:openPopupWindow('con_representantes_estabelecimentos_adm.aspx?id=<%#DataBinder.Eval(Container.DataItem,"COD_ID")%>', 'RepresentantesEstabelecimentos', 1024, 800); return false;" class="btn btn-sm btn-app">
                                                            <i class="fas fa-store"></i> Estabelecimentos
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
