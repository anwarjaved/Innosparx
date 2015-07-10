namespace Framework.Ioc
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Reflection;
    using System.Security;

    using Framework.Activator;

    internal static class DependencyBuilder
    {
        private static readonly MethodInfo GetMethodDefinition = typeof(Container).GetMethod("Get", new[] { typeof(string) });
        private static readonly MethodInfo BindMethodDefinition = typeof(Container).GetMethod("Bind", new[] { typeof(string) });
        private static readonly MethodInfo ContainsMethodDefinition = typeof(Container).GetMethod("Contains", new[] { typeof(string) });

        
        public static void ScanAll()
        {
            List<Assembly> refrencedAssemblies = new List<Assembly>(ActivationManager.Assemblies);

            IEnumerable<Assembly> assemblies = from Assembly assembly in refrencedAssemblies
                                               where
                                                 assembly.GetCustomAttributes(typeof(ContainerAssemblyAttribute), false).Any()
                                               select assembly;

            List<Assembly> postBuildAssemblies = new List<Assembly>();

            foreach (var assembly in assemblies)
            {
                bool postBuildNeeded;

                Scan(assembly, out postBuildNeeded);

                if (postBuildNeeded)
                {
                    postBuildAssemblies.Add(assembly);
                }
            }

            if (postBuildAssemblies.Count > 0)
            {
                PostBuildScan(postBuildAssemblies);
            }
        }

        public static void PostBuildScan(IReadOnlyList<Assembly> assemblies)
        {

            var dependencyBuilder = Container.Get<IBindingDependencyBuilder>();
            var bindingBuilders = Container.TryGetAll<IAssemblyBindingBuilder>();

            foreach (var builder in bindingBuilders)
            {
                builder.Build(dependencyBuilder, assemblies);
            }
        }

        public static Func<object> GetInstance(Type injectType)
        {
            ConstructorInfo constructor = GetPublicConstructor(injectType);

            Expression[] constructorArgumentCalls = BuildGetInstanceCallsForConstructor(constructor);

            Expression<Func<object>> newServiceTypeMethod = Expression.Lambda<Func<object>>(
                Expression.New(constructor, constructorArgumentCalls), new ParameterExpression[0]);

            return newServiceTypeMethod.Compile();
        }

        public static Func<TType> GetInstance<TType>()
        {
            Type injectType = typeof(TType);
            ConstructorInfo constructor = GetPublicConstructor(injectType);

            Expression[] constructorArgumentCalls = BuildGetInstanceCallsForConstructor(constructor);

            Expression<Func<TType>> newServiceTypeMethod = Expression.Lambda<Func<TType>>(
                Expression.New(constructor, constructorArgumentCalls), new ParameterExpression[0]);

            return newServiceTypeMethod.Compile();
        }

        public static Func<TType> GetInstance<TType>(Type injectType)
        {
            ConstructorInfo constructor = GetPublicConstructor(injectType);

            Expression[] constructorArgumentCalls = BuildGetInstanceCallsForConstructor(constructor);

            Expression<Func<TType>> newServiceTypeMethod = Expression.Lambda<Func<TType>>(
                Expression.New(constructor, constructorArgumentCalls), new ParameterExpression[0]);

            return newServiceTypeMethod.Compile();
        }

        public static void BindInstance(Type serviceType, Type implmentedType, string name, ILifetime lifetime, int? order = null)
        {
            MethodInfo bindInstanceMethod = MakeGenericBindMethod(serviceType);
            MethodInfo implementInstanceMethod = MakeGenericToMethod(serviceType, implmentedType);

            MethodCallExpression methodCallExpression = Expression.Call(bindInstanceMethod, new Expression[] { Expression.Constant(name, typeof(string)) });

            Func<IBindingInfo> binder = Expression.Lambda<Func<IBindingInfo>>(methodCallExpression).Compile();
            IBindingInfo binding = binder();

            MethodInfo containsMethod = MakeGenericContainsMethod(serviceType);
            MethodCallExpression containsCallExpression = Expression.Call(containsMethod, new Expression[] { Expression.Constant(name, typeof(string)) });
            Func<bool> containsFunc = Expression.Lambda<Func<bool>>(containsCallExpression).Compile();

            if (containsFunc())
            {
                if (order == null)
                {
                    OrderAttribute priorityAttribute = implmentedType.GetCustomAttribute<OrderAttribute>();

                    int priority = priorityAttribute != null ? priorityAttribute.Value : int.MaxValue;

                    if (binding.Order.HasValue && priority < binding.Order.Value)
                    {
                        return;
                    }

                    binding.Order = priority;
                }
                else
                {
                    binding.Order = order;
                }
            }

            MethodCallExpression implementCallExpression = Expression.Call(Expression.Constant(binding), implementInstanceMethod);
            Func<IBindingInfo> implementer = Expression.Lambda<Func<IBindingInfo>>(implementCallExpression).Compile();

            binding = implementer();
            binding.LifetimeManager = lifetime;
        }


        public static void BindInstance(Type serviceType, Type implmentedType, string name, LifetimeType lifetime, int? order = null)
        {
            MethodInfo bindInstanceMethod = MakeGenericBindMethod(serviceType);
            MethodInfo implementInstanceMethod = MakeGenericToMethod(serviceType, implmentedType);

            MethodCallExpression methodCallExpression = Expression.Call(bindInstanceMethod, new Expression[] { Expression.Constant(name, typeof(string)) });

            Func<IBindingInfo> binder = Expression.Lambda<Func<IBindingInfo>>(methodCallExpression).Compile();
            IBindingInfo binding = binder();

            MethodInfo containsMethod = MakeGenericContainsMethod(serviceType);
            MethodCallExpression containsCallExpression = Expression.Call(containsMethod, new Expression[] { Expression.Constant(name, typeof(string)) });
            Func<bool> containsFunc = Expression.Lambda<Func<bool>>(containsCallExpression).Compile();

            if (containsFunc())
            {
                if (order == null)
                {
                    OrderAttribute priorityAttribute = implmentedType.GetCustomAttribute<OrderAttribute>();

                    int priority = priorityAttribute != null ? priorityAttribute.Value : int.MaxValue;

                    if (binding.Order.HasValue && priority < binding.Order.Value)
                    {
                        return;
                    }

                    binding.Order = priority;
                }
                else
                {
                    binding.Order = order;
                }
            }

            MethodCallExpression implementCallExpression = Expression.Call(Expression.Constant(binding), implementInstanceMethod);
            Func<IBindingInfo> implementer = Expression.Lambda<Func<IBindingInfo>>(implementCallExpression).Compile();

            binding = implementer();
            switch (lifetime)
            {
                case LifetimeType.Transient:
                    binding.LifetimeManager = new TransientLifetime();
                    break;
                case LifetimeType.Singleton:
                    binding.LifetimeManager = new SingletonLifetime();
                    break;
                case LifetimeType.Request:
                    binding.LifetimeManager = new RequestLifetime();
                    break;
                case LifetimeType.Session:
                    binding.LifetimeManager = new SessionLifetime();
                    break;
                case LifetimeType.Thread:
                    binding.LifetimeManager = new ThreadLifetime();
                    break;
                default:
                    binding.LifetimeManager = new SingletonLifetime();
                    break;
            }
        }

        public static void Scan(Assembly assembly, out bool postBuildNeeded)
        {
            ContainerAssemblyAttribute containerAssemblyAttribute = assembly.GetCustomAttributes(typeof(ContainerAssemblyAttribute), false).Cast<ContainerAssemblyAttribute>().First();
            postBuildNeeded = false;
            if (containerAssemblyAttribute != null)
            {
                IList<Type> types = assembly.GetExportedTypes().Where(type => type.IsClass && type.GetCustomAttributes(typeof(InjectBindAttribute), false).Any()).ToList();

                foreach (var type in types)
                {
                    Bind(type);
                }

                postBuildNeeded = containerAssemblyAttribute.PostBuild;
            }

        }

        private static void Bind(Type type)
        {
            try
            {
                IEnumerable<InjectBindAttribute> injectAttributes = type.GetCustomAttributes(typeof(InjectBindAttribute), false).OfType<InjectBindAttribute>().ToList();

                foreach (InjectBindAttribute injectAttribute in injectAttributes)
                {
                    BindInstance(injectAttribute.InjectedType, type, injectAttribute.Name, injectAttribute.Lifetime ?? LifetimeType.Singleton);
                }
            }
            catch (Exception exception)
            {
                throw new ActivationException(exception.Message, exception);
            }
        }

        private static ConstructorInfo GetPublicConstructor(Type injectType)
        {
            List<ConstructorInfo> constructors = (from t in injectType.GetConstructors() where t.IsPublic select t).ToList();

            if (constructors.Count == 0)
            {
                throw new ActivationException(string.Format("No registration for type {0} could be found and an implicit registration could not be made. The type should contain exactly one public constructor or one constructor marked with InjectAttribute, but it currently has {0}.", constructors.Count));
            }

            if (constructors.Count == 1)
            {
                return constructors[0];
            }

            List<ConstructorInfo> injectedConstructors =
                (from t in constructors
                 where t.GetCustomAttributes(typeof(InjectAttribute), false).FirstOrDefault() != null
                 select t).ToList();

            if (injectedConstructors.Count > 1 || injectedConstructors.Count == 0)
            {
                throw new ActivationException(string.Format("No registration for type {0} could be found and an implicit registration could not be made. The type should contain exactly one public constructor or one constructor marked with InjectAttribute, but it currently has {0}.", constructors.Count));
            }

            return injectedConstructors[0];
        }

        private static Expression[] BuildGetInstanceCallsForConstructor(ConstructorInfo constructor)
        {
            return
                constructor.GetParameters().Select(BuildGetInstanceCallForParameter).ToArray();
        }

        private static Expression BuildGetInstanceCallForParameter(ParameterInfo parameter)
        {
            Type parameterType = parameter.ParameterType;

            if (parameterType.IsValueType || parameterType == typeof(string))
            {
                return BuildGetValueCallForParameter(parameter);
            }

            string name = null;
            NamedAttribute namedAttribute =
                (from na in parameterType.GetCustomAttributes(typeof(NamedAttribute), false).Cast<NamedAttribute>() select na).FirstOrDefault();

            if (namedAttribute != null)
            {
                name = namedAttribute.Name;
            }

            MethodInfo getInstanceMethod = MakeGenericGetMethod(parameterType);

            return Expression.Call(getInstanceMethod, new Expression[] { Expression.Constant(name, typeof(string)) });
        }

        private static Expression BuildGetValueCallForParameter(ParameterInfo parameter)
        {
            Type parameterType = parameter.ParameterType;

            ValueAttribute valueAttribute =
                (from na in parameter.GetCustomAttributes(typeof(ValueAttribute), false).Cast<ValueAttribute>() select na).FirstOrDefault();

            if (valueAttribute != null)
            {
                return Expression.Constant(valueAttribute.Value, parameterType);
            }

            return Expression.Default(parameterType);
        }

        private static MethodInfo MakeGenericGetMethod(Type parameterType)
        {
            return GetMethodDefinition.MakeGenericMethod(parameterType);
        }

        private static MethodInfo MakeGenericContainsMethod(Type parameterType)
        {
            return ContainsMethodDefinition.MakeGenericMethod(parameterType);
        }

        private static MethodInfo MakeGenericBindMethod(Type parameterType)
        {
            return BindMethodDefinition.MakeGenericMethod(parameterType);
        }

        private static MethodInfo MakeGenericToMethod(Type serviceType, Type methodType)
        {
            Type genericType = typeof(IBindingInfo<>).MakeGenericType(serviceType);
            MethodInfo method = genericType.GetMethod("To");
            return method.MakeGenericMethod(methodType);
        }

    }
}
