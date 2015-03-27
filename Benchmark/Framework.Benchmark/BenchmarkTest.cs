namespace Framework
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.IO;
    using System.Linq;
    using System.Runtime.InteropServices;
    using System.Text;

    /// -------------------------------------------------------------------------------------------------
    /// <summary>
    ///     Represents a BenchmarkTest.
    /// </summary>
    /// -------------------------------------------------------------------------------------------------
    public class BenchmarkTest
    {
        private const double WarmUpPercentage = 0.05;
        private readonly IList<BenchmarkItem> tests = new List<BenchmarkItem>();
        private readonly IDictionary<string, BenchmarkResult> results = new Dictionary<string, BenchmarkResult>();
        private readonly TextWriter writer;
        private readonly string fileOutputPath;
        private double totalMilliseconds = 0;
        private StringBuilder builder;

        /// <summary>
        /// Initializes a new instance of the BenchmarkTest class.
        /// </summary>
        /// <param name="fileOutputPath">The file output path.</param>
        /// -------------------------------------------------------------------------------------------------
        /// -------------------------------------------------------------------------------------------------
        public BenchmarkTest(string fileOutputPath)
            : this()
        {
            this.fileOutputPath = fileOutputPath;
        }

        /// <summary>
        /// Initializes a new instance of the BenchmarkTest class.
        /// </summary>
        /// <param name="writer">The writer.</param>
        public BenchmarkTest(TextWriter writer)
            : this()
        {
            this.writer = writer;
        }

        /// <summary>
        /// Initializes a new instance of the BenchmarkTest class.
        /// </summary>
        public BenchmarkTest()
        {
            if (this.writer == null)
            {
                this.writer = Console.Out;
            }
        }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        /// Gets or sets a value indicating whether the copy results to clipboard.
        /// </summary>
        /// <value>
        /// true if copy results to clipboard, false if not.
        /// </value>
        /// -------------------------------------------------------------------------------------------------
        public bool CopyResultsToClipboard { get; set; }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        /// Adds a test to 'test'.
        /// </summary>
        /// <param name="title">
        /// The title.
        /// </param>
        /// <param name="test">
        /// The test.
        /// </param>
        /// -------------------------------------------------------------------------------------------------
        public void AddTest(string title, Action test)
        {
            this.tests.Add(new BenchmarkItem { Title = title, Test = test });
        }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        /// Executes the tests.
        /// </summary>
        /// <param name="totalTime">
        /// Time of the total.
        /// </param>
        /// -------------------------------------------------------------------------------------------------
        public void RunTests(TimeSpan totalTime)
        {
            this.RunWarmUp(1);

            this.InternalRun(null, totalTime);
        }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        /// Executes the tests.
        /// </summary>
        /// <param name="minutes">
        /// The minutes.
        /// </param>
        /// <param name="seconds">
        /// The seconds.
        /// </param>
        /// -------------------------------------------------------------------------------------------------
        public void RunTests(int minutes, int seconds)
        {
            this.RunTests(TimeSpan.FromSeconds((minutes * 60) + seconds));
        }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        /// Executes the tests.
        /// </summary>
        /// <param name="num">
        /// (optional) number of.
        /// </param>
        /// -------------------------------------------------------------------------------------------------
        public void RunTests(int num = 100000)
        {
            this.RunWarmUp(num);

            this.InternalRun(num, null);
        }

        [DllImport("user32.dll")]
        private static extern bool OpenClipboard(IntPtr hWndNewOwner);

        [DllImport("user32.dll")]
        private static extern bool CloseClipboard();

        [DllImport("user32.dll")]
        private static extern bool SetClipboardData(uint uFormat, IntPtr data);

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        /// Executes the warm up operation.
        /// </summary>
        /// <param name="totalNum">
        /// The total number.
        /// </param>
        /// -------------------------------------------------------------------------------------------------
        private void RunWarmUp(int totalNum)
        {
            // WARM-UP
            // run thru each of them once because otherwise the first loop is slower due to the Just In Time compilation
            var numWarmups = Math.Ceiling(totalNum * WarmUpPercentage);

            for (var i = 0; i < numWarmups; i++)
            {
                foreach (var test in this.tests)
                {
                    test.Test();
                }
            }
        }

        private void InternalRun(int? totalTests, TimeSpan? totalTime)
        {
            this.builder = new StringBuilder();

            this.WriteLine();

            this.WriteLine(
                totalTests.HasValue
                    ? string.Format("Running each test {0:#,0} times", totalTests)
                    : string.Format("Running tests for {0}", totalTime));

            this.WriteLine();
            this.WriteLine("----------------------------------------------------");
            this.WriteLine("Running tests: ");

            if (totalTests.HasValue)
            {
                for (var i = 0; i < this.tests.Count; i++)
                {
                    this.WriteLine(string.Format("   Pass {0}...", i + 1));
                    this.InternalSingleRun(totalTests.Value / this.tests.Count);
                }
            }
            else if (totalTime.HasValue)
            {
                // the batch size will get adjusted based on how long they are taking to process
                //  we need to start with 1 and then increase based on their speed
                var batchSize = 1;
                long totalTimesRan = 0;
                var batchPercentage = 1.0 / (this.tests.Count + 1);

                while (this.totalMilliseconds < totalTime.Value.TotalMilliseconds)
                {
                    this.InternalSingleRun(batchSize);
                    totalTimesRan += batchSize;

                    // adjust batch size based on the current pace
                    var percentDone = this.totalMilliseconds / totalTime.Value.TotalMilliseconds;
                    var estimatedLeft = (totalTimesRan / percentDone) - totalTimesRan;

                    // let's be safe and try to split it up into a batch for each test so it gets to run in various order
                    batchSize = (int)Math.Ceiling(estimatedLeft * batchPercentage);
        
                    if (batchSize < 1)
                    {
                        batchSize = 1;
                    }

                    this.WriteLine(
                        string.Format(
                            "   Running new batch of {0:#,0} tests ({1:0}% complete so far) ...",
                            batchSize,
                            percentDone * 100));

                    // increase the percentage we want to get to for the next batch
                    //  this is used to estimate the number of times we need to run to get to 25%, then 50%, then 75%, etc. (for example)
                    batchPercentage += 1.0 / (this.tests.Count + 1);
                }

                this.WriteLine();
                this.WriteLine(string.Format("   Ran {0:#,0} times in time allotted", totalTimesRan));
                this.WriteLine(string.Format("   Took a total of {0}", TimeSpan.FromMilliseconds(this.totalMilliseconds)));
            }

            this.DisplayResults();

            var resultsText = this.builder.ToString();

            // write to the file if there is one
            if (!string.IsNullOrEmpty(this.fileOutputPath))
            {
                File.WriteAllText(this.fileOutputPath, resultsText);
            }

            if (this.CopyResultsToClipboard)
            {
                OpenClipboard(IntPtr.Zero);
                var ptr = Marshal.StringToHGlobalUni(resultsText);
                SetClipboardData(13, ptr);
                CloseClipboard();
                Marshal.FreeHGlobal(ptr);
            }
        }

        private void InternalSingleRun(int num)
        {
            var sw = new Stopwatch();

            foreach (var benchmarkItem in this.tests.OrderBy(x => Guid.NewGuid()))
            {
                sw.Reset();
                GC.Collect();
                sw.Start();

                for (var i = 0; i < num; i++)
                {
                    benchmarkItem.Test();
                }

                sw.Stop();

                this.AddResult(benchmarkItem, sw.Elapsed.TotalMilliseconds, num);
            }
        }

        private void DisplayResults()
        {
            this.WriteLine();
            this.WriteLine("Results: (fastest first)");

            var fastestTime = this.results.Values.Min(x => x.TotalTime);
            foreach (var item in this.results.Values.OrderBy(x => x.TotalTime))
            {
                this.WriteLine(item.Display(fastestTime));
                this.WriteLine();
            }

            this.WriteLine();
        }

        private void AddResult(BenchmarkItem item, double totalTime, int timesRun)
        {
            if (!this.results.ContainsKey(item.Title))
            {
                this.results.Add(item.Title, new BenchmarkResult { Title = item.Title, TotalTime = 0.0, TimeRun = 0 });
            }

            var result = this.results[item.Title];
            result.TimeRun += timesRun;
            result.TotalTime += totalTime;
            this.totalMilliseconds += totalTime;
        }
        
        private void WriteLine(string text = "")
        {
            this.Write(text + "\r\n");
        }

        private void Write(string text = "")
        {
            this.writer.Write(text);
            this.builder.Append(text);
        }
    }
}
