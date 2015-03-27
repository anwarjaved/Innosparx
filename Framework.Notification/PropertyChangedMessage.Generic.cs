using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework.Notification
{
    ///-------------------------------------------------------------------------------------------------
    /// <summary>
    ///     Passes a string property name (PropertyName) and a generic value
    ///     (<see cref="PropertyChangedMessage{T}.OldValue" /> and
    ///     <see cref="PropertyChangedMessage{T}.NewValue" />) to a
    ///     recipient. This message type can be used to propagate a PropertyChanged event to a
    ///     recipient using the messenging system.
    /// </summary>
    ///
    /// <remarks>
    ///     Anwar Javed, 04/01/2014 2:00 PM.
    /// </remarks>
    ///
    /// <typeparam name="T">
    ///     The type of the OldValue and NewValue property.
    /// </typeparam>
    ///-------------------------------------------------------------------------------------------------
    public class PropertyChangedMessage<T> : PropertyChangedMessage
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PropertyChangedMessage{T}" /> class.
        /// </summary>
        /// <param name="oldValue">The property's value before the change occurred.</param>
        /// <param name="newValue">The property's value after the change occurred.</param>
        /// <param name="propertyName">The name of the property that changed.</param>
        public PropertyChangedMessage(T oldValue, T newValue, string propertyName)
            : base(propertyName)
        {
            this.OldValue = oldValue;
            this.NewValue = newValue;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PropertyChangedMessage{T}" /> class.
        /// </summary>
        /// <param name="sender">The message's sender.</param>
        /// <param name="oldValue">The property's value before the change occurred.</param>
        /// <param name="newValue">The property's value after the change occurred.</param>
        /// <param name="propertyName">The name of the property that changed.</param>
        public PropertyChangedMessage(object sender, T oldValue, T newValue, string propertyName)
            : base(sender, propertyName)
        {
            this.OldValue = oldValue;
            this.NewValue = newValue;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PropertyChangedMessage{T}" /> class.
        /// </summary>
        /// <param name="sender">The message's sender.</param>
        /// <param name="target">The message's intended target. This parameter can be used
        /// to give an indication as to whom the message was intended for. Of course
        /// this is only an indication, amd may be null.</param>
        /// <param name="oldValue">The property's value before the change occurred.</param>
        /// <param name="newValue">The property's value after the change occurred.</param>
        /// <param name="propertyName">The name of the property that changed.</param>
        public PropertyChangedMessage(object sender, object target, T oldValue, T newValue, string propertyName)
            : base(sender, target, propertyName)
        {
            this.OldValue = oldValue;
            this.NewValue = newValue;
        }

        /// <summary>
        /// Gets the value that the property has after the change.
        /// </summary>
        public T NewValue { get; private set; }

        /// <summary>
        /// Gets the value that the property had before the change.
        /// </summary>
        public T OldValue { get; private set; }
    }
}
