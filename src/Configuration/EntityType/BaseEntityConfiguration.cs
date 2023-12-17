using DataContext.Core.Configuration.Constants;
using DataContext.Core.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataContext.Core.Configuration.EntityType
{
	public partial class BaseEntityConfiguration<TEntity, TKey> : IEntityTypeConfiguration<TEntity>
        where TEntity : BaseEntity<TKey>
        where TKey : struct
    {
		public void Configure(EntityTypeBuilder<TEntity> builder)
		{
			builder.HasKey(x => x.Id)
                .IsClustered(false);

            builder.Property(x => x.Id)
                .HasColumnOrder(DataConstants.ColumnOrder.First);

            builder.ToTable(typeof(TEntity).Name);
        }
    }
}
