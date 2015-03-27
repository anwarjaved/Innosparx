namespace Framework.MessageQueue.Impl
{
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Linq;

    using Framework.Ioc;

    /// <summary>
    /// Class MemoryQueue.
    /// </summary>
    [InjectBind(typeof(IQueue), LifetimeType.Singleton)]
    [InjectBind(typeof(IQueue), "MemoryQueue", LifetimeType.Singleton)]
    public class MemoryQueue : IQueue
    {
        private readonly ConcurrentDictionary<string, QueueData> internalQueue =
            new ConcurrentDictionary<string, QueueData>(StringComparer.OrdinalIgnoreCase);

        /// <summary>
        ///     Creates the queue.
        /// </summary>
        /// <param name="name">The name of the queue.</param>
        /// <param name="delay">The time in seconds that the delivery of all messages in the queue will be delayed.</param>
        /// <param name="messageRetentionPeriod">The number of seconds queue retains a message.</param>
        /// <param name="visibilityTimeout">
        ///     The length of time, in seconds, that a message received from a queue will be invisible
        ///     to other receiving components when they ask to receive messages.
        /// </param>
        public void CreateQueue(
            string name,
            int delay = 0,
            int messageRetentionPeriod = 1209600,
            int visibilityTimeout = 90)
        {
            this.internalQueue.GetOrAdd(
                name,
                s =>
                new QueueData(new QueueInfo(name, delay, messageRetentionPeriod, DateTime.UtcNow, visibilityTimeout)));
        }

        /// <summary>
        ///     The DeleteMessage action deletes the specified message from the specified queue.
        /// </summary>
        /// <param name="name">The name of the queue.</param>
        /// <param name="messageID">The message identifier.</param>
        public void DeleteMessage(string name, string messageID)
        {
            QueueData queueData;
            if (this.internalQueue.TryGetValue(name, out queueData))
            {
                MessageInfo messageData;
                queueData.Messages.TryRemove(messageID, out messageData);
            }
        }

        /// <summary>
        ///     The DeleteMessage action deletes the queue, regardless of whether the queue is empty.
        /// </summary>
        /// <param name="name">The name of the queue.</param>
        public void DeleteQueue(string name)
        {
            QueueData queue;
            this.internalQueue.TryRemove(name, out queue);
        }

        /// <summary>
        ///     Gets a list of your queues.
        /// </summary>
        /// <returns>An collection of <see cref="IReadOnlyCollection{QueueInfo}" /> containing list of queues.</returns>
        public IReadOnlyCollection<QueueInfo> GetAllQueue()
        {
            return this.internalQueue.Values.Select(x => x.Queue).ToList();
        }

        /// <summary>
        ///     The ReceiveMessage action retrieves one or more messages from the specified queue.
        /// </summary>
        /// <param name="name">The name of the queue.</param>
        /// <param name="maxNumberOfMessages">Maximum number of messages to return.</param>
        /// <returns>An collection of <see cref="IReadOnlyCollection{Message}" />.</returns>
        public IReadOnlyCollection<MessageInfo> ReceiveMessage(string name, int maxNumberOfMessages = 1)
        {
            var messages = new List<MessageInfo>();
            QueueData queueData;

            if (this.internalQueue.TryGetValue(name, out queueData))
            {
                lock (queueData)
                {
                    foreach (
                        MessageInfo messageData in
                            queueData.Messages.Where(x => x.Value.IsValid(queueData.Queue.VisibilityTimeout))
                                .Take(maxNumberOfMessages)
                                .Select(x => x.Value))
                    {
                        messageData.LastAccessTimestamp = DateTime.UtcNow;
                        messageData.ApproximateReceiveCount += 1;
                        messages.Add(messageData);
                    }
                }
            }

            return messages;
        }

        /// <summary>
        ///     Delivers a message to the specified queue.
        /// </summary>
        /// <param name="name">The name of the queue.</param>
        /// <param name="message">The message to send.</param>
        /// <param name="delay">The delay.</param>
        /// <returns>The message ID of the message sent to the queue.</returns>
        public string SendMessage(string name, string message, int? delay = null)
        {
            QueueData queueData;
            if (this.internalQueue.TryGetValue(name, out queueData))
            {
                string id = Guid.NewGuid().ToStringValue();

                if (!delay.HasValue)
                {
                    delay = queueData.Queue.Delay;
                }

                MessageInfo messageData = queueData.Messages.GetOrAdd(
                    id,
                    new MessageInfo(id, message, DateTime.UtcNow, DateTime.UtcNow.AddSeconds(delay.Value)));
                return messageData.ID;
            }

            return null;
        }
  
        private class QueueData
        {
            public QueueData(QueueInfo queue)
            {
                this.Queue = queue;
                this.Messages = new ConcurrentDictionary<string, MessageInfo>(StringComparer.OrdinalIgnoreCase);
            }

            public ConcurrentDictionary<string, MessageInfo> Messages { get; private set; }

            public QueueInfo Queue { get; private set; }
        }
    }
}