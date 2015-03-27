namespace Framework.Reflection
{
    using System;
    using System.Collections.Generic;

    /// -------------------------------------------------------------------------------------------------
    /// <summary>
    ///     Interface for reflection property.
    /// </summary>
    /// -------------------------------------------------------------------------------------------------
    public interface IReflectionProperty
    {
        /// <summary>
        /// Gets the attributes.
        /// </summary>
        /// <value>The attributes.</value>
        IReadOnlyList<Attribute> Attributes { get; }

        /// <summary>
        /// Gets the name.
        /// </summary>
        /// <value>The name.</value>
        string Name { get; }

        /// <summary>
        /// Gets the type.
        /// </summary>
        /// <value>The Type.</value>
        Type Type { get; }

        /// <summary>
        /// Gets a value indicating whether this instance is primitive.
        /// </summary>
        /// <value>true if this object is simple type, false if not.</value>
        bool IsPrimitive { get; }

        /// <summary>
        /// Gets a value indicating whether this instance is enumerable.
        /// </summary>
        /// <value>true if this object is enumerable, false if not.</value>
        bool IsEnumerable { get; }

        /// <summary>
        /// Gets a value indicating whether this object is dictionary.
        /// </summary>
        /// <value>true if this object is dictionary, false if not.</value>
        bool IsDictionary { get; }

        /// <summary>
        /// Gets the type of the enumerable.
        /// </summary>
        /// <value>The type of the enumerable.</value>
        Type EnumerableType { get; }

        /// <summary>
        /// Gets the type of the key.
        /// </summary>
        /// <value>The type of the key.</value>
        Type KeyType { get; }

        /// <summary>
        /// Gets a value indicating whether this object is <see cref="Nullable"/> type.
        /// </summary>
        /// <value>true if this object is <see cref="Nullable"/> type, false if not.</value>
        bool IsNullable { get; }

        /// <summary>
        /// Gets a value indicating whether this object is class.
        /// </summary>
        /// <value>true if this object is class, false if not.</value>
        bool IsClass { get; }

        /// <summary>
        /// Gets a value indicating whether we can read.
        /// </summary>
        /// <value>true if we can read, false if not.</value>
        bool CanRead { get; }

        /// <summary>
        /// Gets a value indicating whether we can write.
        /// </summary>
        /// <value>true if we can write, false if not.</value>
        bool CanWrite { get; }

        /// <summary>
        /// Gets the value.
        /// </summary>
        /// <typeparam name="T">Generic type parameter.</typeparam>
        /// <param name="instance">The instance.</param>
        /// <returns>Value of the instance property.</returns>
        T Get<T>(object instance = null);

        /// <summary>
        /// Gets the value.
        /// </summary>
        /// <param name="instance">The instance.</param>
        /// <returns>Value of the instance property.</returns>
        object Get(object instance = null);

        /// <summary>
        /// Sets the property value.
        /// </summary>
        /// <param name="instance">The instance.</param>
        /// <param name="value">The value.</param>
        void Set(object instance, object value);

        /// <summary>
        /// Sets the property value.
        /// </summary>
        /// <param name="value">The value.</param>
        void Set(object value);
    }
}
