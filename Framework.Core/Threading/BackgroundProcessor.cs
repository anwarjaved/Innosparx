using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework.Threading
{
    using System.Threading;

    ///-------------------------------------------------------------------------------------------------
    /// <summary>
    ///     A background processor to invoke background tasks after specified intervals.
    /// </summary>
    ///
    /// <remarks>
    ///     Anwar Javed, 03/27/2014 3:44 PM.
    /// </remarks>
    ///-------------------------------------------------------------------------------------------------
    public sealed class BackgroundProcessor : DisposableObject, IBackgroundProcessor
    {       private readonly int delay;

        private readonly Thread inputQueueThread;

        private readonly Func<string, Task> processorFunc;

        private readonly string name;

        private int errorCount;

        private bool running;

        /// <summary>
        /// Initializes a new instance of the <see cref="BackgroundProcessor"/> class.
        /// </summary>
        /// <param name="name">Name of the processor.</param>
        /// <param name="processorFunc">The processor function.</param>
        /// <param name="delay">The delay.</param>
        public BackgroundProcessor(
            string name,
            Func<string, Task> processorFunc,
            int delay = 30000)
        {
            if (processorFunc == null)
            {
                throw new ArgumentNullException("processorFunc");
            }

            this.name = name;
            this.processorFunc = processorFunc;
            this.delay = delay;
            ThreadStart queueReader = this.QueueReader;
            this.inputQueueThread = new Thread(queueReader) { IsBackground = true };
        }

        public BackgroundProcessor(
           string name,
           Func<Task> processorFunc,
           int delay = 30000)
            : this(name, n => processorFunc(), delay)
        {
        }

        public string Name
        {
            get
            {
                return this.name;
            }
        }

        /// <summary>
        /// Starts this <see cref="BackgroundProcessor"/>.
        /// </summary>
        public void Start()
        {
            if (!this.running)
            {
                this.running = true;
                this.inputQueueThread.Start();
            }
        }

        /// <summary>
        /// Stops this <see cref="BackgroundProcessor"/>.
        /// </summary>
        public void Stop()
        {
            if (this.running)
            {
                this.running = false;
                try
                {
                    this.inputQueueThread.Interrupt();
                }
                catch (Exception)
                {
                }
            }
        }

        /// <summary>
        ///     Override This Method To Dispose Managed Resources.
        /// </summary>
        protected override void DisposeResources()
        {
            this.Stop();
            base.DisposeResources();
        }

        private async void QueueReader()
        {
            while (this.running)
            {
                DateTime nextExecutionTime = DateTime.Now.Add(TimeSpan.FromMilliseconds(this.delay));

                try
                {
                    await this.processorFunc(this.Name);

                    errorCount = 0;

                    DateTime now = DateTime.Now;

                    if (nextExecutionTime > now)
                    {
                        var diff = nextExecutionTime.Subtract(now);

                        Thread.Sleep(diff);
                    }
                }
                catch (ThreadInterruptedException)
                {
                }
                catch (Exception)
                {
                    errorCount++;

                    errorCount++;

                    if (errorCount > 15)
                    {
                        errorCount = 0;

                        Thread.Sleep(TimeSpan.FromMinutes(10));
                    }
                }
            }
        }
    }
}