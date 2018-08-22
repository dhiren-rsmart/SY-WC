using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using smART.Common;
using System.Configuration;
using System.Data.Common;
using System.Data;
using Telerik.Web.Mvc;

namespace smART.Model {

  public class smARTDBContext : DbContext {

    public DbSet<Role> M_Role { get; set; }
    public DbSet<Employee> M_Employee { get; set; }
    public DbSet<EmployeeRole> Emp_Role { get; set; }
    public DbSet<Feature> M_Feature { get; set; }
    public DbSet<RoleFeature> Role_Feature_Int { get; set; }
    //public DbSet<DataAccessControl> M_DataAccessControl { get; set; }
    //public DbSet<DataAccessControlFilter> M_DataAccessControl_Filter { get; set; }

    public DbSet<LOVType> M_LOV { get; set; }
    public DbSet<LOV> LOV_Value { get; set; }

    public DbSet<Party> M_Party { get; set; }
    public DbSet<AddressBook> M_Address { get; set; }
    public DbSet<Contact> M_Contact { get; set; }
    public DbSet<Bank> M_Bank { get; set; }
    public DbSet<Note> Notes_Appointment { get; set; }
    public DbSet<Bin> M_Bin { get; set; }
    public DbSet<Item> M_Item { get; set; }
    public DbSet<ItemNotes> M_Item_Notes { get; set; }
    public DbSet<ItemAttachment> M_Item_Attachment { get; set; }
    public DbSet<PriceList> T_PriceList { get; set; }
    public DbSet<PriceListItem> T_PriceListItem { get; set; }


    public DbSet<DispatcherRequest> T_Dispatcher { get; set; }
    public DbSet<ExpensesRequest> T_Expenses { get; set; }

    public DbSet<SalesOrder> T_SalesOrder { get; set; }
    public DbSet<SalesOrderItem> T_SalesOrderItem { get; set; }
    public DbSet<SalesOrderAttachments> T_SalesOrder_Attachments { get; set; }
    public DbSet<SalesOrderNotes> T_SalesOrder_Notes { get; set; }

    public DbSet<PurchaseOrder> T_PurchaseOrder { get; set; }
    public DbSet<PurchaseOrderItem> T_PurchaseOrderItem { get; set; }
    public DbSet<PurchaseOrderAttachments> T_PurchaseOrder_Attachments { get; set; }
    public DbSet<PurchaseOrderNotes> T_PurchaseOrder_Notes { get; set; }

    public DbSet<Booking> T_Booking_Ref { get; set; }
    public DbSet<Container> T_Container_Ref { get; set; }
    public DbSet<BookingAttachments> T_Booking_Attachments { get; set; }
    public DbSet<BookingNotes> T_Booking_Notes { get; set; }

    public DbSet<Scale> T_Scale { get; set; }
    public DbSet<ScaleDetails> T_Scale_Details { get; set; }
    public DbSet<ScaleAttachments> T_Scale_Attachments { get; set; }
    public DbSet<ScaleNotes> T_Scale_Notes { get; set; }

    public DbSet<Invoice> T_Invoice { get; set; }
    public DbSet<InvoiceAttachments> T_Invoice_Attachments { get; set; }
    public DbSet<InvoiceNotes> T_Invoice_Notes { get; set; }
    public DbSet<Inventory> T_Inventory { get; set; }

    public DbSet<UOMConversion> M_UOM_Conversion { get; set; }

    public DbSet<Settlement> T_Settlement { get; set; }
    public DbSet<SettlementDetails> T_Settlement_Details { get; set; }
    public DbSet<AuditLog> T_Audit_Log { get; set; }

    public DbSet<PaymentReceipt> T_Payment_Receipt { get; set; }
    public DbSet<PaymentReceiptDetails> T_Payment_Receipt_Details { get; set; }
    public DbSet<PaymentReceiptAttachments> T_Payment_Receipt_Attachments { get; set; }
    public DbSet<PaymentReceiptNotes> T_Payment_Receipt_Notes { get; set; }

    public DbSet<Asset> M_Asset { get; set; }
    public DbSet<AssetAudit> T_Asset_Audit { get; set; }
    public DbSet<AssetAttachments> M_Asset_Attachments { get; set; }

    public DbSet<FocusArea> M_Foucs_Area { get; set; }

    public DbSet<InventoryAudit> T_Inventory_Audit { get; set; }

    public DbSet<QBLog> T_QB_Log { get; set; }

    public DbSet<ScaleIDCardAttachments> T_Scale_IDCardAttachments {
      get;
      set;
    }

    public DbSet<Cycle> T_Cycle {
      get;
      set;
    }

    public DbSet<CycleDetails> T_Cycle_Details {
      get;
      set;
    }

    public DbSet<Cash> T_Cash {
      get;
      set;
    }

    public DbSet<DeviceSettings> M_DeviceSettings
    {
        get;
        set;
    }


    public DbSet<LeadLog> T_Lead_Log
    {
        get;
        set;
    }

    public smARTDBContext()
      : base() {
    }

    public smARTDBContext(string dbContextConnectionString)
      : base(dbContextConnectionString) {
    }

    protected override void OnModelCreating(DbModelBuilder modelBuilder) {
      modelBuilder.Conventions.Remove<IncludeMetadataConvention>();
      base.OnModelCreating(modelBuilder);

      modelBuilder.Entity<SettlementDetails>().Property(x => x.Item_UOM_NetWeight).HasPrecision(16, 4);
      modelBuilder.Entity<PriceListItem>().Property(x => x.Price).HasPrecision(16, 4);
      modelBuilder.Entity<ScaleDetails>().Property(x => x.Rate).HasPrecision(16, 4);

    }

  }

