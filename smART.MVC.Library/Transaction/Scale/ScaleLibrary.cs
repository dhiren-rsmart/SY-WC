using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Model = smART.Model;
using VModel = smART.ViewModel;
using AutoMapper;
using System.Data.Entity;

using Telerik.Web.Mvc;
using System.Linq.Expressions;
using ScaleReader;
using System.Data;

namespace smART.Library {

  public class ScaleLibrary : GenericLibrary<VModel.Scale, Model.Scale> {
    public ScaleLibrary()
      : base() {
    }

    public ScaleLibrary(string dbContextConnectionString)
      : base(dbContextConnectionString) {
    }

    public override void Initialize(string dbContextConnectionString) {
      base.Initialize(dbContextConnectionString);

      Mapper.CreateMap<VModel.DispatcherRequest, Model.DispatcherRequest>();
      Mapper.CreateMap<Model.DispatcherRequest, VModel.DispatcherRequest>();

      Mapper.CreateMap<VModel.Party, Model.Party>();
      Mapper.CreateMap<Model.Party, VModel.Party>();

      Mapper.CreateMap<VModel.Container, Model.Container>();
      Mapper.CreateMap<Model.Container, VModel.Container>();

      //AutoMapper.Mapper.CreateMap<Model.Scale, VModel.Scale>()
      //    .ForMember(dest => dest.Container_No, opt => opt.NullSubstitute("N/A"));

      //Mapper.CreateMap<VModel.Scale, Model.Scale>();
      //Mapper.CreateMap<Model.Scale, VModel.Scale>().ForMember(d => d.Container_No, o => o.NullSubstitute(new VModel.Container()))
      //    .ForMember(d => d.Party_ID, o => o.NullSubstitute(new VModel.Party()));

      Mapper.CreateMap<VModel.PurchaseOrder, Model.PurchaseOrder>();
      Mapper.CreateMap<Model.PurchaseOrder, VModel.PurchaseOrder>();

      //Mapper.CreateMap<VModel.PurchaseOrderItem, Model.PurchaseOrderItem>();
      //Mapper.CreateMap<Model.PurchaseOrderItem, VModel.PurchaseOrderItem>();


      Mapper.CreateMap<VModel.Booking, Model.Booking>();
      Mapper.CreateMap<Model.Booking, VModel.Booking>();

      Mapper.CreateMap<VModel.SalesOrder, Model.SalesOrder>();
      Mapper.CreateMap<Model.SalesOrder, VModel.SalesOrder>();

      Mapper.CreateMap<VModel.Item, Model.Item>();
      Mapper.CreateMap<Model.Item, VModel.Item>();

      Mapper.CreateMap<VModel.AddressBook, Model.AddressBook>();
      Mapper.CreateMap<Model.AddressBook, VModel.AddressBook>();

      Mapper.CreateMap<VModel.Invoice, Model.Invoice>();
      Mapper.CreateMap<Model.Invoice, VModel.Invoice>();

      Mapper.CreateMap<VModel.Asset, Model.Asset>();
      Mapper.CreateMap<Model.Asset, VModel.Asset>();

      Mapper.CreateMap<VModel.PriceList, Model.PriceList>();
      Mapper.CreateMap<Model.PriceList, VModel.PriceList>();

    }

