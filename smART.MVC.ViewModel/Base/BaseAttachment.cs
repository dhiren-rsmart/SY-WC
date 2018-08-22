// Copyright 2011, 2012 RecyclesmART, Inc. All rights reserved
// Main Author: Sanjeev Khanna
// Last Major Update: 11/01/2011

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Web.Mvc;
using System.Web;

namespace smART.ViewModel {
  public abstract class BaseAttachment : BaseEntity {


    //[DisplayName("")]
    //[ClientTemplateHtml("<input type='checkbox' name='cbxLock' value='<#= false #>' readonly='readonly'/>")]
    //public bool SelctChk { get; set; }

    [Required]
    [StringLength(45, ErrorMessage = "Maximum length is 45")]
    [Display(Name = "Title", Order = 1, Description = "Title")]
    [DataType(System.ComponentModel.DataAnnotations.DataType.Url)]
    //[ClientTemplateHtml("<a target='blank' href='/../ItemAttachment/OpenDocument?id=<#= Document_RefId #>'><#= Document_Title #></a>")]
    [ClientTemplateHtml("<a target='blank' href='/../<#= ControllerName #>/OpenDocument?id=<#= Document_RefId #>'><#= Document_Title #></a>")]
    public virtual string Document_Title { get; set; }

    [Required]
    [StringLength(45, ErrorMessage = "Maximum length is 45")]
    [Display(Name = "Name", Order = 2, Description = "Title")]
    public string Document_Name { get; set; }


    [DisplayName("Ref ID")]
    [HiddenInput(DisplayValue = false)]
    public Guid Document_RefId { get; set; }

    [StringLength(10, ErrorMessage = "Maximum length is 10")]
    [Display(Name = "Document Type", Order = 2, Description = "Document Type")]
    public string Document_Type { get; set; }

    [ClientTemplateHtml("<#= Math.ceil(parseInt(Document_Size) / 1024) #>")]
    [Display(Name = "File size in KB", Order = 4, Description = "File size in KB")]
    public long Document_Size { get; set; }

    [StringLength(256, ErrorMessage = "Maximum length is 256")]
    [HiddenInput(DisplayValue = false)]
    [Display(Name = "Path", Order = 5, Description = "Path")]
    public string Document_Path { get; set; }

    [StringLength(25, ErrorMessage = "Maximum length is 25")]
    [HiddenInput(DisplayValue = false)]
    [Display(Name = "Mime Type", Order = 6, Description = "Mime Type")]
    public string Mime_Type { get; set; }


    [StringLength(45, ErrorMessage = "Maximum Length is 45")]
    [Display(Name = "Last Modified By", Order = 7, Description = "Last Modified By")]
    public override string Updated_By { get; set; }


    [Display(Name = "Last Updated", Order = 8, Description = "Last Updated")]
    public override DateTime? Last_Updated_Date { get; set; }

    [HiddenInput(DisplayValue = false)]
    public string ControllerName { get; set; }

    //[HiddenInput(DisplayValue = false)]
    [ScaffoldColumn(true)]
    [Display(Name = "Image", Order = 9, Description = "Image")]
    [ClientTemplateHtml("<img alt='<#= Image #>' width='150' height='150' src='<#= Image #>' />")]
    public string Image { get; set; }

  }
}
