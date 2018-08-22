// Copyright 2011, 2012 RecyclesmART, Inc. All rights reserved
// Main Author: Sanjeev Khanna
// Last Major Update: 12/09/2011

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace smART.Model {

  [Table("T_Inventory_Audit")]
  public class InventoryAudit : BaseEntity {

    public Item Item { get; set; }
    public DateTime Date { get; set; }
    public decimal OpeningBal { get; set; }
    public decimal Received { get; set; }
    public decimal ExportSales { get; set; }
    public decimal LocalSales { get; set; }
    public decimal Contamination { get; set; }
    public decimal ClosingBal { get; set; }

  }
}
