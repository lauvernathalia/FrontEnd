<%@ Page Language="C#" AutoEventWireup="true" CodeFile="cad_nova_conta.aspx.cs" Inherits="cad_nova_conta" %>

<%@ Register TagPrefix="Portal" TagName="PageRotina" Src="rotinaspadroes.ascx" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <meta charset="utf-8">
    <meta name="viewport" content="width=device-width, initial-scale=1">
    <meta http-equiv="x-ua-compatible" content="ie=edge">

    <title>LEGACYBANK</title>

    <PORTAL:PAGEROTINA id="PageRotina1" title="Site Directory" runat="server" ModuleSource="rotinaspadroes.ascx"></PORTAL:PAGEROTINA>

    <link rel="stylesheet" href="../plugins/fullcalendar/main.min.css"/>
    <link rel="stylesheet" href="../plugins/fullcalendar-daygrid/main.min.css"/>
    <link rel="stylesheet" href="../plugins/fullcalendar-timegrid/main.min.css"/>
    <link rel="stylesheet" href="../plugins/fullcalendar-bootstrap/main.min.css"/>
    <script src="https://cdnjs.cloudflare.com/ajax/libs/Chart.js/2.5.0/Chart.min.js"></script>
    <asp:Literal ID="FavIconLink" runat="server"></asp:Literal>


</head>

<body runat="server" id="bdyLogin" class="hold-transition login-page">

