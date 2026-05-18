<%@ Page Language="C#" AutoEventWireup="true" CodeFile="con_representantes_usuarios_adm.aspx.cs" Inherits="con_representantes_usuarios_adm" %>

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
            <div class="card card-success">

                <div class="card-header">
                <h3 class="card-title">Usuários</h3><br />
                <asp:Label runat="server" id="lblID"></asp:Label> - <asp:Label runat="server" id="lblNome"></asp:Label>

                </div>
                <div class="card-body">


                <div class="row">
                    <div class="col-sm-8">
                        <div class="form-group">
                            <label class="col-sm-12 col-form-label">Nome Usuário</label>
                            <asp:TextBox id="txtNome" runat="server" class="form-control" placeholder="Nome do Usuário"></asp:TextBox>
                        </div>
                    </div>
                    <div class="col-sm-4">
                        <div class="form-group">
                            <label class="col-sm-12 col-form-label">Ativo</label>
                            <asp:DropDownList runat="server" id="ddlAtivo" CssClass="form-control">
                                <asp:ListItem Value="S">Sim</asp:ListItem>
                                <asp:ListItem Value="N">Não</asp:ListItem>
                            </asp:DropDownList>

                        </div>
                    </div>

                </div>

                <div class="row">
                    <div class="col-sm-6">
                        <div class="form-group">
                            <label class="col-sm-12 col-form-label">E-mail de Login</label>
                            <asp:TextBox id="txtEmail" runat="server" class="form-control" placeholder="E-mail de Login"></asp:TextBox>
                        </div>
                    </div>
                    <div class="col-sm-6">
                        <div class="form-group">
                            <label class="col-sm-12 col-form-label">Confirme o E-mail </label>
                            <asp:TextBox id="txtEmailC" runat="server" class="form-control" 
                                placeholder="Confirme o E-mail do Responsável" 
                                ontextchanged="txtEmailC_TextChanged" AutoPostBack="True"></asp:TextBox>
                                <i class="fas fa-check text-success" runat="server" id="faVerificadoEmail" visible="false"></i>
                        </div>
                    </div>
                </div>


                <div class="row">
                    <div class="col-sm-6">
                        <div class="form-group">
                            <label class="col-sm-12 col-form-label">Senha</label>
                            <asp:TextBox id="txtSenha" runat="server" class="form-control" placeholder="Informe sua Senha"></asp:TextBox>
                        </div>
                    </div>
                    <div class="col-sm-6">
                        <div class="form-group">
                            <label class="col-sm-12 col-form-label">Confirme a Senha </label>
                            <asp:TextBox id="txtSenhaC" runat="server" class="form-control" 
                                placeholder="Confirme a Senha" ontextchanged="txtSenhaC_TextChanged" 
                                AutoPostBack="True"></asp:TextBox>
                            <i class="fas fa-check text-success" runat="server" id="faVerificadoSenha" visible="false"></i>
                        </div>
                    </div>
                </div>


                <div class="card-footer">
                        <asp:Button runat="server" ID="btnSalvar" CssClass="btn btn-whitelabel1" Text="Salvar" onclick="btnSalvar_Click" Visible="false"/>
                        <asp:Button runat="server" ID="btnCancelar" CssClass="btn btn-danger float-right" Text="Cancelar" onclick="btnCancelar_Click"/>
                </div>
            </div>
            </div>
        </div>
        </div>

        <div class="row">
            <div class="col-12">
                <div class="card card-success">
                    <div class="card-header">
                        <h3 class="card-title">Usuários</h3>
                    </div>
                    <div class="card-body">


                        <div class="row">
                            <div class="col-12">
                                <table id="example2" class="table table-bordered table-hover">
                                    <thead>
                                    <tr>
                                        <th>ID</th>
                                        <th>Nome</th>
                                        <th>E-Mail</th>
                                        <th>Status</th>
                                        <th>Ativar/Inativar</th>
                                        <th>Excluir</th>

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
                                                    <small><%# DataBinder.Eval(Container.DataItem, "NOM_NOME")%></small><br />
                                                </td>
                                                <td>
                                                    <small><%# DataBinder.Eval(Container.DataItem, "NOM_LOGIN")%></small>
                                                </td>

                                                <td>
                                                    <small><%# DataBinder.Eval(Container.DataItem, "NOM_FLG_ATIVO")%></small>
                                                </td>

                                                <td>
                                                    <asp:linkbutton ID="lkbAtivo" CssClass="btn btn-whitelabel1 btn-sm" commandname="Ativo" runat="server" text="Ativar/Inativar"  CommandArgument='<%# DataBinder.Eval(Container.DataItem, "COD_ID")%>' />
                                                </td>
                                                <td>
                                                    <asp:linkbutton ID="lkbExcluir" CssClass="btn btn-danger btn-sm" commandname="Excluir" runat="server" text="Excluir"  CommandArgument='<%# DataBinder.Eval(Container.DataItem, "COD_ID")%>' />
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


</body>
</html>
