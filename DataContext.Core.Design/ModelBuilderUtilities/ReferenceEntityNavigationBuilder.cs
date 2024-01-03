using DataContext.Core.Interfaces.Entity;
using DataContext.Core.Configuration.Constants;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Linq.Expressions;

namespace DataContext.Core.Configuration.ModelBuilderUtilities
{
	public static class ReferenceEntityNavigationBuilder
	{
		public static EntityTypeBuilder<TEntity> HasOptionalReferenceEntity<TEntity, TReferenceEntity, K>(this EntityTypeBuilder<TEntity> builder, Expression<Func<TEntity, TReferenceEntity>> navigationExp, Expression<Func<TEntity, K?>> propertySelector, bool fkIsSparse = false)
			where TEntity : class
			where TReferenceEntity : class, IReferenceEntity<K>
			where K : struct, IConvertible
		{
			builder.Property(propertySelector)
				.HasColumnType(DataConstants.ColumnType.Tinyint)
				.WithValueConversion()
				.IsRequired(false)
				.IsSparse(fkIsSparse);

			builder.HasOne(navigationExp)
				.WithMany()
				.HasForeignKey($"{typeof(TReferenceEntity).Name}Id")
				.OnDelete(DeleteBehavior.NoAction)
				.IsRequired(false);

			return builder;
		}

        public static EntityTypeBuilder<TEntity> HasRequiredReferenceEntity<TEntity, TReferenceEntity, K>(this EntityTypeBuilder<TEntity> builder, Expression<Func<TEntity, TReferenceEntity>> navigationExp, Expression<Func<TEntity, K>> propertySelector)
            where TEntity : class
            where TReferenceEntity : class, IReferenceEntity<K>
			where K : struct, IConvertible
		{
			builder.Property(propertySelector)
				.HasColumnType(DataConstants.ColumnType.Tinyint)
				.WithValueConversion()
				.IsRequired();

			builder.HasOne(navigationExp)
                .WithMany()
				.HasForeignKey($"{typeof(TReferenceEntity).Name}Id")
				.OnDelete(DeleteBehavior.NoAction)
				.IsRequired();

			return builder;
		}
	}
}
