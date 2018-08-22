using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using System.ComponentModel;

namespace smART.ViewModel {

  public class InventoryAudit : BaseEntity {

    [Display(Name = "Year")]
    [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy}")]
    public DateTime Year { get{return Date;}  }

    [Display(Name = "Month")]
    [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM}")]
    public DateTime Date { get; set; }

    [Display(Name = "Item")]
    [HiddenInput(DisplayValue = false)]
    public Item Item { get; set; }

    [Display(Name = "Opening Balance")]
    public decimal OpeningBal { get; set; }

    [Display(Name = "Received")]
    public decimal Received { get; set; }

    [Display(Name = "Export Sales")]
    public decimal ExportSales { get; set; }

    [Display(Name = "Local Sales")]
    public decimal LocalSales { get; set; }

    [Display(Name = "Contamination")]
    public decimal Contamination { get; set; }

    [Display(Name = "Closing Balance")]
    public decimal ClosingBal { get; set; }

  }
}
