<%@ Page Language="C#" AutoEventWireup="true" CodeFile="login.aspx.cs" Inherits="login" EnableSessionState="true" Async="true" MaintainScrollPositionOnPostback="true"%>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta charset="utf-8"/>
    <meta http-equiv="X-UA-Compatible" content="IE=edge"/>
    <title>LegacyBank</title>

    <meta name="viewport" content="width=device-width, initial-scale=1"/>
    <link rel="stylesheet" href="../plugins/fontawesome-free/css/all.min.css"/>
    <link rel="stylesheet" href="https://code.ionicframework.com/ionicons/2.0.1/css/ionicons.min.css"/>
    <link rel="stylesheet" href="../plugins/icheck-bootstrap/icheck-bootstrap.min.css"/>
    <link rel="stylesheet" href="../dist/css/adminlte.css"/>
    <link href="https://fonts.googleapis.com/css?family=Source+Sans+Pro:300,400,400i,700" rel="stylesheet"/>

    <asp:Literal ID="FavIconLink" runat="server"></asp:Literal>

    <script type="text/javascript" src="funcoes.js"></script>
    <script src="https://www.google.com/recaptcha/api.js?hl=pt"></script>

    <script type="text/javascript">
        var imNotARobot = function () {
            $("#recaptcha").val(grecaptcha.getResponse());
        };
    </script>

    <style type="text/css">
        .btn-whitelabel1 {
          background-color: <%=CorPrimaria() %>;
          border-color: <%=CorPrimaria() %>;
        }   
        .btn-whitelabel1:hover {
          background-color: <%=CorPrimaria() %>;
          border-color: <%=CorPrimaria() %>;
          opacity:0.9;
        }        
        .btn-whitelabel1.disabled, .btn-whitelabel1:disabled {
          background-color: <%=CorPrimaria() %>;
          border-color: <%=CorPrimaria() %>;
        }

        .btn-whitelabel1:not(:disabled):not(.disabled):active, .btn-whitelabel1:not(:disabled):not(.disabled).active,
        .show > .btn-whitelabel1.dropdown-toggle {
          background-color: <%=CorPrimaria() %>;
          border-color: <%=CorPrimaria() %>;
          opacity:0.9;
        }
        
        .text-whitelabel1 {
          color: <%=CorPrimaria() %> !important;
        }

       
    </style>


</head>
<body runat="server" id="bdyLogin" class="hold-transition login-page vh-100 h-100" style="height: 100vh;">
<form runat="server" id="frmLogin">

