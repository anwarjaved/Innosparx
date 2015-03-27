using System;
using System.ComponentModel;
using System.Globalization;
using System.Text;

namespace Framework
{
    [EditorBrowsable(EditorBrowsableState.Never)]
    public static class DateTimeExtentions
    {
        /// <summary>
        /// Returns the first day of the week that the specified date 
        /// is in. 
        /// </summary>
        public static DateTime GetFirstDayOfWeek(this DateTime dayInWeek, CultureInfo cultureInfo = null)
        {
            if (cultureInfo == null)
            {
                cultureInfo = CultureInfo.CurrentCulture;
            }

            DayOfWeek firstDay = cultureInfo.DateTimeFormat.FirstDayOfWeek;
            DateTime firstDayInWeek = dayInWeek.Date;
            
            while (firstDayInWeek.DayOfWeek != firstDay)
            {
                firstDayInWeek = firstDayInWeek.AddDays(-1);
            }

            return firstDayInWeek;
        }

        public static string GetWeek(this DateTime dayInWeek, string format = "dd MMM", string seperator = "-", CultureInfo cultureInfo = null)
        {
            DateTime firstDayInWeek = dayInWeek.GetFirstDayOfWeek(cultureInfo);

            DateTime lastDayInWeek = firstDayInWeek.AddDays(7);

            StringBuilder sb = new StringBuilder();

            sb.Append(firstDayInWeek.ToString(format, cultureInfo));
            sb.Append(seperator);
            sb.Append(lastDayInWeek.ToString(format, cultureInfo));

            return sb.ToString();
        }

        public static string GetMonth(this DateTime dateTime, string format = "MMM", CultureInfo cultureInfo = null)
        {
            DateTime firstDayInMonth = dateTime.GetFirstDayOfMonth();

            StringBuilder sb = new StringBuilder();

            sb.Append(firstDayInMonth.ToString(format));

            return sb.ToString();
        }

        public static DateTime GetFirstDayOfMonth(this DateTime dateTime)
        {
            return new DateTime(dateTime.Year, dateTime.Month, 1);
        }


        public static DateTime GetLastDayOfMonth(this DateTime dateTime)
        {
            DateTime firstDayOfTheMonth = new DateTime(dateTime.Year, dateTime.Month, 1);
            return firstDayOfTheMonth.AddMonths(1).AddDays(-1);
        }
    }
}
