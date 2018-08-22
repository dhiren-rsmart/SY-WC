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

  public class ScaleIDCardAttachmentsLibrary : AttachmentLibrary<VModel.ScaleIDCardAttachments, Model.ScaleIDCardAttachments, VModel.Scale, Model.Scale> {
    public ScaleIDCardAttachmentsLibrary()
      : base() {
    }
    public ScaleIDCardAttachmentsLibrary(string dbContextConnectionString)
      : base(dbContextConnectionString) {
      Initialize(dbContextConnectionString);
    }

    public override void Initialize(string dbContextConnectionString) {
      base.Initialize(dbContextConnectionString);

      Mapper.CreateMap<VModel.SalesOrder, Model.SalesOrder>();
      Mapper.CreateMap<Model.SalesOrder, VModel.SalesOrder>();

      Mapper.CreateMap<VModel.Booking, Model.Booking>();
      Mapper.CreateMap<Model.Booking, VModel.Booking>();

      Mapper.CreateMap<VModel.BaseAttachment, Model.BaseAttachment>();
      Mapper.CreateMap<Model.BaseAttachment, VModel.BaseAttachment>();

      //Add all done for Scale Library as well
      Mapper.CreateMap<VModel.DispatcherRequest, Model.DispatcherRequest>();
      Mapper.CreateMap<Model.DispatcherRequest, VModel.DispatcherRequest>();

      Mapper.CreateMap<VModel.Party, Model.Party>();
      Mapper.CreateMap<Model.Party, VModel.Party>();

      Mapper.CreateMap<VModel.Container, Model.Container>();
      Mapper.CreateMap<Model.Container, VModel.Container>();

      //AutoMapper.Mapper.CreateMap<Model.Scale, VModel.Scale>()
      //    .ForMember(dest => dest.Container_No, opt => opt.NullSubstitute("N/A"));

      Mapper.CreateMap<Model.Scale, VModel.Scale>()
          .ForMember(d => d.Container_No, o => o.NullSubstitute(new VModel.Container()))
          .ForMember(d => d.Party_ID, o => o.NullSubstitute(new VModel.Party()))
          .ForMember(d => d.Dispatch_Request_No, o => o.NullSubstitute(new VModel.DispatcherRequest()));


      Mapper.CreateMap<VModel.PurchaseOrder, Model.PurchaseOrder>();
      Mapper.CreateMap<Model.PurchaseOrder, VModel.PurchaseOrder>();

      Mapper.CreateMap<VModel.Booking, Model.Booking>();
      Mapper.CreateMap<Model.Booking, VModel.Booking>();

      Mapper.CreateMap<VModel.SalesOrder, Model.SalesOrder>();
      Mapper.CreateMap<Model.SalesOrder, VModel.SalesOrder>();

      Mapper.CreateMap<VModel.Item, Model.Item>();
      Mapper.CreateMap<Model.Item, VModel.Item>();

      Mapper.CreateMap<VModel.Scale, Model.Scale>();
      Mapper.CreateMap<Model.Scale, VModel.Scale>();

      Mapper.CreateMap<VModel.AddressBook, Model.AddressBook>();
      Mapper.CreateMap<Model.AddressBook, VModel.AddressBook>();

      Mapper.CreateMap<VModel.Contact, Model.Contact>();
      Mapper.CreateMap<Model.Contact, VModel.Contact>();

      Mapper.CreateMap<VModel.PriceList, Model.PriceList>();
      Mapper.CreateMap<Model.PriceList, VModel.PriceList>();
    }

    public override VModel.ScaleIDCardAttachments Add(VModel.ScaleIDCardAttachments addObject) {
      VModel.ScaleIDCardAttachments insertedObjectBusiness = addObject;
      try {
        Model.ScaleIDCardAttachments newModObject = Mapper.Map<VModel.ScaleIDCardAttachments, Model.ScaleIDCardAttachments>(addObject);
        newModObject.Parent = _repository.GetQuery<Model.Scale>().SingleOrDefault(o => o.ID == addObject.Parent.ID);

        Model.ScaleIDCardAttachments insertedObject = _repository.Add<Model.ScaleIDCardAttachments>(newModObject);
        _repository.SaveChanges();

        insertedObjectBusiness = Mapper.Map<Model.ScaleIDCardAttachments, VModel.ScaleIDCardAttachments>(insertedObject);
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
