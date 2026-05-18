<%@ Page Language="C#" AutoEventWireup="true" CodeFile="cad_gestao_aluguel.aspx.cs" Inherits="cad_gestao_aluguel" %>

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
</head>
<body class="hold-transition sidebar-mini layout-fixed">
    <form id="frmPrincipal" runat="server">
        <div class="row">
            <div class="col-12">
                <div class="card">
                    <div class="card-header bg-whitelabel1">
                    <h3 class="card-title">Gestão de Aluguel</h3><br />

                    </div>
                    <div class="card-body">

                        <div class="row">
                            <div class="col-sm-2">
                                <div class="form-group">
                                    <label>ID</label>
                                    <asp:TextBox runat="server" id="txtID" cssClass="form-control" Enabled="false"></asp:TextBox>
                                </div>
                            </div>
                            <div class="col-sm-10">
                                <div class="form-group">
                                    <label>Estabelecimento</label>
                                    <asp:DropDownList runat="server" id="ddlEstabelecimento" CssClass="form-control" >
                                    </asp:DropDownList>
                                </div>
                            </div>
                            <div class="col-sm-3">
                                <div class="form-group">
                                    <label>Faturamento Mínimo</label>
                                    <asp:TextBox runat="server" id="txtFaturamentoMinimo" cssClass="form-control" placeholder="0,00" Text="0,00"></asp:TextBox>
                                </div>
                            </div>
                            <div class="col-sm-2">
                                <div class="form-group">
                                    <label>Modelo</label>
                                    <asp:DropDownList runat="server" id="ddlModelo" CssClass="form-control" >
                                        <asp:ListItem Text="Aluguel Fixo: Independe da quantidade de equipamentos" Value="F"></asp:ListItem>
                                        <asp:ListItem Text="Aluguel Dinâmico: Multiplica pela quantidade de equipamentos cadastrados" Value="D"></asp:ListItem>
                                    </asp:DropDownList>
                                </div>
                            </div>
                            <div class="col-sm-2">
                                <div class="form-group">
                                    <label>Tipo</label>
                                    <asp:DropDownList runat="server" id="ddlTipo" CssClass="form-control" >
                                        <asp:ListItem Text="Mensal" Value="M"></asp:ListItem>
                                        <asp:ListItem Text="Fatura Única" Value="U"></asp:ListItem>
                                    </asp:DropDownList>
                                </div>
                            </div>

                            <div class="col-sm-3">
                                <div class="form-group">
                                    <label>Data Primeira Cobrança</label>
						            <div class="col-12">
                                        <div class="input-group date" id="datepickerPC" data-target-input="nearest">
								            <asp:TextBox ID="txtDataPrimeiraCobranca" runat="server" CssClass="form-control datetimepicker-input" data-target="#datepickerPC"></asp:TextBox>
                                            <div class="input-group-append" data-target="#datepickerPC" data-toggle="datetimepicker">
                                                <div class="input-group-text"><i class="fa fa-calendar"></i></div>
                                            </div>
                                        </div>
                                    </div>           
                                </div>
                            </div>
                            <div class="col-sm-2">
                                <div class="form-group">
                                    <label>Valor</label>
                                    <asp:TextBox runat="server" id="txtValor" cssClass="form-control" placeholder="0,00"  Text="0,00"></asp:TextBox>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="card-footer">
                                <asp:Button runat="server" ID="btnSalvar" CssClass="btn btn-whitelabel1" Text="Salvar" onclick="btnSalvar_Click"/>
                                <asp:Button runat="server" ID="btnCancelar" CssClass="btn  btn btn-whitelabel1 float-right" Text="Cancelar" onclick="btnCancelar_Click"/>
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
        $('#datepickerPC').datetimepicker({
            format: 'DD/MM/YYYY'
        });
    })
</script>        



</body>
</html>

