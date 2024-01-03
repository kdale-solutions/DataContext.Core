using DataContext.Core.Events.EventArgs;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace DataContext.Core.Interfaces.Entity
{
	public interface IEntityAuditor
    {
		Dictionary<EntityState, List<EntityEntry>> GetAuditableEntries(IEnumerable<EntityEntry> entityEntries);
		Task<string> PerformAudit(Dictionary<EntityState, List<EntityEntry>> entries, CancellationToken cancellationToken = default);
	}
}
