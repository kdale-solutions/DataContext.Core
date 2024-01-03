namespace DataContext.Core.Attributes
{
	[AttributeUsage(AttributeTargets.Property)]
    public class NestedEntityModelAttribute : Attribute
    {
        private string _entityType;

        public string EntityType
        {
            get => _entityType;
            private set => _entityType = value;
        }

        public NestedEntityModelAttribute(string entityName)
        {
            _entityType = entityName;
        }
    }
}
