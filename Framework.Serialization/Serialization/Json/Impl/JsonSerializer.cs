namespace Framework.Serialization.Json.Impl
{
    using System;
    using System.Runtime.Serialization;

    using Framework.Ioc;
    using Framework.Serialization.Json.Converters;

    using Newtonsoft.Json;

    ///-------------------------------------------------------------------------------------------------
    /// <summary>
    ///     JSON serializer.
    /// </summary>
    ///-------------------------------------------------------------------------------------------------
    [InjectBind(typeof(ISerializer), "Json", LifetimeType.Singleton)]
    [InjectBind(typeof(IJsonSerializer), LifetimeType.Singleton)]
    public class JsonSerializer : IJsonSerializer
    {
        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Deserializes.
        /// </summary>
        ///
        /// <exception cref="SerializationException">
        ///     Thrown when a Serialization error condition occurs.
        /// </exception>
        ///
        /// <param name="type">
        ///     The type.
        /// </param>
        /// <param name="value">
        ///     The value.
        /// </param>
        ///
        /// <returns>
        ///     .
        /// </returns>
        ///-------------------------------------------------------------------------------------------------
        public object Deserialize(Type type, string value)
        {
            try
            {
                return JsonConvert.DeserializeObject(value, type);
            }
            catch (Exception exception)
            {
                throw new SerializationException(exception.Message, exception);
            }
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Deserializes.
        /// </summary>
        ///
        /// <typeparam name="T">
        ///     Generic type parameter.
        /// </typeparam>
        /// <param name="value">
        ///     The value.
        /// </param>
        ///
        /// <returns>
        ///     .
        /// </returns>
        ///-------------------------------------------------------------------------------------------------
        public T Deserialize<T>(string value)
        {
            return (T)this.Deserialize(typeof(T), value);
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Serializes the given value.
        /// </summary>
        ///
        /// <remarks>
        ///     Anwar Javed, 09/24/2013 9:38 AM.
        /// </remarks>
        ///
        /// <typeparam name="T">
        ///     Generic type parameter.
        /// </typeparam>
        /// <param name="value">
        ///     The value.
        /// </param>
        /// <param name="mode">
        ///     (optional) the mode.
        /// </param>
        /// <param name="nullValue">
        ///     (optional) the null value.
        /// </param>
        ///
        /// <returns>
        ///     .
        /// </returns>
        ///-------------------------------------------------------------------------------------------------
        public string Serialize<T>(T value, SerializationMode mode = SerializationMode.None, ValueHandlingMode nullValue = ValueHandlingMode.Ignore)
        {
            return this.Serialize((object)value, mode, nullValue);
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Serializes the given value.
        /// </summary>
        ///
        /// <remarks>
        ///     Anwar Javed, 09/24/2013 9:37 AM.
        /// </remarks>
        ///
        /// <exception cref="SerializationException">
        ///     Thrown when a Serialization error condition occurs.
        /// </exception>
        ///
        /// <param name="value">
        ///     The value.
        /// </param>
        /// <param name="mode">
        ///     (optional) the mode.
        /// </param>
        /// <param name="nullValue">
        ///     (optional) the null value.
        /// </param>
        ///
        /// <returns>
        ///     .
        /// </returns>
        ///-------------------------------------------------------------------------------------------------
        public string Serialize(object value, SerializationMode mode = SerializationMode.None, ValueHandlingMode nullValue = ValueHandlingMode.Ignore)
        {
            try
            {
                JsonSerializerSettings settings = new JsonSerializerSettings()
                                                                {
                                                                    Formatting = Formatting.Indented,
                                                                    NullValueHandling = NullValueHandling.Ignore,
                                                                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                                                                };

                settings.Converters.Add(new GuidConverter());
                settings.Converters.Add(new StringEnumConverter());

                switch (mode)
                {
                    case SerializationMode.Compact:
                        settings.Formatting = Formatting.None;
                        break;
                }

                switch (nullValue)
                {
                    case ValueHandlingMode.Include:
                        settings.NullValueHandling = NullValueHandling.Include;
                        break;
                }

                return JsonConvert.SerializeObject(value, settings);
            }
            catch (Exception exception)
            {
                throw new SerializationException(exception.Message, exception);
            }
        }
    }
}
