<%@ Page Language="C#" AutoEventWireup="true" CodeFile="cad_integracoes.aspx.cs" Inherits="cad_integracoes" %>

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
                        <h3 class="card-title">INTEGRAÇÕES</h3>
                    </div>
                    <div class="card-body">
                        <h3><span class="float-center badge bg-whitelabel1">Dados da Integração</span></h3>

                        <div class="row">
                            <div class="col-sm-2">
                                <div class="form-group">
                                    <label>Código</label>
                                    <asp:TextBox runat="server" id="txtCodigo" cssClass="form-control" Enabled="false"></asp:TextBox>
                                </div>
                            </div>
                        </div>



                        <h3><span class="float-center badge bg-whitelabel1">1. Integração</span></h3>

                        <div class="row">
                            <div class="col-sm-8">
                                <div class="form-group">
                                    <label class="col-sm-12 col-form-label">Integracao</label>
                                    <asp:TextBox id="txtIntegracao" runat="server" CssClass="form-control" ></asp:TextBox>
                                </div>
                            </div>
                            <div class="col-sm-4">
                                <div class="form-group">
                                    <label class="col-sm-12 col-form-label">Ativo</label>
                                    <asp:DropDownList runat="server" ID="ddlAtivo" CssClass="form-control">
                                        <asp:ListItem Value="S" Text="Sim"></asp:ListItem>
                                        <asp:ListItem Value="N" Text="Não"></asp:ListItem>
                                    </asp:DropDownList>                                
                                </div>
                            </div>
                            <div class="col-sm-12">
                                <div class="form-group">
                                    <label class="col-sm-12 col-form-label">Descrição</label>
                                    <textarea runat="server" id="txtDescricao" class="textarea" rows="5"></textarea>
                                </div>
                            </div>
                        </div>

                        <div class="row">
                            <div class="col-sm-12">
                                <div class="form-group">
                                    <label class="col-sm-12 col-form-label">Logo</label>
                                    <asp:TextBox id="txtLogo" runat="server" CssClass="form-control" ></asp:TextBox>
                                      <div class="form-group">
                                        <div class="input-group">
                                          <div class="custom-file">
                                            <input type="file" name="attachment" runat="server" id="flLogo"/>
                                          </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="col-sm-12">
                                <div class="form-group">
                                    <label class="col-sm-12 col-form-label">URL</label>
                                    <asp:TextBox id="txtURL"  runat="server" CssClass="form-control" ></asp:TextBox>
                                </div>
                            </div>
                        </div>

                        <div class="row">
                            <div class="col-sm-4">
                                <div class="form-group">
                                    <label class="col-sm-12 col-form-label">Nome Exibição</label>
                                    <asp:TextBox id="txtFantasia"  runat="server" CssClass="form-control" ></asp:TextBox>
                                </div>
                            </div>
                            <div class="col-sm-4">
                                <div class="form-group">
                                    <label class="col-sm-12 col-form-label">Sigla (Utilizar apenas um caracter)</label>
                                    <asp:TextBox id="txtSigla"  runat="server" CssClass="form-control" ></asp:TextBox>
                                </div>
                            </div>
                            <div class="col-sm-4">
                                <div class="form-group">
                                    <label class="col-sm-12 col-form-label">Tipo</label>
                                    <asp:DropDownList runat="server" ID="ddlTipo" CssClass="form-control">
                                        <asp:ListItem Value="Adquirente" Text="Adquirente"></asp:ListItem>
                                        <asp:ListItem Value="BaaS" Text="BaaS"></asp:ListItem>
                                        <asp:ListItem Value="Serviços" Text="Serviços"></asp:ListItem>
                                    </asp:DropDownList>                                
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
    </form>

<script type="text/javascript" src="../plugins/datatables/jquery.dataTables.js"></script>
<script type="text/javascript" src="../plugins/datatables-bs4/js/dataTables.bootstrap4.js"></script>

<script src="../plugins/inputmask/jquery.inputmask.bundle.js"></script>
<script src="../plugins/moment/moment.min.js"></script>


<script type="text/javascript" src="../plugins/summernote/summernote-bs4.min.js"></script>
<script type="text/javascript">
    $(function () {
        // Summernote
        $('.textarea').summernote()
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

