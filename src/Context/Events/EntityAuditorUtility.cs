using DataContext.Core.Context;
using DataContext.Core.Events.EventArgs;
using DataContext.Core.Interfaces.Entity;
using DataContext.Core.ValueTypes;
using DiagnosticSuite.Logging;
using Global.Utilities;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata;
using System.Collections.Concurrent;
using System.Net.Http.Json;

namespace Infrastructure.Diagnostics.Audit
{
	public static class EntityAuditorUtility
	{
		public static ConcurrentDictionary<string, Dictionary<string, object>> ModificationBags { get; set; }

		static EntityAuditorUtility() 
		{
			ModificationBags = new ConcurrentDictionary<string, Dictionary<string, object>>(StringComparer.OrdinalIgnoreCase);

		}

		public static IEnumerable<AuditEventArgs> ToAuditEventArgs(this Dictionary<EntityState, List<EntityEntry>> entryMap, DateTime timestamp)
		{
			var args = new List<AuditEventArgs>();

			foreach (var kvPair in entryMap)
			{
				args.AddRange(kvPair.ToAuditEventArgs(timestamp));
			}

			return args;
		}

		public static IEnumerable<AuditEventArgs> ToAuditEventArgs(this KeyValuePair<EntityState, List<EntityEntry>> entries, DateTime timestamp)
		{
			var args = new ConcurrentBag<AuditEventArgs>();

			foreach (var entry in entries.Value)
			{
				Dictionary<string, object> modificationBag = null;

				var entity = ((ITransactionalEntity)entry.Entity);

				if (entries.Key == EntityState.Modified)
				{
					ModificationBags.TryGetValue($"{entry.Metadata.Name}_{entity.Id}", out modificationBag);
				}

				args.Add(entry.ToAuditEventArgs(entries.Key, timestamp, modificationBag));
			}

			return args;
		}

		public static AuditEventArgs ToAuditEventArgs(this EntityEntry entry, EntityState entityState, DateTime timestamp, Dictionary<string, object> modifiedProps = null)
		{
			var entity = ((ITransactionalEntity)entry.Entity);

			return new AuditEventArgs(
				entityState,
				timestamp,
				entry.Entity.GetType().Name[0..],
				entity.Id,
				modifiedProps ?? entry.GetModificationBag()
				);
		}

		public static Dictionary<string, object> GetModificationBag(this EntityEntry entry)
		{
			var modificationBag = new Dictionary<string, object>();

			if (entry.State == EntityState.Modified)
			{
				var modifiedProps = (from prop in entry.Properties
									 where prop.IsModified
									 select prop
									 ).AsEnumerable();

				foreach (var prop in modifiedProps)
				{
					modificationBag.TryAdd(prop.Metadata.Name, new ModifiedData(prop.OriginalValue, prop.CurrentValue));
				}
			}
			else
			{
				foreach (var prop in entry.Properties)
				{
					modificationBag.Add(prop.Metadata.Name, prop.CurrentValue);
				}
			}

			return modificationBag;
		}
	}
}
