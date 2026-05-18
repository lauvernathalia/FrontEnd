<%@ Page Language="C#" AutoEventWireup="true" CodeFile="con_onboarding.aspx.cs" Inherits="con_onboarding" %>


<%@ Register TagPrefix="Portal" TagName="PageBottom" Src="rodapepadrao.ascx" %>
<%@ Register TagPrefix="Portal" TagName="PageLeft" Src="menupadrao.ascx" %>
<%@ Register TagPrefix="Portal" TagName="PageHeader" Src="topopadrao.ascx" %>
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

        <div class="wrapper">
            <PORTAL:PAGEHEADER id="PageHeader1" title="Site Directory" runat="server" ModuleSource="topopadrao.ascx"></PORTAL:PAGEHEADER>
            <PORTAL:PAGELEFT id="Pageheader2" title="Site Directory" runat="server" ModuleSource="menupadrao.ascx"></PORTAL:PAGELEFT>
            <div class="content-wrapper">
                <section class="content">
                    <div class="container-fluid" >

                        <div class="row mb-2">
                          <div class="col-sm-12">
                            <ol class="breadcrumb">
                              <li class="breadcrumb-item"><a href="#">Compliance</a></li>
                              <li class="breadcrumb-item active">Onboarding</li>
                            </ol>
                          </div>
                        </div>




                        <div class="row">
                          <div class="col-12">
                            <div class="card">

                              <div class="card-header bg-whitelabel1">
                                <h3 class="card-title">ONBOARDING</h3>
                              </div>
                              <div class="card-body">


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

                                    <label class="col-2 col-form-label">Adquirente/BaaS</label>
						            <div class="col-2">
                                        <asp:DropDownList runat="server" id="ddlStatus" class="form-control" ></asp:DropDownList>
                                    </div>
                                </div>
                                <div class="row col-12">
                                
                                    <div class="col">
                                        <div class="card">
                                            <div class="card-header bg-whitelabel1 d-flex p-0">
                                                <small class="p-3">
                                                  <i class="fas fa-chart-pie mr-1 fa-2x"></i><br />
                                                  Análise de Risco
                                                </small>
                                            </div>
                                            <div class="card-body">

                                            </div>
                                        </div>
                                    </div>
                                    <div class="col">
                                        <div class="card">
                                            <div class="card-header bg-whitelabel1 d-flex p-0">
                                                <small class="p-3">
                                                  <i class="fas fa-address-card mr-1 fa-2x"></i><br />
                                                  Dados Cadastrais
                                                </small>
                                            </div>
                                            <div class="card-body">

                                            </div>
                                        </div>
                                    </div>
                                    <div class="col">
                                        <div class="card">
                                            <div class="card-header bg-whitelabel1 d-flex p-0">
                                                <small class="p-3">
                                                  <i class="fas fa-money-check-alt mr-1 fa-2x"></i><br />
                                                  Dados Bancários
                                                </small>
                                            </div>
                                            <div class="card-body">

                                            </div>
                                        </div>
                                    </div>
                                    <div class="col">
                                        <div class="card">
                                            <div class="card-header bg-whitelabel1 d-flex p-0">
                                                <small class="p-3">
                                                  <i class="fas fa-calculator mr-1 fa-2x"></i><br />
                                                  Terminais
                                                </small>
                                            </div>
                                            <div class="card-body">

                                            </div>
                                        </div>
                                    </div>
                                    <div class="col">
                                        <div class="card">
                                            <div class="card-header bg-whitelabel1 d-flex p-0">
                                                <small class="p-3">
                                                  <i class="fas fa-tasks mr-1 fa-2x"></i><br />
                                                  Plano
                                                </small>
                                            </div>

                                            <div class="card-body">

                                            </div>
                                        </div>
                                    </div>
                                    <div class="col">
                                        <div class="card">

                                            <div class="card-header bg-whitelabel1 d-flex p-0">
                                                <small class="p-3">
                                                  <i class="fas fa-university mr-1 fa-2x"></i><br />
                                                  Adquirente
                                                </small>
                                            </div>
                                            <div class="card-body">

                                            </div>
                                        </div>
                                    </div>
                                    <div class="col">
                                        <div class="card">
                                            <div class="card-header bg-whitelabel1 d-flex p-0">
                                                <small class="p-3">
                                                  <i class="fas fa-comments-dollar mr-1 fa-2x"></i><br />
                                                  Gateway
                                                </small>
                                            </div>
                                            <div class="card-body">

                                            </div>
                                        </div>
                                    </div>

                                
                                </div>
                                <!-- Dashboard onboarding graficos -->

                                <div class="row col-12">
                                
                                    <div class="col">
                                        <div class="card">
                                            <div class="card-header bg-whitelabel1">
                                                <small>Painel</small>
                                            </div>
                                            <div class="card-body">

                                            </div>
                                        </div>
                                    </div>
                                    <div class="col">
                                        <div class="card">
                                            <div class="card-header bg-whitelabel1">
                                                <small>Status Cadastro</small>
                                            </div>
                                            <div class="card-body">

                                            </div>
                                        </div>
                                    </div>
                                    <div class="col">
                                        <div class="card">
                                            <div class="card-header bg-whitelabel1">
                                                <small>Cadastro MCC</small>
                                            </div>
                                            <div class="card-body">

                                            </div>
                                        </div>
                                    </div>
                                </div>

                                <!-- ETAPAS -->

                                <div class="row col-12">
                                
                                    <div class="col">
                                        <div class="card">
                                            <div class="card-header bg-whitelabel1">
                                                <small>Etapas</small>
                                            </div>
                                            <div class="card-body">

                                            </div>
                                        </div>
                                    </div>
                                </div>

                                <!-- Cadastros -->

                                <div class="row col-12">
                                
                                    <div class="col">
                                        <div class="card">
                                            <div class="card-header bg-whitelabel1">
                                                <small>Cadastros</small>
                                            </div>
                                            <div class="card-body">

                                            </div>
                                        </div>
                                    </div>
                                </div>


                              </div>
                              <div class="card-footer">

                              </div>
                            </div>
                          </div>




                        </div>



                    </div>
                </section>
            </div>
            <PORTAL:PAGEBOTTOM id="Pageheader3" title="Site Directory" runat="server" ModuleSource="rodapepadrao.ascx"></PORTAL:PAGEBOTTOM><!-- Fim Rodapé da Pagina -->
    
        </div>
    </form>

