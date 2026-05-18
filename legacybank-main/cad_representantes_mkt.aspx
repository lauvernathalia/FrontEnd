<%@ Page Language="C#" AutoEventWireup="true" CodeFile="cad_representantes_mkt.aspx.cs" Inherits="cad_representantes_mkt" %>

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
                <h3 class="card-title">REPRESENTANTE</h3>
                </div>
                <div class="card-body">

                <div class="row">
                    <div class="col-sm-4">
                        <div class="form-group">
                            <label class="col-sm-12 col-form-label">ID</label>
                            <asp:Label id="lblID" runat="server" class="form-control"></asp:Label>
                        </div>
                    </div>
                    <div class="col-sm-4">
                        <div class="form-group">
                            <label>Tipo de pessoa (PF ou PJ)<strong class="text-danger">*</strong></label>
                            <asp:DropDownList runat="server" id="ddlTipoFJ" CssClass="form-control" 
                                    AutoPostBack="True" onselectedindexchanged="ddlTipoFJ_SelectedIndexChanged">
                                <asp:ListItem Value="PF">Pessoa Física</asp:ListItem>
                                <asp:ListItem Value="PJ">Pessoa Jurídica</asp:ListItem>
                            </asp:DropDownList>
                        </div>
                    </div>


                </div>

                <div class="row">
                    <div class="col-sm-4">
                        <div class="form-group">
                            <label class="col-sm-12 col-form-label">Nome Responsável</label>
                            <asp:TextBox id="txtNome" runat="server" class="form-control" placeholder="Nome do Responsável"></asp:TextBox>
                        </div>
                    </div>
                    <div class="col-sm-4">
                        <div class="form-group">
                            <label class="col-sm-12 col-form-label">Sobrenome Responsável</label>
                            <asp:TextBox id="txtSobrenome"  runat="server" class="form-control" 
                                placeholder="Sobrenome do Responsável"></asp:TextBox>
                        </div>
                    </div>
                    <div class="col-sm-4">
                        <div class="form-group">
                            <label class="col-sm-12 col-form-label">CPF Responsável</label>
                            <asp:TextBox id="txtDocumento"  runat="server" class="form-control" 
                                placeholder="CPF"></asp:TextBox>
                        </div>
                    </div>
                </div>
                <div class="row">
                    <div class="col-sm-6">
                        <div class="form-group">
                            <label class="col-sm-12 col-form-label">E-mail do Responsável</label>
                            <asp:TextBox id="txtEmail" runat="server" class="form-control" placeholder="E-mail do Responsável"></asp:TextBox>
                        </div>
                    </div>
                    <div class="col-sm-6">
                        <div class="form-group">
                            <label class="col-sm-12 col-form-label">Confirme o E-mail </label>
                            <asp:TextBox id="txtEmailC" runat="server" class="form-control" 
                                placeholder="Confirme o E-mail do Responsável" 
                                ontextchanged="txtEmailC_TextChanged" AutoPostBack="True"></asp:TextBox>
                                <i class="fas fa-check text-success" runat="server" id="faVerificadoEmail" visible="false"></i>
                        </div>
                    </div>
                </div>
                <div class="row" runat="server" id="dvSenha" visible="false">
                    <div class="col-sm-6">
                        <div class="form-group">
                            <label class="col-sm-12 col-form-label">Senha</label>
                            <asp:TextBox id="txtSenha" runat="server" class="form-control" placeholder="Informe sua Senha"></asp:TextBox>
                        </div>
                    </div>
                    <div class="col-sm-6">
                        <div class="form-group">
                            <label class="col-sm-12 col-form-label">Confirme a Senha </label>
                            <asp:TextBox id="txtSenhaC" runat="server" class="form-control" 
                                placeholder="Confirme a Senha" ontextchanged="txtSenhaC_TextChanged" 
                                AutoPostBack="True"></asp:TextBox>
                            <i class="fas fa-check text-success" runat="server" id="faVerificadoSenha" visible="false"></i>
                        </div>
                    </div>
                </div>
                <div runat="server" id="dvEmpresa" visible="false">

                    <div class="row">
                        <div class="col-sm-6">
                            <div class="form-group">
                            <label>Razão Social<strong class="text-danger">*</strong></label>
                            <asp:TextBox runat="server" id="txtRazaoSocial" cssClass="form-control"  placeholder="Razão Social da Empresa"></asp:TextBox>
                            </div>
                        </div>
                        <div class="col-sm-6">
                            <div class="form-group">
                            <label>Nome Fantasia<strong class="text-danger">*</strong></label>
                            <asp:TextBox runat="server" id="txtFantasia" CssClass="form-control" placeholder="Nome Fantasia da Empresa">
                            </asp:TextBox>
                            </div>
                        </div>
                    </div>

                    <div class="row">
                        <div class="col-sm-4">
                            <div class="form-group">
                            <label>CNPJ<strong class="text-danger">*</strong></label>
                            <asp:TextBox runat="server" id="txtCNPJ" cssClass="form-control"  placeholder="00.000.000/0000-00" data-inputmask='"mask": "99.999.999/9999-99"' data-mask></asp:TextBox>
                            </div>
                        </div>
                        <div class="col-sm-4">
                            <div class="form-group">
                            <label>Telefone Empresa<strong class="text-danger">*</strong></label>
                            <asp:TextBox runat="server" id="txtTelefoneEmpresa" CssClass="form-control" placeholder="(99) 99999-9999" data-inputmask='"mask": "(99) 99999-9999"' data-mask>
                            </asp:TextBox>
                            </div>
                        </div>
                        <div class="col-sm-4">
                            <div class="form-group">
                            <label>E-mail Empresa<strong class="text-danger">*</strong></label>
                            <asp:TextBox runat="server" id="txtEmailEmpresa" CssClass="form-control" placeholder="nome@dominio.com.br">
                            </asp:TextBox>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="row">
                    <div class="col-sm-6"  >
                        <div class="form-group">
                        <label>CEP<strong class="text-danger">*</strong></label>
                        <asp:TextBox runat="server" ID="txtCEP" CssClass="form-control" 
                                placeholder="00000000" data-inputmask='"mask": "99999-999"' data-mask  
                                AutoPostBack="True" ontextchanged="txtCEP_TextChanged"></asp:TextBox> 
                        </div>
                    </div>
                    <div class="col-sm-6"  >
                        <div class="form-group">
                        <label>Endereço<strong class="text-danger">*</strong></label>
                        <asp:TextBox runat="server" ID="txtEndereco" CssClass="form-control" placeholder="Digite o seu endereço" ></asp:TextBox> 
                        </div>
                    </div>

                </div>
                <div class="row">
                    <div class="col-sm-6"  >
                        <div class="form-group">
                        <label>Número<strong class="text-danger">*</strong></label>
                        <asp:TextBox runat="server" ID="txtNumero" CssClass="form-control" placeholder="Digite o número do endereço" ></asp:TextBox> 
                        </div>
                    </div>
                                          
                    <div class="col-sm-6"  >
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
                <div class="card-footer">
                        <asp:Button runat="server" ID="btnSalvar" CssClass="btn btn-whitelabel1" Text="Salvar" onclick="btnSalvar_Click" Visible="false"/>
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
