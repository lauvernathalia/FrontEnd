<%@ Page Language="C#" AutoEventWireup="true" CodeFile="index.aspx.cs" Inherits="index" Async="true" EnableSessionState="ReadOnly" MaintainScrollPositionOnPostback="true"  %>

<%@ Register TagPrefix="Portal" TagName="PageBottom" Src="rodapepadrao.ascx" %>
<%@ Register TagPrefix="Portal" TagName="PageLeft" Src="menupadrao.ascx" %>
<%@ Register TagPrefix="Portal" TagName="PageHeader" Src="topopadrao.ascx" %>
<%@ Register TagPrefix="Portal" TagName="PageRotina" Src="rotinaspadroes.ascx" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
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

    <link href="https://fontawesome.com/icons/pix?f=brands&s=solid" rel="Stylesheet" />
    <script src="https://kit.fontawesome.com/91552b3746.js" crossorigin="anonymous"></script>

</head>
<body class="hold-transition sidebar-mini layout-fixed">
    <form id="frmPrincipal" runat="server">
    

        <div class="wrapper">

            <PORTAL:PAGEHEADER id="PageHeader1" title="Site Directory" runat="server" ModuleSource="topopadrao.ascx"></PORTAL:PAGEHEADER>

            <PORTAL:PAGELEFT id="Pageheader2" title="Site Directory" runat="server" ModuleSource="menupadrao.ascx"></PORTAL:PAGELEFT>


            <div class="content-wrapper">

                <section class="content">

                <!-- SETOR PADRÃO DO DASHBOARD -->
                    <div class="container-fluid" runat="server" id="cntPadrao">

                        <!-- Painel Titulo -->    
                        <div class="row mb-2" runat="server" id="dvTitulo" visible="true">
                          <div class="col-sm-12">
                            <ol class="breadcrumb">
                              <li class="breadcrumb-item"><a href="#">Dashboard</a></li>
                              <li class="breadcrumb-item active"><asp:Label runat="server" ID="lblTitulo" Text=""></asp:Label></li>
                            </ol>
                          </div>
                        </div>

                        <!-- Painel de Documentos Conta Digital -->
                        <div class="row p-2" runat="server" id="dvDocumentosContaDigital" visible="false">
                            <div class="col-12">
                                <div class="alert alert-warning alert-dismissible">
                                  <button type="button" class="close" data-dismiss="alert" aria-hidden="true">&times;</button>
                                  <h5><i class="icon fas fa-exclamation-triangle"></i> ATENÇÃO!</h5>
                                  <asp:Label runat="server" ID="lblToken" Text="token" Visible="false"></asp:Label>
                                  Sua CONTA DIGITAL ainda NÃO ESTÁ HABILITADA! Envie os documentos para análise clicando no botão abaixo. <br />Qualquer dúvida não hesite em entrar em contato conosco através do nosso suporte.<br />
                                  <a href="" runat="server" id="hrfDocumentos" class="btn btn-sm btn-secondary text-white" target="_blank">Enviar os Documentos</a>
                                </div>
                            </div>
                            
                        </div>


                        <!-- Painel Documentos --> 
                        <asp:Repeater runat="server" ID="rptDocumentos" 
                            onitemcommand="rptDocumentos_ItemCommand" Visible="true">
                        <ItemTemplate>
                            <div class="row p-2"  runat="server" id="dvDocumentos" visible="true">
                                <div class="col-12">
                                    <div class="alert alert-secondary alert-dismissible">
                                      <button type="button" class="close" data-dismiss="alert" aria-hidden="true">&times;</button>
                                      <h6><i class="icon fas fa-exclamation-triangle"></i> Você ainda não assinou o documento: <b><%# DataBinder.Eval(Container.DataItem, "NOM_DOCUMENTO")%></b></h6>
                                      <asp:linkbutton ID="lbkAssinarDocumento" commandname="Assinar" CssClass="btn btn-whitelabel1" runat="server" text="Clique aqui para assinar o documento digitalmente" ToolTip="Clique aqui para assinar o documento digitalmente"  CommandArgument='<%# DataBinder.Eval(Container.DataItem, "COD_ID_DOCUMENTO")%>'></asp:linkbutton>
                                      <asp:linkbutton ID="lbkNaoAssinarDocumento" commandname="NaoAssinar" CssClass="btn btn-danger float-right" runat="server" text="Clique aqui caso não queira assinar" ToolTip="Clique aqui caso não queira assinar"  CommandArgument='<%# DataBinder.Eval(Container.DataItem, "COD_ID_DOCUMENTO")%>'></asp:linkbutton>
                                    </div>
                                </div>
                            </div>
                        </ItemTemplate>
                        </asp:Repeater>



                        <!-- Painel Data --> 
                        <asp:Repeater runat="server" ID="rptMensagens">
                        <ItemTemplate>
                        <div class="row p-2"  runat="server" id="dvMensagens" visible="true">
                            <div class="col-12">
                                <div class="alert alert-warning alert-dismissible">
                                  <button type="button" class="close" data-dismiss="alert" aria-hidden="true">&times;</button>
                                  <h5><i class="icon fas fa-exclamation-triangle"></i> <%# DataBinder.Eval(Container.DataItem, "NOM_MENSAGEM")%></h5>
                                  <%# DataBinder.Eval(Container.DataItem, "DES_MENSAGEM")%>
                                </div>
                            </div>
                        </div>

                        </ItemTemplate>
                        </asp:Repeater>

                        <!-- Painel Atualizacoes -->


                                                <div class="row" runat="server" id="divAtualizacoes" visible="false">
                                                    <div class="col-12">
                                                    <div class="card">
                                                    <div class="card-header">
                                                        <h4 class="card-title">ATUALIZAÇÕES & MELHORIAS</h3>
                                                    </div>
                                                    <div class="card-body">
                                                        <h6><small><b>ATENÇÃO! </b>Para visualizar os detalhes da atualização ou melhoria do portal basta clicar no símbolo de + (mais) no canto direito</small></h6>

                                        <asp:Repeater runat="server" ID="rptAtualizacoes">
                                            <ItemTemplate>
                                                <div class="row  p-2">
                                                    <div class="col-12">
                                                    <div class="card card-info collapsed-card">

                                                      <div class="card-header">
                                                        <h6 class="card-title"><%# DataBinder.Eval(Container.DataItem, "NOM_ATUALIZACAO")%></h6>
                                                            <div class="card-tools">
                                                              <button type="button" class="btn btn-tool" data-card-widget="collapse">
                                                                <i class="fas fa-plus"></i>
                                                              </button>
                                                            <button type="button" class="btn btn-tool" data-card-widget="remove">
                                                                <i class="fas fa-times"></i>
                                                            </button>
                                                            </div>
                                                      </div>

                                                      <div class="card-body">
                                                        <small><%# DataBinder.Eval(Container.DataItem, "DES_ATUALIZACAO")%></small>
                                                      </div>
                                                    </div>
                                                    </div>
                                                </div>
                                            </ItemTemplate>
                                        </asp:Repeater>                                    

                                                </div>
                                                </div>
                                                </div>
                                            </div>

                        <div class="row"  runat="server" id="dvRetorno" visible="false">
                            <div class="col-12">
                                <asp:TextBox runat="server" ID="txtRetorno" CssClass="form-control"></asp:TextBox>
                            </div>
                        </div>




                        <div class="row"  runat="server" id="dvData" visible="true">
                            <div class="col-12">
                                <div class="card">
                                    <div class="card-body">
                        
                                        <div class="row fw-bold">
                                            <label class="col-12 col-form-label"><asp:Label runat="server" id="lblData"></asp:Label></label>
                                        </div>

                                        <div class="row fw-bold">

                                            <label class="col-1 col-form-label">Período</label>
						                    <div class="col-2">
                                                <div class="input-group date" id="datepickerIni" data-target-input="nearest">
								                    <asp:TextBox ID="txtDataIni" runat="server" CssClass="form-control datetimepicker-input" data-target="#datepickerIni"></asp:TextBox>
                                                    <div class="input-group-append" data-target="#datepickerIni" data-toggle="datetimepicker">
                                                        <div class="input-group-text"><i class="fa fa-calendar"></i></div>
                                                    </div>
                                                </div>
                                            </div>
                                            <label class="col-1 col-form-label">à</label>
						                    <div class="col-2">
                                                <div class="input-group date" id="datepickerFim" data-target-input="nearest">
								                    <asp:TextBox ID="txtDataFim" runat="server" CssClass="form-control datetimepicker-input" data-target="#datepickerFim"></asp:TextBox>
                                                    <div class="input-group-append" data-target="#datepickerFim" data-toggle="datetimepicker">
                                                        <div class="input-group-text"><i class="fa fa-calendar"></i></div>
                                                    </div>
                                                </div>
                                            </div>
                                            <label class="col-2 col-form-label">Adquirente</label>
						                    <div class="col-2">
                                                <asp:DropDownList runat="server" ID="ddlAdquirentes" CssClass="form-control" 
                                                    onselectedindexchanged="ddlAdquirentes_SelectedIndexChanged" 
                                                    AutoPostBack="True"></asp:DropDownList>
                                            </div>

                                            <asp:Button runat="server" ID="btnAtualizar" CssClass="btn btn-sm btn-whitelabel1" 
                                                Text="Atualizar" onclick="btnAtualizar_Click" />
                                            <asp:Button runat="server" ID="btnExtratoEDI" CssClass="btn btn-sm btn-whitelabel1" 
                                                Text="Extrato EDI" onclick="btnExtratoEDI_Click" />
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <!-- Painel Alerta -->

                        <div class="row" runat="server" id="dvAlerta" visible="false">
                            <div class="col-md-12 col-xs-12 my-1">
                                <div class="alert alert-warning alert-dismissible">
                                    <h5><i class="fa-solid fa-circle-exclamation"></i> Alerta!</h5>
                                    Alguma coisa deu errado!.
                                </div>
                            </div>
                        </div>



                        <div class="row" runat="server" id="dvBotoes01" visible="false">
                            <div class="col-md-8 col-xs-8 my-1">
                                <center>
                                <div class="card">
                                    <div class="card-body">
                                        <div class="row">
                                            <a href="con_pagamentos_transferencias.aspx" class="btn btn-app btn-outline-success text-whitelabel1 d-flex justify-content-center d-md-table mx-auto "><i class="fas fa-barcode fa-3x"></i>Pagamentos & Transferências</a>
                                            <a href="con_pagamentos_pix.aspx" class="btn btn-app btn-outline-success text-whitelabel1 d-flex justify-content-center d-md-table mx-auto "><i class="fab fa-pix fa-3x"></i>Pix</a>
                                            <!--<a href="con_cobrancas_link_pagamento.aspx" class="btn btn-app btn-outline-success text-whitelabel1 d-flex justify-content-center d-md-table mx-auto "><i class="fas fa-file-invoice-dollar fa-3x"></i>Cobrança</a>-->

                                        </div>
                                    </div>
                                </div>    
                                </center>
                            </div>
                            <div class="col-md-4 col-xs-4 my-1">
                                <center>
                                <div class="card">
                                    <div class="card-body">
                                        <div class="row">
                                            <a href="con_widget_externo.aspx?name=Consulte GRÁTIS e pague suas Multas, IPVA e Licenciamento" runat="server" id="hrfWidget01" visible="false" class="btn btn-app btn-outline-success text-whitelabel1 d-flex justify-content-center d-md-table mx-auto"><i class="fas fa-divide fa-3x"></i>Parcele e Pague</a>

                                        </div>
                                    </div>
                                </div>    
                                </center>
                            </div>

                        </div>

                        <!-- Painel Botoes Estabelecimento -->
                        <div class="row" runat="server" id="dvBotoes" visible="false" style="display:none;">
                            <div class="col-md-12 col-xs-12 my-1">
                                <center>
                                <div class="card">
                                    <div class="card-body">
                                        <div class="row">
                                            <a class="btn btn-app btn-whitelabel1 text-white col" runat="server" id="hrfCartaoCredito" visible="false"><i class="fas fa-credit-card fa-3x"></i> Cartão de Crédito</a>
                                            <a href="con_boletos_bancarios.aspx" class="btn btn-app btn-whitelabel1 text-white col"><i class="fas fa-barcode fa-3x"></i> Boleto</a>
                                            <a href="con_carne.aspx" class="btn btn-app btn-whitelabel1 text-white col"><i class="fas fa-file-invoice fa-3x"></i> Carnê</a>
                                            <a href="con_assinatura_recorrente.aspx" class="btn btn-app btn-whitelabel1 text-white col"><i class="fas fa-calendar-alt fa-3x"></i> Assinaturas</a>
                                            <a href="con_vendas_link.aspx" class="btn btn-app btn-whitelabel1 text-white col"><i class="fas fa-money-check-alt fa-3x"></i> Link de Pagamento</a>
                                            <a href="con_vendas_link.aspx" class="btn btn-app btn-whitelabel1 text-white col"><i class="fas fa-comments-dollar fa-3x"></i> Cobrança</a>
                                            <a href="con_extrato_vendas.aspx" class="btn btn-app btn-whitelabel1 text-white col"><i class="fas fa-receipt fa-3x"></i> Extrato</a>
                                        </div>
                                    </div>
                                </div>    
                                </center>
                            </div>
                        </div>



                        <!-- Painel Vendas -->

                        <div class="row"  runat="server" id="dvVendas" visible="true">
                            
                            <!--<div class="col-sm-3 col-6">
                                <div class="description-block border-right">
                                    <span class="description-percentage text-success"><i class="far fa-thumbs-up text-whitelabel1"></i></span>
                                    <h6 class="description-header"><small>APROVADAS</small></h6>
                                    <span class="description-text"><small>R$ <%=PainelVendas("A") %></small></span>
                                </div>
                            </div>-->


                            <div class="col-md-2 col-xs-12 my-1">
                                <div class="card">
                                    <div class="card-body">
                                        <div class='d-flex align-items-center'>
                                            <div class='flex-grow-1'>
                                                <p class='mb-0 fw-bold' "><small><b>APROVADAS</b></small></p>
                                                <h6 class="mb-0 fw-bold"><small>R$ <%=PainelVendas("A") %></small></h6>
                                            </div>
                                            <div class='flex-shrink-0'>
                                                <h5 class="mb-0"><i class="far fa-thumbs-up text-whitelabel1"></i></h5>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>

                            <div class="col-md-2 col-xs-12 my-1">
                                <div class="card">
                                    <div class="card-body">
                                        <div class='d-flex align-items-center'>
                                            <div class='flex-grow-1'>
                                                <p class='mb-0 fw-bold' "><small><b>QUANTIDADE</b></small></p>
                                                <h6 class="mb-0 fw-bold"><small><%=PainelVendas("Q") %></small></h6>
                                            </div>
                                            <div class='flex-shrink-0'>
                                                <h5 class="mb-0"><i class="far fa-chart-bar text-whitelabel1"></i></h5>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>

                            <div class="col-md-2 col-xs-12 my-1">
                                <div class="card">
                                    <div class="card-body">
                                        <div class='d-flex align-items-center'>
                                            <div class='flex-grow-1'>
                                                <p class='mb-0 fw-bold' "><small><b>FALHADAS</b></small></p>
                                                <h6 class='mb-0 fw-bold' ><small>R$ <%=PainelVendas("F") %></small></h6>
                                            </div>
                                            <div class='flex-shrink-0'>
                                                <h5 class="mb-0"><i class="far fa-thumbs-down text-danger"></i></h5>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>

                            <div class="col-md-2 col-xs-12 my-1">
                                <div class="card">
                                    <div class="card-body">
                                        <div class='d-flex align-items-center'>
                                            <div class='flex-grow-1'>
                                                <p class='mb-0 fw-bold' "><small><b>PENDENTES</b></small></p>
                                                <h6 class='mb-0 fw-bold' ><small>R$ <%=PainelVendas("P") %></small></h6>
                                            </div>
                                            <div class='flex-shrink-0'>
                                                <h5 class="mb-0"><i class="fas fa-hourglass-start text-warning"></i></h5>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>

                            <div class="col-md-2 col-xs-12 my-1">
                                <div class="card">
                                    <div class="card-body">
                                        <div class='d-flex align-items-center'>
                                            <div class='flex-grow-1'>
                                                <p class='mb-0 fw-bold' "><small><b>TICKET MÉDIO</b></small></p>
                                                <h6 class='mb-0 fw-bold'><small>R$ <%=PainelVendas("M") %></small></h6>
                                            </div>
                                            <div class='flex-shrink-0'>
                                                <h5 class="mb-0"><i class="fas fa-money-bill text-info"></i></h5>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="col-md-2 col-xs-12 my-1">
                                <div class="card">
                                    <div class="card-body">
                                        <div class='d-flex align-items-center'>
                                            <div class='flex-grow-1'>
                                                <p class='mb-0 fw-bold' "><small><b>CHARGEBACK</b></small></p>
                                                <h6 class='mb-0 fw-bold'><small>R$ 0,00</small></h6>
                                            </div>
                                            <div class='flex-shrink-0'>
                                                <h5 class="mb-0"><i class="fas fa-recycle text-warning"></i></h5>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>

                        </div>


                        <!-- áinel do Saldo Disponível -->

                        <div class="row" runat="server" id="dvSaldo" visible="true">

                            <div class="col-md-4 col-xs-12" runat="server" id="dvSaldoConta" visible="true">
                                <div class="row">
                                    <div class="col-12 my-1">
                                        <div class="card">
                                            <div class="card-body">
                                                <center>
                                                <p class="text-center"><strong>SALDO DISPONÍVEL</strong></p>
                                                <p class="text-center text-whitelabel1"><strong>R$ <asp:Label runat="server" ID="lblSaldoDisponivel" Text="0,00"></asp:Label> <asp:Label runat="server" ID="lblSaldoDisponivelHiden" Text="0,00" Visible="false"></asp:Label> </strong><i class="fas fa-eye float-end text-whitelabel1" onclick=""></i></p>
                                                <a href="con_extrato_vendas.aspx" class="btn btn-sm mx-2 text-center btn-whitelabel1">Extrato</a> 
                                                </center>
                                            </div>
                                        </div>
                                    </div>
                                </div>
    
                                <div class="row">
                                    <div class="col-12 my-1">
                                        <div class="card">
                                            <div class="card-body">
                                            <div class="row">
    
                                            <div class="col-6 my-1 btn-legacy-green-2">
                                                        <p><strong>Faturamento</strong><span class="badge bg-whitelabel1 float-end">Hoje</span><br />
                                                        <strong>R$ 0,00</strong><br />
                                                        <i class="fas fa-arrow-up text-whitelabel1"></i><strong >0% desde mês passado</strong></p>
                                            </div>
                                            <div class="col-6 my-1 btn-legacy-green-2">
                                                        <p><strong>Faturamento</strong><span class="badge bg-whitelabel1 float-end">Mês</span><br />
                                                        <strong>R$ 0,00</strong><br />
                                                        <i class="fas fa-arrow-up text-whitelabel1"></i><strong>0% desde mês passado</strong></p>
                                            </div>
                                            </div>
                                            <div class="row">
    
                                            <div class="col-6 my-1 btn-legacy-green-2">
                                                <p><strong>Saldo de Vendas</strong><br />
                                                <strong>R$ <asp:Label runat="server" ID="lblSaldoDisponivelDetalhe" Text="0,00"></asp:Label></strong><br />
                                                        
                                                <strong class="text-whitelabel1 text-center">
                                                    <a href="con_extrato_vendas.aspx" class="btn btn-sm btn-whitelabel1">Extrato</a>
                                                </strong>
                                                <strong class="text-whitelabel1 text-center" style="display:none;">
                                                    <a data-toggle="modal" href="#modal-saque" class="btn btn-sm btn-whitelabel1">
                                                        Saque                                                    
                                                    </a>
                                                </strong>
                                                </p>



                                                <!-- Saque via Modal -->
                                                <div class="modal" id="modal-saque">
                                                    <div class="modal-dialog">
                                                        <div class="modal-content">
                                                            <div class="container"></div>
                                                            <div class="modal-body text-black">

                                                                    <h3 class="modal-title text-black text-center"><b class="text-whitelabel1">FAZER</b> TRANSFERÊNCIA</h3>
                                                                    <hr />
                                                                    <h6>Saldo disponível na <b>Conta Digital</b></h6>
                                                                    <h4 class="text-whitelabel1">R$ <asp:Label runat="server" ID="lblSaldoContaDigital"></asp:Label></h4>
                                                                    <hr />
                                                                    <h6>Valor da <b>Transferência</b></h6>
                                                                    <asp:TextBox runat="server" ID="txtValorSaque" CssClass="form-control" Text="0,00"></asp:TextBox>
                                                                    <hr />
                                                                    <h6>Transferir <b>para</b></h6>
                                                                    <div class="form-check mb-3">
                                                                        <div class="row">
                                                                        <div class="col-6">
                                                                            <asp:RadioButton runat="server" ID="rbtContaDigital" 
                                                                                GroupName="ContaTransferencia"  CssClass="form-check-input" 
                                                                                oncheckedchanged="rbtContaDigital_CheckedChanged"  AutoPostBack="True" />
                                                                            <label class="form-check-label">Conta Digital</label>
                                                                        </div>
                                                                        <div class="col-6">
                                                                            <asp:RadioButton runat="server" ID="rbtContaBancaria" 
                                                                                GroupName="ContaTransferencia"  CssClass="form-check-input" Checked="true" 
                                                                                oncheckedchanged="rbtContaBancaria_CheckedChanged" AutoPostBack="True" />
                                                                            <label class="form-check-label">Conta Bancária</label>
                                                                        </div>
                                                                        </div>
                                                                    </div>
                                                                    <div class="row" runat="server" id="divContaBancaria" visible="true">
                                                                        <div class="col-12">
                                                                            <h6>Conta Bancária do <b>Favorecido</b></h6>
                                                                            <asp:DropDownList runat="server" ID="ddlContasBancarias" CssClass="form-control">
                                                                            </asp:DropDownList>
                                                                        </div>
                                                                    </div>

                                                                    <div class="row" runat="server" id="divContaDigital" visible="false">
                                                                        <div class="col-12">
                                                                            <h6>CNPJ ou CPF do <b>Favorecido</b></h6>
                                                                            <asp:TextBox runat="server" ID="txtCNPJCPFTransferencia" 
                                                                                CssClass="form-control" Text="" AutoPostBack="True" 
                                                                                ontextchanged="txtCNPJCPFTransferencia_TextChanged"></asp:TextBox>
                                                                        </div>
                                                                    </div>


                                                                    <div class="card" runat="server" id="crdCNPJCPF" visible="false">
                                                                    <div class="card-body">
                                                                    <div class="row">
                                                                        <div class="col-1">
                                                                            <br />
                                                                        </div>
                                                                        <div class="col-1">
                                                                            <asp:RadioButton runat="server" ID="rbtCNPJCPFSelecionado" GroupName='grnCNPJCPFSelecionado' CssClass="form-check-input" />
                                                                        </div>
                                                                        <div class="col-10">
                                                                            <h6>Trasferir para <b><asp:Label runat="server" ID="lblCNPJCPFNomeFavorecido"></asp:Label></b></h6>
                                                                            <asp:TextBox runat="server" ID="txtTokenFavorecido" Text="" Visible="false"></asp:TextBox>
                                                                            <h6><b>CPF</b> <asp:Label runat="server" ID="lblCNPJCPFDocumento"></asp:Label></h6>
                                                                            <h6><b>E-Mail</b> <asp:Label runat="server" ID="lblCNPJCPFEmail"></asp:Label></h6>
                                                                        </div>
                                                                    </div>
                                                                    </div>                                                                        
                                                                    </div>


                                                                    <!-- Bloco de Contas Bancárias -->
                                                                    <asp:Repeater runat="server" ID="rptContasBancarias" Visible="false">
                                                                        <ItemTemplate>
                                                                            <div class="card">
                                                                            <div class="card-body">
                                                                            <div class="row">
                                                                                <div class="col-1">
                                                                                    <br />
                                                                                </div>
                                                                                <div class="col-1">
                                                                                    <asp:RadioButton runat="server" ID="rbtContaSelecionada" GroupName='grnContaSelecionada' CssClass="form-check-input" />
                                                                                </div>
                                                                                <div class="col-5">
                                                                                    <h6><b>CNPJ/CPF</b> <%# DataBinder.Eval(Container.DataItem, "NOM_CNPJCPF")%></h6>
                                                                                    <h6><b>Titular</b> <%# DataBinder.Eval(Container.DataItem, "NOM_TITULAR")%></h6>
                                                                                </div>
                                                                                <div class="col-5">
                                                                                    <h6><b>Banco</b> <%# DataBinder.Eval(Container.DataItem, "NOM_BANCO")%></h6>
                                                                                    <h6><b>Ag</b> <%# DataBinder.Eval(Container.DataItem, "NOM_NUMERO_AGENCIA_BANCO")%></h6>
                                                                                    <h6><b>Conta</b> <%# DataBinder.Eval(Container.DataItem, "NOM_NUMERO_CONTA_BANCO")%></h6>
                                                                                        
                                                                                </div>
                                                                            </div>
                                                                            </div>                                                                        
                                                                            </div>
                                                                        </ItemTemplate>
                                                                    </asp:Repeater>


                                                                    <!-- Fim Bloco de Contas Bancárias -->

                                                                    <hr />
                                                                    <h6>Descrição da<b> Transferência</b></h6>
                                                                    <asp:TextBox runat="server" ID="txtDescricaoSaque" CssClass="form-control" ></asp:TextBox>
                                                                    <hr />
                                                                    <h6>Transferências solicitadas em dia útil antes das 11hs, serão realizadas no mesmo dia. Após esse horário, no próximo dia útil.</h6>


                                                            </div>

                                                            <div class="modal-footer">
                                                                <center>

                                                                    <asp:Button runat="server" ID="btnConfirmarSaque" CssClass="btn btn-whitelabel1 text-center btn-sm" Text="Confirmar" onclick="btnConfirmarSaque_Click" />
                                                                    <a href="#" data-dismiss="modal" class="btn btn-whitelabel1 text-center btn-sm">Fechar</a>
                                                                </center>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>

                                            </div>
                                            <div class="col-6 my-1 btn-legacy-green-2">
                                                        <p>
                                                            <strong>Lançamentos Futuros</strong><br />
                                                            <strong>R$ <asp:Label runat="server" ID="lblLancamentosFuturosDetalhe" Text="0,00"></asp:Label></strong><br />
                                                            <strong class="text-whitelabel1 text-center">
                                                                <a href="con_antecipar_vendas.aspx" class="btn btn-sm btn-whitelabel1">Antecipar agora</a>
                                                            </strong>
                                                        </p>
    
    
                                            </div>
                                            </div>
    
    
                                            </div>
                                        </div>
                                    </div>
                                </div>
    
    
                            </div>
    

                            <div class="col-sm-4 col-xs-12 col-12 d-flex align-self-stretch" runat="server" id="dvMovimentacao" visible="true">
                                <div class="card col-12 d-flex flex-column">
                                    <div class="card-body">
                                        <p><strong >MOVIMENTAÇÃO</strong></p>
                                        <div class="mw-100 mx-auto d-flex align-items-center align-middle">

                                            <canvas id="myChartDonut2" style="width:100%;max-width:300px"></canvas>

                                            <script>
                                                var xValues = ['Entradas', 'Saídas'];
                                                var yValues = [55, 49];
                                                var barColors = <%=Cores() %>


                                                new Chart("myChartDonut2", {
                                                    type: "doughnut",
                                                    data: {
                                                        labels: xValues,
                                                        datasets: [{
                                                            backgroundColor: barColors,
                                                            data: yValues
                                                        }]
                                                    },
                                                    options: {
                                                        title: {
                                                            display: true,
                                                            text: "Vendas"
                                                        }
                                                    }
                                                });
                                            </script>
                                        </div>
                                    </div>
                                </div>
                            </div>


                            <div class="col-sm-4 col-xs-12 col-12 d-flex align-self-stretch" runat="server" id="dvCanaisVendas" visible="true">
                                <div class="card col-12 d-flex flex-column">
                                    <div class="card-body">
                                        <p><strong >CANAIS DE VENDAS</strong></p>
                                        <div class="mw-100 mx-auto d-flex align-items-center align-middle">

                                            <canvas id="myChartDonut3" style="width:100%;max-width:300px"></canvas>

                                            <script>
                                                var xValues = ['Físico', 'On-line'];
                                                var yValues = [55, 49];
                                                var barColors = <%=Cores() %>

                                                new Chart("myChartDonut3", {
                                                    type: "doughnut",
                                                    data: {
                                                        labels: xValues,
                                                        datasets: [{
                                                            backgroundColor: barColors,
                                                            data: yValues
                                                        }]
                                                    },
                                                    options: {
                                                        title: {
                                                            display: true,
                                                            text: "Vendas"
                                                        }
                                                    }
                                                });
                                            </script>
                                        </div>
                                    </div>
                                </div>
                            </div>

   
                        </div>
                        <!-- ********************************************************************************************************************************************************* -->
                        <!-- TRANSAÇÕES POR LICENCIADO -->
                        <!-- ********************************************************************************************************************************************************* -->

                        <div class="row" runat="server" id="dvTransacoesLicenciados" visible="false">
                            <!-- Painel Progressão Vendas -->
                            <div class="col-sm-6 col-xs-12 col-12 d-flex align-self-stretch">
                                <div class="card d-flex flex-column">
                                    <div class="card-header bg-whitelabel1">
                                        TRANSAÇÕES POR LICENCIADO - NO PERÍODO E APROVADAS
                                    </div>
                                    <div class="card-body" style="overflow:auto; width: 100%;">
                                        <div class="row">
                                            <div class="col-12">
                                                <table id="tabConsultaTransacoesLicenciado" class="table table-bordered table-hover">
                                                    <thead>
                                                        <tr>
                                                            <th>Nome</th>
                                                            <th>Qtde</th>
                                                            <th>Valor Bruto</th>
                                                            <th>Valor Líquido</th>
                                                        </tr>
                                                    </thead>
                                                    <tbody>
                                                        <asp:Repeater runat="server" ID="rptConsultaTransacoesLicenciado">
                                                            <ItemTemplate>
                                                                <tr>
                                                                    <td>
                                                                        <small><%# DataBinder.Eval(Container.DataItem, "NOM_RAZAOSOCIAL")%></small>
                                                                    </td>
                                                                    <td>
                                                                        <small><%# String.Format("{0:n0}",DataBinder.Eval(Container.DataItem, "NUM_TRANSACOES"))%></small>
                                                                    </td>
                                                                    <td>
                                                                        <small><%# String.Format("{0:n2}",DataBinder.Eval(Container.DataItem, "NUM_VALOR_BRUTO"))%></small>
                                                                    </td>
                                                                    <td>
                                                                        <small><%# String.Format("{0:n2}",DataBinder.Eval(Container.DataItem, "NUM_VALOR_LIQUIDO"))%></small>
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
                            </div>

                            <div class="col-sm-6 col-xs-12 col-12 d-flex align-self-stretch">
                                <div class="card d-flex flex-column">
                                    <div class="card-header bg-whitelabel1">
                                        TRANSAÇÕES POR ADQUIRENTE - NO PERÍODO
                                    </div>

                                    <div class="card-body" style="overflow:auto; width: 100%;">
                                        <div class="row">
                                            <div class="col-12">
                                                <table id="tabConsultaTransacoesAdquirente" class="table table-bordered table-hover">
                                                    <thead>
                                                        <tr>
                                                            <th>Nome</th>
                                                            <th>Qtde</th>
                                                        </tr>
                                                    </thead>
                                                    <tbody>
                                                        <asp:Repeater runat="server" ID="rptConsultaTransacoesAdquirente" >
                                                            <ItemTemplate>
                                                                <tr>
                                                                    <td>
                                                                        <small><%# DataBinder.Eval(Container.DataItem, "NOM_ADQUIRENTE")%></small>
                                                                    </td>
                                                                    <td>
                                                                        <small><%# String.Format("{0:n0}",DataBinder.Eval(Container.DataItem, "NUM_TRANSACOES"))%></small>
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
                            </div>
                            <div class="col-sm-12 col-xs-12 col-12">
                                <div class="card d-flex flex-column">
                                    <div class="card-header bg-whitelabel1">
                                        ESTABELECIMENTOS POR LICENCIADO - NO PERÍODO
                                    </div>
                                    <div class="card-body">
                                        <div class="row">
                                            <div class="col-12">
                                                <table id="tabConsultaEstabelecimentosLicenciado" class="table table-bordered table-hover">
                                                    <thead>
                                                        <tr>
                                                            <th>Data</th>
                                                            <th>Tipo</th>
                                                            <th>Estabelecimento</th>
                                                            <th>Licenciado</th>
                                                        </tr>
                                                    </thead>
                                                    <tbody>
                                                        <asp:Repeater runat="server" ID="rptConsultaEstabelecimentosLicenciado">
                                                            <ItemTemplate>
                                                                <tr>
                                                                    <td>
                                                                        <small><%# String.Format("{0:dd/MM/yyyy}",DataBinder.Eval(Container.DataItem, "DTA_DATA"))%></small>
                                                                    </td>
                                                                    <td>
                                                                        <small><%# DataBinder.Eval(Container.DataItem, "FLG_TIPO_PESSOA")%></small>
                                                                    </td>
                                                                    <td>
                                                                        <small><%# DataBinder.Eval(Container.DataItem, "NOM_ESTABELECIMENTO")%></small>
                                                                    </td>
                                                                    <td>
                                                                        <small><%# DataBinder.Eval(Container.DataItem, "NOM_LICENCIADO")%></small>
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
                            </div>

                        </div>

                        <!-- ********************************************************************************************************************************************************* -->
                        <!-- PROGRESSÃO DAS VENDAS -->
                        <!-- ********************************************************************************************************************************************************* -->
                        <div class="row" runat="server" id="dvProgressao" visible="true">
                            <!-- Painel Progressão Vendas -->
                            <div class="col-sm-8 col-xs-12 col-12 d-flex align-self-stretch" runat="server" id="dvProgressaoVendas" visible="true">
                                <div class="card col-12 d-flex flex-column">
                                    <div class="card-body">
                                        <p><strong ><%=ConsultaTitulo() %></strong></p>
                                        <p>
                                            <asp:Button runat="server" ID="btnOntem" 
                                                class="mx-2 btn btn-sm btn-outline btn-whitelabel1" text="Ontem" 
                                                onclick="btnOntem_Click"/>
                                            <asp:Button runat="server" ID="btnHoje" 
                                                class="mx-2 btn btn-sm btn-outline btn-whitelabel1" text="Hoje" 
                                                onclick="btnHoje_Click"/>
                                            <asp:Button runat="server" ID="btnSemana" 
                                                class="mx-2 btn btn-sm btn-outline btn-whitelabel1" text="Semana" 
                                                onclick="btnSemana_Click"/>
                                            <asp:Button runat="server" ID="btnMes" 
                                                class="mx-2 btn btn-sm btn-outline btn-whitelabel1" text="Mês" 
                                                onclick="btnMes_Click"/>
                                            <asp:Button runat="server" ID="btnAno" 
                                                class="mx-2 btn btn-sm btn-outline btn-whitelabel1" text="Ano" 
                                                onclick="btnAno_Click"/>

                                            <asp:Button runat="server" ID="btnLinha" 
                                                class="mx-2 btn btn-sm btn-outline btn-whitelabel1 float-right" text="Linha" 
                                                onclick="btnLinha_Click" />
                                            <asp:Button runat="server" ID="btnBarra" 
                                                class="mx-2 btn btn-sm btn-outline btn-whitelabel1 float-right" text="Barra" 
                                                onclick="btnBarra_Click"/>

                                        </p>
                                        <canvas id="myChartBarPadrao" style="width:100%;max-height:400px"></canvas>

                                        <script>
                                            var xValues = <%=PainelProgressao("L") %>;
                                            var yValues = <%=PainelProgressao("V") %>;
                                            var barColors = <%=PainelProgressao("C") %>;

                                            var barChartOptions = {
                                              responsive              : true,
                                              maintainAspectRatio     : false,
                                              datasetFill             : false
                                            }

                                            new Chart("myChartBarPadrao", {
                                                type: <%=TipoGrafico() %>,
                                                data: {
                                                    labels: xValues,
                                                    datasets: [{
                                                        backgroundColor: barColors,
                                                          backgroundColor     : <%=Cor() %>,
                                                          borderColor         : <%=Cor() %>,
                                                          pointRadius          : true,
                                                          pointColor          : <%=Cor() %>,
                                                          pointStrokeColor    : <%=Cor() %>,
                                                          pointHighlightFill  : <%=Cor() %>,
                                                          pointHighlightStroke: <%=Cor() %>,
                                                        data: yValues
                                                    }]
                                                },
                                                options: {
                                                    legend: { display: false },
                                                  responsive              : true,
                                                  maintainAspectRatio     : false,
                                                  datasetFill             : false,
                                                    title: {
                                                        display: false,
                                                        text: "Progressão de Vendas"
                                                    }
                                                }
                                            });
                                        </script>                                            
                                    </div>
                                </div>
                            </div>
                            <!-- Painel Formas de Pagamento -->
                            <div class="col-sm-4 col-xs-12 col-12 d-flex align-self-stretch" runat="server" id="dvFormasPagamento" visible="true">
                                <div class="card col-12 d-flex flex-column">
                                    <div class="card-body">
                                        <p><strong >FORMAS DE PAGAMENTO</strong></p>
                                        <div class="mw-100 mx-auto d-flex align-items-center align-middle">


                                            <canvas id="myChartDonutPadrao" class="align-middle" style="width:100%; margin: 0 auto;"></canvas>

                                            <script>
                                                var xValues = <%=PainelFormasPagamento("L") %>;
                                                var yValues = <%=PainelFormasPagamento("V") %>;
                                                var barColors = <%=PainelFormasPagamento("C") %>;

                                                new Chart("myChartDonutPadrao", {
                                                    type: "doughnut",
                                                    data: {
                                                        labels: xValues,
                                                        datasets: [{
                                                            backgroundColor: barColors,
                                                            data: yValues
                                                        }]
                                                    },
                                                    options: {
                                                        title: {
                                                            display: false,
                                                            text: "Formas de Pagamento"
                                                        }
                                                    }
                                                });
                                            </script>



                                        </div>
                                    </div>
                                </div>
                            </div>

                        </div>








                        <!-- Painel mix tpv -->

                        <div class="row">
                            <div class="col-sm-8 col-xs-12 col-12 d-flex align-self-stretch" runat="server" id="dvTPV" visible="true">
                                <div class="card col-12 d-flex flex-column">
                                    <div class="card-body">
                                        <div class="row form-control-sm text-whitelabel1"><small><b>MIX TPV</b></small></div>
                                        <div class="row">
                                            <asp:Repeater runat="server" ID="rptTPV">
                                                <ItemTemplate>



                                                      <div class="col-sm-3 col-6">
                                                        <div class="description-block border-right">
                                                          <span class="description-text form-control-sm"><small><%# DataBinder.Eval(Container.DataItem, "NOM_LEGENDA")%></small></span>
                                                          <h5 class="description-header">R$ <%# String.Format("{0:n2}",DataBinder.Eval(Container.DataItem, "NUM_VALOR_TOTAL"))%></h5>
                                                          <span class="description-text"><small><%# String.Format("{0:n2}",DataBinder.Eval(Container.DataItem, "NUM_PERCENTUAL"))%>%</small></span>
                                                          
                                                        </div>
                                                      </div>

                                                </ItemTemplate>
                                            </asp:Repeater>

                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="col-sm-4 col-xs-12 col-12 d-flex align-self-stretch" runat="server" id="dvCanaisVenda" visible="false" style="display:none;">
                                <div class="card col-12 d-flex flex-column">
                                    <div class="card-body">
                                    <p><strong >CANAIS DE VENDA</strong></p>

                                    <div class="progress-group">
                                        FÍSICO
                                        <span class="float-end"><b>20%</b></span>                                    
                                        <div class="progress progress-sm">
                                            <div class="progress-bar bg-warning" style="width: 20%"></div>
                                        </div>
                                    </div>                                    
                                    <div class="progress-group">
                                        ON-LINE
                                        <span class="float-end"><b>80%</b></span>                                    
                                        <div class="progress progress-sm">
                                            <div class="progress-bar bg-primary" style="width: 80%"></div>
                                        </div>
                                    </div>                                    
                                    </div>

                                </div>
                            </div>
                        </div>

                        <div class="row" runat="server" id="dvFaturamento" visible="true">
                            <div class="col-12 my-1">
                                <div class="card">
                                    <div class="card-body">
                                        <div class="row form-control-sm text-whitelabel1"><small><b>CADASTROS</b></small></div>
                                        <div class="row">
                                            <asp:Repeater runat="server" ID="rptCadastros">
                                                <ItemTemplate>
                                                      <div class="col-sm-2 col-6">
                                                        <div class="description-block border-right">
                                                          <h5 class="mb-0"><i class='fas <%# DataBinder.Eval(Container.DataItem, "NOM_LEGENDA_ICONE")%> text-whitelabel1'></i></h5>
                                                          <span class="description-text form-control-sm"><small><%# DataBinder.Eval(Container.DataItem, "NOM_LEGENDA_EXTENSO")%></small></span>
                                                          <h5 class="description-header"><%# String.Format("{0:n0}",DataBinder.Eval(Container.DataItem, "NUM_VALOR_TOTAL"))%></h5>
                                                        </div>
                                                      </div>
                                                </ItemTemplate>
                                            </asp:Repeater>

                                        </div>

                                    </div>                
                                </div>
                            </div>
                            <div class="col-12 my-1">
                                <div class="card">
                                    <div class="card-body">
                                        <div class="row form-control-sm text-whitelabel1"><small><b>VENDAS POR ADQUIRENTE</b></small></div>
                                        <div class="row">
                                            <asp:Repeater runat="server" ID="rpvFaturamento">
                                                <ItemTemplate>
                                                      <div class="col-sm-2 col-6">
                                                        <div class="description-block border-right">
                                                          <h5 class="mb-0"><i class='fas fa-dollar-sign text-whitelabel1'></i></h5>
                                                          <span class="description-text form-control-sm"><small><%# DataBinder.Eval(Container.DataItem, "NOM_LEGENDA")%></small></span>
                                                          <h5 class="description-header">R$ <%# String.Format("{0:n2}",DataBinder.Eval(Container.DataItem, "NUM_VALOR_TOTAL"))%></h5>
                                                        </div>
                                                      </div>
                                                </ItemTemplate>
                                            </asp:Repeater>
                                        </div>
                                    </div>
                                </div>
                            </div>


                        </div>
                        <div class="row" runat="server" id="dvBandeiras" visible="true">

                            <div class="col-12 my-1">
                                <div class="card">
                                    <div class="card-body">
                                        <div class="row form-control-sm text-whitelabel1"><small><b>VENDAS POR BANDEIRA</b></small></div>
                                        <div class="row">

                                            <asp:Repeater runat="server" ID="rptBandeiras">
                                                <ItemTemplate>

                                                      <div class="col-sm-2 col-6">
                                                        <div class="description-block border-right">
                                                          <h5><asp:Image runat="server" id="Image1" CssClass="img-fluid img-thumbnail mx-auto" style="max-width:50px;" ImageUrl='<%# DataBinder.Eval(Container.DataItem, "NOM_IMAGEM")%>'></asp:Image></h5>
                                                          <span class="description-text form-control-sm"><small><%# DataBinder.Eval(Container.DataItem, "NOM_LEGENDA")%></small></span><br />
                                                          <span class="description-text form-control-sm"><small>(<%# DataBinder.Eval(Container.DataItem, "NOM_OPERACAO")%>)</small></span>
                                                          <h5 class="description-header  form-control-sm">R$ <%# String.Format("{0:n2}",DataBinder.Eval(Container.DataItem, "NUM_VALOR_TOTAL"))%></h5>
                                                        </div>
                                                      </div>

                                                </ItemTemplate>
                                            </asp:Repeater>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>


                        <div class="row" runat="server" id="dvPlanos" visible="true">

                            <div class="col-12 my-1">
                                <div class="card">
                                    <div class="card-body">
                                        <div class="row form-control-sm text-whitelabel1"><small><b>PLANOS</b></small></div>
                                        <div class="row">

                                            <div class="col-md-3 col-xs-12 col-12 my-1">
                                                <div class="card">
                                                    <div class="card-body">
                                                        <div class='d-flex align-items-center'>
                                                            <div class='flex-grow-1'>
                                                                <p class='mb-0 fw-bold'>MAIS USADO</p>
                                                                <h5 class='mb-0 fw-bold'><asp:Label runat="server" ID="lblPlanoMaisUsado"></asp:Label></h5>
                                                            </div>
                                                            <div class='flex-shrink-0'>
                                                                <h3 class="mb-0"><i class="fas fa-arrow-up text-whitelabel1"></i></h3>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="col-md-3 col-xs-12 col-12 my-1">
                                                <div class="card">
                                                    <div class="card-body">
                                                        <div class='d-flex align-items-center'>
                                                            <div class='flex-grow-1'>
                                                                <p class='mb-0 fw-bold'>MAIS RENTÁVEL</p>
                                                                <h5 class='mb-0 fw-bold'></h5>
                                                            </div>
                                                            <div class='flex-shrink-0'>
                                                                <h3 class="mb-0"><i class="fas fa-arrow-up text-whitelabel1"></i></h3>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="col-md-3 col-xs-12 col-12 my-1">
                                                <div class="card">
                                                    <div class="card-body">
                                                        <div class='d-flex align-items-center'>
                                                            <div class='flex-grow-1'>
                                                                <p class='mb-0 fw-bold'>MENOS USADO</p>
                                                                <h5 class='mb-0 fw-bold'><asp:Label runat="server" ID="lblPlanoMenosUsado"></asp:Label></h5>
                                                            </div>
                                                            <div class='flex-shrink-0'>
                                                                <h3 class="mb-0"><i class="fas fa-arrow-down text-danger"></i></h3>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="col-md-3 col-xs-12 col-12 my-1">
                                                <div class="card">
                                                    <div class="card-body">
                                                        <div class='d-flex align-items-center'>
                                                            <div class='flex-grow-1'>
                                                                <p class='mb-0 fw-bold'>MENOS RENTÁVEL</p>
                                                                <h5 class='mb-0 fw-bold'></h5>
                                                            </div>
                                                            <div class='flex-shrink-0'>
                                                                <h3 class="mb-0"><i class="fas fa-arrow-down text-danger"></i></h3>
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


                        <div class="row" runat="server" id="dvComissoes" visible="true">
                            <div class="col-12 my-1">
                                <div class="card">
                                    <div class="card-body">
                                        <p class="text-whitelabel1" ><strong>REPASSES & COMISSÕES</strong> DO PERÍODO ESPECIFICADO</p>
                                        <div class="row">


                                            <!--<div class="col-md-3 col-xs-12 col-12 d-flex">
                                                <div class="card card-outline w-100 h-100 p-0">
                                                    <div class="card-body">
                                                        <strong >DÉBITO</strong><br></br>
                                                        <strong class="text-primary float-left">R$ 0,00</strong><br></br>
                                                        <strong >CRÉDITO</strong><br></br>
                                                        <strong class="text-primary float-left">R$ 0,00</strong>
                                                    </div>
                                                </div>
                                            </div>-->

                                                      <div class="col-sm-3 col-6" runat="server" id="dvComissaoTotal" visible="false">
                                                        <div class="description-block border-right">
                                                          <h5 class="text-warning"><i class="fas fa-coins"></i></h5>
                                                          <span class="description-text"><small>Comissão Total</small></span>
                                                          <h5 class="description-header">R$ <asp:Label runat="server" ID="lblComissaoTotal"></asp:Label> </h5>
                                                        </div>
                                                      </div>

                                                      <div class="col-sm-3 col-6" runat="server" id="dvComissaoRepasse" visible="false">
                                                        <div class="description-block border-right">
                                                          <h5 class="text-success"><i class="fas fa-comments-dollar"></i></h5>
                                                          <span class="description-text"><small><asp:Label runat="server" ID="lblTituloComissaoRepasse" Text="Comissão Repasse"></asp:Label></small></span>
                                                          <h5 class="description-header">R$ <asp:Label runat="server" ID="lblComissaoRepasse"></asp:Label></h5>
                                                        </div>
                                                      </div>

                                                      <div class="col-sm-3 col-6" runat="server" id="dvComissaoMKT" visible="false">
                                                        <div class="description-block border-right">
                                                          <h5 class="text-info"><i class="fas fa-store-alt"></i></h5>
                                                          <span class="description-text"><small>Comissão Marketplaces</small></span>
                                                          <h5 class="description-header">R$ <asp:Label runat="server" ID="lblComissaoMKT"></asp:Label></h5>
                                                        </div>
                                                      </div>
                                                      <div class="col-sm-3 col-6" runat="server" id="dvComissaoREP" visible="false">
                                                        <div class="description-block border-right">
                                                          <h5 class="text-info"><i class="fas fa-home"></i></h5>
                                                          <span class="description-text"><small><asp:Label runat="server" ID="lblTituloComissaoREP" Text="Comissão Representantes"></asp:Label></small></span>
                                                          <h5 class="description-header">R$ <asp:Label runat="server" ID="lblComissaoREP"></asp:Label></h5>
                                                        </div>
                                                      </div>



                                        </div>
                                    </div>
                                </div>
                            </div>

                        </div>
   
                        <div class="row" runat="server" id="dvRentabilidade" visible="false">
                            <div class="col-12 my-1">
                                <div class="card">
                                    <div class="card-body">
                                        <p><strong >MEDIA RENTABILIDADE</strong></p>
                                        <div class="row">

                                            <div class="col-md-4 col-xs-6 col-6">
                                                <div class="d-flex align-items-center justify-content-center">
                                                    <div>
                                                       <div class="progress-legacy green">
                                                         <span class="progress-legacy-left">
                                                                           <span class="progress-legacy-bar"></span>
                                                         </span>
                                                         <span class="progress-legacy-right">
                                                                           <span class="progress-legacy-bar"></span>
                                                         </span>
                                                         <div class="progress-legacy-value">10%</div>
                                                       </div>                                                    
                                                    </div>
                                                    <div class="d-flex flex-column justify-content-center align-items-center ms-2">
                                                        <p class="mb-0 fw-bold">PRESTAÇÃO</p>
                                                        <p class="mb-0">R$ 0,00</p>
                                                    </div>
                                                </div>
                                            </div>

                                            <div class="col-md-4 col-xs-6 col-6">
                                                <div class="d-flex align-items-center justify-content-center">
                                                    <div>

                                                       <div class="progress-legacy blue">
                                                         <span class="progress-legacy-left">
                                                                           <span class="progress-legacy-bar"></span>
                                                         </span>
                                                         <span class="progress-legacy-right">
                                                                           <span class="progress-legacy-bar"></span>
                                                         </span>
                                                         <div class="progress-legacy-value">10%</div>
                                                       </div>                                                    


                                                    </div>
                                                    <div class="d-flex flex-column justify-content-center align-items-center ms-2">
                                                        <p class="mb-0 fw-bold">INVESTIMENTOS</p>
                                                        <p class="mb-0">R$ 0,00</p>
                                                    </div>
                                                </div>
                                            </div>

                                            <div class="col-md-4 col-xs-6 col-6">
                                                <div class="d-flex align-items-center justify-content-center">
                                                    <div>

                                                       <div class="progress-legacy red">
                                                         <span class="progress-legacy-left">
                                                                           <span class="progress-legacy-bar"></span>
                                                         </span>
                                                         <span class="progress-legacy-right">
                                                                           <span class="progress-legacy-bar"></span>
                                                         </span>
                                                         <div class="progress-legacy-value">10%</div>
                                                       </div>                                                    


                                                    </div>
                                                    <div class="d-flex flex-column justify-content-center align-items-center ms-2">
                                                        <p class="mb-0 fw-bold">PROPRIEDADES</p>
                                                        <p class="mb-0">R$ 0,00</p>
                                                    </div>
                                                </div>
                                            </div>

                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>

                        <div class="row" runat="server" id="dvMapa" visible="true">

                            <div class="col-md-12 col-xs-12 col-12">
                                <div class="card">
                                    <div class="card-body">

                                        <div id="world-map-markers" class="h-100" style="overflow: hidden">
                                            <div class="container">
                                                <div class="map">Alternative content</div>
                                            </div>                            
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>



                        <div class="row" runat="server" id="dvTop10" visible="true">

                            <div class="col-md-12 col-xs-12 col-12">
                                <div class="card">
                                    <div class="card-body">

                                        <p><strong >
                                        TOP 10 RANKING VENDAS POR: 
                                        <asp:DropDownList runat="server" ID="ddlTop10Vendas" AutoPostBack="True" CssClass="form-control" onselectedindexchanged="ddlTop10Vendas_SelectedIndexChanged">
                                        </asp:DropDownList>
                                        </strong>
                                        </p>
                                        <div class="row col-12" style="overflow:auto; width: 100%;">
                                            <asp:Repeater runat="server" ID="rptTop10Vendas">
                                                <ItemTemplate>

                                                    <div class="col">
                                                        <div class="description-block border-right">
                                                            <h4><span class="badge bg-whitelabel1"><span><%# DataBinder.Eval(Container.DataItem, "NUM_LINHA")%></span></span></h4><br />
                                                            <span class="description-percentage text-whitelabel1"><%# String.Format("{0:c2}",DataBinder.Eval(Container.DataItem, "NUM_VALOR_BRUTO"))%></span><br />
                                                            <h6 class="description-header"><%# DataBinder.Eval(Container.DataItem, "NOM_CIDADE")%>/<%# DataBinder.Eval(Container.DataItem, "NOM_UF")%></h6>
                                                            <small><span class="description-text"><%# DataBinder.Eval(Container.DataItem, "NOM_RAZAOSOCIAL")%></span></small><br />
                                                        </div>
                                                    </div>                                        

                                                </ItemTemplate>
                                            </asp:Repeater>


                                        </div>
                                    </div>
                                </div>
                            </div>
                            
                        </div>



                        <div class="row" runat="server" id="dvMarketplace" visible="true">
                            <div class="col-12 my-1">
                                <div class="card">
                                    <div class="card-body">
                                        <!--<p><strong >MARKETPLACE</strong></p>-->

                                        <div class="row">
                                            <!--<asp:Repeater runat="server" ID="rptBanners">
                                                <ItemTemplate>
                                                    <div class="col-md-4 col-xs-12 col-12">
                                                        <a href="https://whts.co/erp?type=widget" target="_blank"><img class="img-fluid" src="../images/banner_1b.png" width="auto"/></a>
                                                    </div>
                                                </ItemTemplate>
                                            </asp:Repeater>-->


                                            <div class="col-md-4 col-xs-12 col-12">
                                                <a href="https://whts.co/erp?type=widget" target="_blank"><img class="img-fluid" src="../images/banner_1b.png" width="auto"/></a>
                                            </div>
                                            <div class="col-md-4 col-xs-12 col-12">
                                                <a href="https://whts.co/erp?type=widget" target="_blank"><img class="img-fluid" src="../images/banner_2b.png" width="auto"/></a>
                                            </div>
                                            <div class="col-md-4 col-xs-12 col-12">
                                                <a href="https://legacybank.readme.io/reference/introdu%C3%A7%C3%A3o" target="_blank"><img class="img-fluid" src="../images/banner_3b.png" width="auto"/></a>
                                            </div>
                                        </div>


                                        <div class="row" style="display:none;">

                                            <div class="col-md-4 col-xs-12 col-12">
                                                    <div class="card">
                                                    <div class="card-body">
                                                    <div class="row">
                                                    <img class="img-fluid" src="../images/banner_1.jpg" width="auto"></img>
                                                    </div>
                                                    <br />
                                                    <div class="row">
                                                        <div class="col-md-3 text-center">
                                                        <a class="btn btn-lg btn-app bg-primary mx-2"><i class="fas fa-comment-dollar fa-1x"></i> Cashback</a><br />
                                                        </div>

                                                        <div class="col-md-3 text-center">
                                                        <a class="btn btn-lg btn-app bg-primary mx-2"><i class="fas fa-box-open fa-1x"></i> Gift Card</a><br />
                                                        </div>

                                                        <div class="col-md-3 text-center">
                                                        <a class="btn btn-lg btn-app bg-primary mx-2"><i class="fas fa-battery-full fa-1x"></i> Recargas</a><br />
                                                        </div>

                                                        <div class="col-md-3 text-center">
                                                        <a class="btn btn-lg btn-app bg-primary mx-2"><i class="fas fa-handshake fa-1x"></i> Vantagens</a><br />
                                                        </div>
    
                                                    </div>
                                                    </div>
                                                    </div>
                                            </div>
                                            <div class="col-md-4 col-xs-12 col-12">
                                                    <div class="card">
                                                    <div class="card-body">
                                                    <div class="row">
                                                    <img class="img-fluid" src="../images/banner_2.jpg" width="auto"></img>
                                                    </div>
                                                    <br />
                                                    <div class="row">
                                                        <div class="col-md-3 text-center">
                                                        <a class="btn btn-lg btn-app bg-primary mx-2"><i class="fas fa-comment-dollar fa-1x"></i> Cashback</a><br />
                                                        </div>

                                                        <div class="col-md-3 text-center">
                                                        <a class="btn btn-lg btn-app bg-primary mx-2"><i class="fas fa-box-open fa-1x"></i> Gift Card</a><br />
                                                        </div>

                                                        <div class="col-md-3 text-center">
                                                        <a class="btn btn-lg btn-app bg-primary mx-2"><i class="fas fa-battery-full fa-1x"></i> Recargas</a><br />
                                                        </div>

                                                        <div class="col-md-3 text-center">
                                                        <a class="btn btn-lg btn-app bg-primary mx-2"><i class="fas fa-handshake fa-1x"></i> Vantagens</a><br />
                                                        </div>
    
                                                    </div>
                                                    </div>
                                                    </div>
                                            </div>
                                            <div class="col-md-4 col-xs-12 col-12">
                                                    <div class="card">
                                                    <div class="card-body">
                                                    <div class="row">
                                                    <img class="img-fluid" src="../images/banner_3.jpg" width="auto"></img>
                                                    </div>
                                                    <br />
                                                    <div class="row">
                                                        <div class="col-md-3 text-center">
                                                        <a class="btn btn-lg btn-app bg-primary mx-2"><i class="fas fa-comment-dollar fa-1x"></i> Cashback</a><br />
                                                        </div>

                                                        <div class="col-md-3 text-center">
                                                        <a class="btn btn-lg btn-app bg-primary mx-2"><i class="fas fa-box-open fa-1x"></i> Gift Card</a><br />
                                                        </div>

                                                        <div class="col-md-3 text-center">
                                                        <a class="btn btn-lg btn-app bg-primary mx-2"><i class="fas fa-battery-full fa-1x"></i> Recargas</a><br />
                                                        </div>

                                                        <div class="col-md-3 text-center">
                                                        <a class="btn btn-lg btn-app bg-primary mx-2"><i class="fas fa-handshake fa-1x"></i> Vantagens</a><br />
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


                        <div class="row" runat="server" id="dvExtrato" visible="true">
                            <div class="col-md-12 col-xs-12 col-12">
                    
                                <div class="card">
                                    <div class="card-body" style="overflow:auto; width: 100%; ">
                                        <p><strong>EXTRATO RESUMIDO</strong></p>

                                        <table id="tblConsulta" class="table table-striped dt-responsive nowrap">
                                            <thead>
                                            <tr>
                                                <th>Data</th>
                                                <th>Hora</th>
                                                <th>Tipo Operação</th>
                                                <th>Movimentação</th>
                                                <th>Valor do Lançamento</th>
                                                <th>Detalhe</th>

                                            </tr>
                                            </thead>
                                            <tbody>
                                            <asp:Repeater runat="server" ID="rptConsultaExtrato">
                                                <ItemTemplate>

                                                    <tr>
                                                        <td>
                                                        <small><%# String.Format("{0:dd/MM/yyyy}",DataBinder.Eval(Container.DataItem, "DTA_CRIADO"))%></small>
                                                        </td>
                                                        <td>
                                                        <small><%# String.Format("{0:HH:mm}",DataBinder.Eval(Container.DataItem, "DTA_CRIADO"))%></small>
                                                        </td>

                                                        <td>
                                                        <small><%# DataBinder.Eval(Container.DataItem, "NOM_FLG_RECURSO")%> <%# DataBinder.Eval(Container.DataItem, "NOM_FLG_TIPO")%></small>
                                                        </td>

                                                        <td>
                                                        <small><%# DataBinder.Eval(Container.DataItem, "DES_DESCRICAO")%></small>
                                                        </td>

                                                        <td>
                                                        <small class='text-<%# DataBinder.Eval(Container.DataItem, "NOM_FLG_STATUS_COR")%>'><%# String.Format("{0:n2}",DataBinder.Eval(Container.DataItem, "NUM_VALOR"))%></small>
                                                        </td>
                                                        <td>
                                                        <a href="#" data-toggle="modal" data-target='#modal-<%# DataBinder.Eval(Container.DataItem, "FLG_RECURSO")%><%# DataBinder.Eval(Container.DataItem, "COD_ID")%>'>
                                                            <i class="fas fa-chevron-right float-right"></i>
                                                        </a>

                                                        <!-- Modal Transferências -->
                                                        <div class="modal fade" id='modal-transfer<%# DataBinder.Eval(Container.DataItem, "COD_ID")%>'>
                                                        <div class="modal-dialog modal-lg">
                                                            <div class="modal-content bg-white">

                                                                <div class="modal-body text-black">
                                                                    <center>
                                                                    <h3 class="modal-title text-black"><b class="text-whitelabel1">DETALHE</b> DA TRANSFERÊNCIA</h3>
                                                                    <h6>Criada em <%# DataBinder.Eval(Container.DataItem, "DTA_CRIADO")%></h6>
                                                                    </center>
                                                                    <hr>
                                                                    <h4 class='text-<%# DataBinder.Eval(Container.DataItem, "NOM_FLG_STATUS_COR")%>'><%# DataBinder.Eval(Container.DataItem, "NOM_FLG_STATUS")%></h4>
                                                                    <h6><b>Valor da Transferência</b></h6>
                                                                    <h4>R$ <%# String.Format("{0:n2}",DataBinder.Eval(Container.DataItem, "NUM_VALOR"))%></h4>
                                                                    <h6><b>Tipo</b></h6>
                                                                    <h6><%# DataBinder.Eval(Container.DataItem, "NOM_FLG_RECURSO")%> <%# DataBinder.Eval(Container.DataItem, "NOM_FLG_TIPO")%></h6>
                                                                    <h6><b>Descrição</b></h6>
                                                                    <h6><%# DataBinder.Eval(Container.DataItem, "DES_DESCRICAO")%></h6>
                                                                    <h6><b>ID da Transferência</b></h6>
                                                                    <h6><%# DataBinder.Eval(Container.DataItem, "NOM_OBJECT_ID")%></h6>
                                                                    <h6><b>URI</b></h6>
                                                                    <h6><%# DataBinder.Eval(Container.DataItem, "NOM_URI").ToString().Substring(1,30)%></h6>
                                                                    <hr />
                                                                    <h4><b class="text-whitelabel1">Dados</b> do Favorecido</h4>
                                                                    <h6><b>Nome do Titular</b></h6>
                                                                    <h6><%# DataBinder.Eval(Container.DataItem, "NOM_TITULAR_BANCO")%></h6>
                                                                    <h6><b>CNPJ/CPF</b></h6>
                                                                    <h6><%# DataBinder.Eval(Container.DataItem, "NOM_CNPJCPF")%></h6>
                                                                    <h6><b>Banco</b></h6>
                                                                    <h6><%# DataBinder.Eval(Container.DataItem, "NOM_BANCO")%></h6>
                                                                    <h6><b>Agência</b></h6>
                                                                    <h6><%# DataBinder.Eval(Container.DataItem, "NOM_AGENCIA_BANCO")%></h6>
                                                                    <h6><b>Conta</b></h6>
                                                                    <h6><%# DataBinder.Eval(Container.DataItem, "NOM_CONTA_BANCO")%></h6>



                                                                </div>
                                                                <div class="modal-footer justify-content-between">
                                                                    <button type="button" class="btn btn-whitelabel1" data-dismiss="modal">Fechar</button>
                                                                </div>
                                                            </div>
                                                        </div>
                                                        </div>

                                                        <!-- Modal de Recebimentos -->

                                                        <div class="modal fade" id='modal-receivable<%# DataBinder.Eval(Container.DataItem, "COD_ID")%>'>
                                                        <div class="modal-dialog modal-lg">
                                                            <div class="modal-content bg-white">

                                                                <div class="modal-body text-black">
                                                                    <center>
                                                                    <h3 class="modal-title text-black"><b class="text-whitelabel1">DETALHE</b> DO RECEBIMENTO</h3>
                                                                    <h6>Criado em <%# DataBinder.Eval(Container.DataItem, "DTA_CRIADO")%></h6>
                                                                    </center>
                                                                    <hr>
                                                                    <h4 class='text-<%# DataBinder.Eval(Container.DataItem, "NOM_FLG_STATUS_COR")%>'><%# DataBinder.Eval(Container.DataItem, "NOM_FLG_STATUS")%></h4>
                                                                    <h6><b>Valor do Recebimento</b></h6>
                                                                    <h4>R$ <%# String.Format("{0:n2}",DataBinder.Eval(Container.DataItem, "NUM_VALOR"))%></h4>
                                                                    <h6><b>Tipo</b></h6>
                                                                    <h6><%# DataBinder.Eval(Container.DataItem, "NOM_FLG_RECURSO")%> <%# DataBinder.Eval(Container.DataItem, "NOM_FLG_TIPO")%></h6>
                                                                    <h6><b>Descrição</b></h6>
                                                                    <h6><%# DataBinder.Eval(Container.DataItem, "DES_DESCRICAO")%></h6>
                                                                    <h6><b>ID do Recebimento</b></h6>
                                                                    <h6><%# DataBinder.Eval(Container.DataItem, "NOM_OBJECT_ID")%></h6>
                                                                    <h6><b>URI</b></h6>
                                                                    <h6><%# DataBinder.Eval(Container.DataItem, "NOM_URI").ToString().Substring(1,30)%></h6>
                                                                    <!--
                                                                    <hr />
                                                                    <h4><b class="text-whitelabel1">Dados</b> do Favorecido</h4>
                                                                    <h6><b>Nome do Titular</b></h6>
                                                                    <h6><%# DataBinder.Eval(Container.DataItem, "NOM_TITULAR_BANCO")%></h6>
                                                                    <h6><b>CNPJ/CPF</b></h6>
                                                                    <h6><%# DataBinder.Eval(Container.DataItem, "NOM_CNPJCPF")%></h6>
                                                                    <h6><b>Banco</b></h6>
                                                                    <h6><%# DataBinder.Eval(Container.DataItem, "NOM_BANCO")%></h6>
                                                                    <h6><b>Agência</b></h6>
                                                                    <h6><%# DataBinder.Eval(Container.DataItem, "NOM_AGENCIA_BANCO")%></h6>
                                                                    <h6><b>Conta</b></h6>
                                                                    <h6><%# DataBinder.Eval(Container.DataItem, "NOM_CONTA_BANCO")%></h6>
                                                                    -->


                                                                </div>
                                                                <div class="modal-footer justify-content-between">
                                                                    <button type="button" class="btn btn-whitelabel1" data-dismiss="modal">Fechar</button>
                                                                </div>
                                                            </div>
                                                        </div>
                                                        </div>

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

                        <div class="row"  runat="server" id="dvUltimasVendas" visible="true">
                            <div class="col-md-12 col-xs-12 col-12">
                                <div class="card">
                                    <div class="card-body" style="overflow:auto; width: 100%;">
                                            <p><strong>ÚLTIMAS VENDAS</strong></p>

                                            <asp:GridView ID="rptConsultaResumo" runat="server" 
                                                AutoGenerateColumns="false" BorderStyle="None" 
                                                CssClass="table table-striped nowrap table-bordered table-hover">
                                                <AlternatingRowStyle BorderStyle="None" />
                                                <RowStyle BorderStyle="None" />
                                                <HeaderStyle BorderWidth="1" BorderStyle="Solid" />
                                                <Columns>

                                                  <asp:TemplateField HeaderText="Data">
                                                    <ItemTemplate>
                                                        <small><%# String.Format("{0:dd/MM/yyyy}",DataBinder.Eval(Container.DataItem, "DTA_DATA"))%> <%# String.Format("{0:HH:mm}",DataBinder.Eval(Container.DataItem, "DTA_DATA"))%></small>
                                                    </ItemTemplate>
                                                  </asp:TemplateField>
                                                  <asp:TemplateField HeaderText="ID">
                                                    <ItemTemplate>
                                                    <small><strong><%# DataBinder.Eval(Container.DataItem, "COD_ID")%></strong></small>
                                                    </ItemTemplate>
                                                  </asp:TemplateField>

                                                  <asp:TemplateField HeaderText="Adquirente">
                                                    <ItemTemplate>
                                                        <small><strong><%# DataBinder.Eval(Container.DataItem, "NOM_ADQUIRENTE")%></strong></small>
                                                    </ItemTemplate>
                                                  </asp:TemplateField>

                                                  <asp:TemplateField HeaderText="Code">
                                                    <ItemTemplate>
                                                        <p style="max-width: 7ch;  overflow: hidden;  text-overflow: ellipsis;  white-space: nowrap;"><small><strong><%# DataBinder.Eval(Container.DataItem, "NOM_CODE")%></strong></small></p>
                                                    </ItemTemplate>
                                                  </asp:TemplateField>

                                                  <asp:TemplateField HeaderText="Estabelecimento">
                                                    <ItemTemplate>
                                                        <small><%# DataBinder.Eval(Container.DataItem, "NOM_RAZAOSOCIAL")%></small>
                                                    </ItemTemplate>
                                                  </asp:TemplateField>
                                                  <asp:TemplateField HeaderText="Valor Venda">
                                                    <ItemTemplate>
                                                        <small><%# String.Format("{0:c2}",DataBinder.Eval(Container.DataItem, "NUM_VALOR_BRUTO"))%></small>
                                                    </ItemTemplate>
                                                  </asp:TemplateField>


                                                  <asp:TemplateField HeaderText="Total Taxas">
                                                    <ItemTemplate>
                                                        <small><%# String.Format("{0:c2}",DataBinder.Eval(Container.DataItem, "NUM_VALOR_TOTAL_TAXAS"))%></small>
                                                    </ItemTemplate>
                                                  </asp:TemplateField>

                                                  <asp:TemplateField HeaderText="Valor Líquido">
                                                    <ItemTemplate>
                                                        <small><%# String.Format("{0:c2}",DataBinder.Eval(Container.DataItem, "NUM_VALOR_LIQUIDO"))%></small>
                                                    </ItemTemplate>
                                                  </asp:TemplateField>

                                                  <asp:TemplateField HeaderText="No. Parcelas">
                                                    <ItemTemplate>
                                                        <small><%# DataBinder.Eval(Container.DataItem, "NUM_PARCELAS")%></small>
                                                    </ItemTemplate>
                                                  </asp:TemplateField>
                                                  <asp:TemplateField HeaderText="Plano">
                                                    <ItemTemplate>
                                                        <small><%# DataBinder.Eval(Container.DataItem, "NOM_TITULO_PLANO")%></small>
                                                    </ItemTemplate>
                                                  </asp:TemplateField>
                                                  <asp:TemplateField HeaderText="Tipo">
                                                    <ItemTemplate>
                                                        <small><%# DataBinder.Eval(Container.DataItem, "DES_DESCRICAO")%></small>
                                                    </ItemTemplate>
                                                  </asp:TemplateField>
                                                  <asp:TemplateField HeaderText="Bandeira">
                                                    <ItemTemplate>
                                                        <small><%# DataBinder.Eval(Container.DataItem, "NOM_OPERACAO")%></small>
                                                    </ItemTemplate>
                                                  </asp:TemplateField>

                                                  <asp:TemplateField HeaderText="Status">
                                                    <ItemTemplate>
                                                        <small><%# DataBinder.Eval(Container.DataItem, "NOM_STATUS")%></small>
                                                    </ItemTemplate>
                                                  </asp:TemplateField>
                                                  <asp:TemplateField HeaderText="Serial">
                                                    <ItemTemplate>
                                                        <small><%# DataBinder.Eval(Container.DataItem, "NUM_DEVICE_SERIAL")%></small>
                                                    </ItemTemplate>
                                                  </asp:TemplateField>
                                                  <asp:TemplateField HeaderText="Canal">
                                                    <ItemTemplate>
                                                        <small><%# DataBinder.Eval(Container.DataItem, "NOM_FLG_PRESENCIAL_ONLINE")%></small>
                                                    </ItemTemplate>
                                                  </asp:TemplateField>
                                                  <asp:TemplateField HeaderText="Detalhe">
                                                    <ItemTemplate>
                                                        <a data-toggle="modal" href='#modal-detalhe<%# DataBinder.Eval(Container.DataItem, "COD_ID")%>' data-backdrop="static">                                                                
                                                            <i class="fas fa-chevron-right float-right"></i>
                                                        </a>

                                                        <div class="modal" id="modal-detalhe<%# DataBinder.Eval(Container.DataItem, "COD_ID")%>" data-backdrop="static">
                                                            <div class="modal-dialog">
                                                                <div class="modal-content">
                                                                    <div class="container"></div>
                                                                    <div class="modal-body text-black">

                                                                        <center>
                                                                        <asp:TextBox runat="server" ID="txtid" Text='<%# DataBinder.Eval(Container.DataItem, "COD_ID")%>' Visible="false"></asp:TextBox>
                                                                        <asp:TextBox runat="server" ID="txtidtransferencia" Text='<%# DataBinder.Eval(Container.DataItem, "NOM_CODE")%>' Visible="false"></asp:TextBox>
                                                                        <h3 class="modal-title text-black"><b class="text-whitelabel1">DETALHE</b> DA VENDA</h3>
                                                                        <h6>Criada em <%# DataBinder.Eval(Container.DataItem, "DTA_DATA")%></h6>
                                                                        </center>


                                                                    </div>
                                                                    <div class="modal-footer">
                                                                        <a href="#" data-dismiss="modal" class="btn btn-whitelabel1 btn-sm">Fechar</a>
                                                                    </div>


                                                                </div>
                                                            </div>
                                                        </div>
                                                    </ItemTemplate>
                                                  </asp:TemplateField>


                                                </Columns>
                                                <PagerStyle BorderStyle="None" />
                                                <RowStyle BorderStyle="None" />
                                            </asp:GridView>  

                                    </div>                            
                                </div>
                            </div>
                        </div>


                <!-- SETOR DO ADMINISTRADOR *************************************************************************************************************************************************** -->
                    <div class="container-fluid" runat="server" id="ctnAdministrador">

                    </div>

                <!-- FIM ADMINISTRADOR ********************************************************************************************************************************************************** -->

                <!-- SETOR DO MARKETPLACE *************************************************************************************************************************************************** -->
                    <div class="container-fluid" runat="server" id="ctnMarketplace">


                    </div>

                <!-- FIM MARKETPLACE ********************************************************************************************************************************************************** -->




                <!-- SETOR DO REPRESENTANTE *************************************************************************************************************************************************** -->
                    <div class="container-fluid" runat="server" id="ctnRepresentante">


                    </div>

                <!-- FIM REPRESENTANTE ********************************************************************************************************************************************************** -->




                <!-- SETOR DO ESTABELECIMENTO *************************************************************************************************************************************************** -->

                    <div class="container-fluid" runat="server" id="cntEstabelecimento">
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
                            <center>
                            <p>Para confirmar a operação, digite abaixo o código de confirmação que você recebeu no e-mail do administrador da conta: <asp:Label runat="server" ID="lblEmailConfirmacao"></asp:Label></p>
                            </center>
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
                            <asp:Button runat="server" ID="btnConfirmar2FA" CssClass="btn btn-primary" Text="Confirmar" onclick="btnConfirmar2FA_Click" />
                        </div>
                        </div>
                    </div>
                    </div>

                    <div class="modal fade" id="mdVersoes">
                    <div class="modal-dialog modal-sm">
                        <div class="modal-content">
                        <div class="modal-header">
                            <h6 class="modal-title"><b>Novas implementações</b></h6>
                            <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                            <span aria-hidden="true">&times;</span>
                            </button>
                        </div>
                        <div class="modal-body">


                        </div>
                        <div class="modal-footer justify-content-between">
                            <button type="button" class="btn btn-danger" data-dismiss="modal">Fechar</button>
                        </div>
                        </div>
                    </div>
                    </div>


                </section>
            </div>
            <PORTAL:PAGEBOTTOM id="Pageheader3" title="Site Directory" runat="server" ModuleSource="rodapepadrao.ascx"></PORTAL:PAGEBOTTOM><!-- Fim Rodapé da Pagina -->
    
        </div>
    </form>





<script type="text/javascript" src="../dist/js/demo.js"></script>

<script type="text/javascript" src="../plugins/jquery-mousewheel/jquery.mousewheel.js"></script>
<script type="text/javascript" src="../plugins/raphael/raphael.min.js"></script>
<script type="text/javascript" src="../plugins/jquery-mapael/jquery.mapael.min.js"></script>
<script type="text/javascript" src="../plugins/jquery-mapael/maps/world_countries.min.js"></script>
<script type="text/javascript" src="../plugins/jquery-mapael/maps/brazil.min.js"></script>

<script type="text/javascript" src="../plugins/chart.js/Chart.js"></script>


<script type="text/javascript" src="../plugins/datatables/jquery.dataTables.js"></script>
<script type="text/javascript" src="../plugins/datatables-bs4/js/dataTables.bootstrap4.js"></script>

<script type="text/javascript" src="../plugins/datatables-responsive/js/dataTables.responsive.min.js"></script>
<script type="text/javascript" src="../plugins/datatables-responsive/js/responsive.bootstrap4.min.js"></script>
<script type="text/javascript" src="../plugins/datatables-buttons/js/dataTables.buttons.min.js"></script>
<script type="text/javascript" src="../plugins/datatables-buttons/js/buttons.bootstrap4.min.js"></script>
<script type="text/javascript" src="../plugins/jszip/jszip.min.js"></script>
<script type="text/javascript" src="../plugins/pdfmake/pdfmake.min.js"></script>
<script type="text/javascript" src="../plugins/pdfmake/vfs_fonts.js"></script>
<script type="text/javascript" src="../plugins/datatables-buttons/js/buttons.html5.min.js"></script>
<script type="text/javascript" src="../plugins/datatables-buttons/js/buttons.print.min.js"></script>
<script type="text/javascript" src="../plugins/datatables-buttons/js/buttons.colVis.min.js"></script>

<script type="text/javascript">
    $(function () {
        $('[id$=rptConsultaResumo]').prepend($("<thead></thead>").append($('[id$=gvConsulta]').find("tr:first"))).DataTable({
            "responsive": false,
            "sPaginationType": "full_numbers",
            "oLanguage": {
                "sUrl": '//cdn.datatables.net/plug-ins/2.0.6/i18n/pt-BR.json'
            },
            "paging": true,
            "lengthChange": false,
            "searching": true,
            "ordering": true,
            "info": true,
            "autoWidth": false,
            buttons: ['copy', 'excel', 'pdf', 'csv', 'print'],
            initComplete: function () {
                this.api().buttons().container()
                //.appendTo( $ ('#table_id_wrapper .col-md-6:eq(0)', this.api().table (). container ()));
                //.appendTo( $('#table_id_wrapper .col-md-6:eq(0)' ) );
            .appendTo($('.col-md-6:eq(0)', this.api().table().container()));

            }
        });
    });
</script>

<script type="text/javascript">

$(document).ready(function () {
    var table = $('#tabConsultaTransacoesLicenciado').DataTable({
        order: [[0, 'desc']],
        "responsive": false,
        "sPaginationType": "full_numbers",
        "oLanguage": {
            "sUrl": '//cdn.datatables.net/plug-ins/2.0.6/i18n/pt-BR.json'
        },
        "paging": true,
        "lengthChange": true,
        "searching": true,
        "ordering": true,
        "info": true,
        "autoWidth": true,
        buttons: [
                {
                    extend: 'copy',
                    exportOptions: {
                        columns: ':visible'
                    }
                },        
                {
                    extend: 'excel',
                    exportOptions: {
                        columns: ':visible'
                    }
                },        
                {
                    extend: 'pdf',
                       text: 'Export PDF',
                       orientation: 'landscape',
                       pageSize: 'LEGAL',
                       customize: function ( doc ) {
                         doc.pageMargins = [10,10,10,10]
                     },
                     exportOptions: {
                        columns: ':visible'
                    }
                },
                {
                    extend: 'csv',
                    exportOptions: {
                        columns: ':visible'
                    }
                },

                {
                    extend: 'print',
                    exportOptions: {
                        columns: ':visible'
                    }
                },
                'colvis'
        ],
        initComplete: function () {
            this.api().buttons().container()
            //.appendTo( $ ('#table_id_wrapper .col-md-6:eq(0)', this.api().table (). container ()));
            //.appendTo( $('#table_id_wrapper .col-md-6:eq(0)' ) );
        .appendTo($('.col-md-6:eq(0)', this.api().table().container()));

        }

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


<script type="text/javascript">

    $(document).ready(function () {
        var table = $('#tabConsultaTransacoesAdquirente').DataTable({
            order: [[0, 'desc']],
            "responsive": false,
            "sPaginationType": "full_numbers",
            "oLanguage": {
                "sUrl": '//cdn.datatables.net/plug-ins/2.0.6/i18n/pt-BR.json'
            },
            "paging": true,
            "lengthChange": true,
            "searching": true,
            "ordering": true,
            "info": true,
            "autoWidth": true,
            buttons: [
                {
                    extend: 'copy',
                    exportOptions: {
                        columns: ':visible'
                    }
                },
                {
                    extend: 'excel',
                    exportOptions: {
                        columns: ':visible'
                    }
                },
                {
                    extend: 'pdf',
                    text: 'Export PDF',
                    orientation: 'landscape',
                    pageSize: 'LEGAL',
                    customize: function (doc) {
                        doc.pageMargins = [10, 10, 10, 10]
                    },
                    exportOptions: {
                        columns: ':visible'
                    }
                },
                {
                    extend: 'csv',
                    exportOptions: {
                        columns: ':visible'
                    }
                },

                {
                    extend: 'print',
                    exportOptions: {
                        columns: ':visible'
                    }
                },
                'colvis'
        ],
            initComplete: function () {
                this.api().buttons().container()
                //.appendTo( $ ('#table_id_wrapper .col-md-6:eq(0)', this.api().table (). container ()));
                //.appendTo( $('#table_id_wrapper .col-md-6:eq(0)' ) );
        .appendTo($('.col-md-6:eq(0)', this.api().table().container()));

            }

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


<script type="text/javascript">

    $(document).ready(function () {
        var table = $('#tabConsultaEstabelecimentosLicenciado').DataTable({
            order: [[0, 'desc']],
            "responsive": false,
            "sPaginationType": "full_numbers",
            "oLanguage": {
                "sUrl": '//cdn.datatables.net/plug-ins/2.0.6/i18n/pt-BR.json'
            },
            "paging": true,
            "lengthChange": true,
            "searching": true,
            "ordering": true,
            "info": true,
            "autoWidth": true,
            buttons: [
                {
                    extend: 'copy',
                    exportOptions: {
                        columns: ':visible'
                    }
                },
                {
                    extend: 'excel',
                    exportOptions: {
                        columns: ':visible'
                    }
                },
                {
                    extend: 'pdf',
                    text: 'Export PDF',
                    orientation: 'landscape',
                    pageSize: 'LEGAL',
                    customize: function (doc) {
                        doc.pageMargins = [10, 10, 10, 10]
                    },
                    exportOptions: {
                        columns: ':visible'
                    }
                },
                {
                    extend: 'csv',
                    exportOptions: {
                        columns: ':visible'
                    }
                },

                {
                    extend: 'print',
                    exportOptions: {
                        columns: ':visible'
                    }
                },
                'colvis'
        ],
            initComplete: function () {
                this.api().buttons().container()
                //.appendTo( $ ('#table_id_wrapper .col-md-6:eq(0)', this.api().table (). container ()));
                //.appendTo( $('#table_id_wrapper .col-md-6:eq(0)' ) );
        .appendTo($('.col-md-6:eq(0)', this.api().table().container()));

            }

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
  $(function () {
    /* ChartJS
     * -------
     * Here we will create a few charts using ChartJS
     */

    //--------------
    //- AREA CHART -
    //--------------

    // Get context with jQuery - using jQuery's .get() method.
    var areaChartCanvas = $('#areaChart').get(0).getContext('2d')

    var areaChartData = {
      labels  : ['January', 'February', 'March', 'April', 'May', 'June', 'July'],
      datasets: [
        {
          label               : 'Digital Goods',
          backgroundColor     : 'rgba(60,141,188,0.9)',
          borderColor         : 'rgba(60,141,188,0.8)',
          pointRadius          : false,
          pointColor          : '#3b8bba',
          pointStrokeColor    : 'rgba(60,141,188,1)',
          pointHighlightFill  : '#fff',
          pointHighlightStroke: 'rgba(60,141,188,1)',
          data                : [28, 48, 40, 19, 86, 27, 90]
        },
        {
          label               : 'Electronics',
          backgroundColor     : 'rgba(210, 214, 222, 1)',
          borderColor         : 'rgba(210, 214, 222, 1)',
          pointRadius         : false,
          pointColor          : 'rgba(210, 214, 222, 1)',
          pointStrokeColor    : '#c1c7d1',
          pointHighlightFill  : '#fff',
          pointHighlightStroke: 'rgba(220,220,220,1)',
          data                : [65, 59, 80, 81, 56, 55, 40]
        },
      ]
    }

    var areaChartOptions = {
      maintainAspectRatio : false,
      responsive : true,
      legend: {
        display: false
      },
      scales: {
        xAxes: [{
          gridLines : {
            display : false,
          }
        }],
        yAxes: [{
          gridLines : {
            display : false,
          }
        }]
      }
    }

    // This will get the first returned node in the jQuery collection.
    var areaChart       = new Chart(areaChartCanvas, { 
      type: 'line',
      data: areaChartData, 
      options: areaChartOptions
    })

    //-------------
    //- LINE CHART -
    //--------------
    var lineChartCanvas = $('#lineChart').get(0).getContext('2d')
    var lineChartOptions = jQuery.extend(true, {}, areaChartOptions)
    var lineChartData = jQuery.extend(true, {}, areaChartData)
    lineChartData.datasets[0].fill = false;
    lineChartData.datasets[1].fill = false;
    lineChartOptions.datasetFill = false

    var lineChart = new Chart(lineChartCanvas, { 
      type: 'line',
      data: lineChartData, 
      options: lineChartOptions
    })

    //-------------
    //- DONUT CHART -
    //-------------
    // Get context with jQuery - using jQuery's .get() method.
    var donutChartCanvas = $('#donutChart').get(0).getContext('2d')
    var donutData        = {
      labels: [
          'Chrome', 
          'IE',
          'FireFox', 
          'Safari', 
          'Opera', 
          'Navigator', 
      ],
      datasets: [
        {
          data: [700,500,400,600,300,100],
          backgroundColor : ['#f56954', '#00a65a', '#f39c12', '#00c0ef', '#3c8dbc', '#d2d6de'],
        }
      ]
    }
    var donutOptions     = {
      maintainAspectRatio : false,
      responsive : true,
    }
    //Create pie or douhnut chart
    // You can switch between pie and douhnut using the method below.
    var donutChart = new Chart(donutChartCanvas, {
      type: 'doughnut',
      data: donutData,
      options: donutOptions      
    })

    //-------------
    //- PIE CHART -
    //-------------
    // Get context with jQuery - using jQuery's .get() method.
    var pieChartCanvas = $('#pieChart').get(0).getContext('2d')
    var pieData        = donutData;
    var pieOptions     = {
      maintainAspectRatio : false,
      responsive : true,
    }
    //Create pie or douhnut chart
    // You can switch between pie and douhnut using the method below.
    var pieChart = new Chart(pieChartCanvas, {
      type: 'pie',
      data: pieData,
      options: pieOptions      
    })

    //-------------
    //- BAR CHART -
    //-------------
    var barChartCanvas = $('#barChart').get(0).getContext('2d')
    var barChartData = jQuery.extend(true, {}, areaChartData)
    var temp0 = areaChartData.datasets[0]
    var temp1 = areaChartData.datasets[1]
    barChartData.datasets[0] = temp1
    barChartData.datasets[1] = temp0

    var barChartOptions = {
      responsive              : true,
      maintainAspectRatio     : false,
      datasetFill             : false
    }

    var barChart = new Chart(barChartCanvas, {
      type: 'bar', 
      data: barChartData,
      options: barChartOptions
    })

    //---------------------
    //- STACKED BAR CHART -
    //---------------------
    var stackedBarChartCanvas = $('#stackedBarChart').get(0).getContext('2d')
    var stackedBarChartData = jQuery.extend(true, {}, barChartData)

    var stackedBarChartOptions = {
      responsive              : true,
      maintainAspectRatio     : false,
      scales: {
        xAxes: [{
          stacked: true,
        }],
        yAxes: [{
          stacked: true
        }]
      }
    }

    var stackedBarChart = new Chart(stackedBarChartCanvas, {
      type: 'bar', 
      data: stackedBarChartData,
      options: stackedBarChartOptions
    })
  })
</script>



<script>
$(function () {
    $(".container").mapael({
        map: {
            name: "world_countries"
            , zoom: {
                enabled: true,
                maxLevel: 20
            }
            , defaultPlot: {
                attrs: {
                    fill: "#1f5050"
                    , opacity: 0.6
                }
                , attrsHover: {
                    opacity: 1
                }
                , text: {
                    attrs: {
                        fill: "#153642"
                    }
                    , attrsHover: {
                        fill: "#000"
                    }
                }
            }

            , defaultLink: {
                factor: 0.4
                ,attrs: {
                    stroke: "#000000"
                    , opacity: 0.6
                }
                , attrsHover: {
                    fill: "#000",
                    stroke: "#000000"
                }
            }

            , defaultArea: {
                attrs: {
                    fill: "#d9fff2"
                    , stroke: "#1f5050"
                }
                , attrsHover: {
                    fill: "#1f5050"
                }
                , text: {
                    attrs: {
                        fill: "#153642"
                    }
                    , attrsHover: {
                        fill: "#000"
                    }
                }
            }
        },

        plots: {
            <%=PainelMapa("E") %>
        },

    });
});
</script>



</body>
</html>
