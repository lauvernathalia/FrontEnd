<%@ Page Language="C#" AutoEventWireup="true" CodeFile="cad_marketplace_usuarios.aspx.cs" Inherits="cad_marketplace_usuarios" %>

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
                        <h3 class="card-title">MARKETPLACES USUÁRIOS</h3>
                    </div>
                    <div class="card-body">
                        <h3><span class="float-center badge bg-whitelabel1">1. Dados do Usuário</span></h3>

                        <div class="row">
                            <div class="col-sm-3">
                                <div class="form-group">
                                    <label>ID</label>
                                    <asp:TextBox runat="server" id="txtID" cssClass="form-control" Enabled="false"></asp:TextBox>
                                </div>
                            </div>
                            <div class="col-sm-3">
                                <div class="form-group">
                                    <label>Código</label>
                                    <asp:TextBox runat="server" id="txtCodigo" cssClass="form-control" Enabled="false"></asp:TextBox>
                                </div>
                            </div>
                            <div class="col-sm-6">
                                <div class="form-group">
                                    <label>Marketplace</label>
                                    <asp:TextBox runat="server" id="txtRazaosocial" cssClass="form-control" Enabled="false"></asp:TextBox>
                                </div>
                            </div>
                            <div class="col-sm-12">
                                <div class="form-group">
                                    <label>URL</label>
                                    <asp:TextBox runat="server" id="txtURL" cssClass="form-control" Enabled="false"></asp:TextBox>
                                </div>
                            </div>
                        </div>

                        <div class="row">
                            <div class="col-sm-6"  >
                                <div class="form-group">
                                <label>Nome<strong class="text-danger">*</strong></label>
                                <asp:TextBox runat="server" ID="txtNome" CssClass="form-control" placeholder="Nome do Usuário" ></asp:TextBox> 
                                </div>
                            </div>

                            <div class="col-sm-3"  >
                                <div class="form-group">
                                <label>E-Mail<strong class="text-danger">*</strong></label>
                                <asp:TextBox runat="server" ID="txtEmail" CssClass="form-control" placeholder="E-Mail do Usuário" ></asp:TextBox> 
                                </div>
                            </div>
                                          

                            <div class="col-sm-3"  >
                                <div class="form-group">
                                <label>Senha<strong class="text-danger">*</strong></label>
                                <asp:TextBox runat="server" ID="txtSenha" CssClass="form-control" placeholder="Senha de Acesso" ></asp:TextBox> 
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

                    <div class="card-footer">
                        <asp:Button runat="server" ID="btnIncluir" CssClass="btn btn-whitelabel1" Text="Incluir" onclick="btnIncluir_Click"/>
                        <asp:Button runat="server" ID="btnCancelar" CssClass="btn btn-danger float-right" Text="Cancelar" onclick="btnCancelar_Click"/>
                    </div>
                </div>            
            
            </div>
        </div>



        <div class="row">
            <div class="col-12">

                <div class="card">
                    <div class="card-header bg-whitelabel1">
                        <h3 class="card-title">LISTA DE USUÁRIOS</h3>
                    </div>
                    <div class="card-body">
                        <table id="tbUsuario" class="table table-bordered table-hover">
                            <thead>
                            <tr>
                                <th>ID</th>
                                <th>Nome</th>
                                <th>E-Mail</th>
                                <th>Senha</th>
                                <th>Ativo</th>
                                <!--<th>Acesso</th>-->
                                <th>Ativar/Inativar</th>
                                <th>Excluir</th>
                            </tr>
                            </thead>
                            <tbody>
                            <asp:Repeater runat="server" ID="rptConsultaUsuario" 
                                    onitemcommand="rptConsultaUsuario_ItemCommand">
                                <ItemTemplate>
                                    <tr>
                                        <td>
                                            <small><strong><%# DataBinder.Eval(Container.DataItem, "COD_ID")%></strong></small>
                                        </td>
                                        <td>
                                            <small><strong><%# DataBinder.Eval(Container.DataItem, "NOM_NOME")%></strong></small>
                                        </td>
                                        <td>
                                            <small><%# DataBinder.Eval(Container.DataItem, "NOM_LOGIN")%></small>
                                        </td>
                                        <td>
                                            <small><%# Funcoes.Encrypt(DataBinder.Eval(Container.DataItem, "NOM_SENHA").ToString())%></small>
                                        </td>
                                        <td>
                                            <small><%# DataBinder.Eval(Container.DataItem, "FLG_ATIVO")%></small>
                                        </td>
                                        <!--
                                        <td>
                                            <small><strong><a href='https://<%# txtURL.Text.ToString()%>/relogar.aspx?id=<%#Funcoes.Encrypt(DataBinder.Eval(Container.DataItem,"COD_ID").ToString())%>' target="_blank">Acesso</a></strong></small>
                                        </td>
                                        -->
                                        <td>
                                            <asp:linkbutton ID="lkbAtivo" CssClass="btn btn-whitelabel1 btn-sm" commandname="Ativo" runat="server" text="Ativar/Inativar"  CommandArgument='<%# DataBinder.Eval(Container.DataItem, "COD_ID")%>' />
                                        </td>

                                        <td>
                                            <asp:linkbutton ID="lkbExcluir" CssClass="btn btn-warning" commandname="Excluir" runat="server" text="Excluir"  ToolTip="Excluir" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "COD_ID")%>'></asp:linkbutton>
                                        </td>

                                    </tr>
                                </ItemTemplate>
                            </asp:Repeater>

                            </tbody>
                        </table>


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
        $('#txtCPF').inputmask('999.999.999-99')
        $('#txtCNPJ').inputmask('99.999.999/9999-99')
        $('#txtCelular').inputmask('(99) 99999-9999')
        $('#txtTelefoneEmpresa').inputmask('(99) 99999-9999')
        $('#txtCEP').inputmask('99999-999')

    })
</script>

<script type="text/javascript">

$(document).ready(function () {
    var table = $('#tbUsuario').DataTable({
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
