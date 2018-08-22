// Copyright 2011, 2012 RecyclesmART, Inc. All rights reserved
// Main Author: Sanjeev Khanna
// Last Major Update: 12/09/2011

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Web.Mvc;
using DataAnnotationsExtensions;

namespace smART.ViewModel {
  public class ScaleDetails : BaseEntity, IListType {

    [DisplayName("Scale")]
    [HiddenInput(DisplayValue = false)]
    public Scale Scale {
      get;
      set;
    }

    [DisplayName("Item")]
    [ScaffoldColumn(false)]
    public Item Item_Received {
      get;
      set;
    }

    [DisplayName("Apply to Inventory")]
    [UIHint("ItemDropDownList")]
    [HiddenInput(DisplayValue = false)]
    public Item Apply_To_Item {
      get;
      set;
    }

    [DisplayName("% Split")]
    public Decimal Split_Value {
      get;
      set;
    }

    [DisplayName("Gross Weight")]
    public Decimal GrossWeight {
      get;
      set;
    }

    [DisplayName("Tare Weight")]
    public Decimal TareWeight {
      get;
      set;
    }

    [DisplayName("Contamination")]
    public Decimal Contamination_Weight {
      get;
      set;
    }

    [DisplayName("Settlement Difference")]
    public decimal Settlement_Diff_NetWeight {
      get;
      set;
    }

    [DisplayName("Net Weight")]
    public Decimal NetWeight {
      get;
      set;
    }

    [DisplayName("Party Item")]
    [HiddenInput(DisplayValue = false)]
    public string Supplier_Item {
      get;
      set;
    }

    [DisplayName("Party Net Weight")]
    [HiddenInput(DisplayValue = false)]
    public Decimal Supplier_Net_Weight {
      get;
      set;
    }

    [HiddenInput(DisplayValue = false)]
    public Decimal Old_Net_Weight {
      get;
      set;
    }

    // =======================Scale ==========================
    [DisplayName("Rate")]
    [DataType("decimal(16 ,4")]
    [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:0.0000}")] 
    [HiddenInput(DisplayValue = false)]
    public Decimal Rate {
      get;
      set;
    }

    [DisplayName("Amount")]
    [HiddenInput(DisplayValue = false)]    
    public Decimal Amount {
      get {
        return decimal.Round(NetWeight * Rate, 2, MidpointRounding.AwayFromZero) ;
      }
    }

    [DisplayName("Notes")]
    public string Notes {
      get;
      set;
    }

    //[HiddenInput(DisplayValue = false)]    
    //public int Item_ID {
    //  get;
    //  set;
    //}

    public ScaleDetails()
      : base() {
      //Scale = new Scale();
      //Item_Received = new Item();
      //Apply_To_Item = new Item();
    }

    #region IListType Members

    [HiddenInput(DisplayValue = false)]
    public virtual string ListText {
      get {
        return ID.ToString();
      }
    }

    [HiddenInput(DisplayValue = false)]
    public virtual string ListValue {
      get {
        return ID.ToString();
      }
    }

    #endregion
  }
}
