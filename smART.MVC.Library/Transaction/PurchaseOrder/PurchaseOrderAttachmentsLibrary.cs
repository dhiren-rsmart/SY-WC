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
    public class PurchaseOrderAttachmentsLibrary: AttachmentLibrary<VModel.PurchaseOrderAttachments, Model.PurchaseOrderAttachments, VModel.PurchaseOrder, Model.PurchaseOrder>
    {
        public PurchaseOrderAttachmentsLibrary() : base() { }
        public PurchaseOrderAttachmentsLibrary(string dbContextConnectionString) : base(dbContextConnectionString) { }

        public override void Initialize(string dbContextConnectionString)
        {
            base.Initialize(dbContextConnectionString);
            Mapper.CreateMap<VModel.PurchaseOrder, Model.PurchaseOrder>();
            Mapper.CreateMap<Model.PurchaseOrder, VModel.PurchaseOrder>();
            Mapper.CreateMap<VModel.BaseAttachment, Model.BaseAttachment>();
            Mapper.CreateMap<Model.BaseAttachment, VModel.BaseAttachment>();

            Mapper.CreateMap<VModel.Party, Model.Party>();
            Mapper.CreateMap<Model.Party, VModel.Party>();

            Mapper.CreateMap<VModel.Contact, Model.Contact>();
            Mapper.CreateMap<Model.Contact, VModel.Contact>();

            Mapper.CreateMap<VModel.PriceList, Model.PriceList>();
            Mapper.CreateMap<Model.PriceList, VModel.PriceList>();
            
        }

    }

}
