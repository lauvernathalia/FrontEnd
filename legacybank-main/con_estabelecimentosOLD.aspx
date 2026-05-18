<%@ Page Language="C#" AutoEventWireup="true" CodeFile="con_estabelecimentosOLD.aspx.cs" Inherits="con_estabelecimentosOLD" Async="true" EnableSessionState="ReadOnly" %>

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
                              <li class="breadcrumb-item active">Estabelecimentos</li>
                            </ol>
                          </div>
                        </div>




                        <div class="row">
                          <div class="col-12">
                            <div class="card">

                              <div class="card-header bg-whitelabel1">
                                <h3 class="card-title">ESTABELECIMENTOS</h3>
                              </div>
                              <div class="card-body">


                                <div class="row" runat="server" id="dvLimite" visible="true">
                                    <div class="col-12">
                                        <div class="alert alert-warning alert-dismissible">
                                          <button type="button" class="close" data-dismiss="alert" aria-hidden="true">&times;</button>
                                          <small><i class="icon fas fa-exclamation-triangle"></i> ATENÇÃO! O intervalo máximo permitido entre a data inicial e a data final é de 90 dias. Por favor, selecione um período dentro deste limite para realizar a consulta.</small>
                                        </div>
                                    </div>
                                </div>

                                <div class="form-group row col-12">
                                    <label class="col-2 col-form-label">Período de</label>
						            <div class="col-2">
                                        <div class="input-group date" id="datepickerIni" data-target-input="nearest">
								            <asp:TextBox ID="txtDataIni" runat="server" CssClass="form-control datetimepicker-input" data-target="#datepickerIni"></asp:TextBox>
                                            <div class="input-group-append" data-target="#datepickerIni" data-toggle="datetimepicker">
                                                <div class="input-group-text"><i class="fa fa-calendar"></i></div>
                                            </div>
                                        </div>
                                    </div>

                                    <label class="col-1 col-form-label">à</label>
						            <div class="col-2">
                                        <div class="input-group date" id="datepickerFim" data-target-input="nearest">
								            <asp:TextBox ID="txtDataFim" runat="server" CssClass="form-control datetimepicker-input" data-target="#datepickerFim"></asp:TextBox>
                                            <div class="input-group-append" data-target="#datepickerFim" data-toggle="datetimepicker">
                                                <div class="input-group-text"><i class="fa fa-calendar"></i></div>
                                            </div>
                                        </div>
                                    </div>

                                    <label class="col-sm-1 col-form-label">Status</label>
                                    <div class="col-sm-4">
                                        <asp:DropDownList runat="server" id="ddlAtivo" CssClass="form-control">
                                            <asp:ListItem Value=" ">Todos</asp:ListItem>
                                            <asp:ListItem Value="V">Novos</asp:ListItem>
                                            <asp:ListItem Value="S">Ativo</asp:ListItem>
                                            <asp:ListItem Value="N">Inativo</asp:ListItem>
                                            <asp:ListItem Value="P">Pendente</asp:ListItem>
                                        </asp:DropDownList>
                                    </div>
                                </div>




                                <div class="form-group row col-12">
                                    <label class="col-2 col-form-label">Razão Social/Nome</label>
                                    <div class="col-sm-2">
                                        <asp:TextBox runat="server" ID="txtNome" CssClass="form-control" placeholder="Digite a Razão Social ou Nome que deseja procurar"></asp:TextBox>
                                    </div>
                                    <label class="col-2 col-form-label">CNPJ/CPF</label>
                                    <div class="col-sm-2">
                                        <asp:TextBox runat="server" ID="txtCNPJCPF" CssClass="form-control" placeholder="Digite o CNPJ ou CPF que deseja procurar"></asp:TextBox>
                                    </div>
                                    <label class="col-1 col-form-label">E-mail</label>
                                    <div class="col-sm-3">
                                        <asp:TextBox runat="server" ID="txtEmail" CssClass="form-control" placeholder="Digite o E-mail que deseja procurar"></asp:TextBox>
                                    </div>
                                </div>

                                <div class="form-group row col-12">
                                    <div class="col-sm-12">
                                        <asp:Button runat="server" ID="btnPesquisar" CssClass="btn btn-sm btn-whitelabel1" Text="Pesquisar" onclick="btnPesquisar_Click"/>
                                        <asp:Button runat="server" ID="btnNovo" CssClass="btn btn-sm btn-whitelabel1 float-right" Text="(+) Novo Estabelecimento" onclick="btnNovo_Click"/>
                                        <asp:Button runat="server" ID="btnImportar" CssClass="btn btn-sm btn-whitelabel1 float-right" Text="Importar CNPJ/CPF" onclick="btnImportar_Click"/>
                                        <asp:Button runat="server" ID="btnNovoPadrao" CssClass="btn btn-sm btn-whitelabel1 float-right" Text="(+) Novo Registro" onclick="btnNovoPadrao_Click" Visible="false"/>
                                        <asp:Button ID="btnPostback" runat="server" Visible="false" OnClick="btnPostBack_Click" />
                                        <script type="text/javascript">
                                            function PostBackOnMainPage(){
                                            <%=GetPostBackScript()%>
                                            }
                                        </script>

                                    </div>

                                </div>

                                </div>

                              <div class="card-footer">

                                <div class="row">
                                    <div class="col-8">
                                        <div class="card card-outline">
                                          <div class="card-body">
                                            <div class="row">
                                            <asp:Repeater runat="server" ID="rptStatus">
                                                <ItemTemplate>

                                                    <div class="col-sm-2">
                                                        <p class="btn btn-app text-whitelabel1">
                                                          <span class="badge bg-warning"><%# DataBinder.Eval(Container.DataItem, "NUM_REGISTROS")%></span>
                                                          <i class='fas <%# DataBinder.Eval(Container.DataItem, "NOM_FLG_ATIVO_ICONE")%> text-whitelabel1'></i> <%# DataBinder.Eval(Container.DataItem, "NOM_FLG_ATIVO")%>
                                                        </p>
                                                    </div>

                                                </ItemTemplate>
                                            </asp:Repeater>
                                            </div>
                                          </div>
                                        </div>
                                    </div>
                                    <div class="col-4">
                                        <div class="card card-outline">
                                          <div class="card-body">
                                            <div class="row">
                                            <asp:Repeater runat="server" ID="rptTipo">
                                                <ItemTemplate>
                                            
                                                    <div class="col-sm-6">

                                                    <div class="col-sm-2">
                                                        <p class="btn btn-app text-whitelabel1">
                                                          <span class="badge bg-warning"><%# DataBinder.Eval(Container.DataItem, "NUM_REGISTROS")%></span>
                                                          <i class='fas <%# DataBinder.Eval(Container.DataItem, "NOM_FLG_ATIVO_ICONE")%> text-whitelabel1'></i> <%# DataBinder.Eval(Container.DataItem, "NOM_FLG_ATIVO")%>
                                                        </p>
                                                    </div>

                                                    </div>


                                                  <!--<div class="col-md-6 col-xs-12 my-1 float-right">
                                                    <div class='small-box bg-<%# DataBinder.Eval(Container.DataItem, "NOM_FLG_ATIVO_COR")%>'>
                                                      <div class="inner">
                                                        <h3><%# DataBinder.Eval(Container.DataItem, "NUM_REGISTROS")%></h3>
                                                        <p><%# DataBinder.Eval(Container.DataItem, "NOM_FLG_ATIVO")%></p>
                                                      </div>
                                                      <div class="icon">
                                                        <i class='fas <%# DataBinder.Eval(Container.DataItem, "NOM_FLG_ATIVO_ICONE")%>'></i>
                                                      </div>
                                                    </div>
                                                  </div>-->

                                                </ItemTemplate>
                                            </asp:Repeater>
                                            </div>
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
                                <h3 class="card-title">CADASTROS</h3>
                              </div>
                              <div class="card-body" style="overflow:auto; width: 100%; ">

                                <div class="row">
                                    <div class="col-12" runat="server" id="dvCompleta" visible="false">

                                        <table id="tblCompleta" class="table table-bordered table-hover">

                                                    <thead>
                                                    <tr>
                                                        <th><small><strong>Data</strong></small></th>
                                                        <th><small><strong>Código</strong></small></th>
                                                        <th><small><strong>Editar</strong></small></th>
                                                        <th><small><strong>Status</strong></small></th>
                                                        <th><small><strong>Alterar</strong></small></th>
                                                        <th><small><strong>ID Adquirente</strong></small></th>
                                                        <th><small><strong>Tipo</strong></small></th>
                                                        <th><small><strong>Estabelecimento</strong></small></th>
                                                        <th><small><strong>CNPJ/CPF</strong></small></th>
                                                        <th><small><strong>E-mail</strong></small></th>
                                                        <!--
                                                        <th><small><strong>Telefone</strong></small></th>
                                                        <th><small><strong>Responsável</strong></small></th>
                                                        <th><small><strong>CPF</strong></small></th>
                                                        <th><small><strong>E-mail</strong></small></th>
                                                        <th><small><strong>Celular</strong></small></th>
                                                        -->
                                                        <th><small><strong>MKT</strong></small></th>
                                                        <th><small><strong>REP</strong></small></th>
                                                        <th><small><strong></strong></small></th>
                                                        <th><small><strong>Adquirentes</strong></small></th>
                                                        <th><small><strong>...</strong></small></th>
                                                        <th><small><strong>Excluir</strong></small></th>
                                                    </tr>
                                                    </thead>
                                                    <tbody>


                                            <asp:Repeater runat="server" ID="rptConsultaCompleta" OnItemCommand="rptConsultaCompleta_OnItemCommand">
                                                <ItemTemplate>

                                                    <tr>

                                                      <td>
                                                          <small><strong><%# String.Format("{0:yyyy/MM/dd}",DataBinder.Eval(Container.DataItem, "DTA_DATA"))%></strong></small>
                                                      </td>

                                                      <td>
                                                          <small><strong><%# DataBinder.Eval(Container.DataItem, "COD_ID")%></strong></small>
                                                      </td>

                                                      <td>
                                                        <a href="" onclick="javascript:openPopupWindow('cad_estabelecimentos_padrao.aspx?id=<%# Funcoes.Encrypt(DataBinder.Eval(Container.DataItem, "COD_ID").ToString())%>', 'EstabelecimentosEdicao', 1024, 800); return false;" class="btn btn-sm btn-info">
                                                            <i class="fas fa-edit"></i>
                                                        </a>
                                                      </td>
                                                      <td>
                                                          <small><strong><%# DataBinder.Eval(Container.DataItem, "NOM_FLG_ATIVO")%></strong></small>
                                                      </td>

                                                    <td>
                                                        <asp:linkbutton ID="lkbAtivar" CssClass="badge badge-success" commandname="Ativar" runat="server" text="Ativar" ToolTip="Ativar"  CommandArgument='<%# DataBinder.Eval(Container.DataItem, "COD_ID")%>'><i class="fas fa-check"></i></asp:linkbutton>
                                                        <asp:linkbutton ID="lkbInativar" CssClass="badge badge-danger" commandname="Inativar" runat="server" text="Inativar" ToolTip="Inativar"  CommandArgument='<%# DataBinder.Eval(Container.DataItem, "COD_ID")%>'><i class="fas fa-times"></i></asp:linkbutton>
                                                        <!--<asp:linkbutton ID="lkbPendente" CssClass="badge badge-warning" commandname="Pendente" runat="server" text="Pendente" ToolTip="Pendente"  CommandArgument='<%# DataBinder.Eval(Container.DataItem, "COD_ID")%>'><i class="fas fa-exclamation"></i></asp:linkbutton>
                                                        <asp:linkbutton ID="lkbNovo" CssClass="badge badge-primary" commandname="Novo" runat="server" text="Novo" ToolTip="Novo"  CommandArgument='<%# DataBinder.Eval(Container.DataItem, "COD_ID")%>'><i class="fas fa-user"></i></asp:linkbutton>-->
                                                    </td>
                                                      <td>
                                                          <small><%# DataBinder.Eval(Container.DataItem, "COD_ID_PAGSEGURO")%></small>
                                                      </td>


                                                      <td>
                                                          <small><strong><%# DataBinder.Eval(Container.DataItem, "FLG_TIPO_PESSOA")%></strong></small>
                                                      </td>

                                                      <td>
                                                          <small><strong><%# DataBinder.Eval(Container.DataItem, "NOM_RAZAOSOCIAL")%></strong></small>
                                                      </td>
                                                      <td>
                                                          <small><%# DataBinder.Eval(Container.DataItem, "NOM_CNPJ")%></small>
                                                      </td>

                                                      <td>
                                                          <small><%# DataBinder.Eval(Container.DataItem, "NOM_EMAIL_EMPRESA")%></small>
                                                      </td>
                                                      <!--
                                                      <td>
                                                          <small><%# DataBinder.Eval(Container.DataItem, "NUM_TELEFONE")%></small>
                                                      </td>

                                                      <td>
                                                          <small><%# DataBinder.Eval(Container.DataItem, "NOM_NOME")%></small> <small><%# DataBinder.Eval(Container.DataItem, "NOM_SOBRENOME")%></small>
                                                      </td>
                                                      <td>
                                                          <small><%# DataBinder.Eval(Container.DataItem, "NOM_CPF")%></small>
                                                      </td>  


                                                      <td>
                                                          <small><%# DataBinder.Eval(Container.DataItem, "NOM_EMAIL")%></small>
                                                      </td>
                                                      <td>
                                                          <small><%# DataBinder.Eval(Container.DataItem, "NOM_CELULAR")%></small>
                                                      </td>
                                                      -->
                                                      <td>
                                                          <small><%# DataBinder.Eval(Container.DataItem, "NOM_RAZAOSOCIAL_MARKETPLACE")%></small>
                                                      </td>
                                                      <td>
                                                          <small><%# DataBinder.Eval(Container.DataItem, "NOM_RAZAOSOCIAL_REPRESENTANTE")%></small>
                                                      </td>


                                                      <td>
                                                            <asp:linkbutton ID="lkbVerificar" commandname="Verificar" runat="server" text="Verificar Status" ToolTip="Verificar Status"  CommandArgument='<%# DataBinder.Eval(Container.DataItem, "COD_ID")%>'><i class="fas fa-recycle text-success"></i></asp:linkbutton>
                                                      </td>

                                                        <td>
                                                            <small><%# DataBinder.Eval(Container.DataItem, "NOM_STATUS_ZOOP_ONBOARDING")%></small><br />
                                                            <small><%# DataBinder.Eval(Container.DataItem, "NOM_STATUS_CAPPTA_ONBOARDING")%></small><br />
                                                            <small><%# DataBinder.Eval(Container.DataItem, "NOM_STATUS_BAAS_ONBOARDING")%></small>
                                                            <small><%# DataBinder.Eval(Container.DataItem, "NOM_STATUS_PAGSEGURO_ONBOARDING")%></small>
                                                        </td>

                                                              <td>
                                                                <div class="btn-group">
                                                                    <button type="button" class="btn btn-default"><small>Ações</small></button>
                                                                    <button type="button" class="btn btn-default dropdown-toggle" data-toggle="dropdown">
                                                                      <span class="caret"></span>
                                                                      <span class="sr-only">Toggle Dropdown</span>
                                                                    </button>
                                                                    <div class="dropdown-menu" role="menu">
                                                                      <small>
                                                                      <a class="dropdown-item" href="" onclick="javascript:openPopupWindow('cad_estabelecimentos_usuarios.aspx?id=<%# Funcoes.Encrypt(DataBinder.Eval(Container.DataItem,"COD_ID").ToString())%>', 'EstabelecimentosUsuarios', 1024, 800); return false;"><i class="fas fa-users"></i>     Usuários</a>
                                                                      <a class="dropdown-item" href="" onclick="javascript:openPopupWindow('cad_estabelecimentos_contas.aspx?id=<%# Funcoes.Encrypt(DataBinder.Eval(Container.DataItem,"COD_ID").ToString())%>', 'EstabelecimentosContas', 1024, 800); return false;"><i class="fas fa-piggy-bank"></i>     Contas Bancárias</a>
                                                                      <a class="dropdown-item" href="" onclick="javascript:openPopupWindow('cad_estabelecimentos_documentos.aspx?id=<%# Funcoes.Encrypt(DataBinder.Eval(Container.DataItem,"COD_ID").ToString())%>', 'EstabelecimentosDocumentos', 1024, 800); return false;"><i class="fas fa-passport"></i>     Documentos</a>
                                                                      <a class="dropdown-item" href="" onclick="javascript:openPopupWindow('cad_estabelecimentos_onboarding.aspx?id=<%# Funcoes.Encrypt(DataBinder.Eval(Container.DataItem,"COD_ID").ToString())%>', 'EstabelecimentosOnboarding', 1024, 800); return false;"><i class="fas fa-money-check-alt"></i>     Onboarding</a>
                                                                      <a class="dropdown-item" href="" onclick="javascript:openPopupWindow('cad_estabelecimentos_comercial.aspx?id=<%# Funcoes.Encrypt(DataBinder.Eval(Container.DataItem,"COD_ID").ToString())%>', 'EstabelecimentosComercial', 1024, 800); return false;"><i class="fas fa-divide"></i>     Comercial</a>
                                                                      <a class="dropdown-item" href="" onclick="javascript:openPopupWindow('cad_estabelecimentos_ajustes_financeiros.aspx?id=<%# Funcoes.Encrypt(DataBinder.Eval(Container.DataItem,"COD_ID").ToString())%>', 'EstabelecimentosComercial', 1024, 800); return false;"><i class="fas fa-calculator"></i>     Ajustes Financeiros</a>
                                                                      <a class="dropdown-item" href="" onclick="javascript:openPopupWindow('cad_estabelecimentos_markup.aspx?id=<%# Funcoes.Encrypt(DataBinder.Eval(Container.DataItem,"COD_ID").ToString())%>', 'EstabelecimentosComercial', 1024, 800); return false;"><i class="fas fa-percent"></i>     Markup</a>
                                                                      <a class="dropdown-item" href="" onclick="javascript:openPopupWindow('cad_estabelecimentos_taxas.aspx?id=<%# Funcoes.Encrypt(DataBinder.Eval(Container.DataItem,"COD_ID").ToString())%>', 'EstabelecimentosTaxas', 1024, 800); return false;"><i class="fas fa-percent"></i>     Taxas</a>
                                                                      <a class="dropdown-item" href="" onclick="javascript:openPopupWindow('cad_estabelecimentos_api.aspx?id=<%#DataBinder.Eval(Container.DataItem,"COD_ID")%>', 'EstabelecimentosApi', 1024, 800); return false;"><i class="fas fa-cloud-upload-alt"></i>     APIs</a>
                                                                      <a class="dropdown-item" href="" onclick="javascript:openPopupWindow('cad_estabelecimentos_assinaturas.aspx?id=<%# Funcoes.Encrypt(DataBinder.Eval(Container.DataItem,"COD_ID").ToString())%>', 'EstabelecimentosAssinaturas', 1024, 800); return false;"><i class="fas fa-file"></i>     Assinaturas/Aceites</a>
                                                                      </small>
                                                                      <!--<div class="dropdown-divider"></div>
                                                                      <a class="dropdown-item" href="#">04</a>-->
                                                                    </div>
                                                                  </div>                                                              
                                                              </td>                                                      



                                                      <td>
                                                            <asp:TextBox runat="server" ID="txtExcluir" Text='<%# DataBinder.Eval(Container.DataItem, "NUM_TRANSACOES")%>' Visible="false"></asp:TextBox>
                                                            <asp:linkbutton ID="lbkExcluir" commandname="Excluir" runat="server" text="Excluir" ToolTip="Excluir"  CommandArgument='<%# DataBinder.Eval(Container.DataItem, "COD_ID")%>'><i class="fas fa-trash text-danger"></i></asp:linkbutton>
                                                      </td>

                                                    </tr>
                                                </ItemTemplate>
                                            </asp:Repeater>

                                            </tbody>
                                        </table>
                                      </div>
                                      <!-- Tabela Parcial -->
                                    <div class="col-12" runat="server" id="dvParcial" visible="false">
                                        <table id="tblParcial" class="table table-bordered table-hover">
                                            <thead>
                                            <tr>
                                              <th>Data</th>
                                              <th>ID</th>
                                              <th>Editar</th>
                                              <th>Status</th>
                                              <th>Alterar</th>
                                              <th>Estabelecimento</th>
                                              <th>Responsável</th>
                                              <th>E-mail</th>
                                              <th>Telefone</th>
                                              <th>Representante</th>
                                            </tr>
                                            </thead>
                                            <tbody>
                                            <asp:Repeater runat="server" ID="rptConsultaParcial" OnItemCommand="rptConsultaParcial_OnItemCommand">
                                                <ItemTemplate>

                                                    <tr>
                                                      <td>
                                                          <small><strong><%# String.Format("{0:yyyy/MM/dd}",DataBinder.Eval(Container.DataItem, "DTA_DATA"))%></strong></small>
                                                      </td>

                                                      <td>
                                                          <small><strong><%# DataBinder.Eval(Container.DataItem, "COD_ID")%></strong></small>
                                                      </td>

                                                      <td>
                                                        <a href="" onclick="javascript:openPopupWindow('cad_estabelecimentos.aspx?id=<%#DataBinder.Eval(Container.DataItem,"COD_ID")%>', 'EstabelecimentosEdicao', 1024, 800); return false;" class="btn btn-sm btn-app">
                                                            <i class="fas fa-edit"></i> Editar
                                                        </a>
                                                      </td>

                                                      <td>
                                                          <small><strong><%# DataBinder.Eval(Container.DataItem, "NOM_FLG_ATIVO")%></strong></small>
                                                      </td>

                                                    <td>
                                                        <asp:linkbutton ID="lkbAtivar" CssClass="badge badge-success" commandname="Ativar" runat="server" text="Ativar" ToolTip="Ativar"  CommandArgument='<%# DataBinder.Eval(Container.DataItem, "COD_ID")%>'><i class="fas fa-check"></i>   Ativar</asp:linkbutton><br />
                                                        <asp:linkbutton ID="lkbInativar" CssClass="badge badge-danger" commandname="Inativar" runat="server" text="Inativar" ToolTip="Inativar"  CommandArgument='<%# DataBinder.Eval(Container.DataItem, "COD_ID")%>'><i class="fas fa-times"></i>   Inativar</asp:linkbutton><br />
                                                        <asp:linkbutton ID="lkbPendente" CssClass="badge badge-warning" commandname="Pendente" runat="server" text="Pendente" ToolTip="Pendente"  CommandArgument='<%# DataBinder.Eval(Container.DataItem, "COD_ID")%>'><i class="fas fa-exclamation"></i>   Pendente</asp:linkbutton>
                                                        <asp:linkbutton ID="lkbNovo" CssClass="badge badge-primary" commandname="Novo" runat="server" text="Novo" ToolTip="Novo"  CommandArgument='<%# DataBinder.Eval(Container.DataItem, "COD_ID")%>'><i class="fas fa-user"></i>   Novo</asp:linkbutton>
                                                    </td>


                                                      <td>
                                                          <small><strong><%# DataBinder.Eval(Container.DataItem, "NOM_RAZAOSOCIAL")%></strong></small><br />
                                                          <small><%# DataBinder.Eval(Container.DataItem, "NUM_DOCUMENTO")%></small>
                                                      </td>

                                                      <td>
                                                          <small><%# DataBinder.Eval(Container.DataItem, "NOM_NOME")%></small> <small><%# DataBinder.Eval(Container.DataItem, "NOM_SOBRENOME")%></small>
                                                      </td>

                                                      <td>
                                                          <small><%# DataBinder.Eval(Container.DataItem, "NOM_EMAIL")%></small>
                                                      </td>
                                                      <td>
                                                          <small><%# DataBinder.Eval(Container.DataItem, "NOM_CELULAR")%></small><br />
                                                          <small><%# DataBinder.Eval(Container.DataItem, "NUM_TELEFONE")%></small>
                                                      </td>
                                                      <td>
                                                          <small><%# DataBinder.Eval(Container.DataItem, "NOM_RAZAOSOCIAL_REPRESENTANTE")%></small>
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

        //Date range picker
        $('#datepickerIni').datetimepicker({
            format: 'DD/MM/YYYY'
        });
        $('#datepickerFim').datetimepicker({
            format: 'DD/MM/YYYY'
        });
    })
