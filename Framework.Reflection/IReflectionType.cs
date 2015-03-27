namespace Framework.Reflection
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    /// -------------------------------------------------------------------------------------------------
    /// <summary>
    ///     Interface for reflection class.
    /// </summary>
    /// -------------------------------------------------------------------------------------------------
    public interface IReflectionType
    {
        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        /// Gets the attributes.
        /// </summary>
        /// <value>
        /// The attributes.
        /// </value>
        /// -------------------------------------------------------------------------------------------------
        IReadOnlyList<Attribute> Attributes { get; }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        /// Gets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        /// -------------------------------------------------------------------------------------------------
        string Name { get; }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        /// Gets the type.
        /// </summary>
        /// <value>
        /// The Type.
        /// </value>
        /// -------------------------------------------------------------------------------------------------
        Type Type { get; }

        /// <summary>
        /// Gets the properties.
        /// </summary>
        /// <value>The properties.</value>
        IReadOnlyList<IReflectionProperty> Properties { get; }

        /// <summary>
        /// Gets a property.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns>The property.</returns>
        IReflectionProperty GetProperty(string name);

        /// <summary>
        /// Gets property value.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="instance">(optional) the instance.</param>
        /// <returns>The property value.</returns>
        object GetPropertyValue(string name, object instance = null);

        /// <summary>
        /// Creates the instance.
        /// </summary>
        /// <returns>The new instance&lt; t type&gt;</returns>
        object CreateInstance();
    }
}
