<%@ Page Language="C#" AutoEventWireup="true" CodeFile="cad_estabelecimentos_edicao_adm.aspx.cs" Inherits="cad_estabelecimentos_edicao_adm" %>

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
                        <div class="col-sm-12 col-xs-12 col-12 mt-1">
                            <div class="card card-outline">
                              <div class="card-header bg-whitelabel1">
                                <h3 class="card-title">
                                  <i class="fas fa-edit"></i>
                                  Detalhamento do Estabelecimento
                                </h3>
                              </div>
                              <div class="card-body">
                                <h4><asp:Label runat="server" id="lblEstabelecimento"></asp:Label></h4>
                                <ul class="nav nav-tabs" id="ccbtEstabelecimento" role="tablist">
                                  <li class="nav-item">
                                    <a class="nav-link active" id="ccbtFicha" data-toggle="pill" href="#ccbFicha" role="tab" aria-controls="ccbFicha" aria-selected="true">Ficha</a>
                                  </li>
                                  <li class="nav-item">
                                    <a class="nav-link" id="ccbtDados" data-toggle="pill" href="#ccbDados" role="tab" aria-controls="ccbDados" aria-selected="false">Dados</a>
                                  </li>
                                  <li class="nav-item" style="display:none;">
                                    <a class="nav-link" id="ccbtResponsavel" data-toggle="pill" href="#ccbResponsavel" role="tab" aria-controls="ccbResponsavel" aria-selected="false">Responsável</a>
                                  </li>
                                  <li class="nav-item" style="display:none;">
                                    <a class="nav-link" id="ccbtDocumentos" data-toggle="pill" href="#ccbDocumentos" role="tab" aria-controls="ccbDocumentos" aria-selected="false">Documentos</a>
                                  </li>
                                  <li class="nav-item">
                                    <a class="nav-link" id="ccbtEndereco" data-toggle="pill" href="#ccbEndereco" role="tab" aria-controls="ccbEndereco" aria-selected="false">Endereço</a>
                                  </li>
                                  <li class="nav-item">
                                    <a class="nav-link" id="ccbtPlataformasPagSeguro" data-toggle="pill" href="#ccbPlataformasPagSeguro" role="tab" aria-controls="ccbPlataformasPagSeguro" aria-selected="false">PagSeguro</a>
                                  </li>
                                  <li class="nav-item">
                                    <a class="nav-link" id="ccbtPlataformasZoop" data-toggle="pill" href="#ccbPlataformasZoop" role="tab" aria-controls="ccbPlataformasZoop" aria-selected="false">Zoop</a>
                                  </li>
                                  <li class="nav-item">
                                    <a class="nav-link" id="ccbtPlataformasBaaS" data-toggle="pill" href="#ccbPlataformasBaaS" role="tab" aria-controls="ccbPlataformasBaaS" aria-selected="false">BaaS</a>
                                  </li>

                                  <li class="nav-item">
                                    <a class="nav-link" id="ccbtUsuarios" data-toggle="pill" href="#ccbUsuarios" role="tab" aria-controls="ccbUsuarios" aria-selected="false">Usuários</a>
                                  </li>

                                  <li class="nav-item">
                                    <a class="nav-link" id="ccbtPlano" data-toggle="pill" href="#ccbPlano" role="tab" aria-controls="ccbPlano" aria-selected="false">Plano/Taxas</a>
                                  </li>


                                </ul>
                                <div class="tab-content" id="ccbtEstabelecimento">
                                  <!-- Visão Geral do Estabelecimento -->
                                  <div class="tab-pane fade show active" id="ccbFicha" role="tabpanel" aria-labelledby="ccbtFicha">
                                    <div class="row">
                                        <div class="col-sm-4 col-xs-12 col-12 mt-1">
                                            <div class="card card-outline">
                                                <div class="card-body">
                                                    <asp:image class="col-10 col-sm-10 col-xs-10" runat="server" id="imgFoto" src="public_html/silhueta.jpg" ></asp:image>
                                                    
                                                </div>
                                            </div>
                                        
                                        </div>
                                        <div class="col-sm-8 col-xs-12 col-12 mt-1">
                                            <div class="row">
                                            <div class="col-sm-12 col-xs-12 col-12">
                                            <div class="card card-outline">
                                                <div class="card-body">
                                                    <div class="row">
                                                        <div class="col-sm-6"  >
                                                            <div class="form-group">
                                                            <label>Nome</label><br />
                                                            <asp:Label runat="server" id="lblNome"></asp:Label>
                                                            </div>
                                                        </div>
                                                        <div class="col-sm-6"  >
                                                            <div class="form-group">
                                                            <label>Sobrenome</label><br />
                                                            <asp:Label runat="server" id="lblSobrenome"></asp:Label>
                                                            </div>
                                                        </div>
                                                    </div>
                                                    <div class="row">
                                                        <div class="col-sm-6"  >
                                                            <div class="form-group">
                                                            <label>CPF</label><br />
                                                            <asp:Label runat="server" id="lblCPF"></asp:Label>
                                                            </div>
                                                        </div>
                                                        <div class="col-sm-6"  >
                                                            <div class="form-group">
                                                            <label>Tipo de pessoa (PF ou PJ)</label><br />
                                                            <asp:Label runat="server" id="lblTipo"></asp:Label>
                                                            </div>
                                                        </div>
                                                    </div>

                                                    <div class="row">
                                                        <div class="col-sm-6"  >
                                                            <div class="form-group">
                                                            <label>Celular</label><br />
                                                            <asp:Label runat="server" id="lblCelular"></asp:Label>
                                                            </div>
                                                        </div>
                                                        <div class="col-sm-6"  >
                                                            <div class="form-group">
                                                            <label>E-Mail</label><br />
                                                            <asp:Label runat="server" id="lblEmail"></asp:Label>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>                                        
                                            </div>
                                            </div>

                                            <div class="row">
                                            <div class="col-sm-12 col-xs-12 col-12">
                                            <div class="card card-outline">
                                                <div class="card-body">
                                                    <div class="row">
                                                        <div class="col-sm-4"  >
                                                            <div class="form-group">
                                                            <label>CEP</label><br />
                                                            <asp:Label runat="server" id="lblCEP"></asp:Label>
                                                            </div>
                                                        </div>
                                                        <div class="col-sm-8"  >
                                                            <div class="form-group">
                                                            <label>Endereço</label><br />
                                                            <asp:Label runat="server" id="lblEndereco"></asp:Label>
                                                            </div>
                                                        </div>
                                                    </div>
                                                    <div class="row">
                                                        <div class="col-sm-6"  >
                                                            <div class="form-group">
                                                            <label>Numero</label><br />
                                                            <asp:Label runat="server" id="lblNumero"></asp:Label>
                                                            </div>
                                                        </div>
                                                        <div class="col-sm-6"  >
                                                            <div class="form-group">
                                                            <label>Complemento</label><br />
                                                            <asp:Label runat="server" id="lblComplemento"></asp:Label>
                                                            </div>
                                                        </div>
                                                    </div>

                                                    <div class="row">
                                                        <div class="col-sm-6"  >
                                                            <div class="form-group">
                                                            <label>Bairro</label><br />
                                                            <asp:Label runat="server" id="lblBairro"></asp:Label>
                                                            </div>
                                                        </div>
                                                        <div class="col-sm-6"  >
                                                            <div class="form-group">
                                                            <label>Cidade</label><br />
                                                            <asp:Label runat="server" id="lblCidade"></asp:Label>
                                                            </div>
                                                        </div>
                                                    </div>
                                                    <div class="row">
                                                        <div class="col-sm-6"  >
                                                            <div class="form-group">
                                                            <label>Estado</label><br />
                                                            <asp:Label runat="server" id="lblEstado"></asp:Label>
                                                            </div>
                                                        </div>
                                                    </div>
                                               
                                                </div>
                                            </div>                                        
                                            </div>
                                            </div>
                                        </div>
                                    
                                    </div>





                                  </div>
                                  <!-- Dados do Estabelecimento Edição  -->
                                  <div class="tab-pane fade" id="ccbDados" role="tabpanel" aria-labelledby="ccbtDados">

                                    <div class="row">
                                        <div class="col-sm-12 col-xs-12 col-12 mt-1">
                                            <div class="card card-outline">
                                                <div class="card-body">

                                                    <div class="row">
                                                      <div class="col-sm-6"  >
                                                          <div class="form-group">
                                                            <label>Nome<strong class="text-danger">*</strong></label>
                                                            <asp:TextBox runat="server" ID="txtNome" CssClass="form-control" placeholder="Nome" ></asp:TextBox> 
                                                          </div>
                                                      </div>
                                          

                                                      <div class="col-sm-6"  >
                                                          <div class="form-group">
                                                            <label>Sobrenome<strong class="text-danger">*</strong></label>
                                                            <asp:TextBox runat="server" ID="txtSobrenome" CssClass="form-control" placeholder="Sobrenome" ></asp:TextBox>
                                                          </div>
                                                      </div>
                                                    </div>


                                                    <div class="row">
                                                      <div class="col-sm-6"  >
                                                          <div class="form-group">
                                                            <label>Data Nascimento<strong class="text-danger">*</strong></label>
                                                            <asp:TextBox runat="server" ID="txtNascimento" CssClass="form-control" placeholder="00/00/0000" ></asp:TextBox> 
                                                          </div>
                                                      </div>
                                          

                                                      <div class="col-sm-6"  >
                                                          <div class="form-group">
                                                            <label>CPF<strong class="text-danger">*</strong></label>
                                                            <asp:TextBox runat="server" ID="txtCPF" CssClass="form-control" placeholder="000.000.000-00"  data-inputmask='"mask": "999.999.999-99"' data-mask></asp:TextBox>
                                                          </div>
                                                      </div>
                                                    </div>

                                                    <div class="row">
                                                      <div class="col-sm-6"  >
                                                          <div class="form-group">
                                                            <label>Nome completo da mãe<strong class="text-danger">*</strong></label>
                                                            <asp:TextBox runat="server" ID="txtNomeMae" CssClass="form-control" placeholder="Digite o nome completo da mãe" ></asp:TextBox> 
                                                          </div>
                                                      </div>
                                          

                                                      <div class="col-sm-6"  >
                                                          <div class="form-group">
                                                            <label>E-Mail do usuário<strong class="text-danger">*</strong></label>
                                                            <asp:TextBox runat="server" ID="txtEmail" CssClass="form-control" placeholder="nome@dominio.com" ></asp:TextBox>
                                                          </div>
                                                      </div>
                                                    </div>

                                                    <div class="row">
                                                      <div class="col-sm-6"  >
                                                          <div class="form-group">
                                                            <label>celular<strong class="text-danger">*</strong></label>
                                                            <asp:TextBox runat="server" ID="txtCelular" CssClass="form-control" placeholder="(11) 99999-9999" data-inputmask='"mask": "(99) 99999-9999"' data-mask ></asp:TextBox> 
                                                          </div>
                                                      </div>
                                          

                                                    </div>


                                                </div>
                                            </div>
                                        </div>
                                    </div>



                                  </div>
                                  <!-- Dados do Responsável Edição  -->
                                  <div class="tab-pane fade" id="ccbResponsavel" role="tabpanel" aria-labelledby="ccbtResponsavel">
                                     Responsável
                                  </div>
                                  <!-- Dados dos Documentos Edição  -->
                                  <div class="tab-pane fade" id="ccbDocumentos" role="tabpanel" aria-labelledby="ccbtDocumentos">
                                     Documentos 
                                  </div>
                                  <!-- Dados do Endereço Edição  -->
                                  <div class="tab-pane fade" id="ccbEndereco" role="tabpanel" aria-labelledby="ccbtEndereco">


                                    <div class="row">
                                        <div class="col-sm-12 col-xs-12 col-12 mt-1">
                                            <div class="card card-outline">
                                                <div class="card-body">
                                                    <div class="row">
                                                        <div class="col-sm-6"  >
                                                            <div class="form-group">
                                                            <label>CEP<strong class="text-danger">*</strong></label>
                                                            <asp:TextBox runat="server" ID="txtCEP" CssClass="form-control" 
                                                                    placeholder="00000000" data-inputmask='"mask": "99999-999"' data-mask  
                                                                    AutoPostBack="True" ontextchanged="txtCEP_TextChanged"></asp:TextBox> 
                                                            </div>
                                                        </div>
                                                    </div>


                                                    <div class="row">
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

                                                    
                                                </div>
                                            </div>
                                        
                                        </div>
                                    
                                    </div>




                                  </div>
                                  <!-- Dados das Plataformas  -->
                                  <div class="tab-pane fade" id="ccbPlataformasPagSeguro" role="tabpanel" aria-labelledby="ccbtPlataformasPagSeguro">



                                    <div class="row">
                                        <div class="col-sm-12 col-xs-12 col-12 mt-1">
                                            <div class="card card-outline">
                                                <div class="card-body">

                                                    <div class="row">
                                                      <div class="col-sm-4"  >
                                                          <div class="form-group">
                                                            <label>Pagseguro<strong class="text-danger">*</strong> (Habilitar e gerenciar adquirente pagseguro)</label>
                                                            <asp:DropDownList runat="server" id="ddlPagseguro" CssClass="form-control">
                                                                <asp:ListItem Value="0">Não</asp:ListItem>
                                                                <asp:ListItem Value="1">Sim</asp:ListItem>
                                                            </asp:DropDownList>
                                                          </div>
                                                      </div>

                                                      <div class="col-sm-4"  >
                                                          <div class="form-group">
                                                            <label>Código Ativação<strong class="text-danger">*</strong></label>
                                                            <asp:TextBox runat="server" ID="txtCodigoAtivacao" CssClass="form-control" placeholder="Código de ativação" ></asp:TextBox> 
                                                          </div>
                                                      </div>
                                          

                                                      <div class="col-sm-4"  >
                                                          <div class="form-group">
                                                            <label>Conta verificada e ativada<strong class="text-danger">*</strong></label>
                                                            <asp:DropDownList runat="server" id="ddlVerificadaAtivada" CssClass="form-control">
                                                                <asp:ListItem Value="Sim">Sim</asp:ListItem>
                                                                <asp:ListItem Value="Não">Não</asp:ListItem>
                                                                <asp:ListItem Value="Pendente de Documentos">Pendente de Documentos</asp:ListItem>
                                                                <asp:ListItem Value="Dados Inválidos">Dados Inválidos</asp:ListItem>
                                                                <asp:ListItem Value="Negada">Negada</asp:ListItem>
                                                            </asp:DropDownList>
                                                          </div>
                                                      </div>


                                                    </div>


                                                    <div class="row">
                                                      <div class="col-sm-4"  >
                                                          <div class="form-group">
                                                            <label>ID<strong class="text-danger">*</strong></label>
                                                            <asp:TextBox runat="server" ID="txtIDPagseguro" CssClass="form-control" placeholder="ID Pagseguro" ></asp:TextBox> 
                                                          </div>
                                                      </div>
                                          

                                                      <div class="col-sm-4"  >
                                                          <div class="form-group">
                                                            <label>E-mail<strong class="text-danger">*</strong></label>
                                                            <asp:TextBox runat="server" ID="txtEmailPagseguro" CssClass="form-control" placeholder="E-mail ativação pagseguro" ></asp:TextBox>
                                                          </div>
                                                      </div>

                                                      <div class="col-sm-4"  >
                                                          <div class="form-group">
                                                            <label>Token<strong class="text-danger">*</strong></label>
                                                            <asp:TextBox runat="server" ID="txtTokenPagseguro" CssClass="form-control" placeholder="Token Pagseguro" ></asp:TextBox>
                                                          </div>
                                                      </div>

                                                    </div>


                                                </div>
                                            </div>
                                        </div>
                                    </div>

                                    <div class="row">
                                        <div class="col-sm-12 col-xs-12 col-12 mt-1">
                                            <div class="card card-outline">
                                                <div class="card-body">

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
                                            </div>
                                        </div>
                                    </div>
                                     
                                  </div>
                                  <div class="tab-pane fade" id="ccbPlataformasZoop" role="tabpanel" aria-labelledby="ccbtPlataformasZoop">
                                  </div>
                                  <div class="tab-pane fade" id="ccbPlataformasBaaS" role="tabpanel" aria-labelledby="ccbtPlataformasBaaS">
                                  </div>
                                  <div class="tab-pane fade" id="ccbUsuarios" role="tabpanel" aria-labelledby="ccbtUsuarios">
                                    
                                    <div class="row">
                                        <div class="col-12">
                                            <table id="tbUsuario" class="table table-bordered table-hover">
                                                <thead>
                                                <tr>
                                                  <th>ID</th>
                                                  <th>E-Mail</th>
                                                  <th>Senha</th>
                                                </tr>
                                                </thead>
                                                <tbody>
                                                <asp:Repeater runat="server" ID="rptConsultaUsuario">
                                                    <ItemTemplate>
                                                        <tr>
                                                          <td>
                                                              <small><strong><%# DataBinder.Eval(Container.DataItem, "COD_ID")%></strong></small>
                                                          </td>
                                                          <td>
                                                              <small><strong><%# DataBinder.Eval(Container.DataItem, "NOM_LOGIN")%></strong></small>
                                                          </td>
                                                          <td>
                                                              <small><strong><%# Funcoes.Encrypt(DataBinder.Eval(Container.DataItem, "NOM_SENHA").ToString())%></strong></small>
                                                          </td>
                                                        </tr>
                                                    </ItemTemplate>
                                                </asp:Repeater>

                                                </tbody>
                                            </table>
                                          </div>
                                      </div>


                                  </div>
                                  <div class="tab-pane fade" id="ccbPlano" role="tabpanel" aria-labelledby="ccbtPlano">

                                    <div class="row">
                                        <div class="col-sm-12 col-xs-12 col-12 mt-1">
                                            <div class="card card-outline">
                                                <div class="card-body">

                                                    <div class="row">
                                                      <div class="col-sm-12"  >
                                                          <div class="form-group">
                                                            <label>Plano<strong class="text-danger">*</strong></label>
                                                            <asp:DropDownList runat="server" id="ddlPlano" CssClass="form-control">
                                                            </asp:DropDownList>
                                                          </div>
                                                      </div>
                                                    </div>

                                                </div>
                                            </div>
                                        </div>
                                    </div>



                                  </div>

                                </div>
                            </div>
                            <div class="card-footer">
                            
                                <asp:Button runat="server" id="btnVoltar" Text="Voltar" 
                                    class="btn btn-outline btn-warning float-left" enabled="true" 
                                    onclick="btnVoltar_Click"/>
                                <asp:Button runat="server" id="btnSalvar" Text="Salvar" 
                                    class="btn btn-outline btn-whitelabel1 float-right"  enabled="true" 
                                    onclick="btnSalvar_Click"/>
                            
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

<script type="text/javascript" src="../plugins/inputmask/jquery.inputmask.bundle.js"></script>
<script type="text/javascript" src="../plugins/moment/moment.min.js"></script>

<script type="text/javascript" src="../plugins/datatables/jquery.dataTables.js"></script>
<script type="text/javascript" src="../plugins/datatables-bs4/js/dataTables.bootstrap4.js"></script>
<script type="text/javascript">

$(document).ready(function () {
    var table = $('#tbUsuario').DataTable({
      "paging": true,
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
