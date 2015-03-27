namespace Framework
{
    using System.Collections.Generic;
    using System.Linq;

    using Framework.Paging;

    /// <summary>
    /// Paged List Extensions.
    /// </summary>
    /// <author>Anwar</author>
    /// <datetime>1/25/2011 3:07 PM</datetime>
    public static class PagingExtensions
    {
        /// <summary>
        /// Creates a subset of this collection of objects that can be individually accessed by index and containing metadata about the collection of objects the subset was created from.
        /// </summary>
        /// <typeparam name="TSource">The type of object the collection should contain.</typeparam>
        /// <param name="superset">The collection of objects to be divided into subsets. If the collection implements <see cref="IQueryable{T}"/>, it will be treated as such.</param>
        /// <param name="index">The index of the subset of objects to be contained by this instance.</param>
        /// <param name="pageSize">The maximum size of any individual subset.</param>
        /// <returns>A subset of this collection of objects that can be individually accessed by index and containing metadata about the collection of objects the subset was created from.</returns>
        /// <seealso cref="PagedList{TSource}"/>
        public static IPagedList<TSource> ToPagedList<TSource>(this IEnumerable<TSource> superset, int index, int pageSize)
        {
            return new PagedList<TSource>(superset, index, pageSize);
        }

        /// <summary>
        /// Creates a subset of this collection of objects that can be individually accessed by index and containing metadata about the collection of objects the subset was created from.
        /// </summary>
        /// <typeparam name="TSource">The type of object the collection should contain.</typeparam>
        /// <param name="superset">The collection of objects to be divided into subsets. If the collection implements <see cref="IQueryable{T}"/>, it will be treated as such.</param>
        /// <param name="index">The index of the subset of objects to be contained by this instance.</param>
        /// <returns>A subset of this collection of objects that can be individually accessed by index and containing metadata about the collection of objects the subset was created from.</returns>
        /// <seealso cref="PagedList{TSource}"/>
        public static IPagedList<TSource> ToPagedList<TSource>(this IEnumerable<TSource> superset, int index)
        {
            return new PagedList<TSource>(superset, index);
        }

        /// <summary>
        /// Creates a subset of this collection of objects that can be individually accessed by index and containing metadata about the collection of objects the subset was created from.
        /// </summary>
        /// <typeparam name="TSource">The type of object the collection should contain.</typeparam>
        /// <param name="superset">The collection of objects to be divided into subsets. If the collection implements <see cref="IQueryable{T}"/>, it will be treated as such.</param>
        /// <param name="index">The index of the subset of objects to be contained by this instance.</param>
        /// <param name="pageSize">The maximum size of any individual subset.</param>
        /// <returns>A subset of this collection of objects that can be individually accessed by index and containing metadata about the collection of objects the subset was created from.</returns>
        /// <seealso cref="PagedList{TSource}"/>
        public static IPagedList<TSource> ToPagedList<TSource>(this IQueryable<TSource> superset, int index, int pageSize)
        {
            return new PagedList<TSource>(superset, index, pageSize);
        }


        /// <summary>
        /// Creates a subset of this collection of objects that can be individually accessed by index and containing metadata about the collection of objects the subset was created from.
        /// </summary>
        /// <typeparam name="TSource">The type of object the collection should contain.</typeparam>
        /// <param name="superset">The collection of objects to be divided into subsets. If the collection implements <see cref="IQueryable{T}"/>, it will be treated as such.</param>
        /// <param name="index">The index of the subset of objects to be contained by this instance.</param>
        /// <returns>A subset of this collection of objects that can be individually accessed by index and containing metadata about the collection of objects the subset was created from.</returns>
        /// <seealso cref="PagedList{TSource}"/>
        public static IPagedList<TSource> ToPagedList<TSource>(this IQueryable<TSource> superset, int index)
        {
            return new PagedList<TSource>(superset, index);
        }
    }
}
