namespace Framework.Threading
{
    using System;

    public interface IBackgroundProcessor : IDisposable
    {
        string Name { get; }

        /// <summary>
        /// Starts this <see cref="BackgroundProcessor"/>.
        /// </summary>
        void Start();

        /// <summary>
        /// Stops this <see cref="BackgroundProcessor"/>.
        /// </summary>
        void Stop();
    }
}