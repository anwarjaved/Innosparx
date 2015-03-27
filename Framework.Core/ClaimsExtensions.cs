
namespace Framework
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Security.Claims;

    ///-------------------------------------------------------------------------------------------------
    /// <summary>
    ///     Claims extensions.
    /// </summary>
    ///-------------------------------------------------------------------------------------------------
    public static class ClaimsExtensions
    {
        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     An IEnumerable&lt;Claim&gt; extension method that gets a value.
        /// </summary>
        ///
        /// <param name="claims">
        ///     The claims to act on.
        /// </param>
        /// <param name="type">
        ///     The type.
        /// </param>
        ///
        /// <returns>
        ///     The value.
        /// </returns>
        ///-------------------------------------------------------------------------------------------------
        public static string GetValue(this IEnumerable<Claim> claims, string type)
        {
            if (claims != null)
            {
                var claim = claims.SingleOrDefault(x => x.Type == type);
                if (claim != null) return claim.Value;
            }

            return null;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Enumerates get values in this collection.
        /// </summary>
        ///
        /// <param name="claims">
        ///     The claims to act on.
        /// </param>
        /// <param name="claimType">
        ///     Type of the claim.
        /// </param>
        ///
        /// <returns>
        ///     An enumerator that allows foreach to be used to process get values in this collection.
        /// </returns>
        ///-------------------------------------------------------------------------------------------------
        public static IEnumerable<string> GetValues(this IEnumerable<Claim> claims, string claimType)
        {
            if (claims == null) return Enumerable.Empty<string>();

            var query =
                from claim in claims
                where claim.Type == claimType
                select claim.Value;
            return query.ToArray();
        }
    }
}
