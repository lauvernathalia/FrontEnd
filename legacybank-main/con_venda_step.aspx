<%@ Page Language="C#" AutoEventWireup="true" CodeFile="con_venda_step.aspx.cs" Inherits="con_venda_step" %>

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
        <style>
            .stepper {
                display: flex;
                justify-content: center;
                align-items: center;
                margin-bottom: 20px;
            }

            .step {
                width: 40px;
                height: 40px;
                border-radius: 50%;
                background-color: #ddd;
                color: black;
                display: flex;
                align-items: center;
                justify-content: center;
                font-weight: bold;
                margin: 0 10px;
                border: 2px solid #ccc;
            }

            .step.active {
                background-color: #4CAF50;
                color: white;
                border-color: #4CAF50;
            }

            .step-line {
                width: 50px;
                height: 4px;
                background-color: #ccc;
            }

            .step-line.active {
                background-color: #4CAF50;
            }
        </style>

        <div class="stepper">
            <div class='<%= GetStepClass(0) %>'>1</div>
            <div class='<%= GetLineClass(0) %>'></div>
            <div class='<%= GetStepClass(1) %>'>2</div>
            <div class='<%= GetLineClass(1) %>'></div>
            <div class='<%= GetStepClass(2) %>'>3</div>
            <div class='<%= GetLineClass(2) %>'></div>
            <div class='<%= GetStepClass(3) %>'>4</div>
        </div>

        <asp:MultiView ID="multiViewWizard" runat="server" ActiveViewIndex="0">

            <asp:View ID="Passo1" runat="server">
                <h2>Passo 1</h2>
                <p>Digite seu nome:</p>
                <asp:TextBox ID="txtNome" runat="server"></asp:TextBox>
                <br /><br />
                <asp:Button ID="btnNext1" runat="server" Text="Próximo" OnClick="btnNext1_Click" />
            </asp:View>

            <asp:View ID="Passo2" runat="server">
                <h2>Passo 2</h2>
                <p>Digite seu e-mail:</p>
                <asp:TextBox ID="txtEmail" runat="server"></asp:TextBox>
                <br /><br />
                <asp:Button ID="btnBack1" runat="server" Text="Voltar" OnClick="btnBack1_Click" />
                <asp:Button ID="btnNext2" runat="server" Text="Próximo" OnClick="btnNext2_Click" />
            </asp:View>

            <asp:View ID="Passo3" runat="server">
                <h2>Passo 3</h2>
                <p>Digite seu endereço:</p>
                <asp:TextBox ID="txtEndereco" runat="server"></asp:TextBox>
                <br /><br />
                <asp:Button ID="btnBack2" runat="server" Text="Voltar" OnClick="btnBack2_Click" />
                <asp:Button ID="btnNext3" runat="server" Text="Próximo" OnClick="btnNext3_Click" />
            </asp:View>

            <asp:View ID="View1" runat="server">
                <h2>Passo 4</h2>
                <p>Revisão dos dados:</p>
                <asp:Label ID="lblResumo" runat="server"></asp:Label>
                <br /><br />
                <asp:Button ID="btnBack3" runat="server" Text="Voltar" OnClick="btnBack3_Click" />
                <asp:Button ID="btnFinish" runat="server" Text="Finalizar" OnClick="btnFinish_Click" />
            </asp:View>

        </asp:MultiView>

    </form>
<script type="text/javascript" src="../plugins/moment/moment.min.js"></script>
<script type="text/javascript" src="../plugins/moment/moment-with-locales.js"></script>
<script type="text/javascript" src="../plugins/inputmask/min/jquery.inputmask.bundle.min.js"></script>
<script type="text/javascript" src="../plugins/daterangepicker/daterangepicker.js"></script>
<script type="text/javascript" src="../plugins/bootstrap-colorpicker/js/bootstrap-colorpicker.min.js"></script>
<script type="text/javascript" src="../plugins/tempusdominus-bootstrap-4/js/tempusdominus-bootstrap-4.min.js"></script>

<script type="text/javascript" src="../plugins/chart.js/Chart.min.js"></script>
<script type="text/javascript" src="../dist/js/demo.js"></script>

</body>
</html>
