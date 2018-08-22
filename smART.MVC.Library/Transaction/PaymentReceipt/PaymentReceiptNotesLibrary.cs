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

  public class PaymentReceiptNotesLibrary : NotesLibrary<VModel.PaymentReceiptNotes, Model.PaymentReceiptNotes, VModel.PaymentReceipt, Model.PaymentReceipt> {
    public PaymentReceiptNotesLibrary()
      : base() {
    }
    public PaymentReceiptNotesLibrary(string dbContextConnectionString)
      : base(dbContextConnectionString) {
    }

    public override void Initialize(string dbContextConnectionString) {
      base.Initialize(dbContextConnectionString);

      Mapper.CreateMap<VModel.Party, Model.Party>();
      Mapper.CreateMap<Model.Party, VModel.Party>();

      Mapper.CreateMap<VModel.Bank, Model.Bank>();
      Mapper.CreateMap<Model.Bank, VModel.Bank>();

    }

    public override VModel.PaymentReceiptNotes Add(VModel.PaymentReceiptNotes addObject) {
      try {
        Model.PaymentReceiptNotes newModObject = Mapper.Map<VModel.PaymentReceiptNotes, Model.PaymentReceiptNotes>(addObject);
        newModObject.Parent = _repository.GetQuery<Model.PaymentReceipt>().SingleOrDefault(o => o.ID == addObject.Parent.ID);

        Model.PaymentReceiptNotes insertedObject = _repository.Add<Model.PaymentReceiptNotes>(newModObject);
        _repository.SaveChanges();

        VModel.PaymentReceiptNotes insertedObjectBusiness = Mapper.Map<Model.PaymentReceiptNotes, VModel.PaymentReceiptNotes>(insertedObject);
        return insertedObjectBusiness;
      }
      catch (Exception ex) {
        bool rethrow;
        rethrow = LibraryExceptionHandler.HandleException(ref ex, addObject.Updated_By, addObject.GetType().Name, addObject.ID.ToString());
        if (rethrow)
          throw ex;
        return null;
      }
    }

  }

}
