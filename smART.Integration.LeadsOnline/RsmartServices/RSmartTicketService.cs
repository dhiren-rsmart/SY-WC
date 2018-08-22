using System;
using System.Collections.Generic;
using System.Linq;
using smART.Common;
using smART.Integration.LeadsOnline.LeadsOnlineWCF;
using smART.Library;
using smART.ViewModel;

namespace smART.Integration.LeadsOnline {
  public class RSmartTicketService {
    ScaleLibrary _scaleLib;
    ScaleDetailsLibrary _scaleDetailLib;
    ScaleAttachmentsLibrary _scaleAttachmentLib;
    LeadLogLibrary _leadLogLib;
    public RSmartTicketService() {
      _scaleLib = new ScaleLibrary(ConfigurationHelper.GetsmARTDBContextConnectionString());
      _scaleDetailLib = new ScaleDetailsLibrary(ConfigurationHelper.GetsmARTDBContextConnectionString());
      _scaleAttachmentLib = new ScaleAttachmentsLibrary(ConfigurationHelper.GetsmARTDBContextConnectionString());
      _leadLogLib = new LeadLogLibrary(ConfigurationHelper.GetsmARTDBContextConnectionString());
    }


    public Scale GetTicketById(int id) {
      return _scaleLib.GetByID(id.ToString());
    }

    public IEnumerable<Scale> GetPendingTickets() {
      IEnumerable<Scale> tickets = _scaleLib.GetPendingLeadsTicket(new string[] { "Party_ID", "Party_Address" });
      //IEnumerable<Scale> tickets = new List<Scale>() { _scaleLib.GetByID("15", new string[] { "Party_ID", "Party_Address" }) };
      return tickets;
    }


    public IEnumerable<ScaleDetails> GetTicketItemsByTicketId(int ticketId) {
      IEnumerable<ScaleDetails> ticketItems = _scaleDetailLib.GetAllByParentID(ticketId, new string[] { "Scale", "Item_Received", "Apply_To_Item" });
      return ticketItems;
    }


    public IEnumerable<ScaleAttachments> GetTicketAttachmentsByTicketId(int ticketId) {
      IEnumerable<ScaleAttachments> ticketAttachments = _scaleAttachmentLib.GetAllByParentID(ticketId, new string[] { "Parent" });
      return ticketAttachments;
    }

    public IEnumerable<ScaleAttachments> GetItemAttachmentsByTicketIdAndItemId(int ticketId, int itemId) {
      IEnumerable<ScaleAttachments> ticketAttachments = _scaleAttachmentLib.GetAttachmentsByRefIdAndRefTypeAndParentId(EnumAttachmentRefType.Item, itemId, ticketId);
      return ticketAttachments;
    }


    public IEnumerable<ScaleAttachments> GetGeneralAttachmentsByTicketId(int ticketId) {
      IEnumerable<ScaleAttachments> ticketAttachments = _scaleAttachmentLib.GetAttachmentsByRefIdAndRefTypeAndParentId(EnumAttachmentRefType.General, 0, ticketId);
      return ticketAttachments;
    }

    public ScaleAttachments GetSignatureAttachmentByTicketId(int ticketId) {
      IEnumerable<ScaleAttachments> ticketAttachments = _scaleAttachmentLib.GetAttachmentsByRefIdAndRefTypeAndParentId(EnumAttachmentRefType.Signature, 0, ticketId);
      return ticketAttachments.FirstOrDefault();
    }

    public void UpdateTicketStatus(Scale scale) {
      //Scale scale = _scaleLib.GetByID(id.ToString());
      scale.Lead = true;
      _scaleLib.Modify(scale, new string[] { "Party_ID", "Party_Address" });
    }

    public AddressBook GetAddressByPartyId(int partyId) {
      AddressBookLibrary lib = new AddressBookLibrary(ConfigurationHelper.GetsmARTDBContextConnectionString());
      return lib.GetPrimaryAddressesByPartyId(partyId);
    }

