
// Hide Standard buttons.
$(document).ready(function ()
{
  $("#StandardButtonSection").attr('style', 'display:none');
});

// Add New Ticket.
function NewTicket()
{  
    RefreshContent('/QScale/New');
    //$.post('@Url.Action("_Scan", "ThumbScanner")', { id: '@Model.ID' });
    //refreshThumbScanner();
}

// Save and New Ticket.
function SaveAndNewTicket()
{
  var isSettled = $("#Ticket_Settled").val();
  if (isSettled == 'True')
  {  
    NewTicket();
  }
  else
  {
    ValidateAndSaveScaleEntity(AfterValidateAndSaveScaleEntity);
  }
}

function AfterValidateAndSaveScaleEntity()
{ 
   NewTicket(); 
}

//      // Save Ticket.
//      function saveTicket()
//      {      
//        $.ajax({
//          type: "POST",
//          url: '/QScale/Save',
//          data: $("#qscalefrm").serialize(), // serializes the form's elements.
//          success: function (data)
//          {
//            //alert(data); // show response from the php script.
//          }
//        });
//      }







function showOpenTicketWindow()
{
}

function print()
{
}

function onRadingSelectionChange()
{
}

function RefreshTicket() {
    RefreshPannel();
}

function RefreshContent(url)
{
  //get a reference to the splitter and load the corresponding content in the second pane
  splitter = $("#Splitter");
  if (splitter[0] && url)
  {

    splitter.data("tSplitter").ajaxRequest(splitter.find(".t-pane")[1], url);
  }
}

function RefreshPannel()
{
  var url = '/QScale/Navigation'
  //get a reference to the splitter and load the corresponding content in the second pane
  splitter = $("#Splitter");
  if (splitter[0] && url)
  {
    splitter.data("tSplitter").ajaxRequest(splitter.find(".t-pane")[0], url)
    pause(500);    
  }
}

function pause(millis)
{
  var date = new Date();
  var curDate = null;
  do { curDate = new Date(); }
  while (curDate - date < millis)
}

function SetDefaultFilterToContains() {
    $('.t-grid-filter').one('click', function () {
        var container = $(this).data().filter;
        $('.t-filter-operator', container).val('substringof');
    });
}