using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoogleSDK.Analytics
{
    using Framework;

    [Flags]
    public enum MetricesType
    {
        [Description("ga:uniquePageviews")]
        UniquePageViews,

        [Description("ga:pageValue")]
        PageValue,

        [Description("ga:entrances")]
        Entrances,

        [Description("ga:pageviews")]
        PageViews,

        [Description("ga:timeOnPage")]
        TimeOnPage,

        [Description("ga:exits")]
        Exits,

        [Description("ga:entranceRate")]
        EntranceRate,

        [Description("ga:pageviewsPerVisit")]
        PageViewsPerVisit,

        [Description("ga:avgTimeOnPage")]
        AverageTimeOnPage,

        [Description("ga:exitRate")]
        ExitRate,
    }
}
