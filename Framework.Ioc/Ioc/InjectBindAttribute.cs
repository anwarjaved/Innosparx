namespace Framework.Ioc
{
    using System;

    /// <summary>
    /// An <see cref="InjectBindAttribute"/> is used to bind types.
    /// </summary>
    [AttributeUsage(AttributeTargets.Struct | AttributeTargets.Class, AllowMultiple = true, Inherited = true)]
    public sealed class InjectBindAttribute : Attribute, IEquatable<InjectBindAttribute>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="InjectBindAttribute"/> class.
        /// </summary>
        /// <param name="injectedType">The type of injected object.</param>
        public InjectBindAttribute(Type injectedType)
        {
            this.InjectedType = injectedType;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="InjectBindAttribute"/> class.
        /// </summary>
        /// <param name="injectedType">The type of injected object.</param>
        /// <param name="name">The name of registered object.</param>
        public InjectBindAttribute(Type injectedType, string name)
            : this(injectedType)
        {
            this.Name = name;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="InjectBindAttribute"/> class.
        /// </summary>
        /// <param name="injectedType">The type of injected object.</param>
        /// <param name="lifetime">The lifetime.</param>
        public InjectBindAttribute(Type injectedType, LifetimeType lifetime)
            : this(injectedType)
        {
            this.Lifetime = lifetime;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="InjectBindAttribute"/> class.
        /// </summary>
        /// <param name="injectedType">The type of injected object.</param>
        /// <param name="name">The name of registered object.</param>
        /// <param name="lifetime">The lifetime.</param>
        public InjectBindAttribute(Type injectedType, string name, LifetimeType lifetime)
            : this(injectedType, name)
        {
            this.Lifetime = lifetime;
        }

        /// <summary>
        /// Gets the name of registered object.
        /// </summary>
        /// <value>The name of registered object.</value>
        public string Name { get; private set; }

        /// <summary>
        /// Gets the type of injected object.
        /// </summary>
        /// <value>The type of injected object.</value>
        public Type InjectedType { get; private set; }

        /// <summary>
        /// Gets the lifetime.
        /// </summary>
        /// <value>The lifetime.</value>
        public LifetimeType? Lifetime { get; private set; }

        /// <summary>
        /// Indicates whether the current object is equal to another object of the same type.
        /// </summary>
        /// <returns>
        /// true if the current object is equal to the <paramref name="other"/> parameter; otherwise, false.
        /// </returns>
        /// <param name="other">An object to compare with this object.</param>
        public bool Equals(InjectBindAttribute other)
        {
            if (object.ReferenceEquals(null, other))
            {
                return false;
            }

            if (object.ReferenceEquals(this, other))
            {
                return true;
            }

            return object.Equals(other.InjectedType, this.InjectedType)
                   && string.Equals(other.Name, this.Name, StringComparison.OrdinalIgnoreCase);
        }

        /// <summary>
        /// Returns a value that indicates whether this instance is equal to a specified object.
        /// </summary>
        /// <returns>
        /// true if <paramref name="obj"/> equals the type and value of this instance; otherwise, false.
        /// </returns>
        /// <param name="obj">An <see cref="T:System.Object"/> to compare with this instance or null. </param><filterpriority>2</filterpriority>
        public override bool Equals(object obj)
        {
            if (object.ReferenceEquals(null, obj))
            {
                return false;
            }

            return object.ReferenceEquals(this, obj) || this.Equals(obj as InjectBindAttribute);
        }

        /// <summary>
        /// Returns the hash code for this instance.
        /// </summary>
        /// <returns>
        /// A 32-bit signed integer hash code.
        /// </returns>
        /// <filterpriority>2</filterpriority>
        public override int GetHashCode()
        {
            unchecked
            {
                HashCodeUtility codeUtility = new HashCodeUtility();
                codeUtility.AddObject(this.Name.ToLowerInvariant());
                codeUtility.AddObject(this.InjectedType);

                int result = codeUtility.CombinedHash32;
                return result;
            }
        }
    }
}
