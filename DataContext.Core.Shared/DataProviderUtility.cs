using DataContext.Core.Enums;
using Global.Configuration;
using Microsoft.Extensions.Configuration;

namespace DataContext.Core.Configuration
{
	public static class DataProviderUtility
	{
		public static string GetConnectionString(DataProvider databaseProvider)
		{
			return HostUtility.Configuration.GetConnectionString(databaseProvider.ToString());
		}
	}
}
