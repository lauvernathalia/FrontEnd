<%@ Page Language="C#" AutoEventWireup="true" CodeFile="con_historico_vendas_adm.aspx.cs" Inherits="con_historico_vendas_adm" Async="true" %>

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
                              <li class="breadcrumb-item"><a href="#">Operações Diárias</a></li>
                              <li class="breadcrumb-item active">Histórico Venda</li>
                            </ol>
                          </div>
                        </div>

                        <div class="row">
                          <div class="col-12">

                            <div class="card">

                              <div class="card-header bg-whitelabel1">
                                <h3 class="card-title">Campos de Pesquisa</h3>
                              </div>
                              <div class="card-body">
                                <div class="row form-group col-12">
                                    <label class="col-sm-2 col-form-label">Código Venda</label>
                                    <div class="col-sm-4">
                                        <asp:TextBox id="txtFiltro" runat="server" class="form-control" placeholder="Codigo da Transação"></asp:TextBox>
                                    </div>
                                    <label class="col-sm-2 col-form-label">Estabelecimento</label>
                                    <div class="col-sm-4">
                                        <asp:DropDownList runat="server" id="ddlEstabelecimentos" CssClass="form-control">
                                        </asp:DropDownList>
                                    </div>

                                </div>

                                <div class="form-group row col-12">
                                    <label class="col-2 col-form-label">Data Início</label>
						            <div class="col-2">
                                        <div class="input-group date" id="datepickerIni" data-target-input="nearest">
								            <asp:TextBox ID="txtDataIni" runat="server" CssClass="form-control datetimepicker-input" data-target="#datepickerIni"></asp:TextBox>
                                            <div class="input-group-append" data-target="#datepickerIni" data-toggle="datetimepicker">
                                                <div class="input-group-text"><i class="fa fa-calendar"></i></div>
                                            </div>
                                        </div>
                                    </div>

                                    <label class="col-2 col-form-label">Data Fim</label>
						            <div class="col-2">
                                        <div class="input-group date" id="datepickerFim" data-target-input="nearest">
								            <asp:TextBox ID="txtDataFim" runat="server" CssClass="form-control datetimepicker-input" data-target="#datepickerFim"></asp:TextBox>
                                            <div class="input-group-append" data-target="#datepickerFim" data-toggle="datetimepicker">
                                                <div class="input-group-text"><i class="fa fa-calendar"></i></div>
                                            </div>
                                        </div>
                                    </div>

                                    <label class="col-2 col-form-label">Status</label>
						            <div class="col-2">
                                        <asp:DropDownList runat="server" id="ddlStatus" CssClass="form-control" ></asp:DropDownList>
                                    </div>
                                </div>
                                <div class="form-group row col-12">
                                    <label class="col-2 col-form-label">Marketplace</label>
						            <div class="col-4">
                                        <asp:DropDownList runat="server" id="ddlMarketplace" CssClass="form-control" ></asp:DropDownList>
                                    </div>

                                    <label class="col-2 col-form-label">Representante</label>
						            <div class="col-4">
                                        <asp:DropDownList runat="server" id="ddlRepresentante" CssClass="form-control" ></asp:DropDownList>
                                    </div>
                                </div>

                                <div class="form-group row col-12">
                                    <label class="col-2 col-form-label">Tipo de Pagamento</label>
						            <div class="col-3">
                                        <asp:DropDownList runat="server" id="ddlTipoPagamento" CssClass="form-control" ></asp:DropDownList>
                                    </div>
                                    <label class="col-2 col-form-label">Adquirente</label>
						            <div class="col-3">
                                        <asp:DropDownList runat="server" id="ddlAdquirente" CssClass="form-control" >
                                        </asp:DropDownList>
                                    </div>

                                    <div class="col-2">
                                        <asp:Button runat="server" ID="btnPesquisar" CssClass="btn btn-whitelabel1 col-12" Text="Pesquisar" onclick="btnPesquisar_Click"/>
                                        <asp:Button ID="btnPostback" runat="server" Visible="false" OnClick="btnPostBack_Click" />
                                        <script type="text/javascript">
                                            function PostBackOnMainPage(){
                                            <%=GetPostBackScript()%>
                                            }
                                        </script>
                                    </div>

                                </div>
                                
                              </div>

                              <div class="card-footer" style="overflow:auto; width: 100%; ">
                                <div class="row">
                                    <div class="col-md-2 col-sm-6 col-12">
                                        <div class="info-box">
                                            <span class="info-box-icon bg-secondary"><i class="fas fa-coins"></i></span>

                                            <div class="info-box-content">
                                                <span class="info-box-text">Vendas</span>
                                                <span class="info-box-number"><asp:Label runat="server" id="lblVendas" text="0"></asp:Label></span>
                                            </div>
                                        </div>
                                    </div>                                
                                    <div class="col-md-2 col-sm-6 col-12">
                                        <div class="info-box">
                                            <span class="info-box-icon bg-secondary"><i class="fas fa-thumbs-up"></i></span>

                                            <div class="info-box-content">
                                                <span class="info-box-text">Aprovadas</span>
                                                <span class="info-box-number"><asp:Label runat="server" id="lblAprovadas" text="0"></asp:Label></span>
                                            </div>
                                        </div>
                                    </div>                                
                                    <div class="col-md-2 col-sm-6 col-12">
                                        <div class="info-box">
                                            <span class="info-box-icon bg-secondary"><i class="fas fa-hand-holding-usd"></i></span>

                                            <div class="info-box-content">
                                                <span class="info-box-text">Total Saldo</span>
                                                <span class="info-box-number"><asp:Label runat="server" id="lblSaldo" text="0"></asp:Label></span>
                                            </div>
                                        </div>
                                    </div>                                
                                    <div class="col-md-2 col-sm-6 col-12">
                                        <div class="info-box">
                                            <span class="info-box-icon bg-secondary"><i class="fas fa-sort-amount-up"></i></span>

                                            <div class="info-box-content">
                                                <span class="info-box-text">Total Lançamentos</span>
                                                <span class="info-box-number"><asp:Label runat="server" id="lblLancamentos" text="0"></asp:Label></span>
                                            </div>
                                        </div>
                                    </div>                                
                                    <div class="col-md-2 col-sm-6 col-12">
                                        <div class="info-box">
                                            <span class="info-box-icon bg-secondary"><i class="fas fa-receipt"></i></span>

                                            <div class="info-box-content">
                                                <span class="info-box-text">Ticket Médio</span>
                                                <span class="info-box-number"><asp:Label runat="server" id="lblTicket" text="0"></asp:Label></span>
                                            </div>
                                        </div>
                                    </div>                                
                                    <div class="col-md-2 col-sm-6 col-12">
                                        <div class="info-box">
                                            <span class="info-box-icon bg-secondary"><i class="fas fa-thumbs-down"></i></span>

                                            <div class="info-box-content">
                                                <span class="info-box-text">Falhadas</span>
                                                <span class="info-box-number"><asp:Label runat="server" id="lblFalhadas" text="0"></asp:Label></span>
                                            </div>
                                        </div>
                                    </div>                                
                                
                                </div>
                              </div>

                              </div>

                            </div>
                          </div>
                          <div class="row">
                            <div class="col-12">
                                <div class="card">

                                  <div class="card-header bg-whitelabel1">
                                    <h3 class="card-title">Histórico de Vendas</h3>
                                  </div>
                                  <div class="card-body"  style="overflow:auto; width: 100%;">


                                    <div class="row">
                                        <div class="col-12">


                                            <asp:GridView ID="gvConsulta" runat="server" 
                                                AutoGenerateColumns="false" BorderStyle="None" 
                                                CssClass="table table-striped nowrap table-bordered table-hover" 
                                                onrowdatabound="gvConsulta_RowDataBound" AllowPaging="True" 
                                                onpageindexchanging="gvConsulta_PageIndexChanging">
                                                <AlternatingRowStyle BorderStyle="None" />
                                                <RowStyle BorderStyle="None" />
                                                <HeaderStyle BorderWidth="1" BorderStyle="Solid" />
                                                <Columns>

                                                  <asp:TemplateField HeaderText="Data">
                                                    <ItemTemplate>
                                                        <small><%# String.Format("{0:dd/MM/yyyy}",DataBinder.Eval(Container.DataItem, "DTA_DATA"))%> <%# String.Format("{0:HH:mm}",DataBinder.Eval(Container.DataItem, "DTA_DATA"))%></small>
                                                    </ItemTemplate>
                                                  </asp:TemplateField>
                                                  <asp:TemplateField HeaderText="ID">
                                                    <ItemTemplate>
                                                    <small><strong><%# DataBinder.Eval(Container.DataItem, "COD_ID")%></strong></small>
                                                    </ItemTemplate>
                                                  </asp:TemplateField>

                                                  <asp:TemplateField HeaderText="Adquirente">
                                                    <ItemTemplate>
                                                        <small><strong><%# DataBinder.Eval(Container.DataItem, "NOM_ADQUIRENTE")%></strong></small>
                                                    </ItemTemplate>
                                                  </asp:TemplateField>

                                                  <asp:TemplateField HeaderText="Code">
                                                    <ItemTemplate>
                                                        <p style="max-width: 7ch;  overflow: hidden;  text-overflow: ellipsis;  white-space: nowrap;"><small><strong><%# DataBinder.Eval(Container.DataItem, "NOM_CODE")%></strong></small></p>
                                                    </ItemTemplate>
                                                  </asp:TemplateField>

                                                  <asp:TemplateField HeaderText="Estabelecimento">
                                                    <ItemTemplate>
                                                        <small><%# DataBinder.Eval(Container.DataItem, "NOM_RAZAOSOCIAL")%></small>
                                                    </ItemTemplate>
                                                  </asp:TemplateField>

                                                  <asp:TemplateField HeaderText="Marketplace">
                                                    <ItemTemplate>
                                                        <small><%# DataBinder.Eval(Container.DataItem, "NOM_MARKETPLACE")%></small>
                                                    </ItemTemplate>
                                                  </asp:TemplateField>
                                                  <asp:TemplateField HeaderText="% Mkt">
                                                    <ItemTemplate>
                                                        <small><%# String.Format("{0:n2}",DataBinder.Eval(Container.DataItem, "NUM_PERCENTUAL_COMISSIONAMENTO_MARKETPLACE"))%></small>
                                                    </ItemTemplate>
                                                  </asp:TemplateField>

                                                  <asp:TemplateField HeaderText="Representante">
                                                    <ItemTemplate>
                                                        <small><%# DataBinder.Eval(Container.DataItem, "NOM_REPRESENTANTE")%></small>
                                                    </ItemTemplate>
                                                  </asp:TemplateField>


                                                  <asp:TemplateField HeaderText="% Rep">
                                                    <ItemTemplate>
                                                        <small><%# String.Format("{0:n2}",DataBinder.Eval(Container.DataItem, "NUM_PERCENTUAL_COMISSIONAMENTO_REPRESENTANTE"))%></small>
                                                    </ItemTemplate>
                                                  </asp:TemplateField>

                                                  <asp:TemplateField HeaderText="Valor Venda">
                                                    <ItemTemplate>
                                                        <small><%# String.Format("{0:c2}",DataBinder.Eval(Container.DataItem, "NUM_VALOR_BRUTO"))%></small>
                                                    </ItemTemplate>
                                                  </asp:TemplateField>


                                                  <asp:TemplateField HeaderText="Total Taxas">
                                                    <ItemTemplate>
                                                        <small><%# String.Format("{0:c2}",DataBinder.Eval(Container.DataItem, "NUM_VALOR_TOTAL_TAXAS"))%></small>
                                                    </ItemTemplate>
                                                  </asp:TemplateField>

                                                  <asp:TemplateField HeaderText="Valor Líquido">
                                                    <ItemTemplate>
                                                        <small><%# String.Format("{0:c2}",DataBinder.Eval(Container.DataItem, "NUM_VALOR_LIQUIDO"))%></small>
                                                    </ItemTemplate>
                                                  </asp:TemplateField>

                                                  <asp:TemplateField HeaderText="No. Parcelas">
                                                    <ItemTemplate>
                                                        <small><%# DataBinder.Eval(Container.DataItem, "NUM_PARCELAS")%></small>
                                                    </ItemTemplate>
                                                  </asp:TemplateField>
                                                  <asp:TemplateField HeaderText="Plano">
                                                    <ItemTemplate>
                                                        <small><%# DataBinder.Eval(Container.DataItem, "NOM_TITULO_PLANO")%> (<%# DataBinder.Eval(Container.DataItem, "FLG_ANTECIPADO")%>)</small>
                                                    </ItemTemplate>
                                                  </asp:TemplateField>
                                                  <asp:TemplateField HeaderText="Tipo">
                                                    <ItemTemplate>
                                                        <small><%# DataBinder.Eval(Container.DataItem, "NOM_TIPO_OPERACAO")%></small>
                                                    </ItemTemplate>
                                                  </asp:TemplateField>
                                                  <asp:TemplateField HeaderText="Bandeira">
                                                    <ItemTemplate>
                                                        <small><%# DataBinder.Eval(Container.DataItem, "NOM_BANDEIRA")%>
                                                    </ItemTemplate>
                                                  </asp:TemplateField>
                                                  <asp:TemplateField HeaderText="Bandeira">
                                                    <ItemTemplate>
                                                        <asp:Image runat="server" id="imgBandeira" CssClass="img-fluid img-thumbnail mx-auto" style="max-width:50px;" ImageUrl='<%# DataBinder.Eval(Container.DataItem, "NOM_IMAGEM_BANDEIRAS")%>'></asp:Image></small>
                                                    </ItemTemplate>
                                                  </asp:TemplateField>

                                                  <asp:TemplateField HeaderText="Rep/Mkt">
                                                    <ItemTemplate>
                                                        <small><span class="text-danger">Tx.</span><%# DataBinder.Eval(Container.DataItem, "NUM_PERCENTUAL_TAXA")%></small>- <small><span class="text-danger">Reb.</span><%# DataBinder.Eval(Container.DataItem, "NUM_REBATE_TAXA")%></small>
                                                    </ItemTemplate>
                                                  </asp:TemplateField>

                                                  <asp:TemplateField HeaderText="Comissão">
                                                    <ItemTemplate>
                                                        <small><%# String.Format("{0:n2}",DataBinder.Eval(Container.DataItem, "NUM_VALOR_COMISSAO"))%></small>
                                                    </ItemTemplate>
                                                  </asp:TemplateField>

                                                  <asp:TemplateField HeaderText="Status">
                                                    <ItemTemplate>
                                                        <small><%# DataBinder.Eval(Container.DataItem, "NOM_STATUS")%></small>
                                                    </ItemTemplate>
                                                  </asp:TemplateField>
                                                  <asp:TemplateField HeaderText="Serial">
                                                    <ItemTemplate>
                                                        <small><%# DataBinder.Eval(Container.DataItem, "NUM_DEVICE_SERIAL")%></small>
                                                    </ItemTemplate>
                                                  </asp:TemplateField>
                                                  <asp:TemplateField HeaderText="Canal">
                                                    <ItemTemplate>
                                                        <small><%# DataBinder.Eval(Container.DataItem, "NOM_FLG_PRESENCIAL_ONLINE")%></small>
                                                    </ItemTemplate>
                                                  </asp:TemplateField>
                                                  <asp:TemplateField HeaderText="Detalhe">
                                                    <ItemTemplate>
                                                        <a href="" onclick="javascript:openPopupWindow('cad_historico_vendas_detalhe.aspx?id=<%# Funcoes.Encrypt(DataBinder.Eval(Container.DataItem, "COD_ID").ToString()) %>', 'HistoricoVendaDetalhe', 1024, 800); return false;" class="btn btn-block btn-whitelabel1 m-1">
                                                            Detalhe
                                                        </a>
                                                    </ItemTemplate>
                                                  </asp:TemplateField>


                                                </Columns>
                                                <PagerStyle BorderStyle="None" />
                                                <RowStyle BorderStyle="None" />
                                            </asp:GridView>  





                                        
                                        </div>
                                    </div>
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
