namespace Framework.Activator
{
    using System;

    /// -------------------------------------------------------------------------------------------------
    /// <summary>
    /// Same as PreApplicationStartMethodAttribute, but for methods to be called when the app shuts
    /// down.
    /// </summary>
    /// <remarks>
    /// LM ANWAR, 6/2/2013.
    /// </remarks>
    /// -------------------------------------------------------------------------------------------------
    [AttributeUsage(AttributeTargets.Assembly, AllowMultiple = true)]
    public sealed class ApplicationShutdownMethodAttribute : BaseActivationMethodAttribute
    {
        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        /// Initializes a new instance of the ApplicationShutdownMethodAttribute class.
        /// </summary>
        /// <remarks>
        /// LM ANWAR, 6/2/2013.
        /// </remarks>
        /// <param name="type">
        /// The type to use.
        /// </param>
        /// <param name="methodName">
        /// Name of the method.
        /// </param>
        /// -------------------------------------------------------------------------------------------------
        public ApplicationShutdownMethodAttribute(Type type, string methodName)
            : base(type, methodName)
        {
        }
    }
}
