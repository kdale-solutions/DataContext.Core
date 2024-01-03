using DataContext.Core.Interfaces.Entity;
using DataContext.Core.Utilities;
using Global.Configuration;
using System.Threading;

namespace DataContext.Core.Context
{
	public abstract class BaseDataContext<TContext> : DbContext where TContext : DbContext
	{
		private readonly IEntityAuditor _entityAuditor;

		public string Name { get; private set; }

		public BaseDataContext(DbContextOptions<TContext> options) 
			: base(options)
		{
			this.ChangeTracker.LazyLoadingEnabled = false;
			this.ChangeTracker.AutoDetectChangesEnabled = false;

			_entityAuditor = HostUtility.Resolve<IEntityAuditor>();

			Name = this.GetType().Name;
		}

		public override int SaveChanges()
		{
			var result = this.SaveChanges(true);

			return result;
		}

		public override int SaveChanges(bool acceptAllChangesOnSuccess)
		{
			try
			{
				var entries = this.ChangeTracker.Entries();

				var preSaveEntries = _entityAuditor.GetAuditableEntries(entries);

				var saveResult = base.SaveChanges(acceptAllChangesOnSuccess);

				var token = this.GetCancellationToken();

				var auditTask = _entityAuditor.PerformAudit(preSaveEntries, token).ConfigureAwait(false);

				return saveResult;
			}
			catch (Exception _)
			{
				var cancellationTask = this.RequestCancellation();
			}

			return 0;
		}

		public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
		{
			return this.SaveChangesAsync(true, cancellationToken);
		}

		public override async Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
		{
			try
			{
				var entries = this.ChangeTracker.Entries();

				var preSaveEntries = _entityAuditor.GetAuditableEntries(entries);

				var saveResult = await base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);

				var token = this.GetCancellationToken();

				var auditTask = _entityAuditor.PerformAudit(preSaveEntries, token).ConfigureAwait(false);

				return saveResult;
			}
			catch (Exception _)
			{
				var cancellationTask = this.RequestCancellation();
			}

			return 0;
		}
	}
}
