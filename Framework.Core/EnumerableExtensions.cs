namespace Framework
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Linq;
    using System.Text;

    /// -------------------------------------------------------------------------------------------------
    /// <summary>
    ///     Enumerable extensions.
    /// </summary>
    /// -------------------------------------------------------------------------------------------------
    [EditorBrowsable(EditorBrowsableState.Never)]
    public static class EnumerableExtensions
    {
        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        /// An IEnumerable&lt;T&gt; extension method that converts a source to a linked list.
        /// </summary>
        /// <exception cref="ArgumentNullException">
        /// Thrown when one or more required arguments are null.
        /// </exception>
        /// <typeparam name="T">
        /// Generic type parameter.
        /// </typeparam>
        /// <param name="source">
        /// The source to act on.
        /// </param>
        /// <returns>
        /// source as a LinkedList&lt;T&gt;
        /// </returns>
        /// -------------------------------------------------------------------------------------------------
        public static LinkedList<T> ToLinkedList<T>(this IEnumerable<T> source)
        {
            if (source == null)
            {
                throw new ArgumentNullException("source");
            }

            return new LinkedList<T>(source);
        }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        /// An IEnumerable&lt;T&gt; extension method that converts a source to a read only list.
        /// </summary>
        /// <exception cref="ArgumentNullException">
        /// Thrown when one or more required arguments are null.
        /// </exception>
        /// <typeparam name="T">
        /// Generic type parameter.
        /// </typeparam>
        /// <param name="source">
        /// The source to act on.
        /// </param>
        /// <returns>
        /// source as an IReadOnlyList&lt;T&gt;
        /// </returns>
        /// -------------------------------------------------------------------------------------------------
        public static IReadOnlyList<T> ToReadOnlyList<T>(this IEnumerable<T> source)
        {
            if (source == null)
            {
                throw new ArgumentNullException("source");
            }

            IReadOnlyList<T> list = source as IReadOnlyList<T>;
            if (list != null)
            {
                return list;
            }

            return new List<T>(source);
        }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        /// An IEnumerable{T}; extension method that converts this object to a concatenated
        /// string.
        /// </summary>
        /// <typeparam name="T">
        /// Generic type parameter.
        /// </typeparam>
        /// <param name="source">
        /// The source to act on.
        /// </param>
        /// <param name="selector">
        /// The selector.
        /// </param>
        /// <param name="separator">
        /// The separator.
        /// </param>
        /// <returns>
        /// The given data converted to a string.
        /// </returns>
        /// -------------------------------------------------------------------------------------------------
        public static string ToConcatenatedString<T>(this IEnumerable<T> source, Func<T, string> selector, string separator = " ")
        {
            StringBuilder sb = new StringBuilder();

            IList<T> valueArray = source.ToList();

            int count = valueArray.Count;

            for (int index = 0; index < count; index++)
            {
                T item = valueArray[index];
                string value = selector(item);

                if (string.IsNullOrWhiteSpace(value))
                {
                    continue;
                }

                sb.Append(value);

                if (index < count - 1)
                {
                    sb.Append(separator);
                }
            }

            return sb.ToString().Trim();
        }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        /// An IEnumerable&lt;string&gt; extension method that converts this object to a concatenated
        /// string.
        /// </summary>
        /// <param name="source">
        /// The source to act on.
        /// </param>
        /// <param name="separator">
        /// The separator.
        /// </param>
        /// <returns>
        /// The given data converted to a string.
        /// </returns>
        /// -------------------------------------------------------------------------------------------------
        public static string ToConcatenatedString(this IEnumerable<string> source, string separator = " ")
        {
            StringBuilder sb = new StringBuilder();

            IList<string> valueArray = source.Where(value => !value.IsEmpty()).ToList();

            int count = valueArray.Count;

            for (int index = 0; index < count; index++)
            {
                string value = valueArray[index];
                sb.Append(value);

                if (index < count - 1)
                {
                    sb.Append(separator);
                }
            }

            return sb.ToString().Trim();
        }

        /// <summary>
        /// An IEnumerable{T}; extension method that compares two this IEnumerable{T};
        /// objects to determine their relative ordering.
        /// </summary>
        /// <typeparam name="T">Generic type parameter.</typeparam>
        /// <param name="current">The current to act on.</param>
        /// <param name="new">The new.</param>
        /// <returns>Returns Difference Between Enumerable.</returns>
        public static CompareInfo<T> Compare<T>(this IEnumerable<T> current, IEnumerable<T> @new) where T : struct
        {
            var oldList = current.ToList();
            var newList = @new.ToList();
            var added = newList.Except(oldList).ToList();

            var deleted = oldList.Except(newList).ToList();

            var edited = oldList.Where(newList.Contains).ToList();

            return new CompareInfo<T>(added, edited, deleted);
        }
    }
}
