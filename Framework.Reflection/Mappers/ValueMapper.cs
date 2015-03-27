namespace Framework.Reflection.Mappers
{
    using System;

    using Framework.Ioc;

    /// <summary>
    /// Class ValueMapper.
    /// </summary>
    [InjectBind(typeof(IObjectMapper), "ValueMapper")]
    public class ValueMapper : BaseMapper
    {
        /// <summary>
        /// Maps the specified type.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="value">The value.</param>
        /// <returns>Mapped <see cref="object" />.</returns>
        public override object Map(Type type, object value)
        {
            return Convert.ChangeType(value, type);
        }

        /// <summary>
        /// Determines whether this instance can map the specified type.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns><see langword="true" /> if this instance can map the specified type; otherwise, <see langword="false" />.</returns>
        public override bool CanMap(Type type)
        {
            return type.IsValueType && !type.IsEnum && type != typeof(DateTime);
        }
    }
}
