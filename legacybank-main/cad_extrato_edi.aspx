<%@ Page Language="C#" AutoEventWireup="true" CodeFile="cad_extrato_edi.aspx.cs" Inherits="cad_extrato_edi" Async="true" EnableSessionState="ReadOnly" MaintainScrollPositionOnPostback="true" %>


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
        <asp:ScriptManager ID="ScriptManager1" runat="server" EnablePageMethods="true" />
        <div class="row">
            <div class="col-12">

                <div class="card">
                    <div class="card-header bg-whitelabel1">
                        <h3 class="card-title">EXTRATO TRANSACIONAL EDI</h3>
                    </div>
                    <div class="card-body">
                        <h3><span class="float-center badge bg-whitelabel1">1. Dados de Acesso</span></h3>

                        <div class="row">
                            <div class="col-sm-12">
                                <div class="form-group">
                                    <label>Redes</label>
                                    <asp:DropDownList runat="server" ID="ddlRedes" CssClass="form-control" 
                                        AutoPostBack="True" onselectedindexchanged="ddlRedes_SelectedIndexChanged">
                                    </asp:DropDownList>
                                </div>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-sm-2">
                                <div class="form-group">
                                    <label>USER</label>
                                    <asp:TextBox runat="server" id="txtUSER" cssClass="form-control" Enabled="false"></asp:TextBox>
                                </div>
                            </div>
                            <div class="col-sm-8">
                                <div class="form-group">
                                    <label>TOKEN</label>
                                    <asp:TextBox runat="server" id="txtTOKEN" cssClass="form-control" Enabled="false"></asp:TextBox>
                                </div>
                            </div>
                            <div class="col-sm-8">
                                <div class="form-group">
                                    <label>Operações</label>
                                    <asp:DropDownList runat="server" ID="ddlOperacao" CssClass="form-control">
                                        <asp:ListItem Value="transactional" Text="Transacional"></asp:ListItem>
                                        <asp:ListItem Value="financial" Text="Financeiro"></asp:ListItem>
                                    </asp:DropDownList>
                                </div>
                            </div>
                        </div>

                        <div class="row">
						    <div class="col-2">
                                <label>Data</label>
                                <div class="input-group date" id="datepickerIni" data-target-input="nearest">
								    <asp:TextBox ID="txtDataIni" runat="server" CssClass="form-control datetimepicker-input" data-target="#datepickerIni"></asp:TextBox>
                                    <div class="input-group-append" data-target="#datepickerIni" data-toggle="datetimepicker">
                                        <div class="input-group-text"><i class="fa fa-calendar"></i></div>
                                    </div>
                                </div>
                            </div>                            
                            <div class="col-sm-8">
                                <div class="form-group">
                                    <label>No. Transações</label>
                                    <asp:TextBox runat="server" id="txtTransacoes" cssClass="form-control" Enabled="false"></asp:TextBox>
                                </div>
                            </div>
                            <div class="col-sm-2">
                                <div class="form-group">
                                    <label>No. Páginas</label>
                                    <asp:TextBox runat="server" id="txtPaginas" cssClass="form-control" Enabled="false"></asp:TextBox>
                                </div>
                            </div>
                        </div>

                        <div class="row" runat="server" id="divRetorno" visible="true">
                            <div class="col-12">
                                <div class="form-group">
                                    <label class="col-sm-2 col-form-label">Extrato</label>
                                    <div class="col-sm-10">
                                        <asp:TextBox runat="server" ID="txtExtrato" TextMode="MultiLine" Rows="5" CssClass="form-control"></asp:TextBox>
                                    </div>
                                </div>
                            </div>
                        </div>


                    </div>

                    <div class="card-footer">
                        <asp:Button runat="server" ID="btnCarregar" CssClass="btn btn-whitelabel1" Text="Carregar Dados" onclick="btnCarregar_Click"/>
                        <asp:Button runat="server" ID="btnImportar" CssClass="btn btn-whitelabel1" Text="Importar Dados" OnClick="btnImportar_Click" OnClientClick="iniciarProgresso();" />
                        <asp:Button runat="server" ID="btnCancelar" CssClass="btn btn-danger float-right" Text="Cancelar" onclick="btnCancelar_Click"/>
                    </div>
                    <div class="progress" style="height: 25px; margin-top: 10px;">
                      <div id="barraProgresso" class="progress-bar progress-bar-striped progress-bar-animated" role="progressbar" style="width: 0%">0%
                      </div>
                    </div>

                    <div style="margin-top:15px;">
                        <div style="width:100%; background:#eee; border-radius:4px; height:25px;">
                            <div id="barProgresso"
                                 style="width:0%; height:25px; background:#28a745; color:#fff; 
                                        text-align:center; line-height:25px; border-radius:4px;">
                                0%
                            </div>
                        </div>

                        <div style="margin-top:5px;">
                            Processados: <span id="lblAtual">0</span> /
                            <span id="lblTotal">0</span>
                        </div>
                    </div>

                </div>            
            
            </div>
        </div>



        <div class="row" runat="server" id="divTabela" visible="true">
            <div class="col-12">

                <div class="card">
                    <div class="card-header bg-whitelabel1">
                        <h3 class="card-title">LISTA DE TRANSACOES</h3>
                    </div>
                    <div class="card-body">
                        <table id="tbConsulta" class="table table-bordered table-hover">
                            <thead>
                            <tr>
                                <th>ID Estabelecimento</th>
                                <th>Transação</th>
                                <th>Data</th>
                                <th>Valor</th>
                            </tr>
                            </thead>
                            <tbody>
                            <asp:Repeater runat="server" ID="rptConsulta">
                                <ItemTemplate>
                                    <tr>
                                        <td>
                                        </td>

                                        <td>
                                        </td>

                                        <td>
                                        </td>

                                        <td>
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

    </form>

