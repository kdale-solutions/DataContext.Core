using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace DataContext.Core.Interfaces
{
	public interface IDataContextFactory<TContext> where TContext : DbContext
    {
		TContext CreateDataContext(QueryTrackingBehavior trackingBehavior = QueryTrackingBehavior.TrackAll);
		TContext CreateDataContext(object rootEntity, Action<EntityEntryGraphNode> callback);
	}
}
