using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Microsoft.Reporting.WebForms;

namespace smART.MVC.Present.Reports
{
    public partial class PaymentExpense : System.Web.UI.Page
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
                    string rptName = "/Content/Reports/check_expense.rdl";
                    string id = Request.QueryString["ID"];

                    if (string.IsNullOrEmpty(id) == true)
                    {
                        //TODO: to log error
                        return;
                    }
                    if (id == "0")
                    {
                        return;
                    }
                    rptViewer.Visible = true;
                    rptViewer.LocalReport.ReportPath = MapPath(rptName);

                    conRCM.SelectParameters.Clear();
                    //'PaymentID
                    conRCM.SelectParameters.Add("PaymentID", id);
                    // rptViewer.LocalReport.SubreportProcessing += new SubreportProcessingEventHandler(LocalReport_SubreportProcessing);
                    ReportDataSource datasource = new ReportDataSource("DataSet1", conRCM);

                    rptViewer.LocalReport.DataSources.Clear();
                    rptViewer.LocalReport.DataSources.Add(datasource);

                    ReportParameter rptParam = new ReportParameter("PaymentID", id);
                    rptViewer.LocalReport.SetParameters(rptParam);
                    //rptViewer.ShowPrintButton = false;
                    rptViewer.LocalReport.Refresh();
                }
            }
            catch (Exception)
            {
            }

        }

        void LocalReport_SubreportProcessing(object sender, SubreportProcessingEventArgs e)
        {
            ReportDataSource datasourceSUB = new ReportDataSource("DataSet1", SqlDataSource1);
            e.DataSources.Add(datasourceSUB);
        }
    }
}