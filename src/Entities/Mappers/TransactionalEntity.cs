using DataContext.Core.ValueTypes;
using Global.Utilities;

namespace DataContext.Core.Entities
{
	public abstract partial class TransactionalEntity
	{
		public void RecordModification(string propertyName, object oldVal, object newVal)
		{
			if (this.ModificationBag.IsNullOrEmpty())
			{
				this.ModificationBag = new Dictionary<string, object>();
			}

			this.ModificationBag.TryAdd(propertyName, new ModifiedData(oldVal, newVal));
		}
	}
}
