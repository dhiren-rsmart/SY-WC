using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Model = smART.Model;
using VModel = smART.ViewModel;

using AutoMapper;
using System.Data.Entity;

namespace smART.Library {
  public class EmployeeLibrary : GenericLibrary<VModel.Employee, Model.Employee> {
    public EmployeeLibrary() : base() {
    }
    public EmployeeLibrary(string dbContextConnectionString)
      : base(dbContextConnectionString) {
      this.Initialize(dbContextConnectionString);
    }

    public override void Initialize(string dbContextConnectionString) {
      base.Initialize(dbContextConnectionString);
      Mapper.CreateMap<VModel.Feature, Model.Feature>();
      Mapper.CreateMap<Model.Feature, VModel.Feature>();
      Mapper.CreateMap<VModel.RoleFeature, Model.RoleFeature>();
      Mapper.CreateMap<Model.RoleFeature, VModel.RoleFeature>();
      Mapper.CreateMap<VModel.Role, Model.Role>();
      Mapper.CreateMap<Model.Role, VModel.Role>();
    }

    public virtual IEnumerable<VModel.Feature> GetFeaturesForEmployee(int EmployeeID) {
      IEnumerable<Model.Feature> modFeatures = from feature in _repository.GetQuery<Model.Feature>()
                                               join rolefeature in _repository.GetQuery<Model.RoleFeature>()
                                               on feature.ID equals rolefeature.Feature.ID
                                               join employeerole in _repository.GetQuery<Model.EmployeeRole>()
                                               on rolefeature.Role.ID equals employeerole.Role.ID
                                               where (employeerole.Employee.ID == EmployeeID && feature.Active_Ind==true)
                                               select feature;

      IEnumerable<VModel.Feature> busFeatures = Mapper.Map<IEnumerable<Model.Feature>, IEnumerable<VModel.Feature>>(modFeatures);
      return busFeatures;
    }

    public virtual IEnumerable<VModel.RoleFeature> GetRoleFeaturesForEmployee(int EmployeeID) {
      IEnumerable<Model.RoleFeature> modFeatures = (from rolefeature in _repository.GetQuery<Model.RoleFeature>()
                                                    join employeerole in _repository.GetQuery<Model.EmployeeRole>()
                                                    on rolefeature.Role.ID equals employeerole.Role.ID
                                                    where (employeerole.Employee.ID == EmployeeID && rolefeature.Active_Ind == true)
                                                    select rolefeature).Include("Feature").Include("Role");

      IEnumerable<VModel.RoleFeature> busFeatures = Mapper.Map<IEnumerable<Model.RoleFeature>, IEnumerable<VModel.RoleFeature>>(modFeatures);
      return busFeatures;
    }

    public virtual IEnumerable<VModel.Role> GetRolesForEmployee(int EmployeeID) {
      IEnumerable<Model.Role> modRoles = (from role in _repository.GetQuery<Model.Role>()
                                          join employeerole in _repository.GetQuery<Model.EmployeeRole>()
                                          on role.ID equals employeerole.Role.ID
                                          where (employeerole.Employee.ID == EmployeeID && role.Active_Ind== true)
                                          select role);

      IEnumerable<VModel.Role> busRoles = Mapper.Map<IEnumerable<Model.Role>, IEnumerable<VModel.Role>>(modRoles);
      return busRoles;
    }

    public virtual VModel.Employee GetEmployeeByUsername(string Username) {
      Model.Employee modEmployee = _repository.GetQuery<Model.Employee>().SingleOrDefault(o => o.User_ID.Equals(Username) && o.Active_Ind.Equals(true));
      if (modEmployee == null) return null;
      VModel.Employee busEmployee = Mapper.Map<Model.Employee, VModel.Employee>(modEmployee);
      return busEmployee;
    }

    public virtual bool SaveEmployeeRoles(int employeeID, int[] roleIDs) {
      bool retVal = true;
      try {
        _repository.Delete<Model.EmployeeRole>(m => m.Employee.ID == employeeID);

        Model.Employee emp = _repository.GetQuery<Model.Employee>().SingleOrDefault(m => m.ID == employeeID);

        if (roleIDs != null) {
          foreach (int roleID in roleIDs) {
            Model.Role role = _repository.GetQuery<Model.Role>().SingleOrDefault(m => m.ID == roleID);
            Model.EmployeeRole empRole = new Model.EmployeeRole() {
              Employee = emp,
              Role = role
            };
            _repository.Add<Model.EmployeeRole>(empRole);
          }
        }
        _repository.SaveChanges();
      }
      catch (Exception ex) {
        bool rethrow = LibraryExceptionHandler.HandleException(ref ex, System.Diagnostics.TraceEventType.Error);
        if (rethrow) {
          throw ex;
        }
      }
      return retVal;
    }

    public virtual bool AuthenticateUsernameAndPassword(string Username, string Password) {
      Model.Employee modEmployee = _repository.GetQuery<Model.Employee>().SingleOrDefault(o => o.User_ID.Equals(Username) && o.Password.Equals(Password) && o.Active_Ind.Equals(true));
      if (modEmployee == null) return false; else return true;
    }

    public override System.Linq.Expressions.Expression<Func<Model.Employee, bool>> UniqueEntityExp(Model.Employee modelEntity, VModel.Employee businessEntity) {
      return m => m.Emp_Name.Equals(modelEntity.Emp_Name, StringComparison.InvariantCultureIgnoreCase)
                  && m.User_ID.Equals(modelEntity.User_ID, StringComparison.InvariantCultureIgnoreCase)
                  && m.Active_Ind == true
                  && m.ID != modelEntity.ID;
    }
  }
}
