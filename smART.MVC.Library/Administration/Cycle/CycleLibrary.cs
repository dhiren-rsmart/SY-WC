// Copyright 2011, 2012 RecyclesmART, Inc. All rights reserved
// Main Author: Sanjeev Khanna
// Last Major Update: 07/01/2012

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AutoMapper;
using System.Data.Entity;
using Telerik.Web.Mvc;

using Model = smART.Model;
using VModel = smART.ViewModel;

namespace smART.Library {

  public class CycleLibrary : GenericLibrary<VModel.Cycle, Model.Cycle> {

    public CycleLibrary(): base() {
    }

    public CycleLibrary(string dbContextConnectionString)
      : base(dbContextConnectionString) {
    }


    public override void Initialize(string dbContextConnectionString) {
      base.Initialize(dbContextConnectionString); 
    }

    public bool  IsOverlap(int id, DateTime startDate, DateTime endDate) {
      return base.GetByExpression( i => i.ID != id && i.Start_Date <= startDate && i.End_Date>= endDate && i.Active_Ind ==true).Count()>0;
    }

    public int  GetPreviousCycleIDByStartDate(DateTime startDate) {
      int prevCycleId = 0;  
      var cycles = base.GetByExpression(i => i.Start_Date < startDate && i.Active_Ind == true).OrderByDescending(o => o.Start_Date).FirstOrDefault();
      if (cycles != null) {       
        prevCycleId = cycles.ID;
      }
      return prevCycleId;
    }

  }
}
