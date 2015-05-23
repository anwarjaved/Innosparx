namespace Framework.Logging
{
    using System.Security;

    public interface ILogAdapter
    {
        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Writes the given entry.
        /// </summary>
        /// <param name="entry">
        ///     The entry to write.
        /// </param>
        /// -------------------------------------------------------------------------------------------------
        
        void Write(ILogEntry entry);
    }
}