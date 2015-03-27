namespace Framework.Ioc
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// -------------------------------------------------------------------------------------------------
    /// <summary>
    ///     Interface for binding initializer.
    /// </summary>
    /// -------------------------------------------------------------------------------------------------
    public interface IBindingInitializer
    {
        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Initializes this object.
        /// </summary>
        /// -------------------------------------------------------------------------------------------------
        void Initialize();
    }
}
