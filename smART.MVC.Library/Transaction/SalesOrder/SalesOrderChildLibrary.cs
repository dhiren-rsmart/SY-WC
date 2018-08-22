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
    public abstract class SalesOrderChildLibrary<TEntityBusiness, TEntityModel> : GenericLibrary<TEntityBusiness, TEntityModel>, ISalesOrderChildLibrary<TEntityBusiness>
        where TEntityBusiness : VModel.SalesOrderChildEntity, new()
        where TEntityModel : Model.SalesOrderChildEntity, new()
    {
        public SalesOrderChildLibrary() : base() { }
        public SalesOrderChildLibrary(string dbContextConnectionString) : base(dbContextConnectionString) { }

        public IEnumerable<TEntityBusiness> GetAllByPagingBySalesOrderID(
            out int totalRows,
            int id,
            int page,
            int pageSize,
            string sortColumn,
            string sortType,
            string[] includePredicate = null,
            IList<IFilterDescriptor> filters = null)
        {
            IEnumerable<TEntityModel> modEnumeration = _repository.FindByPaging<TEntityModel>(out totalRows, o => o.SalesOrder.ID == id, page, pageSize, sortColumn, sortType, includePredicate, filters);
            IEnumerable<TEntityBusiness> busEnumeration = Map(modEnumeration);

            return busEnumeration;

        }

        public IEnumerable<TEntityBusiness> GetAllBySalesOrderID(
            int SalesOrderId,
            string[] includePredicate = null)
        {
            IEnumerable<TEntityModel> modEnumeration = _repository.Find<TEntityModel>(o => o.SalesOrder.ID == SalesOrderId, includePredicate);
            IEnumerable<TEntityBusiness> busEnumeration = Map(modEnumeration);

            return busEnumeration;
        }
    }
}
