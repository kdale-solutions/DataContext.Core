using DataContext.Core.Entities;
using DataContext.Core.Interfaces.Entity;
using DataContext.Core.ValueTypes;
using DiagnosticSuite.Logging;
using Global.Utilities;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Net.Http.Json;

namespace DataContext.Core.Events.EventArgs
{
	public class EntityAuditor : IEntityAuditor
	{
		private const string _centralContextAddress = "https://localhost:7007/central-context/";
		private const string _centralAuditAddress = "audit";

		private static HttpClient _httpClient;

		private static EntityState[] _auditableEntityStates;

		public ConcurrentDictionary<string, Dictionary<string, object>> ModifiedPropertyBags { get; set; }

		public EntityAuditor()
		{
			_httpClient = new HttpClient
			{
				BaseAddress = new Uri(_centralContextAddress)
			};

			_auditableEntityStates = new EntityState[]
			{
				EntityState.Added,
				EntityState.Modified,
				EntityState.Deleted
			};

			ModifiedPropertyBags = new ConcurrentDictionary<string, Dictionary<string, object>>(StringComparer.OrdinalIgnoreCase);
		}

		/// <summary>
		/// Gathers copies of each auditable <see cref="EntityEntry" /> before <see cref="EntityState" /> changes. 
		/// </summary>
		/// <param name="entityEntries"></param>
		/// <returns></returns>
		public Dictionary<EntityState, List<EntityEntry>> GetAuditableEntries(IEnumerable<EntityEntry> entityEntries)
		{
			var entryMap = (from entry in entityEntries
							where entry.Metadata.ClrType.IsAssignableTo(typeof(ITransactionalEntity)) &&
								  _auditableEntityStates.Contains(entry.State)
							group entry by entry.State into entryGroup
							select entryGroup
						   ).ToDictionary(k => k.Key, v => v.ToList());

			//if (entryMap.TryGetValue(EntityState.Modified, out var entries))
			//{
			//	foreach (var entry in entries)
			//	{
			//		var propertyBag = ToModifiedPropertyBag(entry);
			//		Debug.WriteLine($"TID: {Thread.CurrentThread.ManagedThreadId} - {entry.DebugView.LongView}");
			//		if (!propertyBag.IsNullOrEmpty())
			//		{
			//			this.ModifiedPropertyBags.TryAdd($"{entry.Metadata.Name}_{((ITransactionalEntity)entry.Entity).Id}", propertyBag);
			//		}
			//	}
			//}

			return entryMap;
		}

		public void AddModifiedPropertiesToAuditor(EntityEntry entry)
		{
			var propertyBag = ToModifiedPropertyBag(entry);

			this.ModifiedPropertyBags.TryAdd($"{entry.Metadata.ClrType.Name}_{((ITransactionalEntity)entry.Entity).Id}", propertyBag);
		}

		public async Task<string> PerformAudit(Dictionary<EntityState, List<EntityEntry>> entries, CancellationToken cancellationToken = default)
		{
			if (entries.IsNullOrEmpty()) return null;

			var responseMessage = string.Empty;
			HttpResponseMessage res = null;

			try
			{
				var requestBody = ToAuditEventArgs(entries);

				using HttpResponseMessage response = await _httpClient.PostAsJsonAsync(
					_centralAuditAddress,
					requestBody,
					InternalJsonSerializerOptions.Default,
					cancellationToken
					);

				res = response;

				response.EnsureSuccessStatusCode();

				responseMessage = await response.Content.ReadAsStringAsync();
			}
			catch (Exception ex)
			{
				responseMessage = ex.Message;

				UtilityLogger<HttpClient>.Error($"Error reaching {_centralAuditAddress}", ex);
			}
			finally
			{
				ModifiedPropertyBags.Clear();
			}

			return responseMessage;
		}

		private IEnumerable<AuditEventArgs> ToAuditEventArgs(Dictionary<EntityState, List<EntityEntry>> entryMap)
		{
			var args = new List<AuditEventArgs>();

			foreach (var kvPair in entryMap)
			{
				args.AddRange(ToAuditEventArgs(kvPair));
			}

			return args;
		}

		public IEnumerable<AuditEventArgs> ToAuditEventArgs(KeyValuePair<EntityState, List<EntityEntry>> entries)
		{
			var args = new ConcurrentBag<AuditEventArgs>();

			foreach (var entry in entries.Value)
			{
				Dictionary<string, object> modificationBag = null;

				var entity = ((ITransactionalEntity)entry.Entity);

				if (entries.Key == EntityState.Modified)
				{
					ModifiedPropertyBags.TryGetValue($"{entry.Metadata.ClrType.Name}_{entity.Id}", out modificationBag);
				}

				args.Add(ToAuditEventArgs(entry, entries.Key, modificationBag));
			}

			return args;
		}

		public AuditEventArgs ToAuditEventArgs(EntityEntry entry, EntityState entityState, Dictionary<string, object> modifiedProps = null)
		{
			var entity = ((ITransactionalEntity)entry.Entity);

			return new AuditEventArgs(
				entityState,
				entry.Entity.GetType().Name[0..],
				entity.Id,
				modifiedProps ?? ToModifiedPropertyBag(entry)
				);
		}

		public Dictionary<EntityState, List<EntityEntry>> Merge(Dictionary<EntityState, List<EntityEntry>> principal, Dictionary<EntityState, List<EntityEntry>> dependent)
		{
			if (dependent.IsNullOrEmpty()) return principal;

			foreach (var key in dependent.Keys)
			{
				if (principal.TryGetValue(key, out var entries))
				{
					entries.AddRange(dependent[key]);
				}
				else
				{
					principal.TryAdd(key, dependent[key]);
				}
			}

			return principal;
		}

		public Dictionary<string, object> ToModifiedPropertyBag(EntityEntry entry)
		{
			var modificationBag = new Dictionary<string, object>();

			if (entry.State == EntityState.Modified)
			{
				var modifiedProps = (from prop in entry.Properties
									 where prop.IsModified && 
										   !prop.Metadata.Name.IsEqualTo(nameof(TransactionalEntity.RowVersion))
									 select prop
									).ToList();

				foreach (var prop in modifiedProps)
				{
					modificationBag.TryAdd(prop.Metadata.Name, new ModifiedData(prop.OriginalValue, prop.CurrentValue));
				}
			}
			else
			{
				var properties = (from prop in entry.Properties
								  where !prop.Metadata.Name.IsEqualToIgnoreCase(nameof(TransactionalEntity.RowVersion))
								  select prop);

				foreach (var prop in properties)
				{
					modificationBag.Add(prop.Metadata.Name, prop.CurrentValue);
				}
			}

			return modificationBag;
		}
	}
}
