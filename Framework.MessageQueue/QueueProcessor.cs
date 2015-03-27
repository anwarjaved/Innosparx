namespace Framework.MessageQueue
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;

    using Framework.Ioc;
    using Framework.Threading;

    /// <summary>
    /// Class QueueProcessor. This class cannot be inherited.
    /// </summary>
    public sealed class QueueProcessor : DisposableObject, IBackgroundProcessor
    {
        private readonly int delay;

        private readonly Thread inputQueueThread;

        private readonly int maxNumberOfMessages;

        private readonly Func<MessageInfo, Task<bool>> processorFunc;

        private readonly string queueName;

        private bool running;
        private int errorCount;


        /// <summary>
        /// Initializes a new instance of the <see cref="QueueProcessor"/> class.
        /// </summary>
        /// <param name="queueName">Name of the queue.</param>
        /// <param name="processorFunc">The processor function.</param>
        /// <param name="maxNumberOfMessages">The maximum number of messages.</param>
        /// <param name="delay">The delay.</param>
        public QueueProcessor(
            string queueName,
            Func<MessageInfo, Task<bool>> processorFunc,
            int maxNumberOfMessages = 1,
            int delay = 30000)
        {
            this.queueName = queueName;
            this.processorFunc = processorFunc;
            this.maxNumberOfMessages = maxNumberOfMessages;
            this.delay = delay;
            ThreadStart queueReader = this.QueueReader;
            this.inputQueueThread = new Thread(queueReader) { IsBackground = true };
        }

        public string Name
        {
            get
            {
                return this.queueName;
            }
        }

        /// <summary>
        /// Starts this <see cref="QueueProcessor"/>.
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
        /// Stops this <see cref="QueueProcessor"/>.
        /// </summary>
        public void Stop()
        {
            if (this.running)
            {
                this.running = false;
                this.inputQueueThread.Interrupt();
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
            var queue = Container.Get<IQueue>();

            while (this.running)
            {
                DateTime nextExecutionTime = DateTime.Now.Add(TimeSpan.FromMilliseconds(this.delay));
                try
                {

                    ParallelQuery<MessageInfo> messages = queue.ReceiveMessage(
                        this.queueName,
                        this.maxNumberOfMessages).AsParallel();

                    foreach (MessageInfo messageInfo in messages)
                    {
                        if (await this.processorFunc(messageInfo))
                        {
                            queue.DeleteMessage(this.queueName, messageInfo.ID);
                        }
                    }

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