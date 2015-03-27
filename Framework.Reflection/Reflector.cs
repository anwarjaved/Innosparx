namespace Framework.Reflection
{
    using System;
    using System.Collections.Concurrent;
    using System.Dynamic;

    using Framework.Dynamic;
    using Framework.Reflection.Impl;

    /// -------------------------------------------------------------------------------------------------
    /// <summary>
    ///     Provides Reflection function..
    /// </summary>
    /// -------------------------------------------------------------------------------------------------
    public static class Reflector
    {
        private static readonly ConcurrentDictionary<Type, IReflectionType> TypesCache = new ConcurrentDictionary<Type, IReflectionType>();

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        /// Gets the Reflected Type.
        /// </summary>
        /// <typeparam name="TType">
        /// Type to fetch reflection info.
        /// </typeparam>
        /// <returns>
        /// An <see cref="IReflectionType"/>instance.
        /// </returns>
        /// -------------------------------------------------------------------------------------------------
        public static IReflectionType Get<TType>()
        {
            return Get(typeof(TType));
        }

        /// <summary>
        /// Gets the Reflected Type.
        /// </summary>
        /// <param name="type">Type to fetch reflection info.</param>
        /// <returns>An <see cref="IReflectionType" />instance.</returns>
        public static IReflectionType Get(Type type)
        {
            if (type == null)
            {
                throw new ArgumentNullException("type");
            }

            if (typeof(IDynamicMetaObjectProvider).IsAssignableFrom(type))
            {
                return new ReflectionType(type);
            }

            if (type.IsAnonymousType())
            {
                return new ReflectionType(type);
            }

            return TypesCache.GetOrAdd(type, t => new ReflectionType(t));
        }

        /// <summary>
        /// Gets the specified instance.
        /// </summary>
        /// <param name="instance">The instance.</param>
        /// <returns>IReflectionType instance.</returns>
        public static IReflectionType Get(ExpandedObject instance)
        {
            if (instance == null)
            {
                throw new ArgumentNullException("instance");
            }

            return new ReflectionType(instance);
        }

        /// <summary>
        /// Gets the specified instance.
        /// </summary>
        /// <param name="instance">The instance.</param>
        /// <returns>IReflectionType instance.</returns>
        public static IReflectionType Get(object instance)
        {
            if (instance == null)
            {
                throw new ArgumentNullException("instance");
            }

            Type type = instance as Type;
            if (type != null)
            {
                return Get(type);
            }

            ExpandedObject expandObject = instance as ExpandedObject;
            if (expandObject != null)
            {
                return new ReflectionType(expandObject);
            }

            return Get(instance.GetType());
        }
    }
}
