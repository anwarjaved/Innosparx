namespace Framework.Caching.Impl
{
    using System;
    using System.Runtime.Caching;

    internal class SqlCacheItemData
    {
        public object Data { get; set; }

        public CacheItemPriority Priority { get; set; }

        public DateTimeOffset AbsoluteExpiration { get; set; }

        public TimeSpan SlidingExpiration { get; set; }

        public string[] FilesToMonitor { get; set; }
    }
}
