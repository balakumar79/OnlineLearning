using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace Learning.Entities.Extension
{
    public static class FilterExtensions
    {
        public static IQueryable<T> FilterByProperties<T>(this IQueryable<T> query, string searchString, params Expression<Func<T, string>>[] propertySelectors)
        {
            if (string.IsNullOrEmpty(searchString) || propertySelectors == null || propertySelectors.Length == 0)
            {
                return query;
            }

            var searchTerms = searchString.Trim().Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            if (searchTerms.Length == 0)
            {
                return query;
            }

            var predicate = PredicateBuilder.False<T>();
            foreach (var term in searchTerms)
            {
                var termPredicate = PredicateBuilder.False<T>();
                foreach (var selector in propertySelectors)
                {
                    termPredicate = termPredicate.Or(p => selector.Compile()(p).Contains(term));
                }
                predicate = predicate.And(termPredicate);
            }

            return query.Where(predicate);
        }

    }
    // Helper class to build dynamic predicates
    public static class PredicateBuilder
    {
        public static Expression<Func<T, bool>> True<T>() { return p => true; }
        public static Expression<Func<T, bool>> False<T>() { return p => false; }

        public static Expression<Func<T, bool>> Or<T>(this Expression<Func<T, bool>> expr1, Expression<Func<T, bool>> expr2)
        {
            var invokedExpr = Expression.Invoke(expr2, expr1.Parameters);
            return Expression.Lambda<Func<T, bool>>(Expression.OrElse(expr1.Body, invokedExpr), expr1.Parameters);
        }

        public static Expression<Func<T, bool>> And<T>(this Expression<Func<T, bool>> expr1, Expression<Func<T, bool>> expr2)
        {
            var invokedExpr = Expression.Invoke(expr2, expr1.Parameters);
            return Expression.Lambda<Func<T, bool>>(Expression.AndAlso(expr1.Body, invokedExpr), expr1.Parameters);
        }
    }
    public class FilterColumn
    {
        public static string Column { get; set; }
        public static string Condition { get; set; }
        public static Operator Operator { get; set; }
    }

    public enum Operator
    {
        Equal,
        NotEqual,
        Contains,
        Like,
    }
}
