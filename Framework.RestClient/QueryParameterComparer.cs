namespace Framework.Rest
{
    using System.Collections.Generic;

    /// <summary>
    /// QueryParameter Comparer.
    /// </summary>
    /// <author>Anwar</author>
    /// <datetime>3/24/2011 11:22 PM</datetime>
    public sealed class QueryParameterComparer : IComparer<QueryParameter>
    {
        /// <summary>
        /// Compares the specified x.
        /// </summary>
        /// <param name="left">The x paramter.</param>
        /// <param name="right">The y paramter.</param>
        /// <returns>Comparison result.</returns>
        /// <author>Anwar</author>
        /// <datetime>3/24/2011 11:22 PM</datetime>
        public int Compare(QueryParameter left, QueryParameter right)
        {
            return left.Name.Equals(right.Name)
                       ? string.Compare(left.Value, right.Value, System.StringComparison.Ordinal)
                       : string.Compare(left.Name, right.Name, System.StringComparison.Ordinal);
        }
    }
}
