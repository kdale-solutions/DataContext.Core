using DataContext.Core.Interfaces.DesignTimeServices;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.DependencyInjection;

[assembly: DesignTimeServicesReference("DataContext.Core.DesignTimeServices.Internal.DesignTimeServicesExtension")]
namespace DataContext.Core.DesignTimeServices.Internal
{
	public class DesignTimeServicesExtension : IDesignTimeServices
	{
		public virtual void ConfigureDesignTimeServices(IServiceCollection serviceCollection)
		{
			serviceCollection.AddScoped<IScaffoldingUtility, ScaffoldingUtility>();
		}
	}
}
