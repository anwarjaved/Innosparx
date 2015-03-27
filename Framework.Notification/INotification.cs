using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework.Notification
{
    ///-------------------------------------------------------------------------------------------------
    /// <summary>
    ///     Base Interface for Notification service coordinate and manages the delivery or 
    ///     sending of messages to subscribing endpoints or clients.
    /// </summary>
    ///
    /// <remarks>
    ///     Anwar Javed, 03/31/2014 9:14 PM.
    /// </remarks>
    ///-------------------------------------------------------------------------------------------------
    public interface INotification
    {
        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Creates a topic to which notifications can be published.
        /// </summary>
        ///
        /// <param name="topicName">
        ///    The name of the topic you want to create.
        /// </param>
        ///
        /// <returns>
        ///     The Unique ID assigned to the created topic.
        /// </returns>
        ///-------------------------------------------------------------------------------------------------
        string CreateTopic(string topicName);

        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Subscribes an endpoint.
        /// </summary>
        ///
        /// <typeparam name="TMessage">
        ///     Type of the message.
        /// </typeparam>
        /// <param name="topicID">
        ///     Topic Unique identifier.
        /// </param>
        /// <param name="action">
        ///     The action.
        /// </param>
        ///
        /// <returns>
        ///     The Unique ID assigned to the created subscription.
        /// </returns>
        ///-------------------------------------------------------------------------------------------------
        string Subscribe<TMessage>(string topicID, Action<TMessage> action);

        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Deletes a subscription.
        /// </summary>
        ///
        /// <remarks>
        ///     Anwar Javed, 04/01/2014 10:17 AM.
        /// </remarks>
        ///
        /// <param name="subscriptionID">
        ///     Identifier for the subscription.
        /// </param>
        ///-------------------------------------------------------------------------------------------------
        void Unsubscribe(string subscriptionID);

        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Deletes the topic described by topicID.
        /// </summary>
        ///
        /// <param name="topicID">
        ///     Topic Unique identifier.
        /// </param>
        ///-------------------------------------------------------------------------------------------------
        void DeleteTopic(string topicID);

        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Sends a message to registered recipients. The message will reach all recipients that 
        ///     registered for this message type.
        /// </summary>
        ///
        /// <typeparam name="TMessage">
        ///     The type of message that will be sent.
        /// </typeparam>
        /// <param name="topicID">
        ///     Topic Unique identifier.
        /// </param>
        /// <param name="message">
        ///     The message to send to registered recipients.
        /// </param>
        ///-------------------------------------------------------------------------------------------------
        void Publish<TMessage>(string topicID, TMessage message);
    }
}
