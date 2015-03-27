namespace Framework.MessageQueue
{
    using System.Collections.Generic;

    /// <summary>
    ///     Represent a Message Queue Interface.
    /// </summary>
    public interface IQueue
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
        void CreateQueue(string name, int delay = 0, int messageRetentionPeriod = 1209600, int visibilityTimeout = 90);

        /// <summary>
        ///     The DeleteMessage action deletes the specified message from the specified queue.
        /// </summary>
        /// <param name="name">The name of the queue.</param>
        /// <param name="messageID">The message identifier.</param>
        void DeleteMessage(string name, string messageID);

        /// <summary>
        ///     The DeleteMessage action deletes the queue, regardless of whether the queue is empty.
        /// </summary>
        /// <param name="name">The name of the queue.</param>
        void DeleteQueue(string name);

        /// <summary>
        ///     Gets a list of your queues.
        /// </summary>
        /// <returns>An collection of <see cref="IReadOnlyCollection{QueueInfo}" /> containing info of queue.</returns>
        IReadOnlyCollection<QueueInfo> GetAllQueue();

        /// <summary>
        ///     The ReceiveMessage action retrieves one or more messages from the specified queue.
        /// </summary>
        /// <param name="name">The name of the queue.</param>
        /// <param name="maxNumberOfMessages">Maximum number of messages to return.</param>
        /// <returns>An collection of <see cref="IReadOnlyCollection{Message}" />.</returns>
        IReadOnlyCollection<MessageInfo> ReceiveMessage(string name, int maxNumberOfMessages = 1);

        /// <summary>
        ///     Delivers a message to the specified queue.
        /// </summary>
        /// <param name="name">The name of the queue.</param>
        /// <param name="message">The message to send.</param>
        /// <param name="delay">The delay.</param>
        /// <returns>The message ID of the message sent to the queue.</returns>
        string SendMessage(string name, string message, int? delay = null);
    }
}