<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Print.aspx.cs" Inherits="smART.MVC.Present.Print" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <script type="text/javascript" src="Scripts/jquery-1.4.4.min.js"></script>
    <script type="text/javascript" src="Scripts/pdfobject.js"></script>
    <script type="text/javascript">
      function DoPrint()
      {
        pdfObject = document.getElementById("PDFObj");
        pdfObject.printAll();
      }

      function CloseMe()
      {
        window.close();
      }

      function OnLoad()
      {     
        DoPrint();
        window.setTimeout(CloseMe, 2000);
      }
  </script>
</head>
<body onload ="OnLoad();">
    <form id="form1" runat="server">
      <div id="divPDF" runat="server"></div>
    </form>
</body>
</html>
