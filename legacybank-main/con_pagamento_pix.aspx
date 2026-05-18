<%@ Page Language="C#" AutoEventWireup="true" CodeFile="con_pagamento_pix.aspx.cs" Inherits="con_pagamento_pix" %>

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
                              <li class="breadcrumb-item"><a href="#">Conta de Pagamento</a></li>
                              <li class="breadcrumb-item active">Pix</li>
                            </ol>
                          </div>
                        </div>




                        <div class="row">
                          <div class="col-12">
                            <div class="card">

                              <div class="card-header bg-whitelabel1">
                                <h3 class="card-title">Pix</h3>
                              </div>
                              <div class="card-body">


                                <h3><b>Saldo R$ </b><asp:Label runat="server" ID="lblSaldo" Text="0,00"></asp:Label></h3>

                                <h2>Pagar com Pix</h2>
                                <h6>Para quem você quer pagar?</h6>
                                <div class="card card-outline col-12">
                                    <div class="card-body">
                                        <div class="row">
                                            <div class="col-sm-10">
                                                <div class="form-group">
                                                    <asp:TextBox ID="txtPIX" runat="server" CssClass="form-control" placeholder="QRCode Pix Copia e Cola" ></asp:TextBox>
                                                </div>
                                            </div>

                                            <div class="col-sm-2">
                                                <asp:Button runat="server" ID="btnContinuar" Text="Continuar" 
                                                    CssClass="btn btn-whitelabel1" onclick="btnContinuar_Click" />
                                            </div>
                                        </div>
                                        <div class="row" runat="server" id="divResposta" visible="false">
                                            <div class="col-sm-12">
                                                <div class="form-group">
                                                    <asp:TextBox ID="txtResposta" runat="server" CssClass="form-control" TextMode="MultiLine" Rows="10" ></asp:TextBox>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>

                                <div class="card card-outline col-12" runat="server" id="divDocumento" visible="false">
                                    <div class="card-body">
                                        <div class="row">
                                            <h4><b class="text-whitelabel1">DADOS</b> DO PIX</h4>
                                        </div>

                                        <div class="row">
                                            <div class="col-md-12 col-xs-12 my-1">
                                                <h6><b>Chave Pix Utilizada</b></h6>
                                                <h6 class="text-whitelabel1"><asp:Label runat="server" ID="lblChavePix" Text=""></asp:Label></h6>
                                            </div>
                                            <div class="col-md-12 col-xs-12 my-1">
                                                <h6><b>Instituição Emissora</b></h6>
                                                <h6 class="text-whitelabel1"><asp:Label runat="server" ID="lblInstituicaoEmissora" Text=""></asp:Label></h6>
                                            </div>
                                            <div class="col-md-12 col-xs-12 my-1">
                                                <h6><b>Banco Emissor</b></h6>
                                                <h6 class="text-whitelabel1"><asp:Label runat="server" ID="lblBancoEmissor" Text=""></asp:Label></h6>
                                            </div>


                                        </div>
                                        <div class="row">
                                            <h4><b class="text-whitelabel1">VALORES</b> DO DOCUMENTO</h4>
                                        </div>
                                        <div class="row">
                                            <div class="col-md-12 col-xs-12 my-1">
                                                <h6><b>Valor do Documento</b></h6>
                                                <h6 class="text-whitelabel1"><asp:Label runat="server" ID="lblValorDocumento" Text=""></asp:Label></h6>
                                            </div>
                                        </div>
                                        <div class="row" runat="server" id="divValores" visible="false" >
                                            <div class="col-md-12 col-xs-12 my-1">
                                                <h6><b>Desconto</b></h6>
                                                <h6 class="text-whitelabel1"><asp:Label runat="server" ID="lblDesconto" Text=""></asp:Label></h6>
                                            </div>
                                            <div class="col-md-12 col-xs-12 my-1">
                                                <h6><b>Juros</b></h6>
                                                <h6 class="text-whitelabel1"><asp:Label runat="server" ID="lblJuros" Text=""></asp:Label></h6>
                                            </div>
                                            <div class="col-md-12 col-xs-12 my-1">
                                                <h6><b>Multa</b></h6>
                                                <h6 class="text-whitelabel1"><asp:Label runat="server" ID="lblMulta" Text=""></asp:Label></h6>
                                            </div>
                                        </div>

                                        <div class="row">
                                            <div class="col-md-6 col-xs-12 my-1">
                                                <h6><b>Valor a pagar</b></h6>
                                                <h6 class="text-whitelabel1">
                                                <asp:TextBox runat="server" ID="txtValorPagar" Text="" CssClass="form-control" Enabled="false"></asp:TextBox>
                                                <asp:TextBox runat="server" ID="txtMenorValor" Text="" Visible="false"> </asp:TextBox>
                                                <asp:TextBox runat="server" ID="txtMaiorValor" Text="" Visible="false"> </asp:TextBox>
                                                </h6>
                                            </div>
                                        </div>

                                        <div class="row">
                                            <div class="col-md-6 col-xs-12 my-1">
                                                <h6><b>Taxa cobrada para pagamento da conta</b></h6>
                                                <h6 class="text-whitelabel1">
                                                <asp:Label runat="server" ID="lblTaxaCobrada" Text=""></asp:Label>
                                                </h6>
                                            </div>
                                        </div>


                                        <div class="row">
                                            <div class="col-md-6 col-xs-12 my-1">
                                                <h6><b>Valor Total</b></h6>
                                                <h6 class="text-whitelabel1">
                                                <asp:TextBox runat="server" ID="txtValorTotal" Text="" CssClass="form-control" Enabled="false"></asp:TextBox>
                                                </h6>
                                            </div>
                                        </div>

                                        <div class="row">
                                            <div class="col-md-6 col-xs-12 my-1">
                                                <h6><b>Data de Vencimento</b></h6>
                                                <h6 class="text-whitelabel1"><asp:Label runat="server" ID="lblVencimento" CssClass="form-control" Enabled="false"></asp:Label></h6>
                                            </div>
                                        </div>



                                        <div class="row" runat="server" id="divDatas" visible="true" >
                                            <div class="col-md-6 col-xs-12 my-1">
                                                <h6><b>Data de Pagamento</b></h6>
                                                <div class="input-group date" id="datepickerIni" data-target-input="nearest">
								                    <asp:TextBox ID="txtDataPagamento" runat="server" CssClass="form-control datetimepicker-input" data-target="#datepickerIni"></asp:TextBox>
                                                    <div class="input-group-append" data-target="#datepickerIni" data-toggle="datetimepicker">
                                                        <div class="input-group-text"><i class="fa fa-calendar"></i></div>
                                                    </div>
                                                </div>


                                            </div>
                                        </div>
                                        <div class="row" runat="server" id="divIdentificacao" visible="true" >
                                            <div class="col-md-6 col-xs-12 my-1">
                                                <h6><b>Identificação (15 caracteres)</b></h6>
                                                <h6 class="text-whitelabel1"><asp:TextBox runat="server" ID="txtIdentificacao" CssClass="form-control" Text=""></asp:TextBox></h6>
                                            </div>
                                            <div class="col-md-6 col-xs-12 my-1">
                                                <h6><b>Código Referência (Opcional)</b></h6>
                                                <h6 class="text-whitelabel1"><asp:TextBox runat="server" ID="txtReferencia" CssClass="form-control" Text=""></asp:TextBox></h6>
                                            </div>
                                        </div>

                                        <div class="row" >
                                            <div class="col-md-12 col-xs-12 my-1">
                                                <h6><b>ATENÇÃO!</b> Antes de confirmar o pagamento verifique se todas as informações estão corretas.</h6>
                                            </div>


                                        <div class="row">
                                            <asp:Button runat="server" ID="btnConfirmar" CssClass="btn btn-whitelabel1" onclick="btnConfirmar_Click" Text="Confirmar" />
                                        </div>


                                    </div>
                                </div>





                                </div>
                                <div class="card card-outline col-12" runat="server" id="divPagamentorealizado" visible="false">
                                    <div class="card-body">
                                        <div class="row">
                                            <h4><b class="text-whitelabel1">STATUS - TRANSAÇÃO PIX</h4>
                                        </div>

                                        <div class="col-md-4 col-xs-12 my-1  border border-dark">

                                            <div class="row" runat="server" id="dvReciboCliente">
                                                <div class="col-12">
                                                    <h6 class="text-whitelabel1 text-center">
                                                    <asp:Label runat="server" ID="lblStatus"></asp:Label>
                                                    </h6>
                                                </div>
                                            </div>

                                            <center>
                                            <button class="btn btn-whitelabel1" onclick="printDiv('dvReciboCliente')"><i class="fas fa-print" aria-hidden="true" style="font-size: 17px;">     Imprimir</i></button>                                                
                                            </center>
                                        </div>
                                        <div class="col-md-2 col-xs-12 my-1">
                                        </div>
                                        <div class="row">
                                            <asp:Button runat="server" ID="btnNovoPagamento" CssClass="btn btn-whitelabel1" onclick="btnNovoPagamento_Click" Text="Realizar novo pagamento" />
                                        </div>


                                    </div>
                                </div>

                              <div class="card-footer">
                                 xxx
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
