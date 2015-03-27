namespace Framework.Caching.Impl
{
    using System;
    using System.Collections.Generic;
    using System.Data.SqlClient;
    using System.Linq;
    using System.Runtime.Caching;
    using System.Security;
    using System.Web.Configuration;

    using Framework.Caching.SqlCache.Domain;
    using Framework.Domain;
    using Framework.Ioc;
    using Framework.Serialization.Json;

    [InjectBind(typeof(ICache), "SqlCache", LifetimeType.Singleton)]
    public class SqlCache : MemoryCache
    {
        private readonly string nameOrConnectionString;

        private SqlChangeMonitor monitor;
        private SqlDependency dependency;
        private bool hasDataChanged;

        private bool loaded;

        public SqlCache()
        {
            var overriddenConnectionString = WebConfigurationManager.AppSettings["Framework.Caching.SqlCacheContext"];
            if (!string.IsNullOrWhiteSpace(overriddenConnectionString))
            {
                this.nameOrConnectionString = overriddenConnectionString;
            }

            if (string.IsNullOrWhiteSpace(this.nameOrConnectionString))
            {
                this.nameOrConnectionString = "AppContext";
            }

            SqlDependency.Start(WebConfigurationManager.ConnectionStrings[this.nameOrConnectionString].ConnectionString);
        }

        protected override ObjectCache InternalCache
        {
            get
            {
                this.InitCache();
                return base.InternalCache;
            }
        }

        [SecuritySafeCritical]
        private void InitCache()
        {
            if (!this.loaded)
            {
                // loads the cache item.
                this.LoadCacheData();
                this.loaded = true;
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
        public override void Add(string key, object value, CacheItemPolicy policy)
        {
            this.Add(new CacheItem(key, value), policy);
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
        [SecuritySafeCritical]
        public override void Add(CacheItem cacheItem, CacheItemPolicy policy)
        {
            IJsonSerializer serializer = Container.Get<IJsonSerializer>();

            using (SqlCacheContext context = new SqlCacheContext(this.nameOrConnectionString))
            {
                this.dependency.OnChange -= this.OnDependencyChange;
                SqlCacheItem sqlCacheItem = context.SqlCacheItems.FirstOrDefault(x => x.Name == cacheItem.Key);

                if (sqlCacheItem == null)
                {
                    sqlCacheItem = new SqlCacheItem();
                    sqlCacheItem.Name = cacheItem.Key;

                    context.SqlCacheItems.Add(sqlCacheItem);
                }

                var itemData = BuildCacheItemData(cacheItem, policy);
                sqlCacheItem.Value = serializer.Serialize(itemData);
                context.SaveChanges();
                this.dependency.OnChange += this.OnDependencyChange;

            }

           
            if (base[cacheItem.Key] == null)
            {
                policy.RemovedCallback += this.RemoveCacheItem;
               
            }
            else
            {
                this.UpdateCacheItem(cacheItem, policy);
            }

            base.Add(cacheItem, policy);
        }

        private static SqlCacheItemData BuildCacheItemData(CacheItem cacheItem, CacheItemPolicy policy)
        {
            SqlCacheItemData itemData = new SqlCacheItemData();
            itemData.AbsoluteExpiration = policy.AbsoluteExpiration;
            itemData.Priority = policy.Priority;
            itemData.SlidingExpiration = policy.SlidingExpiration;

            var filesToMonitor = new List<string>();
            if (policy.ChangeMonitors != null)
            {
                foreach (var changeMonitor in policy.ChangeMonitors)
                {
                    var fileChangeMonitor = changeMonitor as FileChangeMonitor;
                    if (fileChangeMonitor != null)
                    {
                        filesToMonitor.AddRange(fileChangeMonitor.FilePaths);
                    }
                }
            }

            itemData.FilesToMonitor = filesToMonitor.ToArray();
            itemData.Data = cacheItem.Value;
            return itemData;
        }

        [SecuritySafeCritical]
        private void UpdateCacheItem(CacheItem cacheItem, CacheItemPolicy policy)
        {
            IJsonSerializer serializer = Container.Get<IJsonSerializer>();
            this.dependency.OnChange -= this.OnDependencyChange;

            try
            {
                using (SqlCacheContext context = new SqlCacheContext(this.nameOrConnectionString))
                {
                    SqlCacheItem sqlCacheItem = context.SqlCacheItems.FirstOrDefault(x => x.Name == cacheItem.Key);

                    if (sqlCacheItem != null)
                    {

                        var itemData = BuildCacheItemData(cacheItem, policy);
                        sqlCacheItem.Value = serializer.Serialize(itemData);
                        context.SaveChanges();
                    }
                }
            }
            catch (Exception)
            {

            }
            finally
            {
                this.dependency.OnChange += this.OnDependencyChange;
            }
        }

        [SecuritySafeCritical]
        private void LoadCacheData()
        {
            const string SqlStatement = "SELECT [Name], [Value], [TypeName] FROM SqlCache";

            this.Clear();
            IJsonSerializer serializer = Container.Get<IJsonSerializer>();
            using (SqlCacheContext context = new SqlCacheContext(this.nameOrConnectionString))
            {
                foreach (var sqlCacheItem in context.SqlCacheItems)
                {
                    if (!string.IsNullOrWhiteSpace(sqlCacheItem.Value))
                    {
                        var itemData = serializer.Deserialize<SqlCacheItemData>(sqlCacheItem.Value);

                        var cachePolicy = new CacheItemPolicy();
                        cachePolicy.AbsoluteExpiration = itemData.AbsoluteExpiration;
                        cachePolicy.SlidingExpiration = itemData.SlidingExpiration;
                        cachePolicy.Priority = itemData.Priority;

                        if (itemData.FilesToMonitor != null && itemData.FilesToMonitor.Length > 0)
                        {
                            cachePolicy.ChangeMonitors.Add(new HostFileChangeMonitor(itemData.FilesToMonitor));
                        }

                        cachePolicy.RemovedCallback += this.RemoveCacheItem;

                        base.Add(sqlCacheItem.Name, itemData.Data, cachePolicy);
                    }

                }
            }

            this.dependency = new SqlDependency(new SqlCommand(SqlStatement));
            this.dependency.OnChange += this.OnDependencyChange;


            // Create a new monitor.
            this.monitor = new SqlChangeMonitor(this.dependency);

            // Create a policy.
            var policy = new CacheItemPolicy();
            policy.ChangeMonitors.Add(this.monitor);
            policy.UpdateCallback = this.SqlCacheUpdateCallback;

            // Put results into Cache Item.
            var cacheItem = new CacheItem("CacheDependencyNotification", true);
            base.Add(cacheItem, policy);

            // Reset the data changed flag.
            this.hasDataChanged = false;
        }

        [SecuritySafeCritical]
        private void RemoveCacheItem(CacheEntryRemovedArguments arguments)
        {
            if (arguments.CacheItem != null)
            {
                this.dependency.OnChange -= this.OnDependencyChange;
                try
                {
                    using (SqlCacheContext context = new SqlCacheContext(this.nameOrConnectionString))
                    {
                        SqlCacheItem sqlCacheItem = context.SqlCacheItems.FirstOrDefault(x => x.Name == arguments.CacheItem.Key);

                        if (sqlCacheItem != null)
                        {
                            context.SqlCacheItems.Remove(sqlCacheItem);
                            context.SaveChanges();
                        }
                    }
                }
                catch (Exception)
                {

                }
                finally
                {
                    this.dependency.OnChange += this.OnDependencyChange;
                }
            }

        }

        void OnDependencyChange(object sender, SqlNotificationEventArgs e)
        {
            // DataChange Detection
            this.hasDataChanged = true;

        }

        private void SqlCacheUpdateCallback(CacheEntryUpdateArguments args)
        {
            // Dispose of monitor
            if (this.monitor != null)
                this.monitor.Dispose();

            // Disconnect event to prevent recursion.
            this.dependency.OnChange -= this.OnDependencyChange;

            // Refresh the cache if tracking data changes.
            if (this.hasDataChanged)
            {
                // Refresh the cache item.
                this.LoadCacheData();
            }

        }
    }
}
