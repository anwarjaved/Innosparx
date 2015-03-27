namespace Framework.Paging
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// Represents a subset of a collection of objects that can be individually accessed by index and containing metadata about the superset collection of objects this subset was created from.
    /// </summary>
    /// <typeparam name="TSource">The type of object the collection should contain.</typeparam>
    /// <seealso cref="IPagedList{TSource}"/>
    /// <seealso cref="BasePagedList{TSource}"/>
    /// <seealso cref="List{TSource}"/>
    public class PagedList<TSource> : BasePagedList<TSource>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PagedList{TSource}"/> class that divides the supplied superset into subsets the size of the supplied pageSize. The instance then only containes the objects contained in the subset specified by index.
        /// </summary>
        /// <param name="superset">The collection of objects to be divided into subsets. If the collection implements <see cref="IQueryable{TSource}"/>, it will be treated as such.</param>
        /// <param name="index">The index of the subset of objects to be contained by this instance.</param>
        /// <param name="pageSize">The maximum size of any individual subset.</param>
        /// <exception cref="ArgumentOutOfRangeException">The specified index cannot be less than zero.</exception>
        /// <exception cref="ArgumentOutOfRangeException">The specified page size cannot be less than one.</exception>
        public PagedList(IEnumerable<TSource> superset, int index = 0, int pageSize = 10)
            : this(superset == null ? new List<TSource>().AsQueryable() : superset.AsQueryable(), index, pageSize)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PagedList{TSource}"/> class that divides the supplied superset into subsets the size of the supplied pageSize. The instance then only containes the objects contained in the subset specified by index.
        /// </summary>
        /// <param name="superset">The collection of objects to be divided into subsets. If the collection implements <see cref="IQueryable{TSource}"/>, it will be treated as such.</param>
        /// <param name="index">The index of the subset of objects to be contained by this instance.</param>
        /// <param name="pageSize">The maximum size of any individual subset.</param>
        /// <exception cref="ArgumentOutOfRangeException">The specified index cannot be less than zero.</exception>
        /// <exception cref="ArgumentOutOfRangeException">The specified page size cannot be less than one.</exception>
        public PagedList(IQueryable<TSource> superset, int index = 0, int pageSize = 10)
            : base(index, pageSize, superset.Count())
        {
            // add items to internal list
            if (this.TotalCount > 0)
            {
                this.AddRange(index == 0
                                ? superset.Take(pageSize).ToList()
                                : superset.Skip(index * pageSize).Take(pageSize).ToList());
            }
        }

        public static IPagedList<TSource> Empty
        {
            get
            {
                return new PagedList<TSource>(Enumerable.Empty<TSource>());
            }
        }
    }
}