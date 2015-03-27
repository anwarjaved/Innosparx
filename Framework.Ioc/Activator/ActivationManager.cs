namespace Framework.Activator
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using System.Security;
    using System.Web;
    using System.Web.Compilation;

    using Framework.Ioc;

    using Container = Framework.Ioc.Container;
    using HostingEnvironment = System.Web.Hosting.HostingEnvironment;

    /// -------------------------------------------------------------------------------------------------
    /// <summary>
    /// Manager for activations.
    /// </summary>
    /// <remarks>
    /// LM ANWAR, 6/2/2013.
    /// </remarks>
    /// -------------------------------------------------------------------------------------------------
    [SecurityCritical]
    public static class ActivationManager
    {
        internal static readonly string[] SkipList = { "EntityFramework", "Microsoft.", "System.", "Newtonsoft.Json", "mscorlib" };
        
        private static bool hasInited;

        private static List<Assembly> assemblies;

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        /// Gets the assemblies.
        /// </summary>
        /// <value>
        /// The assemblies.
        /// </value>
        /// -------------------------------------------------------------------------------------------------
        public static IReadOnlyList<Assembly> Assemblies
        {
            get
            {
                if (assemblies == null)
                {
                    // Cache the list of relevant assemblies, since we need it for both Pre and Post
                    assemblies = new List<Assembly>();
                    foreach (var assemblyFile in GetAssemblyFiles())
                    {
                        try
                        {
                            var assembly = Assembly.LoadFrom(assemblyFile);

                            if (SkipList.All(skipValue => !assembly.FullName.StartsWith(skipValue, StringComparison.OrdinalIgnoreCase)))
                            {
                                assemblies.Add(assembly);
                            }
                        }
                        catch (Win32Exception)
                        {
                        }
                        catch (ArgumentException)
                        {
                        }
                        catch (FileNotFoundException)
                        {
                        }
                        catch (PathTooLongException)
                        {
                        }
                        catch (BadImageFormatException)
                        {
                        }
                        catch (SecurityException)
                        {
                        }
                    }

                    assemblies.Sort(new AssemblyComparer());
                }

                return assemblies;
            }
        }

        // Return all the App_Code assemblies
        private static IEnumerable<Assembly> AppCodeAssemblies
        {
            get
            {
                // Return an empty list if we;re not hosted or there aren't any
                if (!HostingEnvironment.IsHosted || !hasInited || BuildManager.CodeAssemblies == null)
                {
                    return Enumerable.Empty<Assembly>();
                }

                return BuildManager.CodeAssemblies.OfType<Assembly>();
            }
        }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        /// Runs this object.
        /// </summary>
        /// <remarks>
        /// LM ANWAR, 6/2/2013.
        /// </remarks>
        /// -------------------------------------------------------------------------------------------------
        public static void Run()
        {
            bool designerMode = IsInClientBuildManager();
            if (!hasInited)
            {
                Container.ScanAll();

                // Register our module to handle any Post Start methods. But outside of ASP.NET, just run them now
                if (HostingEnvironment.IsHosted)
                {
                    LazyHttpModule<IocManagerModule>.Register(() => new IocManagerModule());
                    LazyHttpModule<FilterHttpModule>.Register(() => new FilterHttpModule());
                    RunPreStartMethods(designerMode);
                    LazyHttpModule<StartMethodCallingModule>.Register(() => new StartMethodCallingModule(designerMode));
                }
                else
                {
                    RunPreStartMethods(designerMode);
                    RunPostStartMethods(designerMode);
                    Bootstrapper.Run();
                }

                hasInited = true;
            }
        }

        // For unit test purpose
        internal static void Reset()
        {
            hasInited = false;
            assemblies = null;
        }

        private static bool IsInClientBuildManager()
        {
            return HostingEnvironment.InClientBuildManager;
        }

        private static IEnumerable<string> GetAssemblyFiles()
        {
            // When running under ASP.NET, find assemblies in the bin folder.
            // Outside of ASP.NET, use whatever folder current dll itself is in
            string directory = HostingEnvironment.IsHosted
                                   ? HttpRuntime.BinDirectory
                                   : Path.GetDirectoryName(typeof(ActivationManager).Assembly.Location);
            return
                Directory.EnumerateFiles(directory, "*.*", SearchOption.AllDirectories)
                    .Where(s => s.EndsWith(".dll") || s.EndsWith(".exe"));
        }

        private static void RunPreStartMethods(bool designerMode)
        {
            RunActivationMethods<PreApplicationStartMethodAttribute>(designerMode);
        }

        private static void RunPostStartMethods(bool designerMode)
        {
            RunActivationMethods<PostApplicationStartMethodAttribute>(designerMode);
        }

        private static void RunShutdownMethods(bool designerMode)
        {
            RunActivationMethods<ApplicationShutdownMethodAttribute>(designerMode);
        }

        // Call the relevant activation method from all assemblies
        private static void RunActivationMethods<T>(bool designerMode) where T : BaseActivationMethodAttribute
        {
            var attribs = Assemblies.Concat(AppCodeAssemblies)
                                    .SelectMany(assembly => assembly.GetActivationAttributes<T>())
                                    .OrderBy(att => att.Order);

            foreach (var activationAttrib in attribs)
            {
                // Don't run it in designer mode, unless the attribute explicitly asks for that
                if (!designerMode || activationAttrib.RunInDesigner)
                {
                    activationAttrib.InvokeMethod();
                }
            }
        }

        [SecurityCritical]
        private class StartMethodCallingModule : IHttpModule
        {
            private static readonly object SyncLock = new object();

            private static int initializedModuleCount;
            private readonly bool designerMode;

            public StartMethodCallingModule(bool designerMode)
            {
                this.designerMode = designerMode;
            }

            [SecuritySafeCritical]
            void IHttpModule.Init(HttpApplication context)
            {
                lock (SyncLock)
                {
                    // Keep track of the number of modules initialized and
                    // make sure we only call the post start methods once per app domain
                    if (initializedModuleCount++ == 0)
                    {
                        RunPostStartMethods(this.designerMode);
                        Bootstrapper.Run();
                    }
                }
            }

            [SecuritySafeCritical]
            void IHttpModule.Dispose()
            {
                lock (SyncLock)
                {
                    // Call the shutdown methods when the last module is disposed
                    if (--initializedModuleCount == 0)
                    {
                        RunShutdownMethods(this.designerMode);
                    }
                }
            }
        }
    }
}
