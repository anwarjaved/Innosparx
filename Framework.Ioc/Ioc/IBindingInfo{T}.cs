namespace Framework.Ioc
{
    using System;

    /// <summary>
    /// Used to define the binding info.
    /// </summary>
    /// <typeparam name="T">The service being bound.</typeparam>
    public interface IBindingInfo<in T> : IBindingInfo
    {
        /// <summary>
        /// Indicates that the service should be bound to the specified implementation type.
        /// </summary>
        /// <typeparam name="TImplementation">The implementation type.</typeparam>
        /// <returns><see cref="IBindingInfo{T}"/> object.</returns>
        IBindingInfo<T> To<TImplementation>() where TImplementation : T;

        /// <summary>
        /// Indicates that the service should be bound to the specified implementation type.
        /// </summary>
        /// <returns><see cref="IBindingInfo{T}"/> object.</returns>
        IBindingInfo<T> ToSelf();

        /// <summary>
        /// Indicates that the service should be bound to the specified method.
        /// </summary>
        /// <param name="method">The method to bound.</param>
        /// <returns><see cref="IBindingInfo{T}"/> object.</returns>
        /// <author>Anwar</author>
        /// <date>11/10/2011</date>
        IBindingInfo<T> ToMethod(Func<T> method);

        /// <summary>
        /// Indicates that instances activated via the binding should not be re-used.
        /// </summary>
        /// <returns><see cref="IBindingInfo{T}"/> object.</returns>
        IBindingInfo<T> InTransientScope();

        /// <summary>
        /// Indicates that only a single instance of the binding should be created, and then
        /// should be re-used for all subsequent requests.
        /// </summary>
        /// <returns><see cref="IBindingInfo{T}"/> object.</returns>
        IBindingInfo<T> InSingletonScope();

        /// <summary>
        /// Indicates that instances activated via the binding should be re-used within the same thread.
        /// </summary>
        /// <returns><see cref="IBindingInfo{T}"/> object.</returns>
        IBindingInfo<T> InThreadScope();

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        /// Indicates that instances activated via the binding should be re-used within the same HTTP
        /// request.
        /// </summary>
        /// <returns>
        /// Binding Info as<see cref="BindingInfo{T}"/>.
        /// </returns>
        /// -------------------------------------------------------------------------------------------------
        IBindingInfo<T> InRequestScope();

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        /// Indicates that instances activated via the binding should be re-used within the same HTTP
        /// Session.
        /// </summary>
        /// <returns>
        /// Binding Info as<see cref="BindingInfo{T}"/>.
        /// </returns>
        /// -------------------------------------------------------------------------------------------------
        IBindingInfo<T> InSessionScope();
    }
}
