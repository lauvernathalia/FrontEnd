<%@ Page Language="C#" AutoEventWireup="true" CodeFile="cad_planos_importar.aspx.cs" Inherits="cad_planos_importar" %>

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
                        <h3 class="card-title">PLANOS IMPORTAR</h3>
                    </div>

                    <div class="form-group row col-12">
                        <div class="col-sm-3">
                            <label class="col-sm-12 col-form-label">Adquirente</label>
                            <asp:DropDownList runat="server" id="ddlAdquirentes" CssClass="form-control" 
                                AutoPostBack="True" 
                                onselectedindexchanged="ddlAdquirentes_SelectedIndexChanged">
                            </asp:DropDownList>
                        </div>
                    </div>

                    <div class="card" runat="server" id="dvAntecipado" visible="false">
                        <div class="card-body">

                            <div class="form-group row col-12">
                                <div class="col-sm-4">
                                    <label class="col-sm-12 col-form-label">Antecipado</label>
                                    <asp:DropDownList runat="server" id="ddlAntecipado" CssClass="form-control">
                                        <asp:ListItem Value="S">Sim</asp:ListItem>
                                        <asp:ListItem Value="N">Não</asp:ListItem>
                                    </asp:DropDownList>
                                </div>

                                <div class="col-sm-4">
                                    <label class="col-sm-12 col-form-label">Dias Liquidação</label>
                                    <asp:TextBox id="txtDias" runat="server" CssClass="form-control" placeholder="No. Dias para Liquidação" Visible="false"></asp:TextBox>
                                    <asp:DropDownList runat="server" id="ddlDias" CssClass="form-control">
                                        <asp:ListItem Value="0">D+0</asp:ListItem>
                                        <asp:ListItem Value="1">D+1</asp:ListItem>
                                        <asp:ListItem Value="2">D+2</asp:ListItem>
                                        <asp:ListItem Value="7">D+7</asp:ListItem>
                                        <asp:ListItem Value="14">D+14</asp:ListItem>
                                        <asp:ListItem Value="30">D+30</asp:ListItem>
                                    </asp:DropDownList>
                                </div>
                                <div class="col-sm-4">
                                    <label class="col-sm-12 col-form-label">Taxa Antecipação (%)</label>
                                    <asp:TextBox id="txtTaxaAntecipacao" runat="server" CssClass="form-control" placeholder="Percentual da Taxa de Antecipação"></asp:TextBox>
                                </div>
                            </div>

                        </div>
                    </div>

                    <div class="card">
                        <div class="card-body">
                            <div class="form-group">
                                <asp:TextBox runat="server" ID="txtJson" Text="" TextMode="MultiLine" Rows="5" CssClass="form-control" Visible="false"></asp:TextBox>
                            </div>

                            <div class="row">
                                <div class="col-6">
                                    <div class="form-group">
                                        <div class="form-check">
                                            <asp:CheckBox CssClass="form-check-input" runat="server" id="ckbTodos" 
                                                AutoPostBack="True" oncheckedchanged="ckbTodos_CheckedChanged"/>
                                            <label class="form-check-label">Clique aqui para marcar ou desmarcar todos os registros</label>
                                        </div>
                                    </div>
                                </div>
                                <div class="col-6">
                                    <asp:Button runat="server" ID="btnImportar" CssClass="btn btn-whitelabel1 float-right" 
                                        Text="Importar Dados Selecionados" onclick="btnImportar_Click"/>

                                    <asp:Button runat="server" ID="btnCarregarCadastros" CssClass="btn btn-whitelabel1 float-right" 
                                        Text="Carregar Dados" onclick="btnCarregarCadastros_Click"/>
                                </div>

                            </div>

                            <table id="tblPlanos" class="table table-bordered table-hover">
                                <thead>
                                <tr>
                                    <th class="style1">Importar</th>
                                    <th>ID</th>
                                    <th>Base</th>
                                    <th>Plano</th>
                                </tr>
                                </thead>
                                <tbody>
                                    <asp:ListView ID="lsvPlanos" runat="server" 
                                        OnItemCommand="lsvPlanos_ItemCommand">
                                        <ItemTemplate>
                                                <tr>
                                                    <td>
                                                        <asp:CheckBox runat="server" ID="ckbImportar" />
                                                    </td>
                                                    <td>
                                                        <small><asp:Label ID="Label1" runat="server" Text='<%# Eval("id") %>'></asp:Label></small>
                                                        <asp:TextBox runat="server" ID="txtID" Text='<%# Eval("id") %>' Visible="false"></asp:TextBox>
                                                    </td>

                                                    <td>
                                                        <small><asp:Label ID="Label2" runat="server" Text='<%# Eval("base") %>'></asp:Label></small>
                                                        <asp:TextBox runat="server" ID="txtBase" Text='<%# Eval("base") %>' Visible="false"></asp:TextBox>
                                                    </td>


                                                    <td>
                                                        <small><asp:Label ID="Label6" runat="server" Text='<%# Eval("plano") %>'></asp:Label></small>
                                                        <asp:TextBox runat="server" ID="txtPlano" Text='<%# Eval("plano") %>' Visible="false"></asp:TextBox>
                                                    </td>
                                                </tr>
                                        </ItemTemplate>
                                    </asp:ListView>
                                </tbody>
                            </table>


                        </div>
                        <div class="card-footer">
                            <asp:Button runat="server" ID="btnFechar" CssClass="btn btn-danger float-right" Text="Fechar" onclick="btnFechar_Click"/>
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
    var table = $('#tblPlanos').DataTable({
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
