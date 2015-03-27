namespace Framework.MessageQueue.Impl
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity.SqlServer;
    using System.Linq;

    using Framework.DataAccess;
    using Framework.Domain;
    using Framework.Ioc;

    /// <summary>
    /// Represent a Sql based Message Queue.
    /// </summary>
    [InjectBind(typeof(IQueue), "SqlQueue", LifetimeType.Singleton)]
    public class SqlQueue : IQueue
    {
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
        public void CreateQueue(string name, int delay = 0, int messageRetentionPeriod = 1209600, int visibilityTimeout = 90)
        {
            IUnitOfWork unitOfWork = Container.Get<IUnitOfWork>();

            IRepository<MessageQueue> repository = unitOfWork.Get<MessageQueue>();

            if (!repository.Exists(x => x.Name == name))
            {
                MessageQueue msgQueue = new MessageQueue()
                                            {
                                                CreatedTimestamp = DateTime.UtcNow,
                                                Delay = delay,
                                                MessageRetentionPeriod = messageRetentionPeriod,
                                                Name = name,
                                                VisibilityTimeout = visibilityTimeout
                                            };

                repository.Add(msgQueue);
                unitOfWork.Commit();
            }
        }

        /// <summary>
        ///     The DeleteMessage action deletes the specified message from the specified queue.
        /// </summary>
        /// <param name="name">The name of the queue.</param>
        /// <param name="messageID">The message identifier.</param>
        public void DeleteMessage(string name, string messageID)
        {
            IUnitOfWork unitOfWork = Container.Get<IUnitOfWork>();

            IRepository<QueueMessage> repository = unitOfWork.Get<QueueMessage>();

            Guid id = new Guid(messageID);

            QueueMessage message = repository.One(x => x.ID == id);

            if (message != null)
            {
                repository.Remove(message);
                unitOfWork.Commit();
            }
        }

        /// <summary>
        ///     The DeleteMessage action deletes the queue, regardless of whether the queue is empty.
        /// </summary>
        /// <param name="name">The name of the queue.</param>
        public void DeleteQueue(string name)
        {
            IUnitOfWork unitOfWork = Container.Get<IUnitOfWork>();

            IRepository<MessageQueue> repository = unitOfWork.Get<MessageQueue>();

            MessageQueue queue = repository.One(x => x.Name == name);

            if (queue != null)
            {
                repository.Remove(queue);
                unitOfWork.Commit();
            }
        }

        /// <summary>
        ///     Gets a list of your queues.
        /// </summary>
        /// <returns>An collection of <see cref="IReadOnlyCollection{QueueInfo}" /> containing info of queue.</returns>
        public IReadOnlyCollection<QueueInfo> GetAllQueue()
        {
            IUnitOfWork unitOfWork = Container.Get<IUnitOfWork>();

            IRepository<MessageQueue> repository = unitOfWork.Get<MessageQueue>();

            return repository.Query.ToList().Select(x => new QueueInfo(x.Name, x.Delay, x.MessageRetentionPeriod, x.CreatedTimestamp, x.VisibilityTimeout)).ToList();
        }

        /// <summary>
        ///     The ReceiveMessage action retrieves one or more messages from the specified queue.
        /// </summary>
        /// <param name="name">The name of the queue.</param>
        /// <param name="maxNumberOfMessages">Maximum number of messages to return.</param>
        /// <returns>An collection of <see cref="IReadOnlyCollection{Message}" />.</returns>
        public IReadOnlyCollection<MessageInfo> ReceiveMessage(string name, int maxNumberOfMessages = 1)
        {
            List<MessageInfo> list = new List<MessageInfo>();
            IUnitOfWork unitOfWork = Container.Get<IUnitOfWork>();

            IRepository<QueueMessage> repository = unitOfWork.Get<QueueMessage>();
            IRepository<MessageQueue> queueRepository = unitOfWork.Get<MessageQueue>();

            IQueryable<QueueMessage> queryable = repository.Query.Where(
                x =>
                x.QueueName == name && x.ApproximateFirstReceiveTimestamp <= DateTime.UtcNow
                && (x.LastAccessTimestamp == null
                    || (SqlFunctions.DateDiff("second", x.LastAccessTimestamp, DateTime.UtcNow)
                        > x.Queue.VisibilityTimeout))).Take(maxNumberOfMessages);
            List<QueueMessage> messages = queryable.ToList();

            foreach (var queueMessage in messages)
            {
                queueMessage.LastAccessTimestamp = DateTime.UtcNow;
                queueMessage.ApproximateReceiveCount += 1;

                list.Add(new MessageInfo(queueMessage.ID.ToStringValue(), queueMessage.Body, queueMessage.SentTimestamp, queueMessage.ApproximateFirstReceiveTimestamp, queueMessage.ApproximateReceiveCount, queueMessage.LastAccessTimestamp));
            }

            unitOfWork.Commit();

            return list;
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
            IUnitOfWork unitOfWork = Container.Get<IUnitOfWork>();

            IRepository<QueueMessage> repository = unitOfWork.Get<QueueMessage>();
            IRepository<MessageQueue> queueRepository = unitOfWork.Get<MessageQueue>();

            var queue = queueRepository.One(x => x.Name == name);
            if (queue != null)
            {
                if (!delay.HasValue)
                {
                    delay = queue.Delay;
                }

                QueueMessage queueMessage = new QueueMessage()
                                                {
                                                    ApproximateFirstReceiveTimestamp =
                                                        DateTime.UtcNow.AddSeconds(delay.Value),
                                                    Body = message,
                                                    Queue = queue,
                                                    SentTimestamp = DateTime.UtcNow
                                                };

                repository.Save(queueMessage);
                unitOfWork.Commit();

                return queueMessage.ID.ToStringValue();
            }

            return null;
        }
    }
}
