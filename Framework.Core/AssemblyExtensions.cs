namespace Framework
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Linq;
    using System.Reflection;

    /// -------------------------------------------------------------------------------------------------
    /// <summary>
    /// Assembly extensions.
    /// </summary>
    /// <remarks>
    /// LM ANWAR, 6/2/2013.
    /// </remarks>
    /// -------------------------------------------------------------------------------------------------
    [EditorBrowsable(EditorBrowsableState.Never)]
    public static class AssemblyExtensions
    {
        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Enumerates get attributes in this collection.
        /// </summary>
        /// <remarks>
        ///     LM ANWAR, 6/2/2013.
        /// </remarks>
        /// <typeparam name="T">
        ///     Generic type parameter.
        /// </typeparam>
        /// <param name="assembly">
        ///     The assembly to act on.
        /// </param>
        /// <returns>
        ///     An enumerator that allows foreach to be used to process get attributes&lt; t&gt; in this
        ///     collection.
        /// </returns>
        /// -------------------------------------------------------------------------------------------------
        public static IEnumerable<T> GetAttributes<T>(this Assembly assembly) where T : Attribute
        {
            return assembly.GetCustomAttributes(
                typeof(T),
                inherit: false).OfType<T>();
        }
    }
}
