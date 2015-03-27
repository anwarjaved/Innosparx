namespace Framework
{
    using System;
    using System.Diagnostics;

    ///-------------------------------------------------------------------------------------------------
    /// <summary>
    ///     Benchmark Watch.
    /// </summary>
    ///-------------------------------------------------------------------------------------------------
    public sealed class Benchmark : DisposableObject
    {
        private readonly Stopwatch watch;

        /// <summary>
        /// Starts A New Benchmark.
        /// </summary>
        /// <returns>.</returns>
        public static Benchmark Start()
        {
            return new Benchmark();
        }

        private Benchmark()
        {
            this.watch = new Stopwatch();
            this.watch.Start();
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Stops this object.
        /// </summary>
        ///-------------------------------------------------------------------------------------------------
        public void Stop()
        {
            this.Dispose();
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Gets the total seconds.
        /// </summary>
        ///
        /// <value>
        ///     The total number of seconds.
        /// </value>
        ///-------------------------------------------------------------------------------------------------
        public TimeSpan TotalTime { get; private set; }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Override This Method To Dispose Managed Resources.
        /// </summary>
        ///-------------------------------------------------------------------------------------------------
        protected override void DisposeResources()
        {
            this.watch.Stop();
            this.TotalTime = this.watch.Elapsed;
            base.DisposeResources();
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Returns a string that represents the current object.
        /// </summary>
        ///
        /// <returns>
        ///     A string that represents the current object.
        /// </returns>
        ///-------------------------------------------------------------------------------------------------
        public override string ToString()
        {
            return "Benchmark: {0}".FormatString(this.TotalTime.Humanize());
        }
    }
}
