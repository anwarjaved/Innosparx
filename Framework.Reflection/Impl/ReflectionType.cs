namespace Framework.Reflection.Impl
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Reflection;

    using Framework.Dynamic;

    internal class ReflectionType : IReflectionType
    {
        private readonly Type info;

       private readonly Lazy<IReadOnlyList<IReflectionProperty>> properties;

        private readonly Lazy<ObjectActivator> instanceCreator;

        private readonly ExpandedObject expandInstance;

        private readonly Lazy<IReadOnlyList<Attribute>> attributes;
        
        public ReflectionType(Type type)
        {
            this.info = type;
            this.attributes = new Lazy<IReadOnlyList<Attribute>>(() => this.info.GetCustomAttributes(false).Cast<Attribute>().ToList());
            this.properties =
                new Lazy<IReadOnlyList<IReflectionProperty>>(
                    () => this.info.GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static).Where(p => p.GetIndexParameters().Length == 0).Select(p => new ReflectionProperty(p)).ToList());

            this.instanceCreator = new Lazy<ObjectActivator>(() =>
                {
                    ConstructorInfo constructor = GetPublicConstructor(type);

                    return GetActivator(constructor);
                });
        }

        public ReflectionType(ExpandedObject expandInstance)
        {
            this.info = expandInstance.GetType();

            this.expandInstance = expandInstance;
            this.attributes = new Lazy<IReadOnlyList<Attribute>>(() => new List<Attribute>());
            this.properties = new Lazy<IReadOnlyList<IReflectionProperty>>(() => new List<IReflectionProperty>());
        }

        private delegate object ObjectActivator(params object[] args);

        public string Name
        {
            get
            {
                return this.info.Name;
            }
        }

        public Type Type
        {
            get
            {
                return this.info;
            }
        }

        public IReadOnlyList<IReflectionProperty> Properties
        {
            get
            {
                return this.properties.Value.ToReadOnlyList();
            }
        }

        public IReadOnlyList<Attribute> Attributes
        {
            get
            {
                return this.attributes.Value;
            }
        }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        /// Gets a property.
        /// </summary>
        /// <param name="name">
        /// The name.
        /// </param>
        /// <returns>
        /// The property.
        /// </returns>
        /// -------------------------------------------------------------------------------------------------
        public IReflectionProperty GetProperty(string name)
        {
            IReflectionProperty property = this.Properties.FirstOrDefault(p => p.Name == name);
            return property;
        }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        /// Gets property value.
        /// </summary>
        /// <remarks>
        /// Anwar Javed, 09/20/2013 6:50 PM.
        /// </remarks>
        /// <param name="name">
        /// The name.
        /// </param>
        /// <param name="objInstance">
        /// (optional) the instance.
        /// </param>
        /// <returns>
        /// The property value.
        /// </returns>
        /// -------------------------------------------------------------------------------------------------
        public object GetPropertyValue(string name, object objInstance = null)
        {
            if (this.expandInstance != null)
            {
                object result = this.expandInstance[name];
                return result;
            }

            IReflectionProperty property = this.GetProperty(name);

            return property == null ? null : property.Get(objInstance);
        }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        /// Creates the instance.
        /// </summary>
        /// <remarks>
        /// LM ANWAR, 6/3/2013.
        /// </remarks>
        /// <returns>
        /// The new instance&lt; t type&gt;
        /// </returns>
        /// -------------------------------------------------------------------------------------------------
        public object CreateInstance()
        {
            if (this.instanceCreator == null)
            {
                throw new NotSupportedException();
            }

            return this.instanceCreator.Value();
        }

        private static ConstructorInfo GetPublicConstructor(Type type)
        {
            ConstructorInfo constructor = type.GetConstructor(Type.EmptyTypes);

            if (constructor == null)
            {
                throw new MissingMethodException("No Constructor for type {0} could be found.The type should contain exactly one public constructor");
            }

            return constructor;
        }

        private static ObjectActivator GetActivator(ConstructorInfo ctor)
        {
            ParameterInfo[] paramsInfo = ctor.GetParameters();

            // create a single param of type object[]
            ParameterExpression param = Expression.Parameter(typeof(object[]), "args");

            Expression[] argsExp = new Expression[paramsInfo.Length];

            // pick each arg from the params array 
            // and create a typed expression of them
            for (int i = 0; i < paramsInfo.Length; i++)
            {
                Expression index = Expression.Constant(i);
                Type paramType = paramsInfo[i].ParameterType;

                Expression paramAccessorExp = Expression.ArrayIndex(param, index);

                Expression paramCastExp = Expression.Convert(paramAccessorExp, paramType);

                argsExp[i] = paramCastExp;
            }

            // make a NewExpression that calls the
            // ctor with the args we just created
            NewExpression newExp = Expression.New(ctor, argsExp);

            // create a lambda with the New
            // Expression as body and our param object[] as arg
            var lambda = Expression.Lambda<ObjectActivator>(newExp, param);

            // compile it
            ObjectActivator compiled = lambda.Compile();
            return compiled;
        }
    }
}
