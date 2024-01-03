using System.Reflection;

#nullable enable
#pragma warning disable CS8600, CS8601, CS8602, CS8603, CS8604, CS8618, CS8629
namespace DataContext.Core.Configuration.EqualityComparers
{
	public class PropertyInfoEqualityComparer : IEqualityComparer<PropertyInfo>
	{
		public bool Equals(PropertyInfo? x, PropertyInfo? y)
		{
			return x == y;
		}

		public int GetHashCode(PropertyInfo x)
		{
			return x.GetHashCode();
		}
	}
}
