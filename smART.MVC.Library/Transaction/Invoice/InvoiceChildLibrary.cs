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
    public abstract class InvoiceChildLibrary<TEntityBusiness, TEntityModel> : GenericLibrary<TEntityBusiness, TEntityModel>, IParentChildLibrary<TEntityBusiness>
        where TEntityBusiness : VModel.InvoiceChildEntity, new()
        where TEntityModel : Model.InvoiceChildEntity, new()
    {
        public InvoiceChildLibrary() : base() { }
        public InvoiceChildLibrary(string dbContextConnectionString) : base(dbContextConnectionString) { }

        public virtual IEnumerable<TEntityBusiness> GetAllByPagingByParentID(
            out int totalRows,
            int id,
            int page,
            int pageSize,
            string sortColumn,
            string sortType,
            string[] includePredicate = null,
            IList<IFilterDescriptor> filters = null)
        {
            IEnumerable<TEntityModel> modEnumeration = _repository.FindByPaging<TEntityModel>(out totalRows, o => o.Invoice.ID == id, page, pageSize, sortColumn, sortType, includePredicate, filters);
            IEnumerable<TEntityBusiness> busEnumeration = Map(modEnumeration);
            return busEnumeration;
        }

        public virtual IEnumerable<TEntityBusiness> GetAllByParentID(
            int InvoiceId,
            string[] includePredicate = null)
        {
            IEnumerable<TEntityModel> modEnumeration = _repository.Find<TEntityModel>(o => o.Invoice.ID == InvoiceId, includePredicate);
            IEnumerable<TEntityBusiness> busEnumeration = Map(modEnumeration);
            return busEnumeration;
        }
    }
}
