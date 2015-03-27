using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace GoogleSDK.Analytics
{

    public class PageTrackingResultTotal
    {

        [JsonProperty("ga:uniquePageviews")]
        public string UniquePageViews { get; set; }

        [JsonProperty("ga:pageValue")]
        public string PageValue { get; set; }

        [JsonProperty("ga:entrances")]
        public string Entrances { get; set; }

        [JsonProperty("ga:pageviews")]
        public string PageViews { get; set; }

        [JsonProperty("ga:timeOnPage")]
        public string TimeOnPage { get; set; }

        [JsonProperty("ga:exits")]
        public string Exits { get; set; }

        [JsonProperty("ga:entranceRate")]
        public string EntranceRate { get; set; }

        [JsonProperty("ga:pageviewsPerVisit")]
        public string PageViewsPerVisit { get; set; }

        [JsonProperty("ga:avgTimeOnPage")]
        public string AverageTimeOnPage { get; set; }

        [JsonProperty("ga:exitRate")]
        public string ExitRate { get; set; }
    }

}
