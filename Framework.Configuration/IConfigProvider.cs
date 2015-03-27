namespace Framework.Configuration
{
    /// -------------------------------------------------------------------------------------------------
    /// <summary>
    ///     Interface for config provider.
    /// </summary>
    /// -------------------------------------------------------------------------------------------------
    public interface IConfigProvider
    {
        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Gets the settings.
        /// </summary>
        /// <returns>
        ///     The settings.
        /// </returns>
        /// -------------------------------------------------------------------------------------------------
        Config GetConfig();

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Saves the settings.
        /// </summary>
        /// <param name="config">
        ///     The config.
        /// </param>
        /// -------------------------------------------------------------------------------------------------
        void SaveConfig(Config config = null);

        /// <summary>
        /// Gets this instance.
        /// </summary>
        /// <typeparam name="T">Type of the object.</typeparam>
        /// <returns>Specified Type.</returns>
        T Get<T>() where T : class, new();

        /// <summary>
        /// Saves the specified configuration object.
        /// </summary>
        /// <typeparam name="T">Type of the object.</typeparam>
        /// <param name="configObject">The configuration object.</param>
        void Save<T>(T configObject) where T : class, new();
    }
}