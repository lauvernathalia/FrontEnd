<%@ Page Language="C#" AutoEventWireup="true" CodeFile="con_teste_api_asaas.aspx.cs" Inherits="con_teste_api_asaas" Async="true" %>

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

    <!-- Implementação do Sistema Anti-fraude -->

    <script type="text/javascript" src="https://h.online-metrix.net/fp/tags.js?org_id=1snn5n9w&session_id=adiq_br<%=AntifraudeGUID() %>"></script>    

</head>
<body class="hold-transition sidebar-mini layout-fixed">

<noscript><iframe style="width: 100px; height: 100px; border: 0; position:absolute; top: -5000px;" src="https://h.online-metrix.net/fp/tags.js?org_id=1snn5n9w&session_id=adiq_br<%=AntifraudeGUID() %>"></iframe></noscript>

    <form id="frmPrincipal" runat="server">



        <div class="wrapper">
            <PORTAL:PAGEHEADER id="PageHeader1" title="Site Directory" runat="server" ModuleSource="topopadrao.ascx"></PORTAL:PAGEHEADER>
            <PORTAL:PAGELEFT id="Pageheader2" title="Site Directory" runat="server" ModuleSource="menupadrao.ascx"></PORTAL:PAGELEFT>
            <div class="content-wrapper">
                <section class="content">
                    <div class="container-fluid" >

                        <div class="row">
                            <div class = "col-12">
                            
                            </div>
                        </div>


                        <div class="row">
                          <div class="col-12">
                            <div class="card">

                              <div class="card-header bg-whitelabel1">
                                <h3 class="card-title">ADIQ</h3>
                              </div>
                              <div class="card-body">




                                <div class="form-group row col-12">
                                    <label class="col-sm-2 col-form-label">JSON</label>
                                    <div class="col-sm-10">
                                        <asp:TextBox runat="server" ID="txtJsonAdiq" TextMode="MultiLine" Rows="5" CssClass="form-control"></asp:TextBox>
                                    </div>
                                </div>

                              </div>
                              <div class="card-footer">
                                <asp:Button runat="server" ID="btnAdiq" CssClass="btn btn-whitelabel1" Text="Pesquisar" onclick="btnAdiq_Click" />

                              </div>
                            </div>
                          </div>
                        </div>


                        <div class="row">
                          <div class="col-6">
                            <div class="card">

                              <div class="card-header bg-whitelabel1">
                                <h3 class="card-title">ASAAS</h3>
                              </div>
                              <div class="card-body">
                                <div class="form-group row col-12">
                                    <label class="col-sm-12 col-form-label">JSON</label>
                                    <div class="col-sm-12">
                                        <asp:Label runat="server" ID="lblJsonAsaas"></asp:Label>
                                        <asp:TextBox runat="server" ID="txtJsonAsaas" TextMode="MultiLine" Rows="10" CssClass="form-control"></asp:TextBox>
                                    </div>
                                </div>
                              </div>
                              <div class="card-footer">
                                <asp:Button runat="server" ID="btnAsaas" 
                                      CssClass="btn btn-whitelabel1" Text="Pesquisar" 
                                      onclick="btnAsaas_Click" />

                              </div>
                            </div>
                          </div>

                          <div class="col-6">
                            <div class="card">

                              <div class="card-header bg-whitelabel1">
                                <h3 class="card-title">WENHOOKS</h3>
                              </div>
                              <div class="card-body">
                                <div class="form-group row col-12">
                                    <label class="col-sm-12 col-form-label">JSON</label>
                                    <div class="col-sm-12">
                                        <asp:TextBox runat="server" ID="txtJsonWebhooks" TextMode="MultiLine" Rows="10" CssClass="form-control"></asp:TextBox>
                                    </div>
                                </div>
                              </div>
                              <div class="card-footer">
                                <asp:Button runat="server" ID="btnWebhooks" 
                                      CssClass="btn btn-whitelabel1" Text="Pesquisar" 
                                      onclick="btnWebhooks_Click" />

                              </div>
                            </div>
                          </div>

                          <div class="col-6">
                            <div class="card">

                              <div class="card-header bg-whitelabel1">
                                <h3 class="card-title">ACCOUNTS</h3>
                              </div>
                              <div class="card-body">

                                <div class="form-group row col-12">
                                    <asp:Label runat="server" ID="lblAccounts"></asp:Label>
                                </div>

                                <div class="form-group row col-12">
                                    <label class="col-sm-12 col-form-label">Documento</label>
                                    <div class="col-sm-12">
                                        <asp:TextBox runat="server" ID="txtDocumento" CssClass="form-control"></asp:TextBox>
                                    </div>
                                </div>

                                <div class="form-group row col-12">
                                    <label class="col-sm-12 col-form-label">JSON</label>
                                    <div class="col-sm-12">
                                        <asp:TextBox runat="server" ID="txtJsonAccounts" TextMode="MultiLine" Rows="10" CssClass="form-control"></asp:TextBox>
                                    </div>
                                </div>
                              </div>
                              <div class="card-footer">
                                <asp:Button runat="server" ID="btnAccounts" 
                                      CssClass="btn btn-whitelabel1" Text="Pesquisar" 
                                      onclick="btnAccounts_Click" />

                              </div>
                            </div>
                          </div>



                          <div class="col-6">
                            <div class="card">

                              <div class="card-header bg-whitelabel1">
                                <h3 class="card-title">CLIENTE E COBRANÇA</h3>
                              </div>
                              <div class="card-body">
                                <div class="form-group row col-12">
                                    <label class="col-sm-12 col-form-label">JSON</label>
                                    <div class="col-sm-12">
                                        <asp:TextBox runat="server" ID="txtJsonCobranca" TextMode="MultiLine" Rows="10" 
                                            CssClass="form-control"></asp:TextBox>
                                    </div>
                                </div>
                              </div>
                              <div class="card-footer">
                                <asp:Button runat="server" ID="btnCobranca" 
                                      CssClass="btn btn-whitelabel1" Text="Pesquisar" 
                                      onclick="btnCobranca_Click" />

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

</body>
</html>

