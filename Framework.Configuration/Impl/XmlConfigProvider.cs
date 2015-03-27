namespace Framework.Configuration.Impl
{
    using System;
    using System.IO;
    using System.Security;

    using Framework.Ioc;
    using Framework.Serialization.Xml;

    [InjectBind(typeof(IConfigProvider), LifetimeType.Singleton)]
    [InjectBind(typeof(IConfigProvider), "XmlConfig", LifetimeType.Singleton)]
    public class XmlConfigProvider : IConfigProvider
    {
        private Config currentConfig;

        public Config GetConfig()
        {
            if (this.currentConfig == null)
            {
                var serializer = Container.Get<IXmlSerializer>();

                string path = HostingEnvironment.IsHosted ?
                    HostingEnvironment.MapPath(Path.Combine(Constants.DataFolderPath, Constants.SettingFileName))
                    : Path.Combine(Environment.CurrentDirectory, Path.Combine(Constants.DataFolderPath.Remove(0, 2), Constants.SettingFileName));

                string directoryName = Path.GetDirectoryName(path);
                if (!Directory.Exists(directoryName))
                {
                    Directory.CreateDirectory(directoryName);
                }

                if (File.Exists(path))
                {
                    this.currentConfig = serializer.Deserialize<Config>(File.ReadAllText(path));
                }
            }

            return this.currentConfig;
        }

        public void SaveConfig(Config config = null)
        {
            if (config != null)
            {
                this.currentConfig = config;
            }

            var serializer = Container.Get<IXmlSerializer>();

            string serialize = serializer.Serialize(this.currentConfig);

            string path = HostingEnvironment.IsHosted ?
                HostingEnvironment.MapPath(Path.Combine(Constants.DataFolderPath, Constants.SettingFileName))
                : Path.Combine(Environment.CurrentDirectory, Path.Combine(Constants.DataFolderPath.Remove(0, 2), Constants.SettingFileName));

            string directoryName = Path.GetDirectoryName(path);
            if (!Directory.Exists(directoryName))
            {
                Directory.CreateDirectory(directoryName);
            }

            File.WriteAllText(path, serialize);
        }

        /// <summary>
        /// Gets this instance.
        /// </summary>
        /// <typeparam name="T">Type of the object.</typeparam>
        /// <returns>Specified Type.</returns>
        public T Get<T>() where T : class, new()
        {
            throw new NotSupportedException();
        }

        /// <summary>
        /// Saves the specified configuration object.
        /// </summary>
        /// <typeparam name="T">Type of the object.</typeparam>
        /// <param name="configObject">The configuration object.</param>
        public void Save<T>(T configObject) where T : class, new()
        {
            throw new NotSupportedException();
        }
    }
}