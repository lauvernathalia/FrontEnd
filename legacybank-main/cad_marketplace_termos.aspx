<%@ Page Language="C#" AutoEventWireup="true" CodeFile="cad_marketplace_termos.aspx.cs" Inherits="cad_marketplace_termos" %>

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

</head>
<body class="hold-transition sidebar-mini layout-fixed">
    <form id="frmPrincipal" runat="server">

        <div class="row">
            <div class="col-12">

                <div class="card">
                    <div class="card-header bg-whitelabel1">
                        <h3 class="card-title">MARKETPLACE TERMOS</h3>
                    </div>
                    <div class="card-body">
                        <h3><span class="float-center badge bg-whitelabel1">1. Dados do Marketplace</span></h3>

                        <div class="row">
                            <div class="col-sm-2">
                                <div class="form-group">
                                    <label>ID</label>
                                    <asp:TextBox runat="server" id="txtID" cssClass="form-control" Enabled="false"></asp:TextBox>
                                </div>
                            </div>
                            <div class="col-sm-2">
                                <div class="form-group">
                                    <label>Código</label>
                                    <asp:TextBox runat="server" id="txtCodigo" cssClass="form-control" Enabled="false"></asp:TextBox>
                                </div>
                            </div>
                            <div class="col-sm-6">
                                <div class="form-group">
                                    <label>Licenciado</label>
                                    <asp:TextBox runat="server" id="txtRazaosocial" cssClass="form-control" Enabled="false"></asp:TextBox>
                                </div>
                            </div>
                        </div>

                        <h3><span class="float-center badge bg-whitelabel1">2. Termos, condições e política</span></h3>

                        <div class="row">
                            <div class="col-sm-12">
                                <div class="form-group">
                                <label>Termos e Condições de Uso<strong class="text-danger">*</strong></label>
                                <textarea runat="server" id="txtTermo" class="textarea" rows="10"></textarea>
                                </div>
                            </div>
                        </div>

                        <div class="row">
                            <div class="col-sm-12">
                                <div class="form-group">
                                <label>Política de Privacidade<strong class="text-danger">*</strong></label>
                                <textarea runat="server" id="txtPolitica" class="textarea" rows="10"></textarea>
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

    <script type="text/javascript" src="../plugins/moment/moment.min.js"></script>
    <script type="text/javascript" src="../plugins/moment/moment-with-locales.js"></script>
    <script type="text/javascript" src="../plugins/inputmask/min/jquery.inputmask.bundle.min.js"></script>
    <script type="text/javascript" src="../plugins/daterangepicker/daterangepicker.js"></script>
    <script type="text/javascript" src="../plugins/bootstrap-colorpicker/js/bootstrap-colorpicker.min.js"></script>
    <script type="text/javascript" src="../plugins/tempusdominus-bootstrap-4/js/tempusdominus-bootstrap-4.min.js"></script>

    <script type="text/javascript" src="../plugins/chart.js/Chart.min.js"></script>
    <script type="text/javascript" src="../dist/js/demo.js"></script>

    <script type="text/javascript" src="../plugins/summernote/summernote-bs4.min.js"></script>
    <script type="text/javascript">
        $(function () {
            // Summernote
            $('.textarea').summernote(
                {
                    height: 200,   //set editable area's height
                    codemirror: { // codemirror options
                        theme: 'monokai'
                    }
                }
            );
        })
    </script>

</body>
</html>
