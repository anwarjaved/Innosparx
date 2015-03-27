using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework.Threading
{
    public sealed class BackgroundWorker : DisposableObject
    {
        private readonly Dictionary<string, IBackgroundProcessor> processors = new Dictionary<string, IBackgroundProcessor>();

        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Queues Processor.
        /// </summary>
        ///
        /// <remarks>
        ///     Anwar Javed, 03/27/2014 4:02 PM.
        /// </remarks>
        ///
        /// <param name="name">
        ///     The name.
        /// </param>
        /// <param name="processorFunc">
        ///     The processor function.
        /// </param>
        /// <param name="delay">
        ///     (Optional) the delay.
        /// </param>
        ///-------------------------------------------------------------------------------------------------
        public void Queue(string name, Func<string, Task> processorFunc, int delay = 30)
        {
            if (!this.processors.ContainsKey(name))
            {
                this.processors.Add(name, new BackgroundProcessor(name, processorFunc, delay));
            }
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Queues Processor.
        /// </summary>
        ///
        /// <remarks>
        ///     Anwar Javed, 03/27/2014 10:03 PM.
        /// </remarks>
        ///
        /// <param name="name">
        ///     The name.
        /// </param>
        /// <param name="processorFunc">
        ///     The processor function.
        /// </param>
        /// <param name="delay">
        ///     (Optional) the delay.
        /// </param>
        ///-------------------------------------------------------------------------------------------------
        public void Queue(string name, Func<Task> processorFunc, int delay = 30)
        {
            if (!this.processors.ContainsKey(name))
            {
                this.processors.Add(name, new BackgroundProcessor(name, processorFunc, delay));
            }
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Queues Processor.
        /// </summary>
        ///
        /// <remarks>
        ///     Anwar Javed, 03/27/2014 6:14 PM.
        /// </remarks>
        ///
        /// <param name="name">
        ///     The name.
        /// </param>
        /// <param name="processor">
        ///     The processor.
        /// </param>
        ///-------------------------------------------------------------------------------------------------
        public void Queue(string name, IBackgroundProcessor processor)
        {
            if (!this.processors.ContainsKey(name))
            {
                this.processors.Add(name, processor);
            }
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Removes the head object from this queue.
        /// </summary>
        ///
        /// <remarks>
        ///     Anwar Javed, 03/27/2014 4:04 PM.
        /// </remarks>
        ///
        /// <param name="name">
        ///     The name.
        /// </param>
        ///-------------------------------------------------------------------------------------------------
        public void Dequeue(string name)
        {
            if (this.processors.ContainsKey(name))
            {
                this.processors[name].Stop();
                this.processors.Remove(name);
            }
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Starts all.
        /// </summary>
        ///
        /// <remarks>
        ///     Anwar Javed, 03/27/2014 4:00 PM.
        /// </remarks>
        ///-------------------------------------------------------------------------------------------------
        public void StartAll()
        {
            foreach (var backgroundProcessor in processors)
            {
                backgroundProcessor.Value.Start();
            }
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Stops all.
        /// </summary>
        ///
        /// <remarks>
        ///     Anwar Javed, 03/27/2014 4:00 PM.
        /// </remarks>
        ///-------------------------------------------------------------------------------------------------
        public void StopAll()
        {
            foreach (var backgroundProcessor in processors)
            {
                backgroundProcessor.Value.Stop();
            }
        }

        /// <summary>
        /// Override This Method To Dispose Managed Resources.
        /// </summary>
        protected override void DisposeResources()
        {
            this.StopAll();
            base.DisposeResources();
        }
    }
}
