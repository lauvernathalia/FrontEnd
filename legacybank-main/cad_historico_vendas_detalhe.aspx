<%@ Page Language="C#" AutoEventWireup="true" CodeFile="cad_historico_vendas_detalhe.aspx.cs" Inherits="cad_historico_vendas_detalhe" %>

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

        <div class="row">
            <div class="col-12">

                        <div class="card m-2">
                            <div class="card-body">
                                <h3 class="modal-title text-black text-center"><b class="text-whitelabel1">DETALHE</b> DA VENDA</h3>

                                <!-- PARTE 1 -->
                                <asp:Repeater runat="server" ID="rptTransacao">
                                    <ItemTemplate>

                                        <h4 class="text-center"><%# DataBinder.Eval(Container.DataItem, "NOM_STATUS")%></h4>

                                        <h6 class="text-center"><b>Criada em <%# DataBinder.Eval(Container.DataItem, "DTA_DATA")%></b></h6>
                                        </center>
                                        <div class="row">
                                            <div class="col-md-6 col-xs-12 my-1">
                                                <h6><b>Valor da Venda</b></h6>
                                                <h6 class="text-whitelabel1">R$ <%# String.Format("{0:n2}",DataBinder.Eval(Container.DataItem, "NUM_VALOR_BRUTO"))%></h6>
                                            </div>
                                            <div class="col-md-6 col-xs-12 my-1">
                                                <h6><b>Valor Líquido</b></h6>
                                                <h6 class="text-whitelabel1">R$ <%# String.Format("{0:n2}",DataBinder.Eval(Container.DataItem, "NUM_VALOR_LIQUIDO"))%></h6>
                                            </div>
                                        </div>
                                        <div class="row">
                                            <div class="col-md-6 col-xs-12 my-1">
                                                <h6><b>Taxa de Venda</b></h6>
                                                <h6 class="text-whitelabel1">R$ <%# String.Format("{0:n2}", DataBinder.Eval(Container.DataItem, "NUM_VALOR_TOTAL_TAXAS"))%></h6>
                                            </div>
                                            <div class="col-md-6 col-xs-12 my-1">
                                                <h6><b>Taxa de Antecipação</b></h6>
                                                <h6 class="text-whitelabel1">R$ 0,00</h6>
                                            </div>
                                        </div>
                                        <h6><b>Tipo da Venda</b></h6>
                                        <h6><%# DataBinder.Eval(Container.DataItem, "NOM_TIPO_PAGAMENTO")%></h6>
                                        <h6><b>ID da Transação</b></h6>
                                        <h6><%# DataBinder.Eval(Container.DataItem, "NOM_CODE")%></h6>
                                        <asp:TextBox runat="server" ID="txtJson" TextMode="MultiLine" CssClass="form-control" Rows="10" Text='<%# DataBinder.Eval(Container.DataItem, "DES_JSON")%>' Visible="false"></asp:TextBox>



                                    </ItemTemplate>
                                </asp:Repeater>

                                <!-- Caixa de Recolher -->

                                <div class="row">
                                    <asp:TextBox runat="server" ID="txtIDTransacao" Visible="false"></asp:TextBox>

                                    <div class="col-md-4 col-xs-12 my-1" runat="server" id="dvEstornoVendasCartao" visible="false">
                                        <a href="#" class="btn btn-danger btn-sm btn-block"  data-toggle="collapse" data-target="#ddestornarvenda">Estornar Venda</a>
                                        <div id="ddestornarvenda" class="collapse">
                                            <div class="card">
                                                <div class="card-body">
                                                    <div class="form-group">
                                                        <b><asp:Label runat="server" id="lblValorEstornar" Text="Valor à estornar"></asp:Label></b><strong class="text-whitelabel1">*</strong> 
                                                        <asp:TextBox runat="server" id="txtValorEstornar" cssClass="form-control"></asp:TextBox>
                                                    </div>
                                                </div>
                                                <div class="card-footer">
                                                    <asp:Button runat="server" ID="btnEstornarVenda" Text="Estornar" 
                                                        CssClass="btn btn-sm btn-whitelabel1" onclick="btnEstornarVenda_Click" />
                                                </div>
                                            </div>

                                        </div>
                                    </div>

                                    <div class="col-md-4 col-xs-12 my-1" runat="server" id="dvEstornoVendasBoleto" visible="false">
                                        <a href="#" class="btn btn-danger btn-sm btn-block"  data-toggle="collapse" data-target="#ddcancelarboleto">Cancelar Boleto</a>
                                        <div id="ddcancelarboleto" class="collapse">
                                            <div class="card">
                                                <div class="card-body">
                                                    <div class="form-group">
                                                        <b><asp:Label runat="server" id="lblValorCancelar" Text="Valor à cancelar"></asp:Label></b><strong class="text-whitelabel1">*</strong> 
                                                        <asp:TextBox runat="server" id="txtValorCancelar" cssClass="form-control"></asp:TextBox>
                                                    </div>
                                                </div>
                                                <div class="card-footer">
                                                    <asp:Button runat="server" ID="btnCancelarBoleto" Text="Cancelar Boleto"  
                                                        CssClass="btn btn-sm btn-whitelabel1" onclick="btnCancelarBoleto_Click" />
                                                </div>
                                            </div>
                                        </div>
                                    </div>

                                    <div class="col-md-4 col-xs-12 my-1" runat="server" id="dvReciboEmail" visible="false">
                                        <a href="#" class="btn btn-whitelabel1 btn-sm btn-block"  data-toggle="collapse" data-target="#ddreciboemail">Recibo por e-mail</a>
                                        <div id="ddreciboemail" class="collapse">
                                            <div class="card">
                                                <div class="card-body">
                                                    <div class="form-group">
                                                        <b><asp:Label runat="server" id="lblEmailRecibo" Text="E-Mail"></asp:Label></b><strong class="text-whitelabel1">*</strong> 
                                                        <asp:TextBox runat="server" id="txtEmailrecibo" cssClass="form-control"></asp:TextBox>
                                                    </div>
                                                </div>
                                                <div class="card-footer">
                                                    <asp:Button runat="server" ID="btnReciboEmail" Text="Enviar" CssClass="btn btn-sm btn-whitelabel1" />
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="col-md-4 col-xs-12 my-1" runat="server" id="dvCancelamento" visible="false">
                                        <a href="#" class="btn btn-danger btn-sm btn-block"  data-toggle="collapse" data-target="#ddcartacancelamento">Carta de Cancelamento</a>
                                        <div id="ddcartacancelamento" class="collapse">
                                            <div class="card">
                                                <div class="card-body">
                                                    <div class="form-group">
                                                        <b><asp:Label runat="server" id="Label1" Text="Link da Carta Cancelamento"></asp:Label></b><strong class="text-whitelabel1">*</strong> 
                                                        <asp:TextBox runat="server" id="txtCartaCancelamento" cssClass="form-control"></asp:TextBox>
                                                    </div>
                                                </div>
                                                <div class="card-footer">
                                                    <a href="" runat="server" id="hrCartaCancelamento" class="btn btn-sm btn-whitelabel1" target="_blank">Abrir Carta de Cancelamento</a>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>


                                <hr />
                                <!-- Dados da Venda -->
                                <div class="row">
                                    <div class="col-md-11 col-xs-12 my-1">
                                        <h3 class="modal-title text-black"><b class="text-whitelabel1">Dados</b> da Venda</h3>
                                    </div>
                                    <div class="col-md-1 col-xs-12 my-1">
                                        <h3><a href="#" data-toggle="collapse" data-target="#ddvenda"><i class="fas fa-arrow-circle-down text-whitelabel1"></i></a></h3>
                                    </div>
                                </div>

                                
                                <div id="ddvenda" class="collapse">

                                    <div runat="server" id="dvPix" visible="false">
                                        <div class="row">
                                            <div class="col-md-12 col-xs-12 my-1">
                                                <h6><b>Código</b></h6>
                                                <h6 class="text-whitelabel1"><asp:Label runat="server" ID="lblPixCodigo"></asp:Label></h6>
                                            </div>
                                        </div>                                    
                                    
                                        <div class="row">
                                            <div class="col-md-12 col-xs-12 my-1">
                                                <h6><b>Descrição</b></h6>
                                                <h6 class="text-whitelabel1"><asp:Label runat="server" ID="lblPixDescricao"></asp:Label></h6>
                                            </div>
                                        </div>                                    
                                    
                                    </div>


                                    <div runat="server" id="dvCredito" visible="false">
                                        <div class="row">
                                            <div class="col-md-6 col-xs-12 my-1">
                                                <h6><b>Portador</b></h6>
                                                <h6 class="text-whitelabel1"><asp:Label runat="server" ID="lblCreditoPortador"></asp:Label></h6>
                                            </div>
                                            <div class="col-md-6 col-xs-12 my-1">
                                                <h6><b>Modo de Captura</b></h6>
                                                <h6 class="text-whitelabel1"><asp:Label runat="server" ID="lblCreditoModoCaptura"></asp:Label></h6>
                                            </div>
                                        </div>
                                        <div class="row">
                                            <div class="col-md-6 col-xs-12 my-1">
                                                <h6><b>Número do Cartão</b></h6>
                                                <h6 class="text-whitelabel1"><asp:Label runat="server" ID="lblCreditoNumero"></asp:Label></h6>
                                            </div>
                                            <div class="col-md-6 col-xs-12 my-1">
                                                <h6><b>Validade do Cartão</b></h6>
                                                <h6 class="text-whitelabel1"><asp:Label runat="server" ID="lblCreditoValidade"></asp:Label></h6>
                                            </div>
                                        </div>
                                        <div class="row">
                                            <div class="col-md-12 col-xs-12 my-1">
                                                <h6><b>Código de Autorização</b></h6>
                                                <h6 class="text-whitelabel1"><asp:Label runat="server" ID="lblCreditoAutorizacao"></asp:Label></h6>
                                            </div>
                                        </div>
                                    </div>



                                    <div runat="server" id="dvDebito" visible="false">
                                        <div class="row">
                                            <div class="col-md-6 col-xs-12 my-1">
                                                <h6><b>Portador</b></h6>
                                                <h6 class="text-whitelabel1"><asp:Label runat="server" ID="lblDebitoPortador"></asp:Label></h6>
                                            </div>
                                            <div class="col-md-6 col-xs-12 my-1">
                                                <h6><b>Modo de Captura</b></h6>
                                                <h6 class="text-whitelabel1"><asp:Label runat="server" ID="lblDebitoModoCaptura"></asp:Label></h6>
                                            </div>
                                        </div>
                                        <div class="row">
                                            <div class="col-md-6 col-xs-12 my-1">
                                                <h6><b>Número do Cartão</b></h6>
                                                <h6 class="text-whitelabel1"><asp:Label runat="server" ID="lblDebitoNumero"></asp:Label></h6>
                                            </div>
                                            <div class="col-md-6 col-xs-12 my-1">
                                                <h6><b>Validade do Cartão</b></h6>
                                                <h6 class="text-whitelabel1"><asp:Label runat="server" ID="lblDebitoValidade"></asp:Label></h6>
                                            </div>
                                        </div>
                                        <div class="row">
                                            <div class="col-md-12 col-xs-12 my-1">
                                                <h6><b>Código de Autorização</b></h6>
                                                <h6 class="text-whitelabel1"><asp:Label runat="server" ID="lblDebitoAutorizacao"></asp:Label></h6>
                                            </div>
                                        </div>                                    
                                    </div>


                                    <div runat="server" id="dvBoleto" visible="false">
                                        <div class="row">
                                            <div class="col-md-12 col-xs-12 my-1">
                                                <h6><b>Código de Barras</b></h6>
                                                <h6 class="text-whitelabel1"><asp:Label runat="server" ID="lblBoletoCodigoBarras"></asp:Label></h6>
                                            </div>
                                        </div>                                    
                                    
                                        <div class="row">
                                            <div class="col-md-12 col-xs-12 my-1">
                                                <a href="" runat="server" id="hrfBoleto" target="_blank" class="btn btn-sm btn-whitelabel1">2ª Via Boleto</a>
                                            </div>
                                        </div>                                    

                                        <div class="row">
                                            <div class="col-md-6 col-xs-12 my-1">
                                                <h6><b>Vencimento</b></h6>
                                                <h6 class="text-whitelabel1"><asp:Label runat="server" ID="lblBoletoVencimento"></asp:Label></h6>
                                            </div>
                                            <div class="col-md-6 col-xs-12 my-1">
                                                <h6><b>Número Documento</b></h6>
                                                <h6 class="text-whitelabel1"><asp:Label runat="server" ID="lblBoletoNumero"></asp:Label></h6>
                                            </div>

                                        </div>                                    

                                    
                                    </div>


                                </div>


                                <hr />
                                <!-- Dados do Terminal -->
                                <div class="row">
                                    <div class="col-md-11 col-xs-12 my-1">
                                        <h3 class="modal-title text-black"><b class="text-whitelabel1">Dados</b> do Terminal</h3>
                                    </div>
                                    <div class="col-md-1 col-xs-12 my-1">
                                        <h3><a href="#" data-toggle="collapse" data-target="#ddterminal"><i class="fas fa-arrow-circle-down text-whitelabel1"></i></a></h3>
                                    </div>
                                </div>

                                <div id="ddterminal" class="collapse">

                                        <div class="row">
                                            <div class="col-md-4 col-xs-12 my-1">
                                                <h6><b>Serial</b></h6>
                                                <h6 class="text-whitelabel1"><asp:Label runat="server" ID="lblSerialTerminal"></asp:Label></h6>
                                            </div>
                                            <div class="col-md-4 col-xs-12 my-1">
                                                <h6><b>Código</b></h6>
                                                <h6 class="text-whitelabel1"><asp:Label runat="server" ID="lblCodigoTerminal"></asp:Label></h6>
                                            </div>
                                            <div class="col-md-4 col-xs-12 my-1">
                                                <h6><b>Modelo</b></h6>
                                                <h6 class="text-whitelabel1"><asp:Label runat="server" ID="lblModeloTerminal"></asp:Label></h6>
                                            </div>
                                        </div>
                                </div>
                                <hr />
                                <!-- Dados do Recibo -->
                                <div class="row">
                                    <div class="col-md-11 col-xs-12 my-1">
                                        <h3 class="modal-title text-black"><b class="text-whitelabel1">Dados</b> do Recibo</h3>
                                    </div>
                                    <div class="col-md-1 col-xs-12 my-1">
                                        <h3><a href="#" data-toggle="collapse" data-target="#ddrecibo"><i class="fas fa-arrow-circle-down text-whitelabel1"></i></a></h3>
                                    </div>
                                </div>

                                <div id="ddrecibo" class="collapse">

                                        <div class="row d-flex justify-content-center">
                                            <div class="col-md-4 col-xs-12 my-1  border border-dark">
                                                <div class="row" runat="server" id="dvReciboCliente">
                                                    <center>
                                                    <asp:Image runat="server" ID="imgCliente" CssClass="img-fluid col-10  p-2"/>
                                                    </center>
                                                    <div class="col-12">
                                                        <h6 class="text-whitelabel1 text-center"><asp:Label runat="server" ID="lblReciboCliente"></asp:Label></h6>
                                                    </div>
                                                </div>
                                                <center>
                                                <button class="btn btn-whitelabel1" onclick="printDiv('dvReciboCliente')"><i class="fas fa-print" aria-hidden="true" style="font-size: 17px;">     Imprimir</i></button>                                                
                                                </center>
                                            </div>
                                            <div class="col-md-2 col-xs-12 my-1">
                                            </div>
                                            <div class="col-md-4 col-xs-12 my-1  border border-dark">

                                                <div class="row" runat="server" id="dvReciboEstabelecimento">
                                                    <center>
                                                    <asp:Image runat="server" ID="imgEstabelecimento" CssClass="img-fluid col-10 p-2" />
                                                    </center>
                                                    <div class="col-12">
                                                        <h6 class="text-whitelabel1 text-center"><asp:Label runat="server" ID="lblReciboEstabelecimento"></asp:Label></h6>
                                                    </div>
                                                </div>
                                                <center>
                                                <button class="btn btn-whitelabel1" onclick="printDiv('dvReciboEstabelecimento')"><i class="fas fa-print" aria-hidden="true" style="font-size: 17px;">     Imprimir</i></button>                                                
                                                </center>

                                            </div>

                                        </div>
                                </div>


                                <hr />
                                <!-- Dados do Vendedor -->
                                <div class="row">
                                    <div class="col-md-11 col-xs-12 my-1">
                                        <h3 class="modal-title text-black"><b class="text-whitelabel1">Dados</b> do Vendedor</h3>
                                    </div>
                                    <div class="col-md-1 col-xs-12 my-1">
                                        <h3><a href="#" data-toggle="collapse" data-target="#ddvendedor"><i class="fas fa-arrow-circle-down text-whitelabel1"></i></a></h3>
                                    </div>
                                </div>

                                <div id="ddvendedor" class="collapse">

                                        <div class="row">
                                            <div class="col-md-6 col-xs-12 my-1">
                                                <h6><b>Nome do Vendedor</b></h6>
                                                <h6 class="text-whitelabel1"><asp:Label runat="server" ID="lblNomeVendedor"></asp:Label></h6>
                                            </div>
                                            <div class="col-md-6 col-xs-12 my-1">
                                                <h6><b>CNPJ/CPF Vendedor</b></h6>
                                                <h6 class="text-whitelabel1"><asp:Label runat="server" ID="lblDocumentoVendedor"></asp:Label></h6>
                                            </div>
                                        </div>

                                        <div class="row">
                                            <div class="col-md-12 col-xs-12 my-1">
                                                <h6><b>ID Vendedor</b></h6>
                                                <h6 class="text-whitelabel1"><asp:Label runat="server" ID="lblIDVendedor"></asp:Label></h6>
                                            </div>
                                        </div>


                                        <div class="row">
                                            <div class="col-md-12 col-xs-12 my-1">
                                                <h6><b>Endereço Vendedor</b></h6>
                                                <h6 class="text-whitelabel1"><asp:Label runat="server" ID="lblEnderecoVendedor"></asp:Label></h6>
                                            </div>
                                        </div>


                                </div>


                                <hr />
                                <!-- Historico da Venda -->
                                <div class="row">
                                    <div class="col-md-11 col-xs-12 my-1">
                                        <h3 class="modal-title text-black"><b class="text-whitelabel1">Histórico</b> da Venda</h3>
                                    </div>
                                    <div class="col-md-1 col-xs-12 my-1">
                                        <h3><a href="#" data-toggle="collapse" data-target="#ddhistorico"><i class="fas fa-arrow-circle-down text-whitelabel1"></i></a></h3>
                                    </div>
                                </div>

                                
                                <div id="ddhistorico" class="collapse">

                                        <table id="Table3" class="table table-bordered table-hover">
                                            <thead>
                                            <tr>
                                                <th>Operação</th>
                                                <th>Status</th>
                                                <th>Descrição</th>
                                                <th>Data</th>
                                                <th>Valor</th>
                                            </tr>
                                            </thead>
                                            <tbody>
                                                <asp:Repeater runat="server" ID="rptHistorico">
                                                    <ItemTemplate>
                                                        <tr>
                                                            <td><small><b><%# DataBinder.Eval(Container.DataItem, "NOM_TIPO")%></b></small></td>
                                                            <td><small class='text-<%# DataBinder.Eval(Container.DataItem, "NOM_STATUS_COR")%>'><b><%# DataBinder.Eval(Container.DataItem, "NOM_STATUS")%></b></small></td>
                                                            <td><small><%# DataBinder.Eval(Container.DataItem, "NOM_DESCRICAO_RESPOSTA")%></small></td>

                                                            <td><small><%# String.Format("{0:dd/MM/yyyy}", DataBinder.Eval(Container.DataItem, "DTA_DATA_CRIACAO"))%></small></td>
                                                            <td><small><%# String.Format("{0:c2}", DataBinder.Eval(Container.DataItem, "NUM_VALOR"))%></small></td>
                                                        </tr>
                                                    </ItemTemplate>
                                                </asp:Repeater>

                                            </tbody>
                                        </table>                                    

                                </div>



                                <hr />
                                <!-- Historico da Venda -->
                                <div class="row">
                                    <div class="col-md-11 col-xs-12 my-1">
                                        <h3 class="modal-title text-black"><b class="text-whitelabel1">Recebimento</b> da Venda</h3>
                                    </div>
                                    <div class="col-md-1 col-xs-12 my-1">
                                        <h3><a href="#" data-toggle="collapse" data-target="#ddrecebimento"><i class="fas fa-arrow-circle-down text-whitelabel1"></i></a></h3>
                                    </div>
                                </div>
                                
                                <div id="ddrecebimento" class="collapse">

                                        <table id="Table1" class="table table-bordered table-hover">
                                            <thead>
                                            <tr>
                                                <th>Data</th>
                                                <th>Parcela</th>
                                                <th>Status</th>
                                                <th>Valor Líquido</th>
                                            </tr>
                                            </thead>
                                            <tbody>

                                                <asp:Repeater ID="rptRecebiveis" runat="server">
                                                    <ItemTemplate>
                                                        <tr>
                                                            <td><%# String.Format("{0:dd/MM/yyyy}", DataBinder.Eval(Container.DataItem, "DTA_DATA_CRIACAO"))%></td>
                                                            <td><small><%# DataBinder.Eval(Container.DataItem, "NUM_PARCELA")%></small></td>
                                                            <td><small class='text-<%# DataBinder.Eval(Container.DataItem, "NOM_STATUS_COR")%>'><b><%# DataBinder.Eval(Container.DataItem, "NOM_STATUS_DESCRICAO")%></b></small></td>
                                                            <td><small><%# String.Format("{0:c2}", DataBinder.Eval(Container.DataItem, "NUM_VALOR"))%></small></td>
                                                        </tr>
                                                    </ItemTemplate>
                                                </asp:Repeater>


                                            </tbody>
                                        </table>                                    

                                </div>
                            </div>

                            <div class="card-footer">
                                <asp:Button runat="server" ID="btnFechar" CssClass="btn btn-whitelabel1 float-right" Text="Fechar" onclick="btnFechar_Click"/>
                            </div>
                        </div>            

            
            </div>
        </div>
    </form>

<script type="text/javascript" src="../plugins/datatables/jquery.dataTables.js"></script>
<script type="text/javascript" src="../plugins/datatables-bs4/js/dataTables.bootstrap4.js"></script>

<script src="../plugins/inputmask/jquery.inputmask.bundle.js"></script>
<script src="../plugins/moment/moment.min.js"></script>


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
<script>

    function printDiv(divName) {
        var printContents = document.getElementById(divName).innerHTML;
        var originalContents = document.body.innerHTML;

        document.body.innerHTML = printContents;

        window.print();

        document.body.innerHTML = originalContents;
    }

</script>

</body>
</html>
