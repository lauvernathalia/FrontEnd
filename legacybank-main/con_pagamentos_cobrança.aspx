<%@ Page Language="C#" AutoEventWireup="true" CodeFile="con_pagamentos_cobrança.aspx.cs" Inherits="con_pagamentos_cobrança" MaintainScrollPositionOnPostback="true" Async="true" %>

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
                    <div class="container-fluid" >

                        <div class="row mb-2">
                          <div class="col-sm-12">
                            <ol class="breadcrumb">
                              <li class="breadcrumb-item"><a href="#">Conta de Pagamento</a></li>
                              <li class="breadcrumb-item active">Cobranças</li>
                            </ol>
                          </div>
                        </div>

                        <!-- Início  ***************************************************************************************************************************************************** -->
                        <div class="row">
                          <div class="col-12">
                            <div class="card">

                              <div class="card-header bg-whitelabel1">
                                <h3 class="card-title">Cobranças</h3>
                              </div>
                              <div class="card-body">

                                <h3><b>Saldo R$ </b><asp:Label runat="server" ID="lblSaldo" Text="0,00"></asp:Label></h3>

                                <h2>Cobranças</h2>
                                <h6>O que deseja fazer?</h6>
                                <div class="card card-outline col-12">
                                    <div class="card-body">
                                        <div class="row">

                                            <asp:LinkButton runat="server" ID="btnGestaoCobranca" 
                                                CssClass="btn btn-app text-whitelabel1" onclick="btnGestaoCobranca_Click"><i class="fas fa-tasks text-wrap"></i>Gestão Cobranças</asp:LinkButton>
                                            
                                            <asp:LinkButton runat="server" ID="btnSimulador" 
                                                CssClass="btn btn-app text-whitelabel1" onclick="btnSimulador_Click"><i class="fas fa-calculator text-wrap"></i>Simulador</asp:LinkButton>

                                            <asp:LinkButton runat="server" ID="btnBoletoAvulso" 
                                                CssClass="btn btn-app text-whitelabel1" onclick="btnBoletoAvulso_Click"><i class="fas fa-barcode text-wrap"></i>Boleto Avulso</asp:LinkButton>

                                            <asp:LinkButton runat="server" ID="btnReceberQRCode" 
                                                CssClass="btn btn-app text-whitelabel1" onclick="btnReceberQRCode_Click"><i class="fas fa-qrcode"></i>Receber por QRCode</asp:LinkButton>

                                            <asp:LinkButton runat="server" ID="btnLinkPagamento" 
                                                CssClass="btn btn-app text-whitelabel1" onclick="btnLinkPagamento_Click"><i class="fas fa-shopping-cart"></i>Link Pagamento</asp:LinkButton>

                                            <asp:LinkButton runat="server" ID="btnVendaDigitada" 
                                                CssClass="btn btn-app text-whitelabel1" onclick="btnVendaDigitada_Click"><i class="fas fa-calculator text-wrap"></i>Venda Digitada</asp:LinkButton>

                                            <asp:LinkButton runat="server" ID="btnCarne" 
                                                CssClass="btn btn-app text-whitelabel1" onclick="btnCarne_Click"><i class="fas fa-file-invoice"></i>Carnê</asp:LinkButton>

                                            <asp:LinkButton runat="server" ID="btnAssinatura" 
                                                CssClass="btn btn-app text-whitelabel1" onclick="btnAssinatura_Click"><i class="fas fa-calendar-check"></i>Assinatura Recorrente</asp:LinkButton>



                                            <asp:Button ID="btnPostback" runat="server" Visible="false" OnClick="btnPostBack_Click" />
                                            <script type="text/javascript">
                                                function PostBackOnMainPage(){
                                                <%=GetPostBackScript()%>
                                                }
                                            </script>


                                        </div>
                                        <div class="row" runat="server" id="divResposta" visible="false">
                                            <div class="col-sm-12">
                                                <div class="form-group">
                                                    <asp:TextBox ID="txtResposta" runat="server" CssClass="form-control" TextMode="MultiLine" Rows="10" ></asp:TextBox>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>

                                <!-- Gestão de Cobranças -->
                                <!-- ********************************************************************************************************************************************************** -->
                                <div class="card card-outline col-12" runat="server" id="divGestao" visible="false">
                                    <div class="card-body">
                                        <div class="row">
                                            <div class="col-sm-12">
                                                <h5 class="text-whitelabel1">Gestão das Cobranças</h5>
                                            </div>
                                        </div>

                                        <div class="form-group row col-12">
                                            <label class="col-2 col-form-label">Período de</label>
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
                                        </div>                                            

                                        <div class="row">
                                            <div class="col-12">
                                                <asp:Button runat="server" ID="btnPesquisarGestao" Text="Pesquisar" 
                                                    CssClass="btn btn-whitelabel1" onclick="btnPesquisarGestao_Click" />
                                            </div>
                                        </div>


                                        <div class="row"  style="overflow:auto; width: 100%;">
                                            <div class="col-12">
                                                <table id="tabConsulta" class="table table-bordered table-hover">
                                                    <thead>
                                                        <tr>
                                                            <th>ID</th>
                                                            <th style="display:none;">Ações</th>
                                                            <th>Data</th>
                                                            <th>Tipo</th>
                                                            <th>Status</th>
                                                            <th></th>
                                                            <th>Cliente</th>
                                                            <th>CNPJ/CPF</th>
                                                            <th>Vencimento</th>
                                                            <th>Valor</th>
                                                            <th>Descrição</th>
                                                            <th>Cancelar</th>
                                                            <th>Link Externo</th>
                                                        </tr>
                                                    </thead>
                                                    <tbody>
                                                        <asp:Repeater runat="server" ID="rptConsulta" OnItemCommand="rptConsulta_OnItemCommand">
                                                            <ItemTemplate>
                                                                <tr>
                                                                    <td>
                                                                        <small><%# DataBinder.Eval(Container.DataItem, "COD_ID")%></small>
                                                                        <asp:TextBox runat="server" ID="txtid" Text='<%# DataBinder.Eval(Container.DataItem, "COD_ID")%>' Visible="false"></asp:TextBox>
                                                                        <asp:TextBox runat="server" ID="txtidcobranca" Text='<%# DataBinder.Eval(Container.DataItem, "COD_ID_COBRANCA")%>' Visible="false"></asp:TextBox>
                                                                        <asp:TextBox runat="server" ID="txttipo" Text='<%# DataBinder.Eval(Container.DataItem, "FLG_TIPO")%>' Visible="false"></asp:TextBox>
                                                                        <asp:TextBox runat="server" ID="txtcancelado" Text='<%# DataBinder.Eval(Container.DataItem, "FLG_CANCELADO")%>' Visible="false"></asp:TextBox>
                                                                    </td>

                                                                    <td style="display:none;">
                                                                        <div class="btn-group">
                                                                            <button type="button" class="btn btn-default"><small>Ações</small></button>
                                                                            <button type="button" class="btn btn-default dropdown-toggle" data-toggle="dropdown">
                                                                            <span class="caret"></span>
                                                                            <span class="sr-only"><small>Ações</small></span>
                                                                            </button>
                                                                            <div class="dropdown-menu" role="menu">
                                                                                <small><asp:linkbutton ID="lkbAtualizar" commandname="Atualizar" runat="server" Text="Atualizar" ToolTip="Atualizar"  CommandArgument='<%# DataBinder.Eval(Container.DataItem, "COD_ID")%>' class="dropdown-item"><i class="fas fa-recycle"></i>     Atualizar</asp:linkbutton></small>
                                                                                <small><asp:linkbutton ID="lbkReenviar" commandname="Reenviar" runat="server" Text="Reenviar Cobrança" ToolTip="Reenviar Cobrança"  CommandArgument='<%# DataBinder.Eval(Container.DataItem, "COD_ID")%>' class="dropdown-item"><i class="fas fa-share-square"></i>     Reenviar Cobrança</asp:linkbutton></small>
                                                                                <small><asp:linkbutton ID="lbkExcluir" commandname="Excluir" runat="server" Text="Excluir Cobrança" ToolTip="Excluir Cobrança"  CommandArgument='<%# DataBinder.Eval(Container.DataItem, "COD_ID")%>' class="dropdown-item"><i class="fas fa-share-square"></i>     Excluir Cobrança</asp:linkbutton></small>
                                                                            </div>
                                                                        </div>                                                              
                                                                    </td>

                                                                    <td>
                                                                        <small><%# String.Format("{0:dd/MM/yyyy}",DataBinder.Eval(Container.DataItem, "DTA_DATA"))%></small>

                                                                    </td>

                                                                    <td>
                                                                        <small><%# DataBinder.Eval(Container.DataItem, "FLG_TIPO")%></small>
                                                                    </td>
                                                                    <td>
                                                                        <small><%# DataBinder.Eval(Container.DataItem, "FLG_STATUS")%></small>
                                                                    </td>
                                                                    <td>
                                                                        <asp:linkbutton ID="lbkStatus" commandname="Status" runat="server" text="Status" ToolTip="Status"  CommandArgument='<%# DataBinder.Eval(Container.DataItem, "COD_ID")%>'><i class="fas fa-recycle text-center text-success"></i></asp:linkbutton>
                                                                    </td>

                                                                    <td>
                                                                        <small><%# DataBinder.Eval(Container.DataItem, "NOM_NOME")%></small>
                                                                    </td>
                                                                    <td>
                                                                        <small><%# DataBinder.Eval(Container.DataItem, "NUM_CNPJCPF")%></small>
                                                                    </td>

                                                                    <td>
                                                                        <small><%# String.Format("{0:dd/MM/yyyy}",DataBinder.Eval(Container.DataItem, "DTA_VENCIMENTO"))%></small>

                                                                    </td>
                                                                    <td>
                                                                        <small><%# String.Format("{0:c2}",DataBinder.Eval(Container.DataItem, "NUM_VALOR"))%></small>

                                                                    </td>

                                                                    <td>
                                                                        <small><%# DataBinder.Eval(Container.DataItem, "DES_DESCRICAO")%></small>
                                                                    </td>
                                                                    <td>
                                                                        <asp:linkbutton ID="lbkCancelar" commandname="Cancelar" runat="server" text="Cancelar" ToolTip="Cancelar"  CommandArgument='<%# DataBinder.Eval(Container.DataItem, "COD_ID")%>'><i class="fas fa-ban text-center text-danger"></i></asp:linkbutton>
                                                                    </td>

                                                                    <td>
                                                                        <small><%# DataBinder.Eval(Container.DataItem, "NOM_URL_BOLETO")%></small>
                                                                    </td>


                                                                </tr>
                                                            </ItemTemplate>
                                                        </asp:Repeater>

                                                    </tbody>
                                                </table>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="card-footer">
                                    </div>
                                </div>


                                <!-- Simulador -->
                                <!-- ********************************************************************************************************************************************************** -->
                                <div class="card card-outline col-12" runat="server" id="divSimulador" visible="false">
                                    <div class="card-body">
                                        <div class="row">
                                            <div class="col-sm-12">
                                                <h5 class="text-whitelabel1">Simulador</h5>
                                            </div>
                                        </div>

                                        <div class="form-group row col-12">
                                            <label class="col-2 col-form-label">Período de</label>
						                    <div class="col-2">
                                                <div class="input-group date" id="datepickerIniSimulador" data-target-input="nearest">
								                    <asp:TextBox ID="txtSimuladorInicio" runat="server" CssClass="form-control datetimepicker-input" data-target="#datepickerIniSimulador"></asp:TextBox>
                                                    <div class="input-group-append" data-target="#datepickerIniSimulador" data-toggle="datetimepicker">
                                                        <div class="input-group-text"><i class="fa fa-calendar"></i></div>
                                                    </div>
                                                </div>
                                            </div>

                                            <label class="col-1 col-form-label">à</label>
						                    <div class="col-2">
                                                <div class="input-group date" id="datepickerFimSimulador" data-target-input="nearest">
								                    <asp:TextBox ID="txtSimuladorFim" runat="server" CssClass="form-control datetimepicker-input" data-target="#datepickerFimSimulador"></asp:TextBox>
                                                    <div class="input-group-append" data-target="#datepickerFimSimulador" data-toggle="datetimepicker">
                                                        <div class="input-group-text"><i class="fa fa-calendar"></i></div>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>                                            

                                        <div class="row">
                                            <div class="col-12">
                                                <asp:Button runat="server" ID="btnPesquisarSimulador" Text="Pesquisar" CssClass="btn btn-whitelabel1" onclick="btnPesquisarSimulador_Click" />
                                                <asp:Button runat="server" ID="btnNovaSimulacao" Text="Nova Simulação" CssClass="btn btn-whitelabel1" onclick="btnNovaSimulacao_Click" />
                                            </div>
                                        </div>


                                        <div class="row"  style="overflow:auto; width: 100%;">
                                            <div class="col-12">
                                                <table id="Table1" class="table table-bordered table-hover">
                                                    <thead>
                                                        <tr>
                                                            <th>Editar</th>
                                                            <th>ID</th>
                                                            <th>Data</th>
                                                            <th>Simulação</th>
                                                        </tr>
                                                    </thead>
                                                    <tbody>
                                                        <asp:Repeater runat="server" ID="rptSimulador">
                                                            <ItemTemplate>
                                                                <tr>
                                                                    <td>
                                                                        <a href="" onclick="javascript:openPopupWindow('cad_simular_venda.aspx?id=<%#DataBinder.Eval(Container.DataItem,"COD_ID")%>', 'SimularVendaEdicao', 1024, 800); return false;" class="btn btn-sm btn-secondary">
                                                                            <i class="fas fa-edit"></i>
                                                                        </a>
                                                                    </td>
                                                                    <td>


                                                                        <small><%# DataBinder.Eval(Container.DataItem, "COD_ID")%></small>
                                                                        <asp:TextBox runat="server" ID="txtid" Text='<%# DataBinder.Eval(Container.DataItem, "COD_ID")%>' Visible="false"></asp:TextBox>
                                                                    </td>

                                                                    <td>
                                                                        <small><%# String.Format("{0:dd/MM/yyyy}",DataBinder.Eval(Container.DataItem, "DTA_DATA"))%></small>

                                                                    </td>

                                                                    <td>
                                                                        <small><%# DataBinder.Eval(Container.DataItem, "NOM_SIMULAR_VENDAS")%></small>
                                                                    </td>

                                                                </tr>
                                                            </ItemTemplate>
                                                        </asp:Repeater>

                                                    </tbody>
                                                </table>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="card-footer">
                                    </div>
                                </div>

                                <!-- Dados Cobrança -->
                                <!-- ********************************************************************************************************************************************************** -->

                                <div class="card card-outline col-12" runat="server" id="divDadosCobranca" visible="false">
                                    <div class="card-body">
                                        <div class="row">
                                            <div class="col-sm-12">
                                                <h5 class="text-whitelabel1"><asp:Label runat="server" ID="lblTituloPadraoTopo" Text=""></asp:Label></h5>
                                            </div>
                                        </div>
                                        <div class="row">
                                            <div class="col-12">
                                                <div class="row">
                                                    <div class="col-sm-12">
                                                        <h5 class="text-whitelabel1">Preencha os dados referentes a cobrança</h5>
                                                    </div>
                                                </div>
                                                <div class="row">
                                                    <div class="col-sm-12">
                                                        <h6 class="text-whitelabel1">Digite um valor maior que R$ 10,00</h6>
                                                    </div>
                                                </div>

                                                <div class="row">
                                                    <div class="col-sm-2">
                                                        <div class="form-group">
                                                            <asp:Label runat="server" ID="Label5" Text="Valor"></asp:Label>
                                                            <asp:TextBox runat="server" ID="txtValor" CssClass="form-control"></asp:TextBox>
                                                        </div>
                                                    </div>
                                                    <div class="col-sm-2">
                                                        <div class="form-group">
                                                            <asp:Label runat="server" ID="Label6" Text="Vencimento"></asp:Label>
                                                            <div class="input-group date" id="dpVencimento" data-target-input="nearest">
								                                <asp:TextBox ID="txtVencimento" runat="server" CssClass="form-control datetimepicker-input" data-target="#dpVencimento"></asp:TextBox>
                                                                <div class="input-group-append" data-target="#dpVencimento" data-toggle="datetimepicker">
                                                                    <div class="input-group-text"><i class="fa fa-calendar"></i></div>
                                                                </div>
                                                            </div>
                                                        </div>
                                                    </div>
                                                    <div class="col-sm-6">
                                                        <div class="form-group">
                                                            <asp:Label runat="server" ID="Label8" Text="Descrição"></asp:Label>
                                                            <asp:TextBox runat="server" ID="txtDescricao" CssClass="form-control"></asp:TextBox>
                                                        </div>
                                                    </div>
                                                    <div class="col-sm-2">
                                                        <div class="form-group">
                                                        <asp:Label runat="server" ID="Label19" Text="Adquirente"></asp:Label>
                                                            <asp:DropDownList runat="server" ID="ddlAdquirentes" CssClass="form-control">
                                                            </asp:DropDownList>
                                                        </div>
                                                    </div>


                                                </div>

                                            </div>
                                        </div>
                                    </div>
                                </div>

                                <!-- Dados Cliente -->
                                <!-- ********************************************************************************************************************************************************** -->

                                <div class="card card-outline col-12" runat="server" id="divDadosCliente" visible="false">
                                    <div class="card-body">
                                        <div class="row">
                                            <div class="col-12">
                                                <div class="row">
                                                    <div class="col-sm-12">
                                                        <h5 class="text-whitelabel1">Selecione abaixo o cliente para quem deseja gerar a cobrança</h5>
                                                    </div>
                                                </div>

                                                <div class="row">
                                                    <div class="col-sm-12">
                                                        <div class="form-group">
                                                            <asp:Label runat="server" ID="Label12" Text="Cliente"></asp:Label>
                                                            <asp:DropDownList runat="server" ID="ddlClientePadrao" CssClass="form-control" 
                                                                AutoPostBack="True" onselectedindexchanged="ddlClientePadrao_SelectedIndexChanged" ></asp:DropDownList>
                                                        </div>
                                                    </div>
                                                </div>
                                                
                                                <div class="row">
                                                    <div class="col-sm-12">
                                                        <h6>ou preencha os dados abaixo</h6>
                                                    </div>
                                                </div>

                                                <div class="row">
                                                    <div class="col-sm-4">
                                                        <div class="form-group">
                                                            <asp:Label runat="server" ID="Label13" Text="Nome"></asp:Label>
                                                            <asp:TextBox runat="server" ID="txtNome" CssClass="form-control"></asp:TextBox>
                                                        </div>
                                                    </div>
                                                    <div class="col-sm-4">
                                                        <div class="form-group">
                                                        <asp:Label runat="server" ID="Label14" Text="CNPJ/CPF"></asp:Label>
                                                            <asp:TextBox runat="server" ID="txtCNPJCPF" CssClass="form-control"></asp:TextBox>
                                                        </div>
                                                    </div>
                                                    <div class="col-sm-4">
                                                        <div class="form-group">
                                                        <asp:Label runat="server" ID="Label15" Text="E-Mail"></asp:Label>
                                                            <asp:TextBox runat="server" ID="txtEmail" CssClass="form-control"></asp:TextBox>
                                                        </div>
                                                    </div>

                                                </div>

                                                <div class="row">
                                                    <div class="col-sm-3">
                                                        <div class="form-group">
                                                            <asp:Label runat="server" ID="Label20" Text="Celular"></asp:Label>
                                                            <asp:TextBox runat="server" ID="txtCelular" CssClass="form-control"></asp:TextBox>
                                                        </div>
                                                    </div>
                                                    <div class="col-sm-3">
                                                        <div class="form-group">
                                                        <asp:Label runat="server" ID="Label21" Text="CEP"></asp:Label>
                                                            <asp:TextBox runat="server" ID="txtCEP" CssClass="form-control"></asp:TextBox>
                                                        </div>
                                                    </div>
                                                    <div class="col-sm-3">
                                                        <div class="form-group">
                                                        <asp:Label runat="server" ID="Label22" Text="Número"></asp:Label>
                                                            <asp:TextBox runat="server" ID="txtNumero" CssClass="form-control"></asp:TextBox>
                                                        </div>
                                                    </div>

                                                </div>

                                            </div>
                                        </div>
                                    </div>
                                </div>

                                <!-- Detalhes da Cobrança -->
                                <!-- ********************************************************************************************************************************************************** -->

                                <div class="card card-outline col-12" runat="server" id="divDetalhesCobranca" visible="false">
                                    <div class="card-body">


                                        <div class="row">
                                            <div class="col-12">
                                                <div class="row">
                                                    <div class="col-sm-12">
                                                        <h5 class="text-whitelabel1">Preencha os dados referente aos detalhes da cobrança</h5>
                                                    </div>
                                                </div>
                                                <div class="row">
                                                    <div class="col-sm-3">
                                                        <div class="form-group">
                                                        <asp:Label runat="server" ID="Label7" Text="Código Referência"></asp:Label>
                                                            <asp:TextBox runat="server" ID="txtReferencia" CssClass="form-control"></asp:TextBox>
                                                        </div>
                                                    </div>
                                                    <div class="col-sm-3">
                                                        <div class="form-group">
                                                        <asp:Label runat="server" ID="Label9" Text="Dias Limite Pagamento"></asp:Label>
                                                            <asp:TextBox runat="server" ID="txtDias" CssClass="form-control" Text="2"></asp:TextBox>
                                                        </div>
                                                    </div>

                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>


                                <!-- Detalhes do Split -->
                                <!-- ********************************************************************************************************************************************************** -->

                                <div class="card card-outline col-12" runat="server" id="divSplitCobranca" visible="false">
                                    <div class="card-body">


                                        <div class="row">
                                            <div class="col-12">
                                                <div class="row">
                                                    <div class="col-sm-12">
                                                        <h5 class="text-whitelabel1">Preencha os dados referente aos detalhes do Split</h5>
                                                    </div>
                                                </div>
                                                <div class="row">
                                                    <div class="col-sm-12">
                                                        <div class="form-group">
                                                            <div class="custom-control custom-checkbox">
                                                              <input class="custom-control-input" type="checkbox" runat="server" id="ckbSplitCobranca" checked>
                                                              <label for="ckbSplitCobranca" class="custom-control-label">A venda terá divisão de Split?</label>
                                                            </div>

                                                        </div>
                                                    </div>
                                                </div>







                                            </div>
                                        </div>
                                    </div>
                                </div>



                                <!-- Cobrança Boleto -->
                                <!-- ********************************************************************************************************************************************************** -->
                                <div class="card card-outline col-12" runat="server" id="divBoleto" visible="false">
                                    <div class="card-body">
                                        <div class="row">
                                            <div class="col-12">

                                                <div class="row">
                                                    <div class="col-sm-12">
                                                        <div class="form-group">
                                                            <div class="custom-control custom-checkbox">
                                                              <input class="custom-control-input" type="checkbox" runat="server" id="ckbBEmail" checked>
                                                              <label for="ckbBEmail" class="custom-control-label">Enviar link do pagamento por e-mail?</label>
                                                            </div>

                                                        </div>
                                                    </div>
                                                </div>



                                                <div class="row" runat="server" id="divBoletoGerado" visible="false">
                                                    <div class="col-sm-12">
                                                        <div class="form-group">
                                                            <asp:Label runat="server" ID="Label10" Text="Endereço do link de pagamento"></asp:Label>
                                                            <asp:TextBox runat="server" ID="txtURLBoletoBancario" CssClass="form-control"></asp:TextBox>
                                                        </div>
                                                    </div>

                                                    <div class="col-sm-12">
                                                        <div class="form-group">
                                                            <a href="" runat="server" id="hrfBoleto" class="btn btn-block btn-whitelabel1" target="_blank"><asp:Label runat="server" ID="Label11" Text="CLIQUE AQUI PARA ABRIR LINK DE PAGAMENTO"></asp:Label></a>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="card-footer">
                                        <asp:Button runat="server" ID="btnContinuarBoletos" Text="Confirmar os dados e gerar boleto bancário" CssClass="btn btn-whitelabel1" onclick="btnContinuarBoletos_Click" />
                                        <asp:Button runat="server" ID="Button1" Text="novo" 
                                            CssClass="btn btn-whitelabel1" onclick="Button1_Click" />
                                        <asp:Button runat="server" ID="btnVoltarBoletos" Text="Nova cobrança por boleto bancário" CssClass="btn btn-whitelabel1" onclick="btnVoltarBoletos_Click" Visible="false" />
                                    </div>
                                </div>

                                <!-- Pix QR Code -->
                                <!-- ********************************************************************************************************************************************************** -->
                                <div class="card card-outline col-12" runat="server" id="divPixQRCode" visible="false">
                                    <div class="card-body">
                                        <div class="row">
                                            <div class="col-sm-12">
                                                <h5 class="text-whitelabel1">Receber por QR Code</h5>
                                            </div>
                                        </div>

                                        <div class="row">
                                            <div class="col-sm-12">
                                                <div class="form-group">
                                                    <div class="custom-control custom-checkbox">
                                                        <input class="custom-control-input" type="checkbox" runat="server" id="ckbEmailQRCode" checked>
                                                        <label for="ckbBEmail" class="custom-control-label">Enviar link do pagamento por e-mail?</label>
                                                    </div>

                                                </div>
                                            </div>
                                        </div>

                                        <div class="row"  runat="server" id="divQRCode" visible="false">

                                            <div class="col-sm-12">
                                                <div class="form-group">
                                                    <asp:Label runat="server" ID="Label23" Text="Endereço do link de pagamento"></asp:Label>
                                                    <asp:TextBox runat="server" ID="txtURLQRCode" CssClass="form-control"></asp:TextBox>
                                                </div>
                                            </div>

                                            <div class="col-sm-12">
                                                <div class="form-group">
                                                    <a href="" runat="server" id="hrfQRCode" class="btn btn-block btn-whitelabel1" target="_blank"><asp:Label runat="server" ID="Label24" Text="CLIQUE AQUI PARA ABRIR LINK DE PAGAMENTO"></asp:Label></a>
                                                </div>
                                            </div>



                                            <div class="col-10">
                                                <h6><i class="fas fa-qrcode"></i>  Pix copia e cola<br /></h6>
                                                <asp:LinkButton ID="lbkCopiar" runat="server" CssClass="btn btn-default" ToolTip="copiar" OnClientClick="myFunctionChavePix()"><i class="fas fa-copy"></i></asp:LinkButton>
                                                <asp:TextBox runat="server" ID="lblPixCopiaCola" Text="" TextMode="MultiLine" Rows="3" CssClass="form-control"></asp:TextBox>
                                            </div>
                                            <div class="col-2">
                                                <center>
                                                <strong><i class="fab fa-pix"></i>  Pague o boleto com Pix usando o QRcode abaixo<br /></strong>
                                                <img runat="server" class="img-fluid" id="imgQRcode" src="" alt=""  />                                    
                                                </center>
                                            </div>
                                        </div>

                                        <div class="row">
                                            <div class="col-12">


                                            </div>
                                        </div>
                                    </div>
                                    <div class="card-footer">
                                        <asp:Button runat="server" ID="btnContinuarPixQRCode" 
                                            Text="Confirmar recebimento por QRCode" CssClass="btn btn-whitelabel1" 
                                            onclick="btnContinuarPixQRCode_Click" />
                                        <asp:Button runat="server" ID="btnVoltarPixQRCode" Text="Voltar" 
                                            CssClass="btn btn-whitelabel1"  Visible="false" 
                                            onclick="btnVoltarPixQRCode_Click" />
                                    </div>
                                </div>

                                <!-- Cobrança Link Pagamento -->
                                <!-- ********************************************************************************************************************************************************** -->
                                <div class="card card-outline col-12" runat="server" id="divLinkPagamento" visible="false">
                                    <div class="card-body">
                                        <div class="row">
                                            <div class="col-sm-12">
                                                <h5 class="text-whitelabel1">Selecione as formas de pagamento</h5>
                                            </div>
                                        </div>





                                        <div class="row">
                                            <div class="col-lg-4 col-md-4 col-12" runat="server" id="dvFPCredito">
                                            <div class="info-box">
                                                <span class="info-box-icon bg-whitelabel1 elevation-1"><i class="fas fa-credit-card"></i></span>

                                                <div class="info-box-content text-center">
                                                <span class="info-box-text">Crédito</span>
                                                <span class="info-box-number">

                                                    <div class="form-group">
                                                        <div class="form-check">
                                                            <asp:CheckBox CssClass="form-check-input" runat="server" id="ckbCredito"/>
                                                        </div>
                                                    </div>

                                                </span>
                                                </div>
                                            </div>
                                            </div>
                                            <div class="col-lg-4 col-md-4 col-12" runat="server" id="dvFPBoleto">
                                            <div class="info-box">
                                                <span class="info-box-icon bg-whitelabel1 elevation-1"><i class="fas fa-barcode"></i></span>

                                                <div class="info-box-content text-center">
                                                <span class="info-box-text">Boleto</span>
                                                <span class="info-box-number">

                                                    <div class="form-group">
                                                        <div class="form-check">
                                                            <asp:CheckBox CssClass="form-check-input" runat="server" id="ckbBoleto"/>
                                                        </div>
                                                    </div>

                                                </span>
                                                </div>
                                            </div>
                                            </div>

                                            <div class="col-lg-4 col-md-4 col-12" runat="server" id="dvFPPix">
                                            <div class="info-box ">
                                                <span class="info-box-icon bg-whitelabel1 elevation-1 "><i class="fab fa-pix"></i>
                                        
                                                </span>

                                                <div class="info-box-content text-center">
                                                <span class="info-box-text">Pix</span>
                                                <span class="info-box-number">

                                                    <div class="form-group">
                                                        <div class="form-check">
                                                            <asp:CheckBox CssClass="form-check-input" runat="server" id="ckbPix"/>
                                                        </div>
                                                    </div>

                                                </span>
                                                </div>
                                            </div>
                                            </div>
                                        </div>

                                        <div class="row">
                                            <div class="col-sm-3">
                                                <div class="form-group">
                                                <asp:Label runat="server" ID="Label26" Text="Forma Pagto"></asp:Label>
                                                    <asp:DropDownList runat="server" ID="ddlFormaLinkPagamento" CssClass="form-control">
                                                        <asp:ListItem Value="V" Text="À Vista"></asp:ListItem>
                                                        <asp:ListItem Value="P" Text="Parcelada"></asp:ListItem>
                                                    </asp:DropDownList>
                                                </div>
                                            </div>
                                            <div class="col-sm-3"  runat="server" id="dvParceladoLinkPagamento">
                                                <div class="form-group">
                                                <asp:Label runat="server" ID="Label27" Text="Parcelas"></asp:Label>
                                                    <asp:DropDownList runat="server" ID="ddlParcelasLinkPagamento" CssClass="form-control">
                                                        <asp:ListItem Value="1" Text="1x"></asp:ListItem>
                                                        <asp:ListItem Value="2" Text="2x"></asp:ListItem>
                                                        <asp:ListItem Value="3" Text="3x"></asp:ListItem>
                                                        <asp:ListItem Value="4" Text="4x"></asp:ListItem>
                                                        <asp:ListItem Value="5" Text="5x"></asp:ListItem>
                                                        <asp:ListItem Value="6" Text="6x"></asp:ListItem>
                                                        <asp:ListItem Value="7" Text="7x"></asp:ListItem>
                                                        <asp:ListItem Value="8" Text="8x"></asp:ListItem>
                                                        <asp:ListItem Value="9" Text="9x"></asp:ListItem>
                                                        <asp:ListItem Value="10" Text="10x"></asp:ListItem>
                                                        <asp:ListItem Value="11" Text="11x"></asp:ListItem>
                                                        <asp:ListItem Value="12" Text="12x"></asp:ListItem>
                                                    </asp:DropDownList>
                                                </div>
                                            </div>
                                            <div class="col-sm-3">
                                                <div class="form-group">
                                                <asp:Label runat="server" ID="Label28" Text="Link Parmanente"></asp:Label>
                                                    <asp:DropDownList runat="server" ID="ddlLinkPermanente" CssClass="form-control">
                                                        <asp:ListItem Value="N" Text="Não (Cobrança única)"></asp:ListItem>
                                                        <asp:ListItem Value="S" Text="Sim (Permanente)"></asp:ListItem>
                                                    </asp:DropDownList>
                                                </div>
                                            </div>
                                        </div>

                                        <div class="row">
                                            <div class="col-lg-12 col-md-12 col-12">
                                                <div class="form-group">
                                                    
                                                    <div class="row">
                                                        <div class="col-lg-1 col-md-2 col-2">
                                                            <div class="form-check">
                                                                <asp:CheckBox CssClass="form-check-input" runat="server" 
                                                                    id="ckbLinkPagamentoAvançado" AutoPostBack="True" oncheckedchanged="ckbLinkPagamentoAvançado_CheckedChanged"/>
                                                            </div>
                                                        </div>
                                                        <div class="col-lg-11 col-md-10 col-10">
                                                            <h5><asp:Label runat="server" ID="Label33" CssClass="text-whitelabel1" Text="Habilitar configurações avançadas"></asp:Label></h5>
                                                        </div>
                                                    </div>

                                                    
                                                </div>
                                            </div>
                                        </div>
                                        
                                        <div class="row" runat="server" id="divLinkPagamentoAvancado" visible="false">
                                            <div class="col-sm-12">

                                                <div class="row">
                                                    <div class="col-sm-12">
                                                    


                                                      <div class="card">
                                                        <div class="card-header bg-whitelabel1">
                                                          <h4 class="card-title">
                                                              <i class="fas fa-user"></i>     Cliente
                                                          </h4>
                                                        </div>
                                                          <div class="card-body">

                                                            <div class="row">
                                                                <div class="col-sm-12">
                                                                    <div class="form-group">
                                                                        <div class="form-check">
                                                                          <asp:CheckBox CssClass="form-check-input" runat="server" id="ckbCliente" 
                                                                                AutoPostBack="True" oncheckedchanged="ckbCliente_CheckedChanged"/>
                                                                          <label class="form-check-label">Clique aqui caso deseje especificar ou selecionar o cliente</label>
                                                                        </div>
                                                                    </div>
                                                                </div>   
                                                            </div>



                                                            <div class="row" runat="server" id="divCliente" visible="false">



                                                                <div class="col-sm-12">
                                                                    <div class="form-group">
                                                                    <asp:Label runat="server" ID="lblFiltro" Text="Selecione o cliente desejado"></asp:Label>
                                                                    <asp:DropDownList runat="server" id="ddlComprador" CssClass="form-control" 
                                                                            AutoPostBack="True" onselectedindexchanged="ddlComprador_SelectedIndexChanged">
                                                                    </asp:DropDownList>
                                                                    </div>
                                                                </div>

                                                                <div class="col-sm-12">
                                                                    <div class="form-group">
                                                                        <asp:Label runat="server" ID="lbl901" Text="Ou digite os dados abaixo"></asp:Label>
                                                                    </div>
                                                                </div>

                                                                <div class="col-sm-6">
                                                                    <div class="form-group">
                                                                        <asp:Label runat="server" ID="lbl902" Text="Nome"></asp:Label>
                                                                        <asp:TextBox id="txtIDCliente" runat="server" class="form-control" Visible="false"></asp:TextBox>
                                                                        <asp:TextBox id="txtIDZOOPCliente" runat="server" class="form-control" Visible="false"></asp:TextBox>
                                                                        <asp:TextBox id="txtNomeCliente" runat="server" class="form-control"></asp:TextBox>
                                                                    </div>
                                                                </div>
                                                                <div class="col-sm-6">
                                                                    <div class="form-group">
                                                                        <asp:Label runat="server" ID="lbl903" Text="Sobrenome"></asp:Label>
                                                                        <asp:TextBox id="txtSobrenomeCliente"  runat="server" class="form-control" ></asp:TextBox>
                                                                    </div>
                                                                </div>
                                                                <div class="col-sm-4">
                                                                    <div class="form-group">
                                                                        <asp:Label runat="server" ID="lbl904" Text="Documento (CNPJ/CPF)"></asp:Label>
                                                                        <asp:TextBox id="txtDocumentoCliente"  runat="server" class="form-control" ></asp:TextBox>
                                                                    </div>
                                                                </div>
                                                                <div class="col-sm-4">
                                                                    <div class="form-group">
                                                                        <asp:Label runat="server" ID="lbl905" Text="E-mail"></asp:Label>
                                                                        <asp:TextBox id="txtEmailCliente" runat="server" class="form-control" ></asp:TextBox>
                                                                    </div>
                                                                </div>
                                                                <div class="col-sm-4">
                                                                    <div class="form-group">
                                                                        <asp:Label runat="server" ID="lbl906" Text="Celular"></asp:Label>
                                                                        <asp:TextBox id="txtCelularCliente" runat="server" class="form-control" ></asp:TextBox>
                                                                    </div>
                                                                </div>


                                                                <div class="col-sm-6"  >
                                                                    <div class="form-group">
                                                                    <asp:Label runat="server" ID="lbl907" Text="CEP"></asp:Label>
                                                                    <asp:TextBox runat="server" ID="txtCEPCliente" CssClass="form-control" 
                                                                            data-inputmask='"mask": "99999-999"' data-mask  
                                                                            AutoPostBack="True" ontextchanged="txtCEP_TextChanged"></asp:TextBox> 
                                                                    </div>
                                                                </div>
                                                                <div class="col-sm-6"  >
                                                                    <div class="form-group">
                                                                    <asp:Label runat="server" ID="lbl908" Text="Endereço"></asp:Label>
                                                                    <asp:TextBox runat="server" ID="txtEnderecoCliente" CssClass="form-control" ></asp:TextBox> 
                                                                    </div>
                                                                </div>

                                                                <div class="col-sm-6"  >
                                                                    <div class="form-group">
                                                                    <asp:Label runat="server" ID="lbl909" Text="Número"></asp:Label>
                                                                    <asp:TextBox runat="server" ID="txtNumeroCliente" CssClass="form-control" ></asp:TextBox> 
                                                                    </div>
                                                                </div>
                                          
                                                                <div class="col-sm-6"  >
                                                                    <div class="form-group">
                                                                    <asp:Label runat="server" ID="lbl910" Text="Complemento"></asp:Label>
                                                                    <asp:TextBox runat="server" ID="txtComplementoCliente" CssClass="form-control"></asp:TextBox>
                                                                    </div>
                                                                </div>
                                                                <div class="col-sm-5"  >
                                                                    <div class="form-group">
                                                                    <asp:Label runat="server" ID="lbl911" Text="Bairro"></asp:Label>
                                                                    <asp:TextBox runat="server" ID="txtBairroCliente" CssClass="form-control"></asp:TextBox> 
                                                                    </div>
                                                                </div>
                                                                <div class="col-sm-5"  >
                                                                    <div class="form-group">
                                                                    <asp:Label runat="server" ID="lbl912" Text="Cidade"></asp:Label>
                                                                    <asp:TextBox runat="server" ID="txtCidadeCliente" CssClass="form-control"></asp:TextBox> 
                                                                    </div>
                                                                </div>
                                          
                                                                <div class="col-sm-2"  >
                                                                    <div class="form-group">
                                                                        <asp:Label runat="server" ID="lbl913" Text="Estado"></asp:Label>
                                                                        <asp:DropDownList id="ddlEstadoCliente" runat="server" class="form-control">
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

                                                
                                                <div class="row">
                                                    <div class="col-sm-12">
                                                    

                                                      <!-- Campo Adicional -->

                                                      <div class="card">
                                                        <div class="card-header bg-whitelabel1">
                                                          <h4 class="card-title">
                                                              <i class="fas fa-user"></i>     Informação Adicional
                                                          </h4>
                                                        </div>
                                                          <div class="card-body">

                                                            <div class="row">
                                                                <div class="col-sm-12">
                                                                    <div class="form-group">
                                                                        <asp:Label runat="server" ID="Label55" Text="Especifique abaixo o nome da informação adicional que deseja coletar no checkout"></asp:Label>
                                                                        <asp:TextBox runat="server" id="txtCampo01" cssClass="form-control" ></asp:TextBox>
                                                                    </div>
                                                                </div>   
                                                            </div>

                                                          </div>
                                                      </div>

                                                    
                                                    </div>
                                                </div>






                                            
                                                <div class="row">
                                                    <div class="col-sm-12">
                                            
                                                        <div class="card">
                                                            <div class="card-header bg-whitelabel1">
                                                                <h4 class="card-title">
                                                                <i class="fas fa-envelope"></i>     E-mail
                                                                </h4>
                                                            </div>
                                                            <div class="card-body">
                                                                <div class="col-sm-12">
                                                                    <div class="form-group">
                                                                    <asp:Label runat="server" ID="Label35" Text="Para quem você deseja enviar o link de pagamento?"></asp:Label>
                                                                    <asp:TextBox runat="server" id="txtEmails" cssClass="form-control" ></asp:TextBox>
                                                                    </div>
                                                                </div>   

                                                                <div class="col-sm-12">
                                                                    <div class="alert alert-warning alert-dismissible">
                                                                        <h5><i class="icon fas fa-exclamation-triangle"></i> Atenção!</h5>
                                                                        Você pode enviar para múltiplos e-mails (separe-os por vírgula)
                                                                    </div>
                                                                </div>
                                                            </div>
                                                        </div>                                                
                                                    </div>
                                                </div>

                                                <div class="row">
                                                    <div class="col-sm-12">
                                                        <div class="card">
                                                            <div class="card-header bg-whitelabel1">
                                                                <h4 class="card-title">
                                                                <i class="fas fa-users"></i>     Split
                                                                </h4>
                                                            </div>
                                                            
                                                            
                                                            
                                                            <div class="card-body">
                                                                <div class="row">
                                                                    <div class="col-sm-12">
                                                                        <div class="form-group">
                                                                            <div class="form-check">
                                                                              <asp:CheckBox CssClass="form-check-input" runat="server" id="ckbSplit" 
                                                                                    AutoPostBack="True" oncheckedchanged="ckbSplit_CheckedChanged"/>
                                                                              <label class="form-check-label">A venda terá divisão de Split</label>
                                                                            </div>
                                                                        </div>
                                                                    </div>   
                                                                </div>
                                                                <div class="row" runat="server" id="divSplit" visible="false">
                                                                    <div class="row">
                                                                        <div class="col-sm-12">
                                                                            <h5 class="text-whitelabel1">Selecione os beneficiários do split</h5>
                                                                        </div>
                                                                        <div class="col-sm-12">
                                                                            <div class="form-group">
                                                                                <asp:Label runat="server" ID="Label37" Text="Beneficiário"></asp:Label>
                                                                                <asp:DropDownList runat="server" id="ddlParceiros" CssClass="form-control" 
                                                                                        AutoPostBack="True" onselectedindexchanged="ddlParceiros_SelectedIndexChanged">
                                                                                </asp:DropDownList>
                                                                            </div>
                                                                        </div>  
                                                                        <div class="col-sm-12">
                                                                            <h5 class="text-whitelabel1">ou Preencha os dados abaixo e clique em Incluir split</h5>
                                                                        </div>
                                                                    </div>

                                                                    <div class="row">
                                                                        <div class="col-sm-3">
                                                                            <div class="form-group">
                                                                                <asp:Label runat="server" ID="Label36" Text="Nome"></asp:Label>
                                                                                <asp:TextBox runat="server" id="txtBeneficiario" cssClass="form-control"></asp:TextBox>
                                                                            </div>
                                                                        </div>                                

                                                                        <div class="col-sm-3">
                                                                            <div class="form-group">
                                                                            <asp:Label runat="server" ID="Label38" Text="Wallet ID"></asp:Label>
                                                                            <asp:TextBox runat="server" id="txtWalletID" cssClass="form-control"></asp:TextBox>
                                                                            </div>
                                                                        </div>                                
                                                                        <div class="col-sm-2">
                                                                            <div class="form-group">
                                                                            <asp:Label runat="server" ID="Label39" Text="Valor"></asp:Label>
                                                                            <asp:TextBox runat="server" id="txtValorBeneficiario" cssClass="form-control" ></asp:TextBox>
                                                                            </div>
                                                                        </div>   
                                                                        <div class="col-sm-2">
                                                                            <div class="form-group">
                                                                            <asp:Label runat="server" ID="Label40" Text="Percentual(%)"></asp:Label>
                                                                            <asp:TextBox runat="server" id="txtPercentualBeneficiario" cssClass="form-control" ></asp:TextBox>
                                                                            </div>
                                                                        </div>   
                                                                        <div class="col-sm-2">
                                                                            <br />
                                                                            <asp:Button runat="server" ID="btnIncluirSplit" CssClass="btn btn-sm btn-whitelabel1" 
                                                                                Text="Incluir Split" onclick="btnIncluirSplit_Click" />
                                                                        </div>
                                                                    </div>

                                                                    <div class="col-sm-12" runat="server" id="divParceirosInclusao" visible="false" style="overflow:auto; width: 100%; ">

                                                                        <table id="Table4" class="table table-bordered table-hover">
                                                                            <thead>
                                                                            <tr>
                                                                                <th>Beneficiário</th>
                                                                                <th>Wallet ID</th>
                                                                                <th>Valor</th>
                                                                                <th>Percentual</th>
                                                                                <th>Excluir</th>
                                                                            </tr>
                                                                            </thead>
                                                                            <tbody>


                                                                                <asp:ListView ID="lsvParceiros" runat="server" 
                                                                                    OnItemCommand="lsvParceiros_ItemCommand">
                                                                                    <ItemTemplate>
                                                                                            <tr>
                                                                                                <td>
                                                                                                    <small><asp:Label ID="lblIDParceiro" runat="server" Text='<%# Eval("beneficiario") %>'></asp:Label></small>
                                                                                                    <asp:TextBox runat="server" ID="txtbeneficiario" Text='<%# Eval("beneficiario") %>' Visible="false"></asp:TextBox>
                                                                                                </td>
                                                                                                <td>
                                                                                                    <small><asp:Label ID="Label6" runat="server" Text='<%# Eval("walletid") %>'></asp:Label></small>
                                                                                                    <asp:TextBox runat="server" ID="txtwalletid" Text='<%# Eval("walletid") %>' Visible="false"></asp:TextBox>
                                                                                                </td>
                                                                                                <td>
                                                                                                    <small><asp:Label ID="Label16" runat="server" Text='<%# Eval("valor") %>'></asp:Label></small>
                                                                                                    <asp:TextBox runat="server" ID="txtValor" Text='<%# Eval("valor") %>' Visible="false"></asp:TextBox>
                                                                                                </td>
                                                                                                <td>
                                                                                                    <small><asp:Label ID="Label17" runat="server" Text='<%# Eval("percentual") %>'></asp:Label></small>
                                                                                                    <asp:TextBox runat="server" ID="txtPercentual" Text='<%# Eval("percentual") %>' Visible="false"></asp:TextBox>
                                                                                                </td>
                                                                                                <td>
                                                                                                    <asp:linkbutton ID="lkbExcluir" CssClass="" commandname="Excluir" runat="server" text="Excluir" ToolTip="Excluir" CommandArgument='<%# Eval("beneficiario") %>' ><i class="fas fa-trash text-danger"></i></asp:linkbutton>
                                                                                                </td>
                                                                                            </tr>
                                                                                    </ItemTemplate>
                                                                                </asp:ListView>


                                                                            </tbody>
                                                                        </table>
                                    
                                                                    </div>


                                                                </div>
                                                            </div>






                                                        </div>                                                      
                                                    </div>
                                                </div>


                                                <div class="row">
                                                    <div class="col-sm-12">
                                                        <div class="card">
                                                            <div class="card-header bg-whitelabel1">
                                                                <h4 class="card-title">
                                                                <i class="fas fa-box"></i>     Produtos
                                                                </h4>
                                                            </div>
                                                            <div class="card-body">

                                                                <div class="row">
                                                                    <div class="col-sm-12">
                                                                        <div class="form-group">
                                                                            <div class="form-check">
                                                                              <asp:CheckBox CssClass="form-check-input" runat="server" id="ckbProdutos" 
                                                                                    AutoPostBack="True" oncheckedchanged="ckbProdutos_CheckedChanged"/>
                                                                              <label class="form-check-label">Detalhar Produtos e Serviços</label>
                                                                            </div>
                                                                        </div>
                                                                    </div>   

                                                                </div>



                                                                <div class="row" runat="server" id="divProdutos" visible="false">

                                                                    <div class="col-sm-12">
                                                                        <div class="form-group">
                                                                            <div class="form-check">
                                                                              <asp:CheckBox CssClass="form-check-input" runat="server" id="ckbExibirProdutos"/>
                                                                              <label class="form-check-label">Exibir os produtos no CHECK-OUT</label>
                                                                            </div>
                                                                        </div>
                                                                    </div>   

                                                                    <div class="col-sm-10">
                                                                        <div class="form-group">
                                                                            <asp:Label runat="server" ID="Label41" Text="Selecione o produto"></asp:Label>
                                                                            <asp:DropDownList runat="server" id="ddlProduto" CssClass="form-control" 
                                                                                AutoPostBack="True" onselectedindexchanged="ddlProduto_SelectedIndexChanged">
                                                                            </asp:DropDownList>
                                                                        </div>
                                                                    </div>
                                                                    <div class="col-sm-2">
                                                                        <br />
                                                                        <asp:Button runat="server" ID="btnNovoProduto" CssClass="btn btn-sm btn-whitelabel1" 
                                                                            Text="Novo Produto" onclick="btnNovoProduto_Click" />
                                                                    </div>

                                                                    <div class="col-sm-2">
                                                                        <div class="form-group">
                                                                        <asp:Label runat="server" ID="Label42" Text="ID"></asp:Label>
                                                                        <asp:TextBox runat="server" id="txtIDProduto" cssClass="form-control" ></asp:TextBox>
                                                                        </div>
                                                                    </div>   
                                                                    <div class="col-sm-4">
                                                                        <div class="form-group">
                                                                        <asp:Label runat="server" ID="Label43" Text="Produto"></asp:Label>
                                                                        <asp:Label runat="server" id="txtProduto" cssClass="form-control" ></asp:Label>
                                                                        </div>
                                                                    </div>                                
                                                                    <div class="col-sm-2">
                                                                        <div class="form-group">
                                                                        <asp:Label runat="server" ID="Label44" Text="Qtde"></asp:Label>
                                                                        <asp:TextBox runat="server" id="txtQtdeProduto" cssClass="form-control" ></asp:TextBox>
                                                                        </div>
                                                                    </div>   


                                                                    <div class="col-sm-2">
                                                                        <div class="form-group">
                                                                        <asp:Label runat="server" ID="Label45" Text="Valor"></asp:Label>
                                                                        <asp:TextBox runat="server" id="txtValorProduto" cssClass="form-control" ></asp:TextBox>
                                                                        </div>
                                                                    </div>   
                                                                    <div class="col-sm-2">
                                                                        <br />
                                                                        <asp:Button runat="server" ID="btnIncluirProduto" CssClass="btn btn-sm btn-whitelabel1" 
                                                                            Text="Incluir Produto" onclick="btnIncluirProduto_Click" />
                                                                    </div>

                                                                    <div class="col-sm-12">

                                    
                                    
                                    
                                                                    </div>


                                                                    <div class="col-sm-12" runat="server" id="divProdutosInclusao" visible="false">

                                                                        <table id="Table3" class="table table-bordered table-hover">
                                                                            <thead>
                                                                            <tr>
                                                                                <th>ID</th>
                                                                                <th>Produto</th>
                                                                                <th>Qtde</th>
                                                                                <th>Valor</th>
                                                                                <th>Excluir</th>
                                                                            </tr>
                                                                            </thead>
                                                                            <tbody>


                                                                                <asp:ListView ID="lsvProdutos" runat="server" 
                                                                                    OnItemCommand="lsvProdutos_ItemCommand">
                                                                                    <ItemTemplate>
                                                                                            <tr>
                                                                                                <td>
                                                                                                    <small><asp:Label ID="lblIDProduto" runat="server" Text='<%# Eval("id") %>'></asp:Label></small>
                                                                                                    <asp:TextBox runat="server" ID="txtIDProduto" Text='<%# Eval("id") %>' Visible="false"></asp:TextBox>
                                                                                                </td>
                                                                                                <td>
                                                                                                    <small><asp:Label ID="Label6" runat="server" Text='<%# Eval("produto") %>'></asp:Label></small>
                                                                                                    <asp:TextBox runat="server" ID="txtProduto" Text='<%# Eval("produto") %>' Visible="false"></asp:TextBox>
                                                                                                </td>
                                                                                                <td>
                                                                                                    <small><asp:Label ID="Label16" runat="server" Text='<%# Eval("qtde") %>'></asp:Label></small>
                                                                                                    <asp:TextBox runat="server" ID="txtQtde" Text='<%# Eval("qtde") %>' Visible="false"></asp:TextBox>
                                                                                                </td>
                                                                                                <td>
                                                                                                    <small><asp:Label ID="Label17" runat="server" Text='<%# Eval("valor") %>'></asp:Label></small>
                                                                                                    <asp:TextBox runat="server" ID="txtValor" Text='<%# Eval("valor") %>' Visible="false"></asp:TextBox>
                                                                                                </td>
                                                                                                <td>
                                                                                                    <asp:linkbutton ID="lkbExcluir" CssClass="" commandname="Excluir" runat="server" text="Excluir" ToolTip="Excluir" CommandArgument='<%# Eval("id") %>' ><i class="fas fa-trash text-danger"></i></asp:linkbutton>
                                                                                                </td>
                                                                                            </tr>
                                                                                    </ItemTemplate>
                                                                                </asp:ListView>


                                                                            </tbody>
                                                                        </table>
                                    
                                                                    </div>

                                    
                                                                </div>


                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>

                                                <div class="row">
                                                    <div class="col-sm-12">

                                                        <div class="card">
                                                            <div class="card-header bg-whitelabel1">
                                                                <h4 class="card-title">
                                                                <i class="fas fa-envelope"></i>     Imagem
                                                                </h4>
                                                            </div>
                                                            <div class="card-body">

                                                                <div class="form-group">
                                                                    <asp:Label runat="server" ID="Label34" Text="Imagem Destaque Check-Out"></asp:Label>
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
                                                    </div>
                                                </div>


                                            </div>
                                        </div>
                                        
                                        
                                        
                                        
                                        <div class="row" runat="server" id="divLinkPagamentoResultado" visible="false">
                                            <div class="col-sm-12">
                                                <div class="form-group">
                                                    <asp:Label runat="server" ID="Label29" Text="Endereço do link de pagamento"></asp:Label>
                                                    <asp:TextBox runat="server" ID="txtUrlLinkPagamento" CssClass="form-control"></asp:TextBox>
                                                </div>
                                            </div>

                                            <div class="col-sm-12">
                                                <div class="form-group">
                                                    <a href="" runat="server" id="hrfUrlLinkPagamento" class="btn btn-block btn-whitelabel1" target="_blank"><asp:Label runat="server" ID="Label32" Text="CLIQUE AQUI PARA ABRIR LINK DE PAGAMENTO"></asp:Label></a>
                                                </div>
                                            </div>
                                        </div>

                                    </div>
                                    <div class="card-footer">
                                        <asp:Button runat="server" ID="btnContinuarLinkPagamento" 
                                            Text="Confirmar link pagamento" CssClass="btn btn-whitelabel1" 
                                            onclick="btnContinuarLinkPagamento_Click" />
                                        <asp:Button runat="server" ID="btnVoltarLinkPagamento" Text="Voltar" 
                                            CssClass="btn btn-whitelabel1"  Visible="false" 
                                            onclick="btnVoltarLinkPagamento_Click" />
                                    </div>
                                </div>


                                <!-- Cobrança Venda Digitada -->
                                <!-- ********************************************************************************************************************************************************** -->
                                <div class="card card-outline col-12" runat="server" id="divVendaDigitada" visible="false">
                                    <div class="card-body">
                                        <div class="row">
                                            <div class="col-12">
                                                <div class="row">
                                                    <div class="col-sm-12">
                                                        <h5 class="text-whitelabel1">Preencha os dados referente ao cartão de crédito</h5>
                                                    </div>
                                                </div>

                                                <div class="row">
                                                    <div class="col-sm-3">
                                                        <div class="form-group">
                                                        <asp:Label runat="server" ID="Label1" Text="Número Cartão"></asp:Label>
                                                            <asp:TextBox runat="server" ID="txtNumeroCartao" CssClass="form-control"></asp:TextBox>
                                                        </div>
                                                    </div>
                                                    <div class="col-sm-3">
                                                        <div class="form-group">
                                                        <asp:Label runat="server" ID="Label2" Text="Nome Portador"></asp:Label>
                                                            <asp:TextBox runat="server" ID="txtPortador" CssClass="form-control"></asp:TextBox>
                                                        </div>
                                                    </div>
                                                    <div class="col-sm-2">
                                                        <div class="form-group">
                                                        <asp:Label runat="server" ID="Label3" Text="Mês Vencto"></asp:Label>
                                                            <asp:TextBox runat="server" ID="txtMes" CssClass="form-control"></asp:TextBox>
                                                        </div>
                                                    </div>
                                                    <div class="col-sm-2">
                                                        <div class="form-group">
                                                        <asp:Label runat="server" ID="Label4" Text="Ano Vencto"></asp:Label>
                                                            <asp:TextBox runat="server" ID="txtAno" CssClass="form-control"></asp:TextBox>
                                                        </div>
                                                    </div>
                                                    <div class="col-sm-2">
                                                        <div class="form-group">
                                                        <asp:Label runat="server" ID="Label16" Text="CVV"></asp:Label>
                                                            <asp:TextBox runat="server" ID="txtCVV" CssClass="form-control"></asp:TextBox>
                                                        </div>
                                                    </div>
                                                </div>
                                                <div class="row">
                                                    <div class="col-sm-3">
                                                        <div class="form-group">
                                                        <asp:Label runat="server" ID="Label17" Text="Forma Pagto"></asp:Label>
                                                            <asp:DropDownList runat="server" ID="ddlFormaPagamento" CssClass="form-control" 
                                                                AutoPostBack="True" 
                                                                onselectedindexchanged="ddlFormaPagamento_SelectedIndexChanged">
                                                                <asp:ListItem Value="V" Text="À Vista"></asp:ListItem>
                                                                <asp:ListItem Value="P" Text="Parcelada"></asp:ListItem>
                                                            </asp:DropDownList>
                                                        </div>
                                                    </div>
                                                    <div class="col-sm-3" runat="server" id="dvParcelado">
                                                        <div class="form-group">
                                                        <asp:Label runat="server" ID="Label18" Text="Parcelas"></asp:Label>
                                                            <asp:DropDownList runat="server" ID="ddlParcelas" CssClass="form-control">
                                                                <asp:ListItem Value="1" Text="1x"></asp:ListItem>
                                                                <asp:ListItem Value="2" Text="2x"></asp:ListItem>
                                                                <asp:ListItem Value="3" Text="3x"></asp:ListItem>
                                                                <asp:ListItem Value="4" Text="4x"></asp:ListItem>
                                                                <asp:ListItem Value="5" Text="5x"></asp:ListItem>
                                                                <asp:ListItem Value="6" Text="6x"></asp:ListItem>
                                                                <asp:ListItem Value="7" Text="7x"></asp:ListItem>
                                                                <asp:ListItem Value="8" Text="8x"></asp:ListItem>
                                                                <asp:ListItem Value="9" Text="9x"></asp:ListItem>
                                                                <asp:ListItem Value="10" Text="10x"></asp:ListItem>
                                                                <asp:ListItem Value="11" Text="11x"></asp:ListItem>
                                                                <asp:ListItem Value="12" Text="12x"></asp:ListItem>
                                                            </asp:DropDownList>
                                                        </div>
                                                    </div>


                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="card-footer">
                                        <asp:Button runat="server" ID="btnContinuarVendaDigitada" 
                                            Text="Confirmar Venda Digitada" CssClass="btn btn-whitelabel1" 
                                            onclick="btnContinuarVendaDigitada_Click" />
                                        <asp:Button runat="server" ID="btnVoltarVendaDigitada" Text="Voltar" CssClass="btn btn-whitelabel1"  Visible="false" />
                                    </div>
                                </div>


                                <!-- Carne de pagamento -->
                                <!-- ********************************************************************************************************************************************************** -->
                                <div class="card card-outline col-12" runat="server" id="divCarne" visible="false">
                                    <div class="card-body">
                                        <div class="row">
                                            <div class="col-sm-12">
                                                <h5 class="text-whitelabel1">Carnê de pagamento</h5>
                                            </div>
                                        </div>

                                        <div class="row">
                                            <div class="col-12">
                                            </div>
                                        </div>

                                    </div>
                                    <div class="card-footer">
                                        <asp:Button runat="server" ID="btnContinuarCarne" 
                                            Text="Confirmar carnê pagamento" CssClass="btn btn-whitelabel1" 
                                            onclick="btnContinuarCarne_Click" />
                                        <asp:Button runat="server" ID="btnVoltarCarne" Text="Voltar" CssClass="btn btn-whitelabel1"  Visible="false" />
                                    </div>
                                </div>


                                <!-- Assinatura recorrente -->
                                <!-- ********************************************************************************************************************************************************** -->
                                <div class="card card-outline col-12" runat="server" id="divAssinatura" visible="false">
                                    <div class="card-body">
                                        <div class="row">
                                            <div class="col-sm-3">
                                                <div class="form-group">
                                                <asp:Label runat="server" ID="Label30" Text="Forma Pagto"></asp:Label>
                                                    <asp:DropDownList runat="server" ID="ddlPeriodicidade" CssClass="form-control" 
                                                        AutoPostBack="True">
                                                        <asp:ListItem Value="WEEKLY" Text="Semanal"></asp:ListItem>
                                                        <asp:ListItem Value="MONTHLY" Text="Mensal"></asp:ListItem>
                                                        <asp:ListItem Value="YEARLY" Text="Anual"></asp:ListItem>
                                                    </asp:DropDownList>
                                                </div>
                                            </div>
                                            <div class="col-sm-3">
                                                <div class="form-group">
                                                    <asp:Label runat="server" ID="Label31" Text="Qtde. Cobranças"></asp:Label>
                                                    <asp:TextBox runat="server" ID="txtQtdeCobranca" CssClass="form-control"></asp:TextBox>
                                                </div>
                                            </div>
                                            <div class="col-sm-2">
                                                <div class="form-group">
                                                    <asp:Label runat="server" ID="Label25" Text="Data Limite"></asp:Label>
                                                    <div class="input-group date" id="dpDataLimite" data-target-input="nearest">
								                        <asp:TextBox ID="txtDataLimite" runat="server" CssClass="form-control datetimepicker-input" data-target="#dpDataLimite"></asp:TextBox>
                                                        <div class="input-group-append" data-target="#dpDataLimite" data-toggle="datetimepicker">
                                                            <div class="input-group-text"><i class="fa fa-calendar"></i></div>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="col-sm-3">
                                                <div class="form-group">
                                                    <asp:Label runat="server" ID="Label46" Text="Retorno"></asp:Label>
                                                    <asp:TextBox runat="server" ID="txtRetornoAssinatura" CssClass="form-control"></asp:TextBox>
                                                </div>
                                            </div>

                                        </div>
                                        <div class="row">
                                            <div class="col-12">
                                            

                                            </div>
                                        </div>

                                    </div>
                                    <div class="card-footer">
                                        <asp:Button runat="server" ID="btnContinuarAssinatura" 
                                            Text="Confirmar assinatura recorrente" CssClass="btn btn-whitelabel1" 
                                            onclick="btnContinuarAssinatura_Click" />
                                        <asp:Button runat="server" ID="btnVoltarAssinatura" Text="Voltar" 
                                            CssClass="btn btn-whitelabel1"  Visible="false" 
                                            onclick="btnVoltarAssinatura_Click" />
                                    </div>
                                </div>


                              </div>
                              <div class="card-footer">

                              </div>
                            </div>
                          </div>
                        </div>

                        <!-- Fim  ***************************************************************************************************************************************************** -->

                    </div>
                </section>
            </div>
            <PORTAL:PAGEBOTTOM id="Pageheader3" title="Site Directory" runat="server" ModuleSource="rodapepadrao.ascx"></PORTAL:PAGEBOTTOM><!-- Fim Rodapé da Pagina -->
    
        </div>
    </form>

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

        //Date range picker
        $('#datepickerIni').datetimepicker({
            format: 'DD/MM/YYYY'
        });
        $('#datepickerFim').datetimepicker({
            format: 'DD/MM/YYYY'
        });

        $('#datepickerIniSimulador').datetimepicker({
            format: 'DD/MM/YYYY'
        });
        $('#datepickerFimSimulador').datetimepicker({
            format: 'DD/MM/YYYY'
        });

        $('#dpVencimento').datetimepicker({
            format: 'DD/MM/YYYY'
        });
        $('#dpDataLimite').datetimepicker({
            format: 'DD/MM/YYYY'
        });

    })
</script>        


<script type="text/javascript">

$(document).ready(function () {
    var table = $('#tabConsulta').DataTable({
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

$(document).ready(function () {
    var table = $('#tblParcial').DataTable({
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

<script type="text/javascript">
    function myFunctionChavePix() {
        var copyText = document.getElementById("lblPixCopiaCola");
        copyText.select();
        copyText.setSelectionRange(0, 99999); // For mobile devices
        navigator.clipboard.writeText(copyText.value);
        alert("Pix: (" + copyText.value + ") copiado com sucesso!");
    }

</script>


</body>
</html>
