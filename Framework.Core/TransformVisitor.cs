using System;

namespace Framework
{
    using System.Linq.Expressions;

    internal class TransformVisitor<TConcrete, TInterface> : ExpressionVisitor
    {
        private readonly ParameterExpression param = Expression.Parameter(typeof(TConcrete), "param_0");

        public static Expression<Func<TConcrete, bool>> Transform(Expression expression)
        {
            var visitor = new TransformVisitor<TConcrete, TInterface>();
            var newLambda = (Expression<Func<TConcrete, bool>>)visitor.Visit(expression);
            return newLambda;
        }

        protected override Expression VisitLambda<T>(Expression<T> node)
        {
            if (typeof(T).IsAssignableFrom(typeof(Func<TInterface, bool>)))
            {
                return Expression.Lambda<Func<TConcrete, bool>>(
                    Visit(node.Body),
                    this.param
                    );
            }

            return base.VisitLambda(node);
        }

        protected override Expression VisitMember(MemberExpression node)
        {
            if (node.Member.DeclaringType.IsAssignableFrom(typeof(TInterface)))
            {
                return Expression.MakeMemberAccess(
                    Visit(node.Expression),
                    typeof(TConcrete).GetProperty(node.Member.Name));
            }

            return base.VisitMember(node);
        }

        protected override Expression VisitParameter(ParameterExpression node)
        {
            if (node.Type.IsAssignableFrom(typeof(TInterface)))
            {
                return this.param;
            }

            return base.VisitParameter(node);
        }
    }
}
