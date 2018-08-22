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

  public class AssetAttachmentsLibrary : AttachmentLibrary<VModel.AssetAttachments, Model.AssetAttachments, VModel.Asset, Model.Asset> {
    public AssetAttachmentsLibrary() : base() { }
    public AssetAttachmentsLibrary(string dbContextConnectionString) : base(dbContextConnectionString) { }

    public override void Initialize(string dbContextConnectionString) {
      base.Initialize(dbContextConnectionString);
      Mapper.CreateMap<VModel.Asset, Model.Asset>();
      Mapper.CreateMap<Model.Asset, VModel.Asset>();

      Mapper.CreateMap<VModel.BaseAttachment, Model.BaseAttachment>();
      Mapper.CreateMap<Model.BaseAttachment, VModel.BaseAttachment>();
    }

  }
}
