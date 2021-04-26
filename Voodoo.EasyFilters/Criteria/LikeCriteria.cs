using System;
using System.Linq;
using System.Linq.Expressions;

namespace Voodoo.EasyFilters.Criteria
{
    public class LikeCriteria : ICriteria<string>
    {
        public LikeCriteria () { }

        public LikeCriteria(string searchText)
        {
            this.Value = searchText;
        }

        public string Value { get; set; }

        public IQueryable<TEntity> Apply<TEntity>(IQueryable<TEntity> query, Expression<Func<TEntity, string>> predicate) where TEntity : class
        {
            var condition = GetExpression(predicate);

            return query.Where(condition); ;
        }

        public Expression<Func<TEntity, bool>> GetExpression<TEntity>(Expression<Func<TEntity, string>> predicate)
        {
            if (string.IsNullOrWhiteSpace(Value) ||
                predicate.Body is not MemberExpression memberExpression)
                return null;

            string propertyName = memberExpression.Member.Name;

            if (memberExpression.Expression.Type.GetProperty(propertyName) == null)
                return null;

            var epx = Expression.Parameter(memberExpression.Expression.Type, predicate.Parameters.FirstOrDefault()?.Name);
            Expression col = Expression.Property(epx, propertyName);
            Expression contains = Expression.Call(col, typeof(string).GetMethod("Contains", new[] { typeof(string) }), Expression.Constant(Value));
            Expression<Func<TEntity, bool>> condition = Expression.Lambda<Func<TEntity, bool>>(contains, new ParameterExpression[] { epx });

            return condition;
        }
    }
}
