using DataContext.Core.Attributes;
using Global.Utilities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Reflection;

namespace DataContext.Core.Configuration.ModelBuilderUtilities
{
	public static class KeyConstraintValueConversionUtility
	{
		private static readonly string _nullableTypeName = typeof(Nullable<>).Name;

		public static PropertyBuilder WithValueConversion(this PropertyBuilder propertyBuilder)
		{
			string typeName;

			if (propertyBuilder.Metadata.ClrType.Name.IsEqualToIgnoreCase(_nullableTypeName))
			{
				typeName = propertyBuilder.Metadata.ClrType.GetGenericArguments().SingleOrDefault()?.Name;
			}
			else
			{
				typeName = propertyBuilder.Metadata.ClrType.Name;
			}

			return propertyBuilder.GetValueConversion(typeName);
		}

		public static PropertyBuilder<K> WithValueConversion<K>(this PropertyBuilder<K> propertyBuilder)
			where K : struct
		{
			propertyBuilder.GetValueConversion(typeof(K).Name);

			return propertyBuilder;
		}

		private static PropertyBuilder GetValueConversion(this PropertyBuilder propertyBuilder, string typeName)
		{
			var metadataAssembly = AppDomain.CurrentDomain.GetAssemblies().FirstOrDefault(x => x.GetCustomAttribute<DataModelConfigurationAttribute>() != null);

			if (metadataAssembly == null) throw new DllNotFoundException($"Unable to locate assembly with {nameof(DataModelConfigurationAttribute)} in AppDomain.CurrentDomain.BaseDirectory {AppDomain.CurrentDomain.BaseDirectory}.");

			var methodInfo = (from dType in metadataAssembly.DefinedTypes
							  from dMethod in dType.GetMethods()
							  where dMethod.GetCustomAttribute<KeyConstraintValueConversionFactoryAttribute>() != null
							  select dMethod)
							 .FirstOrDefault();

			if (methodInfo == null) throw new MissingMethodException($"Unable to locate method with {nameof(KeyConstraintValueConversionFactoryAttribute)} within assembly {metadataAssembly.FullName}.");

			var builderWithChainedValueConversion = (PropertyBuilder)methodInfo.Invoke(null, new object[2] { propertyBuilder, typeName });

			return builderWithChainedValueConversion;
		}
	}
}
