namespace Framework.Collections
{
    using System.Collections;
    using System.Collections.Concurrent;
    using System.Collections.Generic;

    /// <summary>
    /// A data structure that contains multiple values for a each key.
    /// </summary>
    /// <typeparam name="TKey">The type of key.</typeparam>
    /// <typeparam name="TValue">The type of value.</typeparam>
    public class Multimap<TKey, TValue> : IEnumerable<KeyValuePair<TKey, ICollection<TValue>>>
    {
        private readonly ConcurrentDictionary<TKey, ICollection<TValue>> items;

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        /// Initializes a new instance of the <see cref="Multimap{TKey,TValue}"/> class.
        /// </summary>
        /// <remarks>
        /// LM ANWAR, 6/2/2013.
        /// </remarks>
        /// -------------------------------------------------------------------------------------------------
        public Multimap() : this(EqualityComparer<TKey>.Default)
        {
        }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        /// Initializes a new instance of the <see cref="Multimap{TKey,TValue}"/> class.
        /// </summary>
        /// <remarks>
        /// LM ANWAR, 6/2/2013.
        /// </remarks>
        /// <param name="comparer">
        /// The comparer.
        /// </param>
        /// -------------------------------------------------------------------------------------------------
        public Multimap(IEqualityComparer<TKey> comparer)
        {
            this.items = new ConcurrentDictionary<TKey, ICollection<TValue>>(comparer);
        }

        /// <summary>
        /// Gets the collection of keys.
        /// </summary>
        public ICollection<TKey> Keys
        {
            get { return this.items.Keys; }
        }

        /// <summary>
        /// Gets the collection of collections of values.
        /// </summary>
        public ICollection<ICollection<TValue>> Values
        {
            get { return this.items.Values; }
        }

        /// <summary>
        /// Gets the collection of values stored under the specified key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns>An TValue object instance.</returns>
        public ICollection<TValue> this[TKey key]
        {
            get
            {
                return this.items.ContainsKey(key) ? this.items[key] : null;
            }
        }

        /// <summary>
        /// Adds the specified value for the specified key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        public void Add(TKey key, TValue value)
        {
            this.items.GetOrAdd(key, new List<TValue>()).Add(value);
        }

        /// <summary>
        /// Removes the specified value for the specified key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        public void Remove(TKey key, TValue value)
        {
            if (this.items.ContainsKey(key))
            {
                this.items[key].Remove(value);
            }
        }

        /// <summary>
        /// Removes all values for the specified key.
        /// </summary>
        /// <param name="key">The key.</param>
        public void RemoveAll(TKey key)
        {
            ICollection<TValue> values;
            this.items.TryRemove(key, out values);
        }

        /// <summary>
        /// Removes all values.
        /// </summary>
        public void Clear()
        {
            this.items.Clear();
        }

        /// <summary>
        /// Determines whether the <see cref="Multimap{TKey,TValue}"/> contains any values for the specified key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns><c>True</c> if the <see cref="Multimap{TKey,TValue}"/> has one or more values for the specified key; otherwise, <c>false</c>.</returns>
        public bool ContainsKey(TKey key)
        {
            return this.items.ContainsKey(key);
        }

        /// <summary>
        /// Determines whether the <see cref="Multimap{TKey,TValue}"/> contains the specified value for the specified key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        /// <returns><c>True</c> if the <see cref="Multimap{TKey,TValue}"/> contains such a value; otherwise, <c>false</c>.</returns>
        public bool ContainsValue(TKey key, TValue value)
        {
            return this.items.ContainsKey(key) && this.items[key].Contains(value);
        }

        /// <summary>
        /// Returns an enumerator that iterates through a the <see cref="Multimap{TKey,TValue}"/>.
        /// </summary>
        /// <returns>An <see cref="IEnumerator"/> object that can be used to iterate through the <see cref="Multimap{TKey,TValue}"/>.</returns>
        public IEnumerator GetEnumerator()
        {
            return this.items.GetEnumerator();
        }

        IEnumerator<KeyValuePair<TKey, ICollection<TValue>>> IEnumerable<KeyValuePair<TKey, ICollection<TValue>>>.GetEnumerator()
        {
            return this.items.GetEnumerator();
        }
    }
}