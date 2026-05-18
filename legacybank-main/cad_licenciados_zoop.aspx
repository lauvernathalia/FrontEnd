<%@ Page Language="C#" AutoEventWireup="true" CodeFile="cad_licenciados_zoop.aspx.cs" Inherits="cad_licenciados_zoop" %>

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
                        <h3 class="card-title">LICENCIADOS ZOOP</h3>
                    </div>
                    <div class="card-body">
                        <h3><span class="float-center badge bg-whitelabel1">1. Dados do Licenciado</span></h3>

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

                        <h3><span class="float-center badge bg-whitelabel1">2. Dados ZOOP</span></h3>

                        <div class="row">

                            <div class="col-sm-3">
                                <div class="form-group clearfix">
                                    <label>Habilitar uso ZOOP?<strong class="text-danger">*</strong></label><br />
                                    <div class="icheck-secondary d-inline">
                                    <asp:Checkbox runat="server" ID="ckbZoop" />
                                    <label for="ckbZoop">
                                        Sim
                                    </label>
                                    </div>
                                </div>
                            </div>

                            <div class="col-sm-3">
                                <div class="form-group clearfix">
                                    <label>Utiliza chave própria?<strong class="text-danger">*</strong></label><br />
                                    <div class="icheck-secondary d-inline">
                                    <asp:Checkbox runat="server" ID="ckbTerceiros" />
                                    <label for="ckbTerceiros">
                                        Sim
                                    </label>
                                    </div>
                                </div>
                            </div>


                            <div class="col-sm-3">
                                <div class="form-group">
                                <label>Token (ID)<strong class="text-danger">*</strong></label>
                                <asp:TextBox runat="server" ID="txtToken" CssClass="form-control"></asp:TextBox> 
                                </div>
                            </div>
                            <div class="col-sm-3">
                                <div class="form-group">
                                <label>Chave (Usuário)<strong class="text-danger">*</strong></label>
                                <asp:TextBox runat="server" ID="txtChave" CssClass="form-control"></asp:TextBox> 
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

<script src="../../plugins/bootstrap-colorpicker/js/bootstrap-colorpicker.min.js"></script>

<script>
    $(function () {

        //Colorpicker
        $('.my-colorpicker1').colorpicker()

    })
</script>



</body>
</html>
