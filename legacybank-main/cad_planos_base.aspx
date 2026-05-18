<%@ Page Language="C#" AutoEventWireup="true" CodeFile="cad_planos_base.aspx.cs" Inherits="cad_planos_base" %>

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
                <h3 class="card-title">PLANOS BASE</h3>
                </div>
                <div class="card-body">

                    <div class="row">
                        <div class="col-sm-4">
                            <div class="form-group">
                                <label class="col-sm-12 col-form-label">ID</label>
                                <asp:Label id="lblID" runat="server" class="form-control"></asp:Label>
                            </div>
                        </div>
                        <div class="col-sm-8">
                            <div class="form-group">
                                <label class="col-sm-12 col-form-label">Adquirente</label>
                                <asp:DropDownList runat="server" id="ddlAdquirentes" CssClass="form-control" 
                                    AutoPostBack="True" onselectedindexchanged="ddlAdquirentes_SelectedIndexChanged">
                                </asp:DropDownList>
                            </div>
                        </div>
                    </div>

                    <div class="row">
                        <div class="col-sm-4">
                            <div class="form-group">
                                <label class="col-sm-12 col-form-label">Código Referência</label>
                                <asp:DropDownList runat="server" ID="ddlPlanosReferencia" CssClass="form-control"></asp:DropDownList>
                                <asp:TextBox id="txtCodigo" runat="server" class="form-control" placeholder="Código de Referência" Visible="false"></asp:TextBox>
                            </div>
                        </div>
                        <div class="col-sm-4">
                            <div class="form-group">
                                <label class="col-sm-12 col-form-label">Nome</label>
                                <asp:TextBox id="txtNome"  runat="server" class="form-control" placeholder="Nome do Plano de Referência"></asp:TextBox>
                            </div>
                        </div>
                        <div class="col-sm-4">
                            <div class="form-group">
                                <label class="col-sm-12 col-form-label">Descrição</label>
                                <asp:TextBox id="txtDescricao"  runat="server" class="form-control" placeholder="Descrição do Plano de Referência"></asp:TextBox>
                            </div>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-sm-4">
                            <div class="form-group">
                                <label class="col-sm-12 col-form-label">Nome Transação</label>
                                <asp:TextBox id="txtTransacao" runat="server" class="form-control" placeholder="Nome na Transação"></asp:TextBox>
                            </div>
                        </div>
                        <div class="col-sm-4">
                            <div class="form-group">
                                <label class="col-sm-12 col-form-label">Periodicidade </label>
                                <asp:TextBox id="txtPeriodicidade" runat="server" class="form-control" placeholder="Periodicidade"></asp:TextBox>
                            </div>
                        </div>
                        <div class="col-sm-4">
                            <label class="col-sm-12 col-form-label">Ativo</label>
                            <asp:DropDownList runat="server" id="ddlAtivo" CssClass="form-control">
                                <asp:ListItem Value="S">Sim</asp:ListItem>
                                <asp:ListItem Value="N">Não</asp:ListItem>
                            </asp:DropDownList>
                        </div>

                    </div>

                </div>
                <div class="card-footer">
                        <asp:Button runat="server" ID="btnSalvar" CssClass="btn btn-whitelabel1" Text="Salvar" onclick="btnSalvar_Click"/>
                        <asp:Button runat="server" ID="btnCancelar" CssClass="btn btn-danger float-right" Text="Cancelar" onclick="btnCancelar_Click"/>
                        <asp:Button runat="server" ID="btnImportar" CssClass="btn btn-whitelabel1 float-right" Text="Importar Taxas" onclick="btnImportar_Click"/>
                        <asp:Button runat="server" ID="btnZerar" CssClass="btn btn-whitelabel1 float-right" Text="Zerar Taxas" onclick="btnZerar_Click"/>

                </div>
            </div>
            </div>
        </div>

        <div class="row">
            <div class="col-12">
                <div class="card">

                    <div class="card-header bg-whitelabel1">
                    <h3 class="card-title">BANDEIRAS & TAXAS</h3>
                    </div>
                    <div class="card-body">

                                <div class="row">
                                    <div class="col-12">
                                        <table id="tabConsulta" class="table table-bordered table-hover">
                                            <thead>
                                            <tr>
                                              <th>Tipo Pagto</th>
                                              <th>Bandeira</th>
                                              <th>Taxa (R$)</th>
                                              <th>Taxa (%)</th>
                                              <th>Markup (%)</th>
                                              <th>Rebate (%)</th>
                                              <th>Taxa Final (%)</th>
                                              <th>Parcela</th>
                                              <th>Captura</th>

                                            </tr>
                                            </thead>
                                            <tbody>
                                            <asp:Repeater runat="server" ID="rptConsulta">
                                                <ItemTemplate>

                                                    <tr>

                                                      <td>
                                                          <asp:TextBox runat="server" ID="idLicenciado" Text='<%# DataBinder.Eval(Container.DataItem, "COD_ID_PESSOA_LICENCIADO")%>' Visible="false"></asp:TextBox>
                                                          <asp:TextBox runat="server" ID="idPlanoReferencia" Text='<%# DataBinder.Eval(Container.DataItem, "COD_ID_PLANOS_REFERENCIA")%>' Visible="false"></asp:TextBox>
                                                          <asp:TextBox runat="server" ID="idBandeira" Text='<%# DataBinder.Eval(Container.DataItem, "COD_ID_BANDEIRAS")%>' Visible="false"></asp:TextBox>
                                                          <asp:TextBox runat="server" ID="TipoPagamento" Text='<%# DataBinder.Eval(Container.DataItem, "NOM_TIPO_PAGAMENTO")%>' Visible="false"></asp:TextBox>
                                                          <asp:TextBox runat="server" ID="idParcelas" Text='<%# DataBinder.Eval(Container.DataItem, "NUM_PARCELAS")%>' Visible="false"></asp:TextBox>
                                                          <asp:TextBox runat="server" ID="ModoCaptura" Text='<%# DataBinder.Eval(Container.DataItem, "NOM_MODO_CAPTURA")%>' Visible="false"></asp:TextBox>

                                                          <small><%# DataBinder.Eval(Container.DataItem, "NOM_TIPO_PAGAMENTO")%></small>
                                                      </td>

                                                      <td>
                                                          <small><strong><%# DataBinder.Eval(Container.DataItem, "NOM_BANDEIRA")%></strong></small><br />
                                                      </td>
                                                      <td>
                                                          <small><%# String.Format("{0:n2}",DataBinder.Eval(Container.DataItem, "NUM_VALOR"))%></small>
                                                      </td>
                                                      <td>
                                                          <small><%# String.Format("{0:n2}",DataBinder.Eval(Container.DataItem, "NUM_VALOR_PERCENTUAL"))%></small>
                                                      </td>

                                                        <td>
                                                            <small><asp:TextBox runat="server" ID="txtMarkup" Text='<%# String.Format("{0:n2}",DataBinder.Eval(Container.DataItem, "NUM_VALOR_MARKUP"))%>' class="form-control"></asp:TextBox></small>
                                                        </td>
                                                        <td>
                                                            <small><asp:TextBox runat="server" ID="txtRebate" Text='<%# String.Format("{0:n2}",DataBinder.Eval(Container.DataItem, "NUM_VALOR_REBATE"))%>' class="form-control"></asp:TextBox></small>
                                                        </td>
                                                        <td>
                                                            <strong><%# String.Format("{0:n2}",DataBinder.Eval(Container.DataItem, "NUM_TAXA_FINAL"))%></strong>
                                                        </td>
                                                      <td>
                                                          <small><%# String.Format("{0:n0}",DataBinder.Eval(Container.DataItem, "NUM_PARCELAS"))%></small>
                                                      </td>
                                                      <td>
                                                          <small><strong><%# DataBinder.Eval(Container.DataItem, "NOM_MODO_CAPTURA")%></strong></small><br />
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
            </div>
        </div>

    </form>

<script type="text/javascript" src="../plugins/datatables/jquery.dataTables.js"></script>
<script type="text/javascript" src="../plugins/datatables-bs4/js/dataTables.bootstrap4.js"></script>

<script src="../plugins/inputmask/jquery.inputmask.bundle.js"></script>
<script src="../plugins/moment/moment.min.js"></script>

<script type="text/javascript">

$(document).ready(function () {
    var table = $('#tabConsulta').DataTable({
      "paging": false,
      "lengthChange": false,
      "searching": false,
      "ordering": false,
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
