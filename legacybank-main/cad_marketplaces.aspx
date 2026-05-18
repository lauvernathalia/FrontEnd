<%@ Page Language="C#" AutoEventWireup="true" CodeFile="cad_marketplaces.aspx.cs" Inherits="cad_marketplaces" Async="true" EnableSessionState="ReadOnly" MaintainScrollPositionOnPostback="true" %>

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
                        <h3 class="card-title">MARKETPLACE</h3>
                    </div>
                    <div class="card-body">
                        <h3><span class="float-center badge bg-whitelabel1">1. Dados do Marketplace</span></h3>

                        <div class="row" runat="server" id="dvImportar" visible="false">
                            <div class="col-sm-12">
                                <div class="form-group">
                                    <label>Importar dados do estabelecimento selecionado</label>
                                    <div class="input-group input-group-sm">
                                    <asp:DropDownList runat="server" id="ddlEstabelecimento" CssClass="form-control">
                                    </asp:DropDownList>
                                    <span class="input-group-append">
                                        <asp:Button runat="server" ID="btnImportar" CssClass="btn btn-flat btn-secondary" Text="Importar" onclick="btnImportar_Click"/>
                                    </span>
                                    </div>
                                </div>
                            </div>
                        </div>


                        <div class="row">
                            <div class="col-sm-2">
                                <div class="form-group">
                                    <label>ID</label>
                                    <asp:TextBox runat="server" id="txtID" cssClass="form-control" Enabled="false"></asp:TextBox>
                                </div>
                            </div>
                            <div class="col-sm-4">
                                <div class="form-group">
                                    <label>Tipo de pessoa (PF ou PJ)<strong class="text-danger">*</strong></label>
                                    <asp:DropDownList runat="server" id="ddlTipoFJ" CssClass="form-control" 
                                        onselectedindexchanged="ddlTipoFJ_SelectedIndexChanged" 
                                        AutoPostBack="True">
                                        <asp:ListItem Value="PF">Pessoa Física</asp:ListItem>
                                        <asp:ListItem Value="PJ">Pessoa Jurídica</asp:ListItem>
                                    </asp:DropDownList>
                                </div>
                            </div>
                            <div class="col-sm-6">
                                <div class="form-group">
                                <label>Você visitou este marketplace presencialmente?<strong class="text-danger">*</strong></label>
                                <asp:DropDownList runat="server" id="ddlPresencial" CssClass="form-control">
                                    <asp:ListItem Value="S">Sim</asp:ListItem>
                                    <asp:ListItem Value="N">Não</asp:ListItem>
                                </asp:DropDownList>
                                </div>
                            </div>
                        </div>

                        <div class="row">
                            <div class="col-sm-6">
                                <div class="form-group">
                                <b><asp:Label runat="server" id="lblRazaoSocial" Text="Nome"></asp:Label></b><strong class="text-danger">*</strong> 
                                <asp:TextBox runat="server" id="txtRazaoSocial" cssClass="form-control"  placeholder="Razão Social ou Nome"></asp:TextBox>
                                </div>
                            </div>
                            <div class="col-sm-6">
                                <div class="form-group">
                                <b><asp:Label runat="server" id="lblFantasia" Text="Apelido"></asp:Label></b><strong class="text-danger">*</strong> 
                                <asp:TextBox runat="server" id="txtFantasia" CssClass="form-control" placeholder="Nome Fantasia ou Apelido">
                                </asp:TextBox>
                                </div>
                            </div>
                        </div>


                        <div class="row">
                            <div class="col-sm-3">
                                <div class="form-group">
                                <b><asp:Label runat="server" id="lblCNPJ" Text="CPF"></asp:Label></b><strong class="text-danger">*</strong> 
                                <asp:TextBox runat="server" id="txtCNPJ" cssClass="form-control"></asp:TextBox>
                                </div>
                            </div>
                            <div class="col-sm-3">
                                <div class="form-group">
                                <label>Telefone Estabelecimento<strong class="text-danger">*</strong></label>
                                <asp:TextBox runat="server" id="txtTelefoneEmpresa" CssClass="form-control" placeholder="(99) 99999-9999" data-inputmask='"mask": "(99) 99999-9999"' data-mask>
                                </asp:TextBox>
                                </div>
                            </div>
                            <div class="col-sm-3">
                                <div class="form-group">
                                <label>E-mail Estabelecimento<strong class="text-danger">*</strong></label>
                                <asp:TextBox runat="server" id="txtEmailEmpresa" CssClass="form-control" placeholder="nome@dominio.com.br">
                                </asp:TextBox>
                                </div>
                            </div>
                            <div class="col-sm-3">
                                <div class="form-group">
                                    <b><asp:Label runat="server" id="lblDataAbertura" Text="Data Início Atividade"></asp:Label></b><strong class="text-danger">*</strong> 
                                    <asp:TextBox runat="server" id="txtDataAbertura" CssClass="form-control" placeholder="00/00/0000" data-inputmask='"mask": "99/99/9999"' data-mask >
                                    </asp:TextBox>
                                </div>
                            </div>

                        </div>


                        <!-- Falta Atividade econômica e mcc visitou este REPRESENTANTE presencialmente e patrimonio e tipo da empresa -->

                        <div class="row">
                            <div class="col-sm-3">
                                <div class="form-group">
                                    <label>Tipo do Representante<strong class="text-danger">*</strong></label>
                                    <asp:DropDownList runat="server" id="ddlTipoEmpresa" CssClass="form-control">
                                        <asp:ListItem Value="Pessoa Física">Pessoa Física</asp:ListItem>
                                        <asp:ListItem Value="MEI">MEI</asp:ListItem>
                                        <asp:ListItem Value="Empresário Individual">Empresário Individual</asp:ListItem>
                                        <asp:ListItem Value="Sociedade Limitada Unipessoal">Sociedade Limitada Unipessoal</asp:ListItem>
                                        <asp:ListItem Value="Sociedade Empresária Limitada">Sociedade Empresária Limitada</asp:ListItem>
                                        <asp:ListItem Value="Sociedade Simples">Sociedade Simples</asp:ListItem>
                                        <asp:ListItem Value="Sociedade Anônima">Sociedade Anônima</asp:ListItem>
                                    </asp:DropDownList>
                                </div>
                            </div>

                            <div class="col-sm-3">
                                <div class="form-group">
                                    <label>Atividade Econômica<strong class="text-danger">*</strong></label>
                                    <asp:DropDownList runat="server" id="ddlAtividadeEconomica" CssClass="form-control">
                                    </asp:DropDownList>
                                </div>
                            </div>


                            <div class="col-sm-3">
                                <div class="form-group">
                                <label>Faturamento<strong class="text-danger">*</strong></label>
                                <asp:TextBox runat="server" id="txtFaturamento" cssClass="form-control"  placeholder="0,00"></asp:TextBox>
                                </div>
                            </div>
                            <div class="col-sm-3">
                                <div class="form-group">
                                <label>Patrimônio<strong class="text-danger">*</strong></label>
                                <asp:TextBox runat="server" id="txtPatrimonio" CssClass="form-control" placeholder="0,00">
                                </asp:TextBox>
                                </div>
                            </div>

                        </div>
                        <h3><span class="float-center badge bg-whitelabel1">2. Dados do Endereço</span></h3>

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

                        <h3><span class="float-center badge bg-whitelabel1">3. Dados do Responsável</span></h3>

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
                                    <asp:TextBox id="txtSobrenome"  runat="server" class="form-control" placeholder="Sobrenome do Responsável"></asp:TextBox>
                                </div>
                            </div>
                            <div class="col-sm-4">
                                <div class="form-group">
                                    <label class="col-sm-12 col-form-label">CPF Responsável</label>
                                    <asp:TextBox id="txtDocumento"  runat="server" class="form-control" placeholder="CPF do Responsável"></asp:TextBox>
                                </div>
                            </div>
                        </div>

                        <!-- Falta dados Nome da Mãe e renda bruta e data de nascimento e pessoa politicamente exposta -->

                        <div class="row">
                            <div class="col-sm-3"  >
                                <div class="form-group">
                                <label>Data Nascimento<strong class="text-danger">*</strong></label>
                                <asp:TextBox runat="server" ID="txtNascimento" CssClass="form-control" placeholder="00/00/0000" ></asp:TextBox> 
                                </div>
                            </div>
                            <div class="col-sm-3"  >
                                <div class="form-group">
                                <label>Nome completo da mãe<strong class="text-danger">*</strong></label>
                                <asp:TextBox runat="server" ID="txtNomeMae" CssClass="form-control" placeholder="Digite o nome completo da mãe" ></asp:TextBox> 
                                </div>
                            </div>
                            <div class="col-sm-3">
                                <div class="form-group">
                                <label>Renda Mensal<strong class="text-danger">*</strong></label>
                                <asp:TextBox runat="server" id="txtRendaMensalPF" CssClass="form-control" placeholder="0,00">
                                </asp:TextBox>
                                </div>
                            </div>

                        </div>                    
                        <div class="row">
                            <div class="col-sm-4">
                                <div class="form-group">
                                    <label class="col-sm-12 col-form-label">E-mail do Responsável</label>
                                    <asp:TextBox id="txtEmailResponsavel" runat="server" class="form-control" placeholder="E-mail do Responsável"></asp:TextBox>
                                </div>
                            </div>
                            <div class="col-sm-4">
                                <div class="form-group">
                                    <label class="col-sm-12 col-form-label">Celular do Responsável</label>
                                    <asp:TextBox id="txtCelular" runat="server" class="form-control" placeholder="Celular do Responsável"></asp:TextBox>
                                </div>
                            </div>
                            <div class="col-sm-4">
                                <div class="form-group">
                                <label>É pessoa politicamente exposta?<strong class="text-danger">*</strong></label>
                                <asp:DropDownList runat="server" id="ddlPoliticamenteExposta" CssClass="form-control">
                                    <asp:ListItem Value="S">Sim</asp:ListItem>
                                    <asp:ListItem Value="N">Não</asp:ListItem>
                                </asp:DropDownList>
                                </div>
                            </div>


                        </div>
                        <div runat="server" id="dvUsuario" visible="false">
                            <h3><span class="float-center badge bg-whitelabel1">4. Dados do Usuário</span></h3>

                            <div class="row">
                                <div class="col-sm-4">
                                    <div class="form-group">
                                        <label class="col-sm-12 col-form-label">Nome do Usuário Principal</label>
                                        <asp:TextBox id="txtNomeUsuario" runat="server" class="form-control" placeholder="Nome do Usuário"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="col-sm-4">
                                    <div class="form-group">
                                        <label class="col-sm-12 col-form-label">E-mail do Usuário</label>
                                        <asp:TextBox id="txtEmail" runat="server" class="form-control" placeholder="E-mail do Usuário"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="col-sm-4">
                                    <div class="form-group">
                                        <label class="col-sm-12 col-form-label">Senha do Usuário</label>
                                        <asp:TextBox id="txtSenha" runat="server" class="form-control" placeholder="Informe sua Senha"></asp:TextBox>
                                    </div>
                                </div>
                            </div>
                            <div class="row"
                                <div class="col-12">
                                    <div class="alert alert-warning alert-dismissible">
                                        <button type="button" class="close" data-dismiss="alert" aria-hidden="true">&times;</button>
                                        <h5><i class="icon fas fa-exclamation-triangle"></i> ATENÇÃO!</h5>
                                        ATENÇÃO! A senha deve atender os seguintes requisitos: Possua pelo menos 1 número, Possua pelo menos 1 letra maiúscula, Possua pelo menos 1 letra minúscula, Possua pelo menos 1 caractere especial e Não permitir espaço. Além disso, a senha deve ter de 6 a 32 caracteres.
                                    </div>
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
                <p>Para confirmar a operação, digite abaixo o código de confirmação que você recebeu por e-mail</p>
                <div class="input-group mb-3 col-12">
                    <asp:TextBox runat="server" ID="txt2FABoletos" CssClass="form-control" placeholder="Código de Confirmação" Visible="true"></asp:TextBox>
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
                <asp:Button runat="server" ID="btnConfirmar2FA" 
                    CssClass="btn btn-primary" Text="Confirmar" 
                    onclick="btnConfirmar2FA_Click" />
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
        $('#txtDocumento').inputmask('999.999.999-99')
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
