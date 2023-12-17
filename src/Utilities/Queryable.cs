using System.Linq.Expressions;

namespace DataContext.Core.Utilities
{
	public static class QueryableExtensions
    {
        public static Dictionary<int, List<TValue>> ToKeyedCollection<TValue>(this IQueryable<TValue> source, Expression<Func<TValue, int>> keySelector)
        {
            return source.GroupBy(keySelector).ToDictionary(k => k.Key, v => v.Select(x => x).ToList());
        }
    }
}
