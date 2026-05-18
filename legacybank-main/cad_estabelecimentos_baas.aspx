<%@ Page Language="C#" AutoEventWireup="true" CodeFile="cad_estabelecimentos_baas.aspx.cs" Inherits="cad_estabelecimentos_baas" %>

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
                        <h3 class="card-title">ESTABELECIMENTO BAAS</h3>
                    </div>
                    <div class="card-body">
                        <h3><span class="float-center badge bg-whitelabel1">1. Dados do BaaS</span></h3>

                        <div class="row">
                            <div class="col-sm-2">
                                <div class="form-group">
                                    <label>ID</label>
                                    <asp:TextBox runat="server" id="txtID" cssClass="form-control" Enabled="false"></asp:TextBox>
                                </div>
                            </div>
                            <div class="col-sm-5">
                                <div class="form-group">
                                    <label>Estabelecimento</label>
                                    <asp:TextBox runat="server" id="txtRazaosocial" cssClass="form-control" Enabled="false"></asp:TextBox>
                                </div>
                            </div>
                            <div class="col-sm-5">
                                <div class="form-group">
                                    <label>E-mail</label>
                                    <asp:TextBox runat="server" id="txtEmail" cssClass="form-control" Enabled="false"></asp:TextBox>
                                </div>
                            </div>
                        </div>

                        <div class="row">
                            <div class="col-sm-4">
                                <div class="form-group">
                                    <label>Tipo</label>
                                    <asp:TextBox runat="server" id="txtTipo" cssClass="form-control" Enabled="false"></asp:TextBox>
                                </div>
                            </div>
                            <div class="col-sm-8">
                                <div class="form-group">
                                    <label>Documento</label>
                                    <asp:TextBox runat="server" id="txtDocumento" cssClass="form-control" Enabled="false"></asp:TextBox>
                                </div>
                            </div>
                        </div>


                        <div class="row">
                            <div class="col-sm-4">
                                <div class="form-group">
                                <label>BaaS<strong class="text-danger">*</strong> (Habilitar e gerenciar BaaS)</label>
                                <asp:DropDownList runat="server" id="ddlBaaS" CssClass="form-control">
                                    <asp:ListItem Value="N">Não</asp:ListItem>
                                    <asp:ListItem Value="S">Sim</asp:ListItem>
                                </asp:DropDownList>
                                </div>
                            </div>
                            <div class="col-sm-2" runat="server" id="dvStatus" visible="false">
                                <div class="form-group">
                                <label>Status<strong class="text-danger">*</strong></label>
                                <asp:TextBox runat="server" ID="txtStatus" CssClass="form-control"></asp:TextBox>
                                </div>
                            </div>
                            <div class="col-sm-2" runat="server" id="dvNomeStatus" visible="true">
                                <div class="form-group">
                                <label>Status<strong class="text-danger">*</strong></label>
                                <asp:TextBox runat="server" ID="txtNomeStatus" CssClass="form-control"></asp:TextBox>
                                </div>
                            </div>

                            <div class="col-sm-3" runat="server" id="dvTokenAsaas" visible="false">
                                <div class="form-group">
                                <label>Token<strong class="text-danger">*</strong></label>
                                <asp:TextBox runat="server" ID="txtToken" CssClass="form-control" Visible="false"></asp:TextBox>
                                </div>
                            </div>

                            <div class="col-sm-3" runat="server" id="dvIDContaAsaas" visible="false"  >
                                <div class="form-group">
                                <label>ID Conta<strong class="text-danger">*</strong></label>
                                <asp:TextBox runat="server" ID="txtIDConta" CssClass="form-control" Visible="false"></asp:TextBox>
                                </div>
                            </div>
                            <div class="col-sm-2"  >
                                <div class="form-group">
                                <label>Agência<strong class="text-danger">*</strong></label>
                                <asp:TextBox runat="server" ID="txtAgencia" CssClass="form-control"></asp:TextBox>
                                </div>
                            </div>
                            <div class="col-sm-2"  >
                                <div class="form-group">
                                <label>Conta<strong class="text-danger">*</strong></label>
                                <asp:TextBox runat="server" ID="txtConta" CssClass="form-control"></asp:TextBox>
                                </div>
                            </div>
                            <div class="col-sm-2"  >
                                <div class="form-group">
                                <label>Dígito Conta<strong class="text-danger">*</strong></label>
                                <asp:TextBox runat="server" ID="txtDigitoConta" CssClass="form-control"></asp:TextBox>
                                </div>
                            </div>
                            <div class="col-sm-4"  >
                                <div class="form-group">
                                <label>Chave Pix<strong class="text-danger">*</strong></label>
                                <asp:TextBox runat="server" ID="txtChavePix" CssClass="form-control"></asp:TextBox>
                                </div>
                            </div>
                            <div class="col-sm-2">
                                <label><strong class="text-danger"> </strong></label><br /><br />
                                <asp:LinkButton ID="lbkCopiar" runat="server" CssClass="btn btn-default" ToolTip="copiar"><i class="fas fa-copy"></i></asp:LinkButton>
                                <asp:LinkButton ID="lbkGerar" runat="server" CssClass="btn btn-default" 
                                    ToolTip="gerar" onclick="lbkGerar_Click"><i class="fas fa-key"></i></asp:LinkButton>


                            </div>
                            <div class="col-sm-6">
                                <div class="form-group">
                                <label>Wallet ID<strong class="text-danger">*</strong></label>
                                <asp:TextBox runat="server" ID="txtWalletID" CssClass="form-control"></asp:TextBox>
                                </div>
                            </div>



                            <div class="col-sm-12">
                                <br />
                            </div>
                            <div class="col-sm-12" style="display:block;">
                                <div class="form-group">
                                    <asp:TextBox runat="server" ID="txtResposta" CssClass="form-control" TextMode="MultiLine" Rows="5" Visible="false"></asp:TextBox>
                                </div>
                            </div>

                            <div class="col-sm-12" style="display:none;">
                                <asp:Button runat="server" ID="btnPendentes" CssClass="btn btn-whitelabel1" Text="Documentos Pendentes" onclick="btnPendentes_Click"/>
                                <asp:Button runat="server" ID="btnTaxas" CssClass="btn btn-whitelabel1" Text="Taxas" onclick="btnTaxas_Click"/>
                            </div>



                        </div>




                        <div class="col-sm-12">
                            <asp:Button runat="server" ID="btnSalvar" CssClass="btn btn-whitelabel1" Text="Criar conta digital e salvar os dados" onclick="btnSalvar_Click"/>
                            <asp:Button runat="server" ID="btnCancelar" CssClass="btn btn-danger float-right" Text="Cancelar" onclick="btnCancelar_Click"/>
                        </div>

                        </div>
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
