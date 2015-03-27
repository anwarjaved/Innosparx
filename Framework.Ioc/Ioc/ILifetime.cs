namespace Framework.Ioc
{
    /// <summary>
    ///  Manages LifeTime of DI object.
    /// </summary>
    public interface ILifetime
    {
        /// <summary>
        /// Gets the instance.
        /// </summary>
        /// <param name="dependencyInfo">The binding info.</param>
        /// <returns>Instance of Dependency Object.</returns>
        object GetInstance(IBindingInfo dependencyInfo);

        /// <summary>
        /// Dispose the instance if exists.
        /// </summary>
        /// <param name="dependencyInfo">The binding info.</param>
        void ReleaseInstance(IBindingInfo dependencyInfo);
    }
}
