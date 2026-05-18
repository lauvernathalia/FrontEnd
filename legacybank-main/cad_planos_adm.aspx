<%@ Page Language="C#" AutoEventWireup="true" CodeFile="cad_planos_adm.aspx.cs" Inherits="cad_planos_adm" MaintainScrollPositionOnPostback="true" %>

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
                              <li class="breadcrumb-item"><a href="#">Cadastro</a></li>
                              <li class="breadcrumb-item active">Planos</li>
                            </ol>
                          </div>
                        </div>

                        <div class="row">
                          <div class="col-12">
                            <div class="card">

                              <div class="card-header bg-whitelabel1">
                                <h3 class="card-title">PLANOS</h3>
                                    <div class="card-tools">
                                      <button type="button" class="btn btn-tool" data-card-widget="collapse">
                                        <i class="fas fa-minus"></i>
                                      </button>
                                    </div>
                              </div>


                              <div class="card-body">
                                <div class="form-group row col-12">
                                    <div class="col-sm-3">
                                        <label class="col-sm-12 col-form-label">ID</label>
                                        <asp:Label id="lblID" runat="server" class="form-control"></asp:Label>
                                    </div>
                                    <div class="col-sm-3">
                                        <label class="col-sm-12 col-form-label">Adquirente</label>
                                        <asp:DropDownList runat="server" id="ddlModelo" CssClass="form-control" 
                                            AutoPostBack="True" onselectedindexchanged="ddlModelo_SelectedIndexChanged">
                                            <asp:ListItem Value="P">PagSeguro</asp:ListItem>
                                            <asp:ListItem Value="Z">Zoop</asp:ListItem>
                                            <asp:ListItem Value="B">BaaS</asp:ListItem>
                                        </asp:DropDownList>
                                    </div>

                                    <div class="col-sm-3">
                                        <label class="col-sm-12 col-form-label">Plano Referência</label>
                                        <asp:DropDownList runat="server" id="ddlPlanosReferencia" CssClass="form-control">
                                        </asp:DropDownList>
                                    </div>

                                    <div class="col-sm-3">
                                        <label class="col-sm-12 col-form-label">Nome</label>
                                        <asp:TextBox id="txtNome" runat="server" class="form-control" placeholder="Nome do Plano"></asp:TextBox>
                                    </div>


                                </div>

                                <div class="form-group row col-12">
                                    <div class="col-sm-12">
                                        <label class="col-sm-2 col-form-label">Descrição</label>
                                        <asp:TextBox id="txtDescricao"  runat="server" class="form-control" 
                                            placeholder="Descrição do Plano"></asp:TextBox>
                                    </div>
                                </div>

                                <div class="form-group row col-12">
                                    <div class="col-sm-3">
                                        <label class="col-sm-12 col-form-label">Tipo</label>
                                        <asp:DropDownList runat="server" id="ddlTipo" CssClass="form-control">
                                            <asp:ListItem Value="B">Plano Base</asp:ListItem>
                                            <asp:ListItem Value="C">Plano Comercial</asp:ListItem>
                                        </asp:DropDownList>
                                    </div>
                                    <div class="col-sm-3">
                                        <label class="col-sm-12 col-form-label">Visível</label>
                                        <asp:DropDownList runat="server" id="ddlVisivel" CssClass="form-control">
                                            <asp:ListItem Value="S">Sim</asp:ListItem>
                                            <asp:ListItem Value="N">Não</asp:ListItem>
                                        </asp:DropDownList>
                                    </div>
                                    <div class="col-sm-3">
                                        <label class="col-sm-12 col-form-label">Antecipado</label>
                                        <asp:DropDownList runat="server" id="ddlAntecipado" CssClass="form-control" 
                                            AutoPostBack="True" onselectedindexchanged="ddlAntecipado_SelectedIndexChanged">
                                            <asp:ListItem Value="S">Sim</asp:ListItem>
                                            <asp:ListItem Value="N">Não</asp:ListItem>
                                        </asp:DropDownList>
                                    </div>
                                    <div class="col-sm-3">
                                        <label class="col-sm-12 col-form-label">Ativo</label>
                                        <asp:DropDownList runat="server" id="ddlAtivo" CssClass="form-control">
                                            <asp:ListItem Value="S">Sim</asp:ListItem>
                                            <asp:ListItem Value="N">Não</asp:ListItem>
                                        </asp:DropDownList>
                                    </div>
                                </div>
                                <div class="form-group row col-12">
                                    <div class="col-sm-4" style="display:none;">
                                        <label class="col-sm-12 col-form-label">Plano Referência</label>
                                        <asp:DropDownList runat="server" id="ddlPlanos" CssClass="form-control">
                                        </asp:DropDownList>
                                    </div>
                                    <div class="col-sm-6">
                                        <label class="col-sm-12 col-form-label">Dias Liquidação</label>
                                        <asp:TextBox id="txtDias" runat="server" class="form-control" placeholder="No. Dias para Liquidação" Visible="false"></asp:TextBox>
                                        <asp:DropDownList runat="server" id="ddlDias" CssClass="form-control">
                                            <asp:ListItem Value="0">D+0</asp:ListItem>
                                            <asp:ListItem Value="1">D+1</asp:ListItem>
                                            <asp:ListItem Value="2">D+2</asp:ListItem>
                                            <asp:ListItem Value="7">D+7</asp:ListItem>
                                            <asp:ListItem Value="14">D+14</asp:ListItem>
                                            <asp:ListItem Value="30">D+30</asp:ListItem>
                                        </asp:DropDownList>



                                    </div>
                                    <div class="col-sm-6">
                                        <label class="col-sm-12 col-form-label">Taxa Antecipação (%)</label>
                                        <asp:TextBox id="txtTaxaAntecipacao" runat="server" class="form-control" placeholder="Percentual da Taxa de Antecipação"></asp:TextBox>
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

                            <!-- Divisão da tabela de taxas -->

                            <div class="row" runat="server" id="dvTaxas" visible="false">
                                <div class="col-12">
                                <div class="card">

                                    <div class="card-header bg-whitelabel1">
                                    <h3 class="card-title">TAXAS PADRÕES</h3>
                                        <div class="card-tools">
                                          <button type="button" class="btn btn-tool" data-card-widget="collapse">
                                            <i class="fas fa-minus"></i>
                                          </button>
                                        </div>

                                    </div>
                                    <div class="card-body" style="overflow:scroll; height:500px;">

                                        <div class="row">
                                            <div class="col-12">
                                                <table id="tabConsulta" class="table table-bordered table-hover table-striped">
                                                    <thead>
                                                    <tr>
                                                      <th>Ordem</th>
                                                      <th>Bandeira</th>
                                                      <th>Tipo</th>
                                                      <th>Taxa (%)</th>
                                                      <th>Markup (%)</th>
                                                      <th>Taxa Final (%)</th>
                                                    </tr>
                                                    </thead>
                                                    <tbody>
                                                    <asp:Repeater runat="server" ID="rptConsulta" >
                                                        <ItemTemplate>

                                                            <tr>
                                                              <td>
                                                                <asp:TextBox runat="server" id="txtidplano" Text='<%# DataBinder.Eval(Container.DataItem, "COD_ID_PLANOS")%>' visible="false"></asp:TextBox>
                                                                <asp:TextBox runat="server" id="txtidbandeira" Text='<%# DataBinder.Eval(Container.DataItem, "COD_ID_BANDEIRAS")%>' visible="false"></asp:TextBox>
                                                                <asp:TextBox runat="server" id="txtidtabela" Text='<%# DataBinder.Eval(Container.DataItem, "COD_ID_PLANOS_TABELA")%>' visible="false"></asp:TextBox>
                                                                  <small><strong><%# DataBinder.Eval(Container.DataItem, "NUM_ORDEM_BANDEIRAS")%></strong></small>
                                                              </td>

                                                              <td>
                                                                  <small><strong><%# DataBinder.Eval(Container.DataItem, "NOM_BANDEIRA")%></strong></small>
                                                              </td>

                                                              <td>
                                                                  <small><strong><%# DataBinder.Eval(Container.DataItem, "NOM_TABELA")%></strong></small>
                                                              </td>
                                                              <td>
                                                                  <small><asp:TextBox runat="server" id="txtTaxa" Text='<%# String.Format("{0:n2}",DataBinder.Eval(Container.DataItem, "NUM_TAXA"))%>' class="form-control"></asp:TextBox></small>
                                                              </td>
                                                              <td>
                                                                  <small><asp:TextBox runat="server" id="txtMarkup" Text='<%# String.Format("{0:n2}",DataBinder.Eval(Container.DataItem, "NUM_MARKUP"))%>' class="form-control"></asp:TextBox></small>
                                                              </td>
                                                              <td>
                                                                  <strong><%# String.Format("{0:n2}",DataBinder.Eval(Container.DataItem, "NUM_TAXA_FINAL"))%></strong>
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
                                        <asp:Button runat="server" ID="btnGravar" CssClass="btn btn-whitelabel1" Text="Gravar" onclick="btnGravar_Click"/>
                                        <asp:Button runat="server" ID="btnRecalcular" CssClass="btn btn-whitelabel1 float-right" Text="Recalcular" onclick="btnRecalcular_Click"/>
                                    </div>
                                </div>
                                </div>
                            </div>

                            <!-- Fim da Tabela de taxas **************************************************************************************************** -->

                            <!-- Divisão da Tabela de parcelas **************************************************************************************************** -->
                            <div class="row" runat="server" id="dvParcelas" visible="false">
                                <div class="col-12">
                                <div class="card">

                                    <div class="card-header bg-whitelabel1">
                                    <h3 class="card-title">TAXAS ANTECIPAÇÃO</h3>
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
                                                      <th><h6><small><b>Crédito À Vista</b></small></h6></th>
                                                      <th><h6><small><b>Crédito 2x</b></small></h6></th>
                                                      <th><h6><small><b>Crédito 3x</b></small></h6></th>
                                                      <th><h6><small><b>Crédito 4x</b></small></h6></th>
                                                      <th><h6><small><b>Crédito 5x</b></small></h6></th>
                                                      <th><h6><small><b>Crédito 6x</b></small></h6></th>
                                                      <th><h6><small><b>Crédito 7x</b></small></h6></th>
                                                      <th><h6><small><b>Crédito 8x</b></small></h6></th>
                                                      <th><h6><small><b>Crédito 9x</b></small></h6></th>
                                                      <th><h6><small><b>Crédito 10x</b></small></h6></th>
                                                      <th><h6><small><b>Crédito 11x</b></small></h6></th>
                                                      <th><h6><small><b>Crédito 12x</b></small></h6></th>
                                                      <th><h6><small><b>Crédito 13x</b></small></h6></th>
                                                      <th><h6><small><b>Crédito 14x</b></small></h6></th>
                                                      <th><h6><small><b>Crédito 15x</b></small></h6></th>
                                                      <th><h6><small><b>Crédito 16x</b></small></h6></th>
                                                      <th><h6><small><b>Crédito 17x</b></small></h6></th>
                                                      <th><h6><small><b>Crédito 18x</b></small></h6></th>
                                                    </tr>
                                                    </thead>
                                                    <tbody>
                                                    <asp:Repeater runat="server" ID="rptConsultaParcelas" >
                                                        <ItemTemplate>

                                                            <tr>
                                                              <td class="text-center">
                                                                <asp:TextBox runat="server" id="txtidplano" Text='<%# DataBinder.Eval(Container.DataItem, "COD_ID_PLANOS")%>' visible="false"></asp:TextBox>
                                                                <asp:TextBox runat="server" id="txtidbandeira" Text='<%# DataBinder.Eval(Container.DataItem, "COD_ID_BANDEIRAS")%>' visible="false"></asp:TextBox>
                                                                  <small><strong><%# DataBinder.Eval(Container.DataItem, "NOM_BANDEIRA")%></strong></small><br />
                                                                  <asp:Image runat="server" id="imgBandeira" CssClass="img-fluid img-thumbnail mx-auto" style="max-width:70px;" ImageUrl='<%# DataBinder.Eval(Container.DataItem, "NOM_IMAGEM")%>'></asp:Image><br />
                                                                  <small><strong>Reb/Mkt</strong></small>
                                                              </td>

                                                              <td>
                                                                  <small><strong><asp:TextBox runat="server" CssClass="form-control" id="txtdebito" Text='<%# String.Format("{0:n2}",DataBinder.Eval(Container.DataItem, "NUM_VALOR_DEBITO"))%>'></asp:TextBox></strong></small><br />
                                                                  <small><strong><asp:TextBox runat="server" CssClass="form-control" id="txtmarkupdebito" Text='<%# String.Format("{0:n2}",DataBinder.Eval(Container.DataItem, "NUM_VALOR_MARKUP_DEBITO"))%>'></asp:TextBox></strong></small>
                                                              </td>
                                                              <td>
                                                                  <small><strong><asp:TextBox runat="server" CssClass="form-control" id="txtcredito" Text='<%# String.Format("{0:n2}",DataBinder.Eval(Container.DataItem, "NUM_VALOR_CREDITO_VISTA"))%>'></asp:TextBox></strong></small><br />
                                                                  <small><strong><asp:TextBox runat="server" CssClass="form-control" id="txtmarkupcredito" Text='<%# String.Format("{0:n2}",DataBinder.Eval(Container.DataItem, "NUM_VALOR_MARKUP_VISTA"))%>'></asp:TextBox></strong></small>
                                                              </td>
                                                              <td>
                                                                  <small><strong><asp:TextBox runat="server" CssClass="form-control" id="txtcredito2x" Text='<%# String.Format("{0:n2}",DataBinder.Eval(Container.DataItem, "NUM_VALOR_CREDITO_2X"))%>'></asp:TextBox></strong></small><br />
                                                                  <small><strong><asp:TextBox runat="server" CssClass="form-control" id="txtmarkup2x" Text='<%# String.Format("{0:n2}",DataBinder.Eval(Container.DataItem, "NUM_VALOR_MARKUP_2X"))%>'></asp:TextBox></strong></small>
                                                              </td>
                                                              <td>
                                                                  <small><strong><asp:TextBox runat="server" CssClass="form-control" id="txtcredito3x" Text='<%# String.Format("{0:n2}",DataBinder.Eval(Container.DataItem, "NUM_VALOR_CREDITO_3X"))%>'></asp:TextBox></strong></small><br />
                                                                  <small><strong><asp:TextBox runat="server" CssClass="form-control" id="txtmarkup3x" Text='<%# String.Format("{0:n2}",DataBinder.Eval(Container.DataItem, "NUM_VALOR_MARKUP_3X"))%>'></asp:TextBox></strong></small>
                                                              </td>
                                                              <td>
                                                                  <small><strong><asp:TextBox runat="server" class="form-control" id="txtcredito4x" Text='<%# String.Format("{0:n2}",DataBinder.Eval(Container.DataItem, "NUM_VALOR_CREDITO_4X"))%>'></asp:TextBox></strong></small><br />
                                                                  <small><strong><asp:TextBox runat="server" class="form-control" id="txtmarkup4x" Text='<%# String.Format("{0:n2}",DataBinder.Eval(Container.DataItem, "NUM_VALOR_MARKUP_4X"))%>'></asp:TextBox></strong></small>
                                                              </td>
                                                              <td>
                                                                  <small><strong><asp:TextBox runat="server" class="form-control" id="txtcredito5x" Text='<%# String.Format("{0:n2}",DataBinder.Eval(Container.DataItem, "NUM_VALOR_CREDITO_5X"))%>'></asp:TextBox></strong></small><br />
                                                                  <small><strong><asp:TextBox runat="server" class="form-control" id="txtmarkup5x" Text='<%# String.Format("{0:n2}",DataBinder.Eval(Container.DataItem, "NUM_VALOR_MARKUP_5X"))%>'></asp:TextBox></strong></small>
                                                              </td>
                                                              <td>
                                                                  <small><strong><asp:TextBox runat="server" class="form-control" id="txtcredito6x" Text='<%# String.Format("{0:n2}",DataBinder.Eval(Container.DataItem, "NUM_VALOR_CREDITO_6X"))%>'></asp:TextBox></strong></small><br />
                                                                  <small><strong><asp:TextBox runat="server" class="form-control" id="txtmarkup6x" Text='<%# String.Format("{0:n2}",DataBinder.Eval(Container.DataItem, "NUM_VALOR_MARKUP_6X"))%>'></asp:TextBox></strong></small>
                                                              </td>
                                                              <td>
                                                                  <small><strong><asp:TextBox runat="server" class="form-control" id="txtcredito7x" Text='<%# String.Format("{0:n2}",DataBinder.Eval(Container.DataItem, "NUM_VALOR_CREDITO_7X"))%>'></asp:TextBox></strong></small><br />
                                                                  <small><strong><asp:TextBox runat="server" class="form-control" id="txtmarkup7x" Text='<%# String.Format("{0:n2}",DataBinder.Eval(Container.DataItem, "NUM_VALOR_MARKUP_7X"))%>'></asp:TextBox></strong></small>
                                                              </td>
                                                              <td>
                                                                  <small><strong><asp:TextBox runat="server" class="form-control" id="txtcredito8x" Text='<%# String.Format("{0:n2}",DataBinder.Eval(Container.DataItem, "NUM_VALOR_CREDITO_8X"))%>'></asp:TextBox></strong></small><br />
                                                                  <small><strong><asp:TextBox runat="server" class="form-control" id="txtmarkup8x" Text='<%# String.Format("{0:n2}",DataBinder.Eval(Container.DataItem, "NUM_VALOR_MARKUP_8X"))%>'></asp:TextBox></strong></small>
                                                              </td>
                                                              <td>
                                                                  <small><strong><asp:TextBox runat="server" class="form-control" id="txtcredito9x" Text='<%# String.Format("{0:n2}",DataBinder.Eval(Container.DataItem, "NUM_VALOR_CREDITO_9X"))%>'></asp:TextBox></strong></small><br />
                                                                  <small><strong><asp:TextBox runat="server" class="form-control" id="txtmarkup9x" Text='<%# String.Format("{0:n2}",DataBinder.Eval(Container.DataItem, "NUM_VALOR_MARKUP_9X"))%>'></asp:TextBox></strong></small>
                                                              </td>
                                                              <td>
                                                                  <small><strong><asp:TextBox runat="server" class="form-control" id="txtcredito10x" Text='<%# String.Format("{0:n2}",DataBinder.Eval(Container.DataItem, "NUM_VALOR_CREDITO_10X"))%>'></asp:TextBox></strong></small><br />
                                                                  <small><strong><asp:TextBox runat="server" class="form-control" id="txtmarkup10x" Text='<%# String.Format("{0:n2}",DataBinder.Eval(Container.DataItem, "NUM_VALOR_MARKUP_10X"))%>'></asp:TextBox></strong></small>
                                                              </td>
                                                              <td>
                                                                  <small><strong><asp:TextBox runat="server" class="form-control" id="txtcredito11x" Text='<%# String.Format("{0:n2}",DataBinder.Eval(Container.DataItem, "NUM_VALOR_CREDITO_11X"))%>'></asp:TextBox></strong></small><br />
                                                                  <small><strong><asp:TextBox runat="server" class="form-control" id="txtmarkup11x" Text='<%# String.Format("{0:n2}",DataBinder.Eval(Container.DataItem, "NUM_VALOR_MARKUP_11X"))%>'></asp:TextBox></strong></small>
                                                              </td>
                                                              <td>
                                                                  <small><strong><asp:TextBox runat="server" class="form-control" id="txtcredito12x" Text='<%# String.Format("{0:n2}",DataBinder.Eval(Container.DataItem, "NUM_VALOR_CREDITO_12X"))%>'></asp:TextBox></strong></small><br />
                                                                  <small><strong><asp:TextBox runat="server" class="form-control" id="txtmarkup12x" Text='<%# String.Format("{0:n2}",DataBinder.Eval(Container.DataItem, "NUM_VALOR_MARKUP_12X"))%>'></asp:TextBox></strong></small>
                                                              </td>
                                                              <td>
                                                                  <small><strong><asp:TextBox runat="server" class="form-control" id="txtcredito13x" Text='<%# String.Format("{0:n2}",DataBinder.Eval(Container.DataItem, "NUM_VALOR_CREDITO_13X"))%>'></asp:TextBox></strong></small><br />
                                                                  <small><strong><asp:TextBox runat="server" class="form-control" id="txtmarkup13x" Text='<%# String.Format("{0:n2}",DataBinder.Eval(Container.DataItem, "NUM_VALOR_MARKUP_13X"))%>'></asp:TextBox></strong></small>
                                                              </td>
                                                              <td>
                                                                  <small><strong><asp:TextBox runat="server" class="form-control" id="txtcredito14x" Text='<%# String.Format("{0:n2}",DataBinder.Eval(Container.DataItem, "NUM_VALOR_CREDITO_14X"))%>'></asp:TextBox></strong></small><br />
                                                                  <small><strong><asp:TextBox runat="server" class="form-control" id="txtmarkup14x" Text='<%# String.Format("{0:n2}",DataBinder.Eval(Container.DataItem, "NUM_VALOR_MARKUP_14X"))%>'></asp:TextBox></strong></small>
                                                              </td>
                                                              <td>
                                                                  <small><strong><asp:TextBox runat="server" class="form-control" id="txtcredito15x" Text='<%# String.Format("{0:n2}",DataBinder.Eval(Container.DataItem, "NUM_VALOR_CREDITO_15X"))%>'></asp:TextBox></strong></small><br />
                                                                  <small><strong><asp:TextBox runat="server" class="form-control" id="txtmarkup15x" Text='<%# String.Format("{0:n2}",DataBinder.Eval(Container.DataItem, "NUM_VALOR_MARKUP_15X"))%>'></asp:TextBox></strong></small>
                                                              </td>
                                                              <td>
                                                                  <small><strong><asp:TextBox runat="server" class="form-control" id="txtcredito16x" Text='<%# String.Format("{0:n2}",DataBinder.Eval(Container.DataItem, "NUM_VALOR_CREDITO_16X"))%>'></asp:TextBox></strong></small><br />
                                                                  <small><strong><asp:TextBox runat="server" class="form-control" id="txtmarkup16x" Text='<%# String.Format("{0:n2}",DataBinder.Eval(Container.DataItem, "NUM_VALOR_MARKUP_16X"))%>'></asp:TextBox></strong></small>
                                                              </td>
                                                              <td>
                                                                  <small><strong><asp:TextBox runat="server" class="form-control" id="txtcredito17x" Text='<%# String.Format("{0:n2}",DataBinder.Eval(Container.DataItem, "NUM_VALOR_CREDITO_17X"))%>'></asp:TextBox></strong></small><br />
                                                                  <small><strong><asp:TextBox runat="server" class="form-control" id="txtmarkup17x" Text='<%# String.Format("{0:n2}",DataBinder.Eval(Container.DataItem, "NUM_VALOR_MARKUP_17X"))%>'></asp:TextBox></strong></small>
                                                              </td>
                                                              <td>
                                                                  <small><strong><asp:TextBox runat="server" class="form-control" id="txtcredito18x" Text='<%# String.Format("{0:n2}",DataBinder.Eval(Container.DataItem, "NUM_VALOR_CREDITO_18X"))%>'></asp:TextBox></strong></small><br />
                                                                  <small><strong><asp:TextBox runat="server" class="form-control" id="txtmarkup18x" Text='<%# String.Format("{0:n2}",DataBinder.Eval(Container.DataItem, "NUM_VALOR_MARKUP_18X"))%>'></asp:TextBox></strong></small>
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
                                        <asp:Button runat="server" ID="btnGravarParcelas" CssClass="btn btn-whitelabel1" Text="Gravar" onclick="btnGravarParcelas_Click"/>
                                    </div>
                                </div>
                                </div>
                            </div>

                            <!-- Fim tabela de parcelas **************************************************************************************************** -->



                          </div>

                          <div class="tab-pane fade" id="conteudo_online" role="tabpanel" aria-labelledby="conteudo_online_tab">


                            <!-- Divisão da tabela de taxas -->

                            <div class="row" runat="server" id="dvTaxasOnline" visible="false">
                                <div class="col-12">
                                <div class="card">

                                    <div class="card-header bg-whitelabel1">
                                    <h3 class="card-title">TAXAS PADRÕES</h3>
                                        <div class="card-tools">
                                          <button type="button" class="btn btn-tool" data-card-widget="collapse">
                                            <i class="fas fa-minus"></i>
                                          </button>
                                        </div>

                                    </div>
                                    <div class="card-body" style="overflow:scroll; height:500px;">

                                        <div class="row">
                                            <div class="col-12">
                                                <table id="tabConsultaOnline" class="table table-bordered table-hover table-striped">
                                                    <thead>
                                                    <tr>
                                                      <th>Ordem</th>
                                                      <th>Bandeira</th>
                                                      <th>Tipo</th>
                                                      <th>Taxa (%)</th>
                                                      <th>Markup (%)</th>
                                                      <th>Taxa Final (%)</th>
                                                    </tr>
                                                    </thead>
                                                    <tbody>
                                                    <asp:Repeater runat="server" ID="rptConsultaOnline" >
                                                        <ItemTemplate>

                                                            <tr>
                                                              <td>
                                                                <asp:TextBox runat="server" id="txtidplano" Text='<%# DataBinder.Eval(Container.DataItem, "COD_ID_PLANOS")%>' visible="false"></asp:TextBox>
                                                                <asp:TextBox runat="server" id="txtidbandeira" Text='<%# DataBinder.Eval(Container.DataItem, "COD_ID_BANDEIRAS")%>' visible="false"></asp:TextBox>
                                                                <asp:TextBox runat="server" id="txtidtabela" Text='<%# DataBinder.Eval(Container.DataItem, "COD_ID_PLANOS_TABELA")%>' visible="false"></asp:TextBox>
                                                                  <small><strong><%# DataBinder.Eval(Container.DataItem, "NUM_ORDEM_BANDEIRAS")%></strong></small>
                                                              </td>

                                                              <td>
                                                                  <small><strong><%# DataBinder.Eval(Container.DataItem, "NOM_BANDEIRA")%></strong></small>
                                                              </td>

                                                              <td>
                                                                  <small><strong><%# DataBinder.Eval(Container.DataItem, "NOM_TABELA")%></strong></small>
                                                              </td>
                                                              <td>
                                                                  <small><asp:TextBox runat="server" id="txtTaxa" Text='<%# String.Format("{0:n2}",DataBinder.Eval(Container.DataItem, "NUM_TAXA"))%>' class="form-control"></asp:TextBox></small>
                                                              </td>
                                                              <td>
                                                                  <small><asp:TextBox runat="server" id="txtMarkup" Text='<%# String.Format("{0:n2}",DataBinder.Eval(Container.DataItem, "NUM_MARKUP"))%>' class="form-control"></asp:TextBox></small>
                                                              </td>
                                                              <td>
                                                                  <strong><%# String.Format("{0:n2}",DataBinder.Eval(Container.DataItem, "NUM_TAXA_FINAL"))%></strong>
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
                                        <asp:Button runat="server" ID="btnGravarOnline" CssClass="btn btn-whitelabel1" Text="Gravar" onclick="btnGravarOnline_Click"/>
                                        <asp:Button runat="server" ID="btnRecalcularOnline" CssClass="btn btn-whitelabel1 float-right" Text="RecalcularOnline" onclick="btnRecalcular_Click"/>
                                    </div>
                                </div>
                                </div>
                            </div>

                            <!-- Fim da Tabela de taxas **************************************************************************************************** -->

                            <!-- Divisão da Tabela de parcelas **************************************************************************************************** -->
                            <div class="row" runat="server" id="dvParcelasOnline" visible="false">
                                <div class="col-12">
                                <div class="card">

                                    <div class="card-header whitelabel1">
                                    <h3 class="card-title">TAXAS ANTECIPAÇÃO</h3>
                                        <div class="card-tools">
                                          <button type="button" class="btn btn-tool" data-card-widget="collapse">
                                            <i class="fas fa-minus"></i>
                                          </button>
                                        </div>

                                    </div>
                                    <div class="card-body" style="overflow:scroll; height:800px;">

                                        <div class="row">
                                            <div class="col-12">
                                                <table id="Table1Online" class="table table-bordered table-hover table-striped">
                                                    <thead>
                                                    <tr>
                                                      <th><h6><small><b>Bandeira</b></small></h6></th>
                                                      <th><h6><small><b>Débito</b></small></h6></th>
                                                      <th><h6><small><b>Crédito À Vista</b></small></h6></th>
                                                      <th><h6><small><b>Crédito 2x</b></small></h6></th>
                                                      <th><h6><small><b>Crédito 3x</b></small></h6></th>
                                                      <th><h6><small><b>Crédito 4x</b></small></h6></th>
                                                      <th><h6><small><b>Crédito 5x</b></small></h6></th>
                                                      <th><h6><small><b>Crédito 6x</b></small></h6></th>
                                                      <th><h6><small><b>Crédito 7x</b></small></h6></th>
                                                      <th><h6><small><b>Crédito 8x</b></small></h6></th>
                                                      <th><h6><small><b>Crédito 9x</b></small></h6></th>
                                                      <th><h6><small><b>Crédito 10x</b></small></h6></th>
                                                      <th><h6><small><b>Crédito 11x</b></small></h6></th>
                                                      <th><h6><small><b>Crédito 12x</b></small></h6></th>
                                                      <th><h6><small><b>Crédito 13x</b></small></h6></th>
                                                      <th><h6><small><b>Crédito 14x</b></small></h6></th>
                                                      <th><h6><small><b>Crédito 15x</b></small></h6></th>
                                                      <th><h6><small><b>Crédito 16x</b></small></h6></th>
                                                      <th><h6><small><b>Crédito 17x</b></small></h6></th>
                                                      <th><h6><small><b>Crédito 18x</b></small></h6></th>
                                                    </tr>
                                                    </thead>
                                                    <tbody>
                                                    <asp:Repeater runat="server" ID="rptConsultaParcelasOnline" >
                                                        <ItemTemplate>

                                                            <tr>
                                                              <td class="text-center">
                                                                <asp:TextBox runat="server" id="txtidplano" Text='<%# DataBinder.Eval(Container.DataItem, "COD_ID_PLANOS")%>' visible="false"></asp:TextBox>
                                                                <asp:TextBox runat="server" id="txtidbandeira" Text='<%# DataBinder.Eval(Container.DataItem, "COD_ID_BANDEIRAS")%>' visible="false"></asp:TextBox>
                                                                  <small><strong><%# DataBinder.Eval(Container.DataItem, "NOM_BANDEIRA")%></strong></small><br />
                                                                  <asp:Image runat="server" id="imgBandeira" CssClass="img-fluid img-thumbnail mx-auto" style="max-width:70px;" ImageUrl='<%# DataBinder.Eval(Container.DataItem, "NOM_IMAGEM")%>'></asp:Image><br />
                                                                  <small><strong>Reb/Mkt</strong></small>
                                                              </td>

                                                              <td class="col-1">
                                                                  <small><strong><asp:TextBox runat="server" CssClass="form-control" id="txtdebito" Text='<%# String.Format("{0:n2}",DataBinder.Eval(Container.DataItem, "NUM_VALOR_DEBITO"))%>'></asp:TextBox></strong></small><br />
                                                                  <small><strong><asp:TextBox runat="server" CssClass="form-control" id="txtmarkupdebito" Text='<%# String.Format("{0:n2}",DataBinder.Eval(Container.DataItem, "NUM_VALOR_MARKUP_DEBITO"))%>'></asp:TextBox></strong></small>
                                                              </td>
                                                              <td class="col-1">
                                                                  <small><strong><asp:TextBox runat="server" CssClass="form-control" id="txtcredito" Text='<%# String.Format("{0:n2}",DataBinder.Eval(Container.DataItem, "NUM_VALOR_CREDITO_VISTA"))%>'></asp:TextBox></strong></small><br />
                                                                  <small><strong><asp:TextBox runat="server" CssClass="form-control" id="txtmarkupcredito" Text='<%# String.Format("{0:n2}",DataBinder.Eval(Container.DataItem, "NUM_VALOR_MARKUP_VISTA"))%>'></asp:TextBox></strong></small>
                                                              </td>
                                                              <td class="col">
                                                                  <small><strong><asp:TextBox runat="server" CssClass="form-control" id="txtcredito2x" Text='<%# String.Format("{0:n2}",DataBinder.Eval(Container.DataItem, "NUM_VALOR_CREDITO_2X"))%>'></asp:TextBox></strong></small><br />
                                                                  <small><strong><asp:TextBox runat="server" CssClass="form-control" id="txtmarkup2x" Text='<%# String.Format("{0:n2}",DataBinder.Eval(Container.DataItem, "NUM_VALOR_MARKUP_2X"))%>'></asp:TextBox></strong></small>
                                                              </td>
                                                              <td class="col">
                                                                  <small><strong><asp:TextBox runat="server" CssClass="form-control" id="txtcredito3x" Text='<%# String.Format("{0:n2}",DataBinder.Eval(Container.DataItem, "NUM_VALOR_CREDITO_3X"))%>'></asp:TextBox></strong></small><br />
                                                                  <small><strong><asp:TextBox runat="server" CssClass="form-control" id="txtmarkup3x" Text='<%# String.Format("{0:n2}",DataBinder.Eval(Container.DataItem, "NUM_VALOR_MARKUP_3X"))%>'></asp:TextBox></strong></small>
                                                              </td>
                                                              <td class="col">
                                                                  <small><strong><asp:TextBox runat="server" class="form-control" id="txtcredito4x" Text='<%# String.Format("{0:n2}",DataBinder.Eval(Container.DataItem, "NUM_VALOR_CREDITO_4X"))%>'></asp:TextBox></strong></small><br />
                                                                  <small><strong><asp:TextBox runat="server" class="form-control" id="txtmarkup4x" Text='<%# String.Format("{0:n2}",DataBinder.Eval(Container.DataItem, "NUM_VALOR_MARKUP_4X"))%>'></asp:TextBox></strong></small>
                                                              </td>
                                                              <td class="col">
                                                                  <small><strong><asp:TextBox runat="server" class="form-control" id="txtcredito5x" Text='<%# String.Format("{0:n2}",DataBinder.Eval(Container.DataItem, "NUM_VALOR_CREDITO_5X"))%>'></asp:TextBox></strong></small><br />
                                                                  <small><strong><asp:TextBox runat="server" class="form-control" id="txtmarkup5x" Text='<%# String.Format("{0:n2}",DataBinder.Eval(Container.DataItem, "NUM_VALOR_MARKUP_5X"))%>'></asp:TextBox></strong></small>
                                                              </td>
                                                              <td class="col">
                                                                  <small><strong><asp:TextBox runat="server" class="form-control" id="txtcredito6x" Text='<%# String.Format("{0:n2}",DataBinder.Eval(Container.DataItem, "NUM_VALOR_CREDITO_6X"))%>'></asp:TextBox></strong></small><br />
                                                                  <small><strong><asp:TextBox runat="server" class="form-control" id="txtmarkup6x" Text='<%# String.Format("{0:n2}",DataBinder.Eval(Container.DataItem, "NUM_VALOR_MARKUP_6X"))%>'></asp:TextBox></strong></small>
                                                              </td>
                                                              <td class="col">
                                                                  <small><strong><asp:TextBox runat="server" class="form-control" id="txtcredito7x" Text='<%# String.Format("{0:n2}",DataBinder.Eval(Container.DataItem, "NUM_VALOR_CREDITO_7X"))%>'></asp:TextBox></strong></small><br />
                                                                  <small><strong><asp:TextBox runat="server" class="form-control" id="txtmarkup7x" Text='<%# String.Format("{0:n2}",DataBinder.Eval(Container.DataItem, "NUM_VALOR_MARKUP_7X"))%>'></asp:TextBox></strong></small>
                                                              </td>
                                                              <td class="col">
                                                                  <small><strong><asp:TextBox runat="server" class="form-control" id="txtcredito8x" Text='<%# String.Format("{0:n2}",DataBinder.Eval(Container.DataItem, "NUM_VALOR_CREDITO_8X"))%>'></asp:TextBox></strong></small><br />
                                                                  <small><strong><asp:TextBox runat="server" class="form-control" id="txtmarkup8x" Text='<%# String.Format("{0:n2}",DataBinder.Eval(Container.DataItem, "NUM_VALOR_MARKUP_8X"))%>'></asp:TextBox></strong></small>
                                                              </td>
                                                              <td class="col">
                                                                  <small><strong><asp:TextBox runat="server" class="form-control" id="txtcredito9x" Text='<%# String.Format("{0:n2}",DataBinder.Eval(Container.DataItem, "NUM_VALOR_CREDITO_9X"))%>'></asp:TextBox></strong></small><br />
                                                                  <small><strong><asp:TextBox runat="server" class="form-control" id="txtmarkup9x" Text='<%# String.Format("{0:n2}",DataBinder.Eval(Container.DataItem, "NUM_VALOR_MARKUP_9X"))%>'></asp:TextBox></strong></small>
                                                              </td>
                                                              <td class="col">
                                                                  <small><strong><asp:TextBox runat="server" class="form-control" id="txtcredito10x" Text='<%# String.Format("{0:n2}",DataBinder.Eval(Container.DataItem, "NUM_VALOR_CREDITO_10X"))%>'></asp:TextBox></strong></small><br />
                                                                  <small><strong><asp:TextBox runat="server" class="form-control" id="txtmarkup10x" Text='<%# String.Format("{0:n2}",DataBinder.Eval(Container.DataItem, "NUM_VALOR_MARKUP_10X"))%>'></asp:TextBox></strong></small>
                                                              </td>
                                                              <td class="col">
                                                                  <small><strong><asp:TextBox runat="server" class="form-control" id="txtcredito11x" Text='<%# String.Format("{0:n2}",DataBinder.Eval(Container.DataItem, "NUM_VALOR_CREDITO_11X"))%>'></asp:TextBox></strong></small><br />
                                                                  <small><strong><asp:TextBox runat="server" class="form-control" id="txtmarkup11x" Text='<%# String.Format("{0:n2}",DataBinder.Eval(Container.DataItem, "NUM_VALOR_MARKUP_11X"))%>'></asp:TextBox></strong></small>
                                                              </td>
                                                              <td class="col">
                                                                  <small><strong><asp:TextBox runat="server" class="form-control" id="txtcredito12x" Text='<%# String.Format("{0:n2}",DataBinder.Eval(Container.DataItem, "NUM_VALOR_CREDITO_12X"))%>'></asp:TextBox></strong></small><br />
                                                                  <small><strong><asp:TextBox runat="server" class="form-control" id="txtmarkup12x" Text='<%# String.Format("{0:n2}",DataBinder.Eval(Container.DataItem, "NUM_VALOR_MARKUP_12X"))%>'></asp:TextBox></strong></small>
                                                              </td>
                                                              <td class="col">
                                                                  <small><strong><asp:TextBox runat="server" class="form-control" id="txtcredito13x" Text='<%# String.Format("{0:n2}",DataBinder.Eval(Container.DataItem, "NUM_VALOR_CREDITO_13X"))%>'></asp:TextBox></strong></small><br />
                                                                  <small><strong><asp:TextBox runat="server" class="form-control" id="txtmarkup13x" Text='<%# String.Format("{0:n2}",DataBinder.Eval(Container.DataItem, "NUM_VALOR_MARKUP_13X"))%>'></asp:TextBox></strong></small>
                                                              </td>
                                                              <td class="col">
                                                                  <small><strong><asp:TextBox runat="server" class="form-control" id="txtcredito14x" Text='<%# String.Format("{0:n2}",DataBinder.Eval(Container.DataItem, "NUM_VALOR_CREDITO_14X"))%>'></asp:TextBox></strong></small><br />
                                                                  <small><strong><asp:TextBox runat="server" class="form-control" id="txtmarkup14x" Text='<%# String.Format("{0:n2}",DataBinder.Eval(Container.DataItem, "NUM_VALOR_MARKUP_14X"))%>'></asp:TextBox></strong></small>
                                                              </td>
                                                              <td class="col">
                                                                  <small><strong><asp:TextBox runat="server" class="form-control" id="txtcredito15x" Text='<%# String.Format("{0:n2}",DataBinder.Eval(Container.DataItem, "NUM_VALOR_CREDITO_15X"))%>'></asp:TextBox></strong></small><br />
                                                                  <small><strong><asp:TextBox runat="server" class="form-control" id="txtmarkup15x" Text='<%# String.Format("{0:n2}",DataBinder.Eval(Container.DataItem, "NUM_VALOR_MARKUP_15X"))%>'></asp:TextBox></strong></small>
                                                              </td>
                                                              <td class="col">
                                                                  <small><strong><asp:TextBox runat="server" class="form-control" id="txtcredito16x" Text='<%# String.Format("{0:n2}",DataBinder.Eval(Container.DataItem, "NUM_VALOR_CREDITO_16X"))%>'></asp:TextBox></strong></small><br />
                                                                  <small><strong><asp:TextBox runat="server" class="form-control" id="txtmarkup16x" Text='<%# String.Format("{0:n2}",DataBinder.Eval(Container.DataItem, "NUM_VALOR_MARKUP_16X"))%>'></asp:TextBox></strong></small>
                                                              </td>
                                                              <td class="col">
                                                                  <small><strong><asp:TextBox runat="server" class="form-control" id="txtcredito17x" Text='<%# String.Format("{0:n2}",DataBinder.Eval(Container.DataItem, "NUM_VALOR_CREDITO_17X"))%>'></asp:TextBox></strong></small><br />
                                                                  <small><strong><asp:TextBox runat="server" class="form-control" id="txtmarkup17x" Text='<%# String.Format("{0:n2}",DataBinder.Eval(Container.DataItem, "NUM_VALOR_MARKUP_17X"))%>'></asp:TextBox></strong></small>
                                                              </td>
                                                              <td class="col">
                                                                  <small><strong><asp:TextBox runat="server" class="form-control" id="txtcredito18x" Text='<%# String.Format("{0:n2}",DataBinder.Eval(Container.DataItem, "NUM_VALOR_CREDITO_18X"))%>'></asp:TextBox></strong></small><br />
                                                                  <small><strong><asp:TextBox runat="server" class="form-control" id="txtmarkup18x" Text='<%# String.Format("{0:n2}",DataBinder.Eval(Container.DataItem, "NUM_VALOR_MARKUP_18X"))%>'></asp:TextBox></strong></small>
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
                                        <asp:Button runat="server" ID="btnGravarParcelasOnline" CssClass="btn btn-whitelabel1" Text="Gravar" onclick="btnGravarParcelasOnline_Click"/>
                                    </div>
                                </div>
                                </div>
                            </div>

                            <!-- Fim tabela de parcelas **************************************************************************************************** -->



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

$(document).ready(function () {
    var table = $('#tabConsulta').DataTable({
      "paging": false,
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


</body>
</html>
