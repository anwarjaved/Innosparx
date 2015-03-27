namespace Framework.Rest
{
    /// <summary>
    /// Status for responses.
    /// </summary>
    public enum ResponseStatus
    {
        /// <summary>
        /// No Status.
        /// </summary>
        None,

        /// <summary>
        /// Completed State.
        /// </summary>
        Completed,

        /// <summary>
        /// Error State.
        /// </summary>
        Error,

        /// <summary>
        /// Timed Out.
        /// </summary>
        TimedOut
    }
}
