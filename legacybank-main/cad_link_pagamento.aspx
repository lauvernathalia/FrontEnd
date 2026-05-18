<%@ Page Language="C#" AutoEventWireup="true" CodeFile="cad_link_pagamento.aspx.cs" Inherits="cad_link_pagamento" %>

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

        <div runat="server" id="dvInformacoes" visible="true">

            <div class="row">
                <div class="col-12">
                    <div class="card">
                        <div class="card-header bg-whitelabel1">
                            <h3 class="card-title">LINK DE PAGAMENTO</h3>
                        </div>
                        <div class="card-body">
                            
                            <div class="row text-center" id="dvStep01" runat="server">
                                <div class="col-12">
                                    <div style="text-align:center;margin-top:40px;">
                                    <h3 style="text-align:center;">
                                        <span class="step active">1</span>
                                        <span class="step">2</span>
                                        <span class="step">3</span>
                                    </h3>
                                    </div>
                                </div>
                            </div>
                            <div class="row mt-3 text-center">
                                <div class="col-12">
                                    <h2 class="text-whitelabel1 text-center"><b>INFORMAÇÕES GERAIS</b></h2>                            
                                </div>
                            </div>

                            <div class="row ">
                                <div class="col-sm-4">
                                    <div class="form-group">
                                        <asp:Label runat="server" ID="Label22" Text="Valor"></asp:Label>
                                        <asp:TextBox runat="server" id="txtCodigo" cssClass="form-control" Visible="false" ></asp:TextBox>
                                        <asp:TextBox runat="server" id="txtValorTotal" cssClass="form-control" placeholder="0,00" ></asp:TextBox>
                                    </div>
                                </div>                                
                                <div class="col-sm-4">
                                    <div class="form-group">
                                        <asp:Label runat="server" ID="Label8" Text="Nome do link de Pagamento"></asp:Label>
                                        <asp:TextBox runat="server" id="txtReferencia" cssClass="form-control" placeholder="Ex.: Venda de Produtos/Serviços" ></asp:TextBox>
                                    </div>
                                </div>                                
                                <div class="col-sm-4">
                                    <div class="form-group">
                                        <asp:Label runat="server" ID="Label4" Text="Tipo do link?"></asp:Label>
                                        <asp:DropDownList runat="server" ID="ddlLinkPermanente" CssClass="form-control">
                                            <asp:ListItem Text="Cobrança única" Value="N"></asp:ListItem>
                                            <asp:ListItem Text="Permanente" Value="S"></asp:ListItem>
                                        </asp:DropDownList>
                                    </div>
                                </div>                                


                            </div>

                            <div class="row ">
                                <div class="col-sm-12">
                                    <div class="form-group">
                                        <asp:Label runat="server" ID="Label9" Text="Descrição da cobrança (Opcional)"></asp:Label>
                                        <asp:TextBox runat="server" id="txtDescricao" TextMode="MultiLine" Rows="5" cssClass="form-control" placeholder="Descrição detalhada da finalidade do link de pagamento" ></asp:TextBox>
                                    </div>
                                </div>                                
                            </div>
                            <div class="row ">

                            </div>                            
                            <div class="row">
                                <div class="col-sm-12">
                                    <div class="form-group">
                                        <asp:Label runat="server" ID="Label5" Text="Especifique abaixo o nome da informação adicional que deseja coletar no checkout"></asp:Label>
                                        <asp:TextBox runat="server" id="txtCampo01" cssClass="form-control" ></asp:TextBox>
                                    </div>
                                </div>   
                            </div>                            

                            <div class="row">
                                <div class="col-sm-12">
                                    <div class="form-group">
                                        <asp:Label runat="server" ID="Label13" Text="Imagem do link de pagamento (Opcional)"></asp:Label>
                                        <asp:TextBox runat="server" ID="txtImagem" CssClass="form-control"></asp:TextBox> 
                                    </div>
                                </div>

                            </div>
                            <div class="row">
                                <div class="col-sm-12">
                                    <div class="form-group">
                                        <div class="input-group">
                                            <div class="custom-file">
                                                <input runat="server" id="flFoto" type="file" name="attachment" />
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>

                        </div>
                        <div class="card-footer">
                            <center>
                                <asp:Button runat="server" ID="btnCancelar" CssClass="btn btn-danger" Text="Cancelar" onclick="btnCancelar_Click"/>
                                <asp:Button runat="server" ID="btnAvancar01" CssClass="btn btn-whitelabel1" Text="Avançar" onclick="btnAvancar01_Click"/>
                            </center>
                        </div>

                    </div>
                </div>
            </div>        

        </div>


        <div runat="server" id="dvFormaPagamento" visible="false">

            <div class="row">
                <div class="col-12">
                    <div class="card">
                        <div class="card-header bg-whitelabel1">
                            <h3 class="card-title">LINK DE PAGAMENTO</h3>
                        </div>
                        <div class="card-body">
                            <div class="row text-center" id="dvStep02" runat="server">
                                <div class="col-12">
                                    <div style="text-align:center;margin-top:40px;">
                                    <h3 style="text-align:center;">
                                        <span class="step finish">1</span>
                                        <span class="step active">2</span>
                                        <span class="step">3</span>
                                    </h3>
                                    </div>
                                </div>
                            </div>

                            <div class="row mt-3 text-center">
                                <div class="col-12">
                                    <h2 class="text-whitelabel1 text-center"><b>FORMA DE PAGAMENTO</b></h2>                            
                                </div>
                            </div>

                            <div class="row">
                                <div class="col-lg-4 col-md-4 col-12">
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
                                <div class="col-lg-4 col-md-4 col-12">
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

                                <div class="col-lg-4 col-md-4 col-12">
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
                                <div class="col-12 col-sm-12 col-md-12">
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
                            </div>
                            <div class="row">
                            
                                <div class="col-sm-6">
                                    <div class="form-group">
                                        <asp:Label runat="server" ID="Label1" Text="Quantidade de dias para vencimento?"></asp:Label>
                                        <asp:DropDownList runat="server" ID="ddlDias" CssClass="form-control">
                                            <asp:ListItem Text="1" Value="1"></asp:ListItem>
                                            <asp:ListItem Text="2" Value="2"></asp:ListItem>
                                            <asp:ListItem Text="5" Value="5"></asp:ListItem>
                                            <asp:ListItem Text="10" Value="10"></asp:ListItem>
                                            <asp:ListItem Text="15" Value="15"></asp:ListItem>
                                            <asp:ListItem Text="30" Value="30"></asp:ListItem>
                                        </asp:DropDownList>
                                    </div>
                                </div>                                
                                <div class="col-sm-6">
                                    <div class="form-group">
                                        <asp:Label runat="server" ID="Label2" Text="Data Limite (Opcional)"></asp:Label>
                                        <asp:TextBox runat="server" id="txtDataLimite" cssClass="form-control" placeholder="00/00/0000" data-inputmask='"mask": "99/99/9999"' data-mask ></asp:TextBox>
                                    </div>
                                </div>
                            </div>


                        </div>
                        <div class="card-footer">
                            <center>
                                <asp:Button runat="server" ID="btnVoltar02" CssClass="btn btn-danger" Text="Voltar" onclick="btnVoltar02_Click" Visible="true"/>
                                <asp:Button runat="server" ID="btnAvancar02" CssClass="btn btn-whitelabel1" Text="Avançar" onclick="btnAvancar02_Click" Visible="true"/>
                            </center>
                        </div>

                    </div>
                </div>
            </div>        

        </div>

        <div runat="server" id="dvResumo" visible="false">

            <div class="row">
                <div class="col-12">
                    <div class="card">
                        <div class="card-header bg-whitelabel1">
                            <h3 class="card-title">LINK DE PAGAMENTO</h3>
                        </div>
                        <div class="card-body">
                            <div class="row text-center" id="dvStep03" runat="server">
                                <div class="col-12">
                                    <div style="text-align:center;margin-top:40px;">
                                    <h3 style="text-align:center;">
                                        <span class="step finish">1</span>
                                        <span class="step finish">2</span>
                                        <span class="step active">3</span>
                                    </h3>
                                    </div>
                                </div>
                            </div>

                            <div class="row mt-3 text-center">
                                <div class="col-sm-12">
                                    <h2 class="text-whitelabel1 text-center"><b>RESUMO</b></h2>                            
                                </div>
                            </div>



                            <div class="row">
                                <div class="col-sm-12">
                                    <h4>Confira os dados abaixo para finalizar o link de pagamento</h4>
                                </div>
                            </div>

                            <div class="row">
                                <div class="col-sm-6">
                                    <div class="card">
                                        <div class="card-header">
                                            <h3 class="card-title">INFORMAÇÕES DO LINK DE PAGAMENTO</h3>
                                        </div>
                                        <div class="card-body">
                                            <div class="col-sm-12">
                                                <div class="form-group">
                                                    <label runat="server" ID="Label3" class="text-whitelabel1">Valor</label><br />
                                                    <asp:Label runat="server" ID="lblValor"></asp:Label>
                                                </div>
                                            </div>
                                            <div class="col-sm-12">
                                                <div class="form-group">
                                                    <label runat="server" ID="Label6" class="text-whitelabel1">Nome</label><br />
                                                    <asp:Label runat="server" ID="lblNome"></asp:Label>
                                                </div>
                                            </div>
                                            <div class="col-sm-12">
                                                <div class="form-group">
                                                    <label runat="server" ID="Label7" class="text-whitelabel1">Descrição</label><br />
                                                    <asp:Label runat="server" ID="lblDescricao"></asp:Label>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <div class="col-sm-6">
                                    <div class="card">
                                        <div class="card-header">
                                            <h3 class="card-title">FORMAS DE PAGAMENTO</h3>
                                        </div>
                                        <div class="card-body">

                                            <div class="col-sm-12">
                                                <div class="form-group">
                                                    <label runat="server" ID="Label10" class="text-whitelabel1">Forma Pagamento</label><br />
                                                    <asp:Label runat="server" ID="lblFormaPagamento"></asp:Label>
                                                </div>
                                            </div>
                                            <div class="col-sm-12">
                                                <div class="form-group">
                                                    <label runat="server" ID="Label12" class="text-whitelabel1">Dias Vencimento</label><br />
                                                    <asp:Label runat="server" ID="lblDias"></asp:Label>
                                                </div>
                                            </div>
                                            <div class="col-sm-12">
                                                <div class="form-group">
                                                    <label runat="server" ID="Label15" class="text-whitelabel1">Data Limite</label><br />
                                                    <asp:Label runat="server" ID="lblDataLimite"></asp:Label>
                                                </div>
                                            </div>

                                            <div class="col-sm-12">
                                                <div class="form-group">
                                                    <label runat="server" ID="Label11" class="text-whitelabel1">Imagem</label><br />
                                                    <asp:Label runat="server" ID="lblFoto"></asp:Label>
                                                </div>
                                            </div>

                                        </div>
                                    </div>
                                </div>
                            </div>

                        </div>

                        <div class="card-footer">
                            <center>
                                <asp:Button runat="server" ID="btnVoltar03" CssClass="btn btn-sm btn-danger" Text="Voltar" onclick="btnVoltar03_Click" Visible="true"/>
                                <asp:Button runat="server" ID="btnConfirmar" CssClass="btn btn-sm btn-whitelabel1" Text="Confirmar" onclick="btnConfirmar_Click" Visible="true"/>
                                <asp:Button ID="btnPostback" runat="server" Visible="false" OnClick="btnPostBack_Click" />
                                <script type="text/javascript">
                                    function PostBackOnMainPage(){
                                    <%=GetPostBackScript()%>
                                    }
                                </script>

                            </center>
                        </div>

                    </div>
                </div>
            </div>        

        </div>



        <!-- ******************************************************************************************************************************************************************** -->
        <!-- LINK ANTIGO
        <!-- ******************************************************************************************************************************************************************** -->


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


<script>
    $(function () {
        $('#txtDataLimite').inputmask('99/99/9999')
    })
</script>


</body>
</html>
