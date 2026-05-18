<%@ Page Language="C#" AutoEventWireup="true" CodeFile="con_pagamento_extrato_comprovante.aspx.cs" Inherits="con_pagamento_extrato_comprovante" %>

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

<body class="hold-transition sidebar-mini layout-fixed bg-secondary">
    <form id="frmComprovante" runat="server" visible="true">

          <div class="row justify-content-md-center vh-100 d-flex flex-column justify-content-center align-items-center">
            <div class="col-md-6">
                <div class="row">
                    <div class="col-12">
                        <div class="card vh-100">
                            <div class="card-header">
                                <center>
                                <h6 class="text-whitelabel1">COMPROVANTE</h6>
                                </center>
                            </div>
                            <div class="card-body">
                            </div>
                        </div>
                    </div>
                </div>
            </div>
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