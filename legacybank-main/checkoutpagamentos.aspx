<%@ Page Language="C#" AutoEventWireup="true" CodeFile="checkoutpagamentos.aspx.cs" Inherits="checkoutpagamentos" %>

<%@ Register TagPrefix="Portal" TagName="PageRotina" Src="rotinaspadroes.ascx" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">


<head id="Head1" runat="server">
    <meta charset="utf-8">
    <meta name="viewport" content="width=device-width, initial-scale=1">
    <meta http-equiv="x-ua-compatible" content="ie=edge">

    <title>CHECKOUT PAGAMENTOS</title>
    <link rel="icon" type="image/x-icon" href="../images/favicon-96x96.png">

    <PORTAL:PAGEROTINA id="PageRotina1" title="Site Directory" runat="server" ModuleSource="rotinaspadroes.ascx"></PORTAL:PAGEROTINA>
    <link rel="stylesheet" href="../plugins/fullcalendar/main.min.css"/>
    <link rel="stylesheet" href="../plugins/fullcalendar-daygrid/main.min.css"/>
    <link rel="stylesheet" href="../plugins/fullcalendar-timegrid/main.min.css"/>
    <link rel="stylesheet" href="../plugins/fullcalendar-bootstrap/main.min.css"/>

    <script src="https://cdnjs.cloudflare.com/ajax/libs/Chart.js/2.5.0/Chart.min.js"></script>

    <!-- Implementação do Sistema Anti-fraude -->

    <script type="text/javascript" src="https://h.online-metrix.net/fp/tags.js?org_id=1snn5n9w&session_id=adiq_br<%=AntifraudeGUID() %>"></script>    

</head>

