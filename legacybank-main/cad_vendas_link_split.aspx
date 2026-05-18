<%@ Page Language="C#" AutoEventWireup="true" CodeFile="cad_vendas_link_split.aspx.cs" Inherits="cad_vendas_link_split" %>

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
            <div class="col-12">

                        <div class="card m-2">
                            <div class="card-body">
                                <div class="row">
                                    <div class="col-12">
                                        <h3 class="modal-title text-black text-center"><b class="text-whitelabel1">SPLIT</b> DA TRANSAÇÃO</h3>
                                        <asp:TextBox runat="server" ID="txtResposta" CssClass="form-control"></asp:TextBox>
                                    </div>
                                </div>


                                <div class="row">
                                    <div class="col-sm-12">
                                        <label class="col-sm-12 col-form-label">Parceiros</label>
                                        <div class="input-group">
                                            <div class="custom-file">
                                            <asp:DropDownList runat="server" id="ddlParceiros" CssClass="form-control" 
                                                    AutoPostBack="True" onselectedindexchanged="ddlParceiros_SelectedIndexChanged">
                                            </asp:DropDownList>
                                            </div>
                                            <div class="input-group-append">
                                            <asp:Button runat="server" ID="btnNovoParceiro" CssClass="btn btn-whitelabel1" Text="Novo Parceiro" onclick="btNovoParceiro_Click"/>
                                            </div>
                                        </div>
                                    </div>  
                                </div>
                                <div class="row">
                                    <div class="col-sm-2">
                                        <div class="form-group">
                                        <asp:Label runat="server" ID="Label20" Text="ID"></asp:Label>
                                        <asp:Label runat="server" id="txtIDParceiro" cssClass="form-control" ></asp:Label>
                                        </div>
                                    </div>   
                                    <div class="col-sm-4">
                                        <div class="form-group">
                                        <asp:Label runat="server" ID="Label21" Text="Parceiro"></asp:Label>
                                        <asp:Label runat="server" id="txtParceiro" cssClass="form-control" ></asp:Label>
                                        </div>
                                    </div>                                
                                    <div class="col-sm-2">
                                        <div class="form-group">
                                        <asp:Label runat="server" ID="Label23" Text="Valor"></asp:Label>
                                        <asp:TextBox runat="server" id="txtValorParceiro" cssClass="form-control" ></asp:TextBox>
                                        </div>
                                    </div>   


                                    <div class="col-sm-2">
                                        <div class="form-group">
                                        <asp:Label runat="server" ID="Label24" Text="Percentual(%)"></asp:Label>
                                        <asp:TextBox runat="server" id="txtPercentualParceiro" cssClass="form-control" ></asp:TextBox>
                                        </div>
                                    </div>   
                                    <div class="col-sm-2">
                                        <br />
                                        <asp:Button runat="server" ID="btnIncluirParceiro" CssClass="btn btn-sm btn-whitelabel1" 
                                            Text="Incluir Parceiro" onclick="btnIncluirParceiro_Click" />
                                    </div>                                    
                                </div>



                                <div class="row">
                                    <div class="col-12">

                                        <table id="Table2" class="table table-bordered table-hover">
                                            <thead>
                                            <tr>
                                                <th>ID</th>
                                                <th>Data</th>
                                                <th>Parceiro</th>
                                                <th>Valor</th>
                                                <th>Percentual</th>
                                                <th>Transação</th>
                                                <th>ID Split</th>
                                            </tr>
                                            </thead>
                                            <tbody>
                                            <asp:Repeater runat="server" ID="rptParceiros" 
                                                    onitemcommand="rptParceiros_ItemCommand">
                                                <ItemTemplate>
                                                    <tr>

                                                        <td>
                                                            <asp:TextBox runat="server" ID="txtid" Text='<%# DataBinder.Eval(Container.DataItem, "COD_ID")%>' Visible="false"></asp:TextBox>
                                                            <asp:TextBox runat="server" ID="txtidparceiro" Text='<%# DataBinder.Eval(Container.DataItem, "COD_ID_VENDAS_PARCEIROS")%>' Visible="false"></asp:TextBox>
                                                            <asp:TextBox runat="server" ID="txtidvenda" Text='<%# DataBinder.Eval(Container.DataItem, "COD_ID_VENDAS_VENDAS")%>' Visible="false"></asp:TextBox>
                                                            <asp:TextBox runat="server" ID="txttoken" Text='<%# DataBinder.Eval(Container.DataItem, "NOM_TOKEN_ZOOP_PARCEIRO")%>' Visible="false"></asp:TextBox>
                                                            <asp:TextBox runat="server" ID="txtvenda" Text='<%# DataBinder.Eval(Container.DataItem, "COD_ID_VENDAS")%>' Visible="false"></asp:TextBox>

                                                            <small><%# DataBinder.Eval(Container.DataItem, "COD_ID")%></small>
                                                        </td>
                                                        <td>
                                                            <small><%# String.Format("{0:dd/MM/yyyy}", DataBinder.Eval(Container.DataItem, "DTA_DATA"))%></small>
                                                        </td>

                                                        <td>
                                                            <small><%# DataBinder.Eval(Container.DataItem, "NOM_NOME")%></small>
                                                            <asp:TextBox runat="server" ID="txtnome" Text='<%# DataBinder.Eval(Container.DataItem, "NOM_NOME")%>' Visible="false"></asp:TextBox>

                                                        </td>
                                                        <td>
                                                            <small><%# String.Format("{0:n2}", DataBinder.Eval(Container.DataItem, "NUM_VALOR"))%></small>
                                                            <asp:TextBox runat="server" ID="txtValor" CssClass="form-control" Text='<%# DataBinder.Eval(Container.DataItem, "NUM_VALOR")%>' Visible="false"></asp:TextBox>
                                                        </td>
                                                        <td>
                                                            <small><%# String.Format("{0:n2}",DataBinder.Eval(Container.DataItem, "NUM_PERCENTUAL"))%></small>
                                                            <asp:TextBox runat="server" ID="txtPercentual" CssClass="form-control" Text='<%# DataBinder.Eval(Container.DataItem, "NUM_PERCENTUAL")%>' Visible="false"></asp:TextBox>
                                                        </td>
                                                        <td>
                                                            <small><%# DataBinder.Eval(Container.DataItem, "NOM_CODE")%></small>
                                                            <asp:TextBox runat="server" ID="txtidtransacao" Text='<%# DataBinder.Eval(Container.DataItem, "NOM_CODE")%>' Visible="false"></asp:TextBox>
                                                        </td>

                                                        <td>
                                                            <small><%# DataBinder.Eval(Container.DataItem, "NOM_ID")%></small>
                                                            <asp:TextBox runat="server" ID="txtidsplit" Text='<%# DataBinder.Eval(Container.DataItem, "NOM_ID")%>' Visible="false"></asp:TextBox>
                                                        </td>
                                                        <td>
                                                            <asp:linkbutton ID="lkbExcluir" CssClass="" commandname="Excluir" runat="server" text="Excluir" ToolTip="Excluir" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "COD_ID_VENDAS_PARCEIROS")%>' ><i class="fas fa-trash text-danger"></i></asp:linkbutton>                                                        
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
                                <asp:Button runat="server" ID="btnConfirmar" CssClass="btn btn-whitelabel1 float-left" Text="Gerar Split" onclick="btnConfirmar_Click"/>
                                <asp:Button runat="server" ID="btnFechar" CssClass="btn btn-whitelabel1 float-right" Text="Fechar" onclick="btnFechar_Click"/>
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

    function printDiv(divName) {
        var printContents = document.getElementById(divName).innerHTML;
        var originalContents = document.body.innerHTML;

        document.body.innerHTML = printContents;

        window.print();

        document.body.innerHTML = originalContents;
    }

</script>

</body>
</html>
