using DataContext.Core.Configuration.Constants;
using DataContext.Core.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataContext.Core.Configuration.EntityType
{
	public class TransactionalEntityConfiguration<TEntity> : 
        BaseEntityConfiguration<TEntity, int>, 
        IEntityTypeConfiguration<TEntity>
		    where TEntity : TransactionalEntity
	{
		public new void Configure(EntityTypeBuilder<TEntity> builder)
		{
			builder.Ignore(x => x.ModificationBag);

			builder.Property(x => x.RowVersion)
				.HasColumnType(DataConstants.ColumnType.Rowversion);

			builder.Property(x => x.RowVersion)
				.IsRowVersion()
				.IsRequired();

			builder.Property(x => x.Id)
				.UseIdentityColumn();

			builder.HasKey(x => x.Id)
				.IsClustered(true);

			base.Configure(builder);
		}
    }
}
