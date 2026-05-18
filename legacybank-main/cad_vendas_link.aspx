<%@ Page Language="C#" AutoEventWireup="true" CodeFile="cad_vendas_link.aspx.cs" Inherits="cad_vendas_link" MaintainScrollPositionOnPostback="true" %>

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

        <div class="row">
            <div class="col-12">





                <div class="card">
                    <div class="card-header bg-whitelabel1">
                        <h3 class="card-title">NOVA VENDA - VENDA POR LINK</h3>
                    </div>
                    <div class="card-body">

                        <div class="row">
                            <div class="col-sm-2">
                                <div class="form-group">
                                    <label>Código</label>
                                    <asp:TextBox runat="server" id="txtCodigo" cssClass="form-control" Enabled="false"></asp:TextBox>
                                </div>
                            </div>
                            <div class="col-sm-10">
                                <div class="form-group">
                                    <label>Tipo de Cobrança</label>
                                    <div class="custom-control custom-radio">
                                      <input runat="server" type="radio" class="custom-control-input"  id="rbtTipoCobranca" name="TipoCobranca" value="U" />                                        
                                      <label for="rbtTipoCobranca" class="custom-control-label">Venda (Cobrança única)</label>
                                    </div>
                                </div>
                            </div>

                        </div>


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




                          <div class="card">
                            <div class="card-header bg-whitelabel1">
                              <h4 class="card-title">
                                  <i class="fas fa-box"></i>     Informações do Serviço ou Produto
                              </h4>
                            </div>
                              <div class="card-body">

                                <div class="row ">
                                    <div class="col-sm-6">
                                        <div class="form-group">
                                            <asp:Label runat="server" ID="Label8" Text="Referência"></asp:Label>
                                            <asp:TextBox runat="server" id="txtReferencia" cssClass="form-control" ></asp:TextBox>
                                        </div>
                                    </div>                                
                                    <div class="col-sm-6">
                                        <div class="form-group">
                                            <asp:Label runat="server" ID="Label10" Text="Situação"></asp:Label>
                                            <asp:DropDownList runat="server" ID="ddlSituacao" CssClass="form-control">
                                                <asp:ListItem Value="S" Text="Habilitado"></asp:ListItem>
                                                <asp:ListItem Value="N" Text="Desabilitado"></asp:ListItem>
                                            </asp:DropDownList>
                                        </div>
                                    </div>   
                                </div>

                                <div class="row ">
                                    <div class="col-sm-12">
                                        <div class="form-group">
                                            <asp:Label runat="server" ID="Label9" Text="Descrição do Produto ou Serviço"></asp:Label>
                                            <asp:TextBox runat="server" id="txtDescricao" TextMode="MultiLine" Rows="5" cssClass="form-control" ></asp:TextBox>
                                        </div>
                                    </div>                                
                                </div>

                                <div class="row ">
                                    <div class="col-sm-6">
                                        <div class="form-group">
                                            <asp:Label runat="server" ID="Label22" Text="Valor Total Venda"></asp:Label>
                                            <asp:TextBox runat="server" id="txtValorTotal" cssClass="form-control" ></asp:TextBox>
                                        </div>
                                    </div>                                

                                    <div class="col-sm-6">
                                        <div class="form-group">
                                            <asp:Label runat="server" ID="Label25" Text="Data de Vencimento"></asp:Label>
                                            <asp:TextBox runat="server" id="txtVencimento" cssClass="form-control" ></asp:TextBox>
                                        </div>
                                    </div>                                

                                </div>


                                <div class="row">
                                    <div class="col-sm-3">
                                        <asp:Image CssClass="img-fluid" runat="server" ID="imgImagem" /> 
                                    </div>
                                    <div class="col-sm-9">
                                        <div class="form-group">
                                            <asp:Label runat="server" ID="Label13" Text="Imagem Destaque Check-Out"></asp:Label>
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
                                            <asp:Label runat="server" ID="Label5" Text="Selecione o produto"></asp:Label>
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
                                        <asp:Label runat="server" ID="Label18" Text="ID"></asp:Label>
                                        <asp:TextBox runat="server" id="txtIDProduto" cssClass="form-control" ></asp:TextBox>
                                        </div>
                                    </div>   
                                    <div class="col-sm-4">
                                        <div class="form-group">
                                        <asp:Label runat="server" ID="Label1" Text="Produto"></asp:Label>
                                        <asp:Label runat="server" id="txtProduto" cssClass="form-control" ></asp:Label>
                                        </div>
                                    </div>                                
                                    <div class="col-sm-2">
                                        <div class="form-group">
                                        <asp:Label runat="server" ID="Label7" Text="Qtde"></asp:Label>
                                        <asp:TextBox runat="server" id="txtQtdeProduto" cssClass="form-control" ></asp:TextBox>
                                        </div>
                                    </div>   


                                    <div class="col-sm-2">
                                        <div class="form-group">
                                        <asp:Label runat="server" ID="Label2" Text="Valor"></asp:Label>
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

                                    
                                    <div class="col-sm-12" runat="server" id="divProdutosAlteracao" visible="false">


                                        <table id="Table1" class="table table-bordered table-hover">
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
                                                <asp:Repeater runat="server" ID="rptProdutos" 
                                                        onitemcommand="rptProdutos_ItemCommand">
                                                    <ItemTemplate>
                                                        <tr>

                                                            <td>
                                                                <asp:TextBox runat="server" ID="txtid" Text='<%# DataBinder.Eval(Container.DataItem, "COD_ID")%>' Visible="false"></asp:TextBox>
                                                                <small><%# DataBinder.Eval(Container.DataItem, "COD_ID_PRODUTO")%></small>
                                                            </td>

                                                            <td>
                                                                <small><%# DataBinder.Eval(Container.DataItem, "NOM_PRODUTO")%></small>
                                                            </td>
                                                            <td>
                                                                <small><%# DataBinder.Eval(Container.DataItem, "NUM_QTDE")%></small>
                                                                <asp:TextBox runat="server" ID="txtQtde" Text='<%# DataBinder.Eval(Container.DataItem, "NUM_QTDE")%>' Visible="false"></asp:TextBox>

                                                            </td>
                                                            <td>
                                                                <small><%# DataBinder.Eval(Container.DataItem, "NUM_VALOR")%></small>
                                                                <asp:TextBox runat="server" ID="txtValor" Text='<%# DataBinder.Eval(Container.DataItem, "NUM_VALOR")%>' Visible="false"></asp:TextBox>

                                                            </td>
                                                            <td>
                                                                <asp:linkbutton ID="lkbExcluir" CssClass="" commandname="Excluir" runat="server" text="Excluir" ToolTip="Excluir"  CommandArgument='<%# DataBinder.Eval(Container.DataItem, "COD_ID")%>'><i class="fas fa-trash text-danger"></i></asp:linkbutton>
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





                          <div class="card">
                            <div class="card-header bg-whitelabel1">
                              <h4 class="card-title">
                                  <i class="fas fa-user"></i>     Forma Pagamento
                              </h4>
                            </div>
                              <div class="card-body">


                                <div class="row">
                                  <div class="col-2 col-sm-2 col-md-2">
                                    <div class="info-box">
                                      <span class="info-box-icon bg-whitelabel1 elevation-1"><i class="fas fa-barcode"></i></span>

                                      <div class="info-box-content text-center">
                                        <span class="info-box-text">Boleto</span>
                                        <span class="info-box-number">

                                            <div class="form-group">
                                                <div class="form-check">
                                                  <asp:CheckBox CssClass="form-check-input" runat="server" id="ckbBoleto" 
                                                        AutoPostBack="True" oncheckedchanged="ckbBoleto_CheckedChanged"/>
                                                </div>
                                            </div>

                                        </span>
                                      </div>
                                    </div>
                                  </div>
                                  <div class="col-2 col-sm-2 col-md-2">
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
                                  <div class="col-2 col-sm-2 col-md-2">
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
                                  <div class="col-6 col-sm-6 col-md-6">
                                    <div class="row">
                                        <div class="col-12">
                                            <div class="form-group">
                                                <asp:Label runat="server" ID="Label41" Text="Quantidade máxima de parcelas"></asp:Label><br />
                                            </div>
                                        </div>
                                    </div>

                                    <div class="row">
                                        <div class="col-12">
                                            <div class="form-group">
                                                <asp:DropDownList runat="server" ID="ddlParcelas" CssClass="form-control">
                                                    <asp:ListItem Value="1" Text="1"></asp:ListItem>
                                                    <asp:ListItem Value="2" Text="2"></asp:ListItem>
                                                    <asp:ListItem Value="3" Text="3"></asp:ListItem>
                                                    <asp:ListItem Value="4" Text="4"></asp:ListItem>
                                                    <asp:ListItem Value="5" Text="5"></asp:ListItem>
                                                    <asp:ListItem Value="6" Text="6"></asp:ListItem>
                                                    <asp:ListItem Value="7" Text="7"></asp:ListItem>
                                                    <asp:ListItem Value="8" Text="8"></asp:ListItem>
                                                    <asp:ListItem Value="9" Text="9"></asp:ListItem>
                                                    <asp:ListItem Value="10" Text="10"></asp:ListItem>
                                                    <asp:ListItem Value="11" Text="11"></asp:ListItem>
                                                    <asp:ListItem Value="12" Text="12"></asp:ListItem>
                                                    <asp:ListItem Value="13" Text="13"></asp:ListItem>
                                                    <asp:ListItem Value="14" Text="14"></asp:ListItem>
                                                    <asp:ListItem Value="15" Text="15"></asp:ListItem>
                                                    <asp:ListItem Value="16" Text="16"></asp:ListItem>
                                                    <asp:ListItem Value="17" Text="17"></asp:ListItem>
                                                    <asp:ListItem Value="18" Text="18"></asp:ListItem>
                                                    <asp:ListItem Value="19" Text="19"></asp:ListItem>
                                                    <asp:ListItem Value="20" Text="20"></asp:ListItem>
                                                    <asp:ListItem Value="21" Text="21"></asp:ListItem>
                                                </asp:DropDownList><br />
                                            </div>
                                        </div>
                                    </div>

                                  </div>
                                  <div class="col-6 col-sm-6 col-md-6">
                                  </div>
                                </div>
                                <div class="row">
                                    <div class="col-6">

                                    </div>
                                </div>

                                <div class="row">
                                    <div class="col-sm-4">
                                        <div class="form-group">
                                            <asp:Label runat="server" ID="Label11" Text="Tipo de Operação"></asp:Label>
                                            <asp:DropDownList runat="server" ID="ddlTipoOperacao" CssClass="form-control">
                                                <asp:ListItem Value="C" Text="Autorização Completa"></asp:ListItem>
                                                <asp:ListItem Value="P" Text="Pré autorização"></asp:ListItem>
                                            </asp:DropDownList>
                                        </div>
                                    </div>                                
                                    <div class="col-sm-4">
                                        <div class="form-group">
                                            <asp:Label runat="server" ID="Label12" Text="Operadora"></asp:Label>
                                            <asp:DropDownList runat="server" ID="ddlOperadora" CssClass="form-control">
                                                <asp:ListItem Value="Z" Text="Zoop"></asp:ListItem>
                                            </asp:DropDownList>
                                        </div>
                                    </div>   

                                    <div class="col-sm-4">
                                        <div class="form-group">
                                            <asp:Label runat="server" id="Label19"  Text="Data Limite para pagamento"></asp:Label>
                                            <div class="input-group date" id="datepickerIni" data-target-input="nearest">
								                <asp:TextBox ID="txtDataLimite" runat="server" CssClass="form-control datetimepicker-input" data-target="#datepickerIni"></asp:TextBox>
                                                <div class="input-group-append" data-target="#datepickerIni" data-toggle="datetimepicker">
                                                    <div class="input-group-text"><i class="fa fa-calendar"></i></div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-sm-12">
                                        <div class="form-group">
                                            <div class="form-check">
                                              <asp:CheckBox CssClass="form-check-input" runat="server" id="ckbLinkPermanente" />
                                              <label class="form-check-label">Permitir usar o mesmo link mais de uma vez</label>
                                            </div>
                                        </div>
                                    </div>

                                </div>




                                <div class="row">
                                    <div class="col-sm-12">
                                        <div class="form-group">
                                            <div class="form-check">
                                              <asp:CheckBox CssClass="form-check-input" runat="server" id="ckbPreco" 
                                                    AutoPostBack="True" oncheckedchanged="ckbPreco_CheckedChanged"/>
                                              <label class="form-check-label">Mudar o preço conforme o parcelamento</label>
                                            </div>
                                        </div>
                                    </div>   
                                </div>
                                
                                <div class="row">
                                    <div class="col-sm-6">
                                        <div class="form-group">
                                            <asp:Label runat="server" ID="Label38" Text="Simulação de Venda"></asp:Label>
                                            <asp:DropDownList runat="server" id="ddlSimulado" CssClass="form-control">
                                            </asp:DropDownList>
                                        </div>
                                    </div>   
                                    <div class="col-sm-3">
                                        <asp:Label runat="server" ID="Label39" Text=""></asp:Label><br />
                                        <asp:Button CssClass="btn btn-whitelabel1 btn-block" runat="server" 
                                            ID="btnVisualizarSimulado" Text="Visualizar simulado" 
                                            onclick="btnVisualizarSimulado_Click" />
                                    </div>
                                    <div class="col-sm-3">
                                        <asp:Label runat="server" ID="Label40" Text=""></asp:Label><br />
                                        <asp:Button CssClass="btn btn-whitelabel1 btn-block" runat="server" 
                                            ID="btnAplicarSimulado" Text="Aplicar nos valores parcelados" 
                                            onclick="btnAplicarSimulado_Click" />
                                    </div>

                                </div>


                                
                                <div class="row" runat="server" id="divPreco" visible="false">
                                    <div class="col-sm-3">
                                        <div class="form-group">
                                        <asp:Label runat="server" ID="Label26" Text="Preço à vista (1x)"></asp:Label>
                                        <asp:TextBox runat="server" id="txtValor01" cssClass="form-control" ></asp:TextBox>
                                        </div>
                                    </div>   

                                    <div class="col-sm-3">
                                        <div class="form-group">
                                        <asp:Label runat="server" ID="Label27" Text="Preço parcelado (2x)"></asp:Label>
                                        <asp:TextBox runat="server" id="txtValor02" cssClass="form-control" ></asp:TextBox>
                                        </div>
                                    </div>   
                                    <div class="col-sm-3">
                                        <div class="form-group">
                                        <asp:Label runat="server" ID="Label28" Text="Preço parcelado (3x)"></asp:Label>
                                        <asp:TextBox runat="server" id="txtValor03" cssClass="form-control" ></asp:TextBox>
                                        </div>
                                    </div>   
                                    <div class="col-sm-3">
                                        <div class="form-group">
                                        <asp:Label runat="server" ID="Label29" Text="Preço parcelado (4x)"></asp:Label>
                                        <asp:TextBox runat="server" id="txtValor04" cssClass="form-control" ></asp:TextBox>
                                        </div>
                                    </div>   
                                    <div class="col-sm-3">
                                        <div class="form-group">
                                        <asp:Label runat="server" ID="Label30" Text="Preço parcelado (5x)"></asp:Label>
                                        <asp:TextBox runat="server" id="txtValor05" cssClass="form-control" ></asp:TextBox>
                                        </div>
                                    </div>   
                                    <div class="col-sm-3">
                                        <div class="form-group">
                                        <asp:Label runat="server" ID="Label31" Text="Preço parcelado (6x)"></asp:Label>
                                        <asp:TextBox runat="server" id="txtValor06" cssClass="form-control" ></asp:TextBox>
                                        </div>
                                    </div>   
                                    <div class="col-sm-3">
                                        <div class="form-group">
                                        <asp:Label runat="server" ID="Label32" Text="Preço parcelado (7x)"></asp:Label>
                                        <asp:TextBox runat="server" id="txtValor07" cssClass="form-control" ></asp:TextBox>
                                        </div>
                                    </div>   
                                    <div class="col-sm-3">
                                        <div class="form-group">
                                        <asp:Label runat="server" ID="Label33" Text="Preço parcelado (8x)"></asp:Label>
                                        <asp:TextBox runat="server" id="txtValor08" cssClass="form-control" ></asp:TextBox>
                                        </div>
                                    </div>   
                                    <div class="col-sm-3">
                                        <div class="form-group">
                                        <asp:Label runat="server" ID="Label34" Text="Preço parcelado (9x)"></asp:Label>
                                        <asp:TextBox runat="server" id="txtValor09" cssClass="form-control" ></asp:TextBox>
                                        </div>
                                    </div>   
                                    <div class="col-sm-3">
                                        <div class="form-group">
                                        <asp:Label runat="server" ID="Label35" Text="Preço parcelado (10x)"></asp:Label>
                                        <asp:TextBox runat="server" id="txtValor10" cssClass="form-control" ></asp:TextBox>
                                        </div>
                                    </div>   
                                    <div class="col-sm-3">
                                        <div class="form-group">
                                        <asp:Label runat="server" ID="Label36" Text="Preço parcelado (11x)"></asp:Label>
                                        <asp:TextBox runat="server" id="txtValor11" cssClass="form-control" ></asp:TextBox>
                                        </div>
                                    </div>   
                                    <div class="col-sm-3">
                                        <div class="form-group">
                                        <asp:Label runat="server" ID="Label37" Text="Preço parcelado (12x)"></asp:Label>
                                        <asp:TextBox runat="server" id="txtValor12" cssClass="form-control" ></asp:TextBox>
                                        </div>
                                    </div>   
                                    <div class="col-sm-3">
                                        <div class="form-group">
                                        <asp:Label runat="server" ID="Label4" Text="Preço parcelado (13x)"></asp:Label>
                                        <asp:TextBox runat="server" id="txtValor13" cssClass="form-control" ></asp:TextBox>
                                        </div>
                                    </div>   
                                    <div class="col-sm-3">
                                        <div class="form-group">
                                        <asp:Label runat="server" ID="Label42" Text="Preço parcelado (14x)"></asp:Label>
                                        <asp:TextBox runat="server" id="txtValor14" cssClass="form-control" ></asp:TextBox>
                                        </div>
                                    </div>   
                                    <div class="col-sm-3">
                                        <div class="form-group">
                                        <asp:Label runat="server" ID="Label43" Text="Preço parcelado (15x)"></asp:Label>
                                        <asp:TextBox runat="server" id="txtValor15" cssClass="form-control" ></asp:TextBox>
                                        </div>
                                    </div>   
                                    <div class="col-sm-3">
                                        <div class="form-group">
                                        <asp:Label runat="server" ID="Label44" Text="Preço parcelado (16x)"></asp:Label>
                                        <asp:TextBox runat="server" id="txtValor16" cssClass="form-control" ></asp:TextBox>
                                        </div>
                                    </div>   
                                    <div class="col-sm-3">
                                        <div class="form-group">
                                        <asp:Label runat="server" ID="Label45" Text="Preço parcelado (17x)"></asp:Label>
                                        <asp:TextBox runat="server" id="txtValor17" cssClass="form-control" ></asp:TextBox>
                                        </div>
                                    </div>   
                                    <div class="col-sm-3">
                                        <div class="form-group">
                                        <asp:Label runat="server" ID="Label46" Text="Preço parcelado (18x)"></asp:Label>
                                        <asp:TextBox runat="server" id="txtValor18" cssClass="form-control" ></asp:TextBox>
                                        </div>
                                    </div>   
                                    <div class="col-sm-3">
                                        <div class="form-group">
                                        <asp:Label runat="server" ID="Label47" Text="Preço parcelado (19x)"></asp:Label>
                                        <asp:TextBox runat="server" id="txtValor19" cssClass="form-control" ></asp:TextBox>
                                        </div>
                                    </div>   
                                    <div class="col-sm-3">
                                        <div class="form-group">
                                        <asp:Label runat="server" ID="Label48" Text="Preço parcelado (20x)"></asp:Label>
                                        <asp:TextBox runat="server" id="txtValor20" cssClass="form-control" ></asp:TextBox>
                                        </div>
                                    </div>   
                                    <div class="col-sm-3">
                                        <div class="form-group">
                                        <asp:Label runat="server" ID="Label49" Text="Preço parcelado 21x)"></asp:Label>
                                        <asp:TextBox runat="server" id="txtValor21" cssClass="form-control" ></asp:TextBox>
                                        </div>
                                    </div>   
                                    
                                    
                                </div>




                              </div>
                          </div>











                          <div class="card" runat="server" id="dvDadosPix" visible="false">
                            <div class="card-header  bg-whitelabel1">
                              <h4 class="card-title">
                                  <i class="fas fa-money-check-alt"></i>     Pix
                              </h4>
                            </div>
                              <div class="card-body">

                                <div class="row ">
                                    <div class="col-sm-4" runat="server" id="dvPix">
                                        <div class="form-group">
                                            <asp:Label runat="server" id="lblPix"  Text="Data Validade do PIX"></asp:Label>


                                        </div>

                                    </div>

                                </div>

                                <div class="row d-flex justify-content-center">
                                
                                    <div class="col-sm-4">
                                        <img runat="server" class="img-fluid" id="imgQRcode" src="" alt=""  />                                    
                                    </div>
                                
                                </div>
                                <br /><br />
                                <div class="row d-flex justify-content-center">
                                
                                    <div class="col-sm-10" runat="server" id="dvCodigoPix" >
                                        <div class="form-group">
                                        <center>
                                        <b><asp:Label runat="server" ID="Label3" Text="Código PIX"></asp:Label></b>
                                        <asp:TextBox runat="server" id="txtPix" cssClass="form-control" ></asp:TextBox>
                                        </center>
                                        </div>
                                    </div>
                                
                                </div>

                              </div>
                          </div>


                          <div class="card" runat="server" id="dvBoletoBancario" visible="false">
                            <div class="card-header bg-whitelabel1">
                              <h4 class="card-title">
                                  <i class="fas fa-barcode"></i>     Condições Pagamento Boleto Bancário
                              </h4>
                            </div>
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
                                <div class="form-group row">
                                    <label for="inputEmail3" class="col-sm-12 control-label">Avisos: Informações de descontos, multas e juros não terão validade neste campo</label>
                                </div>
                                <div class="form-group row">
                                    <div class="icheck-primary d-inline col-2">
                                    <asp:CheckBox runat="server" id="ckbAvisos" />
                                    <label for="ckbAvisos">
                                        Habilitar Avisos
                                    </label>
                                    </div>
                                    <div class="col-sm-10">
                                        <asp:TextBox runat="server" id="txtAviso" cssClass="form-control" placeholder="Exemplo: Em caso de dúvidas entre em contato pela nossa central de atendimento" ></asp:TextBox>
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
                                              <label class="form-check-label">A venda terá divisão de Split</label>
                                            </div>
                                        </div>
                                    </div>   
                                </div>
                                <div class="row" runat="server" id="divSplit" visible="false">
                                    <div class="col-sm-12">
                                        <div class="form-group">
                                            <asp:Label runat="server" ID="Label15" Text="Forma de Split"></asp:Label>
                                            <asp:DropDownList runat="server" ID="ddlFormaSplit" CssClass="form-control">
                                                <asp:ListItem Value="1" Text="Por valor - Taxas do Vendedor"></asp:ListItem>
                                                <asp:ListItem Value="2" Text="Por percentual - Taxas Proporcionais"></asp:ListItem>
                                                <asp:ListItem Value="3" Text="Por percentual - Taxas do Parceiro"></asp:ListItem>
                                                <asp:ListItem Value="4" Text="Por Valor - Taxas do Parceiro"></asp:ListItem>
                                                <asp:ListItem Value="5" Text="Por Valor - Parceiro assume o valor da venda"></asp:ListItem>
                                            </asp:DropDownList>
                                        </div>
                                    </div>  

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
                        <div class="card">
                            <div class="card-header bg-whitelabel1">
                                <h4 class="card-title">
                                <i class="fas fa-envelope"></i>     E-mail
                                </h4>
                            </div>
                            <div class="card-body">
                                <div class="col-sm-12">
                                    <div class="form-group">
                                    <asp:Label runat="server" ID="Label14" Text="Para quem você deseja enviar o link de pagamento?"></asp:Label>
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

                    <div class="card-footer">
                        <asp:Button runat="server" ID="btnSalvar" CssClass="btn btn-whitelabel1" Text="Salvar" onclick="btnSalvar_Click"/>
                        <asp:Button runat="server" ID="btnEnviar" CssClass="btn btn-whitelabel1" Text="Salvar e Enviar" onclick="btnEnviar_Click" Visible="false"/>
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

<script type="text/javascript" src="../plugins/datatables/jquery.dataTables.js"></script>
<script type="text/javascript" src="../plugins/datatables-bs4/js/dataTables.bootstrap4.js"></script>

<script src="../plugins/inputmask/jquery.inputmask.bundle.js"></script>
<script src="../plugins/moment/moment.min.js"></script>

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
