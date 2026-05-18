<%@ Page Language="C#" AutoEventWireup="true" CodeFile="con_transferencias_contas.aspx.cs" Inherits="con_transferencias_contas" %>
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
                              <li class="breadcrumb-item active">Transferências Contas Digitais</li>
                            </ol>
                          </div>
                        </div>


                        <div class="row">
                          <div class="col-12">
                            <div class="card">

                              <div class="card-header bg-whitelabel1">
                                <h3 class="card-title">Transferências entre contas digitais</h3>
                              </div>
                              <div class="card-body">

                                <h3><b>Saldo R$ </b><asp:Label runat="server" ID="lblSaldo" Text="0,00"></asp:Label></h3>

                                <h2>Transferências Contas Digitais</h2>
                                <div class="card card-outline col-12">
                                    <div class="card-body">

                                    <div class="row" runat="server" id="divResposta" visible="false">
                                        <div class="col-sm-12">
                                            <div class="form-group">
                                                <asp:TextBox ID="txtResposta" runat="server" CssClass="form-control" TextMode="MultiLine" Rows="10" ></asp:TextBox>
                                            </div>
                                        </div>
                                    </div>

                                    <div class="row">
                                        <div class="col-sm-6">
                                            <div class="form-group">
                                                <asp:Label runat="server" ID="Label20" Text="Selecione a conta de Origem"></asp:Label>
                                                <asp:DropDownList runat="server" ID="ddlOrigem" CssClass="form-control"></asp:DropDownList>
                                            </div>
                                        </div>
                                        <div class="col-sm-6">
                                            <div class="form-group">
                                                <asp:Label runat="server" ID="Label1" Text="Selecione a conta de Destino"></asp:Label>
                                                <asp:DropDownList runat="server" ID="ddlDestino" CssClass="form-control"></asp:DropDownList>
                                            </div>
                                        </div>

                                    </div>

                                    <div class="row">
                                        <div class="col-sm-6">
                                            <div class="form-group">
                                                <asp:Label runat="server" ID="Label2" Text="Digite o Valor"></asp:Label>
                                                <asp:TextBox runat="server" ID="txtValor" CssClass="form-control"></asp:TextBox>
                                            </div>
                                        </div>
                                        <div class="col-sm-6">
                                            <div class="form-group">
                                                <asp:Label runat="server" ID="Label3" Text="Digite a Descrição"></asp:Label>
                                                <asp:TextBox runat="server" ID="txtDescricao" CssClass="form-control"></asp:TextBox>
                                            </div>
                                        </div>

                                    </div>

                                    </div>
                                </div>


                              </div>
                              <div class="card-footer">
                                <asp:Button runat="server" ID="btnConfirmar" Text="Confirmar Transferência" CssClass="btn btn-whitelabel1" onclick="btnConfirmar_Click" />
                              </div>
                            </div>
                          </div>
                        </div>
                        <div class="row">
                            <div class="col-12">
                            
                                <div class="card card-outline col-12" runat="server">
                                    <div class="card-body">
                                        <div class="row">
                                            <div class="col-sm-12">
                                                <h5 class="text-whitelabel1">Gestão das Transferências</h5>
                                            </div>
                                        </div>

                                        <div class="form-group row col-12">
                                            <label class="col-2 col-form-label">Período de</label>
						                    <div class="col-2">
                                                <div class="input-group date" id="datepickerIni" data-target-input="nearest">
								                    <asp:TextBox ID="txtDataIni" runat="server" CssClass="form-control datetimepicker-input" data-target="#datepickerIni"></asp:TextBox>
                                                    <div class="input-group-append" data-target="#datepickerIni" data-toggle="datetimepicker">
                                                        <div class="input-group-text"><i class="fa fa-calendar"></i></div>
                                                    </div>
                                                </div>
                                            </div>

                                            <label class="col-1 col-form-label">à</label>
						                    <div class="col-2">
                                                <div class="input-group date" id="datepickerFim" data-target-input="nearest">
								                    <asp:TextBox ID="txtDataFim" runat="server" CssClass="form-control datetimepicker-input" data-target="#datepickerFim"></asp:TextBox>
                                                    <div class="input-group-append" data-target="#datepickerFim" data-toggle="datetimepicker">
                                                        <div class="input-group-text"><i class="fa fa-calendar"></i></div>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>                                            

                                        <div class="row">
                                            <div class="col-12">
                                                <asp:Button runat="server" ID="btnPesquisarGestao" Text="Pesquisar" 
                                                    CssClass="btn btn-whitelabel1" onclick="btnPesquisarGestao_Click" />
                                            </div>
                                        </div>


                                        <div class="row"  style="overflow:auto; width: 100%;">
                                            <div class="col-12">
                                                <table id="tabConsulta" class="table table-bordered table-hover">
                                                    <thead>
                                                        <tr>
                                                            <th>ID</th>
                                                            <th>Data</th>
                                                            <th>Origem</th>
                                                            <th>Destino</th>
                                                            <th>Valor</th>
                                                            <th>Descrição</th>
                                                            <th>Status</th>
                                                        </tr>
                                                    </thead>
                                                    <tbody>
                                                        <asp:Repeater runat="server" ID="rptConsulta" OnItemCommand="rptConsulta_OnItemCommand">
                                                            <ItemTemplate>
                                                                <tr>
                                                                    <td>
                                                                        <small><%# DataBinder.Eval(Container.DataItem, "COD_ID")%></small>
                                                                    </td>

                                                                    <td>
                                                                        <small><%# String.Format("{0:dd/MM/yyyy}",DataBinder.Eval(Container.DataItem, "DTA_DATA"))%></small>

                                                                    </td>

                                                                    <td>
                                                                        <small><%# DataBinder.Eval(Container.DataItem, "NOM_ORIGEM")%></small>
                                                                    </td>
                                                                    <td>
                                                                        <small><%# DataBinder.Eval(Container.DataItem, "NOM_DESTINO")%></small>
                                                                    </td>

                                                                    <td>
                                                                        <small><%# String.Format("{0:c2}",DataBinder.Eval(Container.DataItem, "NUM_VALOR"))%></small>

                                                                    </td>

                                                                    <td>
                                                                        <small><%# DataBinder.Eval(Container.DataItem, "DES_DESCRICAO")%></small>
                                                                    </td>
                                                                    <td>
                                                                        <small><%# DataBinder.Eval(Container.DataItem, "FLG_STATUS")%></small>
                                                                    </td>


                                                                </tr>
                                                            </ItemTemplate>
                                                        </asp:Repeater>

                                                    </tbody>
                                                </table>
                                            </div>
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
        $('#datepickerTransferencia').datetimepicker({
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
