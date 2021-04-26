using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Voodoo.EasyFilters.Criteria;

namespace Voodoo.EasyFilters.CollectionCriteria
{
    public class AnyBinaryCriteria<T> : ICollectionCriteria<T> where T : struct
    {
        public AnyBinaryCriteria(ExpressionType operatorType)
        {
            OperatorType = operatorType;
        }

        private ExpressionType OperatorType { get; set; }

        public T? Value { get; set; }

        public IQueryable<TEntity> Apply<TEntity, TProperty>(IQueryable<TEntity> query,
            Expression<Func<TEntity, IEnumerable<TProperty>>> predicate,
            Expression<Func<TProperty, T>> innerPredicate) where TEntity : class
        {
            var expression = GetExpression(predicate, innerPredicate);

            if (expression != null) query = query.Where(expression);

            return query;
        }

        public Expression<Func<TEntity, bool>> GetExpression<TEntity, TProperty>(Expression<Func<TEntity, IEnumerable<TProperty>>> predicate, Expression<Func<TProperty, T>> innerPredicate)
        {
            if (EqualityComparer<T?>.Default.Equals(Value) ||
                predicate?.Body is not MemberExpression memberExpression ||
                memberExpression.Expression == null)
                return null;

            var equalCriteria = new BinaryCriteria<T>(OperatorType) { Value = Value };
            var innerCriteriaExpression = equalCriteria.GetExpression(innerPredicate);

            if (innerCriteriaExpression == null) return null;

            var parameter = Expression.Parameter(memberExpression.Expression.Type, predicate.Parameters.FirstOrDefault()?.Name);
            var condition = Expression.Lambda<Func<TEntity, bool>>(
                Expression.Call(
                    typeof(Enumerable), nameof(Enumerable.Any), new Type[] { typeof(TProperty) },
                    Expression.Property(parameter, memberExpression.Member.Name), innerCriteriaExpression),
                parameter);

            return condition;
        }
    }
}
