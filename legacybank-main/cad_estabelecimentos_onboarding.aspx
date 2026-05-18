<%@ Page Language="C#" AutoEventWireup="true" CodeFile="cad_estabelecimentos_onboarding.aspx.cs" Inherits="cad_estabelecimentos_onboarding" Async="true" EnableSessionState="ReadOnly" MaintainScrollPositionOnPostback="true"  %>

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


    <style type="text/css">
        .btn-primary
        {
            height: 26px;
        }
    </style>


</head>
<body class="hold-transition sidebar-mini layout-fixed">
    <form id="frmPrincipal" runat="server">
        <!-- ************************************************************************************************************************************************************************** -->
        <!-- SETOR DE PASSOS DO PROCESSO DE ONBOARDING  -->
        <!-- ************************************************************************************************************************************************************************** -->
        <div class="row col-12" runat="server" id="dvAnalise" visible="false">
                                
            <div class="col">
                <div class="card">
                    <div class="card-body">
                        <small>
                            <center>
                                <i class="fas fa-chart-pie mr-1 fa-2x"></i><br />
                                Análise de Risco<br />     
                                <i runat="server" id="i8" class="fas fa-check-circle mr-1 text-success" visible="false"></i> 
                                <i runat="server" id="i9" class="fas fa-exclamation-circle mr-1 text-warning" visible="false"></i>  
                                <i runat="server" id="i10" class="fas fa-times-circle mr-1 text-danger"></i>  
                            </center>
                        </small>
                    </div>
                </div>
            </div>

        </div>
        <!-- ************************************************************************************************************************************************************************** -->
        <!-- DADOS DO ESTABELECIMENTO  -->
        <!-- ************************************************************************************************************************************************************************** -->


        <div class="row">
            <div class="col-12">

                <div class="card">
                    <div class="card-header bg-whitelabel1">
                        <h3 class="card-title">ESTABELECIMENTO ONBOARDING</h3>
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

                        <div class="row">
                            <div class="col-sm-4">
                                <div class="form-group">
                                    <label>Marketplace</label>
                                    <asp:TextBox runat="server" id="txtMarketplace" cssClass="form-control" Enabled="false"></asp:TextBox>
                                </div>
                            </div>
                            <div class="col-sm-4">
                                <div class="form-group">
                                    <label>Documento Marketplace</label>
                                    <asp:TextBox runat="server" id="txtDocumentoMarketplace" cssClass="form-control" Enabled="false"></asp:TextBox>
                                </div>
                            </div>
                            <div class="col-sm-4">
                                <div class="form-group">
                                    <label>Tipo Marketplace</label>
                                    <asp:TextBox runat="server" id="txtTipoMarketplace" cssClass="form-control" Enabled="false"></asp:TextBox>
                                </div>
                            </div>

                        </div>

                        
                        <div class="row">
                            <div class="col-sm-4">
                                <div class="form-group">
                                    <label>Representante</label>
                                    <asp:TextBox runat="server" id="txtRepresentante" cssClass="form-control" Enabled="false"></asp:TextBox>
                                </div>
                            </div>
                            <div class="col-sm-4">
                                <div class="form-group">
                                    <label>Documento Representante</label>
                                    <asp:TextBox runat="server" id="txtDocumentoRepresentante" cssClass="form-control" Enabled="false"></asp:TextBox>
                                </div>
                            </div>
                            <div class="col-sm-4">
                                <div class="form-group">
                                    <label>Tipo Representante</label>
                                    <asp:TextBox runat="server" id="txtTipoRepresentante" cssClass="form-control" Enabled="false"></asp:TextBox>
                                </div>
                            </div>

                        </div>

                        <div class="form-group">
                            <asp:TextBox runat="server" ID="txtResposta" Text="" Visible="false" CssClass="form-control"></asp:TextBox>
                        </div>






                        <!-- Processos de On-boarding -->
                                                
                        <h3><span class="float-center badge bg-whitelabel1 mt-2 mb-2">Processo de Onboarding</span></h3>

                        <div class="row mt-3">
                            <div class="col-sm-4">
                                <div class="form-group">
                                    <label>Adquirentes<strong class="text-danger">*</strong></label>
                                    <asp:DropDownList runat="server" id="ddlAdquirentes" CssClass="form-control" AutoPostBack="True" onselectedindexchanged="ddlAdquirentes_SelectedIndexChanged">
                                    </asp:DropDownList>
                                </div>
                            </div>

                            <div class="col-sm-4">
                                <div class="form-group">
                                    <label>Planos<strong class="text-danger">*</strong></label>
                                    <asp:DropDownList runat="server" id="ddlPlano" CssClass="form-control">
                                    </asp:DropDownList>
                                </div>
                            </div>
                            <div class="col-sm-4">
                                <br />
                                <asp:Button runat="server" ID="btnAtualizarPlano" Text="Atualizar Plano na Adquirente" CssClass="btn btn-whitelabel1" onclick="btnAtualizarPlano_Click" />
                            </div>


                        </div>

                        <div class="row">
                            <div class="col-sm-12">
                                <div class="card">
                                    <div class="card-header bg-whitelabel1">
                                        <h3 class="card-title"><asp:Label runat="server" ID="lblAdquirente" Text=""></asp:Label></h3>
                                    </div>
                                    <div class="card-body">

                                        <!-- *************************************************************************************************************************************** -->
                                        <!-- ADQUIRENTE CAPPTA  -->
                                        <!-- *************************************************************************************************************************************** -->

                                        <div class="row" runat="server" id="divAdquirenteCappta" visible="false">
                                            <div class="col-12">
                                                <div class="row">
                                                    <div class="col-sm-1"  >
                                                        <div class="form-group">
                                                        <label>Rep<strong class="text-danger">*</strong></label>
                                                        <asp:TextBox runat="server" ID="txtStatusRepresentante" CssClass="form-control"></asp:TextBox>
                                                        </div>
                                                    </div>
                                                    <div class="col-sm-1"  >
                                                        <div class="form-group">
                                                        <label>Mkt<strong class="text-danger">*</strong></label>
                                                        <asp:TextBox runat="server" ID="txtStatusMarketplace" CssClass="form-control"></asp:TextBox>
                                                        </div>
                                                    </div>

                                                    <div class="col-sm-3">
                                                        <div class="form-group">
                                                        <label>Habilitar Adquirente<strong class="text-danger">*</strong></label>
                                                        <asp:DropDownList runat="server" id="ddlCappta" CssClass="form-control">
                                                            <asp:ListItem Value="N">Não</asp:ListItem>
                                                            <asp:ListItem Value="S">Sim</asp:ListItem>
                                                        </asp:DropDownList>
                                                        </div>
                                                    </div>

                                                    <div class="col-sm-2" runat="server" id="dvStatus" visible="false">
                                                        <div class="form-group">
                                                        <label>Status<strong class="text-danger">*</strong></label>
                                                        <asp:TextBox runat="server" ID="txtStatusCappta" CssClass="form-control"></asp:TextBox>
                                                        </div>
                                                    </div>
                                                    <div class="col-sm-2" runat="server" id="dvNomeStatus" visible="true">
                                                        <div class="form-group">
                                                        <label>Status<strong class="text-danger">*</strong></label>
                                                        <asp:TextBox runat="server" ID="txtNomeStatusCappta" CssClass="form-control"></asp:TextBox>
                                                        </div>
                                                    </div>
                                                    <div class="col-sm-2"  >
                                                        <div class="form-group">
                                                        <label>Token<strong class="text-danger">*</strong></label>
                                                        <asp:TextBox runat="server" ID="txtTokenCappta" CssClass="form-control"></asp:TextBox>
                                                        </div>
                                                    </div>
                                                    <div class="col-sm-2">
                                                        <div class="form-group">
                                                            <label>Natureza<strong class="text-danger">*</strong></label>
                                                            <asp:DropDownList runat="server" id="ddlNatureza" CssClass="form-control">
                                                            </asp:DropDownList>
                                                        </div>
                                                    </div>
                                                </div>
                                                <div class="row">
                                                    <div class="col-sm-6">
                                                        <div class="form-group">
                                                            <label>Plano Adquirente<strong class="text-danger">*</strong></label>
                                                            <div class="input-group input-group-sm">
                                                                <asp:TextBox runat="server" ID="txtPlanoReferencia" CssClass="form-control" Enabled="false"></asp:TextBox>
                                                                <span class="input-group-append">
                                                                    <asp:LinkButton runat="server" ID="lkbImportarPlano" CssClass="btn btn-info btn-flat" onclick="lkbImportarPlano_Click">IMPORTAR     <i class="fas fa-file-import"></i></asp:LinkButton>
                                                                </span>
                                                            </div>
                                                        </div>
                                                    </div>
                                                    <div class="col-sm-12">
                                                        <asp:Label runat="server" ID="lblPlanoReferencia" Text=""></asp:Label>
                                                    </div>

                                                </div>

                                            </div>

                                        </div>

                                        <!-- *************************************************************************************************************************************** -->
                                        <!-- ADQUIRENTE ZOOP  -->
                                        <!-- *************************************************************************************************************************************** -->

                                        <div class="row" runat="server" id="divAdquirenteZoop" visible="false">
                                            <div class="col-12">

                                                <div class="row">
                                                    <div class="col-12">
                                                        <div class="row">
                                                            <div class="col">
                                                                <div class="card">
                                                                    <div class="card-body">
                                                                        <small>
                                                                            <center>
                                                                                <i class="fas fa-address-card mr-1 fa-2x"></i><br />
                                                                                Dados Cadastrais<br />     
                                                                                <i runat="server" id="dcZoopSim" class="fas fa-check-circle mr-1 text-success" visible="false"></i> 
                                                                                <i runat="server" id="dcZoopNao" class="fas fa-times-circle mr-1 text-danger"></i>  
                                                                            </center>
                                                                        </small>
                                                                    </div>
                                                                </div>
                                                            </div>
                                                            <div class="col">
                                                                <div class="card">
                                                                    <div class="card-body">
                                                                        <small>
                                                                            <center>
                                                                                <i class="fas fa-photo-video mr-1 fa-2x"></i><br />
                                                                                Documentação<br />     
                                                                                <i runat="server" id="docZoopSim" class="fas fa-check-circle mr-1 text-success" visible="false"></i> 
                                                                                <i runat="server" id="docZoopNao" class="fas fa-times-circle mr-1 text-danger"></i>  
                                                                            </center>
                                                                        </small>
                                                                    </div>
                                                                </div>
                                                            </div>
                                                            <div class="col">
                                                                <div class="card">
                                                                    <div class="card-body">
                                                                        <small>
                                                                            <center>
                                                                                <i class="fas fa-percent mr-1 fa-2x"></i><br />
                                                                                Plano<br />     
                                                                                <i runat="server" id="planZoopSim" class="fas fa-check-circle mr-1 text-success" visible="false"></i> 
                                                                                <i runat="server" id="planZoopNao" class="fas fa-times-circle mr-1 text-danger"></i>  
                                                                            </center>
                                                                        </small>
                                                                    </div>
                                                                </div>
                                                            </div>

                                                            <div class="col">
                                                                <div class="card">
                                                                    <div class="card-body">
                                                                        <small>
                                                                            <center>
                                                                                <i class="fas fa-money-check-alt mr-1 fa-2x"></i><br />
                                                                                Dados Bancários<br />     
                                                                                <i runat="server" id="dbZoopSim" class="fas fa-check-circle mr-1 text-success" visible="false"></i> 
                                                                                <i runat="server" id="dbZoopNao" class="fas fa-times-circle mr-1 text-danger"></i>  
                                                                            </center>
                                                                        </small>
                                                                    </div>
                                                                </div>
                                                            </div>

                                                            <div class="col">
                                                                <div class="card">
                                                                    <div class="card-body">
                                                                        <small>
                                                                            <center>
                                                                                <i class="fas fa-user-check mr-1 fa-2x"></i><br />
                                                                                Status Geral<br />     
                                                                                <i runat="server" id="sgZoopSim" class="fas fa-check-circle mr-1 text-success" visible="false"></i> 
                                                                                <i runat="server" id="sgZoopTalvez" class="fas fa-check-circle mr-1 text-warning" visible="false"></i> 
                                                                                <i runat="server" id="sgZoopNao" class="fas fa-times-circle mr-1 text-danger"></i>  
                                                                            </center>
                                                                        </small>
                                                                    </div>
                                                                </div>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>

                                                <div class="row">
                                                    <div class="col-sm-5"  >
                                                        <div class="form-group">
                                                        <label>Habilitar e gerenciar adquirente?<strong class="text-danger">*</strong></label>
                                                        <asp:DropDownList runat="server" id="ddlZoop" CssClass="form-control">
                                                            <asp:ListItem Value="0">Não</asp:ListItem>
                                                            <asp:ListItem Value="1">Sim</asp:ListItem>
                                                        </asp:DropDownList>
                                                        </div>
                                                    </div>
                                                    <div class="col-sm-1"  >
                                                        <div class="form-group">
                                                        <label>Status<strong class="text-danger">*</strong></label>
                                                        <asp:TextBox runat="server" ID="txtStatus" CssClass="form-control" placeholder="Status" Visible="false" ></asp:TextBox>
                                                        <asp:TextBox runat="server" ID="txtStatusZoop" CssClass="form-control" placeholder="Status" Visible="true" ></asp:TextBox>
                                                        </div>
                                                    </div>

                                                    <div class="col-sm-3"  >
                                                        <div class="form-group">
                                                        <label>Token<strong class="text-danger">*</strong></label>
                                                        <asp:TextBox runat="server" ID="txtToken" CssClass="form-control" placeholder="Token Adquirente" ></asp:TextBox>
                                                        </div>
                                                    </div>

                                                    <div class="col-sm-3"  >
                                                        <div class="form-group">
                                                        <label>Saque Automático?<strong class="text-danger">*</strong></label>
                                                        <asp:DropDownList runat="server" id="ddlSaque" CssClass="form-control">
                                                            <asp:ListItem Value="S" Text="Sim"></asp:ListItem>
                                                            <asp:ListItem Value="N" Text="Não"></asp:ListItem>
                                                        </asp:DropDownList>
                                                        </div>
                                                    </div>
                                                    <div class="col-sm-3"  >
                                                        <div class="form-group">
                                                        <label>Periodicidade de saque?<strong class="text-danger">*</strong></label>
                                                        <asp:DropDownList runat="server" id="ddlPeriodicidade" CssClass="form-control">
                                                            <asp:ListItem Value="D" Text="Diário"></asp:ListItem>
                                                            <asp:ListItem Value="S" Text="Semanal"></asp:ListItem>
                                                            <asp:ListItem Value="M" Text="Mensal"></asp:ListItem>
                                                        </asp:DropDownList>
                                                        </div>
                                                    </div>

                                                    <div class="col-sm-3"  >
                                                        <div class="form-group">
                                                        <label>Valor mínimo saque<strong class="text-danger">*</strong></label>
                                                        <asp:TextBox runat="server" ID="txtValorMinimo" CssClass="form-control" placeholder="100" ></asp:TextBox>
                                                        </div>
                                                    </div>


                                                </div>
                                            </div>
                                        </div>


                                        <!-- *************************************************************************************************************************************** -->
                                        <!-- ADQUIRENTE PAGSEGURO  -->
                                        <!-- *************************************************************************************************************************************** -->

                                        <div class="row" runat="server" id="divAdquirentePagseguro" visible="false">
                                            <div class="col-12">
                                                <div class="row">
                                                    <div class="col-sm-6"  >
                                                        <div class="form-group">
                                                        <label>Habilitar e gerenciar adquirente?<strong class="text-danger">*</strong></label>
                                                        <asp:DropDownList runat="server" id="ddlPagseguro" CssClass="form-control">
                                                            <asp:ListItem Value="0">Não</asp:ListItem>
                                                            <asp:ListItem Value="1">Sim</asp:ListItem>
                                                        </asp:DropDownList>
                                                        </div>
                                                    </div>

                                                    <div class="col-sm-3"  >
                                                        <div class="form-group">
                                                        <label>Código Ativação<strong class="text-danger">*</strong></label>
                                                        <asp:TextBox runat="server" ID="txtCodigoAtivacao" CssClass="form-control" placeholder="Código de ativação" ></asp:TextBox> 
                                                        </div>
                                                    </div>
                                          

                                                    <div class="col-sm-3"  >
                                                        <div class="form-group">
                                                        <label>Conta verificada e ativada<strong class="text-danger">*</strong></label>
                                                        <asp:DropDownList runat="server" id="ddlVerificadaAtivada" CssClass="form-control">
                                                            <asp:ListItem Value="Sim">Sim</asp:ListItem>
                                                            <asp:ListItem Value="Não">Não</asp:ListItem>
                                                            <asp:ListItem Value="Pendente de Documentos">Pendente de Documentos</asp:ListItem>
                                                            <asp:ListItem Value="Dados Inválidos">Dados Inválidos</asp:ListItem>
                                                            <asp:ListItem Value="Negada">Negada</asp:ListItem>
                                                        </asp:DropDownList>
                                                        </div>
                                                    </div>
                                                </div>

                                                <div class="row">
                                                    <div class="col-sm-4"  >
                                                        <div class="form-group">
                                                        <label>ID<strong class="text-danger">*</strong></label>
                                                        <asp:TextBox runat="server" ID="txtIDPagseguro" CssClass="form-control" placeholder="ID Adquirente" ></asp:TextBox> 
                                                        </div>
                                                    </div>
                                          

                                                    <div class="col-sm-4"  >
                                                        <div class="form-group">
                                                        <label>E-mail<strong class="text-danger">*</strong></label>
                                                        <asp:TextBox runat="server" ID="txtEmailPagseguro" CssClass="form-control" placeholder="E-mail ativação Adquirente" ></asp:TextBox>
                                                        </div>
                                                    </div>

                                                    <div class="col-sm-4"  >
                                                        <div class="form-group">
                                                        <label>Token<strong class="text-danger">*</strong></label>
                                                        <asp:TextBox runat="server" ID="txtTokenPagseguro" CssClass="form-control" placeholder="Token Adquirente" ></asp:TextBox>
                                                        </div>
                                                    </div>

                                                </div>
                                                <div class="row">
                                                    <div class="col-sm-4">
                                                        <div class="form-group">
                                                        <label>Forma de Recebimento<strong class="text-danger">*</strong></label>
                                                        <asp:DropDownList runat="server" id="ddlFormaRecebimento" CssClass="form-control">
                                                            <asp:ListItem Value="O">Por outra conta bancária</asp:ListItem>
                                                            <asp:ListItem Value="P">Pela adquirente</asp:ListItem>
                                                        </asp:DropDownList>
                                                        </div>
                                                    </div>
                                                </div>


                                            </div>

                                        </div>


                                        <!-- *************************************************************************************************************************************** -->
                                        <!-- ADQUIRENTE ASAAS  -->
                                        <!-- *************************************************************************************************************************************** -->

                                        <div class="row" runat="server" id="divAdquirenteAsaas" visible="false">
                                            <div class="col-12">
                                                <div class="row">
                                                    <div class="col-sm-4">
                                                        <div class="form-group">
                                                        <label>BaaS<strong class="text-danger">*</strong> (Habilitar e gerenciar BaaS)</label>
                                                        <asp:DropDownList runat="server" id="ddlBaaS" CssClass="form-control">
                                                            <asp:ListItem Value="N">Não</asp:ListItem>
                                                            <asp:ListItem Value="S">Sim</asp:ListItem>
                                                        </asp:DropDownList>
                                                        </div>
                                                    </div>
                                                    <div class="col-sm-2" runat="server" id="dvStatusAsaas" visible="false">
                                                        <div class="form-group">
                                                        <label>Status<strong class="text-danger">*</strong></label>
                                                        <asp:TextBox runat="server" ID="txtStatusAsaas" CssClass="form-control"></asp:TextBox>
                                                        </div>
                                                    </div>
                                                    <div class="col-sm-2" runat="server" id="dvNomeStatusAsaas" visible="true">
                                                        <div class="form-group">
                                                        <label>Status<strong class="text-danger">*</strong></label>
                                                        <asp:TextBox runat="server" ID="txtNomeStatus" CssClass="form-control"></asp:TextBox>
                                                        </div>
                                                    </div>

                                                    <div class="col-sm-6" runat="server" id="dvTokenAsaas" visible="true">
                                                        <div class="form-group">
                                                            <label>Token<strong class="text-danger">*</strong></label>
                                                            <div class="input-group input-group-sm">
                                                                <asp:TextBox runat="server" ID="txtTokenAsaas" CssClass="form-control"></asp:TextBox> 
                                                                <span class="input-group-append">
                                                                    <asp:LinkButton runat="server" ID="lkbVer" CssClass="btn btn-info btn-flat" onclick="lkbVer_Click"><i class="fas fa-eye<%=VerSenha()%>"></i></asp:LinkButton>
                                                                </span>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                                <div class="row">
                                                    <div class="col-sm-3" runat="server" id="dvIDContaAsaas" visible="false"  >
                                                        <div class="form-group">
                                                        <label>ID Conta<strong class="text-danger">*</strong></label>
                                                        <asp:TextBox runat="server" ID="txtIDConta" CssClass="form-control" Visible="false"></asp:TextBox>
                                                        </div>
                                                    </div>

                                                    <div class="col-sm-2"  >
                                                        <div class="form-group">
                                                        <label>Agência<strong class="text-danger">*</strong></label>
                                                        <asp:TextBox runat="server" ID="txtAgencia" CssClass="form-control"></asp:TextBox>
                                                        </div>
                                                    </div>
                                                    <div class="col-sm-2"  >
                                                        <div class="form-group">
                                                        <label>Conta<strong class="text-danger">*</strong></label>
                                                        <asp:TextBox runat="server" ID="txtConta" CssClass="form-control"></asp:TextBox>
                                                        </div>
                                                    </div>
                                                    <div class="col-sm-2"  >
                                                        <div class="form-group">
                                                        <label>Dígito Conta<strong class="text-danger">*</strong></label>
                                                        <asp:TextBox runat="server" ID="txtDigitoConta" CssClass="form-control"></asp:TextBox>
                                                        </div>
                                                    </div>
                                                    <div class="col-sm-4"  >
                                                        <div class="form-group">
                                                        <label>Chave Pix<strong class="text-danger">*</strong></label>
                                                        <asp:TextBox runat="server" ID="txtChavePix" CssClass="form-control"></asp:TextBox>
                                                        </div>
                                                    </div>
                                                    <div class="col-sm-2">
                                                        <label><strong class="text-danger"> </strong></label><br /><br />
                                                        <asp:LinkButton ID="lbkCopiar" runat="server" CssClass="btn btn-default" ToolTip="copiar"><i class="fas fa-copy"></i></asp:LinkButton>
                                                        <asp:LinkButton ID="lbkGerar" runat="server" CssClass="btn btn-default" 
                                                            ToolTip="gerar" onclick="lbkGerar_Click"><i class="fas fa-key"></i></asp:LinkButton>


                                                    </div>
                                                </div> 
                                                <div class="row">
                                                    <div class="col-sm-6">
                                                        <div class="form-group">
                                                        <label>Wallet ID<strong class="text-danger">*</strong></label>
                                                        <asp:TextBox runat="server" ID="txtWalletID" CssClass="form-control"></asp:TextBox>
                                                        </div>
                                                    </div>

                                                </div>
                                                <div class="row">
                                                    <div class="col-sm-12">
                                                        <div class="form-group">
                                                        <label class="text-whitelabel1">Só será possível excluir quando o saldo estiver zerado ou inferior a R$10,00<strong class="text-danger">*</strong></label>
                                                        <label><b>NOTA:</b> Se remover com saldo até R$10,00, o valor será perdido. Saldo superior a R$10,00 a plataforma não deixará você prosseguir com a remoção</label>
                                                        <asp:TextBox runat="server" ID="txtMotivo" CssClass="form-control" placeholder="Digite o motivo da exclusão da conta. Ex.: Solicitação do correntista"></asp:TextBox>
                                                        <asp:Button runat="server" ID="btnExcluir" CssClass="btn btn-danger" 
                                                                Text = "Excluir Conta" onclick="btnExcluir_Click" />
                                                        </div>
                                                    </div>

                                                </div>                                           
                                            
                                                <div class="row">
                                                    <div class="col-sm-12">
                                                        <asp:Button runat="server" ID="btnTimeline" CssClass="btn btn-info" 
                                                                Text = "Consultar histórico conta" onclick="btnTimeline_Click" />
                                                        </div>

                                                    </div>
                                                </div>

                                            </div>
                                        </div>

                                        <!-- *************************************************************************************************************************************** -->
                                        <!-- ADQUIRENTE ERP  -->
                                        <!-- *************************************************************************************************************************************** -->

                                        <div class="row" runat="server" id="divAdquirenteErp" visible="false">
                                            <div class="col-12">
                                                <div class="row">
                                                    <div class="col-sm-3">
                                                        <div class="form-group">
                                                        <label>Habilitar Adquirente<strong class="text-danger">*</strong></label>
                                                        <asp:DropDownList runat="server" id="ddlErp" CssClass="form-control">
                                                            <asp:ListItem Value="N">Não</asp:ListItem>
                                                            <asp:ListItem Value="S">Sim</asp:ListItem>
                                                        </asp:DropDownList>
                                                        </div>
                                                    </div>

                                                    <div class="col-sm-3"  >
                                                        <div class="form-group">
                                                        <label>Token<strong class="text-danger">*</strong></label>
                                                        <asp:TextBox runat="server" ID="txtTokenErp" CssClass="form-control"></asp:TextBox>
                                                        </div>
                                                    </div>

                                                    <div class="col-sm-3"  >
                                                        <div class="form-group">
                                                        <label>Usuário<strong class="text-danger">*</strong></label>
                                                        <asp:TextBox runat="server" ID="txtUsuarioErp" CssClass="form-control"></asp:TextBox>
                                                        </div>
                                                    </div>
                                                    <div class="col-sm-3"  >
                                                        <div class="form-group">
                                                        <label>App<strong class="text-danger">*</strong></label>
                                                        <asp:TextBox runat="server" ID="txtAppErp" CssClass="form-control"></asp:TextBox>
                                                        </div>
                                                    </div>

                                                </div>
                                            </div>

                                        </div>


                                    </div>
                                    <div class="card-footer">
                                        <asp:Button runat="server" ID="btnSalvar" CssClass="btn btn-whitelabel1" 
                                            Text="Salvar e enviar os dados" onclick="btnSalvar_Click"/>
                                    </div>
                                </div>
                            </div>

                        </div>


                    </div>
                    <div class="card-footer">
                        <asp:Button runat="server" ID="btnFechar" CssClass="btn btn-danger float-right" Text="Fechar" onclick="btnFechar_Click"/>
                    </div>
                </div>

            </div>            
            
        </div>

        <div class="modal fade" id="mdConfirmar">
        <div class="modal-dialog modal-sm">
            <div class="modal-content">
            <div class="modal-header">
                <h6 class="modal-title"><b>Autenticação de 2 Fatores - 2FA</b></h6>
                <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                <span aria-hidden="true">&times;</span>
                </button>
            </div>
            <div class="modal-body">
                <p>Para confirmar a operação, digite abaixo o código de confirmação que você recebeu por e-mail</p>
                <div class="input-group mb-3 col-12">
                    <asp:TextBox runat="server" ID="txt2FABoletos" CssClass="form-control" placeholder="Código de Confirmação" Visible="true"></asp:TextBox>
                    <div class="input-group-prepend">
                    <span class="input-group-text"><i class="fas fa-laptop-code"></i></span>
                    </div>
                </div>
                <p class="mt-1">
                    <strong>Não recebeu?  </strong>   <asp:LinkButton ID="lkbReenviar" runat="server" onclick="lkbReenviar_Click">Enviar novo código</asp:LinkButton>
                </p>                                                                                                                      
            </div>
            <div class="modal-footer justify-content-between">
                <button type="button" class="btn btn-danger" data-dismiss="modal">Cancelar</button>
                <asp:Button runat="server" ID="btnConfirmar2FA" CssClass="btn btn-primary" Text="Confirmar" onclick="btnConfirmar2FA_Click" />
            </div>
            </div>
        </div>
        </div>

        <div class="modal fade" id="mdToken">
        <div class="modal-dialog modal-sm">
            <div class="modal-content">
            <div class="modal-header">
                <h6 class="modal-title"><b>Token Conta Digital</b></h6>
                <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                <span aria-hidden="true">&times;</span>
                </button>
            </div>
            <div class="modal-body">
                <p>Token Conta Digital</p>
                <div class="input-group mb-3 col-12">
                    <asp:TextBox runat="server" ID="txtTokenAsaasExibir" CssClass="form-control"></asp:TextBox>
                    <div class="input-group-prepend">
                    <span class="input-group-text"><i class="fas fa-laptop-code"></i></span>
                    </div>
                </div>
            </div>
            <div class="modal-footer justify-content-between">
                <button type="button" class="btn btn-danger" data-dismiss="modal">Fechar</button>
            </div>
            </div>
        </div>
        </div>


        <div class="modal fade" id="mdContaDigital">
        <div class="modal-dialog modal-sm">
            <div class="modal-content">
            <div class="modal-header">
                <h6 class="modal-title"><b>Abertura de Conta</b></h6>
                <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                    <span aria-hidden="true">&times;</span>
                </button>
            </div>
            <div class="modal-body">
                <h5><strong>Atenção! Cobrança de Tarifa de Abertura de Conta</strong></h5>
                <p>Ao clicar em <b>"Confirmar"</b>, você concorda que será cobrada a tarifa de abertura de conta, independentemente da validação dos seus dados ou do envio dos documentos necessários.</p>
                <p>Caso não tenha certeza sobre a criação da conta neste momento, recomendamos que revise todas as informações antes de prosseguir.</p>
                <p>Se tiver dúvidas, entre em contato com nosso suporte.</p>
            </div>
            <div class="modal-footer justify-content-between">
                <button type="button" class="btn btn-danger" data-dismiss="modal">Cancelar</button>
                <asp:Button runat="server" ID="btnConfirmarContaDigital" CssClass="btn btn-whitelabel1" Text="Confirmar" onclick="btnConfirmarContaDigital_Click" />
            </div>
            </div>
        </div>
        </div>


    </form>

<script type="text/javascript" src="../plugins/datatables/jquery.dataTables.js"></script>
<script type="text/javascript" src="../plugins/datatables-bs4/js/dataTables.bootstrap4.js"></script>

<script src="../plugins/inputmask/jquery.inputmask.bundle.js"></script>
<script src="../plugins/moment/moment.min.js"></script>

<script>
    $(function () {

        //Datemask dd/mm/yyyy

        $('#txtNascimento').inputmask('99/99/9999')
        $('#txtDataAbertura').inputmask('99/99/9999')
        $('#txtCPF').inputmask('999.999.999-99')
        $('#txtCNPJ').inputmask('99.999.999/9999-99')
        $('#txtCelular').inputmask('(99) 99999-9999')
        $('#txtTelefoneEmpresa').inputmask('(99) 99999-9999')
        $('#txtCEP').inputmask('99999-999')

    })
</script>

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
