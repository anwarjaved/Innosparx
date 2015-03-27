using System;

namespace Framework.Serialization
{
    ///-------------------------------------------------------------------------------------------------
    /// <summary>
    ///     Interface for serializer.
    /// </summary>
    ///-------------------------------------------------------------------------------------------------
    public interface ISerializer
    {
        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Deserializes Object.
        /// </summary>
        ///
        /// <param name="type">
        ///     The type.
        /// </param>
        /// <param name="value">
        ///     The value.
        /// </param>
        ///
        /// <returns>
        ///     Deserialized object.
        /// </returns>
        ///-------------------------------------------------------------------------------------------------
        object Deserialize(Type type, string value);

        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Deserializes Object.
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
        ///     Deserialized object.
        /// </returns>
        ///-------------------------------------------------------------------------------------------------
        T Deserialize<T>(string value);

        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Serializes the given value.
        /// </summary>
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
        ///     Serialiazed Value.
        /// </returns>
        ///-------------------------------------------------------------------------------------------------
        string Serialize<T>(T value, SerializationMode mode = SerializationMode.None, ValueHandlingMode nullValue = ValueHandlingMode.Ignore);

        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Serializes the given value.
        /// </summary>
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
        ///     Serialiazed Value.
        /// </returns>
        ///-------------------------------------------------------------------------------------------------
        string Serialize(object value, SerializationMode mode = SerializationMode.None, ValueHandlingMode nullValue = ValueHandlingMode.Ignore);
    }
}
