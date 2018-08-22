jQuery.fn.autoCapital = function ()
{
  $(this).each(function ()
  {
    $(this).bind('keyup', function ()
    {
      $(this).val($(this).val().toUpperCase());
    });
  });
}

jQuery.fn.autoLower = function ()
{
  $(this).each(function ()
  {
    $(this).bind('keyup', function ()
    {
      $(this).val($(this).val().toLowerCase());
    });
  });
}

jQuery.fn.autoFirstLetterCapital = function ()
{
  $(this).each(function ()
  {
    $(this).bind('keyup', function ()
    {
      $(this).val($(this).val().toLowerCase()
                .replace(/\b[a-z]/g, function (letter)
                {
                  return letter.toUpperCase();
                })
                );
    });
  });
}