using System.ComponentModel;
using System.Reflection;

namespace Framework
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;

    ///-------------------------------------------------------------------------------------------------
    /// <summary>
    ///     Linq Extensions.
    /// </summary>
    ///
    /// <remarks>
    ///     LM ANWAR, 6/3/2013.
    /// </remarks>
    ///-------------------------------------------------------------------------------------------------
    public static class LinqExtensions
    {
        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     A T extension method that default if empty.
        /// </summary>
        ///
        /// <typeparam name="T">
        ///     Generic type parameter.
        /// </typeparam>
        /// <param name="value">
        ///     The value to act on.
        /// </param>
        /// <param name="defaultValue">
        ///     The default value.
        /// </param>
        ///
        /// <returns>
        ///     .
        /// </returns>
        ///-------------------------------------------------------------------------------------------------
        public static T DefaultIfEmpty<T>(this T value, T defaultValue)
        {
            return value.Equals(default(T)) ? defaultValue : value;
        }

        /// <summary>
        /// Returns all distinct elements of the given source, where "distinctness"
        /// is determined via a projection and the specified comparer for the projected type.
        /// </summary>
        /// <remarks>
        /// This operator uses deferred execution and streams the results, although
        /// a set of already-seen keys is retained. If a key is seen multiple times,
        /// only the first element with that key is returned.
        /// </remarks>
        /// <typeparam name="TSource">Type of the source sequence.</typeparam>
        /// <param name="enumerable">Source sequence.</param>
        /// <param name="comparer">The equality comparer to use to determine whether or not keys are equal.
        /// If null, the default equality comparer for <c>TSource</c> is used.</param>
        /// <returns>A sequence consisting of distinct elements from the source sequence,
        /// comparing them by the specified key projection.</returns>
        public static IEnumerable<TSource> Distinct<TSource>(this IEnumerable<TSource> enumerable, Func<TSource, TSource, bool> comparer)
        {
            return enumerable.Distinct(new LambdaEqualityComparer<TSource>(comparer));
        }

        /// <summary>
        /// Returns all distinct elements of the given source, where "distinctness"
        /// is determined via a projection and the specified comparer for the projected type.
        /// </summary>
        /// <typeparam name="TSource">Type of the source sequence.</typeparam>
        /// <typeparam name="TSelection">The type of the selection.</typeparam>
        /// <param name="source">Source sequence.</param>
        /// <param name="selector">The selector.</param>
        /// <returns>
        /// A sequence consisting of distinct elements from the source sequence,
        /// comparing them by the specified key projection.
        /// </returns>
        /// <remarks>
        /// This operator uses deferred execution and streams the results, although
        /// a set of already-seen keys is retained. If a key is seen multiple times,
        /// only the first element with that key is returned.
        /// </remarks>
        public static IEnumerable<TSource> Distinct<TSource, TSelection>(this IEnumerable<TSource> source, Func<TSource, TSelection> selector)
        {
            return source.Distinct(new SelectorEqualityComparer<TSource, TSelection>(selector));
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     An IQueryable&lt;T&gt; extension method that sorts.
        /// </summary>
        ///
        /// <typeparam name="T">
        ///     Generic type parameter.
        /// </typeparam>
        /// <param name="source">
        ///     Source sequence.
        /// </param>
        /// <param name="sortBy">
        ///     Amount to sort by.
        /// </param>
        /// <param name="sortDirection">
        ///     (optional) the sort direction.
        /// </param>
        ///
        /// <returns>
        ///     Ordered Querable.
        /// </returns>
        ///-------------------------------------------------------------------------------------------------
        public static IQueryable<T> Sort<T>(this IQueryable<T> source, string sortBy, string sortDirection = "asc")
        {
            if (!string.IsNullOrWhiteSpace(sortBy) && !string.IsNullOrWhiteSpace(sortDirection))
            {
                var type = typeof(T);
                var parameter = Expression.Parameter(type, "p");
                Expression propertyReference = parameter;
           
                foreach (var member in sortBy.Split('.'))
                {
                    propertyReference = Expression.Property(propertyReference, member);
                }

                Type propertyType = propertyReference.Type;

                if (propertyType == typeof(Guid))
                {
                    return SortInternal(source, sortDirection, Expression.Lambda<Func<T, Guid>>(propertyReference, parameter));
                }

                if (propertyType == typeof(int))
                {
                    return SortInternal(source, sortDirection, Expression.Lambda<Func<T, int>>(propertyReference, parameter));
                }

                if (propertyType == typeof(short))
                {
                    return SortInternal(source, sortDirection, Expression.Lambda<Func<T, short>>(propertyReference, parameter));
                }

                if (propertyType == typeof(byte))
                {
                    return SortInternal(source, sortDirection, Expression.Lambda<Func<T, byte>>(propertyReference, parameter));
                }

                if (propertyType == typeof(long))
                {
                    return SortInternal(source, sortDirection, Expression.Lambda<Func<T, long>>(propertyReference, parameter));
                }

                if (propertyType == typeof(float))
                {
                    return SortInternal(source, sortDirection, Expression.Lambda<Func<T, float>>(propertyReference, parameter));
                }

                if (propertyType == typeof(double))
                {
                    return SortInternal(source, sortDirection, Expression.Lambda<Func<T, double>>(propertyReference, parameter));
                }

                if (propertyType == typeof(decimal))
                {
                    return SortInternal(source, sortDirection, Expression.Lambda<Func<T, decimal>>(propertyReference, parameter));
                }

                if ((propertyType == typeof(DateTime)))
                {
                    return SortInternal(source, sortDirection, Expression.Lambda<Func<T, DateTime>>(propertyReference, parameter));
                }

                if ((propertyType.IsEnum))
                {
                    Type enumType = Enum.GetUnderlyingType(propertyType);

                    if (enumType == typeof(byte))
                    {
                        return SortInternal(source, sortDirection, Expression.Lambda<Func<T, byte>>(Expression.Convert(propertyReference, enumType), parameter));
                    }

                    if (enumType == typeof(int))
                    {
                        return SortInternal(source, sortDirection, Expression.Lambda<Func<T, int>>(Expression.Convert(propertyReference, enumType), parameter));

                    }

                    if (enumType == typeof(short))
                    {
                        return SortInternal(source, sortDirection, Expression.Lambda<Func<T, short>>(Expression.Convert(propertyReference, enumType), parameter));
                    }

                    return SortInternal(source, sortDirection, Expression.Lambda<Func<T, long>>(Expression.Convert(propertyReference, enumType), parameter));
                }


                return SortInternal(source, sortDirection, Expression.Lambda<Func<T, string>>(Expression.Convert(propertyReference, propertyType), parameter));
            }

            return source;
        }


        private static IQueryable<T> SortInternal<T, V>(IQueryable<T> source, string sortDirection, Expression<Func<T, V>> sortExpression)
        {
            switch (sortDirection.ToLower())
            {
                case "asc":
                    return source.OrderBy(sortExpression);
                default:
                    return source.OrderByDescending(sortExpression);
            }
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     An IQueryable&lt;T&gt; extension method that sorts.
        /// </summary>
        ///
        /// <typeparam name="T">
        ///     Generic type parameter.
        /// </typeparam>
        /// <param name="source">
        ///     Source sequence.
        /// </param>
        /// <param name="sortBy">
        ///     Amount to sort by.
        /// </param>
        /// <param name="sortDirection">
        ///     (optional) the sort direction.
        /// </param>
        ///
        /// <returns>
        ///     Ordered Querable.
        /// </returns>
        ///-------------------------------------------------------------------------------------------------
        public static IEnumerable<T> Sort<T>(this IEnumerable<T> source, string sortBy, string sortDirection)
        {
            return source.AsQueryable().Sort(sortBy, sortDirection);
        }

        /// <summary>Returns the number of elements in a sequence.</summary>
        /// <returns>The number of elements in the input sequence.</returns>
        /// <param name="source">A sequence that contains elements to be counted.</param>
        /// <exception cref="T:System.ArgumentNullException">
        /// <paramref name="source" /> is null.</exception>
        /// <exception cref="T:System.OverflowException">The number of elements in <paramref name="source" /> is larger than <see cref="F:System.Int32.MaxValue" />.</exception>
        public static int GetCount(this IEnumerable source)
        {
            ICollection collectionSource = source as ICollection;
            if (collectionSource != null)
            {
                return collectionSource.Count;
            }

            return source.Cast<object>().Count();
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     An IEnumerable&lt;T&gt; extension method that applies an operation to all items in this
        ///     collection.
        /// </summary>
        ///
        /// <typeparam name="T">
        ///     Generic type parameter.
        /// </typeparam>
        /// <param name="source">
        ///     Source sequence.
        /// </param>
        /// <param name="action">
        ///     The action.
        /// </param>
        ///-------------------------------------------------------------------------------------------------
        public static void ForEach<T>(this IEnumerable<T> source, Action<T> action)
        {
            foreach (T element in source)
                action(element);
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     An Expression&lt;Func&lt;TResult&gt;&gt; extension method that executes the given
        ///     operation on a different thread, and waits for the result.
        /// </summary>
        ///
        /// <typeparam name="TResult">
        ///     Type of the result.
        /// </typeparam>
        /// <param name="expr">
        ///     The expr to act on.
        /// </param>
        ///
        /// <returns>
        ///     Result of Expression.
        /// </returns>
        ///-------------------------------------------------------------------------------------------------
        public static TResult Invoke<TResult>(this Expression<Func<TResult>> expr)
        {
            return expr.Compile().Invoke();
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     An Expression&lt;Func&lt;T1,TResult&gt;&gt; extension method that executes the given
        ///     operation on a different thread, and waits for the result.
        /// </summary>
        ///
        /// <typeparam name="T1">
        ///     Generic type parameter.
        /// </typeparam>
        /// <typeparam name="TResult">
        ///     Type of the result.
        /// </typeparam>
        /// <param name="expr">
        ///     The expr to act on.
        /// </param>
        /// <param name="arg1">
        ///     The first argument.
        /// </param>
        ///
        /// <returns>
        ///     Result of Expression.
        /// </returns>
        ///-------------------------------------------------------------------------------------------------
        public static TResult Invoke<T1, TResult>(this Expression<Func<T1, TResult>> expr, T1 arg1)
        {
            return expr.Compile().Invoke(arg1);
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     An Expression&lt;Func&lt;T1,T2,TResult&gt;&gt; extension method that executes the given
        ///     operation on a different thread, and waits for the result.
        /// </summary>
        ///
        /// <typeparam name="T1">
        ///     Generic type parameter.
        /// </typeparam>
        /// <typeparam name="T2">
        ///     Generic type parameter.
        /// </typeparam>
        /// <typeparam name="TResult">
        ///     Type of the result.
        /// </typeparam>
        /// <param name="expr">
        ///     The expr to act on.
        /// </param>
        /// <param name="arg1">
        ///     The first argument.
        /// </param>
        /// <param name="arg2">
        ///     The second argument.
        /// </param>
        ///
        /// <returns>
        ///     Result of Expression.
        /// </returns>
        ///-------------------------------------------------------------------------------------------------
        public static TResult Invoke<T1, T2, TResult>(this Expression<Func<T1, T2, TResult>> expr, T1 arg1, T2 arg2)
        {
            return expr.Compile().Invoke(arg1, arg2);
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     An Expression&lt;Func&lt;T1,T2,T3,TResult&gt;&gt; extension method that executes the
        ///     given operation on a different thread, and waits for the result.
        /// </summary>
        ///
        /// <typeparam name="T1">
        ///     Generic type parameter.
        /// </typeparam>
        /// <typeparam name="T2">
        ///     Generic type parameter.
        /// </typeparam>
        /// <typeparam name="T3">
        ///     Generic type parameter.
        /// </typeparam>
        /// <typeparam name="TResult">
        ///     Type of the result.
        /// </typeparam>
        /// <param name="expr">
        ///     The expr to act on.
        /// </param>
        /// <param name="arg1">
        ///     The first argument.
        /// </param>
        /// <param name="arg2">
        ///     The second argument.
        /// </param>
        /// <param name="arg3">
        ///     The third argument.
        /// </param>
        ///
        /// <returns>
        ///     Result of Expression.
        /// </returns>
        ///-------------------------------------------------------------------------------------------------
        public static TResult Invoke<T1, T2, T3, TResult>(
            this Expression<Func<T1, T2, T3, TResult>> expr, T1 arg1, T2 arg2, T3 arg3)
        {
            return expr.Compile().Invoke(arg1, arg2, arg3);
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     An Expression&lt;Func&lt;T1,T2,T3,T4,TResult&gt;&gt; extension method that executes the
        ///     given operation on a different thread, and waits for the result.
        /// </summary>
        ///
        /// <typeparam name="T1">
        ///     Generic type parameter.
        /// </typeparam>
        /// <typeparam name="T2">
        ///     Generic type parameter.
        /// </typeparam>
        /// <typeparam name="T3">
        ///     Generic type parameter.
        /// </typeparam>
        /// <typeparam name="T4">
        ///     Generic type parameter.
        /// </typeparam>
        /// <typeparam name="TResult">
        ///     Type of the result.
        /// </typeparam>
        /// <param name="expr">
        ///     The expr to act on.
        /// </param>
        /// <param name="arg1">
        ///     The first argument.
        /// </param>
        /// <param name="arg2">
        ///     The second argument.
        /// </param>
        /// <param name="arg3">
        ///     The third argument.
        /// </param>
        /// <param name="arg4">
        ///     The fourth argument.
        /// </param>
        ///
        /// <returns>
        ///     Result of Expression.
        /// </returns>
        ///-------------------------------------------------------------------------------------------------
        public static TResult Invoke<T1, T2, T3, T4, TResult>(
            this Expression<Func<T1, T2, T3, T4, TResult>> expr, T1 arg1, T2 arg2, T3 arg3, T4 arg4)
        {
            return expr.Compile().Invoke(arg1, arg2, arg3, arg4);
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     An Expression&lt;Func&lt;TInterface,bool&gt;&gt; extension method that casts the given
        ///     interface expression.
        /// </summary>
        ///
        /// <exception cref="Exception">
        ///     Thrown when an exception error condition occurs.
        /// </exception>
        ///
        /// <typeparam name="TInterface">
        ///     Type of the interface.
        /// </typeparam>
        /// <typeparam name="TConcrete">
        ///     Type of the concrete.
        /// </typeparam>
        /// <param name="interfaceExpression">
        ///     The interfaceExpression to act on.
        /// </param>
        ///
        /// <returns>
        ///     .
        /// </returns>
        ///-------------------------------------------------------------------------------------------------
        public static Expression<Func<TConcrete, bool>> Cast<TInterface, TConcrete>(this Expression<Func<TInterface, bool>> interfaceExpression)
          where TConcrete : TInterface
        {
            if (!typeof(TInterface).IsAssignableFrom(typeof(TConcrete)))
            {
                throw new Exception("TInterface must be assignable from TConcrete to convert an expression.");
            }

            return TransformVisitor<TConcrete, TInterface>.Transform(interfaceExpression);
        }
    }
}
