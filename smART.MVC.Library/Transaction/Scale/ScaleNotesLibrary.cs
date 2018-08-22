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

  public class ScaleNotesLibrary : NotesLibrary<VModel.ScaleNotes, Model.ScaleNotes, VModel.Scale, Model.Scale> {
    public ScaleNotesLibrary()
      : base() {
    }
    public ScaleNotesLibrary(string dbContextConnectionString)
      : base(dbContextConnectionString) {
    }

    public override void Initialize(string dbContextConnectionString) {
      base.Initialize(dbContextConnectionString);
      Mapper.CreateMap<VModel.Scale, Model.Scale>();
      Mapper.CreateMap<Model.Scale, VModel.Scale>();
      Mapper.CreateMap<VModel.BaseNotes, Model.BaseNotes>();
      Mapper.CreateMap<Model.BaseNotes, VModel.BaseNotes>();
      Mapper.CreateMap<VModel.Booking, Model.Booking>();
      Mapper.CreateMap<Model.Booking, VModel.Booking>();
      Mapper.CreateMap<VModel.SalesOrder, Model.SalesOrder>();
      Mapper.CreateMap<Model.SalesOrder, VModel.SalesOrder>();
    }

    public override VModel.ScaleNotes Add(VModel.ScaleNotes addObject) {
      VModel.ScaleNotes insertedObjectBusiness = addObject;
      try {      
        Model.ScaleNotes newModObject = Mapper.Map<VModel.ScaleNotes, Model.ScaleNotes>(addObject);
        newModObject.Parent = _repository.GetQuery<Model.Scale>().SingleOrDefault(o => o.ID == addObject.Parent.ID);

        Model.ScaleNotes insertedObject = _repository.Add<Model.ScaleNotes>(newModObject);
        _repository.SaveChanges();
        insertedObjectBusiness = Mapper.Map<Model.ScaleNotes, VModel.ScaleNotes>(insertedObject);
      }
      catch (Exception ex) {
        bool rethrow;
        rethrow = LibraryExceptionHandler.HandleException(ref ex, insertedObjectBusiness.Updated_By, insertedObjectBusiness.GetType().Name, insertedObjectBusiness.ID.ToString());
        if (rethrow)
          throw ex;
      }
      return insertedObjectBusiness;
    }

  }

}
