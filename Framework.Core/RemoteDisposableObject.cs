using System;

namespace Framework
{
    /// <summary>
    /// Class that implements <see cref="T:System.IDisposable"/> to provide disposing 
    /// functions to inherited classes also inherit from <see cref="MarshalByRefObject"/> to allow remoting.
    /// </summary>
    public abstract class RemoteDisposableObject : MarshalByRefObject, IDisposable
    {
        /// <summary>
        /// Track whether Dispose has been called.
        /// </summary>
        private bool disposed;

        /// <summary>
        /// Finalizes an instance of the <see cref="RemoteDisposableObject"/> class.
        /// </summary>
        ~RemoteDisposableObject()
        {
            this.Dispose(false);
        }

        /// <summary>
        /// Occurs When the Object is disposed.
        /// </summary>
        public event EventHandler Disposed;

        /// <summary>
        /// Creates a new object that is a copy of the current instance.
        /// </summary>
        /// <returns>
        /// A new object that is a copy of this instance.
        /// </returns>
        public virtual object Clone()
        {
            return MemberwiseClone();
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing,
        /// or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            this.Dispose(true);
        }

        /// <summary>
        /// Override This Method To Dispose Managed Resources.
        /// </summary>
        protected virtual void DisposeManagedResources()
        {
        }

        /// <summary>
        /// Override This Method To Dispose Unmanaged Resources.
        /// </summary>
        protected virtual void DisposeUnmanagedResources()
        {
        }

        /// <summary>
        /// Raises the <see cref="Disposed"/> event.
        /// </summary>
        /// <remarks>
        ///  <note type="inheritinfo">
        ///   When overriding <see ref="M:SmartCore.DisposableObject.OnDisposed"/>
        ///   in a derived class, be sure to call the base class's 
        ///   <see cref="OnDisposed"/> method so that
        ///   registered delegates receive the event.
        ///  </note>
        /// </remarks>
        protected virtual void OnDisposed()
        {
            if (this.Disposed != null)
            {
                this.Disposed(this, EventArgs.Empty);
            }
        }

        /// <summary>
        /// Used Internally To Dispose Object.
        /// </summary>
        /// <param name="disposing">If true, this method is called because the object is being disposed with the Dispose() method. If false, the object is being disposed by the garbage collector.</param>
        /// <exception cref="ObjectDisposingException"><c>Thrown when an error occurs in a disposing object.</c></exception>
        private void Dispose(bool disposing)
        {
            if (this.disposed)
            {
                return;
            }

            // change the state to Disposing
            try
            {
                // If disposing equals true, dispose all managed 
                // and unmanaged resources.
                if (disposing)
                {
                    this.DisposeManagedResources();
                    this.DisposeUnmanagedResources();
                    this.disposed = true;
                    GC.SuppressFinalize(this);
                    this.OnDisposed();
                }
                else
                {
                    this.DisposeUnmanagedResources();
                }
            }
            catch (Exception ex)
            {
                if (disposing)
                {
                    throw new ObjectDisposingException(GetType().Name, ex);
                }
            }
        }
    }
}