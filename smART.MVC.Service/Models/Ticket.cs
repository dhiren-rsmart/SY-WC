using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using smART.ViewModel;

namespace smART.MVC.Service
{

    public class Ticket : smART.MVC.Service.Model.BaseEntity
    {

        public smART.MVC.Service.Model.Scale Scale
        {
            get;
            set;
        }

        public List<smART.MVC.Service.Model.ScaleDetails> ScaleDetails
        {
            get;
            set;
        }

        public List<smART.MVC.Service.Model.ScaleAttachments> ScaleAttachments
        {
            get;
            set;
        }


    }
}