<%@ Page Language="C#" AutoEventWireup="true" CodeFile="cad_pedidos_erp_pagamentos.aspx.cs" Inherits="cad_pedidos_erp_pagamentos" MaintainScrollPositionOnPostback="true" %>

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
                    <h3 class="card-title">PAGAMENTOS</h3>
                    </div>
                    <div class="card-body">

                        <div class="row">
                            <div class="col-sm-3">
                                <div class="form-group">
                                    <label class="col-sm-12 col-form-label">ID</label>
                                    <asp:Label id="lblID" runat="server" CssClass="form-control" enabled="false"></asp:Label>
                                </div>
                            </div>
                            <div class="col-sm-3">
                                <div class="form-group">
                                    <label class="col-sm-12 col-form-label">Pedido</label>
                                    <asp:Label id="lblPedido"  runat="server" CssClass="form-control" enabled="false"></asp:Label>
                                </div>
                            </div>

                            <div class="col-sm-3">
                                <div class="form-group">
                                    <label class="col-sm-12 col-form-label">Valor Total</label>
                                    <asp:Label id="lblValor"  runat="server" CssClass="form-control" enabled="false"></asp:Label>
                                </div>
                            </div>
                            <div class="col-sm-3">
                                <div class="form-group">
                                    <label class="col-sm-12 col-form-label">Parcelas</label>
                                    <div class="col-sm-12">
                                        <asp:Label id="lblParcelas"  runat="server" CssClass="form-control" enabled="false"></asp:Label>
                                    </div>
                                </div>
                            </div>
                        </div>

                        <div class="row">
                            <div class="col-sm-3">
                                <div class="form-group">
                                    <label class="col-sm-12 col-form-label">Cliente</label>
                                    <asp:Label id="lblCliente"  runat="server" CssClass="form-control"  enabled="false"></asp:Label>
                                </div>
                            </div>
                            <div class="col-sm-3">
                                <div class="form-group">
                                    <label class="col-sm-12 col-form-label">Documento</label>
                                    <asp:Label id="lblDocumento"  runat="server" CssClass="form-control"  enabled="false"></asp:Label>
                                </div>
                            </div>
                            <div class="col-sm-3">
                                <div class="form-group">
                                    <label class="col-sm-12 col-form-label">E-Mail</label>
                                    <asp:Label id="lblEmail"  runat="server" CssClass="form-control"  enabled="false"></asp:Label>
                                </div>
                            </div>

                            <div class="col-sm-3">
                                <div class="form-group">
                                    <label class="col-sm-12 col-form-label">ID Cliente</label>
                                    <asp:Label id="lblIDCliente"  runat="server" CssClass="form-control"  enabled="false"></asp:Label>
                                    <asp:TextBox id="txtToken"  runat="server" CssClass="form-control" Visible="false"></asp:TextBox>
                                    <asp:TextBox id="txtLogotipo"  runat="server" CssClass="form-control" Visible="false"></asp:TextBox>
                                    <asp:TextBox id="txtRetorno"  runat="server" CssClass="form-control" Visible="false"></asp:TextBox>
                                </div>
                            </div>

                        </div>

                    </div>
                    <div class="card-footer">
                            <asp:Button runat="server" ID="btnGerar" CssClass="btn btn-whitelabel1" Text="Gerar Pagamentos" onclick="btnGerar_Click"/>
                            <asp:Button runat="server" ID="btnEnviar" CssClass="btn btn-whitelabel1" Text="Enviar Pagamentos por E-mail" onclick="btnEnviar_Click"/>

                            <asp:Button runat="server" ID="btnCancelar" CssClass="btn btn-danger float-right" Text="Cancelar" onclick="btnCancelar_Click"/>
                    </div>
                </div>
            </div>
        </div>


        <div class="row">
            <div class="col-12">

                <div class="card">
                    <div class="card-header bg-whitelabel1">
                        <h3 class="card-title">PAGAMENTOS</h3>
                    </div>
                    <div class="card-body">
                        <table id="tbPagamentos" class="table table-bordered table-hover">
                            <thead>
                            <tr>
                                <th>ID</th>
                                <th>Vencimento</th>
                                <th>Valor</th>
                                <th>Gerado</th>
                                <th>Status</th>
                                <th>Boleto/Pix</th>
                                <th>Estornar/Excluir</th>
                            </tr>
                            </thead>
                            <tbody>
                            <asp:Repeater runat="server" ID="rptConsultaPagamentos" 
                                    onitemcommand="rptConsultaPagamentos_ItemCommand" >
                                <ItemTemplate>
                                    <tr>
                                        <td>
                                            <small><strong><%# DataBinder.Eval(Container.DataItem, "COD_ID")%></strong></small>
                                            <asp:TextBox id="txtid"  runat="server" CssClass="form-control" Text='<%# DataBinder.Eval(Container.DataItem, "COD_ID")%>' Visible="false"></asp:TextBox>
                                            <asp:TextBox id="txtvencimento"  runat="server" CssClass="form-control" Text='<%# String.Format("{0:dd/MM/yyyy}",DataBinder.Eval(Container.DataItem, "DTA_VENCIMENTO"))%>' Visible="false"></asp:TextBox>
                                            <asp:TextBox id="txtvalor"  runat="server" CssClass="form-control" Text='<%# String.Format("{0:n2}",DataBinder.Eval(Container.DataItem, "NUM_VALOR"))%>' Visible="false"></asp:TextBox>
                                        </td>
                                        <td>
                                            <small><strong><%# String.Format("{0:dd/MM/yyyy}",DataBinder.Eval(Container.DataItem, "DTA_VENCIMENTO"))%></strong></small>
                                        </td>
                                        <td>
                                            <small><%# String.Format("{0:n2}",DataBinder.Eval(Container.DataItem, "NUM_VALOR"))%></small>
                                        </td>
                                        <td>
                                            <small><%# DataBinder.Eval(Container.DataItem, "FLG_GERADO")%></small>
                                        </td>
                                        <td>
                                            <small><%# DataBinder.Eval(Container.DataItem, "NOM_FLG_STATUS")%></small>
                                        </td>
                                        <td>
                                            <a href='<%# DataBinder.Eval(Container.DataItem, "NOM_URL_BOLETO")%>' target="_blank" class="text-whitelabel1">
                                            <small><%# DataBinder.Eval(Container.DataItem, "NOM_URL_BOLETO")%></small>
                                            </a>
                                        
                                        </td>
                                        <td>
                                        
                                            <div class="btn-group">
                                                <button type="button" class="btn btn-default"><small>Ações</small></button>
                                                <button type="button" class="btn btn-default dropdown-toggle" data-toggle="dropdown">
                                                <span class="caret"></span>
                                                <span class="sr-only"><small>Ações</small></span>
                                                </button>
                                                <div class="dropdown-menu" role="menu">
                                                    <small><asp:linkbutton ID="lkbAtualizar" commandname="Atualizar" runat="server" Text="Atualizar" ToolTip="Atualizar"  CommandArgument='<%# DataBinder.Eval(Container.DataItem, "COD_ID")%>' class="dropdown-item"><i class="fas fa-recycle"></i>     Atualizar</asp:linkbutton></small>
                                                    <small><asp:linkbutton ID="lbkReenviar" commandname="Reenviar" runat="server" Text="Reenviar Cobrança" ToolTip="Reenviar Cobrança"  CommandArgument='<%# DataBinder.Eval(Container.DataItem, "COD_ID")%>' class="dropdown-item"><i class="fas fa-share-square"></i>     Reenviar Cobrança</asp:linkbutton></small>
                                                    <small><asp:linkbutton ID="lbkExcluir" commandname="Excluir" runat="server" Text="Excluir Cobrança" ToolTip="Excluir Cobrança"  CommandArgument='<%# DataBinder.Eval(Container.DataItem, "COD_ID")%>' class="dropdown-item"><i class="fas fa-share-square"></i>     Excluir Cobrança</asp:linkbutton></small>
                                                </div>
                                            </div>
                                        
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

    <script type="text/javascript" src="../plugins/moment/moment.min.js"></script>
    <script type="text/javascript" src="../plugins/moment/moment-with-locales.js"></script>
    <script type="text/javascript" src="../plugins/inputmask/min/jquery.inputmask.bundle.min.js"></script>
    <script type="text/javascript" src="../plugins/daterangepicker/daterangepicker.js"></script>
    <script type="text/javascript" src="../plugins/bootstrap-colorpicker/js/bootstrap-colorpicker.min.js"></script>
    <script type="text/javascript" src="../plugins/tempusdominus-bootstrap-4/js/tempusdominus-bootstrap-4.min.js"></script>

    <script type="text/javascript" src="../plugins/chart.js/Chart.min.js"></script>
    <script type="text/javascript" src="../dist/js/demo.js"></script>

    <script type="text/javascript" src="../plugins/summernote/summernote-bs4.min.js"></script>
    <script type="text/javascript">
        $(function () {
            // Summernote
            $('.textarea').summernote({ height: 300, })
        })
    </script>


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

</body>
</html>
