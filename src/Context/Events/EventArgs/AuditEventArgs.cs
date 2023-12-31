using DataContext.Core.Enums;
using System.Text.Json.Serialization;

namespace DataContext.Core.Events.EventArgs
{
    public class AuditEventArgs
    {
        public EntityState EntityState { get; init; }
        public string EntityName { get; init; }
        public int EntityId { get; init; }

        public Dictionary<string, object> ModificationBag { get; init; } = null;

        public DateTime Timestamp { get; init; }

        public AuditEventArgs() { }

        [JsonConstructor]
        public AuditEventArgs(EntityState entityState, DateTime timestamp, string entityName, int entityId, Dictionary<string, object> modificationBag)
        {
            EntityState = entityState;
            Timestamp = timestamp;
            EntityName = entityName;
            EntityId = entityId;
            ModificationBag = modificationBag;
        }
    }
}
