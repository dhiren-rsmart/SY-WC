using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Web.Mvc;
using DataAnnotationsExtensions;

namespace smART.ViewModel
{
    public class Party: BaseEntity, IListType
    {
        //[Required]
        [DisplayName("Party Name")]
        [StringLength(45, ErrorMessage = "Maximum length is 45")]
        public string Party_Name { get; set; }

        [DisplayName("Phone")]
        [StringLength(45, ErrorMessage = "Maximum length is 45")]
        public string Party_Phone1 { get; set; }

        [Email]
        [DisplayName("Email")]
        [StringLength(100, ErrorMessage = "Maximum length is 100")]
        public string Party_Email { get; set; }

        //[Required]
        [DisplayName("Short Name")]
        [StringLength(45, ErrorMessage = "Maximum length is 45")]
        public string Party_Short_Name { get; set; }

        //[Required]
        [RegularExpression(@"^((?![Ss]elect).)*$", ErrorMessage="Please select a value for Party Type")]
        [DisplayName("Party Type")]
        [UIHint("LOVDropDownList")]
        [StringLength(45, ErrorMessage = "Maximum length is 45")]
        public string Party_Type { get; set; }

        [DisplayName("Currency")]
        [RegularExpression(@"^((?![Ss]elect).)*$", ErrorMessage = "Please select a value for Currency")]
        [UIHint("LOVDropDownList")]
        [StringLength(45, ErrorMessage = "Maximum length is 45")]
        public string Party_Currency { get; set; }

        [Numeric]
        [Min(0, ErrorMessage = "Please enter a positive number for Credit Limit")]
        [DisplayName("Credit Limit")]
        [DefaultValue(0)]
        public decimal? Party_Credit_Limit { get; set; }

        [Numeric]
        [Min(0, ErrorMessage="Please enter a positive number for Insurance Limit")]
        [DisplayName("Insurance Limit")]
        [DefaultValue(0)]
        public decimal? Party_Insurance_Limit { get; set; }

        [DisplayName("Fax")]
        [StringLength(45, ErrorMessage = "Maximum length is 45")]
        public string Party_Fax_No { get; set; }

        [Url]
        [DisplayName("Web Site")]
        [StringLength(128, ErrorMessage = "Maximum length is 128")]
        public string Party_Web_Site { get; set; }

        [DisplayName("Active")]
        public bool IsActive { get; set; }

        [DisplayName("Driving License#")]
        [StringLength(45, ErrorMessage = "Maximum length is 45")]
        public string License_No {
          get;
          set;
        }
              
        [DisplayName("State")]
        [UIHint("LOVDropDownList")]
        public string State {
          get;
          set;
        }

        [DisplayName("License Image")]
        [StringLength(255, ErrorMessage = "Maximum length is 255")]
        public string LicenseImageRefId {
          get;
          set;
        }

        [DisplayName("Party DOB")]
        [StringLength(45, ErrorMessage = "Maximum length is 45")]
        public string Party_DOB { get; set; }

        [DisplayName("Customer AC License ID")]
        [StringLength(45, ErrorMessage = "Maximum length is 45")]
        public string ACLicense_ID
        {
          get;
          set;
        }

        [DisplayName("Thumb Image1 Ref Id")]
        [StringLength(40, ErrorMessage = "Maximum length is 40")]
        public string ThumbImage1RefId
        {
            get;
            set;
        }

        [DisplayName("Thumb Image2 Ref Id")]
        [StringLength(40, ErrorMessage = "Maximum length is 40")]
        public string ThumbImage2RefId
        {
            get;
            set;
        }

        [DisplayName("Photo Ref Id")]
        [StringLength(40, ErrorMessage = "Maximum length is 40")]
        public string PhotoRefId
        {
            get;
            set;
        }


        [DisplayName("Signature Image Ref Id")]
        [StringLength(40, ErrorMessage = "Maximum length is 40")]
        public string SignatureImageRefId
        {
            get;
            set;
        }

        [DisplayName("Vehicle Image Ref Id")]
        [StringLength(40, ErrorMessage = "Maximum length is 40")]
        public string VehicleImageRegId
        {
            get;
            set;
        }

        [DisplayName("Cash Card Image Ref Id")]
        [StringLength(40, ErrorMessage = "Maximum length is 40")]
        public string CashCardImageRefId {
          get;
          set;
        }

        #region IListType Members

        [HiddenInput(DisplayValue = false)]
        public string ListText
        {
            get { return Party_Name; }
        }

        [HiddenInput(DisplayValue = false)]
        public string ListValue
        {
            get { return ID.ToString(); }
        }

        #endregion
    }
}