    public void AddLeadLog(string ticketNo, EnumPostingStatus status, string statusRemarks) {
      LeadLog log = new LeadLog();
      log.Scale_Ticket_No = ticketNo;
      log.Status = status.ToString();
      log.Status_Remarks = statusRemarks;
      log.Active_Ind = true;
      log.Created_Date = DateTime.Now;
      log.Last_Updated_Date = DateTime.Now;
      log.Created_By = "Admin";
      log.Updated_By = "Admin";
      _leadLogLib.Add(log);
    }

    public Ticket MapTicketFromRsmartTicket(Scale rsmartScale, IEnumerable<ScaleDetails> rsmartScaleDetails, IEnumerable<ScaleAttachments> rsmartScaleAttachments, AddressBook rsmartAddress, Ticket t) {
      FilelHelper fileHelper = new FilelHelper();

      // Ticket Details
      t.key = new TicketKey();
      t.key.ticketnumber = rsmartScale.Scale_Ticket_No;
      t.key.ticketDateTime = rsmartScale.Created_Date.ToString();

      if (rsmartScale.Party_ID != null) {

        // Customer Details
        t.customer = new Customer();
        t.customer.name = rsmartScale.Party_ID.Party_Name;
        t.customer.idNumber = rsmartScale.Party_ID.License_No;
        string dobString = rsmartScale.Party_ID.Party_DOB;
        t.customer.phone = rsmartScale.Party_ID.Party_Phone1;


        DateTime dobDt;
        if (!string.IsNullOrEmpty(dobString) && smART.Common.DateTimeHelper.IsValidDate(dobString, out dobDt))
          t.customer.dob = dobDt.ToString();
        //t.customer.dob = Convert .ToString(rsmartScale.Party_ID.Party_DOB);

        // Address details               
        if (rsmartAddress != null) {
          t.customer.address1 = rsmartAddress.Address1;
          t.customer.city = rsmartAddress.City;
          t.customer.state = rsmartAddress.State;
          t.customer.postalCode = rsmartAddress.Zip_Code;
        }

        // Vehicle details
        t.customer.vehicle = new Vehicle();
        t.customer.vehicle.color = rsmartScale.Color;
        t.customer.vehicle.make = rsmartScale.Make;
        t.customer.vehicle.model = rsmartScale.Model;
        t.customer.vehicle.state = rsmartScale.Plate_State;
        t.customer.vehicle.license = rsmartScale.Vehicle_Plate_No;
        t.customer.vehicle.year = rsmartScale.Vehicle_Year;


        if (rsmartScaleAttachments != null && rsmartScaleAttachments.Count() > 0) {
          List<Image> customerImageList = new List<Image>();

          // Signature
          ScaleAttachments signatureAttachment = rsmartScaleAttachments.FirstOrDefault(a => a.Ref_Type == (int)EnumAttachmentRefType.Signature);
          if (signatureAttachment != null) {
            Image img = new Image();
            img.imageData = fileHelper.GetBytesFromFile(fileHelper.GetFilePathByFileRefId(signatureAttachment.Document_RefId.ToString()));
            img.imageType = ImageType.Jpeg;
            img.imageCategory = ImageCategory.Signature;
            customerImageList.Add(img);
          }

          // Thumb Print                   
          ScaleAttachments thumbAttachment = rsmartScaleAttachments.FirstOrDefault(a => a.Ref_Type == (int)EnumAttachmentRefType.Thumbprint1);
          if (thumbAttachment != null) {
            Image img = new Image();
            img.imageData = fileHelper.GetBytesFromFile(fileHelper.GetFilePathByFileRefId(thumbAttachment.Document_RefId.ToString()));
            img.imageType = ImageType.Jpeg;
            img.imageCategory = ImageCategory.Thumbprint;
            customerImageList.Add(img);

          }

          // Customer                  
          ScaleAttachments customerAttachment = rsmartScaleAttachments.FirstOrDefault(a => a.Ref_Type == (int)EnumAttachmentRefType.Customer);
          if (customerAttachment != null) {
            Image img = new Image();
            img.imageData = fileHelper.GetBytesFromFile(fileHelper.GetFilePathByFileRefId(customerAttachment.Document_RefId.ToString()));
            img.imageType = ImageType.Jpeg;
            img.imageCategory = ImageCategory.Customer;
            customerImageList.Add(img);
          }

          // Customer License Id                   
          ScaleAttachments licenseAttachment = rsmartScaleAttachments.FirstOrDefault(a => a.Ref_Type == (int)EnumAttachmentRefType.DriverLicense);
          if (licenseAttachment != null) {
            Image img = new Image();
            img.imageData = fileHelper.GetBytesFromFile(fileHelper.GetFilePathByFileRefId(licenseAttachment.Document_RefId.ToString()));
            img.imageType = ImageType.Jpeg;
            img.imageCategory = ImageCategory.CustomerID;
            customerImageList.Add(img);
          }

          // Vehicle                   
          ScaleAttachments vehicleLicenseAttachment = rsmartScaleAttachments.FirstOrDefault(a => a.Ref_Type == (int)EnumAttachmentRefType.Vehicle);
          if (vehicleLicenseAttachment != null) {
            Image img = new Image();
            img.imageData = fileHelper.GetBytesFromFile(fileHelper.GetFilePathByFileRefId(vehicleLicenseAttachment.Document_RefId.ToString()));
            img.imageType = ImageType.Jpeg;
            img.imageCategory = ImageCategory.Vehicle;
            customerImageList.Add(img);
          }

          // Cash Card                   
          ScaleAttachments cashCardAttachment = rsmartScaleAttachments.FirstOrDefault(a => a.Ref_Type == (int)EnumAttachmentRefType.CashCard);
          if (cashCardAttachment != null) {
            Image img = new Image();
            img.imageData = fileHelper.GetBytesFromFile(fileHelper.GetFilePathByFileRefId(cashCardAttachment.Document_RefId.ToString()));
            img.imageType = ImageType.Jpeg;
            img.imageCategory = ImageCategory.Customer;
            customerImageList.Add(img);
          }
          t.customer.images = new Image[] { };
          t.customer.images = customerImageList.ToArray<Image>();
        }
      }

      if (rsmartScaleDetails != null && rsmartScaleDetails.Count() > 0) {
        // Item Details
        List<smART.Integration.LeadsOnline.LeadsOnlineWCF.Item> itemList = new List<smART.Integration.LeadsOnline.LeadsOnlineWCF.Item>();
        string clerk =ConfigurationHelper.GetClerk();
        foreach (var item in rsmartScaleDetails) {
          smART.Integration.LeadsOnline.LeadsOnlineWCF.Item item1 = new smART.Integration.LeadsOnline.LeadsOnlineWCF.Item();
          item1.description = item.Apply_To_Item.Long_Name;
          item1.amount = Convert.ToDouble(item.Amount);
          item1.weight = Convert.ToDouble(item.NetWeight);
          item1.employee = clerk;

          // Item image
          List<ScaleAttachments> itemAttachments = rsmartScaleAttachments.Where<ScaleAttachments>(s => s.Ref_ID == item.ID && s.Ref_Type == (int)EnumAttachmentRefType.Item).ToList(); //|| s.Document_Title == item.Apply_To_Item.Short_Name
          if (itemAttachments != null && itemAttachments.Count() > 0) {
            List<Image> imageList = new List<Image>();
            foreach (var itemAttach in itemAttachments) {
              Image itemImage = new Image();
              itemImage.imageData = fileHelper.GetBytesFromFile(fileHelper.GetFilePathByFileRefId(itemAttach.Document_RefId.ToString()));
              itemImage.imageType = ImageType.Jpeg;
              itemImage.imageCategory = ImageCategory.Item;
              imageList.Add(itemImage);
            }
            item1.images = new Image[] { };
            item1.images = imageList.ToArray<Image>();
          }
          itemList.Add(item1);
        }

        t.items = new smART.Integration.LeadsOnline.LeadsOnlineWCF.Item[] { };
        t.items = itemList.ToArray<smART.Integration.LeadsOnline.LeadsOnlineWCF.Item>();
      }
      return t;
    }
  }
}
