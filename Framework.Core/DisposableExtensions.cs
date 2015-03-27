namespace Framework
{
    using System;

    ///-------------------------------------------------------------------------------------------------
    /// <summary>
    ///     Disposable extensions.
    /// </summary>
    ///-------------------------------------------------------------------------------------------------
    public static class DisposableExtensions
    {
        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     An IDisposable extension method that try dispose.
        /// </summary>
        ///
        /// <param name="item">
        ///     The item to act on.
        /// </param>
        ///-------------------------------------------------------------------------------------------------
        public static void TryDispose(this IDisposable item)
        {
            try
            {
                if (item != null)
                {
                    item.Dispose();
                }
            }
            catch (Exception)
            {
            }
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     An IDisposable extension method that try dispose.
        /// </summary>
        ///
        /// <param name="item">
        ///     The item to act on.
        /// </param>
        ///-------------------------------------------------------------------------------------------------
        public static void TryDispose(this object item)
        {
            TryDispose(item as IDisposable);
        }
    }
}
