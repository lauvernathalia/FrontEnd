<%@ Control Language="C#" AutoEventWireup="true" CodeFile="menupadrao.ascx.cs" Inherits="menupadrao" %>

  <nav class="main-header navbar fixed-top navbar-expand navbar-white navbar-light">
    
    <ul class="navbar-nav">

      <li class="nav-item">
        <a class="nav-link" data-widget="pushmenu" href="#"><i class="fas fa-bars"></i></a>
      </li>


      <li class="nav-item d-none d-sm-inline-block">
        <a href="#" class="nav-link"><asp:Label runat="server" ID="lblPerfil"></asp:Label></a>
      </li>

      <li class="nav-item d-none d-sm-inline-block">
        <a href="index.aspx" class="nav-link">Dashboard</a>
      </li>
      <li class="nav-item d-none d-sm-inline-block">
        <a href="exe_logoff.aspx" class="nav-link">Sair</a>
      </li>
      <li class="nav-item d-none d-sm-inline-block">
        <asp:Label runat="server" ID="lblDadosInternos" Text="" Visible="false"></asp:Label>
      </li>

    </ul>


    <ul class="navbar-nav ml-auto">
      <!-- Messages Dropdown Menu -->
      <!-- Menu topo direita com dados da conta -->

      <li class="nav-item" runat="server" id="liContaDigital" visible="true">
        <asp:LinkButton runat="server" ID="lkbContaDigital" class="nav-link btn btn-whitelabel1 btn-sm text-white" onclick="lkbContaDigital_Click" TabIndex="99">
          Clique aqui para criar a sua CONTA DIGITAL
        </asp:LinkButton>
      </li>

      <li class="nav-item dropdown">
        <a class="nav-link" data-toggle="dropdown" href="#">
          <small><asp:Label runat="server" id="lblNomeUsuario"></asp:Label></small>
          <i runat="server" id="icnDadosContaEstabelecimento" visible="true" class="fas fa-id-card ml-1"></i>
        </a>

        <div class="dropdown-menu dropdown-menu-lg dropdown-menu-right" runat="server" id="dvDadosContaEstabelecimento" visible="true">
          <center><img runat="server" id="imgUsuarioConta" class="rounded-circle text-center m-2 p-2" src="../images/user1-128x128.jpg" width="128"/></center>
          <span class="dropdown-item dropdown-header">   
              <small><asp:Label runat="server" id="lblNomeUsuarioConta"></asp:Label></small>
          </span>
          <center><asp:LinkButton ID="lkbAtualizarContaDigital" runat="server" onclick="lkbAtualizarContaDigital_Click" ><i class="fas fa-recycle text-info"></i></asp:LinkButton></center>
          <div class="dropdown-divider"></div>
          
          
          <a href="#" class="dropdown-item">
            <small class="text-wrap"><strong>Status:</strong> <asp:label runat="server" ID="lblStatus"></asp:label></small><br />
          </a>

          <div class="dropdown-divider"></div>
          <a href="#" class="dropdown-item">
            <small class="text-wrap"><strong>CNPJ/CPF:</strong> <asp:label runat="server" ID="lblCNPJCPF"></asp:label></small><br />
            <small class="text-wrap"><strong>E-mail:</strong> <asp:label runat="server" ID="lblEmail"></asp:label></small><br />
          </a>
          <div class="dropdown-divider"></div>
          <a href="#" class="dropdown-item">
            <small class="text-wrap"><strong>Banco:</strong> <asp:label runat="server" ID="lblBanco" Text="461 - Asaas Gestão Financeira Instituição de Pagamento S.A."></asp:label></small><br />
            <small class="text-wrap"><strong>Agência:</strong> <asp:label runat="server" ID="lblAgencia"></asp:label></small><br />
            <small class="text-wrap"><strong>Conta Corrente:</strong> <asp:label runat="server" ID="lblConta"></asp:label></small><br />
            <small class="text-wrap"><strong>Wallet ID:</strong> <asp:label runat="server" ID="lblWalletID"></asp:label></small><br />
          </a>
          <div class="dropdown-divider"></div>
          <a href="#" class="dropdown-item">
            <small class="text-wrap"><strong>Chave Pix:</strong> <asp:label runat="server" ID="lblChavePix"></asp:label></small><br />
            <center>
            <small class="text-center"><asp:LinkButton ID="lkbCopiar" runat="server" OnClientClick="javascript:return copyToClipboard();">Copiar chave pix</asp:LinkButton></small>
            </center>
          </a>
        </div>
      </li>
      <li class="nav-item text-center">

      </li>
      <li class="nav-item text-center">
        <a href="exe_logoff.aspx" class="nav-link"><i class="fas fa-power-off"></i></a>
      </li>

    </ul>


  </nav>

  <aside runat="server" id="asbMenu" class="main-sidebar sidebar-dark-primary elevation-4" style="background-image: linear-gradient(15deg, #002d40 0%, #00895c 100%);">

    <a href="index.aspx" class="brand-link">
      <asp:image runat="server" id="imgLicenciado" alt="LEGACYBANK" 
    class="img-fluid" CssClass="img-fluid" ImageUrl="../images/logo-dark-2.png"></asp:image>
    </a>

    <!-- Sidebar -->
    <div runat="server" id="dvMenu" class="sidebar sidebar-dark-legacy">
      <nav class="mt-2">
        <ul class="nav nav-pills nav-sidebar flex-column" data-widget="treeview" role="menu" data-accordion="false">

          <asp:Repeater runat="server" id="rptMenu" onitemdatabound="rptMenu_ItemDataBound">
              <ItemTemplate>

          
                  <li class="nav-item has-treeview">
                    <a href="#" class="nav-link text-white">
                      <i class="nav-icon fas <%# DataBinder.Eval(Container.DataItem, "NOM_ICONE")%>"></i>
                      <p>
                        <%# DataBinder.Eval(Container.DataItem, "NOM_MENU")%>
                        <asp:TextBox runat="server" ID="txtidmenu" Visible="false" Text='<%# DataBinder.Eval(Container.DataItem, "COD_ID")%>'></asp:TextBox>
                        <i class="right fas fa-angle-left"></i>
                      </p>
                    </a>
                    <ul class="nav nav-treeview">

                      <asp:Repeater runat="server" id="rptSubmenu">  
                          <ItemTemplate>

                              <li class="nav-item text-warning" style="background-color: transparent; color:#FFC107;">
                                <a href="<%# DataBinder.Eval(Container.DataItem, "NOM_ACAO")%>" class="nav-link text-warning">
                                  <i class="fas <%# DataBinder.Eval(Container.DataItem, "NOM_ICONE")%>"></i>
                                  <p><%# DataBinder.Eval(Container.DataItem, "NOM_SUBMENU")%></p>
                                </a>
                              </li>
                            </ItemTemplate>
                      </asp:Repeater>

                    </ul>


                  </li>
                </ItemTemplate>
          </asp:Repeater>
          <asp:Repeater runat="server" id="rptLogins">  
                <ItemTemplate>
                    <li class="nav-item">
                        <!--<a href='relogar.aspx?id=<%#Funcoes.Encrypt(DataBinder.Eval(Container.DataItem,"COD_ID").ToString())%>' class="nav-link text-white">-->
                            <i class="nav-icon fas fa-eye"></i>
                            <span class='right badge badge-<%# DataBinder.Eval(Container.DataItem, "FLG_TIPO_COR")%>'>Visão de <%# DataBinder.Eval(Container.DataItem, "FLG_TIPO")%></span>
                        <!--</a>-->
                    </li>
                </ItemTemplate>
            </asp:Repeater>
        </ul>
      </nav>
    </div>
  </aside>


    <div class="modal fade" id="mdContaDigital">
    <div class="modal-dialog modal-sm">
        <div class="modal-content">
        <div class="modal-header">
            <h6 class="modal-title"><b>Abertura de Conta</b></h6>
            <button type="button" class="close" data-dismiss="modal" aria-label="Close" tabindex="9999">
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
            <button type="button" class="btn btn-danger" data-dismiss="modal" tabindex="9999">Cancelar</button>
            <asp:Button runat="server" ID="btnConfirmarContaDigital" CssClass="btn btn-primary" Text="Confirmar" onclick="btnConfirmarContaDigital_Click" TabIndex="9999" />
        </div>
        </div>
    </div>
    </div>

<script type="text/javascript">
    function copyToClipboard() {
        var text = document.getElementById('<%= lblChavePix.ClientID %>');
        var chave = text.innerHTML
        window.prompt("Copie para área de transferência: Ctrl+C e tecle Enter", chave);
    }
</script>

<script src="dist/js/adminlte.js"></script>
