namespace Framework.Domain
{
    public class SqlCacheItem
    {
        public string Name { get; set; }

        public string Value { get; set; }

        public byte[] RowVersion { get; protected internal set; }
    }
}
