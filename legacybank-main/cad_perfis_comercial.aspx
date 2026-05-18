<%@ Page Language="C#" AutoEventWireup="true" CodeFile="cad_perfis_comercial.aspx.cs" Inherits="cad_perfis_comercial" %>

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
                        <h3 class="card-title">PERFIL CONFIGURAÇÃO COMERCIAL</h3>
                    </div>
                    <div class="card-body">
                        <div class="row">
                            <div class="col-sm-2">
                                <div class="form-group">
                                    <label>ID</label>
                                    <asp:TextBox runat="server" id="txtID" cssClass="form-control" Enabled="false"></asp:TextBox>
                                </div>
                            </div>
                            <div class="col-sm-10">
                                <div class="form-group">
                                    <label>Perfil</label>
                                    <asp:TextBox runat="server" id="txtPerfil" cssClass="form-control" Enabled="false"></asp:TextBox>
                                </div>
                            </div>
                        </div>

                        <div class="row">
                            <div class="col-12">


                                <div class="card">
                                    <div class="card-header bg-whitelabel1">
                                        <h3 class="card-title">CONFIGURAÇÃO COMERCIAL</h3>
                                    </div>
                                    <div class="card-body">
                                        <div class="row">
                                            <div class="col-sm-12">
                                                <h5 class="text-whitelabel1">Marque abaixo as operações de cobrança permitidas para este perfil</h5>
                                            </div>
                                        </div>
                                        

                                        <div class="row">
                                            <div class="col-sm-3">
                                                <div class="form-group">
                                                    <label>Boleto</label>
                                                    <asp:DropDownList runat="server" ID="ddlBoleto" CssClass="form-control">
                                                        <asp:ListItem Value="S" Text="Sim"></asp:ListItem>
                                                        <asp:ListItem Value="N" Text="Não"></asp:ListItem>
                                                    </asp:DropDownList>
                                                </div>
                                            </div>
                                            <div class="col-sm-3">
                                                <div class="form-group">
                                                    <label>Crédito</label>
                                                    <asp:DropDownList runat="server" ID="ddlCredito" CssClass="form-control">
                                                        <asp:ListItem Value="S" Text="Sim"></asp:ListItem>
                                                        <asp:ListItem Value="N" Text="Não"></asp:ListItem>
                                                    </asp:DropDownList>
                                                </div>
                                            </div>
                                            <div class="col-sm-3">
                                                <div class="form-group">
                                                    <label>Pix</label>
                                                    <asp:DropDownList runat="server" ID="ddlPix" CssClass="form-control">
                                                        <asp:ListItem Value="S" Text="Sim"></asp:ListItem>
                                                        <asp:ListItem Value="N" Text="Não"></asp:ListItem>
                                                    </asp:DropDownList>
                                                </div>
                                            </div>
                                            <div class="col-sm-3">
                                                <div class="form-group">
                                                    <label>Parcelado</label>
                                                    <asp:DropDownList runat="server" ID="ddlParcelado" CssClass="form-control">
                                                        <asp:ListItem Value="S" Text="Sim"></asp:ListItem>
                                                        <asp:ListItem Value="N" Text="Não"></asp:ListItem>
                                                    </asp:DropDownList>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>            


                            </div>
                        </div>


                    </div>

                    <div class="card-footer">
                        <asp:Button runat="server" ID="btnSalvar" CssClass="btn btn-whitelabel1" Text="Salvar" onclick="btnSalvar_Click"/>
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


</body>
</html>
