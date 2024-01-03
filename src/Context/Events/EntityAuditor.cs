using DataContext.Core.Context;
using DataContext.Core.Enums;
using DataContext.Core.Interfaces.Entity;
using DiagnosticSuite.Logging;
using Global.Utilities;
using Infrastructure.Diagnostics.Audit;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Collections.Concurrent;
using System.Net.Http.Json;
using System.Text.Json.Serialization;

namespace DataContext.Core.Events.EventArgs
{
    public class EntityAuditor : IEntityAuditor
	{
		private const string _centralContextAddress = "https://localhost:7007/central-context/";
		private const string _centralAuditAddress = "audit";

		private static HttpClient _httpClient;

		private static EntityState[] _auditableEntityStates;

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

			if (entryMap.TryGetValue(EntityState.Modified, out var entries))
			{
				foreach (var entry in entries)
				{
					var propertyBag = entry.GetModificationBag();

					if (!propertyBag.IsNullOrEmpty())
					{
						EntityAuditorUtility.ModificationBags.TryAdd($"{entry.Metadata.Name}_{((ITransactionalEntity)entry.Entity).Id}", propertyBag);
					}
				}
			}

			return entryMap;
		}

		public async Task<string> PerformAudit(Dictionary<EntityState, List<EntityEntry>> entries, CancellationToken cancellationToken = default)
		{
			if (entries.IsNullOrEmpty()) return null;

			var responseMessage = string.Empty;
			HttpResponseMessage res = null;

			try
			{
				var requestBody = entries.ToAuditEventArgs(DateTime.UtcNow);

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

			return responseMessage;
		}
	}
}