    public override VModel.Scale Add(VModel.Scale addObject) {

      VModel.Scale insertedObjectBusiness = addObject;
      try {
        decimal scaleReading = addObject.Scale_Reading;
        Model.Scale newModObject = Mapper.Map<VModel.Scale, Model.Scale>(addObject);
        if (newModObject.Dispatch_Request_No != null)
          newModObject.Dispatch_Request_No = _repository.GetQuery<Model.DispatcherRequest>().SingleOrDefault(o => o.ID == addObject.Dispatch_Request_No.ID);
        if (newModObject.Party_ID != null)
          newModObject.Party_ID = _repository.GetQuery<Model.Party>().SingleOrDefault(o => o.ID == addObject.Party_ID.ID);
        if (newModObject.Purchase_Order != null)
          newModObject.Purchase_Order = _repository.GetQuery<Model.PurchaseOrder>().SingleOrDefault(o => o.ID == addObject.Purchase_Order.ID);
        if (newModObject.Container_No != null)
          newModObject.Container_No = _repository.GetQuery<Model.Container>().SingleOrDefault(o => o.ID == addObject.Container_No.ID);
        if (newModObject.Party_Address != null)
          newModObject.Party_Address = _repository.GetQuery<Model.AddressBook>().SingleOrDefault(o => o.ID == addObject.Party_Address.ID);
        if (newModObject.Sales_Order != null)
          newModObject.Sales_Order = _repository.GetQuery<Model.SalesOrder>().SingleOrDefault(o => o.ID == addObject.Sales_Order.ID);
        if (newModObject.Invoice != null)
          newModObject.Invoice = _repository.GetQuery<Model.Invoice>().SingleOrDefault(o => o.ID == addObject.Invoice.ID);
        if (newModObject.Local_Sales_AND_Trading_Party != null)
          newModObject.Local_Sales_AND_Trading_Party = _repository.GetQuery<Model.Party>().SingleOrDefault(o => o.ID == addObject.Local_Sales_AND_Trading_Party.ID);
        if (newModObject.Asset != null)
          newModObject.Asset = _repository.GetQuery<Model.Asset>().SingleOrDefault(o => o.ID == addObject.Asset.ID);
        if (newModObject.Booking != null)
          newModObject.Booking = _repository.GetQuery<Model.Booking>().SingleOrDefault(o => o.ID == addObject.Booking.ID);
        if (newModObject.PriceList != null)
          newModObject.PriceList = _repository.GetQuery<Model.PriceList>().SingleOrDefault(o => o.ID == addObject.PriceList.ID);

        if (Adding(insertedObjectBusiness, newModObject, _dbContext)) {
          Model.Scale insertedObject = _repository.Add<Model.Scale>(newModObject);
          _repository.SaveChanges();
          insertedObjectBusiness = Mapper.Map<Model.Scale, VModel.Scale>(insertedObject);
          insertedObjectBusiness.Scale_Reading = scaleReading;
          Added(insertedObjectBusiness, newModObject, _dbContext);
        }
      }
      catch (Exception ex) {
        bool rethrow;
        rethrow = LibraryExceptionHandler.HandleException(ref ex, insertedObjectBusiness.Updated_By, insertedObjectBusiness.GetType().Name, insertedObjectBusiness.ID.ToString());
        if (rethrow)
          throw ex;
      }
      return insertedObjectBusiness;
    }

