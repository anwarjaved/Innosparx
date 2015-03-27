using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework.Notification
{
    ///-------------------------------------------------------------------------------------------------
    /// <summary>
    ///     A property changed message.
    /// </summary>
    ///
    /// <remarks>
    ///     Anwar Javed, 04/01/2014 2:00 PM.
    /// </remarks>
    ///-------------------------------------------------------------------------------------------------
    public class PropertyChangedMessage : BaseMessage
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PropertyChangedMessage" /> class.
        /// </summary>
        /// <param name="propertyName">The name of the property that changed.</param>
        protected PropertyChangedMessage(string propertyName)
        {
            this.PropertyName = propertyName;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PropertyChangedMessage" /> class.
        /// </summary>
        /// <param name="sender">The message's sender.</param>
        /// <param name="propertyName">The name of the property that changed.</param>
        protected PropertyChangedMessage(object sender, string propertyName)
            : base(sender)
        {
            this.PropertyName = propertyName;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PropertyChangedMessage" /> class.
        /// </summary>
        /// <param name="sender">The message's sender.</param>
        /// <param name="target">The message's intended target. This parameter can be used
        /// to give an indication as to whom the message was intended for. Of course
        /// this is only an indication, amd may be null.</param>
        /// <param name="propertyName">The name of the property that changed.</param>
        protected PropertyChangedMessage(object sender, object target, string propertyName)
            : base(sender, target)
        {
            this.PropertyName = propertyName;
        }

        /// <summary>
        /// Gets or sets the name of the property that changed.
        /// </summary>
        public string PropertyName { get; protected set; }

    }
}