<form runat="server" id="frmLogin">

    <div class="login-box">
        <!-- Card Iniciar ***************************************************************************************************************************************************************************** -->

        <div runat="server" id="crdLogo">
            <div class="login-card-body">
                <center>
                    <a class="brand-logo mx-0 px-0">
                        <asp:Image height="50" alt="" runat="server" id="imgLogoPrincipal" CssClass="img-fluid"></asp:Image>
                    </a>
                </center>
            </div>
        </div>

        <div class="card" runat="server" id="crdIniciar">
            <div class="card-body login-card-body">
                <center>
                    <br />
                    <br />
                    <h2>A CONTA COMPLETA QUE IRÁ FACILITAR O SEU DIA A DIA! 👋</h2>
                    <br />
                    <h1 class="text-whitelabel1"><i class="fas fa-exclamation-triangle text-whitelabel1"></i>     ATENÇÃO</h1>
                    <br />
                    <p class="card-text">Para prosseguir a gente precisa de algumas informações para poder oferecer o melhor produto para você.</p>
                    <br /><br />
                </center>

                <div class="row">
                    <asp:Button runat="server" ID="btnVamos" CssClass="btn btn-whitelabel1 btn-block" Text="Vamos começar" onclick="btnVamos_Click"/>
                </div>
            </div>
            <div class="card-footer">
                <asp:Button runat="server" ID="btnLogin" CssClass="btn btn-whitelabel1 btn-block" Text="Acessar Conta" onclick="btnLogin_Click"/>
            </div>
        </div>
        <!-- Card Tipo de Conta ***************************************************************************************************************************************************************************** -->
        <div class="card" runat="server" id="crdTipoConta" visible="false">
            <div class="card-body login-card-body">
                <center>
                    <br />
                    <br />
                    <h3 class="card-text">Escolha o seu tipo de conta</h3>
                    <br /><br />
                </center>

                <div class="row">
                    <div class="col-12">
                        <center>
                        <i class="fas fa-user fa-2x text-whitelabel1"></i><br /><br />
                        <asp:Button runat="server" ID="btnPessoal" CssClass="btn btn-whitelabel1 col-5" Text="Pessoal" onclick="btnPessoal_Click"/>
                        <br /><br />
                        </center>
                    </div>
                </div>
                <div class="row">
                    <div class="col-12">
                        <center>
                        <i class="fas fa-store-alt fa-2x text-whitelabel1"></i><br /><br />
                        <asp:Button runat="server" ID="btnNegocio" CssClass="btn btn-whitelabel1 col-5" Text="Negócio" onclick="btnNegocio_Click"/>
                        <br /><br />
                        </center>
                    </div>
                </div>
            </div>
            <div class="card-footer">
                <asp:Button runat="server" ID="btnVoltar01" CssClass="btn btn-whitelabel1 btn-block" Text="Voltar" onclick="btnVoltar01_Click"/>
            </div>
        </div>
        <!-- Card CPF -->
        <div class="card" runat="server" id="crdCPF" visible="false">
            <div class="card-body login-card-body">
                <center>
                    <br />
                    <br />
                    <h3 class="card-text">Por favor, digite seu CPF</h3>
                    <br /><br />
                </center>

                <div class="row">
                    <div class="col-12">
                        <div class="form-group">
                            <asp:Label class="col-form-label" runat="server" ID="lblCPF" Text="CPF"></asp:Label>
                            <asp:TextBox id="txtCPF" runat="server" class="form-control" placeholder="000.000.000-00" data-inputmask='"mask": "999.999.999-99"' data-mask></asp:TextBox>
                        </div>
                    </div>
                </div>
                <div class="row">
                    <div class="col-12">
                        <center>
                        <asp:Button runat="server" ID="btnContinuar01" CssClass="btn btn-whitelabel1 btn-block" Text="Continuar" onclick="btnContinuar01_Click"/>
                        <br /><br />
                        </center>
                    </div>
                </div>
            </div>
            <div class="card-footer">
                <asp:Button runat="server" ID="btnVoltar02" CssClass="btn btn-whitelabel1 btn-block" Text="Voltar" onclick="btnVoltar02_Click"/>
            </div>
        </div>

        <!-- Card Pessoais ***************************************************************************************************************************************************************************** -->
        <div class="card" runat="server" id="crdPessoais" visible="false">
            <div class="card-body login-card-body">
                <center>
                    <br />
                    <br />
                    <h3 class="card-text">Informe os dados pessoais</h3>
                    <br /><br />
                </center>

                <div class="row">
                    <div class="col-12">
                        <div class="form-group">
                            <asp:Label class="col-form-label" runat="server" ID="lblNome" Text="Nome completo"></asp:Label>
                            <asp:TextBox id="txtNome" runat="server" class="form-control"></asp:TextBox>
                            <asp:Label class="col-form-label text-whitelabel1" runat="server" ID="lblAlertaNome" Text="Extamente como está no seu documento"></asp:Label>
                        </div>
                        <div class="form-group">
                            <asp:Label class="col-form-label" runat="server" ID="lblNascimento" Text="Data de nascimento"></asp:Label>
                            <asp:TextBox id="txtNascimento" runat="server" class="form-control" data-inputmask='"mask": "99/99/9999"' data-mask></asp:TextBox>
                        </div>
                        <div class="form-group">
                            <asp:Label class="col-form-label" runat="server" ID="lblEmail" Text="E-Mail"></asp:Label>
                            <asp:TextBox id="txtEmail" runat="server" class="form-control"></asp:TextBox>
                        </div>
                        <div class="form-group">
                            <asp:Label class="col-form-label" runat="server" ID="lblCelular" Text="Celular"></asp:Label>
                            <asp:TextBox id="txtCelular" runat="server" class="form-control" data-inputmask='"mask": "(99) 99999-9999"' data-mask></asp:TextBox>
                        </div>

                    </div>
                </div>
                <div class="row">
                    <div class="col-12">
                        <center>
                        <asp:Button runat="server" ID="btnContinuar02" CssClass="btn btn-whitelabel1 btn-block" Text="Continuar" onclick="btnContinuar02_Click"/>
                        <br /><br />
                        </center>
                    </div>
                </div>
            </div>
            <div class="card-footer">
                <asp:Button runat="server" ID="btnVoltar03" CssClass="btn btn-whitelabel1 btn-block" Text="Voltar" onclick="btnVoltar03_Click"/>
            </div>
        </div>

        <!-- Card Confirmar ***************************************************************************************************************************************************************************** -->
        <div class="card" runat="server" id="crdConfirmar" visible="false">
            <div class="card-body login-card-body">
                <center>
                    <br />
                    <br />
                    <h3 class="card-text">Digite o código que enviamos por E-MAIL</h3>
                    <br /><br />
                    <p class="text-whitelabel1">Você receberá um código para confirmar seus dados.</p>
                </center>

                <div class="row">
                    <div class="col-12">
                        <div class="form-group">
                            <asp:Label class="col-form-label" runat="server" ID="lblCodigo" Text="Digite o código"></asp:Label>
                            <asp:TextBox id="txtCodigo" runat="server" class="form-control"></asp:TextBox>
                        </div>
                    </div>
                </div>
                <div class="row">
                    <div class="col-12">
                        <center>
                        <asp:Button runat="server" ID="btnContinuar03" CssClass="btn btn-whitelabel1 btn-block" Text="Continuar" onclick="btnContinuar03_Click"/>
                        <br /><br />
                        </center>
                    </div>
                </div>
            </div>
            <div class="card-footer">
                <asp:Button runat="server" ID="btnVoltar04" CssClass="btn btn-whitelabel1 btn-block" Text="Voltar" onclick="btnVoltar04_Click"/>
            </div>
        </div>


        <!-- Card Aceite ***************************************************************************************************************************************************************************** -->
        <div class="card" runat="server" id="crdAceite" visible="false">
            <div class="card-body login-card-body">
                <center>
                    <br />
                    <br />
                    <h3 class="card-text">Aceite os termos e condições abaixo</h3>
                    <br /><br />
                    <p class="text-whitelabel1">Para prosseguir é necessário aceitar os termos e condições.</p>
                </center>

                <div class="row">
                    <div class="col-12">
                        <center>
                        <h4><a href="" onclick="javascript:openPopupWindow('public_html/Política_de_Privacidade.pdf', 'TermosCondicoes', 1024, 800); return false;" class="text-whitelabel1">
                            Ler termos e condições
                        </a></h4>
                        <br />
                        </center>
                    </div>
                </div>


                <div class="row">
                    <div class="col-12">
                        <center>
                        <asp:Button runat="server" ID="btnAceito" CssClass="btn btn-whitelabel1 btn-block" Text="Aceito" onclick="btnAceito_Click"/>
                        <br /><br />
                        </center>
                    </div>
                </div>
            </div>
            <div class="card-footer">
                <asp:Button runat="server" ID="btnNaoAceito" CssClass="btn btn-secondary btn-block" Text="Não aceito" onclick="btnNaoAceito_Click"/>
            </div>
        </div>


        <!-- Card Status ***************************************************************************************************************************************************************************** -->
        <div class="card" runat="server" id="crdStatus" visible="false">
            <div class="card-body login-card-body">
                <center>
                    <br />
                    <br />
                    <h3 class="card-text"><asp:Label runat="server" ID="lblStatus" Text=""></asp:Label></h3>
                    <br /><br />
                </center>

                <div class="row">
                    <div class="col-2"><br /></div>
                    <div class="col-8">
                        
                            <div class="form-group clearfix">
                                <div class="icheck-secondary d-inline">
                                <asp:CheckBox runat="server" ID="checkboxSuccess1" Checked="false"/>
                                <label for="checkboxSuccess1">Dados Iniciais
                                </label>
                                </div>
                            </div>                            
                            <div class="form-group clearfix">
                                <div class="icheck-secondary d-inline">
                                <asp:CheckBox runat="server" ID="checkboxSuccess2" Checked="false"/>
                                <label for="checkboxSuccess1">Endereço
                                </label>
                                </div>
                            </div>                            
                            <div class="form-group clearfix">
                                <div class="icheck-secondary d-inline">
                                <asp:CheckBox runat="server" ID="checkboxSuccess3" Checked="false"/>
                                <label for="checkboxSuccess1">Mais sobre você
                                </label>
                                </div>
                            </div>                            
                            <div class="form-group clearfix">
                                <div class="icheck-secondary d-inline">
                                <asp:CheckBox runat="server" ID="checkboxSuccess4" Checked="false"/>
                                <label for="checkboxSuccess1" runat="server" id="lblcheckboxSuccess4">Sobre o negócio
                                </label>
                                </div>
                            </div>
                            <br />
                            <br />
                            <p class="text-whitelabel1">Vamos nos conhecer um pouco melhor?</p>
                        </center>
                    </div>
                    <div class="col-2"><br /></div>
                </div>


                <div class="row">
                    <div class="col-12">
                        <center>
                        <asp:Button runat="server" ID="btnSim" CssClass="btn btn-whitelabel1 btn-block" Text="Sim, continuar" onclick="btnSim_Click"/>
                        <br /><br />
                        </center>
                    </div>
                </div>
            </div>
            <div class="card-footer">
                <asp:Button runat="server" ID="btnVoltarNao" CssClass="btn btn-secondary btn-block" Text="Voltar" onclick="btnVoltarNao_Click"/>
            </div>
        </div>


        <!-- Card Endereço CEP ***************************************************************************************************************************************************************************** -->

        <div class="card" runat="server" id="crdEnderecoCEP" visible="false">
            <div class="card-body login-card-body">
                <center>
                    <br />
                    <br />
                    <h3 class="card-text">Qual o seu CEP?</h3>
                    <br /><br />
                </center>

                <div class="row">
                    <div class="col-12">
                        <div class="form-group">
                            <asp:Label class="col-form-label" runat="server" ID="Label1" Text="CEP"></asp:Label>
                            <asp:TextBox id="txtCEP" runat="server" class="form-control"></asp:TextBox>
                        </div>
                    </div>
                </div>
                <div class="row">
                    <div class="col-12">
                        <center>
                        <asp:Button runat="server" ID="btnContinuarCEP" CssClass="btn btn-whitelabel1 btn-block" Text="Continuar" onclick="btnContinuarCEP_Click"/>
                        <br /><br />
                        </center>
                    </div>
                </div>
            </div>
            <div class="card-footer">
                <asp:Button runat="server" ID="btnVoltarCEP" CssClass="btn btn-whitelabel1 btn-block" Text="Voltar" onclick="btnVoltarCEP_Click"/>
            </div>
        </div>

        <!-- Card Endereço Dados ***************************************************************************************************************************************************************************** -->

        <div class="card" runat="server" id="crdEnderecoDados" visible="false">
            <div class="card-body login-card-body">
                <center>
                    <br />
                    <br />
                    <h3 class="card-text">Você confirma o endereço abaixo?</h3>
                    <br /><br />
                </center>

                <div class="row">
                    <div class="col-12">
                        <div class="form-group">
                            <asp:Label class="col-form-label" runat="server" ID="Label2" Text="Endereço"></asp:Label>
                            <asp:TextBox id="txtEndereco" runat="server" class="form-control"></asp:TextBox>
                        </div>
                    </div>
                </div>
                <div class="row">
                    <div class="col-12">
                        <div class="form-group">
                            <asp:Label class="col-form-label" runat="server" ID="Label3" Text="Bairro"></asp:Label>
                            <asp:TextBox id="txtBairro" runat="server" class="form-control"></asp:TextBox>
                        </div>
                    </div>
                </div>
                <div class="row">
                    <div class="col-12">
                        <div class="form-group">
                            <asp:Label class="col-form-label" runat="server" ID="Label4" Text="Cidade"></asp:Label>
                            <asp:TextBox id="txtCidade" runat="server" class="form-control"></asp:TextBox>
                        </div>
                    </div>
                </div>
                <div class="row">
                    <div class="col-12">
                        <div class="form-group">
                            <asp:Label class="col-form-label" runat="server" ID="Label5" Text="Estado"></asp:Label>
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
                <div class="row">
                    <div class="col-12">
                        <center>
                        <asp:Button runat="server" ID="btnContinuarEnderecoDados" CssClass="btn btn-whitelabel1 btn-block" Text="Continuar" onclick="btnContinuarEnderecoDados_Click"/>
                        <br /><br />
                        </center>
                    </div>
                </div>
            </div>
            <div class="card-footer">
                <asp:Button runat="server" ID="btnVoltarEnderecoDados" CssClass="btn btn-whitelabel1 btn-block" Text="Voltar" onclick="btnVoltarEnderecoDados_Click"/>
            </div>
        </div>

        <!-- Card Endereço Complemento ***************************************************************************************************************************************************************************** -->


        <div class="card" runat="server" id="crdEnderecoComplemento" visible="false">
            <div class="card-body login-card-body">
                <center>
                    <br />
                    <br />
                    <h3 class="card-text">Complete os dados do seu endereço</h3>
                    <br /><br />
                </center>

                <div class="row">
                    <div class="col-12">
                        <div class="form-group">
                            <asp:Label class="col-form-label" runat="server" ID="Label6" Text="Número"></asp:Label>
                            <asp:TextBox id="txtNumero" runat="server" class="form-control"></asp:TextBox>
                        </div>
                    </div>
                </div>
                <div class="row">
                    <div class="col-12">
                        <div class="form-group">
                            <asp:Label class="col-form-label" runat="server" ID="Label7" Text="Complemento"></asp:Label>
                            <asp:TextBox id="txtComplemento" runat="server" class="form-control"></asp:TextBox>
                        </div>
                    </div>
                </div>

                <div class="row">
                    <div class="col-12">
                        <center>
                        <asp:Button runat="server" ID="btnContinuarEnderecoComplemento" CssClass="btn btn-whitelabel1 btn-block" Text="Continuar" onclick="btnContinuarEnderecoComplemento_Click"/>
                        <br /><br />
                        </center>
                    </div>
                </div>
            </div>
            <div class="card-footer">
                <asp:Button runat="server" ID="btnVoltarEnderecoComplamento" CssClass="btn btn-whitelabel1 btn-block" Text="Voltar" onclick="btnVoltarEnderecoComplamento_Click"/>
            </div>
        </div>

        <!-- Card Pais ***************************************************************************************************************************************************************************** -->


        <div class="card" runat="server" id="crdPais" visible="false">
            <div class="card-body login-card-body">
                <center>
                    <br />
                    <br />
                    <h3 class="card-text">Em que País você nasceu?</h3>
                    <br /><br />
                </center>

                <div class="row">
                    <div class="col-12">
                        <div class="form-group">
                            <asp:Label class="col-form-label" runat="server" ID="Label8" Text="País"></asp:Label>
                            <asp:DropDownList runat="server" ID="ddlPais" CssClass="form-control"></asp:DropDownList>
                        </div>
                    </div>
                </div>

                <div class="row">
                    <div class="col-12">
                        <center>
                        <asp:Button runat="server" ID="btnContinuarPais" CssClass="btn btn-whitelabel1 btn-block" Text="Continuar" onclick="btnContinuarPais_Click"/>
                        <br /><br />
                        </center>
                    </div>
                </div>
            </div>
            <div class="card-footer">
                <asp:Button runat="server" ID="btnVoltarPais" CssClass="btn btn-whitelabel1 btn-block" Text="Voltar" onclick="btnVoltarPais_Click"/>
            </div>
        </div>

        <!-- Card Profissao ***************************************************************************************************************************************************************************** -->


        <div class="card" runat="server" id="crdProfissao" visible="false">
            <div class="card-body login-card-body">
                <center>
                    <br />
                    <br />
                    <h3 class="card-text">Qual é a sua profissão?</h3>
                    <br /><br />
                </center>

                <div class="row">
                    <div class="col-12">
                        <div class="form-group">
                            <asp:Label class="col-form-label" runat="server" ID="Label9" Text="Profissão"></asp:Label>
                            <asp:DropDownList runat="server" ID="ddlProfissao" CssClass="form-control"></asp:DropDownList>
                        </div>
                    </div>
                </div>

                <div class="row">
                    <div class="col-12">
                        <center>
                        <asp:Button runat="server" ID="btnContinuarProfissao" CssClass="btn btn-whitelabel1 btn-block" Text="Continuar" onclick="btnContinuarProfissao_Click"/>
                        <br /><br />
                        </center>
                    </div>
                </div>
            </div>
            <div class="card-footer">
                <asp:Button runat="server" ID="btnVoltarProfissao" CssClass="btn btn-whitelabel1 btn-block" Text="Voltar" onclick="btnVoltarProfissao_Click"/>
            </div>
        </div>

    </div>



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

<script src="../plugins/inputmask/jquery.inputmask.bundle.js"></script>
<script src="../plugins/moment/moment.min.js"></script>

<script>
    $(function () {
        //Datemask dd/mm/yyyy
        $('#txtCPF').inputmask('999.999.999-99')
        $('#txtNascimento').inputmask('99/99/9999')
        $('#txtCelular').inputmask('(99) 99999-9999')
    })
</script>


</body>
</html>
