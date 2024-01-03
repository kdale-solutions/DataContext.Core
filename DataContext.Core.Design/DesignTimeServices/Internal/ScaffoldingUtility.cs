using DataContext.Core.Interfaces.DesignTimeServices;
using Microsoft.EntityFrameworkCore.Metadata;

namespace DataContext.Core.DesignTimeServices
{
	public class ScaffoldingUtility : IScaffoldingUtility
    {
        private readonly HashSet<string> _baseEntityProps;
        private readonly HashSet<string> _txEntityProps;
        private readonly HashSet<string> _refEntityProps;
        private readonly HashSet<string> _manyToManyProps;
        private readonly HashSet<string> _manyToManyNavProps;

        private HashSet<string> _entityPropNames;

        public ScaffoldingUtility() 
        {
            _baseEntityProps = new HashSet<string> { "Id" };
            _txEntityProps = new HashSet<string> { "Id", "RowVersion" };
            _refEntityProps = new HashSet<string> { "Id", "Code", "Name" };
            _manyToManyProps = new HashSet<string> { "Id", "LeftEntityId", "RightEntityId" };
            _manyToManyNavProps = new HashSet<string> { "LeftEntity", "RightEntity" };
        }

        public string GetEntityTypeBaseClass(IEntityType entityType, List<IProperty> modelProps)
        {
            var baseClassName = string.Empty;

            var entityProps = entityType.GetProperties().OrderBy(p => p.GetColumnOrder() ?? -1).ToList();

            _entityPropNames = entityProps.Select(x => x.Name).ToHashSet();

            if (IsTransactionalEntity(entityType))
            {
                baseClassName = $"TransactionalEntity";

                modelProps.AddRange(entityProps.ExceptBy(_txEntityProps, x => x.Name));
            }
            else if (IsReferenceEntity(entityType))
            {
                baseClassName = $"ReferenceEntity<KeyConstraintPlaceholder>";

                modelProps.AddRange(entityProps.ExceptBy(_refEntityProps, x => x.Name));
            }
            else if (IsManyToManyEntity(entityType))
            {
                var leftEntityNav = entityType.GetNavigations().Where(x => x.Name.Equals("LeftEntity")).FirstOrDefault();
                var rightEntityNav = entityType.GetNavigations().Where(x => x.Name.Equals("RightEntity")).FirstOrDefault();

                baseClassName = $"ManyToManyRelationshipModel<{leftEntityNav.TargetEntityType.Name}, {rightEntityNav.TargetEntityType.Name}>";

                modelProps.AddRange(entityProps.ExceptBy(_manyToManyProps, x => x.Name));
            }
            else if (IsBaseEntity(entityType))
            {
                var primaryKey = entityType.FindPrimaryKey();

                if (primaryKey.Properties.Count() == 1)
                {
                    var keyProp = primaryKey.Properties.First();
                    var propertyTypeName = keyProp.ClrType.Name;

                    if (propertyTypeName == "Int16")
                    {
                        propertyTypeName = "short";
                    }
                    else if (propertyTypeName == "Int32")
                    {
                        propertyTypeName = "int";
                    }
                    else if (propertyTypeName == "Int64")
                    {
                        propertyTypeName = "long";
                    }

                    baseClassName = keyProp.ValueGenerated == ValueGenerated.Never ?
                        $"BaseReferenceEntity<{propertyTypeName.ToLower()}>" :
                        $"BaseEntity<{propertyTypeName.ToLower()}>";

                    modelProps.AddRange(entityProps.ExceptBy(_baseEntityProps, x => x.Name));
                }
            }
            else
            {
				modelProps.AddRange(entityProps);
            }

            return baseClassName;
        }

        public string GetEntityTypeConfigurationBaseClass(IEntityType entityType)
        {
            var entityProps = entityType.GetProperties().OrderBy(p => p.GetColumnOrder() ?? -1).ToList();

            _entityPropNames = entityProps.Select(x => x.Name).ToHashSet();

            if (IsTransactionalEntity(entityType))
            {
                return $"TransactionalEntityConfiguration<{entityType.Name}>";
            }
            else if (IsReferenceEntity(entityType))
            {
				return $"ReferenceEntityConfiguration<{entityType.Name}, {entityType.Name}Id>";
            }
            else if (IsManyToManyEntity(entityType))
            {
                var leftEntityNav = entityType.GetNavigations().Where(x => x.Name.Equals("LeftEntity")).FirstOrDefault();
                var rightEntityNav = entityType.GetNavigations().Where(x => x.Name.Equals("RightEntity")).FirstOrDefault();

				return $"ManyToManyRelationshipConfiguration<{entityType.Name}, {leftEntityNav.TargetEntityType.Name}, {rightEntityNav.TargetEntityType.Name}>";
            }
            else if (IsBaseEntity(entityType))
            {
                var primaryKey = entityType.FindPrimaryKey();

                if (primaryKey.Properties.Count() == 1)
                {
                    var keyProp = primaryKey.Properties.First();
                    var propertyTypeName = keyProp.ClrType.Name;

                    if (propertyTypeName == "Int16")
                    {
                        propertyTypeName = "short";
                    }
                    else if (propertyTypeName == "Int32")
                    {
                        propertyTypeName = "int";
                    }
                    else if (propertyTypeName == "Int64")
                    {
                        propertyTypeName = "long";
                    }

                    return keyProp.ValueGenerated == ValueGenerated.Never ?
                        $"BaseReferenceEntityConfiguration<{entityType.Name}, {propertyTypeName.ToLower()}>" :
                        $"BaseEntityConfiguration<{entityType.Name}, {propertyTypeName.ToLower()}>";
                }
			}

			return string.Empty;
		}

        public bool IsBaseEntity(IEntityType entityType) => !_baseEntityProps.Except(_entityPropNames).Any();
        public bool IsTransactionalEntity(IEntityType entityType) => !_txEntityProps.Except(_entityPropNames).Any();
        public bool IsReferenceEntity(IEntityType entityType) => !_refEntityProps.Except(_entityPropNames).Any();
        public bool IsManyToManyEntity(IEntityType entityType) => !_manyToManyProps.Except(_entityPropNames).Any();
    }
}
