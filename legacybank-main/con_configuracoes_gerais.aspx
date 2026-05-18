<%@ Page Language="C#" AutoEventWireup="true" CodeFile="con_configuracoes_gerais.aspx.cs" Inherits="con_configuracoes_gerais" %>

<%@ Register TagPrefix="Portal" TagName="PageBottom" Src="rodapepadrao.ascx" %>
<%@ Register TagPrefix="Portal" TagName="PageLeft" Src="menupadrao.ascx" %>
<%@ Register TagPrefix="Portal" TagName="PageHeader" Src="topopadrao.ascx" %>
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
        <div class="wrapper">
            <PORTAL:PAGEHEADER id="PageHeader1" title="Site Directory" runat="server" ModuleSource="topopadrao.ascx"></PORTAL:PAGEHEADER>
            <PORTAL:PAGELEFT id="Pageheader2" title="Site Directory" runat="server" ModuleSource="menupadrao.ascx"></PORTAL:PAGELEFT>
            <div class="content-wrapper">
                <section class="content">
                    <div class="container-fluid" >

                        <div class="row mb-2">
                          <div class="col-sm-12">
                            <ol class="breadcrumb">
                              <li class="breadcrumb-item"><a href="#">Minha Conta</a></li>
                              <li class="breadcrumb-item active">Configurações Gerais</li>
                            </ol>
                          </div>
                        </div>


                        <div class="row">
                            <div class="col-12">

                                <div class="card m-2">
                                    <div class="card-body">


                                    <div class="row">
                                        <div class="col-md-11 col-xs-12 my-1">
                                            <h3 class="modal-title text-black"><b class="text-whitelabel1">Boletos</b> Bancários</h3>
                                        </div>
                                        <div class="col-md-1 col-xs-12 my-1">
                                            <h3><a href="#" data-toggle="collapse" data-target="#ddboletos"><i class="fas fa-arrow-circle-down text-whitelabel1"></i></a></h3>
                                        </div>
                                    </div>

                                    <div class="row">
                                        <div class="col-md-12 col-xs-12 my-1">
                                        <label class="control-label"><b>ATENÇÃO!</b> As informações de multa, juros e descontos informadas abaixo serão utilizadas como padrão para a emissão de boletos bancários!</label>
                                        </div>
                                    </div>


                                    <div id="ddboletos">
                                        <div class="form-group row">
                                            <div class="icheck-primary d-inline col-2">
                                            <asp:CheckBox runat="server" id="ckbMulta" />
                                            <label for="ckbMulta">
                                                Habilitar Multa
                                            </label>
                                            </div>                                        
                                            <label for="inputEmail3" class="col-sm-2 control-label">Cobrar multa de</label>
                                            <div class="col-sm-2">
                                                <asp:DropDownList runat="server" id="ddlMulta" CssClass="form-control">
                                                    <asp:ListItem Value="P">%</asp:ListItem>
                                                    <asp:ListItem Value="V">Valor</asp:ListItem>
                                                </asp:DropDownList>
                                            </div>
                                            <div class="col-sm-2">
                                                <asp:TextBox runat="server" id="txtMulta" cssClass="form-control" ></asp:TextBox>
                                            </div>
                                            <label for="inputEmail3" class="col-sm-2 control-label">após vencimento.</label>
                                        </div>

                                        <div class="form-group row">
                                            <div class="icheck-primary d-inline col-2">
                                            <asp:CheckBox runat="server" id="ckbJuros"></asp:CheckBox>
                                            <label for="ckbJuros">
                                                Habilitar Juros
                                            </label>
                                            </div>                                        
                                            <label for="inputEmail3" class="col-sm-2 control-label">Cobrar juros de</label>
                                            <div class="col-sm-2">
                                                <asp:DropDownList runat="server" id="ddlJuros" CssClass="form-control">
                                                    <asp:ListItem Value="VD">Valor Dia</asp:ListItem>
                                                    <asp:ListItem Value="PD">% Dia</asp:ListItem>
                                                    <asp:ListItem Value="PM">% Mês</asp:ListItem>
                                                </asp:DropDownList>
                                            </div>
                                            <div class="col-sm-2">
                                                <asp:TextBox runat="server" id="txtJuros" cssClass="form-control" ></asp:TextBox>
                                            </div>
                                            <label for="inputEmail3" class="col-sm-2 control-label">após vencimento.</label>

                                        </div>
                                        <div class="form-group row">
                                    
                                            <div class="icheck-primary d-inline col-2">
                                            <asp:CheckBox runat="server" id="ckbDesconto" />
                                            <label for="ckbDesconto">
                                                Habilitar Desconto
                                            </label>
                                            </div>                                        

                                            <label for="inputEmail3" class="col-sm-2 control-label">Desconto de</label>
                                            <div class="col-sm-2">
                                                <asp:DropDownList runat="server" id="ddlDesconto" CssClass="form-control">
                                                    <asp:ListItem Value="P">%</asp:ListItem>
                                                    <asp:ListItem Value="V">Valor</asp:ListItem>
                                                </asp:DropDownList>
                                            </div>
                                            <div class="col-sm-1">
                                                <asp:TextBox runat="server" id="txtDesconto" cssClass="form-control" ></asp:TextBox>
                                            </div>
                                            <label for="inputEmail3" class="col-sm-1 control-label">até</label>
                                            <div class="col-sm-1">
                                                <asp:TextBox runat="server" id="txtDias" cssClass="form-control" ></asp:TextBox>
                                            </div>
                                            <label for="inputEmail3" class="col-sm-3 control-label">dias antes do vencimento</label>


                                        </div>
                                        <div class="form-group row">
                                            <label for="inputEmail3" class="col-sm-12 control-label">Avisos: Informações de descontos, multas e juros não terão validade neste campo</label>
                                        </div>
                                        <div class="form-group row">
                                            <div class="icheck-primary d-inline col-2">
                                            <asp:CheckBox runat="server" id="ckbAvisos" />
                                            <label for="ckbAvisos">
                                                Habilitar Avisos
                                            </label>
                                            </div>
                                            <div class="col-sm-10">
                                                <asp:TextBox runat="server" id="txtAviso" cssClass="form-control" placeholder="Exemplo: Em caso de dúvidas entre em contato pela nossa central de atendimento" ></asp:TextBox>
                                            </div>

                                        </div>
                                    </div>
                                    <hr />

                                </div>
                                <div class="card-footer">
                                    <asp:Button runat="server" ID="btnSalvar" CssClass="btn btn-whitelabel1" Text="Salvar" onclick="btnSalvar_Click"/>
                                </div>

                            </div>
                          </div>



                        </div>
                        </div>
                </section>
            </div>
            <PORTAL:PAGEBOTTOM id="Pageheader3" title="Site Directory" runat="server" ModuleSource="rodapepadrao.ascx"></PORTAL:PAGEBOTTOM><!-- Fim Rodapé da Pagina -->
    
        </div>
    </form>

