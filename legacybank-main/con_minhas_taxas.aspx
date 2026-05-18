<%@ Page Language="C#" AutoEventWireup="true" CodeFile="con_minhas_taxas.aspx.cs" Inherits="con_minhas_taxas" Async="true" %>

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
                              <li class="breadcrumb-item active">Minhas Taxas</li>
                            </ol>
                          </div>
                        </div>

                        <div class="row">
                          <div class="col-12">

                            <div class="card">

                              <div class="card-header bg-whitelabel1">
                                <h3 class="card-title">Minhas Taxas</h3>
                              </div>
                              <div class="card-body">

                                <div class="row" runat="server" id="divJson" visible="false">
                                    <div class="col-sm-12">
                                        <div class="form-group">
                                            <label>Tabela</label>
                                            <asp:TextBox id="txtTabela" runat="server" CssClass="form-control" TextMode="MultiLine" Rows="5"></asp:TextBox>
                                        </div>
                                    </div>
                                </div>

                                
                                <ul class="nav nav-tabs" id="tab_presencial_online" role="tablist">
                                    <li class="nav-item">
                                        <a class="nav-link active" runat="server" id="tab_presencial" data-toggle="pill" href="#conteudo_presencial" role="tab" aria-controls="conteudo_presencial" aria-selected="true">Presencial</a>
                                    </li>
                                    <li class="nav-item">
                                        <a class="nav-link" runat="server" id="tab_online" data-toggle="pill" href="#conteudo_online" role="tab" aria-controls="conteudo_online" aria-selected="false">On-line</a>
                                    </li>
                                </ul>

                                <div class="tab-content" id="tab_presencial_online_conteudo">
                                    <div class="tab-pane fade show active" id="conteudo_presencial" role="tabpanel" aria-labelledby="conteudo_presencial_tab">

                                    <!-- Divisão da Tabela de parcelas **************************************************************************************************** -->
                                    <div class="row" runat="server" id="dvParcelas" visible="false">
                                        <div class="col-12">
                                        <div class="card">

                                            <div class="card-header bg-whitelabel1">
                                            <h3 class="card-title">TAXAS</h3>
                                                <div class="card-tools">
                                                    <button type="button" class="btn btn-tool" data-card-widget="collapse">
                                                    <i class="fas fa-minus"></i>
                                                    </button>
                                                </div>

                                            </div>
                                            <div class="card-body" style="overflow:scroll; height:800px;">

                                                <div class="row">
                                                    <div class="col-12">
                                                        <table id="Table1" class="table table-bordered table-hover table-striped">
                                                            <thead>
                                                            <tr>
                                                                <th><h6><small><b>Bandeira</b></small></h6></th>
                                                                <th><h6><small><b>Débito</b></small></h6></th>
                                                                <th><h6><small><b>Cred.À Vista</b></small></h6></th>
                                                                <th><h6><small><b>Cred.2x</b></small></h6></th>
                                                                <th><h6><small><b>Cred.3x</b></small></h6></th>
                                                                <th><h6><small><b>Cred.4x</b></small></h6></th>
                                                                <th><h6><small><b>Cred.5x</b></small></h6></th>
                                                                <th><h6><small><b>Cred.6x</b></small></h6></th>
                                                                <th><h6><small><b>Cred.7x</b></small></h6></th>
                                                                <th><h6><small><b>Cred.8x</b></small></h6></th>
                                                                <th><h6><small><b>Cred.9x</b></small></h6></th>
                                                                <th><h6><small><b>Cred.10x</b></small></h6></th>
                                                                <th><h6><small><b>Cred.11x</b></small></h6></th>
                                                                <th><h6><small><b>Cred.12x</b></small></h6></th>
                                                                <th><h6><small><b>Cred.13x</b></small></h6></th>
                                                                <th><h6><small><b>Cred.14x</b></small></h6></th>
                                                                <th><h6><small><b>Cred.15x</b></small></h6></th>
                                                                <th><h6><small><b>Cred.16x</b></small></h6></th>
                                                                <th><h6><small><b>Cred.17x</b></small></h6></th>
                                                                <th><h6><small><b>Cred.18x</b></small></h6></th>
                                                            </tr>
                                                            </thead>
                                                            <tbody>
                                                            <asp:Repeater runat="server" ID="rptConsultaParcelas" >
                                                                <ItemTemplate>

                                                                    <tr>
                                                                        <td class="text-center">
                                                                        <asp:TextBox runat="server" id="txtidplano" Text='<%# DataBinder.Eval(Container.DataItem, "COD_ID_PLANOS")%>' visible="false"></asp:TextBox>
                                                                        <asp:TextBox runat="server" id="txtidbandeira" Text='<%# DataBinder.Eval(Container.DataItem, "COD_ID_BANDEIRAS")%>' visible="false"></asp:TextBox>
                                                                            <small><strong>Vlr.Oper.(R$)</strong></small><br />
                                                                            <small><strong>Taxa (%)</strong></small>

                                                                            <small><strong><%# DataBinder.Eval(Container.DataItem, "NOM_BANDEIRA")%></strong></small><br />
                                                                            <asp:Image runat="server" id="imgBandeira" CssClass="img-fluid img-thumbnail mx-auto" style="max-width:50px;" ImageUrl='<%# DataBinder.Eval(Container.DataItem, "NOM_IMAGEM")%>'></asp:Image><br /><br />

                                                                        </td>

                                                                        <td class="text-center" style="min-width:90px;" >
                                                                            <small><strong><asp:Label runat="server" id="Label19" Text='<%# String.Format("{0:n2}",DataBinder.Eval(Container.DataItem, "NUM_VALOR_FIXO_DEBITO"))%>'></asp:Label></strong></small><br />
                                                                            <small><strong><asp:Label runat="server" id="lblfinaldebito" Text='<%# String.Format("{0:n2}",DataBinder.Eval(Container.DataItem, "NUM_VALOR_FINAL_DEBITO"))%>'></asp:Label></strong></small>
                                                                        </td>
                                                                        <td class="text-center" style="min-width:90px;" >
                                                                            <small><strong><asp:Label runat="server" id="Label20" Text='<%# String.Format("{0:n2}",DataBinder.Eval(Container.DataItem, "NUM_VALOR_FIXO_CREDITO_VISTA"))%>'></asp:Label></strong></small><br />
                                                                            <small><strong><asp:Label runat="server" id="Label1" Text='<%# String.Format("{0:n2}",DataBinder.Eval(Container.DataItem, "NUM_VALOR_FINAL_VISTA"))%>'></asp:Label></strong></small>
                                                                        </td>
                                                                        <td class="text-center" style="min-width:90px;" >
                                                                            <small><strong><asp:Label runat="server" id="Label21" Text='<%# String.Format("{0:n2}",DataBinder.Eval(Container.DataItem, "NUM_VALOR_FIXO_CREDITO_2X"))%>'></asp:Label></strong></small><br />
                                                                            <small><strong><asp:Label runat="server" id="Label2" Text='<%# String.Format("{0:n2}",DataBinder.Eval(Container.DataItem, "NUM_VALOR_FINAL_2X"))%>'></asp:Label></strong></small>
                                                                        </td>
                                                                        <td class="text-center" style="min-width:90px;" >
                                                                            <small><strong><asp:Label runat="server" id="Label22" Text='<%# String.Format("{0:n2}",DataBinder.Eval(Container.DataItem, "NUM_VALOR_FIXO_CREDITO_3X"))%>'></asp:Label></strong></small><br />
                                                                            <small><strong><asp:Label runat="server" id="Label3" Text='<%# String.Format("{0:n2}",DataBinder.Eval(Container.DataItem, "NUM_VALOR_FINAL_3X"))%>'></asp:Label></strong></small>
                                                                        </td>
                                                                        <td class="text-center" style="min-width:90px;" >
                                                                            <small><strong><asp:Label runat="server" id="Label23" Text='<%# String.Format("{0:n2}",DataBinder.Eval(Container.DataItem, "NUM_VALOR_FIXO_CREDITO_4X"))%>'></asp:Label></strong></small><br />
                                                                            <small><strong><asp:Label runat="server" id="Label24" Text='<%# String.Format("{0:n2}",DataBinder.Eval(Container.DataItem, "NUM_VALOR_FINAL_4X"))%>'></asp:Label></strong></small>
                                                                        </td>
                                                                        <td class="text-center" style="min-width:90px;" >
                                                                            <small><strong><asp:Label runat="server" id="Label25" Text='<%# String.Format("{0:n2}",DataBinder.Eval(Container.DataItem, "NUM_VALOR_FIXO_CREDITO_5X"))%>'></asp:Label></strong></small><br />
                                                                            <small><strong><asp:Label runat="server" id="Label26" Text='<%# String.Format("{0:n2}",DataBinder.Eval(Container.DataItem, "NUM_VALOR_FINAL_5X"))%>'></asp:Label></strong></small>
                                                                        </td>
                                                                        <td class="text-center" style="min-width:90px;" >
                                                                            <small><strong><asp:Label runat="server" id="Label27" Text='<%# String.Format("{0:n2}",DataBinder.Eval(Container.DataItem, "NUM_VALOR_FIXO_CREDITO_6X"))%>'></asp:Label></strong></small><br />
                                                                            <small><strong><asp:Label runat="server" id="Label28" Text='<%# String.Format("{0:n2}",DataBinder.Eval(Container.DataItem, "NUM_VALOR_FINAL_6X"))%>'></asp:Label></strong></small>
                                                                        </td>
                                                                        <td class="text-center" style="min-width:90px;" >
                                                                            <small><strong><asp:Label runat="server" id="Label29" Text='<%# String.Format("{0:n2}",DataBinder.Eval(Container.DataItem, "NUM_VALOR_FIXO_CREDITO_7X"))%>'></asp:Label></strong></small><br />
                                                                            <small><strong><asp:Label runat="server" id="Label30" Text='<%# String.Format("{0:n2}",DataBinder.Eval(Container.DataItem, "NUM_VALOR_FINAL_7X"))%>'></asp:Label></strong></small>
                                                                        </td>
                                                                        <td class="text-center" style="min-width:90px;" >
                                                                            <small><strong><asp:Label runat="server" id="Label31" Text='<%# String.Format("{0:n2}",DataBinder.Eval(Container.DataItem, "NUM_VALOR_FIXO_CREDITO_8X"))%>'></asp:Label></strong></small><br />
                                                                            <small><strong><asp:Label runat="server" id="Label32" Text='<%# String.Format("{0:n2}",DataBinder.Eval(Container.DataItem, "NUM_VALOR_FINAL_8X"))%>'></asp:Label></strong></small>
                                                                        </td>
                                                                        <td class="text-center" style="min-width:90px;" >
                                                                            <small><strong><asp:Label runat="server" id="Label33" Text='<%# String.Format("{0:n2}",DataBinder.Eval(Container.DataItem, "NUM_VALOR_FIXO_CREDITO_9X"))%>'></asp:Label></strong></small><br />
                                                                            <small><strong><asp:Label runat="server" id="Label34" Text='<%# String.Format("{0:n2}",DataBinder.Eval(Container.DataItem, "NUM_VALOR_FINAL_9X"))%>'></asp:Label></strong></small>
                                                                        </td>
                                                                        <td class="text-center" style="min-width:90px;" >
                                                                            <small><strong><asp:Label runat="server" id="Label35" Text='<%# String.Format("{0:n2}",DataBinder.Eval(Container.DataItem, "NUM_VALOR_FIXO_CREDITO_10X"))%>'></asp:Label></strong></small><br />
                                                                            <small><strong><asp:Label runat="server" id="Label36" Text='<%# String.Format("{0:n2}",DataBinder.Eval(Container.DataItem, "NUM_VALOR_FINAL_10X"))%>'></asp:Label></strong></small>
                                                                        </td>
                                                                        <td class="text-center" style="min-width:90px;" >
                                                                            <small><strong><asp:Label runat="server" id="Label37" Text='<%# String.Format("{0:n2}",DataBinder.Eval(Container.DataItem, "NUM_VALOR_FIXO_CREDITO_11X"))%>'></asp:Label></strong></small><br />
                                                                            <small><strong><asp:Label runat="server" id="Label38" Text='<%# String.Format("{0:n2}",DataBinder.Eval(Container.DataItem, "NUM_VALOR_FINAL_11X"))%>'></asp:Label></strong></small>
                                                                        </td>
                                                                        <td class="text-center" style="min-width:90px;" >
                                                                            <small><strong><asp:Label runat="server" id="Label39" Text='<%# String.Format("{0:n2}",DataBinder.Eval(Container.DataItem, "NUM_VALOR_FIXO_CREDITO_12X"))%>'></asp:Label></strong></small><br />
                                                                            <small><strong><asp:Label runat="server" id="Label40" Text='<%# String.Format("{0:n2}",DataBinder.Eval(Container.DataItem, "NUM_VALOR_FINAL_12X"))%>'></asp:Label></strong></small>
                                                                        </td>
                                                                        <td class="text-center" style="min-width:90px;" >
                                                                            <small><strong><asp:Label runat="server" id="Label41" Text='<%# String.Format("{0:n2}",DataBinder.Eval(Container.DataItem, "NUM_VALOR_FIXO_CREDITO_13X"))%>'></asp:Label></strong></small><br />
                                                                            <small><strong><asp:Label runat="server" id="Label42" Text='<%# String.Format("{0:n2}",DataBinder.Eval(Container.DataItem, "NUM_VALOR_FINAL_13X"))%>'></asp:Label></strong></small>
                                                                        </td>
                                                                        <td class="text-center" style="min-width:90px;" >
                                                                            <small><strong><asp:Label runat="server" id="Label43" Text='<%# String.Format("{0:n2}",DataBinder.Eval(Container.DataItem, "NUM_VALOR_FIXO_CREDITO_14X"))%>'></asp:Label></strong></small><br />
                                                                            <small><strong><asp:Label runat="server" id="Label44" Text='<%# String.Format("{0:n2}",DataBinder.Eval(Container.DataItem, "NUM_VALOR_FINAL_14X"))%>'></asp:Label></strong></small>
                                                                        </td>
                                                                        <td class="text-center" style="min-width:90px;" >
                                                                            <small><strong><asp:Label runat="server" id="Label45" Text='<%# String.Format("{0:n2}",DataBinder.Eval(Container.DataItem, "NUM_VALOR_FIXO_CREDITO_15X"))%>'></asp:Label></strong></small><br />
                                                                            <small><strong><asp:Label runat="server" id="Label46" Text='<%# String.Format("{0:n2}",DataBinder.Eval(Container.DataItem, "NUM_VALOR_FINAL_15X"))%>'></asp:Label></strong></small>
                                                                        </td>
                                                                        <td class="text-center" style="min-width:90px;" >
                                                                            <small><strong><asp:Label runat="server" id="Label47" Text='<%# String.Format("{0:n2}",DataBinder.Eval(Container.DataItem, "NUM_VALOR_FIXO_CREDITO_16X"))%>'></asp:Label></strong></small><br />
                                                                            <small><strong><asp:Label runat="server" id="Label48" Text='<%# String.Format("{0:n2}",DataBinder.Eval(Container.DataItem, "NUM_VALOR_FINAL_16X"))%>'></asp:Label></strong></small>
                                                                        </td>
                                                                        <td class="text-center" style="min-width:90px;" >
                                                                            <small><strong><asp:Label runat="server" id="Label49" Text='<%# String.Format("{0:n2}",DataBinder.Eval(Container.DataItem, "NUM_VALOR_FIXO_CREDITO_17X"))%>'></asp:Label></strong></small><br />
                                                                            <small><strong><asp:Label runat="server" id="Label50" Text='<%# String.Format("{0:n2}",DataBinder.Eval(Container.DataItem, "NUM_VALOR_FINAL_17X"))%>'></asp:Label></strong></small>
                                                                        </td>
                                                                        <td class="text-center" style="min-width:90px;" >
                                                                            <small><strong><asp:Label runat="server" id="Label51" Text='<%# String.Format("{0:n2}",DataBinder.Eval(Container.DataItem, "NUM_VALOR_FIXO_CREDITO_18X"))%>'></asp:Label></strong></small><br />
                                                                            <small><strong><asp:Label runat="server" id="Label52" Text='<%# String.Format("{0:n2}",DataBinder.Eval(Container.DataItem, "NUM_VALOR_FINAL_18X"))%>'></asp:Label></strong></small>
                                                                        </td>

                                                                    </tr>
                                                                </ItemTemplate>
                                                            </asp:Repeater>

                                                            </tbody>
                                                        </table>
                                                        </div>
                                                    </div>

                                            </div>
                                            <div class="card-footer">
                                            </div>
                                        </div>
                                        </div>
                                    </div>

                                    <!-- Fim tabela de parcelas **************************************************************************************************** -->



                                    </div>

                                    <div class="tab-pane fade" id="conteudo_online" role="tabpanel" aria-labelledby="conteudo_online_tab">

                                    <!-- Fim da Tabela de taxas **************************************************************************************************** -->

                                    <!-- Divisão da Tabela de parcelas **************************************************************************************************** -->
                                    <div class="row" runat="server" id="dvParcelasOnline" visible="false">
                                        <div class="col-12">
                                        <div class="card">

                                            <div class="card-header bg-whitelabel1">
                                            <h3 class="card-title">TAXAS</h3>
                                                <div class="card-tools">
                                                    <button type="button" class="btn btn-tool" data-card-widget="collapse">
                                                    <i class="fas fa-minus"></i>
                                                    </button>
                                                </div>

                                            </div>
                                            <div class="card-body" style="overflow:scroll; height:800px;">

                                                <div class="row">
                                                    <div class="col-12">
                                                        <table id="Table1Online" class="table table-bordered table-hover table-striped table-responsive-md">
                                                            <thead>
                                                            <tr>
                                                                <th><h6><small><b>Bandeira</b></small></h6></th>
                                                                <th><h6><small><b>Débito</b></small></h6></th>
                                                                <th><h6><small><b>Cred.À Vista</b></small></h6></th>
                                                                <th><h6><small><b>Cred.2x</b></small></h6></th>
                                                                <th><h6><small><b>Cred.3x</b></small></h6></th>
                                                                <th><h6><small><b>Cred.4x</b></small></h6></th>
                                                                <th><h6><small><b>Cred.5x</b></small></h6></th>
                                                                <th><h6><small><b>Cred.6x</b></small></h6></th>
                                                                <th><h6><small><b>Cred.7x</b></small></h6></th>
                                                                <th><h6><small><b>Cred.8x</b></small></h6></th>
                                                                <th><h6><small><b>Cred.9x</b></small></h6></th>
                                                                <th><h6><small><b>Cred.10x</b></small></h6></th>
                                                                <th><h6><small><b>Cred.11x</b></small></h6></th>
                                                                <th><h6><small><b>Cred.12x</b></small></h6></th>
                                                                <th><h6><small><b>Cred.13x</b></small></h6></th>
                                                                <th><h6><small><b>Cred.14x</b></small></h6></th>
                                                                <th><h6><small><b>Cred.15x</b></small></h6></th>
                                                                <th><h6><small><b>Cred.16x</b></small></h6></th>
                                                                <th><h6><small><b>Cred.17x</b></small></h6></th>
                                                                <th><h6><small><b>Cred.18x</b></small></h6></th>
                                                            </tr>
                                                            </thead>
                                                            <tbody>
                                                            <asp:Repeater runat="server" ID="rptConsultaParcelasOnline" >
                                                                <ItemTemplate>

                                                                    <tr>
                                                                        <td class="text-center">
                                                                        <asp:TextBox runat="server" id="txtidplano" Text='<%# DataBinder.Eval(Container.DataItem, "COD_ID_PLANOS")%>' visible="false"></asp:TextBox>
                                                                        <asp:TextBox runat="server" id="txtidbandeira" Text='<%# DataBinder.Eval(Container.DataItem, "COD_ID_BANDEIRAS")%>' visible="false"></asp:TextBox>
                                                                            <small><strong>Vlr.Oper.(R$)</strong></small><br />
                                                                            <small><strong>Taxa (%)</strong></small>
                                                                            <small><strong><%# DataBinder.Eval(Container.DataItem, "NOM_BANDEIRA")%></strong></small><br />
                                                                            <asp:Image runat="server" id="imgBandeira" CssClass="img-fluid img-thumbnail mx-auto" style="max-width:50px;" ImageUrl='<%# DataBinder.Eval(Container.DataItem, "NOM_IMAGEM")%>'></asp:Image><br /><br />

                                                                        </td>

                                                                        <td class="text-center" style="min-width:90px;" >
                                                                            <small><strong><asp:Label runat="server" id="Label19" Text='<%# String.Format("{0:n2}",DataBinder.Eval(Container.DataItem, "NUM_VALOR_FIXO_DEBITO"))%>'></asp:Label></strong></small><br />
                                                                            <small><strong><asp:Label runat="server" id="lblfinaldebito" Text='<%# String.Format("{0:n2}",DataBinder.Eval(Container.DataItem, "NUM_VALOR_FINAL_DEBITO"))%>'></asp:Label></strong></small>
                                                                        </td>
                                                                        <td class="text-center" style="min-width:90px;" >
                                                                            <small><strong><asp:Label runat="server" id="Label20" Text='<%# String.Format("{0:n2}",DataBinder.Eval(Container.DataItem, "NUM_VALOR_FIXO_CREDITO_VISTA"))%>'></asp:Label></strong></small><br />
                                                                            <small><strong><asp:Label runat="server" id="Label1" Text='<%# String.Format("{0:n2}",DataBinder.Eval(Container.DataItem, "NUM_VALOR_FINAL_VISTA"))%>'></asp:Label></strong></small>
                                                                        </td>
                                                                        <td class="text-center" style="min-width:90px;" >
                                                                            <small><strong><asp:Label runat="server" id="Label21" Text='<%# String.Format("{0:n2}",DataBinder.Eval(Container.DataItem, "NUM_VALOR_FIXO_CREDITO_2X"))%>'></asp:Label></strong></small><br />
                                                                            <small><strong><asp:Label runat="server" id="Label2" Text='<%# String.Format("{0:n2}",DataBinder.Eval(Container.DataItem, "NUM_VALOR_FINAL_2X"))%>'></asp:Label></strong></small>
                                                                        </td>
                                                                        <td class="text-center" style="min-width:90px;" >
                                                                            <small><strong><asp:Label runat="server" id="Label22" Text='<%# String.Format("{0:n2}",DataBinder.Eval(Container.DataItem, "NUM_VALOR_FIXO_CREDITO_3X"))%>'></asp:Label></strong></small><br />
                                                                            <small><strong><asp:Label runat="server" id="Label3" Text='<%# String.Format("{0:n2}",DataBinder.Eval(Container.DataItem, "NUM_VALOR_FINAL_3X"))%>'></asp:Label></strong></small>
                                                                        </td>
                                                                        <td class="text-center" style="min-width:90px;" >
                                                                            <small><strong><asp:Label runat="server" id="Label23" Text='<%# String.Format("{0:n2}",DataBinder.Eval(Container.DataItem, "NUM_VALOR_FIXO_CREDITO_4X"))%>'></asp:Label></strong></small><br />
                                                                            <small><strong><asp:Label runat="server" id="Label24" Text='<%# String.Format("{0:n2}",DataBinder.Eval(Container.DataItem, "NUM_VALOR_FINAL_4X"))%>'></asp:Label></strong></small>
                                                                        </td>
                                                                        <td class="text-center" style="min-width:90px;" >
                                                                            <small><strong><asp:Label runat="server" id="Label25" Text='<%# String.Format("{0:n2}",DataBinder.Eval(Container.DataItem, "NUM_VALOR_FIXO_CREDITO_5X"))%>'></asp:Label></strong></small><br />
                                                                            <small><strong><asp:Label runat="server" id="Label26" Text='<%# String.Format("{0:n2}",DataBinder.Eval(Container.DataItem, "NUM_VALOR_FINAL_5X"))%>'></asp:Label></strong></small>
                                                                        </td>
                                                                        <td class="text-center" style="min-width:90px;" >
                                                                            <small><strong><asp:Label runat="server" id="Label27" Text='<%# String.Format("{0:n2}",DataBinder.Eval(Container.DataItem, "NUM_VALOR_FIXO_CREDITO_6X"))%>'></asp:Label></strong></small><br />
                                                                            <small><strong><asp:Label runat="server" id="Label28" Text='<%# String.Format("{0:n2}",DataBinder.Eval(Container.DataItem, "NUM_VALOR_FINAL_6X"))%>'></asp:Label></strong></small>
                                                                        </td>
                                                                        <td class="text-center" style="min-width:90px;" >
                                                                            <small><strong><asp:Label runat="server" id="Label29" Text='<%# String.Format("{0:n2}",DataBinder.Eval(Container.DataItem, "NUM_VALOR_FIXO_CREDITO_7X"))%>'></asp:Label></strong></small><br />
                                                                            <small><strong><asp:Label runat="server" id="Label30" Text='<%# String.Format("{0:n2}",DataBinder.Eval(Container.DataItem, "NUM_VALOR_FINAL_7X"))%>'></asp:Label></strong></small>
                                                                        </td>
                                                                        <td class="text-center" style="min-width:90px;" >
                                                                            <small><strong><asp:Label runat="server" id="Label31" Text='<%# String.Format("{0:n2}",DataBinder.Eval(Container.DataItem, "NUM_VALOR_FIXO_CREDITO_8X"))%>'></asp:Label></strong></small><br />
                                                                            <small><strong><asp:Label runat="server" id="Label32" Text='<%# String.Format("{0:n2}",DataBinder.Eval(Container.DataItem, "NUM_VALOR_FINAL_8X"))%>'></asp:Label></strong></small>
                                                                        </td>
                                                                        <td class="text-center" style="min-width:90px;" >
                                                                            <small><strong><asp:Label runat="server" id="Label33" Text='<%# String.Format("{0:n2}",DataBinder.Eval(Container.DataItem, "NUM_VALOR_FIXO_CREDITO_9X"))%>'></asp:Label></strong></small><br />
                                                                            <small><strong><asp:Label runat="server" id="Label34" Text='<%# String.Format("{0:n2}",DataBinder.Eval(Container.DataItem, "NUM_VALOR_FINAL_9X"))%>'></asp:Label></strong></small>
                                                                        </td>
                                                                        <td class="text-center" style="min-width:90px;" >
                                                                            <small><strong><asp:Label runat="server" id="Label35" Text='<%# String.Format("{0:n2}",DataBinder.Eval(Container.DataItem, "NUM_VALOR_FIXO_CREDITO_10X"))%>'></asp:Label></strong></small><br />
                                                                            <small><strong><asp:Label runat="server" id="Label36" Text='<%# String.Format("{0:n2}",DataBinder.Eval(Container.DataItem, "NUM_VALOR_FINAL_10X"))%>'></asp:Label></strong></small>
                                                                        </td>
                                                                        <td class="text-center" style="min-width:90px;" >
                                                                            <small><strong><asp:Label runat="server" id="Label37" Text='<%# String.Format("{0:n2}",DataBinder.Eval(Container.DataItem, "NUM_VALOR_FIXO_CREDITO_11X"))%>'></asp:Label></strong></small><br />
                                                                            <small><strong><asp:Label runat="server" id="Label38" Text='<%# String.Format("{0:n2}",DataBinder.Eval(Container.DataItem, "NUM_VALOR_FINAL_11X"))%>'></asp:Label></strong></small>
                                                                        </td>
                                                                        <td class="text-center" style="min-width:90px;" >
                                                                            <small><strong><asp:Label runat="server" id="Label39" Text='<%# String.Format("{0:n2}",DataBinder.Eval(Container.DataItem, "NUM_VALOR_FIXO_CREDITO_12X"))%>'></asp:Label></strong></small><br />
                                                                            <small><strong><asp:Label runat="server" id="Label40" Text='<%# String.Format("{0:n2}",DataBinder.Eval(Container.DataItem, "NUM_VALOR_FINAL_12X"))%>'></asp:Label></strong></small>
                                                                        </td>
                                                                        <td class="text-center" style="min-width:90px;" >
                                                                            <small><strong><asp:Label runat="server" id="Label41" Text='<%# String.Format("{0:n2}",DataBinder.Eval(Container.DataItem, "NUM_VALOR_FIXO_CREDITO_13X"))%>'></asp:Label></strong></small><br />
                                                                            <small><strong><asp:Label runat="server" id="Label42" Text='<%# String.Format("{0:n2}",DataBinder.Eval(Container.DataItem, "NUM_VALOR_FINAL_13X"))%>'></asp:Label></strong></small>
                                                                        </td>
                                                                        <td class="text-center" style="min-width:90px;" >
                                                                            <small><strong><asp:Label runat="server" id="Label43" Text='<%# String.Format("{0:n2}",DataBinder.Eval(Container.DataItem, "NUM_VALOR_FIXO_CREDITO_14X"))%>'></asp:Label></strong></small><br />
                                                                            <small><strong><asp:Label runat="server" id="Label44" Text='<%# String.Format("{0:n2}",DataBinder.Eval(Container.DataItem, "NUM_VALOR_FINAL_14X"))%>'></asp:Label></strong></small>
                                                                        </td>
                                                                        <td class="text-center" style="min-width:90px;" >
                                                                            <small><strong><asp:Label runat="server" id="Label45" Text='<%# String.Format("{0:n2}",DataBinder.Eval(Container.DataItem, "NUM_VALOR_FIXO_CREDITO_15X"))%>'></asp:Label></strong></small><br />
                                                                            <small><strong><asp:Label runat="server" id="Label46" Text='<%# String.Format("{0:n2}",DataBinder.Eval(Container.DataItem, "NUM_VALOR_FINAL_15X"))%>'></asp:Label></strong></small>
                                                                        </td>
                                                                        <td class="text-center" style="min-width:90px;" >
                                                                            <small><strong><asp:Label runat="server" id="Label47" Text='<%# String.Format("{0:n2}",DataBinder.Eval(Container.DataItem, "NUM_VALOR_FIXO_CREDITO_16X"))%>'></asp:Label></strong></small><br />
                                                                            <small><strong><asp:Label runat="server" id="Label48" Text='<%# String.Format("{0:n2}",DataBinder.Eval(Container.DataItem, "NUM_VALOR_FINAL_16X"))%>'></asp:Label></strong></small>
                                                                        </td>
                                                                        <td class="text-center" style="min-width:90px;" >
                                                                            <small><strong><asp:Label runat="server" id="Label49" Text='<%# String.Format("{0:n2}",DataBinder.Eval(Container.DataItem, "NUM_VALOR_FIXO_CREDITO_17X"))%>'></asp:Label></strong></small><br />
                                                                            <small><strong><asp:Label runat="server" id="Label50" Text='<%# String.Format("{0:n2}",DataBinder.Eval(Container.DataItem, "NUM_VALOR_FINAL_17X"))%>'></asp:Label></strong></small>
                                                                        </td>
                                                                        <td class="text-center" style="min-width:90px;" >
                                                                            <small><strong><asp:Label runat="server" id="Label51" Text='<%# String.Format("{0:n2}",DataBinder.Eval(Container.DataItem, "NUM_VALOR_FIXO_CREDITO_18X"))%>'></asp:Label></strong></small><br />
                                                                            <small><strong><asp:Label runat="server" id="Label52" Text='<%# String.Format("{0:n2}",DataBinder.Eval(Container.DataItem, "NUM_VALOR_FINAL_18X"))%>'></asp:Label></strong></small>
                                                                        </td>
                                                                    </tr>
                                                                </ItemTemplate>
                                                            </asp:Repeater>

                                                            </tbody>
                                                        </table>
                                                        </div>
                                                    </div>

                                            </div>
                                            <div class="card-footer">
                                            </div>
                                        </div>
                                        </div>
                                    </div>

                                    <!-- Fim tabela de parcelas **************************************************************************************************** -->



                                    </div>

                                </div>



                              </div>

                              <div class="card-footer" style="overflow:auto; width: 100%; ">
                                <asp:Button runat="server" ID="btnAtualizar" CssClass="btn btn-whitelabel1" Text="Clique aqui para atualizar as taxas da conta digital" onclick="btnAtualizar_Click" />
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

<script type="text/javascript" src="../plugins/bootstrap-colorpicker/js/bootstrap-colorpicker.min.js"></script>

<script>
    $(function () {

        //Colorpicker
        $('.my-colorpicker1').colorpicker()

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


<script type="text/javascript">

$(document).ready(function () {
    var table = $('#tblConsulta').DataTable({
      order: [[0, 'desc']],
      "paging": true,
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

<script type="text/javascript">
    function myFunctionChavePix() {
        var copyText = document.getElementById("txtChavePix");
        copyText.select();
        copyText.setSelectionRange(0, 99999); // For mobile devices
        navigator.clipboard.writeText(copyText.value);
        alert("Chave Pix: " + copyText.value + " copiada com sucesso!");
    }

</script>


</body>
</html>
