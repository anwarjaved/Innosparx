namespace Framework.MessageQueue
{
    using System;

    /// <summary>
    /// Represent a Message.
    /// </summary>
    public class MessageInfo
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MessageInfo" /> class.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="body">The body.</param>
        /// <param name="sentTimestamp">The sent timestamp.</param>
        /// <param name="approximateFirstReceiveTimestamp">The approximate first receive timestamp.</param>
        /// <param name="approximateReceiveCount">The approximate receive count.</param>
        /// <param name="lastAccessTimestamp">The last access timestamp.</param>
        public MessageInfo(string id, string body, DateTime sentTimestamp, DateTime approximateFirstReceiveTimestamp, int approximateReceiveCount = 0, DateTime? lastAccessTimestamp = null)
        {
            this.ID = id;
            this.Body = body;
            this.SentTimestamp = sentTimestamp;
            this.ApproximateFirstReceiveTimestamp = approximateFirstReceiveTimestamp;
            this.ApproximateReceiveCount = approximateReceiveCount;
            this.LastAccessTimestamp = lastAccessTimestamp;
        }

        /// <summary>
        /// Gets or sets the time when the message was first received.
        /// </summary>
        /// <value>The time when the message was first received.</value>
        public DateTime ApproximateFirstReceiveTimestamp { get; protected set; }

        /// <summary>
        /// Gets or sets the number of times a message has been received but not deleted.
        /// </summary>
        /// <value>The number of times a message has been received but not deleted.</value>
        public int ApproximateReceiveCount { get; protected internal set; }

        /// <summary>
        /// Gets or sets the last access timestamp.
        /// </summary>
        /// <value>The last access timestamp.</value>
        public DateTime? LastAccessTimestamp { get; protected internal set; }

        /// <summary>
        ///     Gets the message contents.
        /// </summary>
        /// <value>The body.</value>
        public string Body { get; private set; }

        /// <summary>
        /// Gets the identifier.
        /// </summary>
        /// <value>The identifier.</value>
        public string ID { get; private set; }

        /// <summary>
        ///     Gets the time when the message was sent.
        /// </summary>
        /// <value>The time when the message was sent.</value>
        public DateTime SentTimestamp { get; private set; }

        /// <summary>
        /// Determines whether the message is valid for receive.
        /// </summary>
        /// <param name="visibilityTimeout">The visibility timeout.</param>
        /// <returns><see langword="true" /> if the message is valid for receive; otherwise, <see langword="false" />.</returns>
        public bool IsValid(int visibilityTimeout)
        {
            if (this.ApproximateFirstReceiveTimestamp <= DateTime.UtcNow)
            {
                if (!this.LastAccessTimestamp.HasValue)
                {
                    return true;
                }

                TimeSpan diff = DateTime.UtcNow.Subtract(this.LastAccessTimestamp.Value);
                if (diff.TotalSeconds >= visibilityTimeout)
                {
                    return true;
                }
            }

            return false;
        }
    }
}