namespace Framework
{
    using System.Security.Claims;

    ///-------------------------------------------------------------------------------------------------
    /// <summary>
    ///     Claims principal extensions.
    /// </summary>
    ///-------------------------------------------------------------------------------------------------
    public static class ClaimsPrincipalExtensions
    {
        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     A ClaimsPrincipal extension method that query if 'user' has claim.
        /// </summary>
        ///
        /// <param name="user">
        ///     The user to act on.
        /// </param>
        /// <param name="type">
        ///     The type.
        /// </param>
        ///
        /// <returns>
        ///     true if claim, false if not.
        /// </returns>
        ///-------------------------------------------------------------------------------------------------
        public static bool HasClaim(this ClaimsPrincipal user, string type)
        {
            if (user != null)
            {
                return user.HasClaim(x => x.Type == type);
            }
            return false;
        }
    }
}
