using System;
using System.Linq;
using System.Linq.Expressions;

namespace Voodoo.EasyFilters.Criteria
{
    public interface ICriteria<T>
    {
        Expression<Func<TEntity, bool>> GetExpression<TEntity>(Expression<Func<TEntity, T>> predicate);
        IQueryable<TEntity> Apply<TEntity>(IQueryable<TEntity> query, Expression<Func<TEntity, T>> predicate) where TEntity : class;
    }
}
