namespace Framework.MessageQueue
{
    using System;

    /// <summary>
    /// Represent information about Queue.
    /// </summary>
    public class QueueInfo
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="QueueInfo" /> class.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="delay">The delay.</param>
        /// <param name="messageRetentionPeriod">The message retention period.</param>
        /// <param name="createdTimestamp">The created timestamp.</param>
        /// <param name="visibilityTimeout">The visibility timeout.</param>
        public QueueInfo(
            string name,
            int delay,
            int messageRetentionPeriod,
            DateTime createdTimestamp,
            int visibilityTimeout)
        {
            this.Name = name;
            this.Delay = delay;
            this.MessageRetentionPeriod = messageRetentionPeriod;
            this.CreatedTimestamp = createdTimestamp;
            this.VisibilityTimeout = visibilityTimeout;
        }

        /// <summary>
        /// Gets the time when the queue was created.
        /// </summary>
        /// <value>The time when the queue was created.</value>
        public DateTime CreatedTimestamp { get; private set; }

        /// <summary>
        /// Gets the time in seconds that the delivery of all messages in the queue will be delayed..
        /// </summary>
        /// <value>The time in seconds that the delivery of all messages in the queue will be delayed.</value>
        public int Delay { get; private set; }

        /// <summary>
        /// Gets the number of seconds queue retains a message.
        /// </summary>
        /// <value>The number of seconds queue retains a message.</value>
        public int MessageRetentionPeriod { get; private set; }

        /// <summary>
        /// Gets the name to use for the queue created.
        /// </summary>
        /// <value>The name to use for the queue created.</value>
        public string Name { get; private set; }

        /// <summary>
        /// Gets or sets the length of time, in seconds, that a message received from a queue will be invisible to other receiving components when they ask to receive messages.
        /// </summary>
        /// <value>The length of time, in seconds, that a message received from a queue will be invisible to other receiving components when they ask to receive messages.</value>
        public int VisibilityTimeout { get; set; }
    }
}