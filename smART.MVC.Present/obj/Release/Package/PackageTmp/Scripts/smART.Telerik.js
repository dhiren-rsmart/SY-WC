// This is called from all child Telerik grids in a Parent-Child form
function Grid_onSave(e) {
  // Show grid popup submit buttons
  ShowHideGridSubmitButton(false);
}

//This is called from all child Telerik grids in a Parent-Child form
function Child_Grid_onEdit(e) {

  // Show grid popup submit buttons
  ShowHideGridSubmitButton(true);

  // The following code is to center the edit popup page
  var popup = $("#" + e.currentTarget.id + "PopUp");
  //get the data window contained by the popup
  var popupDataWin = popup.data("tWindow");
  //change popup title by calling the title 
  if (e.mode == "insert") {
    $("#Child_ID").val("0");
    popupDataWin.title("Add new");
  }
  else
    popupDataWin.title("Edit");
  //center the window by calling the method center
  popupDataWin.center();

  //remove validation summary brought over from the parent form
  popup.find('div.validation-summary-errors').find('ul').find('li').remove();
}

function Grid_onError(args) {
  if (args.textStatus == "modelstateerror" && args.modelState) {
    var message = "Errors:\n";
    $.each(args.modelState, function (key, value) {
      if ('errors' in value) {
        $.each(value.errors, function () {
          $(".validation-summary-errors").append("<li>" + this + "</li>");
          message += this + "\n";
        });
      }
    });
    args.preventDefault();
  }

  // Show grid popup submit buttons
  ShowHideGridSubmitButton(true);

}

function TabSelect(tab, index) {
  var tabstrip = $(tab).data("tTabStrip");
  if (tabstrip != null) {
    var item = $("li", tabstrip.element)[index];
    tabstrip.select(item);
  }
}

function TabEnable(tab, index) {
  var tabstrip = $(tab).data("tTabStrip");
  if (tabstrip != null) {
    var item = $("li", tabstrip.element)[index];
    tabstrip.enable(item);
  }
}

function TabDisable(tab, index) {
  var tabstrip = $(tab).data("tTabStrip");
  if (tabstrip != null) {
    var item = $("li", tabstrip.element)[index];
    tabstrip.disable(item);
  }
}

function ShowHideGridSubmitButton(value) {
  if (value == false) {
    $('.t-grid-insert').hide();
    $('.t-grid-update').hide();
    $('.t-grid-cancel').hide();
  }
  else {
    $('.t-grid-insert').show();
    $('.t-grid-update').show();
    $('.t-grid-cancel').show();
  }
}

function isEmpty(value) {
  return (typeof value === "undefined" || value === null || value=='');
}

//#region Attachment

function ShowAttchmentImage(row, grid)
{
    var dataItem = grid.dataItem(row)
    if (dataItem != null && dataItem.Image == '')
    {
      ShowHideAttachmentRowImage(row, "none");
    }
    else
    {
      ShowHideAttachmentRowImage(row, "inline");
    }
}

function ShowHideAttchmentImage(row, grid) {
  if ($("#chkImage").is(':checked')) {
    var dataItem = grid.dataItem(row)
    if (dataItem != null && dataItem.Image == '') {
      ShowHideAttachmentRowImage(row, "none");
    }
    else {
      ShowHideAttachmentRowImage(row, "inline");
    }
  }
  else {
    ShowHideAttachmentRowImage(row, "none");
  }
}

function ShowHideAttachmentRowImage(row, visibility) {
  $("Image", row).css("display", visibility);
}

//#endregion 

//#region Expense

function Expense_Grid_onEdit(e) {
  Child_Grid_onEdit(e);
  if (e.dataItem["Paid_Party_To"] != null) {
    $("#LabelID_Paid_Party_To").val(e.dataItem["Paid_Party_To"].Party_Name);
  }
}

//#endregion 

//#region Common

function SetDefaultFilterToContains() {
    $('.t-grid-filter').one('click', function () {
        var container = $(this).data().filter;
        $('.t-filter-operator', container).val('substringof');
    });
}
//#endregion Common