<div class="row no-gutters min-vh-100">
    
    <div class="col-md-6 order-2 order-md-1 d-none d-md-flex align-items-center justify-content-center">

        <div class="row vh-100 h-100 d-flex">
            <div class="col-12 align-items-center align-self-center align-content-center p-5">
                <center>
                <asp:Image alt="" runat="server" id="imgDestaque" CssClass="img-fluid"></asp:Image>
                </center>
            </div>
        </div>

    </div>

    <div class="col-md-6 order-1 order-md-2 d-flex align-items-center justify-content-center p-4 bg-white">

        <div class="card rounded-left dvh-100 h-100 w-100 dvw-100">
            <div class="card-body login-card-body rounded-left">
            <!-- Card Body -->
                <div runat="server" id="divVoltar" visible="false" class="mb-5">
                    <asp:LinkButton runat="server" ID="lkbVoltar" CssClass="btn btn-whitelabel1 col-12 mb-5 text-white" onclick="lkbAppStore_Click"><i class="fas fa-arrow-left"></i>     Voltar</asp:LinkButton>                    
                </div>

                <center>
                    <a class="brand-logo mx-0 px-0">
                        <asp:Image height="75" alt="" runat="server" id="imgLogoPrincipal" CssClass="img-fluid"></asp:Image>
                    </a>
                </center>
                <div runat="server" id="divBemvindo" visible="true">
                    <center>
                    <h2>Seja bem-vindo(a)!</h2>
                        <div class="row text-centar" runat="server" id="dvQRCodeAPP" visible="false">
                            <center>
                            <h6><b>Em breve</b> você poderá realizar o acesso lendo o <b>QR Code</b> no nosso <b>app</b></h6>
                            <div class="card card-outline card-success col-4">
                                <div class="card-body">
                                <asp:Image runat="server" ID="imgQRCode" CssClass="img-fluid" />
                                </div>
                            </div>
                            </center>
                        </div>

                        <div class="row text-centar" runat="server" id="dvOpcoesLogin" visible="true">
                            <div class="col-12 m-4">
                                <h6 class="text-whitelabel1">Selecione abaixo a forma como você deseja acessar o portal</h6>

                                <asp:Button runat="server" ID="btnEntrarEmail" CssClass="btn btn-whitelabel1 col-5 text-white" Text="Entrar com e-mail" onclick="btnEntrarEmail_Click" />
                                <asp:Button runat="server" ID="btnCriarConta" 
                                    CssClass="btn btn-whitelabel1 col-5  text-white" Text="Criar Conta" 
                                    onclick="btnCriarConta_Click" />
                            </div>
                        </div>

                        <div class="row text-centar" runat="server" id="dvQRCodeBTN" visible="false">
                            <center>
                            <div class="col-12 m-4">
                                <h3>Em breve você poderá baixar nosso app</h3>
                                <h6><small>Em breve você poderá fazer o download do aplicativo e aproveitar os recursos esclusivos</small></h6>
                                <asp:LinkButton runat="server" ID="lkbGooglePlay" CssClass="btn btn-outline-success col-5" onclick="lkbGooglePlay_Click">Google Play     <i class="fab fa-google-play"></i></asp:LinkButton>
                                <asp:LinkButton runat="server" ID="lkbAppStore" CssClass="btn btn-outline-success col-5" onclick="lkbAppStore_Click">App Store     <i class="fab fa-apple"></i></asp:LinkButton>
                        
                            </div>
                            </center>
                        </div>

                    </center>
                </div>

                <div class="" runat="server" id="divEntrarEmail" visible="false">
                    <center>
                    <p class="card-text">Por favor entre em sua conta</p>
                    <div class="callout callout-primary" runat="server" id="divAguardandoAceite" visible="true">
                        <p class="text-black"><span className="me-25"><b>Este site usa cookies e dados pessoais!</b> <br />Para contiuar você deve concordar com os nossos <a href="" data-toggle="modal" data-target="#modal-termo"><b>Termos e Condições de Uso</b></a> e <a href="" data-toggle="modal" data-target="#modal-politica"><b>Política de Privacidade</b></a></span>.</p>
                        <div class="form-group clearfix">
                            <div class="icheck-primary d-inline">
                            <asp:Checkbox runat="server" ID="ckbAceite" />
                            <label for="ckbAceite">
                                Aceitar e continuar
                            </label>
                            </div>
                        </div>
                    </div>

                    <div class="modal fade" id="modal-termo">
                        <div class="modal-dialog modal-xl">
                            <div class="modal-content">
                                <div class="modal-header">
                                    <h4 class="modal-title">Termos e Condições de Uso</h4>
                                    <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                                    <span aria-hidden="true">&times;</span>
                                    </button>
                                </div>
                                <div class="modal-body" style="height:400px; overflow:auto;">
                                    <!--<div class="ExternalFiles">
                                        <iframe src="public_html/Termos_e_Condições_de_Uso.pdf" width="100%" height="auto" frameborder="0" style="min-height:600px;"></iframe>
                                    </div>-->
                                    <asp:Label runat="server" ID="lblTermo"></asp:Label>
                                </div>
                                <div class="modal-footer justify-content-between">
                                    <button type="button" class="btn btn-default" data-dismiss="modal">Fechar</button>
                                </div>
                            </div>
                        </div>
                    </div>

                    <div class="modal fade" id="modal-politica">
                        <div class="modal-dialog modal-xl">
                            <div class="modal-content">
                                <div class="modal-header">
                                    <h4 class="modal-title">Política de Privacidade</h4>
                                    <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                                    <span aria-hidden="true">&times;</span>
                                    </button>
                                </div>
                                <div class="modal-body" style="height:400px; overflow:auto;">
                                    <!--<div class="ExternalFiles">
                                        <iframe src="public_html/Política_de_Privacidade.pdf" width="100%" height="auto" frameborder="0" style="min-height:600px;"></iframe>
                                    </div>-->
                                    <asp:Label runat="server" ID="lblPolitica"></asp:Label>
                                </div>
                                <div class="modal-footer justify-content-between">
                                    <button type="button" class="btn btn-default" data-dismiss="modal">Fechar</button>
                                </div>
                            </div>
                        </div>
                    </div>

                    <div runat="server" id="dvLogin" visible="true">
                        <div class="row">
                            <div class="col-12 m-4">
                                <center>
                                    <div class="input-group mb-3 col-6">
                                      <asp:TextBox runat="server" ID="txtLogin" CssClass="form-control" placeholder="Informe seu e-mail"></asp:TextBox>
                                      <div class="input-group-prepend">
                                        <span class="input-group-text"><i class="fas fa-envelope"></i></span>
                                      </div>

                                    </div>

                                    <div class="input-group mb-3 col-6">
                                        <asp:TextBox runat="server" ID="txtSenha" CssClass="form-control" placeholder="Informe sua senha" TextMode="Password"></asp:TextBox>
                                        <div class="input-group-prepend">
                                            <span class="input-group-text"><i class="fas fa-lock"></i></span>
                                        </div>
                                    </div>

                                    <div class="form-group  mb-3 col-6" runat="server" id="dvTipo" visible="false">
                                        <label for="exampleInputTipo">Perfil</label>
                                        <asp:DropDownList runat="server" id="ddlTipo" CssClass="form-control" 
                                            onselectedindexchanged="ddlTipo_SelectedIndexChanged" AutoPostBack="True">
                                        </asp:DropDownList>
                                    </div>
                                    <asp:Button runat="server" ID="btnEnviar" CssClass="btn btn-whitelabel1 col-5 text-white" Text="Enviar" onclick="btnEnviar_Click"/>
                                    <p class="text-right col-5 m-2">
                                        <a href="#" data-toggle="modal" data-target="#modal-senha" class="text-whitelabel1 fluid-right">
                                            <small class="text-whitelabel1 fluid-right">Esqueci a minha senha ></small>
                                        </a>
                                    </p>

                                </center>
                            </div>
                        </div>
                    </div>

                    <div runat="server" id="divPerfilacesso" visible="true">
                        <div class="row">
                            <div class="col-12">
                                <center>


                                </center>
                            </div>
                        </div>
                    </div>



                    <div runat="server" id="dv2FA" visible="true">
                        <label>Autenticação de dois fatores</label>
                        <p>Por favor, entre com o código de confirmação que foi enviado para o seu e-mail</p>
                        <div class="form-group">
                            <asp:TextBox runat="server" ID="txtID" CssClass="form-control" Visible="false"></asp:TextBox>
                            <asp:TextBox runat="server" ID="txtLicenciado" CssClass="form-control" Visible="false"></asp:TextBox>
                            <asp:TextBox runat="server" ID="txt2FAGerado" CssClass="form-control" Visible="false"></asp:TextBox>
                        </div>
                        <div class="input-group mb-3 col-6">
                            <asp:TextBox runat="server" ID="txt2FA" CssClass="form-control" placeholder="Código de Confirmação" Visible="true"></asp:TextBox>
                            <div class="input-group-prepend">
                            <span class="input-group-text"><i class="fas fa-laptop-code"></i></span>
                            </div>

                        </div>
                
                        <p class="mt-1">
                            <strong>Não recebeu?  </strong>   <asp:LinkButton ID="lkbReenviar" runat="server" onclick="lkbReenviar_Click">Enviar novo código</asp:LinkButton>
                        </p>
                    </div>

                    <div class="col-12 m-4">
                        <center>


                        <asp:Button runat="server" ID="btnAcessar" CssClass="btn btn-whitelabel1 col-5 text-white" Text="Acessar Conta" onclick="btnAcessar_Click"/>
                        <asp:Label runat="server" ID="lblTokenAcesso" Text="000000" Visible="false"></asp:Label>

                        </center>
                    </div>

                    <div class="row" style="display:none;">                    
                        <p class="text-center mt-2">
                            <span class="me-25">Novo na plataforma?</span>  <a href="cad_nova_conta.aspx"><span>Criar uma conta</span></a><br />
                        </p><br />
                    </div>

                    <div class="row">
                        <center>
                        <p class="mt-1 fluid-right text-whitelabel1 col-5">
                            
                        </p>
                        </center>
                        <!-- Modal de recuperação da senha -->

                        <div class="modal" id="modal-senha">
                          <div class="modal-dialog">
                            <div class="modal-content bg-white">
                              <div class="modal-header">
                                <h4 class="modal-title">Recuperação de Senha</h4>
                              </div>
                              <div class="modal-body">
                                    <div class="row">
                                        <div class="col-sm-12">
                                            <div class="form-group">
                                                <label>E-Mail</label>
                                                <asp:TextBox runat="server" id="txtEmailRecuperacao" cssClass="form-control" placeholder="Informe o seu e-mail de login"></asp:TextBox>
                                            </div>
                                        </div>                  
                                    </div>
                                </div>
                              <div class="modal-footer">
                                <asp:Button runat="server" CssClass="btn btn-primary" ID="btnRecuperarSenha" 
                                      Text="Recuperar Senha" onclick="btnRecuperarSenha_Click" />
                                <button type="button" class="btn btn-danger" data-dismiss="modal">Cancelar</button>
                              </div>
                            </div>
                          </div>
                        </div>
                    </div>
                    </center>
                </div>

            <!-- Card Body -->
            </div>
        </div>

    </div>
    <!--</div>-->

