using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework.Notification
{
    ///-------------------------------------------------------------------------------------------------
    /// <summary>
    ///     A base message class which can be use to send messages.
    /// </summary>
    ///
    /// <remarks>
    ///     Anwar Javed, 04/01/2014 1:57 PM.
    /// </remarks>
    ///-------------------------------------------------------------------------------------------------
    public abstract class BaseMessage
    {
        /// <summary>
        /// Initializes a new instance of the MessageBase class.
        /// </summary>
        /// <param name="sender">The message's original sender.</param>
        /// <param name="target">The message's intended target. This parameter can be used
        /// to give an indication as to whom the message was intended for. Of course
        /// this is only an indication, amd may be null.</param>
        protected BaseMessage(object sender = null, object target = null)
        {
            this.Sender = sender;

            this.Target = target;
        }

        /// <summary>
        /// Gets or sets the message's sender.
        /// </summary>
        public object Sender { get; protected set; }

        /// <summary>
        /// Gets or sets the message's intended target. This property can be used
        /// to give an indication as to whom the message was intended for. Of course
        /// this is only an indication, amd may be null.
        /// </summary>
        public object Target { get; protected set; }

    }
}
