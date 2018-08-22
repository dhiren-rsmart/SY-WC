using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Microsoft.Reporting.WebForms;
using smART.ViewModel;

namespace smART.MVC.Present.Reports
{

    public partial class Report : System.Web.UI.Page
    {

        private ReportFilter _reportFilter;

        protected void Page_Load(object sender, EventArgs e)
        {

            if (!IsPostBack)
            {
                if (Session["ReportFilter"] != null)
                {
                    _reportFilter = Session["ReportFilter"] as ReportFilter;
                    if (_reportFilter != null && !string.IsNullOrEmpty(_reportFilter.ReportName))
                    {
                        InitializeReportViewerProperties();
                        GenerateReport();
                        smARTRptViewer.LocalReport.Refresh();
                    }
                }
            }
        }

        /// <summary>
        /// This is the mainfunction which used to load the report to the report viewer.
        /// </summary>
        /// <param name="param">Contains perameter key value collectionfor perameterized report. It's default value is null.</param>
        public void GenerateReport()
        {
            smARTRptViewer.LocalReport.ReportPath = MapPath("/Content/Reports/GeneralReports/" + _reportFilter.ReportName);

            conRCM.SelectCommand = _reportFilter.SP_Name;

            List<String> strParameters = new List<String>(_reportFilter.Parameters.Split(','));

            //conRCM.SelectParameters.Add("FromDate", _reportFilter.FromDate.ToString());
            //conRCM.SelectParameters.Add("ToDate", _reportFilter.ToDate.ToString());
            //conRCM.SelectParameters.Add("PartyID", _reportFilter.PartyID.ToString());

            ReportDataSource reportDataSource = new ReportDataSource(_reportFilter.DataSet_Name, conRCM);
            smARTRptViewer.LocalReport.DataSources.Clear();
            smARTRptViewer.LocalReport.EnableExternalImages = true;

            if (strParameters.Count > 0)
            {
                List<ReportParameter> parameters = new List<ReportParameter>();

                foreach (string param in strParameters)
                {
                    string paramValue = Convert.ToString(ReportFilter.GetPropValue(_reportFilter, param)).Trim();
                    if (string.IsNullOrEmpty(paramValue))
                    {
                        paramValue = " ";
                    }
                    conRCM.SelectParameters.Add(param, paramValue);
                    ReportParameter rptParam = new ReportParameter(param, paramValue);
                    parameters.Add(rptParam);

                }
                smARTRptViewer.LocalReport.SetParameters(parameters);
            }

            if (!string.IsNullOrEmpty(_reportFilter.SubReportInfo))
                smARTRptViewer.LocalReport.SubreportProcessing += new SubreportProcessingEventHandler(LocalReport_SubreportProcessing);

            smARTRptViewer.LocalReport.DataSources.Add(reportDataSource);
            //ReportDataSource subReportDataSource = new ReportDataSource(_reportFilter.DataSet_Name, conRCM1);
            //conRCM1.SelectCommand = "TestSubReport";   
        }

        /// <summary>
        /// Initialize common properties of report viewer.
        /// This function is used to set the common property like width,height etc. of the report which is to be shown
        /// </summary>
        public void InitializeReportViewerProperties()
        {
            //ReportViewer.Width = new Unit(100, UnitType.Percentage);
            //ReportViewer.Height = new Unit(100, UnitType.Percentage);
            smARTRptViewer.AsyncRendering = false;
            smARTRptViewer.SizeToReportContent = true;
            smARTRptViewer.ShowZoomControl = false;
            smARTRptViewer.ShowBackButton = false;
            smARTRptViewer.Visible = true;
        }

        public void LocalReport_SubreportProcessing(object sender, SubreportProcessingEventArgs e)
        {
            //for (int i = 0; i < e.Parameters.Count; i++) {
            //  conRCM.SelectParameters[e.Parameters[i].Name] = new Parameter(e.Parameters[i].Name, GetDataTypeFromParameterType(e.Parameters[i].DataType), e.Parameters[i].Values[0]);
            //}
            //conRCM.SelectCommand =_reportFilter.SubReportInfoDict[e.ReportPath];
            //ReportDataSource reportDataSource = new ReportDataSource(_reportFilter.DataSet_Name, conRCM);
            //e.DataSources.Add(reportDataSource);

            //for (int i = 0; i < e.Parameters.Count; i++) {
            //  conRCM.SelectParameters[e.Parameters[i].Name] = new Parameter(e.Parameters[i].Name, GetDataTypeFromParameterType(e.Parameters[i].DataType), e.Parameters[i].Values[0]);
            //}

            SqlSubDataSource.SelectCommand = _reportFilter.SubReportInfoDict[e.ReportPath];
            for (int i = 0; i < e.Parameters.Count; i++)
            {
                SqlSubDataSource.SelectParameters[e.Parameters[i].Name] = new Parameter(e.Parameters[i].Name, GetDataTypeFromParameterType(e.Parameters[i].DataType), e.Parameters[i].Values[0]);
            }
            ReportDataSource datasourceSUB = new ReportDataSource(_reportFilter.DataSet_Name, SqlSubDataSource);
            e.DataSources.Add(datasourceSUB);
        }

        private System.Data.DbType GetDataTypeFromParameterType(ParameterDataType type)
        {
            switch (type)
            {
                case ParameterDataType.Boolean:
                    return System.Data.DbType.Boolean;
                case ParameterDataType.DateTime:
                    return System.Data.DbType.DateTime;
                case ParameterDataType.Float:
                    return System.Data.DbType.Double;
                case ParameterDataType.Integer:
                    return System.Data.DbType.Int32;
                case ParameterDataType.String:
                    return System.Data.DbType.String;
                default:
                    return System.Data.DbType.String;
            }
        }
    }
}