namespace Framework
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Lambda Comparer.
    /// </summary>
    /// <typeparam name="T">Type to compare.</typeparam>
    internal class LambdaEqualityComparer<T> : IEqualityComparer<T>
    {
        private readonly Func<T, T, bool> lambdaComparer;
        private readonly Func<T, int> lambdaHash;

        internal LambdaEqualityComparer(Func<T, T, bool> lambdaComparer) :
            this(lambdaComparer, o => 0)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LambdaEqualityComparer{T}"/> class.
        /// </summary>
        /// <param name="lambdaComparer">The lambda comparer.</param>
        /// <param name="lambdaHash">The lambda hash.</param>
        internal LambdaEqualityComparer(Func<T, T, bool> lambdaComparer, Func<T, int> lambdaHash)
        {
            if (lambdaComparer == null)
            {
                throw new ArgumentNullException("lambdaComparer");
            }

            if (lambdaHash == null)
            {
                throw new ArgumentNullException("lambdaHash");
            }

            this.lambdaComparer = lambdaComparer;
            this.lambdaHash = lambdaHash;
        }

        /// <summary>
        /// Equalses the specified x.
        /// </summary>
        /// <param name="x">The first object x.</param>
        /// <param name="y">The seceond y.</param>
        /// <returns>Returns Whether x equals y.</returns>
        public bool Equals(T x, T y)
        {
            return this.lambdaComparer(x, y);
        }

        /// <summary>
        /// Returns a hash code for this instance.
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <returns>
        /// A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table. 
        /// </returns>
        public int GetHashCode(T obj)
        {
            return this.lambdaHash(obj);
        }
    }
}
