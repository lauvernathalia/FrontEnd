<%@ Page Language="C#" AutoEventWireup="true" CodeFile="checkout.aspx.cs" Inherits="checkout" %>

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

                <section class="content">
                    <div class="container-fluid" >

                        <div class="row vh-100">

                            <div runat="server" id="dvCheckout" class="col-6 vh-100">
                                <center>
                                <img runat="server" id="imgLogoPrincipal" class="img-fluid p-2 mt-2 mb-2 col-6" />
                                </center>

                                <h4 class="text-white pt-2 mt-2"><strong>Valor: R$ <asp:Label runat="server" ID="lblValor" Text="0,00"></asp:Label></strong></h4>
                                <h4 class="text-white pt-2 mt-2"><strong>Vencimento: <asp:Label runat="server" ID="lblVencimento" Text=""></asp:Label></strong></h4>
                                <h5 class="text-white"><asp:Label runat="server" ID="lblReferencia" Text="PRODUTO REFERÊNCIA"></asp:Label></h5>
                                <h6 class="text-white"><asp:Label runat="server" ID="lblDescricao" Text="Lorem ipsum dolor sit amet, consectetur adipiscing elit. Etiam eget ligula eu lectus lobortis condimentum. Aliquam nonummy auctor massa"></asp:Label></h6>
                                <img runat="server" id="imgVenda" class="img-fluid p-2 mt-2 mb-2 col-8" alt="" />
                                
                                <div class="row" runat="server" id="divProdutos" visible="false">
                                    <div class="col-12">
                                        <div class="card">
                                          <div class="card-header">
                                          <h5><strong>Informações da compra</strong></h5>
                                          </div>
                                          <div class="card-body">

                                                <table id="Table1" class="table table-bordered table-hover">
                                                    <thead>
                                                    <tr>
                                                        <th>Produto</th>
                                                        <th>Qtde</th>
                                                        <th>Valor</th>
                                                    </tr>
                                                    </thead>
                                                    <tbody>
                                                        <asp:Repeater runat="server" ID="rptProdutos">
                                                            <ItemTemplate>
                                                                <tr>
                                                                    <td>
                                                                        <asp:TextBox runat="server" ID="txtid" Text='<%# DataBinder.Eval(Container.DataItem, "COD_ID_PRODUTO")%>' Visible="false"></asp:TextBox>
                                                                        <small><%# DataBinder.Eval(Container.DataItem, "NOM_PRODUTO")%></small>
                                                                    </td>
                                                                    <td>
                                                                        <small><%# DataBinder.Eval(Container.DataItem, "NUM_QTDE")%></small>
                                                                    </td>
                                                                    <td>
                                                                        <small><%# DataBinder.Eval(Container.DataItem, "NUM_VALOR")%></small>
                                                                    </td>
                                                                </tr>
                                                            </ItemTemplate>
                                                        </asp:Repeater>

                                                    </tbody>
                                                </table>

                                          </div>
                                          <div class="card-footer">
                                          </div>
                                        </div>                                
                                    </div>
                                </div>
                            </div>

                            <div runat="server" id="dvPagamento" class="col-6 vh-100" style="overflow-y: scroll;">
                                    
                                    <div class="card">
                                        <div class="card-body" runat="server" id="cbPagamento" visible="false">

                                            <h4><span class="float-center badge  bg-whitelabel">Forma de Pagamento</span></h4>

                                            <div class="row">
                                              <div class="col-4 col-sm-4 col-md-4" runat="server" id="lblFPBoleto" visible="false">
                                                <div class="form-group">
                                                    <center>
                                                        <span class="info-box-icon"><i class="fas fa-barcode fa-3x"></i></span><br />
                                                        <b>Boleto</b><br />
                                                        <asp:RadioButton runat="server" GroupName="optParcelas" ID="FPBoleto" 
                                                            AutoPostBack="True" oncheckedchanged="FPBoleto_CheckedChanged" />
                                                    </center>
                                                </div>
                                              </div>
                                              <div class="col-4 col-sm-4 col-md-4" runat="server" id="lblFPCredito" visible="false">
                                                <div class="form-group">
                                                    <center>
                                                        <span class="info-box-icon"><i class="fas fa-credit-card fa-3x"></i></span><br />
                                                        <b>Crédito</b><br />
                                                        <asp:RadioButton runat="server" GroupName="optParcelas" ID="FPCredito" 
                                                            AutoPostBack="True" oncheckedchanged="FPCredito_CheckedChanged" />
                                                    </center>
                                                </div>
                                              </div>
                                              <div class="col-4 col-sm-4 col-md-4" runat="server" id="lblFPPix" visible="false">
                                                <div class="form-group">
                                                    <center>
                                                        <span class="info-box-icon"><i class="fab fa-pix fa-3x"></i></span><br />
                                                        <b>Pix</b><br />
                                                        <asp:RadioButton runat="server" GroupName="optParcelas" ID="FPPix" 
                                                            AutoPostBack="True" oncheckedchanged="FPPix_CheckedChanged" />
                                                    </center>
                                                </div>
                                              </div>
                                            </div>
                                            
                                            <h4 runat="server" id="h4CartaoCredito" visible="false"><span class="float-center badge bg-whitelabel">Cartão de Crédito</span></h4>

                                            <div class="row" runat="server" id="dvCartaoCredito" visible="false">
                                                <div class="col-sm-6">
                                                    <div class="form-group">
                                                        <label class="col-sm-12 col-form-label">No. Cartão</label>
                                                        <asp:TextBox id="txtNumeroCartao" runat="server" class="form-control" ></asp:TextBox>
                                                    </div>
                                                </div>
                                                <div class="col-sm-6">
                                                    <div class="form-group">
                                                        <label class="col-sm-12 col-form-label">Nome no Cartão</label>
                                                        <asp:TextBox id="txtNomeCartao"  runat="server" class="form-control" ></asp:TextBox>
                                                    </div>
                                                </div>
                                                <div class="col-sm-4">
                                                    <div class="form-group">
                                                        <label class="col-sm-12 col-form-label">Mês Validade</label>
                                                        <asp:DropDownList id="ddlMesCartao" runat="server" class="form-control"  >
							                                <asp:ListItem Value="01">01</asp:ListItem>
							                                <asp:ListItem Value="02">02</asp:ListItem>
							                                <asp:ListItem Value="03">03</asp:ListItem>
							                                <asp:ListItem Value="04">04</asp:ListItem>
							                                <asp:ListItem Value="05">05</asp:ListItem>
							                                <asp:ListItem Value="06">06</asp:ListItem>
							                                <asp:ListItem Value="07">07</asp:ListItem>
							                                <asp:ListItem Value="08">08</asp:ListItem>
							                                <asp:ListItem Value="09">09</asp:ListItem>
							                                <asp:ListItem Value="10">10</asp:ListItem>
							                                <asp:ListItem Value="11">11</asp:ListItem>
							                                <asp:ListItem Value="12">12</asp:ListItem>
                                                        </asp:DropDownList>                    
                                                    </div>
                                                </div>
                                                <div class="col-sm-4">
                                                    <div class="form-group">
                                                        <label class="col-sm-12 col-form-label">Ano Validade</label>
                                                        <asp:DropDownList id="ddlAnoCartao" runat="server" class="form-control">
                                                        </asp:DropDownList>                    
                                                    </div>
                                                </div>
                                                <div class="col-sm-4">
                                                    <div class="form-group">
                                                        <label class="col-sm-12 col-form-label">CVV</label>
                                                        <asp:TextBox id="txtCVVCartao"  runat="server" class="form-control"></asp:TextBox>
                                                    </div>
                                                </div>

                                                <div class="col-sm-12">
                                                    <div class="form-group">
                                                        <label class="col-sm-12 col-form-label">Parcelar em</label>
                                                        <asp:DropDownList id="ddlParcelas" runat="server" class="form-control">
                                                        </asp:DropDownList>                    
                                                    </div>
                                                </div>
                                                <br /><br /><br /><br /><br />


                                            </div>


                                            <h4><span class="float-center badge bg-whitelabel">Dados Pessoais</span></h4>

                                            <label class="col-sm-12 col-form-label">Informe ou confirme os seus dados pessoais</label>

                                            <div class="row">
                                                <div class="col-sm-6">
                                                    <div class="form-group">
                                                        <label class="col-sm-12 col-form-label">Nome</label>
                                                        <asp:TextBox id="txtNome" runat="server" class="form-control" required></asp:TextBox>
                                                    </div>
                                                </div>
                                                <div class="col-sm-6">
                                                    <div class="form-group">
                                                        <label class="col-sm-12 col-form-label">Sobrenome</label>
                                                        <asp:TextBox id="txtSobrenome"  runat="server" class="form-control" required></asp:TextBox>
                                                    </div>
                                                </div>
                                            </div>

                                            <div class="row">
                                                <div class="col-sm-4">
                                                    <div class="form-group">
                                                        <label class="col-sm-12 col-form-label">Documento (CNPJ/CPF)</label>
                                                        <asp:TextBox id="txtDocumento"  runat="server" class="form-control" required></asp:TextBox>
                                                    </div>
                                                </div>
                                                <div class="col-sm-4">
                                                    <div class="form-group">
                                                        <label class="col-sm-12 col-form-label">E-mail</label>
                                                        <asp:TextBox id="txtEmail" runat="server" class="form-control" required></asp:TextBox>
                                                    </div>
                                                </div>
                                                <div class="col-sm-4">
                                                    <div class="form-group">
                                                        <label class="col-sm-12 col-form-label">Celular</label>
                                                        <asp:TextBox id="txtCelular" runat="server" class="form-control" required></asp:TextBox>
                                                    </div>
                                                </div>


                                            </div>

                                            <div class="row">
                                                <div class="col-sm-2"  >
                                                    <div class="form-group">
                                                    <label>CEP<strong class="text-danger">*</strong></label>
                                                    <asp:TextBox runat="server" ID="txtCEP" CssClass="form-control" 
                                                            data-inputmask='"mask": "99999-999"' data-mask  
                                                            AutoPostBack="True" ontextchanged="txtCEP_TextChanged"></asp:TextBox> 
                                                    </div>
                                                </div>
                                                <div class="col-sm-4"  >
                                                    <div class="form-group">
                                                    <label>Endereço<strong class="text-danger">*</strong></label>
                                                    <asp:TextBox runat="server" ID="txtEndereco" CssClass="form-control" required></asp:TextBox> 
                                                    </div>
                                                </div>
                                                <div class="col-sm-3"  >
                                                    <div class="form-group">
                                                    <label>Número<strong class="text-danger">*</strong></label>
                                                    <asp:TextBox runat="server" ID="txtNumero" CssClass="form-control" required></asp:TextBox> 
                                                    </div>
                                                </div>
                                          
                                                <div class="col-sm-3"  >
                                                    <div class="form-group">
                                                    <label>Complemento<strong class="text-danger">*</strong></label>
                                                    <asp:TextBox runat="server" ID="txtComplemento" CssClass="form-control"></asp:TextBox>
                                                    </div>
                                                </div>

                                            </div>
                                            <div class="row">
                                                <div class="col-sm-5"  >
                                                    <div class="form-group">
                                                    <label>Bairro<strong class="text-danger">*</strong></label>
                                                    <asp:TextBox runat="server" ID="txtBairro" CssClass="form-control" required></asp:TextBox> 
                                                    </div>
                                                </div>
                                                <div class="col-sm-5"  >
                                                    <div class="form-group">
                                                    <label>Cidade<strong class="text-danger">*</strong></label>
                                                    <asp:TextBox runat="server" ID="txtCidade" CssClass="form-control" required></asp:TextBox> 
                                                    </div>
                                                </div>
                                          
                                                <div class="col-sm-2"  >
                                                    <div class="form-group">
                                                        <label>Estado<strong class="text-danger">*</strong></label>
                                                        <asp:DropDownList id="ddlEstado" runat="server" class="form-control" required >
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
                                                <div class="col-sm-12">
                                                    <div class="form-group">
                                                        <div class="form-check">
                                                          <asp:CheckBox CssClass="form-check-input" runat="server" id="ckbEnderecoEntrega" 
                                                                AutoPostBack="True" oncheckedchanged="ckbEnderecoEntrega_CheckedChanged"/>
                                                          <label class="form-check-label">Clique aqui caso deseje especificar um endereço de entrega diferente</label>
                                                        </div>
                                                    </div>
                                                </div>   
                                            </div>
                                            <!-- ENDEREÇO DE ENTREGA -->
                                            <div runat="server" id="dvEntrega" visible="false">
                                            
                                                <h4><span class="float-center badge bg-whitelabel">Endereço de Entrega</span></h4>


                                                <div class="row">
                                                    <div class="col-sm-2"  >
                                                        <div class="form-group">
                                                        <label>CEP<strong class="text-danger">*</strong></label>
                                                        <asp:TextBox runat="server" ID="txtCEPEntrega" CssClass="form-control" 
                                                                data-inputmask='"mask": "99999-999"' data-mask  
                                                                AutoPostBack="True" ontextchanged="txtCEPEntrega_TextChanged"></asp:TextBox> 
                                                        </div>
                                                    </div>
                                                    <div class="col-sm-4"  >
                                                        <div class="form-group">
                                                        <label>Endereço<strong class="text-danger">*</strong></label>
                                                        <asp:TextBox runat="server" ID="txtEnderecoEntrega" CssClass="form-control" required></asp:TextBox> 
                                                        </div>
                                                    </div>
                                                    <div class="col-sm-3"  >
                                                        <div class="form-group">
                                                        <label>Número<strong class="text-danger">*</strong></label>
                                                        <asp:TextBox runat="server" ID="txtNumeroEntrega" CssClass="form-control" required></asp:TextBox> 
                                                        </div>
                                                    </div>
                                          
                                                    <div class="col-sm-3"  >
                                                        <div class="form-group">
                                                        <label>Complemento<strong class="text-danger">*</strong></label>
                                                        <asp:TextBox runat="server" ID="txtComplementoEntrega" CssClass="form-control"></asp:TextBox>
                                                        </div>
                                                    </div>

                                                </div>
                                                <div class="row">
                                                    <div class="col-sm-5"  >
                                                        <div class="form-group">
                                                        <label>Bairro<strong class="text-danger">*</strong></label>
                                                        <asp:TextBox runat="server" ID="txtBairroEntrega" CssClass="form-control" required></asp:TextBox> 
                                                        </div>
                                                    </div>
                                                    <div class="col-sm-5"  >
                                                        <div class="form-group">
                                                        <label>Cidade<strong class="text-danger">*</strong></label>
                                                        <asp:TextBox runat="server" ID="txtCidadeEntrega" CssClass="form-control" required></asp:TextBox> 
                                                        </div>
                                                    </div>
                                          
                                                    <div class="col-sm-2"  >
                                                        <div class="form-group">
                                                            <label>Estado<strong class="text-danger">*</strong></label>
                                                            <asp:DropDownList id="ddlEstadoEntrega" runat="server" class="form-control" required >
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

                                            <div  runat="server" id="dvAdicionais" visible="false">
                                                <h4><span class="float-center badge bg-whitelabel">Informações adicionais</span></h4>

                                                <div class="row">
                                                    <div class="col-sm-12">
                                                        <div class="form-group">
                                                            <label class="col-sm-12 col-form-label"><asp:Label runat="server" ID="lblAdicional"></asp:Label></label>
                                                            <asp:TextBox id="txtConteudo01" runat="server" class="form-control" required></asp:TextBox>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>

                                            <div class="row" runat="server" id="dvPix" visible="false">

                                            </div>

                                            <div class="row" runat="server" id="dvBoleto" visible="false">
                                                <asp:TextBox runat="server" ID="txtRetorno" Visible="true" CssClass="form-control"></asp:TextBox>
                                            </div>



                                        </div>



                                        <div class="card-body" runat="server" id="cbConfirmacao" visible="false">
                                            <h4><span class="float-center badge bg-whitelabel">Confirmação de Pagamento</span></h4>
                                            <div class="row">
                                                <div class="col-sm-12">
                                                    <div class="form-group">
                                                        <center><h2><asp:Label runat="server" ID="lblTransacao" CssClass="text-success" Text="Transação"></asp:Label></h2></center>
                                                        <h4><asp:Label runat="server" ID="txtTransacao" CssClass="text-black" Text=""></asp:Label></h4>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="row">
                                                <div class="col-sm-12">
                                                    <div class="form-group">
                                                        <center><h2><asp:Label runat="server" ID="lblStatus" CssClass="text-success" Text="Status"></asp:Label></h2></center>
                                                        <h4><asp:Label runat="server" ID="txtStatus" CssClass="text-black" Text=""></asp:Label></h4>

                                                    </div>
                                                </div>
                                            </div>

                                        </div>

                                        <div class="card-body" runat="server" id="cbPix" visible="false">
                                                <h4><span class="float-center badge bg-whitelabel">Pagamento PIX</span></h4>
                                                <center>
                                                <p>Use o QR Code do PIX abaixo para realizar o pagamento/transferência.<br />
                                                Abra o aplicativo do seu banco e escaneie a imagem abaixo com a câmera<br />ou cole o código do QR Code.</p>
                                                </center>

                                                <div class="row d-flex justify-content-center">
                                                    <div class="col-sm-4">
                                                        <p>Pague até <asp:Label runat="server" ID="lblVencimentoPix"></asp:Label></p>
                                                        <img runat="server" class="img-fluid" id="imgQRcode" src="" alt=""  />                                    
                                                    </div>
                                                </div>
                                                <br /><br />
                                                <div class="row d-flex justify-content-center">
                                
                                                    <div class="col-sm-10" runat="server" id="dvCodigoPix" >
                                                        <div class="form-group">
                                                        <center>
                                                        <b><asp:Label runat="server" ID="Label9" Text="Código PIX"></asp:Label></b>
                                                        <asp:TextBox runat="server" id="txtPix" cssClass="form-control" ></asp:TextBox>
                                                        </center>
                                                        </div>
                                                    </div>
                                
                                                </div>                                            
                                        </div>

                                        <div class="card-body" runat="server" id="cbBoleto" visible="false">
                                                <h4><span class="float-center badge bg-whitelabel">Pagamento Boleto/PIX</span></h4>
                                                <center>
                                                <p>Use o endereço abaixo para acessar o link do Boleto/PIX.</p>
                                                </center>

                                                <br /><br />
                                                <div class="row d-flex justify-content-center">
                                
                                                    <div class="col-sm-10" runat="server" id="dvCodigoBarrasBoleto" >
                                                        <div class="form-group">
                                                        <center>
                                                        <b><asp:Label runat="server" ID="lblLink" Text="LINK DO BOLETO/PIX"></asp:Label></b>
                                                        <asp:TextBox runat="server" id="txtBoleto" cssClass="form-control" ></asp:TextBox>
                                                        </center>
                                                        </div>
                                                    </div>
                                
                                                </div>                                            

                                                <br /><br />
                                                <div class="row d-flex justify-content-center">
                                
                                                    <div class="col-sm-10" runat="server" id="divBoletoBancario" >
                                                        <div class="form-group">
                                                        <center>
                                                            <asp:Repeater runat="server" ID="rptBoleto">
                                                                <ItemTemplate>
                                                                    <a href='<%# DataBinder.Eval(Container.DataItem, "NOM_BOLETO")%>' class="btn btn-whitelabel1" target="_blank"><asp:Label runat="server" ID="lblBotaoLink" Text="CLIQUE PARA VER LINK BOLETO/PIX"></asp:Label></a>
                                                                </ItemTemplate>
                                                            </asp:Repeater>
                                                        </center>
                                                        </div>
                                                    </div>
                                
                                                </div>                                            


                                        </div>

                                        <div class="card-footer">
                                            <div class="row">
                                                <div class="col-12">
                                                    <asp:Button runat="server" ID="btnConfirmar" CssClass="btn bg-whitelabel btn-block" Text="Confirmar Pagamento" onclick="btnConfirmar_Click" Visible="false"/>
                                                    <asp:Button runat="server" ID="btnVoltar" CssClass="btn bg-whitelabel btn-block" Text="Voltar" onclick="btnVoltar_Click" Visible="false"/>
                                                </div>
                                            </div>
                                        </div>
                                    </div>            

                                    <!-- ************************************************************************************************************************************************************************* -->
                                    <!-- Modal para bloquear  ************************************************************************************************************************************* -->
                                    <!-- ************************************************************************************************************************************************************************* -->


                                    <!-- Modal -->
                                    <div class="modal fade" id="exampleModal" tabindex="-1" role="dialog" aria-labelledby="exampleModalLabel" aria-hidden="true">
                                      <div class="modal-dialog" role="document">
                                        <div class="modal-content">
                                          <div class="modal-header">
                                            <h5 class="modal-title" id="exampleModalLabel">Modal title</h5>
                                            <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                                              <span aria-hidden="true">&times;</span>
                                            </button>
                                          </div>
                                          <div class="modal-body">
                                            ...
                                          </div>
                                          <div class="modal-footer">
                                            <button type="button" class="btn btn-secondary" data-dismiss="modal">Close</button>
                                            <button type="button" class="btn btn-primary">Save changes</button>
                                          </div>
                                        </div>
                                      </div>
                                    </div>


                            </div>
                            

                            <div runat="server" id="dvMensagemGeral" class="col-12 vh-100" visible="false">
                                <center>
                                <img runat="server" id="imgLogoMensagemGeral" class="img-fluid p-2 mt-2 mb-2 col-4" /><br />
                                <label><h2><strong><asp:Label runat="server" ID="lblMensagemGeral" CssClass="p-2 mt-2 mb-2 text-white"></asp:Label></strong></h2></label>
                                </center>
                            </div>
                            

                        </div>
                    </div>
                </section>

    </form>
    <script src="../../plugins/jquery/jquery.min.js"></script>
    <script src="../../plugins/bootstrap/js/bootstrap.bundle.min.js"></script>


</body>
</html>
