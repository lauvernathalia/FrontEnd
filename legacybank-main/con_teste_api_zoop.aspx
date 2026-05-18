<%@ Page Language="C#" AutoEventWireup="true" CodeFile="con_teste_api_zoop.aspx.cs" Inherits="con_teste_api_zoop" %>

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
                        <div class="row">
                          <div class="col-6">
                            <div class="card">

                              <div class="card-header bg-whitelabel1">
                                <h3 class="card-title">TRANSFERÊNCAS SELLER</h3>
                              </div>
                              <div class="card-body">
                                <div class="form-group row col-12">
                                    <label class="col-sm-12 col-form-label">JSON</label>
                                    <div class="col-sm-12">
                                        <asp:TextBox runat="server" ID="txtTransferenciasSeller" TextMode="MultiLine" Rows="10" CssClass="form-control"></asp:TextBox>
                                    </div>
                                </div>
                              </div>
                              <div class="card-footer">
                                <asp:Button runat="server" ID="btnTransferenciasSeller" 
                                      CssClass="btn btn-whitelabel1" Text="Pesquisar" 
                                      onclick="btnTransferenciasSeller_Click" />

                              </div>
                            </div>
                          </div>

                          <div class="col-6">

                            <div class="card">

                              <div class="card-header bg-whitelabel1">
                                <h3 class="card-title">TRANSAÇÕES TRANSFERENCIA</h3>
                              </div>
                              <div class="card-body">
                                <div class="form-group row col-12">
                                    <label class="col-sm-12 col-form-label">JSON</label>
                                    <div class="col-sm-12">
                                        <asp:TextBox runat="server" ID="txtID" CssClass="form-control"></asp:TextBox>
                                    </div>

                                    <div class="col-sm-12">
                                        <asp:TextBox runat="server" ID="txtTransacoes" TextMode="MultiLine" Rows="10" CssClass="form-control"></asp:TextBox>
                                    </div>
                                </div>
                              </div>
                              <div class="card-footer">
                                <asp:Button runat="server" ID="btnTransacoes" 
                                      CssClass="btn btn-whitelabel1" Text="Pesquisar" onclick="btnTransacoes_Click" 
                                     />

                              </div>
                            </div>
                          </div>

                          <div class="col-6">

                            <div class="card">

                              <div class="card-header bg-whitelabel1">
                                <h3 class="card-title">TRANSFERENCIA DETALHES</h3>
                              </div>
                              <div class="card-body">
                                <div class="form-group row col-12">
                                    <label class="col-sm-12 col-form-label">JSON</label>
                                    <div class="col-sm-12">
                                        <asp:TextBox runat="server" ID="txtTransferID" CssClass="form-control"></asp:TextBox>
                                    </div>

                                    <div class="col-sm-12">
                                        <asp:TextBox runat="server" ID="txtTransferenciaDetalhe" TextMode="MultiLine" Rows="10" CssClass="form-control"></asp:TextBox>
                                    </div>
                                </div>
                              </div>
                              <div class="card-footer">
                                <asp:Button runat="server" ID="btnTransferenciaDetalhe" 
                                      CssClass="btn btn-whitelabel1" Text="Pesquisar" onclick="btnTransferenciaDetalhe_Click" 
                                     />

                              </div>
                            </div>
                          </div>

                          <div class="col-6">
                          
                            <div class="card">

                              <div class="card-header bg-whitelabel1">
                                <h3 class="card-title">AJUSTES</h3>
                              </div>
                              <div class="card-body">
                                <div class="form-group row col-12">
                                    <label class="col-sm-12 col-form-label">JSON</label>
                                    <div class="col-sm-12">
                                        <asp:TextBox runat="server" ID="txtMarketplace" CssClass="form-control"></asp:TextBox>
                                    </div>

                                    <div class="col-sm-12">
                                        <asp:TextBox runat="server" ID="txtAjusteDetalhe" TextMode="MultiLine" Rows="10" CssClass="form-control"></asp:TextBox>
                                    </div>
                                </div>
                              </div>
                              <div class="card-footer">
                                <asp:Button runat="server" ID="btnAjuste" 
                                      CssClass="btn btn-whitelabel1" Text="Pesquisar" onclick="btnAjuste_Click" 
                                     />

                              </div>
                            </div>
                          </div>


                          <div class="col-6">
                          
                            <div class="card">

                              <div class="card-header bg-whitelabel1">
                                <h3 class="card-title">CLEARSALE</h3>
                              </div>
                              <div class="card-body">
                                <div class="form-group row col-12">
                                    <label class="col-sm-12 col-form-label">JSON</label>
                                    <div class="col-sm-12">
                                        <asp:TextBox runat="server" ID="txtJsonClearsale" CssClass="form-control"></asp:TextBox>
                                    </div>

                                    <div class="col-sm-12">
                                        <asp:TextBox runat="server" ID="txtClearsale" TextMode="MultiLine" Rows="10" CssClass="form-control"></asp:TextBox>
                                    </div>
                                </div>
                              </div>
                              <div class="card-footer">
                                <asp:Button runat="server" ID="btnClearsale" 
                                      CssClass="btn btn-whitelabel1" Text="Pesquisar" onclick="btnClearsale_Click" 
                                     />

                              </div>
                            </div>
                          </div>

                          <div class="col-6">
                          
                            <div class="card">

                              <div class="card-header bg-whitelabel1">
                                <h3 class="card-title">AUTENTIQUE</h3>
                              </div>
                              <div class="card-body">
                                <div class="form-group row col-12">
                                    <label class="col-sm-12 col-form-label">JSON</label>
                                    <div class="col-sm-12">
                                        <asp:TextBox runat="server" ID="txtJsonAutentique" CssClass="form-control"></asp:TextBox>
                                    </div>

                                    <div class="col-sm-12">
                                        <asp:TextBox runat="server" ID="txtAutentique" TextMode="MultiLine" Rows="10" CssClass="form-control"></asp:TextBox>
                                    </div>
                                </div>
                              </div>
                              <div class="card-footer">
                                <asp:Button runat="server" ID="btnAutentique" 
                                      CssClass="btn btn-whitelabel1" Text="Pesquisar" onclick="btnAutentique_Click" 
                                     />

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

