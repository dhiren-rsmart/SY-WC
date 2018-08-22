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

  public class InvoiceBrokerageLibrary : InvoiceChildLibrary<VModel.InvoiceItem, Model.InvoiceItem> {

    public InvoiceBrokerageLibrary() : base() { }
    public InvoiceBrokerageLibrary(string dbContextConnectionString) : base(dbContextConnectionString) { }

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
      IEnumerable<Model.Container> containers = _repository.FindByPaging<Model.Container>(out totalRows, o => o.Booking.ID == id
                  && o.Active_Ind == true
                  && o.Booking.Active_Ind == true,
                 page, pageSize, sortColumn, sortType, includePredicate, filters);
      return GetInvoiceItems(containers);
    }

    public IEnumerable<ViewModel.InvoiceItem> GetInvoiceItems(IEnumerable<Model.Container> containers) {
      List<ViewModel.InvoiceItem> busEnumeration = new List<ViewModel.InvoiceItem>();
      if (containers.Count() == 0)
        return busEnumeration;
      Model.SalesOrderItem soItem = GetSOItem(containers.FirstOrDefault().Booking.Sales_Order_No.ID);
      if (soItem != null && soItem.Item!= null) {
        foreach (var container in containers) {

          ViewModel.InvoiceItem busEntity = new VModel.InvoiceItem();

          busEntity.Container_No = container.Container_No;
          busEntity.Item_Name = soItem.Item.Short_Name;
          busEntity.Seal_No = container.Seal1_No;
          busEntity.Net_Weight = container.Net_Weight;
          busEntity.Price = soItem != null ? soItem.Price : 0;
          busEntity.UOM_SO = soItem != null && !string.IsNullOrWhiteSpace(soItem.Item_UOM) ? soItem.Item_UOM : "LBS";
          busEntity.SO_Item_UOM_NetWeight = GetSOItemUOMWeight(container, busEntity.UOM_SO);
          busEntity.Total = decimal.Round(busEntity.Price * busEntity.SO_Item_UOM_NetWeight, 2, MidpointRounding.AwayFromZero);
          busEnumeration.Add(busEntity);
        }
      }
      return busEnumeration;
    }

    public Model.SalesOrderItem GetSOItem(int soId) {
      Model.SalesOrderItem item = (from soItem in _repository.GetQuery<Model.SalesOrderItem>().Include("Item")
                                   where soItem.SalesOrder.ID == soId && soItem.Active_Ind == true
                                   select soItem).FirstOrDefault();
      return item;
    }



    private decimal GetSOItemUOMWeight(Model.Container container, string uom) {

      decimal soItemUOMWeight = container.Net_Weight;

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


    public override IEnumerable<VModel.InvoiceItem> GetAllByParentID(int InvoiceId, string[] includePredicate = null) {
      IEnumerable<Model.InvoiceItem> modEnumeration = _repository.Find<Model.InvoiceItem>(o => o.Invoice.ID == InvoiceId, includePredicate);
      IEnumerable<VModel.InvoiceItem> busEnumeration = Map(modEnumeration);
      return busEnumeration;
    }

    public decimal GetTotal(int bookingId, string[] includePredicate = null) {
      IEnumerable<Model.Container> containers = _repository.Find<Model.Container>(o => o.Booking.ID == bookingId
                                                                                   && o.Booking.Active_Ind == true
                                                                                   && o.Active_Ind == true, includePredicate
                                                                                 );
      return GetInvoiceItems(containers).Sum(c => c.Total);
    }

  }
}
