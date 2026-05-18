<%@ Page Language="C#" AutoEventWireup="true" CodeFile="cad_representantes_pagseguro.aspx.cs" Inherits="cad_representantes_pagseguro" %>

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
                        <h3 class="card-title">REPRESENTANTE PAGSEGURO</h3>
                    </div>
                    <div class="card-body">
                        <h3><span class="float-center badge bg-whitelabel1">1. Dados do Pagseguro</span></h3>

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
                                    <asp:TextBox runat="server" id="txtRazaosocial" cssClass="form-control" Enabled="false"></asp:TextBox>
                                </div>
                            </div>
                        </div>

                        <div class="row">
                            <div class="col-sm-6"  >
                                <div class="form-group">
                                <label>Pagseguro<strong class="text-danger">*</strong> (Habilitar e gerenciar adquirente pagseguro)</label>
                                <asp:DropDownList runat="server" id="ddlPagseguro" CssClass="form-control">
                                    <asp:ListItem Value="0">Não</asp:ListItem>
                                    <asp:ListItem Value="1">Sim</asp:ListItem>
                                </asp:DropDownList>
                                </div>
                            </div>

                            <div class="col-sm-3"  >
                                <div class="form-group">
                                <label>Código Ativação<strong class="text-danger">*</strong></label>
                                <asp:TextBox runat="server" ID="txtCodigoAtivacao" CssClass="form-control" placeholder="Código de ativação" ></asp:TextBox> 
                                </div>
                            </div>
                                          

                            <div class="col-sm-3"  >
                                <div class="form-group">
                                <label>Conta verificada e ativada<strong class="text-danger">*</strong></label>
                                <asp:DropDownList runat="server" id="ddlVerificadaAtivada" CssClass="form-control">
                                    <asp:ListItem Value="Sim">Sim</asp:ListItem>
                                    <asp:ListItem Value="Não">Não</asp:ListItem>
                                    <asp:ListItem Value="Pendente de Documentos">Pendente de Documentos</asp:ListItem>
                                    <asp:ListItem Value="Dados Inválidos">Dados Inválidos</asp:ListItem>
                                    <asp:ListItem Value="Negada">Negada</asp:ListItem>
                                </asp:DropDownList>
                                </div>
                            </div>


                        </div>


                        <div class="row">
                            <div class="col-sm-4"  >
                                <div class="form-group">
                                <label>ID<strong class="text-danger">*</strong></label>
                                <asp:TextBox runat="server" ID="txtIDPagseguro" CssClass="form-control" placeholder="ID Pagseguro" ></asp:TextBox> 
                                </div>
                            </div>
                                          

                            <div class="col-sm-4"  >
                                <div class="form-group">
                                <label>E-mail<strong class="text-danger">*</strong></label>
                                <asp:TextBox runat="server" ID="txtEmailPagseguro" CssClass="form-control" placeholder="E-mail ativação pagseguro" ></asp:TextBox>
                                </div>
                            </div>

                            <div class="col-sm-4"  >
                                <div class="form-group">
                                <label>Token<strong class="text-danger">*</strong></label>
                                <asp:TextBox runat="server" ID="txtTokenPagseguro" CssClass="form-control" placeholder="Token Pagseguro" ></asp:TextBox>
                                </div>
                            </div>

                        </div>

                        <div class="row">
                            <div class="col-sm-12"  >
                                <div class="form-group">
                                <label>Plano<strong class="text-danger">*</strong></label>
                                <asp:DropDownList runat="server" id="ddlPlano" CssClass="form-control">
                                </asp:DropDownList>
                                </div>
                            </div>
                        </div>


                        <h3><span class="float-center badge bg-whitelabel1">2. Dados do Forma de Recebimento</span></h3>


                        <div class="row" runat="server" id="dvFormaRecebimento">
                            <div class="col-sm-12">
                                <div class="form-group">
                                <label>Forma de Recebimento<strong class="text-danger">*</strong></label>
                                <asp:DropDownList runat="server" id="ddlFormaRecebimento" CssClass="form-control">
                                    <asp:ListItem Value="O">Outra conta bancária</asp:ListItem>
                                    <asp:ListItem Value="P">PagBank</asp:ListItem>
                                </asp:DropDownList>
                                </div>
                            </div>
                        </div>

                        <div class="row" runat="server" id="dvDadosBancarios">
                            <div class="col-sm-6">
                                <div class="form-group">
                                <label>Instituição Financeira<strong class="text-danger">*</strong></label>
                                <asp:DropDownList runat="server" id="ddlInstituicaoFinanceira" CssClass="form-control">
                                </asp:DropDownList>
                                </div>
                            </div>

                            <div class="col-sm-6">
                                <div class="form-group">
                                <label>Tipo de Conta<strong class="text-danger">*</strong></label>
                                <asp:DropDownList runat="server" id="ddlTipoConta" CssClass="form-control">
                                    <asp:ListItem Value="C">Conta Corrente</asp:ListItem>
                                    <asp:ListItem Value="P">Conta Poupança</asp:ListItem>
                                </asp:DropDownList>
                                </div>
                            </div>

                            <div class="col-sm-4">
                                <div class="form-group">
                                <label>Agência<strong class="text-danger">*</strong></label>
                                <asp:TextBox runat="server" ID="txtAgencia" CssClass="form-control" placeholder="Número da Agencia" ></asp:TextBox>
                                </div>
                            </div>

                            <div class="col-sm-4">
                                <div class="form-group">
                                <label>Dígito<strong class="text-danger">*</strong></label>
                                <asp:TextBox runat="server" ID="txtDigitoAgencia" CssClass="form-control" placeholder="Dígito" ></asp:TextBox>
                                </div>
                            </div>
                            <div class="col-sm-4">
                                <div class="form-group">
                                <label>Conta<strong class="text-danger">*</strong></label>
                                <asp:TextBox runat="server" ID="txtConta" CssClass="form-control" placeholder="Número da Conta" ></asp:TextBox>
                                </div>
                            </div>
                            <div class="col-sm-4">
                                <div class="form-group">
                                <label>Dígito<strong class="text-danger">*</strong></label>
                                <asp:TextBox runat="server" ID="txtDigitoConta" CssClass="form-control" placeholder="Dígito" ></asp:TextBox>
                                </div>
                            </div>


                        </div>

                    </div>

                    <div class="card-footer">
                        <asp:Button runat="server" ID="btnSalvar" CssClass="btn btn-whitelabel1" Text="Salvar" onclick="btnSalvar_Click"/>
                        <asp:Button runat="server" ID="btnCancelar" CssClass="btn btn-danger float-right" Text="Cancelar" onclick="btnCancelar_Click"/>
                    </div>
                </div>            
            
            </div>
        </div>
    </form>

<script type="text/javascript" src="../plugins/datatables/jquery.dataTables.js"></script>
<script type="text/javascript" src="../plugins/datatables-bs4/js/dataTables.bootstrap4.js"></script>

<script src="../plugins/inputmask/jquery.inputmask.bundle.js"></script>
<script src="../plugins/moment/moment.min.js"></script>

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
