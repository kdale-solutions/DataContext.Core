using DataContext.Core.Interfaces.Entity;
using DataContext.Core.Utilities;
using Global.Configuration;
using Global.Utilities;
using Infrastructure.Diagnostics.Audit;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace DataContext.Core.Context
{
	public abstract class ConcurrentDbContext : DbContext
	{
		private readonly IEntityAuditor _entityAuditor;
		private readonly SemaphoreSlim _semaphore;

		public string Name { get; private set; }

		public bool IsRoot => Parent == null;
		public ConcurrentDbContext Parent { get; set; } = null;

		public bool HasChildren => !Children.IsNullOrEmpty();
		public List<ConcurrentDbContext> Children { get; set; }

		public Dictionary<EntityState, List<EntityEntry>> ChildAuditableEntries { get; set; }

		public ConcurrentDbContext(DbContextOptions options) 
			: base(options)
		{
			this.ChangeTracker.LazyLoadingEnabled = false;
			this.ChangeTracker.AutoDetectChangesEnabled = false;

			_entityAuditor = HostUtility.Resolve<IEntityAuditor>();
			_semaphore = new SemaphoreSlim(1, 1);

			Name = this.GetType().Name;

			this.Children = new List<ConcurrentDbContext>();
			this.ChildAuditableEntries = new Dictionary<EntityState, List<EntityEntry>>();
		}

		public override void Dispose()
		{
			base.Dispose();
		}

		public override async ValueTask DisposeAsync()
		{
			await base.DisposeAsync().ConfigureAwait(false);
		}

		public void SetParent(ConcurrentDbContext parent)
		{
			Parent = parent;
			Parent.Children.Add(this);
		}

		public override int SaveChanges()
		{
			return this.SaveChanges(true);
		}

		public override int SaveChanges(bool acceptAllChangesOnSuccess)
		{
			try
			{
				var count = 0;

				if (this.HasChildren)
				{
					foreach (var child in this.Children)
					{
						count += this.SaveChanges(acceptAllChangesOnSuccess);
					}
				}

				count += this.Save(acceptAllChangesOnSuccess);

				return count;
			}
			catch (Exception _)
			{
				var cancellationTask = this.RequestCancellation();
			}

			return 0;
		}

		private int Save(bool acceptAllChangesOnSuccess)
		{
			var entries = this.ChangeTracker.Entries();

			var preSaveEntries = _entityAuditor.GetAuditableEntries(entries);

			var saveResult = base.SaveChanges(acceptAllChangesOnSuccess);

			if (Parent != null)
			{
				Parent.ChildAuditableEntries.Merge(preSaveEntries);

				return saveResult;
			}

			var token = this.GetCancellationToken();

			var auditTask = _entityAuditor.PerformAudit(ChildAuditableEntries.Merge(preSaveEntries), token)
				.ConfigureAwait(false);

			return saveResult;
		}

		public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
		{
			return this.SaveChangesAsync(true, cancellationToken);
		}

		public override async Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
		{
			try
			{
				var count = 0;

				if (this.HasChildren)
				{
					var childTasks = new List<Task<int>>();

					foreach (var child in this.Children)
					{
						childTasks.Add(child.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken));
					}

					var completedChildTasks = await Task.WhenAll(childTasks);

					count += completedChildTasks.Sum();
				}

				_semaphore.Wait();
				count += await this.SaveAsync(acceptAllChangesOnSuccess, cancellationToken);
				_semaphore.Release();

				return count;
			}
			catch (Exception _)
			{
				var cancellationTask = this.RequestCancellation();
			}

			return 0;
		}

		private async Task<int> SaveAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken)
		{
			var entries = this.ChangeTracker.Entries();

			var preSaveEntries = _entityAuditor.GetAuditableEntries(entries);

			var saveResult = await base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);

			if (this.Parent != null)
			{
				this.Parent.ChildAuditableEntries.Merge(preSaveEntries);

				return saveResult;
			}

			var token = this.GetCancellationToken();

			var auditTask = _entityAuditor.PerformAudit(this.ChildAuditableEntries.Merge(preSaveEntries), token)
				.ConfigureAwait(false);

			return saveResult;
		}
	}
}
