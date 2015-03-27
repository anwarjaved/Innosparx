namespace Framework.Reflection.Impl
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Reflection;
    using System.Text;
    using System.Threading.Tasks;

    using Framework.Ioc;
    using Framework.Reflection.Mappers;

    internal class ReflectionProperty : IReflectionProperty
    {
        private readonly PropertyInfo property;

        private readonly Func<object, object> getAccessor;

        private readonly Action<object, object> setAccesor;

        private readonly Lazy<IReadOnlyList<Attribute>> attributes;

        public ReflectionProperty(PropertyInfo property)
        {
            this.property = property;
            this.attributes = new Lazy<IReadOnlyList<Attribute>>(() => this.property.GetCustomAttributes(false).Cast<Attribute>().ToList());
            ParameterExpression instance = Expression.Parameter(typeof(object), "instance");
            UnaryExpression instanceCast = (!this.property.DeclaringType.IsValueType)
                                             ? Expression.TypeAs(instance, this.property.DeclaringType)
                                             : Expression.Convert(instance, this.property.DeclaringType);

            if (this.property.GetMethod != null && this.property.GetMethod.IsPublic)
            {
                if (this.property.CanRead)
                {
                    this.CanRead = true;
                    this.getAccessor = this.property.GetGetMethod().IsStatic
                                           ? Expression.Lambda<Func<object, object>>(
                                               Expression.TypeAs(Expression.Call(null, this.property.GetGetMethod()), typeof(object)), instance).Compile()
                                           : Expression.Lambda<Func<object, object>>(
                                               Expression.TypeAs(Expression.Call(instanceCast, this.property.GetGetMethod()), typeof(object)), instance).Compile();
                }
            }

            if (this.property.SetMethod != null && this.property.SetMethod.IsPublic)
            {
                ParameterExpression value = Expression.Parameter(typeof(object), "value");
                UnaryExpression valueCast = (!this.property.PropertyType.IsValueType)
                                        ? Expression.TypeAs(value, this.property.PropertyType)
                                        : Expression.Convert(value, this.property.PropertyType);

                if (this.property.CanWrite)
                {
                    this.CanWrite = true;
                    this.setAccesor = this.property.GetSetMethod().IsStatic
                                          ? Expression.Lambda<Action<object, object>>(Expression.Call(null, this.property.GetSetMethod(), valueCast), new[] { instance, value })
                                                      .Compile()
                                          : Expression.Lambda<Action<object, object>>(
                                              Expression.Call(instanceCast, this.property.GetSetMethod(), valueCast), new[] { instance, value }).Compile();
                }
            }

            this.SetPropertyInfo(this.property.PropertyType);
        }

        public bool IsPrimitive { get; private set; }

        public bool IsEnumerable { get; private set; }

        public bool IsDictionary { get; private set; }

        public Type EnumerableType { get; private set; }

        public Type KeyType { get; private set; }

        public bool IsNullable { get; private set; }

        public bool IsClass { get; private set; }

        public bool CanRead { get; private set; }

        public bool CanWrite { get; private set; }
        
        public string Name
        {
            get
            {
                return this.property.Name;
            }
        }

        public IReadOnlyList<Attribute> Attributes
        {
            get
            {
                return this.attributes.Value;
            }
        }

        public Type Type
        {
            get
            {
                if (this.IsNullable)
                {
                    return Nullable.GetUnderlyingType(this.property.PropertyType);
                }

                return this.property.PropertyType;
            }
        }

        public T Get<T>(object instance = null)
        {
            var accessor = this.getAccessor;
            if (accessor != null)
            {
                return (T)accessor(instance);
            }

            return default(T);
        }

        public object Get(object instance = null)
        {
            var accessor = this.getAccessor;
            if (accessor != null)
            {
                return accessor(instance);
            }

            return null;
        }

        public void Set(object instance, object value)
        {
            var accesor = this.setAccesor;
            if (accesor != null)
            {
                accesor(instance, this.Map(value));
            }
        }

        public void Set(object value)
        {
            var accesor = this.setAccesor;
            if (accesor != null)
            {
                accesor(null, this.Map(value));
            }
        }

        private void SetPropertyInfo(Type type)
        {
            this.IsNullable = type.IsNullableType();

            this.IsPrimitive = type.IsPrimitiveType();

            if (!this.IsPrimitive)
            {
                if (type.IsEnumerable())
                {
                    this.IsEnumerable = true;
                    if (type.IsArray || type == typeof(ArrayList))
                    {
                        if (type.HasElementType)
                        {
                            this.EnumerableType = type.GetElementType();
                        }
                    }
                    else if (type.IsDictionary())
                    {
                        this.IsDictionary = true;
                        Type[] genericArguments = type.GetGenericArguments();

                        this.KeyType = genericArguments[0];
                        this.EnumerableType = genericArguments[1];
                    }
                    else
                    {
                        Type[] genericArguments = type.GetGenericArguments();
                        this.EnumerableType = genericArguments[0];
                    }
                }
                else if (type.IsClass)
                {
                    this.IsClass = true;
                }
            }
        }

        private object Map(object value)
        {
            var mappers = Container.GetAll<IObjectMapper>();

            foreach (var mapper in mappers)
            {
                if (mapper.CanMap(this.Type))
                {
                    return mapper.Map(this.Type, value);
                }
            }

            return value;
        }
    }
}
