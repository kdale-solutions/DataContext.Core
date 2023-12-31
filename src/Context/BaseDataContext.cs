using DataContext.Core.EqualityComparers;
using DataContext.Core.Utilities;
using Infrastructure.Diagnostics.Audit;
using Microsoft.EntityFrameworkCore.Metadata;
using System.Collections.Concurrent;

namespace DataContext.Core.Context
{
	public class BaseDataContext<TContext> : DbContext where TContext : DbContext
	{
		public string Name { get; private set; }

		public ConcurrentDictionary<IEntityType, Dictionary<string, object>> ModificationBags { get; set; }

		public BaseDataContext(DbContextOptions<TContext> options)
		{
			this.ChangeTracker.LazyLoadingEnabled = false;
			this.ChangeTracker.AutoDetectChangesEnabled = false;

			Name = this.GetType().Name;

			ModificationBags = new ConcurrentDictionary<IEntityType, Dictionary<string, object>>(new IEntityTypeEqualityComparer());
		}

		public override int SaveChanges(bool acceptAllChangesOnSuccess)
		{
			var result = base.SaveChanges(acceptAllChangesOnSuccess);

			this.HandleAuditEvent(this.ChangeTracker.Entries())
				.ConfigureAwait(false);

			return result;
		}

		public override int SaveChanges()
		{
			var result = base.SaveChanges();

			this.HandleAuditEvent(this.ChangeTracker.Entries())
				.ConfigureAwait(false);

			return result;
		}

		public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
		{
			try
			{
				var saveTask = base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);

				var token = this.GetCancellationToken();

				this.HandleAuditEvent(this.ChangeTracker.Entries(), token)
					.ConfigureAwait(false);

				return saveTask;
			}
			catch (Exception _)
			{
				this.RequestCancellation();
			}

			return Task.FromResult(0);
		}

		public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
		{
			try
			{
				var saveTask = base.SaveChangesAsync(cancellationToken);

				var token = this.GetCancellationToken();

				this.HandleAuditEvent(this.ChangeTracker.Entries(), token)
					.ConfigureAwait(false);

				return saveTask;
			}
			catch (Exception _)
			{
				this.RequestCancellation();
			}

			return Task.FromResult(0);
		}
	}
}
