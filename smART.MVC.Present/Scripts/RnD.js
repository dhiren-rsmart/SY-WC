// Export Ticket to PDF And Print.
function ExportAndPrintTicket(scaleId)
{
  var postData = { id: scaleId.toString() };
  $.ajax({
    url: '@(Url.Action("_ExportTicketToPDF", "QScale"))',
    type: "GET",
    dataType: "json",
    data: postData,
    cache: false,
    success: function (data)
    {
      if (data != null)
      {

        window.location.href = "../../../Reports/QScale.aspx";

        //        PDDocument doc1 = PDDocument.load("D:\RecyclesmART_Cloud\V1.1\smART.MVC.Present\Content\smARTDocPath\Temp\Attachments\59.pdf");
        //        doc1.silentPrint();

        //  openPrintTicketWindow();
        //var url = "../../../Content/smARTDocPath/Temp/Attachments/" + scaleId + ".pdf"
        //          var strWindowFeatures = "location=yes,height=570,width=520,scrollbars=yes,status=yes";
        //          var url = "/QScale/QScale/Content/smARTDocPath/Temp/Attachments/" + scaleId + ".pdf"
        //          var pdf = window.open(url, "_blank", strWindowFeatures);
        //          pdf.print();

        //          var iframe = document.getElementById('QScalePDF');
        //          $('#QScalePDF').attr('src', url);
        //          iframe.src = iframe.src;
        //          iframe.focus();

        //          var pdf = new PdfUtil(url);
        //          pdf.display(document.getElementById('QScalePDF'));
        //          pdf.print();
        //alert('print');

        //          window.frames["QScalePDF"].focus();
        //          window.frames["QScalePDF"].print();

        //          iframe.contentWindow.print();
        //          $('#QScalePDF').attr('src', '');
      }
    },
    error: function ()
    {
      alert("Error occured to print ticket.");
    }
  });
}

function PdfUtil(url)
{

  var iframe;

  var __construct = function (url)
  {
    iframe = getContentIframe(url);
  }

  var getContentIframe = function (url)
  {
    var iframe = document.createElement('iframe');
    iframe.src = url;
    return iframe;
  }

  this.display = function (parentDomElement)
  {
    parentDomElement.appendChild(iframe);
  }

  this.print = function ()
  {
    try
    {
      iframe.contentWindow.print();
    } catch (e)
    {
      alert("Printing failed.");
    }
  }

  __construct(url);
}

function openPrintTicketWindow(id)
{
  var controller = 'Reports';
  var action = 'QScale?ID=' + id;
  var params = '';
  openWindow("#PrintWindow", controller, action, params);
}


function closePrintWindow()
{
  var window = $("#PrintWindow").data("tWindow");
  window.close();
}

//<div id='document'>
//  <iframe id="QScalePDF" style="display: none;" height="0" width="0"></iframe>
//  @*<embed style="padding-top: 0px; padding-bottom: 0px;" src="../../../Content/smARTDocPath/Temp/QScale.pdf" id="QScaleDoc" hidden="hidden"
//    width="500" height="100">
//*@
//</div>