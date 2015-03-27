namespace Framework.Ioc
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// Describes the target of a binding.
    /// </summary>
    public enum BindingType
    {
        /// <summary>
        /// Indicates that the binding is from a type to itself.
        /// </summary>
        Self = 0,

        /// <summary>
        /// Indicates that the binding is from one type to another.
        /// </summary>
        Type = 1,
    }
}
