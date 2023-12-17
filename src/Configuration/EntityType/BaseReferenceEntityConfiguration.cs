using DataContext.Core.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataContext.Core.Configuration.EntityType
{
	public class BaseReferenceEntityConfiguration<TEntity, TKey> : 
		BaseEntityConfiguration<TEntity, TKey>, 
		IEntityTypeConfiguration<TEntity>
			where TEntity : BaseReferenceEntity<TKey>
			where TKey : struct
	{
        public new void Configure(EntityTypeBuilder<TEntity> builder)
		{
			builder.Property(x => x.Id)
				.ValueGeneratedNever();

			base.Configure(builder);
        }
    }
}
