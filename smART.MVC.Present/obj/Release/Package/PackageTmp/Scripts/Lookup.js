$.fn.lookupRebind = function (URL, searchData, tGrid) {
    onLookupRebind(URL, searchData, tGrid);
}

  function lookupOpenWindow(windowID, gridID) {
    // Refrest grid data.
    refreshGrid(gridID);

    var window = $(windowID).data("tWindow");
//    window.center().open();
    
    var overlay = $('.t-overlay');
//    overlay.data('oldZIndex', overlay.css('zIndex'))
//               .css('zIndex', 20000);

    $(windowID).css('zIndex', 20001)
                     .data("tWindow")
                     .center()
                     .open();
}

function lookupCloseWindow(windowID) {
    var window = $(windowID).data("tWindow");
    window.close();

    var overlay = $('.t-overlay');
    overlay.css('zIndex', overlay.data('oldZIndex'));
    $(windowID).data('tWindow').close();
}

function onLookupMasterSelect(e, valueControlID, textControlID, URL, closeWindowID, textFieldName, onChange) {
    $.ajax({
        url: URL,
        type: 'POST',
        contentType: 'application/Json',
        data: '{ id: ' + e + '}',
        success: function (dat) {
            $.each(dat, function (key, element) {
                $(valueControlID + key).val(element);
                if (key == textFieldName)
                    $(textControlID).val(element);
            });
            
            if(onChange != null && onChange != '')
                window[onChange](dat);

            lookupCloseWindow(closeWindowID);
        }
    });
}

function onLookupRebind(URL, searchData, tGrid) {
    $.ajax({
        url: URL,
        type: 'POST',
        contentType: 'application/Json',
        data: searchData,
        success: function (dat) {
          tGrid.dataBind(dat);

        }
    });
}

function refreshlookupWindow(grid, controller, action, routedvalue) {    
    var tGrid = $(grid).data('tGrid');
    tGrid.rebind(routedvalue);
  }

  function refreshGrid(gridId) {
    initialLoad = false;
    var grid = $(gridId).data("tGrid");  // Modify the grid ID to your own!!!
    grid.ajaxRequest();
  }

  var initialLoad = true;
  function LookupGrid_onDataBinding(e) {    
    if (initialLoad) {      
      e.preventDefault();     
    }   
  }     