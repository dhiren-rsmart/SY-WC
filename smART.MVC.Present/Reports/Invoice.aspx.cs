using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Microsoft.Reporting.WebForms;

namespace smART.MVC.Present.Reports
{

    public partial class Invoice : System.Web.UI.Page
    {

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                // Report Name
                // sql connection for report data
                // Data set name
                // bind data set with report view
                // passing parameter
                // back to calling object

                if (!IsPostBack)
                {
                    string rptName = "/Content/Reports/Invoice.rdl";
                    string soNumber = Request.QueryString["ID"];

                    if (string.IsNullOrEmpty(soNumber) == true)
                    {
                        //TODO: to log error
                        return;
                    }
                    if (soNumber == "0")
                    {
                        return;
                    }
                    rptViewer.Visible = true;
                    rptViewer.LocalReport.ReportPath = MapPath(rptName);

                    conRCM.SelectParameters.Clear();
                    conRCM.SelectParameters.Add("SalesInvoiceID", soNumber);
                    rptViewer.LocalReport.SubreportProcessing += new SubreportProcessingEventHandler(LocalReport_SubreportProcessing);
                    ReportDataSource datasource = new ReportDataSource("dsSalesInvoice", conRCM);

                    rptViewer.LocalReport.DataSources.Clear();
                    rptViewer.LocalReport.DataSources.Add(datasource);

                    ReportParameter rptParam = new ReportParameter("SalesInvoiceID", soNumber);
                    rptViewer.LocalReport.SetParameters(rptParam);
                    rptViewer.ShowPrintButton = true;
                    rptViewer.LocalReport.Refresh();
                }
            }
            catch (Exception)
            {
            }
        }

        void LocalReport_SubreportProcessing(object sender, SubreportProcessingEventArgs e)
        {
            ReportDataSource datasourceSUB = new ReportDataSource("dsPartyHeader", SqlDataSource1);
            e.DataSources.Add(datasourceSUB);
        }

    }
}