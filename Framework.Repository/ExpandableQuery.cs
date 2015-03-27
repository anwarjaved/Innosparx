using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Collections;

namespace Framework
{
    using System.ComponentModel;

    /// <summary>
    /// An IQueryable wrapper that allows us to visit the query's expression tree just before LINQ to SQL gets to it.
    /// This is based on the excellent work of Tomas Petricek: http://tomasp.net/blog/linq-expand.aspx
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    internal class ExpandableQuery<T> : IOrderedQueryable<T>
    {
        private readonly ExpandableQueryProvider<T> provider;

        private readonly IQueryable<T> inner;

        internal IQueryable<T> InnerQuery { get { return this.inner; } }			// Original query, that we're wrapping

        internal ExpandableQuery(IQueryable<T> inner)
        {
            this.inner = inner;
            this.provider = new ExpandableQueryProvider<T>(this);
        }

        Expression IQueryable.Expression { get { return this.inner.Expression; } }
        Type IQueryable.ElementType { get { return typeof(T); } }
        IQueryProvider IQueryable.Provider { get { return this.provider; } }
        public IEnumerator<T> GetEnumerator() { return this.inner.GetEnumerator(); }
        IEnumerator IEnumerable.GetEnumerator() { return this.inner.GetEnumerator(); }
        public override string ToString() { return this.inner.ToString(); }
    }

}
