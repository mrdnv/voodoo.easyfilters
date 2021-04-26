using System;
using System.Linq;
using System.Linq.Expressions;

namespace Voodoo.EasyFilters.Criteria
{
    public class BinaryCriteria<T> : ICriteria<T> where T : struct
    {
        private ExpressionType OperatorType { get; }
        public T? Value { get; set; }

        public BinaryCriteria(ExpressionType operatorType)
        {
            OperatorType = operatorType;
        }

        public virtual IQueryable<TEntity> Apply<TEntity>(IQueryable<TEntity> query, Expression<Func<TEntity, T>> predicate) where TEntity : class
        {
            var expression = GetExpression(predicate);

            if (expression == null) return query;

            return query.Where(expression);
        }

        public virtual Expression<Func<TEntity, bool>> GetExpression<TEntity>(Expression<Func<TEntity, T>> predicate)
        {
            if (predicate.Body is not MemberExpression memberExpression ||
                !Value.HasValue)
                return null;

            string propertyName = memberExpression.Member.Name;

            if (memberExpression.Expression?.Type.GetProperty(propertyName) == null)
                return null;

            var epx = Expression.Parameter(memberExpression.Expression.Type, predicate.Parameters.FirstOrDefault()?.Name);
            Expression left = Expression.PropertyOrField(epx, propertyName);
            Expression right = Expression.Constant(Value);
            Expression body = Expression.MakeBinary(OperatorType, left, right);
            Expression<Func<TEntity, bool>> condition = Expression.Lambda<Func<TEntity, bool>>(body, new ParameterExpression[] { epx });

            return condition;
        }
    }
}
