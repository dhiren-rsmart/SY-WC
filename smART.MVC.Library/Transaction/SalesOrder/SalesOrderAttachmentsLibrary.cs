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
    public class SalesOrderAttachmentsLibrary: AttachmentLibrary<VModel.SalesOrderAttachments, Model.SalesOrderAttachments, VModel.SalesOrder, Model.SalesOrder>
    {
        public SalesOrderAttachmentsLibrary() : base() { }
        public SalesOrderAttachmentsLibrary(string dbContextConnectionString) : base(dbContextConnectionString) { }

        public override void Initialize(string dbContextConnectionString)
        {
            base.Initialize(dbContextConnectionString);
            Mapper.CreateMap<VModel.SalesOrder, Model.SalesOrder>();
            Mapper.CreateMap<Model.SalesOrder, VModel.SalesOrder>();
            Mapper.CreateMap<VModel.BaseAttachment, Model.BaseAttachment>();
            Mapper.CreateMap<Model.BaseAttachment, VModel.BaseAttachment>();
        }

    }

}
