using System;

namespace Framework.DataAccess
{
    ///-------------------------------------------------------------------------------------------------
    /// <summary>
    ///     Exception for signalling unit of work errors.
    /// </summary>
    ///-------------------------------------------------------------------------------------------------
    public class UnitOfWorkException : Exception
    {
        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Initializes a new instance of the UnitOfWorkException class.
        /// </summary>
        ///
        /// <param name="message">
        ///     The message.
        /// </param>
        /// <param name="innerException">
        ///     The inner exception.
        /// </param>
        ///-------------------------------------------------------------------------------------------------
        public UnitOfWorkException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
