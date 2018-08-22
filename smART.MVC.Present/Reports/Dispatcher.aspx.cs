using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Microsoft.Reporting.WebForms;

namespace smART.MVC.Present.Reports
{
    public partial class Dispatcher : System.Web.UI.Page
    {

        protected void Page_Load(object sender, EventArgs e)
        {

            if (!IsPostBack)
            {
                string rptName = "/Content/Reports/Dispatcher.rdl";
                string DispatcherID = Request.QueryString["ID"];

                if (string.IsNullOrEmpty(DispatcherID) == true)
                {
                    return;
                }
                if (DispatcherID == "0")
                {
                    return;
                }
                rptViewer.Visible = true;
                rptViewer.LocalReport.ReportPath = MapPath(rptName);

                conRCM.SelectParameters.Clear();
                conRCM.SelectParameters.Add("DispatcherID", DispatcherID);
                rptViewer.LocalReport.SubreportProcessing += new SubreportProcessingEventHandler(LocalReport_SubreportProcessing);
                ReportDataSource datasource = new ReportDataSource("dsDispatcher", conRCM);

                rptViewer.LocalReport.DataSources.Clear();
                rptViewer.LocalReport.DataSources.Add(datasource);

                ReportParameter rptParam = new ReportParameter("DispatcherID", DispatcherID);
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