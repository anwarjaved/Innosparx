namespace Framework.Reflection.Mappers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    /// <summary>
    /// Base Interface for Mappers.
    /// </summary>
    public interface IObjectMapper
    {
        /// <summary>
        /// Maps the specified type.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="value">The value.</param>
        /// <returns>Mapped <see cref="object"/>.</returns>
        object Map(Type type, object value);

        /// <summary>
        /// Determines whether this instance can map the specified type.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns><see langword="true" /> if this instance can map the specified type; otherwise, <see langword="false" />.</returns>
        bool CanMap(Type type);
    }
}
