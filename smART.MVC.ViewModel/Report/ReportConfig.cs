using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace smART.ViewModel {

  public class ReportConfig {

    public string Title { get; set; }

    public string ReportName { get; set; }

    public string StoreProcedureName { get; set; }

    public System.Collections.Generic.Dictionary<string, object> Parameters { get; set; }

    public string ReportDataSetName { get; set; }

    public string SubReportDataSetName { get; set; }

  }
}
