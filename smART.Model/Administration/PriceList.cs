using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using smART.Common;

namespace smART.Model
{
    [Table("M_PriceList"),Unique("PriceList_Name, Active_Ind")]
    public class PriceList : BaseEntity
    {
        [StringLength(50, ErrorMessage = "Maximum length is 50")]
        public string PriceList_Name { get; set; }
        
        public DateTime? Effective_Date_From { get; set; }
        public DateTime? Effective_Date_To { get; set; }

        [StringLength(45, ErrorMessage = "Maximum length is 45")]
        public string UOM { get; set; }

        [StringLength(250, ErrorMessage = "Maximum length is 250")]
        public string Comments { get; set; }

        public bool Active { get; set; }
              
        public bool IsDefault {
          get;
          set;
        }
    }

    public abstract class PriceListChildEntity : BaseEntity
    {
        public PriceList PriceList { get; set; }
    }

    [Table("M_PriceList_Item"), Unique("Item_ID,PriceList_ID, Active_Ind")]
    public class PriceListItem : PriceListChildEntity
    {
        public Item Item { get; set; }

        [DataType("decimal(16 ,4")]        
        public decimal Price { get; set; }

        [StringLength(250, ErrorMessage = "Maximum length is 250")]
        public string Comments { get; set; }

    }
}
