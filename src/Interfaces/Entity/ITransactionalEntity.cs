namespace DataContext.Core.Interfaces.Entity
{
	public interface ITransactionalEntity : IEntity
	{
		Dictionary<string, object> ModificationBag { get; set; }
    }
}
