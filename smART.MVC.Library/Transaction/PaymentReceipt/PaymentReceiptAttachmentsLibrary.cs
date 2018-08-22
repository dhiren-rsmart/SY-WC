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

    public class PaymentReceiptAttachmentsLibrary : AttachmentLibrary<VModel.PaymentReceiptAttachments, Model.PaymentReceiptAttachments, VModel.PaymentReceipt, Model.PaymentReceipt>
    {
        public PaymentReceiptAttachmentsLibrary() : base() { }
        public PaymentReceiptAttachmentsLibrary(string dbContextConnectionString) : base(dbContextConnectionString) { }

        public override void Initialize(string dbContextConnectionString)
        {
            base.Initialize(dbContextConnectionString);
            Mapper.CreateMap<VModel.PaymentReceipt, Model.PaymentReceipt>();
            Mapper.CreateMap<Model.PaymentReceipt, VModel.PaymentReceipt>();
            
            Mapper.CreateMap<VModel.BaseAttachment, Model.BaseAttachment>();
            Mapper.CreateMap<Model.BaseAttachment, VModel.BaseAttachment>();
        }

   
    }
}
