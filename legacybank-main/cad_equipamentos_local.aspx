<%@ Page Language="C#" AutoEventWireup="true" CodeFile="cad_equipamentos_local.aspx.cs" Inherits="cad_equipamentos_local" %>

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
                        <h3 class="card-title">EQUIPAMENTOS - LOCAL</h3>
                    </div>
                    <div class="card-body">
                        <div class="row">
                            <div class="col-sm-2">
                                <div class="form-group">
                                    <label>ID</label>
                                    <asp:TextBox runat="server" id="txtIDEquipamento" cssClass="form-control" Enabled="false"></asp:TextBox>
                                </div>
                            </div>
                            <div class="col-sm-4">
                                <div class="form-group">
                                    <label>Modelo</label>
                                    <asp:TextBox runat="server" id="txtModelo" cssClass="form-control" Enabled="false"></asp:TextBox>
                                </div>
                            </div>
                            <div class="col-sm-4">
                                <div class="form-group">
                                    <label>Serial</label>
                                    <asp:TextBox runat="server" id="txtSerial" cssClass="form-control" Enabled="false"></asp:TextBox>
                                </div>
                            </div>
                        </div>

                        <div class="row">
                            <div class="col-sm-2">
                                <div class="form-group">
                                    <label class="col-sm-12 col-form-label">Tipo Local</label>
                                    <asp:DropDownList runat="server" id="ddlTipoLocal" CssClass="form-control" 
                                        AutoPostBack="True" 
                                        onselectedindexchanged="ddlTipoLocal_SelectedIndexChanged">
                                        <asp:ListItem Value="A">Licenciado</asp:ListItem>
                                        <asp:ListItem Value="M">Marketplace</asp:ListItem>
                                        <asp:ListItem Value="R">Representante</asp:ListItem>
                                        <asp:ListItem Value="E">Estabelecimento</asp:ListItem>
                                    </asp:DropDownList>
                                </div>
                            </div>
                            <div class="col-sm-6">
                                <div class="form-group">
                                    <label class="col-sm-12 col-form-label">Localização</label>
                                    <asp:DropDownList runat="server" id="ddlLocalizacao" class="form-control"></asp:DropDownList>
                                </div>
                            </div>

                            <div class="col-sm-4">
                                <label class="col-sm-12 col-form-label">Data Início</label>
                                <div class="input-group date" id="datepickerIni" data-target-input="nearest">
								    <asp:TextBox ID="txtDataInicio" runat="server" CssClass="form-control datetimepicker-input" data-target="#datepickerIni"></asp:TextBox>
                                    <div class="input-group-append" data-target="#datepickerIni" data-toggle="datetimepicker">
                                        <div class="input-group-text"><i class="fa fa-calendar"></i></div>
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
                        <h3 class="card-title">LISTA DE CHIPS</h3>
                    </div>
                    <div class="card-body  table-responsive" style="overflow:auto; width: 100%; word-wrap: break-word; ">
                        <table id="tbUsuario" class="table table-bordered table-hover ">
                            <thead>
                            <tr>
                                <th>ID</th>
                                <th>Data Início</th>
                                <th>Tipo Local</th>
                                <th>Localização</th>
                                <th>Excluir</th>

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
                                            <small><strong><%# String.Format("{0:dd/MM/yyyy}",DataBinder.Eval(Container.DataItem, "DTA_DATA"))%></strong></small>
                                        </td>
                                        <td>
                                            <small><%# DataBinder.Eval(Container.DataItem, "NOM_FLG_TIPO")%></small>
                                        </td>

                                        <td>
                                            <small><%# DataBinder.Eval(Container.DataItem, "NOM_LOCALIZACAO")%></small>
                                        </td>

                                        <td>
                                            <asp:linkbutton ID="lkbExcluir" CssClass="btn btn-danger btn-sm" commandname="Excluir" runat="server" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "COD_ID")%>'><i class="fas fa-trash"></i></asp:linkbutton>
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

<script type="text/javascript" src="../plugins/moment/moment.min.js"></script>
<script type="text/javascript" src="../plugins/moment/moment-with-locales.js"></script>
<script type="text/javascript" src="../plugins/inputmask/min/jquery.inputmask.bundle.min.js"></script>
<script type="text/javascript" src="../plugins/daterangepicker/daterangepicker.js"></script>
<script type="text/javascript" src="../plugins/bootstrap-colorpicker/js/bootstrap-colorpicker.min.js"></script>
<script type="text/javascript" src="../plugins/tempusdominus-bootstrap-4/js/tempusdominus-bootstrap-4.min.js"></script>

<script type="text/javascript" src="../plugins/chart.js/Chart.min.js"></script>
<script type="text/javascript" src="../dist/js/demo.js"></script>

<script type="text/javascript">
    $(function () {

        //Date range picker
        $('#datepickerIni').datetimepicker({
            format: 'DD/MM/YYYY'
        });
    })
</script>        


</body>
</html>
