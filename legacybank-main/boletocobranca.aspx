<%@ Page Language="C#" AutoEventWireup="true" CodeFile="boletocobranca.aspx.cs" Inherits="boletocobranca" %>

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

    <style>
        .pagina {
            width: 210mm;
            font-family: Helvetica,
                Arial,
                "Lucida Grande",
                sans-serif;
            font-size: 8pt;
            font-weight: bold;
            margin: 5mm auto;
        }

        table {
            width: 100%;
            border-collapse: collapse;
        }

        td {
            border: 1pt black solid;
            padding-left: 2pt;
            padding-right: 2pt;
            font-weight: normal;
        }

        td.center {
            text-align: center;
            vertical-align: super;
        }

        td.left {
            text-align: left;
            vertical-align: super;
        }

        td.right {
            text-align: right;
            vertical-align: super;
        }

        p {
            font-weight: bold;
            text-align: left;
            white-space: nowrap;
            margin:0;
            padding:0;
        }

        td.sem-borda {
            border: none;
        }

        td.borda-left {
            border: none;
            border-right: 1pt solid;
            vertical-align: bottom;
        }

        td.cabecalho {
            padding: 0 2mm 0 2mm;
            font-weight: bolder;
            font-size: 10pt;
            vertical-align: bottom;
        }

        td.logo {
            font-size: 8pt;
        }

        hr {
            border-top: 1pt dashed
        }

        p.lb-autenticacao {
            text-align: right;
            margin-right: 5mm;
        }

        .codigo-barras {
            width: 116mm;
            height: 60px;
        }

        @media print {
            .noprint {
                display: none;
            }

            .pagina {
                width: 100%;
            }
        }
    </style>
</head>

