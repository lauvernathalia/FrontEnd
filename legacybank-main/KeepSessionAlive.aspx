<%@ Page Language="C#" AutoEventWireup="true" CodeFile="KeepSessionAlive.aspx.cs" Inherits="KeepSessionAlive" EnableSessionState="False" %>
<head id="KeepSession" runat="server" />    
    <meta id="MetaRefresh" http-equiv="refresh" content="60; ,url=KeepSessionAlive.aspx" runat="server" />
    <script language="javascript">
        window.status = "<%=WindowStatusText%>";
    </script>
    <form id="form2" runat="server">
    <asp:Label ID="lblSessao" runat="server"></asp:Label>
    </form>
</head>