    protected override void Modify(Expression<Func<Model.Scale, bool>> predicate, VModel.Scale modObject, string[] includePredicate = null) {
      try {
        Model.Scale newModObject = Mapper.Map<VModel.Scale, Model.Scale>(modObject);
        if (modObject.Dispatch_Request_No != null)
          newModObject.Dispatch_Request_No = _repository.GetQuery<Model.DispatcherRequest>().SingleOrDefault(o => o.ID == modObject.Dispatch_Request_No.ID);
        if (modObject.Party_ID != null)
          newModObject.Party_ID = _repository.GetQuery<Model.Party>().SingleOrDefault(o => o.ID == modObject.Party_ID.ID);
        if (modObject.Purchase_Order != null)
          newModObject.Purchase_Order = _repository.GetQuery<Model.PurchaseOrder>().SingleOrDefault(o => o.ID == modObject.Purchase_Order.ID);
        if (modObject.Container_No != null)
          newModObject.Container_No = _repository.GetQuery<Model.Container>().SingleOrDefault(o => o.ID == modObject.Container_No.ID);
        if (modObject.Party_Address != null)
          newModObject.Party_Address = _repository.GetQuery<Model.AddressBook>().SingleOrDefault(o => o.ID == modObject.Party_Address.ID);
        if (modObject.Sales_Order != null)
          newModObject.Sales_Order = _repository.GetQuery<Model.SalesOrder>().SingleOrDefault(o => o.ID == modObject.Sales_Order.ID);
        if (modObject.Invoice != null)
          newModObject.Invoice = _repository.GetQuery<Model.Invoice>().SingleOrDefault(o => o.ID == modObject.Invoice.ID);
        if (modObject.Local_Sales_AND_Trading_Party != null)
          newModObject.Local_Sales_AND_Trading_Party = _repository.GetQuery<Model.Party>().SingleOrDefault(o => o.ID == modObject.Local_Sales_AND_Trading_Party.ID);
        if (modObject.Asset != null)
          newModObject.Asset = _repository.GetQuery<Model.Asset>().SingleOrDefault(o => o.ID == modObject.Asset.ID);
        if (modObject.Booking != null)
          newModObject.Booking = _repository.GetQuery<Model.Booking>().SingleOrDefault(o => o.ID == modObject.Booking.ID);
        if (modObject.PriceList != null)
          newModObject.PriceList = _repository.GetQuery<Model.PriceList>().SingleOrDefault(o => o.ID == modObject.PriceList.ID);

        if (Modifying(modObject, newModObject, _dbContext)) {
          _repository.Modify<Model.Scale>(predicate, newModObject, includePredicate);
          _repository.SaveChanges();
          Modified(modObject, newModObject, _dbContext);
        }
      }
      catch (Exception ex) {
        bool rethrow;
        rethrow = LibraryExceptionHandler.HandleException(ref ex, modObject.Updated_By, modObject.GetType().Name, modObject.ID.ToString());
        if (rethrow)
          throw ex;
      }
    }

    public IEnumerable<VModel.Scale> GetByStatus(string status, out int totalRows, int page, int pageSize, string sortColumn, string sortType, string[] includePredicate = null, IList<IFilterDescriptor> filters = null, string ticketType ="") {
      IEnumerable<Model.Scale> modEnumeration = _repository.FindByPaging<Model.Scale>(out totalRows, o => o.Ticket_Status.Equals(status, StringComparison.OrdinalIgnoreCase) && o.Ticket_Type == ticketType,
                                                                                     page, pageSize, sortColumn, sortType, includePredicate, filters);

      IEnumerable<VModel.Scale> busEnumeration = Map(modEnumeration);
      return busEnumeration;
    }


    public IEnumerable<VModel.Scale> GetUnSettledRecAndBrokAndTradScaleByPagging(out int totalRows, int page, int pageSize, string sortColumn, string sortType, string[] includePredicate = null, IList<IFilterDescriptor> filters = null) {
      IEnumerable<Model.Scale> modEnumeration = _repository.FindByPaging<Model.Scale>(out totalRows, o => (o.Ticket_Type.Equals("Receiving Ticket", StringComparison.OrdinalIgnoreCase)
                                                                                      || o.Ticket_Type.Equals("Brokerage", StringComparison.OrdinalIgnoreCase)
                                                                                      || o.Ticket_Type.Equals("Trading", StringComparison.OrdinalIgnoreCase))
                                                                                      && o.Ticket_Settled == false && o.Ticket_Status.ToLower() != "open",
                                                                                      page, pageSize, sortColumn, sortType, includePredicate, filters);
      IEnumerable<VModel.Scale> busEnumeration = Map(modEnumeration);

      return busEnumeration;

    }


    //public IEnumerable<VModel.Scale> GetUnSettledReceivingScale(string[] includePredicate = null) {
    //  IEnumerable<Model.Scale> modEnumeration = _repository.Find<Model.Scale>(o => o.Ticket_Type.Equals("Receiving Ticket", StringComparison.OrdinalIgnoreCase) && o.Ticket_Settled == false,
    //                                                                           includePredicate
    //                                                                         );
    //  IEnumerable<VModel.Scale> busEnumeration = Map(modEnumeration);

