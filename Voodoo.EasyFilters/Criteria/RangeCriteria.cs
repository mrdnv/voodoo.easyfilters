using System;
using System.Linq;
using System.Linq.Expressions;

namespace Voodoo.EasyFilters.Criteria
{
    public class RangeCriteria<T> : ICriteria<T> where T : struct
    {
        public RangeCriteria() { }

        public T? From { get; set; }
        public T? To { get; set; }

        public IQueryable<TEntity> Apply<TEntity>(IQueryable<TEntity> query, Expression<Func<TEntity, T>> predicate) where TEntity : class
        {
            var expression = GetExpression(predicate);

            if (expression != null) query = query.Where(expression);

            return query;
        }

        public Expression<Func<TEntity, bool>> GetExpression<TEntity>(Expression<Func<TEntity, T>> predicate)
        {
            if (predicate.Body is not MemberExpression memberExpression ||
                memberExpression.Expression == null)
                return null;

            var fromExpression = new BinaryCriteria<T>(ExpressionType.GreaterThanOrEqual) { Value = From }.GetExpression(predicate);
            var toExpression = (new BinaryCriteria<T>(ExpressionType.LessThanOrEqual) { Value = To }).GetExpression(predicate);

            if (fromExpression == null || toExpression == null)
                return fromExpression ?? toExpression;

            return Expression.Lambda<Func<TEntity, bool>>(Expression.AndAlso(
                Expression.Invoke(fromExpression, predicate.Parameters),
                Expression.Invoke(toExpression, predicate.Parameters)
            ), predicate.Parameters);
        }
    }
}
