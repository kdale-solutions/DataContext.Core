using DataContext.Core.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataContext.Core.Configuration.EntityType
{
	public class JoinEntityConfiguration<TJoin, TLeft, TRight> : 
		BaseEntityConfiguration<TJoin, int>, 
		IEntityTypeConfiguration<TJoin>
			where TJoin : JoinEntity<TLeft, TRight>
			where TLeft : TransactionalEntity
			where TRight : TransactionalEntity
	{
        public new void Configure(EntityTypeBuilder<TJoin> builder)
		{
			builder.Property(x => x.Id)
				.UseIdentityColumn();

			base.Configure(builder);

			builder.HasKey(x => x.Id);

			builder.Property(x => x.LeftEntityId)
                .IsRequired();

			builder.Property(x => x.RightEntityId)
				.IsRequired();
		}
	}
}
