using DataContext.Core.Configuration.Constants;
using DataContext.Core.Configuration.ValueComparers;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System.Linq.Expressions;

namespace DataContext.Core.Configuration.ModelBuilderUtilities
{
	public static class EnumPropertyTypeBuilder
	{
		public static PropertyBuilder<TEnum> HasEnumPropertyType<TEntity, TEnum>(this EntityTypeBuilder<TEntity> builder, Expression<Func<TEntity, TEnum>> propertySelector, bool isRequired = true)
            where TEntity : class
            where TEnum : struct, Enum, IConvertible
        {
            return builder.Property(propertySelector)
                .HasColumnType(DataConstants.ColumnType.Tinyint)
                .HasConversion<EnumToNumberConverter<TEnum, byte>, EnumValueComparer<TEnum>>()
                .IsRequired(isRequired);
        }
	}
}
