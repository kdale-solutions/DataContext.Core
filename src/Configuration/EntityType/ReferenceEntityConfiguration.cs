using DataContext.Core.Configuration.Constants;
using DataContext.Core.Configuration.ModelBuilderUtilities;
using DataContext.Core.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataContext.Core.Configuration.EntityType
{
	public class ReferenceEntityConfiguration<TEntity, K> : 
        BaseReferenceEntityConfiguration<TEntity, K>, 
        IEntityTypeConfiguration<TEntity>
		    where TEntity : ReferenceEntity<K>
			where K : struct
	{
        public new void Configure(EntityTypeBuilder<TEntity> builder)
		{
			builder.Property(x => x.Id)
				.HasColumnType(DataConstants.ColumnType.Tinyint)
				.WithValueConversion();

			base.Configure(builder);

			builder.Property(x => x.Name)
                .HasMaxLength(DataConstants.MaxLength.StandardName);

            builder.Property(x => x.Code)
                .HasMaxLength(DataConstants.MaxLength.ReferenceEntityCode)
				.IsRequired();

            builder.HasIndex(x => x.Code)
                .IsUnique();
        }
    }
}
