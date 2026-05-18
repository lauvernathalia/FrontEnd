<%@ Page Language="C#" AutoEventWireup="true" CodeFile="cad_marketplace_portal.aspx.cs" Inherits="cad_marketplace_portal" %>

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
                        <h3 class="card-title">MARKETPLACE PORTAL</h3>
                    </div>
                    <div class="card-body">
                        <h3><span class="float-center badge bg-whitelabel1">1. Dados do Marketplace</span></h3>

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

                        <h3><span class="float-center badge bg-whitelabel1">2. Domínio</span></h3>

                        <div class="row">
                            <div class="col-sm-6"  >
                                <div class="form-group">
                                <label>Domínio/URL<strong class="text-danger">*</strong></label>
                                <asp:TextBox runat="server" ID="txtDominio" CssClass="form-control"></asp:TextBox> 
                                </div>
                            </div>

                            <div class="col-sm-6"  >
                                <div class="form-group">
                                <label>Titulo Domínio/URL<strong class="text-danger">*</strong></label>
                                <asp:TextBox runat="server" ID="txtTitulo" CssClass="form-control"></asp:TextBox> 
                                </div>
                            </div>

                        </div>

                        <h3><span class="float-center badge bg-whitelabel1">3. Cores</span></h3>

                        <div class="row">
                            <div class="col-sm-6"  >
                                <div class="form-group">
                                <label>Cor Primária<strong class="text-danger">*</strong></label>
                                <asp:TextBox runat="server" ID="txtCorPrimaria" CssClass="form-control my-colorpicker1"></asp:TextBox> 
                                </div>
                            </div>

                            <div class="col-sm-6"  >
                                <div class="form-group">
                                <label>Cor Secundária<strong class="text-danger">*</strong></label>
                                <asp:TextBox runat="server" ID="txtCorSecundaria" CssClass="form-control my-colorpicker1"></asp:TextBox> 
                                </div>
                            </div>
                        </div>
                        <h3><span class="float-center badge bg-whitelabel1">4. Logotipo</span></h3>

                        <div class="row">
                            <div class="col-sm-6"  >
                                <div class="form-group">
                                    <label>Logotipo (Tela Login)<strong class="text-danger">*</strong></label>
                                    <asp:TextBox runat="server" ID="txtLogotipoLogin" CssClass="form-control"></asp:TextBox> 
                                      <div class="form-group">
                                        <div class="input-group">
                                          <div class="custom-file">
                                            <input type="file" name="attachment" runat="server" id="flLogotipoLogin"/>
                                          </div>
                                        </div>
                                    </div>
                                </div>
                            </div>

                            <div class="col-sm-6"  >
                                <div class="form-group">
                                    <label>Logotipo (Menu Principal)<strong class="text-danger">*</strong></label>
                                    <asp:TextBox runat="server" ID="txtLogotipoMenu" CssClass="form-control"></asp:TextBox> 

                                      <div class="form-group">
                                        <div class="input-group">
                                          <div class="custom-file">
                                            <input type="file" name="attachment" runat="server" id="flLogotipoMenu"/>
                                          </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>

                        <h3><span class="float-center badge bg-whitelabel1">5. Imagem Fundo</span></h3>

                        <div class="row">
                            <div class="col-sm-12"  >
                                <div class="form-group">
                                    <label>Imagem Fundo (1920px x 1080px)<strong class="text-danger">*</strong></label>
                                    <asp:TextBox runat="server" ID="txtImagem" CssClass="form-control"></asp:TextBox> 
                                      <div class="form-group">
                                        <div class="input-group">
                                          <div class="custom-file">
                                            <input type="file" name="attachment" runat="server" id="flImagem"/>
                                          </div>
                                        </div>
                                    </div>

                                </div>
                            </div>

                        </div>

                        <h3><span class="float-center badge bg-whitelabel1">6. Favicon</span></h3>

                        <div class="row">
                            <div class="col-sm-12"  >
                                <div class="form-group">
                                    <label>Favicon<strong class="text-danger">*</strong></label>
                                    <asp:TextBox runat="server" ID="txtFavicon" CssClass="form-control"></asp:TextBox> 
                                      <div class="form-group">
                                        <div class="input-group">
                                          <div class="custom-file">
                                            <input type="file" name="attachment" runat="server" id="flFavicon"/>
                                          </div>
                                        </div>
                                    </div>

                                </div>
                            </div>

                        </div>


                        <h3><span class="float-center badge bg-whitelabel1">7. Logo Checkout</span></h3>

                        <div class="row">
                            <div class="col-sm-12"  >
                                <div class="form-group">
                                    <label>Logo Checkout<strong class="text-danger">*</strong></label>
                                    <asp:TextBox runat="server" ID="txtCheckout" CssClass="form-control"></asp:TextBox> 
                                      <div class="form-group">
                                        <div class="input-group">
                                          <div class="custom-file">
                                            <input type="file" name="attachment" runat="server" id="flCheckout"/>
                                          </div>
                                        </div>
                                    </div>

                                </div>
                            </div>

                        </div>

                        <h3><span class="float-center badge bg-whitelabel1">8. Login</span></h3>

                        <div class="row">
                            <div class="col-sm-12"  >
                                <div class="form-group">
                                    <label>Ativar 2FA (Autenticação de dois fatores)<strong class="text-danger">*</strong></label>
                                    <asp:DropDownList runat="server" ID="ddl2FA" CssClass="form-control">
                                        <asp:ListItem Value="N" Text="Não"></asp:ListItem>
                                        <asp:ListItem Value="S" Text="Sim"></asp:ListItem>
                                    </asp:DropDownList>
                                </div>
                            </div>

                        </div>

                        <h3><span class="float-center badge bg-whitelabel1">9. Criação Cadastros Conta Externo</span></h3>

                        <div class="row">
                            <div class="col-sm-12"  >
                                <div class="form-group">
                                    <label>Permite criar contas de estabelecimentos pelo link externo<strong class="text-danger">*</strong></label>
                                    <asp:DropDownList runat="server" ID="ddlCriarConta" CssClass="form-control">
                                        <asp:ListItem Value="N" Text="Não"></asp:ListItem>
                                        <asp:ListItem Value="S" Text="Sim"></asp:ListItem>
                                    </asp:DropDownList>
                                </div>
                            </div>

                        </div>



                    </div>

                    <div class="card-footer">
                        <asp:Button runat="server" ID="btnSalvar" CssClass="btn btn-whitelabel1" Text="Salvar" onclick="btnSalvar_Click"/>
                        <asp:Button runat="server" ID="btnGerar" CssClass="btn btn-info" Text="Gerar Domínio" onclick="btnGerar_Click"/>
                        <asp:Button runat="server" ID="btnCancelar" CssClass="btn btn-danger float-right" Text="Fechar" onclick="btnCancelar_Click"/>
                    </div>
                </div>            
            
            </div>
        </div>


    </form>

<script src="../../plugins/bootstrap-colorpicker/js/bootstrap-colorpicker.min.js"></script>

<script>
    $(function () {

        //Colorpicker
        $('.my-colorpicker1').colorpicker()

    })
</script>



</body>
</html>
