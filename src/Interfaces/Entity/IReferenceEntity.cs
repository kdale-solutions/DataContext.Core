namespace DataContext.Core.Interfaces.Entity
{
    public interface IReferenceEntity<K> : IBaseEntity<K> where K : struct
	{
		string Code { get; set; }
		string Name { get; set; }
	}
}
