namespace Framework.Rest.OAuth
{
    ///-------------------------------------------------------------------------------------------------
    /// <summary>
    ///     Values that represent OAuth2ResponseType.
    /// </summary>
    ///-------------------------------------------------------------------------------------------------
    public enum OAuth2ResponseType
    {
        /// <summary>
        ///     Code.
        /// </summary>
        [Description("code")]
        Code = 0,

        /// <summary>
        ///     Token.
        /// </summary>
        [Description("token")]
        Token = 1
    }
}
