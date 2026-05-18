<%@ Page Language="C#" AutoEventWireup="true" CodeFile="cad_planos.aspx.cs" Inherits="cad_planos" %>

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
            margin:4px;
            height: calc(1.50rem + 2px);
        }
        
        .tab_titulo
        {
            font-size:10px;
        }
        
        .btn_taxas
        {
            font-size:12px;
            padding:4px;
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
                            <asp:Label id="lblID" runat="server" CssClass="form-control"></asp:Label>
                        </div>
                        <div class="col-sm-3">
                            <label class="col-sm-12 col-form-label">Adquirente</label>
                            <asp:DropDownList runat="server" id="ddlAdquirentes" CssClass="form-control" 
                                AutoPostBack="True" onselectedindexchanged="ddlAdquirentes_SelectedIndexChanged">
                            </asp:DropDownList>
                        </div>

                        <div class="col-sm-3">
                            <label class="col-sm-12 col-form-label">Plano Referência</label>
                            <asp:DropDownList runat="server" id="ddlPlanosReferencia" CssClass="form-control">
                            </asp:DropDownList>
                        </div>
                        <div class="col-sm-3">
                            <label class="col-sm-12 col-form-label"><br /></label>
                            <asp:Button runat="server" ID="btnImportarTaxas" Text="Importar Taxas" 
                                CssClass="btn btn-whitelabel1" onclick="btnImportarTaxas_Click" 
                                Visible="False" />
                        </div>
                    </div>

                    <div runat="server" id="dvImportarExcel" visible="false">
                        <div class="form-group row col-12">
                            <div class="col-sm-9">
                                <div class="btn btn-default btn-sm float-right">
                                    <i class="fas fa-paperclip"></i> Anexo
                                    <input type="file" name="attachment" runat="server" id="File1" />
                                </div>
                            </div>
                            <div class="col-sm-3">
                                <asp:Button runat="server" ID="btnImportarTexto" CssClass="btn btn-whitelabel1" Text="Importar EXCEL" onclick="btnImportarTexto_Click"/>
                            </div>
                            <div class="col-sm-12">
                                <br />
                                <asp:GridView ID="GridView1" runat="server" Visible="false"></asp:GridView>
                            </div>
                        </div>
                    </div>

                    <div class="form-group row col-12">
                        <div class="col-sm-6">
                            <label class="col-sm-12 col-form-label">Nome</label>
                            <asp:TextBox id="txtNome" runat="server" CssClass="form-control" placeholder="Nome do Plano"></asp:TextBox>
                        </div>
                        <div class="col-sm-6">
                            <label class="col-sm-12 col-form-label">Descrição</label>
                            <asp:TextBox id="txtDescricao"  runat="server" CssClass="form-control" 
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
                            <asp:TextBox id="txtDias" runat="server" CssClass="form-control" placeholder="No. Dias para Liquidação" Visible="false"></asp:TextBox>
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
                            <asp:TextBox id="txtTaxaAntecipacao" runat="server" CssClass="form-control" placeholder="Percentual da Taxa de Antecipação"></asp:TextBox>
                        </div>
                    </div>

                </div>
                <div class="card-footer">
                        <asp:Button runat="server" ID="btnSalvar" CssClass="btn btn-whitelabel1" Text="Salvar" onclick="btnSalvar_Click"/>
                        <asp:Button runat="server" ID="btnGravar" CssClass="btn btn-whitelabel1" Text="Gravar Taxas" onclick="btnGravar_Click"/>

                        <asp:Button runat="server" ID="btnCancelar" CssClass="btn btn-danger float-right" Text="Fechar" onclick="btnCancelar_Click"/>
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
                                        <th>Vlr.Oper. (R$)</th>
                                        <th>Taxa (%)</th>
                                        <th>Markup (%)</th>
                                        <th>Rebate (%)</th>
                                        <th>Taxa Final (%)</th>
                                    </tr>
                                    </thead>
                                    <tbody>
                                    <asp:Repeater runat="server" ID="rptConsulta" >
                                        <ItemTemplate>

                                            <tr>
                                                <td>
                                                <asp:TextBox runat="server" id="txtidplano" Text='<%# DataBinder.Eval(Container.DataItem, "COD_ID_PLANOS")%>' visible="false"></asp:TextBox>
                                                <asp:TextBox runat="server" id="txtidplanoreferencia" Text='<%# DataBinder.Eval(Container.DataItem, "COD_ID_PLANOS_REFERENCIA")%>' visible="false"></asp:TextBox>
                                                <asp:TextBox runat="server" id="txtidbandeira" Text='<%# DataBinder.Eval(Container.DataItem, "COD_ID_BANDEIRAS")%>' visible="false"></asp:TextBox>
                                                <asp:TextBox runat="server" id="txtnomebandeira" Text='<%# DataBinder.Eval(Container.DataItem, "NOM_BANDEIRA")%>' visible="false"></asp:TextBox>
                                                <asp:TextBox runat="server" id="txtidtabela" Text='<%# DataBinder.Eval(Container.DataItem, "COD_ID_PLANOS_TABELA")%>' visible="false"></asp:TextBox>
                                                <asp:TextBox runat="server" id="txtparcini" Text='<%# DataBinder.Eval(Container.DataItem, "COD_PARCELA_INICIAL")%>' visible="false"></asp:TextBox>
                                                <asp:TextBox runat="server" id="txtparcfim" Text='<%# DataBinder.Eval(Container.DataItem, "COD_PARCELA_FINAL")%>' visible="false"></asp:TextBox>

                                                    <small><strong><%# DataBinder.Eval(Container.DataItem, "NUM_ORDEM_BANDEIRAS")%></strong></small>
                                                </td>

                                                <td class="text-center">
                                                    <small><strong><%# DataBinder.Eval(Container.DataItem, "NOM_BANDEIRA")%></strong></small><br />
                                                    <asp:Image runat="server" id="imgBandeira" CssClass="img-fluid img-thumbnail mx-auto" style="max-width:50px;" ImageUrl='<%# DataBinder.Eval(Container.DataItem, "NOM_IMAGEM")%>'></asp:Image><br /><br />
                                                </td>

                                                <td>
                                                    <small><strong><%# DataBinder.Eval(Container.DataItem, "NOM_TABELA")%></strong></small>
                                                </td>
                                                <td>
                                                    <small><asp:TextBox runat="server" id="txtValor" Text='<%# String.Format("{0:n2}",DataBinder.Eval(Container.DataItem, "NUM_VALOR"))%>' class="form-control" Enabled="false"></asp:TextBox></small>
                                                </td>

                                                <td>
                                                    <small><asp:TextBox runat="server" id="txtTaxa" Text='<%# String.Format("{0:n2}",DataBinder.Eval(Container.DataItem, "NUM_TAXA"))%>' class="form-control" Enabled="false"></asp:TextBox></small>
                                                </td>
                                                <td>
                                                    <small><asp:TextBox runat="server" id="txtMarkup" Text='<%# String.Format("{0:n2}",DataBinder.Eval(Container.DataItem, "NUM_MARKUP"))%>' class="form-control"></asp:TextBox></small>
                                                </td>

                                                <td>
                                                    <small><asp:TextBox runat="server" id="txtRebate" Text='<%# String.Format("{0:n2}",DataBinder.Eval(Container.DataItem, "NUM_REBATE"))%>' class="form-control"></asp:TextBox></small>
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
                    <h3 class="card-title">TAXAS & MARKUPS</h3>
                        <div class="card-tools">
                            <button type="button" class="btn btn-tool" data-card-widget="collapse">
                            <i class="fas fa-minus"></i>
                            </button>
                        </div>

                    </div>
                    <div class="card-body" style="overflow:scroll; height:800px;">

                        <div class="row">
                            <div class="col-12">
                                <div class="form-group row col-12">
                                    <div class="col-sm-6">
                                        <label class="col-sm-12 col-form-label">Selecione o campo que deseja adicionar (+) ou subtrair (-)</label>
                                        <asp:DropDownList runat="server" id="ddlCampos" CssClass="form-control">
                                            <asp:ListItem Value="txtcredito">Taxa</asp:ListItem>
                                            <asp:ListItem Value="txtmarkup">Markup</asp:ListItem>
                                            <asp:ListItem Value="txtrebate">Rebate</asp:ListItem>
                                        </asp:DropDownList>
                                    </div>
                                </div>

                            </div>
                        </div>


                        <div class="row">
                            <div class="col-12">
                                <table id="tbPresencial" class="table table-bordered table-hover table-striped">
                                    <thead>
                                    <tr>
                                        <th><h6><small class="tab_titulo"><b>Bandeira</b></small></h6></th>
                                        <th><h6><small class="tab_titulo"><b>Débito<br />
                                                    <button type="button" class="btn btn-sm btn_taxas btn-success" onclick="alterarColuna(1, 0.01)">+</button>
                                                    <button type="button" class="btn btn-sm btn_taxas btn-danger" onclick="alterarColuna(1, -0.01)">-</button></b></small></h6></th>
                                        <th><h6><small class="tab_titulo"><b>Cred.À Vista<br />
                                                    <button type="button" class="btn btn-sm btn_taxas btn-success" onclick="alterarColuna(2, 0.01)">+</button>
                                                    <button type="button" class="btn btn-sm btn_taxas btn-danger" onclick="alterarColuna(2, -0.01)">-</button></b></small></h6></th>
                                        <th><h6><small class="tab_titulo"><b>Cred.2x<br />
                                                    <button type="button" class="btn btn-sm btn_taxas btn-success" onclick="alterarColuna(3, 0.01)">+</button>
                                                    <button type="button" class="btn btn-sm btn_taxas btn-danger" onclick="alterarColuna(3, -0.01)">-</button></b></small></h6></th>
                                        <th><h6><small class="tab_titulo"><b>Cred.3x<br />
                                                    <button type="button" class="btn btn-sm btn_taxas btn-success" onclick="alterarColuna(4, 0.01)">+</button>
                                                    <button type="button" class="btn btn-sm btn_taxas btn-danger" onclick="alterarColuna(4, -0.01)">-</button></b></small></h6></th>
                                        <th><h6><small class="tab_titulo"><b>Cred.4x<br />
                                                    <button type="button" class="btn btn-sm btn_taxas btn-success" onclick="alterarColuna(5, 0.01)">+</button>
                                                    <button type="button" class="btn btn-sm btn_taxas btn-danger" onclick="alterarColuna(5, -0.01)">-</button></b></small></h6></th>
                                        <th><h6><small class="tab_titulo"><b>Cred.5x<br />
                                                    <button type="button" class="btn btn-sm btn_taxas btn-success" onclick="alterarColuna(6, 0.01)">+</button>
                                                    <button type="button" class="btn btn-sm btn_taxas btn-danger" onclick="alterarColuna(6, -0.01)">-</button></b></small></h6></th>
                                        <th><h6><small class="tab_titulo"><b>Cred.6x<br />
                                                    <button type="button" class="btn btn-sm btn_taxas btn-success" onclick="alterarColuna(7, 0.01)">+</button>
                                                    <button type="button" class="btn btn-sm btn_taxas btn-danger" onclick="alterarColuna(7, -0.01)">-</button></b></small></h6></th>
                                        <th><h6><small class="tab_titulo"><b>Cred.7x<br />
                                                    <button type="button" class="btn btn-sm btn_taxas btn-success" onclick="alterarColuna(8, 0.01)">+</button>
                                                    <button type="button" class="btn btn-sm btn_taxas btn-danger" onclick="alterarColuna(8, -0.01)">-</button></b></small></h6></th>
                                        <th><h6><small class="tab_titulo"><b>Cred.8x<br />
                                                    <button type="button" class="btn btn-sm btn_taxas btn-success" onclick="alterarColuna(9, 0.01)">+</button>
                                                    <button type="button" class="btn btn-sm btn_taxas btn-danger" onclick="alterarColuna(9, -0.01)">-</button></b></small></h6></th>
                                        <th><h6><small class="tab_titulo"><b>Cred.9x<br />
                                                    <button type="button" class="btn btn-sm btn_taxas btn-success" onclick="alterarColuna(10, 0.01)">+</button>
                                                    <button type="button" class="btn btn-sm btn_taxas btn-danger" onclick="alterarColuna(10, -0.01)">-</button></b></small></h6></th>
                                        <th><h6><small class="tab_titulo"><b>Cred.10x<br />
                                                    <button type="button" class="btn btn-sm btn_taxas btn-success" onclick="alterarColuna(11, 0.01)">+</button>
                                                    <button type="button" class="btn btn-sm btn_taxas btn-danger" onclick="alterarColuna(11, -0.01)">-</button></b></small></h6></th>
                                        <th><h6><small class="tab_titulo"><b>Cred.11x<br />
                                                    <button type="button" class="btn btn-sm btn_taxas btn-success" onclick="alterarColuna(12, 0.01)">+</button>
                                                    <button type="button" class="btn btn-sm btn_taxas btn-danger" onclick="alterarColuna(12, -0.01)">-</button></b></small></h6></th>
                                        <th><h6><small class="tab_titulo"><b>Cred.12x<br />
                                                    <button type="button" class="btn btn-sm btn_taxas btn-success" onclick="alterarColuna(13, 0.01)">+</button>
                                                    <button type="button" class="btn btn-sm btn_taxas btn-danger" onclick="alterarColuna(13, -0.01)">-</button></b></small></h6></th>
                                        <th><h6><small class="tab_titulo"><b>Cred.13x<br />
                                                    <button type="button" class="btn btn-sm btn_taxas btn-success" onclick="alterarColuna(14, 0.01)">+</button>
                                                    <button type="button" class="btn btn-sm btn_taxas btn-danger" onclick="alterarColuna(14, -0.01)">-</button></b></small></h6></th>
                                        <th><h6><small class="tab_titulo"><b>Cred.14x<br />
                                                    <button type="button" class="btn btn-sm btn_taxas btn-success" onclick="alterarColuna(15, 0.01)">+</button>
                                                    <button type="button" class="btn btn-sm btn_taxas btn-danger" onclick="alterarColuna(15, -0.01)">-</button></b></small></h6></th>
                                        <th><h6><small class="tab_titulo"><b>Cred.15x<br />
                                                    <button type="button" class="btn btn-sm btn_taxas btn-success" onclick="alterarColuna(16, 0.01)">+</button>
                                                    <button type="button" class="btn btn-sm btn_taxas btn-danger" onclick="alterarColuna(16, -0.01)">-</button></b></small></h6></th>
                                        <th><h6><small class="tab_titulo"><b>Cred.16x<br />
                                                    <button type="button" class="btn btn-sm btn_taxas btn-success" onclick="alterarColuna(17, 0.01)">+</button>
                                                    <button type="button" class="btn btn-sm btn_taxas btn-danger" onclick="alterarColuna(17, -0.01)">-</button></b></small></h6></th>
                                        <th><h6><small class="tab_titulo"><b>Cred.17x<br />
                                                    <button type="button" class="btn btn-sm btn_taxas btn-success" onclick="alterarColuna(18, 0.01)">+</button>
                                                    <button type="button" class="btn btn-sm btn_taxas btn-danger" onclick="alterarColuna(18, -0.01)">-</button></b></small></h6></th>
                                        <th><h6><small class="tab_titulo"><b>Cred.18x<br />
                                                    <button type="button" class="btn btn-sm btn_taxas btn-success" onclick="alterarColuna(19, 0.01)">+</button>
                                                    <button type="button" class="btn btn-sm btn_taxas btn-danger" onclick="alterarColuna(19, -0.01)">-</button></b></small></h6></th>
                                        <th><h6><small class="tab_titulo"><b>Cred.19x<br />
                                                    <button type="button" class="btn btn-sm btn_taxas btn-success" onclick="alterarColuna(20, 0.01)">+</button>
                                                    <button type="button" class="btn btn-sm btn_taxas btn-danger" onclick="alterarColuna(20, -0.01)">-</button></b></small></h6></th>
                                        <th><h6><small class="tab_titulo"><b>Cred.20x<br />
                                                    <button type="button" class="btn btn-sm btn_taxas btn-success" onclick="alterarColuna(21, 0.01)">+</button>
                                                    <button type="button" class="btn btn-sm btn_taxas btn-danger" onclick="alterarColuna(21, -0.01)">-</button></b></small></h6></th>
                                        <th><h6><small class="tab_titulo"><b>Cred.21x<br />
                                                    <button type="button" class="btn btn-sm btn_taxas btn-success" onclick="alterarColuna(22, 0.01)">+</button>
                                                    <button type="button" class="btn btn-sm btn_taxas btn-danger" onclick="alterarColuna(22, -0.01)">-</button></b></small></h6></th>
                                    </tr>
                                    </thead>
                                    <tbody>
                                    <asp:Repeater runat="server" ID="rptConsultaParcelas" >
                                        <ItemTemplate>

                                            <tr>
                                                <td class="text-center">
                                                <asp:TextBox runat="server" id="txtidplano" Text='<%# DataBinder.Eval(Container.DataItem, "COD_ID_PLANOS")%>' visible="false"></asp:TextBox>
                                                <asp:TextBox runat="server" id="txtidbandeira" Text='<%# DataBinder.Eval(Container.DataItem, "COD_ID_BANDEIRAS")%>' visible="false"></asp:TextBox>
                                                <asp:TextBox runat="server" id="txtnomebandeira" Text='<%# DataBinder.Eval(Container.DataItem, "NOM_BANDEIRA")%>' visible="false"></asp:TextBox>

                                                    <small class="tab_titulo"><strong>Vlr.Oper.(R$)</strong></small><br />
                                                    <small class="tab_titulo"><strong>Taxa (%)</strong></small><br /><br />
                                                    <small class="tab_titulo"><strong><%# DataBinder.Eval(Container.DataItem, "NOM_BANDEIRA")%></strong></small><br />
                                                    <asp:Image runat="server" id="imgBandeira" CssClass="img-fluid img-thumbnail mx-auto" style="max-width:50px;" ImageUrl='<%# DataBinder.Eval(Container.DataItem, "NOM_IMAGEM")%>'></asp:Image><br /><br />
                                                    <small class="tab_titulo"><strong>Markup (R$)</strong></small><br />
                                                    <small class="tab_titulo"><strong>Markup (%)</strong></small><br />
                                                    <small class="tab_titulo"><strong>Rebate (%)</strong></small><br /><br />
                                                    <small class="tab_titulo"><strong>Tx.Final (%)</strong></small>
                                                    <small class="tab_titulo"><strong>Tx.Final (R$)</strong></small>

                                                </td>

                                                <td class="text-center" style="min-width:70px;" >
                                                    <small><strong><asp:TextBox runat="server" CssClass="form-control tab_taxas" id="txtdebitofixo" Text='<%# String.Format("{0:n2}",DataBinder.Eval(Container.DataItem, "NUM_VALOR_FIXO_DEBITO"))%>' Enabled="false"></asp:TextBox></strong></small>
                                                    <small><strong><asp:TextBox runat="server" CssClass="form-control tab_taxas" id="txtdebito" Text='<%# String.Format("{0:n2}",DataBinder.Eval(Container.DataItem, "NUM_VALOR_DEBITO"))%>' Enabled="false"></asp:TextBox></strong></small><br />
                                                    <small><strong><asp:TextBox runat="server" CssClass="form-control tab_taxas" id="txtmarkupdebitofixo" Text='<%# String.Format("{0:n2}",DataBinder.Eval(Container.DataItem, "NUM_VALOR_FIXO_MARKUP_DEBITO"))%>'></asp:TextBox></strong></small>
                                                    <small><strong><asp:TextBox runat="server" CssClass="form-control tab_taxas" id="txtmarkupdebito" Text='<%# String.Format("{0:n2}",DataBinder.Eval(Container.DataItem, "NUM_VALOR_MARKUP_DEBITO"))%>'></asp:TextBox></strong></small>
                                                    <small><strong><asp:TextBox runat="server" CssClass="form-control tab_taxas" id="txtrebatedebito" Text='<%# String.Format("{0:n2}",DataBinder.Eval(Container.DataItem, "NUM_VALOR_REBATE_DEBITO"))%>'></asp:TextBox></strong></small><br />
                                                    <small><strong><asp:Label runat="server" id="lblfinaldebito" Text='<%# String.Format("{0:n2}",DataBinder.Eval(Container.DataItem, "NUM_VALOR_FINAL_DEBITO"))%>'></asp:Label></strong></small>
                                                    <small><strong><asp:Label runat="server" id="Label22" Text='<%# String.Format("{0:n2}",DataBinder.Eval(Container.DataItem, "NUM_VALOR_FINAL_FIXO_DEBITO"))%>'></asp:Label></strong></small>
                                                </td                                                </td>
                                                <td class="text-center" style="min-width:70px;" >
                                                    <small><strong><asp:TextBox runat="server" CssClass="form-control tab_taxas" id="txtcreditofixo" Text='<%# String.Format("{0:n2}",DataBinder.Eval(Container.DataItem, "NUM_VALOR_FIXO_CREDITO_VISTA"))%>' Enabled="false" ></asp:TextBox></strong></small>
                                                    <small><strong><asp:TextBox runat="server" CssClass="form-control tab_taxas" id="txtcredito" Text='<%# String.Format("{0:n2}",DataBinder.Eval(Container.DataItem, "NUM_VALOR_CREDITO_VISTA"))%>' Enabled="false" ></asp:TextBox></strong></small><br />
                                                    <small><strong><asp:TextBox runat="server" CssClass="form-control tab_taxas" id="txtmarkupcreditofixo" Text='<%# String.Format("{0:n2}",DataBinder.Eval(Container.DataItem, "NUM_VALOR_FIXO_MARKUP_VISTA"))%>'></asp:TextBox></strong></small>
                                                    <small><strong><asp:TextBox runat="server" CssClass="form-control tab_taxas" id="txtmarkupcredito" Text='<%# String.Format("{0:n2}",DataBinder.Eval(Container.DataItem, "NUM_VALOR_MARKUP_VISTA"))%>'></asp:TextBox></strong></small>
                                                    <small><strong><asp:TextBox runat="server" CssClass="form-control tab_taxas" id="txtrebatevista" Text='<%# String.Format("{0:n2}",DataBinder.Eval(Container.DataItem, "NUM_VALOR_REBATE_VISTA"))%>'></asp:TextBox></strong></small><br />
                                                    <small><strong><asp:Label runat="server" id="Label1" Text='<%# String.Format("{0:n2}",DataBinder.Eval(Container.DataItem, "NUM_VALOR_FINAL_VISTA"))%>'></asp:Label></strong></small>
                                                    <small><strong><asp:Label runat="server" id="Label23" Text='<%# String.Format("{0:n2}",DataBinder.Eval(Container.DataItem, "NUM_VALOR_FINAL_FIXO_VISTA"))%>'></asp:Label></strong></small>
                                                </td>
                                                <td class="text-center" style="min-width:70px;" >
                                                    <small><strong><asp:TextBox runat="server" CssClass="form-control tab_taxas" id="txtcredito2xfixo" Text='<%# String.Format("{0:n2}",DataBinder.Eval(Container.DataItem, "NUM_VALOR_FIXO_CREDITO_2X"))%>' Enabled="false"></asp:TextBox></strong></small>
                                                    <small><strong><asp:TextBox runat="server" CssClass="form-control tab_taxas" id="txtcredito2x" Text='<%# String.Format("{0:n2}",DataBinder.Eval(Container.DataItem, "NUM_VALOR_CREDITO_2X"))%>' Enabled="false"></asp:TextBox></strong></small><br />
                                                    <small><strong><asp:TextBox runat="server" CssClass="form-control tab_taxas" id="txtmarkup2xfixo" Text='<%# String.Format("{0:n2}",DataBinder.Eval(Container.DataItem, "NUM_VALOR_FIXO_MARKUP_2X"))%>'></asp:TextBox></strong></small>
                                                    <small><strong><asp:TextBox runat="server" CssClass="form-control tab_taxas" id="txtmarkup2x" Text='<%# String.Format("{0:n2}",DataBinder.Eval(Container.DataItem, "NUM_VALOR_MARKUP_2X"))%>'></asp:TextBox></strong></small>
                                                    <small><strong><asp:TextBox runat="server" CssClass="form-control tab_taxas" id="txtrebate2x" Text='<%# String.Format("{0:n2}",DataBinder.Eval(Container.DataItem, "NUM_VALOR_REBATE_2X"))%>'></asp:TextBox></strong></small><br />
                                                    <small><strong><asp:Label runat="server" id="Label2" Text='<%# String.Format("{0:n2}",DataBinder.Eval(Container.DataItem, "NUM_VALOR_FINAL_2X"))%>'></asp:Label></strong></small>
                                                    <small><strong><asp:Label runat="server" id="Label24" Text='<%# String.Format("{0:n2}",DataBinder.Eval(Container.DataItem, "NUM_VALOR_FINAL_FIXO_2X"))%>'></asp:Label></strong></small>
                                                </td>
                                                <td class="text-center" style="min-width:70px;" >
                                                    <small><strong><asp:TextBox runat="server" CssClass="form-control tab_taxas" id="txtcredito3xfixo" Text='<%# String.Format("{0:n2}",DataBinder.Eval(Container.DataItem, "NUM_VALOR_FIXO_CREDITO_3X"))%>' Enabled="false"></asp:TextBox></strong></small>
                                                    <small><strong><asp:TextBox runat="server" CssClass="form-control tab_taxas" id="txtcredito3x" Text='<%# String.Format("{0:n2}",DataBinder.Eval(Container.DataItem, "NUM_VALOR_CREDITO_3X"))%>' Enabled="false"></asp:TextBox></strong></small><br />
                                                    <small><strong><asp:TextBox runat="server" CssClass="form-control tab_taxas" id="txtmarkup3xfixo" Text='<%# String.Format("{0:n2}",DataBinder.Eval(Container.DataItem, "NUM_VALOR_FIXO_MARKUP_3X"))%>'></asp:TextBox></strong></small>
                                                    <small><strong><asp:TextBox runat="server" CssClass="form-control tab_taxas" id="txtmarkup3x" Text='<%# String.Format("{0:n2}",DataBinder.Eval(Container.DataItem, "NUM_VALOR_MARKUP_3X"))%>'></asp:TextBox></strong></small>
                                                    <small><strong><asp:TextBox runat="server" CssClass="form-control tab_taxas" id="txtrebate3x" Text='<%# String.Format("{0:n2}",DataBinder.Eval(Container.DataItem, "NUM_VALOR_REBATE_3X"))%>'></asp:TextBox></strong></small><br />
                                                    <small><strong><asp:Label runat="server" id="Label3" Text='<%# String.Format("{0:n2}",DataBinder.Eval(Container.DataItem, "NUM_VALOR_FINAL_3X"))%>'></asp:Label></strong></small>
                                                    <small><strong><asp:Label runat="server" id="Label25" Text='<%# String.Format("{0:n2}",DataBinder.Eval(Container.DataItem, "NUM_VALOR_FINAL_FIXO_3X"))%>'></asp:Label></strong></small>
                                                </td>
                                                <td class="text-center" style="min-width:70px;" >
                                                    <small><strong><asp:TextBox runat="server" CssClass="form-control tab_taxas" id="txtcredito4xfixo" Text='<%# String.Format("{0:n2}",DataBinder.Eval(Container.DataItem, "NUM_VALOR_FIXO_CREDITO_4X"))%>' Enabled="false"></asp:TextBox></strong></small>
                                                    <small><strong><asp:TextBox runat="server" CssClass="form-control tab_taxas" id="txtcredito4x" Text='<%# String.Format("{0:n2}",DataBinder.Eval(Container.DataItem, "NUM_VALOR_CREDITO_4X"))%>' Enabled="false"></asp:TextBox></strong></small><br />
                                                    <small><strong><asp:TextBox runat="server" CssClass="form-control tab_taxas" id="txtmarkup4xfixo" Text='<%# String.Format("{0:n2}",DataBinder.Eval(Container.DataItem, "NUM_VALOR_FIXO_MARKUP_4X"))%>'></asp:TextBox></strong></small>
                                                    <small><strong><asp:TextBox runat="server" CssClass="form-control tab_taxas" id="txtmarkup4x" Text='<%# String.Format("{0:n2}",DataBinder.Eval(Container.DataItem, "NUM_VALOR_MARKUP_4X"))%>'></asp:TextBox></strong></small>
                                                    <small><strong><asp:TextBox runat="server" CssClass="form-control tab_taxas" id="txtrebate4x" Text='<%# String.Format("{0:n2}",DataBinder.Eval(Container.DataItem, "NUM_VALOR_REBATE_4X"))%>'></asp:TextBox></strong></small><br />
                                                    <small><strong><asp:Label runat="server" id="Label4" Text='<%# String.Format("{0:n2}",DataBinder.Eval(Container.DataItem, "NUM_VALOR_FINAL_4X"))%>'></asp:Label></strong></small>
                                                    <small><strong><asp:Label runat="server" id="Label26" Text='<%# String.Format("{0:n2}",DataBinder.Eval(Container.DataItem, "NUM_VALOR_FINAL_FIXO_4X"))%>'></asp:Label></strong></small>
                                                </td>
                                                <td class="text-center" style="min-width:70px;" >
                                                    <small><strong><asp:TextBox runat="server" CssClass="form-control tab_taxas" id="txtcredito5xfixo" Text='<%# String.Format("{0:n2}",DataBinder.Eval(Container.DataItem, "NUM_VALOR_FIXO_CREDITO_5X"))%>' Enabled="false"></asp:TextBox></strong></small>
                                                    <small><strong><asp:TextBox runat="server" CssClass="form-control tab_taxas" id="txtcredito5x" Text='<%# String.Format("{0:n2}",DataBinder.Eval(Container.DataItem, "NUM_VALOR_CREDITO_5X"))%>' Enabled="false"></asp:TextBox></strong></small><br />
                                                    <small><strong><asp:TextBox runat="server" CssClass="form-control tab_taxas" id="txtmarkup5xfixo" Text='<%# String.Format("{0:n2}",DataBinder.Eval(Container.DataItem, "NUM_VALOR_FIXO_MARKUP_5X"))%>'></asp:TextBox></strong></small>
                                                    <small><strong><asp:TextBox runat="server" CssClass="form-control tab_taxas" id="txtmarkup5x" Text='<%# String.Format("{0:n2}",DataBinder.Eval(Container.DataItem, "NUM_VALOR_MARKUP_5X"))%>'></asp:TextBox></strong></small>
                                                    <small><strong><asp:TextBox runat="server" CssClass="form-control tab_taxas" id="txtrebate5x" Text='<%# String.Format("{0:n2}",DataBinder.Eval(Container.DataItem, "NUM_VALOR_REBATE_5X"))%>'></asp:TextBox></strong></small><br />
                                                    <small><strong><asp:Label runat="server" id="Label5" Text='<%# String.Format("{0:n2}",DataBinder.Eval(Container.DataItem, "NUM_VALOR_FINAL_5X"))%>'></asp:Label></strong></small>
                                                    <small><strong><asp:Label runat="server" id="Label27" Text='<%# String.Format("{0:n2}",DataBinder.Eval(Container.DataItem, "NUM_VALOR_FINAL_FIXO_5X"))%>'></asp:Label></strong></small>
                                                </td>
                                                <td class="text-center" style="min-width:70px;" >
                                                    <small><strong><asp:TextBox runat="server" CssClass="form-control tab_taxas" id="txtcredito6xfixo" Text='<%# String.Format("{0:n2}",DataBinder.Eval(Container.DataItem, "NUM_VALOR_FIXO_CREDITO_6X"))%>' Enabled="false"></asp:TextBox></strong></small>
                                                    <small><strong><asp:TextBox runat="server" CssClass="form-control tab_taxas" id="txtcredito6x" Text='<%# String.Format("{0:n2}",DataBinder.Eval(Container.DataItem, "NUM_VALOR_CREDITO_6X"))%>' Enabled="false"></asp:TextBox></strong></small><br />
                                                    <small><strong><asp:TextBox runat="server" CssClass="form-control tab_taxas" id="txtmarkup6xfixo" Text='<%# String.Format("{0:n2}",DataBinder.Eval(Container.DataItem, "NUM_VALOR_FIXO_MARKUP_6X"))%>'></asp:TextBox></strong></small>
                                                    <small><strong><asp:TextBox runat="server" CssClass="form-control tab_taxas" id="txtmarkup6x" Text='<%# String.Format("{0:n2}",DataBinder.Eval(Container.DataItem, "NUM_VALOR_MARKUP_6X"))%>'></asp:TextBox></strong></small>
                                                    <small><strong><asp:TextBox runat="server" CssClass="form-control tab_taxas" id="txtrebate6x" Text='<%# String.Format("{0:n2}",DataBinder.Eval(Container.DataItem, "NUM_VALOR_REBATE_6X"))%>'></asp:TextBox></strong></small><br />
                                                    <small><strong><asp:Label runat="server" id="Label6" Text='<%# String.Format("{0:n2}",DataBinder.Eval(Container.DataItem, "NUM_VALOR_FINAL_6X"))%>'></asp:Label></strong></small>
                                                    <small><strong><asp:Label runat="server" id="Label28" Text='<%# String.Format("{0:n2}",DataBinder.Eval(Container.DataItem, "NUM_VALOR_FINAL_FIXO_6X"))%>'></asp:Label></strong></small>
                                                </td>
                                                <td class="text-center" style="min-width:70px;" >
                                                    <small><strong><asp:TextBox runat="server" CssClass="form-control tab_taxas" id="txtcredito7xfixo" Text='<%# String.Format("{0:n2}",DataBinder.Eval(Container.DataItem, "NUM_VALOR_FIXO_CREDITO_7X"))%>' Enabled="false"></asp:TextBox></strong></small>
                                                    <small><strong><asp:TextBox runat="server" CssClass="form-control tab_taxas" id="txtcredito7x" Text='<%# String.Format("{0:n2}",DataBinder.Eval(Container.DataItem, "NUM_VALOR_CREDITO_7X"))%>' Enabled="false"></asp:TextBox></strong></small><br />
                                                    <small><strong><asp:TextBox runat="server" CssClass="form-control tab_taxas" id="txtmarkup7xfixo" Text='<%# String.Format("{0:n2}",DataBinder.Eval(Container.DataItem, "NUM_VALOR_FIXO_MARKUP_7X"))%>'></asp:TextBox></strong></small>
                                                    <small><strong><asp:TextBox runat="server" CssClass="form-control tab_taxas" id="txtmarkup7x" Text='<%# String.Format("{0:n2}",DataBinder.Eval(Container.DataItem, "NUM_VALOR_MARKUP_7X"))%>'></asp:TextBox></strong></small>
                                                    <small><strong><asp:TextBox runat="server" CssClass="form-control tab_taxas" id="txtrebate7x" Text='<%# String.Format("{0:n2}",DataBinder.Eval(Container.DataItem, "NUM_VALOR_REBATE_7X"))%>'></asp:TextBox></strong></small><br />
                                                    <small><strong><asp:Label runat="server" id="Label7" Text='<%# String.Format("{0:n2}",DataBinder.Eval(Container.DataItem, "NUM_VALOR_FINAL_7X"))%>'></asp:Label></strong></small>
                                                    <small><strong><asp:Label runat="server" id="Label29" Text='<%# String.Format("{0:n2}",DataBinder.Eval(Container.DataItem, "NUM_VALOR_FINAL_FIXO_7X"))%>'></asp:Label></strong></small>
                                                </td>
                                                <td class="text-center" style="min-width:70px;" >
                                                    <small><strong><asp:TextBox runat="server" CssClass="form-control tab_taxas" id="txtcredito8xfixo" Text='<%# String.Format("{0:n2}",DataBinder.Eval(Container.DataItem, "NUM_VALOR_FIXO_CREDITO_8X"))%>' Enabled="false"></asp:TextBox></strong></small>
                                                    <small><strong><asp:TextBox runat="server" CssClass="form-control tab_taxas" id="txtcredito8x" Text='<%# String.Format("{0:n2}",DataBinder.Eval(Container.DataItem, "NUM_VALOR_CREDITO_8X"))%>' Enabled="false"></asp:TextBox></strong></small><br />
                                                    <small><strong><asp:TextBox runat="server" CssClass="form-control tab_taxas" id="txtmarkup8xfixo" Text='<%# String.Format("{0:n2}",DataBinder.Eval(Container.DataItem, "NUM_VALOR_FIXO_MARKUP_8X"))%>'></asp:TextBox></strong></small>
                                                    <small><strong><asp:TextBox runat="server" CssClass="form-control tab_taxas" id="txtmarkup8x" Text='<%# String.Format("{0:n2}",DataBinder.Eval(Container.DataItem, "NUM_VALOR_MARKUP_8X"))%>'></asp:TextBox></strong></small>
                                                    <small><strong><asp:TextBox runat="server" CssClass="form-control tab_taxas" id="txtrebate8x" Text='<%# String.Format("{0:n2}",DataBinder.Eval(Container.DataItem, "NUM_VALOR_REBATE_8X"))%>'></asp:TextBox></strong></small><br />
                                                    <small><strong><asp:Label runat="server" id="Label8" Text='<%# String.Format("{0:n2}",DataBinder.Eval(Container.DataItem, "NUM_VALOR_FINAL_8X"))%>'></asp:Label></strong></small>
                                                    <small><strong><asp:Label runat="server" id="Label30" Text='<%# String.Format("{0:n2}",DataBinder.Eval(Container.DataItem, "NUM_VALOR_FINAL_FIXO_8X"))%>'></asp:Label></strong></small>
                                                </td>
                                                <td class="text-center" style="min-width:70px;" >
                                                    <small><strong><asp:TextBox runat="server" CssClass="form-control tab_taxas" id="txtcredito9xfixo" Text='<%# String.Format("{0:n2}",DataBinder.Eval(Container.DataItem, "NUM_VALOR_FIXO_CREDITO_9X"))%>' Enabled="false"></asp:TextBox></strong></small>
                                                    <small><strong><asp:TextBox runat="server" CssClass="form-control tab_taxas" id="txtcredito9x" Text='<%# String.Format("{0:n2}",DataBinder.Eval(Container.DataItem, "NUM_VALOR_CREDITO_9X"))%>' Enabled="false"></asp:TextBox></strong></small><br />
                                                    <small><strong><asp:TextBox runat="server" CssClass="form-control tab_taxas" id="txtmarkup9xfixo" Text='<%# String.Format("{0:n2}",DataBinder.Eval(Container.DataItem, "NUM_VALOR_FIXO_MARKUP_9X"))%>'></asp:TextBox></strong></small>
                                                    <small><strong><asp:TextBox runat="server" CssClass="form-control tab_taxas" id="txtmarkup9x" Text='<%# String.Format("{0:n2}",DataBinder.Eval(Container.DataItem, "NUM_VALOR_MARKUP_9X"))%>'></asp:TextBox></strong></small>
                                                    <small><strong><asp:TextBox runat="server" CssClass="form-control tab_taxas" id="txtrebate9x" Text='<%# String.Format("{0:n2}",DataBinder.Eval(Container.DataItem, "NUM_VALOR_REBATE_9X"))%>'></asp:TextBox></strong></small><br />
                                                    <small><strong><asp:Label runat="server" id="Label9" Text='<%# String.Format("{0:n2}",DataBinder.Eval(Container.DataItem, "NUM_VALOR_FINAL_9X"))%>'></asp:Label></strong></small>
                                                    <small><strong><asp:Label runat="server" id="Label31" Text='<%# String.Format("{0:n2}",DataBinder.Eval(Container.DataItem, "NUM_VALOR_FINAL_FIXO_9X"))%>'></asp:Label></strong></small>
                                                </td>
                                                <td class="text-center" style="min-width:70px;" >
                                                    <small><strong><asp:TextBox runat="server" CssClass="form-control tab_taxas" id="txtcredito10xfixo" Text='<%# String.Format("{0:n2}",DataBinder.Eval(Container.DataItem, "NUM_VALOR_FIXO_CREDITO_10X"))%>' Enabled="false"></asp:TextBox></strong></small>
                                                    <small><strong><asp:TextBox runat="server" CssClass="form-control tab_taxas" id="txtcredito10x" Text='<%# String.Format("{0:n2}",DataBinder.Eval(Container.DataItem, "NUM_VALOR_CREDITO_10X"))%>' Enabled="false"></asp:TextBox></strong></small><br />
                                                    <small><strong><asp:TextBox runat="server" CssClass="form-control tab_taxas" id="txtmarkup10xfixo" Text='<%# String.Format("{0:n2}",DataBinder.Eval(Container.DataItem, "NUM_VALOR_FIXO_MARKUP_10X"))%>'></asp:TextBox></strong></small>
                                                    <small><strong><asp:TextBox runat="server" CssClass="form-control tab_taxas" id="txtmarkup10x" Text='<%# String.Format("{0:n2}",DataBinder.Eval(Container.DataItem, "NUM_VALOR_MARKUP_10X"))%>'></asp:TextBox></strong></small>
                                                    <small><strong><asp:TextBox runat="server" CssClass="form-control tab_taxas" id="txtrebate10x" Text='<%# String.Format("{0:n2}",DataBinder.Eval(Container.DataItem, "NUM_VALOR_REBATE_10X"))%>'></asp:TextBox></strong></small><br />
                                                    <small><strong><asp:Label runat="server" id="Label10" Text='<%# String.Format("{0:n2}",DataBinder.Eval(Container.DataItem, "NUM_VALOR_FINAL_10X"))%>'></asp:Label></strong></small>
                                                    <small><strong><asp:Label runat="server" id="Label32" Text='<%# String.Format("{0:n2}",DataBinder.Eval(Container.DataItem, "NUM_VALOR_FINAL_FIXO_10X"))%>'></asp:Label></strong></small>
                                                </td>
                                                <td class="text-center" style="min-width:70px;" >
                                                    <small><strong><asp:TextBox runat="server" CssClass="form-control tab_taxas" id="txtcredito11xfixo" Text='<%# String.Format("{0:n2}",DataBinder.Eval(Container.DataItem, "NUM_VALOR_FIXO_CREDITO_11X"))%>' Enabled="false"></asp:TextBox></strong></small>
                                                    <small><strong><asp:TextBox runat="server" CssClass="form-control tab_taxas" id="txtcredito11x" Text='<%# String.Format("{0:n2}",DataBinder.Eval(Container.DataItem, "NUM_VALOR_CREDITO_11X"))%>' Enabled="false"></asp:TextBox></strong></small><br />
                                                    <small><strong><asp:TextBox runat="server" CssClass="form-control tab_taxas" id="txtmarkup11xfixo" Text='<%# String.Format("{0:n2}",DataBinder.Eval(Container.DataItem, "NUM_VALOR_FIXO_MARKUP_11X"))%>'></asp:TextBox></strong></small>
                                                    <small><strong><asp:TextBox runat="server" CssClass="form-control tab_taxas" id="txtmarkup11x" Text='<%# String.Format("{0:n2}",DataBinder.Eval(Container.DataItem, "NUM_VALOR_MARKUP_11X"))%>'></asp:TextBox></strong></small>
                                                    <small><strong><asp:TextBox runat="server" CssClass="form-control tab_taxas" id="txtrebate11x" Text='<%# String.Format("{0:n2}",DataBinder.Eval(Container.DataItem, "NUM_VALOR_REBATE_11X"))%>'></asp:TextBox></strong></small><br />
                                                    <small><strong><asp:Label runat="server" id="Label11" Text='<%# String.Format("{0:n2}",DataBinder.Eval(Container.DataItem, "NUM_VALOR_FINAL_11X"))%>'></asp:Label></strong></small>
                                                    <small><strong><asp:Label runat="server" id="Label33" Text='<%# String.Format("{0:n2}",DataBinder.Eval(Container.DataItem, "NUM_VALOR_FINAL_FIXO_11X"))%>'></asp:Label></strong></small>
                                                </td>
                                                <td class="text-center" style="min-width:70px;" >
                                                    <small><strong><asp:TextBox runat="server" CssClass="form-control tab_taxas" id="txtcredito12xfixo" Text='<%# String.Format("{0:n2}",DataBinder.Eval(Container.DataItem, "NUM_VALOR_FIXO_CREDITO_12X"))%>' Enabled="false"></asp:TextBox></strong></small>
                                                    <small><strong><asp:TextBox runat="server" CssClass="form-control tab_taxas" id="txtcredito12x" Text='<%# String.Format("{0:n2}",DataBinder.Eval(Container.DataItem, "NUM_VALOR_CREDITO_12X"))%>' Enabled="false"></asp:TextBox></strong></small><br />
                                                    <small><strong><asp:TextBox runat="server" CssClass="form-control tab_taxas" id="txtmarkup12xfixo" Text='<%# String.Format("{0:n2}",DataBinder.Eval(Container.DataItem, "NUM_VALOR_FIXO_MARKUP_12X"))%>'></asp:TextBox></strong></small>
                                                    <small><strong><asp:TextBox runat="server" CssClass="form-control tab_taxas" id="txtmarkup12x" Text='<%# String.Format("{0:n2}",DataBinder.Eval(Container.DataItem, "NUM_VALOR_MARKUP_12X"))%>'></asp:TextBox></strong></small>
                                                    <small><strong><asp:TextBox runat="server" CssClass="form-control tab_taxas" id="txtrebate12x" Text='<%# String.Format("{0:n2}",DataBinder.Eval(Container.DataItem, "NUM_VALOR_REBATE_12X"))%>'></asp:TextBox></strong></small><br />
                                                    <small><strong><asp:Label runat="server" id="Label12" Text='<%# String.Format("{0:n2}",DataBinder.Eval(Container.DataItem, "NUM_VALOR_FINAL_12X"))%>'></asp:Label></strong></small>
                                                    <small><strong><asp:Label runat="server" id="Label34" Text='<%# String.Format("{0:n2}",DataBinder.Eval(Container.DataItem, "NUM_VALOR_FINAL_FIXO_12X"))%>'></asp:Label></strong></small>
                                                </td>
                                                <td class="text-center" style="min-width:70px;" >
                                                    <small><strong><asp:TextBox runat="server" CssClass="form-control tab_taxas" id="txtcredito13xfixo" Text='<%# String.Format("{0:n2}",DataBinder.Eval(Container.DataItem, "NUM_VALOR_FIXO_CREDITO_13X"))%>' Enabled="false"></asp:TextBox></strong></small>
                                                    <small><strong><asp:TextBox runat="server" CssClass="form-control tab_taxas" id="txtcredito13x" Text='<%# String.Format("{0:n2}",DataBinder.Eval(Container.DataItem, "NUM_VALOR_CREDITO_13X"))%>' Enabled="false"></asp:TextBox></strong></small><br />
                                                    <small><strong><asp:TextBox runat="server" CssClass="form-control tab_taxas" id="txtmarkup13xfixo" Text='<%# String.Format("{0:n2}",DataBinder.Eval(Container.DataItem, "NUM_VALOR_FIXO_MARKUP_13X"))%>'></asp:TextBox></strong></small>
                                                    <small><strong><asp:TextBox runat="server" CssClass="form-control tab_taxas" id="txtmarkup13x" Text='<%# String.Format("{0:n2}",DataBinder.Eval(Container.DataItem, "NUM_VALOR_MARKUP_13X"))%>'></asp:TextBox></strong></small>
                                                    <small><strong><asp:TextBox runat="server" CssClass="form-control tab_taxas" id="txtrebate13x" Text='<%# String.Format("{0:n2}",DataBinder.Eval(Container.DataItem, "NUM_VALOR_REBATE_13X"))%>'></asp:TextBox></strong></small><br />
                                                    <small><strong><asp:Label runat="server" id="Label13" Text='<%# String.Format("{0:n2}",DataBinder.Eval(Container.DataItem, "NUM_VALOR_FINAL_13X"))%>'></asp:Label></strong></small>
                                                    <small><strong><asp:Label runat="server" id="Label35" Text='<%# String.Format("{0:n2}",DataBinder.Eval(Container.DataItem, "NUM_VALOR_FINAL_FIXO_13X"))%>'></asp:Label></strong></small>
                                                </td>
                                                <td class="text-center" style="min-width:70px;" >
                                                    <small><strong><asp:TextBox runat="server" CssClass="form-control tab_taxas" id="txtcredito14xfixo" Text='<%# String.Format("{0:n2}",DataBinder.Eval(Container.DataItem, "NUM_VALOR_FIXO_CREDITO_14X"))%>' Enabled="false"></asp:TextBox></strong></small>
                                                    <small><strong><asp:TextBox runat="server" CssClass="form-control tab_taxas" id="txtcredito14x" Text='<%# String.Format("{0:n2}",DataBinder.Eval(Container.DataItem, "NUM_VALOR_CREDITO_14X"))%>' Enabled="false"></asp:TextBox></strong></small><br />
                                                    <small><strong><asp:TextBox runat="server" CssClass="form-control tab_taxas" id="txtmarkup14xfixo" Text='<%# String.Format("{0:n2}",DataBinder.Eval(Container.DataItem, "NUM_VALOR_FIXO_MARKUP_14X"))%>'></asp:TextBox></strong></small>
                                                    <small><strong><asp:TextBox runat="server" CssClass="form-control tab_taxas" id="txtmarkup14x" Text='<%# String.Format("{0:n2}",DataBinder.Eval(Container.DataItem, "NUM_VALOR_MARKUP_14X"))%>'></asp:TextBox></strong></small>
                                                    <small><strong><asp:TextBox runat="server" CssClass="form-control tab_taxas" id="txtrebate14x" Text='<%# String.Format("{0:n2}",DataBinder.Eval(Container.DataItem, "NUM_VALOR_REBATE_14X"))%>'></asp:TextBox></strong></small><br />
                                                    <small><strong><asp:Label runat="server" id="Label14" Text='<%# String.Format("{0:n2}",DataBinder.Eval(Container.DataItem, "NUM_VALOR_FINAL_14X"))%>'></asp:Label></strong></small>
                                                    <small><strong><asp:Label runat="server" id="Label36" Text='<%# String.Format("{0:n2}",DataBinder.Eval(Container.DataItem, "NUM_VALOR_FINAL_FIXO_14X"))%>'></asp:Label></strong></small>
                                                </td>
                                                <td class="text-center" style="min-width:70px;" >
                                                    <small><strong><asp:TextBox runat="server" CssClass="form-control tab_taxas" id="txtcredito15xfixo" Text='<%# String.Format("{0:n2}",DataBinder.Eval(Container.DataItem, "NUM_VALOR_FIXO_CREDITO_15X"))%>' Enabled="false"></asp:TextBox></strong></small>
                                                    <small><strong><asp:TextBox runat="server" CssClass="form-control tab_taxas" id="txtcredito15x" Text='<%# String.Format("{0:n2}",DataBinder.Eval(Container.DataItem, "NUM_VALOR_CREDITO_15X"))%>' Enabled="false"></asp:TextBox></strong></small><br />
                                                    <small><strong><asp:TextBox runat="server" CssClass="form-control tab_taxas" id="txtmarkup15xfixo" Text='<%# String.Format("{0:n2}",DataBinder.Eval(Container.DataItem, "NUM_VALOR_FIXO_MARKUP_15X"))%>'></asp:TextBox></strong></small>
                                                    <small><strong><asp:TextBox runat="server" CssClass="form-control tab_taxas" id="txtmarkup15x" Text='<%# String.Format("{0:n2}",DataBinder.Eval(Container.DataItem, "NUM_VALOR_MARKUP_15X"))%>'></asp:TextBox></strong></small>
                                                    <small><strong><asp:TextBox runat="server" CssClass="form-control tab_taxas" id="txtrebate15x" Text='<%# String.Format("{0:n2}",DataBinder.Eval(Container.DataItem, "NUM_VALOR_REBATE_15X"))%>'></asp:TextBox></strong></small><br />
                                                    <small><strong><asp:Label runat="server" id="Label15" Text='<%# String.Format("{0:n2}",DataBinder.Eval(Container.DataItem, "NUM_VALOR_FINAL_15X"))%>'></asp:Label></strong></small>
                                                    <small><strong><asp:Label runat="server" id="Label37" Text='<%# String.Format("{0:n2}",DataBinder.Eval(Container.DataItem, "NUM_VALOR_FINAL_FIXO_15X"))%>'></asp:Label></strong></small>
                                                </td>
                                                <td class="text-center" style="min-width:70px;" >
                                                    <small><strong><asp:TextBox runat="server" CssClass="form-control tab_taxas" id="txtcredito16xfixo" Text='<%# String.Format("{0:n2}",DataBinder.Eval(Container.DataItem, "NUM_VALOR_FIXO_CREDITO_16X"))%>' Enabled="false"></asp:TextBox></strong></small>
                                                    <small><strong><asp:TextBox runat="server" CssClass="form-control tab_taxas" id="txtcredito16x" Text='<%# String.Format("{0:n2}",DataBinder.Eval(Container.DataItem, "NUM_VALOR_CREDITO_16X"))%>' Enabled="false"></asp:TextBox></strong></small><br />
                                                    <small><strong><asp:TextBox runat="server" CssClass="form-control tab_taxas" id="txtmarkup16xfixo" Text='<%# String.Format("{0:n2}",DataBinder.Eval(Container.DataItem, "NUM_VALOR_FIXO_MARKUP_16X"))%>'></asp:TextBox></strong></small>
                                                    <small><strong><asp:TextBox runat="server" CssClass="form-control tab_taxas" id="txtmarkup16x" Text='<%# String.Format("{0:n2}",DataBinder.Eval(Container.DataItem, "NUM_VALOR_MARKUP_16X"))%>'></asp:TextBox></strong></small>
                                                    <small><strong><asp:TextBox runat="server" CssClass="form-control tab_taxas" id="txtrebate16x" Text='<%# String.Format("{0:n2}",DataBinder.Eval(Container.DataItem, "NUM_VALOR_REBATE_16X"))%>'></asp:TextBox></strong></small><br />
                                                    <small><strong><asp:Label runat="server" id="Label16" Text='<%# String.Format("{0:n2}",DataBinder.Eval(Container.DataItem, "NUM_VALOR_FINAL_16X"))%>'></asp:Label></strong></small>
                                                    <small><strong><asp:Label runat="server" id="Label38" Text='<%# String.Format("{0:n2}",DataBinder.Eval(Container.DataItem, "NUM_VALOR_FINAL_FIXO_16X"))%>'></asp:Label></strong></small>
                                                </td>
                                                <td class="text-center" style="min-width:70px;" >
                                                    <small><strong><asp:TextBox runat="server" CssClass="form-control tab_taxas" id="txtcredito17xfixo" Text='<%# String.Format("{0:n2}",DataBinder.Eval(Container.DataItem, "NUM_VALOR_FIXO_CREDITO_17X"))%>' Enabled="false"></asp:TextBox></strong></small>
                                                    <small><strong><asp:TextBox runat="server" CssClass="form-control tab_taxas" id="txtcredito17x" Text='<%# String.Format("{0:n2}",DataBinder.Eval(Container.DataItem, "NUM_VALOR_CREDITO_17X"))%>' Enabled="false"></asp:TextBox></strong></small><br />
                                                    <small><strong><asp:TextBox runat="server" CssClass="form-control tab_taxas" id="txtmarkup17xfixo" Text='<%# String.Format("{0:n2}",DataBinder.Eval(Container.DataItem, "NUM_VALOR_FIXO_MARKUP_17X"))%>'></asp:TextBox></strong></small>
                                                    <small><strong><asp:TextBox runat="server" CssClass="form-control tab_taxas" id="txtmarkup17x" Text='<%# String.Format("{0:n2}",DataBinder.Eval(Container.DataItem, "NUM_VALOR_MARKUP_17X"))%>'></asp:TextBox></strong></small>
                                                    <small><strong><asp:TextBox runat="server" CssClass="form-control tab_taxas" id="txtrebate17x" Text='<%# String.Format("{0:n2}",DataBinder.Eval(Container.DataItem, "NUM_VALOR_REBATE_17X"))%>'></asp:TextBox></strong></small><br />
                                                    <small><strong><asp:Label runat="server" id="Label17" Text='<%# String.Format("{0:n2}",DataBinder.Eval(Container.DataItem, "NUM_VALOR_FINAL_17X"))%>'></asp:Label></strong></small>
                                                    <small><strong><asp:Label runat="server" id="Label39" Text='<%# String.Format("{0:n2}",DataBinder.Eval(Container.DataItem, "NUM_VALOR_FINAL_FIXO_17X"))%>'></asp:Label></strong></small>
                                                </td>
                                                <td class="text-center" style="min-width:70px;" >
                                                    <small><strong><asp:TextBox runat="server" CssClass="form-control tab_taxas" id="txtcredito18xfixo" Text='<%# String.Format("{0:n2}",DataBinder.Eval(Container.DataItem, "NUM_VALOR_FIXO_CREDITO_18X"))%>' Enabled="false"></asp:TextBox></strong></small>
                                                    <small><strong><asp:TextBox runat="server" CssClass="form-control tab_taxas" id="txtcredito18x" Text='<%# String.Format("{0:n2}",DataBinder.Eval(Container.DataItem, "NUM_VALOR_CREDITO_18X"))%>' Enabled="false"></asp:TextBox></strong></small><br />
                                                    <small><strong><asp:TextBox runat="server" CssClass="form-control tab_taxas" id="txtmarkup18xfixo" Text='<%# String.Format("{0:n2}",DataBinder.Eval(Container.DataItem, "NUM_VALOR_FIXO_MARKUP_18X"))%>'></asp:TextBox></strong></small>
                                                    <small><strong><asp:TextBox runat="server" CssClass="form-control tab_taxas" id="txtmarkup18x" Text='<%# String.Format("{0:n2}",DataBinder.Eval(Container.DataItem, "NUM_VALOR_MARKUP_18X"))%>'></asp:TextBox></strong></small>
                                                    <small><strong><asp:TextBox runat="server" CssClass="form-control tab_taxas" id="txtrebate18x" Text='<%# String.Format("{0:n2}",DataBinder.Eval(Container.DataItem, "NUM_VALOR_REBATE_18X"))%>'></asp:TextBox></strong></small><br />
                                                    <small><strong><asp:Label runat="server" id="Label18" Text='<%# String.Format("{0:n2}",DataBinder.Eval(Container.DataItem, "NUM_VALOR_FINAL_18X"))%>'></asp:Label></strong></small>
                                                    <small><strong><asp:Label runat="server" id="Label40" Text='<%# String.Format("{0:n2}",DataBinder.Eval(Container.DataItem, "NUM_VALOR_FINAL_FIXO_18X"))%>'></asp:Label></strong></small>
                                                </td>

                                                <td class="text-center" style="min-width:70px;" >
                                                    <small><strong><asp:TextBox runat="server" CssClass="form-control tab_taxas" id="txtcredito19xfixo" Text='<%# String.Format("{0:n2}",DataBinder.Eval(Container.DataItem, "NUM_VALOR_FIXO_CREDITO_19X"))%>' Enabled="false"></asp:TextBox></strong></small>
                                                    <small><strong><asp:TextBox runat="server" CssClass="form-control tab_taxas" id="txtcredito19x" Text='<%# String.Format("{0:n2}",DataBinder.Eval(Container.DataItem, "NUM_VALOR_CREDITO_19X"))%>' Enabled="false"></asp:TextBox></strong></small><br />
                                                    <small><strong><asp:TextBox runat="server" CssClass="form-control tab_taxas" id="txtmarkup19xfixo" Text='<%# String.Format("{0:n2}",DataBinder.Eval(Container.DataItem, "NUM_VALOR_FIXO_MARKUP_19X"))%>'></asp:TextBox></strong></small>
                                                    <small><strong><asp:TextBox runat="server" CssClass="form-control tab_taxas" id="txtmarkup19x" Text='<%# String.Format("{0:n2}",DataBinder.Eval(Container.DataItem, "NUM_VALOR_MARKUP_19X"))%>'></asp:TextBox></strong></small>
                                                    <small><strong><asp:TextBox runat="server" CssClass="form-control tab_taxas" id="txtrebate19x" Text='<%# String.Format("{0:n2}",DataBinder.Eval(Container.DataItem, "NUM_VALOR_REBATE_19X"))%>'></asp:TextBox></strong></small><br />
                                                    <small><strong><asp:Label runat="server" id="Label19" Text='<%# String.Format("{0:n2}",DataBinder.Eval(Container.DataItem, "NUM_VALOR_FINAL_19X"))%>'></asp:Label></strong></small>
                                                    <small><strong><asp:Label runat="server" id="Label41" Text='<%# String.Format("{0:n2}",DataBinder.Eval(Container.DataItem, "NUM_VALOR_FINAL_FIXO_19X"))%>'></asp:Label></strong></small>
                                                </td>
                                                <td class="text-center" style="min-width:70px;" >
                                                    <small><strong><asp:TextBox runat="server" CssClass="form-control tab_taxas" id="txtcredito20xfixo" Text='<%# String.Format("{0:n2}",DataBinder.Eval(Container.DataItem, "NUM_VALOR_FIXO_CREDITO_20X"))%>' Enabled="false"></asp:TextBox></strong></small>
                                                    <small><strong><asp:TextBox runat="server" CssClass="form-control tab_taxas" id="txtcredito20x" Text='<%# String.Format("{0:n2}",DataBinder.Eval(Container.DataItem, "NUM_VALOR_CREDITO_20X"))%>' Enabled="false"></asp:TextBox></strong></small><br />
                                                    <small><strong><asp:TextBox runat="server" CssClass="form-control tab_taxas" id="txtmarkup20xfixo" Text='<%# String.Format("{0:n2}",DataBinder.Eval(Container.DataItem, "NUM_VALOR_FIXO_MARKUP_20X"))%>'></asp:TextBox></strong></small>
                                                    <small><strong><asp:TextBox runat="server" CssClass="form-control tab_taxas" id="txtmarkup20x" Text='<%# String.Format("{0:n2}",DataBinder.Eval(Container.DataItem, "NUM_VALOR_MARKUP_20X"))%>'></asp:TextBox></strong></small>
                                                    <small><strong><asp:TextBox runat="server" CssClass="form-control tab_taxas" id="txtrebate20x" Text='<%# String.Format("{0:n2}",DataBinder.Eval(Container.DataItem, "NUM_VALOR_REBATE_20X"))%>'></asp:TextBox></strong></small><br />
                                                    <small><strong><asp:Label runat="server" id="Label20" Text='<%# String.Format("{0:n2}",DataBinder.Eval(Container.DataItem, "NUM_VALOR_FINAL_20X"))%>'></asp:Label></strong></small>
                                                    <small><strong><asp:Label runat="server" id="Label42" Text='<%# String.Format("{0:n2}",DataBinder.Eval(Container.DataItem, "NUM_VALOR_FINAL_FIXO_20X"))%>'></asp:Label></strong></small>
                                                </td>
                                                <td class="text-center" style="min-width:70px;" >
                                                    <small><strong><asp:TextBox runat="server" CssClass="form-control tab_taxas" id="txtcredito21xfixo" Text='<%# String.Format("{0:n2}",DataBinder.Eval(Container.DataItem, "NUM_VALOR_FIXO_CREDITO_21X"))%>' Enabled="false"></asp:TextBox></strong></small>
                                                    <small><strong><asp:TextBox runat="server" CssClass="form-control tab_taxas" id="txtcredito21x" Text='<%# String.Format("{0:n2}",DataBinder.Eval(Container.DataItem, "NUM_VALOR_CREDITO_21X"))%>' Enabled="false"></asp:TextBox></strong></small><br />
                                                    <small><strong><asp:TextBox runat="server" CssClass="form-control tab_taxas" id="txtmarkup21xfixo" Text='<%# String.Format("{0:n2}",DataBinder.Eval(Container.DataItem, "NUM_VALOR_FIXO_MARKUP_21X"))%>'></asp:TextBox></strong></small>
                                                    <small><strong><asp:TextBox runat="server" CssClass="form-control tab_taxas" id="txtmarkup21x" Text='<%# String.Format("{0:n2}",DataBinder.Eval(Container.DataItem, "NUM_VALOR_MARKUP_21X"))%>'></asp:TextBox></strong></small>
                                                    <small><strong><asp:TextBox runat="server" CssClass="form-control tab_taxas" id="txtrebate21x" Text='<%# String.Format("{0:n2}",DataBinder.Eval(Container.DataItem, "NUM_VALOR_REBATE_21X"))%>'></asp:TextBox></strong></small><br />
                                                    <small><strong><asp:Label runat="server" id="Label21" Text='<%# String.Format("{0:n2}",DataBinder.Eval(Container.DataItem, "NUM_VALOR_FINAL_21X"))%>'></asp:Label></strong></small>
                                                    <small><strong><asp:Label runat="server" id="Label43" Text='<%# String.Format("{0:n2}",DataBinder.Eval(Container.DataItem, "NUM_VALOR_FINAL_FIXO_21X"))%>'></asp:Label></strong></small>
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
                                        <th>Vlr.Oper. (R$)</th>
                                        <th>Taxa (%)</th>
                                        <th>Markup (%)</th>
                                        <th>Rebate (%)</th>
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
                                                <asp:TextBox runat="server" id="txtparcini" Text='<%# DataBinder.Eval(Container.DataItem, "COD_PARCELA_INICIAL")%>' visible="false"></asp:TextBox>
                                                <asp:TextBox runat="server" id="txtparcfim" Text='<%# DataBinder.Eval(Container.DataItem, "COD_PARCELA_FINAL")%>' visible="false"></asp:TextBox>

                                                    <small><strong><%# DataBinder.Eval(Container.DataItem, "NUM_ORDEM_BANDEIRAS")%></strong></small>
                                                </td>

                                                <td class="text-center">
                                                    <small><strong><%# DataBinder.Eval(Container.DataItem, "NOM_BANDEIRA")%></strong></small><br />
                                                    <asp:Image runat="server" id="imgBandeira" CssClass="img-fluid img-thumbnail mx-auto" style="max-width:50px;" ImageUrl='<%# DataBinder.Eval(Container.DataItem, "NOM_IMAGEM")%>'></asp:Image><br /><br />

                                                </td>

                                                <td>
                                                    <small><strong><%# DataBinder.Eval(Container.DataItem, "NOM_TABELA")%></strong></small>
                                                </td>

                                                <td>
                                                    <small><asp:TextBox runat="server" id="txtValor" Text='<%# String.Format("{0:n2}",DataBinder.Eval(Container.DataItem, "NUM_VALOR"))%>' class="form-control" Enabled="false"></asp:TextBox></small>
                                                </td>

                                                <td>
                                                    <small><asp:TextBox runat="server" id="txtTaxa" Text='<%# String.Format("{0:n2}",DataBinder.Eval(Container.DataItem, "NUM_TAXA"))%>' class="form-control"  Enabled="false"></asp:TextBox></small>
                                                </td>
                                                <td>
                                                    <small><asp:TextBox runat="server" id="txtMarkup" Text='<%# String.Format("{0:n2}",DataBinder.Eval(Container.DataItem, "NUM_MARKUP"))%>' class="form-control"></asp:TextBox></small>
                                                </td>
                                                <td>
                                                    <small><asp:TextBox runat="server" id="txtRebate" Text='<%# String.Format("{0:n2}",DataBinder.Eval(Container.DataItem, "NUM_REBATE"))%>' class="form-control"></asp:TextBox></small>
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

                    <div class="card-header bg-whitelabel1">
                    <h3 class="card-title">TAXAS & MARKUPS</h3>
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
                                        <th><h6><small class="tab_titulo"><b>Bandeira</b></small></h6></th>
                                        <th><h6><small class="tab_titulo"><b>Débito</b></small></h6></th>
                                        <th><h6><small class="tab_titulo"><b>Cred.À Vista</b></small></h6></th>
                                        <th><h6><small class="tab_titulo"><b>Cred.2x</b></small></h6></th>
                                        <th><h6><small class="tab_titulo"><b>Cred.3x</b></small></h6></th>
                                        <th><h6><small class="tab_titulo"><b>Cred.4x</b></small></h6></th>
                                        <th><h6><small class="tab_titulo"><b>Cred.5x</b></small></h6></th>
                                        <th><h6><small class="tab_titulo"><b>Cred.6x</b></small></h6></th>
                                        <th><h6><small class="tab_titulo"><b>Cred.7x</b></small></h6></th>
                                        <th><h6><small class="tab_titulo"><b>Cred.8x</b></small></h6></th>
                                        <th><h6><small class="tab_titulo"><b>Cred.9x</b></small></h6></th>
                                        <th><h6><small class="tab_titulo"><b>Cred.10x</b></small></h6></th>
                                        <th><h6><small class="tab_titulo"><b>Cred.11x</b></small></h6></th>
                                        <th><h6><small class="tab_titulo"><b>Cred.12x</b></small></h6></th>
                                        <th><h6><small class="tab_titulo"><b>Cred.13x</b></small></h6></th>
                                        <th><h6><small class="tab_titulo"><b>Cred.14x</b></small></h6></th>
                                        <th><h6><small class="tab_titulo"><b>Cred.15x</b></small></h6></th>
                                        <th><h6><small class="tab_titulo"><b>Cred.16x</b></small></h6></th>
                                        <th><h6><small class="tab_titulo"><b>Cred.17x</b></small></h6></th>
                                        <th><h6><small class="tab_titulo"><b>Cred.18x</b></small></h6></th>
                                        <th><h6><small class="tab_titulo"><b>Cred.19x</b></small></h6></th>
                                        <th><h6><small class="tab_titulo"><b>Cred.20x</b></small></h6></th>
                                        <th><h6><small class="tab_titulo"><b>Cred.21x</b></small></h6></th>
                                    </tr>
                                    </thead>
                                    <tbody>
                                    <asp:Repeater runat="server" ID="rptConsultaParcelasOnline" >
                                        <ItemTemplate>

                                            <tr>
                                                <td class="text-center">
                                                <asp:TextBox runat="server" id="txtidplano" Text='<%# DataBinder.Eval(Container.DataItem, "COD_ID_PLANOS")%>' visible="false"></asp:TextBox>
                                                <asp:TextBox runat="server" id="txtidbandeira" Text='<%# DataBinder.Eval(Container.DataItem, "COD_ID_BANDEIRAS")%>' visible="false"></asp:TextBox>
                                                    <small class="tab_titulo"><strong>Vlr.Oper.(R$)</strong></small><br />
                                                    <small class="tab_titulo"><strong>Taxa (%)</strong></small><br /><br />
                                                    <small class="tab_titulo"><strong><%# DataBinder.Eval(Container.DataItem, "NOM_BANDEIRA")%></strong></small><br />
                                                    <asp:Image runat="server" id="imgBandeira" CssClass="img-fluid img-thumbnail mx-auto" style="max-width:50px;" ImageUrl='<%# DataBinder.Eval(Container.DataItem, "NOM_IMAGEM")%>'></asp:Image><br /><br />
                                                    <small class="tab_titulo"><strong>Markup (R$)</strong></small><br />
                                                    <small class="tab_titulo"><strong>Markup (%)</strong></small><br />
                                                    <small class="tab_titulo"><strong>Rebate (%)</strong></small><br /><br />
                                                    <small class="tab_titulo"><strong>Tx.Final (%)</strong></small>
                                                    <small class="tab_titulo"><strong>Tx.Final (R$)</strong></small>

                                                </td>

                                                <td class="text-center" style="min-width:70px;" >
                                                    <small><strong><asp:TextBox runat="server" CssClass="form-control tab_taxas" id="txtdebitofixo" Text='<%# String.Format("{0:n2}",DataBinder.Eval(Container.DataItem, "NUM_VALOR_FIXO_DEBITO"))%>' Enabled="false"></asp:TextBox></strong></small>
                                                    <small><strong><asp:TextBox runat="server" CssClass="form-control tab_taxas" id="txtdebito" Text='<%# String.Format("{0:n2}",DataBinder.Eval(Container.DataItem, "NUM_VALOR_DEBITO"))%>' Enabled="false"></asp:TextBox></strong></small><br />
                                                    <small><strong><asp:TextBox runat="server" CssClass="form-control tab_taxas" id="txtmarkupdebitofixo" Text='<%# String.Format("{0:n2}",DataBinder.Eval(Container.DataItem, "NUM_VALOR_FIXO_MARKUP_DEBITO"))%>'></asp:TextBox></strong></small>
                                                    <small><strong><asp:TextBox runat="server" CssClass="form-control tab_taxas" id="txtmarkupdebito" Text='<%# String.Format("{0:n2}",DataBinder.Eval(Container.DataItem, "NUM_VALOR_MARKUP_DEBITO"))%>'></asp:TextBox></strong></small>
                                                    <small><strong><asp:TextBox runat="server" CssClass="form-control tab_taxas" id="txtrebatedebito" Text='<%# String.Format("{0:n2}",DataBinder.Eval(Container.DataItem, "NUM_VALOR_REBATE_DEBITO"))%>'></asp:TextBox></strong></small><br />
                                                    <small><strong><asp:Label runat="server" id="lblfinaldebito" Text='<%# String.Format("{0:n2}",DataBinder.Eval(Container.DataItem, "NUM_VALOR_FINAL_DEBITO"))%>'></asp:Label></strong></small>
                                                    <small><strong><asp:Label runat="server" id="Label22" Text='<%# String.Format("{0:n2}",DataBinder.Eval(Container.DataItem, "NUM_VALOR_FINAL_FIXO_DEBITO"))%>'></asp:Label></strong></small>
                                                </td                                                </td>
                                                <td class="text-center" style="min-width:70px;" >
                                                    <small><strong><asp:TextBox runat="server" CssClass="form-control tab_taxas" id="txtcreditofixo" Text='<%# String.Format("{0:n2}",DataBinder.Eval(Container.DataItem, "NUM_VALOR_FIXO_CREDITO_VISTA"))%>' Enabled="false" ></asp:TextBox></strong></small>
                                                    <small><strong><asp:TextBox runat="server" CssClass="form-control tab_taxas" id="txtcredito" Text='<%# String.Format("{0:n2}",DataBinder.Eval(Container.DataItem, "NUM_VALOR_CREDITO_VISTA"))%>' Enabled="false" ></asp:TextBox></strong></small><br />
                                                    <small><strong><asp:TextBox runat="server" CssClass="form-control tab_taxas" id="txtmarkupcreditofixo" Text='<%# String.Format("{0:n2}",DataBinder.Eval(Container.DataItem, "NUM_VALOR_FIXO_MARKUP_VISTA"))%>'></asp:TextBox></strong></small>
                                                    <small><strong><asp:TextBox runat="server" CssClass="form-control tab_taxas" id="txtmarkupcredito" Text='<%# String.Format("{0:n2}",DataBinder.Eval(Container.DataItem, "NUM_VALOR_MARKUP_VISTA"))%>'></asp:TextBox></strong></small>
                                                    <small><strong><asp:TextBox runat="server" CssClass="form-control tab_taxas" id="txtrebatevista" Text='<%# String.Format("{0:n2}",DataBinder.Eval(Container.DataItem, "NUM_VALOR_REBATE_VISTA"))%>'></asp:TextBox></strong></small><br />
                                                    <small><strong><asp:Label runat="server" id="Label1" Text='<%# String.Format("{0:n2}",DataBinder.Eval(Container.DataItem, "NUM_VALOR_FINAL_VISTA"))%>'></asp:Label></strong></small>
                                                    <small><strong><asp:Label runat="server" id="Label23" Text='<%# String.Format("{0:n2}",DataBinder.Eval(Container.DataItem, "NUM_VALOR_FINAL_FIXO_VISTA"))%>'></asp:Label></strong></small>
                                                </td>
                                                <td class="text-center" style="min-width:70px;" >
                                                    <small><strong><asp:TextBox runat="server" CssClass="form-control tab_taxas" id="txtcredito2xfixo" Text='<%# String.Format("{0:n2}",DataBinder.Eval(Container.DataItem, "NUM_VALOR_FIXO_CREDITO_2X"))%>' Enabled="false"></asp:TextBox></strong></small>
                                                    <small><strong><asp:TextBox runat="server" CssClass="form-control tab_taxas" id="txtcredito2x" Text='<%# String.Format("{0:n2}",DataBinder.Eval(Container.DataItem, "NUM_VALOR_CREDITO_2X"))%>' Enabled="false"></asp:TextBox></strong></small><br />
                                                    <small><strong><asp:TextBox runat="server" CssClass="form-control tab_taxas" id="txtmarkup2xfixo" Text='<%# String.Format("{0:n2}",DataBinder.Eval(Container.DataItem, "NUM_VALOR_FIXO_MARKUP_2X"))%>'></asp:TextBox></strong></small>
                                                    <small><strong><asp:TextBox runat="server" CssClass="form-control tab_taxas" id="txtmarkup2x" Text='<%# String.Format("{0:n2}",DataBinder.Eval(Container.DataItem, "NUM_VALOR_MARKUP_2X"))%>'></asp:TextBox></strong></small>
                                                    <small><strong><asp:TextBox runat="server" CssClass="form-control tab_taxas" id="txtrebate2x" Text='<%# String.Format("{0:n2}",DataBinder.Eval(Container.DataItem, "NUM_VALOR_REBATE_2X"))%>'></asp:TextBox></strong></small><br />
                                                    <small><strong><asp:Label runat="server" id="Label2" Text='<%# String.Format("{0:n2}",DataBinder.Eval(Container.DataItem, "NUM_VALOR_FINAL_2X"))%>'></asp:Label></strong></small>
                                                    <small><strong><asp:Label runat="server" id="Label24" Text='<%# String.Format("{0:n2}",DataBinder.Eval(Container.DataItem, "NUM_VALOR_FINAL_FIXO_2X"))%>'></asp:Label></strong></small>
                                                </td>
                                                <td class="text-center" style="min-width:70px;" >
                                                    <small><strong><asp:TextBox runat="server" CssClass="form-control tab_taxas" id="txtcredito3xfixo" Text='<%# String.Format("{0:n2}",DataBinder.Eval(Container.DataItem, "NUM_VALOR_FIXO_CREDITO_3X"))%>' Enabled="false"></asp:TextBox></strong></small>
                                                    <small><strong><asp:TextBox runat="server" CssClass="form-control tab_taxas" id="txtcredito3x" Text='<%# String.Format("{0:n2}",DataBinder.Eval(Container.DataItem, "NUM_VALOR_CREDITO_3X"))%>' Enabled="false"></asp:TextBox></strong></small><br />
                                                    <small><strong><asp:TextBox runat="server" CssClass="form-control tab_taxas" id="txtmarkup3xfixo" Text='<%# String.Format("{0:n2}",DataBinder.Eval(Container.DataItem, "NUM_VALOR_FIXO_MARKUP_3X"))%>'></asp:TextBox></strong></small>
                                                    <small><strong><asp:TextBox runat="server" CssClass="form-control tab_taxas" id="txtmarkup3x" Text='<%# String.Format("{0:n2}",DataBinder.Eval(Container.DataItem, "NUM_VALOR_MARKUP_3X"))%>'></asp:TextBox></strong></small>
                                                    <small><strong><asp:TextBox runat="server" CssClass="form-control tab_taxas" id="txtrebate3x" Text='<%# String.Format("{0:n2}",DataBinder.Eval(Container.DataItem, "NUM_VALOR_REBATE_3X"))%>'></asp:TextBox></strong></small><br />
                                                    <small><strong><asp:Label runat="server" id="Label3" Text='<%# String.Format("{0:n2}",DataBinder.Eval(Container.DataItem, "NUM_VALOR_FINAL_3X"))%>'></asp:Label></strong></small>
                                                    <small><strong><asp:Label runat="server" id="Label25" Text='<%# String.Format("{0:n2}",DataBinder.Eval(Container.DataItem, "NUM_VALOR_FINAL_FIXO_3X"))%>'></asp:Label></strong></small>
                                                </td>
                                                <td class="text-center" style="min-width:70px;" >
                                                    <small><strong><asp:TextBox runat="server" CssClass="form-control tab_taxas" id="txtcredito4xfixo" Text='<%# String.Format("{0:n2}",DataBinder.Eval(Container.DataItem, "NUM_VALOR_FIXO_CREDITO_4X"))%>' Enabled="false"></asp:TextBox></strong></small>
                                                    <small><strong><asp:TextBox runat="server" CssClass="form-control tab_taxas" id="txtcredito4x" Text='<%# String.Format("{0:n2}",DataBinder.Eval(Container.DataItem, "NUM_VALOR_CREDITO_4X"))%>' Enabled="false"></asp:TextBox></strong></small><br />
                                                    <small><strong><asp:TextBox runat="server" CssClass="form-control tab_taxas" id="txtmarkup4xfixo" Text='<%# String.Format("{0:n2}",DataBinder.Eval(Container.DataItem, "NUM_VALOR_FIXO_MARKUP_4X"))%>'></asp:TextBox></strong></small>
                                                    <small><strong><asp:TextBox runat="server" CssClass="form-control tab_taxas" id="txtmarkup4x" Text='<%# String.Format("{0:n2}",DataBinder.Eval(Container.DataItem, "NUM_VALOR_MARKUP_4X"))%>'></asp:TextBox></strong></small>
                                                    <small><strong><asp:TextBox runat="server" CssClass="form-control tab_taxas" id="txtrebate4x" Text='<%# String.Format("{0:n2}",DataBinder.Eval(Container.DataItem, "NUM_VALOR_REBATE_4X"))%>'></asp:TextBox></strong></small><br />
                                                    <small><strong><asp:Label runat="server" id="Label4" Text='<%# String.Format("{0:n2}",DataBinder.Eval(Container.DataItem, "NUM_VALOR_FINAL_4X"))%>'></asp:Label></strong></small>
                                                    <small><strong><asp:Label runat="server" id="Label26" Text='<%# String.Format("{0:n2}",DataBinder.Eval(Container.DataItem, "NUM_VALOR_FINAL_FIXO_4X"))%>'></asp:Label></strong></small>
                                                </td>
                                                <td class="text-center" style="min-width:70px;" >
                                                    <small><strong><asp:TextBox runat="server" CssClass="form-control tab_taxas" id="txtcredito5xfixo" Text='<%# String.Format("{0:n2}",DataBinder.Eval(Container.DataItem, "NUM_VALOR_FIXO_CREDITO_5X"))%>' Enabled="false"></asp:TextBox></strong></small>
                                                    <small><strong><asp:TextBox runat="server" CssClass="form-control tab_taxas" id="txtcredito5x" Text='<%# String.Format("{0:n2}",DataBinder.Eval(Container.DataItem, "NUM_VALOR_CREDITO_5X"))%>' Enabled="false"></asp:TextBox></strong></small><br />
                                                    <small><strong><asp:TextBox runat="server" CssClass="form-control tab_taxas" id="txtmarkup5xfixo" Text='<%# String.Format("{0:n2}",DataBinder.Eval(Container.DataItem, "NUM_VALOR_FIXO_MARKUP_5X"))%>'></asp:TextBox></strong></small>
                                                    <small><strong><asp:TextBox runat="server" CssClass="form-control tab_taxas" id="txtmarkup5x" Text='<%# String.Format("{0:n2}",DataBinder.Eval(Container.DataItem, "NUM_VALOR_MARKUP_5X"))%>'></asp:TextBox></strong></small>
                                                    <small><strong><asp:TextBox runat="server" CssClass="form-control tab_taxas" id="txtrebate5x" Text='<%# String.Format("{0:n2}",DataBinder.Eval(Container.DataItem, "NUM_VALOR_REBATE_5X"))%>'></asp:TextBox></strong></small><br />
                                                    <small><strong><asp:Label runat="server" id="Label5" Text='<%# String.Format("{0:n2}",DataBinder.Eval(Container.DataItem, "NUM_VALOR_FINAL_5X"))%>'></asp:Label></strong></small>
                                                    <small><strong><asp:Label runat="server" id="Label27" Text='<%# String.Format("{0:n2}",DataBinder.Eval(Container.DataItem, "NUM_VALOR_FINAL_FIXO_5X"))%>'></asp:Label></strong></small>
                                                </td>
                                                <td class="text-center" style="min-width:70px;" >
                                                    <small><strong><asp:TextBox runat="server" CssClass="form-control tab_taxas" id="txtcredito6xfixo" Text='<%# String.Format("{0:n2}",DataBinder.Eval(Container.DataItem, "NUM_VALOR_FIXO_CREDITO_6X"))%>' Enabled="false"></asp:TextBox></strong></small>
                                                    <small><strong><asp:TextBox runat="server" CssClass="form-control tab_taxas" id="txtcredito6x" Text='<%# String.Format("{0:n2}",DataBinder.Eval(Container.DataItem, "NUM_VALOR_CREDITO_6X"))%>' Enabled="false"></asp:TextBox></strong></small><br />
                                                    <small><strong><asp:TextBox runat="server" CssClass="form-control tab_taxas" id="txtmarkup6xfixo" Text='<%# String.Format("{0:n2}",DataBinder.Eval(Container.DataItem, "NUM_VALOR_FIXO_MARKUP_6X"))%>'></asp:TextBox></strong></small>
                                                    <small><strong><asp:TextBox runat="server" CssClass="form-control tab_taxas" id="txtmarkup6x" Text='<%# String.Format("{0:n2}",DataBinder.Eval(Container.DataItem, "NUM_VALOR_MARKUP_6X"))%>'></asp:TextBox></strong></small>
                                                    <small><strong><asp:TextBox runat="server" CssClass="form-control tab_taxas" id="txtrebate6x" Text='<%# String.Format("{0:n2}",DataBinder.Eval(Container.DataItem, "NUM_VALOR_REBATE_6X"))%>'></asp:TextBox></strong></small><br />
                                                    <small><strong><asp:Label runat="server" id="Label6" Text='<%# String.Format("{0:n2}",DataBinder.Eval(Container.DataItem, "NUM_VALOR_FINAL_6X"))%>'></asp:Label></strong></small>
                                                    <small><strong><asp:Label runat="server" id="Label28" Text='<%# String.Format("{0:n2}",DataBinder.Eval(Container.DataItem, "NUM_VALOR_FINAL_FIXO_6X"))%>'></asp:Label></strong></small>
                                                </td>
                                                <td class="text-center" style="min-width:70px;" >
                                                    <small><strong><asp:TextBox runat="server" CssClass="form-control tab_taxas" id="txtcredito7xfixo" Text='<%# String.Format("{0:n2}",DataBinder.Eval(Container.DataItem, "NUM_VALOR_FIXO_CREDITO_7X"))%>' Enabled="false"></asp:TextBox></strong></small>
                                                    <small><strong><asp:TextBox runat="server" CssClass="form-control tab_taxas" id="txtcredito7x" Text='<%# String.Format("{0:n2}",DataBinder.Eval(Container.DataItem, "NUM_VALOR_CREDITO_7X"))%>' Enabled="false"></asp:TextBox></strong></small><br />
                                                    <small><strong><asp:TextBox runat="server" CssClass="form-control tab_taxas" id="txtmarkup7xfixo" Text='<%# String.Format("{0:n2}",DataBinder.Eval(Container.DataItem, "NUM_VALOR_FIXO_MARKUP_7X"))%>'></asp:TextBox></strong></small>
                                                    <small><strong><asp:TextBox runat="server" CssClass="form-control tab_taxas" id="txtmarkup7x" Text='<%# String.Format("{0:n2}",DataBinder.Eval(Container.DataItem, "NUM_VALOR_MARKUP_7X"))%>'></asp:TextBox></strong></small>
                                                    <small><strong><asp:TextBox runat="server" CssClass="form-control tab_taxas" id="txtrebate7x" Text='<%# String.Format("{0:n2}",DataBinder.Eval(Container.DataItem, "NUM_VALOR_REBATE_7X"))%>'></asp:TextBox></strong></small><br />
                                                    <small><strong><asp:Label runat="server" id="Label7" Text='<%# String.Format("{0:n2}",DataBinder.Eval(Container.DataItem, "NUM_VALOR_FINAL_7X"))%>'></asp:Label></strong></small>
                                                    <small><strong><asp:Label runat="server" id="Label29" Text='<%# String.Format("{0:n2}",DataBinder.Eval(Container.DataItem, "NUM_VALOR_FINAL_FIXO_7X"))%>'></asp:Label></strong></small>
                                                </td>
                                                <td class="text-center" style="min-width:70px;" >
                                                    <small><strong><asp:TextBox runat="server" CssClass="form-control tab_taxas" id="txtcredito8xfixo" Text='<%# String.Format("{0:n2}",DataBinder.Eval(Container.DataItem, "NUM_VALOR_FIXO_CREDITO_8X"))%>' Enabled="false"></asp:TextBox></strong></small>
                                                    <small><strong><asp:TextBox runat="server" CssClass="form-control tab_taxas" id="txtcredito8x" Text='<%# String.Format("{0:n2}",DataBinder.Eval(Container.DataItem, "NUM_VALOR_CREDITO_8X"))%>' Enabled="false"></asp:TextBox></strong></small><br />
                                                    <small><strong><asp:TextBox runat="server" CssClass="form-control tab_taxas" id="txtmarkup8xfixo" Text='<%# String.Format("{0:n2}",DataBinder.Eval(Container.DataItem, "NUM_VALOR_FIXO_MARKUP_8X"))%>'></asp:TextBox></strong></small>
                                                    <small><strong><asp:TextBox runat="server" CssClass="form-control tab_taxas" id="txtmarkup8x" Text='<%# String.Format("{0:n2}",DataBinder.Eval(Container.DataItem, "NUM_VALOR_MARKUP_8X"))%>'></asp:TextBox></strong></small>
                                                    <small><strong><asp:TextBox runat="server" CssClass="form-control tab_taxas" id="txtrebate8x" Text='<%# String.Format("{0:n2}",DataBinder.Eval(Container.DataItem, "NUM_VALOR_REBATE_8X"))%>'></asp:TextBox></strong></small><br />
                                                    <small><strong><asp:Label runat="server" id="Label8" Text='<%# String.Format("{0:n2}",DataBinder.Eval(Container.DataItem, "NUM_VALOR_FINAL_8X"))%>'></asp:Label></strong></small>
                                                    <small><strong><asp:Label runat="server" id="Label30" Text='<%# String.Format("{0:n2}",DataBinder.Eval(Container.DataItem, "NUM_VALOR_FINAL_FIXO_8X"))%>'></asp:Label></strong></small>
                                                </td>
                                                <td class="text-center" style="min-width:70px;" >
                                                    <small><strong><asp:TextBox runat="server" CssClass="form-control tab_taxas" id="txtcredito9xfixo" Text='<%# String.Format("{0:n2}",DataBinder.Eval(Container.DataItem, "NUM_VALOR_FIXO_CREDITO_9X"))%>' Enabled="false"></asp:TextBox></strong></small>
                                                    <small><strong><asp:TextBox runat="server" CssClass="form-control tab_taxas" id="txtcredito9x" Text='<%# String.Format("{0:n2}",DataBinder.Eval(Container.DataItem, "NUM_VALOR_CREDITO_9X"))%>' Enabled="false"></asp:TextBox></strong></small><br />
                                                    <small><strong><asp:TextBox runat="server" CssClass="form-control tab_taxas" id="txtmarkup9xfixo" Text='<%# String.Format("{0:n2}",DataBinder.Eval(Container.DataItem, "NUM_VALOR_FIXO_MARKUP_9X"))%>'></asp:TextBox></strong></small>
                                                    <small><strong><asp:TextBox runat="server" CssClass="form-control tab_taxas" id="txtmarkup9x" Text='<%# String.Format("{0:n2}",DataBinder.Eval(Container.DataItem, "NUM_VALOR_MARKUP_9X"))%>'></asp:TextBox></strong></small>
                                                    <small><strong><asp:TextBox runat="server" CssClass="form-control tab_taxas" id="txtrebate9x" Text='<%# String.Format("{0:n2}",DataBinder.Eval(Container.DataItem, "NUM_VALOR_REBATE_9X"))%>'></asp:TextBox></strong></small><br />
                                                    <small><strong><asp:Label runat="server" id="Label9" Text='<%# String.Format("{0:n2}",DataBinder.Eval(Container.DataItem, "NUM_VALOR_FINAL_9X"))%>'></asp:Label></strong></small>
                                                    <small><strong><asp:Label runat="server" id="Label31" Text='<%# String.Format("{0:n2}",DataBinder.Eval(Container.DataItem, "NUM_VALOR_FINAL_FIXO_9X"))%>'></asp:Label></strong></small>
                                                </td>
                                                <td class="text-center" style="min-width:70px;" >
                                                    <small><strong><asp:TextBox runat="server" CssClass="form-control tab_taxas" id="txtcredito10xfixo" Text='<%# String.Format("{0:n2}",DataBinder.Eval(Container.DataItem, "NUM_VALOR_FIXO_CREDITO_10X"))%>' Enabled="false"></asp:TextBox></strong></small>
                                                    <small><strong><asp:TextBox runat="server" CssClass="form-control tab_taxas" id="txtcredito10x" Text='<%# String.Format("{0:n2}",DataBinder.Eval(Container.DataItem, "NUM_VALOR_CREDITO_10X"))%>' Enabled="false"></asp:TextBox></strong></small><br />
                                                    <small><strong><asp:TextBox runat="server" CssClass="form-control tab_taxas" id="txtmarkup10xfixo" Text='<%# String.Format("{0:n2}",DataBinder.Eval(Container.DataItem, "NUM_VALOR_FIXO_MARKUP_10X"))%>'></asp:TextBox></strong></small>
                                                    <small><strong><asp:TextBox runat="server" CssClass="form-control tab_taxas" id="txtmarkup10x" Text='<%# String.Format("{0:n2}",DataBinder.Eval(Container.DataItem, "NUM_VALOR_MARKUP_10X"))%>'></asp:TextBox></strong></small>
                                                    <small><strong><asp:TextBox runat="server" CssClass="form-control tab_taxas" id="txtrebate10x" Text='<%# String.Format("{0:n2}",DataBinder.Eval(Container.DataItem, "NUM_VALOR_REBATE_10X"))%>'></asp:TextBox></strong></small><br />
                                                    <small><strong><asp:Label runat="server" id="Label10" Text='<%# String.Format("{0:n2}",DataBinder.Eval(Container.DataItem, "NUM_VALOR_FINAL_10X"))%>'></asp:Label></strong></small>
                                                    <small><strong><asp:Label runat="server" id="Label32" Text='<%# String.Format("{0:n2}",DataBinder.Eval(Container.DataItem, "NUM_VALOR_FINAL_FIXO_10X"))%>'></asp:Label></strong></small>
                                                </td>
                                                <td class="text-center" style="min-width:70px;" >
                                                    <small><strong><asp:TextBox runat="server" CssClass="form-control tab_taxas" id="txtcredito11xfixo" Text='<%# String.Format("{0:n2}",DataBinder.Eval(Container.DataItem, "NUM_VALOR_FIXO_CREDITO_11X"))%>' Enabled="false"></asp:TextBox></strong></small>
                                                    <small><strong><asp:TextBox runat="server" CssClass="form-control tab_taxas" id="txtcredito11x" Text='<%# String.Format("{0:n2}",DataBinder.Eval(Container.DataItem, "NUM_VALOR_CREDITO_11X"))%>' Enabled="false"></asp:TextBox></strong></small><br />
                                                    <small><strong><asp:TextBox runat="server" CssClass="form-control tab_taxas" id="txtmarkup11xfixo" Text='<%# String.Format("{0:n2}",DataBinder.Eval(Container.DataItem, "NUM_VALOR_FIXO_MARKUP_11X"))%>'></asp:TextBox></strong></small>
                                                    <small><strong><asp:TextBox runat="server" CssClass="form-control tab_taxas" id="txtmarkup11x" Text='<%# String.Format("{0:n2}",DataBinder.Eval(Container.DataItem, "NUM_VALOR_MARKUP_11X"))%>'></asp:TextBox></strong></small>
                                                    <small><strong><asp:TextBox runat="server" CssClass="form-control tab_taxas" id="txtrebate11x" Text='<%# String.Format("{0:n2}",DataBinder.Eval(Container.DataItem, "NUM_VALOR_REBATE_11X"))%>'></asp:TextBox></strong></small><br />
                                                    <small><strong><asp:Label runat="server" id="Label11" Text='<%# String.Format("{0:n2}",DataBinder.Eval(Container.DataItem, "NUM_VALOR_FINAL_11X"))%>'></asp:Label></strong></small>
                                                    <small><strong><asp:Label runat="server" id="Label33" Text='<%# String.Format("{0:n2}",DataBinder.Eval(Container.DataItem, "NUM_VALOR_FINAL_FIXO_11X"))%>'></asp:Label></strong></small>
                                                </td>
                                                <td class="text-center" style="min-width:70px;" >
                                                    <small><strong><asp:TextBox runat="server" CssClass="form-control tab_taxas" id="txtcredito12xfixo" Text='<%# String.Format("{0:n2}",DataBinder.Eval(Container.DataItem, "NUM_VALOR_FIXO_CREDITO_12X"))%>' Enabled="false"></asp:TextBox></strong></small>
                                                    <small><strong><asp:TextBox runat="server" CssClass="form-control tab_taxas" id="txtcredito12x" Text='<%# String.Format("{0:n2}",DataBinder.Eval(Container.DataItem, "NUM_VALOR_CREDITO_12X"))%>' Enabled="false"></asp:TextBox></strong></small><br />
                                                    <small><strong><asp:TextBox runat="server" CssClass="form-control tab_taxas" id="txtmarkup12xfixo" Text='<%# String.Format("{0:n2}",DataBinder.Eval(Container.DataItem, "NUM_VALOR_FIXO_MARKUP_12X"))%>'></asp:TextBox></strong></small>
                                                    <small><strong><asp:TextBox runat="server" CssClass="form-control tab_taxas" id="txtmarkup12x" Text='<%# String.Format("{0:n2}",DataBinder.Eval(Container.DataItem, "NUM_VALOR_MARKUP_12X"))%>'></asp:TextBox></strong></small>
                                                    <small><strong><asp:TextBox runat="server" CssClass="form-control tab_taxas" id="txtrebate12x" Text='<%# String.Format("{0:n2}",DataBinder.Eval(Container.DataItem, "NUM_VALOR_REBATE_12X"))%>'></asp:TextBox></strong></small><br />
                                                    <small><strong><asp:Label runat="server" id="Label12" Text='<%# String.Format("{0:n2}",DataBinder.Eval(Container.DataItem, "NUM_VALOR_FINAL_12X"))%>'></asp:Label></strong></small>
                                                    <small><strong><asp:Label runat="server" id="Label34" Text='<%# String.Format("{0:n2}",DataBinder.Eval(Container.DataItem, "NUM_VALOR_FINAL_FIXO_12X"))%>'></asp:Label></strong></small>
                                                </td>
                                                <td class="text-center" style="min-width:70px;" >
                                                    <small><strong><asp:TextBox runat="server" CssClass="form-control tab_taxas" id="txtcredito13xfixo" Text='<%# String.Format("{0:n2}",DataBinder.Eval(Container.DataItem, "NUM_VALOR_FIXO_CREDITO_13X"))%>' Enabled="false"></asp:TextBox></strong></small>
                                                    <small><strong><asp:TextBox runat="server" CssClass="form-control tab_taxas" id="txtcredito13x" Text='<%# String.Format("{0:n2}",DataBinder.Eval(Container.DataItem, "NUM_VALOR_CREDITO_13X"))%>' Enabled="false"></asp:TextBox></strong></small><br />
                                                    <small><strong><asp:TextBox runat="server" CssClass="form-control tab_taxas" id="txtmarkup13xfixo" Text='<%# String.Format("{0:n2}",DataBinder.Eval(Container.DataItem, "NUM_VALOR_FIXO_MARKUP_13X"))%>'></asp:TextBox></strong></small>
                                                    <small><strong><asp:TextBox runat="server" CssClass="form-control tab_taxas" id="txtmarkup13x" Text='<%# String.Format("{0:n2}",DataBinder.Eval(Container.DataItem, "NUM_VALOR_MARKUP_13X"))%>'></asp:TextBox></strong></small>
                                                    <small><strong><asp:TextBox runat="server" CssClass="form-control tab_taxas" id="txtrebate13x" Text='<%# String.Format("{0:n2}",DataBinder.Eval(Container.DataItem, "NUM_VALOR_REBATE_13X"))%>'></asp:TextBox></strong></small><br />
                                                    <small><strong><asp:Label runat="server" id="Label13" Text='<%# String.Format("{0:n2}",DataBinder.Eval(Container.DataItem, "NUM_VALOR_FINAL_13X"))%>'></asp:Label></strong></small>
                                                    <small><strong><asp:Label runat="server" id="Label35" Text='<%# String.Format("{0:n2}",DataBinder.Eval(Container.DataItem, "NUM_VALOR_FINAL_FIXO_13X"))%>'></asp:Label></strong></small>
                                                </td>
                                                <td class="text-center" style="min-width:70px;" >
                                                    <small><strong><asp:TextBox runat="server" CssClass="form-control tab_taxas" id="txtcredito14xfixo" Text='<%# String.Format("{0:n2}",DataBinder.Eval(Container.DataItem, "NUM_VALOR_FIXO_CREDITO_14X"))%>' Enabled="false"></asp:TextBox></strong></small>
                                                    <small><strong><asp:TextBox runat="server" CssClass="form-control tab_taxas" id="txtcredito14x" Text='<%# String.Format("{0:n2}",DataBinder.Eval(Container.DataItem, "NUM_VALOR_CREDITO_14X"))%>' Enabled="false"></asp:TextBox></strong></small><br />
                                                    <small><strong><asp:TextBox runat="server" CssClass="form-control tab_taxas" id="txtmarkup14xfixo" Text='<%# String.Format("{0:n2}",DataBinder.Eval(Container.DataItem, "NUM_VALOR_FIXO_MARKUP_14X"))%>'></asp:TextBox></strong></small>
                                                    <small><strong><asp:TextBox runat="server" CssClass="form-control tab_taxas" id="txtmarkup14x" Text='<%# String.Format("{0:n2}",DataBinder.Eval(Container.DataItem, "NUM_VALOR_MARKUP_14X"))%>'></asp:TextBox></strong></small>
                                                    <small><strong><asp:TextBox runat="server" CssClass="form-control tab_taxas" id="txtrebate14x" Text='<%# String.Format("{0:n2}",DataBinder.Eval(Container.DataItem, "NUM_VALOR_REBATE_14X"))%>'></asp:TextBox></strong></small><br />
                                                    <small><strong><asp:Label runat="server" id="Label14" Text='<%# String.Format("{0:n2}",DataBinder.Eval(Container.DataItem, "NUM_VALOR_FINAL_14X"))%>'></asp:Label></strong></small>
                                                    <small><strong><asp:Label runat="server" id="Label36" Text='<%# String.Format("{0:n2}",DataBinder.Eval(Container.DataItem, "NUM_VALOR_FINAL_FIXO_14X"))%>'></asp:Label></strong></small>
                                                </td>
                                                <td class="text-center" style="min-width:70px;" >
                                                    <small><strong><asp:TextBox runat="server" CssClass="form-control tab_taxas" id="txtcredito15xfixo" Text='<%# String.Format("{0:n2}",DataBinder.Eval(Container.DataItem, "NUM_VALOR_FIXO_CREDITO_15X"))%>' Enabled="false"></asp:TextBox></strong></small>
                                                    <small><strong><asp:TextBox runat="server" CssClass="form-control tab_taxas" id="txtcredito15x" Text='<%# String.Format("{0:n2}",DataBinder.Eval(Container.DataItem, "NUM_VALOR_CREDITO_15X"))%>' Enabled="false"></asp:TextBox></strong></small><br />
                                                    <small><strong><asp:TextBox runat="server" CssClass="form-control tab_taxas" id="txtmarkup15xfixo" Text='<%# String.Format("{0:n2}",DataBinder.Eval(Container.DataItem, "NUM_VALOR_FIXO_MARKUP_15X"))%>'></asp:TextBox></strong></small>
                                                    <small><strong><asp:TextBox runat="server" CssClass="form-control tab_taxas" id="txtmarkup15x" Text='<%# String.Format("{0:n2}",DataBinder.Eval(Container.DataItem, "NUM_VALOR_MARKUP_15X"))%>'></asp:TextBox></strong></small>
                                                    <small><strong><asp:TextBox runat="server" CssClass="form-control tab_taxas" id="txtrebate15x" Text='<%# String.Format("{0:n2}",DataBinder.Eval(Container.DataItem, "NUM_VALOR_REBATE_15X"))%>'></asp:TextBox></strong></small><br />
                                                    <small><strong><asp:Label runat="server" id="Label15" Text='<%# String.Format("{0:n2}",DataBinder.Eval(Container.DataItem, "NUM_VALOR_FINAL_15X"))%>'></asp:Label></strong></small>
                                                    <small><strong><asp:Label runat="server" id="Label37" Text='<%# String.Format("{0:n2}",DataBinder.Eval(Container.DataItem, "NUM_VALOR_FINAL_FIXO_15X"))%>'></asp:Label></strong></small>
                                                </td>
                                                <td class="text-center" style="min-width:70px;" >
                                                    <small><strong><asp:TextBox runat="server" CssClass="form-control tab_taxas" id="txtcredito16xfixo" Text='<%# String.Format("{0:n2}",DataBinder.Eval(Container.DataItem, "NUM_VALOR_FIXO_CREDITO_16X"))%>' Enabled="false"></asp:TextBox></strong></small>
                                                    <small><strong><asp:TextBox runat="server" CssClass="form-control tab_taxas" id="txtcredito16x" Text='<%# String.Format("{0:n2}",DataBinder.Eval(Container.DataItem, "NUM_VALOR_CREDITO_16X"))%>' Enabled="false"></asp:TextBox></strong></small><br />
                                                    <small><strong><asp:TextBox runat="server" CssClass="form-control tab_taxas" id="txtmarkup16xfixo" Text='<%# String.Format("{0:n2}",DataBinder.Eval(Container.DataItem, "NUM_VALOR_FIXO_MARKUP_16X"))%>'></asp:TextBox></strong></small>
                                                    <small><strong><asp:TextBox runat="server" CssClass="form-control tab_taxas" id="txtmarkup16x" Text='<%# String.Format("{0:n2}",DataBinder.Eval(Container.DataItem, "NUM_VALOR_MARKUP_16X"))%>'></asp:TextBox></strong></small>
                                                    <small><strong><asp:TextBox runat="server" CssClass="form-control tab_taxas" id="txtrebate16x" Text='<%# String.Format("{0:n2}",DataBinder.Eval(Container.DataItem, "NUM_VALOR_REBATE_16X"))%>'></asp:TextBox></strong></small><br />
                                                    <small><strong><asp:Label runat="server" id="Label16" Text='<%# String.Format("{0:n2}",DataBinder.Eval(Container.DataItem, "NUM_VALOR_FINAL_16X"))%>'></asp:Label></strong></small>
                                                    <small><strong><asp:Label runat="server" id="Label38" Text='<%# String.Format("{0:n2}",DataBinder.Eval(Container.DataItem, "NUM_VALOR_FINAL_FIXO_16X"))%>'></asp:Label></strong></small>
                                                </td>
                                                <td class="text-center" style="min-width:70px;" >
                                                    <small><strong><asp:TextBox runat="server" CssClass="form-control tab_taxas" id="txtcredito17xfixo" Text='<%# String.Format("{0:n2}",DataBinder.Eval(Container.DataItem, "NUM_VALOR_FIXO_CREDITO_17X"))%>' Enabled="false"></asp:TextBox></strong></small>
                                                    <small><strong><asp:TextBox runat="server" CssClass="form-control tab_taxas" id="txtcredito17x" Text='<%# String.Format("{0:n2}",DataBinder.Eval(Container.DataItem, "NUM_VALOR_CREDITO_17X"))%>' Enabled="false"></asp:TextBox></strong></small><br />
                                                    <small><strong><asp:TextBox runat="server" CssClass="form-control tab_taxas" id="txtmarkup17xfixo" Text='<%# String.Format("{0:n2}",DataBinder.Eval(Container.DataItem, "NUM_VALOR_FIXO_MARKUP_17X"))%>'></asp:TextBox></strong></small>
                                                    <small><strong><asp:TextBox runat="server" CssClass="form-control tab_taxas" id="txtmarkup17x" Text='<%# String.Format("{0:n2}",DataBinder.Eval(Container.DataItem, "NUM_VALOR_MARKUP_17X"))%>'></asp:TextBox></strong></small>
                                                    <small><strong><asp:TextBox runat="server" CssClass="form-control tab_taxas" id="txtrebate17x" Text='<%# String.Format("{0:n2}",DataBinder.Eval(Container.DataItem, "NUM_VALOR_REBATE_17X"))%>'></asp:TextBox></strong></small><br />
                                                    <small><strong><asp:Label runat="server" id="Label17" Text='<%# String.Format("{0:n2}",DataBinder.Eval(Container.DataItem, "NUM_VALOR_FINAL_17X"))%>'></asp:Label></strong></small>
                                                    <small><strong><asp:Label runat="server" id="Label39" Text='<%# String.Format("{0:n2}",DataBinder.Eval(Container.DataItem, "NUM_VALOR_FINAL_FIXO_17X"))%>'></asp:Label></strong></small>
                                                </td>
                                                <td class="text-center" style="min-width:70px;" >
                                                    <small><strong><asp:TextBox runat="server" CssClass="form-control tab_taxas" id="txtcredito18xfixo" Text='<%# String.Format("{0:n2}",DataBinder.Eval(Container.DataItem, "NUM_VALOR_FIXO_CREDITO_18X"))%>' Enabled="false"></asp:TextBox></strong></small>
                                                    <small><strong><asp:TextBox runat="server" CssClass="form-control tab_taxas" id="txtcredito18x" Text='<%# String.Format("{0:n2}",DataBinder.Eval(Container.DataItem, "NUM_VALOR_CREDITO_18X"))%>' Enabled="false"></asp:TextBox></strong></small><br />
                                                    <small><strong><asp:TextBox runat="server" CssClass="form-control tab_taxas" id="txtmarkup18xfixo" Text='<%# String.Format("{0:n2}",DataBinder.Eval(Container.DataItem, "NUM_VALOR_FIXO_MARKUP_18X"))%>'></asp:TextBox></strong></small>
                                                    <small><strong><asp:TextBox runat="server" CssClass="form-control tab_taxas" id="txtmarkup18x" Text='<%# String.Format("{0:n2}",DataBinder.Eval(Container.DataItem, "NUM_VALOR_MARKUP_18X"))%>'></asp:TextBox></strong></small>
                                                    <small><strong><asp:TextBox runat="server" CssClass="form-control tab_taxas" id="txtrebate18x" Text='<%# String.Format("{0:n2}",DataBinder.Eval(Container.DataItem, "NUM_VALOR_REBATE_18X"))%>'></asp:TextBox></strong></small><br />
                                                    <small><strong><asp:Label runat="server" id="Label18" Text='<%# String.Format("{0:n2}",DataBinder.Eval(Container.DataItem, "NUM_VALOR_FINAL_18X"))%>'></asp:Label></strong></small>
                                                    <small><strong><asp:Label runat="server" id="Label40" Text='<%# String.Format("{0:n2}",DataBinder.Eval(Container.DataItem, "NUM_VALOR_FINAL_FIXO_18X"))%>'></asp:Label></strong></small>
                                                </td>

                                                <td class="text-center" style="min-width:70px;" >
                                                    <small><strong><asp:TextBox runat="server" CssClass="form-control tab_taxas" id="txtcredito19xfixo" Text='<%# String.Format("{0:n2}",DataBinder.Eval(Container.DataItem, "NUM_VALOR_FIXO_CREDITO_19X"))%>' Enabled="false"></asp:TextBox></strong></small>
                                                    <small><strong><asp:TextBox runat="server" CssClass="form-control tab_taxas" id="txtcredito19x" Text='<%# String.Format("{0:n2}",DataBinder.Eval(Container.DataItem, "NUM_VALOR_CREDITO_19X"))%>' Enabled="false"></asp:TextBox></strong></small><br />
                                                    <small><strong><asp:TextBox runat="server" CssClass="form-control tab_taxas" id="txtmarkup19xfixo" Text='<%# String.Format("{0:n2}",DataBinder.Eval(Container.DataItem, "NUM_VALOR_FIXO_MARKUP_19X"))%>'></asp:TextBox></strong></small>
                                                    <small><strong><asp:TextBox runat="server" CssClass="form-control tab_taxas" id="txtmarkup19x" Text='<%# String.Format("{0:n2}",DataBinder.Eval(Container.DataItem, "NUM_VALOR_MARKUP_19X"))%>'></asp:TextBox></strong></small>
                                                    <small><strong><asp:TextBox runat="server" CssClass="form-control tab_taxas" id="txtrebate19x" Text='<%# String.Format("{0:n2}",DataBinder.Eval(Container.DataItem, "NUM_VALOR_REBATE_19X"))%>'></asp:TextBox></strong></small><br />
                                                    <small><strong><asp:Label runat="server" id="Label19" Text='<%# String.Format("{0:n2}",DataBinder.Eval(Container.DataItem, "NUM_VALOR_FINAL_19X"))%>'></asp:Label></strong></small>
                                                    <small><strong><asp:Label runat="server" id="Label41" Text='<%# String.Format("{0:n2}",DataBinder.Eval(Container.DataItem, "NUM_VALOR_FINAL_FIXO_19X"))%>'></asp:Label></strong></small>
                                                </td>
                                                <td class="text-center" style="min-width:70px;" >
                                                    <small><strong><asp:TextBox runat="server" CssClass="form-control tab_taxas" id="txtcredito20xfixo" Text='<%# String.Format("{0:n2}",DataBinder.Eval(Container.DataItem, "NUM_VALOR_FIXO_CREDITO_20X"))%>' Enabled="false"></asp:TextBox></strong></small>
                                                    <small><strong><asp:TextBox runat="server" CssClass="form-control tab_taxas" id="txtcredito20x" Text='<%# String.Format("{0:n2}",DataBinder.Eval(Container.DataItem, "NUM_VALOR_CREDITO_20X"))%>' Enabled="false"></asp:TextBox></strong></small><br />
                                                    <small><strong><asp:TextBox runat="server" CssClass="form-control tab_taxas" id="txtmarkup20xfixo" Text='<%# String.Format("{0:n2}",DataBinder.Eval(Container.DataItem, "NUM_VALOR_FIXO_MARKUP_20X"))%>'></asp:TextBox></strong></small>
                                                    <small><strong><asp:TextBox runat="server" CssClass="form-control tab_taxas" id="txtmarkup20x" Text='<%# String.Format("{0:n2}",DataBinder.Eval(Container.DataItem, "NUM_VALOR_MARKUP_20X"))%>'></asp:TextBox></strong></small>
                                                    <small><strong><asp:TextBox runat="server" CssClass="form-control tab_taxas" id="txtrebate20x" Text='<%# String.Format("{0:n2}",DataBinder.Eval(Container.DataItem, "NUM_VALOR_REBATE_20X"))%>'></asp:TextBox></strong></small><br />
                                                    <small><strong><asp:Label runat="server" id="Label20" Text='<%# String.Format("{0:n2}",DataBinder.Eval(Container.DataItem, "NUM_VALOR_FINAL_20X"))%>'></asp:Label></strong></small>
                                                    <small><strong><asp:Label runat="server" id="Label42" Text='<%# String.Format("{0:n2}",DataBinder.Eval(Container.DataItem, "NUM_VALOR_FINAL_FIXO_20X"))%>'></asp:Label></strong></small>
                                                </td>
                                                <td class="text-center" style="min-width:70px;" >
                                                    <small><strong><asp:TextBox runat="server" CssClass="form-control tab_taxas" id="txtcredito21xfixo" Text='<%# String.Format("{0:n2}",DataBinder.Eval(Container.DataItem, "NUM_VALOR_FIXO_CREDITO_21X"))%>' Enabled="false"></asp:TextBox></strong></small>
                                                    <small><strong><asp:TextBox runat="server" CssClass="form-control tab_taxas" id="txtcredito21x" Text='<%# String.Format("{0:n2}",DataBinder.Eval(Container.DataItem, "NUM_VALOR_CREDITO_21X"))%>' Enabled="false"></asp:TextBox></strong></small><br />
                                                    <small><strong><asp:TextBox runat="server" CssClass="form-control tab_taxas" id="txtmarkup21xfixo" Text='<%# String.Format("{0:n2}",DataBinder.Eval(Container.DataItem, "NUM_VALOR_FIXO_MARKUP_21X"))%>'></asp:TextBox></strong></small>
                                                    <small><strong><asp:TextBox runat="server" CssClass="form-control tab_taxas" id="txtmarkup21x" Text='<%# String.Format("{0:n2}",DataBinder.Eval(Container.DataItem, "NUM_VALOR_MARKUP_21X"))%>'></asp:TextBox></strong></small>
                                                    <small><strong><asp:TextBox runat="server" CssClass="form-control tab_taxas" id="txtrebate21x" Text='<%# String.Format("{0:n2}",DataBinder.Eval(Container.DataItem, "NUM_VALOR_REBATE_21X"))%>'></asp:TextBox></strong></small><br />
                                                    <small><strong><asp:Label runat="server" id="Label21" Text='<%# String.Format("{0:n2}",DataBinder.Eval(Container.DataItem, "NUM_VALOR_FINAL_21X"))%>'></asp:Label></strong></small>
                                                    <small><strong><asp:Label runat="server" id="Label43" Text='<%# String.Format("{0:n2}",DataBinder.Eval(Container.DataItem, "NUM_VALOR_FINAL_FIXO_21X"))%>'></asp:Label></strong></small>
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

<script>
function alterarColuna(index, valor) {
    // Obtém a tabela
    var tabela = document.getElementById("tbPresencial");

    // Obtém o prefixo do ID do DropdownList
    var dropdown = document.getElementById("ddlCampos");
    var prefixo = dropdown.value; // Prefixo selecionado

    // Percorre todas as linhas da tabela
    for (var i = 1; i < tabela.rows.length; i++) {
        var cell = tabela.rows[i].cells[index]; // Obtém a célula da coluna selecionada
        if (cell) {
            var inputs = cell.querySelectorAll("input"); // Busca todos os inputs dentro da célula
            inputs.forEach(input => {
                console.log("ID encontrado:", input.id); // Para depuração

                // Verifica se o ID contém o prefixo ou se é "txtdebito" quando o prefixo for "txtcredito"
                if ((input.id.includes(prefixo) || (prefixo === "txtcredito" && input.id.includes("txtdebito"))) 
                    && !input.id.includes("fixo")) {
                    
                    var atual = parseFloat(input.value.replace(",", ".")) || 0; // Converte para número
                    var novoValor = (atual + valor).toFixed(2); // Aplica incremento/decremento
                    input.value = novoValor.replace(".", ","); // Atualiza o campo
                }
            });
        }
    }
}

function alterarLinha(button, valor) {
    // Obtém a linha onde o botão foi clicado
    let row = button.closest("tr");

    // Obtém o prefixo do ID a partir do dropdown selecionado
    let dropdown = document.getElementById("ddlCampos");
    let prefixo = dropdown.value; // Prefixo selecionado

    // Pega todos os inputs da linha
    let inputs = row.querySelectorAll("input");

    inputs.forEach((input) => {
        console.log("ID encontrado:", input.id); // Para depuração

        // Verifica se o ID contém o prefixo e NÃO contém "fixo"
                // Verifica se o ID contém o prefixo ou se é "txtdebito" quando o prefixo for "txtcredito"
        if ((input.id.includes(prefixo) || (prefixo === "txtcredito" && input.id.includes("txtdebito"))) 
            && !input.id.includes("fixo")) {

            let atual = parseFloat(input.value.replace(",", ".")) || 0; // Converte para número
            let novoValor = (atual + valor).toFixed(2); // Aplica incremento/decremento
            input.value = novoValor.replace(".", ","); // Atualiza o campo
        }
    });
}
</script>


</body>
</html>