<body class="hold-transition sidebar-mini layout-fixed">


    <form id="frmPrincipal" runat="server">
    <noscript><iframe style="width: 100px; height: 100px; border: 0; position:absolute; top: -5000px;" src="https://h.online-metrix.net/fp/tags.js?org_id=1snn5n9w&session_id=adiq_br<%=afGUID() %>"></iframe></noscript>


        <div class="row">
            <div class="col-12">
            <div class="card">

                <div class="card-header bg-whitelabel1">
                <h3 class="card-title">ADIQ</h3>
                </div>
                <div class="card-body">

                <div class="form-group row col-12">
                    Mastercard	5201561050025011	24/09	123
                    Visa	4761739001010036	25/12	123
                </div>

                <div class="form-group row col-12">
                    <label class="col-sm-2 col-form-label">Cartão</label>
                    <div class="col-sm-4">
                        <asp:TextBox runat="server" ID="txtCartao" CssClass="form-control"></asp:TextBox>
                    </div>
                    <label class="col-sm-2 col-form-label">Bandeira</label>
                    <div class="col-sm-4">
                        <asp:TextBox runat="server" ID="txtBandeira" CssClass="form-control"></asp:TextBox>
                    </div>
                </div>


                <div class="form-group row col-12">
                    <label class="col-sm-2 col-form-label">Validade</label>
                    <div class="col-sm-2">
                        <asp:TextBox runat="server" ID="txtValidade" CssClass="form-control" placeholder="MM/YY"></asp:TextBox>
                    </div>
                    <label class="col-sm-2 col-form-label">CVV</label>
                    <div class="col-sm-2">
                        <asp:TextBox runat="server" ID="txtCVV" CssClass="form-control"></asp:TextBox>
                    </div>
                    <label class="col-sm-2 col-form-label">Cofre</label>
                    <div class="col-sm-2">
                        <asp:DropDownList runat="server" ID="ddlCofre" CssClass="form-control">
                            <asp:ListItem Text="Não" Value="N"></asp:ListItem>
                            <asp:ListItem Text="Sim" Value="S"></asp:ListItem>
                        </asp:DropDownList>
                    </div>

                </div>

                <div class="form-group row col-12">
                    <label class="col-sm-2 col-form-label">Tipo</label>
                    <div class="col-sm-3">
                        <asp:DropDownList runat="server" ID="ddlTipo" CssClass="form-control">
                            <asp:ListItem Text="À Vista" Value="avista"></asp:ListItem>
                            <asp:ListItem Text="Lojista" Value="lojista"></asp:ListItem>
                        </asp:DropDownList>
                    </div>
                    <label class="col-sm-2 col-form-label">Captura</label>
                    <div class="col-sm-3">
                        <asp:DropDownList runat="server" ID="ddlCaptura" CssClass="form-control">
                            <asp:ListItem Text="Pré-Autoriza" Value="pa"></asp:ListItem>
                            <asp:ListItem Text="Autoriza e Captura" Value="ac"></asp:ListItem>
                        </asp:DropDownList>
                    </div>
                    <label class="col-sm-1 col-form-label">PC</label>
                    <div class="col-sm-1">
                        <asp:TextBox runat="server" ID="txtParcela" CssClass="form-control" Text="1"></asp:TextBox>
                    </div>
                </div>


                <div class="form-group row col-12">
                    <label class="col-sm-2 col-form-label">Token</label>
                    <div class="col-sm-4">
                        <asp:TextBox runat="server" ID="txtTokenCartao" CssClass="form-control"></asp:TextBox>
                    </div>
                    <label class="col-sm-2 col-form-label">Cofre</label>
                    <div class="col-sm-4">
                        <asp:TextBox runat="server" ID="txtCofreCartao" CssClass="form-control"></asp:TextBox>
                    </div>
                </div>

                <div class="form-group row col-12">
                    <label class="col-sm-2 col-form-label">No. Ordem</label>
                    <div class="col-sm-4">
                        <asp:TextBox runat="server" ID="txtNumeroOrdem" CssClass="form-control"></asp:TextBox>
                    </div>
                    <label class="col-sm-2 col-form-label">Operação</label>
                    <div class="col-sm-4">
                        <asp:DropDownList runat="server" ID="ddlOperacao" CssClass="form-control">
                            <asp:ListItem Text="Autorizar" Value="A"></asp:ListItem>
                            <asp:ListItem Text="Cancelar" Value="C"></asp:ListItem>
                            <asp:ListItem Text="Capturar" Value="P"></asp:ListItem>
                            <asp:ListItem Text="Consultar" Value="T"></asp:ListItem>
                            <asp:ListItem Text="Consultar/Cancelar" Value="L"></asp:ListItem>
                            <asp:ListItem Text="Todos" Value="X"></asp:ListItem>
                        </asp:DropDownList>

                    </div>
                </div>


                <div class="form-group row col-12">
                    <label class="col-sm-2 col-form-label">ID Pagamento</label>
                    <div class="col-sm-4">
                        <asp:TextBox runat="server" ID="txtIDPagamento" CssClass="form-control"></asp:TextBox>
                    </div>
                    <label class="col-sm-2 col-form-label">Autorização</label>
                    <div class="col-sm-4">
                        <asp:TextBox runat="server" ID="txtAutorizacao" CssClass="form-control"></asp:TextBox>
                    </div>
                    <label class="col-sm-2 col-form-label">GUID</label>
                    <div class="col-sm-10">
                        <asp:TextBox runat="server" ID="txtGUID" CssClass="form-control"></asp:TextBox>
                    </div>
                </div>


                <div class="form-group row col-12">
                    <label class="col-sm-2 col-form-label">JSON</label>
                    <div class="col-sm-10">
                        <asp:TextBox runat="server" ID="txtJsonAdiq" TextMode="MultiLine" Rows="5" CssClass="form-control"></asp:TextBox>
                    </div>
                </div>


                <div class="form-group row col-12">
                    <label class="col-sm-2 col-form-label">JSON ENVIO</label>
                    <div class="col-sm-10">
                        <asp:TextBox runat="server" ID="txtJsonEnvio" TextMode="MultiLine" Rows="5" CssClass="form-control"></asp:TextBox>
                    </div>
                </div>


                </div>
                <div class="card-footer">
                <asp:Button runat="server" ID="btnAdiq" CssClass="btn btn-whitelabel1" Text="Pesquisar" onclick="btnAdiq_Click" />

                </div>
            </div>
            </div>
        </div>


    </form>

<script type="text/javascript" src="../plugins/datatables/jquery.dataTables.js"></script>
<script type="text/javascript" src="../plugins/datatables-bs4/js/dataTables.bootstrap4.js"></script>

<script src="../plugins/inputmask/jquery.inputmask.bundle.js"></script>
<script src="../plugins/moment/moment.min.js"></script>

</body>
</html>
