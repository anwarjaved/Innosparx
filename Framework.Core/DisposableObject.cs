namespace Framework
{
    using System;

    /// <summary>
    /// The <see cref="DisposableObject"/> maintains a boolean flag that keeps track of 
    /// whether or not the object has been Disposed. It implements 
    /// <see cref="IDisposable"/> and calls a virtual <see cref="DisposeResources"/>
    /// and <see cref="DisposeUnmanagedResources"/> method to let the derived class know when the object has been Disposed.
    /// </summary>
    public abstract class DisposableObject : IDisposable
    {
        /// <summary>
        /// Track whether Dispose has been called.
        /// </summary>
        private bool disposed;

        /// <summary>
        /// Finalizes an instance of the <see cref="DisposableObject"/> class.
        /// </summary>
        ~DisposableObject()
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
            return this.MemberwiseClone();
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
        protected virtual void DisposeResources()
        {
        }

        /// <summary>
        /// Override This Method To Dispose Unmanaged Resources.
        /// </summary>
        protected virtual void DisposeUnmanagedResources()
        {
        }

        /// <summary>
        /// Raises the <see cref="DisposableObject.Disposed"/> event.
        /// </summary>
        /// <remarks>
        ///  <note type="inheritinfo">
        ///   When overriding <see ref="M:SmartCore.DisposableObject.OnDisposed"/>
        ///   in a derived class, be sure to call the base class's 
        ///   <see cref="DisposableObject.OnDisposed"/> method so that
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
                    this.DisposeResources();
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
                    throw new ObjectDisposingException(this.GetType().Name, ex);
                }
            }
        }
    }
}