    //  return busEnumeration;
    //}

    public VModel.Scale GetUnSettledRecAndBrokAndTradScaleById(int id, string[] includePredicate = null) {
      IEnumerable<Model.Scale> modEnumeration = _repository.Find<Model.Scale>(o => (o.Ticket_Type.Equals("Receiving Ticket", StringComparison.OrdinalIgnoreCase)
                                                                              || o.Ticket_Type.Equals("Brokerage", StringComparison.OrdinalIgnoreCase)
                                                                               || o.Ticket_Type.Equals("Trading", StringComparison.OrdinalIgnoreCase))
                                                                              && o.Ticket_Settled == false
                                                                              && o.ID == id,
                                                                              includePredicate
                                                                             );
      IEnumerable<VModel.Scale> busEnumeration = Map(modEnumeration);

      return busEnumeration.FirstOrDefault();
    }

    public double GetScaleWeight(string ipAddress, int port, int waitTime, string command) {
      string strWeight = "0.00";
      //Get Weight
      ScaleReader.ScaleReader reader = new ScaleReader.ScaleReader(ipAddress, port, waitTime, command);
      strWeight = reader.ProcessString();

      double weight = 0.00;
      double.TryParse(strWeight, out weight);

      return weight;
    }

    public double GetScaleWeight(string comPort, int baudRate, int dataBits, int stopBits, int timeout, string logFileName) {
      string strWeight = "0.00";

      //Get Weight
      ComPortReader.COMData comData = new ComPortReader.COMData() {
        COMPortName = comPort,
        BaudRate = baudRate,
        DataBits = dataBits,
        StopBits = stopBits,
        FileName = logFileName,
        ReadTimeout = timeout,
        WriteTimeout = timeout,
        Parity = "None",
        Loop = 100,
        Duration = 1
      };
      ComPortReader.COMProcessor processor = new ComPortReader.COMProcessor();
      strWeight = processor.ReadData(comData);

      double weight = 0.00;
      double.TryParse(strWeight, out weight);

      return weight;
    }


    public IEnumerable<VModel.Scale> GetTicketsBySOWithPagging(int soId, out int totalRows, int page, int pageSize, string sortColumn, string sortType, string[] includePredicate = null, IList<IFilterDescriptor> filters = null) {
      IEnumerable<Model.Scale> modEnumeration = _repository.FindByPaging<Model.Scale>(out totalRows, o => o.Container_No.Booking.Sales_Order_No.ID == soId || o.Sales_Order.ID == soId,
                                                                                       page, pageSize, sortColumn, sortType, includePredicate,
                                                                                       filters
                                                                                      );

      IEnumerable<VModel.Scale> busEnumeration = Map(modEnumeration);

      UpdateConvertedSOUOMNetWeight(busEnumeration, soId);

      return busEnumeration;
    }

    public IEnumerable<VModel.Scale> GetTicketsByPOWithPagging(int poId, out int totalRows, int page, int pageSize, string sortColumn, string sortType, string[] includePredicate = null, IList<IFilterDescriptor> filters = null) {
      IEnumerable<Model.Scale> modEnumeration = _repository.FindByPaging<Model.Scale>(out totalRows, o => o.Purchase_Order.ID == poId,
                                                                                       page, pageSize, sortColumn, sortType, includePredicate,
                                                                                       filters
                                                                                      );

      IEnumerable<VModel.Scale> busEnumeration = Map(modEnumeration);

      UpdateConvertedPOUOMNetWeight(busEnumeration, poId);

      return busEnumeration;
    }

    private void UpdateConvertedSOUOMNetWeight(IEnumerable<VModel.Scale> scales, int soID) {
      foreach (var scale in scales) {
        scale.Net_Weight_SOUOM = GetConvertedSOUOMNetWeight(scale, soID);
      }
    }

