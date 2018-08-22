﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Microsoft.Reporting.WebForms;

namespace smART.MVC.Present.Reports
{
    public partial class ScaleReceiveTicket : System.Web.UI.Page
    {

        protected void Page_Load(object sender, EventArgs e)
        {

            if (!IsPostBack)
            {
                string rptName = "/Content/Reports/ScaleReceiveTicket.rdl";
                string ScaleID = Request.QueryString["ID"];

                if (string.IsNullOrEmpty(ScaleID) == true)
                {
                    //TODO: to log error
                    return;
                }
                if (ScaleID == "0")
                {
                    return;
                }
                rptViewer.Visible = true;
                rptViewer.LocalReport.ReportPath = MapPath(rptName);

                conRCM.SelectParameters.Clear();
                conRCM.SelectParameters.Add("ScaleID", ScaleID);
                rptViewer.LocalReport.SubreportProcessing += new SubreportProcessingEventHandler(LocalReport_SubreportProcessing);
                ReportDataSource datasource = new ReportDataSource("dsScale", conRCM);

                rptViewer.LocalReport.DataSources.Clear();
                rptViewer.LocalReport.DataSources.Add(datasource);

                ReportParameter rptParam = new ReportParameter("ScaleID", ScaleID);
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