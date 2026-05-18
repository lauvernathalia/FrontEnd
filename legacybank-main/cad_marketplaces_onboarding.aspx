<%@ Page Language="C#" AutoEventWireup="true" CodeFile="cad_marketplaces_onboarding.aspx.cs" Inherits="cad_marketplaces_onboarding" %>

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


    <style type="text/css">
        .btn-primary
        {
            height: 26px;
        }
    </style>


</head>
<body class="hold-transition sidebar-mini layout-fixed">
    <form id="frmPrincipal" runat="server">
        <!-- ************************************************************************************************************************************************************************** -->
        <!-- SETOR DE PASSOS DO PROCESSO DE ONBOARDING  -->
        <!-- ************************************************************************************************************************************************************************** -->
        <div class="row col-12" runat="server" id="dvAnalise" visible="false">
                                
            <div class="col">
                <div class="card">
                    <div class="card-body">
                        <small>
                            <center>
                                <i class="fas fa-chart-pie mr-1 fa-2x"></i><br />
                                Análise de Risco<br />     
                                <i runat="server" id="i8" class="fas fa-check-circle mr-1 text-success" visible="false"></i> 
                                <i runat="server" id="i9" class="fas fa-exclamation-circle mr-1 text-warning" visible="false"></i>  
                                <i runat="server" id="i10" class="fas fa-times-circle mr-1 text-danger"></i>  
                            </center>
                        </small>
                    </div>
                </div>
            </div>

        </div>
        <!-- ************************************************************************************************************************************************************************** -->
        <!-- DADOS DO ESTABELECIMENTO  -->
        <!-- ************************************************************************************************************************************************************************** -->


        <div class="row">
            <div class="col-12">

                <div class="card">
                    <div class="card-header bg-whitelabel1">
                        <h3 class="card-title">MARKETPLACE ONBOARDING</h3>
                    </div>
                    <div class="card-body">
                        <h3><span class="float-center badge bg-whitelabel1">Dados do Marketplace</span></h3>

                        <div class="row">
                            <div class="col-sm-2">
                                <div class="form-group">
                                    <label>ID</label>
                                    <asp:TextBox runat="server" id="txtID" cssClass="form-control" Enabled="false"></asp:TextBox>
                                </div>
                            </div>
                            <div class="col-sm-3">
                                <div class="form-group">
                                    <label>Marketplace</label>
                                    <asp:TextBox runat="server" id="txtRazaosocial" cssClass="form-control" Enabled="false"></asp:TextBox>
                                </div>
                            </div>
                            <div class="col-sm-3">
                                <div class="form-group">
                                    <label>E-mail</label>
                                    <asp:TextBox runat="server" id="txtEmail" cssClass="form-control" Enabled="false"></asp:TextBox>
                                </div>
                            </div>
                            <div class="col-sm-2">
                                <div class="form-group">
                                    <label>Tipo</label>
                                    <asp:TextBox runat="server" id="txtTipo" cssClass="form-control" Enabled="false"></asp:TextBox>
                                </div>
                            </div>
                            <div class="col-sm-2">
                                <div class="form-group">
                                    <label>Documento</label>
                                    <asp:TextBox runat="server" id="txtDocumento" cssClass="form-control" Enabled="false"></asp:TextBox>
                                </div>
                            </div>
                        </div>

                        <div class="form-group">
                            <asp:TextBox runat="server" ID="txtResposta" Text="" Visible="false" CssClass="form-control"></asp:TextBox>
                        </div>






                        <!-- Processos de On-boarding -->
                                                
                        <h3><span class="float-center badge bg-whitelabel1 mt-2 mb-2">Processo de Onboarding</span></h3>

                        <div class="row mt-3">
                            <div class="col-sm-4">
                                <div class="form-group">
                                    <label>Adquirentes<strong class="text-danger">*</strong></label>
                                    <asp:DropDownList runat="server" id="ddlAdquirentes" CssClass="form-control" AutoPostBack="True" onselectedindexchanged="ddlAdquirentes_SelectedIndexChanged">
                                    </asp:DropDownList>
                                </div>
                            </div>
                        </div>

                        <div class="row">
                            <div class="col-sm-12">
                                <div class="card">
                                    <div class="card-header bg-whitelabel1">
                                        <h3 class="card-title"><asp:Label runat="server" ID="lblAdquirente" Text=""></asp:Label></h3>
                                    </div>
                                    <div class="card-body">

                                        <!-- *************************************************************************************************************************************** -->
                                        <!-- ADQUIRENTE CAPPTA  -->
                                        <!-- *************************************************************************************************************************************** -->

                                        <div class="row" runat="server" id="divAdquirenteCappta" visible="false">
                                            <div class="col-12">
                                                <div class="row">


                                                    <div class="col-sm-3">
                                                        <div class="form-group">
                                                        <label>Habilitar Adquirente<strong class="text-danger">*</strong></label>
                                                        <asp:DropDownList runat="server" id="ddlCappta" CssClass="form-control">
                                                            <asp:ListItem Value="N">Não</asp:ListItem>
                                                            <asp:ListItem Value="S">Sim</asp:ListItem>
                                                        </asp:DropDownList>
                                                        </div>
                                                    </div>

                                                    <div class="col-sm-3" runat="server" id="dvStatus" visible="false">
                                                        <div class="form-group">
                                                        <label>Status<strong class="text-danger">*</strong></label>
                                                        <asp:TextBox runat="server" ID="txtStatusCappta" CssClass="form-control"></asp:TextBox>
                                                        </div>
                                                    </div>
                                                    <div class="col-sm-3" runat="server" id="dvNomeStatus" visible="true">
                                                        <div class="form-group">
                                                        <label>Status<strong class="text-danger">*</strong></label>
                                                        <asp:TextBox runat="server" ID="txtNomeStatusCappta" CssClass="form-control"></asp:TextBox>
                                                        </div>
                                                    </div>
                                                    <div class="col-sm-3"  >
                                                        <div class="form-group">
                                                        <label>Token<strong class="text-danger">*</strong></label>
                                                        <asp:TextBox runat="server" ID="txtTokenCappta" CssClass="form-control"></asp:TextBox>
                                                        </div>
                                                    </div>
                                                    <div class="col-sm-3">
                                                        <div class="form-group">
                                                            <label>Natureza<strong class="text-danger">*</strong></label>
                                                            <asp:DropDownList runat="server" id="ddlNatureza" CssClass="form-control">
                                                            </asp:DropDownList>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>

                                        </div>




                                    </div>
                                    <div class="card-footer">
                                        <asp:Button runat="server" ID="btnSalvar" CssClass="btn btn-whitelabel1" 
                                            Text="Salvar e enviar os dados" onclick="btnSalvar_Click"/>
                                    </div>
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
