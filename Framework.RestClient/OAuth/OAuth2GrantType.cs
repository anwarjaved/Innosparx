namespace Framework.Rest.OAuth
{
    ///-------------------------------------------------------------------------------------------------
    /// <summary>
    ///     Values that represent OAuth2GrantType.
    /// </summary>
    ///-------------------------------------------------------------------------------------------------
    public enum OAuth2GrantType
    {
        /// <summary>
        ///     Authorization Code.
        /// </summary>
        [Description("authorization_code")]
        AuthorizationCode = 0,

        /// <summary>
        ///     Password.
        /// </summary>
        [Description("password")]
        Password = 1,

        /// <summary>
        ///     Client Credentials.
        /// </summary>
        [Description("client_credentials")]
        ClientCredentials = 2,

        /// <summary>
        ///     Refresh Token.
        /// </summary>
        [Description("refresh_token")]
        RefreshToken = 3,
    }
}
