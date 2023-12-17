using DataContext.Core.Interfaces;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Diagnostics;

namespace DataContext.Core.Context
{
	public class DataContextFactory<TContext> : IDataContextFactory<TContext> where TContext : DbContext
    {
        private readonly IDbContextFactory<TContext> _dbContextFactory;

        public DataContextFactory(
            IDbContextFactory<TContext> dbContextFactory
            )
        {
            _dbContextFactory = dbContextFactory;
		}

		public TContext CreateDataContext(QueryTrackingBehavior trackingBehavior = QueryTrackingBehavior.TrackAll)
		{
			var context = _dbContextFactory.CreateDbContext();

			context.ChangeTracker.QueryTrackingBehavior = trackingBehavior; 
            
            return context;
		}

		public TContext CreateDataContext(object rootEntity, Action<EntityEntryGraphNode> callback)
        {
            Debug.WriteLine($"Thread ID: {Thread.CurrentThread.ManagedThreadId}");

            var context = _dbContextFactory.CreateDbContext();

            context.ChangeTracker.TrackGraph(rootEntity, callback);

            return context;
        }
    }
}
