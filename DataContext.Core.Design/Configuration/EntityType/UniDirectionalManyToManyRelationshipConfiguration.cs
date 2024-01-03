using DataContext.Core.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataContext.Core.Configuration.EntityType
{
	public class UniDirectionalManyToManyRelationshipConfiguration<TJoin, TLeft, TRight> : 
		JoinEntityConfiguration<TJoin, TLeft, TRight>, 
		IEntityTypeConfiguration<TJoin>
			where TJoin : UniDirectionalManyToManyRelationshipModel<TLeft, TRight>
			where TLeft : TransactionalEntity
			where TRight : TransactionalEntity
	{
        public new void Configure(EntityTypeBuilder<TJoin> builder) 
		{
			base.Configure(builder);

			builder.HasIndex(x => x.LeftEntityId);
		}
    }
}
