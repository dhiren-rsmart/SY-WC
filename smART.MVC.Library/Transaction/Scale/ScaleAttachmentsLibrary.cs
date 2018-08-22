using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Model = smART.Model;
using VModel = smART.ViewModel;
using AutoMapper;
using System.Data.Entity;

using Telerik.Web.Mvc;

namespace smART.Library
{

    public class ScaleAttachmentsLibrary : AttachmentLibrary<VModel.ScaleAttachments, Model.ScaleAttachments, VModel.Scale, Model.Scale>
    {
        public ScaleAttachmentsLibrary()
            : base()
        {
        }
        public ScaleAttachmentsLibrary(string dbContextConnectionString)
            : base(dbContextConnectionString)
        {
            Initialize(dbContextConnectionString);
        }

        public override void Initialize(string dbContextConnectionString)
        {
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

        public override VModel.ScaleAttachments Add(VModel.ScaleAttachments addObject)
        {
            VModel.ScaleAttachments insertedObjectBusiness = addObject;
            try
            {
                Model.ScaleAttachments newModObject = Mapper.Map<VModel.ScaleAttachments, Model.ScaleAttachments>(addObject);
                newModObject.Parent = _repository.GetQuery<Model.Scale>().SingleOrDefault(o => o.ID == addObject.Parent.ID);

                Model.ScaleAttachments insertedObject = _repository.Add<Model.ScaleAttachments>(newModObject);
                _repository.SaveChanges();

                insertedObjectBusiness = Mapper.Map<Model.ScaleAttachments, VModel.ScaleAttachments>(insertedObject);
            }
            catch (Exception ex)
            {
                bool rethrow;
                rethrow = LibraryExceptionHandler.HandleException(ref ex, insertedObjectBusiness.Updated_By, insertedObjectBusiness.GetType().Name, insertedObjectBusiness.ID.ToString());
                if (rethrow)
                    throw ex;
            }
            return insertedObjectBusiness;
        }

        public VModel.ScaleAttachments GetScaleAttachmentByRefId(Guid refId)
        {
            return GetSingleByCriteria(o => o.Document_RefId == refId && o.Active_Ind == true);
        }

        public IEnumerable<VModel.ScaleAttachments> GetAttachmentsWithPagingByRefIdAndRefType(
           Common.EnumAttachmentRefType refType,
           int refId,
           int parentId,
           out int totalRows,
           int page,
           int pageSize,
           string sortColumn,
           string sortType,
           string[] includePredicate = null,
           IList<IFilterDescriptor> filters = null)
        {
            IEnumerable<Model.ScaleAttachments> modEnumeration = _repository.FindByPaging<Model.ScaleAttachments>(out totalRows, o => o.Ref_ID == refId && o.Ref_Type == (int)refType && o.Parent.ID == parentId
              , page, pageSize, sortColumn, sortType, includePredicate, filters);
            IEnumerable<VModel.ScaleAttachments> busEnumeration = Map(modEnumeration);

            return busEnumeration;

        }

        public IEnumerable<VModel.ScaleAttachments> GetAllByPagingByParentID(
          out int totalRows,
          int id,
          int page,
          int pageSize,
          string sortColumn,
          string sortType,
          string[] includePredicate = null,
          IList<IFilterDescriptor> filters = null)
        {
            IEnumerable<Model.ScaleAttachments> modEnumeration = _repository.FindByPaging<Model.ScaleAttachments>(out totalRows, o => o.Parent.ID == id && o.Ref_Type != (int)Common.EnumAttachmentRefType.Item, page, pageSize, sortColumn, sortType, includePredicate, filters);
            IEnumerable<VModel.ScaleAttachments> busEnumeration = Map(modEnumeration);

            return busEnumeration;

        }

        public IEnumerable<VModel.ScaleAttachments> GetAttachmentsByRefIdAndRefTypeAndParentId(
        Common.EnumAttachmentRefType refType,
        int refId,
        int parentId,
        string[] includePredicate = null
       )
        {
            return GetByExpression(o => o.Ref_ID == refId && o.Ref_Type == (int)refType && o.Parent.ID == parentId, includePredicate);
        }

        public IEnumerable<VModel.ScaleAttachments> GetAttachmentsByRefTypeAndParentId(
      Common.EnumAttachmentRefType refType,      
      int parentId,
      string[] includePredicate = null
     )
        {
            return GetByExpression(o => o.Ref_Type == (int)refType && o.Parent.ID == parentId, includePredicate);
        }
    }
}