    private void UpdateConvertedPOUOMNetWeight(IEnumerable<VModel.Scale> scales, int poID) {
      foreach (var scale in scales) {
        scale.Net_Weight_POUOM = GetConvertedPOUOMNetWeight(scale, poID);
      }
    }

    private decimal GetConvertedSOUOMNetWeight(VModel.Scale scale, int soID) {
      IEnumerable<Model.ScaleDetails> scaleDetails = _repository.Find<Model.ScaleDetails>(o => o.Scale.ID == scale.ID, new string[] { "Scale", "Item_Received", "Apply_To_Item" });
      decimal UOMNetWeight = 0;
      foreach (var item in scaleDetails) {
        Model.SalesOrderItem soScaleItem = GetScaleSOItem(soID, item);
        string uom = soScaleItem != null && !string.IsNullOrWhiteSpace(soScaleItem.Item_UOM) ? soScaleItem.Item_UOM : "LBS";
        UOMNetWeight += GetConvertedUOMWeight(item.NetWeight, uom);
      }
      return UOMNetWeight;
    }

    private decimal GetConvertedPOUOMNetWeight(VModel.Scale scale, int poID) {
      IEnumerable<Model.ScaleDetails> scaleDetails = _repository.Find<Model.ScaleDetails>(o => o.Scale.ID == scale.ID, new string[] { "Item_Received", "Apply_To_Item" });
      decimal UOMNetWeight = 0;
      foreach (var item in scaleDetails) {
        Model.PurchaseOrderItem poScaleItem = GetScalePOItem(poID, item);
        string uom = poScaleItem != null && !string.IsNullOrWhiteSpace(poScaleItem.Ordered_Qty_UOM) ? poScaleItem.Ordered_Qty_UOM : "LBS";
        UOMNetWeight += GetConvertedUOMWeight(item.NetWeight, uom);
      }
      return UOMNetWeight;
    }

    public Model.SalesOrderItem GetScaleSOItem(int soId, Model.ScaleDetails scaleDetails) {
      if (scaleDetails == null || scaleDetails.Apply_To_Item == null)
        return null;

      Model.SalesOrderItem item = (from soItem in _repository.GetQuery<Model.SalesOrderItem>()
                                   where soItem.SalesOrder.ID == soId
                                       && soItem.Item.ID == scaleDetails.Item_Received.ID
                                   select soItem).FirstOrDefault();
      return item;
    }

    public Model.PurchaseOrderItem GetScalePOItem(int poId, Model.ScaleDetails scaleDetails) {
      if (scaleDetails == null || scaleDetails.Apply_To_Item == null)
        return null;

      Model.PurchaseOrderItem item = (from poItem in _repository.GetQuery<Model.PurchaseOrderItem>()
                                      where poItem.PurchaseOrder.ID == poId
                                       && poItem.Item.ID == scaleDetails.Item_Received.ID
                                      select poItem).FirstOrDefault();
      return item;
    }

    private decimal GetConvertedUOMWeight(decimal itemUOMWeight, string uom) {
      if (!string.IsNullOrEmpty(uom) && uom.ToLower() != "lbs") {
        UOMConversionLibrary uomConvLib = new UOMConversionLibrary(_dbContextConnectionString);
        VModel.UOMConversion uomConv = uomConvLib.GetByUOM(uom, "LBS");
        if (uomConv != null) {
          itemUOMWeight = itemUOMWeight / (decimal) uomConv.Factor;
        }
      }
      itemUOMWeight = decimal.Round(itemUOMWeight, 3, MidpointRounding.AwayFromZero);
      return itemUOMWeight;
    }

    public IEnumerable<VModel.Scale> GetScalesByBookingId(int bookingId) {
      IEnumerable<Model.Scale> modEnumeration = _repository.Find<Model.Scale>(o => o.Ticket_Type == "Shipping Ticket" && o.Container_No.Booking.ID == bookingId, new string[] { "Container_No.Booking", "Dispatch_Request_No" });
      IEnumerable<VModel.Scale> busEnumeration = Map(modEnumeration);
      return busEnumeration;
    }

