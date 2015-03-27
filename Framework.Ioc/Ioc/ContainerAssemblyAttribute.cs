namespace Framework.Ioc
{
    using System;

    /// <summary>
    /// Attribute to mark assemblies for dependency detection.
    /// </summary>
    [AttributeUsage(AttributeTargets.Assembly, AllowMultiple = false)]
    public class ContainerAssemblyAttribute : Attribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ContainerAssemblyAttribute"/> class.
        /// </summary>
        public ContainerAssemblyAttribute()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ContainerAssemblyAttribute"/> class.
        /// </summary>
        /// <param name="lifetime">The lifetime.</param>
        public ContainerAssemblyAttribute(LifetimeType lifetime)
        {
            this.Lifetime = lifetime;
        }

        /// <summary>
        /// Gets or sets the lifetime.
        /// </summary>
        /// <value>The lifetime.</value>
        public LifetimeType? Lifetime { get; set; }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Gets or sets a value indicating whether the post build is needed.
        /// </summary>
        ///
        /// <remarks>
        ///     Anwar Javed, 04/07/2014 9:36 AM.
        /// </remarks>
        ///
        /// <value>
        ///     true if post build, false if not.
        /// </value>
        ///-------------------------------------------------------------------------------------------------
        public bool PostBuild { get; set; }
    }
}
