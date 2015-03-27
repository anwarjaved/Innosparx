namespace Framework
{
    using System;

    /// -------------------------------------------------------------------------------------------------
    /// <summary>
    ///     BenchmarkTest result.
    /// </summary>
    /// -------------------------------------------------------------------------------------------------
    public class BenchmarkResult
    {
        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        /// Gets or sets the title.
        /// </summary>
        /// <value>
        /// The title.
        /// </value>
        /// -------------------------------------------------------------------------------------------------
        public string Title { get; set; }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        /// Gets or sets the time of the total.
        /// </summary>
        /// <value>
        /// The total number of time.
        /// </value>
        /// -------------------------------------------------------------------------------------------------
        public double TotalTime { get; set; }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        /// Gets or sets the time run.
        /// </summary>
        /// <value>
        /// The time run.
        /// </value>
        /// -------------------------------------------------------------------------------------------------
        public int TimeRun { get; set; }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        /// Gets the time of the average.
        /// </summary>
        /// <value>
        /// The time of the average.
        /// </value>
        /// -------------------------------------------------------------------------------------------------
        public double AverageTime
        {
            get { return this.TotalTime / Convert.ToDouble(this.TimeRun); }
        }

        /// <summary>
        /// Displays the given fastestTime.
        /// </summary>
        /// <param name="fastestTime">Time of the fastest.</param>
        /// <returns>An string containing the fastest item result.</returns>
        public string Display(double fastestTime)
        {
            string percentSlowerText = string.Empty;

            if (fastestTime != this.TotalTime)
            {
                percentSlowerText = string.Format("\r\n      {0:0.0%} slower", (this.TotalTime / fastestTime) - 1.0);
            }

            return string.Format("   {0}{3}\r\n      {1:#,0.00} ms total -- {2:0.0000} avg ms per", this.Title, this.TotalTime, this.AverageTime, percentSlowerText);
        }
    }
}