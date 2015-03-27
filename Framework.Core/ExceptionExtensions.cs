namespace Framework
{
    using System;

    /// <summary>
    /// Exception Extensions.
    /// </summary>
    public static class ExceptionExtensions
    {
        /// <summary>
        /// Throws ArgumentNullException if null.
        /// </summary>
        /// <typeparam name="T">Type of argument.</typeparam>
        /// <param name="argument">The argument.</param>
        /// <param name="name">The argument name.</param>
        public static void ThrowIfNull<T>(this T argument, string name) where T : class
        {
            if (argument == null)
            {
                throw new ArgumentNullException(name);
            }
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Throws ArgumentNullException if null.
        /// </summary>
        ///
        /// <remarks>
        ///     LM ANWAR, 6/2/2013.
        /// </remarks>
        ///
        /// <exception cref="ArgumentNullException">
        ///     Thrown when one or more required arguments are null.
        /// </exception>
        ///
        /// <param name="argument">
        ///     The argument.
        /// </param>
        /// <param name="name">
        ///     The argument name.
        /// </param>
        ///-------------------------------------------------------------------------------------------------
        public static void ThrowIfNull(this string argument, string name)
        {
            if (argument.IsEmpty())
            {
                throw new ArgumentNullException(name);
            }
        }

        public static Exception GetException(this Exception exception)
        {
            Exception innerException = exception;
            while (innerException.InnerException != null)
            {
                innerException = innerException.InnerException;
            }

            return innerException;
        }

        public static string GetExceptionMessage(this Exception exception)
        {
            Exception innerException = exception;
            while (innerException.InnerException != null)
            {
                innerException = innerException.InnerException;
            }

            return innerException != null ? innerException.Message : string.Empty;
        }
    }
}
