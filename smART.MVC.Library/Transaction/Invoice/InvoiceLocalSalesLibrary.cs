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

namespace smART.Library {

  public class InvoiceLocalSalesLibrary : InvoiceChildLibrary<VModel.InvoiceLocalSales, Model.InvoiceItem> {

    public InvoiceLocalSalesLibrary() : base() { }
    public InvoiceLocalSalesLibrary(string dbContextConnectionString) : base(dbContextConnectionString) { }

    public override void Initialize(string dbContextConnectionString) {
      base.Initialize(dbContextConnectionString);
      Mapper.CreateMap<VModel.Invoice, Model.Invoice>();
      Mapper.CreateMap<Model.Invoice, VModel.Invoice>();
      Mapper.CreateMap<VModel.BaseEntity, Model.BaseEntity>();
      Mapper.CreateMap<Model.BaseEntity, VModel.BaseEntity>();
      Mapper.CreateMap<VModel.Item, Model.Item>();
      Mapper.CreateMap<Model.Item, VModel.Item>();
    }

    public override IEnumerable<VModel.InvoiceLocalSales> GetAllByPagingByParentID(
        out int totalRows,
        int id,
        int page,
        int pageSize,
        string sortColumn,
        string sortType,
        string[] includePredicate = null,
        IList<IFilterDescriptor> filters = null) {
      Expression<Func<Model.ScaleDetails, bool>> filterExp = o => o.Scale.Invoice.ID == id;

      IEnumerable<Model.ScaleDetails> scaleDetails = _repository.FindByPaging<Model.ScaleDetails>(out totalRows, filterExp, page, pageSize, sortColumn, sortType, includePredicate, filters);
      return GetInvoiceItems(scaleDetails);
    }

    public  IEnumerable<VModel.InvoiceLocalSales> GetAllByPagingBySalesOrderID(
            out int totalRows,
            int id,
            int page,
            int pageSize,
            string sortColumn,
            string sortType,
            string[] includePredicate = null,
            IList<IFilterDescriptor> filters = null) {

      Expression<Func<Model.ScaleDetails, bool>> filterExp = o => o.Scale.Sales_Order.ID == id && o.Scale.Invoice.ID == null;

      IEnumerable<Model.ScaleDetails> scaleDetails = _repository.FindByPaging<Model.ScaleDetails>(out totalRows, filterExp, page, pageSize, sortColumn, sortType, includePredicate, filters);
      return GetInvoiceItems(scaleDetails);
    }

    public IEnumerable<ViewModel.InvoiceLocalSales> GetInvoiceItems(IEnumerable<Model.ScaleDetails> scaleDetails) {
      List<ViewModel.InvoiceLocalSales> busEnumeration = new List<ViewModel.InvoiceLocalSales>();
      foreach (var item in scaleDetails) {

        Model.SalesOrderItem soScaleItem = GetScaleSOItem(item.Scale.Sales_Order.ID, item);

        ViewModel.InvoiceLocalSales busEntity = new VModel.InvoiceLocalSales();
        busEntity.Ticket_No = item.Scale.ID;
        busEntity.Item_Name = item.Apply_To_Item.Short_Name;
        busEntity.Net_Weight = item.NetWeight;
        busEntity.Price = soScaleItem != null ? soScaleItem.Price : 0;
        busEntity.UOM_SO = soScaleItem != null && !string.IsNullOrWhiteSpace(soScaleItem.Item_UOM) ? soScaleItem.Item_UOM : "LBS";
        busEntity.SO_Item_UOM_NetWeight = GetSOItemUOMWeight(item, busEntity.UOM_SO);
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
                                       && soItem.Item.ID == scaleDetails.Apply_To_Item.ID
                                   select soItem).FirstOrDefault();
      return item;
    }



    private decimal GetSOItemUOMWeight(Model.ScaleDetails scaleDetail, string uom) {
      if (scaleDetail == null || scaleDetail.Scale == null)
        return 0;

      decimal soItemUOMWeight = scaleDetail.Scale.Net_Weight;

      if (uom != null && uom.ToLower() != "lbs") {
        UOMConversionLibrary uomConvLib = new UOMConversionLibrary(_dbContextConnectionString);
        VModel.UOMConversion uomConv = uomConvLib.GetByUOM(uom, "LBS");
        if (uomConv != null) {
          soItemUOMWeight = soItemUOMWeight / (decimal)uomConv.Factor;
        }
      }
      soItemUOMWeight = decimal.Round(soItemUOMWeight, 3, MidpointRounding.AwayFromZero);
      return soItemUOMWeight;

    }

    private Model.ScaleDetails GetScaleDetailByContainerId(int containerId) {
      IEnumerable<Model.ScaleDetails> modEnumeration = _repository.Find<Model.ScaleDetails>(o => o.Scale.Container_No.ID == containerId, new string[] { "Scale", "Scale.Container_No.Booking.Sales_Order_No", "Item_Received", "Apply_To_Item" });
      return modEnumeration.FirstOrDefault();
    }

    public override IEnumerable<VModel.InvoiceLocalSales> GetAllByParentID(
        int InvoiceId,
        string[] includePredicate = null) {
      throw new NotImplementedException();
    }

    public decimal GetTotal(int soId, string[] includePredicate = null) {
      Expression<Func<Model.ScaleDetails, bool>> filterExp = o => o.Scale.Sales_Order.ID == soId && o.Scale.Invoice.ID == null;
      IEnumerable<Model.ScaleDetails> scaleDetails = _repository.Find<Model.ScaleDetails>(filterExp, includePredicate);
      return GetInvoiceItems(scaleDetails).Sum(c => c.Total);
    }

  }
}
