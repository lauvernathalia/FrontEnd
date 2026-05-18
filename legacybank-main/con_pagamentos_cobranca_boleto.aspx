<%@ Page Language="C#" AutoEventWireup="true" CodeFile="con_pagamentos_cobranca_boleto.aspx.cs" Inherits="con_pagamentos_cobranca_boleto" %>

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

                <div class="row">
                    <div class="col-sm-12">
                        <h5 class="text-whitelabel1">Criar nova cobrança via Boleto </h5>
                    </div>
                </div>

                <div class="row">
                    <div class="col-sm-12">
                        <div class="card">
                            <div class="card-header">
                                <h3 class="card-title">INSERIR NOVA COBRANÇA</h3>
                            </div>
                            <div class="card-body">

                            <!-- DADOS REFERENTE A COBRANÇA -->
                                <div class="row" runat="server" id="divDadosCobranca" visible="true">
                                    <div class="col-12">

                                        <div class="row">
                                            <div class="col-sm-12">
                                                <h5 class="text-whitelabel1">Preencha os dados da cobrança</h5>
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

                                <div class="row" runat="server" id="divDadosCliente" visible="true">
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
                                <div class="row" runat="server" id="divDetalhesCobranca" visible="true">
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
                                <div class="row" runat="server" id="divSplitCobranca" visible="true">
                                    <div class="col-12">

                                        <div class="row">
                                            <div class="col-sm-12">
                                                <h5 class="text-whitelabel1">Preencha os dados referente aos detalhes do Split</h5>
                                            </div>
                                        </div>

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
                                                <div class="row">
                                                    <div class="col-sm-12">
                                                        <h5 class="text-whitelabel1">Selecione os beneficiários do split</h5>
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
                                <div class="row" runat="server" id="divLinkEmail" visible="true">
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


</body>
</html>
