using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Model = smART.Model;
using VModel = smART.ViewModel;

using AutoMapper;
using System.Data.Entity;
using Telerik.Web.Mvc;

namespace smART.Library {

  public class InvoiceItemLibrary : InvoiceChildLibrary<VModel.InvoiceItem, Model.InvoiceItem> {

    public InvoiceItemLibrary()
      : base() {
    }
    public InvoiceItemLibrary(string dbContextConnectionString)
      : base(dbContextConnectionString) {
    }

    public override void Initialize(string dbContextConnectionString) {
      base.Initialize(dbContextConnectionString);

      Mapper.CreateMap<VModel.Invoice, Model.Invoice>();
      Mapper.CreateMap<Model.Invoice, VModel.Invoice>();

      Mapper.CreateMap<VModel.BaseEntity, Model.BaseEntity>();
      Mapper.CreateMap<Model.BaseEntity, VModel.BaseEntity>();

      Mapper.CreateMap<VModel.Item, Model.Item>();
      Mapper.CreateMap<Model.Item, VModel.Item>();

      Mapper.CreateMap<VModel.Scale, Model.Scale>();
      Mapper.CreateMap<Model.Scale, VModel.Scale>();

      Mapper.CreateMap<VModel.Booking, Model.Booking>();
      Mapper.CreateMap<Model.Booking, VModel.Booking>();

      Mapper.CreateMap<VModel.Container, Model.Container>();
      Mapper.CreateMap<Model.Container, VModel.Container>();

      Mapper.CreateMap<VModel.SalesOrder, Model.SalesOrder>();
      Mapper.CreateMap<Model.SalesOrder, VModel.SalesOrder>();

      Mapper.CreateMap<VModel.Party, Model.Party>();
      Mapper.CreateMap<Model.Party, VModel.Party>();
    }

    public override IEnumerable<VModel.InvoiceItem> GetAllByPagingByParentID(
        out int totalRows,
        int id,
        int page,
        int pageSize,
        string sortColumn,
        string sortType,
        string[] includePredicate = null,
        IList<IFilterDescriptor> filters = null) {
      IEnumerable<Model.ScaleDetails> scaleDetails = _repository.FindByPaging<Model.ScaleDetails>(out totalRows, o => o.Scale.Container_No.Booking.ID == id
                  && o.Active_Ind == true
                  && o.Scale.Active_Ind == true
                  && o.Scale.Ticket_Type == "Shipping Ticket"
                  && o.Scale.Container_No.Active_Ind == true,
                 page, pageSize, sortColumn, sortType, includePredicate, filters);
      return GetInvoiceItems(scaleDetails);
    }

    public IEnumerable<ViewModel.InvoiceItem> GetInvoiceItems(IEnumerable<Model.ScaleDetails> scaleDetails) {
      List<ViewModel.InvoiceItem> busEnumeration = new List<ViewModel.InvoiceItem>();

      foreach (var scaleDetail in scaleDetails) {

        ViewModel.InvoiceItem busEntity = new VModel.InvoiceItem();

        Model.SalesOrderItem soScaleItem = GetScaleSOItem(scaleDetail.Scale.Container_No.Booking.Sales_Order_No.ID, scaleDetail);

        busEntity.Container_No = scaleDetail.Scale.Container_No.Container_No;
        busEntity.Item_Name = scaleDetail.Apply_To_Item.Short_Name;
        busEntity.Seal_No = scaleDetail.Scale.Seal_No;
        busEntity.Net_Weight = scaleDetail.NetWeight;
        busEntity.Price = soScaleItem != null ? soScaleItem.Price : 0;
        busEntity.UOM_SO = soScaleItem != null && !string.IsNullOrWhiteSpace(soScaleItem.Item_UOM) ? soScaleItem.Item_UOM : "LBS";
        busEntity.SO_Item_UOM_NetWeight = GetSOItemUOMWeight(scaleDetail, busEntity.UOM_SO);
        busEntity.Total = decimal.Round(busEntity.Price * busEntity.SO_Item_UOM_NetWeight, 2, MidpointRounding.AwayFromZero);
        busEnumeration.Add(busEntity);
      }
      return busEnumeration;
    }

