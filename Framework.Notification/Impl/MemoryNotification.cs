using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework.Notification.Impl
{
    using System.Collections.Concurrent;

    using Framework.Collections;
    using Framework.Ioc;

    ///-------------------------------------------------------------------------------------------------
    /// <summary>
    ///     A memory notification.
    /// </summary>
    ///
    /// <remarks>
    ///     Anwar Javed, 03/31/2014 9:16 PM.
    /// </remarks>
    ///-------------------------------------------------------------------------------------------------
    [InjectBind(typeof(INotification), LifetimeType.Singleton)]
    [InjectBind(typeof(INotification), "MemoryNotification", LifetimeType.Singleton)]
    public class MemoryNotification : INotification
    {
        private readonly MultiKeyDictionary<string, NotificationInfo> topicsDictionary =
        new MultiKeyDictionary<string, NotificationInfo>(StringComparer.OrdinalIgnoreCase);

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
        public string CreateTopic(string topicName)
        {
            if (!this.topicsDictionary.ContainsKey(topicName))
            {
                string uniqueID = Guid.NewGuid().ToCombGuid().ToStringValue();
                this.topicsDictionary.Add(new[] { uniqueID, topicName }, new NotificationInfo()
                                                                           {
                                                                               UniqueID = uniqueID,
                                                                               TopicName = topicName
                                                                           });
            }

            return this.topicsDictionary[topicName].UniqueID;
        }

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
        public string Subscribe<TMessage>(string topicID, Action<TMessage> action)
        {
            if (this.topicsDictionary.ContainsKey(topicID))
            {
                string uniqueID = Guid.NewGuid().ToCombGuid().ToStringValue();

                this.topicsDictionary[topicID].Reciepents.GetOrAdd(uniqueID, new WeakAction<TMessage>(action));

                return uniqueID;
            }

            throw new ArgumentException("The requested topic \"{0}\" does not exist.".FormatString(topicID));
        }

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
        public void Unsubscribe(string subscriptionID)
        {
            foreach (var pair in this.topicsDictionary)
            {
                if (pair.Value.Reciepents.ContainsKey(subscriptionID))
                {
                    IWeakAction action;
                    pair.Value.Reciepents.TryRemove(subscriptionID, out action);
                    break;
                }
            }
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Deletes the topic described by topicID.
        /// </summary>
        ///
        /// <param name="topicID">
        ///     Topic Unique identifier.
        /// </param>
        ///-------------------------------------------------------------------------------------------------
        public void DeleteTopic(string topicID)
        {
            if (topicsDictionary.ContainsKey(topicID))
            {
                topicsDictionary.Remove(topicID);
            }
        }

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
        public void Publish<TMessage>(string topicID, TMessage message)
        {
            if (topicsDictionary.ContainsKey(topicID))
            {
                foreach (var reciepent in topicsDictionary[topicID].Reciepents)
                {
                    try
                    {
                        IWeakAction<TMessage> action = reciepent.Value as IWeakAction<TMessage>;
                        if (action != null)
                        {
                            action.Execute(message);
                        }
                    }
                    catch (Exception)
                    {
                    }
                }
            }
        }
    }
}
