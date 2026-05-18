<%@ Page Language="C#" AutoEventWireup="true" CodeFile="con_pagamentos_pix.aspx.cs" Inherits="con_pagamentos_pix" %>

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

                                <h2>Pagamentos & Transferências</h2>
                                <h6>O que deseja fazer?</h6>
                                <div class="card card-outline col-12">
                                    <div class="card-body">
                                        <div class="row">
                                                    <asp:LinkButton runat="server" ID="btnTransferirPix" 
                                                        CssClass="btn btn-app text-whitelabel1" onclick="btnTransferirPix_Click" style="white-space: normal;"><i class="fab fa-pix"></i>Transferir por PIX</asp:LinkButton>

                                                    <asp:LinkButton runat="server" ID="btnReceberQRCode" 
                                                        CssClass="btn btn-app text-whitelabel1" onclick="btnReceberQRCode_Click"><i class="fas fa-qrcode"></i>Receber por QRCode</asp:LinkButton>

                                                    <asp:LinkButton runat="server" ID="btnChaves" 
                                                        CssClass="btn btn-app text-whitelabel1" onclick="btnChaves_Click"><i class="fas fa-key"></i>Minhas Chaves</asp:LinkButton>

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


                                <div class="card card-outline col-12" runat="server" id="divDadosCobranca" visible="false">
                                    <div class="card-body">
                                        <div class="row">
                                            <div class="col-sm-12">
                                                <h5 class="text-whitelabel1"><asp:Label runat="server" ID="lblTituloPadraoTopo" Text=""></asp:Label></h5>
                                            </div>
                                        </div>
                                        <div class="row">
                                            <div class="col-12">
                                                <div class="row">
                                                    <div class="col-sm-12">
                                                        <h5 class="text-whitelabel1">Preencha os dados abaixo</h5>
                                                    </div>
                                                </div>
                                                <div class="row">
                                                    <div class="col-sm-2">
                                                        <div class="form-group">
                                                            <asp:Label runat="server" ID="Label1" Text="Valor"></asp:Label>
                                                            <asp:TextBox runat="server" ID="txtValor" CssClass="form-control"></asp:TextBox>
                                                        </div>
                                                    </div>
                                                    <div class="col-sm-2">
                                                        <div class="form-group">
                                                            <asp:Label runat="server" ID="Label6" Text="Vencimento"></asp:Label>
                                                            <div class="input-group date" id="dpVencimento" data-target-input="nearest">
								                                <asp:TextBox ID="txtVencimento" runat="server" CssClass="form-control datetimepicker-input" data-target="#dpVencimento"></asp:TextBox>
                                                                <div class="input-group-append" data-target="#dpVencimento" data-toggle="datetimepicker">
                                                                    <div class="input-group-text"><i class="fa fa-calendar"></i></div>
                                                                </div>
                                                            </div>
                                                        </div>
                                                    </div>
                                                    <div class="col-sm-6">
                                                        <div class="form-group">
                                                            <asp:Label runat="server" ID="Label2" Text="Descrição"></asp:Label>
                                                            <asp:TextBox runat="server" ID="txtDescricao" CssClass="form-control"></asp:TextBox>
                                                        </div>
                                                    </div>
                                                    <div class="col-sm-2">
                                                        <div class="form-group">
                                                        <asp:Label runat="server" ID="Label19" Text="Adquirente"></asp:Label>
                                                            <asp:DropDownList runat="server" ID="ddlAdquirentes" CssClass="form-control">
                                                            </asp:DropDownList>
                                                        </div>
                                                    </div>


                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>

                                <div class="card card-outline col-12" runat="server" id="divTransferirPix" visible="false">

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
                                                <asp:TextBox runat="server" ID="txtChavePixTransferencia" Text="" CssClass="form-control"></asp:TextBox>
                                                </h6>
                                            </div>
                                        </div>                                    

                                    </div>

                                    <div class="card-footer">
                                        <asp:Button runat="server" ID="btnContinuarTransferencias" Text="Confirmar" 
                                            CssClass="btn btn-whitelabel1" onclick="btnContinuarTransferencias_Click" />
                                    </div>


                                </div>
                                <!-- Dados Cliente -->
                                <!-- ********************************************************************************************************************************************************** -->

                                <div class="card card-outline col-12" runat="server" id="divDadosCliente" visible="false">
                                    <div class="card-body">
                                        <div class="row">
                                            <div class="col-12">
                                                <div class="row">
                                                    <div class="col-sm-12">
                                                        <h5 class="text-whitelabel1">Selecione abaixo o cliente para quem deseja gerar a cobrança</h5>
                                                    </div>
                                                </div>

                                                <div class="row">
                                                    <div class="col-sm-12">
                                                        <div class="form-group">
                                                            <asp:Label runat="server" ID="Label12" Text="Cliente"></asp:Label>
                                                            <asp:DropDownList runat="server" ID="ddlClientePadrao" CssClass="form-control" 
                                                                AutoPostBack="True" onselectedindexchanged="ddlClientePadrao_SelectedIndexChanged" ></asp:DropDownList>
                                                        </div>
                                                    </div>
                                                </div>
                                                
                                                <div class="row">
                                                    <div class="col-sm-12">
                                                        <h6>ou preencha os dados abaixo</h6>
                                                    </div>
                                                </div>

                                                <div class="row">
                                                    <div class="col-sm-4">
                                                        <div class="form-group">
                                                            <asp:Label runat="server" ID="Label13" Text="Nome"></asp:Label>
                                                            <asp:TextBox runat="server" ID="txtNome" CssClass="form-control"></asp:TextBox>
                                                        </div>
                                                    </div>
                                                    <div class="col-sm-4">
                                                        <div class="form-group">
                                                        <asp:Label runat="server" ID="Label14" Text="CNPJ/CPF"></asp:Label>
                                                            <asp:TextBox runat="server" ID="txtCNPJCPF" CssClass="form-control"></asp:TextBox>
                                                        </div>
                                                    </div>
                                                    <div class="col-sm-4">
                                                        <div class="form-group">
                                                        <asp:Label runat="server" ID="Label15" Text="E-Mail"></asp:Label>
                                                            <asp:TextBox runat="server" ID="txtEmail" CssClass="form-control"></asp:TextBox>
                                                        </div>
                                                    </div>

                                                </div>

                                                <div class="row">
                                                    <div class="col-sm-3">
                                                        <div class="form-group">
                                                            <asp:Label runat="server" ID="Label20" Text="Celular"></asp:Label>
                                                            <asp:TextBox runat="server" ID="txtCelular" CssClass="form-control"></asp:TextBox>
                                                        </div>
                                                    </div>
                                                    <div class="col-sm-3">
                                                        <div class="form-group">
                                                        <asp:Label runat="server" ID="Label21" Text="CEP"></asp:Label>
                                                            <asp:TextBox runat="server" ID="txtCEP" CssClass="form-control"></asp:TextBox>
                                                        </div>
                                                    </div>
                                                    <div class="col-sm-3">
                                                        <div class="form-group">
                                                        <asp:Label runat="server" ID="Label22" Text="Número"></asp:Label>
                                                            <asp:TextBox runat="server" ID="txtNumero" CssClass="form-control"></asp:TextBox>
                                                        </div>
                                                    </div>

                                                </div>

                                            </div>
                                        </div>
                                    </div>
                                </div>

                                <!-- Detalhes da Cobrança -->
                                <!-- ********************************************************************************************************************************************************** -->

                                <div class="card card-outline col-12" runat="server" id="divDetalhesCobranca" visible="false">
                                    <div class="card-body">


                                        <div class="row">
                                            <div class="col-12">
                                                <div class="row">
                                                    <div class="col-sm-12">
                                                        <h5 class="text-whitelabel1">Preencha os dados referente aos detalhes da cobrança</h5>
                                                    </div>
                                                </div>
                                                <div class="row">
                                                    <div class="col-sm-3">
                                                        <div class="form-group">
                                                        <asp:Label runat="server" ID="Label7" Text="Código Referência"></asp:Label>
                                                            <asp:TextBox runat="server" ID="txtReferencia" CssClass="form-control"></asp:TextBox>
                                                        </div>
                                                    </div>
                                                    <div class="col-sm-3">
                                                        <div class="form-group">
                                                        <asp:Label runat="server" ID="Label9" Text="Dias Limite Pagamento"></asp:Label>
                                                            <asp:TextBox runat="server" ID="txtDias" CssClass="form-control" Text="2"></asp:TextBox>
                                                        </div>
                                                    </div>

                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>

                                <!-- Receber por qrcode -->
                                <!-- ********************************************************************************************************************************************************** -->
                                <div class="card card-outline col-12" runat="server" id="divReceberQRCode" visible="false">
                                    <div class="card-body">
                                        <div class="row">
                                            <div class="col-sm-12">
                                                <h5 class="text-whitelabel1">Receber por QR Code</h5>
                                            </div>
                                        </div>

                                        <div class="row">
                                            <div class="col-sm-12">
                                                <div class="form-group">
                                                    <div class="custom-control custom-checkbox">
                                                        <input class="custom-control-input" type="checkbox" runat="server" id="ckbEmailQRCode" checked>
                                                        <label for="ckbBEmail" class="custom-control-label">Enviar link do pagamento por e-mail?</label>
                                                    </div>

                                                </div>
                                            </div>
                                        </div>

                                        <div class="row"  runat="server" id="divQRCode" visible="false">

                                            <div class="col-sm-12">
                                                <div class="form-group">
                                                    <asp:Label runat="server" ID="Label23" Text="Endereço do link de pagamento"></asp:Label>
                                                    <asp:TextBox runat="server" ID="txtURLQRCode" CssClass="form-control"></asp:TextBox>
                                                </div>
                                            </div>

                                            <div class="col-sm-12">
                                                <div class="form-group">
                                                    <a href="" runat="server" id="hrfQRCode" class="btn btn-block btn-whitelabel1" target="_blank"><asp:Label runat="server" ID="Label24" Text="CLIQUE AQUI PARA ABRIR LINK DE PAGAMENTO"></asp:Label></a>
                                                </div>
                                            </div>



                                            <div class="col-10">
                                                <h6><i class="fas fa-qrcode"></i>  Pix copia e cola<br /></h6>
                                                <asp:LinkButton ID="LinkButton1" runat="server" CssClass="btn btn-default" ToolTip="copiar" OnClientClick="myFunctionChavePix()"><i class="fas fa-copy"></i></asp:LinkButton>
                                                <asp:TextBox runat="server" ID="lblPixCopiaCola" Text="" TextMode="MultiLine" Rows="3" CssClass="form-control"></asp:TextBox>
                                            </div>
                                            <div class="col-2">
                                                <center>
                                                <strong><i class="fab fa-pix"></i>  Pague o boleto com Pix usando o QRcode abaixo<br /></strong>
                                                <img runat="server" class="img-fluid" id="imgQRcode" src="" alt=""  />                                    
                                                </center>
                                            </div>
                                        </div>

                                        <div class="row">
                                            <div class="col-12">


                                            </div>
                                        </div>
                                    </div>
                                    <div class="card-footer">
                                        <asp:Button runat="server" ID="btnContinuarPixQRCode" 
                                            Text="Confirmar recebimento por QRCode" CssClass="btn btn-whitelabel1" 
                                            onclick="btnContinuarPixQRCode_Click" />
                                        <asp:Button runat="server" ID="btnVoltarPixQRCode" Text="Voltar" 
                                            CssClass="btn btn-whitelabel1"  Visible="false" 
                                            onclick="btnVoltarPixQRCode_Click" />
                                    </div>

                                </div>


                                <div class="card card-outline col-12" runat="server" id="divChaves" visible="false">
                                    <div class="card-header">
                                        <h5 class="text-whitelabel1">Minhas Chaves Pix</h5>
                                    </div>
                                    <div class="card-body">
                                        <div class="row">
                                            <div class="col-sm-8"  >
                                                <div class="form-group">
                                                    <label>Chave Pix</label>
                                                    <asp:TextBox id="txtChavePix" runat="server" CssClass="form-control" Enabled="false"></asp:TextBox>
                                                </div>
                                            </div>

                                            <div class="col-sm-2">
                                                <label><strong class="text-danger"> </strong></label><br /><br />
                                                <asp:LinkButton ID="lbkCopiar" runat="server" CssClass="btn btn-default" ToolTip="copiar" OnClientClick="myFunctionChavePix()"><i class="fas fa-copy"></i></asp:LinkButton>
                                            </div>
                                        </div>
                                    </div>

                                </div>

                                <div class="card card-outline col-12" runat="server" id="divPadrao" visible="false">
                                </div>



                              </div>
                              <div class="card-footer">

                              </div>
                            </div>
                          </div>
                        </div>



                    <div class="modal fade" id="mdConfirmar">
                    <div class="modal-dialog modal-sm">
                        <div class="modal-content">
                        <div class="modal-header">
                            <h6 class="modal-title"><b>Autenticação de 2 Fatores - 2FA</b></h6>
                            <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                            <span aria-hidden="true">&times;</span>
                            </button>
                        </div>
                        <div class="modal-body">
                            <center>
                            <p>Para confirmar a operação, digite abaixo o código de confirmação que você recebeu no e-mail do administrador da conta: <asp:Label runat="server" ID="lblEmailConfirmacao"></asp:Label></p>
                            </center>
                            <div class="input-group mb-3 col-12">
                                <asp:TextBox runat="server" ID="txt2FA" CssClass="form-control" placeholder="Código de Confirmação" Visible="true"></asp:TextBox>
                                <div class="input-group-prepend">
                                <span class="input-group-text"><i class="fas fa-laptop-code"></i></span>
                                </div>
                            </div>
                            <p class="mt-1">
                                <strong>Não recebeu?  </strong>   <asp:LinkButton ID="lkbReenviar" runat="server" onclick="lkbReenviar_Click">Enviar novo código</asp:LinkButton>
                            </p>                                                                                                                      
                        </div>
                        <div class="modal-footer justify-content-between">
                            <button type="button" class="btn btn-danger" data-dismiss="modal">Cancelar</button>
                            <asp:Button runat="server" ID="btnConfirmarPagamentoTransferencia2FA" 
                                CssClass="btn btn-primary" Text="Confirmar" 
                                onclick="btnConfirmarPagamentoTransferencia2FA_Click" />
                        </div>
                        </div>
                    </div>
                    </div>

                    <div class="modal fade" id="mdSenha">
                    <div class="modal-dialog modal-sm">
                        <div class="modal-content">
                        <div class="modal-header">
                            <h6 class="modal-title"><b>Confirmação por senha</b></h6>
                            <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                            <span aria-hidden="true">&times;</span>
                            </button>
                        </div>
                        <div class="modal-body">
                            <center>
                            <p>Para confirmar a operação, digite abaixo a sua senha de acesso</p>
                            </center>
                            <div class="input-group mb-3 col-12">
                                <asp:TextBox runat="server" ID="txtSenha" CssClass="form-control" TextMode="Password" placeholder="Senha de acesso" Visible="true"></asp:TextBox>
                                <div class="input-group-prepend">
                                <span class="input-group-text"><i class="fas fa-key"></i></span>
                                </div>
                            </div>
                        </div>
                        <div class="modal-footer justify-content-between">
                            <button type="button" class="btn btn-danger" data-dismiss="modal">Cancelar</button>
                            <asp:Button runat="server" ID="btnConfirmarSenha" 
                                CssClass="btn btn-primary" Text="Confirmar" 
                                onclick="btnConfirmarSenha_Click" />
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
        $('#dpVencimento').datetimepicker({
            format: 'DD/MM/YYYY'
        });
        $('#dpDataLimite').datetimepicker({
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

<script type="text/javascript">
    function myFunctionChavePix() {
        var copyText = document.getElementById("txtChavePix");
        copyText.select();
        copyText.setSelectionRange(0, 99999); // For mobile devices
        navigator.clipboard.writeText(copyText.value);
        alert("Chave Pix: " + copyText.value + " copiada com sucesso!");
    }

</script>

</body>
</html>