<body class="hold-transition sidebar-mini layout-fixed">
    <form id="frmPrincipal" runat="server">

    <center>
    <button class="btn btn-outline-secondary mb-3 mt-2" onclick="printDiv('dvReciboEstabelecimento')"><i class="fas fa-print" aria-hidden="true"></i>     Imprimir boleto bancário</button>
    </center>

    <div class="row" runat="server" id="dvCancelado" visible="false">
        <div class="col-12">
            <center>
            <h1 class="text-danger"><strong>* BOLETO BANCÁRIO CANCELADO *</strong></h2>
            </center>
        </div>
    </div>

    <div class="pagina" runat="server" id="dvTopoBoleto">
    <table>
        <tr>
            <td class="sem-borda">
                <div class="row mt-2 mb-2">
                    <div class="col-6">
                        <h6><asp:Label runat="server" ID="lblComprador" Text=""></asp:Label></h6>
                        <h6><asp:Label runat="server" ID="lblMensagemEntrada" Text=""></asp:Label></h6>
                    </div>
                    <div class="col-6">
                        <center>
                        <img runat="server" id="imgLogoPrincipal" class="img-fluid p-2 mt-2 mb-2 col-6" />
                        </center>
                    </div>
                </div>

                <div class="row">
                    <div class="col-12">
                        <h5><asp:Label runat="server" ID="lblComoPagar" Text="Como realizar o pagamento"></asp:Label></h5>
                    </div>
                </div>
                    

                <div class="row">
                    <div class="col-10">
                        <h6><i class="fas fa-barcode"></i>  Linha Digitável<br /></h6>
                        <asp:TextBox runat="server" ID="lblLinhaDigitavel" Text="" CssClass="form-control"></asp:TextBox><br />
                        <h6><i class="fas fa-qrcode"></i>  Pix copia e cola<br /></h6>
                        
                        <asp:LinkButton ID="lbkCopiar" runat="server" CssClass="btn btn-default" ToolTip="copiar"><i class="fas fa-copy"></i></asp:LinkButton>
                        <asp:TextBox runat="server" ID="lblPixCopiaCola" Text="" TextMode="MultiLine" Rows="3" CssClass="form-control"></asp:TextBox>
                    </div>
                    <div class="col-2">
                        <center>
                        <strong><i class="fab fa-pix"></i>  Pague o boleto com Pix usando o QRcode abaixo<br /></strong>
                        <img runat="server" class="img-fluid" id="imgQRcode" src="" alt=""  />                                    
                        </center>
                    </div>
                </div>
            </td>
        </tr>
    </table>
    </div>

    <div class="pagina" runat="server" id="dvReciboEstabelecimento">

        <table>
            <tr>
                <td class="borda-left logo cabecalho">
                    <img src="images/banco_asaas.png" width="128">
                </td>
                <td class="borda-left cabecalho center"><h5><asp:Label runat="server" ID="lblCodigoBanco" Text="461"></asp:Label></h5></td>
                <td class="sem-borda cabecalho right" colspan="5">
                    Recibo do Pagador<br>
                    <h5><asp:Label runat="server" ID="lblLinhaDigitavelPagador" Text=""></asp:Label></h5>
                </td>
            </tr>
            <tr>
                <td class="left" colspan="6">
                    <p>Local de Pagamento</p>
                    <asp:Label runat="server" ID="lblLocalPagamentoPagador" Text="Pagável em qualquer banco ou casa lotérica"></asp:Label>
                </td>
                <td class="left">
                    <p>Data de Vencimento</p>
                    <asp:Label runat="server" ID="lblVencimentoPagador" Text=""></asp:Label>
                </td>
            </tr>
            <tr>
                <td class="left" colspan="6">
                    <b>Beneficiário: </b><asp:Label runat="server" ID="lblBeneficiarioPagador" Text=""></asp:Label><br />
                    <b>CNPJ / CPF: </b><asp:Label runat="server" ID="lblDocumentoBeneficiarioPagador" Text=""></asp:Label>
                </td>
                <td class="center">
                    <p>Agência/Código Beneficiário</p>
                    <asp:Label runat="server" ID="lblContaPagador"></asp:Label>
                </td>
            </tr>
            <tr>
                <td class="center">
                    <p>Data do documento</p>
                    <asp:Label runat="server" ID="lblDataDocumentoPagador" Text=""></asp:Label>
                </td>
                <td class="center" colspan="2">
                    <p>Núm. do documento</p>
                    <asp:Label runat="server" ID="lblNumeroDocumentoPagador" Text=""></asp:Label>
                </td>
                <td class="center">
                    <p>Espécie doc</p>
                    DM
                </td>
                <td class="center">
                    <p>Aceite</p>
                    N
                </td>
                <td class="center">
                    <p>Data Processamento</p>
                    <asp:Label runat="server" ID="lblDataProcessamentoPagador" Text=""></asp:Label>

                </td>
                <td class="center">
                    <p>Nosso Número</p><asp:Label runat="server" ID="lblNossoNumeroPagador"></asp:Label>
                </td>
            </tr>
            <tr>
                <td class="center">
                    <p>Uso do Banco</p>
                    <br>
                </td>
                <td class="center">
                    <p>Carteira</p>
                    <asp:Label runat="server" ID="lblCarteiraPagador" Text="1"></asp:Label>
                </td>
                <td class="center">
                    <p>Espécie</p>
                    <asp:Label runat="server" ID="lblEspeciePagador" Text="REAL"></asp:Label>

                </td>
                <td class="center" colspan="2">
                    <p>Quantidade</p>
                    <br>
                </td>
                <td class="center">
                    <p>Valor</p>
                    <br>
                </td>
                <td class="center">
                    <p>(=) Valor do Documento</p>
                    <asp:Label runat="server" ID="lblValorDocumentoPagador" Text=""></asp:Label>
                </td>
            </tr>
            <tr>
                <td class="left" colspan="6" rowspan="3">
                    <p>Instruções</p>
                    <asp:Label runat="server" ID="lblInstrucoesPagador" Text=""></asp:Label>
                </td>
                <td class="center">
                    <p>(-) Descontos/Abatimentos</p><br />
                    <asp:Label runat="server" ID="lblDescontoPagador" Text=""></asp:Label>
                </td>
            </tr>
            <tr>
                <td class="center">
                    <p>(+) Juros/Multa</p><br />
                    <asp:Label runat="server" ID="lblJurosPagador" Text=""></asp:Label>
                </td>
            </tr>
            <tr>
                <td class="center">
                    <p>(=) Valor Pago</p>
                    <br />
                </td>
            </tr>
            <tr>
                <td colspan="7">
                    <table>
                        <tr>
                            <td class="sem-borda"><b>Nome do Pagador: </b><asp:Label runat="server" ID="lblNomeCompradorPagador" Text=""></asp:Label></td>
                            <td class="sem-borda"><b>CPF/CNPJ: </b><asp:Label runat="server" ID="lblDocumentoCompradorPagador" Text=""></asp:Label></td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
        <p class="lb-autenticacao"><b>Autenticação Mecânica</b></p>
        <hr><br>
        <table>
            <tr>
                <td class="borda-left logo cabecalho ">
                    <img src="images/banco_asaas.png" width="128">
                </td>
                <td class="borda-left cabecalho center"><h5><asp:Label runat="server" ID="Label1" Text="461"></asp:Label></h5></td>
                <td class="sem-borda cabecalho right" colspan="5">
                    <h5><asp:Label runat="server" ID="lblLinhaDigitavelCompensacao" Text=""></asp:Label></h5>
                </td>
            </tr>
            <tr>
                <td class="left" colspan="6">
                    <p>Local de Pagamento</p>
                    <asp:Label runat="server" ID="lblLocalPagamentoCompensacao" Text="Pagável em qualquer banco ou casa lotérica"></asp:Label>
                </td>
                <td class="center">
                    <p>Data de Vencimento</p>
                    <asp:Label runat="server" ID="lblVencimentoCompensacao" Text=""></asp:Label>

                </td>
            </tr>
            <tr>
                <td class="left" colspan="6">
                    <b>Beneficiário: </b><asp:Label runat="server" ID="lblBeneficiarioCompensacao" Text=""></asp:Label><br />
                    <b>CNPJ / CPF: </b><asp:Label runat="server" ID="lblDocumentoBeneficiarioCompensacao" Text=""></asp:Label>
                </td>
                <td class="center">
                    <p>Agência/Código Beneficiário</p>
                    <asp:Label runat="server" ID="lblContaCompensacao"></asp:Label>
                </td>
            </tr>
            <tr>
                <td class="center">
                    <p>Data do documento</p>
                    <asp:Label runat="server" ID="lblDataDocumentoCompensacao" Text=""></asp:Label>

                </td>
                <td class="center" colspan="2">
                    <p>Núm. do documento</p>
                    <asp:Label runat="server" ID="lblNumeroDocumentoCompensacao" Text=""></asp:Label>
                </td>
                <td class="center">
                    <p>Espécie doc</p>
                    DM
                </td>
                <td class="center">
                    <p>Aceite</p>
                    N
                </td>
                <td class="center">
                    <p>Data Processamento</p>
                    <asp:Label runat="server" ID="lblDataProcessamentoCompensacao" Text=""></asp:Label>
                </td>
                <td class="center">
                    <p>Nosso Número</p><asp:Label runat="server" ID="lblNossoNumeroCompensacao"></asp:Label>
                </td>
            </tr>
            <tr>
                <td class="center">
                    <p>Uso do Banco</p>
                    <br>
                </td>
                <td class="center">
                    <p>Carteira</p>
                    <asp:Label runat="server" ID="lblCarteiraCompensacao" Text="1"></asp:Label>

                </td>
                <td class="center">
                    <p>Espécie</p>
                    <asp:Label runat="server" ID="lblEspecieCompensacao" Text="REAL"></asp:Label>
                </td>
                <td class="center" colspan="2">
                    <p>Quantidade</p>
                    <br>
                </td>
                <td class="center">
                    <p>Valor</p>
                    <br>
                </td>
                <td class="center">
                    <p>(=) Valor do Documento</p>
                    <asp:Label runat="server" ID="lblValorDocumentoCompensacao" Text=""></asp:Label>

                </td>
            </tr>
            <tr>
                <td class="left" colspan="6" rowspan="3">
                    <p>Instruções</p>
                    <asp:Label runat="server" ID="lblInstrucoesCompensacao" Text=""></asp:Label>
                </td>
                <td class="center">
                    <p>(-) Descontos/Abatimentos</p><br />
                    <asp:Label runat="server" ID="lblDescontoCompensacao" Text=""></asp:Label>
                </td>
            </tr>
            <tr>
                <td class="center">
                    <p>(+) Juros/Multa</p><br />
                    <asp:Label runat="server" ID="lblJurosCompensacao" Text=""></asp:Label>
                </td>
            </tr>
            <tr>
                <td class="center">
                    <p>(=) Valor Pago</p>
                    <br />
                </td>
            </tr>

            <tr>
                <td class="left" colspan="6">
                    <br />
                </td>
                <td class="center">
                    <img runat="server" class="col-6" id="imgQRcodeBoleto" src="" alt="" />
                </td>
            </tr>

            <tr>
                <td colspan="7">
                    <table>
                        <tr>
                            <td class="sem-borda"><b>Nome do Pagador: </b><asp:Label runat="server" ID="lblNomeCompradorCompensacao" Text=""></asp:Label></td>
                            <td class="sem-borda"><b>CPF/CNPJ: </b><asp:Label runat="server" ID="lblDocumentoCompradorCompensacao" Text=""></asp:Label></td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
        <br>
        <table>
            <tr>
                <td class="sem-borda" rowspan="2">
                    <div class="i2of5"><asp:Label runat="server" ID="lblCodigoBarrasCompensacao" Text=""></asp:Label></div><br />
                </td>
                <td class="sem-borda cabecalho right">
                    <small>Ficha de Compensação</small>
                </td>
            </tr>
            <tr>
                <td class="sem-borda right">
                    <p class="lb-autenticacao"><b>Autenticação Mecânica</b></p>
                </td>
            </tr>
        </table>
    </div>
    </form>

<script type="text/javascript">
    function printDiv(divName) {
        //var printContents = document.getElementById(divName).innerHTML;
        //var originalContents = document.body.innerHTML;
        //document.body.innerHTML = printContents;
        window.print();
        //document.body.innerHTML = originalContents;
    }
</script>

</body>

</html>