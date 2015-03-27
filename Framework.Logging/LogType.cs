namespace Framework.Logging
{
    /// -------------------------------------------------------------------------------------------------
    /// <summary>
    ///     Identifies the type of event that has caused the log.
    /// </summary>
    /// -------------------------------------------------------------------------------------------------
    public enum LogType : byte
    {
        /// <summary>Debug Trace.</summary>
        Debug = 0,

        /// <summary>Informational message.</summary>
        Info = 1,

        /// <summary>Fatal error or application crash.</summary>
        Fatal = 2,

        /// <summary>Recoverable error.</summary>
        Error = 4,

        /// <summary>Noncritical problem.</summary>
        Warn = 3
    }
}