//using DataContext.Core.Context;
//using DataContext.Core.Context.Events;
//using DataContext.Core.Entities;
//using DataContext.Core.Events.EventArgs;
//using DataContext.Core.Interfaces.Entity;
//using DataContext.Core.ValueTypes;
//using DiagnosticSuite.Logging;
//using Global.Utilities;
//using Microsoft.EntityFrameworkCore.ChangeTracking;
//using Microsoft.EntityFrameworkCore.Metadata;
//using System.Collections.Concurrent;
//using System.Collections.Frozen;
//using System.Diagnostics;
//using System.Net.Http.Json;

//namespace Infrastructure.Diagnostics.Audit
//{
//	public static class EntityAuditorUtility
//	{
//		public static ConcurrentDictionary<string, Dictionary<string, object>> ModifiedPropertyBags { get; set; }

//		static EntityAuditorUtility() 
//		{
//			ModifiedPropertyBags = new ConcurrentDictionary<string, Dictionary<string, object>>(StringComparer.OrdinalIgnoreCase);

//		}

//		public static IEnumerable<AuditEventArgs> ToAuditEventArgs(this Dictionary<EntityState, List<EntityEntry>> entryMap, DateTime timestamp)
//		{
//			var args = new List<AuditEventArgs>();

//			foreach (var kvPair in entryMap)
//			{
//				args.AddRange(kvPair.ToAuditEventArgs(timestamp));
//			}

//			return args;
//		}

//		public static IEnumerable<AuditEventArgs> ToAuditEventArgs(this KeyValuePair<EntityState, List<EntityEntry>> entries, DateTime timestamp)
//		{
//			var args = new ConcurrentBag<AuditEventArgs>();

//			foreach (var entry in entries.Value)
//			{
//				Dictionary<string, object> modificationBag = null;

//				var entity = ((ITransactionalEntity)entry.Entity);

//				if (entries.Key == EntityState.Modified)
//				{
//					ModifiedPropertyBags.TryGetValue($"{entry.Metadata.Name}_{entity.Id}", out modificationBag);
//				}

//				args.Add(entry.ToAuditEventArgs(entries.Key, timestamp, modificationBag));
//			}

//			return args;
//		}

//		public static AuditEventArgs ToAuditEventArgs(this EntityEntry entry, EntityState entityState, DateTime timestamp, Dictionary<string, object> modifiedProps = null)
//		{
//			var entity = ((ITransactionalEntity)entry.Entity);

//			return new AuditEventArgs(
//				entityState,
//				entry.Entity.GetType().Name[0..],
//				entity.Id,
//				modifiedProps ?? entry.ToModifiedPropertyBag()
//				);
//		}

//		public static Dictionary<string, object> ToModifiedPropertyBag(this EntityEntry entry)
//		{
//			var modificationBag = new Dictionary<string, object>();

			

//			if (entry.State == EntityState.Modified)
//			{
//				var modifiedProps = (from prop in entry.Properties
//									 where prop.IsModified
//									 select prop
//									).ToList();

//				foreach (var prop in modifiedProps)
//				{
//					modificationBag.TryAdd(prop.Metadata.Name, new ModifiedData(prop.OriginalValue, prop.CurrentValue));
//				}
//			}
//			else
//			{
//				var properties = (from prop in entry.Properties
//								  where !prop.Metadata.Name.IsEqualToIgnoreCase(nameof(TransactionalEntity.RowVersion))
//								  select prop);

//				foreach (var prop in properties)
//				{
//					modificationBag.Add(prop.Metadata.Name, prop.CurrentValue);
//				}
//			}

//			return modificationBag;
//		}

//		public static Dictionary<EntityState, List<EntityEntry>> Merge(this Dictionary<EntityState, List<EntityEntry>> principal, Dictionary<EntityState, List<EntityEntry>> dependent)
//		{
//			if (dependent.IsNullOrEmpty()) return principal;

//			foreach (var key in dependent.Keys)
//			{
//				if (principal.TryGetValue(key, out var entries))
//				{
//					entries.AddRange(dependent[key]);
//				}
//				else
//				{
//					principal.TryAdd(key, dependent[key]);
//				}
//			}

//			return principal;
//		}

//		public static void PrintChangeTrackerEntries<TContext>(this TContext context, FrozenSet<EntityEntry> entries, string marker) where TContext : ConcurrentDbContext
//		{
//			Debug.WriteLine($"{marker}: DataContextId: {context.ContextId.InstanceId} ParentId: {context.Parent?.ContextId} Has Children: {context.HasChildren}");
//			foreach (var entry in entries)
//			{
//				var idProp = entry.Entity.GetType().GetProperties().Where(x => x.Name == "Id").FirstOrDefault();
//				var id = idProp.GetValue(entry.Entity);
//				var castId = (int)Convert.ChangeType(id, typeof(int));

//				Debug.WriteLine($"EntityId: {castId} State: {entry.State}");

//				foreach (var item in entry.Properties.Where(x => x.IsModified))
//				{
//					Debug.WriteLine($"Prev: {item.OriginalValue} - Curr: {item.CurrentValue}\n");
//				}
//			}
//		}
//	}
//}
