using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Model = smART.Model;
using VModel = smART.ViewModel;

using AutoMapper;
using System.Data.Entity;
using Telerik.Web.Mvc;
using System.Linq.Expressions;


namespace smART.Library
{
    public class BankLibrary : PartyChildLibrary<VModel.Bank, Model.Bank>
    {
        public BankLibrary() : base() { }
        public BankLibrary(string dbContextConnectionString) : base(dbContextConnectionString) { }

        public override void Initialize(string dbContextConnectionString) {
          base.Initialize(dbContextConnectionString);

          Mapper.CreateMap<VModel.Party, Model.Party>();
          Mapper.CreateMap<Model.Party, VModel.Party>();

          Mapper.CreateMap<VModel.Bank, Model.Bank>();
          Mapper.CreateMap<Model.Bank, VModel.Bank>();
        }


        public IEnumerable<VModel.Bank> GetByPartyTypeWithPaging(string partyType, out int totalRows, int page, int pageSize, string sortColumn, string sortType, string[] includePredicate = null, IList<IFilterDescriptor> filters = null)
        {

            IEnumerable<Model.Bank> modEnumeration = _repository.FindByPaging<Model.Bank>(out totalRows, o => o.Party.Party_Type.Equals(partyType, StringComparison.OrdinalIgnoreCase),
                                                                                          page, pageSize, sortColumn, sortType, includePredicate, filters);
            IEnumerable<VModel.Bank> busEnumeration = Map(modEnumeration);

            return busEnumeration;
        }


        public IEnumerable<VModel.Bank> GetOrganizationBank() {
          IEnumerable<Model.Bank> modEnumeration = _repository.GetAll<Model.Bank>();
          IEnumerable<VModel.Bank> busEnumeration = Map(modEnumeration);
          return busEnumeration;// .Where(o => o.Party.Party_Type == "Organization");
         }

        public override System.Linq.Expressions.Expression<Func<Model.Bank, bool>> UniqueEntityExp(Model.Bank modelEntity, VModel.Bank businessEntity) {
          return m => m.Account_No.Equals(modelEntity.Account_No, StringComparison.InvariantCultureIgnoreCase)
                      && m.Party.ID == modelEntity.Party.ID
                      && m.Active_Ind == true
                      && m.ID != modelEntity.ID;
        }

        public override IEnumerable<VModel.Bank> GetAll(string[] includePredicate = null) {
          try {
            IEnumerable<Model.Bank> modEnumeration = _repository.GetAll<Model.Bank>(includePredicate);
            IEnumerable<VModel.Bank> busEnumeration = Mapper.Map<IEnumerable<Model.Bank>, IEnumerable<VModel.Bank>>(modEnumeration);

            return busEnumeration;
          }
          catch (Exception ex) {
            bool rethrow = LibraryExceptionHandler.HandleException(ref ex, System.Diagnostics.TraceEventType.Error);
            if (rethrow) {
              throw ex;
            }
            return null;
          }
        }
      
        public  void UpdateBalance(Expression<Func<Model.Bank, bool>> predicate,VModel.Bank modObject, string[] includePredicate = null) {
          try {
            Model.Bank newModObject = Mapper.Map<VModel.Bank, Model.Bank>(modObject);
            _repository.Modify<Model.Bank>(predicate, newModObject, includePredicate);
            _repository.SaveChanges();
          }
          catch (Exception ex) {
            bool rethrow;
            rethrow = LibraryExceptionHandler.HandleException(ref ex, modObject.Updated_By, modObject.GetType().Name, modObject.ID.ToString());
            if (rethrow)
              throw ex;
          }
        }

    }
}
