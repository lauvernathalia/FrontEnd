<%@ Page Language="C#" AutoEventWireup="true" CodeFile="cad_estabelecimentos_adm.aspx.cs" Inherits="cad_estabelecimentos_adm" %>

<%@ Register TagPrefix="Portal" TagName="PageBottom" Src="rodapepadrao.ascx" %>
<%@ Register TagPrefix="Portal" TagName="PageLeft" Src="menupadrao.ascx" %>
<%@ Register TagPrefix="Portal" TagName="PageHeader" Src="topopadrao.ascx" %>
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

        <div class="wrapper">
            <PORTAL:PAGEHEADER id="PageHeader1" title="Site Directory" runat="server" ModuleSource="topopadrao.ascx"></PORTAL:PAGEHEADER>
            <PORTAL:PAGELEFT id="Pageheader2" title="Site Directory" runat="server" ModuleSource="menupadrao.ascx"></PORTAL:PAGELEFT>
            <div class="content-wrapper">
                <section class="content">
                    <div class="container-fluid" >

                        <div class="row mb-2">
                          <div class="col-sm-12">
                            <ol class="breadcrumb">
                              <li class="breadcrumb-item"><a href="#">Cadastros</a></li>
                              <li class="breadcrumb-item active">Estabelecimentos</li>
                            </ol>
                          </div>
                        </div>

                        <div class="row">
                            <div class="col-md-12 col-xs-12 col-12 text-center">

                                <div class="row">
                                <div class="col-sm-12">
                                <center>
                                <div class="card card-timeline px-2 border-none"> 
                                        <ul class="bs4-order-tracking"> 
                                        <li runat="server" id="li1" class="step"> 
                                            <div>
                                                1
                                            </div>
                                        </li> 
                                        <li runat="server" id="li2" class="step"> 
                                            <div>
                                                2
                                            </div> 
                                        </li> 
                                        <li runat="server" id="li3" class="step"> 
                                            <div>
                                                3
                                            </div> 
                                        </li>   
                                        <li runat="server" id="li4" class="step"> 
                                            <div>
                                                4
                                            </div> 
                                        </li> 
                                        <li runat="server" id="li5" class="step"> 
                                            <div>
                                                5
                                            </div> 
                                        </li> 
                                        <li runat="server" id="li6" class="step" visible="false"> 
                                            <div>
                                                6
                                            </div> 
                                        </li> 
                                        </ul>
                                    <h4 class="text-secondary"><strong>Criar conta de Usuário</strong></h4>
                                    <h5 class="text-secondary">Preencha os dados corretamenta par criar o acesso do estabelecimento</h5>
                                </div>
                                </center>
                                </div>
                                </div>
                            </div>
                        </div>
                        <!-- Primeira Parte do cadastro -->
                        <div class="row" runat="server" id="div1" visible="true">
                            <div class="col-md-12 col-xs-12 col-12">
                                <div class="card">
                                    <div class="card-header bg-whitelabel1">
                                        <h4>Tipo do Usuário</h4>
                                    
                                    </div>
                                    <div class="card-body">
                                          <div class="form-group">
                                            <label>Tipo de pessoa (PF ou PJ)<strong class="text-danger">*</strong></label>
                                            <asp:DropDownList runat="server" id="ddlTipoFJ" CssClass="form-control" 
                                                  AutoPostBack="True" onselectedindexchanged="ddlTipoFJ_SelectedIndexChanged">
                                                <asp:ListItem Value="PF">Pessoa Física</asp:ListItem>
                                                <asp:ListItem Value="PJ">Pessoa Jurídica</asp:ListItem>
                                            </asp:DropDownList>
                                          </div>
                                    </div>
                                    <div class="card-footer">
                                        <asp:Button runat="server" id="btnProximo1" Text="Próximo >>" 
                                            class="btn btn-outline btn-whitelabel1 float-right"  enabled="true" 
                                            onclick="btnProximo_Click"/>
                                    </div>
                                
                                </div>
                            
                            </div>
                        </div>

                        <!-- Segunda Parte do cadastro -->
                        <div class="row" runat="server" id="div2" visible="true">
                            <div class="col-md-12 col-xs-12 col-12">
                                <div class="card">
                                    <div class="card-header bg-whitelabel1">
                                        <h4>Dados do Usuário</h4>
                                    </div>
                                    <div class="card-body">
                                        <div class="row">
                                          <div class="col-sm-6"  >
                                              <div class="form-group">
                                                <label>Nome<strong class="text-danger">*</strong></label>
                                                <asp:TextBox runat="server" ID="txtNome" CssClass="form-control" placeholder="Nome" required></asp:TextBox> 
                                              </div>
                                          </div>
                                          

                                          <div class="col-sm-6"  >
                                              <div class="form-group">
                                                <label>Sobrenome<strong class="text-danger">*</strong></label>
                                                <asp:TextBox runat="server" ID="txtSobrenome" CssClass="form-control" placeholder="Sobrenome" required></asp:TextBox>
                                              </div>
                                          </div>
                                        </div>


                                        <div class="row">
                                          <div class="col-sm-6"  >
                                              <div class="form-group">
                                                <label>Data Nascimento<strong class="text-danger">*</strong></label>
                                                <asp:TextBox runat="server" ID="txtNascimento" CssClass="form-control" placeholder="00/00/0000" required></asp:TextBox> 
                                              </div>
                                          </div>
                                          

                                          <div class="col-sm-6"  >
                                              <div class="form-group">
                                                <label>CPF<strong class="text-danger">*</strong></label>
                                                <asp:TextBox runat="server" ID="txtCPF" CssClass="form-control" placeholder="000.000.000-00" required data-inputmask='"mask": "999.999.999-99"' data-mask></asp:TextBox>
                                              </div>
                                          </div>
                                        </div>

                                        <div class="row">
                                          <div class="col-sm-6"  >
                                              <div class="form-group">
                                                <label>Nome completo da mãe<strong class="text-danger">*</strong></label>
                                                <asp:TextBox runat="server" ID="txtNomeMae" CssClass="form-control" placeholder="Digite o nome completo da mãe" required></asp:TextBox> 
                                              </div>
                                          </div>
                                          

                                          <div class="col-sm-6"  >
                                              <div class="form-group">
                                                <label>E-Mail do usuário<strong class="text-danger">*</strong></label>
                                                <asp:TextBox runat="server" ID="txtEmail" CssClass="form-control" placeholder="nome@dominio.com" required></asp:TextBox>
                                              </div>
                                          </div>
                                        </div>

                                        <div class="row">
                                          <div class="col-sm-6"  >
                                              <div class="form-group">
                                                <label>Senha<strong class="text-danger">*</strong></label>
                                                <asp:TextBox runat="server" ID="txtSenha" CssClass="form-control" 
                                                      placeholder="Digite a senha" required TextMode="Password" ></asp:TextBox> 
                                                

                                              </div>
                                          </div>
                                          

                                          <div class="col-sm-6">
                                              <div class="form-group">
                                                <label>Confirmar a Senha<strong class="text-danger">*</strong></label>
                                                <asp:TextBox runat="server" ID="txtSenhaConfirmar" CssClass="form-control" 
                                                      placeholder="Repita a senha" required TextMode="Password" 
                                                      ontextchanged="txtSenhaConfirmar_TextChanged" AutoPostBack="True"></asp:TextBox>
                                                
                                              </div>
                                          </div>
                                        </div>


                                        <div class="row">
                                          <div class="col-sm-6"  >
                                              <div class="form-group">
                                                <label>celular<strong class="text-danger">*</strong></label>
                                                <asp:TextBox runat="server" ID="txtCelular" CssClass="form-control" placeholder="(11) 99999-9999" data-inputmask='"mask": "(99) 99999-9999"' data-mask required></asp:TextBox> 
                                              </div>
                                          </div>
                                          

                                        </div>


                                    </div>
                                    <div class="card-footer">
                                        <asp:Button runat="server" id="btnAnterior2" Text="<< Anterior" 
                                            class="btn btn-outline btn-whitelabel1 float-left" enabled="true" 
                                            onclick="btnAnterior_Click"/>
                                        <asp:Button runat="server" id="btnProximo2" Text="Próximo >>" 
                                            class="btn btn-outline btn-whitelabel1 float-right"  enabled="true" 
                                            onclick="btnProximo_Click"/>
                                    </div>
                                
                                </div>
                            
                            </div>
                        </div>



                        <!-- Terceira Parte do cadastro -->
                        <div class="row" runat="server" id="div3" visible="true">
                            <div class="col-md-12 col-xs-12 col-12">
                                <div class="card">
                                    <div class="card-header bg-whitelabel1">
                                        <h4>Dados da Conta</h4>
                                    </div>
                                    <div class="card-body">
                                        
                                        <div class="row">
                                          <div class="col-sm-12">
                                              <div class="form-group">
                                                <label>Modalidade da Conta<strong class="text-danger">*</strong></label>
                                                <asp:DropDownList runat="server" id="ddlModalidade" CssClass="form-control" 
                                                      AutoPostBack="True" onselectedindexchanged="ddlModalidade_SelectedIndexChanged">
                                                    <asp:ListItem Value="B">Banking</asp:ListItem>
                                                    <asp:ListItem Value="A">Adquirência</asp:ListItem>
                                                </asp:DropDownList>
                                              </div>
                                            </div>
                                        </div>


                                        <div class="row" runat="server" id="dvFormaRecebimento">
                                          <div class="col-sm-12">
                                              <div class="form-group">
                                                <label>Forma de Recebimento<strong class="text-danger">*</strong></label>
                                                <asp:DropDownList runat="server" id="ddlFormaRecebimento" CssClass="form-control">
                                                    <asp:ListItem Value="O">Outra conta bancária</asp:ListItem>
                                                    <asp:ListItem Value="P">PagBank</asp:ListItem>
                                                </asp:DropDownList>
                                              </div>
                                            </div>
                                        </div>

                                        <div class="row" runat="server" id="dvDadosBancarios">
                                            <div class="col-sm-6">
                                                <div class="form-group">
                                                <label>Instituição Financeira<strong class="text-danger">*</strong></label>
                                                <asp:DropDownList runat="server" id="ddlInstituicaoFinanceira" CssClass="form-control">
                                                </asp:DropDownList>
                                                </div>
                                            </div>

                                            <div class="col-sm-6">
                                                <div class="form-group">
                                                <label>Tipo de Conta<strong class="text-danger">*</strong></label>
                                                <asp:DropDownList runat="server" id="ddlTipoConta" CssClass="form-control">
                                                    <asp:ListItem Value="C">Conta Corrente</asp:ListItem>
                                                    <asp:ListItem Value="P">Conta Poupança</asp:ListItem>
                                                </asp:DropDownList>
                                                </div>
                                            </div>

                                            <div class="col-sm-4">
                                                <div class="form-group">
                                                <label>Agência<strong class="text-danger">*</strong></label>
                                                <asp:TextBox runat="server" ID="txtAgencia" CssClass="form-control" placeholder="Número da Agencia" ></asp:TextBox>
                                                </div>
                                            </div>

                                            <div class="col-sm-4">
                                                <div class="form-group">
                                                <label>Dígito<strong class="text-danger">*</strong></label>
                                                <asp:TextBox runat="server" ID="txtDigitoAgencia" CssClass="form-control" placeholder="Dígito" ></asp:TextBox>
                                                </div>
                                            </div>
                                            <div class="col-sm-4">
                                                <div class="form-group">
                                                <label>Conta<strong class="text-danger">*</strong></label>
                                                <asp:TextBox runat="server" ID="txtConta" CssClass="form-control" placeholder="Número da Conta" ></asp:TextBox>
                                                </div>
                                            </div>
                                            <div class="col-sm-4">
                                                <div class="form-group">
                                                <label>Dígito<strong class="text-danger">*</strong></label>
                                                <asp:TextBox runat="server" ID="txtDigitoConta" CssClass="form-control" placeholder="Dígito" ></asp:TextBox>
                                                </div>
                                            </div>


                                        </div>


                                    </div>
                                    <div class="card-footer">
                                        <asp:Button runat="server" id="btnAnterior3" Text="<< Anterior" 
                                            class="btn btn-outline btn-whitelabel1 float-left" enabled="true" 
                                            onclick="btnAnterior_Click"/>
                                        <asp:Button runat="server" id="btnProximo3" Text="Próximo >>" 
                                            class="btn btn-outline btn-whitelabel1 float-right"  enabled="true" 
                                            onclick="btnProximo_Click"/>
                                    </div>
                                
                                </div>
                            
                            </div>
                        </div>


                        <!-- Quarta Parte do cadastro -->
                        <div class="row" runat="server" id="div4" visible="true">
                            <div class="col-md-12 col-xs-12 col-12">
                                <div class="card">
                                    <div class="card-header bg-whitelabel1">
                                        <h4>Dados do Estabelecimento</h4>
                                    </div>
                                    <div class="card-body">

                                        <!-- Pessoa Física -->
                                        <div runat="server" id="dvPF">

                                            <div class="row">
                                              <div class="col-sm-6">
                                                  <div class="form-group">
                                                    <label>Ramo de Atividade<strong class="text-danger">*</strong></label>
                                                    <asp:DropDownList runat="server" id="ddlRamoAtividade" CssClass="form-control">
                                                    </asp:DropDownList>
                                                  </div>
                                              </div>
                                              <div class="col-sm-6">
                                                  <div class="form-group">
                                                    <label>Renda Mensal<strong class="text-danger">*</strong></label>
                                                    <asp:TextBox runat="server" id="txtRendaMensalPF" CssClass="form-control" placeholder="0,00">
                                                    </asp:TextBox>
                                                  </div>
                                              </div>
                                            </div>
                                        </div>

                                        
                                        <!-- Pessoa Jurídica -->
                                        <div runat="server" id="dvPJ">
                                        
                                            <div class="row">

                                              <div class="col-sm-6">
                                                  <div class="form-group">
                                                    <label>Razão Social<strong class="text-danger">*</strong></label>
                                                    <asp:TextBox runat="server" id="txtRazaoSocial" cssClass="form-control" required placeholder="Razão Social da Empresa"></asp:TextBox>
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

                                              <div class="col-sm-6">
                                                  <div class="form-group">
                                                    <label>CNPJ<strong class="text-danger">*</strong></label>
                                                    <asp:TextBox runat="server" id="txtCNPJ" cssClass="form-control" required placeholder="00.000.000/0000-00" data-inputmask='"mask": "99.999.999/9999-99"' data-mask></asp:TextBox>
                                                  </div>
                                                </div>
                                              <div class="col-sm-6">
                                                  <div class="form-group">
                                                    <label>Telefone Empresa<strong class="text-danger">*</strong></label>
                                                    <asp:TextBox runat="server" id="txtTelefoneEmpresa" CssClass="form-control" placeholder="(99) 99999-9999" data-inputmask='"mask": "(99) 99999-9999"' data-mask>
                                                    </asp:TextBox>
                                                  </div>
                                                </div>

                                            </div>


                                            <div class="row">

                                              <div class="col-sm-6">
                                                  <div class="form-group">
                                                    <label>Tipo da Empresa<strong class="text-danger">*</strong></label>
                                                    <asp:DropDownList runat="server" id="ddlTipoEmpresa" CssClass="form-control">
                                                        <asp:ListItem Value="MEI">MEI</asp:ListItem>
                                                        <asp:ListItem Value="Empresário Individual">Empresário Individual</asp:ListItem>
                                                        <asp:ListItem Value="Sociedade Limitada Unipessoal">Sociedade Limitada Unipessoal</asp:ListItem>
                                                        <asp:ListItem Value="Sociedade Empresária Limitada">Sociedade Empresária Limitada</asp:ListItem>
                                                        <asp:ListItem Value="Sociedade Simples">Sociedade Simples</asp:ListItem>
                                                        <asp:ListItem Value="Sociedade Anônima">Sociedade Anônima</asp:ListItem>

                                                    </asp:DropDownList>


                                                  </div>
                                                </div>
                                              <div class="col-sm-6">
                                                  <div class="form-group">
                                                    <label>Data Abertura da Empresa<strong class="text-danger">*</strong></label>
                                                    <asp:TextBox runat="server" id="txtDataAbertura" CssClass="form-control" placeholder="00/00/0000" data-inputmask='"mask": "99/99/9999"' data-mask>
                                                    </asp:TextBox>
                                                  </div>
                                                </div>

                                            </div>


                                            <div class="row">

                                              <div class="col-sm-6">
                                                  <div class="form-group">
                                                    <label>Atividade Econômica<strong class="text-danger">*</strong></label>
                                                    <asp:DropDownList runat="server" id="ddlAtividadeEconomica" CssClass="form-control">
                                                    </asp:DropDownList>
                                                  </div>
                                                </div>
                                              <div class="col-sm-6">
                                                  <div class="form-group">
                                                    <label>Faturamento Mensal<strong class="text-danger">*</strong></label>
                                                    <asp:TextBox runat="server" id="txtFaturamentoMensal" CssClass="form-control" placeholder="0,00">
                                                    </asp:TextBox>
                                                  </div>

                                                </div>

                                            </div>
                        

                                            <div class="row">
                                              <div class="col-sm-6">
                                                  <div class="form-group">
                                                    <label>E-mail da Empresa<strong class="text-danger">*</strong></label>
                                                    <asp:TextBox runat="server" id="txtEmailEmpresa" CssClass="form-control">
                                                    </asp:TextBox>

                                                  </div>
                                                </div>

                                            </div>





                                        </div>

                                        <div class="row">
                                            <div class="col-sm-6">
                                                <div class="form-group">
                                                <label>Você visitou este estabelecimento presencialmente?<strong class="text-danger">*</strong></label>
                                                <asp:DropDownList runat="server" id="ddlPresencial" CssClass="form-control">
                                                    <asp:ListItem Value="S">Sim</asp:ListItem>
                                                    <asp:ListItem Value="N">Não</asp:ListItem>
                                                </asp:DropDownList>
                                                </div>
                                            </div>
                                            <div class="col-sm-6">
                                                <div class="form-group">
                                                <label>É pessoa politicamente exposta?<strong class="text-danger">*</strong></label>
                                                <asp:DropDownList runat="server" id="ddlPoliticamenteExposta" CssClass="form-control">
                                                    <asp:ListItem Value="S">Sim</asp:ListItem>
                                                    <asp:ListItem Value="N">Não</asp:ListItem>
                                                </asp:DropDownList>
                                                </div>
                                            </div>

                                        </div>

                                        <div class="row">
                                            <div class="col-sm-6">
                                                <div class="form-group">
                                                <label>Patrimônio<strong class="text-danger">*</strong></label>
                                                <asp:TextBox runat="server" id="txtPatrimonio" CssClass="form-control" placeholder="0,00">
                                                </asp:TextBox>
                                                </div>
                                            </div>
                                        </div>


                                    </div>

                                    <div class="card-footer">
                                        <asp:Button runat="server" id="btnAnterior4" Text="<< Anterior" 
                                            class="btn btn-outline btn-whitelabel1 float-left" enabled="true" 
                                            onclick="btnAnterior_Click"/>
                                        <asp:Button runat="server" id="btnProximo4" Text="Próximo >>" 
                                            class="btn btn-outline btn-whitelabel1 float-right"  enabled="true" 
                                            onclick="btnProximo_Click"/>
                                    </div>
                                
                                </div>
                            
                            </div>
                        </div>

                        <!-- Quinta Parte do cadastro -->
                        <div class="row" runat="server" id="div5" visible="true">
                            <div class="col-md-12 col-xs-12 col-12">
                                <div class="card">
                                    <div class="card-header bg-whitelabel1">
                                        <h4>Dados do Endereço</h4>
                                    </div>
                                    <div class="card-body">

                                        <div class="row">
                                          <div class="col-sm-6"  >
                                              <div class="form-group">
                                                <label>CEP<strong class="text-danger">*</strong></label>
                                                <asp:TextBox runat="server" ID="txtCEP" CssClass="form-control" 
                                                      placeholder="00000000" data-inputmask='"mask": "99999-999"' data-mask required 
                                                      AutoPostBack="True" ontextchanged="txtCEP_TextChanged"></asp:TextBox> 
                                              </div>
                                          </div>
                                        </div>


                                        <div class="row">
                                          <div class="col-sm-6"  >
                                              <div class="form-group">
                                                <label>Endereço<strong class="text-danger">*</strong></label>
                                                <asp:TextBox runat="server" ID="txtEndereco" CssClass="form-control" placeholder="Digite o seu endereço" required></asp:TextBox> 
                                              </div>
                                          </div>
                                        </div>

                                        <div class="row">
                                          <div class="col-sm-6"  >
                                              <div class="form-group">
                                                <label>Número<strong class="text-danger">*</strong></label>
                                                <asp:TextBox runat="server" ID="txtNumero" CssClass="form-control" placeholder="Digite o número do endereço" required></asp:TextBox> 
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
                                                <asp:TextBox runat="server" ID="txtBairro" CssClass="form-control" placeholder="Digite o bairro" required ></asp:TextBox> 
                                              </div>
                                          </div>
                                          <div class="col-sm-5"  >
                                              <div class="form-group">
                                                <label>Cidade<strong class="text-danger">*</strong></label>
                                                <asp:TextBox runat="server" ID="txtCidade" CssClass="form-control" placeholder="Digite a cidade" required ></asp:TextBox> 
                                              </div>
                                          </div>
                                          
                                          <div class="col-sm-2"  >
                                              <div class="form-group">
                                                <label>Estado<strong class="text-danger">*</strong></label>
                                                <asp:DropDownList id="ddlEstado" runat="server" class="form-control" required>
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

                                    </div>
                                    <div class="card-footer">
                                        <asp:Button runat="server" id="btnAnterior5" Text="<< Anterior" 
                                            class="btn btn-outline btn-whitelabel1 float-left" enabled="true" 
                                            onclick="btnAnterior_Click"/>
                                        <asp:Button runat="server" id="btnSalvar" Text="Salvar" 
                                            class="btn btn-outline btn-whitelabel1 float-right"  enabled="true" 
                                            onclick="btnSalvar_Click"/>
                                    </div>
                                
                                </div>
                            
                            </div>
                        </div>

                        <!-- Sexta Parte do cadastro -->
                        <div class="row" runat="server" id="div6" visible="false">
                            <div class="col-md-12 col-xs-12 col-12">
                                <div class="card">
                                    <div class="card-header bg-whitelabel1">
                                        <h4>Conferência dos Dados</h4>
                                    </div>
                                    <div class="card-body">

                                    </div>
                                    <div class="card-footer">
                                        <asp:Button runat="server" id="btnAnterior6" Text="<< Anterior" 
                                            class="btn btn-outline btn-whitelabel1 float-left" enabled="true" 
                                            onclick="btnAnterior_Click"/>
                                    </div>
                                
                                </div>
                            
                            </div>
                        </div>


                    </div>
                </section>
            </div>
            <PORTAL:PAGEBOTTOM id="Pageheader3" title="Site Directory" runat="server" ModuleSource="rodapepadrao.ascx"></PORTAL:PAGEBOTTOM><!-- Fim Rodapé da Pagina -->
    
        </div>
    </form>

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
</body>
</html>
