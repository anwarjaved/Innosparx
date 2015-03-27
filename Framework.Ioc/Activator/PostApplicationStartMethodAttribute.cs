namespace Framework.Activator
{
    using System;

    /// -------------------------------------------------------------------------------------------------
    /// <summary>
    /// Same as PreApplicationStartMethodAttribute, but for methods to be called after App_Start.
    /// </summary>
    /// <remarks>
    /// LM ANWAR, 6/2/2013.
    /// </remarks>
    /// -------------------------------------------------------------------------------------------------
    [AttributeUsage(AttributeTargets.Assembly, AllowMultiple = true)]
    public sealed class PostApplicationStartMethodAttribute : BaseActivationMethodAttribute
    {
        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        /// Initializes a new instance of the PostApplicationStartMethodAttribute class.
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
        public PostApplicationStartMethodAttribute(Type type, string methodName)
            : base(type, methodName)
        {
        }
    }
}
