using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework
{
    ///-------------------------------------------------------------------------------------------------
    /// <summary>
    ///     Pair.
    /// </summary>
    ///
    /// <typeparam name="TKey">
    ///     Type of the key.
    /// </typeparam>
    /// <typeparam name="TValue">
    ///     Type of the value.
    /// </typeparam>
    ///-------------------------------------------------------------------------------------------------
    public struct Pair<TKey, TValue> 
    {
        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Initializes a new instance of the Pair class.
        /// </summary>
        ///
        /// <param name="key">
        ///     The key.
        /// </param>
        /// <param name="value">
        ///     The value.
        /// </param>
        ///-------------------------------------------------------------------------------------------------
        public Pair(TKey key, TValue value)
            : this()
        {
            this.Key = key;
            this.Value = value;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Gets the key.
        /// </summary>
        ///
        /// <value>
        ///     The key.
        /// </value>
        ///-------------------------------------------------------------------------------------------------
        public TKey Key { get; private set; }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Gets the value.
        /// </summary>
        ///
        /// <value>
        ///     The value.
        /// </value>
        ///-------------------------------------------------------------------------------------------------
        public TValue Value { get; private set; }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Returns the fully qualified type name of this instance.
        /// </summary>
        ///
        /// <returns>
        ///     A <see cref="T:System.String" /> containing a fully qualified type name.
        /// </returns>
        ///-------------------------------------------------------------------------------------------------
        public override string ToString()
        {
            return this.Key.ToString();
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Returns the hash code for this instance.
        /// </summary>
        ///
        /// <returns>
        ///     A 32-bit signed integer that is the hash code for this instance.
        /// </returns>
        ///-------------------------------------------------------------------------------------------------
        public override int GetHashCode()
        {
            return this.Key.GetHashCode();
        }
    }
}
