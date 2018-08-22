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

    public class ScaleAttachments : smART.MVC.Service.Model.BaseEntity
    {

        public int Scale_ID
        {
            get;
            set;
        }

        public string Document_Title
        {
            get;
            set;
        }

        public string Document_Path
        {
            get;
            set;
        }

        public int Document_RelatedTo
        {
            get;
            set;
        }

        public int Document_RelatedID
        {
            get;
            set;
        }

        public string Document_RefId
        {
            get;
            set;
        }

        public void MapServiceEntityToServerEntity(smART.ViewModel.ScaleAttachments serverEntity)
        {
            base.MapServiceEntityToServerEntity(serverEntity);
            serverEntity.Document_Title = Document_Title;
            serverEntity.Document_Name = Document_Title;
            //serverEntity.Document_Path = Document_Path;
            serverEntity.Ref_Type = Document_RelatedTo;
            serverEntity.Ref_ID = Document_RelatedID;
            serverEntity.Document_Type = Document_RelatedTo == 3 ? "bmp" : "jpg";
            //serverEntity.Document_RefId = new Guid (Document_RefId);

        }
    }
}
