using System.Linq;

namespace Framework
{
    using System.Linq.Expressions;

    internal class ExpandableQueryProvider<T> : IQueryProvider
    {
        private readonly ExpandableQuery<T> query;

        internal ExpandableQueryProvider(ExpandableQuery<T> query)
        {
            this.query = query;
        }

        // The following four methods first call ExpressionExpander to visit the expression tree, then call
        // upon the inner query to do the remaining work.

        IQueryable<TElement> IQueryProvider.CreateQuery<TElement>(Expression expression)
        {
            return new ExpandableQuery<TElement>(this.query.InnerQuery.Provider.CreateQuery<TElement>(expression.Expand()));
        }

        IQueryable IQueryProvider.CreateQuery(Expression expression)
        {
            return this.query.InnerQuery.Provider.CreateQuery(expression.Expand());
        }

        TResult IQueryProvider.Execute<TResult>(Expression expression)
        {
            return this.query.InnerQuery.Provider.Execute<TResult>(expression.Expand());
        }

        object IQueryProvider.Execute(Expression expression)
        {
            return this.query.InnerQuery.Provider.Execute(expression.Expand());
        }
    }

}
