namespace Framework.Collections
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics;

    /// <summary>
    /// Enumerator class for Indexed Dictionary.
    /// </summary>
    /// <typeparam name="TKey">The type of the keys in the dictionary.</typeparam>
    /// <typeparam name="TValue">The type of the values in the dictionary.</typeparam>
    public sealed class OrderedDictionaryEnumerator<TKey, TValue> : IEnumerator<KeyValuePair<TKey, TValue>>
    {
        private bool disposed;
        private int index;

        private List<KeyValuePair<TKey, TValue>> list;

        internal OrderedDictionaryEnumerator(List<KeyValuePair<TKey, TValue>> list)
        {
            this.index = -1;
            this.list = list;
        }

        /// <summary>
        /// Finalizes an instance of the <see cref="OrderedDictionaryEnumerator{TKey,TValue}"/> class.
        /// </summary>
        ~OrderedDictionaryEnumerator()
        {
            this.Dispose(false);
        }

        /// <summary>
        /// Gets the key in the KeyValue Pair.
        /// </summary>
        /// <value>The key in the KeyValue Pair.</value>
        public TKey Key
        {
            get { return this.InternalCurrent.Key; }
        }

        /// <summary>
        /// Gets the value in the KeyValue Pair.
        /// </summary>
        /// <value>The value in the KeyValue Pair.</value>
        public TValue Value
        {
            get { return this.InternalCurrent.Value; }
        }

        /// <summary>
        /// Gets the element in the collection at the current position of the enumerator.
        /// </summary>
        /// <value>The element in the collection at the current position of the enumerator.</value>
        object IEnumerator.Current
        {
            get { return this.InternalCurrent; } // get
        }

        /// <summary>
        /// Gets the element in the collection at the current position of the enumerator.
        /// </summary>
        /// <value>The element in the collection at the current position of the enumerator.</value>
        public KeyValuePair<TKey, TValue> Current
        {
            get { return this.InternalCurrent; }
        }

        /// <summary>
        /// Gets the internal current.
        /// </summary>
        /// <value>The internal current.</value>
        private KeyValuePair<TKey, TValue> InternalCurrent
        {
            get
            {
                if ((this.index < 0) || (this.index >= this.list.Count))
                {
                    throw new InvalidOperationException();
                }

                return this.list[this.index];
            }
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        [DebuggerNonUserCode]
        public void Dispose()
        {
            this.Dispose(true);
        }

        /// <summary>
        /// Advances the enumerator to the next element of the collection.
        /// </summary>
        /// <returns>
        /// True if the enumerator was successfully advanced to the next element; false if the enumerator has passed the end of the collection.
        /// </returns>
        /// <exception cref="T:System.InvalidOperationException">The collection was modified after the enumerator was created. </exception>
        public bool MoveNext()
        {
            this.index++;
            return this.index < this.list.Count;
        } // MoveNext

        /// <summary>
        /// Sets the enumerator to its initial position, which is before the first element in the collection.
        /// </summary>
        /// <exception cref="T:System.InvalidOperationException">The collection was modified after the enumerator was created. </exception>
        public void Reset()
        {
            this.index = -1;
        }

        /// <summary>
        /// Dispose(<c>bool</c> disposing) executes in two distinct scenarios.
        /// If disposing equals true, the method has been called directly
        /// or indirectly by a user's code. Managed and unmanaged resources
        /// can be disposed.
        /// If disposing equals false, the method has been called by the 
        /// runtime from inside the <c>finalizer</c> and you should not reference 
        /// other objects. Only unmanaged resources can be disposed.
        /// </summary>
        /// <param name="disposing">If set to <see langword="true"/> [disposing].</param>
        private void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    // free managed resources
                    this.list = null;

                    GC.SuppressFinalize(this);
                }

                this.disposed = true;
            }
        }
    }
}