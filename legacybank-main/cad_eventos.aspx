<%@ Page Language="C#" AutoEventWireup="true" CodeFile="cad_eventos.aspx.cs" Inherits="cad_eventos" %>

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
                <h3 class="card-title">Eventos</h3><br />

                </div>
                <div class="card-body">




                <div class="row">
                    <div class="col-sm-2">
                        <div class="form-group">
                            <label class="col-sm-12 col-form-label">ID</label>
                            <asp:Label runat="server" id="lblID" class="form-control"></asp:Label>
                        </div>
                    </div>
                    <div class="col-sm-4">
                        <div class="form-group">
                            <label class="col-sm-12 col-form-label">Nome Evento</label>
                            <asp:TextBox runat="server" id="txtNomeEvento" class="form-control" placeholder="Nome do Evento"></asp:TextBox>
                        </div>
                    </div>
                    <div class="col-sm-6">
                        <div class="form-group">
                            <label class="col-sm-12 col-form-label">Descrição</label>
                            <asp:TextBox id="txtDescricao" runat="server" class="form-control" placeholder="Descrição do Evento"></asp:TextBox>
                        </div>
                    </div>
                </div>

                <div class="row">

                    <div class="col-sm-2">
                        <div class="form-group">
                            <label class="col-sm-12 col-form-label">Realização</label>
                            <asp:DropDownList runat="server" id="ddlRealizacao" CssClass="form-control" 
                                AutoPostBack="True" 
                                onselectedindexchanged="ddlRealizacao_SelectedIndexChanged">
                                <asp:ListItem Value="A">Licenciado</asp:ListItem>
                                <asp:ListItem Value="M">Marketplace</asp:ListItem>
                                <asp:ListItem Value="R">Representante</asp:ListItem>
                                <asp:ListItem Value="E">Estabelecimento</asp:ListItem>
                            </asp:DropDownList>
                        </div>
                    </div>
                    <div class="col-sm-8">
                        <div class="form-group">
                            <label class="col-sm-12 col-form-label">Realizador</label>
                            <asp:DropDownList runat="server" id="ddlRealizador" class="form-control"></asp:DropDownList>
                        </div>
                    </div>
                </div>

                <div class="form-group row col-12">
                    <label class="col-2 col-form-label">Data Início</label>
					<div class="col-2">
                        <div class="input-group date" id="datepickerIni" data-target-input="nearest">
							<asp:TextBox ID="txtDataIni" runat="server" CssClass="form-control datetimepicker-input" data-target="#datepickerIni"></asp:TextBox>
                            <div class="input-group-append" data-target="#datepickerIni" data-toggle="datetimepicker">
                                <div class="input-group-text"><i class="fa fa-calendar"></i></div>
                            </div>
                        </div>
                    </div>

                    <label class="col-2 col-form-label">Data Fim</label>
					<div class="col-2">
                        <div class="input-group date" id="datepickerFim" data-target-input="nearest">
							<asp:TextBox ID="txtDataFim" runat="server" CssClass="form-control datetimepicker-input" data-target="#datepickerFim"></asp:TextBox>
                            <div class="input-group-append" data-target="#datepickerFim" data-toggle="datetimepicker">
                                <div class="input-group-text"><i class="fa fa-calendar"></i></div>
                            </div>
                        </div>
                    </div>

                    <label class="col-2 col-form-label">Ativo</label>
					<div class="col-2">
                        <asp:DropDownList runat="server" id="ddlAtivo" CssClass="form-control">
                            <asp:ListItem Value="S">Sim</asp:ListItem>
                            <asp:ListItem Value="N">Não</asp:ListItem>
                        </asp:DropDownList>
                    </div>
                </div>


                <div class="card-footer">
                        <asp:Button runat="server" ID="btnSalvar" CssClass="btn btn-whitelabel1" Text="Salvar" onclick="btnSalvar_Click"/>
                        <asp:Button runat="server" ID="btnCancelar" CssClass="btn btn-danger float-right" Text="Cancelar" onclick="btnCancelar_Click"/>
                </div>
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
