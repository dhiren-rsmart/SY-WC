using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Web.Mvc;

namespace smART.ViewModel {
  public class Container : BaseEntity, IListType {
    [Required]
    [DisplayName("Container Number")]
    [StringLength(50, ErrorMessage = "Maximum legth is 50")]
    public string Container_No { get; set; }

    [DisplayName("Container Type/Size")]
    [UIHint("LOVDropDownList")]
    public string Container_Size { get; set; }

    [DisplayName("Chasis Number")]
    [StringLength(50, ErrorMessage = "Maximum legth is 50")]
    public string Chasis_No { get; set; }

    [DisplayName("Seal #1")]
    [StringLength(50, ErrorMessage = "Maximum legth is 50")]
    public string Seal1_No { get; set; }

    [DisplayName("Seal #2")]
    [StringLength(50, ErrorMessage = "Maximum legth is 50")]
    public string Seal2_No { get; set; }

    [DisplayName("Origin")]
    [UIHint("LOVDropDownList")]
    [HiddenInput(DisplayValue = false)]
    public string Origin { get; set; }

    [DisplayName("Destination")]
    [UIHint("LOVDropDownList")]
    [HiddenInput(DisplayValue = false)]
    public string Destination { get; set; }

    //Display only based on rulles
    [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM/dd/yyyy}")]
    [DisplayName("Container Return Date")]
    public DateTime? Container_Return_Date { get; set; }

    [DisplayName("Booking#")]
    [HiddenInput(DisplayValue = false)]
    public Booking Booking { get; set; }

    [DisplayName("Date In")]
    [HiddenInput(DisplayValue = false)]
    public DateTime Date_In { get; set; }

    [DisplayName("Date Out")]
    [HiddenInput(DisplayValue = false)]
    public DateTime? Date_Out { get; set; }

    [DisplayName("Gross Weight")]
    [HiddenInput(DisplayValue = false)]
    public decimal Gross_Weight { get; set; }

    [DisplayName("Tare Weight")]
    [HiddenInput(DisplayValue = false)]
    public decimal Tare_Weight { get; set; }

    [DisplayName("Net Weight")]
    public decimal Net_Weight { get; set; }

    [DisplayName("Item ID")]
    [HiddenInput(DisplayValue = false)]
    public int Item_ID { get; set; }

    //Display only based on rulles
    [UIHint("LOVDropDownList")]
    [DisplayName("Container Status")]
    public string Status { get; set; }

    [DisplayName("Transfer To Booking#")]
    [HiddenInput(DisplayValue = false)]
    public Booking TransferBooking { get; set; }

    [DisplayName("Send Mail")]
    public bool Send_Mail { get; set; }

    [DisplayName("Send On")]
    public DateTime? Mail_Send_On { get; set; }


    public Container()
      : base() {
      //Booking = new Booking();
      //TransferBooking = new Booking();
    }

    #region IListType Members

    [HiddenInput(DisplayValue = false)]
    public virtual string ListText {
      get { return ID.ToString(); }
    }

    [HiddenInput(DisplayValue = false)]
    public virtual string ListValue {
      get { return ID.ToString(); }
    }

    #endregion

  }
}
