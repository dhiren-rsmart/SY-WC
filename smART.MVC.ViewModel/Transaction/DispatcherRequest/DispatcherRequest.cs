using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Web.Mvc;

namespace smART.ViewModel {

  public class DispatcherRequest : BaseEntity {

    //[UIHint("PartyDropDownList")]
    [Display(Name = "Trucking Company")]
    [DisplayName("Trucking Company")]
    public Party TruckingCompany { get; set; }

    //[UIHint("ContactDropDownList")]
    [Display(Name = "Driver")]
    public Contact Driver { get; set; }


    [UIHint("LOVDropDownList")]
    [RegularExpression(@"^((?![Ss]elect).)*$", ErrorMessage = "Please select a value for Request Category")]
    [StringLength(45, ErrorMessage = "Maximum length is 45")]
    [Display(Name = "Request Category")]
    public string RequestCategory { get; set; }

    public int Request_No { get; set; }

    [UIHint("LOVDropDownList")]
    [RegularExpression(@"^((?![Ss]elect).)*$", ErrorMessage = "Please select a value for Request Type")]
    [StringLength(45, ErrorMessage = "Maximum length is 45")]
    [Display(Name = "Request Type")]
    public string RequestType { get; set; }

    [UIHint("LOVDropDownList")]
    [RegularExpression(@"^((?![Ss]elect).)*$", ErrorMessage = "Please select a value for Request Status")]
    [StringLength(45, ErrorMessage = "Maximum length is 45")]
    [Display(Name = "Request Status")]
    public string RequestStatus { get; set; }

    [UIHint("PartyDropDownList")]
    public Party Party {
      get {
        if (RequestCategory != null && RequestCategory.Contains("Bin"))
          return Party_Supplier;
        else
          return Party_Buyer;
      }
      set {
        if (RequestCategory != null && RequestCategory.Contains("Bin"))
          Party_Supplier = value;
        else
          Party_Buyer = value;
      }

    }

    [Display(Name = "Buyer")]
    public Party Party_Buyer { get; set; }

    [Display(Name = "Supplier")]
    public Party Party_Supplier { get; set; }

    [Display(Name = "Location")]
    public AddressBook Location { get; set; }

    [StringLength(45, ErrorMessage = "Maximum length is 45")]
    [Display(Name = "Bin No.")]
    public string Bin_No { get; set; }

    [Display(Name = "Bin No.")]
    [HiddenInput(DisplayValue = false)]
    public Asset Asset { get; set; }

    [Display(Name = "Asset#")]
    [HiddenInput(DisplayValue = false)]
    public string Asset_No { get { if (Asset != null) return Asset.Asset_No; else return ""; } }

    [HiddenInput(DisplayValue = false)]
    public AssetAuditLookup AssetAuditLookup { get; set; }

    [Required]
    [StringLength(45, ErrorMessage = "Maximum length is 45")]
    [Display(Name = "Order No")]
    public string Order_No { get; set; }

    [Display(Name = "Purchase Order No.")]
    public PurchaseOrder Purchase_Order_No { get; set; }

    
    [Display(Name = "Booking Ref No.")]
    public Booking Booking_Ref_No { get; set; }

    [HiddenInput(DisplayValue = false)]
    [Display(Name = "Booking#")]
    public string Booking_No { get { if (Booking_Ref_No != null) return Booking_Ref_No.Booking_Ref_No; else return ""; } }

    [Display(Name = "Sales Order No.")]
    public SalesOrder Sales_Order_No { get; set; }

    [Display(Name = "Shipping Line")]
    public Party Shipper { get; set; }

    [StringLength(45, ErrorMessage = "Maximum length is 45")]
    [Display(Name = "Container No.")]
    public string Container_No { get; set; }

    public Container Container_Lookup { get; set; }

    [UIHint("BlankDate")]
    public DateTime? Time { get; set; }

    [Display(Name = "Amount To Be Paid")]
    public Decimal Amount_Supplier { get; set; }

    [Display(Name = "Amount To Be Paid")]
    public Decimal Amount_Buyer { get; set; }

    public Decimal Amount_To_Be_Paid {
      get {
        if (RequestCategory != null && RequestCategory.Contains("Bin"))
          return Amount_Supplier;
        else
          return Amount_Buyer;
      }
      set {
        if (RequestCategory != null && RequestCategory.Contains("Bin"))
          Amount_Supplier = value;
        else
          Amount_Buyer = value;
      }
    }

    [Display(Name = "Source")]
    public string Source_Supplier { get; set; }

    [Display(Name = "Destination")]
    public string Destination_Supplier { get; set; }

    [Display(Name = "Source")]
    public string Source_Buyer { get; set; }

    [Display(Name = "Destination")]
    public string Destination_Buyer { get; set; }

    public string Source {
      get {
        if (RequestCategory != null && RequestCategory.Contains("Bin"))
          return Source_Supplier;
        else
          return Source_Buyer;
      }
      set {
        if (RequestCategory != null && RequestCategory.Contains("Bin"))
          Source_Supplier = value;
        else
          Source_Buyer = value;
      }
    }

    public string Destination {
      get {
        if (RequestCategory != null && RequestCategory.Contains("Bin"))
          return Destination_Supplier;
        else
          return Destination_Buyer;
      }
      set {
        if (RequestCategory != null && RequestCategory.Contains("Bin"))
          Destination_Supplier = value;
        else
          Destination_Buyer = value;
      }
    }

    [Display(Name = "Container Number")]
    public Container Container { get; set; }

    public DispatcherRequest() {
      Time = DateTime.Now;
      //Party = new Party();
      //TruckingCompany = new Party();
      //Driver = new Contact();
      //Location = new AddressBook();
      //Party_Buyer = new Party();
      //Party_Supplier = new Party();
      //Shipper = new Party();
      //Booking_Ref_No = new Booking();
      //Sales_Order_No = new SalesOrder();
      //Asset = new Asset();
      //AssetAuditLookup = new AssetAuditLookup();
      //Container = new Container();
    }

  }
}
