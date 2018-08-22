using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Microsoft.Reporting.WebForms;

namespace smART.MVC.Present.Reports
{
    public partial class PackingList : System.Web.UI.Page
    {

        protected void Page_Load(object sender, EventArgs e)
        {

            if (!IsPostBack)
            {
                string rptName = "/Content/Reports/QScale.rdl";
                string ID = Request.QueryString["ID"];

                if (string.IsNullOrEmpty(ID) == true)
                {
                    return;
                }
                if (ID == "0")
                {
                    return;
                }
                rptViewer.Visible = true;
                rptViewer.LocalReport.ReportPath = MapPath(rptName);

                conRCM.SelectParameters.Clear();
                conRCM.SelectParameters.Add("ScaleID", ID);
                rptViewer.LocalReport.SubreportProcessing += new SubreportProcessingEventHandler(LocalReport_SubreportProcessing);
                ReportDataSource datasource = new ReportDataSource("DataSet1", conRCM);

                rptViewer.LocalReport.DataSources.Clear();
                rptViewer.LocalReport.DataSources.Add(datasource);

                ReportParameter rptParam = new ReportParameter("ID", ID);
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