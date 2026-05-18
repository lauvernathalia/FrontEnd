<%@ Page Language="C#" AutoEventWireup="true" CodeFile="boleto.aspx.cs" Inherits="boleto" %>

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
            margin-top: 2pt;
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

    <button class="btn btn-whitelabel1" onclick="printDiv('dvReciboEstabelecimento')"><i class="fas fa-print" aria-hidden="true" style="font-size: 17px;">     Imprimir</i></button>
    <div class="pagina" runat="server" id="dvReciboEstabelecimento">
        <table>
            <tr>
                <td class="borda-left logo cabecalho">
                    <img src="images/banco_asaas.png" width="128">
                </td>
                <td class="borda-left cabecalho center">341-7</td>
                <td class="sem-borda cabecalho right" colspan="5">
                    Recibo do Pagador<br>
                    12345.12345 12345.121212 12345.121212 8 12345678901112
                </td>
            </tr>
            <tr>
                <td class="left" colspan="6">
                    <p>Local de Pagamento</p>
                    Pagável em qualquer banco ou casa lotérica 
                </td>
                <td class="center">
                    <p>Data de Vencimento</p>
                    <asp:Label runat="server" ID="lblVencimento" Text="30/10/2024"></asp:Label>
                </td>
            </tr>
            <tr>
                <td class="left" colspan="6">
                    <p>Nome do Beneficiário / CNPJ / CPF / Endereço:</p>
                    Fulano de Tal CPF: 123.456.789.10 <br>
                    Rua Suvaco da Cobra, 9 - Narnia - Amazonas - AM - 69060-000
                </td>
                <td class="center">
                    <p>Agência/Código Beneficiário</p>
                    123/54321-01
                </td>
            </tr>
            <tr>
                <td class="center">
                    <p>Data do documento</p>
                    01/01/2001
                </td>
                <td class="center" colspan="2">
                    <p>Núm. do documento</p>
                    123
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
                    01/01/2001
                </td>
                <td class="center">
                    <p>Nosso Número</p>123456789
                </td>
            </tr>
            <tr>
                <td class="center">
                    <p>Uso do Banco</p>
                    <br>
                </td>
                <td class="center">
                    <p>Carteira</p>
                    157
                </td>
                <td class="center">
                    <p>Espécie</p>
                    R$
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
                    10,99
                </td>
            </tr>
            <tr>
                <td class="left" colspan="6" rowspan="3">
                    <p>Instruções</p>
                    Lorem ipsum dolor sit amet consectetur adipisicing elit. Ullam officia labore reprehenderit numquam
                    doloribus ut porro laboriosam itaque ipsa ratione.
                </td>
                <td class="center">
                    <p>(-) Descontos/Abatimentos</p>
                    02/01/2001
                </td>
            </tr>
            <tr>
                <td class="center">
                    <p>(+) Juros/Multa</p>
                    02/01/2001
                </td>
            </tr>
            <tr>
                <td class="center">
                    <p>(=) Valor Pago</p>
                    <br>
                </td>
            </tr>
            <tr>
                <td colspan="7">
                    <table>
                        <tr>
                            <td class="sem-borda"><b>Nome do Pagador: </b> Fulano de Tal 2</td>
                            <td class="sem-borda"><b>CPF/CNPJ: </b> 123.123.123-00</td>
                        </tr>
                        <tr>
                            <td class="sem-borda"><b>Endereço: </b> Av. André Araujo, 999 - Aleixo - Amazonas - AM -
                                69060-000
                            </td>
                        </tr>
                        <tr>
                            <td class="sem-borda"><b>Sacador/Avalista: </b> Fulano de Tal 2</td>
                            <td class="sem-borda"><b>CPF/CNPJ: </b> 123.123.123-00</td>
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
                <td class="borda-left cabecalho center">341-7</td>
                <td class="sem-borda cabecalho right" colspan="5">
                    12345.12345 12345.121212 12345.121212 8 12345678901112
                </td>
            </tr>
            <tr>
                <td class="left" colspan="6">
                    <p>Local de Pagamento</p>
                    Pagável em qualquer banco ou casa lotérica
                </td>
                <td class="center">
                    <p>Data de Vencimento</p>
                    02/01/2001
                </td>
            </tr>
            <tr>
                <td class="left" colspan="6">
                    <p>Nome do Beneficiário / CNPJ / CPF / Endereço:</p>
                    Fulano de Tal CPF: 123.456.789.10 <br>
                    Rua Suvaco da Cobra, 9 - Narnia - Amazonas - AM - 69060-000
                </td>
                <td class="center">
                    <p>Agência/Código Beneficiário</p>
                    123/54321-01
                </td>
            </tr>
            <tr>
                <td class="center">
                    <p>Data do documento</p>
                    01/01/2001
                </td>
                <td class="center" colspan="2">
                    <p>Núm. do documento</p>
                    123
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
                    01/01/2001
                </td>
                <td class="center">
                    <p>Nosso Número</p>123456789
                </td>
            </tr>
            <tr>
                <td class="center">
                    <p>Uso do Banco</p>
                    <br>
                </td>
                <td class="center">
                    <p>Carteira</p>
                    157
                </td>
                <td class="center">
                    <p>Espécie</p>
                    R$
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
                    10,99
                </td>
            </tr>
            <tr>
                <td class="left" colspan="6" rowspan="3">
                    <p>Instruções</p>
                    Lorem ipsum dolor sit amet consectetur adipisicing elit. Ullam officia labore reprehenderit numquam
                    doloribus ut porro laboriosam itaque ipsa ratione.
                </td>
                <td class="center">
                    <p>(-) Descontos/Abatimentos</p>
                    02/01/2001
                </td>
            </tr>
            <tr>
                <td class="center">
                    <p>(+) Juros/Multa</p>
                    02/01/2001
                </td>
            </tr>
            <tr>
                <td class="center">
                    <p>(=) Valor Pago</p>
                    <br>
                </td>
            </tr>
            <tr>
                <td colspan="7">
                    <table>
                        <tr>
                            <td class="sem-borda"><b>Nome do Pagador: </b> Fulano de Tal 2</td>
                            <td class="sem-borda"><b>CPF/CNPJ: </b> 123.123.123-00</td>
                        </tr>
                        <tr>
                            <td class="sem-borda"><b>Endereço: </b> Av. André Araujo, 999 - Aleixo - Amazonas - AM -
                                69060-000
                            </td>
                        </tr>
                        <tr>
                            <td class="sem-borda"><b>Sacador/Avalista: </b> Fulano de Tal 2</td>
                            <td class="sem-borda"><b>CPF/CNPJ: </b> 123.123.123-00</td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
        <br>
        <table>
            <tr>
                <td class="sem-borda" rowspan="2">
                    <img class="codigo-barras" src="codigo-barras.png">
                </td>
                <td class="sem-borda cabecalho right">
                    Ficha de Compensação
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