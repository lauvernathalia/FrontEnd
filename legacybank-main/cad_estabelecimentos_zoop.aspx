<%@ Page Language="C#" AutoEventWireup="true" CodeFile="cad_estabelecimentos_zoop.aspx.cs" Inherits="cad_estabelecimentos_zoop" %>

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
                        <h3 class="card-title">ESTABELECIMENTO ZOOP</h3>
                    </div>
                    <div class="card-body">
                        <h3><span class="float-center badge bg-whitelabel1">1. Dados do Zoop</span></h3>

                        <div class="row">
                            <div class="col-sm-2">
                                <div class="form-group">
                                    <label>ID</label>
                                    <asp:TextBox runat="server" id="txtID" cssClass="form-control" Enabled="false"></asp:TextBox>
                                </div>
                            </div>
                            <div class="col-sm-10">
                                <div class="form-group">
                                    <label>Estabelecimento</label>
                                    <asp:TextBox runat="server" id="txtRazaosocial" cssClass="form-control" Enabled="false"></asp:TextBox>
                                </div>
                            </div>
                        </div>

                        <div class="row">
                            <div class="col-sm-5"  >
                                <div class="form-group">
                                <label>Zoop<strong class="text-danger">*</strong> (Habilitar e gerenciar adquirente Zoop)</label>
                                <asp:DropDownList runat="server" id="ddlZoop" CssClass="form-control">
                                    <asp:ListItem Value="0">Não</asp:ListItem>
                                    <asp:ListItem Value="1">Sim</asp:ListItem>
                                </asp:DropDownList>
                                </div>
                            </div>
                            <div class="col-sm-1"  >
                                <div class="form-group">
                                <label>Status<strong class="text-danger">*</strong></label>
                                <asp:TextBox runat="server" ID="txtStatus" CssClass="form-control" placeholder="Status" ></asp:TextBox>
                                </div>
                            </div>

                            <div class="col-sm-3"  >
                                <div class="form-group">
                                <label>Token<strong class="text-danger">*</strong></label>
                                <asp:TextBox runat="server" ID="txtToken" CssClass="form-control" placeholder="Token Zoop" ></asp:TextBox>
                                </div>
                            </div>

                            <div class="col-sm-3"  >
                                <div class="form-group">
                                <label>Plano<strong class="text-danger">*</strong></label>
                                <asp:DropDownList runat="server" id="ddlPlano" CssClass="form-control">
                                </asp:DropDownList>
                                </div>
                            </div>

                            <div class="col-sm-3"  >
                                <div class="form-group">
                                <label>Saque Automático?<strong class="text-danger">*</strong></label>
                                <asp:DropDownList runat="server" id="ddlSaque" CssClass="form-control">
                                    <asp:ListItem Value="S" Text="Sim"></asp:ListItem>
                                    <asp:ListItem Value="N" Text="Não"></asp:ListItem>
                                </asp:DropDownList>
                                </div>
                            </div>
                            <div class="col-sm-3"  >
                                <div class="form-group">
                                <label>Periodicidade de saque?<strong class="text-danger">*</strong></label>
                                <asp:DropDownList runat="server" id="ddlPeriodicidade" CssClass="form-control">
                                    <asp:ListItem Value="D" Text="Diário"></asp:ListItem>
                                    <asp:ListItem Value="S" Text="Semanal"></asp:ListItem>
                                    <asp:ListItem Value="M" Text="Mensal"></asp:ListItem>
                                </asp:DropDownList>
                                </div>
                            </div>

                            <div class="col-sm-3"  >
                                <div class="form-group">
                                <label>Valor mínimo saque<strong class="text-danger">*</strong></label>
                                <asp:TextBox runat="server" ID="txtValorMinimo" CssClass="form-control" placeholder="100" ></asp:TextBox>
                                </div>
                            </div>


                            <div class="col-sm-4" style="display:none;">
                                <asp:Button runat="server" ID="btnEnviar" CssClass="btn btn-block btn-warning" Text="Enviar dados para Zoop" onclick="btnEnviar_Click"/>
                            </div>
                            <div class="col-sm-4" style="display:none;">
                                <asp:Button runat="server" ID="btnPlano" CssClass="btn btn-block btn-warning" Text="Enviar plano para Zoop" onclick="btnPlano_Click"/>
                            </div>
                            <div class="col-sm-4" style="display:none;">
                                <asp:Button runat="server" ID="btnContas" CssClass="btn btn-block btn-warning" Text="Enviar contas para Zoop" onclick="btnContas_Click"/>
                            </div>
                            <div class="col-sm-12">
                                <br />
                            </div>
                            <div class="col-sm-12" style="display:none;">
                                <div class="form-group">
                                <label>Resposta<strong class="text-danger">*</strong></label>
                                <asp:TextBox runat="server" ID="txtResposta" CssClass="form-control"></asp:TextBox>
                                </div>
                            </div>


                        </div>

                        <h3><span class="float-center badge bg-whitelabel1">2. Dados do Forma de Recebimento</span></h3>


                        <div class="row" runat="server" id="dvFormaRecebimento">
                            <div class="col-sm-12">
                                <div class="form-group">
                                <label>Forma de Recebimento<strong class="text-danger">*</strong></label>
                                <asp:DropDownList runat="server" id="ddlFormaRecebimento" CssClass="form-control">
                                    <asp:ListItem Value="O">Outra conta bancária</asp:ListItem>
                                    <asp:ListItem Value="P">Legacy</asp:ListItem>
                                </asp:DropDownList>
                                </div>
                            </div>
                        </div>

                        <div class="row" runat="server" id="dvDadosBancarios">
                            <div class="col-sm-4">
                                <div class="form-group">
                                <label>Conta Padrão?<strong class="text-danger">*</strong></label>
                                <asp:DropDownList runat="server" id="ddlPadrao" CssClass="form-control">
                                    <asp:ListItem Value="N">Não</asp:ListItem>
                                    <asp:ListItem Value="S">Sim</asp:ListItem>
                                </asp:DropDownList>
                                </div>
                            </div>

                            <div class="col-sm-4">
                                <div class="form-group">
                                <label>Instituição Financeira<strong class="text-danger">*</strong></label>
                                <asp:DropDownList runat="server" id="ddlInstituicaoFinanceira" CssClass="form-control">
                                </asp:DropDownList>
                                </div>
                            </div>

                            <div class="col-sm-4">
                                <div class="form-group">
                                <label>Tipo de Conta<strong class="text-danger">*</strong></label>
                                <asp:DropDownList runat="server" id="ddlTipoConta" CssClass="form-control">
                                    <asp:ListItem Value="C">Conta Corrente</asp:ListItem>
                                    <asp:ListItem Value="P">Conta Poupança</asp:ListItem>
                                </asp:DropDownList>
                                </div>
                            </div>

                            <div class="col-sm-3">
                                <div class="form-group">
                                <label>Agência<strong class="text-danger">*</strong></label>
                                <asp:TextBox runat="server" ID="txtAgencia" CssClass="form-control" placeholder="Número da Agencia" ></asp:TextBox>
                                </div>
                            </div>

                            <div class="col-sm-3">
                                <div class="form-group">
                                <label>Dígito<strong class="text-danger">*</strong></label>
                                <asp:TextBox runat="server" ID="txtDigitoAgencia" CssClass="form-control" placeholder="Dígito" ></asp:TextBox>
                                </div>
                            </div>
                            <div class="col-sm-3">
                                <div class="form-group">
                                <label>Conta<strong class="text-danger">*</strong></label>
                                <asp:TextBox runat="server" ID="txtConta" CssClass="form-control" placeholder="Número da Conta" ></asp:TextBox>
                                </div>
                            </div>
                            <div class="col-sm-3">
                                <div class="form-group">
                                <label>Dígito<strong class="text-danger">*</strong></label>
                                <asp:TextBox runat="server" ID="txtDigitoConta" CssClass="form-control" placeholder="Dígito" ></asp:TextBox>
                                </div>
                            </div>
                            <asp:TextBox runat="server" ID="txtTokenConta" CssClass="form-control" Visible="false"></asp:TextBox>


                            <div class="col-sm-6">
                                <asp:Button runat="server" ID="btnIncluir" CssClass="btn btn-block btn-primary" Text="Incluir Conta" onclick="btnIncluir_Click"/>
                            </div>
                            <div class="col-sm-6">
                                <asp:Button runat="server" ID="btnImportarContas" CssClass="btn btn-block btn-primary" Text="Importar Contas" onclick="btnImportarContas_Click"/>
                            </div>

                        </div>
                    </div>

                </div>            
            
            </div>
        </div>


        <div class="row">
            <div class="col-12">

                <div class="card">
                    <div class="card-header bg-whitelabel1">
                        <h3 class="card-title">LISTA DE CONTAS</h3>
                    </div>
                    <div class="card-body">
                        <table id="tbUsuario" class="table table-bordered table-hover">
                            <thead>
                            <tr>
                                <th>ID</th>
                                <th>Token</th>
                                <th>Banco</th>
                                <th>Tipo</th>
                                <th>Agência</th>
                                <th>Conta</th>
                                <th>Padrão</th>
                                <th>Conta Padrão</th>
                                <th>Excluir</th>
                            </tr>
                            </thead>
                            <tbody>
                            <asp:Repeater runat="server" ID="rptConsultaContas" 
                                    onitemcommand="rptConsultaContas_ItemCommand">
                                <ItemTemplate>
                                    <tr>
                                        <td>
                                            <small><strong><%# DataBinder.Eval(Container.DataItem, "COD_ID")%></strong></small>
                                            <asp:TextBox runat="server" ID="txtid" Text='<%# DataBinder.Eval(Container.DataItem, "COD_ID")%>' Visible="false"></asp:TextBox>
                                            <asp:TextBox runat="server" ID="txtTokenConta" Text='<%# DataBinder.Eval(Container.DataItem, "NOM_TOKEN")%>' Visible="false"></asp:TextBox>
                                            <asp:TextBox runat="server" ID="txtBanco" Text='<%# DataBinder.Eval(Container.DataItem, "NOM_CODIGO_BANCO")%>' Visible="false"></asp:TextBox>
                                            <asp:TextBox runat="server" ID="txtTipo" Text='<%# DataBinder.Eval(Container.DataItem, "NOM_TIPO_BANCO")%>' Visible="false"></asp:TextBox>
                                            <asp:TextBox runat="server" ID="txtAgencia" Text='<%# DataBinder.Eval(Container.DataItem, "NOM_NUMERO_AGENCIA_BANCO")%>' Visible="false"></asp:TextBox>
                                            <asp:TextBox runat="server" ID="txtConta" Text='<%# DataBinder.Eval(Container.DataItem, "NOM_NUMERO_CONTA_BANCO")%>' Visible="false"></asp:TextBox>
                                            <asp:TextBox runat="server" ID="txtContaDigito" Text='<%# DataBinder.Eval(Container.DataItem, "NOM_NUMERO_DIGITO_CONTA_BANCO")%>' Visible="false"></asp:TextBox>
                                        </td>
                                        <td>
                                            <small><strong><%# DataBinder.Eval(Container.DataItem, "NOM_TOKEN")%></strong></small>
                                        </td>

                                        <td>
                                            <small><strong><%# DataBinder.Eval(Container.DataItem, "NOM_CODIGO_BANCO")%></strong></small>
                                        </td>
                                        <td>
                                            <small><%# DataBinder.Eval(Container.DataItem, "NOM_TIPO_BANCO")%></small>
                                        </td>
                                        <td>
                                            <small><%# DataBinder.Eval(Container.DataItem, "NOM_NUMERO_AGENCIA_BANCO")%>-<%# DataBinder.Eval(Container.DataItem, "NOM_NUMERO_DIGITO_AGENCIA_BANCO")%></small>
                                        </td>
                                        <td>
                                            <small><%# DataBinder.Eval(Container.DataItem, "NOM_NUMERO_CONTA_BANCO")%>-<%# DataBinder.Eval(Container.DataItem, "NOM_NUMERO_DIGITO_CONTA_BANCO")%></small>
                                        </td>
                                        <td>
                                            <small><%# DataBinder.Eval(Container.DataItem, "FLG_PADRAO")%></small>
                                        </td>

                                        <td>
                                            <small><asp:linkbutton ID="lkbPadrao" CssClass="btn btn-sm btn-primary" commandname="Padrao" runat="server" text="Padrão"  ToolTip="Padrão" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "COD_ID")%>'><i class="fas fa-check"></i></asp:linkbutton></small>
                                        </td>


                                        <td>
                                            <asp:linkbutton ID="lkbExcluir" CssClass="btn btn-sm btn-danger" commandname="Excluir" runat="server" text="Excluir"  ToolTip="Excluir" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "COD_ID")%>'><i class="fas fa-trash"></i></asp:linkbutton>
                                        </td>

                                    </tr>
                                </ItemTemplate>
                            </asp:Repeater>

                            </tbody>
                        </table>


                    </div>

                    <div class="card-footer">
                        <asp:Button runat="server" ID="btnEnviarTodos" CssClass="btn btn-warning" Text="Salvar e Enviar dados para Zoop" onclick="btnEnviarTodos_Click" Visible="false"/>
                        <asp:Button runat="server" ID="btnSalvar" CssClass="btn btn-whitelabel1" Text="Salvar e Enviar dados para Zoop" onclick="btnSalvar_Click"/>
                        <asp:Button runat="server" ID="btnCancelar" CssClass="btn btn-danger float-right" Text="Finalizar" onclick="btnCancelar_Click"/>

                    </div>






                </div>            
            
            </div>
        </div>




    </form>

<script type="text/javascript" src="../plugins/datatables/jquery.dataTables.js"></script>
<script type="text/javascript" src="../plugins/datatables-bs4/js/dataTables.bootstrap4.js"></script>

<script src="../plugins/inputmask/jquery.inputmask.bundle.js"></script>
<script src="../plugins/moment/moment.min.js"></script>

<script>
    $(function () {

        //Datemask dd/mm/yyyy

        $('#txtNascimento').inputmask('99/99/9999')
        $('#txtDataAbertura').inputmask('99/99/9999')
        $('#txtCPF').inputmask('999.999.999-99')
        $('#txtCNPJ').inputmask('99.999.999/9999-99')
        $('#txtCelular').inputmask('(99) 99999-9999')
        $('#txtTelefoneEmpresa').inputmask('(99) 99999-9999')
        $('#txtCEP').inputmask('99999-999')

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
