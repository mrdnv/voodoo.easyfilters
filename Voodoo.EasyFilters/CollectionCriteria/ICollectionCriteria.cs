using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Voodoo.EasyFilters.CollectionCriteria
{
    public interface ICollectionCriteria<T>
    {
        IQueryable<TEntity> Apply<TEntity, TProperty>(IQueryable<TEntity> query,
            Expression<Func<TEntity, IEnumerable<TProperty>>> predicate,
            Expression<Func<TProperty, T>> innerPredicate) where TEntity : class;

        Expression<Func<TEntity, bool>> GetExpression<TEntity, TProperty>(
            Expression<Func<TEntity, IEnumerable<TProperty>>> predicate,
            Expression<Func<TProperty, T>> innerPredicate);
    }
}
