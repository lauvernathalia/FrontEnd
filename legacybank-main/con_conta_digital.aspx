<%@ Page Language="C#" AutoEventWireup="true" CodeFile="con_conta_digital.aspx.cs" Inherits="con_conta_digital" Async="true" %>

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
                              <li class="breadcrumb-item"><a href="#">Minha Conta</a></li>
                              <li class="breadcrumb-item active">Conta Digital</li>
                            </ol>
                          </div>
                        </div>

                        <div class="row">
                          <div class="col-12">

                            <div class="card">

                              <div class="card-header">
                                <h3 class="card-title">Conta Digital</h3>
                              </div>
                              <div class="card-body">

                                <h3><span class="float-center badge bg-whitelabel1">1. Dados da Conta</span></h3>

                                <div class="row">
                                    <div class="col-sm-2">
                                        <div class="form-group">
                                        <label>Status</label>
                                        <asp:TextBox runat="server" ID="txtStatusAsaas" CssClass="form-control" Enabled="false"></asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="col-sm-2">
                                        <div class="form-group">
                                            <label>Banco</label>
                                            <asp:TextBox id="txtBanco" runat="server" CssClass="form-control" Enabled="false"></asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="col-sm-2"  >
                                        <div class="form-group">
                                            <label>Agência</label>
                                            <asp:TextBox id="txtAgencia" runat="server" CssClass="form-control" Enabled="false"></asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="col-sm-2"  >
                                        <div class="form-group">
                                            <label>Conta Corrente</label>
                                            <asp:TextBox id="txtConta" runat="server" CssClass="form-control" Enabled="false"></asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="col-sm-4"  >
                                        <div class="form-group">
                                            <label>WalletID</label>
                                            <asp:TextBox id="txtWalletID" runat="server" CssClass="form-control" Enabled="false"></asp:TextBox>
                                        </div>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-sm-12">
                                        <div class="form-group">
                                            <label>Token</label>
                                            <asp:TextBox runat="server" ID="txtTokenAsaas" CssClass="form-control" Enabled="false"></asp:TextBox> 
                                        </div>
                                    </div>
                                </div>

                                <div class="row">
                                    <div class="col-sm-4"  >
                                        <div class="form-group">
                                            <label>Chave Pix</label>
                                            <asp:TextBox id="txtChavePix" runat="server" CssClass="form-control" Enabled="false"></asp:TextBox>
                                        </div>
                                    </div>

                                    <div class="col-sm-2">
                                        <label><strong class="text-danger"> </strong></label><br /><br />
                                        <asp:LinkButton ID="lbkCopiar" runat="server" CssClass="btn btn-default" ToolTip="copiar" OnClientClick="myFunctionChavePix()"><i class="fas fa-copy"></i></asp:LinkButton>
                                        <asp:LinkButton ID="lbkGerar" runat="server" CssClass="btn btn-default" ToolTip="gerar" OnClick="lbkGerar_Click"><i class="fas fa-key"></i></asp:LinkButton>
                                    </div>
                                </div>

                                <h3><span class="float-center badge bg-whitelabel1">2. Dados da Antecipação Automática</span></h3>
                                <div class="row">
                                    <div class="col-sm-6">
                                        <div class="form-group">
                                            <label>Antecipação Automática</label>
                                            <div class="input-group">
                                                <asp:DropDownList runat="server" ID="ddlAntecipacaoAutomatica" CssClass="form-control">
                                                    <asp:ListItem Value="N" Text="Não"></asp:ListItem>
                                                    <asp:ListItem Value="S" Text="Sim"></asp:ListItem>
                                                </asp:DropDownList>
                                                <span class="input-group-append">
                                                <asp:Button runat="server" ID="btnAntecipacaoAutomatica" 
                                                    Text="Clique aqui para Atualizar Status" CssClass="btn btn-whitelabel1" 
                                                    onclick="btnAntecipacaoAutomatica_Click" />
                                                </span>
                                            </div>
                                        </div>
                                    </div>
                                </div>

                                <h3><span class="float-center badge bg-whitelabel1">3. Outras Operações/Funções </span></h3>

                                <div class="row">
                                    <div class="col-sm-12">
                                        <div class="form-group">
                                        <label class="text-whitelabel1">Só será possível excluir quando o saldo estiver zerado ou inferior a R$10,00<strong class="text-danger">*</strong></label>
                                        <label><b>NOTA:</b> Se remover com saldo até R$10,00, o valor será perdido. Saldo superior a R$10,00 a plataforma não deixará você prosseguir com a remoção</label>
                                        <asp:TextBox runat="server" ID="txtMotivo" CssClass="form-control" placeholder="Digite o motivo da exclusão da conta. Ex.: Solicitação do correntista"></asp:TextBox>
                                        <asp:Button runat="server" ID="btnExcluir" CssClass="btn btn-danger" 
                                                Text = "Excluir Conta" onclick="btnExcluir_Click" />
                                        </div>
                                    </div>

                                </div>                                           



                              </div>

                              <div class="card-footer" style="overflow:auto; width: 100%; ">

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

<script type="text/javascript" src="../plugins/bootstrap-colorpicker/js/bootstrap-colorpicker.min.js"></script>

<script>
    $(function () {

        //Colorpicker
        $('.my-colorpicker1').colorpicker()

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


<script type="text/javascript">

$(document).ready(function () {
    var table = $('#tblConsulta').DataTable({
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
