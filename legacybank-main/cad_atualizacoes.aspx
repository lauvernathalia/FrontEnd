<%@ Page Language="C#" AutoEventWireup="true" CodeFile="cad_atualizacoes.aspx.cs" Inherits="cad_atualizacoes" %>

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
                <h3 class="card-title">ATUALIZAÇÕES & MELHORIAS</h3>
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
                                <label class="col-sm-12 col-form-label">Licenciado</label>
                                <asp:DropDownList runat="server" id="ddlLicenciado" CssClass="form-control">
                                </asp:DropDownList>
                            </div>
                        </div>
                    </div>

                    <div class="row">
                        <div class="col-sm-12">
                            <div class="form-group">
                                <label class="col-sm-12 col-form-label">Título</label>
                                <asp:TextBox id="txtNome"  runat="server" class="form-control"></asp:TextBox>
                            </div>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-sm-12">
                            <div class="form-group">
                                <label class="col-sm-12 col-form-label">Descrição</label>
                                <div class="col-sm-12">
                                    <textarea runat="server" id="txtDescricao" class="textarea" rows="5"></textarea>
                                </div>
                            </div>
                        </div>
                    </div>

                    <div class="row">
                        <div class="col-sm-4">
                            <div class="form-group">
                                <label class="col-12 col-form-label">Data Início</label>
						        <div class="col-12">
                                    <div class="input-group date" id="datepickerIni" data-target-input="nearest">
								        <asp:TextBox ID="txtDataInicio" runat="server" CssClass="form-control datetimepicker-input" data-target="#datepickerIni"></asp:TextBox>
                                        <div class="input-group-append" data-target="#datepickerIni" data-toggle="datetimepicker">
                                            <div class="input-group-text"><i class="fa fa-calendar"></i></div>
                                        </div>
                                    </div>
                                </div>

                            </div>
                        </div>
                        <div class="col-sm-4">
                            <div class="form-group">

                                <label class="col-12 col-form-label">Data Término</label>
						        <div class="col-12">
                                    <div class="input-group date" id="datepickerFim" data-target-input="nearest">
								        <asp:TextBox ID="txtDataTermino" runat="server" CssClass="form-control datetimepicker-input" data-target="#datepickerFim"></asp:TextBox>
                                        <div class="input-group-append" data-target="#datepickerFim" data-toggle="datetimepicker">
                                            <div class="input-group-text"><i class="fa fa-calendar"></i></div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>

                        <div class="col-sm-4">
                            <label class="col-sm-12 col-form-label">Perfil</label>
                            <asp:DropDownList runat="server" id="ddlPerfil" CssClass="form-control">
                                <asp:ListItem Value="T">Todos</asp:ListItem>
                                <asp:ListItem Value="E">Estabelecimentos</asp:ListItem>
                                <asp:ListItem Value="R">Representantes</asp:ListItem>
                                <asp:ListItem Value="M">Marketplaces</asp:ListItem>
                                <asp:ListItem Value="A">Licenciado</asp:ListItem>
                            </asp:DropDownList>
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
                </div>
            </div>
            </div>
        </div>



    </form>

    <script type="text/javascript" src="../plugins/datatables/jquery.dataTables.js"></script>
    <script type="text/javascript" src="../plugins/datatables-bs4/js/dataTables.bootstrap4.js"></script>

    <script src="../plugins/inputmask/jquery.inputmask.bundle.js"></script>
    <script src="../plugins/moment/moment.min.js"></script>

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
            $('.textarea').summernote({ height: 300, })
        })
    </script>


<script type="text/javascript">

    $(function () {

        //Date range picker
        $('#datepickerIni').datetimepicker({
            format: 'DD/MM/YYYY'
        });
        $('#datepickerFim').datetimepicker({
            format: 'DD/MM/YYYY'
        });
    })
</script>  

</body>
</html>
