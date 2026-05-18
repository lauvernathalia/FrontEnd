<%@ Page Language="C#" AutoEventWireup="true" CodeFile="cad_suporte_interacoes.aspx.cs" Inherits="cad_suporte_interacoes" %>

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
                        <h3 class="card-title">SUPORTE INTERAÇÕES</h3>
                    </div>
                    <div class="card-body">
                        <h3><span class="float-center badge bg-whitelabel1">Dados da Solcitação de Suporte</span></h3>

                        <div class="row">
                            <div class="col-sm-2">
                                <div class="form-group">
                                    <label>ID</label>
                                    <asp:TextBox runat="server" id="txtID" cssClass="form-control" Enabled="false"></asp:TextBox>
                                </div>
                            </div>
                            <div class="col-sm-3">
                                <div class="form-group">
                                    <label>Data</label>
                                    <asp:TextBox runat="server" id="txtData" cssClass="form-control" Enabled="false"></asp:TextBox>
                                </div>
                            </div>
                            <div class="col-sm-3">
                                <div class="form-group">
                                    <label>Protocolo</label>
                                    <asp:TextBox runat="server" id="txtProtocolo" cssClass="form-control" Enabled="false"></asp:TextBox>
                                </div>
                            </div>
                            <div class="col-sm-4">
                                <div class="form-group">
                                    <label>Motivo</label>
                                    <asp:TextBox runat="server" id="txtMotivo" cssClass="form-control" Enabled="false"></asp:TextBox>
                                </div>
                            </div>
                        </div>

                        <div class="row">
                            <div class="col-sm-6">
                                <div class="form-group">
                                    <label>Suporte</label>
                                    <asp:TextBox runat="server" id="txtSuporte" cssClass="form-control" Enabled="false"></asp:TextBox>
                                </div>
                            </div>

                            <div class="col-sm-6">
                                <div class="form-group">
                                    <label>Solicitante</label>
                                    <asp:TextBox runat="server" id="txtSolicitante" cssClass="form-control" Enabled="false"></asp:TextBox>
                                </div>
                            </div>

                        </div>
                        
                        <div class="row">
                            <div class="col-sm-12">
                                <div class="card collapsed-card">
                                  <div class="card-header border-0">
                                    <h3 class="card-title">
                                      <i class="fas fa-comments mr-1"></i>
                                      Descrição da Solicitação de Suporte
                                    </h3>
                                    <div class="card-tools">
                                        <button type="button"
                                                class="btn btn-sm"
                                                data-card-widget="collapse"
                                                data-toggle="tooltip"
                                                title="Expandir/Recolher">
                                            <i class="fas fa-plus"></i>
                                        </button>
                                    </div>
                                  </div>
                                  <div class="card-body">
                                        <textarea runat="server" id="txtDescricaoSuporte" class="textarea" rows="5" Enabled="false"></textarea>
                                  </div>
                                </div>
                            </div>
                        </div>

                        <!-- Dados das Contas Bancárias -->

                        <h3><span class="float-center badge bg-whitelabel1">Nova Interação</span></h3>

                        <div class="row" runat="server" id="dvDados">
                            

                            <div class="col-sm-12">
                                <div class="form-group">
                                <label>Tipo<strong class="text-danger">*</strong></label>
                                <asp:DropDownList runat="server" id="ddlTipo" CssClass="form-control">
                                    <asp:ListItem Value="P">Pergunta</asp:ListItem>
                                    <asp:ListItem Value="R">Resposta</asp:ListItem>
                                    <asp:ListItem Value="S">Solução</asp:ListItem>
                                    <asp:ListItem Value="F">Finalização</asp:ListItem>
                                    <asp:ListItem Value="C">Cancelamento</asp:ListItem>
                                </asp:DropDownList>
                                </div>
                            </div>

                            <div class="col-sm-12">
                                <div class="form-group">
                                    <label class="col-sm-12 col-form-label">Descrição</label>
                                    <textarea runat="server" id="txtDescricao" class="textarea" rows="5"></textarea>
                                </div>
                            </div>

                            <asp:Button runat="server" ID="btnIncluir" CssClass="btn btn-whitelabel1" Text="Incluir" onclick="btnIncluir_Click"/>

                        </div>


                        <div class="row mt-3">
                            <div class="col-12">

                                <div class="card">
                                    <div class="card-header bg-whitelabel1">
                                        <h3 class="card-title">LISTA DE INTERAÇÕES</h3>
                                    </div>
                                    <div class="card-body" style="overflow:auto; width: 100%;">
                                        <table id="tbUsuario" class="table table-bordered table-hover">
                                            <thead>
                                            <tr>
                                                <th>ID</th>
                                                <th>Data</th>
                                                <th>Hora</th>
                                                <th>Responsável</th>
                                                <th>Tipo</th>
                                                <th>Descrição</th>
                                            </tr>
                                            </thead>
                                            <tbody>
                                            <asp:Repeater runat="server" ID="rptConsulta" 
                                                    onitemcommand="rptConsulta_ItemCommand">
                                                <ItemTemplate>
                                                    <tr>
                                                        <td>
                                                            <small><strong><%# DataBinder.Eval(Container.DataItem, "COD_ID")%></strong></small>
                                                        </td>
                                                        <td>
                                                            <small><%# String.Format("{0:dd/MM/yyyy}", DataBinder.Eval(Container.DataItem, "DTA_DATA"))%></small>
                                                        </td>


                                                        <td>
                                                            <small><%# String.Format("{0:HH:mm}", DataBinder.Eval(Container.DataItem, "DTA_DATA"))%></small>
                                                        </td>

                                                        <td>
                                                            <small><%# DataBinder.Eval(Container.DataItem, "NOM_RAZAOSOCIAL")%></small>
                                                        </td>

                                                        <td>
                                                            <small><%# DataBinder.Eval(Container.DataItem, "NOM_FLG_TIPO")%></small>
                                                        </td>

                                                        <td>
                                                            <small><%# DataBinder.Eval(Container.DataItem, "DES_INTERACAO")%></small>
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
                    <div class="card-footer">
                        <asp:Button runat="server" ID="btnFechar" CssClass="btn btn-danger float-right" Text="Fechar" onclick="btnFechar_Click"/>
                    </div>
                </div>

            </div>            
            
        </div>

    </form>

<script type="text/javascript" src="../plugins/datatables/jquery.dataTables.js"></script>
<script type="text/javascript" src="../plugins/datatables-bs4/js/dataTables.bootstrap4.js"></script>
<script src="../plugins/inputmask/jquery.inputmask.bundle.js"></script>
<script src="../plugins/moment/moment.min.js"></script>
<script type="text/javascript" src="../plugins/summernote/summernote-bs4.min.js"></script>

<script type="text/javascript">
    $(function () {
        // Summernote
        $('.textarea').summernote()
    })
</script>

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
