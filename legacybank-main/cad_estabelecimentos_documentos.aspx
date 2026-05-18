<%@ Page Language="C#" AutoEventWireup="true" CodeFile="cad_estabelecimentos_documentos.aspx.cs" Inherits="cad_estabelecimentos_documentos" %>

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
                        <h3 class="card-title">DOCUMENTOS</h3>
                    </div>
                    <div class="card-body">
                        <h3><span class="float-center badge bg-whitelabel1">Dados do Estabelecimento</span></h3>

                        <div class="row">
                            <div class="col-sm-2">
                                <div class="form-group">
                                    <label>ID</label>
                                    <asp:TextBox runat="server" id="txtID" cssClass="form-control" Enabled="false"></asp:TextBox>
                                </div>
                            </div>
                            <div class="col-sm-3">
                                <div class="form-group">
                                    <label>Estabelecimento</label>
                                    <asp:TextBox runat="server" id="txtRazaosocial" cssClass="form-control" Enabled="false"></asp:TextBox>
                                </div>
                            </div>
                            <div class="col-sm-3">
                                <div class="form-group">
                                    <label>E-mail</label>
                                    <asp:TextBox runat="server" id="txtEmail" cssClass="form-control" Enabled="false"></asp:TextBox>
                                </div>
                            </div>
                            <div class="col-sm-2">
                                <div class="form-group">
                                    <label>Tipo</label>
                                    <asp:TextBox runat="server" id="txtTipo" cssClass="form-control" Enabled="false"></asp:TextBox>
                                </div>
                            </div>
                            <div class="col-sm-2">
                                <div class="form-group">
                                    <label>Documento</label>
                                    <asp:TextBox runat="server" id="txtDocumento" cssClass="form-control" Enabled="false"></asp:TextBox>
                                </div>
                            </div>
                        </div>
                        
                        <div class="row">
                            <div class="col-sm-4">
                                <div class="form-group">
                                    <label>Representante</label>
                                    <asp:TextBox runat="server" id="txtRepresentante" cssClass="form-control" Enabled="false"></asp:TextBox>
                                </div>
                            </div>
                            <div class="col-sm-4">
                                <div class="form-group">
                                    <label>Documento Representante</label>
                                    <asp:TextBox runat="server" id="txtDocumentoRepresentante" cssClass="form-control" Enabled="false"></asp:TextBox>
                                </div>
                            </div>
                            <div class="col-sm-4">
                                <div class="form-group">
                                    <label>Tipo Representante</label>
                                    <asp:TextBox runat="server" id="txtTipoRepresentante" cssClass="form-control" Enabled="false"></asp:TextBox>
                                </div>
                            </div>

                        </div>

                        <div class="form-group">
                            <asp:TextBox runat="server" ID="txtResposta" Text="" Visible="false" CssClass="form-control"></asp:TextBox>
                        </div>

                        <!-- Dados das Contas Bancárias -->

                        <h3><span class="float-center badge bg-whitelabel1">Dados Documento</span></h3>

                        <div class="row" runat="server" id="dvDadosDocumentos">
                            <div class="col-sm-2">
                                <div class="form-group">
                                <label>Tipo Documento<strong class="text-danger">*</strong></label>
                                <asp:DropDownList runat="server" id="ddlTipo" CssClass="form-control">
                                </asp:DropDownList>
                                </div>
                            </div>

                            <div class="col-sm-10">
                                <div class="form-group">
                                    <label>Arquivo (bmp, jpg, jpeg, png, pdf)<strong class="text-danger">*</strong></label>
                                    <asp:TextBox runat="server" ID="txtArquivo" CssClass="form-control"></asp:TextBox> 
                                      <div class="form-group">
                                        <div class="input-group">
                                          <div class="custom-file">
                                            <input type="file" name="attachment" runat="server" id="flArquivo" accept="image/*,application/pdf"/>
                                          </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="col-sm-12">
                                <asp:Button runat="server" ID="btnIncluir" CssClass="btn btn-whitelabel1" Text="Incluir Documento" onclick="btnIncluir_Click"/>
                            </div>

                        </div>


                        <div class="row mt-3">
                            <div class="col-12">

                                <div class="card">
                                    <div class="card-header bg-whitelabel1">
                                        <h3 class="card-title">LISTA DE DOCUMENTOS</h3>
                                    </div>
                                    <div class="card-body" style="overflow:auto; width: 100%;">
                                        <table id="tblConsulta" class="table table-bordered table-hover">
                                            <thead>
                                            <tr>
                                                <th>ID</th>
                                                <th>Data</th>
                                                <th>Tipo</th>
                                                <th>Arquivo</th>
                                                <th>Excluir</th>
                                            </tr>
                                            </thead>
                                            <tbody>
                                            <asp:Repeater runat="server" ID="rptConsulta" 
                                                    onitemcommand="rptConsulta_ItemCommand">
                                                <ItemTemplate>
                                                    <tr>
                                                        <td>
                                                            <small><strong><%# DataBinder.Eval(Container.DataItem, "COD_ID")%></strong></small>
                                                            <asp:TextBox runat="server" ID="txtid" Text='<%# DataBinder.Eval(Container.DataItem, "COD_ID")%>' Visible="false"></asp:TextBox>
                                                        </td>
                                                        <td>
                                                            <small><strong><%# String.Format("{0:dd/MM/yyyy}",DataBinder.Eval(Container.DataItem, "DTA_DATA"))%></strong></small>
                                                        </td>
                                                        <td>
                                                            <small><strong><%# DataBinder.Eval(Container.DataItem, "NOM_TIPO_DOCUMENTO")%></strong></small>
                                                        </td>
                                                        <td style="max-width:300px;">
                                                            <small><%# Funcoes.Encrypt(DataBinder.Eval(Container.DataItem, "NOM_ARQUIVO").ToString())%></small>
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
                                </div>            
                            </div>
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