    public VModel.Scale GetScaleByBookingId(int bookingId) {
      IEnumerable<Model.Scale> modEnumeration = _repository.Find<Model.Scale>(o => o.Ticket_Type == "Brokerage" && o.Booking.ID == bookingId, new string[] { "Booking" });
      IEnumerable<VModel.Scale> busEnumeration = Map(modEnumeration);
      return busEnumeration.FirstOrDefault();
    }

    public VModel.Scale GetScalesByContainerId(int containerId) {
      IEnumerable<Model.Scale> modEnumeration = _repository.Find<Model.Scale>(o => o.Ticket_Type == "Shipping Ticket" && o.Container_No.ID == containerId, new string[] { "Container_No" });
      IEnumerable<VModel.Scale> busEnumeration = Map(modEnumeration);
      return busEnumeration.FirstOrDefault();
    }

    public IEnumerable<VModel.Scale> GetOpenTickets(string ticketType = "") {
      IEnumerable<Model.Scale> modEnumeration;
      if (string.IsNullOrEmpty(ticketType))
        modEnumeration = _repository.Find<Model.Scale>(o => o.Ticket_Status.Equals("Open", StringComparison.OrdinalIgnoreCase));
      else
        modEnumeration = _repository.Find<Model.Scale>(o => o.Ticket_Status.Equals("Open", StringComparison.OrdinalIgnoreCase) && o.Ticket_Type == ticketType);

      IEnumerable<VModel.Scale> busEnumeration = Map(modEnumeration);
      return busEnumeration;
    }

    public string GenerateUniqueId(string uniqueId) {
      IEnumerable<Model.Scale> modEnumeration = _repository.Find<Model.Scale>(o => o.Active_Ind == true);
      Model.Scale scale = modEnumeration.LastOrDefault();
      int maxID = scale == null ? 1 : scale.ID + 1;
      return string.Format("{0}-{1}", uniqueId, maxID);
    }

    public IEnumerable<VModel.Scale> GetAllByPaging(
       out int totalRows,
       int page,
       int pageSize,
       string sortColumn,
       string sortType,
       string[] includePredicate = null,
       IList<IFilterDescriptor> filters = null,
       string ticketType = ""
      ) {
      try {
        IEnumerable<Model.Scale> modEnumeration;
        if (string.IsNullOrEmpty(ticketType))
          return GetAllByPaging(out totalRows, page, pageSize, sortColumn, sortType, includePredicate, filters);
        else
          modEnumeration = _repository.FindByPaging<Model.Scale>(out totalRows, o => o.Ticket_Type == ticketType, page, pageSize, sortColumn, sortType, includePredicate, filters);

        IEnumerable<VModel.Scale> busEnumeration = Mapper.Map<IEnumerable<Model.Scale>, IEnumerable<VModel.Scale>>(modEnumeration);
        GotMultiple(busEnumeration, modEnumeration, _dbContext);
        return busEnumeration;
      }
      catch (Exception ex) {
        bool rethrow = LibraryExceptionHandler.HandleException(ref ex, System.Diagnostics.TraceEventType.Error);
        if (rethrow) {
          throw ex;
        }
        totalRows = 0;
        return null;
      }
    }

    public IEnumerable<VModel.Scale> GetPendingLeadsTicket(string[] includePredicate = null)
    {
        return GetByExpression(s => s.Lead == false && s.Active_Ind == true && s.Ticket_Status == "Close" && s.Ticket_Type == "Receiving Ticket",includePredicate);
     
    }


    public VModel.Scale GetLastTicketByPartyId(int partyId, string[] includePredicate = null)
    {
        return GetByExpression(s => s.Party_ID.ID == partyId && s.Active_Ind == true && s.Ticket_Type == "Receiving Ticket", includePredicate).LastOrDefault();

    }
  }
}
