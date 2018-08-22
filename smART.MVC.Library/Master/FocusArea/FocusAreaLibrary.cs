using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AutoMapper;
using System.Data.Entity;
using Telerik.Web.Mvc;

using Model = smART.Model;
using VModel = smART.ViewModel;
using System.Data;
using smART.Model;

namespace smART.Library {

  public class FocusAreaLibrary : GenericLibrary<VModel.FocusArea, Model.FocusArea> {

    ITableRepository _dbTableContext;

    public FocusAreaLibrary(string dbContextConnectionString)
      : base(dbContextConnectionString) {
      _dbTableContext = new DataTableRepository(dbContextConnectionString);
    }

    public override System.Linq.Expressions.Expression<Func<Model.FocusArea, bool>> UniqueEntityExp(Model.FocusArea modelEntity, VModel.FocusArea businessEntity) {
      return m => m.View_Name.Equals(modelEntity.View_Name, StringComparison.InvariantCultureIgnoreCase)
                  && m.Active_Ind == true
                  && m.ID != modelEntity.ID;
    }

    public VModel.FocusArea GetByViewName(string viewName) {
      IEnumerable<Model.FocusArea> modEnumeration = _repository.Find<FocusArea>(o => o.View_Name == viewName);
      IEnumerable<VModel.FocusArea> busEnumeration = Map(modEnumeration);
      return busEnumeration.FirstOrDefault();
    }

    public IEnumerable< VModel.FocusArea> GetByType(string type) {
      IEnumerable<Model.FocusArea> modEnumeration = _repository.Find<FocusArea>(o => o.Focus_Area_Type == type);
      IEnumerable<VModel.FocusArea> busEnumeration = Map(modEnumeration);
      return busEnumeration;
    }


    public DataTable GetAllAsDt(string viewName,string orderByClause) {
      return _dbTableContext.GetAllAsDt(viewName, orderByClause);
    }

    public DataTable GetAllWithPagingAsDt(string tableName, out int totalRows, int page, int pageSize, string sortColumn, string sortType, IList<IFilterDescriptor> filters = null) {
      return _dbTableContext.GetAllByPagingAsDt(tableName, out totalRows, page, pageSize, sortColumn, sortType, filters);
    }

  }
}
