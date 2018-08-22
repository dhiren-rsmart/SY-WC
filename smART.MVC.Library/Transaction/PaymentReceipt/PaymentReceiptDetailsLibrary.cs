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
using smART.Common;

namespace smART.Library {

  public class PaymentReceiptDetailsLibrary : GenericLibrary<VModel.PaymentReceiptDetails, Model.PaymentReceiptDetails>, IParentChildLibrary<VModel.PaymentReceiptDetails> {
    public PaymentReceiptDetailsLibrary()
      : base() {
    }
    public PaymentReceiptDetailsLibrary(string dbContextConnectionString)
      : base(dbContextConnectionString) {
    }


    public override void Initialize(string dbContextConnectionString) {
      base.Initialize(dbContextConnectionString);

      Mapper.CreateMap<VModel.PaymentReceipt, Model.PaymentReceipt>();
      Mapper.CreateMap<Model.PaymentReceipt, VModel.PaymentReceipt>();

      Mapper.CreateMap<VModel.Settlement, Model.Settlement>();
      Mapper.CreateMap<Model.Settlement, VModel.Settlement>();

      Mapper.CreateMap<VModel.Invoice, Model.Invoice>();
      Mapper.CreateMap<Model.Invoice, VModel.Invoice>();

      Mapper.CreateMap<VModel.AddressBook, Model.AddressBook>();
      Mapper.CreateMap<Model.AddressBook, VModel.AddressBook>();


      Mapper.CreateMap<VModel.Scale, Model.Scale>();
      Mapper.CreateMap<Model.Scale, VModel.Scale>().ForMember(d => d.Purchase_Order, o => o.NullSubstitute(new VModel.PurchaseOrder()))
          .ForMember(d => d.Party_ID, o => o.NullSubstitute(new VModel.Party()));

      Mapper.CreateMap<VModel.DispatcherRequest, Model.DispatcherRequest>();
      Mapper.CreateMap<Model.DispatcherRequest, VModel.DispatcherRequest>();

      Mapper.CreateMap<VModel.Party, Model.Party>();
      Mapper.CreateMap<Model.Party, VModel.Party>();

      Mapper.CreateMap<VModel.Container, Model.Container>();
      Mapper.CreateMap<Model.Container, VModel.Container>();

      Mapper.CreateMap<VModel.PurchaseOrder, Model.PurchaseOrder>();
      Mapper.CreateMap<Model.PurchaseOrder, VModel.PurchaseOrder>();

      Mapper.CreateMap<VModel.Booking, Model.Booking>();
      Mapper.CreateMap<Model.Booking, VModel.Booking>();

      Mapper.CreateMap<VModel.SalesOrder, Model.SalesOrder>();
      Mapper.CreateMap<Model.SalesOrder, VModel.SalesOrder>();

      Mapper.CreateMap<VModel.Item, Model.Item>();
      Mapper.CreateMap<Model.Item, VModel.Item>();

      Mapper.CreateMap<VModel.Contact, Model.Contact>();
      Mapper.CreateMap<Model.Contact, VModel.Contact>();

      Mapper.CreateMap<VModel.PriceList, Model.PriceList>();
      Mapper.CreateMap<Model.PriceList, VModel.PriceList>();

      Mapper.CreateMap<VModel.Asset, Model.Asset>();
      Mapper.CreateMap<Model.Asset, VModel.Asset>();

      Mapper.CreateMap<VModel.ExpensesRequest, Model.ExpensesRequest>();
      Mapper.CreateMap<Model.ExpensesRequest, VModel.ExpensesRequest>();

    }


