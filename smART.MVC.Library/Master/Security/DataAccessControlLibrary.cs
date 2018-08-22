using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Model = smART.Model;
using VModel = smART.ViewModel;

using AutoMapper;
using System.Data.Entity;

namespace smART.Library
{

    public class DataAccessControlLibrary : GenericLibrary<VModel.DataAccessControl, Model.DataAccessControl>
    {
        public DataAccessControlLibrary() : base() { }

        public DataAccessControlLibrary(string dbContextConnectionString)
            : base(dbContextConnectionString)
        {
            this.Initialize(dbContextConnectionString);
        }

        public override void Initialize(string dbContextConnectionString)
        {
            base.Initialize(dbContextConnectionString);
            Mapper.CreateMap<VModel.Feature, Model.Feature>();
            Mapper.CreateMap<Model.Feature, VModel.Feature>();           
            Mapper.CreateMap<VModel.Role, Model.Role>();
            Mapper.CreateMap<Model.Role, VModel.Role>();
        }

        //public virtual IEnumerable<VModel.Feature> GetFeaturesForEmployee(int EmployeeID)
        //{
        //    IEnumerable<Model.Feature> modFeatures = from feature in _repository.GetQuery<Model.Feature>()
        //            join rolefeature in _repository.GetQuery<Model.RoleFeature>() 
        //            on feature.ID equals rolefeature.Feature.ID
        //            join employeerole in _repository.GetQuery<Model.EmployeeRole>()
        //            on rolefeature.Role.ID equals employeerole.Role.ID
        //            where (employeerole.Employee.ID == EmployeeID)
        //            select feature;

        //    IEnumerable<VModel.Feature> busFeatures = Mapper.Map<IEnumerable<Model.Feature>, IEnumerable<VModel.Feature>>(modFeatures);
        //    return busFeatures;
        //}
    }
}
