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
 
    public class BookingAttachmentsLibrary : AttachmentLibrary<VModel.BookingAttachments, Model.BookingAttachments, VModel.Booking, Model.Booking>
    {
        public BookingAttachmentsLibrary() : base() { }
        public BookingAttachmentsLibrary(string dbContextConnectionString) : base(dbContextConnectionString) { }

        public override void Initialize(string dbContextConnectionString)
        {
            base.Initialize(dbContextConnectionString);
            Mapper.CreateMap<VModel.Booking, Model.Booking>();
            Mapper.CreateMap<Model.Booking, VModel.Booking>();

            Mapper.CreateMap<VModel.BaseAttachment, Model.BaseAttachment>();
            Mapper.CreateMap<Model.BaseAttachment, VModel.BaseAttachment>();
        }

    }
}
