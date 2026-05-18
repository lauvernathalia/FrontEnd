<%@ Page Language="C#" AutoEventWireup="true" CodeFile="con_resumo_vendas.aspx.cs" Inherits="con_resumo_vendas" %>

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
                              <li class="breadcrumb-item"><a href="#">Minhas Vendas</a></li>
                              <li class="breadcrumb-item active">Resumo de Vendas</li>
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

                                <div class="row">
                                    <div class="col-sm-2">
                                        <div class="form-group">
                                            <label>Data Início</label>
                                            <div class="input-group date" id="datepickerIni" data-target-input="nearest">
								                <asp:TextBox ID="txtDataIni" runat="server" CssClass="form-control datetimepicker-input" data-target="#datepickerIni"></asp:TextBox>
                                                <div class="input-group-append" data-target="#datepickerIni" data-toggle="datetimepicker">
                                                    <div class="input-group-text"><i class="fa fa-calendar"></i></div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>

                                    <div class="col-sm-2">
                                        <div class="form-group">
                                            <label>Data Fim</label>
                                            <div class="input-group date" id="datepickerFim" data-target-input="nearest">
								                <asp:TextBox ID="txtDataFim" runat="server" CssClass="form-control datetimepicker-input" data-target="#datepickerFim"></asp:TextBox>
                                                <div class="input-group-append" data-target="#datepickerFim" data-toggle="datetimepicker">
                                                    <div class="input-group-text"><i class="fa fa-calendar"></i></div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>

                                 

                                    <div class="col-sm-4">
                                        <div class="form-group">
                                            <label>Adquirente</label>
                                            <asp:DropDownList runat="server" ID="ddlAdquirentes" CssClass="form-control">
                                            </asp:DropDownList>
                                        </div>
                                    </div>

                                </div>

                              </div>

                              <div class="card-footer">
                                <asp:Button runat="server" ID="btnPesquisar" CssClass="btn btn-whitelabel1 col-2" Text="Pesquisar" onclick="btnPesquisar_Click"/>
                                
                                <asp:Button ID="btnPostback" runat="server" Visible="false" OnClick="btnPostBack_Click" />
                                <script type="text/javascript">
                                    function PostBackOnMainPage(){
                                    <%=GetPostBackScript()%>
                                    }
                                </script>
                              </div>

                              </div>

                            </div>
                          </div>
                          <div class="row">
                            <div class="col-12">
                                <div class="card">

                                  <div class="card-header bg-whitelabel1">
                                    <h3 class="card-title">Resumo de Vendas</h3>
                                  </div>
                                  <div class="card-body">
                                    
                                    
                                    <div class="row">
                                        <div class="col-lg-2 col-6 my-1 d-flex">
                                            <div class="card flex-fill">
                                                <div class="card-body text-center">
                                                    <span class="description-percentage text-warning"><i class="fas fa-coins"></i></span>
                                                    <h6 class="description-header"><asp:Label runat="server" ID="lblVendas" Text="0"></asp:Label></h6>
                                                    <span class="description-text"><small>Qtde. Vendas</small></span>
                                                </div>
                                            </div>
                                        </div>

                                        <div class="col-lg-2 col-6 my-1 d-flex">
                                            <div class="card flex-fill">
                                                <div class="card-body text-center">
                                                    <span class="description-percentage text-success"><i class="fas fa-thumbs-up"></i></span>
                                                    <h6 class="description-header"><asp:Label runat="server" ID="lblAprovadas" Text="0"></asp:Label></h6>
                                                    <span class="description-text"><small>Aprovadas</small></span>
                                                </div>
                                            </div>
                                        </div>

                                        <div class="col-lg-2 col-6 my-1 d-flex">
                                            <div class="card flex-fill">
                                                <div class="card-body text-center">
                                                    <span class="description-percentage text-success"><i class="fas fa-hand-holding-usd"></i></span>
                                                    <h6 class="description-header"><asp:Label runat="server" ID="lblSaldo" Text="0"></asp:Label></h6>
                                                    <span class="description-text"><small>Vlr. Aprovadas</small></span>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="col-lg-2 col-6 my-1 d-flex">
                                            <div class="card flex-fill">
                                                <div class="card-body text-center">

                                                    <span class="description-percentage text-info"><i class="fas fa-receipt"></i></span>
                                                    <h6 class="description-header"><asp:Label runat="server" ID="lblTicket" Text="0"></asp:Label></h6>
                                                    <span class="description-text"><small>Ticket Médio</small></span>
                                                </div>
                                            </div>
                                        </div>

                                        <div class="col-lg-2 col-6 my-1 d-flex">
                                            <div class="card flex-fill">
                                                <div class="card-body text-center">

                                                    <span class="description-percentage text-warning"><i class="fas fa-hand-holding-usd"></i></span>
                                                    <h6 class="description-header"><asp:Label runat="server" ID="lblPendentes" Text="0"></asp:Label></h6>
                                                    <h6><span class="description-text"><small> (<asp:Label runat="server" ID="lblPendentesValor" Text="0"></asp:Label>)</small></span></h6>
                                                    <span class="description-text"><small>Qtde. Pendentes</small></span>
                                                </div>
                                            </div>
                                        </div>

                                        <div class="col-lg-2 col-6 my-1 d-flex">
                                            <div class="card flex-fill">
                                                <div class="card-body text-center">

                                                    <span class="description-percentage text-danger"><i class="fas fa-thumbs-down"></i></span>
                                                    <h6 class="description-header"><asp:Label runat="server" ID="lblFalhadas" Text="0"></asp:Label></h6>
                                                    <h6><span class="description-text"><small> (<asp:Label runat="server" ID="lblFalhadasValor" Text="0"></asp:Label>)</small></span></h6>
                                                    <span class="description-text"><small>Qtde. Falhadas</small></span>
                                                </div>
                                            </div>
                                        </div>
                                    </div>                                    
                                    
                                    
                                    
                                    <div class="row col-sm-12">
                                        <div class="row form-control-sm text-whitelabel1"><small><b>VENDAS POR TIPO OPERAÇÃO</b></small></div>
                                    </div>
                                    <div class="row">
                                        <asp:Repeater runat="server" ID="rptOperacao" 
                                            onitemdatabound="rptOperacao_ItemDataBound">
                                            <ItemTemplate>

                                                <div class="col-lg-3 col-6 my-1 d-flex">
                                                    <div class="card flex-fill">
                                                        <div class="card-body">
                                                            <div class="row form-control-sm text-whitelabel1"><small><b><%# DataBinder.Eval(Container.DataItem, "NOM_TIPO_OPERACAO")%></b></small></div>
                                                            <asp:TextBox runat="server" ID="txtoperacao" Text='<%# DataBinder.Eval(Container.DataItem, "NOM_TIPO_OPERACAO")%>' Visible="false"></asp:TextBox>
                                                            <asp:Repeater runat="server" ID="rptDetalhe">
                                                                <ItemTemplate>
                                                                    <div class="row">
                                                                        <div class="col-12">
                                                                            <div class="row">
                                                                                <div class="col-4">
                                                                                    <h5><asp:Image runat="server" id="Image1" CssClass="img-fluid img-thumbnail mx-auto" style="max-width:50px;" ImageUrl='<%# DataBinder.Eval(Container.DataItem, "NOM_IMAGEM")%>'></asp:Image></h5>
                                                                                </div>
                                                                                <div class="col-8">
                                                                                <span class="description-text form-control-sm"><small><%# DataBinder.Eval(Container.DataItem, "NOM_BANDEIRA")%></small></span><br />
                                                                                <h6 class="description-header  form-control-sm">R$ <%# String.Format("{0:n2}",DataBinder.Eval(Container.DataItem, "NUM_VALOR_BRUTO"))%></h6>
                                                                                </div>
                                                                            </div>
                                                                        </div>
                                                                    </div>

                                                                </ItemTemplate>
                                                            </asp:Repeater>
                                                        </div>
                                                    </div>
                                                </div>

                                            </ItemTemplate>
                                        </asp:Repeater>

                                    </div>




                                    <div class="row col-sm-12">
                                        <div class="row form-control-sm text-whitelabel1"><small><b>VENDAS POR PARCELAMENTO</b></small></div>
                                    </div>
                                    <div class="row">
                                        <asp:Repeater runat="server" ID="rptParcelamento" 
                                            onitemdatabound="rptParcelamento_ItemDataBound">
                                            <ItemTemplate>

                                                <div class="col-lg-3 col-6 my-1 d-flex">
                                                    <div class="card flex-fill">
                                                        <div class="card-body">
                                                            <h5><asp:Image runat="server" id="Image1" CssClass="img-fluid img-thumbnail mx-auto" style="max-width:50px;" ImageUrl='<%# DataBinder.Eval(Container.DataItem, "NOM_IMAGEM")%>'></asp:Image></h5>                                                            
                                                            <div class="row form-control-sm text-whitelabel1"><small><b><%# DataBinder.Eval(Container.DataItem, "NOM_BANDEIRA")%></b></small></div>
                                                            <asp:TextBox runat="server" ID="txtbandeira" Text='<%# DataBinder.Eval(Container.DataItem, "COD_ID")%>' Visible="false"></asp:TextBox>
                                                            <asp:Repeater runat="server" ID="rptDetalheParcelamento">
                                                                <ItemTemplate>
                                                                    <div class="row">
                                                                        <div class="col-12">
                                                                            <div class="row">
                                                                                <div class="col-4">
                                                                                    <span class="description-text form-control-sm"><small><%# DataBinder.Eval(Container.DataItem, "NUM_PARCELAS")%></small></span><br />
                                                                                </div>
                                                                                <div class="col-8">
                                                                                    <h6 class="description-header  form-control-sm">R$ <%# String.Format("{0:n2}",DataBinder.Eval(Container.DataItem, "NUM_VALOR_BRUTO"))%></h6>
                                                                                </div>
                                                                            </div>
                                                                        </div>
                                                                    </div>

                                                                </ItemTemplate>
                                                            </asp:Repeater>
                                                        </div>
                                                    </div>
                                                </div>

                                            </ItemTemplate>
                                        </asp:Repeater>

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

