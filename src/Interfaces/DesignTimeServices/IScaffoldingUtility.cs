using Microsoft.EntityFrameworkCore.Metadata;

namespace DataContext.Core.Interfaces.DesignTimeServices
{
	public interface IScaffoldingUtility
	{
		string GetEntityTypeBaseClass(IEntityType entityType, List<IProperty> modelProps);
		string GetEntityTypeConfigurationBaseClass(IEntityType entityType);
		bool IsBaseEntity(IEntityType entityType);
		bool IsTransactionalEntity(IEntityType entityType);
		bool IsReferenceEntity(IEntityType entityType);
		bool IsManyToManyEntity(IEntityType entityType);
	}
}
