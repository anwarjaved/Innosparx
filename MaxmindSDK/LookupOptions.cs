namespace MaxmindSDK
{
    public enum LookupOptions
    {
        /// <summary>
        /// read database from filesystem, uses least memory.
        /// </summary>
        Standard,

        /// <summary>
        /// load database into memory, faster performance but uses more memory
        /// </summary>
        Cache
    }
}
