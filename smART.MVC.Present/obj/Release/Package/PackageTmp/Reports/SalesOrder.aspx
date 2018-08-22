<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SalesOrder.aspx.cs" Inherits="smART.MVC.Present.Reports.SalesOrder" %>

<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a"
    Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <script type ="text/ecmascript" src="../../Scripts/jquery-1.4.4.js"></script>
</head>
<body>
    <form id="form1" runat="server">
    <div style = "height:11in; width:8.5in;" > 

        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
        <asp:SqlDataSource ID="SqlDataSource1" runat="server" 
            ConnectionString="<%$ ConnectionStrings:smARTDBContext %>" 
            SelectCommand="SELECT * FROM [V_Company]"></asp:SqlDataSource>
    <rsweb:ReportViewer ID="rptViewer" runat="server" Font-Names="Verdana" 
            Font-Size="8pt" InteractiveDeviceInfos="(Collection)" 
            WaitMessageFont-Names="Verdana" WaitMessageFont-Size="14pt" Width="100%" 
            Height="100%">
    
    <LocalReport ReportPath="/Content/Reports/SalesOrder.rdl"  >
    <DataSources >
    <rsweb:ReportDataSource DataSourceId="conRCM"  Name="dsSO"/>
    </DataSources>
    </LocalReport>
    
    </rsweb:ReportViewer>
        <asp:SqlDataSource ID="conRCM" runat="server" 
            ConnectionString="<%$ ConnectionStrings:smARTDBContext %>" SelectCommand="SPRP_SalesOrder" 
            SelectCommandType="StoredProcedure">
            <SelectParameters>
                <asp:Parameter DefaultValue="1" Name="SalesOrderID" 
                    Type="String" />
            </SelectParameters>
        </asp:SqlDataSource>
    </div>
   
    </form>
</body>
</html>