using DataContext.Core.Interfaces.Entity;

namespace DataContext.Core.Entities
{
	public abstract partial class ReferenceEntity<K> : BaseReferenceEntity<K>, IReferenceEntity<K>
		where K : struct, IConvertible
	{
		public string Code { get; set; }
		public string Name { get; set; }
    }
}
