using System.Linq.Expressions;

namespace Voodoo.EasyFilters.CollectionCriteria
{
    public class AnyEqualCriteria<T> : AnyBinaryCriteria<T>, ICollectionCriteria<T> where T : struct
    {
        public AnyEqualCriteria() : base(ExpressionType.Equal)
        {
        }
    }
}
