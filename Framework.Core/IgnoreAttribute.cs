using System;

namespace Framework
{
    ///-------------------------------------------------------------------------------------------------
    /// <summary>
    ///     Attribute for ignore.
    /// </summary>
    ///-------------------------------------------------------------------------------------------------
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public sealed class IgnoreAttribute : Attribute
    {
    }
}
