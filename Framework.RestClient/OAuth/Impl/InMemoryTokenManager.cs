using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework.Rest.OAuth.Impl
{
    using Framework.Caching;
    using Framework.Infrastructure;
    using Framework.Ioc;

    [InjectBind(typeof(ITokenManager), LifetimeType.Singleton)]
    public class InMemoryTokenManager : ITokenManager
    {
        private readonly ICache cache;

        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Initializes a new instance of the InMemoryTokenManager class.
        /// </summary>
        ///
        /// <remarks>
        ///     Anwar Javed, 09/04/2013 4:36 PM.
        /// </remarks>
        ///
        /// <param name="cache">
        ///     The cache.
        /// </param>
        ///-------------------------------------------------------------------------------------------------
        public InMemoryTokenManager(ICache cache)
        {
            this.cache = cache;
        }

        public void SaveRequestToken(string key, OAuth1Token credential)
        {
            cache.Set(BuildCacheKey(key), credential, TimeSpan.FromMinutes(15));
        }

        public OAuth1Token GetRequestToken(string key)
        {
            return cache.Get<OAuth1Token>(BuildCacheKey(key));
        }

        private static string BuildCacheKey(string key)
        {
            return typeof(InMemoryTokenManager).Name + "." + key;
        }
    }
}
