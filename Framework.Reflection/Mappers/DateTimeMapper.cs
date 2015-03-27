namespace Framework.Reflection.Mappers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    using Framework.Ioc;

    /// <summary>
    /// Class DateTimeMapper.
    /// </summary>
    [InjectBind(typeof(IObjectMapper), "DateTimeMapper")]
    public class DateTimeMapper : BaseMapper
    {
        /// <summary>
        /// Maps the specified type.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="value">The value.</param>
        /// <returns>Mapped <see cref="object" />.</returns>
        /// <exception cref="System.FormatException">cannot convert '{0}' to DateTime.FormatString(dateValue)</exception>
        public override object Map(Type type, object value)
        {
            W3CDateTime dateTime;
            string dateValue = Convert.ToString(value);

            if (W3CDateTime.TryParse(dateValue, out dateTime))
            {
                return dateTime.LocalDateTime;
            }
        
            DateTime date;

            if (DateTime.TryParse(dateValue, out date))
            {
                return date;
            }

            throw new FormatException("cannot convert '{0}' to DateTime".FormatString(dateValue));
        }

        /// <summary>
        /// Determines whether this instance can map the specified type.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns><see langword="true" /> if this instance can map the specified type; otherwise, <see langword="false" />.</returns>
        public override bool CanMap(Type type)
        {
            return type.IsValueType && type == typeof(DateTime);
        }
    }
}
