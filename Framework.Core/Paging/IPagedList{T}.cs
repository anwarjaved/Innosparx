namespace Framework.Paging
{
    using System.Collections.Generic;

    /// <summary>
    /// Represents a subset of a collection of objects that can be individually accessed by index and containing metadata about the superset collection of objects this subset was created from.
    /// </summary>
    /// <typeparam name="T">The type of object the collection should contain.</typeparam>
    /// <seealso cref="IList{T}"/>
    public interface IPagedList<T> : IPagedList, IReadOnlyList<T>
    {
    }
}
