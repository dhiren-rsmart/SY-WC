using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;

namespace smART.MVC.Service.Model
{

    public abstract class BaseEntity
    {

        public int ID
        {
            get;
            set;
        }

        public DateTime? Time_Stamp
        {
            get;
            set;
        }

        public bool Active_Ind
        {
            get;
            set;
        }

        public string Created_By
        {
            get;
            set;
        }

        public string Updated_By
        {
            get;
            set;
        }

        public DateTime Created_Date
        {
            get;
            set;
        }

        public DateTime Last_Updated_Date
        {
            get;
            set;
        }

        
        public int? Unique_ID { get; set; }

        public int? Site_Org_ID { get; set; }

        public virtual void MapServiceEntityToServerEntity(smART.ViewModel.BaseEntity serverEntity)
        {
            serverEntity.ID = ID;
            serverEntity.Created_By = Created_By;
            serverEntity.Updated_By = Updated_By;
            serverEntity.Created_Date = Created_Date;
            serverEntity.Last_Updated_Date = Last_Updated_Date;
            serverEntity.Active_Ind = true;
            serverEntity.Site_Org_ID = Site_Org_ID;
            serverEntity.Unique_ID = Unique_ID;
        }
    }
}
