<%@ Page Language="C#" AutoEventWireup="true" CodeFile="onboardingauth.aspx.cs" Inherits="onboardingauth" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
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

        .vh-75 {
          height: 75vh;
        }       

        .vh-90 {
          height: 90vh;
        }       

    </style>


</head>
<body runat="server" id="bdyLogin" class="hold-transition login-page vh-90 h-75">

<form runat="server" id="frmLogin">


<div class="row d-flex justify-content-center">

    <div class="col-sm-4">
        <div class="card rounded-left vh-90 mt-3">
            <div class="card-header">
                <div class="login-logo">
                    <div class="image">
                        <asp:Image alt="" runat="server" id="imgLogoPrincipal" CssClass="img-fluid"></asp:Image>
                    </div>  
                </div>
            </div>
            <!-- ************************************************************************************************************************************************************************* -->
            <!-- Etapa 1 - Solicitação do Documento ************************************************************************************************************************************** -->
            <!-- ************************************************************************************************************************************************************************* -->
            <div class="card-body login-card-body rounded-left" runat="server" id="dvDocumento" visible="false">
                <div class="row">
                    <div class="col-sm-12">
                        <center>
                        <h3>Agora ficou mais fácil abrir e gerenciar a sua conta</h3>
                        </center>
                        <h5 class="mt-2">1. Selecione abaixo a finalidade da sua conta</h5>
                    </div>
                </div>
                <div class="row">
                    <div class="col-sm-12">
                        <div class="form-group clearfix">
                            <div class="icheck-primary d-inline col">
                                <asp:RadioButton runat="server" id="rbFisica" GroupName="r1" checked 
                                    oncheckedchanged="rbFisica_CheckedChanged" AutoPostBack="True" />
                                <label for="rbFisica"> Para você</label>
                            </div>
                            <div class="icheck-primary d-inline col">
                                    <asp:RadioButton runat="server" id="rbJuridica" GroupName="r1" 
                                        oncheckedchanged="rbJuridica_CheckedChanged" AutoPostBack="True" />
                                    <label for="rbJuridica"> Para o seu negócio</label>
                            </div>
                        </div>               
                    </div>
                </div>
                <div class="row">
                    <div class="col-sm-12">
                        <div class="form-group">
                            <asp:Label runat="server" ID="lblCPF" Text="Digite o seu CPF"></asp:Label>
                            <asp:TextBox runat="server" ID="txtID" CssClass="form-control" Visible="false"></asp:TextBox>
                            <asp:TextBox runat="server" ID="txtGUID" CssClass="form-control" Visible="false"></asp:TextBox>
                            <asp:TextBox runat="server" ID="txtCPF" CssClass="form-control" ontextchanged="txtCPF_TextChanged" AutoPostBack="True"></asp:TextBox>
                        </div>
                    </div>
                </div>

            <div class="row" runat="server" id="dvDocumentof" visible="false">
                <div class="col-sm-12">
                    <asp:Button runat="server" ID="btnDocumento" CssClass="btn btn-whitelabel1 float-right text-white btn-sm" Text="Continuar" Enabled="false" onclick="btnDocumento_Click" />
                    <asp:Button runat="server" ID="btnCancelar" CssClass="btn btn-danger float-left text-white btn-sm" Text="Cancelar" Enabled="true" onclick="btnCancelar_Click"/>
                </div>
            </div>

            </div>

            <!-- ************************************************************************************************************************************************************************* -->
            <!-- Etapa 2 - Solicitação dos Dados CPF ************************************************************************************************************************************* -->
            <!-- ************************************************************************************************************************************************************************* -->
            <div class="card-body login-card-body rounded-left" runat="server" id="dvDados" visible="false">
                <div class="row">
                    <div class="col-sm-12">
                        <h5 class="mt-2"><asp:Label runat="server" ID="lblDados" Text = "2.Informe os seus dados"></asp:Label></h5>
                    </div>
                </div>
                <div class="row">
                    <div class="col-sm-6">
                        <div class="form-group">
                        <asp:Label runat="server" ID="lblNome" Text="Nome completo"></asp:Label>
                            <asp:TextBox runat="server" ID="txtNome" CssClass="form-control"></asp:TextBox>
                            <asp:TextBox runat="server" ID="txtSobrenome" CssClass="form-control" Visible="false"></asp:TextBox>
                        </div>
                    </div>
                    <div class="col-sm-6">
                        <div class="form-group">
                            <asp:Label runat="server" ID="lblNascimento" Text="Data de Nascimento"></asp:Label>
                            <asp:TextBox runat="server" ID="txtNascimento" CssClass="form-control" data-inputmask='"mask": "99/99/9999"' data-mask></asp:TextBox>
                        </div>
                    </div>
                </div>
                <!--
                <div class="row">
                    <div class="col-sm-6">
                        <div class="form-group">
                            <asp:Label runat="server" ID="lblNomeMae" Text="Nome da Mãe"></asp:Label>
                            <asp:TextBox runat="server" ID="txtNomeMae" CssClass="form-control" data-inputmask='"mask": "99/99/9999"' data-mask></asp:TextBox>
                        </div>
                    </div>
                </div>
                -->
                <div class="row">
                    <div class="col-sm-6">
                        <div class="form-group">
                            <asp:Label runat="server" ID="lblEmail" Text="E-mail"></asp:Label>
                            <asp:TextBox runat="server" ID="txtEmail" CssClass="form-control"></asp:TextBox>
                            <asp:TextBox runat="server" ID="txtSenha" CssClass="form-control" Visible="false"></asp:TextBox>

                        </div>
                    </div>
                    <div class="col-sm-6">
                        <div class="form-group">
                            <asp:Label runat="server" ID="lblCelular" Text="Celular"></asp:Label>
                            <asp:TextBox runat="server" ID="txtCelular" CssClass="form-control" data-inputmask='"mask": "(99) 99999-9999"' data-mask></asp:TextBox>
                        </div>
                    </div>
                </div>
                <div class="row">
                    <div class="col-sm-8">
                        <div class="form-group">
                            <asp:Label runat="server" ID="lblPPE" Text="Pessoa politicamente exposta?"></asp:Label>
                            <asp:DropDownList runat="server" id="ddlPoliticamenteExpostaPF" CssClass="form-control">
                                <asp:ListItem Value="N">Não</asp:ListItem>
                                <asp:ListItem Value="S">Sim</asp:ListItem>
                            </asp:DropDownList>
                        </div>
                    </div>

                    <div class="col-sm-4">
                        <div class="form-group">
                            <asp:Label runat="server" ID="lblRendaMensal" Text="Renda Mensal"></asp:Label>
                            <asp:DropDownList runat="server" id="ddlRendaMensal" CssClass="form-control">
                                <asp:ListItem Value="2500">R$ 0 à R$ 2.500,00 por mês</asp:ListItem>
                                <asp:ListItem Value="5000">R$ 2.500,00 à R$ 5.000,00 por mês</asp:ListItem>
                                <asp:ListItem Value="10000">R$ 5.000,00 à R$ 10.000,00 por mês</asp:ListItem>
                                <asp:ListItem Value="25000">R$ 10.000,00 à R$ 25.000,00 por mês</asp:ListItem>
                                <asp:ListItem Value="50000">R$ 25.000,00 à R$ 50.000,00 por mês</asp:ListItem>
                                <asp:ListItem Value="100000">R$ 50.000,00 à R$ 100.000,00 por mês</asp:ListItem>
                                <asp:ListItem Value="300000">R$ 100.000,00 à R$ 300.000,00 por mês</asp:ListItem>
                                <asp:ListItem Value="500000">R$ 300.000,00 à R$ 500.000,00 por mês</asp:ListItem>
                                <asp:ListItem Value="1000000">R$ 500.000,00 à R$ 1.000.000,00 por mês</asp:ListItem>
                                <asp:ListItem Value="5000000">R$ 1.000.000,00 à R$ 5.000.000,00 por mês</asp:ListItem>
                                <asp:ListItem Value="10000000">R$ 5.000.000,00 à R$ 10.000.000,00 por mês</asp:ListItem>
                                <asp:ListItem Value="20000000">R$ 10.000.000,00 à R$ 20.000.000,00 por mês</asp:ListItem>
                                <asp:ListItem Value="50000000">Acima de R$ 20.000.000,00 por mês</asp:ListItem>
                            </asp:DropDownList>

                        </div>
                    </div>

                </div>
                <div class="row" runat="server" id="dvDadosf" visible="false">
                    <div class="col-sm-12">
                        <asp:Button runat="server" ID="btnDados" CssClass="btn btn-whitelabel1 text-white btn-sm float-right" Text="Continuar"  Enabled="true" onclick="btnDados_Click" />
                        <asp:Button runat="server" ID="btnDadosV" CssClass="btn btn-danger text-white btn-sm float-left" Text="Voltar" Enabled="true" onclick="btnDadosV_Click" />
                    </div>
                </div>

            </div>


            <!-- ************************************************************************************************************************************************************************* -->
            <!-- Etapa 3 - Código enviado por e-mail ************************************************************************************************************************************* -->
            <!-- ************************************************************************************************************************************************************************* -->
            <div class="card-body login-card-body rounded-left" runat="server" id="dvCodigo" visible="false">
                <div class="row">
                    <div class="col-sm-12">
                        <h5 class="mt-2">3. Digite o código que enviamos por E-MAIL</h5>
                    </div>
                </div>
                <div class="row">
                    <div class="col-sm-12">
                        <div class="form-group">
                            <asp:Label runat="server" ID="lblCodigo" Text="Digite o código"></asp:Label>
                            <asp:TextBox runat="server" ID="txtCodigo" CssClass="form-control"></asp:TextBox>
                        </div>
                    </div>
                </div>
                <div class="row">
                    <div class="col-sm-12">
                        <p class="mt-1">
                            <strong>Não recebeu?  </strong>   <asp:LinkButton ID="lkbReenviar" runat="server" onclick="lkbReenviar_Click">Enviar novo código</asp:LinkButton>
                        </p>
                    </div>
                </div>

                <div class="row" runat="server" id="dvCodigof" visible="false">
                    <div class="col-sm-12">
                        <asp:Button runat="server" ID="btnCodigo" CssClass="btn btn-whitelabel1 text-white btn-sm float-right" Text="Continuar"  Enabled="true" onclick="btnCodigo_Click" />
                        <asp:Button runat="server" ID="btnCodigoV" CssClass="btn btn-danger text-white btn-sm float-left" Text="Voltar" Enabled="true" onclick="btnCodigoV_Click" />
                    </div>
                </div>



            </div>



            <!-- ************************************************************************************************************************************************************************* -->
            <!-- Etapa 4 - Endereço ************************************************************************************************************************************* -->
            <!-- ************************************************************************************************************************************************************************* -->
            <div class="card-body login-card-body rounded-left" runat="server" id="dvEndereco" visible="false">
                <div class="row">
                    <div class="col-sm-12">
                        <h5 class="mt-2">3. Qual o seu CEP?</h5>
                    </div>
                </div>
                <div class="row">
                    <div class="col-sm-12">
                        <div class="form-group">
                            <asp:Label runat="server" ID="lblCEP" Text="Digite o CEP"></asp:Label>
                            <asp:TextBox runat="server" ID="txtCEP" CssClass="form-control" data-inputmask='"mask": "99999-999"' data-mask AutoPostBack="True" ontextchanged="txtCEP_TextChanged" ></asp:TextBox>
                        </div>
                    </div>
                </div>

                <div class="row">
                    <div class="col-sm-12">
                        <div class="form-group">
                            <asp:Label runat="server" ID="lblEndereco" Text="Endereço"></asp:Label>
                            <asp:TextBox runat="server" ID="txtEndereco" CssClass="form-control"></asp:TextBox>
                        </div>
                    </div>
                </div>
                <div class="row">
                    <div class="col-sm-6">
                        <div class="form-group">
                            <asp:Label runat="server" ID="lblNumero" Text="Número"></asp:Label>
                            <asp:TextBox runat="server" ID="txtNumero" CssClass="form-control"></asp:TextBox>
                        </div>
                    </div>
                    <div class="col-sm-6">
                        <div class="form-group">
                            <asp:Label runat="server" ID="lblComplemento" Text="Complemento"></asp:Label>
                            <asp:TextBox runat="server" ID="txtComplemento" CssClass="form-control"></asp:TextBox>
                        </div>
                    </div>
                </div>
                <div class="row">
                    <div class="col-sm-4">
                        <div class="form-group">
                            <asp:Label runat="server" ID="lblBairro" Text="Bairro"></asp:Label>
                            <asp:TextBox runat="server" ID="txtBairro" CssClass="form-control"></asp:TextBox>
                        </div>
                    </div>
                    <div class="col-sm-4">
                        <div class="form-group">
                            <asp:Label runat="server" ID="lblCidade" Text="Cidade"></asp:Label>
                            <asp:TextBox runat="server" ID="txtCidade" CssClass="form-control"></asp:TextBox>
                        </div>
                    </div>
                    <div class="col-sm-4">
                        <div class="form-group">
                            <asp:Label runat="server" ID="lblEstado" Text="Estado"></asp:Label>
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

                <div class="row" runat="server" id="dvEnderecof" visible="false">
                    <div class="col-sm-12">
                        <asp:Button runat="server" ID="btnEndereco" CssClass="btn btn-whitelabel1 float-right text-white btn-sm" Text="Continuar" Enabled="true" onclick="btnEndereco_Click" />
                        <asp:Button runat="server" ID="btnEnderecov" CssClass="btn btn-danger float-left text-white btn-sm" Text="Voltar" Enabled="true" onclick="btnEnderecoV_Click" />
                    </div>
                </div>


            </div>

            <!-- ************************************************************************************************************************************************************************* -->
            <!-- Etapa 4 - Solicitação dos Dados CNPJ ************************************************************************************************************************************* -->
            <!-- ************************************************************************************************************************************************************************* -->
            <div class="card-body login-card-body rounded-left" runat="server" id="dvJuridica" visible="false">
                <div class="row">
                    <div class="col-sm-12">
                        <h5 class="mt-2"><asp:Label runat="server" ID="lblJuridica" Text = "4.Informe os dados da empresa"></asp:Label></h5>
                    </div>
                </div>
                <div class="row">
                    <div class="col-sm-4">
                        <div class="form-group">
                        <asp:Label runat="server" ID="Label3" Text="CNPJ"></asp:Label>
                            <asp:TextBox runat="server" ID="txtCNPJ" CssClass="form-control"></asp:TextBox>
                        </div>
                    </div>
                    <div class="col-sm-4">
                        <div class="form-group">
                        <asp:Label runat="server" ID="Label2" Text="Razão Social"></asp:Label>
                            <asp:TextBox runat="server" ID="txtRazaoSocial" CssClass="form-control"></asp:TextBox>
                        </div>
                    </div>
                    <div class="col-sm-4">
                        <div class="form-group">
                        <asp:Label runat="server" ID="Label1" Text="Nome Fantasia"></asp:Label>
                            <asp:TextBox runat="server" ID="txtNomeFantasia" CssClass="form-control"></asp:TextBox>
                        </div>
                    </div>
                </div>
                <div class="row">
                    <div class="col-sm-4">
                        <div class="form-group">
                            <asp:Label runat="server" ID="lblAbertura" Text="Data início atividades"></asp:Label>
                            <asp:TextBox runat="server" ID="txtAbertura" CssClass="form-control" data-inputmask='"mask": "99/99/9999"' data-mask></asp:TextBox>
                        </div>
                    </div>
                    <div class="col-sm-4">
                        <div class="form-group">
                            <asp:Label runat="server" ID="lblTipoEmpresa" Text="Tipo da Empresa"></asp:Label>
                            <asp:DropDownList runat="server" id="ddlTipoEmpresa" CssClass="form-control">
                                <asp:ListItem Value="MEI">MEI</asp:ListItem>
                                <asp:ListItem Value="INDIVIDUAL">Individual</asp:ListItem>
                                <asp:ListItem Value="LIMITED">Limitada</asp:ListItem>
                            </asp:DropDownList>
                        </div>
                    </div>
                    <div class="col-sm-4">
                        <div class="form-group">
                            <asp:Label runat="server" ID="lblFaturamento" Text="Faturamento"></asp:Label>
                            <asp:DropDownList runat="server" id="ddlFaturamento" CssClass="form-control">
                                <asp:ListItem Value="2500">R$ 0 à R$ 2.500,00 por mês</asp:ListItem>
                                <asp:ListItem Value="5000">R$ 2.500,00 à R$ 5.000,00 por mês</asp:ListItem>
                                <asp:ListItem Value="10000">R$ 5.000,00 à R$ 10.000,00 por mês</asp:ListItem>
                                <asp:ListItem Value="25000">R$ 10.000,00 à R$ 25.000,00 por mês</asp:ListItem>
                                <asp:ListItem Value="50000">R$ 25.000,00 à R$ 50.000,00 por mês</asp:ListItem>
                                <asp:ListItem Value="100000">R$ 50.000,00 à R$ 100.000,00 por mês</asp:ListItem>
                                <asp:ListItem Value="300000">R$ 100.000,00 à R$ 300.000,00 por mês</asp:ListItem>
                                <asp:ListItem Value="500000">R$ 300.000,00 à R$ 500.000,00 por mês</asp:ListItem>
                                <asp:ListItem Value="1000000">R$ 500.000,00 à R$ 1.000.000,00 por mês</asp:ListItem>
                                <asp:ListItem Value="5000000">R$ 1.000.000,00 à R$ 5.000.000,00 por mês</asp:ListItem>
                                <asp:ListItem Value="10000000">R$ 5.000.000,00 à R$ 10.000.000,00 por mês</asp:ListItem>
                                <asp:ListItem Value="20000000">R$ 10.000.000,00 à R$ 20.000.000,00 por mês</asp:ListItem>
                                <asp:ListItem Value="50000000">Acima de R$ 20.000.000,00 por mês</asp:ListItem>
                            </asp:DropDownList>

                        </div>
                    </div>
                </div>

                <div class="row" runat="server" id="dvJuridicaf" visible="false">
                    <div class="col-sm-12">
                        <asp:Button runat="server" ID="btnJuridica" CssClass="btn btn-whitelabel1 text-white btn-sm float-right" Text="Continuar" Enabled="true" onclick="btnJuridica_Click" />
                        <asp:Button runat="server" ID="btnJuridicaV" CssClass="btn btn-danger text-white btn-sm float-left" Text="Voltar" Enabled="true" onclick="btnJuridicaV_Click" />
                    </div>
                </div>


            </div>

            <!-- ************************************************************************************************************************************************************************* -->
            <!-- Etapa 5 - Confirmação ************************************************************************************************************************************* -->
            <!-- ************************************************************************************************************************************************************************* -->
            <div class="card-body login-card-body rounded-left" runat="server" id="dvConfirmacao" visible="false">
                <div class="row">
                    <div class="col-sm-12">
                        <center>
                        <h3>Tudo pronto!</h3>
                        </center>
                    </div>
                </div>
                <div class="row">
                    <div class="col-sm-12">
                        <div class="alert alert-warning alert-dismissible mt-2 mb-2">
                            <button type="button" class="close" data-dismiss="alert" aria-hidden="true">&times;</button>
                            <h5><i class="icon fas fa-exclamation-triangle"></i> ATENÇÃO!</h5><br />
                            1. Você receberá um e-mail com instruções para acessar SISTEMA WEB.<br /><br />
                            2. No primeiro acesso você será 
                            poderá criar a sua conta digital e enviar os seus documentos para análise.<br /><br />
                        </div>
                    </div>
                </div>


                <div class="row" runat="server" id="dvConfirmacaof" visible="false">
                    <div class="col-sm-12">
                        <asp:Button runat="server" ID="btnConfirmacao" CssClass="btn btn-whitelabel1 float-right text-white btn-sm" Text="Criar Cadastro" Enabled="true" onclick="btnConfirmacao_Click" />
                        <asp:Button runat="server" ID="btnConfirmacaoV" CssClass="btn btn-danger float-left text-white btn-sm" Text="Voltar" Enabled="true" onclick="btnConfirmacaoV_Click" />
                    </div>
                </div>


            </div>


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

<script>
    $(function () {
        //Datemask dd/mm/yyyy

        $('#txtNascimento').inputmask('99/99/9999')
        $('#txtAbertura').inputmask('99/99/9999')
        $('#txtCPF').inputmask('999.999.999-99')
        $('#txtCNPJ').inputmask('99.999.999/9999-99')
        $('#txtCelular').inputmask('(99) 99999-9999')
        $('#txtCEP').inputmask('99999-999')

    })
</script>


</body>
</html>
