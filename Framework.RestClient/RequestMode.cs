namespace Framework.Rest
{
    /// <summary>
    /// Enumeration of the various HTTP Request modes.
    /// </summary>
    public enum RequestMode
    {
        /// <summary>
        /// No Request Mode specified.
        /// </summary>
        None,

        /// <summary>
        /// Url Encoded Data.
        /// </summary>
        UrlEncoded,

        /// <summary>
        /// Json Content Type.
        /// </summary>
        Json,

        /// <summary>
        /// MultiPart Content Type.
        /// </summary>
        MultiPart,

        /// <summary>
        /// Use Xml Data. 
        /// </summary>
        Xml,

        /// <summary>
        /// Use Raw Data.
        /// </summary>
        Raw,
    }
}
