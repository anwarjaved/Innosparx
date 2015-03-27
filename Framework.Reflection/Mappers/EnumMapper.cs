namespace Framework.Reflection.Mappers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    using Framework.Ioc;

    /// <summary>
    ///  Mapper to map <see cref="Enum"/>.
    /// </summary>
    [InjectBind(typeof(IObjectMapper), "EnumMapper")]
    public class EnumMapper : BaseMapper
    {
        /// <summary>
        /// Maps the specified type.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="value">The value.</param>
        /// <returns>Mapped <see cref="object" />.</returns>
        public override object Map(Type type, object value)
        {
            return Enum.Parse(type, Convert.ToString(value));
        }

        /// <summary>
        /// Determines whether this instance can map the specified type.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns><see langword="true" /> if this instance can map the specified type; otherwise, <see langword="false" />.</returns>
        public override bool CanMap(Type type)
        {
            return type.IsEnum;
        }
    }
}
