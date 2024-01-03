namespace DataContext.Core
{
	[AttributeUsage(AttributeTargets.Assembly)]
    public class DataModelAttribute : Attribute 
	{ 
		public string DataContextId { get; }

		public DataModelAttribute(string dataContextId) 
		{
			DataContextId = dataContextId;
		}
	}
}
