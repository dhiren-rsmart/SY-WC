// Copyright 2011, 2012 RecyclesmART, Inc. All rights reserved
// Main Author: Sanjeev Khanna
// Last Major Update: 12/09/2011

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace smART.Model
{
    [Table("T_Inventory")]
    public class Inventory:BaseEntity
    {
        public DateTime? Trans_Date { get; set; }

        public string Trans_Type { get; set; }

        public Party Party_ID { get; set; }

        public Item Item_ID { get; set; }

        public string Impact { get; set; }

        public decimal Quantity { get; set; }

        public decimal Balance { get; set; }

        public Scale Scale_Ticket_ID { get; set; }

    }
}
