namespace Framework.Rest.OAuth
{
    ///-------------------------------------------------------------------------------------------------
    /// <summary>
    ///     Interface for token manager.
    /// </summary>
    ///
    /// <remarks>
    ///     Anwar Javed, 09/04/2013 4:36 PM.
    /// </remarks>
    ///-------------------------------------------------------------------------------------------------
    public interface ITokenManager
    {
        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Stores request token.
        /// </summary>
        ///
        /// <param name="key">
        ///     The key.
        /// </param>
        /// <param name="credential">
        ///     The credential.
        /// </param>
        ///-------------------------------------------------------------------------------------------------
        void SaveRequestToken(string key, OAuth1Token credential);

        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Gets request token.
        /// </summary>
        ///
        /// <param name="key">
        ///     The key.
        /// </param>
        ///
        /// <returns>
        ///     The request token.
        /// </returns>
        ///-------------------------------------------------------------------------------------------------
        OAuth1Token GetRequestToken(string key);
    }
}
