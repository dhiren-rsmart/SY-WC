using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net;
using System.Net.Http;
using System.Web.Http;

using smART.Library;
using smART.ViewModel;
using System.Transactions;
using System.IO;

namespace smART.MVC.Service.Model
{

    public class ScaleDetails : smART.MVC.Service.Model.BaseEntity
    {

        public int Scale_ID
        {
            get;
            set;
        }

        public int Item_ID
        {
            get;
            set;
        }

        public decimal GrossWeight
        {
            get;
            set;
        }


        public decimal TareWeight
        {
            get;
            set;
        }


        public decimal NetWeight
        {
            get;
            set;
        }

        public decimal Rate
        {
            get;
            set;
        }

        public void MapServiceEntityToServerEntity(smART.ViewModel.ScaleDetails serverEntity)
        {
            base.MapServiceEntityToServerEntity(serverEntity);
            serverEntity.GrossWeight = GrossWeight;
            serverEntity.TareWeight = TareWeight;
            serverEntity.NetWeight = NetWeight;            
            serverEntity.Rate = Rate;
        }
    }
}
