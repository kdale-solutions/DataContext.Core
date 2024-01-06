using Global.Utilities;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata;
using System.Collections.Frozen;

namespace DataContext.Core.Context.Events
{
	public class EntityEntryClone : EntityEntry
	{
		public new object Entity { get; init; }
		public new EntityState State { get; init; }
		public new IEntityType Metadata { get; init; }
		public new FrozenSet<PropertyEntry> Properties { get; init; }

		public EntityEntryClone(EntityEntry entry) : base(null)
		{
			this.Entity = entry.Entity;
			this.State = entry.State;
			this.Metadata = entry.Metadata;
			this.Properties = new List<PropertyEntry>(entry.Properties.Each(x => new PropertyEntryClone(x))).ToFrozenSet();
		}
	}

	public class PropertyEntryClone
	{
		public bool IsModified { get; init; }
		public object OriginalValue { get; init; }
		public object CurrentValue { get; init; }
		public IProperty Metadata { get; init; }

		public PropertyEntryClone(PropertyEntry propertyEntry)
		{
			this.IsModified = propertyEntry.IsModified;
			this.OriginalValue = propertyEntry.OriginalValue;
			this.CurrentValue = propertyEntry.CurrentValue;
			this.Metadata = propertyEntry.Metadata;
		}
	}

	public static class EntityEntryCloneMapper
	{
		public static FrozenSet<EntityEntryClone> Clone(this IEnumerable<EntityEntry> entries)
		{
			var cloned = new List<EntityEntryClone>();

			foreach (var entry in entries)
			{
				cloned.Add(new EntityEntryClone(entry));
			}

			return cloned.ToFrozenSet();
		}
	}
}
