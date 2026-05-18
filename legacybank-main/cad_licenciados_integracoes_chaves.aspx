<%@ Page Language="C#" AutoEventWireup="true" CodeFile="cad_licenciados_integracoes_chaves.aspx.cs" Inherits="cad_licenciados_integracoes_chaves" %>

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
                        <h3 class="card-title">LICENCIADOS CHAVES</h3>
                    </div>
                    <div class="card-body">
                        <h3><span class="float-center badge bg-whitelabel1">1. Dados da Integração</span></h3>

                        <div class="row">
                            <div class="col-sm-2">
                                <div class="form-group">
                                    <label>ID</label>
                                    <asp:TextBox runat="server" id="txtIDLicenciado" cssClass="form-control" Enabled="false"></asp:TextBox>
                                </div>
                            </div>
                            <div class="col-sm-2">
                                <div class="form-group">
                                    <label>Código</label>
                                    <asp:TextBox runat="server" id="txtCodigoLicenciado" cssClass="form-control" Enabled="false"></asp:TextBox>
                                </div>
                            </div>
                            <div class="col-sm-8">
                                <div class="form-group">
                                    <label>Licenciado</label>
                                    <asp:TextBox runat="server" id="txtLicenciado" cssClass="form-control" Enabled="false"></asp:TextBox>
                                </div>
                            </div>
                        </div>

                        <div class="row">

                            <div class="col-sm-2"  >
                                <div class="form-group">
                                <label>Integracao<strong class="text-danger">*</strong></label>
                                <asp:DropDownList runat="server" ID="ddlIntegracao" CssClass="form-control"></asp:DropDownList>
                                </div>
                            </div>

                            <div class="col-sm-2"  >
                                <div class="form-group">
                                <label>Chave própria?<strong class="text-danger">*</strong></label>
                                <asp:DropDownList runat="server" ID="ddlAtivo" CssClass="form-control">
                                    <asp:ListItem Text="Não" Value="N"></asp:ListItem>
                                    <asp:ListItem Text="Sim" Value="S"></asp:ListItem>
                                </asp:DropDownList>
                                </div>
                            </div>

                            <div class="col-sm-2"  >
                                <div class="form-group">
                                <label>Token<strong class="text-danger">*</strong></label>
                                <asp:TextBox runat="server" ID="txtToken" CssClass="form-control"></asp:TextBox> 
                                </div>
                            </div>
                            <div class="col-sm-2"  >
                                <div class="form-group">
                                <label>Key<strong class="text-danger">*</strong></label>
                                <asp:TextBox runat="server" ID="txtKey" CssClass="form-control" ></asp:TextBox> 
                                </div>
                            </div>
                            <div class="col-sm-2"  >
                                <div class="form-group">
                                <label>Secret<strong class="text-danger">*</strong></label>
                                <asp:TextBox runat="server" ID="txtSecret" CssClass="form-control" ></asp:TextBox> 
                                </div>
                            </div>
                            <div class="col-sm-2"  >
                                <div class="form-group">
                                <label>ID<strong class="text-danger">*</strong></label>
                                <asp:TextBox runat="server" ID="txtChaveID" CssClass="form-control" ></asp:TextBox> 
                                </div>
                            </div>


                            <div class="col-sm-6"  >
                                <div class="form-group">
                                <label>WEBHOOK ID<strong class="text-danger">*</strong></label>

                                <div class="input-group input-group-sm">
                                <asp:TextBox runat="server" ID="txtWebhookID" CssClass="form-control" ></asp:TextBox> 
                                    <span class="input-group-append">
                                        <asp:LinkButton runat="server" ID="lkbGerar" CssClass="btn btn-info btn-flat" onclick="lkbGerar_Click"><i class="fas fa-cogs"></i></asp:LinkButton>
                                    </span>
                                </div>

                                </div>
                            </div>


                        </div>


                    </div>

                    <div class="card-footer">
                        <asp:Button runat="server" ID="btnIncluir" CssClass="btn btn-whitelabel1" Text="Incluir" onclick="btnIncluir_Click"/>
                        <asp:Button runat="server" ID="btnCancelar" CssClass="btn btn-danger float-right" Text="Cancelar" onclick="btnCancelar_Click"/>
                    </div>
                </div>            
            
            </div>
        </div>



        <div class="row">
            <div class="col-12">

                <div class="card">
                    <div class="card-header bg-whitelabel1">
                        <h3 class="card-title">LISTA DE INTEGRAÇÕES</h3>
                    </div>
                    <div class="card-body  table-responsive" style="overflow:auto; width: 100%; word-wrap: break-word; ">
                        <table id="tbUsuario" class="table table-bordered table-hover ">
                            <thead>
                            <tr>
                                <th>ID</th>
                                <th>Integração</th>
                                <th>Chave Própria</th>
                                <th>Token</th>
                                <th>Key</th>
                                <th>Secret</th>
                                <th>ID</th>
                                <th>Webhook ID</th>
                            </tr>
                            </thead>
                            <tbody>
                            <asp:Repeater runat="server" ID="rptConsultaUsuario" 
                                    onitemcommand="rptConsultaUsuario_ItemCommand" >
                                <ItemTemplate>
                                    <tr>
                                        <td>
                                            <small><strong><%# DataBinder.Eval(Container.DataItem, "COD_ID")%></strong></small>
                                        </td>
                                        <td>
                                            <small><strong><%# DataBinder.Eval(Container.DataItem, "NOM_INTEGRACAO")%></strong></small>
                                        </td>
                                        <td>
                                            <small><%# DataBinder.Eval(Container.DataItem, "FLG_ATIVO")%></small>
                                        </td>
                                        <td style="max-width:150px;">
                                            <small><%# DataBinder.Eval(Container.DataItem, "NOM_TOKEN")%></small>
                                        </td>
                                        <td style="max-width:150px;">
                                            <small><%# DataBinder.Eval(Container.DataItem, "NOM_KEY")%></small>
                                        </td>
                                        <td style="max-width:150px;">
                                            <small><%# DataBinder.Eval(Container.DataItem, "NOM_SECRET")%></small>
                                        </td>

                                        <td style="max-width:150px;">
                                            <small><%# DataBinder.Eval(Container.DataItem, "NOM_ID")%></small>
                                        </td>
                                        <td style="max-width:150px;">
                                            <small><%# DataBinder.Eval(Container.DataItem, "NOM_WEBHOOK_ID")%></small>
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
