<%@ Page Language="C#" AutoEventWireup="true" CodeFile="cad_licenciados_email.aspx.cs" Inherits="cad_licenciados_email" %>

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

</head>
<body class="hold-transition sidebar-mini layout-fixed">
    <form id="frmPrincipal" runat="server">

        <div class="row">
            <div class="col-12">

                <div class="card">
                    <div class="card-header bg-whitelabel1">
                        <h3 class="card-title">LICENCIADOS HOST EMAIL</h3>
                    </div>
                    <div class="card-body">
                        <h3><span class="float-center badge bg-whitelabel1">1. Dados do Licenciado</span></h3>

                        <div class="row">
                            <div class="col-sm-2">
                                <div class="form-group">
                                    <label>ID</label>
                                    <asp:TextBox runat="server" id="txtID" cssClass="form-control" Enabled="false"></asp:TextBox>
                                </div>
                            </div>
                            <div class="col-sm-2">
                                <div class="form-group">
                                    <label>Código</label>
                                    <asp:TextBox runat="server" id="txtCodigo" cssClass="form-control" Enabled="false"></asp:TextBox>
                                </div>
                            </div>
                            <div class="col-sm-6">
                                <div class="form-group">
                                    <label>Licenciado</label>
                                    <asp:TextBox runat="server" id="txtRazaosocial" cssClass="form-control" Enabled="false"></asp:TextBox>
                                </div>
                            </div>
                        </div>

                        <h3><span class="float-center badge bg-whitelabel1">2. Host E-mail</span></h3>

                        <div class="row">
                            <div class="col-sm-6">
                                <div class="form-group">
                                <label>Host E-mail<strong class="text-danger">*</strong></label>
                                <asp:TextBox runat="server" ID="txtHost" CssClass="form-control"></asp:TextBox> 
                                </div>
                            </div>
                            <div class="col-sm-6">
                                <div class="form-group">
                                <label>E-mail<strong class="text-danger">*</strong></label>
                                <asp:TextBox runat="server" ID="txtEmail" CssClass="form-control"></asp:TextBox> 
                                </div>
                            </div>
                        </div>

                        <div class="row">
                            <div class="col-sm-6">
                                <div class="form-group">
                                <label>Senha<strong class="text-danger">*</strong></label>
                                <asp:TextBox runat="server" ID="txtSenha" CssClass="form-control"></asp:TextBox> 
                                <asp:TextBox runat="server" ID="txtSenhaHide" CssClass="form-control" Visible="false"></asp:TextBox> 
                                </div>
                            </div>
                            <div class="col-sm-3">
                                <div class="form-group clearfix">
                                    <label>Conexão Segura?<strong class="text-danger">*</strong></label><br />
                                    <div class="icheck-secondary d-inline">
                                    <asp:Checkbox runat="server" ID="ckbSSL" />
                                    <label for="ckbSSL">
                                        SSL/TLS
                                    </label>
                                    </div>
                                </div>

                            </div>
                            <div class="col-sm-3">
                                <div class="form-group">
                                <label>Porta<strong class="text-danger">*</strong></label>
                                <asp:TextBox runat="server" ID="txtPorta" CssClass="form-control"></asp:TextBox> 
                                </div>
                            </div>
                        </div>

                        <h3><span class="float-center badge bg-whitelabel1">3. Logotipo e Rodapé do E-mail</span></h3>

                        <div class="row">

                            <div class="col-sm-12">
                                <div class="form-group">
                                <label>Rodapé do E-mail<strong class="text-danger">*</strong></label>
                                <textarea runat="server" id="txtRodape" class="textarea" rows="5"></textarea>
                                </div>
                            </div>

                            <div class="col-sm-12"  >
                                <div class="form-group">
                                    <label>Logotipo E-mail<strong class="text-danger">*</strong></label>
                                    <asp:TextBox runat="server" ID="txtLogotipo" CssClass="form-control"></asp:TextBox> 
                                      <div class="form-group">
                                        <div class="input-group">
                                          <div class="custom-file">
                                            <input type="file" name="attachment" runat="server" id="flLogotipo"/>
                                          </div>
                                        </div>
                                    </div>
                                </div>
                            </div>

                        </div>


                        <div class="row">
                            <asp:Button runat="server" ID="btnTestar" CssClass="btn btn-whitelabel1" Text="Enviar teste" onclick="btnTestar_Click"/>
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

<script type="text/javascript" src="../../plugins/bootstrap-colorpicker/js/bootstrap-colorpicker.min.js"></script>

<script type="text/javascript" src="../plugins/summernote/summernote-bs4.min.js"></script>
<script type="text/javascript">
    $(function () {
        // Summernote
        $('.textarea').summernote()
    })
</script>


<script>
    $(function () {

        //Colorpicker
        $('.my-colorpicker1').colorpicker()

    })
</script>



</body>
</html>
