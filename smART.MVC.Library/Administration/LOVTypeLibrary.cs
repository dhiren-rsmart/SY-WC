using VModel = smART.ViewModel;
using Model = smART.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using AutoMapper;
using System.Linq.Expressions;


namespace smART.Library
{
    public class LOVTypeLibrary: GenericLibrary<VModel.LOVType, Model.LOVType>
    {
        public LOVTypeLibrary() : base() { }
        public LOVTypeLibrary(string dbContextConnectionString) : base(dbContextConnectionString) { }

        public override System.Linq.Expressions.Expression<Func<Model.LOVType, bool>> UniqueEntityExp(Model.LOVType modelEntity, VModel.LOVType businessEntity) {
          return m => m.LOVType_Name.Equals(modelEntity.LOVType_Name, StringComparison.InvariantCultureIgnoreCase)                      
                      && m.Active_Ind == true
                      && m.ID != modelEntity.ID;
        }

        public override VModel.LOVType Add(VModel.LOVType addObject) {
          VModel.LOVType insertedObjectBusiness = addObject;
          try {
            Model.LOVType newModObject = Mapper.Map<VModel.LOVType, Model.LOVType>(addObject);
            newModObject.ParentType = _repository.GetQuery<Model.LOVType>().SingleOrDefault(o => o.ID == addObject.ParentType.ID);
            
            if (Adding(insertedObjectBusiness, newModObject, _dbContext)) {
              Model.LOVType insertedObject = _repository.Add<Model.LOVType>(newModObject);
              _repository.SaveChanges();
              insertedObjectBusiness = Mapper.Map<Model.LOVType, VModel.LOVType>(insertedObject);
              Added(insertedObjectBusiness, newModObject, _dbContext);
            }
          }
          catch (Exception ex) {
            bool rethrow;
            rethrow = LibraryExceptionHandler.HandleException(ref ex, insertedObjectBusiness.Updated_By, insertedObjectBusiness.GetType().Name, insertedObjectBusiness.ID.ToString());
            if (rethrow)
              throw ex;
          }
          return insertedObjectBusiness;
        }

        protected override void Modify(Expression<Func<Model.LOVType, bool>> predicate, VModel.LOVType modObject, string[] includePredicate = null) {
          try {
            Model.LOVType newModObject = Mapper.Map<VModel.LOVType, Model.LOVType>(modObject);

            if (newModObject.ParentType != null)
              newModObject.ParentType = _repository.GetQuery<Model.LOVType>().SingleOrDefault(o => o.ID == modObject.ParentType.ID);

            if (Modifying(modObject, newModObject, _dbContext)) {
              _repository.Modify<Model.LOVType>(predicate, newModObject, includePredicate);
              _repository.SaveChanges();
              Modified(modObject, newModObject, _dbContext);
            }
          }
          catch (Exception ex) {
            bool rethrow;
            rethrow = LibraryExceptionHandler.HandleException(ref ex, modObject.Updated_By, modObject.GetType().Name, modObject.ID.ToString());
            if (rethrow)
              throw ex;
          }
        }

        public VModel.LOVType GetByLOVType(string LovType) {
          return GetSingleByExpression(s => s.LOVType_Name== LovType && s.Active_Ind == true );          
        }
    }
}
