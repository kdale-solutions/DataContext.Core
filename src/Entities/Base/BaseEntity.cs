using DataContext.Core.Interfaces.Entity;

namespace DataContext.Core.Entities
{
	public abstract class BaseEntity<K> : IBaseEntity<K> where K : struct
	{
		public K Id { get; set; }
	}
}