  public class smARTDbContextInitializer : DropCreateDatabaseAlways<smARTDBContext> {
    protected override void Seed(smARTDBContext context) {
      Employee[] employees = new Employee[] {
                new Employee() { User_ID="admin", Password="admin", Active_Ind=true, Emp_Name="admin", Unique_ID=101, Created_Date=DateTime.Now },
                new Employee() { User_ID="user", Password="user", Active_Ind=true, Emp_Name="user", Unique_ID=102, Created_Date=DateTime.Now}
            };

      foreach (Employee employee in employees)
        context.M_Employee.Add(employee);
      context.SaveChanges();

      Role[] roles = new Role[] {
                new Role() { Role_Name="administrator", Role_Description="administrator", Unique_ID=201, Created_Date=DateTime.Now },
                new Role() { Role_Name="expenses", Role_Description="expenses", Unique_ID=202, Created_Date=DateTime.Now}
            };

      foreach (Role role in roles)
        context.M_Role.Add(role);
      context.SaveChanges();

      List<Feature> features = new List<Feature>();
      Array enumValues = Enum.GetValues(typeof(smART.Common.EnumFeatures));
      foreach (smART.Common.EnumFeatures feature in enumValues)
        features.Add(new Feature() { FeatureName = Enum.GetName(typeof(smART.Common.EnumFeatures), feature), Created_Date = DateTime.Now, Active_Ind = true });

      foreach (Feature feature in features)
        context.M_Feature.Add(feature);
      context.SaveChanges();

      EmployeeRole[] employeeRoles = new EmployeeRole[] {
                new EmployeeRole() { Employee=context.M_Employee.SingleOrDefault(o=>o.User_ID=="admin"), 
                    Role=context.M_Role.SingleOrDefault(o=>o.Role_Name=="administrator"), Active_Ind=true, Created_Date=DateTime.Now, Unique_ID=401},
                new EmployeeRole() { Employee=context.M_Employee.SingleOrDefault(o=>o.User_ID=="user"), 
                    Role=context.M_Role.SingleOrDefault(o=>o.Role_Name=="expenses"), Active_Ind=true, Created_Date=DateTime.Now, Unique_ID=402},
            };

      foreach (EmployeeRole empRole in employeeRoles)
        context.Emp_Role.Add(empRole);
      context.SaveChanges();

      List<RoleFeature> roleFeatures = new List<RoleFeature>();
      Role adminRole = context.M_Role.SingleOrDefault(o => o.Role_Name == "administrator");
      foreach (Feature feature in features) {
        roleFeatures.Add(new RoleFeature() {
          Role = adminRole,
          Feature = context.M_Feature.SingleOrDefault(o => o.FeatureName == feature.FeatureName),
          Active_Ind = true,
          Created_Date = DateTime.Now,
          DeleteAccessInd = true,
          EditAccessInd = true,
          NewAccessInd = true,
          ViewAccessInd = true
        });
      }

      foreach (RoleFeature roleFeature in roleFeatures)
        context.Role_Feature_Int.Add(roleFeature);
      context.SaveChanges();

      // LOV Types
      string[] lovTypes = new string[] { 
                "PARTY_TYPE",
                "BIN_TYPE",
                "ADDRESS_TYPE",
                "PHONE_TYPE",
                "PAYMENT_TERMS",
                "SHIPPING_CONTAINERS",
                "PORT_OF_DISCHARGE",
                "CONTACT_ROLE",
                "REQUEST_CATEGORY",
                "REQUEST_TYPE",
                "REQUEST_STATUS",
                "TICKET_STATUS",
                "TICKET_TYPE",
                "WEIGHT_TYPE",
                "UOM",
                "CURRENCY_TYPE",
                "PAID_BY",
                "ITEM_CATEGORY",
                "SHIPPING_TERMS",
                "SHIPPING_MEDIUM",
                "ORDER_TYPE",
                "EXPENSE_TYPE",
                "INV_TRAN_TYPE",
                "Tran_mode",
                "ORDER_STATUS",
                "Packaging_Type",
                "ITEM_GROUP",
                "ORDER_SUB_TYPE",
                "ROLE_TYPE",
                "Print_Area",
                "Note_TYPE",
                "Yes_No",
                "LOAD_TYPE",
                "SCALE_LIST",                
                "STATUS",
                "Drawer",
                "ASSET_TYPE",
                "PRICE_TYPE",
                "Container_Status",
                "Material_Type",
                "Invoice_Status",
                "Scale_Type",
                "Make",
                "Model",
                "Color"                
            };

      foreach (string lovType in lovTypes) {
        LOVType lType = new LOVType() { LOVType_Name = lovType, LOVType_Description = lovType, Active_Ind = true, Site_Org_ID = 1, Created_By = "admin", Created_Date = DateTime.Now, Last_Updated_Date = DateTime.Now, Updated_By = "admin" };
        context.M_LOV.Add(lType);
      }
      context.SaveChanges();

      string[][] lovs = null;
      LOVType olovType = null;

      lovs = new string[][] { 
                                       new string[] { "Supplier", "Buyer", "Brokerage", "Industrial", "Trucking company", "Shipping Company ", "Vendor", "External Agency", "Trader","Organization","Forwarder"} ,
                                       new string[] { "20 Yards",	"20 Yards with Hooks",	"30 Yards Long",	"30 Yards Short",	"4x6",	"4x8",	"40 Yards Long",	"40 Yards Short",	"Bunker",	"Gaylord" } ,
                                       new string[] { "Shipping Address",	"Billing Address",	"Head office",	"Site" } ,
                                       new string[] { "Office 1",	"Home",	"Cell",	"Fax",	"Office 2" } ,
                                       new string[] { "10 days net",	"100% advance before shippment",	"30 days net",	"30% advance and rest on shipment" } ,
                                       new string[] { "20' Regular",	"40' Standard",	"53' Standard" } ,
                                       new string[] { "Ahmedabad",	"Chennai",	"Mundra",	"Singapore",	"Houston",	"Colombo",	"Chittagong" } ,
                                       new string[] { "Driver",	"Owner",	"Manager",	"Office Admin",	"Accounting/Finance" } ,
                                       new string[] { "Bin",	"Container" } ,
                                       //new string[] { "Supplier Request - Inbound/bin",	"Buyer Request - Outbound/Container" } ,
                                       new string[] { "Pickup only",	"Drop off only",	"Pickup and Drop off" } ,
                                       new string[] { "Open",	"Close" } ,
                                        new string[] { "Open",	"Close" } ,
                                       new string[] { "Receiving Ticket",	"Shipping Ticket","Local Sales" } ,
                                        new string[] { "GROSS",	"TARE/NETT","Contamination" } ,
                                        new string[] { "Each",	"GT","MT","NT","KG","LBS","ST","Ton","Lot" },
                                       new string[] { "USD", "INR" },
                                        new string[] { "SELF", "PARTY" },
                                       new string[] { "Ferrous","Non-Ferrous"},
                                        new string[] { "C & F","FOB","Houston Port"},
                                        new string[] { "Container", "Ocean Vessel","Truck","Breuk Bull"},
                                        new string[] { "Firm", "Quotation" },
                                        new string[] { "Brokerage Commission","Processing Charge","Bank Fees","Contract Labor","Container pick up","Container turn in","Hauling","PSIC Inspection","Commission","Shipping / Handling","Trucking"},
                                        new string[] { "Receipts_Invoice", "Payments_Invoice" },
                                        new string[] { "Cash", "Check" },
                                        new string[] { "Open", "Closed" },
                                        new string[] { "Loose", "Bundle" },
                                        new string[] { "Aluminium", "Copper","Steel","Other"},
                                        new string[] { "Scale", "Trading" },
                                        new string[] { "Scale Administrator", "Finance Administrator" , "Accounting Administrator", "Shipping and Operations Manager", "Dispatcher", "Administrator", "Owner/Super User", },
                                        new string[] { "Header","Detail","Footer"},
                                        new string [] {"General Notes","Instructions","Print Notes"},
                                        new string[] { "Y", "N" },
                                        new string[] { "Empty",	"Loaded" },
                                        new string [] { "Rail Scale",	"Truck scale - Blue","Truck scale - yellow"  },   
                                        new string [] { "Open",	"Closed","WIP"}  ,
                                        new string [] { "1",	"2","3"},  
                                        new string [] { "Bin",	"Forklift","Crain","Computer","Printer"} , 
                                        new string [] { "Price Per Unit","Price List","AMM/COMEX/LME"}  ,
                                        new string [] {"Open-Empty","Open-Loaded","Shipped","WIP","Closed"},
                                        new string [] {"Prepared","Unprepared"},
                                        new string [] {"Open","Closed"},
                                        new string [] {"Scale1","Scale2"},
                                        new string [] {"Acura","Audi","BMW","Buick","Cadillac","Chevrolet","Chrysler","Dodge","Ferarri","Ford","GMC","Honda","Hummer","Hyundai","Infiniti","Isuzu","Jaguar","Jeep","Kia","Land Rover","Lexus","Lincoln","Lotus","Maserati","Mazda","Mercedes-Benz","Mercury","MINI"},
                                        new string [] {"MDX","RDX","RL","TL","TSX","A3","A4","A5","A6"},
                                        new string [] {"White","Black","Silver","Gray","Red","Blue","Brown","Yellow","Green"},
                            };

      int i = 0;
      foreach (string[] lov in lovs) {
        string strlovType = lovTypes[i];
        olovType = context.M_LOV.SingleOrDefault(o => o.LOVType_Name.Equals(strlovType));
        foreach (string values in lov) {
          LOV l = new LOV() { LOV_Value = values, LOV_Display_Value = values, LOVType = olovType, Active_Ind = true, Site_Org_ID = 1, Created_By = "admin", Created_Date = DateTime.Now, Last_Updated_Date = DateTime.Now, Updated_By = "admin", LOV_Active = true };
          context.LOV_Value.Add(l);
        }
        i++;
      }
      context.SaveChanges();

      int partyCount = 0;
      foreach (string partyTypeLov in lovs[0]) {
        int innerPartyCount = 0;
        for (innerPartyCount = 0; innerPartyCount < 10; innerPartyCount++) {
          Party party = new Party() {
            Party_Name = partyTypeLov + "_Party_Name_" + innerPartyCount.ToString(),
            Party_Short_Name = "PSN" + partyCount.ToString(),
            Party_Type = partyTypeLov,
            Party_Email = "email" + partyCount.ToString() + "@email.com",
            Party_Web_Site = "wwww.partywebsite" + partyCount.ToString() + ".com",
            Active_Ind = true,
            Party_Credit_Limit = 100,
            Party_Currency = "USD",
            Party_Insurance_Limit = 100,
            Party_Fax_No = "111-111-1111",
            Party_Phone1 = "222-222-2222"
          };
          context.M_Party.Add(party);

          partyCount++;
        }
      }
      context.SaveChanges();

      for (int j = 0; j < partyCount; j++) {
        string pSName = "PSN" + j.ToString();
        Party party = context.M_Party.SingleOrDefault(o => o.Party_Short_Name.Equals(pSName));
        for (int k = 0; k < 5; k++) {
          Contact contact = new Contact() {
            First_Name = party.Party_Name + "_First_Name_" + k.ToString(),
            Last_Name = party.Party_Name + "_Last_Name_" + k.ToString(),
            Email = party.Party_Name + "_contact" + k.ToString() + "@email.com",
            Active_Ind = true,
            Fax_No = "111-111-1111",
            Home_Phone = "222-222-2222",
            Party = party
          };
          context.M_Contact.Add(contact);
        }

        //for (int k = 0; k < 50; k++)
        //{
        //    AddressBook ab = new AddressBook()
        //    {
        //        Address1=party.Party_Name + "_Address 1_" + k.ToString(),
        //        Address2 = party.Party_Name + "_Address 2_" + k.ToString(),
        //        City = party.Party_Name + "City" + k.ToString(),
        //        State = party.Party_Name + "State" + k.ToString(),
        //        Country="US",
        //        Zip_Code="77057"
        //    };
        //    context.M_Address.Add(ab);
        //}

        //for (int k = 0; k < 50; k++)
        //{
        //    Bank bank = new Bank()
        //    {

        //        Bank_Address1 = party.Party_Name + "_Address 1_" + k.ToString(),
        //        Bank_Address2 = party.Party_Name + "_Address 2_" + k.ToString(),
        //        Account_Name = party.Party_Name + "Account_Name" + k.ToString(),
        //        Account_No = party.Party_Name + "Account_No" + k.ToString(),
        //        Bank_Name = party.Party_Name + "Bank_Name" + k.ToString(),
        //        Notes_Instruction = party.Party_Name + "Notes_Instruction" + k.ToString(),
        //        Routing_No = party.Party_Name + "Routing_No" + k.ToString(),
        //        Email = party.Party_Name + "@email.com" + k.ToString(),
        //        Fax_No = "111-111-1111",
        //        Phone_No = "222-222-2222",
        //        Active_Ind = true,
        //        Party = party
        //    };
        //    context.M_Bank.Add(bank);
        //}

        //for (int k = 0; k < 50; k++)
        //{
        //    Bin bin = new Bin()
        //    {
        //        Bin_Value = party.Party_Name + "_Bin_Value_" + k.ToString(),
        //        Active_Ind = true,
        //        Party = party
        //    };
        //    context.M_Bin.Add(bin);
        //}

        //for (int k = 0; k < 50; k++)
        //{
        //    Note note = new Note()
        //    {
        //         Date=DateTime.Now,
        //         Notes_Desc = party.Party_Name + "_Notes_Desc_" + k.ToString(),
        //         Reminder = party.Party_Name + "_Reminder_" + k.ToString()
        //    };
        //    context.Notes_Appointment.Add(note);
        //}

      }

      for (int k = 0; k < 10; k++) {
        Item item = new Item() {
          Item_Category = "Ferrous",
          Item_Group = "Aluminium",
          Short_Name = "Item" + k,
          Long_Name = "Item" + k,
          Priced = true,
          Regulated_Item = true,
          Require_VIN = true,
          Active_Ind = true,
          Site_Org_ID = 1,
          Created_By = "admin",
          Created_Date = DateTime.Now,
          Last_Updated_Date = DateTime.Now,
          Updated_By = "admin",
          Opening_Balance = 0,
          Current_Balance = 0
        };
        context.M_Item.Add(item);
      }

      //For Reports header 
      // To change inplace of top1 select the default true one
      string sqlString;
      //= "Create View [V_Company] as " +
      //    " SELECT  TOP (1) M_Party.ID, M_Party.Party_Name, M_Party.Party_Phone1, M_Party.Party_Email, M_Party.Party_Short_Name, M_Party.Party_Type, M_Party.Party_Currency, " +
      //    " M_Party.Party_Credit_Limit, M_Party.Party_Insurance_Limit, M_Party.Party_Fax_No, M_Party.Party_Web_Site, M_Party.Unique_ID, M_Party.Active_Ind, " +
      //    " M_Party.Created_By, M_Party.Updated_By, M_Party.Created_Date, M_Party.Last_Updated_Date, M_Party.Site_Org_ID, M_Address.ID AS Expr1, " +
      //    " M_Address.Address1, M_Address.Address2, M_Address.City, M_Address.State, M_Address.Country, M_Address.Zip_Code, M_Address.Unique_ID AS Expr2, " +
      //    " M_Address.Active_Ind AS Expr3, M_Address.Created_By AS Expr4, M_Address.Updated_By AS Expr5, M_Address.Created_Date AS Expr6, " +
      //    " M_Address.Last_Updated_Date AS Expr7, M_Address.Site_Org_ID AS Expr8, M_Address.Party_ID, M_Party.Party_Type AS Expr9 " +
      //    " FROM  M_Party LEFT OUTER JOIN M_Address ON M_Party.ID = M_Address.Party_ID " +
      //    " WHERE (M_Party.Party_Type = N'Organization')";


      sqlString = "Create View [V_Company] as " +
                  "SELECT TOP (1) dbo.M_Party.ID, dbo.M_Party.Party_Name, dbo.M_Party.Party_Phone1, dbo.M_Party.Party_Email, dbo.M_Party.Party_Short_Name, dbo.M_Party.Party_Type, " +
                  "dbo.M_Party.Party_Currency, dbo.M_Party.Party_Credit_Limit, dbo.M_Party.Party_Insurance_Limit, dbo.M_Party.Party_Fax_No, dbo.M_Party.Party_Web_Site, " +
                  "dbo.M_Party.Unique_ID, dbo.M_Party.Active_Ind, dbo.M_Party.Created_By, dbo.M_Party.Updated_By, dbo.M_Party.Created_Date, dbo.M_Party.Last_Updated_Date, " +
                  "dbo.M_Party.Site_Org_ID, dbo.M_Address.ID AS M_ID, dbo.M_Address.Address1, dbo.M_Address.Address2, dbo.M_Address.City, dbo.M_Address.State, " +
                  "dbo.M_Address.Country, dbo.M_Address.Zip_Code, dbo.M_Address.Unique_ID AS M_Unique_ID, dbo.M_Address.Active_Ind AS M_Active_Ind, " +
                  "dbo.M_Address.Created_By AS M_Created_By, dbo.M_Address.Updated_By AS M_Updated_By, dbo.M_Address.Created_Date AS M_Created_Date, " +
                  "dbo.M_Address.Last_Updated_Date AS M_Last_Updated_Date, dbo.M_Address.Site_Org_ID AS M_Site_Org_ID, dbo.M_Address.Party_ID, " +
                  "dbo.M_Party.Party_Type AS M_Party_Type, dbo.M_Bank.Account_No, dbo.M_Bank.Bank_Name, dbo.M_Bank.Account_Name, dbo.M_Bank.Routing_No, " +
                  "dbo.M_Bank.Notes_Instruction, dbo.M_Bank.Bank_Address1, dbo.M_Bank.Bank_Address2, dbo.M_Bank.City AS M_BankCity, dbo.M_Bank.State AS M_BankState, " +
                  "dbo.M_Bank.Country AS M_BankCountry " +
                  "FROM dbo.M_Party LEFT OUTER JOIN " +
                  "dbo.M_Bank ON dbo.M_Party.ID = dbo.M_Bank.Party_ID LEFT OUTER JOIN " +
                  "dbo.M_Address ON dbo.M_Party.ID = dbo.M_Address.Party_ID " +
                  "WHERE (dbo.M_Party.Party_Type = N'Organization')";

      context.Database.ExecuteSqlCommand(sqlString);
      context.SaveChanges();

      // for SO reports 
      sqlString = "Create procedure [SPRP_SalesOrder](@SalesOrderID varchar(10)) as " +
                  "begin  " +
                  "SELECT      M_Item.Short_Name, SOI.Packaging_Type, Customer.Party_Name, " +
                  "					 SO.ID AS ItemID, dbo.fn_GetCommentsSO(SO.ID,'Header') HeaderNotes,dbo.fn_GetCommentsSO(SO.ID,'Footer')  FooterNotes, " +
                  "                      SO.Sales_Order_No, SO.Order_Type, SO.Order_ConfirmedBy, SO.Due_Date, SO.Payment_Terms, " +
                  "                      SO.Shipping_Terms, SO.Delivery_Destination, SO.Ship_Via, SO.Party_Location, SO.Party_Order_Ref, " +
                  "                      SO.Order_Status, SO.Order_Closed_By, SO.Order_Requested_By, SO.Order_Created_By, SO.Order_Date,  " +
                  "                      SO.Order_Expired_By, SO.Delivery_Due_Date, SO.Qty_Variance, SO.Unique_ID, SO.Active_Ind,  " +
                  "                      SO.Created_By, SO.Updated_By, SO.Created_Date, SO.Last_Updated_Date, SO.Site_Org_ID,  " +
                  "                      SO.Party_ID, SO.Contact_ID,  " +
                  "                      SOI.SalesOrder_ID, SO.ID, SOI.Item_Qty, SOI.Item_UOM, SOI.Price,SOI.Container_Type,soi.No_Of_Containers , " +
                  "                      SOI.Price_UOM,dbo.fn_GetCommentsSO(SO.ID,'Detail') ItemNotes, " +
                  "                      Customer.Party_Name Cust_Name, " +
                  "                      Cust_Address.Address1, Cust_Address.Address2, Cust_Address.City, Cust_Address.State, Cust_Address.Country " +
                  "FROM         T_Sales_Order SO INNER JOIN  " +
                  "                      T_Sales_Order_Items SOI ON SO.ID = SOI.SalesOrder_ID INNER JOIN " +
                  "                      M_Item ON SOI.Item_ID = M_Item.ID INNER JOIN " +
                  "                      M_Party AS Customer ON SO.Party_ID = Customer.ID LEFT OUTER JOIN " +
                  "                      M_Address as Cust_Address ON Customer.ID = Cust_Address.Party_ID " +
                  "WHERE     (SO.ID = @SalesOrderID) " +
                  "end ";

      context.Database.ExecuteSqlCommand(sqlString);
      context.SaveChanges();

      sqlString = "CREATE FUNCTION [fn_GetCommentsSO] " +
                  "(	@Sales_ID int, @Print_Area  varchar(20) )   " +
                  "RETURNS varchar(max)  AS  " + "\n" +
                  "Begin  " +
                  "declare @Comments varchar (max) = ''   " +
                  "	select @Comments+= Notes from T_SalesOrder_Notes where Parent_ID=@Sales_ID and  Note_Type='Print Notes' and [Print] = 1 and Active_Ind = 1 and Print_Area = @Print_Area " +
                  "	return @Comments  " +
                  "end";

      context.Database.ExecuteSqlCommand(sqlString);
      context.SaveChanges();

      //for PO reports
      sqlString = "Create FUNCTION [fn_GetCommentsPO] " +
                  "(@Purchase_ID int, @Print_Area  varchar(20) )  " +
                  " RETURNS varchar(max)  AS  " +
                  "Begin  " +
                  "declare @Comments varchar (max) = ''   	" +
                  " select @Comments+= Notes from T_PurchaseOrder_Notes where Parent_ID=@Purchase_ID and  Note_Type='Print Notes' and [Print] = 1 and Active_Ind = 1 and Print_Area = @Print_Area " +
                  "	return @Comments " +
                  " end ";

      context.Database.ExecuteSqlCommand(sqlString);
      context.SaveChanges();

      //for Scal reports
      sqlString = "Create FUNCTION [fn_GetCommentsScale] " +
                  "(@Scale_ID int, @Print_Area  varchar(20) " +
                  ") " +
                  "RETURNS varchar(max)   AS " +
                  "Begin " +
                  "declare @Comments varchar (max) = '' " +
                  "select @Comments+= Notes from T_Scale_Notes where Parent_ID=@Scale_ID and  Note_Type='Print Notes' and [Print] = 1 and Active_Ind = 1 and Print_Area = @Print_Area " +
                  "return @Comments " +
                  "end";

      context.Database.ExecuteSqlCommand(sqlString);
      context.SaveChanges();


      sqlString = "Create procedure [SPRP_PurchaseOrder](@PurchaseOrderID varchar(10)) " +
      "as " +
      "begin " +
      "SELECT  M_Item.Short_Name, POI.Packaging_Type, " +
      "dbo.fn_GetCommentsPO(PO.ID,'Header') HeaderNotes," +
      "dbo.fn_GetCommentsPO(PO.ID,'Footer') FooterNotes," +
      "dbo.fn_GetCommentsPO(PO.ID,'Detail') ItemNotes," +
      "PO.ID as [PO_ID],   " +
      "[Purchase_Order_No] AS [PO_Purchase_Order_No] ," +
      "[Order_Type] AS [PO_Order_Type] ," +
      "[Order_Status] AS [PO_Order_Status] ," +
      "[Order_Closed_By] AS [PO_Order_Closed_By] ," +
      "[Order_Requested_By] AS [PO_Order_Requested_By] ," +
      "[Order_Created_By] AS [PO_Order_Created_By] ," +
      "[Scale_Broker] AS [PO_Scale_Broker] ," +
      "[Party_Location] AS [PO_Party_Location] ," +
      "[Delivery_Destination] AS [PO_Delivery_Destination] ," +
      "[Ship_Via] AS [PO_Ship_Via] ," +
      "[Payment_Terms] AS [PO_Payment_Terms] ," +
      "[Shipping_Terms] AS [PO_Shipping_Terms] ," +
      "[Order_Date] AS [PO_Order_Date] ," +
      "[Order_Expiry_Date] AS [PO_Order_Expiry_Date] ," +
      "[Delivery_Due_Date] AS [PO_Delivery_Due_Date] ," +
      "[Qty_Variance] AS [PO_Qty_Variance] ," +
      "[Party_Order_Ref] AS [PO_Party_Order_Ref] ," +
      "PO.[Unique_ID] AS [PO_Unique_ID] ," +
      "PO.[Active_Ind] AS [PO_Active_Ind] ," +
      "PO.[Created_By] AS [PO_Created_By] ," +
      "PO.[Updated_By] AS [PO_Updated_By] ," +
      "PO.[Created_Date] AS [PO_Created_Date] ," +
      "PO.[Last_Updated_Date] AS [PO_Last_Updated_Date] ," +
      "PO.[Site_Org_ID] AS [PO_Site_Org_ID] ," +
      "[Price_List_ID] AS [PO_Price_List_ID] ," +
      "PO.[Party_ID] AS [PO_Party_ID] ," +
      "[Contact_ID] AS [PO_Contact_ID] ," +
      "POI.[ID] AS [POI_ID]," +
      "[Packaging_Type] AS [POI_Packaging_Type]," +
      "[Ordered_Qty] AS [POI_Ordered_Qty]," +
      "[Ordered_Qty_UOM] AS [POI_Ordered_Qty_UOM]," +
      "[Price_Type] AS [POI_Price_Type]," +
      "[Price] AS [POI_Price]," +
      "[Price_UOM] AS [POI_Price_UOM]," +
      "[No_Of_Containers] AS [POI_No_Of_Containers]," +
      "[Container_Type] AS [POI_Container_Type]," +
      "[Order_Confirmed_By] AS [POI_Order_Confirmed_By]," +
      "[Expense_Type] AS [POI_Expense_Type]," +
      "[Payment_Method] AS [POI_Payment_Method]," +
      "[Payment_Method_Amt] AS [POI_Payment_Method_Amt]," +
      "[Payment_Method_UOM] AS [POI_Payment_Method_UOM]," +
      "POI.[Unique_ID] AS [POI_Unique_ID]," +
      "POI.[Active_Ind] AS [POI_Active_Ind]," +
      "POI.[Created_By] AS [POI_Created_By]," +
      "POI.[Updated_By] AS [POI_Updated_By]," +
      "POI.[Created_Date] AS [POI_Created_Date]," +
      "POI.[Last_Updated_Date] AS [POI_Last_Updated_Date]," +
      "POI.[Site_Org_ID] AS [POI_Site_Org_ID]," +
      "POI.[Item_ID] AS [POI_Item_ID]," +
      "POI.[PurchaseOrder_ID] AS [POI_PurchaseOrder_ID]," +
      "Customer.Party_Name Cust_Name,Cust_Address.Address1, Cust_Address.Address2, Cust_Address.City, Cust_Address.State, Cust_Address.Country," +
      "CO.[ID] AS [CO_ID]," +
      "CO.[Party_Name] AS [CO_Party_Name]," +
      "CO.[Party_Phone1] AS [CO_Party_Phone1]," +
      "CO.[Party_Email] AS [CO_Party_Email]," +
      "CO.[Party_Short_Name] AS [CO_Party_Short_Name]," +
      "CO.[Party_Type] AS [CO_Party_Type]," +
      "CO.[Party_Currency] AS [CO_Party_Currency]," +
      "CO.[Party_Credit_Limit] AS [CO_Party_Credit_Limit]," +
      "CO.[Party_Insurance_Limit] AS [CO_Party_Insurance_Limit]," +
      "CO.[Party_Fax_No] AS [CO_Party_Fax_No]," +
      "CO.[Party_Web_Site] AS [CO_Party_Web_Site]," +
      "CO.[Unique_ID] AS [CO_Unique_ID]," +
      "CO.[Active_Ind] AS [CO_Active_Ind]," +
      "CO.[Created_By] AS [CO_Created_By]," +
      "CO.[Updated_By] AS [CO_Updated_By]," +
      "CO.[Created_Date] AS [CO_Created_Date]," +
      "CO.[Last_Updated_Date] AS [CO_Last_Updated_Date]," +
      "CO.[Site_Org_ID] AS [CO_Site_Org_ID]," +
      "CO.[Address1] AS [CO_Address1]," +
      "CO.[Address2] AS [CO_Address2]," +
      "CO.[City] AS [CO_City]," +
      "CO.[State] AS [CO_State]," +
      "CO.[Country] AS [CO_Country]," +
      "CO.[Zip_Code] AS [CO_Zip_Code]," +
      "CO.[Party_ID] AS [CO_Party_ID]," +
      "CO.[Account_No] as [CO_Account_No]," +
      "CO.[Bank_Name] as [CO_Bank_Name]," +
      "CO.[Account_Name] as [CO_Account_Name]," +
      "CO.[Routing_No] as [CO_Routing_No]," +
      "CO.[Notes_Instruction] as [CO_Notes_Instruction]," +
      "CO.[Bank_Address1] as [CO_Bank_Address1]," +
      "CO.[Bank_Address2] as [CO_Bank_Address2]," +
      "CO.[M_BankCity] as [CO_M_BankCity]," +
      "CO.[M_BankState] as [CO_M_BankState]," +
      "CO.[M_BankCountry] as [CO_M_BankCountry] " +
      "FROM V_Company as CO, T_Purchase_Order PO INNER JOIN " +
      " T_Purchase_Order_Items POI ON PO.ID = POI.PurchaseOrder_ID INNER JOIN" +
      " M_Item ON POI.Item_ID = M_Item.ID INNER JOIN " +
      " M_Party AS Customer ON PO.Party_ID = Customer.ID LEFT OUTER JOIN " +
      " M_Address as Cust_Address ON Customer.ID = Cust_Address.Party_ID " +
      " WHERE (PO.ID = @PurchaseOrderID) " +
      "end ";

      context.Database.ExecuteSqlCommand(sqlString);
      context.SaveChanges();

      sqlString = "Create procedure [SPRP_Scale_Receive](@ScaleID int) " +
              "as " +
              "begin  " +
              "SELECT " +
              "dbo.fn_GetCommentsScale(Sc.ID,'Header') HeaderNotes, " +
              "dbo.fn_GetCommentsScale(SC.ID,'Footer')  FooterNotes, " +
              "dbo.fn_GetCommentsScale(Sc.ID,'Deails') DetailNotes , " +
              "SC.[ID] AS [SC_ID], " +
              "SC.[Scale_Ticket_No] AS [SC_Scale_Ticket_No], " +
              "SC.[Ticket_Type] AS [SC_Ticket_Type], " +
              "SC.[Ticket_Status] AS [SC_Ticket_Status], " +
              "SC.[Vehicle_Type] AS [SC_Vehicle_Type], " +
              "SC.[Truck_No] AS [SC_Truck_No], " +
              "SC.[Vehicle_Plate_No] AS [SC_Vehicle_Plate_No], " +
              "SC.[Trailer_Chasis_No] AS [SC_Trailer_Chasis_No], " +
              "SC.[Other_Details] AS [SC_Other_Details], " +
              "SC.[Driver_Name] AS [SC_Driver_Name], " +
              "SC.[Gross_Weight] AS [SC_Gross_Weight], " +
              "SC.[Tare_Weight] AS [SC_Tare_Weight], " +
              "SC.[Net_Weight] AS [SC_Net_Weight], " +
              "SC.[Supplier_Scale_Ticket_No] AS [SC_Supplier_Scale_Ticket_No], " +
              "SC.[Asset_ID] AS [SC_Asset_ID], " +
              "SC.[Seal_No] AS [SC_Seal_No], " +
              "SC.[Ticket_Settled] AS [SC_Ticket_Settled], " +
              "SC.[Unique_ID] AS [SC_Unique_ID], " +
              "SC.[Active_Ind] AS [SC_Active_Ind], " +
              "SC.[Created_By] AS [SC_Created_By], " +
              "SC.[Updated_By] AS [SC_Updated_By], " +
              "SC.[Created_Date] AS [SC_Created_Date], " +
              "SC.[Last_Updated_Date] AS [SC_Last_Updated_Date], " +
              "SC.[Site_Org_ID] AS [SC_Site_Org_ID], " +
              "SC.[Dispatch_Request_No_ID] AS [SC_Dispatch_Request_No_ID], " +
              "SC.[Party_ID_ID] AS [SC_Party_ID_ID], " +
              "SC.[Purchase_Order_ID] AS [SC_Purchase_Order_ID], " +
              "SC.[Container_No_ID] AS [SC_Container_No_ID], " +
        //                    "SC.[Shipping_Item_ID_ID] AS [SC_Shipping_Item_ID_ID], " +
              "SCD.[ID] AS [SCD_ID], " +
              "SCD.Split_Value AS [SCD_Split_Value], " +
              "SCD.[GrossWeight] AS [SCD_GrossWeight], " +
              "SCD.[TareWeight] AS [SCD_TareWeight], " +
              "SCD.[Contamination_Weight] AS [SCD_Contamination_Weight], " +
              "SCD.[NetWeight] AS [SCD_NetWeight], " +
              "SCD.[Supplier_Item] AS [SCD_Supplier_Item], " +
              "SCD.[Supplier_Net_Weight] AS [SCD_Supplier_Net_Weight], " +
              "SCD.[Unique_ID] AS [SCD_Unique_ID], " +
              "SCD.[Active_Ind] AS [SCD_Active_Ind], " +
              "SCD.[Created_By] AS [SCD_Created_By], " +
              "SCD.[Updated_By] AS [SCD_Updated_By], " +
              "SCD.[Created_Date] AS [SCD_Created_Date], " +
              "SCD.[Last_Updated_Date] AS [SCD_Last_Updated_Date], " +
              "SCD.[Site_Org_ID] AS [SCD_Site_Org_ID], " +
              "SCD.[Scale_ID] AS [SCD_Scale_ID], " +
              "SCD.[Item_Received_ID] AS [SCD_Item_Received_ID], " +
              "SCD.[Apply_To_Item_ID] AS [SCD_Apply_To_Item_ID], " +
              "ITM_RECV.Short_Name Item_Recv_Name,  " +
              "ITM_APPLY_TO.Short_Name Item_Appy_To_Name, " +
              "Customer.Party_Name Cust_Name,  " +
              "Cust_Address.Address1,  " +
              "Cust_Address.Address2,  " +
              "Cust_Address.City,  " +
              "Cust_Address.State,  " +
              "Cust_Address.Country ," +
              "Company.Party_Name as Co_PartyName " +
              "FROM [T_Scale] SC LEFT Outer Join T_Scale_Details SCD   ON SC.ID = SCD.Scale_ID  " +
              "LEFT OUTER JOIN M_Party AS Customer ON Sc.Party_ID_ID = Customer.ID " +
              "LEFT OUTER JOIN M_Address as Cust_Address ON Customer.ID = Cust_Address.Party_ID " +
              "LEFT OUTER JOIN M_Item ITM_RECV on ITM_RECV.ID= SCD.Item_Received_ID " +
              "LEFT OUTER JOIN M_Item ITM_APPLY_TO on ITM_APPLY_TO.ID= SCD.Apply_To_Item_ID, " +
              "V_Company aS Company " +
              "WHERE (Sc.ID = @ScaleID) " +
              "end  ";

      context.Database.ExecuteSqlCommand(sqlString);
      context.SaveChanges();

      sqlString = "Create procedure [SPRP_Scale_Shipping](@ScaleID int) " +
              "as " +
              "begin  " +
              "SELECT " +
              "dbo.fn_GetCommentsScale(Sc.ID,'Header') HeaderNotes, " +
              "dbo.fn_GetCommentsScale(SC.ID,'Footer')  FooterNotes, " +
              "dbo.fn_GetCommentsScale(Sc.ID,'Deails') DetailNotes , " +
              "SC.[ID] AS [SC_ID], " +
              "SC.[Scale_Ticket_No] AS [SC_Scale_Ticket_No], " +
              "SC.[Ticket_Type] AS [SC_Ticket_Type], " +
              "SC.[Ticket_Status] AS [SC_Ticket_Status], " +
              "SC.[Vehicle_Type] AS [SC_Vehicle_Type], " +
              "SC.[Truck_No] AS [SC_Truck_No], " +
              "SC.[Vehicle_Plate_No] AS [SC_Vehicle_Plate_No], " +
              "SC.[Trailer_Chasis_No] AS [SC_Trailer_Chasis_No], " +
              "SC.[Other_Details] AS [SC_Other_Details], " +
              "SC.[Driver_Name] AS [SC_Driver_Name], " +
              "SC.[Gross_Weight] AS [SC_Gross_Weight], " +
              "SC.[Tare_Weight] AS [SC_Tare_Weight], " +
              "SC.[Net_Weight] AS [SC_Net_Weight], " +
              "SC.[Supplier_Scale_Ticket_No] AS [SC_Supplier_Scale_Ticket_No], " +
              "SC.[Asset_ID] AS [SC_Asset_ID], " +
              "SC.[Seal_No] AS [SC_Seal_No], " +
              "SC.[Ticket_Settled] AS [SC_Ticket_Settled], " +
              "SC.[Unique_ID] AS [SC_Unique_ID], " +
              "SC.[Active_Ind] AS [SC_Active_Ind], " +
              "SC.[Created_By] AS [SC_Created_By], " +
              "SC.[Updated_By] AS [SC_Updated_By], " +
              "SC.[Created_Date] AS [SC_Created_Date], " +
              "SC.[Last_Updated_Date] AS [SC_Last_Updated_Date], " +
              "SC.[Site_Org_ID] AS [SC_Site_Org_ID], " +
              "SC.[Dispatch_Request_No_ID] AS [SC_Dispatch_Request_No_ID], " +
              "SC.[Party_ID_ID] AS [SC_Party_ID_ID], " +
              "SC.[Purchase_Order_ID] AS [SC_Purchase_Order_ID], " +
              "SC.[Container_No_ID] AS [SC_Container_No_ID], " +
              "SCD.[ID] AS [SCD_ID], " +
              "SCD.Split_Value AS [SCD_Split_Value], " +
              "SCD.[GrossWeight] AS [SCD_GrossWeight], " +
              "SCD.[TareWeight] AS [SCD_TareWeight], " +
              "SCD.[Contamination_Weight] AS [SCD_Contamination_Weight], " +
              "SCD.[NetWeight] AS [SCD_NetWeight], " +
              "SCD.[Supplier_Item] AS [SCD_Supplier_Item], " +
              "SCD.[Supplier_Net_Weight] AS [SCD_Supplier_Net_Weight], " +
              "SCD.[Unique_ID] AS [SCD_Unique_ID], " +
              "SCD.[Active_Ind] AS [SCD_Active_Ind], " +
              "SCD.[Created_By] AS [SCD_Created_By], " +
              "SCD.[Updated_By] AS [SCD_Updated_By], " +
              "SCD.[Created_Date] AS [SCD_Created_Date], " +
              "SCD.[Last_Updated_Date] AS [SCD_Last_Updated_Date], " +
              "SCD.[Site_Org_ID] AS [SCD_Site_Org_ID], " +
              "SCD.[Scale_ID] AS [SCD_Scale_ID], " +
              "SCD.[Item_Received_ID] AS [SCD_Item_Received_ID], " +
              "SCD.[Apply_To_Item_ID] AS [SCD_Apply_To_Item_ID], " +
              "ITM_RECV.Short_Name Item_Recv_Name,  " +
              "ITM_APPLY_TO.Short_Name Item_Appy_To_Name, " +
              "Customer.Party_Name Cust_Name,  " +
              "Cust_Address.Address1,  " +
              "Cust_Address.Address2,  " +
              "Cust_Address.City,  " +
              "Cust_Address.State,  " +
              "Cust_Address.Country " +
              "FROM [T_Scale] SC LEFT Outer Join T_Scale_Details SCD   ON SC.ID = SCD.Scale_ID  " +
              "LEFT OUTER JOIN M_Party AS Customer ON Sc.Party_ID_ID = Customer.ID " +
              "LEFT OUTER JOIN M_Address as Cust_Address ON Customer.ID = Cust_Address.Party_ID " +
              "LEFT OUTER JOIN M_Item ITM_RECV on ITM_RECV.ID= SCD.Item_Received_ID " +
              "LEFT OUTER JOIN M_Item ITM_APPLY_TO on ITM_APPLY_TO.ID= SCD.Apply_To_Item_ID " +
              "WHERE (Sc.ID = @ScaleID) " +
              "end  ";

      context.Database.ExecuteSqlCommand(sqlString);
      context.SaveChanges();

      UOMConversion uomConv = new UOMConversion {
        Conversion_UOM = "GT",
        Conversion_UOM_Desc = "Gross Ton",
        Base_UOM = "LBS",
        Base_UOM_Desc = "Pounds",
        Factor = 2240,
        Is_Base_UOM = true,
        Active_Ind = true,
        Created_By = "Admin",
        Created_Date = DateTime.Now
      };
      context.M_UOM_Conversion.Add(uomConv);
      context.SaveChanges();

      uomConv = new UOMConversion {
        Conversion_UOM = "MT",
        Conversion_UOM_Desc = "Metric Ton",
        Base_UOM = "LBS",
        Base_UOM_Desc = "Pounds",
        Factor = 2204.62262,
        Is_Base_UOM = true,
        Active_Ind = true,
        Created_By = "Admin",
        Created_Date = DateTime.Now
      };

      context.M_UOM_Conversion.Add(uomConv);
      context.SaveChanges();

      uomConv = new UOMConversion {
        Conversion_UOM = "NT",
        Conversion_UOM_Desc = "Nett Ton",
        Base_UOM = "LBS",
        Base_UOM_Desc = "Pounds",
        Factor = 2000,
        Is_Base_UOM = true,
        Active_Ind = true,
        Created_By = "Admin",
        Created_Date = DateTime.Now
      };
      context.M_UOM_Conversion.Add(uomConv);
      context.SaveChanges();

      uomConv = new UOMConversion {
        Conversion_UOM = "KG",
        Conversion_UOM_Desc = "Kilogram",
        Base_UOM = "LBS",
        Base_UOM_Desc = "Pounds",
        Factor = 2.20462,
        Is_Base_UOM = true,
        Active_Ind = true,
        Created_By = "Admin",
        Created_Date = DateTime.Now
      };
      context.M_UOM_Conversion.Add(uomConv);
      context.SaveChanges();

      uomConv = new UOMConversion {
        Conversion_UOM = "ST",
        Conversion_UOM_Desc = "Short Ton",
        Base_UOM = "LBS",
        Base_UOM_Desc = "Pounds",
        Factor = 2000,
        Is_Base_UOM = true,
        Active_Ind = true,
        Created_By = "Admin",
        Created_Date = DateTime.Now
      };
      context.M_UOM_Conversion.Add(uomConv);
      context.SaveChanges();

      uomConv = new UOMConversion {
        Conversion_UOM = "Ton",
        Conversion_UOM_Desc = "Ton",
        Base_UOM = "LBS",
        Base_UOM_Desc = "Pounds",
        Factor = 2000,
        Is_Base_UOM = true,
        Active_Ind = true,
        Created_By = "Admin",
        Created_Date = DateTime.Now
      };
      context.M_UOM_Conversion.Add(uomConv);
      context.SaveChanges();

      uomConv = new UOMConversion {
        Conversion_UOM = "LBS",
        Conversion_UOM_Desc = "Pounds",
        Base_UOM = "KG",
        Base_UOM_Desc = "Kilogram",
        Factor = 0.453592,
        Is_Base_UOM = true,
        Active_Ind = true,
        Created_By = "Admin",
        Created_Date = DateTime.Now
      };
      context.M_UOM_Conversion.Add(uomConv);
      context.SaveChanges();

      uomConv = new UOMConversion {
        Conversion_UOM = "GT",
        Conversion_UOM_Desc = "Gross Ton",
        Base_UOM = "KG",
        Base_UOM_Desc = "Kilogram",
        Factor = 1016.04691,
        Is_Base_UOM = false,
        Active_Ind = true,
        Created_By = "Admin",
        Created_Date = DateTime.Now
      };
      context.M_UOM_Conversion.Add(uomConv);
      context.SaveChanges();

      uomConv = new UOMConversion {
        Conversion_UOM = "MT",
        Conversion_UOM_Desc = "Metric Ton",
        Base_UOM = "KG",
        Base_UOM_Desc = "Kilogram",
        Factor = 1000,
        Is_Base_UOM = false,
        Active_Ind = true,
        Created_By = "Admin",
        Created_Date = DateTime.Now
      };
      context.M_UOM_Conversion.Add(uomConv);
      context.SaveChanges();

      uomConv = new UOMConversion {
        Conversion_UOM = "NT",
        Conversion_UOM_Desc = "Nett Ton",
        Base_UOM = "KG",
        Base_UOM_Desc = "Kilogram",
        Factor = 907.184,
        Is_Base_UOM = false,
        Active_Ind = true,
        Created_By = "Admin",
        Created_Date = DateTime.Now
      };
      context.M_UOM_Conversion.Add(uomConv);
      context.SaveChanges();


      //DataAccessControl[] dataAccessControles = new DataAccessControl[] {
      //    new DataAccessControl() {Feature = context.M_Feature.SingleOrDefault(o => o.FeatureName==EnumFeatures.Transaction_PurchaseOrder.ToString()),                
      //                            Role=context.M_Role.SingleOrDefault(o=>o.Role_Name=="DataAccess"), 
      //                            Active_Ind=true, 
      //                            Created_Date=DateTime.Now, 
      //                            Unique_ID=501},

      //                            new DataAccessControl() {Feature = context.M_Feature.SingleOrDefault(o => o.FeatureName==EnumFeatures.Transaction_Dispatcher.ToString()),                
      //                            Role=context.M_Role.SingleOrDefault(o=>o.Role_Name=="DataAccess"), 
      //                            Active_Ind=true, 
      //                            Created_Date=DateTime.Now, 
      //                            Unique_ID=502}

      //};

      //    foreach (DataAccessControl dataAccessControle in dataAccessControles)
      //    {
      //        context.M_DataAccessControl.Add(dataAccessControle);
      //        context.SaveChanges();

      //        DataAccessControlFilter[] dataAccessControlFilters = new DataAccessControlFilter[] {
      //                                new DataAccessControlFilter(){ DataAccessControl = dataAccessControle, FilterFieldType="Table" ,FilterFieldName="Type",
      //                                                               FilterFieldValue="Firm", Unique_ID=201,   Active_Ind=true, Created_Date=DateTime.Now},
      //                                new DataAccessControlFilter(){ DataAccessControl = dataAccessControle, FilterFieldType="Table" ,FilterFieldName="Status",
      //                                                               FilterFieldValue="Closed", Unique_ID=201,   Active_Ind=true, Created_Date=DateTime.Now}
      //        };



      //        foreach (DataAccessControlFilter dataAccessControlFilter in dataAccessControlFilters)
      //        {
      //            context.M_DataAccessControl_Filter.Add(dataAccessControlFilter);
      //            context.SaveChanges();
      //        }


      //    }
    }
  }

}
