using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.IO;
using smART.Common;

namespace smART.MVC.Present {

  public partial class Print : System.Web.UI.Page {

    protected void Page_Load(object sender, EventArgs e) {
      string fileName = Request.QueryString["ID"].ToString();
      //Span Name
      HtmlGenericControl object1 = new HtmlGenericControl("OBJECT");
      object1.ID = "PDFObj";
      object1.Attributes["DATA"] = string.Concat(ConfigurationHelper.GetsmARTDocUrl(), "Temp", "/", fileName + ".pdf"); //"Content/smARTDocPath/Temp/Attachments/" + fileName + ".pdf"; // "TestPDF.pdf";
      object1.Attributes["TYPE"] = "application/pdf";
      object1.Attributes["WIDTH"] = "100%";
      object1.Attributes["HEIGHT"] = "100%";
      divPDF.Controls.Add(object1);
    }
  }
}