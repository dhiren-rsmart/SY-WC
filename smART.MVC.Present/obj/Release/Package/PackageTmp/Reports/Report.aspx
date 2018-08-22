<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Report.aspx.cs" Inherits="smART.MVC.Present.Reports.Report" %>
<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=11.0.0.0, Culture=neutral, PublicKeyToken=89845dcd8080cc91"  Namespace="Microsoft.Reporting.WebForms" tagprefix="rsweb" %>
<%--<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">--%>
<html xmlns="http://www.w3.org/1999/xhtml">
    
<head runat="server">
  <title></title> 
</head>

<body>

  <form id="form1" runat="server">
 
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <asp:SqlDataSource ID="SqlSubDataSource" runat="server" 
            ConnectionString="<%$ ConnectionStrings:smARTDBContext %>" 
            SelectCommandType="StoredProcedure"> 
   </asp:SqlDataSource>

    <rsweb:ReportViewer ID="smARTRptViewer" runat="server" PageCountMode=Actual  Font-Names="Verdana" 
            Font-Size="8pt" InteractiveDeviceInfos="(Collection)" 
            WaitMessageFont-Names="Verdana" WaitMessageFont-Size="14pt" Width="100%" 
            Height="100%">
      <LocalReport>
       <DataSources >
            <rsweb:ReportDataSource DataSourceId="conRCM" Name="DataSet1"/>
           </DataSources>
      </LocalReport>
    </rsweb:ReportViewer>
    <asp:SqlDataSource ID="conRCM" runat="server" ConnectionString="<%$ ConnectionStrings:smARTDBContext %>" SelectCommandType="StoredProcedure">
    </asp:SqlDataSource>
  <%--    <asp:SqlDataSource ID="conRCM1" runat="server" ConnectionString="<%$ ConnectionStrings:smARTDBContext %>" SelectCommandType="StoredProcedure">
    </asp:SqlDataSource>--%>
    </form>
</body>

</html>
