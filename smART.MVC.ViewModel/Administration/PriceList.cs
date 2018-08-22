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
    public class PriceList : BaseEntity, IListType
    {
        //[Required(AllowEmptyStrings = false)]
        [DisplayName("Name")]
        [StringLength(50, ErrorMessage = "Maximum length is 50")]
        public string PriceList_Name { get; set; }

        [DisplayName("Effective From")]
        public DateTime? Effective_Date_From { get; set; }

        [DisplayName("Effective To")]
        public DateTime? Effective_Date_To { get; set; }

        
        [UIHint("LOVDropDownList")]
        [StringLength(45, ErrorMessage = "Maximum length is 45")]
        public string UOM { get; set; }
        
        [StringLength(250, ErrorMessage = "Maximum length is 250")]
        public string Comments { get; set; }

         [DisplayName("Price List Active")]
        public bool Active { get; set; }
        
        string IListType.ListText
        {
            get { return PriceList_Name; }
        }

        string IListType.ListValue
        {
            get { return ID.ToString(); }
        }

        [DisplayName("Default")]
        public bool IsDefault {
          get;
          set;
        }

    }

    public abstract class PriceListChildEntity : BaseEntity, IListType
    {
        [HiddenInput(DisplayValue = false)]
        public PriceList PriceList { get; set; }

        string IListType.ListText
        {
            get { return ID.ToString(); }
        }

        string IListType.ListValue
        {
            get { return ID.ToString(); }
        }
    }

    public class PriceListItem : PriceListChildEntity
    {
        [ScaffoldColumn(false)]
        public Item Item { get; set; }

        [DataType("decimal(18 ,4")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:0.0000}")] 
        public decimal Price { get; set; }

        [StringLength(250, ErrorMessage = "Maximum length is 250")]
        public string Comments { get; set; }

      

        public PriceListItem()
        {
            //Item.AllowRequiredFields(Type.GetType("smART.ViewModel.Item"), false);
            //Item = new Item();
            
            
        }
    }
}
