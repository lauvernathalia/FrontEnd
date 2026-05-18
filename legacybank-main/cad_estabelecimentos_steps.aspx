<%@ Page Language="C#" AutoEventWireup="true" CodeFile="cad_estabelecimentos_steps.aspx.cs" Inherits="cad_estabelecimentos_steps" Async="true" EnableSessionState="ReadOnly" MaintainScrollPositionOnPostback="true"  %>

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

<style>
html, body{
   height: 100%;
}

/* Make circles that indicate the steps of the form: */
.step {
  height: 30px;
  width: 30px;
  margin: 0 2px;
  background-color: #bbbbbb;
  border: none;  
  border-radius: 50%;
  display: inline-block;
  opacity: 1;
}

.step.active {
  opacity: 1;
  background-color: #626262;
  color: #ffffff;
  
}

/* Mark the steps that are finished and valid: */
.step.finish {
  background-color: <%=CorPrimaria(1) %>;
  color: #ffffff;
}

</style>

</head>

<body class="hold-transition sidebar-mini layout-fixed">
    <form id="frmPrincipal" runat="server">

        <div class="row">
            <div class="col-12">

                        <!-- Dados do Tipo *********************************************************************************************************************************** -->
                        
                        <div runat="server" id="dvTipo" visible="true">
                            <div class="row">
                            <div class="col-12">
                              <div class="card" style="min-height:100vh; height:100vh; max-height:100vh;">
                                <div class="card-header bg-whitelabel1">
                                    <h3 class="card-title">CADASTRO DE ESTABELECIMENTO</h3>
                                </div>
                                <div class="card-body text-center">
                                    <div class="row">
                                        <div class="col-12">
                                            <h3>Para prosseguir é preciso selecionar o tipo de conta que deseja cadastrar!</h3>
                                        </div>
                                    </div>

                                    <div class="row">
                                        <div class="col-6">
                                            <center>
                                            <i class="fas fa-user fa-2x text-whitelabel1"></i><br /><br />
                                            <asp:Button runat="server" ID="btnPessoal" CssClass="btn btn-whitelabel1" Text="Física" onclick="btnPessoal_Click"/>
                                            <br /><br />
                                            </center>
                                        </div>
                                        <div class="col-6">
                                            <center>
                                            <i class="fas fa-store-alt fa-2x text-whitelabel1"></i><br /><br />
                                            <asp:Button runat="server" ID="btnNegocio" CssClass="btn btn-whitelabel1" Text="Jurídica" onclick="btnNegocio_Click"/>
                                            <br /><br />
                                            </center>
                                        </div>
                                    </div>
                                
                                </div>
                                <div class="card-footer">
                                    <asp:Button runat="server" ID="Voltar01" CssClass="btn btn-whitelabel1" Text="Voltar" onclick="btnVoltar01_Click" Visible="false"/>
                                    <asp:Button runat="server" ID="Avancar01" CssClass="btn btn-whitelabel1 float-right" Text="Avançar" onclick="btnAvancar01_Click" Visible="false"/>
                                </div>
                              </div>
                            </div>
                            </div>
                            <div class="row" runat="server" id="dvTipoCadastro" visible="false">
                                <div class="col-sm-12">
                                    <div class="form-group">
                                        <label>Tipo do Cadastro (PF ou PJ)<strong class="text-danger">*</strong></label>
                                        <asp:DropDownList runat="server" id="ddlTipoFJ" CssClass="form-control" 
                                            onselectedindexchanged="ddlTipoFJ_SelectedIndexChanged" 
                                            AutoPostBack="True">
                                            <asp:ListItem Value="  ">Selecione o tipo de cadastro</asp:ListItem>
                                            <asp:ListItem Value="PF">Pessoa Física</asp:ListItem>
                                            <asp:ListItem Value="PJ">Pessoa Jurídica</asp:ListItem>
                                        </asp:DropDownList>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <!-- Dados da PESSOA JURÍDICA *********************************************************************************************************************************** -->
                        <div runat="server" id="dvCNPJ" visible="false">


                            <div class="row">
                            <div class="col-12">
                              <div class="card" style="min-height:100vh; height:100vh; max-height:100vh;">
                                <div class="card-header bg-whitelabel1">
                                    <h3 class="card-title">CADASTRO DE ESTABELECIMENTO</h3>
                                </div>
                                <div class="card-body">
                                    <div class="row text-center" id="dvStepPJ01" runat="server">
                                        <div class="col-12">
                                          <div style="text-align:center;margin-top:40px;">
                                            <h3 style="text-align:center;">
                                                <span class="step active">1</span>
                                                <span class="step">2</span>
                                                <span class="step">3</span>
                                                <span class="step">4</span>
                                                <span class="step">5</span>
                                            </h3>
                                          </div>
                                        </div>
                                    </div>

                                    <div class="row">
                                        <div class="col-sm-12">
                                            <h3>Informe o CNPJ da Pessoa Jurídica</h3>
                                        </div>
                                    </div>

                                    <div class="row">
                                        <div class="col-sm-12">
                                            <div class="form-group">
                                                <b><asp:Label runat="server" id="lblCNPJ" Text="CNPJ"></asp:Label></b><strong class="text-danger">*</strong> 
                                                <div class="input-group input-group-sm">
                                                  <asp:TextBox runat="server" id="txtCNPJ" cssClass="form-control" placeholder="00.000.000/0000-00" data-inputmask='"mask": "99.999.999/9999-99"' data-mask></asp:TextBox>
                                                  <span class="input-group-append">
                                                    <asp:LinkButton runat="server" ID="btnPesquisarPJ" CssClass="btn btn-sm btn-secondary" onclick="btnPesquisarPJ_Click"><i class="fas fa-search"></i></asp:LinkButton>
                                                  </span>
                                                </div>
                                            </div>
                                        </div>
                                    </div>

                                    <div class="row" runat="server" id="dvConsultaEmpresa" visible="false">
                                        <div class="col-sm-12">
                                            <asp:TextBox runat="server" ID="txtRespostaPJ" TextMode="MultiLine" Rows="5" CssClass="form-control" Visible="false"></asp:TextBox>
                                        </div>
                                        <div class="col-sm-12">
                                        <table id="tblConsultaEmpresa" class="table table-bordered table-hover">
                                            <thead>
                                            <tr>
                                                <th>Razão Social</th>
                                                <th>Nome Fantasia</th>
                                                <th>CNPJ</th>
                                                <th>Utilizar</th>
                                            </tr>
                                            </thead>
                                            <tbody>
                                                <asp:ListView ID="lsvConsultaEmpresa" runat="server" 
                                                    OnItemCommand="lsvConsultaEmpresa_ItemCommand">
                                                    <ItemTemplate>
                                                            <tr>
                                                                <td>
                                                                    <small><asp:Label ID="Label1" runat="server" Text='<%# Eval("razaosocial") %>'></asp:Label></small>
                                                                </td>
                                                                <td>
                                                                    <small><asp:Label ID="Label6" runat="server" Text='<%# Eval("nomefantasia") %>'></asp:Label></small>
                                                                </td>
                                                                <td>
                                                                    <small><asp:Label ID="Label16" runat="server" Text='<%# Eval("cnpj") %>'></asp:Label></small>
                                                                </td>
                                                                <td>
                                                                    <asp:LinkButton runat="server" ID="LinkButton1" CssClass="btn btn-sm btn-secondary" commandname="Utilizar" text="Utilizar" ToolTip="Utilizar"  CommandArgument='<%# Eval("cnpj") %>' ><i class="fas fa-address-card"></i></asp:LinkButton>
                                                                </td>

                                                            </tr>
                                                    </ItemTemplate>
                                                </asp:ListView>
                                            </tbody>
                                        </table>                                            
                                        </div>
                                    </div>
                                    <!-- Dados da PESSOA JURÍDICA *********************************************************************************************************************************** -->
                                    <div runat="server" id="dvJuridica" visible="false">
                                        <h3><span class="float-center badge bg-whitelabel1">Dados da Pessoa Jurídica</span></h3>

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
                                            <div class="col-sm-4">
                                                <div class="form-group">
                                                <label>Telefone Estabelecimento<strong class="text-danger">*</strong></label>
                                                <asp:TextBox runat="server" id="txtTelefoneEmpresa" CssClass="form-control" placeholder="(99) 99999-9999" data-inputmask='"mask": "(99) 99999-9999"' data-mask>
                                                </asp:TextBox>
                                                </div>
                                            </div>
                                            <div class="col-sm-4">
                                                <div class="form-group">
                                                <label>E-mail Estabelecimento<strong class="text-danger">*</strong></label>
                                                <asp:TextBox runat="server" id="txtEmailEmpresa" CssClass="form-control" placeholder="nome@dominio.com.br">
                                                </asp:TextBox>
                                                </div>
                                            </div>
                                            <div class="col-sm-4">
                                                <div class="form-group">
                                                    <b><asp:Label runat="server" id="lblDataAbertura" Text="Data Início Atividade"></asp:Label></b><strong class="text-danger">*</strong> 
                                                    <asp:TextBox runat="server" id="txtDataAbertura" CssClass="form-control" placeholder="00/00/0000" data-inputmask='"mask": "99/99/9999"' data-mask required>
                                                    </asp:TextBox>
                                                </div>
                                            </div>
                                        </div>

                                        <!-- Falta Atividade econômica e mcc visitou este estabelecimento presencialmente e patrimonio e tipo da empresa -->

                                        <div class="row">


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


                                    </div>




                                </div>
                                <div class="card-footer">
                                    <asp:Button runat="server" ID="btnVoltar02PJ" CssClass="btn btn-whitelabel1" Text="Voltar" onclick="btnVoltar02PJ_Click" Visible="true"/>
                                    <asp:Button runat="server" ID="btnAvancar02PJ" CssClass="btn btn-whitelabel1 float-right" Text="Avançar" onclick="btnAvancar02PJ_Click" Visible="true"/>
                                </div>

                              </div>
                            </div>
                            </div>
                        </div>
                        <!-- Dados da PESSOA FISICA *********************************************************************************************************************************** -->
                        <div runat="server" id="dvCPF" visible="false">
                            <div class="row">
                            <div class="col-12">
                              <div class="card" style="min-height:100vh; height:100vh; max-height:100vh;">
                                <div class="card-header bg-whitelabel1">
                                    <h3 class="card-title">CADASTRO DE ESTABELECIMENTO</h3>
                                </div>
                                <div class="card-body">
                                    <div class="row" id="dvStepPF01" runat="server">
                                        <div class="col-12">
                                          <div style="text-align:center;margin-top:40px;">
                                            <h3 style="text-align:center;">
                                                <span class="step active">1</span>
                                                <span class="step">2</span>
                                                <span class="step">3</span>
                                                <span class="step">4</span>
                                            </h3>
                                          </div>
                                        </div>
                                    </div>


                                    <div class="row">
                                        <div class="col-sm-12">
                                            <h3>Informe o CPF da Pessoa Física</h3>
                                        </div>
                                    </div>

                                    <div class="row">
                                        <div class="col-sm-12">
                                            <div class="form-group">
                                                <label class="col-sm-12 col-form-label">CPF</label>
                                                <div class="input-group input-group-sm">
                                                  <asp:TextBox id="txtDocumentoPF"  runat="server" class="form-control" placeholder="000.000.000-00" data-inputmask='"mask": "999.999.999-99"' data-mask ></asp:TextBox>
                                                  <span class="input-group-append">
                                                    <asp:LinkButton runat="server" ID="btnPesquisarPF" CssClass="btn btn-sm btn-secondary" onclick="btnPesquisarPF_Click"><i class="fas fa-search"></i></asp:LinkButton>
                                                  </span>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="row" runat="server" id="dvConsultaPessoa" visible="false">
                                        
                                        <div class="col-sm-12">
                                            <asp:TextBox runat="server" ID="txtRespostaPF" TextMode="MultiLine" Rows="5"  CssClass="form-control" Visible="false"></asp:TextBox>
                                        </div>

                                        <div class="col-sm-12">
                                        <table id="tblConsultaPessoa" class="table table-bordered table-hover">
                                            <thead>
                                            <tr>
                                                <th>Nome</th>
                                                <th>E-mail</th>
                                                <th>CPF</th>
                                                <th>Utilizar</th>
                                            </tr>
                                            </thead>
                                            <tbody>
                                                <asp:ListView ID="lsvConsultaPessoa" runat="server" 
                                                    OnItemCommand="lsvConsultaPessoa_ItemCommand">
                                                    <ItemTemplate>
                                                            <tr>
                                                                <td>
                                                                    <small><asp:Label ID="Label1" runat="server" Text='<%# Eval("nome") %>'></asp:Label></small>
                                                                </td>
                                                                <td>
                                                                    <small><asp:Label ID="Label6" runat="server" Text='<%# Eval("email") %>'></asp:Label></small>
                                                                </td>
                                                                <td>
                                                                    <small><asp:Label ID="Label16" runat="server" Text='<%# Eval("cpf") %>'></asp:Label></small>
                                                                </td>
                                                                <td>
                                                                    <asp:LinkButton runat="server" ID="btnUtilizar" CssClass="btn btn-sm btn-secondary" commandname="Utilizar" text="Utilizar" ToolTip="Utilizar"  CommandArgument='<%# Eval("cpf") %>' ><i class="fas fa-address-card"></i></asp:LinkButton>
                                                                </td>

                                                            </tr>
                                                    </ItemTemplate>
                                                </asp:ListView>
                                            </tbody>
                                        </table>                                            
                                        </div>
                                    </div>

                                <!-- Dados da PESSOA FÍSICA *********************************************************************************************************************************** -->
                                <div runat="server" id="dvFisica" visible="false">
                                    <h3><span class="float-center badge bg-whitelabel1">Dados da Pessoa Física</span></h3>

                                    <div class="row">
                                        <div class="col-sm-12">
                                            <div class="form-group">
                                                <label class="col-sm-12 col-form-label">Nome</label>
                                                <asp:TextBox id="txtNomePF" runat="server" class="form-control" placeholder="Nome do Responsável"></asp:TextBox>
                                            </div>
                                        </div>
                                        <div class="col-sm-6" runat="server" id="dvSobrenomePF" visible="false">
                                            <div class="form-group">
                                                <label class="col-sm-12 col-form-label">Sobrenome</label>
                                                <asp:TextBox id="txtSobrenomePF"  runat="server" class="form-control" placeholder="Sobrenome do Responsável"></asp:TextBox>
                                            </div>
                                        </div>
                                    </div>

                                    <!-- Falta dados Nome da Mãe e renda bruta e data de nascimento e pessoa politicamente exposta -->

                                    <div class="row">
                                        <div class="col-sm-4"  >
                                            <div class="form-group">
                                            <label>Data Nascimento<strong class="text-danger">*</strong></label>
                                            <asp:TextBox runat="server" ID="txtNascimentoPF" CssClass="form-control" placeholder="00/00/0000" data-inputmask='"mask": "99/99/9999"' data-mask required></asp:TextBox> 
                                            </div>
                                        </div>
                                        <div class="col-sm-4"  >
                                            <div class="form-group">
                                            <label>Nome completo da mãe<strong class="text-danger">*</strong></label>
                                            <asp:TextBox runat="server" ID="txtNomeMaePF" CssClass="form-control" placeholder="Digite o nome completo da mãe" required></asp:TextBox> 
                                            </div>
                                        </div>
                                        <div class="col-sm-4">
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
                                                <label class="col-sm-12 col-form-label">E-mail</label>
                                                <asp:TextBox id="txtEmailPF" runat="server" class="form-control" placeholder="E-mail do Responsável"></asp:TextBox>
                                            </div>
                                        </div>
                                        <div class="col-sm-4">
                                            <div class="form-group">
                                                <label class="col-sm-12 col-form-label">Celular</label>
                                                <asp:TextBox id="txtCelularPF" runat="server" class="form-control" placeholder="Celular do Responsável"></asp:TextBox>
                                            </div>
                                        </div>
                                        <div class="col-sm-4">
                                            <div class="form-group">
                                            <label>É pessoa politicamente exposta?<strong class="text-danger">*</strong></label>
                                            <asp:DropDownList runat="server" id="ddlPoliticamenteExpostaPF" CssClass="form-control">
                                                <asp:ListItem Value="N">Não</asp:ListItem>
                                                <asp:ListItem Value="S">Sim</asp:ListItem>
                                            </asp:DropDownList>
                                            </div>
                                        </div>


                                    </div>

                                </div>


                                </div>
                                <div class="card-footer">
                                    <asp:Button runat="server" ID="btnVoltar02PF" CssClass="btn btn-whitelabel1" Text="Voltar" onclick="btnVoltar02PF_Click" Visible="true"/>
                                    <asp:Button runat="server" ID="btnAvancar02PF" CssClass="btn btn-whitelabel1 float-right" Text="Avançar" onclick="btnAvancar02PF_Click" Visible="true"/>
                                </div>

                              </div>
                            </div>
                            </div> 

                        </div>





                        <!-- Dados do ENDEREÇO *********************************************************************************************************************************** -->
                        <div runat="server" id="dvEndereco" visible="false">
                            <div class="row">
                            <div class="col-12">
                              <div class="card" style="min-height:100vh; height:100vh; max-height:100vh;">
                                <div class="card-header bg-whitelabel1">
                                    <h3 class="card-title">CADASTRO DE ESTABELECIMENTO</h3>
                                </div>
                                <div class="card-body">
                                    <div class="row text-center" id="dvStepPJ02" runat="server">
                                        <div class="col-12">
                                          <div style="text-align:center;margin-top:40px;">
                                            <h3 style="text-align:center;">
                                                <span class="step finish">1</span>
                                                <span class="step active">2</span>
                                                <span class="step">3</span>
                                                <span class="step">4</span>
                                                <span class="step">5</span>
                                            </h3>
                                          </div>
                                        </div>
                                    </div>

                                    <div class="row" id="dvStepPF02" runat="server">
                                        <div class="col-12">
                                          <div style="text-align:center;margin-top:40px;">
                                            <h3 style="text-align:center;">
                                                <span class="step finish">1</span>
                                                <span class="step active">2</span>
                                                <span class="step">3</span>
                                                <span class="step">4</span>
                                            </h3>
                                          </div>
                                        </div>
                                    </div>


                                    <div class="row">
                                        <div class="col-sm-12">
                                            <h3>Informe os dados de endereçamento</h3>
                                        </div>
                                    </div>


                                    <h3><span class="float-center badge bg-whitelabel1">Dados Endereço</span></h3>

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
                                </div>
                                <div class="card-footer">
                                    <asp:Button runat="server" ID="btnVoltar03" CssClass="btn btn-whitelabel1" Text="Voltar" onclick="btnVoltar03_Click" Visible="true"/>
                                    <asp:Button runat="server" ID="btnAvancar03" CssClass="btn btn-whitelabel1 float-right" Text="Avançar" onclick="btnAvancar03_Click" Visible="true"/>
                                </div>

                            </div>
                        </div>
                        </div>
                       
                        </div>


                        <!-- Dados do RESPONSÁVEL PESSOA JURIDICA *********************************************************************************************************************************** -->
                        <div runat="server" id="dvResponsavel" visible="false">
                            <div class="row">
                            <div class="col-12">
                              <div class="card" style="min-height:100vh; height:100vh; max-height:100vh;">
                                <div class="card-header bg-whitelabel1">
                                    <h3 class="card-title">CADASTRO DE ESTABELECIMENTO</h3>
                                </div>
                                <div class="card-body">
                                    <div class="row text-center" id="dvStepPJ03" runat="server">
                                        <div class="col-12">
                                          <div style="text-align:center;margin-top:40px;">
                                            <h3 style="text-align:center;">
                                                <span class="step finish">1</span>
                                                <span class="step finish">2</span>
                                                <span class="step active">3</span>
                                                <span class="step">4</span>
                                                <span class="step">5</span>
                                            </h3>
                                          </div>
                                        </div>
                                    </div>


                                    <div class="row">
                                        <div class="col-sm-12">
                                            <h3>Informe os dados do responsável pelo estabelecimento</h3>
                                        </div>
                                    </div>

                                    <h3><span class="float-center badge bg-whitelabel1">Dados do Responsável</span></h3>

                                    <div class="row">
                                        <div class="col-sm-8">
                                            <div class="form-group">
                                                <label class="col-sm-12 col-form-label">Nome Responsável</label>
                                                <asp:TextBox id="txtNome" runat="server" class="form-control" placeholder="Nome do Responsável"></asp:TextBox>
                                            </div>
                                        </div>
                                        <div class="col-sm-4" runat="server" id="dvSobrenome" visible="false">
                                            <div class="form-group">
                                                <label class="col-sm-12 col-form-label">Sobrenome Responsável</label>
                                                <asp:TextBox id="txtSobrenome"  runat="server" class="form-control" placeholder="Sobrenome do Responsável"></asp:TextBox>
                                            </div>
                                        </div>
                                        <div class="col-sm-4">
                                            <div class="form-group">
                                                <label class="col-sm-12 col-form-label">CPF Responsável</label>
                                                <asp:TextBox id="txtDocumento"  runat="server" class="form-control" placeholder="000.000.000-00" data-inputmask='"mask": "999.999.999-99"' data-mask ></asp:TextBox>
                                            </div>
                                        </div>
                                    </div>

                                    <!-- Falta dados Nome da Mãe e renda bruta e data de nascimento e pessoa politicamente exposta -->

                                    <div class="row">
                                        <div class="col-sm-3"  >
                                            <div class="form-group">
                                            <label>Data Nascimento<strong class="text-danger">*</strong></label>
                                            <asp:TextBox runat="server" ID="txtNascimento" CssClass="form-control" placeholder="00/00/0000" data-inputmask='"mask": "99/99/9999"' data-mask required></asp:TextBox> 
                                            </div>
                                        </div>
                                        <div class="col-sm-3"  >
                                            <div class="form-group">
                                            <label>Nome completo da mãe<strong class="text-danger">*</strong></label>
                                            <asp:TextBox runat="server" ID="txtNomeMae" CssClass="form-control" placeholder="Digite o nome completo da mãe" required></asp:TextBox> 
                                            </div>
                                        </div>
                                        <div class="col-sm-3">
                                            <div class="form-group">
                                            <label>Renda Mensal<strong class="text-danger">*</strong></label>
                                            <asp:TextBox runat="server" id="txtRendaMensal" CssClass="form-control" placeholder="0,00">
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
                                </div>
                                <div class="card-footer">
                                    <asp:Button runat="server" ID="btnVoltar04" CssClass="btn btn-whitelabel1" Text="Voltar" onclick="btnVoltar04_Click" Visible="true"/>
                                    <asp:Button runat="server" ID="btnAvancar04" CssClass="btn btn-whitelabel1 float-right" Text="Avançar" onclick="btnAvancar04_Click" Visible="true"/>
                                </div>

                            </div>
                        </div>
                        </div>

                        </div>



                        <!-- Dados GERAIS *********************************************************************************************************************************** -->
                        <div runat="server" id="dvGeral" visible="false">
                            <div class="row">
                            <div class="col-12">
                              <div class="card" style="min-height:100vh; height:100vh; max-height:100vh;">
                                <div class="card-header bg-whitelabel1">
                                    <h3 class="card-title">CADASTRO DE ESTABELECIMENTO</h3>
                                </div>
                                <div class="card-body">
                                    <div class="row text-center" id="dvStepPJ04" runat="server">
                                        <div class="col-12">
                                          <div style="text-align:center;margin-top:40px;">
                                            <h3 style="text-align:center;">
                                                <span class="step finish">1</span>
                                                <span class="step finish">2</span>
                                                <span class="step finish">3</span>
                                                <span class="step active">4</span>
                                                <span class="step">5</span>
                                            </h3>
                                          </div>
                                        </div>
                                    </div>
                                    <div class="row" id="dvStepPF03" runat="server">
                                        <div class="col-12">
                                          <div style="text-align:center;margin-top:40px;">
                                            <h3 style="text-align:center;">
                                                <span class="step finish">1</span>
                                                <span class="step finish">2</span>
                                                <span class="step active">3</span>
                                                <span class="step">4</span>
                                            </h3>
                                          </div>
                                        </div>
                                    </div>

                                    <div class="row">
                                        <div class="col-sm-12">
                                            <h3>Informe os dados gerais</h3>
                                        </div>
                                    </div>


                                    <h3><span class="float-center badge bg-whitelabel1">Dados Gerais</span></h3>

                                    <div class="row">
                                        <div class="col-sm-4">
                                            <div class="form-group">
                                            <label>Você visitou este pessoa presencialmente?<strong class="text-danger">*</strong></label>
                                            <asp:DropDownList runat="server" id="ddlPresencial" CssClass="form-control">
                                                <asp:ListItem Value="S">Sim</asp:ListItem>
                                                <asp:ListItem Value="N">Não</asp:ListItem>
                                            </asp:DropDownList>
                                            </div>
                                        </div>

                                        <div class="col-sm-4">
                                            <div class="form-group">
                                                <label>Tipo do Estabelecimento<strong class="text-danger">*</strong></label>
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

                                        <div class="col-sm-4">
                                            <div class="form-group">
                                                <label>Atividade Econômica<strong class="text-danger">*</strong></label>
                                                <asp:DropDownList runat="server" id="ddlAtividadeEconomica" CssClass="form-control">
                                                </asp:DropDownList>
                                            </div>
                                        </div>


                                    </div>


                                    <div class="row">
                                        <div class="col-sm-6">
                                            <div class="form-group">
                                            <label>Marketplace Responsável<strong class="text-danger">*</strong></label>
                                            <asp:DropDownList runat="server" id="ddlMarketplace" CssClass="form-control">
                                            </asp:DropDownList>
                                            </div>
                                        </div>
                                        <div class="col-sm-6">
                                            <div class="form-group">
                                            <label>Representante Responsável<strong class="text-danger">*</strong></label>
                                            <asp:DropDownList runat="server" id="ddlRepresentante" CssClass="form-control">
                                            </asp:DropDownList>
                                            </div>
                                        </div>


                                    </div>

                                </div>
                                <div class="card-footer">
                                    <asp:Button runat="server" ID="btnVoltar05" CssClass="btn btn-whitelabel1" Text="Voltar" onclick="btnVoltar05_Click" Visible="true"/>
                                    <asp:Button runat="server" ID="btnAvancar05" CssClass="btn btn-whitelabel1 float-right" Text="Avançar" onclick="btnAvancar05_Click" Visible="true"/>
                                </div>

                            </div>
                        </div>
                        </div>


                        </div>


                        <!-- Dados USUÁRIO *********************************************************************************************************************************** -->

                        <div runat="server" id="dvUsuario" visible="false">

                            <div class="row">
                            <div class="col-12">
                              <div class="card" style="min-height:100vh; height:100vh; max-height:100vh;">
                                <div class="card-header bg-whitelabel1">
                                    <h3 class="card-title">CADASTRO DE ESTABELECIMENTO</h3>
                                </div>
                                <div class="card-body">
                                    <div class="row text-center" id="dvStepPJ05" runat="server">
                                        <div class="col-12">
                                          <div style="text-align:center;margin-top:40px;">
                                            <h3 style="text-align:center;">
                                                <span class="step finish">1</span>
                                                <span class="step finish">2</span>
                                                <span class="step finish">3</span>
                                                <span class="step finish">4</span>
                                                <span class="step active">5</span>
                                            </h3>
                                          </div>
                                        </div>
                                    </div>

                                    <div class="row" id="dvStepPF04" runat="server">
                                        <div class="col-12">
                                          <div style="text-align:center;margin-top:40px;">
                                            <h3 style="text-align:center;">
                                                <span class="step finish">1</span>
                                                <span class="step finish">2</span>
                                                <span class="step finish">3</span>
                                                <span class="step active">4</span>
                                            </h3>
                                          </div>
                                        </div>
                                    </div>

                                    <div class="row">
                                        <div class="col-sm-12">
                                            <h3>Informe os dados do responsável pelo estabelecimento</h3>
                                        </div>
                                    </div>

                                    <h3><span class="float-center badge bg-whitelabel1">Dados do Usuário</span></h3>

                                    <div class="row">
                                        <div class="col-sm-6">
                                            <div class="form-group">
                                                <label class="col-sm-12 col-form-label">Nome do Usuário Principal</label>
                                                <asp:TextBox id="txtNomeUsuario" runat="server" CssClass="form-control" placeholder="Nome do Usuário" Enabled="false"></asp:TextBox>
                                            </div>
                                        </div>
                                        <div class="col-sm-6">
                                            <div class="form-group">
                                                <label class="col-sm-12 col-form-label">E-mail do Usuário</label>
                                                <asp:TextBox id="txtEmail" runat="server" CssClass="form-control" placeholder="E-mail do Usuário" Enabled="false"></asp:TextBox>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="row">
                                        <div class="col-sm-12">
                                            <div class="form-group">
                                                <div class="form-check">
                                                  <asp:CheckBox CssClass="form-check-input" runat="server" id="ckbEnviarEmail" />
                                                  <label class="form-check-label">Clique aqui caso deseje enviar por e-mail o link de criação de senha para acesso ao sistema</label>
                                                </div>
                                            </div>
                                        </div>   
                                    </div>


                                    <div class="row" runat="server" visible="false" id="dvConfirmaEmail">
                                        <div class="col-sm-6">
                                            <div class="form-group">
                                                <label class="col-sm-12 col-form-label">Confirme o E-mail </label>
                                                <asp:TextBox id="txtEmailC" runat="server" class="form-control" placeholder="Confirme o E-mail do Usuário" ></asp:TextBox>
                                            </div>
                                        </div>
                                    </div>                    

                                    <div class="row" runat="server" visible="false" id="dvSenha">
                                        <div class="col-sm-6">
                                            <div class="form-group">
                                                <label class="col-sm-12 col-form-label">Senha do Usuário</label>
                                                <asp:TextBox id="txtSenha" runat="server" class="form-control" placeholder="Informe sua Senha"></asp:TextBox>
                                            </div>
                                        </div>
                                        <div class="col-sm-6">
                                            <div class="form-group">
                                                <label class="col-sm-12 col-form-label">Confirme a Senha </label>
                                                <asp:TextBox id="txtSenhaC" runat="server" class="form-control" placeholder="Confirme a Senha"></asp:TextBox>
                                            </div>
                                        </div>
                                    </div>

                                <div class="card-footer">
                                    <asp:Button runat="server" ID="btnVoltar06" CssClass="btn btn-whitelabel1" Text="Voltar" onclick="btnVoltar06_Click" Visible="true"/>
                                    <asp:Button runat="server" ID="btnCancelar" CssClass="btn btn-danger float-right" Text="Cancelar" onclick="btnCancelar_Click"/>
                                    <asp:Button runat="server" ID="btnSalvar" CssClass="btn btn-whitelabel1 float-right" Text="Salvar" onclick="btnSalvar_Click"/>
                                </div>

                            </div>
                        </div>
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
        $('#txtNascimentoPF').inputmask('99/99/9999')
        $('#txtDataAbertura').inputmask('99/99/9999')
        $('#txtDocumento').inputmask('999.999.999-99')
        $('#txtDocumentoPF').inputmask('999.999.999-99')
        $('#txtCelular').inputmask('(99) 99999-9999')
        $('#txtCelularPF').inputmask('(99) 99999-9999')

        $('#txtTelefoneEmpresa').inputmask('(99) 99999-9999')
        $('#txtCEP').inputmask('99999-999')
        $('#txtCNPJ').inputmask('99.999.999/9999-99')

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
