using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework.DataAccess.Impl
{
    using System.Collections.Concurrent;
    using System.Reflection;

    using Framework.Domain;
    using Framework.Ioc;

    [InjectBind(typeof(IEntityFormatter), "DateTimeKind", LifetimeType.Singleton)]
    public class DateTimeKindFormatter : IEntityFormatter
    {

        private static readonly ConcurrentDictionary<Type, IList<PropertyInfo>> DateTimeProperties = new ConcurrentDictionary<Type, IList<PropertyInfo>>();

        public bool OnLoad(Type type, IBaseEntity entity)
        {
            var properties = GetProperties(type);

            bool propertiesChanged = false;

            foreach (var property in properties)
            {
                var attr = property.GetCustomAttribute<DateTimeFormatAttribute>();
                object propertyValue = property.GetValue(entity, null);

                if (propertyValue != null)
                {
                    DateTime dateTimeValue = (DateTime)propertyValue;
                    if (dateTimeValue.Kind != DateTimeKind.Unspecified)
                    {
                        //Sanity check
                        throw new ArgumentException(
                            "DateTime property kind must be Unspecified, {0}.{1}".FormatString(
                                entity.GetType().FullName,
                                property.Name));
                    }

                    if (attr.IsUtc)
                    {
                        //All DateTimes in database is in UTC
                        dateTimeValue = new DateTime(
                            dateTimeValue.Year,
                            dateTimeValue.Month,
                            dateTimeValue.Day,
                            dateTimeValue.Hour,
                            dateTimeValue.Minute,
                            dateTimeValue.Second,
                            dateTimeValue.Millisecond,
                            DateTimeKind.Utc);
                    }
                    else
                    {
                        //All DateTimes in database is in UTC
                        dateTimeValue = new DateTime(
                            dateTimeValue.Year,
                            dateTimeValue.Month,
                            dateTimeValue.Day,
                            dateTimeValue.Hour,
                            dateTimeValue.Minute,
                            dateTimeValue.Second,
                            dateTimeValue.Millisecond,
                            DateTimeKind.Local);
                    }
                
                    property.SetValue(entity, dateTimeValue);
                    propertiesChanged = true;
                }
            }

            return propertiesChanged;
        }

        public void OnSave(Type type, IBaseEntity entity)
        {
            var properties = GetProperties(type);

            foreach (var property in properties)
            {
                var attr = property.GetCustomAttribute<DateTimeFormatAttribute>();
                object propertyValue = property.GetValue(entity, null);
                if (propertyValue != null)
                {
                    DateTime dateTimeValue = (DateTime)propertyValue;

                    if (dateTimeValue.Kind == DateTimeKind.Unspecified)
                    {
                        throw new ArgumentException(
                            "DateTime property kind must be Local or UTC, {0}. {1}".FormatString(
                                type.FullName,
                                property.Name));
                    }

                    if (attr.IsLocal && dateTimeValue.Kind != DateTimeKind.Local)
                    {
                        property.SetValue(entity, dateTimeValue.ToLocalTime(), null);
                    }

                    if (attr.IsUtc && dateTimeValue.Kind != DateTimeKind.Utc)
                    {
                        property.SetValue(entity, dateTimeValue.ToUniversalTime(), null);
                    }
                }
            }
        }

        private static IEnumerable<PropertyInfo> GetProperties(Type type)
        {
            return DateTimeProperties.GetOrAdd(
                type,
                t =>
                t.GetProperties()
                    .Where(x => (x.PropertyType == typeof(DateTime) || x.PropertyType == typeof(DateTime?)) && x.GetCustomAttribute<DateTimeFormatAttribute>() != null)
                    .ToList());
        }
    }
}
