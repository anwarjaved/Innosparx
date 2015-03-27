using System;

namespace Framework.Serialization.Binary.Impl
{
    using System.IO;
    using System.Runtime.Serialization;
    using System.Runtime.Serialization.Formatters.Binary;

    using Framework.Ioc;

    [InjectBind(typeof(ISerializer), "Binary", LifetimeType.Singleton)]
    [InjectBind(typeof(IBinarySerializer), LifetimeType.Singleton)]
    public class BinarySerializer : IBinarySerializer
    {
        public object Deserialize(Type type, string value)
        {
            try
            {
                using (var stream = new MemoryStream(Convert.FromBase64String(value)))
                {
                    stream.Position = 0;

                    BinaryFormatter formatter = new BinaryFormatter();

                    return formatter.Deserialize(stream);
                }
            }
            catch (Exception exception)
            {
                throw new SerializationException(exception.Message, exception);
            }
        }

        public T Deserialize<T>(string value)
        {
            return (T)this.Deserialize(typeof(T), value);
        }

        public string Serialize<T>(T value, SerializationMode mode = SerializationMode.None, ValueHandlingMode nullValue = ValueHandlingMode.Ignore)
        {
            return this.Serialize((object)value, mode, nullValue);
        }

        public string Serialize(object value, SerializationMode mode = SerializationMode.None, ValueHandlingMode nullValue = ValueHandlingMode.Ignore)
        {
            BinaryFormatter formatter = new BinaryFormatter();

            using (var stream = new MemoryStream())
            {
                formatter.Serialize(stream, value);
                stream.Position = 0;

                return Convert.ToBase64String(stream.ToArray());
            }
        }
    }
}
