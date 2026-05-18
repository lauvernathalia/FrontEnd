<%@ Page Language="C#" AutoEventWireup="true" CodeFile="cad_representantes_importar.aspx.cs" Inherits="cad_representantes_importar" %>

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
                        <h3 class="card-title">REPRESENTANTES IMPORTAR</h3>
                    </div>
                    <div class="card-body">
                        <h3><span class="float-center badge bg-whitelabel1 mt-2 mb-2">Processo de Importação</span></h3>

                        <div class="form-group row col-12">
                            <div class="col-sm-3">
                                <label class="col-sm-12 col-form-label">Adquirente</label>
                                <asp:DropDownList runat="server" id="ddlAdquirentes" CssClass="form-control" 
                                    AutoPostBack="True" onselectedindexchanged="ddlAdquirentes_SelectedIndexChanged">
                                </asp:DropDownList>
                            </div>
                        </div>


                        <div class="row" runat="server" id="divAdquirenteCappta" visible="false">
                            <div class="col-sm-12">

                                <div class="card">
                                    <div class="card-header bg-whitelabel1">
                                        <h3 class="card-title">IMPORTAÇÃO</h3>
                                    </div>
                                    <div class="card-body">
                                        <div class="form-group">
                                            <asp:TextBox runat="server" ID="TextBox1" Text="" TextMode="MultiLine" Rows="5" CssClass="form-control" Visible="false"></asp:TextBox>
                                        </div>
                                        <div class="row">
                                            <div class="col-12">
                                                <div class="alert alert-warning alert-dismissible">
                                                  <button type="button" class="close" data-dismiss="alert" aria-hidden="true">&times;</button>
                                                  <h5><i class="icon fas fa-exclamation-triangle"></i> ATENÇÃO!</h5>
                                                  Para importar apenas um cadastro específico, digite o documento que deseja importar e clique em <b>"Importar Estabelecimento"</b>. Caso deseje importar vários cadastros simultaneamente,clique em <b>"Carregar Dados"</b>, selecione os documentos a importar e clique em <b>"Importar Dados Selecionados"</b>.
                                                </div>                                            
                                            </div>
                                        </div>

                                        <div class="row">
                                            <div class="col-9">
                                                <div class="form-group">
                                                <label>Documento</label>
                                                <asp:TextBox runat="server" ID="txtDocumentoCappta" Text="" CssClass="form-control" placeholder="Digite o documento que deseja importar (CPF/CNPJ)"></asp:TextBox>
                                                </div>

                                            </div>

                                            <div class="col-3">
                                                <div class="form-group">
                                                <label><br /></label><br />
                                                <asp:Button runat="server" ID="btnImportarDocumentoCappta" CssClass="btn btn-whitelabel1 btn-block" 
                                                        Text="Importar Documento" onclick="btnImportarDocumentoCappta_Click" />
                                                </div>

                                            </div>

                                        </div>


                                        <div class="row">
                                            <div class="col-6">
                                                <div class="form-group">
                                                    <div class="form-check">
                                                      <asp:CheckBox CssClass="form-check-input" runat="server" id="ckbTodosCappta" 
                                                            AutoPostBack="True" oncheckedchanged="ckbTodosCappta_CheckedChanged"/>
                                                      <label class="form-check-label">Clique aqui para marcar ou desmarcar todos os registros</label>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="col-6">
                                                <asp:Button runat="server" ID="btnImportarCappta" CssClass="btn btn-whitelabel1 float-right" 
                                                    Text="Importar Dados Selecionados" onclick="btnImportarCappta_Click"/>

                                                <asp:Button runat="server" ID="btnCarregarCadastrosCappta" CssClass="btn btn-whitelabel1 float-right" 
                                                    Text="Carregar Dados" onclick="btnCarregarCadastrosCappta_Click"/>
                                            </div>

                                        </div>

                                        <table id="tblMarketplacesCappta" class="table table-bordered table-hover">
                                            <thead>
                                            <tr>
                                                <th>Importar</th>
                                                <th>Documento</th>
                                                <th>Nome</th>
                                                <th>Status</th>
                                            </tr>
                                            </thead>
                                            <tbody>
                                                <asp:ListView ID="lsvMarketplacesCappta" runat="server">
                                                    <ItemTemplate>
                                                            <tr>
                                                                <td>
                                                                    <asp:CheckBox runat="server" ID="ckbImportar" />
                                                                </td>
                                                                <td>
                                                                    <small><asp:Label ID="Label6" runat="server" Text='<%# Eval("documento") %>'></asp:Label></small>
                                                                    <asp:TextBox runat="server" ID="txtDocumento" Text='<%# Eval("documento") %>' Visible="false"></asp:TextBox>
                                                                </td>
                                                                <td>
                                                                    <small><asp:Label ID="Label16" runat="server" Text='<%# Eval("nome") %>'></asp:Label></small>
                                                                    <asp:TextBox runat="server" ID="txtNome" Text='<%# Eval("nome") %>' Visible="false"></asp:TextBox>
                                                                </td>
                                                                <td>
                                                                    <small><asp:Label ID="Label1" runat="server" Text='<%# Eval("status") %>'></asp:Label></small>
                                                                    <asp:TextBox runat="server" ID="txtStatus" Text='<%# Eval("status") %>' Visible="false"></asp:TextBox>
                                                                </td>

                                                            </tr>
                                                    </ItemTemplate>
                                                </asp:ListView>
                                            </tbody>
                                        </table>


                                    </div>
                                    <div class="card-footer">
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="row" runat="server" id="divAdquirenteZoop" visible="false">
                        </div>
                        <div class="row" runat="server" id="divAdquirenteAsaas" visible="false">
                        </div>
                        <div class="row" runat="server" id="divAdquirenteErp" visible="false">
                        </div>
                        <div class="row" runat="server" id="divAdquirentePagseguro" visible="false">
                        </div>


                    </div>
                    <div class="card-footer">
                        <asp:Button runat="server" ID="btnFechar" CssClass="btn btn-danger float-right" Text="Fechar" onclick="btnFechar_Click"/>
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
    var table = $('#tblMarketplacesCappta').DataTable({
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
