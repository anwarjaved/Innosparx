namespace Framework.Reflection.Mappers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    /// <summary>
    /// Base Class for Mapper.
    /// </summary>
    public abstract class BaseMapper : IObjectMapper
    {
        /// <summary>
        /// Maps the specified type.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="value">The value.</param>
        /// <returns>Mapped <see cref="object"/>.</returns>
        public abstract object Map(Type type, object value);

        /// <summary>
        /// Determines whether this instance can map the specified type.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns><see langword="true" /> if this instance can map the specified type; otherwise, <see langword="false" />.</returns>
        public abstract bool CanMap(Type type);

        /// <summary>
        /// Determines whether the specified matching type is matching.
        /// </summary>
        /// <param name="matchingType">Type of the matching.</param>
        /// <param name="matcher">The matcher.</param>
        /// <returns><see langword="true" /> if the specified matching type is matching; otherwise, <see langword="false" />.</returns>
        protected static bool IsMatching(
            Type matchingType,
            Predicate<Type> matcher)
        {
            while (matchingType != null)
            {
                if (matchingType.IsGenericType)
                {
                    var definationType = matchingType
                        .GetGenericTypeDefinition();

                    if (matcher(definationType))
                    {
                        return true;
                    }
                }
                else
                {
                    if (matcher(matchingType))
                    {
                        return true;
                    }
                }

                matchingType = matchingType.BaseType;
            }

            return false;
        }
    }
}
