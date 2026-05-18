<%@ Page Language="C#" AutoEventWireup="true" CodeFile="con_meusdados.aspx.cs" Inherits="con_meusdados" %>

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
                              <li class="breadcrumb-item"><a href="#">Minha Conta</a></li>
                              <li class="breadcrumb-item active">Meus Dados</li>
                            </ol>
                          </div>
                        </div>

                        <div class="row">
                          <div class="col-12">

                            <div class="card">

                              <div class="card-header">
                                <h3 class="card-title">Meus Dados</h3>
                              </div>
                              <div class="card-body">

                                <h3><span class="float-center badge bg-whitelabel1">Dados Estabelecimento</span></h3>

                                <div class="row">
                                    <div class="col-sm-2"  >
                                        <div class="form-group">
                                            <label>Tipo</label>
                                            <asp:TextBox id="txtTipo" runat="server" CssClass="form-control" Enabled="false"></asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="col-sm-3"  >
                                        <div class="form-group">
                                            <label>Razão Social/Nome</label>
                                            <asp:TextBox id="txtNome" runat="server" CssClass="form-control" Enabled="false"></asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="col-sm-2"  >
                                        <div class="form-group">
                                            <label>CNPJ/CPF</label>
                                            <asp:TextBox id="txtCNPJCPF" runat="server" CssClass="form-control" Enabled="false"></asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="col-sm-3"  >
                                        <div class="form-group">
                                            <label>E-Mail</label>
                                            <asp:TextBox id="txtEmailEstabelecimento" runat="server" CssClass="form-control" ></asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="col-sm-2"  >
                                        <div class="form-group">
                                            <label>Faturamento</label>
                                            <asp:TextBox id="txtFaturamento" runat="server" CssClass="form-control" ></asp:TextBox>
                                        </div>
                                    </div>

                                </div>
                                <div class="row">
                                    <div class="col-sm-2">
                                        <div class="form-group">
                                            <label>MCC<strong class="text-danger">*</strong></label>
                                            <asp:TextBox id="txtMCC" runat="server" CssClass="form-control" Enabled="false"></asp:TextBox>
                                        </div>
                                    </div>

                                    <div class="col-sm-10">
                                        <div class="form-group">
                                            <label>Atividade Econômica<strong class="text-danger">*</strong></label>
                                            <asp:DropDownList runat="server" id="ddlAtividadeEconomica" CssClass="form-control">
                                            </asp:DropDownList>
                                        </div>
                                    </div>
                                </div>


                                <h3><span class="float-center badge bg-whitelabel1">Dados Responsável</span></h3>

                                <div class="row">
                                    <div class="col-sm-3"  >
                                        <div class="form-group">
                                            <label>Nome</label>
                                            <asp:TextBox id="txtResponsavel" runat="server" CssClass="form-control" Enabled="false"></asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="col-sm-2"  >
                                        <div class="form-group">
                                            <label>CPF</label>
                                            <asp:TextBox id="txtDocumento" runat="server" CssClass="form-control" Enabled="false"></asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="col-sm-2"  >
                                        <div class="form-group">
                                            <label>Celular</label>
                                            <asp:TextBox id="txtCelular" runat="server" CssClass="form-control" Enabled="false"></asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="col-sm-3"  >
                                        <div class="form-group">
                                            <label>E-Mail</label>
                                            <asp:TextBox id="txtEmail" runat="server" CssClass="form-control" ></asp:TextBox>
                                        </div>
                                    </div>

                                    <div class="col-sm-2"  >
                                        <div class="form-group">
                                            <label>Renda Mensal</label>
                                            <asp:TextBox id="txtRendaMensal" runat="server" CssClass="form-control" ></asp:TextBox>
                                        </div>
                                    </div>


                                </div>

                                <h3><span class="float-center badge bg-whitelabel1">Dados do Endereço</span></h3>

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


                                <div class="row">
                                    <div class="col-sm-12"  >
                                        <div class="form-group">
                                            <label>Foto<strong class="text-danger">*</strong></label>
                                            <asp:TextBox runat="server" ID="txtFoto" CssClass="form-control"></asp:TextBox> 
                                              <div class="form-group">
                                                <div class="input-group">
                                                  <div class="custom-file">
                                                    <input type="file" name="attachment" runat="server" id="flFoto"/>
                                                  </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>

                                <div runat="server" id="dvSplit" visible="false">
                                    <h3><span class="float-center badge bg-whitelabel1">Favorecido Split</span></h3>

                                    <div class="row">
                                        <div class="col-sm-12">
                                            <div class="form-group">
                                                <label>Favorecido<strong class="text-danger">*</strong></label>
                                                <asp:DropDownList runat="server" id="ddlFavorecido" CssClass="form-control" 
                                                    AutoPostBack="True" onselectedindexchanged="ddlFavorecido_SelectedIndexChanged">
                                                </asp:DropDownList>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="row">
                                        <div class="col-sm-12">
                                            <h5 class="text-whitelabel1">Lista de adquirentes habilitadas no favorecido do split</h5>
                                        </div>
                                    </div>

                                    <div class="row">

                                            <asp:Repeater runat="server" ID="rptIntegracoes">
                                                <ItemTemplate>

                                                    <div class="col">
                                                        <div class="description-block border-right">
                                                            <h4><span class="badge bg-whitelabel1"><span><%# DataBinder.Eval(Container.DataItem, "NOM_INTEGRACAO")%></span></span></h4><br />
                                                        </div>
                                                    </div>                                        

                                                </ItemTemplate>
                                            </asp:Repeater>
                                    </div>
                                </div>
                                <!-- DADOS BANCÁRIOS -->
                                <h3><span class="float-center badge bg-whitelabel1">Contas Bancárias Repasse/Pagamentos</span></h3>


                                <div class="row" runat="server" id="dvDadosBancarios">
                                    <div class="col-sm-2">
                                        <div class="form-group">
                                        <label>Conta Padrão?<strong class="text-danger">*</strong></label>
                                        <asp:DropDownList runat="server" id="ddlPadrao" CssClass="form-control">
                                            <asp:ListItem Value="N">Não</asp:ListItem>
                                            <asp:ListItem Value="S">Sim</asp:ListItem>
                                        </asp:DropDownList>
                                        </div>
                                    </div>

                                    <div class="col-sm-2">
                                        <div class="form-group">
                                        <label>Inst.Financ.<strong class="text-danger">*</strong></label>
                                        <asp:DropDownList runat="server" id="ddlInstituicaoFinanceira" CssClass="form-control">
                                        </asp:DropDownList>
                                        </div>
                                    </div>

                                    <div class="col-sm-2">
                                        <div class="form-group">
                                        <label>Tipo de Conta<strong class="text-danger">*</strong></label>
                                        <asp:DropDownList runat="server" id="ddlTipoConta" CssClass="form-control">
                                            <asp:ListItem Value="C">Conta Corrente</asp:ListItem>
                                            <asp:ListItem Value="P">Conta Poupança</asp:ListItem>
                                        </asp:DropDownList>
                                        </div>
                                    </div>

                                    <div class="col-sm-2">
                                        <div class="form-group">
                                        <label>Agência<strong class="text-danger">*</strong></label>
                                        <asp:TextBox runat="server" ID="txtAgencia" CssClass="form-control" placeholder="Número da Agencia" ></asp:TextBox>
                                        </div>
                                    </div>

                                    <div class="col-sm-1">
                                        <div class="form-group">
                                        <label>Dígito<strong class="text-danger">*</strong></label>
                                        <asp:TextBox runat="server" ID="txtDigitoAgencia" CssClass="form-control" placeholder="Dígito" ></asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="col-sm-2">
                                        <div class="form-group">
                                        <label>Conta<strong class="text-danger">*</strong></label>
                                        <asp:TextBox runat="server" ID="txtConta" CssClass="form-control" placeholder="Número da Conta" ></asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="col-sm-1">
                                        <div class="form-group">
                                        <label>Dígito<strong class="text-danger">*</strong></label>
                                        <asp:TextBox runat="server" ID="txtDigitoConta" CssClass="form-control" placeholder="Dígito" ></asp:TextBox>
                                        </div>
                                    </div>
                                    <asp:TextBox runat="server" ID="txtTokenConta" CssClass="form-control" Visible="false"></asp:TextBox>

                                    <asp:Button runat="server" ID="btnIncluirContaBancaria" CssClass="btn btn-whitelabel1" Text="Incluir Conta Bancária" onclick="btnIncluirContaBancaria_Click"/>

                                </div>


                                <div class="row mt-3">
                                    <div class="col-12">

                                        <div class="card">
                                            <div class="card-header bg-whitelabel1">
                                                <h3 class="card-title">LISTA DE CONTAS</h3>
                                            </div>
                                            <div class="card-body" style="overflow:auto; width: 100%;">
                                                <table id="tbUsuario" class="table table-bordered table-hover">
                                                    <thead>
                                                    <tr>
                                                        <th>ID</th>
                                                        <th>Token</th>
                                                        <th>Banco</th>
                                                        <th>Tipo</th>
                                                        <th>Agência</th>
                                                        <th>Conta</th>
                                                        <th>Padrão</th>
                                                        <th>Conta Padrão</th>
                                                        <th>Excluir</th>
                                                    </tr>
                                                    </thead>
                                                    <tbody>
                                                    <asp:Repeater runat="server" ID="rptConsultaContas" 
                                                            onitemcommand="rptConsultaContas_ItemCommand">
                                                        <ItemTemplate>
                                                            <tr>
                                                                <td>
                                                                    <small><strong><%# DataBinder.Eval(Container.DataItem, "COD_ID")%></strong></small>
                                                                    <asp:TextBox runat="server" ID="txtid" Text='<%# DataBinder.Eval(Container.DataItem, "COD_ID")%>' Visible="false"></asp:TextBox>
                                                                    <asp:TextBox runat="server" ID="txtTokenConta" Text='<%# DataBinder.Eval(Container.DataItem, "NOM_TOKEN")%>' Visible="false"></asp:TextBox>
                                                                    <asp:TextBox runat="server" ID="txtBanco" Text='<%# DataBinder.Eval(Container.DataItem, "NOM_CODIGO_BANCO")%>' Visible="false"></asp:TextBox>
                                                                    <asp:TextBox runat="server" ID="txtTipo" Text='<%# DataBinder.Eval(Container.DataItem, "NOM_TIPO_BANCO")%>' Visible="false"></asp:TextBox>
                                                                    <asp:TextBox runat="server" ID="txtAgencia" Text='<%# DataBinder.Eval(Container.DataItem, "NOM_NUMERO_AGENCIA_BANCO")%>' Visible="false"></asp:TextBox>
                                                                    <asp:TextBox runat="server" ID="txtConta" Text='<%# DataBinder.Eval(Container.DataItem, "NOM_NUMERO_CONTA_BANCO")%>' Visible="false"></asp:TextBox>
                                                                    <asp:TextBox runat="server" ID="txtContaDigito" Text='<%# DataBinder.Eval(Container.DataItem, "NOM_NUMERO_DIGITO_CONTA_BANCO")%>' Visible="false"></asp:TextBox>
                                                                </td>
                                                                <td style="max-width:200px;">
                                                                    <small><strong><%# Funcoes.Encrypt(DataBinder.Eval(Container.DataItem, "NOM_TOKEN").ToString())%></strong></small>
                                                                </td>

                                                                <td>
                                                                    <small><strong><%# DataBinder.Eval(Container.DataItem, "NOM_CODIGO_BANCO")%></strong></small>
                                                                </td>
                                                                <td>
                                                                    <small><%# DataBinder.Eval(Container.DataItem, "NOM_TIPO_BANCO")%></small>
                                                                </td>
                                                                <td>
                                                                    <small><%# DataBinder.Eval(Container.DataItem, "NOM_NUMERO_AGENCIA_BANCO")%>-<%# DataBinder.Eval(Container.DataItem, "NOM_NUMERO_DIGITO_AGENCIA_BANCO")%></small>
                                                                </td>
                                                                <td>
                                                                    <small><%# DataBinder.Eval(Container.DataItem, "NOM_NUMERO_CONTA_BANCO")%>-<%# DataBinder.Eval(Container.DataItem, "NOM_NUMERO_DIGITO_CONTA_BANCO")%></small>
                                                                </td>
                                                                <td>
                                                                    <small><%# DataBinder.Eval(Container.DataItem, "FLG_PADRAO")%></small>
                                                                </td>

                                                                <td>
                                                                    <small><asp:linkbutton ID="lkbPadrao" CssClass="btn btn-sm btn-primary" commandname="Padrao" runat="server" text="Padrão"  ToolTip="Padrão" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "COD_ID")%>'><i class="fas fa-check"></i></asp:linkbutton></small>
                                                                </td>


                                                                <td>
                                                                    <asp:linkbutton ID="lkbExcluir" CssClass="btn btn-sm btn-danger" commandname="Excluir" runat="server" text="Excluir"  ToolTip="Excluir" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "COD_ID")%>'><i class="fas fa-trash"></i></asp:linkbutton>
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


                                <!-- FIM DADOS BANCÁRIOS -->
                                <h3><span class="float-center badge bg-whitelabel1">Configuração Checkout Pagamento</span></h3>

                                <div class="row">
                                    <div class="col-sm-4">
                                        <div class="form-group">
                                        <label>Cor Primária<strong class="text-danger">*</strong></label>
                                        <asp:TextBox runat="server" ID="txtCorPrimaria" CssClass="form-control my-colorpicker1"></asp:TextBox> 
                                        </div>
                                    </div>

                                    <div class="col-sm-4">
                                        <div class="form-group">
                                        <label>Cor Secundária<strong class="text-danger">*</strong></label>
                                        <asp:TextBox runat="server" ID="txtCorSecundaria" CssClass="form-control my-colorpicker1"></asp:TextBox> 
                                        </div>
                                    </div>

                                    <div class="col-sm-4">
                                        <div class="form-group">
                                        <label>Cor Fonte<strong class="text-danger">*</strong></label>
                                        <asp:TextBox runat="server" ID="txtCorFonte" CssClass="form-control my-colorpicker1"></asp:TextBox> 
                                        </div>
                                    </div>

                                </div>

                                <div class="row">
                                    <div class="col-sm-6"  >
                                        <div class="form-group">
                                            <label>Logotipo<strong class="text-danger">*</strong></label>
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

                                    <div class="col-sm-6"  >
                                        <div class="form-group">
                                            <label>Imagem<strong class="text-danger">*</strong></label>
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



                                <div class="form-group row col-12">

                                </div>
                                
                              </div>

                              <div class="card-footer" style="overflow:auto; width: 100%; ">
                                <div class="row">
                                    <div class="col-12">
                                        <asp:Button runat="server" ID="btnSalvar" CssClass="btn btn-whitelabel1" Text="Salvar" onclick="btnSalvar_Click"/>
                                    </div>
                                </div>
                              </div>

                              </div>

                            </div>
                          </div>



                        </div>

                </section>
            </div>
            <PORTAL:PAGEBOTTOM id="Pageheader3" title="Site Directory" runat="server" ModuleSource="rodapepadrao.ascx"></PORTAL:PAGEBOTTOM><!-- Fim Rodapé da Pagina -->
    
        </div>
        <!-- Modal de confirmação 2FA -->
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
                    <asp:TextBox runat="server" ID="txt2FA" CssClass="form-control" placeholder="Código de Confirmação" Visible="true"></asp:TextBox>
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

<script type="text/javascript" src="../plugins/bootstrap-colorpicker/js/bootstrap-colorpicker.min.js"></script>

<script>
    $(function () {

        //Colorpicker
        $('.my-colorpicker1').colorpicker()

    })
</script>


<script type="text/javascript">

    $(function () {

        //Date range picker
        $('#datepickerIni').datetimepicker({
            format: 'DD/MM/YYYY'
        });
        $('#datepickerFim').datetimepicker({
            format: 'DD/MM/YYYY'
        });
    })
</script>        


<script type="text/javascript">

$(document).ready(function () {
    var table = $('#tblConsulta').DataTable({
      order: [[0, 'desc']],
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


</body>
</html>
