namespace Framework.Caching
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.Caching;

    /// <summary>
    /// Interface implemented by each storage.
    /// </summary>
    public interface ICache
    {
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

        T Get<T>(string key);

        /// <summary>
        /// Removes the given item from the cache. If no item exists with that key, this method does nothing.
        /// </summary>
        /// <param name="key">Key of item to remove from cache.</param>
        void Remove(string key);

        /// <summary>
        /// Returns true if key refers to item current stored in cache.
        /// </summary>
        /// <param name="key">Key of item to check for.</param>
        /// <returns>True if item referenced by key is in the cache.</returns>
        bool Exists(string key);

        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Add/Update new CacheItem to cache. If another item already exists with the same key, that item
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
        ///-------------------------------------------------------------------------------------------------
        void Set(string key, object value);

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
        void Set(string key, object value, params string[] filePaths);

        /// <summary>
        ///  Add/Update new CacheItem to cache. If another item already exists with the same key, that item is removed before
        /// the new item is added. If any failure occurs during this process, the cache will not contain the item being added.
        /// </summary>
        /// <param name="key">Identifier for this CacheItem.</param>
        /// <param name="value">Value to be stored in cache. May be null.</param>
        /// <param name="expirations">The expirations date time.</param>
        void Set(string key, object value, DateTime expirations);

        /// <summary>
        /// Adds new CacheItem to cache. If another item already exists with the same key, that item is removed before
        /// the new item is added. If any failure occurs during this process, the cache will not contain the item being added.
        /// </summary>
        /// <param name="key">Identifier for this CacheItem.</param>
        /// <param name="value">Value to be stored in cache. May be null.</param>
        /// <param name="expirations">The expirations date time.</param>
        void Set(string key, object value, TimeSpan expirations);
    }
}