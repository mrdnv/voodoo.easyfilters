using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Voodoo.EasyFilters.Criteria;
using Voodoo.EasyFilters.CollectionCriteria;

namespace Voodoo.EasyFilters.Extensions
{
    public static class CriteriaExtensions
    {
        public static IQueryable<T> ApplyCriteria<T, TCriteria>(this IQueryable<T> query,
            Expression<Func<T, TCriteria>> predicate, 
            ICriteria<TCriteria> criteria) where T : class
        {
            return criteria?.Apply(query, predicate) ?? query;
        }

        public static IQueryable<T> ApplyCriteria<T, TCollection, TCriteria>(this IQueryable<T> query,
            Expression<Func<T, IEnumerable<TCollection>>> predicate,
            Expression<Func<TCollection, TCriteria>> innerPredicate,
            ICollectionCriteria<TCriteria> criteria) where T : class
        {
            return criteria?.Apply(query, predicate, innerPredicate) ?? query;
        }
    }
}
