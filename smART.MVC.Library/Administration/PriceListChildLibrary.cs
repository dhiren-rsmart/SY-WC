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
    public abstract class PriceListChildLibrary<TEntityBusiness, TEntityModel> : GenericLibrary<TEntityBusiness, TEntityModel>, IParentChildLibrary<TEntityBusiness>
        where TEntityBusiness : VModel.PriceListChildEntity, new()
        where TEntityModel : Model.PriceListChildEntity, new()
    {
        public PriceListChildLibrary() : base() { }
        public PriceListChildLibrary(string dbContextConnectionString) : base(dbContextConnectionString) { }

        public IEnumerable<TEntityBusiness> GetAllByPagingByParentID(
            out int totalRows,
            int id,
            int page,
            int pageSize,
            string sortColumn,
            string sortType,
            string[] includePredicate = null,
            IList<IFilterDescriptor> filters = null)
        {
            IEnumerable<TEntityModel> modEnumeration = _repository.FindByPaging<TEntityModel>(out totalRows, o => o.PriceList.ID == id, page, pageSize, sortColumn, sortType, includePredicate, filters);
            IEnumerable<TEntityBusiness> busEnumeration = Map(modEnumeration);

            return busEnumeration;

        }

        public IEnumerable<TEntityBusiness> GetAllByParentID(
            int PriceListId, 
            string[] includePredicate = null)
        {
            IEnumerable<TEntityModel> modEnumeration = _repository.Find<TEntityModel>(o => o.PriceList.ID == PriceListId, includePredicate);
            IEnumerable<TEntityBusiness> busEnumeration = Map(modEnumeration);

            return busEnumeration;
        }
    }
}
