<%@ Page Language="C#" AutoEventWireup="true" CodeFile="cad_pagamentos_cobrança_simulador.aspx.cs" Inherits="cad_pagamentos_cobrança_simulador" %>

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
                        <h3 class="card-title">SIMULADOR COBRANÇA</h3>
                    </div>
                    <div class="card-body">
                        <h3><span class="float-center badge bg-whitelabel1">1. Dados da Simulação</span></h3>
                        <p>Faça uma simulação para saber como ficarão as taxas de uma cobrança</p>
                        <div class="row">
                            <div class="col-sm-2">
                                <div class="form-group">
                                    <label>ID</label>
                                    <asp:TextBox runat="server" id="txtID" cssClass="form-control" Enabled="false"></asp:TextBox>
                                </div>
                            </div>
                            <div class="col-sm-10">
                                <div class="form-group">
                                    <label>Nome da Simulação de Cobrança</label>
                                    <asp:TextBox runat="server" id="txtSimulacao" cssClass="form-control"></asp:TextBox>
                                </div>
                            </div>
                        </div>

                        <div class="row">
                            <div class="col-sm-6"  >
                                <div class="form-group">
                                <label>Plano de Cobrança</label>
                                <asp:DropDownList runat="server" ID="ddlPlano" CssClass="form-control">
                                </asp:DropDownList>
                                </div>
                            </div>
                        </div>


                        <div class="row">
                            <div class="col-sm-5"  >
                                <div class="form-group">
                                <label>Qual a Modalidade do plano?</label>
                                <asp:DropDownList runat="server" ID="ddlModalidade" CssClass="form-control">
                                    <asp:ListItem Text="Cobrança Presencial" Value="P"></asp:ListItem>
                                    <asp:ListItem Text="Cobrança On-line" Value="O"></asp:ListItem>
                                </asp:DropDownList>
                                </div>
                            </div>

                            <div class="col-sm-4"  >
                                <div class="form-group">
                                <label>Qual é a bandeira para cobrança?</label>
                                <asp:DropDownList runat="server" ID="ddlBandeira" CssClass="form-control">
                                </asp:DropDownList>
                                </div>
                            </div>
                                          

                            <div class="col-sm-3"  >
                                <div class="form-group">
                                <label>Qual é o valor da cobrança?</label>
                                <div class="input-group">
                                  <div class="input-group-prepend">
                                    <span class="input-group-text">
                                      <i class="fas fa-dollar-sign"></i>
                                    </span>
                                  </div>
                                  <asp:TextBox runat="server" ID="txtValor" CssClass="form-control"></asp:TextBox>
                                </div>


                                </div>
                            </div>


                        </div>

                        <div class="row">
                            <div class="col-sm-6"  >
                                <div class="form-group">
                                    <div class="form-check">
                                      <asp:CheckBox CssClass="form-check-input" runat="server" id="ckbJurosCliente"/>
                                      <label class="form-check-label">Repassar os juros para o cliente</label>
                                    </div>
                                </div>
                            </div>
                        </div>

                    </div>

                    <div class="card-footer">
                        <asp:Button runat="server" ID="btnIncluir" CssClass="btn btn-whitelabel1" Text="Incluir e Calcular" onclick="btnIncluir_Click"/>
                        <asp:Button runat="server" ID="btnAtualizar" CssClass="btn btn-whitelabel1" Text="Atualizar e Calcular" onclick="btnAtualizar_Click"/>
                        <asp:Button runat="server" ID="btnCancelar" CssClass="btn btn-danger float-right" Text="Cancelar" onclick="btnCancelar_Click"/>
                    </div>
                </div>            
            
            </div>
        </div>



        <div class="row">
            <div class="col-12">

                <div class="card">
                    <div class="card-header bg-whitelabel1">
                        <h3 class="card-title">LISTA DE USUÁRIOS</h3>
                    </div>
                    <div class="card-body">
                        <table id="tbUsuario" class="table table-bordered table-hover">
                            <thead>
                            <tr>
                                <th>Forma de pagamento</th>
                                <th>Taxa</th>
                                <th>Cliente pagará</th>
                                <th>Valor da Parcela</th>
                                <th>Você Receberá</th>
                            </tr>
                            </thead>
                            <tbody>
                            <asp:Repeater runat="server" ID="rptConsulta">
                                <ItemTemplate>
                                    <tr>
                                        <td>
                                            <small><%# DataBinder.Eval(Container.DataItem, "NOM_FORMA_PAGAMENTO")%></small>
                                        </td>
                                        <td>
                                            <small><%# String.Format("{0:n2}", DataBinder.Eval(Container.DataItem, "NUM_JUROS"))%>%</small>
                                        </td>

                                        <td>
                                            <small><%# String.Format("{0:n2}", DataBinder.Eval(Container.DataItem, "VALOR_TOTAL"))%></small>
                                        </td>
                                        <td>
                                            <small><%# String.Format("{0:n2}", DataBinder.Eval(Container.DataItem, "VALOR_PARCELA"))%></small>
                                        </td>
                                        <td>
                                            <small><%# String.Format("{0:n2}", DataBinder.Eval(Container.DataItem, "VALOR_RECEBER"))%></small>
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

    </form>

<script type="text/javascript" src="../plugins/datatables/jquery.dataTables.js"></script>
<script type="text/javascript" src="../plugins/datatables-bs4/js/dataTables.bootstrap4.js"></script>

<script src="../plugins/inputmask/jquery.inputmask.bundle.js"></script>
<script src="../plugins/moment/moment.min.js"></script>

</body>
</html>
