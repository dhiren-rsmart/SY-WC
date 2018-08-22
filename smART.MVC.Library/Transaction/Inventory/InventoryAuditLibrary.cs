// Copyright 2011, 2012 RecyclesmART, Inc. All rights reserved
// Main Author: Sanjeev Khanna
// Last Major Update: 07/01/2012

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AutoMapper;
using System.Data.Entity;
using Telerik.Web.Mvc;

using Model = smART.Model;
using VModel = smART.ViewModel;

namespace smART.Library {

  public class InventoryAuditLibrary : GenericLibrary<VModel.InventoryAudit, Model.InventoryAudit>, IParentChildLibrary<VModel.InventoryAudit> {
    public InventoryAuditLibrary()
      : base() {
    }
    public InventoryAuditLibrary(string dbContextConnectionString)
      : base(dbContextConnectionString) {
    }


    public override void Initialize(string dbContextConnectionString) {
      base.Initialize(dbContextConnectionString);

      Mapper.CreateMap<VModel.Item, Model.Item>();
      Mapper.CreateMap<Model.Item, VModel.Item>();
   
    }

    public IEnumerable<VModel.InventoryAudit> GetAllByPagingByParentID(
          out int totalRows,
          int id,
          int page,
          int pageSize,
          string sortColumn,
          string sortType,
          string[] includePredicate = null,
          IList<IFilterDescriptor> filters = null) {
            IEnumerable<Model.InventoryAudit> modEnumeration = _repository.FindByPaging<Model.InventoryAudit>(out totalRows, o => o.Item.ID == id, page, pageSize, sortColumn, sortType, includePredicate, filters);
            IEnumerable<VModel.InventoryAudit> busEnumeration = Map(modEnumeration);

      return busEnumeration;
    }

    public IEnumerable<VModel.InventoryAudit> GetAllByParentID(int parentId, string[] includePredicate = null) {
      IEnumerable<Model.InventoryAudit> modEnumeration = _repository.Find<Model.InventoryAudit>(o => o.Item.ID == parentId, includePredicate);
      IEnumerable<VModel.InventoryAudit> busEnumeration = Map(modEnumeration);

      return busEnumeration;
    }

  }
}
