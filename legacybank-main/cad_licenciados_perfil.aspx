<%@ Page Language="C#" AutoEventWireup="true" CodeFile="cad_licenciados_perfil.aspx.cs" Inherits="cad_licenciados_perfil" MaintainScrollPositionOnPostback="true" %>

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

                <div class="card">
                    <div class="card-header bg-whitelabel1">
                        <h3 class="card-title">LICENCIADO PERFIL</h3>
                    </div>
                    <div class="card-body">
                        <h3><span class="float-center badge bg-whitelabel1">1. Perfil do Licenciado</span></h3>

                        <div class="row">
                            <div class="col-sm-2">
                                <div class="form-group">
                                    <label>ID</label>
                                    <asp:TextBox runat="server" id="txtID" cssClass="form-control" Enabled="false"></asp:TextBox>
                                </div>
                            </div>
                            <div class="col-sm-10">
                                <div class="form-group">
                                    <label>Licenciado</label>
                                    <asp:TextBox runat="server" id="txtRazaosocial" cssClass="form-control" Enabled="false"></asp:TextBox>
                                </div>
                            </div>
                        </div>

                        <div class="row">
                            <div class="col-sm-6"  >
                                <div class="form-group">
                                    <label>Perfil<strong class="text-danger">*</strong></label>
                                    <asp:DropDownList runat="server" id="ddlPerfil" CssClass="form-control">
                                    </asp:DropDownList>
                                
                                </div>
                            </div>
                        </div>


                    </div>

                    <div class="card-footer">
                        <asp:Button runat="server" ID="btnSalvar" CssClass="btn btn-whitelabel1" Text="Salvar" onclick="btnSalvar_Click"/>
                        <asp:Button runat="server" ID="btnLiberarTodos" CssClass="btn btn-whitelabel1" Text="Liberar Todos" onclick="btnLiberarTodos_Click"/>
                        <asp:Button runat="server" ID="btnCancelar" CssClass="btn btn-danger float-right" Text="Cancelar" onclick="btnCancelar_Click"/>
                    </div>
                </div>            
            
            </div>
        </div>



        <div class="row">
            <div class="col-12">

                <div class="card">
                    <div class="card-header bg-whitelabel1">
                        <h3 class="card-title">LISTA DE ACESSOS</h3>
                    </div>
                    <div class="card-body">

                        <table id="tbAcessos" class="table table-bordered table-hover">
                            <thead>
                            <tr>
                                <th>Menu</th>
                                <th>Opção</th>
                                <th>Acesso</th>
                                <th>Liberar/Bloquear</th>
                            </tr>
                            </thead>
                            <tbody>
                            <asp:Repeater runat="server" ID="rptConsultaAcessos" 
                                    onitemcommand="rptConsultaAcessos_ItemCommand">
                                <ItemTemplate>
                                    <tr>
                                        <td>
                                            <asp:TextBox runat="server" id="txtidperfil" Text='<%# DataBinder.Eval(Container.DataItem, "COD_ID_PERFIL")%>' visible="false"></asp:TextBox>
                                            <asp:TextBox runat="server" id="txtidsubmenu" Text='<%# DataBinder.Eval(Container.DataItem, "COD_ID_SUBMENUS")%>' visible="false"></asp:TextBox>
                                            <asp:TextBox runat="server" id="txtidacesso" Text='<%# DataBinder.Eval(Container.DataItem, "FLG_ACESSO")%>' visible="false"></asp:TextBox>

                                            <small><strong><%# DataBinder.Eval(Container.DataItem, "NOM_MENU")%></strong></small>
                                        </td>
                                        <td>
                                            <small><strong><%# DataBinder.Eval(Container.DataItem, "NOM_SUBMENU")%></strong></small>
                                        </td>
                                        <td>
                                            <small><strong><%# DataBinder.Eval(Container.DataItem, "NOM_FLG_ACESSO")%></strong></small>
                                        </td>
                                        <td>
                                            <asp:linkbutton ID="lkbLiberar" CssClass="btn btn-whitelabel1 btn-sm" commandname="Liberar" runat="server" text="Liberar"  ToolTip="Liberar" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "COD_ID_SUBMENUS")%>'></asp:linkbutton>
                                            <asp:linkbutton ID="lkbBloquear" CssClass="btn btn-danger btn-sm" commandname="Bloquear" runat="server" text="Bloquear"  ToolTip="Bloquear" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "COD_ID_SUBMENUS")%>'></asp:linkbutton>
                                        </td>
                                    </tr>
                                </ItemTemplate>
                            </asp:Repeater>

                            </tbody>
                        </table>

                    </div>
                    <div class="card-footer">
                        <asp:Button runat="server" ID="btnAtualizar" CssClass="btn btn-whitelabel1" Text="Atualizar" onclick="btnAtualizar_Click"/>
                    </div>
                </div>            
            
            </div>
        </div>

    </form>

<script type="text/javascript" src="../plugins/datatables/jquery.dataTables.js"></script>
<script type="text/javascript" src="../plugins/datatables-bs4/js/dataTables.bootstrap4.js"></script>

<script src="../plugins/inputmask/jquery.inputmask.bundle.js"></script>
<script src="../plugins/moment/moment.min.js"></script>

<script>
    $(function () {

        //Datemask dd/mm/yyyy

        $('#txtNascimento').inputmask('99/99/9999')
        $('#txtDataAbertura').inputmask('99/99/9999')
        $('#txtCPF').inputmask('999.999.999-99')
        $('#txtCNPJ').inputmask('99.999.999/9999-99')
        $('#txtCelular').inputmask('(99) 99999-9999')
        $('#txtTelefoneEmpresa').inputmask('(99) 99999-9999')
        $('#txtCEP').inputmask('99999-999')

    })
</script>

<script type="text/javascript">

$(document).ready(function () {
    var table = $('#tbUsuario').DataTable({
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


</body>
</html>