<script type="text/javascript" src="../plugins/datatables/jquery.dataTables.js"></script>
<script type="text/javascript" src="../plugins/datatables-bs4/js/dataTables.bootstrap4.js"></script>

<script src="../plugins/inputmask/jquery.inputmask.bundle.js"></script>
<script src="../plugins/moment/moment.min.js"></script>


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
    var intervalo;

    // Quando clicar no botão Importar
    $("#<%= btnImportar.ClientID %>").click(function () {
        // inicia polling
        intervalo = setInterval(atualizarProgresso, 1000);
    });

    function atualizarProgresso() {
        $.ajax({
            type: "POST",
            url: "cad_extrato_edi.aspx/GetProgresso",
            data: "{}",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (res) {
                var valor = res.d;
                $("#barraProgresso").css("width", valor + "%").text(valor + "%");

                if (valor >= 100) {
                    clearInterval(intervalo);
                }
            }
        });
    }
</script>

<script type="text/javascript">
    var timerProgresso = null;

    function iniciarProgresso() {
        if (timerProgresso != null)
            clearInterval(timerProgresso);

        timerProgresso = setInterval(atualizarProgresso, 1000); // 1s
    }

    function pararProgresso() {
        if (timerProgresso != null)
            clearInterval(timerProgresso);
    }

    function atualizarProgresso() {

        PageMethods.ProgressoImportacao(onSucessoProgresso, onErroProgresso);

    }

    function onSucessoProgresso(ret) {

        if (!ret) return;

        var total = ret.total || 0;
        var atual = ret.atual || 0;

        document.getElementById("lblTotal").innerHTML = total;
        document.getElementById("lblAtual").innerHTML = atual;

        var perc = 0;
        if (total > 0)
            perc = Math.round((atual * 100) / total);

        var bar = document.getElementById("barProgresso");
        bar.style.width = perc + "%";
        bar.innerHTML = perc + "%";

        // finalizou
        if (total > 0 && atual >= total) {
            pararProgresso();
        }
    }

    function onErroProgresso() {
        // se der erro, só para o timer
        pararProgresso();
    }
</script>

</body>
</html>
