namespace Framework.Ioc
{
    using System;
    using System.ComponentModel;

    /// <summary>
    /// Contains information about a binding registration.
    /// </summary>
    public interface IBindingInfo
    {
        /// <summary>
        /// Gets the service type that is controlled by the binding.
        /// </summary>
        Type Service { get; }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets the implementation type that is controlled by the binding. </summary>
        ///
        /// <value> The service. </value>
        ///-------------------------------------------------------------------------------------------------

        Type Implementation { get; }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        /// Gets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        /// -------------------------------------------------------------------------------------------------
        string Name { get; }

        /// <summary>
        /// Gets the type of binding.
        /// </summary>
        BindingType BindingType { get; }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        /// Gets the instance.
        /// </summary>
        /// <value>
        /// The instance.
        /// </value>
        /// -------------------------------------------------------------------------------------------------
        Func<object> Instance { get; }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        /// Gets or sets the manager for lifetime.
        /// </summary>
        /// <value>
        /// The lifetime manager.
        /// </value>
        /// -------------------------------------------------------------------------------------------------
        [EditorBrowsable(EditorBrowsableState.Never)]
        ILifetime LifetimeManager { get; set; }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        /// Gets an unique identifier.
        /// </summary>
        /// <value>
        /// The identifier of the unique.
        /// </value>
        /// -------------------------------------------------------------------------------------------------
        string UniqueID { get; }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        /// Gets or sets the priority.
        /// </summary>
        /// <value>
        /// The priority.
        /// </value>
        /// -------------------------------------------------------------------------------------------------
        [EditorBrowsable(EditorBrowsableState.Never)]
        [Browsable(false)]
        int? Order { get; set; }

        /// <summary>
        /// Gets the instance of Dependency object with <see cref="ILifetime"/>.
        /// </summary>
        /// <returns>Instance of Dependency Object from <see cref="ILifetime"/>.</returns>
        [EditorBrowsable(EditorBrowsableState.Never)]
        object GetInstance();

        /// <summary>
        /// Dispose the instance if exists.
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        void ReleaseInstance();

        /// <summary>
        /// Determines whether the specified <see cref="System.Object" /> is equal to this instance.
        /// </summary>
        /// <param name="other">The <see cref="System.Object" /> to compare with this instance.</param>
        /// <returns><c>true</c> if the specified <see cref="System.Object" /> is equal to this instance; otherwise, <c>false</c>.</returns>
        [EditorBrowsable(EditorBrowsableState.Never)]
        bool Equals(object other);

        /// <summary>
        /// Returns a hash code for this instance.
        /// </summary>
        /// <returns>A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table.</returns>
        [EditorBrowsable(EditorBrowsableState.Never)]
        int GetHashCode();

        /// <summary>
        /// Gets the type.
        /// </summary>
        /// <returns>Type of current object.</returns>
        [EditorBrowsable(EditorBrowsableState.Never)]
        Type GetType();

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>A <see cref="System.String" /> that represents this instance.</returns>
        [EditorBrowsable(EditorBrowsableState.Never)]
        string ToString();
    }
}