<script type="text/javascript" src="../plugins/datatables/jquery.dataTables.js"></script>
<script type="text/javascript" src="../plugins/datatables-bs4/js/dataTables.bootstrap4.js"></script>


<script type="text/javascript" src="../plugins/datatables-responsive/js/dataTables.responsive.min.js"></script>
<script type="text/javascript" src="../plugins/datatables-responsive/js/responsive.bootstrap4.min.js"></script>
<script type="text/javascript" src="../plugins/datatables-buttons/js/dataTables.buttons.min.js"></script>
<script type="text/javascript" src="../plugins/datatables-buttons/js/buttons.bootstrap4.min.js"></script>
<script type="text/javascript" src="../plugins/jszip/jszip.min.js"></script>
<script type="text/javascript" src="../plugins/pdfmake/pdfmake.min.js"></script>
<script type="text/javascript" src="../plugins/pdfmake/vfs_fonts.js"></script>
<script type="text/javascript" src="../plugins/datatables-buttons/js/buttons.html5.min.js"></script>
<script type="text/javascript" src="../plugins/datatables-buttons/js/buttons.print.min.js"></script>
<script type="text/javascript" src="../plugins/datatables-buttons/js/buttons.colVis.min.js"></script>

<script type="text/javascript">
    $(function () {
        $('[id$=gvConsulta]').prepend($("<thead></thead>").append($('[id$=gvConsulta]').find("tr:first"))).DataTable({
            "responsive": false,
            "sPaginationType": "full_numbers",
            "oLanguage": {
                "sUrl": '//cdn.datatables.net/plug-ins/2.0.6/i18n/pt-BR.json'
            },
            "paging": true,
            "lengthChange": false,
            "searching": true,
            "ordering": true,
            "info": true,
            "autoWidth": false,
            buttons: ['copy', 'excel', 'pdf', 'csv', 'print'],
            initComplete: function () {
                this.api().buttons().container()
                //.appendTo( $ ('#table_id_wrapper .col-md-6:eq(0)', this.api().table (). container ()));
                //.appendTo( $('#table_id_wrapper .col-md-6:eq(0)' ) );
            .appendTo($('.col-md-6:eq(0)', this.api().table().container()));

            }
        });
    });
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


<script type="text/javascript">

$(document).ready(function () {
    var table = $('#tblConsulta').DataTable({
      "paging": true,
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
