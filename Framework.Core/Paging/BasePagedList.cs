namespace Framework.Paging
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Represents a subset of a collection of objects that can be individually accessed by index and containing metadata about the superset collection of objects this subset was created from.
    /// </summary>
    /// <typeparam name="TSource">The type of object the collection should contain.</typeparam>
    /// <seealso cref="IPagedList{TSource}"/>
    /// <seealso cref="List{TSource}"/>
    public abstract class BasePagedList<TSource> : List<TSource>, IPagedList<TSource>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BasePagedList&lt;TSource&gt;"/> class.
        /// </summary>
        /// <param name="index">The index of the subset of objects contained by this instance.</param>
        /// <param name="pageSize">The maximum size of any individual subset.</param>
        /// <param name="totalItemCount">The size of the superset.</param>
        protected internal BasePagedList(int index, int pageSize, int totalItemCount)
        {
            // set source to blank list if superset is null to prevent exceptions
            this.TotalCount = totalItemCount;
            this.PageSize = pageSize;
            this.PageIndex = index;
            if (this.TotalCount > 0)
            {
                this.PageCount = (int)Math.Ceiling(this.TotalCount / (double)this.PageSize);
            }
            else
            {
                this.PageCount = 0;
            }

            if (index < 0)
            {
                throw new ArgumentOutOfRangeException("index", index, "PageIndex cannot be below 0.");
            }

            if (pageSize < 1)
            {
                throw new ArgumentOutOfRangeException("pageSize", pageSize, "PageSize cannot be less than 1.");
            }
        }

        /// <summary>
        /// Gets or sets the page count.
        /// </summary>
        /// <value>Total number of subsets within the superset.</value>
        public int PageCount { get; protected set; }

        /// <summary>
        /// Gets or sets the total item count.
        /// </summary>
        /// <value>Total number of objects contained within the superset.</value>
        public int TotalCount { get; protected set; }

        /// <summary>
        /// Gets or sets the index of the page.
        /// </summary>
        /// <value>Zero-based index of this subset within the superset.</value>
        public int PageIndex { get; protected set; }

        /// <summary>
        /// Gets the page number.
        /// </summary>
        /// <value>One-based index of this subset within the superset.</value>
        public int PageNumber
        {
            get { return this.PageIndex + 1; }
        }

        /// <summary>
        /// Gets or sets the size of the page.
        /// </summary>
        /// <value>Maximum size any individual subset.</value>
        public int PageSize { get; protected set; }

        /// <summary>
        /// Gets a value indicating whether this instance has previous page.
        /// </summary>
        /// <value>
        /// Returns true if this is NOT the first subset within the superset.
        /// </value>
        public bool HasPreviousPage
        {
            get { return this.PageIndex > 0; }
        }

        /// <summary>
        /// Gets a value indicating whether this instance has next page.
        /// </summary>
        /// <value>Returns true if this is NOT the last subset within the superset.</value>
        public bool HasNextPage
        {
            get { return this.PageIndex < (this.PageCount - 1); }
        }

        /// <summary>
        /// Gets a value indicating whether this instance is first page.
        /// </summary>
        /// <value>Returns true if this is the first subset within the superset.</value>
        public bool IsFirstPage
        {
            get { return this.PageIndex <= 0; }
        }

        /// <summary>
        /// Gets a value indicating whether this instance is last page.
        /// </summary>
        /// <value>Returns true if this is the last subset within the superset.</value>
        public bool IsLastPage
        {
            get { return this.PageIndex >= (this.PageCount - 1); }
        }
    }
}
