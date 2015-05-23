namespace Framework.DataAccess.Impl
{
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Data.Entity;
    using System.Data.Entity.ModelConfiguration;
    using System.Data.Entity.ModelConfiguration.Configuration;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Reflection;
    using System.Reflection.Emit;
    using System.Security;
    using System.Text;
    using System.Threading.Tasks;
    using System.Web.Configuration;

    using Framework.Activator;
    using Framework.Infrastructure;
    using Framework.Ioc;

    
    internal static class EntityContextFactory
    {
        private static AssemblyBuilder dbContextAssemblyBuilder;
        private static ModuleBuilder dbContextModuleBuilder;

        private static readonly Dictionary<string, Func<IEntityContext>> DbContextCache = new Dictionary<string, Func<IEntityContext>>();

        internal static readonly Dictionary<string, IDictionary<MethodInfo, object>> MappingConfigurations = new Dictionary<string, IDictionary<MethodInfo, object>>();

        private static readonly Type EntityType = typeof(EntityTypeConfiguration<>);

        private static readonly Type ComplexType = typeof(ComplexTypeConfiguration<>);

        private static readonly MethodInfo SetInitializerMethodDefinition = typeof(Database).GetMethod("SetInitializer", BindingFlags.Public | BindingFlags.Static);
        private static readonly List<MethodInfo> AddMethods = typeof(ConfigurationRegistrar).GetMethods().Where(m => m.Name.Equals("Add")).ToList();

        private static readonly MethodInfo EntityTypeMethod =
            AddMethods.First(
                m => m.GetParameters().First().ParameterType.GetGenericTypeDefinition().IsAssignableFrom(EntityType));

        private static readonly MethodInfo ComplexTypeMethod =
            AddMethods.First(
                m =>
                m.GetParameters().First().ParameterType.GetGenericTypeDefinition().IsAssignableFrom(ComplexType));

        private static bool init;

        private static readonly object SyncLock = new object();

        public static void Initalize()
        {
            if (!init)
            {
                lock (SyncLock)
                {
                    if (HostingEnvironment.IsSharedHost)
                    {
                        BuildSharedHostConfigurations();
                    }
                    else
                    {
                        dbContextAssemblyBuilder = AppDomain.CurrentDomain.DefineDynamicAssembly(new AssemblyName("Framework.Data.Proxies"), AssemblyBuilderAccess.Run);

                        string assemblyName = dbContextAssemblyBuilder.GetName().Name;

                        dbContextModuleBuilder = dbContextAssemblyBuilder.DefineDynamicModule(assemblyName);

                        BuildConfigurations();

                        BuildDbContextTypes();

                    }

                    init = true;

                }
            }
        }

        private static void DisableMigration(Type type)
        {
            SetInitializerMethodDefinition.MakeGenericMethod(type).Invoke(null, new object[] { null });
        }

        private static void BuildDbContextTypes()
        {
            ICollection<string> collection = MappingConfigurations.Keys;
            foreach (var nameOrConnectionString in collection)
            {
                TypeBuilder builder = dbContextModuleBuilder.DefineType(
                    String.Format("DbContext_Proxy_{0}", Guid.NewGuid().ToStringValue()), TypeAttributes.Class | TypeAttributes.Public, typeof(EntityContext));

                ConstructorBuilder constructorBuilder = builder.DefineConstructor(MethodAttributes.Public, CallingConventions.HasThis, new Type[] { typeof(string) });

                ILGenerator cIl = constructorBuilder.GetILGenerator();

                cIl.Emit(OpCodes.Ldarg_0);
                cIl.Emit(OpCodes.Ldarg_1);

                cIl.Emit(OpCodes.Call, typeof(EntityContext).GetConstructor(new Type[] { typeof(string) })); // Call base (object) constructor

                cIl.Emit(OpCodes.Nop);
                cIl.Emit(OpCodes.Nop);
                cIl.Emit(OpCodes.Nop);

                cIl.Emit(OpCodes.Ret);

                Type type = builder.CreateType();
                DisableMigration(type);
                ConstructorInfo ctor = type.GetConstructor(new[] { typeof(string) });
                Expression[] constructorArgumentCalls = new[] { Expression.Constant(nameOrConnectionString, typeof(string)) };

                Expression<Func<IEntityContext>> newServiceTypeMethod = Expression.Lambda<Func<IEntityContext>>(
                 Expression.New(ctor, constructorArgumentCalls), new ParameterExpression[0]);

                Func<IEntityContext> func = newServiceTypeMethod.Compile();

                DbContextCache.Add(nameOrConnectionString, func);
            }

        }

        private static void BuildConfigurations()
        {
            IEnumerable<Assembly> assemblies = ActivationManager.Assemblies;

            string defaultConnectionString = null;

            var overriddenConnectionString = WebConfigurationManager.AppSettings["Framework.RepositoryContext"];
            if (!string.IsNullOrWhiteSpace(overriddenConnectionString))
            {
                defaultConnectionString = overriddenConnectionString;
            }

            if (string.IsNullOrWhiteSpace(defaultConnectionString))
            {
                defaultConnectionString = "AppContext";
            }

            foreach (Assembly assembly in assemblies)
            {
                string nameOrConnectionString = defaultConnectionString;

                ContextNameAttribute contextNameAttribute = assembly.GetCustomAttribute<ContextNameAttribute>();

                if (contextNameAttribute != null && !string.IsNullOrWhiteSpace(contextNameAttribute.Name))
                {
                    nameOrConnectionString = contextNameAttribute.Name;
                }


                IDictionary<MethodInfo, object> configurations;
                if (!MappingConfigurations.ContainsKey(nameOrConnectionString))
                {
                    configurations = new Dictionary<MethodInfo, object>();
                    MappingConfigurations.Add(nameOrConnectionString, configurations);
                }
                else
                {
                    configurations = MappingConfigurations[nameOrConnectionString];
                }

                List<Type> types = assembly.GetExportableLoadableTypes().Where(IsMappingType).ToList();
                BindMapping(types, EntityTypeMethod, configurations, ComplexTypeMethod);
            }
        }

        private static void BuildSharedHostConfigurations()
        {
            IEnumerable<Assembly> assemblies = ActivationManager.Assemblies;

            string defaultConnectionString = null;

            var overriddenConnectionString = WebConfigurationManager.AppSettings["Framework.RepositoryContext"];
            if (!string.IsNullOrWhiteSpace(overriddenConnectionString))
            {
                defaultConnectionString = overriddenConnectionString;
            }

            if (string.IsNullOrWhiteSpace(defaultConnectionString))
            {
                defaultConnectionString = "AppContext";
            }

            foreach (Assembly assembly in assemblies)
            {

                IDictionary<MethodInfo, object> configurations;
                if (!MappingConfigurations.ContainsKey(defaultConnectionString))
                {
                    configurations = new Dictionary<MethodInfo, object>();
                    MappingConfigurations.Add(defaultConnectionString, configurations);
                }
                else
                {
                    configurations = MappingConfigurations[defaultConnectionString];
                }

                List<Type> types = assembly.GetExportableLoadableTypes().Where(IsMappingType).ToList();
                BindMapping(types, EntityTypeMethod, configurations, ComplexTypeMethod);
            }

            DbContextCache.Add(defaultConnectionString, () => new EntityContext(defaultConnectionString));
            Database.SetInitializer<EntityContext>(null);
        }

        public static IEntityContext CreateContext(string nameOrConnectionString)
        {
            return DbContextCache[nameOrConnectionString]();
        }

        private static void BindMapping(IEnumerable<Type> types, MethodInfo entityTypeMethod, IDictionary<MethodInfo, object> configurations, MethodInfo complexTypeMethod)
        {
            foreach (var type in types)
            {
                MethodInfo typedMethod;
                Type modelType;

                if (IsMatching(type, out modelType, t => EntityType.IsAssignableFrom(t)))
                {
                    typedMethod = entityTypeMethod.MakeGenericMethod(modelType);
                }
                else if (IsMatching(type, out modelType, t => ComplexType.IsAssignableFrom(t)))
                {
                    typedMethod = complexTypeMethod.MakeGenericMethod(modelType);
                }
                else
                {
                    continue;
                }

                configurations.Add(typedMethod, System.Activator.CreateInstance(type));
            }
        }

        private static bool IsMappingType(Type matchingType)
        {
            if (matchingType.IsClass && !matchingType.IsAbstract)
            {
                Type temp;

                return IsMatching(matchingType, out temp, t => EntityType.IsAssignableFrom(t) || ComplexType.IsAssignableFrom(t));
            }

            return false;
        }

        private static bool IsMatching(Type matchingType, out Type modelType, Predicate<Type> matcher)
        {
            modelType = null;

            while (matchingType != null)
            {
                if (matchingType.IsGenericType)
                {
                    var definationType = matchingType.GetGenericTypeDefinition();

                    if (matcher(definationType))
                    {
                        modelType = matchingType.GetGenericArguments().First();
                        return true;
                    }
                }

                matchingType = matchingType.BaseType;
            }

            return false;
        }
    }
}
