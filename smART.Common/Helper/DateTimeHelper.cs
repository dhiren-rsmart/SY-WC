using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Globalization;

namespace smART.Common
{

    /// <summary>
    /// Provides methods to validate and parse different DateTime formats.   
    /// </summary>
    public class DateTimeHelper
    {

        /// <summary>
        /// Validate text by diffrent date format and cast text into date, if it is 
        /// of date time type.
        /// </summary>
        /// <param name="searchText">Search text.</param>
        /// <param name="searchDate">Valid date in output.</param>
        /// <returns>True, if date is valid.</returns>
        public static bool IsValidDate(string searchText, out DateTime validDate)
        {
            validDate = DateTime.Now;
            //bool isValidDate = DateTime.TryParse(searchText, out validDate);

            bool isValidDate = DateTime.TryParseExact(searchText, DateFormates(), null,
                                        DateTimeStyles.AllowWhiteSpaces,
                                        out validDate);

            if (validDate < DateTime.Parse("1/1/1753"))
            {
                isValidDate = false;

            }
            //// Swap date and month to make another possible date.  
            //// For Ex- if date is 7/8/2014 than make another date 8/7/2014 by swaping month and date.
            //if (isValidDate)
            //{
            //    if (validDate.Day <= 12 && validDate.Month <= 12 && (validDate.Day != validDate.Month))
            //        searchDate = string.Format("'{0}/{1}/{2}','{1}/{0}/{2}'", validDate.Day, validDate.Month, validDate.Year);
            //    else
            //        searchDate = string.Format("'{0}/{1}/{2}'", validDate.Month, validDate.Day, validDate.Year);
            //}

            return isValidDate;
        }

        /// <summary>
        /// Provides possible date formates.
        /// </summary>
        /// <returns>Array of date formates.</returns>
        public static string[] DateFormates()
        {
            string[] formats = { 
                    "dd/MM/yyyy", "dd.MM.yyyy",	"dd-MM-yyyy",	"dd MMMM yyyy",	"dd MMM yyyy",
                    "d/MM/yyyy", "d.MM.yyyy",	"d-MM-yyyy",	 "d MMMM yyyy", "d MMM yyyy",
                    "dd/M/yyyy", "dd.M.yyyy",	"dd-M-yyyy", 
                    "d/M/yyyy", "d.M.yyyy",	"d-M-yyyy",
                    "MM/dd/yyyy",	"MM.dd.yyyy",	"MM-dd-yyyy",	"MMMM dd yyyy",	"MMM dd yyyy",
                    "M/dd/yyyy",	"M.dd.yyyy",	"M-dd-yyyy",	"MMMM d yyyy",	"MMM d yyyy",
                    "MM/d/yyyy", "MM.d.yyyy",	"MM-d-yyyy",
                    "M/d/yyyy",	"M.d.yyyy",	"M-d-yyyy",
                    "yyyy/MM/dd",	"yyyy.MM.dd",	"yyyy-MM-dd",	"yyyy MMMM dd",	"yyyy MMM dd",
                    "yyyy/MM/d",	"yyyy.MM.d",	"yyyy-MM-d",	"yyyy MMMM d",	"yyyy MMM d",
                    "yyyy/M/dd",	"yyyy.M.dd",	"yyyy-M-dd" ,
                    "yyyy/M/d",   "yyyy.M.d",   "yyyy-M-d", "yyyy M d" ,
                    "yyyy/d/M",   "yyyy.d.M",   "yyyy-d-M", "yyyy d M","yyyyMMdd","MMddyyyy" };

            return formats;
        }

    }
}