<script type="text/javascript" src="../plugins/datatables/jquery.dataTables.js"></script>
<script type="text/javascript" src="../plugins/datatables-bs4/js/dataTables.bootstrap4.js"></script>

<script type="text/javascript" src="../plugins/datatables-responsive/js/dataTables.responsive.min.js"></script>
<script type="text/javascript" src="../plugins/datatables-responsive/js/responsive.bootstrap4.min.js"></script>
<script type="text/javascript" src="../plugins/datatables-buttons/js/dataTables.buttons.min.js"></script>
<script type="text/javascript" src="../plugins/datatables-buttons/js/buttons.bootstrap4.min.js"></script>
<script type="text/javascript" src="../plugins/jszip/jszip.min.js"></script>
<script type="text/javascript" src="../plugins/pdfmake/pdfmake.min.js"></script>
<script type="text/javascript" src="../plugins/pdfmake/vfs_fonts.js"></script>
<script type="text/javascript" src="../plugins/datatables-buttons/js/buttons.html5.min.js"></script>
<script type="text/javascript" src="../plugins/datatables-buttons/js/buttons.print.min.js"></script>
<script type="text/javascript" src="../plugins/datatables-buttons/js/buttons.colVis.min.js"></script>

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
    var table = $('#tblCompleta').DataTable({
      order: [[0, 'desc']],
      "paging": true,
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

$(document).ready(function () {
    var table = $('#tblParcial').DataTable({
      order: [[0, 'desc']],
      "paging": true,
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
