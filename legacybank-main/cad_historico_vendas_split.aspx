<%@ Page Language="C#" AutoEventWireup="true" CodeFile="cad_historico_vendas_split.aspx.cs" Inherits="cad_historico_vendas_split" %>

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
                        <h3 class="card-title">SPLIT</h3>
                    </div>
                    <div class="card-body">
                        <h3><span class="float-center badge bg-whitelabel1">Dados da Transação</span></h3>

                        <div class="row">
                            <div class="col-sm-2">
                                <div class="form-group">
                                    <label>ID</label>
                                    <asp:TextBox runat="server" id="txtID" cssClass="form-control" Enabled="false"></asp:TextBox>
                                </div>
                            </div>
                            <div class="col-sm-3">
                                <div class="form-group">
                                    <label>Estabelecimento</label>
                                    <asp:TextBox runat="server" id="txtRazaosocial" cssClass="form-control" Enabled="false"></asp:TextBox>
                                </div>
                            </div>
                            <div class="col-sm-3">
                                <div class="form-group">
                                    <label>Tipo</label>
                                    <asp:TextBox runat="server" id="txtTipo" cssClass="form-control" Enabled="false"></asp:TextBox>
                                </div>
                            </div>
                            <div class="col-sm-2">
                                <div class="form-group">
                                    <label>Data</label>
                                    <asp:TextBox runat="server" id="txtData" cssClass="form-control" Enabled="false"></asp:TextBox>
                                </div>
                            </div>
                            <div class="col-sm-2">
                                <div class="form-group">
                                    <label>Valor</label>
                                    <asp:TextBox runat="server" id="txtValorVenda" cssClass="form-control" Enabled="false"></asp:TextBox>
                                </div>
                            </div>
                        </div>
                        
                        <div class="row">
                            <div class="col-sm-12">
                                <div class="form-group">
                                    <label>ID Transação</label>
                                    <asp:TextBox runat="server" id="txtIDTransacao" cssClass="form-control" Enabled="false"></asp:TextBox>
                                </div>
                            </div>

                            <div class="col-sm-12">
                                <div class="form-group">
                                    <asp:TextBox runat="server" id="txtRetorno" cssClass="form-control" Enabled="true"></asp:TextBox>
                                </div>
                            </div>

                        </div>

                        <h3><span class="float-center badge bg-whitelabel1">Dados do Split</span></h3>

                        <div class="row" runat="server" id="dvDadosSplit">

                            <div class="col-sm-2">
                                <div class="form-group">
                                <label>Adquirente<strong class="text-danger">*</strong></label>
                                <asp:DropDownList runat="server" id="ddlAdquirentes" CssClass="form-control" 
                                        AutoPostBack="True" 
                                        onselectedindexchanged="ddlAdquirentes_SelectedIndexChanged">
                                </asp:DropDownList>
                                </div>
                            </div>

                            <div class="col-sm-4">
                                <div class="form-group">
                                <label>Favorecido<strong class="text-danger">*</strong></label>
                                <asp:DropDownList runat="server" id="ddlFavorecido" CssClass="form-control">
                                </asp:DropDownList>
                                </div>
                            </div>

                            <div class="col-sm-2">
                                <div class="form-group">
                                <label>Percentual<strong class="text-danger">*</strong></label>
                                <asp:TextBox runat="server" ID="txtPercentual" CssClass="form-control"></asp:TextBox>
                                </div>
                            </div>

                            <div class="col-sm-1">
                                <div class="form-group">
                                <label>Valor<strong class="text-danger">*</strong></label>
                                <asp:TextBox runat="server" ID="txtValor" CssClass="form-control"></asp:TextBox>
                                </div>
                            </div>

                            <asp:Button runat="server" ID="btnIncluirSplit" CssClass="btn btn-whitelabel1" Text="Incluir Split" onclick="btnIncluirSplit_Click"/>

                        </div>


                        <div class="row mt-3">
                            <div class="col-12">

                                <div class="card">
                                    <div class="card-header bg-whitelabel1">
                                        <h3 class="card-title">LISTA DE SPLIT</h3>
                                    </div>
                                    <div class="card-body" style="overflow:auto; width: 100%;">
                                        <table id="tbConsultaSplit" class="table table-bordered table-hover">
                                            <thead>
                                            <tr>
                                                <th>ID</th>
                                                <th>Data</th>
                                                <th>Favorecido</th>
                                                <th>Percentual</th>
                                                <th>Valor</th>
                                                <th>ID Split</th>
                                                <th>Processar/Cancelar</th>
                                                <th>Excluir</th>
                                            </tr>
                                            </thead>
                                            <tbody>
                                            <asp:Repeater runat="server" ID="rptConsultaSplit" 
                                                    onitemcommand="rptConsultaSplit_ItemCommand">
                                                <ItemTemplate>
                                                    <tr>
                                                        <td>
                                                            <small><strong><%# DataBinder.Eval(Container.DataItem, "COD_ID")%></strong></small>
                                                            <asp:TextBox runat="server" ID="txtid" Text='<%# DataBinder.Eval(Container.DataItem, "COD_ID")%>' Visible="false"></asp:TextBox>
                                                            <asp:TextBox runat="server" ID="txtcodesplit" Text='<%# DataBinder.Eval(Container.DataItem, "NOM_CODE_SPLIT")%>' Visible="false"></asp:TextBox>
                                                            <asp:TextBox runat="server" ID="txtidcadastro" Text='<%# DataBinder.Eval(Container.DataItem, "NOM_ID_CADASTRO")%>' Visible="false"></asp:TextBox>
                                                        </td>
                                                        <td>
                                                            <small><strong><%# DataBinder.Eval(Container.DataItem, "DTA_DATA")%></strong></small>
                                                        </td>

                                                        <td>
                                                            <small><%# DataBinder.Eval(Container.DataItem, "NOM_RAZAOSOCIAL")%></small>
                                                        </td>
                                                        <td>
                                                            <small><%# String.Format("{0:n2}",DataBinder.Eval(Container.DataItem, "NUM_PERCENTUAL"))%></small>
                                                        </td>
                                                        <td>
                                                            <small><%# String.Format("{0:n2}",DataBinder.Eval(Container.DataItem, "NUM_VALOR"))%></small>
                                                        </td>

                                                        <td>
                                                            <small><%# String.Format("{0:n2}",DataBinder.Eval(Container.DataItem, "NOM_CODE_SPLIT"))%></small>

                                                        </td>

                                                        <td>
                                                            <asp:linkbutton ID="lkbProcessar" CssClass="btn btn-sm btn-success" commandname="Processar" runat="server" text="Processar"  ToolTip="Processar" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "COD_ID")%>'><i class="fas fa-check"></i></asp:linkbutton>
                                                            <asp:linkbutton ID="lkbCancelar" CssClass="btn btn-sm btn-warning" commandname="Cancelar" runat="server" text="Cancelar"  ToolTip="Cancelar" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "COD_ID")%>'><i class="fas fa-ban"></i></asp:linkbutton>
                                                        </td>

                                                        <td>
                                                            <asp:linkbutton ID="lkbExcluir" CssClass="btn btn-sm btn-danger" commandname="Excluir" runat="server" text="Excluir"  ToolTip="Excluir" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "COD_ID")%>'><i class="fas fa-trash"></i></asp:linkbutton>
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


<script type="text/javascript">

$(document).ready(function () {
    var table = $('#tbConsultaSplit').DataTable({
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
