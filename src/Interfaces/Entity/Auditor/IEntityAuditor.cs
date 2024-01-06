using DataContext.Core.Events.EventArgs;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace DataContext.Core.Interfaces.Entity
{
	public interface IEntityAuditor
    {
		Dictionary<EntityState, List<EntityEntry>> GetAuditableEntries(IEnumerable<EntityEntry> entityEntries);
		Dictionary<EntityState, List<EntityEntry>> Merge(Dictionary<EntityState, List<EntityEntry>> principal, Dictionary<EntityState, List<EntityEntry>> dependent);
		Dictionary<string, object> ToModifiedPropertyBag(EntityEntry entry);
		void AddModifiedPropertiesToAuditor(EntityEntry entry);
		Task<string> PerformAudit(Dictionary<EntityState, List<EntityEntry>> entries, CancellationToken cancellationToken = default);
	}
}
