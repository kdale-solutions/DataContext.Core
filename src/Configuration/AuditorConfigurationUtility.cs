using DataContext.Core.Events.EventArgs;
using DataContext.Core.Interfaces.Entity;
using Microsoft.Extensions.DependencyInjection;

namespace DataContext.Core.Configuration
{
	public static class AuditorConfigurationUtility
	{
		public static IServiceCollection AddEntityAuditor(this IServiceCollection services)
		{
			return services.AddScoped<IEntityAuditor, EntityAuditor>();
		}
	}
}
