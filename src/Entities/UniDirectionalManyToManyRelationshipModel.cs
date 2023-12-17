namespace DataContext.Core.Entities
{
    public abstract class UniDirectionalManyToManyRelationshipModel<TLeftEntity, TRightEntity> : JoinEntity<TLeftEntity, TRightEntity>
			where TLeftEntity : TransactionalEntity
			where TRightEntity : TransactionalEntity
	{ }
}
