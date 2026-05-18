<%@ Page Language="C#" AutoEventWireup="true" CodeFile="con_documentos_assinaturas.aspx.cs" Inherits="con_documentos_assinaturas" MaintainScrollPositionOnPostback="true" Async="true" %>

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
                              <li class="breadcrumb-item active">Documentos</li>
                            </ol>
                          </div>
                        </div>

                        <div class="row">
                          <div class="col-12">

                            <div class="card">

                              <div class="card-header bg-whitelabel1">
                                <h3 class="card-title">Documentos</h3>
                              </div>
                                  <div class="card-body" style="overflow:auto; width: 100%;>
                                    <div class="row">
                                        <div class="col-12">

                                            <table id="tblConsulta" class="table table-striped dt-responsive nowrap">
                                                <thead>
                                                <tr>
                                                  <th><small>Data da Criação</small></th>
                                                  <th><small>Data da Assinatura</small></th>
                                                  <th><small>Nome documento</small></th>
                                                  <th><small>ID Documento</small></th>
                                                  <th><small>Documento Original</small></th>
                                                  <th><small>Documento Assinado</small></th>
                                                </tr>
                                                </thead>
                                                <tbody>
                                                <asp:Repeater runat="server" ID="rptConsulta">
                                                    <ItemTemplate>

                                                        <tr>
                                                          <td>
                                                            <small><%# String.Format("{0:dd/MM/yyyy HH:mm}",DataBinder.Eval(Container.DataItem, "DTA_DATA"))%></small>
                                                          </td>
                                                          <td>
                                                            <small><%# String.Format("{0:dd/MM/yyyy HH:mm}", DataBinder.Eval(Container.DataItem, "DTA_ASSINATURA"))%></small>
                                                          </td>
                                                          <td>
                                                            <small><%# DataBinder.Eval(Container.DataItem, "NOM_DOCUMENTO")%></small>
                                                          </td>

                                                          <td>
                                                            <small><%# DataBinder.Eval(Container.DataItem, "COD_ID_DOCUMENTO_ENVIADO")%></small>
                                                          </td>
                                                          <td>
                                                            <a href='<%# DataBinder.Eval(Container.DataItem, "NOM_DOCUMENTO_ENVIADO")%>' target="_blank" class="btn btn-whitelabel1 m-1">
                                                                Visualizar
                                                            </a>
                                                          </td>
                                                          <td>
                                                            <a href='<%# DataBinder.Eval(Container.DataItem, "NOM_DOCUMENTO_ASSINADO")%>' target="_blank" class="btn btn-whitelabel1 m-1">
                                                                Visualizar
                                                            </a>
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

                </section>
            </div>
            <PORTAL:PAGEBOTTOM id="Pageheader3" title="Site Directory" runat="server" ModuleSource="rodapepadrao.ascx"></PORTAL:PAGEBOTTOM><!-- Fim Rodapé da Pagina -->
    
        </div>
    </form>

<script src="../plugins/datatables/jquery.dataTables.min.js"></script>
<script src="../plugins/datatables-bs4/js/dataTables.bootstrap4.min.js"></script>
<script src="../plugins/datatables-responsive/js/dataTables.responsive.min.js"></script>
<script src="../plugins/datatables-responsive/js/responsive.bootstrap4.min.js"></script>
<script src="../plugins/datatables-buttons/js/dataTables.buttons.min.js"></script>
<script src="../plugins/datatables-buttons/js/buttons.bootstrap4.min.js"></script>
<script src="../plugins/jszip/jszip.min.js"></script>
<script src="../plugins/pdfmake/pdfmake.min.js"></script>
<script src="../plugins/pdfmake/vfs_fonts.js"></script>
<script src="../plugins/datatables-buttons/js/buttons.html5.min.js"></script>
<script src="../plugins/datatables-buttons/js/buttons.print.min.js"></script>
<script src="../plugins/datatables-buttons/js/buttons.colVis.min.js"></script>
<script src="../plugins/datatables-colreorder/js/colReorder.bootstrap4.js"></script>
<script src="../plugins/datatables-colreorder/js/dataTables.colReorder.js"></script>

</body>
</html>

