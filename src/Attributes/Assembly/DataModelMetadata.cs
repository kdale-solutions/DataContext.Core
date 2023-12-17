using DataContext.Core.Enums;

namespace DataContext.Core
{
	[AttributeUsage(AttributeTargets.Assembly)]
    public class DataModelMetadataAttribute : Attribute 
	{
		public DataProvider Provider { get; init; }

		public DataModelMetadataAttribute() 
		{
			Provider = DataProvider.SqlServer;
		}

		public DataModelMetadataAttribute(DataProvider provider)
		{
			Provider = provider;
		}
	}
}