    public Model.SalesOrderItem GetScaleSOItem(int soId, Model.ScaleDetails scaleDetails) {
      if (scaleDetails == null || scaleDetails.Apply_To_Item == null)
        return new Model.SalesOrderItem();

      Model.SalesOrderItem item = (from soItem in _repository.GetQuery<Model.SalesOrderItem>()
                                   where soItem.SalesOrder.ID == soId
                                       && soItem.Item.ID == scaleDetails.Item_Received.ID
                                   select soItem).FirstOrDefault();
      return item;
    }



    private decimal GetSOItemUOMWeight(Model.ScaleDetails scaleDetail, string uom) {
      if (scaleDetail == null || scaleDetail.Scale == null)
        return 0;

      decimal soItemUOMWeight = scaleDetail.NetWeight;

      if (uom != null && uom.ToLower() != "lbs") {
        UOMConversionLibrary uomConvLib = new UOMConversionLibrary(_dbContextConnectionString);
        VModel.UOMConversion uomConv = uomConvLib.GetByUOM(uom, "LBS");
        if (uomConv != null) {
          soItemUOMWeight = soItemUOMWeight / (decimal) uomConv.Factor;
        }
      }
      soItemUOMWeight = decimal.Round(soItemUOMWeight, 3, MidpointRounding.AwayFromZero);
      return soItemUOMWeight;
    }

    private Model.ScaleDetails GetScaleDetailByContainerId(int containerId) {
      IEnumerable<Model.ScaleDetails> modEnumeration = _repository.Find<Model.ScaleDetails>(o => o.Scale.Container_No.ID == containerId, new string[] { "Scale", "Scale.Container_No.Booking.Sales_Order_No", "Item_Received", "Apply_To_Item" });
      return modEnumeration.FirstOrDefault();
    }

    public override IEnumerable<VModel.InvoiceItem> GetAllByParentID(
        int InvoiceId,
        string[] includePredicate = null) {
      IEnumerable<Model.InvoiceItem> modEnumeration = _repository.Find<Model.InvoiceItem>(o => o.Invoice.ID == InvoiceId, includePredicate);
      IEnumerable<VModel.InvoiceItem> busEnumeration = Map(modEnumeration);

      return busEnumeration;
    }

    public decimal GetTotal(int bookingId, string[] includePredicate = null) {
      IEnumerable<Model.ScaleDetails> scaleDetails = _repository.Find<Model.ScaleDetails>(o => o.Scale.Container_No.Booking.ID == bookingId, includePredicate);
      return GetInvoiceItems(scaleDetails).Sum(c => c.Total);
    }

    public decimal GetTotalAvgCostAmt(int bookingId, string[] includePredicate = null) {
      decimal itemAvgCost = 0;
      decimal totalAvgCostAmount = 0;

      IEnumerable<Model.ScaleDetails> scaleDetails = _repository.Find<Model.ScaleDetails>(o => o.Scale.Container_No.Booking.ID == bookingId, includePredicate);
   //   IEnumerable<VModel.InvoiceItem> invoiceItems = GetInvoiceItems(scaleDetails);
      foreach (var item in scaleDetails) {
        Model.CycleDetails cycleDetails = _repository.Find<Model.CycleDetails>(o => o.Item.ID == item.Apply_To_Item.ID && o.Date <=   DateTime.Now).OrderByDescending(o=> o.Created_Date).FirstOrDefault();
        if (cycleDetails != null) {
          itemAvgCost = cycleDetails.Average_Cost;
          totalAvgCostAmount += GetInvoiceItems(new List<Model.ScaleDetails>() { item }).FirstOrDefault().SO_Item_UOM_NetWeight* itemAvgCost;
        }
      }
      return totalAvgCostAmount;
    }


  }
}
