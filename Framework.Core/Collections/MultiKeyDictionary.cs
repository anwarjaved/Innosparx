namespace Framework.Collections
{
    using System;
    using System.Collections;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;

    /// <summary>
    /// Dictionary of multi keys.
    /// </summary>
    /// <typeparam name="TKey">Type of the key.</typeparam>
    /// <typeparam name="TValue">Type of the value.</typeparam>
    public class MultiKeyDictionary<TKey, TValue> : IEnumerable<KeyValuePair<TKey, TValue>>
    {
        private readonly ConcurrentDictionary<TKey, int> keyItems;
        private readonly List<TValue> valueItems;

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Initializes a new instance of the MultiKeyDictionary class.
        /// </summary>
        /// -------------------------------------------------------------------------------------------------
        public MultiKeyDictionary()
            : this(EqualityComparer<TKey>.Default)
        {
        }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        /// Initializes a new instance of the MultiKeyDictionary class.
        /// </summary>
        /// <param name="comparer">
        /// The comparer.
        /// </param>
        /// -------------------------------------------------------------------------------------------------
        public MultiKeyDictionary(IEqualityComparer<TKey> comparer)
        {
            this.keyItems = new ConcurrentDictionary<TKey, int>(comparer);
            this.valueItems = new List<TValue>();
        }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        /// Gets the keys.
        /// </summary>
        /// <value>
        /// The keys.
        /// </value>
        /// -------------------------------------------------------------------------------------------------
        public ICollection<TKey> Keys
        {
            get { return this.keyItems.Keys; }
        }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        /// Gets the values.
        /// </summary>
        /// <value>
        /// The values.
        /// </value>
        /// -------------------------------------------------------------------------------------------------
        public ICollection<TValue> Values
        {
            get { return this.valueItems; }
        }

        /// <summary>
        /// Gets the value stored under the specified key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns>A TValue instance.</returns>
        /// <exception cref="System.ArgumentNullException">key;@Cannot be null</exception>
        public TValue this[TKey key]
        {
            get
            {
                if (EqualityComparer<TKey>.Default.Equals(key, default(TKey)))
                {
                    throw new ArgumentNullException("key", @"Cannot be null");
                }

                if (this.keyItems.ContainsKey(key))
                {
                    int index = this.keyItems[key];
                    return this.valueItems[index];
                }

                return default(TValue);
            }
        }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        /// Adds key.
        /// </summary>
        /// <param name="key">
        /// The key.
        /// </param>
        /// <param name="value">
        /// The value.
        /// </param>
        /// -------------------------------------------------------------------------------------------------
        public void Add(TKey key, TValue value)
        {
            this.Add(new Collection<TKey>() { key }, value);
        }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        /// Adds key.
        /// </summary>
        /// <exception cref="ArgumentNullException">
        /// Thrown when one or more required arguments are null.
        /// </exception>
        /// <param name="keys">
        /// The keys.
        /// </param>
        /// <param name="value">
        /// The value.
        /// </param>
        /// -------------------------------------------------------------------------------------------------
        public void Add(IEnumerable<TKey> keys, TValue value)
        {
            if (EqualityComparer<TValue>.Default.Equals(value, default(TValue)))
            {
                throw new ArgumentNullException("value", @"Cannot be null");
            }

            int index = -1;
            foreach (var key in keys)
            {
                if (EqualityComparer<TKey>.Default.Equals(key, default(TKey)))
                {
                    throw new ArgumentNullException("key", @"Cannot be null");
                }

                if (index == -1)
                {
                    index = this.valueItems.IndexOf(value);

                    if (index == -1)
                    {
                        this.valueItems.Add(value);
                        index = this.valueItems.Count - 1;
                    }
                }

                if (this.keyItems.ContainsKey(key))
                {
                    this.keyItems[key] = index;
                }
                else
                {
                    this.keyItems.GetOrAdd(key, index);
                }
            }
        }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        /// Removes the given key.
        /// </summary>
        /// <exception cref="ArgumentNullException">
        /// Thrown when one or more required arguments are null.
        /// </exception>
        /// <param name="key">
        /// The key.
        /// </param>
        /// <returns>
        /// true if it succeeds, false if it fails.
        /// </returns>
        /// -------------------------------------------------------------------------------------------------
        public bool Remove(TKey key)
        {
            if (this.keyItems.ContainsKey(key))
            {
                int index = this.keyItems[key];
                int outValue;
                this.keyItems.TryRemove(key, out outValue);

                if (!this.keyItems.Values.Contains(index))
                {
                    this.valueItems.RemoveAt(index);
                }

                return true;
            }

            return false;
        }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        /// Removes all described by value.
        /// </summary>
        /// <exception cref="ArgumentNullException">
        /// Thrown when one or more required arguments are null.
        /// </exception>
        /// <param name="value">
        /// The value.
        /// </param>
        /// <returns>
        /// true if it succeeds, false if it fails.
        /// </returns>
        /// -------------------------------------------------------------------------------------------------
        public bool RemoveAll(TValue value)
        {
            if (EqualityComparer<TValue>.Default.Equals(value, default(TValue)))
            {
                throw new ArgumentNullException("value", @"Cannot be null");
            }

            int index = this.valueItems.IndexOf(value);

            if (index >= 0)
            {
                this.valueItems.RemoveAt(index);

                IEnumerable<TKey> keys = this.keyItems.Where(x => x.Value == index).Select(x => x.Key);

                foreach (var key in keys)
                {
                    int outValue;
                    this.keyItems.TryRemove(key, out outValue);
                }

                return true;
            }

            return false;
        }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Clears this object to its blank/initial state.
        /// </summary>
        /// -------------------------------------------------------------------------------------------------
        public void Clear()
        {
            this.keyItems.Clear();
            this.valueItems.Clear();
        }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        /// Query if 'key' contains key.
        /// </summary>
        /// <exception cref="ArgumentNullException">
        /// Thrown when one or more required arguments are null.
        /// </exception>
        /// <param name="key">
        /// The key.
        /// </param>
        /// <returns>
        /// true if it succeeds, false if it fails.
        /// </returns>
        /// -------------------------------------------------------------------------------------------------
        public bool ContainsKey(TKey key)
        {
            return this.keyItems.ContainsKey(key);
        }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        /// Gets the enumerator.
        /// </summary>
        /// <returns>
        /// The enumerator.
        /// </returns>
        /// -------------------------------------------------------------------------------------------------
        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
        {
            return this.keyItems.Select(keyValuePair => new KeyValuePair<TKey, TValue>(keyValuePair.Key, this.valueItems[keyValuePair.Value])).GetEnumerator();
        }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        /// Gets the enumerator.
        /// </summary>
        /// <returns>
        /// The enumerator.
        /// </returns>
        /// -------------------------------------------------------------------------------------------------
        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
    }
}
