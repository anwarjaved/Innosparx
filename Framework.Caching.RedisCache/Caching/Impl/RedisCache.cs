using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework.Caching.Impl
{
    using System.Runtime.Caching;

    using Framework.Caching;

    using Framework.Caching;

    using Framework.Caching;

    using Framework.Caching;
    using Framework.Ioc;
    using Framework.Serialization.Json;

    using IX.Redis;

    ///-------------------------------------------------------------------------------------------------
    /// <summary>
    ///     The redis cache.
    /// </summary>
    ///-------------------------------------------------------------------------------------------------

    public class RedisCache : ICache
    {
        private readonly IRedisClient client;

        private readonly IJsonSerializer serializer;

        public RedisCache(string host, int port = 6379)
        {
            this.client = new RedisClient(host, port);
            serializer = Container.Get<IJsonSerializer>();
        }

        /// <summary>
        /// Returns the item identified by the provided key.
        /// </summary>
        /// <param name="key">Key to retrieve from cache.</param>
        public string this[string key]
        {
            get
            {
                return this.client.Get(key);
            }
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

            string json = this[key];

            if (string.IsNullOrWhiteSpace(json))
            {
                return default(T);
            }

            var obj = serializer.Deserialize<T>(json);

            return obj;
        }

        /// <summary>
        /// Removes the given item from the cache. If no item exists with that key, this method does nothing.
        /// </summary>
        /// <param name="key">Key of item to remove from cache.</param>
        public void Remove(string key)
        {
            this.client.Del(key);
        }

        /// <summary>
        /// Returns true if key refers to item current stored in cache.
        /// </summary>
        /// <param name="key">Key of item to check for.</param>
        /// <returns>True if item referenced by key is in the cache.</returns>
        public bool Exists(string key)
        {
            return this.client.Exists(key);
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///      Add/Update new CacheItem to cache. If another item already exists with the same key, that item
        ///     is removed before the new item is added. If any failure occurs during this process, the
        ///     cache will not contain the item being added. Items added with this method will be not
        ///     expire, and will have a Normal <see cref="CacheItemPriority" /> priority.
        /// </summary>
        ///
        /// <param name="key">
        ///     Identifier for this CacheItem.
        /// </param>
        /// <param name="value">
        ///     Value to be stored in cache. May be null.
        /// </param>
        /// <param name="filePaths">
        ///     The path of the file on which cache will be invalidated.
        /// </param>
        public void Set(string key, object value, params string[] filePaths)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Adds new CacheItem to cache. If another item already exists with the same key, that item is removed before
        /// the new item is added. If any failure occurs during this process, the cache will not contain the item being added. 
        /// Items added with this method will be not expire, and will have a Normal <see cref="System.Runtime.Caching.CacheItemPriority" /> priority.
        /// </summary>
        /// <param name="key">Identifier for this CacheItem.</param>
        /// <param name="value">Value to be stored in cache. May be null.</param>
        public void Set(string key, object value)
        {
            this.client.Set(key, Serialize(value));
        }

        /// <summary>
        /// Adds new CacheItem to cache. If another item already exists with the same key, that item is removed before
        /// the new item is added. If any failure occurs during this process, the cache will not contain the item being added.
        /// </summary>
        /// <param name="key">Identifier for this CacheItem.</param>
        /// <param name="value">Value to be stored in cache. May be null.</param>
        /// <param name="expirations">The expirations date time.</param>
        public void Set(string key, object value, TimeSpan expirations)
        {
            this.client.Set(key, Serialize(value), expirations);
        }

        private string Serialize(object value)
        {
            return serializer.Serialize(value);
        }

        /// <summary>
        /// Adds new CacheItem to cache. If another item already exists with the same key, that item is removed before
        /// the new item is added. If any failure occurs during this process, the cache will not contain the item being added.
        /// </summary>
        /// <param name="key">Identifier for this CacheItem.</param>
        /// <param name="value">Value to be stored in cache. May be null.</param>
        /// <param name="expirations">The expirations date time.</param>
        public void Set(string key, object value, DateTime expirations)
        {
            TimeSpan duration = expirations.Subtract(DateTime.Now);
            this.client.Set(key, Serialize(value), duration);
        }
    }
}
