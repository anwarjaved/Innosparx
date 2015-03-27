using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework
{
    ///-------------------------------------------------------------------------------------------------
    /// <summary>
    ///     Time span extensions.
    /// </summary>
    ///-------------------------------------------------------------------------------------------------
    public static class TimeSpanExtensions
    {
        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     A TimeSpan extension method that humanizes the given span.
        /// </summary>
        ///
        /// <param name="span">
        ///     The span to act on.
        /// </param>
        /// <param name="skipEmpty">
        ///     (optional) the skip empty.
        /// </param>
        ///
        /// <returns>
        ///     Human Readable Timespan.
        /// </returns>
        ///-------------------------------------------------------------------------------------------------
        public static string Humanize(this TimeSpan span, bool skipEmpty = false)
        {
            var values = span.GetReadableStringElements().Where(str => !string.IsNullOrWhiteSpace(str)).ToList();

            if (values.Any())
            {
                return string.Join(", ", values);
            }

            if (skipEmpty) return string.Empty;
            return "0 ms";
        }

        private static IEnumerable<string> GetReadableStringElements(this TimeSpan span)
        {
            yield return GetDaysString((int)Math.Floor(span.TotalDays));
            yield return GetHoursString(span.Hours);
            yield return GetMinutesString(span.Minutes);
            yield return GetSecondsString(span.Seconds);
            yield return GetMiliSecondsString(span.Milliseconds);
        }

        private static string GetDaysString(int days)
        {
            if (days == 0)
                return string.Empty;

            if (days == 1)
                return "1 day";

            return string.Format("{0:0} days", days);
        }

        private static string GetHoursString(int hours)
        {
            if (hours == 0)
                return string.Empty;

            if (hours == 1)
                return "1 hour";

            return string.Format("{0:0} hours", hours);
        }

        private static string GetMinutesString(int minutes)
        {
            if (minutes == 0)
                return string.Empty;

            if (minutes == 1)
                return "1 minute";

            return string.Format("{0:0} minutes", minutes);
        }

        private static string GetSecondsString(int seconds)
        {
            if (seconds == 0)
                return string.Empty;

            if (seconds == 1)
                return "1 second";

            return string.Format("{0:0} seconds", seconds);
        }

        private static string GetMiliSecondsString(int milesconds)
        {
            if (milesconds == 0)
                return string.Empty;

            if (milesconds == 1)
                return "1 ms";

            return string.Format("{0:0} ms", milesconds);
        }
    }
}
