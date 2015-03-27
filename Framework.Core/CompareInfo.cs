namespace Framework
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    /// -------------------------------------------------------------------------------------------------
    /// <summary>
    /// Information about the compare.
    /// </summary>
    /// <typeparam name="T">
    /// Generic type parameter.
    /// </typeparam>
    /// -------------------------------------------------------------------------------------------------
    public class CompareInfo<T>
    {
        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        /// Initializes a new instance of the CompareInfo class.
        /// </summary>
        /// <param name="added">
        /// The added.
        /// </param>
        /// <param name="modified">
        /// The modified.
        /// </param>
        /// <param name="deleted">
        /// The deleted.
        /// </param>
        /// -------------------------------------------------------------------------------------------------
        public CompareInfo(IReadOnlyList<T> added, IReadOnlyList<T> modified, IReadOnlyList<T> deleted)
        {
            this.Added = added;
            this.Modified = modified;
            this.Deleted = deleted;
        }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        /// Gets the added.
        /// </summary>
        /// <value>
        /// The added.
        /// </value>
        /// -------------------------------------------------------------------------------------------------
        public IReadOnlyList<T> Added { get; private set; }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        /// Gets the modified.
        /// </summary>
        /// <value>
        /// The modified.
        /// </value>
        /// -------------------------------------------------------------------------------------------------
        public IReadOnlyList<T> Modified { get; private set; }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        /// Gets the deleted.
        /// </summary>
        /// <value>
        /// The deleted.
        /// </value>
        /// -------------------------------------------------------------------------------------------------
        public IReadOnlyList<T> Deleted { get; private set; }
    }
}
