<%@ Page Language="C#" AutoEventWireup="true" CodeFile="cad_habilitar_terminal.aspx.cs" Inherits="cad_habilitar_terminal" %>

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
                <h3 class="card-title">Habilitar Terminais S920, D195 e Similares</h3><br />

                </div>
                <div class="card-body">

                <div class="row">
                    <div class="col-5">
                        <div class="card">            
                            <div class="card-body">                    
                                <div class="row">
                                        <div class="form-group">
                                            <label class="col-sm-12 col-form-label">Selecione o modelo que deseja habilitar?</label>
                                            <asp:DropDownList runat="server" id="ddlModelo" class="form-control" 
                                                onselectedindexchanged="ddlModelo_SelectedIndexChanged" 
                                                AutoPostBack="True">
                                            </asp:DropDownList>
                                        </div>
                                </div>
                                <div class="row d-flex align-items-center">
                                    <div class="col-12">
                                        <div class="form-group">
                                            <label class="col-sm-12 col-form-label text-center"><asp:Label runat="server" ID="lblModelo"></asp:Label></label>
                                            <img runat="server" id="imgModelo" src="" class="img-fluid" alt="" />
                                        </div>
                                    </div>
                                </div>

                            </div>
                        </div>
                    </div>

                    <div class="col-7">
                        <div class="card">            
                            <div class="card-body">                    
                                <div class="row">

                                    <div class="col-7">
                                        <h6><strong>Como habilitar um terminal?</strong></h6><br />
                                        <h6><strong>Ao receber o equipamento:</strong></h6>
                                        <small>1. LIGUE O EQUIPAMENTO</small><br />
                                        <small>2. Certifique-se que o mesmo esteja conectado à internet</small><br />
                                        <small>3. Aguarde o token aparecer na tela</small><br />
                                        <small>4. Insira o token no campo abaixo</small><br />
                                        <small>5. Selecione o estabelecimento que será associado ao terminal</small><br />
                                        <small>6. Pressione habilitar terminal</small><br />
                                    </div>
                                    <div class="col-5">
                                        <div class="form-group">
                                            <label class="col-sm-12 col-form-label">ID</label>
                                            <asp:Label runat="server" id="lblID" class="form-control"></asp:Label>
                                        </div>
                                        <div class="form-group">
                                            <label class="col-sm-12 col-form-label">Serial</label>
                                            <asp:TextBox runat="server" id="txtSerial" class="form-control"></asp:TextBox>
                                        </div>
                                        <div class="form-group">
                                            <label class="col-sm-12 col-form-label">Adquirente</label>
                                            <asp:DropDownList runat="server" id="ddlAdquirente" class="form-control">
                                                <asp:ListItem Text="Zoop" Value="Zoop"></asp:ListItem>
                                            </asp:DropDownList>
                                        </div>
                                    </div>
                                    <div class="col-12 m-1">

                                        <div class="form-group">
                                            <label class="col-sm-12 col-form-label">Estabelecimento</label>
                                            <asp:DropDownList runat="server" id="ddlEstabelecimento" CssClass="form-control">
                                            </asp:DropDownList>
                                        </div>

                                        <div class="form-group">
                                            <asp:TextBox runat="server" id="txtToken" class="form-control" placeholder="Token ou Terminal"></asp:TextBox>
                                        </div>                                    
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>

                </div>



 
            </div>
            <div class="card-footer">
                        <asp:Button runat="server" ID="btnSalvar" CssClass="btn btn-whitelabel1" Text="Habilitar Terminal" onclick="btnSalvar_Click"/>
                        <asp:Button runat="server" ID="btnCancelar" CssClass="btn  btn btn-whitelabel1 float-right" Text="Fechar" onclick="btnCancelar_Click"/>
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