    public override VModel.PaymentReceiptDetails Add(VModel.PaymentReceiptDetails addObject) {
      VModel.PaymentReceiptDetails insertedObjectBusiness = addObject;
      try {
        int paymentRecId, settlementId, invoiceId, expReqId;
        paymentRecId = settlementId = invoiceId = expReqId = 0;

        Model.PaymentReceiptDetails newModObject = Mapper.Map<VModel.PaymentReceiptDetails, Model.PaymentReceiptDetails>(addObject);

        if (addObject.PaymentReceipt != null)
          paymentRecId = addObject.PaymentReceipt.ID;

        if (addObject.Settlement != null)
          settlementId = addObject.Settlement.ID;

        if (addObject.Invoice != null)
          invoiceId = addObject.Invoice.ID;

        if (addObject.ExpenseRequest != null)
          expReqId = addObject.ExpenseRequest.ID;

        newModObject.PaymentReceipt = _repository.GetQuery<Model.PaymentReceipt>().SingleOrDefault(o => o.ID == paymentRecId);
        newModObject.Settlement = _repository.GetQuery<Model.Settlement>().SingleOrDefault(o => o.ID == settlementId);
        newModObject.Invoice = _repository.GetQuery<Model.Invoice>().SingleOrDefault(o => o.ID == invoiceId);
        newModObject.ExpenseRequest = _repository.GetQuery<Model.ExpensesRequest>().SingleOrDefault(o => o.ID == expReqId);

        if (Adding(insertedObjectBusiness, newModObject, _dbContext)) {
          Model.PaymentReceiptDetails insertedObject = _repository.Add<Model.PaymentReceiptDetails>(newModObject);
          _repository.SaveChanges();

          insertedObjectBusiness = Mapper.Map<Model.PaymentReceiptDetails, VModel.PaymentReceiptDetails>(insertedObject);

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



    protected override void Modify(Expression<Func<Model.PaymentReceiptDetails, bool>> predicate, VModel.PaymentReceiptDetails modObject, string[] includePredicate = null) {
      try {
        Model.PaymentReceiptDetails newModObject = Mapper.Map<VModel.PaymentReceiptDetails, Model.PaymentReceiptDetails>(modObject);
        if (modObject.PaymentReceipt != null)
          newModObject.PaymentReceipt = _repository.GetQuery<Model.PaymentReceipt>().SingleOrDefault(o => o.ID == modObject.PaymentReceipt.ID);
        if (modObject.Settlement != null)
          newModObject.Settlement = _repository.GetQuery<Model.Settlement>().SingleOrDefault(o => o.ID == modObject.Settlement.ID);
        if (modObject.Invoice != null)
          newModObject.Invoice = _repository.GetQuery<Model.Invoice>().SingleOrDefault(o => o.ID == modObject.Invoice.ID);
        if (modObject.ExpenseRequest != null)
          newModObject.ExpenseRequest = _repository.GetQuery<Model.ExpensesRequest>().SingleOrDefault(o => o.ID == modObject.ExpenseRequest.ID);

        Model.PaymentReceiptDetails oldModObject = _repository.GetQuery<Model.PaymentReceiptDetails>().SingleOrDefault(o => o.ID == modObject.ID);

        if (Modifying(modObject, newModObject, _dbContext)) {
          _repository.Modify<Model.PaymentReceiptDetails>(predicate, newModObject, includePredicate);
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


    public IEnumerable<VModel.PaymentReceiptDetails> GetAllByPagingByParentID(
       out int totalRows,
       int id,
       int page,
       int pageSize,
       string sortColumn,
       string sortType,
       string[] includePredicate = null,
       IList<IFilterDescriptor> filters = null) {
      IEnumerable<Model.PaymentReceiptDetails> modEnumeration = _repository.FindByPaging<Model.PaymentReceiptDetails>(out totalRows, o => o.PaymentReceipt.ID == id, page, pageSize, sortColumn, sortType, includePredicate, filters);
      IEnumerable<VModel.PaymentReceiptDetails> busEnumeration = Map(modEnumeration);     
      return busEnumeration;
    }

    public IEnumerable<VModel.PaymentReceiptDetails> GetAllByParentID(int parentId, string[] includePredicate = null) {
      IEnumerable<Model.PaymentReceiptDetails> modEnumeration = _repository.Find<Model.PaymentReceiptDetails>(o => o.PaymentReceipt.ID == parentId, includePredicate);
      IEnumerable<VModel.PaymentReceiptDetails> busEnumeration = Map(modEnumeration);
      GotMultiple(busEnumeration, modEnumeration, _dbContext);
      return busEnumeration;
    }

    public IEnumerable<VModel.PaymentReceiptDetails> GetReceiptDetailsByPagging(out int totalRows, int page, int pageSize, string sortColumn, string sortType, string[] includePredicate = null, IList<IFilterDescriptor> filters = null) {
      string trasnType = EnumTransactionType.Receipt.ToString();
      IEnumerable<Model.PaymentReceiptDetails> modEnumeration = _repository.FindByPaging<Model.PaymentReceiptDetails>(out totalRows, o => o.PaymentReceipt.Transaction_Type == trasnType,
                                                                                      page, pageSize, sortColumn, sortType, includePredicate, filters);
      IEnumerable<VModel.PaymentReceiptDetails> busEnumeration = Map(modEnumeration);
      return busEnumeration;
    }

    public override void Delete(string id, string[] includePredicate = null) {
      int paymentDetailsId = Convert.ToInt32(id);
      IEnumerable<Model.PaymentReceiptDetails> modEnumeration = _repository.Find<Model.PaymentReceiptDetails>(o => o.ID == paymentDetailsId, new string[] { "Settlement.Scale" });
      IEnumerable<VModel.PaymentReceiptDetails> busEnumeration = Map(modEnumeration);
      string idFieldName = "ID";
      this.Delete(GetModelIDExpression(idFieldName, Convert.ToInt32(id), typeof(int)));
      Deleted(busEnumeration.FirstOrDefault(), modEnumeration.FirstOrDefault(), _dbContext);
    }

    public IEnumerable<VModel.PaymentReceiptDetails> GetQScalePaymentHistoryWithPaging(
      out int totalRows,    
      int page,
      int pageSize,
      string sortColumn,
      string sortType,
      string[] includePredicate = null,
      IList<IFilterDescriptor> filters = null) {    
      IEnumerable<Model.PaymentReceiptDetails> modEnumeration = _repository.FindByPaging<Model.PaymentReceiptDetails>( out totalRows ,p => p.PaymentReceipt.Transaction_Type=="Payment" ,  page, pageSize, sortColumn, sortType, includePredicate, filters);
      IEnumerable<VModel.PaymentReceiptDetails> busEnumeration = Map(modEnumeration);
      return busEnumeration;
    }


    public bool IsPaidTicket(int ticketID, string[] includePredicate = null)
    {
        IEnumerable<Model.PaymentReceiptDetails> modEnumeration = _repository.Find<Model.PaymentReceiptDetails>(p => p.Settlement.Scale.ID == ticketID && p.PaymentReceipt.Transaction_Type == "Payment",  includePredicate);
        return modEnumeration != null && modEnumeration.Count()>0;
    }
  }
}
