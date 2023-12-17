namespace DataContext.Core
{
	[AttributeUsage(AttributeTargets.Assembly)]
    public class DataModelConfigurationAttribute : Attribute 
	{
		public DataModelConfigurationAttribute() { }
	}
}
