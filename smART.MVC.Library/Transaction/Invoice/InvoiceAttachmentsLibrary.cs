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
    public class InvoiceAttachmentsLibrary: AttachmentLibrary<VModel.InvoiceAttachments, Model.InvoiceAttachments, VModel.Invoice, Model.Invoice>
    {
        public InvoiceAttachmentsLibrary() : base() { }
        public InvoiceAttachmentsLibrary(string dbContextConnectionString) : base(dbContextConnectionString) { }

        public override void Initialize(string dbContextConnectionString)
        {
            base.Initialize(dbContextConnectionString);
            Mapper.CreateMap<VModel.Invoice, Model.Invoice>();
            Mapper.CreateMap<Model.Invoice, VModel.Invoice>();
            Mapper.CreateMap<VModel.BaseAttachment, Model.BaseAttachment>();
            Mapper.CreateMap<Model.BaseAttachment, VModel.BaseAttachment>();
        }

    }

}
