namespace Framework.Caching.Impl
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.Caching;

    using Framework.Ioc;

    /// <summary>
    ///     Cache Service.
    /// </summary>
    [InjectBind(typeof(ICache), LifetimeType.Singleton)]
    [InjectBind(typeof(ICache), "MemoryCache", LifetimeType.Singleton)]
    public class MemoryCache : ICache
    {
        private static readonly TimeSpan DefaultTimeout = new TimeSpan(0, 0, 30, 0);

        private readonly ObjectCache internalCache;

        /// <summary>
        ///     Initializes a new instance of the <see cref="MemoryCache" /> class.
        /// </summary>
        public MemoryCache()
        {
            this.internalCache = new System.Runtime.Caching.MemoryCache(typeof(MemoryCache).FullName);
        }

        /// <summary>
        ///     Gets the number of items currently in the cache.
        /// </summary>
        /// <value>The number of items currently in the cache.</value>
        public long Count
        {
            get
            {
                return this.InternalCache.GetCount();
            }
        }

        protected virtual ObjectCache InternalCache
        {
            get
            {
                return this.internalCache;
            }
        }

        /// <summary>
        ///     Returns the item identified by the provided key.
        /// </summary>
        /// <param name="key">Key to retrieve from cache.</param>
        public object this[string key]
        {
            get
            {
                return this.InternalCache.Get(key);
            }
        }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Adds new CacheItem to cache. If another item already exists with the same key, that item
        ///     is removed before the new item is added. If any failure occurs during this process, the
        ///     cache will not contain the item being added. Items added with this method will be not
        ///     expire, and will have a Normal <see cref="CacheItemPriority" /> priority.
        /// </summary>
        /// <param name="key">
        ///     Identifier for this CacheItem.
        /// </param>
        /// <param name="value">
        ///     Value to be stored in cache. May be null.
        /// </param>
        /// <param name="policy">
        ///     The policy.
        /// </param>
        /// -------------------------------------------------------------------------------------------------
        public virtual void Set(string key, object value, CacheItemPolicy policy)
        {
            this.InternalCache.Add(key, value, policy);
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Adds new CacheItem to cache. If another item already exists with the same key, that item
        ///     is removed before the new item is added. If any failure occurs during this process, the
        ///     cache will not contain the item being added. Items added with this method will be not
        ///     expire, and will have a Normal <see cref="CacheItemPriority" /> priority.
        /// </summary>
        ///
        /// <param name="cacheItem">    Identifier for this CacheItem. </param>
        /// <param name="policy">       The policy. </param>
        ///-------------------------------------------------------------------------------------------------
        public virtual void Set(CacheItem cacheItem, CacheItemPolicy policy)
        {
            this.InternalCache.Set(cacheItem, policy);
        }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Adds new CacheItem to cache. If another item already exists with the same key, that item
        ///     is removed before the new item is added. If any failure occurs during this process, the
        ///     cache will not contain the item being added. Items added with this method will be not
        ///     expire, and will have a Normal <see cref="CacheItemPriority" /> priority.
        /// </summary>
        /// <param name="key">
        ///     Identifier for this CacheItem.
        /// </param>
        /// <param name="value">
        ///     Value to be stored in cache. May be null.
        /// </param>
        /// <param name="filePaths">
        ///     The path of the files on which cache will be invalidated.
        /// </param>
        public void Set(string key, object value, params string[] filePaths)
        {
            var policy = new CacheItemPolicy();
            policy.ChangeMonitors.Add(new HostFileChangeMonitor(filePaths));

            this.Set(key, value, policy);
        }

        /// <summary>
        ///     Adds new CacheItem to cache. If another item already exists with the same key, that item is removed before
        ///     the new item is added. If any failure occurs during this process, the cache will not contain the item being added.
        ///     Items added with this method will be not expire, and will have a Normal <see cref="CacheItemPriority" /> priority.
        /// </summary>
        /// <param name="key">Identifier for this CacheItem.</param>
        /// <param name="value">Value to be stored in cache. May be null.</param>
        public void Set(string key, object value)
        {
            this.Set(key, value, DefaultTimeout);
        }

        /// <summary>
        ///     Adds new CacheItem to cache. If another item already exists with the same key, that item is removed before
        ///     the new item is added. If any failure occurs during this process, the cache will not contain the item being added.
        /// </summary>
        /// <param name="key">Identifier for this CacheItem.</param>
        /// <param name="value">Value to be stored in cache. May be null.</param>
        /// <param name="expirations">The expirations date time.</param>
        public void Set(
            string key,
            object value,
            DateTime expirations)
        {
            this.Set(key, value, new CacheItemPolicy() { AbsoluteExpiration = expirations, Priority = CacheItemPriority.Default });
        }

        /// <summary>
        ///     Adds new CacheItem to cache. If another item already exists with the same key, that item is removed before
        ///     the new item is added. If any failure occurs during this process, the cache will not contain the item being added.
        /// </summary>
        /// <param name="key">Identifier for this CacheItem.</param>
        /// <param name="value">Value to be stored in cache. May be null.</param>
        /// <param name="expirations">The expirations date time.</param>
        public void Set(
            string key,
            object value,
            TimeSpan expirations)
        {
            this.Set(key, value, new CacheItemPolicy() { SlidingExpiration = expirations, Priority = CacheItemPriority.Default });
        }

        /// <summary>
        ///     Removes all items from the cache. If an error occurs during the removal, the cache is left unchanged.
        /// </summary>
        /// <remarks>
        ///     The CacheManager can be configured to use different storage mechanisms in which to store the CacheItems.
        ///     Each of these storage mechanisms can throw exceptions particular to their own implementations.
        /// </remarks>
        public virtual void Clear()
        {
            IEnumerable<string> cacheKeys = this.GetCacheKeys();
            foreach (string s in cacheKeys)
            {
                this.Remove(s);
            }
        }

        /// <summary>
        ///     Returns true if key refers to item current stored in cache.
        /// </summary>
        /// <param name="key">Key of item to check for.</param>
        /// <returns>True if item referenced by key is in the cache.</returns>
        public virtual bool Exists(string key)
        {
            return this.InternalCache[key] != null;
        }

        /// <summary>
        ///     Gets the cache keys.
        /// </summary>
        /// <returns>Get All Cache keys.</returns>
        public IEnumerable<string> GetCacheKeys()
        {
            var keys = new List<string>();

            IEnumerator<KeyValuePair<string, object>> ca =
                ((IEnumerable<KeyValuePair<string, object>>)this.InternalCache).GetEnumerator();

            while (ca.MoveNext())
            {
                KeyValuePair<string, object> keyValuePair = ca.Current;
                string key = keyValuePair.Key;
                keys.Add(key);
            }

            return keys;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Returns the item identified by the provided key.
        /// </summary>
        ///
        /// <tparam name="T">
        ///     Generic type parameter.
        /// </tparam>
        /// <param name="key">
        ///     Key of item to remove from cache.
        /// </param>
        ///
        /// <returns>
        ///     A T.
        /// </returns>
        ///-------------------------------------------------------------------------------------------------
        public T Get<T>(string key)
        {
            return (T)this[key];
        }

        /// <summary>
        ///     Removes the given item from the cache. If no item exists with that key, this method does nothing.
        /// </summary>
        /// <param name="key">Key of item to remove from cache.</param>
        public virtual void Remove(string key)
        {
            if (this.Exists(key))
            {
                this.InternalCache.Remove(key);
            }
        }
    }
}