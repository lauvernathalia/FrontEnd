<%@ Page Language="C#" AutoEventWireup="true" CodeFile="cad_boletos_bancarios.aspx.cs" Inherits="cad_boletos_bancarios" MaintainScrollPositionOnPostback="true" %>

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


                <div class="card">
                    <div class="card-header bg-whitelabel1">
                        <h3 class="card-title">BOLETO BANCÁRIO</h3>
                    </div>
                    <div class="card-body">

                        <div class="row">
                            <div class="col-sm-6">
                                <div class="form-group">
                                    <label>Código</label>
                                    <asp:TextBox runat="server" id="txtCodigo" cssClass="form-control" Enabled="false"></asp:TextBox>
                                </div>
                            </div>

                        </div>

                        <div id="accordion">

                          <div class="card">
                            <div class="card-header bg-whitelabel1">
                              <h4 class="card-title text-white">
                                <a data-toggle="collapse" data-parent="#accordion" aria-expanded="false" href="#AbaCliente" class="text-white">
                                    <i class="fas fa-user text-white"></i>          Dados do Cliente
                                </a>
                              </h4>
                            </div>

                            <div id="AbaCliente" class="panel-collapse collapse show in">
                              <div class="card-body">

                                <div class="row">

                                    <div class="col-sm-12">
                                        <div class="form-group">
                                        <b><asp:Label runat="server" ID="lblFiltro" Text="Selecione o cliente desejado"></asp:Label></b>
                                        <asp:DropDownList runat="server" id="ddlComprador" CssClass="form-control" 
                                                AutoPostBack="True" onselectedindexchanged="ddlComprador_SelectedIndexChanged">
                                        </asp:DropDownList>
                                        </div>
                                    </div>

                                    <div class="col-sm-12">
                                        <div class="form-group">
                                            <b><asp:Label runat="server" ID="lbl901" Text="Ou digite os dados abaixo"></asp:Label></b>
                                        </div>
                                    </div>

                                    <div class="col-sm-3">
                                        <div class="form-group">
                                            <asp:Label runat="server" ID="lbl902" Text="Nome Completo"></asp:Label>
                                            <asp:TextBox id="txtIDCliente" runat="server" class="form-control" Visible="false"></asp:TextBox>
                                            <asp:TextBox id="txtIDZOOPCliente" runat="server" class="form-control" Visible="false"></asp:TextBox>
                                            <asp:TextBox id="txtTokenCliente" runat="server" class="form-control" Visible="false"></asp:TextBox>

                                            <asp:TextBox id="txtNomeCliente" runat="server" class="form-control"></asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="col-sm-3" style="display:none;">
                                        <div class="form-group">
                                            <asp:Label runat="server" ID="lbl903" Text="Sobrenome"></asp:Label>
                                            <asp:TextBox id="txtSobrenomeCliente"  runat="server" class="form-control" ></asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="col-sm-3">
                                        <div class="form-group">
                                            <asp:Label runat="server" ID="lbl904" Text="Documento (CNPJ/CPF)"></asp:Label>
                                            <asp:TextBox id="txtDocumentoCliente"  runat="server" class="form-control" ></asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="col-sm-3">
                                        <div class="form-group">
                                            <asp:Label runat="server" ID="lbl905" Text="E-mail"></asp:Label>
                                            <asp:TextBox id="txtEmailCliente" runat="server" class="form-control" ></asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="col-sm-3">
                                        <div class="form-group">
                                            <asp:Label runat="server" ID="lbl906" Text="Celular"></asp:Label>
                                            <asp:TextBox id="txtCelularCliente" runat="server" class="form-control" ></asp:TextBox>
                                        </div>
                                    </div>

                                    <div class="col-sm-12">
                                        <div class="form-group">
                                            <b><asp:Label runat="server" ID="Label5" Text="Endereço (opcional)"></asp:Label></b>
                                        </div>
                                    </div>


                                    <div class="col-sm-2"  >
                                        <div class="form-group">
                                        <asp:Label runat="server" ID="lbl907" Text="CEP"></asp:Label>
                                        <asp:TextBox runat="server" ID="txtCEPCliente" CssClass="form-control" 
                                                data-inputmask='"mask": "99999-999"' data-mask  
                                                AutoPostBack="True" ontextchanged="txtCEP_TextChanged"></asp:TextBox> 
                                        </div>
                                    </div>
                                    <div class="col-sm-4"  >
                                        <div class="form-group">
                                        <asp:Label runat="server" ID="lbl908" Text="Endereço"></asp:Label>
                                        <asp:TextBox runat="server" ID="txtEnderecoCliente" CssClass="form-control" ></asp:TextBox> 
                                        </div>
                                    </div>

                                    <div class="col-sm-3"  >
                                        <div class="form-group">
                                        <asp:Label runat="server" ID="lbl909" Text="Número"></asp:Label>
                                        <asp:TextBox runat="server" ID="txtNumeroCliente" CssClass="form-control" ></asp:TextBox> 
                                        </div>
                                    </div>
                                          
                                    <div class="col-sm-3"  >
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

                          <div class="card">
                            <div class="card-header bg-whitelabel1">
                              <h4 class="card-title">
                                <a data-toggle="collapse" data-parent="#accordion" href="#AbaCobranca" class="text-white">
                                  <i class="fas fa-dollar-sign"></i>          Dados de Cobrança
                                </a>
                              </h4>
                            </div>
                            <div id="AbaCobranca" class="panel-collapse collapse show in">
                              <div class="card-body">
                                <div class="row ">
                                    <div class="col-sm-3">
                                        <div class="form-group">
                                            <asp:Label runat="server" ID="Label22" Text="Valor total a cobrar"></asp:Label>
                                            <asp:TextBox runat="server" id="txtValor" cssClass="form-control" ></asp:TextBox>
                                        </div>
                                    </div>                                

                                    <div class="col-sm-3">
                                        <asp:Label runat="server" ID="Label1" Text="Vencimento"></asp:Label>						                
                                        <div class="input-group date" id="datepickerIni" data-target-input="nearest">
								            <asp:TextBox ID="txtVencimento" runat="server" CssClass="form-control datetimepicker-input" data-target="#datepickerIni"></asp:TextBox>
                                            <div class="input-group-append" data-target="#datepickerIni" data-toggle="datetimepicker">
                                                <div class="input-group-text"><i class="fa fa-calendar"></i></div>
                                            </div>
                                        </div>
                                    </div>    
                                    
                                                                
                                    <div class="col-sm-3">
                                        <div class="form-group">
                                        <asp:Label runat="server" ID="Label4" Text="Tipo de boleto"></asp:Label>
                                        <asp:DropDownList runat="server" id="ddlFormaRecebimento" CssClass="form-control">
                                            <asp:ListItem Value="B">Boleto Bancário com Código de Barras</asp:ListItem>
                                            <asp:ListItem Value="P">Boleto Bancário com Código de Barras e QR Code Pix</asp:ListItem>
                                        </asp:DropDownList>

                                        </div>
                                    </div>   
                                    <div class="col-sm-3">
                                        <div class="form-group">
                                            <asp:Label runat="server" ID="Label2" Text="Número Documento"></asp:Label>
                                            <asp:TextBox runat="server" id="txtNumeroDocumento" cssClass="form-control" ></asp:TextBox>
                                        </div>
                                    </div>       

                                </div>      
                                <div class="row">
                                    <div class="col-sm-12">
                                        <div class="form-group">
                                            <b><asp:Label CssClass="text-whitelabel1" runat="server" ID="Label12" Text="As próximas parcelas terão o mesmo dia de vencimento nos meses posteriores."></asp:Label></b>
                                        </div>
                                    </div>
                                </div>
                                <div class="row" runat="server" id="divParcelas" visible="false">
                                    <div class="col-sm-6">
                                        <div class="form-group">
                                            <asp:Label runat="server" ID="Label11" Text="Parcelas"></asp:Label>
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


                                <div class="row">
                                    <div class="col-sm-12">
                                        <div class="form-group">
                                            <b><asp:Label runat="server" ID="Label6" CssClass="text-whitelabel1" Text="Envio por e-mail - Nota:Você pode enviar para múltiplos e-mails (separe-os por vírgula)"></asp:Label></b>
                                        </div>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-sm-12">
                                        <div class="form-group">
                                        <asp:Label runat="server" ID="Label14" Text="Para quem você deseja enviar o link de pagamento?"></asp:Label>
                                        <asp:TextBox runat="server" id="txtEmails" cssClass="form-control" ></asp:TextBox>
                                        </div>
                                    </div>   
                                </div>


                              </div>
                            </div>
                          </div>


                          <div class="card">
                            <div class="card-header bg-whitelabel1">
                              <h4 class="card-title">
                                <a data-toggle="collapse" data-parent="#accordion" href="#AbaInformacoesAvancadas" class="text-white">
                                  <i class="fas fa-cogs"></i>          Informações Avançadas
                                </a>
                              </h4>
                            </div>
                            <div id="AbaInformacoesAvancadas" class="panel-collapse collapse show in">
                              <div class="card-body">

                                <div class="form-group row">
                                    <div class="icheck-primary d-inline col-2">
                                    <asp:CheckBox runat="server" id="ckbMulta" />
                                    <label for="ckbMulta">
                                        Habilitar Multa
                                    </label>
                                    </div>                                        
                                    <label for="inputEmail3" class="col-sm-2 control-label">Cobrar multa de</label>
                                    <div class="col-sm-2">
                                        <asp:DropDownList runat="server" id="ddlMulta" CssClass="form-control">
                                            <asp:ListItem Value="P">%</asp:ListItem>
                                            <asp:ListItem Value="V">Valor</asp:ListItem>
                                        </asp:DropDownList>
                                    </div>
                                    <div class="col-sm-2">
                                        <asp:TextBox runat="server" id="txtMulta" cssClass="form-control" ></asp:TextBox>
                                    </div>
                                    <label for="inputEmail3" class="col-sm-2 control-label">após vencimento.</label>
                                </div>

                                <div class="form-group row">
                                    <div class="icheck-primary d-inline col-2">
                                    <asp:CheckBox runat="server" id="ckbJuros"></asp:CheckBox>
                                    <label for="ckbJuros">
                                        Habilitar Juros
                                    </label>
                                    </div>                                        
                                    <label for="inputEmail3" class="col-sm-2 control-label">Cobrar juros de</label>
                                    <div class="col-sm-2">
                                        <asp:DropDownList runat="server" id="ddlJuros" CssClass="form-control">
                                            <asp:ListItem Value="VD">Valor Dia</asp:ListItem>
                                            <asp:ListItem Value="PD">% Dia</asp:ListItem>
                                            <asp:ListItem Value="PM">% Mês</asp:ListItem>
                                        </asp:DropDownList>
                                    </div>
                                    <div class="col-sm-2">
                                        <asp:TextBox runat="server" id="txtJuros" cssClass="form-control" ></asp:TextBox>
                                    </div>
                                    <label for="inputEmail3" class="col-sm-2 control-label">após vencimento.</label>

                                </div>
                                <div class="form-group row">
                                    
                                    <div class="icheck-primary d-inline col-2">
                                    <asp:CheckBox runat="server" id="ckbDesconto" />
                                    <label for="ckbDesconto">
                                        Habilitar Desconto
                                    </label>
                                    </div>                                        

                                    <label for="inputEmail3" class="col-sm-2 control-label">Desconto de</label>
                                    <div class="col-sm-2">
                                        <asp:DropDownList runat="server" id="ddlDesconto" CssClass="form-control">
                                            <asp:ListItem Value="P">%</asp:ListItem>
                                            <asp:ListItem Value="V">Valor</asp:ListItem>
                                        </asp:DropDownList>
                                    </div>
                                    <div class="col-sm-1">
                                        <asp:TextBox runat="server" id="txtDesconto" cssClass="form-control" ></asp:TextBox>
                                    </div>
                                    <label for="inputEmail3" class="col-sm-1 control-label">até</label>
                                    <div class="col-sm-1">
                                        <asp:TextBox runat="server" id="txtDias" cssClass="form-control" ></asp:TextBox>
                                    </div>
                                    <label for="inputEmail3" class="col-sm-3 control-label">dias antes do vencimento</label>


                                </div>

                                <div class="row ">
                                    <div class="col-sm-12">
                                        <div class="form-group">
                                        <asp:Label runat="server" ID="Label3" Text="Informações Adicionais (opcional)"></asp:Label>
                                        <asp:TextBox runat="server" id="txtAviso" cssClass="form-control" ></asp:TextBox>
                                        </div>
                                    </div>
                                </div>


                              </div>
                            </div>
                          </div>


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
                                              <label class="form-check-label">Habilitar configurações de split de valores</label>
                                            </div>
                                        </div>
                                    </div>   
                                </div>
                                <div class="row" runat="server" id="divSplit" visible="false">

                                    <div class="col-sm-12">
                                        <label class="col-sm-12 col-form-label">Parceiros</label>
                                        <div class="input-group">
                                            <div class="custom-file">
                                            <asp:DropDownList runat="server" id="ddlParceiros" CssClass="form-control" 
                                                    AutoPostBack="True" onselectedindexchanged="ddlParceiros_SelectedIndexChanged">
                                            </asp:DropDownList>
                                            </div>
                                            <div class="input-group-append">
                                            <asp:Button runat="server" ID="btnNovoParceiro" CssClass="btn btn-whitelabel1" Text="Novo Parceiro" onclick="btNovoParceiro_Click"/>
                                            </div>
                                        </div>
                                    </div>  
                                    

                                    <div class="col-sm-2">
                                        <div class="form-group">
                                        <asp:Label runat="server" ID="Label20" Text="ID"></asp:Label>
                                        <asp:Label runat="server" id="txtIDParceiro" cssClass="form-control" ></asp:Label>
                                        </div>
                                    </div>   
                                    <div class="col-sm-4">
                                        <div class="form-group">
                                        <asp:Label runat="server" ID="Label21" Text="Parceiro"></asp:Label>
                                        <asp:Label runat="server" id="txtParceiro" cssClass="form-control" ></asp:Label>
                                        </div>
                                    </div>                                
                                    <div class="col-sm-2">
                                        <div class="form-group">
                                        <asp:Label runat="server" ID="Label23" Text="Valor"></asp:Label>
                                        <asp:TextBox runat="server" id="txtValorParceiro" cssClass="form-control" ></asp:TextBox>
                                        </div>
                                    </div>   


                                    <div class="col-sm-2">
                                        <div class="form-group">
                                        <asp:Label runat="server" ID="Label24" Text="Percentual(%)"></asp:Label>
                                        <asp:TextBox runat="server" id="txtPercentualParceiro" cssClass="form-control" ></asp:TextBox>
                                        </div>
                                    </div>   
                                    <div class="col-sm-2">
                                        <br />
                                        <asp:Button runat="server" ID="btnIncluirParceiro" CssClass="btn btn-sm btn-whitelabel1" 
                                            Text="Incluir Parceiro" onclick="btnIncluirParceiro_Click" />
                                    </div>


                                    <div class="col-sm-12" runat="server" id="divParceirosInclusao" visible="false">

                                        <table id="Table4" class="table table-bordered table-hover">
                                            <thead>
                                            <tr>
                                                <th>ID</th>
                                                <th>Parceiro</th>
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
                                                                    <small><asp:Label ID="lblIDParceiro" runat="server" Text='<%# Eval("id") %>'></asp:Label></small>
                                                                    <asp:TextBox runat="server" ID="txtIDParceiro" Text='<%# Eval("id") %>' Visible="false"></asp:TextBox>
                                                                </td>
                                                                <td>
                                                                    <small><asp:Label ID="Label6" runat="server" Text='<%# Eval("parceiro") %>'></asp:Label></small>
                                                                    <asp:TextBox runat="server" ID="txtParceiro" Text='<%# Eval("parceiro") %>' Visible="false"></asp:TextBox>
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
                                                                    <asp:linkbutton ID="lkbExcluir" CssClass="" commandname="Excluir" runat="server" text="Excluir" ToolTip="Excluir" CommandArgument='<%# Eval("id") %>' ><i class="fas fa-trash text-danger"></i></asp:linkbutton>
                                                                </td>
                                                            </tr>
                                                    </ItemTemplate>
                                                </asp:ListView>


                                            </tbody>
                                        </table>
                                    
                                    </div>
                                    <div class="col-sm-12" runat="server" id="divParceirosAlteracao" visible="false">
 
                                        <table id="Table2" class="table table-bordered table-hover">
                                            <thead>
                                            <tr>
                                                <th>ID</th>
                                                <th>Parceiro</th>
                                                <th>Valor</th>
                                                <th>Percentual</th>
                                                <th>Excluir</th>
                                            </tr>
                                            </thead>
                                            <tbody>
                                            <asp:Repeater runat="server" ID="rptParceiros" onitemcommand="rptParceiros_ItemCommand" >
                                                <ItemTemplate>
                                                    <tr>
                                                        <td>
                                                            <asp:TextBox runat="server" ID="txtid" Text='<%# DataBinder.Eval(Container.DataItem, "COD_ID")%>' Visible="false"></asp:TextBox>
                                                            <small><%# DataBinder.Eval(Container.DataItem, "COD_ID_PARCEIRO")%></small>
                                                        </td>
                                                        <td>
                                                            <small><%# DataBinder.Eval(Container.DataItem, "NOM_NOME")%></small>
                                                        </td>
                                                        <td>
                                                            <small><%# String.Format("{0:n2}", DataBinder.Eval(Container.DataItem, "NUM_VALOR"))%></small>
                                                            <asp:TextBox runat="server" ID="txtValor" CssClass="form-control" Text='<%# DataBinder.Eval(Container.DataItem, "NUM_VALOR")%>' Visible="false"></asp:TextBox>
                                                        </td>
                                                        <td>
                                                            <small><%# String.Format("{0:n2}",DataBinder.Eval(Container.DataItem, "NUM_PERCENTUAL"))%></small>
                                                            <asp:TextBox runat="server" ID="txtPercentual" CssClass="form-control" Text='<%# DataBinder.Eval(Container.DataItem, "NUM_PERCENTUAL")%>' Visible="false"></asp:TextBox>
                                                        </td>
                                                        <td>
                                                            <asp:linkbutton ID="lkbExcluirParceiros" CssClass="" commandname="Excluir" runat="server" text="X"  ToolTip="Excluir" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "COD_ID")%>'><i class="fas fa-trash text-danger"></i></asp:linkbutton>
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



                          <div class="card" runat="server" id="divDadosBoletoBancario">
                            <div class="card-header bg-whitelabel1">
                              <h4 class="card-title">
                                  <i class="fas fa-barcode"></i>          Dados do Boleto Bancário
                              </h4>
                            </div>
                              <div class="card-body">
                                <div class="row ">
                                    <div class="col-sm-12">
                                        <div class="form-group">
                                            <asp:Label runat="server" ID="Label7" Text="Código Barras"></asp:Label>
                                            <asp:TextBox runat="server" id="txtCodigoBarras" cssClass="form-control" Enabled="false" ></asp:TextBox>
                                        </div>
                                    </div>                                
                                    <div class="col-sm-12">
                                        <div class="form-group">
                                            <asp:Label runat="server" ID="Label10" Text="Link Pagamento"></asp:Label>
                                            <asp:TextBox runat="server" id="txtLinkPagamento" cssClass="form-control" Enabled="false" ></asp:TextBox>
                                        </div>
                                    </div>                                

                                    <div class="col-sm-12">
                                        <div class="form-group">
                                            <asp:Label runat="server" ID="Label8" Text="QR-Code"></asp:Label>
                                            <asp:TextBox runat="server" id="txtQRCode" cssClass="form-control" ></asp:TextBox>
                                        </div>
                                    </div>                                
                                    <div class="col-sm-12">
                                        <center>
                                            <a href="" runat="server" id="hrBoleto" class="btn btn-whitelabel1" target="_blank"><asp:Label runat="server" ID="Label9" Text="CLIQUE AQUI PARA VISUALIZAR O BOLETO BANCÁRIO"></asp:Label></a>
                                        </center>
                                    </div>
                                </div>


                              </div>
                          </div>

                        </div>

                    </div>

                    <div class="card-footer">
                        <asp:Button runat="server" ID="btnSalvar" CssClass="btn btn-whitelabel1" Text="Salvar" onclick="btnSalvar_Click"/>
                        <asp:Button runat="server" ID="btnCancelar" CssClass="btn btn-danger float-right" Text="Cancelar" onclick="btnCancelar_Click"/>
                        <asp:Button ID="btnPostback" runat="server" Visible="false" OnClick="btnPostBack_Click" />
                        <script type="text/javascript">
                            function PostBackOnMainPage(){
                            <%=GetPostBackScript()%>
                            }
                        </script>

                    </div>
                </div>            
            
            </div>
        </div>


    </form>


<script type="text/javascript" src="../plugins/moment/moment.min.js"></script>
<script type="text/javascript" src="../plugins/moment/moment-with-locales.js"></script>
<script type="text/javascript" src="../plugins/inputmask/min/jquery.inputmask.bundle.min.js"></script>
<script type="text/javascript" src="../plugins/daterangepicker/daterangepicker.js"></script>
<script type="text/javascript" src="../plugins/bootstrap-colorpicker/js/bootstrap-colorpicker.min.js"></script>
<script type="text/javascript" src="../plugins/tempusdominus-bootstrap-4/js/tempusdominus-bootstrap-4.min.js"></script>

<script type="text/javascript" src="../plugins/chart.js/Chart.min.js"></script>
<script type="text/javascript" src="../dist/js/demo.js"></script>

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

</body>
</html>
