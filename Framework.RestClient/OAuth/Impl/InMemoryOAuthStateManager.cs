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

    [InjectBind(typeof(IOAuthStateManager), LifetimeType.Singleton)]
    public class InMemoryOAuthStateManager : IOAuthStateManager
    {
        private readonly ICache cache;
        public InMemoryOAuthStateManager(ICache cache)
        {
            this.cache = cache;
        }

        public void SaveState(string key, OAuthState state)
        {
            cache.Set(BuildCacheKey(key), state, TimeSpan.FromMinutes(15));
        }

        public OAuthState GetState(string key)
        {
            return cache.Get<OAuthState>(BuildCacheKey(key));
        }

        private static string BuildCacheKey(string key)
        {
            return typeof(InMemoryOAuthStateManager).Name + "." + key;
        }
    }
}
