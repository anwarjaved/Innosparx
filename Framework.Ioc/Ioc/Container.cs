namespace Framework.Ioc
{
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Configuration;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using System.Security;
    using System.Text;
    using System.Text.RegularExpressions;
    using System.Web.Compilation;

    using Framework.Activator;
    using Framework.Collections;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;

    /// <summary>
    /// Dependency Container.
    /// </summary>
    public static class Container
    {
        private static readonly Multimap<Type, IBindingInfo> Bindings = new Multimap<Type, IBindingInfo>();
        private static readonly ConcurrentDictionary<Type, string> DefaultBinding = new ConcurrentDictionary<Type, string>();

        /// <summary>
        /// Binds the specified <typeparamref name="TType"/>.
        /// </summary>
        /// <typeparam name="TType">The type of the type.</typeparam>
        /// <param name="name">The name.</param>
        /// <returns>
        /// Binding Info as a <see cref="IBindingInfo{T}"/>.
        /// </returns>
        public static IBindingInfo<TType> Bind<TType>(string name = null)
        {
            Type service = typeof(TType);

            IBindingInfo existingBinding = Resolve<TType>(name);

            if (existingBinding == null)
            {
                BindingInfo<TType> binding = new BindingInfo<TType>(name);
                Bindings.Add(service, binding);

                existingBinding = binding;
            }

            return (IBindingInfo<TType>)existingBinding;
        }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        /// Bind a type to specified activators.
        /// </summary>
        /// <typeparam name="TType">
        /// Type of the type.
        /// </typeparam>
        /// <param name="activators">
        /// The activators.
        /// </param>
        /// -------------------------------------------------------------------------------------------------
        public static void BindAll<TType>(IEnumerable<Func<TType>> activators)
        {
            Type service = typeof(TType);

            foreach (var activator in activators)
            {
                Func<TType> method = activator;
                BindingInfo<TType> binding = new BindingInfo<TType>();

                binding.Instance = () => method;

                Bindings.Add(service, binding);
            }
        }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        /// Bind a type to specified activators.
        /// </summary>
        /// <param name="service">
        /// The service.
        /// </param>
        /// <param name="activators">
        /// The activators.
        /// </param>
        /// -------------------------------------------------------------------------------------------------
        public static void BindAll(Type service, IEnumerable<Func<object>> activators)
        {
            foreach (var activator in activators)
            {
                Func<object> method = activator;
                BindingInfo<object> binding = new BindingInfo<object>();
                binding.Instance = method;

                Bindings.Add(service, binding);
            }
        }

        /// <summary>
        /// Binds the specified type.
        /// </summary>
        /// <param name="service">The service.</param>
        /// <param name="name">The name.</param>
        /// <returns>
        /// Binding Info as a <see cref="IBindingInfo{T}"/>.
        /// </returns>
        public static IBindingInfo<object> Bind(Type service, string name = null)
        {
            IBindingInfo existingBinding = Resolve(service, name);

            if (existingBinding == null)
            {
                BindingInfo<object> binding = new BindingInfo<object>(service, name);
                Bindings.Add(service, binding);

                existingBinding = binding;
            }

            return (IBindingInfo<object>)existingBinding;
        }

        /// <summary>
        /// Removes all.
        /// </summary>
        /// <typeparam name="TType">The type of the type.</typeparam>
        /// <author>Anwar</author>
        /// <date>11/10/2011</date>
        public static void RemoveAll<TType>()
        {
            Type service = typeof(TType);
            Bindings.RemoveAll(service);
        }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Removes all bindings.
        /// </summary>
        /// -------------------------------------------------------------------------------------------------
        public static void RemoveAll()
        {
            foreach (KeyValuePair<Type, ICollection<IBindingInfo>> valuePair in Bindings)
            {
                foreach (var existingBinding in valuePair.Value)
                {
                    existingBinding.ReleaseInstance();
                }
            }

            Bindings.Clear();
        }

        /// <summary>
        /// Removes all.
        /// </summary>
        /// <typeparam name="TType">The type of the type.</typeparam>
        /// <param name="name">The name.</param>
        /// <date>11/10/2011</date>
        public static void Remove<TType>(string name = null)
        {
            Type service = typeof(TType);
            IBindingInfo existingBinding = Resolve(service, name);

            if (existingBinding != null)
            {
                existingBinding.ReleaseInstance();
                Bindings.Remove(service, existingBinding);
            }
        }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        /// Removes the type from DI.
        /// </summary>
        /// <param name="service">
        /// The service.
        /// </param>
        /// <param name="name">
        /// The name.
        /// </param>
        /// -------------------------------------------------------------------------------------------------
        public static void Remove(Type service, string name = null)
        {
            IBindingInfo existingBinding = Resolve(service, name);

            if (existingBinding != null)
            {
                existingBinding.ReleaseInstance();
                Bindings.Remove(service, existingBinding);
            }
        }

        /// <summary>
        /// Overrides the default service.
        /// </summary>
        /// <typeparam name="TType">The type of the t type.</typeparam>
        /// <param name="name">The name.</param>
        public static void OverrideDefaultService<TType>(string name = null)
        {
            Type service = typeof(TType);
            DefaultBinding.AddOrUpdate(service, name, (t, s) => name);
        }

        /// <summary>
        /// Overrides the default service.
        /// </summary>
        /// <param name="service">The service.</param>
        /// <param name="name">The name.</param>
        public static void OverrideDefaultService(Type service, string name = null)
        {
            DefaultBinding.AddOrUpdate(service, name, (t, s) => name);
        }

        /// <summary>
        /// The get.
        /// </summary>
        /// <typeparam name="TType">The type of the type.</typeparam>
        /// <param name="name">The name.</param>
        /// <returns>Resolved Type Reference.</returns>
        public static TType Get<TType>(string name = null)
        {
            Type service = typeof(TType);

            if (Bindings.ContainsKey(service))
            {
                IBindingInfo binding = Resolve<TType>(name);

                if (binding == null)
                {
                    throw new ActivationException(string.Format("No registration for type {0} could be found.", service.FullName));
                }

                try
                {
                    object instance = binding.GetInstance();

                    if (instance == null)
                    {
                        throw new ActivationException(string.Format("The registered delegate for type {0} returned null.", service.FullName));
                    }

                    return (TType)instance;
                }
                catch (Exception exception)
                {
                    throw new ActivationException("Cannot Resolve type " + service.FullName, exception);
                }
            }

            throw new ActivationException(
                string.Format("No registration for type {0} could be found.", service.FullName));
        }

        /// <summary>
        /// The get.
        /// </summary>
        /// <typeparam name="TType">The type of the type.</typeparam>
        /// <returns>Resolved Type Reference.</returns>
        public static IReadOnlyList<TType> GetAll<TType>()
        {
            Type service = typeof(TType);

            if (!Bindings.ContainsKey(service))
            {
                throw new ActivationException(
                    string.Format("No registration for type {0} could be found.", service.FullName));
            }

            List<TType> list = new List<TType>();

            foreach (var binding in Bindings[service])
            {
                try
                {
                    object instance = binding.GetInstance();

                    if (instance == null)
                    {
                        throw new ActivationException(
                            string.Format("The registered delegate for type {0} returned null.", service.FullName));
                    }

                    list.Add((TType)instance);
                }
                catch (Exception exception)
                {
                    throw new ActivationException("Cannot Resolve type " + service.FullName, exception);
                }
            }

            return list;
        }

        /// <summary>
        /// The get.
        /// </summary>
        /// <typeparam name="TType">The type of the type.</typeparam>
        /// <returns>Resolved Type Reference.</returns>
        public static IReadOnlyList<TType> TryGetAll<TType>()
        {
            Type service = typeof(TType);

            List<TType> list = new List<TType>();
            if (Bindings.ContainsKey(service))
            {
                foreach (var binding in Bindings[service])
                {
                    try
                    {
                        object instance = binding.GetInstance();

                        if (instance != null)
                        {
                            list.Add((TType)instance);
                        }
                    }
                    catch (Exception)
                    {
                    }
                }
            }

            return list;
        }

        /// <summary>
        /// The get.
        /// </summary>
        /// <param name="serviceType">Type of the service.</param>
        /// <returns>
        /// Resolved Type Reference.
        /// </returns>
        public static IReadOnlyList<object> TryGetAll(Type serviceType)
        {
            Type service = serviceType;
            List<object> list = new List<object>();

            if (Bindings.ContainsKey(service))
            {
                foreach (var binding in Bindings[service])
                {
                    try
                    {
                        object instance = binding.GetInstance();

                        if (instance != null)
                        {
                            list.Add(instance);
                        }
                    }
                    catch
                    {
                    }
                }
            }

            return list;
        }

        /// <summary>
        /// The get.
        /// </summary>
        /// <param name="serviceType">Type of the service.</param>
        /// <param name="name">The name.</param>
        /// <returns>
        /// Resolved Type Reference.
        /// </returns>
        public static object Get(Type serviceType, string name = null)
        {
            Type service = serviceType;

            if (!Bindings.ContainsKey(service))
            {
                throw new ActivationException(
                    string.Format("No registration for type {0} could be found.", service.FullName));
            }

            IBindingInfo binding = Resolve(service, name);

            if (binding == null)
            {
                throw new ActivationException(
                    string.Format("No registration for type {0} could be found.", service.FullName));
            }

            try
            {
                object instance = binding.GetInstance();

                if (instance == null)
                {
                    throw new ActivationException(
                        string.Format("The registered delegate for type {0} returned null.", service.FullName));
                }

                return instance;
            }
            catch (Exception exception)
            {
                throw new ActivationException("Cannot Resolve type " + service.FullName, exception);
            }
        }

        /// <summary>
        /// The get.
        /// </summary>
        /// <param name="serviceType">Type of the service.</param>
        /// <param name="name">The name.</param>
        /// <returns>
        /// Resolved Type Reference.
        /// </returns>
        public static object TryGet(Type serviceType, string name = null)
        {
            Type service = serviceType;

            if (Bindings.ContainsKey(service))
            {
                IBindingInfo binding = Resolve(serviceType, name);

                if (binding != null)
                {
                    try
                    {
                        object instance = binding.GetInstance();

                        if (instance != null)
                        {
                            return instance;
                        }
                    }
                    catch
                    {
                        return null;
                    }
                }
            }

            return null;
        }

        /// <summary>
        /// The get.
        /// </summary>
        /// <typeparam name="TType">The type of the type.</typeparam>
        /// <param name="name">The name.</param>
        /// <returns>
        /// Resolved Type Reference.
        /// </returns>
        public static TType TryGet<TType>(string name = null)
        {
            Type service = typeof(TType);

            if (Bindings.ContainsKey(service))
            {
                IBindingInfo binding = Resolve<TType>(name);

                if (binding != null)
                {
                    try
                    {
                        object instance = binding.GetInstance();

                        if (instance != null)
                        {
                            return (TType)instance;
                        }
                    }
                    catch
                    {
                        return default(TType);
                    }
                }
            }

            return default(TType);
        }

        /// <summary>
        /// Query if this Container contains the type with given name.
        /// </summary>
        /// <typeparam name="TType">Type of the type.</typeparam>
        /// <param name="name">The name.</param>
        /// <returns>true if it succeeds, false if it fails.</returns>
        public static bool Contains<TType>(string name = null)
        {
            return Resolve<TType>(name) != null;
        }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        /// Query if this Container contains the type with given name.
        /// </summary>
        /// <param name="service">
        /// The service.
        /// </param>
        /// <param name="name">
        /// The name.
        /// </param>
        /// <returns>
        /// true if the object is in this collection, false if not.
        /// </returns>
        /// -------------------------------------------------------------------------------------------------
        public static bool Contains(Type service, string name = null)
        {
            return Resolve(service, name) != null;
        }

        /// <summary>
        /// Registers all dependencies in referenced assemblies.
        /// </summary>
        /// <author>Anwar</author>
        /// <date>11/9/2011</date>

        public static void ScanAll()
        {
            var path = HostingEnvironment.IsHosted
                           ? HostingEnvironment.MapPath("~/App_Data/bindings.json")
                           : Path.Combine(Environment.CurrentDirectory, "bindings.json");
            if (File.Exists(path))
            {
                using (Stream stream = new FileStream(path, FileMode.Open))
                {
                    Container.LoadFrom(stream);
                    var assemblies = ActivationManager.Assemblies;


                    var list = new List<Assembly>();

                    foreach (var assembly in assemblies.AsParallel())
                    {
                        var attr = assembly.GetCustomAttribute<ContainerAssemblyAttribute>();

                        if (attr != null && attr.PostBuild)
                        {
                            list.Add(assembly);
                        }
                    }
                       
                    DependencyBuilder.PostBuildScan(list);
                }
            }
            else
            {
                DependencyBuilder.ScanAll();
                using (Stream stream = new FileStream(path, FileMode.Create))
                {
                    Container.SaveTo(stream);
                }
            }
        }

        public static void LoadFrom(Stream stream)
        {
            StreamReader reader = new StreamReader(stream, Encoding.UTF8);

            dynamic deserializeObject = JsonConvert.DeserializeObject(reader.ReadToEnd());
            reader.Dispose();
            if (deserializeObject.defaultBindings != null)
            {
                foreach (var binding in deserializeObject.defaultBindings)
                {
                    if (binding.type != null && binding.name != null)
                    {
                        Type type = Type.GetType(ParseTypeName((string)binding.type), false);

                        if (type != null)
                        {
                            Container.OverrideDefaultService(type, binding.name);
                        }
                    }

                }
            }

            if (deserializeObject.allBindings != null)
            {
                foreach (var bindingInfo in deserializeObject.allBindings)
                {
                    if (bindingInfo.type != null && bindingInfo.bindings != null)
                    {
                        Type type = Type.GetType(ParseTypeName((string)bindingInfo.type), false);

                        if (type != null)
                        {
                            foreach (var binding in bindingInfo.bindings)
                            {
                                Type service = Type.GetType(ParseTypeName((string)binding.service), false);
                                Type implementation = Type.GetType(ParseTypeName((string)binding.implementation), false);

                                if (service != null && implementation != null)
                                {
                                    string name = binding.name;
                                    int? order = null;

                                    if (binding.order != null)
                                    {
                                        order = Convert.ToInt32(binding.order);
                                    }

                                    Type scopeType = Type.GetType(ParseTypeName((string)binding.scope), false);
                                    ILifetime lifetime = scopeType == null
                                                             ? new TransientLifetime()
                                                             : (ILifetime)Activator.CreateInstance(scopeType);


                                    DependencyBuilder.BindInstance(service, implementation, name, lifetime, order);
                                }
                            }
                        }
                    }
                }
            }

        }

        private static string ParseTypeName(string typeNameWithAssembly)
        {
            var split = typeNameWithAssembly.Split(',');
            return typeNameWithAssembly;
        }

        private static string GetTypeNameWithAssembly(string assemblyQualifiedName)
        {
            assemblyQualifiedName = Regex.Replace(assemblyQualifiedName, @", Version=\d+.\d+.\d+.\d+", string.Empty);

            assemblyQualifiedName = Regex.Replace(assemblyQualifiedName, @", Culture=\w+", string.Empty);

            return Regex.Replace(assemblyQualifiedName, @", PublicKeyToken=\w+", string.Empty);
        }

        private static void SaveTo(Stream stream)
        {
            dynamic json = new JObject();

            var defaultBindings = new JArray();

            foreach (var binding in Container.DefaultBinding)
            {
                defaultBindings.Add(new { type = GetTypeNameWithAssembly(binding.Key.AssemblyQualifiedName), name = binding.Value });
            }

            json.defaultBindings = defaultBindings;

            var allBindings = new JArray();
            foreach (var bindingType in Container.Bindings.Keys)
            {
                var bindingArray = new JArray();

                var anyFound = false;
                foreach (var binding in Container.Bindings[bindingType].Where(x => x.BindingType != BindingType.Method))
                {
                    dynamic bindingObject = new JObject();
                    bindingObject.service = GetTypeNameWithAssembly(binding.Service.AssemblyQualifiedName);
                    bindingObject.implementation = GetTypeNameWithAssembly(binding.Implementation.AssemblyQualifiedName);
                    bindingObject.name = binding.Name;
                    bindingObject.order = binding.Order;
                    bindingObject.scope =
                        GetTypeNameWithAssembly(binding.LifetimeManager.GetType().AssemblyQualifiedName);
                    bindingArray.Add(bindingObject);

                    anyFound = true;
                }


                if (anyFound)
                {
                    dynamic jBinding = new JObject();
                    jBinding.type = GetTypeNameWithAssembly(bindingType.AssemblyQualifiedName);
                    jBinding.bindings = bindingArray;
                    allBindings.Add(jBinding);
                }
            }

            json.allBindings = allBindings;
            StreamWriter writer = new StreamWriter(stream, Encoding.UTF8);
            writer.Write(JsonConvert.SerializeObject(json));
            writer.Flush();
            writer.Dispose();
        }



        private static IBindingInfo Resolve<TType>(string name = null)
        {
            Type service = typeof(TType);
            IBindingRequest request = new BindingRequest() { Name = name, Service = service };

            return Resolve(request);
        }

        private static IBindingInfo Resolve(Type service, string name = null)
        {
            IBindingRequest request = new BindingRequest() { Name = name, Service = service };

            return Resolve(request);
        }

        private static IBindingInfo Resolve(IBindingRequest request)
        {
            ICollection<IBindingInfo> bindings = Bindings[request.Service];

            if (string.IsNullOrWhiteSpace(request.Name))
            {
                if (DefaultBinding.ContainsKey(request.Service))
                {
                    request.Name = DefaultBinding[request.Service];
                }
            }

            return bindings != null ? bindings.FirstOrDefault(binding => ((IBindingMatcher)binding).Matches(request)) : null;
        }
    }
}