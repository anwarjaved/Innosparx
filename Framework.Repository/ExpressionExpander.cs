using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;

namespace Framework
{
    /// <summary>
    /// Custom expresssion visitor for ExpandableQuery. This expands calls to Expression.Compile() and
    /// collapses captured lambda references in subqueries which LINQ to SQL can't otherwise handle.
    /// </summary>
    internal class ExpressionExpander : ExpressionVisitor
    {
        // Replacement parameters - for when invoking a lambda expression.
        private readonly Dictionary<ParameterExpression, Expression> replaceVars;

        internal ExpressionExpander() { }

        private ExpressionExpander(Dictionary<ParameterExpression, Expression> replaceVars)
        {
            this.replaceVars = replaceVars;
        }

        protected override Expression VisitParameter(ParameterExpression p)
        {
            if ((this.replaceVars != null) && (this.replaceVars.ContainsKey(p)))
                return this.replaceVars[p];
            
            return base.VisitParameter(p);
        }

        /// <summary>
        /// Flatten calls to Invoke so that Entity Framework can understand it. Calls to Invoke are generated
        /// by PredicateBuilder.
        /// </summary>
        protected override Expression VisitInvocation(InvocationExpression iv)
        {
            Expression target = iv.Expression;
            if (target is MemberExpression) target = TransformExpr((MemberExpression)target);
            if (target is ConstantExpression) target = ((ConstantExpression)target).Value as Expression;

            LambdaExpression lambda = (LambdaExpression)target;

            Dictionary<ParameterExpression, Expression> vars = this.replaceVars == null
                                                                   ? new Dictionary<ParameterExpression, Expression>()
                                                                   : new Dictionary<ParameterExpression, Expression>(this.replaceVars);

            try
            {
                for (int i = 0; i < lambda.Parameters.Count; i++)
                    vars.Add(lambda.Parameters[i], iv.Arguments[i]);
            }
            catch (ArgumentException ex)
            {
                throw new InvalidOperationException("Invoke cannot be called recursively - try using a temporary variable.", ex);
            }

            return new ExpressionExpander(vars).Visit(lambda.Body);
        }

        protected override Expression VisitMethodCall(MethodCallExpression m)
        {
            if (m.Method.Name == "Invoke" && m.Method.DeclaringType == typeof(LinqExtensions))
            {
                Expression target = m.Arguments[0];
                if (target is MemberExpression) target = TransformExpr((MemberExpression)target);
                if (target is ConstantExpression) target = ((ConstantExpression)target).Value as Expression;

                LambdaExpression lambda = (LambdaExpression)target;

                Dictionary<ParameterExpression, Expression> dictionary = this.replaceVars == null
                                                                             ? new Dictionary
                                                                                   <ParameterExpression, Expression>()
                                                                             : new Dictionary
                                                                                   <ParameterExpression, Expression>(
                                                                                   this.replaceVars);

                try
                {
                    for (int i = 0; i < lambda.Parameters.Count; i++)
                        dictionary.Add(lambda.Parameters[i], m.Arguments[i + 1]);
                }
                catch (ArgumentException ex)
                {
                    throw new InvalidOperationException("Invoke cannot be called recursively - try using a temporary variable.", ex);
                }

                return new ExpressionExpander(dictionary).Visit(lambda.Body);
            }

            // Expand calls to an expression's Compile() method:
            if (m.Method.Name == "Compile" && m.Object is MemberExpression)
            {
                var me = (MemberExpression)m.Object;
                Expression newExpr = TransformExpr(me);
                if (newExpr != me) return newExpr;
            }

            // Strip out any nested calls to AsExpandable():
            if (m.Method.Name == "AsExpandable" && m.Method.DeclaringType == typeof(LinqExpandExtensions))
                return m.Arguments[0];

            return base.VisitMethodCall(m);
        }

        protected override Expression VisitMember(MemberExpression m)
        {
            // Strip out any references to expressions captured by outer variables - LINQ to SQL can't handle these:
            if (m.Member.DeclaringType.Name.StartsWith("<>"))
                return TransformExpr(m);

            return base.VisitMember(m);
        }

        Expression TransformExpr(MemberExpression input)
        {
            // Collapse captured outer variables
            if (input == null
                || !(input.Member is FieldInfo)
                || !input.Member.ReflectedType.IsNestedPrivate
                || !input.Member.ReflectedType.Name.StartsWith("<>"))	// captured outer variable
                return input;

            ConstantExpression constantExpression = input.Expression as ConstantExpression;
            if (constantExpression != null)
            {
                object obj = constantExpression.Value;
                if (obj == null) return input;
                Type t = obj.GetType();
                if (!t.IsNestedPrivate || !t.Name.StartsWith("<>")) return input;
                FieldInfo fi = (FieldInfo)input.Member;
                object result = fi.GetValue(obj);
                Expression expression = result as Expression;
                if (expression != null) return this.Visit(expression);
            }

            return input;
        }
    }
}
