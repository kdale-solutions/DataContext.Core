namespace DataContext.Core.Attributes
{
	// Preprended with "Is" to avoid confilcts with frameworks.
	[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
    public class IsUnicodeAttribute : Attribute { }
}
