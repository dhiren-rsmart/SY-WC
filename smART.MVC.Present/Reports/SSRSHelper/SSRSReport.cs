using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.Reporting.WebForms;
using System.IO;
using System.Web.UI.WebControls;
using smART.ViewModel;
using smART.Common;

namespace smART.MVC.Present {

  public class SSRSReport {

    private ReportConfig _reportConfig;

    public SSRSReport(string reportName, Dictionary<string, object> parameters) {
      _reportConfig = GetReportConfigByReportName(reportName, parameters);
    }

    public string ExportReportToPDF() {

      ReportViewer reportViewer1 = new ReportViewer();
      reportViewer1.LocalReport.SubreportProcessing += new SubreportProcessingEventHandler(LocalReport_SubreportProcessing);
      reportViewer1.ProcessingMode = ProcessingMode.Local;
     
      CreateReport(ref reportViewer1);

      Warning[] warnings;
      string[] streamids;
      string mimeType;
      string encoding;
      string filenameExtension;


      ReportParameterInfoCollection pInfo = reportViewer1.LocalReport.GetParameters();
      string filenameParams = _reportConfig.Title;

      byte[] bytes;
      if (reportViewer1.ProcessingMode == ProcessingMode.Local) {
        bytes = reportViewer1.LocalReport.Render("PDF", null, out mimeType,
         out encoding, out filenameExtension, out streamids, out warnings);
      }
      else {
        bytes = reportViewer1.ServerReport.Render("PDF", null, out mimeType,
         out encoding, out filenameExtension, out streamids, out warnings);
      }


      string filename = Path.Combine(ConfigurationHelper.GetsmARTTempDocPath(), filenameParams + ".pdf");

      if (System.IO.File.Exists(filename))
        System.IO.File.Delete(filename);

      using (FileStream fs = new FileStream(filename, FileMode.Create)) { fs.Write(bytes, 0, bytes.Length); }
      
      return filename;
    }

    public bool CreateReport(ref  Microsoft.Reporting.WebForms.ReportViewer ReportViewer) {

      ReportViewer.LocalReport.ReportPath = System.Configuration.ConfigurationManager.AppSettings["ReportPath"].ToString() + _reportConfig.ReportName;

      SqlDataSource sqlDS = new SqlDataSource();
      sqlDS.ConnectionString = ConfigurationHelper.GetsmARTDBContextConnectionString();
      sqlDS.SelectCommandType = SqlDataSourceCommandType.StoredProcedure;
      sqlDS.SelectCommand = _reportConfig.StoreProcedureName;
      foreach (var param in _reportConfig.Parameters) {
        sqlDS.SelectParameters.Add(param.Key, param.Value.ToString());
      }
      ReportDataSource datasource1 = new ReportDataSource(_reportConfig.ReportDataSetName, sqlDS);
      ReportViewer.LocalReport.DataSources.Clear();
      ReportViewer.LocalReport.DataSources.Add(datasource1);
            
      List<ReportParameter> rptParams = new List<ReportParameter>();
      foreach (var param in _reportConfig.Parameters) {
        ReportParameter rptParam = new ReportParameter(param.Key, param.Value.ToString());
        rptParams.Add(rptParam);
      }
      ReportViewer.LocalReport.SetParameters(rptParams);
      
      ReportViewer.LocalReport.Refresh();
      return true;
    }

    void LocalReport_SubreportProcessing(object sender, SubreportProcessingEventArgs e) {
      SqlDataSource sqlDS = new SqlDataSource();
      sqlDS.ConnectionString = ConfigurationHelper.GetsmARTDBContextConnectionString();
      sqlDS.SelectCommandType = SqlDataSourceCommandType.Text;
      sqlDS.SelectCommand = "SELECT * FROM [V_Company]";
      ReportDataSource datasourceSUB = new ReportDataSource("dsPartyHeader", sqlDS);
      e.DataSources.Add(datasourceSUB);
    }

    public ReportConfig GetReportConfigByReportName(string reportName, Dictionary<string, object> parameters) {
      ReportConfig rptConfig = new ReportConfig();
      if (reportName == "ScaleReceiveTicket.rdl") {
        rptConfig.ReportName = "ScaleReceiveTicket.rdl";
        rptConfig.StoreProcedureName = "SPRP_Scale_Receive";
        rptConfig.ReportDataSetName = "dsScale";
        rptConfig.SubReportDataSetName = "dsPartyHeader";
        rptConfig.Parameters = parameters;
        rptConfig.Title = parameters["ScaleID"].ToString();
      }
      else if (reportName =="QScale.rdl") {
        rptConfig.ReportName = "QScale.rdl";
        rptConfig.StoreProcedureName = "SPRP_QScale";
        rptConfig.ReportDataSetName = "DataSet1";
        rptConfig.SubReportDataSetName = "";
        rptConfig.Parameters = parameters;
        rptConfig.Title = parameters["ScaleID"].ToString();
      }
      return rptConfig;
    }
  }

}