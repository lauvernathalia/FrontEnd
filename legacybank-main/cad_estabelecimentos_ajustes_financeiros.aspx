<%@ Page Language="C#" AutoEventWireup="true" CodeFile="cad_estabelecimentos_ajustes_financeiros.aspx.cs" Inherits="cad_estabelecimentos_ajustes_financeiros" %>

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
                        <h3 class="card-title">AJUSTES FINANCEIROS</h3>
                    </div>
                    <div class="card-body">
                        <h3><span class="float-center badge bg-whitelabel1">Dados do Estabelecimento</span></h3>

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
                                    <label>E-mail</label>
                                    <asp:TextBox runat="server" id="txtEmail" cssClass="form-control" Enabled="false"></asp:TextBox>
                                </div>
                            </div>
                            <div class="col-sm-2">
                                <div class="form-group">
                                    <label>Tipo</label>
                                    <asp:TextBox runat="server" id="txtTipo" cssClass="form-control" Enabled="false"></asp:TextBox>
                                </div>
                            </div>
                            <div class="col-sm-2">
                                <div class="form-group">
                                    <label>Documento</label>
                                    <asp:TextBox runat="server" id="txtDocumento" cssClass="form-control" Enabled="false"></asp:TextBox>
                                </div>
                            </div>
                        </div>
                        


                        <h3><span class="float-center badge bg-whitelabel1">Dados dos Ajustes Financeiros</span></h3>

                        <div class="row" runat="server" id="dvDadosAjustesFinanceiros">
					        
                            <div class="col-sm-2">
                                <div class="form-group">
                                <label>Data Início<strong class="text-danger">*</strong></label>
                                <div class="input-group date" id="datepickerIni" data-target-input="nearest">
							        <asp:TextBox ID="txtDataIni" runat="server" CssClass="form-control datetimepicker-input" data-target="#datepickerIni"></asp:TextBox>
                                    <div class="input-group-append" data-target="#datepickerIni" data-toggle="datetimepicker">
                                        <div class="input-group-text"><i class="fa fa-calendar"></i></div>
                                    </div>
                                </div>
                                </div>
                            </div>


                            <div class="col-sm-3">
                                <div class="form-group">
                                <label>Adquirente<strong class="text-danger">*</strong></label>
                                <asp:DropDownList runat="server" id="ddlAdquirentes" CssClass="form-control" 
                                        AutoPostBack="True" 
                                        onselectedindexchanged="ddlAdquirentes_SelectedIndexChanged">
                                </asp:DropDownList>
                                </div>
                            </div>

                            <div class="col-sm-3">
                                <div class="form-group">
                                <label>Valor Total (R$)<strong class="text-danger">*</strong></label>
                                <asp:TextBox runat="server" ID="txtValorTotal" CssClass="form-control"></asp:TextBox>
                                </div>
                            </div>


                            <div class="col-sm-2">
                                <div class="form-group">
                                <label>Tipo Taxa (%/R$)<strong class="text-danger">*</strong></label>
                                <asp:DropDownList runat="server" id="ddlTipo" CssClass="form-control">
                                    <asp:ListItem Value="P">Percentual</asp:ListItem>
                                    <asp:ListItem Value="V">Valor</asp:ListItem>
                                </asp:DropDownList>
                                </div>
                            </div>

                            <div class="col-sm-2">
                                <div class="form-group">
                                <label>Taxa (%/R$)<strong class="text-danger">*</strong></label>
                                <asp:TextBox runat="server" ID="txtTaxa" CssClass="form-control"></asp:TextBox>
                                </div>
                            </div>

                            <div class="col-sm-6">
                                <div class="form-group">
                                <label>Credor<strong class="text-danger">*</strong></label>
                                <asp:DropDownList runat="server" id="ddlCredor" CssClass="form-control">
                                </asp:DropDownList>
                                </div>
                            </div>


                            <div class="col-sm-6">
                                <div class="form-group">
                                <label>Devedor<strong class="text-danger">*</strong></label>
                                <asp:DropDownList runat="server" id="ddlDevedor" CssClass="form-control">
                                </asp:DropDownList>
                                </div>
                            </div>

                            <div class="col-sm-12">
                                <div class="form-group">
                                <label>Descrição<strong class="text-danger">*</strong></label>
                                <asp:TextBox runat="server" ID="txtDescricao" CssClass="form-control"></asp:TextBox>
                                </div>
                            </div>

                            <asp:Button runat="server" ID="btnIncluirAjustesFinanceiros" CssClass="btn btn-whitelabel1" Text="Incluir Ajuste Financeiro" onclick="btnIncluirAjustesFinanceiros_Click"/>

                        </div>


                        <div class="row mt-3">
                            <div class="col-12">

                                <div class="card">
                                    <div class="card-header bg-whitelabel1">
                                        <h3 class="card-title">LISTA DE AJUSTES FINANCEIROS</h3>
                                    </div>
                                    <div class="card-body" style="overflow:auto; width: 100%;">
                                        <table id="tbConsultaSplit" class="table table-bordered table-hover">
                                            <thead>
                                            <tr>
                                                <th><small>ID</small></th>
                                                <th><small>Data</small></th>
                                                <th><small>Data Início</small></th>
                                                <th><small>Status</small></th>
                                                <th><small>Adquirente</small></th>
                                                <th><small>Credor</small></th>
                                                <th><small>Devedor</small></th>
                                                <th><small>Valor Total</small></th>
                                                <th><small>Tipo Taxa (%/R$)</small></th>
                                                <th><small>Taxa (%/R$)</small></th>
                                                <th><small>Cancelar</small></th>
                                            </tr>
                                            </thead>
                                            <tbody>
                                            <asp:Repeater runat="server" ID="rptConsultaAjustesFinanceiros" 
                                                    onitemcommand="rptConsultaAjustesFinanceiros_ItemCommand">
                                                <ItemTemplate>
                                                    <tr>
                                                        <td>
                                                            <small><strong><%# DataBinder.Eval(Container.DataItem, "COD_ID")%></strong></small>
                                                            <asp:TextBox runat="server" ID="txtid" Text='<%# DataBinder.Eval(Container.DataItem, "COD_ID")%>' Visible="false"></asp:TextBox>
                                                        </td>
                                                        <td>
                                                            <small><strong><%# String.Format("{0:dd/MM/yyyy}",DataBinder.Eval(Container.DataItem, "DTA_DATA"))%></strong></small>
                                                        </td>
                                                        <td>
                                                            <small><strong><%# String.Format("{0:dd/MM/yyyy}", DataBinder.Eval(Container.DataItem, "DTA_DATA_INICIO"))%></strong></small>
                                                        </td>
                                                        <td>
                                                            <small><strong><%# DataBinder.Eval(Container.DataItem, "FLG_STATUS")%></strong></small>
                                                        </td>
                                                        <td>
                                                            <small><strong><%# DataBinder.Eval(Container.DataItem, "FLG_INTEGRACAO")%></strong></small>
                                                        </td>
                                                        <td>
                                                            <small><%# DataBinder.Eval(Container.DataItem, "NOM_RAZAOSOCIAL")%></small>
                                                        </td>

                                                        <td>
                                                            <small><%# DataBinder.Eval(Container.DataItem, "NOM_RAZAOSOCIAL_AJUSTES_FINANCEIROS")%></small>
                                                        </td>

                                                        <td>
                                                            <small><%# String.Format("{0:n2}",DataBinder.Eval(Container.DataItem, "NUM_VALOR"))%></small>
                                                        </td>

                                                        <td>
                                                            <small><%# DataBinder.Eval(Container.DataItem, "FLG_TIPO_AJUSTE")%></small>
                                                        </td>
                                                        <td>
                                                            <small><%# String.Format("{0:n2}",DataBinder.Eval(Container.DataItem, "NUM_VALOR_PERCENTUAL"))%></small>
                                                        </td>
                                                        <td>
                                                            <asp:linkbutton ID="lkbCancelar" CssClass="btn btn-sm btn-danger" commandname="Cancelar" runat="server" text="Cancelar"  ToolTip="Cancelar" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "COD_ID")%>'><i class="fas fa-ban"></i></asp:linkbutton>
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

<script type="text/javascript" src="../plugins/moment/moment.min.js"></script>
<script type="text/javascript" src="../plugins/moment/moment-with-locales.js"></script>
<script type="text/javascript" src="../plugins/inputmask/min/jquery.inputmask.bundle.min.js"></script>
<script type="text/javascript" src="../plugins/daterangepicker/daterangepicker.js"></script>
<script type="text/javascript" src="../plugins/bootstrap-colorpicker/js/bootstrap-colorpicker.min.js"></script>
<script type="text/javascript" src="../plugins/tempusdominus-bootstrap-4/js/tempusdominus-bootstrap-4.min.js"></script>

<script type="text/javascript" src="../plugins/chart.js/Chart.min.js"></script>
<script type="text/javascript" src="../dist/js/demo.js"></script>
<script type="text/javascript" src="../plugins/datatables/jquery.dataTables.js"></script>
<script type="text/javascript" src="../plugins/datatables-bs4/js/dataTables.bootstrap4.js"></script>
<script type="text/javascript" src="../plugins/inputmask/jquery.inputmask.bundle.js"></script>
<script type="text/javascript" src="../plugins/moment/moment.min.js"></script>

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
