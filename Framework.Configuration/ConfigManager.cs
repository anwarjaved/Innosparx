namespace Framework.Configuration
{
    using Framework.Ioc;

    /// -------------------------------------------------------------------------------------------------
    /// <summary>
    ///     Manager for configurations.
    /// </summary>
    /// -------------------------------------------------------------------------------------------------
    public static class ConfigManager
    {
        private static readonly Config DefaultConfig = new Config();

        /// <summary>
        /// Gets the amazon.
        /// </summary>
        /// <value>The amazon.</value>
        public static AmazonSettings Amazon
        {
            get
            {
                return GetConfig(true).Amazon;
            }
        }

        /// <summary>
        /// Gets the mail.
        /// </summary>
        /// <value>The mail.</value>
        public static MailSettings Mail
        {
            get
            {
                return GetConfig(true).Mail;
            }
        }

        /// <summary>
        /// Gets the application.
        /// </summary>
        /// <value>The application.</value>
        public static ApplicationSetting Application
        {
            get
            {
                return GetConfig(true).Application;
            }
        }

        /// <summary>
        /// Gets the social.
        /// </summary>
        /// <value>The social.</value>
        public static SocialApiSetting Social
        {
            get
            {
                return GetConfig(true).Social;
            }
        }

        /// <summary>
        /// Gets the specified configuration object.
        /// </summary>
        /// <typeparam name="T">Type of the object.</typeparam>
        /// <returns>Specified Type.</returns>
        public static T Get<T>() where T : class, new()
        {
            var configProvider = Container.TryGet<IConfigProvider>();

            if (configProvider != null)
            {
                return configProvider.Get<T>();
            }

            return new T();
        }

        /// <summary>
        /// Saves the specified configuration object.
        /// </summary>
        /// <typeparam name="T">Type of the object.</typeparam>
        /// <param name="configObject">The configuration object.</param>
        public static void Save<T>(T configObject) where T : class, new()
        {
            var configProvider = Container.TryGet<IConfigProvider>();

            if (configProvider != null)
            {
                configProvider.Save(configObject);
            }
        }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Gets the get.
        /// </summary>
        /// <returns>
        ///     .
        /// </returns>
        /// -------------------------------------------------------------------------------------------------
        public static Config GetConfig(bool returnDefault = false)
        {
            var configProvider = Container.TryGet<IConfigProvider>();

            if (configProvider != null)
            {
                Config config = configProvider.GetConfig();

                if (config != null)
                {
                    return config;
                }
            }

            if (returnDefault)
            {
                return DefaultConfig;
            }

            return null;
        }

        public static void SaveConfig(Config config = null)
        {
            var configProvider = Container.TryGet<IConfigProvider>();

            if (configProvider != null)
            {
                configProvider.SaveConfig(config);
            }
        }
    }
}