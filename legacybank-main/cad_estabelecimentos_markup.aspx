<%@ Page Language="C#" AutoEventWireup="true" CodeFile="cad_estabelecimentos_markup.aspx.cs" Inherits="cad_estabelecimentos_markup" %>

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


    <style>
        .tab_taxas
        {
            font-size:10px;
            padding:4px;
            margin:0px;
            height: calc(1.50rem + 1px);
        }
        
        .tab_titulo
        {
            font-size:10px;
        }
        
        .btn_taxas
        {
            font-size:10px;
            padding:2px;
            margin:0px;
        }
        
    </style>


</head>
<body class="hold-transition sidebar-mini layout-fixed">
    <form id="frmPrincipal" runat="server">

        <div class="row">
            <div class="col-12">

                <div class="card">
                    <div class="card-header bg-whitelabel1">
                        <h3 class="card-title">PERCENTUAIS MARKUP</h3>
                    </div>
                    <div class="card-body">
                        <h3><span class="float-center badge bg-whitelabel1">Dados do Estabelecimento</span></h3>

                        <div class="row">
                            <div class="col-sm-2">
                                <div class="form-group">
                                    <label>ID</label>
                                    <asp:TextBox runat="server" id="txtID" cssClass="form-control" Enabled="false"></asp:TextBox>
                                </div>
                            </div>
                            <div class="col-sm-3">
                                <div class="form-group">
                                    <label>Estabelecimento</label>
                                    <asp:TextBox runat="server" id="txtRazaosocial" cssClass="form-control" Enabled="false"></asp:TextBox>
                                </div>
                            </div>
                            <div class="col-sm-3">
                                <div class="form-group">
                                    <label>E-mail</label>
                                    <asp:TextBox runat="server" id="txtEmail" cssClass="form-control" Enabled="false"></asp:TextBox>
                                </div>
                            </div>
                            <div class="col-sm-2">
                                <div class="form-group">
                                    <label>Tipo</label>
                                    <asp:TextBox runat="server" id="txtTipo" cssClass="form-control" Enabled="false"></asp:TextBox>
                                </div>
                            </div>
                            <div class="col-sm-2">
                                <div class="form-group">
                                    <label>Documento</label>
                                    <asp:TextBox runat="server" id="txtDocumento" cssClass="form-control" Enabled="false"></asp:TextBox>
                                </div>
                            </div>
                        </div>
                        


                        <h3><span class="float-center badge bg-whitelabel1">Dados do Percentual Markup</span></h3>

                        <div class="row">
                            <div class="col-sm-12">
                                <div class="form-group">
                                <label>Carregar dados a partir da tabela padrão<strong class="text-danger">*</strong></label>
                                <asp:DropDownList runat="server" id="ddlTabelasMarkup" CssClass="form-control" 
                                        AutoPostBack="True" 
                                        onselectedindexchanged="ddlTabelasMarkup_SelectedIndexChanged">
                                </asp:DropDownList>
                                </div>
                            </div>
                        </div>


                        <div class="row">
                            <div class="col-sm-4">
                                <div class="form-group">
                                <label>Adquirente<strong class="text-danger">*</strong></label>
                                <asp:DropDownList runat="server" id="ddlAdquirentes" CssClass="form-control" 
                                        AutoPostBack="True" 
                                        onselectedindexchanged="ddlAdquirentes_SelectedIndexChanged">
                                </asp:DropDownList>
                                </div>
                            </div>

                            <div class="col-sm-4">
                                <div class="form-group">
                                <label>Plano<strong class="text-danger">*</strong></label>
                                <asp:DropDownList runat="server" id="ddlPlano" CssClass="form-control" Enabled="false">
                                </asp:DropDownList>
                                </div>
                            </div>

                            <div class="col-sm-4">
                                <div class="form-group">
                                <label>Presencial/On-Line<strong class="text-danger">*</strong></label>
                                <asp:DropDownList runat="server" id="ddlPresencialOnLine" CssClass="form-control" AutoPostBack="True" onselectedindexchanged="ddlPresencialOnLine_SelectedIndexChanged">
                                    <asp:ListItem Text="Presencial" Value="P"></asp:ListItem>
                                    <asp:ListItem Text="On-Line" Value="O"></asp:ListItem>
                                </asp:DropDownList>
                                </div>
                            </div>

                        </div>

                        <div class="row mt-3">
                            <div class="col-12">

                                <div class="card">
                                    <div class="card-header bg-whitelabel1">
                                        <h3 class="card-title">LISTA DO PERCENTUAL DE MARKUP</h3>
                                    </div>
                                    <div class="card-body" style="overflow:auto; width: 100%;">
                                        <table id="tbConsultaMarkup" class="table table-bordered table-hover">
                                            <thead>
                                            <tr>
                                                <th><small class="tab_titulo">Bandeira</small></th>

                                                <th><small class="tab_titulo">
                                                    Por Transação (R$)<br />
                                                    <button type="button" class="btn btn-sm btn_taxas btn-success" onclick="alterarColuna(1, 0.01)">+</button>
                                                    <button type="button" class="btn btn-sm btn_taxas btn-danger" onclick="alterarColuna(1, -0.01)">-</button>
                                                </small></th>

                                                <th><small class="tab_titulo">
                                                    Débito/À Vista (%)<br />
                                                    <button type="button" class="btn btn-sm btn_taxas btn-success" onclick="alterarColuna(2, 0.01)">+</button>
                                                    <button type="button" class="btn btn-sm btn_taxas btn-danger" onclick="alterarColuna(2, -0.01)">-</button>
                                                </small></th>
                                                <th><small class="tab_titulo">Crédito 1x (%)<br />
                                                    <button type="button" class="btn btn-sm btn_taxas btn-success" onclick="alterarColuna(3, 0.01)">+</button>
                                                    <button type="button" class="btn btn-sm btn_taxas btn-danger" onclick="alterarColuna(3, -0.01)">-</button></small></th>
                                                <th><small class="tab_titulo">Crédito 2x (%)<br />
                                                    <button type="button" class="btn btn-sm btn_taxas btn-success" onclick="alterarColuna(4, 0.01)">+</button>
                                                    <button type="button" class="btn btn-sm btn_taxas btn-danger" onclick="alterarColuna(4, -0.01)">-</button></small></th>
                                                <th><small class="tab_titulo">Crédito 3x (%)<br />
                                                    <button type="button" class="btn btn-sm btn_taxas btn-success" onclick="alterarColuna(5, 0.01)">+</button>
                                                    <button type="button" class="btn btn-sm btn_taxas btn-danger" onclick="alterarColuna(5, -0.01)">-</button></small></th>
                                                <th><small class="tab_titulo">Crédito 4x (%)<br />
                                                    <button type="button" class="btn btn-sm btn_taxas btn-success" onclick="alterarColuna(6, 0.01)">+</button>
                                                    <button type="button" class="btn btn-sm btn_taxas btn-danger" onclick="alterarColuna(6, -0.01)">-</button></small></th>
                                                <th><small class="tab_titulo">Crédito 5x (%)<br />
                                                    <button type="button" class="btn btn-sm btn_taxas btn-success" onclick="alterarColuna(7, 0.01)">+</button>
                                                    <button type="button" class="btn btn-sm btn_taxas btn-danger" onclick="alterarColuna(7, -0.01)">-</button></small></th>
                                                <th><small class="tab_titulo">Crédito 6x (%)<br />
                                                    <button type="button" class="btn btn-sm btn_taxas btn-success" onclick="alterarColuna(8, 0.01)">+</button>
                                                    <button type="button" class="btn btn-sm btn_taxas btn-danger" onclick="alterarColuna(8, -0.01)">-</button></small></th>
                                                <th><small class="tab_titulo">Crédito 7x (%)<br />
                                                    <button type="button" class="btn btn-sm btn_taxas btn-success" onclick="alterarColuna(9, 0.01)">+</button>
                                                    <button type="button" class="btn btn-sm btn_taxas btn-danger" onclick="alterarColuna(9, -0.01)">-</button></small></th>
                                                <th><small class="tab_titulo">Crédito 8x (%)<br />
                                                    <button type="button" class="btn btn-sm btn_taxas btn-success" onclick="alterarColuna(10, 0.01)">+</button>
                                                    <button type="button" class="btn btn-sm btn_taxas btn-danger" onclick="alterarColuna(10, -0.01)">-</button></small></th>
                                                <th><small class="tab_titulo">Crédito 9x (%)<br />
                                                    <button type="button" class="btn btn-sm btn_taxas btn-success" onclick="alterarColuna(11, 0.01)">+</button>
                                                    <button type="button" class="btn btn-sm btn_taxas btn-danger" onclick="alterarColuna(11, -0.01)">-</button></small></th>
                                                <th><small class="tab_titulo">Crédito 10x (%)<br />
                                                    <button type="button" class="btn btn-sm btn_taxas btn-success" onclick="alterarColuna(12, 0.01)">+</button>
                                                    <button type="button" class="btn btn-sm btn_taxas btn-danger" onclick="alterarColuna(12, -0.01)">-</button></small></th>
                                                <th><small class="tab_titulo">Crédito 11x (%)<br />
                                                    <button type="button" class="btn btn-sm btn_taxas btn-success" onclick="alterarColuna(13, 0.01)">+</button>
                                                    <button type="button" class="btn btn-sm btn_taxas btn-danger" onclick="alterarColuna(13, -0.01)">-</button></small></th>
                                                <th><small class="tab_titulo">Crédito 12x (%)<br />
                                                    <button type="button" class="btn btn-sm btn_taxas btn-success" onclick="alterarColuna(14, 0.01)">+</button>
                                                    <button type="button" class="btn btn-sm btn_taxas btn-danger" onclick="alterarColuna(14, -0.01)">-</button></small></th>
                                                <th><small class="tab_titulo">Crédito 13x (%)<br />
                                                    <button type="button" class="btn btn-sm btn_taxas btn-success" onclick="alterarColuna(15, 0.01)">+</button>
                                                    <button type="button" class="btn btn-sm btn_taxas btn-danger" onclick="alterarColuna(15, -0.01)">-</button></small></th>
                                                <th><small class="tab_titulo">Crédito 14x (%)<br />
                                                    <button type="button" class="btn btn-sm btn_taxas btn-success" onclick="alterarColuna(16, 0.01)">+</button>
                                                    <button type="button" class="btn btn-sm btn_taxas btn-danger" onclick="alterarColuna(16, -0.01)">-</button></small></th>
                                                <th><small class="tab_titulo">Crédito 15x (%)<br />
                                                    <button type="button" class="btn btn-sm btn_taxas btn-success" onclick="alterarColuna(17, 0.01)">+</button>
                                                    <button type="button" class="btn btn-sm btn_taxas btn-danger" onclick="alterarColuna(17, -0.01)">-</button></small></th>
                                                <th><small>Crédito 16x (%)<br />
                                                    <button type="button" class="btn btn-sm btn_taxas btn-success" onclick="alterarColuna(18, 0.01)">+</button>
                                                    <button type="button" class="btn btn-sm btn_taxas btn-danger" onclick="alterarColuna(18, -0.01)">-</button></small></th>
                                                <th><small>Crédito 17x (%)<br />
                                                    <button type="button" class="btn btn-sm btn_taxas btn-success" onclick="alterarColuna(19, 0.01)">+</button>
                                                    <button type="button" class="btn btn-sm btn_taxas btn-danger" onclick="alterarColuna(19, -0.01)">-</button></small></th>
                                                <th><small>Crédito 18x (%)<br />
                                                    <button type="button" class="btn btn-sm btn_taxas btn-success" onclick="alterarColuna(20, 0.01)">+</button>
                                                    <button type="button" class="btn btn-sm btn_taxas btn-danger" onclick="alterarColuna(20, -0.01)">-</button></small></th>

                                                <th><small class="tab_titulo">Crédito 19x (%)<br />
                                                    <button type="button" class="btn btn-sm btn_taxas btn-success" onclick="alterarColuna(21, 0.01)">+</button>
                                                    <button type="button" class="btn btn-sm btn_taxas btn-danger" onclick="alterarColuna(21, -0.01)">-</button></small></th>
                                                <th><small class="tab_titulo">Crédito 20x (%)<br />
                                                    <button type="button" class="btn btn-sm btn_taxas btn-success" onclick="alterarColuna(22, 0.01)">+</button>
                                                    <button type="button" class="btn btn-sm btn_taxas btn-danger" onclick="alterarColuna(22, -0.01)">-</button></small></th>
                                                <th><small class="tab_titulo">Crédito 21x (%)<br />
                                                    <button type="button" class="btn btn-sm btn_taxas btn-success" onclick="alterarColuna(23, 0.01)">+</button>
                                                    <button type="button" class="btn btn-sm btn_taxas btn-danger" onclick="alterarColuna(23, -0.01)">-</button></small></th>


                                            </tr>
                                            </thead>
                                            <tbody>
                                            <asp:Repeater runat="server" ID="rptConsultaMarkup" 
                                                    onitemcommand="rptConsultaMarkup_ItemCommand">
                                                <ItemTemplate>
                                                    <tr>
                                                        <td class="text-center">
                                                            <small class="tab_titulo"><strong><%# DataBinder.Eval(Container.DataItem, "NOM_BANDEIRA")%></strong></small><br />
                                                            <asp:TextBox runat="server" id="txtidbandeira" Text='<%# DataBinder.Eval(Container.DataItem, "COD_ID_BANDEIRAS")%>' visible="false"></asp:TextBox>
                                                            <asp:Image runat="server" id="imgBandeira" CssClass="img-fluid img-thumbnail mx-auto" style="max-width:35px;" ImageUrl='<%# DataBinder.Eval(Container.DataItem, "NOM_IMAGEM")%>'></asp:Image><br />
                                                            <button type="button" class="btn btn-sm btn_taxas btn-success" onclick="alterarLinha(this, 0.01)">+</button>
                                                            <button type="button" class="btn btn-sm btn_taxas btn-danger" onclick="alterarLinha(this, -0.01)">-</button>
                                                        </td>

                                                        <td class="text-right" style="min-width:35px;">
                                                            <small><strong><asp:TextBox runat="server" CssClass="form-control tab_taxas" id="txtmarkuptransacao" Text='<%# Convert.ToDecimal(DataBinder.Eval(Container.DataItem, "NUM_VALOR_MARKUP_TRANSACAO")).ToString("0.00") %>'></asp:TextBox></strong></small>
                                                            <small><strong>T:<asp:Label runat="server" CssClass="tab_taxas" id="txtmarkuptransacaoTOTAL" Text='<%# Convert.ToDecimal(DataBinder.Eval(Container.DataItem, "NUM_VALOR_MARKUP_TRANSACAO_TOTAL")).ToString("0.00") %>' ></asp:Label></strong></small>
                                                        </td>

                                                        <td class="text-right" style="min-width:35px;">
                                                            <small><strong><asp:TextBox runat="server" CssClass="form-control tab_taxas" id="txtmarkupdebito" Text='<%# Convert.ToDecimal(DataBinder.Eval(Container.DataItem, "NUM_VALOR_MARKUP_DEBITO")).ToString("0.00") %>'></asp:TextBox></strong></small>
                                                            <small><strong>T:<asp:Label runat="server" CssClass="tab_taxas" id="txtmarkupdebitoTOTAL" Text='<%# Convert.ToDecimal(DataBinder.Eval(Container.DataItem, "NUM_VALOR_MARKUP_DEBITO_TOTAL")).ToString("0.00") %>'></asp:Label></strong></small>

                                                        </td>
                                                        <td class="text-right" style="min-width:35px;">
                                                            <small><strong><asp:TextBox runat="server" CssClass="form-control tab_taxas" id="txtmarkupcredito" Text='<%# Convert.ToDecimal(DataBinder.Eval(Container.DataItem, "NUM_VALOR_MARKUP_VISTA")).ToString("0.00") %>'></asp:TextBox></strong></small>
                                                            <small><strong>T:<asp:Label runat="server" CssClass="tab_taxas" id="txtmarkupcreditoTOTAL" Text='<%# Convert.ToDecimal(DataBinder.Eval(Container.DataItem, "NUM_VALOR_MARKUP_VISTA_TOTAL")).ToString("0.00") %>'></asp:Label></strong></small>
                                                        </td>
                                                        <td class="text-right" style="min-width:35px;">
                                                            <small><strong><asp:TextBox runat="server" CssClass="form-control tab_taxas" id="txtmarkup2x" Text='<%# Convert.ToDecimal(DataBinder.Eval(Container.DataItem, "NUM_VALOR_MARKUP_2X")).ToString("0.00") %>'></asp:TextBox></strong></small>
                                                            <small><strong>T:<asp:Label runat="server" CssClass="tab_taxas" id="txtmarkup2xTOTAL" Text='<%# Convert.ToDecimal(DataBinder.Eval(Container.DataItem, "NUM_VALOR_MARKUP_2X_TOTAL")).ToString("0.00") %>'></asp:Label></strong></small>

                                                        </td>
                                                        <td class="text-right" style="min-width:35px;">
                                                            <small><strong><asp:TextBox runat="server" CssClass="form-control tab_taxas" id="txtmarkup3x" Text='<%# Convert.ToDecimal(DataBinder.Eval(Container.DataItem, "NUM_VALOR_MARKUP_3X")).ToString("0.00") %>'></asp:TextBox></strong></small>
                                                            <small><strong>T:<asp:Label runat="server" CssClass="tab_taxas" id="txtmarkup3xTOTAL" Text='<%# Convert.ToDecimal(DataBinder.Eval(Container.DataItem, "NUM_VALOR_MARKUP_3X_TOTAL")).ToString("0.00") %>'></asp:Label></strong></small>

                                                        </td>
                                                        <td class="text-right" style="min-width:35px;">
                                                            <small><strong><asp:TextBox runat="server" CssClass="form-control tab_taxas" id="txtmarkup4x" Text='<%# Convert.ToDecimal(DataBinder.Eval(Container.DataItem, "NUM_VALOR_MARKUP_4X")).ToString("0.00") %>'></asp:TextBox></strong></small>
                                                            <small><strong>T:<asp:Label runat="server" CssClass="tab_taxas" id="txtmarkup4xTOTAL" Text='<%# Convert.ToDecimal(DataBinder.Eval(Container.DataItem, "NUM_VALOR_MARKUP_4X_TOTAL")).ToString("0.00") %>'></asp:Label></strong></small>
                                                        </td>
                                                        <td class="text-right" style="min-width:35px;">
                                                            <small><strong><asp:TextBox runat="server" CssClass="form-control tab_taxas" id="txtmarkup5x" Text='<%# Convert.ToDecimal(DataBinder.Eval(Container.DataItem, "NUM_VALOR_MARKUP_5X")).ToString("0.00") %>'></asp:TextBox></strong></small>
                                                            <small><strong>T:<asp:Label runat="server" CssClass="tab_taxas" id="txtmarkup5xTOTAL" Text='<%# Convert.ToDecimal(DataBinder.Eval(Container.DataItem, "NUM_VALOR_MARKUP_5X_TOTAL")).ToString("0.00") %>'></asp:Label></strong></small>
                                                        </td>
                                                        <td class="text-right" style="min-width:35px;">
                                                            <small><strong><asp:TextBox runat="server" CssClass="form-control tab_taxas" id="txtmarkup6x" Text='<%# Convert.ToDecimal(DataBinder.Eval(Container.DataItem, "NUM_VALOR_MARKUP_6X")).ToString("0.00") %>'></asp:TextBox></strong></small>
                                                            <small><strong>T:<asp:Label runat="server" CssClass="tab_taxas" id="txtmarkup6xTOTAL" Text='<%# Convert.ToDecimal(DataBinder.Eval(Container.DataItem, "NUM_VALOR_MARKUP_6X_TOTAL")).ToString("0.00") %>'></asp:Label></strong></small>
                                                        </td>
                                                        <td class="text-right" style="min-width:35px;">
                                                            <small><strong><asp:TextBox runat="server" CssClass="form-control tab_taxas" id="txtmarkup7x" Text='<%# Convert.ToDecimal(DataBinder.Eval(Container.DataItem, "NUM_VALOR_MARKUP_7X")).ToString("0.00") %>'></asp:TextBox></strong></small>
                                                            <small><strong>T:<asp:Label runat="server" CssClass="tab_taxas" id="txtmarkup7xTOTAL" Text='<%# Convert.ToDecimal(DataBinder.Eval(Container.DataItem, "NUM_VALOR_MARKUP_7X_TOTAL")).ToString("0.00") %>'></asp:Label></strong></small>
                                                        </td>
                                                        <td class="text-right" style="min-width:35px;">
                                                            <small><strong><asp:TextBox runat="server" CssClass="form-control tab_taxas" id="txtmarkup8x" Text='<%# Convert.ToDecimal(DataBinder.Eval(Container.DataItem, "NUM_VALOR_MARKUP_8X")).ToString("0.00") %>'></asp:TextBox></strong></small>
                                                            <small><strong>T:<asp:Label runat="server" CssClass="tab_taxas" id="txtmarkup8xTOTAL" Text='<%# Convert.ToDecimal(DataBinder.Eval(Container.DataItem, "NUM_VALOR_MARKUP_8X_TOTAL")).ToString("0.00") %>'></asp:Label></strong></small>
                                                        </td>
                                                        <td class="text-right" style="min-width:35px;">
                                                            <small><strong><asp:TextBox runat="server" CssClass="form-control tab_taxas" id="txtmarkup9x" Text='<%# Convert.ToDecimal(DataBinder.Eval(Container.DataItem, "NUM_VALOR_MARKUP_9X")).ToString("0.00") %>'></asp:TextBox></strong></small>
                                                            <small><strong>T:<asp:Label runat="server" CssClass="tab_taxas" id="txtmarkup9xTOTAL" Text='<%# Convert.ToDecimal(DataBinder.Eval(Container.DataItem, "NUM_VALOR_MARKUP_9X_TOTAL")).ToString("0.00") %>'></asp:Label></strong></small>
                                                        </td>
                                                        <td class="text-right" style="min-width:35px;">
                                                            <small><strong><asp:TextBox runat="server" CssClass="form-control tab_taxas" id="txtmarkup10x" Text='<%# Convert.ToDecimal(DataBinder.Eval(Container.DataItem, "NUM_VALOR_MARKUP_10X")).ToString("0.00") %>'></asp:TextBox></strong></small>
                                                            <small><strong>T:<asp:Label runat="server" CssClass="tab_taxas" id="txtmarkup10xTOTAL" Text='<%# Convert.ToDecimal(DataBinder.Eval(Container.DataItem, "NUM_VALOR_MARKUP_10X_TOTAL")).ToString("0.00") %>'></asp:Label></strong></small>
                                                        </td>
                                                        <td class="text-right" style="min-width:35px;">
                                                            <small><strong><asp:TextBox runat="server" CssClass="form-control tab_taxas" id="txtmarkup11x" Text='<%# Convert.ToDecimal(DataBinder.Eval(Container.DataItem, "NUM_VALOR_MARKUP_11X")).ToString("0.00") %>'></asp:TextBox></strong></small>
                                                            <small><strong>T:<asp:Label runat="server" CssClass="tab_taxas" id="txtmarkup11xTOTAL" Text='<%# Convert.ToDecimal(DataBinder.Eval(Container.DataItem, "NUM_VALOR_MARKUP_11X_TOTAL")).ToString("0.00") %>'></asp:Label></strong></small>
                                                        </td>
                                                        <td class="text-right" style="min-width:35px;">
                                                            <small><strong><asp:TextBox runat="server" CssClass="form-control tab_taxas" id="txtmarkup12x" Text='<%# Convert.ToDecimal(DataBinder.Eval(Container.DataItem, "NUM_VALOR_MARKUP_12X")).ToString("0.00") %>'></asp:TextBox></strong></small>
                                                            <small><strong>T:<asp:Label runat="server" CssClass="tab_taxas" id="txtmarkup12xTOTAL" Text='<%# Convert.ToDecimal(DataBinder.Eval(Container.DataItem, "NUM_VALOR_MARKUP_12X_TOTAL")).ToString("0.00") %>'></asp:Label></strong></small>
                                                        </td>
                                                        <td class="text-right" style="min-width:35px;">
                                                            <small><strong><asp:TextBox runat="server" CssClass="form-control tab_taxas" id="txtmarkup13x" Text='<%# Convert.ToDecimal(DataBinder.Eval(Container.DataItem, "NUM_VALOR_MARKUP_13X")).ToString("0.00") %>'></asp:TextBox></strong></small>
                                                            <small><strong>T:<asp:Label runat="server" CssClass="tab_taxas" id="txtmarkup13xTOTAL" Text='<%# Convert.ToDecimal(DataBinder.Eval(Container.DataItem, "NUM_VALOR_MARKUP_13X_TOTAL")).ToString("0.00") %>'></asp:Label></strong></small>
                                                        </td>
                                                        <td class="text-right" style="min-width:35px;">
                                                            <small><strong><asp:TextBox runat="server" CssClass="form-control tab_taxas" id="txtmarkup14x" Text='<%# Convert.ToDecimal(DataBinder.Eval(Container.DataItem, "NUM_VALOR_MARKUP_14X")).ToString("0.00") %>'></asp:TextBox></strong></small>
                                                            <small><strong>T:<asp:Label runat="server" CssClass="tab_taxas" id="txtmarkup14xTOTAL" Text='<%# Convert.ToDecimal(DataBinder.Eval(Container.DataItem, "NUM_VALOR_MARKUP_14X_TOTAL")).ToString("0.00") %>'></asp:Label></strong></small>
                                                        </td>
                                                        <td class="text-right" style="min-width:35px;">
                                                            <small><strong><asp:TextBox runat="server" CssClass="form-control tab_taxas" id="txtmarkup15x" Text='<%# Convert.ToDecimal(DataBinder.Eval(Container.DataItem, "NUM_VALOR_MARKUP_15X")).ToString("0.00") %>'></asp:TextBox></strong></small>
                                                            <small><strong>T:<asp:Label runat="server" CssClass="tab_taxas" id="txtmarkup15xTOTAL" Text='<%# Convert.ToDecimal(DataBinder.Eval(Container.DataItem, "NUM_VALOR_MARKUP_15X_TOTAL")).ToString("0.00") %>'></asp:Label></strong></small>
                                                        </td>
                                                        <td class="text-right" style="min-width:35px;">
                                                            <small><strong><asp:TextBox runat="server" CssClass="form-control tab_taxas" id="txtmarkup16x" Text='<%# Convert.ToDecimal(DataBinder.Eval(Container.DataItem, "NUM_VALOR_MARKUP_16X")).ToString("0.00") %>'></asp:TextBox></strong></small>
                                                            <small><strong>T:<asp:Label runat="server" CssClass="tab_taxas" id="txtmarkup16xTOTAL" Text='<%# Convert.ToDecimal(DataBinder.Eval(Container.DataItem, "NUM_VALOR_MARKUP_16X_TOTAL")).ToString("0.00") %>'></asp:Label></strong></small>
                                                        </td>
                                                        <td class="text-right" style="min-width:35px;">
                                                            <small><strong><asp:TextBox runat="server" CssClass="form-control tab_taxas" id="txtmarkup17x" Text='<%# Convert.ToDecimal(DataBinder.Eval(Container.DataItem, "NUM_VALOR_MARKUP_17X")).ToString("0.00") %>'></asp:TextBox></strong></small>
                                                            <small><strong>T:<asp:Label runat="server" CssClass="tab_taxas" id="txtmarkup17xTOTAL" Text='<%# Convert.ToDecimal(DataBinder.Eval(Container.DataItem, "NUM_VALOR_MARKUP_17X_TOTAL")).ToString("0.00") %>'></asp:Label></strong></small>
                                                        </td>
                                                        <td class="text-right" style="min-width:35px;">
                                                            <small><strong><asp:TextBox runat="server" CssClass="form-control tab_taxas" id="txtmarkup18x" Text='<%# Convert.ToDecimal(DataBinder.Eval(Container.DataItem, "NUM_VALOR_MARKUP_18X")).ToString("0.00") %>'></asp:TextBox></strong></small>
                                                            <small><strong>T:<asp:Label runat="server" CssClass="tab_taxas" id="txtmarkup18xTOTAL" Text='<%# Convert.ToDecimal(DataBinder.Eval(Container.DataItem, "NUM_VALOR_MARKUP_18X_TOTAL")).ToString("0.00") %>'></asp:Label></strong></small>
                                                        </td>

                                                        <td class="text-right" style="min-width:35px;">
                                                            <small><strong><asp:TextBox runat="server" CssClass="form-control tab_taxas" id="txtmarkup19x" Text='<%# Convert.ToDecimal(DataBinder.Eval(Container.DataItem, "NUM_VALOR_MARKUP_19X")).ToString("0.00") %>'></asp:TextBox></strong></small>
                                                            <small><strong>T:<asp:Label runat="server" CssClass="tab_taxas" id="txtmarkup19xTOTAL" Text='<%# Convert.ToDecimal(DataBinder.Eval(Container.DataItem, "NUM_VALOR_MARKUP_19X_TOTAL")).ToString("0.00") %>'></asp:Label></strong></small>
                                                        </td>
                                                        <td class="text-right" style="min-width:35px;">
                                                            <small><strong><asp:TextBox runat="server" CssClass="form-control tab_taxas" id="txtmarkup20x" Text='<%# Convert.ToDecimal(DataBinder.Eval(Container.DataItem, "NUM_VALOR_MARKUP_20X")).ToString("0.00") %>'></asp:TextBox></strong></small>
                                                            <small><strong>T:<asp:Label runat="server" CssClass="tab_taxas" id="txtmarkup20xTOTAL" Text='<%# Convert.ToDecimal(DataBinder.Eval(Container.DataItem, "NUM_VALOR_MARKUP_20X_TOTAL")).ToString("0.00") %>'></asp:Label></strong></small>
                                                        </td>
                                                        <td class="text-right" style="min-width:35px;">
                                                            <small><strong><asp:TextBox runat="server" CssClass="form-control tab_taxas" id="txtmarkup21x" Text='<%# Convert.ToDecimal(DataBinder.Eval(Container.DataItem, "NUM_VALOR_MARKUP_21X")).ToString("0.00") %>'></asp:TextBox></strong></small>
                                                            <small><strong>T:<asp:Label runat="server" CssClass="tab_taxas" id="txtmarkup21xTOTAL" Text='<%# Convert.ToDecimal(DataBinder.Eval(Container.DataItem, "NUM_VALOR_MARKUP_21X_TOTAL")).ToString("0.00") %>'></asp:Label></strong></small>
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
                    </div>
                    <div class="card-footer">
                        <asp:Button runat="server" ID="btnSalvar" CssClass="btn btn-whitelabel1 float-left" Text="Salvar" onclick="btnSalvar_Click"/>
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


