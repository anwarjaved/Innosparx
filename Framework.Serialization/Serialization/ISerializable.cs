namespace Framework.Serialization
{
    ///-------------------------------------------------------------------------------------------------
    /// <summary>
    ///     Allows an object to control its own serialization and deserialization.
    /// </summary>
    ///-------------------------------------------------------------------------------------------------
    public interface ISerializable
    {
        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///    Serializes current instance.
        /// </summary>
        ///
        /// <returns>
        ///     Serialized object as <see cref="string"/>.
        /// </returns>
        ///-------------------------------------------------------------------------------------------------
        string Serialize();

        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Deserializes value into current object.
        /// </summary>
        ///
        /// <param name="value">
        ///     The value.
        /// </param>
        ///-------------------------------------------------------------------------------------------------
        void Deserialize(string value);
    }
}
