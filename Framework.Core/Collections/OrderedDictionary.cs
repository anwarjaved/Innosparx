namespace Framework.Collections
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;

    /// <summary>
    /// Represents a Indexed dictionary of keys and values. 
    /// </summary>
    /// <typeparam name="TKey">The type of the keys in the dictionary.</typeparam>
    /// <typeparam name="TValue">The type of the values in the dictionary.</typeparam>
    [DebuggerNonUserCode]
    public sealed class OrderedDictionary<TKey, TValue> : IDictionary<TKey, TValue>,
                                                   ICloneable<OrderedDictionary<TKey, TValue>>
    {
        private readonly Dictionary<TKey, TValue> dictionary;
        private readonly List<KeyValuePair<TKey, TValue>> list;

        /// <summary>
        /// Initializes a new instance of the <see cref="OrderedDictionary{TKey,TValue}"/> class.
        /// </summary>
        /// <summary>Initializes a new instance of the
        /// <see cref="System.Collections.Generic.Dictionary{TKey, TValue}"></see> class that is empty, has the default initial capacity, and uses the default equality comparer for the key type.</summary>
        public OrderedDictionary()
        {
            this.dictionary = new Dictionary<TKey, TValue>();
            this.list = new List<KeyValuePair<TKey, TValue>>();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="OrderedDictionary{TKey,TValue}"/> class.
        /// </summary>
        /// <param name="dictionary">The dictionary.</param>
        public OrderedDictionary(IDictionary<TKey, TValue> dictionary)
        {
            this.dictionary = new Dictionary<TKey, TValue>(dictionary);
            this.list = new List<KeyValuePair<TKey, TValue>>();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="OrderedDictionary{TKey,TValue}"/> class.
        /// </summary>
        /// <param name="comparer">The comparer.</param>
        public OrderedDictionary(IEqualityComparer<TKey> comparer)
        {
            this.dictionary = new Dictionary<TKey, TValue>(comparer);
            this.list = new List<KeyValuePair<TKey, TValue>>();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="OrderedDictionary{TKey,TValue}"/> class.
        /// </summary>
        /// <param name="capacity">The capacity.</param>
        public OrderedDictionary(int capacity)
        {
            this.dictionary = new Dictionary<TKey, TValue>(capacity);
            this.list = new List<KeyValuePair<TKey, TValue>>();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="OrderedDictionary{TKey,TValue}"/> class.
        /// </summary>
        /// <param name="dictionary">The dictionary.</param>
        /// <param name="comparer">The comparer.</param>
        public OrderedDictionary(IDictionary<TKey, TValue> dictionary, IEqualityComparer<TKey> comparer)
        {
            this.dictionary = new Dictionary<TKey, TValue>(dictionary, comparer);
            this.list = new List<KeyValuePair<TKey, TValue>>();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="OrderedDictionary{TKey,TValue}"/> class.
        /// </summary>
        /// <param name="capacity">The capacity.</param>
        /// <param name="comparer">The comparer.</param>
        public OrderedDictionary(int capacity, IEqualityComparer<TKey> comparer)
        {
            this.dictionary = new Dictionary<TKey, TValue>(capacity, comparer);
            this.list = new List<KeyValuePair<TKey, TValue>>();
        }

        /// <summary>
        /// Gets the <see cref="System.Collections.Generic.IEqualityComparer{TKey}"></see> that is used to determine equality of keys for the dictionary.
        /// </summary>
        /// <value>The <see cref="System.Collections.Generic.IEqualityComparer{TKey}"></see> generic interface implementation that is used to determine equality of keys for the current <see cref="T:System.Collections.Generic.Dictionary`2"></see> and to provide hash values for the keys.</value>
        public IEqualityComparer<TKey> Comparer
        {
            get { return this.dictionary.Comparer; }
        }

        /// <summary>
        /// Gets an collection containing the values of the <see cref="OrderedDictionary{TKey,TValue}"></see>.
        /// </summary>
        /// <value>An collection containing the values of the object that implements <see cref="OrderedDictionary{TKey,TValue}"></see>.</value>
        public ICollection<TKey> Keys
        {
            get
            {
                return this.list.Select(keyValue => keyValue.Key).ToList();
            }
        }

        /// <summary>
        /// Gets an collection containing the values in the <see cref="OrderedDictionary{TKey,TValue}"></see>.
        /// </summary>
        /// <value>An collection containing the values in the object that implements <see cref="OrderedDictionary{TKey,TValue}"></see>.</value>
        public ICollection<TValue> Values
        {
            get
            {
                return this.list.Select(keyValue => keyValue.Value).ToList();
            }
        }

        /// <summary>
        /// Gets the number of elements contained in the collection.
        /// </summary>
        /// <value>The number of elements contained in the collection.</value>
        public int Count
        {
            get { return this.list.Count; }
        }

        /// <summary>
        /// Gets a value indicating whether the collection is read-only.
        /// </summary>
        /// <value>True if the collection is read-only; otherwise, false.</value>
        bool ICollection<KeyValuePair<TKey, TValue>>.IsReadOnly
        {
            get { return false; }
        }

        /// <summary>
        /// Gets or sets the value associated with the specified key.
        /// </summary>
        /// <param name="key">The key of the value to get.</param>
        /// <value>The value associated with the specified key.</value>
        /// <returns>The TValue instance object.</returns>
        public TValue this[TKey key]
        {
            get
            {
                return this.dictionary.ContainsKey(key) ? this.dictionary[key] : default(TValue);
            }

            set
            {
                if (this.dictionary.ContainsKey(key))
                {
                    this.dictionary[key] = value;
                    this.list[this.IndexOf(key)] = new KeyValuePair<TKey, TValue>(key, value);
                }
                else
                {
                    this.Add(key, value);
                }
            }
        }

        /// <summary>Gets or sets the value associated on the specified index.</summary>
        /// <returns>The value associated on the specified index. If the specified index is invalid, 
        /// a get or set operation throws a <see cref="ArgumentOutOfRangeException"></see>
        /// </returns>
        /// <param name="index">The index at which the value is get or set.</param>
        /// <exception cref="ArgumentOutOfRangeException">index is greater than or less than zero</exception>
        public TValue this[int index]
        {
            get
            {
                if ((index < 0) || (index >= this.Count))
                {
                    throw new ArgumentOutOfRangeException("index");
                }

                return this.list[index].Value;
            }

            set
            {
                if ((index < 0) || (index >= this.Count))
                {
                    throw new ArgumentOutOfRangeException("index");
                }

                TKey key = this.list[index].Key;

                if (this.dictionary.ContainsKey(key))
                {
                    this.dictionary[key] = value;
                    this.list[index] = new KeyValuePair<TKey, TValue>(key, value);
                }
                else
                {
                    this.Add(key, value);
                }
            }
        }

        /// <summary>
        /// Adds an element with the provided key and value to the 
        /// <see cref="OrderedDictionary{TKey,TValue}"></see>.
        /// </summary>
        /// <param name="key">The object to use as the key of the element to add.</param>
        /// <param name="value">The object to use as the value of the element to add.</param>
        /// <exception cref="System.NotSupportedException">The 
        /// <see cref="OrderedDictionary{TKey,TValue}"></see> 
        /// is read-only.</exception>
        /// <exception cref="System.ArgumentException">An element with the 
        /// same key already exists in the 
        /// <see cref="OrderedDictionary{TKey,TValue}"></see>.</exception>
        /// <exception cref="System.ArgumentNullException">key is null.</exception>
        public void Add(TKey key, TValue value)
        {
            this.dictionary.Add(key, value);
            this.list.Add(new KeyValuePair<TKey, TValue>(key, value));
        }

        /// <summary>
        /// Determines whether the <see cref="OrderedDictionary{TKey,TValue}"></see> ContainsKey an element with the specified key.
        /// </summary>
        /// <param name="key">The key to locate in the <see cref="OrderedDictionary{TKey,TValue}"></see>.</param>
        /// <returns>
        /// True if the <see cref="OrderedDictionary{TKey,TValue}"></see> ContainsKey an element with the key; otherwise, false.
        /// </returns>
        /// <exception cref="System.ArgumentNullException">key is null.</exception>
        public bool ContainsKey(TKey key)
        {
            return this.dictionary.ContainsKey(key);
        }

        /// <summary>
        /// Removes the element with the specified key from the <see cref="OrderedDictionary{TKey,TValue}"></see>.
        /// </summary>
        /// <param name="key">The key of the element to remove.</param>
        /// <returns>
        /// True if the element is successfully removed; otherwise, false.  This method also returns false if key was not found in the original <see cref="OrderedDictionary{TKey,TValue}"></see>.
        /// </returns>
        /// <exception cref="System.NotSupportedException">The <see cref="OrderedDictionary{TKey,TValue}"></see> is read-only.</exception>
        /// <exception cref="System.ArgumentNullException">key is null.</exception>
        public bool Remove(TKey key)
        {
            bool flag = this.dictionary.Remove(key);
            this.list.RemoveAt(this.IndexOf(key));
            return flag;
        }

        /// <summary>
        /// Get the value associated with the specified key.
        /// </summary>
        /// <param name="key">The key of the value to get.</param>
        /// <param name="value">The value associated with key.</param>
        /// <returns>True If value exists with specified key else false.</returns>
        public bool TryGetValue(TKey key, out TValue value)
        {
            return this.dictionary.TryGetValue(key, out value);
        }

        /// <summary>
        /// Adds the specified key value pair.
        /// </summary>
        /// <param name="keyValuePair">The key value pair.</param>
        void ICollection<KeyValuePair<TKey, TValue>>.Add(KeyValuePair<TKey, TValue> keyValuePair)
        {
            this.Add(keyValuePair.Key, keyValuePair.Value);
        }

        /// <summary>
        /// Removes all items from the <see cref="T:System.Collections.Generic.ICollection`1"/>.
        /// </summary>
        /// <exception cref="T:System.NotSupportedException">
        /// The <see cref="T:System.Collections.Generic.ICollection`1"/> is read-only.
        /// </exception>
        public void Clear()
        {
            this.dictionary.Clear();
            this.list.Clear();
        }

        /// <summary>
        /// Determines whether the collection <see cref="ContainsKey"/> a specific value.
        /// </summary>
        /// <param name="item">The object to locate in the collection.</param>
        /// <returns>
        /// True if item is found in the collection; otherwise, false.
        /// </returns>
        bool ICollection<KeyValuePair<TKey, TValue>>.Contains(KeyValuePair<TKey, TValue> item)
        {
            if (this.ContainsKey(item.Key))
            {
                TValue keyValue = this[item.Key];

                if (keyValue.Equals(item.Value))
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Copies the elements of the collection to an <see cref="System.Array"></see>, starting at a particular <see cref="System.Array"></see> index.
        /// </summary>
        /// <param name="array">The one-dimensional <see cref="System.Array"></see> that is the destination of the elements copied from collection. The <see cref="System.Array"></see> must have zero-based indexing.</param>
        /// <param name="arrayIndex">The zero-based index in array at which copying begins.</param>
        /// <exception cref="System.ArgumentOutOfRangeException"><paramref name="arrayIndex"/> is less than 0.</exception>
        /// <exception cref="System.ArgumentNullException">array is null.</exception>
        /// <exception cref="System.ArgumentException">array is multidimensional.-or-<paramref name="arrayIndex"/> is equal to or greater than the length of array.-or-The number of elements in the source collection is greater than the available space from <paramref name="arrayIndex"/> to the end of the destination array.-or-Type <typeparamref name="TKey"/> cannot be cast automatically to the type of the destination array.</exception>
        public void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
        {
            ((ICollection<KeyValuePair<TKey, TValue>>)this.dictionary).CopyTo(array, arrayIndex);
        }

        /// <summary>
        /// Removes the first occurrence of a specific object from the collection.
        /// </summary>
        /// <param name="item">The object to remove from the collection.</param>
        /// <returns>
        /// True if item was successfully removed from the collection; otherwise, false. This method also returns false if item is not found in the original collection.
        /// </returns>
        /// <exception cref="System.NotSupportedException">The collection is read-only.</exception>
        bool ICollection<KeyValuePair<TKey, TValue>>.Remove(KeyValuePair<TKey, TValue> item)
        {
            if (this.ContainsKey(item.Key))
            {
                TValue keyValue = this[item.Key];

                if (keyValue.Equals(item.Value))
                {
                    return this.dictionary.Remove(item.Key);
                }
            }

            return false;
        }

        /// <summary>
        /// Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.Collections.Generic.IEnumerator`1"></see> that can be used to iterate through the collection.
        /// </returns>
        IEnumerator<KeyValuePair<TKey, TValue>>
            IEnumerable<KeyValuePair<TKey, TValue>>.GetEnumerator()
        {
            return new OrderedDictionaryEnumerator<TKey, TValue>(this.list);
        }

        /// <summary>
        /// Returns an enumerator that iterates through a collection.
        /// </summary>
        /// <returns>
        /// An <see cref="System.Collections.IEnumerator"></see> object that can be used to iterate through the collection.
        /// </returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return new OrderedDictionaryEnumerator<TKey, TValue>(this.list);
        }

        /// <summary>
        /// Determines whether the <see cref="System.Collections.Generic.Dictionary{TKey, TValue}"></see> <see cref="ContainsKey"/> a specific value.
        /// </summary>
        /// <param name="value">The value to locate in the <see cref="System.Collections.Generic.Dictionary{TKey, TValue}"></see>. The value can be null for reference types.</param>
        /// <returns>
        /// True if the <see cref="System.Collections.Generic.Dictionary{TKey, TValue}"></see> <see cref="ContainsKey"/> an element with the specified value; otherwise, false.
        /// </returns>
        public bool ContainsValue(TValue value)
        {
            return this.dictionary.ContainsValue(value);
        }

        /// <summary>
        /// Inserts an item to the <see cref="OrderedDictionary{TKey,TValue}"></see> at the specified index.
        /// </summary>
        /// <param name="index">The zero-based index at which item should be inserted.</param>
        /// <param name="key">The object to use as the key of the element to insert.</param>
        /// <param name="value">The object to use as the value of the element to insert.</param>
        /// <exception cref="T:System.NotSupportedException">The <see cref="OrderedDictionary{TKey,TValue}"></see> is read-only.</exception>
        /// <exception cref="T:System.ArgumentOutOfRangeException">index is not a valid index in the <see cref="OrderedDictionary{TKey,TValue}"></see>.</exception>
        public void Insert(int index, TKey key, TValue value)
        {
            if ((index < 0) || (index >= this.Count))
            {
                throw new ArgumentOutOfRangeException("index");
            }

            this.dictionary.Add(key, value);
            this.list.Insert(index, new KeyValuePair<TKey, TValue>(key, value));
        }

        /// <summary>
        /// Determines whether the specified key exist in <see cref="OrderedDictionary{TKey,TValue}"/>.
        /// </summary>
        /// <param name="key">The key associated with value to get.</param>
        /// <returns>
        /// <c>True</c> If the specified key <see cref="ContainsKey"/> key; otherwise, <c>false</c>.
        /// </returns>
        public bool Contains(TKey key)
        {
            return this.ContainsKey(key);
        }

        /// <summary>
        /// Determines the index of a specific item in the <see cref="OrderedDictionary{TKey,TValue}"></see>.
        /// </summary>
        /// <param name="value">The object to locate in the <see cref="OrderedDictionary{TKey,TValue}"></see>.</param>
        /// <returns>
        /// The index of item if found in the list; otherwise, -1.
        /// </returns>
        public int IndexOf(TValue value)
        {
            for (int index = 0; index < this.list.Count; index++)
            {
                KeyValuePair<TKey, TValue> keyValue = this.list[index];
                if (keyValue.Value.Equals(value))
                {
                    return index;
                }
            }

            return -1;
        }

        /// <summary>
        /// Removes the <see cref="OrderedDictionary{TKey,TValue}"></see> item at the specified index.
        /// </summary>
        /// <param name="index">The zero-based index of the item to remove.</param>
        /// <exception cref="T:System.NotSupportedException">The <see cref="OrderedDictionary{TKey,TValue}"></see> is read-only.</exception>
        /// <exception cref="T:System.ArgumentOutOfRangeException">index is not a valid index in the <see cref="OrderedDictionary{TKey,TValue}"></see>.</exception>
        public void RemoveAt(int index)
        {
            if ((index < 0) || (index >= this.Count))
            {
                throw new ArgumentOutOfRangeException("index");
            }

            KeyValuePair<TKey, TValue> keyValue = this.list[index];
            this.dictionary.Remove(keyValue.Key);
            this.list.RemoveAt(index);
        }

        /// <summary>
        /// Gets the enumerator.
        /// </summary>
        /// <returns>The <see cref="OrderedDictionaryEnumerator{TKey,TValue}"/> object to enumerate values.</returns>
        public OrderedDictionaryEnumerator<TKey, TValue> GetEnumerator()
        {
            return new OrderedDictionaryEnumerator<TKey, TValue>(this.list);
        }

        /// <summary>
        /// Creates a new object that is a copy of the current instance.
        /// </summary>
        /// <returns>
        /// A new object that is a copy of this instance.
        /// </returns>
        OrderedDictionary<TKey, TValue> ICloneable<OrderedDictionary<TKey, TValue>>.Clone()
        {
            return new OrderedDictionary<TKey, TValue>(this);
        }

        /// <summary>
        /// Determines the index of a specific item in the <see cref="OrderedDictionary{TKey,TValue}"></see> by key.
        /// </summary>
        /// <param name="key">The object key to locate in the <see cref="OrderedDictionary{TKey,TValue}"></see>.</param>
        /// <returns>
        /// The index of item if found in the list; otherwise, -1.
        /// </returns>
        private int IndexOf(TKey key)
        {
            for (int index = 0; index < this.list.Count; index++)
            {
                KeyValuePair<TKey, TValue> keyValue = this.list[index];
                if (keyValue.Key.Equals(key))
                {
                    return index;
                }
            }

            return -1;
        }
    }
}