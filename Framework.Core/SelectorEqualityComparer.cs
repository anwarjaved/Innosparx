namespace Framework
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// Selector Equality Comparer.
    /// </summary>
    /// <typeparam name="TSource">The type of the source.</typeparam>
    /// <typeparam name="TSelection">The type of the selection.</typeparam>
    internal class SelectorEqualityComparer<TSource, TSelection> : IEqualityComparer<TSource>
    {
        private readonly Func<TSource, TSelection> selector;

        internal SelectorEqualityComparer(Func<TSource, TSelection> selector)
        {
            this.selector = selector;
        }

        /// <summary>
        /// Check Equality on the specified objects.
        /// </summary>
        /// <param name="x">The first object x.</param>
        /// <param name="y">The seceond y.</param>
        /// <returns>Returns Whether x equals y.</returns>
        public bool Equals(TSource x, TSource y)
        {
            return object.Equals(this.selector(x), this.selector(y));
        }

        /// <summary>
        /// Returns a hash code for this instance.
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <returns>
        /// A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table. 
        /// </returns>
        public int GetHashCode(TSource obj)
        {
            return this.selector(obj).GetHashCode();
        }
    }
}