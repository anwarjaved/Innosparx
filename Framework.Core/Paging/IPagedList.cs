namespace Framework.Paging
{
    using System.Collections;

    /// <summary>
    /// Paged List.
    /// </summary>
    /// <author>Anwar</author>
    /// <datetime>1/25/2011 3:52 PM</datetime>
    public interface IPagedList : IEnumerable
    {
        /// <summary>
        /// Gets Total number of subsets within the superset.
        /// </summary>
        /// <value>
        /// Total number of subsets within the superset.
        /// </value>
        int PageCount { get; }

        /// <summary>
        /// Gets Total number of objects contained within the superset.
        /// </summary>
        /// <value>
        /// Total number of objects contained within the superset.
        /// </value>
        int TotalCount { get; }

        /// <summary>
        /// Gets Zero-based index of this subset within the superset.
        /// </summary>
        /// <value>
        /// Zero-based index of this subset within the superset.
        /// </value>
        int PageIndex { get; }

        /// <summary>
        /// Gets One-based index of this subset within the superset.
        /// </summary>
        /// <value>
        /// One-based index of this subset within the superset.
        /// </value>
        int PageNumber { get; }

        /// <summary>
        /// Gets Maximum size any individual subset.
        /// </summary>
        /// <value>
        /// Maximum size any individual subset.
        /// </value>
        int PageSize { get; }

        /// <summary>
        /// Gets a value indicating whether this is NOT the first subset within the superset.
        /// </summary>
        /// <value>
        /// Returns true if this is NOT the first subset within the superset.
        /// </value>
        bool HasPreviousPage { get; }

        /// <summary>
        /// Gets a value indicating whether this is NOT the last subset within the superset..
        /// </summary>
        /// <value>Returns true if this is NOT the last subset within the superset.</value>
        bool HasNextPage { get; }

        /// <summary>
        /// Gets a value indicating whether this is the first subset within the superset.
        /// </summary>
        /// <value>Returns true if this is the first subset within the superset.</value>
        bool IsFirstPage { get; }

        /// <summary>
        /// Gets a value indicating whether this is the last subset within the superset.
        /// </summary>
        /// <value>Returns true if this is the last subset within the superset.</value>
        bool IsLastPage { get; }
    }
}
