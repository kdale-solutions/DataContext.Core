using DataContext.Core.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataContext.Core.Configuration.EntityType
{
	public class ManyToManyRelationshipConfiguration<TJoin, TLeft, TRight> : 
		JoinEntityConfiguration<TJoin, TLeft, TRight>, 
		IEntityTypeConfiguration<TJoin>
			where TJoin : ManyToManyRelationshipModel<TLeft, TRight>
			where TLeft : TransactionalEntity
			where TRight : TransactionalEntity
	{
        public new void Configure(EntityTypeBuilder<TJoin> builder)
		{
			base.Configure(builder);
        }
    }
}
