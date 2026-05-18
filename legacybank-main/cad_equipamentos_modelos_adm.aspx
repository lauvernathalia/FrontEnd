<%@ Page Language="C#" AutoEventWireup="true" CodeFile="cad_equipamentos_modelos_adm.aspx.cs" Inherits="cad_equipamentos_modelos_adm" %>


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
                <h3 class="card-title">Equipamentos - Modelos</h3><br />

                </div>
                <div class="card-body">


                <div class="row">
                    <div class="col-sm-2">
                        <div class="form-group">
                            <label class="col-sm-12 col-form-label">ID</label>
                            <asp:TextBox id="txtID" runat="server" CssClass="form-control" Enabled="false"></asp:TextBox>
                        </div>
                    </div>

                    <div class="col-sm-2">
                        <div class="form-group">
                            <label class="col-sm-12 col-form-label">Código</label>
                            <asp:TextBox id="txtCodigo" runat="server" CssClass="form-control" placeholder="Código do Modelo"></asp:TextBox>
                        </div>
                    </div>

                    <div class="col-sm-3">
                        <div class="form-group">
                            <label class="col-sm-12 col-form-label">Nome Modelo</label>
                            <asp:TextBox id="txtNome" runat="server" CssClass="form-control" placeholder="Nome do Modelo"></asp:TextBox>
                        </div>
                    </div>
                    <div class="col-sm-3">
                        <div class="form-group">
                            <label class="col-sm-12 col-form-label">Descrição Modelo</label>
                            <asp:TextBox id="txtDescricao" runat="server" CssClass="form-control" placeholder="Descrição Modelo"></asp:TextBox>
                        </div>
                    </div>
                    <div class="col-sm-2">
                        <div class="form-group">
                            <label class="col-sm-12 col-form-label">Ativo</label>
                            <asp:DropDownList runat="server" id="ddlAtivo" CssClass="form-control">
                                <asp:ListItem Value="S">Sim</asp:ListItem>
                                <asp:ListItem Value="N">Não</asp:ListItem>
                            </asp:DropDownList>
                        </div>
                    </div>
                </div>
                <div class="row">
                    <div class="col-sm-12">
                        <div class="form-group">
                            <label>Foto<strong class="text-danger">*</strong></label>
                            <asp:TextBox runat="server" ID="txtFoto" CssClass="form-control"></asp:TextBox> 
                                <div class="form-group">
                                <div class="input-group">
                                    <div class="custom-file">
                                    <input type="file" name="attachment" runat="server" id="flFoto"/>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>

                <div class="card-footer">
                        <asp:Button runat="server" ID="btnSalvar" CssClass="btn btn-whitelabel1" Text="Salvar" onclick="btnSalvar_Click"/>
                        <asp:Button runat="server" ID="btnCancelar" CssClass="btn btn-danger float-right" Text="Cancelar" onclick="btnCancelar_Click"/>
                </div>
            </div>
            </div>
        </div>
        </div>

        <!-- *************************************************************************************************************************************************************************** -->
        <!-- SETOR IMPORTAÇÃO                                                                                                                                                            -->
        <!-- *************************************************************************************************************************************************************************** -->       

        <div class="row">

            <div class="col-12">

                <div class="card">
                    <div class="card-header bg-whitelabel1">
                        <h3 class="card-title">MODELOS IMPORTAR</h3>
                    </div>
                    <div class="card-body">
                        <h3><span class="float-center badge bg-whitelabel1 mt-2 mb-2">Processo de Importação</span></h3>

                        <ul class="nav nav-tabs" id="tbimportacao" role="tablist">
                            <li class="nav-item">
                                <a class="nav-link" runat="server" id="tbcapptatab" data-toggle="pill" href="#tbcappta" role="tab" aria-controls="tbcappta" aria-selected="true">Cappta</a>
                            </li>
                            <li class="nav-item">
                                <a class="nav-link" runat="server" id="tbzooptab" data-toggle="pill" href="#tbzoop" role="tab" aria-controls="tbzoop" aria-selected="false">Zoop</a>
                            </li>
                            <li class="nav-item">
                                <a class="nav-link" runat="server" id="tbasaastab" data-toggle="pill" href="#tbasaas" role="tab" aria-controls="tbasaas" aria-selected="false">AsaaS</a>
                            </li>
                            <li class="nav-item">
                                <a class="nav-link" runat="server" id="tbpagsegurotab" data-toggle="pill" href="#tbpagseguro" role="tab" aria-controls="tbpagseguro" aria-selected="false">Pagseguro</a>
                            </li>
                        </ul>
                        <div class="tab-content" id="tbimportacaoContent">
                            <div class="tab-pane fade" runat="server" id="tbcappta" role="tabpanel" aria-labelledby="tbcapptatab">
                                <div class="card">
                                    <div class="card-header bg-whitelabel1">
                                        <h3 class="card-title">REDE CAPPTA</h3>
                                    </div>
                                    <div class="card-body">
                                        <div class="form-group">
                                            <asp:TextBox runat="server" ID="txtJsonCappta" Text="" TextMode="MultiLine" Rows="5" CssClass="form-control" Visible="false"></asp:TextBox>
                                        </div>


                                        <div class="row">
                                            <div class="col-6">
                                                <div class="form-group">
                                                    <div class="form-check">
                                                      <asp:CheckBox CssClass="form-check-input" runat="server" id="ckbTodos" 
                                                            AutoPostBack="True" oncheckedchanged="ckbTodos_CheckedChanged"/>
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

                                        <table id="tblModelos" class="table table-bordered table-hover">
                                            <thead>
                                            <tr>
                                                <th>Importar</th>
                                                <th>Código</th>
                                                <th>Nome</th>
                                            </tr>
                                            </thead>
                                            <tbody>
                                                <asp:ListView ID="lsvModelos" runat="server" 
                                                    OnItemCommand="lsvModelos_ItemCommand">
                                                    <ItemTemplate>
                                                            <tr>
                                                                <td>
                                                                    <asp:CheckBox runat="server" ID="ckbImportar" />
                                                                </td>
                                                                <td>
                                                                    <small><asp:Label ID="Label6" runat="server" Text='<%# Eval("codigo") %>'></asp:Label></small>
                                                                    <asp:TextBox runat="server" ID="txtCodigo" Text='<%# Eval("codigo") %>' Visible="false"></asp:TextBox>
                                                                </td>
                                                                <td>
                                                                    <small><asp:Label ID="Label16" runat="server" Text='<%# Eval("nome") %>'></asp:Label></small>
                                                                    <asp:TextBox runat="server" ID="txtNome" Text='<%# Eval("nome") %>' Visible="false"></asp:TextBox>
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
                            <div class="tab-pane fade" runat="server" id="tbzoop" role="tabpanel" aria-labelledby="tbzooptab">
                                <div class="card">
                                    <div class="card-header bg-whitelabel1">
                                        <h3 class="card-title">REDE ZOOP</h3>
                                    </div>
                                    <div class="card-body">

                                    </div>
                                    <div class="card-footer"  style="display:none;">
                                        <asp:Button runat="server" ID="btnCarregarCadastrosZoop" CssClass="btn btn-whitelabel1" 
                                            Text="Carregar Dados" onclick="btnCarregarCadastrosZoop_Click"/>

                                        <asp:Button runat="server" ID="btnImportarZoop" CssClass="btn btn-whitelabel1" 
                                            Text="Importar Dados Selecionados" onclick="btnImportarZoop_Click"/>
                                    </div>
                                </div>
                            </div>
                            <div class="tab-pane fade" runat="server" id="tbasaas" role="tabpanel" aria-labelledby="tbasaastab">
                                <div class="card">
                                    <div class="card-header bg-whitelabel1">
                                        <h3 class="card-title">REDE ASAAS</h3>
                                    </div>
                                    <div class="card-body">

                                    </div>
                                    <div class="card-footer">
                                        <asp:Button runat="server" ID="btnCarregarCadastrosAsaas" CssClass="btn btn-whitelabel1" 
                                            Text="Carregar Dados" onclick="btnCarregarCadastrosAsaas_Click"/>

                                        <asp:Button runat="server" ID="btnImportarAsaas" CssClass="btn btn-whitelabel1" 
                                            Text="Importar Dados Selecionados" onclick="btnImportarAsaas_Click"/>
                                    </div>
                                </div>
                            </div>
                            <div class="tab-pane fade" runat="server" id="tbpagseguro" role="tabpanel" aria-labelledby="tbpagsegurotab">
                                <div class="card">
                                    <div class="card-header bg-whitelabel1">
                                        <h3 class="card-title">REDE PAGSEGURO</h3>
                                    </div>
                                    <div class="card-body">

                                    </div>
                                    <div class="card-footer">
                                        <asp:Button runat="server" ID="btnCarregarCadastrosPagseguro" CssClass="btn btn-whitelabel1" 
                                            Text="Carregar Dados" onclick="btnCarregarCadastrosPagseguro_Click"/>

                                        <asp:Button runat="server" ID="btnImportarPagseguro" CssClass="btn btn-whitelabel1" 
                                            Text="Importar Dados Selecionados" onclick="btnImportarPagseguro_Click"/>
                                    </div>
                                </div>
                            </div>
                        </div>






                    </div>
                    <div class="card-footer">
                    </div>
                </div>

            </div>


        </div>



        <div class="row">
            <div class="col-12">
                <div class="card">
                    <div class="card-header bg-whitelabel1">
                        <h3 class="card-title">Modelos</h3>
                    </div>
                    <div class="card-body">


                        <div class="row">
                            <div class="col-12">
                                <table id="example2" class="table table-bordered table-hover">
                                    <thead>
                                    <tr>
                                        <th>ID</th>
                                        <th>Código</th>
                                        <th>Nome</th>
                                        <th>Descrição</th>
                                        <th>Ativo</th>
                                        <th>Foto</th>
                                        <th>Ativar/Inativar</th>
                                        <th>Atualizar</th>
                                        <th>Excluir</th>

                                    </tr>
                                    </thead>
                                    <tbody>
                                    <asp:Repeater runat="server" ID="rptConsulta" OnItemCommand="rptConsulta_OnItemCommand">
                                        <ItemTemplate>

                                            <tr>
                                                <td>
                                                    <small><strong><%# DataBinder.Eval(Container.DataItem, "COD_ID")%></strong></small>
                                                    <asp:TextBox runat="server" id="txtID" Text='<%# DataBinder.Eval(Container.DataItem, "COD_ID")%>' Visible="false"></asp:TextBox>
                                                </td>

                                                <td>
                                                    <small>
                                                    <asp:TextBox runat="server" id="txtCodigo" Text='<%# DataBinder.Eval(Container.DataItem, "COD_ID_MODELO")%>' CssClass="form-control"></asp:TextBox>
                                                    </small>
                                                </td>

                                                <td>
                                                    <small>
                                                    <asp:TextBox runat="server" id="txtNomeModelo" Text='<%# DataBinder.Eval(Container.DataItem, "NOM_MODELO")%>' CssClass="form-control"></asp:TextBox>
                                                    </small>
                                                </td>
                                                <td>
                                                    <small>
                                                    <asp:TextBox runat="server" id="txtDescricaoModelo" Text='<%# DataBinder.Eval(Container.DataItem, "DES_MODELO")%>' CssClass="form-control"></asp:TextBox>
                                                    </small>
                                                </td>
                                                <td>
                                                    <small><%# DataBinder.Eval(Container.DataItem, "NOM_FLG_ATIVO")%></small>
                                                </td>


                                                <td>
                                                    <small>
                                                    <div class="form-group">
                                                    <asp:TextBox runat="server" id="txtFoto" Text='<%# DataBinder.Eval(Container.DataItem, "NOM_FOTO")%>' CssClass="form-control"></asp:TextBox>
                                                    <div class="input-group">
                                                        <div class="custom-file">
                                                        <input type="file" name="attachment" runat="server" id="flFoto"/>
                                                        </div>
                                                    </div>
                                                    </div>
                                                    </small>
                                                </td>


                                                <td>
                                                    <asp:linkbutton ID="lkbAtivo" CssClass="btn btn-whitelabel1 btn-sm" commandname="Ativo" runat="server" text="Ativar/Inativar"  CommandArgument='<%# DataBinder.Eval(Container.DataItem, "COD_ID")%>' />
                                                </td>
                                                <td>
                                                    <asp:linkbutton ID="lkbAtualizar" CssClass="btn btn-info btn-sm" commandname="Atualizar" runat="server" text="Atualizar"  CommandArgument='<%# DataBinder.Eval(Container.DataItem, "COD_ID")%>' />
                                                </td>

                                                <td>
                                                    <asp:linkbutton ID="lkbExcluir" CssClass="btn btn-danger btn-sm" commandname="Excluir" runat="server" text="Excluir"  CommandArgument='<%# DataBinder.Eval(Container.DataItem, "COD_ID")%>' />
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
