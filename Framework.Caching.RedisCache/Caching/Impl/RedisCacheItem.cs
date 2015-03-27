namespace Framework.Caching.Impl
{
    using System;

    public class RedisCacheItem
    {
        public Type Type { get; set; }

        public string Data { get; set; }
    }
}
