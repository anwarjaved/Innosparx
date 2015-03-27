namespace Framework.Activator
{
    using System;

    /// -------------------------------------------------------------------------------------------------
    /// <summary>
    /// This attribute is similar to its System.Web namesake, except that it can be used multiple
    /// times on an assembly.
    /// </summary>
    /// <remarks>
    /// LM ANWAR, 6/2/2013.
    /// </remarks>
    /// -------------------------------------------------------------------------------------------------
    [AttributeUsage(AttributeTargets.Assembly, AllowMultiple = true)]
    public sealed class PreApplicationStartMethodAttribute : BaseActivationMethodAttribute
    {
        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        /// Initializes a new instance of the PreApplicationStartMethodAttribute class.
        /// </summary>
        /// <remarks>
        /// LM ANWAR, 6/2/2013.
        /// </remarks>
        /// <param name="type">
        /// The type.
        /// </param>
        /// <param name="methodName">
        /// Name of the method.
        /// </param>
        /// -------------------------------------------------------------------------------------------------
        public PreApplicationStartMethodAttribute(Type type, string methodName)
            : base(type, methodName)
        {
        }
    }
}
