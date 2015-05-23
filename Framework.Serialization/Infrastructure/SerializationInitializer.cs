namespace Framework.Infrastructure
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Linq;
    using System.Security;
    using System.Text;
    using System.Threading.Tasks;

    using Framework.Serialization.Json.Converters;

    using Newtonsoft.Json;

    using Container = Framework.Ioc.Container;
    using GuidConverter = Framework.Serialization.Json.Converters.GuidConverter;

    /// -------------------------------------------------------------------------------------------------
    /// <summary>
    ///     Pre application start code.
    /// </summary>
    /// -------------------------------------------------------------------------------------------------
    [EditorBrowsable(EditorBrowsableState.Never)]
    public static class SerializationInitializer
    {
        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Pre-Init this Library.
        /// </summary>
        /// -------------------------------------------------------------------------------------------------
        
        public static void PreInit()
        {
            JsonConvert.DefaultSettings = () =>
            {
                JsonSerializerSettings settings = new JsonSerializerSettings
                {
                    Formatting = Formatting.Indented,
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                    NullValueHandling = NullValueHandling.Ignore,
                };
                settings.Converters.Add(new GuidConverter());
                settings.Converters.Add(new StringEnumConverter());
                return settings;
            };
        }
    }
}