</script>        


<!--
<script type="text/javascript">
    $(function () {
        $('[id$=gvConsultaCompleta]').prepend($("<thead></thead>").append($('[id$=gvConsultaCompleta]').find("tr:first"))).DataTable({
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

        $('a.toggle-vis').on('click', function (e) {
            e.preventDefault();

            // Get the column API object
            var column = table.column($(this).attr('data-column'));

            // Toggle the visibility
            column.visible(!column.visible());
        });


    });
</script>
-->
<!--
<script type="text/javascript">
    $(function () {
        $('[id$=tblCompleta]').prepend($("<thead></thead>").append($('[id$=tblCompleta]').find("tr:first"))).DataTable({
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

        $('a.toggle-vis').on('click', function (e) {
            e.preventDefault();

            // Get the column API object
            var column = table.column($(this).attr('data-column'));

            // Toggle the visibility
            column.visible(!column.visible());
        });


    });
</script>
-->

<script type="text/javascript">

$(document).ready(function () {
    var table = $('#tblCompleta').DataTable({
        order: [[0, 'desc']],
        "responsive": false,
        "sPaginationType": "full_numbers",
        "oLanguage": {
            "sUrl": '//cdn.datatables.net/plug-ins/2.0.6/i18n/pt-BR.json'
        },
        "paging": true,
        "lengthChange": true,
        "searching": true,
        "ordering": true,
        "info": true,
        "autoWidth": true,
        buttons: [
                {
                    extend: 'copy',
                    exportOptions: {
                        columns: ':visible'
                    }
                },        
                {
                    extend: 'excel',
                    exportOptions: {
                        columns: ':visible'
                    }
                },        
                {
                    extend: 'pdf',
                       text: 'Export PDF',
                       orientation: 'landscape',
                       pageSize: 'LEGAL',
                       customize: function ( doc ) {
                         doc.pageMargins = [10,10,10,10]
                     },
                     exportOptions: {
                        columns: ':visible'
                    }
                },
                {
                    extend: 'csv',
                    exportOptions: {
                        columns: ':visible'
                    }
                },

                {
                    extend: 'print',
                    exportOptions: {
                        columns: ':visible'
                    }
                },
                'colvis'
        ],
        initComplete: function () {
            this.api().buttons().container()
            //.appendTo( $ ('#table_id_wrapper .col-md-6:eq(0)', this.api().table (). container ()));
            //.appendTo( $('#table_id_wrapper .col-md-6:eq(0)' ) );
        .appendTo($('.col-md-6:eq(0)', this.api().table().container()));

        }

  });
 
    $('a.toggle-vis').on('click', function (e) {
        e.preventDefault();
 
        // Get the column API object
        var column = table.column($(this).attr('data-column'));
 
        // Toggle the visibility
        column.visible(!column.visible());
    });
});

$(document).ready(function () {
    var table = $('#tblParcial').DataTable({
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

</body>
</html>
