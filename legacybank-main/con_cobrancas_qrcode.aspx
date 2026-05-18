<%@ Page Language="C#" AutoEventWireup="true" CodeFile="con_cobrancas_qrcode.aspx.cs" Inherits="con_cobrancas_qrcode" %>

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
                              <li class="breadcrumb-item"><a href="#">Cobranças</a></li>
                              <li class="breadcrumb-item active">Receber por QR Code (Pix)</li>
                            </ol>
                          </div>
                        </div>

                        <div class="row">
                          <div class="col-12">
                            <div class="card">

                              <div class="card-header bg-whitelabel1">
                                <h3 class="card-title">Cobranças - Receber por QR Code (Pix)</h3>
                              </div>
                              <div class="card-body">

                                <div class="row">
                                    <div class="col-sm-12">
                                        <h5 class="text-whitelabel1">Preencha os dados referentes a Cobrança</h5>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-sm-12">
                                        <h6 class="text-whitelabel1">Digite um valor maior que R$ 10,00</h6>
                                    </div>
                                </div>

                                <div class="row">
                                    <div class="col-sm-2">
                                        <div class="form-group">
                                            <asp:Label runat="server" ID="Label5" Text="Valor Total a Cobrar"></asp:Label>
                                            <asp:TextBox runat="server" ID="txtValor" CssClass="form-control"></asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="col-sm-2" runat="server" id="dvTaxaPrevista" visible="false">
                                        <div class="form-group">
                                            <asp:Label runat="server" ID="Label1" Text="Taxa Operação (R$)"></asp:Label>
                                            <asp:TextBox runat="server" ID="txtTaxa" CssClass="form-control" Enabled="false"></asp:TextBox>
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


                                    <div class="col-sm-4">
                                        <div class="form-group">
                                            <asp:Label runat="server" ID="Label8" Text="Descrição"></asp:Label>
                                            <asp:TextBox runat="server" ID="txtDescricao" CssClass="form-control"></asp:TextBox>
                                        </div>
                                    </div>

                                    <div class="col-sm-2">
                                        <div class="form-group">
                                            <asp:Label runat="server" ID="Label2" Text="referência"></asp:Label>
                                            <asp:TextBox runat="server" ID="txtReferencia" CssClass="form-control"></asp:TextBox>
                                        </div>
                                    </div>

                                    <div class="col-sm-2">
                                        <div class="form-group">
                                        <asp:Label runat="server" ID="Label19" Text="Adquirente"></asp:Label>
                                            <asp:DropDownList runat="server" ID="ddlAdquirentes" CssClass="form-control" 
                                                onselectedindexchanged="ddlAdquirentes_SelectedIndexChanged" 
                                                AutoPostBack="True">
                                            </asp:DropDownList>
                                        </div>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-sm-12">
                                        <div class="form-group">
                                        <h6 class="text-whitelabel1">Selecione o cliente desejado</h6>
                                        <asp:DropDownList runat="server" id="ddlComprador" CssClass="form-control" 
                                                AutoPostBack="True" onselectedindexchanged="ddlComprador_SelectedIndexChanged">
                                        </asp:DropDownList>
                                        </div>
                                    </div>
                                </div>


                                <div class="row">
                                    <div class="col-sm-12">
                                        <h6 class="text-whitelabel1">Ou digite os dados abaixo</h6>
                                    </div>
                                </div>

                                <div class="row">
                                    <div class="col-sm-3">
                                        <div class="form-group">
                                            <label>Nome<strong class="text-danger">*</strong></label>
                                            <asp:TextBox runat="server" ID="txtNome" CssClass="form-control"></asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="col-sm-3">
                                        <div class="form-group">
                                            <label>CNPJ/CPF<strong class="text-danger">*</strong></label>
                                            <asp:TextBox runat="server" ID="txtCNPJCPF" CssClass="form-control"></asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="col-sm-3">
                                        <div class="form-group">
                                            <label>E-Mail<strong class="text-danger">*</strong></label>
                                            <asp:TextBox runat="server" ID="txtEmail" CssClass="form-control" 
                                                ontextchanged="txtEmail_TextChanged"></asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="col-sm-3">
                                        <div class="form-group">
                                            <label>Celular<strong class="text-danger">*</strong></label>
                                            <asp:TextBox runat="server" ID="txtCelular" CssClass="form-control"></asp:TextBox>
                                        </div>
                                    </div>
                                </div>

                                <div class="row">
                                    <div class="col-sm-2"  >
                                        <div class="form-group">
                                        <label>CEP<strong class="text-danger">*</strong></label>
                                        <asp:TextBox runat="server" ID="txtCEP" CssClass="form-control" 
                                                placeholder="00000000" data-inputmask='"mask": "99999-999"' data-mask  
                                                AutoPostBack="True" ontextchanged="txtCEP_TextChanged"></asp:TextBox> 
                                        </div>
                                    </div>
                                    <div class="col-sm-6">
                                        <div class="form-group">
                                        <label>Endereço<strong class="text-danger">*</strong></label>
                                        <asp:TextBox runat="server" ID="txtEndereco" CssClass="form-control" placeholder="Digite o seu endereço" ></asp:TextBox> 
                                        </div>
                                    </div>
                                    <div class="col-sm-2"  >
                                        <div class="form-group">
                                        <label>Número<strong class="text-danger">*</strong></label>
                                        <asp:TextBox runat="server" ID="txtNumero" CssClass="form-control" placeholder="Digite o número do endereço" ></asp:TextBox> 
                                        </div>
                                    </div>
                                          
                                    <div class="col-sm-2"  >
                                        <div class="form-group">
                                        <label>Complemento<strong class="text-danger">*</strong></label>
                                        <asp:TextBox runat="server" ID="txtComplemento" CssClass="form-control" placeholder="Digite o complemento do endereço"></asp:TextBox>
                                        </div>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-sm-5"  >
                                        <div class="form-group">
                                        <label>Bairro<strong class="text-danger">*</strong></label>
                                        <asp:TextBox runat="server" ID="txtBairro" CssClass="form-control" placeholder="Digite o bairro"  ></asp:TextBox> 
                                        </div>
                                    </div>
                                    <div class="col-sm-5"  >
                                        <div class="form-group">
                                        <label>Cidade<strong class="text-danger">*</strong></label>
                                        <asp:TextBox runat="server" ID="txtCidade" CssClass="form-control" placeholder="Digite a cidade"  ></asp:TextBox> 
                                        </div>
                                    </div>
                                          
                                    <div class="col-sm-2"  >
                                        <div class="form-group">
                                            <label>Estado<strong class="text-danger">*</strong></label>
                                            <asp:DropDownList id="ddlEstado" runat="server" class="form-control" >
							                    <asp:ListItem Value="  ">  </asp:ListItem>
							                    <asp:ListItem Value="AC">AC</asp:ListItem>
							                    <asp:ListItem Value="AL">AL</asp:ListItem>
							                    <asp:ListItem Value="AM">AM</asp:ListItem>
							                    <asp:ListItem Value="AP">AP</asp:ListItem>
							                    <asp:ListItem Value="BA">BA</asp:ListItem>
							                    <asp:ListItem Value="CE">CE</asp:ListItem>
							                    <asp:ListItem Value="DF">DF</asp:ListItem>
							                    <asp:ListItem Value="ES">ES</asp:ListItem>
							                    <asp:ListItem Value="GO">GO</asp:ListItem>
							                    <asp:ListItem Value="MA">MA</asp:ListItem>
							                    <asp:ListItem Value="MG">MG</asp:ListItem>
							                    <asp:ListItem Value="MS">MS</asp:ListItem>
							                    <asp:ListItem Value="MT">MT</asp:ListItem>
							                    <asp:ListItem Value="PA">PA</asp:ListItem>
							                    <asp:ListItem Value="PB">PB</asp:ListItem>
							                    <asp:ListItem Value="PE">PE</asp:ListItem>
							                    <asp:ListItem Value="PI">PI</asp:ListItem>
							                    <asp:ListItem Value="PR">PR</asp:ListItem>
							                    <asp:ListItem Value="RJ">RJ</asp:ListItem>
							                    <asp:ListItem Value="RN">RN</asp:ListItem>
							                    <asp:ListItem Value="RO">RO</asp:ListItem>
							                    <asp:ListItem Value="RR">RR</asp:ListItem>
							                    <asp:ListItem Value="RS">RS</asp:ListItem>
							                    <asp:ListItem Value="SC">SC</asp:ListItem>
							                    <asp:ListItem Value="SE">SE</asp:ListItem>
							                    <asp:ListItem Value="SP">SP</asp:ListItem>
							                    <asp:ListItem Value="TO">TO</asp:ListItem>					            
                                            </asp:DropDownList>                    
                                        </div>
                                    </div>
                                </div>


                                <div class="row">
                                    <div class="col-sm-12">
                                        <h6 class="text-whitelabel1">Para enviar o link do boleto por e-mail basta informar os e-mail(s) separados por vírgula (,)</h6>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-sm-12"  >
                                        <div class="form-group">
                                        <label>E-Mail(s)<strong class="text-danger">*</strong></label>
                                        <asp:TextBox runat="server" ID="txtEmails" CssClass="form-control" placeholder="Ex.: teste@dominio.com, enviar@teste.com"></asp:TextBox> 
                                        </div>
                                    </div>

                                </div>



                                <div class="row" runat="server" id="dvqrcode" visible="false">
                                    <div class="col-sm-12">
                                        <div class="row">
                                            <div class="col-sm-12">
                                                <div class="form-group">
                                                    <asp:Label runat="server" ID="Label3" Text="QRCode copia e cola"></asp:Label>
                                                    <asp:TextBox runat="server" ID="txtQRCode" CssClass="form-control"></asp:TextBox>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="row d-flex justify-content-center">
                                            <div class="col-sm-4">
                                                <img runat="server" class="img-fluid" id="imgQRcode" src="" alt=""  />                                    
                                            </div>
                                        </div>
                                    </div>
                                </div>



                              </div>
                              <div class="card-footer">
                                  <asp:Button runat="server" ID="btnGerarQRCode" Text="Gerar QR Code" CssClass="btn btn-whitelabel1" onclick="btnGerarQRCode_Click" />
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

        $('#datepickerIniSimulador').datetimepicker({
            format: 'DD/MM/YYYY'
        });
        $('#datepickerFimSimulador').datetimepicker({
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
    var table = $('#tabConsulta').DataTable({
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
        var copyText = document.getElementById("lblPixCopiaCola");
        copyText.select();
        copyText.setSelectionRange(0, 99999); // For mobile devices
        navigator.clipboard.writeText(copyText.value);
        alert("Pix: (" + copyText.value + ") copiado com sucesso!");
    }

</script>


</body>
</html>
