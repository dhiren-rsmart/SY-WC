using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using smART.Common;

namespace smART.Model
{
    [Table("M_Party"), Unique("Party_Name, Party_Type, Active_Ind")]
    public class Party : BaseEntity
    {
        [StringLength(45, ErrorMessage = "Maximum length is 45")]
        public string Party_Name { get; set; }
        [StringLength(45, ErrorMessage = "Maximum length is 45")]
        public string Party_Phone1 { get; set; }
        [StringLength(100, ErrorMessage = "Maximum length is 100")]
        public string Party_Email { get; set; }
        [StringLength(45, ErrorMessage = "Maximum length is 45")]
        public string Party_Short_Name { get; set; }
        [StringLength(45, ErrorMessage = "Maximum length is 45")]
        public string Party_Type { get; set; }

        [StringLength(45, ErrorMessage = "Maximum length is 45")]
        public string Party_Currency { get; set; }

        [DefaultValue(0)]
        public decimal? Party_Credit_Limit { get; set; }
        [DefaultValue(0)]
        public decimal? Party_Insurance_Limit { get; set; }

        [StringLength(45, ErrorMessage = "Maximum length is 45")]
        public string Party_Fax_No { get; set; }
        [StringLength(128, ErrorMessage = "Maximum length is 128")]
        public string Party_Web_Site { get; set; }
        public bool IsActive { get; set; }
        [StringLength(45, ErrorMessage = "Maximum length is 45")]
        public string License_No { get; set; }
        [StringLength(45, ErrorMessage = "Maximum length is 45")]
        public string State
        {
            get;
            set;
        }
        [StringLength(255, ErrorMessage = "Maximum length is 255")]
        public string LicenseImageRefId
        {
            get;
            set;
        }

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
    }
}
