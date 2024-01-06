using System.Text.Json.Serialization;

namespace DataContext.Core.Events.EventArgs
{
	public class AuditEventArgs
    {
        public EntityState EntityState { get; init; }
        public string EntityName { get; init; }
        public int EntityId { get; set; }

        public Dictionary<string, object> ModificationBag { get; set; }

        public DateTime Timestamp { get; init; }

        [JsonIgnore]
        public Guid? TempId { get; set; }

        public AuditEventArgs() { }

        [JsonConstructor]
        public AuditEventArgs(EntityState entityState, string entityName, int entityId, Dictionary<string, object> modificationBag)
        {
            EntityState = entityState;
            EntityId = entityId;
            EntityName = entityName;
            Timestamp = DateTime.UtcNow;
            ModificationBag = new Dictionary<string, object>(modificationBag, StringComparer.OrdinalIgnoreCase);
        }
    }
}
