// Copyright 2011, 2012 RecyclesmART, Inc. All rights reserved
// Main Author: Sanjeev Khanna
// Last Major Update: 07/01/2012

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Web.Mvc;
using DataAnnotationsExtensions;

namespace smART.ViewModel
{

    public class LeadLog:BaseEntity
    {

        public string Scale_Ticket_No
        {
            get;
            set;
        }


        public string Status
        {
            get;
            set;
        }

        public string Status_Remarks
        {
            get;
            set;
        }

    }
}
