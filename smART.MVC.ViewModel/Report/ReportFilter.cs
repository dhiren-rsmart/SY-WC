using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Text.RegularExpressions;

namespace smART.ViewModel
{

    public class ReportFilter : BaseEntity
    {

        [DisplayName("Report Name")]
        public string ReportName
        {
            get;
            set;
        }

        [DisplayName("From Date")]
        public DateTime? FromDate
        {
            get;
            set;
        }

        [DisplayName("To Date")]
        public DateTime? ToDate
        {
            get;
            set;
        }

        [DisplayName("Party")]
        public int PartyID
        {
            get;
            set;
        }

        [DisplayName("Item")]
        public int ItemID
        {
            get;
            set;
        }

        [DisplayName("PO#")]
        public int POID
        {
            get;
            set;
        }

        [DisplayName("SO#")]
        public int SOID
        {
            get;
            set;
        }


        [DisplayName("Booking#")]
        public string BookingID
        {
            get;
            set;
        }

        [DisplayName("Container#")]
        public string ContainerID
        {
            get;
            set;
        }

        [DisplayName("UOM")]
        [UIHint("LOVDropDownList")]
        public string UOM
        {
            get;
            set;
        }

        [DisplayName("Status")]
        [UIHint("LOVDropDownList")]
        public string Status
        {
            get;
            set;
        }

        [DisplayName("Destination")]
        [UIHint("LOVDropDownList")]
        public string Destination
        {
            get;
            set;
        }

        [DisplayName("Purchase Order")]
        public PurchaseOrder PurchaseOrder
        {
            get;
            set;
        }

        [DisplayName("Sales Order")]
        public SalesOrder SalesOrder
        {
            get;
            set;
        }

        [DisplayName("License ID")]
        public string LicenseID
        {
            get;
            set;
        }

        [DisplayName("Ticket#")]
        public string TicketNo
        {
            get;
            set;
        }

        [DisplayName("FocusAreaID")]
        public int FocusAreaID
        {
            get;
            set;
        }

        [DisplayName("Store Procedure Name")]
        public String SP_Name
        {
            get;
            set;
        }

        [DisplayName("Report Name")]
        public string DataSet_Name
        {
            get;
            set;
        }


        [DisplayName("Parameters")]
        public string Parameters
        {
            get;
            set;
        }

        [DisplayName("Parameters")]
        public string SubReportInfo
        {
            get;
            set;
        }

        public Dictionary<string, string> SubReportInfoDict
        {
            get
            {
                string[] arr = SubReportInfo.Split(';');
                Dictionary<string, string> dic = new Dictionary<string, string>();
                for (int i = 0; i < arr.Length; i++)
                {
                    string[] arrItem = arr[i].Split(':');
                    dic.Add(arrItem[0], arrItem[1]);
                }
                return dic;
            }
        }

        public ReportFilter()
        {
            //FromDate = DateTime.Now;
            //ToDate = DateTime.Now;   
            //SalesOrder = new SalesOrder();
            //PurchaseOrder = new PurchaseOrder();
        }

        public static object GetPropValue(object src, string propName)
        {
            object value = src.GetType().GetProperty(propName).GetValue(src, null);
            if (propName == "FromDate" && value == null)
                value = System.DateTime.Parse("01/01/1900");// DateTime.MinValue;
            if (propName == "ToDate" && value == null)
                value = DateTime.Now;// DateTime.MaxValue;
            return value;
        }
    }
}
