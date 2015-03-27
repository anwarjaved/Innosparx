namespace Framework
{
    using System;
    using System.Globalization;

    /// <summary>
    /// HashCode Combiner class to create new hashes from objects.
    /// </summary>
    public sealed class HashCodeUtility
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="HashCodeUtility"/> class.
        /// </summary>
        public HashCodeUtility()
        {
            this.CombinedHash = 5381L;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="HashCodeUtility"/> class.
        /// </summary>
        /// <param name="initialCombinedHash">
        /// The initial combined hash.
        /// </param>
        public HashCodeUtility(long initialCombinedHash)
        {
            this.CombinedHash = initialCombinedHash;
        }

        /// <summary>
        /// Gets the combined hash.
        /// </summary>
        /// <value>
        /// The combined hash.
        /// </value>
        public long CombinedHash
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the combined hash32.
        /// </summary>
        /// <value>
        /// The combined hash32.
        /// </value>
        public int CombinedHash32
        {
            get
            {
                return this.CombinedHash.GetHashCode();
            }
        }

        /// <summary>
        /// Gets the combined hash string.
        /// </summary>
        /// <value>
        /// The combined hash string.
        /// </value>
        public string CombinedHashString
        {
            get
            {
                return this.CombinedHash.ToString("x", CultureInfo.InvariantCulture);
            }
        }

        /// <summary>
        /// Performs an implicit conversion from <see cref="HashCodeUtility"/> to <see cref="System.String"/>.
        /// </summary>
        /// <param name="other">The other.</param>
        /// <returns>The result of the conversion.</returns>
        public static implicit operator string(HashCodeUtility other)
        {
            return other == null ? string.Empty : other.ToString();
        }

        /// <summary>
        /// Adds the array.
        /// </summary>
        /// <param name="array">Array to add.</param>
        public void AddArray(string[] array)
        {
            if (array != null)
            {
                int length = array.Length;
                for (int i = 0; i < length; i++)
                {
                    this.AddObject(array[i]);
                }
            }
        }

        /// <summary>
        /// Adds the case insensitive string.
        /// </summary>
        /// <param name="value">The value.</param>
        public void AddCaseInsensitiveString(string value)
        {
            if (value != null)
            {
                this.AddInt(StringComparer.InvariantCultureIgnoreCase.GetHashCode(value));
            }
        }

        /// <summary>
        /// Adds the date time.
        /// </summary>
        /// <param name="date">The date to add.</param>
        public void AddDateTime(DateTime date)
        {
            this.AddInt(date.GetHashCode());
        }

        /// <summary>
        /// Adds the value.
        /// </summary>
        /// <param name="value">The number to add.</param>
        public void AddInt(int value)
        {
            this.CombinedHash = ((this.CombinedHash << 5) + this.CombinedHash) ^ value;
        }

        /// <summary>
        /// Adds the object.
        /// </summary>
        /// <param name="value">The value to add.</param>
        public void AddObject(bool value)
        {
            this.AddInt(value.GetHashCode());
        }

        /// <summary>
        /// Adds the value.
        /// </summary>
        /// <param name="obj">The value to add.</param>
        public void AddObject(byte obj)
        {
            this.AddInt(obj.GetHashCode());
        }

        /// <summary>
        /// Adds the object.
        /// </summary>
        /// <param name="value">
        /// The number to add.
        /// </param>
        public void AddObject(int value)
        {
            this.AddInt(value);
        }

        /// <summary>
        /// Adds the object.
        /// </summary>
        /// <param name="value">
        /// The number to add.
        /// </param>
        public void AddObject(long value)
        {
            this.AddInt(value.GetHashCode());
        }

        /// <summary>
        /// Adds the object.
        /// </summary>
        /// <param name="value">
        /// The object to add.
        /// </param>
        public void AddObject(object value)
        {
            if (value != null)
            {
                this.AddInt(value.GetHashCode());
            }
        }

        /// <summary>
        /// Adds the object.
        /// </summary>
        /// <param name="value">
        /// The value.
        /// </param>
        public void AddObject(string value)
        {
            if (value != null)
            {
                this.AddInt(value.GetHashCode());
            }
        }

        /// <summary>
        /// Serves as a hash function for a particular type. <see cref="M:System.Object.GetHashCode"></see> is suitable for use in hashing algorithms and data structures like a hash table.
        /// </summary>
        /// <returns>
        /// A hash code for the current <see cref="T:System.Object"></see>.
        /// </returns>
        public override int GetHashCode()
        {
            return this.CombinedHash32;
        }

        /// <summary>
        /// Returns a <see cref="T:System.String"></see> that represents the current <see cref="T:System.Object"></see>.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.String"></see> that represents the current <see cref="T:System.Object"></see>.
        /// </returns>
        public override string ToString()
        {
            return this.CombinedHash32.ToString(CultureInfo.InvariantCulture);
        }
    }
}