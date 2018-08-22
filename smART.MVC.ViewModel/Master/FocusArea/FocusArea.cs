using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace smART.ViewModel
{

    public class FocusArea : BaseEntity
    {

        [StringLength(45, ErrorMessage = "Maximum length is 45")]
        [DisplayName("Focus Area")]
        public string Focus_Area { get; set; }

        [StringLength(45, ErrorMessage = "Maximum length is 45")]
        [DisplayName("Sub Focus Area")]
        public string Sub_Focus_Area { get; set; }

        [StringLength(45, ErrorMessage = "Maximum length is 45")]
        [DisplayName("View Name")]
        public string View_Name { get; set; }

        [StringLength(45, ErrorMessage = "Maximum length is 45")]
        [DisplayName("Order By")]
        public string OrderBy_Clause { get; set; }

        [StringLength(10, ErrorMessage = "Maximum length is 10")]
        [DisplayName("Focus Area Type")]
        public string Focus_Area_Type
        {
            get;
            set;
        }

        [StringLength(45, ErrorMessage = "Maximum length is 45")]
        [DisplayName("Report Name")]
        public string Report_Name
        {
            get;
            set;
        }

        [StringLength(30, ErrorMessage = "Maximum length is 30")]
        [DisplayName("Report Name")]
        public string DataSet_Name
        {
            get;
            set;
        }

        [StringLength(300, ErrorMessage = "Maximum length is 300")]
        [DisplayName("Parameters")]
        public string Parameters
        {
            get;
            set;
        }

        [StringLength(500, ErrorMessage = "Maximum length is 500")]
        [DisplayName("ReportInfo")]
        public string SubReportInfo
        {
            get;
            set;
        }

        public FocusArea()
        {
        }
    }
}
