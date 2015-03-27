namespace Framework.Serialization.Json.Converters
{
    using System;

    using Newtonsoft.Json;

    ///-------------------------------------------------------------------------------------------------
    /// <summary>
    ///     Unique identifier converter.
    /// </summary>
    ///-------------------------------------------------------------------------------------------------
    public class GuidConverter : JsonConverter
    {
        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Writes the JSON representation of the object.
        /// </summary>
        ///
        /// <param name="writer">
        ///     The <see cref="T:Newtonsoft.Json.JsonWriter"/> to write to.
        /// </param>
        /// <param name="value">
        ///     The value.
        /// </param>
        /// <param name="serializer">
        ///     The calling serializer.
        /// </param>
        ///-------------------------------------------------------------------------------------------------
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            if (value == null)
            {
                writer.WriteNull();
            }
            else
            {
                Guid guid = (Guid)value;
                writer.WriteValue(guid.ToStringValue());
            }
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Reads the JSON representation of the object.
        /// </summary>
        ///
        /// <exception cref="JsonSerializationException">
        ///     Thrown when a JSON Serialization error condition occurs.
        /// </exception>
        ///
        /// <param name="reader">
        ///     The <see cref="T:Newtonsoft.Json.JsonReader"/> to read from.
        /// </param>
        /// <param name="objectType">
        ///     Type of the object.
        /// </param>
        /// <param name="existingValue">
        ///     The existing value of object being read.
        /// </param>
        /// <param name="serializer">
        ///     The calling serializer.
        /// </param>
        ///
        /// <returns>
        ///     The object value.
        /// </returns>
        ///-------------------------------------------------------------------------------------------------
        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            var isNullableType = objectType.IsNullableType();

            if ((reader.TokenType == JsonToken.Null) && !isNullableType)
            {
                throw new JsonSerializationException("Cannot convert null value to {0}.".FormatString(objectType));
            }

            var value = reader.Value;

            if (!isNullableType && (value == null || string.IsNullOrWhiteSpace(value.ToString())))
            {
                throw new JsonSerializationException("Unexpected token when parsing guid. Expected string, got {0}.".FormatString(reader.TokenType));
            }

            return Guid.Parse(value.ToString());
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Determines whether this instance can convert the specified object type.
        /// </summary>
        ///
        /// <param name="objectType">
        ///     Type of the object.
        /// </param>
        ///
        /// <returns>
        ///     <c>true</c> if this instance can convert the specified object type; otherwise,
        ///     <c>false</c>.
        /// </returns>
        ///-------------------------------------------------------------------------------------------------
        public override bool CanConvert(Type objectType)
        {
            Type t = objectType.IsNullableType() ? Nullable.GetUnderlyingType(objectType) : objectType;
            return t == typeof(Guid);
        }
    }
}
