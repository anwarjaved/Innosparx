namespace Framework
{
    using System;
    using System.Globalization;
    using System.Runtime.Serialization;
    using System.Security;

    /// <summary>
    ///     The exception that is thrown when an error occurs in a disposing method of object.
    /// </summary>
    /// <seealso cref="T:System.Exception"/>
    /// <threadsafety static="true" instance="false"/>
    public class ObjectDisposingException : ArgumentException
    {
        private readonly string objectName;

        /// <summary>
        /// Initializes a new instance of the <see cref="ObjectDisposingException"/> class.
        /// </summary>
        public ObjectDisposingException()
            : this(null, "Cannot dispose object.")
        {
        }

        /// <summary>Initializes a new instance of the <see cref="ObjectDisposingException"></see> class with a string containing the name of the disposing object.</summary>
        /// <param name="objectName">A string containing the name of the disposing object. </param>
        public ObjectDisposingException(string objectName)
            : this(objectName, "Cannot dispose object.")
        {
        }

        /// <summary>Initializes a new instance of the <see cref="ObjectDisposingException"></see> class with a specified error message and a reference to the inner exception that is the cause of this exception.</summary>
        /// <param name="objectName">The name of the disposing object. </param>
        /// <param name="innerException">The exception that is the cause of the current exception. If <paramref name="innerException"/> is not null, the current exception is raised in a catch block that handles the inner exception.</param>
        public ObjectDisposingException(string objectName, Exception innerException)
            : base(null, objectName, innerException)
        {
            this.objectName = objectName;
        }

        /// <summary>Initializes a new instance of the <see cref="ObjectDisposingException"></see> class with the specified object name and message.</summary>
        /// <param name="objectName">The name of the disposing object. </param>
        /// <param name="message">The error message that explains the reason for the exception. </param>
        public ObjectDisposingException(string objectName, string message)
            : base(message, objectName)
        {
            this.objectName = objectName;
        }

        /// <summary>Initializes a new instance of the <see cref="ObjectDisposingException"></see> class with the specified object name and message.</summary>
        /// <param name="objectName">The name of the disposing object. </param>
        /// <param name="message">The error message that explains the reason for the exception. </param>
        /// <param name="innerException">The exception that is the cause of the current exception. If <paramref name="innerException"/> is not null, the current exception is raised in a catch block that handles the inner exception.</param>
        public ObjectDisposingException(string objectName, string message, Exception innerException)
            : base(message, objectName, innerException)
        {
            this.objectName = objectName;
        }

        /// <summary>
        /// Gets the message that describes the error.
        /// </summary>
        /// <value>The message that describes the error.</value>
        /// <returns>A string that describes the error.</returns>
        public override string Message
        {
            get
            {
                if (string.IsNullOrEmpty(this.ObjectName))
                {
                    return base.Message;
                }

                return base.Message + Environment.NewLine
                        +
                        string.Format(
                            CultureInfo.CurrentCulture,
                            "Cannot dispose object named '{0}'",
                            new object[] { this.ObjectName });
            }
        }

        /// <summary>
        /// Gets the name of the disposing object.
        /// </summary>
        /// <value>The name of the disposing object.</value>
        /// <returns>A string containing the name of the disposing object.</returns>
        public string ObjectName
        {
            get { return this.objectName ?? string.Empty; }
        }
    }
}