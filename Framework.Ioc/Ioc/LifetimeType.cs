namespace Framework.Ioc
{
    /// <summary>
    /// Specifies Inject Lifetime.
    /// </summary>
    public enum LifetimeType
    {
        /// <summary>
        /// Use AlwaysNew Lifetime.
        /// </summary>
        Transient,

        /// <summary>
        /// Use Container Lifetime.
        /// </summary>
        Singleton,

        /// <summary>
        /// Use Request Lifetime.
        /// </summary>
        Request,

        /// <summary>
        /// Use Session Lifetime.
        /// </summary>
        Session,

        /// <summary>
        /// Use Thread Lifetime.
        /// </summary>
        Thread,
    }
}
