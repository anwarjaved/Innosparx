using System.Runtime.Remoting.Messaging;

namespace Framework.Configuration.Impl
{
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Linq;
    using System.Runtime.InteropServices;
    using System.Security;
    using System.Text;
    using System.Threading.Tasks;
    using System.Web.Configuration;

    using Framework.Domain;
    using Framework.Ioc;
    using Framework.Reflection;
    using Framework.Serialization.Json;

    /// <summary>
    /// Class Sql Config Provider.
    /// </summary>
    [InjectBind(typeof(IConfigProvider), "SqlConfig", LifetimeType.Request)]
    public class SqlConfigProvider : IConfigProvider
    {
        private static readonly Dictionary<string, IReflectionProperty> Properties = Build(typeof(Config), "Config");

        private readonly ConcurrentDictionary<string, object> extraConfigs = new ConcurrentDictionary<string, object>();

        private readonly string nameOrConnectionString;
        private readonly object syncLock = new object();
        private Config currentConfig;

        private bool loaded;

        /// <summary>
        /// Initializes a new instance of the <see cref="SqlConfigProvider"/> class.
        /// </summary>
        public SqlConfigProvider()
        {
            var overriddenConnectionString = WebConfigurationManager.AppSettings["Framework.Configuration.SqlProviderContext"];
            if (!string.IsNullOrWhiteSpace(overriddenConnectionString))
            {
                this.nameOrConnectionString = overriddenConnectionString;
            }

            if (string.IsNullOrWhiteSpace(this.nameOrConnectionString))
            {
                this.nameOrConnectionString = "AppContext";
            }
        }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Gets the settings.
        /// </summary>
        /// <returns>
        ///     The settings.
        /// </returns>
        /// -------------------------------------------------------------------------------------------------
        [SecuritySafeCritical]
        public Config GetConfig()
        {
            if (!this.loaded)
            {
                lock (this.syncLock)
                {
                    Dictionary<string, string> settings;
                    using (SqlConfigContext configContext = new SqlConfigContext(this.nameOrConnectionString))
                    {
                        settings = configContext.SiteSettings.Where(x => x.Name.StartsWith("Config.")).ToDictionary(x => x.Name, x => x.Value);
                    }

                    if (settings.Count > 0)
                    {
                        this.currentConfig = new Config();

                        LoadConfig(this.currentConfig, settings);
                    }

                    this.loaded = true;
                }
            }

            return this.currentConfig;
        }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Saves the settings.
        /// </summary>
        /// <param name="config">
        ///     The config.
        /// </param>
        /// -------------------------------------------------------------------------------------------------
        [SecuritySafeCritical]
        public void SaveConfig(Config config = null)
        {
            if (config != null)
            {
                this.currentConfig = config;
            }

            using (SqlConfigContext configContext = new SqlConfigContext(this.nameOrConnectionString))
            {
                Dictionary<string, SiteSetting> settings = configContext.SiteSettings.Where(x => x.Name.StartsWith("Config.")).ToDictionary(x => x.Name, x => x);

                foreach (var property in Properties)
                {
                    string[] propertyAccessors = property.Key.Split(new[] { "." }, StringSplitOptions.RemoveEmptyEntries);
                    SiteSetting setting;
                    if (!settings.ContainsKey(property.Key))
                    {
                        setting = new SiteSetting();
                        setting.Name = property.Key;
                        configContext.SiteSettings.Add(setting);
                    }
                    else
                    {
                        setting = settings[property.Key];
                    }

                    string propertyValue = SqlConfigProvider.GetPropertyValue(this.currentConfig, propertyAccessors.Skip(1).ToList());

                    setting.Value = string.IsNullOrWhiteSpace(propertyValue) ? string.Empty : propertyValue;
                }

                configContext.SaveChanges();
            }
        }

        /// <summary>
        /// Gets this instance.
        /// </summary>
        /// <typeparam name="T">Type of the object.</typeparam>
        /// <returns>Specified Type.</returns>
        [SecuritySafeCritical]
        public T Get<T>() where T : class, new()
        {
            Type type = typeof(T);
            string configName = type.Name;

            object configValue = this.extraConfigs.GetOrAdd(
                configName,
                (key) =>
                    {
                        var configObject = this.BuildConfigObject<T>(key);

                        return configObject;
                    });


            return (T)configValue;
        }

        [SecuritySafeCritical]
        private T BuildConfigObject<T>(string key) where T : class, new()
        {
            T configObject = new T();
            Dictionary<string, string> settings;
            using (SqlConfigContext configContext = new SqlConfigContext(this.nameOrConnectionString))
            {
                settings = configContext.SiteSettings.Where(x => x.Name.StartsWith(key + ".")).ToDictionary(x => x.Name, x => x.Value);
            }

            if (settings.Count > 0)
            {
                LoadConfig(configObject, settings);
            }
            return configObject;
        }

