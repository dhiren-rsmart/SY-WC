<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PackingList.aspx.cs" Inherits="smART.MVC.Present.Reports.PackingList" %>

<%@ Register assembly="Microsoft.ReportViewer.WebForms, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a" namespace="Microsoft.Reporting.WebForms" tagprefix="rsweb" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
 <%--   <style type="text/css">
        #form1
        {
            height: 411px;
        }
    </style>--%>
</head>
<body>
    <form id="form1" runat="server">
    <div style = "height:11in; width:8.5in;" > 
    
        <asp:SqlDataSource ID="SqlDataSource1" runat="server" 
            ConnectionString="<%$ ConnectionStrings:smARTDBContext %>" 
            SelectCommand="Select * from V_COMPANY"></asp:SqlDataSource>

        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
        <asp:SqlDataSource ID="conRCM" runat="server" 
            ConnectionString="<%$ ConnectionStrings:smARTDBContext %>" 
            SelectCommandType="StoredProcedure" SelectCommand="SPRP_PackingList">
            <SelectParameters>
                <asp:Parameter Name="BookingRefNo" Type="String" />
            </SelectParameters>
        </asp:SqlDataSource>
    <rsweb:ReportViewer ID="rptViewer" runat="server" Font-Names="Verdana" 
            Font-Size="8pt" InteractiveDeviceInfos="(Collection)" 
            WaitMessageFont-Names="Verdana" WaitMessageFont-Size="14pt" Width="100%" 
            Height="100%">
    
    <LocalReport ReportPath="/Content/Reports/PackingList.rdl"  >
    <DataSources >
    <rsweb:ReportDataSource DataSourceId="conRCM"  Name="DataSet1"/>
    </DataSources>
    </LocalReport>
    
    </rsweb:ReportViewer>
    </div>
    </form>
</body>
</html>
