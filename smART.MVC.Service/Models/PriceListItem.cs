using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace smART.MVC.Service.Model
{
    public class PriceListItem : smART.MVC.Service.Model.BaseEntity
    {
        public decimal Price { get; set; }

        public Int32 Item_ID { get; set; }

        public Int32 PriceList_ID { get; set; }

        public bool Active_Ind { get; set; }
    }
}