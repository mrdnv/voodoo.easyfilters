using System.Linq.Expressions;

namespace Voodoo.EasyFilters.Criteria
{
    public class EqualCriteria<T> : BinaryCriteria<T>, ICriteria<T> where T : struct
    {
        public EqualCriteria() : base(ExpressionType.Equal) { }
    }
}
