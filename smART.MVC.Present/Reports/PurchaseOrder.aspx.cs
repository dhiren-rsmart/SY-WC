using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Microsoft.Reporting.WebForms;

namespace smART.MVC.Present.Reports
{
    public partial class PurchaseOrder : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

            // Report Name
            // sql connection for report data
            // Data set name
            // bind data set with report view
            // passing parameter
            // back to calling object

            if (!IsPostBack)
            {
                string rptName = "/Content/Reports/PurchaseOrder.rdl";
                string poNumber = Request.QueryString["ID"];

                if (string.IsNullOrEmpty(poNumber) == true)
                {
                    //TODO: to log error
                    return;
                }
                if (poNumber == "0")
                {
                    return;
                }
                rptViewer.Visible = true;
                rptViewer.LocalReport.ReportPath = MapPath(rptName);

                conRCM.SelectParameters.Clear();
                conRCM.SelectParameters.Add("PurchaseOrderID", poNumber);
                rptViewer.LocalReport.SubreportProcessing += new SubreportProcessingEventHandler(LocalReport_SubreportProcessing);
                ReportDataSource datasource = new ReportDataSource("dsPO", conRCM);

                rptViewer.LocalReport.DataSources.Clear();
                rptViewer.LocalReport.DataSources.Add(datasource);

                ReportParameter rptParam = new ReportParameter("PurchaseOrderID", poNumber);
                rptViewer.LocalReport.SetParameters(rptParam);
                rptViewer.ShowPrintButton = true;
                rptViewer.LocalReport.Refresh();
            }

        }

        void LocalReport_SubreportProcessing(object sender, SubreportProcessingEventArgs e)
        {
            ReportDataSource datasourceSUB = new ReportDataSource("dsPartyHeader", SqlDataSource1);
            e.DataSources.Add(datasourceSUB);
        }

    }
}