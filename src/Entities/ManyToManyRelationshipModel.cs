namespace DataContext.Core.Entities
{
	public abstract class ManyToManyRelationshipModel<TLeftEntity, TRightEntity> : JoinEntity<TLeftEntity, TRightEntity> 
        where TLeftEntity : TransactionalEntity
		where TRightEntity : TransactionalEntity
	{ }
}
