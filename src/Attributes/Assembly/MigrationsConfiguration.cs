namespace DataContext.Core
{
	[AttributeUsage(AttributeTargets.Assembly)]
    public class MigrationsConfigurationAttribute : Attribute 
	{
		public MigrationsConfigurationAttribute() { }
	}
}
