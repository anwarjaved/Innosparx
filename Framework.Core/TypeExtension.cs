namespace Framework
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using System.Runtime.CompilerServices;

    /// <summary>
    /// Extension Methods For <see cref="Type"/>.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public static class TypeExtension
    {
        /// <summary>
        /// Check if a type subclasses a generic type.
        /// </summary>
        /// <param name="type">The Type to compare.</param>
        /// <param name="genericType">The suspected base class.</param>
        /// <returns>
        /// True if this is indeed a subclass of the given generic type.
        /// </returns>
        public static bool IsSubclassOfGeneric(this Type type, Type genericType)
        {
            Type baseType = type.BaseType;

            while (baseType != null)
            {
                if (baseType.IsGenericType && baseType.GetGenericTypeDefinition() == genericType)
                {
                    return true;
                }

                baseType = baseType.BaseType;
            }

            return false;
        }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        /// A Type extension method that query if 'extendType' is assignable from.
        /// </summary>
        /// <param name="extendType">
        /// The extendType to act on.
        /// </param>
        /// <param name="baseType">
        /// Type of the base.
        /// </param>
        /// <returns>
        /// true if assignable from, false if not.
        /// </returns>
        /// -------------------------------------------------------------------------------------------------
        public static bool IsAssignable(this Type extendType, Type baseType)
        {
            while (!baseType.IsAssignableFrom(extendType))
            {
                if (extendType == typeof(object))
                {
                    return false;
                }

                if (extendType.IsGenericType && !extendType.IsGenericTypeDefinition)
                {
                    extendType = extendType.GetGenericTypeDefinition();
                }
                else
                {
                    extendType = extendType.BaseType;
                }
            }

            return true;
        }

        /// <summary>
        /// Determines whether [is assignable to generic type] [the specified given type].
        /// </summary>
        /// <param name="givenType">Type of the given.</param>
        /// <param name="genericType">Type of the generic.</param>
        /// <returns><see langword="true" /> if [is assignable to generic type] [the specified given type]; otherwise, <see langword="false" />.</returns>
        public static bool IsAssignableToGenericType(this Type givenType, Type genericType)
        {
            var interfaceTypes = givenType.GetInterfaces();

            foreach (var it in interfaceTypes)
            {
                if (it.IsGenericType && it.GetGenericTypeDefinition() == genericType)
                    return true;
            }

            if (givenType.IsGenericType && givenType.GetGenericTypeDefinition() == genericType)
                return true;

            Type baseType = givenType.BaseType;
            if (baseType == null) return false;

            return IsAssignableToGenericType(baseType, genericType);
        }

        /// <summary>
        /// Is the simple type (string, DateTime, TimeSpan, Decimal, Enumeration or other primitive type)
        /// </summary>
        /// <param name="type">Type to check.</param>
        /// <returns><c>true</c> if <paramref name="type" /> is primitive type; otherwise, <c>false</c>.</returns>
        public static bool IsPrimitiveType(this Type type)
        {
            return (type == typeof(string))
                    || ((type == typeof(DateTime))
                        || ((type == typeof(TimeSpan))
                            || ((type == typeof(decimal))
                                || ((type == typeof(Guid))
                                    || (((type == typeof(Type)) 
                                    || type.IsSubclassOf(typeof(Type))) || (type.IsEnum || type.IsPrimitive))))));
        }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        /// Enumerates get loadable types in this collection.
        /// </summary>
        /// <remarks>
        /// LM ANWAR, 6/3/2013.
        /// </remarks>
        /// <exception cref="ArgumentNullException">
        /// Thrown when one or more required arguments are null.
        /// </exception>
        /// <param name="assembly">
        /// The assembly to act on.
        /// </param>
        /// <returns>
        /// An enumerator that allows foreach to be used to process get loadable types in this
        /// collection.
        /// </returns>
        /// -------------------------------------------------------------------------------------------------
        public static IEnumerable<Type> GetLoadableTypes(this Assembly assembly)
        {
            if (assembly == null)
            {
                throw new ArgumentNullException("assembly");
            }

            try
            {
                return assembly.GetTypes();
            }
            catch (ReflectionTypeLoadException e)
            {
                return e.Types.Where(t => t != null);
            }
        }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        /// Enumerates get exportable loadable types in this collection.
        /// </summary>
        /// <remarks>
        /// LM ANWAR, 6/3/2013.
        /// </remarks>
        /// <exception cref="ArgumentNullException">
        /// Thrown when one or more required arguments are null.
        /// </exception>
        /// <param name="assembly">
        /// The assembly to act on.
        /// </param>
        /// <returns>
        /// An enumerator that allows foreach to be used to process get exportable loadable types in
        /// this collection.
        /// </returns>
        /// -------------------------------------------------------------------------------------------------
        public static IEnumerable<Type> GetExportableLoadableTypes(this Assembly assembly)
        {
            if (assembly == null)
            {
                throw new ArgumentNullException("assembly");
            }

            try
            {
                return assembly.GetExportedTypes();
            }
            catch (ReflectionTypeLoadException e)
            {
                return e.Types.Where(t => t != null);
            }
        }

        /// <summary>
        /// Check whether specified type is enumerable.
        /// </summary>
        /// <param name="type">The type to check.</param>
        /// <returns>True if Type is Enumerable else false.</returns>
        public static bool IsEnumerable(this Type type)
        {
            return typeof(IEnumerable).IsAssignableFrom(type) || typeof(IEnumerable<>).IsAssignableFrom(type);
        }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        /// A Type extension method that query if 'type' is dictionary.
        /// </summary>
        /// <param name="type">
        /// The Type to compare.
        /// </param>
        /// <returns>
        /// true if dictionary, false if not.
        /// </returns>
        /// -------------------------------------------------------------------------------------------------
        public static bool IsDictionary(this Type type)
        {
            return typeof(IDictionary).IsAssignableFrom(type) || typeof(IDictionary<,>).IsAssignableFrom(type);
        }

        /// <summary>
        /// Gets the type of the enumerable.
        /// </summary>
        /// <param name="type">The type to check.</param>
        /// <returns>Enumerable Type if enumerable else child type.</returns>
        public static Type GetEnumerableType(this Type type)
        {
            if (type.IsEnumerable())
            {
                return (from currentType in type.GetInterfaces()
                        where currentType.IsGenericType && currentType.GetGenericTypeDefinition() == typeof(IEnumerable<>)
                        select currentType.GetGenericArguments()[0]).FirstOrDefault();
            }

            return type;
        }

        /// <summary>
        /// Gets the resource stream from inside stream.
        /// </summary>
        /// <param name="type">The type of the resource.</param>
        /// <param name="resourceName">Name of the resource.</param>
        /// <returns>Stream object for the resource else null.</returns>
        public static Stream GetResourceStream(this Type type, string resourceName)
        {
            return type.Assembly.GetManifestResourceStream(resourceName);
        }

        /// <summary>
        /// Determines whether type passed in is a <see cref="Nullable"/> type.
        /// </summary>
        /// <param name="type">Type To check.</param>
        /// <returns>
        /// True if the type passed in is a <see cref="Nullable"/> type, otherwise false.
        /// </returns>
        public static bool IsNullableType(this Type type)
        {
            if ((type == null) || !type.IsValueType)
            {
                return false;
            }

            return type.IsGenericType && (type.GetGenericTypeDefinition() == typeof(Nullable<>));
        }

        /// <summary>
        /// Determines whether specified type is anonymous type.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns><c>true</c> if [is anonymous type] [the specified type]; otherwise, <c>false</c>.</returns>
        public static bool IsAnonymousType(this Type type)
        {
            bool hasCompilerGeneratedAttribute = type.GetCustomAttributes(typeof(CompilerGeneratedAttribute), false).Any();
            bool nameContainsAnonymousType = type.FullName.Contains("AnonymousType");
            bool anonymousType = hasCompilerGeneratedAttribute && nameContainsAnonymousType;

            return anonymousType;
        }
    }
}