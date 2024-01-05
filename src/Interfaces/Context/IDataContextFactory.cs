using DataContext.Core.Context;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace DataContext.Core.Interfaces
{
	public interface IDataContextFactory<TContext> where TContext : ConcurrentDbContext
    {
		TContext CreateDataContext(QueryTrackingBehavior trackingBehavior = QueryTrackingBehavior.NoTracking);
		TContext CreateDataContext(TContext parent, QueryTrackingBehavior trackingBehavior = QueryTrackingBehavior.NoTracking);
		Task<TContext> CreateDataContextAsync(QueryTrackingBehavior trackingBehavior = QueryTrackingBehavior.NoTracking);
		Task<TContext> CreateDataContextAsync(TContext parent, QueryTrackingBehavior trackingBehavior = QueryTrackingBehavior.NoTracking);
	}
}
