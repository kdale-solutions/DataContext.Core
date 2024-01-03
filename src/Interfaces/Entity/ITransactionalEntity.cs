namespace DataContext.Core.Interfaces.Entity
{
	public interface ITransactionalEntity : IEntity
	{
		Guid? TempId { get; set; }

		Dictionary<string, object> ModificationBag { get; set; }
		void RecordModification(string propertyName, object oldVal, object newVal);
    }
}
