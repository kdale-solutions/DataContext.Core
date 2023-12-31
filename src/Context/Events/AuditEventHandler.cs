using DataContext.Core.Context;
using DataContext.Core.Events.EventArgs;
using DataContext.Core.Interfaces.Entity;
using DiagnosticSuite.Logging;
using Global.Utilities;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata;
using System.Collections.Concurrent;
using System.Net.Http.Json;

namespace Infrastructure.Diagnostics.Audit
{
	public static class AuditEventHandler
	{
		private const string _centralContextAddress = "https://localhost:7007/central-context/";
		private const string _centralAuditAddress = "audit";

        private static HttpClient _httpClient;

        static AuditEventHandler()
        {
            _httpClient = new HttpClient
			{
				BaseAddress = new Uri(_centralContextAddress)
			};
		}

        public static async Task<string> HandleAuditEvent<TContext>(this BaseDataContext<TContext> context, IEnumerable<EntityEntry> entries, CancellationToken cancellationToken = default)
			where TContext : DbContext
        {
            var str = string.Empty;
            HttpResponseMessage res = null;

            try
            {
                var requestBody = entries.ToAuditEventArgs(DateTime.UtcNow, context.ModificationBags);

                using HttpResponseMessage response = await _httpClient.PostAsJsonAsync(
                    _centralAuditAddress,
					requestBody, 
                    InternalJsonSerializerOptions.Default,
                    cancellationToken
                    );

                res = response;

                response.EnsureSuccessStatusCode();

                str = await response.Content.ReadAsStringAsync();
            }
            catch (Exception ex)
            {
                str = ex.Message;

                UtilityLogger<HttpClient>.Error($"Error reaching {_centralAuditAddress}", ex);
            }

            return str;
		}

		public static IEnumerable<AuditEventArgs> ToAuditEventArgs(this IEnumerable<EntityEntry> entries, DateTime timestamp, ConcurrentDictionary<IEntityType, Dictionary<string, object>> modificationBags)
		{
			return entries.Select(entry => entry.ToAuditEventArgs(timestamp, modificationBags.ContainsKey(entry.Metadata) ? modificationBags[entry.Metadata] : null));
		}

		public static AuditEventArgs ToAuditEventArgs(this EntityEntry entry, DateTime timestamp, Dictionary<string, object> modificationBag)
		{
			var entity = ((ITransactionalEntity)entry.Entity);

			return new AuditEventArgs(
				entry.State,
				timestamp,
				entry.Entity.GetType().Name,
				entity.Id,
				modificationBag
				);
		}
	}
}
