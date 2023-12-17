namespace DataContext.Core.Interfaces.Entity
{
	public interface IBaseEntity<K> : IDataTableParameter where K : struct
    {
		K Id { get; set; }
    }
}