</div>
<asp:Label runat="server" ID="lbldados"></asp:Label>
</form>

<script type="text/javascript" src="../plugins/jquery/jquery.min.js"></script>
<script type="text/javascript" src="../plugins/bootstrap/js/bootstrap.bundle.min.js"></script>
<script type="text/javascript" src="../dist/js/adminlte.min.js"></script>
<script type="text/javascript" src="../plugins/select2/js/select2.full.min.js"></script>
<script type="text/javascript" src="../plugins/bootstrap4-duallistbox/jquery.bootstrap-duallistbox.min.js"></script>
<script type="text/javascript" src="../plugins/moment/moment.min.js"></script>
<script type="text/javascript" src="../plugins/inputmask/min/jquery.inputmask.bundle.min.js"></script>
<script type="text/javascript" src="../plugins/daterangepicker/daterangepicker.js"></script>
<script type="text/javascript" src="../plugins/bootstrap-colorpicker/js/bootstrap-colorpicker.min.js"></script>
<script type="text/javascript" src="../plugins/tempusdominus-bootstrap-4/js/tempusdominus-bootstrap-4.min.js"></script>
<script type="text/javascript" src="../plugins/bootstrap-switch/js/bootstrap-switch.min.js"></script>
<script type="text/javascript" src="../dist/js/demo.js"></script>

</body>
</html>
