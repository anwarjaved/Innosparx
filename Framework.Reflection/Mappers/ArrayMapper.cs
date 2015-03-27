namespace Framework.Reflection.Mappers
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Text;
    using System.Threading.Tasks;

    using Framework.Ioc;

    using Newtonsoft.Json.Linq;

    /// <summary>
    /// Class ArrayMapper.
    /// </summary>
    [InjectBind(typeof(IObjectMapper), "ArrayMapper")]
    public class ArrayMapper : BaseMapper
    {
        private static readonly Type ArrayType = typeof(Array);

        /// <summary>
        /// Maps the specified type.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="value">The value.</param>
        /// <returns>Mapped <see cref="object" />.</returns>
        public override object Map(Type type, object value)
        {
            Type elementType = type.GetElementType();
            List<System.Linq.Expressions.Expression> values = new List<System.Linq.Expressions.Expression>();

            if (value.GetType() == typeof(JArray))
            {
                JArray jsonArray = (JArray)value;

                foreach (JValue item in jsonArray.AsJEnumerable())
                {
                    UnaryExpression valueCast = (!elementType.IsValueType)
                              ? Expression.TypeAs(Expression.Constant(item.ToString(CultureInfo.CurrentCulture)), elementType)
                              : Expression.Convert(Expression.Constant(item.ToString(CultureInfo.CurrentCulture)), elementType);

                    values.Add(valueCast);
                }
            }
            else if (value.GetType() == typeof(IEnumerable))
            {
                IEnumerable enumerable = (IEnumerable)value;

                foreach (var item in enumerable)
                {
                    UnaryExpression valueCast = (!elementType.IsValueType)
                              ? Expression.TypeAs(Expression.Constant(item.ToString()), elementType)
                              : Expression.Convert(Expression.Constant(item.ToString()), elementType);

                    values.Add(valueCast);
                }
            }

            System.Linq.Expressions.NewArrayExpression newArrayExpression =
                System.Linq.Expressions.Expression.NewArrayInit(elementType, values);

            var func = Expression.Lambda<Func<object>>(newArrayExpression).Compile();
            return func();
        }

        /// <summary>
        /// Determines whether this instance can map the specified type.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns><see langword="true" /> if this instance can map the specified type; otherwise, <see langword="false" />.</returns>
        public override bool CanMap(Type type)
        {
            return BaseMapper.IsMatching(type, t => ArrayType.IsAssignableFrom(t));
        }
    }
}
