<%@ Page Language="C#" AutoEventWireup="true" CodeFile="cad_planos_exportar.aspx.cs" Inherits="cad_planos_exportar" %>

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

                        <div class="card m-2">
                            <div class="card-body">
                                <h3 class="modal-title text-black text-center"><b class="text-whitelabel1">EXPORTAR</b> PLANO</h3>

                                <div class="form-group row col-12">
                                    <div class="col-sm-3">
                                        <label class="col-sm-12 col-form-label">ID</label>
                                        <asp:Label id="lblID" runat="server" CssClass="form-control"></asp:Label>
                                    </div>
                                    <div class="col-sm-3">
                                        <label class="col-sm-12 col-form-label">Adquirente</label>
                                        <asp:DropDownList runat="server" id="ddlAdquirentes" CssClass="form-control" 
                                            AutoPostBack="True" onselectedindexchanged="ddlAdquirentes_SelectedIndexChanged">
                                        </asp:DropDownList>
                                    </div>

                                    <div class="col-sm-3">
                                        <label class="col-sm-12 col-form-label">Plano Referência</label>
                                        <asp:DropDownList runat="server" id="ddlPlanosReferencia" CssClass="form-control">
                                        </asp:DropDownList>
                                    </div>
                                    <div class="col-sm-3">
                                        <label class="col-sm-12 col-form-label">ID Referência</label>
                                        <asp:Label id="lblIDReferencia" runat="server" CssClass="form-control" Enabled="false"></asp:Label>
                                    </div>

                                </div>

                                <div class="form-group row col-12">
                                    <div class="col-sm-6">
                                        <label class="col-sm-12 col-form-label">Nome</label>
                                        <asp:TextBox id="txtNome" runat="server" CssClass="form-control" Enabled="false"></asp:TextBox>
                                    </div>
                                    <div class="col-sm-6">
                                        <label class="col-sm-12 col-form-label">Descrição</label>
                                        <asp:TextBox id="txtDescricao"  runat="server" CssClass="form-control" Enabled="false"></asp:TextBox>
                                    </div>
                                </div>



                                <!-- Caixa de Recolher -->

                                <div class="row" runat="server" id="dvBloqueado" visible="false">
                                    <h3 class="modal-title text-black text-center"><b class="text-whitelabel1"><b>ATENÇÃO!</b> ESTE TIPO DE ADQUIRENTE NÃO PERMITE EXPORTAÇÃO PARA CRIAÇÃO DE PLANOS.</h3>
                                </div>

                                <div class="row" runat="server" id="dvPermitido" visible="false">
                                <div class="form-group row col-12">
                                    <div class="col-sm-3">
                                        <label class="col-sm-12 col-form-label"><br /></label>
                                        <asp:Button runat="server" ID="btnExportar" Text="Exportar" CssClass="btn btn-whitelabel1" onclick="btnExportar_Click"/>
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

        <div class="row">
            <div class="col-12">
                
                <div class="form-group">
                    <div class="col-sm-12">
                        <asp:TextBox ID="txtExportacao" runat="server" CssClass="form-control" TextMode="MultiLine" Rows="5" Visible="false"></asp:TextBox>
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
<script>

    function printDiv(divName) {
        var printContents = document.getElementById(divName).innerHTML;
        var originalContents = document.body.innerHTML;

        document.body.innerHTML = printContents;

        window.print();

        document.body.innerHTML = originalContents;
    }

</script>

</body>
</html>
