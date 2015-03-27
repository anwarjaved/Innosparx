namespace Framework.Serialization.Json.Converters
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;

    using Newtonsoft.Json;

    ///-------------------------------------------------------------------------------------------------
    /// <summary>
    ///     String enum converter.
    /// </summary>
    ///-------------------------------------------------------------------------------------------------
    public class StringEnumConverter : JsonConverter
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
                Enum e = (Enum)value;

                Type type = e.GetType();

                List<Type> skipAttributes = type.GetCustomAttributes<SkipAttribute>().SelectMany(x => x.Types).ToList();

                writer.WriteValue(
                    skipAttributes.Contains(typeof(StringEnumConverter)) ? e.ToString("D") : e.ToString("G"));
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
            Type type = isNullableType ? Nullable.GetUnderlyingType(objectType) : objectType;

            if (reader.TokenType == JsonToken.Null && !isNullableType)
            {
                throw new JsonSerializationException("Cannot convert null value to {0}.".FormatString(objectType));
            }

            if (reader.TokenType == JsonToken.Null)
            {
                return null;
            }
            if (reader.TokenType == JsonToken.String)
            {
                var pairs = type.EnumToDictionaryValues();

                if (pairs.ContainsKey(reader.Value.ToString()))
                {
                    return Enum.Parse(type, pairs[reader.Value.ToString()].ToString());
                }
            }

            if (reader.TokenType != JsonToken.Integer && reader.TokenType != JsonToken.String)
            {
                throw new JsonSerializationException("Unexpected token when parsing enum. Expected String or Integer, got {0}.".FormatString(reader.TokenType));
            }

            return Enum.Parse(type, Convert.ToInt32(reader.Value).ToString());
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
            return t.IsEnum;
        }
    }
}