<script type="text/javascript">

$(document).ready(function () {
    var table = $('#tbConsultaMarkup').DataTable({
      "paging": false,
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

<script>
    function alterarColuna(index, valor) {
        // Seleciona a tabela
        var tabela = document.getElementById("tbConsultaMarkup");

        // Percorre todas as linhas do tbody
        for (var i = 1; i < tabela.rows.length; i++) {
            var cell = tabela.rows[i].cells[index]; // Obtém a célula da coluna selecionada
            if (cell) {
                var input = cell.querySelector("input"); // Busca o input dentro da célula
                if (input) {
                    var atual = parseFloat(input.value.replace(",", ".")) || 0; // Converte para número
                    var novoValor = (atual + valor).toFixed(2); // Aplica incremento/decremento
                    input.value = novoValor.replace(".", ","); // Atualiza o campo
                }
            }
        }
    }

    function alterarLinha(button, valor) {
        // Obtém a linha onde o botão foi clicado
        let row = button.closest("tr");

        // Pega todas as células da linha (ignorando a primeira que contém os botões)
        let cells = row.querySelectorAll("td input");

        cells.forEach((input) => {
            let atual = parseFloat(input.value.replace(",", ".")) || 0; // Converte para número
            let novoValor = (atual + valor).toFixed(2); // Aplica incremento/decremento
            input.value = novoValor.replace(".", ","); // Atualiza o campo
        });
    }
</script>


</body>
</html>
