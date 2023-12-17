namespace DataContext.Core.Entities
{
    public abstract class JoinEntity<TLeftEntity, TRightEntity> : BaseEntity<int> 
		where TLeftEntity : TransactionalEntity
		where TRightEntity : TransactionalEntity
	{
		public int LeftEntityId { get; set; }
		public abstract TLeftEntity LeftEntity { get; init; }

		public int RightEntityId { get; set; }
		public abstract TRightEntity RightEntity { get; init; }
	}
}
