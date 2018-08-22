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
    public class PartyLibrary : GenericLibrary<VModel.Party, Model.Party>
    {
        public PartyLibrary() : base() { }
        public PartyLibrary(string dbContextConnectionString) : base(dbContextConnectionString) { }


        public virtual IEnumerable<VModel.Party> GetByType(string partyType)
        {
            IEnumerable<Model.Party> modParties = from parties in _repository.GetQuery<Model.Party>()
                                                  where parties.Party_Type.ToLower().Equals(partyType.ToLower())
                                                  select parties;
            IEnumerable<VModel.Party> busParties = Map(modParties);
            return busParties;
        }


        public IEnumerable<VModel.Party> GetByTypeWithPaging(string partyType, out int totalRows, int page, int pageSize, string sortColumn, string sortType, string[] includePredicate = null, IList<IFilterDescriptor> filters = null)
        {

            IEnumerable<Model.Party> modEnumeration = _repository.FindByPaging<Model.Party>(out totalRows, o => o.Party_Type.Equals(partyType, StringComparison.OrdinalIgnoreCase),
                                                                                          page, pageSize, sortColumn, sortType, includePredicate, filters);
            IEnumerable<VModel.Party> busEnumeration = Map(modEnumeration);

            return busEnumeration;
        }

        public IEnumerable<VModel.Party> GetByTypesWithPaging(string partyType1, string partyType2, out int totalRows, int page, int pageSize, string sortColumn, string sortType, string[] includePredicate = null, IList<IFilterDescriptor> filters = null)
        {

            IEnumerable<Model.Party> modEnumeration = _repository.FindByPaging<Model.Party>(out totalRows, o => o.Party_Type.Contains(partyType1) || o.Party_Type.Contains(partyType2),
                                                                                         page, pageSize, sortColumn, sortType, includePredicate, filters);
            IEnumerable<VModel.Party> busEnumeration = Map(modEnumeration);

            return busEnumeration;
        }

        public IEnumerable<VModel.Party> GetPartiesForPaymentsWithPaging(out int totalRows, int page, int pageSize, string sortColumn, string sortType, string[] includePredicate = null, IList<IFilterDescriptor> filters = null)
        {

            IEnumerable<Model.Party> modEnumeration = _repository.FindByPaging<Model.Party>(out totalRows, o => o.Party_Type.Contains("Supplier")
                                                                                             || o.Party_Type.Contains("Broker")
                                                                                             || o.Party_Type.Contains("Trader")
                                                                                             || o.Party_Type.Contains("Industrial"),
                                                                                                page, pageSize, sortColumn, sortType, includePredicate, filters);
            IEnumerable<VModel.Party> busEnumeration = Map(modEnumeration);

            return busEnumeration;
        }

        public IEnumerable<VModel.Party> GetPartiesForReceiptWithPaging(out int totalRows, int page, int pageSize, string sortColumn, string sortType, string[] includePredicate = null, IList<IFilterDescriptor> filters = null)
        {

            IEnumerable<Model.Party> modEnumeration = _repository.FindByPaging<Model.Party>(out totalRows, o => o.Party_Type.Contains("Buyer")                                                                                            
                                                                                             || o.Party_Type.Contains("Trader")
                                                                                             || o.Party_Type.Contains("Broker"),
                                                                                                page, pageSize, sortColumn, sortType, includePredicate, filters);
            IEnumerable<VModel.Party> busEnumeration = Map(modEnumeration);

            return busEnumeration;
        }

        public bool IsDuplicate(ViewModel.Party party)
        {
            IEnumerable<Model.Party> modParties = from parties in _repository.GetQuery<Model.Party>()
                                                  where parties.Party_Name.ToLower() == party.Party_Name.Trim().ToLower() && parties.Party_Type.ToLower() == party.Party_Type.Trim().ToLower() && parties.ID != party.ID && parties.Active_Ind == true && parties.IsActive == true
                                                  select parties;
            IEnumerable<VModel.Party> busParties = Map(modParties);

            return  !( busParties.Count()<1) ;
        }

        public virtual VModel.Party GetByLicenseNo(string licenseNo) {
          IEnumerable<Model.Party> modParties = from parties in _repository.GetQuery<Model.Party>()
                                                where parties.License_No.ToLower().Equals(licenseNo.ToLower()) && parties.Active_Ind== true && parties.IsActive== true
                                                select parties;
          IEnumerable<VModel.Party> busParties = Map(modParties);
          return busParties.FirstOrDefault();
        }

        public override System.Linq.Expressions.Expression<Func<Model.Party, bool>> UniqueEntityExp(Model.Party modelEntity, VModel.Party businessEntity) {
          if (modelEntity.Party_Type == "Individual")
            return m => m.License_No.Equals(modelEntity.License_No, StringComparison.InvariantCultureIgnoreCase)
                                  && m.Party_Type.Equals(modelEntity.Party_Type, StringComparison.InvariantCultureIgnoreCase)
                                  && m.Active_Ind == true
                                  && m.ID != modelEntity.ID;

          else
          return m => m.Party_Name.Equals(modelEntity.Party_Name, StringComparison.InvariantCultureIgnoreCase)
                      && m.Party_Type.Equals(modelEntity.Party_Type, StringComparison.InvariantCultureIgnoreCase)
                      && m.Active_Ind == true
                      && m.ID != modelEntity.ID;
        }


        public virtual IEnumerable<VModel.Party> GetOrgParties() {
          IEnumerable<Model.Party> modParties = from parties in _repository.GetQuery<Model.Party>()
                                                where parties.Party_Type.ToLower().Equals("Organization") && parties.Active_Ind == true && parties.IsActive == true
                                                select parties;
          IEnumerable<VModel.Party> busParties = Map(modParties);
          return busParties;
        }


    }
}