        /// <summary>
        /// Saves the specified configuration object.
        /// </summary>
        /// <typeparam name="T">Type of the object.</typeparam>
        /// <param name="configObject">The configuration object.</param>
        [SecuritySafeCritical]
        public void Save<T>(T configObject) where T : class, new()
        {
            Type type = typeof(T);
            string configName = type.Name;

            using (SqlConfigContext configContext = new SqlConfigContext(this.nameOrConnectionString))
            {
                IList<SiteSetting> siteSettings = configContext.SiteSettings.Where(x => x.Name.StartsWith(configName + ".")).ToList();
                if (configObject == null)
                {
                    foreach (var siteSetting in siteSettings)
                    {
                        configContext.SiteSettings.Remove(siteSetting);
                    }
                }
                else
                {
                    Dictionary<string, SiteSetting> settings = siteSettings.ToDictionary(x => x.Name, x => x);

                    foreach (var property in Build(type, configName))
                    {
                        string[] propertyAccessors = property.Key.Split(new[] { "." }, StringSplitOptions.RemoveEmptyEntries);
                        SiteSetting setting;
                        if (!settings.ContainsKey(property.Key))
                        {
                            setting = new SiteSetting();
                            setting.Name = property.Key;
                            configContext.SiteSettings.Add(setting);
                        }
                        else
                        {
                            setting = settings[property.Key];
                        }

                        string propertyValue = SqlConfigProvider.GetPropertyValue(configObject, propertyAccessors.Skip(1).ToList());

                        setting.Value = string.IsNullOrWhiteSpace(propertyValue) ? string.Empty : propertyValue;
                    }
                }

                configContext.SaveChanges();
            }
            this.extraConfigs.AddOrUpdate(configName, configObject, (k, obj) => configObject);
        }

        private static void LoadConfig<T>(T config, Dictionary<string, string> settings)
        {
            foreach (var siteSetting in settings)
            {
                string[] propertyAccessors = siteSetting.Key.Split(new[] { "." }, StringSplitOptions.RemoveEmptyEntries);

                SetPropertyValue(config, propertyAccessors.Skip(1).ToList(), siteSetting.Value);
            }
        }

        private static void SetPropertyValue<T>(T config, IReadOnlyList<string> propertyAccessors, string value)
        {
            if (!string.IsNullOrWhiteSpace(value))
            {
                IReflectionType reflectionType = Reflector.Get(typeof(T));
                var jsonSerializer = Container.Get<IJsonSerializer>();
                object propertyValue = reflectionType.GetPropertyValue(propertyAccessors[0], config);
                IReflectionProperty reflectionProperty = reflectionType.GetProperty(propertyAccessors[0]);

                if (reflectionProperty != null)
                {
                    if (reflectionProperty.IsPrimitive)
                    {
                        object deserializeValue = jsonSerializer.Deserialize(reflectionProperty.Type, value);

                        reflectionProperty.Set(config, deserializeValue);
                    }
                    else
                    {
                        reflectionType = Reflector.Get(reflectionProperty.Type);
                        for (int i = 1; i < propertyAccessors.Count - 1; i++)
                        {
                            propertyValue = reflectionType.GetPropertyValue(propertyAccessors[i], propertyValue);
                            reflectionType = Reflector.Get(reflectionType.GetProperty(propertyAccessors[i]).Type);
                        }

                        IReflectionProperty property =
                            reflectionType.GetProperty(propertyAccessors[propertyAccessors.Count - 1]);

                        object deserializeValue = jsonSerializer.Deserialize(property.Type, value);

                        property.Set(propertyValue, deserializeValue);
                    }
                
                }
            }
        }

        private static string GetPropertyValue<T>(T config, IReadOnlyList<string> propertyAccessors)
        {
            IReflectionType reflectionType = Reflector.Get(typeof(T));
            var jsonSerializer = Container.Get<IJsonSerializer>();
            object propertyValue = reflectionType.GetPropertyValue(propertyAccessors[0], config);
            reflectionType = Reflector.Get(reflectionType.GetProperty(propertyAccessors[0]).Type);

            for (int i = 1; i < propertyAccessors.Count; i++)
            {
                propertyValue = reflectionType.GetPropertyValue(propertyAccessors[i], propertyValue);
                reflectionType = Reflector.Get(reflectionType.GetProperty(propertyAccessors[i]).Type);

                if (i == (propertyAccessors.Count - 2))
                {
                    i++;
                    propertyValue = reflectionType.GetPropertyValue(propertyAccessors[i], propertyValue);
                }
            }

            if (propertyValue != null)
            {
                string serializedValue = jsonSerializer.Serialize(propertyValue);

                return serializedValue;
            }

            return null;
        }

        private static Dictionary<string, IReflectionProperty> Build(Type type, string name = null)
        {
            Dictionary<string, IReflectionProperty> dictionary = new Dictionary<string, IReflectionProperty>();

            IReflectionType reflectionType = Reflector.Get(type);

            foreach (var property in reflectionType.Properties)
            {
                ParseProperty(dictionary, property, name);
            }

            return dictionary;
        }

        private static void ParseProperty(Dictionary<string, IReflectionProperty> dictionary, IReflectionProperty property, string parentName = "")
        {
            string propertyName = property.Name;

            if (!string.IsNullOrWhiteSpace(parentName))
            {
                propertyName = parentName + "." + propertyName;
            }

            if (property.IsClass)
            {
                IReflectionType reflectionType = Reflector.Get(property.Type);

                foreach (var reflectionProperty in reflectionType.Properties)
                {
                    ParseProperty(dictionary, reflectionProperty, propertyName);
                }
            }
            else
            {
                dictionary.Add(propertyName, property);
            }
        }
    }
}
