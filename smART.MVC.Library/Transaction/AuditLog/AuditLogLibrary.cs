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
  public class AuditLogLibrary : GenericLibrary<VModel.AuditLog, Model.AuditLog> {
    public AuditLogLibrary() : base() { }
    public AuditLogLibrary(string dbContextConnectionString) : base(dbContextConnectionString) { }

    public IEnumerable<VModel.AuditLog> GetAuditLogByEntityWithPagging(string entityName, int entityID, out int totalRows, int page, int pageSize, string sortColumn, string sortType, string[] includePredicate = null, IList<IFilterDescriptor> filters = null) {
      IEnumerable<Model.AuditLog> modEnumeration = _repository.FindByPaging<Model.AuditLog>(out totalRows, o => o.Entity_Name.Equals(entityName, StringComparison.OrdinalIgnoreCase)
                                                                                                           && o.Entity_ID == entityID && o.Old_Value!= o.New_Value,
                                                                                             page, pageSize, sortColumn, sortType, includePredicate,
                                                                                             filters
                                                                                            );
      IEnumerable<VModel.AuditLog> busEnumeration = Map(modEnumeration);
      return busEnumeration;
    }

    public IEnumerable<VModel.AuditLog> GetCashAuditLogWithPagging(out int totalRows, int page, int pageSize, string sortColumn, string sortType, string[] includePredicate = null, IList<IFilterDescriptor> filters = null) {
      IEnumerable<Model.AuditLog> modEnumeration = _repository.FindByPaging<Model.AuditLog>(out totalRows, o => o.Entity_Name.Equals("Cash", StringComparison.OrdinalIgnoreCase),
                                                                                             page, pageSize, sortColumn, sortType, includePredicate,
                                                                                             filters
                                                                                            );
      IEnumerable<VModel.AuditLog> busEnumeration = Map(modEnumeration);
      return busEnumeration;
    }
  }
}
