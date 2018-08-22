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
    public class Item : BaseEntity, IListType
    {
        //[Required]
        [DisplayName("Category")]
        [UIHint("LOVDropDownList")]
        [StringLength(45, ErrorMessage = "Maximum length is 45")]
        public string Item_Category { get; set; }

        //[Required(AllowEmptyStrings =false )]
        [DisplayName("Group")]
        [UIHint("LOVDropDownList")]
        [StringLength(45, ErrorMessage = "Maximum length is 45")]
        public string Item_Group { get; set; }

        [DisplayName("Item Short Name")]
        [StringLength(25, ErrorMessage = "Maximum length is 25")]
        public string Short_Name { get; set; }

        [DisplayName("Item Long Name")]
        [StringLength(100, ErrorMessage = "Maximum lenght is 100")]
        public string Long_Name { get; set; }

        [DefaultValue(false)]
        [DisplayName("Priced")]
        public bool Priced { get; set; }

        [DefaultValue(false)]
        [DisplayName("Regulated Item")]
        public bool Regulated_Item { get; set; }

        [DefaultValue(false)]
        [DisplayName("Require VIN")]
        public bool Require_VIN { get; set; }

        [DisplayName("Opening Balance")]
        public Decimal Opening_Balance { get; set; }

        [DisplayName("Current Balance")]
        public Decimal Current_Balance { get; set; }

        [DisplayName("Net Balance")]
        public Decimal Net_Balance { get { return Opening_Balance + Current_Balance; } }

        #region IListType Members

        [HiddenInput(DisplayValue = false)]
        public string ListText
        {
            get { return Short_Name; }
        }

        [HiddenInput(DisplayValue = false)]
        public string ListValue
        {
            get { return ID.ToString(); }
        }

        [DisplayName("Active")]
        public bool IsActive {
          get;
          set;
        }

        #endregion


    }
}
