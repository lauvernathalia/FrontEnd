<%@ Page Language="C#" AutoEventWireup="true" CodeFile="con_pagamento_transferencias.aspx.cs" Inherits="con_pagamento_transferencias" %>

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

    <link href="https://fontawesome.com/icons/pix?f=brands&s=solid" rel="Stylesheet" />
    <script src="https://kit.fontawesome.com/91552b3746.js" crossorigin="anonymous"></script>


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
                              <li class="breadcrumb-item active">Transferências</li>
                            </ol>
                          </div>
                        </div>




                        <div class="row">
                          <div class="col-12">
                            <div class="card">

                              <div class="card-header bg-whitelabel1">
                                <h3 class="card-title">Transferências</h3>
                              </div>
                              <div class="card-body">

                                <h3><b>Saldo R$ </b><asp:Label runat="server" ID="lblSaldo" Text="0,00"></asp:Label></h3>

                                <h2>Transferêcia</h2>
                                <h6>De que forma deseja realizar a transferência</h6>
                                <div class="card card-outline col-12">
                                    <div class="card-body">
                                        <div class="row">
                                            <div class="col-sm-2">
                                                <div class="form-group">
                                                    <asp:LinkButton runat="server" ID="btnPix" 
                                                        CssClass="btn btn-app text-whitelabel1" onclick="btnPix_Click"><i class="fab fa-pix"></i>Pix Copia e Cola</asp:LinkButton>
                                                </div>
                                            </div>
                                            <div class="col-sm-2">
                                                <div class="form-group">
                                                    <asp:LinkButton runat="server" ID="btnConta" 
                                                        CssClass="btn btn-app text-whitelabel1" onclick="btnConta_Click"><i class="fas fa-bank"></i>Agência e Conta</asp:LinkButton>
                                                </div>
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

                                <div class="card card-outline col-12" runat="server" id="divPix" visible="false">
                                    <div class="card-body">
                                        <div class="row">
                                            <div class="col-sm-12">
                                                <h4>Dados do Beneficiário</h4>
                                            </div>
                                        </div>

                                        <div class="row">
                                            <div class="col-md-6 col-xs-12 my-1">
                                                <h6><b>Tipo Chave PIX</b></h6>
                                                <h6 class="text-whitelabel1">
                                                <asp:DropDownList runat="server" ID="ddlTipoPIX" CssClass="form-control">
                                                    <asp:ListItem Text="E-mail" Value="EMAIL"></asp:ListItem>
                                                    <asp:ListItem Text="CNPJ" Value="CPF"></asp:ListItem>
                                                    <asp:ListItem Text="CPF" Value="CNPJ"></asp:ListItem>
                                                    <asp:ListItem Text="Telefone" Value="PHONE"></asp:ListItem>
                                                    <asp:ListItem Text="Aleatória" Value="EVP"></asp:ListItem>
                                                </asp:DropDownList>
                                                </h6>
                                            </div>
                                        </div>                                    
                                        <div class="row">
                                            <div class="col-md-6 col-xs-12 my-1">
                                                <h6><b>Chave PIX</b></h6>
                                                <h6 class="text-whitelabel1">
                                                <asp:TextBox runat="server" ID="txtChavePIX" Text="" CssClass="form-control"></asp:TextBox>
                                                </h6>
                                            </div>
                                        </div>                                    

                                    </div>
                                </div>
                                <div class="card card-outline col-12" runat="server" id="divConta" visible="false">
                                    <div class="card-body">
                                        <div class="row">
                                            <div class="col-sm-12">
                                                <h4>Dados do Beneficiário</h4>
                                            </div>
                                        </div>

                                        <div class="row">
                                            <div class="col-md-6 col-xs-12 my-1">
                                                <h6><b>Nome do Beneficiário</b></h6>
                                                <h6 class="text-whitelabel1">
                                                <asp:TextBox runat="server" ID="txtNome" Text="" CssClass="form-control"></asp:TextBox>
                                                </h6>
                                            </div>
                                        </div>
                                        <div class="row">
                                            <div class="col-md-6 col-xs-12 my-1">
                                                <h6><b>Documento (CPF/CNPJ)</b></h6>
                                                <h6 class="text-whitelabel1">
                                                <asp:TextBox runat="server" ID="txtDocumento" Text="" CssClass="form-control"></asp:TextBox>
                                                </h6>
                                            </div>
                                        </div>
                                        <div class="row">
                                            <div class="col-md-6 col-xs-12 my-1">
                                                <h6><b>Banco</b></h6>
                                                <h6 class="text-whitelabel1">
                                                <asp:DropDownList runat="server" ID="ddlBanco" CssClass="form-control">
                                                </asp:DropDownList>
                                                </h6>
                                            </div>
                                        </div>

                                        <div class="row">
                                            <div class="col-md-2 col-xs-12 my-1">
                                                <h6><b>Agência</b></h6>
                                                <h6 class="text-whitelabel1">
                                                <asp:TextBox runat="server" ID="txtAgencia" Text="" CssClass="form-control"></asp:TextBox>
                                                </h6>
                                            </div>
                                            <div class="col-md-2 col-xs-12 my-1">
                                                <h6><b>Conta</b></h6>
                                                <h6 class="text-whitelabel1">
                                                <asp:TextBox runat="server" ID="txtConta" Text="" CssClass="form-control"></asp:TextBox>
                                                </h6>
                                            </div>
                                            <div class="col-md-2 col-xs-12 my-1">
                                                <h6><b>Dígito</b></h6>
                                                <h6 class="text-whitelabel1">
                                                <asp:TextBox runat="server" ID="txtContaDigito" Text="" CssClass="form-control"></asp:TextBox>
                                                </h6>
                                            </div>
                                        </div>
                                    </div>
                                </div>

                                <div class="card card-outline col-12" runat="server" id="divPadrao" visible="false">
                                    <div class="card-body">
                                        <div class="row">
                                            <div class="col-sm-12">
                                                <h4>Dados da Transferência</h4>
                                            </div>
                                        </div>

                                        <div class="row">
                                            <div class="col-md-6 col-xs-12 my-1">
                                                <h6><b>Valor da Transferência</b></h6>
                                                <h6 class="text-whitelabel1">
                                                <asp:TextBox runat="server" ID="txtValorTotal" Text="" CssClass="form-control"></asp:TextBox>
                                                </h6>
                                            </div>
                                        </div>
                                        <div class="row">
                                            <div class="col-md-6 col-xs-12 my-1">
                                                <h6><b>Data da Transferência</b></h6>
                                                <div class="input-group date" id="datepickerIni" data-target-input="nearest">
								                    <asp:TextBox ID="txtDataPagamento" runat="server" CssClass="form-control datetimepicker-input" data-target="#datepickerIni"></asp:TextBox>
                                                    <div class="input-group-append" data-target="#datepickerIni" data-toggle="datetimepicker">
                                                        <div class="input-group-text"><i class="fa fa-calendar"></i></div>
                                                    </div>
                                                </div>
                                            </div>

                                        </div>
                                        <div class="row">
                                            <div class="col-md-6 col-xs-12 my-1">
                                                <h6><b>Descrição</b></h6>
                                                <h6 class="text-whitelabel1">
                                                <asp:TextBox runat="server" ID="txtDescricao" Text="" CssClass="form-control"></asp:TextBox>
                                                </h6>
                                            </div>
                                        </div>
                                        <div class="col-sm-2">
                                            <asp:Button runat="server" ID="btnConfirmar" Text="Confirmar" 
                                                CssClass="btn btn-whitelabel1" onclick="btnConfirmar_Click" />
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
