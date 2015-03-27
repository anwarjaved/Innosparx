namespace Framework.Ioc
{
    using System;

    /// <summary>
    /// Named Attribute for specify key for dependency resolution.
    /// </summary>
    [AttributeUsage(AttributeTargets.Parameter | AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public class NamedAttribute : Attribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="NamedAttribute"/> class.
        /// </summary>
        /// <param name="name">The type name.</param>
        public NamedAttribute(string name)
        {
            this.Name = name;
        }

        /// <summary>
        /// Gets the name.
        /// </summary>
        /// <value>The type name.</value>
        public string Name { get; private set; }
    }
}
