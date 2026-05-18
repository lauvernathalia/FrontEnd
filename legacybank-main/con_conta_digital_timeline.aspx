<%@ Page Language="C#" AutoEventWireup="true" CodeFile="con_conta_digital_timeline.aspx.cs" Inherits="con_conta_digital_timeline" %>


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

        <div class="row">
          <div class="col-md-12">

                <div class="card">
                    <div class="card-header bg-whitelabel1">
                        <h3 class="card-title">ESTABELECIMENTO TIMELINE CONTA DIGITAL</h3>
                    </div>
                    <div class="card-body">



                        <div class="timeline">

                          <asp:Repeater runat="server" id="rptTimeline" onitemdatabound="rptTimeline_ItemDataBound">
                              <ItemTemplate>


                                  <div class="time-label">
                                    <span class="bg-red"><%# DataBinder.Eval(Container.DataItem, "DTA_TIMELINE")%></span>
                                    <asp:TextBox runat="server" ID="txtdatatimeline" Visible="false" Text='<%# DataBinder.Eval(Container.DataItem, "DTA_TIMELINE")%>'></asp:TextBox>
                                  </div>

                                  <asp:Repeater runat="server" id="rptTimelineDetalhe">
                                      <ItemTemplate>

                                          <div>
                                            <i class="fas fa-clock bg-blue"></i>
                                            <div class="timeline-item">
                                              <span class="time"><i class="fas fa-clock"></i> <%# String.Format("{0:HH:mm:ss}", DataBinder.Eval(Container.DataItem, "DTA_TIMELINE"))%></span>
                                              <h3 class="timeline-header"><%# DataBinder.Eval(Container.DataItem, "NOM_STATUS")%></h3>
                                            </div>
                                          </div>


                                      </ItemTemplate>
                                  </asp:Repeater>


                              </ItemTemplate>
                          </asp:Repeater>




                        </div>
                    </div>
                    <div class="card-footer">
                        <asp:TextBox runat="server" ID="txtStatus" Text="" Visible="false"></asp:TextBox>
                    </div>
                </div>
          </div>

        </div>
    </form>

<script type="text/javascript" src="../plugins/datatables/jquery.dataTables.js"></script>
<script type="text/javascript" src="../plugins/datatables-bs4/js/dataTables.bootstrap4.js"></script>

<script type="text/javascript" src="../plugins/inputmask/jquery.inputmask.bundle.js"></script>
<script type="text/javascript" src="../plugins/moment/moment.min.js"></script>

<script type="text/javascript">

$(document).ready(function () {
    var table = $('#tabConsulta').DataTable({
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


</body>
</html>
